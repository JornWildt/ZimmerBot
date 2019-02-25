using System;
using System.Collections.Generic;
using log4net;
using VDS.RDF;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Pipeline.InputStages;

namespace ZimmerBot.Core.Knowledge
{
  public static class BotUtility
  {
    public static readonly ILog EvaluationLogger = LogManager.GetLogger("EvaluationLogger");

    public static readonly ILog DiaLogger = LogManager.GetLogger("DialogLogger");

    public static readonly ILog Logger = LogManager.GetLogger(typeof(BotUtility));

    static INodeFactory NodeFactory = new NodeFactory();


    public static Response Invoke(KnowledgeBase kb, Request request)
    {
      RequestContext context = BuildRequestContext(kb, request);
      return Invoke(context, request, false);
    }


    public static RequestContext BuildRequestContext(KnowledgeBase kb, Request request)
    {
      RequestState state = new RequestState();

      // Add session store
      Session session = SessionManager.GetOrCreateSession(request.SessionId, request.UserId);
      state[StateKeys.SessionStore] = session.Store;

      // Add user store
      var userStore = new RDFDictionaryWrapper(
        kb.MemoryStore,
        UrlConstants.UsersUrl(request.UserId),
        UrlConstants.UserValuesUrl,
        RDFStore.DynamicStoreName);
      state[StateKeys.UserStore] = userStore;

      // Add bot store
      var botStore = new RDFDictionaryWrapper(
        kb.MemoryStore,
        UrlConstants.BotUrl,
        UrlConstants.BotValuesUrl,
        RDFStore.DynamicStoreName);
      state[StateKeys.BotStore] = botStore;

      // Register <user, is-a, user>
      kb.MemoryStore.Update(
        NodeFactory.CreateUriNode(UrlConstants.UsersUrl(request.UserId)),
        NodeFactory.CreateUriNode(UrlConstants.Rdf("type")),
        NodeFactory.CreateUriNode(UrlConstants.UserTypeUrl),
        RDFStore.DynamicStoreName);

      // Register <chat, is-a, chat>
      kb.MemoryStore.Update(
        NodeFactory.CreateUriNode(UrlConstants.ChatsUrl(request.SessionId)),
        NodeFactory.CreateUriNode(UrlConstants.Rdf("type")),
        NodeFactory.CreateUriNode(UrlConstants.ChatTypeUrl),
        RDFStore.DynamicStoreName);

      RequestContext context = new RequestContext(kb, state, session);

      return context;
    }



    internal static Response Invoke(RequestContext context, Request request, bool fromTemplate)
    {
      try
      {
        List<string> output = new List<string>();

        InvokeStatements(context, request, fromTemplate, output);

        if (output.Count > 0)
        {
          context.State[StateKeys.SessionStore][SessionKeys.ResponseCount] = context.State[StateKeys.SessionStore][SessionKeys.ResponseCount] + 1;

          return new Response(output, request.State, context.Session);
        }
        else
        {
          return new Response(new string[0], request.State, context.Session);
        }
      }
      catch (Exception ex)
      {
        Logger.Error(ex);
        throw;
      }
    }


    static internal void InvokeStatements(RequestContext context, Request request, bool fromTemplate, List<string> output)
    {
      if (request.Input != null)
      {
        DiaLogger.InfoFormat($"[{context.Session.SessionId}] > {request.Input}");

        if (context.Session.IsBusyWriting())
          context.Session.RegisterLatestInput(request.Input);
        else
          context.Session.RegisterLatestInput(null);

        ZTokenizer tokenizer = new ZTokenizer();
        ZStatementSequence statements = tokenizer.Tokenize(request.Input);

        // Always evaluate at least one empty statement in order to invoke triggers without regex
        if (statements.Statements.Count == 0)
          statements.Statements.Add(new ZTokenSequence());

        foreach (ZTokenSequence input in statements.Statements)
        {
          InputRequestContext inputContext = new InputRequestContext(context, request, input);
          InvokeWithInput(inputContext, output);
        }
      }
      else
      {
        DiaLogger.InfoFormat($"[{context.Session.SessionId}] Invoke without input");
        InputRequestContext inputContext = new InputRequestContext(context, request, null);
        InvokeWithInput(inputContext, output);
      }
    }


    static internal void InvokeWithInput(InputRequestContext inputContext, List<string> output)
    {
      if (++inputContext.RepetitionCount >= AppSettings.MaxRecursionCount)
        throw new RepetitionException($"Stopping repeated evaluation of {inputContext.RepetitionCount} tries.");

      BotUtility.EvaluationLogger.Debug($"Invoking with input: {inputContext.Request.Input}");

      var pipelineItem = new InputPipelineItem(inputContext);
      inputContext.KnowledgeBase.InputPipeline.Invoke(pipelineItem);
      output.AddRange(pipelineItem.Output);

      if (inputContext.ContinuationChoice != null)
      {
        if (inputContext.ContinuationChoice.ContinuationType == Continuation.ContinuationEnum.Answer)
        {
          // Set last-rule-id as the rule of the answer
          inputContext.State[StateKeys.SessionStore][SessionKeys.LastRuleId]
            = inputContext.KnowledgeBase.GetRuleFromLabel(inputContext.ContinuationChoice.Text).Id;
        }
        else
        {
          Request request = (inputContext.ContinuationChoice.ContinuationType == Continuation.ContinuationEnum.Label
            ? new Request(inputContext.Request, null) { RuleLabel = inputContext.ContinuationChoice.Text }
            : new Request(inputContext.Request, inputContext.ContinuationChoice.Text));

          request.BotId = inputContext.Request.BotId;

          InvokeStatements(
            inputContext.RequestContext,
            request,
            inputContext.FromTemplate,
            output);
        }
      }
    }
  }
}

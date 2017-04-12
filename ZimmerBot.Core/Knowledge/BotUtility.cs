using System.Collections.Generic;
using log4net;
using VDS.RDF;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Pipeline.InputStages;

namespace ZimmerBot.Core.Knowledge
{
  public static class BotUtility
  {
    static ILog DiaLogger = LogManager.GetLogger("DialogLogger");

    static INodeFactory NodeFactory = new NodeFactory();


    public static Response Invoke(KnowledgeBase kb, Request request, bool executeScheduledRules = false)
    {
      RequestState state = new RequestState();

      // Add session store
      Session session = SessionManager.GetOrCreateSession(request.SessionId);
      state[StateKeys.SessionStore] = session.Store;

      // Add user store
      var userStore = new RDFDictionaryWrapper(kb.MemoryStore, UrlConstants.UsersUrl(request.UserId), UrlConstants.UserValuesUrl);
      state[StateKeys.UserStore] = userStore;

      // Add bot store
      var botStore = new RDFDictionaryWrapper(kb.MemoryStore, UrlConstants.BotUrl, UrlConstants.BotValuesUrl);
      state[StateKeys.BotStore] = botStore;

      // Register <user, is-a, user>
      kb.MemoryStore.Update(
        NodeFactory.CreateUriNode(UrlConstants.UsersUrl(request.UserId)),
        NodeFactory.CreateUriNode(UrlConstants.Rdf("type")),
        NodeFactory.CreateUriNode(UrlConstants.UserTypeUrl));

      // Register <chat, is-a, chat>
      kb.MemoryStore.Update(
        NodeFactory.CreateUriNode(UrlConstants.ChatsUrl(request.SessionId)),
        NodeFactory.CreateUriNode(UrlConstants.Rdf("type")),
        NodeFactory.CreateUriNode(UrlConstants.ChatTypeUrl));

      RequestContext context = new RequestContext(kb, state, session);

      return Invoke(context, request, executeScheduledRules, false);
    }


    internal static Response Invoke(RequestContext context, Request request, bool executeScheduledRules, bool fromTemplate)
    {
      List<string> output = new List<string>();

      InvokeStatements(context, request, fromTemplate, output);

      if (output.Count > 0)
      {
        context.State[StateKeys.SessionStore][StateKeys.ResponseCount] = context.State[StateKeys.SessionStore][StateKeys.ResponseCount] + 1;

        return new Response(output, request.State);
      }
      else
      {
        return new Response(new string[0], request.State);
      }
    }


    static internal void InvokeStatements(RequestContext context, Request request, bool fromTemplate, List<string> output)
    {
      if (request.Input != null)
      {
        DiaLogger.InfoFormat("Invoke: {0}", request.Input);

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
        DiaLogger.InfoFormat("Invoke without input");
        InputRequestContext inputContext = new InputRequestContext(context, request, null);
        InvokeWithInput(inputContext, output);
      }
    }


    static internal void InvokeWithInput(InputRequestContext inputContext, List<string> output)
    {
      if (++inputContext.RepetitionCount >= AppSettings.MaxRecursionCount)
        throw new RepetitionException($"Stopping repeated evaluation of {inputContext.RepetitionCount} tries.");

      var pipelineItem = new InputPipelineItem(inputContext);
      inputContext.KnowledgeBase.InputPipeline.Invoke(pipelineItem);
      output.AddRange(pipelineItem.Output);

      if (inputContext.ContinuationChoice != null)
      {
        if (inputContext.ContinuationChoice.ContinuationType == Continuation.ContinuationEnum.Answer)
        {
          // Set last-rule-id as the rule of the answer
          inputContext.State[StateKeys.SessionStore][StateKeys.LastRuleId]
            = inputContext.KnowledgeBase.GetRuleFromLabel(inputContext.ContinuationChoice.Text).Id;
        }
        else
        {
          Request request = (inputContext.ContinuationChoice.ContinuationType == Continuation.ContinuationEnum.Label
            ? new Request(inputContext.Request, null) { RuleLabel = inputContext.ContinuationChoice.Text }
            : new Request(inputContext.Request, inputContext.ContinuationChoice.Text));

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

using System;
using System.Collections.Generic;
using System.Linq;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.WordRegex
{
  public abstract class WRegexBase
  {
    public class EvaluationContext
    {
      public ZTokenSequence Input { get; protected set; }

      public int CurrentRepetitionIndex { get; set; }

      public Stack<string> MatchNames { get; protected set; }

      public int StartPosition { get; set; }

      public int EndPosition { get; set; }

      public EvaluationContext(TriggerEvaluationContext context)
      {
        // FIXME: must handle multiple possibilities!
        if (context.InputContext.Input != null)
          Input = context.InputContext.Input[0];
        else
          Input = null;
        CurrentRepetitionIndex = context.CurrentRepetitionIndex;
        MatchNames = context.MatchNames;
        StartPosition = 0;
        EndPosition = 99999;
      }

      public EvaluationContext(ZTokenSequence input, int startPos, int endPos)
      {
        Input = input;
        StartPosition = startPos;
        EndPosition = endPos;
        CurrentRepetitionIndex = 1;
        MatchNames = new Stack<string>();
      }
    }


    public abstract void ExtractWordsForSpellChecker();

    public abstract double CalculateSize();

    public abstract NFAFragment CalculateNFAFragment(EvaluationContext context);


    public NFANode CalculateNFA(EvaluationContext context)
    {
      NFAFragment f = CalculateNFAFragment(context);
      PatchNFAEdges(f.Out, NFANode.MatchNode);

      return f.Start;
    }


    protected void PatchNFAEdges(List<NFAEdge> edges, NFANode n)
    {
      foreach (NFAEdge e in edges)
      {
        if (e.Target == null)
          e.Target = n;
      }
    }


    public MatchResult CalculateNFAMatch(EvaluationContext context)
    {
      NFANode start = CalculateNFA(context);
      return CalculateNFAMatch(start, context);
    }


    public MatchResult CalculateNFAMatch(NFANode start, EvaluationContext context)
    {
      List<NFAMatchNode> clist = new List<NFAMatchNode>();
      List<NFAMatchNode> nlist = new List<NFAMatchNode>();

      AddNode(new NFAMatchNode(start), clist);

      int end = Math.Min(context.Input.Count, context.EndPosition);
      for (int i=context.StartPosition; i<end; ++i)
      {
        var inp = context.Input[i];

        Step(clist, inp, nlist);

        List<NFAMatchNode> tmp = clist;
        clist = nlist;
        nlist = tmp;
      }

      NFAMatchNode matchNode = clist.FirstOrDefault(n => n.Node.Type == NFANode.TypeEnum.Match);
      double score = (matchNode != null ? 1 : 0);

      MatchResult result = new MatchResult(score);

      // Make sure all match groups exists
      for (int i = 1; i < context.CurrentRepetitionIndex; ++i)
        result.Matches[i.ToString()] = new ZTokenWord("");

      if (matchNode != null)
        foreach (var m in matchNode.Matches)
          result.Matches[m.Key] = new ZTokenWord(m.Value);

      return result;
    }


    protected void AddNode(NFAMatchNode s, List<NFAMatchNode> nodes)
    {
      if (s.Node.Type == NFANode.TypeEnum.Split)
      {
        foreach (NFAEdge e in s.Node.Out)
          AddNode(new NFAMatchNode(e.Target, s.Matches), nodes);
      }
      else
      {
        nodes.Add(s);
      }
    }


    protected void Step(List<NFAMatchNode> clist, ZToken inp, List<NFAMatchNode> nlist)
    {
      nlist.Clear();
      foreach (NFAMatchNode c in clist)
      {
        bool literalOk = 
          c.Node.Type == NFANode.TypeEnum.Literal
            && (c.Node.Literal == null || c.Node.Literal.Equals(inp.OriginalText, StringComparison.CurrentCultureIgnoreCase));

        bool entityOk = 
          c.Node.Type == NFANode.TypeEnum.EntityLiteral
            && inp is ZTokenEntity;

        if (literalOk || entityOk)
        {
          foreach (string m in c.Node.MatchNames)
          {
            if (!c.Matches.ContainsKey(m))
              c.Matches[m] = inp.OriginalText;
            else
              c.Matches[m] += " " + inp.OriginalText;
          }
          AddNode(new NFAMatchNode(c.Node.Out[0].Target, c.Matches), nlist);
        }
      }
    }
  }
}

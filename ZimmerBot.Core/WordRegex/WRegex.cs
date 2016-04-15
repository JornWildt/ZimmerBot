using System;
using System.Collections.Generic;
using System.Linq;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.WordRegex
{
  public abstract class WRegex
  {
    public abstract double CalculateSize();


    public abstract NFAFragment CalculateNFAFragment(TriggerEvaluationContext context);


    public NFANode CalculateNFA(TriggerEvaluationContext context)
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


   public MatchResult CalculateNFAMatch(TriggerEvaluationContext context)
    {
      NFANode start = CalculateNFA(context);

      List<NFAMatchNode> clist = new List<NFAMatchNode>();
      List<NFAMatchNode> nlist = new List<NFAMatchNode>();

      AddNode(new NFAMatchNode(start), clist);

      foreach (var inp in context.InputContext.Input)
      {
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
        result.Matches[i.ToString()] = "";

      if (matchNode != null)
        foreach (var m in matchNode.Matches)
          result.Matches[m.Key] = m.Value;

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
        if (c.Node.Type == NFANode.TypeEnum.Literal)
        {
          if (c.Node.Literal == null || c.Node.Literal.Equals(inp.OriginalText, StringComparison.CurrentCultureIgnoreCase))
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
}

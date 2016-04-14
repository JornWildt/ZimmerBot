using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.WordRegex
{
  public abstract class WRegex
  {
    public class MatchResult
    {
      public double Score { get; set; }

      public Dictionary<string, object> Matches { get; protected set; }

      public string MatchedText { get; protected set; }


      public MatchResult(double score, string matchedText)
      {
        Score = score;
        Matches = new Dictionary<string, object>();
        MatchedText = matchedText;
      }


      public MatchResult(MatchResult src, double score, string matchedText)
      {
        Score = score;
        Matches = new Dictionary<string, object>(src.Matches);
        MatchedText = matchedText;
      }


      public MatchResult RegisterMatch(string matchName, object value)
      {
        if (matchName != null && value != null)
          Matches[matchName] = value;
        return this;
      }

      public static MatchResult Sequence(MatchResult a, MatchResult b)
      {
        if (a == null && b == null)
          return null;
        else if (a == null)
          return b;
        else if (b == null)
          return a;

        MatchResult v = new MatchResult(a.Score * b.Score, (a.MatchedText + " " + b.MatchedText).Trim());

        foreach (var item in a.Matches)
          v.Matches[item.Key] = item.Value;
        foreach (var item in b.Matches)
          v.Matches[item.Key] = item.Value;

        return v;
      }
    }


    public string MatchName { get; set; }


    public abstract double CalculateSize();

    public abstract WRegex GetLookahead();

    public abstract MatchResult CalculateMatchResult(TriggerEvaluationContext context, WRegex lookahead);


    public virtual NFAFragment CalculateNFAFragment(TriggerEvaluationContext context)
    {
      throw new NotImplementedException("CalculateNFAFragment not implemented for " + GetType());
    }


    public NFANode CalculateNFA(TriggerEvaluationContext context)
    {
      NFAFragment f = CalculateNFAFragment(context);
      Patch(f.Out, NFANode.MatchNode);

      return f.Start;
    }


    protected void Patch(List<NFAEdge> edges, NFANode n)
    {
      foreach (NFAEdge e in edges)
      {
        if (e.Target == null)
          e.Target = n;
      }
    }


    public class NFAMatchNode
    {
      public NFANode Node { get; set; }
      public Dictionary<string, string> Matches { get; set; }

      public NFAMatchNode(NFANode n)
      {
        Node = n;
        Matches = new Dictionary<string, string>();
      }

      public NFAMatchNode(NFANode n, Dictionary<string,string> matches)
      {
        Node = n;
        Matches = new Dictionary<string, string>(matches);
      }
    }

    public MatchResult CalculateNFAMatch(TriggerEvaluationContext context)
    {
      NFANode start = CalculateNFA(context);

      //Dictionary<string, string> matches = new Dictionary<string, string>();
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

      MatchResult result = new MatchResult(score, "");

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
          //Condition.Requires(c.Out, nameof(c.Out)).IsNotNull();
          //Condition.Requires(c.Out.Count, nameof(c.Out.Count)).IsEqualTo(1);

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

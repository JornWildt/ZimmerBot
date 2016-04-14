using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.WordRegex
{
  public class NFANode
  {
    public enum TypeEnum { Literal, Split, Match }

    public TypeEnum Type { get; protected set; }

    public string Literal { get; protected set; }

    public List<string> MatchNames { get; protected set; }

    public List<NFAEdge> Out { get; protected set; }


    public static readonly NFANode MatchNode = new NFANode { Type = TypeEnum.Match };


    internal NFANode()
    {
      Out = new List<NFAEdge>();
      MatchNames = new List<string>();
    }


    public static NFANode CreateLiteral(TriggerEvaluationContext context, string literal)
    {
      NFANode n = new NFANode { Type = TypeEnum.Literal, Literal = literal };
      n.Out.Add(new NFAEdge(null));
      n.MatchNames.AddRange(context.MatchNames);
      return n;
    }


    public static NFANode CreateSplit(TriggerEvaluationContext context, IEnumerable<NFANode> choices)
    {
      NFANode n = new NFANode { Type = TypeEnum.Split };
      n.Out.AddRange(choices.Select(c => new NFAEdge(c)));
      return n;
    }


    public static NFANode CreateSplit(TriggerEvaluationContext context, params NFANode[] choices)
    {
      return CreateSplit(context, (IEnumerable<NFANode>)choices);
    }
  }
}

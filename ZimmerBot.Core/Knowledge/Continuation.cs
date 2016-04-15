using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;

namespace ZimmerBot.Core.Knowledge
{
  public class Continuation
  {
    public enum ContinuationEnum
    {
      Empty,   // Continue with no input
      Input,   // Continue as if this were the input
      Label,   // Continue with the rule having the supplied label labe
      Answer   // Continue as if expecting answer to the rule with the supplied labe
    }

    public ContinuationEnum ContinuationType { get; protected set; }

    public string Text { get; protected set; }


    public Continuation(ContinuationEnum type, string s)
    {
      ContinuationType = type;
      Text = s;
    }
  }
}

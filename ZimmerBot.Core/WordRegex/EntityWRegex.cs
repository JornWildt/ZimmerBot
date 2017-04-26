using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.WordRegex
{
  public class EntityWRegex : WRegexBase
  {
    public override void ExtractWordsForSpellChecker()
    {
      // Do nothing
    }


    public override double CalculateSize()
    {
      return 1;
    }


    public override NFAFragment CalculateNFAFragment(TriggerEvaluationContext context)
    {
      NFANode node = NFANode.CreateEntityLiteral(context);
      return new NFAFragment(node, node.Out);
    }


    public override string ToString()
    {
      return "%ENTITY";
    }
  }
}

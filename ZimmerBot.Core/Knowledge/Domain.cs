using System.Collections.Generic;
using ZimmerBot.Core.Language;


namespace ZimmerBot.Core.Knowledge
{
  public class Domain
  {
    public string Name { get; protected set; }

    private List<WordDefinition> WordDefinitions = new List<WordDefinition>();

    private List<Rule> Rules = new List<Rule>();


    internal Domain(string name)
    {
      Name = name;
    }


    public WordDefinition DefineWord(string word)
    {
      WordDefinition w = new WordDefinition(word);
      WordDefinitions.Add(w);
      return w;
    }


    public Rule AddRule(params string[] topics)
    {
      Rule r = new Rule(topics);
      Rules.Add(r);
      return r;
    }


    public void ExpandTokens(ZTokenString input)
    {
      foreach (ZToken t in input)
      {
        foreach (WordDefinition w in WordDefinitions)
        {
          w.ExpandToken(t);
        }
      }
    }


    public void FindMatchingReactions(ZTokenString input, ReactionSet reactions)
    {
      foreach (Rule r in Rules)
      {
        Reaction reaction = r.CalculateReaction(input);
        if (reaction != null)
          reactions.Add(reaction);
      }
    }
  }
}

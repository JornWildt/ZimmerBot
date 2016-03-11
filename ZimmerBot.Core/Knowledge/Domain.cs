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


    public Rule AddRule(params string[] matches)
    {
      Rule r = new Rule(matches);
      Rules.Add(r);
      return r;
    }


    public void ExpandTokens(TokenString input)
    {
      foreach (Token t in input)
      {
        foreach (WordDefinition w in WordDefinitions)
        {
          w.ExpandToken(t);
        }
      }
    }


    public void FindMatchingReactions(TokenString input, IList<Reaction> reactions)
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

using System.Collections.Generic;


namespace ZimmerBot.Core.Knowledge
{
  public static class RuleRepository
  {
    static Dictionary<string, Rule> Rules = new Dictionary<string, Rule>();


    public static void Add(Rule r)
    {
      Rules[r.Id] = r;
    }


    public static Rule Get(string id)
    {
      return Rules[id];
    }
  }
}

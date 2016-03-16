using System.Collections.Generic;


namespace ZimmerBot.Core.Knowledge
{
  public static class BotRepository
  {
    static Dictionary<string, Bot> Bots = new Dictionary<string, Bot>();


    public static void Add(Bot b)
    {
      Bots[b.Id] = b;
    }


    public static Bot Get(string id)
    {
      return Bots[id];
    }


    public static void Remove(string id)
    {
      Bots.Remove(id);
    }
  }
}

using System;
using ZimmerBot.Core;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBotEliza
{
  class Program
  {
    static Random Randomizer = new Random();


    static void Main(string[] args)
    {
      ZimmerBotConfiguration.Initialize();
      KnowledgeBase kb = new KnowledgeBase();
      ElizaDomain.Initialize(kb);
      Bot b = new Bot(kb);
      ConsoleBotEnvironment.RunInteractiveConsoleBot("Eliza> ", b, InputModifier);
      ZimmerBotConfiguration.Shutdown();
    }


    private static string InputModifier(string s)
    {
      // Introduce a tiny break before answering way too quick
      //System.Threading.Thread.Sleep((int)(Randomizer.NextDouble() * 500 + 200));

      return s;
    }
  }
}

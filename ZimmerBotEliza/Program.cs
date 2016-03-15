using ZimmerBot.Core;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBotEliza
{
  class Program
  {
    static void Main(string[] args)
    {
      KnowledgeBase kb = new KnowledgeBase();
      ElizaDomain.Initialize(kb);
      Bot b = new Bot(kb);
      ConsoleBotEnvironment.RunInteractiveConsoleBot("Eliza> ", b);
    }
  }
}

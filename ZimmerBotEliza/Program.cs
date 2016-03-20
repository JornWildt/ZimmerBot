using System;
using log4net;
using ZimmerBot.Core;
using ZimmerBot.Core.ConfigParser;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBotEliza
{
  class Program
  {
    static ILog Logger = LogManager.GetLogger(typeof(Program));

    static Random Randomizer = new Random();


    static void Main(string[] args)
    {
      log4net.Config.XmlConfigurator.Configure();
      Logger.Info("**** STARTING ZimmerBotEliza ****");

      ZimmerBotConfiguration.Initialize();
      KnowledgeBase kb = new KnowledgeBase();
      Domain de = kb.NewDomain("Eliza");

      ConfigurationParser cfg = new ConfigurationParser();
      cfg.ParseConfigurationFromFile(de, "Eliza.txt");

      //ElizaDomain.Initialize(kb);

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

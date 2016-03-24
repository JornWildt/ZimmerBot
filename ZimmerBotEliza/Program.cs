using System;
using System.IO;
using log4net;
using ZimmerBot.Core;
using ZimmerBot.Core.ConfigParser;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBotEliza
{
  class Program
  {
    static ILog Logger = LogManager.GetLogger(typeof(Program));


    static void Main(string[] args)
    {
      // Start logging
      log4net.Config.XmlConfigurator.Configure();
      Logger.Info("**** STARTING ZimmerBotEliza ****");

      // Initialize bot framework
      ZimmerBotConfiguration.Initialize();

      // Initialize bot from Eliza file
      KnowledgeBase kb = KnowledgeBase.LoadFromFiles(".");
      Bot b = new Bot(kb);

      // Run bot
      ConsoleBotEnvironment.RunInteractiveConsoleBot("Eliza> ", b);

      // Shutdown framework again (this is required as there are some background threads that need to be aborted)
      ZimmerBotConfiguration.Shutdown();
    }
  }
}

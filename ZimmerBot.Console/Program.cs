using System.IO;
using log4net;
using ZimmerBot.Core;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Console
{
  class Program
  {
    static ILog Logger = LogManager.GetLogger(typeof(Program));


    static void Main(string[] args)
    {
      // Start logging
      log4net.Config.XmlConfigurator.Configure();
      Logger.Info("**** STARTING ZimmerBot.Console ****");

      // Initialize bot framework
      ZimmerBotConfiguration.Initialize();

      KnowledgeBase.InitializationMode initMode = KnowledgeBase.InitializationMode.RestoreIfExists;

      try
      {
        KnowledgeBase kb = new KnowledgeBase();
        kb.Initialize(initMode);

        if (args.Length == 0)
        {
          // Initialize bot from files in example directory
          kb.LoadFromFiles("..\\..\\..\\Examples\\da-DK");
          kb.LoadFromFiles("..\\..\\..\\Examples");
          kb.LoadFromFiles("..\\..\\..\\Rejseplanen.ZimmerBot.AddOn\\Examples\\da-DK");
        }
        else
        {
          foreach (string path in args)
            kb.LoadFromFiles(path);
        }

        Bot b = new Bot(kb);

        // Run bot
        ConsoleBotEnvironment.RunInteractiveConsoleBot("ZimmerBot> ", b);
      }
      catch (ParserException ex)
      {
        Logger.Fatal(ex);
        System.Console.WriteLine("Parser error:\n" + ex.ToString());
      }

      // Shutdown framework again (this is required as there are some background threads that need to be aborted)
      ZimmerBotConfiguration.Shutdown();
    }


    #region Some sanity checks for manual checking in output (and fun)

    static void RunTestOutput(Bot b)
    {
      Invoke(b, "Er det oktober");
      Invoke(b, "Er det marts");

      Invoke(b, "Hvad er vejret i Boston");
      Invoke(b, "Hvornår blev Snehvide skrevet");
      Invoke(b, "Hvornår");
      Invoke(b, "Hvem spillede med i Snehvide");
      Invoke(b, "Himmel og helvede");
      Invoke(b, "Er det tirsdag");
      Invoke(b, "Er det fredag");
      Invoke(b, "Hvad ved du");
      Invoke(b, "Er det forår");
      Invoke(b, "Hvor gammel er du");
      Invoke(b, "Hvilken dag er det");
      Invoke(b, "Hvilken måned er det");
      Invoke(b, "Hvad er klokken");
      Invoke(b, "Hvilken dato er det");
      Invoke(b, "Hvilken dato er det? Hvad er klokken?");
    }


    // Invoke bot directly with request/response
    static void Invoke(Bot b, string input)
    {
      System.Console.WriteLine("Input> " + input);
      string[] output = b.Invoke(new Request { Input = input }).Output;
      foreach (string s in output)
        System.Console.WriteLine("ZimmerBot> " + s);
    }

    #endregion
  }
}

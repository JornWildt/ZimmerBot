﻿using ZimmerBot.cBrain.F2.Domain;
using ZimmerBot.Console.Domains;
using ZimmerBot.Core;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Console
{
  class Program
  {
    static void Main(string[] args)
    {
      ZimmerBotConfiguration.Initialize();

      // Initialize bot
      KnowledgeBase kb = InitializeKnowledgeBase();
      Bot b = new Bot(kb);

      // Remove this line if you do not want to see a bunch of test interactions
      //RunTestOutput(b);

      // Run bot
      ConsoleBotEnvironment.RunInteractiveConsoleBot("ZimmerBot> ", b);

      ZimmerBotConfiguration.Shutdown();
    }


    static KnowledgeBase InitializeKnowledgeBase()
    {
      KnowledgeBase kb = new KnowledgeBase();

      GeographyDomain.Initialize(kb);
      WeatherDomain.Initialize(kb);
      GeneralDomain.Initialize(kb);
      MovieDomain.Initialize(kb);
      DateTimeDomain.Initialize(kb);
      SelfDomain.Initialize(kb);
      LanguageDomain.Initialize(kb);
      F2Domain.Initialize(kb);

      return kb;
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

using ZimmerBot.cBrain.F2.Domain;
using ZimmerBot.Console.Domains;
using ZimmerBot.Core;
using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.Console
{
  class Program
  {
    static void Main(string[] args)
    {
      KnowledgeBase kb = InitializeKnowledgeBase();
      Bot b = new Bot(kb);

      //RunTestOutput(b);
      RunInteractive(b);
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

      F2Domain.Initialize(kb);

      return kb;
    }


    #region Interactive asynchronous bot via console

    static void RunInteractive(Bot b)
    {
      BotHandle bh = b.Run(new ConsoleBotEnvironment("ZimmerBot> "));

      string input;
      System.Console.Write("> ");

      do
      {
        input = System.Console.ReadLine();
        if (!string.IsNullOrEmpty(input))
        {
          bh.Invoke(new Request { Input = input });
        }
      }
      while (!string.IsNullOrEmpty(input));

      bh.Abort();
    }

    #endregion


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

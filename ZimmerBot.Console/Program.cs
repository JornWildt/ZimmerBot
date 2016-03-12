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
      KnowledgeBase kb = InitializeKnowledge();
      Check(kb);
      Interactive(kb);
    }

    static void Check(KnowledgeBase kb)
    {
      Bot b = new Bot(kb);

      Invoke(b, "skrevet Snehvide hvornår");

      Invoke(b, "Er det oktober");
      Invoke(b, "Er det marts");
      Invoke(b, "Hvad er vejret i Boston");
      Invoke(b, "Hvornår blev Snehvide skrevet");
      Invoke(b, "skrevet Hvornår blev Snehvide");
      Invoke(b, "skrevet Snehvide hvornår");
      Invoke(b, "Hvornår");
      Invoke(b, "Hvem spillede med i Snehvide");
      Invoke(b, "Himmel og helvede");
      Invoke(b, "Er det tirsdag");
      Invoke(b, "Er det fredag");
      Invoke(b, "Hvad ved du");
      Invoke(b, "Er det forår");
      Invoke(b, "Hvor gammel er du");
    }


    static void Invoke(Bot b, string input)
    {
      System.Console.WriteLine("Input> " + input);
      string[] output = b.Invoke(new Request { Input = input }).Output;
      foreach (string s in output)
        System.Console.WriteLine("ZimmerBot> " + s);
    }

    static void Interactive(KnowledgeBase kb)
    {
      Bot b = new Bot(kb);
      string input;

      do
      {
        System.Console.Write("> ");
        input = System.Console.ReadLine();
        if (!string.IsNullOrEmpty(input))
        {
          string[] output = b.Invoke(new Request { Input = input }).Output;

          foreach (string s in output)
            System.Console.WriteLine("ZimmerBot> " + s);
        }
      }
      while (!string.IsNullOrEmpty(input));
    }


    static KnowledgeBase InitializeKnowledge()
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
  }
}

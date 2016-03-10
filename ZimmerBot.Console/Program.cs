using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    static KnowledgeBase InitializeKnowledge()
    {
      KnowledgeBase kb = new KnowledgeBase();

      // General
      Domain d = kb.NewDomain("General");
      d.DefineWord("Hvornår").Is("question").Is("question-when");

      // Movies
      Domain md = kb.NewDomain("Movies");
      md.DefineWord("Snehvide").Is("movie-role").Is("movie-title");
      md.DefineWord("Blinkende lygter").Is("movie-title");
      md.DefineWord("Rødhætte").Is("movie-role");
      md.DefineWord("lavet").Is("movie-recorded");
      md.DefineWord("optaget").Is("movie-recorded");
      md.DefineWord("skudt").Is("movie-recorded");
      md.DefineWord("skrevet").Is("movie-written");

      md.AddRule("question-when", "movie-title", "movie-recorded")
        .Describe("")
        .Parameter("name", "movie-title")
        .SetResponse(new MovieResponseGenerator());

      return kb;
    }

    static void Check(KnowledgeBase kb)
    {
      Bot b = new Bot(kb);

      Invoke(b, "Hvornår blev Snehvide skrevet");
      Invoke(b, "skrevet Hvornår blev Snehvide");
      Invoke(b, "skrevet Snehvide hvornår");
    }


    static void Invoke(Bot b, string input)
    {
      System.Console.WriteLine("Input> " + input);
      string output = b.Invoke(new Request { Input = input }).Output;
      System.Console.WriteLine("ZimmerBot> " + output);
    }

    static void Interactive(KnowledgeBase kb)
    {
      Bot b = new Bot(kb);
      string input;

      do
      {
        input = System.Console.ReadLine();
        if (!string.IsNullOrEmpty(input))
        {
          string output = b.Invoke(new Request { Input = input }).Output;

          System.Console.WriteLine("ZimmerBot> " + output);
        }
      }
      while (!string.IsNullOrEmpty(input));
    }
  }
}

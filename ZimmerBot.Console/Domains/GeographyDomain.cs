using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Console.Domains
{
  public class GeographyDomain
  {
    public static void Initialize(KnowledgeBase kb)
    {
      Domain dd = kb.NewDomain("Geografi");
      dd.DefineWord("Boston").Is("location").Is("city");
      dd.DefineWord("Nærum").Is("location").Is("city");
      dd.DefineWord("Allerød").Is("location").Is("city");
    }
  }
}

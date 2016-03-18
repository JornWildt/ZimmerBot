using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.WordRegex;
using ZimmerBot.Core.Processors;


namespace ZimmerBot.Console.Domains
{
  public class LanguageDomain
  {
    public static void Initialize(KnowledgeBase kb)
    {
      Domain ds = kb.NewDomain("Sprog");

      //ds.AddRule("oversæt", new RepitionWRegex(new WildcardWRegex(), "t"), "til", new WildcardWRegex("s"))
      //  .Response(c => LanguageProcessor.Translate(c.Match.Matches["t"], c.Match.Matches["s"], "Javel. Det er: '<answer>'."));
    }
  }
}
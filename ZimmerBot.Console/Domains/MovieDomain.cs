using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Responses;


namespace ZimmerBot.Console.Domains
{
  public class MovieDomain
  {
    public static void Initialize(KnowledgeBase kb)
    {
      Domain md = kb.NewDomain("Film");
      md.DefineWord("Snehvide").Is("movie-role").Is("movie-title");
      md.DefineWord("Blinkende lygter").Is("movie-title");
      md.DefineWord("Rødhætte").Is("movie-role");
      md.DefineWord("lavet").Is("movie-recorded");
      md.DefineWord("optaget").Is("movie-recorded");
      md.DefineWord("skudt").Is("movie-recorded");
      md.DefineWord("skrevet").Is("movie-written");
      md.DefineWord("spillede").And("spiller").Is("movie-played");

      md.AddRule("question-when", "movie-title", "movie-recorded")
        .Describe("Hvornår blev en film optaget.")
        .Parameter("name", "movie-title")
        .SetResponse(new MovieResponseGenerator("Recorded-date"));

      md.AddRule("question-who", "movie-title", "movie-played")
        .Describe("Hvem spillede med i.")
        .Parameter("name", "movie-title")
        .SetResponse(new MovieResponseGenerator("actors"));
    }
  }
}

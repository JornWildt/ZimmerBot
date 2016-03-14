using System;
using ZimmerBot.Core.Parser;
using ZimmerBot.Core.Utilities;


namespace ZimmerBot.Core.DataProcessors
{
  public static class MovieProcessors
  {
    public static Func<string> FindActorsInMovie(ZToken movie, string template)
    {
      // FIXME: this is only a skeleton of how it could be implemented. 
      // - Answer should be found by API lookup.
      string[] answer = new string[] { "Jennifer Einstein", "Steen Gråbøl" };

      return () => TextMerge.MergeTemplate(template, new { movie_title = movie.OriginalText, answer = answer });
    }


    public static Func<string> FindRecordedDate(ZToken movie, string template)
    {
      // FIXME: this is only a skeleton of how it could be implemented. 
      // - Answer should be found by API lookup.
      string answer = "January 2011";

      return () => TextMerge.MergeTemplate(template, new { movie_title = movie.OriginalText, answer = answer });
    }
  }
}

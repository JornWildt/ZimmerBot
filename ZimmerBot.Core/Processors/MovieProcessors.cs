using System;
using ZimmerBot.Core.Language;
using ZimmerBot.Core.Utilities;


namespace ZimmerBot.Core.DataProcessors
{
  public static class MovieProcessors
  {
    public static Func<string> FindActorsInMovie(ZToken movie, string template)
    {
      string[] answer = new string[] { "Jennifer Einstein", "Steen Gråbøl" };

      return () => TextMerge.MergeTemplate(template, new { movie_title = movie.OriginalText, answer = answer });
    }


    public static Func<string> FindRecordedDate(ZToken movie, string template)
    {
      string answer = "January 2011";

      return () => TextMerge.MergeTemplate(template, new { movie_title = movie.OriginalText, answer = answer });
    }
  }
}

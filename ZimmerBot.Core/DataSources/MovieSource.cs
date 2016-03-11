﻿using System;
using ZimmerBot.Core.Language;
using ZimmerBot.Core.Utilities;


namespace ZimmerBot.Core.DataSources
{
  public static class MovieSource
  {
    public static Func<string> FindActorsInMovie(Token movie, string template)
    {
      string[] answer = new string[] { "Jennifer Einstein", "Steen Gråbøl" };

      return () => TextMerge.MergeTemplate(template, new { movie_title = movie.OriginalText, answer = answer });
    }


    public static Func<string> FindRecordedDate(Token movie, string template)
    {
      string answer = "January 2011";

      return () => TextMerge.MergeTemplate(template, new { movie_title = movie.OriginalText, answer = answer });
    }
  }
}

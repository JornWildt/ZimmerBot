using System;
using ZimmerBot.Core.Utilities;


namespace ZimmerBot.Core.Processors
{
  public static class LanguageProcessor
  {
    public static Func<string> Translate(object input, object language, string template)
    {
      // Another simplistic example of a processor. This one does language translation.
      // It could for instance use Google Translate for the purpose.

      // FIXME: how to do type check (and avoid rule match when types mismatch)
      if (!(input is string))
        throw new InvalidOperationException("Invalid input string");
      if (!(language is string))
        throw new InvalidOperationException("Invalid language string");

      string answer = "<" + (string)language + ">" + (string)input;

      return () => TextMerge.MergeTemplate(template, new { input = input, language = language, answer = answer });
    }
  }
}

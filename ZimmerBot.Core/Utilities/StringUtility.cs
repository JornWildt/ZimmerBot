using System;
using System.Text.RegularExpressions;

namespace ZimmerBot.Core.Utilities
{
  public static class StringUtility
  {
    static Regex IdentifierRegex = new Regex("[^\\w0-9_]");


    public static string Word2Identifier(string word)
    {
      if (word == null)
        return Guid.NewGuid().ToString();

      return IdentifierRegex.Replace(word, "_");
    }
  }
}

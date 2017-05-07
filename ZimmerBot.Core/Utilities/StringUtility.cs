using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZimmerBot.Core.Utilities
{
  public static class StringUtility
  {
    static Regex IdentifierRegex = new Regex("[^\\w0-9_]");


    public static string Word2Identifier(string word)
    {
      return IdentifierRegex.Replace(word, "_");
    }
  }
}

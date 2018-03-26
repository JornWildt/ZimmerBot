using System.Globalization;

namespace ZimmerBot.Core.Utilities
{
  public class StringRender : Antlr4.StringTemplate.StringRenderer
  {
    public override string ToString(object obj, string formatString, CultureInfo culture)
    {
      string s = obj as string;
      if (s == null)
        return null;
      if (s == "")
        return "";

      if (formatString == "UF")
        return char.ToUpper(s[0]) + s.Substring(1);
      else
        return base.ToString(obj, formatString, culture);
    }
  }
}

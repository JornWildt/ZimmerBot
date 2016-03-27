namespace ZimmerBot.Core.TemplateParser
{
  public static class TemplateUtility
  {
    public static SequenceTemplateToken Parse(string s)
    {
      TemplateParser parser = new TemplateParser();
      parser.Parse(s);
      return parser.Result;
    }
  }
}

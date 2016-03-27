using ZimmerBot.Core.TemplateParser;

namespace ZimmerBot.Core.Tests.TemplateParser
{
  public class TestHelper : Tests.TestHelper
  {
    public SequenceTemplateToken ParseTemplate(string s)
    {
      return TemplateUtility.Parse(s);
    }
  }
}

using NUnit.Framework;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class StringTemplateLoaderTests : TestHelper
  {
    [Test]
    public void CanUseGroupTemplate()
    {
      BuildBot(@"
> aaa
: <CanUseGroupTemplate()>");

      AssertDialog("aaa", "Success 1");
    }


    [Test]
    public void CanUseGroupTemplateWithArgument()
    {
      BuildBot(@"
> aaa +
: <CanUseGroupTemplate2(1)>");

      AssertDialog("aaa horse", "Riding my horse");
    }


    [Test]
    public void CanUseDictionary()
    {
      BuildBot(@"
> map *
: <MapThis(1)>");

      AssertDialog("map aaa", "!aaa!");
      AssertDialog("map bbb", "!bbb!");
      AssertDialog("map ccc", "!default!");
      AssertDialog("map", "!default!");
    }
  }
}

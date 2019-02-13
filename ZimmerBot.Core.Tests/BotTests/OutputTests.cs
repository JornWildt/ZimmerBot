using NUnit.Framework;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class OutputTests : TestHelper
  {
    [Test]
    public void CanTestForUndefinedValueInOutputTemplates()
    {
      BuildBot(@"
> aaa
: Value is <if(session.x)>SET<else>UNSET<endif>

> bbb
: OK
! set session.x = 1
");

      AssertDialog("aaa", "Value is UNSET");
      AssertDialog("bbb", "OK");
      AssertDialog("aaa", "Value is SET");
    }


    [Test]
    public void CanChooseRandomElementsInOutputTemplate()
    {
      BuildBot(@"
> animals
: I know ""<(dog|cat)>""
");
      bool[] matches = new bool[2];
      for (int i = 0; i < 10; ++i)
      {
        string output = Invoke("animals");

        if (output == "I know \"dog\"")
          matches[0] = true;
        else if (output == "I know \"cat\"")
          matches[1] = true;
        else
          Assert.Fail("Unexpected output: " + output);
      }

      Assert.True(matches[0]);
      Assert.True(matches[1]);
    }
  }
}

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class RepetitionAvoidanceTests : TestHelper
  {
    [Test]
    public void ItNeverRepeatsItSelfBeforeAllOutputsAreUsed()
    {
      // Arrange
      string[] outputs = { "aaa", "bbb", "ccc" };
      Dictionary<string, int> outputCount = new Dictionary<string, int>();

      foreach (string o in outputs)
        outputCount[o] = 0;

      string input = @"
>go
: aaa
: bbb
: ccc
";

      for (int x = 0; x < 10; ++x)
      {
        Bot b = BuildBot(input);

        for (int i = 0; i < outputs.Length; ++i)
        {
          string result = Invoke(b, "go");
          outputCount[result] = outputCount[result] + 1;
        }

        foreach (string o in outputs)
          Assert.AreEqual(x+1, outputCount[o]);
      }
    }
  }
}

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

      // Act
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


    [Test]
    public void ItAvoidsRepeatingTextButDoRepeatCallStatements()
    {
      // Arrange
      string[] outputs = { "aaa", "bbb", "ccc" };
      Dictionary<string, int> outputCount = new Dictionary<string, int>();

      foreach (string o in outputs)
        outputCount[o] = 0;
      outputCount["Now"] = 0;

      string input = @"
> go
: aaa
: bbb
: ccc

> what|go
! call DateTime.Time()
: Now
";

      // Act
      for (int x = 0; x < 10; ++x)
      {
        Bot b = BuildBot(input);

        for (int i = 0; i < outputs.Length+1; ++i)
        {
          string result = Invoke(b, "go");
          outputCount[result] = outputCount[result] + 1;

          result = Invoke(b, "what");
          outputCount[result] = outputCount[result] + 1;
        }

        // The idea here is that the first round will select all outputs exactly one time for "go", and "Now" everytime for "what".
        // The following rounds always select "Now" as the "aaa", "bbb" and "ccc" outputs has been down prioritized.

        foreach (string o in outputs)
          Assert.AreEqual(1, outputCount[o]);

        Assert.AreEqual(5+x*8, outputCount["Now"]);
      }
    }
  }
}

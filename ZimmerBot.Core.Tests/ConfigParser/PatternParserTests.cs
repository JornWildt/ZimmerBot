using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class PatternParserTests : TestHelper
  {
    [Test]
    public void CanParseWeatherRequests()
    {
      string cfg = @"
! pattern (intent = current_weather, type = ""question"")
{
  > is it snowing
  > is it raining in {entity:location:}
}
";
      KnowledgeBase kb = ParseKnowledgeBase(cfg);
    }
  }
}

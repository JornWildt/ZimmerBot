using NUnit.Framework;
using ZimmerBot.Core.Expressions;

namespace ZimmerBot.Core.Tests.Expressions
{
  [TestFixture]
  public class ConverterTests : TestHelper
  {
    [TestCase(false, false)]
    [TestCase(true, true)]
    [TestCase(null, false)]
    [TestCase((string)null, false)]
    [TestCase("x", true)]
    public void CanConvertToBool(object src, bool expected)
    {
      bool result;
      bool ok = Expression.TryConvertToBool(src, out result);

      Assert.True(ok);
      Assert.AreEqual(expected, result);
    }
  }
}

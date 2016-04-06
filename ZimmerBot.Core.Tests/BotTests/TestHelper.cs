using NUnit.Framework;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Tests.BotTests
{
  public class TestHelper : ZimmerBot.Core.Tests.TestHelper
  {
    protected Bot B { get; set; }


    protected Bot BuildBot(string cfg)
    {
      KnowledgeBase kb = ParseKnowledgeBase(cfg);
      B = new Bot(kb);
      return B;
    }


    protected string Invoke(string s)
    {
      return Invoke(B, s);
    }


    protected string Invoke(Bot b, string s)
    {
      Request request = new Request { Input = s };
      return Invoke(b, request);
    }


    protected string Invoke(Bot b, Request request)
    {
      Response response = b.Invoke(request);
      return response.Output[0];
    }


    protected void AssertDialog(string s, string expectedAnswer)
    {
      string r = Invoke(s);
      Assert.AreEqual(expectedAnswer, r);
    }
  }
}

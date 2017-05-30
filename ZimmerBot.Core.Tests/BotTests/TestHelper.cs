using System.Linq;
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
      if (response.Output.Length == 0)
        return "<empty>";
      return response.Output.Aggregate((x,y) => x + "\n" + y);
    }


    protected void AssertDialog(string s, string expectedAnswer, string message = null)
    {
      string r = Invoke(s);
      Assert.AreEqual(expectedAnswer, r, $"Input: {s}" + (message != null ? " <= " + message : ""));
    }
  }
}

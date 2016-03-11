using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.Responses;

namespace ZimmerBot.Console.Domains
{
  public class GeneralDomain
  {
    public static void Initialize(KnowledgeBase kb)
    {
      Domain d = kb.NewDomain("Diverse");
      d.DefineWord("Hvornår").Is("question").Is("question-when");
      d.DefineWord("Hvem").Is("question").Is("question-who");
      d.DefineWord("Er").Is("question").Is("question-is");
      d.DefineWord("Hvad").Is("question").Is("question-what");

      d.AddRule("question")
       .Describe("Hvornår?")
       .SetResponse(new TemplateResponseGenerator("Hvad tror du selv?"));
    }
  }
}

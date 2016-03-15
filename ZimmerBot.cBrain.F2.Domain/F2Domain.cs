using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Knowledge;
using ZimmerBot.Core.WordRegex;


namespace ZimmerBot.cBrain.F2.Domain
{
  public static class F2Domain
  {
    public static void Initialize(KnowledgeBase kb)
    {
      var f2d = kb.NewDomain("cBrain F2");

      f2d.AddRule()
        .Condition(new BinaryOperatorExpr(new IdentifierExpression("state.conversation.entries.Count"), new ConstantNumberExpr(0), BinaryOperatorExpr.OperatorType.Equals))
         .Response("Hej, jeg er F2-assistenten Dr. Zimmer Frei (mit Alles)");

      f2d.AddRule()
        .Condition(new BinaryOperatorExpr(new IdentifierExpression("state.conversation.entries.Count"), new ConstantNumberExpr(0), BinaryOperatorExpr.OperatorType.Equals))
         .Response("- men kald mig bare Zimmer, det gør mine venner.");

      f2d.AddRule()
        .Condition(new BinaryOperatorExpr(new IdentifierExpression("state.conversation.entries.Count"), new ConstantNumberExpr(0), BinaryOperatorExpr.OperatorType.Equals))
         .Response("Spørg mig om hvad som helst :-)");

      f2d.AddRule("f2").Response("F2 er fantastisk!");

      f2d.AddRule("hvor", "gammel", "er", "du")
        //.AddTrigger("Hvornår er du fra")
         .Response(i => ResponseHelper.OneOf(i, "Jeg er ny-i-jobbet.", "Det ved jeg snart ikke.", "Det føles som evigheder.", "Jeg blev skabt i marts 2016."));

      f2d.AddRule("Hvem", "skabte", "dig")
         .Response("Jørn");
    }
  }
}

using System;
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
        .WithCondition(new BinaryOperatorExpr(new IdentifierExpr("state.conversation.entries.Count"), new ConstantValueExpr(0), BinaryOperatorExpr.OperatorType.Equals))
         .WithResponse("Hej, jeg er F2-assistenten Dr. Zimmer Frei (mit Alles)");

      f2d.AddRule()
        .WithCondition(new BinaryOperatorExpr(new IdentifierExpr("state.conversation.entries.Count"), new ConstantValueExpr(0), BinaryOperatorExpr.OperatorType.Equals))
         .WithResponse("- men kald mig bare Zimmer, det gør mine venner.");

      f2d.AddRule()
        .WithCondition(new BinaryOperatorExpr(new IdentifierExpr("state.conversation.entries.Count"), new ConstantValueExpr(0), BinaryOperatorExpr.OperatorType.Equals))
         .WithResponse("Spørg mig om hvad som helst :-)");

      f2d.AddRule("f2").WithResponse("F2 er fantastisk!");

      //f2d.AddRule("hvor", "gammel", "er", "du")
      //  //.AddTrigger("Hvornår er du fra")
      //   .Response(ResponseHelper.OneOf("Jeg er ny-i-jobbet.", "Det ved jeg snart ikke.", "Det føles som evigheder.", "Jeg blev skabt i marts 2016."));

      f2d.AddRule("Hvem", "skabte", "dig")
         .WithResponse("Jørn");

      //f2d.AddRule()
      //   .WithCondition(new FunctionCallExpr("probability", new ConstantNumberExpr(0.2)))
      //   .WithSchedule(TimeSpan.FromSeconds(60))
      //   .Response(i => ResponseHelper.OneOf(i,
      //    "Nåh... ?", 
      //    "Er der nogen der kan en vittighed?"));
    }
  }
}

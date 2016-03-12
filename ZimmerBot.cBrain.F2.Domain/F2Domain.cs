﻿using ZimmerBot.Core.Knowledge;


namespace ZimmerBot.cBrain.F2.Domain
{
  public static class F2Domain
  {
    public static void Initialize(KnowledgeBase kb)
    {
      var f2d = kb.NewDomain("cBrain F2");

      f2d.AddRule("f2").SetResponse("F2 er fantastisk!");

      f2d.AddRule("hvor", "gammel", "er", "du")
         .SetResponse(i => ResponseHelper.OneOf("Jeg er ny-i-jobbet.", "Det ved jeg snart ikke", "Det føles som evigheder"));
    }
  }
}

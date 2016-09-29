using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class EntityTests : TestHelper
  {
    [Test]
    public void CanMatchEntity()
    {
      string z = @"
> what is %ENTITY
: '<1>' is an entity

> where is (%ENTITY) located
: '<1>' is not here
";
      KnowledgeBase kb = ParseKnowledgeBase(z);
      kb.AddEntity("ZimmerBot");
      kb.AddEntity("Acme Inc.");
      kb.AddEntity("Blue-Whale");
      B = new Bot(kb);

      AssertDialog("what is zimmerbot", "'ZimmerBot' is an entity");
      AssertDialog("what is Acme Inc.", "'Acme Inc.' is an entity");
      AssertDialog("what is Acme Inc", "'Acme Inc.' is an entity");
      AssertDialog("Where is Blue-Whale located", "'Blue-Whale' is not here");
      AssertDialog("Where is blue whale located", "'Blue-Whale' is not here");
      AssertDialog("Where is \"blue whale\" located", "'Blue-Whale' is not here");
      AssertDialog("what is randomword", "'randomword' is an entity");
    }


    [Test]
    public void CanLoadEntitiesFromRDF()
    {
      KnowledgeBase kb = ParseKnowledgeBase(@"
!rdf_prefix rdfs ""http://www.w3.org/2000/01/rdf-schema#""
!rdf_import ""ConfigParser/EntityNames.ttl""

!rdf_entities(""
SELECT ?label
WHERE
{
  ?thing rdfs:label ?label .
}
"")

> describe (%ENTITY)
: <1> is an entity
");

      kb.Run();

      Assert.True(kb.Entities.ContainsKey("Alibaba Inc"));
      Assert.True(kb.Entities.ContainsKey("The Little Mermaid"));
      Assert.False(kb.Entities.ContainsKey("The Great Mermaid"));

      B = new Bot(kb);
      AssertDialog("Describe the little mermaid", "The Little Mermaid is an entity");
    }
  }
}

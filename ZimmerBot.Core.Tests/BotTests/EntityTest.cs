using NUnit.Framework;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class EntityTests : TestHelper
  {
    [Test]
    public void CanMatchEntity()
    {
      string z = @"
! entities (company)
{
  ""ZimmerBot"",
  ""Acme Inc."",
  ""Blue-Whale"",
  ""National Health Service""
}

> what is %ENTITY
: '<1>' is an entity

> where is (%ENTITY) located
: '<1>' is not here
";
      BuildBot(z);

      //AssertDialog("what is zimmerbot", "'zimmerbot' is an entity");
      AssertDialog("what is Acme Inc.", "'Acme Inc' is an entity");
      AssertDialog("what is Acme Inc", "'Acme Inc' is an entity");
      AssertDialog("Where is Blue-Whale located", "'Blue Whale' is not here");
      AssertDialog("Where is blue whale located", "'blue whale' is not here");
      AssertDialog("Where is \"blue whale\" located", "'blue whale' is not here");
      AssertDialog("what is randomword", "'randomword' is an entity");
    }


    [Test]
    public void CanLoadEntitiesFromRDF()
    {
      string cfg = @"
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
: '<1>' is an entity
";

      BuildBot(cfg);
      AssertDialog("Describe the little mermaid", "'the little mermaid' is an entity");
      AssertDialog("Describe alibaba inc", "'alibaba inc' is an entity");
      //AssertDialog("Describe alibaba mermaid", "'alibaba mermaid' is an entity");
    }


    [Test]
    public void CanMatchEntityAsNormalWord()
    {
      BuildBot(@"
! entities (person)
{
  ""friend""
}

> do you have a friend
: Yes!
");
      AssertDialog("do you have a friend", "Yes!");
    }
  }
}

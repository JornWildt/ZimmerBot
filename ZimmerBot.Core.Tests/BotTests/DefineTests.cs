using NUnit.Framework;

namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class DefineTests : TestHelper
  {
    [Test]
    public void CanDefineWords()
    {
      BuildBot(@"
! define (animal)
{
  ""Elefant"" (""elefants""):
    ""description"": ""Big animal"";
    ""class"": ""mamal"";
    ""location"": <India>, <Africa>.

  Horse (horses):
    description: ""Large animal with four legs"";
    class: mamal;
    location: <Earth>.

  Bison (bisons):
    description: ""Large animal with four legs"";
    class: mamal;
    location: <North America>.
}

! define (country)
{
  India: .
  ""Africa"": .
  ""North America"": .
}

! define (person)
{
  ""Walt Disney"":
    description: ""Well known person"";
    born: 1901.
}

! pattern (intent = echo)
{
  > echo {a}
}

>> { intent = echo, a = * }
: Got '<a>'
");

      AssertDialog("echo horse", "Got 'horse'");
      AssertDialog("echo horses", "Got 'horses'");
      AssertDialog("echo walt disney", "Got 'walt disney'");
    }


    [Test]
    public void CanFindDefinedDataViaRDF()
    {
      BuildBot(@"
! define (animal)
{
  ""Elefant"" (""elefants""):
    ""description"": ""Big animal"";
    ""class"": ""mamal"";
    ""location"": <India>, <Africa>;
    ""color"": ""gray"".

  Horse (horses):
    description: ""Large animal with four legs"";
    class: mamal;
    location: <Earth>;
    color: brown.

  Bison (bisons):
    description: ""Large animal with four legs"";
    class: mamal;
    color: brown;
    location: <North America>.
}

! define (country)
{
  India: .
  ""Africa"": .
  ""North America"": .
  ""Earth"": .
}

> show animals
! call RDF.Query(""
PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
PREFIX bot: <http://zimmerbot/stuff#>
SELECT ?cname
WHERE
{
  ?animal rdf:type bot:animal.
  ?animal rdfs:label ?name.
}
"")
: <result:{r | <r.name> }>.
");

      AssertDialog("show animals", "Horse etc.");
    }
  }
}

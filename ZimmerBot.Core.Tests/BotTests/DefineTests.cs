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
  ""Elephant"" (""elephants""):
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

! rdf_prefix rdf ""http://www.w3.org/1999/02/22-rdf-syntax-ns#""
! rdf_prefix rdfs ""http://www.w3.org/2000/01/rdf-schema#""
! rdf_prefix ztype ""http://zimmerbot.org/fact/type/""
! rdf_prefix zatt ""http://zimmerbot.org/fact/att/""

> show animals
! call RDF.Query(""
SELECT ?name
WHERE
{
  ?animal rdf:type ztype:animal.
  ?animal rdfs:label ?name
}
ORDER BY ?name
"")
: <result:{r | <r.name> }>.

> show countries
! call RDF.Query(""
SELECT ?country
WHERE
{
  ?country a ztype:country.
}
ORDER BY ?country
"")
: <result:{r | <r.country> }>.

> where can I find elephants
! call RDF.Query(""
SELECT ?name
WHERE
{
  ?country a ztype:country.
  ?animal a ztype:animal.
  ?animal rdfs:label ?aname.
  ?animal zatt:location ?country.
  ?country rdfs:label ?name.
  FILTER (?aname = 'Elephant')
}
ORDER BY ?country
"")
: <result:{r |<r.name>}; separator="", "">.
");

      //AssertDialog("show animals", "Bison Elephant Horse .");
      //AssertDialog("show countries", "http://zimmerbot.org/fact/id/Africa http://zimmerbot.org/fact/id/Earth http://zimmerbot.org/fact/id/India http://zimmerbot.org/fact/id/North_America .");
      AssertDialog("where can I find elephants", "Africa, India.");
    }
  }
}

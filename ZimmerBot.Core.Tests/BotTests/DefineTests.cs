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
      AssertDialog("echo north America", "Got 'north America'");
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
! rdf_prefix zr ""http://zimmerbot.org/resource/""
! rdf_prefix zp ""http://zimmerbot.org/property/""

> show animals
! call RDF.Query(""
SELECT ?name
WHERE
{
  ?animal rdf:type zr:animal.
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
  ?country a zr:country.
}
ORDER BY ?country
"")
: <result:{r | <r.country> }>.

> where can I find elephants
! call RDF.Query(""
SELECT ?name
WHERE
{
  ?country a zr:country.
  ?animal a zr:animal.
  ?animal rdfs:label ?aname.
  ?animal zp:location ?country.
  ?country rdfs:label ?name.
  FILTER (?aname = 'Elephant')
}
ORDER BY ?country
"")
: <result:{r |<r.name>}; separator="", "">.
");

      AssertDialog("show animals", "Bison Elephant Horse .");
      AssertDialog("show countries", "http://zimmerbot.org/resource/Africa http://zimmerbot.org/resource/Earth http://zimmerbot.org/resource/India http://zimmerbot.org/resource/North_America .");
      AssertDialog("where can I find elephants", "Africa, India.");
    }


    [Test]
    public void CanDefineMultipleClassesSimultaneously()
    {
      BuildBot(@"
!define (politician, person)
{
  ""Anker Jørgensen"":.
}

! rdf_prefix rdf ""http://www.w3.org/1999/02/22-rdf-syntax-ns#""
! rdf_prefix rdfs ""http://www.w3.org/2000/01/rdf-schema#""

> show all
! call RDF.Query(""
SELECT ?name
WHERE
{
  ?s rdf:type ?type.
  ?type rdfs:label ?name.
}
ORDER BY ?name
"")
: <result:{r | <r.name> }>.
");

      AssertDialog("show all", "person politician .");
    }

    [Test]
    public void WordAlternativesAreAddedAsEntities()
    {
      BuildBot(@"
! define (person)
{
  ""John Benny Andersen"" (""John Ben""):.
}

! pattern (intent = who)
{
  > who is {a}
}

! rdf_prefix zp ""http://zimmerbot.org/property/""
! rdf_prefix rdfs ""http://www.w3.org/2000/01/rdf-schema#""

>> { intent = who, a = * }
! call RDF.Query(""
SELECT ?name
WHERE
{
  ?subj zp:knownby? ?knownby.
  ?subj rdfs:label ?name
  FILTER(?knownby = lcase(@a))
}
LIMIT 1
"")
: <result:{r |<r.name>}>
");
      AssertDialog("who is ben", "???");
      AssertDialog("who is John Benny Andersen", "John Benny Andersen");
      AssertDialog("who is John Ben", "John Benny Andersen");
    }
  }
}

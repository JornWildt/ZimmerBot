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
}

! pattern (intent = show_me_animals)
{
  > show me the animals
}

>> { intent = show_me_animals, a = * }
! call RDF.Query(""
SELECT ? cname ? csname ? aname
WHERE
{
  ?name rdfs:
  ?feature rdf:type gn:Feature.
  ? feature gn:alternateName? fname .
  ?feature gn:parentCountry? country .
  OPTIONAL { ?country gn:alternateName? cname }.
  OPTIONAL { ?country gn:shortName? csname }.
  OPTIONAL { 
    ?feature gn:parentADM1? area .
    ?area gn:alternateName? aname
  }.
  FILTER(lcase(str(?fname)) = lcase(str(@2)))
}
      LIMIT 1
"")
: < 2 > ligger i < result:{ r | <if (r.csname)>< r.csname ><else>< r.cname >< endif ><if (r.aname)> (< r.aname >) < endif >}>.

");

      AssertDialog("echo horse", "Got 'horse'");
      AssertDialog("echo horses", "Got 'horses'");
      AssertDialog("echo walt disney", "Got 'walt disney'");
    }
  }
}

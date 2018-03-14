using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;


namespace ZimmerBot.Core.Tests.BotTests
{
  [TestFixture]
  public class RDFTests : TestHelper
  {
    [Test]
    public void CanLookupEntitiesByAlternatives()
    {
      BuildBot(@"
! define (animal)
{
  Crocodile (crocodiles, croc):.
  Horse (horses):.
}

! rdf_prefix rdfs ""http://www.w3.org/2000/01/rdf-schema#""
! rdf_prefix zp ""http://zimmerbot.org/property/""

! pattern (intent = what_is)
{
  > what is {topic}
}

>> { intent = what_is, topic = * }
! call RDF.Query(""
SELECT DISTINCT ?name
WHERE
{
  ?thing rdfs:label ?name.
  ?thing zp:knownby ?k.
  FILTER ( ?k = lcase(@topic) )
}
"")
: Yes: <result:{r |<r.name>}>.
");

      AssertDialog("what is crocodile", "Yes: Crocodile.");
      AssertDialog("what is croc", "Yes: Crocodile.");
      AssertDialog("what is CROCODILES", "Yes: Crocodile.");
    }


    [Test]
    public void CanStartTopicFromRdf()
    {
      BuildBot(@"
! define (company)
{
  ""Bryan Sport"":
    intro: ""Blah ..."";
    topic: Bryan.
}

! rdf_prefix rdfs ""http://www.w3.org/2000/01/rdf-schema#""
! rdf_prefix zp ""http://zimmerbot.org/property/""

! pattern (intent = what_is)
{
  > what is {subj}
}

>> { intent = what_is, subj = * }
! call RDF.Query(""
SELECT DISTINCT ?name ?topic
WHERE
{
  ?subject rdfs:label ?name.
  ?subject zp:knownby ?k.
  ?subject zp:topic ?topic
  FILTER ( ?k = lcase(@subj) )
}
"")
! call RDF.TrySetTopic($_, ""topic"")
: Yes: <result:{r |<r.name> (topic: <r.topic>)}>.

! topic Bryan
{
  > help
  : Bryan or Bruce - who cares?
}
");

      AssertDialog("help", "???");
      AssertDialog("what is bryan sport", "Yes: Bryan Sport (topic: Bryan).");
      AssertDialog("help", "Bryan or Bruce - who cares?");
    }


    [Test]
    public void CanStartTopicFromRdfWithDefaultNameForTopicParameter()
    {
      BuildBot(@"
! define (company)
{
  ""Bryan Sport"":
    intro: ""Blah ..."";
    topic: Bryan.
}

! rdf_prefix rdfs ""http://www.w3.org/2000/01/rdf-schema#""
! rdf_prefix zp ""http://zimmerbot.org/property/""

! pattern (intent = what_is)
{
  > what is {subj}
}

>> { intent = what_is, subj = * }
! call RDF.Query(""
SELECT DISTINCT ?name ?topic
WHERE
{
  ?subject rdfs:label ?name.
  ?subject zp:knownby ?k.
  ?subject zp:topic ?topic
  FILTER ( ?k = lcase(@subj) )
}
"")
! call RDF.TrySetTopic($_)
: Yes: <result:{r |<r.name> (topic: <r.topic>)}>.

! topic Bryan
{
  > help
  : Bryan or Bruce - who cares?
}
");

      AssertDialog("help", "???");
      AssertDialog("what is bryan sport", "Yes: Bryan Sport (topic: Bryan).");
      AssertDialog("help", "Bryan or Bruce - who cares?");
    }


    [Test]
    public void CanDefineAnonymousEntitites()
    {
      BuildBot(@"
! define (registration)
{
  $:
    intro: ""Blah"".
}

! rdf_prefix rdfs ""http://www.w3.org/2000/01/rdf-schema#""
! rdf_prefix zp ""http://zimmerbot.org/property/""

! pattern (intent = what_is)
{
  > show it
}

>> { intent = what_is }
! call RDF.Query(""
SELECT DISTINCT ?name ?intro
WHERE
{
  ?thing zp:intro ?intro.
  OPTIONAL { ?thing rdfs:label ?name }
}
"")
: Got: <result:{r |<r.name>/<r.intro>}>.
");

      AssertDialog("show it", "Got: /Blah.");
    }
  }
}

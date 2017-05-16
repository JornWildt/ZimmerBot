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
  }
}

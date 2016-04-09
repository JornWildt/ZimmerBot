using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Tests.ConfigParser
{
  [TestFixture]
  public class RDFTests : TestHelper
  {
    [Test]
    public void CanLoadRDFFile()
    {
      KnowledgeBase kb = ParseKnowledgeBase(@"
! rdf_import ""ConfigParser/Friends.ttl""

> hvem er ven med +
! call RDF.Query(""
PREFIX pers: <http://wellknown-persons.fake#>
PREFIX foaf: <http://xmlns.com/foaf/0.1/>
SELECT * WHERE
{
  ?pers foaf:knows pers:A .
  ?pers foaf:name ?name
}
ORDER BY RAND()"")
: Det er '<result:{r | <r.name>}>'
");
      Assert.AreEqual(1, kb.Rules.Count);
      Rule r = kb.Rules[0];

      Reaction reaction = CalculateReaction(r, "hvem er ven med \"Peter Parker\"");
      Assert.IsNotNull(reaction);

      string response = reaction.GenerateResponse().Aggregate((a,b) => a + "\n" + b);
      Assert.AreEqual("Det er 'Lisa Nilson'", response);
    }


    [Test]
    public void CanDeclareCommonPrefixes()
    {
      KnowledgeBase kb = ParseKnowledgeBase(@"
! rdf_import ""ConfigParser/Friends.ttl""
! rdf_prefix foaf ""http://xmlns.com/foaf/0.1/""

> Who is friend with +
! call RDF.Query(""
PREFIX pers: <http://wellknown-persons.fake#>
SELECT * WHERE
{
  ?pers foaf:knows pers:A .
  ?pers foaf:name ?name
}
ORDER BY RAND()"")
: '<result:{r | <r.name>}>' is a friend
");
      Assert.AreEqual(1, kb.Rules.Count);
      Rule r = kb.Rules[0];

      Reaction reaction = CalculateReaction(r, "who is friend with \"Peter Parker\"");
      Assert.IsNotNull(reaction);

      string response = reaction.GenerateResponse().Aggregate((a, b) => a + "\n" + b);
      Assert.AreEqual("'Lisa Nilson' is a friend", response);
    }
  }
}

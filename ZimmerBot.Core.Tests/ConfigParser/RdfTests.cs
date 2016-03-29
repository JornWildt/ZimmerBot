﻿using System;
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
      Domain d = ParseDomain(@"
! rdf_import ""ConfigParser/Friends.ttl""

> hvem er ven med +
! call RDF.Query(""
PREFIX pers: <http://wellknown-persons.fake#>
PREFIX foaf: <http://xmlns.com/foaf/0.1/>
SELECT * WHERE
{
  ?pers foaf:knows pers:D .
  ?pers foaf:name ?name
}"")
: <result>
");
      Assert.AreEqual(1, d.Rules.Count);
      Rule r = d.Rules[0];

      Reaction reaction = CalculateReaction(r, "hvem er ven med \"Peter Parker\"");
      Assert.IsNotNull(reaction);

      string response = reaction.GenerateResponse();
      Assert.AreEqual("Det er Lisa Nilson", response);
    }
  }
}
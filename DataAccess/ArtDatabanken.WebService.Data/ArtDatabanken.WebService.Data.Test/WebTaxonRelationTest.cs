// -----------------------------------------------------------------------
// <copyright file="WebTaxonRelationTest.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [TestClass]
    public class WebTaxonRelationTest : WebDomainTestBase<WebTaxonRelation>
    {
        [TestMethod]
        public void Id()
        {
            var id = 1;
            GetObject(true).Id = id;
            Assert.AreEqual(GetObject().Id, id);
        }

        [TestMethod]
        public void RelatedTaxon()
        {
            var relatedTaxon = new WebTaxon() { Id = 1 };
            GetObject(true).ParentTaxonId = relatedTaxon.Id;
            Assert.AreEqual(GetObject().ParentTaxonId, relatedTaxon.Id);
        }

        [TestMethod]
        public void ValidFromDate()
        {
            var validFromDate = new DateTime(20110101);
            GetObject(true).ValidFromDate = validFromDate;
            Assert.AreEqual(GetObject().ValidFromDate, validFromDate);
        }

        [TestMethod]
        public void ValidToDate()
        {
            var validToDate = new DateTime(20110101);
            GetObject(true).ValidToDate = validToDate;
            Assert.AreEqual(GetObject().ValidToDate, validToDate);
        }
    }
}

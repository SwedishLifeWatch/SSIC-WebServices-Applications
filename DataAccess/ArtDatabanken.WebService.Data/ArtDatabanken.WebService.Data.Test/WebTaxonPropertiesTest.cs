// -----------------------------------------------------------------------
// <copyright file="WebTaxonPropertiesTest.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ArtDatabanken.WebService.Data.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Domain tests for WebTaxonProperties
    /// </summary>
    [TestClass]
    public class WebTaxonPropertiesTest : WebDomainTestBase<WebTaxonProperties>
    {
        [TestMethod]
        public void Id()
        {
            var value = 1;
            GetObject(true).Id = value;
            Assert.AreEqual(GetObject().Id, value);
        }

        [TestMethod]
        public void Taxon()
        {
            var value = new WebTaxon() { Id = 1 };
            GetObject(true).Taxon = value;
            Assert.AreEqual(GetObject().Taxon, value);
        }

        [TestMethod]
        public void TaxonCategory()
        {
            var value = new WebTaxonCategory() { Id = 1 };
            GetObject(true).TaxonCategory = value;
            Assert.AreEqual(GetObject().TaxonCategory, value);
        }

        [TestMethod]
        public void ValidFromDate()
        {
            var value = new DateTime(20100101);
            GetObject(true).ValidFromDate = value;
            Assert.AreEqual(GetObject().ValidFromDate, value);
        }

        [TestMethod]
        public void ValidToDate()
        {
            var value = new DateTime(20100101);
            GetObject(true).ValidToDate = value;
            Assert.AreEqual(GetObject().ValidToDate, value);
        }

        [TestMethod]
        public void ModifiedDate()
        {
            var value = new DateTime(20100101);
            GetObject(true).ModifiedDate = value;
            Assert.AreEqual(GetObject().ModifiedDate, value);
        }

        [TestMethod]
        public void MNodifiedBy()
        {
            var value = new WebUser() { Id = 1 };
            GetObject(true).ModifiedBy = value;
            Assert.AreEqual(GetObject().ModifiedBy, value);
        }
    }
}

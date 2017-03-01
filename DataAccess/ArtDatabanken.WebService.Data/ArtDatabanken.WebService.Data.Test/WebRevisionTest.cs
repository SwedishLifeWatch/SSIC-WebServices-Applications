// -----------------------------------------------------------------------
// <copyright file="WebRevisionTest.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [TestClass]
    public class WebRevisionTest : WebDomainTestBase<WebTaxonRevision>
    {
        [TestMethod]
        public void Id()
        {
            const int value = 1;
            GetObject(true).Id = value;
            Assert.AreEqual(GetObject().Id, value);
        }

        [TestMethod]
        public void Taxon()
        {
            var value = new WebTaxon() { Id = 1 };
            GetObject(true).RootTaxon = value;
            Assert.AreEqual(GetObject().RootTaxon, value);
        }

        [TestMethod]
        public void DescriptionString()
        {
            const string value = "descVal";
            GetObject(true).Description = value;
            Assert.AreEqual(GetObject().Description, value);
        }

        [TestMethod]
        public void ExpectedStartTime()
        {
            var value = DateTime.Now;
            GetObject(true).ExpectedStartDate = value;
            Assert.AreEqual(GetObject().ExpectedStartDate, value);
        }

        [TestMethod]
        public void ExpectedEndTime()
        {
            var value = DateTime.Now;
            GetObject(true).ExpectedEndDate = value;
            Assert.AreEqual(GetObject().ExpectedEndDate, value);
        }

        [TestMethod]
        public void GUID()
        {
            const string value = "GUID";
            GetObject(true).Guid = value;
            Assert.AreEqual(GetObject().Guid, value);
        }
    }
}

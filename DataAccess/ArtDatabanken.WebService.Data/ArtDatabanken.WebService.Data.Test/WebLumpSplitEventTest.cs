// -----------------------------------------------------------------------
// <copyright file="WebLumpSplitEventTest.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using ArtDatabanken.Data;
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
    public class WebLumpSplitEventTest : WebDomainTestBase<WebLumpSplitEvent>
    {
        [TestMethod]
        public void Id()
        {
            var value = 1;
            GetObject(true).Id = value;
            Assert.AreEqual(GetObject().Id, value);
        }

        [TestMethod]
        public void EventType()
        {
            LumpSplitEventTypeId value;

            value = LumpSplitEventTypeId.Lump;
            GetObject(true).TypeId = (Int32)(value);
            Assert.AreEqual(GetObject().TypeId, (Int32)(value));
        }

        [TestMethod]
        public void TaxonBefore()
        {
            WebTaxon value = new WebTaxon() { Id = 1 };
            GetObject(true).TaxonIdBefore = value.Id;
            Assert.AreEqual(GetObject().TaxonIdBefore, value.Id);            
        }

        [TestMethod]
        public void TaxonAfter()
        {
            WebTaxon value = new WebTaxon() { Id = 1 };
            GetObject(true).TaxonIdAfter = value.Id;
            Assert.AreEqual(GetObject().TaxonIdAfter, value.Id);
        }

        [TestMethod]
        public void CreatedDate()
        {
            var value = new DateTime(20100101);
            GetObject(true).CreatedDate = value;
            Assert.AreEqual(GetObject().CreatedDate, value);
        }

        [TestMethod]
        public void CreatedBy()
        {
            var value = 1;
            GetObject(true).CreatedBy = value;
            Assert.AreEqual(GetObject().CreatedBy, value);
        }

        [TestMethod]
        public void DescriptionString()
        {
            var value = "Descvdffdgdgfgdg";
            GetObject(true).Description = value;
            Assert.AreEqual(GetObject().Description, value);
        }
    }
}

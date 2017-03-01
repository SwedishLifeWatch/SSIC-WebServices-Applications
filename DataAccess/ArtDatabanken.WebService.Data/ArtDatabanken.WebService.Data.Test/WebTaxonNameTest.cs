// -----------------------------------------------------------------------
// <copyright file="WebTaxonNameTest.cs" company="Microsoft">
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
    public class WebTaxonNameTest : WebDomainTestBase<WebTaxonName>
    {
        [TestMethod]
        public void Id()
        {
            var id = 1;
            GetObject(true).Id = id;
            Assert.AreEqual(GetObject().Id, id);
        }

        [TestMethod]
        public void GUID()
        {
            var GUID = "GUID";
            GetObject(true).Guid = GUID;
            Assert.AreEqual(GetObject().Guid, GUID);
        }

        [TestMethod]
        public void Author()
        {
            var author = " Carl von Linne";
            GetObject(true).Author = author;
            Assert.AreEqual(GetObject().Author, author); 
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
        public void ModifiedDate()
        {
            var value = new DateTime(20100101);
            GetObject(true).ModifiedDate = value;
            Assert.AreEqual(GetObject().ModifiedDate, value);
        }

        [TestMethod]
        public void ModifiedBy()
        {
            var value = 1;
            GetObject(true).ModifiedBy = value;
            Assert.AreEqual(GetObject().ModifiedBy, value);
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
        public void Description()
        {
            var value = "Descvdffdgdgfgdg";
            GetObject(true).Description = value;
            Assert.AreEqual(GetObject().Description, value);            
        }
    }
}

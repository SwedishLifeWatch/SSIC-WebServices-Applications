using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Test all properties and constructor for WebTaxon class.
    /// </summary>
    [TestClass]
    public class WebTaxonTest : WebDomainTestBase<WebTaxon>
    {
        [TestMethod]
        public void Constructor()
        {
            WebTaxon webTaxon = new WebTaxon();
            Assert.IsNotNull(webTaxon);
        }

        [TestMethod]
        public void ConceptDefinitionPartStringId()
        {
            const string value = "TEST";
            GetObject(true).PartOfConceptDefinition = value;
            Assert.AreEqual(GetObject().PartOfConceptDefinition, value);
        }

        [TestMethod]
        public void CreatedBy()
        {
            const Int32 user = 2010;
            GetObject(true).CreatedBy = user;
            Assert.AreEqual(GetObject().CreatedBy,user);
        }

        [TestMethod]
        public void CreatedDate()
        {
            DateTime dateTime = new DateTime(2007,01,3);
            GetObject(true).CreatedDate = dateTime;
            Assert.AreEqual(GetObject().CreatedDate, dateTime);
        }


        [TestMethod]
        public void GUID()
        {
            String guid = null;

            GetObject(true).Guid = guid;
            Assert.IsNull(GetObject().Guid);

            guid = String.Empty;
            GetObject(true).Guid = guid;
            Assert.IsTrue(GetObject().Guid.IsEmpty());

            guid = "LKdakldf4422-sdf";
            GetObject(true).Guid = guid;
            Assert.AreEqual(GetObject().Guid, guid);
        }

        [TestMethod]
        public void Id()
        {
            const Int32 id = 20889;
            GetObject(true).Id = id;
            Assert.AreEqual(GetObject().Id, id);
        }

        [TestMethod]
        public void ValidFromDate()
        {
           DateTime dateTime = new DateTime(2010, 12, 24);
            GetObject(true).ValidFromDate = dateTime;
            Assert.AreEqual(GetObject().ValidFromDate, dateTime);
        }

        [TestMethod]
        public void ValidToDate()
        {
            DateTime dateTime = new DateTime(2016, 11, 1);
            GetObject(true).ValidToDate = dateTime;
            Assert.AreEqual(GetObject().ValidToDate, dateTime);
        }

  
    }
}

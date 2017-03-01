using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Test all properties and constructor for WebSpeciesObservationFieldMapping class.
    /// </summary>
    [TestClass]
    public class WebSpeciesObservationFieldMappingTest : WebDomainTestBase<WebSpeciesObservationFieldMapping>
    {
        [TestMethod]
        public void Constructor()
        {
            WebSpeciesObservationFieldMapping webSpeciesObservationFieldMapping = new WebSpeciesObservationFieldMapping();
            Assert.IsNotNull(webSpeciesObservationFieldMapping);
        }

        [TestMethod]
        public void Id()
        {
            const Int32 id = 20889;
            GetObject(true).Id = id;
            Assert.AreEqual(GetObject().Id, id);
        }

        [TestMethod]
        public void DataProviderId()
        {
            const Int32 id = 20889;
            GetObject(true).DataProviderId = id;
            Assert.AreEqual(GetObject().DataProviderId, id);
        }

        [TestMethod]
        public void FieldId()
        {
            const Int32 id = 20889;
            GetObject(true).FieldId = id;
            Assert.AreEqual(GetObject().FieldId, id);
        }

        [TestMethod]
        public void ProviderFieldName()
        {
            const String testString = "TaxonID";
            GetObject(true).ProviderFieldName = testString;
            Assert.AreEqual(GetObject().ProviderFieldName, testString);
        }

        [TestMethod]
        public void Method()
        {
            const String testString = "GetTaxonID";
            GetObject(true).Method = testString;
            Assert.AreEqual(GetObject().Method, testString);
        }

        [TestMethod]
        public void DefaultValue()
        {
            const String testString = "5000";
            GetObject(true).DefaultValue = testString;
            Assert.AreEqual(GetObject().DefaultValue, testString);
        }

        [TestMethod]
        public void Documentation()
        {
            const String testString = "This field is equal to TaxonId";
            GetObject(true).Documentation = testString;
            Assert.AreEqual(GetObject().Documentation, testString);
        }

        [TestMethod]
        public void IsImplemented()
        {
            const Boolean testBool = true;
            GetObject(true).IsImplemented = testBool;
            Assert.AreEqual(GetObject().IsImplemented, testBool);
        }

        [TestMethod]
        public void IsPlanned()
        {
            const Boolean testBool = true;
            GetObject(true).IsPlanned = testBool;
            Assert.AreEqual(GetObject().IsPlanned, testBool);
        }
    }
}

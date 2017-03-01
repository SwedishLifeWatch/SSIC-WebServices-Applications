using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class SpeciesObservationFieldMappingTest : TestBase
    {
        SpeciesObservationFieldMapping _mapping;

        public SpeciesObservationFieldMappingTest()
        {
            _mapping = null;
        }

        [TestMethod]
        public void Constructor()
        {
            SpeciesObservationFieldMapping mapping;

            mapping = new SpeciesObservationFieldMapping();
            Assert.IsNotNull(mapping);
        }

        public static SpeciesObservationFieldMapping GetOneMapping()
        {
            return new SpeciesObservationFieldMapping();
        }

        private SpeciesObservationFieldMapping GetMapping()
        {
            return GetMapping(false);
        }

        private SpeciesObservationFieldMapping GetMapping(Boolean refresh)
        {
            if (_mapping.IsNull() || refresh)
            {
                _mapping = new SpeciesObservationFieldMapping();
            }
            return _mapping;
        }

        [TestMethod]
        public void Id()
        {
            const Int32 id = 20889;
            GetMapping(true).Id = id;
            Assert.AreEqual(GetMapping().Id, id);
        }

        [TestMethod]
        public void DataSourceId()
        {
            const Int32 id = 20889;
            GetMapping(true).DataProviderId = id;
            Assert.AreEqual(GetMapping().DataProviderId, id);
        }

        [TestMethod]
        public void FieldId()
        {
            const Int32 id = 20889;
            GetMapping(true).FieldId = id;
            Assert.AreEqual(GetMapping().FieldId, id);
        }

        [TestMethod]
        public void ProviderFieldName()
        {
            const String testString = "TaxonID";
            GetMapping(true).ProviderFieldName = testString;
            Assert.AreEqual(GetMapping().ProviderFieldName, testString);
        }

        [TestMethod]
        public void Method()
        {
            const String testString = "GetTaxonID";
            GetMapping(true).Method = testString;
            Assert.AreEqual(GetMapping().Method, testString);
        }

        [TestMethod]
        public void DefaultValue()
        {
            const String testString = "5000";
            GetMapping(true).DefaultValue = testString;
            Assert.AreEqual(GetMapping().DefaultValue, testString);
        }

        [TestMethod]
        public void Documentation()
        {
            const String testString = "This field is equal to TaxonId";
            GetMapping(true).Documentation = testString;
            Assert.AreEqual(GetMapping().Documentation, testString);
        }

        [TestMethod]
        public void IsImplemented()
        {
            const Boolean testBool = true;
            GetMapping(true).IsImplemented = testBool;
            Assert.AreEqual(GetMapping().IsImplemented, testBool);
        }

        [TestMethod]
        public void IsPlanned()
        {
            const Boolean testBool = true;
            GetMapping(true).IsPlanned = testBool;
            Assert.AreEqual(GetMapping().IsPlanned, testBool);
        }
    }
}

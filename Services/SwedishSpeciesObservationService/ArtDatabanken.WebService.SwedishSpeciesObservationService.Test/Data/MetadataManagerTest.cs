using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Test.Data
{
    [TestClass]
    public class MetadataManagerTest : TestBase
    {
        private MetadataManager _metadataManager;

        public MetadataManagerTest()
        {
            _metadataManager = null;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetDarwinCoreFieldDescriptions()
        {
            List<WebSpeciesObservationFieldDescription> descriptions;

            descriptions = GetMetadataManager(true).GetSpeciesObservationFieldDescriptions(Context);
            Assert.IsTrue(descriptions.Count > 90);
            foreach (WebSpeciesObservationFieldDescription description in descriptions)
            {
                Assert.IsNotNull(description.Name);
                Assert.IsTrue(description.Name.IsNotEmpty());
                if (description.IsMandatoryFromProvider)
                {
                    Assert.IsTrue(description.Mappings.IsNotEmpty());
                }
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetDarwinCoreFieldDescriptionsCompareExtended()
        {
            List<WebSpeciesObservationFieldDescription> descriptions;
            List<WebSpeciesObservationFieldDescriptionExtended> descriptionsExtended;

            descriptions = GetMetadataManager(true).GetSpeciesObservationFieldDescriptions(Context);
            descriptionsExtended = GetMetadataManager().GetSpeciesObservationFieldDescriptionsExtended(Context);
            Assert.IsTrue(descriptions.Count <= descriptionsExtended.Count);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetDarwinCoreFieldDescriptionsExtended()
        {
            List<WebSpeciesObservationFieldDescriptionExtended> descriptions;

            descriptions = GetMetadataManager(true).GetSpeciesObservationFieldDescriptionsExtended(Context);
            Assert.IsTrue(descriptions.Count > 100);
            foreach (WebSpeciesObservationFieldDescriptionExtended description in descriptions)
            {
                Assert.IsTrue(description.Name.IsNotEmpty());
                if (description.IsMandatoryFromProvider)
                {
                    Assert.IsTrue(description.Mappings.IsNotEmpty());
                }
            }
        }

        private MetadataManager GetMetadataManager(Boolean refresh = false)
        {
            if (_metadataManager.IsNull() || refresh)
            {
                _metadataManager = new MetadataManager();
            }

            return _metadataManager;
        }
    }
}

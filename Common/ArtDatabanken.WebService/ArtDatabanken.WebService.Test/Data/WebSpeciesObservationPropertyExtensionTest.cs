using System;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Test.Data
{
    [TestClass]
    public class WebSpeciesObservationPropertyExtensionTest
    {
        private WebSpeciesObservationProperty _speciesObservationProperty;

        public WebSpeciesObservationPropertyExtensionTest()
        {
            _speciesObservationProperty = null;
        }

        [TestMethod]
        public void GetProperty()
        {
            String newProperty;

            newProperty = "test property";
            GetSpeciesObservationProperty(true);
            foreach (SpeciesObservationPropertyId speciesObservationPropertyId in Enum.GetValues(typeof(SpeciesObservationPropertyId)))
            {
                GetSpeciesObservationProperty().Id = speciesObservationPropertyId;
                GetSpeciesObservationProperty().Identifier = newProperty;
                Assert.IsTrue(GetSpeciesObservationProperty().GetProperty().IsNotEmpty());
                if (speciesObservationPropertyId == SpeciesObservationPropertyId.None)
                {
                    Assert.AreEqual(newProperty, GetSpeciesObservationProperty().GetProperty());
                }
                else
                {
                    Assert.AreEqual(speciesObservationPropertyId.ToString(), GetSpeciesObservationProperty().GetProperty());
                }
            }
        }

        private WebSpeciesObservationProperty GetSpeciesObservationProperty(Boolean refresh = false)
        {
            if (_speciesObservationProperty.IsNull() || refresh)
            {
                _speciesObservationProperty = new WebSpeciesObservationProperty();
            }
            return _speciesObservationProperty;
        }
    }
}

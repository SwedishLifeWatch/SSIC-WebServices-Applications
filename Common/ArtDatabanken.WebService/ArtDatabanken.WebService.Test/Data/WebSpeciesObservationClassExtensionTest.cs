using System;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Test.Data
{
    [TestClass]
    public class WebSpeciesObservationClassExtensionTest
    {
        private WebSpeciesObservationClass _speciesObservationClass;

        public WebSpeciesObservationClassExtensionTest()
        {
            _speciesObservationClass = null;
        }

        [TestMethod]
        public void GetClass()
        {
            String newClass;

            newClass = "test class";
            GetSpeciesObservationClass(true);
            foreach (SpeciesObservationClassId speciesObservationClassId in Enum.GetValues(typeof(SpeciesObservationClassId)))
            {
                GetSpeciesObservationClass().Id = speciesObservationClassId;
                GetSpeciesObservationClass().Identifier = newClass;
                Assert.IsTrue(GetSpeciesObservationClass().GetClass().IsNotEmpty());
                if (speciesObservationClassId == SpeciesObservationClassId.None)
                {
                    Assert.AreEqual(newClass, GetSpeciesObservationClass().GetClass());
                }
                else
                {
                    Assert.AreEqual(speciesObservationClassId.ToString(), GetSpeciesObservationClass().GetClass());
                }
            }
        }

        private WebSpeciesObservationClass GetSpeciesObservationClass(Boolean refresh = false)
        {
            if (_speciesObservationClass.IsNull() || refresh)
            {
                _speciesObservationClass = new WebSpeciesObservationClass();
            }
            return _speciesObservationClass;
        }
    }
}

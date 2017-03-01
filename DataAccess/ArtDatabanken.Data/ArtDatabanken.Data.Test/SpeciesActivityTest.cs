using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.Data.Test
{
    [TestClass]
    public class SpeciesAcitvityTest
    {
        SpeciesActivity _speciesActivity;

        public void SpeciesActivityTest()
        {
            _speciesActivity = null;
        }

        [TestMethod]
        public void Constructor()
        {
            SpeciesActivity speciesActivity;

            speciesActivity = new SpeciesActivity();
            Assert.IsNotNull(speciesActivity);
        }

       

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNull(GetSpeciesActivity(true).DataContext);
        }

        private SpeciesActivity GetSpeciesActivity()
        {
            return GetSpeciesActivity(false);
        }

        private SpeciesActivity GetSpeciesActivity(Boolean refresh)
        {
            if (_speciesActivity.IsNull() || refresh)
            {
                _speciesActivity = new SpeciesActivity();
            }
            return _speciesActivity;
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = GetSpeciesActivity(true).Id;
            Assert.AreEqual(id, GetSpeciesActivity().Id);
        }
    }
}

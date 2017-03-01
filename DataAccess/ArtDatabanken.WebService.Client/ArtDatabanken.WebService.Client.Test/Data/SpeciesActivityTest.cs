using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class SpeciesActivityTest : TestBase
    {
        SpeciesActivity _speciesActivity;

        public SpeciesActivityTest()
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
            Assert.IsNotNull(GetSpeciesActivity(true).DataContext);
        }



        private SpeciesActivity GetSpeciesActivity(Boolean refresh)
        {
            if (_speciesActivity.IsNull() || refresh)
            {
                _speciesActivity = new SpeciesActivity();
            }
            return _speciesActivity;
        }

        public static SpeciesActivity GetOneSpeciesActivity(IUserContext userContext)
        {
            return new SpeciesActivity();
        }
    }
}

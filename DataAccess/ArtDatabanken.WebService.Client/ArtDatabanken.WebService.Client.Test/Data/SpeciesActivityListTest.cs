using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class SpeciesActivitiesTest : TestBase
    {
        private SpeciesActivityList _speciesActivities;

        public SpeciesActivitiesTest()
        {
            _speciesActivities = null;
        }

        [TestMethod]
        public void Constructor()
        {
            SpeciesActivityList speciesActivities;

            speciesActivities = new SpeciesActivityList();
            Assert.IsNotNull(speciesActivities);
        }

        [TestMethod]
        public void Get()
        {
            GetSpeciesActivities(true);
            foreach (ISpeciesActivity speciesActivity in GetSpeciesActivities())
            {
                Assert.AreEqual(speciesActivity, GetSpeciesActivities().Get(speciesActivity.Id));
            }
        }

        private SpeciesActivityList GetSpeciesActivities()
        {
            return GetSpeciesActivities(false);
        }

        private SpeciesActivityList GetSpeciesActivities(Boolean refresh)
        {
            if (_speciesActivities.IsNull() || refresh)
            {
                _speciesActivities = new SpeciesActivityList();
                _speciesActivities.Add(SpeciesActivityTest.GetOneSpeciesActivity(GetUserContext()));
            }
            return _speciesActivities;
        }

    }
}

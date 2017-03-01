using System.Collections.Generic;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Test.Data
{
    using SpeciesActivityManager = ArtDatabanken.WebService.SwedishSpeciesObservationService.Data.SpeciesActivityManager;

    [TestClass]
    public class SpeciesActivityManagerTest : TestBase
    {
        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetBirdNestActivities()
        {
            List<WebSpeciesActivity> birdNestActivities;

            birdNestActivities = SpeciesActivityManager.GetBirdNestActivities(Context);
            Assert.IsTrue(birdNestActivities.IsNotEmpty());
            foreach (WebSpeciesActivity birdNestActivity in birdNestActivities)
            {
                Assert.IsTrue(SwedishSpeciesObservationService.Settings.Default.BirdNestActivityIdMin <= birdNestActivity.Id);
                Assert.IsTrue(birdNestActivity.Id <= SwedishSpeciesObservationService.Settings.Default.BirdNestActivityIdMax);
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesActivities()
        {
            List<WebSpeciesActivity> speciesActivities;

            speciesActivities = SpeciesActivityManager.GetSpeciesActivities(Context);
            Assert.IsTrue(speciesActivities.IsNotEmpty());
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesActivityCategories()
        {
            List<WebSpeciesActivityCategory> speciesActivityCategories;

            speciesActivityCategories = SpeciesActivityManager.GetSpeciesActivityCategories(Context);
            Assert.IsTrue(speciesActivityCategories.IsNotEmpty());
        } 
    }
}

using System;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeciesObservationDataSource = ArtDatabanken.WebService.Client.SpeciesObservationService.SpeciesObservationDataSource;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class SpeciesObservationManagerMultiThreadCacheTest : TestBase
    {
        private SpeciesObservationManagerMultiThreadCache _speciesObservationManager;

        public SpeciesObservationManagerMultiThreadCacheTest()
        {
            _speciesObservationManager = null;
        }

        [TestMethod]
        public void GetBirdNestActivities()
        {
            Int64 durationFirst, durationSecond;
            SpeciesActivityList birdNestActivities;

            Stopwatch.Start();
            birdNestActivities = GetSpeciesObservationManager(true).GetBirdNestActivities(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(birdNestActivities.IsNotEmpty());

            Stopwatch.Start();
            birdNestActivities = GetSpeciesObservationManager().GetBirdNestActivities(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(birdNestActivities.IsNotEmpty());

            Assert.IsTrue(durationSecond < (durationFirst / 4));
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesActivities()
        {
            Int64 durationFirst, durationSecond;
            SpeciesActivityList speciesActivities;

            Stopwatch.Start();
            speciesActivities = GetSpeciesObservationManager(true).GetSpeciesActivities(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(speciesActivities.IsNotEmpty());

            Stopwatch.Start();
            speciesActivities = GetSpeciesObservationManager().GetSpeciesActivities(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(speciesActivities.IsNotEmpty());

            Assert.IsTrue(durationSecond < (durationFirst / 4));
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesActivityCategories()
        {
            Int64 durationFirst, durationSecond;
            SpeciesActivityCategoryList speciesActivityCategories;

            Stopwatch.Start();
            speciesActivityCategories = GetSpeciesObservationManager(true).GetSpeciesActivityCategories(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(speciesActivityCategories.IsNotEmpty());

            Stopwatch.Start();
            speciesActivityCategories = GetSpeciesObservationManager().GetSpeciesActivityCategories(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(speciesActivityCategories.IsNotEmpty());

            Assert.IsTrue(durationSecond < (durationFirst / 4));
        }

        [TestMethod]
        public void GetSpeciesObservationDataProviders()
        {
            Int64 durationFirst, durationSecond;
            ArtDatabanken.Data.SpeciesObservationDataProviderList speciesObservationDataProviders;

            Stopwatch.Start();
            speciesObservationDataProviders = GetSpeciesObservationManager(true).GetSpeciesObservationDataProviders(GetUserContext());
            Stopwatch.Stop();
            durationFirst = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(speciesObservationDataProviders.IsNotEmpty());

            Stopwatch.Start();
            speciesObservationDataProviders = GetSpeciesObservationManager().GetSpeciesObservationDataProviders(GetUserContext());
            Stopwatch.Stop();
            durationSecond = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            Assert.IsTrue(speciesObservationDataProviders.IsNotEmpty());
             
            Assert.IsTrue(durationSecond < (durationFirst / 6));
         }

         private SpeciesObservationManagerMultiThreadCache GetSpeciesObservationManager(Boolean refresh = false)
         {
             if (_speciesObservationManager.IsNull() || refresh)
             {
                 _speciesObservationManager = new SpeciesObservationManagerMultiThreadCache();
                 _speciesObservationManager.DataSource = new SpeciesObservationDataSource();
             }
             return _speciesObservationManager;
         }

        
    }
}

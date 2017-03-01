using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class BirdNestActivityTest : TestBase
    {
        private Data.ArtDatabankenService.BirdNestActivity _birdNestActivity;

        public BirdNestActivityTest()
        {
            _birdNestActivity = null;
        }

        #region Additional test attributes
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion

        [TestMethod]
        public void Constructor()
        {
            Data.ArtDatabankenService.BirdNestActivity birdNestActivity;

            birdNestActivity = GetBirdNestActivity(true);
            Assert.IsNotNull(birdNestActivity);
        }

        private Data.ArtDatabankenService.BirdNestActivity GetBirdNestActivity()
        {
            return GetBirdNestActivity(false);
        }

        private Data.ArtDatabankenService.BirdNestActivity GetBirdNestActivity(Boolean refresh)
        {
            if (_birdNestActivity.IsNull() || refresh)
            {
                _birdNestActivity = SpeciesObservationManagerTest.GetOneBirdNestActivity();
            }
            return _birdNestActivity;
        }

        [TestMethod]
        public void Name()
        {
            Assert.IsTrue(GetBirdNestActivity(true).Name.IsNotEmpty());
        }
    }
}

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class BirdNestActivityListTest : TestBase
    {
        private Data.ArtDatabankenService.BirdNestActivityList _birdNestActivities;

        public BirdNestActivityListTest()
        {
            _birdNestActivities = null;
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
        public void Get()
        {
            foreach (BirdNestActivity birdNestActivity in GetBirdNestActivities(true))
            {
                Assert.AreEqual(birdNestActivity, GetBirdNestActivities().Get(birdNestActivity.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 birdNestActivityId;

            birdNestActivityId = Int32.MinValue;
            GetBirdNestActivities(true).Get(birdNestActivityId);
        }

        private Data.ArtDatabankenService.BirdNestActivityList GetBirdNestActivities()
        {
            return GetBirdNestActivities(false);
        }

        private Data.ArtDatabankenService.BirdNestActivityList GetBirdNestActivities(Boolean refresh)
        {
            if (_birdNestActivities.IsNull() || refresh)
            {
                _birdNestActivities = SpeciesObservationManagerTest.GetAllBirdNestActivities();
            }
            return _birdNestActivities;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            Int32 birdNestActivityIndex;
            Data.ArtDatabankenService.BirdNestActivityList newBirdNestActivityList, oldBirdNestActivityList;

            oldBirdNestActivityList = GetBirdNestActivities(true);
            newBirdNestActivityList = new Data.ArtDatabankenService.BirdNestActivityList();
            for (birdNestActivityIndex = 0; birdNestActivityIndex < oldBirdNestActivityList.Count; birdNestActivityIndex++)
            {
                newBirdNestActivityList.Add(oldBirdNestActivityList[oldBirdNestActivityList.Count - birdNestActivityIndex - 1]);
            }
            for (birdNestActivityIndex = 0; birdNestActivityIndex < oldBirdNestActivityList.Count; birdNestActivityIndex++)
            {
                Assert.AreEqual(newBirdNestActivityList[birdNestActivityIndex], oldBirdNestActivityList[oldBirdNestActivityList.Count - birdNestActivityIndex - 1]);
            }
        }
    }
}

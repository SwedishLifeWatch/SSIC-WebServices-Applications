using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebSpeciesObservationChangeTest : TestBase
    {
        private WebSpeciesObservationChange _change;

        public WebSpeciesObservationChangeTest()
            : base(false, 0)
        {
            _change = null;
        }

        [TestMethod]
        public void Constructor()
        {
            DateTime changedFrom, changedTo;
            Int32 maxProtectionLevel;
            WebSpeciesObservationChange change;

            // Get some changes.
            changedFrom = new DateTime(2011, 2, 1, 0, 0, 0);
            changedTo = new DateTime(2011, 2, 2, 0, 0, 0);
            maxProtectionLevel = 2;
            using (DataReader dataReader = DataServer.GetSpeciesObservationChange(GetContext(), maxProtectionLevel, changedFrom, changedTo))
            {
                change = new WebSpeciesObservationChange(dataReader);
            }
            Assert.IsNotNull(change);
            Assert.IsTrue(change.NewSpeciesObservations.IsNotEmpty());
            Assert.IsTrue(change.NewSpeciesObservationIds.IsEmpty());

            // Get many changes.
            changedFrom = new DateTime(2011, 2, 1, 0, 0, 0);
            changedTo = new DateTime(2011, 2, 3, 0, 0, 0);
            maxProtectionLevel = 2;
            using (DataReader dataReader = DataServer.GetSpeciesObservationChange(GetContext(), maxProtectionLevel, changedFrom, changedTo))
            {
                change = new WebSpeciesObservationChange(dataReader);
            }
            Assert.IsNotNull(change);
            Assert.IsTrue(change.NewSpeciesObservations.IsEmpty());
            Assert.IsTrue(change.NewSpeciesObservationIds.IsNotEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        [Ignore]
        public void ConstructorToManyChangesError()
        {
            DateTime changedFrom, changedTo;
            Int32 maxProtectionLevel;
            WebSpeciesObservationChange change;

            changedFrom = new DateTime(2011, 2, 1, 0, 0, 0);
            changedTo = new DateTime(2011, 2, 28, 0, 0, 0);
            maxProtectionLevel = 2;
            using (DataReader dataReader = DataServer.GetSpeciesObservationChange(GetContext(), maxProtectionLevel, changedFrom, changedTo))
            {
                change = new WebSpeciesObservationChange(dataReader);
            }
            Assert.IsNotNull(change);
        }

        [TestMethod]
        public void DeletedSpeciesObservationCount()
        {
            Assert.IsTrue(0 < GetSpeciesObservationChange().DeletedSpeciesObservationCount);
        }

        [TestMethod]
        public void DeletedSpeciesObservationGuids()
        {
            Assert.IsTrue(GetSpeciesObservationChange().DeletedSpeciesObservationGuids.IsNotEmpty());
        }

        private WebSpeciesObservationChange GetSpeciesObservationChange()
        {
            return GetSpeciesObservationChange(false);
        }

        private WebSpeciesObservationChange GetSpeciesObservationChange(Boolean refresh)
        {
            DateTime changedFrom, changedTo;
            Int32 maxProtectionLevel;

            if (_change.IsNull() || refresh)
            {
                changedFrom = new DateTime(2011, 2, 1, 0, 0, 0);
                changedTo = new DateTime(2011, 2, 2, 0, 0, 0);
                maxProtectionLevel = 2;
                using (DataReader dataReader = DataServer.GetSpeciesObservationChange(GetContext(), maxProtectionLevel, changedFrom, changedTo))
                {
                    _change = new WebSpeciesObservationChange(dataReader);
                }
            }
            return _change;
        }

        [TestMethod]
        public void MaxSpeciesObservationCount()
        {
            Assert.AreEqual(ArtDatabankenService.Settings.Default.MaxSpeciesObservationWithInformation / 10,
                            GetSpeciesObservationChange().MaxSpeciesObservationCount);
        }

        [TestMethod]
        public void NewSpeciesObservationCount()
        {
            Assert.IsTrue(0 < GetSpeciesObservationChange().NewSpeciesObservationCount);
        }

        [TestMethod]
        public void NewSpeciesObservationIds()
        {
            Assert.IsTrue(GetSpeciesObservationChange().NewSpeciesObservationIds.IsEmpty());
        }

        [TestMethod]
        public void NewSpeciesObservations()
        {
            Assert.IsTrue(GetSpeciesObservationChange().NewSpeciesObservations.IsNotEmpty());
        }

        [TestMethod]
        public void UpdatedSpeciesObservationCount()
        {
            Assert.IsTrue(0 < GetSpeciesObservationChange().UpdatedSpeciesObservationCount);
        }

        [TestMethod]
        public void UpdatedSpeciesObservationIds()
        {
            Assert.IsTrue(GetSpeciesObservationChange().UpdatedSpeciesObservationIds.IsEmpty());
        }

        [TestMethod]
        public void UpdatedSpeciesObservations()
        {
            Assert.IsTrue(GetSpeciesObservationChange().UpdatedSpeciesObservations.IsNotEmpty());
        }
    }
}

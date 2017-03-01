using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;
using ArtDatabanken.WebService.ArtDatabankenService.Database;
using ArtDatabanken.WebService.ArtDatabankenService.Test.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebSpeciesObservationTest : TestBase
    {
        private WebSpeciesObservation _speciesObservation;

        public WebSpeciesObservationTest()
        {
            ApplicationIdentifier = ArtDatabankenService.Data.ApplicationIdentifier.PrintObs.ToString();
            _speciesObservation = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebSpeciesObservation speciesObservation;

            using (DataReader dataReader = DataServerTest.GetSpeciesObservationsDataReader(GetContext()))
            {
                while (dataReader.Read())
                {
                    speciesObservation = new WebSpeciesObservation(dataReader);
                    Assert.IsNotNull(speciesObservation);
                }
            }
        }

        [TestMethod]
        public void GetGuid()
        {
            String guid;

            using (DataReader dataReader = DataServerTest.GetSpeciesObservationsDataReader(GetContext()))
            {
                while (dataReader.Read())
                {
                    guid = WebSpeciesObservation.GetGuid(dataReader);
                    Assert.IsTrue(guid.IsNotEmpty());
                }
            }
        }

        private WebSpeciesObservation GetSpeciesObservation()
        {
            return GetSpeciesObservation(false);
        }

        private WebSpeciesObservation GetSpeciesObservation(Boolean refresh)
        {
            if (_speciesObservation.IsNull() || refresh)
            {
                _speciesObservation = SpeciesObservationManagerTest.GetOneSpeciesObservation(GetContext());
            }
            return _speciesObservation;
        }

        [TestMethod]
        public void Id()
        {
            Int64 id;

            id = 423234342344234;
            GetSpeciesObservation(true).Id = id;
            Assert.AreEqual(id, GetSpeciesObservation().Id);
        }
    }
}

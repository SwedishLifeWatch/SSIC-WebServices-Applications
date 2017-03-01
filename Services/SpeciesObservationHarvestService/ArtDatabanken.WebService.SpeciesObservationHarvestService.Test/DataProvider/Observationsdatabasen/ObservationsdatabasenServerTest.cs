using System;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Observationsdatabasen;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.DataProvider.Observationsdatabasen
{
    [TestClass]
    public class ObservationsdatabasenServerTest : TestBase
    {
        [TestMethod]
        public void GetSpeciesObservations()
        {
            ObservationsdatabasenServer observationsdatabasenServer = new ObservationsdatabasenServer();
            DateTime changedTo = new DateTime(2010, 09, 26);
            DateTime changedFrom = new DateTime(2010, 09, 28);
            using (DataReader dataReader = observationsdatabasenServer.GetSpeciesObservations(changedTo, changedFrom))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }
    }
}

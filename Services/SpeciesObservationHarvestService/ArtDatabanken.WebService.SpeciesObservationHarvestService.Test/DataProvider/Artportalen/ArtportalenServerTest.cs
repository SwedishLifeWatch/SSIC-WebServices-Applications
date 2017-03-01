using System;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Artportalen;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.DataProvider.Artportalen
{
    [TestClass]
    public class ArtportalenServerTest : TestBase
    {
        [TestMethod]
        public void GetDeletedObservations()
        {
            ArtportalenServer artportalenServer = new ArtportalenServer();
            DateTime changedTo = new DateTime(2012, 10, 15); 
            DateTime changedFrom = new DateTime(2012, 10, 30);
            using (DataReader dataReader = artportalenServer.GetDeletedObservations(changedTo, changedFrom))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }
    }
}

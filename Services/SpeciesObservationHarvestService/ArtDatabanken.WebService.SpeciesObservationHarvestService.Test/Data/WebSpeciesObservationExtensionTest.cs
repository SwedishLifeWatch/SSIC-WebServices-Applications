using System;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Artportalen;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.Data
{
    [TestClass]
    public class WebSpeciesObservationExtensionTest : TestBase
    {
        [TestMethod]
        public void LoadData()
        {
            ArtportalenServer artportalenServer = new ArtportalenServer();

            DateTime changedTo = DateTime.Now;
            DateTime changedFrom = DateTime.Now.AddDays(-1);

            using (DataReader dataReader = artportalenServer.GetSpeciesObservations(changedFrom, changedTo))
            {
                WebData webData = new WebSpeciesObservation();
                Assert.IsTrue(dataReader.Read());

                webData.LoadData(dataReader);
                Assert.IsTrue(webData.DataFields.IsNotEmpty());
            }
        }
    }
}

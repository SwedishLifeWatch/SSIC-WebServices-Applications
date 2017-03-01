using ArtDatabanken.Database;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using ArtDatabanken.WebService.SwedishSpeciesObservationService.Data;
using ArtDatabanken.WebService.SwedishSpeciesObservationService.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Test.Data
{
    [TestClass]
    public class TaxonInformationTest
    {
        [TestMethod]
        [TestCategory("NightlyTest")]
        public void LoadData()
        {
            TaxonInformation taxonInformation;

            using (SpeciesObservationServer database = new SpeciesObservationServer())
            {
                using (DataReader dataReader = database.GetTaxonInformation())
                {
                    Assert.IsTrue(dataReader.Read());
                    taxonInformation = new TaxonInformation();
                    taxonInformation.LoadData(dataReader);
                    Assert.IsNotNull(taxonInformation);
                }
            }
        }
    }
}

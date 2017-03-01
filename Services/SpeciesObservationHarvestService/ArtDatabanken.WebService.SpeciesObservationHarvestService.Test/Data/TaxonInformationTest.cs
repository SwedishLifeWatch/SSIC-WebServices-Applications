using ArtDatabanken.Database;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.Data
{
    [TestClass]
    public class TaxonInformationTest
    {
        [TestMethod]
        public void LoadData()
        {
            TaxonInformation taxonInformation;

            using (SpeciesObservationHarvestServer database = new SpeciesObservationHarvestServer())
            {
                using (DataReader dataReader = database.GetTaxon())
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

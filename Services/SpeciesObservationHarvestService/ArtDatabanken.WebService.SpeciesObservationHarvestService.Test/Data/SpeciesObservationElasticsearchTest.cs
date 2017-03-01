using ArtDatabanken.Database;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.Data
{
    [TestClass]
    public class SpeciesObservationElasticsearchTest : TestBase
    {
        [TestMethod]
        public void LoadData()
        {
            SpeciesObservationElasticsearch speciesObservationElasticsearch;

            speciesObservationElasticsearch = new SpeciesObservationElasticsearch();
            using (DataReader dataReader = GetContext().GetSpeciesObservationDatabase().GetSpeciesObservationElasticsearch())
            {
                if (dataReader.Read())
                {
                    speciesObservationElasticsearch.LoadData(dataReader);
                }
            }

            Assert.IsNotNull(speciesObservationElasticsearch);
            Assert.IsTrue(speciesObservationElasticsearch.CurrentIndexChangeId > 0);
            Assert.IsTrue(speciesObservationElasticsearch.CurrentIndexCount > 0);
            Assert.IsTrue(speciesObservationElasticsearch.CurrentIndexName.IsNotEmpty());
        }
    }
}

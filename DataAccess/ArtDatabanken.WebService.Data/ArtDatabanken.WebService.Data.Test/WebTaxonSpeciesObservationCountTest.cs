using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    [TestClass]
    public class WebTaxonSpeciesObservationCountTest : WebDomainTestBase<WebTaxonSpeciesObservationCount>
    {
        [TestMethod]
        public void SpeciesObservationCount()
        {
            var speciesObservationCount = 10;

            GetObject(true).SpeciesObservationCount = speciesObservationCount;
            Assert.AreEqual(GetObject().SpeciesObservationCount, speciesObservationCount);
        }
    }
}
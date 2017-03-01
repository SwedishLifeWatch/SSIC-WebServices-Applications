using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.Data.Test
{
    [TestClass]
    public class SpeciesObservationProvenanceTest
    {
        [TestMethod]
        public void Constructor_ExpectsInstanciatedList()
        {
            // Arrange
            SpeciesObservationProvenance provenance;
            
            // Act
            provenance = new SpeciesObservationProvenance();
            
            // Assert
            Assert.IsNotNull(provenance.Values, "Constructor needs to instanciate the property Values.");
        }
    }
}

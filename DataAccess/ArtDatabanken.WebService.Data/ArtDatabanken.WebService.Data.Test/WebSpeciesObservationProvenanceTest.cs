using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    [TestClass]
    public class WebSpeciesObservationProvenanceTest
    {
        [TestMethod]
        public void Constructor_ExpectsInstanciatedList()
        {
            // Arrange
            WebSpeciesObservationProvenance provenance;

            // Act
            provenance = new WebSpeciesObservationProvenance();
            provenance.Values = new List<WebSpeciesObservationProvenanceValue>();

            // Assert
            Assert.IsNotNull(provenance.Values, "Constructor needs to instanciate the property Values.");
        }
    }
}

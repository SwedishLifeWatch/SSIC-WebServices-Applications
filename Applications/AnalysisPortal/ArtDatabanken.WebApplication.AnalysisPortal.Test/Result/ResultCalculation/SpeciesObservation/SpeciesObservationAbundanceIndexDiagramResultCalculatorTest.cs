using System.Collections.Concurrent;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultModels;
using ArtDatabanken.WebApplication.AnalysisPortal.Test.TestModels;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.Result.ResultCalculation.SpeciesObservation
{
    [TestClass]
    public class SpeciesObservationAbundanceIndexDiagramResultCalculatorTest : TestBase
    {
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetResult_Add1TaxonToFilter_Always0AbundanceIndexForSpecificTaxaPerYear()
        {
            // Arrange
            SpeciesObservationAbundanceIndexDiagramResultCalculator resultCalculator;
            List<AbundanceIndexData> result;

            LoginApplicationUser();
            SessionHandler.MySettings.Filter.Taxa.AddTaxonId(5);
            resultCalculator = new SpeciesObservationAbundanceIndexDiagramResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);
            // Act
            result = resultCalculator.CalculateAbundanceIndex(0, SessionHandler.MySettings.Filter.Taxa.TaxonIds[0]);
            
            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            foreach (AbundanceIndexData abundanceIndexData in result)
            {
                Assert.IsNull(abundanceIndexData.AbundanceIndex); // the logarithmic value (Value) is always Null if only 1 taxon is selected
                Assert.IsTrue(!string.IsNullOrEmpty(abundanceIndexData.TimeStep));
            }
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetResult_AddMultipleTaxonToFilter_ReturnedValueIsDouble_PerYear()
        {
            // Arrange
            SpeciesObservationAbundanceIndexDiagramResultCalculator resultCalculator;
            List<AbundanceIndexData> result;

            LoginApplicationUser();
            SessionHandler.MySettings.Filter.Taxa.AddTaxonIds(new List<int> { 5, 6, 8, 14, 22, 25 });
            resultCalculator = new SpeciesObservationAbundanceIndexDiagramResultCalculator(SessionHandler.UserContext, SessionHandler.MySettings);
            // Act
            result = resultCalculator.CalculateAbundanceIndex(0, SessionHandler.MySettings.Filter.Taxa.TaxonIds[0]);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            foreach (AbundanceIndexData abundanceIndexData in result)
            {
                if (abundanceIndexData.AbundanceIndex.HasValue)
                {
                    Assert.IsTrue(abundanceIndexData.AbundanceIndex > -2); // Value contains the logarithmic value, it seems it is always greater than -2                
                }
            }
        }
    }
}
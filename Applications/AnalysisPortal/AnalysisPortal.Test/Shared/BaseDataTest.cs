using System;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Observations;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace AnalysisPortal.Tests
{
    /// <summary>
    /// Base class for some data ie tests that will be used by many test cases.
    /// </summary>
    [TestClass]
    public static class BaseDataTest
    {
        
    #region Helper methods

        /// <summary>
        /// Verify correctness of species observation data for taxonid 100573.
        /// </summary>
        /// <param name="observationListResult"></param>
        internal static void VerifyObservationDataForGriffelblomfluga1000573(List<Dictionary<string, string>> observationListResult, int index1 = 0, int index2 = 6, bool additionalTest = true)
        {
            Dictionary<string, string> obsData1 = observationListResult[index1];
            Dictionary<string, string> obsData2 = observationListResult[index2];
            Assert.AreEqual("Ceriana conopsoides", obsData1["ScientificName"]);
            Assert.AreEqual("Ceriana conopsoides", obsData2["ScientificName"]);
            Assert.AreEqual("griffelblomfluga", obsData1["VernacularName"]);
            Assert.AreEqual("griffelblomfluga", obsData2["VernacularName"]);
            

        }

        #endregion
    }
}

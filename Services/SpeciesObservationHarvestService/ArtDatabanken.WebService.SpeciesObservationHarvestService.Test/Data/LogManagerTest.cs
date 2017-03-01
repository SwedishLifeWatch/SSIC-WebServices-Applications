using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using LogManager = ArtDatabanken.WebService.SpeciesObservationHarvestService.Data.LogManager;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.Data
{
    [TestClass]
    public class LogManagerTest : TestBase
    {
        [TestMethod]
        public void GetLog_FromStatisticLog_ExpectedObjectInReturn()
        {
            // Arrange
            LogManager logManager = new LogManager();
            String userName = "";
            Int32 rowCount = 10;

            // Act
            var actual = logManager.GetLog(this.GetContext(), LogType.SpeciesObservationStatistic, userName, rowCount);

            // Assert
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void GetLog_FromUpdateErrorLog_ExpectedObjectInReturn()
        {
            // Arrange
            LogManager logManager = new LogManager();
            String userName = "";
            Int32 rowCount = 10;

            // Act
            var actual = logManager.GetLog(this.GetContext(), LogType.SpeciesObservationUpdate, userName, rowCount);

            // Assert
            Assert.IsNotNull(actual);
        }
    }
}

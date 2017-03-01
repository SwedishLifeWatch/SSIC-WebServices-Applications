using System;
using System.Collections.Generic;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HarvestManager = ArtDatabanken.WebService.SpeciesObservationHarvestService.Data.HarvestManager;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.Data
{
    [TestClass]
    public class HarvestJobManagerTest : TestBase
    {
        public HarvestJobManagerTest()
            : base(USE_TRANSACTION, 50)
        {
        }

        #region Additional test attributes

        private const Boolean USE_TRANSACTION = false;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        #endregion

        /// <summary>
        /// Counter holding index for rows.
        /// </summary>
        private static int harvestJobCountIndex = 0;

        /// <summary>
        /// Counter holding index for data reader column index.
        /// </summary>
        private static int harvestJobColumnCounter = 1;

        /// <summary>
        /// Counter holding index for rows.
        /// </summary>
        private static int harvestJobDataProviderCountIndex = 0;

        /// <summary>
        /// Counter holding index for data reader column index.
        /// </summary>
        private static int harvestJobDataProviderColumnCounter = 1;

        [TestCleanup]
        public override void TestCleanup()
        {
            harvestJobCountIndex = 0;
            harvestJobColumnCounter = 1;
            harvestJobDataProviderCountIndex = 0;
            harvestJobDataProviderColumnCounter = 1;

            base.TestCleanup();
        }

        [TestMethod]
        public void StartSpeciesObservationUpdate_ParametersHasValues_ExpectHarvestFromStartDateToEndDateForSpecifiedDataProviders()
        {
            DateTime startDate = new DateTime(2014, 2, 27);
            DateTime endDate = new DateTime(2014, 2, 27);
            List<Int32> dataProviderIds;
            dataProviderIds = new List<Int32>();
            dataProviderIds.Add(1); // Artportalen 2
            //dataProviderIds.Add(8); // WRAM
            HarvestManager.StartSpeciesObservationUpdate(this.GetContext(), startDate, endDate, dataProviderIds, true);

            //// infinity loop to let separate thread stay alive :)
            //while (true)
            //{
            //    System.Threading.Thread.Sleep(1000);
            //}
        }

        [TestMethod]
        public void StartSpeciesObservationUpdate_ParametersAreNotSet_ExpectHarvestAllDataProvidersFromOldestHarvestBeginFromDate()
        {
            HarvestManager.StartSpeciesObservationUpdate(this.GetContext(), DateTime.MinValue, DateTime.MinValue, null, false);
        }

        [TestMethod]
        public void ContinueSpeciesObservationUpdate_AJobIsPaused_ExpectContinueHarvestByStoredMetaData()
        {
            HarvestManager.ContinueSpeciesObservationUpdate(this.GetContext());
        }

        [TestMethod]
        public void PauseSpeciesObservationUpdate_AJobIsRunning_ExpectPauseHarvestStatus()
        {
            HarvestManager.PauseSpeciesObservationUpdate(this.GetContext());
        }

        [TestMethod]
        public void StopSpeciesObservationUpdate_AJobIsRunning_ExpectStopHarvestStatus()
        {
            HarvestManager.StopSpeciesObservationUpdate(this.GetContext());
        }

        [TestMethod]
        public void StopWorkingThread()
        {
            HarvestManager.ShutdownThread = true;
        }

        [TestMethod]
        public void StartWorkingThread()
        {
            HarvestManager.ShutdownThread = false;

            // Continue manual harvest if a job exists
            HarvestManager.StartSpeciesObservationUpdateThread();
            
            //// infinity loop to let separate thread stay alive :)
            //while (true)
            //{
            //    System.Threading.Thread.Sleep(1000);
            //}
        }
    }
}

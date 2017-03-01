using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test
{
    [TestClass]
    public class GlobalTest : TestBase
    {
        private readonly Boolean _stopUpdate;
        private DateTime _lastCacheTime;

        public GlobalTest()
        {
            _stopUpdate = false;
        }

        [TestMethod]
        public void UpdateInformation()
        {
            TimeSpan sleepTime, sleeping;
            WebServiceContext context;

            context = new WebServiceContextCached(WebServiceData.WebServiceManager.Name,
                                                  ApplicationIdentifier.ArtDatabankenSOA.ToString());
            _lastCacheTime = DateTime.Now - new TimeSpan(24, 0, 0);
            sleepTime = new TimeSpan(1, 0, 0);
            while (!_stopUpdate)
            {
                //Thread.Sleep(sleepTime);
                if (!_stopUpdate)
                {
                    // Check if it is time to update taxon information.
                    sleeping = DateTime.Now - _lastCacheTime;
                    if (sleeping.TotalHours > 12)
                    {
                        // Update taxon information in the middle of the night.
                        _lastCacheTime = DateTime.Now;
                        try
                        {
                            HarvestManager.UpdateTaxonInformation(context);
                            UpdateSpeciesObservations(context);
                        }
                        catch (Exception exception)
                        {
                            // All errors are catched.
                            // Keep information update thread alive
                            // so that it will try again to update information.
                            WebServiceData.LogManager.LogError(context, exception);
                        }
                    }
                }
            }
        }

        ///// <summary>
        ///// Update database with changes in species observation.
        ///// </summary>
        //[TestMethod]
        //public void UpdateSpeciesObservations()
        //{
        //    DateTime changedFrom, changedTo;
        //    List<Int32> dataProviderIds;

        //    changedFrom = new DateTime(2010, 3, 10);
        //    changedTo = changedFrom;
        //    dataProviderIds = new List<Int32>();
        //    dataProviderIds.Add(1); // Artportalen 2
        //    //dataProviderIds.Add(2); // Obsdatabasen, test with date DateTime(2003, 4, 24);
        //    //dataProviderIds.Add(4); // Artportalen 1
        //    //dataProviderIds.Add(6); // Nors
        //    //dataProviderIds.Add(7); // Sers
        //    //dataProviderIds.Add(8); // WRAM
        //    HarvestManager.UpdateSpeciesObservations(GetContext(),
        //                                             changedFrom,
        //                                             changedTo,
        //                                             dataProviderIds, true);
        //}

        /// <summary>
        /// Update database with changes in species observation.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        private void UpdateSpeciesObservations(WebServiceContext context)
        {
            DateTime changedFrom, changedTo;
            List<Int32> dataProviderIds;

            // Set changedFrom and changedTo to yesterday.
            changedFrom = new DateTime(DateTime.Now.Year,
                                       DateTime.Now.Month,
                                       DateTime.Now.Day) - new TimeSpan(1, 0, 0, 0);
            changedTo = changedFrom;
            dataProviderIds = new List<Int32>();
            dataProviderIds.Add(1); // Artportalen 2
            dataProviderIds.Add(2); // Obsdatabasen
            dataProviderIds.Add(4); // Artportalen 1
            //dataProviderIds.Add(8); // WRAM
            HarvestManager.UpdateSpeciesObservations(context,
                                                     changedFrom,
                                                     changedTo,
                                                     dataProviderIds, false);
        }

        [TestMethod]
        public void UpdateSpeciesObservations_LatestHarvestDateIsEarlier_ExpectedHarvestFromDataProvider()
        {
            DateTime changedFrom, dataProviderOldestLatestHarvestDate, currentDataProviderLatestHarvestDate;
            Int32 actual;
            Int32 expected = -1;

            // Set changedFrom date to the day after the oldest LatestHarvestDate for active providers
            dataProviderOldestLatestHarvestDate = DateTime.Now - new TimeSpan(1, 0, 0, 0);
            changedFrom = dataProviderOldestLatestHarvestDate.AddDays(1).Date;

            currentDataProviderLatestHarvestDate = DateTime.Now - new TimeSpan(1, 0, 0, 0);

            actual = currentDataProviderLatestHarvestDate.Date.CompareTo(changedFrom);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateSpeciesObservations_LatestHarvestDateIsEqual_ExpectedNoHarvestFromDataProvider()
        {
            DateTime changedFrom, dataProviderOldestLatestHarvestDate, currentDataProviderLatestHarvestDate;
            Int32 actual;
            Int32 expected = 0;

            // Set changedFrom date to the day after the oldest LatestHarvestDate for active providers
            dataProviderOldestLatestHarvestDate = DateTime.Now - new TimeSpan(1, 0, 0, 0);
            changedFrom = dataProviderOldestLatestHarvestDate.AddDays(1).Date;

            currentDataProviderLatestHarvestDate = DateTime.Now;

            actual = currentDataProviderLatestHarvestDate.Date.CompareTo(changedFrom);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateSpeciesObservations_LatestHarvestDateIsNewer_ExpectedNoHarvestFromDataProvider()
        {
            DateTime changedFrom, dataProviderOldestLatestHarvestDate, currentDataProviderLatestHarvestDate;
            Int32 actual;
            Int32 expected = 1;

            // Set changedFrom date to the day after the oldest LatestHarvestDate for active providers
            dataProviderOldestLatestHarvestDate = DateTime.Now - new TimeSpan(1, 0, 0, 0);
            changedFrom = dataProviderOldestLatestHarvestDate.AddDays(1).Date;

            currentDataProviderLatestHarvestDate = DateTime.Now + new TimeSpan(1, 0, 0, 0);

            actual = currentDataProviderLatestHarvestDate.Date.CompareTo(changedFrom);

            Assert.AreEqual(expected, actual);
        }
    }
}

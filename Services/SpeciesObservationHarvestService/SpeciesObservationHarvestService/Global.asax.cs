using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.WebService;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Artportalen;
using Microsoft.ApplicationInsights.Extensibility;

namespace SpeciesObservationHarvestService
{
    /// <summary>
    /// Global.
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        private Boolean _stopUpdate;
        private DateTime _lastCacheTime;

        /// <summary>
        /// Application_Start.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(object sender, EventArgs e)
        {
            if (Configuration.InstallationType == InstallationType.Production)
            {
                TelemetryConfiguration.Active.InstrumentationKey = "2ae866ae-1a70-4721-98d4-94612b068fd3";
            }

            SqlServerTypes.Utilities.LoadNativeAssemblies(Server.MapPath("~/bin"));

            Configuration.SetInstallationType();
            if (Configuration.InstallationType == InstallationType.Production)
            {
                SpeciesObservationConfiguration.IsElasticsearchUsed = false;
            }

            _stopUpdate = false;
            HarvestManager.ShutdownThread = false;
            if (Environment.MachineName == "ARTSERVICE2-1")
            {
                StartManualUpdate();
                StartUpdate();
            }
            else
            {
                if (Configuration.Debug)
                {
                    StartManualUpdate();
                }
            }
        }

        /// <summary>
        /// Application_End.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_End(object sender, EventArgs e)
        {
            _stopUpdate = true;

            // Let manual harvest thread stop current harvest date, and then shut thread down
            HarvestManager.ShutdownThread = true;
        }

        /// <summary>
        /// Handle manual update of information.
        /// </summary>
        private void StartManualUpdate()
        {
            Thread thread;

            // Start thread.
            thread = new Thread(StartSpeciesObservationUpdateThread);
            thread.Start();
        }

        /// <summary>
        /// Handle manual update of information.
        /// </summary>
        private void StartSpeciesObservationUpdateThread()
        {
            TimeSpan sleepTime;
            sleepTime = new TimeSpan(0, 0, 30); // Because harvest service constructor need time start up.
            Thread.Sleep(sleepTime);

            // Continue manual harvest if a job exists
            HarvestManager.StartSpeciesObservationUpdateThread();
        }

        /// <summary>
        /// Start handling of automatic taxon information update.
        /// </summary>
        private void StartUpdate()
        {
            Thread thread;

            // Start thread.
            thread = new Thread(UpdateInformation);
            thread.Start();
        }

        /// <summary>
        /// Handle automatic update of information.
        /// </summary>
        private void UpdateInformation()
        {
            TimeSpan sleepTime, sleeping;
            WebServiceContext context;

            _lastCacheTime = DateTime.Now - new TimeSpan(24, 0, 0);
            sleepTime = new TimeSpan(0, 3, 0); // Wait 3 minutes before update starts.
            while (!_stopUpdate)
            {
                Thread.Sleep(sleepTime);
                if (!_stopUpdate)
                {
                    // Check if it is time to update taxon and project parameter information.
                    sleeping = DateTime.Now - _lastCacheTime;
                    if ((sleeping.TotalHours > 12) &&
                        (4 == DateTime.Now.Hour))
                    {
                        context = new WebServiceContextCached(WebServiceData.WebServiceManager.Name, ApplicationIdentifier.ArtDatabankenSOA.ToString());

                        // Update taxon and project parameter information in the middle of the night.
                        _lastCacheTime = DateTime.Now;
                        try
                        {
                            HarvestManager.UpdateTaxonInformation(context);
                            HarvestManager.UpdateProjectParameterInformation(new ArtportalenServer(), context);
                        }
                        catch (Exception exception)
                        {
                            // All errors are catched.
                            // Keep information update thread alive
                            // so that it will try again to update information.
                            WebServiceData.LogManager.LogError(context, exception);
                        }

                        try
                        {
                            Boolean isUpdateSpeciesObservationsSuccessful = UpdateSpeciesObservations(context);

                            if (!isUpdateSpeciesObservationsSuccessful)
                            {
                                WebServiceData.LogManager.Log(context, "Error in UpdateSpeciesObservations", LogType.SpeciesObservationUpdate, "Read details in LogUpdateError.");
                            }

                            HarvestManager.DeleteUnnecessaryChanges(context);
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

        /// <summary>
        /// Update database with changes in species observation.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>True if update is successful.</returns>
        private Boolean UpdateSpeciesObservations(WebServiceContext context)
        {
            DateTime changedFrom, changedTo;
            List<Int32> dataProviderIds = new List<Int32>();
            Boolean isUpdateSuccessful = false;
            List<WebSpeciesObservationDataProvider> dataProviders,
                                                    dataProvidersThatUseChangeId,
                                                    dataProvidersThatUseDate;
            List<WebSpeciesObservationDataProvider> activeDataProviders = GetActiveDataProviders(context);

            // Set changedTo to yesterday.
            changedTo = DateTime.Now.Date - new TimeSpan(1, 0, 0, 0);

            // Set changedFrom date to the day after the oldest LatestHarvestDate for active providers
            dataProvidersThatUseDate = activeDataProviders.GetDataProvidersThatUseDate();
            changedFrom = dataProvidersThatUseDate.Min(dp => dp.LatestHarvestDate).AddDays(1).Date;

            // Harvest until yesterday
            while (changedFrom <= changedTo)
            {
                // Harvest from sources that have been harvested before
                dataProviders = dataProvidersThatUseDate.Where(dp => dp.LatestHarvestDate.Date.CompareTo(changedFrom.Date) < 0).ToList();

                dataProviderIds.Clear();
                dataProviderIds.AddRange(dataProviders.Select(dp => dp.Id));

                if (dataProviderIds.Count > 0)
                {
                    isUpdateSuccessful = HarvestManager.UpdateSpeciesObservations(context, changedFrom, changedFrom, dataProviderIds, false);
                }

                // Continue with next day
                changedFrom = changedFrom.AddDays(1);
            }

            dataProvidersThatUseChangeId = activeDataProviders.GetDataProvidersThatUseChangeId();
            if (dataProvidersThatUseChangeId.IsNotEmpty())
            {
                changedFrom = DateTime.Now.Date - new TimeSpan(1, 0, 0, 0);
                dataProviderIds.Clear();
                dataProviderIds.AddRange(dataProvidersThatUseChangeId.Select(dp => dp.Id));
                isUpdateSuccessful = HarvestManager.UpdateSpeciesObservations(context, changedFrom, changedFrom, dataProviderIds, false) && isUpdateSuccessful;
            }

            return isUpdateSuccessful;
        }

        /// <summary>
        /// Get active data providers.
        /// </summary>
        /// <param name="context">Web service context.</param>
        /// <returns>List of data providers.</returns>
        private List<WebSpeciesObservationDataProvider> GetActiveDataProviders(WebServiceContext context)
        {
            List<WebSpeciesObservationDataProvider> allDataProviders = HarvestManager.GetSpeciesObservationDataProviders(context);

            // Get active providers that have been harvested at least one time before
            return allDataProviders.Where(dataProvider => dataProvider.IsActiveHarvest && dataProvider.LatestHarvestDate.CompareTo(DateTime.MinValue) > 0).ToList();
        }
    }
}
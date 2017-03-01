using System;
using System.Threading;
using System.Web;
using ArtDatabanken.Data;
using Microsoft.ApplicationInsights.Extensibility;
using TaxonManager = ArtDatabanken.WebService.ArtDatabankenService.Data.TaxonManager;

namespace ArtDatabankenService
{
    /// <summary>
    /// Controls handling of automatic taxon information update.
    /// </summary>
    public class Global : HttpApplication
    {
        private Boolean _stopTaxonInformationUpdate;
        private DateTime _lastCacheTime;

        /// <summary>
        /// Starts the application.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected void Application_Start(object sender, EventArgs e)
        {
            if (Configuration.InstallationType == InstallationType.Production)
            {
                TelemetryConfiguration.Active.InstrumentationKey = "2ae866ae-1a70-4721-98d4-94612b068fd3";
            }

            Configuration.SetInstallationType();
            _stopTaxonInformationUpdate = false;
            if (Environment.MachineName == "ARTSERVICE2-1")
            {
                StartTaxonInformationUpdate();
            }
        }

        /// <summary>
        /// Ends the application.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected void Application_End(object sender, EventArgs e)
        {
            _stopTaxonInformationUpdate = true;
        }

        /// <summary>
        /// Start handling of automatic taxon information update.
        /// </summary>
        private void StartTaxonInformationUpdate()
        {
            Thread thread;

            // Start thread.
            thread = new Thread(UpdateTaxonInformation);
            thread.Priority = ThreadPriority.BelowNormal;
            thread.Start();
        }

        /// <summary>
        /// Handle automatic update of taxon information.
        /// </summary>
        private void UpdateTaxonInformation()
        {
            TimeSpan sleepTime, sleeping;

            _lastCacheTime = DateTime.Now - new TimeSpan(50, 0, 0);
            sleepTime = new TimeSpan(1, 0, 0);
            while (!_stopTaxonInformationUpdate)
            {
                Thread.Sleep(sleepTime);
                if (!_stopTaxonInformationUpdate)
                {
                    // Check if it is time to update taxon information.
                    sleeping = DateTime.Now - _lastCacheTime;
                    if ((sleeping.TotalHours > 48) ||
                        ((sleeping.TotalHours > 12) &&
                         ((DateTime.Now.Hour > 22) || (DateTime.Now.Hour < 6))))
                    {
                        // Update taxon information in the middle of the night.
                        _lastCacheTime = DateTime.Now;
                        TaxonManager.UpdateTaxonInformation();
                    }
                }
            }
        }
    }
}
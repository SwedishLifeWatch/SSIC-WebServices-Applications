using System;
using System.Threading;
using System.Web;
using ArtDatabanken.Data;
using ArtDatabanken.WebService;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService;
using ArtDatabanken.WebService.TaxonService.Data;
using Microsoft.ApplicationInsights.Extensibility;

namespace TaxonService
{
    public class Global : HttpApplication
    {
        private Boolean _stopUpdate;
        private DateTime _lastSpeciesFactsUpdate;

        protected void Application_Start(object sender, EventArgs e)
        {
            if (Configuration.InstallationType == InstallationType.Production)
            {
                TelemetryConfiguration.Active.InstrumentationKey = "2ae866ae-1a70-4721-98d4-94612b068fd3";
            }

            Configuration.SetInstallationType();

            _stopUpdate = false;
            if (Environment.MachineName == "ARTSERVICE2-1")
            {
                StartSpeciesFactsUpdate();
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            _stopUpdate = true;
        }

        /// <summary>
        /// Start handling of automatic species fact update.
        /// </summary>
        private void StartSpeciesFactsUpdate()
        {
            Thread thread;

            // Start thread.
            thread = new Thread(UpdateSpeciesFacts);
            thread.Start();
        }

        /// <summary>
        /// Handle automatic species facts update.
        /// </summary>
        private void UpdateSpeciesFacts()
        {
            TimeSpan sleepTime, sleeping;
            WebServiceContext context;

            _lastSpeciesFactsUpdate = DateTime.Now - new TimeSpan(24, 0, 0);
            sleepTime = new TimeSpan(0, 3, 0); // Wait 3 minutes before update starts.
            while (!_stopUpdate)
            {
                Thread.Sleep(sleepTime);
                if (!_stopUpdate)
                {
                    // Check if it is time to update taxon and project parameter information.
                    sleeping = DateTime.Now - _lastSpeciesFactsUpdate;
                    if ((sleeping.TotalHours > 12) &&
                        (3 == DateTime.Now.Hour))
                    {
                        context = new WebServiceContextCached(WebServiceData.WebServiceManager.Name, ApplicationIdentifier.ArtDatabankenSOA.ToString());

                        // Update species facts information in the middle of the night.
                        _lastSpeciesFactsUpdate = DateTime.Now;
                        try
                        {
                            TaxonUpdateManager.UpdateTaxonSpeciesFacts(context);
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
    }
}
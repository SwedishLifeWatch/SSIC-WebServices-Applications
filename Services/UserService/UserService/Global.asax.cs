using System;
using System.Threading;
using ArtDatabanken.Data;
using ArtDatabanken.WebService;
using ArtDatabanken.WebService.Data;
using Microsoft.ApplicationInsights.Extensibility;

namespace UserService
{
    /// <summary>
    /// Global.
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        private Boolean _stopUpdate;
        private DateTime _lastUpdateTime;

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

            _stopUpdate = false;
            if (Environment.MachineName == "ARTSERVICE2-1")
            {
                StartUpdate();
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
        /// Start handling of automatic role member information update.
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

            _lastUpdateTime = DateTime.Now - new TimeSpan(24, 0, 0);
            sleepTime = new TimeSpan(0, 1, 0);
            while (!_stopUpdate)
            {
                Thread.Sleep(sleepTime);
                if (!_stopUpdate)
                {
                    // Check if it is time to update taxon information.
                    sleeping = DateTime.Now - _lastUpdateTime;
                    if ((sleeping.TotalHours > 12) &&
                        (1 == DateTime.Now.Hour))
                    {
                        context = new WebServiceContext(WebServiceData.WebServiceManager.Name, ApplicationIdentifier.ArtDatabankenSOA.ToString());

                        _lastUpdateTime = DateTime.Now;
                        try
                        {
                            ArtDatabanken.WebService.UserService.Data.UserManager.DeleteRoleMembers(context);
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
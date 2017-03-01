using System;
using System.Web;
using ArtDatabanken.Data;
using Microsoft.ApplicationInsights.Extensibility;

namespace TaxonAttributeService
{
    public class Global : HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            if (Configuration.InstallationType == InstallationType.Production)
            {
                TelemetryConfiguration.Active.InstrumentationKey = "2ae866ae-1a70-4721-98d4-94612b068fd3";
            }

            Configuration.SetInstallationType();
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

        }
    }
}
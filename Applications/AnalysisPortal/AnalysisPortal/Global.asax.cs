using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AnalysisPortal.Controllers;
using AnalysisPortal.Helpers;
using AnalysisPortal.Helpers.ActionFilters;
using ArtDatabanken.Data;
using ArtDatabanken.GIS.IO;
using ArtDatabanken.WebApplication.AnalysisPortal.Factories;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Logging;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList;
using ArtDatabanken.WebService.Client.AnalysisService;
using ArtDatabanken.WebService.Client.GeoReferenceService;
using ArtDatabanken.WebService.Client.ReferenceService;
using ArtDatabanken.WebService.Client.SpeciesObservationService;
using ArtDatabanken.WebService.Client.TaxonAttributeService;
using ArtDatabanken.WebService.Client.TaxonService;
using ArtDatabanken.WebService.Client.UserService;

namespace AnalysisPortal
{
    using ArtDatabanken.WebService.Proxy;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
#if DEBUG
            Logger.WriteMessage("Web application started");
#endif
            Configuration.SetInstallationType();
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(double), new DoubleModelBinder());
            SqlServerTypes.Utilities.LoadNativeAssemblies(Server.MapPath("~/bin"));
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            String path = Server.MapPath("~/App_Data/Map/Sverigekarta med län.geojson");
            ArtDatabanken.WebApplication.AnalysisPortal.IO.MapImage.InitializeMap(path);
            SpeciesObservationPointMap.InitializeMap(path);
            
            IFilterProvider[] providers = FilterProviders.Providers.ToArray();
            FilterProviders.Providers.Clear();
            FilterProviders.Providers.Add(new ExcludeFilterProvider(providers));

            CoreData.CountryManager = new CountryManagerMultiThreadCache();
            CoreData.LocaleManager = new LocaleManagerMultiThreadCache();
            CoreData.TaxonManager = new TaxonManagerMultiThreadCache();
            CoreData.UserManager = new UserManagerMultiThreadCache();
            CoreData.RegionManager = new RegionManagerMultiThreadCache(new CoordinateSystem());
            CoreData.MetadataManager = new MetadataManagerMultiThreadCache();

            // Get species observation information from Elasticsearch.
            //WebServiceProxy.AnalysisService.WebServiceAddress = @"silurus2-1.artdata.slu.se/AnalysisService/AnalysisService.svc";
            //WebServiceProxy.SwedishSpeciesObservationService.WebServiceAddress = @"silurus2-1.artdata.slu.se/SwedishSpeciesObservationService/SwedishSpeciesObservationService.svc";

            // Set datasources
            UserDataSource.SetDataSource();
            TaxonDataSource.SetDataSource();
            SpeciesObservationDataSource.SetDataSource();
            GeoReferenceDataSource.SetDataSource();
            AnalysisDataSource.SetDataSource();
            TaxonAttributeDataSource.SetDataSource();
            ReferenceDataSource.SetDataSource();            
            WebServiceProxy.SwedishSpeciesObservationService.MaxBufferPoolSize = Resources.AppSettings.Default.MaxBufferPoolSize;            
            // Local test.
            //WebServiceProxy.SwedishSpeciesObservationService.InternetProtocol = InternetProtocol.Http;
            //WebServiceProxy.SwedishSpeciesObservationService.WebServiceAddress = @"localhost:1667/SwedishSpeciesObservationService.svc";
            //WebServiceProxy.SwedishSpeciesObservationService.WebServiceProtocol = WebServiceProtocol.SOAP11;
            
            // Login application user
            try
            {
                CoreData.UserManager.LoginApplicationUser();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to login application user", ex);
            }          

            // Initialize Red list cache
            try
            {
                CoreData.AnalysisManager.InitAnalysisCache(CoreData.UserManager.GetApplicationContext());
                //TaxonListInformationManager.Instance.InitCache();
                IUserContext cacheUserContext = CoreData.UserManager.GetApplicationContext();
                cacheUserContext.Locale = CoreData.LocaleManager.GetLocale(cacheUserContext, LocaleId.sv_SE);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to initialize cache", ex);
            }     

            ScheduledTasksManager.AddTasks();                        
        }

        protected void Application_EndRequest()
        {
            //System.Diagnostics.Debug.WriteLine(string.Format("{0}:{1}", (DateTime.Now - HttpContext.Current.Timestamp).TotalMilliseconds, HttpContext.Current.Request.RawUrl));
        }

        void Session_Start(object sender, EventArgs e)
        {
            string requestCookies = Request.Headers["Cookie"];
            if ((requestCookies != null) && (requestCookies.IndexOf("ASP.NET_SessionId") >= 0))
            {
                //cookie existed, so this new one is due to timeout. 
                //Redirect the user
                //System.Diagnostics.Debug.WriteLine("Session expired!");
                //Response.RedirectToRoute(new { controller = "Home", Action = "Index" });
            }

            // You can't use SessionHandler.Language in Global.asax since we don't have any request yet
            Session["language"] = Thread.CurrentThread.CurrentUICulture.Name;
            //SessionHandler.Language = Thread.CurrentThread.CurrentUICulture.Name;            
            Session["results"] = new CalculatedDataItemCollection();   
            Session["mySettings"] = new MySettings();

#if !DEBUG
            NewsScrapeHelper.UpdateNews(Request.PhysicalApplicationPath);
#endif

        }

        private void Session_End(object sender, EventArgs e)
        {
            var userContext = Session["userContext"] as IUserContext;
            var mySettings = Session["mySettings"] as MySettings;
            if (userContext != null && mySettings != null)
            {
                LastUserSessionIdManager.UpdateLastUserSessionId(userContext.User.UserName, Session.SessionID);                
                MySettingsManager.SaveLastSettings(userContext, mySettings);
                MySettingsManager.SaveToDisk(userContext, MySettingsManager.SettingsName, mySettings);
            }  
        }

        /// <summary>
        /// Executes once in every page request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            /* Globalization with cookie */            
            HttpCookie cookie = Request.Cookies["CultureInfo"];

            if (cookie != null && cookie.Value != null)
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(cookie.Value);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(cookie.Value);
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("sv-SE");
                Thread.CurrentThread.CurrentCulture = new CultureInfo("sv-SE");
            }         
        }

        /// <summary> 
        /// Handles the Error event of the Application control. 
        /// </summary> 
        /// <param name="sender">The source of the event.</param> 
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param> 
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            var httpException = exception as HttpException;
            Response.Clear();
            Server.ClearError();
            var routeData = new RouteData();
            routeData.Values["controller"] = "Errors";
            routeData.Values["action"] = "General";
            routeData.Values["exception"] = exception;
            Response.StatusCode = 500;
            if (httpException != null)
            {
                Response.StatusCode = httpException.GetHttpCode();
                switch (Response.StatusCode)
                {
                    case 400:
                        routeData.Values["action"] = "Http400";
                        break;
                    case 403:
                        routeData.Values["action"] = "Http403";
                        break;
                    case 404:
                        routeData.Values["action"] = "Http404";
                        break;
                    case 500:
                        routeData.Values["action"] = "Http500";
                        break;
                }
            }
            // Avoid IIS7 getting in the middle
            Response.TrySkipIisCustomErrors = true;
            IController errorsController = new ErrorsController();
            var wrapper = new HttpContextWrapper(Context);
            var rc = new RequestContext(wrapper, routeData);

            // Log to file (Debug Level) when http statuscode 500 (Internal Server Error)
            if (Response.StatusCode == 500)
            {
                Logger.WriteDebugException(exception);
            }
            errorsController.Execute(rc);
        }

        private void TestDependencyInjection()
        {
            var diTaxon = DependencyFactory.Resolve<IDITaxon>();
            var diTaxonViewManager = DependencyFactory.Resolve<IDITaxonViewManager>();
        }   
    }
}
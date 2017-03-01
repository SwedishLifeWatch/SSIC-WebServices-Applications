using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;
using ArtDatabanken.WebService.Client.AnalysisService;
using ArtDatabanken.WebService.Client.PESINameService;
using ArtDatabanken.WebService.Client.TaxonService;
using ArtDatabanken.WebService.Client.UserService;
using ArtDatabanken.WebService.Data;
using Dyntaxa.Controllers;
using ArtDatabanken.WebApplication.Dyntaxa.Data;

namespace Dyntaxa
{
    using ArtDatabanken.WebService.Client.ReferenceService;
    using ArtDatabanken.WebService.Client.TaxonAttributeService;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        #if DEBUG
            public const int RootTaxonId = 4000107;           
        #else
           public const int RootTaxonId = 0;           
        #endif

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            // Delete if favicon is used
            //routes.IgnoreRoute("favicon.ico");
            //routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute("", "Revision/Info/{revisionInfoId}", new { controller = "Revision", action = "Info" });
            routes.MapRoute("", "Revision/Add/{taxonId}", new { controller = "Revision", action = "Add" });
            routes.MapRoute("", "Revision/List/{taxonId}", new { controller = "Revision", action = "List" });
            routes.MapRoute("", "Revision/StartEditing/{revisionId}", new { controller = "Revision", action = "StartEditing", revisionId = UrlParameter.Optional });
            routes.MapRoute("", "Export/TaxonAttributes/{taxonId}", new { controller = "SpeciesFact", action = "SpeciesFactList" });
            //routes.MapRoute("", "TaxonAttributes/EditFactors", new { controller = "SpeciesFact", action = "EditFactors" });
            //routes.MapRoute("", "TaxonAttributes/HostTaxa/EditHostFactorsForSubstrate", new { controller = "SpeciesFact", action = "EditHostFactorsForSubstrate" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{taxonId}", // URL with parameters
                new { controller = "Taxon", action = "SearchResult", taxonId = UrlParameter.Optional }) // Parameter defaults
            ;
        }

        void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new NoCacheFilterAttribute());
        }

        void Session_Start(object sender, EventArgs e)
        {
            const string masterName = "Site";
            Session["masterpage"] = masterName;

            Session["TaxonId"] = TaxonIdTuple.Create(RootTaxonId.ToString(), RootTaxonId);
            Session["RootTaxonId"] = RootTaxonId;

            string requestCookies = Request.Headers["Cookie"];
            if ((requestCookies != null) && (requestCookies.IndexOf("ASP.NET_SessionId") >= 0))
            {
                //cookie existed, so this new one is due to timeout. 
                //Redirect the user
                System.Diagnostics.Debug.WriteLine("Session expired!");
                Response.RedirectToRoute(new { controller = "Taxon", Action = "SearchResult" });                
            }
            Session["language"] = Thread.CurrentThread.CurrentUICulture.Name;
        }
        
        protected void Application_Start()
        {
#if DEBUG
            DyntaxaLogger.WriteMessage("Web application started");
#endif
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);

            RegisterRoutes(RouteTable.Routes);
            // MK 2015-10-12 County map isn't used?
            //String path = Server.MapPath("~/Images/Maps/Sverigekarta med län.shp");
            //CountyOccurrenceMap.InitializeMap(path);

            Configuration.SetInstallationType();            
            CoreData.CountryManager = new CountryManagerMultiThreadCache();
            CoreData.LocaleManager = new LocaleManagerMultiThreadCache();
            CoreData.ReferenceManager = new ReferenceManagerMultiThreadCache();
            CoreData.TaxonManager = new TaxonManagerMultiThreadCache();
            CoreData.UserManager = new UserManagerMultiThreadCache();
            CoreData.SpeciesFactManager = new SpeciesFactManagerMultiThreadCache();

            UserDataSource.SetDataSource();
            TaxonDataSource.SetDataSource();
            PesiNameDataSource.SetDataSource();
            TaxonAttributeDataSource.SetDataSource();
            ReferenceDataSource.SetDataSource();
            AnalysisDataSource.SetDataSource();

            CoreData.UserManager.LoginApplicationUser();
            CoreData.UserManager.LoginApplicationTransactionUser();
            ScheduledTasksManager.AddTasks();
        }

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
                DyntaxaLogger.WriteDebugException(exception);
            }
            errorsController.Execute(rc);
        } 
    }

    public class NoCacheFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();
            filterContext.HttpContext.Response.AppendHeader("Cache-Control", "no-store");
            filterContext.HttpContext.Response.AppendHeader("pragma", "no-cache");

            base.OnResultExecuting(filterContext);
        }
    } 
}

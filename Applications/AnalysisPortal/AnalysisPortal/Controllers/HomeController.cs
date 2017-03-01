using System;
using System.Threading;
using System.Web.Mvc;
using AnalysisPortal.Helpers;
using AnalysisPortal.Helpers.ActionFilters;
using ArtDatabanken.WebApplication.AnalysisPortal.Json;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Home;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Home;

namespace AnalysisPortal.Controllers
{
    /// <summary>
    /// This Controller contains Actions that is used on the main page
    /// </summary>    
    public class HomeController : BaseController
    {
        /// <summary>
        /// The main page.
        /// </summary>
        /// <returns></returns>
        [IndexedBySearchRobots]
        public ActionResult Index()
        {
            var model = new IndexViewModel(Thread.CurrentThread.CurrentCulture.Parent.Name);
            return View(model);   
        }


        /// <summary>
        /// Renders a view where we can see the build version & date of AnalysisPortal
        /// </summary>
        /// <returns></returns>
        public ActionResult VersionNumber()
        {
            var viewManager = new HomeViewManager(GetCurrentUser(), SessionHandler.MySettings);
            VersionNumberViewModel model = viewManager.CreateVersionNumberViewModel(typeof(AnalysisPortal.MvcApplication).Assembly);
            return View(model);
        }

        public ActionResult Cookies()
        {
            return View();
        }

        public ActionResult NotImplemented()
        {
            return Content("Not implemented");
        }

        [IgnorePageInfoManager]
        public ActionResult Robots()
        {
#if DEBUG
            return File("~/robots-debug.txt", "text/plain");
#endif
         
            Response.ContentType = "text/plain";
            //List<string> noRobots = NoRobotsAttribute.GetNoRobots();
            //-- Here you should write a response with the list of 
            //areas/controllers/action for search engines not to follow.
            return PartialView();
        }

        //public ActionResult TestOleDb()
        //{
        //    try
        //    {
        //        String path = Server.MapPath("~/App_Data/Map/Sverigekarta med län.shp");
        //        ArtDatabanken.WebApplication.AnalysisPortal.IO.SpeciesObservationCountMap.InitializeMap(path);
        //        ArtDatabanken.WebApplication.AnalysisPortal.IO.SpeciesObservationPointMap.InitializeMap(path);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(ex.Message);                
        //    }
        //    return Content("Success");
        //}
        
        public ActionResult Status()
        {
            return View();
        }

#if DEBUG

        [IgnorePageInfoManager]        
        public ActionResult AvailableThreads()
        {
            return View();
        }

        [IgnorePageInfoManager]
        public ActionResult LongTimeRunningAction(int? seconds)
        {
            if (!seconds.HasValue)
            {
                seconds = 10;
            }

            Thread.Sleep(seconds.Value * 1000);            
            return Content("Sleeping 10 seconds finished");
        }

        public JsonNetResult GetAvailableWorkerThreads()
        {
            JsonModel jsonModel;
            try
            {
                int availableWorker, availableIO;
                int maxWorker, maxIO;
                ThreadPool.GetAvailableThreads(out availableWorker, out availableIO);
                ThreadPool.GetMaxThreads(out maxWorker, out maxIO);
                jsonModel = JsonModel.CreateFromObject(new { AvailableWorkers = availableWorker, MaxWorkers = maxWorker, AvailableIO = availableIO, MaxIO = maxIO });
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }
            return new JsonNetResult(jsonModel);
        }

        [IgnorePageInfoManager]
        public ActionResult JobQueue()
        {
            return View();
        }

#endif

    }
}

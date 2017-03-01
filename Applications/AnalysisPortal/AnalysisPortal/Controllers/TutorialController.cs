using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using AnalysisPortal.Helpers;
using System.Diagnostics;
using ArtDatabanken.WebApplication.AnalysisPortal.Json;

namespace AnalysisPortal.Controllers
{
#if DEBUG

    public class TutorialController : Controller
    {
        #region General
        public PartialViewResult TutorialHeader()
        {            
            return PartialView();
        }

        private void FunctionThatThrowsAnException()
        {
            throw new Exception("An unexpected error occurred.");
        }
        #endregion

        public ActionResult AddNewPage()
        {
            return View();
        }

        public ActionResult AddResultPage()
        {
            return View();
        }

        #region Save changes before leaving page

        public ActionResult SaveChangesBeforeLeavingPageTutorial()
        {
            return View();
        }

        public JsonNetResult WriteNameAndAgeToDebug(string name, int bornYear)
        {            
            JsonModel jsonModel;
            try
            {
                // here you can save data...
                int age = DateTime.Now.Year - bornYear;
                Debug.WriteLine("Name: {0}, Age: {1}", name, age);
                jsonModel = JsonModel.CreateSuccess("Name and age was successfully written to Debug output window");
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }
            return new JsonNetResult(jsonModel);            
        }

        #endregion

        #region MakeAjaxCall JavaScript function
        public ActionResult MakeAjaxCallTutorial()
        {
            return View();
        }

        public JsonNetResult GetMountains()
        {
            JsonModel jsonModel;
            try
            {
                var dictionary = new Dictionary<string, int>();
                dictionary.Add("Mount Everest", 8848);
                dictionary.Add("Kilimanjaro", 5892);
                dictionary.Add("Kebnekaise", 2102);                
                jsonModel = JsonModel.CreateFromObject(dictionary);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }
            return new JsonNetResult(jsonModel);
        }

        #endregion

        #region Make Ajax Request sample
        public ActionResult MakeAjaxRequest()
        {
            return View();
        }

        public JsonNetResult GetJSONExample(int number, string company)
        {
            JsonModel jsonModel;
            try
            {
                Thread.Sleep(2000); // simulate processing time
                if (company == "Saab")
                {
                    jsonModel = JsonModel.CreateFailure("This company doesn't exist anymore");
                }
                else
                {
                    var result = new WorkPlaceModel(100 / number, company);
                    jsonModel = JsonModel.CreateFromObject(result);
                }
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }
            return new JsonNetResult(jsonModel);
        }

        public class WorkPlaceModel
        {
            public int Id { get; set; }
            public string WorkPlace { get; set; }

            public WorkPlaceModel(int id, string workPlace)
            {
                Id = id;
                WorkPlace = workPlace;
            }
        }

        #endregion

        #region PartialViewError sample
        public ActionResult HandlePartialViewErrorSample()
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult PartialViewWithError()
        {
            try
            {
                FunctionThatThrowsAnException();
                return PartialView();
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);
            }
        }

        [ChildActionOnly]
        public PartialViewResult PartialViewWithoutError()
        {
            try
            {
                return PartialView();
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);
            }
        }
        
        #endregion

        #region ExtJsGridSaveChangesTutorial
        
        public ActionResult ExtJsGridSaveChangesTutorial()
        {
            return View();
        }

        #endregion
    }

#endif
}

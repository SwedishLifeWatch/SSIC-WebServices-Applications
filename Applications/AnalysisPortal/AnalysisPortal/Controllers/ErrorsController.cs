using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Logging;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.General;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Error;

namespace AnalysisPortal.Controllers
{
    /// <summary>
    /// This Controller contains Actions that is used when an unexpected error occurred.
    /// </summary>
    public class ErrorsController : BaseController
    {
        /// <summary>
        /// Renders a general error page.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public ActionResult General(Exception exception)
        {
            string additionalErrorMsg = null;
            additionalErrorMsg = exception.StackTrace;
            var errorMsg = exception.Message;
            var errorModelManger = new ErrorViewManager(exception, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.Resource.SharedError,
                exception.GetType().Name,
                errorMsg,
                additionalErrorMsg);

            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Renders a partial error view.
        /// Used when an unexpected error in a partial view occurrs.
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult PartialViewError(Exception exception)
        {            
            Logger.WriteException(exception);
            return PartialView("PartialViewError");
        }

        /// <summary>
        /// The http 400 error. Bad request - indicates a bad request.
        /// </summary>
        /// <returns>
        /// Error view 400.
        /// </returns>
        public ActionResult Http400()
        {
            return View("400");
        }

        /// <summary>
        /// Not found
        /// </summary>
        /// <returns>
        /// Error view 404
        /// </returns>
        public ActionResult Http404()
        {
            return View("404");
        }

        /// <summary>
        /// Forbidden
        /// </summary>
        /// <returns>
        /// Error view 403
        /// </returns>
        public ActionResult Http403()
        {
            return View("403");
        }

        /// <summary>
        /// Internal server error
        /// </summary>
        /// <returns>
        /// Error view 500
        /// </returns>
        public ActionResult Http500()
        {
            return View("500");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers.Extensions;
using ArtDatabanken.Data;

namespace Dyntaxa.Controllers
{
    public class ErrorsController : DyntaxaBaseController
    {
        public ActionResult General(Exception exception)
        {
            string additionalErrorMsg = null;
            additionalErrorMsg = exception.StackTrace;
            var errorMsg = exception.Message;            
            var errorModelManger = new ErrorModelManager(exception, RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.SharedError,
                exception.GetType().Name,
                errorMsg, 
                additionalErrorMsg);
                    
            return View("ErrorInfo", errorModel);
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

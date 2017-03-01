using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Logging;

namespace AnalysisPortal.Controllers
{
    /// <summary>
    /// This class is a base class for the Controllers in AnalysisPortal
    /// The method OnActionExecuting is executed every time a page is requested
    /// and handle things that is common for all pages, for example logging.
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    [AnalysisPortal.Helpers.ActionFilters.NoCacheFilter]
    public class BaseController : Controller
    {
        public readonly ISessionHelper _sessionHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        public BaseController()
        {
        }

        /// <summary>
        /// Constructor called by test
        /// </summary>
        /// <param name="userDataSourceRepository"></param>
        /// <param name="session"></param>
        public BaseController(IUserDataSource userDataSourceRepository, ISessionHelper session)
        {
            CoreData.UserManager.DataSource = userDataSourceRepository;
            _sessionHelper = session;
        }

        /// <summary>
        /// Gets the Application UserContext
        /// </summary>
        /// <returns></returns>        
        protected internal IUserContext GetApplicationUser()
        {
            return CoreData.UserManager.GetApplicationContext();
        }

        /// <summary>
        /// Gets the current UserContext
        /// </summary>
        /// <returns></returns>        
        protected internal IUserContext GetCurrentUser()
        {
            return CoreData.UserManager.GetCurrentUser();
        }

        /// <summary>
        /// Sets the current language.
        /// Changes CurrentCulture and saves the language selection in a cookie.
        /// </summary>
        /// <param name="cultureISOCode">The culture ISO code.</param>
        protected internal void SetLanguage(string cultureISOCode)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureISOCode);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureISOCode);
            SessionHandler.Language = cultureISOCode;            
            var cookie = new HttpCookie("CultureInfo");
            cookie.Value = cultureISOCode;
            cookie.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Removes a specific cookie
        /// </summary>
        /// <param name="key">the name of the cookie</param>
        protected internal void RemoveCookie(string key)
        {            
            var cookie = new HttpCookie(Server.UrlEncode(key));
            cookie.HttpOnly = true;
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Remove(Server.UrlEncode(key));
            Response.Cookies.Add(cookie);
        }

        // Here is some code that could be used when session times out
        ////http://forums.asp.net/t/1829631.aspx/1?not+redirected+to+login+page+when+session+timed+out+with+ajax+call
        //    if (Request.IsAjaxRequest())
        //    {
        //        // check if session is supported
        //        if (filterContext.HttpContext.Session != null)
        //        {

        //            // check if a new session id was generated
        //            if (filterContext.HttpContext.Session.IsNewSession)
        //            {
        //                // If it says it is a new session, but an existing cookie exists, then it must
        //                // have timed out
        //                string sessionCookie = filterContext.HttpContext.Request.Headers["Cookie"];
        //                if ((null != sessionCookie) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
        //                {                            
        //                    Response.ClearContent();
        //                    Response.Write("Session expired");                            
        //                    //filterContext.RequestContext.HttpContext.Response.StatusCode = 401;
        //                    return;                            
        //                }
        //            }
        //        }
        //        return;
        //        //base.OnActionExecuting(filterContext);
        //    }

        /// <summary>
        /// Executes every time an action method is invoked.
        /// Called before the action method is invoked.
        /// Handles things that is common for all pages, for example logging.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        [ValidateInput(false)]
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {            
            if (filterContext.IsChildAction || Request.IsAjaxRequest() || Session == null)
            {
                return;
            }

            IUserContext user = GetCurrentUser();

            //Response.AppendHeader("Refresh",Convert.ToString((Session.Timeout * 30)) + ";URL=~/Home/Index");        

            // set language
            if (Request.QueryString["lang"] != null)
            {
                ILocale locale;

                locale = CoreData.LocaleManager.GetUsedLocales(GetCurrentUser()).Get(Request.QueryString["lang"]);
                if (locale.IsNotNull())
                {
                    SetLanguage(locale.ISOCode);
                    if (user.IsAuthenticated())
                    {
                        user.Locale = locale;
                    }
                }
            }

            // Render image mode.
            // If we render the map as image, we use Google Mercator coordinate system and Google maps.
            if (Request.QueryString["renderImageMode"] != null)
            {
                ViewBag.RenderImageMode = true;
            }

            // Set LogOn information            
            ViewBag.IsLoggedIn = user.IsAuthenticated();
            ViewBag.UserName = user.User.UserName;
            ViewBag.UserRole = string.Empty;
            ViewBag.MultipleRoles = false;
            if (user.CurrentRoles.IsNotNull() && user.CurrentRoles.Count > 0)
            {
                ViewBag.UserRole = user.CurrentRole.IsNotNull() ? "(" + user.CurrentRole.Name + ")" : string.Empty;
                if (user.CurrentRoles.Count > 1 && user.CurrentRole.IsNotNull())
                {
                    ViewBag.MultipleRoles = true;
                }
            }            

            // User message
            if (SessionHandler.UserMessages != null)
            {
                ViewBag.UserMessages = new List<UserMessage>(SessionHandler.UserMessages);
                SessionHandler.UserMessages.Clear();
            }

            // MySettings message
            if (SessionHandler.MySettingsMessage != null)
            {
                ViewBag.MySettingsMessage = SessionHandler.MySettingsMessage;
                SessionHandler.MySettingsMessage = null;
            }

#if DEBUG
            ViewBag.Debug = true;
#else
            ViewBag.Debug = false;
#endif

            ActionDescriptor actionDescriptor = filterContext.ActionDescriptor;
            string actionName = actionDescriptor.ActionName;
            string controllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            ViewBag.CookieName = string.Format("{0}{1}", controllerName, actionName);

            PageInfo currentPage = PageInfoManager.GetPageInfo(controllerName, actionName);
            if (currentPage.IsNotNull())
            {
                SessionHandler.CurrentPage = currentPage;
            }

            // Log history event            
            var logEventHistoryItem = new LogEventHistoryItem();
            logEventHistoryItem.Action = actionName;
            logEventHistoryItem.Controller = controllerName;            
            logEventHistoryItem.Form = Request.Form != null ? Request.Form.PrettyPrint() : "-";
            logEventHistoryItem.HttpAction = Request.HttpMethod;
            logEventHistoryItem.Url = Request.RawUrl;
            logEventHistoryItem.Date = DateTime.Now;
            logEventHistoryItem.UserName = user.User != null ? user.User.UserName : "-";
            logEventHistoryItem.UserRole = user.CurrentRole != null ? user.CurrentRole.Identifier : "-";
            logEventHistoryItem.Referrer = Request.UrlReferrer != null ? Request.UrlReferrer.OriginalString : "-";
            var logEventHistory = SessionHandler.LogEventHistory;            
            if (logEventHistory == null)
            {
                logEventHistory = new LogEventHistory();
                SessionHandler.LogEventHistory = logEventHistory;                
            }
            logEventHistory.HistoryItems.Add(logEventHistoryItem);
            if (logEventHistory.HistoryItems.Count > 10)
            {
                logEventHistory.HistoryItems.RemoveAt(0);
            }

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Creates the an image using PhantomJs http://phantomjs.org/,        
        /// which is a headless WebKit with JavaScript API that can be used
        /// to save images of a web page.
        /// </summary>
        /// <param name="actionUrl">The action URL.</param>
        /// <param name="divId">The div id.</param>
        /// <param name="zoomFactor">Zoom image. Default is 1.0.</param>
        /// <returns>A file stream containing a png file.</returns>
        protected FileStreamResult CreateImageUsingPhantomJs(string actionUrl, string divId, double zoomFactor = 1.0)
        {
            Process process = null;
            try
            {                
                string filename = System.IO.Path.GetRandomFileName();
                filename = Path.ChangeExtension(filename, ".png");
                string filePath = Path.Combine(Server.MapPath("~/Temp/Images/"), filename);                

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                process = CreatePhantomJsProcess(filePath, actionUrl, divId, zoomFactor);                
                process.Start();                
                TimeSpan timeout = TimeSpan.FromMinutes(10);
                if (process.WaitForExit((int)timeout.TotalMilliseconds)) // wait max 10 minutes.
                {
                    byte[] fileContent = System.IO.File.ReadAllBytes(filePath);
                    System.IO.File.Delete(filePath);
                    MemoryStream ms = new MemoryStream(fileContent);
                    return new FileStreamResult(ms, "image/png");
                }
                else // timeout
                {
                    process.Kill();
                    throw new Exception("PhantomJs process Timeout");
                    //string errorFilePath = Server.MapPath("~/Content/images/error.png");
                    //byte[] fileContent = System.IO.File.ReadAllBytes(errorFilePath);
                    //MemoryStream ms = new MemoryStream(fileContent);
                    //return new FileStreamResult(ms, "image/png");
                }
            }
            catch (Exception ex)
            {
                if (process != null && !process.HasExited)
                {
                    process.Kill();
                }
                throw ex;
                //string errorFilePath = Server.MapPath("~/Content/images/error.png");
                //byte[] fileContent = System.IO.File.ReadAllBytes(errorFilePath);
                //MemoryStream ms = new MemoryStream(fileContent);
                //return new FileStreamResult(ms, "image/png");
            }
        }

        /// <summary>
        /// Creates the PhantomJs process used to render an image.
        /// </summary>
        /// <param name="filePath">The file path where the image will be saved.</param>
        /// <param name="actionUrl">The url to the page that should be rendered to an image.</param>
        /// <param name="divId">The id of the html object on the page that should be rendered.</param>
        /// <param name="zoomFactor"></param>
        /// <returns>An process object that can be executed.</returns>
        protected Process CreatePhantomJsProcess(string filePath, string actionUrl, string divId, double zoomFactor)
        {            
            if (actionUrl.Contains("?"))
            {
                actionUrl += "&renderImageMode=true";
            }
            else
            {
                actionUrl += "?renderImageMode=true";
            }

            string strUrl = this.Request.Url.GetLeftPart(UriPartial.Authority) + actionUrl;
            string strExecutable = Server.MapPath("~/bin/phantomjs.exe");
            string flags = "--proxy-type=none --ignore-ssl-errors=true";
            string jsFileArgument = "\"" + Server.MapPath("~/Scripts/Phantomjs/rasterize_element3.js") + "\"";
            string urlArgument = strUrl;
            string filePathArgument = "\"" + filePath + "\"";

            string elementIdArgument = string.Format("'#{0}'", divId.Replace("#", ""));
            //string zoomArgument = "\"" + zoomFactor + "\"";
            string zoomArgument = zoomFactor.ToString(CultureInfo.InvariantCulture);
            //string zoomArgument = "1.5";
            string sessionIdArgument = "'" + Session.SessionID + "'";
            string domainArgument = "'" + Request.Url.Host + "'";

            string arguments = flags + " " + jsFileArgument + " " + urlArgument + " " + filePathArgument + " " + elementIdArgument + " " + zoomArgument + " " + domainArgument + " " + sessionIdArgument;

            ProcessStartInfo processStartInfo;
            processStartInfo = new ProcessStartInfo();
            processStartInfo.CreateNoWindow = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = arguments;
            processStartInfo.FileName = strExecutable;
            Process process = new Process();
            process.StartInfo = processStartInfo;
            return process;
        }

        protected void SetServerDone()
        {
            if (Response == null)
            {
                return;
            }

            Response.Cookies["ServerDone"].Value = "true";
        }

      /*  protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = int.MaxValue
            };
        }*/

        #region Add ability to render partial views to string

        protected string RenderPartialViewToString()
        {
            return RenderPartialViewToString(null, null);
        }

        protected string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
        }

        protected string RenderPartialViewToString(object model)
        {
            return RenderPartialViewToString(null, model);
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            try
            {            
                if (string.IsNullOrEmpty(viewName))
                {
                    viewName = ControllerContext.RouteData.GetRequiredString("action");
                }

                ViewData.Model = model;           

                using (StringWriter sw = new StringWriter())
                {                
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                
                    ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                    viewResult.View.Render(viewContext, sw);

                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        // nya

        protected string RenderPartialViewToString(PartialViewResult viewResult, Controller controller, string viewName)
        {            
            ViewData.Model = viewResult.Model;
            var controllerContext = new ControllerContext(HttpContext, RouteData, controller);
            var routeData = new System.Web.Routing.RouteData();

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult2 = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
     
                var viewContext = new ViewContext(controllerContext, viewResult2.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();                
            }
        }

        //public string RazorRender(Controller context, string DefaultAction)
        //{

        //    string Cache = string.Empty;
        //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //    System.IO.TextWriter tw = new System.IO.StringWriter(sb);
        //    RazorView view_ = new RazorView(context.ControllerContext, DefaultAction, null, false, null);
        //    view_.Render(new ViewContext(context.ControllerContext, view_, new ViewDataDictionary(), new TempDataDictionary(), tw), tw);
        //    Cache = sb.ToString();
        //    return Cache;

        //}

        //public string RenderRazorViewToString(string viewName, object model)
        //{

        //    ViewData.Model = model;

        //    using (var sw = new StringWriter())
        //    {

        //        var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);

        //        var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);

        //        viewResult.View.Render(viewContext, sw);

        //        return sw.GetStringBuilder().ToString();

        //    }

        //}

#endregion

    }
}

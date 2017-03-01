using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using System.Web.Routing;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using Dyntaxa.Helpers;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Navigation;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers.Extensions;
using log4net;

namespace Dyntaxa.Controllers
{
    /// <summary>
    /// Base class for all controllers in Dyntaxa    
    /// </summary>
    public class DyntaxaBaseController : Controller
    {
        public TaxonSearchManager TaxonSearchManager { get; protected set; }
        public readonly ISessionHelper _sessionHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public DyntaxaBaseController()
        {
            TaxonSearchManager = new TaxonSearchManager();
        }

        // Called by test
        public DyntaxaBaseController(IUserDataSource userDataSourceRepository, ITaxonDataSource taxonDataSourceRepository, ISessionHelper session)
        {
            CoreData.UserManager.DataSource = userDataSourceRepository;
            CoreData.TaxonManager.DataSource = taxonDataSourceRepository;
            TaxonSearchManager = new TaxonSearchManager(userDataSourceRepository, taxonDataSourceRepository, session);
            _sessionHelper = session;
        }

        /// <summary>
        /// Change master file using session and querystring
        /// </summary>        
        protected override ViewResult View(string viewName, string masterName, object model)
        {
            // set masterpage
            if (Session != null && Session["masterpage"] != null && !Request.IsAjaxRequest())
            {
                masterName = (string)Session["masterpage"];
            }
            return base.View(viewName, masterName, model);
        }       

        protected int? ReplaceTaxonId
        {
            get { return Session["ReplaceTaxonId"] as int?; }
            set { Session["ReplaceTaxonId"] = value; }
        }

        protected List<int?> LumpTaxonIdList
        {
            get { return Session["LumpTaxonIdList"] as List<int?>; }
            set { Session["LumpTaxonIdList"] = value; }
        }

        protected int? SplitTaxonId
        {
            get { return Session["SplitTaxonId"] as int?; }
            set { Session["SplitTaxonId"] = value; }
        }

        protected List<int?> ReplaceTaxonIdList
        {
            get { return Session["ReplaceTaxonIdList"] as List<int?>; }
            set { Session["ReplaceTaxonIdList"] = value; }
        }

        protected List<int?> RevisionListSettings
        {
            get { return Session["RevisionListSettings"] as List<int?>; }
            set { Session["RevisionListSettings"] = value; }
        }

        protected List<SpeciesFactHostsIdListHelper> SpeciesFactHostTaxonIdList
        {
            get { return Session["SpeciesFactHostTaxonIdList"] as List<SpeciesFactHostsIdListHelper>; }
            set { Session["SpeciesFactHostTaxonIdList"] = value; }
        }

        protected List<SpeciesFactHostsIdListHelper> SpeciesFactFactorIdList
        {
            get { return Session["SpeciesFactFactorIdList"] as List<SpeciesFactHostsIdListHelper>; }
            set { Session["SpeciesFactFactorIdList"] = value; }
        }

        /// <summary>
        /// Current Taxon Identifier.
        /// Consist of the Taxon Id and possible Taxon string identifier.
        /// </summary>
        public TaxonIdTuple TaxonIdentifier
        {
            get
            {
                var val = Session["TaxonId"] as TaxonIdTuple;
                return val ?? TaxonIdTuple.Create();
            }

            set
            {
                Session["TaxonId"] = value;
            }
        }

        /// <summary>
        /// Current revision Id. Should be null if we aren't in a revision.
        /// </summary>
        public int? RevisionId
        {
            get { return Session["RevisionId"] as int?; }
            set { Session["RevisionId"] = value; }
        }

        /// <summary>
        /// Current revision
        /// </summary>
        public ITaxonRevision TaxonRevision
        {
            get
            {
                var revision = Session["Revision"] as ITaxonRevision;
                if (!RevisionId.HasValue)
                {
                    return null;
                }
                if (revision != null)
                {
                    if (revision.Id != RevisionId.Value)
                    {
                        revision = CoreData.TaxonManager.GetTaxonRevision(GetCurrentUser(), RevisionId.Value);
                        TaxonRevision = revision;
                    }
                }
                return revision;
            }

            set
            {
                Session["Revision"] = value;
            }
        }

        /// <summary>
        /// Current revision Id that we are working with.
        /// </summary>
        public int? RevisionHandlingId
        {
            get { return Session["RevisionHandlingId"] as int?; }
            set { Session["RevisionHandlingId"] = value; }
        }

        public int? RevisionTaxonId
        {
            get { return Session["RevisionTaxonId"] as int?; }
            set { Session["RevisionTaxonId"] = value; }            
        }

        public int? RevisionTaxonCategorySortOrder
        {
            get { return Session["RevisionTaxonCategorySortOrder"] as int?; }
            set { Session["RevisionTaxonCategorySortOrder"] = value; }
        }

        /// <summary>        
        /// Used to know to which view a user will see when 
        /// navigating with the help of the taxon tree.
        /// </summary>
        public NavigateData NavigateData
        {
            get { return Session["NavigateData"] as NavigateData ?? new NavigateData("Taxon", "Info"); }
            set { Session["NavigateData"] = value; }
        }

        protected string CurrentController
        {
            get { return Session["CurrentController"] as string ?? "Taxon"; }
            set { Session["CurrentController"] = value; }
        }

        protected string CurrentAction
        {
            get { return Session["CurrentAction"] as string ?? "SearchResult"; }
            set { Session["CurrentAction"] = value; }
        }

        /// <summary>
        /// Root Taxon Id. Used in the Taxon tree.
        /// </summary>
        public int? RootTaxonId
        {
            get { return Session["RootTaxonId"] as int?; }
            set { Session["RootTaxonId"] = value; }
        }

        protected void ResetAllLumpSplitData()
        {
            ReplaceTaxonId = null;
            ReplaceTaxonIdList = null;
            SplitTaxonId = null;
            LumpTaxonIdList = null;
        }

        protected void RedrawTree()
        {
            var model = Session["TaxonTree"] as TaxonTreeViewModel;
            Session.Remove("TaxonTree");
            if (model != null)
            {
                TempData.Add("selectedIdInTree", model.ActiveId);
                TempData.Add("expandedTaxaInTree", model.ExpandedTaxa);                
            }
        }

        protected void RedrawTree(ITaxon newRootTaxon)
        {
            if (!newRootTaxon.IsValid)
            {
                return;
            }

            this.RedrawTree(newRootTaxon.Id);            
        }

        protected void RedrawTree(int newRootTaxonId)
        {
            Session.Remove("TaxonTree");
            this.RootTaxonId = newRootTaxonId;
        }

        /// <summary>
        /// Trigger a refresh of the tree next time it is rendered.
        /// Tries to remember which taxa is expanded and which taxon is selected
        /// </summary>
        /// <param name="newRootTaxonId">root taxon in tree</param>
        /// <param name="selectedTaxonId">selected taxon</param>
        protected void RedrawTree(int newRootTaxonId, int selectedTaxonId)
        {
            var model = Session["TaxonTree"] as TaxonTreeViewModel;
            Session.Remove("TaxonTree");
            if (model != null && this.RootTaxonId.GetValueOrDefault() == newRootTaxonId)
            {
                TempData.Add("expandedTaxaInTree", model.ExpandedTaxa);
            }
            this.RootTaxonId = newRootTaxonId;

            if (model != null)
            {
                TaxonTreeViewItem treeViewItem = model.GetTreeViewItemByTaxonId(selectedTaxonId);
                if (treeViewItem != null)
                {
                    TempData.Add("selectedIdInTree", treeViewItem.Id);
                }
                else
                {
                    // this is the case when we have created a new taxon
                    TempData.Add("selectedTaxonIdInTree", selectedTaxonId);
                }
                //TempData.Add("selectedIdInTree", selectedTaxonId);
            }
        }

        /// <summary>
        /// Gets a taxon from session state if it was newly set (less than 10 seconds ago)
        /// With this we can reuse taxon objects from Ajax calls.
        /// </summary>        
        protected ITaxon GetTempDataTaxon(int taxonId)
        {
            var tempTaxonValue = Session["TempDataTaxon"] as TempDataValue<ITaxon>;
            if (tempTaxonValue != null && tempTaxonValue.Value != null && tempTaxonValue.Value.Id == taxonId)
            {
                TimeSpan elapsedTime = DateTime.Now - tempTaxonValue.TimeStamp;
                if (elapsedTime.Seconds < 10)
                {
                    return tempTaxonValue.Value;
                }
            }            
            return CoreData.TaxonManager.GetTaxon(GetCurrentUser(), taxonId);
        }

        /// <summary>
        /// Sets a temporary taxon. With this we can reuse a taxon from an Ajax call
        /// </summary>        
        protected void SetTempDataTaxon(ITaxon taxon)
        {            
            Session["TempDataTaxon"] = new TempDataValue<ITaxon>(taxon);
        }

        /// <summary>
        /// Check if the taxon exist in the revision
        /// </summary>        
        protected bool IsTaxonInRevision(IUserContext userContext, int taxonId)
        {
            try
            {
                return CoreData.TaxonManager.GetTaxon(userContext, taxonId).IsInRevision;
            }
            catch (Exception)
            {
                return false;
            }            
        }

        /// <summary>
        /// Perform taxon validation.
        /// For example if the taxon is in revision or not.
        /// Adds errors to ModelState
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="taxonId"></param>
        protected void ValidateTaxon(IUserContext user, int? taxonId)
        {                        
            if (taxonId.HasValue)
            {
                if (!IsTaxonInRevision(user, taxonId.Value))
                {
                    ModelState.AddModelError("", Resources.DyntaxaResource.SharedIsTaxonPartOfRevision);
                }
            }
        }

        /// <summary>
        /// Removes a specific cookie
        /// </summary>
        /// <param name="key">the name of the cookie</param>
        protected void RemoveCookie(string key)
        {            
            var cookie = new HttpCookie(Server.UrlEncode(key));
            cookie.HttpOnly = true;
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Remove(Server.UrlEncode(key));
            Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Redirects to an action and preserves the query string
        /// </summary>
        /// <param name="actionName">action</param>
        /// <param name="controllerName">controller</param>
        /// <param name="routeValues">additional route values</param>        
       protected RedirectToRouteResult RedirectToActionWithPreservedQuery(string actionName, string controllerName, object routeValues)
       {
           return RedirectToActionWithPreservedQuery(actionName, controllerName, new RouteValueDictionary(routeValues));           
       }

       /// <summary>
       /// Redirects to an action and preserves the query string
       /// </summary>
       /// <param name="actionName">action</param>
       /// <param name="controllerName">controller</param>
       /// <param name="routeValues">additional route values</param>
        protected RedirectToRouteResult RedirectToActionWithPreservedQuery(string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            RouteValueDictionary queryDic = Request.QueryString.ToRouteValueDictionary();
            RouteValueDictionary mergedDic = DictionaryHelper.MergeDictionaries(queryDic, routeValues);            
            return RedirectToAction(actionName, controllerName, mergedDic);
        }

        /// <summary>
        /// Redirects the user to the search page. Tha user can select a taxon and return to the original view.
        /// </summary>
        /// <param name="id">taxon search string</param>        
        /// <returns></returns>
        protected RedirectToRouteResult RedirectToSearch(string id)
        {
            return RedirectToSearch(id, null);
        }

        /// <summary>
        /// Redirects the user to the search page. Tha user can select a taxon and return to the original view.
        /// </summary>
        /// <param name="id">taxon search string</param>
        /// <param name="returnParams">Dictionary with all the params that will be returned to the original view when the user selects a taxon</param>
        /// <returns></returns>
        protected RedirectToRouteResult RedirectToSearch(string id, object returnParams)
        {
            return RedirectToSearch(id, new RouteValueDictionary(returnParams));
        }

        /// <summary>
        /// Redirects the user to the search page. Tha user can select a taxon and return to the original view.
        /// </summary>
        /// <param name="id">taxon search string</param>
        /// <param name="returnParams">Dictionary with all the params that will be returned to the original view when the user selects a taxon</param>
        /// <returns></returns>
        protected RedirectToRouteResult RedirectToSearch(string id, RouteValueDictionary returnParams)
        {
            RouteValueDictionary dic = new RouteValueDictionary
                                           {
                                               { "search", id },
                                               { "returnController", RouteData.Values["controller"] },
                                               { "returnAction", RouteData.Values["action"] }
                                           };
            if (returnParams != null && returnParams.Count > 0)
            {
                dic.Add("returnParameters", EncodeRouteQueryString(returnParams));
            }

            return RedirectToAction("SearchResult", "Taxon", dic);
        }

        /// <summary>
        /// Encodes a RouteValueDictionary into a string that can be used in the url as a querystring value
        /// </summary>
        /// <param name="parameters">the dictionary to encode</param>
        /// <returns></returns>
        protected internal String EncodeRouteQueryString(RouteValueDictionary parameters)
        {            
            List<String> items = new List<String>();
            foreach (String name in parameters.Keys)
            {
                items.Add(String.Concat(name, "=", HttpUtility.UrlEncode(parameters[name].ToString())));
            }

            return String.Join("&", items.ToArray());
        } 

        /// <summary>
        /// Decodes a string that was encoded with EncodeRouteQueryString()
        /// </summary>
        /// <param name="queryString">the string to decode</param>
        /// <returns></returns>
        protected internal RouteValueDictionary DecodeRouteQueryString(string queryString)
        {
            NameValueCollection col = HttpUtility.ParseQueryString(queryString);
            RouteValueDictionary dic = new RouteValueDictionary();
            foreach (string str in col.Keys)
            {
                dic.Add(str, HttpUtility.UrlDecode(col[str]));
            }

            return dic;
        }

        /// <summary>
        /// Gets the Application UserContext
        /// </summary>
        /// <returns></returns>        
        protected IUserContext GetApplicationUser()
        {
            return CoreData.UserManager.GetApplicationContext();
        }

        /// <summary>
        /// Gets the application transaction user context.
        /// Use this when making transaction with the application user.
        /// </summary>        
        protected IUserContext GetApplicationTransactionUser()
        {
            return CoreData.UserManager.GetApplicationTransactionContext();
        }

        /// <summary>
        /// Sets the current UserContext
        /// </summary>
        /// <returns></returns>        
        protected IUserContext GetCurrentUser()
        {
            return CoreData.UserManager.GetCurrentUser();
        }

        protected IUserContext GetLoggedInUser()
        {
            return CoreData.UserManager.GetUserContext();
        }

        protected void SetLanguage(string cultureISOCode)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureISOCode);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureISOCode);            
            Session["language"] = cultureISOCode;
            var cookie = new HttpCookie("CultureInfo");
            cookie.Value = cultureISOCode;
            cookie.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(cookie);            
        }

        /// <summary>
        /// Reads information from Querystring and sets Session variables if query strings is found.        
        /// </summary>
        /// <param name="filterContext"></param>
        [ValidateInput(false)]
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction || Request.IsAjaxRequest() || Session == null)
            {
                return;
            }

            IUserContext user = GetCurrentUser();

            string masterName;            
            if (Request.QueryString["mode"] != null)
            {
                if (Request.QueryString["mode"].ToLower() == "full")
                {
                    masterName = "Site";
                    Session["masterpage"] = masterName;                    
                }
                else if (Request.QueryString["mode"].ToLower() == "mini")
                {
                    masterName = "Rest";
                    Session["masterpage"] = masterName;                    
                }
            }

            if (Session["masterpage"] != null && (string)Session["masterpage"] == "Rest")
            {
                ViewBag.MiniMode = true;
            }
            else
            {
                ViewBag.MiniMode = false;
            }

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

            // Removed from code 2012-02-28 (agoh) only set revision from Revision Controller - start editing 
            // set revisionId
            //string strRevisionId = (filterContext.RouteData.Values["revisionId"] ??
            //                    filterContext.RequestContext.HttpContext.Request.QueryString["revisionId"] ??
            //                    filterContext.RequestContext.HttpContext.Request.Form["revisionId"]) as string;
            //int revisionId;
            //if (int.TryParse(strRevisionId, out revisionId))
            //{
            //    Session["RevisionId"] = revisionId;
            //}

            // Set LogOn information            
            ViewBag.IsLoggedIn = user.IsAuthenticated();            
            ViewBag.UserName = user.User.UserName;

            // Set Taxon and Revision information
            ViewBag.TaxonId = this.TaxonIdentifier.Id;            
            ViewBag.RevisionId = this.RevisionId;
            ViewBag.RootTaxonId = this.RootTaxonId;

            if (this.RevisionId.HasValue && user.CurrentRole == null)
            {
                user.SetCurrentUserRoleToTaxonRevisionEditor(this.RevisionId.Value);
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

            // Log history event
            if (actionName != "TaxonQualitySummaryChart")
            {
                var logEventHistoryItem = new LogEventHistoryItem();
                logEventHistoryItem.Action = actionName;
                logEventHistoryItem.Controller = controllerName;
                logEventHistoryItem.TaxonId = this.TaxonIdentifier.Id;
                logEventHistoryItem.RevisionId = this.RevisionId;
                logEventHistoryItem.Form = Request.Form != null ? Request.Form.PrettyPrint() : "-";                
                logEventHistoryItem.HttpAction = Request.HttpMethod;
                logEventHistoryItem.Url = DyntaxaLogger.GetUrl();
                logEventHistoryItem.Date = DateTime.Now;
                logEventHistoryItem.UserName = user.User != null ? user.User.UserName : "-";
                logEventHistoryItem.UserRole = user.CurrentRole != null ? user.CurrentRole.Identifier : "-";
                logEventHistoryItem.Referrer = Request.UrlReferrer != null ? Request.UrlReferrer.OriginalString : "-";
                var logEventHistory = Session["LogEventHistory"] as LogEventHistory;
                if (logEventHistory == null)
                {
                    logEventHistory = new LogEventHistory();
                    Session["LogEventHistory"] = logEventHistory;
                }
                logEventHistory.HistoryItems.Add(logEventHistoryItem);
                if (logEventHistory.HistoryItems.Count > 10)
                {
                    logEventHistory.HistoryItems.RemoveAt(0);
                }
            }

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Sets ViewBag data
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction || Request.IsAjaxRequest() || Session == null)
            {
                return;
            }

            ViewBag.TaxonId = this.TaxonIdentifier.Id;            
            ViewBag.RevisionId = this.RevisionId;
            ViewBag.RootTaxonId = this.RootTaxonId;

            ActionDescriptor actionDescriptor = filterContext.ActionDescriptor;
            string actionName = actionDescriptor.ActionName;
            string controllerName = actionDescriptor.ControllerDescriptor.ControllerName;            
            var navTo = TreeNavigationManager.GetNavigation(controllerName, actionName);
            if (navTo != null)
            {
                this.NavigateData = navTo;
                this.CurrentController = controllerName;
                this.CurrentAction = actionName;
            }
            //ViewContext.RouteData.Values["action"]

            base.OnActionExecuted(filterContext);
        }

       /// <summary>
       /// Extract property name out of a property
       /// </summary>
        public static class ReflectionUtility
        {
            public static string GetPropertyName<T>(Expression<Func<T>> expression)
            {
                MemberExpression body = (MemberExpression)expression.Body;
                return body.Member.Name;
            }
        }

        /// <summary>
        /// Check if revision is correct
        /// </summary>
        /// <param name="inputRevisionId"></param>
        /// <param name="taxonRevision"></param>
        /// <returns></returns>
        protected bool CheckRevisionValidity(string inputRevisionId, ITaxonRevision taxonRevision)
        {
            string errorMsg;
            bool valid = true;
            if (taxonRevision.IsNull())
            {
                errorMsg = Resources.DyntaxaResource.RevisionAddInvalidRevisionErrorText;
                ModelState.AddModelError("", errorMsg);
                valid = false;
            }
            if (taxonRevision.IsNotNull() && !taxonRevision.Id.ToString().Equals(inputRevisionId))
            {
                errorMsg = Resources.DyntaxaResource.RevisionSharedNoValidRevisionIdErrorText;
                string propName = string.Empty;
                string propValue = string.Empty;
                if (taxonRevision.IsNotNull())
                {
                    propName = ReflectionUtility.GetPropertyName(() => taxonRevision.Id);
                    PropertyInfo pi = taxonRevision.GetType().GetProperty(propName);
                    propValue = pi.GetValue(taxonRevision, null).ToString();
                    ModelState.AddModelError("", errorMsg + " " + propValue);
                }
                else
                {
                    ModelState.AddModelError("", errorMsg); 
                }
                
                valid = false;
            }
            return valid;
        }

        /// <summary>
        /// Check if taxon is correct
        /// </summary>
        /// <param name="inputTaxonId"></param>
        /// <param name="taxon"></param>
        /// <returns></returns>
        protected bool CheckTaxonVaildity(string inputTaxonId, ITaxon taxon)
        {
            string errorMsg;
            bool valid = true;
            if (taxon.IsNull())
            {
                errorMsg = Resources.DyntaxaResource.RevisonAddInvalidTaxonErrorText;
                ModelState.AddModelError("", errorMsg);
                valid = false;
            }
            if (taxon.IsNotNull() && !taxon.Id.ToString().Equals(inputTaxonId))
            {
                errorMsg = Resources.DyntaxaResource.RevisionSharedNoValidTaxonIdErrorText;
                string propName = string.Empty;
                string propValue = string.Empty;
                if (taxon.IsNotNull())
                {
                    propName = ReflectionUtility.GetPropertyName(() => taxon.Id);
                    PropertyInfo pi = taxon.GetType().GetProperty(propName);
                    propValue = pi.GetValue(taxon, null).ToString();
                    ModelState.AddModelError("", errorMsg + " " + propValue);
                }
                else
                {
                    ModelState.AddModelError("", errorMsg);
                }
                valid = false;
            }
            return valid;
        }

        /// <summary>
        /// Check if user context is correct
        /// </summary>
        /// <param name="loggedInUser"></param>
        /// <returns></returns>
        protected bool CheckUserContextValidity(IUserContext loggedInUser)
        {
            string errorMsg;
            bool valid = true;
            if (loggedInUser.IsNull())
            {
                errorMsg = Resources.DyntaxaResource.SharedInvalidUserContext;
                ModelState.AddModelError("", errorMsg);
                valid = false;
            }
            return valid;
        }

        /// <summary>
        /// Check if user context is correct
        /// </summary>
        /// <param name="applicationUser"></param>
        /// <returns></returns>
        protected bool CheckDyntaxaApplicationUserContextValidity(IUserContext applicationUser)
        {
            string errorMsg;
            bool valid = true;
            if (applicationUser.IsNull())
            {
                errorMsg = Resources.DyntaxaResource.SharedInvalidApplicationUserContext;
                ModelState.AddModelError("", errorMsg);
                valid = false;
            }
            return valid;
        }
    }

    public class TempDataValue<T>
    {
        public T Value { get; set; }
        public DateTime TimeStamp { get; set; }

        public TempDataValue(T value)
        {
            Value = value;
            TimeStamp = DateTime.Now;
        }
    }
}

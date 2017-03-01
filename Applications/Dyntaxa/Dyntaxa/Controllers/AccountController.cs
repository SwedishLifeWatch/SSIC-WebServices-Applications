using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using ArtDatabanken.Data;
using ArtDatabanken;
using ArtDatabanken.Security;

namespace Dyntaxa.Controllers
{
    [HandleError]
    public class AccountController : DyntaxaBaseController
    {
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        public AccountController()
        {
        }

        // Test constructor
        public AccountController(ISessionHelper session, IUserManager userManagerRepository)
        {
             CoreData.UserManager = userManagerRepository; 
        }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        // **************************************
        // GET: /Account/LogIn
        // **************************************
        public ActionResult LogIn(string userName)
        {
            IUserContext userContext = Session["userContext"] as IUserContext;
            if (userContext.IsNotNull())
            {
                return View("LogOut");                
                //Session.Remove("userContext");                
            }

            LogInModel model = new LogInModel();
            model.UserName = userName;

            return View(model);
        }

        // **************************************
        // POST: /Account/LogIn
        // **************************************
        [HttpPost]
        public ActionResult LogIn(LogInModel model, string returnUrl)
        {
            IUserContext userContext;

            if (ModelState.IsValid)
            {
                userContext = CoreData.UserManager.Login(model.UserName, model.Password, Resources.DyntaxaSettings.Default.DyntaxaApplicationIdentifier);

                if (userContext.IsNotNull())
                {
                    Session["userContext"] = userContext;

                    // Must clear the service cash so that roles can be reloded.
                    // CoreData.TaxonManager.ClearCacheForUserRoles(userContext);                    
                 //   _sessionHelper.SetInSession("userContext", userContext);

                     /* globalization with cookie */
                    var cookie = Request.Cookies["CultureInfo"];
                    
                    if (cookie == null)
                    {
                        var locale = CoreData.LocaleManager.GetUsedLocales(GetCurrentUser()).Get(userContext.Locale.ISOCode);
                        SetLanguage(locale.ISOCode);
                    }
                    else
                    {
                        if (cookie.Value != null)
                        {
                            var locale = CoreData.LocaleManager.GetUsedLocales(GetCurrentUser()).Get(cookie.Value);
                            userContext.Locale = locale;
                        }
                    }

                    //HttpCookie cookie = new HttpCookie("CultureInfo");
                    //cookie.Value = userContext.Locale.ISOCode;
                    //cookie.Expires = DateTime.Now.AddYears(1);
                    //Response.Cookies.Add(cookie);

                    FormsService.SignIn(model.UserName);
                    this.RedrawTree();
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        //TempData.Add("RedirectedFromLogin", true);
                        return Redirect(returnUrl.ToLower());
                    }
                    else
                    {
                        return RedirectToAction("SearchResult", "Taxon");
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.DyntaxaResource.AccountControllerIncorrectLoginError);
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOut
        // **************************************

        public ActionResult LogOut(string returnUrl)
        {
            var userContext = Session["userContext"] as IUserContext;
            if (userContext != null)
            {
                CoreData.UserManager.Logout(userContext);
                Session["userContext"] = null;
                FormsService.SignOut();
            }

            this.RevisionId = null;
            this.TaxonRevision = null;
            this.RevisionTaxonId = null;
            this.RevisionTaxonCategorySortOrder = null;

            // check if the taxon exists. Perhaps it only exists in revision.
            if (this.TaxonIdentifier.Id.HasValue)
            {
                try
                {
                    ITaxon taxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), this.TaxonIdentifier.Id.Value);
                    this.RedrawTree();
                }
                catch (Exception)
                {
                    this.TaxonIdentifier.Id = 0;
                    this.RedrawTree(0);
                }
            }
            else
            {
                RedrawTree(0);
            }

            //return RedirectToAction("SearchResult", "Taxon");
            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("SearchResult", "Taxon");
            }            
        }

        public ActionResult AccessIsNotAllowed(string url)
        {
            var model = new AccessIsNotAllowedViewModel();
            model.Url = url;

            return View(model);
        }

#if DEBUG

        // **************************************
        // URL: /Account/AutoLogIn
        // **************************************
        // TODO: inactivate this action when publish
        public ActionResult AutoLogIn(string returnUrl, int? revisionId)
        {
            IUserContext userContext = CoreData.UserManager.Login("testuser", "Qwertyasdfg123", Resources.DyntaxaSettings.Default.DyntaxaApplicationIdentifier);
           
            if (userContext.IsNotNull())
            {
                Session["userContext"] = userContext;

                // Must clear the service cash so that roles can be reloded.
                // CoreData.TaxonManager.ClearCacheForUserRoles(userContext);

                /* globalization with cookie */
                HttpCookie cookie = new HttpCookie("CultureInfo");
                cookie.Value = userContext.Locale.ISOCode;
                cookie.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(cookie);

                FormsService.SignIn("testuser");
                if (revisionId.HasValue)
                {
                    this.RevisionId = revisionId.Value;                    
                    ITaxonRevision taxonRevision = CoreData.TaxonManager.GetTaxonRevision(GetCurrentUser(), revisionId.Value);
                    this.TaxonRevision = taxonRevision;
                    this.RevisionTaxonId = taxonRevision.RootTaxon.Id;
                    this.RevisionTaxonCategorySortOrder = taxonRevision.RootTaxon.Category.SortOrder;
                }
                this.RedrawTree();
            }
            else
            {
                ModelState.AddModelError("", Resources.DyntaxaResource.AccountControllerIncorrectLoginError);
            }

            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("SearchResult", "Taxon");
            }

            return Redirect(returnUrl);            
        }

        // **************************************
        // URL: /Account/AutoLogIn
        // **************************************
        // TODO: inactivate this action when publish

        public ActionResult AutoLogInEN(string returnUrl)
        {
            IUserContext userContext = CoreData.UserManager.Login("testuser", "Qwertyasdfg123", ApplicationIdentifier.Dyntaxa.ToString());

            if (userContext.IsNotNull())
            {
                Session["userContext"] = userContext;                                
                userContext.Locale = CoreData.LocaleManager.GetLocale(userContext, LocaleId.en_GB);

                // Must clear the service cash so that roles can be reloded.
                // CoreData.TaxonManager.ClearCacheForUserRoles(userContext);

                /* globalization with cookie */
                HttpCookie cookie = new HttpCookie("CultureInfo");
                cookie.Value = userContext.Locale.ISOCode;
                cookie.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(cookie);

                Thread.CurrentThread.CurrentUICulture = userContext.Locale.CultureInfo;
                Thread.CurrentThread.CurrentCulture = userContext.Locale.CultureInfo;

                FormsService.SignIn("testuser");
                this.RedrawTree();
            }
            else
            {
                ModelState.AddModelError("", Resources.DyntaxaResource.AccountControllerIncorrectLoginError);
            }

            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("SearchResult", "Taxon");
            }

            return Redirect(returnUrl);
        }

        // **************************************
        // URL: /Account/AutoLogIn
        // **************************************
        // TODO: inactivate this action when publish

        public ActionResult AutoLogInOnlyEditor(string returnUrl)
        {
            IUserContext userContext = CoreData.UserManager.Login("Testuseronlyeditor", "TestUser11", Resources.DyntaxaSettings.Default.DyntaxaApplicationIdentifier);

            if (userContext.IsNotNull())
            {
                Session["userContext"] = userContext;
                userContext.Locale = CoreData.LocaleManager.GetLocale(userContext, LocaleId.en_GB);

                // Must clear the service cash so that roles can be reloded.
                // CoreData.TaxonManager.ClearCacheForUserRoles(userContext);

                /* globalization with cookie */
                HttpCookie cookie = new HttpCookie("CultureInfo");
                cookie.Value = userContext.Locale.ISOCode;
                cookie.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(cookie);

                Thread.CurrentThread.CurrentUICulture = userContext.Locale.CultureInfo;
                Thread.CurrentThread.CurrentCulture = userContext.Locale.CultureInfo;

                FormsService.SignIn("Testuseronlyeditor");
                this.RedrawTree();
            }
            else
            {
                ModelState.AddModelError("", Resources.DyntaxaResource.AccountControllerIncorrectLoginError);
            }

            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("SearchResult", "Taxon");
            }

            return Redirect(returnUrl);
        }

#endif

    }
}

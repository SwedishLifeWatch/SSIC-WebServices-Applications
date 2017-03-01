using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers.Extensions;
using ArtDatabanken.Data;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Authorization alternatives implemented in UserAdmin web application currently.
    /// </summary>
    public struct RequiredAuthorization
    {        
        public const int Authenticated = -1; // This is the default, which correspond to the default MVC Authorice attribute.
        public const int TaxonRevisionAdministrator = 1; // Has autority to give other users TaxonEditor and TaxonRevisionEditor permission
        public const int TaxonRevisionEditor = 2; // Has authority for editing specific revision
        public const int TaxonEditor = 3; // Has authority to get a revision task and be a TaxonRevisionEditor
        public const int SpeciesFactEditor = 4; //Has authority to edit, create, delete SpeciesFact and References
        public const int SpeciesFactFactorEditor = 5; //Has authority to view artfakta for all taxa and export artfakta factors for taxa. 
        public const int EditReference = 6; // Has authority to create, edit & delete references.
        public const int SpeciesFactEVAEditor = 7; //Has authority to edit, create, delete SpeciesFact when logging in from EVA
    }

    /// <summary>
    /// This class handle authorization.
    /// </summary>
    public class DyntaxaAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Decides if the current role should be changed. Default is true
        /// </summary>
        public bool ChangeCurrentRole { get; set; }

        public DyntaxaAuthorizeAttribute()
        {
            ChangeCurrentRole = true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            bool isUserLoggedIn = false;
            if (filterContext != null && filterContext.RequestContext != null && filterContext.RequestContext.HttpContext != null && filterContext.RequestContext.HttpContext.Session != null)
            {
                var userContext = filterContext.RequestContext.HttpContext.Session["userContext"] as IUserContext;
                if (userContext != null)
                {
                    isUserLoggedIn = userContext.IsAuthenticated();
                }
            }

            if (isUserLoggedIn)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Account" }, { "action", "AccessIsNotAllowed" }, { "url", filterContext.RequestContext.HttpContext.Request.Url } });
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }

            ////IUserContext userContext = CoreData.UserManager.
            //bool? redirectFromLogin = null;
            //if (filterContext != null && filterContext.Controller != null && filterContext.Controller.TempData != null)
            //{
            //    redirectFromLogin = filterContext.Controller.TempData["RedirectedFromLogin"] as bool?;
            //}

            //if (!redirectFromLogin.GetValueOrDefault()) 
            //{ 
            //    base.HandleUnauthorizedRequest(filterContext); 
            //} 
            //// Otherwise deny access 
            //else 
            //{
            //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Account" }, { "action", "AccessIsNotAllowed" }, { "url", filterContext.RequestContext.HttpContext.Request.Url } }); 
            //} 
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorized = false;

            AuthorizationManager authorizationManager = new AuthorizationManager(httpContext.Session);
            int roleIndex = -1;
            if (authorizationManager.UserContext.IsNotNull())
            {                
                switch (this.Order)
                {
                    case RequiredAuthorization.Authenticated:
                        authorized = authorizationManager.UserContext.IsNotNull();
                        break;

                    case RequiredAuthorization.TaxonRevisionAdministrator:
                        
                        authorized = authorizationManager.IsTaxonRevisionAdministrator(out roleIndex);
                        break;

                    case RequiredAuthorization.TaxonEditor:
                        authorized = authorizationManager.IsTaxonEditor(out roleIndex);
                        break;
                        
                    case RequiredAuthorization.TaxonRevisionEditor:
                        int? revId = httpContext.Session["RevisionId"] as int?;
                        if (revId.HasValue)
                        {
                            ITaxonRevision taxonRevision = CoreData.TaxonManager.GetTaxonRevision(authorizationManager.UserContext, revId.Value);
                            authorized = authorizationManager.IsTaxonRevisionEditor(taxonRevision, out roleIndex);
                        }
                        
                        //var strRevisionId = httpContext.Request.RequestContext.RouteData.Values["revisionId"] ??
                        //                    httpContext.Request.QueryString["revisionId"] ?? 
                        //                    httpContext.Request.Form["revisionId"];
                        
                        //if (strRevisionId != null)
                        //{
                        //    int revisionId;
                        //    if (int.TryParse(strRevisionId.ToString(), out revisionId))
                        //    {
                        //        authorized = authorizationManager.IsTaxonRevisionEditor(revisionId, out roleIndex);
                        //    }
                        //}
                        break;

                    case RequiredAuthorization.SpeciesFactEditor:
                        authorized = authorizationManager.HasSpeciesFactAuthority();
                        ChangeCurrentRole = false;
                        break;

                    case RequiredAuthorization.SpeciesFactEVAEditor:
                        authorized = authorizationManager.HasEVASpeciesFactAuthority();
                        ChangeCurrentRole = false;
                        break;

                    case RequiredAuthorization.EditReference:
                        authorized = authorizationManager.HasEditReferenceAuthority();
                        ChangeCurrentRole = false;
                        break;
                }
            }

            if (ChangeCurrentRole && authorized && Order != RequiredAuthorization.Authenticated)
            {
                authorizationManager.SetUserRole(roleIndex);
            }

            return authorized;
        }
    }
}

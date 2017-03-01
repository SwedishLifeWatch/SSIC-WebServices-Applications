using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers.Authorization
{
    /// <summary>
    /// Authorization alternatives implemented in UserAdmin web application currently.
    /// </summary>
    public struct RequiredAuthorization
    {
        public const int Authenticated = -1; // This is the default, which correspond to the default MVC Authorice attribute.        
        public const int AuthenticatedAndPrivatePersonRole = 2; // The user is logged in and has current role set to private person.
        public const int SpeciesFactEditor = 4; //Has authority to edit, create, delete SpeciesFact and References        
    }

    /// <summary>
    /// This class handle authorization.
    /// </summary>
    public class AnalysisPortalAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Decides if the current role should be changed. Default is true
        /// </summary>
        public bool ChangeCurrentRole { get; set; }

        public AnalysisPortalAuthorizeAttribute()
        {
            ChangeCurrentRole = true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            bool isUserLoggedIn = false;
            if (filterContext != null && filterContext.RequestContext != null && filterContext.RequestContext.HttpContext != null && filterContext.RequestContext.HttpContext.Session != null)
            {
                var userContext = SessionHandler.UserContext;                
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
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorized = false;

            var authorizationManager = new AuthorizationManager();
            int roleIndex = -1;
            if (authorizationManager.UserContext.IsNotNull())
            {
                switch (this.Order)
                {
                    case RequiredAuthorization.Authenticated:
                        authorized = authorizationManager.UserContext.IsNotNull();
                        break;

                    case RequiredAuthorization.AuthenticatedAndPrivatePersonRole:
                        authorized = authorizationManager.UserContext.IsAuthenticated() &&
                                     authorizationManager.UserContext.IsCurrentRolePrivatePerson();
                        break;
                    case RequiredAuthorization.SpeciesFactEditor:
                        authorized = authorizationManager.HasSpeciesFactAuthority();
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

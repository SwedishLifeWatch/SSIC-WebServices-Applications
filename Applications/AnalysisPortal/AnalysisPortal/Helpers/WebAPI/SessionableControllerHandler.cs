using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;

namespace AnalysisPortal.Helpers.WebAPI
{
    /// <summary>
    /// This class will provide sessiona state to a specific module.
    /// </summary>
    public class SessionableControllerHandler : HttpControllerHandler, IRequiresSessionState
    {
        /// <summary>
        /// The constructor just calls the base class constructor.
        /// </summary>
        /// <param name="routeData">The routes to activate the session on.</param>
        public SessionableControllerHandler(RouteData routeData)
            : base(routeData)
        {
        }
    }
}
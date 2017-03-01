using System.Web;
using System.Web.Routing;

namespace AnalysisPortal.Helpers.WebAPI
{
    /// <summary>
    /// This class is needed to plug into the routing workflow the handler to make a controller sessionable.
    /// </summary>
    public class SessionStateRouteHandler : IRouteHandler
    {
        /// <summary>
        /// Implements the GetHttpHandler method of IRouteHandler.
        /// </summary>
        /// <param name="requestContext">The context of the request being sent.</param>
        /// <returns>A handler to make a controller sessionable.</returns>
        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
        {
            return new SessionableControllerHandler(requestContext.RouteData);
        }
    }
}
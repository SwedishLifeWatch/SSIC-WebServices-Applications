using System;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService
{
    /// <summary>
    /// Extension methods to the WebServiceContext class.
    /// </summary>
    public static class WebServiceContextExtension
    {
        /// <summary>
        /// Get user database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>User database.</returns>
        public static UserServer GetUserDatabase(this WebServiceContext context)
        {
            return (UserServer)(context.GetDatabase());
        }
    }
}

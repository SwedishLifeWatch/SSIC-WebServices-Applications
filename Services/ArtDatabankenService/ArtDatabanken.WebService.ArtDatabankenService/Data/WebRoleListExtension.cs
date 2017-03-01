using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Contains extension to lists of WebRole class.
    /// </summary>
    public static class WebRoleListExtension
    {
        /// <summary>
        /// Test if access rights to species observations are
        /// easy or complex to handle.
        /// </summary>
        /// <param name="roles">Roles to test.</param>
        /// <returns>True, if access rights to species observations are easy to handle.</returns>
        public static Boolean IsSimpleSpeciesObservationAccessRights(this List<WebRole> roles)
        {
            if (roles.IsNotEmpty())
            {
                foreach (WebRole role in roles)
                {
                    if (!role.IsSimpleSpeciesObservationAccessRights())
                    {
                        return false;
                    }
                }
            }

            // No complex species observation access rights found.
            return true;
        }
    }
}

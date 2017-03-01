using System;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Contains extension to the WebRole class.
    /// </summary>
    public static class WebRoleExtension
    {
        /// <summary>
        /// Test if access rights to species observations are
        /// easy or complex to handle.
        /// </summary>
        /// <param name="authority">Authority to test.</param>
        /// <returns>True, if access rights to species observations are easy to handle.</returns>
        private static Boolean IsSimpleSpeciesObservationAccessRights(WebAuthority authority)
        {
            if (authority.Identifier != AuthorityIdentifier.Sighting.ToString())
            {
                // Authority is not related to species observations.
                return true;
            }

            if (authority.RegionGUIDs.IsNotEmpty() ||
                authority.TaxonGUIDs.IsNotEmpty())
            {
                // Complex species observation access rights found.
                return false;
            }

            // No complex species observation access rights found.
            return true;
        }

        /// <summary>
        /// Test if access rights to species observations are
        /// easy or complex to handle.
        /// </summary>
        /// <param name="role">Role to test.</param>
        /// <returns>True, if access rights to species observations are easy to handle.</returns>
        public static Boolean IsSimpleSpeciesObservationAccessRights(this WebRole role)
        {
            if (role.IsNotNull() && role.Authorities.IsNotEmpty())
            {
                foreach (WebAuthority authority in role.Authorities)
                {
                    if (!IsSimpleSpeciesObservationAccessRights(authority))
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

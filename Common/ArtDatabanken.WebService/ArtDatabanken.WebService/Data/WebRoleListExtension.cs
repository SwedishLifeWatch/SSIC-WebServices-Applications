using System;
using System.Collections.Generic;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to lists of WebRole class.
    /// </summary>
    public static class WebRoleListExtension
    {
        /// <summary>
        /// Get protected species observation indication access rights
        /// in Json format.
        /// </summary>
        /// <param name="roles">Current roles.</param>
        /// <param name="context">Web service request context.</param>
        /// <returns>Protected species observation indication access rights in Json format.</returns>
        public static String GetProtectedSpeciesObservationIndicationAccessRightsJson(this List<WebRole> roles,
                                                                                      WebServiceContext context)
        {
            Boolean isFirstAuthority;
            List<WebAuthority> authorities;
            List<WebPolygon> polygons;
            List<WebRegionGeography> regionGeographys;
            StringBuilder filter;
            WebCoordinateSystem speciesObservationCoordinateSystem;

            filter = new StringBuilder();
            authorities = new List<WebAuthority>();
            foreach (WebRole role in roles)
            {
                if (role.Authorities.IsNotEmpty())
                {
                    foreach (WebAuthority authority in role.Authorities)
                    {
                        if ((authority.Identifier == AuthorityIdentifier.SightingIndication.ToString()) &&
                             authority.RegionGUIDs.IsNotEmpty())
                        {
                            authorities.Add(authority);
                        }
                    }
                }
            }

            if (authorities.IsNotEmpty())
            {
                isFirstAuthority = true;
                if (1 < authorities.Count)
                {
                    filter.Append("{\"bool\":{ \"should\" : [");
                }

                foreach (WebAuthority authority in authorities)
                {
                    if (isFirstAuthority)
                    {
                        isFirstAuthority = false;
                    }
                    else
                    {
                        filter.Append(", ");
                    }

                    if (authority.RegionGUIDs.IsNotEmpty())
                    {
                        filter.Append("{\"bool\":{ \"must\" : [");

                        speciesObservationCoordinateSystem = new WebCoordinateSystem();
                        speciesObservationCoordinateSystem.Id = CoordinateSystemId.WGS84;
                        regionGeographys = WebServiceData.RegionManager.GetRegionsGeographyByGuids(context,
                                                                                                    authority.RegionGUIDs,
                                                                                                    speciesObservationCoordinateSystem);
                        polygons = new List<WebPolygon>();
                        foreach (WebRegionGeography regionGeography in regionGeographys)
                        {
                            polygons.AddRange(regionGeography.MultiPolygon.Polygons);
                        }

                        filter.Append("{ \"geo_shape\": {");
                        //filter.Append(" \"_cache\": true, ");
                        filter.Append(" \"Location\": {");
                        filter.Append(" \"shape\": {");
                        filter.Append(" \"type\": \"envelope\", ");
                        filter.Append(" \"coordinates\": ");
                        filter.Append(polygons.GetEnvelopeJson(speciesObservationCoordinateSystem, 10));
                        filter.Append("}}}}");

                        filter.Append(", ");

                        filter.Append("{ \"geo_shape\": {");
                        //filter.Append(" \"_cache\": true, ");
                        filter.Append(" \"Location\": {");
                        filter.Append(" \"shape\": {");
                        filter.Append(" \"type\": \"multipolygon\", ");
                        filter.Append(" \"coordinates\": ");
                        filter.Append(polygons.GetJson());
                        filter.Append("}}}}");

                        // End AND condition.
                        filter.Append("]}}");
                    }
                }

                if (1 < authorities.Count)
                {
                    // End OR condition.
                    filter.Append("]}}");
                }
            }

            return filter.ToString();
        }

        /// <summary>
        /// Get species observation access rights in Json format.
        /// </summary>
        /// <param name="roles">Current roles.</param>
        /// <param name="context">Web service request context.</param>
        /// <returns>Species observation access rights in Json format.</returns>
        public static String GetSpeciesObservationAccessRightsJson(this List<WebRole> roles,
                                                                   WebServiceContext context)
        {
            Int32 index;
            List<Int32> taxonIds;
            List<WebAuthority> authorities;
            List<WebPolygon> polygons;
            List<WebRegionGeography> regionGeographys;
            StringBuilder filter;
            WebCoordinateSystem speciesObservationCoordinateSystem;
                
            filter = new StringBuilder();
            if (roles.IsSimpleSpeciesObservationAccessRights())
            {
                filter.Append("{ \"range\": {");
                filter.Append(" \"Conservation_ProtectionLevel\": {");
                filter.Append(" \"lte\": " + roles.GetMaxProtectionLevel().WebToString());
                filter.Append("}}}");
            }
            else
            {
                authorities = new List<WebAuthority>();
                foreach (WebRole role in roles)
                {
                    if (role.Authorities.IsNotEmpty())
                    {
                        foreach (WebAuthority authority in role.Authorities)
                        {
                            if (authority.Identifier == AuthorityIdentifier.Sighting.ToString())
                            {
                                authorities.Add(authority);
                            }
                        }
                    }
                }

                if (authorities.IsNotEmpty())
                {
                    // Start OR condition.
                    filter.Append("{\"bool\":{ \"should\" : [");

                    filter.Append("{ \"range\": {");
                    filter.Append(" \"Conservation_ProtectionLevel\": {");
                    filter.Append(" \"lte\": " + roles.GetMaxProtectionLevelSimpleSpeciesObservationAccessRights().WebToString());
                    filter.Append("}}}");

                    foreach (WebAuthority authority in authorities)
                    {
                        if (!IsSimpleSpeciesObservationAccessRights(authority))
                        {
                            filter.Append(", ");

                            if (authority.RegionGUIDs.IsNotEmpty() ||
                                authority.TaxonGUIDs.IsNotEmpty())
                            {
                                // Start AND condition.
                                filter.Append("{\"bool\":{ \"must\" : [");
                            }

                            filter.Append("{ \"range\": {");
                            filter.Append(" \"Conservation_ProtectionLevel\": {");
                            filter.Append(" \"lte\": " + authority.MaxProtectionLevel.WebToString());
                            filter.Append("}}}");

                            if (authority.RegionGUIDs.IsNotEmpty())
                            {
                                filter.Append(", ");

                                speciesObservationCoordinateSystem = new WebCoordinateSystem();
                                speciesObservationCoordinateSystem.Id = CoordinateSystemId.WGS84;
                                regionGeographys = WebServiceData.RegionManager.GetRegionsGeographyByGuids(context,
                                                                                                           authority.RegionGUIDs,
                                                                                                           speciesObservationCoordinateSystem);
                                polygons = new List<WebPolygon>();
                                foreach (WebRegionGeography regionGeography in regionGeographys)
                                {
                                    polygons.AddRange(regionGeography.MultiPolygon.Polygons);
                                }

                                filter.Append("{ \"geo_shape\": {");
                                //filter.Append(" \"_cache\": true, ");
                                filter.Append(" \"Location\": {");
                                filter.Append(" \"shape\": {");
                                filter.Append(" \"type\": \"envelope\", ");
                                filter.Append(" \"coordinates\": ");
                                filter.Append(polygons.GetEnvelopeJson(speciesObservationCoordinateSystem, 10));
                                filter.Append("}}}}");

                                filter.Append(", ");

                                filter.Append("{ \"geo_shape\": {");
                                //filter.Append(" \"_cache\": true, ");
                                filter.Append(" \"Location\": {");
                                filter.Append(" \"shape\": {");
                                filter.Append(" \"type\": \"multipolygon\", ");
                                filter.Append(" \"coordinates\": ");
                                filter.Append(polygons.GetJson());
                                filter.Append("}}}}");
                            }

                            if (authority.TaxonGUIDs.IsNotEmpty())
                            {
                                filter.Append(", ");

                                taxonIds = new List<Int32>();
                                foreach (String taxonGuid in authority.TaxonGUIDs)
                                {
                                    taxonIds.Add(taxonGuid.WebParseInt32());
                                }

                                taxonIds = WebServiceData.TaxonManager.GetChildTaxonIds(context, taxonIds);
                                filter.Append("{ \"terms\": {");
                                filter.Append(" \"Taxon_DyntaxaTaxonID\":[");
                                filter.Append(taxonIds[0].WebToString());
                                for (index = 1; index < taxonIds.Count; index++)
                                {
                                    filter.Append(", " + taxonIds[index].WebToString());
                                }

                                filter.Append("]}}");
                            }

                            if (authority.RegionGUIDs.IsNotEmpty() ||
                                authority.TaxonGUIDs.IsNotEmpty())
                            {
                                // End AND condition.
                                filter.Append("]}}");
                            }
                        }
                    }

                    // End OR condition.
                    filter.Append("]}}");
                }
            }

            return filter.ToString();
        }

        /// <summary>
        /// Get max species observation access rights.
        /// </summary>
        /// <param name="roles">Current roles.</param>
        /// <returns>Max species observation access rights.</returns>
        public static Int32 GetMaxProtectionLevel(this List<WebRole> roles)
        {
            Int32 maxProtectionLevel;

            maxProtectionLevel = 1;
            if (roles.IsNotEmpty())
            {
                foreach (WebRole role in roles)
                {
                    if (role.Authorities.IsNotEmpty())
                    {
                        foreach (WebAuthority authority in role.Authorities)
                        {
                            if (authority.Identifier == AuthorityIdentifier.Sighting.ToString())
                            {
                                maxProtectionLevel = Math.Max(maxProtectionLevel,
                                                              authority.MaxProtectionLevel);
                            }
                        }
                    }
                }
            }

            return maxProtectionLevel;
        }

        /// <summary>
        /// Get max species observation access rights.
        /// </summary>
        /// <param name="roles">Current roles.</param>
        /// <returns>Max species observation access rights.</returns>
        public static Int32 GetMaxProtectionLevelSimpleSpeciesObservationAccessRights(this List<WebRole> roles)
        {
            Int32 maxProtectionLevel;

            maxProtectionLevel = 1;
            if (roles.IsNotEmpty())
            {
                foreach (WebRole role in roles)
                {
                    if (role.Authorities.IsNotEmpty())
                    {
                        foreach (WebAuthority authority in role.Authorities)
                        {
                            if ((authority.Identifier == AuthorityIdentifier.Sighting.ToString()) &&
                                IsSimpleSpeciesObservationAccessRights(authority))
                            {
                                maxProtectionLevel = Math.Max(maxProtectionLevel,
                                                              authority.MaxProtectionLevel);
                            }
                        }
                    }
                }
            }

            return maxProtectionLevel;
        }

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
        /// <param name="roles">Roles to test.</param>
        /// <returns>True, if access rights to species observations are easy to handle.</returns>
        public static Boolean IsSimpleSpeciesObservationAccessRights(this List<WebRole> roles)
        {
            if (roles.IsNotEmpty())
            {
                foreach (WebRole role in roles)
                {
                    if (role.Authorities.IsNotEmpty())
                    {
                        foreach (WebAuthority authority in role.Authorities)
                        {
                            if (!IsSimpleSpeciesObservationAccessRights(authority))
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            // No complex species observation access rights found.
            return true;
        }
    }
}

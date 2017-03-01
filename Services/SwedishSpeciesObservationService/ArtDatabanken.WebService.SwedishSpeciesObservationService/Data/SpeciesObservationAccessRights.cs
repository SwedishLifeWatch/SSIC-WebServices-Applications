using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Database;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Data
{
    /// <summary>
    /// This class contains enough information about a species
    /// observation to determine if a user with complicated species
    /// observation access rights has access to the observation.
    /// </summary>
    public class SpeciesObservationAccessRights
    {
        /// <summary>
        /// Not defined in Darwin Core.
        /// East-west value of the coordinate.
        /// The properties CoordinateX, CoordinateY, CoordinateZ,
        /// CoordinateM and CoordinateSystemWkt defines where the
        /// species observation was made.
        /// Which values that are valid depends on which
        /// coordinate system that is used.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public Double CoordinateX { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// North-south value of the coordinate.
        /// The properties CoordinateX, CoordinateY, CoordinateZ,
        /// CoordinateM and CoordinateSystemWkt defines where the
        /// species observation was made.
        /// Which values that are valid depends on which
        /// coordinate system that is used.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public Double CoordinateY { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Taxon id (not GUID) value in Dyntaxa.
        /// </summary>
        public Int32 DyntaxaTaxonId { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// SwedishSpeciesObservationService specific id
        /// for this species observation.
        /// The id is only used in communication with
        /// SwedishSpeciesObservationService and has no 
        /// meaning in other contexts.
        /// This id is currently not stable.
        /// The same observation may have another id tomorrow.
        /// In the future this id should be stable.
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Information about how protected information
        /// about a species is in Sweden.
        /// Currently this is a value between 1 to 6.
        /// 1 indicates public access and 6 is the highest security level.
        /// </summary>
        public Int32 ProtectionLevel { get; set; }

        /// <summary>
        /// Check if user has access right to this species observation.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authority">Check access right in this authority.</param>
        /// <returns>True if user has access right to this observation.</returns>
        private Boolean CheckAccessRights(WebServiceContext context,
                                          WebAuthority authority)
        {
            Dictionary<Int32, WebTaxon> taxa;
            List<WebRegionGeography> regionsGeography;
            WebPoint point;

            // Test if authority is related to species observations.
            if (authority.Identifier != AuthorityIdentifier.Sighting.ToString())
            {
                return false;
            }

            // Test if authority has enough protection level.
            if (authority.MaxProtectionLevel < ProtectionLevel)
            {
                return false;
            }

            // Test if species observation is inside regions.
            if (authority.RegionGUIDs.IsNotEmpty())
            {
                point = new WebPoint(CoordinateX,
                                     CoordinateY);
                regionsGeography = WebServiceData.RegionManager.GetRegionsGeographyByGuids(context,
                                                                                           authority.RegionGUIDs,
                                                                                           WebServiceData.SpeciesObservationManager.SpeciesObservationCoordinateSystem);
                if (!regionsGeography.IsPointInsideGeometry(context,
                                                            WebServiceData.SpeciesObservationManager.SpeciesObservationCoordinateSystem,
                                                            point))
                {
                    return false;
                }
            }

            // Test if species observation belongs to specified taxa.
            if (authority.TaxonGUIDs.IsNotEmpty())
            {
                taxa = WebServiceData.TaxonManager.GetTaxaByAuthority(context,
                                                                      authority);
                if (!taxa.ContainsKey(DyntaxaTaxonId))
                {
                    return false;
                }
            }

            // Species observation has passed all tests.
            // User has access right to this species observation.
            return true;
        }

        /// <summary>
        /// Check if user has access right to a species observation.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="role">Check access right in this role.</param>
        /// <returns>True if user has access right to provided observation.</returns>
        private Boolean CheckAccessRights(WebServiceContext context,
                                          WebRole role)
        {
            foreach (WebAuthority authority in role.Authorities)
            {
                if (CheckAccessRights(context, authority))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if user has access rights to this species observation.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>True if user has access right to this observation.</returns>
        public Boolean CheckAccessRights(WebServiceContext context)
        {
            Boolean hasAccessRight;

            hasAccessRight = false;
            foreach (WebRole role in context.CurrentRoles)
            {
                if (CheckAccessRights(context, role))
                {
                    hasAccessRight = true;
                    break;
                }
            }

            return hasAccessRight;
        }

        /// <summary>
        /// Load data into the SpeciesObservationAccessRights instance.
        /// </summary>
        /// <param name="dataReader">DataReader contains speciesObservationsAccessRights data.</param>
        public void Load(DataReader dataReader)
        {
            CoordinateX = dataReader.GetInt32(SpeciesObservationData.COORDINATE_X);
            CoordinateY = dataReader.GetInt32(SpeciesObservationData.COORDINATE_Y);
            DyntaxaTaxonId = dataReader.GetInt32(SpeciesObservationData.DYNTAXA_TAXON_ID);
            Id = dataReader.GetInt64(SpeciesObservationData.ID);
            ProtectionLevel = dataReader.GetInt32(SpeciesObservationData.PROTECTION_LEVEL);
        }
    }
}

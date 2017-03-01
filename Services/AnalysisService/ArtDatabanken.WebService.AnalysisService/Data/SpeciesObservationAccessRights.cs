using System;
using System.Collections.Generic;
using System.Web.Caching;
using ArtDatabanken.Data.WebService;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Database;
using TaxonList = ArtDatabanken.Data.ArtDatabankenService.TaxonList;
using TaxonTreeNode = ArtDatabanken.Data.ArtDatabankenService.TaxonTreeNode;
using TaxonTreeNodeList = ArtDatabanken.Data.ArtDatabankenService.TaxonTreeNodeList;
using TaxonTreeSearchCriteria = ArtDatabanken.Data.ArtDatabankenService.TaxonTreeSearchCriteria;

namespace ArtDatabanken.WebService.AnalysisService.Data
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
        public Double CoordinateX
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// North-south value of the coordinate.
        /// The properties CoordinateX, CoordinateY, CoordinateZ,
        /// CoordinateM and CoordinateSystemWkt defines where the
        /// species observation was made.
        /// Which values that are valid depends on which
        /// coordinate system that is used.
        /// </summary>
        public Double CoordinateY
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Taxon id (not GUID) value in Dyntaxa.
        /// </summary>
        public Int32 DyntaxaTaxonId
        { get; set; }

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
        public Int64 Id
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Information about how protected information
        /// about a species is in Sweden.
        /// Currently this is a value between 1 to 6.
        /// 1 indicates public access and 6 is the highest security level.
        /// </summary>
        public Int32 ProtectionLevel
        { get; set; }

        /// <summary>
        /// Check if user has access right to this species observation.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authority">Check access right in this authority.</param>
        /// <returns>True if user has access right to this observation.</returns>
        private Boolean CheckAccessRights(WebServiceContext context,
                                          WebAuthority authority)
        {
            List<WebRegionGeography> regionsGeography;
            TaxonList taxa;
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
                taxa = GetTaxa(context, authority);
                if (!taxa.Exists(DyntaxaTaxonId))
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
            if (context.IsNotNull())
            {
                foreach (WebRole role in context.CurrentRoles)
                {
                    if (CheckAccessRights(context, role))
                    {
                        hasAccessRight = true;
                        break;
                    }
                }
            }
            return hasAccessRight;
        }

        /// <summary>
        /// Get all child taxa.
        /// Parent taxa are also included in the result.
        /// </summary>
        /// <param name="taxonTree">Taxon tree.</param>
        /// <param name="taxa">Aggregated taxa.</param>
        private void GetChildTaxa(TaxonTreeNode taxonTree,
                                  TaxonList taxa)
        {
            taxa.Merge(taxonTree.Taxon);
            if (taxonTree.Children.IsNotEmpty())
            {
                foreach (TaxonTreeNode childTaxonTree in taxonTree.Children)
                {
                    GetChildTaxa(childTaxonTree, taxa);
                }
            }
        }

        /// <summary>
        /// Get all child taxa.
        /// Parent taxa are also included in the result.
        /// </summary>
        /// <param name="parentTaxonIds">Ids for parent taxa.</param>
        /// <param name="taxonInformationType">Type of taxon information to get.</param>
        /// <returns>All child taxa.</returns>
        public TaxonList GetChildTaxa(List<Int32> parentTaxonIds,
                                      TaxonInformationType taxonInformationType)
        {
            TaxonList childTaxa;
            TaxonTreeNodeList taxonTrees;
            TaxonTreeSearchCriteria taxonTreeSearchCriteria;

            taxonTreeSearchCriteria = new TaxonTreeSearchCriteria();
            taxonTreeSearchCriteria.RestrictSearchToTaxonIds = parentTaxonIds;
            taxonTreeSearchCriteria.TaxonInformationType = taxonInformationType;
            taxonTrees =
                ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxonTreesBySearchCriteria(
                    taxonTreeSearchCriteria);
            childTaxa = new TaxonList(true);
            if (taxonTrees.IsNotEmpty())
            {
                foreach (TaxonTreeNode taxonTree in taxonTrees)
                {
                    GetChildTaxa(taxonTree, childTaxa);
                }
            }
            return childTaxa;
        }

        /// <summary>
        /// Get all child taxa.
        /// Parent taxa are also included in the result.
        /// </summary>
        /// <param name="parentTaxonGuids">GUIDs for parent taxa.</param>
        /// <param name="taxonInformationType">Type of taxon information to get.</param>
        /// <returns>All child taxa.</returns>
        private TaxonList GetChildTaxa(List<String> parentTaxonGuids,
                                       TaxonInformationType taxonInformationType)
        {
            List<Int32> parentTaxonIds;

            parentTaxonIds = new List<Int32>();
            foreach (String parentTaxonGuid in parentTaxonGuids)
            {
                // TODO: This assumption about taxon GUIDs may
                // change in the future.
                parentTaxonIds.Add(Int32.Parse(parentTaxonGuid));
            }
            return GetChildTaxa(parentTaxonIds, taxonInformationType);
        }

        /// <summary>
        /// Get taxa that belongs to authority.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authority">Check access right in this authority.</param>
        /// <returns>Taxa that belongs to authority.</returns>
        private TaxonList GetTaxa(WebServiceContext context,
                                  WebAuthority authority)
        {
            String authorityTaxaCacheKey;
            TaxonList taxa;

            // Get cached information.
            authorityTaxaCacheKey = Settings.Default.AuthorityTaxaCacheKey +
                                    WebService.Settings.Default.CacheKeyDelimiter +
                                    authority.Id;
            taxa = (TaxonList)(context.GetCachedObject(authorityTaxaCacheKey));

            // Data not in cache - store it in the cache.
            if (taxa.IsNull())
            {
                taxa = GetChildTaxa(authority.TaxonGUIDs,
                                    TaxonInformationType.Basic);

                // Add information to cache.
                context.AddCachedObject(authorityTaxaCacheKey,
                                        taxa,
                                        DateTime.Now + new TimeSpan(0, 1, 0, 0),
                                        CacheItemPriority.BelowNormal);
            }

            return taxa;
        }

        /// <summary>
        /// Load data into the SpeciesObservationAccessRights instance.
        /// </summary>
        /// <param name="dataReader"></param>
        public void Load(DataReader dataReader)
        {
            if (dataReader.IsNotNull())
            {
                this.CoordinateX = dataReader.GetInt32(SpeciesObservationData.COORDINATE_X);
                this.CoordinateY = dataReader.GetInt32(SpeciesObservationData.COORDINATE_Y);
                this.DyntaxaTaxonId = dataReader.GetInt32(SpeciesObservationData.DYNTAXA_TAXON_ID);
                this.Id = dataReader.GetInt64(SpeciesObservationData.ID);
                this.ProtectionLevel = dataReader.GetInt32(SpeciesObservationData.PROTECTION_LEVEL);
            }
        }
    }
}

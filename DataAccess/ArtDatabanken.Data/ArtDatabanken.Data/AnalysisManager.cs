using System;
using System.Collections.Generic;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Analysis manager processes any type of data.
    /// </summary>
    public class AnalysisManager : IAnalysisManager
    {
        /// <summary>
        /// This property is used to retrieve or update information.
        /// </summary>
        public IAnalysisDataSource DataSource { get; set; }

        /// <summary>
        /// Get unique hosts for all species facts
        /// that matches search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Hosts for all species facts that matches search criteria.</returns>
        public virtual TaxonList GetHosts(IUserContext userContext,
                                          ISpeciesFactSearchCriteria searchCriteria)
        {
            return DataSource.GetHosts(userContext, searchCriteria);
        }

        /// <summary>
        /// Get number of species  that matches the search criteria.
        /// Scope is restricted to those species that the user has access rights to.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>Number of species.</returns>
        public Int64 GetSpeciesCountBySearchCriteria(IUserContext userContext,
                                                     ISpeciesObservationSearchCriteria searchCriteria,
                                                     ICoordinateSystem coordinateSystem)
        {
            return DataSource.GetSpeciesCountBySearchCriteria(userContext, searchCriteria, coordinateSystem);
        }


        /// <summary>
        /// Get number of species observations that matches the search criteria.
        /// Scope is restricted to those observations that the user has access rights to.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>Number of species observations.</returns>
        public Int64 GetSpeciesObservationCountBySearchCriteria(IUserContext userContext,
                                                                ISpeciesObservationSearchCriteria searchCriteria,
                                                                ICoordinateSystem coordinateSystem)
        {
            return CoreData.SpeciesObservationManager.GetSpeciesObservationCount(userContext, searchCriteria, coordinateSystem);
            //return DataSource.GetSpeciesObservationCountBySearchCriteria(userContext, searchCriteria, coordinateSystem);
        }

        /// <summary>
        /// Get species observation provenances that matches the search criteria.
        /// Scope is restricted to those observations that the user has access rights to.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>List of species observation provenances.</returns>
        public List<SpeciesObservationProvenance> GetSpeciesObservationProvenancesBySearchCriteria(IUserContext userContext,
                                                                ISpeciesObservationSearchCriteria searchCriteria,
                                                                ICoordinateSystem coordinateSystem)
        {
            return DataSource.GetSpeciesObservationProvenancesBySearchCriteria(userContext, searchCriteria, coordinateSystem);
        }

        /// <summary>
        /// Get information about spatial features in a grid representation inside a user supplied bounding box.
        /// </summary>
        /// /// <param name="userContext">User context.</param>
        /// <param name="featureStatistics">Information about what statistics are requested from a web feature 
        /// service and wich spatial feature type that is to be measured</param>
        /// <param name="featuresUrl">String with the requested url.</param>
        /// <param name="featureCollectionJson">Feature collection as json string.</param>
        /// <param name="gridSpecification">Specifications of requested grid cell size, requested coordinate system 
        /// and user supplied bounding box.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>Statistical measurements on spatial features in grid format.</returns>
        public List<IGridCellFeatureStatistics> GetGridFeatureStatistics(IUserContext userContext,
            FeatureStatisticsSummary featureStatistics, String featuresUrl, String featureCollectionJson, IGridSpecification gridSpecification, ICoordinateSystem coordinateSystem)
        {
            //Todo: Check data for weird combinations like area of point features and such. Use existing check-methods.
            return DataSource.GetGridCellFeatureStatistics(userContext, featureStatistics, featuresUrl, featureCollectionJson, gridSpecification, coordinateSystem);
            
        }


        /// <summary>
        /// Gets no of species observations
        /// that matches the search criteria and grid specifications.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="gridSpecification">The species observation search criteria, GridCellSize and GridCoordinateSyatem are the
        /// only properties that is used in this method.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>Information about changed species observations.</returns>
        public IList<IGridCellSpeciesObservationCount> GetGridSpeciesObservationCounts(IUserContext userContext,
                                                                                       ISpeciesObservationSearchCriteria searchCriteria,
                                                                                       IGridSpecification gridSpecification, 
                                                                                       ICoordinateSystem coordinateSystem)
        {
            return DataSource.GetGridSpeciesObservationCounts(userContext, searchCriteria, gridSpecification, coordinateSystem);
        }

        /// <summary>
        /// Get EOO as geojson, EOO and AOO area as attributes
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="gridCells">Grid cells used to calculate result</param>
        /// <param name="alphaValue"> If greater than 0 a concave hull will be calculated with this alpha value</param>
        /// <param name="useCenterPoint">Used when concave hull is calculated. Grid corner coordinates used when false</param>
        /// <returns>A JSON FeatureCollection with one feature showing EOO. EOO AND AOO Areas stored in feature attributes</returns>
        public string GetSpeciesObservationAOOEOOAsGeoJson(IUserContext userContext, IList<IGridCellSpeciesObservationCount> gridCells, int alphaValue = 0, bool useCenterPoint = true)
        {
            return DataSource.GetSpeciesObservationAOOEOOAsGeoJson(userContext, gridCells, alphaValue, useCenterPoint);
        }

        /// <summary>
        /// Gets the grid cell feature statistics combined with species observation counts.
        /// </summary>        
        /// <param name="userContext">User context.</param>
        /// <param name="gridSpecification">The grid specification: bounding box, grid cell size, etc.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="featureStatistics">Information about requested information from a web feature service.</param>
        /// <param name="featuresUrl">Address to data in a web feature service.</param>
        /// <param name="featureCollectionJson">Feature collection json.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned grid.</param>
        /// <returns>A list with combined result from GetGridSpeciesCounts() and GetGridCellFeatureStatistics().</returns>
        public IList<IGridCellCombinedStatistics> GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts(
            IUserContext userContext,
            IGridSpecification gridSpecification,
            ISpeciesObservationSearchCriteria searchCriteria,
            FeatureStatisticsSummary featureStatistics,
            String featuresUrl,
            String featureCollectionJson,
            ICoordinateSystem coordinateSystem)
        {
            return DataSource.GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts(userContext,
                                                                                               gridSpecification,
                                                                                               searchCriteria,
                                                                                               featureStatistics,
                                                                                               featuresUrl,
                                                                                               featureCollectionJson,
                                                                                               coordinateSystem);
        }

        /// <summary>
        /// Gets no of species 
        /// that matches the search criteria and grid specifications.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="gridSpecification">The species observation search criteria, GridCellSize and GridCoordinateSystem are the
        /// only properties that is used in this method.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>Information about species.</returns>
        public IList<IGridCellSpeciesCount> GetGridSpeciesCounts(IUserContext userContext,
                                                                            ISpeciesObservationSearchCriteria searchCriteria,
                                                                            IGridSpecification gridSpecification,
                                                                            ICoordinateSystem coordinateSystem)
        {
            return DataSource.GetGridSpeciesCounts(userContext, searchCriteria, gridSpecification, coordinateSystem);
        }

        /// <summary>
        /// Get taxa that belongs to specified organism group.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organismGroup">Organism group.</param>
        /// <returns>Taxa that belongs to specified organism group.</returns>
        public virtual TaxonList GetTaxa(IUserContext userContext,
                                         IOrganismGroup organismGroup)
        {
            IFactor factor;
            ISpeciesFactFieldSearchCriteria fieldSearchCriteria;
            ISpeciesFactSearchCriteria searchCriteria;
            TaxonList taxa;

            taxa = null;
            if (organismGroup.IsNotNull())
            {
                searchCriteria = new SpeciesFactSearchCriteria();
                factor = CoreData.FactorManager.GetFactor(userContext,
                                                          FactorId.Redlist_OrganismLabel1);
                searchCriteria.Add(factor);
                searchCriteria.Add(CoreData.FactorManager.GetDefaultIndividualCategory(userContext));
                searchCriteria.IncludeNotValidHosts = false;
                searchCriteria.IncludeNotValidTaxa = false;
                fieldSearchCriteria = new SpeciesFactFieldSearchCriteria();
                fieldSearchCriteria.FactorField = factor.DataType.Field1;
                fieldSearchCriteria.Operator = CompareOperator.Equal;
                fieldSearchCriteria.AddValue(organismGroup.Id);
                searchCriteria.Add(fieldSearchCriteria);
                taxa = DataSource.GetTaxa(userContext, searchCriteria);
            }

            return taxa;
        }

        /// <summary>
        /// Get taxa that belongs to specified organism groups.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organismGroups">Organism groups.</param>
        /// <returns>Taxa that belongs to specified organism groups.</returns>
        public virtual TaxonList GetTaxa(IUserContext userContext,
                                         OrganismGroupList organismGroups)
        {
            IFactor factor;
            ISpeciesFactFieldSearchCriteria fieldSearchCriteria;
            ISpeciesFactSearchCriteria searchCriteria;
            TaxonList taxa;

            taxa = null;
            if (organismGroups.IsNotEmpty())
            {
                searchCriteria = new SpeciesFactSearchCriteria();
                factor = CoreData.FactorManager.GetFactor(userContext,
                                                          FactorId.Redlist_OrganismLabel1);
                searchCriteria.Add(factor);
                searchCriteria.IncludeNotValidHosts = false;
                searchCriteria.IncludeNotValidTaxa = false;
                searchCriteria.Add(CoreData.FactorManager.GetDefaultIndividualCategory(userContext));
                searchCriteria.FieldLogicalOperator = LogicalOperator.Or;
                foreach (IOrganismGroup organismGroup in organismGroups)
                {
                    fieldSearchCriteria = new SpeciesFactFieldSearchCriteria();
                    fieldSearchCriteria.FactorField = factor.DataType.Field1;
                    fieldSearchCriteria.Operator = CompareOperator.Equal;
                    fieldSearchCriteria.AddValue(organismGroup.Id);
                    searchCriteria.Add(fieldSearchCriteria);
                }

                taxa = DataSource.GetTaxa(userContext, searchCriteria);
            }

            return taxa;
        }

        /// <summary>
        /// Get unique taxa for all species facts
        /// that matches search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa for all species facts that matches search criteria.</returns>
        public virtual TaxonList GetTaxa(IUserContext userContext,
                                         ISpeciesFactSearchCriteria searchCriteria)
        {
            return DataSource.GetTaxa(userContext, searchCriteria);
        }

        /// <summary>
        /// Gets taxa that matches the specified species observation search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>Information about taxa.</returns>
        public TaxonList GetTaxaBySearchCriteria(IUserContext userContext,
                                                 ISpeciesObservationSearchCriteria searchCriteria,
                                                 ICoordinateSystem coordinateSystem)
        {
            TaxonList taxa = DataSource.GetTaxaBySearchCriteria(userContext, searchCriteria, coordinateSystem);
            taxa.Sort();
            return taxa;
        }

        /// <summary>
        /// Gets taxa, with related number of observed species, that matches the specified species observation search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>Information about taxa.</returns>
        public TaxonSpeciesObservationCountList GetTaxaWithSpeciesObservationCountsBySearchCriteria(IUserContext userContext,
                                                 ISpeciesObservationSearchCriteria searchCriteria,
                                                 ICoordinateSystem coordinateSystem)
        {
            TaxonSpeciesObservationCountList taxaSpeciesObservationCount = DataSource.GetTaxaWithSpeciesObservationCountsBySearchCriteria(userContext, searchCriteria, coordinateSystem);
            taxaSpeciesObservationCount.Sort();
            return taxaSpeciesObservationCount;
        }

        /// <summary>
        /// Get information about data source.
        /// </summary>
        /// <returns>Information about data source.</returns>
        public virtual IDataSourceInformation GetDataSourceInformation()
        {
            return DataSource.GetDataSourceInformation();
        }

        /// <summary>
        /// Gets a time serie with species observation counts based on time step specifications and observation search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="periodicity">Specification on time step length and interval.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>A list of time step specific counts of species observations.</returns>
        public TimeStepSpeciesObservationCountList GetTimeSpeciesObservationCounts(IUserContext userContext,
                                                                            ISpeciesObservationSearchCriteria searchCriteria,
                                                                            Periodicity periodicity,
                                                                            ICoordinateSystem coordinateSystem)
        {
            return DataSource.GetTimeSpeciesObservationCounts(userContext, searchCriteria, periodicity, coordinateSystem);
        }

    }
}

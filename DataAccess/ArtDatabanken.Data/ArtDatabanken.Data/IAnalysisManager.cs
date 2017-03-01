using System;
using System.Collections.Generic;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Interface for manager that handles species observation counts and calculations.
    /// </summary>
    public interface IAnalysisManager
    {
        /// <summary>
        /// This property is used to retrieve or update information.
        /// </summary>
        IAnalysisDataSource DataSource { get; set; }

        /// <summary>
        /// Get unique hosts for all species facts
        /// that matches search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Hosts for all species facts that matches search criteria.</returns>
        TaxonList GetHosts(IUserContext userContext,
                           ISpeciesFactSearchCriteria searchCriteria);

        /// <summary>
        /// Get number of species  that matches the search criteria.
        /// Scope is restricted to those species that the user has access rights to.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>Number of species.</returns>
        Int64 GetSpeciesCountBySearchCriteria(IUserContext userContext,
                                              ISpeciesObservationSearchCriteria searchCriteria,
                                              ICoordinateSystem coordinateSystem);

        /// <summary>
        /// Get number of species observations that matches the search criteria.
        /// Scope is restricted to those observations that the user has access rights to.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criterias.</param>
        /// <returns>Number of species observations.</returns>
        Int64 GetSpeciesObservationCountBySearchCriteria(IUserContext userContext,
                                                                          ISpeciesObservationSearchCriteria searchCriteria,
                                                                          ICoordinateSystem coordinateSystem);

        /// <summary>
        /// Get species observation provenances that matches the search criteria.
        /// Scope is restricted to those observations that the user has access rights to.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criterias.</param>
        /// <returns>List of species observation provenances.</returns>
        List<SpeciesObservationProvenance> GetSpeciesObservationProvenancesBySearchCriteria(IUserContext userContext,
                                                                          ISpeciesObservationSearchCriteria searchCriteria,
                                                                          ICoordinateSystem coordinateSystem);

        /// <summary>
        /// Get information about spatial features in a grid representation inside a user supplied bounding box.
        /// </summary>
        /// /// <param name="userContext">User context.</param>
        /// <param name="featureStatistics">Information about what statistics are requested from a web feature 
        /// service and wich spatial feature type that is to be measured</param>
        /// <param name="featuresUrl">Resource address.</param>
        /// <param name="featureCollectionJson">Feature collection as json string.</param>
        /// <param name="gridSpecification">Specifications of requested grid cell size, requested coordinate system 
        /// and user supplied bounding box.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criterias.</param>
        /// <returns>Statistical measurements on spatial features in grid format.</returns>
        List<IGridCellFeatureStatistics> GetGridFeatureStatistics(IUserContext userContext, FeatureStatisticsSummary featureStatistics,
            String featuresUrl, String featureCollectionJson, IGridSpecification gridSpecification, ICoordinateSystem coordinateSystem);

        /// <summary>
        /// Gets the grid cell feature statistics combined with species observation counts.
        /// </summary>        
        /// <param name="userContext">User context.</param>
        /// <param name="gridSpecification">The grid specification: bounding box, grid cell size, etc.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="featureStatistics">Information about requested information from a web feature service.</param>
        /// <param name="featuresUrl">Address to data in a web feature service.</param>
        /// <param name="featureCollectionJson">Feature collection as json.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned grid.</param>
        /// <returns>A list with combined result from GetGridSpeciesCounts() and GetGridCellFeatureStatistics().</returns>
        IList<IGridCellCombinedStatistics> GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts(
            IUserContext userContext,
            IGridSpecification gridSpecification,
            ISpeciesObservationSearchCriteria searchCriteria,
            FeatureStatisticsSummary featureStatistics,
            String featuresUrl,
            String featureCollectionJson,
            ICoordinateSystem coordinateSystem
            );


        /// <summary>
        /// Gets features in a grid cell that matches the feature type request and grid specifications.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="gridSpecification">Specifications of requested grid.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criterias.</param>
        /// <returns>A list of gridcells with information about the features they contain.</returns>
        IList<IGridCellSpeciesObservationCount> GetGridSpeciesObservationCounts(IUserContext userContext,
                                                                                    ISpeciesObservationSearchCriteria searchCriteria,
                                                                                    IGridSpecification gridSpecification,
                                                                                    ICoordinateSystem coordinateSystem);

        /// <summary>
        /// Get EOO as geojson, EOO and AOO area as attributes
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="gridCells">Grid cells used to calculate result</param>
        /// <param name="alphaValue"> If greater than 0 a concave hull will be calculated with this alpha value</param>
        /// <param name="useCenterPoint">Used when concave hull is calculated. Grid corner coordinates used when false</param>
        /// <returns>A JSON FeatureCollection with one feature showing EOO. EOO AND AOO Areas stored in feature attributes</returns>
        string GetSpeciesObservationAOOEOOAsGeoJson(IUserContext userContext, IList<IGridCellSpeciesObservationCount> gridCells, int alphaValue = 0, bool useCenterPoint = true);

        ///<summary>
        /// Gets no of species 
        /// that matches the search criteria and grid specifications.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="gridSpecification">The species observation search criteria, GridCellSize and GridCoordinateSyatem are the
        /// only properties that is used in this method.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criterias.</param>
        /// <returns>Information about species.</returns>
        IList<IGridCellSpeciesCount> GetGridSpeciesCounts(IUserContext userContext,
                                                          ISpeciesObservationSearchCriteria searchCriteria,
                                                          IGridSpecification gridSpecification,
                                                          ICoordinateSystem coordinateSystem);

        /// <summary>
        /// Get taxa that belongs to specified organism group.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organismGroup">Organism group.</param>
        /// <returns>Taxa that belongs to specified organism group.</returns>
        TaxonList GetTaxa(IUserContext userContext,
                          IOrganismGroup organismGroup);

        /// <summary>
        /// Get taxa that belongs to specified organism groups.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organismGroups">Organism groups.</param>
        /// <returns>Taxa that belongs to specified organism groups.</returns>
        TaxonList GetTaxa(IUserContext userContext,
                          OrganismGroupList organismGroups);

        /// <summary>
        /// Get unique taxa for all species facts
        /// that matches search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa for all species facts that matches search criteria.</returns>
        TaxonList GetTaxa(IUserContext userContext,
                          ISpeciesFactSearchCriteria searchCriteria);

        /// <summary>
        /// Gets taxa 
        /// that matches the search criteria and grid specifications.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.
        /// only properties that is used in this method.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criterias.</param>
        /// <returns>Information about taxa.</returns>
        TaxonList GetTaxaBySearchCriteria(IUserContext userContext,
                                          ISpeciesObservationSearchCriteria searchCriteria,
                                          ICoordinateSystem coordinateSystem);

        /// <summary>
        /// Gets taxa, with related number of observed species,
        /// that matches the search criteria and grid specifications.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.
        /// only properties that is used in this method.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criterias.</param>
        /// <returns>Information about taxa.</returns>
        TaxonSpeciesObservationCountList GetTaxaWithSpeciesObservationCountsBySearchCriteria(IUserContext userContext,
                                          ISpeciesObservationSearchCriteria searchCriteria,
                                          ICoordinateSystem coordinateSystem);

        /// <summary>
        /// Gets a time serie with species observation counts based on time step specifications and observation search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="periodicity">Specification on time step length and interval.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>A list of time step specific counts of species observations.</returns>
        TimeStepSpeciesObservationCountList GetTimeSpeciesObservationCounts(IUserContext userContext,
                                                                            ISpeciesObservationSearchCriteria searchCriteria,
                                                                            Periodicity periodicity,
                                                                            ICoordinateSystem coordinateSystem);
         
    }
}

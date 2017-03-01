using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data.DataSource
{
     /// <summary>
    /// Definition of the SpeciesObservationDataProvider interface.
    /// </summary>
    public interface ISpeciesObservationDataSource : IDataSource
    {
        /// <summary>
        /// Get all bird nest activities.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>Information about bird nest activities.</returns>
        SpeciesActivityList GetBirdNestActivities(IUserContext userContext);

        /// <summary>
        /// Get all county regions
        /// GetCountyRegions is for searching observations in SpeciesObservationService and AnalysisServce only.
        /// The regions does not contain polygons
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <returns>Information about county regions</returns>
        RegionList GetCountyRegions(IUserContext userContext);


        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// This method provides paging functionality of the result.
        /// Max page size is 10000 species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Species observations are returned in a format
        /// that is compatible with Darwin Core 1.5.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in geometry search criteria
        /// and returned species observations.
        /// </param>
        /// <param name="pageSpecification">
        /// Specification of paging information when
        /// species observations are retrieved.
        /// </param>
        /// <returns>Information about requested species observations.</returns>
        DarwinCoreList GetDarwinCore(IUserContext userContext,
                                     ISpeciesObservationSearchCriteria searchCriteria,
                                     ICoordinateSystem coordinateSystem,
                                     ISpeciesObservationPageSpecification pageSpecification);

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in geometry search criteria
        /// and returned species observations.
        /// </param>
        /// <param name="sortOrder">
        /// Defines how species observations should be sorted.
        /// This parameter is optional. Random order is used
        /// if no sort order has been specified.
        /// </param>
        /// <returns>Information about species observations.</returns>
        DarwinCoreList GetDarwinCore(IUserContext userContext,
                                     ISpeciesObservationSearchCriteria searchCriteria,
                                     ICoordinateSystem coordinateSystem,
                                     SpeciesObservationFieldSortOrderList sortOrder);

        /// <summary>
        /// Get requested species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Species observations are returned in a format
        /// that is compatible with Darwin Core 1.5.
        /// Max 25000 observations with information
        /// can be retrieved in one call.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="speciesObservationIds">Id for species observations to get.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in returned species observations.
        /// </param>
        /// <returns>Requested species observations.</returns>
        DarwinCoreList GetDarwinCore(IUserContext userContext,
                                     List<Int64> speciesObservationIds,
                                     ICoordinateSystem coordinateSystem);

        /// <summary>
        /// Get information about species observations that has
        /// changed.
        /// 
        /// Scope is restricted to those observations that the
        /// user has access rights to. There is no access right
        /// check on deleted species observations. This means
        /// that a client can obtain information about deleted
        /// species observations that the client has not
        /// received any create or update information about.
        /// 
        /// Max 25000 species observation changes can be
        /// retrieved in one web service call.
        /// Exactly one of the parameters changedFrom and 
        /// changeId should be specified.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="changedFrom">Start date and time for changes that should be returned.</param>
        /// <param name="changedTo">
        /// End date and time for changes that should be
        /// returned. Parameter changedTo is optional and works
        /// with either parameter changedFrom or changeId.
        /// </param>
        /// <param name="changeId">
        /// Start id for changes that should be returned.
        /// The species observation that is changed in the
        /// specified change id may be included in returned
        /// information.
        /// </param>
        /// <param name="maxReturnedChanges">
        /// Requested maximum number of changes that should
        /// be returned. This property is used by the client
        /// to avoid problems with resource limitations on
        /// the client side.
        /// Max 25000 changes are returned if property
        /// maxChanges has a higher value than 25000.
        /// </param>
        /// <param name="searchCriteria">
        /// Only species observations that matches the search 
        /// criteria are included in the returned information.
        /// This parameter is optional and may be null.
        /// There is no check on search criteria for
        /// deleted species observations.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned species observations.</param>
        /// <returns>
        /// Information about changed species observations.
        /// </returns>
        IDarwinCoreChange GetDarwinCoreChange(IUserContext userContext,
                                              DateTime? changedFrom,
                                              DateTime? changedTo,
                                              Int64? changeId,
                                              Int64 maxReturnedChanges,
                                              ISpeciesObservationSearchCriteria searchCriteria,
                                              ICoordinateSystem coordinateSystem);

        /// <summary>
        /// Get an indication if specified geometries contains any
        /// protected species observations.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">
        /// The species observation search criteria.
        /// At least one of BoundingBox, Polygons and RegionGuids
        /// must be specified.
        /// Search criteria that may be used: Accuracy,
        /// BirdNestActivityLimit, BoundingBox, IsAccuracyConsidered,
        /// IsDisturbanceSensitivityConsidered, MinProtectionLevel,
        /// ObservationDateTime, Polygons and RegionGuids.
        /// </param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>
        /// True, if specified geometries contains any
        /// protected species observations.
        /// </returns>
        Boolean GetProtectedSpeciesObservationIndication(IUserContext userContext,
                                                         ISpeciesObservationSearchCriteria searchCriteria,
                                                         ICoordinateSystem coordinateSystem);

        /// <summary>
        /// Get all province regions
        /// GetProvinceRegions is for searching observations in SpeciesObservationService and AnalysisServce only.
        /// The regions does not contain polygons
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <returns>Information about province regions</returns>
        RegionList GetProvinceRegions(IUserContext userContext);

        /// <summary>
        /// Get requested species activities.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All species activities.</returns>
        SpeciesActivityList GetSpeciesActivities(IUserContext userContext);

         /// <summary>
         /// Get requested species activity categories.
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <returns>All species activities categories.</returns>
         SpeciesActivityCategoryList GetSpeciesActivityCategories(IUserContext userContext);

        /// <summary>
        /// Get information about species observations that has
        /// changed.
        /// 
        /// Scope is restricted to those observations that the
        /// user has access rights to. There is no access right
        /// check on deleted species observations. This means
        /// that a client can obtain information about deleted
        /// species observations that the client has not
        /// received any create or update information about.
        /// 
        /// Max 25000 species observation changes can be
        /// retrieved in one web service call.
        /// Exactly one of the parameters changedFrom and 
        /// changeId should be specified.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="changedFrom">Start date and time for changes that should be returned.</param>
        /// <param name="changedTo">
        /// End date and time for changes that should be
        /// returned. Parameter changedTo is optional and works
        /// with either parameter changedFrom or changeId.
        /// </param>
        /// <param name="changeId">
        /// Start id for changes that should be returned.
        /// The species observation that is changed in the
        /// specified change id may be included in returned
        /// information.
        /// </param>
        /// <param name="maxReturnedChanges">
        /// Requested maximum number of changes that should
        /// be returned. This property is used by the client
        /// to avoid problems with resource limitations on
        /// the client side.
        /// Max 25000 changes are returned if property
        /// maxChanges has a higher value than 25000.
        /// </param>
        /// <param name="searchCriteria">
        /// Only species observations that matches the search 
        /// criteria are included in the returned information.
        /// This parameter is optional and may be null.
        /// There is no check on search criteria for
        /// deleted species observations. </param>
        /// <param name="coordinateSystem">Coordinate system used in returned species observations. </param>
        /// <param name="speciesObservationSpecification">
        /// Specify which subset of the data that should be
        /// returned for each species observation.
        /// All information for each species observation is
        /// returned if this parameter is null. 
        /// This parameter is currently not used.
        /// </param>
        /// <returns>
        /// Information about changed species observations.
        /// </returns>
        ISpeciesObservationChange GetSpeciesObservationChange(IUserContext userContext,
                                                              DateTime? changedFrom,
                                                              DateTime? changedTo,
                                                              Int64? changeId,
                                                              Int64 maxReturnedChanges,
                                                              ISpeciesObservationSearchCriteria searchCriteria,
                                                              ICoordinateSystem coordinateSystem,
                                                              ISpeciesObservationSpecification speciesObservationSpecification);

        /// <summary>
        /// Get requested information about the data providers.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>A list of data sources.</returns>
        SpeciesObservationDataProviderList GetSpeciesObservationDataProviders(IUserContext userContext);

        /// <summary>
        /// Retrieve all Species Observation Field Descriptions.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>A Species Observation Field Description List.</returns>
        SpeciesObservationFieldDescriptionList GetSpeciesObservationFieldDescriptions(IUserContext userContext);

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// This method provides paging functionality of the result.
        /// Max page size is 10000 species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in geometry search criteria
        /// and returned species observations.
        /// </param>
        /// <param name="pageSpecification">
        /// SpecificationId of paging information when
        /// species observations are retrieved.
        /// </param>
        /// <param name="speciesObservationSpecification">
        /// Specify which subset of the data that should be
        /// returned for each species observation.
        /// All information for each species observation is
        /// returned if this parameter is null. 
        /// This parameter is currently not used.
        /// </param>
        /// <returns>Information about requested species observations.</returns>
        SpeciesObservationList GetSpeciesObservations(IUserContext userContext,
                                                      ISpeciesObservationSearchCriteria searchCriteria,
                                                      ICoordinateSystem coordinateSystem,
                                                      ISpeciesObservationPageSpecification pageSpecification,
                                                      ISpeciesObservationSpecification speciesObservationSpecification);

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// Max 25000 observations with information
        /// can be retrieved in one call.
        /// Max 1000000 observation ids can be retrieved in one call.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria
        /// and returned species observations. </param>
        /// <param name="speciesObservationSpecification">
        /// Specify which subset of the data that should be
        /// returned for each species observation.
        /// All information for each species observation is
        /// returned if this parameter is null. 
        /// This parameter is currently not used.
        /// </param>
        /// <param name="sortOrder">
        /// Defines how species observations should be sorted.
        /// This parameter is optional. Random order is used
        /// if no sort order has been specified.
        /// </param>
        /// <returns>Information about requested species observations.</returns>
        SpeciesObservationList GetSpeciesObservations(IUserContext userContext,
                                                      ISpeciesObservationSearchCriteria searchCriteria,
                                                      ICoordinateSystem coordinateSystem,
                                                      ISpeciesObservationSpecification speciesObservationSpecification,
                                                      SpeciesObservationFieldSortOrderList sortOrder);

        /// <summary>
        /// Get requested species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Species observations are returned in a format
        /// that is compatible with Darwin Core 1.5.
        /// Max 25000 observations with information
        /// can be retrieved in one call.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="speciesObservationIds">Id for species observations to get.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in returned species observations.
        /// </param>
        /// <param name="speciesObservationSpecification">
        /// Specify which subset of the data that should be
        /// returned for each species observation.
        /// All information for each species observation is
        /// returned if this parameter is null. 
        /// This parameter is currently not used.
        /// </param>
        /// <returns>Species observations.</returns>
        SpeciesObservationList GetSpeciesObservations(IUserContext userContext,
                                                      List<Int64> speciesObservationIds,
                                                      ICoordinateSystem coordinateSystem,
                                                      ISpeciesObservationSpecification speciesObservationSpecification);

        /// <summary>
        /// Get number of species observations that matches the search criteria.
        /// Scope is restricted to those observations that the user has access rights to.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criterias.</param>
        /// <returns>Number of species observations.</returns>
        Int64 GetSpeciesObservationCount(IUserContext userContext,
                                                       ISpeciesObservationSearchCriteria searchCriteria,
                                                       ICoordinateSystem coordinateSystem);
    }
}
using System;
using System.Collections.Generic;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class for manager that handles species observation information.
    /// </summary>
    public class SpeciesObservationManager : ISpeciesObservationManager
    {
        /// <summary>
        /// This property is used to retrieve or update information.
        /// </summary>
        public ISpeciesObservationDataSource DataSource { get; set; }

        /// <summary>
        /// Get all bird nest activities.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>Information about bird nest activities.</returns>
        public virtual SpeciesActivityList GetBirdNestActivities(IUserContext userContext)
        {
            return DataSource.GetBirdNestActivities(userContext);
        }

        /// <summary>
        /// Get all county regions
        /// GetCountyRegions is for searching observations in SpeciesObservationService and AnalysisServce only.
        /// The regions does not contain polygons
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <returns>Information about county regions</returns>
        public virtual RegionList GetCountyRegions(IUserContext userContext)
        {
            return DataSource.GetCountyRegions(userContext);
        }

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
        public virtual DarwinCoreList GetDarwinCore(IUserContext userContext,
                                                    ISpeciesObservationSearchCriteria searchCriteria,
                                                    ICoordinateSystem coordinateSystem,
                                                    ISpeciesObservationPageSpecification pageSpecification)
        {
            return DataSource.GetDarwinCore(userContext,
                                                                searchCriteria,
                                                                coordinateSystem,
                                                                pageSpecification);
        }

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
        public virtual DarwinCoreList GetDarwinCore(IUserContext userContext,
                                                    ISpeciesObservationSearchCriteria searchCriteria,
                                                    ICoordinateSystem coordinateSystem,
                                                    SpeciesObservationFieldSortOrderList sortOrder)
        {
            return DataSource.GetDarwinCore(userContext,
                                                            searchCriteria,
                                                            coordinateSystem,
                                                            sortOrder);
        }

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
        public virtual DarwinCoreList GetDarwinCore(IUserContext userContext,
                                                    List<Int64> speciesObservationIds,
                                                    ICoordinateSystem coordinateSystem)
        {
            return DataSource.GetDarwinCore(userContext,
                                                 speciesObservationIds,
                                                 coordinateSystem);
        }

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
        public virtual IDarwinCoreChange GetDarwinCoreChange(IUserContext userContext,
                                                             DateTime? changedFrom,
                                                             DateTime? changedTo,
                                                             Int64? changeId,
                                                             Int64 maxReturnedChanges,
                                                             ISpeciesObservationSearchCriteria searchCriteria,
                                                             ICoordinateSystem coordinateSystem)
        {
            return DataSource.GetDarwinCoreChange(userContext,
                                                  changedFrom,
                                                  changedTo,
                                                  changeId,
                                                  maxReturnedChanges,
                                                  searchCriteria,
                                                  coordinateSystem);
        }

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
        public virtual Boolean GetProtectedSpeciesObservationIndication(IUserContext userContext,
                                                                        ISpeciesObservationSearchCriteria searchCriteria,
                                                                        ICoordinateSystem coordinateSystem)
        {
            return DataSource.GetProtectedSpeciesObservationIndication(userContext,
                                                                       searchCriteria,
                                                                       coordinateSystem);
        }

        /// <summary>
        /// Get all county regions
        /// GetProvinceRegions is for searching observations in SpeciesObservationService and AnalysisServce only.
        /// The regions does not contain polygons.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <returns>Information about province regions</returns>
        public virtual RegionList GetProvinceRegions(IUserContext userContext)
        {
            return DataSource.GetProvinceRegions(userContext);
        }

        /// <summary>
        /// Get all activities.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All activities.</returns>
        public virtual SpeciesActivityList GetSpeciesActivities(IUserContext userContext)
        {
            return DataSource.GetSpeciesActivities(userContext);
        }

        /// <summary>
        /// Get all activity categories.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All activity categories.</returns>
        public virtual SpeciesActivityCategoryList GetSpeciesActivityCategories(IUserContext userContext)
        {
            return DataSource.GetSpeciesActivityCategories(userContext);
        }

        /// <summary>
        /// Get specific activity category.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="speciesActivityCategoryId">Id for the category to get.</param>
        /// <returns>One activity category.</returns>
        public virtual ISpeciesActivityCategory GetSpeciesActivityCategory(IUserContext userContext,
                                                                           Int32 speciesActivityCategoryId)
        {
            return DataSource.GetSpeciesActivityCategories(userContext).Get(speciesActivityCategoryId);
        }

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
        public virtual ISpeciesObservationChange GetSpeciesObservationChange(IUserContext userContext,
                                                                             DateTime? changedFrom,
                                                                             DateTime? changedTo,
                                                                             Int64? changeId,
                                                                             Int64 maxReturnedChanges,
                                                                             ISpeciesObservationSearchCriteria searchCriteria,
                                                                             ICoordinateSystem coordinateSystem,
                                                                             ISpeciesObservationSpecification speciesObservationSpecification)
        {
            return DataSource.GetSpeciesObservationChange(userContext,
                                                          changedFrom,
                                                          changedTo,
                                                          changeId,
                                                          maxReturnedChanges,
                                                          searchCriteria,
                                                          coordinateSystem,
                                                          speciesObservationSpecification);
        }

        /// <summary>
        /// Get all species observation data providers.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All species observation data providers.</returns>
        public virtual SpeciesObservationDataProviderList GetSpeciesObservationDataProviders(IUserContext userContext)
        {
            return DataSource.GetSpeciesObservationDataProviders(userContext);
        }

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
        /// <returns>Information about requested species observations.</returns>
        public virtual SpeciesObservationList GetSpeciesObservations(IUserContext userContext,
                                                                     ISpeciesObservationSearchCriteria searchCriteria,
                                                                     ICoordinateSystem coordinateSystem)
        {
            return GetSpeciesObservations(userContext,
                                          searchCriteria,
                                          coordinateSystem,
                                          (ISpeciesObservationSpecification)null,
                                          null);
        }

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
        /// <returns>Information about requested species observations.</returns>
        public SpeciesObservationList GetSpeciesObservations(IUserContext userContext,
                                                             ISpeciesObservationSearchCriteria searchCriteria,
                                                             ICoordinateSystem coordinateSystem,
                                                             ISpeciesObservationPageSpecification pageSpecification)
        {
            return GetSpeciesObservations(userContext,
                                          searchCriteria,
                                          coordinateSystem,
                                          pageSpecification,
                                          null);
        }

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
        public virtual SpeciesObservationList GetSpeciesObservations(IUserContext userContext,
                                                                     ISpeciesObservationSearchCriteria searchCriteria,
                                                                     ICoordinateSystem coordinateSystem,
                                                                     ISpeciesObservationPageSpecification pageSpecification,
                                                                     ISpeciesObservationSpecification speciesObservationSpecification)
        {
            return DataSource.GetSpeciesObservations(userContext,
                                                     searchCriteria,
                                                     coordinateSystem,
                                                     pageSpecification,
                                                     speciesObservationSpecification);
        }

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
        /// </param>
        /// <param name="sortOrder">
        /// Defines how species observations should be sorted.
        /// This parameter is optional. Random order is used
        /// if no sort order has been specified.
        /// This parameter is currently not used.
        /// </param>
        /// <returns>Information about requested species observations.</returns>
        public virtual SpeciesObservationList GetSpeciesObservations(IUserContext userContext,
                                                                     ISpeciesObservationSearchCriteria searchCriteria,
                                                                     ICoordinateSystem coordinateSystem,
                                                                     ISpeciesObservationSpecification speciesObservationSpecification,
                                                                     SpeciesObservationFieldSortOrderList sortOrder)
        {
            return DataSource.GetSpeciesObservations(userContext,
                                                     searchCriteria,
                                                     coordinateSystem,
                                                     speciesObservationSpecification,
                                                     sortOrder);
        }

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
        /// <returns>Species observations.</returns>
        public virtual SpeciesObservationList GetSpeciesObservations(IUserContext userContext,
                                                                     List<Int64> speciesObservationIds,
                                                                     ICoordinateSystem coordinateSystem)
        {
            return GetSpeciesObservations(userContext,
                                          speciesObservationIds,
                                          coordinateSystem,
                                          null);
        }

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
        public virtual SpeciesObservationList GetSpeciesObservations(IUserContext userContext,
                                                                     List<Int64> speciesObservationIds,
                                                                     ICoordinateSystem coordinateSystem,
                                                                     ISpeciesObservationSpecification speciesObservationSpecification)
        {
            return DataSource.GetSpeciesObservations(userContext,
                                                     speciesObservationIds,
                                                     coordinateSystem,
                                                     speciesObservationSpecification);
        }

        /// <summary>
        /// Get number of species observations that matches the search criteria.
        /// Scope is restricted to those observations that the user has access rights to.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>Number of species observations.</returns>
        public Int64 GetSpeciesObservationCount(IUserContext userContext,
                                                ISpeciesObservationSearchCriteria searchCriteria,
                                                ICoordinateSystem coordinateSystem)
        {
            return DataSource.GetSpeciesObservationCount(userContext, searchCriteria, coordinateSystem);
        }
    }
}

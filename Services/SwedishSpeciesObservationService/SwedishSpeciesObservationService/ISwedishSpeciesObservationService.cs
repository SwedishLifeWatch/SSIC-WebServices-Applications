using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArtDatabanken.WebService.Data;
using Microsoft.ApplicationInsights.Wcf;
using LogType = ArtDatabanken.WebService.Data.LogType;
using WebLogRow = ArtDatabanken.WebService.Data.WebLogRow;
using WebSpeciesObservationChange = ArtDatabanken.WebService.Data.WebSpeciesObservationChange;
using WebSpeciesObservationSearchCriteria = ArtDatabanken.WebService.Data.WebSpeciesObservationSearchCriteria;

namespace SwedishSpeciesObservationService
{
    /// <summary>
    /// Interface to the web service SwedishSpeciesObservationSOAPService.
    /// This web service is used to retrieve information about 
    /// species observations made in Sweden.
    /// Data sources for the moment:
    /// Species Gateway (www.artportalen.se).
    /// Artdatabanken internal observation database.
    /// </summary>
    [ServiceContract(Namespace = "urn:WebServices.ArtDatabanken.slu.se",
                     SessionMode = SessionMode.NotAllowed)]
    public interface ISwedishSpeciesObservationService
    {
        /// <summary>
        /// Clear data cache in web service.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        void ClearCache(WebClientInformation clientInformation);

        /// <summary>
        /// Delete trace information from the web service log.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        void DeleteTrace(WebClientInformation clientInformation);

        /// <summary>
        /// Get all bird nest activities.
        /// Definition of these species activities are not
        /// stable and will change in the future.
        /// A bird nest activity can be used in class
        /// WebSpeciesObservationSearchCriteria as BirdNestActivityLimit
        /// in order to filter search of species observations.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All bird nest activities.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebSpeciesActivity> GetBirdNestActivities(WebClientInformation clientInformation);

        /// <summary>
        /// Get all county Regions
        /// </summary>
        /// <param name="clientInformation"></param>
        /// <returns>All county regions</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebRegion> GetCountyRegions(WebClientInformation clientInformation);

        /// <summary>
        /// Get all province Regions
        /// </summary>
        /// <param name="clientInformation"></param>
        /// <returns>All province regions</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebRegion> GetProvinceRegions(WebClientInformation clientInformation);

        /// <summary>
        /// Get requested species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Species observations are returned in a format
        /// that is compatible with Darwin Core 1.5.
        /// Max 25000 observations with information
        /// can be retrieved in one call.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="speciesObservationIds">Id for species observations to get.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in returned species observations.
        /// </param>
        /// <returns>Species observations.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebDarwinCoreInformation GetDarwinCoreByIds(WebClientInformation clientInformation,
                                                    List<Int64> speciesObservationIds,
                                                    WebCoordinateSystem coordinateSystem);

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Species observations are returned in a format
        /// that is compatible with Darwin Core 1.5.
        /// Max 25000 observations with information
        /// can be retrieved in one call.
        /// Max 1000000 observation ids can be retrieved in one call.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
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
        /// <returns>Information about requested species observations.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebDarwinCoreInformation GetDarwinCoreBySearchCriteria(WebClientInformation clientInformation,
                                                               WebSpeciesObservationSearchCriteria searchCriteria,
                                                               WebCoordinateSystem coordinateSystem,
                                                               List<WebSpeciesObservationFieldSortOrder> sortOrder);

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
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
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
        [OperationContract]
        [OperationTelemetry]
        List<WebDarwinCore> GetDarwinCoreBySearchCriteriaPage(WebClientInformation clientInformation,
                                                              WebSpeciesObservationSearchCriteria searchCriteria,
                                                              WebCoordinateSystem coordinateSystem,
                                                              WebSpeciesObservationPageSpecification pageSpecification);

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
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="changedFrom">Start date and time for changes that should be returned.</param>
        /// <param name="isChangedFromSpecified">Indicates if parameter changedFrom should be used.</param>
        /// <param name="changedTo">
        /// End date and time for changes that should be
        /// returned. Parameter changedTo is optional and works
        /// with either parameter changedFrom or changeId.
        /// </param>
        /// <param name="isChangedToSpecified">Indicates if parameter changedTo should be used.</param>
        /// <param name="changeId">
        /// Start id for changes that should be returned.
        /// The species observation that is changed in the
        /// specified change id may be included in returned
        /// information.
        /// </param>
        /// <param name="isChangedIdSpecified">Indicates if parameter changeId should be used.</param>
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
        [OperationContract]
        [OperationTelemetry]
        WebDarwinCoreChange GetDarwinCoreChange(WebClientInformation clientInformation,
                                                DateTime changedFrom,
                                                Boolean isChangedFromSpecified,
                                                DateTime changedTo,
                                                Boolean isChangedToSpecified,
                                                Int64 changeId,
                                                Boolean isChangedIdSpecified,
                                                Int64 maxReturnedChanges,
                                                WebSpeciesObservationSearchCriteria searchCriteria,
                                                WebCoordinateSystem coordinateSystem);

        /// <summary>
        /// Get entries from the web service log
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="type">Get log entries of this type.</param>
        /// <param name="userName">Get log entries for this user. May be empty.</param>
        /// <param name="rowCount">Maximum number of log entries to get.</param>
        /// <returns> Requested web log entries.</returns>
        [OperationContract]
        List<WebLogRow> GetLog(WebClientInformation clientInformation,
                               LogType type,
                               String userName,
                               Int32 rowCount);

        /// <summary>
        /// Get an indication if specified geometries contains any
        /// protected species observations.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
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
        [OperationContract]
        [OperationTelemetry]
        Boolean GetProtectedSpeciesObservationIndication(WebClientInformation clientInformation,
                                                         WebSpeciesObservationSearchCriteria searchCriteria,
                                                         WebCoordinateSystem coordinateSystem);

#if NOT_YET_DEFINED
        /// <summary>
        /// Get all species activities.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All species activities.</returns>
        [OperationContract]
        List<WebSpeciesActivity> GetSpeciesActivities(WebClientInformation clientInformation);

        /// <summary>
        /// Get all species activity categories.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All species activity categories.</returns>
        [OperationContract]
        List<WebSpeciesActivityCategory> GetSpeciesActivityCategories(WebClientInformation clientInformation);
#endif

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
        /// This procedure is not yet implemented.
        /// </summary>
        /// <param name="clientInformation">
        /// Information about the client that makes this web
        /// service call.
        /// </param>
        /// <param name="changedFrom">
        /// Start date and time for changes that should be
        /// returned.
        /// </param>
        /// <param name="isChangedFromSpecified">
        /// Indicates if parameter changedFrom should be used.
        /// </param>
        /// <param name="changedTo">
        /// End date and time for changes that should be
        /// returned. Parameter changedTo is optional and works
        /// with either parameter changedFrom or changeId.
        /// </param>
        /// <param name="isChangedToSpecified">
        /// Indicates if parameter changedTo should be used.
        /// </param>
        /// <param name="changeId">
        /// Start id for changes that should be returned.
        /// The species observation that is changed in the
        /// specified change id may be included in returned
        /// information.
        /// </param>
        /// <param name="isChangedIdSpecified">
        /// Indicates if parameter changeId should be used.
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
        /// deleted species observations.
        /// </param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in returned species
        /// observations and in the optional species observation
        /// search criteria.
        /// </param>
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
        [OperationContract]
        [OperationTelemetry]
        WebSpeciesObservationChange GetSpeciesObservationChange(WebClientInformation clientInformation,
                                                                DateTime changedFrom,
                                                                Boolean isChangedFromSpecified,
                                                                DateTime changedTo,
                                                                Boolean isChangedToSpecified,
                                                                Int64 changeId,
                                                                Boolean isChangedIdSpecified,
                                                                Int64 maxReturnedChanges,
                                                                WebSpeciesObservationSearchCriteria searchCriteria,
                                                                WebCoordinateSystem coordinateSystem,
                                                                WebSpeciesObservationSpecification speciesObservationSpecification);

        /// <summary>
        /// Get number of species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in search criteria.</param>
        /// <returns>Number of species observations that matches the search criteria.</returns>
        [OperationContract]
        [OperationTelemetry]
        Int64 GetSpeciesObservationCountBySearchCriteria(WebClientInformation clientInformation,
                                                         WebSpeciesObservationSearchCriteria searchCriteria,
                                                         WebCoordinateSystem coordinateSystem);

        /// <summary>
        /// Get information about species observation data providers.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>Information about species observation data providers.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebSpeciesObservationDataProvider> GetSpeciesObservationDataProviders(WebClientInformation clientInformation);
        
        /// <summary>
        /// Get all species observation field descriptions.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All species observation field descriptions.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebSpeciesObservationFieldDescription> GetSpeciesObservationFieldDescriptions(WebClientInformation clientInformation);

        /// <summary>
        /// Get requested species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Species observations are returned in a format
        /// that is compatible with Darwin Core 1.5.
        /// Max 25000 observations with information
        /// can be retrieved in one call.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
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
        [OperationContract]
        [OperationTelemetry]
        WebSpeciesObservationInformation GetSpeciesObservationsByIds(WebClientInformation clientInformation,
                                                                     List<Int64> speciesObservationIds,
                                                                     WebCoordinateSystem coordinateSystem,
                                                                     WebSpeciesObservationSpecification speciesObservationSpecification);

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// Max 25000 observations with information
        /// can be retrieved in one call.
        /// Max 1000000 observation ids can be retrieved in one call.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
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
        /// </param>
        /// This parameter is currently not used.
        /// <returns>Information about requested species observations.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebSpeciesObservationInformation GetSpeciesObservationsBySearchCriteria(WebClientInformation clientInformation,
                                                                                WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                WebCoordinateSystem coordinateSystem,
                                                                                WebSpeciesObservationSpecification speciesObservationSpecification,
                                                                                List<WebSpeciesObservationFieldSortOrder> sortOrder);

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// This method provides paging functionality of the result.
        /// Max page size is 10000 species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in geometry search criteria
        /// and returned species observations.
        /// </param>
        /// <param name="pageSpecification">
        /// Specification of paging information when
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
        [OperationContract]
        [OperationTelemetry]
        List<WebSpeciesObservation> GetSpeciesObservationsBySearchCriteriaPage(WebClientInformation clientInformation,
                                                                               WebSpeciesObservationSearchCriteria searchCriteria,
                                                                               WebCoordinateSystem coordinateSystem,
                                                                               WebSpeciesObservationPageSpecification pageSpecification,
                                                                               WebSpeciesObservationSpecification speciesObservationSpecification);

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Status for this web service.</returns>       
        [OperationContract]
        List<WebResourceStatus> GetStatus(WebClientInformation clientInformation);

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">The password.</param>
        /// <param name="applicationIdentifier">
        /// Identifier of the application that the user uses.
        /// </param>
        /// <param name="isActivationRequired">
        /// Flag that indicates that the user account must
        /// be activated before login can succeed.
        /// </param>
        /// <returns>
        /// Token and user roles for the specified application
        /// or null if the login failed.
        /// </returns>       
        [OperationContract]
        WebLoginResponse Login(String userName,
                               String password,
                               String applicationIdentifier,
                               Boolean isActivationRequired);

        /// <summary>
        /// Logout user. Release resources.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        [OperationContract]
        void Logout(WebClientInformation clientInformation);

        /// <summary>
        /// Check if the web service is up and running.
        /// </summary>
        /// <returns>
        /// True = OK.
        /// False = Some kind of problem.
        /// </returns>       
        [OperationContract]
        Boolean Ping();

        /// <summary>
        /// Start trace usage of web service for specified user.
        /// If no user is specified then all usage of web service
        /// is traced.
        /// Note: Tracing has negative impact on web service performance.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="userName">User name.</param>
        [OperationContract]
        void StartTrace(WebClientInformation clientInformation,
                        String userName);

        /// <summary>
        /// Stop tracing usage of web service.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        void StopTrace(WebClientInformation clientInformation);
    }
}

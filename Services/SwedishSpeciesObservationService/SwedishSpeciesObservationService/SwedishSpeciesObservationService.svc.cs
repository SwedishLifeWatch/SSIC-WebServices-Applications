using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Log;
using ArtDatabanken.WebService;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using ArtDatabanken.WebService.SwedishSpeciesObservationService.Data;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Wcf;
using ApplicationManager = ArtDatabanken.WebService.Data.ApplicationManager;
using DatabaseManager = ArtDatabanken.WebService.SwedishSpeciesObservationService.Data.DatabaseManager;
using MetadataManager = ArtDatabanken.WebService.Data.MetadataManager;
using RegionManager = ArtDatabanken.WebService.Data.RegionManager;
using SpeciesFactManager = ArtDatabanken.WebService.Data.SpeciesFactManager;
using SpeciesObservationManager =
    ArtDatabanken.WebService.SwedishSpeciesObservationService.Data.SpeciesObservationManager;
using UserManager = ArtDatabanken.WebService.Data.UserManager;

namespace SwedishSpeciesObservationService
{
    /// <summary>
    /// Implementation of the web service SwedishSpeciesObservationSOAPService.
    /// This web service is used to retrieve information about 
    /// species observations made in Sweden.
    /// Data sources for the moment:
    /// Species Gateway (www.artportalen.se).
    /// Artdatabanken internal observation database.
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SwedishSpeciesObservationService : WebServiceBase, ISwedishSpeciesObservationService
    {
        /// <summary>
        /// Static constructor.
        /// </summary>
        static SwedishSpeciesObservationService()
        {
            WebServiceData.ApplicationManager = new ApplicationManager();
            WebServiceData.AuthorizationManager = new AuthorizationManager();
            WebServiceData.CoordinateConversionManager = new CoordinateConversionManager();
            WebServiceData.DatabaseManager = new DatabaseManager();
            WebServiceData.LogManager = new LogManager();
            WebServiceData.MetadataManager = new MetadataManager();
            WebServiceData.RegionManager = new RegionManager();
            WebServiceData.SpeciesActivityManager = new SpeciesActivityManagerLocalWebService();
            WebServiceData.SpeciesFactManager = new SpeciesFactManager();
            WebServiceData.SpeciesObservationManager = new SpeciesObservationManagerAdapter();
            WebServiceData.UserManager = new UserManager();
            WebServiceData.TaxonManager = new ArtDatabanken.WebService.Data.TaxonManager();
            WebServiceData.WebServiceManager = new WebServiceManager();
        }

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
        public List<WebSpeciesActivity> GetBirdNestActivities(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return
                        ArtDatabanken.WebService.SwedishSpeciesObservationService.Data.SpeciesActivityManager
                            .GetBirdNestActivities(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all County regions
        /// </summary>
        /// <param name="clientInformation"></param>
        /// <returns>All county regions</returns>
        public List<WebRegion> GetCountyRegions(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return SpeciesObservationManager.GetCountyRegions(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all province regions
        /// </summary>
        /// <param name="clientInformation"></param>
        /// <returns>All province regions</returns>
        public List<WebRegion> GetProvinceRegions(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return SpeciesObservationManager.GetProvinceRegions(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
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
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="speciesObservationIds">Id for species observations to get.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in returned species observations.
        /// </param>
        /// <returns>Species observations.</returns>
        public WebDarwinCoreInformation GetDarwinCoreByIds(
            WebClientInformation clientInformation,
            List<Int64> speciesObservationIds,
            WebCoordinateSystem coordinateSystem)
        {
            WebDarwinCoreInformation speciesObservationInformation;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    if (SpeciesObservationConfiguration.IsElasticsearchUsed)
                    {
                        speciesObservationInformation =
                            SpeciesObservationManager.GetDarwinCoreByIdsElasticsearch(
                                context,
                                speciesObservationIds,
                                coordinateSystem);
                    }
                    else
                    {
                        speciesObservationInformation = SpeciesObservationManager.GetDarwinCoreByIds(
                            context,
                            speciesObservationIds,
                            coordinateSystem);
                    }

                    LogSpeciesObservationCount(speciesObservationInformation);
                    return speciesObservationInformation;
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

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
        public WebDarwinCoreInformation GetDarwinCoreBySearchCriteria(
            WebClientInformation clientInformation,
            WebSpeciesObservationSearchCriteria searchCriteria,
            WebCoordinateSystem coordinateSystem,
            List<WebSpeciesObservationFieldSortOrder> sortOrder)
        {
            WebDarwinCoreInformation speciesObservationInformation;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    if (SpeciesObservationConfiguration.IsElasticsearchUsed)
                    {
                        speciesObservationInformation =
                            SpeciesObservationManager.GetDarwinCoreBySearchCriteriaElasticsearch(
                                context,
                                searchCriteria,
                                coordinateSystem,
                                sortOrder);
                    }
                    else
                    {
                        speciesObservationInformation = SpeciesObservationManager.GetDarwinCoreBySearchCriteria(
                            context,
                            searchCriteria,
                            coordinateSystem,
                            sortOrder);
                    }

                    LogSpeciesObservationCount(speciesObservationInformation);
                    return speciesObservationInformation;
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
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
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
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
        public List<WebDarwinCore> GetDarwinCoreBySearchCriteriaPage(
            WebClientInformation clientInformation,
            WebSpeciesObservationSearchCriteria searchCriteria,
            WebCoordinateSystem coordinateSystem,
            WebSpeciesObservationPageSpecification pageSpecification)
        {
            List<WebDarwinCore> speciesObservations;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    if (SpeciesObservationConfiguration.IsElasticsearchUsed)
                    {
                        speciesObservations =
                            SpeciesObservationManager.GetDarwinCoreBySearchCriteriaPageElasticsearch(
                                context,
                                searchCriteria,
                                coordinateSystem,
                                pageSpecification);
                    }
                    else
                    {
                        speciesObservations = SpeciesObservationManager.GetDarwinCoreBySearchCriteriaPage(
                            context,
                            searchCriteria,
                            coordinateSystem,
                            pageSpecification);
                    }

                    LogSpeciesObservationCount(speciesObservations);
                    return speciesObservations;
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
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
        public WebDarwinCoreChange GetDarwinCoreChange(
            WebClientInformation clientInformation,
            DateTime changedFrom,
            Boolean isChangedFromSpecified,
            DateTime changedTo,
            Boolean isChangedToSpecified,
            Int64 changeId,
            Boolean isChangedIdSpecified,
            Int64 maxReturnedChanges,
            WebSpeciesObservationSearchCriteria searchCriteria,
            WebCoordinateSystem coordinateSystem)
        {
            WebDarwinCoreChange speciesObservationChange;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    speciesObservationChange = SpeciesObservationManager.GetDarwinCoreChange(
                        context,
                        changedFrom,
                        isChangedFromSpecified,
                        changedTo,
                        isChangedToSpecified,
                        changeId,
                        isChangedIdSpecified,
                        maxReturnedChanges,
                        searchCriteria,
                        coordinateSystem);
                    LogSpeciesObservationCount(speciesObservationChange);
                    return speciesObservationChange;
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

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
        public Boolean GetProtectedSpeciesObservationIndication(
            WebClientInformation clientInformation,
            WebSpeciesObservationSearchCriteria searchCriteria,
            WebCoordinateSystem coordinateSystem)
        {
            Boolean protectedSpeciesObservationIndication;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    if (SpeciesObservationConfiguration.IsElasticsearchUsed)
                    {
                        protectedSpeciesObservationIndication =
                            SpeciesObservationManager.GetProtectedSpeciesObservationIndicationElasticsearch(
                                context,
                                searchCriteria,
                                coordinateSystem);
                    }
                    else
                    {
                        protectedSpeciesObservationIndication =
                            SpeciesObservationManager.GetProtectedSpeciesObservationIndication(
                                context,
                                searchCriteria,
                                coordinateSystem);
                    }

                    LogProtectedSpeciesObservationIndicationResult(protectedSpeciesObservationIndication);
                    return protectedSpeciesObservationIndication;
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

#if NOT_YET_DEFINED
/// <summary>
/// Get all species activities.
/// </summary>
/// <param name="clientInformation">Client information.</param>
/// <returns>All species activity categories.</returns>
        public List<WebSpeciesActivity> GetSpeciesActivities(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.SwedishSpeciesObservationService.Data.SpeciesActivityManager.GetSpeciesActivities(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all species activity categories.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All species activity categories.</returns>
        public List<WebSpeciesActivityCategory> GetSpeciesActivityCategories(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.SwedishSpeciesObservationService.Data.SpeciesActivityManager.GetSpeciesActivityCategories(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }
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
        /// <param name="isChangedIdSpecified">Indicates if parameter changeId should be used. </param>
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
        public WebSpeciesObservationChange GetSpeciesObservationChange(
            WebClientInformation clientInformation,
            DateTime changedFrom,
            Boolean isChangedFromSpecified,
            DateTime changedTo,
            Boolean isChangedToSpecified,
            Int64 changeId,
            Boolean isChangedIdSpecified,
            Int64 maxReturnedChanges,
            WebSpeciesObservationSearchCriteria searchCriteria,
            WebCoordinateSystem coordinateSystem,
            WebSpeciesObservationSpecification speciesObservationSpecification)
        {
            WebSpeciesObservationChange speciesObservationChange;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    speciesObservationChange = SpeciesObservationManager.GetSpeciesObservationChange(
                        context,
                        changedFrom,
                        isChangedFromSpecified,
                        changedTo,
                        isChangedToSpecified,
                        changeId,
                        isChangedIdSpecified,
                        maxReturnedChanges,
                        searchCriteria,
                        coordinateSystem,
                        speciesObservationSpecification);
                    LogSpeciesObservationCount(speciesObservationChange);
                    return speciesObservationChange;
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets no of species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned species observations.</param>
        /// <returns>Information about changed species observations.</returns>
        public Int64 GetSpeciesObservationCountBySearchCriteria(
            WebClientInformation clientInformation,
            WebSpeciesObservationSearchCriteria searchCriteria,
            WebCoordinateSystem coordinateSystem)
        {
            Int64 speciesObservationCount;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    if (SpeciesObservationConfiguration.IsElasticsearchUsed)
                    {
                        speciesObservationCount =
                            SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteriaElasticsearch(
                                context,
                                searchCriteria,
                                coordinateSystem);
                    }
                    else
                    {
                        speciesObservationCount =
                            SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteria(
                                context,
                                searchCriteria,
                                coordinateSystem);
                    }

                    LogSpeciesObservationCount(speciesObservationCount);
                    return speciesObservationCount;
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about species observation data providers.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>Information about species observation data providers.</returns>
        public List<WebSpeciesObservationDataProvider> GetSpeciesObservationDataProviders(
            WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return SpeciesObservationManager.GetSpeciesObservationDataProviders(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all Species Observation Field Descriptions. 
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <returns>A list of all Species Observation Field Descriptions.</returns>
        public List<WebSpeciesObservationFieldDescription> GetSpeciesObservationFieldDescriptions(
            WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptions(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
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
        public WebSpeciesObservationInformation GetSpeciesObservationsByIds(
            WebClientInformation clientInformation,
            List<Int64> speciesObservationIds,
            WebCoordinateSystem coordinateSystem,
            WebSpeciesObservationSpecification speciesObservationSpecification)
        {
            WebSpeciesObservationInformation speciesObservationInformation;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    if (SpeciesObservationConfiguration.IsElasticsearchUsed)
                    {
                        speciesObservationInformation =
                            SpeciesObservationManager.GetSpeciesObservationsByIdsElasticsearch(
                                context,
                                speciesObservationIds,
                                coordinateSystem,
                                speciesObservationSpecification);
                    }
                    else
                    {
                        speciesObservationInformation = SpeciesObservationManager.GetSpeciesObservationsByIds(
                            context,
                            speciesObservationIds,
                            coordinateSystem,
                            speciesObservationSpecification);
                    }

                    LogSpeciesObservationCount(speciesObservationInformation);
                    return speciesObservationInformation;
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

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
        /// This parameter is currently not used.
        /// </param>
        /// <param name="sortOrder">
        /// Defines how species observations should be sorted.
        /// This parameter is optional. Random order is used
        /// if no sort order has been specified.
        /// </param>
        /// <returns>Information about requested species observations.</returns>
        public WebSpeciesObservationInformation GetSpeciesObservationsBySearchCriteria(
            WebClientInformation clientInformation,
            WebSpeciesObservationSearchCriteria searchCriteria,
            WebCoordinateSystem coordinateSystem,
            WebSpeciesObservationSpecification speciesObservationSpecification,
            List<WebSpeciesObservationFieldSortOrder> sortOrder)
        {
            WebSpeciesObservationInformation speciesObservationInformation;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    if (SpeciesObservationConfiguration.IsElasticsearchUsed)
                    {
                        speciesObservationInformation =
                            SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaElasticsearch(
                                context,
                                searchCriteria,
                                coordinateSystem,
                                speciesObservationSpecification,
                                sortOrder);
                    }
                    else
                    {
                        speciesObservationInformation =
                            SpeciesObservationManager.GetSpeciesObservationsBySearchCriteria(
                                context,
                                searchCriteria,
                                coordinateSystem,
                                speciesObservationSpecification,
                                sortOrder);
                    }

                    LogSpeciesObservationCount(speciesObservationInformation);
                    return speciesObservationInformation;
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

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
        public List<WebSpeciesObservation> GetSpeciesObservationsBySearchCriteriaPage(
            WebClientInformation clientInformation,
            WebSpeciesObservationSearchCriteria searchCriteria,
            WebCoordinateSystem coordinateSystem,
            WebSpeciesObservationPageSpecification pageSpecification,
            WebSpeciesObservationSpecification speciesObservationSpecification)
        {
            List<WebSpeciesObservation> speciesObservations;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    if (SpeciesObservationConfiguration.IsElasticsearchUsed)
                    {
                        speciesObservations =
                            SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaPageElasticsearch(
                                context,
                                searchCriteria,
                                coordinateSystem,
                                pageSpecification,
                                speciesObservationSpecification);
                    }
                    else
                    {
                        speciesObservations =
                            SpeciesObservationManager.GetSpeciesObservationsBySearchCriteriaPage(
                                context,
                                searchCriteria,
                                coordinateSystem,
                                pageSpecification,
                                speciesObservationSpecification);
                    }

                    LogSpeciesObservationCount(speciesObservations);
                    return speciesObservations;
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get web service context.
        /// This method is used to add Application Insights telemetry data from the request.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Web service context.</returns>
        private WebServiceContext GetWebServiceContext(WebClientInformation clientInformation)
        {
            RequestTelemetry telemetry;
            WebServiceContext context;
            WebUser user;

            try
            {
                context = new WebServiceContext(clientInformation);
                try
                {
                    if (context.IsNotNull() && (Configuration.InstallationType == InstallationType.Production))
                    {
                        telemetry = OperationContext.Current.GetRequestTelemetry();
                        if (telemetry.IsNotNull())
                        {
                            if (context.ClientToken.IsNotNull())
                            {
                                telemetry.Properties[TelemetryProperty.ApplicationIdentifier.ToString()] = context.ClientToken.ApplicationIdentifier;
                                telemetry.Properties[TelemetryProperty.ClientIpAddress.ToString()] = context.ClientToken.ClientIpAddress;
                                telemetry.Properties[TelemetryProperty.LoginDateTime.ToString()] = context.ClientToken.CreatedDate.WebToString();
                            }

                            user = context.GetUser();
                            if (user.IsNotNull())
                            {
                                telemetry.Properties[TelemetryProperty.UserId.ToString()] = user.Id.WebToString();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // Do nothing. We don't want calls to fail because of logging problems.
                }
            }
            catch (ApplicationException)
            {
                LogClientToken(clientInformation);
                throw;
            }
            catch (ArgumentException)
            {
                LogClientToken(clientInformation);
                throw;
            }

            return context;
        }

        /// <summary>
        /// Add information about client to Application Insights.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        private void LogClientToken(WebClientInformation clientInformation)
        {
            RequestTelemetry telemetry;
            WebClientToken clientToken;
            WebUser user;

            try
            {
                if (Configuration.InstallationType == InstallationType.Production)
                {
                    clientToken = new WebClientToken(clientInformation.Token, WebServiceData.WebServiceManager.Key);
                    if (clientToken.IsNotNull())
                    {
                        telemetry = OperationContext.Current.GetRequestTelemetry();
                        if (telemetry.IsNotNull())
                        {
                            telemetry.Properties[TelemetryProperty.ApplicationIdentifier.ToString()] = clientToken.ApplicationIdentifier;
                            telemetry.Properties[TelemetryProperty.ClientIpAddress.ToString()] = clientToken.ClientIpAddress;
                            telemetry.Properties[TelemetryProperty.LoginDateTime.ToString()] = clientToken.CreatedDate.WebToString();

                            user = WebServiceData.UserManager.GetUser(clientToken.UserName);
                            if (user.IsNotNull())
                            {
                                telemetry.Properties[TelemetryProperty.UserId.ToString()] = user.Id.WebToString();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Do nothing. We don't want calls to fail because of logging problems.
            }
        }

        /// <summary>
        /// Add information about protected species observation indication result to Application Insights. 
        /// </summary>
        /// <param name="protectedSpeciesObservationIndication">Protected species observation indication.</param>
        private void LogProtectedSpeciesObservationIndicationResult(Boolean protectedSpeciesObservationIndication)
        {
            RequestTelemetry telemetry;

            try
            {
                if (Configuration.InstallationType == InstallationType.Production)
                {
                    telemetry = OperationContext.Current.GetRequestTelemetry();
                    if (telemetry.IsNotNull())
                    {
                        telemetry.Properties[TelemetryProperty.ProtectedSpeciesObservationIndication.ToString()] = protectedSpeciesObservationIndication.WebToString();
                    }
                }
            }
            catch (Exception)
            {
                // Do nothing. We don't want calls to fail because of logging problems.
            }
        }

        /// <summary>
        /// Add information about retrieved species observations to Application Insights. 
        /// </summary>
        /// <param name="speciesObservationCount">Species observation count.</param>
        private void LogSpeciesObservationCount(Int64 speciesObservationCount)
        {
            RequestTelemetry telemetry;

            try
            {
                if (Configuration.InstallationType == InstallationType.Production)
                {
                    telemetry = OperationContext.Current.GetRequestTelemetry();
                    if (telemetry.IsNotNull())
                    {
                        telemetry.Metrics[TelemetryMetric.SpeciesObservationCount.ToString()] = speciesObservationCount;
                    }
                }
            }
            catch (Exception)
            {
                // Do nothing. We don't want calls to fail because of logging problems.
            }
        }

        /// <summary>
        /// Add information about retrieved species observations to Application Insights. 
        /// </summary>
        /// <param name="speciesObservations">Species observations.</param>
        private void LogSpeciesObservationCount(List<WebDarwinCore> speciesObservations)
        {
            if (speciesObservations.IsEmpty())
            {
                LogSpeciesObservationCount(0);
            }
            else
            {
                LogSpeciesObservationCount(speciesObservations.Count);
            }
        }

        /// <summary>
        /// Add information about retrieved species observations to Application Insights. 
        /// </summary>
        /// <param name="speciesObservationChange">Species observation change.</param>
        private void LogSpeciesObservationCount(WebDarwinCoreChange speciesObservationChange)
        {
            Int64 speciesObservationCount;

            speciesObservationCount = 0;
            if (speciesObservationChange.IsNotNull())
            {
                if (speciesObservationChange.CreatedSpeciesObservations.IsNotEmpty())
                {
                    speciesObservationCount += speciesObservationChange.CreatedSpeciesObservations.Count;
                }

                if (speciesObservationChange.UpdatedSpeciesObservations.IsNotEmpty())
                {
                    speciesObservationCount += speciesObservationChange.UpdatedSpeciesObservations.Count;
                }
            }

            LogSpeciesObservationCount(speciesObservationCount);
        }

        /// <summary>
        /// Add information about retrieved species observations to Application Insights. 
        /// </summary>
        /// <param name="speciesObservationInformation">Species observations.</param>
        private void LogSpeciesObservationCount(WebDarwinCoreInformation speciesObservationInformation)
        {
            if (speciesObservationInformation.IsNull())
            {
                LogSpeciesObservationCount(0);
            }
            else
            {
                LogSpeciesObservationCount(speciesObservationInformation.SpeciesObservations);
            }
        }

        /// <summary>
        /// Add information about retrieved species observations to Application Insights. 
        /// </summary>
        /// <param name="speciesObservations">Species observations.</param>
        private void LogSpeciesObservationCount(List<WebSpeciesObservation> speciesObservations)
        {
            if (speciesObservations.IsEmpty())
            {
                LogSpeciesObservationCount(0);
            }
            else
            {
                LogSpeciesObservationCount(speciesObservations.Count);
            }
        }

        /// <summary>
        /// Add information about retrieved species observations to Application Insights. 
        /// </summary>
        /// <param name="speciesObservationChange">Species observation change.</param>
        private void LogSpeciesObservationCount(WebSpeciesObservationChange speciesObservationChange)
        {
            Int64 speciesObservationCount;

            speciesObservationCount = 0;
            if (speciesObservationChange.IsNotNull())
            {
                if (speciesObservationChange.CreatedSpeciesObservations.IsNotEmpty())
                {
                    speciesObservationCount += speciesObservationChange.CreatedSpeciesObservations.Count;
                }

                if (speciesObservationChange.UpdatedSpeciesObservations.IsNotEmpty())
                {
                    speciesObservationCount += speciesObservationChange.UpdatedSpeciesObservations.Count;
                }
            }

            LogSpeciesObservationCount(speciesObservationCount);
        }

        /// <summary>
        /// Add information about retrieved species observations to Application Insights. 
        /// </summary>
        /// <param name="speciesObservationInformation">Species observations.</param>
        private void LogSpeciesObservationCount(WebSpeciesObservationInformation speciesObservationInformation)
        {
            if (speciesObservationInformation.IsNull())
            {
                LogSpeciesObservationCount(0);
            }
            else
            {
                LogSpeciesObservationCount(speciesObservationInformation.SpeciesObservations);
            }
        }
    }
}

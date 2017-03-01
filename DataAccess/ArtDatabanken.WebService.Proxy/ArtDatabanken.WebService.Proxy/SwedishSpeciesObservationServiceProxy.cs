using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy.SwedishSpeciesObservationService;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Class that manages species observation related requests.
    /// </summary>
    public class SwedishSpeciesObservationServiceProxy : WebServiceProxyBase, IWebService
    {
        /// <summary>
        /// Create a SwedishSpeciesObservationServiceProxy instance.
        /// </summary>
        public SwedishSpeciesObservationServiceProxy()
            : this(null)
        {
        }

        /// <summary>
        /// Create a UserServiceProxy instance.
        /// </summary>
        /// <param name="webServiceAddress">
        /// Address to web service without internet protocol (http or https)
        /// and binding protocol (SOAP 1.1, SOAP 1.2 or binary).
        /// For example SwedishSpeciesObservation.ArtDatabankenSOA.se/SwedishSpeciesObservationService.svc.
        /// </param>
        public SwedishSpeciesObservationServiceProxy(String webServiceAddress)
        {
            WebServiceAddress = webServiceAddress;
            switch (Configuration.InstallationType)
            {
                case InstallationType.ArtportalenTest:
                case InstallationType.ServerTest:
                    WebServiceComputer = WebServiceComputer.Moneses;
                    break;

                case InstallationType.LocalTest:
                    WebServiceComputer = WebServiceComputer.LocalTest;
                    break;

                case InstallationType.Production:
                    WebServiceComputer = WebServiceComputer.ArtDatabankenSoa;
                    break;

                case InstallationType.SpeciesFactTest:
                    WebServiceComputer = WebServiceComputer.SpeciesFactTest;
                    break;

                case InstallationType.SystemTest:
                    WebServiceComputer = WebServiceComputer.SystemTest;
                    break;

                case InstallationType.TwoBlueberriesTest:
                    WebServiceComputer = WebServiceComputer.TwoBlueberriesTest;
                    break;

                default:
                    throw new ApplicationException("Not handled installation type " + Configuration.InstallationType);
            }
        }

        /// <summary>
        /// Address to web service without internet protocol (http or https)
        /// and binding protocol (SOAP 1.1, SOAP 1.2 or binary).
        /// For example SwedishSpeciesObservation.ArtDatabankenSOA.se/SwedishSpeciesObservationService.svc.
        /// </summary>
        public String WebServiceAddress { get; set; }

        /// <summary>
        /// Clear data cache in web service.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        public void ClearCache(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.ClearCache(clientInformation);
            }
        }

        /// <summary>
        /// Close a web service client.
        /// </summary>
        /// <param name="client">Web service client.</param>
        protected override void CloseClient(Object client)
        {
            try
            {
                ((ClientBase<ISwedishSpeciesObservationService>)client).Close();
            }
            catch (Exception)
            {
                try
                {
                    ((ClientBase<ISwedishSpeciesObservationService>)client).Abort();
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch (Exception)
                {
                    // We are only interested in releasing resources.
                }
            }
        }

        /// <summary>
        /// Create a web service client.
        /// </summary>
        /// <returns>A web service client.</returns>
        protected override Object CreateClient()
        {
            SwedishSpeciesObservationServiceClient client;

            client = new SwedishSpeciesObservationServiceClient(GetBinding(),
                                                                GetEndpointAddress());

            // Increase data size for all methods that  
            // sends or receives a lot of data.
            IncreaseDataSize("GetDarwinCoreByIds", client.Endpoint);
            IncreaseDataSize("GetDarwinCoreBySearchCriteria", client.Endpoint);
            IncreaseDataSize("GetDarwinCoreBySearchCriteriaPage", client.Endpoint);
            IncreaseDataSize("GetDarwinCoreChange", client.Endpoint);
            IncreaseDataSize("GetLog", client.Endpoint);
            IncreaseDataSize("GetSpeciesObservationChange", client.Endpoint);
            IncreaseDataSize("GetSpeciesObservationsByIds", client.Endpoint);
            IncreaseDataSize("GetSpeciesObservationsBySearchCriteria", client.Endpoint);
            IncreaseDataSize("GetSpeciesObservationsBySearchCriteriaPage", client.Endpoint);

            return client;
        }

        /// <summary>
        /// Delete trace information from the web service log.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        public void DeleteTrace(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.DeleteTrace(clientInformation);
            }
        }

        /// <summary>
        /// Get all bird nest activities.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Information about bird nest activities.</returns>
        public List<WebSpeciesActivity> GetBirdNestActivities(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetBirdNestActivities(clientInformation);
            }
        }

        /// <summary>
        /// Get all county Regions
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>County Regions</returns>
        public List<WebRegion> GetCountyRegions(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetCountyRegions(clientInformation);
            }
        }

        /// <summary>
        /// Get requested species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Max 25000 observations can be retrieved in one call.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="speciesObservationIds">Id for species observations to get.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in returned species observations.
        /// </param>
        /// <returns>Species observations.</returns>
        public WebDarwinCoreInformation GetDarwinCoreByIds(WebClientInformation clientInformation,
                                                           List<Int64> speciesObservationIds,
                                                           WebCoordinateSystem coordinateSystem)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetDarwinCoreByIds(clientInformation, speciesObservationIds, coordinateSystem);
            }
        }

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in geometry search criterias
        /// and returned species observations.
        /// </param>
        /// <param name="webSpeciesObservationFieldSortOrders"></param>
        /// <returns>Information about requested species observations.</returns>
        public WebDarwinCoreInformation GetDarwinCoreBySearchCriteria(WebClientInformation clientInformation,
                                                                      WebSpeciesObservationSearchCriteria searchCriteria,
                                                                      WebCoordinateSystem coordinateSystem,
                                                                      List<WebSpeciesObservationFieldSortOrder> webSpeciesObservationFieldSortOrders )
        {
            using (ClientProxy client = new ClientProxy(this, 20))
            {
                return client.Client.GetDarwinCoreBySearchCriteria(clientInformation, searchCriteria, coordinateSystem, webSpeciesObservationFieldSortOrders);
            }
        }

        /// <summary>
        /// Get information about species observations paged
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in geometry search criterias
        /// and returned species observations.
        /// </param>
        /// <param name="pageSpecification"></param>
        /// <returns>Information about requested species observations.</returns>
        public List<WebDarwinCore> GetDarwinCoreBySearchCriteriaPage(WebClientInformation clientInformation,
                                                                      WebSpeciesObservationSearchCriteria searchCriteria,
                                                                      WebCoordinateSystem coordinateSystem,
                                                                      WebSpeciesObservationPageSpecification pageSpecification)
        {
            using (ClientProxy client = new ClientProxy(this, 20))
            {
                return client.Client.GetDarwinCoreBySearchCriteriaPage(clientInformation, searchCriteria, coordinateSystem, pageSpecification);
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
        public WebDarwinCoreChange GetDarwinCoreChange(WebClientInformation clientInformation,
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
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetDarwinCoreChange(clientInformation, changedFrom, isChangedFromSpecified,
                                                         changedTo, isChangedToSpecified, changeId, isChangedIdSpecified,
                                                         maxReturnedChanges, searchCriteria, coordinateSystem);
            }
        }

        /// <summary>
        /// Get entries from the web service log.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="type">Get log entries of this type.</param>
        /// <param name="userName">Get log entries for this user. May be empty.</param>
        /// <param name="rowCount">Maximum number of log entries to get.</param>
        /// <returns> Requested web log entries.</returns>
        public List<WebLogRow> GetLog(WebClientInformation clientInformation,
                                      LogType type,
                                      String userName,
                                      Int32 rowCount)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetLog(clientInformation, type, userName, rowCount);
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
        /// Search criterias that may be used: Accuracy,
        /// BirdNestActivityLimit, BoundingBox, IsAccuracyConsidered,
        /// IsDisturbanceSensitivityConsidered, MinProtectionLevel,
        /// ObservationDateTime, Polygons and RegionGuids.
        /// </param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criterias.</param>
        /// <returns>
        /// True, if specified geometries contains any
        /// protected species observations.
        /// </returns>
        public Boolean GetProtectedSpeciesObservationIndication(WebClientInformation clientInformation,
                                                                WebSpeciesObservationSearchCriteria searchCriteria,
                                                                WebCoordinateSystem coordinateSystem)
        {
            using (ClientProxy client = new ClientProxy(this, 20))
            {
                return client.Client.GetProtectedSpeciesObservationIndication(clientInformation, searchCriteria, coordinateSystem);
            }
        }

        /// <summary>
        /// Get all province Regions
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Province Regions</returns>
        public List<WebRegion> GetProvinceRegions(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetProvinceRegions(clientInformation);
            }
        }

        /// <summary>
        /// Get all species activities.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All species activities.</returns>
        public List<WebSpeciesActivity> GetSpeciesActivities(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
//                return client.Client.GetSpeciesActivities(clientInformation);
                return null;
            }
        }

        /// <summary>
        /// Get all species activities.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All species activities.</returns>
        public List<WebSpeciesActivityCategory> GetSpeciesActivityCategories(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
//                return client.Client.GetSpeciesActivityCategories(clientInformation);
                return null;
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
        public WebSpeciesObservationChange GetSpeciesObservationChange(WebClientInformation clientInformation,
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
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetSpeciesObservationChange(clientInformation,
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
            }
        }

        /// <summary>
        /// Get information about the data providers for the Swedish Species Information data base.
        /// </summary>
        /// <param name="clientInformation">Information about the client.</param>
        /// <returns> Requested data sources .</returns>
        public List<WebSpeciesObservationDataProvider> GetSpeciesObservationDataProviders(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetSpeciesObservationDataProviders(clientInformation);
            }
        }

        /// <summary>
        /// Get all Species Observation Field Descriptions.
        /// </summary>
        /// <param name="clientInformation">Information about the client.</param>
        /// <returns>A List with all Species Observation Field Descriptions.</returns>
        public List<WebSpeciesObservationFieldDescription> GetSpeciesObservationFieldDescriptions(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetSpeciesObservationFieldDescriptions(clientInformation);
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
        public WebSpeciesObservationInformation GetSpeciesObservationsByIds(WebClientInformation clientInformation,
                                                                            List<Int64> speciesObservationIds,
                                                                            WebCoordinateSystem coordinateSystem,
                                                                            WebSpeciesObservationSpecification speciesObservationSpecification)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetSpeciesObservationsByIds(clientInformation, speciesObservationIds, coordinateSystem, speciesObservationSpecification);
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
        /// </param>
        /// <param name="sortOrder">
        /// Defines how species observations should be sorted.
        /// This parameter is optional. Random order is used
        /// if no sort order has been specified.
        /// This parameter is currently not used.
        /// </param>
        /// <returns>Information about requested species observations.</returns>
        public WebSpeciesObservationInformation GetSpeciesObservationsBySearchCriteria(WebClientInformation clientInformation,
                                                                                       WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                       WebCoordinateSystem coordinateSystem,
                                                                                       WebSpeciesObservationSpecification speciesObservationSpecification,
                                                                                       List<WebSpeciesObservationFieldSortOrder> sortOrder)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetSpeciesObservationsBySearchCriteria(clientInformation,
                                                                            searchCriteria,
                                                                            coordinateSystem,
                                                                            speciesObservationSpecification,
                                                                            sortOrder);
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
        public List<WebSpeciesObservation> GetSpeciesObservationsBySearchCriteriaPage(WebClientInformation clientInformation,
                                                                                      WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                      WebCoordinateSystem coordinateSystem,
                                                                                      WebSpeciesObservationPageSpecification pageSpecification,
                                                                                      WebSpeciesObservationSpecification speciesObservationSpecification)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetSpeciesObservationsBySearchCriteriaPage(clientInformation,
                                                                                searchCriteria,
                                                                                coordinateSystem,
                                                                                pageSpecification,
                                                                                speciesObservationSpecification);
            }
        }

        /// <summary>
        /// Gets number of species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.
        /// </param>
        /// <returns>No of species observations that matches the search criteria.</returns>
        public Int64 GetSpeciesObservationCountBySearchCriteria(WebClientInformation clientInformation,
                                                                                   WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                   WebCoordinateSystem coordinateSystem)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetSpeciesObservationCountBySearchCriteria(clientInformation, searchCriteria, coordinateSystem);
            }
        }

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Status for this web service.</returns>       
        public List<WebResourceStatus> GetStatus(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetStatus(clientInformation);
            }
        }

        /// <summary>
        /// Get address of currently used web service.
        /// </summary>
        /// <returns>Address of currently used web service.</returns>
        protected override String GetWebServiceAddress()
        {
            if (WebServiceAddress.IsEmpty())
            {
                if (Configuration.InstallationType != InstallationType.LocalTest)
                {
                    WebServiceAddress = WebServiceProxy.UserService.GetSoaWebServiceAddress(ApplicationIdentifier.SwedishSpeciesObservationService);
                }
                if (WebServiceAddress.IsEmpty())
                {
                    switch (WebServiceComputer)
                    {
                        case WebServiceComputer.ArtDatabankenSoa:
                            WebServiceAddress = Settings.Default.SwedishSpeciesObservationServiceArtDatabankenSoaAddress;
                            break;
                        case WebServiceComputer.LocalTest:
                            WebServiceAddress = Settings.Default.SwedishSpeciesObservationServiceLocalAddress;
                            break;
                        case WebServiceComputer.Moneses:
                            WebServiceAddress = Settings.Default.SwedishSpeciesObservationServiceMonesesAddress;
                            break;
                        default:
                            throw new Exception("Not handled computer in web service " + GetWebServiceName() + " " +
                                                WebServiceComputer);
                    }
                }
            }

            return WebServiceAddress;
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="applicationIdentifier">
        /// Application identifier.
        /// User authorities for this application is included in
        /// the user context.
        /// </param>
        /// <param name="isActivationRequired">
        /// Flag that indicates if user must be activated
        /// for login to succed.
        /// </param>
        /// <returns>Web login response or null if login failed.</returns>
        public WebLoginResponse Login(String userName,
                                      String password,
                                      String applicationIdentifier,
                                      Boolean isActivationRequired)
        {
            WebServiceProxy.UserService.LoadSoaWebServiceAddresses(userName,
                                                                   password,
                                                                   applicationIdentifier,
                                                                   isActivationRequired);

            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.Login(userName, password, applicationIdentifier, isActivationRequired);
            }
        }

        /// <summary>
        /// Logout user from web service.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        public void Logout(WebClientInformation clientInformation)
        {
            try
            {
                using (ClientProxy client = new ClientProxy(this, 1))
                {
                    client.Client.Logout(clientInformation);
                }
            }
            catch
            {
                // No need to handle errors.
                // Logout is only used to relase
                // resources in the web service.
            }
        }

        /// <summary>
        /// Check if the web service is up and running.
        /// </summary>
        /// <returns>
        /// True = OK.
        /// False = Some kind of problem.
        /// </returns>       
        public Boolean Ping()
        {
            try
            {
                using (ClientProxy client = new ClientProxy(this, 0, 10))
                {
                    return client.Client.Ping();
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Start trace usage of web service for specified user.
        /// If no user is specified then all usage of web service
        /// is traced.
        /// Note: Tracing has negativ impact on web service performance.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="userName">User name.</param>
        public void StartTrace(WebClientInformation clientInformation,
                               String userName)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.StartTrace(clientInformation, userName);
            }
        }

        /// <summary>
        /// Stop tracing usage of web service.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        public void StopTrace(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.StopTrace(clientInformation);
            }
        }

        /// <summary>
        /// Private class that encapsulate handling
        /// of web service connections.
        /// </summary>
        private class ClientProxy : IDisposable
        {
            private readonly Int32 _operationTimeout;
            private readonly SwedishSpeciesObservationServiceProxy _webService;

            /// <summary>
            /// Constructor for the ClientProxy class.
            /// Allocates an instance of the real web service client.
            /// </summary>
            /// <param name="webService">Web service proxy.</param>
            /// <param name="operationTimeoutMinutes">Operation timeout to set in the client. Unit is minutes.</param>
            /// <param name="operationTimeoutSeconds">Operation timeout to set in the client. Unit is seconds.</param>
            public ClientProxy(SwedishSpeciesObservationServiceProxy webService,
                               Int32 operationTimeoutMinutes,
                               Int32 operationTimeoutSeconds = 0)
            {
                _operationTimeout = (operationTimeoutMinutes * 60) + operationTimeoutSeconds;
                _webService = webService;
                Client = (SwedishSpeciesObservationServiceClient)(_webService.PopClient(_operationTimeout));
                _webService.SetTimeout(Client.Endpoint.Binding, _operationTimeout);
            }

            /// <summary>
            /// Get the real web service client.
            /// </summary>
            public SwedishSpeciesObservationServiceClient Client
            { get; private set; }

            /// <summary>
            /// Implementation of the IDisposable interface.
            /// Recycle the client instance.
            /// </summary>
            public void Dispose()
            {
                if ((Client.State != CommunicationState.Opened) ||
                    (!_webService.PushClient(Client, _operationTimeout)))
                {
                    // Client is not in state open or
                    // was not added to the client pool.
                    // Release resources.
                    Client.Close();
                }
                Client = null;
            }
        }
    }
}

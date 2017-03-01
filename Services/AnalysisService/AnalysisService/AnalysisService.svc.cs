using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Log;
using ArtDatabanken.WebService;
using ArtDatabanken.WebService.AnalysisService.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Wcf;
using AnalysisManager = ArtDatabanken.WebService.AnalysisService.Data.AnalysisManager;
using ApplicationManager = ArtDatabanken.WebService.Data.ApplicationManager;
using DatabaseManager = ArtDatabanken.WebService.AnalysisService.Data.DatabaseManager;
using RegionManager = ArtDatabanken.WebService.Data.RegionManager;
using SpeciesFactManager = ArtDatabanken.WebService.Data.SpeciesFactManager;
using SpeciesObservationManager = ArtDatabanken.WebService.Data.SpeciesObservationManager;
using TaxonManager = ArtDatabanken.WebService.Data.TaxonManager;
using UserManager = ArtDatabanken.WebService.Data.UserManager;

namespace AnalysisService
{
    /// <summary>
    /// Implementation of the web service AnalysisService.
    /// This web service is used to retrieve information about 
    /// species observations made in Sweden.
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class AnalysisService : WebServiceBase, IAnalysisService
    {
        /// <summary>
        /// Static constructor.
        /// </summary>
        static AnalysisService()
        {
            WebServiceData.ApplicationManager = new ApplicationManager();
            WebServiceData.AuthorizationManager = new AuthorizationManager();
            WebServiceData.CoordinateConversionManager = new CoordinateConversionManager();
            WebServiceData.DatabaseManager = new DatabaseManager();
            WebServiceData.GeometryManager = new GeometryManager();
            WebServiceData.LogManager = new LogManager();
            WebServiceData.RegionManager = new RegionManager();
            WebServiceData.UserManager = new UserManager();
            WebServiceData.WebServiceManager = new WebServiceManager();
            WebServiceData.SpeciesActivityManager = new SpeciesActivityManager();
            WebServiceData.SpeciesFactManager = new SpeciesFactManager();
            WebServiceData.SpeciesObservationManager = new SpeciesObservationManager();
            WebServiceData.TaxonManager = new TaxonManager();
            ArtDatabanken.Data.ArtDatabankenService.UserManager.Login(WebServiceData.WebServiceManager.Name, WebServiceData.WebServiceManager.Password, ApplicationIdentifier.ArtDatabankenSOA.ToString(), false);
        }

        /// <summary>
        /// Gets the grid cell feature statistics combined with species observation counts.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="gridSpecification">The grid specification: bounding box, grid cell size, etc.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="featureStatisticsSpecification">Information about requested information from a web feature service.</param>
        /// <param name="featuresUrl">Address to data in a web feature service.</param>
        /// <param name="featureCollectionJson">Feature collecation as json.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned grid.</param>
        /// <returns>A list with combined result from GetGridSpeciesCounts() and GetGridCellFeatureStatistics().</returns>
        public List<WebGridCellCombinedStatistics> GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts(
            WebClientInformation clientInformation,
            WebGridSpecification gridSpecification,
            WebSpeciesObservationSearchCriteria searchCriteria,
            WebFeatureStatisticsSpecification featureStatisticsSpecification,
            String featuresUrl,
            String featureCollectionJson,
            WebCoordinateSystem coordinateSystem)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    if (SpeciesObservationConfiguration.IsElasticsearchUsed)
                    {
                        return AnalysisManager.GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCountsElasticsearch(context,
                            searchCriteria,
                            gridSpecification,
                            coordinateSystem,
                            featuresUrl,
                            featureCollectionJson);
                    }
                    else
                    {
                        return AnalysisManager.GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts(context,
                            searchCriteria,
                            gridSpecification,
                            coordinateSystem,
                            featuresUrl,
                            featureCollectionJson);
                    }
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the grid cell feature statistics combined with species observation counts.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="gridSpecification">The grid specification: bounding box, grid cell size, etc.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="featureStatisticsSpecification">Information about requested information from a web feature service.</param>
        /// <param name="featuresUrl">Address to data in a web feature service.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned grid.</param>
        /// <returns>A list with combined result from GetGridSpeciesCounts() and GetGridCellFeatureStatistics().</returns>
        public List<WebGridCellCombinedStatistics> GetGridFeatureStatisticsCombinedWithSpeciesObservationCounts(
            WebClientInformation clientInformation,
            WebGridSpecification gridSpecification,
            WebSpeciesObservationSearchCriteria searchCriteria,
            WebFeatureStatisticsSpecification featureStatisticsSpecification,
            String featuresUrl,
            WebCoordinateSystem coordinateSystem)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    if (SpeciesObservationConfiguration.IsElasticsearchUsed)
                    {
                        return AnalysisManager.GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCountsElasticsearch(context,
                                                                                                            searchCriteria,
                                                                                                            gridSpecification,
                                                                                                            coordinateSystem,
                                                                                                            featuresUrl);
                    }
                    else
                    {
                        return AnalysisManager.GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts(context,
                                                                                                            searchCriteria,
                                                                                                            gridSpecification,
                                                                                                            coordinateSystem,
                                                                                                            featuresUrl);
                    }
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about spatial features in a grid representation inside a user supplied bounding box.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="featureStatisticsSpecification">Information about requested information from a web feature service.</param>
        /// <param name="featuresUrl">Address to data in a web feature service.</param>
        /// <param name="featureCollectionJson">Feature collection as json string.</param>
        /// <param name="gridSpecification">Information about the returned grid.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned grid.</param>
        /// <returns>Statistical measurements on spatial features in grid format.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<WebGridCellFeatureStatistics> GetGridFeatureStatistics(WebClientInformation clientInformation,
                                                                           WebFeatureStatisticsSpecification featureStatisticsSpecification,
                                                                           String featuresUrl,
                                                                           String featureCollectionJson,
                                                                           WebGridSpecification gridSpecification,
                                                                           WebCoordinateSystem coordinateSystem)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    var featuresUri = string.IsNullOrEmpty(featuresUrl) ? null : new Uri(featuresUrl);
                    return AnalysisManager.GetGridCellFeatureStatistics(context,
                                                                     featuresUri,
                                                                     featureCollectionJson,
                                                                     gridSpecification,
                                                                     coordinateSystem);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// This method get species (Taxa of category species) and observations per grid cell that matches 
        /// species observation search criteria, grid cell specifications and specified coordinate 
        /// system to getting results in.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="gridSpecification">Grid specifications i.e settings for result grid. These settings are defined in class 
        /// WebGridSpecifications. Note! GridCellSize and GridCoordinateSystem classes are the
        /// only properties that is used in this method. If Bounding Box is set then WebSpeciesObservationSearchCriteria property 
        /// BoundingBox is override with WebGridSpecifications property BoundingBox.</param>
        /// <param name="coordinateSystem">Coordinate systen whch is used for performing grid caluculations defined in 
        /// WebCoordinateSystem</param>
        /// <returns> A list of grid cells is returned that matches. Grid cell results is presented in WebGridCellSpeciesCount class showing number of species, no of observations.</returns>
        public List<WebGridCellSpeciesCount> GetGridSpeciesCountsBySpeciesObservationSearchCriteria(
                                             WebClientInformation clientInformation,
                                             WebSpeciesObservationSearchCriteria searchCriteria,
                                             WebGridSpecification gridSpecification,
                                             WebCoordinateSystem coordinateSystem)
        {
            List<WebGridCellSpeciesCount> grid;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    if (SpeciesObservationConfiguration.IsElasticsearchUsed)
                    {
                        grid = AnalysisManager.GetGridSpeciesCountsElasticsearch(context, searchCriteria, gridSpecification, coordinateSystem);
                    }
                    else
                    {
                        grid = AnalysisManager.GetGridSpeciesCounts(context, searchCriteria, gridSpecification, coordinateSystem);
                    }

                    LogSpeciesObservationCount(grid);
                    return grid;
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
        /// that matches the search criteria and selected grid specifications.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="gridSpecification">Information about the returned grid.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned grid.</param>
        /// <returns>Information about changed species observations.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<WebGridCellSpeciesObservationCount> GetGridSpeciesObservationCountsBySpeciesObservationSearchCriteria(WebClientInformation clientInformation,
                                                                                                                          WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                                                          WebGridSpecification gridSpecification,
                                                                                                                          WebCoordinateSystem coordinateSystem)
        {
            List<WebGridCellSpeciesObservationCount> grid;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    if (SpeciesObservationConfiguration.IsElasticsearchUsed)
                    {
                        grid = AnalysisManager.GetGridSpeciesObservationCountsElasticsearch(context, searchCriteria, gridSpecification, coordinateSystem);
                    }
                    else
                    {
                        grid = AnalysisManager.GetGridSpeciesObservationCounts(context, searchCriteria, gridSpecification, coordinateSystem);
                    }

                    LogSpeciesObservationCount(grid);
                    return grid;
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get unique hosts for all species facts
        /// that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Hosts for all species facts that matches search criteria.</returns>
        public List<WebTaxon> GetHostsBySpeciesFactSearchCriteria(WebClientInformation clientInformation,
                                                                  WebSpeciesFactSearchCriteria searchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return AnalysisManager.GetHostsBySpeciesFactSearchCriteria(context,
                                                                               searchCriteria);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get species observation provenances 
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in search criteria.</param>
        /// <returns>Number of species that matches the search criteria.</returns>
        public List<WebSpeciesObservationProvenance> GetSpeciesObservationProvenancesBySearchCriteria(WebClientInformation clientInformation,
                                                                                                      WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                                      WebCoordinateSystem coordinateSystem)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    if (SpeciesObservationConfiguration.IsElasticsearchUsed)
                    {
                        return AnalysisManager.GetProvenancesBySearchCriteriaElasticsearch(context,
                                                                          searchCriteria,
                                                                          coordinateSystem);
                    }
                    else
                    {
                        return AnalysisManager.GetProvenancesBySearchCriteria(context,
                                                                          searchCriteria,
                                                                          coordinateSystem);
                    }
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get number of species 
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in search criteria.</param>
        /// <returns>Number of species that matches the search criteria.</returns>
        public Int64 GetSpeciesCountBySpeciesObservationSearchCriteria(WebClientInformation clientInformation,
                                                                       WebSpeciesObservationSearchCriteria searchCriteria,
                                                                       WebCoordinateSystem coordinateSystem)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    if (SpeciesObservationConfiguration.IsElasticsearchUsed)
                    {
                        return AnalysisManager.GetSpeciesCountBySearchCriteriaElasticsearch(context, searchCriteria, coordinateSystem);
                    }
                    else
                    {
                        return AnalysisManager.GetSpeciesCountBySearchCriteria(context, searchCriteria, coordinateSystem);
                    }
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get number of species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in search criteria.</param>
        /// <returns>Number of species observations that matches the search criteria.</returns>
        public Int64 GetSpeciesObservationCountBySpeciesObservationSearchCriteria(WebClientInformation clientInformation,
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
                            AnalysisManager.GetSpeciesObservationCountBySearchCriteriaElasticsearch(context, searchCriteria, coordinateSystem);
                    }
                    else
                    {
                        speciesObservationCount =
                            AnalysisManager.GetSpeciesObservationCountBySearchCriteria(context, searchCriteria, coordinateSystem);
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
        /// Get unique taxa for all species facts
        /// that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa for all species facts that matches search criteria.</returns>
        public List<WebTaxon> GetTaxaBySpeciesFactSearchCriteria(WebClientInformation clientInformation,
                                                                 WebSpeciesFactSearchCriteria searchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return AnalysisManager.GetTaxaBySpeciesFactSearchCriteria(context, searchCriteria);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get no of species that matches WebSpeciesObservationSearchCriteria and specified what
        /// coordinate system to use in WebCoordinateSystem.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned species observations.</param>
        /// <returns>No of species that matches search criteria.</returns>
        public List<WebTaxon> GetTaxaBySpeciesObservationSearchCriteria(WebClientInformation clientInformation,
                                                                        WebSpeciesObservationSearchCriteria searchCriteria,
                                                                        WebCoordinateSystem coordinateSystem)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    if (SpeciesObservationConfiguration.IsElasticsearchUsed)
                    {
                        return AnalysisManager.GetTaxaBySearchCriteriaElasticsearch(context, searchCriteria, coordinateSystem) as List<WebTaxon>;
                    }
                    else
                    {
                        return AnalysisManager.GetTaxaBySearchCriteria(context, searchCriteria, coordinateSystem) as List<WebTaxon>;
                    }
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get no of species, with related number of observed species, that matches WebSpeciesObservationSearchCriteria and specified what
        /// coordinate system to use in WebCoordinateSystem.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned species observations.</param>
        /// <returns>No of species that matches search criteria.</returns>
        public List<WebTaxonSpeciesObservationCount> GetTaxaWithSpeciesObservationCountsBySpeciesObservationSearchCriteria(WebClientInformation clientInformation,
                                                                        WebSpeciesObservationSearchCriteria searchCriteria,
                                                                        WebCoordinateSystem coordinateSystem)
        {
            List<WebTaxonSpeciesObservationCount> taxonSpeciesObservationCount;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    if (SpeciesObservationConfiguration.IsElasticsearchUsed)
                    {
                        taxonSpeciesObservationCount = AnalysisManager.GetTaxaWithSpeciesObservationCountsBySearchCriteriaElasticsearch(context, searchCriteria, coordinateSystem) as List<WebTaxonSpeciesObservationCount>;
                    }
                    else
                    {
                        taxonSpeciesObservationCount = AnalysisManager.GetTaxaWithSpeciesObservationCountsBySearchCriteria(context, searchCriteria, coordinateSystem) as List<WebTaxonSpeciesObservationCount>;
                    }

                    LogSpeciesObservationCount(taxonSpeciesObservationCount);
                    return taxonSpeciesObservationCount;
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get time step specific species observation counts for a specific set of species observation search criteria.
        /// Only counts greater than zero is included in the resulting list. 
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="periodicity">Specification on time step length and interval.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned species observations.</param>
        /// <returns>A list of time step specific species observation counts.</returns>
        public List<WebTimeStepSpeciesObservationCount> GetTimeSpeciesObservationCountsBySpeciesObservationSearchCriteria(WebClientInformation clientInformation,
                                                                                                                          WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                                                          Periodicity periodicity,
                                                                                                                          WebCoordinateSystem coordinateSystem)
        {
            List<WebTimeStepSpeciesObservationCount> timeStepSpeciesObservationCount;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    if (SpeciesObservationConfiguration.IsElasticsearchUsed)
                    {
                        timeStepSpeciesObservationCount = AnalysisManager.GetTimeSpeciesObservationCountsBySearchCriteriaElasticsearch(context, searchCriteria, periodicity, coordinateSystem);
                    }
                    else
                    {
                        timeStepSpeciesObservationCount = AnalysisManager.GetTimeSpeciesObservationCountsBySearchCriteria(context, searchCriteria, periodicity, coordinateSystem);
                    }

                    LogSpeciesObservationCount(timeStepSpeciesObservationCount);
                    return timeStepSpeciesObservationCount;
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
        /// <param name="grid">Grid with species observation count.</param>
        private void LogSpeciesObservationCount(List<WebGridCellSpeciesObservationCount> grid)
        {
            Int64 speciesObservationCount;

            speciesObservationCount = 0;
            if (grid.IsNotEmpty())
            {
                foreach (WebGridCellSpeciesObservationCount gridCell in grid)
                {
                    if (gridCell.IsNotNull())
                    {
                        speciesObservationCount += gridCell.Count;
                    }
                }
            }

            LogSpeciesObservationCount(speciesObservationCount);
        }

        /// <summary>
        /// Add information about retrieved species observations to Application Insights.
        /// </summary>
        /// <param name="grid">Grid with species observation count.</param>
        private void LogSpeciesObservationCount(List<WebGridCellSpeciesCount> grid)
        {
            Int64 speciesObservationCount;

            speciesObservationCount = 0;
            if (grid.IsNotEmpty())
            {
                foreach (WebGridCellSpeciesCount gridCell in grid)
                {
                    if (gridCell.IsNotNull())
                    {
                        speciesObservationCount += gridCell.SpeciesObservationCount;
                    }
                }
            }

            LogSpeciesObservationCount(speciesObservationCount);
        }

        /// <summary>
        /// Add information about retrieved species observations to Application Insights.
        /// </summary>
        /// <param name="list">List with species observation count.</param>
        private void LogSpeciesObservationCount(List<WebTaxonSpeciesObservationCount> list)
        {
            Int64 speciesObservationCount;

            speciesObservationCount = 0;
            if (list.IsNotEmpty())
            {
                foreach (WebTaxonSpeciesObservationCount taxonSpeciesObservationCount in list)
                {
                    if (taxonSpeciesObservationCount.IsNotNull())
                    {
                        speciesObservationCount += taxonSpeciesObservationCount.SpeciesObservationCount;
                    }
                }
            }

            LogSpeciesObservationCount(speciesObservationCount);
        }

        /// <summary>
        /// Add information about retrieved species observations to Application Insights.
        /// </summary>
        /// <param name="list">List with species observation count.</param>
        private void LogSpeciesObservationCount(List<WebTimeStepSpeciesObservationCount> list)
        {
            Int64 speciesObservationCount;

            speciesObservationCount = 0;
            if (list.IsNotEmpty())
            {
                foreach (WebTimeStepSpeciesObservationCount timeStepSpeciesObservationCount in list)
                {
                    if (timeStepSpeciesObservationCount.IsNotNull())
                    {
                        speciesObservationCount += timeStepSpeciesObservationCount.Count;
                    }
                }
            }

            LogSpeciesObservationCount(speciesObservationCount);
        }

        /// <summary>
        /// Get EOO as geojson, EOO and AOO area as attributes
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="gridCells">Grid cells used to calculate result</param>
        /// <param name="alphaValue">If greater than 0 a concave hull will be calculated with this alpha value</param>
        /// <param name="useCenterPoint">Used when concave hull is calculated. Grid corner coordinates used when false</param>
        /// <returns>A JSON FeatureCollection with one feature showing EOO. EOO AND AOO Areas stored in feature attributes</returns>
        public string GetSpeciesObservationAOOEOOAsGeoJson(WebClientInformation clientInformation, List<WebGridCellSpeciesObservationCount> gridCells, int alphaValue, bool useCenterPoint)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return AnalysisManager.GetSpeciesObservationAOOEOOAsGeoJson(gridCells, alphaValue, useCenterPoint);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }
    }
}

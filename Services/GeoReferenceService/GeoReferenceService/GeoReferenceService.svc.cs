using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Log;
using ArtDatabanken.WebService;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.GeoReferenceService.Data;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Wcf;

namespace GeoReferenceService
{
    /// <summary>
    /// Implementation of the service that handles geografical information.
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class GeoReferenceService : WebServiceBase, IGeoReferenceService
    {
        /// <summary>
        /// Static constructor.
        /// </summary>
        static GeoReferenceService()
        {
            WebServiceData.AuthorizationManager = new AuthorizationManager();
            WebServiceData.CoordinateConversionManager = new CoordinateConversionManager();
            WebServiceData.DatabaseManager = new DatabaseManager();
            WebServiceData.LogManager = new LogManager();
            WebServiceData.UserManager = new UserManager();
            WebServiceData.WebServiceManager = new WebServiceManager();
        }

        /// <summary>
        /// Get cities with names matching the search criteria
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">City name search criteria</param>
        /// <param name="coordinateSystem">Coordinate system used in returned geography information.</param>
        /// <returns></returns>
        public List<WebCityInformation> GetCitiesByNameSearchString(
            WebClientInformation clientInformation,
            WebStringSearchCriteria searchCriteria,
            WebCoordinateSystem coordinateSystem)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return
                        ArtDatabanken.WebService.GeoReferenceService.Data.RegionManager.GetCitiesByNameSearchString(
                            context,
                            searchCriteria,
                            coordinateSystem);
                }
                catch (Exception exception)
                {
                    LogException(clientInformation, context, exception);
                    LogParameter(context, "CoordinateSystem", coordinateSystem.WebToString());
                    LogParameter(context, "SearchCriteria", searchCriteria.WebToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Convert bounding box from one coordinate system to
        /// another coordinate system.
        /// Converted bounding box is returned as a polygon
        /// since it probably is not a rectangle any more.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="boundingBox">Bounding boxe that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Converted bounding box.</returns>       
        public WebPolygon GetConvertedBoundingBox(
            WebClientInformation clientInformation,
            WebBoundingBox boundingBox,
            WebCoordinateSystem fromCoordinateSystem,
            WebCoordinateSystem toCoordinateSystem)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return WebServiceData.CoordinateConversionManager.GetConvertedBoundingBox(
                        boundingBox,
                        fromCoordinateSystem,
                        toCoordinateSystem);
                }
                catch (Exception exception)
                {
                    LogException(clientInformation, context, exception);
                    LogParameter(context, "BoundingBox", boundingBox.WebToString());
                    LogParameter(context, "FromCoordinateSystem", fromCoordinateSystem.WebToString());
                    LogParameter(context, "ToCoordinateSystem", toCoordinateSystem.WebToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Geographic coordinate conversion of linear rings.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="linearRings">Linear rings that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Converted linear rings.</returns>       
        public List<WebLinearRing> GetConvertedLinearRings(
            WebClientInformation clientInformation,
            List<WebLinearRing> linearRings,
            WebCoordinateSystem fromCoordinateSystem,
            WebCoordinateSystem toCoordinateSystem)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return WebServiceData.CoordinateConversionManager.GetConvertedLinearRings(
                        linearRings,
                        fromCoordinateSystem,
                        toCoordinateSystem);
                }
                catch (Exception exception)
                {
                    LogException(clientInformation, context, exception);
                    LogParameter(context, "LinearRings", linearRings.WebToString());
                    LogParameter(context, "FromCoordinateSystem", fromCoordinateSystem.WebToString());
                    LogParameter(context, "ToCoordinateSystem", toCoordinateSystem.WebToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Geographic coordinate conversion of multi polygons.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="multiPolygons">Multi polygons that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Converted multi polygons.</returns>       
        public List<WebMultiPolygon> GetConvertedMultiPolygons(
            WebClientInformation clientInformation,
            List<WebMultiPolygon> multiPolygons,
            WebCoordinateSystem fromCoordinateSystem,
            WebCoordinateSystem toCoordinateSystem)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return WebServiceData.CoordinateConversionManager.GetConvertedMultiPolygons(
                        multiPolygons,
                        fromCoordinateSystem,
                        toCoordinateSystem);
                }
                catch (Exception exception)
                {
                    LogException(clientInformation, context, exception);
                    LogParameter(context, "MultiPolygons", multiPolygons.WebToString());
                    LogParameter(context, "FromCoordinateSystem", fromCoordinateSystem.WebToString());
                    LogParameter(context, "ToCoordinateSystem", toCoordinateSystem.WebToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Geographic coordinate conversion of points.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="points">Points that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Converted points.</returns>       
        public List<WebPoint> GetConvertedPoints(
            WebClientInformation clientInformation,
            List<WebPoint> points,
            WebCoordinateSystem fromCoordinateSystem,
            WebCoordinateSystem toCoordinateSystem)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return WebServiceData.CoordinateConversionManager.GetConvertedPoints(
                        points,
                        fromCoordinateSystem,
                        toCoordinateSystem);
                }
                catch (Exception exception)
                {
                    LogException(clientInformation, context, exception);
                    LogParameter(context, "Points", points.WebToString());
                    LogParameter(context, "FromCoordinateSystem", fromCoordinateSystem.WebToString());
                    LogParameter(context, "ToCoordinateSystem", toCoordinateSystem.WebToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Geographic coordinate conversion of polygons.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="polygons">Polygons that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Converted points.</returns>       
        public List<WebPolygon> GetConvertedPolygons(
            WebClientInformation clientInformation,
            List<WebPolygon> polygons,
            WebCoordinateSystem fromCoordinateSystem,
            WebCoordinateSystem toCoordinateSystem)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return WebServiceData.CoordinateConversionManager.GetConvertedPolygons(
                        polygons,
                        fromCoordinateSystem,
                        toCoordinateSystem);
                }
                catch (Exception exception)
                {
                    LogException(clientInformation, context, exception);
                    LogParameter(context, "Polygons", polygons.WebToString());
                    LogParameter(context, "FromCoordinateSystem", fromCoordinateSystem.WebToString());
                    LogParameter(context, "ToCoordinateSystem", toCoordinateSystem.WebToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Get region categories.
        /// All region categories are returned if parameter countryIsoCode is not specified.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="isCountryIsoCodeSpecified">Indicates if parameter countryIsoCode is specified.</param>
        /// <param name="countryIsoCode">
        /// Get region categories related to this country.
        /// Country iso codes are specified in standard ISO-3166.
        /// </param>
        /// <returns>Region categories.</returns>       
        public List<WebRegionCategory> GetRegionCategories(
            WebClientInformation clientInformation,
            Boolean isCountryIsoCodeSpecified,
            Int32 countryIsoCode)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.GeoReferenceService.Data.RegionManager.GetRegionCategories(
                        context,
                        isCountryIsoCodeSpecified,
                        countryIsoCode);
                }
                catch (Exception exception)
                {
                    LogException(clientInformation, context, exception);
                    LogParameter(context, "IsCountryIsoCodeSpecified", isCountryIsoCodeSpecified.WebToString());
                    LogParameter(context, "CountryIsoCode", countryIsoCode.WebToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Get regions related to specified region categories.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="regionCategories">Get regions related to specified region categories.</param>
        /// <returns>Regions related to specified region categories.</returns>       
        public List<WebRegion> GetRegionsByCategories(
            WebClientInformation clientInformation,
            List<WebRegionCategory> regionCategories)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.GeoReferenceService.Data.RegionManager.GetRegionsByCategories(context, regionCategories);
                }
                catch (Exception exception)
                {
                    LogException(clientInformation, context, exception);
                    LogParameter(context, "RegionCategories", regionCategories.WebToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Get regions by GUIDs.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="GUIDs">Region GUIDs.</param>
        /// <returns>Regions matching provided GUIDs.</returns>       
        public List<WebRegion> GetRegionsByGUIDs(WebClientInformation clientInformation, List<String> GUIDs)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.GeoReferenceService.Data.RegionManager.GetRegionsByGuids(
                        context,
                        GUIDs);
                }
                catch (Exception exception)
                {
                    LogException(clientInformation, context, exception);
                    LogParameter(context, "GUIDs", GUIDs.WebToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Get regions by ids.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="regionIds">Region ids.</param>
        /// <returns>Regions matching provided ids.</returns>       
        public List<WebRegion> GetRegionsByIds(WebClientInformation clientInformation,
                                               List<Int32> regionIds)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.GeoReferenceService.Data.RegionManager.GetRegionsByIds(
                        context,
                        regionIds);
                }
                catch (Exception exception)
                {
                    LogException(clientInformation, context, exception);
                    LogParameter(context, "RegionIds", regionIds.WebToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Get regions that matches the search criterias.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Region search criterias.</param>
        /// <returns>Regions that matches the search criterias.</returns>       
        public List<WebRegion> GetRegionsBySearchCriteria(
            WebClientInformation clientInformation,
            WebRegionSearchCriteria searchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return
                        ArtDatabanken.WebService.GeoReferenceService.Data.RegionManager.GetRegionsBySearchCriteria(
                            context,
                            searchCriteria);
                }
                catch (Exception exception)
                {
                    LogException(clientInformation, context, exception);
                    LogParameter(context, "SearchCriteria", searchCriteria.WebToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Get geography for regions.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="GUIDs">Region GUIDs.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned geography information.</param>
        /// <returns>Geography for regions.</returns>       
        public List<WebRegionGeography> GetRegionsGeographyByGUIDs(
            WebClientInformation clientInformation,
            List<String> GUIDs,
            WebCoordinateSystem coordinateSystem)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return
                        ArtDatabanken.WebService.GeoReferenceService.Data.RegionManager.GetRegionsGeographyByGuids(
                            context,
                            GUIDs,
                            coordinateSystem);
                }
                catch (Exception exception)
                {
                    LogException(clientInformation, context, exception);
                    LogParameter(context, "GUIDs", GUIDs.WebToString());
                    LogParameter(context, "CoordinateSystem", coordinateSystem.WebToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Get geography for regions.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="regionIds">Region ids.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned geography information.</param>
        /// <returns>Geography for regions.</returns>       
        public List<WebRegionGeography> GetRegionsGeographyByIds(
            WebClientInformation clientInformation,
            List<Int32> regionIds,
            WebCoordinateSystem coordinateSystem)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return
                        ArtDatabanken.WebService.GeoReferenceService.Data.RegionManager.GetRegionsGeographyByIds(
                            context,
                            regionIds,
                            coordinateSystem);
                }
                catch (Exception exception)
                {
                    LogException(clientInformation, context, exception);
                    LogParameter(context, "RegionIds", regionIds.WebToString());
                    LogParameter(context, "CoordinateSystem", coordinateSystem.WebToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all region types.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All region types.</returns>       
        public List<WebRegionType> GetRegionTypes(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.GeoReferenceService.Data.RegionManager.GetRegionTypes(context);
                }
                catch (Exception exception)
                {
                    LogException(clientInformation, context, exception);
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
        /// Log exception.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="context">Web service request context.</param>
        /// <param name="exception">Log this exception.</param>
        private void LogException(WebClientInformation clientInformation,
                                  WebServiceContext context,
                                  Exception exception)
        {
            RequestTelemetry telemetry;

            try
            {
                if (clientInformation.IsNotNull() && (Configuration.InstallationType == InstallationType.Production))
                {
                    telemetry = OperationContext.Current.GetRequestTelemetry();
                    if (telemetry.IsNotNull())
                    {
                        if (clientInformation.Locale.IsNotNull())
                        {
                            telemetry.Properties[TelemetryProperty.LocaleId.ToString()] = clientInformation.Locale.Id.WebToString();
                        }

                        if (clientInformation.Role.IsNotNull())
                        {
                            telemetry.Properties[TelemetryProperty.RoleId.ToString()] = clientInformation.Role.Id.WebToString();
                        }

                        if (context.IsNotNull())
                        {
                            telemetry.Properties[TelemetryProperty.RequestId.ToString()] = context.RequestId.WebToString();
                        }
                    }
                }

                if (context.IsNotNull())
                {
                    WebServiceData.LogManager.LogError(context, exception);
                }
            }
            catch (Exception)
            {
                // Do nothing. We are already in an exception handling sequence.
            }
        }

        /// <summary>
        /// Log parameter.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="parameterValue">Parameter value.</param>
        private void LogParameter(WebServiceContext context,
                                  String parameterName,
                                  String parameterValue)
        {
            Dictionary<String, String> properties;
            TelemetryClient telemetryClient;

            try
            {
                if (Configuration.InstallationType == InstallationType.Production)
                {
                    telemetryClient = new TelemetryClient();
                    if (telemetryClient.IsNotNull())
                    {
                        properties = new Dictionary<String, String>();
                        if (context.IsNotNull())
                        {
                            properties[TelemetryProperty.RequestId.ToString()] = context.RequestId.WebToString();
                        }

                        telemetryClient.TrackTrace(TelemetryProperty.Parameter + parameterName + " = " + parameterValue,
                                                   SeverityLevel.Information,
                                                   properties);
                    }
                }
            }
            catch (Exception)
            {
                // Do nothing. We are already in an exception handling sequence.
            }
        }
    }
}
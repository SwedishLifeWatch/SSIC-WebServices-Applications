using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArtDatabanken.WebService.Data;
using Microsoft.ApplicationInsights.Wcf;

namespace GeoReferenceService
{
    /// <summary>
    /// Interface to service that handles geografical information.
    /// </summary>
    [ServiceContract(Namespace = "urn:WebServices.ArtDatabanken.slu.se",
                     SessionMode = SessionMode.NotAllowed)]
    public interface IGeoReferenceService
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
        ///  Get Cities with names that match the search criteria
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">City name search criteria</param>
        /// <param name="coordinateSystem">Coordinate system used in returned geography information.</param>
        /// <returns></returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebCityInformation> GetCitiesByNameSearchString(
                                                                WebClientInformation clientInformation,
                                                                WebStringSearchCriteria searchCriteria,
                                                                WebCoordinateSystem coordinateSystem);

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
        [OperationContract]
        [OperationTelemetry]
        WebPolygon GetConvertedBoundingBox(WebClientInformation clientInformation,
                                           WebBoundingBox boundingBox,
                                           WebCoordinateSystem fromCoordinateSystem,
                                           WebCoordinateSystem toCoordinateSystem);

        /// <summary>
        /// Geographic coordinate conversion of linear rings.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="linearRings">Linear rings that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Converted linear rings.</returns>       
        [OperationContract]
        [OperationTelemetry]
        List<WebLinearRing> GetConvertedLinearRings(WebClientInformation clientInformation,
                                                    List<WebLinearRing> linearRings,
                                                    WebCoordinateSystem fromCoordinateSystem,
                                                    WebCoordinateSystem toCoordinateSystem);

        /// <summary>
        /// Geographic coordinate conversion of multi polygons.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="multiPolygons">Multi polygons that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Converted multi polygons.</returns>       
        [OperationContract]
        [OperationTelemetry]
        List<WebMultiPolygon> GetConvertedMultiPolygons(WebClientInformation clientInformation,
                                                        List<WebMultiPolygon> multiPolygons,
                                                        WebCoordinateSystem fromCoordinateSystem,
                                                        WebCoordinateSystem toCoordinateSystem);

        /// <summary>
        /// Geographic coordinate conversion of points.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="points">Points that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Converted points.</returns>       
        [OperationContract]
        [OperationTelemetry]
        List<WebPoint> GetConvertedPoints(WebClientInformation clientInformation,
                                          List<WebPoint> points,
                                          WebCoordinateSystem fromCoordinateSystem,
                                          WebCoordinateSystem toCoordinateSystem);

        /// <summary>
        /// Geographic coordinate conversion of polygons.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="polygons">Polygons that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Converted polygons.</returns>       
        [OperationContract]
        [OperationTelemetry]
        List<WebPolygon> GetConvertedPolygons(WebClientInformation clientInformation,
                                              List<WebPolygon> polygons,
                                              WebCoordinateSystem fromCoordinateSystem,
                                              WebCoordinateSystem toCoordinateSystem);

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
        [OperationContract]
        [OperationTelemetry]
        List<WebRegionCategory> GetRegionCategories(WebClientInformation clientInformation,
                                                    Boolean isCountryIsoCodeSpecified,
                                                    Int32 countryIsoCode);

        /// <summary>
        /// Get regions related to specified region categories.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="regionCategories">Get regions related to specified region categories.</param>
        /// <returns>Regions related to specified region categories.</returns>       
        [OperationContract]
        [OperationTelemetry]
        List<WebRegion> GetRegionsByCategories(WebClientInformation clientInformation,
                                               List<WebRegionCategory> regionCategories);

        /// <summary>
        /// Get regions by GUIDs.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="GUIDs">Region GUIDs.</param>
        /// <returns>Regions matching provided GUIDs.</returns>       
        [OperationContract]
        [OperationTelemetry]
        List<WebRegion> GetRegionsByGUIDs(WebClientInformation clientInformation,
                                          List<String> GUIDs);

        /// <summary>
        /// Get regions by ids.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="regionIds">Region ids.</param>
        /// <returns>Regions matching provided ids.</returns>       
        [OperationContract]
        [OperationTelemetry]
        List<WebRegion> GetRegionsByIds(WebClientInformation clientInformation,
                                        List<Int32> regionIds);

        /// <summary>
        /// Get regions that matches the search criterias.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Region search criterias.</param>
        /// <returns>Regions that matches the search criterias.</returns>       
        [OperationContract]
        [OperationTelemetry]
        List<WebRegion> GetRegionsBySearchCriteria(WebClientInformation clientInformation,
                                                   WebRegionSearchCriteria searchCriteria);

        /// <summary>
        /// Get geography for regions.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="GUIDs">Region GUIDs.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned geography information.</param>
        /// <returns>Geography for regions.</returns>       
        [OperationContract]
        [OperationTelemetry]
        List<WebRegionGeography> GetRegionsGeographyByGUIDs(WebClientInformation clientInformation,
                                                            List<String> GUIDs,
                                                            WebCoordinateSystem coordinateSystem);

        /// <summary>
        /// Get geography for regions.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="regionIds">Region ids.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned geography information.</param>
        /// <returns>Geography for regions.</returns>       
        [OperationContract]
        [OperationTelemetry]
        List<WebRegionGeography> GetRegionsGeographyByIds(WebClientInformation clientInformation,
                                                          List<Int32> regionIds,
                                                          WebCoordinateSystem coordinateSystem);

        /// <summary>
        /// Get all region types.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All region types.</returns>       
        [OperationContract]
        [OperationTelemetry]
        List<WebRegionType> GetRegionTypes(WebClientInformation clientInformation);

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
        /// <param name="password">Password.</param>
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
        /// Note: Tracing has negativ impact on web service performance.
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

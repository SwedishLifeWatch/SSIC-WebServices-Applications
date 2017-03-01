using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using Microsoft.ApplicationInsights.Wcf;

namespace AnalysisService
{
    /// <summary>
    /// Interface to the web service AnalysisService.
    /// This web service is used to perform calculations on 
    /// species observations made in Sweden.
    /// </summary>
    [ServiceContract(Namespace = "urn:WebServices.ArtDatabanken.slu.se",
                     SessionMode = SessionMode.NotAllowed)]
    public interface IAnalysisService
    {
        /// <summary>
        /// Clear data cache in web service.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        [OperationContract]
        void ClearCache(WebClientInformation clientInformation);

        /// <summary>
        /// Delete trace information from the web service log.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        [OperationContract]
        void DeleteTrace(WebClientInformation clientInformation);

        /// <summary>
        /// Get statistics about features in a web feature service.
        /// Returned statistics is adjusted to specified grid. 
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="featureStatisticsSpecification">Information about requested information from a web feature service.</param>
        /// <param name="featuresUrl">Address to data in a web feature service.</param>
        /// <param name="featureCollectionJson">Feature collection as json string.</param>
        /// <param name="gridSpecification">Information about the returned grid.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned grid.</param>
        /// <returns>Statistics about features in a web feature service.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), OperationContract]
        [OperationTelemetry]
        List<WebGridCellFeatureStatistics> GetGridFeatureStatistics(WebClientInformation clientInformation,
                                                                    WebFeatureStatisticsSpecification featureStatisticsSpecification,
                                                                    String featuresUrl,
                                                                    String featureCollectionJson,
                                                                    WebGridSpecification gridSpecification,
                                                                    WebCoordinateSystem coordinateSystem);

        /// <summary>
        /// Gets the grid cell feature statistics combined with species observation counts.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="gridSpecification">The grid specification: bounding box, grid cell size, etc.</param>
        /// <param name="searchCriteria">Species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="featureStatisticsSpecification">Information about requested information from a web feature service.</param>
        /// <param name="featuresUrl">Address to data in a web feature service.</param>
        /// <param name="featureCollectionJson">Feature collection as json</param>
        /// <param name="coordinateSystem">Coordinate system used in returned grid.</param>
        /// <returns>A list with combined result from GetGridSpeciesCounts() and GetGridCellFeatureStatistics().</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebGridCellCombinedStatistics> GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts(
            WebClientInformation clientInformation,
            WebGridSpecification gridSpecification,
            WebSpeciesObservationSearchCriteria searchCriteria,
            WebFeatureStatisticsSpecification featureStatisticsSpecification,
            String featuresUrl,
            String featureCollectionJson,
            WebCoordinateSystem coordinateSystem);

        /// <summary>
        /// Gets the grid feature statistics combined with species observation counts.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="gridSpecification">The grid specification: bounding box, grid cell size, etc.</param>
        /// <param name="searchCriteria">Species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="featureStatisticsSpecification">Information about requested information from a web feature service.</param>
        /// <param name="featuresUrl">Address to data in a web feature service.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned grid.</param>
        /// <returns>A list with combined result from GetGridSpeciesCounts() and GetGridCellFeatureStatistics().</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebGridCellCombinedStatistics> GetGridFeatureStatisticsCombinedWithSpeciesObservationCounts(
            WebClientInformation clientInformation,
            WebGridSpecification gridSpecification,
            WebSpeciesObservationSearchCriteria searchCriteria,
            WebFeatureStatisticsSpecification featureStatisticsSpecification,
            String featuresUrl,
            WebCoordinateSystem coordinateSystem);

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
        [OperationContract]
        [OperationTelemetry]
        List<WebGridCellSpeciesCount> GetGridSpeciesCountsBySpeciesObservationSearchCriteria(WebClientInformation clientInformation,
                                                                                             WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                             WebGridSpecification gridSpecification,
                                                                                             WebCoordinateSystem coordinateSystem);

        /// <summary>
        /// Gets no of species observations
        /// that matches the search criteria and selected grid specifications.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="gridSpecification">Information about the returned grid.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned grid.</param>
        /// <returns>Information about changed species observations.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), OperationContract]
        [OperationTelemetry]
        List<WebGridCellSpeciesObservationCount> GetGridSpeciesObservationCountsBySpeciesObservationSearchCriteria(WebClientInformation clientInformation,
                                                                                                                   WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                                                   WebGridSpecification gridSpecification,
                                                                                                                   WebCoordinateSystem coordinateSystem);

        /// <summary>
        /// Get unique hosts for all species facts
        /// that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Hosts for all species facts that matches search criteria.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxon> GetHostsBySpeciesFactSearchCriteria(WebClientInformation clientInformation,
                                                           WebSpeciesFactSearchCriteria searchCriteria);

        /// <summary>
        /// Get entries from the web service log
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="type">Get log entries of this type.</param>
        /// <param name="userName">Get log entries for this user. May be empty.</param>
        /// <param name="rowCount">Maximum number of log entries to get.</param>
        /// <returns> Requested web log entries.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), OperationContract]
        List<WebLogRow> GetLog(WebClientInformation clientInformation,
                               LogType type,
                               String userName,
                               Int32 rowCount);

        /// <summary>
        /// Get species observation provenances 
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in search criteria.</param>
        /// <returns>
        /// Species observation provenances 
        /// that matches the search criteria.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebSpeciesObservationProvenance> GetSpeciesObservationProvenancesBySearchCriteria(WebClientInformation clientInformation,
                                                                                               WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                               WebCoordinateSystem coordinateSystem);

        /// <summary>
        /// Get number of species 
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in search criteria.</param>
        /// <returns>Number of species that matches the search criteria.</returns>
        [OperationContract]
        [OperationTelemetry]
        Int64 GetSpeciesCountBySpeciesObservationSearchCriteria(WebClientInformation clientInformation,
                                                                WebSpeciesObservationSearchCriteria searchCriteria,
                                                                WebCoordinateSystem coordinateSystem);

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
        Int64 GetSpeciesObservationCountBySpeciesObservationSearchCriteria(WebClientInformation clientInformation,
                                                                           WebSpeciesObservationSearchCriteria searchCriteria,
                                                                           WebCoordinateSystem coordinateSystem);

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Status for this web service.</returns>       
        [OperationContract]
        List<WebResourceStatus> GetStatus(WebClientInformation clientInformation);

        /// <summary>
        /// Get unique taxa for all species facts
        /// that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa for all species facts that matches search criteria.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxon> GetTaxaBySpeciesFactSearchCriteria(WebClientInformation clientInformation,
                                                          WebSpeciesFactSearchCriteria searchCriteria);

        /// <summary>
        /// Get no of species that matches WebSpeciesObservationSearchCriteria and specified what
        /// coordinate system to use in WebCoordinateSystem.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned species observations.</param>
        /// <returns>No of species that matches search criteria.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Taxa"), OperationContract]
        [OperationTelemetry]
        List<WebTaxon> GetTaxaBySpeciesObservationSearchCriteria(WebClientInformation clientInformation,
                                                                 WebSpeciesObservationSearchCriteria searchCriteria,
                                                                 WebCoordinateSystem coordinateSystem);

        /// <summary>
        /// Get no of species, with related number of observed species, that matches WebSpeciesObservationSearchCriteria and specified what
        /// coordinate system to use in WebCoordinateSystem.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned species observations.</param>
        /// <returns>No of species that matches search criteria.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Taxa"), OperationContract]
        [OperationTelemetry]
        List<WebTaxonSpeciesObservationCount> GetTaxaWithSpeciesObservationCountsBySpeciesObservationSearchCriteria(WebClientInformation clientInformation,
                                                                 WebSpeciesObservationSearchCriteria searchCriteria,
                                                                 WebCoordinateSystem coordinateSystem);

        /// <summary>
        /// Get time step specific species observation counts for a specific set of species observation search criteria.
        /// Only counts greater than zero is included in the resulting list. 
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="periodicity">Specification on time step length and interval.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned species observations.</param>
        /// <returns>A list of time step specific species observation counts.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTimeStepSpeciesObservationCount> GetTimeSpeciesObservationCountsBySpeciesObservationSearchCriteria(WebClientInformation clientInformation,
                                                                                                                   WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                                                   Periodicity periodicity,
                                                                                                                   WebCoordinateSystem coordinateSystem);

        /// <summary>
        /// Get EOO as geojson, EOO and AOO area as attributes
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="gridCells">Grid cells used to calculate result</param>
        /// <param name="alphaValue">If greater than 0 a concave hull will be calculated with this alpha value</param>
        /// <param name="useCenterPoint">Used when concave hull is calculated. Grid corner coordinates used when false</param>
        /// <returns>A JSON FeatureCollection with one feature showing EOO. EOO AND AOO Areas stored in feature attributes</returns>
        [OperationContract]
        [OperationTelemetry]
        string GetSpeciesObservationAOOEOOAsGeoJson(WebClientInformation clientInformation, List<WebGridCellSpeciesObservationCount> gridCells, int alphaValue, bool useCenterPoint);
        
        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password for logged in user.</param>
        /// <param name="applicationIdentifier">Identifier of the application that the user uses.</param>
        /// <param name="isActivationRequired">Flag that indicates that the user account must
        /// be activated before login can succeed.</param>
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
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="userName">The user name to trace.</param>
        [OperationContract]
        void StartTrace(WebClientInformation clientInformation,
                        String userName);

        /// <summary>
        /// Stop tracing usage of web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        [OperationContract]
        void StopTrace(WebClientInformation clientInformation);
    }
}

using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Database;

namespace ArtDatabanken.WebService.SpeciesObservation.Data
{
    /// <summary>
    /// Interface to species observation database related functionality.
    /// </summary>
    public interface ISpeciesObservationManager
    {
        /// <summary>
        /// Check that the species observation
        /// database is not updating right now.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <exception cref="ApplicationException">Thrown if database is beeing updated.</exception>
        void CheckAutomaticDatabaseUpdate(WebServiceContext context);

        /// <summary>
        /// Check that the species observation
        /// database is not updating right now.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <exception cref="ApplicationException">Thrown if database is beeing updated.</exception>
        void CheckManualDatabaseUpdate(WebServiceContext context);

        /// <summary>
        /// Check that all species observation fields has been
        /// mapped in Elasticsearch.
        /// </summary>
        /// <param name="context"> Web service request context.</param>
        /// <param name="speciesObservation">Species observation.</param>
        /// <param name="elasticsearch">Proxy to Elasticsearch.</param>
        void CheckMappingElasticsearch(WebServiceContext context,
                                       WebSpeciesObservation speciesObservation,
                                       ElasticsearchSpeciesObservationProxy elasticsearch);

        /// <summary>
        /// Get mapping for species observation fields.
        /// </summary>
        /// <param name="context">Web service request context. </param>
        /// <param name="elasticsearch">Proxy to Elasticsearch.</param>
        /// <returns>Mapping for species observation fields.</returns>
        Dictionary<String, WebSpeciesObservationField> GetMapping(WebServiceContext context,
                                                                  ElasticsearchSpeciesObservationProxy elasticsearch);

        /// <summary>
        /// Get specified species observation data provider.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationDataProviderId">Species observation data provider id.</param>
        /// <returns>Specified species observation data provider.</returns>
        WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context,
                                                                            SpeciesObservationDataProviderId speciesObservationDataProviderId);

        /// <summary>
        /// Get information related to Elasticsearch and species observations.
        /// </summary>
        /// <param name="context">Web service request context. </param>
        /// <returns>Information related to Elasticsearch and species observations.</returns>
        SpeciesObservationElasticsearch GetSpeciesObservationElasticsearch(WebServiceContext context);

        /// <summary>
        /// Convert species observations from JSON to
        /// instances of class WebSpeciesObservation.
        /// </summary>
        /// <param name="speciesObservationsJson">Species observation in JSON format.</param>
        /// <param name="mapping">Species observation field information mapping.</param>
        /// <returns>Species observations instances of class WebSpeciesObservation.</returns>
        List<WebSpeciesObservation> GetSpeciesObservations(String speciesObservationsJson,
                                                           Dictionary<String, WebSpeciesObservationField> mapping);

        /// <summary>
        /// Update information related to Elasticsearch and species observations.
        /// </summary>
        /// <param name="context">Web service request context. </param>
        /// <param name="currentIndexChangeId">Max species observation change id that has been processed to Elasticsearch.</param>
        /// <param name="currentIndexCount">Number of species observations in current index in Elasticsearch.</param>
        /// <param name="currentIndexName">Name of current index in Elasticsearch.</param>
        /// <param name="nextIndexChangeId">
        /// Max species observation change id that has been
        /// processed into next index in Elasticsearch.
        /// </param>
        /// <param name="nextIndexCount">Number of species observations in next index in Elasticsearch.</param>
        /// <param name="nextIndexName">Name of next index in Elasticsearch.</param>
        void UpdateSpeciesObservationElasticsearch(WebServiceContext context,
                                                   Int64? currentIndexChangeId,
                                                   Int64? currentIndexCount,
                                                   String currentIndexName,
                                                   Int64? nextIndexChangeId,
                                                   Int64? nextIndexCount,
                                                   String nextIndexName);

        /// <summary>
        /// Get number of species observations that matches
        /// provided species observation search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>
        /// Number of species observations that matches
        /// provided species observation search criteria.
        /// </returns>
        Int64 GetSpeciesObservationCountBySearchCriteriaElasticsearch(WebServiceContext context,
                                                                      WebSpeciesObservationSearchCriteria searchCriteria,
                                                                      WebCoordinateSystem coordinateSystem);

        /// <summary>
        /// Convert polygons from provided coordinate
        /// system to a Elasticsearch coordinate system.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="inputPolygons">Input polygons.</param>
        /// <param name="regionGuids">Region GUIDs.</param>
        /// <param name="inputCoordinateSystem">Which coordinate system the coordinates should be converted from.</param>
        /// <returns>Polygons in Elasticsearch coordinate system.</returns>
        List<WebPolygon> ConvertToElasticSearchCoordinates(WebServiceContext context,
                                                           List<WebPolygon> inputPolygons,
                                                           List<String> regionGuids,
                                                           WebCoordinateSystem inputCoordinateSystem);

        /// <summary>
        /// Check that species observation search criteria is valid.
        /// And also changes the searchCriteria by converting coordinates and adding taxonid's
        /// This method should only be used together with Elasticsearch.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in geometry search criteria
        /// and returned species observations.
        /// </param>
        void CheckData(WebServiceContext context,
                       WebSpeciesObservationSearchCriteria searchCriteria,
                       WebCoordinateSystem coordinateSystem);
        
    }
}

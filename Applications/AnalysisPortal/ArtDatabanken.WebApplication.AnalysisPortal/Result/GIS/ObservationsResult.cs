using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.Data;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.GIS.GeoJSON.Net.Geometry;
using Newtonsoft.Json;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.GIS
{
    /// <summary>
    /// This class contains observations as GeoJSON
    /// </summary>
    [DataContract]
    public class SpeciesObservationsGeoJsonModel
    {
        [JsonProperty(PropertyName = "points")]
        [DataMember]
        public FeatureCollection Points { get; set; }

        [JsonProperty(PropertyName = "spatialFilter")]
        [DataMember]
        public FeatureCollection SpatialFilter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeciesObservationsGeoJsonModel"/> class.
        /// </summary>
        /// <param name="points">The points.</param>
        public SpeciesObservationsGeoJsonModel(FeatureCollection points)
        {
            Points = points;
        }

        public SpeciesObservationsGeoJsonModel(FeatureCollection points, FeatureCollection spatialFilter)
        {
            Points = points;
            SpatialFilter = spatialFilter;
        }

        /// <summary>
        /// Creates an ObservationsGISResult from a list of species observations.
        /// </summary>
        /// <param name="speciesObservationList">The observations list.</param>
        /// <returns></returns>
        public static SpeciesObservationsGeoJsonModel CreateResult(SpeciesObservationList speciesObservationList, FeatureCollection spatialFilter)
        {
            var features = new List<Feature>();
            foreach (ISpeciesObservation observation in speciesObservationList)
            {
                // if x or y-coordinate doesn't exist, continue
                if (!observation.Location.CoordinateX.HasValue || !observation.Location.CoordinateY.HasValue)
                {
                    continue;
                }

                var pos = new GeographicPosition(observation.Location.CoordinateX.Value, observation.Location.CoordinateY.Value);
                var point = new ArtDatabanken.GIS.GeoJSON.Net.Geometry.Point(pos);

                var dicProperties = new Dictionary<string, object>();                              
                dicProperties.Add("observationId", observation.Id);
                dicProperties.Add("siteType", 2);
                dicProperties.Add("accuracy", observation.Location.CoordinateUncertaintyInMeters);

                var feature = new Feature(point, dicProperties);
                feature.Id = observation.DatasetID;
                features.Add(feature);
            }

            var featureCollection = new FeatureCollection(features);            
            var result = new SpeciesObservationsGeoJsonModel(featureCollection, spatialFilter);
            return result;            
        }

        public static SpeciesObservationsGeoJsonModel CreateResult(SpeciesObservationList speciesObservationList)
        {
            return CreateResult(speciesObservationList, null);
        }

        /// <summary>
        /// Tries to parse text into integer
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <returns></returns>
        private static int? NullableTryParseInt32(string text)
        {
            int value;
            return int.TryParse(text, out value) ? (int?)value : null;
        }
    }
}

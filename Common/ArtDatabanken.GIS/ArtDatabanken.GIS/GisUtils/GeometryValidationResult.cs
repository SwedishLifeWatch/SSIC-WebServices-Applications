using System;
using System.Collections.Generic;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;

namespace ArtDatabanken.GIS.GisUtils
{
    /// <summary>
    /// Geometry validation result.
    /// </summary>
    public struct GeometryValidationResult
    {
        /// <summary>
        /// Specifies whether the Geometry is valid or not.
        /// </summary>        
        public bool IsValid { get; set; }

        /// <summary>
        /// Geometry description.
        /// </summary>  
        public String Description { get; set; }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>        
        public Int32 StatusCode { get; set; }

        /// <summary>
        /// Geometry validation status id enum.
        /// </summary>        
        public GeometryValidationStatusId GeometryValidationStatusId { get; set; }
    }

    /// <summary>
    /// Validation result for a feature collection.
    /// </summary>
    public struct FeatureCollectionValidationResult
    {
        /// <summary>
        /// Specifies whether all feature geometries is valid or not.
        /// </summary>
        public Boolean IsValid { get; set; }

        /// <summary>
        /// All features with invalid geometries.
        /// </summary>        
        public List<FeatureValidationResult> InvalidFeatureResults { get; set; }
    }

    /// <summary>
    /// Validation result for a feature.
    /// </summary>
    public struct FeatureValidationResult
    {
        /// <summary>
        /// The feature with invalid geometry.
        /// </summary>        
        public Feature Feature { get; set; }
        /// <summary>
        /// Validation result.
        /// </summary>        
        public GeometryValidationResult ValidationResult { get; set; }
    }
}
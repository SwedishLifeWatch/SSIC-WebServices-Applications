using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using Microsoft.SqlServer.Types;

namespace ArtDatabanken.GIS.GisUtils
{    
    /// <summary>
    /// This class contains Gismethods, such as methods to check or manipulate geometries.
    /// </summary>
    public class GeometryTools
    {
        /// <summary>
        /// Checks a FeatureCollection object for errors in its Geometry. E.g. self intersection.
        /// </summary>
        /// <param name="featureCollection">The feature collection.</param>
        /// <returns>Validation result.</returns>
        public FeatureCollectionValidationResult ValidateFeatureCollectionGeometries(FeatureCollection featureCollection)
        {
            var geometryTools = new GeometryTools();
           
            var featureCollectionValidation = new FeatureCollectionValidationResult
            {
                IsValid = true,
                InvalidFeatureResults = new List<FeatureValidationResult>(),                
            };

            foreach (Feature feature in featureCollection.Features)
            {
                var validationResult = geometryTools.ValidateFeatureGeometry(feature);
                if (!validationResult.IsValid)
                {
                    featureCollectionValidation.IsValid = false;
                    featureCollectionValidation.InvalidFeatureResults.Add(new FeatureValidationResult
                    {
                        Feature = feature,
                        ValidationResult = validationResult,                        
                    });                    
                }
            }

            return featureCollectionValidation;
        }

        /// <summary>
        /// Checks a Feature object for errors in its Geometry. E.g. self intersection.
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <returns>Validation result.</returns>
        public GeometryValidationResult ValidateFeatureGeometry(Feature feature)
        {
            GeometryValidationResult result = new GeometryValidationResult();
            GeometryConversionTool geometryConversionTool = new GeometryConversionTool();
            List<SqlGeometry> sqlGeometries = geometryConversionTool.FeatureGeometryToSqlGeometries(feature);
            if (sqlGeometries.Any())
            {
                foreach (SqlGeometry sqlGeometry in sqlGeometries)
                {
                    result = ValidateGeometry(sqlGeometry);
                    if (!result.IsValid)
                    {
                        return result;
                    }
                }

                return result;
            }

            return new GeometryValidationResult
            {
                IsValid = true,
                StatusCode = (int)GeometryValidationStatusId.Valid,
                GeometryValidationStatusId = GeometryValidationStatusId.Valid
            };            
        }

        /// <summary>
        /// Checks a Polygon object for errors. E.g. self intersection.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <returns>Validation result.</returns>
        public GeometryValidationResult ValidateGeometry(IPolygon polygon)
        {
            GeometryConversionTool geometryConversionTool = new GeometryConversionTool();
            SqlGeometry sqlGeometry = geometryConversionTool.PolygonToSqlGeometry(polygon);
            return ValidateGeometry(sqlGeometry);
        }

        /// <summary>
        /// Checks a SqlGeometry object for errors. E.g. self intersection.
        /// </summary>
        /// <param name="sqlGeometry">The SQL geometry.</param>
        /// <returns>Validation result.</returns>
        public GeometryValidationResult ValidateGeometry(SqlGeometry sqlGeometry)
        {
            const int DESCRIPTION_START_INDEX = 7;
            const int STATUS_CODE_END_INDEX = 5;
            string str = sqlGeometry.IsValidDetailed();
            int statusCode = int.Parse(str.Substring(0, STATUS_CODE_END_INDEX));
            string description = str.Substring(DESCRIPTION_START_INDEX, str.Length - DESCRIPTION_START_INDEX);
            GeometryValidationStatusId geometryValidationStatusId;
            if (!TryConvertIntToEnum(statusCode, out geometryValidationStatusId))
            {
                geometryValidationStatusId = GeometryValidationStatusId.Unknown;
            }
            GeometryValidationResult result = new GeometryValidationResult
            {
                StatusCode = statusCode,
                Description = description,
                IsValid = geometryValidationStatusId == GeometryValidationStatusId.Valid,                
                GeometryValidationStatusId = geometryValidationStatusId
            };
            return result;
        }

        /// <summary>
        /// Converts int to enum.
        /// </summary>        
        /// <param name="value">The value.</param>
        /// <param name="result">The result.</param>
        /// <returns>True if the conversion succeeds; otherwise false.</returns>
        private bool TryConvertIntToEnum<T>(int value, out T result)
        {
            result = default(T);
            bool success = Enum.IsDefined(typeof(T), value);
            if (success)
            {
                result = (T)Enum.ToObject(typeof(T), value);
            }
            return success;
        }
    }
}




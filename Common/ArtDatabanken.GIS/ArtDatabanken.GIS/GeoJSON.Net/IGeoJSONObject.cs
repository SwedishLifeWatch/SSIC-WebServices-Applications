﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGeoJSONObject.cs" company="Jörg Battermann">
//   Copyright © Jörg Battermann 2011
// </copyright>
// <summary>
//   Defines the IGeoJSONObject interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace ArtDatabanken.GIS.GeoJSON.Net
{
    /// <summary>
    /// Base Interface for GeoJSONObject types.
    /// </summary>    
    public interface IGeoJSONObject
    {
        /// <summary>
        /// Gets the (mandatory) type of the <see>GeoJSON Object
        ///         <cref>http://geojson.org/geojson-spec.html#geojson-objects</cref>
        ///     </see>.
        /// </summary>
        /// <value>
        /// The type of the object.
        /// </value>
        GeoJSONObjectType Type { get; }

        /// <summary>
        /// Gets the (optional) <see>Coordinate Reference System Object
        ///         <cref>http://geojson.org/geojson-spec.html#coordinate-reference-system-objects</cref>
        ///     </see>.
        /// </summary>
        /// <value>
        /// The Coordinate Reference System Objects.
        /// </value>
        CoordinateReferenceSystem.ICRSObject CRS { get; }

        /// <summary>
        /// Gets or sets the (optional) <see>Bounding Boxes
        ///         <cref>http://geojson.org/geojson-spec.html#coordinate-reference-system-objects</cref>
        ///     </see>.
        /// </summary>
        /// <value>
        /// The value of the bbox member must be a 2*n array where n is the number of dimensions represented in the
        /// contained geometries, with the lowest values for all axes followed by the highest values.
        /// The axes order of a bbox follows the axes order of geometries.
        /// In addition, the coordinate reference system for the bbox is assumed to match the coordinate reference
        /// system of the GeoJSON object of which it is a member.
        /// </value>
        double[] BoundingBoxes { get; set; }
    }
}

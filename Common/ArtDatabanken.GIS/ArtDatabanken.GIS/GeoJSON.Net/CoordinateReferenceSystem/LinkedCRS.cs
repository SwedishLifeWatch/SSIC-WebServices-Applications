﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkedCRS.cs" company="Jörg Battermann">
//   Copyright © Jörg Battermann 2011
// </copyright>
// <summary>
//   Defines the <see cref="http://geojson.org/geojson-spec.html#named-crs">Linked CRS type</see>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace ArtDatabanken.GIS.GeoJSON.Net.CoordinateReferenceSystem
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see>Linked CRS type
    ///         <cref>http://geojson.org/geojson-spec.html#linked-crs</cref>
    ///     </see>.
    /// </summary>
    [DataContract]
    public class LinkedCRS : CRSBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkedCRS"/> class.
        /// </summary>
        /// <param name="href">The mandatory <see>href
        ///     <cref>http://geojson.org/geojson-spec.html#linked-crs</cref>
        /// </see> member must be a dereferenceable URI.</param>
        /// <param name="type">The optional type member will be put in the properties Dictionary as specified in the <see>GeoJSON spec
        ///     <cref>http://geojson.org/geojson-spec.html#linked-crs</cref>
        /// </see>.</param>
        public LinkedCRS(string href, string type = "")
        {
            if (href == null)
            {
                throw new ArgumentNullException("href");
            }

            if (string.IsNullOrWhiteSpace(href))
            {
                throw new ArgumentOutOfRangeException("href", "May not be empty");
            }

            this.Properties = new Dictionary<string, object> { { "href", href } };

            if (!string.IsNullOrWhiteSpace(type))
            {
                this.Properties.Add("type", type);
            }

            this.Type = CRSType.Link;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkedCRS"/> class.
        /// </summary>
        /// <param name="href">The mandatory <see>href
        ///     <cref>http://geojson.org/geojson-spec.html#linked-crs</cref>
        /// </see> member must be a dereferenceable URI.</param>
        /// <param name="type">The optional type member will be put in the properties Dictionary as specified in the <see>GeoJSON spec
        ///     <cref>http://geojson.org/geojson-spec.html#linked-crs</cref>
        /// </see>.</param>
        public LinkedCRS(Uri href, string type = "") : this(href.ToString(), type)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkedCRS"/> class.
        /// </summary>
        public LinkedCRS()
        {
            Type = CRSType.Link;
            Properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets the href.
        /// </summary>        
        public string Href
        {
            get
            {
                if (Properties != null)
                {
                    object hrefValue;
                    if (Properties.TryGetValue("href", out hrefValue))
                    {
                        if (hrefValue is string)
                        {
                            return (string)hrefValue;
                        }
                    }
                }

                return string.Empty;
            }
        }
    }
}

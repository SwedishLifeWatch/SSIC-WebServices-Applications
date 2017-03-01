// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NamedCRS.cs" company="Jörg Battermann">
//   Copyright © Jörg Battermann 2011
// </copyright>
// <summary>
//   Defines the <see cref="http://geojson.org/geojson-spec.html#named-crs">Named CRS type</see>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace ArtDatabanken.GIS.GeoJSON.Net.CoordinateReferenceSystem
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see>Named CRS type
    ///         <cref>http://geojson.org/geojson-spec.html#named-crs</cref>
    ///     </see>.
    /// </summary>
    [DataContract]
    public class NamedCRS : CRSBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedCRS"/> class.
        /// </summary>
        /// <param name="name">
        /// The mandatory <see>name
        ///         <cref>http://geojson.org/geojson-spec.html#named-crs</cref>
        ///     </see>
        /// member must be a string identifying a coordinate reference system. OGC CRS URNs such as
        /// 'urn:ogc:def:crs:OGC:1.3:CRS84' shall be preferred over legacy identifiers such as 'EPSG:4326'.
        /// </param>
        public NamedCRS(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException("name", "May not be empty");
            }

            Properties = new Dictionary<string, object> { { "name", name } };

            Type = CRSType.Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedCRS"/> class.
        /// </summary>
        public NamedCRS()
        {
            Type = CRSType.Name;
            Properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets the name.
        /// </summary>        
        public string Name
        {
            get
            {
                if (Properties != null)
                {
                    object nameValue;
                    if (Properties.TryGetValue("name", out nameValue))
                    {
                        if (nameValue is string)
                        {
                            return (string)nameValue;                            
                        }
                    }
                }

                return string.Empty;
            }
        }
    }
}

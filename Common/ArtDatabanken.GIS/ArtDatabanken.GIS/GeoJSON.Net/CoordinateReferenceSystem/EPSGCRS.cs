// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EPSGCRS.cs" company="ÅF">
//   Copyright ©  2016
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
    public class EPSGCRS : CRSBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EPSGCRS"/> class.
        /// </summary>
        /// <param name="code">
        /// The mandatory <see>name
        ///         <cref>http://geojson.org/geojson-spec.html#coordinate-reference-system-objects</cref>
        ///     </see>
        /// </param>
        public EPSGCRS(int? code)
        {
            if (!code.HasValue)
            {
                throw new ArgumentNullException("code");
            }

            if (code.Value == 0)
            {
                throw new ArgumentOutOfRangeException("code", "May not be 0");
            }

            Properties = new Dictionary<string, object> { { "code", code } };

            Type = CRSType.EPSG;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EPSGCRS"/> class.
        /// </summary>
        public EPSGCRS()
        {
            Type = CRSType.EPSG;
            Properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets the name.
        /// </summary>        
        public int Code
        {
            get
            {
                if (Properties != null)
                {
                    object codeValue;
                    if (Properties.TryGetValue("code", out codeValue))
                    {
                        var code = 0;

                        if (int.TryParse(codeValue.ToString(), out code))
                        {
                            return code;                            
                        }
                    }
                }

                return 0;
            }
        }
    }
}

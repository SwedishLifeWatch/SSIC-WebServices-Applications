// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeographicPosition.cs" company="Jörg Battermann">
//   Copyright © Jörg Battermann 2011
// </copyright>
// <summary>
//   Defines the Geographic Position type a.k.a. <see cref="http://geojson.org/geojson-spec.html#positions">Geographic Coordinate Reference System</see>.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ArtDatabanken.GIS.GeoJSON.Net.Geometry
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the Geographic Position type a.k.a. <see>Geographic Coordinate Reference System
    ///         <cref>http://geojson.org/geojson-spec.html#positions</cref>
    ///     </see>.
    /// </summary>
    [DataContract]
    public class GeographicPosition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeographicPosition"/> class.
        /// </summary>
        /// <param name="longitude">The longitude.</param>
        /// <param name="latitude">The latitude.</param>        
        /// <param name="altitude">The altitude in m(eter).</param>
        public GeographicPosition(double longitude, double latitude, double? altitude = null)
            : this()
        {
            this.Longitude = longitude;
            this.Latitude = latitude;            
            this.Altitude = altitude;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeographicPosition"/> class.
        /// </summary>
        /// <param name="longitude">The longitude, e.g. '-77.008889'.</param>
        /// <param name="latitude">The latitude, e.g. '38.889722'.</param>        
        /// <param name="altitude">The altitude in m(eter).</param>
        public GeographicPosition(string longitude, string latitude, string altitude = null)
            : this()
        {

            if (longitude == null)
            {
                throw new ArgumentNullException("longitude");
            }

            if (latitude == null)
            {
                throw new ArgumentNullException("latitude");
            }

            if (string.IsNullOrWhiteSpace(longitude))
            {
                throw new ArgumentOutOfRangeException("longitude", "May not be empty.");
            }

            if (string.IsNullOrWhiteSpace(latitude))
            {
                throw new ArgumentOutOfRangeException("latitude", "May not be empty.");
            }

            double lng;
            double lat;
            
//            !double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            //double.TryParse(latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out lat);
            //Double.Parse("3,5".Replace(',', '.'), CultureInfo.InvariantCulture); 
            latitude = latitude.Replace(',', '.');
            longitude = longitude.Replace(',', '.');
            if (!string.IsNullOrEmpty(altitude))
            {
                altitude = altitude.Replace(',', '.');
            }

            if (!double.TryParse(longitude, NumberStyles.Float, CultureInfo.InvariantCulture, out lng))
            {
                throw new ArgumentOutOfRangeException("longitude", "Longitude must be a proper lon (+/- double) value, e.g. '-77.008889'.");
            }

            if (!double.TryParse(latitude, NumberStyles.Float, CultureInfo.InvariantCulture, out lat))
            {
                throw new ArgumentOutOfRangeException("latitude", "Latitude must be a proper lat (+/- double) value, e.g. '38.889722'.");
            }

            this.Longitude = lng;
            this.Latitude = lat;
            

            if (altitude == null)
            {
                this.Altitude = null;
            }
            else
            {
                double alt;
                if (!double.TryParse(altitude, NumberStyles.Float, CultureInfo.InvariantCulture, out alt))
                {
                    throw new ArgumentOutOfRangeException("altitude", "Altitude must be a proper altitude (m(eter) as double) value, e.g. '6500'.");
                }

                this.Altitude = alt;
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="GeographicPosition"/> class from being created.
        /// </summary>
        private GeographicPosition()
        {
            this.Coordinates = new double?[3];
        }


        /// <summary>
        /// Gets or sets the coordinates, is a 2-size array
        /// </summary>
        /// <value>
        /// The coordinates.
        /// </value>
        [DataMember(Order=0)]
        private double?[] Coordinates { get; set; }


        /// <summary>
        /// Gets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        [DataMember(Order = 1)]
        public double Longitude
        {
            get
            {
                return this.Coordinates[0].GetValueOrDefault();
            }

            private set
            {
                this.Coordinates[0] = value;
            }
        }

        /// <summary>
        /// Gets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        [DataMember(Order = 2)]
        public double Latitude
        {
            get
            {
                return this.Coordinates[1].GetValueOrDefault();
            }

            private set
            {
                this.Coordinates[1] = value;
            }
        }


        /// <summary>
        /// Gets the X value (longitude).
        /// </summary>
        /// <value>The X value (longitude).</value>
        [DataMember(Order = 1)]
        public double X
        {
            get
            {
                return Longitude;
            }

            private set
            {
                Longitude = value;
            }
        }

        /// <summary>
        /// Gets the Y value (latitude).
        /// </summary>
        /// <value>The Y value (latitude).</value>
        [DataMember(Order = 2)]
        public double Y
        {
            get
            {
                return Latitude;
            }

            private set
            {
                Latitude = value;
            }
        }



        /// <summary>
        /// Gets the altitude.
        /// </summary>
        [DataMember(Order = 3)]
        public double? Altitude
        {
            get
            {
                return this.Coordinates[2];
            }

            private set
            {
                this.Coordinates[2] = value;
            }
        }


        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Altitude == null ? string.Format(CultureInfo.InvariantCulture, "Longitude: {0}, Latitude: {1}", this.Longitude, this.Latitude) : string.Format(CultureInfo.InvariantCulture, "Longitude: {0}, Latitude: {1}, Altitude: {2}", this.Longitude, this.Latitude, this.Altitude);
        }

        /// <summary>
        /// Determines whether the specified GeographicPosition is equal to this instance.
        /// </summary>
        /// <param name="other">The GeographicPosition to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified GeographicPosition is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(GeographicPosition other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            const double precision = 0.0001;
            return Math.Abs(this.Latitude - other.Latitude) < precision && 
                Math.Abs(this.Longitude - other.Longitude) < precision && 
                Math.Abs(this.Altitude.GetValueOrDefault() - other.Altitude.GetValueOrDefault()) < precision;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (GeographicPosition)) return false;
            return Equals((GeographicPosition) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return (Coordinates != null ? Coordinates.GetHashCode() : 0);
        }
        
    }
}

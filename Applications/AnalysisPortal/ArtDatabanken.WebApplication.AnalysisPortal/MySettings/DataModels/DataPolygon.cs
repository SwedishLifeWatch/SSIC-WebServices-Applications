using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataModels
{
    /// <summary>
    /// A Polygon is a planar Surface, defined by 1 exterior boundary
    /// and 0 or more interior boundaries. Each interior boundary
    /// defines a hole in the Polygon.
    /// </summary>
    [DataContract]
    public class DataPolygon : IEquatable<DataPolygon>
    {
        /// <summary>
        /// Linear rings that defines the polygon.
        /// </summary>
        [DataMember]
        public List<DataLinearRing> LinearRings { get; set; }

        #region Equality members

        public bool Equals(DataPolygon other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return LinearRings.SequenceEqual(other.LinearRings);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((DataPolygon)obj);
        }

        public override int GetHashCode()
        {            
            int hashCode = HashCodeHelper.GetOrderIndependentHashCode(LinearRings);
            return hashCode;
        }

        public static bool operator ==(DataPolygon left, DataPolygon right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DataPolygon left, DataPolygon right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}

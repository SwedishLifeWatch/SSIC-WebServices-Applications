using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataModels
{
    /// <summary>
    /// A LinearRing is a LineString that is both closed and simple.
    /// A LineString is a curve with linear interpolation
    /// between points. Each consecutive pair of points
    /// defines a line segment.
    /// </summary>
    [DataContract]
    public class DataLinearRing : IEquatable<DataLinearRing>
    {
        /// <summary>
        /// Points that defines the LinearRing.
        /// </summary>]
        [DataMember]
        public List<DataPoint> Points { get; set; }

        #region Equality members

        public bool Equals(DataLinearRing other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Points.SequenceEqual(other.Points);            
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

            return Equals((DataLinearRing)obj);
        }

        public override int GetHashCode()
        {
            int hashCode = HashCodeHelper.GetOrderIndependentHashCode(Points);
            return hashCode;
        }

        public static bool operator ==(DataLinearRing left, DataLinearRing right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DataLinearRing left, DataLinearRing right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}

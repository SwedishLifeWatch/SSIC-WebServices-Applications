using System;
using System.Runtime.Serialization;
using ArtDatabanken.GIS.WFS.DescribeFeature;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders.MapLayers
{
    /// <summary>
    /// This class contains basic information about a WFS layer
    /// </summary>
    [DataContract]
    public class WfsLayerSetting : IEquatable<WfsLayerSetting>
    {
        /// <summary>
        /// Gets or sets the unique id.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the server URL.
        /// </summary>
        [DataMember]
        public string ServerUrl { get; set; }

        /// <summary>
        /// Gets or sets the TypeName.
        /// </summary>        
        [DataMember]
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the name of the geometry field.
        /// </summary>        
        [DataMember]
        public string GeometryName { get; set; }

        /// <summary>
        /// Gets or sets the type of the geometry field.
        /// </summary>        
        [DataMember]        
        public GeometryType GeometryType { get; set; }
        //public string GeometryType { get; set; }

        /// <summary>
        /// Gets or sets the filter as an XML-string.
        /// </summary>
        [DataMember]
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets the layer color.
        /// </summary>
        [DataMember]
        public string Color { get; set; }

        /// <summary>
        /// Determines whether the bounding box of the current spatial filter,
        /// will act as bounding box for the WFS layer.
        /// Useful if the WFS layer contains much data.
        /// </summary>
        [DataMember]        
        public bool UseSpatialFilterExtentAsBoundingBox { get; set; }

        [DataMember]
        public bool IsFile { get; set; }

        public bool Equals(WfsLayerSetting other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id == other.Id && string.Equals(Name, other.Name) && string.Equals(ServerUrl, other.ServerUrl) && string.Equals(TypeName, other.TypeName) && string.Equals(GeometryName, other.GeometryName) && GeometryType == other.GeometryType && string.Equals(Filter, other.Filter) && string.Equals(Color, other.Color);
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

            return Equals((WfsLayerSetting)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Id;
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ServerUrl != null ? ServerUrl.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (TypeName != null ? TypeName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (GeometryName != null ? GeometryName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)GeometryType;
                hashCode = (hashCode * 397) ^ (Filter != null ? Filter.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Color != null ? Color.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(WfsLayerSetting left, WfsLayerSetting right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(WfsLayerSetting left, WfsLayerSetting right)
        {
            return !Equals(left, right);
        }
    }
}

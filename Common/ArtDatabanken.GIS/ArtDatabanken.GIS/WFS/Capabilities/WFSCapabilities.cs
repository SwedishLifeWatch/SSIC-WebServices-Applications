using System;
using System.Collections.Generic;
using ArtDatabanken.GIS.WFS.DescribeFeature;

namespace ArtDatabanken.GIS.WFS.Capabilities
{
    /// <summary>
    /// IListExtensions
    /// </summary>
    public static class IListExtensions
    {
        /// <summary>
        /// IsNotEmpty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static Boolean IsNotEmpty<T>(this IList<T> collection)
        {
            return (collection != null) && (collection.Count >= 1);
        }
    }

    /// <summary>
    /// WFSCapabilities
    /// </summary>
    public class WFSCapabilities
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public WfsCapability Capability { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public List<WfsFeatureType> FeatureTypes { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public WfsService Service { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public object FilterCapabilities { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string Version { get; set; }
    }

    /// <summary>
    /// WfsFeatureType
    /// </summary>
    public class WfsFeatureType
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public WfsTypeName Name { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public List<string> Keywords { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string SRS { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public WfsBoundingBox BoundingBox { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string MetadataURL { get; set; }

        /// <summary>
        /// Gets or sets DescribeFeatureType.
        /// This is not part of the GetCapabilites request, but can be merged
        /// when an DescribeFeatureType request has been made.
        /// </summary>        
        public WFSDescribeFeatureType DescribeFeatureType { get; set; }
    }

    /// <summary>
    /// TBD ?!?
    /// </summary>
    public class WfsBoundingBox
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string CRS { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string Dimensions { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string LowerCorner { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string UpperCorner { get; set; }
    }

    /// <summary>
    /// TBD ?!?
    /// </summary>
    public class WfsCapability
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public WfsRequests Requests { get; set; }
    }

    /// <summary>
    /// TBD ?!?
    /// </summary>
    public class WfsRequests
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public WfsGetFeaturesRequest GetFeaturesRequest { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public WfsDescribeFeatureTypeRequest DescribeFeatureTypeRequest { get; set; }
    }

    /// <summary>
    /// TBD ?!?
    /// </summary>
    public class WfsDescribeFeatureTypeRequest
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public List<string> Formats { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string GetUrl { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string PostUrl { get; set; }
    }

    /// <summary>
    /// TBD ?!?
    /// </summary>
    public class WfsGetFeaturesRequest
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public List<string> Formats { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public List<string> ResultType { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string GetUrl { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string PostUrl { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string GetUrlBase
        {
            get
            {
                var uri = new Uri(GetUrl);
                string baseUrl = uri.GetLeftPart(UriPartial.Path);
                return baseUrl;
            }
        }
    }

    /// <summary>
    /// TBD ?!?
    /// </summary>
    public class WfsService
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public List<string> Keywords { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string Fees { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public List<string> AccessConstraints { get; set; }

    }

    /// <summary>
    /// TBD ?!?
    /// </summary>
    public class WfsTypeName
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string FullName
        {
            get { return string.Format("{0}:{1}", Namespace, Name); }
        }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public WfsTypeName(string ns, string name)
        {
            Namespace = ns;
            Name = name;
        }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public WfsTypeName(string typeName)
        {
            int nsIndex = typeName.IndexOf(":");
            string ns = typeName.Substring(0, nsIndex);
            string name = typeName.Substring(nsIndex + 1, typeName.Length - nsIndex - 1);
            Name = name;
            Namespace = ns;            
        }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public WfsTypeName()
        {

        }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public override string ToString()
        {
            return FullName;
        }
    }

    

}

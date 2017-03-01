using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using ArtDatabanken.GIS.WFS.Capabilities.Parsers;
using ArtDatabanken.GIS.WFS.Capabilities;
using ArtDatabanken.GIS.WFS.DescribeFeature;
using ArtDatabanken.GIS.WFS.DescribeFeature.Parsers;
using ArtDatabanken.GIS.WFS.Filter;
using ArtDatabanken.GIS.WFS.Filter.Formula;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;

namespace ArtDatabanken.GIS.WFS
{
    using Newtonsoft.Json;

    /// <summary>
    /// WFS version enum
    /// </summary>
    public enum WFSVersion
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        Ver100,
        /// <summary>
        /// TBD ?!?
        /// </summary>
        Ver110,
        /// <summary>
        /// TBD ?!?
        /// </summary>
        Unknown
    }


    /// <summary>
    /// This class is a manager class for handling WFS requests
    /// </summary>
    public static class WFSManager
    {        
        private static readonly WFSCapabilitiesParserVer100 WFSCapabilitiesParserVer100 = new WFSCapabilitiesParserVer100();
        private static readonly WFSCapabilitiesParserVer110 WFSCapabilitiesParserVer110 = new WFSCapabilitiesParserVer110();
        private static readonly WFSDescribeFeatureParser WFSDescribeFeatureParser = new WFSDescribeFeatureParser();


        #region WFS Common

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public static string AddOrReplaceSrsInUrl(string url, string newSrsName)
        {
            string srsName;
            int index = 0;
            index = url.IndexOf("EPSG:", index, System.StringComparison.Ordinal);
            if (index < 0)
            {
                // add srs
                url = url.Trim() + "&srsName=" + newSrsName;
                return url;
            }
            // replace srs
            int endIndex = url.IndexOf("&", index, System.StringComparison.Ordinal);
            if (endIndex < 0)
                endIndex = url.Length;                
            srsName = url.Substring(index, endIndex - index).Trim();
            url = url.Replace(srsName, newSrsName);
            return url;
        }

        /// <summary>
        /// Extract the srid number from the WFS url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Matching coordinate system</returns>
        public static string GetCoordinateSystemSridFromUrl(string url)
        {
            string srsName;
            int index = 0;
            index = url.IndexOf("EPSG:", index, System.StringComparison.Ordinal);
            if (index < 0)
                return null;
            index = index + 5;
            int endIndex = url.IndexOf("&", index, System.StringComparison.Ordinal);
            if (endIndex < 0)
                endIndex = url.Length;

            srsName = url.Substring(index, endIndex - index).Trim();
            return srsName;
        }

        /// <summary>
        /// Extract the srid number from the WFS url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Matching coordinate system</returns>
        public static string GetCoordinateSystemSrsFromUrl(string url)
        {
            string srsName;
            int index = 0;
            index = url.IndexOf("EPSG:", index, System.StringComparison.Ordinal);
            if (index < 0)
                return null;
            int endIndex = url.IndexOf("&", index, System.StringComparison.Ordinal);
            if (endIndex < 0)
                endIndex = url.Length;

            srsName = url.Substring(index, endIndex - index).Trim();
            return srsName;
        }

        /// <summary>
        /// Gets the WFS version from a WFS request url.
        /// </summary>
        /// <param name="url">The WFS request url</param>
        /// <returns></returns>
        public static WFSVersion GetWFSVersionFromRequestUrl(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    return WFSVersion.Unknown;
                }
                string urlString = url.ToLower();
                const string versionSearchString = "version=";
                const int versionLength = 5;
                int index = urlString.IndexOf(versionSearchString);
                if (index >= 0)
                {
                    if (urlString.Length >= index + versionLength + versionSearchString.Length)
                    {
                        string strVersion = urlString.Substring(index + versionSearchString.Length, versionLength);
                        return GetWFSVersionFromString(strVersion);
                    }
                }
                return WFSVersion.Unknown;
            }
            catch (Exception)
            {
                return WFSVersion.Unknown;
            }           
        }

        /// <summary>
        /// Gets the WFS version from a string.
        /// </summary>
        /// <param name="wfsVersion">The WFS version string.</param>
        /// <returns></returns>
        public static WFSVersion GetWFSVersionFromString(string wfsVersion)
        {
            if (wfsVersion == "1.0.0")
            {
                return WFSVersion.Ver100;
            }
            if (wfsVersion == "1.1.0")
            {
                return WFSVersion.Ver110;
            }
            return WFSVersion.Unknown;
        }


        /// <summary>
        /// Gets the WFS version string from WFSVersion enum.
        /// </summary>
        /// <param name="version">The WFS version.</param>
        /// <returns></returns>
        public static string GetWFSVersionStringFromEnum(WFSVersion version)
        {
            switch (version)
            {
                case WFSVersion.Ver100:
                    return "1.0.0";
                case WFSVersion.Ver110:
                    return "1.1.0";
                default:
                    throw new Exception("WFS Version is unknown");
            }
        }

        #endregion


        #region WFS Capabilities

        /// <summary>
        /// Requests the WFS capabilities from a web server.
        /// </summary>
        /// <param name="url">The WFS URL.</param>
        /// <returns></returns>
        public static WFSCapabilities GetWFSCapabilities(string url)
        {
            var version = GetWFSVersionFromRequestUrl(url);
            if (version == WFSVersion.Unknown)
            {
                version = WFSVersion.Ver110;
            }
            return GetWFSCapabilities(url, version);
        }


        /// <summary>
        /// Requests the WFS capabilities from a web server.
        /// </summary>
        /// <param name="url">The WFS URL.</param>
        /// <param name="version">The WFS version.</param>
        /// <returns></returns>
        /// <remarks>The function version parameter is chosen before the query string version parameter</remarks>
        public static WFSCapabilities GetWFSCapabilities(string url, WFSVersion version)
        {
            string requestUrl = CreateGetCapabiltiesRequestUrl(url, version);
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                string strXml = wc.DownloadString(requestUrl);
                return ParseCapabilities(strXml, version);
            }
        }


        /// <summary>
        /// Parses a WFS capabilities XML string.
        /// </summary>
        /// <param name="strXml">The WFS xml string.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public static WFSCapabilities ParseCapabilities(string strXml, WFSVersion version)
        {
            switch (version)
            {
                case WFSVersion.Ver100:
                    return WFSCapabilitiesParserVer100.Parse(strXml);
                case WFSVersion.Ver110:
                    return WFSCapabilitiesParserVer110.Parse(strXml);
                default:
                    throw new Exception("WFS Version is unknown");
            }
        }

        /// <summary>
        /// Creates a WFS GetCapabilities url.
        /// </summary>
        /// <param name="strUrl">The url to the wfs service</param>
        /// <param name="version">The WFS version.</param>
        /// <returns></returns>
        public static string CreateGetCapabiltiesRequestUrl(string strUrl, WFSVersion version)
        {
            if (version == WFSVersion.Unknown)
            {
                throw new Exception("WFSVersion is unknown");
            }

            var strVersion = GetWFSVersionStringFromEnum(version);
            var uriBuilder = new UriBuilder(strUrl);
            var uri = uriBuilder.Uri;
            var baseUrl = uri.GetLeftPart(UriPartial.Path);

            return string.Format("{0}?service=wfs&request=GetCapabilities&version={1}", baseUrl, strVersion);
        }

        #endregion


        #region WFS DescribeFeature


        /// <summary>
        /// Makes a WFS DescribeFeatureType Request and return a dictionary where the key is the 
        /// full name of the feature layer and the value is its corresponding WFSDescribeFeature        
        /// </summary>
        /// <param name="url">The WFS server URL.</param>
        /// <param name="featureTypes">The feature layers to get information about.</param>        
        /// <returns></returns>
        public static Dictionary<string, WFSDescribeFeatureType> GetWFSDescribeFeatureTypes(string url, List<WfsFeatureType> featureTypes)
        {
            List<WfsTypeName> typeNames = featureTypes.Select(featureType => featureType.Name).ToList();
            return GetWFSDescribeFeatureTypes(url, typeNames);
        }

        /// <summary>
        /// Makes a WFS DescribeFeatureType Request and return a dictionary where the key is the 
        /// full name of the feature layer and the value is its corresponding WFSDescribeFeature        
        /// </summary>
        /// <param name="url">The WFS server URL.</param>
        /// <param name="featureTypes">The feature layers to get information about.</param>
        /// <param name="version">The WFS version.</param>
        /// <returns></returns>
        public static Dictionary<string, WFSDescribeFeatureType> GetWFSDescribeFeatureTypes(string url, List<WfsFeatureType> featureTypes, WFSVersion version)
        {
            List<WfsTypeName> typeNames = featureTypes.Select(featureType => featureType.Name).ToList();
            return GetWFSDescribeFeatureTypes(url, typeNames, version);
        }

        /// <summary>
        /// Makes a WFS DescribeFeatureType Request and return a dictionary where the key is the 
        /// full name of the feature layer and the value is its corresponding WFSDescribeFeature        
        /// </summary>
        /// <param name="url">The WFS server URL.</param>
        /// <param name="typeNames">The feature layers to get information about.</param>                
        /// <returns></returns>
        public static Dictionary<string, WFSDescribeFeatureType> GetWFSDescribeFeatureTypes(string url, List<WfsTypeName> typeNames)
        {
            var version = GetWFSVersionFromRequestUrl(url);
            if (version == WFSVersion.Unknown)
            {
                version = WFSVersion.Ver110;
            }
            return GetWFSDescribeFeatureTypes(url, typeNames, version);
        }

        /// <summary>
        /// Makes WFS DescribeFeatureType Request and return a dictionary where the key is the 
        /// full name of the feature layer and the value is its corresponding WFSDescribeFeature        
        /// </summary>
        /// <param name="url">The WFS server URL.</param>        
        /// <param name="typeNames">The feature layers to get information about.</param>
        /// <param name="version">The WFS version.</param>
        /// <returns></returns>
        public static Dictionary<string, WFSDescribeFeatureType> GetWFSDescribeFeatureTypes(string url, List<WfsTypeName> typeNames, WFSVersion version)
        {
            var dicDescribeFeaturesResult = new Dictionary<string, WFSDescribeFeatureType>();

            Dictionary<string, List<WfsTypeName>> dicTypeNames = GetTypeNamesByNamespace(typeNames);
            foreach (List<WfsTypeName> typeNameList in dicTypeNames.Values)
            {
                List<List<WfsTypeName>> validTypeNameLists = GetTypeNameListsOfValidSize(typeNameList);

                foreach (List<WfsTypeName> validTypeNameList in validTypeNameLists)
                {                    
                    string requestUrl = CreateDescribeFeaturesRequestUrl(url, validTypeNameList, version);    

                    using (WebClient wc = new WebClient())
                    {
                        wc.Encoding = Encoding.UTF8;
                        string strXml = wc.DownloadString(requestUrl);
                        Dictionary<string, WFSDescribeFeatureType> dicDescribeFeature = ParseDescribeFeatureTypes(strXml, version);
                        foreach (KeyValuePair<string, WFSDescribeFeatureType> pair in dicDescribeFeature)
                        {
                            if (!dicDescribeFeaturesResult.ContainsKey(pair.Key))
                                dicDescribeFeaturesResult.Add(pair.Key, pair.Value);
                        }
                    }                        

                }               
            }
            return dicDescribeFeaturesResult;
        }


        /// <summary>
        /// Creates a list with list of type names.
        /// There is usually a query string request length maximum on web servers that we don't want to exceed.
        /// This is why this function is used.
        /// </summary>
        /// <param name="typeNames">The type names.</param>
        /// <returns></returns>
        private static List<List<WfsTypeName>> GetTypeNameListsOfValidSize(IEnumerable<WfsTypeName> typeNames)
        {
            const int maxLength = 1800;
            int totalLength = 0;
            var typeNamesList = new List<List<WfsTypeName>>();
            var typeNameList = new List<WfsTypeName>();
            typeNamesList.Add(typeNameList);
            foreach (WfsTypeName typeName in typeNames)
            {
                if (totalLength > maxLength)
                {
                    totalLength = 0;
                    typeNameList = new List<WfsTypeName>();
                    typeNamesList.Add(typeNameList);
                }
                typeNameList.Add(typeName);
                totalLength += typeName.FullName.Length;                
            }
            return typeNamesList;
        }


        /// <summary>
        /// Gets the type names by namespace.
        /// </summary>
        /// <param name="typeNames">The type names.</param>
        /// <returns></returns>
        private static Dictionary<string, List<WfsTypeName>> GetTypeNamesByNamespace(IEnumerable<WfsTypeName> typeNames)
        {
            var dicTypeNames = new Dictionary<string, List<WfsTypeName>>();
            foreach (WfsTypeName typeName in typeNames)
            {
                if (!dicTypeNames.ContainsKey(typeName.Namespace))
                    dicTypeNames.Add(typeName.Namespace, new List<WfsTypeName>());
                dicTypeNames[typeName.Namespace].Add(typeName);
            }

            return dicTypeNames;
        }

        

        /// <summary>
        /// Makes a WFS DescribeFeatureType Request and returns a WFSDescribeFeature.
        /// </summary>
        /// <param name="url">The WFS server URL.</param>
        /// <param name="featureType">The feature layer to get information about.</param>
        /// <param name="version">The WFS version.</param>
        /// <returns></returns>
        public static WFSDescribeFeatureType GetWFSDescribeFeatureType(string url, WfsFeatureType featureType, WFSVersion version)
        {
            return GetWFSDescribeFeatureType(url, string.Format("{0}:{1}", featureType.Name.Namespace, featureType.Name), version);
        }

        /// <summary>
        /// Makes a WFS DescribeFeatureType Request and returns a WFSDescribeFeature.
        /// </summary>
        /// <param name="url">The WFS server URL.</param>
        /// <param name="featureType">The feature layer to get information about.</param>        
        /// <returns></returns>
        public static WFSDescribeFeatureType GetWFSDescribeFeatureType(string url, WfsFeatureType featureType)
        {
            return GetWFSDescribeFeatureType(url, string.Format("{0}:{1}", featureType.Name.Namespace, featureType.Name));
        }

        /// <summary>
        /// Makes a WFS DescribeFeatureType Request and returns a WFSDescribeFeature.
        /// </summary>
        /// <param name="url">The WFS server URL.</param>        
        /// <param name="namespace">The feature layer namespace</param>
        /// <param name="name">The feature layer name</param>
        /// <returns></returns>
        public static WFSDescribeFeatureType GetWFSDescribeFeatureType(string url, string @namespace, string name)
        {
            return GetWFSDescribeFeatureType(url, string.Format("{0}:{1}", @namespace, name));
        }

        /// <summary>
        /// Makes a WFS DescribeFeatureType Request and returns a WFSDescribeFeature.
        /// </summary>
        /// <param name="url">The WFS server URL.</param>        
        /// <param name="name">The feature layer name</param>
        /// <param name="version">The WFS version.</param>
        /// <param name="namespace">The feature layer namespace</param>
        /// <returns></returns>
        public static WFSDescribeFeatureType GetWFSDescribeFeatureType(string url, string @namespace, string name, WFSVersion version)
        {
            return GetWFSDescribeFeatureType(url, string.Format("{0}:{1}", @namespace, name), version);
        }


        /// <summary>
        /// Makes a WFS DescribeFeatureType Request and returns a WFSDescribeFeature.
        /// </summary>
        /// <param name="url">The WFS server URL.</param>        
        /// <param name="typeName">The feature layer name including namespace</param>
        /// <returns></returns>
        public static WFSDescribeFeatureType GetWFSDescribeFeatureType(string url, string typeName)
        {
            var version = GetWFSVersionFromRequestUrl(url);
            if (version == WFSVersion.Unknown)
            {
                version = WFSVersion.Ver110;
            }
            return GetWFSDescribeFeatureType(url, typeName, version);
        }

        /// <summary>
        /// Makes a WFS DescribeFeatureType Request and returns a WFSDescribeFeature.
        /// </summary>
        /// <param name="url">The WFS server URL.</param>        
        /// <param name="typeName">The feature layer name including namespace</param>
        /// <param name="version">The WFS version.</param>
        /// <returns></returns>
        public static WFSDescribeFeatureType GetWFSDescribeFeatureType(string url, string typeName, WFSVersion version)
        {
            string requestUrl = CreateDescribeFeatureTypeRequestUrl(url, typeName, version);
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                string strXml = wc.DownloadString(requestUrl);
                WFSDescribeFeatureType result = ParseDescribeFeatureType(strXml, version);
                result.Name = new WfsTypeName(typeName);
                return result;
                //return ParseDescribeFeatureType(strXml, version);
            }
        }


        /// <summary>
        /// Parses a WFS response XML into a WFSDescribeFeature object        
        /// </summary>
        /// <param name="strXml">WFS response XML.</param>
        /// <param name="version">The WFS version.</param>
        /// <returns></returns>
        public static WFSDescribeFeatureType ParseDescribeFeatureType(string strXml, WFSVersion version)
        {
            return WFSDescribeFeatureParser.ParseDescribeFeatureType(strXml);            
        }


        /// <summary>
        /// Parses a WFS response XML into a Dictionary where the key is the full name of the feature
        /// and the value is its corresponding WFSDescribeFeature
        /// </summary>
        /// <param name="strXml">WFS response XML.</param>
        /// <param name="version">The WFS version.</param>
        /// <returns></returns>
        public static Dictionary<string,WFSDescribeFeatureType> ParseDescribeFeatureTypes(string strXml, WFSVersion version)
        {
            return WFSDescribeFeatureParser.ParseDescribeFeatureTypes(strXml);
        }

        /// <summary>
        /// Creates a WFS DescribeFeatureType request URL.
        /// </summary>
        /// <param name="strUrl">The WFS server URL.</param>
        /// <param name="namespace">The namespace of the feature layer you want info about.</param>
        /// <param name="name">The name of the feature layer you want info about.</param>
        /// <param name="version">The WFS version.</param>        
        /// <returns></returns>
        public static string CreateDescribeFeatureTypeRequestUrl(string strUrl, string @namespace, string name,  WFSVersion version)
        {
            return CreateDescribeFeatureTypeRequestUrl(strUrl, string.Format("{0}:{1}", @namespace, name), version);
        }

        /// <summary>
        /// Creates a WFS DescribeFeatureType request URL.
        /// </summary>
        /// <param name="strUrl">The WFS server URL.</param>        
        /// <param name="typeName">The feature layer you want info about.</param>
        /// <param name="version">The WFS version.</param>
        /// <returns></returns>
        public static string CreateDescribeFeatureTypeRequestUrl(string strUrl, string typeName, WFSVersion version)
        {
            if (version == WFSVersion.Unknown)
            {
                throw new Exception("WFSVersion is unknown");
            }

            string strVersion = GetWFSVersionStringFromEnum(version);
            var uri = new Uri(strUrl);
            string baseUrl = uri.GetLeftPart(UriPartial.Path);
            var strReq = new StringBuilder(baseUrl);
            strReq.Append("?");
            strReq.AppendFormat("service=wfs&");
            strReq.AppendFormat("request=DescribeFeatureType&");
            strReq.AppendFormat("TypeName=" + typeName + "&");
            strReq.AppendFormat("version=" + strVersion);

            return strReq.ToString();
        }


        /// <summary>
        /// Creates a WFS DescribeFeatureType request URL.
        /// </summary>
        /// <param name="strUrl">The WFS server URL.</param>
        /// <param name="typeNames">The features layer names you want info about.</param>
        /// <param name="version">The WFS version.</param>
        /// <returns></returns>
        public static string CreateDescribeFeaturesRequestUrl(string strUrl, List<WfsTypeName> typeNames, WFSVersion version)
        {
            var strTypeNames = typeNames.Select(typeName => typeName.FullName).ToList();
            return CreateDescribeFeaturesRequestUrl(strUrl, strTypeNames, version);
        }

        /// <summary>
        /// Creates a WFS DescribeFeatureType request URL.
        /// </summary>
        /// <param name="strUrl">The WFS server URL.</param>
        /// <param name="typeNames">The features layer names you want info about.</param>
        /// <param name="version">The WFS version.</param>
        /// <returns></returns>
        public static string CreateDescribeFeaturesRequestUrl(string strUrl, List<string> typeNames, WFSVersion version)
        {
            if (version == WFSVersion.Unknown)
            {
                throw new Exception("WFSVersion is unknown");
            }
            //XXX
            string strVersion = GetWFSVersionStringFromEnum(version);
            var uri = new Uri(strUrl);
            string baseUrl = uri.GetLeftPart(UriPartial.Path);
            var strReq = new StringBuilder(baseUrl);
            strReq.Append("?");
            strReq.AppendFormat("service=wfs&");
            strReq.AppendFormat("request=DescribeFeatureType&");
            strReq.AppendFormat("TypeName=" + string.Join(",",typeNames) + "&");
            strReq.AppendFormat("version=" + strVersion);

            return strReq.ToString();
        }


        #endregion

        #region WFS GetFeature

        /// <summary>
        /// For test purposes, should be moved to GeometryManagerTest
        /// </summary>
        public static FeatureCollection MakefeatureCollection(String strGeojson)
        {
            // Make a Json string to a featue collection type
            FeatureCollection featureCollection =
                JsonConvert.DeserializeObject(strGeojson, typeof (FeatureCollection)) as FeatureCollection;
            return featureCollection;
        }

        /// <summary>
        /// Creates a WFS DescribeFeatureType request URL.
        /// </summary>
        /// <param name="serverUrl">The WFS server URL.</param>
        /// <param name="version">The WFS version.</param>
        /// <param name="bbox">The bounding box given as bottom left and top right corner coordinates.</param>
        /// <param name="typeName">Name of the feature type to describe.</param>
        /// <param name="srsName">The spatial reference system (SRID) in the format of EPSG:XXXX </param>
        /// <param name="parameter"></param>
        /// <param name="parameterValue"></param>
        /// <returns></returns>
        public static string CreateGetFeatureRequestUrl(string serverUrl, WFSVersion version, string bbox, WfsTypeName typeName, string srsName, string parameter, string parameterValue)
        {
            // As for now only polygon features and output format json are supported
            //
            const string outputFormat = "json";
            string strFilter = "";

            if (version == WFSVersion.Unknown)
            {
                throw new Exception("WFSVersion is unknown");
            }

            //The query should specify either typeName, featureId filter(, or a stored query id)
            //
            if (typeName.Namespace.IsEmpty() && parameter.IsEmpty())
            {
                throw new Exception("Missing TypeName and filter in url, wich is not allowed.");
            }

            //The query should not specify both bounding box and filter at the same time
            //
            if (parameter.IsNotEmpty() && bbox.IsNotEmpty())
            {
                throw new Exception("There is a filter and a bounding box defined at the same time: filter and bbox both specified but are mutually exclusive");
                //Todo: Or make the filter with the bounding box
            }

            string strVersion = GetWFSVersionStringFromEnum(version);
            var uriBuilder = new UriBuilder(serverUrl);
            Uri uri = uriBuilder.Uri;
            string baseUrl = uri.GetLeftPart(UriPartial.Path);
            var strReq = new StringBuilder(baseUrl);
            strReq.Append("?");
            strReq.AppendFormat("&service=wfs");
            strReq.AppendFormat("&request=GetFeature");
            strReq.AppendFormat("&version=" + strVersion);
            
            if (parameter.IsNotEmpty())
            {
                strFilter = CreateWFSFilter(parameter, parameterValue);
                strReq.AppendFormat("&filter=" + strFilter);
            }
            if (bbox.IsNotEmpty()) strReq.AppendFormat("&bbox=" + bbox);
            if (typeName.Namespace.IsNotEmpty()) {strReq.AppendFormat("&typeName=" + typeName.Namespace);}
            if (srsName.IsNotEmpty()) {strReq.AppendFormat("&srs=" + srsName);}
            //Todo Hantera Parameter och parameterValue
            //
            strReq.AppendFormat("&outputFormat=" + outputFormat);

            return strReq.ToString();
        }
       
        /// <summary>
        /// Creates a WFS DescribeFeatureType request URL.
        /// </summary>
        /// <param name="requestUrl">The complete url including all parameters except version.</param>
        /// <param name="version">The WFS version.</param>
        /// <returns></returns>
        public static FeatureCollection GetWfsFeatures(string requestUrl, WFSVersion version)
        {
            const string outputFormat = "json";
            var strReq = new StringBuilder();
            strReq.Append(requestUrl);
            strReq.AppendFormat("&outputFormat=" + outputFormat);
            string str = strReq.ToString();
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                string strJson = wc.DownloadString(str);

                // Make a Json string to a featue collection type
                FeatureCollection featureCollection = JsonConvert.DeserializeObject(strJson, typeof(FeatureCollection)) as FeatureCollection;
                return featureCollection;
            }               
        }


        /// <summary>
        /// Gets the WFS features using a HTTP Post request.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        public static FeatureCollection GetWfsFeaturesUsingHttpPost(string requestUrl)
        {
            WfsBoundingBox boundingBox = null;
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(requestUrl);

            // try parse bounding box
            string srsName = nameValueCollection["srsName"];
            string strBBox = nameValueCollection["bbox"];
            if (!string.IsNullOrEmpty(strBBox) && !string.IsNullOrEmpty(srsName))
            {
                string[] bboxItems = strBBox.Split(',');
                if (bboxItems.Length == 4)
                {
                    double minX, minY, maxX, maxY;
                    if (double.TryParse(bboxItems[0], NumberStyles.Number,CultureInfo.InvariantCulture, out minX) && double.TryParse(bboxItems[1], NumberStyles.Number,CultureInfo.InvariantCulture, out minY) &&
                        double.TryParse(bboxItems[2], NumberStyles.Number,CultureInfo.InvariantCulture, out maxX) && double.TryParse(bboxItems[3], NumberStyles.Number,CultureInfo.InvariantCulture, out maxY))
                    {
                        boundingBox = new WfsBoundingBox(minX, minY, maxX, maxY, srsName);
                    }
                }
            }

            return GetWfsFeaturesUsingHttpPost(requestUrl, boundingBox);
        }

        /// <summary>
        /// Gets the WFS features using a HTTP Post request.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <param name="boundingBox">The bounding box.</param>
        public static FeatureCollection GetWfsFeaturesUsingHttpPost(string requestUrl, WfsBoundingBox boundingBox)
        {            
            var uri = new Uri(requestUrl);
            string baseUrl = uri.GetLeftPart(UriPartial.Path);
            string postData = CreateWfsGetFeatureXmlString(requestUrl, boundingBox);
            
            string strJson = MakeHttpPostRequest(baseUrl, postData);
            // Make a Json string to a featue collection type
            FeatureCollection featureCollection = JsonConvert.DeserializeObject(strJson, typeof(FeatureCollection)) as FeatureCollection;
            return featureCollection;
        }


        /// <summary>
        /// Creates the WFS XML string that can be used in a HTTP Post request.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <param name="boundingBox">The bounding box.</param>
        /// <returns>A string containing WFS XML-text.</returns>
        private static string CreateWfsGetFeatureXmlString(string requestUrl, WfsBoundingBox boundingBox)
        {        
            NameValueCollection nameValueCollection;
            StringBuilder sb;
            const string getFeatureStart = "<wfs:GetFeature xmlns:wfs=\"http://www.opengis.net/wfs\" service=\"WFS\" version=\"[Version]\" outputFormat=\"[OutputFormat]\" xsi:schemaLocation=\"http://www.opengis.net/wfs http://schemas.opengis.net/wfs/1.1.0/wfs.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">";
            const string getFeatureEnd = "</wfs:GetFeature>";
            const string queryStart = "<wfs:Query typeName=\"[TypeName]\" srsName=\"[SrsName]\">";
            const string queryEnd = "</wfs:Query>";

            nameValueCollection = HttpUtility.ParseQueryString(requestUrl);
            string version = nameValueCollection["version"];
            string typeName = nameValueCollection["typename"];
            string srsName = nameValueCollection["srsName"];
            string filter = nameValueCollection["filter"];

            // Get geometry name            
            WFSDescribeFeatureType describeFeatureType = GetWFSDescribeFeatureType(requestUrl, typeName);
            string geometryName = describeFeatureType.GeometryField.Name;
            
            string strFilterXml = CreateWfsXmlFilterString(filter, geometryName, boundingBox);
            sb = new StringBuilder();
            sb.Append(getFeatureStart.Replace("[Version]", version).Replace("[OutputFormat]", "json"));
            sb.Append(queryStart.Replace("[TypeName]", typeName).Replace("[SrsName]", srsName));
            sb.Append(strFilterXml);
            sb.Append(queryEnd);
            sb.Append(getFeatureEnd);

            string str = sb.ToString();
            return str;
        }


        /// <summary>
        /// Creates a WFS XML filter string.
        /// </summary>
        /// <param name="filter">The filter string.</param>
        /// <param name="geometryName">Name of the geometry object.</param>
        /// <param name="boundingBox">The bounding box.</param>
        /// <returns>A string containing WFS XML-text.</returns>
        private static string CreateWfsXmlFilterString(string filter, string geometryName, WfsBoundingBox boundingBox)
        {
            FormulaOperation formulaOperation;
            SpatialOperation bboxOperation = null;

            if (string.IsNullOrEmpty(geometryName))
                geometryName = "the_geom";            
            if (boundingBox != null)
            {
                SpatialBoundingBox spatialBoundingBox = new SpatialBoundingBox(boundingBox.MinX, boundingBox.MinY, boundingBox.MaxX, boundingBox.MaxY, boundingBox.SrsName);
                bboxOperation = new SpatialOperation(new SpatialFieldValue(geometryName), spatialBoundingBox, WFSSpatialOperator.InsideBbox);
            }
            
            if (string.IsNullOrEmpty(filter))
            {
                formulaOperation = bboxOperation ?? null;
            }
            else
            {
                WfsFormulaParser parser = new WfsFormulaParser();
                formulaOperation = parser.Parse(filter);
                if (bboxOperation != null)
                {
                    formulaOperation = new BinaryLogicalOperation(formulaOperation, bboxOperation, WFSBinaryLogicalOperator.And);        
                }
            }
            WFSFilter wfsFilter = new WFSFilter { Formula = formulaOperation };
            string strWfsXmlRepresentation = wfsFilter.WfsXmlRepresentation();
            return strWfsXmlRepresentation;
        }
        

        /// <summary>
        /// Executes a HTTP POST request and returns the result as a string.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="postData">The post data.</param>
        /// <returns></returns>
        private static string MakeHttpPostRequest(string url, string postData)
        {
            StreamReader reader = null;
            Stream dataStream = null;
            WebResponse response = null;

            try
            {
                // Create a request using a URL that can receive a post. 
                WebRequest request = WebRequest.Create(url);            
                request.Timeout = (int)TimeSpan.FromMinutes(20).TotalMilliseconds;
                // Set the Method property of the request to POST.
                request.Method = "POST";
                                
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                //request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                Console.WriteLine();
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                reader = new StreamReader(dataStream);
                //reader = new StreamReader(dataStream, Encoding.UTF8); // todo - should we use this line instead of the line above
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
            
                // Display the content.
                return responseFromServer;
            }
            finally
            {
                // Clean up the streams.
                if (reader != null) reader.Close();
                if (dataStream != null) dataStream.Close();
                if (response != null) response.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverUrl">The WFS server URL.</param>
        /// <param name="version">The WFS version.</param>        
        /// <param name="bbox">The bounding box in which the analysyis should be done.</param>
        /// <param name="typeName">The features layer names you want info about.</param>
        /// <param name="srsName">The spatial reference system (SRID) in the format of EPSG:XXXX </param>
        /// <param name="parameter"></param>
        /// <param name="parameterValue"></param>
        /// <returns></returns>
        public static FeatureCollection GetWfsFeatures(string serverUrl, WFSVersion version, string bbox, WfsTypeName typeName, string srsName, string parameter, string parameterValue)
        {
            string requestUrl = "";
            requestUrl = CreateGetFeatureRequestUrl(serverUrl, version, bbox, typeName, srsName, parameter, parameterValue);
            
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                string strJson = wc.DownloadString(requestUrl);
                
                // Make a Json string to a featue collection type
                FeatureCollection featureCollection = JsonConvert.DeserializeObject(strJson, typeof(FeatureCollection)) as FeatureCollection;
                return featureCollection;
            }   
        }

        /// <summary>
        /// Creates a WFS filter.
        /// </summary>
        /// <param name="parameter">
        /// The parameter corresponds to an attribute field in the database layer.
        /// </param>
        /// <param name="parameterValue">
        /// The value to search for in the database attribute field.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string CreateWFSFilter(string parameter, string parameterValue)
        {
            var strReq = new StringBuilder();
            //Todo: Replace åäöÅÄÖ in with proper things
            //Å=, Ä=, Ö=, å=, ä=Ã¤, ö=
            //
            

            strReq.Append("<Filter><PropertyIsEqualTo><PropertyName>");
            strReq.AppendFormat(parameter);
            strReq.AppendFormat("</PropertyName><Literal>");
            strReq.AppendFormat(parameterValue);
            strReq.AppendFormat("</Literal></PropertyIsEqualTo></Filter>");

        //<Filter><PropertyIsEqualTo><PropertyName>SLW:LÃ¤nSKOD</PropertyName><Literal>17</Literal></PropertyIsEqualTo></Filter>


            return strReq.ToString();
        }

        #endregion


        /// <summary>
        /// Makes a GetCapabilities request and a DescribeFeature request
        /// The feature types that we get from DescribeFeature is added to
        /// the WfsCapabilities object.
        /// </summary>
        /// <param name="url">
        /// The WFS Server URL.
        /// </param>
        /// <param name="version">
        /// WFS Version.
        /// </param>
        /// <returns>
        /// The <see cref="WFSCapabilities"/>.
        /// </returns>
        public static WFSCapabilities GetWFSCapabilitiesAndMergeDescribeFeatureTypes(string url, WFSVersion version)
        {
            WFSCapabilities wfsCapabilities = GetWFSCapabilities(url, version);
            Dictionary<string, WFSDescribeFeatureType> dicDescribeFeatureTypes = GetWFSDescribeFeatureTypes(url, wfsCapabilities.FeatureTypes, version);
            //List<WFSDescribeFeatureType> describeFeatureTypes = dicDescribeFeatureTypes.Values.ToList();

            foreach (var featureType in wfsCapabilities.FeatureTypes)
            {
                WFSDescribeFeatureType describeFeatureType;
                if (dicDescribeFeatureTypes.TryGetValue(featureType.Name.FullName, out describeFeatureType))
                {
                    featureType.DescribeFeatureType = describeFeatureType;
                }
            }

            return wfsCapabilities;
        }


        /// <summary>
        /// Makes a GetCapabilities request and a DescribeFeature request
        /// The feature types that we get from DescribeFeature is added to
        /// the WfsCapabilities object.
        /// </summary>
        /// <param name="url">The WFS Server URL.</param>
        /// <returns></returns>
        public static WFSCapabilities GetWFSCapabilitiesAndMergeDescribeFeatureTypes(string url)
        {
            WFSCapabilities wfsCapabilities = GetWFSCapabilities(url);
            Dictionary<string, WFSDescribeFeatureType> dicDescribeFeatureTypes = GetWFSDescribeFeatureTypes(url, wfsCapabilities.FeatureTypes);
            //List<WFSDescribeFeatureType> describeFeatureTypes = dicDescribeFeatureTypes.Values.ToList();

            foreach (var featureType in wfsCapabilities.FeatureTypes)
            {
                WFSDescribeFeatureType describeFeatureType;
                if (dicDescribeFeatureTypes.TryGetValue(featureType.Name.FullName, out describeFeatureType))
                {
                    featureType.DescribeFeatureType = describeFeatureType;
                }
            }

            return wfsCapabilities;
        }


        /// <summary>
        /// Makes a GetCapabilities request and a DescribeFeature request
        /// The feature type that we get from DescribeFeature is added to
        /// the WfsCapabilities object.
        /// </summary>
        /// <param name="url">The WFS Server URL.</param>
        /// <param name="typeName">The feature layer name including namespace.</param>
        /// <param name="version">The WFS version.</param>
        /// <returns></returns>
        public static WFSCapabilities GetWFSCapabilitiesAndMergeDescribeFeatureType(string url, string typeName, WFSVersion version)
        {
            WFSCapabilities wfsCapabilities = GetWFSCapabilities(url, version);
            WFSDescribeFeatureType describeFeatureType = GetWFSDescribeFeatureType(url, typeName, version);
            
            foreach (var featureType in wfsCapabilities.FeatureTypes)
            {
                if (featureType.Name.FullName == describeFeatureType.Name.FullName)
                {
                    featureType.DescribeFeatureType = describeFeatureType;
                }
            }

            return wfsCapabilities;
        }

        /// <summary>
        /// Makes a GetCapabilities request and a DescribeFeature request
        /// The feature type that we get from DescribeFeature is added to
        /// the WfsCapabilities object.        
        /// </summary>
        /// <param name="url">The WFS Server URL.</param>
        /// <param name="typeName">The feature layer name including namespace.</param>
        /// <returns></returns>
        public static WFSCapabilities GetWFSCapabilitiesAndMergeDescribeFeatureType(string url, string typeName)
        {
            WFSCapabilities wfsCapabilities = GetWFSCapabilities(url);
            WFSDescribeFeatureType describeFeatureType = GetWFSDescribeFeatureType(url, typeName);

            foreach (var featureType in wfsCapabilities.FeatureTypes)
            {
                if (featureType.Name.FullName == describeFeatureType.Name.FullName)
                {
                    featureType.DescribeFeatureType = describeFeatureType;
                }
            }

            return wfsCapabilities;           
        }

     
    }
}

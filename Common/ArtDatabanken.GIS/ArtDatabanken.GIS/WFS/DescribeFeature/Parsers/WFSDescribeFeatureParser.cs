using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using ArtDatabanken.GIS.WFS.Capabilities;

namespace ArtDatabanken.GIS.WFS.DescribeFeature.Parsers
{


    /// <summary>
    /// This class is used to pars WFS DescribeFeature requests XML result
    /// </summary>
    public class WFSDescribeFeatureParser
    {

        /// <summary>
        /// Parses WFS response XML into one WFSDescribeFeature
        /// </summary>
        /// <param name="strXml">WFS XML response string.</param>
        /// <returns></returns>
        public WFSDescribeFeatureType ParseDescribeFeatureType(string strXml)
        {
            Dictionary<string, WFSDescribeFeatureType> dicDescribeFeatures = ParseDescribeFeatureTypes(strXml);
            return dicDescribeFeatures.Values.FirstOrDefault();
        }



        /// <summary>
        /// Parses WFS response XML into a Dictionary where the key is the full name of the feature
        /// and the value is its corresponding WFSDescribeFeature
        /// </summary>
        /// <param name="strXml">WFS response XML.</param>
        /// <returns></returns>
        public Dictionary<string,WFSDescribeFeatureType> ParseDescribeFeatureTypes(string strXml)
        {
            var dicDescribeFeatures = new Dictionary<string, WFSDescribeFeatureType>();
            try
            {
                var doc = new XmlDocument();
                doc.XmlResolver = null;
                doc.LoadXml(strXml);
                if (doc.DocumentElement == null)
                {
                    return dicDescribeFeatures;
                }                

                var nsmgr = new XmlNamespaceManager(doc.NameTable);
                foreach (XmlNode nodes in doc.DocumentElement.Attributes)
                {
                    if (nodes.Prefix == "xmlns")
                        nsmgr.AddNamespace(nodes.Name, nodes.NamespaceURI);
                }                    
                                    
                XmlNodeList nodeList = doc.DocumentElement.GetElementsByTagName("xsd:complexType");
                if (nodeList.Count == 0)
                {
                    nodeList = doc.DocumentElement.GetElementsByTagName("complexType");
                }
                foreach (XmlElement node in nodeList)
                {
                    XmlElement xname = node.NextSibling as XmlElement;
                    WFSDescribeFeatureType wfsDescribeFeatureType = ParseDescribeFeature(node, xname);
                    if (!dicDescribeFeatures.ContainsKey(wfsDescribeFeatureType.Name.FullName))
                        dicDescribeFeatures.Add(wfsDescribeFeatureType.Name.FullName, wfsDescribeFeatureType);                    
                }                

            }
            catch (Exception)
            {
                return dicDescribeFeatures;
            }
            return dicDescribeFeatures;
        }



        /// <summary>
        /// Parses XML objects into a WFSDescribeFeature object
        /// </summary>
        /// <param name="xtype">Describes the feature fields.</param>
        /// <param name="xname">Describes the feature name.</param>
        /// <returns></returns>
        private WFSDescribeFeatureType ParseDescribeFeature(XmlElement xtype, XmlElement xname)
        {
            if (xtype == null || xname == null)
            {
                return null;
            }

            var wfsDescribeFeature = new WFSDescribeFeatureType();
            wfsDescribeFeature.Fields = new List<Field>();
            try
            {
                var xcomplexContent = xtype["xsd:complexContent"] ?? xtype["complexContent"];
                var xextension = xcomplexContent["xsd:extension"] == null ? xcomplexContent["extension"] : xcomplexContent["xsd:extension"];
                var xsequence = xextension["xsd:sequence"] == null ? xextension["sequence"] : xextension["xsd:sequence"];

                if (xsequence != null)
                {
                    foreach (XmlNode ele in xsequence)
                    {
                        var field = new Field();
                        field.Name = ele.Attributes["name"].Value.ToString(CultureInfo.InvariantCulture);
                        if (ele.Attributes["type"] == null)
                        {
                            field.DataType = "Object";
                        }
                        else
                        {
                            field.DataType = ele.Attributes["type"].Value.ToString(CultureInfo.InvariantCulture);                            
                        }
                        wfsDescribeFeature.Fields.Add(field);
                    }
                }

                // name and namespace
                string name = xname.Attributes["name"].Value.ToString();
                string typeName = xname.Attributes["type"].Value.ToString();                
                wfsDescribeFeature.Name = new WfsTypeName(typeName);
                wfsDescribeFeature.Name.Name = name;
                foreach (Field field in wfsDescribeFeature.Fields)
                {
                    if (field.DataType.StartsWith("gml:"))
                    {                        
                        wfsDescribeFeature.GeometryField = field;

                        if (field.DataType == "gml:PointPropertyType" || field.DataType == "gml:MultiPointPropertyType" || field.DataType == "gml:MultiPointType")
                        {                            
                            wfsDescribeFeature.GeometryType = GeometryType.Point;
                        }
                        else if (field.DataType == "gml:MultiLineStringPropertyType" || field.DataType == "gml:MultiLineStringType")
                        {
                            wfsDescribeFeature.GeometryType = GeometryType.Line;
                        }
                        else if (field.DataType == "gml:MultiSurfacePropertyType" || field.DataType == "gml:MultiSurfaceType" || field.DataType == "gml:MultiPolygonPropertyType" || field.DataType == "gml:MultiPolygonType")
                        {
                            wfsDescribeFeature.GeometryType = GeometryType.Polygon;
                        }
                        else
                        {
                            wfsDescribeFeature.GeometryType = GeometryType.Geometry;
                        }
                    }
                }

                return wfsDescribeFeature;
            }
            catch (Exception)
            {

                return null;
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using WFS_Schemas;

namespace ArtDatabanken.GIS.WFS.Capabilities.Parsers
{
    /// <summary>
    /// TBD ?!?
    /// </summary>
    public class WFSCapabilitiesParserVer100
    {

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public WFSCapabilities Parse(string xml)
        {
            WFS_Schemas.WFS_CapabilitiesType readWFSCaps = WFS_Schemas.WFS_CapabilitiesType.Deserialize(xml);
            var wfsCapabilities = new WFSCapabilities();

            wfsCapabilities.Version = readWFSCaps.version; // Parse version
            ParseService(readWFSCaps, wfsCapabilities);
            ParseCapability(readWFSCaps, wfsCapabilities);
            ParseFeatureTypes(readWFSCaps, wfsCapabilities);
            ParseXml(xml, wfsCapabilities);

            return wfsCapabilities;
        }


        private void ParseXml(string strXml, WFSCapabilities wfsCapabilities)
        {            
            XPathNavigator nav;
            XPathDocument docNav;
            XPathNodeIterator NodeIter;
          //  String strExpression;

            try
            {
                using (StringReader stream = new StringReader(strXml))
                {
                    docNav = new XPathDocument(stream);
                    nav = docNav.CreateNavigator();

                    var ns = new XmlNamespaceManager(nav.NameTable);                
                    ns.AddNamespace("wfs", "http://www.opengis.net/wfs");
                    ns.AddNamespace("ows", "http://www.opengis.net/ows");
                    ns.AddNamespace("xlink", "http://www.w3.org/1999/xlink");

                    XPathExpression expr = nav.Compile("/wfs:WFS_Capabilities/wfs:Capability/wfs:Request/wfs:GetFeature/wfs:ResultFormat/*");
                    expr.SetContext(ns);
                    NodeIter = nav.Select(expr);
                                
                    var formats = new List<string>();
                    while (NodeIter.MoveNext())
                    {
                        if (!string.IsNullOrEmpty(NodeIter.Current.Name))
                        {
                            formats.Add(NodeIter.Current.Name);
                        }
                    }
                    wfsCapabilities.Capability.Requests.GetFeaturesRequest.Formats = formats;
                }
            }
            catch (Exception)
            {
            }
        }

        private void ParseService(WFS_CapabilitiesType readWFSCaps, WFSCapabilities wfsCapabilities)
        {
            wfsCapabilities.Service = new WfsService();
            wfsCapabilities.Service.Title = readWFSCaps.Service.Title;
            wfsCapabilities.Service.Abstract = readWFSCaps.Service.Abstract;
            wfsCapabilities.Service.Name = readWFSCaps.Service.Name;
            string[] keywords = readWFSCaps.Service.Keywords.Split(',');
            for (int i = 0; i < keywords.Length; i++)
            {
                keywords[i] = keywords[i].Trim();
            }
            wfsCapabilities.Service.Keywords = new List<string>(keywords);
            wfsCapabilities.Service.Fees = readWFSCaps.Service.Fees;
            // osäker på om listan ska splittas med ',' eller hur den är uppbyggd.
            string[] accessConstraints = readWFSCaps.Service.AccessConstraints.Split(',');
            for (int i = 0; i < accessConstraints.Length; i++)
            {
                accessConstraints[i] = accessConstraints[i].Trim();
            }
            wfsCapabilities.Service.AccessConstraints = new List<string>(accessConstraints);
        }


        private void ParseFeatureTypes(WFS_CapabilitiesType readWFSCaps, WFSCapabilities wfsCapabilities)
        {
            wfsCapabilities.FeatureTypes = new List<WfsFeatureType>();

            foreach (FeatureTypeType readFeatureType in readWFSCaps.FeatureTypeList.FeatureType)
            {
                WfsFeatureType featureType = new WfsFeatureType();
                featureType.Abstract = readFeatureType.Abstract;

                featureType.Keywords = new List<string>();
                // osäker på om listan ska splittas med ',' eller hur den är uppbyggd.
                string[] keywords = readFeatureType.Keywords.Split(',');
                for (int i = 0; i < keywords.Length; i++)
                {
                    keywords[i] = keywords[i].Trim();
                }
                featureType.Keywords = new List<string>(keywords);
                
                if (readFeatureType.MetadataURL.IsNotEmpty())
                {
                    featureType.MetadataURL = readFeatureType.MetadataURL[0].Value;
                }

                featureType.Name = new WfsTypeName(readFeatureType.Name);                 
                featureType.Title = readFeatureType.Title;
                featureType.SRS = readFeatureType.SRS;

                featureType.BoundingBox = new WfsBoundingBox();
                if (readFeatureType.LatLongBoundingBox.IsNotEmpty())
                {
                    var readBBox = readFeatureType.LatLongBoundingBox[0];                    
                    featureType.BoundingBox.LowerCorner = readBBox.minx + " " + readBBox.miny;
                    featureType.BoundingBox.UpperCorner = readBBox.maxx + " " + readBBox.maxy;
                }
                wfsCapabilities.FeatureTypes.Add(featureType);
            }

        }


        private void ParseCapability(WFS_Schemas.WFS_CapabilitiesType readWFSCaps, WFSCapabilities wfsCapabilities)
        {
            // Parse GetFeatureRequest
            wfsCapabilities.Capability = new WfsCapability();
            wfsCapabilities.Capability.Requests = new WfsRequests();
            wfsCapabilities.Capability.Requests.GetFeaturesRequest = new WfsGetFeaturesRequest();
            wfsCapabilities.Capability.Requests.DescribeFeatureTypeRequest = new WfsDescribeFeatureTypeRequest();

            for (int i = 0; i < readWFSCaps.Capability.Request.ItemsElementName.Length; i++)
            {
                ItemsChoiceType operation = readWFSCaps.Capability.Request.ItemsElementName[i];
                
                if (operation == ItemsChoiceType.GetFeature)
                {
                    GetFeatureTypeType getFeatureType = (GetFeatureTypeType) readWFSCaps.Capability.Request.Items[i];
                    // todo - fel i schema för format???

                    foreach (DCPTypeType dcpType in getFeatureType.DCPType)
                    {
                        if (dcpType.HTTP.IsNotEmpty())
                        {
                            if (dcpType.HTTP[0].GetType() == typeof(GetType))
                            {
                                wfsCapabilities.Capability.Requests.GetFeaturesRequest.GetUrl = ((GetType) dcpType.HTTP[0]).onlineResource;                                
                            }
                            else if (dcpType.HTTP[0].GetType() == typeof(PostType))
                            {
                                wfsCapabilities.Capability.Requests.GetFeaturesRequest.PostUrl = ((PostType)dcpType.HTTP[0]).onlineResource;
                            }
   
                        }                        
                    }
                    
                }
                else if (operation == ItemsChoiceType.DescribeFeatureType)
                {
                    DescribeFeatureTypeType describeFeatureType = (DescribeFeatureTypeType)readWFSCaps.Capability.Request.Items[i];

                    foreach (DCPTypeType dcpType in describeFeatureType.DCPType)
                    {
                        if (dcpType.HTTP.IsNotEmpty())
                        {
                            if (dcpType.HTTP[0].GetType() == typeof(GetType))
                            {
                                wfsCapabilities.Capability.Requests.DescribeFeatureTypeRequest.GetUrl = ((GetType)dcpType.HTTP[0]).onlineResource;
                            }
                            else if (dcpType.HTTP[0].GetType() == typeof(PostType))
                            {
                                wfsCapabilities.Capability.Requests.DescribeFeatureTypeRequest.PostUrl = ((PostType)dcpType.HTTP[0]).onlineResource;
                            }
                        }
                    }

                }
            }


        }




    }
}

using ArtDatabanken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WFS_Schemas.Ver1_1_0;

namespace ArtDatabanken.GIS.WFS.Capabilities.Parsers
{
    /// <summary>
    /// WFSCapabilitiesParserVer110
    /// </summary>
    public class WFSCapabilitiesParserVer110
    {

        /// <summary>
        /// WFSCapabilities
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public WFSCapabilities Parse(string xml)
        {
            WFS_Schemas.Ver1_1_0.WFS_CapabilitiesType readWFSCaps = WFS_CapabilitiesType.Deserialize(xml);
            var wfsCapabilities = new WFSCapabilities();


            // Parse version
            wfsCapabilities.Version = readWFSCaps.version;


            // Parse service
            wfsCapabilities.Service = new WfsService();
            wfsCapabilities.Service.Title = readWFSCaps.ServiceIdentification.Title;
            wfsCapabilities.Service.Abstract = readWFSCaps.ServiceIdentification.Abstract;
            wfsCapabilities.Service.Name = readWFSCaps.ServiceIdentification.ServiceType.Value;
            wfsCapabilities.Service.Keywords = new List<string>();
            foreach (KeywordsType keywords in readWFSCaps.ServiceIdentification.Keywords)
            {
                wfsCapabilities.Service.Keywords.AddRange(keywords.Keyword);                
            }            
            wfsCapabilities.Service.Fees = readWFSCaps.ServiceIdentification.Fees;
            wfsCapabilities.Service.AccessConstraints = new List<string>(readWFSCaps.ServiceIdentification.AccessConstraints);
            




            // Parse capability
            ParseCapability(readWFSCaps, wfsCapabilities);

            ParseFeatureTypes(readWFSCaps, wfsCapabilities);

            return wfsCapabilities;
        }


        private void ParseFeatureTypes(WFS_CapabilitiesType readWFSCaps, WFSCapabilities wfsCapabilities)
        {
            wfsCapabilities.FeatureTypes = new List<WfsFeatureType>();

            foreach (FeatureTypeType readFeatureType in readWFSCaps.FeatureTypeList.FeatureType)
            {
                WfsFeatureType featureType = new WfsFeatureType();
                featureType.Abstract = readFeatureType.Abstract;
                                             
                featureType.Keywords = new List<string>();
                foreach (KeywordsType keywordsType in readFeatureType.Keywords)
                {
                    featureType.Keywords.AddRange(keywordsType.Keyword);
                }
                if (readFeatureType.MetadataURL.IsNotEmpty())
                {
                    featureType.MetadataURL = readFeatureType.MetadataURL[0].Value;
                }

                featureType.Name = new WfsTypeName(readFeatureType.Name);                
                featureType.Title = readFeatureType.Title;                
                for (int i = 0; i < readFeatureType.ItemsElementName.Length; i++)
                {
                    var itemsChoiceType = readFeatureType.ItemsElementName[i];
                    if (itemsChoiceType == ItemsChoiceType14.DefaultSRS)
                    {
                        featureType.SRS = readFeatureType.Items[i].ToString();
                    }
                }

                featureType.BoundingBox = new WfsBoundingBox();
                if (readFeatureType.WGS84BoundingBox.IsNotEmpty())
                {
                    var readBBox = readFeatureType.WGS84BoundingBox[0];
                    featureType.BoundingBox.CRS = readBBox.crs;
                    featureType.BoundingBox.LowerCorner = readBBox.LowerCorner;
                    featureType.BoundingBox.UpperCorner = readBBox.UpperCorner;
                    featureType.BoundingBox.Dimensions = readBBox.dimensions;
                }    
                wfsCapabilities.FeatureTypes.Add(featureType);
            }

        }



        private void ParseCapability(WFS_CapabilitiesType readWFSCaps, WFSCapabilities wfsCapabilities)
        {
            // Parse GetFeatureRequest
            wfsCapabilities.Capability = new WfsCapability();
            wfsCapabilities.Capability.Requests = new WfsRequests();
            wfsCapabilities.Capability.Requests.GetFeaturesRequest = new WfsGetFeaturesRequest();
            wfsCapabilities.Capability.Requests.DescribeFeatureTypeRequest = new WfsDescribeFeatureTypeRequest();
            
            foreach (Operation operation in readWFSCaps.OperationsMetadata.Operation)
            {
                // Requests.GetFeaturesRequest
                if (operation.name.ToLower() == "getfeature")
                {
                    foreach (var parameter in operation.Parameter)
                    {
                        // Formats
                        if (parameter.name.ToLower() == "outputformat")
                        {
                            wfsCapabilities.Capability.Requests.GetFeaturesRequest.Formats = new List<string>(parameter.Value);
                        }
                        // ResultType
                        else if (parameter.name.ToLower() == "resulttype")
                        {
                            wfsCapabilities.Capability.Requests.GetFeaturesRequest.ResultType = new List<string>(parameter.Value);
                        }
                    }

                    
                    if (operation.DCP.IsNotEmpty() && operation.DCP[0].Item != null)
                    {
                        HTTP item = operation.DCP[0].Item;
                        for (int i=0; i < item.ItemsElementName.Length; i++)
                        {                        
                            // GetUrl
                            if (item.ItemsElementName[i] == ItemsChoiceType15.Get)
                            {
                                wfsCapabilities.Capability.Requests.GetFeaturesRequest.GetUrl = item.Items[i].href + "?request=GetFeature";
                            }
                            // PostUrl
                            else if (item.ItemsElementName[i] == ItemsChoiceType15.Post)
                            {
                                wfsCapabilities.Capability.Requests.GetFeaturesRequest.PostUrl = item.Items[i].href;
                            }
                        }
                        

                    }
                }

                // Requests.DescribeFeatureTypeRequest
                else if (operation.name.ToLower() == "describefeaturetype")
                {
                    foreach (var parameter in operation.Parameter)
                    {
                        // Formats
                        if (parameter.name.ToLower() == "outputformat")
                        {
                            wfsCapabilities.Capability.Requests.DescribeFeatureTypeRequest.Formats = new List<string>(parameter.Value);
                        }
                    }
                    
                    if (operation.DCP.IsNotEmpty() && operation.DCP[0].Item != null)
                    {
                        HTTP item = operation.DCP[0].Item;
                        for (int i = 0; i < item.ItemsElementName.Length; i++)
                        {
                            // GetUrl
                            if (item.ItemsElementName[i] == ItemsChoiceType15.Get)
                            {
                                wfsCapabilities.Capability.Requests.DescribeFeatureTypeRequest.GetUrl = item.Items[i].href + "?request=DescribeFeatureType";
                            }
                            // PostUrl
                            else if (item.ItemsElementName[i] == ItemsChoiceType15.Post)
                            {
                                wfsCapabilities.Capability.Requests.DescribeFeatureTypeRequest.PostUrl = item.Items[i].href;
                            }
                        }
                    }
                }
            }
            
        }
    }
}

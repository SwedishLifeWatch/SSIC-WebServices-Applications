/// <reference path="../jquery-2.1.0-vsdoc.js" />
/// <reference path="../OpenLayers/OpenLayers.js" />
/// <reference path="AnalysisPortal.Resources-vsdoc.js" />
/// <reference path="../extjs-4.2.1/ext-all.js" />
/// <reference path="AnalysisPortal.GIS.js" />
/// <reference path="AnalysisPortal.WFS.Formula.js" />

/* ===========================================================
 * AnalysisPortal.WFS.js v1.0.0
 * =========================================================== 
 * Copyright 2012 
 *
 * =========================================================== */


var AnalysisPortal = AnalysisPortal || {};
AnalysisPortal.WFS.Base = {};
(function (context) {
    /*
     * Public properties are written as: context.VariableName =
     * Private properties are written as: var variableName =  
     * Public functions are written as: context.functionName = function(args) {
     * Private functions are written as: function functionName(args) {
     */    

     context.wfsVersion = "1.1.0";
    


    context.createWFSProtocol = function(wfsVersion, serverUrl, featurePrefix, featureType, geometryName, projection) {
        /// <summary>
        /// Creates an OpenLayers WFS protocol that can be used to make requests.
        /// </summary>        
        var protocol = new OpenLayers.Protocol.WFS({
            version: wfsVersion, //"1.1.0"
            url: serverUrl, //"http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs"
            featurePrefix: featurePrefix, //SLW
            featureType: featureType, 
            //featureNS: featureNS, // "www.slw.se"
            geometryName: geometryName, //"the_geom"
            srsName: projection, //"EPSG:4326"
            maxFeatures: 1000, //10, 
            //defaultFilter: filt,                
            outputFormat: "json" //, //"GML2", "GML3"
        //schema: "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&request=DescribeFeatureType&version=1.0.0&typename=SLW:MapOfSwedishCounties"                
        });

        return protocol;
    };
 
   
    context.createWFSGetHitsProtocolFromProtocol = function(protocol) {
        /// <summary>
        /// Creates an OpenLayers WFS protocol that can be used to make hits requests (total count).
        /// Hits requests cannot be made in IE if "json" is used as outputFormat.
        /// </summary>    
        /// <param name="protocol" type="Object">The protocol to read info from.</param>        
        /// <returns type="Object">A new protocol.</returns>

        var getHitsProtocol = new OpenLayers.Protocol.WFS({
            version: protocol.version,
            url: protocol.url,
            featurePrefix: protocol.featurePrefix,
            featureType: protocol.featureType,
            geometryName: protocol.geometryName,
            srsName: protocol.srsName,
            maxFeatures: protocol.maxFeatures                
        //outputFormat: "GML3"            
        });
        return getHitsProtocol;
    };


    context.makeWFSGetFeaturesRequest = function(protocol, filter, maxNrFeatures, bbox, callback) {
    	/// <summary>
    	/// 
    	/// </summary>
    	/// <param name="map"></param>
    	/// <param name="layer">the layer which the polygons will be added to</param>
    	/// <param name="protocol"></param>
    	/// <param name="filter"></param>
    	/// <param name="maxNrFeatures"></param>
                        
        protocol.defaultFilter = filter;        
        var bboxFilter = null;
        if (bbox) {
            bboxFilter = new OpenLayers.Filter.Spatial({
                type: OpenLayers.Filter.Spatial.BBOX,
                value: bbox,
                projection: protocol.srsName.projCode
            });
        } else {
            bboxFilter = new OpenLayers.Filter.Spatial({
                type: OpenLayers.Filter.Spatial.BBOX,
                value: new OpenLayers.Bounds(-180, -90, 180, 90),
                projection: "EPSG:4326"
            });
        }
        

        var response = protocol.read({            
            filter: bboxFilter,
            maxFeatures: maxNrFeatures,
            callback: function (resp) {
                try {
                    var count = 0;
                    if (resp.error) {
                        console.log('error in makeWFSGetFeaturesRequest()');
                        return -1;
                    }
                    if (resp.features) {                        
                        count = resp.features.length;
                    }
                    else {
                        count = 0;
                    }
                    if (callback && typeof(callback) === "function") {
                        callback(resp.features, count);
                    }
                } catch(err) {
                    console.log(err);                
                }
            }
        });

    };

  

    context.makeWFSGetFeaturesTotalCountRequest = function(map, protocol, callback) {        
        var getHitsProtocol = context.createWFSGetHitsProtocolFromProtocol(protocol);
        
        // Get the number of features
        getHitsProtocol.read({
            readOptions: {
                output: "object"
            },
            resultType: "hits",
            maxFeatures: null,
            callback: function (resp) {
                try {
                    if (resp.error) {
                        console.log('error');
                        return -1;
                    }
                    var count = resp.priv.responseXML.documentElement.getAttribute("numberOfFeatures");
                    if (callback && typeof(callback) === "function") {
                        callback(count);
                    }                    
                    return count;
                } catch(err) {
                    console.log(err);
                    return -1;
                }
            }
        });
    };
    
    
    function isStringNullOrEmpty(str) {
        return (!str || 0 === str.length);
    }

    context.createWFSFilter = function(strFilter, wfsVersion) {
        if (isStringNullOrEmpty(strFilter)) {
            return null;
        }
        
        var parser = null;
        if (wfsVersion == "1.0.0") {
            parser = new OpenLayers.Format.Filter.v1_0_0();
        } else if (wfsVersion == "1.1.0") {
            parser = new OpenLayers.Format.Filter.v1_1_0();
        }
        var xml = new OpenLayers.Format.XML();
        var x = xml.read(strFilter).documentElement;
        var filter = parser.read(x);
        return filter;
    };


    context.getWFSFilterString = function(filter) {
        var filterFormat = new OpenLayers.Format.Filter({ version: wfsVersion });
        var xml = new OpenLayers.Format.XML();
        var filterParam = xml.write(filterFormat.write(filter));
        return filterParam;
    };    


    context.parseFilterFromXmlString = function(strXml) {
    	/// <summary>
    	/// Parses strings of the following kind:
    	/// '<Filter><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>5</Literal></PropertyIsEqualTo></Filter>'
    	/// and creates an AnalysisPortal.WFS.Formula.WFSFilter object
    	/// </summary>
    	/// <param name="strXml">filter string in xml format</param>
    	/// <returns type="">An AnalysisPortal.WFS.Formula.WFSFilter object</returns>

        try {
            if (AnalysisPortal.isStringNullOrEmpty(strXml)) {
                return new AnalysisPortal.WFS.Formula.WFSFilter();
            }
            var xml = $.parseXML(strXml);
            if (!xml)
                return null;
            if (xml.documentElement.childNodes.length == 0)
                return null;
            var formula = parseNode(xml.documentElement.childNodes[0]);
            var wfsFilter = new AnalysisPortal.WFS.Formula.WFSFilter();
            wfsFilter.Formula = formula;
            return wfsFilter;
        } catch(err) {
            return new AnalysisPortal.WFS.Formula.WFSFilter();
        }
    };


    function parseNode(node) {    	
        if (AnalysisPortal.WFS.Formula.WFSFilterUtils.IsLogicalOperator(node.nodeName)) {
            var formula = parseLogicalOperation(node);
            return formula;
        }
        else if (AnalysisPortal.WFS.Formula.WFSFilterUtils.IsComparisionOperator(node.nodeName)) {
            var operation = parseComparisionOperation(node);                    
            return operation;
        }
        else {
            console.log('error in parseNode(). Could not parse: ' + node.nodeName);
            return null;
        }        
    }



    function parseComparisionOperation(node) {
        var leftOperandNode = node.childNodes[0];
        var rightOperandNode = node.childNodes[1];

        var leftOperand = new AnalysisPortal.WFS.Formula.FieldValue($(leftOperandNode).text());
        var rightOperand = null;
        if (rightOperandNode.nodeName.toLowerCase() == 'PropertyName'.toLowerCase()) {                        
            rightOperand = new AnalysisPortal.WFS.Formula.FieldValue($(rightOperandNode).text());
        }
        else if (rightOperandNode.nodeName.toLowerCase() == 'Literal'.toLowerCase()) {                        
            rightOperand = new AnalysisPortal.WFS.Formula.ConstantValue($(rightOperandNode).text());
        }
                    
        var comparisionOperator = AnalysisPortal.WFS.Formula.WFSFilterUtils.GetComparisionOperator(node.nodeName);
        var operation;
        if (comparisionOperator == AnalysisPortal.WFS.Formula.WFSComparisionOperator.IsNull) {
            operation = new AnalysisPortal.WFS.Formula.UnaryComparisionOperation(
            leftOperand,
            comparisionOperator);
        } else {
            operation = new AnalysisPortal.WFS.Formula.BinaryComparisionOperation(
                leftOperand,
                rightOperand,
                comparisionOperator);
        }
        return operation;
    }

    function parseLogicalOperation(node) {
        var logicalOperator = AnalysisPortal.WFS.Formula.WFSFilterUtils.GetLogicalOperator(node.nodeName);
        var formula = null;
        if (logicalOperator == AnalysisPortal.WFS.Formula.WFSLogicalOperator.Not) {
            formula = new AnalysisPortal.WFS.Formula.UnaryLogicalOperation(parseNode(node.childNodes[0]), AnalysisPortal.WFS.Formula.WFSUnaryLogicalOperator.Not);            
        }
        else {
            formula = new AnalysisPortal.WFS.Formula.BinaryLogicalOperation(parseNode(node.childNodes[0]), parseNode(node.childNodes[1]), logicalOperator);
        }
        return formula;
    }

    
})(AnalysisPortal.WFS.Base);    
    
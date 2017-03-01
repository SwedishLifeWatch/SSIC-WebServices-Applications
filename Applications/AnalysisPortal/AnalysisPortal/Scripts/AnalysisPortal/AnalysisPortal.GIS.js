/// <reference path="../jquery-2.1.0-vsdoc.js" />
/// <reference path="../OpenLayers/OpenLayers.js" />
/// <reference path="AnalysisPortal.Resources-vsdoc.js" />
/// <reference path="../extjs-4.2.1/ext-all.js" />
/* ===========================================================
 * AnalysisPortal.GIS.js v1.0.0
 * ===========================================================
 * Copyright 2012 
 *
 * =========================================================== */

// EPSG:3021 = RT90 2.5 gon V 
Proj4js.defs["EPSG:3021"] = "+proj=tmerc +lat_0=0 +lon_0=15.80827777777778 +k=1 +x_0=1500000 +y_0=0 +ellps=bessel +units=m +no_defs";
// EPSG:3006 = SWEREF99 TM 
Proj4js.defs["EPSG:3006"] = "+proj=utm +zone=33 +ellps=GRS80 +towgs84=0,0,0,0,0,0,0 +units=m +no_defs";

var BaseMaps = {
    GoogleTerrain: "GoogleTerrain",
    GoogleStreets: "GoogleStreets",
    GoogleHybrid: "GoogleHybrid",    
    TopoWebMap: "TopoWebMap",
    Lanskarta: "Lanskarta"
};

var VectorLayers = {
    Observations: "Observations",
    ObservationAccuracy: "ObservationAccuracy",
    EditLayer: "EditLayer",
    SpatialFilter: "SpatialFilter",    
    WFS: "WFS",
    WFSUserDefined: "WFSUserDefined",
    GridLayer: "GridLayer",
    EOOLayerConvexHull: "EOOLayerConvexHull",
    EOOLayerConcaveHull: "EOOLayerConcaveHull"
};

var SystemLayerIds = {
    EooConvexHull: -100,
    EooConcaveHull: -101,
    Observations: -102,
    Precision: -103,
    SpeciesRichnessGridLayer: -1,
    SpeciesObservationGridMap: -2,
    SpeciesObservationClusterPointMapLayer: -3
}

var LayerInitVisibilityState = {
    NotSet: 0,
    AlwaysVisible: 1,
    AlwaysHidden: 2
}

var colorIndex = 0;

var swedenExtentWGS84 = [10.321655273437257, 54.321984300162164, 24.49401855468613, 69.4916528710284];
var swedenExtentGoogleMercator = [1149001.4090228, 7231373.6474714, 2726661.6726091, 10905242.974458];         
var wfsColors = new Array("#99CCFF", "#CCFFCC", "#FFFF99", "#CC99CC", "#CCFFFF",
                          "#FF9999", "#FF9666", "#CCCCCC", "#CCCCFF", "#99CC99",
                          "#FFCCCC", "#CC99FF", "#FFCC99", "99CCCC#", "#9999FF",
                          "#FFCC66", "#CC9999", "#99FFCC", "#66CCFF", "#CCCCFF");

var AnalysisPortal = AnalysisPortal || {};
AnalysisPortal.GIS = {};
(function (context) {
    /*
     * Public properties are written as: context.VariableName =
     * Private properties are written as: var variableName =  
     * Public functions are written as: context.functionName = function(args) {
     * Private functions are written as: function functionName(args) {
     */
    
    // Private variables
    var getObservationsRequest = null; // ajax request object
    var oldGeometry = null;
    var dragDropSort = null;
    
    //alpha values used for concave hull
    var defaultAlphaValue = 200;

    var cookieName = null;
    var aooEooData = {};

    var histograms = { }

    // Public properties    
    context.GetFeatureDataByAjax = false;    
    context.CurrentCoordinateSystem = "EPSG:900913"; // Google Mercator
    //context.CurrentCoordinateSystem = "EPSG:3006"; // SWEREF 99
    //context.CurrentCoordinateSystem = "EPSG:4326"; // WGS84
    //context.CurrentCoordinateSystem = "EPSG:3021"; // RT90
    context.GoogleMercatorProjection = new OpenLayers.Projection('EPSG:900913');
    context.RenderImageMode = false;    

    context.CoordinateSystems = [];
    context.CoordinateSystems["EPSG:900913"] = {
        name: "Google Mercator",
        maxExtentWgs84: [-180.0000, -90.0000, 180.0000, 90.0000],
        maxExtentProjected: [-20037508, -20037508, 20037508, 20037508.34],
        maxResolution: 156543.0339,
        units: "m"
    };    
    context.CoordinateSystems["EPSG:4326"] = {
        name: "WGS 84",
        maxExtentWgs84: [-180.0000, -90.0000, 180.0000, 90.0000],
        maxExtentProjected: [-180.0000, -90.0000, 180.0000, 90.0000],
        maxResolution: 1.40625,
        units: "degrees"
    };    
    context.CoordinateSystems["EPSG:3006"] = {
        name: "SWEREF 99",
        maxExtentWgs84: [10.5700, 55.2000, 24.1800, 69.1000],
        //maxExtentProjected: [218128.7031, 6126002.9379, 1083427.2970, 7692850.9468], //enligt spatial reference
        maxExtentProjected: [-1550660, 4701860, 2250720, 8503400], // enligt topowebbkartan
        maxResolution: "auto",
        units: "m"
    };
    context.CoordinateSystems["EPSG:3021"] = {
        name: "RT90",
        //maxExtentWgs84: [14.0900, 56.0000, 16.9400, 68.0000],
        maxExtentWgs84: [10.5700, 55.2000, 24.1800, 69.1000],
        //maxExtentProjected: [1392811.0743, 6208496.7665, 1570600.8906, 7546077.6984],
        //maxExtentProjected: [1166653.6161, 6131388.6471, 2032341.6763, 7690505.5552],
        //maxExtentProjected: [500000.6161, 5541965.817, 2032341.6763, 7690505.5552],
        maxExtentProjected: [-1300999.642808768, 4686699.978230515, 3612753.9250470437, 9802410.54590885], // enligt topowebbkartan transformation från Sweref99 till RT90       
        maxResolution: "auto",
        units: "m"
    };



    //***************
    // Functions
    //***************            

    context.initMap = function(options) {
        /// <summary>
        /// Initiates a new OpenLayer map in a div
        /// </summary>
        /// <param name="divId" type="Object">Name of the div where the map will be placed on the page</param>
        /// <param name="panelDivId" type="Object"></param>
        /// <param name="mapOption" type="Object"></param>
        /// <returns type="Object">Reference to the new OpenLayer map</returns>

        var settings = $.extend(true,
        {
            divId: 'mapDiv',
            panelDivId: 'mapNavDiv',
            toolsOptions: null,
            baseMapOptions: null,
            baseLayers: [BaseMaps.GoogleTerrain, BaseMaps.GoogleStreets, BaseMaps.GoogleHybrid, BaseMaps.Lanskarta],
            restoreMapState: true,
            theme: false,
            cookieName: 'MapState'
    }, options);
        var useSwitcher = settings.toolsOptions != null && settings.toolsOptions.LayerOptions != null && settings.toolsOptions.LayerOptions.UseSwitcher;
        var baseMapSettings = getBaseMapSettings(settings.baseMapOptions);

        cookieName = settings.cookieName;

        OpenLayers.Lang.setCode(AnalysisPortal.Language);
        $("#" + settings.divId).html("");
        var map = new OpenLayers.Map(settings.divId, baseMapSettings); // create map        
        
        initBaseLayers(map, settings.baseLayers, settings.customBaseLayers);
        addBlankBaseLayer(map);                
        initVectorLayers(map, settings.vectorLayers, useSwitcher);        
        context.addWmsLayers(map, settings.wmsLayers);
        
        initKeyboard(map);        
        map.zoomTo(baseMapSettings.zoom); // zoom doesn't work in Map constructor since we create the base layers afterwards        

        //If layer switcher is on, bind change layer event to custom handler
        if (useSwitcher) {
            map.events.register("changelayer", this, function (e) {
                if (e.layer.visibility) {
                    if (e.layer.vectorLayerEnum == VectorLayers.WFSUserDefined) {
                        context.activateWfsLayer(map, e.layer);
                    } else if (e.layer.vectorLayerEnum == VectorLayers.SpatialFilter) {
                        context.activateSpatialFilterLayer(map, e.layer);
                    }
                }
            });
        }

       /* map.events.register("loadstart", this, function (e) {
            alert(e.layer.name);
        });*/

        if (settings.restoreMapState) {
            context.restoreMapState(map, settings.toolsOptions);
        }

        initTools(map, settings.toolsOptions, settings.divId);
        initToolbar(map, settings.panelDivId, settings.toolsOptions);

        return map;
    };        
    
    context.showWaitMsg = function (msg) {
        /// <summary>
        /// Shows a wait message.
        /// </summary>
        /// <param name="msg">The message to display.</param>
        /// <param name="divId">
        /// The name of the div that the message box will centered over. 
        /// If divId is null or 'body', the message will be centered over the page.
        /// </param>

        var divId = 'mapControl';

        Ext.get(divId).mask(msg);

        var $targetDiv = $('#' + divId);
        var openMsgCount = $targetDiv.data('openMsgCount');

        if (!openMsgCount) {
            openMsgCount = 0;
        }

        $targetDiv.data('openMsgCount', openMsgCount + 1);
    };


    context.hideWaitMsg = function () {
        /// <summary>
        /// Hides a wait message
        /// </summary>        
        /// <param name="divId">
        /// The name of the div that the message box is centered over. 
        /// If divId is null or 'body', the message is centered over the page.
        /// </param>

        var divId = 'mapControl';
        var $targetDiv = $('#' + divId);
        var openMsgCount = $targetDiv.data('openMsgCount') - 1;

        $targetDiv.data('openMsgCount', openMsgCount);

        if (!openMsgCount) {
            Ext.get(divId).unmask();
        }
    };

    function getBaseMapSettings(baseMapOptions) {
    	/// <summary>
    	/// Creates base map settings by extending default settings by the parameter settings.
    	/// </summary>
    	/// <param name="baseMapOptions">
    	///     Valid options are the one in the OpenLayers Map constructor
    	///     http://dev.openlayers.org/releases/OpenLayers-2.12/doc/apidocs/files/OpenLayers/Map-js.html#OpenLayers.Map.OpenLayers.Map
    	/// </param>
        /// <returns type=""></returns>

        if (baseMapOptions == null) {

            var srid = context.CurrentCoordinateSystem;
            var wgs84_projection = new OpenLayers.Projection('EPSG:4326');
            var baseProjection = new OpenLayers.Projection(srid);
            var centerCoordinate = new OpenLayers.LonLat(17.8480, 62.4865).transform(new OpenLayers.Projection('EPSG:4326'), baseProjection);
            var maxResolution = context.CoordinateSystems[srid].maxResolution;
            var bounds = new OpenLayers.Bounds(context.CoordinateSystems[srid].maxExtentProjected); //.transform(wgs84_projection, baseProjection);
            var units = context.CoordinateSystems[srid].units;

            baseMapOptions = {
                projection: baseProjection,
                displayProjection: wgs84_projection,  //new OpenLayers.Projection('EPSG:900913'), // This property specifies what projection various controls, such as the MousePosition control, display coordinates in. Requires proj4js support for projections other than EPSG:4326 or EPSG:900913/EPSG:3857            
                center: centerCoordinate, // Sweden center in Google Mercator coordinates            
                zoom: 3,
                maxExtent: bounds,
                maxResolution: maxResolution,
                units: units,
                controls: [
                    new OpenLayers.Control.Navigation(),
                    new OpenLayers.Control.Zoom(),
                    new OpenLayers.Control.ArgParser(),
                    new OpenLayers.Control.Attribution()
                ]
            };
        }
        return baseMapOptions;

        //var settings = $.extend(true, {
        //    maxScale: 423.186, // This specifies the maximum scale of the map. This property is similar to maxResolution—it limits how far out the user can zoom the map. If you specify the maxScale property, setting the maxResolution or minResolution properties may cause errors. Use either scales or resolutions, but avoid mixing them.                
        //    projection: new OpenLayers.Projection('EPSG:900913'),
        //    displayProjection: new OpenLayers.Projection('EPSG:900913'), // This property specifies what projection various controls, such as the MousePosition control, display coordinates in. Requires proj4js support for projections other than EPSG:4326 or EPSG:900913/EPSG:3857
        //    maxExtent: new OpenLayers.Bounds(-128 * 156543.0339, -128 * 156543.0339, 128 * 156543.0339, 128 * 156543.0339), // Setting this will specify the maximum extent of your map. Tiles that fall outside of the maximum extent will not be requested, nor can users pan to a coordinate that lies outside the maxExtent.
        //    maxResolution: 156543.0339, // The maxResolution property specifies what the maximum resolution of the map can be. Or, in other words, how far 'zoomed out' the map can be. Setting this property will affect what the base zoom level is and how far you can zoom out.                
        //    zoom: 3, // Sweden zoom 
        //    center: new OpenLayers.LonLat(1937831.5408159, 9068308.3109647), // Sweden center in Google Mercator coordinates
        //    units: "m", // Since Spherical Mercator is a projection that uses meters, we need to specify it here.
        //    controls: [
        //        new OpenLayers.Control.Navigation(),
        //        new OpenLayers.Control.Zoom(),
        //        new OpenLayers.Control.ArgParser(),
        //        new OpenLayers.Control.Attribution()
        //    ]
        //}, baseMapOptions);
        //return settings;
    }


    function initVectorLayers(map, vectorLayers, useSwitcher) {
        /// <summary>
        /// Initializes and adds vector layers to the map
        /// </summary>
        /// <param name="map" type="Object">The map object</param>
        /// <param name="vectorLayers" type="Object">Array with info about which vector layers to initialize</param>
        
        map.theObservationsLayer = OpenLayers.Layer.Vector;
        map.thePrecisionLayer = OpenLayers.Layer.Vector;
        map.theEditLayer = OpenLayers.Layer.Vector;
        map.wfsLayer = OpenLayers.Layer.Vector;
        map.vectorLayersInfo = vectorLayers;
        var currentProjection = context.getBaseProjection();


        var layers = [];
        for (var i = vectorLayers.length-1; i >= 0; i--) {
            var visibility = useSwitcher ? false : vectorLayers[i].visible;
            var initVisibilityState = vectorLayers[i].initVisibilityState == null ? LayerInitVisibilityState.NotSet : vectorLayers[i].initVisibilityState;

            if (vectorLayers[i].id == VectorLayers.Observations) {
                map.theObservationsLayer = new OpenLayers.Layer.Vector(AnalysisPortal.Resources.MapLayerObservations, {
                    visibility: visibility,
                    projection: context.GoogleMercatorProjection
                });
                map.theObservationsLayer.id = SystemLayerIds.Observations;
                map.theObservationsLayer.vectorLayerEnum = VectorLayers.Observations;
                map.theObservationsLayer.initVisibilityState = initVisibilityState;
                setObservationsLayerStyle(map.theObservationsLayer, false);
                layers.push(map.theObservationsLayer);
            }
            else if (vectorLayers[i].id == VectorLayers.ObservationAccuracy) {
                map.thePrecisionLayer = new OpenLayers.Layer.Vector(AnalysisPortal.Resources.MapLayerObservationsPrecision, {
                    visibility: visibility,
                    projection: context.GoogleMercatorProjection
                });
                map.thePrecisionLayer.id = SystemLayerIds.Precision;
                map.thePrecisionLayer.vectorLayerEnum = VectorLayers.ObservationAccuracy;
                map.thePrecisionLayer.initVisibilityState = initVisibilityState;
                map.thePrecisionLayer.notExportable = true;
                setPrecisionLayerStyle(map, map.thePrecisionLayer);
                layers.push(map.thePrecisionLayer);
            }
            else if (vectorLayers[i].id == VectorLayers.EditLayer) {
                map.theEditLayer = new OpenLayers.Layer.Vector(AnalysisPortal.Resources.MapLayerSpatialFilter, {
                    visibility: visibility,
                    projection: context.GoogleMercatorProjection
                });
                map.theEditLayer.vectorLayerEnum = VectorLayers.EditLayer;
                map.theEditLayer.initVisibilityState = initVisibilityState;
                //setSpatialFilterEvents(map, map.theEditLayer);
                layers.push(map.theEditLayer);
            }
            else if (vectorLayers[i].id == VectorLayers.GridLayer) {
                map.gridLayer = new OpenLayers.Layer.Vector(vectorLayers[i].name == null ? AnalysisPortal.Resources.MapGridLayer : vectorLayers[i].name, {
                    visibility: visibility,
                    projection: context.GoogleMercatorProjection
                });
                map.gridLayer.id = vectorLayers[i].gridTypeId;
                map.gridLayer.vectorLayerEnum = VectorLayers.GridLayer; 
                map.gridLayer.initVisibilityState = initVisibilityState;
                layers.push(map.gridLayer);
            }
            else if (vectorLayers[i].id == VectorLayers.EOOLayerConvexHull) {
                map.eooLayerConvexHull = new OpenLayers.Layer.Vector(AnalysisPortal.Resources.MapEOOLayer, {
                    visibility: visibility,
                    projection: currentProjection,
                    strategies: [new OpenLayers.Strategy.Fixed({ preload: false })],
                    protocol: new OpenLayers.Protocol.HTTP({
                        url: AnalysisPortal.ApplicationPath + '/Result/GetSpeciesObservationAOOEOOAsGeoJson',
                        format: new OpenLayers.Format.GeoJSON()
                    }),
                    eventListeners: {
                        loadend: function(e) {
                            var layer = e.object;

                            setAOOEOOFeatureSelected(map, layer.features[0], 'EOO convex');
                        }
                    }
                });
                map.eooLayerConvexHull.id = SystemLayerIds.EooConvexHull;
                map.eooLayerConvexHull.initVisibilityState = initVisibilityState;
                map.eooLayerConvexHull.vectorLayerEnum = VectorLayers.EOOLayerConvexHull;
                addLayerLoadEvents(map.eooLayerConvexHull);

                layers.push(map.eooLayerConvexHull);
            }
            else if (vectorLayers[i].id == VectorLayers.EOOLayerConcaveHull) {
                map.eooLayer = new OpenLayers.Layer.Vector(AnalysisPortal.Resources.MapEOOLayerConcave, {
                    visibility: false,
                    projection: currentProjection,
                    strategies: [new OpenLayers.Strategy.Fixed({ preload: false }), new OpenLayers.Strategy.Refresh({ force: true, active: true })],
                    protocol: new OpenLayers.Protocol.HTTP({
                        url: AnalysisPortal.ApplicationPath + '/Result/GetSpeciesObservationAOOEOOAsGeoJson',
                        format: new OpenLayers.Format.GeoJSON()
                    }),
                    eventListeners: {
                        visibilitychanged: function (e) {
                         
                            var layer = e.object;
                            if (layer.visibility) {
                                var $alphaValueInput = $('<input/>')
                                    .attr({
                                        id: 'alphaValue',
                                        name: 'alphaValue',
                                        class: 'slider',
                                        'data-slider-min': 0,
                                        'data-slider-max': 1000,
                                        'data-slider-step': 1,
                                        'data-slider-value': (
                                            layer.alphaValue == null ? defaultAlphaValue : layer.alphaValue),
                                        'data-slider-tooltip': 'always'
                                    })
                                    .css({ width: '100%' });

                                var $alphaValueContainer = $('<div/>')
                                    .append(
                                        $('<label/>')
                                        .attr('for', 'alphaValue')
                                        .text(AnalysisPortal.Resources.MapEOOLayerConcaveCalculateAlphaValue)
                                        .css({ 'margin-right': '10px' })
                                    )
                                    .append(
                                        $alphaValueInput
                                    );

                                var $form = $('<form/>').attr('id', 'alphaForm');
                                $form.append($alphaValueContainer);

                                var $useCenterPointContainer = $('<div/>')
                                    .addClass('radio')
                                    .append(
                                        $('<label/>')
                                        .append(
                                            $('<input/>')
                                            .attr({ id: "useCenterPoint", name: "pointUsage", type: "radio", value: true })
                                        )
                                        .append(AnalysisPortal.Resources.MapEOOLayerConcaveCalculateUseCenterPoint)
                                    );

                                $form.append($useCenterPointContainer);

                                var $useCornerPointsContainer = $('<div/>')
                                    .addClass('radio')
                                    .append(
                                        $('<label/>')
                                        .append(
                                            $('<input/>')
                                            .attr({ id: "useCornerPoints", name: "pointUsage", type: "radio", value: false })
                                        )
                                        .append(AnalysisPortal.Resources.MapEOOLayerConcaveCalculateUseGridCellCornerPoints)
                                    );

                                $form.append($useCornerPointsContainer);

                                var $selectedPointUsage = $('input[name="pointUsage"][value="' + (layer.useCenterPoint == null ? true : layer.useCenterPoint) + '"]', $form);
                                $selectedPointUsage.attr('checked', 'checked');

                                var $calculateButton = $('<button/>')
                                    .attr({ type: 'button', id: 'calculateConcaveHull' })
                                    .addClass('btn btn-default')
                                    .text(AnalysisPortal.Resources.SharedCalculate);

                                $form.append($calculateButton);

                                $('body')
                                    .off()
                                    .on('click',
                                        '#calculateConcaveHull',
                                        function() {
                                            var $alphaForm = $(this).closest('form');
                                            if ($alphaForm.valid()) {
                                                var alphaValue = $('#alphaValue', $alphaForm).val();
                                                var useCenterPoint = $('#useCenterPoint', $alphaForm).is(':checked');
                         
                                                layer.protocol.options
                                                    .url = AnalysisPortal.ApplicationPath +
                                                    '/Result/GetSpeciesObservationAOOEOOAsGeoJson?alphaValue=' +
                                                    alphaValue +
                                                    '&useCenterPoint=' +
                                                    useCenterPoint;
                                                layer.strategies[1].refresh();

                                                //Save current values, use in map export
                                                layer.alphaValue = alphaValue;
                                                layer.useCenterPoint = useCenterPoint;
                                            }
                                        });

                                var $content = $('<div/>')
                                    .append($('<div/>')
                                        .addClass('alert alert-info')
                                        .text(AnalysisPortal.Resources.MapEOOLayerConcaveCalculateDescription)
                                    )
                                    .append($form);
                                AnalysisPortal.showDialog({
                                    sizeClass: 'modal-md',
                                    title: AnalysisPortal.Resources.MapEOOLayerConcaveCalculateTitle,
                                    content: $content.html(),
                                    afterLoadCallback: function() {
                                        $("input.slider", $('#alphaForm')).slider();
                                    }
                                });

                                return false;
                            }

                            return true;
                        },
                        loadend: function(e) {
                            var layer = e.object;

                            setAOOEOOFeatureSelected(map, layer.features[0], 'EOO concave');
                        }
                    }
                });
                map.eooLayer.id =  SystemLayerIds.EooConcaveHull;
                map.eooLayer.vectorLayerEnum = VectorLayers.EOOLayerConcaveHull;
                map.eooLayer.initVisibilityState = initVisibilityState;
                addLayerLoadEvents(map.eooLayer);

                layers.push(map.eooLayer);
            }
            else if (vectorLayers[i].id == VectorLayers.SpatialFilter) {                
                map.spatialLayer = new OpenLayers.Layer.Vector(AnalysisPortal.Resources.MapLayerSpatialFilter, {
                    visibility: visibility,
                    projection: context.GoogleMercatorProjection
                });
                map.spatialLayer.vectorLayerEnum = VectorLayers.SpatialFilter;
                map.spatialLayer.initVisibilityState = initVisibilityState;
                layers.push(map.spatialLayer);
            }

            else if (vectorLayers[i].id == VectorLayers.WFS) {
                map.wfsLayer = new OpenLayers.Layer.Vector("WFS", {visibility: vectorLayers[i].visible });
                map.wfsLayer.vectorLayerEnum = VectorLayers.WFS;
                map.wfsLayer.initVisibilityState = initVisibilityState;
                context.setWfsLayerStyle(map.wfsLayer, "");
                layers.push(map.wfsLayer);
            }            
            else if (vectorLayers[i].id != VectorLayers.WFS && vectorLayers[i].id != VectorLayers.Observations
                && vectorLayers[i].id != VectorLayers.ObservationAccuracy && vectorLayers[i].id != VectorLayers.EditLayer) {
                var wfsLayer = new OpenLayers.Layer.Vector(vectorLayers[i].id, {visibility: vectorLayers[i].visible });
                wfsLayer.vectorLayerEnum = VectorLayers.WFSUserDefined;
                wfsLayer.initVisibilityState = initVisibilityState;
                wfsLayer.Identifier = vectorLayers[i].id;
                wfsLayer.id = vectorLayers[i].id;
                wfsLayer.name = vectorLayers[i].name;
                var wfsColor = vectorLayers[i].color;
                context.setWfsLayerStyle(wfsLayer, wfsColor);
                layers.push(wfsLayer);
            }
        }
        map.addLayers(layers);
        map.vectorLayers = layers;

        // if we want to use Canvas rendering we can add the vector layer like this:
        //map.theObservationsLayer = new OpenLayers.Layer.Vector("Observations", { renderers: ['Canvas', 'SVG', 'VML'] });        
    }
     
    context.addWfsVectorLayer = function (map, vectorLayer) {
    /// <summary>
    /// Initializes and add a userdefined wfs vectorlayer to the map
    /// </summary>
    /// <param name="map" type="Object">The map object</param>
    /// <param name="vectorLayers" type="Object">Info about userDefined vector layer to initialize</param>       
        //Create a new Vectorlayer from selected MaplayerSetting
        var wfsLayer = new OpenLayers.Layer.Vector(vectorLayer.id, {visibility: vectorLayer.visible });
        wfsLayer.vectorLayerEnum = VectorLayers.WFSUserDefined;
        wfsLayer.Identifier = vectorLayer.id;
        wfsLayer.id = vectorLayer.id;
        wfsLayer.name = vectorLayer.name;
        //Set layer to be visible
        wfsLayer.visible = true;
        //Set userdefined colors
        var wfsColor = vectorLayer.color;
        context.setWfsLayerStyle(wfsLayer, wfsColor);
        //Add Wfs layer to map
        map.addLayer(wfsLayer);
        map.vectorLayers.push(wfsLayer);       
        
        return wfsLayer;
    };
    
    function addLayerLoadEvents(layer) {
        layer.events.register("loadstart", layer, function (e) {
            context.showWaitMsg(AnalysisPortal.Resources.SharedLoadingData);
        });

        layer.events.register("loadend", layer, function (e) {
            context.hideWaitMsg();
        });
    };

     function addObservationVectorLayers (map) {
    /// <summary>
    /// Add and create observation vectorlayers to map 
    /// </summary>
    /// <param name="map" type="Object">The map object</param>
        
        // Create observation vector layer and add it to map
        var observationLayer = new OpenLayers.Layer.Vector(AnalysisPortal.Resources.MapLayerObservations, {visibility: true }); 
        observationLayer.vectorLayerEnum = VectorLayers.Observations;
        observationLayer.visible = true;
        
        map.theObservationsLayer = observationLayer;
        setObservationsLayerStyle(map.theObservationsLayer, false);
       
        map.addLayer(map.theObservationsLayer);
        map.vectorLayers.push(map.theObservationsLayer);
         
       // Create observation precision vector layer and add it to map
        var precisionLayer = new OpenLayers.Layer.Vector(AnalysisPortal.Resources.MapLayerObservationsPrecision, {visibility: true }); 
        precisionLayer.vectorLayerEnum = VectorLayers.ObservationAccuracy;
        precisionLayer.visible = true;
        
        map.thePrecisionLayer = precisionLayer;
        setObservationsLayerStyle(map.thePrecisionLayer, false);
       
        setPrecisionLayerStyle(map, map.thePrecisionLayer);
        
        map.addLayer(map.thePrecisionLayer);
        map.vectorLayers.push(map.thePrecisionLayer);
    };
    
     function addEditVectorLayer (map) {
        /// <summary>
        /// Create and add the edit vectorlayer to the map
        /// </summary>
        /// <param name="map" type="Object">The map object</param>
        /// <param name="vectorLayers" type="Object">Info about userDefined vector layer to initialize</param>       
         
        // Create edit/spatial layer
        var editLayer = new OpenLayers.Layer.Vector(AnalysisPortal.Resources.MapLayerSpatialFilter, {visibility: true }); 
        editLayer.vectorLayerEnum = VectorLayers.EditLayer;
        editLayer.visible = true;
        map.theEditLayer = editLayer;
        map.addLayer(editLayer);
        map.vectorLayers.push(editLayer);
            
        map.theEditLayer.setVisibility(false);
    };
    
    function getVectorLayerArray(map, layerNames) {
        /// <summary>
        /// Loops thru all the layers and returns the ones that exist in layerNames
        /// </summary>        
        // We must get the existing spatialfilters
        var layers = [];        
        for (var i = 0; i < layerNames.length; i++) {
            for (var j= 0; j < map.vectorLayers.length; j++) {
                if (map.vectorLayers[j].vectorLayerEnum == layerNames[i]) {
                    layers.push(map.vectorLayers[j]);
                    //break;
                }
            }
        }
        return layers;
    }

    function getVectorLayer(map, layerName) {
        /// <summary>
        /// Loops thru all the layers and returns the one that corresponds to layerName
        /// or null if it doesn't exist.
        /// </summary>        

        for (var j= 0; j < map.vectorLayers.length; j++) {
            if (map.vectorLayers[j].vectorLayerEnum == layerName) {
                return map.vectorLayers[j];                
            }
        }
        return null;
    }

    function initTools(map, toolsOptions, mapDivId) {
        /// <summary>
        /// Initializes the map tools (layer switcher, mouse position, overview map)
        /// </summary>                 
        
        if (!toolsOptions) {
            return;
        }

        // create layer switcher sidebar if settings is true
        if (toolsOptions.LayerOptions.UseSwitcher) {
            initMapLayerSideBar(map, mapDivId, toolsOptions.LayerOptions.OpenSwitcher);
        }
        
        // create mouse position panel if settings is true
        if (toolsOptions.ShowMousePosition == true) {
            initMousePosition(map);
        }
        
        // create overview map panel if settings is true
        if (toolsOptions.UseOvMap == true) {
            initOverviewMap(map);
        }        
    }

    function initMousePosition(map) {
        /// <summary>
        /// Initializes the mouse position panel
        /// </summary>

        map.addControl(new OpenLayers.Control.MousePosition());
    }

    function initOverviewMap(map) {
        /// <summary>
        /// Initializes the overview map panel
        /// </summary>        
        
        var ovMapOptions = map.mapOption.OvMapOptions;
        var ovMapLayer = ovMapOptions.OvMapLayer;
        map.OvMapCtrl = new OpenLayers.Control.OverviewMap({
                layers: [ovMapLayer],
                mapOptions: ovMapOptions,
                autoPan: true
            });
        map.addControl(map.OvMapCtrl);
                
        if (ovMapOptions.UseOvMap == true && ovMapOptions.ShowOvMap == false) {
            map.OvMapCtrl.minimizeControl();
        }
        else {
            map.OvMapCtrl.maximizeControl();
        }
    }

    function initKeyboard(map) {
        /// <summary>
        /// Initializes the keyboard handler
        /// </summary>                
        var controls = [map.selectControl];
        if (map.selectManyControl)
            controls.push(map.selectManyControl);
        var vectorLayer = getVectorLayer(map, VectorLayers.EditLayer);
        if (!isEmpty(vectorLayer)) {
            map.keyboardHandler = new OpenLayers.Handler.Keyboard(controls, {
                "keydown": function(evt) {                    
                    if (evt.keyCode === 46) { // delete 
                        map.theEditLayer.destroyFeatures(map.theEditLayer.selectedFeatures, { silent: false });
                        //map.theEditLayer.removeFeatures(map.theEditLayer.selectedFeatures, { silent: true });
                    }                    
                }
            });
            map.keyboardHandler.setMap(map);
            map.keyboardHandler.activate();
        }           
    }

    function initToolbar(map, panelDivId, toolsOptions) {
        /// <summary>
        /// Initializes the toolbar
        /// </summary>                

        if (panelDivId == null)
            return;

        var vectorLayerArray = [];        
        var paneldiv = OpenLayers.Util.getElement("mapToolbarPanel");
        if (panelDivId != null) {
            paneldiv = $("#" + panelDivId).get(0);
        }
        map.mapPanel = new OpenLayers.Control.Panel({ displayClass: "olControlPanel", div: paneldiv });

        // Zoom to Sweden extent
        map.zoomToSwedenExtentControl = new OpenLayers.Control.Button({            
            title: AnalysisPortal.Resources.MapZoomToSweden,
            displayClass: "olControlZoomToSwedenExtent",
            trigger: function () {
                var srid = context.CurrentCoordinateSystem;
                var baseProjection = new OpenLayers.Projection(srid);
                var bounds = new OpenLayers.Bounds(swedenExtentWGS84).transform(new OpenLayers.Projection('EPSG:4326'), baseProjection);
                map.zoomToExtent(bounds);
            }
        });
        map.mapPanel.addControls([map.zoomToSwedenExtentControl]);

        // Zoom to features bounding box
        map.zoomToFeaturesControl = new OpenLayers.Control.Button({
            title: AnalysisPortal.Resources.MapZoomToFeatures,            
            displayClass: "olControlZoomToFeatures",
            trigger: function() {
                context.zoomToFeaturesInVisibleVectorLayers(map, {showMessages: true});                
            }
        });
        map.mapPanel.addControls([map.zoomToFeaturesControl]);

        // Navigation buttons
        if (toolsOptions.UseNavigationHistory == true) {
            map.theNavHist = new OpenLayers.Control.NavigationHistory({ previousOptions: { title: AnalysisPortal.Resources.MapHistoryBack }, nextOptions: { title: AnalysisPortal.Resources.MapHistoryNext } });
            map.addControl(map.theNavHist);
            map.mapPanel.addControls([map.theNavHist.previous, map.theNavHist.next]);
        }

        // Zoom box
        map.zoomBoxControl = new OpenLayers.Control.ZoomBox({ title: AnalysisPortal.Resources.MapZoomBox });
        map.mapPanel.addControls([map.zoomBoxControl]);

        // DragPan
        map.dragPanControl = new OpenLayers.Control.DragPan({            
            title: AnalysisPortal.Resources.MapDragMap
        });
        map.mapPanel.addControls([map.dragPanControl]);

        // Highlight, shows when hovering mouse pointer over
        //vectorLayerArray = getVectorLayerArray(map, [VectorLayers.Observations, VectorLayers.EditLayer, VectorLayers.WFS]);
        vectorLayerArray = getVectorLayerArray(map, [VectorLayers.Observations, VectorLayers.EditLayer, VectorLayers.WFS, VectorLayers.WFSUserDefined, VectorLayers.GridLayer, VectorLayers.EOOLayerConvexHull, VectorLayers.EOOLayerConcaveHull]);
        if (!isEmpty(vectorLayerArray)) {
            map.highlightControl = new OpenLayers.Control.SelectFeature(vectorLayerArray, {
                hover: true,
                highlightOnly: true,
                renderIntent: "temporary",
                eventListeners: {
                    featurehighlighted: function (e) {                        
                        // den här körs när man hovrar musen över punkten
                        $('#' + map.div.id).triggerHandler('onMapFeatureHighlighted', [e.feature]);
                        showFeaturePopup(map, e);
                    },
                    featureunhighlighted: function (e) {                        
                        $('#' + map.div.id).triggerHandler('onMapFeatureUnhighlighted', [e.feature]);
                        hideFeaturePopup(map, e);
                    }
                }
            });
            map.addControl(map.highlightControl);
            map.highlightControl.activate();
        }

        // Select
        vectorLayerArray = getVectorLayerArray(map, [VectorLayers.Observations, VectorLayers.EditLayer, VectorLayers.WFS, VectorLayers.WFSUserDefined, VectorLayers.GridLayer, VectorLayers.EOOLayerConvexHull, VectorLayers.EOOLayerConcaveHull]);
        if (!isEmpty(vectorLayerArray)) {
            map.selectControl = new OpenLayers.Control.SelectFeature(vectorLayerArray, {
                title: AnalysisPortal.Resources.MapSelect,
                clickout: true,
                onSelect: function (feature) {
                    if (feature.layer.vectorLayerEnum == VectorLayers.WFSUserDefined)
                        setWfsFeatureSelected(map, feature);
                    else if (feature.layer.vectorLayerEnum == VectorLayers.EOOLayerConvexHull) {
                        setAOOEOOFeatureSelected(map, feature, 'EOO convex');
                    }
                    else if (feature.layer.vectorLayerEnum == VectorLayers.EOOLayerConcaveHull) {
                        setAOOEOOFeatureSelected(map, feature, 'EOO concave');
                    }
                    else
                        setMapObservationSelected(map, feature);
                },
                onUnselect: function (feature) {
                    if (feature.layer.vectorLayerEnum == VectorLayers.WFSUserDefined)
                        setWfsFeatureUnSelected(map, feature);
                    else
                        setMapObservationsUnSelected(map, feature);
                },
                eventListeners: {
                    deactivate: function(e) {
                        map.selectControl.unselectAll();
                    }                                    
                }            
            });
            map.mapPanel.addControls([map.selectControl]);
            //map.selectControl.activate();
        }    
                
        // Select many
        if (toolsOptions.ShowSelectManyControl == true) {
            vectorLayerArray = getVectorLayerArray(map, [VectorLayers.Observations, VectorLayers.EditLayer]); //, VectorLayers.WFS]);
            if (!isEmpty(vectorLayerArray)) {
                map.selectManyControl = new OpenLayers.Control.SelectFeature(vectorLayerArray, {
                    title: AnalysisPortal.Resources.MapSelectMany,                    
                    displayClass: "olControlSelectManyFeature",
                    multiple: true,
                    box: true,
                    clickout: true,
                    onSelect: function(feature) {
                        setMapObservationSelected(map, feature);
                    },
                    onUnselect: function(feature) {
                        setMapObservationsUnSelected(map, feature);
                    }
                });
                map.mapPanel.addControls([map.selectManyControl]);
                //map.selectControl.activate();
            }
        }        

        if (toolsOptions.ShowCreatePolygonTools) {
            initEditPolygonTools(map, toolsOptions);
        }

        map.addControl(map.mapPanel);        
        map.events.register('moveend', map, function() { onMapPanOrZoom(map); });  
        if (map.selectControl) {
            map.selectControl.activate();
        }        
    }

    function initEditPolygonTools(map, toolsOptions) {
        /// <summary>
        /// Initializes the overview map panel
        /// </summary>                
        
        map.addRectangleControl = new OpenLayers.Control.DrawFeature(map.theEditLayer, 
            OpenLayers.Handler.RegularPolygon, {
                'displayClass': 'olControlDrawFeatureRectangle',
                title: AnalysisPortal.Resources.MapCreateRectangle,
                handlerOptions: {
                    sides: 4,
                    irregular: true
                }                    
            });

        map.mapPanel.addControls([map.addRectangleControl]);
        map.addRectangleControl.events.register('activate', map.addRectangleControl, function () { onSiteAddActivateCheck(map); });            
            
        // Example how to make holes in polygon: http://openlayers.org/dev/examples/donut.html
        // var draw = new OpenLayers.Control.DrawFeature(map.layers[1],OpenLayers.Handler.Polygon,{handlerOptions: {holeModifier: "altKey"}});             
        map.addPolygonControl = new OpenLayers.Control.DrawFeature(map.theEditLayer, OpenLayers.Handler.Polygon, { 'displayClass': 'olControlDrawFeaturePolygon', title: AnalysisPortal.Resources.MapCreatePolygon });
        map.mapPanel.addControls([map.addPolygonControl]);
        map.addPolygonControl.events.register('activate', map.addPolygonControl, function () { onSiteAddActivateCheck(map); });
        map.theEditLayer.events.register('featureadded', map.theEditLayer, function (e) { TryToAddSiteToMap(map, e); });

        map.editFeatureControl = new OpenLayers.Control.ModifyFeature(map.theEditLayer, { title: AnalysisPortal.Resources.MapEditPolygon }); 
        map.mapPanel.addControls(map.editFeatureControl);
        map.editFeatureControl.events.register('activate', map.theEditLayer, function () {
            map.addControl(map.editFeatureControl);
            map.highlightControl.deactivate();
            map.mapPopupId = null;
            if (map.mapPopup) {
                map.removePopup(map.mapPopup);
                map.mapPopup = null;
            }
            map.selectControl.unselectAll();
        });

        map.editFeatureControl.events.register('deactivate', map, function () {
            map.removeControl(map.editFeatureControl);
            map.highlightControl.activate();
        });

        map.theEditLayer.events.register('afterfeaturemodified', map.theEditLayer, function (e) { SiteEdited(map, e); });
        map.theEditLayer.events.register('beforefeaturemodified', map.theEditLayer, function (e) {
            oldGeometry = e.feature.geometry.clone();
        });

        map.dragFeatureControl = new OpenLayers.Control.DragFeature(map.theEditLayer, { title: AnalysisPortal.Resources.MapMovePolygon, 
            onComplete: function (feature, pixel) {
                onFeatureMoved(map, feature);
            },
            onStart: function (feature, pixel) {
                oldGeometry = feature.geometry.clone();
            }
        });
        map.mapPanel.addControls(map.dragFeatureControl);
        map.dragFeatureControl.events.register('activate', map.theEditLayer, function () {
            map.addControl(map.dragFeatureControl);
            map.highlightControl.deactivate();
            map.mapPopupId = null;
            if (map.mapPopup) {
                map.removePopup(map.mapPopup);
                map.mapPopup = null;
            }
            map.selectControl.unselectAll();
        });

        map.dragFeatureControl.events.register('deactivate', map, function () {
            map.removeControl(map.dragFeatureControl);
            map.highlightControl.activate();
        });        
    }

    context.updateMapSize = function(map, mapControlId, mapToolbarId) {        
        try {            
            var $mapControl = $('#' + mapControlId);
            var toolbarHeight = $('#' + mapToolbarId).height();
            var $mapControlParent = $mapControl.parent();
            var width = $mapControl.width();
            var height = $mapControl.height();
            var parentWidth = $mapControlParent.width();
            var parentHeight = $mapControlParent.height() - toolbarHeight;
            if (width != parentWidth || height != parentHeight) {
                $mapControl.width(parentWidth);
                $mapControl.height(parentHeight);                
                map.updateSize();                                
            }
        } catch(err) {            
        }                    
    };

    context.startUpdateMapSizeTimer = function(map, options) {
        var settings = $.extend(true, {
                mapControlId: 'mapControl',
                mapToolbarId: 'mapNavDiv',
                interval: 500
            }, options);
        
        setInterval(function() {
            context.updateMapSize(map, settings.mapControlId, settings.mapToolbarId);
        }, settings.interval);
    };


    context.addLayer = function (map, layer, options) {
        var settings = $.extend(true, {
            SelectTool: false,
            SelectManyTool: false,
            DrawRectangleTool: false,
            DrawPolygonTool: false,
            EditPolygonTool: false
        }, options);

        if (settings.SelectTool) { // OpenLayers.Control.SelectFeature
            var layers = map.highlightControl.layers;
            layers.push(layer);
            map.highlightControl.setLayer(layers);
        }
    };

    context.saveMapState = function () {
    	/// <summary>
    	/// Save the map state (position,zoom,baselayer) into
    	/// a session cookie.
        /// </summary>
        try {
            var zoom = map.getZoom();
            var center = map.getCenter();                                    
            center = center.transform(context.getBaseProjection(), context.GoogleMercatorProjection);
            var centerLon = center.lon;
            var centerLat = center.lat;
            var baseLayerIndex = getLayerIndex(map, map.baseLayer);            
            var options = { expires: 90, path: '/' }; // Session cookie when expires is omitted (expires: 90 // 90days)
            $.subcookie(cookieName, 'zoom', zoom, options);
            $.subcookie(cookieName, 'centerLon', centerLon, options);
            $.subcookie(cookieName, 'centerLat', centerLat, options);
            $.subcookie(cookieName, 'baseLayerIndex', baseLayerIndex, options);
            $.subcookie(cookieName, 'action', 'zoomToSettings', options);
            $.subcookie(cookieName, 'layerSidebarOpen', islayerSidebarOpen(), options);
            
            var treeLayers = getOtherLayerFromTree();
           
            var saveLayers = [];
            $.each(treeLayers, function(index, treeLayer) {
                var layer = map.getLayersByName(treeLayer.name)[0];
                saveLayers.push({ id: layer.id, index: getLayerIndex(map, layer), visibility: layer.visibility == true });
            });

            $.subcookie(cookieName, 'layers', Ext.JSON.encode(saveLayers), options);
        } 
        catch(err) {
            console.log('Error! saveMapState()');
        }
    };

    context.mapStateCookieExists = function() {
    	/// <summary>
    	/// Checks if a map state session cookie has been created.
    	/// </summary>
        var exists = $.cookie(cookieName) !== null;
        return exists;
    };

    context.restoreMapState = function (map, toolOptions) {
        var success = false;
    	/// <summary>
    	/// Restores the map state from session cookie.
    	/// </summary>    	
        try {
            var cookieExists = context.mapStateCookieExists();
            var layerCount = map.layers.length;
            var visibleLayerBaseIndex = 100;

            if (cookieExists) {
                var zoom = $.subcookie(cookieName, 'zoom');
                var centerLon = $.subcookie(cookieName, 'centerLon');
                var centerLat = $.subcookie(cookieName, 'centerLat');
                var baseLayerIndex = $.subcookie(cookieName, 'baseLayerIndex');
                var action = $.subcookie(cookieName, 'action');
                var openLayerSidebar = $.subcookie(cookieName, 'layerSidebarOpen');
                var savedLayers = Ext.JSON.decode($.subcookie(cookieName, 'layers'), false);

                //Update open switcher from saved settings if it's exists, else use init value
                if (toolOptions != null && toolOptions.LayerOptions != null && openLayerSidebar != null) {
                    toolOptions.LayerOptions.OpenSwitcher = openLayerSidebar != 'false';
                }

                if (!context.RenderImageMode) {
                    if (baseLayerIndex) {
                        if (baseLayerIndex < map.layers.length && map.layers[baseLayerIndex].isBaseLayer)
                            map.setBaseLayer(map.layers[baseLayerIndex]);
                    }
                }

                //If we dont have any saved layers, order visible layers first
                if (savedLayers == null) {
                    $.each(map.layers, function (i, layer) {
                        if (!layer.isBaseLayer && layer.displayInLayerSwitcher) {
                            var indexBase = layer.visibility ? visibleLayerBaseIndex : 0;
                            setLayerIndex(map, layer, indexBase + layerCount - i);
                        }
                    });
                } else {
                    //Loop trow all layers in map and update z index and visibility
                    $.each(savedLayers, function (index, savedLayer) {
                        $.each(map.layers, function (i, layer) {

                            if (!layer.isBaseLayer && layer.displayInLayerSwitcher && layer.id == savedLayer.id) {
                                setLayerIndex(map, layer, savedLayer.index);
                                setLayerVisibility(map, layer, savedLayer.visibility && layer.initVisibilityState !== LayerInitVisibilityState.AlwaysHidden);

                                return;
                            }
                        });
                    });
                }

                if (action != null && action == "zoomToFeatures") {
                    context.zoomToFeaturesInVisibleVectorLayers(map, { zoomToSwedenIfFail: true });
                    return true;
                }
                if (action != null && action == "zoomToSweden") {
                    context.zoomToSweden(map);
                    return true;
                }
                if (zoom)
                    map.zoomTo(zoom);
                if (centerLon != null && centerLat != null) {
                    var center = new OpenLayers.LonLat(centerLon, centerLat).transform(context.GoogleMercatorProjection, context.getBaseProjection());
                    map.setCenter(center);
                }
                
            } else {
                context.zoomToFeaturesInVisibleVectorLayers(map, { zoomToSwedenIfFail: true });
            }
            
            //Make sure that mandatory layers are visible at start
            $.each(map.layers, function (i, layer) {
                if (layer.initVisibilityState === LayerInitVisibilityState.AlwaysVisible) {
                    setLayerVisibility(map, layer, true);

                    //If cookie is missing, set mandatory layers on top
                    if (!cookieExists) {
                        setLayerIndex(map, layer, visibleLayerBaseIndex + layerCount - i);
                    }
                }
            });

            success = true;
        } 
        catch(err) {
            console.log('Error! restoreMapState()');
        }

        return success;
    };

    context.isCoordinateSystemSupported = function (srs) {
        for (var key in context.CoordinateSystems) {
            if (key == srs)
                return true;            
        }
        return false;     
    };

    context.getCoordinateSystemName = function (srs) {
        return context.CoordinateSystems[srs].name;
    };

    context.getBaseProjection = function() {
        var srid = context.CurrentCoordinateSystem;
        var baseProjection = new OpenLayers.Projection(srid);
        return baseProjection;
    };

    context.createGeoJsonFormat = function () {
        var geoJsonFormat = new OpenLayers.Format.GeoJSON({
            internalProjection: context.getBaseProjection(),
            externalProjection: context.GoogleMercatorProjection           
        });        
        return geoJsonFormat;
    };
    
    context.zoomToSweden = function(map) {
    	/// <summary>
    	/// Zoom so that the whole Sweden is visible
    	/// </summary>
        map.zoomToExtent(new OpenLayers.Bounds(swedenExtentGoogleMercator));
    };

    context.zoomToFeaturesInVisibleVectorLayers = function(map, options) {
    	/// <summary>
    	/// Calculates the bounding box of all visible features and
    	/// zooms to the bounding box, recenter.    	
    	/// </summary>
    	/// <param name="options">
    	///     showMessages {Boolean} Shows message if there are no visible layers. Default is false.
        ///     zoomToSwedenIfFail {Boolean} Zooms to Sweden if there is no visible layers. Default is false.
        ///     allowZoomIn {Boolean} if false and all features are visible no zoom is made. Defaults to true.
        ///     allowZoomOutsideSwedenExtent: {Boolean} if false the zoom will be not zoom outside Sweden extent. Default is true.
    	/// </param>        
        var settings = $.extend(true, {
            showMessages: false,
            zoomToSwedenIfFail: false,
            allowZoomIn: true,
            allowZoomOutsideSwedenExtent: true
        }, options);
        var layers = [];
        var vectorLayers = map.getLayersByClass("OpenLayers.Layer.Vector");
        for (var i = 0; i < vectorLayers.length; i++) {
            if (vectorLayers[i].getVisibility()) {
                layers.push(vectorLayers[i]);
            }                
        }
        if (layers.length == 0) {
            if (settings.showMessages) {
                AnalysisPortal.showMsg("Could not zoom to visible features since there are no visible layers");
            }
            if (settings.zoomToSwedenIfFail) {                
                context.zoomToSweden(map);
            }
            return;
        }
        context.zoomToFeaturesInLayers(map, layers, options);
    };


    context.zoomToFeaturesInLayers = function(map, layers, options) {
        /// <summary>
    	/// Calculates the bounding box of the features in layers and
    	/// zooms to the bounding box, recenter.    	
    	/// </summary>
    	/// <param name="layers">The layers containing features.</param>
    	/// <param name="options">        
    	///     showMessages {Boolean} Shows message if there are no features in the layers. Default is false.
        ///     zoomToSwedenIfFail {Boolean} Zooms to Sweden if there is no features. Default is false.         
        ///     allowZoomIn {Boolean} if false and all features are visible no zoom is made. Defaults to true.        
        ///     allowZoomOutsideSwedenExtent: {Boolean} if false the zoom will be not zoom outside Sweden extent. Default is true.
    	/// </param>        
        var settings = $.extend(true, {
            showMessages: false,
            zoomToSwedenIfFail: false,
            allowZoomIn: true,
            allowZoomOutsideSwedenExtent: true
        }, options);
        
        if (layers == null || layers.length == 0)
            return;

        var bounds,i;
        for (i = 0; i < layers.length; i++) {
            if (layers[i].features.length > 0) {
                bounds = layers[i].features[0].geometry.getBounds().clone();
                break;
            }
        }
        if (bounds == null) {
            if (settings.showMessages) {
                AnalysisPortal.showMsg("Could not zoom to visible features since there exist no features");
            }
            if (settings.zoomToSwedenIfFail) {                
                context.zoomToSweden(map);
            }            
            return;        
        }            

        for (i = 0; i < layers.length; i++) {
            for(var j=0; j<layers[i].features.length; j++)
                bounds.extend(layers[i].features[j].geometry.getBounds());
        }
        
        if (!settings.allowZoomOutsideSwedenExtent) {
            var swedenExtent = context.getProjectedSwedenExtentBoundingBox();
            if (bounds.left < swedenExtent.left)
                bounds.left = swedenExtent.left;
            if (bounds.right > swedenExtent.right)
                bounds.right = swedenExtent.right;
            if (bounds.bottom < swedenExtent.bottom)
                bounds.bottom = swedenExtent.bottom;
            if (bounds.top > swedenExtent.top)
                bounds.top = swedenExtent.top;            
        }


        if (!settings.allowZoomIn) {
            var mapBounds = map.getExtent();
            if (!mapBounds.containsBounds(bounds))
                map.zoomToExtent(bounds, false);
            return;
        }
        map.zoomToExtent(bounds,false);
    };


    context.zoomToFeatures = function(map, features) {
        /// <summary>
    	/// Calculates the bounding box of the features and
    	/// zooms to the bounding box, recenter.    	
    	/// </summary>
    	/// <param name="feature">The features.</param>            
        if (features == null || features.length == 0) {
            return;
        }
        var bounds = features[0].geometry.getBounds().clone(); 
        for(var i=1; i<features.length; i++)
            bounds.extend(features[i].geometry.getBounds());
        map.zoomToExtent(bounds,false);
    };


    context.zoomToFeature = function(map, feature) {
    	/// <summary>
    	/// Calculates the bounding box of the feature and
    	/// zooms to the bounding box, recenter.    	
    	/// </summary>
    	/// <param name="feature">The feature.</param>    
        map.zoomToExtent(feature.geometry.getBounds());
    };

    function onSiteAddActivateCheck(map) {
        console.log('onSiteAddActivateCheck');
    }   
    
    function TryToAddSiteToMap(map, e) {
        console.log('TryToAddSiteToMap');
    }    
    
    function SiteEdited(map, e) {
        console.log('SiteEdited');
    }    
    
    function onFeatureMoved(map, feature) {        
        feature.layer.events.triggerEvent("afterfeaturemodified", {
          feature :feature
        });
    }    
    
    function initBaseLayers(map, baseLayers, customBaseLayers) {
        /// <summary>
        /// Adds base layers to the map. For example google maps.
        /// </summary>
        /// <param name="map" type="Object">The map object</param>
        /// <param name="baseLayers" type="Object">Array with base layers</param>
        /// <param name="customBaseLayers" type="Object">Array with custom defined base layers</param>

        var layers = [];
        var layer;
        var i; 
        
        if (baseLayers) {
            for (i = 0; i < baseLayers.length; i++) {
                layer = context.getBaseLayer(baseLayers[i]);
                if ($.inArray(context.CurrentCoordinateSystem, layer.supportedCoordinateSystems) >= 0)                 
                    layers.push(layer);
            }
        }
        
        //// add layer with white background
        //var emptyLayer = new OpenLayers.Layer(AnalysisPortal.Resources.MapWhiteBaseLayerTitle, { isBaseLayer: true }); //, displayInLayerSwitcher: false });
        //layers.push(emptyLayer);

        if (customBaseLayers) {
            for (i = 0; i < customBaseLayers.length; i++) {
                layer = eval(customBaseLayers[i].initString);
                layers.push(layer);
            }
        }

        map.addLayers(layers);                
    }

    function addBlankBaseLayer(map) {
        // add layer with white background
        var emptyLayer = new OpenLayers.Layer(
            AnalysisPortal.Resources.MapWhiteBaseLayerTitle,
            { isBaseLayer: true }); //, displayInLayerSwitcher: false }); 
        emptyLayer.supportedCoordinateSystems = ["EPSG:900913", "EPSG:4326", "EPSG:3006", "EPSG:3021"];
        map.addLayers([emptyLayer]);
    }

    context.addWmsLayer = function (map, wmsLayerSetting) {
        if (wmsLayerSetting == null)
            return;
        var wmsLayerSettings = [];
        wmsLayerSettings.push(wmsLayerSetting);
        context.addWmsLayers(wmsLayerSettings);        
    };

    context.addWmsLayers = function (map, wmsLayerSettings) {
        if (wmsLayerSettings == null)
            return;
        var layers = [];
        for (var i = 0; i < wmsLayerSettings.length; i++) {
            var wmsLayerSetting = wmsLayerSettings[i];
            //MK 2017-02-06. Commented out, because we want to show all maps. The maps that doesn't support the current coordinate system is rendered as disabled.
            //if (!($.inArray(context.CurrentCoordinateSystem, wmsLayerSetting.supportedCoordinateSystems) >= 0))
            //    continue;

            var options1 = {
                layers: wmsLayerSetting.layers.join(",")//,
                //sphericalMercator: true
            };
            var options2 = {
                isBaseLayer: true,
                visibility: false//,
                //sphericalMercator: true
            };

            if (!wmsLayerSetting.isBaseLayer) {
                options1.transparent = true;
                options2.isBaseLayer = false;
            }

            var layer = new OpenLayers.Layer.WMS(
                wmsLayerSetting.name,
                wmsLayerSetting.serverUrl,
                options1,
                options2);
            layer.supportedCoordinateSystems = wmsLayerSetting.supportedCoordinateSystems;
            layers.push(layer);
        }
        map.addLayers(layers);
    };

    context.getBaseLayer = function(baseMapId) {
        /// <summary>
        /// Gets a base layer
        /// </summary>
        /// <param name="baseMapId" type="String">BaseMap enum</param>        
        /// <returns type="Object">An OpenLayers layer</returns>    

        var layer = null;
        if (baseMapId == BaseMaps.GoogleTerrain) {
            layer = new OpenLayers.Layer.Google(AnalysisPortal.Resources.MapBaseLayerGoogleTerrain, {
                    type: google.maps.MapTypeId.TERRAIN,
                    sphericalMercator: true,
                    maxExtent: new OpenLayers.Bounds(-20037508.34, -20037508.34, 20037508.34, 20037508.34),
                    numZoomLevels: 15,
                    minZoomLevel: 1
                },
                { transitionEffect: "resize" }
            );
            layer.supportedCoordinateSystems = ["EPSG:900913"];
            return layer;
        }

        if (baseMapId == BaseMaps.GoogleStreets) {
            layer = new OpenLayers.Layer.Google(AnalysisPortal.Resources.MapBaseLayerGoogleStreets, {
                    sphericalMercator: true,
                    maxExtent: new OpenLayers.Bounds(-20037508.34, -20037508.34, 20037508.34, 20037508.34),
                    numZoomLevels: 19,
                    minZoomLevel: 1
                },
                { transitionEffect: "resize" }
            );
            layer.supportedCoordinateSystems = ["EPSG:900913"];
            return layer;
        }

        if (baseMapId == BaseMaps.GoogleHybrid) {
            layer = new OpenLayers.Layer.Google(AnalysisPortal.Resources.MapBaseLayerGoogleHybrid, {
                    type: google.maps.MapTypeId.HYBRID,
                    sphericalMercator: true,
                    maxExtent: new OpenLayers.Bounds(-20037508.34, -20037508.34, 20037508.34, 20037508.34),
                    numZoomLevels: 19,
                    minZoomLevel: 1
                }, { transitionEffect: "resize" }
            );
            layer.supportedCoordinateSystems = ["EPSG:900913"];
            return layer;
        }
     
        if (baseMapId == BaseMaps.TopoWebMap) {
            layer = new OpenLayers.Layer.WMS(
                'Topografiska webbkartan',
                'http://pandora.slu.se:8080/geoserver/slu/wms',
                {
                    layers: 'slu:topowebbkartan' //,
                    //version: "1.0.0"
                },
                {}
            );
            layer.supportedCoordinateSystems = ["EPSG:900913", "EPSG:4326", "EPSG:3006", "EPSG:3021"];
            
            return layer;
        }

        if (baseMapId == BaseMaps.Lanskarta) {
            layer = new OpenLayers.Layer.WMS(
                'Artdatabankens länskarta',
                'http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wms',
                {
                    layers: 'artdatabankenslanskarta' //,
                    //layers: 'lansgranser' //,
                    //version: "1.0.0"
                },
                {}
            );
            layer.supportedCoordinateSystems = ["EPSG:900913", "EPSG:4326", "EPSG:3006", "EPSG:3021"];

            return layer;
        }   

        return layer;
    };

    context.setWfsLayerStyle = function(wfsLayer, wfsColor) {
        var fillColor = '#336699';
        if (!wfsColor == "") {
            fillColor = wfsColor;            
        }

        var defaultStyle = new OpenLayers.Style({
            strokeWidth: 1,
            strokeColor: "#333333",
            fillColor: fillColor,
            fillOpacity: .6,
            pointRadius: 4
        });

        var selectStyle = new OpenLayers.Style({
            strokeWidth: 2,
            strokeColor: "#333333",
            fillColor: '#19334D',
            fillOpacity: .6,
            pointRadius: 6
        });

        var hoverStyle = new OpenLayers.Style({
            strokeWidth: 2,
            strokeColor: "#333333",
            fillColor: "#29527A",
            //fillColor: '#3D7AB8',
            fillOpacity: .6,
            pointRadius: 6
        });

        var wfsLayerStyleMap = new OpenLayers.StyleMap({ "default": defaultStyle, "select": selectStyle, "temporary": hoverStyle });
        wfsLayer.styleMap = wfsLayerStyleMap;
   
        //        nyStyle.addRules([ruleParent, ruleChild, rulePrivate, ruleChildLess, ruleCluster]);
        //        var observationsLayerStyleMap = new OpenLayers.StyleMap({ "default": nyStyle, "select": { pointRadius: 6, strokeColor: "red", strokeWidth: 2} }); //OpenLayers.Util.applyDefaults({ fillColor: "yellow", fillOpacity: 1, strokeColor: "black", pointRadius: 5 },
        //        observationsLayer.styleMap = observationsLayerStyleMap;
    };

  function setObservationsLayerStyle(observationsLayer) {
        /// <summary>
        /// Sets styling rules for the observations layer.
        /// </summary>
        /// <param name="observationsLayer">The observations layer.</param>        
      

      var defaultStyle = new OpenLayers.Style({
          pointRadius: 5,
          fillColor: '#ffff00',
          fillOpacity: 1,
          strokeColor: "black",
          strokeWidth: 1,
      });

      var hoverStyle = new OpenLayers.Style({
          pointRadius: 6,
          strokeColor: "black",
          strokeWidth: 2,
          fillColor: '#FFFF5A'
      });

      var selectStyle = new OpenLayers.Style({
          pointRadius: 6,
          strokeColor: "#D70404",
          strokeWidth: 2
      });

      var observationsLayerStyleMap = new OpenLayers.StyleMap({ "default": defaultStyle, "temporary": hoverStyle, "select": selectStyle });
      observationsLayer.styleMap = observationsLayerStyleMap;
    }

    function setPrecisionLayerStyle(map, precisionLayer) {
        /// <summary>
        /// Sets styling rules for the precisions layer.
        /// </summary>
        /// <param name="map">The map object.</param>
        /// <param name="observationsLayer">The precision layer.</param>        
        
        var layerStyleContext = {
            getSize: function (feature) {
                if (feature.geometry.toString().substr(0, 4) == "LINE") { return 0; }
                return (feature.attributes.accuracy * 2834 / map.getScale()); // todo handle other projections than Google Mercator
            }
        };
        var precisionLayerStyleTemp = {
            strokeColor: 'black',
            strokeWidth: 1,
            strokeOpacity: 0.9,
            fillColor: 'green',
            fillOpacity: 0.1,
            pointRadius: "${getSize}"
        };

        var precisionLayerStyle = new OpenLayers.Style(precisionLayerStyleTemp, { context: layerStyleContext });
        var precisionLayerStyleMap = new OpenLayers.StyleMap({ 'default': precisionLayerStyle });
        precisionLayer.styleMap = precisionLayerStyleMap;
    }
    
    context.getDefaultGeoJSONDataObjectKeyValuePairArray = function() {        
        var strGeoJSON = '{}';
        //var strGeoJSON = '{"data":"data"}';
        var obj = Ext.decode(strGeoJSON);
        var pairs = [];
        for(var propertyName in obj) {
            var val = obj[propertyName];
            pairs.push({ Key: propertyName, Value: val });               
        }        
        return pairs;
    };
    

    context.getAllObservationsAsGeoJSON = function(map, callback) {
        /// <summary>
        /// Gets all observations that the user has selected.
        /// The taxa that the user has selected is stored in session state on the server.
        /// The server returns the observations as GeoJSON objects.
        /// </summary>
        /// <param name="map">The map object.</param>        
        getObservationsRequest = AnalysisPortal.makeAjaxCall({
            url: AnalysisPortal.ApplicationPath + "/Result/GetObservationsAsGeoJSON",
                showWaitMessage: true,
                waitMessage: AnalysisPortal.Resources.SharedLoadingData,
                waitMessageDivId: 'mapControl',
                allowMultipleRequests: false,
                requestObject: getObservationsRequest
            }, 
            function(result) {                                 
                context.clearLayer(map.theObservationsLayer);
                context.clearLayer(map.thePrecisionLayer);
                addFeaturesToMap(map, result.data, map.theObservationsLayer);        
                
                if (result.data.spatialFilter == null || result.data.spatialFilter.length == 0)
                    map.removeLayer(map.theEditLayer);
                // Check what filter is selceted and then get filter as GeoJson
                if (result.data.spatialFilter != null) {
                    // Must create layer if not exist
                    if (map.theEditLayer == null || map.theEditLayer == 'undefined') {
                        addEditVectorLayer(map);
                    }
                    context.addFeaturesToLayerFromGeoJson(map.theEditLayer, result.data.spatialFilter, {clearLayer: true});
                }
//                if (map.theEditLayer != null && map.theEditLayer != 'undefined') {
//                    map.theEditLayer.setVisibility(false);   
                //                }
               
                if (callback && typeof(callback) === "function") {
                    callback(result.data.points); 
                }
            }
        );
    };

    context.addTheFeaturesToMap = function(map, geoJson) {
        /// <summary>
        /// Add GeoJSON objects to map
        /// </summary>
        /// <param name="map">The map object.</param>
        /// <param name="geoJson">GeoJSON object containing the observations.</param>        
       
        var myObject = geoJson; // eval(results);        
        var format = new OpenLayers.Format.GeoJSON();
        
        var features = format.read(myObject.points);
        var polfeatures = format.read(myObject.polygons);
        var precisionfeatures = format.read(myObject.points);

        if ((polfeatures != null && polfeatures.length > 0) || (features != null && features.length > 0) || (precisionfeatures != null && precisionfeatures.length > 0)) {
            var observationLayersExist = false;
            for (var i = 0; i < map.layers.length; i++) {
                if (map.layers[i].vectorLayerEnum == VectorLayers.Observations) {
                    observationLayersExist = true;
                }
            }
            if (!observationLayersExist) {
                addObservationVectorLayers(map);
            }

            var layer = map.theObservationsLayer;
            if (polfeatures != null) {
                layer.addFeatures(polfeatures, { silent: true });
            }
            if (features != null) {
                layer.addFeatures(features, { silent: true });
            }
            if (precisionfeatures != null) {
                for (i in precisionfeatures) {
                    if (precisionfeatures[i].attributes.accuracy > 0) {
                        map.thePrecisionLayer.addFeatures([precisionfeatures[i]]);
                    }
                }
            }
        }
    };

    function addFeaturesToMap(map, geoJson) {
        /// <summary>
        /// Add GeoJSON objects to map
        /// </summary>
        /// <param name="map">The map object.</param>
        /// <param name="geoJson">GeoJSON object containing the observations.</param>        
                
        var myObject = geoJson; // eval(results);
        
        var features = new OpenLayers.Format.GeoJSON().read(myObject.points);
        var polfeatures = new OpenLayers.Format.GeoJSON().read(myObject.polygons);
        var precisionfeatures = new OpenLayers.Format.GeoJSON().read(myObject.points);
        
        if((polfeatures != null && polfeatures.length > 0) || (features != null && features.length > 0) || (precisionfeatures != null && precisionfeatures.length > 0))
        {
            var observationLayersExist = false;
            for (var i = 0; i < map.layers.length; i++) {
                if(map.layers[i].vectorLayerEnum == VectorLayers.Observations) {
                    observationLayersExist = true;
                }
            }
            if (!observationLayersExist) {
                addObservationVectorLayers(map);
            }
                
            var layer = map.theObservationsLayer;
            if (polfeatures != null) {
                layer.addFeatures(polfeatures, { silent: true });
            }
            if (features != null) {
                layer.addFeatures(features, { silent: true });
            }
            if (precisionfeatures != null) {
                for (i in precisionfeatures) {
                    if (precisionfeatures[i].attributes.accuracy > 0) {
                        map.thePrecisionLayer.addFeatures([precisionfeatures[i]]);
                    }
                }
            }
        }
    }    
    
    context.getMapExtent = function(map) {
        /// <summary>
        /// Gets the map extents as a string
        /// </summary>
        /// <param name="map">The map object.</param>
                
        return map.getProjection() + ';' + map.getExtent().toBBOX();
    };

    context.setMapExtent = function(map, value) {
        /// <summary>
        /// Sets the map extents as a string
        /// </summary>
        /// <param name="map">The map object.</param>
        /// <param name="value" type="String">The new map extents as a string</param>                
        var element = value.split(';');
        var newBounds = OpenLayers.Bounds.fromString(element[1]);
        map.zoomToExtent(newBounds, false);
        return true;
    };

    context.setMapExtentByCoords = function(map, xcoord, ycoord) {
        /// <summary>
        /// Sets the map extents from coordinates
        /// </summary>
        /// <param name="map">The map object.</param>
        /// <param name="xcoord">X coordinate.</param>
        /// <param name="ycoord">Y coordinate.</param>                
        var xoffset = 5000;
        var minX = parseInt(xcoord) - xoffset;
        var maxX = parseInt(xcoord) + xoffset;
        var yoffset = 6000;
        var minY = parseInt(ycoord) - yoffset;
        var maxY = parseInt(ycoord) + yoffset;
        setMapExtent(map, 'dummy;' + minX + ',' + minY + ',' + maxX + ',' + maxY);
    };

    context.getSpatialFilterAsGeoJSON = function(map, options, callback) {
        /// <summary>
        /// Gets the spatial filter
        /// </summary>
        /// <param name="map">The map object.</param>  
        /// <param name="options">
    	///     waitMessageDivId {String} Name of the Div where the wait message will be centered over.    	
        /// </param>
        var settings = $.extend(true, {
            waitMessageDivId: 'mapControl'            
        }, options);          
        
        AnalysisPortal.makeAjaxCall({
                url: AnalysisPortal.ApplicationPath + "/Filter/GetSpatialFilterAsGeoJSON",            
                showWaitMessage: true,
                waitMessage: AnalysisPortal.Resources.SharedLoadingData,
                waitMessageDivId: settings.waitMessageDivId // map.div.parentElement.id // 'mapControl'                
            }, 
            function(result) {
                context.addFeaturesToLayerFromGeoJson(map.theEditLayer, result.data, {clearLayer: true});                
                if (callback && typeof(callback) === "function") {
                    callback(); 
                }
            }
        );                
    };

    context.getSpatialFilterFeaturesAsGeoJson = function (map, options, callback) {
        /// <summary>
        /// Gets the spatial filter
        /// </summary>
        /// <param name="map">The map object.</param>  
        /// <param name="options">
        ///     waitMessageDivId {String} Name of the Div where the wait message will be centered over.    	
        /// </param>
        var settings = $.extend(true, {
            waitMessageDivId: 'mapControl'
        }, options);
     
        AnalysisPortal.makeAjaxCall({
            url: AnalysisPortal.ApplicationPath + "/Filter/GetSpatialFilterAsGeoJSON",
            showWaitMessage: true,
            waitMessage: AnalysisPortal.Resources.SharedLoadingData,
            waitMessageDivId: settings.waitMessageDivId // map.div.parentElement.id // 'mapControl'                
        },
            function (result) {                
                if (callback && typeof (callback) === "function") {
                    callback(result.data);
                }
            }
        );
    };

    context.clearLayer = function(layer) {
    	/// <summary>
    	/// Removes all features from a layer.
    	/// </summary>
    	/// <param name="layer">The layer.</param>
        if (!layer)
            return;
        if ((layer.features != null) && (layer.features != 'undefined')) {
            if (!isEmpty(layer.features)) {
                layer.destroyFeatures();
            }
        }   
    };

    context.addFeaturesToLayerFromGeoJson = function(layer,strJson, options) {
    	/// <summary>
    	/// Add features from a GeoJSON string to a layer.
    	/// </summary>    	
    	/// <param name="layer">The layer which the features are added to.</param>
    	/// <param name="strJson">The GeoJSON string.</param>
    	/// <param name="options">
    	///     clearLayer {Bool} If true delete all features from layer. Defaults to true.
    	///     silent {Bool} If true, no add-events will be triggered. Defaults to true.
        /// </param>

        var settings = $.extend(true, {
            clearLayer: true,
            silent: true
        }, options);        
        
        if (settings.clearLayer)
            context.clearLayer(layer);
        
        var geoJsonFormat = context.createGeoJsonFormat();        
        var features = geoJsonFormat.read(strJson);        

        if (features != null) {
            layer.addFeatures(features, { silent: settings.silent });
        }
    };

        
    context.saveSpatialFilter = function(callback, options) {
        /// <summary>
        /// Saves the current polygons in theEditLayer as spatial filter
        /// </summary>
        /// <param name="callback">This function gets called when this function succeeds</param>
        var settings = $.extend(true, {
            async: true
        }, options);
        
        if (map.editFeatureControl)
            map.editFeatureControl.deactivate();
        var serializer = new OpenLayers.Format.GeoJSON();
        var json = serializer.write(map.theEditLayer.features);

        AnalysisPortal.makeAjaxCall({
                url: AnalysisPortal.ApplicationPath + "/Filter/UpdateSpatialFilter",
                params: {
                    geojson: json
                },
                async: settings.async,
                waitMessage: AnalysisPortal.Resources.SharedSaving,
            },
            function(result) {
                if (settings.async)
                    AnalysisPortal.showMsg(result.msg);
                if (callback && typeof(callback) === "function") {                             
                    callback();
                }                
            }
        );               
    };

    context.clearSpatialFilter = function(callback) {
        /// <summary>
        /// Clears the spatial filter. Removes all polygons from theEditLayer.
        /// </summary>
        /// <param name="callback">This function gets called when this function succeeds</param>

        AnalysisPortal.makeAjaxCall({
                url: AnalysisPortal.ApplicationPath + "/Filter/ClearSpatialFilter",
                waitMessage: AnalysisPortal.Resources.SharedLoadingData,
                method: 'POST'
            },
            function(result) {
                context.clearLayer(map.theEditLayer);                            
                //AnalysisPortal.showMsg(result.msg);
                if (callback && typeof(callback) === "function") {
                    callback(result);
                }
            }
        );
    };
    
    context.getWfsLayerFeaturesAndAddToLayer = function(id, layer, callback) {
        /// <summary>
        /// Gets features from a WFS layer and adds them to a layer
        /// </summary>        
        /// <param name="id">The id of the layer in MySettings</param>
        /// <param name="layer">The layer the features will be added to.</param>                    
        context.showWaitMsg(AnalysisPortal.Resources.SharedLoadingData);
        try {
            context.getWfsLayerSettings(id, function(layerSettings) {
                context.getWfsLayerFeatures(layerSettings, function(features) {                    
                    var vectors = convertFeatures(features, layerSettings);
                    layer.addFeatures(vectors);
                    if(layer.vectorLayerEnum == VectorLayers.WFSUserDefined) {
                        for (var i = 0; i < layer.features.length; i++) {
                            layer.Identifier = layer.Identifier + " " + layer.features[i].id;
                        }
                    }
                    context.hideWaitMsg();
                    if (callback && typeof(callback) === "function")
                        callback();
                });
            });
        } catch(err) {
            console.log('Error in getWfsLayerFeaturesAndAddToLayer()');
            context.hideWaitMsg();
        }
    };           

    context.getWfsLayerSettings = function(id, callback) {
    /// <summary>
    /// Gets a WFS layer setting
    /// </summary>
    /// <param name="map">The map object or the id of the map div.</param>
    /// <param name="id">The id of layer defined in calling function.</param>
    /// <param name="layer">The layer is the selected layer to be shown.</param>
    /// <param name="callback">Callback function</param>
        
        AnalysisPortal.makeAjaxCall({
        //Get settings for selected wfs layer
                url: AnalysisPortal.ApplicationPath + "/Data/GetWfsLayerSettings",
                params: {
                    id: id
                },
                showWaitMessage: false                
            },
            // If result is ok get the rest of the layer information ie add layer features from defined source (using url, typename etc.)
            function(result) {
                if (callback && typeof(callback) === "function") 
                    callback(result.data);
            }
        );              
    };

    context.getWfsLayerFeatures = function(layerSettings, callback) {
    	/// <summary>
    	/// Makes a request to a WFS web service and returns the features in the callback
    	/// </summary>
    	/// <param name="layerSettings">The WFS layer settings.</param>
    	/// <param name="callback">A callback function. Returns: callback(features).</param>
        var typeName = parseWfsTypeName(layerSettings.TypeName);            
        var wfsFilter = AnalysisPortal.WFS.Base.parseFilterFromXmlString(layerSettings.Filter);
        var wfsVersion = AnalysisPortal.WFS.Base.wfsVersion;
        var featureType = typeName.name;
        var featurePrefix = typeName.namespace;
        var geometryName = layerSettings.GeometryName;        
        var wfsProtocol = AnalysisPortal.WFS.Base.createWFSProtocol(wfsVersion, layerSettings.ServerUrl, featurePrefix, featureType, geometryName, map.projection);
        var filter = AnalysisPortal.WFS.Base.createWFSFilter(wfsFilter.WFSRepresentation(), wfsVersion);
        var maxNrFeatures = 10000;
        
        if (layerSettings.UseSpatialFilterExtentAsBoundingBox) {
            AnalysisPortal.makeAjaxCall({
                url: AnalysisPortal.ApplicationPath + "/Filter/GetSpatialFilterBboxAsGeoJSON"
                },
                function (result) {
                    var bboxFeatureCollection = result.data;
                    var bbox = null;
                    if (bboxFeatureCollection != null && bboxFeatureCollection.features.length > 0) {
                        var coords = bboxFeatureCollection.features[0].geometry.coordinates[0];
                        bbox = new OpenLayers.Bounds([coords[0][0], coords[0][1], coords[2][0], coords[2][1]]);
                    }
                    AnalysisPortal.WFS.Base.makeWFSGetFeaturesRequest(wfsProtocol, filter, maxNrFeatures, bbox, function (features, count) {
                        if (callback && typeof (callback) === "function") {
                            callback(features);
                        }
                    });
                });
        } else {            
            AnalysisPortal.WFS.Base.makeWFSGetFeaturesRequest(wfsProtocol, filter, maxNrFeatures, null, function(features, count) {
                if (callback && typeof(callback) === "function") {
                    callback(features);
                }
            });
        }
    };

    function convertFeatures(features) {
        /// <summary>
        /// Converts MultiPolygon features to Polygon features.
        /// All other types of features are included in the result but not converted.
        /// </summary>
        /// <param name="features">The features.</param>    	
        var vectors = [];
        if (features) {
            for (var i = 0; i < features.length; i++) {
                if (features[i].geometry != null && features[i].geometry.CLASS_NAME == "OpenLayers.Geometry.MultiPolygon" && features[i].geometry.components != null) {
                    for (var j = 0; j < features[i].geometry.components.length; j++) {
                        var geom = features[i].geometry.components[j];
                        var feature = new OpenLayers.Feature.Vector(geom, features[i].attributes);
                        //var feature = new OpenLayers.Feature.Vector(geom);
                        vectors.push(feature);
                    }
                } else {
                    vectors.push(features[i]);
                }
            }

        }
        return vectors;
    }

    function parseWfsTypeName(typeName) {
    	/// <summary>
    	/// Parses a typename into two parts. The namespace and the name.
    	/// </summary>
    	/// <param name="typeName">The typename.</param>    	
        var nsIndex = typeName.indexOf(":");
        var ns = typeName.substr(0, nsIndex);
        var name = typeName.substr(nsIndex + 1, typeName.length - nsIndex - 1);
        return {
            name : name,
            namespace : ns
        };            
    }

    context.getAllWfsFilters = function(callback) {
        /// <summary>
        /// Gets all wfs layers that the user has created from server
        /// </summary>    
        /// <param name="callback">Callback function - list of all available vector layers</param>    
        AnalysisPortal.makeAjaxCall({
                url: AnalysisPortal.ApplicationPath + "/Result/GetWfsLayersAsJSON",
                waitMessage: AnalysisPortal.Resources.SharedLoadingData,
                waitMessageDivId: 'mapControl'                
            },
            function(result) {                
                var layers = [];
                var mapLayers = result.data;
                for (var i = 0; i < mapLayers.length; i++) 
                {
                    var wfsObj = {
                        id: mapLayers[i].Id,
                        name: mapLayers[i].Name,
                        visible: false,
                        color: mapLayers[i].Color
                    };                
                    layers.push(wfsObj);
                }
                // Return array of all layers         
                if (callback && typeof(callback) === "function")
                    callback(layers);

            }
        );      
    };

    context.activateSpatialFilterLayer = function (map, layer) {
        if (typeof layer.isLoaded === 'undefined' || layer.isLoaded == false) {
            context.getSpatialFilterFeaturesAsGeoJson(map, null, function (features) {
                context.addFeaturesToLayerFromGeoJson(layer, features, { clearLayer: true });
                layer.isLoaded = true;
            });
        }        
    };


    context.activateWfsLayer = function(map, layer) {
        var features = layer.features;
        var featureExist = false;
        if (features != null && features != 'undefined') {
            for (var i = 0; i < features.length; i++) {
                var featureId = features[i].id;
                if (layer.Identifier.indexOf(featureId) !== -1) {
                    featureExist = true;
                    break;
                }
            }
        }
        // Feature don't exist in layer, must add it
        if (!featureExist) {
            context.getWfsLayerFeaturesAndAddToLayer(layer.id, layer);
        }
    };

    context.zoomToObservationById = function(map, id, select) {
        /// <summary>
        /// Centers the map around the observation
        /// </summary>
        /// <param name="map">The map object.</param>
        /// <param name="id">Observation id.</param>
        /// <param name="select" type="Boolean">If true the observation will be selected.</param>
                
        map.featureToSelectId = id;
        var feature = map.theObservationsLayer.getFeatureByFid(id);
        var centroid = feature.geometry.getCentroid();
        map.panTo(new OpenLayers.LonLat(centroid.x, centroid.y));
        if (select) {
            // deselect all selected
            map.selectControl.unselectAll();
            map.skipSelectEvent = true;
            map.selectControl.select(feature);
            var e = new Object();
            e.feature = feature;
            showFeaturePopup(map, e);
        }
    };

    context.getProjectedSwedenExtentBoundingBox = function () {
        /// <summary>
        /// Gets sweden bounding box in the currently used coordinate system.
        /// </summary>
        var srid = context.CurrentCoordinateSystem;
        var baseProjection = new OpenLayers.Projection(srid);
        var bounds = new OpenLayers.Bounds(swedenExtentWGS84).transform(new OpenLayers.Projection('EPSG:4326'), baseProjection);
        return bounds;
    };

    context.zoomToObservationByCoordinate = function(map, xcoord, ycoord) {
        /// <summary>
        /// Centers the map around the given coordinates.
        /// </summary>
        /// <param name="map">The map object.</param>
        /// <param name="xcoord">X coordinate.</param>
        /// <param name="ycoord">Y coordinate.</param>
                
        map.panTo(new OpenLayers.LonLat(xcoord, ycoord));
    };
    
    
    context.zoomToObservationByIdOrCoordinate = function(map, id, xcoord, ycoord) {
        /// <summary>
        /// Centers the map around the observation, and if it wasn't found the
        /// map is centered around the given coordinates.
        /// </summary>
        /// <param name="map">The map object.</param>
        /// <param name="id">Observation id.</param>
        /// <param name="xcoord">X coordinate.</param>
        /// <param name="ycoord">Y coordinate.</param>
                
        if (map.getZoom() < 13) {
            map.zoomTo(13);
        }
        var feature = map.theObservationsLayer.getFeatureByFid(id);
        if (feature != null) {
            zoomToObservationById(map, id, true);
        } else {
            map.featureToSelectId = id;
            map.panTo(new OpenLayers.LonLat(xcoord, ycoord));
        }
    };

    // Check if it is possible to zoom more
    context.isMaxZoom = function (map) {
        var currentResolution = map.getResolution();
        var zoomOneStepResolution = map.baseLayer.getResolutionForZoom(map.getZoom() + 1);
        return Math.abs(zoomOneStepResolution - currentResolution) < 0.0001;
        //return zoomOneStepResolution === currentResolution;
    }

    context.getWmsCapabilities = function (serverUrl, callback) {
        if (serverUrl == null)
            serverUrl = "http://gpt.vic-metria.nu/wmsconnector/com.esri.wms.Esrimap/luft?service=WMS&request=getCapabilities&version=1.1.1";
        serverUrl += "?service=WMS&request=getCapabilities&version=1.1.1";
        OpenLayers.Request.GET({
            url: serverUrl,
            success: function (e) {
                var wms = new OpenLayers.Format.WMSCapabilities.v1_1_1();
                var response = wms.read(e.responseText);
                
                if (callback && typeof(callback) === "function") {
                    callback(response);
                }
            },
            failure: function (e) {
                AnalysisPortal.showToasterMsg("Couldn't access WMS server " + serverUrl, "Error", {
                    messageType: "error",
                    timeout: 6000
                });
            }
        });
    };

    context.createEqualRule = function (value, propertyName, options) {
        var settings = $.extend(true, {
            fillColor: '#2F88BB',
            fillOpacity: 1,
            strokeOpacity: 0,
            strokeWidth: 1,
            pointRadius: 14,
            strokeColor: '#000000'
        }, options);

        var betweenRule = new OpenLayers.Rule({
            filter: new OpenLayers.Filter.Comparison({
                type: OpenLayers.Filter.Comparison.EQUAL_TO,
                property: propertyName,
                value: value                
            }),
            symbolizer: {
                fillColor: settings.fillColor,
                fillOpacity: settings.fillOpacity,                
                pointRadius: settings.pointRadius,                
                strokeColor: settings.strokeColor,
                strokeWidth: settings.strokeWidth,
                strokeOpacity: settings.strokeOpacity,
                stroke: true

                //fillColor: fillColor,
                //fillOpacity: 1 - gridCellOpacity,
                //pointRadius: 14,
                //strokeColor: '#C3C3C3',
                //strokeWidth: 1,
                //strokeOpacity: 1,
                //stroke: true
            }
        });
        return betweenRule;
    };
        
    context.createBetweenRule = function(lowerValue, upperValue, propertyName, options) {
        var settings = $.extend(true, {
            fillColor: '#2F88BB',
            fillOpacity: 1,            
            strokeOpacity: 0,
            strokeWidth: 1,
            pointRadius: 14,
            strokeColor: '#000000'
        }, options);

        var betweenRule = new OpenLayers.Rule({
            filter: new OpenLayers.Filter.Comparison({
                type: OpenLayers.Filter.Comparison.BETWEEN,
                property: propertyName,
                lowerBoundary: lowerValue,
                upperBoundary: upperValue
            }),
            symbolizer: {
                fillColor: settings.fillColor,
                fillOpacity: settings.fillOpacity,                
                pointRadius: settings.pointRadius,                
                strokeColor: settings.strokeColor,
                strokeWidth: settings.strokeWidth,
                strokeOpacity: settings.strokeOpacity,
                stroke: true
            }
        });
        return betweenRule;
    };

    context.createGreaterThanRule = function(value, propertyName, options) {
        var settings = $.extend(true, {
            fillColor: '#2F88BB',
            fillOpacity: 1,
            strokeOpacity: 0,
            strokeWidth: 1,
            pointRadius: 14,
            strokeColor: '#000000'
        }, options);

        var greaterThanRule = new OpenLayers.Rule({
            filter: new OpenLayers.Filter.Comparison({
                type: OpenLayers.Filter.Comparison.GREATER_THAN,
                property: propertyName,
                value: value
            }),
            symbolizer: {
                fillColor: settings.fillColor,
                fillOpacity: settings.fillOpacity,                
                pointRadius: settings.pointRadius,                
                strokeColor: settings.strokeColor,
                strokeWidth: settings.strokeWidth,
                strokeOpacity: settings.strokeOpacity,
                stroke: true

                //fillColor: fillColor,
                //fillOpacity: 1 - gridCellOpacity,
                //pointRadius: 14,
                //strokeColor: '#C3C3C3',
                //strokeWidth: 1,
                //strokeOpacity: 1,
                //stroke: true
            }
        });
        return greaterThanRule;
    };

    context.createLessThanRule = function(lessThanValue, propertyName, fillColor, gridCellOpacity) {
        var lessThanRule = new OpenLayers.Rule({
            filter: new OpenLayers.Filter.Comparison({
                type: OpenLayers.Filter.Comparison.LESS_THAN_OR_EQUAL_TO,
                property: propertyName,
                value: lessThanValue
            }),
            symbolizer: {
                fillColor: fillColor,
                fillOpacity: 1 - gridCellOpacity,
                pointRadius: 14,
                strokeColor: '#C3C3C3',
                strokeWidth: 1,
                strokeOpacity: 1,
                stroke: true
            }
        });
        return lessThanRule;
    };

    context.createGridStatisticsLegendTable = function(histogram) {        
        var tbl = $('<table></table>').attr({ 'class': "table grid-statistics-table", style: "width: 100px;" });
        var rowTemplate = '<tr><td style="cursor:default; background-color: {0}" title="{2}">{1}</td></tr>';
        for (var i = 0; i < histogram.bins.length; i++) {
            var bin = histogram.bins[i];
            
            var str = Ext.String.htmlEncode(bin.toString());
            var colorString = bin.color.toHexString();
            var strTooltip = "";
            if (Math.abs(bin.relativeFrequency - bin.relativeFrequencyToAllItemsPlacedInBins) > 0.00001) {
                strTooltip = Ext.String.format(AnalysisPortal.Resources.SharedCount + ": {0}, " + AnalysisPortal.Resources.HistogramBinPercentOfVisibleItems + ": {1} %, " + AnalysisPortal.Resources.HistogramBinPercentOfAllItems + ": {2} %", bin.frequency, Ext.util.Format.number(bin.relativeFrequencyToAllItemsPlacedInBins * 100, '0.00'), Ext.util.Format.number(bin.relativeFrequency * 100, '0.00'));
            } else {
                strTooltip = Ext.String.format(AnalysisPortal.Resources.SharedCount + ": {0}, " + AnalysisPortal.Resources.SharedPercent + ": {1} %", bin.frequency, Ext.util.Format.number(bin.relativeFrequency * 100, '0.00'));
            }
            //strTooltip = Ext.String.format(AnalysisPortal.Resources.SharedCount + ": {0}, " + AnalysisPortal.Resources.SharedPercent + ": {1} %", bin.frequency, Ext.util.Format.number(bin.relativeFrequency * 100, '0.00'));
            var sf = Ext.String.format(rowTemplate, colorString, str, strTooltip);
            $(sf).appendTo(tbl);
        }
        return tbl;
    };

    context.calculateHistogramWithPredefinedBins = function(gridCells, propertyName, bins, startColor, endColor) {
        var values = [];
        for (var i = 0; i < gridCells.length; i++) {
            var cell = gridCells[i];
            values.push(cell[propertyName]);
        }
        var sortedValues = values.sort(function (a, b) { return a - b; });
        
        var histogram = AnalysisPortal.Statistics.Histogram.calculateHistogramWithPredefinedBins(sortedValues, bins);
        //var colors = new Array(startColor, endColor);
        //var gradientColorCollection = AnalysisPortal.Statistics.GradientColorCollection.createGradientColorCollection(0, 1, colors); //0,1 is dummy values. Not used in this case when creating interpolated color.
        //histogram.calculateHistogramBinColors(gradientColorCollection);
        return histogram;
    };
    
    context.calculateHistogram = function(gridCells, propertyName, interpolationMode) {
        var values = [];
        for (var i = 0; i < gridCells.length; i++) {
            var cell = gridCells[i];
            values.push(cell[propertyName]);
        }
        var sortedValues = values.sort(function (a, b) { return a - b; });
        var minPercentile = 1.0 / 7.0;
        var maxPercentile = 1.0 - minPercentile;
        var percentiles = AnalysisPortal.Statistics.Histogram.getPercentiles(sortedValues, minPercentile, maxPercentile);
        //var percentiles = AnalysisPortal.Statistics.Histogram.getPercentiles(sortedValues, 0.1, 0.9);
        var nrBins = AnalysisPortal.Statistics.Histogram.calcNumberOfBins(values, percentiles[1] - percentiles[0]);
        //var histogram = AnalysisPortal.Statistics.Histogram.calculateIntegerBoundedHistogram(sortedValues, percentiles[0], percentiles[1], nrBins);
        var histogram = AnalysisPortal.Statistics.Histogram.calculateIntegerBoundedHistogram(sortedValues, nrBins);
        histogram.colorInterpolationMode = interpolationMode;
        nrBins = histogram.bins.length;
        //var gradientColorCollection = AnalysisPortal.Statistics.GradientColorCollection.createGradientColorCollection(percentiles[0], percentiles[1]);
        //var colors = new Array(new AnalysisPortal.Statistics.Color(255, 255, 255), new AnalysisPortal.Statistics.Color(255, 0, 128));
        // Colorblind blue color is selected...
        var colors = new Array(new AnalysisPortal.Statistics.Color(255, 255, 255), new AnalysisPortal.Statistics.Color(0, 117, 178));
        var gradientColorCollection = AnalysisPortal.Statistics.GradientColorCollection.createGradientColorCollection(percentiles[0], percentiles[1], colors);
        histogram.calculateHistogramBinColors(gradientColorCollection);
        return histogram;
    };

    context.addHistogram = function(layerEnum, attribute, histogram) {
        histograms[layerEnum] = { attribute: attribute, histogram: histogram };
    };

    context.getHistograms = function() {
        return histograms;
    };

    context.createGridStatisticsStyleRules = function(histogram, propertyName, gridCellOpacity, gridCellBorderOpacity) {        
        var nrBins = histogram.bins.length;        
        var rules = [];
        var temporaryBetweenRules = [];
        var selectBetweenRules = [];
        // one rule for each bin and each mode (default, hover, selected)
        for (var i = 0; i < nrBins; i++) {
            var color = histogram.bins[i].color.toHexString();
            var betweenRule = AnalysisPortal.GIS.createBetweenRule(histogram.bins[i].lowerBound, histogram.bins[i].upperBound, propertyName, {
                fillColor: color,
                fillOpacity: 1 - gridCellOpacity,                
                strokeOpacity: gridCellBorderOpacity                
            });
            rules.push(betweenRule);
            
            var temporaryBetweenRule = AnalysisPortal.GIS.createBetweenRule(histogram.bins[i].lowerBound, histogram.bins[i].upperBound, propertyName, {
                fillColor: color,
                fillOpacity: 0.2, //1 - gridCellOpacity,
                strokeOpacity: 1,
                strokeWidth: 2,
                strokeColor: '#FFFF00'
            });
            temporaryBetweenRules.push(temporaryBetweenRule);
            
            var selectBetweenRule = AnalysisPortal.GIS.createBetweenRule(histogram.bins[i].lowerBound, histogram.bins[i].upperBound, propertyName, {
                fillColor: color,
                fillOpacity: 0, //1 - gridCellOpacity,
                strokeOpacity: 1,
                strokeWidth: 2,
                strokeColor: '#FFFF00'
            });
            selectBetweenRules.push(selectBetweenRule);            
        }
      
        var defaultStyle = new OpenLayers.Style({
            strokeWidth: 1,            
            fillColor: '#669933',
            fillOpacity: .6,
            pointRadius: 4
        });
        defaultStyle.addRules(rules);

        var selectStyle = new OpenLayers.Style({
            fillColor: '#669933',
            fillOpacity: 0,            
            strokeWidth: 2,            
        });
        selectStyle.addRules(selectBetweenRules);

        var temporaryStyle = new OpenLayers.Style({
            strokeWidth: 3            
        });
        temporaryStyle.addRules(temporaryBetweenRules);

        return new OpenLayers.StyleMap({
            'default': defaultStyle,
            'select': selectStyle,
            'temporary': temporaryStyle
        });
    };
    
    context.createGridStatisticsStyleRulesPoint = function (histogram, propertyName, gridCellOpacity, gridCellBorderOpacity) {
        var nrBins = histogram.bins.length;
        var rules = [];
        var temporaryBetweenRules = [];
        var selectBetweenRules = [];
        var minSize = 7;
        // one rule for each bin and each mode (default, hover, selected)
        for (var i = 0; i < nrBins; i++) {
            var color = histogram.bins[i].color.toHexString();
            //var color = '#ffff00'; // yellow point color                       
            var betweenRule = AnalysisPortal.GIS.createBetweenRule(histogram.bins[i].lowerBound, histogram.bins[i].upperBound, propertyName, {
                fillColor: color,
                fillOpacity: 1 - gridCellOpacity,
                strokeOpacity: 1,
                //pointRadius: minSize // yellow point
                pointRadius: i*2 + minSize
        });
            rules.push(betweenRule);

            var temporaryBetweenRule = AnalysisPortal.GIS.createBetweenRule(histogram.bins[i].lowerBound, histogram.bins[i].upperBound, propertyName, {
                fillColor: color,
                fillOpacity: 0.2, //1 - gridCellOpacity,
                strokeOpacity: 1,
                strokeWidth: 2,
                strokeColor: '#FFFF00',
                //pointRadius: minSize // yellow point
                pointRadius: i*2 + minSize
            });
            temporaryBetweenRules.push(temporaryBetweenRule);

            var selectBetweenRule = AnalysisPortal.GIS.createBetweenRule(histogram.bins[i].lowerBound, histogram.bins[i].upperBound, propertyName, {
                fillColor: color,
                fillOpacity: 0.2, //1 - gridCellOpacity,
                strokeOpacity: 1,
                strokeWidth: 2,
                strokeColor: '#FFFF00',
                //pointRadius: minSize // yellow point
                pointRadius: i*2 + minSize
            });
            selectBetweenRules.push(selectBetweenRule);
        }

        var defaultStyle = new OpenLayers.Style({
            strokeWidth: 1,
            fillColor: '#669933',
            fillOpacity: .6,
            pointRadius: 4
        });
        defaultStyle.addRules(rules);

        var selectStyle = new OpenLayers.Style({
            fillColor: '#669933',
            fillOpacity: 0,
            strokeWidth: 2,
        });
        selectStyle.addRules(selectBetweenRules);

        var temporaryStyle = new OpenLayers.Style({
            strokeWidth: 3
        });
        temporaryStyle.addRules(temporaryBetweenRules);

        return new OpenLayers.StyleMap({
            'default': defaultStyle,
            'select': selectStyle,
            'temporary': temporaryStyle
        });
    };
          
    context.createGridStatisticsStyleRulesClusterPoint = function (histogram, propertyName, gridCellOpacity, gridCellBorderOpacity, zoomData) {        
        var nrBins = histogram.bins.length;
        var rules = [];
        var temporaryBetweenRules = [];
        var selectBetweenRules = [];
        var minSize = 5;
        // one rule for each bin and each mode (default, hover, selected)        
        
        var color = '#ffff00'; // yellow point color                       
        var oneObservationRule = AnalysisPortal.GIS.createEqualRule(1, 'ObservationCount', {
            fillColor: color,
            fillOpacity: 1 - gridCellOpacity,
            strokeOpacity: 1 - gridCellOpacity + 0.1,
            pointRadius: zoomData.pointSize
            //pointRadius: i * 2 + minSize
        });
        rules.push(oneObservationRule);

        var manyObservationsRule = AnalysisPortal.GIS.createGreaterThanRule(1, 'ObservationCount', {            
            fillColor: '#0099FF', // blue point color            
            fillOpacity: 1 - gridCellOpacity,
            strokeOpacity: 1 - gridCellOpacity + 0.1,
            pointRadius: zoomData.pointSize
            //pointRadius: i * 2 + minSize
        });
        rules.push(manyObservationsRule);

        //for (var i = 0; i < nrBins; i++) {
        //    //var color = histogram.bins[i].color.toHexString();
        //    var color = '#ffff00'; // yellow point color                       
        //    var betweenRule = AnalysisPortal.GIS.createBetweenRule(histogram.bins[i].lowerBound, histogram.bins[i].upperBound, propertyName, {
        //        fillColor: color,
        //        fillOpacity: 1 - gridCellOpacity,
        //        strokeOpacity: 1,
        //        pointRadius: zoomData.pointSize // yellow point
        //        //pointRadius: i * 2 + minSize
        //    });
        //    rules.push(betweenRule);

        //    var temporaryBetweenRule = AnalysisPortal.GIS.createBetweenRule(histogram.bins[i].lowerBound, histogram.bins[i].upperBound, propertyName, {
        //        fillColor: color,
        //        fillOpacity: 0.2, //1 - gridCellOpacity,
        //        strokeOpacity: 1,
        //        strokeWidth: 2,
        //        strokeColor: '#FFFF00',
        //        pointRadius: zoomData.pointSize // yellow point
        //        //pointRadius: i * 2 + minSize
        //    });
        //    temporaryBetweenRules.push(temporaryBetweenRule);

        //    var selectBetweenRule = AnalysisPortal.GIS.createBetweenRule(histogram.bins[i].lowerBound, histogram.bins[i].upperBound, propertyName, {
        //        fillColor: color,
        //        fillOpacity: 0.2, //1 - gridCellOpacity,
        //        strokeOpacity: 1,
        //        strokeWidth: 2,
        //        strokeColor: '#FFFF00',
        //        pointRadius: zoomData.pointSize // yellow point
        //        //pointRadius: i * 2 + minSize
        //    });
        //    selectBetweenRules.push(selectBetweenRule);
        //}

        var defaultStyle = new OpenLayers.Style({
            strokeWidth: 1,
            fillColor: '#669933',
            fillOpacity: .6,
            pointRadius: 4
        });
        defaultStyle.addRules(rules);

        var selectStyle = new OpenLayers.Style({            
            fillOpacity: 0.2,
            strokeWidth: 2,
        });
        selectStyle.addRules(selectBetweenRules);

        var temporaryStyle = new OpenLayers.Style({
            fillOpacity: 0.2,
            strokeWidth: 2
        });
        temporaryStyle.addRules(temporaryBetweenRules);

        return new OpenLayers.StyleMap({
            'default': defaultStyle,
            'select': selectStyle,
            'temporary': temporaryStyle
        });
    };

    context.createGridStatisticsStyleRulesClusterGrid = function (histogram, propertyName, gridCellOpacity, gridCellBorderOpacity, zoomData) {
        console.log(zoomData);
        var nrBins = histogram.bins.length;
        var rules = [];
        var temporaryBetweenRules = [];
        var selectBetweenRules = [];
        var minSize = 7;
        // one rule for each bin and each mode (default, hover, selected)
        for (var i = 0; i < nrBins; i++) {
            var color = histogram.bins[i].color.toHexString();
            //var color = '#ffff00'; // yellow point color                       
            var betweenRule = AnalysisPortal.GIS.createBetweenRule(histogram.bins[i].lowerBound, histogram.bins[i].upperBound, propertyName, {
                fillColor: color,
                fillOpacity: 1 - gridCellOpacity,
                strokeOpacity: 1,
                //pointRadius: minSize // yellow point
                pointRadius: i * 2 + minSize
            });
            rules.push(betweenRule);

            var temporaryBetweenRule = AnalysisPortal.GIS.createBetweenRule(histogram.bins[i].lowerBound, histogram.bins[i].upperBound, propertyName, {
                fillColor: color,
                fillOpacity: 0.2, //1 - gridCellOpacity,
                strokeOpacity: 1,
                strokeWidth: 2,
                strokeColor: '#FFFF00',
                //pointRadius: minSize // yellow point
                pointRadius: i * 2 + minSize
            });
            temporaryBetweenRules.push(temporaryBetweenRule);

            var selectBetweenRule = AnalysisPortal.GIS.createBetweenRule(histogram.bins[i].lowerBound, histogram.bins[i].upperBound, propertyName, {
                fillColor: color,
                fillOpacity: 0.2, //1 - gridCellOpacity,
                strokeOpacity: 1,
                strokeWidth: 2,
                strokeColor: '#FFFF00',
                //pointRadius: minSize // yellow point
                pointRadius: i * 2 + minSize
            });
            selectBetweenRules.push(selectBetweenRule);
        }

        var defaultStyle = new OpenLayers.Style({
            strokeWidth: 1,
            fillColor: '#669933',
            fillOpacity: .6,
            pointRadius: 4
        });
        defaultStyle.addRules(rules);

        var selectStyle = new OpenLayers.Style({
            fillColor: '#669933',
            fillOpacity: 0,
            strokeWidth: 2,
        });
        selectStyle.addRules(selectBetweenRules);

        var temporaryStyle = new OpenLayers.Style({
            strokeWidth: 3
        });
        temporaryStyle.addRules(temporaryBetweenRules);

        return new OpenLayers.StyleMap({
            'default': defaultStyle,
            'select': selectStyle,
            'temporary': temporaryStyle
        });
    };

    function onMapPanOrZoom(map) {
        /// <summary>
        /// This function gets called every time the user pans or zooms the map.
        /// </summary>
        /// <param name="map">The map object.</param>   
          
    }    
    
    function isEmpty(obj) {
        /// <summary>
        /// Checks if the object is empty (null).
        /// </summary>        
        
        for (var i in obj) { if (obj.hasOwnProperty(i)) { return false; } }
        return true;
    }    
    
    function getMapState(map) {
        /// <summary>Creates a mapState object for a map</summary>
        /// <return>Returns MapState</return>
        
        var theMapState = {};
        var bounds = map.getExtent().toArray();
        theMapState.CurrentMapExtent.MinX = parseInt(bounds[0]);
        theMapState.CurrentMapExtent.MinY = parseInt(bounds[1]);
        theMapState.CurrentMapExtent.MaxX = parseInt(bounds[2]);
        theMapState.CurrentMapExtent.MaxY = parseInt(bounds[3]);
        theMapState.CurrentBaseLayerId = map.baseLayer.id;
        return theMapState;
    }

    function getMapReference(map) {
        /// <summary>
        /// Returns a reference to the map object.        
        /// </summary>
        /// <param name="map">The map object or the id of the map div.</param>
        /// <returns>The map object.</returns>
        
        if (typeof (map) != 'object') {
            return $("#" + map).get(0);
        } else {
            return map;
        }
    }

    function showFeaturePopup(map, e) {
        /// <summary>
        /// Shows a popup for an observation
        /// </summary>
        /// <param name="map">The map object.</param>
        /// <param name="e">The feature selected</param>
        //console.log(e);

        //if (a.fid == -1) {
        //    renderFeaturePopup(map, a);
        //} else {
        //    Artportalen.ajaxPost(Artportalen_ApplicationPath + "/ViewSighting/SiteFeatureAttributes", { SiteId: a.fid }, function(d, c, e) {
        //        a.attributes.siteName = d.siteName;
        //        a.attributes.siteAreaName = d.siteAreaName;
        //        a.attributes.projectAndExternalIds = d.projectAndExternalIds;
        //        a.attributes.coordString = d.coordString;
        //        renderFeaturePopup(map, a);
        //    });
        //}
    }

    
    function renderFeaturePopup(map, c) {        
        if (c && c.fid != d.mapPopupId && d.mapOption.SelectByPolygon == false) {
            var a = "";
            if (c.attributes.projectAndExternalIds && c.attributes.projectAndExternalIds.length > 0) {
                a = c.attributes.projectAndExternalIdDescription + ": <strong>" + c.attributes.projectAndExternalIds + "</strong>"
            }
            if (map.mapPopup) {
                map.removePopup(map.mapPopup);
                map.mapPopup = null;
            }
            var g = calculateOffset(map);
            var h = map.getResolution();
            var f = ((map.center.lon + ((map.getSize().w / 2) * h)) - (234 * h));
            var e = ((map.center.lat - ((map.getSize().h / 2) * h)) + (95 * h));
            if (c.geometry) {
                var b = c.geometry.getCentroid();
                if (b.x + g < f) {
                    f = b.x + g;
                }
                if (b.y - g < e) {
                    e = b.y + g + (95 * h);
                } else {
                    e = b.y - g;
                }
                if (map.mapPopupCreateHandler != null) {
                    map.mapPopup = map.mapPopupCreateHandler(map, c, f, e);
                    map.addPopup(map.mapPopup);
                } else {
                    if (map.mapOption.SelectByPolygon && c.fid == -1) {
                        map.mapPopup = new OpenLayers.Popup("sitepopup", new OpenLayers.LonLat(f, e), new OpenLayers.Size(200, 85), "<span class='sitename'> Utvalgsområde </span><br />Dette polygonet brukes som søkeparameter!<br />", true, null, { autosize: false, closeOnMove: false, contentDisplayClass: "sitepopup", panMapIfOutOfView: true });
                    } else {
                        map.mapPopup = new OpenLayers.Popup("sitepopup", new OpenLayers.LonLat(f, e), new OpenLayers.Size(200, 85), ["<span class='siteindicator' style='background-color:", c.attributes.colorString, ";'></span><span class='sitename'>", c.attributes.siteName, "</span><div class='site-info'>", c.attributes.siteAreaDescription != "" ? c.attributes.siteAreaDescription + ": <strong>" + c.attributes.siteAreaName + "</strong><br />" : "",, c.attributes.coordString, "(±", c.attributes.accuracy, "m)<br />",, a, "</div>"].join(""), true, null, { autosize: false, closeOnMove: false, contentDisplayClass: "sitepopup", panMapIfOutOfView: true });
                    }
                    map.addPopup(map.mapPopup);
                    map.mapPopup.updateSize();
                    map.mapPopup.closeOnMove = false;
                }
            }
        }
    }
    function calculateOffset(map) {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="map">The map object.</param>
        /// <returns></returns>
        
        var test = map.getResolution();
        return Math.round(test * 10);
    }

    function hideFeaturePopup(map, e) {
        /// <summary>
        /// Hides an observation popup
        /// </summary>
        /// <param name="map">The map object.</param>
        /// <param name="e"></param>
            
        
    //    map.mapPopupId = null;
    //    if (map.mapPopup && map.mapPopupHideOnUnHoover) {
    //        map.removePopup(map.mapPopup);
    //        map.mapPopup = null;
    //    }
    }


    context.populateFeatureStoreByAjax = function(map, observationId, options) {
        var settings = $.extend(true, {
            importance: 5,
            showDwcTitle: false,
            hideEmptyFields: false
        }, options);

        if (map.selectedFeatureStore == null)
            return;

        AnalysisPortal.makeAjaxCall({
                url: AnalysisPortal.ApplicationPath + "/Result/GetObservationData",
                params: {
                    observationId: observationId,
                    importance: settings.importance,
                    showDwcTitle: settings.showDwcTitle,
                    hideEmptyFields: settings.hideEmptyFields
                },
                showWaitMessage: true,
                waitMessage: AnalysisPortal.Resources.SharedLoadingData,
                waitMessageDivId: 'SpeciesObservationPropertyGrid-body',
                method: "POST"
            },
            function(result) {
                var pairs = [];
                console.log(result.data);
                for (var i = 0; i < result.data.Fields.length; i++) { // DarwinCore fields
                    if (settings.hideEmptyFields && AnalysisPortal.isStringNullOrEmpty(result.data.Fields[i].Value)) {
                        continue;
                    }
                    if (result.data.Fields[i].Importance > settings.importance) {
                        continue;
                    }
                    var key;
                    if (settings.showDwcTitle) {
                        key = result.data.Fields[i].Name;
                    } else {
                        key = result.data.Fields[i].Label;
                    }
                    pairs.push({ Key: key, Value: result.data.Fields[i].Value });
                }
                for (var i = 0; i < result.data.Projects.length; i++) { // Project fields
                    pairs.push({ Key: '', Value: '' }); // Separator line between projects
                    pairs.push({ Key: '[Project name]', Value: '[' + result.data.Projects[i].Name + ']' });
                    for (var j = 0; j < result.data.Projects[i].ProjectParameters.length; j++) {
                        pairs.push({ Key: result.data.Projects[i].ProjectParameters[j].Label, Value: result.data.Projects[i].ProjectParameters[j].Value });
                    }                    
                }                
                map.selectedFeatureStore.removeAll();
           
                // Start working                
                map.selectedFeatureStore.suspendEvents();
                map.selectedFeatureStore.suspendEvents(); // MK: you need to call suspendEvents() twice for some reason I don't understand.
                map.selectedFeatureStore.add(pairs);
                map.selectedFeatureStore.resumeEvents();
                map.selectedFeatureGrid.getView().refresh();
                // End working                

                if (map.selectedFeatureGrid) {
                    map.selectedFeatureGrid.expand();
                }
            }
        );
    };

    function populateFeatureStoreFromObjectData(map, feature) {
        if (map.selectedFeatureStore == null)
            return;
        
        var pairs = [];
        for (var propertyName in feature.data) {
            var val = feature.data[propertyName];
            pairs.push({ Key: propertyName, Value: val });
        }
        map.selectedFeatureStore.removeAll();
        // Start working                
        map.selectedFeatureStore.suspendEvents();
        map.selectedFeatureStore.suspendEvents(); // MK: you need to call suspendEvents() twice for some reason I don't understand.
        map.selectedFeatureStore.add(pairs);
        map.selectedFeatureStore.resumeEvents();
        if (map.selectedFeatureGrid)
            map.selectedFeatureGrid.getView().refresh();
        // End working
     //   map.selectedFeatureStore.add(pairs);

        if (map.selectedFeatureGrid) {
            map.selectedFeatureGrid.expand();
        }        
    }

    function setAOOEOOFeatureSelected(map, feature, eooPropertyName) {
        for (var propertyName in feature.data) {
            var value = feature.data[propertyName];
            if (propertyName === 'EOO') {
                propertyName = eooPropertyName;
            }
            aooEooData[propertyName] = value;
        }

        var dummyFeatue = { data: aooEooData };

        populateFeatureStoreFromObjectData(map, dummyFeatue);
    };

    function setWfsFeatureSelected(map, feature) {
        /// <summary>
        /// Selects a Wfs Feature
        /// </summary>
        /// <param name="map">The map object.</param>
        /// <param name="feature">The wfs feature</param>
        //alert('setWfsFeatureSelected(map, feature)');
        populateFeatureStoreFromObjectData(map, feature);
        return;

        if (feature != null && feature.data != null && feature.data.observationId != null) {
            AnalysisPortal.CurrentSelectedObservationId = feature.data.observationId;
        }

        if (context.GetFeatureDataByAjax) {

            var settings = $.extend(true, {
                importance: 5,
                showDwcTitle: false,
                hideEmptyFields: false
            }, AnalysisPortal.ObservationDetailsSettings);
            context.populateFeatureStoreByAjax(map, feature.data.observationId, settings);
        } else
            populateFeatureStoreFromObjectData(map, feature);


        if (map.unSelectTimeout != null) {
            window.clearTimeout(map.unSelectTimeout);
            map.unSelectTimeout = null;
        }

        if (map.skipSelectEvent) {
            map.skipSelectEvent = false;
            return;
        }

        if (feature) {
            $('#' + map.div.id).triggerHandler('onMapObservationsSelected', [feature]);
        }
    }

    function setWfsFeatureUnSelected(map, feature) {
        /// <summary>
        /// Deselects a wfs feature
        /// </summary>
        /// <param name="map">The map object.</param>
        /// <param name="feature">The feature</param>
        //alert('setWfsFeatureUnSelected(map, feature)');
        //return;
        
        AnalysisPortal.CurrentSelectedObservationId = null;
        if (map.selectedFeatureStore) {
            //map.selectedFeatureStore.each(function (record, idx) {
            //    record.set('Value', '');
            //});
            //map.selectedFeatureStore.save(); // if we don't call save() a red icon, indicating that the record is edited but not saved, will appear to the left of each record in the grid
            map.selectedFeatureStore.removeAll();           

            if (map.selectedFeatureGrid) {
                map.selectedFeatureGrid.getView().refresh();
                //map.selectedFeatureGrid.collapse();
                //map.selectedFeatureGrid.collapse(Ext.Component.DIRECTION_RIGHT, false);
                //map.selectedFeatureGrid.collapse(null, false);
            }
        }

        if (map.unSelectTimeout != null) {
            window.clearTimeout(map.unSelectTimeout);
            map.unSelectTimeout = null;
        }
        map.unSelectTimeout = window.setTimeout(function () {
            if (feature) {                
                $('#' + map.div.id).triggerHandler('onMapObservationsUnSelected', [feature]);
            }
        }, 100);
    }

    function setMapObservationSelected(map, feature) {
        /// <summary>
        /// Selects an observation
        /// </summary>
        /// <param name="map">The map object.</param>
        /// <param name="feature">The observation</param>

        if (feature != null && feature.data != null && feature.data.observationId != null) {
            AnalysisPortal.CurrentSelectedObservationId = feature.data.observationId;
        }

        if (context.GetFeatureDataByAjax) {

            var settings = $.extend(true, {
                importance: 5,
                showDwcTitle: false,
                hideEmptyFields: false
            }, AnalysisPortal.ObservationDetailsSettings);
            context.populateFeatureStoreByAjax(map, feature.data.observationId, settings);
        } else
            populateFeatureStoreFromObjectData(map, feature);


        if (map.unSelectTimeout != null) {
            window.clearTimeout(map.unSelectTimeout);
            map.unSelectTimeout = null;
        }
        
        if (map.skipSelectEvent) {
            map.skipSelectEvent = false;
            return;
        }

        if (feature) {
            $('#' + map.div.id).triggerHandler('onMapObservationsSelected', [feature]);            
        }
    }

    context.clearSpeciesObservationPropertyGrid = function(map) {
        setMapObservationsUnSelected(map);
    };

    function setMapObservationsUnSelected(map, feature) {
        /// <summary>
        /// Deselects an observation
        /// </summary>
        /// <param name="map">The map object.</param>
        /// <param name="feature">The observation</param>
        console.log('onMapObservationsUnSelected');
        AnalysisPortal.CurrentSelectedObservationId = null;
        if (map.selectedFeatureStore) {
            map.selectedFeatureStore.each(function(record, idx) {                
                record.set('Value', '');
            });
            map.selectedFeatureStore.save(); // if we don't call save() a red icon, indicating that the record is edited but not saved, will appear to the left of each record in the grid
            //map.selectedFeatureStore.removeAll();           
            
            if (map.selectedFeatureGrid) {
                map.selectedFeatureGrid.getView().refresh();
                //map.selectedFeatureGrid.collapse();
                //map.selectedFeatureGrid.collapse(Ext.Component.DIRECTION_RIGHT, false);
                //map.selectedFeatureGrid.collapse(null, false);
            }
        }

        if (feature) {
            $('#' + map.div.id).triggerHandler('onMapObservationsUnSelected', [feature]);
        }

        //if (map.unSelectTimeout != null) {
        //    window.clearTimeout(map.unSelectTimeout);
        //    map.unSelectTimeout = null;
        //}        
        //map.unSelectTimeout = window.setTimeout(function () {
        //    if (feature) {                
        //        $('#' + map.div.id).triggerHandler('onMapObservationsUnSelected', [feature]);
        //    }
        //}, 100);
    }

    //Initialize the side bar 
    function initMapLayerSideBar(map, mapContainerId, openSwitcher) {
        var sideBarTop = $('#' + mapContainerId).offset().top - 27;        
        var sideBarHtml = $('<div/>')
            .css({ position: 'absolute', top: sideBarTop + 'px', left: 0, 'z-index': 99, height: '0px' })
            .append($('<div/>')
                .attr({ id: 'sidebar', role: 'tablist', 'aria-multiselectable': 'true' })
                .addClass('sidebar')
                .append($('<div/>')
                    .addClass('panel panel-default')
                    .append($('<div/>')
                        .attr({ id: 'collapseSidebar', role: 'tabpanel', 'aria-labelledby': 'headingSidebar' })
                        .addClass('panel-collapse collapse' + (openSwitcher ? ' in' : ''))
                        .append($('<div/>')
                            .css({ height: '300px', width: '300px', overflow: 'auto' })
                            .addClass('panel-body')
                            .append($('<div/>')
                                .attr({ id: 'baseLayers' })
                            )
                            .append($('<div/>')
                                .attr({ id: 'otherLayers' })
                            )
                        )
                    )
                    .append($('<div/>')
                        .attr({ id: 'headingSidebar', href: '#collapseSidebar', role: 'tab', 'data-toggle': 'collapse', 'aria-expanded': true, 'aria-controls': 'collapseSidebar' })
                        .addClass('panel-heading ' + (openSwitcher ? '' : ' collapsed'))
                        .append($('<h4/>')
                            .addClass('panel-title')
                            .html('&nbsp;&nbsp;&nbsp;&nbsp;' + AnalysisPortal.Resources.LayerSideBarTitle + '&nbsp;&nbsp;')
                        )
                    )
                )
            );
        
        //Add sidebar to container div
        $('#container').prepend(sideBarHtml);

        //Get layers from map
        var layers = getMapLayers(map);

        var baseTree = [
            {
                text: AnalysisPortal.Resources.LayerSidebarBaseLayers,
                nodes: layers.baseLayers,
                selectable: false
            }
        ];

        //Create a treeview with base layers
        //https://github.com/jonmiles/bootstrap-treeview
        $('#baseLayers').treeview({
            data: baseTree,
            showIcon: true,            
            selectedIcon: 'glyphicon glyphicon-ok',
            showTags: true,
            onNodeSelected: function (event, node) {
                var layer = map.getLayersByName(node.text)[0];
                map.setBaseLayer(layer);
            },
            onNodeUnselected: function (event, node) {
                setLayerVisibility(map, node.text, false);
            }
        });

        initOtherTree(map, layers.otherLayers);
        $('#collapseSidebar span.badge').tooltip(
            {
                container: 'body',
                title: AnalysisPortal.Resources.SharedLayerDoesntSupportCurrentCoordinateSystem + ' (' + context.CoordinateSystems[context.CurrentCoordinateSystem].name + ').'
            });
    }

    //`Get baselayers and other layers from map
    function getMapLayers(map) {
        var baseLayers = [];
        var otherLayers = [];
        for (var i = 0; i < map.layers.length; i++) {
            var layer = map.layers[i];
            
            var supportsCurrentCoordinateSystem = true;
            if (layer.supportedCoordinateSystems) {
                supportsCurrentCoordinateSystem = $.inArray(context.CurrentCoordinateSystem, layer.supportedCoordinateSystems) >= 0;
            }
    
            if (layer.displayInLayerSwitcher) {
                if (layer.isBaseLayer) {
                    baseLayers.push({
                        text: layer.name,
                        //icon: "glyphicon glyphicon-warning-sign",
                        state: {
                            checked: layer.visibility,
                            selected: layer.visibility,
                            disabled: !supportsCurrentCoordinateSystem
                        },
                        tags: !supportsCurrentCoordinateSystem ? ['i'] : null,
                        index: 0
                    });
                } else {                    
                    otherLayers.push({
                        text: layer.name,
                        state: {
                            checked: layer.visibility,
                            selected: layer.visibility,
                            disabled: !supportsCurrentCoordinateSystem
                        },
                        tags: !supportsCurrentCoordinateSystem ? ['i'] : null,
                        index: getLayerIndex(map, layer)
                    });
                }
            }
        }
       
        return { baseLayers: baseLayers, otherLayers: otherLayers.sort(dynamicSort('-index')) };
    }

    //Generic sort function for objects
    function dynamicSort(property) {
        var sortOrder = 1;
        if (property[0] === "-") {
            sortOrder = -1;
            property = property.substr(1);
        }
        return function (a, b) {
            var result = (a[property] < b[property]) ? -1 : (a[property] > b[property]) ? 1 : 0;
            return result * sortOrder;
        }
    }

    //Initialize the other layers tree
    function initOtherTree(map, otherLayers) {
        //Create the icon used to toggle sort on end off. 
        var sortActivationIcon = $('<span/>').append(
            $('<span/>')
            .attr({ id: 'toggleSortLayers', title: AnalysisPortal.Resources.LayerSidebarSortingTooltip, 'data-toggle': 'tooltip' })
            .css({ 'font-size': '12px' })
            .addClass('pull-right')
            .text(AnalysisPortal.Resources.LayerSidebarSorting)
            .append(
                $('<i/>')
                .css({ 'margin-left': '5px' })
                .addClass('glyphicon glyphicon-unchecked')
            )
        );
       
        var resultTree = [
          {     //Add toogle icon to text since the tree control is rerendered all the time
              text: AnalysisPortal.Resources.LayerSidebarOtherLayers + sortActivationIcon.html(),
              nodes: otherLayers,
              selectable: false,
              state: { checked: false, disabled: false, expanded: true }
          }
        ];

        //Make sure tree dosen't exists
        $('#otherLayers').treeview('remove');
        $('#otherLayers').treeview({
            data: resultTree,
            multiSelect: true,
            showIcon: true,
            showTags: true,
            selectedIcon: 'glyphicon glyphicon-ok',
            onNodeSelected: function (event, node) {
                setLayerVisibility(map, node.text, true);
                initTreeTootip();
            },
            onNodeUnselected: function (event, node) {
                setLayerVisibility(map, node.text, false);
                initTreeTootip();
            },
            onNodeCollapsed: function (event, node) {
                initTreeTootip();
            },
            onNodeExpanded: function (event, node) {
                initTreeTootip();
            }
        });

        initTreeTootip();

        //Add event handler to enable/disable sorting
        $('#otherLayers').on('click', '#toggleSortLayers', function (e) {
            var $icon = $(this).children('i');

            if ($icon.hasClass('glyphicon-unchecked')) {
                $icon.removeClass('glyphicon-unchecked');
                $icon.addClass('glyphicon-check');

                //Add drag drop to layer list
                initDragDropSort(map);
            } else {
                $icon.removeClass('glyphicon-check');
                $icon.addClass('glyphicon-unchecked');

                //Remove drag drop sorting
                dragDropSort.deactivate();

                //If layers is expand, sorting can been done
                if ($('#otherLayers').treeview('getExpanded').length) {
                    var layers = [];
                    var treeLayers = getOtherLayerFromTree();

                    //Update layer sort order
                    $.each(treeLayers, function (index, layer) {
                        for (var i = 0; i < otherLayers.length; i++) {
                            var mapLayer = otherLayers[i];
                            
                            if (layer.name == mapLayer.text) {
                                mapLayer.state.selected = layer.selected;
                                layers.push(mapLayer);
                                continue;
                            }
                        }
                    });

                    initOtherTree(map, layers);
                } 
            }

            return false;
        });
    }

    //Init drag and drop sorting of other layers
    function initDragDropSort(map) {
        if ($('#otherLayers').treeview('getExpanded').length != 0) {
            //Disable node open/close
            $('#otherLayers').on('click', 'ul li:first', function (e) {
                return false;
            });

            //Add drag drop to layer list
            dragDropSort = $('#otherLayers ul').dad({
                target: '>li:not(:first)',
                callback: function (e) {
                    var layers = e.closest('ul').children('li:not(:first):not(:last)');
                    var layerCount = layers.length;
                    for (var i = 0; i < layerCount; i++) {
                        setLayerIndex(map, layers[i].textContent, layerCount - i);
                    }
                }
            });
        }
    }

    //Get layers from other layers tree
    function getOtherLayerFromTree() {
        var layers = [];

        //Update layer sort order
        $('#otherLayers ul li:not(:first)').each(function (index, item) {
            var $item = $(item);
            layers.push({ name: $item.text(), selected: $item.hasClass('node-selected') });
        });

        return layers;
    }

    //Sets the layer object if only name is passed, return null if no layer found
    function verifyLayer(map, layer) {
        if (typeof layer == "string") {
            var layers = map.getLayersByName(layer);

            if (layers.length == 1) {
                layer = layers[0];
            } else {
                layer = null;
            }
        }

        return layer;
    }

    //Hide or show map layer
    function setLayerVisibility(map, layer, show) {
        layer = verifyLayer(map, layer);
        if (layer != null) {
            layer.setVisibility(show);
        }
    };

    //Get map layer index
    function getLayerIndex(map, layer) {
        layer = verifyLayer(map, layer);

        if (layer == null) {
            return null;
        }

        return map.getLayerIndex(layer);
    }

    //Set map layer index
    function setLayerIndex(map, layer, index) {
        layer = verifyLayer(map, layer);

        if (layer != null) {
            map.setLayerIndex(layer, index);
        }
    }

    //Returns true if layer sidebar is open
    function islayerSidebarOpen() {
        return $('#collapseSidebar').hasClass('in');
    }

    function initTreeTootip() {
        //Tooltip will disapear when interaction with tree is made, since the three is rewritten all the time.
        setTimeout(function () { $('#otherLayers [data-toggle="tooltip"]').tooltip(); }, 100);
       
    }
})(AnalysisPortal.GIS);    
    

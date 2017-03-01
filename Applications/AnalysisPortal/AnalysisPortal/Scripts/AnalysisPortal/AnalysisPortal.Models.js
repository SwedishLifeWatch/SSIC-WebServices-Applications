var AnalysisPortal = AnalysisPortal || {};
AnalysisPortal.Models = {};
(function (context) {
    /*
     * Public properties are written as: context.VariableName =
     * Private properties are written as: var variableName =  
     * Public functions are written as: context.functionName = function(args) {
     * Private functions are written as: function functionName(args) {
     */


    context.defineTaxonViewModel = function() {
        /// <summary>
        /// Defines TaxonViewModel as a model in Ext JS
        /// </summary>    
        
        var modelName = "TaxonViewModel";        
        if (Ext.ClassManager.isCreated(modelName)) {            
            return;
        }                        
        Ext.define(modelName, {
            extend: 'Ext.data.Model',
            idProperty: 'TaxonId',
            fields: [
                { name: 'ScientificName', type: 'string' },
                { name: 'Author', type: 'string' },
                { name: 'CommonName', type: 'string' },
                { name: 'Category', type: 'string' },
                { name: 'TaxonId', type: 'int' },
                { name: 'TaxonStatus', type: 'int' },
                { name: 'SpeciesProtectionLevel', type: 'int' }
            ]
        });
    };

    context.defineTaxonSpeciesObservationCountViewModel = function () {
        /// <summary>
        /// Defines TaxonSpeciesObservationCountViewModel as a model in Ext JS
        /// </summary>    

        var modelName = "TaxonSpeciesObservationCountViewModel";
        if (Ext.ClassManager.isCreated(modelName)) {
            return;
        }
        Ext.define(modelName, {
            extend: 'Ext.data.Model',
            idProperty: 'TaxonId',
            fields: [
                { name: 'ScientificName', type: 'string' },
                { name: 'Author', type: 'string' },
                { name: 'CommonName', type: 'string' },
                { name: 'Category', type: 'string' },
                { name: 'TaxonId', type: 'int' },
                { name: 'TaxonStatus', type: 'int' },
                { name: 'SpeciesObservationCount', type: 'int'}
            ]
        });
    };

    context.defineObservationsListItemViewModel = function () {
        /// <summary>
        /// Defines ObservationsListItemViewModel as a model in Ext JS
        /// </summary>    

        var modelName = "ObservationsListItemViewModel";
        if (Ext.ClassManager.isCreated(modelName)) {
            return;
        }
        Ext.define(modelName, {
            extend: 'Ext.data.Model',
            idProperty: 'ObservationId',
            fields: [
                { name: 'ObservationId', type: 'string' },
                { name: 'ScientificName', type: 'string' },
                { name: 'CommonName', type: 'string' },
                { name: 'ObservationDate', type: 'string' },
                { name: 'RecordedBy', type: 'string' },
                { name: 'TaxonId', type: 'string' },
                { name: 'Locality', type: 'string' },
                { name: 'Description', type: 'string' }
            ]
        });
    };

    context.defineHistogramBoxModel = function () {
        /// <summary>
        /// Defines defineHistogramBoxModel as a model in Ext JS
        /// </summary>    

        var modelName = "HistogramBox";
        if (Ext.ClassManager.isCreated(modelName)) {
            return;
        }
        Ext.define(modelName, {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'Name', type: 'string' },
                { name: 'FromValue', type: 'int' },
                { name: 'ToValue', type: 'int' },
                { name: 'Color', type: 'string' }
            ]
        });
    };


    context.defineKeyValuePairModel = function () {
        /// <summary>
        /// Defines KeyValuePairModel as a model in Ext JS
        /// </summary>    

        var modelName = "KeyValuePair";
        if (Ext.ClassManager.isCreated(modelName)) {
            return;
        }
        Ext.define(modelName, {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'Key' },
                { name: 'Value' }
            ]
        });
    };
    
    context.defineKeyValueDiagramPairModel = function () {
        /// <summary>
        /// Defines KeyValueDiagramPairModel as a model in Ext JS
        /// </summary>    

        var modelName = "KeyValueDiagramPair";
        if (Ext.ClassManager.isCreated(modelName)) {
            return;
        }
        Ext.define(modelName, {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'Key' },
                { name: 'Value' }
            ]
        });
    };

    context.defineSummaryStatisticsPerPolygonModel = function () {
        /// <summary>
        /// Defines SummaryStatisticsPerPolygonModel as a model in Ext JS
        /// </summary>

        var modelName = "SummaryStatisticsPerPolygon";
        if (Ext.ClassManager.isCreated(modelName)) {
            return;
        }
        Ext.define(modelName, {            
            extend: 'Ext.data.Model',
            fields: [
                { name: 'SpeciesObservationsCount', type: 'string' },
                { name: 'SpeciesCount', type: 'string' },
                { name: 'Properties', type: 'string' }
            ]
        });
    };

    context.defineProvenanceModel = function () {
        /// <summary>
        /// Defines ProvenanceModel as a model in Ext JS
        /// </summary>

        var modelName = "ProvenanceModel";
        if (Ext.ClassManager.isCreated(modelName)) {
            return;
        }
        Ext.define(modelName, {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'Name', type: 'string' }
            ],
            hasMany: { model: 'ProvenanceValueModel', name: 'Values' }
        });
    };

    context.defineProvenanceValueModel = function () {
        /// <summary>
        /// Defines ProvenanceModel as a model in Ext JS
        /// </summary>

        var modelName = "ProvenanceValueModel";
        if (Ext.ClassManager.isCreated(modelName)) {
            return;
        }
        Ext.define(modelName, {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'Value', type: 'string' },
                { name: 'SpeciesObservationCount', type: 'int' },
                { name: 'Id', type: 'int' },
                { name: 'IdIsSpecified', type: 'boolean' }
            ],
            belongsTo: 'ProvenanceModel'
        });
    };

    context.defineWfsLayerInfoModel = function () {
        /// <summary>
        /// Defines WmsCapapabilitiesLayerModel as a model in Ext JS
        /// </summary>    

        var modelName = "WfsLayerInfoModel";
        if (Ext.ClassManager.isCreated(modelName)) {
            return;
        }
        Ext.define(modelName, {
            extend: 'Ext.data.Model',
            //idProperty: 'Id',
            fields: [
                { name: 'Title', type: 'string' },
                { name: 'Name', type: 'string' },
                { name: 'Namespace', type: 'string' },
                { name: 'FullName', type: 'string' },
                { name: 'Url', type: 'string' }
            ]
        });
    };

    context.defineWmsCapabilitiesLayerModel = function () {
        /// <summary>
        /// Defines WmsCapapabilitiesLayerModel as a model in Ext JS
        /// </summary>    

        var modelName = "WmsCapapabilitiesLayerModel";
        if (Ext.ClassManager.isCreated(modelName)) {
            return;
        }
        Ext.define(modelName, {
            extend: 'Ext.data.Model',
            //idProperty: 'Id',
            fields: [
                { name: 'Title', type: 'string' },
                { name: 'Name', type: 'string' },
                { name: 'Abstract', type: 'string' },
                { name: 'Prefix', type: 'string' },
                { name: 'Opaque', type: 'boolean' },
                { name: 'SrsString', type: 'string' },
                { name: 'LegendUrl', type: 'string' },
                { name: 'LegendWidth', type: 'int' },
                { name: 'LegendHeight', type: 'int' }
            ]
        });
    };




    context.defineWmsLayerViewModel = function () {
        /// <summary>
        /// Defines WmsLayerViewModel as a model in Ext JS
        /// </summary>    

        var modelName = "WmsLayerViewModel";
        if (Ext.ClassManager.isCreated(modelName)) {
            return;
        }
        Ext.define(modelName, {
            extend: 'Ext.data.Model',
            idProperty: 'Id',
            fields: [
                { name: 'Id', type: 'int' },
                { name: 'Name', type: 'string' },
                { name: 'ServerUrl', type: 'string' },
                { name: 'IsBaseLayer', type: 'boolean' },
                { name: 'Layers' } // array
            ]
        });
    };


    context.defineWmsCapabilitiesLayerModelGeoExt = function () {
        /// <summary>
        /// 
        /// </summary>    

        var modelName = "GeoExt.data.WmsCapabilitiesLayerModel";
        if (Ext.ClassManager.isCreated(modelName)) {
            return;
        }
        
        Ext.define(modelName, {
            extend: 'GeoExt.data.LayerModel',
            alternateClassName: [
                'GeoExt.data.WMSCapabilitiesModel',
                'GeoExt.data.WmsCapabilitiesModel'
            ],
            //requires: ['GeoExt.data.reader.WmsCapabilities'],
            alias: 'model.gx_wmscapabilities',
            fields: [
                'id',
                {name: 'title', type: 'string', mapping: 'name'},
                {name: 'legendURL', type: 'string', mapping: 'metadata.legendURL'},
                {name: 'hideTitle', type: 'bool', mapping: 'metadata.hideTitle'},
                {name: 'hideInLegend', type: 'bool', mapping: 'metadata.hideInLegend'},
                { name: "name", type: "string", mapping: "metadata.name" },
                { name: "abstract", type: "string", mapping: "metadata.abstract" },
                { name: "queryable", type: "boolean", mapping: "metadata.queryable" },
                { name: "opaque", type: "boolean", mapping: "metadata.opaque" },
                { name: "noSubsets", type: "boolean", mapping: "metadata.noSubsets" },
                { name: "cascaded", type: "int", mapping: "metadata.cascaded" },
                { name: "fixedWidth", type: "int", mapping: "metadata.fixedWidth" },
                { name: "fixedHeight", type: "int", mapping: "metadata.fixedHeight" },
                { name: "minScale", type: "float", mapping: "metadata.minScale" },
                { name: "maxScale", type: "float", mapping: "metadata.maxScale" },
                { name: "prefix", type: "string", mapping: "metadata.prefix" },
                { name: "attribution", type: "string" },
                { name: "formats", mapping: "metadata.formats" }, // array
                { name: "infoFormats", mapping: "metadata.infoFormats" }, //array
                { name: "styles", mapping: "metadata.styles" }, // array
                { name: "srs", mapping: "metadata.srs" }, // object
                { name: "dimensions", mapping: "metadata.dimensions" }, // object
                { name: "bbox", mapping: "metadata.bbox" }, // object
                { name: "llbbox", mapping: "metadata.llbbox" }, // array
                { name: "keywords", mapping: "metadata.keywords" }, // array
                { name: "identifiers", mapping: "metadata.identifiers" }, // object
                { name: "authorityURLs", mapping: "metadata.authorityURLs" }, // object
                { name: "metadataURLs", mapping: "metadata.metadataURLs" } // array
            ]//,
            //proxy: {
            //    type: 'ajax',
            //    reader: {
            //        type: 'gx_wmscapabilities'
            //    }
            //}
        });
      
    };



   



    context.defineWfsLayerViewModel = function() {
        /// <summary>
        /// Defines TaxonViewModel as a model in Ext JS
        /// </summary>    
        
        var modelName = "WfsLayerViewModel";        
        if (Ext.ClassManager.isCreated(modelName)) {            
            return;
        }                        
        Ext.define(modelName, {
            extend: 'Ext.data.Model',
            idProperty: 'Id',
            fields: [
                { name: 'Id', type: 'int' },
                { name: 'Name', type: 'string' },
                { name: 'ServerUrl', type: 'string' },
                { name: 'TypeName', type: 'string' },
                { name: 'GeometryName', type: 'string' },
                { name: 'GeometryType', type: 'string' },                
                { name: 'Filter', type: 'string' },
                { name: 'Color', type: 'string' },
                { name: 'MediaName', type: 'string' }
            ]
        });
    };

    context.defineUploadedGisLayersViewModel = function () {
        /// <summary>
        /// Defines UploadedGisLayers as a model in Ext JS
        /// </summary>    

        var modelName = "UploadedGisLayersViewModel";
        if (Ext.ClassManager.isCreated(modelName)) {
            return;
        }
        Ext.define(modelName, {
            extend: 'Ext.data.Model',
            //idProperty: 'FileName',
            fields: [
                { name: 'Name', type: 'string' },
                { name: 'FileName', type: 'string' }                
            ]
        });
    };


    context.defineTableFieldDescriptionViewModel = function() {
        /// <summary>
        /// Defines TableFieldDescriptionViewModel as a model in Ext JS
        /// </summary>    
        
        var modelName = "TableFieldDescriptionViewModel";        
        if (Ext.ClassManager.isCreated(modelName)) {            
            return;
        }                        
        Ext.define(modelName, {
            extend: 'Ext.data.Model',
            idProperty: 'Id',
            fields: [
                { name: 'Id', type: 'int' },
                { name: 'Name', type: 'string' },
                { name: 'Label', type: 'string' },                
                { name: 'Class', type: 'string' },
                { name: 'Definition', type: 'string' },
                { name: 'DefinitionUrl', type: 'string' },
                { name: 'Documentation', type: 'string' },
                { name: 'DocumentationUrl', type: 'string' },
                { name: 'Guid', type: 'string' },
                { name: 'Remarks', type: 'string' },
                { name: 'Type', type: 'string' },
                { name: 'Importance', type: 'int' },
                { name: 'SortOrder', type: 'int' },
                { name: 'IsAcceptedByTdwg', type: 'bool' },
                { name: 'IsClassName', type: 'bool' },
                { name: 'IsImplemented', type: 'bool' },
                { name: 'IsMandatory', type: 'bool' },
                { name: 'IsMandatoryFromProvider', type: 'bool' },
                { name: 'IsObtainedFromProvider', type: 'bool' },
                { name: 'IsPlanned', type: 'bool' }
            ]
        });
    };


    context.defineRegionViewModel = function() {
        /// <summary>
        /// Defines RegionViewModel as a model in Ext JS
        /// </summary>    
        
        var modelName = "RegionViewModel";        
        if (Ext.ClassManager.isCreated(modelName)) {            
            return;
        }                        
        Ext.define(modelName, {
            extend: 'Ext.data.Model',
            idProperty: 'Id',
            fields: [
                { name: 'Id', type: 'int' },
                { name: 'Name', type: 'string' },
                { name: 'GUID', type: 'string' },
                { name: 'CategoryId', type: 'int' }                
            ]            
        });
    };


    context.defineSpeciesObservationGridResult = function () {
        /// <summary>
        /// Defines SpeciesObservationGridResult as a model in Ext JS
        /// </summary>    

        var modelName = "SpeciesObservationGridResult";
        if (Ext.ClassManager.isCreated(modelName)) {
            return;
        }
        Ext.define(modelName, {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'GridCellCoordinateSystemId', type: 'int' },
                { name: 'GridCellCoordinateSystem', type: 'string' },
                { name: 'GridCellSize', type: 'int' }
            ],
            hasMany : [
                { model: 'SpeciesObservationGridCellResult', name: 'SpeciesObservationGridCellResult' }
            ]
        });
    };


    context.defineSpeciesObservationGridCellResult = function () {
        /// <summary>
        /// Defines SpeciesObservationGridCellResult as a model in Ext JS
        /// </summary>    

        var modelName = "SpeciesObservationGridCellResult";
        if (Ext.ClassManager.isCreated(modelName)) {
            return;
        }
        Ext.define(modelName, {
            extend: 'Ext.data.Model',            
            fields: [                
                { name: 'CentreCoordinateX', type: 'double' },
                { name: 'CentreCoordinateY', type: 'double' },
                { name: 'OriginalCentreCoordinateX', type: 'double' },
                { name: 'OriginalCentreCoordinateY', type: 'double' },                
                { name: 'ObservationCount', type: 'int' }
            ],
            belongsTo: 'SpeciesObservationGridResult'
        });
    };
    
    context.defineTaxonGridResult = function () {
        /// <summary>
        /// Defines TaxonGridResult as a model in Ext JS
        /// </summary>    

        var modelName = "TaxonGridResult";
        if (Ext.ClassManager.isCreated(modelName)) {
            return;
        }
        Ext.define(modelName, {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'GridCellCoordinateSystemId', type: 'int' },
                { name: 'GridCellCoordinateSystem', type: 'string' },
                { name: 'GridCellSize', type: 'int' }
            ],
            hasMany: [
                { model: 'TaxonGridCellResult', name: 'TaxonGridCellResult' }
            ]
        });
    };


    context.defineTaxonGridCellResult = function () {
        /// <summary>
        /// Defines TaxonGridCellResult as a model in Ext JS
        /// </summary>    

        var modelName = "TaxonGridCellResult";
        if (Ext.ClassManager.isCreated(modelName)) {
            return;
        }
        Ext.define(modelName, {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'CentreCoordinateX', type: 'double' },
                { name: 'CentreCoordinateY', type: 'double' },
                { name: 'OriginalCentreCoordinateX', type: 'double' },
                { name: 'OriginalCentreCoordinateY', type: 'double' },
                { name: 'ObservationCount', type: 'int' },
                { name: 'SpeciesCount', type: 'int' }
            
            ],
            belongsTo: 'TaxonGridResult'
        });
    };

    
    context.defineCombinedGridStatisticsResult = function () {
        /// <summary>
        /// Defines CombinedGridStatisticsResult as a model in Ext JS
        /// </summary>    

        var modelName = "CombinedGridStatisticsResult";
        if (Ext.ClassManager.isCreated(modelName)) {
            return;
        }
        Ext.define(modelName, {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'GridCellCoordinateSystemId', type: 'int' },
                { name: 'GridCellCoordinateSystem', type: 'string' },
                { name: 'GridCellSize', type: 'int' }
            ],
            hasMany: [
                { name: 'Cells', model: 'CombinedGridStatisticsCellResult' }
            ]
        });
    };


    context.defineCombinedGridStatisticsCellResult = function () {
        /// <summary>
        /// Defines CombinedGridStatisticsCellResult as a model in Ext JS
        /// </summary>    

        var modelName = "CombinedGridStatisticsCellResult";
        if (Ext.ClassManager.isCreated(modelName)) {
            return;
        }
        Ext.define(modelName, {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'CentreCoordinateX', type: 'double' },
                { name: 'CentreCoordinateY', type: 'double' },
                { name: 'OriginalCentreCoordinateX', type: 'double' },
                { name: 'OriginalCentreCoordinateY', type: 'double' },
                { name: 'ObservationCount', type: 'int' },
                { name: 'SpeciesCount', type: 'int' },
                { name: 'FeatureCount', type: 'int' },
                { name: 'FeatureLength', type: 'double' },
                { name: 'FeatureArea', type: 'double' }
            ],
            belongsTo: 'CombinedGridStatisticsResult'
        });
    };



    context.defineGeoJsonFeature = function () {
        /// <summary>
        /// Defines TaxonGridCellResult as a model in Ext JS
        /// </summary>    

        var modelName = "GeoJsonFeature";
        if (Ext.ClassManager.isCreated(modelName)) {
            return;
        }
        Ext.define(modelName, {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'id', type: 'string' },
                { name: 'type', type: 'string' }                
            ]            
        });
    };


//    proxy: {
//                type: 'ajax',
//                api: {
//                    read: 'bookRead.php',
//                    create: AnalysisPortal.ApplicationPath + "MySettings/AddRegion",
//                    update: 'bookUpdate.php',
//                    destroy: 'bookDestroy.php'
//                }
//            }

    context.defineTaxonSearchResultItemViewModel = function() {
        /// <summary>
        /// Defines TaxonSearchResultItemViewModel as a model in Ext JS
        /// </summary>    

        Ext.define('TaxonSearchResultItemViewModel', {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'ScientificName', type: 'string' },
                { name: 'Author', type: 'string' },
                { name: 'CommonName', type: 'string' },
                { name: 'Category', type: 'string' },
                { name: 'TaxonId', type: 'int' },
                { name: 'TaxonStatus', type: 'int' },
                { name: 'SearchMatchName', type: 'string' },
                { name: 'NameCategory', type: 'string' },                
                { name: 'TaxonAlertStatus', type: 'int' },
                { name: 'SpeciesProtectionLevel', type: 'int' }
            ]
        });
    };

    context.defineTaxonSearchResultTreeViewModel = function () {
        /// <summary>
        /// Defines TaxonSearchResultTreeViewModel as a model in Ext JS
        /// </summary>    

        Ext.define('TaxonSearchResultTreeViewModel', {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'compositeId', type: 'int' },
                { name: 'parentFactorId', type: 'int' },
                { name: 'nodeFactorId', type: 'int' },
                { name: 'name', type: 'string' }
            ]
        });
    };
    
    context.defineObservationViewModel = function() {
        /// <summary>
        /// Defines ObservationViewModel as a model in Ext JS
        /// </summary>    

        Ext.define('ObservationViewModel', {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'ObservationId', type: 'string' },
                { name: 'OrganismGroup', type: 'string' },
                { name: 'ScientificName', type: 'string' },
                { name: 'CommonName', type: 'string' },
                { name: 'TaxonConceptStatus', type: 'string' },
                { name: 'RedlistCategory', type: 'string' },
                { name: 'StartDate', type: 'string' },
                { name: 'EndDate', type: 'string' },
                { name: 'Locality', type: 'string' },
                { name: 'Parish', type: 'string' },
                { name: 'Municipality', type: 'string' },
                { name: 'County', type: 'string' },
                { name: 'StateProvince', type: 'string' },
                { name: 'CoordinateNorth', type: 'string' },
                { name: 'CoordinateEast', type: 'string' },
                { name: 'Accurancy', type: 'string' },
                { name: 'RecordedBy', type: 'string' },
                { name: 'Owner', type: 'string' },
                { name: 'Quantity', type: 'string' },
                { name: 'QuantityUnit', type: 'string' },
                { name: 'LifeStage', type: 'string' },
                { name: 'Behaviour', type: 'string' },
                { name: 'Substrate', type: 'string' },
                { name: 'OcurranceRemarks', type: 'string' },
                { name: 'DetConf', type: 'string' },
                { name: 'CollectionCode', type: 'string' },
                { name: 'IsNeverFoundObservarion', type: 'string' },
                { name: 'IsNotRediscoveredObservation', type: 'string' },
                { name: 'DatasetID', type: 'string' },
                { name: 'Database', type: 'string' },
                { name: 'TaxonSortOrder', type: 'string' },
                { name: 'TaxonId', type: 'string' },
                { name: 'GroupSorting', type: 'string' },
                { name: 'ProtectionLevel', type: 'string' }
            ]
        });   
    };
    
    context.defineDiagramViewModel = function () {
        /// <summary>
        /// Defines ObservationViewModel as a model in Ext JS
        /// </summary>    

        Ext.define('DiagramViewModel', {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'NoOfObservations', type: 'int' },
                { name: 'Mounth', type: 'string' }
                
            ]
        });
    };

    context.defineObservationDarwinCoreViewModel = function() {
        /// <summary>
        /// Defines ObservationViewModel as a model in Ext JS
        /// </summary>    

        Ext.define('ObservationDarwinCoreViewModel', {
            extend: 'Ext.data.Model',
            fields: [
                { name: 'ObservationId', type: 'string' },
                { name: 'AccessRights', type: 'string'},
                { name: 'BasisOfRecord', type: 'string'},
                { name: 'BibliographicCitation', type: 'string'},
                { name: 'CollectionCode', type: 'string'},
                { name: 'CollectionID', type: 'string'},
                { name: 'ActionPlan', type: 'string'},
                { name: 'ConservationRelevant', type: 'string'},
                { name: 'Natura2000', type: 'string'},
                { name: 'ProtectedByLaw', type: 'string'},
                { name: 'ProtectionLevel', type: 'string'},
                { name: 'RedlistCategory', type: 'string'},
                { name: 'SwedishImmigrationHistory', type: 'string'},
                { name: 'SwedishOccurrence', type: 'string'},
                { name: 'DataGeneralizations', type: 'string'},
                { name: 'DatasetID', type: 'string'},
                { name: 'DatasetName', type: 'string'},
                { name: 'DynamicProperties', type: 'string'},
                { name: 'Day', type: 'string'},
                { name: 'End', type: 'string'},
                { name: 'EndDayOfYear', type: 'string'},
                { name: 'EventDate', type: 'string'},
                { name: 'EventID', type: 'string'},
                { name: 'EventRemarks', type: 'string'},
                { name: 'EventTime', type: 'string'},
                { name: 'FieldNotes', type: 'string'},
                { name: 'FieldNumber', type: 'string'},
                { name: 'Habitat', type: 'string'},
                { name: 'Month', type: 'string'},
                { name: 'SamplingEffort', type: 'string'},
                { name: 'SamplingProtocol', type: 'string'},
                { name: 'Start', type: 'string'},
                { name: 'StartDayOfYear', type: 'string'},
                { name: 'VerbatimEventDate', type: 'string'},
                { name: 'Year', type: 'string'},
                { name: 'Bed', type: 'string'},
                { name: 'EarliestAgeOrLowestStage', type: 'string'},
                { name: 'EarliestEonOrLowestEonothem', type: 'string'},
                { name: 'EarliestEpochOrLowestSeries', type: 'string'},
                { name: 'EarliestEraOrLowestErathem', type: 'string'},
                { name: 'EarliestPeriodOrLowestSystem', type: 'string'},
                { name: 'Formation', type: 'string'},
                { name: 'GeologicalContextID', type: 'string'},
                { name: 'Group', type: 'string'},
                { name: 'HighestBiostratigraphicZone', type: 'string'},
                { name: 'LatestAgeOrHighestStage', type: 'string'},
                { name: 'LatestEonOrHighestEonothem', type: 'string'},
                { name: 'LatestEpochOrHighestSeries', type: 'string'},
                { name: 'LatestEraOrHighestErathem', type: 'string'},
                { name: 'LatestPeriodOrHighestSystem', type: 'string'},
                { name: 'LithostratigraphicTerms', type: 'string'},
                { name: 'LowestBiostratigraphicZone', type: 'string'},
                { name: 'Member', type: 'string'},
                { name: 'Id', type: 'string'},
                { name: 'DateIdentified', type: 'string'},
                { name: 'IdentificationID', type: 'string'},
                { name: 'IdentificationQualifier', type: 'string'},
                { name: 'IdentificationReferences', type: 'string'},
                { name: 'IdentificationRemarks', type: 'string'},
                { name: 'IdentificationVerificationStatus', type: 'string'},
                { name: 'IdentifiedBy', type: 'string'},
                { name: 'TypeStatus', type: 'string'},
                { name: 'UncertainDetermination', type: 'string'},
                { name: 'InformationWithheld', type: 'string'},
                { name: 'InstitutionCode', type: 'string'},
                { name: 'InstitutionID', type: 'string'},
                { name: 'Language', type: 'string'},
                { name: 'Continent', type: 'string'},
                { name: 'CoordinateM', type: 'string'},
                { name: 'CoordinatePrecision', type: 'string'},
                { name: 'CoordinateSystemWkt', type: 'string'},
                { name: 'CoordinateUncertaintyInMeters', type: 'string'},
                { name: 'CoordinateX', type: 'string'},
                { name: 'CoordinateY', type: 'string'},
                { name: 'CoordinateZ', type: 'string'},
                { name: 'Country', type: 'string'},
                { name: 'CountryCode', type: 'string'},
                { name: 'County', type: 'string'},
                { name: 'DecimalLatitude', type: 'string'},
                { name: 'DecimalLongitude', type: 'string'},
                { name: 'FootprintSpatialFit', type: 'string'},
                { name: 'FootprintSRS', type: 'string'},
                { name: 'FootprintWKT', type: 'string'},
                { name: 'GeodeticDatum', type: 'string'},
                { name: 'GeoreferencedBy', type: 'string'},
                { name: 'GeoreferencedDate', type: 'string'},
                { name: 'GeoreferenceProtocol', type: 'string'},
                { name: 'GeoreferenceRemarks', type: 'string'},
                { name: 'GeoreferenceSources', type: 'string'},
                { name: 'GeoreferenceVerificationStatus', type: 'string'},
                { name: 'HigherGeography', type: 'string'},
                { name: 'HigherGeographyID', type: 'string'},
                { name: 'Island', type: 'string'},
                { name: 'IslandGroup', type: 'string'},
                { name: 'Locality', type: 'string'},
                { name: 'LocationAccordingTo', type: 'string'},
                { name: 'LocationId', type: 'string'},
                { name: 'LocationRemarks', type: 'string'},
                { name: 'LocationURL', type: 'string'},
                { name: 'MaximumDepthInMeters', type: 'string'},
                { name: 'MaximumDistanceAboveSurfaceInMeters', type: 'string'},
                { name: 'MaximumElevationInMeters', type: 'string'},
                { name: 'MinimumDepthInMeters', type: 'string'},
                { name: 'MinimumDistanceAboveSurfaceInMeters', type: 'string'},
                { name: 'MinimumElevationInMeters', type: 'string'},
                { name: 'Municipality', type: 'string'},
                { name: 'Parish', type: 'string'},
                { name: 'PointRadiusSpatialFit', type: 'string'},
                { name: 'StateProvince', type: 'string'},
                { name: 'VerbatimCoordinates', type: 'string'},
                { name: 'VerbatimCoordinateSystem', type: 'string'},
                { name: 'VerbatimDepth', type: 'string'},
                { name: 'VerbatimElevation', type: 'string'},
                { name: 'VerbatimLatitude', type: 'string'},
                { name: 'VerbatimLocality', type: 'string'},
                { name: 'VerbatimLongitude', type: 'string'},
                { name: 'VerbatimSRS', type: 'string'},
                { name: 'WaterBody', type: 'string'},
                { name: 'MeasurementAccuracy', type: 'string'},
                { name: 'MeasurementDeterminedBy', type: 'string'},
                { name: 'MeasurementDeterminedDate', type: 'string'},
                { name: 'MeasurementID', type: 'string'},
                { name: 'MeasurementMethod', type: 'string'},
                { name: 'MeasurementRemarks', type: 'string'},
                { name: 'MeasurementType', type: 'string'},
                { name: 'MeasurementUnit', type: 'string'},
                { name: 'MeasurementValue', type: 'string'},
                { name: 'Modified', type: 'string'},
                { name: 'AssociatedMedia', type: 'string'},
                { name: 'AssociatedOccurrences', type: 'string'},
                { name: 'AssociatedReferences', type: 'string'},
                { name: 'AssociatedSequences', type: 'string'},
                { name: 'AssociatedTaxa', type: 'string'},
                { name: 'Behavior', type: 'string'},
                { name: 'CatalogNumber', type: 'string'},
                { name: 'Disposition', type: 'string'},
                { name: 'EstablishmentMeans', type: 'string'},
                { name: 'IndividualCount', type: 'string'},
                { name: 'IndividualID', type: 'string'},
                { name: 'IsNaturalOccurrence', type: 'string'},
                { name: 'IsNeverFoundObservation', type: 'string'},
                { name: 'IsNotRediscoveredObservation', type: 'string'},
                { name: 'IsPositiveObservation', type: 'string'},
                { name: 'LifeStage', type: 'string'},
                { name: 'OccurrenceID', type: 'string'},
                { name: 'OccurrenceRemarks', type: 'string'},
                { name: 'OccurrenceStatus', type: 'string'},
                { name: 'OccurrenceURL', type: 'string'},
                { name: 'OtherCatalogNumbers', type: 'string'},
                { name: 'Preparations', type: 'string'},
                { name: 'PreviousIdentifications', type: 'string'},
                { name: 'Quantity', type: 'string'},
                { name: 'QuantityUnit', type: 'string'},
                { name: 'RecordedBy', type: 'string'},
                { name: 'RecordNumber', type: 'string'},
                { name: 'ReproductiveCondition', type: 'string'},
                { name: 'Sex', type: 'string'},
                { name: 'Substrate', type: 'string'},
                { name: 'Owner', type: 'string'},
                { name: 'OwnerInstitutionCode', type: 'string'},
                { name: 'IsPublic', type: 'string'},
                { name: 'ProjectCategory', type: 'string'},
                { name: 'ProjectDescription', type: 'string'},
                { name: 'ProjectEndDate', type: 'string'},
                { name: 'ProjectID', type: 'string'},
                { name: 'ProjectName', type: 'string'},
                { name: 'ProjectOwner', type: 'string'},
                { name: 'ProjectStartDate', type: 'string'},
                { name: 'ProjectURL', type: 'string'},
                { name: 'SurveyMethod', type: 'string'},
                { name: 'References', type: 'string'},
                { name: 'ReportedBy', type: 'string'},
                { name: 'RelatedResourceID', type: 'string'},
                { name: 'RelationshipAccordingTo', type: 'string'},
                { name: 'RelationshipEstablishedDate', type: 'string'},
                { name: 'RelationshipOfResource', type: 'string'},
                { name: 'RelationshipRemarks', type: 'string'},
                { name: 'ResourceID', type: 'string'},
                { name: 'ResourceRelationshipID', type: 'string'},
                { name: 'Rights', type: 'string'},
                { name: 'RightsHolder', type: 'string'},
                { name: 'SpeciesObservationURL', type: 'string'},
                { name: 'AcceptedNameUsage', type: 'string'},
                { name: 'AcceptedNameUsageID', type: 'string'},
                { name: 'Class', type: 'string'},
                { name: 'DyntaxaTaxonID', type: 'string'},
                { name: 'Family', type: 'string'},
                { name: 'Genus', type: 'string'},
                { name: 'HigherClassification', type: 'string'},
                { name: 'InfraspecificEpithet', type: 'string'},
                { name: 'Kingdom', type: 'string'},
                { name: 'NameAccordingTo', type: 'string'},
                { name: 'NameAccordingToID', type: 'string'},
                { name: 'NamePublishedIn', type: 'string'},
                { name: 'NamePublishedInID', type: 'string'},
                { name: 'NamePublishedInYear', type: 'string'},
                { name: 'NomenclaturalCode', type: 'string'},
                { name: 'NomenclaturalStatus', type: 'string'},
                { name: 'Order', type: 'string'},
                { name: 'OrganismGroup', type: 'string'},
                { name: 'OriginalNameUsage', type: 'string'},
                { name: 'OriginalNameUsageID', type: 'string'},
                { name: 'ParentNameUsage', type: 'string'},
                { name: 'ParentNameUsageID', type: 'string'},
                { name: 'Phylum', type: 'string'},
                { name: 'ScientificName', type: 'string'},
                { name: 'ScientificNameAuthorship', type: 'string'},
                { name: 'ScientificNameID', type: 'string'},
                { name: 'SpecificEpithet', type: 'string'},
                { name: 'Subgenus', type: 'string'},
                { name: 'TaxonConceptID', type: 'string'},
                { name: 'TaxonConceptStatus', type: 'string'},
                { name: 'TaxonID', type: 'string'},
                { name: 'TaxonomicStatus', type: 'string'},
                { name: 'TaxonRank', type: 'string'},
                { name: 'TaxonRemarks', type: 'string'},
                { name: 'TaxonSortOrder', type: 'string'},
                { name: 'TaxonURL', type: 'string'},
                { name: 'VerbatimScientificName', type: 'string'},
                { name: 'VerbatimTaxonRank', type: 'string'},
                { name: 'VernacularName', type: 'string'},
                { name: 'Type', type: 'string'},
                { name: 'ValidationStatus', type: 'string'}

            ]
        });   
    };

})(AnalysisPortal.Models);

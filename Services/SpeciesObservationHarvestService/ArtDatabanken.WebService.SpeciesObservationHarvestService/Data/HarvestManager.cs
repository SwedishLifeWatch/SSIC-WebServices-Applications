using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Database;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using ArtDatabanken.WebService.SpeciesObservation.Database;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Database;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Artportalen;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif.BirdRingingCentre;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif.EntomologicalCollections;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif.HerbariumOfOskarshamn;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif.HerbariumOfUmeaUniversity;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif.LundBotanicalMuseum;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif.LundMuseumOfZoology;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif.Porpoises;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif.SwedishMalaiseTrapProject;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Kul;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Mvm;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Nors;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Observationsdatabasen;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Sers;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Shark;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Wram;
using LogType = ArtDatabanken.WebService.Data.LogType;
using SpeciesObservationElasticsearchData = ArtDatabanken.WebService.SpeciesObservation.Database.SpeciesObservationElasticsearchData;
using WebDataType = ArtDatabanken.WebService.Data.WebDataType;
using WebTaxon = ArtDatabanken.WebService.Data.WebTaxon;
using WebTaxonTreeNode = ArtDatabanken.WebService.Data.WebTaxonTreeNode;
using WebTaxonTreeSearchCriteria = ArtDatabanken.WebService.Data.WebTaxonTreeSearchCriteria;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Data
{
    /// <summary>
    /// Manager of species observation harvest services.
    /// </summary>
    public partial class HarvestManager
    {
        // Constants used in this class
        // ----------------------------

        /// <summary>
        /// The Taxon URL.
        /// </summary>
        private const String TAXON_URL_PREFIX = "https://www.dyntaxa.se/Taxon/Info/";

        /// <summary>
        /// Parts of a scientific name.
        /// </summary>
        private const Int32 SPECIFIC_EPITHET = 1;

        /// <summary>
        /// Parts of a scientific name.
        /// </summary>
        private const Int32 INFRA_SPECIFIC_EPITHET = 2;

        /// <summary>
        /// Taxon category.
        /// </summary>
        private const Int32 CATEGORY_HYBRID = 21;

        /// <summary>
        ///  AF Class definition OrganismGroup.
        /// </summary>
        private const Int32 KLASSDEFINITION_KLASS = 10;

        /// <summary>
        ///  ArtFakta Class definition Swedish history.
        /// </summary>
        private const Int32 CLASS_DEFINITION_SWEDISH_HISTORY = 69;

        /// <summary>
        ///  ArtFakta Class definition Swedish occurrence.
        /// </summary>
        private const Int32 CLASS_DEFINITION_SWEDISH_OCCURRENCE = 68;

        /// <summary>
        /// The AreaDataset id that is used to get the polygons for validation that all observations is made in Sweden
        /// </summary>
        private const int AreaDatasetIdForSwedenPolygon = 25;

        /// <summary>
        /// The Feature id that is used to get the polygons for validation that all observations is made in Sweden
        /// </summary>
        private const string FeatureIdForSwedenPolygon = "1";

        /// <summary>
        /// The SpeciesObservationId.
        /// </summary>
        //public static Int64 SpeciesObservationId;

        /// <summary>
        /// The _taxon tree cache.
        /// </summary>
        private static Hashtable _taxonTreeCache, _taxonOriginalNameCache, _taxonCategoryCache, _organismGroupCache, _swedishHistoryCache, _swedishOccurrenceCache;

        /// <summary>
        /// The connectors.
        /// Key is data provider id.
        /// </summary>
        private static readonly Dictionary<int, IDataProviderConnector> Connectors;

        /// <summary>
        /// The lock object.
        /// </summary>
        private static readonly WebData LockObject;

        /// <summary>
        /// The polygons that is used to validate that all observations is made in Sweden
        /// </summary>
        private static List<WebRegionGeography> _sweden;

        /// <summary>
        /// Status of current harvest job.
        /// </summary>
        public static HarvestStatusEnum CurrentStatus { get; set; }

        /// <summary>
        /// Request of change of current harvest job's status.
        /// </summary>
        public static HarvestStatusEnum RequestedStatus { get; set; }

        /// <summary>
        /// Controls if harvest thread should continue or stop.
        /// </summary>
        public static Boolean ShutdownThread { get; set; }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static HarvestManager()
        {
            Dictionary<int, IDataProviderConnector> connectors = new Dictionary<int, IDataProviderConnector>();
            IDataProviderConnector connector = new ArtportalenConnector();
            connectors[(int)(SpeciesObservationDataProviderId.SpeciesGateway)] = connector;
            connector = new MvmConnector();
            connectors[(int)(SpeciesObservationDataProviderId.Mvm)] = connector;
            connector = new NorsConnector();
            connectors[(int)(SpeciesObservationDataProviderId.Nors)] = connector;
            connector = new KulConnector();
            connectors[(int)(SpeciesObservationDataProviderId.Kul)] = connector;
            connector = new ObservationsdatabasenConnector();
            connectors[(int)(SpeciesObservationDataProviderId.Observationsdatabasen)] = connector;
            connector = new SersConnector();
            connectors[(int)(SpeciesObservationDataProviderId.Sers)] = connector;
            connector = new WramConnector();
            connectors[(int)(SpeciesObservationDataProviderId.Wram)] = connector;
            connector = new SharkConnector();
            connectors[(int)(SpeciesObservationDataProviderId.Shark)] = connector;
            connector = new LundBotanicalMuseumConnector();
            connectors[(int)(SpeciesObservationDataProviderId.LundBotanicalMuseum)] = connector;
            connector = new LundMuseumOfZoologyConnector();
            connectors[(int)(SpeciesObservationDataProviderId.LundMuseumOfZoology)] = connector;
            connector = new BirdRingingCentreConnector();
            connectors[(int)(SpeciesObservationDataProviderId.BirdRingingCentre)] = connector;
            connector = new PorpoisesConnector();
            connectors[(int)(SpeciesObservationDataProviderId.Porpoises)] = connector;
            connector = new HerbariumOfOskarshamnConnector();
            connectors[(int)(SpeciesObservationDataProviderId.HerbariumOfOskarshamn)] = connector;
            connector = new HerbariumOfUmeaUniversityConnector();
            connectors[(int)(SpeciesObservationDataProviderId.HerbariumOfUmeaUniversity)] = connector;
            connector = new EntomologicalCollectionsConnector();
            connectors[(int)(SpeciesObservationDataProviderId.EntomologicalCollections)] = connector;
            connector = new SwedishMalaiseTrapProjectConnector();
            connectors[(int)(SpeciesObservationDataProviderId.SwedishMalaiseTrapProject)] = connector;
            Connectors = connectors;
            LockObject = new WebData();
            SpeciesObservationCoordinateSystem = new WebCoordinateSystem
            {
                Id = CoordinateSystemId.WGS84
            };
        }

        /// <summary>
        /// Coordinate system used for species observations that 
        /// are stored in SwedishSpeciesObservation database.
        /// </summary>
        public static WebCoordinateSystem SpeciesObservationCoordinateSystem { get; set; }

        /// <summary>
        /// Add species observation field to
        /// species observation field table.
        /// </summary>
        /// <param name="speciesObservationFieldTable">Species observation field table.</param>
        /// <param name="speciesObservationField">Species observation field.</param>
        /// <param name="speciesObservationId">Species observation id.</param>
        /// <param name="catalogNumber">The Catalog number. </param>
        /// <param name="dataProviderId">The data provider id. </param>
        private static void AddSpeciesObservationField(DataTable speciesObservationFieldTable,
                                                       HarvestSpeciesObservationField speciesObservationField,
                                                       Int64 speciesObservationId,
                                                       string catalogNumber,
                                                       int dataProviderId)
        {
            // Skip to add field if DarwinCore is TRUE
            // and IsSearchable is FALSE
            if ((speciesObservationField.IsDarwinCore) && (!speciesObservationField.IsSearchable))
            {
                return;
            }

            DataRow speciesObservationFieldRow = speciesObservationFieldTable.NewRow();
            speciesObservationFieldRow[0] = speciesObservationId;
            speciesObservationFieldRow[1] = speciesObservationField.Class.GetClass();
            if (speciesObservationField.IsClassIndexSpecified)
            {
                speciesObservationFieldRow[2] = speciesObservationField.ClassIndex;
            }

            if (speciesObservationField.Locale.IsNotNull())
            {
                speciesObservationFieldRow[3] = speciesObservationField.Locale.Id;
            }

            speciesObservationFieldRow[4] = speciesObservationField.Information;
            speciesObservationFieldRow[5] = speciesObservationField.Property.GetProperty();
            if (speciesObservationField.IsPropertyIndexSpecified)
            {
                speciesObservationFieldRow[6] = speciesObservationField.PropertyIndex;
            }

            speciesObservationFieldRow[7] = (Int32)(speciesObservationField.Type);
            speciesObservationFieldRow[8] = speciesObservationField.Unit;
            speciesObservationFieldRow[9] = speciesObservationField.Value;
            speciesObservationFieldRow[10] = catalogNumber;
            speciesObservationFieldRow[11] = dataProviderId;

            try
            {
                switch (speciesObservationField.Type)
                {
                    case WebDataType.Float64:
                        speciesObservationFieldRow[12] = speciesObservationField.Value.Replace(",", ".").WebParseDouble(); // double
                        break;

                    case WebDataType.Int32:
                    case WebDataType.Int64:
                        speciesObservationFieldRow[13] = (Int64)speciesObservationField.Value.Replace(",", ".").WebParseDouble(); // int    
                        break;

                    case WebDataType.Boolean:
                        speciesObservationFieldRow[14] = speciesObservationField.Value.WebParseBoolean(); // boolean
                        break;

                    case WebDataType.DateTime:
                        speciesObservationFieldRow[15] = speciesObservationField.Value.WebParseDateTime(); // datetime
                        break;

                    case WebDataType.String:
                        if (speciesObservationField.Value.Length > 440)
                        {
                            speciesObservationFieldRow[16] = speciesObservationField.Value.Substring(0, 440); // nvarchar(440)
                        }
                        else
                        {
                            speciesObservationFieldRow[16] = speciesObservationField.Value;
                        }

                        break;
                }
            }
            catch (FormatException)
            {
                // Store value in column value_string in same way as in case WebDataType.String:
                speciesObservationFieldRow[7] = (Int32)(WebDataType.String);

                if (speciesObservationField.Value.Length > 440)
                {
                    speciesObservationFieldRow[16] = speciesObservationField.Value.Substring(0, 440); // nvarchar(440)
                }
                else
                {
                    speciesObservationFieldRow[16] = speciesObservationField.Value;
                }

                Debug.WriteLine(speciesObservationField.Value + " could not be converted to " + speciesObservationField.Type);
            }

            speciesObservationFieldTable.Rows.Add(speciesObservationFieldRow);
        }

        /// <summary>
        /// Add species observation field to
        /// species observation field table.
        /// </summary>
        /// <param name="speciesObservationErrorFieldTable">
        /// Species observation field table.
        /// </param>
        /// <param name="speciesObservationErrorField">
        /// Species observation field.
        /// </param>
        /// <param name="speciesObservationId">
        /// Species observation id. 
        /// </param>
        /// <param name="dataProviderId">
        /// Data provider id. 
        /// </param>
        /// <param name="dataProvider">
        /// Data provider name. 
        /// </param>
        /// <param name="error">
        /// The error. 
        /// </param>
        /// <param name="transactionType">
        /// Type of transaction. 
        /// </param>
        private static void AddSpeciesObservationFieldError(DataTable speciesObservationErrorFieldTable,
                                                            HarvestSpeciesObservationField speciesObservationErrorField,
                                                            string speciesObservationId,
                                                            Int32 dataProviderId,
                                                            string dataProvider,
                                                            string error,
                                                            string transactionType)
        {
            DataRow speciesObservationErrorFieldRow = speciesObservationErrorFieldTable.NewRow();

            speciesObservationErrorFieldRow[0] = speciesObservationId;
            speciesObservationErrorFieldRow[1] = speciesObservationErrorField.Class.GetClass();
            if (speciesObservationErrorField.IsClassIndexSpecified)
            {
                speciesObservationErrorFieldRow[2] = speciesObservationErrorField.ClassIndex.WebToString();
            }

            if (speciesObservationErrorField.Locale.IsNotNull())
            {
                speciesObservationErrorFieldRow[3] = speciesObservationErrorField.Locale.Id.WebToString();
            }

            if (speciesObservationErrorField.Information.IsNotNull())
            {
                speciesObservationErrorFieldRow[4] = speciesObservationErrorField.Information;
            }

            speciesObservationErrorFieldRow[5] = speciesObservationErrorField.Property.GetProperty();
            if (speciesObservationErrorField.IsPropertyIndexSpecified)
            {
                speciesObservationErrorFieldRow[6] = speciesObservationErrorField.PropertyIndex.WebToString();
            }

            speciesObservationErrorFieldRow[7] = speciesObservationErrorField.Type.ToString();
            speciesObservationErrorFieldRow[8] = speciesObservationErrorField.Unit;
            speciesObservationErrorFieldRow[9] = speciesObservationErrorField.Value;

            speciesObservationErrorFieldRow[10] = dataProviderId;
            speciesObservationErrorFieldRow[11] = dataProvider;
            speciesObservationErrorFieldRow[12] = error;
            speciesObservationErrorFieldRow[13] = transactionType;

            speciesObservationErrorFieldTable.Rows.Add(speciesObservationErrorFieldRow);
        }

        /// <summary>
        /// Create new species observation.
        /// </summary>
        /// <param name="speciesObservation">Created species observation.</param>
        /// <param name="darwinCoreTable">Darwin core table.</param>
        /// <param name="speciesObservationFieldTable">Species observation field table.</param>
        /// <param name="speciesObservationId">Species observation id.</param>
        /// <param name="dataProvider">Species observation data source.</param>
        /// <param name="catalogNumber">The catalog number. </param>
        internal static void AddToTempSpeciesObservation(HarvestSpeciesObservation speciesObservation,
                                                        DataTable darwinCoreTable,
                                                        DataTable speciesObservationFieldTable,
                                                        Int64 speciesObservationId,
                                                        WebSpeciesObservationDataProvider dataProvider,
                                                        string catalogNumber)
        {
            if (speciesObservation.Fields.IsNotEmpty())
            {
                DataRow darwinCoreRow = darwinCoreTable.NewRow();
                darwinCoreRow[(Int32)(DarwinCoreColumn.DataProviderId)] = dataProvider.Id;
                darwinCoreRow[(Int32)(DarwinCoreColumn.Id)] = speciesObservationId;
                foreach (HarvestSpeciesObservationField speciesObservationField in speciesObservation.Fields)
                {
                    try
                    {
                        AddSpeciesObservationField(speciesObservationFieldTable,
                                                   speciesObservationField,
                                                   speciesObservationId,
                                                   catalogNumber,
                                                   dataProvider.Id);
                        AddSpeciesObservationField(darwinCoreRow,
                                                   speciesObservationField);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(speciesObservationField.Property.Id.ToString(), ex);
                    }
                }

                darwinCoreTable.Rows.Add(darwinCoreRow);
            }
        }

        /// <summary>
        /// Create new species observation.
        /// </summary>
        /// <param name="deleteTable">Delete table.</param>
        /// <param name="dataProvider">Species observation data source.</param>
        /// <param name="catalogNumber">The Catalog number.</param>
        internal static void AddToTempDelete(DataTable deleteTable, WebSpeciesObservationDataProvider dataProvider, string catalogNumber)
        {
            DataRow deleteRow = deleteTable.NewRow();
            deleteRow[0] = -1; // This indicates temporary ID is not set. to be updated in TempDeleteSpeciesObservation in database
            deleteRow[1] = catalogNumber;
            deleteRow[2] = dataProvider.Id;

            deleteTable.Rows.Add(deleteRow);
        }

        /// <summary>
        /// The add taxa to cache.
        /// </summary>
        /// <param name="taxonTreeNode">
        /// The taxon tree node.
        /// </param>
        private static void AddTaxaToCache(WebTaxonTreeNode taxonTreeNode)
        {
            if (!_taxonTreeCache.ContainsKey(taxonTreeNode.Taxon.Id))
            {
                _taxonTreeCache[taxonTreeNode.Taxon.Id] = taxonTreeNode;
                if (taxonTreeNode.Children.IsNotEmpty())
                {
                    foreach (WebTaxonTreeNode taxonChildTreeNode in taxonTreeNode.Children)
                    {
                        AddTaxaToCache(taxonChildTreeNode);
                    }
                }
            }
        }

        /// <summary>
        /// Check that Elasticsearch cluster is ok.
        /// </summary>
        /// <param name="context">Web service context.</param>
        /// <param name="elasticsearch">Proxy to Elasticsearch.</param>
        private static void CheckHealth(WebServiceContext context,
                                        ElasticsearchSpeciesObservationProxy elasticsearch)
        {
            while (!(elasticsearch.IsClusterOk()))
            {
                // Wait 5 minutes until Elasticsearch cluster is ok.
                WebServiceData.LogManager.Log(context, "Elasticsearch cluster is not ok, " + DateTime.Now.WebToString(), LogType.Information, null);
                Thread.Sleep(300000);
            }
        }

        /// <summary>
        /// Check that species observations in SQL Server and Elasticsearch are synchronized.
        /// If they are not synchronized fix the problems.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void CheckSpeciesObservations(WebServiceContext context)
        {
            ElasticsearchSpeciesObservationProxy elasticsearch;
            List<Int64> speciesObservationAdd, speciesObservationDelete;
            SpeciesObservationElasticsearch speciesObservationElasticsearch;

            // Empty temporary Elasticsearch tables.
            context.GetSpeciesObservationDatabase().EmptyTempElasticsearchTables();

            // Get all species observations ids from Elasticsearch into SQL Server.
            speciesObservationElasticsearch = WebSpeciesObservationServiceData.SpeciesObservationManager.GetSpeciesObservationElasticsearch(context);
            elasticsearch = new ElasticsearchSpeciesObservationProxy(speciesObservationElasticsearch.CurrentIndexName);
            GetSpeciesObservationIds(context, elasticsearch);

            // Get difference between Elasticsearch and SQL Server.
            speciesObservationAdd = new List<Int64>();
            speciesObservationDelete = new List<Int64>();
            GetSpeciesObservationDifference(context, speciesObservationAdd, speciesObservationDelete);

            // Update Elasticsearch with the difference.
            UpdateSpeciesObservationsElasticsearch(context,
                                                   speciesObservationAdd,
                                                   speciesObservationDelete,
                                                   elasticsearch);
        }

        /// <summary>
        /// Get difference between Elasticsearch and SQL Server.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationAdd">Ids for species observations that should be added to Elasticsearch.</param>
        /// <param name="speciesObservationDelete">Ids for species observations that should be deleted to Elasticsearch.</param>
        public static void GetSpeciesObservationDifference(WebServiceContext context,
                                                           List<Int64> speciesObservationAdd,
                                                           List<Int64> speciesObservationDelete)
        {
            using (DataReader dataReader = context.GetSpeciesObservationDatabase().GetSpeciesObservationDifference())
            {
                // Get species observations that should be added to Elasticsearch.
                while (dataReader.Read())
                {
                    speciesObservationAdd.Add(dataReader.GetInt64("Id"));
                }

                // Get species observations that should be deleted from Elasticsearch.
                dataReader.NextResultSet();
                while (dataReader.Read())
                {
                    speciesObservationDelete.Add(dataReader.GetInt64("Id"));
                }
            }
        }

        /// <summary>
        /// Get all species observations ids from Elasticsearch into SQL Server.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="elasticsearch">Elasticsearch proxy.</param>
        private static void GetSpeciesObservationIds(WebServiceContext context,
                                                     ElasticsearchSpeciesObservationProxy elasticsearch)
        {
            Boolean areMoreSpeciesObservationsAvailable;
            DataColumn column;
            DataRow row;
            DataTable speciesObservationTable;
            DateTime lastPause = DateTime.Now;
            Dictionary<String, WebSpeciesObservationField> mapping;
            DocumentFilterResponse documentFilterResponse;
            ElasticsearchScroll scroll;
            List<WebSpeciesObservation> speciesObservations;

            areMoreSpeciesObservationsAvailable = true;
            scroll = new ElasticsearchScroll();
            scroll.KeepAlive = 59;
            mapping = WebSpeciesObservationServiceData.SpeciesObservationManager.GetMapping(context, elasticsearch);

            while (areMoreSpeciesObservationsAvailable)
            {
                documentFilterResponse = elasticsearch.GetSpeciesObservations(null, scroll);
                if (documentFilterResponse.TimedOut)
                {
                    throw new Exception("Method GetSpeciesObservationsByScroll() timed out!");
                }

                speciesObservations = WebSpeciesObservationServiceData.SpeciesObservationManager.GetSpeciesObservations(documentFilterResponse.DocumentsJson,
                                                                                                                        mapping);
                areMoreSpeciesObservationsAvailable = speciesObservations.IsNotEmpty();
                if (areMoreSpeciesObservationsAvailable)
                {
                    speciesObservationTable = new DataTable("ElasticsearchSpeciesObservationAll");
                    column = new DataColumn("Id", typeof(Int64));
                    speciesObservationTable.Columns.Add(column);

                    foreach (WebSpeciesObservation speciesObservation in speciesObservations)
                    {
                        row = speciesObservationTable.NewRow();
                        row[0] = speciesObservation.Fields.GetField(SpeciesObservationClassId.DarwinCore, SpeciesObservationPropertyId.Id).Value.WebParseInt64();
                        speciesObservationTable.Rows.Add(row);
                    }

                    context.GetSpeciesObservationDatabase().AddTableData(context, speciesObservationTable);
                }

                if ((DateTime.Now - lastPause).TotalMinutes > 60)
                {
                    // Make a 10 minute break each hour.
                    Thread.Sleep(600000);
                    lastPause = DateTime.Now;
                }
            }
        }

        private static void ResetTaxaCache()
        {
            _taxonTreeCache = new Hashtable();
        }

        /// <summary>
        /// Creates an error record to keep track of observations that did not fulfill all demands.
        /// </summary>
        /// <param name="speciesObservation">
        /// Created species observation.
        /// </param>
        /// <param name="speciesObservationErrorFieldTable">
        /// Species observation field table.
        /// </param>
        /// <param name="dataProvider">
        /// Species observation data source.
        /// </param>
        /// <param name="errors">
        /// List of errors.
        /// </param>
        /// <param name="transactionType">
        /// The Transaction type.
        /// </param>
        internal static void CreateSpeciesObservationError(HarvestSpeciesObservation speciesObservation,
                                                          DataTable speciesObservationErrorFieldTable,
                                                          WebSpeciesObservationDataProvider dataProvider,
                                                          Dictionary<SpeciesObservationPropertyId, string> errors,
                                                          string transactionType)
        {
            if (speciesObservation.Fields.IsNotEmpty())
            {
                string speciesObservationId = errors[SpeciesObservationPropertyId.CatalogNumber];
                int dataProviderId = dataProvider.Id;
                string dataProviderName = dataProvider.Name;

                foreach (HarvestSpeciesObservationField speciesObservationField in speciesObservation.Fields)
                {
                    string errorString = String.Empty;

                    if (errors.ContainsKey(speciesObservationField.Property.Id))
                    {
                        errorString = errors[speciesObservationField.Property.Id];
                    }

                    AddSpeciesObservationFieldError(speciesObservationErrorFieldTable,
                                                    speciesObservationField,
                                                    speciesObservationId,
                                                    dataProviderId,
                                                    dataProviderName,
                                                    errorString,
                                                    transactionType);
                }
            }
        }

        /// <summary>
        /// Checks if a Observation is valid to save in SpeciesObservationDB.
        /// </summary>
        /// <param name="speciesObservation">
        /// Created species observation.
        /// </param>
        /// <param name="context">
        /// Web service request context.
        /// </param>
        /// <param name="errors">
        /// Dictionary of errors.
        /// </param>
        /// <param name="catalogNumber">
        /// The catalog number.
        /// </param>
        /// <returns>
        /// The status of the method.<see cref="bool"/>.
        /// </returns>
        internal static Boolean CheckSpeciesObservation(HarvestSpeciesObservation speciesObservation,
                                                       WebServiceContext context,
                                                       out Dictionary<SpeciesObservationPropertyId, string> errors,
                                                       out string catalogNumber)
        {
            bool result = true;
            catalogNumber = String.Empty;

            errors = new Dictionary<SpeciesObservationPropertyId, string>();

            int unitLength = context.GetSpeciesObservationDatabase().GetColumnLength(SpeciesObservationFieldData.TABLE_NAME, SpeciesObservationFieldData.UNIT);

            if (speciesObservation.Fields.IsEmpty())
            {
                return true;
            }

            // all mandatory fields that should exist in the observation
            var mandatoryDictionary = new Dictionary<SpeciesObservationPropertyId, bool>();

            foreach (HarvestSpeciesObservationField speciesObservationField in speciesObservation.Fields)
            {
                if (speciesObservationField.IsMandatory)
                {
                    mandatoryDictionary.Add(speciesObservationField.Property.Id, false);
                }
            }

            // mandatoryDictionary.Add(SpeciesObservationPropertyId.DyntaxaTaxonID, false);
            // mandatoryDictionary.Add(SpeciesObservationPropertyId.CoordinateUncertaintyInMeters, false);
            // mandatoryDictionary.Add(SpeciesObservationPropertyId.CoordinateX, false); xxx
            // mandatoryDictionary.Add(SpeciesObservationPropertyId.CoordinateY, false); xxx
            // mandatoryDictionary.Add(SpeciesObservationPropertyId.Start, false);
            // mandatoryDictionary.Add(SpeciesObservationPropertyId.End, false);
            // mandatoryDictionary.Add(SpeciesObservationPropertyId.ProtectionLevel, false); xxx
            // mandatoryDictionary.Add(SpeciesObservationPropertyId.CatalogNumber, false);
            foreach (HarvestSpeciesObservationField speciesObservationField in speciesObservation.Fields)
            {
                switch (speciesObservationField.Type)
                {
                    case WebDataType.Boolean:
                        if (speciesObservationField.Value.Trim() == "1")
                        {
                            speciesObservationField.Value = "True";
                        }

                        if (speciesObservationField.Value.Trim() == "0")
                        {
                            speciesObservationField.Value = "False";
                        }

                        if (
                            !((speciesObservationField.Value == "True") ||
                              (speciesObservationField.Value == "False")))
                        {
                            errors.Add(speciesObservationField.Property.Id, String.Format("Value: {0} is not Boolean.", speciesObservationField.Value));
                            result = false;
                        }

                        break;

                    case WebDataType.DateTime:
                        // Debug.WriteLine("DateTime");
                        try
                        {
                            speciesObservationField.Value.WebParseDateTime();
                        }
                        catch (Exception)
                        {
                            errors.Add(speciesObservationField.Property.Id,
                                String.Format("Field, {0}, value: {1}, is not a valid DateTime.", speciesObservationField.Property.Id, speciesObservationField.Value));

                            result = false;
                        }

                        break;

                    case WebDataType.Float64:
                        if (String.IsNullOrEmpty(speciesObservationField.Value))
                        {
                            result = false;
                            break;
                        }

                        try
                        {
                            speciesObservationField.Value.Replace(",", ".").WebParseDouble();
                        }
                        catch (Exception)
                        {
                            errors.Add(speciesObservationField.Property.Id,
                                String.Format("Field, {0}, value: {1}, is not a valid double.", speciesObservationField.Property.Id, speciesObservationField.Value));
                            result = false;
                        }

                        break;

                    case WebDataType.Int32:
                        // Debug.WriteLine("Int32");
                        break;

                    case WebDataType.Int64:
                        // Debug.WriteLine("Int64");
                        break;

                    case WebDataType.String:
                        // MAX equals to -1
                        // Todo: Detta behöver tas om hand då det ska gå att spara species observation fields oxå
                        // Todo: Dessa har ju alltid column length 440 om de är strängar

                        // Check Field Type and length
                        int columnLength;

                        // None => uses the table SpeciesObservationField and the column name is 'value_String'
                        if (speciesObservationField.Property.Id == SpeciesObservationPropertyId.None)
                        {
                            columnLength = context.GetSpeciesObservationDatabase().GetColumnLength(speciesObservationField.PersistedInTable, SpeciesObservationFieldData.VALUE_STRING);
                        }
                        else
                        {
                            columnLength = context.GetSpeciesObservationDatabase().GetColumnLength(speciesObservationField.PersistedInTable, speciesObservationField.Property.Id.ToString());
                        }

                        if (columnLength != -1)
                        {
                            if (speciesObservationField.Value.IsNull())
                            {
                                errors.Add(speciesObservationField.Property.Id,
                                    String.Format("Value for: {0}, is null.", speciesObservationField.Value));
                                result = false;
                                break;
                            }

                            if ((columnLength / 2) < speciesObservationField.Value.Length)
                            {
                                errors.Add(speciesObservationField.Property.Id,
                                    String.Format("Too long, {0}, value: {1}, is too long.", speciesObservationField.Value.Length, speciesObservationField.Value));
                                result = false;
                            }
                        }

                        break;

                    default:
                        result = false;
                        break;
                }

                // Save CatalogNumber since it is used to identify this observation
                if (speciesObservationField.Property.Id == SpeciesObservationPropertyId.CatalogNumber)
                {
                    mandatoryDictionary[speciesObservationField.Property.Id] = true;

                    if (String.IsNullOrEmpty(speciesObservationField.Value))
                    {
                        result = false;
                        catalogNumber = String.Format(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                        errors.Add(speciesObservationField.Property.Id, catalogNumber);
                    }
                    else
                    {
                        errors.Add(speciesObservationField.Property.Id, speciesObservationField.Value);
                        catalogNumber = speciesObservationField.Value;
                    }
                }
                else if (mandatoryDictionary.ContainsKey(speciesObservationField.Property.Id))
                {
                    // Check that the mandatory field is set
                    mandatoryDictionary[speciesObservationField.Property.Id] = true;

                    // If the mandatory field does not contain a value at this stage, then it's an error
                    if (String.IsNullOrEmpty(speciesObservationField.Value))
                    {
                        result = false;
                        if (!errors.ContainsKey(speciesObservationField.Property.Id))
                        {
                            errors.Add(speciesObservationField.Property.Id, String.Format("Mandatory field {0} is empty.", speciesObservationField.Property.Id));
                            Debug.WriteLine("Mandatory field {0} is empty.", speciesObservationField.Property.Id);
                        }
                        else
                        {
                            Debug.WriteLine("Mandatory field {0} is null", speciesObservationField.Property.Id);
                        }
                    }
                }

                // Check if Unit is used. 
                if (!String.IsNullOrEmpty(speciesObservationField.Unit))
                {
                    // Check that Unit is not to long for database
                    if (speciesObservationField.Unit.Length > unitLength)
                    {
                        result = false;
                        errors.Add(speciesObservationField.Property.Id, String.Format("Value in Unit, {0}, is to long for the database column.", speciesObservationField.Unit));
                    }
                }
            }

            // loop throug the mandatory fields and check that all are set, oterwise its a invalid record
            foreach (KeyValuePair<SpeciesObservationPropertyId, bool> id in mandatoryDictionary)
            {
                if (!id.Value)
                {
                    result = false;
                    errors.Add(id.Key, String.Format("Mandatory field {0} is missing.", id.Key));
                }
            }

            if (result)
            {
                // Check against Sweden polygon.
                double x = speciesObservation.Fields.FirstOrDefault(item => item.Property.Id == SpeciesObservationPropertyId.CoordinateX).Value.WebParseDouble();
                double y = speciesObservation.Fields.FirstOrDefault(item => item.Property.Id == SpeciesObservationPropertyId.CoordinateY).Value.WebParseDouble();
                WebPoint googleMercatorPoint = new WebPoint(x, y);

                if (!(GetSweden(context).IsPointInsideGeometry(context, new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator }, googleMercatorPoint)))
                {
                    result = false;
                    errors.Add(SpeciesObservationPropertyId.CoordinateX, string.Format("The coordinates (x, y) [{0}, {1}], are outside of Sweden.", x, y));
                }
            }

            return result;
        }

        /// <summary>
        /// Delete species observations from elasticsearch.
        /// </summary>
        /// <param name="deletedSpeciesObservations">Delete these species observations from Elasticsearch.</param>
        /// <param name="elasticsearch">Proxy to Elasticsearch.</param>
        private static void DeleteSpeciesObservationsElasticsearch(List<String> deletedSpeciesObservations,
                                                                   ElasticsearchSpeciesObservationProxy elasticsearch)
        {
            Int32 index = 0;

            if (deletedSpeciesObservations.IsNotEmpty())
            {
                foreach (String occurrenceId in deletedSpeciesObservations)
                {
                    index++;
                    elasticsearch.DeleteSpeciesObservation(occurrenceId);
                    if ((index%1000) == 999)
                    {
                        Thread.Sleep(60000);
                    }
                }
            }
        }

        /// <summary>
        /// Delete unnecessary species observation change information.
        /// </summary>
        /// <param name="context">Web service context.</param>
        public static void DeleteUnnecessaryChanges(WebServiceContext context)
        {
            context.GetSpeciesObservationDatabase().DeleteUnnecessaryChanges();
        }

        /// <summary>
        /// Get all unique child taxa.
        /// This method operates on current taxon tree in contrast to
        /// the full taxon tree with all taxon tree nodes.
        /// </summary>
        /// <param name="taxonTreeNode">Current child taxon tree node.</param>
        /// <param name="childTaxa">Accumulated child taxa so far.</param>
        internal static void GetChildTaxa(WebTaxonTreeNode taxonTreeNode,
                                         List<WebTaxon> childTaxa)
        {
            // Add the taxon for this taxon tree node.
            // TODO Fix Merge method -- GuNy
            // childTaxa.Merge(taxonTreeNode.Taxon);
            childTaxa.Add(taxonTreeNode.Taxon);

            if (taxonTreeNode.Children.IsNotEmpty())
            {
                // Add taxa for child taxon tree node.
                foreach (WebTaxonTreeNode childTaxonTreeNode in taxonTreeNode.Children)
                {
                    GetChildTaxa(childTaxonTreeNode, childTaxa);
                }
            }
        }

        /// <summary>
        /// Update database with changes in species observation.
        /// Loop over each connector and read all changes 
        /// Add data to create, update or delete data tables
        /// Write Data tables to Temporary tables in database
        /// Update ID, TaxonId and convert Points to wgs85 if needed
        /// Call DB procedures to Copy/Update/Delete from temporary tables to production tables.  
        /// </summary>
        /// <param name="context">
        /// Web service request context.
        /// </param>
        /// <param name="changedFrom">
        /// Start date for changes.
        /// </param>
        /// <param name="changedTo">
        /// End date for changes.
        /// </param>
        /// <param name="dataProviderIds">
        /// Update species observations for these data providers.
        /// </param>
        /// <param name="isChangedDatesSpecified">Notification if start and end dates are specified or not.</param>
        public static void StartSpeciesObservationUpdate(
            WebServiceContext context,
            DateTime changedFrom,
            DateTime changedTo,
            List<Int32> dataProviderIds,
            bool isChangedDatesSpecified)
        {
            DateTime validatedChangedFrom;
            DateTime validatedChangedTo;

            if (context.ClientToken.UserName != ApplicationIdentifier.SpeciesObservationHarvestService.ToString())
            {
                WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.WebServiceAdministrator);
            }

            RequestedStatus = HarvestStatusEnum.Working;

            // Read status for current job.
            var harvestJobManager = new HarvestJobManager(context);
            var harvestJob = harvestJobManager.GetHarvestJob();
            CurrentStatus = harvestJob.JobStatus; // No current job: CurrentStatus = DONE
            DateTime oldestBeginHarvestFromDate = GetOldestBeginHarvestFromDate(context, dataProviderIds);

            if (!isChangedDatesSpecified)
            {
                SetChangedDates(out changedFrom, out changedTo, oldestBeginHarvestFromDate);
            }
            else
            {
                ValidateChangedDates(changedFrom, changedTo, out validatedChangedFrom, out validatedChangedTo, oldestBeginHarvestFromDate);

                changedFrom = validatedChangedFrom;
                changedTo = validatedChangedTo;
            }

            // Start work when CurrentStatus is status Done or Stopped and RequestedStatus differ from CurrentStatus
            if ((CurrentStatus.Equals(HarvestStatusEnum.Done) || CurrentStatus.Equals(HarvestStatusEnum.Stopped)) &&
                !RequestedStatus.Equals(CurrentStatus))
            {
                // Clean meta data and statistics for old job
                harvestJobManager.CleanUpHarvestJob();

                // Set meta data for current job
                harvestJob = new HarvestJob
                {
                    JobStartDate = DateTime.Now,
                    JobEndDate = null,
                    HarvestStartDate = changedFrom,
                    HarvestCurrentDate = changedFrom,
                    HarvestEndDate = changedTo,
                    JobStatus = RequestedStatus,
                    DataProviders = new List<HarvestJobDataProvider>()
                };

                if (dataProviderIds.IsNotNull())
                {
                    foreach (var dataProviderId in dataProviderIds)
                    {
                        var dataProvider = new HarvestJobDataProvider { DataProviderId = dataProviderId, ChangeId = -1 };
                        harvestJob.DataProviders.Add(dataProvider);
                    }
                }

                harvestJobManager.SetHarvestJob(harvestJob);
                harvestJobManager.SetHarvestJobDataProvider(harvestJob);

                StartSpeciesObservationUpdateThread();
            }
        }

        /// <summary>
        /// Start a new thread.
        /// </summary>
        public static void StartSpeciesObservationUpdateThread()
        {
            // Start a new separate thread
            Task harvestTask = new Task(SpeciesObservationUpdateTask);
            harvestTask.Start(TaskScheduler.Default);
        }

        /// <summary>
        /// Get oldest BeginHarvestFromDate by selected data providers.
        /// </summary>
        /// <param name="context">Web service context.</param>
        /// <param name="dataProviderIds">Selected data providers.</param>
        /// <returns>Begin harvest from date.</returns>
        private static DateTime GetOldestBeginHarvestFromDate(WebServiceContext context, IEnumerable<Int32> dataProviderIds)
        {
            // Get active providers to get latest harvest date
            IEnumerable<WebSpeciesObservationDataProvider> activeDataProviders = GetActiveDataProviders(context);

            activeDataProviders = activeDataProviders.Where(dp => dataProviderIds.Contains(dp.Id));

            // Get the oldest BeginHarvestFromDate for selected providers
            DateTime oldestBeginHarvestFromDate = activeDataProviders.Min(dp => dp.BeginHarvestFromDate);

            return oldestBeginHarvestFromDate.Date;
        }

        /// <summary>
        /// Update species observations.
        /// Used in own thread.
        /// </summary>
        private static void SpeciesObservationUpdateTask()
        {
            WebServiceContext context = new WebServiceContextCached(WebServiceData.WebServiceManager.Name, ApplicationIdentifier.ArtDatabankenSOA.ToString());
            DateTime validatedChangedFrom;
            DateTime validatedChangedTo;
            Boolean isChangedFromDateUpdated = false;
            Boolean resetMaxChangeId;

            // Read meta data for current job
            var harvestJobManager = new HarvestJobManager(context);
            var harvestJob = harvestJobManager.GetHarvestJob();
            CurrentStatus = harvestJob.JobStatus;
            DateTime changedFrom = harvestJob.HarvestCurrentDate.Date;
            DateTime changedTo = harvestJob.HarvestEndDate.Date;
            var dataProviderIds = harvestJob.DataProviders.Select(harvestJobDataProvider => harvestJobDataProvider.DataProviderId).ToList();
            Boolean isChangedDatesSpecified = !changedFrom.Equals(DateTime.MinValue) && !changedTo.Equals(DateTime.MinValue);
            DateTime oldestBeginHarvestFromDate = GetOldestBeginHarvestFromDate(context, dataProviderIds);

            // Validate dates
            if (!isChangedDatesSpecified)
            {
                SetChangedDates(out changedFrom, out changedTo, oldestBeginHarvestFromDate);
            }
            else
            {
                ValidateChangedDates(changedFrom, changedTo, out validatedChangedFrom, out validatedChangedTo, oldestBeginHarvestFromDate);

                changedFrom = validatedChangedFrom;
                changedTo = validatedChangedTo;
            }

            resetMaxChangeId = false; // SetResetMaxChangeIdByJobStatus();

            if (ValidateJobStatus())
            {
                return; // Stop thread
            }

            CurrentStatus = HarvestStatusEnum.Working;
            harvestJob.JobStatus = HarvestStatusEnum.Working;
            harvestJobManager.SetHarvestJob(harvestJob);

            // Harvest for each date
            while (changedFrom <= changedTo)
            {
                if (Configuration.InstallationType == InstallationType.Production)
                {
                    // Do not harvest between 23:50 and 06:00.
                    // Do not harvest between 07:00 and 17:00 on workdays.
                    // Do not harvest between 19:00 and 22:00.
                    while (((DateTime.Now.TimeOfDay.Hours == 23) &&
                            (DateTime.Now.TimeOfDay.Minutes > 50)) ||
                           ((DateTime.Now.TimeOfDay.Hours >= 0) &&
                            (DateTime.Now.TimeOfDay.Hours <= 5)) ||
                           ((DateTime.Now.TimeOfDay.Hours >= 7) &&
                            (DateTime.Now.TimeOfDay.Hours < 17) &&
                            (DateTime.Now.DayOfWeek >= DayOfWeek.Monday) &&
                            (DateTime.Now.DayOfWeek <= DayOfWeek.Friday))) // ||
                                                                           //((DateTime.Now.TimeOfDay.Hours >= 19) &&
                                                                           // (DateTime.Now.TimeOfDay.Hours <= 21)))
                    {
                        // wait a minute...
                        Thread.Sleep(60000);

                        // Check if harvest thread should be stoped.
                        if (ValidateJobStatus() ||
                            (RequestedStatus != HarvestStatusEnum.Working) ||
                            ShutdownThread)
                        {
                            harvestJob.JobStatus = RequestedStatus;
                            CurrentStatus = RequestedStatus;
                            AbortHarvestJob(changedFrom, harvestJob, context, harvestJobManager, dataProviderIds);

                            // Stop harvest thread.
                            return;
                        }
                    }
                }
                else
                {
                    // Do not harvest between 18:00 and 04:00.
                    // while ((DateTime.Now.TimeOfDay.Hours >= 18) ||
                    //       (DateTime.Now.TimeOfDay.Hours <= 3))
                    {
                        // wait a minute...
                        Thread.Sleep(60000);

                        // Check if harvest thread should be stoped.
                        if (ValidateJobStatus() ||
                            (RequestedStatus != HarvestStatusEnum.Working) ||
                            ShutdownThread)
                        {
                            harvestJob.JobStatus = RequestedStatus;
                            CurrentStatus = RequestedStatus;
                            AbortHarvestJob(changedFrom, harvestJob, context, harvestJobManager, dataProviderIds);

                            // Stop harvest thread.
                            return;
                        }
                    }
                }

                // Continue with harvest job until another status request is made
                if (RequestedStatus.Equals(CurrentStatus) && !ShutdownThread)
                {
                    DoHarvestJob(changedFrom, harvestJob, context, harvestJobManager, dataProviderIds, resetMaxChangeId);

                    // Continue with next day
                    changedFrom = changedFrom.AddDays(1);
                    isChangedFromDateUpdated = true;

                    // Reset max change id only for first date in date interval
                    resetMaxChangeId = false;
                }
                else
                {
                    AbortHarvestJob(changedFrom, harvestJob, context, harvestJobManager, dataProviderIds);

                    return; // Stop thread
                }

                // Let other processes be made during harvest time; such as status requests from end user.
                Thread.Sleep(60000);
            }

            CompleteHarvestJob(isChangedFromDateUpdated, changedFrom, harvestJob, harvestJobManager);
        }

        /// <summary>
        /// Update current status or requested status in different scenario.
        /// </summary>
        /// <returns>True will shutdown thread.</returns>
        private static Boolean ValidateJobStatus()
        {
            Boolean stopThread = false;

            // Validate status
            switch (RequestedStatus)
            {
                case HarvestStatusEnum.Done:
                    // Check if current status is set as Working (case when web service has been restarted)
                    if (CurrentStatus.Equals(HarvestStatusEnum.Working))
                    {
                        RequestedStatus = CurrentStatus; // Continue harvest job
                    }
                    else
                    {
                        stopThread = true; // Stop thread
                    }

                    break;
                case HarvestStatusEnum.Working:
                    // Check if current status is set as Paused (case when job is paused and should continue)
                    if (CurrentStatus.Equals(HarvestStatusEnum.Paused))
                    {
                        CurrentStatus = RequestedStatus; // Continue harvest job
                    }

                    break;
            }

            return stopThread;
        }

        /// <summary>
        /// Decide if max change id should be reset or not by job status.
        /// </summary>
        /// <returns>False means keep latest max change id. True means reset max change id for provider.</returns>
        private static Boolean SetResetMaxChangeIdByJobStatus()
        {
            Boolean resetMaxChangeId = true;

            // Validate status
            switch (RequestedStatus)
            {
                case HarvestStatusEnum.Done:
                    // Check if current status is set as Working (case when web service has been restarted)
                    if (CurrentStatus.Equals(HarvestStatusEnum.Working))
                    {
                        resetMaxChangeId = false; // Keep latest max change id
                    }
                    break;
                case HarvestStatusEnum.Working:
                    // Check if current status is set as Paused (case when job is paused and should continue)
                    if (CurrentStatus.Equals(HarvestStatusEnum.Paused))
                    {
                        resetMaxChangeId = false; // Keep latest max change id
                    }
                    break;
            }

            return resetMaxChangeId;
        }

        /// <summary>
        /// Update meta data for current job.
        /// </summary>
        /// <param name="isChangedFromDateUpdated">If true then set changedFrom back one day.</param>
        /// <param name="changedFrom">Changed from date.</param>
        /// <param name="harvestJob">Contains meta data about harvest job.</param>
        /// <param name="harvestJobManager">Handles harvest job objects.</param>
        private static void CompleteHarvestJob(bool isChangedFromDateUpdated, DateTime changedFrom, HarvestJob harvestJob, HarvestJobManager harvestJobManager)
        {
            // Job is completed
            RequestedStatus = HarvestStatusEnum.Done;

            // Remove a day since harvest was not performed the last date
            if (isChangedFromDateUpdated)
            {
                changedFrom = changedFrom.AddDays(-1);
            }

            // Set meta data for current job
            harvestJob.JobEndDate = DateTime.Now;
            harvestJob.HarvestCurrentDate = changedFrom;
            harvestJob.JobStatus = RequestedStatus;
            harvestJobManager.SetHarvestJob(harvestJob);
        }

        /// <summary>
        /// Update meta data for current harvest job and log about progress.
        /// </summary>
        /// <param name="changedFrom">Changed from date.</param>
        /// <param name="harvestJob">Contains meta data about harvest job.</param>
        /// <param name="context">Web service context.</param>
        /// <param name="harvestJobManager">Handles harvest job objects.</param>
        /// <param name="dataProviderIds">Contains current harvest job's data provider ids.</param>
        private static void AbortHarvestJob(DateTime changedFrom, HarvestJob harvestJob, WebServiceContext context, HarvestJobManager harvestJobManager, List<int> dataProviderIds)
        {
            // Job is aborted by some reason

            // Set meta data for current job
            harvestJob.JobEndDate = DateTime.Now;
            harvestJob.HarvestCurrentDate = changedFrom;
            harvestJob.JobStatus = RequestedStatus;
            harvestJobManager.SetHarvestJob(harvestJob);

            // Log data provider harvest status
            context.GetSpeciesObservationDatabase().SetHarvestJobStatistics(dataProviderIds, harvestJob.JobStatus, changedFrom);

            RequestedStatus = HarvestStatusEnum.Done;
        }

        /// <summary>
        /// Do actual harvest job and log about progress.
        /// </summary>
        /// <param name="changedFrom">Changed from date.</param>
        /// <param name="harvestJob">Contains meta data about harvest job.</param>
        /// <param name="context">Web service context.</param>
        /// <param name="harvestJobManager">Handles harvest job objects.</param>
        /// <param name="dataProviderIds">Contains current harvest job's data provider ids.</param>
        /// <param name="resetMaxChangeId">True will reset max change id for data provider.</param>
        private static void DoHarvestJob(DateTime changedFrom, HarvestJob harvestJob, WebServiceContext context, HarvestJobManager harvestJobManager, List<int> dataProviderIds, Boolean resetMaxChangeId)
        {
            List<WebSpeciesObservationDataProvider> dataProviders;
            IEnumerable<WebSpeciesObservationDataProvider> activeDataProviders;
            List<int> activeDataProviderIds = dataProviderIds;

            // Set meta data for current job
            harvestJob.HarvestCurrentDate = changedFrom;
            harvestJob.JobStatus = CurrentStatus;
            harvestJob.JobEndDate = null;
            harvestJobManager.SetHarvestJob(harvestJob);

            // Harvest only from selected providers that are active and have any data for current date
            activeDataProviders = GetActiveDataProviders(context);
            dataProviders = activeDataProviders.Where(dp => dp.BeginHarvestFromDate.Date.CompareTo(changedFrom.Date) <= 0 && dataProviderIds.Contains(dp.Id)).ToList();
            activeDataProviderIds.Clear();
            activeDataProviderIds.AddRange(dataProviders.Select(dp => dp.Id));

            if (activeDataProviderIds.Count > 0)
            {
                foreach (var activeDataProviderId in activeDataProviderIds)
                {
                    var dataProviderId = new List<int>() { activeDataProviderId };

                    // Log data provider harvest status
                    context.GetSpeciesObservationDatabase().SetHarvestJobStatistics(dataProviderId, harvestJob.JobStatus, changedFrom);

                    Boolean updateDone = UpdateSpeciesObservations(context, changedFrom, changedFrom, dataProviderId, resetMaxChangeId);

                    // Log success by date
                    if (updateDone)
                    {
                        // Log data provider harvest status
                        context.GetSpeciesObservationDatabase().SetHarvestJobStatistics(dataProviderId, HarvestStatusEnum.Done, changedFrom);
                    }
                }
            }
        }

        /// <summary>
        /// Set changed dates by providers default values if they are not specified.
        /// </summary>
        /// <param name="changedFrom">Changed from date.</param>
        /// <param name="changedTo">Changed to date.</param>
        /// <param name="oldestBeginHarvestFromDate">Oldest beginHarvestFromDate by active data providers.</param>
        private static void SetChangedDates(out DateTime changedFrom, out DateTime changedTo, DateTime oldestBeginHarvestFromDate)
        {
            // Set changedFrom date to the oldest BeginHarvestFromDate for active providers
            changedFrom = oldestBeginHarvestFromDate;

            // Set changedTo date to yesterday's date since auto harvest will cover later dates
            changedTo = DateTime.Now.Date.AddDays(-1);
        }

        /// <summary>
        /// Set changed dates by providers default values if they are not specified.
        /// </summary>
        /// <param name="changedFrom">Changed from date.</param>
        /// <param name="changedTo">Changed to date.</param>
        /// <param name="validatedChangedFrom">Validated changed from date.</param>
        /// <param name="validatedChangedTo">Validated changed to date.</param>
        /// <param name="oldestBeginHarvestFromDate">Oldest beginHarvestFromDate by active data providers.</param>
        private static void ValidateChangedDates(DateTime changedFrom, DateTime changedTo, out DateTime validatedChangedFrom, out DateTime validatedChangedTo, DateTime oldestBeginHarvestFromDate)
        {
            validatedChangedFrom = changedFrom;
            validatedChangedTo = changedTo;

            // Validate from date
            if (changedFrom.CompareTo(oldestBeginHarvestFromDate) < 0)
            {
                validatedChangedFrom = oldestBeginHarvestFromDate;
            }

            // Validate to date
            if (changedTo.CompareTo(DateTime.MinValue) <= 0 || changedTo.CompareTo(DateTime.Now.AddDays(-1)) > 0)
            {
                // Set changedTo date to yesterday's date since auto harvest will cover later dates
                validatedChangedTo = DateTime.Now.AddDays(-1);
            }
        }

        /// <summary>
        /// Get active data providers.
        /// </summary>
        /// <param name="context">Web service context.</param>
        /// <returns>List of data providers.</returns>
        private static IEnumerable<WebSpeciesObservationDataProvider> GetActiveDataProviders(WebServiceContext context)
        {
            List<WebSpeciesObservationDataProvider> allDataProviders = HarvestManager.GetSpeciesObservationDataProviders(context);

            // Get active providers that have been harvested at least one time before
            return allDataProviders.Where(dataProvider => dataProvider.IsActiveHarvest && dataProvider.LatestHarvestDate.CompareTo(DateTime.MinValue) > 0).ToList();
        }

        /// <summary>
        /// Update database with changes in species observation.
        /// Loop over each connector and read all changes 
        /// Add data to create, update or delete data tables
        /// Write Data tables to Temporary tables in database
        /// Update ID, TaxonId and convert Points to wgs85 if needed
        /// Call DB procedures to Copy/Update/Delete from temporary tables to production tables.  
        /// </summary>
        /// <param name="context">
        /// Web service request context.
        /// </param>
        /// <param name="changedFrom">
        /// Start date for changes.
        /// </param>
        /// <param name="changedTo">
        /// End date for changes.
        /// </param>
        /// <param name="dataProviderIds">
        /// Update species observations for these data providers.
        /// </param>
        /// <param name="restartHarvest">
        /// If true, harvest from beginning.
        /// </param>
        /// <returns>Update observation was successful=TRUE or failure=FALSE.</returns>
        public static Boolean UpdateSpeciesObservations(WebServiceContext context,
                                                        DateTime changedFrom,
                                                        DateTime changedTo,
                                                        List<Int32> dataProviderIds,
                                                        Boolean restartHarvest)
        {
            try
            {
                if (context.ClientToken.UserName != ApplicationIdentifier.SpeciesObservationHarvestService.ToString())
                {
                    WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.WebServiceAdministrator);
                }

                // This lock ensures that the update of species observations
                // and taxon information is not carried out simultaneously.
                lock (LockObject)
                {
                    if (dataProviderIds.IsEmpty())
                    {
                        // If no provider is in list - harvest for all providers that exist in database.
                        dataProviderIds = new DataId32List<int>();
                        foreach (int key in Connectors.Keys)
                        {
                            dataProviderIds.Add(key);
                        }
                    }

                    foreach (int dataProviderId in dataProviderIds)
                    {
                        if (Connectors.ContainsKey(dataProviderId))
                        {
                            IDataProviderConnector connector = Connectors[dataProviderId];
                            UpdateSpeciesObservations(context,
                                                      changedFrom,
                                                      changedTo,
                                                      connector,
                                                      restartHarvest);
                        }
                    }

                    return true;
                }
            }
            catch (Exception exception)
            {
                // We log the exception and return false as an indication something went wrong,
                // but we still want to continue with harvesting, no matter what.
                WebServiceData.LogManager.LogError(context, exception);
                return false;
            }
            finally
            {
                try
                {
                    // Clean up.
                    using (SpeciesObservationHarvestServer database = (SpeciesObservationHarvestServer)(WebServiceData.DatabaseManager.GetDatabase(context)))
                    {
                        database.EmptyTempTables();
                    }
                }

                // ReSharper disable once EmptyGeneralCatchClause
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Update database with changes in species observation.
        /// Loop over each connector and read all changes 
        /// Add data to create, update or delete data tables
        /// Write Data tables to Temporary tables in database
        /// Update ID, TaxonId and convert Points to wgs85 if needed
        /// Call DB procedures to Copy/Update/Delete from temporary tables to production tables.  
        /// </summary>
        /// <param name="context">
        /// Web service request context.
        /// </param>
        /// <param name="changedFrom">
        /// Start date for changes.
        /// </param>
        /// <param name="changedTo">
        /// End date for changes.
        /// </param>
        /// <param name="connector">
        /// Update species observations for this connector.
        /// </param>
        /// <param name="restartHarvest">
        /// If true, harvest from beginning.
        /// </param>
        public static void UpdateSpeciesObservations(WebServiceContext context,
                                                     DateTime changedFrom,
                                                     DateTime changedTo,
                                                     IDataProviderConnector connector,
                                                     Boolean restartHarvest)
        {
            Boolean areMoreSpeciesObservationsAvailable = true;

            // Get provider information
            WebSpeciesObservationDataProvider dataProvider = connector.GetSpeciesObservationDataProvider(context);
            if (dataProvider.IsNull() || !dataProvider.IsActiveHarvest)
            {
                return; // provider does not exist in database or is not active.
            }

            // Read FieldDescriptions from SpeciesObservationDatabase
            // Description contain information about all fields that should be written to the database
            // and how they are connected to the data from the provider
            // and witch data that is mandatory.
            List<WebSpeciesObservationFieldDescriptionExtended> webDarwinCoreFieldDescriptions = WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(context, true);
            context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest DB - OPERATIONS", 0, "webDarwinCoreFieldDescriptions.");

            // Ready with provider information
            context.GetSpeciesObservationDatabase().Log(context, dataProvider.Name, LogType.Information, "Start " + dataProvider.Name + ", Day:" + changedFrom.ToString("yyyyMMdd"), String.Empty);

            // Read the mapping for the specific provider
            List<HarvestMapping> mappings = CreateMappingList(webDarwinCoreFieldDescriptions, dataProvider.Id);

            // Harvest from beginning
            if (restartHarvest)
            {
                // Reset change id to start value for the data provider
                context.GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, null);
            }

            ConnectorServer connectorServer = new ConnectorServer();
            while (areMoreSpeciesObservationsAvailable)
            {
                // Make sure the temp tables in db are empty
                context.GetSpeciesObservationDatabase().EmptyTempTables();
                context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest DB - OPERATIONS", 0, "Empty Temp tables before reading.");

                areMoreSpeciesObservationsAvailable = connector.GetSpeciesObservationChange(changedFrom, changedTo, mappings, context, connectorServer);
                var stopwatch = Stopwatch.StartNew();

                // Log latest changed date (at data source), of modified information of harvested observations
                context.GetSpeciesObservationDatabase().SetDataProviderLatestChangedDate(dataProvider.Id);
                context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - Check", 0, " SetDataProviderLatestChangedDate: " + (stopwatch.ElapsedMilliseconds));

                Stopwatch stopwatchM = Stopwatch.StartNew();
                long stopwatchPrev = 0;

                context.GetSpeciesObservationDatabase().UpdateTempObservationId();
                context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - Check", stopwatchM.ElapsedMilliseconds, " UpdateTempObservationId: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                context.GetSpeciesObservationDatabase().CheckTaxonIdOnTemp();
                context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - Check", stopwatchM.ElapsedMilliseconds, " CheckTaxonIdOnTemp: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                context.GetSpeciesObservationDatabase().CreatePointGoogleMercatorInTemp();
                context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - Calculate", stopwatchM.ElapsedMilliseconds, " CreatePointGoogleMercatorInTemp: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                context.GetSpeciesObservationDatabase().UpdateAccuracyAndDisturbanceRadius();
                context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - Calculate", stopwatchM.ElapsedMilliseconds, " UpdateAccuracyAndDisturbanceRadius: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                context.GetSpeciesObservationDatabase().MergeTempUpdateToPosition();
                context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - POSITION", stopwatchM.ElapsedMilliseconds, " MergeTempUpdateToPosition: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                context.GetSpeciesObservationDatabase().UpdateSpeciesObservationChange();
                context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - CREATE & UPDATE", stopwatchM.ElapsedMilliseconds, " UpdateSpeciesObservationChange: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                context.GetSpeciesObservationDatabase().MergeTempUpdateToDarwinCoreObservation();
                context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - CREATE & UPDATE", stopwatchM.ElapsedMilliseconds, " MergeTempUpdateToDarwinCoreObservation: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                context.GetSpeciesObservationDatabase().MergeTempUpdateToSpeciesObservationField();
                context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - CREATE & UPDATE", stopwatchM.ElapsedMilliseconds, " MergeTempUpdateToSpeciesObservationField: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                context.GetSpeciesObservationDatabase().CopyDeleteToSpeciesObservation();
                context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - DELETE", stopwatchM.ElapsedMilliseconds, " CopyDeleteToSpeciesObservation: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                context.GetSpeciesObservationDatabase().UpdateStatistics();
                context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest DB - OPERATIONS", stopwatchM.ElapsedMilliseconds, " UpdateStatistics:" + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                context.GetSpeciesObservationDatabase().EmptyTempTables();
                context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest DB - OPERATIONS", stopwatchM.ElapsedMilliseconds, " EmptyTempTables: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                context.GetSpeciesObservationDatabase().CleanLogUpdateError();
                context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest DB - OPERATIONS", stopwatchM.ElapsedMilliseconds, " CleanLogUpdateError: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                context.GetSpeciesObservationDatabase().CleanLogUpdateErrorDuplicates();
                context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest DB - OPERATIONS", stopwatchM.ElapsedMilliseconds, " CleanLogUpdateErrorDuplicates: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                if ((Configuration.InstallationType == InstallationType.Production) &&
                    SpeciesObservationConfiguration.IsElasticsearchUsed)
                {
                    UpdateSpeciesObservationsElasticsearchCurrentIndex(context);
                    context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest DB - OPERATIONS", stopwatchM.ElapsedMilliseconds, " UpdateSpeciesObservationsElasticsearchCurrentIndex: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                }

                stopwatchM.Stop();
            }
        }

        /// <summary>
        /// Update current index in Elasticsearch with changes in species observations.
        /// This method is automatically called during harvest from 
        /// species observation data sources.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        private static void UpdateSpeciesObservationsElasticsearchCurrentIndex(WebServiceContext context)
        {
            ElasticsearchSpeciesObservationProxy elasticsearch;
            Int64 maxChangeId;
            SpeciesObservationElasticsearch speciesObservationElasticsearch;

            speciesObservationElasticsearch = WebSpeciesObservationServiceData.SpeciesObservationManager.GetSpeciesObservationElasticsearch(context);
            elasticsearch = new ElasticsearchSpeciesObservationProxy(speciesObservationElasticsearch.CurrentIndexName);
            maxChangeId = Int64.MaxValue;
            while (speciesObservationElasticsearch.CurrentIndexChangeId < maxChangeId)
            {
                maxChangeId = UpdateSpeciesObservationsElasticsearch(context,
                                                                     speciesObservationElasticsearch.CurrentIndexChangeId,
                                                                     elasticsearch,
                                                                     null);
                speciesObservationElasticsearch.CurrentIndexCount = elasticsearch.GetSpeciesObservationCount(null).DocumentCount;
                speciesObservationElasticsearch.CurrentIndexChangeId = Math.Min(maxChangeId,
                                                                                speciesObservationElasticsearch.CurrentIndexChangeId + Settings.Default.ElasticsearchSpeciesObservationUpdateSize);
                WebSpeciesObservationServiceData.SpeciesObservationManager.UpdateSpeciesObservationElasticsearch(context,
                                                                                                                 speciesObservationElasticsearch.CurrentIndexChangeId,
                                                                                                                 speciesObservationElasticsearch.CurrentIndexCount,
                                                                                                                 null,
                                                                                                                 null,
                                                                                                                 null,
                                                                                                                 null);
            }
        }

        /// <summary>
        /// Update new index in Elasticsearch with changes in species observations.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>True if more changes are available.</returns>
        public static Boolean UpdateSpeciesObservationsElasticsearchNewIndex(WebServiceContext context)
        {
            Boolean areMoreChangesAvailable;
            ElasticsearchSpeciesObservationProxy elasticsearch;
            Int64 maxChangeId, nextIndexCountBeforeUpdate, nextIndexChangeId;
            SpeciesObservationElasticsearch speciesObservationElasticsearch;

            areMoreChangesAvailable = false;
            speciesObservationElasticsearch = WebSpeciesObservationServiceData.SpeciesObservationManager.GetSpeciesObservationElasticsearch(context);
            if (speciesObservationElasticsearch.NextIndexChangeId.HasValue &&
                speciesObservationElasticsearch.NextIndexName.IsNotEmpty())
            {
                elasticsearch = new ElasticsearchSpeciesObservationProxy(speciesObservationElasticsearch.NextIndexName);
                if (speciesObservationElasticsearch.NextIndexCount.HasValue)
                {
                    nextIndexCountBeforeUpdate = speciesObservationElasticsearch.NextIndexCount.Value;
                }
                else
                {
                    nextIndexCountBeforeUpdate = 0;
                }

                maxChangeId = UpdateSpeciesObservationsElasticsearch(context,
                                                                     speciesObservationElasticsearch.NextIndexChangeId.Value,
                                                                     elasticsearch,
                                                                     speciesObservationElasticsearch.NextIndexHarvestStart);
                areMoreChangesAvailable = (speciesObservationElasticsearch.NextIndexChangeId + Settings.Default.ElasticsearchSpeciesObservationUpdateSize) < maxChangeId;
                speciesObservationElasticsearch.NextIndexCount = elasticsearch.GetSpeciesObservationCount(null).DocumentCount;
                if (nextIndexCountBeforeUpdate == speciesObservationElasticsearch.NextIndexCount.Value)
                {
                    nextIndexChangeId = context.GetSpeciesObservationDatabase().GetNextChangeId(speciesObservationElasticsearch.NextIndexChangeId.Value,
                                                                                                speciesObservationElasticsearch.NextIndexHarvestStart.Value);
                    speciesObservationElasticsearch.NextIndexChangeId = Math.Max(nextIndexChangeId,
                                                                                 speciesObservationElasticsearch.NextIndexChangeId.Value + Settings.Default.ElasticsearchSpeciesObservationUpdateSize);
                }
                else
                {
                    speciesObservationElasticsearch.NextIndexChangeId = Math.Min(maxChangeId,
                                                                                 speciesObservationElasticsearch.NextIndexChangeId.Value + Settings.Default.ElasticsearchSpeciesObservationUpdateSize);
                }

                WebSpeciesObservationServiceData.SpeciesObservationManager.UpdateSpeciesObservationElasticsearch(context,
                                                                                                                 null,
                                                                                                                 null,
                                                                                                                 null,
                                                                                                                 speciesObservationElasticsearch.NextIndexChangeId,
                                                                                                                 speciesObservationElasticsearch.NextIndexCount,
                                                                                                                 null);
            }

            return areMoreChangesAvailable;
        }

        /// <summary>
        /// Update Elasticsearch with changes in species observations.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="changeId">Get changes from this change id.</param>
        /// <param name="elasticsearch">Proxy to Elasticsearch.</param>
        /// <param name="nextIndexHarvestStart">
        /// Start date of harvest.
        /// This value is used to avoid handling old delete of species observations.
        /// </param>
        /// <returns>Max species observation change id.</returns>
        private static long UpdateSpeciesObservationsElasticsearch(WebServiceContext context,
                                                                   long changeId,
                                                                   ElasticsearchSpeciesObservationProxy elasticsearch,
                                                                   DateTime? nextIndexHarvestStart)
        {
            Dictionary<Int32, WebSpeciesObservationDataProvider> dataProviders;
            Dictionary<Int32, TaxonInformation> taxonInformations;
            Int64 maxSpeciesObservationChangeId, speicesObservationId;
            List<String> deletedSpeciesObservations;
            List<WebSpeciesObservation> speciesObservations;
            List<WebSpeciesObservationField> speciesObservationFields;
            Dictionary<Int64, List<WebSpeciesObservationField>> speciesObservationProjectParameters;
            TaxonInformation taxonInformation;
            WebSpeciesObservation speciesObservation;
            WebSpeciesObservationFieldProjectParameter speciesObservationField;

            // Set english locale.
            context.Locale = context.GetDefaultLocale();
            taxonInformations = WebSpeciesObservationServiceData.TaxonManager.GetTaxonInformation(context);
            dataProviders = new Dictionary<Int32, WebSpeciesObservationDataProvider>();
            foreach (WebSpeciesObservationDataProvider dataProvider in WebServiceData.SpeciesObservationManager.GetSpeciesObservationDataProviders(context))
            {
                dataProviders[dataProvider.Id] = dataProvider;
            }

            speciesObservationProjectParameters = new Dictionary<Int64, List<WebSpeciesObservationField>>();
            using (DataReader dataReader = context.GetSpeciesObservationDatabase().GetSpeciesObservationChangesElasticsearch(changeId, Settings.Default.ElasticsearchSpeciesObservationUpdateSize, nextIndexHarvestStart))
            {
                // Get created or updated species observations.
                speciesObservations = new List<WebSpeciesObservation>();
                while (dataReader.Read())
                {
                    taxonInformation = taxonInformations[dataReader.GetInt32((Int32)(DarwinCoreColumn.DyntaxaTaxonId))];

                    speciesObservation = new WebSpeciesObservation();
                    speciesObservation.Fields = new List<WebSpeciesObservationField>();

                    LoadConservation(dataReader, speciesObservation, taxonInformation);
                    LoadDarwinCore(dataReader, speciesObservation, dataProviders);
                    LoadEvent(dataReader, speciesObservation);
                    LoadIdentification(dataReader, speciesObservation);
                    LoadLocation(dataReader, speciesObservation);
                    LoadOccurrence(dataReader, speciesObservation);
                    LoadProject(dataReader, speciesObservation);
                    LoadTaxon(dataReader, speciesObservation, taxonInformation);

                    speciesObservations.Add(speciesObservation);
                }

                // Get species observation project parameters.
                dataReader.NextResultSet();
                while (dataReader.Read())
                {
                    speciesObservationField = new WebSpeciesObservationFieldProjectParameter();
                    speciesObservationField.LoadData(dataReader);
                    if (speciesObservationProjectParameters.ContainsKey(speciesObservationField.SpeciesObservationId))
                    {
                        speciesObservationFields = speciesObservationProjectParameters[speciesObservationField.SpeciesObservationId];
                    }
                    else
                    {
                        speciesObservationFields = new List<WebSpeciesObservationField>();
                        speciesObservationProjectParameters[speciesObservationField.SpeciesObservationId] = speciesObservationFields;
                    }

                    speciesObservationFields.Add(speciesObservationField.GetSpeciesObservationField());
                }

                // Get deleted species observations.
                dataReader.NextResultSet();
                deletedSpeciesObservations = new List<String>();
                while (dataReader.Read())
                {
                    deletedSpeciesObservations.Add(dataReader.GetString(SpeciesObservationElasticsearchData.OCCURRANCE_ID));
                }

                // Get max species observation change id.
                dataReader.NextResultSet();
                if (dataReader.Read())
                {
                    maxSpeciesObservationChangeId = dataReader.GetInt64(SpeciesObservationChangeData.MAX_SPECIES_OBSERVATION_CHANGE_ID);
                }
                else
                {
                    throw new ApplicationException("Missing value: Max species observation change id.");
                }
            }

            // Add species observation project parameters into species observations.
            if (speciesObservations.IsNotEmpty() &&
                speciesObservationProjectParameters.IsNotEmpty())
            {
                foreach (WebSpeciesObservation tempSpeciesObservation in speciesObservations)
                {
                    speicesObservationId = tempSpeciesObservation.Fields.GetField(SpeciesObservationClassId.DarwinCore, SpeciesObservationPropertyId.Id).Value.WebParseInt64();
                    if (speciesObservationProjectParameters.ContainsKey(speicesObservationId))
                    {
                        tempSpeciesObservation.Fields.AddRange(speciesObservationProjectParameters[speicesObservationId]);
                    }
                }
            }

            // Update species observations in Elasticsearch.
            UpdateSpeciesObservationsElasticsearch(context, speciesObservations, elasticsearch);

            // Delete species observations from Elasticsearch.
            DeleteSpeciesObservationsElasticsearch(deletedSpeciesObservations, elasticsearch);

            return maxSpeciesObservationChangeId;
        }

        /// <summary>
        /// Update species observation information in elasticsearch.
        /// </summary>
        /// <param name="context">Web service context.</param>
        /// <param name="speciesObservations">Add these species observations to Elasticsearch.</param>
        /// <param name="elasticsearch">Proxy to Elasticsearch.</param>
        private static void UpdateSpeciesObservationsElasticsearch(WebServiceContext context,
                                                                   List<WebSpeciesObservation> speciesObservations,
                                                                   ElasticsearchSpeciesObservationProxy elasticsearch)
        {
            Int32 index;
            String occurrenceId;
            StringBuilder json;

            if (speciesObservations.IsNotEmpty())
            {
                json = new StringBuilder();
                for (index = 0; index < speciesObservations.Count; index++)
                {
                    WebSpeciesObservationServiceData.SpeciesObservationManager.CheckMappingElasticsearch(context, speciesObservations[index], elasticsearch);
                    occurrenceId = speciesObservations[index].Fields.GetField(SpeciesObservationClassId.Occurrence.ToString(),
                                                                              SpeciesObservationPropertyId.OccurrenceID.ToString()).Value;

                    json.Append("{\"index\":{\"_index\":\"" + elasticsearch.IndexName + "\",\"_type\":\"" + elasticsearch.SpeciesObservationType + "\",\"_id\":\"" + occurrenceId + "\"}}" + Environment.NewLine);
                    json.Append(speciesObservations[index].GetJson(context) + Environment.NewLine);

                    if ((index % 1000) == 999)
                    {
                        CheckHealth(context, elasticsearch);
                        elasticsearch.UpdateSpeciesObservations(json.ToString());
                        json = new StringBuilder();
                        if ((index + 1) < speciesObservations.Count)
                        {
                            Thread.Sleep(10000);
                        }
                    }
                }

                if (json.ToString().IsNotEmpty())
                {
                    CheckHealth(context, elasticsearch);
                    elasticsearch.UpdateSpeciesObservations(json.ToString());
                }
            }
        }

        /// <summary>
        /// Update species observation information in elasticsearch.
        /// </summary>
        /// <param name="context">Web service context.</param>
        /// <param name="speciesObservationAdd">Ids for species observations that should be added to Elasticsearch.</param>
        /// <param name="speciesObservationDelete">Ids for species observations that should be deleted to Elasticsearch.</param>
        /// <param name="elasticsearch">Proxy to Elasticsearch.</param>
        private static void UpdateSpeciesObservationsElasticsearch(WebServiceContext context,
                                                                   List<Int64> speciesObservationAdd,
                                                                   List<Int64> speciesObservationDelete,
                                                                   ElasticsearchSpeciesObservationProxy elasticsearch)
        {
            DateTime lastPause;
            Dictionary<Int32, TaxonInformation> taxonInformations;
            Dictionary<Int32, WebSpeciesObservationDataProvider> dataProviders;
            Dictionary<String, WebSpeciesObservationField> mapping;
            DocumentFilterResponse documentFilterResponse;
            Int32 index;
            List<Int64> tempSpeciesObservationAdd, tempSpeciesObservationDelete;
            List<String> speciesObservationDeleteGuids;
            List<WebSpeciesObservation> speciesObservations;
            StringBuilder filter;
            TaxonInformation taxonInformation;
            WebSpeciesObservation speciesObservation;

            lastPause = DateTime.Now;

            // Set english locale.
            context.Locale = context.GetDefaultLocale();

            // Add species observations to Elasticsearch.
            if (speciesObservationAdd.IsNotEmpty())
            {
                taxonInformations = WebSpeciesObservationServiceData.TaxonManager.GetTaxonInformation(context);
                dataProviders = new Dictionary<Int32, WebSpeciesObservationDataProvider>();
                foreach (WebSpeciesObservationDataProvider dataProvider in WebServiceData.SpeciesObservationManager.GetSpeciesObservationDataProviders(context))
                {
                    dataProviders[dataProvider.Id] = dataProvider;
                }

                while (speciesObservationAdd.IsNotEmpty())
                {
                    tempSpeciesObservationAdd = new List<Int64>();
                    for (index = 0; index < 1000; index++)
                    {
                        if (speciesObservationAdd.IsNotEmpty())
                        {
                            tempSpeciesObservationAdd.Add(speciesObservationAdd[0]);
                            speciesObservationAdd.RemoveAt(0);
                        }
                    }

                    using (DataReader dataReader = context.GetSpeciesObservationDatabase().GetSpeciesObservationsByIdElasticsearch(tempSpeciesObservationAdd))
                    {
                        // Get species observations.
                        speciesObservations = new List<WebSpeciesObservation>();
                        while (dataReader.Read())
                        {
                            taxonInformation = taxonInformations[dataReader.GetInt32((Int32)(DarwinCoreColumn.DyntaxaTaxonId))];

                            speciesObservation = new WebSpeciesObservation();
                            speciesObservation.Fields = new List<WebSpeciesObservationField>();

                            LoadConservation(dataReader, speciesObservation, taxonInformation);
                            LoadDarwinCore(dataReader, speciesObservation, dataProviders);
                            LoadEvent(dataReader, speciesObservation);
                            LoadIdentification(dataReader, speciesObservation);
                            LoadLocation(dataReader, speciesObservation);
                            LoadOccurrence(dataReader, speciesObservation);
                            LoadProject(dataReader, speciesObservation);
                            LoadTaxon(dataReader, speciesObservation, taxonInformation);

                            speciesObservations.Add(speciesObservation);
                        }
                    }

                    // Update species observations in Elasticsearch.
                    UpdateSpeciesObservationsElasticsearch(context, speciesObservations, elasticsearch);

                    WebSpeciesObservationServiceData.SpeciesObservationManager.UpdateSpeciesObservationElasticsearch(context,
                                                                                                                     null,
                                                                                                                     elasticsearch.GetSpeciesObservationCount(null).DocumentCount,
                                                                                                                     null,
                                                                                                                     null,
                                                                                                                     null,
                                                                                                                     null);

                    while ((DateTime.Now.Hour == 4) || (DateTime.Now.Hour == 5))
                    {
                        // Do not manually update index at the same
                        // time as automatic update is done to index.
                        Thread.Sleep(600000);
                    }

                    Thread.Sleep(5000);

                    if ((DateTime.Now - lastPause).TotalMinutes > 60)
                    {
                        // Make a 5 minute break each hour.
                        // This will allow Elasticsearch to save transactions to disk.
                        Thread.Sleep(300000);
                        lastPause = DateTime.Now;
                    }
                }
            }

            // Delete species observations from Elasticsearch.
            if (speciesObservationDelete.IsNotEmpty())
            {
                mapping = WebSpeciesObservationServiceData.SpeciesObservationManager.GetMapping(context, elasticsearch);

                while (speciesObservationDelete.IsNotEmpty())
                {
                    tempSpeciesObservationDelete = new List<Int64>();
                    for (index = 0; index < 1000; index++)
                    {
                        if (speciesObservationDelete.IsNotEmpty())
                        {
                            tempSpeciesObservationDelete.Add(speciesObservationDelete[0]);
                            speciesObservationDelete.RemoveAt(0);
                        }
                    }

                    filter = new StringBuilder();
                    filter.Append("{");
                    filter.Append(" \"size\": " + tempSpeciesObservationDelete.Count);
                    filter.Append(", \"_source\" : {\"include\": [\"Occurrence_OccurrenceID\", \"DarwinCore_Id\"]}");
                    filter.Append(", \"filter\": { \"terms\": { \"DarwinCore_Id\":[");
                    filter.Append(tempSpeciesObservationDelete[0].WebToString());
                    for (index = 1; index < tempSpeciesObservationDelete.Count; index++)
                    {
                        filter.Append(", " + tempSpeciesObservationDelete[index].WebToString());
                    }

                    filter.Append("]}}}");
                    documentFilterResponse = elasticsearch.GetSpeciesObservations(filter.ToString());
                    if (documentFilterResponse.TimedOut)
                    {
                        throw new Exception("Method UpdateSpeciesObservationsElasticsearch() timed out!");
                    }

                    speciesObservations = WebSpeciesObservationServiceData.SpeciesObservationManager.GetSpeciesObservations(documentFilterResponse.DocumentsJson,
                                                                                                                            mapping);
                    if (speciesObservations.IsNotEmpty())
                    {
                        speciesObservationDeleteGuids = new List<String>();
                        foreach (WebSpeciesObservation speciesObservationDeleteTemp in speciesObservations)
                        {
                            speciesObservationDeleteGuids.Add(speciesObservationDeleteTemp.Fields[0].Value);
                        }

                        DeleteSpeciesObservationsElasticsearch(speciesObservationDeleteGuids, elasticsearch);
                    }

                    WebSpeciesObservationServiceData.SpeciesObservationManager.UpdateSpeciesObservationElasticsearch(context,
                                                                                                                     null,
                                                                                                                     elasticsearch.GetSpeciesObservationCount(null).DocumentCount,
                                                                                                                     null,
                                                                                                                     null,
                                                                                                                     null,
                                                                                                                     null);

                    if ((DateTime.Now - lastPause).TotalMinutes > 60)
                    {
                        // Make a 15 minute break each hour.
                        // This will alove Elasticsearch to save transactions to disk.
                        Thread.Sleep(900000);
                        lastPause = DateTime.Now;
                    }
                }
            }
        }

        /// <summary>
        /// Change active index in Elasticsearch from old index to next index.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void UpdateSpeciesObservationsElasticsearchToNewIndex(WebServiceContext context)
        {
            ElasticsearchSpeciesObservationProxy elasticsearch;
            SpeciesObservationElasticsearch speciesObservationElasticsearch;

            speciesObservationElasticsearch = WebSpeciesObservationServiceData.SpeciesObservationManager.GetSpeciesObservationElasticsearch(context);
            elasticsearch = new ElasticsearchSpeciesObservationProxy(speciesObservationElasticsearch.NextIndexName);
            elasticsearch.UpdateIndexAlias(SpeciesObservation.Settings.Default.IndexNameElasticsearch,
                                           speciesObservationElasticsearch.CurrentIndexName,
                                           speciesObservationElasticsearch.NextIndexName);
            if (speciesObservationElasticsearch.CurrentIndexName.IsEmpty())
            {
                WebSpeciesObservationServiceData.SpeciesObservationManager.UpdateSpeciesObservationElasticsearch(context,
                                                                                                                 speciesObservationElasticsearch.NextIndexChangeId.Value,
                                                                                                                 speciesObservationElasticsearch.NextIndexCount.Value,
                                                                                                                 speciesObservationElasticsearch.NextIndexName,
                                                                                                                 speciesObservationElasticsearch.CurrentIndexChangeId,
                                                                                                                 speciesObservationElasticsearch.CurrentIndexCount,
                                                                                                                 speciesObservationElasticsearch.CurrentIndexName);
            }
            else
            {
                WebSpeciesObservationServiceData.SpeciesObservationManager.UpdateSpeciesObservationElasticsearch(context,
                                                                                                                 speciesObservationElasticsearch.NextIndexChangeId.Value,
                                                                                                                 speciesObservationElasticsearch.NextIndexCount.Value,
                                                                                                                 speciesObservationElasticsearch.NextIndexName,
                                                                                                                 speciesObservationElasticsearch.CurrentIndexChangeId,
                                                                                                                 speciesObservationElasticsearch.CurrentIndexCount,
                                                                                                                 speciesObservationElasticsearch.CurrentIndexName);
            }
        }

        /// <summary>
        /// Read the mapping for the dataProvider and create a mapping list.
        /// </summary>
        /// <param name="descriptions">
        /// The descriptions list.
        /// </param>
        /// <param name="dataProviderId">
        /// The data provider id.
        /// </param>
        /// <returns>
        /// The Harvest mapping list.<see cref="List{HarvestMapping}"/>.
        /// </returns>
        public static List<HarvestMapping> CreateMappingList(List<WebSpeciesObservationFieldDescriptionExtended> descriptions, Int32 dataProviderId)
        {
            List<HarvestMapping> returnList = null;
            if (descriptions.IsNotNull())
            {
                returnList = new List<HarvestMapping>();
                foreach (var description in descriptions)
                {
                    if (description.Mappings.Count == 0)
                    {
                        continue;
                    }

                    foreach (WebSpeciesObservationFieldMapping mapping in description.Mappings)
                    {
                        if (!mapping.DataProviderId.Equals(dataProviderId))
                        {
                            continue;
                        }

                        if (mapping.IsImplemented == false)
                        {
                            continue;
                        }

                        HarvestMapping harvestMapping = new HarvestMapping();
                        harvestMapping.Property = description.Name;
                        harvestMapping.Class = description.Class.GetClass();
                        harvestMapping.Type = description.Type.ToString();
                        harvestMapping.Name = mapping.ProviderFieldName;
                        harvestMapping.Method = mapping.Method;
                        harvestMapping.Default = mapping.DefaultValue;
                        harvestMapping.IsSearchable = description.IsSearchable;
                        harvestMapping.IsDarwinCore = description.IsDarwinCoreProperty;
                        harvestMapping.Mandatory = description.IsMandatory;
                        harvestMapping.IsMandatoryFromProvider = description.IsMandatoryFromProvider;
                        harvestMapping.IsObtainedFromProvider = description.IsObtainedFromProvider;
                        harvestMapping.PersistedInTable = description.PersistedInTable;
                        harvestMapping.GUID = mapping.GetGUID();
                        harvestMapping.PropertyIdentifier = mapping.GetPropertyIdentifier();
                        returnList.Add(harvestMapping);
                        break;
                    }
                }
            }

            return returnList;
        }

        /// <summary>
        /// Gets all information on project parameters from the Artportalen database and adds or updates the information in the Observations database
        /// </summary>
        /// <param name="artportalenServer">An ArtportalenServer instance</param>
        /// <param name="context">A WebServiceContext instance</param>
        public static void UpdateProjectParameterInformation(ArtportalenServer artportalenServer, WebServiceContext context)
        {
            var speciesObservationFieldDescriptionTable = GetSpeciesObservationFieldDescriptionTable();
            var speciesObservationFieldMappingTable = GetSpeciesObservationFieldMappingTable();

            WebServiceData.LogManager.Log(context, "Begin project parameter metadata update", LogType.Information, null);

            // Read the mappings for the Artportalen provider
            var mappings = CreateMappingList(WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(context, true), (int)SpeciesObservationDataProviderId.SpeciesGateway);

            using (var reader = artportalenServer.GetProjectParametersInformation())
            {
                while (reader.Read())
                {
                    var descriptionRow = speciesObservationFieldDescriptionTable.NewRow();
                    descriptionRow[SpeciesObservationFieldDescriptionData.ID] = 0;
                    var mappingRow = speciesObservationFieldMappingTable.NewRow();
                    mappingRow[SpeciesObservationFieldMappingData.ID] = 0;
                    FillTableRow(descriptionRow, reader);
                    FillTableRow(mappingRow, reader);

                    var mapping = mappings.Find(item => item.GUID != null && item.GUID.Equals(descriptionRow[SpeciesObservationFieldDescriptionData.GUID]));
                    if (mapping == null)
                    {
                        speciesObservationFieldDescriptionTable.Rows.Add(descriptionRow);
                        speciesObservationFieldMappingTable.Rows.Add(mappingRow);
                        //WebServiceData.LogManager.Log(context, string.Format("Adding project parameter [{0}]", descriptionRow[SpeciesObservationFieldDescriptionData.GUID]), LogType.Information, null);
                    }
                    else
                    {
                        // Check if the mapping needs updating
                        // This comparison is made using not all, but only the properties that can change from Artportalen
                        // TODO: NOTE: [Definition] i.e. ProjectParameter.Description is not a part of the comparison, right now
                        //if (mapping.HasDifferences(descriptionRow, mappingRow))
                        {
                            speciesObservationFieldDescriptionTable.Rows.Add(descriptionRow);
                            speciesObservationFieldMappingTable.Rows.Add(mappingRow);
                            //WebServiceData.LogManager.Log(context, string.Format("Updating project parameter [{0}]", mapping.GUID), LogType.Information, null);
                        }
                    }
                }
            }

            if (speciesObservationFieldDescriptionTable.Rows.Count > 0)
            {
                WebServiceData.MetadataManager.UpdateSpeciesObservationFieldDescription(context, speciesObservationFieldDescriptionTable);
                WebServiceData.MetadataManager.UpdateSpeciesObservationFieldMapping(context, speciesObservationFieldMappingTable);
            }

            WebServiceData.LogManager.Log(context, "End project parameter metadata update", LogType.Information, null);
        }

        /// <summary>
        /// Handle automatic update of taxon information.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void UpdateTaxonInformation(WebServiceContext context)
        {
            // This lock ensures that the update of species observations
            // and taxon information is not carried out simultaneously.
            lock (LockObject)
            {
                ResetTaxaCache();

                WebServiceData.LogManager.Log(context, "Begin taxon information update", LogType.Information, null);

                // Empty TempTaxon Tables
                context.GetSpeciesObservationDatabase().EmptyTempTaxonTables(context);

                WebTaxonSearchCriteria searchCriteria = new WebTaxonSearchCriteria();

                List<WebTaxon> taxon = WebServiceData.TaxonManager.GetTaxaBySearchCriteria(context, searchCriteria);

                // Get taxon and taxon tree information.
                WebTaxonTreeSearchCriteria taxonTreeSearchCriteria = new WebTaxonTreeSearchCriteria();
                taxonTreeSearchCriteria.IsValidRequired = true;  // Cannot be false due to nested loop in TaxonService.
                List<WebTaxonTreeNode> taxonTrees = WebServiceData.TaxonManager.GetTaxonTreesBySearchCriteria(context, taxonTreeSearchCriteria);

                Dictionary<Int32, String> taxonRemarks = TaxonManager.GetTaxonRemarks(context);

                // Get taxoncategories
                GetTaxonCategories(context);
                Debug.WriteLine("Ready with GetTaxonCategories");

                // Get taxa original name
                GetTaxaOriginalNames(context);
                Debug.WriteLine("Ready with GetTaxaOriginalNames");

                // Update table TaxonTree.
                UpdateTempTaxonTreeInformation(context, taxonTrees);
                Debug.WriteLine("Ready with UpdateTempTaxonTreeInformation");

                // Update table Taxon. (Write to db)
                UpdateTempTaxonInformation(context, taxon, taxonTrees, taxonRemarks);
                Debug.WriteLine("Ready with UpdateTempTaxonInformation");

                // Update taxon table.
                context.GetSpeciesObservationDatabase().ProcessTempTaxon(context);
                Debug.WriteLine("Ready with ProcessTempTaxon");

                // Update TempTaxon with remarks from the Taxon table
                context.GetSpeciesObservationDatabase().UpdateRemarkInTempTaxonFromTaxon(context);
                Debug.WriteLine("Ready with UpdateRemarkInTempTaxonFromTaxon");

                //// Update the SpeciesObservationChange table for the ElasticSearch database 
                WebServiceData.LogManager.Log(context, "Begin SpeciesObservationChange information update", LogType.Information, null);
                var retval = context.GetSpeciesObservationDatabase().AddSpeciesObservationChangeForElasticSearch(100000, false);
                WebServiceData.LogManager.Log(context, string.Format("End SpeciesObservationChange information update, {0} affected observations", retval), LogType.Information, null);
                Debug.WriteLine("Ready with UpdateElasticSearch");

                // Update Taxon table with data from TempTaxon
                context.GetSpeciesObservationDatabase().CopyTempToTaxon(context);
                Debug.WriteLine("Ready with CopyTempTaxon");

                // Update taxonTree table.
                context.GetSpeciesObservationDatabase().CopyTempToTaxonTree(context);
                Debug.WriteLine("Ready with CopyTempToTaxonTree");

                // Uppdatera befintliga observationer med avseende på uppdaterade Taxon
                context.GetSpeciesObservationDatabase().UpdateDarwincoreObservationOnTaxonUpdate(context);

                WebServiceData.LogManager.Log(context, "End taxon information update", LogType.Information, null);
            }
        }

        /// <summary>
        /// Get all unique child taxa.
        /// This method operates on current taxon tree in contrast to
        /// the full taxon tree with all taxon tree nodes.
        /// </summary>
        /// <param name="taxonTreeNode">
        /// The taxon Tree Node.
        /// </param>
        /// <returns>
        /// All child taxa.
        /// </returns>
        public static List<WebTaxon> GetChildTaxa(WebTaxonTreeNode taxonTreeNode)
        {
            // TODO Optimze (true) HashTable
            // childTaxa = new List<WebTaxon>(true);
            List<WebTaxon> childTaxa = new List<WebTaxon>();

            if (taxonTreeNode.Children.IsNotEmpty())
            {
                // Add taxa for child taxon tree node.
                foreach (WebTaxonTreeNode childTaxonTreeNode in taxonTreeNode.Children)
                {
                    GetChildTaxa(childTaxonTreeNode, childTaxa);
                }
            }

            // childTaxa.Sort();
            return childTaxa;
        }

        /// <summary>
        /// Get all species observation data providers.
        /// No cache is used.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All species observation data providers.</returns>
        public static List<WebSpeciesObservationDataProvider> GetSpeciesObservationDataProviders(WebServiceContext context)
        {
            return new SpeciesObservationManager().GetSpeciesObservationDataProviders(context);
        }

        /// <summary>
        /// Stop currently running harvest job.
        /// </summary>
        /// <param name="context">Web service context.</param>
        public static void StopSpeciesObservationUpdate(WebServiceContext context)
        {
            RequestedStatus = HarvestStatusEnum.Stopped;

            if (context.ClientToken.UserName != ApplicationIdentifier.SpeciesObservationHarvestService.ToString())
            {
                WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.WebServiceAdministrator);
            }

            // Read meta data for current job
            var harvestJobManager = new HarvestJobManager(context);
            var harvestJob = harvestJobManager.GetHarvestJob();
            CurrentStatus = harvestJob.JobStatus;

            // Stop a paused harvest job (not currently running thread for a paused job)
            if (HarvestStatusEnum.Paused.Equals(CurrentStatus))
            {
                // Set meta data for current job
                harvestJob.JobStatus = RequestedStatus;
                harvestJobManager.SetHarvestJob(harvestJob);

                var dataProviderIds = harvestJob.DataProviders.Select(dp => dp.DataProviderId).ToList();
                var changedFrom = harvestJob.HarvestCurrentDate;

                // Log data provider harvest status
                context.GetSpeciesObservationDatabase().SetHarvestJobStatistics(dataProviderIds, RequestedStatus, changedFrom);
            }
        }

        /// <summary>
        /// Update species observation information in elasticsearch.
        /// </summary>
        /// <param name="dataReader">An open data reader with species observation information.</param>
        /// <param name="speciesObservation">Add information to this species observation instance.</param>
        /// <param name="taxonInformation">Taxon information.</param>
        private static void LoadConservation(DataReader dataReader,
                                             WebSpeciesObservation speciesObservation,
                                             TaxonInformation taxonInformation)
        {
            WebSpeciesObservationField field;

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Conservation.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.ActionPlan.ToString();
            field.Type = WebDataType.Boolean;
            field.Value = taxonInformation.ActionPlan.WebToString();
            speciesObservation.Fields.Add(field);

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Conservation.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.Natura2000.ToString();
            field.Type = WebDataType.Boolean;
            field.Value = taxonInformation.Natura2000.WebToString();
            speciesObservation.Fields.Add(field);

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Conservation.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.ProtectedByLaw.ToString();
            field.Type = WebDataType.Boolean;
            field.Value = taxonInformation.ProtectedByLaw.WebToString();
            speciesObservation.Fields.Add(field);

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Conservation.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.ProtectionLevel.ToString();
            field.Type = WebDataType.Int32;
            field.Value = dataReader.GetInt32((Int32)(DarwinCoreColumn.ProtectionLevel)).WebToString();
            speciesObservation.Fields.Add(field);

            if (taxonInformation.RedlistCategory.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Conservation.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.RedlistCategory.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.RedlistCategory;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.SwedishImmigrationHistory.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Conservation.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.SwedishImmigrationHistory.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.SwedishImmigrationHistory;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.SwedishOccurrence.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Conservation.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.SwedishOccurrence.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.SwedishOccurrence;
                speciesObservation.Fields.Add(field);
            }
        }

        /// <summary>
        /// Update species observation information in elasticsearch.
        /// </summary>
        /// <param name="dataReader">An open data reader with species observation information.</param>
        /// <param name="speciesObservation">Add information to this species observation instance.</param>
        /// <param name="dataProviders">All data providers.</param>
        private static void LoadDarwinCore(DataReader dataReader,
                                           WebSpeciesObservation speciesObservation,
                                           Dictionary<Int32, WebSpeciesObservationDataProvider> dataProviders)
        {
            WebSpeciesObservationDataProvider dataProvider;
            WebSpeciesObservationField field;

            dataProvider = dataProviders[dataReader.GetInt32((Int32)(DarwinCoreColumn.DataProviderId))];

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.AccessRights)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.AccessRights.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.AccessRights));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.BasisOfRecord)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.BasisOfRecord.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.BasisOfRecord));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.CollectionCode)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.CollectionCode.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.CollectionCode));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.CollectionId)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.CollectionID.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.CollectionId));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.DataGeneralizations)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.DataGeneralizations.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.DataGeneralizations));
                speciesObservation.Fields.Add(field);
            }

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
            field.PropertyIdentifier = SpeciesObservationElasticsearchData.DATA_PROVIDER_ID;
            field.Type = WebDataType.Int32;
            field.Value = dataProvider.Id.WebToString();
            speciesObservation.Fields.Add(field);

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.DatasetID.ToString();
            field.Type = WebDataType.String;
            field.Value = dataProvider.Guid;
            speciesObservation.Fields.Add(field);

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.DatasetName.ToString();
            field.Type = WebDataType.String;
            field.Value = dataProvider.Name;
            speciesObservation.Fields.Add(field);

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.DynamicProperties)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.DynamicProperties.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.DynamicProperties));
                speciesObservation.Fields.Add(field);
            }

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.Id.ToString();
            field.Type = WebDataType.Int64;
            field.Value = dataReader.GetInt64((Int32)(DarwinCoreColumn.Id)).WebToString();
            speciesObservation.Fields.Add(field);

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.InformationWithheld)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.InformationWithheld.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.InformationWithheld));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.InstitutionCode)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.InstitutionCode.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.InstitutionCode));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.InstitutionId)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.InstitutionID.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.InstitutionId));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.Language)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Language.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.Language));
                speciesObservation.Fields.Add(field);
            }

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.Modified.ToString();
            field.Type = WebDataType.DateTime;
            field.Value = dataReader.GetDateTime((Int32)(DarwinCoreColumn.Modified)).WebToString();
            speciesObservation.Fields.Add(field);

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.Owner)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Owner.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.Owner));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.OwnerInstitutionCode)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.OwnerInstitutionCode.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.OwnerInstitutionCode));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.References)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.References.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.References));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.ReportedBy)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.ReportedBy.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.ReportedBy));
                speciesObservation.Fields.Add(field);
            }

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.ReportedDate.ToString();
            field.Type = WebDataType.DateTime;
            field.Value = dataReader.GetDateTime((Int32)(DarwinCoreColumn.ReportedDate)).WebToString();
            speciesObservation.Fields.Add(field);

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.Rights)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Rights.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.Rights));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.RightsHolder)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.RightsHolder.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.RightsHolder));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.SpeciesObservationUrl)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.SpeciesObservationURL.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.SpeciesObservationUrl));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.Type)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Type.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.Type));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.ValidationStatus)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.DarwinCore.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.ValidationStatus.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.ValidationStatus));
                speciesObservation.Fields.Add(field);
            }
        }

        /// <summary>
        /// Update species observation information in elasticsearch.
        /// </summary>
        /// <param name="dataReader">An open data reader with species observation information.</param>
        /// <param name="speciesObservation">Add information to this species observation instance.</param>
        private static void LoadEvent(DataReader dataReader,
                                      WebSpeciesObservation speciesObservation)
        {
            WebSpeciesObservationField field;

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.Day)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Event.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Day.ToString();
                field.Type = WebDataType.Int32;
                field.Value = dataReader.GetInt16((Int32)(DarwinCoreColumn.Day)).WebToString();
                speciesObservation.Fields.Add(field);
            }

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Event.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.End.ToString();
            field.Type = WebDataType.DateTime;
            field.Value = dataReader.GetDateTime((Int32)(DarwinCoreColumn.End)).WebToString();
            speciesObservation.Fields.Add(field);

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.EndDayOfYear)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Event.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.EndDayOfYear.ToString();
                field.Type = WebDataType.Int32;
                field.Value = dataReader.GetInt16((Int32)(DarwinCoreColumn.EndDayOfYear)).WebToString();
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.EventDate)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Event.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.EventDate.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.EventDate));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.EventId)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Event.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.EventID.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.EventId));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.EventRemarks)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Event.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.EventRemarks.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.EventRemarks));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.EventTime)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Event.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.EventTime.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.EventTime));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.FieldNotes)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Event.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.FieldNotes.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.FieldNotes));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.FieldNumber)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Event.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.FieldNumber.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.FieldNumber));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.Habitat)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Event.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Habitat.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.Habitat));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.Month)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Event.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Month.ToString();
                field.Type = WebDataType.Int32;
                field.Value = dataReader.GetInt16((Int32)(DarwinCoreColumn.Month)).WebToString();
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.SamplingEffort)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Event.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.SamplingEffort.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.SamplingEffort));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.SamplingProtocol)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Event.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.SamplingProtocol.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.SamplingProtocol));
                speciesObservation.Fields.Add(field);
            }

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Event.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.Start.ToString();
            field.Type = WebDataType.DateTime;
            field.Value = dataReader.GetDateTime((Int32)(DarwinCoreColumn.Start)).WebToString();
            speciesObservation.Fields.Add(field);

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.StartDayOfYear)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Event.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.StartDayOfYear.ToString();
                field.Type = WebDataType.Int32;
                field.Value = dataReader.GetInt16((Int32)(DarwinCoreColumn.StartDayOfYear)).WebToString();
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.VerbatimEventDate)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Event.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.VerbatimEventDate.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimEventDate));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.Year)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Event.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Year.ToString();
                field.Type = WebDataType.Int32;
                field.Value = dataReader.GetInt16((Int32)(DarwinCoreColumn.Year)).WebToString();
                speciesObservation.Fields.Add(field);
            }
        }

        /// <summary>
        /// Update species observation information in elasticsearch.
        /// </summary>
        /// <param name="dataReader">An open data reader with species observation information.</param>
        /// <param name="speciesObservation">Add information to this species observation instance.</param>
        private static void LoadIdentification(DataReader dataReader,
                                               WebSpeciesObservation speciesObservation)
        {
            WebSpeciesObservationField field;

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.DateIdentified)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Identification.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.DateIdentified.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.DateIdentified));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.IdentificationId)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Identification.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.IdentificationID.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.IdentificationId));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.IdentificationQualifier)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Identification.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.IdentificationQualifier.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.IdentificationQualifier));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.IdentificationReferences)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Identification.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.IdentificationReferences.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.IdentificationReferences));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.IdentificationRemarks)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Identification.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.IdentificationRemarks.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.IdentificationRemarks));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.IdentificationVerificationStatus)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Identification.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.IdentificationVerificationStatus.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.IdentificationVerificationStatus));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.IdentifiedBy)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Identification.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.IdentifiedBy.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.IdentifiedBy));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.TypeStatus)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Identification.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.TypeStatus.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.TypeStatus));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.UncertainDetermination)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Identification.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.UncertainDetermination.ToString();
                field.Type = WebDataType.Boolean;
                field.Value = (dataReader.GetByte((Int32)(DarwinCoreColumn.UncertainDetermination)) == 1).WebToString();
                speciesObservation.Fields.Add(field);
            }
        }

        /// <summary>
        /// Update species observation information in elasticsearch.
        /// </summary>
        /// <param name="dataReader">An open data reader with species observation information.</param>
        /// <param name="speciesObservation">Add information to this species observation instance.</param>
        private static void LoadLocation(DataReader dataReader,
                                         WebSpeciesObservation speciesObservation)
        {
            WebSpeciesObservationField field;

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.Continent.ToString();
            field.Type = WebDataType.String;
            field.Value = SpeciesObservation.Settings.Default.ContinentName;
            speciesObservation.Fields.Add(field);

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.CoordinateM)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.CoordinateM.ToString();
                field.Type = WebDataType.Float64;
                field.Value = dataReader.GetDouble((Int32)(DarwinCoreColumn.CoordinateM)).WebToStringR();
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.CoordinatePrecision)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.CoordinatePrecision.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.CoordinatePrecision));
                speciesObservation.Fields.Add(field);
            }

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.CoordinateUncertaintyInMeters.ToString();
            field.Type = WebDataType.Int32;
            field.Value = dataReader.GetInt32((Int32)(DarwinCoreColumn.CoordinateUncertaintyInMeters)).WebToString();
            speciesObservation.Fields.Add(field);

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.CoordinateX.ToString();
            field.Type = WebDataType.Int32;
            field.Value = dataReader.GetInt32((Int32)(DarwinCoreColumn.CoordinateX)).WebToString();
            speciesObservation.Fields.Add(field);

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
            field.PropertyIdentifier = SpeciesObservation.Settings.Default.CoordinateXRt90FieldName;
            field.Type = WebDataType.Int32;
            field.Value = dataReader.GetInt32(SpeciesObservationElasticsearchData.COORDINATE_X_RT90).WebToString();
            speciesObservation.Fields.Add(field);

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
            field.PropertyIdentifier = SpeciesObservation.Settings.Default.CoordinateXSweref99FieldName;
            field.Type = WebDataType.Int32;
            field.Value = dataReader.GetInt32(SpeciesObservationElasticsearchData.COORDINATE_X_SWEREF99).WebToString();
            speciesObservation.Fields.Add(field);

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.CoordinateY.ToString();
            field.Type = WebDataType.Int32;
            field.Value = dataReader.GetInt32((Int32)(DarwinCoreColumn.CoordinateY)).WebToString();
            speciesObservation.Fields.Add(field);

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
            field.PropertyIdentifier = SpeciesObservation.Settings.Default.CoordinateYRt90FieldName;
            field.Type = WebDataType.Int32;
            field.Value = dataReader.GetInt32(SpeciesObservationElasticsearchData.COORDINATE_Y_RT90).WebToString();
            speciesObservation.Fields.Add(field);

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
            field.PropertyIdentifier = SpeciesObservation.Settings.Default.CoordinateYSweref99FieldName;
            field.Type = WebDataType.Int32;
            field.Value = dataReader.GetInt32(SpeciesObservationElasticsearchData.COORDINATE_Y_SWEREF99).WebToString();
            speciesObservation.Fields.Add(field);

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.CoordinateZ)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.CoordinateZ.ToString();
                field.Type = WebDataType.Float64;
                field.Value = dataReader.GetDouble((Int32)(DarwinCoreColumn.CoordinateZ)).WebToStringR();
                speciesObservation.Fields.Add(field);
            }

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.Country.ToString();
            field.Type = WebDataType.String;
            speciesObservation.Fields.Add(field);
            if (dataReader.GetString((Int32)(DarwinCoreColumn.CountryCode)) == SpeciesObservation.Settings.Default.CountryCodeSweden)
            {
                field.Value = SpeciesObservation.Settings.Default.CountryNameSweden;
            }
            else
            {
                field.Value = SpeciesObservation.Settings.Default.CountryNameUnknownCountry + ": " + dataReader.GetString((Int32)(DarwinCoreColumn.CountryCode));
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.CountryCode)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.CountryCode.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.CountryCode));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.County)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.County.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.County));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.DecimalLatitude)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.DecimalLatitude.ToString();
                field.Type = WebDataType.Float64;
                field.Value = dataReader.GetDouble((Int32)(DarwinCoreColumn.DecimalLatitude)).WebToStringR();
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.DecimalLongitude)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.DecimalLongitude.ToString();
                field.Type = WebDataType.Float64;
                field.Value = dataReader.GetDouble((Int32)(DarwinCoreColumn.DecimalLongitude)).WebToStringR();
                speciesObservation.Fields.Add(field);
            }

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
            field.PropertyIdentifier = SpeciesObservation.Settings.Default.DisturbanceRadiusFieldName;
            field.Type = WebDataType.Int32;
            field.Value = dataReader.GetInt32(SpeciesObservationElasticsearchData.DISTURBANCE_RADIUS).WebToString();
            speciesObservation.Fields.Add(field);

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.FootprintSpatialFit)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.FootprintSpatialFit.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.FootprintSpatialFit));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.FootprintSrs)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.FootprintSRS.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.FootprintSrs));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.FootprintWkt)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.FootprintWKT.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.FootprintWkt));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.GeodeticDatum)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.GeodeticDatum.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.GeodeticDatum));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.GeoreferencedBy)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.GeoreferencedBy.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.GeoreferencedBy));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.GeoreferencedDate)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.GeoreferencedDate.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.GeoreferencedDate));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.GeoreferenceProtocol)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.GeoreferenceProtocol.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.GeoreferenceProtocol));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.GeoreferenceRemarks)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.GeoreferenceRemarks.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.GeoreferenceRemarks));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.GeoreferenceSources)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.GeoreferenceSources.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.GeoreferenceSources));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.GeoreferenceVerificationStatus)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.GeoreferenceVerificationStatus.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.GeoreferenceVerificationStatus));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.HigherGeography)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.HigherGeography.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.HigherGeography));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.HigherGeographyId)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.HigherGeographyID.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.HigherGeographyId));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.Island)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Island.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.Island));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.IslandGroup)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.IslandGroup.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.IslandGroup));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.Locality)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Locality.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.Locality));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.LocationAccordingTo)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.LocationAccordingTo.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.LocationAccordingTo));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.LocationId)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.LocationId.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.LocationId));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.LocationRemarks)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.LocationRemarks.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.LocationRemarks));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.LocationUrl)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.LocationURL.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.LocationUrl));
                speciesObservation.Fields.Add(field);
            }

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
            field.PropertyIdentifier = SpeciesObservation.Settings.Default.MaxCoordinateUncertaintyInMetersOrDisturbanceRadiusFieldName;
            field.Type = WebDataType.Int32;
            field.Value = dataReader.GetInt32(SpeciesObservationElasticsearchData.MAX_ACCURACY_OR_DISTURBANCE_RADIUS).WebToString();
            speciesObservation.Fields.Add(field);

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.MaximumDepthInMeters)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.MaximumDepthInMeters.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.MaximumDepthInMeters));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.MaximumDistanceAboveSurfaceInMeters)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.MaximumDistanceAboveSurfaceInMeters.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.MaximumDistanceAboveSurfaceInMeters));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.MaximumElevationInMeters)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.MaximumElevationInMeters.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.MaximumElevationInMeters));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.MinimumDepthInMeters)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.MinimumDepthInMeters.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.MinimumDepthInMeters));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.MinimumDistanceAboveSurfaceInMeters)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.MinimumDistanceAboveSurfaceInMeters.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.MinimumDistanceAboveSurfaceInMeters));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.MinimumElevationInMeters)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.MinimumElevationInMeters.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.MinimumElevationInMeters));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.Municipality)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Municipality.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.Municipality));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.Parish)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Parish.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.Parish));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.PointRadiusSpatialFit)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.PointRadiusSpatialFit.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.PointRadiusSpatialFit));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.StateProvince)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.StateProvince.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.StateProvince));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.VerbatimCoordinates)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.VerbatimCoordinates.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimCoordinates));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.VerbatimCoordinateSystem)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.VerbatimCoordinateSystem.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimCoordinateSystem));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.VerbatimDepth)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.VerbatimDepth.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimDepth));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.VerbatimElevation)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.VerbatimElevation.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimElevation));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.VerbatimLatitude)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.VerbatimLatitude.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimLatitude));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.VerbatimLocality)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.VerbatimLocality.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimLocality));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.VerbatimLongitude)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.VerbatimLongitude.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimLongitude));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.VerbatimSrs)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.VerbatimSRS.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimSrs));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.WaterBody)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.WaterBody.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.WaterBody));
                speciesObservation.Fields.Add(field);
            }
        }

        /// <summary>
        /// Update species observation information in elasticsearch.
        /// </summary>
        /// <param name="dataReader">An open data reader with species observation information.</param>
        /// <param name="speciesObservation">Add information to this species observation instance.</param>
        private static void LoadOccurrence(DataReader dataReader,
                                           WebSpeciesObservation speciesObservation)
        {
            WebSpeciesObservationField field;

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.AssociatedMedia)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.AssociatedMedia.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.AssociatedMedia));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.AssociatedOccurrences)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.AssociatedOccurrences.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.AssociatedOccurrences));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.AssociatedReferences)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.AssociatedReferences.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.AssociatedReferences));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.AssociatedSequences)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.AssociatedSequences.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.AssociatedSequences));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.AssociatedTaxa)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.AssociatedTaxa.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.AssociatedTaxa));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.Behavior)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Behavior.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.Behavior));
                speciesObservation.Fields.Add(field);
            }

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
            field.PropertyIdentifier = SpeciesObservation.Settings.Default.BirdNestActivityIdFieldName;
            field.Type = WebDataType.Int32;
            speciesObservation.Fields.Add(field);
            if (dataReader.IsDbNull(SpeciesObservationElasticsearchData.BIRD_NEST_ACTIVITY_ID))
            {
                field.Value = SpeciesObservation.Settings.Default.DefaultBirdNestActivityId.WebToString();
            }
            else
            {
                field.Value = dataReader.GetInt32(SpeciesObservationElasticsearchData.BIRD_NEST_ACTIVITY_ID).WebToString();
            }

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.CatalogNumber.ToString();
            field.Type = WebDataType.String;
            field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.CatalogNumber));
            speciesObservation.Fields.Add(field);

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.Disposition)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Disposition.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.Disposition));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.EstablishmentMeans)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.EstablishmentMeans.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.EstablishmentMeans));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.IndividualCount)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.IndividualCount.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.IndividualCount));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.IndividualId)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.IndividualID.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.IndividualId));
                speciesObservation.Fields.Add(field);
            }

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.IsNaturalOccurrence.ToString();
            field.Type = WebDataType.Boolean;
            field.Value = (dataReader.GetByte((Int32)(DarwinCoreColumn.IsNaturalOccurrence)) == 1).WebToString();
            speciesObservation.Fields.Add(field);

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.IsNeverFoundObservation)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.IsNeverFoundObservation.ToString();
                field.Type = WebDataType.Boolean;
                field.Value = (dataReader.GetByte((Int32)(DarwinCoreColumn.IsNeverFoundObservation)) == 1).WebToString();
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.IsNotRediscoveredObservation)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.IsNotRediscoveredObservation.ToString();
                field.Type = WebDataType.Boolean;
                field.Value = (dataReader.GetByte((Int32)(DarwinCoreColumn.IsNotRediscoveredObservation)) == 1).WebToString();
                speciesObservation.Fields.Add(field);
            }

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.IsPositiveObservation.ToString();
            field.Type = WebDataType.Boolean;
            field.Value = (dataReader.GetByte((Int32)(DarwinCoreColumn.IsPositiveObservation)) == 1).WebToString();
            speciesObservation.Fields.Add(field);

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.LifeStage)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.LifeStage.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.LifeStage));
                speciesObservation.Fields.Add(field);
            }

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.OccurrenceID.ToString();
            field.Type = WebDataType.String;
            field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.OccurrenceId));
            speciesObservation.Fields.Add(field);

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.OccurrenceRemarks)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.OccurrenceRemarks.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.OccurrenceRemarks));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.OccurrenceStatus)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.OccurrenceStatus.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.OccurrenceStatus));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.OccurrenceUrl)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.OccurrenceURL.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.OccurrenceUrl));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.OtherCatalogNumbers)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.OtherCatalogNumbers.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.OtherCatalogNumbers));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.Preparations)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Preparations.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.Preparations));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.PreviousIdentifications)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.PreviousIdentifications.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.PreviousIdentifications));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.Quantity)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Quantity.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.Quantity));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.QuantityUnit)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.QuantityUnit.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.QuantityUnit));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.RecordedBy)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.RecordedBy.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.RecordedBy));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.RecordNumber)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.RecordNumber.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.RecordNumber));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.ReproductiveCondition)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.ReproductiveCondition.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.ReproductiveCondition));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.Sex)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Sex.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.Sex));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.ActivityId)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservation.Settings.Default.SpeciesActivityIdFieldName;
                field.Type = WebDataType.Int32;
                field.Value = dataReader.GetInt32((Int32)(DarwinCoreColumn.ActivityId)).WebToString();
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.Substrate)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Occurrence.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Substrate.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.Substrate));
                speciesObservation.Fields.Add(field);
            }
        }

        /// <summary>
        /// Update species observation information in elasticsearch.
        /// </summary>
        /// <param name="dataReader">An open data reader with species observation information.</param>
        /// <param name="speciesObservation">Add information to this species observation instance.</param>
        private static void LoadProject(DataReader dataReader,
                                        WebSpeciesObservation speciesObservation)
        {
            WebSpeciesObservationField field;

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.ProjectIsPublic)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Project.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.IsPublic.ToString();
                field.Type = WebDataType.Boolean;
                field.Value = (dataReader.GetByte((Int32)(DarwinCoreColumn.ProjectIsPublic)) == 1).WebToString();
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.ProjectCategory)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Project.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.ProjectCategory.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.ProjectCategory));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.ProjectDescription)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Project.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.ProjectDescription.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.ProjectDescription));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.ProjectEndDate)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Project.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.ProjectEndDate.ToString();
                field.Type = WebDataType.DateTime;
                field.Value = dataReader.GetDateTime((Int32)(DarwinCoreColumn.ProjectEndDate)).WebToString();
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.ProjectId)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Project.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.ProjectID.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.ProjectId));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.ProjectName)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Project.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.ProjectName.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.ProjectName));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.ProjectOwner)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Project.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.ProjectOwner.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.ProjectOwner));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.ProjectStartDate)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Project.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.ProjectStartDate.ToString();
                field.Type = WebDataType.DateTime;
                field.Value = dataReader.GetDateTime((Int32)(DarwinCoreColumn.ProjectStartDate)).WebToString();
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.ProjectUrl)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Project.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.ProjectURL.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.ProjectUrl));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.ProjectSurveyMethod)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Project.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.SurveyMethod.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.ProjectSurveyMethod));
                speciesObservation.Fields.Add(field);
            }
        }


        /// <summary>
        /// Update species observation information in elasticsearch.
        /// </summary>
        /// <param name="dataReader">An open data reader with species observation information.</param>
        /// <param name="speciesObservation">Add information to this species observation instance.</param>
        /// <param name="taxonInformation">Taxon information.</param>
        private static void LoadTaxon(DataReader dataReader,
                                      WebSpeciesObservation speciesObservation,
                                      TaxonInformation taxonInformation)
        {
            Int32 dyntaxaTaxonId;
            WebSpeciesObservationField field;

            dyntaxaTaxonId = dataReader.GetInt32((Int32)(DarwinCoreColumn.DyntaxaTaxonId));

            if (taxonInformation.Class.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Class.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.Class;
                speciesObservation.Fields.Add(field);
            }

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.DyntaxaTaxonID.ToString();
            field.Type = WebDataType.Int32;
            field.Value = dyntaxaTaxonId.WebToString();
            speciesObservation.Fields.Add(field);

            if (taxonInformation.Family.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Family.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.Family;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.Genus.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Genus.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.Genus;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.HigherClassification.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.HigherClassification.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.HigherClassification;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.InfraspecificEpithet.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.InfraspecificEpithet.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.InfraspecificEpithet;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.Kingdom.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Kingdom.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.Kingdom;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.NameAccordingTo.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.NameAccordingTo.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.NameAccordingTo;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.NameAccordingToId.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.NameAccordingToID.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.NameAccordingToId;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.NamePublishedIn.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.NamePublishedIn.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.NamePublishedIn;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.NamePublishedInId.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.NamePublishedInID.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.NamePublishedInId;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.NamePublishedInYear.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.NamePublishedInYear.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.NamePublishedInYear;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.NomenclaturalCode.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.NomenclaturalCode.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.NomenclaturalCode;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.NomenclaturalStatus.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.NomenclaturalStatus.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.NomenclaturalStatus;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.Order.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Order.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.Order;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.OrganismGroup.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.OrganismGroup.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.OrganismGroup;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.OriginalNameUsage.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.OriginalNameUsage.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.OriginalNameUsage;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.OriginalNameUsageId.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.OriginalNameUsageID.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.OriginalNameUsageId;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.Phylum.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Phylum.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.Phylum;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.ScientificName.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.ScientificName.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.ScientificName;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.ScientificNameAuthorship.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.ScientificNameAuthorship.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.ScientificNameAuthorship;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.ScientificNameId.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.ScientificNameID.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.ScientificNameId;
                speciesObservation.Fields.Add(field);
            }

            //This loads the dyntaxaTaxonId for the specie(or parent specie in the case of a subspecie), only used with taxon categories 'specie' and 'subspecie'
            if (taxonInformation.SpeciesTaxonId > 0)
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = "Species_TaxonId";
                field.Type = WebDataType.Int32;
                field.Value = taxonInformation.SpeciesTaxonId.WebToString();
                speciesObservation.Fields.Add(field);
            }


            if (taxonInformation.SpecificEpithet.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.SpecificEpithet.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.SpecificEpithet;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.Subgenus.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.Subgenus.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.Subgenus;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.TaxonConceptId.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.TaxonConceptID.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.TaxonConceptId;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.TaxonConceptStatus.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.TaxonConceptStatus.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.TaxonConceptStatus;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.TaxonConceptId.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.TaxonID.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.TaxonConceptId;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.TaxonomicStatus.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.TaxonomicStatus.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.TaxonomicStatus;
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.TaxonRank.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.TaxonRank.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.TaxonRank;
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.TaxonRemarks)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.TaxonRemarks.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.TaxonRemarks));
                speciesObservation.Fields.Add(field);
            }

            // Support for field taxon sort order has been removed since data
            // update handling (species observation) could not cope with all changes. 
            //field = new WebSpeciesObservationField();
            //field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
            //field.PropertyIdentifier = SpeciesObservationPropertyId.TaxonSortOrder.ToString();
            //field.Type = WebDataType.Int32;
            //field.Value = taxonInformation.TaxonSortOrder.WebToString();
            //speciesObservation.Fields.Add(field);

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
            field.PropertyIdentifier = SpeciesObservationPropertyId.TaxonURL.ToString();
            field.Type = WebDataType.String;
            field.Value = SpeciesObservation.Settings.Default.TaxonInformationUrl + dyntaxaTaxonId;
            speciesObservation.Fields.Add(field);

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.VerbatimScientificName)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.VerbatimScientificName.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimScientificName));
                speciesObservation.Fields.Add(field);
            }

            if (dataReader.IsNotDbNull((Int32)(DarwinCoreColumn.VerbatimTaxonRank)))
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.VerbatimTaxonRank.ToString();
                field.Type = WebDataType.String;
                field.Value = dataReader.GetString((Int32)(DarwinCoreColumn.VerbatimTaxonRank));
                speciesObservation.Fields.Add(field);
            }

            if (taxonInformation.VernacularName.IsNotEmpty())
            {
                field = new WebSpeciesObservationField();
                field.ClassIdentifier = SpeciesObservationClassId.Taxon.ToString();
                field.PropertyIdentifier = SpeciesObservationPropertyId.VernacularName.ToString();
                field.Type = WebDataType.String;
                field.Value = taxonInformation.VernacularName;
                speciesObservation.Fields.Add(field);
            }
        }

        /// <summary>
        /// Pause currently running harvest job.
        /// </summary>
        /// <param name="context">Web service context.</param>
        public static void PauseSpeciesObservationUpdate(WebServiceContext context)
        {
            if (context.ClientToken.UserName != ApplicationIdentifier.SpeciesObservationHarvestService.ToString())
            {
                WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.WebServiceAdministrator);
            }

            RequestedStatus = HarvestStatusEnum.Paused;
        }

        /// <summary>
        /// Continue a paused harvest job.
        /// </summary>
        /// <param name="context">Web service context.</param>
        public static void ContinueSpeciesObservationUpdate(WebServiceContext context)
        {
            if (context.ClientToken.UserName != ApplicationIdentifier.SpeciesObservationHarvestService.ToString())
            {
                WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.WebServiceAdministrator);
            }

            RequestedStatus = HarvestStatusEnum.Working;

            // Read status for current job in table HarvestJob. No job = status DONE
            var harvestJobManager = new HarvestJobManager(context);
            var harvestJob = harvestJobManager.GetHarvestJob();
            CurrentStatus = harvestJob.JobStatus;

            // Start work when currentStatus is status Paused and requestedStatus differ from currentStatus
            if (CurrentStatus.Equals(HarvestStatusEnum.Paused) && !RequestedStatus.Equals(CurrentStatus))
            {
                StartSpeciesObservationUpdateThread();
            }
        }

        /// <summary>
        /// Get species observation update status.
        /// </summary>
        /// <param name="context">Web service context.</param>
        /// <returns>Web object.</returns>
        public static WebSpeciesObservationHarvestStatus GetSpeciesObservationUpdateStatus(WebServiceContext context)
        {
            return GetSpeciesObservationHarvestStatus(context);
        }

        /// <summary>
        /// Get species observation harvest status.
        /// </summary>
        /// <param name="context">Web service context.</param>
        /// <returns>Web object.</returns>
        private static WebSpeciesObservationHarvestStatus GetSpeciesObservationHarvestStatus(WebServiceContext context)
        {
            // Read status for current job.
            var harvestJobManager = new HarvestJobManager(context);
            var harvestJob = harvestJobManager.GetHarvestJob();
            var webSpeciesObservationHarvestStatus = new WebSpeciesObservationHarvestStatus()
            {
                CurrentHarvestDate = harvestJob.HarvestCurrentDate,
                HarvestFromDate = harvestJob.HarvestStartDate,
                HarvestToDate = harvestJob.HarvestEndDate,
                JobEndDate = harvestJob.JobEndDate.HasValue ? harvestJob.JobEndDate.Value : DateTime.MinValue,
                JobStartDate = harvestJob.JobStartDate,
                JobStatus = harvestJob.JobStatus.ToString(),
                RequestedJobStatus = RequestedStatus.ToString()
            };

            webSpeciesObservationHarvestStatus.DataProviders = new List<WebSpeciesObservationDataProvider>();
            foreach (var dataProvider in harvestJob.DataProviders)
            {
                var webSpeciesObservationDataProvider = new WebSpeciesObservationDataProvider()
                {
                    MaxChangeId = dataProvider.ChangeId,
                    Id = dataProvider.DataProviderId
                };
                webSpeciesObservationHarvestStatus.DataProviders.Add(webSpeciesObservationDataProvider);
            }

            webSpeciesObservationHarvestStatus.Statistics = new List<WebSpeciesObservationStatistic>();
            foreach (var dataProviderStatistics in harvestJob.Statistics)
            {
                var webSpeciesObservationDataProviderStatistic = new WebSpeciesObservationStatistic()
                {
                    DataProviderId = dataProviderStatistics.DataProviderId,
                    JobStatus = dataProviderStatistics.JobStatus.ToString(),
                    HarvestDate = dataProviderStatistics.HarvestDate
                };
                webSpeciesObservationHarvestStatus.Statistics.Add(webSpeciesObservationDataProviderStatistic);
            }

            webSpeciesObservationHarvestStatus.ActiveDataProviders = GetActiveDataProviders(context).ToList();

            return webSpeciesObservationHarvestStatus;
        }


        /// <summary>
        /// Returns a (list of) WebRegionGeography that represents 'Sweden with surroundings' - used to validate that all observations is made in Sweden
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static List<WebRegionGeography> GetSweden(WebServiceContext context)
        {
            if (_sweden != null)
            {
                return _sweden;
            }

            var swedenGuid = new RegionGUID(AreaDatasetIdForSwedenPolygon, FeatureIdForSwedenPolygon).GUID;
            var swedenRegionIds = WebServiceData.RegionManager.GetRegionIdsByGuids(context, new List<string>() { swedenGuid });
            _sweden = WebServiceData.RegionManager.GetRegionsGeographyByIds(context, swedenRegionIds, new WebCoordinateSystem() { Id = CoordinateSystemId.GoogleMercator });
            return _sweden;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SpeciesObservation.Database
{
    /// <summary>
    /// Class that contains species observation specific functionality
    /// that is used when communicating with Elasticsearch.
    /// </summary>
    public class ElasticsearchSpeciesObservationProxy : ElasticsearchProxy
    {
        /// <summary>
        /// Create an ElasticsearchSpeciesObservationProxyBase instance.
        /// </summary>
        /// <param name="indexName">Index name.</param>
        public ElasticsearchSpeciesObservationProxy(String indexName)
        {
            IndexName = indexName;
            SpeciesObservationType = Settings.Default.SpeciesObservationTypeNameElasticsearch;
        }

        /// <summary>
        /// Name of the species observation type.
        /// </summary>
        public String SpeciesObservationType { get; private set; }

        /// <summary>
        /// Create species observation index in elasticsearch.
        /// It is assumed that property IndexName contains the name of the new index.
        /// </summary>
        public void CreateIndex()
        {
            String mapping; //, settings;

            // Moneses Elasticsearch 2.1.0 
            // Current index name: "swedish_species_observation" + "_2015_??_??"
            // Current index alias: "swedish_species_observation"
            // CurrentIndexAlias "swedish_species_observation"
            // CurrentIndexChangeId 56025824
            // CurrentIndexCount 6988859
            // CurrentIndexName "swedish_species_observation_2015_10_14"
            // NextIndexChangeId 56247485
            // NextIndexCount 6893756
            // NextIndexName "swedish_species_observation_2015_12_16"

            // Locale test Elasticsearch 2.4.1 
            // Current index name: "swedish_species_observation" + "_2016_10_14"
            // Current index alias: "swedish_species_observation"
            // CurrentIndexAlias "swedish_species_observation"
            // CurrentIndexChangeId 0
            // CurrentIndexCount 0
            // CurrentIndexName "swedish_species_observation_2016_10_14"
            // NextIndexChangeId 0
            // NextIndexCount 0
            // NextIndexName "swedish_species_observation_2016_10_14"

            // Create index.
            CreateIndex(IndexName);

            // Update mapping.
            mapping = "{" +
                      "\"_all\": {\"enabled\": false}," +
                      "\"properties\": {" +
                      "\"CoordinateUncertaintyInMeters\": {\"type\": \"geo_shape\", \"precision\": \"2m\", \"distance_error_pct\": 0.025}," +
                      "\"DisturbanceRadius\": {\"type\": \"geo_shape\", \"precision\": \"2m\", \"distance_error_pct\": 0.025}," +
                      "\"Location\": {\"type\": \"geo_shape\", \"precision\": \"2m\", \"distance_error_pct\": 0.025}," +
                      "\"ObservationDateTimeAccuracy\": {\"type\": \"long\"}," +
                      "\"ObservationDateTimeIsOneDay\": {\"type\": \"boolean\"}," +
                      "\"ObservationDateTimeIsOneWeek\": {\"type\": \"boolean\"}," +
                      "\"ObservationDateTimeIsOneMonth\": {\"type\": \"boolean\"}," +
                      "\"ObservationDateTimeIsOneYear\": {\"type\": \"boolean\"}" +
                      "}}";
            UpdateMapping(SpeciesObservationType, mapping);

            // Update setting.
            // Moved to configuration file for Elasticsearch 2.1
            //settings = "{\"refresh_interval\" : \"" +
            //           Settings.Default.RefreshIntervalElasticsearch +
            //           "s\"}";
            //UpdateSettings(settings);
        }

        /// <summary>
        /// Delete species observation index in elasticsearch.
        /// It is assumed that property IndexName contains the
        /// name of the index that should be deleted.
        /// </summary>
        public void DeleteIndex()
        {
            DeleteIndex(IndexName);
        }

        /// <summary>
        /// Delete specified document from Elasticsearch.
        /// It is ok to try to delete a document that is not in the index.
        /// </summary>
        /// <param name="occurrenceId">Occurrence id.</param>
        public void DeleteSpeciesObservation(String occurrenceId)
        {
            DeleteDocument(SpeciesObservationType, occurrenceId);
        }

        /// <summary>
        /// Get name of specified field in the database.
        /// </summary>
        /// <param name="classId">Species observation class.</param>
        /// <param name="propertyId">Species observation property.</param>
        /// <returns>Name of specified field in the database.</returns>
        public String GetFieldName(SpeciesObservationClassId classId,
                                   SpeciesObservationPropertyId propertyId)
        {
            return classId + "_" + propertyId;
        }

        /// <summary>
        /// Get name of specified field in the database.
        /// </summary>
        /// <param name="className">Species observation class.</param>
        /// <param name="propertyName">Species observation property.</param>
        /// <returns>Name of specified field in the database.</returns>
        public String GetFieldName(String className,
                                   String propertyName)
        {
            return className + "_" + propertyName;
        }


        public long GroupByYearToCount(string filter)
        {
            return GroupBy(SpeciesObservationType, new ElasticsearchTerm("Event_Year"), filter).UniqueValues.Count;
        }

        public List<DocumentUniqueValue> GroupByTaxonToCount(string filter)
        {
            return GroupByCountAndSum(SpeciesObservationType, new List<ElasticsearchTerm> { new ElasticsearchTerm("Taxon_DyntaxaTaxonID") }, ":", filter).UniqueValues;
        }

        public List<DocumentUniqueValue> GroupByYearTaxonToCount(string filter)
        {
            return GroupByCountAndSum(SpeciesObservationType, new List<ElasticsearchTerm> { new ElasticsearchTerm("Event_Year"), new ElasticsearchTerm("Taxon_DyntaxaTaxonID") }, ":", filter).UniqueValues;
        }

        public Dictionary<string, long> GroupByYearToList(string filter)
        {
            return GroupBy(SpeciesObservationType, new ElasticsearchTerm("Event_Year"), filter).UniqueValues;
        }

        public long GroupByYearCountryTaxonIdToCount(string filter)
        {
            return GroupBy(SpeciesObservationType, new List<ElasticsearchTerm> { new ElasticsearchTerm("Location_Country"), new ElasticsearchTerm("Event_Year"), new ElasticsearchTerm("Taxon_DyntaxaTaxonID") }, ":", filter).UniqueValues.Count;
        }
        public Dictionary<string, long> GroupByYearCountryTaxonIdToList(string filter)
        {
            return GroupBy(SpeciesObservationType, new List<ElasticsearchTerm> { new ElasticsearchTerm("Location_Country"), new ElasticsearchTerm("Event_Year"), new ElasticsearchTerm("Taxon_DyntaxaTaxonID") }, ":", filter).UniqueValues;
        }

        public List<DocumentUniqueValue> GetGridSpeciesCounts(string filter, WebGridSpecification gridSpecification)
        {
            string script;

            switch (gridSpecification.GridCoordinateSystem)
            {
                case GridCoordinateSystem.Rt90_25_gon_v:
                    script = string.Format("(floor(doc['Location_{0}'].value / {1}) * {1} + {1} / 2).toString() + ':' + (floor(doc['Location_{2}'].value / {1}) * {1} + {1} / 2).toString()", Settings.Default.CoordinateXRt90FieldName, gridSpecification.GridCellSize, Settings.Default.CoordinateYRt90FieldName);
                    break;

                case GridCoordinateSystem.SWEREF99_TM:
                    script = string.Format("(floor(doc['Location_{0}'].value / {1}) * {1} + {1} / 2).toString() + ':' + (floor(doc['Location_{2}'].value / {1}) * {1} + {1} / 2).toString()", Settings.Default.CoordinateXSweref99FieldName, gridSpecification.GridCellSize, Settings.Default.CoordinateYSweref99FieldName);
                    break;
                default:
                    throw new NotImplementedException(string.Format("Coordinate system not supported in grid query [{0}]", gridSpecification.GridCoordinateSystem));
            }

            return GroupByCountAndSum(SpeciesObservationType, new List<ElasticsearchTerm> { new ElasticsearchTerm(script) { TermType = ElasticsearchTermType.Script }, new ElasticsearchTerm("Taxon_Species_TaxonId") }, null, filter).UniqueValues;
        }

        /// <summary>
        /// Get the TOP 20 unique DataProviderIds and their document count on documents that matches filter.
        /// </summary>
        /// <param name="filter">Filter for species observations.</param>
        /// <returns>Species count.</returns>
        public DocumentUniqueValuesResponse GetProvenanceUniqueDataProviders(string filter)
        {
            return GetProvenanceUniqueValues("DarwinCore_DatasetName", filter);
        }

        /// <summary>
        /// Get the TOP 20 unique Owners and their document count on documents that matches filter.
        /// </summary>
        /// <param name="filter">Filter for species observations.</param>
        /// <returns>Species count.</returns>
        public DocumentUniqueValuesResponse GetProvenanceUniqueOwners(string filter)
        {
            return GetProvenanceUniqueValues("DarwinCore_Owner", filter);
        }

        /// <summary>
        /// Get the TOP 20 unique Observer and their document count on documents that matches filter.
        /// </summary>
        /// <param name="filter">Filter for species observations.</param>
        /// <returns>Species count.</returns>
        public DocumentUniqueValuesResponse GetProvenanceUniqueObservers(string filter)
        {
            return GetProvenanceUniqueValues("Occurrence_RecordedBy", filter);
        }

        /// <summary>
        /// Get the TOP 20 unique Reporters and their document count on documents that matches filter.
        /// </summary>
        /// <param name="filter">Filter for species observations.</param>
        /// <returns>Species count.</returns>
        public DocumentUniqueValuesResponse GetProvenanceUniqueReporters(string filter)
        {
            return GetProvenanceUniqueValues("DarwinCore_ReportedBy", filter);
        }

        /// <summary>
        /// Returns the TOP 20 unique values and their document count on documents that matches filter.
        /// It requests the TOP 100 to mitigate the risk that the document count has errors, see https://www.elastic.co/guide/en/elasticsearch/reference/2.1/search-aggregations-bucket-terms-aggregation.html
        /// </summary>
        /// <param name="searchTerm">The search term, i.e. a field name</param>
        /// <param name="filter">Filter for species observations.</param>
        /// <returns>Species count.</returns>
        private DocumentUniqueValuesResponse GetProvenanceUniqueValues(string searchTerm, string filter)
        {
            const int top = 20;
            var topTwenty = GroupBy(SpeciesObservationType, new ElasticsearchTerm(searchTerm) { Top = top, SortType = ElasticsearchSortType.DocumentCount, SortOrder = ElasticsearchSortOrder.Descending }, filter);

            var index = 0;
            foreach (var uniqueValue in topTwenty.UniqueValues.ToList())
            {
                index++;
                if (index > top)
                {
                    topTwenty.UniqueValues.Remove(uniqueValue.Key);
                }
            }


            return topTwenty;
        }


        /// <summary>
        /// Get species count that matches filter.
        /// </summary>
        /// <param name="filter">Filter for species observations.</param>
        /// <returns>Species count.</returns>
        public long GetSpeciesCount(string filter)
        {
            Debug.WriteLine("Species count filter = " + filter);
            return GroupBy(SpeciesObservationType, new ElasticsearchTerm("Taxon_Species_TaxonId"), filter).UniqueValues.Count;
        }

        /// <summary>
        /// Get species observation count that matches filter.
        /// </summary>
        /// <param name="filter">Filter for species observations.</param>
        /// <returns>Species observation count.</returns>
        public DocumentCountResponse GetSpeciesObservationCount(String filter)
        {
            Debug.WriteLine("Species observation count filter = " + filter);
            return GetCount(SpeciesObservationType, filter);
        }

        /// <summary>
        /// Get field mappings.
        /// </summary>
        /// <returns>Field mappings.</returns>
        public FieldDefinitionList GetSpeciesObservationMapping()
        {
            return GetMapping(SpeciesObservationType);
        }

        /// <summary>
        /// Get species observations that matches filter.
        /// </summary>
        /// <param name="filter">Filter for species observations.</param>
        /// <returns>Species observations in JSON format.</returns>
        public DocumentFilterResponse GetSpeciesObservations(String filter)
        {
            Debug.WriteLine("Species observation filter = " + filter);
            return GetDocuments(SpeciesObservationType, filter);
        }

        /// <summary>
        /// Get species observations that matches filter.
        /// This method returns next documents in the scroll in an effecient way.
        /// </summary>
        /// <param name="filter">Filter for species observations.</param>
        /// <param name="scroll">Information about the scroll.</param>
        /// <returns>Species observations in JSON format.</returns>
        public DocumentFilterResponse GetSpeciesObservations(String filter,
                                                             ElasticsearchScroll scroll)
        {
            if (scroll.ScrollId.IsEmpty())
            {
                return StartScroll(filter, scroll);
            }
            else
            {
                return GetScroll(scroll);
            }
        }

        /// <summary>
        /// Get unique taxonId's (Taxon_DyntaxaTaxonID) that matches filter.
        /// </summary>
        /// <param name="filter">Filter for species observations.</param>
        /// <returns>Species count.</returns>
        public DocumentUniqueValuesResponse GetTaxonIdUniqueValues(string filter)
        {
            return GroupBy(SpeciesObservationType, new ElasticsearchTerm("Taxon_DyntaxaTaxonID"), filter);
        }

        public Dictionary<string, long> GetTimeSpeciesObservationCountsBySearchCriteria(string filter, Periodicity periodicity)
        {
            Dictionary<string, long> uniqueValues;

            switch (periodicity)
            {
                case Periodicity.Yearly:
                    uniqueValues = GroupBy(SpeciesObservationType, new ElasticsearchTerm("Event_Start_Year"), filter).UniqueValues;
                    break;

                case Periodicity.MonthOfTheYear:
                    uniqueValues = GroupBy(SpeciesObservationType, new ElasticsearchTerm("Event_Start_MonthOfYear"), filter).UniqueValues;
                    break;

                case Periodicity.WeekOfTheYear:
                    uniqueValues = GroupBy(SpeciesObservationType, new ElasticsearchTerm("Event_Start_WeekOfYear"), filter).UniqueValues;
                    break;

                case Periodicity.DayOfTheYear:
                    uniqueValues = GroupBy(SpeciesObservationType, new ElasticsearchTerm("Event_Start_DayOfYear"), filter).UniqueValues;
                    break;

                case Periodicity.Monthly:
                    uniqueValues = GroupBy(SpeciesObservationType, new ElasticsearchTerm("Event_Start_YearAndMonth"), filter).UniqueValues;
                    break;

                case Periodicity.Weekly:
                    uniqueValues = GroupBy(SpeciesObservationType, new ElasticsearchTerm("Event_Start_YearAndWeek"), filter).UniqueValues;
                    break;

                case Periodicity.Daily:
                    uniqueValues = GroupBy(SpeciesObservationType, new ElasticsearchTerm("Event_Start_DatePartOnly"), filter).UniqueValues;
                    break;

                default:
                    throw new NotImplementedException(string.Format("Periodicity not recognized [{0}]", periodicity));
            }
            return uniqueValues;
        }

        /// <summary>
        /// Start scroll of documents that matches search criteria.
        /// </summary>
        /// <param name="filter">Filter for species observations.</param>
        /// <param name="scroll">Information about the scroll.</param>
        /// <returns>The first set of species observation.</returns>
        public DocumentFilterResponse StartScroll(String filter,
                                                  ElasticsearchScroll scroll)
        {
            return StartScroll(SpeciesObservationType, filter, scroll);
        }

        /// <summary>
        /// Create species observation information in elasticsearch.
        /// </summary>
        /// <param name="speciesObservationIdentifier">Unique species observation identifier.</param>
        /// <param name="speciesObservationJson">Species observation in JSon format.</param>
        public void UpdateSpeciesObservation(String speciesObservationIdentifier,
                                             String speciesObservationJson)
        {
            Index(SpeciesObservationType,
                  speciesObservationIdentifier,
                  speciesObservationJson);
        }

        /// <summary>
        /// Update mappings.
        /// </summary>
        /// <param name="mapping">Modified mapping.</param>
        /// <returns>Updated mappings.</returns>
        public FieldDefinitionList UpdateSpeciesObservationMapping(String mapping)
        {
            return UpdateMapping(SpeciesObservationType, mapping);
        }

        /// <summary>
        /// Create species observations information in elasticsearch.
        /// </summary>
        /// <param name="speciesObservationsJson">Species observations in JSon format.</param>
        public void UpdateSpeciesObservations(String speciesObservationsJson)
        {
            if (speciesObservationsJson.IsNotEmpty())
            {
                Bulk(speciesObservationsJson);
            }
        }
    }
}

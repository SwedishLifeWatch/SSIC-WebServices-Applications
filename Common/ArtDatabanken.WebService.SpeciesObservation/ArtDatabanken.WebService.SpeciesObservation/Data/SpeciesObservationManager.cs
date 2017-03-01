using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web.Caching;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Database;

namespace ArtDatabanken.WebService.SpeciesObservation.Data
{
    /// <summary>
    /// Manager of meta information about databases.
    /// </summary>
    public class SpeciesObservationManager : ISpeciesObservationManager
    {
        /// <summary>
        /// Check that the species observation
        /// database is not updating right now.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <exception cref="ApplicationException">Thrown if database is beeing updated.</exception>
        public void CheckAutomaticDatabaseUpdate(WebServiceContext context)
        {
            DateTime now, updateEnd, updateStart;

            now = DateTime.Now;
            updateStart = new DateTime(2000, 1, 1, 4, 0, 0);
            updateEnd = new DateTime(2000, 1, 1, 4, 0, 0);
            if ((updateStart.Hour <= now.Hour) &&
                (now.Hour <= updateEnd.Hour))
            {
                // Wait a minute in order to avoid
                // overload on database server.
                Thread.Sleep(60000);

                throw new ApplicationException("Database is beeing updated!");
            }
        }

        /// <summary>
        /// Check that the species observation
        /// database is not updating right now.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <exception cref="ApplicationException">Thrown if database is beeing updated.</exception>
        public void CheckManualDatabaseUpdate(WebServiceContext context)
        {
            if (context.GetSpeciesObservationDatabase().IsDatabaseUpdated())
            {
                // Wait a minute in order to avoid overload on database server.
                Thread.Sleep(60000);

                throw new ApplicationException("Database is beeing updated!");
            }
        }

        /// <summary>
        /// Check that all species observation fields has been
        /// mapped in Elasticsearch.
        /// </summary>
        /// <param name="context"> Web service request context.</param>
        /// <param name="speciesObservation">Species observation.</param>
        /// <param name="elasticsearch">Proxy to Elasticsearch.</param>
        public void CheckMappingElasticsearch(WebServiceContext context,
                                              WebSpeciesObservation speciesObservation,
                                              ElasticsearchSpeciesObservationProxy elasticsearch)
        {
            Boolean isMappingUpdated;
            Dictionary<String, WebSpeciesObservationField> mapping;
            String cacheKey, fieldName, indexType, newMapping;

            isMappingUpdated = false;
            if (speciesObservation.IsNotNull() &&
                speciesObservation.Fields.IsNotEmpty())
            {
                mapping = GetMapping(context, elasticsearch);
                foreach (WebSpeciesObservationField field in speciesObservation.Fields)
                {
                    fieldName = field.GetFieldName();
                    if (!mapping.ContainsKey(fieldName))
                    {
                        // Add mapping for new field.
                        switch (field.Type)
                        {
                            case WebDataType.Boolean:
                                newMapping = "{\"properties\": {" +
                                             "\"" + fieldName + "\": {\"type\": \"boolean\"}" +
                                             "}}";
                                break;
                            case WebDataType.DateTime:
                                newMapping = "{\"properties\": {" +
                                             "\"" + fieldName + "\": {\"type\": \"date\", \"format\": \"dateOptionalTime\"}," +
                                             "\"" + fieldName + "_DatePartOnly\": {\"type\": \"string\", \"index\": \"not_analyzed\"}," +
                                             "\"" + fieldName + "_DayOfMonth\": {\"type\": \"byte\"}," +
                                             "\"" + fieldName + "_DayOfYear\": {\"type\": \"short\"}," +
                                             "\"" + fieldName + "_MonthOfYear\": {\"type\": \"byte\"}," +
                                             "\"" + fieldName + "_WeekOfYear\": {\"type\": \"byte\"}," +
                                             "\"" + fieldName + "_Year\": {\"type\": \"short\"}," +
                                             "\"" + fieldName + "_YearAndMonth\": {\"type\": \"string\", \"index\": \"not_analyzed\"}," +
                                             "\"" + fieldName + "_YearAndWeek\": {\"type\": \"string\", \"index\": \"not_analyzed\"}" +
                                             "}}";
                                break;
                            case WebDataType.Float64:
                                newMapping = "{\"properties\": {" +
                                             "\"" + fieldName + "\": {\"type\": \"double\"}" +
                                             "}}";
                                break;
                            case WebDataType.Int32:
                                newMapping = "{\"properties\": {" +
                                             "\"" + fieldName + "\": {\"type\": \"integer\"}" +
                                             "}}";
                                break;
                            case WebDataType.Int64:
                                newMapping = "{\"properties\": {" +
                                             "\"" + fieldName + "\": {\"type\": \"long\"}" +
                                             "}}";
                                break;
                            case WebDataType.String:
                                switch (fieldName)
                                {
                                    case "DarwinCore_DatasetName":
                                    case "DarwinCore_Owner":
                                    case "Occurrence_RecordedBy":
                                    case "DarwinCore_ReportedBy":
                                        // Aggregations are done on these fields in AnalysisService.
                                        indexType = "not_analyzed";
                                        break;
                                    default:
                                        indexType = "no";
                                        break;
                                }

                                newMapping = "{\"properties\": {" +
                                             "\"" + fieldName + "\": {\"type\": \"string\", \"index\": \"" + indexType + "\"}," +
                                             "\"" + fieldName + "_Lowercase" + "\": {\"type\": \"string\", \"index\": \"not_analyzed\"}" +
                                             "}}";
                                break;
                            default:
                                throw new Exception("Not handled field data type = " + field.Type);
                        }

                        // ReSharper disable once PossibleNullReferenceException
                        elasticsearch.UpdateSpeciesObservationMapping(newMapping);
                        isMappingUpdated = true;
                    }
                }
            }

            if (isMappingUpdated)
            {
                // Wait a while in order to make sure that
                // Elasticsearch has saved updated mapping.
                Thread.Sleep(6000);

                // Update cached mapping information.
                cacheKey = Settings.Default.SpeciesObservationFieldMappingCacheKey;
                context.RemoveCachedObject(cacheKey);
                GetMapping(context, elasticsearch);
            }
        }

        /// <summary>
        /// Get mapping for species observation fields.
        /// </summary>
        /// <param name="context">Web service request context. </param>
        /// <param name="elasticsearch">Proxy to Elasticsearch.</param>
        /// <returns>Mapping for species observation fields.</returns>
        public Dictionary<String, WebSpeciesObservationField> GetMapping(WebServiceContext context,
                                                                         ElasticsearchSpeciesObservationProxy elasticsearch)
        {
            Dictionary<String, WebSpeciesObservationField> mapping;
            FieldDefinitionList fieldDefinitions;
            String cacheKey;
            String[] splitField;
            WebSpeciesObservationField field;

            // Get data from cache.
            cacheKey = Settings.Default.SpeciesObservationFieldMappingCacheKey;
            mapping = (Dictionary<String, WebSpeciesObservationField>)context.GetCachedObject(cacheKey);

            if (mapping.IsNull())
            {
                // Get data from Elasticsearch.
                mapping = new Dictionary<String, WebSpeciesObservationField>();

                fieldDefinitions = elasticsearch.GetSpeciesObservationMapping();
                if (fieldDefinitions.IsNotEmpty())
                {
                    foreach (FieldDefinition fieldDefinition in fieldDefinitions)
                    {
                        if (1 <= fieldDefinition.Name.IndexOf('_'))
                        {
                            splitField = fieldDefinition.Name.Split('_');
                            field = new WebSpeciesObservationField();
                            field.ClassIdentifier = splitField[0];
                            field.PropertyIdentifier = splitField[1];
                            switch (fieldDefinition.DataType)
                            {
                                case "boolean":
                                    field.Type = WebDataType.Boolean;
                                    break;
                                case "byte":
                                    field.Type = WebDataType.Int32;
                                    break;
                                case "date":
                                    field.Type = WebDataType.DateTime;
                                    break;
                                case "double":
                                    field.Type = WebDataType.Float64;
                                    break;
                                case "integer":
                                    field.Type = WebDataType.Int32;
                                    break;
                                case "long":
                                    field.Type = WebDataType.Int64;
                                    break;
                                case "short":
                                    field.Type = WebDataType.Int32;
                                    break;
                                case "string":
                                    field.Type = WebDataType.String;
                                    break;
                                default:
                                    throw new Exception("Not handled data type = " + fieldDefinition.DataType);
                            }

                            mapping[fieldDefinition.Name] = field;
                            ////Debug.WriteLine(fieldDefinition.Name + " " +
                            ////                field.ClassIdentifier + " " +
                            ////                field.PropertyIdentifier + " " +
                            ////                field.Type);
                        }
                    }
                }

                // Store data in cache.
                context.AddCachedObject(cacheKey,
                                        mapping,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return mapping;
        }

        /// <summary>
        /// Get specified species observation data provider.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationDataProviderId">Species observation data provider id.</param>
        /// <returns>Specified species observation data provider.</returns>
        public virtual WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context,
                                                                                           SpeciesObservationDataProviderId speciesObservationDataProviderId)
        {
            foreach (WebSpeciesObservationDataProvider speciesObservationDataProvider in GetSpeciesObservationDataProviders(context))
            {
                if (speciesObservationDataProvider.Id == (Int32)speciesObservationDataProviderId)
                {
                    return speciesObservationDataProvider;
                }
            }

            return null;
        }

        /// <summary>
        /// Get all species observation data providers.
        /// No cache is used.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All species observation data providers.</returns>
        private List<WebSpeciesObservationDataProvider> GetSpeciesObservationDataProviders(WebServiceContext context)
        {
            List<WebSpeciesObservationDataProvider> dataProviders;
            WebSpeciesObservationDataProvider dataProvider;

            // Data not in cache. Get information from database.
            dataProviders = new List<WebSpeciesObservationDataProvider>();
            using (DataReader dataReader = context.GetDatabase().GetSpeciesObservationDataProviders(context.Locale.Id))
            {
                while (dataReader.Read())
                {
                    dataProvider = new WebSpeciesObservationDataProvider();
                    dataProvider.LoadData(dataReader);
                    dataProviders.Add(dataProvider);
                }
            }

            return dataProviders;
        }

        /// <summary>
        /// Get information related to Elasticsearch and species observations.
        /// </summary>
        /// <param name="context">Web service request context. </param>
        /// <returns>Information related to Elasticsearch and species observations.</returns>
        public SpeciesObservationElasticsearch GetSpeciesObservationElasticsearch(WebServiceContext context)
        {
            SpeciesObservationElasticsearch speciesObservationElasticsearch;

            speciesObservationElasticsearch = new SpeciesObservationElasticsearch();
            using (DataReader dataReader = context.GetSpeciesObservationDatabase().GetSpeciesObservationElasticsearch())
            {
                if (dataReader.Read())
                {
                    speciesObservationElasticsearch.LoadData(dataReader);
                }
                else
                {
                    throw new ApplicationException("Failed to retrieve species observation information related to Elasticsearch.");
                }
            }

            return speciesObservationElasticsearch;
        }

        /// <summary>
        /// Convert species observations from JSON to
        /// instances of class WebSpeciesObservation.
        /// </summary>
        /// <param name="speciesObservationsJson">Species observation in JSON format.</param>
        /// <param name="mapping">Species observation field information mapping.</param>
        /// <returns>Species observations instances of class WebSpeciesObservation.</returns>
        public List<WebSpeciesObservation> GetSpeciesObservations(String speciesObservationsJson,
                                                                  Dictionary<String, WebSpeciesObservationField> mapping)
        {
            Int32 startIndex;
            List<WebSpeciesObservation> speciesObservations;

            startIndex = 0;
            speciesObservations = new List<WebSpeciesObservation>();

            while (startIndex < (speciesObservationsJson.Length - 10))
            {
                startIndex = GetSpeciesObservation(speciesObservations,
                                                   speciesObservationsJson,
                                                   mapping,
                                                   startIndex);
            }

            return speciesObservations;
        }

        /// <summary>
        /// Convert one species observation from JSON to
        /// class WebSpeciesObservation.
        /// </summary>
        /// <param name="speciesObservations">Species observation is stored in this list.</param>
        /// <param name="speciesObservationsJson">Species observation in JSON format.</param>
        /// <param name="mapping">Species observation field information mapping.</param>
        /// <param name="startIndex">Current position in the species observation JSON string.</param>
        /// <returns>Updated current position in the species observation JSON string.</returns>
        private Int32 GetSpeciesObservation(List<WebSpeciesObservation> speciesObservations,
                                            String speciesObservationsJson,
                                            Dictionary<String, WebSpeciesObservationField> mapping,
                                            Int32 startIndex)
        {
            WebSpeciesObservation speciesObservation;
            WebSpeciesObservationField field;

            // Skip general part.
            startIndex = speciesObservationsJson.IndexOf("_source", startIndex, StringComparison.Ordinal);
            if (startIndex < 0)
            {
                // No species observations in input data.
                return speciesObservationsJson.Length;
            }

            startIndex = speciesObservationsJson.IndexOf("{", startIndex, StringComparison.Ordinal) + 1;

            speciesObservation = new WebSpeciesObservation();
            speciesObservation.Fields = new List<WebSpeciesObservationField>();
            speciesObservations.Add(speciesObservation);

            do
            {
                startIndex = GetSpeciesObservationField(out field,
                                                        speciesObservationsJson,
                                                        mapping,
                                                        startIndex);
                if (field.IsNotNull())
                {
                    speciesObservation.Fields.Add(field);
                }
            }
            while (field.IsNotNull());

            return startIndex;
        }

        /// <summary>
        /// Convert one species observation field from JSON to
        /// class WebSpeciesObservationField.
        /// </summary>
        /// <param name="speciesObservationField">Field is returned in this parameter.</param>
        /// <param name="speciesObservationsJson">Species observation field in JSON format.</param>
        /// <param name="mapping">Species observation field information mapping.</param>
        /// <param name="startIndex">Current position in the species observation JSON string.</param>
        /// <returns>Updated current position in the species observation JSON string.</returns>
        private Int32 GetSpeciesObservationField(out WebSpeciesObservationField speciesObservationField,
                                                 String speciesObservationsJson,
                                                 Dictionary<String, WebSpeciesObservationField> mapping,
                                                 Int32 startIndex)
        {
            Int32 stopIndex;
            String fieldKey, value;
            String[] splitKey;
            WebSpeciesObservationField mappingField;

            // Get field key.
            if (speciesObservationsJson[startIndex] != '}')
            {
                stopIndex = speciesObservationsJson.IndexOf(':', startIndex);
                fieldKey = speciesObservationsJson.Substring(startIndex + 1, stopIndex - startIndex - 2);
                startIndex = stopIndex + 1;
                if (mapping.ContainsKey(fieldKey))
                {
                    mappingField = mapping[fieldKey];
                    splitKey = fieldKey.Split('_');
                    if (splitKey.Length > 2)
                    {
                        // Get field in order to advance startIndex to next position.
                        startIndex = GetSpeciesObservationFieldValue(out value,
                                                                     mappingField.Type,
                                                                     speciesObservationsJson,
                                                                     startIndex);

                        // This field should not be returned.
                        // Return next field instead.
                        return GetSpeciesObservationField(out speciesObservationField,
                                                          speciesObservationsJson,
                                                          mapping,
                                                          startIndex);
                    }
                    else
                    {
                        // Create field instance.
                        speciesObservationField = new WebSpeciesObservationField();
                        speciesObservationField.ClassIdentifier = mappingField.ClassIdentifier;
                        speciesObservationField.PropertyIdentifier = mappingField.PropertyIdentifier;
                        speciesObservationField.Type = mappingField.Type;
                        startIndex = GetSpeciesObservationFieldValue(out value,
                                                                     speciesObservationField.Type,
                                                                     speciesObservationsJson,
                                                                     startIndex);
                        speciesObservationField.Value = value;
                    }
                }
                else
                {
                    if (fieldKey == "\"sort")
                    {
                        // No more fields in current species observation.
                        // Read to next species observation.
                        stopIndex = speciesObservationsJson.IndexOf('}', startIndex);
                        startIndex = stopIndex + 1;
                        speciesObservationField = null;
                    }
                    else
                    {
                        throw new Exception("Unknown field name = " + fieldKey);
                    }
                }
            }
            else
            {
                // No more fields in current species observation.
                // Read to next species observation.
                startIndex += 2;
                speciesObservationField = null;
            }

            return startIndex;
        }

        /// <summary>
        /// Convert one species observation field value from JSON to
        /// class WebSpeciesObservationField.
        /// </summary>
        /// <param name="value">Field value is returned in this parameter.</param>
        /// <param name="type">Field value is of this type.</param>
        /// <param name="speciesObservationsJson">Species observation field in JSON format.</param>
        /// <param name="startIndex">Current position in the species observation JSON string.</param>
        /// <returns>Updated current position in the species observation JSON string.</returns>
        private Int32 GetSpeciesObservationFieldValue(out String value,
                                                      WebDataType type,
                                                      String speciesObservationsJson,
                                                      Int32 startIndex)
        {
            Boolean stringEndFound;
            Int32 stopIndex;

            switch (type)
            {
                case WebDataType.Boolean:
                    switch (speciesObservationsJson.Substring(startIndex, 4))
                    {
                        case "fals":
                            value = Boolean.FalseString;
                            startIndex += 6;
                            break;
                        case "true":
                            value = Boolean.TrueString;
                            startIndex += 5;
                            break;
                        default:
                            throw new Exception("Not handled boolean data type value = " + speciesObservationsJson.Substring(startIndex, 4));
                    }

                    break;

                case WebDataType.DateTime:
                    stopIndex = speciesObservationsJson.IndexOf('\"', startIndex + 1);
                    value = speciesObservationsJson.Substring(startIndex + 1, stopIndex - startIndex - 1);
                    startIndex = stopIndex + 2;
                    break;

                case WebDataType.Float64:
                    stopIndex = speciesObservationsJson.IndexOfAny(new[] { ',', '}' }, startIndex);
                    value = speciesObservationsJson.Substring(startIndex, stopIndex - startIndex);

                    // Convert float value from Elasticsearch
                    // format to SOAP web service format.
                    value = value.WebParseDouble().WebToString();
                    startIndex = stopIndex + 1;
                    break;

                case WebDataType.Int32:
                case WebDataType.Int64:
                    stopIndex = speciesObservationsJson.IndexOfAny(new[] { ',', '}' }, startIndex);
                    value = speciesObservationsJson.Substring(startIndex, stopIndex - startIndex);
                    startIndex = stopIndex + 1;
                    break;

                case WebDataType.String:
                    stopIndex = startIndex + 1;
                    stringEndFound = false;
                    while (!stringEndFound)
                    {
                        stringEndFound = (speciesObservationsJson[stopIndex] == '"') &&
                                         (speciesObservationsJson[stopIndex - 1] != '\\');
                        if (!stringEndFound)
                        {
                            stopIndex++;
                        }
                    }

                    value = speciesObservationsJson.Substring(startIndex + 1, stopIndex - startIndex - 1);
                    startIndex = stopIndex + 2;

                    // Remove escape of special characters.
                    value = value.Replace("\\\"", "\"");
                    value = value.Replace("\\\\", "\\");
                    break;

                default:
                    throw new Exception("Not handled data type = " + type);
            }

            return startIndex;
        }

        /// <summary>
        /// Update information related to Elasticsearch and species observations.
        /// </summary>
        /// <param name="context">Web service request context. </param>
        /// <param name="currentIndexAlias">Alias used to retrieve species observation information in Elasticsearch.</param>
        /// <param name="currentIndexChangeId">Max species observation change id that has been processed to Elasticsearch.</param>
        /// <param name="currentIndexCount">Number of species observations in current index in Elasticsearch.</param>
        /// <param name="currentIndexName">Name of current index in Elasticsearch.</param>
        /// <param name="nextIndexChangeId">
        /// Max species observation change id that has been
        /// processed into next index in Elasticsearch.
        /// </param>
        /// <param name="nextIndexCount">Number of species observations in next index in Elasticsearch.</param>
        /// <param name="nextIndexName">Name of next index in Elasticsearch.</param>
        public void UpdateSpeciesObservationElasticsearch(WebServiceContext context,
                                                          Int64? currentIndexChangeId,
                                                          Int64? currentIndexCount,
                                                          String currentIndexName,
                                                          Int64? nextIndexChangeId,
                                                          Int64? nextIndexCount,
                                                          String nextIndexName)
        {
            context.GetSpeciesObservationDatabase().UpdateSpeciesObservationElasticsearch(currentIndexChangeId,
                                                                                          currentIndexCount,
                                                                                          currentIndexName,
                                                                                          nextIndexChangeId,
                                                                                          nextIndexCount,
                                                                                          nextIndexName);
        }



        /// <summary>
        /// Get number of species observations that matches
        /// provided species observation search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>
        /// Number of species observations that matches
        /// provided species observation search criteria.
        /// </returns>
        public Int64 GetSpeciesObservationCountBySearchCriteriaElasticsearch(WebServiceContext context,
                                                                                    WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                    WebCoordinateSystem coordinateSystem)
        {
            DocumentCountResponse speciesObservationCountResponse;
            StringBuilder filter;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid.
            CheckData(context, searchCriteria, coordinateSystem);

            // Get species observation filter.
            filter = new StringBuilder();
            filter.Append("{");
            filter.Append(searchCriteria.GetFilter(context, false));
            filter.Append("}");

            // Get species observation count.
            using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
            {
                speciesObservationCountResponse = elastisearch.GetSpeciesObservationCount(filter.ToString());
            }

            return speciesObservationCountResponse.DocumentCount;
        }

        /// <summary>
        /// Check that species observation search criteria is valid.
        /// And also changes the searchCriteria by converting coordinates and adding taxonid's
        /// This method should only be used together with Elasticsearch.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in geometry search criteria
        /// and returned species observations.
        /// </param>
        public void CheckData(WebServiceContext context,
                                      WebSpeciesObservationSearchCriteria searchCriteria,
                                      WebCoordinateSystem coordinateSystem)
        {
            Dictionary<String, WebSpeciesObservationField> mapping;

            coordinateSystem.CheckData();
            searchCriteria.CheckNotNull("searchCriteria");
            using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
            {
                mapping = GetMapping(context, elastisearch);
            }

            searchCriteria.CheckData(context, true, mapping);
            searchCriteria.Polygons = ConvertToElasticSearchCoordinates(context,
                                                                        searchCriteria.Polygons,
                                                                        searchCriteria.RegionGuids,
                                                                        coordinateSystem);
            searchCriteria.TaxonIds = WebSpeciesObservationServiceData.TaxonManager.GetTaxonIds(context, searchCriteria, true);
        }


        /// <summary>
        /// Convert polygons from provided coordinate
        /// system to a Elasticsearch coordinate system.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="inputPolygons">Input polygons.</param>
        /// <param name="regionGuids">Region GUIDs.</param>
        /// <param name="inputCoordinateSystem">Which coordinate system the coordinates should be converted from.</param>
        /// <returns>Polygons in Elasticsearch coordinate system.</returns>
        public List<WebPolygon> ConvertToElasticSearchCoordinates(WebServiceContext context,
                                                                          List<WebPolygon> inputPolygons,
                                                                          List<String> regionGuids,
                                                                          WebCoordinateSystem inputCoordinateSystem)
        {
            List<WebPolygon> outputPolygons;
            List<WebRegionGeography> regionsGeography;
            WebCoordinateSystem speciesObservationCoordinateSystem;

            outputPolygons = null;
            speciesObservationCoordinateSystem = new WebCoordinateSystem();
            speciesObservationCoordinateSystem.Id = CoordinateSystemId.WGS84;
            if (inputPolygons.IsNotEmpty())
            {
                if (inputCoordinateSystem.GetWkt().ToLower() == speciesObservationCoordinateSystem.GetWkt().ToLower())
                {
                    outputPolygons = inputPolygons;
                }
                else
                {
                    outputPolygons = WebServiceData.CoordinateConversionManager.GetConvertedPolygons(inputPolygons,
                                                                                                     inputCoordinateSystem,
                                                                                                     speciesObservationCoordinateSystem);
                }
            }

            if (regionGuids.IsNotEmpty())
            {
                regionsGeography = WebServiceData.RegionManager.GetRegionsGeographyByGuids(context,
                                                                                           regionGuids,
                                                                                           speciesObservationCoordinateSystem);
                if (outputPolygons.IsNull())
                {
                    outputPolygons = new List<WebPolygon>();
                }

                foreach (WebRegionGeography regionGeography in regionsGeography)
                {
                    // ReSharper disable once PossibleNullReferenceException
                    outputPolygons.AddRange(regionGeography.MultiPolygon.Polygons);
                }
            }

            return outputPolygons;
        }
    }
}

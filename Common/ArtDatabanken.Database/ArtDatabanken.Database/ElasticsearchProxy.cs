using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using ArtDatabanken.Data;
using ArtDatabanken.Database.ElasticsearchSerializingClasses;
using Elasticsearch.Net;

namespace ArtDatabanken.Database
{
    /// <summary>
    /// Class that works as proxy for Elasticsearch.
    /// </summary>
    public class ElasticsearchProxy : IDisposable
    {
        private const Int32 MaxClientCount = 10;

        /// <summary>
        /// The _clients.
        /// </summary>
        private static List<ElasticLowLevelClient> _clients;

        /// <summary>
        /// The real Elasticsearch client.
        /// </summary>
        private ElasticLowLevelClient _client;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static ElasticsearchProxy()
        {
            _clients = new List<ElasticLowLevelClient>();
        }

        /// <summary>
        /// Create an ElasticsearchProxy instance.
        /// </summary>
        protected ElasticsearchProxy()
        {
            _client = PopClient();
        }

        /// <summary>
        /// Name of used index.
        /// This name may be an alias.
        /// </summary>
        public String IndexName { get; set; }

        /// <summary>
        /// Perform an operation with the specified data.
        /// </summary>
        /// <param name="bulkDataJson">Bulk data in JSON format.</param>
        protected void Bulk(String bulkDataJson)
        {
            Boolean succeeded;
            ElasticsearchResponse<DynamicResponse> response;

            succeeded = false;
            while (!succeeded)
            {
                try
                {
                    response = _client.Bulk<DynamicResponse>(bulkDataJson);
                    CheckResponse(response);
                    succeeded = true;
                }
                catch (Exception)
                {
                    //if (Configuration.Debug)
                    //{
                    //    throw;
                    //}
                    // Maybe Elasticsearch is to busy. Try again after a minute.
                    Thread.Sleep(60000);
                    succeeded = false;
                }
            }
        }

        /// <summary>
        /// Check that a call to Elasticsearch was successfull.
        /// </summary>
        /// <param name="response">Response from call to Elasticsearch.</param>
        /// <exception cref="Exception">Thrown if call to Elasticsearch was not successfull.</exception>
        private void CheckResponse<T>(ElasticsearchResponse<T> response)
        {
            if (response.HttpStatusCode != 200)
            {
                throw new Exception("Call to Elasticsearch failed." +
                                    " Response: " + response.ToString().Substring(0, Math.Min(response.ToString().Length, 800)));
            }
        }

        /// <summary>
        /// Create an Elasticsearch low level client instance.
        /// </summary>
        /// <returns>An Elasticsearch low level client instance.</returns>
        private static ElasticLowLevelClient CreateClient()
        {
            ConnectionConfiguration config;
            ElasticLowLevelClient client;
            SniffingConnectionPool connectionPool;
            Uri node;

            client = null;
            switch (Configuration.InstallationType)
            {
                case InstallationType.Production:
                    if ((_clients.Count % 2) == 0)
                    {
                        node = new Uri(@"http://bombus2-1.artdatadb.slu.se:8080/");
                    }
                    else
                    {
                        node = new Uri(@"http://bombus2-2.artdatadb.slu.se:8080/");
                    }

                    connectionPool = new SniffingConnectionPool(new[] { node });
                    config = new ConnectionConfiguration(connectionPool);
                    config.RequestTimeout(new TimeSpan(0, 5, 0));
                    config.PingTimeout(new TimeSpan(0, 0, 20));
                    client = new ElasticLowLevelClient(config);
                    break;

                case InstallationType.TwoBlueberriesTest:
                node = new Uri(@"http://bombus-dev.artdatadb.slu.se:8080/");
                connectionPool = new SniffingConnectionPool(new[] { node });
                config = new ConnectionConfiguration(connectionPool);
                config.RequestTimeout(new TimeSpan(0, 5, 0));
                config.PingTimeout(new TimeSpan(0, 0, 20));
                client = new ElasticLowLevelClient(config);
                break;

                case InstallationType.LocalTest:
                    client = new ElasticLowLevelClient();
                    break;
            }

            return client;
        }

        /// <summary>
        /// Create index in elasticsearch.
        /// </summary>
        /// <param name="indexName">Index name.</param>
        protected void CreateIndex(String indexName)
        {
            ElasticsearchResponse<DynamicResponse> response;
            //String locale, settings;

            //            locale = "{\"index\" : { \"analysis\" : { \"analyzer\" : {" + 
            //                     "\"collation\" : { \"tokenizer\" : \"icu_tokenizer\", " +
            //                     "\"filter\" : [\"SwedishCollator\"] }}, " +
            //                     "\"filter\" : { \"SwedishCollator\" : { " +
            //                     "\"type\" : \"lowercase\", \"country\" : \"SE\", \"language\" : \"sv\" }}}}}";
            ////            "\"type\" : \"icu_collation\", \"country\" : \"SE\", \"language\" : \"sv\" }}}}}";
            //            settings = "{\"settings\" : " +
            //                            "{\"index\" : {\"analysis\" : " +
            //                                "{\"filter\" : {\"SwedishCollation\": {\"type\" : \"icu_collation\", \"country\" : \"SE\", \"language\" : \"sv\"}}," +
            //                                " \"analyzer\" : {\"SwedishAnalyzer\" : {\"type\" : \"custom\", \"tokenizer\" : \"keyword\", \"filter\" : \"SwedishCollation\"}}}}}, " +
            //                       "\"mappings\" : {\"_default_\": {\"properties\" : {\"name\" : {\"type\" : \"string\", \"analyzer\" : \"SwedishAnalyzer\"}}}}}";
            response = _client.IndicesCreate<DynamicResponse>(indexName, null);
            CheckResponse(response);
            //        response = mClient.IndicesPutSettings(indexName, locale);
            //response = mClient.IndicesPutAlias(indexName, IndexName, null);
            //CheckResponse(response);
        }

        /// <summary>
        /// Delete specified document from Elasticsearch.
        /// It is ok to try to delete a document that is not in the index.
        /// </summary>
        /// <param name="type">Type name.</param>
        /// <param name="documentId">Type name.</param>
        protected void DeleteDocument(String type, String documentId)
        {
            Boolean succeeded;
            ElasticsearchResponse<DynamicResponse> response;

            succeeded = false;
            while (!succeeded)
            {
                try
                {
                    response = _client.Delete<DynamicResponse>(IndexName, type, documentId);
                    if (response.HttpStatusCode == 404)
                    {
                        succeeded = true;
                    }
                    else
                    {
                        CheckResponse(response);
                        succeeded = true;
                    }
                }
                catch (Exception)
                {
                    // Maybe Elasticsearch is to busy. Try again after a minute.
                    Thread.Sleep(60000);
                    succeeded = false;
                }
            }
        }

        /// <summary>
        /// Delete index in elasticsearch.
        /// </summary>
        /// <param name="indexName">Index name.</param>
        protected void DeleteIndex(String indexName)
        {
            ElasticsearchResponse<DynamicResponse> response;

            response = _client.IndicesDelete<DynamicResponse>(indexName, null);
            CheckResponse(response);
        }

        /// <summary>
        /// Delete type in Elasticsearch.
        /// </summary>
        /// <param name="type">Type name.</param>
        public void DeleteType(String type)
        {
            ElasticsearchResponse<DynamicResponse> response;

            response = _client.DeleteByQuery<DynamicResponse>(IndexName, type);
            CheckResponse(response);
        }

        /// <summary>
        /// Implementation of the IDisposable interface.
        /// Recycle the client instance.
        /// </summary>
        public void Dispose()
        {
            if (_client.IsNotNull())
            {
                PushClient(_client);
            }

            _client = null;
        }

        ///// <summary>
        ///// Get number of items that matches filter.
        ///// </summary>
        ///// <param name="type">Type name.</param>
        ///// <param name="filter">Filter for object of specified type.</param>
        ///// <returns>Number of items that matches filter.</returns>
        //protected DocumentCountResponse GetCount(String type,
        //                                         String filter)
        //{
        //    DocumentCountResponse documentCountResponse;
        //    ElasticsearchResponse<DynamicDictionary> response;
        //    String shardInformation;
        //    String[] splitShardInformation;

        //    response = mClient.Count(IndexName, type, filter);

        //    CheckResponse(response);
        //    documentCountResponse = new DocumentCountResponse();
        //    if (response.Response.IsNotNull())
        //    {
        //        documentCountResponse.DocumentCount = (Int32)(response.Response.Values.ElementAt(0));

        //        // Get shard information.
        //        shardInformation = (String)(response.Response.Values.ElementAt(1));
        //        splitShardInformation = shardInformation.Split(':');
        //        documentCountResponse.ShardTotalCount = splitShardInformation[1].Substring(0, splitShardInformation[1].IndexOf(',')).WebParseInt32();
        //        documentCountResponse.ShardSuccessfulCount = splitShardInformation[2].Substring(0, splitShardInformation[2].IndexOf(',')).WebParseInt32();
        //        documentCountResponse.ShardFailedCount = splitShardInformation[3].Substring(0, splitShardInformation[3].IndexOf('}')).WebParseInt32();
        //    }

        //    return documentCountResponse;
        //}

        /// <summary>
        ///  Returns a dictionary with a 'flattened' key from a termcount tree structure, the value is the last bucket node's doc_count (works in a recursive fashion)
        ///  All keys are concatenated with the divider (like "key1:key2:key3" given that the divider is a colon)
        /// </summary>
        /// <param name="termcount"></param>
        /// <param name="divider"></param>
        /// <param name="parentKey">This value should be null on first call (parameter used during recursion)</param>
        /// <returns></returns>
        private static Dictionary<string, long> Flatten(ElasticsearchTermCount termcount,
                                                        string divider,
                                                        string parentKey)
        {
            var list = new Dictionary<string, long>();

            foreach (var bucket in termcount.buckets)
            {
                var key = string.Format("{0}{1}", parentKey ?? string.Empty, bucket.key);
                if (bucket.term_count == null)
                {
                    list.Add(key, bucket.doc_count);
                }
                else
                {
                    foreach (var innerItem in Flatten(bucket.term_count, divider, string.Format("{0}{1}{2}", parentKey ?? string.Empty, bucket.key, divider)))
                    {
                        list.Add(innerItem.Key, innerItem.Value);
                    }
                }
            }
            return list;
        }
        private static List<DocumentUniqueValue> FlattenCountAndSum(ElasticsearchTermCount termcount,
                                                 string divider,
                                                 string parentKey)
        {
            var list = new List<DocumentUniqueValue>();

            foreach (var bucket in termcount.buckets)
            {
                var key = string.Format("{0}{1}", parentKey ?? string.Empty, bucket.key);
                if (bucket.term_count == null || bucket.term_count.buckets.Count == 0 || bucket.term_count.buckets[0].term_count == null)
                {
                    //If 'depth' (number of terms) = 1 (i.e. when bucket.term_count == null) then the unique count is set to 1
                    list.Add(new DocumentUniqueValue { Key = key, Count = bucket.term_count == null ? 1 : bucket.term_count.buckets.Count, DocumentCount = bucket.doc_count });
                }
                else
                {
                    foreach (var innerItem in FlattenCountAndSum(bucket.term_count, divider, string.Format("{0}{1}{2}", parentKey ?? string.Empty, bucket.key, divider)))
                    {
                        list.Add(innerItem);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Returns a 'aggregate string' that is used to have Elasticsearch return a result that's grouped (and sorted) by the provided terms (works in a recursive fashion)
        /// </summary>
        /// <param name="terms"></param>
        /// <returns></returns>
        private static string GetAggregateString(List<ElasticsearchTerm> terms)
        {
            var term = terms.First();
            var type = term.TermType == ElasticsearchTermType.Script ? "script" : "field";
            var sortType = term.SortType == ElasticsearchSortType.DocumentCount ? "_count" : "_term";
            var sortOrder = term.SortOrder == ElasticsearchSortOrder.Descending ? "desc" : "asc";
            var order = term.SortOrder == ElasticsearchSortOrder.None ? "" : string.Format("\"order\" : {{\"{0}\" : \"{1}\"}}", sortType, sortOrder);
            var top = term.Top * 5;//Elastic aggregations gives approximate document counts when the number of shards > 1 - therefore the number of queried rows are multiplied with 5 to mitigate the risk for returning the wrong set of rows

            if (terms.Count > 1)
            {
                terms.Remove(term);
                return string.Format("\"aggs\" : {{\"term_count\" : {{ \"terms\" : {{ \"{0}\" : \"{1}\", \"size\" : {2}, {3}}}, {4}}}}}", type, term.Term, top, order, GetAggregateString(terms));
            }
            return string.Format("\"aggs\" : {{\"term_count\" : {{ \"terms\" : {{ \"{0}\" : \"{1}\", \"size\" : {2}, {3}}}}}}}", type, term.Term, top, order);

            //if (terms.Count > 1)
            //{
            //    terms.Remove(term);
            //    return string.Format("\"aggs\" : {{\"term_count\" : {{ \"terms\" : {{ \"{0}\" : \"{1}\", {2}}}, {3}}}}}", type, term.Term, order, GetAggregateString(terms));
            //}
            //return string.Format("\"aggs\" : {{\"term_count\" : {{ \"terms\" : {{ \"{0}\" : \"{1}\", {2}}}}}}}", type, term.Term, order);
        }


        /// <summary>
        /// Returns a complete 'query' to be used when posting an aggregate query to Elasticsearch
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private static string GetCompleteQueryString(List<ElasticsearchTerm> terms,
                                                     string filter)
        {
            var aggregate = GetAggregateString(terms);

            return string.IsNullOrEmpty(filter)
                ? "{ " + aggregate + "}"
                : filter.Substring(0, filter.Length - 1) + ", " + aggregate + "}";
            //return string.IsNullOrEmpty(filter)
            //    ? "{ " + aggregate + "}"
            //    : filter.Substring(0, filter.Length - 3) + ", " + aggregate + "}}}";
        }

        /// <summary>
        /// Get number of items that matches filter.
        /// </summary>
        /// <param name="type">Type name.</param>
        /// <param name="filter">Filter for object of specified type.</param>
        /// <returns>Number of items that matches filter.</returns>
        protected DocumentCountResponse GetCount(string type,
                                                 string filter)
        {
            var documentCountResponse = new DocumentCountResponse();

            var response = _client.Count<ElasticsearchCountResponse>(IndexName, type, filter);

            CheckResponse(response);

            if (response.Body.IsNotNull())
            {
                documentCountResponse.ShardTotalCount = response.Body._shards.total;
                documentCountResponse.ShardSuccessfulCount = response.Body._shards.successful;
                documentCountResponse.ShardFailedCount = response.Body._shards.failed;
                documentCountResponse.DocumentCount = response.Body.count;
            }

            return documentCountResponse;
        }

        /// <summary>
        /// Get documents that matches filter.
        /// </summary>
        /// <param name="type">Type name.</param>
        /// <param name="filter">Filter for species observations.</param>
        /// <returns>Documents that matches filter.</returns>
        protected DocumentFilterResponse GetDocuments(String type, String filter)
        {
            ElasticsearchResponse<DynamicResponse> response;
            Int32 startIndex, stopIndex;
            DocumentFilterResponse documentFilterResponse;
            String shardInformation, documentCount;
            String[] splitShardInformation;

            response = _client.Search<DynamicResponse>(IndexName, type, filter, qs => qs.AddQueryString("filter_path", "took,timed_out,_shards,hits.total,hits.hits._source"));
            CheckResponse(response);
            documentFilterResponse = new DocumentFilterResponse();
            if (response.Body.IsNotNull())
            {
                // Get shard information.
                shardInformation = (String)(response.Body.Values.ElementAt(2));
                splitShardInformation = shardInformation.Split(':');
                documentFilterResponse.ShardTotalCount = splitShardInformation[1].Substring(0, splitShardInformation[1].IndexOf(',')).WebParseInt32();
                documentFilterResponse.ShardSuccessfulCount = splitShardInformation[2].Substring(0, splitShardInformation[2].IndexOf(',')).WebParseInt32();
                documentFilterResponse.ShardFailedCount = splitShardInformation[3].Substring(0, splitShardInformation[3].IndexOf('}')).WebParseInt32();

                documentFilterResponse.DocumentCount = 0;
                documentFilterResponse.DocumentsJson = (String)(response.Body.Values.ElementAt(3));
                documentFilterResponse.TimedOut = (Boolean)(response.Body.Values.ElementAt(1));
                if (!documentFilterResponse.TimedOut)
                {
                    // Get species observation count.
                    startIndex = documentFilterResponse.DocumentsJson.IndexOf(':') + 1;
                    stopIndex = documentFilterResponse.DocumentsJson.IndexOf(',');
                    if (stopIndex < 0)
                    {
                        // There are no character ',' in the response when 0 documents matched.
                        stopIndex = documentFilterResponse.DocumentsJson.IndexOf('}');
                    }

                    documentCount = documentFilterResponse.DocumentsJson.Substring(startIndex, stopIndex - startIndex);
                    documentFilterResponse.DocumentCount = documentCount.WebParseInt64();
                }

                //Debug.WriteLine("Total document count = " +
                //                documentFilterResponse.DocumentCount +
                //                ", time = " + documentFilterResponse.ElapsedTime + "ms" +
                //                ", timed out = " + documentFilterResponse.TimedOut + ".");
            }

            return documentFilterResponse;
        }

        /// <summary>
        /// Get all index alias.
        /// </summary>
        /// <returns>All index alias.</returns>
        public ElasticsearchHealth GetHealth()
        {
            ElasticsearchResponse<ElasticsearchHealth> response;

            response = _client.ClusterHealth<ElasticsearchHealth>();
            CheckResponse(response);
            return response.Body;
        }

        /// <summary>
        /// Get all index alias.
        /// </summary>
        /// <returns>All index alias.</returns>
        public Dictionary<String, String> GetIndexAliases()
        {
            Dictionary<String, String> aliases;
            ElasticsearchResponse<DynamicResponse> response;
            Int32 index;
            String indexAliases, indexName;

            response = _client.IndicesGetAliasesForAll<DynamicResponse>();
            CheckResponse(response);
            aliases = new Dictionary<String, String>();
            if (response.Body.Keys.IsNotNull() && (response.Body.Keys.Count > 0))
            {
                for (index = 0; index < response.Body.Keys.Count; index++)
                {
                    indexName = response.Body.Keys.ElementAt(index);
                    indexAliases = response.Body.Values.ElementAt(index);
                    Debug.WriteLine("Index name = " +
                                    indexName +
                                    ", aliases = " + indexAliases);
                    aliases[indexName] = indexAliases;
                }
            }

            return aliases;
        }

        /// <summary>
        /// Get mapping.
        /// </summary>
        /// <param name="type">Type name.</param>
        /// <returns>Current mapping.</returns>
        protected FieldDefinitionList GetMapping(String type)
        {
            ElasticsearchResponse<DynamicResponse> response;
            FieldDefinition fieldDefinition;
            FieldDefinitionList fieldDefinitions;
            String fieldName, mappingJson;
            String[] splitDataType, fieldProperties, splitFields;

            fieldDefinitions = null;
            response = _client.IndicesGetMapping<DynamicResponse>(IndexName, type);
            CheckResponse(response);
            if (response.Body.IsNotNull() &&
                (response.Body.Count > 0))
            {
                fieldDefinitions = new FieldDefinitionList();
                mappingJson = (String)(response.Body.Values.ElementAt(0));
                mappingJson = mappingJson.Substring(mappingJson.IndexOf('{') + 1);
                mappingJson = mappingJson.Substring(mappingJson.IndexOf('{') + 1);
                mappingJson = mappingJson.Substring(mappingJson.IndexOf('{') + 1);
                mappingJson = mappingJson.Substring(mappingJson.IndexOf("properties") + 1);
                mappingJson = mappingJson.Substring(mappingJson.IndexOf('{') + 1);

                //// If Elasticsearch 2.1 is used run next line.
                //mappingJson = mappingJson.Substring(mappingJson.IndexOf('{') + 1);

                splitFields = mappingJson.Split('}');

                foreach (String fieldMappingJson in splitFields)
                {
                    if (fieldMappingJson.IsNotEmpty() &&
                        (2 <= fieldMappingJson.IndexOf(':')))
                    {
                        fieldName = fieldMappingJson.Substring(1, fieldMappingJson.IndexOf(':') - 2);
                        if (fieldName[0] == '"')
                        {
                            fieldName = fieldName.Substring(1);
                        }

                        fieldDefinition = new FieldDefinition();
                        fieldDefinition.Index = IndexName;
                        fieldDefinition.Json = fieldMappingJson + "}";
                        if (fieldDefinition.Json[0] == ',')
                        {
                            fieldDefinition.Json = fieldDefinition.Json.Substring(1);
                        }

                        fieldDefinition.Name = fieldName;
                        fieldDefinition.Type = type;
                        fieldProperties = fieldMappingJson.Substring(fieldMappingJson.IndexOf('{') + 1).Split(',');
                        foreach (String fieldProperty in fieldProperties)
                        {
                            splitDataType = fieldProperty.Split(':');
                            splitDataType[0] = splitDataType[0].Substring(1, splitDataType[0].Length - 2);
                            switch (splitDataType[0])
                            {
                                case "format":
                                    fieldDefinition.Format = splitDataType[1].Substring(1, splitDataType[1].Length - 2);
                                    break;
                                case "index":
                                    fieldDefinition.FieldIndex = splitDataType[1].Substring(1, splitDataType[1].Length - 2);
                                    break;
                                case "tree_levels":
                                    fieldDefinition.TreeLevel = splitDataType[1].WebParseInt32();
                                    break;
                                case "type":
                                    fieldDefinition.DataType = splitDataType[1].Substring(1, splitDataType[1].Length - 2);
                                    break;
                            }
                        }

                        fieldDefinitions.Add(fieldDefinition);
                    }
                }
            }

            return fieldDefinitions;
        }

        /// <summary>
        /// Get documents from a started scroll.
        /// </summary>
        /// <param name="scroll">Information about the scroll.</param>
        public DocumentFilterResponse GetScroll(ElasticsearchScroll scroll)
        {
            DocumentFilterResponse documentFilterResponse;
            ElasticsearchResponse<DynamicResponse> response;
            StringBuilder scrollInformation;
            Int32 startIndex, stopIndex;
            String maxScore, shardInformation, documentCount;
            String[] splitShardInformation;

            scrollInformation = new StringBuilder();
            scrollInformation.Append("{");
            scrollInformation.Append("\"scroll\" : \"" + scroll.KeepAlive + "m\"");
            scrollInformation.Append(", \"scroll_id\" : \"" + scroll.ScrollId + "\"");
            scrollInformation.Append("}");
            response = _client.Scroll<DynamicResponse>(scrollInformation.ToString());
            CheckResponse(response);
            documentFilterResponse = new DocumentFilterResponse();
            scroll.ScrollId = (String)(response.Body.Values.ElementAt(0));
            if (response.Body.IsNotNull())
            {
                documentFilterResponse.ElapsedTime = (Int32)(response.Body.Values.ElementAt(1));

                // Get shard information.
                shardInformation = (String)(response.Body.Values.ElementAt(4));
                splitShardInformation = shardInformation.Split(':');
                documentFilterResponse.ShardTotalCount = splitShardInformation[1].Substring(0, splitShardInformation[1].IndexOf(',')).WebParseInt32();
                documentFilterResponse.ShardSuccessfulCount = splitShardInformation[2].Substring(0, splitShardInformation[2].IndexOf(',')).WebParseInt32();
                if (documentFilterResponse.ShardSuccessfulCount == documentFilterResponse.ShardTotalCount)
                {
                    documentFilterResponse.ShardFailedCount = splitShardInformation[3].Substring(0, splitShardInformation[3].IndexOf('}')).WebParseInt32();
                }
                else
                {
                    documentFilterResponse.ShardFailedCount = splitShardInformation[3].Substring(0, splitShardInformation[3].IndexOf(',')).WebParseInt32();
                }

                documentFilterResponse.DocumentCount = 0;
                documentFilterResponse.DocumentsJson = (String)(response.Body.Values.ElementAt(5));
                documentFilterResponse.TimedOut = (Boolean)(response.Body.Values.ElementAt(2));
                if (!documentFilterResponse.TimedOut)
                {
                    // Get species observation count.
                    startIndex = documentFilterResponse.DocumentsJson.IndexOf(':') + 1;
                    stopIndex = documentFilterResponse.DocumentsJson.IndexOf(',');
                    documentCount = documentFilterResponse.DocumentsJson.Substring(startIndex, stopIndex - startIndex);
                    documentFilterResponse.DocumentCount = documentCount.WebParseInt64();
                    startIndex = stopIndex + 1;

                    if (documentFilterResponse.DocumentCount > 0)
                    {
                        // Get max score
                        startIndex = documentFilterResponse.DocumentsJson.IndexOf(':', startIndex) + 1;
                        stopIndex = documentFilterResponse.DocumentsJson.IndexOf(',', startIndex);
                        maxScore = documentFilterResponse.DocumentsJson.Substring(startIndex, stopIndex - startIndex);
                        if (maxScore != "null")
                        {
                            documentFilterResponse.MaxScore = maxScore.WebParseDouble();
                        }
                    }
                }
            }

            return documentFilterResponse;
        }

        /// <summary>
        /// Get documents from a scroll.
        /// </summary>
        /// <param name="response">Response from a scroll request to Elasticsearch.</param>
        /// <returns>The next set of species observation.</returns>
        private DocumentFilterResponse GetScroll(ElasticsearchResponse<DynamicResponse> response)
        {
            DocumentFilterResponse documentFilterResponse;
            Int32 startIndex, stopIndex;
            String maxScore, shardInformation, documentCount;
            String[] splitShardInformation;

            documentFilterResponse = new DocumentFilterResponse();
            if (response.Body.IsNotNull())
            {
                documentFilterResponse.ElapsedTime = (Int32)(response.Body.Values.ElementAt(1));

                // Get shard information.
                shardInformation = (String)(response.Body.Values.ElementAt(4));
                splitShardInformation = shardInformation.Split(':');
                documentFilterResponse.ShardTotalCount = splitShardInformation[1].Substring(0, splitShardInformation[1].IndexOf(',')).WebParseInt32();
                documentFilterResponse.ShardSuccessfulCount = splitShardInformation[2].Substring(0, splitShardInformation[2].IndexOf(',')).WebParseInt32();
                if (documentFilterResponse.ShardSuccessfulCount == documentFilterResponse.ShardTotalCount)
                {
                    documentFilterResponse.ShardFailedCount = splitShardInformation[3].Substring(0, splitShardInformation[3].IndexOf('}')).WebParseInt32();
                }
                else
                {
                    documentFilterResponse.ShardFailedCount = splitShardInformation[3].Substring(0, splitShardInformation[3].IndexOf(',')).WebParseInt32();
                }

                documentFilterResponse.DocumentCount = 0;
                documentFilterResponse.DocumentsJson = (String)(response.Body.Values.ElementAt(5));
                documentFilterResponse.TimedOut = (Boolean)(response.Body.Values.ElementAt(2));
                if (!documentFilterResponse.TimedOut)
                {
                    // Get species observation count.
                    startIndex = documentFilterResponse.DocumentsJson.IndexOf(':') + 1;
                    stopIndex = documentFilterResponse.DocumentsJson.IndexOf(',');
                    documentCount = documentFilterResponse.DocumentsJson.Substring(startIndex, stopIndex - startIndex);
                    documentFilterResponse.DocumentCount = documentCount.WebParseInt64();
                    startIndex = stopIndex + 1;

                    if (documentFilterResponse.DocumentCount > 0)
                    {
                        // Get max score
                        startIndex = documentFilterResponse.DocumentsJson.IndexOf(':', startIndex) + 1;
                        stopIndex = documentFilterResponse.DocumentsJson.IndexOf(',', startIndex);
                        maxScore = documentFilterResponse.DocumentsJson.Substring(startIndex, stopIndex - startIndex);
                        if (maxScore != "null")
                        {
                            documentFilterResponse.MaxScore = maxScore.WebParseDouble();
                        }
                    }
                }
            }

            return documentFilterResponse;
        }

        /// <summary>
        /// Returns a list of unique values and their count from the provided field(column) from all objects that matches the filter.
        /// </summary>
        /// <param name="type">Type name.</param>
        /// <param name="searchTerm">The search term, could be either a field name or a script to calculate a 'field value'</param>
        /// <param name="filter">Filter for object of specified type.</param>
        /// <returns>Number of items that matches filter.</returns>
        protected DocumentUniqueValuesResponse GroupBy(string type,
                                                       ElasticsearchTerm searchTerm,
                                                       string filter)
        {
            return GroupBy(type, new List<ElasticsearchTerm> { searchTerm }, null, filter);
        }

        /// <summary>
        /// Returns a list of unique, 'flattened', values and their count from the provided fields(columns) from all objects that matches the filter.
        ///  All keys are concatenated with the divider, like "field1value:field2value:field3value"... (given that the divider is a colon)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="terms"></param>
        /// <param name="divider"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        protected DocumentUniqueValuesResponse GroupBy(string type,
                                                       List<ElasticsearchTerm> terms,
                                                       string divider,
                                                       string filter)
        {
            var query = GetCompleteQueryString(terms, filter);
            Debug.WriteLine("QUERY  " + query);
            var response = _client.Search<ElasticsearchTermCountResponse>(IndexName, type, query, qs => qs.AddQueryString("search_type", "count"));

            CheckResponse(response);

            if (response.Body.IsNull())
            {
                return new DocumentUniqueValuesResponse();
            }

            return new DocumentUniqueValuesResponse
            {
                ShardTotalCount = response.Body._shards.total,
                ShardSuccessfulCount = response.Body._shards.successful,
                ShardFailedCount = response.Body._shards.failed,
                UniqueValues = Flatten(response.Body.aggregations.term_count, divider, null)
            };
        }

        protected DocumentUniqueCountSumResponse GroupByCountAndSum(string type,
                                                                    List<ElasticsearchTerm> terms,
                                                                    string divider,
                                                                    string filter)
        {
            var query = GetCompleteQueryString(terms, filter);
            Debug.WriteLine("QUERY  " + query);
            var response = _client.Search<ElasticsearchTermCountResponse>(IndexName, type, query, qs => qs.AddQueryString("search_type", "count"));

            CheckResponse(response);

            if (response.Body.IsNull())
            {
                return new DocumentUniqueCountSumResponse();
            }

            return new DocumentUniqueCountSumResponse
            {
                ShardTotalCount = response.Body._shards.total,
                ShardSuccessfulCount = response.Body._shards.successful,
                ShardFailedCount = response.Body._shards.failed,
                UniqueValues = FlattenCountAndSum(response.Body.aggregations.term_count, divider, null)
            };
        }

        /// <summary>
        /// Index document.
        /// </summary>
        /// <param name="type">Type name.</param>
        /// <param name="documentIdentifier">Unique document identifier.</param>
        /// <param name="documentJson">Document in JSon format.</param>
        protected void Index(String type,
                             String documentIdentifier,
                             String documentJson)
        {
            ElasticsearchResponse<DynamicResponse> response;

            response = _client.Index<DynamicResponse>(IndexName,
                                                      type,
                                                      documentIdentifier,
                                                      documentJson);
            CheckResponse(response);
        }

        /// <summary>
        /// Check if cluster is ok.
        /// </summary>
        /// <returns>True, if cluster is ok.</returns>
        public Boolean IsClusterOk()
        {
            ElasticsearchHealth elasticsearchHealth;

            try
            {
                elasticsearchHealth = GetHealth();
                return elasticsearchHealth.IsOk();
            }
            catch (Exception)
            {
                // Cluster is not ok.
                return false;
            }
        }

        /// <summary>
        /// Get an Elasticsearch low level client instance from the client pool.
        /// </summary>
        /// <returns>An Elasticsearch low level client instance.</returns>
        private static ElasticLowLevelClient PopClient()
        {
            ElasticLowLevelClient client;

            lock (_clients)
            {
                if (_clients.IsNotEmpty())
                {
                    client = _clients[0];
                    _clients.RemoveAt(0);
                }
                else
                {
                    client = CreateClient();
                }
            }

            return client;
        }

        /// <summary>
        /// Add an Elasticsearch low level client instance to the client pool.
        /// <param name="client">An Elasticsearch low level client.</param>
        /// </summary>
        private static void PushClient(ElasticLowLevelClient client)
        {
            lock (_clients)
            {
                if (client.IsNotNull() && (_clients.Count < MaxClientCount))
                {
                    _clients.Add(client);
                }
            }
        }

        /// <summary>
        /// Start scroll of documents that matches search criteria.
        /// </summary>
        /// <param name="type">Type name.</param>
        /// <param name="filter">Filter for species observations.</param>
        /// <param name="scroll">Information about the scroll.</param>
        /// <param name="response">Response from a scroll request to Elasticsearch.</param>
        /// <returns>The first set of species observation.</returns>
        public DocumentFilterResponse StartScroll(String type,
                                                  String filter,
                                                  ElasticsearchScroll scroll)
        {
            ElasticsearchResponse<DynamicResponse> response;
            StringBuilder scrollInformation;
            DocumentFilterResponse documentFilterResponse;
            Int32 startIndex, stopIndex;
            String maxScore, shardInformation, documentCount;
            String[] splitShardInformation;

            scrollInformation = new StringBuilder();
            scrollInformation.Append("{");
            scrollInformation.Append("\"size\" : 10000"); 
            scrollInformation.Append(", \"_source\" : {\"include\": [\"DarwinCore_Id\"]}");
            //scrollInformation.Append(", \"_source\" : {\"include\": [\"DarwinCore_Id\", \"Taxon_DyntaxaTaxonID\", \"Taxon_Species_TaxonId\"]}");
            //            scrollInformation.Append(", \"filter\": { \"terms\": { \"Taxon_Species_TaxonId\":[1]}}");
            scrollInformation.Append(", \"sort\" : [\"_doc\"]");
            scrollInformation.Append("}");
            response = _client.Search<DynamicResponse>(IndexName, type, scrollInformation.ToString(), s => s.Scroll(new TimeSpan(0, scroll.KeepAlive, 0)));
            CheckResponse(response);
            scroll.ScrollId = (String)(response.Body.Values.ElementAt(0));
            documentFilterResponse = new DocumentFilterResponse();
            if (response.Body.IsNotNull())
            {
                documentFilterResponse.ElapsedTime = (Int32)(response.Body.Values.ElementAt(1));

                // Get shard information.
                shardInformation = (String)(response.Body.Values.ElementAt(3));
                splitShardInformation = shardInformation.Split(':');
                documentFilterResponse.ShardTotalCount = splitShardInformation[1].Substring(0, splitShardInformation[1].IndexOf(',')).WebParseInt32();
                documentFilterResponse.ShardSuccessfulCount = splitShardInformation[2].Substring(0, splitShardInformation[2].IndexOf(',')).WebParseInt32();
                if (documentFilterResponse.ShardSuccessfulCount == documentFilterResponse.ShardTotalCount)
                {
                    documentFilterResponse.ShardFailedCount = splitShardInformation[3].Substring(0, splitShardInformation[3].IndexOf('}')).WebParseInt32();
                }
                else
                {
                    documentFilterResponse.ShardFailedCount = splitShardInformation[3].Substring(0, splitShardInformation[3].IndexOf(',')).WebParseInt32();
                }

                documentFilterResponse.DocumentCount = 0;
                documentFilterResponse.DocumentsJson = (String)(response.Body.Values.ElementAt(4));
                documentFilterResponse.TimedOut = (Boolean)(response.Body.Values.ElementAt(2));
                if (!documentFilterResponse.TimedOut)
                {
                    // Get species observation count.
                    startIndex = documentFilterResponse.DocumentsJson.IndexOf(':') + 1;
                    stopIndex = documentFilterResponse.DocumentsJson.IndexOf(',');
                    documentCount = documentFilterResponse.DocumentsJson.Substring(startIndex, stopIndex - startIndex);
                    documentFilterResponse.DocumentCount = documentCount.WebParseInt64();
                    startIndex = stopIndex + 1;

                    if (documentFilterResponse.DocumentCount > 0)
                    {
                        // Get max score
                        startIndex = documentFilterResponse.DocumentsJson.IndexOf(':', startIndex) + 1;
                        stopIndex = documentFilterResponse.DocumentsJson.IndexOf(',', startIndex);
                        maxScore = documentFilterResponse.DocumentsJson.Substring(startIndex, stopIndex - startIndex);
                        if (maxScore != "null")
                        {
                            documentFilterResponse.MaxScore = maxScore.WebParseDouble();
                        }
                    }
                }
            }

            return documentFilterResponse;
        }

        /// <summary>
        /// Update index alias, e.g. switch usage of alias from current index to new index.
        /// </summary>
        /// <param name="alias">Alias name.</param>
        /// <param name="currentIndex">Current index name.</param>
        /// <param name="newIndex">New index name.</param>
        public void UpdateIndexAlias(String alias,
                                     String currentIndex,
                                     String newIndex)
        {
            ElasticsearchResponse<DynamicResponse> response;

            if (newIndex.IsNotEmpty())
            {
                // Create new alias.
                response = _client.IndicesPutAlias<DynamicResponse>(newIndex, alias, null);
                CheckResponse(response);
            }

            if (currentIndex.IsNotEmpty())
            {
                // Remove old alias.
                response = _client.IndicesDeleteAlias<DynamicResponse>(currentIndex, alias);
                CheckResponse(response);
            }
        }

        /// <summary>
        /// Update mappings.
        /// </summary>
        /// <param name="type">Type name.</param>
        /// <param name="mapping">Modified mapping.</param>
        /// <returns>Updated mappings.</returns>
        protected FieldDefinitionList UpdateMapping(String type,
                                                    String mapping)
        {
            ElasticsearchResponse<DynamicResponse> response;

            Debug.WriteLine("Update mapping = " + mapping);
            response = _client.IndicesPutMapping<DynamicResponse>(IndexName, type, mapping);
            CheckResponse(response);
            if (response.Body.IsNotNull() && (response.Body.Count > 0))
            {
                return GetMapping(type);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update settings.
        /// </summary>
        /// <param name="settings">Modified settings.</param>
        protected void UpdateSettings(String settings)
        {
            ElasticsearchResponse<DynamicResponse> response;

            Debug.WriteLine("Update settings = " + settings);
            response = _client.IndicesPutSettings<DynamicResponse>(IndexName, settings);
            CheckResponse(response);
        }
    }
}

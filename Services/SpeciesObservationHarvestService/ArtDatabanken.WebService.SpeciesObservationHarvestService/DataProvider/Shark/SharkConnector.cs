using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Shark
{
    /// <summary>
    /// The Shark connector.
    /// Information about what sets of species observations that are included in Shark
    /// can be retrived from http://sharkdata.se/datasets/table.json
    /// Species observations are retrived from
    /// http://sharkdata.se/speciesobs/table.json/?page={0}&per_page={1}&view_deleted=true
    /// 2016-10-20 Total number of species observations in Shark: 682051.
    /// </summary>
    public class SharkConnector : IDataProviderConnector
    {
        /// <summary>
        /// How many observations per page should be processed.
        /// </summary>
        internal const Int64 MaxSpeciesObservationsPerPage = 1000;

        /// <summary>
        /// Contains observation data by specified page and number of observations per page.
        /// I.e. use String.Format(SPECIES_OBSERVATION_RESULT_API_URI, 1, 10);
        /// </summary>
        private const String SpeciesObservationResultApiUri = "/speciesobs/table.json/?page={0}&per_page={1}&view_deleted=true";
        private const String StatusActive = "active";
        private const String StatusColumnName = "status";
        private const String StatusDeleted = "deleted";

        /// <summary>
        /// Initializes a new instance of the <see cref="SharkConnector"/> class.
        /// </summary>
        public SharkConnector()
        {
            WebServiceAddress = "http://www.sharkdata.se";
        }

        /// <summary>
        /// Get base address to web service without internet protocol (http or https)
        /// </summary>
        private String WebServiceAddress { get; set; }

        /// <summary>
        /// Opens a new connection to the web service and tries to return deserialized JSON rows for the "data-page" as requested by the pageNumber and pageSize parameters
        /// </summary>
        /// <param name="webServiceUri">The uri to the web service (base address without any query part)</param>
        /// <param name="requestUri">The query part of the request to the web service</param>
        /// <returns></returns>
        public SharkDataSetsJson GetData(string webServiceUri, string requestUri)
        {
            using (HttpClient client = new HttpClient { BaseAddress = new Uri(webServiceUri), Timeout = new TimeSpan(0, 10, 0) })
            {
                // Add an Accept header for JSON format.
                MediaTypeWithQualityHeaderValue mediaType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(mediaType);

                HttpResponseMessage dataSetResponse = client.GetAsync(requestUri).Result;

                if (dataSetResponse.IsSuccessStatusCode)
                {
                    var response = dataSetResponse.Content.ReadAsByteArrayAsync().Result;
                    var responseString = Encoding.UTF8.GetString(response, 0, response.Length);
                    return new JavaScriptSerializer { MaxJsonLength = int.MaxValue }.Deserialize<SharkDataSetsJson>(responseString);
                }
            }

            return null;
        }

        /// <summary>
        /// Get information about data sets in Shark.
        /// </summary>
        /// <returns>Information about data sets in Shark.</returns>
        public List<String> GetDatasetInformation()
        {
            List<String> dataSets;
            SharkDataSetsJson dataSetsJson, speciesObservationJson;
            String datasetName, query;

            dataSets = new List<String>();
            dataSetsJson = GetData(WebServiceAddress, "datasets/table.json");
            int datasetNameIndex = -1;
            for (Int32 index = 0; index < dataSetsJson.Header.Count; index++)
            {
                if (dataSetsJson.Header[index].ToLower() == "dataset_name")
                {
                    datasetNameIndex = index;
                }
            }

            foreach (List<String> row in dataSetsJson.Rows)
            {
                datasetName = row[datasetNameIndex];
                query = string.Format("/datasets/{0}/data.json?page=1&per_page=1", datasetName);
                speciesObservationJson = GetData(WebServiceAddress, query);
                dataSets.Add(string.Format("Dataset [{0}] contains {1} species observations", datasetName, speciesObservationJson.Rows.Count));
            }

            return dataSets;
        }

        /// <summary>
        /// Get number of pages that should be retreived
        /// if all species observations are retreived.
        /// </summary>
        /// <returns>Number of pages.</returns>
        public Int64 GetPageCount(Int64 speciesObservationCount)
        {
            Int64 pageCount;

            pageCount = (Int64)(Math.Ceiling(speciesObservationCount * 1.0 / MaxSpeciesObservationsPerPage));
            return pageCount;
        }

        /// <summary>
        /// Converts a deserialized JSON row to a WebData object.
        /// Also injection protection is performed on both names and values
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="speciesObservationJson"></param>
        /// <returns></returns>
        public static WebData GetSpeciesObservation(IList<string> headers,
                                                    IList<string> speciesObservationJson)
        {
            WebData speciesObservation = new WebData { DataFields = new List<WebDataField>() };

            foreach (var columnHeader in headers)
            {
                var columnIndex = headers.IndexOf(columnHeader);
                var rowCellValue = speciesObservationJson[columnIndex];
                speciesObservation.DataFields.Add(new WebDataField
                {
                    Name = columnHeader.CheckInjection(),
                    Type = WebDataType.String,
                    Value = rowCellValue.CheckInjection()
                });
            }

            return speciesObservation;
        }

        /// <summary>
        /// Get information about species observations
        /// that has changed in the specified date range.
        /// Only date part of parameters changedFrom and changedTo
        /// are used. It does not matter what time of day that is set
        /// in parameters changedFrom and changedTo.
        /// </summary>
        /// <param name="changedFrom">Start date for changes.</param>
        /// <param name="changedTo">End date for changes.</param>
        /// <param name="mappings">The mappings.</param>
        /// <param name="context">Web service context.</param>
        /// <param name="connectorServer">The connector server.</param>
        /// <returns>
        /// Returns true if there are more species
        /// observations to retrieve for current date.
        /// </returns>
        public Boolean GetSpeciesObservationChange(DateTime changedFrom,
                                                   DateTime changedTo,
                                                   List<HarvestMapping> mappings,
                                                   WebServiceContext context,
                                                   IConnectorServer connectorServer)
        {
            Int64 currentPage;
            Int32 noOfCreated, noOfCreatedErrors;
            Int32 noOfDeleted, noOfDeletedErrors;
            Int64 nextPage, pageCount, speciesObservationCount;
            Tuple<List<HarvestSpeciesObservation>, List<String>> page;
            Stopwatch stopwatch;
            WebSpeciesObservationDataProvider dataProvider;

            stopwatch = new Stopwatch();
            stopwatch.Start();
            dataProvider = GetSpeciesObservationDataProvider(context);
            currentPage = context.GetSpeciesObservationDatabase().GetMaxChangeId(dataProvider.Id);
            speciesObservationCount = GetSpeciesObservationCount();
            pageCount = GetPageCount(speciesObservationCount);

            if (currentPage <= pageCount)
            {
                nextPage = currentPage;
                if (nextPage < pageCount)
                {
                    nextPage++;
                }

                // Get species observation information.
                page = GetSpeciesObservationPage(mappings, context, nextPage, MaxSpeciesObservationsPerPage);

                // New species observations.
                connectorServer.UpdateSpeciesObservations(context,
                                                          page.Item1,
                                                          dataProvider,
                                                          out noOfCreated,
                                                          out noOfCreatedErrors);

                // Deleted species observations.
                connectorServer.DeleteSpeciesObservations(context,
                                                          page.Item2,
                                                          dataProvider,
                                                          out noOfDeleted,
                                                          out noOfDeletedErrors);

                context.GetSpeciesObservationDatabase().LogHarvestRead(context,
                                                                       dataProvider,
                                                                       changedFrom,
                                                                       changedTo,
                                                                       stopwatch.ElapsedMilliseconds,
                                                                       noOfCreated,
                                                                       noOfCreatedErrors,
                                                                       -1,
                                                                       -1,
                                                                       noOfDeleted,
                                                                       noOfDeletedErrors,
                                                                       currentPage);

                stopwatch.Stop();
                Debug.WriteLine(string.Format("SHARK handled page {0} of {1} in {2} msec", nextPage, pageCount, stopwatch.ElapsedMilliseconds));

                if (nextPage <= pageCount)
                {
                    context.GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, nextPage);
                }

                // Log latest harvest date for the data provider
                context.GetSpeciesObservationDatabase().SetDataProviderLatestHarvestDate(dataProvider.Id, changedTo);
            }

            return false;
        }

        /// <summary>
        /// Opens a new connection to the SHARK web service and tries to return processed update-rows and delete-guids for the "data-page" as requested by the pageNumber and pageSize parameters
        /// </summary>
        /// <param name="mappings"></param>
        /// <param name="context"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Tuple<List<HarvestSpeciesObservation>, List<String>> GetSpeciesObservationPage(List<HarvestMapping> mappings, WebServiceContext context, Int64 pageNumber, Int64 pageSize)
        {
            var createdSpeciesObservations = new List<HarvestSpeciesObservation>();
            var deletedSpeciesObservationGuids = new List<string>();
            var dyntaxaTaxonIdReplacementVector = new Dictionary<int, int>();

            var page = GetSpeciesObservationPage(pageNumber, pageSize);

            if (page.IsNotNull())
            {
                // Extract dataset data
                foreach (List<String> speciesObservationJson in page.Rows)
                {
                    var speciesObservation = GetSpeciesObservation(page.Header, speciesObservationJson);
                    var dyntaxaTaxonId = speciesObservation.DataFields.FirstOrDefault(item => item.Name.ToLower() == "dyntaxataxonid");
                    var occurrenceId = speciesObservation.DataFields.FirstOrDefault(item => item.Name.ToLower() == "occurrenceid");
                    // find status in webData, get expected values [ACTIVE|DELETED]
                    var statusElement = speciesObservation.DataFields.FirstOrDefault(item => item.Name.ToLower().Equals(StatusColumnName));

                    // If DELETED, just add to list and continue
                    if (statusElement != null && statusElement.Value.ToLower().Equals(StatusDeleted) && occurrenceId != null)
                    {
                        deletedSpeciesObservationGuids.Add(occurrenceId.Value);
                        continue;
                    }

                    // If ACTIVE, add to list and continue
                    if (statusElement != null && statusElement.Value.ToLower().Equals(StatusActive))
                    {
                        var sharkProcess = new SharkProcess();

                        // Skip processing values that's missing a TaxonId value
                        if (dyntaxaTaxonId == null || !dyntaxaTaxonId.Value.IsInteger())
                        {
                            // Debug.WriteLine(string.Format("SHARK, page {0}, OccurenceId [{1}] has an invalid DyntaxaTaxonId [{2}]", pageNumber, occurrenceId == null ? "null" : occurrenceId.Value, dyntaxaTaxonId == null ? "null" : dyntaxaTaxonId.Value));
                            if (dyntaxaTaxonId != null)
                            {
                                dyntaxaTaxonId.Value = string.Format("Invalid DyntaxaTaxonId [{0}]", dyntaxaTaxonId.Value);
                            }
                        }
                        else
                        {
                            // If a valid TaxonId could not be found, try to find it's replacement and use the new value instead
                            if (!sharkProcess.IsDyntaxaTaxonValid(speciesObservation, context))
                            {
                                var currentDyntaxaTaxonId = int.Parse(dyntaxaTaxonId.Value);
                                int newDyntaxaTaxonId;

                                // Small local cache for the replacement dyntaxaTaxonId's
                                if (dyntaxaTaxonIdReplacementVector.ContainsKey(currentDyntaxaTaxonId))
                                {
                                    newDyntaxaTaxonId = dyntaxaTaxonIdReplacementVector[currentDyntaxaTaxonId];
                                }
                                else
                                {
                                    newDyntaxaTaxonId = Data.TaxonManager.GetCurrentTaxonId(context, currentDyntaxaTaxonId);
                                    dyntaxaTaxonIdReplacementVector.Add(currentDyntaxaTaxonId, newDyntaxaTaxonId);
                                }

                                // Save the new value into the row
                                dyntaxaTaxonId.Value = newDyntaxaTaxonId.ToString();

                                // Re-validate TaxonId that does not exist at all in the Taxon table
                                if (newDyntaxaTaxonId == currentDyntaxaTaxonId || !sharkProcess.IsDyntaxaTaxonValid(speciesObservation, context))
                                {
                                    // Debug.WriteLine(string.Format("SHARK, page {0}, OccurenceId [{1}] has an invalid DyntaxaTaxonId [{2}] and a valid replacement couldn't be found in the Taxon table", pageNumber, occurrenceId == null ? "null" : occurrenceId.Value, currentDyntaxaTaxonId));
                                    // Add error text to make the row invalid during processing
                                    dyntaxaTaxonId.Value = string.Format("Invalid DyntaxaTaxonId [{0}]", newDyntaxaTaxonId);
                                }
                                else
                                {
                                    // Debug.WriteLine(string.Format("SHARK, page {0}, OccurenceId [{1}] has an invalid DyntaxaTaxonId [{2}] and it's replaced with [{3}]", pageNumber, occurrenceId == null ? "null" : occurrenceId.Value, currentDyntaxaTaxonId, newDyntaxaTaxonId));
                                }
                            }
                        }

                        createdSpeciesObservations.Add(sharkProcess.ProcessObservation(speciesObservation, mappings, context));
                        continue;
                    }

                    if (statusElement == null)
                    {
                        throw new ArgumentNullException(String.Format("Field {0} not found.", StatusColumnName));
                    }

                    throw new ArgumentNullException(String.Format("Field {0} has an invalid value [{1}].", StatusColumnName, statusElement.Value));
                }
            }

            return new Tuple<List<HarvestSpeciesObservation>, List<string>>(createdSpeciesObservations, deletedSpeciesObservationGuids);
        }

        /// <summary>
        /// Opens a new connection to the SHARK web service and tries to return deserialized JSON rows for the "data-page" as requested by the pageNumber and pageSize parameters
        /// </summary>
        /// <param name="pageNumber">The number of the requested page</param>
        /// <param name="pageSize">The size, rowcount, of the requested page</param>
        /// <returns></returns>
        public SharkDataSetsJson GetSpeciesObservationPage(Int64 pageNumber, Int64 pageSize)
        {
            return GetData(WebServiceAddress, string.Format(SpeciesObservationResultApiUri, pageNumber, pageSize));
        }

        /// <summary>
        /// Get species observation count.
        /// </summary>
        /// <returns>species observation count.</returns>
        public Int64 GetSpeciesObservationCount()
        {
            SharkDataSetsJson observationPage;

            observationPage = GetSpeciesObservationPage(1, 1);
            if (observationPage.IsNull())
            {
                throw new ApplicationException("Faild to retrieve species observation count.");
            }

            return observationPage.Total;
        }

        /// <summary>
        /// Get species observation data source for this connector.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Species observation data source for this connector.</returns>
        public WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context)
        {
            return new Data.SpeciesObservationManager().GetSpeciesObservationDataProvider(context, SpeciesObservationDataProviderId.Shark);
        }
    }
}

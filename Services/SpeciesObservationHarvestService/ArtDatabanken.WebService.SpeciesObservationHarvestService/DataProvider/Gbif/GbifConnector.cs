using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Script.Serialization;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif
{
    /// <summary>
    /// Example web address to GBIF data sources http://api.gbif.org/v1/occurrence/search?country=SE&datasetKey=427a6290-0c65-11dd-84d2-b8a03c50a862
    /// </summary>
    public class GbifConnector : IDataProviderConnector
    {
        /// <summary>
        /// Used for paging, the GBIF web service has a limit of 300 rows per request
        /// </summary>
        internal const int MaxObservationsPerWebPage = 300;

        /// <summary>
        /// Used for paging with file based
        /// </summary>
        internal const int MaxObservationsPerFilePage = 3000;

        /// <summary>
        /// The GBIF web service has a total limit of 200.000 rows per queried set
        /// </summary>
        private const int MaxObservationsPerQuery = 200000;

        /// <summary>
        /// The GBIF web service base address
        /// </summary>
        private const string WebServiceAddress = "http://api.gbif.org/";

        /// <summary>
        /// The number of minutes used when connecting to the GBIF web service
        /// </summary>
        private const int TimeoutMinutes = 10;

        /// <summary>
        /// Initializes a new instance of the <see cref="GbifConnector"/> class.
        /// Constructor without parameters are set to private to disable using the GbifConnector without a datasetKey and processInstance
        /// </summary>
        private GbifConnector()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GbifConnector"/> class.
        /// </summary>
        /// <param name="datasetKey">The unique identifier that's used when requesting the dataset</param>
        /// <param name="processInstance">The process instance used when processing the harvested rows</param>
        public GbifConnector(string datasetKey, BaseProcess processInstance)
            : this()
        {
            DatasetKey = datasetKey;
            ProcessInstance = processInstance;
            WebServiceQueryString = "/v1/occurrence/search?"
                .AddSimpleQueryParameter("country", "SE")
                .AddSimpleQueryParameter("datasetKey", DatasetKey);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GbifConnector"/> class.
        /// </summary>
        /// <param name="datasetKey">The unique identifier that's used to request the dataset</param>
        /// <param name="processInstance">The process instance used when processing the harvested rows</param>
        /// <param name="filename">The filename that holds the dataset to be harvested</param>
        public GbifConnector(string datasetKey, BaseProcess processInstance, string filename)
            : this(datasetKey, processInstance)
        {
            Filename = filename;
            IsFileBased = true;
        }

        /// <summary>
        /// The unique identifier that's used to request the dataset
        /// </summary>
        public string DatasetKey { private set; get; }

        /// <summary>
        /// The process instance used when processing the harvested rows
        /// </summary>
        public BaseProcess ProcessInstance { private set; get; }

        /// <summary>
        /// If true, the harvest of the current Dataset is made from the file that's referenced in Filename and not from the Internet
        /// </summary>
        public bool IsFileBased { private set; get; }

        /// <summary>
        /// If set to a filename, the harvest of the current Dataset is made from that file and not from the Internet
        /// </summary>
        public string Filename { set; get; }

        /// <summary>
        /// The querystring used for querying the GBIF REST service, the country and datasetKey parameters are set in the constructor.
        /// Extend the querystring using the extension methods AddSimpleQueryParameter and AddRangeQueryParameter
        /// </summary>
        internal string WebServiceQueryString { private set; get; }

        /// <summary>
        /// The line count, used only if the current Dataset is file based, otherwise 0 is returned
        /// </summary>
        private int _fileLineCount = 0;

        private int FileLineCount
        {
            get
            {
                if (_fileLineCount == 0 && IsFileBased)
                {
                    _fileLineCount = File.ReadLines(Filename).Count();
                }

                return _fileLineCount;
            }
        }

        /// <summary>
        /// The first row in the file, used only when the current Dataset is file based, otherwise an empty string is returned
        /// </summary>
        private string _headerRow = null;

        private string HeaderRow
        {
            get
            {
                if (_headerRow == null && IsFileBased)
                {
                    _headerRow = File.ReadLines(Filename).First();
                }

                return _headerRow;
            }
        }

        /// <summary>
        /// Get number of pages that should be retreived
        /// if all species observations are retreived.
        /// </summary>
        /// <returns>Number of pages.</returns>
        public Int64 GetPageCount(Int64 speciesObservationCount)
        {
            Int64 maxObservationsPerPage, pageCount;

            maxObservationsPerPage = IsFileBased ? MaxObservationsPerFilePage : MaxObservationsPerWebPage;
            pageCount = (Int64)(Math.Ceiling(speciesObservationCount * 1.0 / maxObservationsPerPage));
            return pageCount;
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
            Int32 createdObservationCount, createdObservationErrors;
            Int64 currentPage, maxObservationsPerPage, pageCount, speciesObservationCount;
            List<HarvestSpeciesObservation> speciesObservations;
            Stopwatch datasetStopwatch;
            WebSpeciesObservationDataProvider dataProvider;

            datasetStopwatch = new Stopwatch();
            datasetStopwatch.Start();

            dataProvider = GetSpeciesObservationDataProvider(context);
            currentPage = dataProvider.MaxChangeId;
            speciesObservationCount = GetSpeciesObservationCount();

            if ((speciesObservationCount > MaxObservationsPerQuery) && !IsFileBased)
            {
                throw new Exception("To many observations in data provider = " + dataProvider.Name + " observation count = " + speciesObservationCount);
            }

            maxObservationsPerPage = IsFileBased ? MaxObservationsPerFilePage : MaxObservationsPerWebPage;
            pageCount = GetPageCount(speciesObservationCount);
            if (currentPage < pageCount)
            {
                speciesObservations = GetSpeciesObservations(mappings, context, currentPage, maxObservationsPerPage);
                if (speciesObservations.IsNotEmpty())
                {
                    connectorServer.UpdateSpeciesObservations(context,
                                                              speciesObservations,
                                                              dataProvider,
                                                              out createdObservationCount,
                                                              out createdObservationErrors);

                    context.GetSpeciesObservationDatabase().LogHarvestRead(context,
                                                                           dataProvider,
                                                                           changedFrom,
                                                                           changedTo,
                                                                           datasetStopwatch.ElapsedMilliseconds,
                                                                           createdObservationCount,
                                                                           createdObservationErrors,
                                                                           -1,
                                                                           -1,
                                                                           -1,
                                                                           -1,
                                                                           -1);

                    // Log latest harvest date for the data provider
                    context.GetSpeciesObservationDatabase().SetDataProviderLatestHarvestDate(dataProvider.Id, changedTo);

                    if (currentPage < (pageCount - 1))
                    {
                        context.GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, currentPage + 1);
                    }

                    datasetStopwatch.Stop();
                    Debug.WriteLine("Dataset with a total of {0} rows ({1} ok and {2} errors) took [{3}] seconds", createdObservationCount + createdObservationErrors, createdObservationCount, createdObservationErrors, datasetStopwatch.ElapsedMilliseconds / 1000);
                }
            }

            return false;
        }

        /// <summary>
        /// Get species observation count.
        /// </summary>
        /// <returns>species observation count.</returns>
        public Int64 GetSpeciesObservationCount()
        {
            GbifDataSetsJson page = GetSpeciesObservationPage(0, 0);

            if (page.IsNull())
            {
                throw new ApplicationException("Faild to retrieve species observation count.");
            }

            return page.Count;
        }

        /// <summary>
        /// Opens a new connection to the GBIF web service and tries to return processed update-rows for the "data-page" as requested by the dataSetKey combined with the pageNumber and pageSize parameters
        /// </summary>
        /// <param name="mappings"></param>
        /// <param name="context"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private List<HarvestSpeciesObservation> GetSpeciesObservations(List<HarvestMapping> mappings, WebServiceContext context, Int64 pageNumber, Int64 pageSize)
        {
            var createdSpeciesObservations = new List<HarvestSpeciesObservation>();
            GbifDataSetsJson datasetData = GetSpeciesObservationPage(pageNumber, pageSize);

            if (datasetData.IsNotNull())
            {
                // Convert and process species observations.
                foreach (Dictionary<string, object> speciesObservations in datasetData.Results)
                {
                    createdSpeciesObservations.Add(ProcessInstance.ProcessObservation(GetWebData(speciesObservations), mappings, context));
                }
            }

            return createdSpeciesObservations;
        }

        public virtual WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context)
        {
            throw new NotImplementedException("This should be overridden in all inherited classes");
        }

        /// <summary>
        /// Opens the file tries to return rows for the "data-page" as requested by the pageNumber and pageSize parameters
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private GbifDataSetsJson GetSpeciesObservationPage(Int64 pageNumber, Int64 pageSize)
        {
            if (IsFileBased)
            {
                var startRow = pageNumber * pageSize + 1;
                var endRow = (pageNumber + 1) * pageSize + 1;
                var columns = HeaderRow.Split('\t');
                var page = new GbifDataSetsJson
                {
                    Results = new List<Dictionary<string, object>>(),
                    Count = FileLineCount,
                    Limit = pageSize,
                    Offset = startRow,
                    EndOfRecords = endRow >= FileLineCount
                };

                foreach (var row in File.ReadLines(Filename).Skip((Int32)startRow).Take((Int32)pageSize))
                {
                    var resultRow = new Dictionary<string, object>();
                    var values = row.Split('\t');

                    for (int i = 0; i < columns.Length; i++)
                    {
                        if (values[i].IsNotEmpty())
                        {
                            resultRow.Add(columns[i], values[i]);
                        }
                    }

                    page.Results.Add(resultRow);
                }

                if ((pageNumber <= 0) && page.Results.IsNotEmpty())
                {
                    // Remove first line in file.
                    // This line is expected to hold the column headers.
                    page.Results.RemoveAt(0);
                }

                return page;
            }
            else
            {
                using (var client = new HttpClient { BaseAddress = new Uri(WebServiceAddress), Timeout = new TimeSpan(0, TimeoutMinutes, 0) })
                {
                    // Add an Accept header for JSON format.
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var querystring = WebServiceQueryString
                        .AddSimpleQueryParameter("offset", pageNumber * pageSize)
                        .AddSimpleQueryParameter("limit", pageSize);

                    var dataSetResponse = client.GetAsync(querystring).Result;

                    if (dataSetResponse.IsSuccessStatusCode)
                    {
                        var response = dataSetResponse.Content.ReadAsByteArrayAsync().Result;
                        var responseString = Encoding.UTF8.GetString(response, 0, response.Length);
                        return new JavaScriptSerializer { MaxJsonLength = int.MaxValue }.Deserialize<GbifDataSetsJson>(responseString);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Converts a deserialized JSON row to a WebData object.
        /// Also injection protection is performed on both names and values
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private static WebData GetWebData(IDictionary<string, object> dataRow)
        {
            var webData = new WebData { DataFields = new List<WebDataField>() };

            foreach (var keyValuePair in dataRow)
            {
                webData.DataFields.Add(new WebDataField
                {
                    Name = keyValuePair.Key.CheckInjection(),
                    Type = WebDataType.String,
                    Value = keyValuePair.Value == null ? string.Empty : keyValuePair.Value.ToString().CheckInjection()
                });
            }

            return webData;
        }
    }
}

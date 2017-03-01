using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Artportalen
{
    /// <summary>
    /// Connector for Artportalen.
    /// </summary>
    public class ArtportalenConnector : IDataProviderConnector
    {
        private List<Int64> mUpdatedSpeciesObservationIds = null;
        private List<WebData> mUpdatedSpeciesObservations = null;

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
            Int32 noOfDeleted, noOfDeletedErrors;
            int noOfUpdated = 0, noOfUpdatedErrors = 0;
            DateTime calcChangedTo = changedTo.AddDays(1);
            WebSpeciesObservationDataProvider dataProvider = GetSpeciesObservationDataProvider(context);
            WebData webData;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            if (mUpdatedSpeciesObservations.IsEmpty() && mUpdatedSpeciesObservationIds.IsEmpty())
            {
                SpeciesObservationChange speciesObservationChange = new SpeciesObservationChange();
                mUpdatedSpeciesObservationIds = new List<Int64>();
                mUpdatedSpeciesObservations = new List<WebData>();

                int sumNoOfCreated = 0, sumNoOfCreatedErrors = 0,
                    sumNoOfUpdated = 0, sumNoOfUpdatedErrors = 0,
                    sumNoOfDeleted = 0, sumNoOfDeletedErrors = 0;

                using (ArtportalenServer artportalenServer = new ArtportalenServer())
                {
                    var artportalenProcess = new ArtportalenProcess();

                    // Get changes from Artportalen.
                    using (DataReader dataReader = artportalenServer.GetSpeciesObservations(changedFrom, calcChangedTo))
                    {
                        while (dataReader.Read())
                        {
                            webData = new WebData();
                            webData.LoadData(dataReader);
                            if (mUpdatedSpeciesObservations.Count >= 100000)
                            {
                                mUpdatedSpeciesObservationIds.Add(GetSpeciesObservationId(webData));
                            }
                            else
                            {
                                mUpdatedSpeciesObservations.Add(webData);
                            }
                        }

                        AddProjectParameters(this.mUpdatedSpeciesObservations, dataReader);
                    }

                    // Save updated species observations into database.
                    // Max 10000 species observations are saved in one call.
                    UpdateSpeciesObservations(context,
                                              mUpdatedSpeciesObservations,
                                              dataProvider,
                                              connectorServer,
                                              mappings,
                                              out noOfUpdated,
                                              out noOfUpdatedErrors);
                    sumNoOfUpdated += noOfUpdated;
                    sumNoOfUpdatedErrors += noOfUpdatedErrors;

                    // Get deleted observations from Artportalen
                    speciesObservationChange.DeletedSpeciesObservationGuids = new List<String>();
                    using (DataReader dataReader = artportalenServer.GetDeletedObservations(changedFrom, calcChangedTo))
                    {
                        int i = 0;
                        while (dataReader.Read())
                        {
                            webData = new WebData();
                            webData.LoadData(dataReader);
                            String deletedCatalogNumber = artportalenProcess.ProcessDeletedObservation(webData);
                            speciesObservationChange.DeletedSpeciesObservationGuids.Add(deletedCatalogNumber);
                            if (decimal.Remainder(++i, 10000) == 0)
                            {
                                // Write every 10000 observation to database to avoid memory problems
                                connectorServer.DeleteSpeciesObservations(context, speciesObservationChange.DeletedSpeciesObservationGuids, dataProvider, out noOfDeleted, out noOfDeletedErrors);

                                sumNoOfDeleted += noOfDeleted;
                                sumNoOfDeletedErrors += noOfDeletedErrors;

                                speciesObservationChange.DeletedSpeciesObservationGuids.Clear();
                            }
                        }

                        // Write the remaining observations to database
                        connectorServer.DeleteSpeciesObservations(context, speciesObservationChange.DeletedSpeciesObservationGuids, dataProvider, out noOfDeleted, out noOfDeletedErrors);
                        sumNoOfDeleted += noOfDeleted;
                        sumNoOfDeletedErrors += noOfDeletedErrors;

                        speciesObservationChange.DeletedSpeciesObservationGuids.Clear();
                    }
                }

                // Log latest harvest date for the data provider
                context.GetSpeciesObservationDatabase().SetDataProviderLatestHarvestDate(dataProvider.Id, changedTo);

                context.GetSpeciesObservationDatabase().LogHarvestRead(context,
                                                                       dataProvider,
                                                                       changedFrom,
                                                                       changedTo,
                                                                       stopwatch.ElapsedMilliseconds,
                                                                       sumNoOfCreated,
                                                                       sumNoOfCreatedErrors,
                                                                       sumNoOfUpdated,
                                                                       sumNoOfUpdatedErrors,
                                                                       sumNoOfDeleted,
                                                                       sumNoOfDeletedErrors);
            }
            else
            {
                // Save updated species observations into database.
                // Max 10000 species observations are saved in one call.
                if (mUpdatedSpeciesObservations.IsNotEmpty())
                {
                    UpdateSpeciesObservations(context,
                                              mUpdatedSpeciesObservations,
                                              dataProvider,
                                              connectorServer,
                                              mappings,
                                              out noOfUpdated,
                                              out noOfUpdatedErrors);
                }
                else
                {
                    if (mUpdatedSpeciesObservationIds.IsNotEmpty())
                    {
                        UpdateSpeciesObservations(context,
                                                  mUpdatedSpeciesObservationIds,
                                                  dataProvider,
                                                  connectorServer,
                                                  mappings,
                                                  out noOfUpdated,
                                                  out noOfUpdatedErrors);
                    }
                }

                context.GetSpeciesObservationDatabase().LogHarvestRead(context,
                                                                       dataProvider,
                                                                       changedFrom,
                                                                       changedTo,
                                                                       stopwatch.ElapsedMilliseconds,
                                                                       0,
                                                                       0,
                                                                       noOfUpdated,
                                                                       noOfUpdatedErrors,
                                                                       0,
                                                                       0);
            }

            stopwatch.Stop();
            return mUpdatedSpeciesObservations.IsNotEmpty() || mUpdatedSpeciesObservationIds.IsNotEmpty();
        }

        /// <summary>
        /// Adds projectParameters, if any, to mUpdatedSpeciesObservations, as separate new WebFields objects
        /// </summary>
        /// <param name="updatedSpeciesObservations">The observations</param>
        /// <param name="dataReader">The datareader </param>
        private static void AddProjectParameters(List<WebData> updatedSpeciesObservations, DataReader dataReader)
        {
            var hasSecondDataSet = false;
            if (!dataReader.HasColumn("ProjectParameterName"))
            {
                hasSecondDataSet = dataReader.NextResultSet(); // TODO: Kasta exception om den här ger false (efter produktionssättning av de nya procedurerna)
            }

            if (hasSecondDataSet)
            {
                int sightingId = -1;
                WebData speciesObservation = null;
                while (dataReader.Read())
                {
                    if (sightingId != dataReader.GetInt32("SightingId"))
                    {
                        sightingId = dataReader.GetInt32("SightingId");
                        speciesObservation = updatedSpeciesObservations.FirstOrDefault(item => item.DataFields.Exists(field => field.Name == "Id" && field.Value.Equals(sightingId.WebToString())));
                    }

                    if (speciesObservation != null)
                    {
                        speciesObservation.DataFields.Add(new WebDataField
                        {
                            Information = dataReader.GetString("ProjectParameterId").CheckInjection(),
                            Name = dataReader.GetString("PropertyIdentifier").CheckInjection(),
                            Type = dataReader.GetString("ProjectParameterDataType") == "string"
                                ? WebDataType.String
                                : WebDataType.Float64,
                            Unit = dataReader.GetString("ProjectParameterUnit").CheckInjection(),
                            Value = dataReader.GetString("ProjectParameterValue").CheckInjection()
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Get information about species observations
        /// that has it's id in the SightingIds list.
        /// </summary>
        /// <param name="sightingIds">A list of id's to return information about.</param>
        /// <param name="mappings">The mappings.</param>
        /// <param name="context">Web service context.</param>
        /// <param name="connectorServer">The connector server.</param>
        public void GetSpeciesObservationChange(List<string> sightingIds,
                                                List<HarvestMapping> mappings,
                                                WebServiceContext context,
                                                IConnectorServer connectorServer)
        {
            WebSpeciesObservationDataProvider dataProvider = GetSpeciesObservationDataProvider(context);

            WebData webData;
            SpeciesObservationChange speciesObservationChange = new SpeciesObservationChange();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int sumNoOfCreated = 0, sumNoOfCreatedErrors = 0,
                sumNoOfUpdated = 0, sumNoOfUpdatedErrors = 0,
                sumNoOfDeleted = 0, sumNoOfDeletedErrors = 0;


            using (ArtportalenServer artportalenServer = new ArtportalenServer())
            {
                ArtportalenProcess artportalenProcess = new ArtportalenProcess();

                // Get created and edited observations from Artportalen
                using (DataReader dataReader = artportalenServer.GetSpeciesObservationsByIds(sightingIds))
                {
                    speciesObservationChange.CreatedSpeciesObservations = new List<HarvestSpeciesObservation>();
                    int i = 0;
                    int noOfCreated, noOfCreatedErrors;

                    while (dataReader.Read())
                    {
                        webData = new WebData();
                        webData.LoadData(dataReader);

                        HarvestSpeciesObservation harvestSpeciesObservation = artportalenProcess.ProcessObservation(webData, mappings, context);

                        speciesObservationChange.CreatedSpeciesObservations.Add(harvestSpeciesObservation);

                        if (decimal.Remainder(++i, 10000) == 0)
                        {
                            // Write every 10000 observation to database to avoid memory problems.
                            connectorServer.UpdateSpeciesObservations(context,
                                                                      speciesObservationChange.CreatedSpeciesObservations,
                                                                      dataProvider,
                                                                      out noOfCreated,
                                                                      out noOfCreatedErrors);


                            sumNoOfCreated += noOfCreated;
                            sumNoOfCreatedErrors += noOfCreatedErrors;

                            speciesObservationChange.CreatedSpeciesObservations.Clear();
                            speciesObservationChange.UpdatedSpeciesObservations.Clear();

                        }
                    }

                    // Write the remaining observations to database
                    connectorServer.UpdateSpeciesObservations(context,
                                                              speciesObservationChange.CreatedSpeciesObservations,
                                                              dataProvider,
                                                              out noOfCreated,
                                                              out noOfCreatedErrors);


                    sumNoOfCreated += noOfCreated;
                    sumNoOfCreatedErrors += noOfCreatedErrors;

                    speciesObservationChange.CreatedSpeciesObservations.Clear();

                }

                //// Get deleted observations from Artportalen
                //using (DataReader dataReader = artportalenServer.GetDeletedObservations(changedFrom, calcChangedTo))
                //{
                //    speciesObservationChange.DeletedSpeciesObservationGuids = new List<String>();

                //    int i = 0;
                //    while (dataReader.Read())
                //    {
                //        webData = new WebData();
                //        webData.LoadData(dataReader);
                //        String deletedCatalogNumber = artportalenProcess.ProcessDeletedObservation(webData);
                //        speciesObservationChange.DeletedSpeciesObservationGuids.Add(deletedCatalogNumber);
                //        if (decimal.Remainder(++i, 10000) == 0)
                //        {
                //            // Write every 10000 observation to database to avoid memory problems
                //            connectorServer.DeleteSpeciesObservations(context, speciesObservationChange.DeletedSpeciesObservationGuids, dataProvider, out noOfDeleted, out noOfDeletedErrors);

                //            sumNoOfDeleted += noOfDeleted;
                //            sumNoOfDeletedErrors += noOfDeletedErrors;

                //            speciesObservationChange.DeletedSpeciesObservationGuids.Clear();
                //        }
                //    }

                //    // Write the remaining observations to database
                //    connectorServer.DeleteSpeciesObservations(context, speciesObservationChange.DeletedSpeciesObservationGuids, dataProvider, out noOfDeleted, out noOfDeletedErrors);
                //    sumNoOfDeleted += noOfDeleted;
                //    sumNoOfDeletedErrors += noOfDeletedErrors;

                //    speciesObservationChange.DeletedSpeciesObservationGuids.Clear();
                //}
            }

            // Log latest harvest date for the data provider
            //context.GetSpeciesObservationDatabase().SetDataProviderLatestHarvestDate(dataProvider.Id, changedTo);

            context.GetSpeciesObservationDatabase().LogHarvestRead(context,
                                                                   dataProvider,
                                                                   new DateTime(1900, 01, 01),
                                                                   new DateTime(1900, 01, 01),
                                                                   stopwatch.ElapsedMilliseconds,
                                                                   sumNoOfCreated,
                                                                   sumNoOfCreatedErrors,
                                                                   sumNoOfUpdated,
                                                                   sumNoOfUpdatedErrors,
                                                                   sumNoOfDeleted,
                                                                   sumNoOfDeletedErrors);
            stopwatch.Stop();
        }

        /// <summary>
        /// Get species observation data source for this connector.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Species observation data source for this connector.</returns>
        public WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context)
        {
            return new Data.SpeciesObservationManager().GetSpeciesObservationDataProvider(context, SpeciesObservationDataProviderId.SpeciesGateway);
        }

        /// <summary>
        /// Get species observation id from species observation.
        /// </summary>
        /// <param name="speciesObservation">Web service request context.</param>
        private Int64 GetSpeciesObservationId(WebData speciesObservation)
        {
            if (speciesObservation.DataFields.IsNotEmpty())
            {
                foreach (WebDataField field in speciesObservation.DataFields)
                {
                    if (field.Name == "Id")
                    {
                        return field.Value.WebParseInt64();
                    }
                }
            }

            throw new ArgumentException("Species observation id not found in observation.");
        }

        /// <summary>
        /// Update species observations.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservations">Updated species observations.</param>
        /// <param name="dataProvider">The dataProvider.</param>
        /// <param name="connectorServer">The connector server.</param>
        /// <param name="mappings">The mappings.</param>
        /// <param name="noOfUpdated">No of updated species observations.</param>
        /// <param name="noOfUpdatedErrors">No of updating errors.</param>
        public void UpdateSpeciesObservations(WebServiceContext context,
                                              List<WebData> speciesObservations,
                                              WebSpeciesObservationDataProvider dataProvider,
                                              IConnectorServer connectorServer,
                                              List<HarvestMapping> mappings,
                                              out int noOfUpdated,
                                              out int noOfUpdatedErrors)
        {
            noOfUpdated = 0;
            noOfUpdatedErrors = 0;
            ArtportalenProcess artportalenProcess = new ArtportalenProcess();

            if (speciesObservations.IsNotEmpty())
            {
                List<HarvestSpeciesObservation> updatedSpeciesObservations = new List<HarvestSpeciesObservation>();
                for (Int32 index = speciesObservations.Count - 1; index >= 0; index--)
                {
                    HarvestSpeciesObservation harvestSpeciesObservation = artportalenProcess.ProcessObservation(speciesObservations[index], mappings, context);
                    updatedSpeciesObservations.Add(harvestSpeciesObservation);
                    speciesObservations.RemoveAt(index);
                    if (updatedSpeciesObservations.Count >= 10000)
                    {
                        break;
                    }
                }

                connectorServer.UpdateSpeciesObservations(context,
                                                          updatedSpeciesObservations,
                                                          dataProvider,
                                                          out noOfUpdated,
                                                          out noOfUpdatedErrors);
            }
        }

        /// <summary>
        /// Update species observations.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationIds">Ids for updated species observations.</param>
        /// <param name="dataProvider">The dataProvider.</param>
        /// <param name="connectorServer">The connector server.</param>
        /// <param name="mappings">The mappings.</param>
        /// <param name="noOfUpdated">No of updated species observations.</param>
        /// <param name="noOfUpdatedErrors">No of updating errors.</param>
        public void UpdateSpeciesObservations(WebServiceContext context,
                                              List<Int64> speciesObservationIds,
                                              WebSpeciesObservationDataProvider dataProvider,
                                              IConnectorServer connectorServer,
                                              List<HarvestMapping> mappings,
                                              out int noOfUpdated,
                                              out int noOfUpdatedErrors)
        {
            List<Int64> tempSpeciesObservtionIds;
            List<WebData> updatedSpeciesObservations;
            WebData webData;

            noOfUpdated = 0;
            noOfUpdatedErrors = 0;
            if (speciesObservationIds.IsNotEmpty())
            {
                tempSpeciesObservtionIds = new List<Int64>();
                for (Int32 index = speciesObservationIds.Count - 1; index >= 0; index--)
                {
                    tempSpeciesObservtionIds.Add(speciesObservationIds[index]);
                    speciesObservationIds.RemoveAt(index);
                    if (tempSpeciesObservtionIds.Count >= 10000)
                    {
                        break;
                    }
                }

                updatedSpeciesObservations = new List<WebData>();
                using (ArtportalenServer artportalenServer = new ArtportalenServer())
                {
                    // Get updated observations from Artportalen.
                    using (DataReader dataReader = artportalenServer.GetSpeciesObservationsByIds(tempSpeciesObservtionIds))
                    {
                        while (dataReader.Read())
                        {
                            webData = new WebData();
                            webData.LoadData(dataReader);
                            updatedSpeciesObservations.Add(webData);
                        }
                    }

                    UpdateSpeciesObservations(context,
                                              updatedSpeciesObservations,
                                              dataProvider,
                                              connectorServer,
                                              mappings,
                                              out noOfUpdated,
                                              out noOfUpdatedErrors);
                }
            }
        }
    }
}

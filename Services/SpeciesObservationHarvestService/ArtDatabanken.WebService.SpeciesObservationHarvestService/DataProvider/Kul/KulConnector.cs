using System;
using System.Collections.Generic;
using System.Diagnostics;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using WebSpeciesObservationChange = ArtDatabanken.WebService.Proxy.KulService.WebSpeciesObservationChange;
using WebSpeciesObservationField = ArtDatabanken.WebService.Proxy.KulService.WebSpeciesObservationField;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Kul
{
    /// <summary>
    /// The Kul connector.
    /// </summary>
    public class KulConnector : IDataProviderConnector
    {
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
            // DOES NOT WORK EQUAL TO OTHERS. UNTIL FIX IT RECIEVES ALL DATA IN KUL
            ////

            Int64 maxReturnedChanges = 1000;
            DateTime calcChangedTo = changedTo.AddDays(1);

            WebSpeciesObservationDataProvider dataProvider = GetSpeciesObservationDataProvider(context);
            Int64 nextChangeId = context.GetSpeciesObservationDatabase().GetMaxChangeId(dataProvider.Id) + 1;

            GetSpeciesObservationChange(changedFrom, false, calcChangedTo, false, nextChangeId, true, maxReturnedChanges, mappings, context, connectorServer);
            return false;
        }

        /// <summary>
        /// Get information about species observations
        /// that has changed in the specified date range or
        /// from a specified changeId.
        /// Only date part of parameters changedFrom and changedTo
        /// are used. It does not matter what time of day that is set
        /// in parameters changedFrom and changedTo.
        /// </summary>
        /// <param name="changedFrom">
        /// Changed from date.
        /// </param>
        /// <param name="isChangedFromSpecified">
        /// Is changed from specified.
        /// </param>
        /// <param name="changedTo">
        /// Changed to date.
        /// </param>
        /// <param name="isChangedToSpecified">
        /// Is changed to specified.
        /// </param>
        /// <param name="changeId">
        /// From witch change id.
        /// </param>
        /// <param name="isChangeIdspecified">
        /// Is changed id specified.
        /// </param>
        /// <param name="maxReturnedChanges">
        /// Max number of observations returned.
        /// </param>
        /// <param name="mappings">
        /// The mapping list.
        /// </param>
        /// <param name="context">
        /// The web service context.
        /// </param>
        /// <param name="connectorServer">
        /// The connector service.
        /// </param>
        public void GetSpeciesObservationChange(DateTime changedFrom,
                                                Boolean isChangedFromSpecified,
                                                DateTime changedTo,
                                                Boolean isChangedToSpecified,
                                                Int64 changeId,
                                                Boolean isChangeIdspecified,
                                                Int64 maxReturnedChanges,
                                                List<HarvestMapping> mappings,
                                                WebServiceContext context,
                                                IConnectorServer connectorServer)
        {
            SpeciesObservationChange speciesObservationChange = new SpeciesObservationChange
                                                                    {
                                                                        CreatedSpeciesObservations = new List<HarvestSpeciesObservation>(),
                                                                        UpdatedSpeciesObservations = new List<HarvestSpeciesObservation>()
                                                                    };

            var stopwatch = Stopwatch.StartNew();
            
            int sumNoOfCreated = 0, sumNoOfCreatedErrors = 0,
                sumNoOfUpdated = 0, sumNoOfUpdatedErrors = 0,
                sumNoOfDeleted = 0, sumNoOfDeletedErrors = 0;

            KulProcess kulProcess = new KulProcess();
            WebSpeciesObservationDataProvider dataProvider = GetSpeciesObservationDataProvider(context);

            Stopwatch stopwatch1 = Stopwatch.StartNew();
            WebSpeciesObservationChange webSpeciesObservationChange = WebServiceProxy.KulService.GetSpeciesObservationChangeAsSpecies(changedFrom,
                                                                              isChangedFromSpecified,
                                                                              changedTo,
                                                                              isChangedToSpecified,
                                                                              changeId,
                                                                              isChangeIdspecified,
                                                                              maxReturnedChanges);

            Debug.WriteLine("Harvest: " + DateTime.Now.ToLongTimeString() + " - Time: " + TimeSpan.FromMilliseconds(stopwatch1.ElapsedMilliseconds).TotalMinutes + " Minutes");
            

            // LOOP OVER CHANGEID
            // Denna verkar inte klara mer än ca 100 obsar så det behövs en loop över dessa...
            Int32 readSize = 0;
            Int64 currentChangeId = 0;
            int j = 0;
            while ((readSize++ < 50) && (currentChangeId < webSpeciesObservationChange.MaxChangeId))
            {
                currentChangeId = webSpeciesObservationChange.MaxChangeId;

                context.GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, webSpeciesObservationChange.MaxChangeId);
                
                // CREATED
                AddNewSpeciesObservations(webSpeciesObservationChange, speciesObservationChange, kulProcess, mappings, context, connectorServer, dataProvider, out sumNoOfCreated, out sumNoOfCreatedErrors);
                
                /* int i = 0;
                int noOfCreated, noOfCreatedErrors;
                foreach (Proxy.KulService.WebSpeciesObservation kulSpeciesObservation in webSpeciesObservationChange.CreatedSpeciesObservations)
                {
                    WebData webData = new WebData { DataFields = new List<WebDataField>() };

                    foreach (WebSpeciesObservationField webSpeciesObservationField in kulSpeciesObservation.Fields)
                    {
                        WebDataField webDataField = new WebDataField
                                                        {
                                                            Information = webSpeciesObservationField.Information,
                                                            Name = webSpeciesObservationField.Property.Id.ToString(),
                                                            Type = (WebDataType)webSpeciesObservationField.Type,
                                                            Unit = webSpeciesObservationField.Unit,
                                                            Value = webSpeciesObservationField.Value.CheckInjection()
                                                        };

                        if (webDataField.IsNotNull())
                        {
                            webData.DataFields.Add(webDataField);
                        }
                    }

                    // map webdata
                    HarvestSpeciesObservation harvestSpeciesObservation = kulProcess.ProcessObservation(webData, mappings, context);

                    speciesObservationChange.CreatedSpeciesObservations.Add(harvestSpeciesObservation);

                    if (decimal.Remainder(++i, 1000) != 0)
                    {
                        continue;
                    }

                    // write every 10000 observation to database to avoid memory problems
                    connectorServer.UpdateSpeciesObservations(context, speciesObservationChange.CreatedSpeciesObservations, dataProvider, out noOfCreated, out noOfCreatedErrors);

                    sumNoOfCreated += noOfCreated;
                    sumNoOfCreatedErrors += noOfCreatedErrors;
                    speciesObservationChange.CreatedSpeciesObservations.Clear();
                }

                connectorServer.UpdateSpeciesObservations(context, speciesObservationChange.CreatedSpeciesObservations, dataProvider, out noOfCreated, out noOfCreatedErrors);

                sumNoOfCreated += noOfCreated;
                sumNoOfCreatedErrors += noOfCreatedErrors;
                speciesObservationChange.CreatedSpeciesObservations.Clear();
                */

                // UPDATED
                int i = 0;
                int noOfUpdated, noOfUpdatedErrors;
                foreach (Proxy.KulService.WebSpeciesObservation kulSpeciesObservation in webSpeciesObservationChange.UpdatedSpeciesObservations)
                {
                    WebData webData = new WebData { DataFields = new List<WebDataField>() };

                    foreach (WebSpeciesObservationField webSpeciesObservationField in kulSpeciesObservation.Fields)
                    {
                        WebDataField webDataField = new WebDataField
                                                        {
                                                            Information = webSpeciesObservationField.Information,
                                                            Name = webSpeciesObservationField.Property.Id.ToString(),
                                                            Type = (WebDataType)webSpeciesObservationField.Type,
                                                            Unit = webSpeciesObservationField.Unit,
                                                            Value = webSpeciesObservationField.Value.CheckInjection()
                                                        };

                        if (webDataField.IsNotNull())
                        {
                            webData.DataFields.Add(webDataField);
                        }
                    }

                    // map webdata
                    HarvestSpeciesObservation harvestSpeciesObservation = kulProcess.ProcessObservation(webData, mappings, context);

                    speciesObservationChange.UpdatedSpeciesObservations.Add(harvestSpeciesObservation);

                    if (decimal.Remainder(++i, 1000) != 0)
                    {
                        continue;
                    }

                    // write every 10000 observation to database to avoid memory problems
                    connectorServer.UpdateSpeciesObservations(context, speciesObservationChange.UpdatedSpeciesObservations, dataProvider, out noOfUpdated, out noOfUpdatedErrors);

                    sumNoOfUpdated += noOfUpdated;
                    sumNoOfUpdatedErrors += noOfUpdatedErrors;
                    speciesObservationChange.UpdatedSpeciesObservations.Clear();
                }

                connectorServer.UpdateSpeciesObservations(context, speciesObservationChange.UpdatedSpeciesObservations, dataProvider, out noOfUpdated, out noOfUpdatedErrors);

                sumNoOfCreated += noOfUpdated;
                sumNoOfCreatedErrors += noOfUpdatedErrors;
                speciesObservationChange.UpdatedSpeciesObservations.Clear();

                // DELETED
                this.RemoveDeletedSpeciesObservations(webSpeciesObservationChange, speciesObservationChange, kulProcess, mappings, context, connectorServer, dataProvider, out sumNoOfDeleted, out sumNoOfDeletedErrors);

                /*
                i = 0;
                int noOfDeleted = 0, noOfDeletedErrors = 0;
                speciesObservationChange.DeletedSpeciesObservationGuids = new List<String>();

                foreach (String kulSpeciesObservation in webSpeciesObservationChange.DeletedSpeciesObservationGuids)
                {
                    // if (adsSpeciesObservation.DatabaseId == 99) continue;
                    String id = kulSpeciesObservation.Substring(kulSpeciesObservation.LastIndexOf('.') + 1);
                    speciesObservationChange.DeletedSpeciesObservationGuids.Add(id);

                    if (decimal.Remainder(++i, 1000) != 0)
                    {
                        continue;
                    }

                    // write every 10000 observation to database to avoid memory problems
                    connectorServer.DeleteSpeciesObservations(
                        context,
                        speciesObservationChange.DeletedSpeciesObservationGuids,
                        dataProvider,
                        out noOfDeleted,
                        out noOfDeletedErrors);
                    sumNoOfDeleted += noOfDeleted;
                    sumNoOfDeletedErrors += noOfDeletedErrors;
                    speciesObservationChange.DeletedSpeciesObservationGuids.Clear();
                }

                // write remaining observations to database
                connectorServer.DeleteSpeciesObservations(
                    context,
                    speciesObservationChange.DeletedSpeciesObservationGuids,
                    dataProvider,
                    out noOfDeleted,
                    out noOfDeletedErrors);
                sumNoOfDeleted += noOfDeleted;
                sumNoOfDeletedErrors += noOfDeletedErrors;
                speciesObservationChange.DeletedSpeciesObservationGuids.Clear();
                */
                // HANDLE LOOP OVER CHANGEID           
                webSpeciesObservationChange = WebServiceProxy.KulService.GetSpeciesObservationChangeAsSpecies(changedFrom,
                                                                                                              isChangedFromSpecified,
                                                                                                              changedTo,
                                                                                                              isChangedToSpecified,
                                                                                                              webSpeciesObservationChange.MaxChangeId + 1,
                                                                                                              isChangeIdspecified,
                                                                                                              maxReturnedChanges);
                if (decimal.Remainder(++j, 10) != 0)
                {
                    continue;
                }

                Debug.WriteLine(webSpeciesObservationChange.MaxChangeId + " : count created: " + sumNoOfCreated);
            }

            Debug.WriteLine("end :" + webSpeciesObservationChange.MaxChangeId + " : count created: " + sumNoOfCreated);

            // Log latest harvest date for the data provider
            // Use changeFrom since changedTo is calculated (+1 day)
            context.GetSpeciesObservationDatabase().SetDataProviderLatestHarvestDate(dataProvider.Id, changedFrom);

            context.GetSpeciesObservationDatabase()
               .LogHarvestRead(context, dataProvider, changedFrom, changedTo, stopwatch.ElapsedMilliseconds, sumNoOfCreated, sumNoOfCreatedErrors, sumNoOfUpdated, sumNoOfUpdatedErrors, sumNoOfDeleted, sumNoOfDeletedErrors, currentChangeId); 
            stopwatch.Stop();
        }

        /// <summary>
        /// Get species observation data source for this connector.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Species observation data source for this connector.</returns>
        public WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context)
        {
            return new Data.SpeciesObservationManager().GetSpeciesObservationDataProvider(context, SpeciesObservationDataProviderId.Kul);
        }

        /// <summary> The add new species observations. </summary>
        /// <param name="webSpeciesObservationChange"> The web species observation change. </param>
        /// <param name="speciesObservationChange"> The species observation change. </param>
        /// <param name="kulProcess"> The kul Process. </param>
        /// <param name="mappings"> The mappings. </param>
        /// <param name="context"> The context. </param>
        /// <param name="connectorServer"> The connector Server. </param>
        /// <param name="dataProvider"> The data Provider. </param>
        /// <param name="sumNoOfCreated"> The sum No Of Created. </param>
        /// <param name="sumNoOfCreatedErrors"> The sum No Of Created Errors. </param>
        private void AddNewSpeciesObservations(
            WebSpeciesObservationChange webSpeciesObservationChange,
            SpeciesObservationChange speciesObservationChange,
            KulProcess kulProcess, 
            List<HarvestMapping> mappings,
            WebServiceContext context,
            IConnectorServer connectorServer,
            WebSpeciesObservationDataProvider dataProvider,
            out int sumNoOfCreated,
            out int sumNoOfCreatedErrors)
        {
            int observationCount = 0;
            int noOfCreated, noOfCreatedErrors;
            sumNoOfCreated = 0;
            sumNoOfCreatedErrors = 0;
            
            foreach (Proxy.KulService.WebSpeciesObservation kulSpeciesObservation in webSpeciesObservationChange.CreatedSpeciesObservations)
            {
                WebData webData = new WebData { DataFields = new List<WebDataField>() };

                foreach (WebSpeciesObservationField webSpeciesObservationField in kulSpeciesObservation.Fields)
                {
                    WebDataField webDataField = new WebDataField
                    {
                        Information = webSpeciesObservationField.Information,
                        Name = webSpeciesObservationField.Property.Id.ToString(),
                        Type = (WebDataType)webSpeciesObservationField.Type,
                        Unit = webSpeciesObservationField.Unit,
                        Value = webSpeciesObservationField.Value.CheckInjection()
                    };
                    
                        webData.DataFields.Add(webDataField);
               }

                // map webdata
                HarvestSpeciesObservation harvestSpeciesObservation = kulProcess.ProcessObservation(webData, mappings, context);

                speciesObservationChange.CreatedSpeciesObservations.Add(harvestSpeciesObservation);

                if (decimal.Remainder(++observationCount, 1000) != 0)
                {
                    continue;
                }

                // write every 10000 observation to database to avoid memory problems
                connectorServer.UpdateSpeciesObservations(context, speciesObservationChange.CreatedSpeciesObservations, dataProvider, out noOfCreated, out noOfCreatedErrors);

                sumNoOfCreated += noOfCreated;
                sumNoOfCreatedErrors += noOfCreatedErrors;
                speciesObservationChange.CreatedSpeciesObservations.Clear();
            }

            connectorServer.UpdateSpeciesObservations(context, speciesObservationChange.CreatedSpeciesObservations, dataProvider, out noOfCreated, out noOfCreatedErrors);

            sumNoOfCreated += noOfCreated;
            sumNoOfCreatedErrors += noOfCreatedErrors;
            speciesObservationChange.CreatedSpeciesObservations.Clear();
        }

        /// <summary> The remove deleted species observations. </summary>
        /// <param name="webSpeciesObservationChange"> The web species observation change. </param>
        /// <param name="speciesObservationChange"> The species observation change. </param>
        /// <param name="kulProcess"> The kul process. </param>
        /// <param name="mappings"> The mappings. </param>
        /// <param name="context"> The context. </param>
        /// <param name="connectorServer"> The connector server. </param>
        /// <param name="dataProvider"> The data provider. </param>
        /// <param name="sumNoOfDeleted"> The sum No Of Deleted. </param>
        /// <param name="sumNoOfDeletedErrors"> The sum No Of Deleted Errors. </param>
        private void RemoveDeletedSpeciesObservations(
            WebSpeciesObservationChange webSpeciesObservationChange,
            SpeciesObservationChange speciesObservationChange,
            KulProcess kulProcess,
            List<HarvestMapping> mappings,
            WebServiceContext context,
            IConnectorServer connectorServer,
            WebSpeciesObservationDataProvider dataProvider,
            out int sumNoOfDeleted,
            out int sumNoOfDeletedErrors)
        {
            sumNoOfDeleted = 0;
            sumNoOfDeletedErrors = 0;
            int observationCount = 0;
            int noOfDeleted = 0, noOfDeletedErrors = 0;
            speciesObservationChange.DeletedSpeciesObservationGuids = new List<String>();

            foreach (String kulSpeciesObservation in webSpeciesObservationChange.DeletedSpeciesObservationGuids)
            {
                // if (adsSpeciesObservation.DatabaseId == 99) continue;
                String id = kulSpeciesObservation.Substring(kulSpeciesObservation.LastIndexOf('.') + 1);
                speciesObservationChange.DeletedSpeciesObservationGuids.Add(id);

                if (decimal.Remainder(++observationCount, 1000) != 0)
                {
                    continue;
                }

                // write every 10000 observation to database to avoid memory problems
                connectorServer.DeleteSpeciesObservations(
                    context,
                    speciesObservationChange.DeletedSpeciesObservationGuids,
                    dataProvider,
                    out noOfDeleted,
                    out noOfDeletedErrors);
                sumNoOfDeleted += noOfDeleted;
                sumNoOfDeletedErrors += noOfDeletedErrors;
                speciesObservationChange.DeletedSpeciesObservationGuids.Clear();
            }

            // write remaining observations to database
            connectorServer.DeleteSpeciesObservations(
                context,
                speciesObservationChange.DeletedSpeciesObservationGuids,
                dataProvider,
                out noOfDeleted,
                out noOfDeletedErrors);
            sumNoOfDeleted += noOfDeleted;
            sumNoOfDeletedErrors += noOfDeletedErrors;
            speciesObservationChange.DeletedSpeciesObservationGuids.Clear();
        }
    }
}

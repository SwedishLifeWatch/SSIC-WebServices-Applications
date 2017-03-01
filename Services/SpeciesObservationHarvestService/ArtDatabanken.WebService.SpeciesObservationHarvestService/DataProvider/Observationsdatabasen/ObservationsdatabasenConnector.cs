using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Observationsdatabasen
{
    /// <summary>
    /// Get species observations for Observationsdatabasen.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class ObservationsdatabasenConnector : IDataProviderConnector
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
            DateTime calcChangedTo = changedTo.AddDays(1);
            calcChangedTo = new DateTime(calcChangedTo.Year, calcChangedTo.Month, calcChangedTo.Day);

            WebData webData;
            SpeciesObservationChange speciesObservationChange = new SpeciesObservationChange();

            WebSpeciesObservationDataProvider dataProvider = GetSpeciesObservationDataProvider(context);

            WebSpeciesObservationClass speciesObservationClass = new WebSpeciesObservationClass(SpeciesObservationClassId.DarwinCore);

            WebSpeciesObservationProperty reportedDateProp = new WebSpeciesObservationProperty(SpeciesObservationPropertyId.ReportedDate);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int sumNoOfCreated = 0, sumNoOfCreatedErrors = 0,
                sumNoOfUpdated = 0, sumNoOfUpdatedErrors = 0,
                sumNoOfDeleted = 0, sumNoOfDeletedErrors = 0;

            using (ObservationsdatabasenServer observationDatabasenServer = new ObservationsdatabasenServer())
            {
                ObservationsdatabasenProcess observationsdatabasenProcess = new ObservationsdatabasenProcess();

                // Get created and edited observations from Observationsdatabasen
                using (DataReader dataReader = observationDatabasenServer.GetSpeciesObservations(changedFrom, calcChangedTo))
                {
                    speciesObservationChange.CreatedSpeciesObservations = new List<HarvestSpeciesObservation>();
                    speciesObservationChange.UpdatedSpeciesObservations = new List<HarvestSpeciesObservation>();
                    int i = 0;
                    int noOfCreated, noOfCreatedErrors, noOfUpdated, noOfUpdatedErrors;

                    while (dataReader.Read())
                    {
                        webData = new WebData();
                        webData.LoadData(dataReader);
                        HarvestSpeciesObservation harvestSpeciesObservation = observationsdatabasenProcess.ProcessObservation(webData, mappings, context);

                        DateTime reportedDate = harvestSpeciesObservation.GetFieldValue(speciesObservationClass, reportedDateProp).WebParseDateTime();
                        
                        // If reportedDate is earlier than changedFrom the observation is edited.
                        if (reportedDate < changedFrom)
                        {
                            speciesObservationChange.UpdatedSpeciesObservations.Add(harvestSpeciesObservation);
                        }
                        else
                        {
                            speciesObservationChange.CreatedSpeciesObservations.Add(harvestSpeciesObservation);
                        }

                        if (decimal.Remainder(++i, 10000) == 0)
                        {
                            // Write every 10000 observation to database to avoid memory problems
                            connectorServer.UpdateSpeciesObservations(context, speciesObservationChange.CreatedSpeciesObservations, dataProvider, out noOfCreated, out noOfCreatedErrors);

                            connectorServer.UpdateSpeciesObservations(context, speciesObservationChange.UpdatedSpeciesObservations, dataProvider, out noOfUpdated, out noOfUpdatedErrors);

                            sumNoOfCreated += noOfCreated;
                            sumNoOfCreatedErrors += noOfCreatedErrors;
                            sumNoOfUpdated += noOfUpdated;
                            sumNoOfUpdatedErrors += noOfUpdatedErrors;

                            speciesObservationChange.CreatedSpeciesObservations.Clear();
                            speciesObservationChange.UpdatedSpeciesObservations.Clear();
                        }
                    }

                    // Write the remaining observations to database
                    connectorServer.UpdateSpeciesObservations(context, speciesObservationChange.CreatedSpeciesObservations, dataProvider, out noOfCreated, out noOfCreatedErrors);

                    connectorServer.UpdateSpeciesObservations(context, speciesObservationChange.UpdatedSpeciesObservations, dataProvider, out noOfUpdated, out noOfUpdatedErrors);

                    sumNoOfCreated += noOfCreated;
                    sumNoOfCreatedErrors += noOfCreatedErrors;
                    sumNoOfUpdated += noOfUpdated;
                    sumNoOfUpdatedErrors += noOfUpdatedErrors;

                    speciesObservationChange.CreatedSpeciesObservations.Clear();
                    speciesObservationChange.UpdatedSpeciesObservations.Clear();
                }
                
                // Get deleted observations from Observationsdatabasen
                using (DataReader dataReader = observationDatabasenServer.GetDeletedObservations(changedFrom, calcChangedTo))
                {
                    speciesObservationChange.DeletedSpeciesObservationGuids = new List<String>();
                    int noOfDeleted, noOfDeletedErrors;
                    int i = 0;
                    while (dataReader.Read())
                    {
                        webData = new WebData();
                        webData.LoadData(dataReader);
                        String deletedCatalogNumber = observationsdatabasenProcess.ProcessDeletedObservation(webData);
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
            stopwatch.Stop();
            return false;
        }

        /// <summary>
        /// Get species observation data source for this connector.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Species observation data source for this connector.</returns>
        public WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context)
        {
            return new Data.SpeciesObservationManager().GetSpeciesObservationDataProvider(context, SpeciesObservationDataProviderId.Observationsdatabasen);
        }
    }
}

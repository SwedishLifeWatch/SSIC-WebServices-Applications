using System;
using System.Collections.Generic;
using System.Diagnostics;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using SpeciesObservationPropertyId = ArtDatabanken.WebService.Proxy.WramService.SpeciesObservationPropertyId;
using WebSpeciesObservationChange = ArtDatabanken.WebService.Proxy.WramService.WebSpeciesObservationChange;
using WebSpeciesObservationField = ArtDatabanken.WebService.Proxy.WramService.WebSpeciesObservationField;
using WebSpeciesObservation = ArtDatabanken.WebService.Proxy.WramService.WebSpeciesObservation;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Wram
{
    /// <summary>
    /// The Wram connector.
    /// </summary>
    public class WramConnector : IDataProviderConnector
    {
        /// <summary>
        /// Add updated species observations to species observation dictionary.
        /// Updated species observations that are already in the species
        /// observation dictionary are not added again.
        /// </summary>
        /// <param name="speciesObservations">Species observation dictionary.</param>
        /// <param name="updatedSpeciesObservations">Updated species observations</param>
        private void AddSpeciesObservations(Dictionary<String, WebSpeciesObservation> speciesObservations,
                                            List<WebSpeciesObservation> updatedSpeciesObservations)
        {
            String catalogNumber;

            if (updatedSpeciesObservations.IsNotEmpty())
            {
                for (Int32 index = updatedSpeciesObservations.Count - 1; index >= 0; index--)
                {
                    catalogNumber = GetCatalogNumber(updatedSpeciesObservations[index].Fields);
                    if (!(speciesObservations.ContainsKey(catalogNumber)))
                    {
                        speciesObservations[catalogNumber] = updatedSpeciesObservations[index];
                    }

                    // else: Species observation has already been added to change set.
                }
            }
        }

        /// <summary>
        /// Get catalog number from fields.
        /// </summary>
        /// <param name="fields">Fields in one species observation.</param>
        /// <returns>Catalog number from fields.</returns>
        private String GetCatalogNumber(List<WebSpeciesObservationField> fields)
        {
            String catalogNumber;

            catalogNumber = null;
            foreach (WebSpeciesObservationField field in fields)
            {
                if (field.Property.Id == SpeciesObservationPropertyId.CatalogNumber)
                {
                    catalogNumber = field.Value;
                    break;
                }
            }

            if (catalogNumber.IsEmpty())
            {
                throw new ArgumentException("Can't find catalog number in species observation from WRAM.");
            }

            return catalogNumber;
        }

        /// <summary>
        /// Get information about species observations that has changed.
        /// The service contains observations that are aggregated and has start
        /// and end dates that are set to the first and last day of every month.
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
            Dictionary<String, WebSpeciesObservation> updatedSpeciesObservations;
            SpeciesObservationChange speciesObservationChange = new SpeciesObservationChange();
            speciesObservationChange.CreatedSpeciesObservations = new List<HarvestSpeciesObservation>();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            WramProcess wramProcess = new WramProcess();
            WebSpeciesObservationDataProvider dataProvider = GetSpeciesObservationDataProvider(context);
            WebSpeciesObservationChange webSpeciesObservationChange = WebServiceProxy.WramService.GetSpeciesObservationChangeAsSpecies(DateTime.Now,
                                                                                                                                       false,
                                                                                                                                       DateTime.Now,
                                                                                                                                       false,
                                                                                                                                       dataProvider.MaxChangeId,
                                                                                                                                       true,
                                                                                                                                       Settings.Default.MaxSpeciesObservationsFromWram);

            // Run all created and updated together as one list.
            updatedSpeciesObservations = new Dictionary<String, WebSpeciesObservation>();
            AddSpeciesObservations(updatedSpeciesObservations, webSpeciesObservationChange.UpdatedSpeciesObservations);
            AddSpeciesObservations(updatedSpeciesObservations, webSpeciesObservationChange.CreatedSpeciesObservations);
            List<WebSpeciesObservation> createdAndUpdatedObservations = new List<WebSpeciesObservation>();
            createdAndUpdatedObservations.AddRange(updatedSpeciesObservations.Values);

            // Handle created and updated species observations.
            int noOfCreated = 0, noOfCreatedErrors = 0;
            if (createdAndUpdatedObservations.IsNotEmpty())
            {
                foreach (WebSpeciesObservation wramSpeciesObservation in createdAndUpdatedObservations)
                {
                    WebData webData = new WebData { DataFields = new List<WebDataField>() };
                    foreach (WebSpeciesObservationField webSpeciesObservationField in wramSpeciesObservation.Fields)
                    {
                        WebDataField webDataField = new WebDataField();
                        webDataField.LoadData(webSpeciesObservationField);
                        webData.DataFields.Add(webDataField);
                    }

                    // Map and add webdata to the 'created collection'
                    speciesObservationChange.CreatedSpeciesObservations.Add(wramProcess.ProcessObservation(webData, mappings, context));
                }

                connectorServer.UpdateSpeciesObservations(context, speciesObservationChange.CreatedSpeciesObservations, dataProvider, out noOfCreated, out noOfCreatedErrors);
            }

            // Handle deleted species observtions.
            int noOfDeleted = 0, noOfDeletedErrors = 0;
            speciesObservationChange.DeletedSpeciesObservationGuids = new List<String>();

            if (webSpeciesObservationChange.DeletedSpeciesObservationGuids.IsNotEmpty())
            {
                foreach (String wramSpeciesObservation in webSpeciesObservationChange.DeletedSpeciesObservationGuids)
                {
                    String id = wramSpeciesObservation.Substring(wramSpeciesObservation.LastIndexOf(':') + 1);
                    speciesObservationChange.DeletedSpeciesObservationGuids.Add(id);
                }

                connectorServer.DeleteSpeciesObservations(context, speciesObservationChange.DeletedSpeciesObservationGuids, dataProvider, out noOfDeleted, out noOfDeletedErrors);
            }

            Debug.WriteLine(webSpeciesObservationChange.MaxChangeId + " : count created: " + noOfCreated);

            context.GetSpeciesObservationDatabase().LogHarvestRead(
                    context,
                    dataProvider,
                    changedFrom,
                    changedTo,
                    stopwatch.ElapsedMilliseconds,
                    noOfCreated,
                    noOfCreatedErrors,
                    0,
                    0,
                    noOfDeleted,
                    noOfDeletedErrors,
                    webSpeciesObservationChange.MaxChangeId);
            stopwatch.Stop();

            if (webSpeciesObservationChange.MaxChangeId > 0)
            {
//                context.GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, webSpeciesObservationChange.MaxChangeId);

                // Testing if WRAM has a problem with handling of change id.
                context.GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, webSpeciesObservationChange.MaxChangeId - 1);
            }
            // else: No changes was retrieved. There are no more changes available right now.

            return false;
        }

        /// <summary>
        /// Get species observation data source for this connector.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Species observation data source for this connector.</returns>
        public WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context)
        {
            return new Data.SpeciesObservationManager().GetSpeciesObservationDataProvider(context, SpeciesObservationDataProviderId.Wram);
        }
    }
}

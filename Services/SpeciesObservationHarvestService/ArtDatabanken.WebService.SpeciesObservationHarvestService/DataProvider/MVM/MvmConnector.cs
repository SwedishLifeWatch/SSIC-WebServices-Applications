using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using SpeciesObservationPropertyId = ArtDatabanken.WebService.Proxy.MvmService.SpeciesObservationPropertyId;
using WebSpeciesObservationChange = ArtDatabanken.WebService.Proxy.MvmService.WebSpeciesObservationChange;
using WebSpeciesObservationField = ArtDatabanken.WebService.Proxy.MvmService.WebSpeciesObservationField;
using WebSpeciesObservation = ArtDatabanken.WebService.Proxy.MvmService.WebSpeciesObservation;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Mvm
{
    /// <summary>
    /// Connector to Mvm data provider.
    /// </summary>
    public class MvmConnector : IDataProviderConnector
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
                    if (IsPublic(updatedSpeciesObservations[index].Fields))
                    {
                        catalogNumber = GetCatalogNumber(updatedSpeciesObservations[index].Fields);
                        if (!(speciesObservations.ContainsKey(catalogNumber)))
                        {
                            speciesObservations[catalogNumber] = updatedSpeciesObservations[index];
                        }
                        // else: Species observation has already been added to change set.
                    }
                    // else: Do not include none public species observations.
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
                throw new ArgumentException("Can't find catalog number in species observation from MVM.");
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
            Int32 minutes;
            Int64 maxChangeId;

            // Wait for MVM service to be ready for use.
            for (minutes = 0; minutes < 60; minutes++)
            {
                if (WebServiceProxy.MvmService.IsReadyToUse())
                {
                    break;
                }
                else
                {
                    // Wait one minute for MVM service to be ready for use.
                    Thread.Sleep(60000);
                }
            }

            if ((minutes == 60) && !(WebServiceProxy.MvmService.IsReadyToUse()))
            {
                // Can not wait any longer for MVM service to be ready.
                return false;
            }

                SpeciesObservationChange speciesObservationChange = new SpeciesObservationChange();
            speciesObservationChange.CreatedSpeciesObservations = new List<HarvestSpeciesObservation>();
            WebSpeciesObservationChange webSpeciesObservationChange;
            webSpeciesObservationChange = new WebSpeciesObservationChange();
            webSpeciesObservationChange.CreatedSpeciesObservations = new List<WebSpeciesObservation>();
            webSpeciesObservationChange.DeletedSpeciesObservationGuids = new List<String>();
            webSpeciesObservationChange.UpdatedSpeciesObservations = new List<WebSpeciesObservation>();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            MvmProcess mvmProcess = new MvmProcess();
            WebSpeciesObservationDataProvider dataProvider = GetSpeciesObservationDataProvider(context);
            maxChangeId = dataProvider.MaxChangeId;
            for (Int32 index = 0; index < 10; index++)
            {
                WebSpeciesObservationChange webSpeciesObservationChangeTemp = WebServiceProxy.MvmService.GetSpeciesObservationChangeAsSpecies(DateTime.Now,
                                                                                                                                          false,
                                                                                                                                          DateTime.Now,
                                                                                                                                          false,
                                                                                                                                          maxChangeId,
                                                                                                                                          true,
                                                                                                                                          Settings.Default.MaxSpeciesObservationsFromMvm);
                if (webSpeciesObservationChangeTemp.CreatedSpeciesObservations.IsNotEmpty())
                {
                    webSpeciesObservationChange.CreatedSpeciesObservations.AddRange(webSpeciesObservationChangeTemp.CreatedSpeciesObservations);
                }

                if (webSpeciesObservationChangeTemp.DeletedSpeciesObservationGuids.IsNotEmpty())
                {
                    webSpeciesObservationChange.DeletedSpeciesObservationGuids.AddRange(webSpeciesObservationChangeTemp.DeletedSpeciesObservationGuids);
                }

                if (webSpeciesObservationChangeTemp.UpdatedSpeciesObservations.IsNotEmpty())
                {
                    webSpeciesObservationChange.UpdatedSpeciesObservations.AddRange(webSpeciesObservationChangeTemp.UpdatedSpeciesObservations);
                }

                if (maxChangeId < webSpeciesObservationChangeTemp.MaxChangeId)
                {
                    maxChangeId = webSpeciesObservationChangeTemp.MaxChangeId;
                }
                else
                {
                    // No more species observation changes are available.
                    break;
                }
            }

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
                foreach (WebSpeciesObservation mvmSpeciesObservation in createdAndUpdatedObservations)
                {
                    WebData webData = new WebData { DataFields = new List<WebDataField>() };
                    foreach (WebSpeciesObservationField webSpeciesObservationField in mvmSpeciesObservation.Fields)
                    {
                        WebDataField webDataField = new WebDataField();
                        webDataField.LoadData(webSpeciesObservationField);
                        webData.DataFields.Add(webDataField);
                    }

                    // Map and add webdata to the 'created collection'
                    speciesObservationChange.CreatedSpeciesObservations.Add(mvmProcess.ProcessObservation(webData, mappings, context));
                }

                connectorServer.UpdateSpeciesObservations(context, speciesObservationChange.CreatedSpeciesObservations, dataProvider, out noOfCreated, out noOfCreatedErrors);
            }

            // Handle deleted species observtions.
            int noOfDeleted = 0, noOfDeletedErrors = 0;
            speciesObservationChange.DeletedSpeciesObservationGuids = new List<String>();

            if (webSpeciesObservationChange.DeletedSpeciesObservationGuids.IsNotEmpty())
            {
                foreach (String mvmSpeciesObservation in webSpeciesObservationChange.DeletedSpeciesObservationGuids)
                {
                    String id = mvmSpeciesObservation.Substring(mvmSpeciesObservation.LastIndexOf(':') + 1);
                    speciesObservationChange.DeletedSpeciesObservationGuids.Add(id);
                }

                connectorServer.DeleteSpeciesObservations(context, speciesObservationChange.DeletedSpeciesObservationGuids, dataProvider, out noOfDeleted, out noOfDeletedErrors);
            }

            Debug.WriteLine(maxChangeId + " : count created: " + noOfCreated);

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
                    maxChangeId);
            stopwatch.Stop();

            if (maxChangeId > 0)
            {
                context.GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, maxChangeId);
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
            return new Data.SpeciesObservationManager().GetSpeciesObservationDataProvider(context, SpeciesObservationDataProviderId.Mvm);
        }

        /// <summary>
        /// Test if species observation is public.
        /// </summary>
        /// <param name="fields">Fields in one species observation.</param>
        /// <returns>True, if species observation is public.</returns>
        private Boolean IsPublic(List<WebSpeciesObservationField> fields)
        {
            Boolean isPublic;

            isPublic = true;
            foreach (WebSpeciesObservationField field in fields)
            {
                if (field.Property.Id == SpeciesObservationPropertyId.IsPublic)
                {
                    isPublic = field.Value.WebParseBoolean();
                    break;
                }
            }

            return isPublic;
        }
    }
}

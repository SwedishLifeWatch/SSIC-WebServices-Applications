using System;
using System.Collections.Generic;
using System.Data;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider
{
    /// <summary>
    /// Handles operations on species observation data.
    /// </summary>
    public class ConnectorServer : IConnectorServer
    {
        public const string CREATE = "CREATE";
        public const string UPDATE = "UPDATE";
        public const string DEFAULT = "DEFAULT";

        /// <summary>
        /// Update species observations.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservations">Updated species observations.</param>
        /// <param name="dataProvider">The dataProvider.</param>
        /// <param name="noOfUpdated">No of updated species observations.</param>
        /// <param name="noOfUpdatedErrors">No of updating errors.</param>
        void IConnectorServer.UpdateSpeciesObservations(WebServiceContext context, List<HarvestSpeciesObservation> speciesObservations, WebSpeciesObservationDataProvider dataProvider, out int noOfUpdated, out int noOfUpdatedErrors)
        {
            UpdateSpeciesObservations(context, speciesObservations, dataProvider, out noOfUpdated, out noOfUpdatedErrors);
        }

        /// <summary>
        /// Insert all deleted observations into a DataTable 
        /// If ok, add it to TempDelete
        /// If there are errors add the observation to speciesObservationErrorFieldTable.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationGuids">GUIDs for deleted species observations.</param>
        /// <param name="dataProvider">The dataProvider.</param>
        /// <param name="noOfDeleted">No of deleted species observations.</param>
        /// <param name="noOfErrors">No of deleting errors.</param>
        void IConnectorServer.DeleteSpeciesObservations(WebServiceContext context, List<string> speciesObservationGuids, WebSpeciesObservationDataProvider dataProvider, out int noOfDeleted, out int noOfErrors)
        {
            DeleteSpeciesObservations(context, speciesObservationGuids, dataProvider, out noOfDeleted, out noOfErrors);
        }

        ////void IConnectorServer.CreateSpeciesObservations(WebServiceContext context, List<HarvestSpeciesObservation> speciesObservations, WebSpeciesObservationDataProvider dataProvider, out int noOfCreated, out int noOfErrors)
        ////{
        ////    throw new NotImplementedException();
        ////    //   CreateSpeciesObservations(context, speciesObservations, dataProvider, out noOfCreated, out noOfErrors);
        ////}
        ///// <summary>
        ///// Insert all new (created) observations into a DataTable 
        ///// First check the observation
        ///// If ok, add it to SpeciesObservationTable and DarwinCoreTable
        ///// If there are errors add the observation to speciesObservationErrorFieldTable
        ///// </summary>
        ///// <param name="context">Web service request context.</param>
        ///// <param name="speciesObservations">Created species observations.</param>
        ///// <param name="dataProvider">Species observation data source.</param>
        ///// <param name="noOfCreated">Created </param>
        ///// <param name="noOfErrors">Errors </param>
        ////public static void CreateSpeciesObservations(WebServiceContext context,
        ////                                              List<HarvestSpeciesObservation> speciesObservations,
        ////                                              WebSpeciesObservationDataProvider dataProvider,
        ////                                              out int noOfCreated, out int noOfErrors)
        ////{
        ////    noOfCreated = 0;
        ////    noOfErrors = 0;
        ////    if (speciesObservations.IsNotEmpty())
        ////    {
        ////        DataTable darwinCoreTable = HarvestManager.GetDarwinCoreTable(CREATE);

        ////        DataTable speciesObservationFieldTable = HarvestManager.GetSpeciesObservationFieldTable(CREATE);
        ////        DataTable speciesObservationErrorFieldTable = HarvestManager.GetSpeciesObservationErrorFieldTable();

        ////        // Save all species observations into tables.
        ////        foreach (HarvestSpeciesObservation speciesObservation in speciesObservations)
        ////        {
        ////            string catalogNumber;
        ////            Dictionary<SpeciesObservationPropertyId, string> errors;
        ////            if (HarvestManager.CheckSpeciesObservation(speciesObservation, context, out errors, out catalogNumber))
        ////            {
        ////              //  HarvestManager.SpeciesObservationId++;
        ////                HarvestManager.AddToTempSpeciesObservation(speciesObservation,
        ////                                            darwinCoreTable,
        ////                                            speciesObservationFieldTable,
        ////                                            HarvestManager.SpeciesObservationId,
        ////                                            dataProvider,
        ////                                            catalogNumber);
        ////                noOfCreated++;
        ////            }
        ////            else
        ////            {
        ////                HarvestManager.CreateSpeciesObservationError(speciesObservation,
        ////                                              speciesObservationErrorFieldTable,
        ////                                              dataProvider,
        ////                                              errors,
        ////                                              CREATE);
        ////                noOfErrors++;
        ////            }
        ////        }

        ////        // Save tables to database.
        ////        context.GetSpeciesObservationDatabase().AddTableData(speciesObservationFieldTable);
        ////        context.GetSpeciesObservationDatabase().AddTableData(darwinCoreTable);
        ////        if (noOfErrors > 0)
        ////            context.GetSpeciesObservationDatabase().AddTableData(speciesObservationErrorFieldTable);
        ////    }

        ////}

        /// <summary>
        /// Insert all changed (updated) observations into a DataTable 
        /// First check the observation
        /// If ok, add it to SpeciesObservationUpdateTable and DarwinCoreUpdateTable
        /// If there are errors add the observation to speciesObservationErrorFieldTable.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservations">Updated species observations.</param>
        /// <param name="dataProvider">The dataProvider.</param>
        /// <param name="noOfUpdated">No of updated species observations.</param>
        /// <param name="noOfUpdatedErrors">No of updating errors.</param>
        public static void UpdateSpeciesObservations(WebServiceContext context,
                                                     List<HarvestSpeciesObservation> speciesObservations,
                                                     WebSpeciesObservationDataProvider dataProvider,
                                                     out int noOfUpdated,
                                                     out int noOfUpdatedErrors)
        {
            noOfUpdated = 0;
            noOfUpdatedErrors = 0;

            if (!speciesObservations.IsNotEmpty())
            {
                return;
            }

            DataTable darwinCoreTable = HarvestManager.GetDarwinCoreTable(HarvestManager.UPDATE);
            DataTable speciesObservationFieldTable = HarvestManager.GetSpeciesObservationFieldTable(HarvestManager.UPDATE);
            DataTable speciesObservationErrorFieldTable = HarvestManager.GetSpeciesObservationErrorFieldTable();

            // Save all species observations into tables.
            foreach (HarvestSpeciesObservation speciesObservation in speciesObservations)
            {
                string catalogNumber;
                Boolean someError = true;
                Dictionary<SpeciesObservationPropertyId, string> errors;
                if (HarvestManager.CheckSpeciesObservation(speciesObservation, context, out errors, out catalogNumber))
                {
                    try
                    {
                        HarvestManager.AddToTempSpeciesObservation(speciesObservation,
                                                                   darwinCoreTable,
                                                                   speciesObservationFieldTable,
                                                                   -1,
                                                                   dataProvider,
                                                                   catalogNumber);
                        noOfUpdated++;
                        someError = false;
                    }
                    catch (Exception ex)
                    {
                        var prop = (SpeciesObservationPropertyId)Enum.Parse(typeof(SpeciesObservationPropertyId), ex.Message);
                        if (prop.IsNotNull())
                        {
                            errors.Add(prop, ex.InnerException.Message);
                        }
                        else
                        {
                            errors.Add(SpeciesObservationPropertyId.Id, ex.InnerException.ToString());
                        }
                    }
                }

                if (someError)
                {
                    HarvestManager.CreateSpeciesObservationError(speciesObservation,
                                                                 speciesObservationErrorFieldTable,
                                                                 dataProvider,
                                                                 errors,
                                                                 HarvestManager.UPDATE);
                    noOfUpdatedErrors++;
                }
            }

            // Save tables to database.
            context.GetSpeciesObservationDatabase().AddTableData(speciesObservationFieldTable);
            context.GetSpeciesObservationDatabase().AddTableData(darwinCoreTable);
            if (noOfUpdatedErrors > 0)
            {
                context.GetSpeciesObservationDatabase().AddTableData(speciesObservationErrorFieldTable);
            }
        }

        /// <summary>
        /// Insert all deleted observations into a DataTable 
        /// If ok, add it to TempDelete
        /// If there are errors add the observation to speciesObservationErrorFieldTable.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationGuids">GUIDs for deleted species observations.</param>
        /// <param name="dataProvider">The dataProvider.</param>
        /// <param name="noOfDeleted">No of deleted species observations.</param>
        /// <param name="noOfErrors">No of deleting errors.</param>
        public static void DeleteSpeciesObservations(WebServiceContext context,
                                                     List<String> speciesObservationGuids,
                                                     WebSpeciesObservationDataProvider dataProvider,
                                                     out int noOfDeleted, 
                                                     out int noOfErrors)
        {
            noOfDeleted = 0;
            noOfErrors = 0;
            if (speciesObservationGuids.IsNotEmpty())
            {
                DataTable deletedObservationTable = HarvestManager.GetDeletedObservationTable();

                foreach (string speciesObservation in speciesObservationGuids)
                {
                    HarvestManager.AddToTempDelete(deletedObservationTable, dataProvider, speciesObservation);
                    noOfDeleted++;
                }

                context.GetSpeciesObservationDatabase().AddTableData(deletedObservationTable);
            }
        }
    }
}

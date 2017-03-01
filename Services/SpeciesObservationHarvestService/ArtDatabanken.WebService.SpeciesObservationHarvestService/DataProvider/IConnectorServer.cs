using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider
{
    /// <summary>
    /// Used to implement Update and Delete methods for species observations.
    /// </summary>
    public interface IConnectorServer
    {
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
        ////void CreateSpeciesObservations(WebServiceContext context,
        ////                                             List<HarvestSpeciesObservation> speciesObservations,
        ////                                             WebSpeciesObservationDataProvider dataProvider,
        ////                                             out int noOfCreated, out int noOfErrors);

        /// <summary>
        /// Insert all changed (updated) observations into a DataTable 
        /// First check the observation
        /// If ok, add it to SpeciesObservationUpdateTable and DarwinCoreUpdateTable
        /// If there are errors add the observation to speciesObservationErrorFieldTable.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservations">Updated species observations.</param>
        /// <param name="dataProvider">Species observation data source.</param>
        /// <param name="noOfUpdated">No of updated observations.</param>
        /// <param name="noOfUpdatedErrors">No of updating errors.</param>
        void UpdateSpeciesObservations(WebServiceContext context,
                                       List<HarvestSpeciesObservation> speciesObservations,
                                       WebSpeciesObservationDataProvider dataProvider,
                                       out int noOfUpdated,
                                       out int noOfUpdatedErrors);

        /// <summary>
        /// Insert all deleted observations into a DataTable 
        /// If ok, add it to TempDelete
        /// If there are errors add the observation to speciesObservationErrorFieldTable.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationGuids">GUIDs for deleted species observations.</param>
        /// <param name="dataProvider">Species observation data source.</param>
        /// <param name="noOfDeleted">No of deleted observations.</param>
        /// <param name="noOfErrors">No of deleting errors.</param>
        void DeleteSpeciesObservations(WebServiceContext context,
                                       List<String> speciesObservationGuids,
                                       WebSpeciesObservationDataProvider dataProvider,
                                       out int noOfDeleted, 
                                       out int noOfErrors);
    }
}

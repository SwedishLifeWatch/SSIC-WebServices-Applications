using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider
{
    /// <summary>
    /// Interface that connectors to data providers in swedish
    /// LifeWatch must implement.
    /// </summary>
    public interface IDataProviderConnector
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
        /// <param name="context">The context.</param>
        /// <param name="connectorServer">The connector server.</param>
        /// <returns>
        /// Returns true if there are more species
        /// observations to retrieve for current date.
        /// </returns>
        Boolean GetSpeciesObservationChange(DateTime changedFrom,
                                            DateTime changedTo,
                                            List<HarvestMapping> mappings,
                                            WebServiceContext context,
                                            IConnectorServer connectorServer);

        /// <summary>
        /// Get species observation data provider for this connector.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Species observation data provider for this connector.</returns>
        WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context);
    }
}

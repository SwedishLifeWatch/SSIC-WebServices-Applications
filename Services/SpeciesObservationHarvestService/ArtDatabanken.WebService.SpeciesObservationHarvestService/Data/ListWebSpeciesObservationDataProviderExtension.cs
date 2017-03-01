using System.Collections.Generic;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Data
{
    /// <summary>
    /// Contains extension methods to a generic list of type WebSpeciesObservationDataProvider.
    /// </summary>
    public static class ListWebSpeciesObservationDataProviderExtension
    {
        /// <summary>
        /// Get data providers that use change id.
        /// </summary>
        /// <param name="dataProviders">Species observation data providers.</param>
        /// <returns>Data providers that use change id.</returns>
        public static List<WebSpeciesObservationDataProvider> GetDataProvidersThatUseChangeId(this List<WebSpeciesObservationDataProvider> dataProviders)
        {
            List<WebSpeciesObservationDataProvider> dataProvidersThatUseChangeId;

            dataProvidersThatUseChangeId = new List<WebSpeciesObservationDataProvider>();
            if (dataProviders.IsNotEmpty())
            {
                foreach (WebSpeciesObservationDataProvider dataProvider in dataProviders)
                {
                    if (dataProvider.IsMaxChangeIdSpecified)
                    {
                        dataProvidersThatUseChangeId.Add(dataProvider);
                    }
                }
            }

            return dataProvidersThatUseChangeId;
        }

        /// <summary>
        /// Get data providers that use date.
        /// </summary>
        /// <param name="dataProviders">Species observation data providers.</param>
        /// <returns>Data providers that use date.</returns>
        public static List<WebSpeciesObservationDataProvider> GetDataProvidersThatUseDate(this List<WebSpeciesObservationDataProvider> dataProviders)
        {
            List<WebSpeciesObservationDataProvider> getDataProvidersThatUseDate;

            getDataProvidersThatUseDate = new List<WebSpeciesObservationDataProvider>();
            if (dataProviders.IsNotEmpty())
            {
                foreach (WebSpeciesObservationDataProvider dataProvider in dataProviders)
                {
                    if (!(dataProvider.IsMaxChangeIdSpecified))
                    {
                        getDataProvidersThatUseDate.Add(dataProvider);
                    }
                }
            }

            return getDataProvidersThatUseDate;
        }
    }
}

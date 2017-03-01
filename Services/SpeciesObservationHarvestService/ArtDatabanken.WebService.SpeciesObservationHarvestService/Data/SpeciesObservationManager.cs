using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Data
{
    /// <summary>
    /// Class that handles species observation related information.
    /// </summary>
    public class SpeciesObservationManager : WebService.Data.SpeciesObservationManager
    {
        /// <summary>
        /// Get all species observation data providers.
        /// No cache is used.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All species observation data providers.</returns>
        public override List<WebSpeciesObservationDataProvider> GetSpeciesObservationDataProviders(WebServiceContext context)
        {
            List<WebSpeciesObservationDataProvider> dataProviders;
            WebSpeciesObservationDataProvider dataProvider;

            // Data not in cache. Get information from database.
            dataProviders = new List<WebSpeciesObservationDataProvider>();
            using (DataReader dataReader = context.GetDatabase().GetSpeciesObservationDataProviders(context.Locale.Id))
            {
                while (dataReader.Read())
                {
                    dataProvider = new WebSpeciesObservationDataProvider();
                    dataProvider.LoadData(dataReader);
                    dataProviders.Add(dataProvider);
                }
            }

            return dataProviders;
        }

        /// <summary>
        /// Get specified species observation data provider.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationDataProviderId">Species observation data provider id.</param>
        /// <returns>Specified species observation data provider.</returns>
        public override WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context,
                                                                                           Int32 speciesObservationDataProviderId)
        {
            foreach (WebSpeciesObservationDataProvider speciesObservationDataProvider in GetSpeciesObservationDataProviders(context))
            {
                if (speciesObservationDataProvider.Id == speciesObservationDataProviderId)
                {
                    return speciesObservationDataProvider;
                }
            }

            return null;
        }

        /// <summary>
        /// Get specified species observation data provider.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationDataProviderId">Species observation data provider id.</param>
        /// <returns>Specified species observation data provider.</returns>
        public override WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context,
                                                                                           SpeciesObservationDataProviderId speciesObservationDataProviderId)
        {
            return GetSpeciesObservationDataProvider(context, (Int32)(speciesObservationDataProviderId));
        }
    }
}

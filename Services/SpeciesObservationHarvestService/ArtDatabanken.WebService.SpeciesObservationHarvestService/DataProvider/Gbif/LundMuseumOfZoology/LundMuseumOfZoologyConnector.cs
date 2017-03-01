using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif.LundMuseumOfZoology
{
    /// <summary>
    /// Class that retrieves species observations from Lund Museum of Zoology (MZLU)
    /// published by GBIF-Sweden.
    /// 2016-10-14 Total number of Swedish species observations in GBIF: 91554.
    /// </summary>
    public class LundMuseumOfZoologyConnector : GbifConnector
    {
        /// <summary>
        /// Key to dataset in GBIF.
        /// </summary>
        private const String DATASET_KEY = "427a6290-0c65-11dd-84d2-b8a03c50a862";

        /// <summary>
        /// Initializes a new instance of the <see cref="LundMuseumOfZoologyConnector"/> class.
        /// </summary>
        public LundMuseumOfZoologyConnector()
            : base(DATASET_KEY, new LundMuseumOfZoologyProcess())
        {
        }

        /// <summary>
        /// Get species observation data provider for this connector.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Species observation data provider for this connector.</returns>
        public override WebService.Data.WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context)
        {
            return new Data.SpeciesObservationManager().GetSpeciesObservationDataProvider(context, SpeciesObservationDataProviderId.LundMuseumOfZoology);
        }
    }
}

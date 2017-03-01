using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif.SwedishMalaiseTrapProject
{
    /// <summary>
    /// Class that retrieves species observations from Swedish Malaise Trap Project (SMTP)
    /// Collection Inventory published by Swedish Museum of Natural History.
    /// 2016-10-14 Total number of Swedish species observations in GBIF: 99830.
    /// </summary>
    public class SwedishMalaiseTrapProjectConnector : GbifConnector
    {
        /// <summary>
        /// Key to dataset in GBIF.
        /// </summary>
        private const String DATASET_KEY = "38c1351d-9cfe-42c0-97da-02d2c8be141c";

        /// <summary>
        /// Initializes a new instance of the <see cref="SwedishMalaiseTrapProjectConnector"/> class.
        /// </summary>
        public SwedishMalaiseTrapProjectConnector()
            : base(DATASET_KEY, new SwedishMalaiseTrapProjectProcess())
        {
        }

        /// <summary>
        /// Get species observation data provider for this connector.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Species observation data provider for this connector.</returns>
        public override WebService.Data.WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context)
        {
            return new Data.SpeciesObservationManager().GetSpeciesObservationDataProvider(context, SpeciesObservationDataProviderId.SwedishMalaiseTrapProject);
        }
    }
}

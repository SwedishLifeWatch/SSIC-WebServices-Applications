using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif.EntomologicalCollections
{
    /// <summary>
    /// Class that retrieves species observations from Entomological Collections (NHRS)
    /// published by Swedish Museum of Natural History.
    /// 2016-10-14 Total number of Swedish species observations in GBIF: 50474.
    /// </summary>
    public class EntomologicalCollectionsConnector : GbifConnector
    {
        /// <summary>
        /// Key to dataset in GBIF.
        /// </summary>
        private const String DATASET_KEY = "9940af5a-3271-4e6a-ad71-ced986b9a9a5";

        /// <summary>
        /// Initializes a new instance of the <see cref="EntomologicalCollectionsConnector"/> class.
        /// </summary>
        public EntomologicalCollectionsConnector()
            : base(DATASET_KEY, new EntomologicalCollectionsProcess())
        {
        }

        /// <summary>
        /// Get species observation data provider for this connector.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Species observation data provider for this connector.</returns>
        public override WebService.Data.WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context)
        {
            return new Data.SpeciesObservationManager().GetSpeciesObservationDataProvider(context, SpeciesObservationDataProviderId.EntomologicalCollections);
        }
    }
}

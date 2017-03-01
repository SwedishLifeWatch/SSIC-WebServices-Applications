using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif.HerbariumOfOskarshamn
{
    /// <summary>
    /// Class that retrieves species observations from Herbarium of Oskarshamn (OHN)
    /// published by GBIF-Sweden. 
    /// 2016-10-14 Total number of Swedish species observations in GBIF: 133226.
    /// </summary>
    public class HerbariumOfOskarshamnConnector : GbifConnector
    {
        /// <summary>
        /// Key to dataset in GBIF.
        /// </summary>
        private const String DATASET_KEY = "41b59050-0a12-11dd-953d-b8a03c50a862";

        /// <summary>
        /// Initializes a new instance of the <see cref="HerbariumOfOskarshamnConnector"/> class.
        /// </summary>
        public HerbariumOfOskarshamnConnector()
            : base(DATASET_KEY, new HerbariumOfOskarshamnProcess())
        {
        }

        /// <summary>
        /// Get species observation data provider for this connector.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Species observation data provider for this connector.</returns>
        public override WebService.Data.WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context)
        {
            return new Data.SpeciesObservationManager().GetSpeciesObservationDataProvider(context, SpeciesObservationDataProviderId.HerbariumOfOskarshamn);
        }
    }
}

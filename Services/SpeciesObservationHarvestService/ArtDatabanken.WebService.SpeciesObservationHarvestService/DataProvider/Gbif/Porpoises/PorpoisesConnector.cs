using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif.Porpoises
{
    /// <summary>
    /// Class that retrieves species observations from Bird Ringing Centre in Sweden (NRM)
    /// published by GBIF-Sweden.
    /// 2016-10-14 Total number of Swedish species observations in GBIF: 2270.
    /// </summary>
    public class PorpoisesConnector : GbifConnector
    {
        /// <summary>
        /// Key to dataset in GBIF.
        /// </summary>
        private const String DATASET_KEY = "6aa7c400-0c66-11dd-84d2-b8a03c50a862";

        /// <summary>
        /// Initializes a new instance of the <see cref="PorpoisesConnector"/> class.
        /// </summary>
        public PorpoisesConnector()
            : base(DATASET_KEY, new PorpoisesProcess())
        {
        }

        /// <summary>
        /// Get species observation data provider for this connector.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Species observation data provider for this connector.</returns>
        public override WebService.Data.WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context)
        {
            return new Data.SpeciesObservationManager().GetSpeciesObservationDataProvider(context, SpeciesObservationDataProviderId.Porpoises);
        }
    }
}

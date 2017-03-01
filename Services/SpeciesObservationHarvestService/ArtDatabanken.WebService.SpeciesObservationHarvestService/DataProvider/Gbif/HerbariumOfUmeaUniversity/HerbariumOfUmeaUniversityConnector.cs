using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif.HerbariumOfUmeaUniversity
{
    /// <summary>
    /// Class that retrieves species observations from Herbarium of Umeå University (UME)
    /// published by GBIF-Sweden.
    /// 2016-10-14 Total number of Swedish species observations in GBIF: 79837.
    /// </summary>
    public class HerbariumOfUmeaUniversityConnector : GbifConnector
    {
        /// <summary>
        /// Key to dataset in GBIF.
        /// </summary>
        private const String DATASET_KEY = "905daf20-04fd-11df-8c28-b8a03c50a862";

        /// <summary>
        /// Initializes a new instance of the <see cref="HerbariumOfUmeaUniversityConnector"/> class.
        /// </summary>
        public HerbariumOfUmeaUniversityConnector()
            : base(DATASET_KEY, new HerbariumOfUmeaUniversityProcess())
        {
        }

        /// <summary>
        /// Get species observation data provider for this connector.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Species observation data provider for this connector.</returns>
        public override WebService.Data.WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context)
        {
            return new Data.SpeciesObservationManager().GetSpeciesObservationDataProvider(context, SpeciesObservationDataProviderId.HerbariumOfUmeaUniversity);
        }
    }
}

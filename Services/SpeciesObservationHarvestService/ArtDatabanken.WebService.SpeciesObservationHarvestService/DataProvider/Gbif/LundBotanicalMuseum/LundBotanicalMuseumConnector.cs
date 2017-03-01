using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif.LundBotanicalMuseum
{
    /// <summary>
    /// Class that retrieves species observations from Lund Botanical Museum (LD)
    /// published by Lund Botanical Museum (LD)
    /// 2016-10-14 Total number of Swedish species observations in GBIF: 571148.
    /// </summary>
    public class LundBotanicalMuseumConnector : GbifConnector
    {
        /// <summary>
        /// Key to dataset in GBIF.
        /// </summary>
        private const String DATASET_KEY = "aab0cf80-0c64-11dd-84d1-b8a03c50a862";

        /// <summary>
        /// All datasets with more than 200000 species observations must be
        /// downloaded to file before they can be imported.
        /// 
        /// How to get a new file from GBIF.
        /// 1: Create an account on http://www.gbif.org/
        /// 2: Loggin to http://www.gbif.org/
        /// 3: Go to menu Data->Explore dataset.
        /// 4: Search for dataset "Lund Botanical Museum (LD)".
        /// 5: Press the button "View occurrences".
        /// 6: Use filter "Add a filter"->Country.
        /// 7: Search for "Sweden" and press button "Apply".
        /// 8: Press button "Download".
        /// 9: Select format "Darwin Core Archive".
        /// 10: Follow instructions and download zip-file.
        /// 11: It is the file "occurrence.txt" that should be imported.
        /// </summary>
        private const String FILE_NAME = @"C:\Temp\LundBotanicalMuseum_occurrence.txt";

        /// <summary>
        /// Initializes a new instance of the <see cref="LundBotanicalMuseumConnector"/> class.
        /// </summary>
        public LundBotanicalMuseumConnector()
            : base(DATASET_KEY, new LundBotanicalMuseumProcess(), FILE_NAME)
        {
        }

        /// <summary>
        /// Get species observation data provider for this connector.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Species observation data provider for this connector.</returns>
        public override WebService.Data.WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context)
        {
            return new Data.SpeciesObservationManager().GetSpeciesObservationDataProvider(context, SpeciesObservationDataProviderId.LundBotanicalMuseum);
        }
    }
}

using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif.BirdRingingCentre
{
    /// <summary>
    /// Class that retrieves species observations from Bird Ringing Centre in Sweden (NRM)
    /// published by GBIF-Sweden.
    /// 2016-10-14 Total number of Swedish species observations in GBIF: 4852551.
    /// </summary>
    public class BirdRingingCentreConnector : GbifConnector
    {
        /// <summary>
        /// Key to dataset in GBIF.
        /// </summary>
        private const String DATASET_KEY = "4f70108a-dda7-4e8b-8298-babaee5182c3";

        /// <summary>
        /// All datasets with more than 200000 species observations must be
        /// downloaded to file before they can be imported.
        ///
        /// How to get a new file from GBIF.
        /// 1: Create an account on http://www.gbif.org/
        /// 2: Loggin to http://www.gbif.org/
        /// 3: Go to menu Data->Explore dataset.
        /// 4: Search for dataset "Bird Ringing Centre in Sweden (NRM)".
        /// 5: Press the button "View occurrences".
        /// 6: Use filter "Add a filter"->Country.
        /// 7: Search for "Sweden" and press button "Apply".
        /// 8: Press button "Download".
        /// 9: Select format "Darwin Core Archive".
        /// 10: Follow instructions and download zip-file.
        /// 11: It is the file "occurrence.txt" that should be imported.
        /// </summary>
        private const String FILE_NAME = @"C:\Temp\BirdRingingCentre_occurrence.txt";

        /// <summary>
        /// Initializes a new instance of the <see cref="BirdRingingCentreConnector"/> class.
        /// </summary>
        public BirdRingingCentreConnector()
            : base(DATASET_KEY, new BirdRingingCentreProcess(), FILE_NAME)
        {
        }

        /// <summary>
        /// Get species observation data provider for this connector.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Species observation data provider for this connector.</returns>
        public override WebService.Data.WebSpeciesObservationDataProvider GetSpeciesObservationDataProvider(WebServiceContext context)
        {
            return new Data.SpeciesObservationManager().GetSpeciesObservationDataProvider(context, SpeciesObservationDataProviderId.BirdRingingCentre);
        }
    }
}

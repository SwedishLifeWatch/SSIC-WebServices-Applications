using System;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Database;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to the WebSpeciesObservationDataProvider class.
    /// </summary>
    public static class WebSpeciesObservationDataProviderExtension
    {
        /// <summary>
        /// Load data into the WebSpeciesObservationDataProvider instance.
        /// </summary>
        /// <param name='speciesObservationDataProvider'>Species observation data Provider.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebSpeciesObservationDataProvider speciesObservationDataProvider,
                                    DataReader dataReader)
        {
            speciesObservationDataProvider.ContactEmail = dataReader.GetString(SpeciesObservationDataProviderData.CONTACT_EMAIL);
            speciesObservationDataProvider.ContactPerson = dataReader.GetString(SpeciesObservationDataProviderData.CONTACT_PERSON);
            speciesObservationDataProvider.Description = dataReader.GetString(SpeciesObservationDataProviderData.DESCRIPTION);
            speciesObservationDataProvider.Guid = dataReader.GetString(SpeciesObservationDataProviderData.GUID);
            speciesObservationDataProvider.Id = dataReader.GetInt32(SpeciesObservationDataProviderData.ID);
            speciesObservationDataProvider.Name = dataReader.GetString(SpeciesObservationDataProviderData.NAME);
            speciesObservationDataProvider.NonPublicSpeciesObservationCount = dataReader.GetInt64(SpeciesObservationDataProviderData.NON_PUBLIC_SPECIES_OBSERVATION_COUNT);
            speciesObservationDataProvider.Organization = dataReader.GetString(SpeciesObservationDataProviderData.ORGANIZATION);
            speciesObservationDataProvider.PublicSpeciesObservationCount = dataReader.GetInt64(SpeciesObservationDataProviderData.PUBLIC_SPECIES_OBSERVATION_COUNT);
            speciesObservationDataProvider.SpeciesObservationCount = dataReader.GetInt64(SpeciesObservationDataProviderData.SPECIES_OBSERVATION_COUNT);
            speciesObservationDataProvider.Url = dataReader.GetString(SpeciesObservationDataProviderData.URL);
            speciesObservationDataProvider.IsActiveHarvest = dataReader.GetBoolean(SpeciesObservationDataProviderData.IS_ACTIVE_HARVEST);
            speciesObservationDataProvider.LatestHarvestDate = dataReader.GetDateTime(SpeciesObservationDataProviderData.LATEST_HARVEST_DATE, DateTime.MinValue);
            speciesObservationDataProvider.BeginHarvestFromDate = dataReader.GetDateTime(SpeciesObservationDataProviderData.BEGIN_HARVEST_FROM_DATE, DateTime.MinValue);
            speciesObservationDataProvider.LatestChangedDate = dataReader.GetDateTime(SpeciesObservationDataProviderData.LATEST_CHANGED_DATE, DateTime.MinValue);
            speciesObservationDataProvider.IsMaxChangeIdSpecified = dataReader.IsNotDbNull(SpeciesObservationDataProviderData.MAX_CHANGE_ID);
            speciesObservationDataProvider.MaxChangeId = dataReader.GetInt64(SpeciesObservationDataProviderData.MAX_CHANGE_ID, -1);
        }
    }
}

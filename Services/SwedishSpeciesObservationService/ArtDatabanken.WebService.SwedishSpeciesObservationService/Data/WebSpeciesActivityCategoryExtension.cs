using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SwedishSpeciesObservationService.Database;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Data
{
    /// <summary>
    /// Extension class that has methods for WebSpeciesActivityCategory objects.
    /// </summary>
    public static class WebSpeciesActivityCategoryExtension
    {
        /// <summary>
        /// Populate species activity category with content from data reader.
        /// </summary>
        /// <param name="speciesActivityCategory">Species activity category that will be populated.</param>
        /// <param name="dataReader">Data source that will populate the species activity.</param>
        public static void LoadData(this WebSpeciesActivityCategory speciesActivityCategory,
                                    DataReader dataReader)
        {
            speciesActivityCategory.Guid = dataReader.GetString(SpeciesActivityCategoryData.GUID);
            speciesActivityCategory.Id = dataReader.GetInt32(SpeciesActivityCategoryData.ID);
            speciesActivityCategory.Identifier = dataReader.GetString(SpeciesActivityCategoryData.IDENTIFIER);
            speciesActivityCategory.Name = dataReader.GetString(SpeciesActivityCategoryData.NAME);
        }
    }
}

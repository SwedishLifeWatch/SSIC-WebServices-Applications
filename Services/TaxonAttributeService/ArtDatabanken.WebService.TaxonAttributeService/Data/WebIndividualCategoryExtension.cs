using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonAttributeService.Database;

namespace ArtDatabanken.WebService.TaxonAttributeService.Data
{
    /// <summary>
    /// Contains extension to the WebIndividualCategory class.
    /// </summary>
    public static class WebIndividualCategoryExtension
    {
        /// <summary>
        /// Load data into the WebFactorField instance.
        /// </summary>
        /// <param name="individualCategory">The individual category instance.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebIndividualCategory individualCategory, DataReader dataReader)
        {
            individualCategory.Definition = dataReader.GetString(IndividualCategoryData.DEFINITION);
            individualCategory.Id = dataReader.GetInt32(IndividualCategoryData.ID);
            individualCategory.Name = dataReader.GetString(IndividualCategoryData.NAME);
        }
    }
}
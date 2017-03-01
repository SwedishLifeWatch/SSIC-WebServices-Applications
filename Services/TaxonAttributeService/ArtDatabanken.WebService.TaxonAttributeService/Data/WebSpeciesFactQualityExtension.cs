using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonAttributeService.Database;

namespace ArtDatabanken.WebService.TaxonAttributeService.Data
{
    /// <summary>
    /// Contains extension to the WebSpeciesFactQuality class.
    /// </summary>
    public static class WebSpeciesFactQualityExtension
    {
        /// <summary>
        /// Load data into the WebSpeciesFactQuality instance.
        /// </summary>
        /// <param name="speciesFactQuality">The species fact quality instance.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebSpeciesFactQuality speciesFactQuality, DataReader dataReader)
        {
            speciesFactQuality.Definition = dataReader.GetString(SpeciesFactQualityData.DEFINITION);
            speciesFactQuality.Id = dataReader.GetInt32(SpeciesFactQualityData.ID);
            speciesFactQuality.Name = dataReader.GetString(SpeciesFactQualityData.NAME);
        }
    }
}
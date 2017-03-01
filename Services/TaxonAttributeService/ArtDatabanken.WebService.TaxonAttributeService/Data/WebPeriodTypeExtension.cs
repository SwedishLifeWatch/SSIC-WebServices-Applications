using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonAttributeService.Database;

namespace ArtDatabanken.WebService.TaxonAttributeService.Data
{
    /// <summary>
    /// Contains extension methods to the WebPeriodType class.
    /// </summary>
    public static class WebPeriodTypeExtension
    {
        /// <summary>
        /// Load data into the WebPeriodType instance.
        /// </summary>
        /// <param name="periodType">The period type instance.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebPeriodType periodType, DataReader dataReader)
        {
            periodType.Description = dataReader.GetString(PeriodTypeData.DESCRIPTION);
            periodType.Id = dataReader.GetInt32(PeriodTypeData.ID);
            periodType.Name = dataReader.GetString(PeriodTypeData.NAME);
        }
    }
}
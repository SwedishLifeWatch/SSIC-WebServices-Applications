using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonAttributeService.Database;

namespace ArtDatabanken.WebService.TaxonAttributeService.Data
{
    /// <summary>
    /// Contains extension to the WebPeriod class.
    /// </summary>
    public static class WebPeriodExtension
    {
        /// <summary>
        /// Load data into the WebPeriod instance.
        /// </summary>
        /// <param name="period">The period instance.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebPeriod period,
                                    DataReader dataReader)
        {
            period.Description = dataReader.GetString(PeriodData.DESCRIPTION_SWEDISH);
            period.Id = dataReader.GetInt32(PeriodData.ID);
            period.Name = dataReader.GetString(PeriodData.NAME);
            period.StopUpdates = dataReader.GetDateTime(PeriodData.STOP_UPDATES);
            period.TypeId = dataReader.GetInt32(PeriodData.TYPE_ID);
            period.Year = dataReader.GetInt32(PeriodData.YEAR);
        }
    }
}
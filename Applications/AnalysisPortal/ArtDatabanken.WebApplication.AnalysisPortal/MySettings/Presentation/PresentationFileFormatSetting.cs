using System.Runtime.Serialization;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.FileFormat;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation
{
    /// <summary>
    /// This class stores file format settings.
    /// </summary>
    [DataContract]
    public class PresentationFileFormatSetting : SettingBase
    {
        /// <summary>
        /// Gets or sets the CSV file settings.
        /// </summary>
        /// <value>
        /// The CSV file settings.
        /// </value>
        [DataMember]
        public CsvFileFormatSetting CsvFileSettings { get; set; }

        /// <summary>
        /// Gets or sets the Excel file settings.
        /// </summary>
        /// <value>
        /// The Excel file settings.
        /// </value>
        [DataMember]
        public ExcelFileFormat ExcelFileSettings { get; set; }

        /// <summary>
        /// Resets the settings.
        /// </summary>
        public void ResetSettings()
        {
            CsvFileSettings.ResetSettings();
            ExcelFileSettings.ResetSettings();
        }

        /// <summary>
        /// Determines whether the settings is the default settings.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the settings is default settings; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsSettingsDefault()
        {
            return CsvFileSettings.IsSettingsDefault() && ExcelFileSettings.IsSettingsDefault();
        }
    }
}

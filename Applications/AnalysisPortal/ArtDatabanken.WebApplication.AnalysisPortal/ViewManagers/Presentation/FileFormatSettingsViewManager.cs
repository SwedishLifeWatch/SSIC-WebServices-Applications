using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Presentation;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Presentation
{
    /// <summary>
    /// This class is a view manager for handling file format settings using the MySettings object.
    /// </summary>
    public class FileFormatSettingsViewManager : ViewManagerBase
    {
        /// <summary>
        /// Gets the file format setting that exists in MySettings.
        /// </summary>
        public PresentationFileFormatSetting FileFormatSetting 
        {
            get { return MySettings.Presentation.FileFormat; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormatSettingsViewManager"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="mySettings">The MySettings object.</param>
        public FileFormatSettingsViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        /// <summary>
        /// Creates a file format view model.
        /// </summary>
        /// <returns>A file format view model.</returns>
        public FileFormatViewModel CreateFileFormatViewModel()
        {
            FileFormatViewModel model = new FileFormatViewModel();
            model.CsvQuoteAllColumns = FileFormatSetting.CsvFileSettings.QuoteAllColumns;            
            model.CsvEnumSeparator = FileFormatSetting.CsvFileSettings.EnumSeparator;
            model.ExcelFileFormatType = FileFormatSetting.ExcelFileSettings.EnumExcelFormat;
            model.IsSettingsDefault = FileFormatSetting.IsSettingsDefault();
            return model;
        }

        /// <summary>
        /// Updates the file format settings.
        /// </summary>
        /// <param name="model">A file format view model.</param>
        public void UpdateFileFormatSettings(FileFormatViewModel model)
        {
            FileFormatSetting.CsvFileSettings.QuoteAllColumns = model.CsvQuoteAllColumns;            
            FileFormatSetting.CsvFileSettings.EnumSeparator = model.CsvEnumSeparator;
            FileFormatSetting.ExcelFileSettings.EnumExcelFormat = model.ExcelFileFormatType;
        }
    }
}
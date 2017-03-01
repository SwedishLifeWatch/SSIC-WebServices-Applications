using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Presentation
{
    /// <summary>
    /// This class contains settings summary for file format settings.
    /// </summary>
    public class FileFormatSettingSummary : MySettingsSummaryItemBase
    {
        private PresentationFileFormatSetting FileFormatSetting
        {
            get { return SessionHandler.MySettings.Presentation.FileFormat; }
        }

        public override string Title
        {
            get
            {
                return Resource.PresentationFileFormatTitle;                
            }
        }

        /// <summary>
        /// Gets the page info.
        /// </summary>
        public override PageInfo PageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Format", "FileFormat");
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has settings summary.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has settings summary; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettingsSummary
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the settings summary view width.
        /// If null, use default.
        /// </summary>
        public override int? SettingsSummaryWidth
        {
            get { return null; }
        }

        /// <summary>
        /// Gets a value indicating whether this setting is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public override bool IsActive
        {
            get { return true; }
            set { }
        }

        // Todo: has been set to false to prevent from showing in my settings summary
        public override bool HasSettings
        {
            get { return true; }
        }

        public override MySettingsSummaryItemIdentifier Identifier
        {
            get { return MySettingsSummaryItemIdentifier.PresentationFileFormat; }
        }

        public IEnumerable<string> GetSettingsSummaryModel()
        {
            return new List<string>
            {
                string.Format("{0}: {1}", Resource.PresentationFileFormatCsvSeparator, FileFormatSetting.CsvFileSettings.EnumSeparator.ToText()),
                string.Format("{0}: {1}", Resource.PresentationFileFormatCsvQuoteAllColumns, FileFormatSetting.CsvFileSettings.QuoteAllColumns ? Resource.SharedDialogButtonTextYes : Resource.SharedDialogButtonTextNo),
                string.Format("{0}: {1}", Resource.PresentationExcelFileFormatTypes, FileFormatSetting.ExcelFileSettings.EnumExcelFormat.ToText())
            };
        }
    }
}

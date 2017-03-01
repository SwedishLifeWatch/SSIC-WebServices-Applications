using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.FileFormat
{
    /// <summary>
    /// This class stores Excel file format settings.
    /// </summary>
    [DataContract]
    public class ExcelFileFormat : SettingBase
    {       
        /// <summary>
        /// Gets or sets the Excel file type.
        /// </summary>
        /// <value>
        /// The file type.
        /// </value>
        [DataMember]
        public ExcelFileType EnumExcelFormat { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationExcelFileFormatSetting"/> class.
        /// </summary>
        public ExcelFileFormat()
        {
            EnumExcelFormat = ExcelFileType.Xlsx;
        }

        /// <summary>
        /// Resets the settings.
        /// </summary>
        public void ResetSettings()
        {
            EnumExcelFormat = ExcelFileType.Xlsx;
        }

        /// <summary>
        /// Determines whether the settings is the default settings.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the settings is default settings; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsSettingsDefault()
        {
            if (EnumExcelFormat == ExcelFileType.Xlsx)
            {
                return true;
            }

            return false;
        }
    }
}

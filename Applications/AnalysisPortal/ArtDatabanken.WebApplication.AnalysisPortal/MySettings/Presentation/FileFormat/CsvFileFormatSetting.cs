using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.FileFormat
{
    /// <summary>
    /// This class stores CSV file format settings.
    /// </summary>
    [DataContract]
    public class CsvFileFormatSetting : SettingBase
    {       
        /// <summary>
        /// Gets or sets the CSV file column separator.
        /// </summary>
        /// <value>
        /// The column separator.
        /// </value>
        [DataMember]
        public CsvSeparator EnumSeparator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether each column should be enclosed in quotation marks.
        /// </summary>
        /// <value>
        ///   <c>true</c> if all columns should be enclosed in quotation marks; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool QuoteAllColumns { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvFileFormatSetting"/> class.
        /// </summary>
        public CsvFileFormatSetting()
        {            
            QuoteAllColumns = true;
            EnumSeparator = CsvSeparator.Comma;
        }

        /// <summary>
        /// Resets the settings.
        /// </summary>
        public void ResetSettings()
        {
            EnumSeparator = CsvSeparator.Comma;
            QuoteAllColumns = true;            
        }

        /// <summary>
        /// Determines whether the settings is the default settings.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the settings is default settings; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsSettingsDefault()
        {
            if (QuoteAllColumns == true
                && EnumSeparator == CsvSeparator.Comma)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the column separator as string.
        /// </summary>
        /// <returns>The column separator as string.</returns>
        public string GetSeparator()
        {
            switch (EnumSeparator)
            {
                case CsvSeparator.Comma:
                    return ",";
                case CsvSeparator.Semicolon:
                    return ";";
                case CsvSeparator.Pipe:
                    return "|";
                case CsvSeparator.Tab:
                    return "\t";
                default:
                    throw new ArgumentException(EnumSeparator + " doesn't exist");
            }
        }
    }
}

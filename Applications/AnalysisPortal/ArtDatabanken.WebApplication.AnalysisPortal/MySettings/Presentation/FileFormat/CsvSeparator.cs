using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.FileFormat
{
    /// <summary>
    /// CSV file column separator.
    /// </summary>
    [DataContract]
    public enum CsvSeparator
    {
        /// <summary>
        /// Comma separator (,).
        /// </summary>
        [EnumMember]
        Comma,

        /// <summary>
        /// Semicolon separator (;).
        /// </summary>
        [EnumMember]
        Semicolon,

        /// <summary>
        /// Tab separator (tab).
        /// </summary>
        [EnumMember]        
        Tab,

        /// <summary>
        /// Pipe separator (|).
        /// </summary>
        [EnumMember]
        Pipe,        
    }
}

using System.Runtime.Serialization;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.FileFormat
{
    /// <summary>
    /// Excel file column separator.
    /// </summary>
    [DataContract]
    public enum ExcelFileType
    {
        /// <summary>
        /// Xml.
        /// </summary>
        [EnumMember]
        Xml,

        /// <summary>
        /// Xlsx.
        /// </summary>
        [EnumMember]
        Xlsx,
    }
}

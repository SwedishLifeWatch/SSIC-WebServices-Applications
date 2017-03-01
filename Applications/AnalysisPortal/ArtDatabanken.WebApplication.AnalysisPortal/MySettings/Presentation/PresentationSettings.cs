using System.Runtime.Serialization;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation
{
    /// <summary>
    /// This class stores View settings.
    /// </summary>
    [DataContract]
    public sealed class PresentationSettings : SettingBase
    {
        /// <summary>
        /// Gets or sets the presentation table setting.
        /// </summary>
        [DataMember]
        public PresentationTableSetting Table { get; set; }

        /// <summary>
        /// Gets or sets the presentation map settings.
        /// </summary>        
        [DataMember]
        public PresentationMapSetting Map { get; set; }

        /// <summary>
        /// Gets or sets the presentation report settings.
        /// </summary>        
        [DataMember]
        public PresentationReportSetting Report { get; set; }

        /// <summary>
        /// Gets or sets the file format settings.
        /// </summary>        
        [DataMember]
        public PresentationFileFormatSetting FileFormat { get; set; }
    }
}

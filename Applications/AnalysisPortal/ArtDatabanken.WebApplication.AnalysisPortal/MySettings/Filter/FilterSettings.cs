using System.Runtime.Serialization;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter
{
    /// <summary>
    /// This class stores Filter settings.
    /// </summary>
    [DataContract]
    public sealed class FilterSettings : SettingBase
    {
        /// <summary>
        /// Gets or sets the taxa setting.
        /// </summary>
        [DataMember]
        public TaxaSetting Taxa { get; set; }

        /// <summary>
        /// Gets or sets the spatial setting.
        /// </summary>
        [DataMember]
        public SpatialSetting Spatial { get; set; }

        /// <summary>
        /// Gets or sets the temporal setting.
        /// </summary>
        [DataMember]
        public TemporalSetting Temporal { get; set; }

        /// <summary>
        /// Gets or sets the quality setting.
        /// </summary>
        /// TODO: Delete (accuracy will probably replace quality)
        [DataMember]
        public QualitySetting Quality { get; set; }

        /// <summary>
        /// Gets or sets the accuracy setting.
        /// </summary>
        [DataMember]
        public AccuracySetting Accuracy { get; set; }

        /// <summary>
        /// Gets or sets the occurrence setting.
        /// </summary>
        [DataMember]
        public OccurrenceSetting Occurrence { get; set; }

        /// <summary>
        /// Gets or sets the field setting.
        /// </summary>
        [DataMember]
        public FieldSetting Field { get; set; }
        
    }
}

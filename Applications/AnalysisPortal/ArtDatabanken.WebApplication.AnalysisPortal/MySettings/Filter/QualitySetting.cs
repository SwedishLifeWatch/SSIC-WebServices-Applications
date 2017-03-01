using System.Runtime.Serialization;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter
{
    /// <summary>
    /// This class stores filtered quality settings
    /// </summary>
    [DataContract]
    public sealed class QualitySetting : SettingBase
    {
        private bool _isActive;

        /// <summary>
        /// Gets or sets whether QualitySetting is active or not.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is checked; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsActive
        {
            get
            {
                return _isActive;
            }

            set
            {
                _isActive = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        /// <summary>
        /// Indicates whether this instance has values set or not 
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has values; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettings
        {            
            get { return false; }
        }

        public override bool IsSettingsDefault()
        {
            return true;
        }
    }
}

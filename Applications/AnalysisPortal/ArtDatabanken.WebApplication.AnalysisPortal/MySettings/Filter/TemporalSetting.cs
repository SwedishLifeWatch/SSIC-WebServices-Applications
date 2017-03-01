using System.Runtime.Serialization;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter
{
    /// <summary>
    /// This class stores filtered taxa settings
    /// </summary>
    [DataContract]
    public sealed class TemporalSetting : SettingBase
    {
        private bool _isActive;

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

        [DataMember]
        public TemporalDateSetting ObservationDate { get; set; }

        [DataMember]
        public TemporalDateSetting RegistrationDate { get; set; }

        [DataMember]
        public TemporalDateSetting ChangeDate { get; set; }

        public override bool IsSettingsDefault()
        {
            return ObservationDate.IsSettingsDefault()
                   && RegistrationDate.IsSettingsDefault()
                   && ChangeDate.IsSettingsDefault();
        }

        /// <summary>
        /// Checks if all temporal settings are disabled.
        /// </summary>        
        public bool IsAllTemporalSettingsDisabled
        {
            get
            {
                return ObservationDate.UseSetting == false
                       && RegistrationDate.UseSetting == false
                       && ChangeDate.UseSetting == false;
            }            
        }

        public void ResetSettings()
        {
            ObservationDate.ResetSettings();
            RegistrationDate.ResetSettings();
            ChangeDate.ResetSettings();
        }
    }
}

using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation
{
    [DataContract]
    public sealed class TimeSeriesSetting : SettingBase
    {
        private bool _isActive;
        private int _defaultPeriodicityIndex; // = (int)Periodicity.Monthly;
        
        [DataMember]
        public bool IsActive
        {
            get
            {
                return _isActive;
            }

            set
            {
                if (_isActive == value)
                {
                    return;
                }
                _isActive = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        [DataMember]
        public int DefaultPeriodicityIndex
        {
            get
            {
                return _defaultPeriodicityIndex;
            }

            set
            {
                if (_defaultPeriodicityIndex == value)
                {
                    return;
                }
                _defaultPeriodicityIndex = value;
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
            get
            {
                return true;
                //return CalculateNumberOfObservationsfromObsData || CalculateNumberOfSpeciesfromObsData;
                /*
                if (_defaultPeriodicityIndex == value) return; 
                _defaultPeriodicityIndex = value;  
                 */
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSeriesSetting"/> class.
        /// </summary>
        public TimeSeriesSetting()
        {
            _defaultPeriodicityIndex = (int)Periodicity.MonthOfTheYear;
            // Set calculations to be selected/active and number of observations to be selected/checked by default
            //CalculateNumberOfObservationsfromObsData = true;
            //CalculateNumberOfSpeciesfromObsData = true;
            IsActive = true;
        }

        public bool HasActiveSettings
        {
            get
            {
                return IsActive && HasSettings;
            }        
        }

        public override bool IsSettingsDefault()
        {
            if (DefaultPeriodicityIndex == (int)Periodicity.MonthOfTheYear)
            {
                return true;
            }
            return false;
        }

        public void ResetSettings()
        {
            DefaultPeriodicityIndex = (int)Periodicity.MonthOfTheYear;            
        }
    }
}

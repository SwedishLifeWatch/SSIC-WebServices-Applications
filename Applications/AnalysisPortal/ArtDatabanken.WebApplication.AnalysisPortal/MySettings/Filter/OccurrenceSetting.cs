using System.Runtime.Serialization;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter
{
    /// <summary>
    /// This class stores filtered Occurrence settings
    /// </summary>
    [DataContract]
    public sealed class OccurrenceSetting : SettingBase
    {
        private bool _isActive;
        private bool _includeNeverFoundObservations;
        private bool _includeNotRediscoveredObservations;
        private bool _includePositiveObservations;
        private bool _isNaturalOccurrence;
        private bool _isNotNaturalOccurrence;

        /// <summary>
        /// Gets or sets whether OccurrenceSetting is active or not.
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

        [DataMember]
        public bool IncludeNeverFoundObservations
        {
            get
            {
                return _includeNeverFoundObservations;
            }

            set
            {
                _includeNeverFoundObservations = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        [DataMember]
        public bool IncludeNotRediscoveredObservations
        {
            get
            {
                return _includeNotRediscoveredObservations;
            }

            set
            {
                _includeNotRediscoveredObservations = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        [DataMember]
        public bool IncludePositiveObservations
        {
            get
            {
                return _includePositiveObservations;
            }

            set
            {
                _includePositiveObservations = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        [DataMember]
        public bool IsNaturalOccurrence
        {
            get
            {
                return _isNaturalOccurrence;
            }

            set
            {
                _isNaturalOccurrence = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        [DataMember]
        public bool IsNotNaturalOccurrence
        {
            get
            {
                return _isNotNaturalOccurrence;
            }

            set
            {
                _isNotNaturalOccurrence = value;
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
            get
            {
                return IncludeNeverFoundObservations
                    || IncludeNotRediscoveredObservations
                    || IsNaturalOccurrence
                    || IsNotNaturalOccurrence
                    || IncludePositiveObservations;
            }
        }

        public OccurrenceSetting()
        {            
            ResetSettings();
        }

        public override bool IsSettingsDefault()
        {
            return IsNaturalOccurrence == true 
                && IsNotNaturalOccurrence == false
                && IncludePositiveObservations == true
                && IncludeNeverFoundObservations == false
                && IncludeNotRediscoveredObservations == false;
        }

        public void ResetSettings()
        {
            IsNaturalOccurrence = true;
            IsNotNaturalOccurrence = false;
            IncludePositiveObservations = true;
            IncludeNeverFoundObservations = false;
            IncludeNotRediscoveredObservations = false;
            IsActive = true;            
        }
    }
}

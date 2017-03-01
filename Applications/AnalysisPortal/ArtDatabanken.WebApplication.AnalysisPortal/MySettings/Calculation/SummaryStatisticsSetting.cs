using System.Runtime.Serialization;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation
{
    /// <summary>
    /// Settings class for Summary statistics.
    /// </summary>
    [DataContract]
    public sealed class SummaryStatisticsSetting : SettingBase
    {
        /// <summary>
        /// SummaryStatisticsSetting is active or not.
        /// </summary>
        private bool _isActive;

        /// <summary>
        /// Calculate number of observations.
        /// </summary>
        private bool _calculateNumberOfObservationsfromObsData;

        /// <summary>
        /// Calculate number of species.
        /// </summary>
        private bool _calculateNumberOfSpeciesfromObsData;

        /// <summary>
        /// Layer id for Wfs grid statistics.
        /// </summary>
        private int? _wfsSummaryStatisticsLayerId;

        /// <summary>
        /// Gets or sets whether SummaryStatisticsSetting is active or not.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is checked; otherwise, <c>false</c>.
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
                if (_isActive == value)
                {
                    return;
                }

                _isActive = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        /// <summary>
        /// Gets or sets calculate number of observations.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is checked; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool CalculateNumberOfObservationsfromObsData
        {
            get
            {
                return _calculateNumberOfObservationsfromObsData;
            }

            set
            {
                if (_calculateNumberOfObservationsfromObsData == value)
                {
                    return;
                }

                _calculateNumberOfObservationsfromObsData = value;                
                ResultCacheNeedsRefresh = true;                    
            }
        }

        /// <summary>
        /// Gets or sets calculate number of species.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is checked; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool CalculateNumberOfSpeciesfromObsData
        {
            get
            {
                return _calculateNumberOfSpeciesfromObsData;
            }

            set
            {
                if (_calculateNumberOfSpeciesfromObsData != value)
                {
                    _calculateNumberOfSpeciesfromObsData = value;
                    ResultCacheNeedsRefresh = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets Wfs layer id for summary statistics.
        /// </summary>
        /// <value>The id of the layer.</value>
        [DataMember]
        public int? WfsSummaryStatisticsLayerId
        {
            get
            {
                return _wfsSummaryStatisticsLayerId;
            }

            set
            {
                _wfsSummaryStatisticsLayerId = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        /// <summary>
        /// Indicates whether this instance has values set or not (i.e. TaxonIds).
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has values; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettings
        {
            get { return CalculateNumberOfObservationsfromObsData || CalculateNumberOfSpeciesfromObsData; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridStatisticsSetting"/> class.
        /// </summary>
        public SummaryStatisticsSetting()
        {                        
            // Set calculations to be selected/active and number of observations to be selected/checked by default
            CalculateNumberOfObservationsfromObsData = true;
            CalculateNumberOfSpeciesfromObsData = true;
            WfsSummaryStatisticsLayerId = null;
            IsActive = true;
        }
      
        /// <summary>
        /// Gets or sets whether SummaryStatisticsSetting has active settings or not.
        /// </summary>
        public bool HasActiveSettings
        {
            get { return IsActive && HasSettings; }        
        }

        /// <summary>
        /// Checks if settings are default.
        /// </summary>
        /// <returns>True, if settings are default, false otherwise.</returns>
        public override bool IsSettingsDefault()
        {
            if (CalculateNumberOfObservationsfromObsData
                && CalculateNumberOfSpeciesfromObsData
                && WfsSummaryStatisticsLayerId == null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Resets the settings.
        /// </summary>
        public void ResetSettings()
        {
            CalculateNumberOfObservationsfromObsData = true;
            CalculateNumberOfSpeciesfromObsData = true;
            WfsSummaryStatisticsLayerId = null;
        }
    }
}
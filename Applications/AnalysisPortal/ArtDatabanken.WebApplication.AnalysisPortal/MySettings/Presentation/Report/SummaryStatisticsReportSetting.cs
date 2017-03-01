using System.Runtime.Serialization;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.Report
{
    /// <summary>
    /// This class stores view settings
    /// </summary>
    [DataContract]
    public sealed class SummaryStatisticsReportSetting : SettingBase
    {
        private bool _isActive;
        private int _selectedReportId;

        /// <summary>
        /// Gets or sets whether PresentationViewSetting is active or not.
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
        public int SelectedReportId
        {
            get
            {
                return _selectedReportId;
            }

            set
            {
                _selectedReportId = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        /// <summary>
        /// Determines whether any settings has been done.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has settings; otherwise, <c>false</c>.
        ///   </returns>
        public override bool HasSettings
        {
            get { return false; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationTableSetting"/> class.
        /// </summary>
        public SummaryStatisticsReportSetting()
        {
            EnsureInitialized();
            // Set summary statistics to be selected/active by default
            IsActive = true;
        }
    }
}

using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.Report;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation
{
    public sealed class PresentationReportSetting : SettingBase
    {
        private bool _isActive;
        private ObservableCollection<PresentationReportType> _selectedReportTypes;

        /// <summary>
        /// Gets or sets whether PresentationViewSetting is active or not.
        /// </summary>        
        [DataMember]
        public bool IsActive
        {
            get
            {
                return _isActive;
            }

            set
            {
                if (value.Equals(_isActive))
                {
                    return;
                }
                _isActive = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        [DataMember]
        public SummaryStatisticsReportSetting SummaryStatisticsReportSetting { get; set; }

        [DataMember]
        public ObservableCollection<PresentationReportType> SelectedReportTypes
        {
            get
            {
                if (_selectedReportTypes.IsNull())
                {
                    _selectedReportTypes = new ObservableCollection<PresentationReportType>();
                    _selectedReportTypes.CollectionChanged += (sender, args) => { ResultCacheNeedsRefresh = true; };
                }
                return _selectedReportTypes;
            }
            set
            {                
                _selectedReportTypes = value;                
                ResultCacheNeedsRefresh = true; 
                if (_selectedReportTypes.IsNotNull())
                {
                    _selectedReportTypes.CollectionChanged += (sender, args) => { ResultCacheNeedsRefresh = true; };
                }
            }
        }

        /// <summary>
        /// Determines whether any settings has been done.
        /// </summary>        
        public override bool HasSettings
        {
            get { return SummaryStatisticsReportSetting.HasSettings; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationTableSetting"/> class.
        /// </summary>
        public PresentationReportSetting()
        {            
            IsActive = true;            
            SelectedReportTypes.Add(PresentationReportType.SummaryStatistics);
        }

        public override void EnsureInitialized()
        {
            base.EnsureInitialized();
        }

        public override bool IsSettingsDefault()
        {
            return true;
        }
    }
}

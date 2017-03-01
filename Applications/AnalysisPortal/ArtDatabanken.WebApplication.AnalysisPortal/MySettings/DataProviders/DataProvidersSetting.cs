using System.Collections.ObjectModel;
using ArtDatabanken.Data;
using System.Runtime.Serialization;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders
{
    [DataContract]
    public sealed class DataProvidersSetting : SettingBase
    {
        private ObservableCollection<string> _dataProvidersGuids;
        private bool _isActive;        

        [DataMember]
        public ObservableCollection<string> DataProvidersGuids
        {
            get
            {
                if (_dataProvidersGuids == null)
                {
                    _dataProvidersGuids = new ObservableCollection<string>();
                    _dataProvidersGuids.CollectionChanged += (sender, args) => { ResultCacheNeedsRefresh = true; };
                }
                return _dataProvidersGuids;
            }
            set
            {                
                _dataProvidersGuids = value;                
                ResultCacheNeedsRefresh = true;

                if (_dataProvidersGuids.IsNotNull())
                {
                    _dataProvidersGuids.CollectionChanged += (sender, args) => { ResultCacheNeedsRefresh = true; };
                }
            }
        }

        /// <summary>
        /// Gets or sets whether DataProviders is active or not.
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

        public override bool HasSettings
        {
            get { return !DataProvidersGuids.IsEmpty(); }
        }

        public DataProvidersSetting()
        {            
            IsActive = true;
            AddAllAvailableDataProviders();
        }
        
        public override bool IsSettingsDefault()
        {
            DataProvidersSetting newSetting = new DataProvidersSetting();
            if (newSetting.DataProvidersGuids.Count == DataProvidersGuids.Count)
            {
                return true;
            }

            return false;
        }

        private void AddAllAvailableDataProviders()
        {
            try
            {
                SpeciesObservationDataProviderList dataProviders = DataProviderManager.GetAllDataProviders(CoreData.UserManager.GetCurrentUser());
                foreach (ISpeciesObservationDataProvider dataProvider in dataProviders)
                {                    
                    DataProvidersGuids.Add(dataProvider.Guid);                    
                }
            }
            catch
            {
            }
        }

        public void ResetSettings()
        {
            DataProvidersGuids = new ObservableCollection<string>();
            AddAllAvailableDataProviders();
        }
    }
}

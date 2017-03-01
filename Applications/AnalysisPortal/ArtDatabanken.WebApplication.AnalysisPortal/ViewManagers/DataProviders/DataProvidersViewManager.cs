using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.DataProviders;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.DataProviders
{
    /// <summary>
    /// This class is a view manager for handling data sources operations using the MySettings object.
    /// </summary>
    public class DataProvidersViewManager : ViewManagerBase
    {
        /// <summary>
        /// Gets the data providers setting that exists in MySettings.
        /// </summary>
        public DataProvidersSetting DataProvidersSetting
        {
            get { return MySettings.DataProvider.DataProviders; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProvidersViewManager"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="mySettings">The MySettings object.</param>
        public DataProvidersViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        /// <summary>
        /// Creates the data providers view model.
        /// </summary>
        /// <returns></returns>
        public DataProvidersViewModel CreateDataProvidersViewModel()
        {
            var model = new DataProvidersViewModel();            
            model.DataProviders = GetAllDataProviders();
            model.IsSettingsDefault = DataProvidersSetting.IsSettingsDefault();
            return model;
        }

        public bool IsDataProvidersDefault()
        {
            return DataProvidersSetting.IsSettingsDefault();
        }

        /// <summary>
        /// Gets all data providers as a List of view models.
        /// </summary>        
        public List<DataProviderViewModel> GetAllDataProviders()
        {
            var dataProviders = DataProviderManager.GetAllDataProviders(UserContext);

            return (from dp in dataProviders
                    select new DataProviderViewModel(
                        dp.Id, 
                        dp.Guid, 
                        dp.Name, 
                        dp.Organization, 
                        dp.SpeciesObservationCount, 
                        dp.SpeciesObservationCount - dp.NonPublicSpeciesObservationCount, 
                        dp.Description, 
                        dp.Url)
                    {
                        IsSelected = DataProvidersSetting.DataProvidersGuids.Contains(dp.Guid)
                    }).ToList();
        }

        /// <summary>
        /// Updates the data providers. At least one must be selected.
        /// </summary>
        /// <param name="dataProviderIds">The data provider ids.</param>
        public void UpdateDataProviders(List<string> dataProviderIds)
        {
            DataProvidersSetting.DataProvidersGuids = new ObservableCollection<string>(dataProviderIds);
            if (DataProvidersSetting.DataProvidersGuids.Count == 0)
            {
                DataProvidersSetting.DataProvidersGuids.Add("urn:lsid:swedishlifewatch.se:DataProvider:1"); // at least one must be selected
            }
        }

        public DataProviderViewModel CreateDataProviderViewModel(int id)
        {
            List<DataProviderViewModel> dataProviders = GetAllDataProviders();
            DataProviderViewModel dataProvider = dataProviders.FirstOrDefault(t => t.Id == id);
            return dataProvider;
        }
    }
}

using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Temporal;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Filters
{
    /// <summary>
    /// This class is a view manager for handling temporal filters using the MySettings object.
    /// </summary>
    public class TemporalFilterViewManager : ViewManagerBase
    {
        /// <summary>
        /// Gets the temporal filter setting that exists in MySettings.
        /// </summary>
        public TemporalSetting TemporalSetting 
        {
            get { return MySettings.Filter.Temporal; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporalFilterViewManager"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="mySettings">The MySettings object.</param>
        public TemporalFilterViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        public TemporalFilterViewModel CreateTemporalFilterViewModel()
        {
            TemporalFilterViewModel model = new TemporalFilterViewModel();
            model.ObservationDate = new TemporalFilterDateViewModel();
            model.ObservationDate.StartDate = TemporalSetting.ObservationDate.StartDate;
            model.ObservationDate.EndDate = TemporalSetting.ObservationDate.EndDate;
            model.ObservationDate.UseSetting = TemporalSetting.ObservationDate.UseSetting;
            model.ObservationDate.Annually = TemporalSetting.ObservationDate.Annually;

            model.RegistrationDate = new TemporalFilterDateViewModel();
            model.RegistrationDate.StartDate = TemporalSetting.RegistrationDate.StartDate;
            model.RegistrationDate.EndDate = TemporalSetting.RegistrationDate.EndDate;
            model.RegistrationDate.UseSetting = TemporalSetting.RegistrationDate.UseSetting;
            model.RegistrationDate.Annually = TemporalSetting.RegistrationDate.Annually;

            model.ChangeDate = new TemporalFilterDateViewModel();
            model.ChangeDate.StartDate = TemporalSetting.ChangeDate.StartDate;
            model.ChangeDate.EndDate = TemporalSetting.ChangeDate.EndDate;
            model.ChangeDate.UseSetting = TemporalSetting.ChangeDate.UseSetting;
            model.ChangeDate.Annually = TemporalSetting.ChangeDate.Annually;

            model.IsSettingsDefault = TemporalSetting.IsSettingsDefault();
            model.IsAllTemporalSettingsDisabled = TemporalSetting.IsAllTemporalSettingsDisabled;
            return model;
        }

        public void UpdateTemporalFilter(TemporalFilterViewModel model)
        {
            if (model.ObservationDate != null)
            {
                TemporalSetting.ObservationDate.StartDate = model.ObservationDate.StartDate;
                TemporalSetting.ObservationDate.EndDate = model.ObservationDate.EndDate.AddDays(1).AddMilliseconds(-1);
                TemporalSetting.ObservationDate.UseSetting = model.ObservationDate.UseSetting;
                TemporalSetting.ObservationDate.Annually = model.ObservationDate.Annually;
            }

            if (model.RegistrationDate != null)
            {
                TemporalSetting.RegistrationDate.StartDate = model.RegistrationDate.StartDate;
                TemporalSetting.RegistrationDate.EndDate = model.RegistrationDate.EndDate.AddDays(1).AddMilliseconds(-1);
                TemporalSetting.RegistrationDate.UseSetting = model.RegistrationDate.UseSetting;
                TemporalSetting.RegistrationDate.Annually = model.RegistrationDate.Annually;
            }

            if (model.ChangeDate != null)
            {
                TemporalSetting.ChangeDate.StartDate = model.ChangeDate.StartDate;
                TemporalSetting.ChangeDate.EndDate = model.ChangeDate.EndDate.AddDays(1).AddMilliseconds(-1);
                TemporalSetting.ChangeDate.UseSetting = model.ChangeDate.UseSetting;
                TemporalSetting.ChangeDate.Annually = model.ChangeDate.Annually;
            }

            TemporalSetting.IsActive = true;
        }

        ///// <summary>
        ///// Gets all data providers as a List of view models.
        ///// </summary>        
        //public List<DataProviderViewModel> GetAllDataProviders()
        //{
        //    List<DataProviderViewModel> list = new List<DataProviderViewModel>();
        //    SpeciesObservationDataProviderList dataProviders = CoreData.SpeciesObservationManager.GetSpeciesObservationDataProviders(UserContext);
        //    foreach (ISpeciesObservationDataProvider dataProvider in dataProviders)
        //    {
        //        var provider = new DataProviderViewModel(dataProvider.Id, dataProvider.Name, dataProvider.Organization, dataProvider.SpeciesObservationCount);
        //        if (DataProvidersSetting.DataProvidersIds.Contains(provider.Id))
        //            provider.IsActive = true;
        //        else
        //            provider.IsActive = false;
        //        list.Add(provider);                
        //    }
        //    return list;
        //}

        ///// <summary>
        ///// Updates the temporal filter settings. Only one time type must be selected.
        ///// </summary>
        ///// <param name="dataProviderIds">The data provider ids.</param>
        //public void UpdateDataProviders(List<int> dataProviderIds)
        //{
        //    DataProvidersSetting.DataProvidersIds = dataProviderIds;
        //    if (DataProvidersSetting.DataProvidersIds.Count == 0)
        //        DataProvidersSetting.DataProvidersIds.Add(1); // at least one must be selected
        //}

        public List<string> GetTemporalSettingsSummary()
        {
            var strings = new List<string>();            
            if (TemporalSetting.ObservationDate.UseSetting)
            {
                if (TemporalSetting.ObservationDate.Annually)
                {
                    string str = string.Format(
                        "{0}: {1} - {2}",
                        Resources.Resource.FilterTemporalObsDateAnnually,
                        TemporalSetting.ObservationDate.StartDate.ToString("dd MMM"),
                        TemporalSetting.ObservationDate.EndDate.ToString("dd MMM"));
                    strings.Add(str);
                }
                else
                {
                    string str = string.Format(
                        "{0}: {1} - {2}",
                        Resources.Resource.FilterTemporalObsDateTitle,
                        TemporalSetting.ObservationDate.StartDate.ToString("yyyy-MM-dd"),
                        TemporalSetting.ObservationDate.EndDate.ToString("yyyy-MM-dd"));
                    strings.Add(str);
                }
            }

            if (TemporalSetting.RegistrationDate.UseSetting)
            {
                if (TemporalSetting.RegistrationDate.Annually)
                {
                    string str = string.Format(
                        "{0}: {1} - {2}",
                        Resources.Resource.FilterTemporalRegDateAnnually,
                        TemporalSetting.RegistrationDate.StartDate.ToString("dd MMM"),
                        TemporalSetting.RegistrationDate.EndDate.ToString("dd MMM"));
                    strings.Add(str);
                }
                else
                {
                    string str = string.Format(
                        "{0}: {1} - {2}",
                        Resources.Resource.FilterTemporalRegDateTitle,
                        TemporalSetting.RegistrationDate.StartDate.ToString("yyyy-MM-dd"),
                        TemporalSetting.RegistrationDate.EndDate.ToString("yyyy-MM-dd"));
                    strings.Add(str);
                }
            }

            if (TemporalSetting.ChangeDate.UseSetting)
            {
                if (TemporalSetting.ChangeDate.Annually)
                {
                    string str = string.Format(
                        "{0}: {1} - {2}",
                        Resources.Resource.FilterTemporalChangeDateAnnually,
                        TemporalSetting.ChangeDate.StartDate.ToString("dd MMM"),
                        TemporalSetting.ChangeDate.EndDate.ToString("dd MMM"));
                    strings.Add(str);
                }
                else
                {
                    string str = string.Format(
                        "{0}: {1} - {2}",
                        Resources.Resource.FilterTemporalChangeDateTitle,
                        TemporalSetting.ChangeDate.StartDate.ToString("yyyy-MM-dd"),
                        TemporalSetting.ChangeDate.EndDate.ToString("yyyy-MM-dd"));
                    strings.Add(str);
                }
            }
            return strings;
        }
    }
}
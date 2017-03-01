using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.MySettingsSummary;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.MySettings
{
    public class MySettingsViewManager : ViewManagerBase
    {
        public MySettingsViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        /// <summary>
        /// Creates a MySettingsButtonGroupViewModel.
        /// </summary>
        /// <returns></returns>
        public MySettingsButtonGroupViewModel CreateMySettingsButtonGroupViewModel()
        {
            var model = new MySettingsButtonGroupViewModel();
            bool isAnySettingsMade = MySettings.HasSettings;
            bool isUserLoggedIn = UserContext.IsAuthenticated();
            bool doesSettingsFileExistOnDisk = false;
            if (isUserLoggedIn)
            {
                doesSettingsFileExistOnDisk = MySettingsManager.DoesNameExistOnDisk(UserContext, MySettingsManager.SettingsName);    
            }
                        
            if (isAnySettingsMade && isUserLoggedIn)
            {
                model.IsSaveButtonEnabled = true;
            }

            if (isUserLoggedIn && doesSettingsFileExistOnDisk)
            {
                model.IsLoadButtonEnabled = true;
            }

            if (isAnySettingsMade)
            {
                model.IsResetButtonEnabled = true;
            }
            if (isUserLoggedIn)
            {
                model.LastSettingsSaveTime = MySettingsManager.GetLastSettingsSaveTime(UserContext);
                if (model.LastSettingsSaveTime.HasValue)
                {
                    model.DoesLastSettingsExists = true;
                }
            }

            return model;
        }
    }
}

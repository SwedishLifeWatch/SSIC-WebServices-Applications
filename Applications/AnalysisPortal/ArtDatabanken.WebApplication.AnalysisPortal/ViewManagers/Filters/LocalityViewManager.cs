using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Spatial;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Filters
{
    /// <summary>
    /// View manager for locality settings.
    /// </summary>
    public class LocalityViewManager : ViewManagerBase
    {
        /// <summary>
        /// Gets the locality setting.
        /// </summary>
        /// <value>
        /// The locality setting.
        /// </value>
        public LocalitySetting LocalitySetting
        {
            get { return MySettings.Filter.Spatial.Locality; }
        }

        /// <summary>
        /// Gets the spatial setting.
        /// </summary>
        /// <value>
        /// The spatial setting.
        /// </value>
        public SpatialSetting SpatialSetting
        {
            get { return MySettings.Filter.Spatial; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalityViewManager"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="mySettings">The MySettings object.</param>
        public LocalityViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        /// <summary>
        /// Creates a locality view model.
        /// </summary>
        /// <returns>
        /// A locality view model initialized from MySettings object.
        /// </returns>
        public LocalityViewModel CreateLocalityViewModel()
        {
            LocalityViewModel model = new LocalityViewModel();
            model.LocalityName = LocalitySetting.LocalityName;
            model.CompareOperator = LocalitySetting.CompareOperator;
            model.IsSettingsDefault = LocalitySetting.IsSettingsDefault();
            return model;
        }

        /// <summary>
        /// Updates the locality settings in MySettings.
        /// </summary>
        /// <param name="localityViewModel">The locality view model.</param>
        public void UpdateLocalitySetting(LocalityViewModel localityViewModel)
        {
            LocalitySetting.LocalityName = localityViewModel.LocalityName;
            LocalitySetting.CompareOperator = localityViewModel.CompareOperator;
            SpatialSetting.IsActive = true;
        }
    }
}
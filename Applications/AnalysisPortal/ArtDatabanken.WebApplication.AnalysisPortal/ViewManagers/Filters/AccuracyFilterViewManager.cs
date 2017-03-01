using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Accuracy;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Filters
{
    /// <summary>
    /// This class is a view manager for handling accuracy filters using the MySettings object.
    /// </summary>
    public class AccuracyFilterViewManager : ViewManagerBase
    {
        /// <summary>
        /// Gets the data providers setting that exists in MySettings.
        /// </summary>
        public AccuracySetting AccuracySetting
        {
            get { return MySettings.Filter.Accuracy; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccuracyFilterViewManager"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="mySettings">The MySettings object.</param>
        public AccuracyFilterViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        public AccuracyFilterViewModel CreateAccuracyFilterViewModel()
        {
            AccuracyFilterViewModel model = new AccuracyFilterViewModel();
            model.Inclusive = AccuracySetting.Inclusive;
            //model.Exclusive = AccuracySetting.Exclusive;
            model.MaxCoordinateAccuracy = AccuracySetting.MaxCoordinateAccuracy;
            model.IsSettingsDefault = AccuracySetting.IsSettingsDefault();
            model.IsCoordinateAccuracyActive = AccuracySetting.IsCoordinateAccuracyActive;

            return model;
        }

        public void UpdateAccuracyFilter(AccuracyFilterViewModel model)
        {
            AccuracySetting.MaxCoordinateAccuracy = model.MaxCoordinateAccuracy;
            AccuracySetting.Inclusive = model.Inclusive;
            AccuracySetting.IsCoordinateAccuracyActive = model.IsCoordinateAccuracyActive;
            AccuracySetting.IsActive = model.IsCoordinateAccuracyActive;
        }

        public List<string> GetAccuracySettingsSummary()
        {
            AccuracyFilterViewModel accuracyModel = CreateAccuracyFilterViewModel();
            var strings = new List<string>();

            if (accuracyModel.IsCoordinateAccuracyActive)
            {
                if (accuracyModel.MaxCoordinateAccuracy > 0)
                {
                    strings.Add(Resources.Resource.FilterAccuracyMaxCoordinateAccuracy + " - " + accuracyModel.MaxCoordinateAccuracy.ToString() + "m");
                }

                if (accuracyModel.Inclusive)
                {
                    strings.Add(Resources.Resource.FilterAccuracyInclusiveHint);
                }
                else
                {
                    strings.Add(Resources.Resource.FilterAccuracyExclusiveHint);
                }
            }

            return strings;
        }
    }
}
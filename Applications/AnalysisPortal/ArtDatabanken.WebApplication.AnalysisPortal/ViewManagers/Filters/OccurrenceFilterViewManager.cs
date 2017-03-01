using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Occurrence;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Filters
{
    /// <summary>
    /// This class is a view manager for handling data sources operations using the MySettings object.
    /// </summary>
    public class OccurrenceFilterViewManager : ViewManagerBase
    {
        /// <summary>
        /// Gets the data providers setting that exists in MySettings.
        /// </summary>
        public OccurrenceSetting OccurrenceSetting
        {
            get { return MySettings.Filter.Occurrence; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OccurrenceFilterViewManager"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="mySettings">The MySettings object.</param>
        public OccurrenceFilterViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        public OccurrenceFilterViewModel CreateOccurrenceFilterViewModel()
        {
            OccurrenceFilterViewModel model = new OccurrenceFilterViewModel();
            model.IncludeNeverFoundObservations = OccurrenceSetting.IncludeNeverFoundObservations;
            model.IncludeNotRediscoveredObservations = OccurrenceSetting.IncludeNotRediscoveredObservations;
            model.IncludePositiveObservations = OccurrenceSetting.IsNaturalOccurrence || OccurrenceSetting.IsNaturalOccurrence == true;
            model.IsNaturalOccurrence = OccurrenceSetting.IsNaturalOccurrence;
            model.IsNotNaturalOccurrence = OccurrenceSetting.IsNotNaturalOccurrence;
            model.IsSettingsDefault = OccurrenceSetting.IsSettingsDefault();
            return model;
        }

        public void UpdateOccurrenceSetting(OccurrenceFilterViewModel occurrenceFilterViewModel)
        {
            OccurrenceSetting.IncludeNeverFoundObservations = occurrenceFilterViewModel.IncludeNeverFoundObservations;
            OccurrenceSetting.IncludeNotRediscoveredObservations = occurrenceFilterViewModel.IncludeNotRediscoveredObservations;
            OccurrenceSetting.IsNaturalOccurrence = occurrenceFilterViewModel.IsNaturalOccurrence;
            OccurrenceSetting.IsNotNaturalOccurrence = occurrenceFilterViewModel.IsNotNaturalOccurrence;
            OccurrenceSetting.IncludePositiveObservations = occurrenceFilterViewModel.IsNaturalOccurrence || occurrenceFilterViewModel.IsNotNaturalOccurrence == true;
        }

        public List<string> GetOccurrenceSettingsSummary()
        {
            OccurrenceFilterViewModel occurrenceModel = CreateOccurrenceFilterViewModel();
            var strings = new List<string>();
            if (occurrenceModel.IsNaturalOccurrence)
            {
                strings.Add(Resources.Resource.FilterOccurrenceIsNaturalOccurrence);
            }

            if (occurrenceModel.IsNotNaturalOccurrence)
            {
                strings.Add(Resources.Resource.FilterOccurrenceIsNotNaturalOccurrence);
            }

            if (occurrenceModel.IncludeNeverFoundObservations)
            {
                strings.Add(Resources.Resource.FilterOccurrenceIncludeNeverFoundObservations);
            }

            if (occurrenceModel.IncludeNotRediscoveredObservations)
            {
                strings.Add(Resources.Resource.FilterOccurrenceIncludeNotRediscoveredObservations);
            }

            return strings;
        }
    }
}

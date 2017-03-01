using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Filter
{
    public class OccurrenceSettingSummary : MySettingsListSummaryItem
    {   
        public OccurrenceSettingSummary()
        {
            SupportDeactivation = true;
        }
        private OccurrenceSetting OccurrenceSetting
        {
            get { return SessionHandler.MySettings.Filter.Occurrence; }
        }

        public override string Title
        {
            get
            {
               /* string str = Resources.Resource.StateButtonFilterOccurrence;
                if (OccurrenceSetting.IsNaturalOccurrence && !OccurrenceSetting.IsNotNaturalOccurrence)
                {
                    str = Resources.Resource.FilterOccurrenceIsNaturalOccurrence;
                }
                if (!OccurrenceSetting.IsNaturalOccurrence && OccurrenceSetting.IsNotNaturalOccurrence)
                {
                    str = Resources.Resource.FilterOccurrenceIsNotNaturalOccurrence;
                }
                if (OccurrenceSetting.IsNaturalOccurrence && OccurrenceSetting.IsNotNaturalOccurrence)
                {
                    str = Resources.Resource.FilterOccurrenceIsNaturalOccurrence + " &amp;<br />" + Resources.Resource.FilterOccurrenceIsNotNaturalOccurrence;
                }*/
                return Resources.Resource.StateButtonFilterOccurrence;
            }
        }

        public override PageInfo PageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Filter", "Occurrence");
            }
        }

        public override bool HasSettingsSummary
        {
            get { return IsActive && HasSettings; }
        }        

        public override int? SettingsSummaryWidth
        {
            get { return null; }
        }

        public override bool IsActive
        {
            get { return OccurrenceSetting.IsActive; }
            set { OccurrenceSetting.IsActive = value; }
        }

        public override bool HasSettings
        {
            get { return OccurrenceSetting.HasSettings; }
        }

        public override MySettingsSummaryItemIdentifier Identifier
        {
            get { return MySettingsSummaryItemIdentifier.FilterOccurrence; }
        }

        public override List<string> GetSummaryList()
        {
            var occurrenceSetting = OccurrenceSetting;
            var summarySetting = new List<string>();

            if (occurrenceSetting.IsNaturalOccurrence)
            {
                summarySetting.Add(Resources.Resource.FilterOccurrenceIsNaturalOccurrence);
            }

            if (occurrenceSetting.IsNotNaturalOccurrence)
            {
                summarySetting.Add(Resources.Resource.FilterOccurrenceIsNotNaturalOccurrence);
            }

            if (occurrenceSetting.IncludeNeverFoundObservations)
            {
                summarySetting.Add(Resources.Resource.FilterOccurrenceIncludeNeverFoundObservations);
            }

            if (occurrenceSetting.IncludeNotRediscoveredObservations)
            {
                summarySetting.Add(Resources.Resource.FilterOccurrenceIncludeNotRediscoveredObservations);
            }
            
            return summarySetting;
        }
    }
}
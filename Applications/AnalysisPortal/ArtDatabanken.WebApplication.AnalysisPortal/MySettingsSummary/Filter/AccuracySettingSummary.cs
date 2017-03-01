using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Filter
{
    /// <summary>
    /// This class contains settings summary for accuracy settings.
    /// </summary>
    public class AccuracySettingSummary : MySettingsListSummaryItem
    {   
        public AccuracySettingSummary()
        {
            SupportDeactivation = true;
        }

        private AccuracySetting AccuracySetting
        {
            get { return SessionHandler.MySettings.Filter.Accuracy; }
        }

        public override string Title
        {
            get
            {
                var str = Resources.Resource.FilterAccuracyTitle; //"Filter accuracy test"; //Resources.Resource.MySettingsFilterAccuracy;

                if (AccuracySetting.IsCoordinateAccuracyActive)
                {
                    if (AccuracySetting.Inclusive)
                    {                        
                        str = Resources.Resource.SharedAccuracyFilterInclude;
                    }
                    else
                    {
                        str = Resources.Resource.SharedAccuracyFilterExclude;
                    }
                    if (AccuracySetting.MaxCoordinateAccuracy > 0)
                    {                        
                        str = string.Format(Resources.Resource.SharedAccuracyFilterMaxMeterTemplate, AccuracySetting.MaxCoordinateAccuracy.ToString());                        
                    }
                }
                return str;
            }
        }

        public override PageInfo PageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Filter", "Accuracy");
            }
        }

        public override bool HasSettingsSummary
        {
            get { return AccuracySetting.IsCoordinateAccuracyActive; }
        }        

        public override int? SettingsSummaryWidth
        {
            get { return 450; }
        }

        public override bool IsActive
        {
            get { return AccuracySetting.IsActive; }
            set { AccuracySetting.IsActive = value; }
        }

        public override bool HasSettings
        {
            get { return AccuracySetting.HasSettings; }
        }

        public override MySettingsSummaryItemIdentifier Identifier
        {
            get { return MySettingsSummaryItemIdentifier.FilterAccuracy; }
        }

        public override List<string> GetSummaryList()
        {
            var strings = new List<string>();

            if (AccuracySetting.IsCoordinateAccuracyActive)
            {
                if (AccuracySetting.MaxCoordinateAccuracy > 0)
                {
                    strings.Add(Resources.Resource.FilterAccuracyMaxCoordinateAccuracy + " - " + AccuracySetting.MaxCoordinateAccuracy.ToString() + "m");
                }

                if (AccuracySetting.Inclusive)
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

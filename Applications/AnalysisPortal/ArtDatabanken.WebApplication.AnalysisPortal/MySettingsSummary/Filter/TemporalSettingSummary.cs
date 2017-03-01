using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Filter
{
    /// <summary>
    /// This class contains settings summary for temporal settings.
    /// </summary>
    public class TemporalSettingSummary : MySettingsListSummaryItem
    {
        public TemporalSettingSummary()
        {
            SupportDeactivation = true;
        }

        private TemporalSetting TemporalSetting
        {
            get { return SessionHandler.MySettings.Filter.Temporal; }
        }

        public override string Title
        {
            get
            {
                string str = Resources.Resource.MySettingsFilterTemporalDate;
                if (TemporalSetting.ObservationDate.HasSettings && !TemporalSetting.RegistrationDate.HasSettings && !TemporalSetting.ChangeDate.HasSettings)
                {
                    string template = Resources.Resource.MySettingsFilterTemporalObservationDate;
                    str = string.Format(template, TemporalSetting.ObservationDate.StartDate.ToShortDateString() + " - " + TemporalSetting.ObservationDate.EndDate.ToShortDateString());
                }
                if (!TemporalSetting.ObservationDate.HasSettings && TemporalSetting.RegistrationDate.HasSettings && !TemporalSetting.ChangeDate.HasSettings)
                {
                    string template = Resources.Resource.MySettingsFilterTemporalRegistrationDate;
                    str = string.Format(template, TemporalSetting.RegistrationDate.StartDate.ToShortDateString() + " - " + TemporalSetting.RegistrationDate.EndDate.ToShortDateString());
                }
                if (!TemporalSetting.ObservationDate.HasSettings && !TemporalSetting.RegistrationDate.HasSettings && TemporalSetting.ChangeDate.HasSettings)
                {
                    string template = Resources.Resource.MySettingsFilterTemporalChangeDate;
                    str = string.Format(template, TemporalSetting.ChangeDate.StartDate.ToShortDateString() + " - " + TemporalSetting.ChangeDate.EndDate.ToShortDateString());
                }
                return str;
            }
        }

        public override PageInfo PageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Filter", "Temporal");
            }
        }

        public override bool HasSettingsSummary
        {
            get { return IsActive && HasSettings; }
        }        

        public override int? SettingsSummaryWidth
        {
            get { return 450; }
        }

        public override bool IsActive
        {
            get { return TemporalSetting.IsActive; }
            set { TemporalSetting.IsActive = value; }
        }

        public override bool HasSettings
        {
            get { return TemporalSetting.HasSettings; }
        }

        public override MySettingsSummaryItemIdentifier Identifier
        {
            get { return MySettingsSummaryItemIdentifier.FilterTemporal; }
        }

        public override List<string> GetSummaryList()
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
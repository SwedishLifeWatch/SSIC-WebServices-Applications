using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Filter
{
    public class FieldSettingSummary : MySettingsSummaryItemBase
    {
        public FieldSettingSummary()
        {
            SupportDeactivation = true;
        }
        private FieldSetting FieldSetting
        {
            get { return SessionHandler.MySettings.Filter.Field; }
        }

        public override string Title
        {
            get
            {
                if (FieldSetting.FieldFilterExpressions.Count == 1)
                {
                    string template = Resources.Resource.MySettingsFilterOneSelectedField;
                    string str = string.Format(template, FieldSetting.FieldFilterExpressions[0].Property.GetName());
                    return str;
                }
                else
                {
                    string template = Resources.Resource.MySettingsFilterNumberOfSelectedFields;
                    string str = string.Format(template, FieldSetting.FieldFilterExpressions.Count);
                    return str;
                }
            }
        }

        public override PageInfo PageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Filter", "Field");
            }
        }

        public override bool HasSettingsSummary
        {
            get { return IsActive && HasSettings; }
        }

        public override int? SettingsSummaryWidth
        {
            get { return 500; }
        }

        public override bool IsActive
        {
            get { return FieldSetting.IsActive; }
            set { FieldSetting.IsActive = value; }
        }
        
    public override bool HasSettings
        {
            get { return FieldSetting.FieldFilterExpressions.Count > 0; }
        }

        public override MySettingsSummaryItemIdentifier Identifier
        {
            get { return MySettingsSummaryItemIdentifier.FilterField; }
        }

        public IEnumerable<string> GetSettingsSummaryModel()
        {
            var strings = new List<string>();

            if (SessionHandler.MySettings.Filter.Field.IsActive && SessionHandler.MySettings.Filter.Field.HasSettings)
            {
                strings.Add(Utils.Converters.FieldFilterExpressionConverter.GetFieldFilterExpression(SessionHandler.MySettings.Filter.Field.FieldFilterExpressions, SessionHandler.MySettings.Filter.Field.FieldLogicalOperator));
            }

            return strings;
        }
    }
}

using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Improved;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.DataProviders;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Improved.DataProviders
{
    /// <summary>
    /// This class contains settings summary for data provider settings.
    /// </summary>
    public class ImprovedDataProvidersSettingSummary : ImprovedMySettingsListSummaryItem
    {
        public ImprovedDataProvidersSettingSummary(IUserContext userContext, MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        private DataProvidersSetting DataProvidersSetting
        {
            get { return SessionHandler.MySettings.DataProvider.DataProviders; }
        }

        public override MySettingsSummaryItemIdentifierModel IdentifierModel
        {
            get
            {
                return new MySettingsSummaryItemIdentifierModel(MySettingsSummaryItemIdentifier.DataProviders);
            }
        }

        public override MySettingsSummaryItemIdentifier Identifier
        {
            get { return MySettingsSummaryItemIdentifier.DataProviders; }
        }

        public override List<MySettingsSummaryItemSubIdentifier> SubIdentifiers
        {
            get { return null; }
        }

        public override List<MySettingsSummaryItemIdentifier> SubIdentifiers2
        {
            get { return null; }
        }

        public override string Title
        {
            get
            {
                string template = Resources.Resource.MySettingsDataSourcesNumberOfDataProviders;
                string str = string.Format(template, DataProvidersSetting.DataProvidersGuids.Count);
                return str;
            }
        }

        public override PageInfo PageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Data", "DataProviders");
            }
        }

        public override bool HasSettingsSummary
        {
            get { return true; }
        }

        public List<DataProviderViewModel> GetSettingsSummaryModel()
        {                        
            var viewManager = new DataProvidersViewManager(UserContext, MySettings);
            List<DataProviderViewModel> dataProviders = viewManager.GetAllDataProviders();
            return dataProviders;            
        }
        
        public override int? SettingsSummaryWidth
        {
            get { return 350; }
        }        

        public override bool IsActive
        {
            get { return DataProvidersSetting.IsActive; }
        }

        public override bool HasSettings
        {
            get { return DataProvidersSetting.DataProvidersGuids.Count > 0; }
        }

        public static MySettingsSummaryItemIdentifier StaticIdentifier
        {
            get { return MySettingsSummaryItemIdentifier.DataProviders; }
        }

        public static List<ResultType> GetAffectedResultTypes()
        {
            return MySettingsSummaryResultTypeManager.AllResultTypes;
        }

        /// <summary>
        /// Get summary list.
        /// </summary>
        /// <returns>
        /// A list with settings information.
        /// </returns>
        public override ImprovedMySettingsItem<string> GetSummaryList(PageInfo pageInfo)
        {
            return null;
            ////List<DataProviderViewModel> summaryModel = GetSettingsSummaryModel();
            ////List<string> settingsList = new List<string>();
            ////foreach (DataProviderViewModel dataProvider in summaryModel)
            ////{
            ////    if (!dataProvider.IsSelected)
            ////    {
            ////        continue;
            ////    }

            ////    settingsList.Add(dataProvider.NameAndOrganization);                    
            ////}

            ////return settingsList;
        }
    }
}

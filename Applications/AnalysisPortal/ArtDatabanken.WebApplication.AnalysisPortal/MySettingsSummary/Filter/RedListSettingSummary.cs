using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.RedList;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Filter
{
    //public class RedListSettingSummary : MySettingsSummaryItemBase
    //{
    //    public RedListSettingSummary()
    //    {
    //        SupportDeactivation = true;
    //    }
    //    private RedListSetting RedListSetting
    //    {
    //        get
    //        {
    //            return SessionHandler.MySettings.Filter.RedList;
    //        }
    //    }
        
    //    public override string Title
    //    {
    //        get
    //        {
    //            var count = 0;
    //            var categories = RedListSetting.Categories;

    //            if (categories != null)
    //            {
    //                count = categories.Count();
    //            }
              
    //            return string.Format(Resources.Resource.MySettingsFilterNumberOfSelectedRedListCategories, count);
    //        }
    //    }

    //    public override PageInfo PageInfo
    //    {
    //        get
    //        {
    //            return PageInfoManager.GetPageInfo("Filter", "RedList");
    //        }
    //    }

    //    public override bool HasSettingsSummary
    //    {
    //        get { return IsActive && HasSettings; }
    //    }

    //    public override int? SettingsSummaryWidth
    //    {
    //        get { return 500; }
    //    }

    //    public override bool IsActive
    //    {
    //        get { return RedListSetting.IsActive; }
    //        set { RedListSetting.IsActive = value; }
    //    }

    //    public override bool HasSettings
    //    {
    //        get { return RedListSetting.HasSettings; }
    //    }

    //    public override MySettingsSummaryItemIdentifier Identifier
    //    {
    //        get { return MySettingsSummaryItemIdentifier.RedList; }
    //    }

    //    public IEnumerable<string> GetSettingsSummaryModel()
    //    {
    //        if (!HasSettings)
    //        {
    //            return null;
    //        }

    //        //Create a RedListViewModel to get all categories 
    //        var model = new RedListViewModel();
    //        var categories = RedListSetting.Categories.ToArray();

    //        return (from c in categories select model.Categories[c].Text).ToList();
    //    }
    //}
}

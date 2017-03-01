using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.RedList;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Filters
{
    /// <summary>
    /// This class is a view manager for handling red list filters using the MySettings object.
    /// </summary>
    public class RedListFilterViewManager : ViewManagerBase
    {        

        /// <summary>
        /// Initializes a new instance of the <see cref="RedListFilterViewManager"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="mySettings">The MySettings object.</param>
        public RedListFilterViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        /// <summary>
        /// Creates an RedListViewModel
        /// </summary>
        /// <returns></returns>
        public RedListViewModel CreateRedListViewModel()
        {
            var model = new RedListViewModel();

            //if (Setting.HasSettings)
            //{
            //    foreach (var category in Setting.Categories)
            //    {
            //        model.Categories[category].Selected = true;
            //    }
            //}
            
            model.IsSettingsDefault = MySettings.Filter.Taxa.IsSettingsDefault();

            return model;
        }

        ///// <summary>
        ///// Update filter with selected categories
        ///// </summary>
        ///// <param name="categories"></param>
        //public void UpdateRedListFilter(IEnumerable<RedListCategory> categories)
        //{
        //    Setting.Categories = categories;
        //    Setting.IsActive = true;
        //}

        ///// <summary>
        ///// Get selected categories
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<string> GetRedListSettingsSummary()
        //{
        //    if (!Setting.HasSettings)
        //    {
        //        return null;
        //    }

        //    var model = new RedListViewModel();

        //    return (from c in Setting.Categories select model.Categories[c].Text).ToList();
        //}

        private TaxonIdList GetTaxonIdListFromTaxaFilter()
        {
            TaxonIdList taxonIdList = new TaxonIdList();
            foreach (int taxonId in MySettings.Filter.Taxa.TaxonIds)
            {
                taxonIdList.Add(new TaxonIdImplementation(taxonId));
            }

            return taxonIdList;
        }

        public void GetTaxaByRedListCategories(IEnumerable<RedListCategory> categories)
        {            
            AnalysisSearchCriteria analysisSearchCriteria = new AnalysisSearchCriteria();            
            analysisSearchCriteria.RedListCategories = categories.Select(x => (int)x).ToList();
            TaxonIdList currenTaxonIdList = GetTaxonIdListFromTaxaFilter();            

            TaxonIdList taxonIdList = CoreData.AnalysisManager.GetTaxonIds(analysisSearchCriteria, currenTaxonIdList);
            MySettings.Filter.Taxa.ResetSettings();
            MySettings.Filter.Taxa.AddTaxonIds(taxonIdList.GetIds());
            int v = 8;
            //TaxonIdList taxonIds = CoreData.AnalysisManager.GetTaxonIds(searchCriteria);

            //TaxonListInformationManager.Instance.
            //CoreData.AnalysisManager
        }

        public void UseAsCurrentSelection(IEnumerable<RedListCategory> categories)
        {
            AnalysisSearchCriteria analysisSearchCriteria = new AnalysisSearchCriteria();
            analysisSearchCriteria.RedListCategories = categories.Select(x => (int)x).ToList();            
            TaxonIdList taxonIdList = CoreData.AnalysisManager.GetTaxonIds(analysisSearchCriteria);
            MySettings.Filter.Taxa.ResetSettings();
            MySettings.Filter.Taxa.AddTaxonIds(taxonIdList.GetIds());
        }

        public void AddToCurrentSelection(IEnumerable<RedListCategory> categories)
        {
            AnalysisSearchCriteria analysisSearchCriteria = new AnalysisSearchCriteria();
            analysisSearchCriteria.RedListCategories = categories.Select(x => (int)x).ToList();
            TaxonIdList taxonIdList = CoreData.AnalysisManager.GetTaxonIds(analysisSearchCriteria);            
            MySettings.Filter.Taxa.AddTaxonIds(taxonIdList.GetIds());
        }

        public void FilterCurrentSelection(IEnumerable<RedListCategory> categories)
        {
            AnalysisSearchCriteria analysisSearchCriteria = new AnalysisSearchCriteria();
            analysisSearchCriteria.RedListCategories = categories.Select(x => (int)x).ToList();
            TaxonIdList currenTaxonIdList = GetTaxonIdListFromTaxaFilter();

            TaxonIdList taxonIdList = CoreData.AnalysisManager.GetTaxonIds(analysisSearchCriteria, currenTaxonIdList);
            MySettings.Filter.Taxa.ResetSettings();
            MySettings.Filter.Taxa.AddTaxonIds(taxonIdList.GetIds());
        }
    }
}
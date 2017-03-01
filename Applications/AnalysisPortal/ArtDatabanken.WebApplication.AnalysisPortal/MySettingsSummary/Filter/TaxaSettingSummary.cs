using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Filter
{
    /// <summary>
    /// This class contains settings summary for taxa settings.
    /// </summary>
    public class TaxaSettingSummary : MySettingsSummaryItemBase
    {           
        public TaxaSettingSummary()
        {
            SupportDeactivation = true;
        }

        private TaxaSetting TaxaSetting
        {
            get { return SessionHandler.MySettings.Filter.Taxa; }
        }

        public override string Title
        {
            get
            {
                if (TaxaSetting.TaxonIds.Count == 1)
                {
                    IUserContext userContext = CoreData.UserManager.GetCurrentUser();
                    TaxonList taxa = CoreData.TaxonManager.GetTaxa(userContext, TaxaSetting.TaxonIds.ToList());
                    string template = Resources.Resource.MySettingsFilterOneSelectedTaxon;
                    string str = string.Format(template, taxa[0].ScientificName);
                    return str;
                }
                else
                {
                    string template = Resources.Resource.MySettingsFilterNumberOfSelectedTaxa;
                    string str = string.Format(template, TaxaSetting.TaxonIds.Count);
                    return str;
                }
            }
        }

        public override PageInfo PageInfo
        {
            get { return PageInfoManager.GetPageInfo("Filter", "Taxa"); }
        }

        public override bool HasSettingsSummary
        {
            get { return IsActive && HasSettings; }
        }

        public List<TaxonViewModel> GetSettingsSummaryModel()
        {            
            IUserContext userContext = CoreData.UserManager.GetCurrentUser();
            ObservableCollection<int> taxonIds = SessionHandler.MySettings.Filter.Taxa.TaxonIds;
            TaxonList taxa = CoreData.TaxonManager.GetTaxa(userContext, taxonIds.ToList());
            List<TaxonViewModel> taxonList = taxa.GetGenericList().ToTaxonViewModelList();
            return taxonList;            
        }

        /// <summary>
        /// Gets a short description of selected taxa.
        /// </summary>
        /// <returns></returns>
        public string GetShortDescription()
        {
            if (SessionHandler.MySettings.Filter.Taxa.TaxonIds.Count == 0)
            {
                return Resource.MySettingsAllTaxa;
            }
            
            List<TaxonViewModel> settingsSummaryModel = GetSettingsSummaryModel();
            if (settingsSummaryModel.Count == 1)
            {
                return settingsSummaryModel[0].ScientificName;
            }

            return settingsSummaryModel[0].ScientificName + string.Format(", + {0} other taxa...", settingsSummaryModel.Count - 1);
        }

        public override int? SettingsSummaryWidth
        {
            get { return 500; }
        }

        public override bool IsActive
        {
            get { return TaxaSetting.IsActive; }
            set { TaxaSetting.IsActive = value; }
        }

        public override bool HasSettings
        {
            get { return TaxaSetting.TaxonIds.Count > 0; }
        }

        public override MySettingsSummaryItemIdentifier Identifier
        {
            get { return MySettingsSummaryItemIdentifier.FilterTaxa; }
        }

        //public ImprovedMySettingsSummaryItemBase2 GetExtensiveRepresentation()
        //{
        //    MySettingsSummaryTable table = new MySettingsSummaryTable();
        //    table.Columns.Add(new DataColumn("Taxon Id"));
        //    table.Columns.Add(new DataColumn("Vetenskapligt namn"));
        //    table.Columns.Add(new DataColumn("Svenskt namn"));
            
        //    List<TaxonViewModel> settingsSummaryModel = GetSettingsSummaryModel();
        //    foreach (TaxonViewModel taxon in settingsSummaryModel)
        //    {
        //        DataRow dataRow = table.NewRow();
        //        dataRow[0] = taxon.TaxonId;
        //        dataRow[1] = taxon.ScientificName;
        //        dataRow[2] = taxon.CommonName;
        //        table.Rows.Add(dataRow);
        //    }

        //    return table;
        //}

        //public ImprovedMySettingsSummaryItemBase2 GetNormalRepresentation()
        //{
        //    return GetExtensiveRepresentation();
        //}

        //public ImprovedMySettingsSummaryItemBase2 GetMinimalRepresentation()
        //{
        //    MySettingsSummaryList list = new MySettingsSummaryList();
        //    List<TaxonViewModel> settingsSummaryModel = GetSettingsSummaryModel();
        //    foreach (TaxonViewModel taxon in settingsSummaryModel)
        //    {
        //        list.Add(string.Format("{0} ({1})", taxon.ScientificName, taxon.TaxonId));                
        //    }

        //    return list;
        //}

        //public bool SettingAffectsTheResult(ResultType resultType)
        //{
        //    return true; // taxa filter can affect all type of results.            
        //}
    }
}

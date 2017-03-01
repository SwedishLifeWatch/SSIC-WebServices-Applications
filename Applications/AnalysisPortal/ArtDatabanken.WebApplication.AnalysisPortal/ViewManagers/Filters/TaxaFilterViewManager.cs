using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Enums;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Localization;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Taxa;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Filters
{
    /// <summary>
    /// This class is a manager class for Filtering Taxa
    /// </summary>
    public class TaxaFilterViewManager : ViewManagerBase
    {
        public TaxaSetting TaxaSetting
        {
            get { return MySettings.Filter.Taxa; }
        }

        public TaxaFilterViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        /// <summary>
        /// Tries to parse the text into taxon ids and return a list of taxons.
        /// </summary>
        /// <param name="text">The text with taxonids.</param>
        /// <param name="rowDelimiter">The row delimiter type.</param>
        /// <returns></returns>
        public List<ITaxon> GetMatchingTaxaFromText(string text, RowDelimiter rowDelimiter)
        {
            var matchManager = new TaxonMatchManager(UserContext);
            return matchManager.GetMatchingTaxaFromText(text, rowDelimiter);
        }

        public List<ITaxon> GetChildTaxa(int[] parentTaxonIds)
        {
            var searchCriteria = new TaxonSearchCriteria()
            {
                TaxonIds = new List<int>(parentTaxonIds),
                TaxonCategoryIds = new List<int> { (int)TaxonCategoryId.Species },
                Scope = TaxonSearchScope.AllChildTaxa
            };
            
            return CoreData.TaxonManager.GetTaxa(UserContext, searchCriteria);
        }

        /// <summary>
        /// Creates an TaxonFromIdsViewModel
        /// </summary>
        /// <returns></returns>
        public TaxonFromIdsViewModel CreateTaxonFromIdsViewModel()
        {
            var model = new TaxonFromIdsViewModel();
            var rowDelimiters = from RowDelimiter rd in Enum.GetValues(typeof(RowDelimiter))
                                select new { value = (int)rd, text = rd.GetLocalizedDescription() };
            model.RowDelimiterSelectList = new SelectList(rowDelimiters, "value", "text", model.RowDelimiter.ToString());
            model.IsSettingsDefault = TaxaSetting.IsSettingsDefault();
            return model;
        }

        /// <summary>
        /// Creates an TaxonFromSearchViewModel
        /// </summary>
        /// <returns></returns>
        public TaxonFromSearchViewModel CreateTaxonFromSearchViewModel()
        {
            var model = new TaxonFromSearchViewModel();
            model.SearchOptions = new TaxonSearchOptions();
            model.IsSettingsDefault = TaxaSetting.IsSettingsDefault();
            return model;
        }

        /// <summary>
        /// Creates an TaxonByTaxonAttributesModel.
        /// </summary>
        /// <returns></returns>
        public TaxonByTaxonAttributesViewModel CreateTaxonByTaxonAttributesViewModel()
        {
            var model = new TaxonByTaxonAttributesViewModel();
            model.SearchOptions = new TaxonSearchOptions();
            model.IsSettingsDefault = TaxaSetting.IsSettingsDefault();
            return model;
        }

        /// <summary>
        /// Create an TaxaFilterFromFactorViewModel.
        /// </summary>
        /// <param name="factor"></param>
        /// <returns></returns>
        public TaxaFilterFromFactorViewModel CreateTaxaFilterFromFactorViewModel(IFactor factor)
        {
            var model = new TaxaFilterFromFactorViewModel();
            model.Factor = factor;
            return model;
        }

        public void AddTaxonIds(int[] taxonIds)
        {
            TaxaSetting.AddTaxonIds(taxonIds);
        }

        /// <summary>
        /// Add underlying species taxon ids to taxa filter.
        /// </summary>
        /// <param name="parentTaxonIds">
        /// The parent taxon ids, where we will search for underlying species.
        /// </param>
        public void AddUnderlyingSpeciesTaxonIds(int[] parentTaxonIds)
        {
            var taxonList = GetChildTaxa(parentTaxonIds);
            var taxonIds = new List<int>();
            if (taxonList != null)
            {
                foreach (var taxon in taxonList)
                {
                    taxonIds.Add(taxon.Id);
                }
            }
            AddTaxonIds(taxonIds.ToArray());
        }

        public void RemoveTaxonId(int taxonId)
        {
            TaxaSetting.RemoveTaxonId(taxonId);
        }

        public void RemoveTaxonIds(int[] taxonIds)
        {
            TaxaSetting.RemoveTaxonIds(taxonIds);            
        }

        public void RemoveAllTaxa()
        {
            TaxaSetting.TaxonIds.Clear();
        }

        public List<TaxonViewModel> GetAllTaxaViewModel()
        {
            TaxonList taxonList = CoreData.TaxonManager.GetTaxa(UserContext, TaxaSetting.TaxonIds.ToList());
            List<TaxonViewModel> taxaList = taxonList.GetGenericList().ToTaxonViewModelList();
            return taxaList;
        }

        public List<TaxonViewModel> GetAllTaxaBySearchCriteriaViewModel()
        {
            TaxonList taxonList = CoreData.TaxonManager.GetTaxa(UserContext, TaxaSetting.TaxonIds.ToList());
            List<TaxonViewModel> taxaList = taxonList.GetGenericList().ToTaxonViewModelList();
            return taxaList;
        }
    }
}

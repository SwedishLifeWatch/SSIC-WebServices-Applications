using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.General
{
    public class TaxonTreeViewManager : ViewManagerBase
    {
        public TaxonTreeViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        public List<CategoryTaxaViewModel> GetCategoryTaxaList(List<int> taxonIds)
        {
            List<CategoryTaxaViewModel> categoryTaxaList;
            TaxonList taxonList = CoreData.TaxonManager.GetTaxa(UserContext, taxonIds);
            IList<ITaxon> genericList = taxonList.GetGenericList();
            Dictionary<int, CategoryTaxaViewModel> categoryTaxaDictionary = new Dictionary<int, CategoryTaxaViewModel>();

            foreach (ITaxon taxon in genericList)
            {
                if (!categoryTaxaDictionary.ContainsKey(taxon.Category.Id))
                {
                    categoryTaxaDictionary.Add(taxon.Category.Id, new CategoryTaxaViewModel(taxon.Category));
                }
                categoryTaxaDictionary[taxon.Category.Id].Taxa.Add(TaxonViewModel.CreateFromTaxon(taxon));
            }
            categoryTaxaList = categoryTaxaDictionary.Values.OrderBy(x => x.CategorySortOrder).ToList();
            return categoryTaxaList;
        }

        public List<CategoryTaxaViewModel> GetCategoryTaxaList(int taxonId)
        {
            List<CategoryTaxaViewModel> categoryTaxaList;
            List<ITaxon> taxonAndAllChildren = GetTaxonAndAllChildren(taxonId);
            Dictionary<int, CategoryTaxaViewModel> categoryTaxaDictionary = new Dictionary<int, CategoryTaxaViewModel>();

            foreach (ITaxon taxon in taxonAndAllChildren)
            {                
                if (!categoryTaxaDictionary.ContainsKey(taxon.Category.Id))
                {
                    categoryTaxaDictionary.Add(taxon.Category.Id, new CategoryTaxaViewModel(taxon.Category));
                }
                categoryTaxaDictionary[taxon.Category.Id].Taxa.Add(TaxonViewModel.CreateFromTaxon(taxon));
            }
            categoryTaxaList = categoryTaxaDictionary.Values.OrderBy(x => x.CategorySortOrder).ToList();
            return categoryTaxaList;
        }

        public void GetTaxonTree(int taxonId)
        {
            ITaxon taxon = CoreData.TaxonManager.GetTaxon(UserContext, taxonId);
            //CoreData.TaxonManager.GetTaxonTrees()
            ITaxonTreeNode taxonTreeNode = taxon.GetTaxonTree(UserContext, true);
            TaxonList childTaxa = taxonTreeNode.GetChildTaxa();

            ITaxonTreeNode taxonTreeNode2 = taxon.GetChildTaxonTree(UserContext, true);
            TaxonList childTaxa2 = taxonTreeNode.GetChildTaxa();
        }

        public List<ITaxon> GetAllSpecies(int taxonId)
        {
            ITaxonCategory speciesTaxonCategory = CoreData.TaxonManager.GetTaxonCategory(UserContext, TaxonCategoryId.Species);

            List<ITaxon> taxonAndAllChildren = GetTaxonAndAllChildren(taxonId);
            List<ITaxon> speciesTaxa = new List<ITaxon>();
            foreach (ITaxon taxon in taxonAndAllChildren)
            {
                if (taxon.Category.SortOrder >= speciesTaxonCategory.SortOrder)
                {
                    speciesTaxa.Add(taxon);
                }
            }
            return speciesTaxa;
        }

        public List<ITaxon> GetTaxonAndAllChildren(int taxonId)
        {
            List<ITaxon> taxa = new List<ITaxon>();
            ITaxon taxon = CoreData.TaxonManager.GetTaxon(UserContext, taxonId);
            taxa.Add(taxon);

            ITaxonTreeNode taxonTreeNode = taxon.GetChildTaxonTree(UserContext, true);
            TaxonList childTaxa = taxonTreeNode.GetChildTaxa();
            taxa.AddRange(childTaxa);
            return taxa;            
        }

        public void GetAllChildSpecies(int taxonId)
        {
        }

        //public Dictionary<int, ITaxonTreeNode> GetTaxonTreeNode(ITaxon taxon)
        //{
        //    Dictionary<int, ITaxonTreeNode> taxonTrees = new Dictionary<int, ITaxonTreeNode>();

        //    foreach (ITaxonTreeNode taxonTreeNode in TaxonTree.GetTaxonTreeNodes())
        //    {
        //        _taxonTrees[taxonTreeNode.Taxon.Id] = taxonTreeNode;
        //    }
            
        //    return (ITaxonTreeNode)(_taxonTrees[taxon.Id]);
        //}
    }
}

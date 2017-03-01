using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon
{
    public class CategoryTaxaViewModel
    {
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public int CategorySortOrder { get; set; }
        public List<TaxonViewModel> Taxa { get; set; }

        public CategoryTaxaViewModel(ITaxonCategory category)
        {
            CategoryName = category.Name;
            CategoryId = category.Id;
            CategorySortOrder = category.SortOrder;
            Taxa = new List<TaxonViewModel>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Export
{
    public class ExportTaxonCategory : ICloneable
    {
        public bool IsChecked { get; set; }
        //public int Id { get; set; }
        public string CategoryName { get; set; }
        public int SortOrder { get; set; }
        public int CategoryId { get; set; }

        public static ExportTaxonCategory Create(ITaxonCategory taxonCategory)
        {
            return Create(taxonCategory, false); 
        }

        public static ExportTaxonCategory Create(ITaxonCategory taxonCategory, bool isChecked)
        {
            var model = new ExportTaxonCategory();
            model.IsChecked = isChecked;
            model.CategoryName = taxonCategory.Name;
            model.CategoryId = taxonCategory.Id;
            //model.Id = taxonCategory.Id;
            model.SortOrder = taxonCategory.SortOrder;
            return model;
        }

        public object Clone()
        {
            var model = new ExportTaxonCategory();
            model.IsChecked = this.IsChecked;
            model.CategoryName = this.CategoryName;
            model.CategoryId = this.CategoryId;
            //model.Id = this.Id;
            model.SortOrder = this.SortOrder;
            return model;
        }
    }
}

//Statistics
//public int TotalTaxonCount { get; set; }
//public int NationalTaxonCount { get; set; }
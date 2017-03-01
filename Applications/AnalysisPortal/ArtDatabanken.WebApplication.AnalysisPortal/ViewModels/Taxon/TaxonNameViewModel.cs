using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon
{
    public class TaxonNameViewModel
    {
        public TaxonNameViewModel(ITaxonName taxonName, ITaxon taxon)
        {
            this.Version = taxonName.Version;
            this.TaxonCategorySortOrder = taxon.Category.SortOrder;
            this.GUID = taxonName.Guid;
            this.Name = taxonName.Name;
            this.Author = taxonName.Author ?? "";
            this.CategoryName = taxonName.Category.Name;
            this.NameStatus = taxonName.Status.Name;
            this.IsOriginal = taxonName.IsOriginalName;
            this.IsRecommended = taxonName.IsRecommended;
            this.CategoryId = taxonName.Category.Id;
        }

        public int Version { get; set; }
        public int TaxonCategorySortOrder { get; set; }
        public string GUID { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string CategoryName { get; set; }
        public string NameStatus { get; set; }
        public bool IsOriginal { get; set; }
        public bool IsRecommended { get; set; }
        public int CategoryId { get; set; }

        public bool IsScientificName
        {
            get { return this.CategoryId == (Int32)TaxonNameCategoryId.ScientificName; }
        }
    }
}

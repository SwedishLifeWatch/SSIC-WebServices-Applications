using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon
{
    /// <summary>
    /// A class that represents a related taxon
    /// </summary>
    public class RelatedTaxonViewModel
    {
        public int TaxonId { get; set; }
        public string Category { get; set; }
        public int SortOrder { get; set; }
        public string ScientificName { get; set; }
        public DateTime? EndDate { get; set; }
        public string CommonName { get; set; }

        public RelatedTaxonViewModel(ITaxon taxon, ITaxonCategory taxonCategory, DateTime? endDate)
        {
            this.CommonName = taxon.CommonName.IsNotEmpty() ? taxon.CommonName : "";
            this.EndDate = endDate;
            this.ScientificName = taxon.ScientificName.IsEmpty() ? string.Format("({0})", Resource.ErrorNameIsMissing) : taxon.ScientificName;
            this.SortOrder = taxonCategory.SortOrder;
            this.Category = taxonCategory.Name;
            this.TaxonId = taxon.Id;
        }
    }
}

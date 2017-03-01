using System;
using System.Collections.Generic;
using ArtDatabanken.WebApplication.Dyntaxa.Data.SortTaxon;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public class SortChildTaxaViewModel
    {        
        public string SortableChildrenLabel { get { return Resources.DyntaxaResource.TaxonSortChildTaxaSortableChildren; } }        

        public int RevisionId { get; set; }

        /// <summary>
        /// Get the internal taxon object.
        /// </summary>
        public int TaxonId { get; set; }
        
        /// <summary>
        /// Taxon category the taxon, e.g. Species, Genus or Family.
        /// </summary>
        public string TaxonCategory { get; set; }

        /// <summary>
        /// Recommended Scientific Name of the taxon.
        /// </summary>
        public string ScientificName { get; set; }
        
        /// <summary>
        /// Recommended common name of the taxon.
        /// </summary>
        public string CommonName { get; set; }

        public List<SortChildTaxonItem> SortChildTaxaList { get; set; }
    }

    public class SortChildTaxonItemComparer : IComparer<SortChildTaxonItem>
    {
        private bool asc = true;

        public SortChildTaxonItemComparer(bool asc)
        {
            this.asc = asc;
        }

        public int Compare(SortChildTaxonItem x, SortChildTaxonItem y)
        {
            if (asc)
            {
                return x.ChildScientificName.CompareTo(y.ChildScientificName);
            }
            else
            {
                return -x.ChildScientificName.CompareTo(y.ChildScientificName);
            }
        }
    }
}

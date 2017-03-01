using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Navigation
{
    public class TaxonTreeViewTaxon
    {
        public int TaxonId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public bool IsCurrentRoot { get; set; }

        public TaxonTreeViewTaxon(int taxonId, string name)
        {
            TaxonId = taxonId;
            Name = name;
        }

        public TaxonTreeViewTaxon(int taxonId, string name, string category)
        {
            TaxonId = taxonId;
            Name = name;
            Category = category;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.SortTaxon
{
    public class SortChildTaxonItem
    {
        /// <summary>
        /// Get the internal taxon object.
        /// </summary>
        public int ChildTaxonId { get; set; }

        /// <summary>
        /// Taxon category the taxon, e.g. Species, Genus or Family.
        /// </summary>
        public string ChildTaxonCategory { get; set; }

        /// <summary>
        /// Recommended Scientific Name of the taxon.
        /// </summary>
        public string ChildScientificName { get; set; }

        /*
        /// <summary>
        /// Recommended common name of the taxon.
        /// </summary>
        public string CommonName { get; set; }
        */
    }
}

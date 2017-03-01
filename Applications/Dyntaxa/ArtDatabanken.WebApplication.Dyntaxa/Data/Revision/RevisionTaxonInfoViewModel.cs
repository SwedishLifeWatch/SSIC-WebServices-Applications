using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using Resources;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// View model for Taxon summary    
    /// </summary>
    public class RevisionTaxonInfoViewModel
    {               
        /// <summary>
        /// Id of the revision.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Taxon category of the taxon, e.g. Species, Genus or Family.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The sort order for the taxon category
        /// Decides if the scientific name should be rendered italic or not
        /// </summary>
        public int CategorySortOrder { get; set; }

        /// <summary>
        /// Taxon scientific name
        /// </summary>
        public string ScientificName { get; set; }

        /// <summary>
        /// Taxon common name
        /// </summary>
        public string CommonName { get; set; }

        public object MainHeaderText { get; set; }

        public string RevisionText { get; set; }
    }
}
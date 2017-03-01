using System.Collections.Generic;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    public class RevisionListViewModel
    {
        /// <summary>
        /// Indication of whether data in this view should be possible to change, ie depending on logged in user.
        /// </summary>
        public bool IsViewReadonly { get; set; }

        ///// <summary>
        ///// Get the internal taxon object.
        ///// </summary>
        public string TaxonId { get; set; }

        /// <summary>
        /// Get the description for internal taxon object.
        /// </summary>
        public string TaxonDescription { get; set; }

        /// <summary>
        /// Taxon category the taxon, e.g. Species, Genus or Family.
        /// </summary>
        public string TaxonCategory { get; set; }

        /// <summary>
        /// Recommended Scientific Name of the taxon.
        /// </summary>
        public string TaxonScientificName { get; set; }

        /// <summary>
        /// Recommended common name of the taxon.
        /// </summary>
        public string TaxonCommonName { get; set; }

        /// <summary>
        /// Get the status for internal taxon object.
        /// </summary>
       // public string TaxonStatus { get; set; }

        /// <summary>
        /// A  list for showing or not avaliable revision status ie Preliminary, Ongoing, Published.
        /// </summary>
        public IList<RevisionStatusItemModelHelper> RevisionStatus { get; set; }

        /// <summary>
        /// A  list for showing all avaliable revision.
        /// </summary>
        public IList<RevisionItemModel> Revisions { get; set; }

        /// <summary>
        /// Indicates if revsions only assigned to selected taxon should be visible.
        /// </summary>
        public bool ShowTaxonNameLabelForRevisions { get; set; }
   
        /// <summary>
        /// Holds information on if only revisons beloning to a taxon should be visible or all revisions
        /// </summary>
        public RevisionSelectionItemModelHelper RevisionSelectionItemHelper { get; set; }

        public string EventListLabel
        {
            get
            {
                return Resources.DyntaxaResource.RevisionListEventListTitleLabelText;
            }
        }       
    }
}

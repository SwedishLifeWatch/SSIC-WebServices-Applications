using System;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// Holds information to be needed for displaying a list of dyntaxa revisions.
    /// </summary>
    public class RevisionItemModel
    {
        /// <summary>
        /// GUID for this revision object.
        /// </summary>
        public string GUID { get; set; }

        /// <summary>
        /// Revision id
        /// </summary>
        public int RevisionId { get; set; }

        /// <summary>
        /// Taxon category name for revision taxon.
        /// </summary>
        public string TaxonCategory { get; set; }

        /// <summary>
        /// Taxon scentific recommended name for revision taxon.
        /// </summary>
        public string TaxonScentificRecomendedName { get; set; }

       /// <summary>
       /// The status of the revision ie Created, Ongoing etc.
       /// </summary>
        public string RevisionStatus { get; set; }

        /// <summary>
        /// Start date for the revision.
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// Expected (for ongoing/created revision) or published date for revsion.
        /// </summary>
        public string PublishingDate { get; set; }

        /// <summary>
        /// Indication of whether data for this revision should be possible to change, ie depending if logged in user.
        /// har the privilige to edit the taxon.
        /// </summary>
        public bool IsRevisionEditable { get; set; }

        /// <summary>
        /// Indication of whether data for this revision is possible to start.
        /// </summary>
        public bool IsRevisionPossibleToStart { get; set; }

        /// <summary>
        /// Indication of whether data for this revision is possible to stop ie started.
        /// </summary>
        public bool IsRevisionPossibleToStop { get; set; }

        /// <summary>
        /// Indication of whether data for this revision  is possible to delete.
        /// </summary>
        public bool IsRevisionPossibleToDelete { get; set; }
    }
}

using System;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Holds information to be needed for displaying a list of dyntaxa revisions.
    /// </summary>
    public class RevisionSelectionItemModelHelper
    {
        /// <summary>
        /// Indicated if only revisions for a taxon is to be selected
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Holds revision selection status id.
        /// </summary>
        public int RevisionSelctionStatusId { get; set; } 
    }
}

// -----------------------------------------------------------------------
// <copyright file="TaxonRelation.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ArtDatabanken.Data.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TaxonRelation
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets RelatedTaxon.
        /// </summary>
        public Taxon RelatedTaxon { get; set; }

        /// <summary>
        /// Gets or sets ValidFromDate.
        /// </summary>
        public DateTime ValidFromDate { get; set; }

        /// <summary>
        /// Gets or sets ValidToDate.
        /// </summary>
        public DateTime ValidToDate { get; set; }

        /// <summary>
        /// Gets or sets LumpSplitEvent.
        /// </summary>
        public TaxonRevisionEvent TaxonRevisionEvent { get; set; }

        /// <summary>
        /// Gets or sets ChangedInRevisionEvent.
        /// </summary>
        public TaxonRevisionEvent ChangedInTaxonRevisionEvent { get; set; }

        /// <summary>
        /// Gets or sets IsPublished.
        /// </summary>
        public Boolean IsPublished { get; set; }
    }
}

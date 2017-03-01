using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Taxon
{
    /// <summary>
    /// This class is a view model for SwedishOccurrenceSummary partial view
    /// </summary>
    public class SwedishOccurrenceSummaryViewModel
    {
        public string SwedishOccurrence { get; set; }
        public string SwedishHistory { get; set; }
        public string RedListInfo { get; set; }
        public LinkItem RedListLink { get; set; }
        public ISpeciesFact SwedishOccurrenceFact { get; set; }
        public ISpeciesFact SwedishHistoryFact { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="SwedishOccurrenceSummaryViewModel"/> should be shown.
        /// </summary>
        /// <value>
        ///   <c>true</c> if show; otherwise, <c>false</c>.
        /// </value>
        public bool Show
        {
            get
            {
                if (string.IsNullOrEmpty(SwedishOccurrence) && string.IsNullOrEmpty(SwedishHistory) && string.IsNullOrEmpty(RedListInfo) && RedListLink == null)
                {
                    return false;
                }
                return true;
            }
        }

        public ModelLabels Labels
        {
            get
            {
                if (_labels == null)
                {
                    _labels = new ModelLabels();
                }
                return _labels;
            }
        }

        private ModelLabels _labels;

        /// <summary>
        /// Localized labels
        /// </summary>
        public class ModelLabels
        {
            public string SwedishHistoryLabel
            {
                get { return Resources.DyntaxaResource.TaxonSummarySwedishHistoryLabel; }
            }

            public string SwedishOccurrenceLabel
            {
                get { return Resources.DyntaxaResource.TaxonSummarySwedishOccurrenceLabel; }
            }

            public string RedListClassLabel
            {
                get { return Resources.DyntaxaResource.TaxonInfoSwedishOccurrenceSummaryRedListClass; }
            }
        }
    }
}

using System;
using ArtDatabanken;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    /// <summary>
    /// List class for the ITaxonId interface.
    /// </summary>
    [Serializable]
    public class TaxonIdList : DataId32List<ITaxonId>
    {
        /// <summary>
        /// Constructor for the TaxonIdList class.
        /// </summary>
        public TaxonIdList()
            : base(true)
        {
        }

        /// <summary>
        /// Get a copy of this taxon id list.
        /// </summary>
        /// <returns>A copy of this taxon id list.</returns>
        public TaxonIdList Clone()
        {
            TaxonIdList taxa;

            taxa = new TaxonIdList();
            taxa.AddRange(this);
            return taxa;
        }

        /// <summary>
        /// Merge data object with this list.
        /// Only objects that are not already in the list
        /// are added to the list.
        /// </summary>
        /// <param name='taxa'>The data to merge.</param>
        public void Merge(TaxonList taxa)
        {
            if (taxa.IsNotEmpty())
            {
                foreach (ITaxon taxon in taxa)
                {
                    this.Merge(new TaxonIdImplementation(taxon.Id));
                }
            }
        }
    }
}

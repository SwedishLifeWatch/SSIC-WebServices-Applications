using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    /// <summary>
    /// Information about a taxon id.
    /// </summary>
    [Serializable]
    public class TaxonIdImplementation : ITaxonId
    {
        /// <summary>
        /// Crete a TaxonIdImplementation instance.
        /// </summary>
        /// <param name="taxonId">Taxon id.</param>
        public TaxonIdImplementation(int taxonId)
        {
            Id = taxonId;
        }

        /// <summary>
        /// Id for this taxon.
        /// </summary>
        public int Id { get; set; }
    }
}

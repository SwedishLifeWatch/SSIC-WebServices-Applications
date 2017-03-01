using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class holds taxon tree filter information.
    /// </summary>
    public class TaxonTreeSearchCriteria
    {
        private List<Int32> _restrictSearchToTaxonIds;
        private List<Int32> _restrictSearchToTaxonTypeIds;
        private TaxonInformationType _taxonInformationType;

        /// <summary>
        /// Create a TaxonTreeSearchCriteria instance.
        /// </summary>
        public TaxonTreeSearchCriteria()
        {
            _restrictSearchToTaxonIds = null;
            _restrictSearchToTaxonTypeIds = null;
            _taxonInformationType = TaxonInformationType.Basic;
        }

        /// <summary>
        /// Limit search to taxa.
        /// </summary>
        public List<Int32> RestrictSearchToTaxonIds
        {
            get { return _restrictSearchToTaxonIds; }
            set { _restrictSearchToTaxonIds = value; }
        }

        /// <summary>The types of taxon that is to be searched.</summary>
        public List<Int32> RestrictSearchToTaxonTypeIds
        {
            get { return _restrictSearchToTaxonTypeIds; }
            set { _restrictSearchToTaxonTypeIds = value; }
        }

        /// <summary>Type of taxon information to return.</summary>
        public TaxonInformationType TaxonInformationType
        {
            get { return _taxonInformationType; }
            set { _taxonInformationType = value; }
        }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public void CheckData()
        {
            // Nothing to check yet.
        }
    }
}

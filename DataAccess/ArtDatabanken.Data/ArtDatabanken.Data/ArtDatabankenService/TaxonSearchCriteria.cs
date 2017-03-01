using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class holds taxa filter information.
    /// </summary>
    public class TaxonSearchCriteria
    {
        private Boolean _restrictSearchToSwedishSpecies;
        private Boolean _restrictReturnToSwedishSpecies;
        private List<Int32> _restrictReturnToTaxonTypeIds;
        private List<Int32> _restrictSearchToTaxonIds;
        private List<Int32> _restrictSearchToTaxonTypeIds;
        private String _taxonNameSearchString;
        private TaxonInformationType _taxonInformationType;
        private WebService.TaxonSearchScope _restrictReturnToScope;

        /// <summary>
        /// Create a TaxonSearchCriteria instance.
        /// </summary>
        public TaxonSearchCriteria()
        {
            _restrictReturnToScope = WebService.TaxonSearchScope.NoScope;
            _restrictReturnToTaxonTypeIds = null;
            _restrictReturnToSwedishSpecies = false;
            _restrictSearchToSwedishSpecies = false;
            _restrictSearchToTaxonIds = null;
            _restrictSearchToTaxonTypeIds = null;
            _taxonInformationType = TaxonInformationType.Basic;
            _taxonNameSearchString = null;
        }

        /// <summary>Scope for taxa that is returned.</summary>
        public WebService.TaxonSearchScope RestrictReturnToScope
        {
            get { return _restrictReturnToScope; }
            set { _restrictReturnToScope = value; }
        }

        /// <summary>
        /// 	<c>true</c> if [exclude none swedish species]; otherwise, <c>false</c>.
        /// </summary>
        public Boolean RestrictReturnToSwedishSpecies
        {
            get { return _restrictReturnToSwedishSpecies; }
            set { _restrictReturnToSwedishSpecies = value; }
        }

        /// <summary>The types of taxon that is to be returned.</summary>
        public List<Int32> RestrictReturnToTaxonTypeIds
        {
            get { return _restrictReturnToTaxonTypeIds; }
            set { _restrictReturnToTaxonTypeIds = value; }
        }

        /// <summary>
        /// 	<c>true</c> if [exclude none swedish species]; otherwise, <c>false</c>.
        /// </summary>
        public Boolean RestrictSearchToSwedishSpecies
        {
            get { return _restrictSearchToSwedishSpecies; }
            set { _restrictSearchToSwedishSpecies = value; }
        }

        /// <summary>
        /// 	Limit taxon search (not taxon return) to taxa.
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

        /// <summary>The taxon name search string.</summary>
        public String TaxonNameSearchString
        {
            get { return _taxonNameSearchString; }
            set { _taxonNameSearchString = value; }
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

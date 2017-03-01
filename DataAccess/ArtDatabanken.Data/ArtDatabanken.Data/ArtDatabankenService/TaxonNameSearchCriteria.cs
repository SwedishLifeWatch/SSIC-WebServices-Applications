using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class holds taxa name filter information.
    /// </summary>
    public class TaxonNameSearchCriteria
    {
        private String _nameSearchString;
        private WebService.SearchStringComparisonMethod _nameSearchMethod;

        /// <summary>
        /// Create a TaxonNameSearchCriteria instance.
        /// </summary>
        public TaxonNameSearchCriteria()
        {
            _nameSearchString = null;
            _nameSearchMethod = WebService.SearchStringComparisonMethod.Like;
        }

        /// <summary>
        /// Name search method.
        /// </summary>
        public WebService.SearchStringComparisonMethod NameSearchMethod
        {
            get { return _nameSearchMethod; }
            set { _nameSearchMethod = value; }
        }

        /// <summary>The search string.</summary>
        public String NameSearchString
        {
            get { return _nameSearchString; }
            set { _nameSearchString = value; }
        }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if name search string is empty.</exception>
        public void CheckData()
        {
            NameSearchString.CheckNotEmpty("NameSearchString");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class hold factor filter information
    /// </summary>
    public class FactorSearchCriteria
    {
        private String _factorNameSearchString;
        private WebService.SearchStringComparisonMethod _nameSearchMethod;
        private Boolean _idInNameSearchString;
        private List<Int32> _restrictSearchToFactorIds;
        private WebService.FactorSearchScope _restrictSearchToScope;
        private WebService.FactorSearchScope _restrictReturnToScope;

        /// <summary>
        /// Create a FactorSearchCriteria instance.
        /// </summary>
        public FactorSearchCriteria()
        {
            _factorNameSearchString = null;
            _nameSearchMethod = WebService.SearchStringComparisonMethod.Like;
            _idInNameSearchString = false;
            _restrictSearchToFactorIds = null;
            _restrictSearchToScope = WebService.FactorSearchScope.NoScope;
            _restrictReturnToScope = WebService.FactorSearchScope.NoScope;
        }

        /// <summary>
        /// The factor name search string.
        /// </summary>
        public String FactorNameSearchString
        {
            get { return _factorNameSearchString; }
            set { _factorNameSearchString = value; }
        }

        /// <summary>
        /// Name search method.
        /// </summary>
        public WebService.SearchStringComparisonMethod NameSearchMethod
        {
            get { return _nameSearchMethod; }
            set { _nameSearchMethod = value; }
        }

        /// <summary>
        /// Indication whether or not the name search string may be a factor id.
        /// </summary>
        public Boolean IdInNameSearchString
        {
            get { return _idInNameSearchString; }
            set { _idInNameSearchString = value; }
        }


        /// <summary>
        /// Scope for factors that is returned.
        /// </summary>
        public WebService.FactorSearchScope RestrictReturnToScope
        {
            get { return _restrictReturnToScope; }
            set { _restrictReturnToScope = value; }
        }

        /// <summary>
        /// Scope for factors that is searched.
        /// </summary>
        public WebService.FactorSearchScope RestrictSearchToScope
        {
            get { return _restrictSearchToScope; }
            set { _restrictSearchToScope = value; }
        }

        /// <summary>
        /// Limit factor search (not factor return) to factors.
        /// </summary>
        public List<Int32> RestrictSearchToFactorIds
        {
            get { return _restrictSearchToFactorIds; }
            set { _restrictSearchToFactorIds = value; }
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

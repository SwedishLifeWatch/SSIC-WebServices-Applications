using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class handles search criteria used when 
    /// searching factors.
    /// </summary>
    public class FactorSearchCriteria : IFactorSearchCriteria
    {
        /// <summary>
        /// Data context with meta information about this object.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Indication whether or not the name search string may be a factor id.
        /// </summary>
        public Boolean IsIdInNameSearchString { get; set; }

        /// <summary>
        /// The factor name search string.
        /// </summary>
        public IStringSearchCriteria NameSearchString { get; set; }

        /// <summary>
        /// Scope for factors that is returned.
        /// </summary>
        public FactorSearchScope RestrictReturnToScope { get; set; }

        /// <summary>
        /// Limit factor search (not factor return) to factors.
        /// </summary>
        public List<Int32> RestrictSearchToFactorIds { get; set; }

        /// <summary>
        /// Scope for factors that is searched.
        /// </summary>
        public FactorSearchScope RestrictSearchToScope { get; set; }
    }
}
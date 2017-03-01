using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handles search criteria used when 
    /// searching factors.
    /// </summary>
    public interface IFactorSearchCriteria
    {
        /// <summary>
        /// Data context with meta information about this object.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Indication whether or not the name search string may be a factor id.
        /// </summary>
        Boolean IsIdInNameSearchString { get; set; }

        /// <summary>
        /// The factor name search string.
        /// </summary>
        IStringSearchCriteria NameSearchString { get; set; }

        /// <summary>
        /// Scope for factors that is returned.
        /// </summary>
        FactorSearchScope RestrictReturnToScope { get; set; }

        /// <summary>
        /// Limit factor search (not factor return) to factors.
        /// </summary>
        List<Int32> RestrictSearchToFactorIds { get; set; }

        /// <summary>
        /// Scope for factors that is searched.
        /// </summary>
        FactorSearchScope RestrictSearchToScope { get; set; }
    }
}
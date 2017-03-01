using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class handles search criteria used when 
    /// searching roles.
    /// </summary>
    public class TaxonSearchCriteria : ITaxonSearchCriteria
    {
        /// <summary>
        /// Data context with meta information about this object.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Find taxa who have taxonCategoryIds related
        /// to a specified taxon.
        /// </summary>
        public List<Int32> TaxonCategoryIds
        { get; set; }

        /// <summary>
        /// Find taxa who have taxonIds related
        /// to the specified taxon.
        /// </summary>
        public List<Int32> TaxonIds
        { get; set; }

        /// <summary>
        /// Find roles with an identifier
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
         public String TaxonNameSearchString
        { get; set; }

         /// <summary>
         /// The search scope.
         /// AllChildTaxa or AllParentTaxa
         /// </summary>
         public TaxonSearchScope Scope
         { get; set; }

         /// <summary>
         /// Find taxa who have "Swedish immigration history" value
         /// set to one on the int values in this list.
         /// </summary>
         public List<Int32> SwedishImmigrationHistory
         { get; set; }

         /// <summary>
         /// Find taxa who have "Swedish occurrence" value
         /// set to one on the int values in this list.
         /// </summary>
         public List<Int32> SwedishOccurrence
         { get; set; }

         /// <summary>
         /// Restrict to names to taxa that are valid - value true
         /// </summary>
         public Boolean? IsValidTaxon
         { get; set; }

    }
}

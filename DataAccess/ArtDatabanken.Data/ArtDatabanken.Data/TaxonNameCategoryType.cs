using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Contains definitions of taxon name category types.
    /// </summary>
    public class TaxonNameCategoryType : ITaxonNameCategoryType
    {
        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Unique identifier for this taxon name category type.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Id for this taxon name category type.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Unique identifier for this taxon name category type.
        /// </summary>
        public String Identifier { get; set; }
    }
}

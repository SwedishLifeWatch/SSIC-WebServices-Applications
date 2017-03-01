using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    ///  This class contains information about a taxon name category.
    /// </summary>
    [Serializable]
    public class TaxonNameCategory : ITaxonNameCategory
    {
        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Id for taxon name category.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Locale is used together with Type = CommonName.
        /// </summary>
        public ILocale Locale { get; set; }

        /// <summary>
        /// Name of taxon name category.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets ShortName.
        /// </summary>
        public String ShortName { get; set; }

        /// <summary>
        /// Gets or sets SortOrder.
        /// </summary>
        public Int32 SortOrder { get; set; }

        /// <summary>
        /// Type of taxon name category.
        /// </summary>
        public ITaxonNameCategoryType Type { get; set; }
    }
}

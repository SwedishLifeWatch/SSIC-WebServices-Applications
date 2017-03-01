using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information on a taxon name category.
    /// </summary>
    public interface ITaxonNameCategory : IDataId32
    {
        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Locale is used together with Type = CommonName.
        /// </summary>
        ILocale Locale { get; set; }

        /// <summary>
        /// Name of taxon name category.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Gets or sets ShortName.
        /// </summary>
        String ShortName { get; set; }

        /// <summary>
        /// Gets or sets SortOrder.
        /// </summary>        
        Int32 SortOrder { get; set; }

        /// <summary>
        /// Type of taxon name category.
        /// </summary>
        ITaxonNameCategoryType Type { get; set; }
    }
}

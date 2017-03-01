using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about a taxon category.
    /// </summary>
    public interface ITaxonCategory : IDataId32
    {
        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Test if taxon category is a main category.
        /// </summary>
        Boolean IsMainCategory { get; set; }

        /// <summary>
        /// Test if taxon category is taxonomic.
        /// </summary>
        Boolean IsTaxonomic { get; set; }

        /// <summary>
        /// Get the name on definite form.
        /// </summary>
        String NameDefinite { get; }

        /// <summary>
        /// Name of the taxon category.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Id of parent taxon category.
        /// </summary> 
        Int32 ParentId { get; set; }

        /// <summary>
        /// Sort order for this taxon category.
        /// </summary>
        Int32 SortOrder { get; set; }

        /// <summary>
        /// Get parent taxon category.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>Parent taxon category.</returns>
        ITaxonCategory GetParent(IUserContext userContext);
    }
}

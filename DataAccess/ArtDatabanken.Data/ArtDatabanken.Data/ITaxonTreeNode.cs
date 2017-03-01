namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about a taxon tree node.
    /// Taxon trees consists of taxon tree nodes.
    /// </summary>
    public interface ITaxonTreeNode : IDataId32
    {
        /// <summary>
        /// Children to this taxon tree node.
        /// </summary>
        TaxonTreeNodeList Children
        { get; set; }

        /// <summary>
        /// Children to this taxon tree node.
        /// The difference between property Children and property ChildrenCircular are
        /// that taxon tree nodes in this property occurs at least twice in the taxon tree.
        /// </summary>
        TaxonTreeNodeList ChildrenCircular
        { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Parents to this taxon tree node.
        /// </summary>
        TaxonTreeNodeList Parents
        { get; set; }

        /// <summary>
        /// Parents to this taxon tree node.
        /// The difference between property Parents and property ParentsCircular are
        /// that taxon tree nodes in this property occurs at least twice in the taxon tree.
        /// </summary>
        TaxonTreeNodeList ParentsCircular
        { get; set; }

        /// <summary>
        /// Taxon that belongs to this taxon tree node.
        /// </summary>
        ITaxon Taxon
        { get; set; }

        /// <summary>
        /// Get all unique child taxa.
        /// This method operates on current taxon tree in contrast to
        /// the full taxon tree with all taxon tree nodes.
        /// </summary>
        /// <returns>All child taxa.</returns>
        TaxonList GetChildTaxa();

        /// <summary>
        /// Get unique taxon categories for all child taxa.
        /// This method operates on current taxon tree in contrast to
        /// the full taxon tree with all taxon tree nodes.
        /// </summary>
        /// <returns>Taxon categories for all child taxons.</returns>
        TaxonCategoryList GetChildTaxonCategories();

        /// <summary>
        /// Get all unique parent taxa.
        /// This method operates on current taxon tree in contrast to
        /// the full taxon tree with all taxon tree nodes.
        /// </summary>
        /// <returns>All parent taxa.</returns>
        TaxonList GetParentTaxa();

        /// <summary>
        /// Get unique taxon categories for all parent taxa.
        /// This method operates on current taxon tree in contrast to
        /// the full taxon tree with all taxon tree nodes.
        /// </summary>
        /// <returns>Taxon categories for all parent taxons.</returns>
        TaxonCategoryList GetParentTaxonCategories();

        /// <summary>
        /// Get all unique taxa in taxon tree.
        /// This method operates on current taxon tree in contrast to
        /// the full taxon tree with all taxon tree nodes.
        /// </summary>
        /// <returns>All taxa in taxon tree.</returns>
        TaxonList GetTaxa();

        /// <summary>
        /// Get all unique taxon tree nodes.
        /// This method operates on current taxon tree in contrast to
        /// the full taxon tree with all taxon tree nodes.
        /// </summary>
        /// <returns>All taxon tree nodes.</returns>
        TaxonTreeNodeList GetTaxonTreeNodes();
    }
}

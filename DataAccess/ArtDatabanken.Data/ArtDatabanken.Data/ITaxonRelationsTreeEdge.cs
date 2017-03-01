namespace ArtDatabanken.Data
{
    /// <summary>
    /// Taxon relation tree edge interface.
    /// </summary>
    public interface ITaxonRelationsTreeEdge
    {
        /// <summary>
        /// Parent node.
        /// </summary>
        ITaxonRelationsTreeNode Parent { get; set; }

        /// <summary>
        /// Child node.
        /// </summary>
        ITaxonRelationsTreeNode Child { get; set; }

        /// <summary>
        /// Taxon relation.
        /// </summary>
        ITaxonRelation TaxonRelation { get; set; }

        /// <summary>
        /// Gets a value indicating whether this relation is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this relation is valid; otherwise, <c>false</c>.
        /// </value>
        bool IsValid { get; }

        /// <summary>
        /// Gets a value indicating whether this relation is main relation.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this relation is main relation; otherwise, <c>false</c>.
        /// </value>
        bool IsMain { get; }

        /// <summary>
        /// Gets a value indicating whether this relation is published.
        /// </summary>
        /// <value>
        /// <c>true</c> if this relation is published; otherwise, <c>false</c>.
        /// </value>
        bool IsPublished { get; }
    }
}
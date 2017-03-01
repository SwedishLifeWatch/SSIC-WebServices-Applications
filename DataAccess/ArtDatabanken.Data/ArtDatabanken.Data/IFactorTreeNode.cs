namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a node in a factor tree.
    /// </summary>
    public interface IFactorTreeNode : IDataId32
    {
        /// <summary>
        /// Children to this factor tree node.
        /// </summary>
        FactorTreeNodeList Children { get; set; }

        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Factor of this tree node.
        /// </summary>
        IFactor Factor { get; set; }

        /// <summary>
        /// Parents to this factor tree node.
        /// </summary>
        FactorTreeNodeList Parents { get; set; }

        /// <summary>
        /// Add a factor tree node to the children
        /// of this factor tree node.
        /// </summary>
        /// <param name='factorTreeNode'>Factor tree node to add.</param>
        void AddChild(IFactorTreeNode factorTreeNode);

        /// <summary>
        /// Add a factor tree node to the parents
        /// of this factor tree node.
        /// </summary>
        /// <param name='factorTreeNode'>Factor tree node to add.</param>
        void AddParent(IFactorTreeNode factorTreeNode);

        /// <summary>
        /// Get all child factors that belongs to this factor tree node.
        /// The factor for this tree node is also included in the result.
        /// </summary>
        /// <returns>All child factors that belongs to this factor tree node.</returns>
        FactorList GetAllChildFactors();

        /// <summary>
        /// Get all factor tree nodes that belongs
        /// to this factor tree node.
        /// This tree node is also included in the result.
        /// </summary>
        /// <returns>
        /// All factor tree nodes that belongs
        /// to this factor tree node.
        /// </returns>
        FactorTreeNodeList GetAllChildTreeNodes();

        /// <summary>
        /// Get all leaf factors that belongs to this factor tree node.
        /// The factor for this tree node may also 
        /// be included in the result.
        /// </summary>
        /// <returns>All leaf factors that belongs to this factor tree node.</returns>
        FactorList GetAllLeafFactors();

        /// <summary>
        /// Get all leafs that belongs to this factor tree node.
        /// This tree node may also be included in the result.
        /// </summary>
        /// <returns>All leaf tree node that belongs to this factor tree node.</returns>
        FactorTreeNodeList GetAllLeafTreeNodes();
    }
}
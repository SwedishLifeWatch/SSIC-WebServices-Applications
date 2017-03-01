using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents a node in a factor tree.
    /// </summary>
    [Serializable]
    public class FactorTreeNode : IFactorTreeNode
    {
        /// <summary>
        /// Children to this factor tree node.
        /// </summary>
        public FactorTreeNodeList Children { get; set; }

        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Factor of this tree node.
        /// </summary>
        public IFactor Factor { get; set; }

        /// <summary>
        /// Id for this factor tree node.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Parents to this factor tree node.
        /// </summary>
        public FactorTreeNodeList Parents { get; set; }

        /// <summary>
        /// Add a factor tree node to the children
        /// of this factor tree node.
        /// </summary>
        /// <param name='factorTreeNode'>Factor tree node to add.</param>
        public void AddChild(IFactorTreeNode factorTreeNode)
        {
            factorTreeNode.AddParent(this);
            Children.Add(factorTreeNode);
        }

        /// <summary>
        /// Add a factor tree node to the parents
        /// of this factor tree node.
        /// </summary>
        /// <param name='factorTreeNode'>Factor tree node to add.</param>
        public void AddParent(IFactorTreeNode factorTreeNode)
        {
            Parents.Add(factorTreeNode);
        }

        /// <summary>
        /// Get all child factors that belongs to this factor tree node.
        /// The factor for this tree node is also included in the result.
        /// </summary>
        /// <returns>All child factors that belongs to this factor tree node.</returns>
        public FactorList GetAllChildFactors()
        {
            FactorList factors = new FactorList { Factor };

            if (Children.IsNotEmpty())
            {
                foreach (IFactorTreeNode child in Children)
                {
                    factors.AddRange(child.GetAllChildFactors());
                }
            }

            return factors;
        }

        /// <summary>
        /// Get all factor tree nodes that belongs
        /// to this factor tree node.
        /// This tree node is also included in the result.
        /// </summary>
        /// <returns>
        /// All factor tree nodes that belongs
        /// to this factor tree node.
        /// </returns>
        public FactorTreeNodeList GetAllChildTreeNodes()
        {
            FactorTreeNodeList factorTreeNodeList = new FactorTreeNodeList { this };

            if (Children.IsNotEmpty())
            {
                foreach (IFactorTreeNode child in Children)
                {
                    factorTreeNodeList.AddRange(child.GetAllChildTreeNodes());
                }
            }

            return factorTreeNodeList;
        }

        /// <summary>
        /// Get all child factors that belongs to this factor tree node.
        /// The factor for this tree node may also 
        /// be included in the result.
        /// </summary>
        /// <returns>All leaf factors that belongs to this factor tree node.</returns>
        public FactorList GetAllLeafFactors()
        {
            FactorList leaves = new FactorList();

            if (Children.IsEmpty())
            {
                leaves.Add(Factor);
            }
            else
            {
                foreach (IFactorTreeNode child in Children)
                {
                    leaves.AddRange(child.GetAllLeafFactors());
                }
            }

            return leaves;
        }

        /// <summary>
        /// Get all leafs that belongs to this factor tree node.
        /// This tree node may also be included in the result.
        /// </summary>
        /// <returns>All leaf tree node that belongs to this factor tree node.</returns>
        public FactorTreeNodeList GetAllLeafTreeNodes()
        {
            FactorTreeNodeList leaves = new FactorTreeNodeList();

            if (Children.IsEmpty())
            {
                leaves.Add(this);
            }
            else
            {
                foreach (IFactorTreeNode child in Children)
                {
                    leaves.AddRange(child.GetAllLeafTreeNodes());
                }
            }

            return leaves;
        }
    }
}
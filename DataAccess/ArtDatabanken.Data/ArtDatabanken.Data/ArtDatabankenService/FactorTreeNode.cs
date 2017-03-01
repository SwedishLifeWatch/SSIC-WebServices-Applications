using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class represents a node in a factor tree.
    /// </summary>
    [Serializable]
    public class FactorTreeNode : DataSortOrder
    {
        private Factor _factor;
        private FactorTreeNodeList _children;
        private FactorTreeNodeList _parents;

        /// <summary>
        /// Create a Factor tree node instance.
        /// </summary>
        /// <param name='factor'>Factor belonging to this factor tree node.</param>
        public FactorTreeNode(Factor factor)
            : base(factor.Id, factor.SortOrder)
        {
            _factor = factor;
            _children = new FactorTreeNodeList();
            _parents = new FactorTreeNodeList();
        }

        /// <summary>
        /// Get children to this factor tree node.
        /// </summary>
        public FactorTreeNodeList Children
        {
            get { return _children; }
        }

        /// <summary>
        /// Get parents to this factor tree node.
        /// </summary>
        public FactorTreeNodeList Parents
        {
            get { return _parents; }
        }

        /// <summary>
        /// Get factor belonging to this factor tree node.
        /// </summary>
        public Factor Factor
        {
            get { return _factor; }
        }

        /// <summary>
        /// Get all child factors that belongs to this factor tree node.
        /// The factor for this tree node is also included in the result.
        /// </summary>
        public FactorList GetAllChildFactors()
        {
            FactorList factors;

            factors = new FactorList();
            factors.Add(Factor);
            if (_children.IsNotEmpty())
            {
                foreach (FactorTreeNode child in _children)
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
        public FactorTreeNodeList GetAllChildTreeNodes()
        {
            FactorTreeNodeList factorTreeNodeList;

            factorTreeNodeList = new FactorTreeNodeList();
            factorTreeNodeList.Add(this);
            if (_children.IsNotEmpty())
            {
                foreach (FactorTreeNode child in _children)
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
        public FactorList GetAllLeafFactors()
        {
            FactorList leafs;

            leafs = new FactorList();
            if (_children.IsEmpty())
            {
                leafs.Add(Factor);
            }
            else
            {
                foreach (FactorTreeNode child in _children)
                {
                    leafs.AddRange(child.GetAllLeafFactors());
                }
            }
            return leafs;
        }

        /// <summary>
        /// Get all leafs that belongs to this factor tree node.
        /// This tree node may also be included in the result.
        /// </summary>
        public FactorTreeNodeList GetAllLeafTreeNodes()
        {
            FactorTreeNodeList leafs;

            leafs = new FactorTreeNodeList();
            if (_children.IsEmpty())
            {
                leafs.Add(this);
            }
            else
            {
                foreach (FactorTreeNode child in _children)
                {
                    leafs.AddRange(child.GetAllLeafTreeNodes());
                }
            }
            return leafs;
        }

        /// <summary>
        /// Add a factor tree node to the children
        /// of this factor tree node.
        /// </summary>
        /// <param name='factorTreeNode'>Factor tree node to add.</param>
        public void AddChild(FactorTreeNode factorTreeNode)
        {
            factorTreeNode.AddParent(this);
            _children.Add(factorTreeNode);
            
        }

        /// <summary>
        /// Add a factor tree node to the parents
        /// of this factor tree node.
        /// </summary>
        /// <param name='factorTreeNode'>Factor tree node to add.</param>
        public void AddParent(FactorTreeNode factorTreeNode)
        {
            _parents.Add(factorTreeNode);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  This class represents a node in a taxon tree.
    /// </summary>
    public class TaxonTreeNode : DataSortOrder
    {
        private Taxon _taxon;
        private TaxonTreeNodeList _children;
        private TaxonTreeNodeList _parents;

        /// <summary>
        /// Create a Taxon tree node instance.
        /// </summary>
        /// <param name='taxon'>Taxon belonging to this taxon tree node.</param>
        public TaxonTreeNode(Taxon taxon)
            : base(taxon.Id, taxon.SortOrder)
        {
            _taxon = taxon;
            _children = new TaxonTreeNodeList();
            _parents = new TaxonTreeNodeList();
        }

        /// <summary>
        /// Get children to this taxon tree node.
        /// </summary>
        public TaxonTreeNodeList Children
        {
            get { return _children; }
        }

        /// <summary>
        /// Get parents to this taxon tree node.
        /// </summary>
        public TaxonTreeNodeList Parents
        {
            get { return _parents; }
        }

        /// <summary>
        /// Get taxon belonging to this taxon tree node.
        /// </summary>
        public Taxon Taxon
        {
            get { return _taxon; }
        }

        /// <summary>
        /// Add a taxon tree node to the children
        /// of this taxon tree node.
        /// </summary>
        /// <param name='taxonTreeNode'>Taxon tree node to add.</param>
        public void AddChild(TaxonTreeNode taxonTreeNode)
        {
            _children.Add(taxonTreeNode);
            taxonTreeNode.AddParent(this);
        }

        /// <summary>
        /// Add a taxon tree node to the parents
        /// of this taxon tree node.
        /// </summary>
        /// <param name='taxonTreeNode'>Taxon tree node to add.</param>
        public void AddParent(TaxonTreeNode taxonTreeNode)
        {
            _parents.Add(taxonTreeNode);
        }
    }
}

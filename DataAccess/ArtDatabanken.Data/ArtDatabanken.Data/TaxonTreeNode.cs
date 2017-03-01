using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about a taxon tree node.
    /// Taxon trees consists of taxon tree nodes.
    /// </summary>
    public class TaxonTreeNode : ITaxonTreeNode
    {
        /// <summary>
        /// Children to this taxon tree node.
        /// </summary>
        public TaxonTreeNodeList Children
        { get; set; }

        /// <summary>
        /// Children to this taxon tree node.
        /// The difference between property Children and property ChildrenCircular are
        /// that taxon tree nodes in this property occurs at least twice in the taxon tree.
        /// </summary>
        public TaxonTreeNodeList ChildrenCircular
        { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Id of a taxon.
        /// </summary>
        public Int32 Id
        {
            get { return Taxon.Id; }
            set { }
        }

        /// <summary>
        /// Parents to this taxon tree node.
        /// </summary>
        public TaxonTreeNodeList Parents
        { get; set; }

        /// <summary>
        /// Parents to this taxon tree node.
        /// The difference between property Parents and property ParentsCircular are
        /// that taxon tree nodes in this property occurs at least twice in the taxon tree.
        /// </summary>
        public TaxonTreeNodeList ParentsCircular
        { get; set; }

        /// <summary>
        /// Taxon that belongs to this taxon tree node.
        /// </summary>
        public ITaxon Taxon
        { get; set; }

        /// <summary>
        /// Get all unique child taxa.
        /// This method operates on current taxon tree in contrast to
        /// the full taxon tree with all taxon tree nodes.
        /// </summary>
        /// <returns>All child taxa.</returns>
        public virtual TaxonList GetChildTaxa()
        {
            TaxonList childTaxa;

            childTaxa = new TaxonList(true);
            if (Children.IsNotEmpty())
            {
                // Add taxa for child taxon tree node.
                foreach (ITaxonTreeNode childTaxonTreeNode in Children)
                {
                    GetChildTaxa(childTaxonTreeNode, childTaxa);
                }
            }

            childTaxa.Sort();
            return childTaxa;
        }

        /// <summary>
        /// Get all unique child taxa.
        /// This method operates on current taxon tree in contrast to
        /// the full taxon tree with all taxon tree nodes.
        /// </summary>
        /// <param name="taxonTreeNode">Current child taxon tree node.</param>
        /// <param name="childTaxa">Accumulated child taxa so far.</param>
        private void GetChildTaxa(ITaxonTreeNode taxonTreeNode,
                                  TaxonList childTaxa)
        {
            // Add the taxon for this taxon tree node.
            childTaxa.Merge(taxonTreeNode.Taxon);

            if (taxonTreeNode.Children.IsNotEmpty())
            {
                // Add taxa for child taxon tree node.
                foreach (ITaxonTreeNode childTaxonTreeNode in taxonTreeNode.Children)
                {
                    GetChildTaxa(childTaxonTreeNode, childTaxa);
                }
            }
        }

        /// <summary>
        /// Get unique taxon categories for all child taxa.
        /// This method operates on current taxon tree in contrast to
        /// the full taxon tree with all taxon tree nodes.
        /// </summary>
        /// <returns>Taxon categories for all child taxons.</returns>
        public virtual TaxonCategoryList GetChildTaxonCategories()
        {
            TaxonCategoryList childTaxonCategories;

            childTaxonCategories = new TaxonCategoryList();
            foreach (ITaxon childTaxon in GetChildTaxa())
            {
                if (!childTaxonCategories.Exists(childTaxon.Category))
                {
                    childTaxonCategories.Add(childTaxon.Category);
                }
            }
            childTaxonCategories.Sort();
            return childTaxonCategories;
        }

        /// <summary>
        /// Get child taxon tree nodes.
        /// This method operates on current taxon tree in contrast to
        /// the full taxon tree with all taxon tree nodes.
        /// </summary>
        /// <param name="taxonTreeNode">Current child taxon tree node.</param>
        /// <param name="childTaxonTreeNodes">Accumulated child taxon tree nodes so far.</param>
        private void GetChildTaxonTreeNode(ITaxonTreeNode taxonTreeNode,
                                           TaxonTreeNodeList childTaxonTreeNodes)
        {
            // Add this taxon tree node.
            childTaxonTreeNodes.Merge(taxonTreeNode);

            if (taxonTreeNode.Children.IsNotEmpty())
            {
                // Add child taxon tree nodes.
                foreach (ITaxonTreeNode childTaxonTreeNode in taxonTreeNode.Children)
                {
                    GetChildTaxonTreeNode(childTaxonTreeNode, childTaxonTreeNodes);
                }
            }
        }

        /// <summary>
        /// Get all unique parent taxa.
        /// This method operates on current taxon tree in contrast to
        /// the full taxon tree with all taxon tree nodes.
        /// </summary>
        /// <returns>All parent taxa.</returns>
        public virtual TaxonList GetParentTaxa()
        {
            TaxonList parentTaxa;

            parentTaxa = new TaxonList(true);
            if (Parents.IsNotEmpty())
            {
                // Add taxa for parent taxon tree node.
                foreach (ITaxonTreeNode parentTaxonTreeNode in Parents)
                {
                    GetParentTaxa(parentTaxonTreeNode, parentTaxa);
                }
            }

            parentTaxa.Sort();
            return parentTaxa;
        }

        /// <summary>
        /// Get all unique parent taxa.
        /// This method operates on current taxon tree in contrast to
        /// the full taxon tree with all taxon tree nodes.
        /// </summary>
        /// <param name="taxonTreeNode">Current parent taxon tree node.</param>
        /// <param name="parentTaxa">Accumulated parent taxa so far.</param>
        private void GetParentTaxa(ITaxonTreeNode taxonTreeNode,
                                   TaxonList parentTaxa)
        {
            // Add the taxon for this taxon tree node.
            parentTaxa.Merge(taxonTreeNode.Taxon);

            if (taxonTreeNode.Parents.IsNotEmpty())
            {
                // Add taxa for parent taxon tree node.
                foreach (ITaxonTreeNode parentTaxonTreeNode in taxonTreeNode.Parents)
                {
                    GetParentTaxa(parentTaxonTreeNode, parentTaxa);
                }
            }
        }

        /// <summary>
        /// Get unique taxon categories for all parent taxa.
        /// This method operates on current taxon tree in contrast to
        /// the full taxon tree with all taxon tree nodes.
        /// </summary>
        /// <returns>Taxon categories for all parent taxons.</returns>
        public virtual TaxonCategoryList GetParentTaxonCategories()
        {
            TaxonCategoryList parentTaxonCategories;

            parentTaxonCategories = new TaxonCategoryList();
            foreach (ITaxon parentTaxon in GetParentTaxa())
            {
                if (!parentTaxonCategories.Contains(parentTaxon.Category))
                {
                    parentTaxonCategories.Add(parentTaxon.Category);
                }
            }
            parentTaxonCategories.Sort();
            return parentTaxonCategories;
        }

        /// <summary>
        /// Get parent taxon tree nodes.
        /// This method operates on current taxon tree in contrast to
        /// the full taxon tree with all taxon tree nodes.
        /// </summary>
        /// <param name="taxonTreeNode">Current parent taxon tree node.</param>
        /// <param name="parentTaxonTreeNodes">Accumulated parent taxon tree nodes so far.</param>
        private void GetParentTaxonTreeNode(ITaxonTreeNode taxonTreeNode,
                                             TaxonTreeNodeList parentTaxonTreeNodes)
        {
            // Add this taxon tree node.
            parentTaxonTreeNodes.Merge(taxonTreeNode);

            if (taxonTreeNode.Parents.IsNotEmpty())
            {
                // Add parent taxon tree nodes.
                foreach (ITaxonTreeNode parentTaxonTreeNode in taxonTreeNode.Parents)
                {
                    GetParentTaxonTreeNode(parentTaxonTreeNode, parentTaxonTreeNodes);
                }
            }
        }

        /// <summary>
        /// Get all unique taxa in taxon tree.
        /// This method operates on current taxon tree in contrast to
        /// the full taxon tree with all taxon tree nodes.
        /// </summary>
        /// <returns>All taxa in taxon tree.</returns>
        public virtual TaxonList GetTaxa()
        {
            TaxonList taxa;

            taxa = GetParentTaxa();
            taxa.Add(Taxon);
            taxa.AddRange(GetChildTaxa());
            return taxa;
        }

        /// <summary>
        /// Get all unique taxon tree nodes.
        /// This method operates on current taxon tree in contrast to
        /// the full taxon tree with all taxon tree nodes.
        /// </summary>
        /// <returns>All taxon tree nodes.</returns>
        public virtual TaxonTreeNodeList GetTaxonTreeNodes()
        {
            TaxonTreeNodeList taxonTreeNodes;

            taxonTreeNodes = new TaxonTreeNodeList(true);
            if (Parents.IsNotEmpty())
            {
                // Add parent taxon tree nodes.
                foreach (ITaxonTreeNode parentTaxonTreeNode in Parents)
                {
                    GetParentTaxonTreeNode(parentTaxonTreeNode, taxonTreeNodes);
                }
            }

            // Add this taxon tree node.
            taxonTreeNodes.Add(this);

            if (Children.IsNotEmpty())
            {
                // Add child taxon tree nodes.
                foreach (ITaxonTreeNode childTaxonTreeNode in Children)
                {
                    GetChildTaxonTreeNode(childTaxonTreeNode, taxonTreeNodes);
                }
            }

            return taxonTreeNodes;
        }
    }
}

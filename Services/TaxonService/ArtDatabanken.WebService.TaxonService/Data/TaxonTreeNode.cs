using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    /// <summary>
    /// Contains information about a taxon tree node.
    /// This class is only for internal use in TaxonService.
    /// </summary>
    internal class TaxonTreeNode
    {
        /// <summary>
        /// Create an TaxonTreeNode instance.
        /// </summary>
        /// <param name="taxon">Taxon.</param>
        public TaxonTreeNode(WebTaxon taxon)
        {
            ChildRelations = null;
            Children = null;
            ChildrenCircular = null;
            IsValid = taxon.IsValid;
            ParentRelations = null;
            Parents = null;
            SortOrder = taxon.SortOrder;
            TaxonId = taxon.Id;
        }

        /// <summary>
        /// Information about the relation between this
        /// taxon tree node and child taxon tree nodes.
        /// Properties ChildRelations and Children are sorted in the same order.
        /// </summary>
        public List<WebTaxonRelation> ChildRelations
        { get; set; }

        /// <summary>
        /// Children to this taxon tree node.
        /// Properties ChildRelations and Children are sorted in the same order.
        /// </summary>
        public List<TaxonTreeNode> Children
        { get; set; }

        /// <summary>
        /// Children to this taxon tree node.
        /// The difference between property Children and property ChildrenCircular are
        /// that taxon tree nodes in this property occurs at least twice in the taxon tree.
        /// </summary>
        public List<TaxonTreeNode> ChildrenCircular
        { get; set; }

        /// <summary>
        /// Indicates if the taxon, that belongs to this taxon tree node, is valid.
        /// </summary>
        public Boolean IsValid
        { get; set; }

        /// <summary>
        /// Information about the relation between this
        /// taxon tree node and parent taxon tree nodes.
        /// Properties ParentRelations and Parents are sorted in the same order.
        /// </summary>
        public List<WebTaxonRelation> ParentRelations
        { get; set; }

        /// <summary>
        /// Parents to this taxon tree node.
        /// Properties ParentRelations and Parents are sorted in the same order.
        /// </summary>
        public List<TaxonTreeNode> Parents
        { get; set; }

        /// <summary>
        /// Sort order for the taxon that belongs to this taxon tree node.
        /// </summary>
        public Int32 SortOrder { get; set; }

        /// <summary>
        /// Id for the taxon that belongs to this taxon tree node.
        /// </summary>
        public Int32 TaxonId
        { get; set; }

        /// <summary>
        /// Add child information.
        /// </summary>
        /// <param name="childTaxonTreeNode">Child taxon tree node.</param>
        /// <param name="childTaxonRelation">Child taxon relation.</param>
        public void AddChild(TaxonTreeNode childTaxonTreeNode,
                             WebTaxonRelation childTaxonRelation)
        {
            Int32 childIndex;

            if (Children.IsEmpty() ||
                Children[Children.Count - 1].SortOrder <= childTaxonTreeNode.SortOrder)
            {
                // Insert at the end of the lists.
                if (Children.IsNull())
                {
                    ChildRelations = new List<WebTaxonRelation>();
                    Children = new List<TaxonTreeNode>();
                }
                ChildRelations.Add(childTaxonRelation);
                Children.Add(childTaxonTreeNode);
            }
            else
            {
                // Insert into the list.
                for (childIndex = 0; childIndex < Children.Count; childIndex++)
                {
                    if (childTaxonTreeNode.SortOrder < Children[childIndex].SortOrder)
                    {
                        ChildRelations.Insert(childIndex, childTaxonRelation);
                        Children.Insert(childIndex, childTaxonTreeNode);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Add parent information.
        /// </summary>
        /// <param name="parentTaxonTreeNode">Parent taxon tree node.</param>
        /// <param name="parentTaxonRelation">Parent taxon tree relation.</param>
        public void AddParent(TaxonTreeNode parentTaxonTreeNode,
                              WebTaxonRelation parentTaxonRelation)
        {
            Int32 parentIndex;

            if (Parents.IsEmpty() ||
                Parents[Parents.Count - 1].SortOrder <= parentTaxonTreeNode.SortOrder)
            {
                // Insert at the end of the lists.
                if (Parents.IsNull())
                {
                    ParentRelations = new List<WebTaxonRelation>();
                    Parents = new List<TaxonTreeNode>();
                }
                ParentRelations.Add(parentTaxonRelation);
                Parents.Add(parentTaxonTreeNode);
            }
            else
            {
                // Insert into the list.
                for (parentIndex = 0; parentIndex < Parents.Count; parentIndex++)
                {
                    if (parentTaxonTreeNode.SortOrder < Parents[parentIndex].SortOrder)
                    {
                        ParentRelations.Insert(parentIndex, parentTaxonRelation);
                        Parents.Insert(parentIndex, parentTaxonTreeNode);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Test if this tree node is a root taxon tree node,
        /// i.e. it has no parent tree nodes..
        /// </summary>
        /// <param name="isMainRelationRequired">Indicates if only main relations are considered or not.</param>
        /// <param name="isValidRequired">Indicates if only valid parent tree nodes are considered or not.</param>
        /// <returns>True if this tree node is a root taxon tree node.</returns>
        public Boolean IsTreeRoot(Boolean isMainRelationRequired,
                                  Boolean isValidRequired)
        {
            DateTime now;
            Int32 index;

            if (Parents.IsEmpty())
            {
                // No parents. This is a root taxon tree node.
                return true;
            }

            if (!isValidRequired && !isMainRelationRequired)
            {
                // Has parents.
                return false;
            }

            // Check if any parent tree node and parent tree relation 
            // matches specified conditions.
            now = DateTime.Now;
            for (index = 0; index < Parents.Count; index++)
            {
                if ((!isMainRelationRequired ||
                     ParentRelations[index].IsMainRelation) &&
                    (!isValidRequired ||
                     ((now < ParentRelations[index].ValidToDate) &&
                      Parents[index].IsValid)))
                {
                    // At least one valid parent.
                    return false;
                }
            }
            // No valid parents.
            return true;
        }
    }
}

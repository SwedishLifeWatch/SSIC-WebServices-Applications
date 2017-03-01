using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Contains information about a taxon tree node.
    /// </summary>
    [DataContract]
    public class WebTaxonTreeNode : WebData
    {
        /// <summary>
        /// Create a WebTaxon instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebTaxonTreeNode(DataReader dataReader)
        {
            Id = dataReader.GetInt32(TaxonData.ID);
            Taxon = new WebTaxon(dataReader);
            Children = new List<WebTaxonTreeNode>();
            IsChild = false;
            base.LoadData(dataReader);
        }

        /// <summary>
        /// Children to this taxon tree node.
        /// </summary>
        [DataMember]
        public List<WebTaxonTreeNode> Children
        { get; set; }

        /// <summary>
        /// Id for this taxon tree node.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Controll if this taxon tree node is a child
        /// to any other taxon tree node
        /// </summary>
        public Boolean IsChild
        { get; set; }

        /// <summary>
        /// Taxon for this taxon tree node.
        /// </summary>
        [DataMember]
        public WebTaxon Taxon
        { get; set; }

        /// <summary>
        /// Add a taxon tree node to the children
        /// of this taxon tree node.
        /// </summary>
        /// <param name='taxonTreeNode'>Taxon tree node to add.</param>
        public void AddChild(WebTaxonTreeNode taxonTreeNode)
        {
            Int32 taxonTreeIndex;

            if (Children.IsNull())
            {
                Children = new List<WebTaxonTreeNode>();
            }
            taxonTreeNode.IsChild = true;

            // Sort children in Taxon.SortOrder;
            if (Children.IsNotEmpty())
            {
                for (taxonTreeIndex = 0; taxonTreeIndex < Children.Count; taxonTreeIndex++ )
                {
                    if (taxonTreeNode.Taxon.SortOrder < Children[taxonTreeIndex].Taxon.SortOrder)
                    {
                        Children.Insert(taxonTreeIndex, taxonTreeNode);
                        return;
                    }
                }
            }
            Children.Add(taxonTreeNode);
        }

        /// <summary>
        /// Remove children that are not of the specified
        /// taxon types.
        /// </summary>
        /// <param name='taxonTypeIds'>Taxon type ids.</param>
        public void RestrictTaxonTypes(List<Int32> taxonTypeIds)
        {
            Int32 childTaxonTreeIndex, taxonTreeIndex;
            List<WebTaxonTreeNode> children;

            // Handle this taxon tree node.
            if (Children.IsNotEmpty())
            {
                for (taxonTreeIndex = 0; taxonTreeIndex < Children.Count; taxonTreeIndex++)
                {
                    if (!taxonTypeIds.Contains(Children[taxonTreeIndex].Taxon.TaxonTypeId))
                    {   
                        // Compress one taxon type level in tree.
                        // Get childes children.
                        children = Children[taxonTreeIndex].Children;

                        // Remove child.
                        Children.RemoveAt(taxonTreeIndex);

                        // Add childes children.
                        if (children.IsNotEmpty())
                        {
                            childTaxonTreeIndex = taxonTreeIndex;
                            foreach (WebTaxonTreeNode child in children)
                            {
                                Children.Insert(childTaxonTreeIndex, child);
                                childTaxonTreeIndex++;
                            }
                        }

                        taxonTreeIndex--;
                    }
                }
            }

            // Handle childrens taxon tree nodes.
            if (Children.IsNotEmpty())
            {
                foreach (WebTaxonTreeNode taxonTreeNode in Children)
                {
                    taxonTreeNode.RestrictTaxonTypes(taxonTypeIds);
                }
            }
        }
    }
}

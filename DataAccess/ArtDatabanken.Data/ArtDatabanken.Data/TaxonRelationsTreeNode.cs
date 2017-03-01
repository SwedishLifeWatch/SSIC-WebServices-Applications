using System.Collections.Generic;
using System.Linq;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Taxon relations tree node.
    /// </summary>
    public class TaxonRelationsTreeNode : ITaxonRelationsTreeNode
    {
        /// <summary>
        /// Creates a tree node.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        public TaxonRelationsTreeNode(ITaxon taxon)
        {
            Taxon = taxon;
        }

        /// <summary>
        /// Gets or sets the taxon.
        /// </summary>        
        public ITaxon Taxon { get; set; }

        /// <summary>
        /// Gets or sets all edges.
        /// </summary>
        public List<ITaxonRelationsTreeEdge> AllEdges { get; set; }

        /// <summary>
        /// All parent edges.
        /// </summary>
        public List<ITaxonRelationsTreeEdge> AllParentEdges { get; set; }
        
        /// <summary>
        /// All child edges.
        /// </summary>
        public List<ITaxonRelationsTreeEdge> AllChildEdges { get; set; }

        /// <summary>
        /// The valid main children.
        /// </summary>
        public List<ITaxonRelationsTreeEdge> ValidMainChildren { get; set; }
        
        /// <summary>
        /// The nonvalid main children.
        /// </summary>
        public List<ITaxonRelationsTreeEdge> NonvalidMainChildren { get; set; }
        
        /// <summary>
        /// The valid secondary children.
        /// </summary>
        public List<ITaxonRelationsTreeEdge> ValidSecondaryChildren { get; set; }
        
        /// <summary>
        /// The nonvalid secondary children.
        /// </summary>
        public List<ITaxonRelationsTreeEdge> NonvalidSecondaryChildren { get; set; }

        /// <summary>
        /// The valid main parents.
        /// </summary>
        public List<ITaxonRelationsTreeEdge> ValidMainParents { get; set; }
        
        /// <summary>
        /// The nonvalid main parents.
        /// </summary>
        public List<ITaxonRelationsTreeEdge> NonvalidMainParents { get; set; }
        
        /// <summary>
        /// The valid secondary parents.
        /// </summary>
        public List<ITaxonRelationsTreeEdge> ValidSecondaryParents { get; set; }
        
        /// <summary>
        /// The nonvalid secondary parents.
        /// </summary>
        public List<ITaxonRelationsTreeEdge> NonvalidSecondaryParents { get; set; }
        
        /// <summary>
        /// The not used edges.
        /// </summary>
        public List<ITaxonRelationsTreeEdge> NotUsedEdges { get; set; }

        /// <summary>
        /// Gets all valid secondary parent nodes in root to leaf order.
        /// </summary>
        /// <returns>
        /// All valid secondary parents.
        /// </returns>
        public List<ITaxonRelationsTreeNode> GetAllValidSecondaryParentNodesInRootToNodeOrder()
        {
            HashSet<ITaxonRelationsTreeNode> onlyMainParentNodesSet = new HashSet<ITaxonRelationsTreeNode>();
            HashSet<ITaxonRelationsTreeNode> resultSet = new HashSet<ITaxonRelationsTreeNode>();

            // Add only main relations
            foreach (ITaxonRelationsTreeNode node in this.AsTopFirstParentNodeIterator(TaxonRelationsTreeParentsIterationMode.OnlyValidMainParents))
            {
                onlyMainParentNodesSet.Add(node);
            }

            // Add both main and secondary relations
            foreach (ITaxonRelationsTreeNode node in this.AsTopFirstBreadthFirstParentNodeIterator(TaxonRelationsTreeParentsIterationMode.BothValidMainAndSecondaryParents))
            {
                resultSet.Add(node);
            }

            // Remove main relations from list with both main and secondary relations
            resultSet.ExceptWith(onlyMainParentNodesSet);

            return resultSet.ToList();
        }

        /// <summary>
        /// Gets all valid parents edges hierarchical from top to bottom (this node).
        /// </summary>
        /// <returns>A list containing all valid parent edges hierarchical from top to bottom.</returns>
        public List<ITaxonRelationsTreeEdge> GetAllValidParentEdgesTopToBottom(bool onlyMainRelations)
        {
            if (onlyMainRelations)
            {
                return this.AsTopDownBreadthFirstParentEdgeIterator(TaxonRelationsTreeParentsIterationMode.OnlyValidMainParents).ToList();
            }
            else
            {
                return this.AsTopDownBreadthFirstParentEdgeIterator(TaxonRelationsTreeParentsIterationMode.BothValidMainAndSecondaryParents).ToList();
            }            
        }

        /// <summary>
        /// Gets the root node.
        /// </summary>        
        public ITaxonRelationsTreeNode RootNode
        {
            get
            {
                List<ITaxonRelationsTreeNode> treeNodes = new List<ITaxonRelationsTreeNode>();
                ITaxonRelationsTreeNode rootNode = null;
                foreach (ITaxonRelationsTreeNode node in this.AsTopFirstParentNodeIterator(TaxonRelationsTreeParentsIterationMode.BothValidMainAndSecondaryParents))
                {
                    treeNodes.Add(node);
                    if (node.ValidMainParents == null && node.ValidSecondaryParents == null)
                    {
                        rootNode = node;
                        return rootNode;
                    }
                }
                treeNodes.Reverse();
               
                return treeNodes[0];
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="TaxonRelationsTreeNode" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="TaxonRelationsTreeNode" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="TaxonRelationsTreeNode" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        protected bool Equals(ITaxonRelationsTreeNode other)
        {
            return Equals(Taxon.Id, other.Taxon.Id);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((ITaxonRelationsTreeNode)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Taxon != null ? Taxon.Id.GetHashCode() : 0;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} [{1}]", Taxon.ScientificName, Taxon.Id);

            //if (!string.IsNullOrEmpty(Taxon.CommonName))
            //{
            //    return string.Format("{0} - {1} ({2})", Taxon.ScientificName, Taxon.CommonName, Taxon.Id);
            //}
            //else
            //{
            //    return string.Format("{0} ({1})", Taxon.ScientificName, Taxon.Id);
            //}            
        }
    }
}
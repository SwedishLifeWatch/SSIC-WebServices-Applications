using System.Collections.Generic;
using System.Text;

namespace ArtDatabanken.Data
{    
    /// <summary>
    /// Taxon relations tree.
    /// </summary>
    public class TaxonRelationsTree
    {
        /// <summary>
        /// Root node.
        /// </summary>
        public ITaxonRelationsTreeNode Root { get; set; }        

        /// <summary>
        /// TreeNode dictionary for fast access to tree nodes.
        /// </summary>
        public Dictionary<int, ITaxonRelationsTreeNode> TreeNodeDictionary { get; set; }

        /// <summary>
        /// All tree nodes.
        /// </summary>
        public List<ITaxonRelationsTreeNode> TreeNodes { get; set; }

        /// <summary>
        /// All tree edges.
        /// </summary>
        public List<ITaxonRelationsTreeEdge> TreeEdges { get; set; }

        /// <summary>
        /// All taxon relations.
        /// </summary>
        public TaxonRelationList OriginalTaxonRelationList { get; set; }

        /// <summary>
        /// Taxa list.
        /// </summary>
        public TaxonList OriginalTaxaList { get; set; }

        /// <summary>
        /// Tree edges that is not put in any of the node lists.
        /// </summary>
        public List<ITaxonRelationsTreeEdge> NotUsedTreeEdges { get; set; }

        /// <summary>
        /// All tree edges.
        /// </summary>
        public List<ITaxonRelationsTreeEdge> AllTreeEdges { get; set; }

        /// <summary>
        /// All tree edges dictionary.
        /// </summary>
        public Dictionary<int, List<ITaxonRelationsTreeEdge>> AllTreeEdgesDictionary { get; set; }

        /// <summary>
        /// All tree nodes.
        /// </summary>
        public List<ITaxonRelationsTreeNode> AllTreeNodes { get; set; }        

        /// <summary>
        /// Root nodes.
        /// </summary>
        public HashSet<ITaxonRelationsTreeNode> RootNodes { get; set; }

        /// <summary>
        /// Valid root nodes.
        /// </summary>
        public HashSet<ITaxonRelationsTreeNode> ValidRootNodes { get; set; }

        /// <summary>
        /// Tree nodes without edges.
        /// </summary>
        public List<ITaxonRelationsTreeNode> TreeNodesWithoutEdges { get; set; }

        /// <summary>
        /// Gets a tree node.
        /// </summary>
        /// <param name="taxonId">The taxon id.</param>
        /// <returns>A tree node or null if not found.</returns>
        public ITaxonRelationsTreeNode GetTreeNode(int taxonId)
        {
            ITaxonRelationsTreeNode node;
            TreeNodeDictionary.TryGetValue(taxonId, out node);
            return node;
        }

        /// <summary>
        /// Tries to get tree node.
        /// </summary>
        /// <param name="taxonId">The taxon identifier.</param>
        /// <param name="treeNode">The tree node.</param>
        /// <returns>True if the node was found; otherwise false.</returns>
        public bool TryGetTreeNode(int taxonId, out ITaxonRelationsTreeNode treeNode)
        {
            return TreeNodeDictionary.TryGetValue(taxonId, out treeNode);
        }

        /// <summary>
        /// Gets the tree edge.
        /// </summary>
        /// <param name="parentTaxonId">The parent taxon identifier.</param>
        /// <param name="childTaxonId">The child taxon identifier.</param>
        /// <returns>A tree edge or null if not found.</returns>
        public ITaxonRelationsTreeEdge GetTreeEdge(int parentTaxonId, int childTaxonId)
        {
            var parentNode = GetTreeNode(parentTaxonId);
            var childNode = GetTreeNode(childTaxonId);

            ITaxonRelationsTreeEdge edge = null;

            //foreach (ITaxonRelationsTreeEdge treeEdge in parentNode.AllChildEdges)                
            if (parentNode.AllEdges != null)
            {
                foreach (ITaxonRelationsTreeEdge treeEdge in parentNode.AllEdges)
                {
                    if (treeEdge.Child.Taxon.Id == childTaxonId)
                    {
                        edge = treeEdge;
                        break;
                    }
                }
            }

            return edge;            
        }

        /// <summary>
        /// Gets the tree nodes.
        /// </summary>
        /// <param name="taxonIds">The taxon ids.</param>
        /// <returns>A list with tree nodes.</returns>
        public List<ITaxonRelationsTreeNode> GetTreeNodes(IEnumerable<int> taxonIds)
        {
            List<ITaxonRelationsTreeNode> treeNodes = new List<ITaxonRelationsTreeNode>();
            foreach (int taxonId in taxonIds)
            {
                ITaxonRelationsTreeNode node;
                if (TreeNodeDictionary.TryGetValue(taxonId, out node))
                {
                    treeNodes.Add(node);
                }
            }

            return treeNodes;
        }

        /// <summary>
        /// Gets all child and parent edges. 
        /// Includes both valid and invalid edges.
        /// Includes both main and secondary parent/children.
        /// </summary>
        /// <param name="taxonIds">The taxon ids.</param>
        /// <returns>A set with all child and parent edges.</returns>
        public HashSet<ITaxonRelationsTreeEdge> GetAllChildAndParentEdges(IEnumerable<int> taxonIds)
        {
            List<ITaxonRelationsTreeNode> treeNodes = GetTreeNodes(taxonIds);
            return GetAllChildAndParentEdges(treeNodes);
        }

        /// <summary>
        /// Gets all child and parent edges. 
        /// Includes both valid and invalid edges.
        /// Includes both main and secondary parent/children.
        /// </summary>        
        /// <param name="taxonId">The taxon id.</param>
        /// <returns>A set with all child and parent edges.</returns>
        public HashSet<ITaxonRelationsTreeEdge> GetAllChildAndParentEdges(int taxonId)
        {
            return GetAllChildAndParentEdges(GetTreeNode(taxonId));
        }

        /// <summary>
        /// Gets all child and parent edges. 
        /// Includes both valid and invalid edges.
        /// Includes both main and secondary parent/children.
        /// </summary>        
        /// <param name="treeNode">The tree node.</param>
        /// <returns>A set with all child and parent edges.</returns>
        public HashSet<ITaxonRelationsTreeEdge> GetAllChildAndParentEdges(ITaxonRelationsTreeNode treeNode)
        {            
            return GetAllChildAndParentEdges(new List<ITaxonRelationsTreeNode>() { treeNode });
        }

        /// <summary>
        /// Gets all child and parent edges. 
        /// Includes both valid and invalid edges.
        /// Includes both main and secondary parent/children.
        /// </summary>        
        /// <param name="treeNodes">The tree nodes.</param>
        /// <returns>A set with all child and parent edges.</returns>
        public HashSet<ITaxonRelationsTreeEdge> GetAllChildAndParentEdges(ICollection<ITaxonRelationsTreeNode> treeNodes)
        {
            HashSet<ITaxonRelationsTreeEdge> hashEdges = new HashSet<ITaxonRelationsTreeEdge>();            
            foreach (var edge in treeNodes.AsTopDownBreadthFirstParentEdgeIterator(TaxonRelationsTreeParentsIterationMode.Everything))
            {
                hashEdges.Add(edge);
            }
            foreach (var edge in treeNodes.AsBreadthFirstChildEdgeIterator(TaxonRelationsTreeChildrenIterationMode.Everything))
            {
                hashEdges.Add(edge);

                if (edge.Child.ValidSecondaryParents != null)
                {
                    foreach (var parentEdge in edge.Child.ValidSecondaryParents.AsBreadthFirstParentEdgeIterator(TaxonRelationsTreeParentsIterationMode.BothValidMainAndSecondaryParents))
                    {
                        if (!hashEdges.Contains(parentEdge))
                        {
                            hashEdges.Add(parentEdge);
                        }
                    }
                }
            }

            return hashEdges;
        }

        /// <summary>
        /// Gets all valid child and parent edges.         
        /// Includes both main and secondary parent/children.
        /// </summary>
        /// <param name="taxonIds">The taxon ids.</param>
        /// <returns>A set with all child and parent edges.</returns>
        public HashSet<ITaxonRelationsTreeEdge> GetAllValidChildAndParentEdges(IEnumerable<int> taxonIds)
        {
            List<ITaxonRelationsTreeNode> treeNodes = GetTreeNodes(taxonIds);
            return GetAllValidChildAndParentEdges(treeNodes);
        }

        /// <summary>
        /// Gets all valid child and parent edges.         
        /// Includes both main and secondary parent/children.
        /// </summary>        
        /// <param name="taxonId">The taxon id.</param>
        /// <returns>A set with all child and parent edges.</returns>
        public HashSet<ITaxonRelationsTreeEdge> GetAllValidChildAndParentEdges(int taxonId)
        {
            return GetAllValidChildAndParentEdges(GetTreeNode(taxonId));
        }

        /// <summary>
        /// Gets all valid child and parent edges.         
        /// Includes both main and secondary parent/children.
        /// </summary>        
        /// <param name="treeNode">The tree node.</param>
        /// <returns>A set with all child and parent edges.</returns>
        public HashSet<ITaxonRelationsTreeEdge> GetAllValidChildAndParentEdges(ITaxonRelationsTreeNode treeNode)
        {
            return GetAllValidChildAndParentEdges(new List<ITaxonRelationsTreeNode>() { treeNode });
        }

        /// <summary>
        /// Gets all child and parent edges.         
        /// Includes both main and secondary parent/children.
        /// </summary>        
        /// <param name="treeNodes">The tree nodes.</param>
        /// <returns>A set with all child and parent edges.</returns>
        public HashSet<ITaxonRelationsTreeEdge> GetAllValidChildAndParentEdges(ICollection<ITaxonRelationsTreeNode> treeNodes)
        {
            HashSet<ITaxonRelationsTreeEdge> hashEdges = new HashSet<ITaxonRelationsTreeEdge>();
            foreach (var edge in treeNodes.AsTopDownBreadthFirstParentEdgeIterator(TaxonRelationsTreeParentsIterationMode.BothValidMainAndSecondaryParents))
            {
                hashEdges.Add(edge);
            }
            foreach (var edge in treeNodes.AsBreadthFirstChildEdgeIterator(TaxonRelationsTreeChildrenIterationMode.BothValidMainAndSecondaryChildren))
            {
                hashEdges.Add(edge);

                if (edge.Child.ValidSecondaryParents != null)
                {
                    foreach (var parentEdge in edge.Child.ValidSecondaryParents.AsBreadthFirstParentEdgeIterator(TaxonRelationsTreeParentsIterationMode.BothValidMainAndSecondaryParents))
                    {
                        if (!hashEdges.Contains(parentEdge))
                        {
                            hashEdges.Add(parentEdge);
                        }
                    }
                }
            }

            return hashEdges;
        }

        /// <summary>
        /// Gets all child and parent edges. 
        /// Includes both valid and invalid edges.
        /// Includes both main and secondary parent/children.
        /// </summary>        
        /// <param name="treeNodes">The tree nodes.</param>
        /// <param name="onlyValid">Include only valid edges.</param>
        /// <param name="includeChildrenSecondaryParents">Include secondary parents to children.</param>
        /// <returns>A set with all child and parent edges.</returns>
        public HashSet<ITaxonRelationsTreeEdge> GetAllChildAndParentEdges(
            ICollection<ITaxonRelationsTreeNode> treeNodes,
            bool onlyValid,
            bool includeChildrenSecondaryParents = true)
        {                        
            // Get parents
            var parentEdges = GetAllParentEdges(treeNodes, onlyValid);

            // Get children
            var childrenEdges = GetAllChildrenEdges(treeNodes, onlyValid, includeChildrenSecondaryParents);

            var hashEdges = parentEdges;
            hashEdges.UnionWith(childrenEdges);

            return hashEdges;
        }

        /// <summary>
        /// Gets all valid child and parent edges.
        /// Includes both main and secondary parent/children.
        /// </summary>
        /// <param name="taxonIds">The taxon ids.</param>
        /// <param name="treeIterationMode">The tree iteration mode.</param>
        /// <param name="onlyValid">if set to <c>true</c> only valid relations are included.</param>
        /// <param name="includeChildrenSecondaryParents">if set to <c>true</c> secondary parents to children are included.</param>
        /// <returns>
        /// A set with all child and parent edges.
        /// </returns>
        public HashSet<ITaxonRelationsTreeEdge> GetAllEdges(
            IEnumerable<int> taxonIds,
            TaxonRelationsTreeIterationMode treeIterationMode,
            bool onlyValid,
            bool includeChildrenSecondaryParents = true)
        {
            List<ITaxonRelationsTreeNode> treeNodes = GetTreeNodes(taxonIds);
            return GetAllEdges(
                treeNodes,
                treeIterationMode,
                onlyValid,
                includeChildrenSecondaryParents);
        }

        /// <summary>
        /// Gets all valid child and parent edges.
        /// Includes both main and secondary parent/children.
        /// </summary>
        /// <param name="taxonId">The taxon id.</param>
        /// <param name="treeIterationMode">The tree iteration mode.</param>
        /// <param name="onlyValid">if set to <c>true</c> only valid relations are included.</param>
        /// <param name="includeChildrenSecondaryParents">if set to <c>true</c> secondary parents to children are included.</param>
        /// <returns>
        /// A set with all child and parent edges.
        /// </returns>
        public HashSet<ITaxonRelationsTreeEdge> GetAllEdges(
            int taxonId,
            TaxonRelationsTreeIterationMode treeIterationMode,
            bool onlyValid,
            bool includeChildrenSecondaryParents = true)
        {
            return GetAllEdges(
                GetTreeNode(taxonId),
                treeIterationMode,
                onlyValid,
                includeChildrenSecondaryParents);
        }

        /// <summary>
        /// Gets all valid child and parent edges.
        /// Includes both main and secondary parent/children.
        /// </summary>
        /// <param name="treeNode">The tree node.</param>
        /// <param name="treeIterationMode">The tree iteration mode.</param>
        /// <param name="onlyValid">if set to <c>true</c> only valid relations are included.</param>
        /// <param name="includeChildrenSecondaryParents">if set to <c>true</c> secondary parents to children are included.</param>
        /// <returns>
        /// A set with all child and parent edges.
        /// </returns>
        public HashSet<ITaxonRelationsTreeEdge> GetAllEdges(
            ITaxonRelationsTreeNode treeNode,
            TaxonRelationsTreeIterationMode treeIterationMode,
            bool onlyValid,
            bool includeChildrenSecondaryParents = true)
        {
            return GetAllEdges(
                new List<ITaxonRelationsTreeNode>() { treeNode },
                treeIterationMode,
                onlyValid,
                includeChildrenSecondaryParents);
        }

        /// <summary>
        /// Gets all valid child and parent edges.
        /// Includes both main and secondary parent/children.
        /// </summary>
        /// <param name="treeNodes">The tree nodes.</param>
        /// <param name="treeIterationMode">The tree iteration mode.</param>
        /// <param name="onlyValid">if set to <c>true</c> only valid relations are included.</param>
        /// <param name="includeChildrenSecondaryParents">if set to <c>true</c> secondary parents to children are included.</param>
        /// <returns>
        /// A set with all child and parent edges.
        /// </returns>
        public HashSet<ITaxonRelationsTreeEdge> GetAllEdges(
            ICollection<ITaxonRelationsTreeNode> treeNodes,
            TaxonRelationsTreeIterationMode treeIterationMode,
            bool onlyValid,
            bool includeChildrenSecondaryParents = true)
        {
            HashSet<ITaxonRelationsTreeEdge> hashEdges = new HashSet<ITaxonRelationsTreeEdge>();

            if (treeIterationMode == TaxonRelationsTreeIterationMode.BothParentsAndChildren ||
                treeIterationMode == TaxonRelationsTreeIterationMode.OnlyParents)
            {
                hashEdges = GetAllParentEdges(treeNodes, onlyValid);
            }

            if (treeIterationMode == TaxonRelationsTreeIterationMode.BothParentsAndChildren ||
                treeIterationMode == TaxonRelationsTreeIterationMode.OnlyChildren)
            {
                var childrenEdges = GetAllChildrenEdges(treeNodes, onlyValid, includeChildrenSecondaryParents);
                hashEdges.UnionWith(childrenEdges);
            }

            return hashEdges;
        }

        /// <summary>
        /// Gets all children edges hierarchical.
        /// </summary>
        /// <param name="treeNodes">The tree nodes.</param>
        /// <param name="onlyValid">if set to <c>true</c> only valid edges are included.</param>
        /// <param name="includeChildrenSecondaryParents">if set to <c>true</c> childrens secondary parents are included.</param>
        /// <returns>Children edges hierarchical.</returns>
        private static HashSet<ITaxonRelationsTreeEdge> GetAllChildrenEdges(
            ICollection<ITaxonRelationsTreeNode> treeNodes, 
            bool onlyValid, 
            bool includeChildrenSecondaryParents)
        {
            HashSet<ITaxonRelationsTreeEdge> hashEdges = new HashSet<ITaxonRelationsTreeEdge>();
            TaxonRelationsTreeChildrenIterationMode childrenIterationMode;
            if (onlyValid)
            {
                childrenIterationMode = TaxonRelationsTreeChildrenIterationMode.BothValidMainAndSecondaryChildren;
            }
            else
            {
                childrenIterationMode = TaxonRelationsTreeChildrenIterationMode.Everything;
            }
            foreach (var edge in treeNodes.AsBreadthFirstChildEdgeIterator(childrenIterationMode))
            {
                hashEdges.Add(edge);

                if (includeChildrenSecondaryParents && edge.Child.ValidSecondaryParents != null)
                {
                    foreach (
                        var parentEdge in
                            edge.Child.ValidSecondaryParents.AsBreadthFirstParentEdgeIterator(
                                TaxonRelationsTreeParentsIterationMode.BothValidMainAndSecondaryParents))
                    {
                        if (!hashEdges.Contains(parentEdge))
                        {
                            hashEdges.Add(parentEdge);
                        }
                    }
                }
            }
            return hashEdges;
        }

        /// <summary>
        /// Gets all parent edges hierarchical.
        /// </summary>
        /// <param name="treeNodes">The tree nodes.</param>
        /// <param name="onlyValid">if set to <c>true</c> only valid edges is included.</param>
        /// <returns>Parent edges hierarchical</returns>
        private static HashSet<ITaxonRelationsTreeEdge> GetAllParentEdges(
            ICollection<ITaxonRelationsTreeNode> treeNodes, 
            bool onlyValid)
        {
            HashSet<ITaxonRelationsTreeEdge> hashEdges = new HashSet<ITaxonRelationsTreeEdge>();
            TaxonRelationsTreeParentsIterationMode parentsIterationMode;
            if (onlyValid)
            {
                parentsIterationMode = TaxonRelationsTreeParentsIterationMode.BothValidMainAndSecondaryParents;
            }
            else
            {
                parentsIterationMode = TaxonRelationsTreeParentsIterationMode.Everything;
            }
            foreach (var edge in treeNodes.AsTopDownBreadthFirstParentEdgeIterator(parentsIterationMode))
            {
                hashEdges.Add(edge);
            }
            return hashEdges;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Taxon relations tree relation type mode.
    /// </summary>
    public enum TaxonRelationsTreeRelationTypeMode
    {
        /// <summary>
        /// Only valid relations are included.
        /// </summary>
        OnlyValidRelations,

        /// <summary>
        /// Both valid and invalid relations are included.
        /// </summary>
        BothValidAndInvalidRelations
    }

    /// <summary>
    /// Taxon relations tree iteration mode.
    /// </summary>
    public enum TaxonRelationsTreeIterationMode
    {
        /// <summary>
        /// Both parents and children are included.
        /// </summary>
        BothParentsAndChildren,

        /// <summary>
        /// Only parents are included.
        /// </summary>
        OnlyParents,

        /// <summary>
        /// Only children are included.
        /// </summary>
        OnlyChildren
    }

    /// <summary>
    /// Taxon relations tree iteration upward mode.
    /// </summary>
    public enum TaxonRelationsTreeParentsIterationMode
    {
        /// <summary>
        /// Traverses only valid main parents.
        /// </summary>
        OnlyValidMainParents,
        
        /// <summary>
        /// Traverses both valid main and secondary parents.
        /// </summary>
        BothValidMainAndSecondaryParents,
        
        /// <summary>
        /// Traverses both valid and nonvalid main and secondary parents.
        /// </summary>
        Everything
    }

    /// <summary>
    /// Taxon relations tree iteration downward mode.
    /// </summary>
    public enum TaxonRelationsTreeChildrenIterationMode
    {
        /// <summary>
        /// Traverses only valid main children.
        /// </summary>
        OnlyValidMainChildren,

        /// <summary>
        /// Traverses both valid main and secondary children.
        /// </summary>
        BothValidMainAndSecondaryChildren,

        /// <summary>
        /// Traverses both valid and nonvalid main and secondary children.
        /// </summary>
        Everything
    }

    /// <summary>
    /// Tree iteration order enum.
    /// </summary>
    public enum TaxonRelationsTreeIterationOrder
    {
        /// <summary>
        /// Iterates from root to node.
        /// </summary>
        RootToNode,

        /// <summary>
        /// Iterates from node to root.
        /// </summary>
        NodeToRoot
    }

    /// <summary>
    /// Extension methods for Taxon relations tree.
    /// </summary>
    public static class TaxonRelationsTreeExtension
    {
        /// <summary>
        /// Converts to taxon relation list.
        /// </summary>
        /// <param name="edges">The edges.</param>
        /// <returns>A TaxonRelationList.</returns>
        public static TaxonRelationList ToTaxonRelationList(this ICollection<ITaxonRelationsTreeEdge> edges)
        {
            TaxonRelationList taxonRelationList = new TaxonRelationList();            
            taxonRelationList.AddRange(edges.Select(x => x.TaxonRelation));
            return taxonRelationList;
        }

        /// <summary>
        /// Creates an iterator that traverses the tree depth first.
        /// </summary>
        public static IEnumerable<ITaxonRelationsTreeNode> AsDepthFirstNodeIterator(this TaxonRelationsTree tree)
        {
            return AsDepthFirstNodeIterator(tree.Root);
        }

        /// <summary>
        /// Creates an iterator that traverses a tree nodes children depth first.
        /// </summary>
        public static IEnumerable<ITaxonRelationsTreeNode> AsDepthFirstNodeIterator(this ITaxonRelationsTreeNode treeNode)
        {
            return AsDepthFirstNodeIterator(new List<ITaxonRelationsTreeNode> { treeNode });
        }

        /// <summary>
        /// Creates an iterator that traverses a list of tree nodes depth first.
        /// </summary>
        public static IEnumerable<ITaxonRelationsTreeNode> AsDepthFirstNodeIterator(this ICollection<ITaxonRelationsTreeNode> treeNodes)
        {
            HashSet<ITaxonRelationsTreeNode> visitedNodes = new HashSet<ITaxonRelationsTreeNode>();
            Stack<ITaxonRelationsTreeNode> stack = new Stack<ITaxonRelationsTreeNode>();
            foreach (var treeNode in treeNodes)
            {
                stack.Push(treeNode);
            }

            while (stack.Any())
            {
                ITaxonRelationsTreeNode current = stack.Pop();
                if (current != null)
                {
                    if (current.ValidMainChildren != null)
                    {
                        for (int i = current.ValidMainChildren.Count - 1; i >= 0; i--)
                        {
                            stack.Push(current.ValidMainChildren[i].Child);
                        }
                    }

                    if (current.ValidSecondaryChildren != null)
                    {
                        for (int i = current.ValidSecondaryChildren.Count - 1; i >= 0; i--)
                        {
                            stack.Push(current.ValidSecondaryChildren[i].Child);
                        }
                    }

                    // Avoid cycles. Just return not visited nodes.
                    if (!visitedNodes.Contains(current))
                    {
                        visitedNodes.Add(current);
                        yield return current;
                    }
                }
            }
        }
       
        /// <summary>
        /// Creates an iterator that traverses a tree nodes parents top first breadth first.
        /// </summary>
        public static IEnumerable<ITaxonRelationsTreeNode> AsTopFirstBreadthFirstParentNodeIterator(
            this ITaxonRelationsTreeNode treeNode, 
            TaxonRelationsTreeParentsIterationMode treeIterationMode, 
            bool returnStartNode = true)
        {
            return AsTopFirstBreadthFirstParentNodeIterator(new List<ITaxonRelationsTreeNode> { treeNode }, treeIterationMode, returnStartNode);
        }

        /// <summary>
        /// Creates an iterator that traverses a tree nodes parents top first breadth first.
        /// </summary>
        public static IEnumerable<ITaxonRelationsTreeNode> AsTopFirstBreadthFirstParentNodeIterator(
            this ICollection<ITaxonRelationsTreeNode> treeNodes, 
            TaxonRelationsTreeParentsIterationMode treeIterationMode,
            bool returnStartNodes = true)
        {
            List<ITaxonRelationsTreeNode> nodes = new List<ITaxonRelationsTreeNode>();
            foreach (var node in treeNodes.AsBottomFirstBreadthFirstParentNodeIterator(treeIterationMode))
            {
                nodes.Add(node);
            }
            nodes.Reverse();

            foreach (var node in nodes)
            {
                yield return node;
            }            
        }

        /// <summary>
        /// Creates an iterator that traverses a tree nodes parents bottom first breadth first.
        /// </summary>
        public static IEnumerable<ITaxonRelationsTreeNode> AsBottomFirstBreadthFirstParentNodeIterator(
            this ITaxonRelationsTreeNode treeNode, 
            TaxonRelationsTreeParentsIterationMode treeIterationMode, 
            bool returnStartNode = true)
        {
            return AsBottomFirstBreadthFirstParentNodeIterator(new List<ITaxonRelationsTreeNode> { treeNode }, treeIterationMode, returnStartNode);
        }

        /// <summary>
        /// Creates an iterator that traverses a tree nodes parents bottom first breadth first.
        /// </summary>
        public static IEnumerable<ITaxonRelationsTreeNode> AsBottomFirstBreadthFirstParentNodeIterator(
            this ICollection<ITaxonRelationsTreeNode> treeNodes, 
            TaxonRelationsTreeParentsIterationMode treeIterationMode, 
            bool returnStartNodes = true)
        {
            HashSet<ITaxonRelationsTreeNode> visitedNodes = new HashSet<ITaxonRelationsTreeNode>();
            Queue<ITaxonRelationsTreeNode> queue = new Queue<ITaxonRelationsTreeNode>();
            foreach (var treeNode in treeNodes)
            {
                queue.Enqueue(treeNode);
            }

            while (queue.Any())
            {
                ITaxonRelationsTreeNode current = queue.Dequeue();
                if (current != null)
                {
                    if (current.ValidMainParents != null)
                    {
                        for (int i = current.ValidMainParents.Count - 1; i >= 0; i--)
                        {
                            queue.Enqueue(current.ValidMainParents[i].Parent);
                        }
                    }

                    if (treeIterationMode == TaxonRelationsTreeParentsIterationMode.BothValidMainAndSecondaryParents)
                    {
                        if (current.ValidSecondaryParents != null)
                        {
                            for (int i = current.ValidSecondaryParents.Count - 1; i >= 0; i--)
                            {
                                queue.Enqueue(current.ValidSecondaryParents[i].Parent);
                            }
                        }
                    }

                    // Avoid cycles. Just return not visited nodes.
                    if (!visitedNodes.Contains(current))
                    {
                        visitedNodes.Add(current);
                        if (returnStartNodes || !treeNodes.Any(x => Equals(x, current)))
                        {
                            yield return current;
                        }
                    }
                }
            }
        }        

        /// <summary>
        /// Creates an iterator that traverses a tree nodes parents top first.
        /// </summary>
        public static IEnumerable<ITaxonRelationsTreeNode> AsTopFirstParentNodeIterator(this ITaxonRelationsTreeNode treeNode, TaxonRelationsTreeParentsIterationMode treeIterationMode, bool returnStartNode = true)
        {
            return AsTopFirstParentNodeIterator(new List<ITaxonRelationsTreeNode> { treeNode }, treeIterationMode, returnStartNode);
        }

        /// <summary>
        /// Creates an iterator that traverses a list of tree nodes parents top first.
        /// </summary>
        public static IEnumerable<ITaxonRelationsTreeNode> AsTopFirstParentNodeIterator(this ICollection<ITaxonRelationsTreeNode> treeNodes, TaxonRelationsTreeParentsIterationMode treeIterationMode, bool returnStartNodes = true)
        {
            HashSet<ITaxonRelationsTreeNode> visitedNodes = new HashSet<ITaxonRelationsTreeNode>();
            Stack<ITaxonRelationsTreeNode> stack = new Stack<ITaxonRelationsTreeNode>();
            foreach (var treeNode in treeNodes)
            {
                stack.Push(treeNode);
            }

            while (stack.Any())
            {
                ITaxonRelationsTreeNode current = stack.Pop();
                if (current != null)
                {
                    if (current.ValidMainParents != null)
                    {
                        for (int i = current.ValidMainParents.Count - 1; i >= 0; i--)
                        {
                            stack.Push(current.ValidMainParents[i].Parent);
                        }
                    }

                    if (treeIterationMode == TaxonRelationsTreeParentsIterationMode.BothValidMainAndSecondaryParents)
                    {
                        if (current.ValidSecondaryParents != null)
                        {
                            for (int i = current.ValidSecondaryParents.Count - 1; i >= 0; i--)
                            {
                                stack.Push(current.ValidSecondaryParents[i].Parent);
                            }
                        }
                    }

                    // Avoid cycles. Just return not visited nodes.
                    if (!visitedNodes.Contains(current))
                    {
                        visitedNodes.Add(current);
                        if (returnStartNodes || !treeNodes.Any(x => Equals(x, current)))
                        {
                            yield return current;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns a depth first child edge iterator.
        /// </summary>
        /// <param name="treeNode">The tree node.</param>
        /// <param name="treeIterationMode">The tree iteration mode.</param>
        /// <returns>A depth first child edge iterator.</returns>
        public static IEnumerable<ITaxonRelationsTreeEdge> AsDepthFirstChildEdgeIterator(
            this ITaxonRelationsTreeNode treeNode,
            TaxonRelationsTreeChildrenIterationMode treeIterationMode)
        {
            return AsDepthFirstChildEdgeIterator(new List<ITaxonRelationsTreeNode> { treeNode }, treeIterationMode);
        }

        /// <summary>
        /// Returns a depth first child edge iterator.
        /// </summary>
        /// <param name="treeNodes">The tree nodes.</param>
        /// <param name="treeIterationMode">The tree iteration mode.</param>
        /// <returns>A depth first child edge iterator.</returns>
        public static IEnumerable<ITaxonRelationsTreeEdge> AsDepthFirstChildEdgeIterator(
            this ICollection<ITaxonRelationsTreeNode> treeNodes,
            TaxonRelationsTreeChildrenIterationMode treeIterationMode)
        {
            HashSet<ITaxonRelationsTreeNode> visitedNodes = new HashSet<ITaxonRelationsTreeNode>();
            HashSet<ITaxonRelationsTreeEdge> visitedEdges = new HashSet<ITaxonRelationsTreeEdge>();
            Stack<ITaxonRelationsTreeEdge> stack = new Stack<ITaxonRelationsTreeEdge>();

            foreach (var treeNode in treeNodes)
            {
                visitedNodes.Add(treeNode);

                if (treeNode.ValidMainChildren != null)
                {
                    for (int i = treeNode.ValidMainChildren.Count - 1; i >= 0; i--)
                    {
                        stack.Push(treeNode.ValidMainChildren[i]);
                    }
                }

                if (treeIterationMode == TaxonRelationsTreeChildrenIterationMode.BothValidMainAndSecondaryChildren)
                {
                    if (treeNode.ValidSecondaryChildren != null)
                    {
                        for (int i = treeNode.ValidSecondaryChildren.Count - 1; i >= 0; i--)
                        {
                            stack.Push(treeNode.ValidSecondaryChildren[i]);
                        }
                    }
                }
            }

            while (stack.Any())
            {
                ITaxonRelationsTreeEdge current = stack.Pop();
                if (current != null)
                {
                    if (current.Child.ValidMainChildren != null)
                    {
                        for (int i = current.Child.ValidMainChildren.Count - 1; i >= 0; i--)
                        {
                            stack.Push(current.Child.ValidMainChildren[i]);
                        }
                    }

                    if (current.Child.ValidSecondaryChildren != null)
                    {
                        for (int i = current.Child.ValidSecondaryChildren.Count - 1; i >= 0; i--)
                        {
                            stack.Push(current.Child.ValidSecondaryChildren[i]);
                        }
                    }

                    // Avoid cycles. Just return not visited nodes.
                    if (!visitedNodes.Contains(current.Child))
                    {
                        visitedNodes.Add(current.Child);
                    }                    

                    // Avoid cycles. Just return not visited edges.
                    if (!visitedEdges.Contains(current))
                    {
                        visitedEdges.Add(current);
                        yield return current;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a breadth first child edge iterator.
        /// </summary>
        /// <param name="treeNode">The tree node.</param>
        /// <param name="treeIterationMode">The tree iteration mode.</param>
        /// <returns>A breadth first child edge iterator.</returns>
        public static IEnumerable<ITaxonRelationsTreeEdge> AsBreadthFirstChildEdgeIterator(
                    this ITaxonRelationsTreeNode treeNode,
                    TaxonRelationsTreeChildrenIterationMode treeIterationMode)
        {
            return AsBreadthFirstChildEdgeIterator(new List<ITaxonRelationsTreeNode> { treeNode }, treeIterationMode);
        }

        /// <summary>
        /// Returns a breadth first child edge iterator.
        /// </summary>
        /// <param name="treeNodes">The tree nodes.</param>
        /// <param name="treeIterationMode">The tree iteration mode.</param>
        /// <returns>A breadth first child edge iterator.</returns>
        public static IEnumerable<ITaxonRelationsTreeEdge> AsBreadthFirstChildEdgeIterator(
            this ICollection<ITaxonRelationsTreeNode> treeNodes,
            TaxonRelationsTreeChildrenIterationMode treeIterationMode)
        {
            HashSet<ITaxonRelationsTreeNode> visitedNodes = new HashSet<ITaxonRelationsTreeNode>();
            HashSet<ITaxonRelationsTreeEdge> visitedEdges = new HashSet<ITaxonRelationsTreeEdge>();
            Queue<ITaxonRelationsTreeEdge> queue = new Queue<ITaxonRelationsTreeEdge>();

            foreach (var treeNode in treeNodes)
            {
                visitedNodes.Add(treeNode);

                if (treeIterationMode == TaxonRelationsTreeChildrenIterationMode.Everything)
                {
                    if (treeNode.AllChildEdges != null)
                    {
                        for (int i = 0; i < treeNode.AllChildEdges.Count; i++)
                        {
                            queue.Enqueue(treeNode.AllChildEdges[i]);
                        }
                    }
                }
                else
                {
                    if (treeNode.ValidMainChildren != null)
                    {
                        for (int i = 0; i < treeNode.ValidMainChildren.Count; i++)
                        {
                            queue.Enqueue(treeNode.ValidMainChildren[i]);
                        }
                    }

                    if (treeIterationMode == TaxonRelationsTreeChildrenIterationMode.BothValidMainAndSecondaryChildren)
                    {
                        if (treeNode.ValidSecondaryChildren != null)
                        {
                            for (int i = 0; i < treeNode.ValidSecondaryChildren.Count; i++)
                            {
                                queue.Enqueue(treeNode.ValidSecondaryChildren[i]);
                            }
                        }
                    }
                }
            }

            while (queue.Any())
            {
                ITaxonRelationsTreeEdge current = queue.Dequeue();
                if (current != null)
                {
                    if (treeIterationMode == TaxonRelationsTreeChildrenIterationMode.Everything)
                    {
                        if (current.Child.AllChildEdges != null)
                        {
                            for (int i = 0; i < current.Child.AllChildEdges.Count; i++)
                            {
                                queue.Enqueue(current.Child.AllChildEdges[i]);
                            }
                        }
                    }
                    else
                    {
                        if (current.Child.ValidMainChildren != null)
                        {
                            for (int i = 0; i < current.Child.ValidMainChildren.Count; i++)
                            {
                                queue.Enqueue(current.Child.ValidMainChildren[i]);
                            }
                        }

                        if (current.Child.ValidSecondaryChildren != null)
                        {
                            for (int i = 0; i < current.Child.ValidSecondaryChildren.Count; i++)
                            {
                                queue.Enqueue(current.Child.ValidSecondaryChildren[i]);
                            }
                        }
                    }

                    // Avoid cycles. Just return not visited nodes.
                    if (!visitedNodes.Contains(current.Child))
                    {
                        visitedNodes.Add(current.Child);
                    }                    

                    // Avoid cycles. Just return not visited edges.
                    if (!visitedEdges.Contains(current))
                    {
                        visitedEdges.Add(current);
                        yield return current;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a breadth first parent edge iterator.
        /// </summary>
        /// <param name="treeEdge">The tree edge.</param>
        /// <param name="treeIterationMode">The tree iteration mode.</param>
        /// <returns>A breadth first parent edge iterator.</returns>
        public static IEnumerable<ITaxonRelationsTreeEdge> AsBreadthFirstParentEdgeIterator(
            this ITaxonRelationsTreeEdge treeEdge,
            TaxonRelationsTreeParentsIterationMode treeIterationMode)
        {
            return AsBreadthFirstParentEdgeIterator(new List<ITaxonRelationsTreeEdge> { treeEdge }, treeIterationMode);
        }

        /// <summary>
        /// Returns a breadth first parent edge iterator.
        /// </summary>
        /// <param name="treeNodes">The tree nodes.</param>
        /// <param name="treeIterationMode">The tree iteration mode.</param>
        /// <returns>A breadth first parent edge iterator.</returns>
        public static IEnumerable<ITaxonRelationsTreeEdge> AsBreadthFirstParentEdgeIterator(
            this ICollection<ITaxonRelationsTreeNode> treeNodes,
            TaxonRelationsTreeParentsIterationMode treeIterationMode)
        {
            List<ITaxonRelationsTreeEdge> edges = new List<ITaxonRelationsTreeEdge>();

            if (treeIterationMode == TaxonRelationsTreeParentsIterationMode.Everything)
            {
                foreach (var treeNode in treeNodes)
                {
                    if (treeNode.AllParentEdges != null)
                    {
                        for (int i = treeNode.AllParentEdges.Count - 1; i >= 0; i--)
                        {
                            edges.Add(treeNode.AllParentEdges[i]);
                        }
                    }
                }

                return AsBreadthFirstParentEdgeIterator(edges, treeIterationMode);
            }

            foreach (var treeNode in treeNodes)
            {
                if (treeNode.ValidMainParents != null)
                {
                    for (int i = treeNode.ValidMainParents.Count - 1; i >= 0; i--)
                    {
                        edges.Add(treeNode.ValidMainParents[i]);
                    }
                }

                if (treeIterationMode == TaxonRelationsTreeParentsIterationMode.BothValidMainAndSecondaryParents)
                {
                    if (treeNode.ValidSecondaryParents != null)
                    {
                        for (int i = treeNode.ValidSecondaryParents.Count - 1; i >= 0; i--)
                        {
                            edges.Add(treeNode.ValidSecondaryParents[i]);
                        }
                    }
                }
            }

            return AsBreadthFirstParentEdgeIterator(edges, treeIterationMode);
        }

        /// <summary>
        /// Returns a breadth first parent edge iterator.
        /// </summary>
        /// <param name="treeNode">The tree node.</param>
        /// <param name="treeIterationMode">The tree iteration mode.</param>
        /// <returns>A breadth first parent edge iterator.</returns>
        public static IEnumerable<ITaxonRelationsTreeEdge> AsBreadthFirstParentEdgeIterator(
            this ITaxonRelationsTreeNode treeNode,
            TaxonRelationsTreeParentsIterationMode treeIterationMode)
        {
            return AsBreadthFirstParentEdgeIterator(new List<ITaxonRelationsTreeNode> { treeNode }, treeIterationMode);
        }

        /// <summary>
        /// Creates an iterator that traverses a list of tree nodes parents top first.
        /// </summary>
        public static IEnumerable<ITaxonRelationsTreeEdge> AsBreadthFirstParentEdgeIterator(
            this ICollection<ITaxonRelationsTreeEdge> treeEdges,
            TaxonRelationsTreeParentsIterationMode treeIterationMode)
        {
            HashSet<ITaxonRelationsTreeNode> visitedNodes = new HashSet<ITaxonRelationsTreeNode>();
            HashSet<ITaxonRelationsTreeEdge> visitedEdges = new HashSet<ITaxonRelationsTreeEdge>();
            Queue<ITaxonRelationsTreeEdge> queue = new Queue<ITaxonRelationsTreeEdge>();

            foreach (ITaxonRelationsTreeEdge edge in treeEdges)
            {
                visitedNodes.Add(edge.Child);
                queue.Enqueue(edge);
            }
            
            while (queue.Any())
            {
                ITaxonRelationsTreeEdge current = queue.Dequeue();
                if (current != null)
                {
                    if (treeIterationMode == TaxonRelationsTreeParentsIterationMode.Everything)
                    {
                        if (current.Parent.AllParentEdges != null)
                        {
                            for (int i = current.Parent.AllParentEdges.Count - 1; i >= 0; i--)
                            {
                                queue.Enqueue(current.Parent.AllParentEdges[i]);
                            }
                        }
                    }
                    else
                    {
                        if (current.Parent.ValidMainParents != null)
                        {
                            for (int i = current.Parent.ValidMainParents.Count - 1; i >= 0; i--)
                            {
                                queue.Enqueue(current.Parent.ValidMainParents[i]);
                            }
                        }

                        if (treeIterationMode == TaxonRelationsTreeParentsIterationMode.BothValidMainAndSecondaryParents)
                        {
                            if (current.Parent.ValidSecondaryParents != null)
                            {
                                for (int i = current.Parent.ValidSecondaryParents.Count - 1; i >= 0; i--)
                                {
                                    queue.Enqueue(current.Parent.ValidSecondaryParents[i]);
                                }
                            }
                        }
                    }

                    // Avoid cycles. Just return not visited nodes.
                    if (!visitedNodes.Contains(current.Parent))
                    {
                        visitedNodes.Add(current.Parent);
                    }                    

                    // Avoid cycles. Just return not visited edges.
                    if (!visitedEdges.Contains(current))
                    {
                        visitedEdges.Add(current);
                        yield return current;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a reversed breadth first parent edge iterator.
        /// </summary>
        /// <param name="treeNode">The tree node.</param>
        /// <param name="treeIterationMode">The tree iteration mode.</param>
        /// <returns>A reversed breadth first parent edge iterator.</returns>
        public static IEnumerable<ITaxonRelationsTreeEdge> AsTopDownBreadthFirstParentEdgeIterator(
            this ITaxonRelationsTreeNode treeNode, 
            TaxonRelationsTreeParentsIterationMode treeIterationMode)
        {            
            return AsTopDownBreadthFirstParentEdgeIterator(new List<ITaxonRelationsTreeNode> { treeNode }, treeIterationMode);
        }
                
        /// <summary>
        /// Creates an iterator that traverses a list of tree nodes parents top first.
        /// </summary>
        public static IEnumerable<ITaxonRelationsTreeEdge> AsTopDownBreadthFirstParentEdgeIterator(
            this ICollection<ITaxonRelationsTreeNode> treeNodes, 
            TaxonRelationsTreeParentsIterationMode treeIterationMode)
        {            
            List<ITaxonRelationsTreeEdge> edges = new List<ITaxonRelationsTreeEdge>();
            foreach (var edge in treeNodes.AsBreadthFirstParentEdgeIterator(treeIterationMode))
            {
                edges.Add(edge);
            }
            edges.Reverse();

            foreach (var edge in edges)
            {
                yield return edge;
            }
        }
    }
}
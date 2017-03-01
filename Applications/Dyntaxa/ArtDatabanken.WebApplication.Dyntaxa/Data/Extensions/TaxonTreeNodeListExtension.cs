using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions
{
    /// <summary>
    /// Extension methods for TaxonTreeNodeList.
    /// </summary>
    public static class TaxonTreeNodeListExtension
    {       
        /// <summary>
        /// Creates an iterator that traverses the tree breadth first.
        /// </summary>
        /// <param name="taxonTreeNodeList">The Taxon tree node list.</param>        
        public static IEnumerable<ITaxonTreeNode> AsBreadthFirstIterator(this TaxonTreeNodeList taxonTreeNodeList)
        {
            Queue<ITaxonTreeNode> q = new Queue<ITaxonTreeNode>();
            foreach (ITaxonTreeNode taxonTreeNode in taxonTreeNodeList)
            {
                q.Enqueue(taxonTreeNode);
            }            

            while (q.Any())
            {
                ITaxonTreeNode current = q.Dequeue();
                if (current != null)
                {
                    if (current.Children != null)
                    {
                        foreach (ITaxonTreeNode childNode in current.Children)
                        {
                            q.Enqueue(childNode);
                        }
                    }

                    yield return current;
                }
            }
        }

        /// <summary>
        /// Creates an iterator that traverses the tree depth first.
        /// </summary>
        /// <param name="taxonTreeNodeList">The Taxon tree node list.</param>        
        public static IEnumerable<ITaxonTreeNode> AsDepthFirstIterator(this TaxonTreeNodeList taxonTreeNodeList)
        {
            Stack<ITaxonTreeNode> q = new Stack<ITaxonTreeNode>();
            foreach (ITaxonTreeNode taxonTreeNode in taxonTreeNodeList)
            {
                q.Push(taxonTreeNode);
            }

            while (q.Any())
            {
                ITaxonTreeNode current = q.Pop();
                if (current != null)
                {                    
                    if (current.Children != null)
                    {                        
                        for (int i = current.Children.Count - 1; i >= 0; i--)
                        {
                            q.Push(current.Children[i]);
                        }                        
                    }

                    yield return current;
                }
            }
        }
    }    
}

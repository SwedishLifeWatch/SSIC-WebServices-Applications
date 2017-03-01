using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// TaxonRelationsTree manager.
    /// </summary>
    public static class TaxonRelationsTreeManager
    {
        /// <summary>
        /// Creates a TaxonRelations tree with all parents to a taxon.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">The taxon to get all parents for.</param>
        /// <returns>A Taxon relations tree.</returns>
        public static TaxonRelationsTree CreateTaxonRelationsParentsTree(
            IUserContext userContext,
            ITaxon taxon)
        {
            TaxonRelationSearchCriteria searchCriteria = new TaxonRelationSearchCriteria();
            searchCriteria.IsMainRelation = null;
            searchCriteria.IsValid = null;
            searchCriteria.Scope = TaxonRelationSearchScope.AllParentRelations;
            searchCriteria.Taxa = new TaxonList { taxon };
            TaxonRelationList relations = CoreData.TaxonManager.GetTaxonRelations(userContext, searchCriteria);
            TaxonRelationsTree taxonRelationsTree = CreateTaxonRelationsTree(userContext, relations, new TaxonList { taxon });
            return taxonRelationsTree;
        }

        /// <summary>
        /// Creates a TaxonRelations tree from a taxon relations list.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRelations">Taxon relations.</param>
        /// <param name="taxa">Taxa list. Use this if you want to be able to find isolated nodes (taxon).</param>
        /// <param name="relationTaxonMustBeValid">If true only relations to valid taxon is added for Validproperties.</param>
        /// <returns>A Taxon relations tree.</returns>
        public static TaxonRelationsTree CreateTaxonRelationsTree(
            IUserContext userContext, 
            TaxonRelationList taxonRelations, 
            TaxonList taxa, 
            bool relationTaxonMustBeValid = false)
        {
            DateTime today = DateTime.Now;
            Dictionary<int, ITaxonRelationsTreeNode> treeNodesDictionary = new Dictionary<int, ITaxonRelationsTreeNode>();
            TaxonRelationsTree tree = new TaxonRelationsTree();
            tree.AllTreeEdgesDictionary = new Dictionary<int, List<ITaxonRelationsTreeEdge>>();
            tree.AllTreeEdges = new List<ITaxonRelationsTreeEdge>();
            tree.AllTreeNodes = new List<ITaxonRelationsTreeNode>();            
            tree.NotUsedTreeEdges = new List<ITaxonRelationsTreeEdge>();
            tree.OriginalTaxonRelationList = taxonRelations;
            tree.OriginalTaxaList = taxa;

            tree.TreeNodes = new List<ITaxonRelationsTreeNode>();
            tree.TreeEdges = new List<ITaxonRelationsTreeEdge>();
            tree.TreeNodesWithoutEdges = new List<ITaxonRelationsTreeNode>();

            foreach (ITaxonRelation taxonRelation in taxonRelations)
            {
                ITaxonRelationsTreeNode parentNode;
                ITaxonRelationsTreeNode childNode;
                if (!treeNodesDictionary.ContainsKey(taxonRelation.ParentTaxon.Id))
                {
                    parentNode = new TaxonRelationsTreeNode(taxonRelation.ParentTaxon);
                    tree.TreeNodes.Add(parentNode);
                    tree.AllTreeNodes.Add(parentNode);                    
                    treeNodesDictionary.Add(taxonRelation.ParentTaxon.Id, parentNode);
                }
                parentNode = treeNodesDictionary[taxonRelation.ParentTaxon.Id];

                if (!treeNodesDictionary.ContainsKey(taxonRelation.ChildTaxon.Id))
                {
                    childNode = new TaxonRelationsTreeNode(taxonRelation.ChildTaxon);
                    tree.TreeNodes.Add(childNode);
                    tree.AllTreeNodes.Add(childNode);                    
                    treeNodesDictionary.Add(taxonRelation.ChildTaxon.Id, childNode);
                }
                childNode = treeNodesDictionary[taxonRelation.ChildTaxon.Id];

                ITaxonRelationsTreeEdge treeEdge = new TaxonRelationsTreeEdge(parentNode, childNode, taxonRelation);
                tree.AllTreeEdges.Add(treeEdge);

                // Start DyntaxaTree.AllTreeEdgesDictionary
                if (!tree.AllTreeEdgesDictionary.ContainsKey(parentNode.Taxon.Id))
                {
                    tree.AllTreeEdgesDictionary.Add(parentNode.Taxon.Id, new List<ITaxonRelationsTreeEdge>());
                }
                tree.AllTreeEdgesDictionary[parentNode.Taxon.Id].Add(treeEdge);
                if (!tree.AllTreeEdgesDictionary.ContainsKey(childNode.Taxon.Id))
                {
                    tree.AllTreeEdgesDictionary.Add(childNode.Taxon.Id, new List<ITaxonRelationsTreeEdge>());
                }
                tree.AllTreeEdgesDictionary[childNode.Taxon.Id].Add(treeEdge);
                // End DyntaxaTree.AllTreeEdgesDictionary

                bool isMain = taxonRelation.IsMainRelation;
                bool isValid = taxonRelation.ValidFromDate <= today && today <= taxonRelation.ValidToDate;

                if (parentNode.AllEdges == null)
                {
                    parentNode.AllEdges = new List<ITaxonRelationsTreeEdge>();
                }
                parentNode.AllEdges.Add(treeEdge);

                if (parentNode.AllChildEdges == null)
                {
                    parentNode.AllChildEdges = new List<ITaxonRelationsTreeEdge>();
                }
                parentNode.AllChildEdges.Add(treeEdge);

                if (childNode.AllEdges == null)
                {
                    childNode.AllEdges = new List<ITaxonRelationsTreeEdge>();
                }
                childNode.AllEdges.Add(treeEdge);

                if (childNode.AllParentEdges == null)
                {
                    childNode.AllParentEdges = new List<ITaxonRelationsTreeEdge>();
                }
                childNode.AllParentEdges.Add(treeEdge);

                if (isMain && isValid && ((relationTaxonMustBeValid && parentNode.Taxon.IsValid && childNode.Taxon.IsValid) || !relationTaxonMustBeValid))
                // vet inte om taxonen måste vara valid...
                {
                    if (parentNode.ValidMainChildren == null)
                    {
                        parentNode.ValidMainChildren = new List<ITaxonRelationsTreeEdge>();
                    }
                    parentNode.ValidMainChildren.Add(treeEdge);

                    if (childNode.ValidMainParents == null)
                    {
                        childNode.ValidMainParents = new List<ITaxonRelationsTreeEdge>();
                    }
                    childNode.ValidMainParents.Add(treeEdge);

                    tree.TreeEdges.Add(treeEdge);
                }
                else if (isMain && !isValid)
                {
                    if (parentNode.NonvalidMainChildren == null)
                    {
                        parentNode.NonvalidMainChildren = new List<ITaxonRelationsTreeEdge>();
                    }
                    parentNode.NonvalidMainChildren.Add(treeEdge);

                    if (childNode.NonvalidMainParents == null)
                    {
                        childNode.NonvalidMainParents = new List<ITaxonRelationsTreeEdge>();
                    }
                    childNode.NonvalidMainParents.Add(treeEdge);

                    tree.TreeEdges.Add(treeEdge);
                }
                else if (!isMain && isValid && ((relationTaxonMustBeValid && parentNode.Taxon.IsValid && childNode.Taxon.IsValid) || !relationTaxonMustBeValid))
                // vet inte om taxonen måste vara valid...
                {
                    if (parentNode.ValidSecondaryChildren == null)
                    {
                        parentNode.ValidSecondaryChildren = new List<ITaxonRelationsTreeEdge>();
                    }
                    parentNode.ValidSecondaryChildren.Add(treeEdge);

                    if (childNode.ValidSecondaryParents == null)
                    {
                        childNode.ValidSecondaryParents = new List<ITaxonRelationsTreeEdge>();
                    }
                    childNode.ValidSecondaryParents.Add(treeEdge);

                    tree.TreeEdges.Add(treeEdge);
                }
                else if (!isMain && !isValid)
                {
                    if (parentNode.NonvalidSecondaryChildren == null)
                    {
                        parentNode.NonvalidSecondaryChildren = new List<ITaxonRelationsTreeEdge>();
                    }
                    parentNode.NonvalidSecondaryChildren.Add(treeEdge);

                    if (childNode.NonvalidSecondaryParents == null)
                    {
                        childNode.NonvalidSecondaryParents = new List<ITaxonRelationsTreeEdge>();
                    }
                    childNode.NonvalidSecondaryParents.Add(treeEdge);

                    tree.TreeEdges.Add(treeEdge);
                }
                else
                {
                    tree.NotUsedTreeEdges.Add(treeEdge);
                    if (parentNode.NotUsedEdges == null)
                    {
                        parentNode.NotUsedEdges = new List<ITaxonRelationsTreeEdge>();
                    }
                    parentNode.NotUsedEdges.Add(treeEdge);

                    if (childNode.NotUsedEdges == null)
                    {
                        childNode.NotUsedEdges = new List<ITaxonRelationsTreeEdge>();
                    }
                    childNode.NotUsedEdges.Add(treeEdge);
                }
            }

            if (taxa != null)
            {
                foreach (var taxon in taxa)
                {
                    if (!treeNodesDictionary.ContainsKey(taxon.Id))
                    {
                        var node = new TaxonRelationsTreeNode(taxon);
                        treeNodesDictionary.Add(taxon.Id, node);                        
                        tree.AllTreeNodes.Add(node);
                        tree.TreeNodesWithoutEdges.Add(node);
                    }
                }
            }

            // todo - find all root nodes.
            if (treeNodesDictionary.Count > 0)
            {
                ITaxonRelationsTreeNode rootNode;
                // Try to set Biota as root node
                if (treeNodesDictionary.TryGetValue(0, out rootNode))
                {
                    tree.Root = rootNode;
                }
                // Else try set parameter taxon as root
                else if (taxa != null && taxa.Count == 1)
                {
                    if (treeNodesDictionary.TryGetValue(taxa[0].Id, out rootNode))
                    {
                        tree.Root = rootNode;
                    }
                }
            }
            
            tree.TreeNodeDictionary = treeNodesDictionary;

            return tree;
        }
    }
}

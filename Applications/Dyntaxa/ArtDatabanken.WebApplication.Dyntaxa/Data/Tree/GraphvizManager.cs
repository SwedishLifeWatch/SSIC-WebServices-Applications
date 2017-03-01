using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Tree
{
    /// <summary>
    /// GraphViz format.
    /// </summary>
    public struct GraphVizFormat
    {
        /// <summary>
        /// Gets or sets a value indicating whether relation id should be shown in graph.
        /// </summary>        
        public bool ShowRelationId { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether lumps and splits should be included in graph.
        /// </summary>        
        public bool ShowLumpsAndSplits { get; set; }
    }

    /// <summary>
    /// Manager for creating graphviz files.
    /// </summary>
    public static class GraphvizManager
    {
        /// <summary>
        /// Creates a graphviz format representation of the tree edges and its nodes.
        /// </summary>
        /// <param name="edges">The edges.</param>
        /// <returns>A graphviz format representation of the edges and its nodes.</returns>
        public static string CreateGraphvizFormatRepresentation(IEnumerable<ITaxonRelationsTreeEdge> edges)
        {
            // Get all nodes and remove duplicates using a HashSet.
            HashSet<ITaxonRelationsTreeNode> nodes = new HashSet<ITaxonRelationsTreeNode>();            
            foreach (var edge in edges)
            {
                nodes.Add(edge.Parent);
                nodes.Add(edge.Child);
            }            

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph {");            
            foreach (ITaxonRelationsTreeNode node in nodes)
            {
                sb.AppendLine(string.Format(
                    "node_{0} [label=\"{1}\", shape=box, style=rounded, color={2}, peripheries={3}, penwidth={4}];",
                    node.Taxon.Id,
                    GetNodeLabel(node),
                    node.Taxon.IsValid ? "black" : "red",
                    node.Taxon.Category.IsTaxonomic ? 1 : 2,
                    node.Taxon.IsValid ? 1 : 2));
            }

            foreach (var edge in edges)
            {
                sb.AppendLine(string.Format(
                    "node_{0} -> node_{1} [style={2}, color={3}, label=\"{4}\"];",
                    edge.Parent.Taxon.Id,
                    edge.Child.Taxon.Id,
                    edge.IsMain ? "solid" : "dashed",
                    edge.IsValid ? "black" : "red",
                    edge.IsValid ? edge.TaxonRelation.Id.ToString() : string.Format("({0})\\n\\[Not valid\\]", edge.TaxonRelation.Id)));                
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        /// <summary>
        /// Creates a graphviz format representation of the tree edges and its nodes.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="tree">The tree.</param>
        /// <param name="treeNodes">The tree nodes.</param>
        /// <param name="treeIterationMode">The tree iteration mode.</param>
        /// <param name="onlyValid">if set to <c>true</c> only valid relations are included.</param>
        /// <param name="format">The format.</param>
        /// <returns>A GraphViz graph.</returns>
        public static string CreateGraphvizFormatRepresentation(
            IUserContext userContext,
            TaxonRelationsTree tree,
            ICollection<ITaxonRelationsTreeNode> treeNodes, 
            TaxonRelationsTreeIterationMode treeIterationMode, 
            bool onlyValid,
            GraphVizFormat format)
        {
            // Get edges.
            var edges = tree.GetAllEdges(
                treeNodes, 
                treeIterationMode, 
                onlyValid, 
                treeIterationMode != TaxonRelationsTreeIterationMode.OnlyChildren);

            string str = CreateGraphvizFormatRepresentation(
                userContext,
                tree,
                edges,
                treeNodes,
                format);

            return str;
        }

        /// <summary>
        /// Creates a graphviz format representation of the tree edges and its nodes.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="tree">The tree.</param>
        /// <param name="edges">The edges.</param>
        /// <param name="sourceNodes">The source nodes.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static string CreateGraphvizFormatRepresentation(
            IUserContext userContext,
            TaxonRelationsTree tree,
            IEnumerable<ITaxonRelationsTreeEdge> edges,
            IEnumerable<ITaxonRelationsTreeNode> sourceNodes,
            GraphVizFormat format)
        {
            // Get all nodes and remove duplicates using a HashSet.
            HashSet<ITaxonRelationsTreeNode> nodes = new HashSet<ITaxonRelationsTreeNode>();
            if (edges != null)
            {
                foreach (var edge in edges)
                {
                    nodes.Add(edge.Parent);
                    nodes.Add(edge.Child);
                }
            }

            if (sourceNodes != null)
            {
                foreach (ITaxonRelationsTreeNode node in sourceNodes)
                {
                    nodes.Add(node);
                }
            }

            HashSet<ITaxon> taxaSet = new HashSet<ITaxon>(nodes.Select(x => x.Taxon));
            HashSet<LumpSplitEventList> lumps = null;
            HashSet<LumpSplitEventList> splits = null;

            if (format.ShowLumpsAndSplits)
            {
                HashSet<int> taxonIdsSet = new HashSet<int>(nodes.Select(x => x.Taxon.Id));
                lumps = GetAllLumpEventListsForTaxa(userContext, nodes);
                splits = GetAllSplitEventListsForTaxa(userContext, nodes);
                HashSet<int> lumpSplitExtraTaxonIds = new HashSet<int>();
                HashSet<ITaxon> lumpSplitExtraTaxa = GetLumpSplitsExtraTaxa(taxaSet, splits, lumps);

                foreach (var taxon in lumpSplitExtraTaxa)
                {
                    nodes.Add(tree.GetTreeNode(taxon.Id));
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph {");

            //-------------------
            // Create main tree
            //-------------------
            foreach (ITaxonRelationsTreeNode node in nodes)
            {
                sb.AppendLine(string.Format(
                    "node_{0} [label=\"{1}\", shape=box, style=rounded, color={2}, peripheries={3}, penwidth={4}];",
                    node.Taxon.Id,
                    GetNodeLabel(node),
                    node.Taxon.IsValid ? "black" : "red",
                    node.Taxon.Category.IsTaxonomic ? 1 : 2,
                    node.Taxon.IsValid ? 1 : 2));
            }

            foreach (var edge in edges)
            {
                string edgeLabel;
                if (format.ShowRelationId)
                {
                    edgeLabel = edge.IsValid ? edge.TaxonRelation.Id.ToString() : string.Format("({0})\\n\\[Not valid\\]", edge.TaxonRelation.Id);
                }
                else
                {
                    edgeLabel = edge.IsValid ? "" : "\\[Not valid\\]";
                }

                sb.AppendLine(string.Format(
                    "node_{0} -> node_{1} [style={2}, color={3}, label=\"{4}\"];",
                    edge.Parent.Taxon.Id,
                    edge.Child.Taxon.Id,
                    edge.IsMain ? "solid" : "dashed",
                    edge.IsValid ? "black" : "red",
                    edgeLabel));
            }

            //------------------
            // Add lump splits
            //------------------
            if (format.ShowLumpsAndSplits)
            {
                //lumps = GetAllLumpEventListsForTaxa(userContext, nodes);
                //splits = GetAllSplitEventListsForTaxa(userContext, nodes);

                int clusterCount = 0;
                // Lumps
                foreach (LumpSplitEventList lumpSplitEventList in lumps)
                {
                    AddLumpCluster(sb, lumpSplitEventList, clusterCount, taxaSet);
                    clusterCount++;
                }

                // Splits
                foreach (LumpSplitEventList lumpSplitEventList in splits)
                {
                    AddSplitCluster(sb, lumpSplitEventList, clusterCount, taxaSet);
                    clusterCount++;
                }
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        /// <summary>
        /// Gets the lump splits extra taxa that is not in taxaSet.
        /// </summary>
        /// <param name="taxaSet">The taxa set.</param>
        /// <param name="splits">The splits.</param>
        /// <param name="lumps">The lumps.</param>
        /// <returns>All lump split taxa that is not in taxaSet</returns>
        private static HashSet<ITaxon> GetLumpSplitsExtraTaxa(HashSet<ITaxon> taxaSet, HashSet<LumpSplitEventList> splits, HashSet<LumpSplitEventList> lumps)
        {
            HashSet<ITaxon> lumpSplitExtraTaxa = new HashSet<ITaxon>();
            foreach (LumpSplitEventList lumpSplitEventList in lumps)
            {
                foreach (ILumpSplitEvent lumpSplitEvent in lumpSplitEventList)
                {
                    //if (!taxonIdsSet.Contains(lumpSplitEvent.TaxonBefore.Id))
                    if (!taxaSet.Contains(lumpSplitEvent.TaxonBefore))
                    {
                        lumpSplitExtraTaxa.Add(lumpSplitEvent.TaxonBefore);
                    }

                    if (!taxaSet.Contains(lumpSplitEvent.TaxonAfter))
                    {
                        lumpSplitExtraTaxa.Add(lumpSplitEvent.TaxonAfter);
                    }
                }
            }

            foreach (LumpSplitEventList lumpSplitEventList in splits)
            {
                foreach (ILumpSplitEvent lumpSplitEvent in lumpSplitEventList)
                {
                    if (!taxaSet.Contains(lumpSplitEvent.TaxonBefore))
                    {
                        lumpSplitExtraTaxa.Add(lumpSplitEvent.TaxonBefore);
                    }

                    if (!taxaSet.Contains(lumpSplitEvent.TaxonAfter))
                    {
                        lumpSplitExtraTaxa.Add(lumpSplitEvent.TaxonAfter);
                    }
                }
            }

            return lumpSplitExtraTaxa;
        }

        /// <summary>
        /// Adds the lump cluster.
        /// </summary>
        /// <param name="sb">The string builder.</param>
        /// <param name="lumpSplitEventList">The lump split event list.</param>
        /// <param name="clusterCount">The cluster count.</param>
        /// <param name="taxaSet">The taxa set.</param>
        private static void AddLumpCluster(
            StringBuilder sb,
            LumpSplitEventList lumpSplitEventList,
            int clusterCount,
            HashSet<ITaxon> taxaSet)
        {
            HashSet<ITaxon> lumpNodes = new HashSet<ITaxon>();
            foreach (ILumpSplitEvent lumpSplitEvent in lumpSplitEventList)
            {
                lumpNodes.Add(lumpSplitEvent.TaxonAfter);
                lumpNodes.Add(lumpSplitEvent.TaxonBefore);
            }

            sb.AppendLine("");
            sb.AppendLine(string.Format("subgraph cluster_{0} {{", clusterCount));
            sb.AppendLine("style = dashed;");
            sb.AppendLine("color = red;");
            sb.AppendLine("label = \"Lump\";");

            // Nodes
            foreach (ITaxon node in lumpNodes)
            {
                sb.AppendLine(string.Format(
                    "node_{0}cluster_{5} [label=\"{1}\", shape=box, style=rounded, color={2}, peripheries={3}, penwidth={4}];",
                    node.Id,
                    GetNodeLabel(node),
                    node.IsValid ? "black" : "red",
                    node.Category.IsTaxonomic ? 1 : 2,
                    node.IsValid ? 1 : 2,
                    clusterCount));
            }

            // Edges
            foreach (ILumpSplitEvent lumpSplitEvent in lumpSplitEventList)
            {
                //var edge = tree.GetTreeEdge(lumpSplitEvent.TaxonBefore.Id, lumpSplitEvent.TaxonAfter.Id);

                sb.AppendLine(string.Format(
                    "node_{0}cluster_{5} -> node_{1}cluster_{5} [style={2}, color={3}, label=\"{4}\"];",
                    lumpSplitEvent.TaxonBefore.Id,
                    lumpSplitEvent.TaxonAfter.Id,
                    "solid",
                    "black",
                    lumpSplitEvent.Description,
                    clusterCount));

                //edge.IsMain ? "solid" : "dashed",
                //    edge.IsValid ? "black" : "red",
                //    edge.IsValid ? edge.TaxonRelation.Id.ToString() : string.Format("({0})\\n\\[Not valid\\]", edge.TaxonRelation.Id)));
            }

            sb.AppendLine("}");
            bool appendOnlyLumpedIntoRelationFromCluster = false;

            if (appendOnlyLumpedIntoRelationFromCluster)
            {
                sb.AppendLine(
                    string.Format(
                        "node_{0}lump -> node_{0} [style={1}, color={2}, label=\"{3}\", arrowhead=\"{4}\"];",
                        lumpSplitEventList[0].TaxonAfter.Id,
                        "solid",
                        "black",
                        "",
                        "dot"));
            }
            else
            {
                foreach (ITaxon taxon in lumpNodes)
                {                    
                    if (taxaSet.Any(x => x.Id == taxon.Id))
                    {
                        sb.AppendLine(
                            string.Format(
                                "node_{0}cluster_{5} -> node_{0} [style={1}, color={2}, label=\"{3}\", arrowhead=\"{4}\"];",
                                taxon.Id,
                                "solid",
                                "black",
                                "",
                                "dot",
                                clusterCount));
                    }
                }
            }
        }

        /// <summary>
        /// Adds the split cluster.
        /// </summary>
        /// <param name="sb">The string builder.</param>
        /// <param name="lumpSplitEventList">The lump split event list.</param>
        /// <param name="clusterCount">The cluster count.</param>
        /// <param name="taxaSet">The taxa set.</param>
        private static void AddSplitCluster(
            StringBuilder sb,
            LumpSplitEventList lumpSplitEventList,
            int clusterCount,
            HashSet<ITaxon> taxaSet)
        {
            HashSet<ITaxon> splitNodes = new HashSet<ITaxon>();
            foreach (ILumpSplitEvent lumpSplitEvent in lumpSplitEventList)
            {
                splitNodes.Add(lumpSplitEvent.TaxonAfter);
                splitNodes.Add(lumpSplitEvent.TaxonBefore);
            }
            sb.AppendLine("");
            sb.AppendLine(string.Format("subgraph cluster_{0} {{", clusterCount));
            sb.AppendLine("style = dashed;");
            sb.AppendLine("color = red;");
            sb.AppendLine("label = \"Split\";");

            // Nodes                    
            foreach (ITaxon node in splitNodes)
            {
                sb.AppendLine(string.Format(
                    "node_{0}cluster_{5} [label=\"{1}\", shape=box, style=rounded, color={2}, peripheries={3}, penwidth={4}];",
                    node.Id,
                    GetNodeLabel(node),
                    node.IsValid ? "black" : "red",
                    node.Category.IsTaxonomic ? 1 : 2,
                    node.IsValid ? 1 : 2,
                    clusterCount));
            }

            // Edges
            foreach (ILumpSplitEvent lumpSplitEvent in lumpSplitEventList)
            {
                //var edge = tree.GetTreeEdge(lumpSplitEvent.TaxonBefore.Id, lumpSplitEvent.TaxonAfter.Id);

                sb.AppendLine(string.Format(
                    "node_{0}cluster_{5} -> node_{1}cluster_{5} [style={2}, color={3}, label=\"{4}\"];",
                    lumpSplitEvent.TaxonBefore.Id,
                    lumpSplitEvent.TaxonAfter.Id,
                    "solid",
                    "black",
                    lumpSplitEvent.Description,
                    clusterCount));

                //edge.IsMain ? "solid" : "dashed",
                //    edge.IsValid ? "black" : "red",
                //    edge.IsValid ? edge.TaxonRelation.Id.ToString() : string.Format("({0})\\n\\[Not valid\\]", edge.TaxonRelation.Id)));
            }

            sb.AppendLine("}");
            bool appendOnlyLumpedIntoRelationFromCluster = false;

            if (appendOnlyLumpedIntoRelationFromCluster)
            {
                sb.AppendLine(
                    string.Format(
                        "node_{0}lump -> node_{0} [style={1}, color={2}, label=\"{3}\", arrowhead=\"{4}\"];",
                        lumpSplitEventList[0].TaxonAfter.Id,
                        "solid",
                        "black",
                        "",
                        "dot"));
            }
            else
            {
                foreach (ITaxon taxon in splitNodes)
                {                    
                    if (taxaSet.Any(x => x.Id == taxon.Id))
                    {
                        sb.AppendLine(
                            string.Format(
                                "node_{0}cluster_{5} -> node_{0} [style={1}, color={2}, label=\"{3}\", arrowhead=\"{4}\"];",
                                taxon.Id,
                                "solid",
                                "black",
                                "",
                                "dot",
                                clusterCount));
                    }
                }
            }
        }

        /// <summary>
        /// Gets all lump event lists for taxa.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="nodes">The nodes.</param>
        /// <returns>All lump split events for taxa set.</returns>
        private static HashSet<LumpSplitEventList> GetAllLumpEventListsForTaxa(IUserContext userContext, HashSet<ITaxonRelationsTreeNode> nodes)
        {
            HashSet<LumpSplitEventList> lumpSet = new HashSet<LumpSplitEventList>(new LumpSplitEventListComparer());            

            foreach (ITaxonRelationsTreeNode treeNode in nodes)
            {
                LumpSplitEventList lumpList = GetLumpEventsIncludingRelatedLumpEventsForTaxon(userContext, treeNode.Taxon);
                if (lumpList != null && lumpList.Count > 0)
                {
                    lumpSet.Add(lumpList);
                }
            }

            return lumpSet;
        }

        /// <summary>
        /// Gets the lump events including related lump events for taxon.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">The taxon.</param>
        /// <returns>Lump events including related lumpevents for the taxon.</returns>
        private static LumpSplitEventList GetLumpEventsIncludingRelatedLumpEventsForTaxon(IUserContext userContext, ITaxon taxon)
        {
            LumpSplitEventList lumpEvents = null;
            if (taxon.ChangeStatus.Id == (int)TaxonChangeStatusId.InvalidDueToLump)
            {
                lumpEvents = CoreData.TaxonManager.GetLumpSplitEventsByOldReplacedTaxon(userContext, taxon);

                if (lumpEvents != null && lumpEvents.Count > 0)
                {
                    ILumpSplitEvent firstLumpEvent = lumpEvents.First(x => x.Type.Id == (int)LumpSplitEventTypeId.Lump);
                    if (firstLumpEvent != null)
                    {
                        lumpEvents = CoreData.TaxonManager.GetLumpSplitEventsByNewReplacingTaxon(userContext, firstLumpEvent.TaxonAfter);
                    }

                    //lumpEvents.RemoveAll(x => x.Type.Id != (int)LumpSplitEventTypeId.Lump);
                }
            }
            else if (taxon.ChangeStatus.Id == (int)TaxonChangeStatusId.ValidAfterLump)
            {
                lumpEvents = CoreData.TaxonManager.GetLumpSplitEventsByNewReplacingTaxon(userContext, taxon);
                //lumpEvents.RemoveAll(x => x.Type.Id != (int)LumpSplitEventTypeId.Lump);
            }

            return lumpEvents;
        }

        /// <summary>
        /// Gets all split event lists for taxa set.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="nodes">The nodes.</param>
        /// <returns>All split event lists for taxa set.</returns>
        private static HashSet<LumpSplitEventList> GetAllSplitEventListsForTaxa(IUserContext userContext, HashSet<ITaxonRelationsTreeNode> nodes)
        {
            HashSet<LumpSplitEventList> splitSet = new HashSet<LumpSplitEventList>(new LumpSplitEventListComparer());

            foreach (ITaxonRelationsTreeNode treeNode in nodes)
            {
                LumpSplitEventList splitList = GetSplitEventsIncludingRelatedSplitEventsForTaxon(userContext, treeNode.Taxon);
                if (splitList != null && splitList.Count > 0)
                {
                    splitSet.Add(splitList);
                }
            }

            return splitSet;
        }

        /// <summary>
        /// Gets the split events including related split events for taxon.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">The taxon.</param>
        /// <returns>All split events including related split events for taxon.</returns>
        private static LumpSplitEventList GetSplitEventsIncludingRelatedSplitEventsForTaxon(IUserContext userContext, ITaxon taxon)
        {
            LumpSplitEventList splitEvents = null;
            if (taxon.ChangeStatus.Id == (int)TaxonChangeStatusId.InvalidDueToSplit)
            {
                splitEvents = CoreData.TaxonManager.GetLumpSplitEventsByOldReplacedTaxon(userContext, taxon);
                //splitEvents.RemoveAll(x => x.Type.Id != (int)LumpSplitEventTypeId.Split);
            }
            else if (taxon.ChangeStatus.Id == (int)TaxonChangeStatusId.ValidAfterSplit)
            {
                splitEvents = CoreData.TaxonManager.GetLumpSplitEventsByNewReplacingTaxon(userContext, taxon);

                ILumpSplitEvent firstSplitEvent = splitEvents.First(x => x.Type.Id == (int)LumpSplitEventTypeId.Split);
                if (firstSplitEvent != null)
                {
                    splitEvents = CoreData.TaxonManager.GetLumpSplitEventsByNewReplacingTaxon(userContext, firstSplitEvent.TaxonBefore);
                }

                //splitEvents.RemoveAll(x => x.Type.Id != (int)LumpSplitEventTypeId.Split);
            }

            return splitEvents;
        }        

        /// <summary>
        /// Gets the node label.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>A string representation for the node.</returns>
        private static string GetNodeLabel(ITaxonRelationsTreeNode node)
        {
            return GetNodeLabel(node.Taxon);
            //string label = string.Format(
            //    "{0}\\n{1}\\n{2}",
            //    node.Taxon.ScientificName.Replace("/", ""),
            //    node.Taxon.Id,
            //    node.Taxon.Category.Name);

            //if (!node.Taxon.Category.IsTaxonomic)
            //{
            //    label += "\\nTaxonomic: False";
            //}
            //if (!node.Taxon.IsValid)
            //{
            //    label += "\\nValid: False";
            //}

            //return label;
        }

        /// <summary>
        /// Gets the node label.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        /// <returns>A string representation for the node.</returns>
        private static string GetNodeLabel(ITaxon taxon)
        {
            string label = string.Format(
                "{0}\\n{1}\\n{2}",
                taxon.ScientificName.Replace("/", ""),
                taxon.Id,
                taxon.Category.Name);

            if (!taxon.Category.IsTaxonomic)
            {
                label += "\\nTaxonomic: False";
            }
            if (!taxon.IsValid)
            {
                label += "\\nValid: False";
            }

            return label;
        }

        /// <summary>
        /// Removes the whitespace from a string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>A string without whitespace.</returns>
        public static string RemoveWhitespace(this string input)
        {
            int j = 0, inputlen = input.Length;
            char[] newarr = new char[inputlen];

            for (int i = 0; i < inputlen; ++i)
            {
                char tmp = input[i];

                if (!char.IsWhiteSpace(tmp))
                {
                    newarr[j] = tmp;
                    ++j;
                }
            }

            return new string(newarr, 0, j);
        }
    }

    /// <summary>
    /// LumpSplitEventListComparer compares for each LumpSplitEvent Id property.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEqualityComparer{ArtDatabanken.Data.LumpSplitEventList}" />
    public class LumpSplitEventListComparer : IEqualityComparer<LumpSplitEventList>
    {
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <paramref name="T" /> to compare.</param>
        /// <param name="y">The second object of type <paramref name="T" /> to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public bool Equals(LumpSplitEventList x, LumpSplitEventList y)
        {            
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null && y == null)
            {
                return true;
            }

            if (x == null)
            {
                return false;
            }

            if (y == null)
            {
                return false;
            }

            if (x.Count != y.Count)
            {
                return false;
            }

            bool a = new HashSet<ILumpSplitEvent>(x, new ILumpSplitEventComparer()).SetEquals(y);
            return a;

            //var a = ints1.All(ints2.Contains) && ints1.Count == ints2.Count;
            //var a = new HashSet<int>(ints1).SetEquals(ints2);

            //return x.Id.Equals(y.Id);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(LumpSplitEventList obj)
        {
            unchecked
            {
                int hash = 19;
                foreach (ILumpSplitEvent foo in obj)
                {
                    hash = hash * 31 + foo.Id.GetHashCode();
                }
                return hash;
            }
        }        
    }

    /// <summary>
    /// ILumpSplitEvent comparer compares by ILumpSplitEvent.Id.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEqualityComparer{ArtDatabanken.Data.ILumpSplitEvent}" />
    public class ILumpSplitEventComparer : IEqualityComparer<ILumpSplitEvent>
    {
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <paramref name="T" /> to compare.</param>
        /// <param name="y">The second object of type <paramref name="T" /> to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public bool Equals(ILumpSplitEvent x, ILumpSplitEvent y)
        {
            return x.Id.Equals(y.Id);            
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(ILumpSplitEvent obj)
        {
            return obj.Id.GetHashCode();            
        }
    }

    /// <summary>
    /// Taxon equality comparer by taxon id.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEqualityComparer{ArtDatabanken.Data.ITaxon}" />
    public class ITaxonEqualityComparer : IEqualityComparer<ITaxon>
    {
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <paramref name="T" /> to compare.</param>
        /// <param name="y">The second object of type <paramref name="T" /> to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public bool Equals(ITaxon x, ITaxon y)
        {
            return x.Id.Equals(y.Id);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(ITaxon obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
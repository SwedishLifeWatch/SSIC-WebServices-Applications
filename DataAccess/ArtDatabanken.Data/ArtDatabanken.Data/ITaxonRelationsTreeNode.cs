using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Taxon relations tree node interface
    /// </summary>
    public interface ITaxonRelationsTreeNode
    {
        /// <summary>
        /// Gets or sets the taxon.
        /// </summary>        
        ITaxon Taxon { get; set; }

        /// <summary>
        /// Gets or sets all edges.
        /// </summary>
        List<ITaxonRelationsTreeEdge> AllEdges { get; set; }

        /// <summary>
        /// All parent edges.
        /// </summary>
        List<ITaxonRelationsTreeEdge> AllParentEdges { get; set; }

        /// <summary>
        /// All child edges.
        /// </summary>
        List<ITaxonRelationsTreeEdge> AllChildEdges { get; set; }

        /// <summary>
        /// The valid main children.
        /// </summary>
        List<ITaxonRelationsTreeEdge> ValidMainChildren { get; set; }

        /// <summary>
        /// The nonvalid main children.
        /// </summary>
        List<ITaxonRelationsTreeEdge> NonvalidMainChildren { get; set; }

        /// <summary>
        /// The valid secondary children.
        /// </summary>
        List<ITaxonRelationsTreeEdge> ValidSecondaryChildren { get; set; }

        /// <summary>
        /// The nonvalid secondary children.
        /// </summary>
        List<ITaxonRelationsTreeEdge> NonvalidSecondaryChildren { get; set; }

        /// <summary>
        /// The valid main parents.
        /// </summary>
        List<ITaxonRelationsTreeEdge> ValidMainParents { get; set; }

        /// <summary>
        /// The nonvalid main parents.
        /// </summary>
        List<ITaxonRelationsTreeEdge> NonvalidMainParents { get; set; }

        /// <summary>
        /// The valid secondary parents.
        /// </summary>
        List<ITaxonRelationsTreeEdge> ValidSecondaryParents { get; set; }

        /// <summary>
        /// The nonvalid secondary parents.
        /// </summary>
        List<ITaxonRelationsTreeEdge> NonvalidSecondaryParents { get; set; }

        /// <summary>
        /// The not used edges.
        /// </summary>
        List<ITaxonRelationsTreeEdge> NotUsedEdges { get; set; }        
        
        /// <summary>
        /// Gets the root node.
        /// </summary>        
        ITaxonRelationsTreeNode RootNode { get; }

        /// <summary>
        /// Gets all valid secondary parent nodes in root to leaf order (top to bottom).
        /// </summary>
        /// <returns>All valid secondary parents.</returns>
        List<ITaxonRelationsTreeNode> GetAllValidSecondaryParentNodesInRootToNodeOrder();

        /// <summary>
        /// Gets all valid parents edges hierarchical from top to bottom (this node).
        /// </summary>
        /// <returns>A list containing all valid parent edges hierarchical from top to bottom.</returns>
        List<ITaxonRelationsTreeEdge> GetAllValidParentEdgesTopToBottom(bool onlyMainRelations);
    }
}
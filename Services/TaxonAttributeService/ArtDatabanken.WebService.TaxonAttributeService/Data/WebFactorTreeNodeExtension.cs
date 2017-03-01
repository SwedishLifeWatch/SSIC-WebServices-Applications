using System;
using System.Collections.Generic;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonAttributeService.Database;

namespace ArtDatabanken.WebService.TaxonAttributeService.Data
{
    /// <summary>
    /// Contains extension to the WebFactorTreeNode class.
    /// </summary>
    public static class WebFactorTreeNodeExtension
    {
        /// <summary>
        /// Add a factor tree node to the children
        /// of this factor tree node.
        /// </summary>
        /// <param name="parent">Parent factor tree node.</param>
        /// <param name='child'>Child factor tree node.</param>
        public static void AddChild(this WebFactorTreeNode parent,
                                    WebFactorTreeNode child)
        {
            parent.Children.Add(child);
            child.Parents.Add(parent);
        }

        /// <summary>
        /// Test if this factor tree node is child.
        /// </summary>
        /// <param name="parent">The factor tree node instance.</param>
        /// <returns>True, if this factor tree node has parents.</returns>
        public static Boolean IsChild(this WebFactorTreeNode parent)
        {
            return parent.Parents.Count > 0;
        }

        /// <summary>
        /// Load data into the WebFactorTreeNode instance.
        /// </summary>
        /// <param name="factorTreeNode">The factor tree node instance.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebFactorTreeNode factorTreeNode,
                                    DataReader dataReader)
        {
            factorTreeNode.Id = dataReader.GetInt32(FactorData.ID);
            factorTreeNode.Factor = new WebFactor();
            factorTreeNode.Factor.LoadData(dataReader);
            factorTreeNode.Children = new List<WebFactorTreeNode>();
            factorTreeNode.Parents = new List<WebFactorTreeNode>();
        }
    }
}
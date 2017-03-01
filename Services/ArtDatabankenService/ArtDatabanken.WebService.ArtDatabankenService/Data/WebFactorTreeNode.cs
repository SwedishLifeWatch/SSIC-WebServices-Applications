using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Contains information about a factor tree node.
    /// </summary>
    [DataContract]
    public class WebFactorTreeNode : WebData
    {
        private List<WebFactorTreeNode> _parents;

        /// <summary>
        /// Create a WebFactorTreeNode instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebFactorTreeNode(DataReader dataReader)
        {
            Id = dataReader.GetInt32(FactorData.ID);
            Factor = new WebFactor(dataReader);
            Children = new List<WebFactorTreeNode>();
            _parents = new List<WebFactorTreeNode>();
            base.LoadData(dataReader);
        }

        /// <summary>
        /// Children to this factor tree node.
        /// </summary>
        [DataMember]
        public List<WebFactorTreeNode> Children
        { get; set; }

        /// <summary>
        /// Factor for this factor tree node.
        /// </summary>
        [DataMember]
        public WebFactor Factor
        { get; set; }

        /// <summary>
        /// Id for this factor tree node.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Test if this factor tree node is child.
        /// </summary>
        public Boolean IsChild
        {
            get { return (_parents.Count > 0); }
        }

        /// <summary>
        /// Add a factor tree node to the children
        /// of this factor tree node.
        /// </summary>
        /// <param name='factorTreeNode'>Factor tree node to add.</param>
        public void AddChild(WebFactorTreeNode factorTreeNode)
        {
            if (Children.IsNull())
            {
                Children = new List<WebFactorTreeNode>();
            }
            Children.Add(factorTreeNode);
            factorTreeNode.AddParent(this);
        }

        /// <summary>
        /// Add a factor tree node to the parents
        /// of this factor tree node.
        /// </summary>
        /// <param name='factorTreeNode'>Factor tree node to add.</param>
        public void AddParent(WebFactorTreeNode factorTreeNode)
        {
            _parents.Add(factorTreeNode);
        }

        /// <summary>
        /// Check if a factor tree node is among the
        /// parents to this factor tree node.
        /// </summary>
        /// <param name='factorTreeNode'>Factor tree node to search for.</param>
        /// <returns>
        /// True if the factor tree node is among the parents
        /// to this factor tree node.
        /// </returns>
        public Boolean HasParent(WebFactorTreeNode factorTreeNode)
        {
            if (factorTreeNode.Id == this.Id)
            {
                // This object is the search parent.
                return true;
            }

            // Search among parents.
            foreach (WebFactorTreeNode parentFactorTreeNode in _parents)
            {
                if (parentFactorTreeNode.HasParent(factorTreeNode))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

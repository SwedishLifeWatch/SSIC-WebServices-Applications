using System;
using System.Collections;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.TaxonAttributeService.Data
{
    /// <summary>
    /// List class for the WebFactorTreeNode class.
    /// </summary>
    [DataContract]
    public class WebFactorTreeNodeList : ArrayList
    {
        private Hashtable _idHashtable;

        /// <summary>
        /// Constructor for the WebFactorTreeNodeList class.
        /// </summary>
        public WebFactorTreeNodeList()
        {
            _idHashtable = new Hashtable();
        }

        /// <summary>
        /// Add data to list.
        /// </summary>
        /// <param name='value'>Data to add.</param>
        /// <returns>The list index at which the value has been added.</returns>
        public override int Add(object value)
        {
            WebFactorTreeNode data;

            if (value.IsNotNull() && (value is WebFactorTreeNode))
            {
                data = (WebFactorTreeNode)value;
                _idHashtable.Add(data.Id, data);
            }

            return base.Add(value);
        }

        /// <summary>
        /// Add a collection of data objects to the list.
        /// Override method in base class.
        /// </summary>
        /// <param name='collection'>The collection to add.</param>
        public override void AddRange(ICollection collection)
        {
            WebFactorTreeNode data;

            if (collection.IsNotEmpty())
            {
                foreach (object value in collection)
                {
                    if (value.IsNotNull() && (value is WebFactorTreeNode))
                    {
                        data = (WebFactorTreeNode)value;
                        _idHashtable.Add(data.Id, data);
                    }
                }
            }

            base.AddRange(collection);
        }

        /// <summary>
        /// Checks if data is in the list.
        /// </summary>
        /// <param name='id'>Id of object to be checked.</param>
        /// <returns>Boolean value indicating if the data exists in the list or not.</returns>
        public bool Contains(int id)
        {
            object value = _idHashtable[id];

            return (value is WebFactorTreeNode);
        }

        /// <summary>
        /// Get data with specified id.
        /// </summary>
        /// <param name='id'>Id of requested data.</param>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        /// <returns>Requested data.</returns>
        private WebFactorTreeNode GetById(Int32 id)
        {
            object value = _idHashtable[id];

            if (value.IsNotNull() && (value is WebFactorTreeNode))
            {
                return (WebFactorTreeNode)value;
            }

            foreach (WebFactorTreeNode dataId in this)
            {
                if (dataId.Id == id)
                {
                    // Data found. Return it
                    return dataId;
                }
            }

            // No data found with requested id.
            throw new ArgumentException("No data with id " + id + "!");
        }

        /// <summary>
        /// Merge data object with this list.
        /// Only objects that are not already in the list
        /// are added to the list.
        /// </summary>
        /// <param name='data'>The data to merge.</param>
        public void Merge(WebFactorTreeNode data)
        {
            if (data.IsNotNull() && _idHashtable[data.Id].IsNull())
            {
                Add(data);
            }
        }

        /// <summary>
        /// Get WebFactorTreeNode with specified id.
        /// </summary>
        /// <param name='factorTreeNodeId'>Id of requested factor tree node.</param>
        /// <returns>Requested factor tree node.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public WebFactorTreeNode Get(int factorTreeNodeId)
        {
            return GetById(factorTreeNodeId);
        }

        /// <summary>
        /// Handle WebFactorTreeNode by list index.
        /// </summary>
        /// <param name="index">Factor tree nod index.</param>
        /// <returns>Factor tree node from specified index.</returns>
        // ReSharper disable once UnusedMember.Global
        public new WebFactorTreeNode this[int index]
        {
            get
            {
                return (WebFactorTreeNode)(base[index]);
            }

            set
            {
                base[index] = value;
            }
        }
    }
}
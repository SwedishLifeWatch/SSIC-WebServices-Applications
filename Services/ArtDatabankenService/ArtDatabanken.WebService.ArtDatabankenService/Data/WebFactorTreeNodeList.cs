using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// List class for the WebFactorTreeNode class.
    /// </summary>
    public class WebFactorTreeNodeList : ArrayList
    {
        private Hashtable _idHashTable;

        /// <summary>
        /// Constructor for the WebFactorTreeNodeList class.
        /// </summary>
        public WebFactorTreeNodeList()
        {
            _idHashTable = new Hashtable();
        }

        /// <summary>
        /// Add data to list.
        /// </summary>
        /// <param name='value'>Data to add</param>
        /// <returns>The list index at which the value has been added.</returns>
        public override int Add(object value)
        {
            WebFactorTreeNode data;

            if (value.IsNotNull() && (value is WebFactorTreeNode))
            {
                data = (WebFactorTreeNode)value;
                _idHashTable.Add(data.Id, data);
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
                foreach (Object value in collection)
                {
                    if (value.IsNotNull() && (value is WebFactorTreeNode))
                    {
                        data = (WebFactorTreeNode)value;
                        _idHashTable.Add(data.Id, data);
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
        public Boolean Contains(Int32 id)
        {
            Object value;

            value = _idHashTable[id];
            return ((value != null) && (value is WebFactorTreeNode));
        }

        /// <summary>
        /// Get data with specified id.
        /// </summary>
        /// <param name='id'>Id of requested data.</param>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        /// <returns>Requested data.</returns>
        protected WebFactorTreeNode GetById(Int32 id)
        {
            Object value;

            value = _idHashTable[id];
            if (value.IsNotNull() && (value is WebFactorTreeNode))
            {
                return (WebFactorTreeNode)value;
            }

            foreach (WebFactorTreeNode dataId in this)
            {
                if (dataId.Id == id)
                {
                    // Data found. Return it.
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
            if (data.IsNotNull() && _idHashTable[data.Id].IsNull())
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
        public WebFactorTreeNode Get(Int32 factorTreeNodeId)
        {
            return (WebFactorTreeNode)(GetById(factorTreeNodeId));
        }

        /// <summary>
        /// Get/set WebFactorTreeNode by list index.
        /// </summary>
        public new WebFactorTreeNode this[Int32 index]
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

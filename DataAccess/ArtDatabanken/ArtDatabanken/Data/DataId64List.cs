using System;
using System.Collections;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for all classes that implements
    /// the IDataId64 interface.
    /// </summary>
    [Serializable]
    public class DataId64List<T> : DataList<T>
    {
        private readonly Boolean _optimize;
        private readonly Hashtable _idHashTable;

        /// <summary>
        /// Constructor for the DataId64ListT class.
        /// </summary>
        public DataId64List()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the DataId64ListT class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public DataId64List(Boolean optimize)
        {
            _optimize = optimize;
            if (_optimize)
            {
                _idHashTable = new Hashtable();
            }
        }

        /// <summary>
        /// Adds an object to the end of the list.
        /// </summary>
        /// <param name='item'>
        /// The object to be added to the end of the list.
        /// </param>
        public new void Add(T item)
        {
            IDataId64 data;

            if (item.IsNotNull() && (item is IDataId64))
            {
                if (_optimize)
                {
                    data = (IDataId64)item;
                    if (!_idHashTable.Contains(data.Id))
                    {
                        _idHashTable.Add(data.Id, data);
                    }
                }
                base.Add(item);
            }
        }

        /// <summary>
        /// Adds the elements of the specified collection to
        /// the end of the list.
        /// </summary>
        /// <param name='collection'>
        /// The collection whose elements should be added
        /// to the end of the list.
        /// </param>
        public new void AddRange(IEnumerable<T> collection)
        {
            if (collection.IsNotNull())
            {
                foreach (T value in collection)
                {
                    Add(value);
                }
            }
        }

        /// <summary>
        /// Removes all elements from the list.
        /// </summary>
        public new void Clear()
        {
            if (_optimize)
            {
                _idHashTable.Clear();
            }
            base.Clear();
        }

        /// <summary>
        /// Determines whether an element is in the list.
        /// </summary>
        /// <param name='item'>
        /// The object to locate in the list.
        /// The value can be null.
        /// </param>
        /// <returns>True if item is found in the list.</returns>
        public new bool Contains(T item)
        {
            IDataId64 value;
            Int64 itemId;

            if (item.IsNotNull() && (item is IDataId64))
            {
                itemId = ((IDataId64)item).Id;
                if (_optimize)
                {
                    value = (IDataId64)(_idHashTable[itemId]);
                    return (value.IsNotNull());
                }
                else
                {
                    foreach (IDataId64 dataId64 in this)
                    {
                        if (dataId64.Id == itemId)
                        {
                            // Data found.
                            return true;
                        }
                    }
                }
            }

            // Data not found.
            return false;
        }

        /// <summary>
        /// Determines whether an element is in the list.
        /// </summary>
        /// <param name='itemId'>
        /// Id for the object to locate in the list.
        /// </param>
        /// <returns>True if item is found in the list.</returns>
        public Boolean Contains(Int64 itemId)
        {
            return Find(itemId).IsNotNull();
        }

        /// <summary>
        /// Determines whether an element is in the list.
        /// </summary>
        /// <param name='item'>
        /// The object to locate in the list.
        /// The value can be null.
        /// </param>
        /// <returns>True if item is found in the list.</returns>
        public Boolean Exists(T item)
        {
            return Contains(item);
        }

        /// <summary>
        /// Determines whether an element is in the list.
        /// </summary>
        /// <param name='itemId'>
        /// Id for the object to locate in the list.
        /// </param>
        /// <returns>True if item is found in the list.</returns>
        public Boolean Exists(Int64 itemId)
        {
            return Find(itemId).IsNotNull();
        }

        /// <summary>
        /// Returns the first item in the list wich has
        /// an id corresponding to the parameter itemId.
        /// </summary>
        /// <param name="itemId">Id to search for</param>
        /// <returns>Object with the correct id or null if none were found.</returns>
        public virtual T Find(Int64 itemId)
        {
            IDataId64 value;

            if (_optimize)
            {
                value = (IDataId64)(_idHashTable[itemId]);
                if (value.IsNotNull())
                {
                    // Item found.
                    return (T)value;
                }
            }
            else
            {
                foreach (IDataId64 dataId64 in this)
                {
                    if (dataId64.Id == itemId)
                    {
                        // Item found.
                        return (T)dataId64;
                    }
                }
            }

            // Item not found.
            return default(T);
        }

        /// <summary>
        /// Returns the first item in the list wich has
        /// an id corresponding to the parameter itemId.
        /// </summary>
        /// <param name="itemId">Id to search for</param>
        /// <returns>Object with the correct id.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public T Get(Int64 itemId)
        {
            T value;

            value = Find(itemId);
            if (value.IsNull())
            {
                // Item not found.
                throw new ArgumentException("No data with id = " + itemId);
            }

            return value;
        }

        /// <summary>
        /// Makes a list of all item ids in this list.
        /// </summary>
        /// <returns>All item ids in this list.</returns>
        public List<Int64> GetIds()
        {
            List<Int64> itemIds;

            if (Count > 0)
            {
                itemIds = new List<Int64>();
                foreach (IDataId64 dataId64 in this)
                {
                    itemIds.Add(dataId64.Id);
                }
                return itemIds;
            }
            return null;
        }

        /// <summary>
        /// Merge data object with this list.
        /// Only objects that are not already in the list
        /// are added to the list.
        /// </summary>
        /// <param name='item'>The data to merge.</param>
        public void Merge(T item)
        {
            if (item.IsNotNull() && !Exists(item))
            {
                Add(item);
            }
        }

        /// <summary>
        /// Add the elements of the specified collection to
        /// the end of the list.
        /// Only objects that are not already in the list
        /// are added to the list.
        /// </summary>
        /// <param name='collection'>
        /// The collection whose elements may be added
        /// to the end of the list.
        /// </param>
        public void Merge(IEnumerable<T> collection)
        {
            if (collection.IsNotNull())
            {
                foreach (T value in collection)
                {
                    if (value.IsNotNull() &&
                        (value is IDataId64) &&
                        !Exists(value))
                    {
                        Add(value);
                    }
                }
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object
        /// from the list.
        /// </summary>
        /// <param name='item'>The object to remove from the list.</param>
        public new bool Remove(T item)
        {
            Int32 index;
            Int64 itemId;

            if ((Count > 0) &&
                item.IsNotNull() &&
                (item is IDataId64))
            {
                itemId = ((IDataId64)item).Id;
                if (_optimize)
                {
                    _idHashTable.Remove(itemId);
                }
                for (index = 0; index < Count; index++)
                {
                    if (((IDataId64)(this[index])).Id == itemId)
                    {
                        // Data found.
                        RemoveAt(index);
                        return true;
                    }
                }
            }

            // No data found.
            return false;
        }

        /// <summary>
        /// Removes the element at the specified index of the list.
        /// </summary>
        /// <param name='index'>The zero-based index of the element to remove.</param>
        public new void RemoveAt(int index)
        {
            IDataId64 data;

            if (_optimize)
            {
                data = (IDataId64)(this[index]);
                _idHashTable.Remove(data.Id);
            }
            base.RemoveAt(index);
        }

        /// <summary>
        /// Sort items based on id.
        /// </summary>
        public new void Sort()
        {
            Sort(new DataId64Comparer());
        }

        /// <summary>
        /// Get the subset of two lists.
        /// All data in "this" list that is not contained
        /// in parameter "data" is removed from this list.
        /// </summary>
        /// <param name='data'>The data list to compare with.</param>
        public void Subset(DataId64List<T> data)
        {
            Int32 index;

            if (this.IsNotEmpty())
            {
                if (data.IsEmpty())
                {
                    Clear();
                }
                else
                {
                    for (index = Count - 1; index >= 0; index--)
                    {
                        if (!data.Exists(this[index]))
                        {
                            RemoveAt(index);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the Union of two collections.
        /// The result is returned in the first collection.
        /// </summary>
        /// <param name='collection'>The collection to union with.</param>
        public void Union(IEnumerable<T> collection)
        {
            Merge(collection);
        }

        /// <summary>
        /// Private class that compares two items based on ids.
        /// </summary>
        private class DataId64Comparer : IComparer<T>
        {
            /// <summary>
            /// Compares two objects and returns a value
            /// indicating whether one is less than, equal
            /// to, or greater than the other.
            /// </summary>
            /// <param name='x'>The first object to compare.</param>
            /// <param name='y'>The second object to compare.</param>
            /// <returns>
            /// A signed integer that indicates the relative values of x and y.
            /// If x is less than y less than zero is returned.
            /// If x equals y zero is returned.
            /// If x is greater than y greater than zero is returned.
            /// </returns>
            public int Compare(T x, T y)
            {
                Int64 idX, idY;

                idX = ((IDataId64)x).Id;
                idY = ((IDataId64)y).Id;
                if (idX < idY)
                {
                    return -1;
                }

                if (idX == idY)
                {
                    return 0;
                }

                // if (idX > idY)
                return 1;
            }
        }
    }
}

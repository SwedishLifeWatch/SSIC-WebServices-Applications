using System;
using System.Collections;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for all classes that implements
    /// the IDataId32 interface.
    /// </summary>
    [Serializable]
    public class DataId32List<T> : DataList<T>
    {
        private readonly Hashtable _idHashTable;

        /// <summary>
        /// Constructor for the DataId32ListT class.
        /// </summary>
        public DataId32List()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the DataId32ListT class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public DataId32List(Boolean optimize)
        {
            Optimize = optimize;
            if (Optimize)
            {
                _idHashTable = new Hashtable();
            }
        }

        /// <summary>
        /// Get the factor of this species fact object.
        /// </summary>
        protected Boolean Optimize { get; private set; }

        /// <summary>
        /// Adds an object to the end of the list.
        /// </summary>
        /// <param name='item'>
        /// The object to be added to the end of the list.
        /// </param>
        public new void Add(T item)
        {
            IDataId32 data;

            if (item.IsNotNull() && (item is IDataId32))
            {
                if (Optimize)
                {
                    data = (IDataId32)item;
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
            if (Optimize)
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
            IDataId32 value;
            Int32 itemId;

            if (item.IsNotNull() && (item is IDataId32))
            {
                itemId = ((IDataId32)item).Id;
                if (Optimize)
                {
                    value = (IDataId32)(_idHashTable[itemId]);
                    return (value.IsNotNull());
                }
                else
                {
                    foreach (IDataId32 dataId32 in this)
                    {
                        if (dataId32.Id == itemId)
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
        public Boolean Contains(Int32 itemId)
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
        public Boolean Exists(Int32 itemId)
        {
            return Find(itemId).IsNotNull();
        }

        /// <summary>
        /// Returns the first item in the list wich has
        /// an id corresponding to the parameter itemId.
        /// </summary>
        /// <param name="itemId">Id to search for</param>
        /// <returns>Object with the correct id or null if none were found.</returns>
        public T Find(Int32 itemId)
        {
            IDataId32 value;

            if (Optimize)
            {
                value = (IDataId32)(_idHashTable[itemId]);
                if (value.IsNotNull())
                {
                    // Item found.
                    return (T)value;
                }
            }
            else
            {
                foreach (IDataId32 dataId32 in this)
                {
                    if (dataId32.Id == itemId)
                    {
                        // Item found.
                        return (T)dataId32;
                    }
                }
            }

            // Item not found.
            return default(T);
        }

        /// <summary>
        /// Returns the first item in the list which has
        /// an id corresponding to the parameter itemId.
        /// </summary>
        /// <param name="itemId">Id to search for.</param>
        /// <returns>Object with the correct id.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public T Get(Int32 itemId)
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
        /// Get this list converted to a DataId32List list.
        /// </summary>
        /// <returns>This list converted to a DataId32List list.</returns>
        public DataId32List<IDataId32> GetDataId32List()
        {
            DataId32List<IDataId32> dataId32List;

            dataId32List = null;
            if (this.IsNotEmpty())
            {
                dataId32List = new DataId32List<IDataId32>();
                // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
                foreach (IDataId32 dataId32 in this)
                {
                    dataId32List.Add(dataId32);
                }
            }

            return dataId32List;
        }

        /// <summary>
        /// Makes a list of all item ids in this list.
        /// </summary>
        /// <returns>All item ids in this list.</returns>
        public List<Int32> GetIds()
        {
            List<Int32> itemIds;

            if (Count > 0)
            {
                itemIds = new List<Int32>();
                // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
                foreach (IDataId32 dataId32 in this)
                {
                    itemIds.Add(dataId32.Id);
                }

                return itemIds;
            }

            return null;
        }

        /// <summary>
        /// Get index for specified object.
        /// </summary>
        /// <param name='item'>
        /// The object to locate in the list.
        /// </param>
        /// <returns>
        /// Index for specified object.
        /// -1 is returned if object was not found in list.
        /// </returns>
        public Int32 GetIndex(T item)
        {
            Int32 objectIndex, searchIndex;

            objectIndex = -1;
            if (this.IsNotEmpty() && item.IsNotNull())
            {
                for (searchIndex = 0; searchIndex < Count; searchIndex++)
                {
                    if (((IDataId32)(this[searchIndex])).Id ==
                        ((IDataId32)item).Id)
                    {
                        objectIndex = searchIndex;
                        break;
                    }
                }
            }

            return objectIndex;
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
                        (value is IDataId32) &&
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
            Int32 index, itemId;

            if ((Count > 0) &&
                item.IsNotNull() &&
                (item is IDataId32))
            {
                itemId = ((IDataId32)item).Id;
                if (Optimize)
                {
                    _idHashTable.Remove(itemId);
                }

                for (index = 0; index < Count; index++)
                {
                    if (((IDataId32)(this[index])).Id == itemId)
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
        /// Remove data from this list.
        /// All data in "this" list that is contained
        /// in parameter "data" is removed from this list.
        /// </summary>
        /// <param name='data'>Data that should be removed.</param>
        public void Remove(DataId32List<T> data)
        {
            Int32 index;

            if (this.IsNotEmpty() && data.IsNotEmpty())
            {
                for (index = Count - 1; index >= 0; index--)
                {
                    if (data.Exists(this[index]))
                    {
                        RemoveAt(index);
                    }
                }
            }
        }

        /// <summary>
        /// Removes the element at the specified index of the list.
        /// </summary>
        /// <param name='index'>The zero-based index of the element to remove.</param>
        public new void RemoveAt(int index)
        {
            IDataId32 data;

            if (Optimize)
            {
                data = (IDataId32)(this[index]);
                _idHashTable.Remove(data.Id);
            }

            base.RemoveAt(index);
        }

        /// <summary>
        /// Sort items based on id.
        /// </summary>
        public new void Sort()
        {
            Sort(new DataId32Comparer());
        }

        /// <summary>
        /// Get the subset of two lists.
        /// All data in "this" list that is not contained
        /// in parameter "data" is removed from this list.
        /// </summary>
        /// <param name='data'>The data list to compare with.</param>
        public void Subset(DataId32List<T> data)
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
        private class DataId32Comparer : IComparer<T>
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
                Int32 idX, idY;

                idX = ((IDataId32)x).Id;
                idY = ((IDataId32)y).Id;
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

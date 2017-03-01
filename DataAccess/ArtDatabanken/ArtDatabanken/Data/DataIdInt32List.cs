using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the IDataId32 interface.
    /// </summary>
    [Serializable]
    public class DataIdInt32List : DataId32List<IDataId32>
    {
        /// <summary>
        /// Constructor for the DataIdInt32List class.
        /// </summary>
        public DataIdInt32List()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the DataIdInt32List class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public DataIdInt32List(Boolean optimize)
            : base(optimize)
        {
        }

        /// <summary>
        /// Adds an id to the end of the list.
        /// </summary>
        /// <param name='id'>Id to be added to the end of the list.</param>
        public void Add(Int32 id)
        {
            Add(new DataId32(id));
        }

        /// <summary>
        /// Adds the elements of the specified collection to
        /// the end of the list.
        /// </summary>
        /// <param name='collection'>
        /// The collection whose elements should be added
        /// to the end of the list.
        /// </param>
        public void AddRange(IEnumerable<Int32> collection)
        {
            if (collection.IsNotNull())
            {
                foreach (Int32 id in collection)
                {
                    Add(id);
                }
            }
        }

        /// <summary>
        /// Get a copy of this data id list.
        /// </summary>
        /// <returns>A copy of this data id list.</returns>
        public DataIdInt32List Clone()
        {
            DataIdInt32List dataIds;

            dataIds = new DataIdInt32List(this.Optimize);
            dataIds.AddRange(this);
            return dataIds;
        }

        /// <summary>
        /// Get generic list.
        /// </summary>
        /// <returns>Generic list.</returns>       
        public List<Int32> GetInt32List()
        {
            List<Int32> list;
            Int32 index;

            list = new List<Int32>();
            if (this.IsNotEmpty())
            {
                for (index = 0; index < Count; index++)
                {
                    list.Add(this[index].Id);
                }
            }

            return list;
        }

        /// <summary>
        /// Merge data object with this list.
        /// Only objects that are not already in the list
        /// are added to the list.
        /// </summary>
        /// <param name='id'>The data to merge.</param>
        public void Merge(Int32 id)
        {
            Merge(new DataId32(id));
        }
    }
}

using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about a data id.
    /// </summary>
    [Serializable]
    public class DataId32 : IDataId32
    {
        /// <summary>
        /// Crete a DataId32 instance.
        /// </summary>
        /// <param name="dataId">Data id.</param>
        public DataId32(Int32 dataId)
        {
            Id = dataId;
        }

        /// <summary>
        /// Id for this data.
        /// </summary>
        public Int32 Id { get; set; }
    }
}

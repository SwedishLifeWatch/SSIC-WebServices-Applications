using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the IFactorDataType interface.
    /// </summary>
    [Serializable]
    public class FactorDataTypeList : DataId32List<IFactorDataType>
    {
        /// <summary>
        /// Constructor for the FactorDataTypeList class.
        /// </summary>
        public FactorDataTypeList()
            : this(true)
        {
        }

        /// <summary>
        /// Constructor for the FactorDataTypeList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public FactorDataTypeList(Boolean optimize)
            : base(optimize)
        {
        }
    }
}
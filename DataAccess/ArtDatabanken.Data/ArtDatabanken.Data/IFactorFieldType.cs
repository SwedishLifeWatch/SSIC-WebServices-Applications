using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a factor field type.
    /// </summary>
    public interface IFactorFieldType : IDataId32
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Get data type of this factor field type object.
        /// </summary>
        FactorFieldDataTypeId DataType { get; }

        /// <summary>
        /// Definition for this factor field type.
        /// </summary>
        String Definition { get; set; }

        /// <summary>
        /// Name for this factor field type.
        /// </summary>
        String Name { get; set; }
    }
}

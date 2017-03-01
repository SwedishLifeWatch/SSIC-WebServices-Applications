using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a factor data type.
    /// </summary>
    public interface IFactorDataType : IDataId32
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Definition for this factor data type.
        /// </summary>
        String Definition { get; set; }

        /// <summary>
        /// Get field 1 of this factor data type.
        /// </summary>
        IFactorField Field1 { get; }

        /// <summary>
        /// Get field 2 of this factor data type.
        /// </summary>
        IFactorField Field2 { get; }

        /// <summary>
        /// Get field 3 of this factor data type.
        /// </summary>
        IFactorField Field3 { get; }

        /// <summary>
        /// Get field 4 of this factor data type.
        /// </summary>
        IFactorField Field4 { get; }

        /// <summary>
        /// Get field 5 of this factor data type.
        /// </summary>
        IFactorField Field5 { get; }

        /// <summary>
        /// Fields this factor data type.
        /// </summary>
        FactorFieldList Fields { get; set; }

        /// <summary>
        /// An ordered list of factor fields for this factor data type. May contain empty slots.
        /// </summary>
        IFactorField[] FieldArray { get; set; }

        /// <summary>
        /// Get the main field of this factor data type.
        /// </summary>
        IFactorField MainField { get; }

        /// <summary>
        /// Name for this factor data type.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Get all substantial fields of this factor data type.
        /// </summary>
        FactorFieldList SubstantialFields { get; }
    }
}
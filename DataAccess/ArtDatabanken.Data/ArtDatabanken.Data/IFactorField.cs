using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a factor field.
    /// </summary>
    public interface IFactorField : IDataId32
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Name for this factor field in database.
        /// </summary>
        String DatabaseFieldName { get; set; }

        /// <summary>
        /// Enumeration for this factor field.
        /// This property has the value null if this factor field
        /// is not of type enumeration.
        /// </summary>
        IFactorFieldEnum Enum { get; set; }

        /// <summary>
        /// Factor data type that this factor field belongs to.
        /// </summary>
        IFactorDataType FactorDataType { get; set; }

        /// <summary>
        /// Index of this factor field.
        /// </summary>
        Int32 Index { get; set; }

        /// <summary>
        /// Information for this factor field.
        /// </summary>
        String Information { get; set; }

        /// <summary>
        /// Indicator of weather or not this factor field is in the main field.
        /// </summary>
        Boolean IsMain { get; set; }

        /// <summary>
        /// Indicator of weather or not this factor field is a substantial field.
        /// </summary>
        Boolean IsSubstantial { get; set; }

        /// <summary>
        /// Label for this factor field.
        /// </summary>
        String Label { get; set; }

        /// <summary>
        /// Maximum length of this factor field if it is of type String.
        /// </summary>
        Int32 Size { get; set; }

        /// <summary>
        /// Type for this factor field.
        /// </summary>
        IFactorFieldType Type { get; set; }

        /// <summary>
        /// Unit label for this factor field.
        /// </summary>
        String Unit { get; set; }

        /// <summary>
        /// Get min and max values for this factor field
        /// in specified factor context.
        /// Values are returned in parameter minValue and maxValue.
        /// </summary>
        /// <param name="factor">Factor context.</param>
        /// <param name='minValue'>Is set to min value.</param>
        /// <param name='maxValue'>Is set to max value.</param>
        void GetMinMax(IFactor factor,
                       ref Double minValue,
                       ref Double maxValue);

        /// <summary>
        /// Get min and max values for this factor field
        /// in specified factor context.
        /// Values are returned in parameter minValue and maxValue.
        /// </summary>
        /// <param name="factor">Factor context.</param>
        /// <param name='minValue'>Is set to min value.</param>
        /// <param name='maxValue'>Is set to max value.</param>
        void GetMinMax(IFactor factor,
                       ref Int32 minValue,
                       ref Int32 maxValue);

        /// <summary>
        /// Indicates whether or not this factor field
        /// has min and max values in specified factor context.
        /// </summary>
        /// <param name="factor">Factor context.</param>
        Boolean IsMinMaxDefined(IFactor factor);
    }
}
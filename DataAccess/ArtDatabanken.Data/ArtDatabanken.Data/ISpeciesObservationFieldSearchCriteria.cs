using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface contains a search criteria on a combination
    /// of class and data field.
    /// The search criteria handles several data types.
    /// </summary>
    public interface ISpeciesObservationFieldSearchCriteria
    {
        /// <summary>
        /// Type of data that the search criteria should be applied to.
        /// </summary>
        SpeciesObservationClass Class
        { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        DataContext DataContext { get; set; }

        /// <summary>
        /// Search fields based on information related to this locale.
        /// This search criteria is optional and for many data
        /// types it is not relevant. For example data and time
        /// fields does not use this property.
        /// The value of this property Locale may be different from the
        /// value of property Locale in class ClientInformation.
        /// </summary>
        Locale Locale
        { get; set; }

        /// <summary>
        /// Compare operator that is used when this search
        /// criteria is compared to actual data.
        /// </summary>
        CompareOperator Operator
        { get; set; }

        /// <summary>
        /// Information about which species observation property
        /// that the search criteria should be applied to.
        /// </summary>
        SpeciesObservationProperty Property
        { get; set; }

        /// <summary>
        /// Type of the data. Both compare value (property Value
        /// in this class) and actual value should be of this
        /// data type.
        /// </summary>
        DataType Type
        { get; set; }

        /// <summary>
        /// Value used to compare the data field with.
        /// Value is a string representation of the actual data type
        /// and value.
        /// </summary>
        String Value
        { get; set; }
    }
}

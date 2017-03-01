using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains a search criteria on a combination
    /// of class and data field.
    /// The search criteria handles several data types.
    /// </summary>
    [DataContract]
    public class SpeciesObservationFieldSearchCriteria : ISpeciesObservationFieldSearchCriteria
    {
        /// <summary>
        /// Type of data that the search criteria should be applied to.
        /// </summary>
        [DataMember]
        public SpeciesObservationClass Class
        { get; set; }

        /// <summary>
        /// Search fields based on information related to this locale.
        /// This search criteria is optional and for many data
        /// types it is not relevant. For example data and time
        /// fields does not use this property.
        /// The value of this property Locale may be different from the
        /// value of property Locale in class ClientInformation.
        /// </summary>
        [DataMember]
        public Locale Locale
        { get; set; }

        /// <summary>
        /// Compare operator that is used when this search
        /// criteria is compared to actual data.
        /// </summary>
        [DataMember]
        public CompareOperator Operator
        { get; set; }

        /// <summary>
        /// Information about which species observation property
        /// that the search criteria should be applied to.
        /// </summary>
        [DataMember]
        public SpeciesObservationProperty Property
        { get; set; }

        /// <summary>
        /// Type of the data. Both compare value (property Value
        /// in this class) and actual value should be of this
        /// data type.
        /// </summary>
        [DataMember]
        public DataType Type
        { get; set; }

        /// <summary>
        /// Value used to compare the data field with.
        /// Value is a string representation of the actual data type
        /// and value.
        /// </summary>
        [DataMember]
        public String Value
        { get; set; }

        /// <summary>
        /// The data context.
        /// </summary>
        [DataMember]
        public DataContext DataContext
        { get; set; }
    }
}

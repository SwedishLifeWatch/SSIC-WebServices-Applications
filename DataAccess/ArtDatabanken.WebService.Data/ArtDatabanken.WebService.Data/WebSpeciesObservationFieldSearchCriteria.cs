using System;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains a search criteria on a combination
    /// of class and data field.
    /// The search criteria handles several data types.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationFieldSearchCriteria : WebData
    {
        /// <summary>
        /// Type of data that the search criteria should be applied to.
        /// </summary>
        [DataMember]
        public WebSpeciesObservationClass Class
        { get; set; }

        /// <summary>
        /// Search fields based on information related to this locale.
        /// This search criteria is optional and for many data
        /// types it is not relevant. For example data and time
        /// fields does not use this property.
        /// The value of this property Locale may be different from the
        /// value of property Locale in class WebClientInformation.
        /// Information returned from a web service call uses the locale
        /// specified in class WebClientInformation.
        /// </summary>
        [DataMember]
        public WebLocale Locale
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
        public WebSpeciesObservationProperty Property
        { get; set; }

        /// <summary>
        /// Type of the data. Both compare value (property Value
        /// in this class) and actual value should be of this
        /// data type.
        /// </summary>
        [DataMember]
        public WebDataType Type
        { get; set; }

        /// <summary>
        /// Value used to compare the data field with.
        /// Value is a string representation of the actual data type
        /// and value.
        /// </summary>
        [DataMember]
        public String Value
        { get; set; }
    }
}

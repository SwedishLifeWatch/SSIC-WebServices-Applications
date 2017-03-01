using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Definition of operators that can be used
    /// when search criterias are compared to real data.
    /// Only a limited combinations of operators and 
    /// data types are used.
    /// For example: Data type 'Boolean' should only be used
    /// in combination with operator 'Equal' and 'NotEqual'.
    /// Some values are shared by other compare operator enums. For example, StringCompareOperator and NumberCompareOperator.
    /// Make sure index numbers are mapped correctly for each enum. This is to make it possible to convert values between related enums.
    /// </summary>
    [DataContract]
    public enum CompareOperator
    {
        /// <summary>
        /// Search for strings that begins with the specified
        /// search string. Wild chard is added to the search
        /// string (at the end) before the search begins.
        /// ex. like searchtext%
        /// </summary>
        [EnumMember]
        BeginsWith = 0,

        /// <summary>
        /// Search for strings that contains the specified
        /// search string. Wild chards are added to the search string
        /// (at the beginning and end) before the search begins.
        /// ex. like %searchtext%
        /// </summary>
        [EnumMember]
        Contains = 1,

        /// <summary>
        /// Search for strings that ends with the specified
        /// search string. Wild chard is added to the search
        /// string (at the beginning) before the search begins.
        /// ex. like %searchtext
        /// </summary>
        [EnumMember]
        EndsWith = 2,

        /// <summary>Equal.</summary>
        [EnumMember]
        Equal = 3,

        /// <summary>
        /// This operator may be used if data that has a range
        /// is compared to some other data that also has a range.
        /// Excluding means that true is returned if range of compared
        /// data are completely within the range of the data that
        /// it is compared to.
        /// </summary>
        [EnumMember]
        Excluding = 4,

        /// <summary>Greater.</summary>
        [EnumMember]
        Greater = 5,

        /// <summary>Greater or equal.</summary>
        [EnumMember]
        GreaterOrEqual = 6,

        /// <summary>
        /// This operator may be used if data that has a range
        /// is compared to some other data that also has a range.
        /// Including means that true is returned if range of compare
        /// data to any part is within the range of the data that it
        /// is compared to.
        /// </summary>
        [EnumMember]
        Including = 7,

        /// <summary>Less.</summary>
        [EnumMember]
        Less = 8,

        /// <summary>Less or equal.</summary>
        [EnumMember]
        LessOrEqual = 9,

        /// <summary>
        /// Like.
        /// This operator only applies to data type 'String'
        /// </summary>
        [EnumMember]
        Like = 10,

        /// <summary>Not equal.</summary>
        [EnumMember]
        NotEqual = 11
    }
}

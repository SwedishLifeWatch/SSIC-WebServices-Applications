using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Definition of operators that can be used
    /// when search criterias are compared to real data.
    /// Only a limited combinations of operators and 
    /// data types are used.
    /// Some values are shared by other compare operator enums. For example, StringCompareOperator and CompareOperator.
    /// Make sure index numbers are mapped correctly for each enum. This is to make it possible to convert values between related enums.
    /// </summary>
    [DataContract]
    public enum NumberCompareOperator
    {
        /// <summary>Equal.</summary>
        [EnumMember]
        Equal = 3,

        /// <summary>Greater.</summary>
        [EnumMember]
        Greater = 5,

        /// <summary>Greater or equal.</summary>
        [EnumMember]
        GreaterOrEqual = 6,

        /// <summary>Less.</summary>
        [EnumMember]
        Less = 8,

        /// <summary>Less or equal.</summary>
        [EnumMember]
        LessOrEqual = 9,

        /// <summary>Not equal.</summary>
        [EnumMember]
        NotEqual = 11
    }
}

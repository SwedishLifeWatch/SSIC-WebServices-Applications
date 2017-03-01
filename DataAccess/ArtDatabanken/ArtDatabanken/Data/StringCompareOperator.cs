using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Definition of how search string is compared to
    /// stored string data.
    /// Some values are shared by other compare operator enums. For example, CompareOperator and NumberCompareOperator.
    /// Make sure index numbers are mapped correctly for each enum. This is to make it possible to convert values between related enums.
    /// </summary>
    [DataContract]
    public enum StringCompareOperator
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

        /// <summary>
        /// Search for strings that are equal to the specified
        /// search string.
        /// ex. = searchtext
        /// </summary>
        [EnumMember]
        Equal = 3,

        /// <summary>
        /// Search for strings that similar to the specified search string.
        /// No wild chards are added to the search string.
        /// ex. like searchtext
        /// </summary>
        [EnumMember]
        Like = 10,

        /// <summary>
        /// Search for strings that are not equal to the specified
        /// search string.
        /// ex. &lt;&gt; searchtext
        /// </summary>
        [EnumMember]
        NotEqual = 11
    }
}

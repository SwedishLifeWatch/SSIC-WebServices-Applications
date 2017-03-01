namespace ArtDatabanken.Data
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Definition of data types that can be contained
    /// in a WebDataField instance.
    /// </summary>
    [DataContract]
    public enum WebDataType
    {
        /// <summary>A Boolean value.</summary>
        [EnumMember]
        Boolean,
        /// <summary>A DateTime value.</summary>
        [EnumMember]
        DateTime,
        /// <summary>A Float value. Standard 64-bit floating point.</summary>
        [EnumMember]
        Float,
        /// <summary>A Int32 value.</summary>
        [EnumMember]
        Int32,
        /// <summary>A Int64 value.</summary>
        [EnumMember]
        Int64,
        /// <summary>A String value.</summary>
        [EnumMember]
        String
    }
}
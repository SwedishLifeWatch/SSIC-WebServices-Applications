using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Definition of data types that can be handled
    /// in a generic 'Value' property as string.
    /// </summary>
    [DataContract]
    public enum DataType
    {
        /// <summary>
        /// A Boolean value.
        /// String representation of a Boolean value is
        /// either True or False.
        /// </summary> 
        [EnumMember]
        Boolean,
        /// <summary>
        /// A DateTime value.
        /// String representation of a DateTime value is in the format
        /// Year-Month-DayTHour:Minute:Second:PartOfSecond, example
        /// 2008-06-15T21:15:07.0000000.
        /// </summary>
        [EnumMember]
        DateTime,
        /// <summary>
        /// A Float value. Standard 64-bit floating point.
        /// String representation of a Float64 value is in the format
        /// 3.1415926536E+000.
        /// </summary>
        [EnumMember]
        Float64,
        /// <summary>
        /// An Int32 value.
        /// String representation of a Int32 value is in the format
        /// 2147483647.
        /// </summary>
        [EnumMember]
        Int32,
        /// <summary>
        /// An Int64 value.
        /// String representation of a Int64 value is in the format
        /// 9223372036854775807.
        /// </summary>
        [EnumMember]
        Int64,
        /// <summary>
        /// A String value.
        /// </summary>
        [EnumMember]
        String,
        /// <summary>
        /// A Byte value.
        /// </summary>
        [EnumMember]
        Byte,
        /// <summary>
        /// A float 32.
        /// </summary>
        [EnumMember]
        Float32
    }
}


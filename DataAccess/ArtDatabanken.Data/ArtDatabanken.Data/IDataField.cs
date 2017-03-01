using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// An instance of this interface contains one piece of data.
    /// The data can be of any type that the enum DataType defines.
    /// </summary>
    public interface IDataField
    {
        /// <summary>
        /// Any relevant information related to this data.
        /// </summary>
        String Information { get; set; }

        /// <summary>
        /// Name of the data.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Type of the data.
        /// </summary>
        DataType Type { get; set; }

        /// <summary>
        /// Unit of the data.
        /// </summary>
        String Unit { get; set; }

        /// <summary>
        /// Value of the data in string format.
        /// String representation of different data types:
        /// 
        /// Boolean: String has value "True" or "False".
        /// 
        /// DateTime: Format is YYYY-MM-DDTHH:mm:ss.fffffff,
        /// for example 1998-01-01T00:00:00.0000000.
        /// 
        /// Float: Handled as a double-precision 64-bit number that
        /// complies with the IEC 60559:1989 (IEEE 754) standard for
        /// binary floating-point arithmetic. Format is
        /// D.DDDDDDDDDDE+or-DDD. for example 3.1415926536E+000.
        /// May have a leading minus sign.
        /// 
        /// 32 bits integer: A sequence of digits without spaces or commas,
        /// for example 6258250. May have a leading minus sign.
        /// 
        /// 64 bits integer: A sequence of digits without spaces or commas,
        /// for example 43254236258250. May have a leading minus sign.
        /// 
        /// String: No conversion, contains actual value.
        /// </summary>
        String Value { get; set; }
    }
}

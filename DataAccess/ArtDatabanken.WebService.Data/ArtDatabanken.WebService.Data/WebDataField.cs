using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// An instance of this class contains one piece of data.
    /// The data can be of any type that the enum WebDataType defines.
    /// </summary>
    [DataContract]
    public class WebDataField
    {
        /// <summary>
        /// Any relevant information related to this data.
        /// </summary>
        [DataMember]
        public String Information
        { get; set; }

        /// <summary>
        /// Name of the data.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Type of the data.
        /// </summary>
        [DataMember]
        public WebDataType Type
        { get; set; }

        /// <summary>
        /// Unit of the data.
        /// </summary>
        [DataMember]
        public String Unit
        { get; set; }

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
        [DataMember]
        public String Value
        { get; set; }
    }
}

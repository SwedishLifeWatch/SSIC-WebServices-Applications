using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains a value for one property
    /// in one species observation class.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationField : WebData
    {
        /// <summary>
        /// Information about which species observation
        /// class that this value belongs to.
        /// </summary>
        [DataMember]
        public String ClassIdentifier { get; set; }

        /// <summary>
        /// This property contains index information related
        /// to this species observation field instance.
        /// This property i currently not used.
        /// </summary>
        [DataMember]
        public WebSpeciesObservationFieldIndex Index { get; set; }

        /// <summary>
        /// Information about which species observation
        /// property that this value belongs to.
        /// </summary>
        [DataMember]
        public String PropertyIdentifier { get; set; }

        /// <summary>
        /// Type of the data.
        /// </summary>
        [DataMember]
        public WebDataType Type { get; set; }

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
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public String Value { get; set; }
    }
}

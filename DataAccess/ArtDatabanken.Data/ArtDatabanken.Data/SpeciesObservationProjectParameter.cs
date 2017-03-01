using System;
using System.Diagnostics.CodeAnalysis;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about species observation project parameters that are 
    /// related to this species observation.
    /// Each project parameter contains parameter information but also information
    /// about the project. The reason for this is that one species observation can
    /// belong to more than one project.
    /// </summary>
    public class SpeciesObservationProjectParameter : ISpeciesObservationProjectParameter
    {
        /// <summary>
        /// Unique GUID among all species observation project parameters.
        /// </summary>
        public String Guid { get; set; }

        /// <summary>
        /// Project id.
        /// </summary>
        public Int32 ProjectId { get; set; }

        /// <summary>
        /// Project name.
        /// </summary>
        public String ProjectName { get; set; }

        /// <summary>
        /// Not unique property name.
        /// </summary>
        public String Property { get; set; }

        /// <summary>
        /// Unique identifier among all species observation project parameters.
        /// </summary>
        public String PropertyIdentifier { get; set; }

        /// <summary>
        /// Data type for this species observation project parameter.
        /// </summary>
        public DataType Type { get; set; }

        /// <summary>
        /// Unit for this species observation project parameter.
        /// Not defined in all species observation project parameters.
        /// </summary>
        public String Unit { get; set; }

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
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public String Value { get; set; }
    }
}

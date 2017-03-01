using System;
using System.Diagnostics.CodeAnalysis;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Data
{
    /// <summary>
    /// Harvest species observation field.
    /// </summary>
    public class HarvestSpeciesObservationField
    {
        /// <summary>
        /// Web species observation class.
        /// </summary>
        public WebSpeciesObservationClass Class { get; set; }

        /// <summary>
        /// Class index.
        /// </summary>
        public Int64 ClassIndex { get; set; }

        /// <summary>
        /// Web locale.
        /// </summary>
        public WebLocale Locale { get; set; }

        /// <summary>
        /// Information about the value.
        /// </summary>
        public String Information { get; set; }

        /// <summary>
        /// Specifies if property ClassIndex has a value or not.
        /// </summary>
        public Boolean IsClassIndexSpecified { get; set; }

        /// <summary>
        /// Specifies if property PropertyIndex has a value or not.
        /// </summary>
        public Boolean IsPropertyIndexSpecified { get; set; }

        /// <summary>
        /// Information about which species observation
        /// property that this value belongs to.
        /// </summary>
        public WebSpeciesObservationProperty Property { get; set; }

        /// <summary>
        /// Defines array index if property may have multiple values.
        /// </summary>
        public Int64 PropertyIndex { get; set; }

        /// <summary>
        /// Type of the data.
        /// </summary>
        public WebDataType Type { get; set; }

        /// <summary>
        /// Unit of the data.
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

        /// <summary>
        /// Is field in darwin core.
        /// </summary>
        public Boolean IsDarwinCore { get; set; }

        /// <summary>
        /// Is field searchable.
        /// </summary>
        public Boolean IsSearchable { get; set; }

        /// <summary>
        /// Is field mandatory.
        /// </summary>
        public Boolean IsMandatory { get; set; }

        /// <summary>
        /// Is field mandatory at provider.
        /// </summary>
        public Boolean IsMandatoryFromProvider { get; set; }

        /// <summary>
        /// Is field obtained from provider.
        /// </summary>
        public Boolean IsObtainedFromProvider { get; set; }

        /// <summary>
        /// In what database table is the field persisted.
        /// </summary>
        public String PersistedInTable { get; set; }
    }
}

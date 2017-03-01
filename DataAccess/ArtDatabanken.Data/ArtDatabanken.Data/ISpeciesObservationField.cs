using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface contains information on one piece of data related to a species observation. 
    /// Properties Class and Property specifies which value this field contains. 
    /// Property ClassIndex is used when one species observation can have values ​​for multiple instances 
    /// of species observation classes. Property PropertyIndex is used when one species observation class 
    /// can have multiple values for one species observation property. For example if information about 
    /// one observer is represented as a species observation class must property ClassIndex be used to 
    /// distinguish between different observers.
    /// </summary>
    public interface ISpeciesObservationField
    {
        /// <summary>
        /// Information about which species observation
        /// class that this value belongs to.
        /// </summary>
        ISpeciesObservationClass Class { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Data fields with any information related to the species observation field.
        /// </summary>
        DataFieldList DataFields { get; set; }

        /// <summary>
        /// Information about which species observation
        /// property that this value belongs to.
        /// </summary>
        ISpeciesObservationProperty Property { get; set; }

        /// <summary>
        /// Type of the data.
        /// </summary>
        DataType Type { get; set; }

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

        /// <summary>
        /// Get property Value converted to a Boolean.
        /// This method should only be used if value is of type Boolean.
        /// </summary>
        /// <returns>A Boolean value.</returns>
        Boolean GetBoolean();

        /// <summary>
        /// Get property Value converted to a DateTime.
        /// This method should only be used if value is of type DateTime.
        /// </summary>
        /// <returns>A DateTime value.</returns>
        DateTime GetDateTime();

        /// <summary>
        /// Get property Value converted to a Double.
        /// This method should only be used if value is of type Float64.
        /// </summary>
        /// <returns>A Double value.</returns>
        Double GetDouble();

        /// <summary>
        /// Get property Value converted to an Int32.
        /// This method should only be used if value is of type Int32.
        /// </summary>
        /// <returns>An Int32 value.</returns>
        Int32 GetInt32();

        /// <summary>
        /// Get property Value converted to an Int64.
        /// This method should only be used if value is of type Int64.
        /// </summary>
        /// <returns>An Int64 value.</returns>
        Int64 GetInt64();

        /// <summary>
        /// Get property Value converted to a String.
        /// This method should only be used if value is of type String.
        /// </summary>
        /// <returns>A String value.</returns>
        String GetString();
    }
}

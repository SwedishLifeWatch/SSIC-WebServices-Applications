using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about a field
    /// that is included in a species observation.
    /// </summary>
    public class SpeciesObservationField : ISpeciesObservationField
    {
        /// <summary>
        /// Information about which species observation
        /// class that this value belongs to.
        /// </summary>
        public ISpeciesObservationClass Class { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Data fields with any information related to the species observation field.
        /// </summary>
        public DataFieldList DataFields { get; set; }

        /// <summary>
        /// Information about which species observation
        /// property that this value belongs to.
        /// </summary>
        public ISpeciesObservationProperty Property { get; set; }

        /// <summary>
        /// Type of the data.
        /// </summary>
        public DataType Type { get; set; }

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
        public String Value { get; set; }

        /// <summary>
        /// Check that value is of the specified data type.
        /// </summary>
        /// <param name="dataType">Expected data type.</param>
        private void CheckDataType(DataType dataType)
        {
            if (Type != dataType)
            {
                throw new Exception("Wrong data type." +
                                    " Actual data type = " + Type +
                                    " Expected data type = " + dataType);
            }
        }

        /// <summary>
        /// Get property Value converted to a Boolean.
        /// This method should only be used if value is of type Boolean.
        /// </summary>
        /// <returns>A Boolean value.</returns>
        public Boolean GetBoolean()
        {
            CheckDataType(DataType.Boolean);
            return Value.WebParseBoolean();
        }

        /// <summary>
        /// Get property Value converted to a DateTime.
        /// This method should only be used if value is of type DateTime.
        /// </summary>
        /// <returns>A DateTime value.</returns>
        public DateTime GetDateTime()
        {
            CheckDataType(DataType.DateTime);
            return Value.WebParseDateTime();
        }

        /// <summary>
        /// Get property Value converted to a Double.
        /// This method should only be used if value is of type Float64.
        /// </summary>
        /// <returns>A Double value.</returns>
        public Double GetDouble()
        {
            CheckDataType(DataType.Float64);
            return Value.WebParseDouble();
        }

        /// <summary>
        /// Get property Value converted to an Int32.
        /// This method should only be used if value is of type Int32.
        /// </summary>
        /// <returns>An Int32 value.</returns>
        public Int32 GetInt32()
        {
            CheckDataType(DataType.Int32);
            return Value.WebParseInt32();
        }

        /// <summary>
        /// Get property Value converted to an Int64.
        /// This method should only be used if value is of type Int64.
        /// </summary>
        /// <returns>An Int64 value.</returns>
        public Int64 GetInt64()
        {
            CheckDataType(DataType.Int64);
            return Value.WebParseInt64();
        }

        /// <summary>
        /// Get property Value converted to a String.
        /// This method should only be used if value is of type String.
        /// </summary>
        /// <returns>A String value.</returns>
        public String GetString()
        {
            CheckDataType(DataType.String);
            return Value;
        }
    }
}

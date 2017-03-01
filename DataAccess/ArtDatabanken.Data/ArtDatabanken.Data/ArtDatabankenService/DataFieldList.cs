using System;
using System.Collections;
using System.Collections.Generic;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class handles an array of WebDataFields.
    /// It provides help to read the data.
    /// All methods and properties are read only.
    /// </summary>
    public class DataFieldList
    {
        private Hashtable _dataTable;

        /// <summary>
        /// Create a DataFieldList instance.
        /// </summary>
        /// <param name='dataFields'>Data fields that this instance will handle.</param>
        public DataFieldList(List<WebDataField> dataFields)
        {
            _dataTable = new Hashtable();
            if (dataFields.IsNotEmpty())
            {
                foreach (WebDataField dataField in dataFields)
                {
                    _dataTable.Add(dataField.Name, dataField);
                }
            }
        }

        /// <summary>
        /// Get a Boolean value from the data field with the specified name.
        /// </summary>
        /// <param name='fieldName'>Name of field to get value for.</param>
        /// <returns>An Boolean value</returns>
        public Boolean GetBoolean(String fieldName)
        {
            WebDataField dataField;

            dataField = GetDataField(fieldName);
            return dataField.Value.WebParseBoolean();
        }

        /// <summary>
        /// Get a DataField with the specified name.
        /// </summary>
        /// <param name='fieldName'>Name of field to get.</param>
        /// <exception cref="ArgumentException">Thrown if no field was found.</exception>
        private WebDataField GetDataField(String fieldName)
        {
            WebDataField dataField;

            dataField = (WebDataField)(_dataTable[fieldName]);
            if (dataField.IsNull())
            {
                // No field with specified name.
                throw new ArgumentException("No data field with name " + fieldName + "!");
            }

            return dataField;
        }

        /// <summary>
        /// Get a DateTime value from the data field with the specified name.
        /// </summary>
        /// <param name='fieldName'>Name of field to get value for.</param>
        /// <returns>An DateTime value</returns>
        public DateTime GetDateTime(String fieldName)
        {
            WebDataField dataField;

            dataField = GetDataField(fieldName);
            return dataField.Value.WebParseDateTime();
        }

        /// <summary>
        /// Get a Double value from the data field with the specified name.
        /// </summary>
        /// <param name='fieldName'>Name of field to get value for.</param>
        /// <returns>An Double value</returns>
        public Double GetDouble(String fieldName)
        {
            WebDataField dataField;

            dataField = GetDataField(fieldName);
            return dataField.Value.WebParseDouble();
        }

        /// <summary>
        /// Get an Int32 value from the data field with the specified name.
        /// </summary>
        /// <param name='fieldName'>Name of field to get value for.</param>
        /// <returns>An Int32 value</returns>
        public Int32 GetInt32(String fieldName)
        {
            WebDataField dataField;

            dataField = GetDataField(fieldName);
            return dataField.Value.WebParseInt32();
        }

        /// <summary>
        /// Get an Int64 value from the data field with the specified name.
        /// </summary>
        /// <param name='fieldName'>Name of field to get value for.</param>
        /// <returns>An Int64 value</returns>
        public Int64 GetInt64(String fieldName)
        {
            WebDataField dataField;

            dataField = GetDataField(fieldName);
            return dataField.Value.WebParseInt64();
        }

        /// <summary>
        /// Get an String value from the data field with the specified name.
        /// </summary>
        /// <param name='fieldName'>Name of field to get value for.</param>
        /// <returns>An String value</returns>
        public String GetString(String fieldName)
        {
            WebDataField dataField;

            dataField = GetDataField(fieldName);
            return dataField.Value;
        }

        /// <summary>
        /// Test if field with the specified name has a value.
        /// </summary>
        /// <param name='fieldName'>Name of field to get information about.</param>
        /// <returns>An String value</returns>
        public Boolean IsValueSpecified(String fieldName)
        {
            WebDataField dataField;

            dataField = GetDataField(fieldName);
            return dataField.IsValueSpecified;
        }
    }
}

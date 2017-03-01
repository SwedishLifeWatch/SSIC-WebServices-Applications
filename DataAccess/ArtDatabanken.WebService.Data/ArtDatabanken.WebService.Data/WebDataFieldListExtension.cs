using System;
using System.Collections.Generic;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension methods to the generic List 
    /// class with WebDataField elements.
    /// </summary>
    public static class WebDataFieldListExtension
    {
        /// <summary>
        /// Check if the WebDataField is of the expected type.
        /// </summary>
        /// <param name='dataField'>Web data field instance.</param>
        /// <param name='type'>Expected data type.</param>
        private static void CheckDataType(WebDataField dataField,
                                          WebDataType type)
        {
            if (dataField.Type != type)
            {
                throw new ArgumentException("Wrong data type in WebDataField " + dataField.Name + ". " +
                                            "Expected data type = " + type +
                                            ". Actual data type = " + dataField.Type + ".");
            }
        }

        /// <summary>
        /// Get a Boolean value from the data field with the specified name.
        /// </summary>
        /// <param name='dataFields'>The WebDataField list.</param>
        /// <param name='fieldName'>Name of field to get value for.</param>
        /// <returns>An Boolean value</returns>
        public static Boolean GetBoolean(this List<WebDataField> dataFields, 
                                         String fieldName)
        {
            WebDataField dataField;

            dataField = dataFields.GetDataField(fieldName);
            CheckDataType(dataField, WebDataType.Boolean);
            return dataField.Value.WebParseBoolean();
        }

        /// <summary>
        /// Get a DataField with the specified name.
        /// </summary>
        /// <param name='dataFields'>The WebDataField list.</param>
        /// <param name='fieldName'>Name of field to get.</param>
        /// <exception cref="ArgumentException">Thrown if no field was found.</exception>
        private static WebDataField GetDataField(this List<WebDataField> dataFields,
                                                 String fieldName)
        {
            return dataFields.GetDataField(fieldName, false, WebDataType.Boolean);
        }

        /// <summary>
        /// Get a DataField with the specified name.
        /// </summary>
        /// <param name='dataFields'>The WebDataField list.</param>
        /// <param name='fieldName'>Name of field to get.</param>
        /// <param name='create'>If true, DataField is created if it was not found.</param>
        /// <param name='dataType'>Data type of the field. This parameter is only used if a field is created.</param>
        /// <exception cref="ArgumentException">Thrown if no field was found.</exception>
        private static WebDataField GetDataField(this List<WebDataField> dataFields,
                                                 String fieldName,
                                                 Boolean create,
                                                 WebDataType dataType)
        {
            if (dataFields.IsNotEmpty())
            {
                foreach (WebDataField dataField in dataFields)
                {
                    if (dataField.Name == fieldName)
                    {
                        return dataField;
                    }
                }
            }

            if (create)
            {
                WebDataField dataField;

                dataField = new WebDataField();
                dataField.Name = fieldName;
                dataField.Type = dataType;
                dataFields.Add(dataField);
                return dataField;
            }
            else
            {
                // No field with specified name.
                throw new ArgumentException("No data field with name " + fieldName + "!");
            }
        }

        /// <summary>
        /// Get a DateTime value from the data field with the specified name.
        /// </summary>
        /// <param name='dataFields'>The WebDataField list.</param>
        /// <param name='fieldName'>Name of field to get value for.</param>
        /// <returns>An DateTime value</returns>
        public static DateTime GetDateTime(this List<WebDataField> dataFields, 
                                           String fieldName)
        {
            WebDataField dataField;

            dataField = dataFields.GetDataField(fieldName);
            CheckDataType(dataField, WebDataType.DateTime);
            return dataField.Value.WebParseDateTime();
        }
        
        /// <summary>
        /// Get a Double value from the data field with the specified name.
        /// </summary>
        /// <param name='dataFields'>The WebDataField list.</param>
        /// <param name='fieldName'>Name of field to get value for.</param>
        /// <returns>An Double value</returns>
        public static Double GetFloat64(this List<WebDataField> dataFields,
                                        String fieldName)
        {
            WebDataField dataField;

            dataField = dataFields.GetDataField(fieldName);
            CheckDataType(dataField, WebDataType.Float64);
            return dataField.Value.WebParseDouble();
        }
                
        /// <summary>
        /// Get an Int32 value from the data field with the specified name.
        /// </summary>
        /// <param name='dataFields'>The WebDataField list.</param>
        /// <param name='fieldName'>Name of field to get value for.</param>
        /// <returns>An Int32 value</returns>
        public static Int32 GetInt32(this List<WebDataField> dataFields,
                                     String fieldName)
        {
            WebDataField dataField;

            dataField = dataFields.GetDataField(fieldName);
            CheckDataType(dataField, WebDataType.Int32);
            return dataField.Value.WebParseInt32();
        }
        
        /// <summary>
        /// Get an Int64 value from the data field with the specified name.
        /// </summary>
        /// <param name='dataFields'>The WebDataField list.</param>
        /// <param name='fieldName'>Name of field to get value for.</param>
        /// <returns>An Int64 value</returns>
        public static Int64 GetInt64(this List<WebDataField> dataFields,
                                     String fieldName)
        {
            WebDataField dataField;

            dataField = dataFields.GetDataField(fieldName);
            CheckDataType(dataField, WebDataType.Int64);
            return dataField.Value.WebParseInt64();
        }
        
        /// <summary>
        /// Get an String value from the data field with the specified name.
        /// </summary>
        /// <param name='dataFields'>The WebDataField list.</param>
        /// <param name='fieldName'>Name of field to get value for.</param>
        /// <returns>An String value</returns>
        public static String GetString(this List<WebDataField> dataFields,
                                       String fieldName)
        {
            WebDataField dataField;

            dataField = dataFields.GetDataField(fieldName);
            CheckDataType(dataField, WebDataType.String);
            return dataField.Value;
        }

        /// <summary>
        /// Test if a DataField with the specified name belongs to the list.
        /// </summary>
        /// <param name='dataFields'>The WebDataField list.</param>
        /// <param name='fieldName'>Name of field to check.</param>
        public static Boolean IsDataFieldSpecified(this List<WebDataField> dataFields,
                                                   String fieldName)
        {
            if (dataFields.IsNotEmpty())
            {
                foreach (WebDataField dataField in dataFields)
                {
                    if (dataField.Name == fieldName)
                    {
                        return true;
                    }
                }
            }

            // No field with specified name.
            return false;
        }

        /// <summary>
        /// Set a Boolean value in the data field with the specified name.
        /// </summary>
        /// <param name='dataFields'>The WebDataField list.</param>
        /// <param name='fieldName'>Name of field to update value for.</param>
        /// <param name='value'>New value for the field.</param>
        public static void SetBoolean(this List<WebDataField> dataFields,
                                      String fieldName,
                                      Boolean value)
        {
            WebDataField dataField;

            dataField = dataFields.GetDataField(fieldName, true, WebDataType.Boolean);
            CheckDataType(dataField, WebDataType.Boolean);
            dataField.Value = value.WebToString();
        }

        /// <summary>
        /// Set a DateTime value in the data field with the specified name.
        /// </summary>
        /// <param name='dataFields'>The WebDataField list.</param>
        /// <param name='fieldName'>Name of field to update value for.</param>
        /// <param name='value'>New value for the field.</param>
        public static void SetDateTime(this List<WebDataField> dataFields,
                                       String fieldName,
                                       DateTime value)
        {
            WebDataField dataField;

            dataField = dataFields.GetDataField(fieldName, true, WebDataType.DateTime);
            CheckDataType(dataField, WebDataType.DateTime);
            dataField.Value = value.WebToString();
        }

        /// <summary>
        /// Set a Float64 value in the data field with the specified name.
        /// </summary>
        /// <param name='dataFields'>The WebDataField list.</param>
        /// <param name='fieldName'>Name of field to update value for.</param>
        /// <param name='value'>New value for the field.</param>
        public static void SetFloat64(this List<WebDataField> dataFields,
                                      String fieldName,
                                      Double value)
        {
            WebDataField dataField;

            dataField = dataFields.GetDataField(fieldName, true, WebDataType.Float64);
            CheckDataType(dataField, WebDataType.Float64);
            dataField.Value = value.WebToString();
        }

        /// <summary>
        /// Set an Int32 value in the data field with the specified name.
        /// </summary>
        /// <param name='dataFields'>The WebDataField list.</param>
        /// <param name='fieldName'>Name of field to update value for.</param>
        /// <param name='value'>New value for the field.</param>
        public static void SetInt32(this List<WebDataField> dataFields,
                                    String fieldName,
                                    Int32 value)
        {
            WebDataField dataField;

            dataField = dataFields.GetDataField(fieldName, true, WebDataType.Int32);
            CheckDataType(dataField, WebDataType.Int32);
            dataField.Value = value.WebToString();
        }

        /// <summary>
        /// Set an Int64 value in the data field with the specified name.
        /// </summary>
        /// <param name='dataFields'>The WebDataField list.</param>
        /// <param name='fieldName'>Name of field to update value for.</param>
        /// <param name='value'>New value for the field.</param>
        public static void SetInt64(this List<WebDataField> dataFields,
                                    String fieldName,
                                    Int64 value)
        {
            WebDataField dataField;

            dataField = dataFields.GetDataField(fieldName, true, WebDataType.Int64);
            CheckDataType(dataField, WebDataType.Int64);
            dataField.Value = value.WebToString();
        }

        /// <summary>
        /// Set a String value in the data field with the specified name.
        /// </summary>
        /// <param name='dataFields'>The WebDataField list.</param>
        /// <param name='fieldName'>Name of field to update value for.</param>
        /// <param name='value'>New value for the field.</param>
        public static void SetString(this List<WebDataField> dataFields,
                                     String fieldName,
                                     String value)
        {
            WebDataField dataField;

            dataField = dataFields.GetDataField(fieldName, true, WebDataType.String);
            CheckDataType(dataField, WebDataType.String);
            dataField.Value = value;
        }
    }
}

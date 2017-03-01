using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Text;
using ArtDatabanken.Database;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension methods to the WebDataField class.
    /// </summary>
    public static class WebDataFieldExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name='dataField'>Data field object.</param>
        public static void CheckData(this WebDataField dataField)
        {
            dataField.Name = dataField.Name.CheckInjection();
            dataField.Unit = dataField.Unit.CheckInjection();
            dataField.Value = dataField.Value.CheckInjection();
        }

        /// <summary>
        /// Check if the WebDataField is of the expected type.
        /// </summary>
        /// <param name='dataField'>Data field object.</param>
        /// <param name='type'>Expected data type.</param>
        private static void CheckDataType(this WebDataField dataField,
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
        /// Get a Boolean value from the data field.
        /// </summary>
        /// <param name='dataField'>Data field object.</param>
        /// <returns>An Boolean value</returns>
        public static Boolean GetBoolean(this WebDataField dataField)
        {
            dataField.CheckDataType(WebDataType.Boolean);
            return dataField.Value.WebParseBoolean();
        }

        /// <summary>
        /// Get a DateTime value from the data field.
        /// </summary>
        /// <param name='dataField'>Data field object.</param>
        /// <returns>An DateTime value</returns>
        public static DateTime GetDateTime(this WebDataField dataField)
        {
            dataField.CheckDataType(WebDataType.DateTime);
            return dataField.Value.WebParseDateTime();
        }

        /// <summary>
        /// Get a Double value from the data field.
        /// </summary>
        /// <param name='dataField'>Data field object.</param>
        /// <returns>An Double value</returns>
        public static Double GetFloat64(this WebDataField dataField)
        {
            dataField.CheckDataType(WebDataType.Float64);
            return dataField.Value.WebParseDouble();
        }

        /// <summary>
        /// Get an Int32 value from the data field.
        /// </summary>
        /// <param name='dataField'>Data field object.</param>
        /// <returns>An Int32 value</returns>
        public static Int32 GetInt32(this WebDataField dataField)
        {
            dataField.CheckDataType(WebDataType.Int32);
            return dataField.Value.WebParseInt32();
        }

        /// <summary>
        /// Get an Int64 value from the data field.
        /// </summary>
        /// <param name='dataField'>Data field object.</param>
        /// <returns>An Int64 value</returns>
        public static Int64 GetInt64(this WebDataField dataField)
        {
            dataField.CheckDataType(WebDataType.Int64);
            return dataField.Value.WebParseInt64();
        }

        /// <summary>
        /// Get an String value from the data field with the specified name.
        /// </summary>
        /// <param name='dataField'>Data field object.</param>
        /// <returns>An String value</returns>
        public static String GetString(this WebDataField dataField)
        {
            dataField.CheckDataType(WebDataType.String);
            return dataField.Value;
        }

        /// <summary>
        /// Load data into WebDataField.
        /// </summary>
        /// <param name='dataField'>Data field object.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebDataField dataField,
                                    DataReader dataReader)
        {
            String type;

            dataField.Name = dataReader.GetUnreadColumnName();
            dataField.Value = null;
            type = dataReader.GetUnreadColumnType().ToString();
            switch (type)
            {
                case "System.Boolean":
                    dataField.Type = WebDataType.Boolean;
                    if (dataReader.IsNotDbNull(dataField.Name))
                    {
                        dataField.Value = dataReader.GetBoolean(dataField.Name).WebToString();
                    }
                    break;
                case "System.DateTime":
                    dataField.Type = WebDataType.DateTime;
                    if (dataReader.IsNotDbNull(dataField.Name))
                    {
                        dataField.Value = dataReader.GetDateTime(dataField.Name).WebToString();
                    }
                    break;
                case "System.Double":
                case "System.Single":
                    dataField.Type = WebDataType.Float64;
                    if (dataReader.IsNotDbNull(dataField.Name))
                    {
                        dataField.Value = dataReader.GetDouble(dataField.Name).WebToString();
                    }
                    break;
                case "System.Byte":
                case "System.SByte":
                case "System.Int16":
                case "System.UInt16":
                    dataField.Type = WebDataType.Int32;
                    if (dataReader.IsNotDbNull(dataField.Name))
                    {
                        dataField.Value = dataReader.GetInt16(dataField.Name).WebToString();
                    }
                    break;
                case "System.Int32":
                case "System.UInt32":
                    dataField.Type = WebDataType.Int32;
                    if (dataReader.IsNotDbNull(dataField.Name))
                    {
                        dataField.Value = dataReader.GetInt32(dataField.Name).WebToString();
                    }
                    break;
                case "System.Int64":
                case "System.UInt64":
                    dataField.Type = WebDataType.Int64;
                    if (dataReader.IsNotDbNull(dataField.Name))
                    {
                        dataField.Value = dataReader.GetInt64(dataField.Name).WebToString();
                    }
                    break;
                default:
                    dataField.Type = WebDataType.String;
                    if (dataReader.IsNotDbNull(dataField.Name))
                    {
                        dataField.Value = dataReader.GetString(dataField.Name);
                    }
                    break;
            }
        }

        /// <summary>
        /// Get web data field as string.
        /// </summary>
        /// <param name="dataField">Data field.</param>
        /// <returns>Web data field as string.</returns>
        public static String WebToString(this WebDataField dataField)
        {
            StringBuilder stringBuilder;
            if (dataField.IsNull())
            {
                return String.Empty;
            }
            else
            {
                stringBuilder = new StringBuilder();
                stringBuilder.Append("Data field:");
                if (dataField.Information.IsNotEmpty())
                {
                    stringBuilder.Append(" Information = [" + dataField.Information + "],");
                }

                stringBuilder.Append(" Name = " + dataField.Name);
                stringBuilder.Append(", Type = " + dataField.Type);
                if (dataField.Unit.IsNotEmpty())
                {
                    stringBuilder.Append(", Unit = [" + dataField.Unit + "]");
                }

                stringBuilder.Append(", Value = [" + dataField.Value + "]");
                return stringBuilder.ToString();
            }
        }
    }
}

using System;
using System.Runtime.Serialization;
using ArtDatabanken;
using ArtDatabanken.WebService.ArtDatabankenService;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Definition of data types that can be contained
    /// in a WebDataField instance.
    /// </summary>
    [DataContract]
    public enum WebDataType
    {
        /// <summary>A Boolean value.</summary>
        [EnumMember]
        Boolean,
        /// <summary>A DateTime value.</summary>
        [EnumMember]
        DateTime,
        /// <summary>A Float value. Standard 64-bit floating point.</summary>
        [EnumMember]
        Float,
        /// <summary>A Int32 value.</summary>
        [EnumMember]
        Int32,
        /// <summary>A Int64 value.</summary>
        [EnumMember]
        Int64,
        /// <summary>A String value.</summary>
        [EnumMember]
        String
    }

    /// <summary>
    /// Generic holder of one piece of data.
    /// </summary>
    [DataContract]
    public class WebDataField
    {
        private Boolean _isValueSpecified; // Not used dummy value.

        /// <summary>
        /// Create a WebDataField instance.
        /// </summary>
        public WebDataField()
        {
            Name = null;
            Type = WebDataType.String;
            Value = null;
        }

        /// <summary>
        /// Create a WebDataField instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebDataField(DataReader dataReader)
        {
            String type;

            Name = dataReader.GetUnreadColumnName();
            Value = null;
            type = dataReader.GetUnreadColumnType().ToString();
            switch (type)
            {
                case "System.Boolean":
                    Type = WebDataType.Boolean;
                    if (dataReader.IsNotDBNull(Name))
                    {
                        Value = dataReader.GetBoolean(Name).WebToString();
                    }
                    break;
                case "System.DateTime":
                    Type = WebDataType.DateTime;
                    if (dataReader.IsNotDBNull(Name))
                    {
                        Value = dataReader.GetDateTime(Name).WebToString();
                    }
                    break;
                case "System.Double":
                case "System.Single":
                    Type = WebDataType.Float;
                    if (dataReader.IsNotDBNull(Name))
                    {
                        Value = dataReader.GetDouble(Name).WebToString();
                    }
                    break;
                case "System.Byte":
                case "System.SByte":
                case "System.Int16":
                case "System.UInt16":
                case "System.Int32":
                case "System.UInt32":
                    Type = WebDataType.Int32;
                    if (dataReader.IsNotDBNull(Name))
                    {
                        Value = dataReader.GetInt32(Name).WebToString();
                    }
                    break;
                case "System.Int64":
                case "System.UInt64":
                    Type = WebDataType.Int64;
                    if (dataReader.IsNotDBNull(Name))
                    {
                        Value = dataReader.GetInt64(Name).WebToString();
                    }
                    break;
                default:
                    Type = WebDataType.String;
                    if (dataReader.IsNotDBNull(Name))
                    {
                        Value = dataReader.GetString(Name);
                    }
                    break;
            }
        }

        /// <summary>Test if DataField contains a value.</summary>
        [DataMember]
        public Boolean IsValueSpecified
        {
            get { return Value.IsNotNull(); }
            set { _isValueSpecified = value; }
        }

        /// <summary>Name of the data.</summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>Type of the data.</summary>
        [DataMember]
        public WebDataType Type
        { get; set; }

        /// <summary>Value of the data.</summary>
        [DataMember]
        public String Value
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public void CheckData()
        {
            Name = Name.CheckSqlInjection();
            Value = Value.CheckSqlInjection();
        }
    }
}

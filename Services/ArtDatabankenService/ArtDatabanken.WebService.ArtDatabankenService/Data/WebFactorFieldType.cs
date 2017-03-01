using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    ///  This class represents a factor field type.
    /// </summary>
    [DataContract]
    public class WebFactorFieldType : WebData
    {
        /// <summary>
        /// Create a WebFactorFieldType instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebFactorFieldType(DataReader dataReader)
        {
            Id = dataReader.GetInt32(FactorFieldTypeData.ID);
            Name = dataReader.GetString(FactorFieldTypeData.NAME);
            Definition = dataReader.GetString(FactorFieldTypeData.DEFINITION);
            base.LoadData(dataReader);
        }

        /// <summary>
        /// Get data type for this factor field type.
        /// </summary>
        public WebDataType DataType
        {
            get
            {
                return GetDataType(Id);
            }
        }

        /// <summary>
        /// Defintion for this factor field type.
        /// </summary>
        [DataMember]
        public String Definition
        { get; set; }

        /// <summary>
        /// Id for this factor field type.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Name for this factor field type.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Get data type given a factor field type id.
        /// </summary>
        /// <param name='factorFieldTypeId'>Factor field type id.</param>
        /// <returns>Data type.</returns>
        public static WebDataType GetDataType(Int32 factorFieldTypeId)
        {
            switch (factorFieldTypeId)
            {
                case 0:
                    return WebDataType.Boolean;
                case 1:
                    return WebDataType.Int32;
                case 2:
                    return WebDataType.String;
                case 3:
                    return WebDataType.Int32;
                case 4:
                    return WebDataType.Float;
                default:
                    throw new ApplicationException("Unknown factor field type, ID = " + factorFieldTypeId);
            }
        }
    }
}

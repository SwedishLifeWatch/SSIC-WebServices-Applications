using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    ///  This class represents a factor data type.
    /// </summary>
    [DataContract]
    public class WebFactorDataType : WebData
    {
        /// <summary>
        /// Create a WebFactorDataType instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebFactorDataType(DataReader dataReader)
        {
            Definition = dataReader.GetString(FactorDataTypeData.DEFINITION);
            Fields = new List<WebFactorField>();
            Id = dataReader.GetInt32(FactorDataTypeData.ID);
            Name = dataReader.GetString(FactorDataTypeData.NAME);
        }

        /// <summary>
        /// Defintion for this factor data type.
        /// </summary>
        [DataMember]
        public String Definition
        { get; set; }

        /// <summary>
        /// Fields this factor data type.
        /// </summary>
        [DataMember]
        public List<WebFactorField> Fields
        { get; set; }

        /// <summary>
        /// Id for this factor data type.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Name for this factor data type.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }
    }
}

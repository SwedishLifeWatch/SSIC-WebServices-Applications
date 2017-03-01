using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    ///  This class represents a factor field enum.
    /// </summary>
    [DataContract]
    public class WebFactorFieldEnum : WebData
    {
        /// <summary>
        /// Create a WebFactorFieldEnum instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebFactorFieldEnum(DataReader dataReader)
        {
            Id = dataReader.GetInt32(FactorFieldEnumData.ID);
            Values = new List<WebFactorFieldEnumValue>();
        }

        /// <summary>
        /// Id for this factor field enum.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Factor field enum values for this enum
        /// </summary>
        [DataMember]
        public List<WebFactorFieldEnumValue> Values
        { get; set; }
    }
}

using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// This class represents an origin.
    /// </summary>
    [DataContract]
    public class WebFactorOrigin : WebData
    {
        /// <summary>
        /// Create an instance of origin (table af_ursprung).
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebFactorOrigin(DataReader dataReader)
        {
            Id = dataReader.GetInt32(FactorOriginData.ID);
            Name = dataReader.GetString(FactorOriginData.NAME);
            Definition = dataReader.GetString(FactorOriginData.DEFINITION);
            SortOrder = dataReader.GetInt32(FactorOriginData.SORTORDER);
        }

        /// <summary>
        /// Definition for this origin.
        /// </summary>
        [DataMember]
        public string Definition
        { get; set; }

        /// <summary>
        /// Id for this origin.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Name for this origin.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Date for last allowed update for this origin.
        /// </summary>
        [DataMember]
        public Int32 SortOrder
        { get; set; }
    }
}

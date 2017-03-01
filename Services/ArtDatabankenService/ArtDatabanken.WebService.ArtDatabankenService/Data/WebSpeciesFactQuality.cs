using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    ///  This class represents a category of species fact quality.
    /// </summary>
    [DataContract]
    public class WebSpeciesFactQuality : WebData
    {
        /// <summary>
        /// Create a WebSpeciesFactQuality instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebSpeciesFactQuality(DataReader dataReader)
        {
            Id = dataReader.GetInt32(SpeciesFactQualityData.ID);
            Name = dataReader.GetString(SpeciesFactQualityData.NAME);
            Definition = dataReader.GetString(SpeciesFactQualityData.DEFINITION);
            base.LoadData(dataReader);
        }

        /// <summary>
        /// Definition for this category of species fact quality.
        /// </summary>
        [DataMember]
        public String Definition
        { get; set; }

        /// <summary>
        /// Id for this category of species fact quality.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Name for this category of species fact quality.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }
    }
}

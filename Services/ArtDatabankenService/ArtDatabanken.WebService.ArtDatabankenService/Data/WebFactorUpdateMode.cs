using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Is used to decide which information that should be
    /// returned together with the taxon information.
    /// </summary>
    [DataContract]
    public enum FactorUpdateModeType
    {
        /// <summary>
        /// Can not be changed.
        /// </summary>
        [EnumMember]
        Archive,
        /// <summary>
        /// Is changed by automatic calculations.
        /// </summary>
        [EnumMember]
        AutomaticUpdate,
        /// <summary>
        /// Factor without data. Used as header.
        /// </summary>
        [EnumMember]
        Header,
        /// <summary>
        /// Can be changed by user anytime.
        /// </summary>
        [EnumMember]
        ManualUpdate
    }

    /// <summary>
    ///  This class represents a factor update mode.
    /// </summary>
    [DataContract]
    public class WebFactorUpdateMode : WebData
    {
        /// <summary>
        /// Create a WebFactorUpdateMode instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebFactorUpdateMode(DataReader dataReader)
        {
            Id = dataReader.GetInt32(FactorUpdateModeData.ID);
            Name = dataReader.GetString(FactorUpdateModeData.NAME);
            Type = (FactorUpdateModeType)(Enum.Parse(typeof(FactorUpdateModeType), dataReader.GetString(FactorUpdateModeData.TYPE)));
            Definition = dataReader.GetString(FactorUpdateModeData.DEFINITION);
            IsUpdateAllowed = dataReader.GetBoolean(FactorUpdateModeData.ALLOW_UPDATE);
            base.LoadData(dataReader);
        }

        /// <summary>
        /// Definition for this factor update mode.
        /// </summary>
        [DataMember]
        public String Definition
        { get; set; }

        /// <summary>
        /// Id for this factor update mode.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Indicates if factor may be updated.
        /// </summary>
        [DataMember]
        public Boolean IsUpdateAllowed
        { get; set; }

        /// <summary>
        /// Name for this factor update mode.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Type for this factor update mode.
        /// </summary>
        [DataMember]
        public FactorUpdateModeType Type
        { get; set; }
    }
}


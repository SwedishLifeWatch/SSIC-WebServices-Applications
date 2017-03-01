using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// This class represents a province.
    /// </summary>
    [DataContract]
    public class WebProvince : WebData
    {
        private Boolean _dummyIsProvincePart;

        /// <summary>
        /// Create a WebProvince instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebProvince(DataReader dataReader)
        {
            Id = dataReader.GetInt32(ProvinceData.ID);
            Identifier = dataReader.GetString(ProvinceData.IDENTIFIER);
            Name = dataReader.GetString(ProvinceData.NAME);
            PartOfProvinceId = dataReader.GetInt32(ProvinceData.PART_OF_PROVINCE_ID, NO_ID);
            base.LoadData(dataReader);
        }

        /// <summary>
        /// Id for this province.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Identifier for this province.
        /// </summary>
        [DataMember]
        public String Identifier
        { get; set; }

        /// <summary>
        /// Test if this is only a part of a province.
        /// </summary>
        [DataMember]
        public Boolean IsProvincePart
        {
            get { return PartOfProvinceId != NO_ID; }
            set { _dummyIsProvincePart = value; }
        }

        /// <summary>
        /// Name for this province.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Id for province that this province is part of.
        /// </summary>
        [DataMember]
        public Int32 PartOfProvinceId
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public override void CheckData()
        {
            base.CheckData();
            Identifier = Identifier.CheckSqlInjection();
            Name = Name.CheckSqlInjection();
        }
    }
}

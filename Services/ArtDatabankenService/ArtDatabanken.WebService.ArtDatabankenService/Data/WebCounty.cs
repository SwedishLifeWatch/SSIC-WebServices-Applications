using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// This class represents a county.
    /// </summary>
    [DataContract]
    public class WebCounty : WebData
    {
        private Boolean _dummyIsCountyPart;

        /// <summary>
        /// Create a WebCounty instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebCounty(DataReader dataReader)
        {
            Id = dataReader.GetInt32(CountyData.ID);
            Identifier = dataReader.GetString(CountyData.IDENTIFIER);
            Name = dataReader.GetString(CountyData.NAME);
            IsNumberSpecified = dataReader.IsNotDBNull(CountyData.NUMBER);
            Number = dataReader.GetInt32(CountyData.NUMBER, Int32.MinValue);
            PartOfCountyId = dataReader.GetInt32(CountyData.PART_OF_COUNTY_ID, NO_ID);
            base.LoadData(dataReader);
        }

        /// <summary>
        /// Id for this county.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Identifier for this county.
        /// </summary>
        [DataMember]
        public String Identifier
        { get; set; }

        /// <summary>
        /// Test if this is only a part of a county.
        /// </summary>
        [DataMember]
        public Boolean IsCountyPart
        {
            get { return PartOfCountyId != NO_ID; }
            set { _dummyIsCountyPart = value; }
        }

        /// <summary>
        /// Test if this county has a number.
        /// </summary>
        [DataMember]
        public Boolean IsNumberSpecified
        { get; set; }

        /// <summary>
        /// Name for this county.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Number for this county.
        /// </summary>
        [DataMember]
        public Int32 Number
        { get; set; }

        /// <summary>
        /// Id for county that this county is part of.
        /// </summary>
        [DataMember]
        public Int32 PartOfCountyId
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

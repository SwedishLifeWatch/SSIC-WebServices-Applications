using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// This class represents a period type.
    /// </summary>
    [DataContract]
    public class WebPeriodType : WebData
    {
        /// <summary>
        /// Create an instance of period type.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebPeriodType(DataReader dataReader)
        {
            Id = dataReader.GetInt32(PeriodTypeData.ID);
            Name = dataReader.GetString(PeriodTypeData.NAME);
            Description = dataReader.GetString(PeriodTypeData.DESCRIPTION);

            base.LoadData(dataReader);
        }

        /// <summary>
        /// Description for this period type.
        /// </summary>
        [DataMember]
        public String Description
        { get; set; }

        /// <summary>
        /// Id for this period type.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Name for this period type.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public override void CheckData()
        {
            base.CheckData();
            Description = Description.CheckSqlInjection();
            Name = Name.CheckSqlInjection();
        }
    }
}

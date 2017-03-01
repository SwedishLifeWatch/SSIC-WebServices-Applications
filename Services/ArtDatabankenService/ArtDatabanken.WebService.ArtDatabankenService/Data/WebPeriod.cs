using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Holds information about how user selected periods should be used.
    /// </summary>
    public enum UserSelectedPeriodUsage
    {
        /// <summary>
        /// Periods are used as input to stored procedures.
        /// </summary>
        Input,
        /// <summary>
        /// Periods are used when producing output from stored procedures.
        /// </summary>
        Output
    }

    /// <summary>
    /// This class represents a period.
    /// </summary>
    [DataContract]
    public class WebPeriod : WebData
    {
        /// <summary>
        /// Create an instance of period.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebPeriod(DataReader dataReader)
        {
            Id = dataReader.GetInt32(PeriodData.ID);
            Name = dataReader.GetString(PeriodData.NAME);
            Information = dataReader.GetString(PeriodData.INFORMATION);
            PeriodTypeId = dataReader.GetInt32(PeriodData.PERIOD_TYPE_ID);
            StopUpdate = dataReader.GetDateTime(PeriodData.STOP_UPDATE);
            Year = dataReader.GetInt32(PeriodData.YEAR);
            base.LoadData(dataReader);
        }

        /// <summary>
        /// Id for this period.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Information for this period.
        /// </summary>
        [DataMember]
        public String Information
        { get; set; }

        /// <summary>
        /// Name for this period.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Period Type Id for this period.
        /// </summary>
        [DataMember]
        public Int32 PeriodTypeId
        { get; set; }

        /// <summary>
        /// Date for last allowed update for this period.
        /// </summary>
        [DataMember]
        public DateTime StopUpdate
        { get; set; }

        /// <summary>
        /// Year representing this period.
        /// </summary>
        [DataMember]
        public Int32 Year
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public override void CheckData()
        {
            base.CheckData();
            Information = Information.CheckSqlInjection();
            Name = Name.CheckSqlInjection();
        }
    }
}

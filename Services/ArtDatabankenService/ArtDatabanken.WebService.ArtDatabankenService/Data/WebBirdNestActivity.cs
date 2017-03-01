using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    ///  This class represents information about a bird nest activity.
    /// </summary>
    [DataContract]
    public class WebBirdNestActivity : WebData
    {
        /// <summary>
        /// Create a WebBirdNestActivity instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebBirdNestActivity(DataReader dataReader)
        {
            Id = dataReader.GetInt32(BirdNestActivityData.ID);
            Name = dataReader.GetString(BirdNestActivityData.NAME);
            base.LoadData(dataReader);
        }

        /// <summary>
        /// Id for this bird nest activity.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Name for this bird nest activity.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public void CheckData(WebServiceContext context)
        {
            base.CheckData();
            Name = Name.CheckSqlInjection();
            Name.CheckLength(GetNameMaxLength(context));
        }

        /// <summary>
        /// Get max string length for name.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Max string length for name.</returns>
        public static Int32 GetNameMaxLength(WebServiceContext context)
        {
            return DataServer.GetColumnLength(context, DataServer.DatabaseId.SpeciesFact, BirdNestActivityData.TABLE_NAME, BirdNestActivityData.NAME_COLUMN);
        }
    }
}

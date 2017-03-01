using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    ///  This class contains information about database update.
    /// </summary>
    [DataContract]
    public class WebDatabaseUpdate : WebData
    {
        /// <summary>
        /// Create a WebDatabaseUpdate instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebDatabaseUpdate(DataReader dataReader)
        {
            UpdateEnd = dataReader.GetDateTime(DatabaseData.UPDATE_END);
            UpdateStart = dataReader.GetDateTime(DatabaseData.UPDATE_START);
        }

        /// <summary>
        /// End time for database update.
        /// Only hour, minute and second are significant.
        /// </summary>
        [DataMember]
        public DateTime UpdateEnd
        { get; set; }

        /// <summary>
        /// Start time for database update.
        /// Only hour, minute and second are significant.
        /// </summary>
        [DataMember]
        public DateTime UpdateStart
        { get; set; }
    }
}

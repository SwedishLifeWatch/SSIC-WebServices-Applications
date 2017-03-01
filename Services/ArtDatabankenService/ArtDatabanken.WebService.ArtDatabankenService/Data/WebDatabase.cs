using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    ///  This class represents information about a database.
    /// </summary>
    [DataContract]
    public class WebDatabase : WebData
    {
        /// <summary>
        /// Create a WebDatabase instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebDatabase(DataReader dataReader)
        {
            Id = dataReader.GetInt32(DatabaseData.ID);
            LongName = dataReader.GetString(DatabaseData.LONG_NAME);
            ShortName = dataReader.GetString(DatabaseData.SHORT_NAME);
            base.LoadData(dataReader);
        }

        /// <summary>
        /// Id for this database.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Long name for this database.
        /// </summary>
        [DataMember]
        public String LongName
        { get; set; }

        /// <summary>
        /// Short name for this database.
        /// </summary>
        [DataMember]
        public String ShortName
        { get; set; }
    }
}

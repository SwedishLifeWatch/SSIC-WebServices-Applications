using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// This class represents an endangered list.
    /// </summary>
    [DataContract]
    public class WebEndangeredList : WebData
    {
        /// <summary>
        /// Create a WebEndangeredList instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebEndangeredList(DataReader dataReader)
        {
            Id = dataReader.GetInt32(EndangeredListData.ID);
            Name = dataReader.GetString(EndangeredListData.NAME);
            base.LoadData(dataReader);
        }

        /// <summary>
        /// Id for this endangered list.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Name for this endangered list.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }
    }
}

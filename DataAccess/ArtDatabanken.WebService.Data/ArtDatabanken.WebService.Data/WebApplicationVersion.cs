using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents an application version.
    /// </summary>
    [DataContract]
    public class WebApplicationVersion : WebData
    {
        /// <summary>
        /// Application id.
        /// </summary>
        [DataMember]
        public Int32 ApplicationId
        { get; set; }

        /// <summary>
        /// UserId that created the record
        /// Set by database when inserted.
        /// </summary>
        [DataMember]
        public Int32 CreatedBy
        { get; set; }

        /// <summary>
        /// Date record was created.
        /// </summary>
        [DataMember]
        public DateTime CreatedDate
        { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        [DataMember]
        public String Description
        { get; set; }

        /// <summary>
        /// Id.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Is recommended.
        /// </summary>
        [DataMember]
        public Boolean IsRecommended
        { get; set; }

        /// <summary>
        /// Is valid.
        /// </summary>
        [DataMember]
        public Boolean IsValid
        { get; set; }

        /// <summary>
        /// UserId that modified the record
        /// Set by database when record is modified.
        /// </summary>
        [DataMember]
        public Int32 ModifiedBy
        { get; set; }

        /// <summary>
        /// Date record was last modified.
        /// </summary>
        [DataMember]
        public DateTime ModifiedDate
        { get; set; }

        /// <summary>
        /// Date valid from.
        /// </summary>
        [DataMember]
        public DateTime ValidFromDate
        { get; set; }

        /// <summary>
        /// Date valid to.
        /// </summary>
        [DataMember]
        public DateTime ValidToDate
        { get; set; }

        /// <summary>
        /// Version.
        /// </summary>
        [DataMember]
        public String Version
        { get; set; }
    }
}

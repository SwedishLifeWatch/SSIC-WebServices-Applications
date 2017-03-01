using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents an application action
    /// </summary>
    [DataContract]
    public class WebApplicationAction : WebData
    {
        /// <summary>
        /// Administration role id.
        /// </summary>
        [DataMember]
        public Int32 AdministrationRoleId
        { get; set; }

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
        /// GUID.
        /// </summary>
        [DataMember]
        public String GUID
        { get; set; }

        /// <summary>
        /// Id for this action.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Action identifier.
        /// </summary>
        [DataMember]
        public String Identifier
        { get; set; }

        /// <summary>
        /// Is administration role id specified.
        /// </summary>
        [DataMember]
        public Boolean IsAdministrationRoleIdSpecified
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
        /// Application action name.
        /// </summary>
        [DataMember]
        public String Name
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
    }
}

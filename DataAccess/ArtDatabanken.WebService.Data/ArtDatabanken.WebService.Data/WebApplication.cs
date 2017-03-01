using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents an Application
    /// </summary>
    [DataContract]
    public class WebApplication : WebData
    {
        /// <summary>
        /// This applications actions.
        /// </summary>
        [DataMember]
        public List<WebApplicationAction> Actions
        { get; set; }

        /// <summary>
        /// Administration role id.
        /// </summary>
        [DataMember]
        public Int32 AdministrationRoleId
        { get; set; }

        /// <summary>
        /// Id - Contact person.
        /// </summary>
        [DataMember]
        public Int32 ContactPersonId
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
        /// Id for this application.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Application identifier.
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
        /// Is contact person id specified.
        /// </summary>
        [DataMember]
        public Boolean IsContactPersonIdSpecified
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
        /// Application name.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Application short name.
        /// </summary>
        [DataMember]
        public String ShortName
        { get; set; }

        /// <summary>
        /// URL.
        /// </summary>
        [DataMember]
        public String URL
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
        /// This applications versions.
        /// </summary>
        [DataMember]
        public List<WebApplicationVersion> Versions
        { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents a role.
    /// </summary>
    [DataContract]
    public class WebRole : WebData
    {
        /// <summary>
        /// Administration role id.
        /// </summary>
        [DataMember]
        public Int32 AdministrationRoleId
        { get; set; }

        /// <summary>
        /// Authorities
        /// </summary>
        [DataMember]
        public List<WebAuthority> Authorities
        { get; set; }

        /// <summary>
        /// UserId that created the record
        /// Set by database when inserted
        /// </summary>
        [DataMember]
        public Int32 CreatedBy
        { get; set; }

        /// <summary>
        /// Date record was created
        /// </summary>
        [DataMember]
        public DateTime CreatedDate
        { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public String Description
        { get; set; }

        /// <summary>
        /// GUID value
        /// </summary>
        [DataMember]
        public String GUID
        { get; set; }

        /// <summary>
        /// Id for this role
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Identifier for this role
        /// </summary>
        [DataMember]
        public String Identifier
        { get; set; }

        /// <summary>
        /// Is activation of role membership required for this role.
        /// </summary>
        [DataMember]
        public Boolean IsActivationRequired
        { get; set; }

        /// <summary>
        /// Is administration role id specified.
        /// </summary>
        [DataMember]
        public Boolean IsAdministrationRoleIdSpecified
        { get; set; }

        /// <summary>
        /// Is organization id specified.
        /// </summary>
        [DataMember]
        public Boolean IsOrganizationIdSpecified
        { get; set; }

        /// <summary>
        /// Is the role used as a user administration role 
        /// </summary>
        [DataMember]
        public Boolean IsUserAdministrationRole
        { get; set; }

        /// <summary>
        /// Is user administration role id specified.
        /// </summary>
        [DataMember]
        public Boolean IsUserAdministrationRoleIdSpecified
        { get; set; }

        /// <summary>
        /// Id of the Message Type
        /// </summary>
        [DataMember]
        public Int32 MessageTypeId
        { get; set; }

        /// <summary>
        /// UserId that modified the record
        /// Set by database when record is modified
        /// </summary>
        [DataMember]
        public Int32 ModifiedBy
        { get; set; }

        /// <summary>
        /// Date record was last modified
        /// </summary>
        [DataMember]
        public DateTime ModifiedDate
        { get; set; }

        /// <summary>
        /// RoleName
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Organization id that this role relates to.
        /// </summary>
        [DataMember]
        public Int32 OrganizationId
        { get; set; }

        /// <summary>
        /// ShortName
        /// </summary>
        [DataMember]
        public String ShortName
        { get; set; }

        /// <summary>
        /// User administration role id.
        /// </summary>
        [DataMember]
        public Int32 UserAdministrationRoleId
        { get; set; }

        /// <summary>
        /// Date role is valid from
        /// </summary>
        [DataMember]
        public DateTime ValidFromDate
        { get; set; }

        /// <summary>
        /// Date role is valid to
        /// </summary>
        [DataMember]
        public DateTime ValidToDate
        { get; set; }
    }
}

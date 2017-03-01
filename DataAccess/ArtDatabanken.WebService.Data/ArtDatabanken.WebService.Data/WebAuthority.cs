using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{

    /// <summary>
    ///  This class represents an Authority.
    /// </summary>
    [DataContract]
    public class WebAuthority : WebData
    {
        /// <summary>
        /// Users who has this autority has access 
        /// rights to these application actions.
        /// </summary>
        [DataMember]
        public List<String> ActionGUIDs
        { get; set; }

        /// <summary>
        /// Administration role id.
        /// </summary>
        [DataMember]
        public Int32 AdministrationRoleId
        { get; set; }

        /// <summary>
        /// ApplicationId
        /// </summary>
        [DataMember]
        public Int32 ApplicationId
        { get; set; }
        
        /// <summary>
        /// AuthorityDataType 
        /// </summary>
        [DataMember]
        public WebAuthorityDataType AuthorityDataType
        { get; set; }

        /// <summary>
        /// AuthorityType indicates type of Authority i.e Application or AuthorityDataType.
        /// </summary>
        [DataMember]
        public AuthorityType AuthorityType
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
        /// Create permission.
        /// </summary>
        [DataMember]
        public Boolean CreatePermission
        { get; set; }

        /// <summary>
        /// Delete permission.
        /// </summary>
        [DataMember]
        public Boolean DeletePermission
        { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        [DataMember]
        public String Description
        { get; set; }

        /// <summary>
        /// Users who has this autority has access 
        /// rights to information related to these factors.
        /// </summary>
        [DataMember]
        public List<String> FactorGUIDs
        { get; set; }

        /// <summary>
        /// GUID.
        /// </summary>
        [DataMember]
        public String GUID
        { get; set; }

        /// <summary>
        /// Id for this authority.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Authority identifier.
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
        /// Users who has this autority has access 
        /// rights to information related to these localities.
        /// </summary>
        [DataMember]
        public List<String> LocalityGUIDs
        { get; set; }

        /// <summary>
        /// Max species observation protection level that a
        /// user with this autority has read access rights to.
        /// </summary>
        [DataMember]
        public Int32 MaxProtectionLevel
        { get; set; }

        /// <summary>
        /// UserId that modified the record.
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
        /// Authority name.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Obligation.
        /// </summary>
        [DataMember]
        public String Obligation
        { get; set; }

        /// <summary>
        /// Users who has this autority has access 
        /// rights to information in these projects.
        /// </summary>
        [DataMember]
        public List<String> ProjectGUIDs
        { get; set; }

        /// <summary>
        /// Read permission.
        /// </summary>
        [DataMember]
        public Boolean ReadPermission
        { get; set; }

        /// <summary>
        /// Users who has this autority has access 
        /// rights to information related to these regions.
        /// </summary>
        [DataMember]
        public List<String> RegionGUIDs
        { get; set; }

        /// <summary>
        /// Role id.
        /// </summary>
        [DataMember]
        public Int32 RoleId
        { get; set; }

        /// <summary>
        /// Show non public data.
        /// This normally relates to species observations 
        /// that are protected by their owners.
        /// </summary>
        [DataMember]
        public Boolean ShowNonPublicData
        { get; set; }

        /// <summary>
        /// Users who has this autority has access 
        /// rights to information related to these taxa.
        /// </summary>
        [DataMember]
        public List<String> TaxonGUIDs
        { get; set; }

        /// <summary>
        /// Update permission.
        /// </summary>
        [DataMember]
        public Boolean UpdatePermission
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

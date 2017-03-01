using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents an organization.
    /// </summary>
    [DataContract]
    public class WebOrganization : WebData
    {
        /// <summary>
        /// Addresses.
        /// </summary>
        [DataMember]
        public List<WebAddress> Addresses
        { get; set; }

        /// <summary>
        /// Administration role id.
        /// </summary>
        [DataMember]
        public Int32 AdministrationRoleId
        { get; set; }

        /// <summary>
        /// Organization category.
        /// </summary>
        [DataMember]
        public WebOrganizationCategory Category
        { get; set; }

        /// <summary>
        /// Organization category id.
        /// </summary>
        public Int32 CategoryId
        { get; set; }

        /// <summary>
        /// UserId that created the record.
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
        /// GUID value.
        /// </summary>
        [DataMember]
        public String GUID
        { get; set; }

        /// <summary>
        /// Has species collection.
        /// Is set to False by default in database.
        /// If set to True the organization has a collection
        /// of biological material related to taxon observations.
        /// </summary>
        [DataMember]
        public Boolean HasSpeciesCollection
        { get; set; }

        /// <summary>
        /// Id for this organization.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Is administration role id specified.
        /// </summary>
        [DataMember]
        public Boolean IsAdministrationRoleIdSpecified
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
        /// Name
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// PhoneNumbers.
        /// </summary>
        [DataMember]
        public List<WebPhoneNumber> PhoneNumbers
        { get; set; }

        /// <summary>
        /// ShortName
        /// </summary>
        [DataMember]
        public String ShortName
        { get; set; }

        /// <summary>
        /// Date organization is valid from
        /// </summary>
        [DataMember]
        public DateTime ValidFromDate
        { get; set; }

        /// <summary>
        /// Date organization is valid to
        /// </summary>
        [DataMember]
        public DateTime ValidToDate
        { get; set; }
    }
}

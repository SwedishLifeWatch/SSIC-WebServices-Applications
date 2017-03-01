using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents an Organization Type
    /// </summary>
    [DataContract]
    public class WebOrganizationCategory : WebData
    {
        /// <summary>
        /// Administration role id.
        /// </summary>
        [DataMember]
        public Int32 AdministrationRoleId
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
        /// StringId for the Description property
        /// </summary>
        [DataMember]
        public Int32 DescriptionStringId
        { get; set; }

        /// <summary>
        /// Id for this organization category.
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
        /// Name
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }
    }
}

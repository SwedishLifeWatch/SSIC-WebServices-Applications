using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class handles search criterias that
    /// are used to find organizations.
    /// The operator 'AND' is used between the
    /// different search conditions.
    /// </summary>
    [DataContract]
    public class WebOrganizationSearchCriteria : WebData
    {
        /// <summary>
        /// Find organizations that have 
        /// the specified administration role
        /// </summary>
        [DataMember]
        public Int32 AdministrationRoleId
        { get; set; }

        /// <summary>
        /// Search for organizations with specified category.
        /// </summary>
        [DataMember]
        public WebOrganizationCategory Category
        { get; set; }

        /// <summary>
        /// Find organizations with a description
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        [DataMember]
        public String Description
        { get; set; }

        /// <summary>
        /// Find organizations that have species collection.
        /// Is set to False by default in database.
        /// If set to True the organization has a collection
        /// of biological material related to taxon observations.
        /// </summary>
        [DataMember]
        public Boolean HasSpeciesCollection
        { get; set; }

        /// <summary>
        /// Specifies if administration role id should be used.
        /// </summary>
        [DataMember]
        public Boolean IsAdministrationRoleIdSpecified
        { get; set; }

        /// <summary>
        /// Specifies if category should be used.
        /// </summary>
        [DataMember]
        public Boolean IsCategorySpecified
        { get; set; }

        /// <summary>
        /// Specifies if HasSpeciesCollection should be used.
        /// </summary>
        [DataMember]
        public Boolean IsHasSpeciesCollectionSpecified
        { get; set; }

        /// <summary>
        /// Specifies if is valid should be used.
        /// </summary>
        [DataMember]
        public Boolean IsIsValidSpecified
        { get; set; }

        /// <summary>
        /// Specifies if organization category id should be used.
        /// </summary>
        [DataMember]
        public Boolean IsOrganizationCategoryIdSpecified
        { get; set; }

        /// <summary>
        /// Restrict search to organizations that
        /// are valid today.
        /// </summary>
        [DataMember]
        public Boolean IsValid
        { get; set; }

        /// <summary>
        /// Find roles for organizations that is categorized 
        /// as the specified organization category.
        /// </summary>
        [DataMember]
        public Int32 OrganizationCategoryId
        { get; set; }

        /// <summary>
        /// Find organizations with a name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Find organizations with a shortname
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        [DataMember]
        public String ShortName
        { get; set; }
    }
}

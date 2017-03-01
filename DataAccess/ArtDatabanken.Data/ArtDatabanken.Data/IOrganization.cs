using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about an organization
    /// </summary>
    public interface IOrganization : IDataId32
    {
        /// <summary>
        /// GUID (Globally Unique Identifier) for this object.
        /// It is a LSID, which is unique for each version of the record holding the information included in this object. 
        /// It is updated automatically by database each time information is saved.
        /// </summary>
        String GUID
        { get; set; }

        /// <summary>
        /// HasCollection
        /// Is set to False by default in database. If set to True the organization has a collection of biological material related to taxon observations.
        /// </summary>
        Boolean HasSpeciesCollection
        { get; set; }

        /// <summary>
        /// Name of organization
        /// </summary>
        String Name
        { get; set; }

        /// <summary>
        /// Organization shortname
        /// </summary>
        String ShortName
        { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        String Description
        { get; set; }

        /// <summary>
        /// AdministrationRoleId
        /// </summary>
        Int32? AdministrationRoleId
        { get; set; }

        /// <summary>
        /// Date organization is valid from. Not Null. Is set to date created by default.
        /// </summary>
        DateTime ValidFromDate
        { get; set; }

        /// <summary>
        /// Date organization is valid to. Not Null. Is set to date created + 100 years by default.
        /// </summary>
        DateTime ValidToDate
        { get; set; }

        /// <summary>
        /// OrganizationCategory.
        /// The organization is of this type.
        /// </summary>
        IOrganizationCategory Category
        { get; set; }

        /// <summary>
        /// Addresses.
        /// The list object is automatically created.
        /// It is optional to add addresses
        /// </summary>
        AddressList Addresses
        { get; }

        /// <summary>
        /// Phone numbers.
        /// The list object is automatically created. 
        /// </summary>
        PhoneNumberList PhoneNumbers
        { get; }

         /// <summary>
        /// Get data context.
        /// </summary>
        IDataContext DataContext
        { get; set; }

         /// <summary>
        /// Information about create/update of organization.
        /// </summary>
        IUpdateInformation UpdateInformation
        { get; }
 
    }
}

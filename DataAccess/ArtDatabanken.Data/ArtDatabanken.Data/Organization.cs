using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    ///  This class contains information about a organization.
    /// </summary>
    public class Organization : IOrganization
    {

        /// <summary>
        /// Create an Organization instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public Organization(IUserContext userContext)
        {
            // Set default values.
            Addresses = new AddressList();
            AdministrationRoleId = null;
            DataContext = new DataContext(userContext);
            Description = null;
            HasSpeciesCollection = false;
            GUID = Settings.Default.OrganizationGUIDTemplate;
            Id = Int32.MinValue;
            Name = null;
            Category = new OrganizationCategory(Int32.MinValue, @"OrganizationCategoryName", null, Int32.MinValue, Int32.MinValue, new UpdateInformation(), DataContext);
            PhoneNumbers = new PhoneNumberList();
            ShortName = null;
            UpdateInformation = new UpdateInformation();
            ValidFromDate = DateTime.Now;
            ValidToDate = ValidFromDate + new TimeSpan(Settings.Default.ValidToDateYearIncrement * 365, 0, 0, 0);
        }

        /// <summary>
        /// Addresses.
        /// The list object is automatically created.
        /// It is optional to add addresses
        /// </summary>
        public AddressList Addresses
        { get; private set; }

        /// <summary>
        /// Administration role id. 
        /// Optional. It is set by super administrators in order to enable delegation of the administration of this object.
        /// </summary>
        public Int32? AdministrationRoleId
        { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Organization description. Optional. Max length 3000.
        /// </summary>
        public String Description
        { get; set; }

        /// <summary>
        /// GUID (Globally Unique Identifier) for this object.
        /// It is a LSID, which is unique for each version of the record holding the information included in this object. 
        /// It is updated automatically by database each time information is saved.
        /// </summary>
        public String GUID
        { get; set; }

        /// <summary>
        /// Has species collection
        /// Is set to False by default in database. If set to True the Organization owns a collection of biological material related to taxon observations.
        /// </summary>
        public Boolean HasSpeciesCollection
        { get; set; }

        /// <summary>
        /// Id for this organization
        /// </summary>
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Name of organization
        /// </summary>
        public String Name
        { get; set; }

        /// <summary>
        /// OrganizationCategory
        /// The organization is of this type.
        /// </summary>
        public IOrganizationCategory Category
        { get; set; }

        /// <summary>
        /// Phone numbers.
        /// The list object is automatically created. 
        /// </summary>
        public PhoneNumberList PhoneNumbers
        { get; private set; }

        /// <summary>
        /// ShortName of organization
        /// </summary>
        public String ShortName
        { get; set; }

        /// <summary>
        /// Date organization is valid from. Not Null. Is set to date created by default.
        /// </summary>
        public DateTime ValidFromDate
        { get; set; }

        /// <summary>
        /// Date organization is valid to. Not Null. Is set to date created + 100 years by default.
        /// </summary>
        public DateTime ValidToDate
        { get; set; }

        /// <summary>
        /// Information about create/update of organization.
        /// </summary>
        public IUpdateInformation UpdateInformation
        { get; private set; }




    }
}

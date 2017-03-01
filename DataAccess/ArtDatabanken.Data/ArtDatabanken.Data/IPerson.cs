using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    ///  This interface handles information about a person.
    /// </summary>
    public interface IPerson : IDataId32
    {
        /// <summary>
        /// Addresses.
        /// The list object is automatically created.
        /// It is optional to add addresses, but a person should be enforsed to provide at least one address in the process of user registration.
        /// If the person do not want to show its full address to other users the person should set ShowAddresses to False.
        /// </summary>
        AddressList Addresses
        { get; }

        /// <summary>
        /// Administration role id. 
        /// Optional. It is set by super administrators in order to enable delegation of the administration of this object.
        /// </summary>
        Int32? AdministrationRoleId
        { get; set; }

        /// <summary>
        /// Year person was born. Optional.
        /// </summary>
        DateTime? BirthYear
        { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Year person died. Optional.
        /// </summary>
        DateTime? DeathYear
        { get; set; }

        /// <summary>
        /// E-mail address. Is mandatory if the person object is associated with a user. 
        /// If the user do not want to expose this information in public set ShowEmailAddress to False.
        /// </summary>
        String EmailAddress
        { get; set; }

        /// <summary>
        /// Persons first name. Mandatory. Max length 50.
        /// </summary>
        String FirstName
        { get; set; }

        /// <summary>
        /// Get a persons full name.
        /// </summary>
        String FullName
        { get; }

        /// <summary>
        /// Gender. Not null. Is by default set to "Not specified".
        /// </summary>
        IPersonGender Gender
        { get; set; }

        /// <summary>
        /// GUID (Globally Unique Identifier) for this object.
        /// It is a LSID, which is unique for each version of the record holding the information included in this object. 
        /// It is updated automatically by database each time information is saved.
        /// </summary>
        String GUID
        { get; set; }

        /// <summary>
        /// Has species collection
        /// Is set to False by default in database. If set to True the Person owns a collection of biological material related to taxon observations.
        /// </summary>
        Boolean HasSpeciesCollection
        { get; set; }

        /// <summary>
        /// Persons last name. Mandatory. Max length 50.
        /// </summary>
        String LastName
        { get; set; }

        /// <summary>
        /// Selected language for person. Mandatory.
        /// </summary>
        ILocale Locale
        { get; set; }

        /// <summary>
        /// Persons middle name. Optional. Max length 50.
        /// </summary>
        String MiddleName
        { get; set; }

        /// <summary>
        /// Phone numbers.
        /// The list object is automatically created. 
        /// </summary>
        PhoneNumberList PhoneNumbers
        { get; }

        /// <summary>
        /// Presentation about the person. Optional. Max length 3000.
        /// </summary>
        String Presentation
        { get; set; }

        /// <summary>
        /// Show addresses to all. 
        /// Is set to False by default in database. If False address information (except City) should not be exposed to public users.
        /// It should only be set to true by the user if the user wants to expose its addresses to all other users.
        /// </summary>
        Boolean ShowAddresses
        { get; set; }

        /// <summary>
        /// Show E-mail address to all.
        /// Is set to False by default in database. If False E-mail should not be exposed to public users.
        /// It should only be set to true by the user if the user wants to expose its E-mail to all other users.
        /// </summary>
        Boolean ShowEmailAddress
        { get; set; }

        /// <summary>
        /// Show personal information (gender, birthyear) to all.
        /// If False personal information should not be exposed to public users.
        /// It should only be set to true by the user if the user wants to expose its personal information to all other users.
        /// </summary>
        Boolean ShowPersonalInformation
        { get; set; }

        /// <summary>
        /// Show phone numbers to all.
        /// If False phone number information should not be exposed to public users.
        /// It should only be set to true by the user if the user wants to expose its phone numbers to all other users.
        /// </summary>
        Boolean ShowPhoneNumbers
        { get; set; }

        /// <summary>
        /// Show presentation.
        /// If False presentation should not be exposed to public users.
        /// It should only be set to true by the user if the user wants to expose its presentation to all other users.
        /// </summary>
        Boolean ShowPresentation
        { get; set; }

        /// <summary>
        /// Selected Taxon name type id. Id represents the name type in the taxonomic database called Dyntaxa. 
        /// Not null. It is by default set to 0 which correspond to Scientific names. It should reflect the persons preferens for how taxon names should be presented.
        /// </summary>
        Int32 TaxonNameTypeId
        { get; set; }

        /// <summary>
        /// Information about create/update of person.
        /// </summary>
        IUpdateInformation UpdateInformation
        { get; }

        /// <summary>
        /// URL to the persons homepage. Optional. Max length 400.
        /// </summary>
        String URL
        { get; set; }

        /// <summary>
        /// User id.
        /// </summary>
        Int32? UserId
        { get; set; }

        /// <summary>
        /// Get user.
        /// May be null if no user is connected to the person.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>User.</returns>
        IUser GetUser(IUserContext userContext);

        /// <summary>
        /// Connect a user to this person.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="user">User.</param>
        void SetUser(IUserContext userContext, IUser user);
    }
}

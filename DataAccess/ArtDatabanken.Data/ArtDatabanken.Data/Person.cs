using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    ///  This class contains information about a person.
    /// </summary>
    [Serializable]
    public class Person : IPerson
    {
        private Int32? _userId;
        private IUser _user;

        /// <summary>
        /// Create a Person instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public Person(IUserContext userContext)
        {
            // Set default values.
            Addresses = new AddressList();
            AdministrationRoleId = null;
            BirthYear = null;
            DataContext = new DataContext(userContext);
            DeathYear = null;
            FirstName = null;
            Gender = CoreData.UserManager.GetPersonGender(userContext,
                                                          PersonGenderId.Unspecified);
            GUID = Settings.Default.PersonGUIDTemplate;
            Id = Int32.MinValue;
            Locale = userContext.Locale;
            MiddleName = null;
            PhoneNumbers = new PhoneNumberList();
            Presentation = null;
            ShowEmailAddress = false;
            ShowPersonalInformation = false;
            ShowPresentation = false;
            TaxonNameTypeId = (Int32)(TaxonNameCategoryId.ScientificName);
            UpdateInformation = new UpdateInformation();
            URL = null;
            _user = null;
            _userId = null;
        }

        /// <summary>
        /// Addresses.
        /// The list object is automatically created.
        /// It is optional to add addresses, but a person should be enforsed to provide at least one address in the process of user registration.
        /// If the person do not want to show its full address to other users the person should set ShowAddresses to False.
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
        /// Year person was born. Optional.
        /// </summary>
        public DateTime? BirthYear
        { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Year person died. Optional.
        /// </summary>
        public DateTime? DeathYear
        { get; set; }

        /// <summary>
        /// E-mail address. Is mandatory if the person object is associated with a user. If the user do not want to expose this information in public set ShowEmailAddress to False.
        /// </summary>
        public String EmailAddress
        { get; set; }

        /// <summary>
        /// Persons first name. Mandatory. Max length 50.
        /// </summary>
        public String FirstName
        { get; set; }

        /// <summary>
        /// Get a persons full name.
        /// </summary>
        public String FullName
        {
            get
            {
                return (FirstName + " " + LastName).Trim();
            }
        }

        /// <summary>
        /// Gender. Not null. Is by default set to "Not specified".
        /// </summary>
        public IPersonGender Gender
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
        /// Is set to False by default in database. If set to True the Person owns a collection of biological material related to taxon observations.
        /// </summary>
        public Boolean HasSpeciesCollection
        { get; set; }

        /// <summary>
        /// Id for this person.
        /// </summary>
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Persons last name. Mandatory. Max length 50.
        /// </summary>
        public String LastName
        { get; set; }

        /// <summary>
        /// Selected language for person. Mandatory.
        /// </summary>
        public ILocale Locale
        { get; set; }

        /// <summary>
        /// Persons middle name. Optional. Max length 50.
        /// </summary>
        public String MiddleName
        { get; set; }

        /// <summary>
        /// Phone numbers.
        /// The list object is automatically created. 
        /// </summary>
        public PhoneNumberList PhoneNumbers
        { get; private set; }

        /// <summary>
        /// Presentation about the person. Optional. Max length 3000.
        /// </summary>
        public String Presentation
        { get; set; }

        /// <summary>
        /// Show addresses to all. 
        /// Is set to False by default in database. If False address information (except City) should not be exposed to public users.
        /// It should only be set to true by the user if the user wants to expose its addresses to all other users.
        /// </summary>
        public Boolean ShowAddresses
        { get; set; }

        /// <summary>
        /// Show E-mail address to all.
        /// Is set to False by default in database. If False E-mail should not be exposed to public users.
        /// It should only be set to true by the user if the user wants to expose its E-mail to all other users.
        /// </summary>
        public Boolean ShowEmailAddress
        { get; set; }

        /// <summary>
        /// Show personal information (gender, birthyear) to all.
        /// If False personal information should not be exposed to public users.
        /// It should only be set to true by the user if the user wants to expose its personal information to all other users.
        /// </summary>
        public Boolean ShowPersonalInformation
        
        { get; set; }
        /// <summary>
        /// Show phone numbers to all.
        /// If False phone number information should not be exposed to public users.
        /// It should only be set to true by the user if the user wants to expose its phone numbers to all other users.
        /// </summary>
        public Boolean ShowPhoneNumbers
        { get; set; }

        /// <summary>
        /// Show presentation.
        /// If False presentation should not be exposed to public users.
        /// It should only be set to true by the user if the user wants to expose its presentation to all other users.
        /// </summary>
        public Boolean ShowPresentation
        { get; set; }

        /// <summary>
        /// Selected Taxon name type id. Id represents the name type in the taxonomic database called Dyntaxa. 
        /// Not null. It is by default set to 0 which correspond to Scientific names. It should reflect the persons preferens for how taxon names should be presented.
        /// </summary>
        public Int32 TaxonNameTypeId
        { get; set; }

        /// <summary>
        /// Information about create/update of person.
        /// </summary>
        public IUpdateInformation UpdateInformation
        { get; private set; }

        /// <summary>
        /// URL to the persons homepage. Optional. Max length 400.
        /// </summary>
        public String URL
        { get; set; }

        /// <summary>
        /// User id.
        /// </summary>
        public Int32? UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                _userId = value;

                if ((!_userId.HasValue) ||
                    (_user.IsNotNull() &&
                     (_user.Id != _userId.Value)))
                {
                    // Clear user object.
                    _user = null;
                }

                if (_user.IsNotNull() &&
                    ((!_user.PersonId.HasValue) ||
                     (_user.PersonId.Value != Id)))
                {
                    // Update user object with person information.
                    _user.PersonId = Id;
                }
            }
        }

        /// <summary>
        /// Get user.
        /// May be null if no user is connected to the person.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>User.</returns>
        public IUser GetUser(IUserContext userContext)
        {
            if (_user.IsNull() && _userId.HasValue)
            {
                // Get user object.
                _user = CoreData.UserManager.GetUser(userContext, _userId.Value);
                UserId = _user.Id;
            }
            return _user;
        }

        /// <summary>
        /// Connect a user to this person.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="user">User.</param>
        public void SetUser(IUserContext userContext, IUser user)
        {
            _user = user;
            if (user.IsNull())
            {
                UserId = null;
            }
            else
            {
                UserId = user.Id;
            }
        }
    }
}

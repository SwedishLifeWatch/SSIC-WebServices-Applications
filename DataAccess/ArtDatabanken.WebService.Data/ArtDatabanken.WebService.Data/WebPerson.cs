using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents a person.
    /// </summary>
    [DataContract]
    public class WebPerson : WebData
    {
        /// <summary>
        /// Addresses.
        /// The list object is automatically created.
        /// It is optional to add addresses, but a person should be enforsed to provide at least one address in the process of user registration.
        /// If the person do not want to show its full address to other users the person should set ShowAddresses to False.
        /// </summary>
        [DataMember]
        public List<WebAddress> Addresses
        { get; set; }

        /// <summary>
        /// Administration role id. 
        /// Optional. It is set by super administrators in order to enable delegation of the administration of this object.
        /// </summary>
        [DataMember]
        public Int32 AdministrationRoleId
        { get; set; }

        /// <summary>
        /// Year person was born. Optional.
        /// </summary>
        [DataMember]
        public DateTime BirthYear
        { get; set; }

        /// <summary>
        /// User id that created the person.
        /// Set by database when inserted.
        /// </summary>
        [DataMember]
        public Int32 CreatedBy
        { get; set; }

        /// <summary>
        /// Date and time when the person was created. Not null. Is set by the database.
        /// </summary>
        [DataMember]
        public DateTime CreatedDate
        { get; set; }

        /// <summary>
        /// Year person died. Optional.
        /// </summary>
        [DataMember]
        public DateTime DeathYear
        { get; set; }

        /// <summary>
        /// E-mail address. Is mandatory if the person object is associated with a user. 
        /// If the user do not want to expose this information in public set ShowEmailAddress to False.
        /// </summary>
        [DataMember]
        public String EmailAddress
        { get; set; }

        /// <summary>
        /// Persons first name. Mandatory. Max length 50.
        /// </summary>
        [DataMember]
        public String FirstName
        { get; set; }

        /// <summary>
        /// Gender. Not null. Is by default set to "Not specified".
        /// </summary>
        [DataMember]
        public WebPersonGender Gender
        { get; set; }

        /// <summary>
        /// Gender id. Not null. Is by default set to 3, which is the id representing "Not specified".
        /// </summary>
        public Int32 GenderId
        { get; set; }

        /// <summary>
        /// GUID (Globally Unique Identifier) for this object.
        /// It is a LSID, which is unique for each version of the record holding the information included in this object. 
        /// It is updated automatically by database each time information is saved.
        /// </summary>
        [DataMember]
        public String GUID
        { get; set; }

        /// <summary>
        /// HasSpeciesCollection
        /// Is set to False by default in database. If set to True the Person owns a collection of biological material related to taxon observations.
        /// </summary>
        [DataMember]
        public Boolean HasSpeciesCollection
        { get; set; }

        /// <summary>
        /// Id for this person.
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
        /// Is birth year specified.
        /// </summary>
        [DataMember]
        public Boolean IsBirthYearSpecified
        { get; set; }

        /// <summary>
        /// Is death year specified.
        /// </summary>
        [DataMember]
        public Boolean IsDeathYearSpecified
        { get; set; }

        /// <summary>
        /// Is user id specified.
        /// </summary>
        [DataMember]
        public Boolean IsUserIdSpecified
        { get; set; }

        /// <summary>
        /// Persons last name. Mandatory. Max length 50.
        /// </summary>
        [DataMember]
        public String LastName
        { get; set; }

        /// <summary>
        /// Selected locale (language) for this person. Mandatory.
        /// </summary>
        [DataMember]
        public WebLocale Locale
        { get; set; }

        /// <summary>
        /// ISO code representing selected locale (langage) for this person.
        /// </summary>
        public String LocaleISOCode
        { get; set; }

        /// <summary>
        /// Persons middle name. Optional. Max length 50.
        /// </summary>
        [DataMember]
        public String MiddleName
        { get; set; }

        /// <summary>
        /// Id of user that modified the object. Not null. Must be set each time the object is saved.
        /// </summary>
        [DataMember]
        public Int32 ModifiedBy
        { get; set; }

        /// <summary>
        /// Date and time when the object was last modified. Not null. Is set by the database.
        /// </summary>
        [DataMember]
        public DateTime ModifiedDate
        { get; set; }

        /// <summary>
        /// Phone numbers.
        /// The list object is automatically created. 
        /// </summary>
        [DataMember]
        public List<WebPhoneNumber> PhoneNumbers
        { get; set; }

        /// <summary>
        /// Presentation about the person. Optional. Max length 3000.
        /// </summary>
        [DataMember]
        public String Presentation
        { get; set; }

        /// <summary>
        /// Show addresses to all. 
        /// Is set to False by default in database. If False address information (except City) should not be exposed to public users.
        /// It should only be set to true by the user if the user wants to expose its addresses to all other users.
        /// </summary>
        [DataMember]
        public Boolean ShowAddresses
        { get; set; }

        /// <summary>
        /// Show E-mail address to all.
        /// Is set to False by default in database. If False E-mail should not be exposed to public users.
        /// It should only be set to true by the user if the user wants to expose its E-mail to all other users.
        /// </summary>
        [DataMember]
        public Boolean ShowEmailAddress
        { get; set; }

        /// <summary>
        /// Show personal information (gender, birthyear) to all.
        /// If False personal information should not be exposed to public users.
        /// It should only be set to true by the user if the user wants to expose its personal information to all other users.
        /// </summary>
        [DataMember]
        public Boolean ShowPersonalInformation
        { get; set; }

        /// <summary>
        /// Show phone numbers to all.
        /// If False phone number information should not be exposed to public users.
        /// It should only be set to true by the user if the user wants to expose its phone numbers to all other users.
        /// </summary>
        [DataMember]
        public Boolean ShowPhoneNumbers
        { get; set; }

        /// <summary>
        /// Show presentation.
        /// If False presentation should not be exposed to public users.
        /// It should only be set to true by the user if the user wants to expose its presentation to all other users.
        /// </summary>
        [DataMember]
        public Boolean ShowPresentation
        { get; set; }

        /// <summary>
        /// Selected Taxon name type id. Id represents the name type in the taxonomic database called Dyntaxa. 
        /// Not null. It is by default set to 0 which correspond to Scientific names. It should reflect the persons preferens for how taxon names should be presented.
        /// </summary>
        [DataMember]
        public Int32 TaxonNameTypeId
        { get; set; }

        /// <summary>
        /// URL to the persons homepage. Optional. Max length 400.
        /// </summary>
        [DataMember]
        public String URL
        { get; set; }

        /// <summary>
        /// User id. Optional. Is set by functions when a person is associated with a user account.
        /// </summary>
        [DataMember]
        public Int32 UserId
        { get; set; }
    }
}

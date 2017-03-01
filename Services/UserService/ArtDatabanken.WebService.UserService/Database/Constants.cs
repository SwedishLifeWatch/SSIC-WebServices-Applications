using System;

namespace ArtDatabanken.WebService.UserService.Database
{
    /// <summary>
    /// Constants used when activating role memebership.
    /// </summary>
    public struct ActivateRoleMembershipData
    {
        /// <summary>
        /// UserId
        /// </summary>
        public const String USER_ID = "UserId";
        /// <summary>
        /// RoleId
        /// </summary>
        public const String ROLE_ID = "RoleId";
    }


    /// <summary>
    /// Constants used when accessing adresses in database.
    /// </summary>
    public struct AddressData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// City
        /// </summary>
        public const String CITY = "City";
        /// <summary>
        /// CountryId
        /// </summary>
        public const String COUNTRY_ID = "CountryId";
        /// <summary>
        /// LocaleId
        /// </summary>
        public const String LOCALE_ID = "LocaleId";
        /// <summary>
        /// PostalAddress1
        /// </summary>
        public const String POSTALADDRESS1 = "PostalAddress1";
        /// <summary>
        /// PostalAddress2
        /// </summary>
        public const String POSTALADDRESS2 = "PostalAddress2";
        /// <summary>
        /// ZipCode
        /// </summary>
        public const String ZIPCODE = "ZipCode";
        /// <summary>
        /// AddressType
        /// </summary>
        public const String ADDRESS_TYPE = "AddressType";
        /// <summary>
        /// AddressType
        /// </summary>
        public const String ADDRESS_TYPE_ID = "AddressTypeId";
        /// <summary>
        /// PersonId
        /// </summary>
        public const String PERSON_ID = "PersonId";
        /// <summary>
        /// OrganizationId
        /// </summary>
        public const String ORGANIZATION_ID = "OrganizationId";
    }

    /// <summary>
    /// Constants used when accessing addresstypes in database.
    /// </summary>
    public struct AddressTypeData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// String Id of the name property
        /// </summary>
        public const String NAME_STRING_ID = "NameStringId";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
    }

    /// <summary>
    /// Constants used when accessing application information in database.
    /// </summary>
    public struct ApplicationData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// Identifier
        /// </summary>
        public const String IDENTIFIER = "Identifier";
        /// <summary>
        /// ApplicationId
        /// </summary>
        public const String APPLICATION_ID = "ApplicationId";
        /// <summary>
        /// ApplicationIdentity
        /// </summary>
        public const String APPLICATION_IDENTITY = "ApplicationIdentity";
        /// <summary>
        /// GUID
        /// </summary>
        public const String GUID = "GUID";
        /// <summary>
        /// LocaleId
        /// </summary>
        public const String LOCALE_ID = "LocaleId";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// ShortName
        /// </summary>
        public const String SHORT_NAME = "ShortName";
        /// <summary>
        /// URL
        /// </summary>
        public const String URL = "URL";
        /// <summary>
        /// Description
        /// </summary>
        public const String DESCRIPTION = "Description";
        /// <summary>
        /// ContactUserId
        /// </summary>
        public const String CONTACT_PERSON_ID = "ContactPersonId";
        /// <summary>
        /// AdministrationRoleId
        /// </summary>
        public const String ADMINISTRATION_ROLE_ID = "AdministrationRoleId";
        /// <summary>
        /// CreatedDate
        /// </summary>
        public const String CREATED_DATE = "CreatedDate";
        /// <summary>
        /// CreatedBy
        /// </summary>
        public const String CREATED_BY = "CreatedBy";
        /// <summary>
        /// ModifiedDate
        /// </summary>
        public const String MODIFIED_DATE = "ModifiedDate";
        /// <summary>
        /// ModifiedBy
        /// </summary>
        public const String MODIFIED_BY = "ModifiedBy";
        /// <summary>
        /// ValidFromDate
        /// </summary>
        public const String VALID_FROM_DATE = "ValidFromDate";
        /// <summary>
        /// ValidToDate
        /// </summary>
        public const String VALID_TO_DATE = "ValidToDate";
    }

    /// <summary>
    /// Constants used when accessing ApplicationAction information in database.
    /// </summary>
    public struct ApplicationActionData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// ApplicationIdentity
        /// </summary>
        public const String APPLICATION_ID = "ApplicationId";
        /// <summary>
        /// ApplicationActionIdentity
        /// </summary>
        public const String APPLICATION_ACTION_ID = "ApplicationActionId";
        /// <summary>
        /// GUID
        /// </summary>
        public const String GUID = "GUID";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// ActionIdentity
        /// </summary>
        public const String ACTION_IDENTITY = "ActionIdentity";
        /// <summary>
        /// Description
        /// </summary>
        public const String DESCRIPTION = "Description";
        /// <summary>
        /// AdministrationRoleId
        /// </summary>
        public const String ADMINISTRATION_ROLE_ID = "AdministrationRoleId";
        /// <summary>
        /// CreatedDate
        /// </summary>
        public const String CREATED_DATE = "CreatedDate";
        /// <summary>
        /// CreatedBy
        /// </summary>
        public const String CREATED_BY = "CreatedBy";
        /// <summary>
        /// ModifiedDate
        /// </summary>
        public const String MODIFIED_DATE = "ModifiedDate";
        /// <summary>
        /// ModifiedBy
        /// </summary>
        public const String MODIFIED_BY = "ModifiedBy";
        /// <summary>
        /// ValidFromDate
        /// </summary>
        public const String VALID_FROM_DATE = "ValidFromDate";
        /// <summary>
        /// ValidToDate
        /// </summary>
        public const String VALID_TO_DATE = "ValidToDate";
    }

    /// <summary>
    /// Constants used when accessing applicationversion information in database.
    /// </summary>
    public struct ApplicationVersionData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// ApplicationId
        /// </summary>
        public const String APPLICATION_ID = "ApplicationId";
        /// <summary>
        /// Version
        /// </summary>
        public const String VERSION = "Version";
        /// <summary>
        /// IsRecommended
        /// </summary>
        public const String IS_RECOMMENDED = "IsRecommended";
        /// <summary>
        /// IsValid
        /// </summary>
        public const String IS_VALID = "IsValid";
        /// <summary>
        /// Description
        /// </summary>
        public const String DESCRIPTION = "Description";
        /// <summary>
        /// CreatedDate
        /// </summary>
        public const String CREATED_DATE = "CreatedDate";
        /// <summary>
        /// CreatedBy
        /// </summary>
        public const String CREATED_BY = "CreatedBy";
        /// <summary>
        /// ModifiedDate
        /// </summary>
        public const String MODIFIED_DATE = "ModifiedDate";
        /// <summary>
        /// ModifiedBy
        /// </summary>
        public const String MODIFIED_BY = "ModifiedBy";
        /// <summary>
        /// ValidFromDate
        /// </summary>
        public const String VALID_FROM_DATE = "ValidFromDate";
        /// <summary>
        /// ValidToDate
        /// </summary>
        public const String VALID_TO_DATE = "ValidToDate";
    }

    /// <summary>
    /// Constants used when accessing authority in database.
    /// </summary>
    public struct AuthorityData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// ApplicationId
        /// </summary>
        public const String APPLICATION_ID = "ApplicationId";
        /// <summary>
        /// AuthorityId
        /// </summary>
        public const String AUTHORITY_ID = "AuthorityId";
        /// <summary>
        /// AuthorityIdentity
        /// </summary>
        public const String AUTHORITY_IDENTITY = "AuthorityIdentity";
        /// <summary>
        /// AuthorityAttributeTypeId
        /// </summary>
        public const String AUTHORITY_ATTRIBUTE_TYPE_ID = "AuthorityAttributeTypeId";
        /// <summary>
        /// AuthorityAttributeType
        /// </summary>
        public const String AUTHORITY_ATTRIBUTE_TYPE = "AuthorityAttributeType";
        /// <summary>
        /// AuthorityDataTypeId
        /// </summary>
        public const String AUTHORITY_DATA_TYPE_ID= "AuthorityDataTypeId";
        /// <summary>
        /// AttributeValue
        /// </summary>
        public const String ATTRIBUTE_VALUE = "AttributeValue";
        /// <summary>
        /// GUID
        /// </summary>
        public const String GUID = "GUID";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// RoleId
        /// </summary>
        public const String ROLE_ID = "RoleId";
        /// <summary>
        /// ShowNonPublicData
        /// </summary>
        public const String SHOW_NON_PUBLIC_DATA = "ShowNonPublicData";
        /// <summary>
        /// MaxProtectionLevel
        /// </summary>
        public const String MAX_PROTECTION_LEVEL = "MaxProtectionLevel";
        /// <summary>
        /// ReadPermission
        /// </summary>
        public const String READ_PERMISSION = "ReadPermission";
        /// <summary>
        /// CreatePermission
        /// </summary>
        public const String CREATE_PERMISSION = "CreatePermission";
        /// <summary>
        /// UpdatePermission
        /// </summary>
        public const String UPDATE_PERMISSION = "UpdatePermission";
        /// <summary>
        /// DeletePermission
        /// </summary>
        public const String DELETE_PERMISSION = "DeletePermission";
        /// <summary>
        /// AdministrationRoleId
        /// </summary>
        public const String ADMINISTRATION_ROLE_ID = "AdministrationRoleId";
        /// <summary>
        /// Description
        /// </summary>
        public const String DESCRIPTION = "Description";
        /// <summary>
        /// Obligation
        /// </summary>
        public const String OBLIGATION = "Obligation";
        /// <summary>
        /// CreatedDate
        /// </summary>
        public const String CREATED_DATE = "CreatedDate";
        /// <summary>
        /// CreatedBy
        /// </summary>
        public const String CREATED_BY = "CreatedBy";
        /// <summary>
        /// ModifiedDate
        /// </summary>
        public const String MODIFIED_DATE = "ModifiedDate";
        /// <summary>
        /// ModifiedBy
        /// </summary>
        public const String MODIFIED_BY = "ModifiedBy";
        /// <summary>
        /// ValidFromDate
        /// </summary>
        public const String VALID_FROM_DATE = "ValidFromDate";
        /// <summary>
        /// ValidToDate
        /// </summary>
        public const String VALID_TO_DATE = "ValidToDate";
    }

    /// <summary>
    /// Constants used when accessing authority data type in database.
    /// </summary>
    public struct AuthorityDataType
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// AuthorityDataTypeId
        /// </summary>
        public const String AUTHORITYDATATYPE_ID = "AuthorityDataTypeId";
        /// <summary>
        /// AuthorityTypeName
        /// </summary>
        public const String AUTHORITY_DATA_TYPE_IDENTITY = "AuthorityDataTypeIdentity";
    }

    /// <summary>
    /// Constants used when checking a users authorities.
    /// </summary>
    public struct CheckAuthorityData
    {
        /// <summary>
        /// User Id
        /// </summary>
        public const String USER_ID = "UserId";
        /// <summary>
        /// Role Id
        /// </summary>
        public const String ROLE_ID = "RoleId";
        /// <summary>
        /// Authority identifier
        /// </summary>
        public const String AUTHORITY_IDENTIFIER = "AuthorityIdentifier";
        /// <summary>
        /// Application identifier
        /// </summary>
        public const String APPLICATION_IDENTIFIER = "ApplicationIdentifier";
        /// <summary>
        /// Application action identifier
        /// </summary>
        public const String APPLICATION_ACTION_IDENTIFIER = "ApplicationActionIdentifier";
        /// <summary>
        /// Application action identifier
        /// </summary>
        public const String COUNT = "NumOfRecs";

    }

    /// <summary>
    /// Constants used when accessing country information in database
    /// </summary>
    public struct CountryData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// Name of the country in english.
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// Name of country in native language.
        /// </summary>
        public const String NATIVE_NAME = "NativeName";
        /// <summary>
        /// ISO Name
        /// </summary>
        public const String ISO_NAME = "IsoName";
        /// <summary>
        /// ISO Code
        /// </summary>
        public const String ISO_CODE = "ISO_3166_Code";
        /// <summary>
        /// Phone number prefix
        /// </summary>
        public const String PHONE_NUMBER_PREFIX = "PhoneNumberPrefix";
    }

    /// <summary>
    /// Constants used when accessing email information in database.
    /// </summary>
    public struct EmailData
    {
        /// <summary>
        /// EmailAddress
        /// </summary>
        public const String EMAIL_ADDRESS = "EmailAddress";
        /// <summary>
        /// Person id.
        /// </summary>
        public const String PERSON_ID = "PersonId";
        /// <summary>
        /// ShowEmail
        /// </summary>
        public const String SHOW_EMAIL = "ShowEmail";
        /// <summary>
        /// User id.
        /// </summary>
        public const String USER_ID = "UserId";
    }

    /// <summary>
    /// Constants used when accessing strings that needs translation.
    /// </summary>
    public struct LocaleData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// ISO code
        /// </summary>
        public const String ISO_CODE = "ISOCode";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// Native name
        /// </summary>
        public const String NATIVE_NAME = "NativeName";
        /// <summary>
        /// Locale String
        /// </summary>
        public const String LOCALE_STRING = "LocaleString";
        /// <summary>
        /// LocaleId
        /// </summary>
        public const String LOCALE_ID = "LocaleId";
    }

    /// <summary>
    /// Constants used when accessing information about message types.
    /// </summary>
    public struct MessageTypeData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// String Id of the name property
        /// </summary>
        public const String NAME_STRING_ID = "NameStringId";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
    }

    /// <summary>
    /// Constants used when accessing organization in database.
    /// </summary>
    public struct OrganizationData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// OrganizationId
        /// </summary>
        public const String ORGANIZATION_ID = "OrganizationId";
        /// <summary>
        /// GUID
        /// </summary>
        public const String GUID = "GUID";
        /// <summary>
        /// AdministrationRoleId
        /// </summary>
        public const String ADMINISTRATION_ROLE_ID = "AdministrationRoleId";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// ShortName
        /// </summary>
        public const String SHORT_NAME = "ShortName";
        /// <summary>
        /// OrganizationCategoryId
        /// </summary>
        public const String ORGANIZATION_CATEGORY_ID = "OrganizationCategoryId";
        /// <summary>
        /// Description
        /// </summary>
        public const String DESCRIPTION = "Description";
        /// <summary>
        /// HasCollection
        /// </summary>
        public const String HAS_COLLECTION = "HasCollection";
        /// <summary>
        /// CreatedDate
        /// </summary>
        public const String CREATED_DATE = "CreatedDate";
        /// <summary>
        /// CreatedBy
        /// </summary>
        public const String CREATED_BY = "CreatedBy";
        /// <summary>
        /// ModifiedDate
        /// </summary>
        public const String MODIFIED_DATE = "ModifiedDate";
        /// <summary>
        /// ModifiedBy
        /// </summary>
        public const String MODIFIED_BY = "ModifiedBy";
        /// <summary>
        /// ValidFromDate
        /// </summary>
        public const String VALID_FROM_DATE = "ValidFromDate";
        /// <summary>
        /// ValidToDate
        /// </summary>
        public const String VALID_TO_DATE = "ValidToDate";
    }

    /// <summary>
    /// Constants used when accessing organizationcategories in database.
    /// </summary>
    public struct OrganizationCategoryData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// String Id of the description property
        /// </summary>
        public const String DESCRIPTION_STRING_ID = "DescriptionId";
        /// <summary>
        /// OrganizationCategoryDescription
        /// </summary>
        public const String ORGANIZATION_CATEGORY_DESCRIPTION = "OrganizationCategoryDescription";
        /// <summary>
        /// OrganizationCategoryName
        /// </summary>
        public const String ORGANIZATION_CATEGORY_NAME = "OrganizationCategoryName";
        /// <summary>
        /// AdministrationRoleId
        /// </summary>
        public const String ADMINISTRATION_ROLE_ID = "AdministrationRoleId";
        /// <summary>
        /// CreatedDate
        /// </summary>
        public const String CREATED_DATE = "CreatedDate";
        /// <summary>
        /// CreatedBy
        /// </summary>
        public const String CREATED_BY = "CreatedBy";
        /// <summary>
        /// ModifiedDate
        /// </summary>
        public const String MODIFIED_DATE = "ModifiedDate";
        /// <summary>
        /// ModifiedBy
        /// </summary>
        public const String MODIFIED_BY = "ModifiedBy";
    }

    /// <summary>
    /// Constants used when accessing person information in database.
    /// </summary>
    public struct PersonData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// GUID
        /// </summary>
        public const String GUID = "GUID";
        /// <summary>
        /// FirstName
        /// </summary>
        public const String FIRST_NAME = "FirstName";
        /// <summary>
        /// FullName
        /// </summary>
        public const String FULL_NAME = "FullName";
        /// <summary>
        /// HasCollection
        /// </summary>
        public const String HAS_COLLECTION = "HasCollection";
        /// <summary>
        /// LocaleId
        /// </summary>
        public const String LOCALE_ID = "LocaleId";
        /// <summary>
        /// MiddleName
        /// </summary>
        public const String MIDDLE_NAME = "MiddleName";
        /// <summary>
        /// LastName
        /// </summary>
        public const String LAST_NAME = "LastName";
        /// <summary>
        /// Gender
        /// </summary>
        public const String GENDER_ID = "GenderId";
        /// <summary>
        /// TaxonNameTypeId
        /// </summary>
        public const String TAXON_NAME_TYPE_ID = "TaxonNameTypeId";
        /// <summary>
        /// BirthYear
        /// </summary>
        public const String BIRTH_YEAR = "BirthYear";
        /// <summary>
        /// BirthYear
        /// </summary>
        public const String DEATH_YEAR = "DeathYear";
        /// <summary>
        /// URL
        /// </summary>
        public const String URL = "URL";
        /// <summary>
        /// Presentation
        /// </summary>
        public const String PRESENTATION = "Presentation";
        /// <summary>
        /// ShowPresentation
        /// </summary>
        public const String SHOW_PRESENTATION = "ShowPresentation";
        /// <summary>
        /// ShowAddresses
        /// </summary>
        public const String SHOW_ADDRESSES = "ShowAddresses";
        /// <summary>
        /// ShowPersonalInformation
        /// </summary>
        public const String SHOW_PERSONALINFORMATION = "ShowPersonalInformation";
        /// <summary>
        /// ShowPhoneNumbers
        /// </summary>
        public const String SHOW_PHONENUMBERS = "ShowPhoneNumbers";
        /// <summary>
        /// UserId
        /// </summary>
        public const String USER_ID = "UserId";
        /// <summary>
        /// AdministrationRoleId
        /// </summary>
        public const String ADMINISTRATION_ROLE_ID = "AdministrationRoleId";
        /// <summary>
        /// CreatedDate
        /// </summary>
        public const String CREATED_DATE = "CreatedDate";
        /// <summary>
        /// CreatedBy
        /// </summary>
        public const String CREATED_BY = "CreatedBy";
        /// <summary>
        /// ModifiedDate
        /// </summary>
        public const String MODIFIED_DATE = "ModifiedDate";
        /// <summary>
        /// ModifiedBy
        /// </summary>
        public const String MODIFIED_BY = "ModifiedBy";
        /// <summary>
        /// From ModifiedDate
        /// </summary>
        public const String MODIFIED_FROM_DATE = "ModifiedFromDate";
        /// <summary>
        /// Until ModifiedDate
        /// </summary>
        public const String MODIFIED_UNTIL_DATE = "ModifiedUntilDate";
    }

    /// <summary>
    /// Constants used when accessing person genders in database.
    /// </summary>
    public struct PersonGenderData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// LocaleId
        /// </summary>
        public const String LOCALE_ID = "LocaleId";
        /// <summary>
        /// String Id of the name property
        /// </summary>
        public const String NAME_STRING_ID = "NameStringId";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
    }

    /// <summary>
    /// Constants used when accessing phonenumber in database.
    /// </summary>
    public struct PhoneNumberData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// PhoneNumber
        /// </summary>
        public const String PHONENUMBER = "PhoneNumber";
        /// <summary>
        /// CountryId
        /// </summary>
        public const String COUNTRY_ID = "CountryId";
        /// <summary>
        /// PhoneNumberType
        /// </summary>
        public const String PHONENUMBER_TYPE = "PhoneNumberType";
        /// <summary>
        /// PhoneNumberTypeId
        /// </summary>
        public const String PHONENUMBER_TYPE_ID = "TypeId";
        /// <summary>
        /// PersonId
        /// </summary>
        public const String PERSON_ID = "PersonId";
        /// <summary>
        /// OrganizationId
        /// </summary>
        public const String ORGANIZATION_ID = "OrganizationId";
    }

    /// <summary>
    /// Constants used when accessing phonenumber types in database.
    /// </summary>
    public struct PhoneNumberTypeData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// LocaleId
        /// </summary>
        public const String LOCALE_ID = "LocaleId";
        /// <summary>
        /// String Id of the name property
        /// </summary>
        public const String NAME_STRING_ID = "NameStringId";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
    }

    /// <summary>
    /// Constants used when accessing role in database.
    /// </summary>
    public struct RoleData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// Identifier
        /// </summary>
        public const String IDENTIFIER = "Identifier";
        /// <summary>
        /// RoleId
        /// </summary>
        public const String ROLE_ID = "RoleId";
        /// <summary>
        /// GUID
        /// </summary>
        public const String GUID = "GUID";
        /// <summary>
        /// RoleName
        /// </summary>
        public const String ROLE_NAME = "RoleName";
        /// <summary>
        /// ShortName
        /// </summary>
        public const String SHORT_NAME = "ShortName";
        /// <summary>
        /// Description
        /// </summary>
        public const String DESCRIPTION = "Description";
        /// <summary>
        /// AdministrationRoleId
        /// </summary>
        public const String ADMINISTRATION_ROLE_ID = "AdministrationRoleId";
        /// <summary>
        /// UserAdministrationRoleId
        /// </summary>
        public const String USER_ADMINISTRATION_ROLE_ID = "UserAdministrationRoleId";
        /// <summary>
        /// OrganizationId
        /// </summary>
        public const String ORGANIZATION_ID = "OrganizationId";
        /// <summary>
        /// IsActivationRequired
        /// </summary>
        public const String IS_ACTIVATION_REQUIRED = "IsActivationRequired";
        /// <summary>
        /// IsUserAdministrationRole
        /// </summary>
        public const String IS_USER_ADMINISTRATION_ROLE = "IsUserAdministrationRole";
        /// <summary>
        /// CreatedDate
        /// </summary>
        public const String CREATED_DATE = "CreatedDate";
        /// <summary>
        /// CreatedBy
        /// </summary>
        public const String CREATED_BY = "CreatedBy";
        /// <summary>
        /// MessageTypeId
        /// </summary>
        public const String MESSAGE_TYPE_ID = "MessageTypeId";
        /// <summary>
        /// ModifiedDate
        /// </summary>
        public const String MODIFIED_DATE = "ModifiedDate";
        /// <summary>
        /// ModifiedBy
        /// </summary>
        public const String MODIFIED_BY = "ModifiedBy";
        /// <summary>
        /// ValidFromDate
        /// </summary>
        public const String VALID_FROM_DATE = "ValidFromDate";
        /// <summary>
        /// ValidToDate
        /// </summary>
        public const String VALID_TO_DATE = "ValidToDate";
    }

    /// <summary>
    /// Constants used when accessing translation information in database.
    /// </summary>
    public struct TranslationData
    {
        /// <summary>
        /// PropertyName 
        /// </summary>
        public const String COLUMN_NAME = "ColumnName";
        /// <summary>
        /// NumOfRecs
        /// </summary>
        public const String NUM_OF_RECS = "NumOfRecs";
        /// <summary>
        /// TableName
        /// </summary>
        public const String TABLE_NAME = "TableName";
        /// <summary>
        /// Translation table name.
        /// </summary>
        public const String TRANSLATION_TABLE_NAME = "Translation";
        /// <summary>
        /// Value
        /// </summary>
        public const String VALUE = "Value";
    }

    /// <summary>
    /// Constants used when accessing user information in database.
    /// </summary>
    public struct UserData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// UserId
        /// </summary>
        public const String USER_ID = "UserId";
        /// <summary>
        /// ApplicationId
        /// </summary>
        public const String APPLICATION_ID = "ApplicationId";
        /// <summary>
        /// GUID
        /// </summary>
        public const String GUID = "GUID";
        /// <summary>
        /// LocaleId
        /// </summary>
        public const String LOCALE_ID = "LocaleId";
        /// <summary>
        /// Pasword
        /// </summary>
        public const String PASSWORD = "Password";
        /// <summary>
        /// PersonId
        /// </summary>
        public const String PERSON_ID = "PersonId";
        /// <summary>
        /// AccountActivated
        /// </summary>
        public const String ACCOUNT_ACTIVATED = "AccountActivated";
        /// <summary>
        /// ActivationKey
        /// </summary>
        public const String ACTIVATION_KEY = "ActivationKey";
        /// <summary>
        /// AuthenticationType
        /// </summary>
        public const String AUTHENTICATION_TYPE = "AuthenticationType";
        /// <summary>
        /// AdministrationRoleId
        /// </summary>
        public const String ADMINISTRATION_ROLE_ID = "AdministrationRoleId";
        /// <summary>
        /// IsActivationRequired
        /// </summary>
        public const String IS_ACTIVATION_REQUIRED = "IsActivationRequired";
        /// <summary>
        /// UserName
        /// </summary>
        public const String USER_NAME = "UserName";
        /// <summary>
        /// UserType
        /// </summary>
        public const String USER_TYPE = "UserType";
        /// <summary>
        /// TableName
        /// </summary>
        public const String TABLE_NAME = "User";
        /// <summary>
        /// CreatedDate
        /// </summary>
        public const String CREATED_DATE = "CreatedDate";
        /// <summary>
        /// CreatedBy
        /// </summary>
        public const String CREATED_BY = "CreatedBy";
        /// <summary>
        /// ModifiedDate
        /// </summary>
        public const String MODIFIED_DATE = "ModifiedDate";
        /// <summary>
        /// ModifiedBy
        /// </summary>
        public const String MODIFIED_BY = "ModifiedBy";
        /// <summary>
        /// ValidFromDate
        /// </summary>
        public const String VALID_FROM_DATE = "ValidFromDate";
        /// <summary>
        /// ValidToDate
        /// </summary>
        public const String VALID_TO_DATE = "ValidToDate";
        /// <summary>
        /// FirstName
        /// </summary>
        public const String FIRST_NAME = "FirstName";
        /// <summary>
        /// FullName
        /// </summary>
        public const String FULL_NAME = "FullName";
        /// <summary>
        /// LastName
        /// </summary>
        public const String LAST_NAME = "LastName";
    }
}

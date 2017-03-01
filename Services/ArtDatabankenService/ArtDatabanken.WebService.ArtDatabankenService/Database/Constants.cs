using System;

namespace ArtDatabanken.WebService.ArtDatabankenService.Database
{
    /// <summary>
    /// Constants used when accessing bird nest activity information in database.
    /// </summary>
    public struct BirdNestActivityData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// Name column name
        /// </summary>
        public const String NAME_COLUMN = "aktivnamn";
        /// <summary>
        /// TableName
        /// </summary>
        public const String TABLE_NAME = "BirdActivity";
    }

    /// <summary>
    /// Constants used when accessing city information in database.
    /// </summary>
    public struct CityData
    {
        /// <summary>
        /// County
        /// </summary>
        public const String COUNTY = "County";
        /// <summary>
        /// County column name
        /// </summary>
        public const String COUNTY_COLUMN = "Ln";
        /// <summary>
        /// Municipality
        /// </summary>
        public const String MUNICIPALITY = "Municipality";
        /// <summary>
        /// Municipality column name
        /// </summary>
        public const String MUNICIPALITY_COLUMN = "Kn";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// Name column name
        /// </summary>
        public const String NAME_COLUMN = "Namn";
        /// <summary>
        /// Parish
        /// </summary>
        public const String PARISH = "Parish";
        /// <summary>
        /// Parish column name
        /// </summary>
        public const String PARISH_COLUMN = "Fg";
        /// <summary>
        /// SearchString
        /// </summary>
        public const String SEARCH_STRING = "SearchString";
        /// <summary>
        /// Table name
        /// </summary>
        public const String TABLE_NAME = "Cities";
        /// <summary>
        /// XCoordinate
        /// </summary>
        public const String X_COORDINATE = "XCoordinate";
        /// <summary>
        /// YCoordinate
        /// </summary>
        public const String Y_COORDINATE = "YCoordinate";
    }

    /// <summary>
    /// Constants used when accessing column information in database.
    /// </summary>
    public struct ColumnLenghtData
    {
        /// <summary>
        /// ColumnName
        /// </summary>
        public const String COLUMN_NAME = "ColumnName";
        /// <summary>
        /// TableName
        /// </summary>
        public const String TABLE_NAME = "TableName";
    }

    /// <summary>
    /// Constants used when accessing county data in database.
    /// </summary>
    public struct CountyData
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
        /// Name
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// Number
        /// </summary>
        public const String NUMBER = "Number";
        /// <summary>
        /// PartOfCountyId
        /// </summary>
        public const String PART_OF_COUNTY_ID = "PartOfCountyId";
    }

    /// <summary>
    /// Constants used when accessing information about databases.
    /// </summary>
    public struct DatabaseData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// LongName
        /// </summary>
        public const String LONG_NAME = "LongName";
        /// <summary>
        /// ShortName
        /// </summary>
        public const String SHORT_NAME = "ShortName";
        /// <summary>
        /// UpdateEnd
        /// </summary>
        public const String UPDATE_END = "UpdateEnd";
        /// <summary>
        /// UpdateStart
        /// </summary>
        public const String UPDATE_START = "UpdateStart";
    }

    /// <summary>
    /// Constants used when accessing generic data with id in database.
    /// </summary>
    public struct DataIdData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
    }

    /// <summary>
    /// Constants used when accessing endangered lists information in database.
    /// </summary>
    public struct EndangeredListData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
    }

    /// <summary>
    /// Constants used when accessing Factor information in database.
    /// </summary>
    public struct FactorData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// IsLeaf
        /// </summary>
        public const String IS_LEAF = "IsLeaf";
        /// <summary>
        /// SortOrder
        /// </summary>
        public const String SORT_ORDER = "SortOrder";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// Label
        /// </summary>
        public const String LABEL = "Label";
        /// <summary>
        /// Information
        /// </summary>
        public const String INFORMATION = "Information";
        /// <summary>
        /// FactorUpdateModeId
        /// </summary>
        public const String FACTOR_UPDATE_MODE_ID = "FactorUpdateModeId";
        /// <summary>
        /// FactorDataTypeId
        /// </summary>
        public const String FACTOR_DATA_TYPE_ID = "FactorDataTypeId";
        /// <summary>
        /// FactorOriginId
        /// </summary>
        public const String FACTOR_ORIGIN_ID = "FactorOriginId";
        /// <summary>
        /// HostLabel
        /// </summary>
        public const String HOST_LABEL = "HostLabel";
        /// <summary>
        /// DefaultHostParentTaxonId
        /// </summary>
        public const String DEFAULT_HOST_PARENT_TAXON_ID = "DefaultHostParentTaxonId";
        /// <summary>
        /// IsPeriodic
        /// </summary>
        public const String IS_PERIODIC = "IsPeriodic";
        /// <summary>
        /// IsPublic
        /// </summary>
        public const String IS_PUBLIC = "IsPublic";
        /// <summary>
        /// RequestId
        /// </summary>
        public const String REQUEST_ID = "RequestId";
        /// <summary>
        /// HasSearchFactors
        /// </summary>
        public const String HAS_SEARCH_FACTORS = "HasSearchFactors";
        /// <summary>
        /// FactorNameSearchString
        /// </summary>
        public const String FACTOR_NAME_SEARCH_STRING = "FactorNameSearchString";
        /// <summary>
        /// NameSearchMethod
        /// </summary>
        public const String NAME_SEARCH_METHOD = "NameSearchMethod";
        /// <summary>
        /// RestrictSearchToScope
        /// </summary>
        public const String RESTRICT_SEARCH_TO_SCOPE = "RestrictSearchToScope";
        /// <summary>
        /// RestrictReturnToScope
        /// </summary>
        public const String RESTRICT_RETURN_TO_SCOPE = "RestrictReturnToScope";
    }

    /// <summary>
    /// Constants used when accessing FactorDataType information in database.
    /// </summary>
    public struct FactorDataTypeData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// Definition
        /// </summary>
        public const String DEFINITION = "Definition";
    }

    /// <summary>
    /// Constants used when accessing FactorField information in database.
    /// </summary>
    public struct FactorFieldData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// FactorDataTypeId
        /// </summary>
        public const String FACTOR_DATA_TYPE_ID = "FactorDataTypeId";
        /// <summary>
        /// DatabaseFieldName
        /// </summary>
        public const String DATABASE_FIELD_NAME = "DatabaseFieldName";
        /// <summary>
        /// Label
        /// </summary>
        public const String LABEL = "Label";
        /// <summary>
        /// Information
        /// </summary>
        public const String INFORMATION = "Information";
        /// <summary>
        /// IsMain
        /// </summary>
        public const String IS_MAIN = "IsMain";
        /// <summary>
        /// IsSubstantial
        /// </summary>
        public const String IS_SUBSTANTIAL = "IsSubstantial";
        /// <summary>
        /// TypeId
        /// </summary>
        public const String FACTOR_FIELD_TYPE_ID = "TypeId";
        /// <summary>
        /// Size
        /// </summary>
        public const String SIZE = "Size";
        /// <summary>
        /// FactorFieldEnumId
        /// </summary>
        public const String FACTOR_FIELD_ENUM_ID = "FactorFieldEnumId";
        /// <summary>
        /// UnitLabel
        /// </summary>
        public const String UNIT_LABEL = "UnitLabel";
    }

    /// <summary>
    /// Constants used when accessing FactorFieldEnum information in database.
    /// </summary>
    public struct FactorFieldEnumData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
    }

    /// <summary>
    /// Constants used when accessing FactorFieldEnumValue information in database.
    /// </summary>
    public struct FactorFieldEnumValueData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// FactorFieldEnumId
        /// </summary>
        public const String FACTOR_FIELD_ENUM_ID = "FactorFieldEnumId";
        /// <summary>
        /// KeyText
        /// </summary>
        public const String KEY_TEXT = "KeyText";
        /// <summary>
        /// KeyInt
        /// </summary>
        public const String KEY_INT = "KeyInt";
        /// <summary>
        /// Label
        /// </summary>
        public const String LABEL = "Label";
        /// <summary>
        /// Information
        /// </summary>
        public const String INFOMATION = "Information";
        /// <summary>
        /// SortOrder
        /// </summary>
        public const String SORT_ORDER = "SortOrder";
        /// <summary>
        /// ShouldBeSaved
        /// </summary>
        public const String SHOULD_BE_SAVED = "ShouldBeSaved";
    }

    /// <summary>
    /// Constants used when accessing FactorFieldType information in database.
    /// </summary>
    public struct FactorFieldTypeData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// DefinitionSwedish
        /// </summary>
        public const String DEFINITION = "DefinitionSwedish";
    }

    /// <summary>
    /// Constants used when accessing period data in database.
    /// </summary>
    public struct FactorOriginData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// Definition
        /// </summary>
        public const String DEFINITION = "Definition";
        /// <summary>
        /// SortOrder
        /// </summary>
        public const String SORTORDER = "SortOrder";
    }

    /// <summary>
    /// Constants used when accessing factor tree information in database.
    /// </summary>
    public struct FactorTreeData
    {
        /// <summary>
        /// ChildFactorId
        /// </summary>
        public const String CHILD_FACTOR_ID = "ChildFactorId";
        /// <summary>
        /// ParentFactorId
        /// </summary>
        public const String PARENT_FACTOR_ID = "ParentFactorId";
        /// <summary>
        /// RequestId
        /// </summary>
        public const String REQUEST_ID = "RequestId";
        /// <summary>
        /// HasSearchFactors
        /// </summary>
        public const String HAS_SEARCH_FACTORS = "HasSearchFactors";
        /// <summary>
        /// RelationId
        /// </summary>
        public const String RELATION_ID = "RelationId";
    }

    /// <summary>
    /// Constants used when accessing FactorUpdateMode information in database.
    /// </summary>
    public struct FactorUpdateModeData
    {
        /// <summary>
        /// AllowUpdate
        /// </summary>
        public const String ALLOW_UPDATE = "AllowUpdate";
        /// <summary>
        /// Definition
        /// </summary>
        public const String DEFINITION = "Definition";
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// Type
        /// </summary>
        public const String TYPE = "Type";
    }

    /// <summary>
    /// Constants used when accessing individual category data in database.
    /// </summary>
    public struct IndividualCategoryData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// Definition
        /// </summary>
        public const String DEFINITION = "Definition";
    }

    /// <summary>
    /// Constants used when accessing period data in database.
    /// </summary>
    public struct PeriodData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// Description_swedish
        /// </summary>
        public const String INFORMATION = "Description_Swedish";
        /// <summary>
        /// Period type id.
        /// </summary>
        public const String PERIOD_TYPE_ID = "PeriodTypeId";
        /// <summary>
        /// Period stop update date.
        /// </summary>
        public const String STOP_UPDATE = "StopUpdates";
        /// <summary>
        /// Period year.
        /// </summary>
        public const String YEAR = "Year";
    }

    /// <summary>
    /// Constants used when accessing period type data in database.
    /// </summary>
    public struct PeriodTypeData
    {
        /// <summary>
        /// Id.
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// Name.
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// Description.
        /// </summary>
        public const String DESCRIPTION = "Description";
    }

    /// <summary>
    /// Constants used when accessing province data in database.
    /// </summary>
    public struct ProvinceData
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
        /// Name
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// PartOfProvinceId
        /// </summary>
        public const String PART_OF_PROVINCE_ID = "PartOfProvinceId";
    }

    /// <summary>
    /// Constants used when accessing user reference information in database.
    /// </summary>
    public struct ReferenceData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// Name column name
        /// </summary>
        public const String NAME_COLUMN = "namn";
        /// <summary>
        /// Person
        /// </summary>
        public const String PERSON = "Person";
        /// <summary>
        /// Search string
        /// </summary>
        public const String SEARCH_STRING = "SearchString";
        /// <summary>
        /// TableName
        /// </summary>
        public const String TABLE_NAME = "dt_referens";
        /// <summary>
        /// Text
        /// </summary>
        public const String TEXT = "Text";
        /// <summary>
        /// Text column
        /// </summary>
        public const String TEXT_COLUMN = "text";
        /// <summary>
        /// Year
        /// </summary>
        public const String YEAR = "Year";
    }

    /// <summary>
    /// Constants used when accessing species facts information in database.
    /// </summary>
    public struct SpeciesFactData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// FactorId
        /// </summary>
        public const String FACTOR_ID = "FactorId";
        /// <summary>
        /// Factor id column name
        /// </summary>
        public const String FACTOR_ID_COLUMN = "faktor";
        /// <summary>
        /// FieldValue1
        /// </summary>
        public const String FIELD_VALUE_1 = "FieldValue1";
        /// <summary>
        /// FieldValue2
        /// </summary>
        public const String FIELD_VALUE_2 = "FieldValue2";
        /// <summary>
        /// FieldValue3
        /// </summary>
        public const String FIELD_VALUE_3 = "FieldValue3";
        /// <summary>
        /// FieldValue4
        /// </summary>
        public const String FIELD_VALUE_4 = "FieldValue4";
        /// <summary>
        /// Field value 4 column name
        /// </summary>
        public const String FIELD_VALUE_4_COLUMN = "text1";
        /// <summary>
        /// FieldValue5
        /// </summary>
        public const String FIELD_VALUE_5 = "FieldValue5";
        /// <summary>
        /// Field value 5 column name
        /// </summary>
        public const String FIELD_VALUE_5_COLUMN = "text2";
        /// <summary>
        /// HasSearchFactors
        /// </summary>
        public const String HAS_SEARCH_FACTORS = "HasSearchFactors";
        /// <summary>
        /// HasSearchTaxa
        /// </summary>
        public const String HAS_SEARCH_TAXA = "HasSearchTaxa";
        /// <summary>
        /// HasSearchIndividualCategories
        /// </summary>
        public const String HAS_SEARCH_INDIVIDUAL_CATEGORIES = "HasSearchIndividualCategories";
        /// <summary>
        /// HasSearchPeriods
        /// </summary>
        public const String HAS_SEARCH_PERIODS = "HasSearchPeriods";
        /// <summary>
        /// HasSearchHosts
        /// </summary>
        public const String HAS_SEARCH_HOSTS = "HasSearchHosts";
        /// <summary>
        /// HasSearchReferences
        /// </summary>
        public const String HAS_SEARCH_REFERENCES = "HasSearchReferences";
        /// <summary>
        /// HostId
        /// </summary>
        public const String HOST_ID = "HostId";
        /// <summary>
        /// Host id column name
        /// </summary>
        public const String HOST_ID_COLUMN = "host";
        /// <summary>
        /// IndividualCategoryId
        /// </summary>
        public const String INDIVIDUAL_CATEGORY_ID = "IndividualCategoryId";
        /// <summary>
        /// Individual category id column name
        /// </summary>
        public const String INDIVIDUAL_CATEGORY_ID_COLUMN = "IndividualCategoryId";
        /// <summary>
        /// PeriodId
        /// </summary>
        public const String PERIOD_ID = "PeriodId";
        /// <summary>
        /// Period id column name
        /// </summary>
        public const String PERIOD_ID_COLUMN = "period";
        /// <summary>
        /// QualityId
        /// </summary>
        public const String QUALITY_ID = "QualityId";
        /// <summary>
        /// ReferenceId
        /// </summary>
        public const String REFERENCE_ID = "ReferenceId";
        /// <summary>
        /// RequestId
        /// </summary>
        public const String REQUEST_ID = "RequestId";
        /// <summary>
        /// TableName
        /// </summary>
        public const String TABLE_NAME = "af_data";
        /// <summary>
        /// Taxon id column name
        /// </summary>
        public const String TAXON_ID_COLUMN = "taxon";
        /// <summary>
        /// TaxonId
        /// </summary>
        public const String TAXON_ID = "TaxonId";
        /// <summary>
        /// UpdateDate
        /// </summary>
        public const String UPDATE_DATE = "UpdateDate";
        /// <summary>
        /// UpdateUserFullName
        /// </summary>
        public const String UPDATE_USER_FULL_NAME = "UpdateUserFullName";
        /// <summary>
        /// Update user full name column name
        /// </summary>
        public const String UPDATE_USER_FULL_NAME_COLUMN = "person";
    }

    /// <summary>
    /// Constants used when accessing species fact quality information in database.
    /// </summary>
    public struct SpeciesFactQualityData
    {
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// Definition
        /// </summary>
        public const String DEFINITION = "Definition";
    }

    /// <summary>
    /// Constants used when accessing species observation information in database.
    /// </summary>
    public struct SpeciesObservationData
    {
        /// <summary>
        /// ChangedFrom
        /// </summary>
        public const String CHANGED_FROM = "ChangedFrom";
        /// <summary>
        /// ChangedTo
        /// </summary>
        public const String CHANGED_TO = "ChangedTo";

        /// <summary>
        /// Coordinate X.
        /// </summary>
        public const String COORDINATE_X = "EastCoordinate";

        /// <summary>
        /// Coordinate Y.
        /// </summary>
        public const String COORDINATE_Y = "NorthCoordinate";

        /// <summary>
        /// Database.
        /// </summary>
        public const String DATABASE = "Database";
        /// <summary>
        /// GUID.
        /// </summary>
        public const String GUID = "GUID";
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// MaxProtectionLevel
        /// </summary>
        public const String MAX_PROTECTION_LEVEL = "MaxProtectionLevel";

        /// <summary>
        /// Original species observation id.
        /// </summary>
        public const String ORIGINAL_SPECIES_OBSERVATION_ID = "DatabaseObservationId";

        /// <summary>
        /// Protection level.
        /// </summary>
        public const String PROTECTION_LEVEL = "ProtectionLevel";

        /// <summary>
        /// Taxon id.
        /// </summary>
        public const String TAXON_ID = "TaxonId";
    }

    /// <summary>
    /// Constants used when accessing taxon county occurrence information in database.
    /// </summary>
    public struct TaxonCountyOccurrenceData
    {
        /// <summary>
        /// ArtDataId
        /// </summary>
        public const String ART_DATA_ID = "ArtDataId";
        /// <summary>
        /// CountyId
        /// </summary>
        public const String COUNTY_ID = "CountyId";
        /// <summary>
        /// CountyOccurrence
        /// </summary>
        public const String COUNTY_OCCURRENCE = "CountyOccurrence";
        /// <summary>
        /// OriginalCountyOccurrence
        /// </summary>
        public const String ORIGINAL_COUNTY_OCCURRENCE = "OriginalCountyOccurrence";
        /// <summary>
        /// Source
        /// </summary>
        public const String SOURCE = "Source";
        /// <summary>
        /// SourceId
        /// </summary>
        public const String SOURCE_ID = "SourceId";
        /// <summary>
        /// TaxonId
        /// </summary>
        public const String TAXON_ID = "TaxonId";
    }

    /// <summary>
    /// Constants used when accessing taxon information in database.
    /// </summary>
    public struct TaxonData
    {
        /// <summary>
        /// Author
        /// </summary>
        public const String AUTHOR = "Author";
        /// <summary>
        /// Class
        /// </summary>
        public const String CLASS = "Class";
        /// <summary>
        /// CommonName
        /// </summary>
        public const String COMMON_NAME = "CommonName";
        /// <summary>
        /// Family
        /// </summary>
        public const String FAMILY = "Family";
        /// <summary>
        /// Genus
        /// </summary>
        public const String GENUS = "Genus";
        /// <summary>
        /// HasReturnTaxonTypes
        /// </summary>
        public const String HAS_RETURN_TAXON_TYPES = "HasReturnTaxonTypes";
        /// <summary>
        /// HasSearchTaxa
        /// </summary>
        public const String HAS_SEARCH_TAXA = "HasSearchTaxa";
        /// <summary>
        /// HasSearchTaxonTypes
        /// </summary>
        public const String HAS_SEARCH_TAXON_TYPES = "HasSearchTaxonTypes";
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// IsNatura2000Listed
        /// </summary>
        public const String IS_NATURA2000_LISTED = "IsNatura2000Listed";
        /// <summary>
        /// IsRedlisted
        /// </summary>
        public const String IS_REDLISTED = "IsRedlisted";
        /// <summary>
        /// IsRedlistedSpecies
        /// </summary>
        public const String IS_REDLISTED_SPECIES = "IsRedlistedSpecies";
        /// <summary>
        /// IsSwedishTaxon
        /// </summary>
        public const String IS_SWEDISH_TAXON = "IsSwedishTaxon";
        /// <summary>
        /// Kingdom
        /// </summary>
        public const String KINGDOM = "Kingdom";
        /// <summary>
        /// Landscape
        /// </summary>
        public const String LANDSCAPE = "Landscape";
        /// <summary>
        /// Order
        /// </summary>
        public const String ORDER = "Order";
        /// <summary>
        /// OrganismGroup
        /// </summary>
        public const String ORGANISM_GROUP = "OrganismGroup";
        /// <summary>
        /// OrganismGroupId
        /// </summary>
        public const String ORGANISM_GROUP_ID = "OrganismGroupId";
        /// <summary>
        /// OrganismSubGroup
        /// </summary>
        public const String ORGANISM_SUB_GROUP = "OrganismSubGroup";
        /// <summary>
        /// OrganismSubGroupId
        /// </summary>
        public const String ORGANISM_SUB_GROUP_ID = "OrganismSubGroupId";
        /// <summary>
        /// Phylum
        /// </summary>
        public const String PHYLUM = "Phylum";
        /// <summary>
        /// Query
        /// </summary>
        public const String QUERY = "Query";
        /// <summary>
        /// RequestId
        /// </summary>
        public const String REQUEST_ID = "RequestId";
        /// <summary>
        /// RedlistCategory
        /// </summary>
        public const String REDLIST_CATEGORY = "RedlistCategory";
        /// <summary>
        /// RedlistCategoryId
        /// </summary>
        public const String REDLIST_CATEGORY_ID = "RedlistCategoryId";
        /// <summary>
        /// RedlistCriteria
        /// </summary>
        public const String REDLIST_CRITERIA = "RedlistCriteria";
        /// <summary>
        /// RedlistTaxonCategoryId
        /// </summary>
        public const String REDLIST_TAXON_CATEGORY_ID = "RedlistTaxonCategoryId";
        /// <summary>
        /// RestrictReturnToScope
        /// </summary>
        public const String RESTRICT_RETURN_TO_SCOPE = "RestrictReturnToScope";
        /// <summary>
        /// RestrictReturnToSwedishSpecies
        /// </summary>
        public const String RESTRICT_RETURN_TO_SWEDISH_SPECIES = "RestrictReturnToSwedishSpecies";
        /// <summary>
        /// RestrictSearchToSwedishSpecies
        /// </summary>
        public const String RESTRICT_SEARCH_TO_SWEDISH_SPECIES = "RestrictSearchToSwedishSpecies";
        /// <summary>
        /// ScientificName
        /// </summary>
        public const String SCIENTIFIC_NAME = "ScientificName";
        /// <summary>
        /// SortOrder
        /// </summary>
        public const String SORT_ORDER = "SortOrder";
        /// <summary>
        /// Taxon
        /// </summary>
        public const String TABLE_NAME = "Taxon";
        /// <summary>
        /// TaxonUpdate
        /// </summary>
        public const String TABLE_UPDATE_NAME = "TaxonUpdate";
        /// <summary>
        /// TaxonId
        /// </summary>
        public const String TAXON_ID = "TaxonId";
        /// <summary>
        /// TaxonInformationType
        /// </summary>
        public const String TAXON_INFORMATION_TYPE = "TaxonInformationType";
        /// <summary>
        /// TaxonNameSearchString
        /// </summary>
        public const String TAXON_NAME_SEARCH_STRING = "TaxonNameSearchString";
        /// <summary>
        /// TaxonTypeId
        /// </summary>
        public const String TAXON_TYPE_ID = "TaxonTypeId";
        /// <summary>
        /// FactorId
        /// </summary>
        public const String FACTOR_ID = "FactorId";
    }

    /// <summary>
    /// Constants used when accessing taxon information of taxa related by host taxa in database.
    /// </summary>
    public struct TaxonHostData
    {
        /// <summary>
        /// Taxon id
        /// </summary>
        public const String TAXON_ID = "TaxonId";
        /// <summary>
        /// Host taxon id
        /// </summary>
        public const String HOST_TAXON_ID = "HostTaxonId";
        /// <summary>
        /// RequestId
        /// </summary>
        public const String REQUEST_ID = "RequestId";
        /// <summary>
        /// TaxonInformationType
        /// </summary>
        public const String TAXON_INFORMATION_TYPE = "TaxonInformationType";
    }

    /// <summary>
    /// Constants used when accessing taxon name information in database.
    /// </summary>
    public struct TaxonNameData
    {
        /// <summary>
        /// Author
        /// </summary>
        public const String AUTHOR = "Author";
        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// Is recommended.
        /// </summary>
        public const String IS_RECOMMENDED = "IsRecommended";
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// NameSearchMethod
        /// </summary>
        public const String NAME_SEARCH_METHOD = "NameSearchMethod";
        /// <summary>
        /// NameSearchString
        /// </summary>
        public const String NAME_SEARCH_STRING = "NameSearchString";
        /// <summary>
        /// TaxonId
        /// </summary>
        public const String TAXON_ID = "TaxonId";
        /// <summary>
        /// TaxonNameId
        /// </summary>
        public const String TAXON_NAME_ID = "TaxonNameId";
        /// <summary>
        /// TaxonNameTypeId
        /// </summary>
        public const String TAXON_NAME_TYPE_ID = "TaxonNameTypeId";
        /// <summary>
        /// TaxonNameUseTypeId
        /// </summary>
        public const String TAXON_NAME_USE_TYPE_ID = "TaxonNameUseTypeId";
        /// <summary>
        /// TaxonTypeId
        /// </summary>
        public const String TAXON_TYPE_ID = "TaxonTypeId";
        /// <summary>
        /// TaxonNameUpdate
        /// </summary>
        public const String TABLE_UPDATE_NAME = "TaxonNameUpdate";
    }

    /// <summary>
    /// Constants used when accessing taxon tree information in database.
    /// </summary>
    public struct TaxonTreeData
    {
        /// <summary>
        /// ChildTaxonId
        /// </summary>
        public const String CHILD_TAXON_ID = "ChildTaxonId";
        /// <summary>
        /// HasSearchTaxa
        /// </summary>
        public const String HAS_SEARCH_TAXA = "HasSearchTaxa";
        /// <summary>
        /// HasSearchTaxonTypes
        /// </summary>
        public const String HAS_SEARCH_TAXON_TYPES = "HasSearchTaxonTypes";
        /// <summary>
        /// ParentTaxonId
        /// </summary>
        public const String PARENT_TAXON_ID = "ParentTaxonId";
        /// <summary>
        /// ParentChildRelationId
        /// </summary>
        public const String PARENT_CHILD_RELATION_ID = "ParentChildRelationId";
        /// <summary>
        /// RequestId
        /// </summary>
        public const String REQUEST_ID = "RequestId";
        /// <summary>
        /// TaxonTreeUpdate
        /// </summary>
        public const String TABLE_UPDATE_NAME = "TaxonTreeUpdate";
        /// <summary>
        /// TaxonInformationType
        /// </summary>
        public const String TAXON_INFORMATION_TYPE = "TaxonInformationType";
        /// <summary>
        /// RelationId
        /// </summary>
        public const String RELATION_ID = "RelationId";
    }

    /// <summary>
    /// Constants used when accessing user selected factors in database.
    /// </summary>
    public struct UserSelectedFactorData
    {
        /// <summary>
        /// RequestId
        /// </summary>
        public const String REQUEST_ID = "RequestId";
        /// <summary>
        /// UserSelectedFactors
        /// </summary>
        public const String TABLE_NAME = "UserSelectedFactors";
        /// <summary>
        /// FactorId
        /// </summary>
        public const String FACTOR_ID = "FactorId";
        /// <summary>
        /// FactorUsage
        /// </summary>
        public const String FACTOR_USAGE = "FactorUsage";
    }

    /// <summary>
    /// Constants used when accessing user selected hosts in database.
    /// </summary>
    public struct UserSelectedHostData
    {
        /// <summary>
        /// RequestId
        /// </summary>
        public const String REQUEST_ID = "RequestId";
        /// <summary>
        /// UserSelectedHosts
        /// </summary>
        public const String TABLE_NAME = "UserSelectedHosts";
        /// <summary>
        /// HostId
        /// </summary>
        public const String HOST_ID = "HostId";
        /// <summary>
        /// HostUsage
        /// </summary>
        public const String HOST_USAGE = "HostUsage";
    }

    /// <summary>
    /// Constants used when accessing user selected individual categories in database.
    /// </summary>
    public struct UserSelectedIndividualCategoryData
    {
        /// <summary>
        /// RequestId
        /// </summary>
        public const String REQUEST_ID = "RequestId";
        /// <summary>
        /// UserSelectedIndividualCategories
        /// </summary>
        public const String TABLE_NAME = "UserSelectedIndividualCategories";
        /// <summary>
        /// IndividualCategoryId
        /// </summary>
        public const String INDIVIDUAL_CATEGORY_ID = "IndividualCategoryId";
        /// <summary>
        /// IndividualCategoryUsage
        /// </summary>
        public const String INDIVIDUAL_CATEGORY_USAGE = "IndividualCategoryUsage";
    }

    /// <summary>
    /// Constants used when accessing user selected parameters in database.
    /// </summary>
    public struct UserSelectedParameterData
    {
        /// <summary>
        /// FactorId
        /// </summary>
        public const String FACTOR_ID = "FactorId";
        /// <summary>
        /// HostId
        /// </summary>
        public const String HOST_ID = "HostId";
        /// <summary>
        /// IndividualCategoryId
        /// </summary>
        public const String INDIVIDUAL_CATEGORY_ID = "IndividualCategoryId";
        /// <summary>
        /// PeriodId
        /// </summary>
        public const String PERIOD_ID = "PeriodId";
        /// <summary>
        /// ReferenceId
        /// </summary>
        public const String REFERENCE_ID = "ReferenceId";
        /// <summary>
        /// RequestId
        /// </summary>
        public const String REQUEST_ID = "RequestId";
        /// <summary>
        /// UserSelectedParameters
        /// </summary>
        public const String TABLE_NAME = "UserSelectedParameters";
        /// <summary>
        /// TaxonId
        /// </summary>
        public const String TAXON_ID = "TaxonId";
    }

    /// <summary>
    /// Constants used when accessing user selected periods in database.
    /// </summary>
    public struct UserSelectedPeriodData
    {
        /// <summary>
        /// RequestId
        /// </summary>
        public const String REQUEST_ID = "RequestId";
        /// <summary>
        /// UserSelectedPeriods
        /// </summary>
        public const String TABLE_NAME = "UserSelectedPeriods";
        /// <summary>
        /// PeriodId
        /// </summary>
        public const String PERIOD_ID = "PeriodId";
        /// <summary>
        /// PeriodUsage
        /// </summary>
        public const String PERIOD_USAGE = "PeriodUsage";
    }

    /// <summary>
    /// Constants used when accessing user selected references in database.
    /// </summary>
    public struct UserSelectedReferenceData
    {
        /// <summary>
        /// RequestId
        /// </summary>
        public const String REQUEST_ID = "RequestId";
        /// <summary>
        /// UserSelectedReferences
        /// </summary>
        public const String TABLE_NAME = "UserSelectedReferences";
        /// <summary>
        /// ReferenceId
        /// </summary>
        public const String REFERENCE_ID = "ReferenceId";
        /// <summary>
        /// ReferenceUsage
        /// </summary>
        public const String REFERENCE_USAGE = "ReferenceUsage";
    }

    /// <summary>
    /// Constants used when accessing user selected species facts in database.
    /// </summary>
    public struct UserSelectedSpeciesFactData
    {
        /// <summary>
        /// FactorId
        /// </summary>
        public const String FACTOR_ID = "FactorId";
        /// <summary>
        /// HostId
        /// </summary>
        public const String HOST_ID = "HostId";
        /// <summary>
        /// IndividualCategoryId
        /// </summary>
        public const String INDIVIDUAL_CATEGORY_ID = "IndividualCategoryId";
        /// <summary>
        /// PeriodId
        /// </summary>
        public const String PERIOD_ID = "PeriodId";
        /// <summary>
        /// RequestId
        /// </summary>
        public const String REQUEST_ID = "RequestId";
        /// <summary>
        /// SpeciesFactId
        /// </summary>
        public const String SPECIES_FACT_ID = "SpeciesFactId";
        /// <summary>
        /// SpeciesFactUsage
        /// </summary>
        public const String SPECIES_FACT_USAGE = "SpeciesFactUsage";
        /// <summary>
        /// UserSelectedSpeciesFacts
        /// </summary>
        public const String TABLE_NAME = "UserSelectedSpeciesFacts";
        /// <summary>
        /// TaxonId
        /// </summary>
        public const String TAXON_ID = "TaxonId";
    }

    /// <summary>
    /// Constants used when accessing user selected taxon types in database.
    /// </summary>
    public struct UserSelectedSpeciesObservationsData
    {
        /// <summary>
        /// RequestId
        /// </summary>
        public const String REQUEST_ID = "RequestId";
        /// <summary>
        /// SpeciesObservationId
        /// </summary>
        public const String SPECIES_OBSERVATION_ID = "SpeciesObservationId";
        /// <summary>
        /// UserSelectedSpeciesObservations
        /// </summary>
        public const String TABLE_NAME = "UserSelectedSpeciesObservations";
    }

    /// <summary>
    /// Constants used when accessing user selected taxa in database.
    /// </summary>
    public struct UserSelectedTaxaData
    {
        /// <summary>
        /// RequestId
        /// </summary>
        public const String REQUEST_ID = "RequestId";
        /// <summary>
        /// UserSelectedTaxa
        /// </summary>
        public const String TABLE_NAME = "UserSelectedTaxa";
        /// <summary>
        /// TaxonId
        /// </summary>
        public const String TAXON_ID = "TaxonId";
        /// <summary>
        /// TaxonUsage
        /// </summary>
        public const String TAXON_USAGE = "TaxonUsage";
    }

    /// <summary>
    /// Constants used when accessing user selected taxon types in database.
    /// </summary>
    public struct UserSelectedTaxonTypesData
    {
        /// <summary>
        /// RequestId
        /// </summary>
        public const String REQUEST_ID = "RequestId";
        /// <summary>
        /// UserSelectedTaxonTypes
        /// </summary>
        public const String TABLE_NAME = "UserSelectedTaxonTypes";
        /// <summary>
        /// TaxonTypeId
        /// </summary>
        public const String TAXON_TYPE_ID = "TaxonTypeId";
        /// <summary>
        /// TaxonTypeUsage
        /// </summary>
        public const String TAXON_TYPE_USAGE = "TaxonTypeUsage";
    }

    /// <summary>
    /// Constants used when accessing web service log in database.
    /// </summary>
    public struct WebServiceLogData
    {
        /// <summary>
        /// ApplicationIdentifier
        /// </summary>
        public const String APPLICATION_IDENTIFIER = "ApplicationIdentifier";
        /// <summary>
        /// Information
        /// </summary>
        public const String INFORMATION = "Information";
        /// <summary>
        /// RowCount
        /// </summary>
        public const String ROW_COUNT = "RowCount";
        /// <summary>
        /// SqlServerUser
        /// </summary>
        public const String SQL_SERVER_USER = "SqlServerUser";
        /// <summary>
        /// Table name.
        /// </summary>
        public const String TABLE_NAME = "WebServiceLog";
        /// <summary>
        /// TcpIp
        /// </summary>
        public const String TCP_IP = "TcpIp";
        /// <summary>
        /// Text
        /// </summary>
        public const String TEXT = "Text";
        /// <summary>
        /// Time
        /// </summary>
        public const String TIME = "Time";
        /// <summary>
        /// Type
        /// </summary>
        public const String TYPE = "Type";
        /// <summary>
        /// WebServiceUser
        /// </summary>
        public const String WEB_SERVICE_USER = "WebServiceUser";
    }
}

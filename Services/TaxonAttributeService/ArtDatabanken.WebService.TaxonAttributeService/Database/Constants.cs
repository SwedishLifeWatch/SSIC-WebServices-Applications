using System;

namespace ArtDatabanken.WebService.TaxonAttributeService.Database
{
    /// <summary>
    /// Constants used when accessing Factor information in database.
    /// </summary>
    public struct FactorData
    {
        /// <summary>
        /// Default host parent taxon id for factor.
        /// </summary>
        public const string DEFAULT_HOST_PARENT_TAXON_ID = "DefaultHostParentTaxonId";

        /// <summary>
        /// Factor data type id for factor.
        /// </summary>
        public const string FACTOR_DATA_TYPE_ID = "FactorDataTypeId";

        /// <summary>
        /// Factor ids used to search factors.
        /// </summary>
        public const string FACTOR_IDS = "FactorIds";

        /// <summary>
        /// Name search method for factor.
        /// </summary>
        public const string FACTOR_NAME_SEARCH_METHOD = "NameSearchMethod";

        /// <summary>
        /// Factor name search string for factor.
        /// </summary>
        public const string FACTOR_NAME_SEARCH_STRING = "FactorNameSearchString";

        /// <summary>
        /// Factor origin id for Factor.
        /// </summary>
        public const string FACTOR_ORIGIN_ID = "FactorOriginId";

        /// <summary>
        /// Factor update mode id for factor.
        /// </summary>
        public const string FACTOR_UPDATE_MODE_ID = "FactorUpdateModeId";

        /// <summary>
        /// Host label for Factor.
        /// </summary>
        public const string HOST_LABEL = "HostLabel";

        /// <summary>
        /// Id for factor.
        /// </summary>
        public const string ID = "Id";

        /// <summary>
        /// Information for factor.
        /// </summary>
        public const string INFORMATION = "Information";

        /// <summary>
        /// Indicates if factor ids are specified in factor search.
        /// </summary>
        public const string IS_FACTOR_IDS_SPECIFIED = "HasSearchFactors";

        /// <summary>
        /// Is leaf for factor.
        /// </summary>
        public const string IS_LEAF = "IsLeaf";

        /// <summary>
        /// Is periodic for factor.
        /// </summary>
        public const string IS_PERIODIC = "IsPeriodic";

        /// <summary>
        /// Is public for factor.
        /// </summary>
        public const string IS_PUBLIC = "IsPublic";

        /// <summary>
        /// Label for factor.
        /// </summary>
        public const string LABEL = "Label";

        /// <summary>
        /// Name for Factor.
        /// </summary>
        public const string NAME = "Name";

        /// <summary>
        /// Restrict return to scope for factor.
        /// </summary>
        public const string RESTRICT_RETURN_TO_SCOPE = "RestrictReturnToScope";

        /// <summary>
        /// Restrict search to scope for factor.
        /// </summary>
        public const string RESTRICT_SEARCH_TO_SCOPE = "RestrictSearchToScope";

        /// <summary>
        /// Sort order for factor.
        /// </summary>
        public const string SORT_ORDER = "SortOrder";

        /// <summary>
        /// Restrict return to only public or all factors.
        /// </summary>
        public const string GET_ONLY_PUBLIC_FACTORS = "GetOnlyPublicFactors";
    }

    /// <summary>
    /// Constants used when accessing factor data type
    /// information in database.
    /// </summary>
    public struct FactorDataTypeData
    {
        /// <summary>
        /// Prefix used when reading factor data type
        /// information from data reader.
        /// </summary>
        public const string COLUMN_NAME_PREFIX = "FactorDataType";

        /// <summary>
        /// Definition for factor data type.
        /// </summary>
        public const string DEFINITION = "Definition";

        /// <summary>
        /// Id for factor data type.
        /// </summary>
        public const string ID = "Id";

        /// <summary>
        /// Name for factor data type.
        /// </summary>
        public const string NAME = "Name";
    }

    /// <summary>
    /// Constants used when accessing FactorField information in database.
    /// </summary>
    public struct FactorFieldData
    {
        /// <summary>
        /// Database field name for factor field.
        /// </summary>
        public const string DATABASE_FIELD_NAME = "DatabaseFieldName";

        /// <summary>
        /// Factor data type id for factor field.
        /// </summary>
        public const string FACTOR_DATA_TYPE_ID = "FactorDataTypeId";

        /// <summary>
        /// Factor field enumeration id for factor field.
        /// </summary>
        public const string FACTOR_FIELD_ENUM_ID = "FactorFieldEnumId";

        /// <summary>
        /// Type id for factor field.
        /// </summary>
        public const string FACTOR_FIELD_TYPE_ID = "TypeId";

        /// <summary>
        /// Id for factor field.
        /// </summary>
        public const string ID = "Id";

        /// <summary>
        /// Information for factor field.
        /// </summary>
        public const string INFORMATION = "Information";

        /// <summary>
        /// Is main for factor field.
        /// </summary>
        public const string IS_MAIN = "IsMain";

        /// <summary>
        /// Is substantial for factor field.
        /// </summary>
        public const string IS_SUBSTANTIAL = "IsSubstantial";

        /// <summary>
        /// Label for factor field.
        /// </summary>
        public const string LABEL = "Label";

        /// <summary>
        /// Size for factor field.
        /// </summary>
        public const string SIZE = "Size";

        /// <summary>
        /// Unit for factor field.
        /// </summary>
        public const string UNIT = "UnitLabel";
    }

    /// <summary>
    /// Constants used when accessing factor field enumeration
    ///  information in database.
    /// </summary>
    public struct FactorFieldEnumData
    {
        /// <summary>
        /// Prefix used when reading factor field enumeration.
        /// information from data reader.
        /// </summary>
        public const string COLUMN_NAME_PREFIX = "FactorFieldEnum";

        /// <summary>
        /// Id for factor field enumeration.
        /// </summary>
        public const string ID = "Id";
    }

    /// <summary>
    /// Constants used when accessing factor field enumeration value
    /// information in database.
    /// </summary>
    public struct FactorFieldEnumValueData
    {
        /// <summary>
        /// Enumeration id for factor field enumeration value.
        /// </summary>
        public const string FACTOR_FIELD_ENUM_ID = "FactorFieldEnumId";

        /// <summary>
        /// Id for factor factor field enumeration value.
        /// </summary>
        public const string ID = "Id";

        /// <summary>
        /// Information for factor field enumeration value.
        /// </summary>
        public const string INFORMATION = "Information";

        /// <summary>
        /// Key int for factor field enumeration value.
        /// </summary>
        public const string KEY_INT = "KeyInt";

        /// <summary>
        /// Key text for factor field enumeration value.
        /// </summary>
        public const string KEY_TEXT = "KeyText";

        /// <summary>
        /// Label for factor field enumeration value.
        /// </summary>
        public const string LABEL = "Label";

        /// <summary>
        /// Should be saved for factor field enumeration value.
        /// </summary>
        public const string SHOULD_BE_SAVED = "ShouldBeSaved";

        /// <summary>
        /// Sort order for factor field enumeration value.
        /// </summary>
        public const string SORT_ORDER = "SortOrder";
    }

    /// <summary>
    /// Constants used when accessing factor field type data in database.
    /// </summary>
    public struct FactorFieldTypeData
    {
        /// <summary>
        /// Definition for factor field type.
        /// </summary>
        public const string DEFINITION_SWEDISH = "DefinitionSwedish";

        /// <summary>
        /// Id for factor field type.
        /// </summary>
        public const string ID = "Id";

        /// <summary>
        /// Name for factor field type.
        /// </summary>
        public const string NAME = "Name";
    }

    /// <summary>
    /// Constants used when accessing factor origin data in database.
    /// </summary>
    public struct FactorOriginData
    {
        /// <summary>
        /// Definition for factor origin.
        /// </summary>
        public const string DEFINITION = "Definition";

        /// <summary>
        /// Id for factor origin.
        /// </summary>
        public const string ID = "Id";

        /// <summary>
        /// Name for factor origin.
        /// </summary>
        public const string NAME = "Name";

        /// <summary>
        /// Sort order for factor origin.
        /// </summary>
        public const string SORT_ORDER = "SortOrder";
    }

    /// <summary>
    /// Constants used when accessing factor tree information in database.
    /// </summary>
    public struct FactorTreeData
    {
        /// <summary>
        /// Child factor id.
        /// </summary>
        public const string CHILD_FACTOR_ID = "ChildFactorId";

        /// <summary>
        /// Factor ids used to search factors.
        /// </summary>
        public const string FACTOR_IDS = "FactorIds";

        /// <summary>
        /// Indicates if factor ids are specified in factor search.
        /// </summary>
        public const string IS_FACTOR_IDS_SPECIFIED = "HasSearchFactors";

        /// <summary>
        /// Parent factor id.
        /// </summary>
        public const string PARENT_FACTOR_ID = "ParentFactorId";
    }

    /// <summary>
    /// Constants used when accessing factor update mode data in database.
    /// </summary>
    public struct FactorUpdateModeData
    {
        /// <summary>
        /// Definition for factor update mode.
        /// </summary>
        public const string DEFINITION = "Definition";

        /// <summary>
        /// Id for factor update mode.
        /// </summary>
        public const string ID = "Id";

        /// <summary>
        /// Allow update for factor update mode.
        /// </summary>
        public const string IS_UPDATE_ALLOWED = "AllowUpdate";

        /// <summary>
        /// Name for factor update mode.
        /// </summary>
        public const string NAME = "Name";

        /// <summary>
        /// Type for factor update mode.
        /// </summary>
        public const string TYPE = "Type";
    }

    /// <summary>
    /// Constants used when accessing individual category data in database.
    /// </summary>
    public struct IndividualCategoryData
    {
        /// <summary>
        /// Definition for individual category.
        /// </summary>
        public const string DEFINITION = "Definition";

        /// <summary>
        /// Id for individual category.
        /// </summary>
        public const string ID = "Id";

        /// <summary>
        /// Name for individual category.
        /// </summary>
        public const string NAME = "Name";
    }

    /// <summary>
    /// Constants used when accessing period data in database.
    /// </summary>
    public struct PeriodData
    {
        /// <summary>
        /// Description for period.
        /// </summary>
        public const string DESCRIPTION_SWEDISH = "Description_Swedish";

        /// <summary>
        /// Id for period.
        /// </summary>
        public const string ID = "Id";

        /// <summary>
        /// Name for period.
        /// </summary>
        public const string NAME = "Name";

        /// <summary>
        /// Stop updates of species facts related to a period.
        /// </summary>
        public const string STOP_UPDATES = "StopUpdates";

        /// <summary>
        /// Period type id for period.
        /// </summary>
        public const string TYPE_ID = "PeriodTypeId";

        /// <summary>
        /// Year for period.
        /// </summary>
        public const string YEAR = "Year";
    }

    /// <summary>
    /// Constants used when accessing period type data in database.
    /// </summary>
    public struct PeriodTypeData
    {
        /// <summary>
        /// Id for period type.
        /// </summary>
        public const string ID = "Id";

        /// <summary>
        /// Name for period type.
        /// </summary>
        public const string NAME = "Name";

        /// <summary>
        /// Description for period type.
        /// </summary>
        public const string DESCRIPTION = "Description";
    }

    /// <summary>
    /// Constants used when accessing species facts information in database.
    /// </summary>
    public struct SpeciesFactData
    {
        /// <summary>
        /// Factor data type ids.
        /// </summary>
        public const String FACTOR_DATA_TYPE_IDS = "FactorDataTypeIds";

        /// <summary>
        /// Factor id.
        /// </summary>
        public const string FACTOR_ID = "FactorId";

        /// <summary>
        /// Factor ids.
        /// </summary>
        public const String FACTOR_IDS = "FactorIds";

        /// <summary>
        /// Field value 1.
        /// </summary>
        public const string FIELD_VALUE_1 = "FieldValue1";

        /// <summary>
        /// Field value 2.
        /// </summary>
        public const string FIELD_VALUE_2 = "FieldValue2";

        /// <summary>
        /// Field value 3.
        /// </summary>
        public const string FIELD_VALUE_3 = "FieldValue3";

        /// <summary>
        /// Field value 4.
        /// </summary>
        public const string FIELD_VALUE_4 = "FieldValue4";

        /// <summary>
        /// Field value 4 column name.
        /// </summary>
        public const string FIELD_VALUE_4_COLUMN_NAME = "text1";

        /// <summary>
        /// Field value 5.
        /// </summary>
        public const string FIELD_VALUE_5 = "FieldValue5";

        /// <summary>
        /// Field value 5 column name.
        /// </summary>
        public const string FIELD_VALUE_5_COLUMN_NAME = "text2";

        /// <summary>
        /// Host id.
        /// </summary>
        public const string HOST_ID = "HostId";

        /// <summary>
        /// Host id column name.
        /// </summary>
        public const string HOST_ID_COLUMN_NAME = "host";

        /// <summary>
        /// Host ids.
        /// </summary>
        public const String HOST_IDS = "HostIds";

        /// <summary>
        /// Id for this species fact.
        /// </summary>
        public const string ID = "Id";

        /// <summary>
        /// Individual category id.
        /// </summary>
        public const string INDIVIDUAL_CATEGORY_ID = "IndividualCategoryId";

        /// <summary>
        /// Individual category id column name.
        /// </summary>
        public const string INDIVIDUAL_CATEGORY_ID_COLUMN_NAME = "IndividualCategoryId";

        /// <summary>
        /// Is factor data type ids specified.
        /// </summary>
        public const string IS_FACTOR_DATA_TYPE_IDS_SPECIFIED = "IsFactorDataTypeIdsSpecified";

        /// <summary>
        /// Is factor ids specified.
        /// </summary>
        public const string IS_FACTOR_IDS_SPECIFIED = "IsFactorIdsSpecified";

        /// <summary>
        /// Is host ids specified.
        /// </summary>
        public const string IS_HOST_IDS_SPECIFIED = "IsHostIdsSpecified";

        /// <summary>
        /// Is taxon ids specified.
        /// </summary>
        public const string IS_TAXON_IDS_SPECIFIED = "IsTaxonIdsSpecified";

        /// <summary>
        /// Period id.
        /// </summary>
        public const string PERIOD_ID = "PeriodId";

        /// <summary>
        /// Period id column name.
        /// </summary>
        public const string PERIOD_ID_COLUMN_NAME = "period";

        /// <summary>
        /// Quality id.
        /// </summary>
        public const string QUALITY_ID = "QualityId";

        /// <summary>
        /// SQL server query.
        /// </summary>
        public const string QUERY = "Query";

        /// <summary>
        /// Reference id.
        /// </summary>
        public const string REFERENCE_ID = "ReferenceId";

        /// <summary>
        /// Species fact identifiers.
        /// </summary>
        public const string SPECIES_FACT_IDENTIFIERS = "SpeciesFactIdentifiers";

        /// <summary>
        /// Species fact ids.
        /// </summary>
        public const string SPECIES_FACT_IDS = "SpeciesFactIds";

        /// <summary>
        /// Species fact table name.
        /// </summary>
        public const string TABLE_NAME = "af_data";

        /// <summary>
        /// Taxon id.
        /// </summary>
        public const string TAXON_ID = "TaxonId";

        /// <summary>
        /// Taxon count column.
        /// </summary>
        public const string TAXON_COUNT = "TaxonCount";

        /// <summary>
        /// Taxon ids.
        /// </summary>
        public const string TAXON_IDS = "TaxonIds";

        /// <summary>
        /// Last time this species fact was updated.
        /// </summary>
        public const string UPDATE_DATE = "UpdateDate";

        /// <summary>
        /// Update person.
        /// </summary>
        public const string UPDATE_PERSON = "UpdateUserFullName";

        /// <summary>
        /// Update person column name.
        /// </summary>
        public const string UPDATE_PERSON_COLUMN_NAME = "person";

        /// <summary>
        /// Values to update species facts with.
        /// </summary>
        public const string VALUES = "Values";
    }

    /// <summary>
    /// Constants used when accessing period type data in database.
    /// </summary>
    public struct SpeciesFactQualityData
    {
        /// <summary>
        /// Description for species fact quality.
        /// </summary>
        public const string DEFINITION = "Definition";

        /// <summary>
        /// Id for species fact quality.
        /// </summary>
        public const string ID = "Id";

        /// <summary>
        /// Name for species fact quality.
        /// </summary>
        public const string NAME = "Name";
    }
}
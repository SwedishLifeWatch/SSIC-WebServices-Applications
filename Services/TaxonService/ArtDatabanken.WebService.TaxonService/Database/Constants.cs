using System;

namespace ArtDatabanken.WebService.TaxonService.Database
{
    /// <summary>
    /// Constants used when accessing taxon information in database.
    /// Common constants used by several taxon objects
    /// </summary>
    public struct TaxonCommon
    {
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
        /// GUID
        /// </summary>
        public const String GUID = "GUID";

        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";

        /// <summary>
        /// Id table
        /// </summary>
        public const String ID_TABLE = "IdValueTable";

        /// <summary>
        /// ShortName
        /// </summary>
        public const String SHORT_NAME = "ShortName";
        
        /// <summary>
        /// SortOrder
        /// </summary>
        public const String SORT_ORDER = "SortOrder";
        
        /// <summary>
        /// TaxonId
        /// </summary>
        public const String TAXON_ID = "TaxonId";

        /// <summary>
        /// TaxonIdAnchor
        /// </summary>
        public const String TAXON_ID_ANCHOR = "TaxonIdAnchor";
        
        /// <summary>
        /// Taxon id table
        /// </summary>
        public const String TAXONID_TABLE = "TaxonIdTable";
       
        /// <summary>
        /// ValidFromDate
        /// </summary>
        public const String VALID_FROM_DATE = "ValidFromDate";

        /// <summary>
        /// ValidToDate
        /// </summary>
        public const String VALID_TO_DATE = "ValidToDate";

        /// <summary>
        /// Is published
        /// </summary>
        public const String IS_PUBLISHED = "IsPublished";

        /// <summary>
        /// RevisionId
        /// </summary>
        public const String REVISON_ID = "RevisionId";

        /// <summary>
        /// RevisionEventId
        /// </summary>
        public const String REVISON_EVENT_ID = "RevisionEventId";

        /// <summary>
        /// ChangedInRevisionEventId
        /// </summary>
        public const String CHANGED_IN_REVISON_EVENT_ID = "ChangedInRevisionEventId";

        /// <summary>
        /// NumberOfSpeciesInSweden
        /// </summary>
        public const String NUMBER_OF_SPECIES_IN_SWEDEN = "NumberOfSpeciesInSweden";

        /// <summary>
        /// ReturnScope
        /// </summary>
        public const String RETURN_SCOPE = "ReturnScope";
    }

    /// <summary>
    /// Constants used when accessing taxon tree information in database.
    /// </summary>
    public struct TaxonTreeData
    {
        /// <summary>
        /// Child taxon id.
        /// </summary>
        public const String CHILD_TAXON_ID = "TaxonIdChild";
        /// <summary>
        /// Is valid.
        /// </summary>
        public const String IS_VALID = "IsValid";
        /// <summary>
        /// Locale id
        /// </summary>
        public const String LOCALE_ID = "LocaleId";
        /// <summary>
        /// Parent taxon id.
        /// </summary>
        public const String PARENT_TAXON_ID = "TaxonIdParent";
    }

    /// <summary>
    /// Constants used when accessing taxon categories information in database.
    /// </summary>
    public struct TaxonCategoryData
    {
        /// <summary>
        /// CreatedBy
        /// </summary>
        public const String CATEGORY_NAME = "CategoryName";
        /// <summary>
        /// CreatedBy
        /// </summary>
        public const String MAIN_CATEGORY = "MainCategory";
        /// <summary>
        /// CreatedBy
        /// </summary>
        public const String PARENT_CATEGORY = "ParentCategory";
        /// <summary>
        /// Taxon category id table
        /// </summary>
        public const String TAXONCATEGORY_ID_TABLE = "TaxonCategoryIdTable";
        /// <summary>
        /// CreatedBy
        /// </summary>
        public const String TAXONOMIC = "Taxonomic";
        /// <summary>
        /// Type
        /// </summary>
        public const String TYPE = "Type";
        /// <summary>
        /// CategoryId
        /// </summary>
        public const String CATEGORY_ID = "CategoryId";

    }

    /// <summary>
    /// Constants used when accessing taxon information in database.
    /// </summary>
    public struct TaxonData
    {
        /// <summary>
        /// AlertStatus
        /// </summary>
        public const String ALERT_STATUS = "AlertStatus";
        /// <summary>
        /// Author
        /// </summary>
        public const String AUTHOR = "Author";
        /// <summary>
        /// Category
        /// </summary>
        public const String CATEGORY = "Category";
        /// <summary>
        /// ChangeStatus
        /// </summary>
        public const String CHANGE_STATUS = "ChangeStatus";
        /// <summary>
        /// ChildTaxon in some cases SelectedTaxon"
        /// </summary>
        public const String CHILD_TAXON_ID = "TaxonNodeChildId";
        /// <summary>
        /// CommonName
        /// </summary>
        public const String COMMON_NAME = "CommonName";
        /// <summary>
        /// ConceptDefinitionFullGenerated
        /// </summary>
        public const String CONCEPT_DEFINITION_FULL_GENERATED = "ConceptDefinitionFullGenerated";
        /// <summary>
        /// ConceptDefinitionPart
        /// </summary>
        public const String CONCEPT_DEFINITION_PART = "ConceptDefinitionPart";
        /// <summary>
        /// IsValid
        /// </summary>
        public const String IS_VALID = "IsValid";
        /// <summary>
        /// OngoingRevisionId
        /// </summary>
        public const String ONGOING_REVISION_ID = "OngoingRevisionId";
        /// <summary>
        /// ParentTAxon
        /// </summary>
        public const String PARENT_TAXON_ID = "TaxonNodeParentId";
        /// <summary>
        /// PersonName
        /// </summary>
        public const String PERSON_NAME = "PersonName";
        /// <summary>
        /// ScientificName
        /// </summary>
        public const String SCIENTIFIC_NAME = "ScientificName";
    }

    /// <summary>
    /// Constants used when accessing strings that needs translation
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
    /// Constants used when accessing TaxonName
    /// </summary>
    public struct TaxonNameData
    {        

        /// <summary>
        /// TaxonId
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// TaxonNameId
        /// </summary>
        public const String TAXON_NAME_ID = "TaxonNameId";
        /// <summary>
        /// GUID
        /// </summary>
        public const String GUID = "GUID";
        /// <summary>
        /// CreatedDate
        /// </summary>
        public const String CREATED_DATE = "CreatedDate";
        /// <summary>
        /// CreatedBy
        /// </summary>
        public const String CREATED_BY = "CreatedBy";
        /// <summary>
        /// ValidFromDate
        /// </summary>
        public const String VALID_FROM_DATE = "ValidFromDate";
        /// <summary>
        /// ValidToDate
        /// </summary>
        public const String VALID_TO_DATE = "ValidToDate";
        /// <summary>
        /// Name
        /// </summary>
        public const string NAME = "Name";
        /// <summary>
        /// Author
        /// </summary>
        public const string AUTHOR = "Author";
        /// <summary>
        /// NameCategory
        /// </summary>
        public const string NAMECATEGORY = "NameCategory";
        /// <summary>
        /// NameUsage
        /// </summary>
        public const string NAMEUSAGE = "NameUsage";
        /// <summary>
        /// NameUsageNew
        /// </summary>
        public const string NAMEUSAGENEW = "NameUsageNew";
        /// <summary>
        /// DescriptionStringId
        /// </summary>
        public const string DESCRIPTIONSTRINGID = "DescriptionStringId";
        /// <summary>
        /// Description
        /// </summary>
        public const string DESCRIPTION = "Description";
        /// <summary>
        /// PersonName
        /// </summary>
        public const string PERSONNAME = "PersonName";
        /// <summary>
        /// IsAuthorIncudedInName
        /// </summary>
        public const string IS_AUTHOR_INCLUDED_IN_NAME = "IsAuthorIncludedInName";
        /// <summary>
        /// IsRecommended
        /// </summary>
        public const string IS_RECOMMENDED = "IsRecommended";
        /// <summary>
        /// IsOriginalName
        /// </summary>
        public const string IS_ORIGINAL = "IsOriginal";
        /// <summary>
        /// IsUnique
        /// </summary>
        public const string IS_UNIQUE = "IsUnique";
        /// <summary>
        /// IsOkForObsSystems
        /// </summary>
        public const string IS_OK_FOR_OBS_SYSTEMS = "IsOkForObsSystems";
        /// <summary>
        /// IsValidTaxon
        /// </summary>
        public const string IS_VALID_TAXON = "IsValidTaxon";
        /// <summary>
        /// IsValidTaxonName
        /// </summary>
        public const string IS_VALID_TAXONNAME = "IsValidTaxonName";
        /// <summary>
        /// TaxonName
        /// </summary>
        public const string TAXONNAME_PARAM = "TaxonName";
        /// <summary>
        /// CompareOperators
        /// </summary>
        public const string COMPARE_OPERATORS = "CompareOperators";
        /// <summary>
        /// CompareOperator
        /// </summary>
        public const string COMPARE_OPERATOR = "CompareOperator";
        /// <summary>
        /// TaxonIdTable
        /// </summary>
        public const string TAXON_ID_TABLE = "TaxonIdTable";
        /// <summary>
        /// Version
        /// </summary>
        public const string VERSION = "Version";
        /// <summary>
        /// ModifiedDateStart
        /// </summary>
        public const string MODIFIED_DATE_START = "ModifiedDateStart";
        /// <summary>
        /// ModifiedDateEnd
        /// </summary>
        public const string MODIFIED_DATE_END = "ModifiedDateEnd";
    }

    /// <summary>
    /// Constants used when accessing taxon name usage information in database.
    /// </summary>
    public struct TaxonNameUsageData
    {
        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";
        /// <summary>
        /// Description
        /// </summary>
        public const String DESCRIPTION = "Description";
    }

    /// <summary>
    /// Constants used when accessing reference relation information in database.
    /// </summary>
    public struct ReferenceRelationData
    {
        /// <summary>
        /// Reference ids.
        /// </summary>
        public const String REFERENCE_IDS = "ReferenceIds";
    }

    /// <summary>
    /// Constants used when accessing revision information in database.
    /// </summary>
    public struct RevisionData
    {
        /// <summary>
        /// Id.
        /// </summary>
        public const String ID = "Id";

        /// <summary>
        /// Identifier.
        /// </summary>
        public const String IDENTIFIER = "Identifier";
        
        /// <summary>
        /// Revision id.
        /// </summary>
        public const String REVISIONID = "RevisionId";
        
        /// <summary>
        /// Taxon id.
        /// </summary>
        public const String TAXONID = "TaxonId";
        
        /// <summary>
        /// Description string.
        /// </summary>
        public const String DESCRIPTIONSTRING = "DescriptionString";
        
        /// <summary>
        /// Expected start date.
        /// </summary>
        public const String EXPECTEDSTARTDATE = "ExpectedStartDate";
        
        /// <summary>
        /// Expected end date.
        /// </summary>
        public const String EXPECTEDENDDATE = "ExpectedEndDate";

        /// <summary>
        /// Revision state.
        /// </summary>
        public const String REVISIONSTATE = "RevisionState";
        
        /// <summary>
        /// Set to true when Artfakta update of species facts succeded
        /// </summary>
        public const String ISSPECIESFACTPUBLISHED = "IsSpeciesFactPublished";

        /// <summary>
        /// Set to true when Artfakta update of reference relations succeded
        /// </summary>
        public const string ISREFERENCERELATIONSPUBLISHED = "IsReferenceRelationsPublished";

        /// <summary>
        /// GUID.
        /// </summary>
        public const String GUID = "GUID";
        
        /// <summary>
        /// Created by.
        /// </summary>
        public const String CREATEDBY = "CreatedBy";
        
        /// <summary>
        /// Created date.
        /// </summary>
        public const String CREATEDDATE = "CreatedDate";
        
        /// <summary>
        /// Modified by.
        /// </summary>
        public const String MODIFIEDBY = "ModifiedBy";
        
        /// <summary>
        /// Modified date.
        /// </summary>
        public const String MODIFIEDDATE = "ModifiedDate";

        /// <summary>
        /// Taxon id table
        /// </summary>
        public const String REVISIONSTATEID_TABLE = "RevisionStateIdTable";

        /// <summary>
        /// Event type description.
        /// </summary>
        public const String EVENT_TYPE_DESCRIPTION = "EventType";
    }

    /// <summary>
    /// Constants used when accessing revision event information in database.
    /// </summary>
    public struct RevisionEventData
    {
        /// <summary>
        /// Id.
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// Revision id.
        /// </summary>
        public const String REVISIONID = "RevisionId";
        /// <summary>
        /// Event type id.
        /// </summary>
        public const String EVENTTYPEID = "EventType";
        /// <summary>
        /// Event type identifier.
        /// </summary>
        public const String EVENTTYPEIDENTIFIER = "Identifier";
        /// <summary>
        /// Event type description.
        /// </summary>
        public const String EVENTTYPEDESCRIPTION = "EventTypeDescription";
        /// <summary>
        /// Created by id.
        /// </summary>
        public const String CREATEDBYID = "CreatedBy";
        /// <summary>
        /// Created date.
        /// </summary>
        public const String CREATEDDATE = "CreatedDate";
        /// <summary>
        /// Description affected taxa.
        /// </summary>
        public const String DESCRIPTION_AFFECTEDTAXA = "Description_AffectedTaxa";
        /// <summary>
        /// Description new value.
        /// </summary>
        public const String DESCRIPTION_NEWVALUE = "Description_NewValue";
        /// <summary>
        /// Description old value.
        /// </summary>
        public const String DESCRIPTION_OLDVALUE = "Description_OldValue";
    }

    /// <summary>
    /// Constants used when accessing taxon properties information in database.
    /// </summary>
    public struct TaxonPropertiesData
    {
        /// <summary>
        /// Revision id.
        /// </summary>
        public const String REVISIONID = "RevisionId";
        /// <summary>
        /// Taxon catyegory id.
        /// </summary>
        public const String TAXONCATEGORYID = "TaxonCategory";
        /// <summary>
        /// Taxon category name.
        /// </summary>
        public const String TAXONCATEGORYNAME = "CategoryName";
        /// <summary>
        /// Is valid.
        /// </summary>
        public const String IS_VALID = "IsValid";
        /// <summary>
        /// Revision event id.
        /// </summary>
        public const String REVISIONEVENTID = "RevisionEventId";
        /// <summary>
        /// Changes in revision event id.
        /// </summary>
        public const String CHANGEDINREVISIONEVENTID = "ChangedInRevisionEventId";
        /// <summary>
        /// Is published.
        /// </summary>
        public const String ISPUBLISHED = "IsPublished";
        /// <summary>
        /// ConceptDefinitionPart
        /// </summary>
        public const String CONCEPT_DEFINITION_PART = "ConceptDefinitionPart";
        /// <summary>
        /// ConceptDefinitionFullGenerated
        /// </summary>
        public const String CONCEPT_DEFINITION_FULL_GENERATED = "ConceptDefinitionFullGenerated";
        /// <summary>
        /// AlertStatus
        /// </summary>
        public const String ALERT_STATUS = "AlertStatus";

        /// <summary>
        /// Is Microspecie
        /// </summary>
        public const String IS_MICROSPECIES = "IsMicrospecies";
    }

    /// <summary>
    /// Constants used when accessing taxon relation information in database.
    /// </summary>
    public struct TaxonRelationData
    {
        /// <summary>
        /// Id.
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// GUID.
        /// </summary>
        public const String GUID = "GUID";
        /// <summary>
        /// Taxon id parent.
        /// </summary>
        public const String TAXONIDPARENT = "TaxonIdParent";
        /// <summary>
        /// Taxon id child.
        /// </summary>
        public const String TAXONIDCHILD = "TaxonIdChild";
        /// <summary>
        /// Is main relation.
        /// </summary>
        public const String IS_MAIN_RELATION = "IsMainRelation";
        /// <summary>
        /// Person name.
        /// </summary>
        public const String PERSONNAME = "PersonName";
        /// <summary>
        /// Sort order.
        /// </summary>
        public const String SORTORDER = "SortOrder";
        /// <summary>
        /// Valid from date.
        /// </summary>
        public const String VALIDFROMDATE = "ValidFromDate";
        /// <summary>
        /// Valid to date.
        /// </summary>
        public const String VALIDTODATE = "ValidToDate";
        /// <summary>
        /// Revision event id.
        /// </summary>
        public const String REVISIONEVENTID = "RevisionEventId";
        /// <summary>
        /// Changed in revision event id.
        /// </summary>
        public const String CHANGEDINREVISIONEVENTID = "ChangedInRevisionEventId";
        /// <summary>
        /// Is published.
        /// </summary>
        public const String ISPUBLISHED = "IsPublished";
        /// <summary>
        /// Modified by.
        /// </summary>
        public const String MODIFIEDBY = "ModifiedBy";
        /// <summary>
        /// Modified date.
        /// </summary>
        public const String MODIFIEDDATE = "ModifiedDate";
        /// <summary>
        /// Created by.
        /// </summary>
        public const String CREATEDBY = "CreatedBy";
        /// <summary>
        /// Created date.
        /// </summary>
        public const String CREATEDDATE = "CreatedDate";
        /// <summary>
        /// Taxon relation search scope.
        /// </summary>
        public const String SEARCH_SCOPE = "TaxonRelationSearchScope";
    }

    /// <summary>
    /// Constants used when accessing lump split event information in database.
    /// </summary>
    public struct LumpSplitEventData
    {
        /// <summary>
        /// Id.
        /// </summary>
        public const String ID = "Id";
        /// <summary>
        /// Changed in revision event id.
        /// </summary>
        public const String CHANGEDINREVISIONEVENTID = "ChangedInRevisionEventId";
        /// <summary>
        /// Created by.
        /// </summary>
        public const String CREATEDBY = "CreatedBy";
        /// <summary>
        /// Created date.
        /// </summary>
        public const String CREATEDDATE = "CreatedDate";
        /// <summary>
        /// Modified by.
        /// </summary>
        public const String MODIFIEDBY = "ModifiedBy";
        /// <summary>
        /// Modified date.
        /// </summary>
        public const String MODIFIEDDATE = "ModifiedDate";
        /// <summary>
        /// Description string.
        /// </summary>
        public const String DESCRIPTIONSTRING = "DescriptionString";
        /// <summary>
        /// Event time stamp.
        /// </summary>
        public const String EVENTTIMESTAMP = "EventTimestamp";
        /// <summary>
        /// Event type.
        /// </summary>
        public const String EVENTTYPE = "EventType";
        /// <summary>
        /// Is published.
        /// </summary>
        public const String ISPUBLISHED = "IsPublished";
        /// <summary>
        /// Person name.
        /// </summary>
        public const String PERSONNAME = "PersonName";
        /// <summary>
        /// Revision event id.
        /// </summary>
        public const String REVISIONEVENTID = "RevisionEventId";
        /// <summary>
        /// Taxon id after.
        /// </summary>
        public const String TAXONIDAFTER = "TaxonIdAfter";
        /// <summary>
        /// Taxon id before.
        /// </summary>
        public const String TAXONIDBEFORE = "TaxonIdBefore";
        /// <summary>
        /// Valid from date.
        /// </summary>
        public const String VALIDFROMDATE = "ValidFromDate";
        /// <summary>
        /// Valid to date.
        /// </summary>
        public const String VALIDTODATE = "ValidToDate"; 
    }

    /// <summary>
    /// Constants used when updating taxon species facts in database.
    /// </summary>
    public struct TaxonSpeciesFact
    {
        /// <summary>
        /// Swedish occurrence.
        /// </summary>
        public const String SWEDISH_OCCURENCE = "SWEDISH_OCCURENCE";
        /// <summary>
        /// Reset swedish occurrence.
        /// </summary>
        public const String RESET_SWEDISH_OCCURRENCE = "RESET_SWEDISH_OCCURRENCE";
        /// <summary>
        /// Swedish history.
        /// </summary>
        public const String SWEDISH_HISTORY = "SWEDISH_HISTORY";
        /// <summary>
        /// Exclude from reporting systems.
        /// </summary>
        public const String EXCLUDE_FROM_REPORTING_SYSTEMS = "EXCLUDE_FROM_REPORTING_SYSTEMS";
        /// <summary>
        /// Dyntaxa quality.
        /// </summary>
        public const String DYNTAXA_QUALITY = "DYNTAXA_QUALITY";
        /// <summary>
        /// Reset dyntaxa quality.
        /// </summary>
        public const String RESET_DYNTAXA_QUALITY = "RESET_DYNTAXA_QUALITY";
        /// <summary>
        /// Number of species in sweden.
        /// </summary>
        public const String NUMBER_OF_SPECIES_IN_SWEDEN = "NUMBER_OF_SPECIES_IN_SWEDEN";
        /// <summary>
        /// Swedish history table.
        /// </summary>
        public const String SWEDISH_HISTORY_TABLE = "SWEDISHHISTORYTABLE";
        /// <summary>
        /// Swedish occurrence table.
        /// </summary>
        public const String SWEDISH_OCCURENCE_TABLE = "SWEDISHOCCURRENCETABLE";
    }

    /// <summary>
    /// Constants used when reading taxon statistics from database.
    /// </summary>
    public struct TaxonStatistics
    {
        /// <summary>
        /// Number of swedish occurrence.
        /// </summary>
        public const String NUMBER_OF_SWEDISH_OCCURRENCE = "NumberOfSwedishOccurrence";
        /// <summary>
        /// Number in dyntaxa.
        /// </summary>
        public const String NUMBER_IN_DYNTAXA = "NumberInDyntaxa";
        /// <summary>
        /// Quality category.
        /// </summary>
        public const String QUALITY_CATEGORY = "QualityCategory";
        /// <summary>
        /// Number of taxa.
        /// </summary>
        public const String NUMBER_OF_TAXA = "NumberOfTaxa";
        /// <summary>
        /// Root taxon id.
        /// </summary>
        public const String ROOT_TAXON_ID = "RootTaxonId";
        /// <summary>
        /// Number of swedish occurrence.
        /// </summary>
        public const String NUMBER_OF_SWEDISH_REPRO = "NumberOfSwedishRepro";
    }

    /// <summary>
    /// Constants used when reading taxon changes from database.
    /// </summary>
    public struct TaxonChange
    {
        /// <summary>
        /// TaxonId
        /// </summary>
        public const String TAXON_ID= "TaxonId";
        /// <summary>
        /// Taxa scientific name.
        /// </summary>
        public const String SCIENTIFIC_NAME = "ScientificName";
        /// <summary>
        /// Taxon id after lump/split.
        /// </summary>
        public const String TAXON_ID_AFTER = "TaxonIdAfter";
        /// <summary>
        /// Taxon category.
        /// </summary>
        public const String CATEGORY = "Category";
        /// <summary>
        /// Event type.
        /// </summary>
        public const String EVENT_TYPE = "EventType";
        /// <summary>
        /// Root taxon id
        /// </summary>
        public const String ROOT_TAXON_ID = "RootTaxonId";
        /// <summary>
        /// Date from
        /// </summary>
        public const String DATE_FROM = "DateFrom";
        /// <summary>
        /// Date to
        /// </summary>
        public const String DATE_TO =  "DateTo";
    }

    /// <summary>
    /// Constants used when accessing dyntaxa revision species fact in database.
    /// </summary>
    public struct DyntaxaRevisionSpeciesFact
    {
        /// <summary>
        /// FactorId
        /// </summary>
        public const String FACTOR_ID = "FactorId";

        /// <summary>
        /// StatusId
        /// </summary>
        public const String STATUS_ID = "StatusId";

        /// <summary>
        /// QualityId
        /// </summary>
        public const String QUALITY_ID = "QualityId";

        /// <summary>
        /// ReferenceId
        /// </summary>
        public const String REFERENCE_ID = "ReferenceId";

        /// <summary>
        /// Description
        /// </summary>
        public const String DESCRIPTION = "Description";

        /// <summary>
        /// Original StatusId
        /// </summary>
        public const String ORIGINAL_STATUS_ID = "OriginalStatusId";

        /// <summary>
        /// Original QualityId
        /// </summary>
        public const String ORIGINAL_QUALITY_ID = "OriginalQualityId";

        /// <summary>
        /// Original ReferenceId
        /// </summary>
        public const String ORIGINAL_REFERENCE_ID = "OriginalReferenceId";

        /// <summary>
        /// Original Description
        /// </summary>
        public const String ORIGINAL_DESCRIPTION = "OriginalDescription";

        /// <summary>
        /// Has values
        /// </summary>
        public const String SPECIESFACT_EXISTS = "SpeciesFactExists";
    }

    /// <summary>
    /// Constants used when accessing dyntaxa revision reference relation in database.
    /// </summary>
    public struct DyntaxaRevisionReferenceRelation
    {
        /// <summary>
        /// OldReferenceType
        /// </summary>
        public const string OLD_REFERENCE_TYPE = "OldReferenceType";
        
        /// <summary>
        /// ReferenceType
        /// </summary>
        public const string REFERENCE_TYPE = "ReferenceType";
        
        /// <summary>
        /// Action
        /// </summary>
        public const string ACTION = "Action";

        /// <summary>
        /// RelatedObjectGUID
        /// </summary>
        public const string RELATED_OBJECT_GUID = "RelatedObjectGUID";

        /// <summary>
        /// ReferenceRelationId
        /// </summary>
        public const String REFERENCE_RELATION_ID = "ReferenceRelationId";

        /// <summary>
        /// ReferenceId
        /// </summary>
        public const String REFERENCE_ID = "ReferenceId";
    }
}

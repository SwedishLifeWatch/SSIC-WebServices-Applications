using System;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Database
{
    /// <summary>
    /// Constants used when accessing artfakta information in database.
    /// </summary>
    public struct ArtFaktaData
    {
        /// <summary>
        /// Dyntaxa taxon id.
        /// </summary>
        public const String DYNTAXA_TAXON_ID = "dyntaxaTaxonId";

        /// <summary>
        /// Action plan.
        /// </summary>
        public const String ACTION_PLAN = "actionPlan";

        /// <summary>
        /// Conservation relevant.
        /// </summary>
        public const String CONSERVATION_RELEVANT = "conservationRelevant";

        /// <summary>
        /// Natura 2000.
        /// </summary>
        public const String NATURA_2000 = "natura2000";

        /// <summary>
        /// Protected by law.
        /// </summary>
        public const String PROTECTED_BY_LAW = "protectedByLaw";

        /// <summary>
        /// Protection level.
        /// </summary>
        public const String PROTECTION_LEVEL = "protectionLevel";

        /// <summary>
        /// Redlist category.
        /// </summary>
        public const String REDLIST_CATEGORY = "redlistCategory";

        /// <summary>
        /// Swedish immigration history.
        /// </summary>
        public const String SWEDISH_IMMIGRATION_HISTORY = "swedishImmigrationHistory";

        /// <summary>
        /// Swedish occurrence.
        /// </summary>
        public const String SWEDISH_OCCURRENCE = "swedishOccurrence";

        /// <summary>
        /// Organism group.
        /// </summary>
        public const String ORGANISM_GROUP = "organismGroup";

        /// <summary>
        /// Disturbance radius.
        /// </summary>
        public const String DISTURBANCE_RADIUS = "disturbanceRadius";
    }

    /// <summary>
    /// Constants used when accessing species observation field
    /// information in database.
    /// </summary>
    public struct SpeciesObservationFieldData
    {
        /// <summary>
        /// Species observation class.
        /// </summary>
        public const String CLASS = "class";

        /// <summary>
        /// Species observation class index.
        /// </summary>
        public const String CLASS_INDEX = "classIndex";

        /// <summary>
        /// Species observation field information.
        /// </summary>
        public const String INFORMATION = "information";

        /// <summary>
        /// Locale id.
        /// </summary>
        public const String LOCALE_ID = "localeId";

        /// <summary>
        /// Species observation property.
        /// </summary>
        public const String PROPERTY = "property";

        /// <summary>
        /// Species observation property index.
        /// </summary>
        public const String PROPERTY_INDEX = "propertyIndex";

        /// <summary>
        /// Species observation id.
        /// </summary>
        public const String SPECIES_OBSERVATION_ID = "observationId";

        /// <summary>
        /// Name of species observation field table in database.
        /// </summary>
        public const String TABLE_NAME = "SpeciesObservationField";

        /// <summary>
        /// Name of species observation field table in database.
        /// </summary>
        public const String CREATE_TABLE_NAME = "TempCreateSpeciesObservationField";

        /// <summary>
        /// Name of species observation field table in database.
        /// </summary>
        public const String UPDATE_TABLE_NAME = "TempUpdateSpeciesObservationField";

        /// <summary>
        /// Web data type.
        /// </summary>
        public const String TYPE_ID = "type";

        /// <summary>
        /// The unit.
        /// </summary>
        public const String UNIT = "unit";

        /// <summary>
        /// The value.
        /// </summary>
        public const String VALUE = "value";

        /// <summary>
        /// Catalog number.
        /// </summary>
        public const String CATALOGNUMBER = "catalogNumber";

        /// <summary>
        /// Species observation data provider id.
        /// </summary>
        public const String DATAPROVIDERID = "DataProviderId";

        /// <summary>
        /// The value as data type double.
        /// </summary>
        public const String VALUE_DOUBLE = "value_Double";

        /// <summary>
        /// The value as data type integer.
        /// </summary>
        public const String VALUE_BIGINT = "value_Int";

        /// <summary>
        /// The value as data type boolean.
        /// </summary>
        public const String VALUE_BOOLEAN = "value_Boolean";

        /// <summary>
        /// The value as data type datetime.
        /// </summary>
        public const String VALUE_DATETIME = "value_DateTime";

        /// <summary>
        /// The value as string.
        /// </summary>
        public const String VALUE_STRING = "value_String";
    }

    /// <summary>
    /// Constants used when accessing species observation field
    /// information in database.
    /// </summary>
    public struct SpeciesObservationErrorFieldData
    {
        /// <summary>
        /// Species observation Dataprovider.
        /// </summary>
        public const String DATAPROVIDER = "Dataprovider";

        /// <summary>
        /// Species observation Description.
        /// </summary>
        public const String DESCRIPTION = "Description";

        /// <summary>
        /// Species observation Description.
        /// </summary>
        public const String TRANSACTIONTYPE = "TransactionType";

        /// <summary>
        /// Species observation Description.
        /// </summary>
        public const String DATAPROVIDERID = "DataProviderID";


        public const String TABLE_NAME = "LogUpdateError";
    }

    /// <summary>
    /// Constants used when accessing taxon information in database.
    /// </summary>
    public struct TaxonData
    {
        /// <summary>
        /// TempTaxon.
        /// </summary>
        public const String TABLE_UPDATE_NAME = "TempTaxon";

        /// <summary>
        /// DyntaxaTaxonId.
        /// </summary>
        public const String DYNTAXA_TAXON_ID = "DyntaxaTaxonId";

        /// <summary>
        /// InfraspecificEpithet.
        /// </summary>
        public const String INFRASPECIFIC_EPITHET = "InfraspecificEpithet";

        /// <summary>
        /// NameAccordingTo.
        /// </summary>
        public const String NAME_ACCORDING_TO = "NameAccordingTo";

        /// <summary>
        /// NameAccordingToId.
        /// </summary>
        public const String NAME_ACCORDING_TO_ID = "NameAccordingToId";

        /// <summary>
        /// NamePublishedIn.
        /// </summary>
        public const String NAME_PUBLISHED_IN = "NamePublishedIn";

        /// <summary>
        /// NamePublishedInId.
        /// </summary>
        public const String NAME_PUBLISHED_IN_ID = "NamePublishedInId";

        /// <summary>
        /// NamePublishedInYear.
        /// </summary>
        public const String NAME_PUBLISHED_IN_YEAR = "NamePublishedInYear";

        /// <summary>
        /// NomenclaturalCode.
        /// </summary>
        public const String NOMENCLATURAL_CODE = "NomenclaturalCode";

        /// <summary>
        /// NomenclaturalStatus.
        /// </summary>
        public const String NOMENCLATURAL_STATUS = "NomenclaturalStatus";

        /// <summary>
        /// OriginalNameUsage.
        /// </summary>
        public const String ORIGINAL_NAME_USAGE = "OriginalNameUsage";

        /// <summary>
        /// OriginalNameUsageId
        /// </summary>
        public const String ORIGINAL_NAME_USAGE_ID = "OriginalNameUsageId";

        /// <summary>
        /// ScientificName
        /// </summary>
        public const String SCIENTIFIC_NAME = "ScientificName";

        /// <summary>
        /// ScientificNameAuthorship
        /// </summary>
        public const String SCIENTIFIC_NAME_AUTHORSHIP = "ScientificNameAuthorship";

        /// <summary>
        /// ScientificNameId
        /// </summary>
        public const String SCIENTIFIC_NAME_ID = "ScientificNameId";

        /// <summary>
        /// SpecificEpithet
        /// </summary>
        public const String SPECIFIC_EPITHET = "SpecificEpithet";

        /// <summary>
        /// TaxonConceptId
        /// </summary>
        public const String TAXON_CONCEPT_ID = "TaxonConceptId";

        /// <summary>
        /// TaxonConceptStatus
        /// </summary>
        public const String TAXON_CONCEPT_STATUS = "TaxonConceptStatus";

        /// <summary>
        /// TaxonomicStatus
        /// </summary>
        public const String TAXONOMIC_STATUS = "TaxonomicStatus";

        /// <summary>
        /// TaxonRank
        /// </summary>
        public const String TAXON_RANK = "TaxonRank";

        /// <summary>
        /// TaxonSortOrder
        /// </summary>
        public const String TAXON_SORT_ORDER = "TaxonSortOrder";

        /// <summary>
        /// TaxonURL
        /// </summary>
        public const String TAXON_URL = "TaxonURL";

        /// <summary>
        /// VernacularName
        /// </summary>
        public const String VERNACULAR_NAME = "VernacularName";

        /// <summary>
        /// TaxonCategoryId
        /// </summary>
        public const String CATEGORY_ID = "TaxonCategoryId";

        /// <summary>
        /// Action plan
        /// </summary>
        public const String ACTION_PLAN = "actionPlan";

        /// <summary>
        /// Conservation relevant
        /// </summary>
        public const String CONSERVATION_RELEVANT = "conservationRelevant";

        /// <summary>
        /// Natura 2000
        /// </summary>
        public const String NATURA_2000 = "natura2000";

        /// <summary>
        /// Protected by law
        /// </summary>
        public const String PROTECTED_BY_LAW = "protectedByLaw";

        /// <summary>
        /// Protection level
        /// </summary>
        public const String PROTECTION_LEVEL = "protectionLevel";

        /// <summary>
        /// Redlist category
        /// </summary>
        public const String REDLIST_CATEGORY = "redlistCategory";

        /// <summary>
        /// Swedish immigration history
        /// </summary>
        public const String SWEDISH_IMMIGRATION_HISTORY = "swedishImmigrationHistory";

        /// <summary>
        /// Swedish occurrence
        /// </summary>
        public const String SWEDISH_OCCURRENCE = "swedishOccurrence";

        /// <summary>
        /// Organism group
        /// </summary>
        public const String ORGANISM_GROUP = "organismGroup";

        /// <summary>
        /// Organism group
        /// </summary>
        public const String DISTURBANCE_RADIUS = "disturbanceRadius";

        /// <summary>
        /// Is Valid
        /// </summary>
        public const String ISVALID = "isValid";

        /// <summary>
        /// New DyntaxaTaxonId
        /// </summary>
        public const String NEW_DYNTAXA_TAXONID = "newDyntaxaTaxonId";

        /// <summary>
        /// Taxon Remark
        /// </summary>
        public const String TAXON_REMARK = "taxonRemark";

        /// <summary>
        /// New DyntaxaTaxonId
        /// </summary>
        public const String CURRENT_DYNTAXA_TAXONID = "currentDyntaxaTaxonId";

        /// <summary>
        /// Modified By
        /// </summary>
        public const String MODIFIED_BY = "modifiedBy";

        /// <summary>
        /// Modified Date
        /// </summary>
        public const String MODIFIED_DATE = "modifiedDate";
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
        public const String TABLE_UPDATE_NAME = "TempTaxonTree";

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
    /// Taxon tree information data.
    /// </summary>
    public struct TaxonTreeInformationData
    {
        /// <summary>
        /// ChildTaxonId.
        /// </summary>
        public const String HIGHER_CLASSIFICATION = "higherClassification";

        public const String CLASS = "class";
        public const String FAMILY = "family";
        public const String GENUS = "genus";
        public const String KINGDOM = "kingdom";
        public const String ORDER = "order";
        public const String PHYLUM = "phylum";
        public const String SUBGENUS = "subgenus";
    }

    /// <summary>
    /// Harvest log.
    /// </summary>
    public struct HarvestLog
    {
        public const string TYPE = "Type";
        public const string DESCRIPTION = "Description";
        public const string DATAPROVIDER = "Dataprovider";
        public const string OBSERVATIONID = "ObservationID";
        public const string CHANGEDFROM = "changedFrom";
        public const string CHANGEDTO = "changedTo";
        public const string PROCESSTIME = "processTime";
        public const string RESULT = "result";
        public const string ACTION = "action";
    }

    /// <summary>
    /// Harvest job.
    /// </summary>
    public struct HarvestJobTableData
    {
        public const string JOBSTARTDATE = "JobStartDate";
        public const string JOBENDDATE = "JobEndDate";
        public const string HARVESTSTARTDATE = "HarvestStartDate";
        public const string HARVESTCURRENTDATE = "HarvestCurrentDate";
        public const string HARVESTENDDATE = "HarvestEndDate";
        public const string JOBSTATUS = "JobStatus";
        public const string DATAPROVIDERID = "DataProviderId";
        public const string CHANGEID = "ChangeId";
        public const string HARVESTDATE = "HarvestDate";
    }

    /// <summary>
    /// Deleted observation table data.
    /// </summary>
    public struct DeletedObservationTableData
    {
        public const String TABLE_NAME = "DeletedSpeciesObservation";

        public const String DELETE_TABLE_NAME = "TempDeleteSpeciesObservation";

        /// <summary>
        /// Species observation catalog number.
        /// </summary>
        public const String CATALOGNUMBER = "catalogNumber";

        /// <summary>
        /// Species observation Description.
        /// </summary>
        public const String DATAPROVIDERID = "DataProviderId";

        /// <summary>
        /// Species observation Description.
        /// </summary>
        public const String OBSERVATIONID = "ObservationId";
    }
}

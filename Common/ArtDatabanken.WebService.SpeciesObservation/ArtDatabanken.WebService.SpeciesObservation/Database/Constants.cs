using System;

namespace ArtDatabanken.WebService.SpeciesObservation.Database
{
    /// <summary>
    /// Constants used when accessing region information in database.
    /// </summary>
    public struct RegionData
    {
        /// <summary>
        /// CategoryId
        /// </summary>
        public const String CATEGORY_ID = "CategoryId";

        /// <summary>
        /// Id
        /// </summary>
        public const String ID = "Id";

        /// <summary>
        /// Name
        /// </summary>
        public const String NAME = "Name";

        /// <summary>
        /// NativeId
        /// </summary>
        public const String NATIVE_ID = "NativeId";

        /// <summary>
        /// ShortName
        /// </summary>
        public const String SHORT_NAME = "ShortName";
    }

    /// <summary>
    /// Constants used when accessing species observation change information.
    /// </summary>
    public struct SpeciesObservationChangeData
    {
        /// <summary>
        /// Current change id.
        /// </summary>
        public const String CURRENT_CHANGE_ID = "CurrentChangeId";

        /// <summary>
        /// Max species observation change id.
        /// </summary>
        public const String MAX_SPECIES_OBSERVATION_CHANGE_ID = "MaxSpeciesObservationChangeId";

        /// <summary>
        /// Next change id.
        /// </summary>
        public const String NEXT_CHANGE_ID = "NextChangeId";
    }

    /// <summary>
    /// Constants used when accessing species observation
    /// information in database that is used for Elasticsearch.
    /// </summary>
    public struct SpeciesObservationElasticsearchData
    {
        /// <summary>
        /// Bird nest activity id.
        /// </summary>
        public const String BIRD_NEST_ACTIVITY_ID = "birdNestActivityId";

        /// <summary>
        /// Coordinate x RT90.
        /// </summary>
        public const String COORDINATE_X_RT90 = "coordinateX_RT90";

        /// <summary>
        /// Coordinate x SWEREF99.
        /// </summary>
        public const String COORDINATE_X_SWEREF99 = "coordinateX_SWEREF99";

        /// <summary>
        /// Coordinate y RT90.
        /// </summary>
        public const String COORDINATE_Y_RT90 = "coordinateY_RT90";

        /// <summary>
        /// Coordinate y SWEREF99.
        /// </summary>
        public const String COORDINATE_Y_SWEREF99 = "coordinateY_SWEREF99";

        /// <summary>
        /// Current index change id.
        /// </summary>
        public const String CURRENT_INDEX_CHANGE_ID = "CurrentIndexChangeId";

        /// <summary>
        /// Current index count.
        /// </summary>
        public const String CURRENT_INDEX_COUNT = "CurrentIndexCount";

        /// <summary>
        /// Current index name.
        /// </summary>
        public const String CURRENT_INDEX_NAME = "CurrentIndexName";

        /// <summary>
        /// Data provider id.
        /// </summary>
        public const String DATA_PROVIDER_ID = "DataProviderId";

        /// <summary>
        /// Disturbance radius.
        /// </summary>
        public const String DISTURBANCE_RADIUS = "disturbanceRadius";

        /// <summary>
        /// Max accuracy or disturbance radius.
        /// </summary>
        public const String MAX_ACCURACY_OR_DISTURBANCE_RADIUS = "maxAccuracyOrDisturbanceRadius";

        /// <summary>
        /// Next index change id.
        /// </summary>
        public const String NEXT_INDEX_CHANGE_ID = "NextIndexChangeId";

        /// <summary>
        /// Next index count.
        /// </summary>
        public const String NEXT_INDEX_COUNT = "NextIndexCount";

        /// <summary>
        /// Next index harvest start.
        /// </summary>
        public const String NEXT_INDEX_HARVEST_START = "NextIndexHarvestStart";

        /// <summary>
        /// Next index name.
        /// </summary>
        public const String NEXT_INDEX_NAME = "NextIndexName";

        /// <summary>
        /// Occurrance id.
        /// </summary>
        public const String OCCURRANCE_ID = "occuranceId";
    }

    /// <summary>
    /// Constants used when accessing species observation
    /// project parameters in database.
    /// </summary>
    public struct SpeciesObservationProjectParameterData
    {
        /// <summary>
        /// Class identifier.
        /// </summary>
        public const String CLASS_IDENTIFIER = "class";

        /// <summary>
        /// Data type.
        /// </summary>
        public const String DATA_TYPE = "type";

        /// <summary>
        /// Property identifier.
        /// </summary>
        public const String PROPERTY_IDENTIFIER = "property";

        /// <summary>
        /// Species observation id.
        /// </summary>
        public const String SPECIES_OBSERVATION_ID = "observationId";

        /// <summary>
        /// Species observation ids.
        /// </summary>
        public const String SPECIES_OBSERVATION_IDS = "SpeciesObservationIdTable";

        /// <summary>
        /// Unit is in data.
        /// </summary>
        public const String UNIT = "unit";

        /// <summary>
        /// Value of the data.
        /// </summary>
        public const String VALUE = "value";
    }

    /// <summary>
    /// Constants used when accessing species observation
    /// log information in database.
    /// </summary>
    public struct TaxonInformationData
    {
        /// <summary>
        /// Action plan.
        /// </summary>
        public const String ACTION_PLAN = "actionPlan";

        /// <summary>
        /// Class name.
        /// </summary>
        public const String CLASS = "class";

        /// <summary>
        /// Conservation relevant.
        /// </summary>
        public const String CONSERVATION_RELEVANT = "conservationRelevant";

        /// <summary>
        /// Current dyntaxa taxon id.
        /// </summary>
        public const String CURRENT_DYNTAXA_TAXON_ID = "currentDyntaxaTaxonId";

        /// <summary>
        /// Dyntaxa taxon id.
        /// </summary>
        public const String DYNTAXA_TAXON_ID = "dyntaxaTaxonId";

        /// <summary>
        /// Family name.
        /// </summary>
        public const String FAMILY = "family";

        /// <summary>
        /// Genus name.
        /// </summary>
        public const String GENUS = "genus";

        /// <summary>
        /// Higher classification.
        /// </summary>
        public const String HIGHER_CLASSIFICATION = "higherClassification";

        /// <summary>
        /// Infraspecific epithet.
        /// </summary>
        public const String INFRASPECIFIC_EPITHET = "infraspecificEpithet";

        /// <summary>
        /// Is valid.
        /// </summary>
        public const String IS_VALID = "isValid";

        /// <summary>
        /// Kingdom name.
        /// </summary>
        public const String KINGDOM = "kingdom";

        /// <summary>
        /// Name according to.
        /// </summary>
        public const String NAME_ACCORDING_TO = "nameAccordingTo";

        /// <summary>
        /// Name according to id.
        /// </summary>
        public const String NAME_ACCORDING_TO_ID = "nameAccordingToId";

        /// <summary>
        /// Name published in.
        /// </summary>
        public const String NAME_PUBLISHED_IN = "namePublishedIn";

        /// <summary>
        /// Name published in id.
        /// </summary>
        public const String NAME_PUBLISHED_IN_ID = "namePublishedInId";

        /// <summary>
        /// Name published in year.
        /// </summary>
        public const String NAME_PUBLISHED_IN_YEAR = "namePublishedInYear";

        /// <summary>
        /// Natura 2000.
        /// </summary>
        public const String NATURA_2000 = "natura2000";

        /// <summary>
        /// Nomenclatural code.
        /// </summary>
        public const String NOMENCLATURAL_CODE = "nomenclaturalCode";

        /// <summary>
        /// Nomenclatural status.
        /// </summary>
        public const String NOMENCLATURAL_STATUS = "nomenclaturalStatus";

        /// <summary>
        /// Order name.
        /// </summary>
        public const String ORDER = "order";

        /// <summary>
        /// Organism group.
        /// </summary>
        public const String ORGANISM_GROUP = "organismGroup";

        /// <summary>
        /// Original name usage.
        /// </summary>
        public const String ORIGINAL_NAME_USAGE = "originalNameUsage";

        /// <summary>
        /// Original name usage id.
        /// </summary>
        public const String ORIGINAL_NAME_USAGE_ID = "originalNameUsageId";

        /// <summary>
        /// Phylum name.
        /// </summary>
        public const String PHYLUM = "phylum";

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
        /// Scientific name.
        /// </summary>
        public const String SCIENTIFIC_NAME = "scientificName";

        /// <summary>
        /// Scientific name authorship.
        /// </summary>
        public const String SCIENTIFIC_NAME_AUTHORSHIP = "scientificNameAuthorship";

        /// <summary>
        /// Scientific name id.
        /// </summary>
        public const String SCIENTIFIC_NAME_ID = "scientificNameId";

        /// <summary>
        /// Species taxon id.
        /// </summary>
        public const String SPECIES_TAXON_ID = "speciesTaxonId";

        /// <summary>
        /// Specific epithet.
        /// </summary>
        public const String SPECIFIC_EPITHET = "specificEpithet";

        /// <summary>
        /// Subgenus name.
        /// </summary>
        public const String SUBGENUS = "subgenus";

        /// <summary>
        /// Swedish immigration history.
        /// </summary>
        public const String SWEDISH_IMMIGRATION_HISTORY = "swedishImmigrationHistory";

        /// <summary>
        /// Swedish occurrence.
        /// </summary>
        public const String SWEDISH_OCCURRENCE = "swedishOccurrence";

        /// <summary>
        /// Taxon category id.
        /// </summary>
        public const String TAXON_CATEGORY_ID = "taxonCategoryId";

        /// <summary>
        /// Taxon concept id.
        /// </summary>
        public const String TAXON_CONCEPT_ID = "taxonConceptId";

        /// <summary>
        /// Taxon concept status.
        /// </summary>
        public const String TAXON_CONCEPT_STATUS = "taxonConceptStatus";

        /// <summary>
        /// Taxonomic status.
        /// </summary>
        public const String TAXONOMIC_STATUS = "taxonomicStatus";

        /// <summary>
        /// Taxon rank.
        /// </summary>
        public const String TAXON_RANK = "taxonRank";

        /// <summary>
        /// Taxon remark.
        /// </summary>
        public const String TAXON_REMARK = "taxonRemark";

        /// <summary>
        /// Taxon sort order.
        /// </summary>
        public const String TAXON_SORT_ORDER = "taxonSortOrder";

        /// <summary>
        /// Vernacular name.
        /// </summary>
        public const String VERNACULAR_NAME = "vernacularName";
    }
}

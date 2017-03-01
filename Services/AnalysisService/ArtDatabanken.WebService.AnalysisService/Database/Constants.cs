using System;

namespace ArtDatabanken.WebService.AnalysisService.Database
{
    /// <summary>
    /// Constants used when accessing species observations and using grid cells.
    /// </summary>     
    public struct ObservationGridCellSearchCriteriaData
    {
        /// <summary>
        /// Grid cell size. 
        /// </summary>
        // ReSharper disable once InconsistentNaming
       public const String GRID_CELL_SIZE = "gridCellSize";

        /// <summary>
        /// Grid cell coordinate system. 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const String GRID_COORDINATE_SYSTEM = "gridCoordinateSystem ";
        
        /// <summary>
        /// Grid coordinate X.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const String GRID_CELL_COORDINATE_X = "centreCoordinateX";

        /// <summary>
        /// Grid coordinate Y.
        /// </summary>
        // ReSharper disable InconsistentNaming
        public const String GRID_CELL_COORDINATE_Y = "centreCoordinateY";
        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Species observation count.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const String SPECIES_OBSERVATION_COUNT = "SpeciesObservationCount";

        /// <summary>
        /// Species count.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const String SPECIES_COUNT = "SpeciesCount";
    }

    /// <summary>
    /// Constants used when accessing species observations and using time steps.
    /// </summary>
    public struct TimeSpeciesObservationCountData
    {
        /// <summary>
        /// Periodicity type.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const String PERIODICITY = "Periodicity";

        /// <summary>
        /// Species Observation Count.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const String SPECIES_OBSERVATION_COUNT = "Count";

        /// <summary>
        /// Species Count.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const String SPECIES_COUNT = "SpeciesCount";

        /// <summary>
        /// Year definition.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const String YEAR = "Year";

        /// <summary>
        /// Month definition.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const String MONTH = "Month";

        /// <summary>
        /// Week definition.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const String WEEK = "Week";

        /// <summary>
        /// Day definition.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const String DAY = "Day";
    }

    /// <summary>
    /// Constants used when accessing taxon search criteria by ids.
    /// </summary>
    public struct TaxonIdsSearchCriteria
    {
        /// <summary>
        /// TaxonId definition.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const String TAXON_ID = "TaxonId";

        /// <summary>
        /// Species Observation Count.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const String SPECIES_OBSERVATION_COUNT = "SpeciesObservationCount";
    }

    /// <summary>
    /// Constants used when accessing species observations provenance.
    /// </summary>     
    public struct SpeciesObservationProvenanceSearchCriteriaData
    {
        /// <summary>
        /// Name of the provenance property.
        /// </summary>
        public const String NAME = "Name";

        /// <summary>
        /// Value of the provenance property.
        /// </summary>
        public const String VALUE = "Value";

        /// <summary>
        /// Unique id of the provenance property.
        /// </summary>
        public const String ID = "Id";

        /// <summary>
        /// Amount of species observations related to the provenance property.
        /// </summary>
        public const String SPECIES_OBSERVATION_COUNT = "SpeciesObservationCount";

        /// <summary>
        /// Defines of id is specified or not for to the provenance property.
        /// </summary>
        public const String ID_IS_SPECIFIED = "IdIsSpecified";
    }
}

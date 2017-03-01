using System;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Enum that contains possible values for taxon county occurrence.
    /// </summary>
    public enum TaxonCountyOccurrenceId
    {
        /// <summary>
        /// Missing, no know occurrence of the species in this county.
        /// </summary>
        Missing = 0,
        /// <summary>
        /// Uncertain, the species may occure in the county.
        /// </summary>
        Uncertain = 1,
        /// <summary>
        /// Temporary, the species occures temporary in the county.
        /// </summary>
        Temporary = 2,
        /// <summary>
        /// Extinct, the species has occured in the county
        /// but it does not occure in the county today.
        /// </summary>
        Extinct = 3,
        /// <summary>
        /// Resident, the species occures in the county.
        /// </summary>
        Resident = 4
    }

    /// <summary>
    /// Enum that contains possible values for taxon county occurrence.
    /// </summary>
    public struct TaxonCountyOccurrenceString
    {
        /// <summary>
        /// Missing, no know occurrence of the species in this county.
        /// </summary>
        public const String MISSING = "S";
        /// <summary>
        /// Uncertain, the species may occure in the county.
        /// </summary>
        public const String UNCERTAIN = "O";
        /// <summary>
        /// Temporary, the species occures temporary in the county.
        /// </summary>
        public const String TEMPORARY = "T";
        /// <summary>
        /// Extinct, the species has occured in the county
        /// but it does not occure in the county today.
        /// </summary>
        public const String EXTINCT = "U";
        /// <summary>
        /// Resident, the species occures in the county.
        /// </summary>
        public const String RESIDENT = "B";
    }

    /// <summary>
    ///  This class represents a taxon.
    /// </summary>
    [Serializable]
    public class TaxonCountyOccurrence
    {
        private const Int32 DEFAULT_ART_DATA_ID = 0;
        private const Int32 DEFAULT_SOURCE_ID = 0;

        private Boolean _hasArtDataId;
        private Boolean _hasSourceId;
        private County _county;
        private Int32 _artDataId;
        private Int32 _sourceId;
        private String _countyOccurrence;
        private String _originalCountyOccurrence;
        private String _source;
        private Taxon _taxon;

        /// <summary>
        /// Create a TaxonCountyOccurrence instance.
        /// </summary>
        /// <param name='taxon'>Taxon.</param>
        /// <param name='countyId'>County id.</param>
        /// <param name='countyOccurrence'>County occurrence.</param>
        /// <param name='hasSourceId'>Indicates if sourceId has a value.</param>
        /// <param name='sourceId'>Source id.</param>
        /// <param name='source'>Source.</param>
        /// <param name='hasArtDataId'>Indicates if artDataId has a value.</param>
        /// <param name='artDataId'>Art data id.</param>
        /// <param name='originalCountyOccurrence'>Original county occurrence.</param>
        public TaxonCountyOccurrence(Taxon taxon,
                                     Int32 countyId,
                                     String countyOccurrence,
                                     Boolean hasSourceId,
                                     Int32 sourceId,
                                     String source,
                                     Boolean hasArtDataId,
                                     Int32 artDataId,
                                     String originalCountyOccurrence)
        {
            _taxon = taxon;
            _county = GeographicManager.GetCounty(countyId);
            _countyOccurrence = countyOccurrence;
            _hasSourceId = hasSourceId;
            if (_hasSourceId)
            {
                _sourceId = sourceId;
            }
            else
            {
                _sourceId = DEFAULT_SOURCE_ID;
            }
            _source = source;
            _hasArtDataId = hasArtDataId;
            if (_hasArtDataId)
            {
                _artDataId = artDataId;
            }
            else
            {
                _artDataId = DEFAULT_ART_DATA_ID;
            }
            _originalCountyOccurrence = originalCountyOccurrence;
        }

        /// <summary>
        /// Create a TaxonCountyOccurrence instance.
        /// </summary>
        /// <param name='taxon'>Taxon.</param>
        /// <param name='countyIdentifier'>County identifier.</param>
        /// <param name='countyOccurrence'>County occurrence.</param>
        /// <param name='originalCountyOccurrence'>Original county occurrence.</param>
        public TaxonCountyOccurrence(Taxon taxon,
                                     String countyIdentifier,
                                     String countyOccurrence,
                                     String originalCountyOccurrence)
        {
            _taxon = taxon;
            _county = GeographicManager.GetCounty(countyIdentifier);
            _countyOccurrence = countyOccurrence;
            _hasSourceId = false;
            _sourceId = DEFAULT_SOURCE_ID;
            _source = null;
            _hasArtDataId = false;
            _artDataId = DEFAULT_ART_DATA_ID;
            _originalCountyOccurrence = originalCountyOccurrence;
        }

        /// <summary>
        /// Get art data id.
        /// </summary>
        public Int32 ArtDataId
        {
            get { return _artDataId; }
        }

        /// <summary>
        /// Get county which this county
        /// occurrence information belongs to.
        /// </summary>
        public County County
        {
            get { return _county; }
        }

        /// <summary>
        /// Get county occurrence information.
        /// </summary>
        public String CountyOccurrence
        {
            get { return _countyOccurrence; }
        }

        /// <summary>
        /// Test if art data id has a value.
        /// </summary>
        public Boolean HasArtDataId
        {
            get { return _hasArtDataId; }
        }

        /// <summary>
        /// Test if source id has a value.
        /// </summary>
        public Boolean HasSourceId
        {
            get { return _hasSourceId; }
        }

        /// <summary>
        /// Get original county occurrence information.
        /// </summary>
        public String OriginalCountyOccurrence
        {
            get { return _originalCountyOccurrence; }
        }

        /// <summary>
        /// Get source of the county occurrence information.
        /// </summary>
        public String Source
        {
            get { return _source; }
        }

        /// <summary>
        /// Get id for source of the county occurrence information.
        /// </summary>
        public Int32 SourceId
        {
            get { return _sourceId; }
        }

        /// <summary>
        /// Get taxon which this county
        /// occurrence information belongs to.
        /// </summary>
        public Taxon Taxon
        {
            get { return _taxon; }
        }
    }
}

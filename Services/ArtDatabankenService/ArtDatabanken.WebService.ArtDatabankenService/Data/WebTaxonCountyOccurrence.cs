using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    ///  This class represents information about county occurrence
    ///  for a taxon.
    /// </summary>
    [DataContract]
    public class WebTaxonCountyOccurrence : WebData
    {
        private const Double DEFAULT_ART_DATA_ID = 0;
        private const Double DEFAULT_SOURCE_ID = 0;

        /// <summary>
        /// Create a WebTaxonCountyOccurrence instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebTaxonCountyOccurrence(DataReader dataReader)
        {
            TaxonId = dataReader.GetInt32(TaxonCountyOccurrenceData.TAXON_ID);
            CountyId = dataReader.GetInt32(TaxonCountyOccurrenceData.COUNTY_ID);
            CountyOccurrence = dataReader.GetString(TaxonCountyOccurrenceData.COUNTY_OCCURRENCE);
            IsSourceIdSpecified = dataReader.IsNotDBNull(TaxonCountyOccurrenceData.SOURCE_ID);
            SourceId = (Int32)(dataReader.GetDouble(TaxonCountyOccurrenceData.SOURCE_ID, DEFAULT_SOURCE_ID));
            Source = dataReader.GetString(TaxonCountyOccurrenceData.SOURCE);
            IsArtDataIdSpecified = dataReader.IsNotDBNull(TaxonCountyOccurrenceData.ART_DATA_ID);
            ArtDataId = (Int32)(dataReader.GetDouble(TaxonCountyOccurrenceData.ART_DATA_ID, DEFAULT_ART_DATA_ID));
            OriginalCountyOccurrence = dataReader.GetString(TaxonCountyOccurrenceData.ORIGINAL_COUNTY_OCCURRENCE);
            base.LoadData(dataReader);
        }

        /// <summary>
        /// Art data id.
        /// </summary>
        [DataMember]
        public Int32 ArtDataId
        { get; set; }

        /// <summary>
        /// Id for county which this county
        /// occurrence information belongs to.
        /// </summary>
        [DataMember]
        public Int32 CountyId
        { get; set; }

        /// <summary>
        /// County occurrence information.
        /// </summary>
        [DataMember]
        public String CountyOccurrence
        { get; set; }

        /// <summary>
        /// Test if art data id has a value.
        /// </summary>
        [DataMember]
        public Boolean IsArtDataIdSpecified
        { get; set; }

        /// <summary>
        /// Test if source id has a value.
        /// </summary>
        [DataMember]
        public Boolean IsSourceIdSpecified
        { get; set; }

        /// <summary>
        /// Original county occurrence information.
        /// </summary>
        [DataMember]
        public String OriginalCountyOccurrence
        { get; set; }

        /// <summary>
        /// Source of the county occurrence information.
        /// </summary>
        [DataMember]
        public String Source
        { get; set; }

        /// <summary>
        /// Id for source of the county occurrence information.
        /// </summary>
        [DataMember]
        public Int32 SourceId
        { get; set; }

        /// <summary>
        /// Id for taxon which this county
        /// occurrence information belongs to.
        /// </summary>
        [DataMember]
        public Int32 TaxonId
        { get; set; }
    }
}

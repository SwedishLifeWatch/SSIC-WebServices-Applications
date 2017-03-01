using System;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.SpeciesObservation.Database;

namespace ArtDatabanken.WebService.SpeciesObservation.Data
{
    /// <summary>
    /// This class handles information related to Elasticsearch and species observations.
    /// </summary>
    public class SpeciesObservationElasticsearch
    {
        /// <summary>
        /// Alias used to retrieve species observation information in Elasticsearch.
        /// </summary>
        public String CurrentIndexAlias { get; set; }

        /// <summary>
        /// Max species observation change id that has been processed to Elasticsearch.
        /// </summary>
        public Int64 CurrentIndexChangeId { get; set; }

        /// <summary>
        /// Number of species observations in current index in Elasticsearch.
        /// </summary>
        public Int64 CurrentIndexCount { get; set; }

        /// <summary>
        /// Name of current index in Elasticsearch.
        /// </summary>
        public String CurrentIndexName { get; set; }

        /// <summary>
        /// Max species observation change id that has been
        /// processed into next index in Elasticsearch.
        /// </summary>
        public Int64? NextIndexChangeId { get; set; }

        /// <summary>
        /// Number of species observations in next index in Elasticsearch.
        /// </summary>
        public Int64? NextIndexCount { get; set; }

        /// <summary>
        /// Date when the harvest started -1 day.
        /// This value is used to avoid handling old delete of species observations.
        /// </summary>
        public DateTime? NextIndexHarvestStart { get; set; }

        /// <summary>
        /// Name of next index in Elasticsearch.
        /// </summary>
        public String NextIndexName { get; set; }

        /// <summary>
        /// Populate taxon information object with content from data reader.
        /// </summary>
        /// <param name="dataReader">Data source that has content.</param>
        public void LoadData(DataReader dataReader)
        {
            // Taxon information.
            CurrentIndexChangeId = dataReader.GetInt64(SpeciesObservationElasticsearchData.CURRENT_INDEX_CHANGE_ID);
            CurrentIndexCount = dataReader.GetInt64(SpeciesObservationElasticsearchData.CURRENT_INDEX_COUNT);
            CurrentIndexName = dataReader.GetString(SpeciesObservationElasticsearchData.CURRENT_INDEX_NAME);
            if (dataReader.IsNotDbNull(SpeciesObservationElasticsearchData.NEXT_INDEX_CHANGE_ID))
            {
                NextIndexChangeId = dataReader.GetInt64(SpeciesObservationElasticsearchData.NEXT_INDEX_CHANGE_ID);
            }

            if (dataReader.IsNotDbNull(SpeciesObservationElasticsearchData.NEXT_INDEX_COUNT))
            {
                NextIndexCount = dataReader.GetInt64(SpeciesObservationElasticsearchData.NEXT_INDEX_COUNT);
            }

            if (dataReader.IsNotDbNull(SpeciesObservationElasticsearchData.NEXT_INDEX_HARVEST_START))
            {
                NextIndexHarvestStart = dataReader.GetDateTime(SpeciesObservationElasticsearchData.NEXT_INDEX_HARVEST_START);
            }
            else
            {
                NextIndexHarvestStart = null;
            }

            if (dataReader.IsNotDbNull(SpeciesObservationElasticsearchData.NEXT_INDEX_NAME))
            {
                NextIndexName = dataReader.GetString(SpeciesObservationElasticsearchData.NEXT_INDEX_NAME);
            }
            else
            {
                NextIndexName = null;
            }
        }
    }
}

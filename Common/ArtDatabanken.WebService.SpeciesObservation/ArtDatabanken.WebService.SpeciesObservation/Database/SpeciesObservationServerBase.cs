using System;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Database;
using SqlCommandBuilder = ArtDatabanken.Database.SqlCommandBuilder;

namespace ArtDatabanken.WebService.SpeciesObservation.Database
{
    /// <summary>
    /// Database interface for the swedish species observation database.
    /// </summary>
    public abstract class SpeciesObservationServerBase : WebServiceDataServer
    {
        /// <summary>
        /// Get next higher species observation change id that is larger than current change id.
        /// If no higher change id exists then currentChangeId is returned.
        /// </summary>
        /// <param name="currentChangeId">Current change id.</param>
        /// <param name="nextIndexHarvestStart">
        /// Start date of harvest.
        /// This value is used to avoid handling old delete of species observations.
        /// </param>
        /// <returns>Next higher species observation change id that is larger than current change id.</returns>
        public Int64 GetNextChangeId(Int64 currentChangeId,
                                     DateTime? nextIndexHarvestStart)
        {
            Int64 nextChangeId;

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetNextChangeId");
            commandBuilder.AddParameter(SpeciesObservationChangeData.CURRENT_CHANGE_ID, currentChangeId);
            if (nextIndexHarvestStart.HasValue)
            {
                commandBuilder.AddParameter(SpeciesObservationElasticsearchData.NEXT_INDEX_HARVEST_START, nextIndexHarvestStart.Value);
            }

            using (DataReader dataReader = GetReader(commandBuilder))
            {
                if (dataReader.Read())
                {
                    nextChangeId = dataReader.GetInt64(SpeciesObservationChangeData.NEXT_CHANGE_ID);
                }
                else
                {
                    throw new ApplicationException("Next species observation change id was not found.");
                }
            }

            return nextChangeId;
        }

        /// <summary>
        /// Get information related to Elasticsearch and species observations.
        /// </summary>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetSpeciesObservationElasticsearch()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetSpeciesObservationElasticsearch");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get information about all taxa.
        /// </summary>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonInformation()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetTaxon");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Test if species observation information are being updated.
        /// </summary>
        /// <returns>True, if species observation information are being updated.</returns>
        public Boolean IsDatabaseUpdated()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("IsDatabaseUpdated");
            return ExecuteScalar(commandBuilder) > 0;
        }

        /// <summary>
        /// Update information related to Elasticsearch and species observations.
        /// </summary>
        /// <param name="currentIndexAlias">Alias used to retrieve species observation information in Elasticsearch.</param>
        /// <param name="currentIndexChangeId">Max species observation change id that has been processed to Elasticsearch.</param>
        /// <param name="currentIndexCount">Number of species observations in current index in Elasticsearch.</param>
        /// <param name="currentIndexName">Name of current index in Elasticsearch.</param>
        /// <param name="nextIndexChangeId">
        /// Max species observation change id that has been
        /// processed into next index in Elasticsearch.
        /// </param>
        /// <param name="nextIndexCount">Number of species observations in next index in Elasticsearch.</param>
        /// <param name="nextIndexName">Name of next index in Elasticsearch.</param>
        public void UpdateSpeciesObservationElasticsearch(Int64? currentIndexChangeId,
                                                          Int64? currentIndexCount,
                                                          String currentIndexName,
                                                          Int64? nextIndexChangeId,
                                                          Int64? nextIndexCount,
                                                          String nextIndexName)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("UpdateSpeciesObservationElasticsearch");
            if (currentIndexChangeId.HasValue)
            {
                commandBuilder.AddParameter(SpeciesObservationElasticsearchData.CURRENT_INDEX_CHANGE_ID, currentIndexChangeId.Value);
            }

            if (currentIndexCount.HasValue)
            {
                commandBuilder.AddParameter(SpeciesObservationElasticsearchData.CURRENT_INDEX_COUNT, currentIndexCount.Value);
            }

            if (currentIndexName.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationElasticsearchData.CURRENT_INDEX_NAME, currentIndexName);
            }

            if (nextIndexChangeId.HasValue)
            {
                commandBuilder.AddParameter(SpeciesObservationElasticsearchData.NEXT_INDEX_CHANGE_ID, nextIndexChangeId.Value);
            }

            if (nextIndexCount.HasValue)
            {
                commandBuilder.AddParameter(SpeciesObservationElasticsearchData.NEXT_INDEX_COUNT, nextIndexCount.Value);
            }

            if (nextIndexName.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationElasticsearchData.NEXT_INDEX_NAME, nextIndexName);
            }

            ExecuteCommand(commandBuilder);
        }
    }
}

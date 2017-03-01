using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Database;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using SqlCommandBuilder = ArtDatabanken.Database.SqlCommandBuilder;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Database
{
    /// <summary>
    /// Database interface for the swedish species observation database.
    /// </summary>
    public class SpeciesObservationHarvestServer : HarvestBaseServer
    {
        /// <summary>
        /// Add data from a DataTable to database.
        /// Used by HarvestManager UpdateTempTaxonInformation.
        /// </summary>
        /// <param name="context">
        /// Web service request context.
        /// </param>
        /// <param name="dataTable">
        /// Table with information.
        /// </param>
        public void AddTableData(WebServiceContext context, DataTable dataTable)
        {
            AddTableData(dataTable);
        }

        /// <summary>
        /// Adds rows in the TempDeleteSpeciesObservation based on the difference between the AllSourceSpeciesObservationIds and DarwinCoreObservation tables
        /// </summary>
        /// <param name="dataProviderId"></param>
        /// <returns></returns>
        public void AddTempDeleteSpeciesObservation(int dataProviderId)
        {
            var list = new List<string>();
            var commandBuilder = new SqlCommandBuilder("AddTempDeleteSpeciesObservation");
            commandBuilder.AddParameter("dataProviderId", dataProviderId);

            ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Check that TaxonId is valid in Temp table otherwise write to error log.
        /// Used by HarvestManager (UpdateSpeciesObservations).
        /// </summary>
        /// <returns>
        /// Current max value for species observation ids.
        /// </returns>
        public int CheckTaxonIdOnTemp()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("CheckTaxonIdOnTemp");
            return ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Remove rows from table LogUpdateError by those observations located in table DarwinCoreObservation.
        /// Used by HarvestManager (UpdateSpeciesObservations), after a successful harvest.
        /// </summary>
        /// <returns>
        /// The status of the query.
        /// </returns>
        public int CleanLogUpdateError()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("CleanLogUpdateError");
            return ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Remove old duplicate rows from table LogUpdateError for observations that has errors.
        /// Used by HarvestManager (UpdateSpeciesObservations), after a successful harvest.
        /// </summary>
        /// <returns>
        /// The status of the query.
        /// </returns>
        public int CleanLogUpdateErrorDuplicates()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("CleanLogUpdateErrorDuplicates");
            return ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Add removed observations to DeletedSpeciesObservation
        /// Cleans up SpeciesObservationField and DarwinCoreObservation
        /// Used by HarvestManager (UpdateSpeciesObservations).
        /// </summary>
        /// <returns>
        /// The status of the query.
        /// </returns>
        public int CopyDeleteToSpeciesObservation()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("CopyDeleteToSpeciesObservation");
            return ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Copies the TaxonRemark information from the Taxon table to the TempTaxon table (previously done as the first step in the CopyTempToTaxon sql procedure)
        /// Used by HarvestManager (UpdateTaxonInformation).
        /// </summary>
        /// <param name="context">
        /// The web service context.
        /// </param>
        /// <returns>
        /// The status of the query.<see cref="int"/>.
        /// </returns>
        public int UpdateRemarkInTempTaxonFromTaxon(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("UpdateRemarkInTempTaxonFromTaxon");
            return ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Adds rows to the SpeciesObservationChange table (for later update of ElasticSearch).
        /// Runs in two different steps where the last step adds rows in batches, either on the server or the client side, can be used for performance tuning
        /// Used by HarvestManager. 
        /// </summary>
        /// <param name="batchSize">The number of rows that should be added in each batch, can be used for performance tuning</param>
        /// <param name="loopOnServerSide">If set to true all processing of all batches is done on the server side.
        ///  If set to false each batch is processed using a new connection to the database. This can be used for performance tuning.</param>
        /// <returns>The number of change (affected) DarwinCoreObservations.<see cref="int"/> </returns>
        public int AddSpeciesObservationChangeForElasticSearch(int batchSize, bool loopOnServerSide)
        {
            var observationCount = AddTempSpeciesObservationChange();
            AddSpeciesObservationChange(batchSize, loopOnServerSide);
            return observationCount;
        }

        /// <summary>
        /// step 1(2)
        /// Used by HarvestManager (Updates SpeciesObservationChange for ElasticSearch)
        /// </summary>
        /// <returns>The number of rows inserted in the temp table.<see cref="int"/>.
        /// </returns>
        private int AddTempSpeciesObservationChange()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("AddTempSpeciesObservationChange");
            return ExecuteScalar(commandBuilder);
        }

        /// <summary>
        /// step 2(2)
        /// Used by HarvestManager (Updates SpeciesObservationChange for ElasticSearch)
        /// </summary>
        /// <param name="context"> The web service context. </param>
        /// <param name="batchSize">The number of rows that should be added in each batch, used for performance tuning</param>
        /// <param name="loopOnServerSide">If set to true all processing of all batches is done on the server side. If set to false each batch is processed using a new connection to the database. This is used to enable performance tuning.</param>
        /// <returns>
        /// The status of the query.<see cref="int"/>.
        /// </returns>
        private int AddSpeciesObservationChange(int batchSize, bool loopOnServerSide)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("AddSpeciesObservationChange");
            commandBuilder.AddParameter("BatchSize", batchSize);
            commandBuilder.AddParameter("LoopOnServerSide", loopOnServerSide);
            if (loopOnServerSide)
            {
                return ExecuteCommand(commandBuilder);
            }
            else
            {
                var rowsToProcess = 1;
                while (rowsToProcess > 0)
                {
                    //TODO: add race protection
                    rowsToProcess = ExecuteScalar(commandBuilder);
                    //Call DeleteUnnecessaryChanges in each batch for performance reasons
                    DeleteUnnecessaryChanges();
                }
                return 0; //TODO: is this ok?
            }
        }

        /// <summary>
        /// Copy from TempTaxon till Taxon
        /// Used by HarvestManager (UpdateTaxonInformation).
        /// </summary>
        /// <param name="context">
        /// The web service context.
        /// </param>
        /// <returns>
        /// The status of the query.<see cref="int"/>.
        /// </returns>
        public int CopyTempToTaxon(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("CopyTempToTaxon");
            return ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Clean and copy from TempTaxonTree to TaxonTree
        /// Used by HarvestManager (UpdateTaxonInformation).
        /// </summary>
        /// <param name="context">
        /// The web service context.
        /// </param>
        /// <returns>
        /// The status of the query. <see cref="int"/>.
        /// </returns>
        public int CopyTempToTaxonTree(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("CopyTempToTaxonTree");
            return ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Creates a PointGoogleMercator from CoordinateX and CoordinateY
        /// Used by HarvestManager (UpdateSpeciesObservations).
        /// </summary>
        /// <returns>
        /// The status of the query.
        /// </returns>
        public int CreatePointGoogleMercatorInTemp()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("CreatePointGoogleMercatorInTemp");
            return ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Delete unnecessary species observation change information.
        /// </summary>
        public void DeleteUnnecessaryChanges()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("DeleteUnnecessaryChanges");
            ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Remove all rows from temporary Elasticsearch tables.
        /// </summary>
        public void EmptyTempElasticsearchTables()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("EmptyTempElasticsearchTables");
            ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Truncates TempTables before harvest
        /// Used by HarvestManager (UpdateSpeciesObservations).
        /// </summary>
        /// <returns>
        /// The status of the query.
        /// </returns>
        public int EmptyTempTables()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("EmptyTempTables");
            return ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Clean tables TempTaxon, TempTaxonTree, TempArtFakta.
        /// </summary>
        /// <param name="context">
        /// The web service context.
        /// </param>
        /// <returns>
        /// The status of the query. <see cref="int"/>.
        /// </returns>
        public int EmptyTempTaxonTables(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("EmptyTempTaxonTables");
            return ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Get difference between Elasticsearch and SQL Server.
        /// </summary>
        /// <returns>Information about changed species observations.</returns>
        public DataReader GetSpeciesObservationDifference()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetSpeciesObservationDifference");
            return GetReader(commandBuilder, CommandBehavior.Default);
        }

        /// <summary>
        /// Get information about species observations that has
        /// changed.
        /// 
        /// Scope is restricted to those observations that the
        /// user has access rights to. There is no access right
        /// check on deleted species observations. This means
        /// that a client can obtain information about deleted
        /// species observations that the client has not
        /// received any create or update information about.
        /// </summary>
        /// <param name="changeId">
        /// Start id for changes that should be returned.
        /// The species observation that is changed in the
        /// specified change id may be included in returned
        /// information.
        /// </param>
        /// <param name="maxReturnedChanges">Max number of changes to return.</param>
        /// <param name="nextIndexHarvestStart">
        /// Start date of harvest.
        /// This value is used to avoid handling old delete of species observations.
        /// </param>
        /// <returns>Information about changed species observations.</returns>
        public DataReader GetSpeciesObservationChangesElasticsearch(long changeId,
                                                                    long maxReturnedChanges,
                                                                    DateTime? nextIndexHarvestStart)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetSpeciesObservationChangesElasticsearch");

            commandBuilder.AddParameter(SpeciesObservationElasticsearchData.CHANGE_ID, changeId);
            commandBuilder.AddParameter(SpeciesObservationElasticsearchData.IS_DEBUG, Configuration.Debug);
            commandBuilder.AddParameter(SpeciesObservationElasticsearchData.MAX_RETURNED_CHANGES, maxReturnedChanges);
            if (nextIndexHarvestStart.HasValue)
            {
                commandBuilder.AddParameter(SpeciesObservation.Database.SpeciesObservationElasticsearchData.NEXT_INDEX_HARVEST_START, nextIndexHarvestStart.Value);
            }

            return GetReader(commandBuilder, CommandBehavior.Default);
        }

        /// <summary>
        /// Used By GetTaxon in HarvestManager. 
        /// </summary>
        /// <returns> A Taxon data reader.
        /// </returns>
        public DataReader GetTaxon()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetTaxon");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get species observations by ids in a format that
        /// is adjusted to the needs of Elasticsearch.
        /// </summary>
        /// <param name="speciesObservationIds">Species observation ids.</param>
        /// <returns>Information about species observations.</returns>
        public DataReader GetSpeciesObservationsByIdElasticsearch(List<Int64> speciesObservationIds)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetSpeciesObservationsByIdElasticsearch", true);
            commandBuilder.AddParameter("SpeciesObservationIdTable", speciesObservationIds);
            commandBuilder.AddParameter(SpeciesObservationElasticsearchData.IS_DEBUG, Configuration.Debug);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Used By GetTempTaxon in HarvestManager.
        /// </summary>
        /// <returns>
        /// A temp Taxon data reader.
        /// </returns>
        public DataReader GetTempTaxon()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetTempTaxon");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Used by Get Temp Taxon Tree List in HarvestManager.
        /// </summary>
        /// <returns> A temp Taxon tree data reader.
        /// </returns>
        public DataReader GetTempTaxonTree()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetTempTaxonTree");
            return GetReader(commandBuilder);
        }

        public DataReader GetWeekOfYear(DateTime date)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetWeekOfYear");
            commandBuilder.AddParameter("Date", date);
            return GetReader(commandBuilder);
        }


        /// <summary>
        /// Returns a list of id's with all differences between the tables DarwinCoreObservation and AllSourceSpeciesObservationIds that has a suggestedAction of INSERT
        /// </summary>
        /// <param name="dataProviderId"></param>
        /// <returns></returns>
        public List<string> GetAllSourceSpeciesObservationIdsForInsert(int dataProviderId)
        {
            var list = new List<string>();
            var commandBuilder = new SqlCommandBuilder("GetAllSourceSpeciesObservationIds");
            commandBuilder.AddParameter("dataProviderId", dataProviderId);
            commandBuilder.AddParameter("suggestedAction", "INSERT");

            using (var reader = GetReader(commandBuilder))
            {
                while (reader.Read())
                {
                    list.Add(reader.GetString(0));
                }
            }
            return list;
        }

        /// <summary>
        /// Add information to the web service log.
        /// </summary>
        /// <param name="context">
        /// Web service request context.
        /// </param>
        /// <param name="dataProvider">
        /// Log text.
        /// </param>
        /// <param name="type">
        /// Type of log entry.
        /// </param>
        /// <param name="description">
        /// Extended information about the log entry.
        /// </param>
        /// <param name="observationId">
        /// Id of the observation.
        /// </param>
        public virtual void Log(WebServiceContext context,
                                String dataProvider,
                                LogType type,
                                String description,
                                String observationId)
        {
            context.GetSpeciesObservationDatabase().UpdateLog(dataProvider,
                      type,
                      description,
                      observationId);
        }

        /// <summary>
        /// Add information to the web service log.
        /// </summary>
        /// <param name="context">
        /// Web service request context.
        /// </param>
        /// <param name="action">
        /// The Action to log.
        /// </param>
        /// <param name="processtime">
        /// How long time did the process take.
        /// </param>
        /// <param name="message">
        /// The message to log.
        /// </param>
        public virtual void LogHarvestMove(WebServiceContext context,
                                string action,
                                long processtime,
                                string message)
        {
            context.GetSpeciesObservationDatabase().WriteToLogStatistics(
                      action,
                      processtime,
                      message);
        }

        /// <summary>
        /// Add information to the web service log.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="dataProvider">Log text.</param>
        /// <param name="changedFrom">Changed from date.</param>
        /// <param name="changedTo">Changed to date.</param>
        /// <param name="processtime">How long time did the process take.</param>
        /// <param name="noOfCreated">Number of created observations.</param>
        /// <param name="noOfCreatedErrors">Number of errors when created observations.</param>
        /// <param name="noOfUpdated">Number of updated observations.</param>
        /// <param name="noOfUpdatedErrors">Number of errors when updated observations.</param>
        /// <param name="noOfDeleted">Number of deleted observations.</param>
        /// <param name="noOfDeletedErrors">Number of errors when deleted observations.</param>
        /// <param name="maxChangeId">Latest change id that was harvest from data source. -1 means no changeid is used for this dataprovider.</param>
        public virtual void LogHarvestRead(WebServiceContext context,
                                WebSpeciesObservationDataProvider dataProvider,
                                DateTime changedFrom,
                                DateTime changedTo,
                                Int64 processtime,
                                Int64 noOfCreated,
                                Int64 noOfCreatedErrors,
                                Int64 noOfUpdated,
                                Int64 noOfUpdatedErrors,
                                Int64 noOfDeleted,
                                Int64 noOfDeletedErrors,
                                Int64 maxChangeId = -1)
        {
            string action = String.Format("Update from {0}, date: {1}{2}", dataProvider.Name, changedFrom.ToShortDateString(), (maxChangeId > -1 ? ", maxChangeId: " + maxChangeId.ToString() : String.Empty));

            string result =
                String.Format(
                    "Created: {0} ({1} errors), Updated: {2} ({3} errors), Deleted: {4} ({5} errors)",
                    noOfCreated,
                    noOfCreatedErrors,
                    noOfUpdated,
                    noOfUpdatedErrors,
                    noOfDeleted,
                    noOfDeletedErrors);

            context.GetSpeciesObservationDatabase().WriteToLogStatistics(
                      action,
                      processtime,
                      result);
        }

        /// <summary>
        /// Update columns disturbanceRadius and
        /// maxAccuracyOrDisturbanceRadius in table 
        /// TempUpdateDarwinCoreObservation.
        /// </summary>
        public void UpdateAccuracyAndDisturbanceRadius()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("UpdateAccuracyAndDisturbanceRadius");
            ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Clean up DarwinCoreObservations after taxon update.
        /// </summary>
        /// <param name="context">
        /// Web service context.
        /// </param>
        /// <returns>
        /// Result of the database operation.
        /// </returns>
        public int UpdateDarwincoreObservationOnTaxonUpdate(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("UpdateDarwincoreObservationOnTaxonUpdate");
            return ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Used by HarvestManager to merge in data from temp to DarwinCoreObservation.
        /// </summary>
        /// <returns>Result of the database operation.</returns>
        public int MergeTempUpdateToDarwinCoreObservation()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("MergeTempUpdateToDarwinCoreObservation");
            return ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Used by HarvestManager to merge in data from temp to Location.
        /// </summary>
        /// <returns>
        /// Result of the database operation.
        /// </returns>
        public int MergeTempUpdateToPosition()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("MergeTempUpdateToPosition");
            return ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Used by HarvestManager to merge in data from temp to SpeciesObservationField.
        /// </summary>
        /// <returns>
        /// Result of the database operation.
        /// </returns>
        public int MergeTempUpdateToSpeciesObservationField()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("MergeTempUpdateToSpeciesObservationField");
            return ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Process data in TempTaxon. 
        /// </summary>
        /// <param name="context">
        /// Web service context.
        /// </param>
        /// <returns>
        /// Result of the database operation.
        /// </returns>
        public int ProcessTempTaxon(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("ProcessTempTaxon");
            return ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Create an instance of the database.
        /// </summary>
        public SpeciesObservationHarvestServer()
        {
            CommandTimeout = Settings.Default.DatabaseDefaultCommandTimeout; // Unit is seconds.
        }

        /// <summary>
        /// Update both Data Provider and Position statistics.
        /// </summary>
        public void UpdateStatistics()
        {
            UpdateDataProviderStatistics();
            UpdatePositionStatistics();
        }

        /// <summary>
        /// Update Data Provider Statistics.
        /// </summary>
        private void UpdateDataProviderStatistics()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("UpdateDataProviderStatistics");
            ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Update Position Statistics.
        /// </summary>
        private void UpdatePositionStatistics()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("UpdatePositionStatistics");
            ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Create log entry in the log of updated observations.
        /// </summary>
        /// <param name="dataProvider">Log for which data provider.</param>
        /// <param name="type">What type of log entry.</param>
        /// <param name="description">Description of the log entry.</param>
        /// <param name="observationId">What observation id is related to the log entry.</param>
        public void UpdateLog(String dataProvider,
                             LogType type,
                             String description,
                             String observationId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("UpdateUpdateLog");

            if (dataProvider.IsNotEmpty())
            {
                commandBuilder.AddParameter(HarvestLog.DATAPROVIDER, dataProvider);
            }

            commandBuilder.AddParameter(HarvestLog.TYPE, type.ToString());

            if (description.IsNotEmpty())
            {
                commandBuilder.AddParameter(HarvestLog.DESCRIPTION, description);
            }

            if (description.IsNotEmpty())
            {
                commandBuilder.AddParameter(HarvestLog.OBSERVATIONID, observationId);
            }

            ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Used by HarvestManager to update SpeciesObservationChange. 
        /// </summary>
        /// <returns>
        /// Result of the database operation.
        /// </returns>
        public int UpdateSpeciesObservationChange()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("UpdateSpeciesObservationChange");
            return ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Get current max value for species observation ids.
        /// </summary>
        /// <returns>Current max value for species observation ids.</returns>
        public int UpdateTempObservationId()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("UpdateTempObservationId");
            return ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Write to log of statistics.
        /// </summary>
        /// <param name="action">What type of action should be logged.</param>
        /// <param name="processtime">How long time did the process take.</param>
        /// <param name="result">How did the action went.</param>
        public void WriteToLogStatistics(string action,
                              long processtime,
                              string result)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("WriteToLogStatistics");

            commandBuilder.AddParameter(HarvestLog.ACTION, action);

            commandBuilder.AddParameter(HarvestLog.RESULT, result);

            commandBuilder.AddParameter(HarvestLog.PROCESSTIME, processtime);
            Debug.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + ": " + action + ", " + result + ", " + processtime);
            ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// The get invalid taxon ids in darwin core observation.
        /// </summary>
        /// <returns>
        /// The data reader for invalid taxon ids.<see cref="DataReader"/>.
        /// </returns>
        public DataReader GetInvalidTaxonIdsInDarwinCoreObservation()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetInvalidTaxonIdsInDarwinCoreObservation", false);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Query the database for maximum taxon update date.
        /// </summary>
        /// <returns>The latest taxon update date.</returns>
        public DateTime GetLatestTaxonUpdateDate()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetLatestTaxonUpdateDate", false);

            DateTime latestTaxonUpdateDate = DateTime.Now.AddDays(-100);
            using (DataReader dataReader = GetReader(commandBuilder))
            {
                while (dataReader.Read())
                {
                    latestTaxonUpdateDate = dataReader.GetDateTime(0);
                }
            }

            return latestTaxonUpdateDate;
        }

        /// <summary>
        /// Query the database for maximum Id for data provider.
        /// </summary>
        /// <param name="dataProviderId">
        /// The data Provider Id.
        /// </param>
        /// <returns>
        /// Integer with the maximum Change Id.
        /// </returns>
        public Int64 GetMaxChangeId(Int32 dataProviderId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetMaxChangeID", false);
            commandBuilder.AddParameter("dataProviderId", dataProviderId);

            Int64 changeId = 0;
            using (DataReader currentId = GetReader(commandBuilder))
            {
                while (currentId.Read())
                {
                    changeId = currentId.GetInt64(0);
                }
            }

            return changeId;
        }

        /// <summary>
        /// Log the latest date when information about retrieved observations have been changed at provider.
        /// </summary>
        /// <param name="dataProviderId">Data provider.</param>
        /// <returns>If database operation was successful.</returns>
        public Int32 SetDataProviderLatestChangedDate(Int32 dataProviderId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("SetDataProviderLatestChangedDate", false);
            commandBuilder.AddParameter("dataProviderId", dataProviderId);

            return ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Log when latest harvest was made for this data provider.
        /// </summary>
        /// <param name="dataProviderId">Data provider.</param>
        /// <param name="harvestDate">Latest date when harvest was made.</param>
        /// <returns>If database operation was successful.</returns>
        public Int32 SetDataProviderLatestHarvestDate(Int32 dataProviderId, DateTime harvestDate)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("SetDataProviderLatestHarvestDate", false);
            commandBuilder.AddParameter("dataProviderId", dataProviderId);
            commandBuilder.AddParameter("harvestDate", harvestDate);

            return ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Write the data provider's change id to database.
        /// </summary>
        /// <param name="dataProviderId">
        /// The data Provider Id.
        /// </param>
        /// <param name="changeId">
        /// The data provider's Change Id.
        /// </param>
        /// <returns>
        /// The status of the query.
        /// </returns>
        public Int32 SetMaxChangeId(Int32 dataProviderId, Int64? changeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("SetDataProviderMaxChangeId", false);
            commandBuilder.AddParameter("dataProviderId", dataProviderId);

            if (changeId.HasValue)
            {
                commandBuilder.AddParameter("maxChangeId", changeId.Value);
            }
            else
            {
                commandBuilder.AddParameter("maxChangeId", (Int64?)null);
            }

            return ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Clean up harvest job.
        /// Used by HarvestManager (UpdateSpeciesObservations).
        /// </summary>
        /// <returns>
        /// The result of the query.
        /// </returns>
        public int CleanUpHarvestJob()
        {
            var commandBuilder = new SqlCommandBuilder("HarvestJobCleanUp");
            return ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Get harvest job.
        /// Used by HarvestManager (UpdateSpeciesObservations).
        /// </summary>
        /// <returns>
        /// A harvest job data reader.
        /// </returns>
        public DataReader GetHarvestJob()
        {
            var commandBuilder = new SqlCommandBuilder("HarvestJobGet");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get harvest job's data providers.
        /// Used by HarvestManager (UpdateSpeciesObservations).
        /// </summary>
        /// <returns>
        /// A harvest job data reader.
        /// </returns>
        public DataReader GetHarvestJobDataProviders()
        {
            var commandBuilder = new SqlCommandBuilder("HarvestJobGetDataProviders");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get statistics about harvest job.
        /// Used by HarvestManager (UpdateSpeciesObservations).
        /// </summary>
        /// <returns>
        /// A harvest job data reader.
        /// </returns>
        public DataReader GetHarvestJobStatistics()
        {
            var commandBuilder = new SqlCommandBuilder("HarvestJobGetStatistics");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get entries from the LogStatistics log.
        /// </summary>
        /// <param name="rowCount">Maximum number of log entries to get.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetLogStatistics(Int32 rowCount)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetLogStatistics");

            commandBuilder.AddParameter(WebServiceLogData.ROW_COUNT, rowCount);

            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get entries from the LogUpdateError log.
        /// </summary>
        /// <param name="rowCount">Maximum number of log entries to get.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetLogUpdateError(Int32 rowCount)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetLogUpdateError");

            commandBuilder.AddParameter(WebServiceLogData.ROW_COUNT, rowCount);

            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Set harvest job metadata.
        /// Used by HarvestManager (UpdateSpeciesObservations).
        /// </summary>
        /// <param name="harvestJob">
        /// The harvest Job.
        /// </param>
        /// <returns>
        /// The result of the query.
        /// </returns>
        public int SetHarvestJob(HarvestJob harvestJob)
        {
            if (harvestJob.IsNotNull())
            {
                var commandBuilder = new SqlCommandBuilder("HarvestJobSet");

                commandBuilder.AddParameter(HarvestJobTableData.JOBSTARTDATE, harvestJob.JobStartDate);
                commandBuilder.AddParameter(HarvestJobTableData.HARVESTSTARTDATE, harvestJob.HarvestStartDate);
                commandBuilder.AddParameter(HarvestJobTableData.HARVESTCURRENTDATE, harvestJob.HarvestCurrentDate);
                commandBuilder.AddParameter(HarvestJobTableData.HARVESTENDDATE, harvestJob.HarvestEndDate);
                commandBuilder.AddParameter(HarvestJobTableData.JOBENDDATE, harvestJob.JobEndDate.Equals(DateTime.MinValue) ? null : harvestJob.JobEndDate);
                commandBuilder.AddParameter(HarvestJobTableData.JOBSTATUS, harvestJob.JobStatus.ToString());

                Debug.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + ": harvestJob, ");
                return ExecuteCommand(commandBuilder);
            }

            return -1;
        }

        /// <summary>
        /// Set harvest job's data provider metadata.
        /// Used by HarvestManager (UpdateSpeciesObservations).
        /// </summary>
        /// <param name="harvestJob">
        /// The harvest Job.
        /// </param>
        /// <returns>
        /// The result of the query.
        /// </returns>
        public int SetHarvestJobDataProvider(HarvestJob harvestJob)
        {
            if (harvestJob.IsNotNull())
            {
                int cumulativeResult = 1;

                if (harvestJob.DataProviders.Any())
                {
                    Debug.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + ": harvestJob(dataproviders), ");
                    foreach (var harvestJobDataProvider in harvestJob.DataProviders)
                    {
                        var commandBuilder = new SqlCommandBuilder("HarvestJobSetDataProvider");
                        commandBuilder.AddParameter(HarvestJobTableData.DATAPROVIDERID, harvestJobDataProvider.DataProviderId);
                        commandBuilder.AddParameter(HarvestJobTableData.CHANGEID, harvestJobDataProvider.ChangeId);

                        int result = ExecuteCommand(commandBuilder);

                        if (result < 1)
                        {
                            cumulativeResult = result;
                        }
                    }
                }

                return cumulativeResult;
            }

            return -1;
        }

        /// <summary>
        /// Log statistics about harvest job.
        /// </summary>
        /// <param name="dataProviderIds">List of data provider ids.</param>
        /// <param name="currentStatus">Harvest job current job status.</param>
        /// <param name="changedFrom">Harvest job current harvest date.</param>
        /// <returns>Result of operation.</returns>
        public Int32 SetHarvestJobStatistics(System.Collections.Generic.List<int> dataProviderIds, HarvestStatusEnum currentStatus, DateTime changedFrom)
        {
            Int32 cumulativeResult = 1;

            Debug.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + ": harvestJob(statistics), ");
            foreach (var dataProviderId in dataProviderIds)
            {
                var commandBuilder = new SqlCommandBuilder("HarvestJobSetStatistics");
                commandBuilder.AddParameter(HarvestJobTableData.DATAPROVIDERID, dataProviderId);
                commandBuilder.AddParameter(HarvestJobTableData.JOBSTATUS, currentStatus.ToString());
                commandBuilder.AddParameter(HarvestJobTableData.HARVESTDATE, changedFrom);

                int result = ExecuteCommand(commandBuilder);

                if (result < 1)
                {
                    cumulativeResult = result;
                }
            }

            return cumulativeResult;
        }
    }
}

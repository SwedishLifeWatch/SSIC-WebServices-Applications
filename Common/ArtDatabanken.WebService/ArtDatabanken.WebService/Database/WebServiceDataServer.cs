using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using ArtDatabanken.Database;
using Microsoft.SqlServer.Types;
using NotUsedCommandBuilder = System.Data.SqlClient.SqlCommandBuilder;
using SqlCommandBuilder = ArtDatabanken.Database.SqlCommandBuilder;

namespace ArtDatabanken.WebService.Database
{
    /// <summary>
    /// Class used as database interface.
    /// This class contains methods that
    /// are common to all database connections.
    /// </summary>
    public abstract class WebServiceDataServer : DataServer
    {
        private static readonly Hashtable _columnLenghtTable;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static WebServiceDataServer()
        {
            _columnLenghtTable = new Hashtable();
        }

        /// <summary>
        /// Clear data cache in data service.
        /// </summary>
        public static void ClearCache()
        {
            _columnLenghtTable.Clear();
        }

        /// <summary>
        /// Delete trace information from the web service log.
        /// </summary>
        public void DeleteTrace()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("DeleteWebServiceTrace");
            ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Get length of a column in a database.
        /// This method is mainly used to retrive information about
        /// text columns but is works on any data type.
        /// </summary>
        /// <param name="tableName">Name of table to get information from.</param>
        /// <param name="columnName">Name of column to get length of.</param>
        /// <returns> The column length.</returns>
        public Int32 GetColumnLength(String tableName,
                                     String columnName)
        {
            Int32 columnLength;
            SqlCommandBuilder commandBuilder;
            String hashKey;

            // Get length of specified column.
            hashKey = "Table:" + tableName + "Column:" + columnName;
            if (_columnLenghtTable.Contains(hashKey))
            {
                // Get cached value.
                columnLength = (Int32)(_columnLenghtTable[hashKey]);
            }
            else
            {
                // Get value from database.
                commandBuilder = new SqlCommandBuilder("GetColumnLength");
                commandBuilder.AddParameter(ColumnLenghtData.TABLE_NAME, tableName);
                commandBuilder.AddParameter(ColumnLenghtData.COLUMN_NAME, columnName);
                columnLength = ExecuteScalar(commandBuilder);

                // Add value to cache.
                lock (_columnLenghtTable)
                {
                    if (!(_columnLenghtTable.Contains(hashKey)))
                    {
                        _columnLenghtTable.Add(hashKey, columnLength);
                    }
                }
            }

            return columnLength;
        }

        /// <summary>
        /// Get number of species observations
        /// that matches search criteria. 
        /// </summary>
        /// <param name="polygons">Selected polygons.</param>
        /// <param name="regionIds">Region ids.</param>
        /// <param name="taxonIds">
        /// Taxon ids. Observations related to specified taxa and
        /// all underlying taxa are included.
        /// </param>
        /// <param name="joinCondition">SQL join condition.</param>
        /// <param name="whereCondition">SQL where condition.</param>
        /// <param name="geometryWhereCondition">SQL geometry where condition.</param>
        /// <returns>
        /// Number of species observations that matches search criteria.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Wkt"),
         System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "multi"),
         System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public Int64 GetSpeciesObservationCountBySearchCriteria(List<SqlGeometry> polygons,
                                                                List<Int32> regionIds,
                                                                List<Int32> taxonIds,
                                                                String joinCondition,
                                                                String whereCondition,
                                                                String geometryWhereCondition)
        {
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter, name the stored procedure
            CommandTimeout = 600;
            commandBuilder = new SqlCommandBuilder("GetSpeciesObservationCountBySearchCriteria", true);
            commandBuilder.AddParameter(SpeciesObservationData.POLYGONS, polygons);
            commandBuilder.AddParameter(SpeciesObservationData.REGION_IDS, regionIds);
            commandBuilder.AddParameter(SpeciesObservationData.TAXON_IDS, taxonIds);
            if (joinCondition.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.JOIN_CONDITION, joinCondition);
            }

            commandBuilder.AddParameter(SpeciesObservationData.WHERE_CONDITION, whereCondition);

            if (geometryWhereCondition.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.GEOMETRY_WHERE_CONDITION, geometryWhereCondition);
            }

            return ExecuteScalar(commandBuilder);
        }


        /// <summary>
        /// This method retrieves all Species observation field descriptions for a given locale by id from database.
        /// </summary>
        /// <param name="localeId">Id of requested locale.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.</returns>
        public DataReader GetSpeciesObservationFieldDescriptions(Int32 localeId)
        {
            // Use SqlCommandBuilder with SqlParameter, name the stored procedure
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetSpeciesObservationFieldDescriptions", true);
            commandBuilder.AddParameter("LocaleId", localeId);

            return GetReader(commandBuilder);
        }

        /// <summary>
        /// This method retrieves all Species observation field mapping descriptions.
        /// </summary>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.</returns>
        public DataReader GetSpeciesObservationFieldMappings()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetSpeciesObservationFieldMappings", false);

            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Adds or updates the SpeciesObservationFieldDescription tabel
        /// </summary>
        /// <param name="table">The added or updated rows</param>
        public void UpdateSpeciesObservationFieldDescription(DataTable table)
        {
            var commandBuilder = new SqlCommandBuilder("UpdateSpeciesObservationFieldDescription", true);
            commandBuilder.AddParameter("Values", table);
            ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Adds or updates the SpeciesObservationFieldMapping tabel
        /// </summary>
        /// <param name="table">The added or updated rows</param>
        public void UpdateSpeciesObservationFieldMapping(DataTable table)
        {
            var commandBuilder = new SqlCommandBuilder("UpdateSpeciesObservationFieldMapping", true);
            commandBuilder.AddParameter("Values", table);
            ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Get species observations
        /// that matches search criteria. 
        /// </summary>
        /// <param name="polygons">Selected polygons.</param>
        /// <param name="regionIds">Region ids.</param>
        /// <param name="taxonIds">
        /// Taxon ids. Observations related to specified taxa and
        /// all underlying taxa are returned.
        /// </param>
        /// <param name="joinCondition">SQL join condition.</param>
        /// <param name="whereCondition">SQL where condition.</param>
        /// <param name="geometryWhereCondition">SQL geometry where condition.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetSpeciesObservationsAccessRights(List<SqlGeometry> polygons,
                                                             List<Int32> regionIds,
                                                             List<Int32> taxonIds,
                                                             String joinCondition,
                                                             String whereCondition,
                                                             String geometryWhereCondition)
        {
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter, name the stored procedure
            commandBuilder = new SqlCommandBuilder("GetSpeciesObservationsAccessRights", true);
            commandBuilder.AddParameter(SpeciesObservationData.POLYGONS, polygons);
            commandBuilder.AddParameter(SpeciesObservationData.REGION_IDS, regionIds);
            commandBuilder.AddParameter(SpeciesObservationData.TAXON_IDS, taxonIds);
            if (joinCondition.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.JOIN_CONDITION, joinCondition);
            }

            commandBuilder.AddParameter(SpeciesObservationData.WHERE_CONDITION, whereCondition);

            if (geometryWhereCondition.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.GEOMETRY_WHERE_CONDITION, geometryWhereCondition);
            }

            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Returns latestLog
        /// </summary>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.</returns>
        public DataReader GetLatestLog()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetLatestLog", false);
            return GetReader(commandBuilder);
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="scientificName"></param>
        /// <returns></returns>
        public Int32 GetTaxonIdByScientificName(String scientificName)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetTaxonIdByScientificName");
            commandBuilder.AddParameter("ScientificName", scientificName);
            int taxonId = ExecuteScalar(commandBuilder);

            return taxonId;
        }

        /// <summary>
        /// Get taxon tree relations.
        /// </summary>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public DataReader GetTaxonTree()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetTaxonTree");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public DataReader GetDyntaxaTaxonIdByQueryString(String queryString)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetTaxonIdByQueryString");

            commandBuilder.AddParameter("QueryString", queryString);
            return GetReader(commandBuilder);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetDyntaxaTaxonForTaxonIdQueryString()
        {


            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetTaxonTableForQueryString");

            return GetDataTable(commandBuilder);


        }

        /// <summary>
        /// Get entries from the web service log
        /// </summary>
        /// <param name="type">Get log entries of this type. May be empty.</param>
        /// <param name="userName">Get log entries for this user. May be empty.</param>
        /// <param name="rowCount">Maximum number of log entries to get.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetLog(String type,
                                 String userName,
                                 Int32 rowCount)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetWebServiceLog");
            if (type.IsNotEmpty())
            {
                commandBuilder.AddParameter(WebServiceLogData.TYPE, type);
            }

            if (userName.IsNotEmpty())
            {
                commandBuilder.AddParameter(WebServiceLogData.WEB_SERVICE_USER, userName, 2);
            }

            commandBuilder.AddParameter(WebServiceLogData.ROW_COUNT, rowCount);

            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get information about all species observation data sources.
        /// </summary>
        /// <param name="localeId">Locale id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetSpeciesObservationDataProviders(Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetDataProviders");
            commandBuilder.AddParameter(SpeciesObservationData.LOCALE_ID, localeId);

            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Check if the database is up and running.
        /// </summary>
        /// <returns>
        /// True = OK.
        /// False = Some kind of problem.
        /// </returns>       
        public Boolean Ping()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("Ping");
            return (ExecuteScalar(commandBuilder) == 1);
        }

        /// <summary>
        /// Add an entry to the web service log
        /// </summary>
        /// <param name="text">Log text.</param>
        /// <param name="type">Type of log entry.</param>
        /// <param name="information">Extended information about the log entry.</param>
        /// <param name="userName">Name of user that creates the log entry.</param>
        /// <param name="clientIpAddress">IP address of user that creates the log entry.</param>
        /// <param name="applicationIdentifier">Application identifier.</param>
        public void UpdateLog(String text,
                              String type,
                              String information,
                              String userName,
                              String clientIpAddress,
                              String applicationIdentifier)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("UpdateWebServiceLog");
            if (text.IsNotEmpty())
            {
                commandBuilder.AddParameter(WebServiceLogData.TEXT, text.CheckSqlInjection());
            }
            commandBuilder.AddParameter(WebServiceLogData.TYPE, type.CheckSqlInjection());
            if (userName.IsNotEmpty())
            {
                commandBuilder.AddParameter(WebServiceLogData.WEB_SERVICE_USER, userName.CheckSqlInjection());
            }
            if (clientIpAddress.IsNotEmpty())
            {
                commandBuilder.AddParameter(WebServiceLogData.TCP_IP, clientIpAddress.CheckSqlInjection());
            }
            if (information.IsNotEmpty())
            {
                commandBuilder.AddParameter(WebServiceLogData.INFORMATION, information.CheckSqlInjection());
            }
            if (applicationIdentifier.IsNotEmpty())
            {
                commandBuilder.AddParameter(WebServiceLogData.APPLICATION_IDENTIFIER, applicationIdentifier.CheckSqlInjection());
            }

            ExecuteCommand(commandBuilder);
        }

        public string GetMunicipalityFromCoordinates(double x, double y)
        {
            var command = new SqlCommandBuilder("GetMunicipalityFromCoordinates", false);
            command.AddParameter("X", x);
            command.AddParameter("Y", y);

            return ExecuteScalarString(command).Trim();
        }

        public string GetParishFromCoordinates(double x, double y)
        {
            var command = new SqlCommandBuilder("GetParishFromCoordinates", false);
            command.AddParameter("X", x);
            command.AddParameter("Y", y);

            return ExecuteScalarString(command).Trim();
        }

        public string GetStateProvinceFromCoordinates(double x, double y)
        {
            var command = new SqlCommandBuilder("GetStateProvinceFromCoordinates", false);
            command.AddParameter("X", x);
            command.AddParameter("Y", y);

            return ExecuteScalarString(command).Trim();
        }

        public string GetCountyFromCoordinates(double x, double y)
        {
            var command = new SqlCommandBuilder("GetCountyFromCoordinates", false);
            command.AddParameter("X", x);
            command.AddParameter("Y", y);

            return ExecuteScalarString(command).Trim();
        }
    }
}

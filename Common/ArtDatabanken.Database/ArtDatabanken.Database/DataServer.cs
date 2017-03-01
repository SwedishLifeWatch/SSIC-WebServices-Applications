using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using NotUsedCommandBuilder = System.Data.SqlClient.SqlCommandBuilder;

namespace ArtDatabanken.Database
{
    /// <summary>
    /// Class used as database interface.
    /// This class contains methods that
    /// are common to all web services.
    /// </summary>
    public abstract class DataServer : IDisposable
    {
        /// <summary>
        /// The _transaction.
        /// </summary>
        private SqlTransaction _transaction;

        /// <summary>
        /// Create an instance of a database.
        /// </summary>
        protected DataServer()
        {
            CommandTimeout = Settings.Default.DatabaseDefaultCommandTimeout; // Unit is seconds.
            Connection = null;
            Connect();
        }

        /// <summary>
        /// Time out to use. Unit is seconds.
        /// </summary>
        public Int32 CommandTimeout { get; set; }

        /// <summary>
        /// Connection to database.
        /// </summary>
        protected SqlConnection Connection { get; set; }

        /// <summary>
        /// Add information to a table in the database.
        /// </summary>
        /// <param name="table">Holds data and name of table.</param>
        public void AddTableData(DataTable table)
        {
            SqlBulkCopy bulkCopy;

            bulkCopy = new SqlBulkCopy(Connection,
                                       SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.CheckConstraints,
                                       _transaction);
            bulkCopy.BulkCopyTimeout = CommandTimeout;

            bulkCopy.DestinationTableName = "dbo." + table.TableName;
            bulkCopy.WriteToServer(table);
        }

        /// <summary>
        /// Start a transaction.
        /// </summary>
        /// <exception cref="ApplicationException">Thrown if a transaction is already active.</exception>
        public void BeginTransaction()
        {
            if (_transaction.IsNull())
            {
                _transaction = Connection.BeginTransaction();
            }
            else
            {
                throw new ApplicationException("Transaction already active.");
            }
        }

        /// <summary>
        /// Check that this database connection has a transaction.
        /// </summary>
        /// <exception cref="ApplicationException">Thrown if no transaction is active.</exception>
        public void CheckTransaction()
        {
            if (_transaction.IsNull())
            {
                throw new ApplicationException("No active transaction.");
            }
        }

        /// <summary>
        /// Commit a transaction.
        /// </summary>
        /// <exception cref="ApplicationException">Thrown if no transaction is active.</exception>
        public void CommitTransaction()
        {
            if (_transaction.IsNull())
            {
                throw new ApplicationException("Unable to commit inactive transaction.");
            }
            else
            {
                _transaction.Commit();
                _transaction = null;
            }
        }

        /// <summary>
        /// Connect to the database.
        /// </summary>
        protected abstract void Connect();

        /// <summary>
        /// Disconnect from the database.
        /// </summary>
        public void Disconnect()
        {
            // Closes the current database connection.
            if (Connection.IsNotNull() &&
                ((Connection.State == ConnectionState.Open) ||
                 (Connection.State == ConnectionState.Fetching)))
            {
                if (HasPendingTransaction())
                {
                    RollbackTransaction();
                }

                Connection.Close();
                Connection = null;
            }
        }

        /// <summary>
        /// Implementation of the IDisposable interface.
        /// Releases all resources related to this request.
        /// </summary>
        public void Dispose()
        {
            Disconnect();
        }

        /// <summary>
        /// Execute a command in the database.
        /// </summary>
        /// <param name="commandBuilder">Command to execute.</param>
        /// <returns>Status information about the command execution.</returns>
        protected Int32 ExecuteCommand(SqlCommandBuilder commandBuilder)
        {
            return GetCommand(commandBuilder).ExecuteNonQuery();
        }

        /// <summary>
        /// Get an Integer value from the database.
        /// </summary>
        /// <param name="commandBuilder">Command to execute.</param>
        /// <returns>The Integer value.</returns>
        protected Int32 ExecuteScalar(SqlCommandBuilder commandBuilder)
        {
            return Convert.ToInt32(GetCommand(commandBuilder).ExecuteScalar());
        }

        /// <summary>
        /// Gets a string value from the first column of the first row from the database.
        /// Returns an empty string if the value is null
        /// </summary>
        /// <param name="commandBuilder">Command to execute.</param>
        /// <returns></returns>
        protected string ExecuteScalarString(SqlCommandBuilder commandBuilder)
        {
            var value = GetCommand(commandBuilder).ExecuteScalar();
            return value != null ? value.ToString() : string.Empty;
        }

        /// <summary>
        /// Get address to database.
        /// That is, address to data base server and name of database.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <returns>Address to database.</returns>
        public static String GetAddress(String connectionString)
        {
            String database, dataServer;

            dataServer = connectionString.Substring(connectionString.IndexOf("data source=", System.StringComparison.Ordinal) + 12);
            dataServer = dataServer.Substring(0, dataServer.IndexOf(";", System.StringComparison.Ordinal));
            database = connectionString.Substring(connectionString.IndexOf("initial catalog=", System.StringComparison.Ordinal) + 16);
            database = database.Substring(0, database.IndexOf(";", System.StringComparison.Ordinal));
            return dataServer + "#" + database;
        }

        /// <summary>
        /// Get a Sql Command instance.
        /// Transaction is added if available.
        /// </summary>
        /// <param name="commandBuilder">Command to set in the Sql Command instance.</param>
        /// <returns> The Sql Command instance.</returns>
        protected SqlCommand GetCommand(SqlCommandBuilder commandBuilder)
        {
            SqlCommand command;

            if (commandBuilder.IsSqlParameterUsed)
            {
                if (_transaction.IsNull())
                {
                    command = new SqlCommand(null, Connection);
                }
                else
                {
                    command = new SqlCommand(null, Connection, _transaction);
                }

                command.Parameters.AddRange(commandBuilder.GetSqlParameters().ToArray());
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = commandBuilder.StoredProcedureName;
            }
            else
            {
                if (_transaction.IsNull())
                {
                    command = new SqlCommand(commandBuilder.GetCommand(), Connection);
                }
                else
                {
                    command = new SqlCommand(commandBuilder.GetCommand(), Connection, _transaction);
                }
            }

            command.CommandTimeout = CommandTimeout;
            return command;
        }

        /// <summary>
        /// Get a data set.
        /// </summary>
        /// <param name="commandBuilder">Command to get data set from.</param>
        /// <returns>A DataSet with results from the Sql command.</returns>
        protected DataSet GetDataSet(SqlCommandBuilder commandBuilder)
        {
            SqlDataAdapter adapter;
            DataSet dataSet;
            SqlCommand command;

            command = GetCommand(commandBuilder);
            adapter = new SqlDataAdapter();
            dataSet = new DataSet();
            adapter.SelectCommand = command;
            adapter.Fill(dataSet);
            return dataSet;
        }

        /// <summary>
        /// Get a data table.
        /// </summary>
        /// <param name="commandBuilder">Command to get data table from.</param>
        /// <returns>A DataTable with results from the Sql command.</returns>
        protected DataTable GetDataTable(SqlCommandBuilder commandBuilder)
        {
            DataTable table;
            SqlCommand command;

            table = new DataTable("Result");
            command = GetCommand(commandBuilder);
            using (SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.SingleResult))
            {
                table.Load(dataReader);
            }

            return table;
        }

        /// <summary>
        /// Get a data reader.
        /// </summary>
        /// <param name="commandBuilder">Command to get data reader from.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        protected DataReader GetReader(SqlCommandBuilder commandBuilder)
        {
            return GetReader(commandBuilder, CommandBehavior.SingleResult);
        }

        /// <summary>
        /// Get a data reader.
        /// </summary>
        /// <param name="commandBuilder">Command to get data reader from.</param>
        /// <param name="commandBehavior">Command behavior. Is used to optimize (in speed) the data fetch.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        protected DataReader GetReader(SqlCommandBuilder commandBuilder,
                                       CommandBehavior commandBehavior)
        {
            return new DataReader(GetCommand(commandBuilder).ExecuteReader(commandBehavior));
        }

        /// <summary>
        /// Get a data reader that only contains one row of data.
        /// </summary>
        /// <param name="commandBuilder">Command to get data reader from.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        protected DataReader GetRow(SqlCommandBuilder commandBuilder)
        {
            return GetReader(commandBuilder,
                             CommandBehavior.SingleRow | CommandBehavior.SingleResult);
        }

        /// <summary>
        /// Test if the database has a pending transaction.
        /// </summary>
        /// <returns>True if there is a pending transaction.</returns>
        public Boolean HasPendingTransaction()
        {
            return (_transaction.IsNotNull());
        }

        /// <summary>
        /// Rollback transaction.
        /// </summary>
        /// <exception cref="Exception">Thrown if no transaction is active.</exception>
        public void RollbackTransaction()
        {
            if (_transaction.IsNull())
            {
                throw new ApplicationException("Unable to rollback inactive transaction.");
            }
            else
            {
                try
                {
                    _transaction.Rollback();
                }
                catch
                {
                }

                _transaction = null;
            }
        }
    }
}

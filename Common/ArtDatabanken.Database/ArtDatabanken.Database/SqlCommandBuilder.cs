using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Types;

namespace ArtDatabanken.Database
{
    /// <summary>
    /// Class that simplifies the construction of an sql
    /// query that is based on a call to a stored procedure.
    /// </summary>
    public class SqlCommandBuilder
    {
        private Boolean _hasParameters;
        private readonly StringBuilder _command;
        private readonly List<SqlParameter> _sqlParameters;

        /// <summary>
        /// Constructor that ties the sql query to a stored procedure.
        /// </summary>
        /// <param name="storedProcedure">Name of stored procedure to call.</param>
        public SqlCommandBuilder(String storedProcedure)
            : this(storedProcedure, false)
        {
        }

        /// <summary>
        /// Constructor that ties the sql query to a stored procedure.
        /// Use this constructor when call to stored procedure need to use SqlParameters.
        /// </summary>
        /// <param name="storedProcedure">Name of stored procedure to call.</param>
        /// <param name="useSqlParameters">
        /// true if CommandBuilder has to use SqlParameter
        /// which is needed when passing "Table-Value Parameters" to stored procedures.
        /// </param>
        public SqlCommandBuilder(String storedProcedure, Boolean useSqlParameters)
        {
            _hasParameters = false;
            IsSqlParameterUsed = useSqlParameters;
            StoredProcedureName = storedProcedure;
            if (IsSqlParameterUsed)
            {
                _sqlParameters = new List<SqlParameter>();
            }
            else
            {
                _command = new StringBuilder("EXECUTE " + StoredProcedureName);
            }
        }

        /// <summary>
        /// Test if SqlParameters are used.
        /// </summary>
        public Boolean IsSqlParameterUsed
        { get; private set; }

        /// <summary>
        /// Get the name of stored procedure to call.
        /// </summary>
        public String StoredProcedureName
        { get; private set; }

        /// <summary>
        /// Add the name part of a parameter to the sql query.
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        private void AddParameterName(String parameterName)
        {
            if (_hasParameters)
            {
                _command.Append(",");
            }
            _command.Append(" @");
            _command.Append(parameterName);
            _command.Append(" = ");
            _hasParameters = true;
        }

        /// <summary>
        /// Add a Boolean parameter to the sql query.
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        /// <param name="parameterValue">Value of parameter to add.</param>
        public void AddParameter(String parameterName, Boolean parameterValue)
        {
            SqlParameter parameter;

            if (IsSqlParameterUsed)
            {
                parameter = new SqlParameter();
                parameter.ParameterName = parameterName;
                parameter.SqlDbType = SqlDbType.Bit;
                parameter.Value = parameterValue;
                _sqlParameters.Add(parameter);
            }
            else
            {
                AddParameterName(parameterName);
                if (parameterValue)
                {
                    _command.Append("1");
                }
                else
                {
                    _command.Append("0");
                }
            }
        }

        /// <summary>
        /// Add a DataTable as parameter to the sql query.
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        /// <param name="parameterValue">Values of parameter to add.</param>
        public void AddParameter(String parameterName, DataTable parameterValue)
        {
            SqlParameter parameter;

            if (IsSqlParameterUsed)
            {
                parameter = new SqlParameter();
                parameter.ParameterName = parameterName;
                parameter.SqlDbType = SqlDbType.Structured;
                parameter.Value = parameterValue;
                _sqlParameters.Add(parameter);
            }
            else
            {
                throw new ArgumentException("SqlCommandBuilder: Parameters of List-type not supported without use of SqlParameter");
            }
        }

        /// <summary>
        /// Add a DateTime parameter to the sql query.
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        /// <param name="parameterValue">Value of parameter to add.</param>
        public void AddParameter(String parameterName, DateTime parameterValue)
        {
            if (IsSqlParameterUsed)
            {
                var parameter = new SqlParameter();
                parameter.ParameterName = parameterName;
                parameter.SqlDbType = SqlDbType.DateTime;
                parameter.Value = parameterValue;
                _sqlParameters.Add(parameter);
            }
            else
            {
                AddParameterName(parameterName);
                _command.Append("'" + parameterValue.ToString(Settings.Default.DatabaseDateTimeFormat) + "'");
            }
        }

        /// <summary>
        /// Add a Nullable datetime parameter to the sql query.
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        /// <param name="parameterValue">Value of parameter to add.</param>
        public void AddParameter(String parameterName, DateTime? parameterValue)
        {
            if (IsSqlParameterUsed)
            {
                var parameter = new SqlParameter();
                parameter.ParameterName = parameterName;
                parameter.SqlDbType = SqlDbType.DateTime;
                parameter.Value = parameterValue;
                _sqlParameters.Add(parameter);
            }
            else
            {
                if (parameterValue.HasValue)
                {
                    AddParameter(parameterName, parameterValue.Value);
                }
                else
                {
                    AddParameterName(parameterName);
                    _command.Append("null");
                }
            }
        }

        /// <summary>
        /// Add a DateTime parameter to the sql query.
        /// Replace by defaultIfMinValue if parameterValue equals .Minvalue
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        /// <param name="parameterValue">Value of parameter to add.</param>
        /// <param name="defaultIfMinValue">Value if parameter equals .MinValue.</param>
        public void AddParameter(String parameterName, DateTime parameterValue, String defaultIfMinValue)
        {
            if (IsSqlParameterUsed)
            {
                if (parameterValue.Equals(DateTime.MinValue))
                {
                    var parameter = new SqlParameter();
                    parameter.ParameterName = parameterName;
                    parameter.SqlDbType = SqlDbType.DateTime;
                    parameter.Value = defaultIfMinValue;
                    _sqlParameters.Add(parameter);
                }
                else
                {
                    var parameter = new SqlParameter();
                    parameter.ParameterName = parameterName;
                    parameter.SqlDbType = SqlDbType.DateTime;
                    parameter.Value = parameterValue;
                    _sqlParameters.Add(parameter);
                }
            }
            else
            {
                AddParameterName(parameterName);
                if (parameterValue.Equals(DateTime.MinValue))
                {
                    _command.Append(defaultIfMinValue);
                }
                else
                {
                    _command.Append("'" + parameterValue.ToString(Settings.Default.DatabaseDateTimeFormat) + "'");
                }
            }
        }

        /// <summary>
        /// Add a Double parameter to the sql query.
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        /// <param name="parameterValue">Value of parameter to add.</param>
        public void AddParameter(String parameterName, Double parameterValue)
        {
            SqlParameter parameter;

            if (IsSqlParameterUsed)
            {
                parameter = new SqlParameter();
                parameter.ParameterName = parameterName;
                parameter.SqlDbType = SqlDbType.Decimal;
                parameter.Value = parameterValue;
                _sqlParameters.Add(parameter);
            }
            else
            {
                AddParameterName(parameterName);
                _command.Append(parameterValue.ToString().Replace(",", "."));
            }
        }

        /// <summary>
        /// Add an Int32 parameter to the sql query.
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        /// <param name="parameterValue">Value of parameter to add.</param>
        public void AddParameter(String parameterName, Int32 parameterValue)
        {
            SqlParameter parameter;

            if (IsSqlParameterUsed)
            {
                parameter = new SqlParameter();
                parameter.ParameterName = parameterName;
                parameter.SqlDbType = SqlDbType.Int;
                parameter.Value = parameterValue;
                _sqlParameters.Add(parameter);
            }
            else
            {
                AddParameterName(parameterName);
                _command.Append(parameterValue.ToString());
            }
        }

        /// <summary>
        /// Add an Int32 parameter to the sql query.
        /// Replace by defaultIfMinValue if parameterValue equals .Minvalue
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        /// <param name="parameterValue">Value of parameter to add.</param>
        /// <param name="defaultIfMinValue">Value if parameter equals .MinValue.</param>
        public void AddParameter(String parameterName, Int32 parameterValue, String defaultIfMinValue)
        {
            if (IsSqlParameterUsed)
            {
                if (parameterValue.Equals(Int32.MinValue))
                {
                    AddParameter(parameterName, Convert.ToInt32(defaultIfMinValue));
                }
                else
                {
                    AddParameter(parameterName, defaultIfMinValue);
                }
            }
            else
            {
                AddParameterName(parameterName);
                if (parameterValue.Equals(Int32.MinValue))
                {
                    _command.Append(defaultIfMinValue);
                }
                else
                {
                    _command.Append(parameterValue.ToString());
                }
            }
        }

        /// <summary>
        /// Add a List of Int32 as parameter to the sql query.
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        /// <param name="parameterValue">Values of parameter to add.</param>
        public void AddParameter(String parameterName, List<Int32> parameterValue)
        {
            DataTable table;

            if (IsSqlParameterUsed)
            {
                table = new DataTable();
                table.Columns.Add("Id");
                if (parameterValue.IsNotEmpty())
                {
                    foreach (Int32 id in parameterValue)
                    {
                        table.Rows.Add(id);
                    }
                }
                AddParameter(parameterName, table);
            }
            else
            {
                throw new ArgumentException("SqlCommandBuilder: Parameters of List-type not supported without use of SqlParameter");
            }
        }

        /// <summary>
        /// Add a List of Int32 as parameter to the sql query.
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        /// <param name="parameterValue">Values of parameter to add.</param>
        public void AddParameter(String parameterName, List<Int64> parameterValue)
        {
            DataTable table;

            if (IsSqlParameterUsed)
            {
                table = new DataTable();
                table.Columns.Add("Id");
                if (parameterValue.IsNotEmpty())
                {
                    foreach (Int64 id in parameterValue)
                    {
                        table.Rows.Add(id);
                    }
                }
                AddParameter(parameterName, table);
            }
            else
            {
                throw new ArgumentException("SqlCommandBuilder: Parameters of List-type not supported without use of SqlParameter");
            }
        }

        /// <summary>
        /// Add a List od String as parameter to the sql query.
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        /// <param name="parameterValue">Values of parameter to add.</param>
        public void AddParameter(String parameterName, List<String> parameterValue)
        {
            DataTable table;

            if (IsSqlParameterUsed)
            {
                table = new DataTable();
                table.Columns.Add("Id");
                if (parameterValue.IsNotEmpty())
                {
                    foreach (string id in parameterValue)
                    {
                        table.Rows.Add(id);
                    }
                }
                AddParameter(parameterName, table);
            }
            else
            {
                throw new ArgumentException("SqlCommandBuilder: Parameters of List-type not supported without use of SqlParameter");
            }
        }

        /// <summary>
        /// Add a list of SqlGeographies as parameter to the sql query.
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        /// <param name="parameterValue">Values of parameter to add.</param>
        public void AddParameter(String parameterName,
                                 List<SqlGeography> parameterValue)
        {
            DataTable table;

            if (IsSqlParameterUsed)
            {
                table = new DataTable();
                table.Columns.Add("Geography");
                if (parameterValue.IsNotEmpty())
                {
                    foreach (SqlGeography geography in parameterValue)
                    {
                        table.Rows.Add(geography);
                    }
                }
                AddParameter(parameterName, table);
            }
            else
            {
                throw new ArgumentException("SqlCommandBuilder: Parameters of List-type not supported without use of SqlParameter");
            }
        }

        /// <summary>
        /// Add a list of SqlGeometries as parameter to the sql query.
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        /// <param name="parameterValue">Values of parameter to add.</param>
        public void AddParameter(String parameterName,
                                 List<SqlGeometry> parameterValue)
        {
            DataTable table;

            if (IsSqlParameterUsed)
            {
                table = new DataTable();
                table.Columns.Add("Geometry");
                if (parameterValue.IsNotEmpty())
                {
                    foreach (SqlGeometry geometry in parameterValue)
                    {
                        table.Rows.Add(geometry);
                    }
                }
                AddParameter(parameterName, table);
            }
            else
            {
                throw new ArgumentException("SqlCommandBuilder: Parameters of List-type not supported without use of SqlParameter");
            }
        }

        /// <summary>
        /// Add an Int64 parameter to the sql query.
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        /// <param name="parameterValue">Value of parameter to add.</param>
        public void AddParameter(String parameterName, Int64 parameterValue)
        {
            SqlParameter parameter;

            if (IsSqlParameterUsed)
            {
                parameter = new SqlParameter();
                parameter.ParameterName = parameterName;
                parameter.SqlDbType = SqlDbType.BigInt;
                parameter.Value = parameterValue;
                _sqlParameters.Add(parameter);
            }
            else
            {
                AddParameterName(parameterName);
                _command.Append(parameterValue.ToString());
            }
        }

        /// <summary>
        /// Add an nullable Int64 parameter to the sql query.
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        /// <param name="parameterValue">Value of parameter to add.</param>
        public void AddParameter(String parameterName, Int64? parameterValue)
        {
            SqlParameter parameter;

            if (IsSqlParameterUsed)
            {
                parameter = new SqlParameter();
                parameter.ParameterName = parameterName;
                parameter.SqlDbType = SqlDbType.BigInt;
                if (parameterValue.HasValue)
                {
                    parameter.Value = parameterValue;
                }
                _sqlParameters.Add(parameter);
            }
            else
            {
                AddParameterName(parameterName);
                if (parameterValue.HasValue)
                {
                    _command.Append(parameterValue.Value);
                }
                else
                {
                    _command.Append("NULL");
                }
            }
        }

        /// <summary>
        /// Add a String parameter to the sql query.
        /// It is assumed that String.CheckInjection() or String.CheckSqlInjection()
        /// has been called once on string parameter "parameterValue". That call should result in a 
        /// duplication of numbers of instances of the character '.
        /// 
        /// Possible combinations when handling ' in string parameters:
        /// SQL Parameters and static SQL in stored procedure => 1 ' for each ' in the original text.
        /// Parameters as text and static SQL in stored procedure => 2 ' for each ' in the original text.
        /// Dynamic SQL is used in stored procedure =>  Double the number of ' in the original text
        /// foreach time that the parameter is interpreted as text in the stored procedure. 
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        /// <param name="parameterValue">Value of parameter to add.</param>
        /// <param name="dynamicSqlCount">
        /// Number of times that this parameter is interpreted as text
        /// in dynamic SQL in the stored procedure.
        /// </param>
        public void AddParameter(String parameterName, String parameterValue, Int32 dynamicSqlCount = 0)
        {
            if (IsSqlParameterUsed && (dynamicSqlCount <= 0))
            {
                parameterValue = RemoveDoubleQuotationMarks(parameterValue);
            }

            while ((dynamicSqlCount > 1) && parameterValue.IsNotEmpty())
            {
                parameterValue = parameterValue.Replace("'", "''");
                dynamicSqlCount--;
            }

            if (IsSqlParameterUsed)
            {
                SqlParameter parameter;

                parameter = new SqlParameter();
                parameter.ParameterName = parameterName;
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Value = parameterValue;
                _sqlParameters.Add(parameter);
            }
            else
            {
                if (parameterValue.IsNotNull())
                {
                    AddParameterName(parameterName);
                    _command.Append("N'");
                    _command.Append(parameterValue);
                    _command.Append("'");
                }
            }
        }

        /// <summary>
        /// Get the sql query.
        /// </summary>
        /// <returns>The sql query.</returns>
        public String GetCommand()
        {
            return _command.ToString();
        }

        /// <summary>
        /// Get the SqlParameters
        /// </summary>
        /// <returns>List of SqlParameter.</returns>
        public List<SqlParameter> GetSqlParameters()
        {
            return _sqlParameters;
        }

        /// <summary>
        /// Replaces '' (two single quotationmarks) with ' (single quotationmark)
        /// The .NET SqlParameter class adds a single quotationmark to prevent SQL injection. 
        /// The same is done in ArtDatabanken.WebService.StringExtension.CheckSqlInjection.
        /// This method handles the problem.
        /// </summary>
        /// <param name='text'>Text to test.</param>
        /// <returns>The string where two single quotationmarks has been replaced by a single quotationmark.</returns>
        private String RemoveDoubleQuotationMarks(String text)
        {
            if (text.IsNotEmpty())
            {
                return text.Replace("''", "'");
            }
            else
            {
                return text;
            }
        }
    }
}

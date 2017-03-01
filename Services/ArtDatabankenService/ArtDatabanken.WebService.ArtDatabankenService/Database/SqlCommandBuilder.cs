using System;
using System.Text;

namespace ArtDatabanken.WebService.ArtDatabankenService.Database
{
    /// <summary>
    /// Class that simplifies the construction of an sql
    /// query that is based on a call to a stored procedure.
    /// </summary>
    public class SqlCommandBuilder
    {
        private Boolean _hasParameters;
        private StringBuilder _command;

        /// <summary>
        /// Constructor that ties the sql query to a stored procedure.
        /// </summary>
        /// <param name="storedProcedure">Name of stored procedure to call.</param>
        public SqlCommandBuilder(String storedProcedure)
        {
            _command = new StringBuilder("EXECUTE " + storedProcedure);
            _hasParameters = false;
        }

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

        /// <summary>
        /// Add a DateTime parameter to the sql query.
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        /// <param name="parameterValue">Value of parameter to add.</param>
        public void AddParameter(String parameterName, DateTime parameterValue)
        {
            AddParameterName(parameterName);
            _command.Append("'" + parameterValue.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'");
        }

        /// <summary>
        /// Add a Double parameter to the sql query.
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        /// <param name="parameterValue">Value of parameter to add.</param>
        public void AddParameter(String parameterName, Double parameterValue)
        {
            AddParameterName(parameterName);
            _command.Append(parameterValue.ToString().Replace(",", "."));
        }

        /// <summary>
        /// Add an Int32 parameter to the sql query.
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        /// <param name="parameterValue">Value of parameter to add.</param>
        public void AddParameter(String parameterName, Int32 parameterValue)
        {
            AddParameterName(parameterName);
            _command.Append(parameterValue.ToString());
        }

        /// <summary>
        /// Add an Int64 parameter to the sql query.
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        /// <param name="parameterValue">Value of parameter to add.</param>
        public void AddParameter(String parameterName, Int64 parameterValue)
        {
            AddParameterName(parameterName);
            _command.Append(parameterValue.ToString());
        }

        /// <summary>
        /// Add a String parameter to the sql query.
        /// </summary>
        /// <param name="parameterName">Name of parameter to add.</param>
        /// <param name="parameterValue">Value of parameter to add.</param>
        public void AddParameter(String parameterName, String parameterValue)
        {
            if (parameterValue.IsNotNull())
            {
                AddParameterName(parameterName);
                _command.Append("N'");
                _command.Append(parameterValue);
                _command.Append("'");
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
    }
}

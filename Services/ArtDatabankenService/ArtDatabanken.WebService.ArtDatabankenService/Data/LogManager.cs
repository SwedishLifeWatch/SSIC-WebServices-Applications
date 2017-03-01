using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.ArtDatabankenService;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Manager of log handling.
    /// </summary>
    public class LogManager
    {
        /// <summary>
        /// Max number of log rows that can be retrieved
        /// in one request.
        /// </summary>
        private const Int32 MAX_LOG_ROWS = 10000;

        /// <summary>
        /// Delete trace information from the web service log.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteTrace(WebServiceContext context)
        {
            // Check access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, ApplicationIdentifier.WebAdministration, AuthorityIdentifier.WebServiceAdministrator);
            DataServer.DeleteTrace(context);
        }

        /// <summary>
        /// Get rows from the web service log
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="type">Get log rows of this type.</param>
        /// <param name="userName">Get log rows for this user. May be empty.</param>
        /// <param name="rowCount">Maximum number of log rows to get.</param>
        /// <returns> Requested web log rows.</returns>
        public static List<WebLogRow> GetLog(WebServiceContext context,
                                             LogType type,
                                             String userName,
                                             Int32 rowCount)
        {
            List<WebLogRow> logRows;
            String typeString;

            // Check access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, ApplicationIdentifier.WebAdministration, AuthorityIdentifier.WebServiceAdministrator);

            // Check arguments.
            if (type == LogType.None)
            {
                typeString = "";
            }
            else
            {
                typeString = type.ToString();
            }
            userName = userName.CheckSqlInjection();
            if (MAX_LOG_ROWS < rowCount)
            {
                rowCount = MAX_LOG_ROWS;
            }

            // Get information from database.
            logRows = new List<WebLogRow>();
            if (rowCount > 0)
            {
                using (DataReader dataReader = DataServer.GetLog(context, typeString, userName, rowCount))
                {
                    while (dataReader.Read())
                    {
                        logRows.Add(new WebLogRow(dataReader));
                    }
                }
            }
            return logRows;
        }

        /// <summary>
        /// Add information to the web service log.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="text">Log text.</param>
        /// <param name="type">Type of log entry.</param>
        /// <param name="information">Extended information about the log entry.</param>
        public static void Log(WebServiceContext context, 
                               String text,
                               LogType type,
                               String information)
        {
            DataServer.UpdateLog(context,
                                 text,
                                 type.ToString(),
                                 information);
        }

        /// <summary>
        /// Add information about exception to the web service log.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='exception'>Exception to log.</param>
        public static void LogError(WebServiceContext context, 
                                    Exception exception)
        {
            try
            {
                DataServer.UpdateLog(context,
                                     exception.Message,
                                     LogType.Error.ToString(),
                                     exception.StackTrace);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Add information about security related exception to the web service log.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='exception'>Exception to log.</param>
        public static void LogSecurityError(WebServiceContext context, 
                                            Exception exception)
        {
            try
            {
                DataServer.UpdateLog(context,
                                     exception.Message,
                                     LogType.Security.ToString(),
                                     exception.StackTrace);
            }
            catch
            {
            }
        }
    }
}

using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Database;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Manager of log handling.
    /// </summary>
    public interface ILogManager
    {
        /// <summary>
        /// Delete trace information from the web service log.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        void DeleteTrace(WebServiceContext context);

        /// <summary>
        /// Get rows from the web service log
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="type">Get log rows of this type.</param>
        /// <param name="userName">Get log rows for this user. May be empty.</param>
        /// <param name="rowCount">Maximum number of log rows to get.</param>
        /// <returns> Requested web log rows.</returns>
        List<WebLogRow> GetLog(WebServiceContext context,
                               LogType type,
                               String userName,
                               Int32 rowCount);

        /// <summary>
        /// Add information to the web service log.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="text">Log text.</param>
        /// <param name="type">Type of log entry.</param>
        /// <param name="information">Extended information about the log entry.</param>
        void Log(WebServiceContext context,
                 String text,
                 LogType type,
                 String information);

        /// <summary>
        /// Add information about exception to the web service log.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='exception'>Exception to log.</param>
        void LogError(WebServiceContext context, Exception exception);

        /// <summary>
        /// Add information about security related exception to the web service log.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='exception'>Exception to log.</param>
        void LogSecurityError(WebServiceContext context,
                              Exception exception);
    }
}

using System;
using System.Collections.Generic;
//using System.IO;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Database;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Manager of log handling.
    /// </summary>
    public class LogManager : ILogManager
    {
        //public static object LockingTarget = new object(); 

        /// <summary>
        /// Delete trace information from the web service log.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public virtual void DeleteTrace(WebServiceContext context)
        {
            // Check access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.WebServiceAdministrator);
            context.GetDatabase().DeleteTrace();
        }

        /// <summary>
        /// Get rows from the web service log
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="type">Get log rows of this type.</param>
        /// <param name="userName">Get log rows for this user. May be empty.</param>
        /// <param name="rowCount">Maximum number of log rows to get.</param>
        /// <returns> Requested web log rows.</returns>
        public virtual List<WebLogRow> GetLog(WebServiceContext context,
                                              LogType type,
                                              String userName,
                                              Int32 rowCount)
        {
            List<WebLogRow> logRows;
            String typeString;
            WebLogRow logRow;

            // Check access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.WebServiceAdministrator);

            // Check arguments.
            if (type == LogType.None)
            {
                typeString = String.Empty;
            }
            else
            {
                typeString = type.ToString();
            }
            userName = userName.CheckInjection();
            if (Settings.Default.MaxReturnedLogRows < rowCount)
            {
                rowCount = Settings.Default.MaxReturnedLogRows;
            }

            // Get information from database.
            logRows = new List<WebLogRow>();
            if (rowCount > 0)
            {
                using (DataReader dataReader = context.GetDatabase().GetLog(typeString, userName, rowCount))
                {
                    while (dataReader.Read())
                    {
                        logRow = new WebLogRow();
                        logRow.LoadData(dataReader);
                        logRows.Add(logRow);
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
        public virtual void Log(WebServiceContext context, 
                                String text,
                                LogType type,
                                String information)
        {
            UpdateLog(context,
                      text,
                      type.ToString(),
                      information);
        }

        /// <summary>
        /// Add information about exception to the web service log.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='exception'>Exception to log.</param>
        public virtual void LogError(WebServiceContext context, 
                                     Exception exception)
        {
            try
            {
                UpdateLog(context,
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
        public virtual void LogSecurityError(WebServiceContext context, 
                                             Exception exception)
        {
            try
            {
                UpdateLog(context,
                          exception.Message,
                          LogType.Security.ToString(),
                          exception.StackTrace);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Add an entry to the web service log
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="text">Log text.</param>
        /// <param name="type">Type of log entry.</param>
        /// <param name="information">Extended information about the log entry.</param>
        private void UpdateLog(WebServiceContext context,
                               String text,
                               String type,
                               String information)
        {
            if (context.GetDatabase().HasPendingTransaction())
            {
                // Create new database connection that has no transaction.
                // The log entry may be removed (rollback of transaction) 
                // if the database connection has an active transaction.
                using (WebServiceDataServer database = WebServiceData.DatabaseManager.GetDatabase(context))
                {
                    database.UpdateLog(text,
                                       type,
                                       information,
                                       context.ClientToken.UserName,
                                       context.ClientToken.ClientIpAddress,
                                       context.ClientToken.ApplicationIdentifier);
                }
            }
            else
            {
                context.GetDatabase().UpdateLog(text,
                                                type,
                                                information,
                                                context.ClientToken.UserName,
                                                context.ClientToken.ClientIpAddress,
                                                context.ClientToken.ApplicationIdentifier);
            }
        }

        ///// <summary>
        ///// Write log-str to file
        ///// This function needs more development.
        ///// Used by Gunnar during RevisionCheckIn problem April 2013
        ///// </summary>
        ///// <param name="message">The msg to write to the file.</param>
        ///// <returns> Requested web log rows.</returns>
        //public static void WriteMessage(string message)
        //{
        //    lock (LockingTarget)
        //    {
        //        try
        //        {
        //            string path = "C:/inetpub/wwwroot/TaxonService/logs/" + "log " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
        //            if (!File.Exists(path))
        //            {
        //                File.Create(path).Close();
        //            }

        //            using (StreamWriter w = File.AppendText(path))
        //            {
        //                w.WriteLine("\r\nLog Entry : ");
        //                w.WriteLine("{0}", DateTime.Now.ToShortTimeString());
        //                w.WriteLine(message);
        //                w.Flush();
        //                w.Close();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // nothing
        //        }
        //    }
        //}
    }
}

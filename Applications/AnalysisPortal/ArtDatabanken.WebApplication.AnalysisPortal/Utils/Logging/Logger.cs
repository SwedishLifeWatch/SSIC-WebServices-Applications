using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using log4net;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Logging
{
    public static class Logger
    {
        /// <summary>
        /// log4net logger for this Logger
        /// </summary>
        private static readonly ILog mLogger = LogManager.GetLogger(typeof(Logger));
        
        public static object LockingTarget = new object();

        public static void WriteMessage(string format, params object[] args)
        {
            WriteMessage(string.Format(format, args));
        }

        public static void WriteMessage(string message)
        {
            lock (LockingTarget)
            {
                try
                {
                    mLogger.Info(message);
                }
                catch (Exception)
                {
                    //WriteError(ex.Message);
                }
            }
        }

        public static void WriteExceptionAndHistoryLog(Exception ex)
        {
            lock (LockingTarget)
            {
                try
                {
                    string ip = GetIPNumber();
                    string ipNumber = string.Format("IP number: {0}, Name: {1}", ip, HttpContext.Current.Request.UserHostName ?? "-");

                    string path = Resources.AppSettings.Default.PathToTempDirectory + "error and history " +
                                  DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    if (!File.Exists(HostingEnvironment.MapPath(path)))
                    {
                        File.Create(HostingEnvironment.MapPath(path)).Close();
                    }

                    using (StreamWriter w = File.AppendText(HostingEnvironment.MapPath(path)))
                    {
                        w.WriteLine("\r\nLog Entry : ");
                        w.WriteLine("{0}", DateTime.Now.ToShortTimeString());
                        w.WriteLine(ipNumber);
                        w.WriteLine("Error in: " + System.Web.HttpContext.Current.Request.Url);
                        w.WriteLine("Error Message: " + ex.Message);
                        if (ex.GetType() != typeof(HttpException))
                        {
                            w.WriteLine(ex.StackTrace);
                            LogEventHistory logEventHistory = SessionHandler.LogEventHistory;
                            if (logEventHistory != null)
                            {
                                w.WriteLine("HISTORY");
                                w.WriteLine("==========");
                                for (int i = logEventHistory.HistoryItems.Count - 1; i >= 0; i--)
                                {
                                    LogEventHistoryItem item = logEventHistory.HistoryItems[i];
                                    w.WriteLine("Event number: {0}. Time: {1}", i + 1, item.Date.ToLongTimeString());
                                    //w.WriteLine("Action: {0}. Controller: {1}, HttpAction: {2}", item.Action ?? "-", item.Controller ?? "-", item.HttpAction ?? "-");
                                    w.WriteLine("Url: {0}, HttpAction: {1}", item.Url ?? "-", item.HttpAction ?? "-");
                                    w.WriteLine("Referrer: {0}", item.Referrer);
                                    w.WriteLine("Form: {0}", item.Form);
                                    w.WriteLine("TaxonId: {0}, RevisionId: {1}", item.TaxonId.HasValue ? item.TaxonId.Value.ToString() : "-", item.RevisionId.HasValue ? item.RevisionId.Value.ToString() : "-");
                                    w.WriteLine("User: {0}, Role: {1}", item.UserName ?? "-", item.UserRole ?? "-");
                                    w.WriteLine("====================================================================");
                                }
                            }
                        }
                        w.WriteLine("__________________________");

                        w.Flush();
                        w.Close();
                    }
                }
                catch (Exception)
                {
                    //WriteError(ex.Message);
                }
            }
        }

        public static void WriteException(Exception ex)
        {
            // WriteExceptionAndHistoryLog(ex);
            lock (LockingTarget)
            {
                try
                {
                    mLogger.Error(ex);
                }
                catch (Exception)
                {
                    //WriteError(ex.Message);
                }
            }
        }

        /// <summary>
        /// Writes a Debug Exception
        /// </summary>
        /// <param name="exception"></param>
        public static void WriteDebugException(Exception ex)
        {
            // WriteExceptionAndHistoryLog(ex);
            lock (LockingTarget)
            {
                try
                {
                    mLogger.Debug(ex);
                }
                catch (Exception)
                {
                    //WriteError(ex.Message);
                }
            }
        }

        private static string GetIPNumber()
        {
            string ip = "";
            string ipNumber = "";
            if (HttpContext.Current != null)
            {
                HttpRequest request = HttpContext.Current.Request;
                ipNumber = string.Format("IP number: {0}, Name: {1}", request.UserHostAddress ?? "-", request.UserHostName ?? "-");
                // Look for a proxy address first 
                ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                // If there is no proxy, get the standard remote address 
                if (ip == null || ip.ToLower() == "unknown" || ip == "")
                {
                    ip = request.ServerVariables["REMOTE_ADDR"];
                }
            }
            return ip ?? "";
        }
    }
}

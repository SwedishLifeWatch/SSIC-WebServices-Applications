using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;
using log4net;
using log4net.Repository.Hierarchy;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public static class DyntaxaLogger
    {
        /// <summary>
        /// log4net logger for this Logger
        /// </summary>
        private static readonly ILog mLogger = LogManager.GetLogger(typeof(Logger));

        private static object lockingTarget = new object(); 

        public static void WriteMessage(string format, params object[] args)
        {
            WriteMessage(string.Format(format, args));
        }

        public static void WriteMessage(string message)
        {
            lock (lockingTarget)
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

        public static string GetUrl()
        {
            string url, tempUrl;

            if (HttpContext.Current.IsNull())
            {
                return "In method GetUrl(): HttpContext.Current is null";
            }

            if (HttpContext.Current.Request.IsNull())
            {
                return "In method GetUrl(): HttpContext.Current.Request is null";
            }

            if (HttpContext.Current.Request.Url.IsNull())
            {
                return "In method GetUrl(): HttpContext.Current.Request.Url is null";
            }

            url = HttpContext.Current.Request.Url.ToString();
            if (url.Contains("credentials"))
            {
                // Remove credentials from logged information.
                tempUrl = url.Substring(url.IndexOf("credentials", StringComparison.CurrentCulture));
                if (tempUrl.Contains("&"))
                {
                    tempUrl = tempUrl.Substring(tempUrl.IndexOf("&", StringComparison.CurrentCulture));
                }
                else
                {
                    tempUrl = String.Empty;
                }

                url = url.Substring(0, url.IndexOf("credentials", StringComparison.CurrentCulture)) + tempUrl;
            }

            return url;
        }

        public static void WriteDebugException(Exception ex)
        {
            lock (lockingTarget)
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

        public static void WriteException(Exception ex)
        {
            lock (lockingTarget)
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

        private static string GetIp()
        {
            var ip = "";

            if (HttpContext.Current.IsNull())
            {
                return "In method GetIp(): HttpContext.Current is null";
            }

            if (HttpContext.Current.Request.IsNull())
            {
                return "In method GetIp(): HttpContext.Current.Request is null";
            }

            if (HttpContext.Current.Request.ServerVariables.IsNull())
            {
                return "In method GetIp(): HttpContext.Current.Request.ServerVariables is null";
            }

            HttpRequest request = HttpContext.Current.Request;
            // Look for a proxy address first 
            ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            // If there is no proxy, get the standard remote address 
            if (ip == null || ip.ToLower() == "unknown" || ip == "")
            {
                ip = request.ServerVariables["REMOTE_ADDR"];
            }

            return ip ?? "";
        }

        private static string GetIPNumber()
        {
            if (HttpContext.Current.IsNull())
            {
                return "In method GetIPNumber(): HttpContext.Current is null";
            }

            if (HttpContext.Current.Request.IsNull())
            {
                return "In method GetIPNumber(): HttpContext.Current.Request is null";
            }

            string ipNumber = string.Format("IP number: {0}, Name: {1}", GetIp(), HttpContext.Current.Request.UserHostName ?? "-");
            return ipNumber;
        }

        private static string GetSession()
        {
            if (HttpContext.Current.IsNull())
            {
                return "In method GetSession(): HttpContext.Current is null";
            }

            if (HttpContext.Current.Session.IsNull())
            {
                return "In method GetSession(): HttpContext.Current.Session is null";
            }

            return "Session: " + HttpContext.Current.Session.SessionID;
        }

        private static string GetUser()
        {
            IUserContext userContext;

            try
            {
                userContext = CoreData.UserManager.GetCurrentUser();
                if (userContext.IsNull())
                {
                    return "User context is null.";
                }

                if (userContext.User.IsNull())
                {
                    return "No user in user context.";
                }

                if (userContext.CurrentRole.IsNull())
                {
                    return "User: " + userContext.User.UserName;
                }
                else
                {
                    return "User: " + userContext.User.UserName + " Role:" + userContext.CurrentRole.Name;
                }
            }
            catch (Exception exception)
            {
                return "Failed to retrieve user context. " + exception.Message;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers
{
    /// <summary>
    /// This class stores the last user session id in a dictionary.
    /// This is used for security reasons in the action MySettings/LoadAutoSavedSettings.
    /// </summary>
    public static class LastUserSessionIdManager
    {
        private static readonly Dictionary<string, string> LastUserSessionId = new Dictionary<string, string>();

        public static bool IsLastUserSessionIdOk(string userName, string sessionId)
        {
            if (LastUserSessionId.ContainsKey(userName))
            {
                return LastUserSessionId[userName] == sessionId;                
            }
            return false;
        }

        public static void UpdateLastUserSessionId(string userName, string sessionId)
        {
            if (LastUserSessionId.ContainsKey(userName))
            {
                LastUserSessionId[userName] = sessionId;
            }
            else
            {
                LastUserSessionId.Add(userName, sessionId);
            }
        }
    }
}

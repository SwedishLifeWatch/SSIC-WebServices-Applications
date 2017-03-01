using System.Collections;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Logging;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers
{
    /// <summary>
    /// Handles session storage.
    /// <remarks>
    /// In test mode you can call SetSessionHelper(...) to use a mock storage.
    /// </remarks>   
    /// </summary>
    public static class SessionHandler
    {
        private static ISessionHelper _sessionHelper;

        /// <summary>
        /// Static constructor
        /// </summary>
        static SessionHandler()
        {
            _sessionHelper = new HttpContextSessionHelper();
        }

        /// <summary>
        /// Sets which session helper to use.
        /// </summary>
        /// <param name="sessionHelper"></param>
        public static void SetSessionHelper(ISessionHelper sessionHelper) // use this in test
        {
            _sessionHelper = sessionHelper;
        }     

        /// <summary>
        /// Current language ISO code.
        /// </summary>
        public static string Language
        {
            get
            {                
                return _sessionHelper.GetFromSession<string>(LanguageString);
            }
            set
            {                
                _sessionHelper.SetInSession(LanguageString, value);
            }            
        }
        private const string LanguageString = "language";

        /// <summary>
        /// Current user context.
        /// </summary>
        public static IUserContext UserContext
        {
            get
            {                
                return _sessionHelper.GetFromSession<IUserContext>(UserContextString);
            }
            set
            {                
                _sessionHelper.SetInSession(UserContextString, value);
            }            
        }
        private const string UserContextString = "userContext";

        /// <summary>
        /// History with log items.
        /// <remarks>
        /// Used by Logger when we get an unexpected Exception.
        /// </remarks>
        /// </summary>
        public static LogEventHistory LogEventHistory
        {
            get
            {                
                return _sessionHelper.GetFromSession<LogEventHistory>(LogEventHistoryString);
            }
            set
            {                
                _sessionHelper.SetInSession(LogEventHistoryString, value);
            }
        }
        private const string LogEventHistoryString = "logEventHistory";

        /// <summary>
        /// Current settings object        
        /// </summary>
        public static MySettings.MySettings MySettings
        {
            get
            {
                return _sessionHelper.GetFromSession<MySettings.MySettings>(MySettingsString);
            }
            set
            {                
                _sessionHelper.SetInSession(MySettingsString, value);
            }
        }
        private const string MySettingsString = "mySettings";

        /// <summary>
        /// Gets or sets the current page info.
        /// </summary>
        public static PageInfo CurrentPage
        {
            get
            {
                return _sessionHelper.GetFromSession<PageInfo>(CurrentPageInfoString);
            }
            set
            {
                _sessionHelper.SetInSession(CurrentPageInfoString, value);
            }
        }
        private const string CurrentPageInfoString = "currentPageInfo";

        public static CalculatedDataItemCollection Results
        {
            get
            {
                return _sessionHelper.GetFromSession<CalculatedDataItemCollection>(ResultsString);
            }
            set
            {
                _sessionHelper.SetInSession(ResultsString, value);
            }
        }
        private const string ResultsString = "results";

        public static List<UserMessage> UserMessages
        {
            get
            {
                List<UserMessage> list = _sessionHelper.GetFromSession<List<UserMessage>>(UserMessagesString);
                if (list == null)
                {
                    list = new List<UserMessage>();
                    _sessionHelper.SetInSession(UserMessagesString, list);
                }
                return list;
            }            
        }
        private const string UserMessagesString = "userMessages";

        public static string MySettingsMessage
        {
            get
            {
                return _sessionHelper.GetFromSession<string>(MySettingsMessageString);
            }
            set
            {
                _sessionHelper.SetInSession(MySettingsMessageString, value);
            }
        }
        private const string MySettingsMessageString = "MySettingsMessage";
    }
}

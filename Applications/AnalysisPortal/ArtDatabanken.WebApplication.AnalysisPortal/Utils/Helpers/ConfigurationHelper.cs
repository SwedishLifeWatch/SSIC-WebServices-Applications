using System;
using System.Configuration;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers
{
    public static class ConfigurationHelper
    {
        public static T GetValue<T>(string key, T defaultValue = default(T))
        {
            try
            {
                return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
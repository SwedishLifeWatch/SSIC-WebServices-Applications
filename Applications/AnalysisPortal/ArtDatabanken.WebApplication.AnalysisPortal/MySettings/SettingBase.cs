using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings
{
    /// <summary>
    /// This class is an abstract base class for settings in MySettings
    /// </summary>
    [DataContract]
    public abstract class SettingBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether the object is initialized.
        /// </summary>        
        protected bool Initialized
        {
            get
            {
                return _initialized;
            }

            set
            {
                _initialized = value;
                foreach (var settingBase in GetSettingBasePropertyObjects())
                {
                    settingBase.Initialized = value;
                }
            }
        }
        private bool _initialized;

        /// <summary>
        /// This method ensures that all properties is initialized.
        /// Specifically it initializes properties that is null when
        /// an object is constructed or loaded by deserializing a file.
        /// </summary>        
        public virtual void EnsureInitialized()
        {
            foreach (PropertyInfo settingBaseProperty in GetSettingBaseProperties())
            {
                SettingBase settingBase = (SettingBase)settingBaseProperty.GetValue(this, null);
                if (settingBase.IsNull())
                {                    
                    settingBase = (SettingBase)Activator.CreateInstance(settingBaseProperty.PropertyType);
                    settingBaseProperty.SetValue(this, settingBase, null); 
                    settingBase.EnsureInitialized();
                }
                else
                {
                    settingBase.EnsureInitialized();
                }
            }            
        }

        /// <summary>
        /// Determines whether any settings has been done.
        /// </summary>        
        public virtual bool HasSettings
        {
            get
            {
                foreach (var settingBase in GetSettingBasePropertyObjects())
                {
                    if (settingBase.HasSettings)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        
        /// <summary>
        /// Determines whether the settings is the default settings.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the settings is default settings; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsSettingsDefault()
        {
            foreach (SettingBase settingBase in GetSettingBasePropertyObjects())
            {
                if (!settingBase.IsSettingsDefault())
                {
                    return false;
                }
            }

            return true;
        }
      
        /// <summary>
        /// Check if we have made changes since the last time we made a calculation.
        /// Return true any changes has been made since the last time we made a calculation, otherwise false.        
        /// If false, the result cache is up to date. Otherwise it needs to be recalculated.
        /// </summary>  
        public virtual bool ResultCacheNeedsRefresh
        {
            get
            {
                if (_resultCacheNeedsRefresh)
                {
                    return true;
                }

                foreach (var settingBase in GetSettingBasePropertyObjects())
                {
                    if (settingBase.ResultCacheNeedsRefresh)
                    {
                        return true;
                    }
                }
                return false;
            }
            set
            {                                
                _resultCacheNeedsRefresh = value;                
                foreach (var settingBase in GetSettingBasePropertyObjects())
                {
                    settingBase.ResultCacheNeedsRefresh = value;
                }                                          
                if (!Initialized)
                {
                    return;
                }

                if (_resultCacheNeedsRefresh)
                {
                    if (SessionHandler.Results != null)
                    {
                        SessionHandler.Results.Clear();
                    }

                    SessionHandler.MySettingsMessage = "MySettings updated";
                }
            }
        }
        private bool _resultCacheNeedsRefresh;

        private IEnumerable<PropertyInfo> GetSettingBaseProperties()
        {
            var properties = GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.PropertyType.IsSubclassOf(typeof(SettingBase)))
                {
                    yield return propertyInfo;
                }
            }
        }

        private IEnumerable<SettingBase> GetSettingBasePropertyObjects()
        {
            foreach (PropertyInfo settingBaseProperty in GetSettingBaseProperties())
            {
                SettingBase obj = (SettingBase)settingBaseProperty.GetValue(this, null);
                if (obj != null)
                {
                    yield return obj;
                }
            }
        }
    }
}

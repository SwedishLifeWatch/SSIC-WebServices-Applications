using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter
{
    /// <summary>
    /// This class stores Red list settings
    /// </summary>
    [DataContract]
    public sealed class RedListSetting : SettingBase
    {
        private bool _isActive;
        private IEnumerable<RedListCategory> _categories;
        
        /// <summary>
        /// Gets or sets whether OccurrenceSetting is active or not.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is checked; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsActive
        {
            get
            {
                return _isActive;
            }

            set
            {
                _isActive = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        [DataMember]
        public IEnumerable<RedListCategory> Categories
        {
            get
            {
                return _categories;
            }

            set
            {
                _categories = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        /// <summary>
        /// Indicates whether this instance has values set or not 
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has values; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettings
        {            
            get
            {
                return _categories != null && _categories.Any();
            }
        }

        public RedListSetting()
        {            
            ResetSettings();
        }

        public override bool IsSettingsDefault()
        {
            return _categories == null || !_categories.Any();
        }

        public void ResetSettings()
        {
            _categories = null;
            IsActive = true;            
        }
    }
}

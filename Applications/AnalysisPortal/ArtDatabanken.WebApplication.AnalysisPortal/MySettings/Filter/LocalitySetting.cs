using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter
{
    /// <summary>
    /// This class stores locality settings.
    /// </summary>
    [DataContract]
    public sealed class LocalitySetting : SettingBase
    {
        ////private bool _isActive;
        private string _localityName;
        private StringCompareOperator _compareOperator;

        /////// <summary>
        /////// Gets or sets whether locality settings is active or not.
        /////// </summary>
        /////// <value>
        ///////  <c>true</c> if locality settings is active; otherwise, <c>false</c>.
        /////// </value>
        ////[DataMember]
        ////public bool IsActive
        ////{
        ////    get
        ////    {
        ////        return _isActive;
        ////    }

        ////    set
        ////    {
        ////        _isActive = value;
        ////        ResultCacheNeedsRefresh = true;
        ////    }
        ////}

        /// <summary>
        /// Gets or sets the name of the locality.
        /// </summary>
        /// <value>
        /// The name of the locality.
        /// </value>
        [DataMember]
        public string LocalityName
        {
            get
            {
                return _localityName;
            }

            set
            {
                _localityName = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        /// <summary>
        /// Gets or sets the compare operator.
        /// </summary>
        /// <value>
        /// The compare operator.
        /// </value>
        public StringCompareOperator CompareOperator
        {
            get
            {
                return _compareOperator;
            }

            set
            {
                _compareOperator = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        /// <summary>
        /// Indicates whether this instance has values set or not. 
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has values; otherwise, <c>false</c>.
        /// </value>
        /// <summary>
        /// Indicates whether this instance has values set or not. 
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has values; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettings
        {            
            get
            {
                return true;
            }
        }
      
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalitySetting"/> class.
        /// </summary>
        public LocalitySetting()
        {            
            ResetSettings();
        }

        /// <summary>
        /// Determines whether the settings is the default settings.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the settings is default settings; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsSettingsDefault()
        {
            return LocalityName == null && CompareOperator == StringCompareOperator.BeginsWith;              
        }

        /// <summary>
        /// Resets the settings.
        /// </summary>
        public void ResetSettings()
        {
            LocalityName = null;
            CompareOperator = StringCompareOperator.BeginsWith;
        }
    }
}

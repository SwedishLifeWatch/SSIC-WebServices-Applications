using System.Runtime.Serialization;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter
{
    /// <summary>
    /// This class stores filtered accuracy settings
    /// </summary>
    [DataContract]
    public sealed class AccuracySetting : SettingBase
    {
        private bool _isActive;
        public int _maxCoordinateAccuracy;
        public bool _inclusive;
        //public bool _exclusive;
        private bool _isCoordinateAccuracyActive;

        /// <summary>
        /// Gets or sets whether AccuracySetting is active or not.
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
        public bool Inclusive
        {
            get
            {
                return _inclusive;
            }

            set
            {
                _inclusive = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        //[DataMember]
        //public bool Exclusive
        //{
        //    get { return _exclusive; }
        //    set
        //    {
        //        _exclusive = value;
        //        ResultCacheNeedsRefresh = true;
        //    }
        //}

        [DataMember]
        public int MaxCoordinateAccuracy
        {
            get
            {
                return _maxCoordinateAccuracy;
            }

            set
            {
                _maxCoordinateAccuracy = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        [DataMember]
        public bool IsCoordinateAccuracyActive
        {
            get
            {
                return _isCoordinateAccuracyActive;
            }

            set
            {
                _isCoordinateAccuracyActive = value;
                ResultCacheNeedsRefresh = true;
            }
        }
        
        /// <summary>
        /// Indicates whether this instance has values set or not 
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has values; otherwise, <c>false</c>.
        /// </value>
        //public override bool HasSettings
        //{
        //    get { return false; }
        //}

        //public override bool IsSettingsDefault()
        //{
        //    return true;
        //}

        //public override bool ResetSettings()
        //{
        //    return true;
        //}

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
                return IsCoordinateAccuracyActive;
            }
        }

        public AccuracySetting()
        {            
            ResetSettings();
        }

        public override bool IsSettingsDefault()
        {
            return Inclusive && (MaxCoordinateAccuracy == 100) && !IsCoordinateAccuracyActive;
        }

        public void ResetSettings()
        {
            Inclusive = true;
            IsActive = false;
            MaxCoordinateAccuracy = 100;
            IsCoordinateAccuracyActive = false;
        }
    }
}

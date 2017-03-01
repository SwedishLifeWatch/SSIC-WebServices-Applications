using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation
{
    public sealed class PresentationMapSetting : SettingBase
    {
        //private CoordinateSystemId _coordinateSystemId;
        private CoordinateSystemId _presentationCoordinateSystemId;
        private CoordinateSystemId _downloadCoordinateSystemId;

        /// <summary>
        /// Gets or sets whether PresentationViewSetting is active or not.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is checked; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsActive { get; set; } // todo - remove?      

        /// <summary>
        /// The presentation coordinate system identifier.
        /// </summary>
        [DataMember]
        public CoordinateSystemId PresentationCoordinateSystemId
        {
            get
            {
                return _presentationCoordinateSystemId;
            }
            set
            {
                _presentationCoordinateSystemId = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        /// <summary>
        /// The download coordinate system identifier.
        /// </summary>
        [DataMember]
        public CoordinateSystemId DownloadCoordinateSystemId
        {
            get
            {
                return _downloadCoordinateSystemId;
            }
            set
            {
                _downloadCoordinateSystemId = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        /// <summary>
        /// Gets the display coordinate system.
        /// </summary>        
        public CoordinateSystem DisplayCoordinateSystem
        {
            get
            {
                return new CoordinateSystem(PresentationCoordinateSystemId);
            }
        }

        /// <summary>
        /// Determines whether any settings has been done.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has settings; otherwise, <c>false</c>.
        /// </returns>
        public override bool HasSettings // todo - remove?
        {
            get { return false; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationTableSetting"/> class.
        /// </summary>
        public PresentationMapSetting()
        {
            PresentationCoordinateSystemId = CoordinateSystemId.GoogleMercator;
            DownloadCoordinateSystemId = CoordinateSystemId.SWEREF99_TM;
            EnsureInitialized();
            IsActive = true;
        }

        /// <summary>
        /// Resets the settings.
        /// </summary>
        public void ResetSettings()
        {
            PresentationCoordinateSystemId = CoordinateSystemId.GoogleMercator;
            DownloadCoordinateSystemId = CoordinateSystemId.SWEREF99_TM;
        }

        /// <summary>
        /// Determines whether the settings is the default settings.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the settings is default settings; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsSettingsDefault()
        {
            if (PresentationCoordinateSystemId == CoordinateSystemId.GoogleMercator && 
                DownloadCoordinateSystemId == CoordinateSystemId.SWEREF99_TM)
            {
                return true;
            }

            return false;
        }
    }
}
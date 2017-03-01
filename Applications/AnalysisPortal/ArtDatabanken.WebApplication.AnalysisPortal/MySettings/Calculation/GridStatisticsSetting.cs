using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation
{
    [DataContract]
    public sealed class GridStatisticsSetting : SettingBase
    {
        private int? _gridSize;
        private int? _coordinateSystemId;
        private bool _isActive;
        private bool _calculateNumberOfObservations;
        private bool _calculateNumberOfTaxa;
        private int _wfsGridStatisticsCalculationModeId;
        private int? _wfsGridStatisticsLayerId;
        private bool _generateAllGridCells;

        /// <summary>
        /// Gets or sets whether GridStatistics is active or not.
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
        public int? CoordinateSystemId
        {
            get
            {
                return _coordinateSystemId;
            }

            set
            {
                _coordinateSystemId = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether all grid cells should be generated.
        /// Even those who do not have any data.
        /// </summary>
        /// <value>
        /// <c>true</c> if all grid cells should be generated; otherwise, <c>false</c>.
        /// </value>
        [DataMember]        
        public bool GenerateAllGridCells
        {
            get
            {
                return _generateAllGridCells;
            }

            set
            {
                _generateAllGridCells = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        /// <summary>
        /// The grid size in meters.
        /// </summary>        
        [DataMember]
        public int? GridSize
        {
            get
            {
                return _gridSize;
            }

            set
            {
                _gridSize = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        [DataMember]
        public bool CalculateNumberOfObservations
        {
            get
            {
                return _calculateNumberOfObservations;
            }

            set
            {
                _calculateNumberOfObservations = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        [DataMember]
        public bool CalculateNumberOfTaxa
        {
            get
            {
                return _calculateNumberOfTaxa;
            }

            set
            {
                _calculateNumberOfTaxa = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        [DataMember]
        public int WfsGridStatisticsCalculationModeId
        {
            get
            {
                return _wfsGridStatisticsCalculationModeId;
            }

            set
            {
                _wfsGridStatisticsCalculationModeId = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        [DataMember]
        public int? WfsGridStatisticsLayerId
        {
            get
            {
                return _wfsGridStatisticsLayerId;
            }

            set
            {
                _wfsGridStatisticsLayerId = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        /// <summary>
        /// Indicates whether this instance has values set or not (ie. TaxonIds)
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has values; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettings
        {
            get
            {
                //return false;
                return CoordinateSystemId.HasValue && GridSize.HasValue;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridStatisticsSetting"/> class.
        /// </summary>
        public GridStatisticsSetting()
        {                        
            ResetSettings();
            IsActive = true;            
        }
        
        public bool HasActiveSettings
        {
            get
            {
                return IsActive && HasSettings;
            }        
        }

        public bool IsCoordinateSystemSettingsDefault()
        {
            if (!CoordinateSystemId.HasValue)
            {
                return false;
            }

            return CoordinateSystemId.Value == (int)GridCoordinateSystem.SWEREF99_TM;
        }

        public override bool IsSettingsDefault()
        {
            if (CalculateNumberOfObservations == true
                && CalculateNumberOfTaxa == true
                && GridSize == 10000
                && CoordinateSystemId == (int)GridCoordinateSystem.SWEREF99_TM
                && WfsGridStatisticsCalculationModeId == (int)WfsGridStatisticsCalculationMode.Count
                && WfsGridStatisticsLayerId == null
                && GenerateAllGridCells == false)
            {
                return true;
            }
            return false;
        }

        public void ResetSettings()
        {
            CalculateNumberOfObservations = true;
            CalculateNumberOfTaxa = true;
            GridSize = 10000;            
            CoordinateSystemId = (int)GridCoordinateSystem.SWEREF99_TM;
            WfsGridStatisticsCalculationModeId = (int)WfsGridStatisticsCalculationMode.Count;
            WfsGridStatisticsLayerId = null;
            GenerateAllGridCells = false;
        }
    }
}

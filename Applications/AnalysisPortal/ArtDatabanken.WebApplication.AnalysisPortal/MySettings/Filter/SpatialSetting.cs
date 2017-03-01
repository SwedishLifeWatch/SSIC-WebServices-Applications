using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataModels;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter
{
    /// <summary>
    /// This class stores filtered taxa settings.
    /// </summary>
    [DataContract]
    public sealed class SpatialSetting : SettingBase
    {
        private bool _isActive;
        private ObservableCollection<DataPolygon> _polygons;
        private ObservableCollection<int> _regionIds;
        private CoordinateSystem _polygonsCoordinateSystem;

        /// <summary>
        /// Gets or sets whether TaxaSetting is active or not.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is checked; otherwise, <c>false</c>.
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
                if (value.Equals(_isActive))
                {
                    return;
                }

                _isActive = value;                
                ResultCacheNeedsRefresh = true;                                    
            }
        }

        /// <summary>
        /// Gets or sets the locality settings.
        /// </summary>
        /// <value>
        /// The locality settings.
        /// </value>
        [DataMember]
        public LocalitySetting Locality { get; set; }

        /// <summary>
        /// List with polygons.
        /// </summary>
        [DataMember]
        public ObservableCollection<DataPolygon> Polygons
        {
            get
            {
                if (_polygons.IsNull())
                {
                    _polygons = new ObservableCollection<DataPolygon>();
                    _polygons.CollectionChanged += (sender, args) => { ResultCacheNeedsRefresh = true; };
                }

                return _polygons;
            }

            set
            {                
                _polygons = value;                
                ResultCacheNeedsRefresh = true;
                if (_polygons.IsNotNull())
                {
                    _polygons.CollectionChanged += (sender, args) => { ResultCacheNeedsRefresh = true; };
                }
            }
        }

        /// <summary>
        /// Gets the coordinate system that the polygons are defined in.
        /// </summary>        
        public CoordinateSystem PolygonsCoordinateSystem
        {
            get
            {
                if (_polygonsCoordinateSystem == null)
                {
                    _polygonsCoordinateSystem = new CoordinateSystem(CoordinateSystemId.GoogleMercator);
                }

                return _polygonsCoordinateSystem;
            }
        }

        /// <summary>
        /// List with region ids.
        /// </summary>
        [DataMember]
        public ObservableCollection<int> RegionIds
        {
            get
            {
                if (_regionIds.IsNull())
                {
                    _regionIds = new ObservableCollection<int>();
                    _regionIds.CollectionChanged += (sender, args) => { ResultCacheNeedsRefresh = true; };
                }

                return _regionIds;
            }

            set
            {                
                _regionIds = value;                
                ResultCacheNeedsRefresh = true;

                if (_regionIds.IsNotNull())
                {
                    _regionIds.CollectionChanged += (sender, args) => { ResultCacheNeedsRefresh = true; };
                }
            }
        }

        /// <summary>
        /// Indicates whether this instance has values set or not (i.e. TaxonIds).
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has values; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettings
        {
            get
            {
                if (!Polygons.IsEmpty())
                {
                    return true;
                }

                if (!RegionIds.IsEmpty())
                {
                    return true;
                }

                return false;
            }
        }
     
        /// <summary>
        /// Sets the polygons to a new list.
        /// </summary>
        /// <param name="polygons">
        /// The new polygons.
        /// </param>
        public void SetPolygons(List<DataPolygon> polygons)
        {
            Polygons = new ObservableCollection<DataPolygon>(polygons);
            IsActive = true;
        }

        /// <summary>
        /// Adds region ids to the RegionIds list.
        /// </summary>
        /// <param name="regionIds">The region ids to add.</param>
        public void AddRegions(IEnumerable<int> regionIds)
        {
            int countBefore = RegionIds.Count;
            if (regionIds.IsNull())
            {
                return;
            }

            var newList = new List<int>(RegionIds);
            newList.AddRange(regionIds);
            IEnumerable<int> distinctList = newList.Distinct();
            var distinctCollection = new ObservableCollection<int>();
            foreach (int id in distinctList)
            {
                distinctCollection.Add(id);
            }

            RegionIds = distinctCollection;
            int countAfter = RegionIds.Count;
            if (countAfter > countBefore)
            {
                IsActive = true;
            }
        }

        /// <summary>
        /// Adds a region id to the RegionIds list.
        /// </summary>
        /// <param name="id">The id.</param>
        public void AddRegion(int id)
        {
            if (!RegionIds.Contains(id))
            {
                RegionIds.Add(id);
                IsActive = true;
            }
        }

        /// <summary>
        /// Removes a region id from the RegionIds list.
        /// </summary>
        /// <param name="id">The id.</param>
        public void RemoveRegion(int id)
        {
            RegionIds.Remove(id);
        }

        /// <summary>
        /// Removes region ids from the RegionIds list.
        /// </summary>
        /// <param name="ids">The ids.</param>
        public void RemoveRegions(IEnumerable<int> ids)
        {
            foreach (int id in ids)
            {
                RegionIds.Remove(id);
            }
        }

        /// <summary>
        /// Resets the regions. Clears the RegionIds list.
        /// </summary>
        public void ResetRegions()
        {
            RegionIds.Clear();
        }

        /// <summary>
        /// Resets the locality settings.
        /// </summary>
        public void ResetLocality()
        {
            Locality.ResetSettings();
        }

        /// <summary>
        /// Determines whether locality settings is default.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if locality settings is default; otherwise, <c>false</c>.
        /// </returns>
        public bool IsLocalitySettingsDefault()
        {
            return Locality.IsSettingsDefault();
        }

        /// <summary>
        /// Determines whether common regions settings is default.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if common regions settings is default; otherwise, <c>false</c>.
        /// </returns>
        public bool IsCommonRegionsSettingsDefault()
        {
            return RegionIds.Count == 0;
        }

        /// <summary>
        /// Determines whether polygon settings is default.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if polygon settings is default; otherwise, <c>false</c>.
        /// </returns>
        public bool IsPolygonSettingsDefault()
        {
            return Polygons.Count == 0;
        }

        /// <summary>
        /// Determines whether the settings is the default settings.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the settings is default settings; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsSettingsDefault()
        {
            return IsCommonRegionsSettingsDefault()
                && IsPolygonSettingsDefault() && IsLocalitySettingsDefault();
        }

        /// <summary>
        /// Resets the settings.
        /// </summary>
        public void ResetSettings()
        {
            Polygons = new ObservableCollection<DataPolygon>();
            RegionIds = new ObservableCollection<int>();
            Locality = new LocalitySetting();
        }
    }
}
using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Labels;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Spatial
{
    /// <summary>
    /// This class is the view model for the Filter/SpatialCommonRegions action
    /// </summary>
    public class FilterSpatialViewModel
    {
        public Dictionary<int, string> RegionCategoriesDictionary { get; set; }
        
        /// <summary>
        /// Kommun
        /// </summary>
        public List<RegionViewModel> MunicipalityList { get; set; }

        /// <summary>
        /// Landskap
        /// </summary>
        public List<RegionViewModel> ProvinceList { get; set; }

        /// <summary>
        /// Län
        /// </summary>
        public List<RegionViewModel> CountyList { get; set; }

        public SharedRegionLabels RegionLabels
        {
            get { return SharedRegionLabels.Instance; }
        }

        /// <summary>
        /// Gets the model labels.
        /// </summary>
        public ModelLabels Labels
        {
            get
            {
                if (_labels == null)
                {
                    _labels = new ModelLabels();
                }

                return _labels;
            }
        }
        private ModelLabels _labels;

        /// <summary>
        /// Localized labels
        /// </summary>
        public class ModelLabels
        {
            //Spatial filter summary
            public string TitleLabel { get { return Resources.Resource.FilterSpatialTitle; } }            
        }

        public SharedLabels SharedLabels
        {
            get { return SharedLabels.Instance; }
        }

        public bool IsSettingsDefault { get; set; }

        //public List<RegionViewModel> HavList { get; set; }
        //public List<RegionViewModel> LandsdelList { get; set; }
        //public List<RegionViewModel> NaturtypList { get; set; }
        //public List<RegionViewModel> RamsarList { get; set; }
        //public List<RegionViewModel> RrkList { get; set; }        
        //public List<RegionViewModel> SpaList { get; set; }

        ///// <summary>
        ///// Församling
        ///// </summary>
        //public List<RegionViewModel> ParishList { get; set; }
        //public List<RegionViewModel> SockenList { get; set; }
    }
}

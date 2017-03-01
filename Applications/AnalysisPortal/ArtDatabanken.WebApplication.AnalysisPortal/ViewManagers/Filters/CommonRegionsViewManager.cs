using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Spatial;
using AnalysisPortalEnums = ArtDatabanken.WebApplication.AnalysisPortal.Enums;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Filters
{
    /// <summary>
    /// This class is used to sort regions by name
    /// </summary>
    class RegionTypeNameComparer : System.Collections.IComparer
    {        
        public int Compare(object obj1, object obj2)
        {
            IRegionCategory a = (IRegionCategory)obj1;
            IRegionCategory b = (IRegionCategory)obj2;
            return String.Compare(a.Name, b.Name, StringComparison.Ordinal);
        }
    }

    /// <summary>
    /// This class is a view manager for Common regions actions
    /// </summary>
    public class CommonRegionsViewManager : ViewManagerBase
    {
        public SpatialSetting SpatialSetting
        {
            get { return MySettings.Filter.Spatial; }
        }

        public CommonRegionsViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        /// <summary>
        /// Creates a CommonRegionsViewModel.
        /// </summary>
        /// <returns></returns>
        public CommonRegionsViewModel CreateCommonRegionsViewModel()
        {
            var model = new CommonRegionsViewModel();
            model.IsSettingsDefault = SpatialSetting.IsCommonRegionsSettingsDefault();
            //var categories = 
            RegionCategoryList regionCategories = GetRegionCategories(new List<AnalysisPortalEnums.RegionCategoryId>
                {
                    AnalysisPortalEnums.RegionCategoryId.Landsdel,
                    AnalysisPortalEnums.RegionCategoryId.Province,
                    AnalysisPortalEnums.RegionCategoryId.Municipality,
                    AnalysisPortalEnums.RegionCategoryId.County
                    //APortalEnums.RegionCategoryId.Hav,
                    //APortalEnums.RegionCategoryId.Landsdel,                    
                    //APortalEnums.RegionCategoryId.Naturtyp,                    
                    //APortalEnums.RegionCategoryId.Ramsar,
                    //APortalEnums.RegionCategoryId.Rrk,
                    //APortalEnums.RegionCategoryId.Spa
                });
            model.RegionCategoriesDictionary = CreateCategoryDictionary(regionCategories);
            Dictionary<int, List<IRegion>> dicRegions = GetRegionsDictionary(regionCategories);
            model.MunicipalityList = CreateRegionList(dicRegions, (int)AnalysisPortalEnums.RegionCategoryId.Municipality);             
            model.CountyList = CreateRegionList(dicRegions, (int)AnalysisPortalEnums.RegionCategoryId.County);
            model.ProvinceList = CreateRegionList(dicRegions, (int)AnalysisPortalEnums.RegionCategoryId.Province);
            //model.HavList = CreateRegionList(dicRegions, (int)APortalEnums.RegionCategoryId.Hav);
            model.ProvinceGroupList = CreateRegionList(dicRegions, (int)AnalysisPortalEnums.RegionCategoryId.Landsdel);
            //model.NaturtypList = CreateRegionList(dicRegions, (int)APortalEnums.RegionCategoryId.Naturtyp);
            //model.RamsarList = CreateRegionList(dicRegions, (int)APortalEnums.RegionCategoryId.Ramsar);
            //model.RrkList = CreateRegionList(dicRegions, (int)APortalEnums.RegionCategoryId.Rrk);            
            //model.SpaList = CreateRegionList(dicRegions, (int)APortalEnums.RegionCategoryId.Spa);
            return model;
        }

        /// <summary>
        /// Creates a dictionary where 
        /// key = region category id
        /// value = region category name
        /// </summary>
        /// <param name="regionCategories">The region categories.</param>
        /// <returns></returns>
        private Dictionary<int, string> CreateCategoryDictionary(RegionCategoryList regionCategories)
        {
            var dic = new Dictionary<int, string>();
            foreach (IRegionCategory regionCategory in regionCategories)
            {
                dic.Add(regionCategory.Id, regionCategory.Name);
            }
            return dic;
        }

        /// <summary>
        /// Retrieves region categories from the GeoReference service.
        /// </summary>
        /// <param name="categories">The categories to retrieve.</param>
        /// <returns></returns>
        private RegionCategoryList GetRegionCategories(IEnumerable<AnalysisPortalEnums.RegionCategoryId> categories)
        {            
            Int32 defaultCountryIsoCode = Resources.AppSettings.Default.SwedenCountryIsoCode;            
            RegionCategoryList regionCategories = CoreData.RegionManager.GetRegionCategories(UserContext, defaultCountryIsoCode);
            var selectedCategories = new RegionCategoryList();
            foreach (AnalysisPortalEnums.RegionCategoryId regionCategoryId in categories)
            {
                IRegionCategory category = regionCategories.Get((int)regionCategoryId);
                selectedCategories.Add(category);
            }
            return selectedCategories;
        }

        /// <summary>
        /// Gets all regions of a specific category type and returns it as an entry in a dictionary.
        /// </summary>
        /// <param name="regionCategories">The region categories.</param>
        /// <returns></returns>
        private Dictionary<int, List<IRegion>> GetRegionsDictionary(RegionCategoryList regionCategories)
        {
            RegionList regions = CoreData.RegionManager.GetRegionsByCategories(UserContext, regionCategories);
            var dicRegions = new Dictionary<int, List<IRegion>>();
            foreach (IRegion region in regions)
            {
                if (!dicRegions.ContainsKey(region.CategoryId))
                {
                    dicRegions.Add(region.CategoryId, new List<IRegion>());
                }

                dicRegions[region.CategoryId].Add(region);
            }
            return dicRegions;
        }

        /// <summary>
        /// Creates a RegionViewModel list for a specific categoryId
        /// </summary>
        /// <param name="dicRegions">The dic regions.</param>
        /// <param name="categoryId">The category id.</param>
        /// <returns></returns>
        private List<RegionViewModel> CreateRegionList(Dictionary<int, List<IRegion>> dicRegions, int categoryId)
        {            
            var list = new List<RegionViewModel>();
            foreach (IRegion region in dicRegions[categoryId])
            {
                list.Add(RegionViewModel.CreateFromRegion(region));
            }
            return list;
        }

        /// <summary>
        /// Returns all Region categories.
        /// </summary>
        /// <returns></returns>
        private RegionCategoryList GetAllRegionCategories()
        {
            var categories = new List<AnalysisPortalEnums.RegionCategoryId>
                {
                    AnalysisPortalEnums.RegionCategoryId.County,
                    AnalysisPortalEnums.RegionCategoryId.Hav,
                    AnalysisPortalEnums.RegionCategoryId.Landsdel,
                    AnalysisPortalEnums.RegionCategoryId.Municipality,
                    AnalysisPortalEnums.RegionCategoryId.Naturtyp,
                    AnalysisPortalEnums.RegionCategoryId.Parish,
                    AnalysisPortalEnums.RegionCategoryId.Province,
                    AnalysisPortalEnums.RegionCategoryId.Ramsar,
                    AnalysisPortalEnums.RegionCategoryId.Rrk,
                    AnalysisPortalEnums.RegionCategoryId.Socken,
                    AnalysisPortalEnums.RegionCategoryId.Spa
                };
            return GetRegionCategories(categories);
        }
    }
}

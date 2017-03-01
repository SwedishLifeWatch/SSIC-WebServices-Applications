using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Spatial
{
    /// <summary>
    /// This class is a view model for a Region
    /// </summary>
    public class RegionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GUID { get; set; }
        public int CategoryId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionViewModel"/> class.
        /// </summary>
        public RegionViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionViewModel"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        public RegionViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionViewModel"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="guid">The GUID.</param>
        /// <param name="categoryId">The category id.</param>
        public RegionViewModel(int id, string name, string guid, int categoryId)
        {
            Id = id;
            Name = name;
            GUID = guid;
            CategoryId = categoryId;
        }

        /// <summary>
        /// Creates a RegionViewModel from a Region object.
        /// </summary>
        /// <param name="region">The region object.</param>
        /// <returns></returns>
        public static RegionViewModel CreateFromRegion(IRegion region)
        {                        
            return new RegionViewModel(region.Id, region.Name, region.GUID, region.CategoryId);            
        }
    }
}
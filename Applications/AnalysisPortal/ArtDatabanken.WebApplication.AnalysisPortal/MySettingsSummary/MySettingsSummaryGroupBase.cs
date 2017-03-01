using System.Collections.Generic;
using System.Linq;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary
{
    /// <summary>
    /// This class is an abstract base class for MySettings summary groups
    /// </summary>
    public abstract class MySettingsSummaryGroupBase
    {
        protected List<MySettingsSummaryItemBase> _items = new List<MySettingsSummaryItemBase>();

        /// <summary>
        /// Gets the group title.
        /// </summary>
        public abstract string Title { get; }

        public abstract string ImageUrl { get; }        

        public abstract string IconClass { get; }        

        /// <summary>
        /// Gets the group items.
        /// </summary>
        public List<MySettingsSummaryItemBase> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets all items that are active.
        /// </summary>
        /// <returns></returns>
        public List<MySettingsSummaryItemBase> GetActiveItems()
        {
            var list = Items.Where(item => item.HasSettings && item.IsActive).ToList();
            return list;
        }

        /// <summary>
        /// Gets all items that are inactive.
        /// </summary>
        /// <returns></returns>
        public List<MySettingsSummaryItemBase> GetInActiveItems()
        {
            var list = Items.Where(item => item.HasSettings && !item.IsActive).ToList();
            return list;
        }

        /// <summary>
        /// True if group should be hidden in my settings
        /// </summary>
        public bool HideInMySettings { get; set; }
    }
}

using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Improved
{
    /// <summary>
    /// This class is an abstract base class for MySettings summary groups
    /// </summary>
    public abstract class ImprovedMySettingsSummaryGroupBase
    {
        protected readonly IUserContext UserContext;
        protected readonly AnalysisPortal.MySettings.MySettings MySettings;

        protected List<ImprovedMySettingsSummaryItemBase> _items = new List<ImprovedMySettingsSummaryItemBase>();

        protected ImprovedMySettingsSummaryGroupBase(IUserContext userContext, MySettings.MySettings mySettings)
        {
            UserContext = userContext;
            MySettings = mySettings;
        }

        /// <summary>
        /// Gets the group title.
        /// </summary>
        public abstract string Title { get; }

        /// <summary>
        /// Gets the group items.
        /// </summary>
        public List<ImprovedMySettingsSummaryItemBase> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets all items that are active.
        /// </summary>
        /// <returns></returns>
        public List<ImprovedMySettingsSummaryItemBase> GetActiveItems()
        {
            var list = Items.Where(item => item.HasSettings && item.IsActive).ToList();
            return list;
        }

        /// <summary>
        /// Gets all items that are inactive.
        /// </summary>
        /// <returns></returns>
        public List<ImprovedMySettingsSummaryItemBase> GetInActiveItems()
        {
            var list = Items.Where(item => item.HasSettings && !item.IsActive).ToList();
            return list;
        }
    }
}

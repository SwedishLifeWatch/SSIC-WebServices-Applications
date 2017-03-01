using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Buttons;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews
{
    public abstract class ResultGroupBase : IButtonGroupInfo
    {
        protected List<ResultViewBase> _items = new List<ResultViewBase>();        

        /// <summary>
        /// Gets the group title.
        /// </summary>
        public abstract string Title { get; }

        public abstract ResultGroupType ResultGroupType { get; }

        /// <summary>
        /// Gets the group items.
        /// </summary>
        public List<ResultViewBase> Items
        {
            get { return _items; }
        }

        public bool IsExpanded { get; set; }

        /// <summary>
        /// Gets all items that are active.
        /// </summary>
        /// <returns></returns>
        public List<ResultViewBase> GetActiveItems()
        {
            var list = Items.Where(item => item.IsActive).ToList();
            return list;
        }
       
        public bool HasActiveItems()
        {
            return Items.Any(resultView => resultView.IsActive);
        }

        public abstract PageInfo OverviewPageInfo { get; }

        public IEnumerable<IButtonInfo> Buttons
        {
            get { return _items; }
            set { _items = value as List<ResultViewBase>; }
        }
        public abstract string OverviewButtonTooltip { get; }
    }
}

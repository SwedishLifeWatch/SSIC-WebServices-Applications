using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Buttons;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews
{
    public abstract class ResultViewBase : IButtonInfo
    {
        protected ResultViewBase()
        {
            IsEnabled = true;
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        public abstract string Title { get; }

        public abstract string Tooltip { get; }

        public virtual PageInfo DynamicPageInfo
        {
            get
            {
                return StaticPageInfo;
            }
        }
        public bool IsEnabled { get; set; }

        public bool IsCurrent
        {
            get
            {
                PageInfo pageInfo = SessionHandler.CurrentPage;
                return pageInfo == this.StaticPageInfo;                
            }
        }

        /// <summary>
        /// Gets the page info.
        /// </summary>
        public abstract PageInfo StaticPageInfo { get; }

        public abstract ResultType ResultType { get; }

        /// <summary>
        /// Gets a value indicating whether this result view is active.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsActive { get; }        

        /// <summary>
        /// Sub buttons if any
        /// </summary>
        public IEnumerable<IButtonInfo> SubButtons { get; set; }
    }
}

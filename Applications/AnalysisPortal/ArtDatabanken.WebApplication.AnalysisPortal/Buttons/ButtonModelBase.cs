using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons
{
    /// <summary>
    /// This class is an abstract base class for button models
    /// </summary>
    public abstract class ButtonModelBase : IButtonInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonModelBase"/> class.
        /// </summary>
        protected ButtonModelBase()
        {
            IsActive = true;
            IsEnabled = true;
        }

        /// <summary>
        /// Gets the page info.
        /// </summary>
        public abstract PageInfo StaticPageInfo { get; }

        public virtual PageInfo DynamicPageInfo
        {
            get
            {
                return StaticPageInfo;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this button is enabled.
        /// </summary>        
        public bool IsEnabled { get; set; }

        public bool IsActive { get; private set; }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public abstract string Title { get; }

        /// <summary>
        /// Gets the button tooltip.
        /// </summary>
        public abstract string Tooltip { get; }
        
        /// <summary>
        /// Indicates if the button is the current selected
        /// </summary>                
        public virtual bool IsCurrent
        {
            get
            {
                PageInfo pageInfo = SessionHandler.CurrentPage;
                return pageInfo == this.StaticPageInfo;
            }
        }
    }
}

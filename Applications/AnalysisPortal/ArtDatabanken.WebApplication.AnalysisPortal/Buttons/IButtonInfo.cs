using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons
{
    public interface IButtonInfo
    {
        /// <summary>
        /// Gets the page info.
        /// </summary>
        PageInfo StaticPageInfo { get; }

        /// <summary>
        /// Gets the page info. This varies depending on mysettings.
        /// </summary>        
        PageInfo DynamicPageInfo { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this button is enabled.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets a value indicating whether the view has active settings.
        /// </summary>        
        bool IsActive { get; }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the button tooltip.
        /// </summary>
        string Tooltip { get; }

        /// <summary>
        /// Gets or sets value indicating wich button that is active
        /// </summary>
        /// <value>
        ///     <c>true</c> if the user is on the matching page; otherwise, <c>false</c>.
        /// </value>
        bool IsCurrent { get; }
    }
}
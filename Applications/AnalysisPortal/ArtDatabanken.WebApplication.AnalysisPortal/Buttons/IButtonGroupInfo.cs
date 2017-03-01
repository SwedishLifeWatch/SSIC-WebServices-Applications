using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons
{
    public interface IButtonGroupInfo
    {
        PageInfo OverviewPageInfo { get; }

        /// <summary>
        /// Gets or sets the buttons.
        /// </summary>
        IEnumerable<IButtonInfo> Buttons { get; set; }

        string OverviewButtonTooltip { get; }
    }
}
using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.FilterSectionButtons
{
    /// <summary>
    /// This class acts as a view model for the Filter Fields state button.
    /// </summary>
    public class FilterTaxaRedListButtonModel : ButtonModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterTaxaRedListButtonModel"/> class.
        /// </summary>
        public FilterTaxaRedListButtonModel()
        {                        
            this.IsEnabled = true;
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get
            { return Resources.Resource.FilterRedListButton; 
            }
        }

        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Filter", "RedList"); }
        }

        /// <summary>
        /// Gets the button tooltip.
        /// </summary>
        public override string Tooltip
        {
            get { return Resources.Resource.FilterRedListButtonTooltip; }
        }        
    }
}

using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.FilterSectionButtons
{
    /// <summary>
    /// This class acts as a view model for the Filter Fields state button.
    /// </summary>
    public class FilterTaxaTaxonByTaxonAttributesButton : ButtonModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterTaxaTaxonByTaxonAttributesButton"/> class.
        /// </summary>
        public FilterTaxaTaxonByTaxonAttributesButton()
        {                        
            this.IsEnabled = true;
        }        

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get
            {
                return Resources.Resource.FilterTaxonByTaxonAttributes; 
            }
        }

        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Filter", "TaxonByTaxonAttributes"); }
        }

        /// <summary>
        /// Gets the button tooltip.
        /// </summary>
        public override string Tooltip
        {
            get { return Resources.Resource.FilterTaxonByTaxonAttributes; }
        }    
    }
}

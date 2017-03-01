using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.FilterSectionButtons
{
    /// <summary>
    /// This class acts as a view model for the Filter Taxa state button
    /// </summary>
    public class FilterTaxaButtonModel : StateButtonModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterTaxaButtonModel"/> class.
        /// </summary>
        public FilterTaxaButtonModel()
        {                        
            this.IsEnabled = true;
        }

        /// <summary>
        /// Gets the button identifier.
        /// This is used to identify a button, for example when we want to know which button was pressed.
        /// </summary>
        public override StateButtonIdentifier Identifier
        {
            get { return StateButtonIdentifier.FilterTaxa; }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.StateButtonFilterTaxa; }
        }
        
        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Filter", "Taxa"); }
        }

        public override PageInfo DynamicPageInfo
        {
            get
            {
                if (SessionHandler.MySettings.Filter.Taxa.HasSettings)
                {
                    return PageInfoManager.GetPageInfo("Filter", "Taxa");
                }

                return PageInfoManager.GetPageInfo("Filter", "TaxonFromSearch");
            }
        }

        public override string Tooltip
        {
            get { return Resources.Resource.StateButtonFilterTaxaTooltip; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this button is checked.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is checked; otherwise, <c>false</c>.
        /// </value>
        public override bool IsChecked
        {
            get
            {                
                return SessionHandler.MySettings.Filter.Taxa.IsActive;
            }
            set
            {
                SessionHandler.MySettings.Filter.Taxa.IsActive = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user has made any settings in the Action page
        /// </summary>
        /// <value>
        ///     <c>true</c> if the user hade made changes; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettings
        {
            get { return SessionHandler.MySettings.Filter.Taxa.HasSettings; }
        }

        public override bool IsSettingsDefault
        {
            get { return SessionHandler.MySettings.Filter.Taxa.IsSettingsDefault(); }
        }        

        // --------------- SubMenu ----------------------------

        protected List<ButtonModelBase> _buttons = new List<ButtonModelBase>();

        public override List<ButtonModelBase> Children
        {
            get
            {
                if (_buttons.IsEmpty())
                {
                    _buttons.Add(new FilterTaxaTaxonFromSearchButtonModel());
                    _buttons.Add(new FilterTaxaTaxonFromIdsButtonModel());
                    _buttons.Add(new FilterTaxaTaxonByTaxonAttributesButton());
                    _buttons.Add(new FilterTaxaRedListButtonModel());
                }

                return _buttons;
            }            
        }        
    }
}

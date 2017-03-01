using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.ButtonGroups
{
    /// <summary>
    /// This class acts as a view model for the Filter button group
    /// </summary>
    public class FilterButtonGroupModel : ButtonGroupModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterButtonGroupModel"/> class.
        /// </summary>
        public FilterButtonGroupModel()
        {
            Buttons.Add(StateButtonManager.FilterOccurrenceButton);
            Buttons.Add(StateButtonManager.FilterTaxaButton);
            Buttons.Add(StateButtonManager.FilterSpatialButton);
            Buttons.Add(StateButtonManager.FilterTemporalButton);
            Buttons.Add(StateButtonManager.FilterAccuracyButton);
            Buttons.Add(StateButtonManager.FilterFieldsButton);            
        }

        public override PageInfo MainPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Filter", "Index"); }
        }

        public override string ImageUrl
        {
            get { return "~/Content/images/FilterSummaryGroup.png"; }
        }

        public override string IconClass
        {
            get { return "icon-filter"; }
        }

        /// <summary>
        /// Gets the button group title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.ButtonGroupFilter; }            
        }

        /// <summary>
        /// Gets the button group identifier.
        /// </summary>
        public override ButtonGroupIdentifier Identifier
        {
            get { return ButtonGroupIdentifier.Filter; }
        }
    }
}

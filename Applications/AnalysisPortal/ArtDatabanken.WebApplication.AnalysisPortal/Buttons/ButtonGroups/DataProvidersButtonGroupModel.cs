using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.ButtonGroups
{
    /// <summary>
    /// This class acts as a view model for the DataProviderss button group
    /// </summary>
    public class DataProvidersButtonGroupModel : ButtonGroupModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataProvidersButtonGroupModel"/> class.
        /// </summary>
        public DataProvidersButtonGroupModel()
        {            
            Buttons.Add(StateButtonManager.DataProvidersSpeciesObservationButton);
            Buttons.Add(StateButtonManager.DataProvidersWfsLayersButton);
            Buttons.Add(StateButtonManager.DataProvidersWmsLayersButton);
            Buttons.Add(StateButtonManager.DataProvidersMetadataSearchButton);
        }

        /// <summary>
        /// Gets the button group title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.ButtonGroupDataProviders; }            
        }

        public override PageInfo MainPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Data", "Index"); }
        }

        public override string ImageUrl
        {
            get { return "~/Content/Images/DataProvidersSummaryGroup.png"; }
        }
        public override string IconClass
        {
            get { return "icon-sitemap"; }
        }

        /// <summary>
        /// Gets the button group identifier.
        /// </summary>
        public override ButtonGroupIdentifier Identifier
        {
            get { return ButtonGroupIdentifier.DataProviders; }
        }
    }
}

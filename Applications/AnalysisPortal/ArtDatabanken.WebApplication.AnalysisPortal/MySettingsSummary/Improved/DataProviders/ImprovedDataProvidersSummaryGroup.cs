using System.Collections.Generic;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Improved.DataProviders
{
    /// <summary>
    /// This class is the MySettings summary group for all Data sources settings
    /// </summary>
    public class ImprovedDataProvidersSummaryGroup : ImprovedMySettingsSummaryGroupBase
    {
        public ImprovedDataProvidersSummaryGroup(IUserContext userContext, MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        public List<ImprovedMySettingsSummaryItemBase> GetItems()
        {
            List<ImprovedMySettingsSummaryItemBase> items = new List<ImprovedMySettingsSummaryItemBase>();
            items.Add(new ImprovedDataProvidersSettingSummary(UserContext, MySettings));
            return items;
        }

        /// <summary>
        /// Gets the group title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.HeadMenuDataProviders; }
        }

        /// <summary>
        /// Gets the data providers summary setting model.
        /// </summary>
        ////public ImprovedMySettingsSummaryGroupBase DataProviders
        ////{
        ////    get { return MySettingsSummaryItemManager.DataProvidersDataProvidersSettingsSummary; }
        ////}

        /////// <summary>
        /////// Gets the WFS layers summary setting model.
        /////// </summary>        
        ////public WfsLayersSettingSummary WfsLayers
        ////{
        ////    get { return MySettingsSummaryItemManager.DataProvidersWfsLayersSettingsSummary; }
        ////}

        /////// <summary>
        /////// Gets the map layers summary setting model.
        /////// </summary>
        ////public MapLayersSettingSummary MapLayers
        ////{
        ////    get { return (MapLayersSettingSummary)MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.DataMapLayers); }
        ////}
        
        /////// <summary>
        /////// Initializes a new instance of the <see cref="ImprovedDataProvidersSummaryGroup"/> class.
        /////// </summary>
        ////public ImprovedDataProvidersSummaryGroup()
        ////{
        ////    Items.Add(MySettingsSummaryItemManager.DataProvidersDataProvidersSettingsSummary);
        ////    Items.Add(MySettingsSummaryItemManager.DataProvidersWfsLayersSettingsSummary);
        ////    Items.Add(MySettingsSummaryItemManager.DataProvidersMapLayersSettingsSummary);
        ////}
    }
}

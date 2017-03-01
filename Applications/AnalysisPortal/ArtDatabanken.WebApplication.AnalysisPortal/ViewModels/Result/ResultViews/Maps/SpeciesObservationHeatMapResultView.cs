using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Maps
{
    /// <summary>
    /// This class represents the view showing the species observation heat map.
    /// </summary>
    public class SpeciesObservationHeatMapResultView : ResultViewBase
    {
        /// <summary>
        /// The title of the heat map.
        /// </summary>
        public override string Title
        {
            get
            {
                return Resources.Resource.ResultSpeciesObservationHeatMap;
            }
        }

        /// <summary>
        /// The tooltip for the view.
        /// </summary>
        public override string Tooltip
        {
            get { return ""; }
        }

        /// <summary>
        /// The information about the page.
        /// </summary>
        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Result", "SpeciesObservationHeatMap"); }
        }

        /// <summary>
        /// The type of result that is calculated.
        /// </summary>
        public override ResultType ResultType
        {
            get { return ResultType.SpeciesObservationHeatMap; }
        }

        /// <summary>
        /// Returns true if view is active.
        /// </summary>
        public override bool IsActive
        {
            get { return true; }
        }        
    }
}

using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.General;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Result
{
    /// <summary>
    /// View manager for SpeciesObservationResult.
    /// </summary>
    public class SpeciesObservationResultViewManager : ViewManagerBase
    {
        /// <summary>
        /// Actual settings.
        /// </summary>
        private AnalysisPortal.MySettings.MySettings settings = null;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="mySettings">The settings.</param>
        public SpeciesObservationResultViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
            settings = mySettings;
        }        

        /// <summary>
        /// Creates the view model for SpeciesObservationMap.
        /// </summary>
        /// <returns>A view model for SpeciesObservationMap.</returns>
        public ResultSpeciesObservationMapViewModel CreateResultSpeciesObservationMapViewModel()
        {
            ResultSpeciesObservationMapViewModel model = new ResultSpeciesObservationMapViewModel();
            PagedSpeciesObservationResultCalculator resultCalculator = new PagedSpeciesObservationResultCalculator(UserContext, MySettings);
            model.ComplexityEstimate = resultCalculator.GetQueryComplexityEstimate();            
            
            if (MySettings.Filter.Taxa.TaxonIds.Count > 1)
            {
                TaxonTreeViewManager taxonTreeViewManager = new TaxonTreeViewManager(UserContext, MySettings);
                model.CategoryTaxaList = taxonTreeViewManager.GetCategoryTaxaList(MySettings.Filter.Taxa.TaxonIds.ToList());
            }
            else if (MySettings.Filter.Taxa.TaxonIds.Count == 0)
            {
                model.SelectedTaxaDescription = Resource.MySettingsAllTaxaSelected;
            }
            else if (MySettings.Filter.Taxa.TaxonIds.Count == 1)
            {
                // TaxonList taxa = CoreData.TaxonManager.GetTaxa(UserContext, MySettings.Filter.Taxa.TaxonIds.ToList());
                ITaxon taxon = CoreData.TaxonManager.GetTaxon(UserContext, MySettings.Filter.Taxa.TaxonIds.First());
                TaxonViewModel taxonViewModel = TaxonViewModel.CreateFromTaxon(taxon);
                model.SelectedTaxaDescription = taxonViewModel.FullName;

                // List<TaxonViewModel> taxonList = taxa.GetGenericList().ToTaxonViewModelList();

                // List<string> strTaxa = taxonList.Select(taxon => taxon.FullName).ToList();

                // model.SelectedTaxaDescription = String.Join(", ", strTaxa).Substring(0, 100) + "...";
            }

            model.AddSpartialFilterLayer = MySettings.Filter.Spatial.HasSettings && MySettings.Filter.Spatial.IsActive;
            return model;
        }

        /// <summary>
        /// Creates the view model for TimeSeriesOnSpeciesObservationCounts.
        /// </summary>
        /// <returns>A view model for TimeSeriesOnSpeciesObservationCounts.</returns>
        public ResultTimeSeriesOnSpeciesObservationCountsViewModel CreateResultTimeSeriesOnSpeciesObservationCountsViewModel()
        {
            var model = new ResultTimeSeriesOnSpeciesObservationCountsViewModel();
            SpeciesObservationDiagramResultCalculator resultCalculator = new SpeciesObservationDiagramResultCalculator(UserContext, MySettings);
            model.ComplexityEstimate = resultCalculator.GetQueryComplexityEstimate();

            // model.ComplexityEstimate = QueryComplexityManager.GetQueryComplexityEstimate(ResultType.SpeciesObservationDiagram, UserContext, MySettings);
            model.NoOfTaxa = settings.Filter.Taxa.NumberOfSelectedTaxa;
            return model;
        }

        /// <summary>
        /// Creates the view model for TimeSeriesDiagramOnSpeciesObservationAbundanceIndex.
        /// </summary>
        /// <returns>A view model for TimeSeriesDiagramOnSpeciesObservationAbundanceIndex.</returns>
        public ResultTimeSeriesOnSpeciesObservationCountsViewModel CreateResultTimeSeriesDiagramOnSpeciesObservationAbundanceIndexViewModel()
        {
            var model = new ResultTimeSeriesOnSpeciesObservationCountsViewModel();
            SpeciesObservationAbundanceIndexDiagramResultCalculator resultCalculator = new SpeciesObservationAbundanceIndexDiagramResultCalculator(UserContext, MySettings);
            model.ComplexityEstimate = resultCalculator.GetQueryComplexityEstimate();
            model.NoOfTaxa = settings.Filter.Taxa.NumberOfSelectedTaxa;
            TaxonList taxonList = CoreData.TaxonManager.GetTaxa(UserContext, MySettings.Filter.Taxa.TaxonIds.ToList());
            model.SelectedTaxa = taxonList.GetGenericList().ToTaxonViewModelList();
            return model;
        }
    }
}

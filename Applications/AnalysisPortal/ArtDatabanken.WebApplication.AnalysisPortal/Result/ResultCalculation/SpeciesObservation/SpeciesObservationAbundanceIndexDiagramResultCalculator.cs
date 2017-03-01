using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultModels;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation
{
    /// <summary>
    /// Calculates the abundance index over time of species.
    /// </summary>
    public class SpeciesObservationAbundanceIndexDiagramResultCalculator : ResultCalculatorBase
    {   
        /// <summary>
        /// Calls the base class constructor.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="mySettings">The session settings.</param>
        public SpeciesObservationAbundanceIndexDiagramResultCalculator(IUserContext userContext, MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        /// <summary>
        /// Calculates abundance index for all selected taxa.
        /// </summary>
        /// <param name="periodicityId">The periodicity.</param>
        /// <returns>Dictionary where the keys are info about the taxon, and the value are the abundance index for the taxon.</returns>
        public Dictionary<TaxonViewModel, List<KeyValuePair<string, string>>> GetAbundanceIndexDiagramResults(int periodicityId)
        {            
            if (MySettings.Filter.Taxa.TaxonIds.Count == 0)
            {
                return null; // todo  - throw Exception?
            }

            TaxonList taxa = CoreData.TaxonManager.GetTaxa(UserContext, MySettings.Filter.Taxa.TaxonIds.ToList());
            List<TaxonViewModel> taxonList = taxa.GetGenericList().ToTaxonViewModelList();

            Dictionary<TaxonViewModel, List<KeyValuePair<string, string>>> dicResults = new Dictionary<TaxonViewModel, List<KeyValuePair<string, string>>>();
            foreach (TaxonViewModel taxonViewModel in taxonList)
            {
                List<KeyValuePair<string, string>> result = GetAbundanceIndexDiagramResult(periodicityId, taxonViewModel.TaxonId);
                dicResults.Add(taxonViewModel, result);
            }
            return dicResults;
        }

        /// <summary>
        /// Gets the abundance index data results.
        /// </summary>
        /// <param name="periodicityId">The periodicity id.</param>
        /// <returns></returns>
        public Dictionary<TaxonViewModel, List<AbundanceIndexData>> GetAbundanceIndexDataResults(int periodicityId)
        {
            if (MySettings.Filter.Taxa.TaxonIds.Count == 0)
            {
                return null; // todo  - throw Exception?
            }

            TaxonList taxa = CoreData.TaxonManager.GetTaxa(UserContext, MySettings.Filter.Taxa.TaxonIds.ToList());
            List<TaxonViewModel> taxonList = taxa.GetGenericList().ToTaxonViewModelList();

            Dictionary<TaxonViewModel, List<AbundanceIndexData>> dicResults = new Dictionary<TaxonViewModel, List<AbundanceIndexData>>();
            TimeStepSpeciesObservationCountList allSelectedSpeciesTimeSeriesData = GetSpeciesObservationCountFromWebService(UserContext, periodicityId, MySettings.Filter.Taxa.TaxonIds.ToList());
            foreach (TaxonViewModel taxonViewModel in taxonList)
            {
                List<AbundanceIndexData> abundanceIndexDatas = CalculateAbundanceIndex(periodicityId, taxonViewModel.TaxonId, allSelectedSpeciesTimeSeriesData);
                dicResults.Add(taxonViewModel, abundanceIndexDatas);
            }
            return dicResults;
        }

        /// <summary>
        /// Calculates the abundance index over a selected period of time for a specific taxon.
        /// </summary>
        /// <param name="periodicityId">The time period id.</param>
        /// <param name="taxonId">The taxon id.</param>
        /// <returns>A list of values representing the abundance index.</returns>
        public List<AbundanceIndexData> CalculateAbundanceIndex(int periodicityId, int taxonId)
        {
            TimeStepSpeciesObservationCountList allSelectedSpeciesTimeSeriesData = GetSpeciesObservationCountFromWebService(UserContext, periodicityId, MySettings.Filter.Taxa.TaxonIds.ToList());
            return CalculateAbundanceIndex(periodicityId, taxonId, allSelectedSpeciesTimeSeriesData);
        }

        /// <summary>
        /// Calculates the abundance index over a selected period of time for a specific taxon.
        /// </summary>
        /// <param name="periodicityId">The time period id.</param>
        /// <param name="taxonId">The taxon id.</param>
        /// <param name="allSelectedSpeciesTimeSeriesData">Calculated time series data for all selected taxa.</param>
        /// <returns>A list of values representing the abundance index.</returns>
        public List<AbundanceIndexData> CalculateAbundanceIndex(int periodicityId, int taxonId, TimeStepSpeciesObservationCountList allSelectedSpeciesTimeSeriesData)
        {        
            List<AbundanceIndexData> abundanceIndexList = new List<AbundanceIndexData>();
            TimeStepSpeciesObservationCountList singleSpeciesTimeSeriesData = GetSpeciesObservationCountFromWebService(UserContext, periodicityId, taxonId);
            //TimeStepSpeciesObservationCountList allSelectedSpeciesTimeSeriesData = GetSpeciesObservationCountFromWebService(UserContext, periodicityId, MySettings.Filter.Taxa.TaxonIds.ToList());
            long singleSpeciesTotalSum = singleSpeciesTimeSeriesData.Sum(t => t.ObservationCount);
            long allSelectedSpeciesTotalSum = allSelectedSpeciesTimeSeriesData.Sum(t => t.ObservationCount);
            double divisor = (double)singleSpeciesTotalSum / (allSelectedSpeciesTotalSum - singleSpeciesTotalSum);

            foreach (ITimeStepSpeciesObservationCount timeStepSpeciesObservationCount in allSelectedSpeciesTimeSeriesData)
            {
                double singleSpeciesPeriodObservationCount = 0.0;
                
                // Find the current timeStep value.
                foreach (TimeStepSpeciesObservationCount timeStep in singleSpeciesTimeSeriesData)
                {
                    if (timeStep.Name == timeStepSpeciesObservationCount.Name)
                    {
                        singleSpeciesPeriodObservationCount = timeStep.ObservationCount;
                        break;
                    }
                }

                double dividend = singleSpeciesPeriodObservationCount / (timeStepSpeciesObservationCount.ObservationCount - singleSpeciesPeriodObservationCount);
                double logArgument = dividend / divisor;
                double value = Math.Log10(logArgument);

                AbundanceIndexData abundanceIndexData = new AbundanceIndexData();
                abundanceIndexData.TaxonId = taxonId;
                abundanceIndexData.TimeStep = timeStepSpeciesObservationCount.Name;
                if (!double.IsNaN(value) && !double.IsInfinity(value))
                {
                    abundanceIndexData.AbundanceIndex = value;
                }
                abundanceIndexData.Count = (int)singleSpeciesPeriodObservationCount;
                abundanceIndexData.TotalCount = (int)timeStepSpeciesObservationCount.ObservationCount;
                abundanceIndexList.Add(abundanceIndexData);
            }
            return abundanceIndexList;            
        }

        /// <summary>
        /// Calculates the abundance index over a selected period of time for a specific taxon.
        /// </summary>
        /// <param name="periodicityId">The time period id.</param>
        /// <param name="taxonId">The taxon id.</param>
        /// <returns>A list of values representing the abundance index.</returns>
        public List<KeyValuePair<string, string>> GetAbundanceIndexDiagramResult(int periodicityId, int taxonId)
        {
            Dictionary<string, string> observationCountData = new Dictionary<string, string>();

            // Todo Update when temporal filter is connected to diagram result sprint 13
            if (true) // MySettings.Filter.Temporal.IsActive)
            {
                TimeStepSpeciesObservationCountList sumSpeciesPeriod = GetSpeciesObservationCountFromWebService(UserContext, periodicityId, taxonId);
                TimeStepSpeciesObservationCountList sumPeriod = GetSpeciesObservationCountFromWebService(UserContext, periodicityId, MySettings.Filter.Taxa.TaxonIds.ToList());
                long sumSpeciesTotal = sumSpeciesPeriod.Sum(t => t.ObservationCount);
                long sumTotal = sumPeriod.Sum(t => t.ObservationCount);
                double divisor = (double)sumSpeciesTotal / (sumTotal - sumSpeciesTotal);

                for (int i = 0; i < sumPeriod.Count; ++i)
                {
                    double sumSpeciesPeriodObservationCount = 0.0;

                    foreach (TimeStepSpeciesObservationCount timeStep in sumSpeciesPeriod)
                    {
                        if (timeStep.Name == sumPeriod[i].Name)
                        {
                            sumSpeciesPeriodObservationCount = timeStep.ObservationCount;
                            break;
                        }
                    }

                    double dividend = sumSpeciesPeriodObservationCount / (sumPeriod[i].ObservationCount - sumSpeciesPeriodObservationCount);
                    double logArgument = dividend / divisor;
                    double value = Math.Log10(logArgument);

                    observationCountData.Add(sumPeriod[i].Name, !double.IsNaN(value) && !double.IsInfinity(value) ? Convert.ToString(value) : string.Empty);
                }
            }

            List<KeyValuePair<string, string>> result = observationCountData.ToList();

            return result;
        }

        /// <summary>
        /// Get the species observation count from web service.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="periodicityId">The time period id.</param>
        /// <param name="taxonId">The taxon id.</param>
        /// <returns>No of observations that matches my settings.</returns>
        private TimeStepSpeciesObservationCountList GetSpeciesObservationCountFromWebService(IUserContext userContext, int periodicityId, int taxonId)
        {
            return GetSpeciesObservationCountFromWebService(userContext, periodicityId, new List<int> { taxonId });
        }

        /// <summary>
        /// Get the species observation count from web service.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="periodicityId">The time period id.</param>
        /// <param name="taxonIds">The taxon ids.</param>
        /// <returns>No of observations that matches my settings.</returns>
        private TimeStepSpeciesObservationCountList GetSpeciesObservationCountFromWebService(IUserContext userContext, int periodicityId, List<int> taxonIds)
        {
            var searchCriteriaManager = new SpeciesObservationSearchCriteriaManager(userContext);
            SpeciesObservationSearchCriteria searchCriteria = searchCriteriaManager.CreateSearchCriteria(MySettings);
            searchCriteria.TaxonIds = taxonIds;
            if (userContext.CurrentRole.IsNull() && userContext.CurrentRoles.Count > 0)
            {
                userContext.CurrentRole = userContext.CurrentRoles[0];
            }

            var displayCoordinateSystem = MySettings.Presentation.Map.DisplayCoordinateSystem;

            // TODO: undersök om detta kan tas bort. Fråga Agneta varför Id istället för de faktiska enum
            Periodicity timeStepType = Periodicity.Yearly;
            foreach (Periodicity value in Enum.GetValues(typeof(Periodicity)))
            {
                if ((int)value == periodicityId)
                {
                    timeStepType = value;
                    break;
                }
            }

            TimeStepSpeciesObservationCountList speciesObservationCount = CoreData.AnalysisManager.GetTimeSpeciesObservationCounts(userContext, searchCriteria, timeStepType, displayCoordinateSystem);

            return speciesObservationCount;
        }

        /// <summary>
        /// Gets the estimated complexity of the query being run.
        /// </summary>
        /// <returns>The estimated complexity of the query.</returns>
        public QueryComplexityEstimate GetQueryComplexityEstimate()
        {            
            var complexityEstimate = new QueryComplexityEstimate
            {
                QueryComplexityExecutionTime = QueryComplexityExecutionTime.Fast
            };

            if (MySettings.Filter.Taxa.TaxonIds.Count == 0)
            {
                complexityEstimate.QueryComplexityExecutionTime = QueryComplexityExecutionTime.Medium;
                complexityEstimate.ComplexityDescription.Text = string.Format(Resource.QueryComplexityAllTaxaSelected);
            }
            else if (MySettings.Filter.Taxa.TaxonIds.Count > 50)
            {
                complexityEstimate.QueryComplexityExecutionTime = QueryComplexityExecutionTime.Medium;
                complexityEstimate.ComplexityDescription.Text = string.Format(Resource.QueryComplexityManyTaxaIsUsedTemplate, MySettings.Filter.Taxa.TaxonIds.Count);
            }

            return complexityEstimate;
        }
    }
}
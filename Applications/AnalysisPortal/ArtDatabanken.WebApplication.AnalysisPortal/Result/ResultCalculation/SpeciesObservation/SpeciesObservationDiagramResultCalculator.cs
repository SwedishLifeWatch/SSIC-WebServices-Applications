using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Diagrams;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation
{
    public class SpeciesObservationDiagramResultCalculator : ResultCalculatorBase
    {
        public SpeciesObservationDiagramResultCalculator(IUserContext userContext, MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }
       
        /// <summary>
        /// Not used since option is required..
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> CalculateResult()
        {
            Dictionary<string, string> observationCountData = new Dictionary<string, string>();
            //Todo Update when temporal filter is connected to diagram result sprint 13
            if (true) //MySettings.Filter.Temporal.IsActive)
            {
                TimeStepSpeciesObservationCountList countList = GetSpeciesObservationCountFromWebService(UserContext, 0);
                foreach (ITimeStepSpeciesObservationCount timeStep in countList)
                {
                    observationCountData.Add(timeStep.Name, Convert.ToString(timeStep.ObservationCount));
                }                
            }
           
            List<KeyValuePair<string, string>> result = observationCountData.ToList();
            return result;
        }

        /// <summary>
        /// Get diagram result.
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetDiagramResult(int periodicityId)
        {
            Dictionary<string, string> observationCountData = new Dictionary<string, string>();
            //Todo Update when temporal filter is connected to diagram result sprint 13
            if (true) //MySettings.Filter.Temporal.IsActive)
            {
                TimeStepSpeciesObservationCountList count = GetSpeciesObservationCountFromWebService(UserContext, periodicityId);
                foreach (TimeStepSpeciesObservationCount timeStep in count)
                {
                    observationCountData.Add(timeStep.Name, Convert.ToString(timeStep.ObservationCount));
                    /*
                    string date = timeStep.Name;
                    if (periodicityId == (int)Periodicity.DayOfTheYear)
                    {
                        if (timeStep.Date != null)
                        {
                             date = ((DateTime)timeStep.Date).ToShortDateString();
                        }
                    }
                    if (periodicityId == (int)Periodicity.Monthly)
                    {
                        if (timeStep.Date != null)
                        {
                            date = ((DateTime)timeStep.Date).ToShortDateString();
                            if (SessionHandler.Language == "sv-SE")
                            {
                                date = date.Substring(0, 7);
                            }
                            else
                            {
                                date = date.Substring(3, 7);
                            }
                        }
                    }
                    

                    observationCountData.Add(date, Convert.ToString(timeStep.ObservationCount));
                     *  * */
                }
            }

            List<KeyValuePair<string, string>> result = observationCountData.ToList();
            return result;
        }

        /// <summary>
        /// Get the species observation count from web service
        /// </summary>
        /// <returns>No of observations that matches my settings.</returns>
        private TimeStepSpeciesObservationCountList GetSpeciesObservationCountFromWebService(IUserContext userContext, int periodicityId)
        {
            var searchCriteriaManager = new SpeciesObservationSearchCriteriaManager(userContext);
            SpeciesObservationSearchCriteria searchCriteria = searchCriteriaManager.CreateSearchCriteria(MySettings);
            if (userContext.CurrentRole.IsNull() && userContext.CurrentRoles.Count > 0)
            {
                userContext.CurrentRole = userContext.CurrentRoles[0];
            }

            var displayCoordinateSystem = MySettings.Presentation.Map.DisplayCoordinateSystem;

            //TODO: undersök om detta kan tas bort. Fråga Agneta varför Id istället för de faktiska enum
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

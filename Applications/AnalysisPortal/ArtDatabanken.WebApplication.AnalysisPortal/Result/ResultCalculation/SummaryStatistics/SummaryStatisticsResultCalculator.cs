using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SummaryStatistics
{
    public class SummaryStatisticsResultCalculator : ResultCalculatorBase
    {
        public SummaryStatisticsResultCalculator(IUserContext userContext, MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {            
        }        

        public List<KeyValuePair<string, string>> GetSummaryStatisticsFromCacheIfAvailableOrElseCalculate()
        {
            List<KeyValuePair<string, string>> result;
            var calculatedDataItemType = CalculatedDataItemType.SummaryStatistics;
            string localeISOCode = UserContext.Locale.ISOCode;

            // Try get cached data.
            if (TryGetCachedCalculatedResult(calculatedDataItemType, localeISOCode, out result))
            {
                return result;
            }

            // Calculate data.
            result = CalculateSummaryStatistics();
            AddResultToCache(calculatedDataItemType, localeISOCode, result);

            return result;
        }

        public List<KeyValuePair<string, string>> CalculateSummaryStatistics(ILocale locale)
        {
            CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentUICulture = locale.CultureInfo;
                Thread.CurrentThread.CurrentCulture = locale.CultureInfo;
                return CalculateSummaryStatistics();
            }
            finally
            {
                Thread.CurrentThread.CurrentUICulture = currentUICulture;
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }
        }

        private List<KeyValuePair<string, string>> CalculateSummaryStatistics()
        {
            Dictionary<string, string> observationCountData = new Dictionary<string, string>();
            if (MySettings.Calculation.SummaryStatistics.HasActiveSettings && MySettings.Calculation.SummaryStatistics.CalculateNumberOfObservationsfromObsData)
            {
                Int64 count = GetSpeciesObservationCountFromWebService(UserContext);
                observationCountData.Add(Resources.Resource.ResultViewSummaryStatisticsNumberOfObservations, Convert.ToString(count));
            }
            if (MySettings.Calculation.SummaryStatistics.HasActiveSettings && MySettings.Calculation.SummaryStatistics.CalculateNumberOfSpeciesfromObsData)
            {
                Int64 speciesCount = GetSpeciesCountFromWebService(UserContext);
                observationCountData.Add(Resources.Resource.SummaryStatisticsNumberOfSpecies, Convert.ToString(speciesCount));
            }
            List<KeyValuePair<string, string>> result = observationCountData.ToList();
            return result;
        }

        /// <summary>
        /// Get the species observation count from web service
        /// </summary>
        /// <returns>No of observations that matches my settings.</returns>
        private long GetSpeciesObservationCountFromWebService(IUserContext userContext)
        {
            var searchCriteriaManager = new SpeciesObservationSearchCriteriaManager(userContext);
            SpeciesObservationSearchCriteria searchCriteria = searchCriteriaManager.CreateSearchCriteria(MySettings);
            if (userContext.CurrentRole.IsNull() && userContext.CurrentRoles.Count > 0)
            {
                userContext.CurrentRole = userContext.CurrentRoles[0];
            }

            var displayCoordinateSystem = MySettings.Presentation.Map.DisplayCoordinateSystem;
            long speciesObservationCount = CoreData.AnalysisManager.GetSpeciesObservationCountBySearchCriteria(userContext, searchCriteria, displayCoordinateSystem);

            return speciesObservationCount;
        }

        /// <summary>
        /// Get the species count from web service
        /// </summary>
        /// <returns>No of species that matches my settings.</returns>
        private long GetSpeciesCountFromWebService(IUserContext userContext)
        {
            var searchCriteriaManager = new SpeciesObservationSearchCriteriaManager(userContext);
            SpeciesObservationSearchCriteria searchCriteria = searchCriteriaManager.CreateSearchCriteria(MySettings);
            if (userContext.CurrentRole.IsNull() && userContext.CurrentRoles.Count > 0)
            {
                userContext.CurrentRole = userContext.CurrentRoles[0];
            }

            var displayCoordinateSystem = MySettings.Presentation.Map.DisplayCoordinateSystem;
            long speciesCount = CoreData.AnalysisManager.GetSpeciesCountBySearchCriteria(userContext, searchCriteria, displayCoordinateSystem);
            return speciesCount;
        }

        public QueryComplexityEstimate GetQueryComplexityEstimate()
        {
            QueryComplexityEstimate queryComplexity = new QueryComplexityEstimate();            
            return queryComplexity;
        }
    }
}
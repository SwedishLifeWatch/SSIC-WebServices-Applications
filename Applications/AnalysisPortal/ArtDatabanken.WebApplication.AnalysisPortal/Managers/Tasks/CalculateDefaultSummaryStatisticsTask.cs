using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
//using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SummaryStatistics;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SummaryStatistics;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers.Tasks
{
    /// <summary>
    /// This class calculates default result for summary statistics report
    /// </summary>
    public class CalculateDefaultSummaryStatisticsTask : ScheduledTaskBase
    {
        public CalculateDefaultSummaryStatisticsTask(TimeSpan interval)
            : base(interval)
        {
        }

        public override ScheduledTaskType ScheduledTaskType
        {
            get { return ScheduledTaskType.CalculateDefaultSummaryStatistics; }
        }

        public override void Execute()
        {
            System.Diagnostics.Debug.WriteLine("CalculateDefaultSummaryStatisticsTask started");
            IUserContext userContext = CoreData.UserManager.GetCurrentUser();
            MySettings.MySettings mySettings = new MySettings.MySettings();
            SummaryStatisticsResultCalculator resultCalculator = new SummaryStatisticsResultCalculator(userContext, mySettings);
            //SummaryStatisticsResultCalculator resultCalculator = new SummaryStatisticsResultCalculator(userContext, mySettings);
                        
            LocaleList usedLocales = CoreData.LocaleManager.GetUsedLocales(userContext);
            foreach (ILocale locale in usedLocales)
            {                
                List<KeyValuePair<string, string>> result = resultCalculator.CalculateSummaryStatistics(locale);
                DefaultResultsManager.AddSummaryStatistics(result, locale.ISOCode);                
            }

            System.Diagnostics.Debug.WriteLine("CalculateDefaultSummaryStatisticsTask finished");
        }
    }
}

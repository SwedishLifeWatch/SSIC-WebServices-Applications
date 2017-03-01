using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers.Tasks
{
    /// <summary>
    /// This class calculates default result for species observation grid map/table
    /// </summary>
    public class CalculateDefaultTaxonGridTask : ScheduledTaskBase
    {
        public CalculateDefaultTaxonGridTask(TimeSpan interval)
            : base(interval)
        {
        }

        public override ScheduledTaskType ScheduledTaskType
        {
            get { return ScheduledTaskType.CalculateDefaultTaxonGridTask; }
        }

        public override void Execute()
        {
            System.Diagnostics.Debug.WriteLine("CalculateDefaultTaxonGridTask started");
            IUserContext userContext = CoreData.UserManager.GetCurrentUser();
            MySettings.MySettings mySettings = new MySettings.MySettings();
            TaxonGridCalculator resultCalculator = new TaxonGridCalculator(userContext, mySettings);
            TaxonGridResult result = resultCalculator.CalculateTaxonGrid(mySettings);
            DefaultResultsManager.AddGridCellTaxa(result);
            System.Diagnostics.Debug.WriteLine("CalculateDefaultTaxonGridTask finished");
        }
    }
}

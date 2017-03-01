using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Logging;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers.Tasks
{
    /// <summary>
    /// This class calculates default result for taxon grid map/table
    /// </summary>
    public class CalculateDefaultGridCellObservationsTask : ScheduledTaskBase
    {
        public CalculateDefaultGridCellObservationsTask(TimeSpan interval)
            : base(interval)
        {
        }

        public override ScheduledTaskType ScheduledTaskType
        {
            get { return ScheduledTaskType.CalculateDefaultGridCellObservationsTask; }
        }

        public override void Execute()
        {
            System.Diagnostics.Debug.WriteLine("CalculateDefaultGridCellObservationsTask started");
            IUserContext userContext = CoreData.UserManager.GetCurrentUser();            
            MySettings.MySettings mySettings = new MySettings.MySettings();
            SpeciesObservationGridCalculator resultCalculator = new SpeciesObservationGridCalculator(userContext, mySettings);
            SpeciesObservationGridResult result = resultCalculator.CalculateSpeciesObservationGridResult(mySettings);
            DefaultResultsManager.AddGridCellObservations(result);
            System.Diagnostics.Debug.WriteLine("CalculateDefaultGridCellObservationsTask finished");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers.Tasks
{
    /// <summary>
    /// Scheduled task identifier
    /// </summary>
    public enum ScheduledTaskType
    {
        RefreshSpeciesObservationDataProviders,
        CalculateDefaultGridCellObservationsTask,
        CalculateDefaultSummaryStatistics,
        CalculateDefaultTaxonGridTask
    }
}

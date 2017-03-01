using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers.Tasks
{
    /// <summary>
    /// This task clears the species observation data providers.
    /// </summary>
    public class RefreshSpeciesObservationDataProvidersTask : ScheduledTaskBase
    {
        public RefreshSpeciesObservationDataProvidersTask(TimeSpan interval)
            : base(interval)
        {
        }

        public override ScheduledTaskType ScheduledTaskType
        {
            get { return ScheduledTaskType.RefreshSpeciesObservationDataProviders; }
        }

        public override void Execute()
        {
            if (CoreData.SpeciesObservationManager != null)
            {
                if (CoreData.SpeciesObservationManager.GetType() == typeof(SpeciesObservationManagerSingleThreadCache))
                {
                    ((SpeciesObservationManagerSingleThreadCache)CoreData.SpeciesObservationManager).RefreshSpeciesObservationDataProviders();
                }
                else if (CoreData.SpeciesObservationManager.GetType() == typeof(SpeciesObservationManagerMultiThreadCache))
                {
                    ((SpeciesObservationManagerMultiThreadCache)CoreData.SpeciesObservationManager).RefreshSpeciesObservationDataProviders();
                }
            }
        }
    }
}
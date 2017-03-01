using System;
using System.Collections.Generic;
using ArtDatabanken.Database;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Data
{
    /// <summary>
    /// Handle harvest job.
    /// </summary>
    public class HarvestJobManager
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly WebServiceContext _context;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="context">Web service context.</param>
        public HarvestJobManager(WebServiceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Clean up metadata for harvest job.
        /// </summary>
        public void CleanUpHarvestJob()
        {
            _context.GetSpeciesObservationDatabase().CleanUpHarvestJob();
        }

        /// <summary>
        /// Get metadata for harvest job.
        /// </summary>
        /// <returns>
        /// The <see cref="HarvestJob"/>.
        /// </returns>
        public HarvestJob GetHarvestJob()
        {
            var harvestJob = new HarvestJob();

            using (DataReader dataReader = _context.GetSpeciesObservationDatabase().GetHarvestJob())
            {
                while (dataReader.Read())
                {
                    harvestJob.LoadHarvestJob(dataReader);
                }
            }

            using (DataReader dataReader = _context.GetSpeciesObservationDatabase().GetHarvestJobDataProviders())
            {
                harvestJob.DataProviders = new List<HarvestJobDataProvider>();

                while (dataReader.Read())
                {
                    harvestJob.LoadHarvestJobDataProviders(dataReader);
                }
            }

            using (DataReader dataReader = _context.GetSpeciesObservationDatabase().GetHarvestJobStatistics())
            {
                harvestJob.Statistics = new List<HarvestJobStatistic>();

                while (dataReader.Read())
                {
                    harvestJob.LoadHarvestJobStatistics(dataReader);
                }
            }

            return harvestJob;
        }

        /// <summary>
        /// Update metadata for harvest job.
        /// </summary>
        /// <param name="harvestJob">
        /// The harvest job.
        /// </param>
        public void SetHarvestJob(HarvestJob harvestJob)
        {
            _context.GetSpeciesObservationDatabase().SetHarvestJob(harvestJob);
        }

        /// <summary>
        /// Update metadata for harvest job's data provider.
        /// </summary>
        /// <param name="harvestJob">
        /// The harvest job.
        /// </param>
        public void SetHarvestJobDataProvider(HarvestJob harvestJob)
        {
            _context.GetSpeciesObservationDatabase().SetHarvestJobDataProvider(harvestJob);
        }
    }
}
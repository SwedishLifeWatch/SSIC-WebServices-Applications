using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Database;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Data
{
    /// <summary>
    /// Container of properties for harvest job.
    /// </summary>
    public class HarvestJob
    {
        /// <summary>
        /// Which data providers the job will harvest from.
        /// </summary>
        public List<HarvestJobDataProvider> DataProviders { get; set; }

        /// <summary>
        /// Result of harvest job.
        /// </summary>
        public List<HarvestJobStatistic> Statistics { get; set; }

        /// <summary>
        /// Date the job is currently harvesting by.
        /// </summary>
        public DateTime HarvestCurrentDate { get; set; }

        /// <summary>
        /// Date the job will harvest to.
        /// </summary>
        public DateTime HarvestEndDate { get; set; }
      
        /// <summary>
        /// Date the job will harvest from.
        /// </summary>
        public DateTime HarvestStartDate { get; set; }
      
        /// <summary>
        /// When job is ended.
        /// </summary>
        public DateTime? JobEndDate { get; set; }
        
        /// <summary>
        /// When job is started.
        /// </summary>
        public DateTime JobStartDate { get; set; }

        /// <summary>
        /// What the job is doing right now.
        /// </summary>
        public HarvestStatusEnum JobStatus { get; set; }
    }

    /// <summary>
    /// Contains data provider meta data.
    /// </summary>
    public class HarvestJobDataProvider
    {
        /// <summary>
        /// Data provider id.
        /// </summary>
        public Int32 DataProviderId { get; set; }

        /// <summary>
        /// Change id.
        /// </summary>
        public Int64 ChangeId { get; set; }
    }

    /// <summary>
    /// Contains harvest job meta data.
    /// </summary>
    public class HarvestJobStatistic
    {
        /// <summary>
        /// Data provider id.
        /// </summary>
        public Int32 DataProviderId { get; set; }

        /// <summary>
        /// Change id.
        /// </summary>
        public Int32 ChangeId { get; set; }

        /// <summary>
        /// Job status.
        /// </summary>
        public HarvestStatusEnum JobStatus { get; set; }

        /// <summary>
        /// Harvest date.
        /// </summary>
        public DateTime HarvestDate { get; set; }
    }

    /// <summary>
    /// Extension methods for HarvestJob object.
    /// </summary>
    public static class HarvestJobExtension
    {
        /// <summary>
        /// Populate harvest job information object with meta data by the data reader.
        /// </summary>
        /// <param name="harvestJob">Harvest job.</param>
        /// <param name="dataReader">Database reader.</param>
        public static void LoadHarvestJob(this HarvestJob harvestJob, ArtDatabanken.Database.DataReader dataReader)
        {
            harvestJob.JobStartDate = dataReader.GetDateTime(HarvestJobTableData.JOBSTARTDATE, DateTime.MinValue);
            harvestJob.HarvestStartDate = dataReader.GetDateTime(HarvestJobTableData.HARVESTSTARTDATE, DateTime.MinValue);
            harvestJob.HarvestCurrentDate = dataReader.GetDateTime(HarvestJobTableData.HARVESTCURRENTDATE, DateTime.MinValue);
            harvestJob.HarvestEndDate = dataReader.GetDateTime(HarvestJobTableData.HARVESTENDDATE, DateTime.MinValue);
            harvestJob.JobEndDate = dataReader.GetDateTime(HarvestJobTableData.JOBENDDATE, DateTime.MinValue);
            harvestJob.JobStatus = (HarvestStatusEnum)Enum.Parse(typeof(HarvestStatusEnum), dataReader.GetString(HarvestJobTableData.JOBSTATUS), true);
        }

        /// <summary>
        /// Populate harvest job information object with data provider meta data by the data reader.
        /// </summary>
        /// <param name="harvestJob">Harvest job.</param>
        /// <param name="dataReader">Database reader.</param>
        public static void LoadHarvestJobDataProviders(this HarvestJob harvestJob, ArtDatabanken.Database.DataReader dataReader)
        {
            HarvestJobDataProvider harvestJobDataProvider = new HarvestJobDataProvider()
                                                                {
                                                                    DataProviderId = dataReader.GetInt32(HarvestJobTableData.DATAPROVIDERID),
                                                                    ChangeId = dataReader.GetInt64(HarvestJobTableData.CHANGEID, -1)
                                                                };
            harvestJob.DataProviders.Add(harvestJobDataProvider);
        }

        /// <summary>
        /// Populate harvest job information object with harvest job meta data by the data reader.
        /// </summary>
        /// <param name="harvestJob">Harvest job.</param>
        /// <param name="dataReader">Database reader.</param>
        public static void LoadHarvestJobStatistics(this HarvestJob harvestJob, ArtDatabanken.Database.DataReader dataReader)
        {
            HarvestJobStatistic harvestJobStatistic = new HarvestJobStatistic()
                                        {
                                            DataProviderId = dataReader.GetInt32(HarvestJobTableData.DATAPROVIDERID),
                                            ChangeId = -1,
                                            JobStatus = (HarvestStatusEnum)Enum.Parse(typeof(HarvestStatusEnum), dataReader.GetString(HarvestJobTableData.JOBSTATUS), true),
                                            HarvestDate = dataReader.GetDateTime(HarvestJobTableData.HARVESTDATE, DateTime.MinValue)
                                        };
            harvestJob.Statistics.Add(harvestJobStatistic);
        }
    }
}
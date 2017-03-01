using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.Database.ElasticsearchSerializingClasses
{
    /// <summary>
    /// Class used for deserializing health information from Elastisearch. 
    /// </summary>
    public class ElasticsearchHealth
    {
        /// <summary>
        /// Current number of primary shards in the cluster. 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public Int32 active_primary_shards { get; set; }

        /// <summary>
        /// Current number of shards in the cluster.
        /// Replica shards are included in this number.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public Int32 active_shards { get; set; }

        /// <summary>
        /// Current number of active shards relative to total number of shards.
        /// Unit is %. Value of 100 means that all shards are active.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public Double active_shards_percent_as_number { get; set; }

        /// <summary>
        /// Name of the Elastisearch cluster. 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public String cluster_name { get; set; }

        /// <summary>
        /// Current number of shards in the cluster that are not used
        /// that might be used in the future.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public Int32 delayed_unassigned_shards { get; set; }

        /// <summary>
        /// Current number of shards in the cluster that are initializing.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public Int32 initializing_shards { get; set; }

        /// <summary>
        /// Current number of computers with data that are participating in the cluster. 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public Int32 number_of_data_nodes { get; set; }

        /// <summary>
        /// Current number of fetches.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public Int32 number_of_in_flight_fetch { get; set; }

        /// <summary>
        /// Current number of computers that are participating in the cluster. 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public Int32 number_of_nodes { get; set; }

        /// <summary>
        /// Current number of pending tasks.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public Int32 number_of_pending_tasks { get; set; }

        /// <summary>
        /// Current number of relocating shards in the cluster.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public Int32 relocating_shards { get; set; }

        /// <summary>
        /// Status for the Elastisearch cluster.
        /// Possible values are green, yellow or red. 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public String status { get; set; }

        /// <summary>
        /// Max delay for task.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public Int32 task_max_waiting_in_queue_millis { get; set; }

        /// <summary>
        /// Indicates if status check took too long time to respond.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public Boolean timed_out { get; set; }

        /// <summary>
        /// Current number of shards in the cluster that are not used.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public Int32 unassigned_shards { get; set; }

        /// <summary>
        /// Check if cluster is ok.
        /// </summary>
        /// <returns>True, if cluster is ok.</returns>
        public Boolean IsOk()
        {
            if (Configuration.InstallationType == InstallationType.Production)
            {
                return (status == "green") &&
                       (!timed_out) &&
                       (((Int32)active_shards_percent_as_number) == 100) &&
                       (active_shards >= 8) &&
                       (number_of_data_nodes == 2);
            }
            else
            {
                return (status == "green") &&
                       (!timed_out) &&
                       (((Int32)active_shards_percent_as_number) == 100);
            }
        }
    }
}

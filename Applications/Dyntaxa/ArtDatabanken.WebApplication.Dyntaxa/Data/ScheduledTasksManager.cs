using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Tasks;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// This class handles scheduled tasks that will be executed on given intervals.
    /// </summary>
    public static class ScheduledTasksManager
    {
        private static readonly Dictionary<ScheduledTaskType, ScheduledTaskBase> _scheduledTasks = new Dictionary<ScheduledTaskType, ScheduledTaskBase>();
        private const string ScheduledtaskStringPrefix = "ScheduledTask_";        

        private static Dictionary<ScheduledTaskType, Task> _runningTasks = new Dictionary<ScheduledTaskType, Task>();

        /// <summary>
        /// Adds the scheduled tasks that will be executed on given intervals.
        /// </summary>
        public static void AddTasks()
        {
            AddTaskAndExecute(new RefreshDyntaxaTaxonTreeTask(TimeSpan.FromHours(12)), 5);

#if !DEBUG // Only calculate data in Release mode. If you want the calculation in Debug uncomment this line and #endif.
            
#endif
        }

        /// <summary>
        /// Adds a task and queues it to run after the number of seconds taken from the "seconds" parameter
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="seconds">The seconds.</param>
        private static void AddTaskAndExecute(ScheduledTaskBase task, double seconds)
        {
            _scheduledTasks.Add(task.ScheduledTaskType, task);
            QueueTask(task, seconds <= 1 ? 1 : seconds); // needs to queue the task and run it after at least 1 second, so it will be handled by another thread.
        }

        /// <summary>
        /// Adds a task.
        /// </summary>
        /// <param name="task">The task.</param>
        private static void AddTask(ScheduledTaskBase task)
        {
            _scheduledTasks.Add(task.ScheduledTaskType, task);
            QueueTask(task);
        }

        /// <summary>
        /// Queues a task.
        /// </summary>
        /// <param name="task">The task.</param>
        private static void QueueTask(ScheduledTaskBase task)
        {
            QueueTask(task, task.Interval.TotalSeconds);
        }

        /// <summary>
        /// Queues a task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="seconds">The seconds.</param>
        private static void QueueTask(ScheduledTaskBase task, double seconds)
        {
            string name = string.Format("{0}{1}", ScheduledtaskStringPrefix, task.ScheduledTaskType);

            HttpRuntime.Cache.Insert(
                name,
                seconds,
                null,
                DateTime.Now.AddSeconds(seconds),
                Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable,
                CacheItemRemoved);
        }

        /// <summary>
        /// Triggered when the cache item is removed from cache.
        /// The task is executed and then queued again.
        /// </summary>        
        private static void CacheItemRemoved(string cacheItemKey, object seconds, CacheItemRemovedReason removedReason)
        {            
            //Task.Factory.StartNew()

            Task task = new Task(() => RunTask(cacheItemKey));
            Task continuationTask = task.ContinueWith((t) => FinishTask(cacheItemKey));
            ScheduledTaskType scheduledTaskType = GetScheduledTaskType(cacheItemKey);
            if (!_runningTasks.ContainsKey(scheduledTaskType))
            {
                _runningTasks.Add(scheduledTaskType, task);
            }
            task.Start();            
        }

        private static void FinishTask(string cacheItemKey)
        {
            ScheduledTaskType scheduledTaskType = GetScheduledTaskType(cacheItemKey);
            if (_runningTasks.ContainsKey(scheduledTaskType))
            {
                _runningTasks.Remove(scheduledTaskType);
            }
        }

        public static void ExecuteTaskNow(ScheduledTaskType scheduledTaskType)
        {
            string name = string.Format("{0}{1}", ScheduledtaskStringPrefix, scheduledTaskType);
            ScheduledTaskBase scheduledTask = GetScheduledTask(name);

            if (HttpRuntime.Cache[name] == null)
            {
                // Task doesn't exist in cache. Probably running.
                Task task;
                if (_runningTasks.TryGetValue(scheduledTaskType, out task))
                {
                    if (!task.IsCompleted)
                    {
                        task.ContinueWith((t) =>
                        {
                            scheduledTask.Execute();
                        });                        
                    }
                    else
                    {
                        scheduledTask.Execute();
                    }

                    //if (!task.IsCompleted)
                    //{                    
                    //    task.Wait();
                    //}
                    //Thread.Sleep(100);

                    //if (HttpRuntime.Cache[name] != null)
                    //{
                    //    HttpRuntime.Cache.Remove(name);
                    //}
                    //else
                    //{
                    //    scheduledTask.Execute();
                    //}

                    //RunTask(name);
                    //task.ContinueWith((t) => RunTask(name));
                }
                else
                {
                    scheduledTask.Execute();
                    //RunTask(name);
                }
            }
            else
            {
                HttpRuntime.Cache.Remove(name);
            }          
        }

        private static ScheduledTaskType GetScheduledTaskType(string cacheItemKey)
        {
            int startIndex = ScheduledtaskStringPrefix.Length;
            string substring = cacheItemKey.Substring(startIndex, cacheItemKey.Length - startIndex);
            ScheduledTaskType taskType = (ScheduledTaskType)Enum.Parse(typeof(ScheduledTaskType), substring);
            return taskType;
        }

        private static ScheduledTaskBase GetScheduledTask(string cacheItemKey)
        {
            ScheduledTaskType taskType = GetScheduledTaskType(cacheItemKey);
            ScheduledTaskBase scheduledTask = _scheduledTasks[taskType];
            return scheduledTask;
        }

        private static void RunTask(string cacheItemKey)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("{0} removed from cache", cacheItemKey));            
            ScheduledTaskBase scheduledTask = GetScheduledTask(cacheItemKey);

            //Don't run any data retreivals between 4 and 5 in the morning, just re-queue in one hour
            if (DateTime.Now.Hour == 4)
            {
                const int cacheSeconds = 3600;
                System.Diagnostics.Debug.WriteLine(string.Format("{0} is re-queued in {1}", cacheItemKey, cacheSeconds));
                QueueTask(scheduledTask, cacheSeconds);
            }
            else
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("{0} is executed", cacheItemKey));
                    scheduledTask.Execute();
                }
                catch (Exception)
                {
                }
                System.Diagnostics.Debug.WriteLine(string.Format("{0} is re-queued in {1} seconds", cacheItemKey, scheduledTask.Interval.TotalSeconds));
                QueueTask(scheduledTask);
            }
        }
    }
}

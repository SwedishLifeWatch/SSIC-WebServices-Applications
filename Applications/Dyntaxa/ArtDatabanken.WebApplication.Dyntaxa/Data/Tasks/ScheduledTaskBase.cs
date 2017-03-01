using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Tasks
{
    /// <summary>
    /// This class is a base class for all scheduled tasks
    /// </summary>
    public abstract class ScheduledTaskBase
    {
        /// <summary>
        /// Gets the scheduled task identifier.
        /// </summary>        
        public abstract ScheduledTaskType ScheduledTaskType { get; }

        /// <summary>
        /// Gets or sets the time interval the task will be run.
        /// </summary>        
        public TimeSpan Interval { get; private set; }

        /// <summary>
        /// Code to execute when the task is run.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledTaskBase"/> class.
        /// </summary>
        /// <param name="interval">The interval.</param>
        protected ScheduledTaskBase(TimeSpan interval)
        {
            Interval = interval;
        }
    }

}

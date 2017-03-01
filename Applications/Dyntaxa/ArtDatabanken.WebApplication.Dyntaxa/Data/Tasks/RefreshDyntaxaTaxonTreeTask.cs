using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Tree;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Tasks
{
    /// <summary>
    /// This task refreshes the Dyntaxa taxon tree.
    /// </summary>
    public class RefreshDyntaxaTaxonTreeTask : ScheduledTaskBase
    {
        public RefreshDyntaxaTaxonTreeTask(TimeSpan interval)
            : base(interval)
        {
        }

        public override ScheduledTaskType ScheduledTaskType
        {
            get { return ScheduledTaskType.RefreshDyntaxaTaxonTree; }
        }

        public override void Execute()
        {
            Debug.WriteLine("RefreshDyntaxaTaxonTreeTask START executing: {0}", DateTime.Now);
            var applicationUserContext = CoreData.UserManager.GetApplicationContext("sv-SE");
            TaxonRelationsTreeCacheManager.UpdateCache(applicationUserContext);
            Debug.WriteLine("RefreshDyntaxaTaxonTreeTask END executing: {0}", DateTime.Now);
        }
    }
}

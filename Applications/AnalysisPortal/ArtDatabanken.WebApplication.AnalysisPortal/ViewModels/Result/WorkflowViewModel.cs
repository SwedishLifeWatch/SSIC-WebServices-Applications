using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result
{
    /// <summary>
    /// This class is a view model for the /Result/Workflow page.
    /// </summary>
    public class WorkflowViewModel
    {
        public bool IsUserLoggedIn { get; set; }
        public bool IsUserCurrentRolePrivatePerson { get; set; }
    }
}

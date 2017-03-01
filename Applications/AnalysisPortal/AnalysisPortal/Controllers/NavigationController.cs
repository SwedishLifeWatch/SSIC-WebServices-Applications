using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using ArtDatabanken.WebApplication.AnalysisPortal.Buttons.ButtonGroups;

namespace AnalysisPortal.Controllers
{
    /// <summary>
    /// This Controller contains Actions used for navigation in the application.
    /// </summary>
    public class NavigationController : BaseController
    {
        //public PartialViewResult StateButtonGroup(ButtonGroupIdentifier buttonGroupId)
        public PartialViewResult StateButtonGroup(string buttonGroup)
        {
            try
            {                
                ButtonGroupIdentifier buttonGroupId;
                if (Enum.TryParse(buttonGroup, true, out buttonGroupId))
                {
                    var theButtonGroup = ButtonGroupManager.GetButtonGroup(buttonGroupId);
                    return PartialView("StateButtonGroup_Partial", theButtonGroup);    
                }
                else
                {
                    return PartialView("PartialViewError", new Exception(string.Format("State button group '{0}' not found", buttonGroup ?? "null")));
                }
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);
            }            
        }
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using AnalysisPortal.Controllers;
using AnalysisPortal.Helpers;
using AnalysisPortal.Helpers.ActionFilters;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal;
using ArtDatabanken.WebApplication.AnalysisPortal.Buttons;
using ArtDatabanken.WebApplication.AnalysisPortal.Buttons.ButtonGroups;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Filter;
using ArtDatabanken.WebService.Client.UserService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using MvcContrib.TestHelper;


namespace AnalysisPortal.Tests
{
    [TestClass]
    public class PageInfoManagerTest : DBTestControllerBaseTest
    {

        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void Add_Filter_Data()
        {
            PageInfo pageInfo;            
            pageInfo = PageInfoManager.GetPageInfo("Filter", "Index");
            Assert.AreEqual("Index", pageInfo.Action);
            Assert.AreEqual("Filter", pageInfo.Controller);
            Assert.AreEqual(StateButtonIdentifier.None ,pageInfo.StateButton);
            Assert.AreEqual(ButtonGroupIdentifier.Filter, pageInfo.ButtonGroup);
            
        }


        private List<Type> GetAllControllers()
        {
            var controllers = new List<Type>();
            controllers.Add(typeof(AccountController));
            controllers.Add(typeof(BaseController));
            controllers.Add(typeof(CalculationController));
            controllers.Add(typeof(CultureController));
            controllers.Add(typeof(DataController));
            controllers.Add(typeof(DetailsController));
            controllers.Add(typeof(ErrorsController));
            controllers.Add(typeof(FilterController));
            controllers.Add(typeof(HomeController));
            controllers.Add(typeof(MySettingsController));
            controllers.Add(typeof(NavigationController));
            controllers.Add(typeof(FormatController));            
            controllers.Add(typeof(ResultController));

            return controllers;
        }

        private List<Type> IgnoreActionResultTypes
        {
        get
        {
            if (_ignoreActionResultTypes == null)
            {
                List<Type> list = new List<Type>();
                list.Add(typeof (JsonResult));
                list.Add(typeof (JsonNetResult));
                list.Add(typeof (ContentResult));
                list.Add(typeof (RedirectResult));
                list.Add(typeof (RedirectToRouteResult));
                list.Add(typeof (PartialViewResult));
                list.Add(typeof (FileStreamResult));
                list.Add(typeof (FilePathResult));
                list.Add(typeof (FileResult));
                
                _ignoreActionResultTypes = list;
            }
            return _ignoreActionResultTypes;
        }
            
        }
        private List<Type> _ignoreActionResultTypes;


        private bool IgnoreActionResultType(Type type)
        {
            return IgnoreActionResultTypes.Any(ignoreActionResultType => type == ignoreActionResultType);
        }

        public List<MethodInfo> GetMethodsWithSpecificReturnType(Type type, Type returnType, bool canBeDescendant)
        {
            var list = new List<MethodInfo>();
            MethodInfo[] methods = type.GetMethods();
            foreach (MethodInfo methodInfo in methods)
            {
                if (canBeDescendant)
                {
                    if (methodInfo.ReturnType == returnType || methodInfo.ReturnType.IsSubclassOf(returnType))
                    {
                        list.Add(methodInfo);
                    }    
                }
                else
                {
                    if (methodInfo.ReturnType == returnType)
                    {
                        list.Add(methodInfo);
                    }    
                }
            }
            return list;
        }

        /// <summary>
        /// Traverses all Action methods and checks if they are defined in Page Info Manager.
        /// </summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void CheckIfAllViewActionsAreDefinedInPageInfoManager()
        {
            List<string> missingActionsInPageInfoManager = new List<string>();
            List<Type> allControllers = GetAllControllers();
            foreach (Type controllerType in allControllers)
            {                
                string controllerName = controllerType.Name.Replace("Controller", "");
                List<MethodInfo> actionResultMethods = GetMethodsWithSpecificReturnType(controllerType, typeof(ActionResult), true);                
                foreach (MethodInfo methodInfo in actionResultMethods)
                {
                    if (IgnoreActionResultType(methodInfo.ReturnType))
                        continue;
                    if (methodInfo.GetCustomAttributes(typeof (IgnorePageInfoManagerAttribute), false).Length > 0)
                        continue;                    
                    PageInfo pageInfo = PageInfoManager.GetPageInfo(controllerName, methodInfo.Name);
                    if (pageInfo == null)
                    {
                        missingActionsInPageInfoManager.Add(string.Format("{0}/{1}  ", controllerName, methodInfo.Name));
                    }
                }
            }

            string strMissingMethods = "No missing methods";
            if (missingActionsInPageInfoManager.Count > 0)
            {
                strMissingMethods = "Missing methods in PageInfoManager: ";
                foreach (string str in missingActionsInPageInfoManager)
                {
                    strMissingMethods += string.Format("\n{0}", str);
                }
            }
                       
            Assert.AreEqual(0, missingActionsInPageInfoManager.Count, strMissingMethods);                       
        }
    }
}

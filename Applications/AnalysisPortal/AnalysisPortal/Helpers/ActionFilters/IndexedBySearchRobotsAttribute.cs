using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using AnalysisPortal.Controllers;

namespace AnalysisPortal.Helpers.ActionFilters
{
    public class IndexedBySearchRobotsAttribute : System.Attribute
    {
        private static IEnumerable<MethodInfo> GetActions3()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                           .Where(t => typeof(Controller).IsAssignableFrom(t))
                           .SelectMany(
                               type =>
                               type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                   .Where(a => a.ReturnType.IsSubclassOf(typeof(ActionResult))));
        }

        private static IEnumerable<MethodInfo> GetAllMethodsWithReturnTypeActionResult()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                           .Where(t => typeof(Controller).IsAssignableFrom(t))
                           .SelectMany(
                               type =>
                               type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                   .Where(a => a.ReturnType == typeof(ActionResult) || a.ReturnType.IsSubclassOf(typeof(ActionResult))));
        }

        private static IEnumerable<Type> GetAllControllers()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                           .Where(t => typeof(Controller).IsAssignableFrom(t));
        }

        public static List<MethodInfo> GetAllRobotIndexedPages()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

    //        return Assembly.GetExecutingAssembly().GetTypes()
    //           .Where(t => (typeof(Controller).IsAssignableFrom(t)))
    //           .SelectMany(
    //               type =>
    //               type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
    //                   .Where(a => a.ReturnType == typeof(ActionResult) || a.ReturnType.IsSubclassOf(typeof(ActionResult)))
    //);

            IEnumerable<MethodInfo> methodInfos = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(Controller).IsAssignableFrom(t))
                .SelectMany(
                    type =>
                    type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .Where(a => IsDefined(a, typeof(IndexedBySearchRobotsAttribute)) && (a.ReturnType == typeof(ActionResult) || a.ReturnType.IsSubclassOf(typeof(ActionResult)))));

            return methodInfos.ToList();

            //// alt 3
            //var methods2 = assembly.GetTypes()
            //          .SelectMany(t => t.GetMethods())
            //          .Where(m => m.GetCustomAttributes(typeof(IndexedBySearchRobotsAttribute), false).Length > 0)
            //          .ToArray();

            //// alt 1
            //IEnumerable<MemberInfo> memberInfos = 
            // assembly.GetTypes()
            //.SelectMany(x => x.GetMembers())
            //.Union(assembly.GetTypes())
            //.Where(x => Attribute.IsDefined(x, typeof(IndexedBySearchRobotsAttribute)));

            //// alt 4
            //IEnumerable<MemberInfo> memberInfos2 = Assembly.GetExecutingAssembly().GetTypes()
            //               .Where(t => (typeof(Controller).IsAssignableFrom(t)))
            //               .SelectMany(
            //                   type =>
            //                   type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            //                   //.Where(a => a.IsDefined(type) (a.ReturnType == typeof(ActionResult) || a.ReturnType.IsSubclassOf(typeof(ActionResult)))));
            //                       .Where(a => IsDefined(a, typeof(IndexedBySearchRobotsAttribute)) &&  (a.ReturnType == typeof(ActionResult) || a.ReturnType.IsSubclassOf(typeof(ActionResult)))));

            //IEnumerable<MemberInfo> memberInfos3 = Assembly.GetExecutingAssembly().GetTypes()
            //               .Where(t => (typeof(BaseController).IsAssignableFrom(t)))
            //               .SelectMany(
            //                   type =>
            //                   type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            //                       //.Where(a => a.IsDefined(type) (a.ReturnType == typeof(ActionResult) || a.ReturnType.IsSubclassOf(typeof(ActionResult)))));
            //                       .Where(a => IsDefined(a, typeof(IndexedBySearchRobotsAttribute)) && (a.ReturnType == typeof(ActionResult) || a.ReturnType.IsSubclassOf(typeof(ActionResult)))));

            //// alt 2
            ////Dictionary<string, MethodInfo> methods = assembly
            ////.GetTypes()
            ////.SelectMany(x => x.GetMethods())
            ////.Where(y => y.GetCustomAttributes().OfType<IndexedBySearchRobotsAttribute>().Any())
            ////.ToDictionary(z => z.Name);

            //int xyz = 8;
        }

        public static List<MethodInfo> GetAllNonRobotIndexedPages()
        {
            IEnumerable<MethodInfo> methodInfos = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(Controller).IsAssignableFrom(t))
                .SelectMany(
                    type =>
                    type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .Where(a => !IsDefined(a, typeof(IndexedBySearchRobotsAttribute)) && (a.ReturnType == typeof(ActionResult) || a.ReturnType.IsSubclassOf(typeof(ActionResult)))));

            return methodInfos.ToList();
        }

        //public static List<string> GetAllowedPages()
        //{
        //    List<MethodInfo> allNonRobotIndexedPages = GetAllRobotIndexedPages();
        //    List<string> robotsList = new List<string>();

        //    foreach (var methodInfo in allNonRobotIndexedPages)
        //    {
        //        var robotAttributes = methodInfo
        //            .GetCustomAttributes(typeof(NoRobotsAttribute), false)
        //            .Cast<NoRobotsAttribute>();

        //        foreach (var robotAttribute in robotAttributes)
        //        {
        //            //-- run through any custom attributes on the norobots attribute. None currently specified.
        //        }

        //        List<string> namespaceSplit = methodInfo.DeclaringType.FullName.Split('.').ToList();

        //        var areaIndex = namespaceSplit.IndexOf("Areas");
        //        var area = (areaIndex > -1 ? "/" + namespaceSplit[areaIndex + 1] : "");

        //        var controllersIndex = namespaceSplit.IndexOf("Controllers");
        //        var controller = (controllersIndex > -1 ? "/" + namespaceSplit[controllersIndex + 1] : "");

        //        var action = "/" + methodInfo.Name;

        //        robotsList.Add(area + controller + action);
        //    }
        //    return robotsList;
        //}

        public static List<ActionInfo> GetAllowedPages()
        {
            List<MethodInfo> allNonRobotIndexedPages = GetAllRobotIndexedPages();
            List<ActionInfo> robotsList = new List<ActionInfo>();

            foreach (var methodInfo in allNonRobotIndexedPages)
            {
                //var robotAttributes = methodInfo
                //    .GetCustomAttributes(typeof(NoRobotsAttribute), false)
                //    .Cast<NoRobotsAttribute>();

                //foreach (var robotAttribute in robotAttributes)
                //{
                //    //-- run through any custom attributes on the norobots attribute. None currently specified.
                //}

                var controllerName = methodInfo.ReflectedType.Name.Replace("Controller", "");
                var actionName = methodInfo.Name;

                robotsList.Add(new ActionInfo(controllerName, actionName));

                //List<string> namespaceSplit = methodInfo.DeclaringType.FullName.Split('.').ToList();

                //var areaIndex = namespaceSplit.IndexOf("Areas");
                //var area = (areaIndex > -1 ? "/" + namespaceSplit[areaIndex + 1] : "");

                //var controllersIndex = namespaceSplit.IndexOf("Controllers");
                //var controller = (controllersIndex > -1 ? "/" + namespaceSplit[controllersIndex + 1] : "");

                //var action = "/" + methodInfo.Name;

                //robotsList.Add(new ActionInfo(area,controller,action));
            }
            return robotsList.Distinct().ToList();
        }

        public static List<ActionInfo> GetDisallowedPages()
        {
            List<MethodInfo> allNonRobotIndexedPages = GetAllNonRobotIndexedPages();
            List<ActionInfo> noRobotsList = new List<ActionInfo>();

            foreach (var methodInfo in allNonRobotIndexedPages)
            {
                //var robotAttributes = methodInfo
                //    .GetCustomAttributes(typeof(NoRobotsAttribute), false)
                //    .Cast<NoRobotsAttribute>();

                //foreach (var robotAttribute in robotAttributes)
                //{
                //    //-- run through any custom attributes on the norobots attribute. None currently specified.
                //}

                var controllerName = methodInfo.ReflectedType.Name.Replace("Controller", "");
                var actionName = methodInfo.Name;

                noRobotsList.Add(new ActionInfo(controllerName, actionName));

                //List<string> namespaceSplit = methodInfo.DeclaringType.FullName.Split('.').ToList();

                //var areaIndex = namespaceSplit.IndexOf("Areas");
                //var area = (areaIndex > -1 ? "/" + namespaceSplit[areaIndex + 1] : "");

                //var controllersIndex = namespaceSplit.IndexOf("Controllers");
                //var controller = (controllersIndex > -1 ? "/" + namespaceSplit[controllersIndex + 1] : "");

                //var action = "/" + methodInfo.Name;

                //noRobotsList.Add(new ActionInfo(area,controller, action));
            }
            
            return noRobotsList.Distinct().ToList();
        }

        //public static List<string> GetDisallowedPages()
        //{
        //    List<MethodInfo> allNonRobotIndexedPages = GetAllNonRobotIndexedPages();
        //    List<string> noRobotsList = new List<string>();

        //    foreach (var methodInfo in allNonRobotIndexedPages)
        //    {
        //        var robotAttributes = methodInfo
        //            .GetCustomAttributes(typeof(NoRobotsAttribute), false)
        //            .Cast<NoRobotsAttribute>();

        //        foreach (var robotAttribute in robotAttributes)
        //        {
        //            //-- run through any custom attributes on the norobots attribute. None currently specified.
        //        }

        //        List<string> namespaceSplit = methodInfo.DeclaringType.FullName.Split('.').ToList();

        //        var areaIndex = namespaceSplit.IndexOf("Areas");
        //        var area = (areaIndex > -1 ? "/" + namespaceSplit[areaIndex + 1] : "");

        //        var controllersIndex = namespaceSplit.IndexOf("Controllers");
        //        var controller = (controllersIndex > -1 ? "/" + namespaceSplit[controllersIndex + 1] : "");

        //        var action = "/" + methodInfo.Name;

        //        noRobotsList.Add(area + controller + action);
        //    }
        //    return noRobotsList;
        //}
    }

    public class ActionInfo
    {
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public ActionInfo(string area, string controller, string action)
        {
            Area = area;
            Controller = controller;
            Action = action;
        }

        public ActionInfo(string controller, string action)
        {
            Controller = controller;
            Action = action;
        }

        #region Equality members
        protected bool Equals(ActionInfo other)
        {
            return string.Equals(Area, other.Area) && string.Equals(Controller, other.Controller) && string.Equals(Action, other.Action);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((ActionInfo)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Area != null ? Area.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Controller != null ? Controller.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Action != null ? Action.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(ActionInfo left, ActionInfo right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ActionInfo left, ActionInfo right)
        {
            return !Equals(left, right);
        }
        #endregion

    }
}
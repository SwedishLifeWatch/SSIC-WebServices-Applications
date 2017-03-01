//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Web;
//using System.Web.Mvc;

//namespace AnalysisPortal.Helpers.ActionFilters
//{
//    public sealed class NoRobotsAttribute : System.Attribute
//    {

//        public static IEnumerable<MethodInfo> GetActions()
//        {
//            return Assembly.GetExecutingAssembly().GetTypes()
//                           .Where(t => (typeof (Controller).IsAssignableFrom(t)))
//                           .SelectMany(
//                               type =>
//                               type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
//                                   .Where(a => a.ReturnType == typeof (ActionResult))
//                );

//        }

//        public static IEnumerable<MethodInfo> GetActions2()
//        {
//            Type tv = typeof(ActionResult);            
//            return Assembly.GetExecutingAssembly().GetTypes()
//                           .Where(t => (typeof(Controller).IsAssignableFrom(t)))
//                           .SelectMany(
//                               type =>
//                               type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
//                                   .Where(a => a.ReturnType.BaseType == typeof(ActionResult))
//                );
            

//        }

//        public static IEnumerable<MethodInfo> GetActions3()
//        {
//            return Assembly.GetExecutingAssembly().GetTypes()
//                           .Where(t => (typeof(Controller).IsAssignableFrom(t)))
//                           .SelectMany(
//                               type =>
//                               type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
//                                   .Where(a => a.ReturnType.IsSubclassOf(typeof(ActionResult)))
//                );            
//        }

//        public static IEnumerable<MethodInfo> GetActions4()
//        {
//            return Assembly.GetExecutingAssembly().GetTypes()
//                           .Where(t => (typeof(Controller).IsAssignableFrom(t)))
//                           .SelectMany(
//                               type =>
//                               type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
//                                   .Where(a => a.ReturnType == typeof(ActionResult) ||  a.ReturnType.IsSubclassOf(typeof(ActionResult)))
//                );
//        }
 

//        public static IEnumerable<Type> GetControllers()
//        {
//            return Assembly.GetExecutingAssembly().GetTypes()
//                           .Where(t => (typeof (Controller).IsAssignableFrom(t)));

//        }

//        public static List<string> GetNoRobots()
//        {
//            var robotList = new List<string>();
//            var controllers = GetControllers();
//            var actions1 = GetActions();
//            var action2 = GetActions2();
//            var action3 = GetActions3();
//            var action4 = GetActions4();

//            foreach (var methodInfo in GetControllers().Where(w => w.DeclaringType != null))
//            {
//                var robotAttributes = methodInfo
//                    .GetCustomAttributes(typeof (NoRobotsAttribute), false)
//                    .Cast<NoRobotsAttribute>();

//                foreach (var robotAttribute in robotAttributes)
//                {
//                    //-- run through any custom attributes on the norobots attribute. None currently specified.
//                }
//                List<string> namespaceSplit = methodInfo.DeclaringType.FullName.Split('.').ToList();

//                var controllersIndex = namespaceSplit.IndexOf("Controllers");
//                var controller = (controllersIndex > -1 ? "/" + namespaceSplit[controllersIndex + 1] : "");
//                robotList.Add(controller);

//            }

//            foreach (var methodInfo in GetActions())
//            {
//                var robotAttributes = methodInfo
//                    .GetCustomAttributes(typeof (NoRobotsAttribute), false)
//                    .Cast<NoRobotsAttribute>();

//                foreach (var robotAttribute in robotAttributes)
//                {
//                    //-- run through any custom attributes on the norobots attribute. None currently specified.
//                }

//                List<string> namespaceSplit = methodInfo.DeclaringType.FullName.Split('.').ToList();

//                var areaIndex = namespaceSplit.IndexOf("Areas");
//                var area = (areaIndex > -1 ? "/" + namespaceSplit[areaIndex + 1] : "");

//                var controllersIndex = namespaceSplit.IndexOf("Controllers");
//                var controller = (controllersIndex > -1 ? "/" + namespaceSplit[controllersIndex + 1] : "");

//                var action = "/" + methodInfo.Name;

//                robotList.Add(area + controller + action);

//            }
//            return robotList;
//        }
//    }

//}
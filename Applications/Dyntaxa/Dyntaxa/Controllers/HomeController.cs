using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Tasks;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Tree;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers.Extensions;

namespace Dyntaxa.Controllers
{
    public class HomeController : DyntaxaBaseController
    {
        public ActionResult About()
        {
            IUserContext user = GetCurrentUser();            
            if (user.IsTaxonRevisionAdministrator())
            {
                ViewBag.ShowClearCacheLink = true;
            }            
            AboutViewModel model = new AboutViewModel();
            DateTime creationTime = GetBuildDateTime(typeof(Dyntaxa.MvcApplication).Assembly);
            model.ServerName = GetServerName();
            model.CreationDate = creationTime.ToShortDateString() + " (" + creationTime.ToShortTimeString() + ")";
            model.Version = typeof(Dyntaxa.MvcApplication).Assembly.GetName().Version.ToString();
            ViewData.Model = model;
            return View();
        }    

        public ActionResult TermsOfUse()
        {
            return View();
        }

        public ActionResult Cookies()
        {
            return View();
        }
        public ActionResult Licensing()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AcceptCookies()
        {
            var cookie = new HttpCookie("AcceptCookies") { Expires = DateTime.Now.AddYears(10) };
            cookie["Accepted"] = "true";
            Response.Cookies.Add(cookie);

            return Json(true);
        }

        public ContentResult TestRefreshTaxonTree()
        {
            IUserContext userContext = GetApplicationUser();
            Task.Factory.StartNew(() => TaxonRelationsTreeCacheManager.UpdateCache(userContext));
            //ScheduledTasksManager.ExecuteTaskNow(ScheduledTaskType.RefreshDyntaxaTaxonTree);
            return Content("Tree updated");
        }        

#pragma warning disable SA1300 // Element must begin with upper-case letter
        struct _IMAGE_FILE_HEADER
#pragma warning restore SA1300 // Element must begin with upper-case letter
        {
            public ushort Machine;
            public ushort NumberOfSections;
            public uint TimeDateStamp;
            public uint PointerToSymbolTable;
            public uint NumberOfSymbols;
            public ushort SizeOfOptionalHeader;
            public ushort Characteristics;
        }

        /// <summary>
        /// Returns the name of the server were the application is currently running
        /// </summary>
        /// <returns></returns>
        private string GetServerName()
        {
            string machineName = Environment.MachineName;
            return machineName;
        }

        static DateTime GetBuildDateTime(Assembly assembly)
        {
            if (System.IO.File.Exists(assembly.Location))
            {
                var buffer = new byte[Math.Max(Marshal.SizeOf(typeof(_IMAGE_FILE_HEADER)), 4)];
                using (var fileStream = new FileStream(assembly.Location, FileMode.Open, FileAccess.Read))
                {
                    fileStream.Position = 0x3C;
                    fileStream.Read(buffer, 0, 4);
                    fileStream.Position = BitConverter.ToUInt32(buffer, 0); // COFF header offset
                    fileStream.Read(buffer, 0, 4); // "PE\0\0"
                    fileStream.Read(buffer, 0, buffer.Length);
                }
                var pinnedBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                try
                {
                    var coffHeader = (_IMAGE_FILE_HEADER)Marshal.PtrToStructure(pinnedBuffer.AddrOfPinnedObject(), typeof(_IMAGE_FILE_HEADER));

                    return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1) + new TimeSpan(coffHeader.TimeDateStamp * TimeSpan.TicksPerSecond));
                }
                finally
                {
                    pinnedBuffer.Free();
                }
            }
            return new DateTime();
        }
    }
}

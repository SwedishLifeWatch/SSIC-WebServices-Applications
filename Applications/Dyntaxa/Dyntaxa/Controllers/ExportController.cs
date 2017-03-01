using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Export;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;
using System.Runtime.Caching;
using System.IO;
using System.Text;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Tasks;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Tree;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers.Extensions;
using ArtDatabanken.WebService.Client.TaxonAttributeService;
using ArtDatabanken.WebService.Client.TaxonService;
using ArtDatabanken.WebService.Proxy;
using Dyntaxa.Helpers;

namespace Dyntaxa.Controllers
{
    public class ExportController : DyntaxaBaseController
    {
        /// <summary>
        /// Replaces all illegal file name characters with '_'
        /// and returns a legal file name
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string GetValidFileName(string filename)
        {            
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(c, '_');
            }
            return filename;
        }

        [HttpGet]
        public ActionResult TaxonList(string taxonId)
        {
            TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
            if (searchResult.NumberOfMatches != 1)
            {
                return RedirectToSearch(taxonId);
            }

            ITaxon taxon = searchResult.Taxon;
            this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
            ViewBag.Taxon = taxon;
            var model = ExportViewModel.Create(GetCurrentUser(), taxon, false);
            ModelState.Remove("TaxonId");

            return View(model);
        }

        [HttpPost]
        public ActionResult TaxonList(ExportViewModel model, List<int> filterTaxonCategories, List<int> outputTaxonCategories, List<int> outputTaxonNames, List<int> filterSwedishOccurrence, List<int> filterSwedishHistory, string downloadTokenValue)
        {
            if (filterTaxonCategories != null && filterTaxonCategories.Count > 0)
            {
                filterTaxonCategories.Remove(model.AllTaxonCategoryId);
            }

            if (filterTaxonCategories == null || filterTaxonCategories.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "No category is selected");
            }
            ITaxon taxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), model.TaxonId);

            if (ModelState.IsValid)
            {                
                model.ReInitialize(GetCurrentUser(), taxon, false, filterTaxonCategories, outputTaxonCategories, outputTaxonNames, filterSwedishOccurrence, filterSwedishHistory);

                var manager = new ExportManager(model, GetCurrentUser());
                manager.CreateExportItems();
                ExcelFileFormat fileFormat = ExcelFileFormat.OpenXml;                
                MemoryStream excelFileStream = manager.CreateExcelFile(fileFormat);                
                var fileStreamResult = new FileStreamResult(excelFileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                var fileDownloadName = GetValidFileName(taxon.ScientificName) + ExcelFileFormatHelper.GetExtension(fileFormat);
                fileStreamResult.FileDownloadName = fileDownloadName;                    
                Response.AppendCookie(new HttpCookie("fileDownloadToken", downloadTokenValue)); 
                return fileStreamResult;
            }

            model = ExportViewModel.Create(GetCurrentUser(), taxon, false);
            return View(model);
        }
        
        public ActionResult DefaultStraightFile(string taxonId)
        {           
            TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
            if (searchResult.NumberOfMatches != 1)
            {
                return RedirectToSearch(taxonId);
            }

            ITaxon taxon = searchResult.Taxon;
            this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
            ViewBag.Taxon = taxon;
            var model = ExportViewModel.Create(GetCurrentUser(), taxon, false);            

            var manager = new ExportManager(model, GetCurrentUser());
            manager.CreateExportItems();
            ExcelFileFormat fileFormat = ExcelFileFormat.OpenXml;            

            var fileDownloadName = GetValidFileName(taxon.ScientificName) + ExcelFileFormatHelper.GetExtension(fileFormat);
            MemoryStream excelFileStream = manager.CreateExcelFile(fileFormat);
            var fileStreamResult = new FileStreamResult(excelFileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fileStreamResult.FileDownloadName = fileDownloadName;            

            return fileStreamResult;
        }

        [HttpGet]
        public ActionResult HierarchicalTaxonList(string taxonId)
        {
            TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
            if (searchResult.NumberOfMatches != 1)
            {
                return RedirectToSearch(taxonId);
            }

            ITaxon taxon = searchResult.Taxon;
            this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
            ViewBag.Taxon = taxon;
            var model = ExportViewModel.Create(GetCurrentUser(), taxon, true);
            ModelState.Remove("TaxonId");

            return View("TaxonList", model);
        }

        [HttpPost]
        public ActionResult HierarchicalTaxonList(ExportViewModel model, List<int> filterTaxonCategories, List<int> outputTaxonCategories, List<int> outputTaxonNames, List<int> filterSwedishOccurrence, List<int> filterSwedishHistory, string downloadTokenValue)
        {
            if (filterTaxonCategories != null && filterTaxonCategories.Count > 0)
            {
                filterTaxonCategories.Remove(model.AllTaxonCategoryId);
            }

            if (filterTaxonCategories == null || filterTaxonCategories.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "No category is selected");
            }
            ITaxon taxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), model.TaxonId);

            if (ModelState.IsValid)
            {
                model.ReInitialize(GetCurrentUser(), taxon, true, filterTaxonCategories, outputTaxonCategories, outputTaxonNames, filterSwedishOccurrence, filterSwedishHistory);

                var manager = new ExportManager(model, GetCurrentUser());
                manager.CreateExportItems();
                
                ExcelFileFormat fileFormat = ExcelFileFormat.OpenXml;                
                
                var fileDownloadName = GetValidFileName(taxon.ScientificName) + ExcelFileFormatHelper.GetExtension(fileFormat);
                MemoryStream excelFileStream = manager.CreateExcelFile(fileFormat);
                var fileStreamResult = new FileStreamResult(excelFileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                fileStreamResult.FileDownloadName = fileDownloadName;
                Response.AppendCookie(new HttpCookie("fileDownloadToken", downloadTokenValue)); 
                return fileStreamResult;
            }

            model = ExportViewModel.Create(GetCurrentUser(), taxon, true);
            return View(model);
        }

        [HttpGet]        
        public ActionResult Database(string taxonId)
        {
            TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
            if (searchResult.NumberOfMatches != 1)
            {
                return RedirectToSearch(taxonId);
            }

            ITaxon taxon = searchResult.Taxon;
            this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
            ViewBag.Taxon = taxon;

            var model = ExportDatabaseViewModel.Create(GetCurrentUser(), taxon);
            model.ClipBoard = this.TaxonIdentifier.Id.Value.ToString();
            // RowDelimiter Dropbox
            var rowDelimiters = from MatchTaxonRowDelimiter rd in Enum.GetValues(typeof(MatchTaxonRowDelimiter))
                                select new { value = (int)rd, text = rd.GetLocalizedDescription() };
            ViewData["RowDelimiter"] = new SelectList(rowDelimiters, "value", "text", model.RowDelimiter.ToString());
            ModelState.Remove("TaxonId");
            
            return View(model);
        }

        [HttpPost]             
        public ActionResult Database(ExportDatabaseViewModel model, string downloadTokenValue)
        {
             model.UserContext = GetCurrentUser();
                      
            if (ModelState.IsValid)
            {                                
             model.CreateDataTables();
              
                int maxNumberOfRows = 0;
                foreach (System.Data.DataTable dataTable in model.DataTables)
                {
                    if (dataTable != null && dataTable.Rows != null)
                    {
                        maxNumberOfRows = Math.Max(maxNumberOfRows, dataTable.Rows.Count);
                    }
                }
                ExcelFileFormat fileFormat = ExcelFileFormat.OpenXml;

                MemoryStream excelFileStream = model.CreateExcelFile(fileFormat);
                var fileStreamResult = new FileStreamResult(excelFileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");                
                var fileDownloadName = "Dyntaxa database" + ExcelFileFormatHelper.GetExtension(fileFormat);
                fileStreamResult.FileDownloadName = fileDownloadName;
                Response.AppendCookie(new HttpCookie("fileDownloadToken", downloadTokenValue));
                return fileStreamResult;
            }
            //Task.Factory.StartNew(() => GenerateReport(model));

            //model = ExportViewModel.Create(GetCurrentUser(), taxon, true);
            // RowDelimiter Dropbox
            var rowDelimiters = from MatchTaxonRowDelimiter rd in Enum.GetValues(typeof(MatchTaxonRowDelimiter))
                                select new { value = (int)rd, text = rd.GetLocalizedDescription() };
            ViewData["RowDelimiter"] = new SelectList(rowDelimiters, "value", "text", model.RowDelimiter.ToString());
            
            return View(model);
        }

        [HttpGet]
        public ActionResult DatabaseNew(string taxonId)
        {
            TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
            if (searchResult.NumberOfMatches != 1)
            {
                return RedirectToSearch(taxonId);
            }

            ITaxon taxon = searchResult.Taxon;
            this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
            ViewBag.Taxon = taxon;

            var model = ExportDatabaseViewModel.Create(GetCurrentUser(), taxon);
            model.ClipBoard = this.TaxonIdentifier.Id.Value.ToString();
            // RowDelimiter Dropbox
            var rowDelimiters = from MatchTaxonRowDelimiter rd in Enum.GetValues(typeof(MatchTaxonRowDelimiter))
                                select new { value = (int)rd, text = rd.GetLocalizedDescription() };
            ViewData["RowDelimiter"] = new SelectList(rowDelimiters, "value", "text", model.RowDelimiter.ToString());
            ModelState.Remove("TaxonId");

            return View(model);
        }

        [HttpPost]
        public ActionResult DatabaseNew(ExportDatabaseViewModel model, string downloadTokenValue)
        {
            model.UserContext = GetCurrentUser();
            
            if (ModelState.IsValid)
            {
                //model.CreateDataTables();
                List<ITaxon> taxa = model.GetTaxa();
                NewExportDataTableCollection result = ExportDatabaseViewModel.CreateDataTablesWithDyntaxaTree(GetCurrentUser(), taxa);
                List<DataTable> dataTables = new List<DataTable>();
                dataTables.Add(result.TaxonDataTable);
                dataTables.Add(result.TaxonNameDataTable);
                dataTables.Add(result.ParentChildDataTable);                
                dataTables.Add(result.SplitTaxonDataTable);
                dataTables.Add(result.LumpTaxonDataTable);
                dataTables.Add(result.TaxonReferencesDataTable);
                dataTables.Add(result.TaxonNameReferencesTable);
                dataTables.Add(result.TaxonCategoriesTable);
                dataTables.Add(result.TaxonNameCategoriesTable);
                dataTables.Add(result.TaxonNameStatusTable);
                dataTables.Add(result.TaxonNameUsageTable);                
                dataTables.Add(result.ChangeStatusTable);
                dataTables.Add(result.AlertStatusTable);
                dataTables.Add(result.AllReferencesTable);

                ExcelFileFormat fileFormat = ExcelFileFormat.OpenXml;
                var excelFile = new ExportDatabaseExcelFile();
                MemoryStream excelFileStream = excelFile.CreateExcelFile(dataTables, fileFormat, true);
                
                var fileStreamResult = new FileStreamResult(excelFileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                var fileDownloadName = "Dyntaxa database" + ExcelFileFormatHelper.GetExtension(fileFormat);
                fileStreamResult.FileDownloadName = fileDownloadName;
                Response.AppendCookie(new HttpCookie("fileDownloadToken", downloadTokenValue));
                return fileStreamResult;
            }
    
            // RowDelimiter Dropbox
            var rowDelimiters = from MatchTaxonRowDelimiter rd in Enum.GetValues(typeof(MatchTaxonRowDelimiter))
                                select new { value = (int)rd, text = rd.GetLocalizedDescription() };
            ViewData["RowDelimiter"] = new SelectList(rowDelimiters, "value", "text", model.RowDelimiter.ToString());

            return View(model);
        }

        public ActionResult RefreshTree()
        {
            IUserContext user = GetCurrentUser();
            if (user.IsWebServiceAdministrator())
            {
                WebServiceProxy.TaxonService.ClearCache(new TaxonDataSource().GetClientInformation(user));
                CacheManager.FireRefreshCache(GetCurrentUser());
                ScheduledTasksManager.ExecuteTaskNow(ScheduledTaskType.RefreshDyntaxaTaxonTree);
                return Content("Taxon service cache cleared. Dyntaxa application cache cleared. Taxon tree is being refreshed from database. Wait 1-2 minute.");
            }
            else if (user.IsTaxonRevisionAdministrator() || user.IsTaxonEditor())
            {
                CacheManager.FireRefreshCache(GetCurrentUser());
                ScheduledTasksManager.ExecuteTaskNow(ScheduledTaskType.RefreshDyntaxaTaxonTree);
                return Content("Dyntaxa application cache cleared. Taxon service cache not cleared (you need Web service administration authority). Taxon tree is being refreshed from database. Wait 1-2 minute.");
            }
            else
            {
                return Content("Not authorized to refresh tree since you're not Taxon editor or Taxon revision administrator.");
            }
        }

        [HttpGet]        
        public ActionResult Graphviz(string taxonId)
        {
            IUserContext userContext = GetCurrentUser();
            if (!(userContext.IsTaxonRevisionAdministrator() || userContext.IsTaxonEditor()))
            {
                return RedirectToAction("AccessIsNotAllowed", "Account", new { url = this.Request.Url });
            }
                
            TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
            ExportGraphVizViewModel model = null; 
            ITaxon taxon = null;
            if (searchResult.NumberOfMatches == 1)
            {
                taxon = searchResult.Taxon;
                this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
                ViewBag.Taxon = taxon;                
                model = new ExportGraphVizViewModel();
                model.ClipBoard = this.TaxonIdentifier.Id.Value.ToString();
            }
            else
            {
                //model = ExportDatabaseViewModel.Create(GetCurrentUser(), taxon);
                model = new ExportGraphVizViewModel();
                model.ClipBoard = "";
            }            
            var rowDelimiters = from MatchTaxonRowDelimiter rd in Enum.GetValues(typeof(MatchTaxonRowDelimiter))
                                select new { value = (int)rd, text = rd.GetLocalizedDescription() };
            ViewData["RowDelimiter"] = new SelectList(rowDelimiters, "value", "text", model.RowDelimiter.ToString());
            ModelState.Remove("TaxonId");
                       
            if (userContext.IsTaxonRevisionAdministrator() || userContext.IsTaxonEditor())
            {
                ViewBag.ShowRefreshDyntaxaTaxonTreeButton = true;
            }
            else
            {
                ViewBag.ShowRefreshDyntaxaTaxonTreeButton = false;
            }
            ViewBag.TreeLastUpdatedTime = TaxonRelationsTreeCacheManager.CacheLastUpdatedTime;            

            return View(model);
        }

        public JsonNetResult GetGraphVizJson(ExportGraphVizViewModel model)
        {
            IUserContext userContext = GetCurrentUser();
            if (!(userContext.IsTaxonRevisionAdministrator() || userContext.IsTaxonEditor()))
            {
                return new JsonNetResult("Access is not allowed");                
            }

            if (TaxonRelationsTreeCacheManager.CachedTaxonRelationTree == null)
            {
                return new JsonNetResult("Dyntaxa Tree not yet created. Please try again in 1 minute.");                
            }

            if (ModelState.IsValid)
            {
                var tree = TaxonRelationsTreeCacheManager.CachedTaxonRelationTree;

                string graphviz = GraphvizManager.CreateGraphvizFormatRepresentation(
                    userContext,
                    tree,
                    tree.GetTreeNodes(model.GetTaxonIdsFromString()),
                    model.TreeIterationMode,
                    model.RelationTypeMode == TaxonRelationsTreeRelationTypeMode.OnlyValidRelations,
                    new GraphVizFormat()
                    {
                        ShowRelationId = model.ShowRelationId,
                        ShowLumpsAndSplits = model.IncludeLumpSplits
                    });

                return new JsonNetResult(graphviz);                
            }
            
            return new JsonNetResult("Unexpected error");
        }

        [HttpPost]        
        public ActionResult Graphviz(ExportGraphVizViewModel model, string downloadTokenValue)
        {
            IUserContext userContext = GetCurrentUser();
            if (!(userContext.IsTaxonRevisionAdministrator() || userContext.IsTaxonEditor()))
            {
                return RedirectToAction("AccessIsNotAllowed", "Account", new { url = this.Request.Url });
            }
            
            if (TaxonRelationsTreeCacheManager.CachedTaxonRelationTree == null)
            {
                ModelState.AddModelError("TreeError", "Dyntaxa Tree not yet created. Please try again in 1 minute.");
            }

            if (ModelState.IsValid)
            {
                var tree = TaxonRelationsTreeCacheManager.CachedTaxonRelationTree;                

                string graphviz = GraphvizManager.CreateGraphvizFormatRepresentation(
                    userContext,
                    tree,
                    tree.GetTreeNodes(model.GetTaxonIdsFromString()),                    
                    model.TreeIterationMode,
                    model.RelationTypeMode == TaxonRelationsTreeRelationTypeMode.OnlyValidRelations,
                    new GraphVizFormat()
                    {
                        ShowRelationId = model.ShowRelationId,
                        ShowLumpsAndSplits = model.IncludeLumpSplits
                    });                      
                return Content(graphviz);
            }
            
            // RowDelimiter Dropbox
            var rowDelimiters = from MatchTaxonRowDelimiter rd in Enum.GetValues(typeof(MatchTaxonRowDelimiter))
                                select new { value = (int)rd, text = rd.GetLocalizedDescription() };
            ViewData["RowDelimiter"] = new SelectList(rowDelimiters, "value", "text", model.RowDelimiter.ToString());
            
            if (userContext.IsTaxonRevisionAdministrator() || userContext.IsTaxonEditor())
            {
                ViewBag.ShowRefreshDyntaxaTaxonTreeButton = true;
            }
            else
            {
                ViewBag.ShowRefreshDyntaxaTaxonTreeButton = false;
            }
            ViewBag.TreeLastUpdatedTime = TaxonRelationsTreeCacheManager.CacheLastUpdatedTime;

            return View(model);
        }      
    
        protected void GenerateReport(ExportDatabaseViewModel model)
        {            
            //Thread.Sleep(10000);
            //var fileDownloadName = "Dyntaxa database.xls";            
            //MemoryStream excelFileStream = model.CreateExcelFile();

            //var fileStreamResult = new FileStreamResult(excelFileStream, "application/vnd.ms-excel");
            //fileStreamResult.FileDownloadName = fileDownloadName;
        }

        //public static void SaveMemoryStream(MemoryStream ms, string fileName)
        //{
        //    byte[] data = ms.ToArray();
        //    var fileDownloadName = "Dyntaxa database.xls";
        //    var fs = new FileStream(fileDownloadName, FileMode.CreateNew);
        //    fs.Write(data, 0, data.Length);
        //    fs.Close();

        //}       
    }
}


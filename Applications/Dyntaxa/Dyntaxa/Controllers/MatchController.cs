using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArtDatabanken;
using ArtDatabanken.IO;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Match;

namespace Dyntaxa.Controllers
{
    public class MatchController : DyntaxaBaseController
    {
        private void CreateMatchSelectLists(MatchSettingsViewModel model)
        {
            // Column content Dropbox
            var columnContent = from MatchColumnContentAlternative cC in Enum.GetValues(typeof(MatchColumnContentAlternative))
                                select new { value = (int)cC, text = cC.GetLocalizedDescription() };
            ViewData["ColumnContentAlternative"] = new SelectList(columnContent, "value", "text", model.ColumnContentAlternative.ToString());

            // RowDelimiter Dropbox
            var rowDelimiters = from MatchTaxonRowDelimiter rd in Enum.GetValues(typeof(MatchTaxonRowDelimiter))
                                select new { value = (int)rd, text = rd.GetLocalizedDescription() };
            ViewData["RowDelimiter"] = new SelectList(rowDelimiters, "value", "text", model.RowDelimiter.ToString());

            // ColumnDelimiter Dropbox
            var columnDelimiters = from MatchTaxonColumnDelimiter rd in Enum.GetValues(typeof(MatchTaxonColumnDelimiter))
                                   select new { value = (int)rd, text = rd.GetLocalizedDescription() };
            ViewData["ColumnDelimiter"] = new SelectList(columnDelimiters, "value", "text", model.ColumnDelimiter.ToString());

            // MatchTaxonToType Dropbox
            var matchToTypes = from MatchTaxonToType mt in Enum.GetValues(typeof(MatchTaxonToType))
                               select new { value = (int)mt, text = mt.GetLocalizedDescription() };
            ViewData["MatchToType"] = new SelectList(matchToTypes, "value", "text", model.MatchToType.ToString());
        }

        private string GetFileName(string fileExtension)
        {
            string tempDirectory = Resources.DyntaxaSettings.Default.PathToTempDirectory;
            string fileName = Path.GetRandomFileName();
            fileName = Path.ChangeExtension(fileName, fileExtension);            
            return System.Web.HttpContext.Current.Server.MapPath(Path.Combine(tempDirectory, fileName));
        }

        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("Settings", new { TaxonId = 0 });
        }

        [HttpGet]
        public ActionResult Settings(string taxonId)
        {
            TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
            if (searchResult.NumberOfMatches != 1)
            {
                return RedirectToSearch(taxonId);
            }

            ITaxon taxon = searchResult.Taxon;
            this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
            ViewBag.Taxon = taxon;
            var viewManager = new MatchViewManager(GetCurrentUser());
            MatchSettingsViewModel model = viewManager.GetMatchSettingsViewModel(taxon);
            CreateMatchSelectLists(model);
            ModelState.Remove("TaxonId");

            return View(model);
        }

        [HttpPost]
        public ActionResult Settings(MatchSettingsViewModel model)
        {
            CreateMatchSelectLists(model);            
           
            if (ModelState.IsValid)
            {
                var manager = new DyntaxaMatchManager(GetCurrentUser());
                List<DyntaxaMatchItem> items;

                if (model.MatchToType == MatchTaxonToType.TaxonId)
                {
                    model.LabelForProvidedText = Resources.DyntaxaResource.MatchOptionsOutputProvidedTaxonIdLabel;
                }
                
                bool fileFound = false;
                if (Request.Files.Count > 0)
                {
                    foreach (string file in Request.Files)
                    {
                        HttpPostedFileBase hpf = this.Request.Files[file];
                        if (hpf == null || hpf.ContentLength == 0)
                        {
                            continue;
                        }
                        fileFound = true;
                        
                        string fileExtension = Path.GetExtension(hpf.FileName);                        
                        if (fileExtension == ".xls" || fileExtension == ".xlsx")
                        {
                            string fileName = GetFileName(fileExtension);
                            hpf.SaveAs(fileName);
                            DyntaxaLogger.WriteMessage("Match->Settings saved uploaded file: " + fileName);
                            model.FileName = fileName;
                            items = manager.GetMatches(fileName, model);

                            try
                            {
                                if (System.IO.File.Exists(fileName))
                                {
                                    System.IO.File.Delete(fileName);
                                    DyntaxaLogger.WriteMessage("Match->Settings deleted uploaded file: " + fileName);
                                }
                            }
                            catch
                            {
                                DyntaxaLogger.WriteMessage("Match->Settings exception when trying to delete: " + fileName);
                            }                                                       

                            if (items.IsNotEmpty())
                            {
                                Session["MatchOptions"] = model;
                                Session["MatchItems"] = items;
                                return RedirectToAction("MatchItems");
                            }
                            else
                            {
                                //TODO: Error. File has no valid content or is not a valid excel file.
                            }
                        }
                        else
                        {
                            ViewData.ModelState.AddModelError("FileName", Resources.DyntaxaResource.MatchOptionsFileExtensionError);
                        }
                    }
                }

                if (!fileFound)                
                {
                    if (model.ClipBoard.IsNotEmpty())
                    {                                                
                        items = manager.GetDyntaxaMatchItemsFromText(model.ClipBoard, model);
                        if (items.IsNotEmpty())
                        {
                            Session["MatchOptions"] = model;
                            Session["MatchItems"] = items;
                            return RedirectToAction("MatchItems");
                        }
                        else
                        {
                            //TODO: Error. File has no valid content or is not a valid excel file.
                        }
                    }
                    else
                    {
                        //TODO: Handle error when data is missing
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult MatchItems()
        {
            var options = (MatchSettingsViewModel)Session["MatchOptions"];
            var items = (List<DyntaxaMatchItem>)Session["MatchItems"];

            if (options.IsNull() || items.IsNull())
            {
                return RedirectToAction("Settings");
            }

            var manager = new DyntaxaMatchManager(GetCurrentUser());
            List<DyntaxaMatchItem> problematicItems = manager.GetMatchProblems(items);
            if (problematicItems.Count > 0)
            {
                ViewData["MatchItems"] = problematicItems;
                foreach (DyntaxaMatchItem item in problematicItems)
                {
                    if (item.Status == MatchStatus.NeedsManualSelection)
                    {
                        ViewData[item.DropDownListIdentifier] = item.AlternativeTaxa;
                    }
                }
                return View(options);
            }

            return RedirectToAction("ResultTable");            
        }

        [HttpPost]
        public ActionResult MatchItems(FormCollection form, string submitButton, string downloadTokenValue)
        {
            if (submitButton == null)
            {
                submitButton = "excelbutton";
            }

            var options = (MatchSettingsViewModel)Session["MatchOptions"];
            var items = (List<DyntaxaMatchItem>)Session["MatchItems"];

            if (options.IsNull() || items.IsNull())
            {
                return RedirectToAction("Settings");
            }

            foreach (var item in items)
            {
                if (item.Status == MatchStatus.NeedsManualSelection)
                {                    
                    item.TaxonId = int.Parse(form[item.DropDownListIdentifier]);
                }
            }

            Session["MatchItems"] = items;

            if (submitButton == "excelbutton" || submitButton == Resources.DyntaxaResource.MatchOptionsSaveAsExcel)
            {
                return CreateExcelFile(options, items, downloadTokenValue);
            }

            return RedirectToAction("ResultTable");
        }

        [HttpGet]
        public ActionResult ResultTable()
        {
            var options = (MatchSettingsViewModel)Session["MatchOptions"];
            var items = (List<DyntaxaMatchItem>)Session["MatchItems"];

            if (options.IsNull() || items.IsNull())
            {
                return RedirectToAction("Settings");
            }

            var manager = new DyntaxaMatchManager(GetCurrentUser());
            items = manager.GetMatchResults(items, options);

            ViewData["MatchItems"] = items;
            Session["MatchItems"] = items;

            return View(options);
        }

        [HttpPost]
        public ActionResult ResultTable(string submitButton, string downloadTokenValue)
        {
            var options = (MatchSettingsViewModel)Session["MatchOptions"];
            var items = (List<DyntaxaMatchItem>)Session["MatchItems"];
            return CreateExcelFile(options, items, downloadTokenValue);
        }

        public ActionResult CreateExcelFile(MatchSettingsViewModel options, List<DyntaxaMatchItem> items, string downloadTokenValue)
        {
            if (options.IsNull() || items.IsNull())
            {
                return RedirectToAction("Settings");
            }

            var manager = new DyntaxaMatchManager(GetCurrentUser());
            items = manager.GetMatchResults(items, options);

            ExcelFileFormat fileFormat = ExcelFileFormat.OpenXml;         
            var fileDownloadName = "match" + ExcelFileFormatHelper.GetExtension(fileFormat);
            MemoryStream excelFileStream = manager.CreateExcelFile(options, items, fileFormat);
            var fileStreamResult = new FileStreamResult(excelFileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            fileStreamResult.FileDownloadName = fileDownloadName;            
            Response.AppendCookie(new HttpCookie("fileDownloadToken", downloadTokenValue)); 
            return fileStreamResult;
        }

        ////
        //// GET: /Match/ 

        //public ActionResult Index()
        //{
        //    return RedirectToAction("Options");
        //}

        ///// <summary>
        ///// GET: /Match/Ambiguities
        ///// Match all names or identifiers provided in a list.
        ///// </summary>
        ///// <returns>A view that communicate possible match problems.</returns>
        //public ActionResult Ambiguities()
        //{
        //    MatchOptionsModel options = (MatchOptionsModel)Session["MatchOptions"];
        //    List<MatchItem> items = (List<MatchItem>)Session["MatchItems"];

        //    if (options.IsNull() || items.IsNull())
        //    {
        //        return RedirectToAction("Options");
        //    }

        //    MatchManager manager = new MatchManager();
        //    List<MatchItem> problematicItems = manager.GetMatchProblems(items);
        //    if (problematicItems.Count > 0)
        //    {
        //        ViewData["MatchItems"] = problematicItems;
        //        foreach (MatchItem item in problematicItems)
        //        {
        //            if (item.Status == MatchStatus.NeedsManualSelection)
        //            {
        //                ViewData[item.DropDownListIdentifier] = item.AlternativeTaxa;
        //            }
        //        }
        //        return View(options);
        //    }

        //    return RedirectToAction("Results");
        //}

        ///// <summary> 
        ///// POST: /Match/Ambiguities
        ///// Match all names or identifiers provided in a list.
        ///// </summary>
        ///// <returns>A view that communicate possible match problems.</returns>
        //[HttpPost]
        //public ActionResult Ambiguities(FormCollection form, string submitButton)
        //{
        //    MatchOptionsModel options = (MatchOptionsModel)Session["MatchOptions"];
        //    List<MatchItem> items = (List<MatchItem>)Session["MatchItems"];

        //    if (options.IsNull() || items.IsNull())
        //    {
        //        return RedirectToAction("Options");
        //    }

        //    foreach (MatchItem item in items)
        //    {
        //        if (item.Status == MatchStatus.NeedsManualSelection)
        //        {
        //            item.TaxonId = form[item.DropDownListIdentifier];
        //        }
        //    }

        //    Session["MatchItems"] = items;

        //    if (submitButton == "Save result as Excel")
        //    {
        //        return RedirectToAction("ResultExcelFile");
        //    }

        //    return RedirectToAction("Results");
        //}

        ///// <summary>
        ///// GET: /Match/Options
        ///// Get a view where match options can be provided.
        ///// </summary>
        ///// <param name="id">Taxon name, id or GUID.</param>
        ///// <returns>A view where match settings can be entered.</returns>
        //public ActionResult Options(string id)
        //{
        //    MatchOptionsModel model = null;
        //    model = new MatchOptionsModel(id);

        //    model.MatchInputType = MatchTaxonInputType.ExcelFile;
        //    model.OutputTaxonId = true;
        //    model.OutputScientificName = true;

        //    // Column content Dropbox
        //    var columnContent = from MatchColumnContentAlternative cC in Enum.GetValues(typeof(MatchColumnContentAlternative))
        //                        select new { value = (int)cC, text = cC.ToString() };
        //    ViewData["ColumnContentAlternative"] =
        //        new SelectList(columnContent, "value", "text", model.ColumnContentAlternative.ToString());

        //    // RowDelimiter Dropbox
        //    var rowDelimiters = from MatchTaxonRowDelimiter rd in Enum.GetValues(typeof(MatchTaxonRowDelimiter))
        //                        select new { value = (int)rd, text = rd.ToString() };
        //    ViewData["RowDelimiter"] =
        //        new SelectList(rowDelimiters, "value", "text", model.RowDelimiter.ToString());

        //    // ColumnDelimiter Dropbox
        //    var columnDelimiters = from MatchTaxonColumnDelimiter rd in Enum.GetValues(typeof(MatchTaxonColumnDelimiter))
        //                           select new { value = (int)rd, text = rd.ToString() };
        //    ViewData["ColumnDelimiter"] =
        //        new SelectList(columnDelimiters, "value", "text", model.ColumnDelimiter.ToString());

        //    // MatchTaxonToType Dropbox
        //    var matchToTypes = from MatchTaxonToType mt in Enum.GetValues(typeof(MatchTaxonToType))
        //                       select new { value = (int)mt, text = mt.ToString() };
        //    ViewData["MatchToType"] =
        //        new SelectList(matchToTypes, "value", "text", model.MatchToType.ToString());

        //    return View(model);
        //}

        ///// <summar>y
        ///// POST: /Match/Options
        ///// Get a view where match options can be provided.
        ///// </summary>
        ///// <param name="id">Taxon name, id or GUID.</param>
        ///// <returns>A view where match settings can be entered.</returns>
        //[HttpPost]
        //public ActionResult Options(MatchOptionsModel model)
        //{
        //    string fileName,
        //           filePath,
        //           tempDirectory = Resources.DyntaxaSettings.Default.TempDirectoryName; //"Temp"

        //    // Column content Dropbox
        //    var columnContent = from MatchColumnContentAlternative cC in Enum.GetValues(typeof(MatchColumnContentAlternative))
        //                        select new { value = (int)cC, text = cC.ToString() };
        //    ViewData["ColumnContentAlternative"] =
        //        new SelectList(columnContent, "value", "text", model.ColumnContentAlternative.ToString());

        //    // RowDelimiter Dropbox
        //    var rowDelimiters = from MatchTaxonRowDelimiter rd in Enum.GetValues(typeof(MatchTaxonRowDelimiter))
        //                        select new { value = (int)rd, text = rd.ToString() };
        //    ViewData["RowDelimiter"] =
        //        new SelectList(rowDelimiters, "value", "text", model.RowDelimiter.ToString());

        //    // ColumnDelimiter Dropbox
        //    var columnDelimiters = from MatchTaxonColumnDelimiter cd in Enum.GetValues(typeof(MatchTaxonColumnDelimiter))
        //                           select new { value = (int)cd, text = cd.ToString() };
        //    ViewData["ColumnDelimiter"] =
        //        new SelectList(columnDelimiters, "value", "text", model.ColumnDelimiter.ToString());

        //    // MatchTaxonToType Dropbox
        //    var matchToTypes = from MatchTaxonToType mt in Enum.GetValues(typeof(MatchTaxonToType))
        //                       select new { value = (int)mt, text = mt.ToString() };
        //    ViewData["MatchToType"] =
        //        new SelectList(matchToTypes, "value", "text", model.MatchToType.ToString());

        //    if (ModelState.IsValid)
        //    {
        //        MatchManager manager = new MatchManager();
        //        List<MatchItem> items = new List<MatchItem>();

        //        if (model.MatchToType == MatchTaxonToType.TaxonId)
        //        {
        //            model.LabelForProvidedText = Resources.DyntaxaResource.MatchOptionsOutputProvidedTaxonIdLabel;
        //        }
                
        //        if (model.MatchInputType == MatchTaxonInputType.ExcelFile)
        //        {

        //            foreach (string file in Request.Files)
        //            {
        //                var hpf = this.Request.Files[file];
        //                if (hpf.ContentLength == 0)
        //                {
        //                    continue;
        //                }

        //                // Generates random file name.
        //                string uploadedFileName = Path.GetFileName(hpf.FileName);
        //                string fileExtension = Path.GetExtension(hpf.FileName);

        //                //if (fileExtension == ".txt" || fileExtension == ".csv" || fileExtension == ".xls" || fileExtension == ".xlsx")
        //                if (fileExtension == ".xls" || fileExtension == ".xlsx")
        //                {
        //                    fileName = Path.GetRandomFileName();
        //                    fileName = Path.ChangeExtension(fileName, fileExtension);

        //                    filePath = Path.Combine(
        //                        AppDomain.CurrentDomain.BaseDirectory, tempDirectory);

        //                    filePath = Path.Combine(filePath, fileName);

        //                    // TODO: Unneccery?
        //                    ViewData["filename"] = fileName;

        //                    hpf.SaveAs(filePath);

        //                    model.FileName = filePath;
        //                    items = manager.GetMatches(filePath, model);
        //                    if (items.IsNotEmpty())
        //                    {
        //                        Session["MatchOptions"] = model;
        //                        Session["MatchItems"] = items;
        //                        return RedirectToAction("Ambiguities");
        //                    }
        //                    else
        //                    {
        //                        //TODO: Error. File has no valid content or is not a valid excel file.
        //                    }
        //                }
        //                else
        //                {
        //                    ViewData.ModelState.AddModelError("FileName", Resources.DyntaxaResource.MatchOptionsFileExtensionError);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            fileName = Path.GetRandomFileName();
        //            fileName = Path.ChangeExtension(fileName, ".xls");
        //            filePath = Path.Combine(
        //                AppDomain.CurrentDomain.BaseDirectory, tempDirectory);
        //            filePath = Path.Combine(filePath, fileName);
        //            model.FileName = filePath;

        //            if (model.ClipBoard.IsNotEmpty())
        //            {
        //                items = manager.GetMatchItemsFromText(model.ClipBoard, model);
        //                if (items.IsNotEmpty())
        //                {
        //                    Session["MatchOptions"] = model;
        //                    Session["MatchItems"] = items;
        //                    return RedirectToAction("Ambiguities");
        //                }
        //                else
        //                {
        //                    //TODO: Error. File has no valid content or is not a valid excel file.
        //                }
        //            }
        //            else
        //            {
        //                //TODO: Handle error when data is missing
        //            }
        //        }
        //    }

        //    return View(model);
        //}

        ///// <summary>
        ///// GET: /Match/Results/
        ///// List all matching results.
        ///// </summary>
        ///// <param name="items">List of match items.</param>
        ///// <param name="options">Match options.</param>
        ///// <returns>A view that desplay the match results.</returns>
        //public ActionResult Results()
        //{
        //    MatchOptionsModel options = (MatchOptionsModel)Session["MatchOptions"];
        //    List<MatchItem> items = (List<MatchItem>)Session["MatchItems"];

        //    if (options.IsNull() || items.IsNull())
        //    {
        //        return RedirectToAction("Options");
        //    }

        //    MatchManager manager = new MatchManager();
        //    items = manager.GetMatchResults(items, options);

        //    ViewData["MatchItems"] = items;
        //    Session["MatchItems"] = items;

        //    return View(options);
        //}

        ///// <summary>
        ///// POST: /Match/Results/
        ///// </summary>
        ///// <returns>Redirect to action that writes results into an Excel file.</returns>
        //[HttpPost]
        //public ActionResult Results(string submitButton)
        //{
        //    return RedirectToAction("ResultExcelFile");
        //}

        ///// <summary>
        ///// GET: /Match/ResultExcelFile
        ///// Write match results into an excel file and redirect to the file address.
        ///// </summary>
        ///// <returns>A Excel file.</returns>
        //public ActionResult ResultExcelFile()
        //{
        //    MatchOptionsModel options = (MatchOptionsModel)Session["MatchOptions"];
        //    List<MatchItem> items = (List<MatchItem>)Session["MatchItems"];

        //    if (options.IsNull() || items.IsNull())
        //    {
        //        return RedirectToAction("Options");
        //    }

        //    MatchManager manager = new MatchManager();
        //    items = manager.GetMatchResults(items, options);

        //    if (options.FileName != null)
        //    {
        //        ArtDatabanken.IO.ExcelFile file = new ExcelFile(manager.GetResultTable(items, options), options.FileName, true);
        //    }

        //    return Redirect(Resources.DyntaxaSettings.Default.PathToTempDirectory + Path.GetFileName(options.FileName)); //"~/Temp/"
        //}
    }
}

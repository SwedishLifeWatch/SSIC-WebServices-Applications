using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Xml;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebApplication.Dyntaxa;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Export;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Reference;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using Resources;

namespace Dyntaxa.Controllers
{
    public class SpeciesFactController : DyntaxaBaseController
    {
        ISpeciesFactModelManager SpeciesFactTestModelManager { get; set; }
        
          // Called "LIVE"
        public SpeciesFactController()
        {
        }

        // Called by test
        public SpeciesFactController(IUserDataSource userDataSourceRepository, ITaxonDataSource taxonDataSourceRepository, ISessionHelper session)
            : base(userDataSourceRepository, taxonDataSourceRepository, session)
        {
        }

        // Called by test
        public SpeciesFactController(IUserDataSource userDataSourceRepository, ITaxonDataSource taxonDataSourceRepository, ISpeciesFactModelManager speciesFactModelManager, ISessionHelper session)
            : base(userDataSourceRepository, taxonDataSourceRepository, session)
        {
            SpeciesFactTestModelManager = speciesFactModelManager;
        }

        /// <summary>
        /// Post action for adding a factor, default values should be saved for all field values.
        /// </summary>
        /// <param name="model"> Model returning required values from view.</param>
        /// <returns></returns>
        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.SpeciesFactEVAEditor)]
        public ActionResult AddFactor(SpeciesFactViewModel model)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;

            try
            {
                // Get selected taxon
                ITaxon taxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), model.TaxonId);
                IUserContext user = GetCurrentUser();
                if (ModelState.IsValid)
                {
                    try
                    {                        
                        SpeciesFactModelManager modelManager = new SpeciesFactModelManager(user);
                        modelManager.Taxon = taxon;
                        modelManager.AddFactorToTaxon(model.TaxonId, model.DropDownFactorId, model.ReferenceId, (int)model.MainParentFactorId);
                        return RedirectToAction("EditFactors", new { @taxonId = Convert.ToString(model.TaxonId), @factorId = Convert.ToString((int)model.MainParentFactorId), @dataType = Convert.ToString((int)model.DataType), @factorDataType = Convert.ToString((int)model.FactorDataType), @referenceId = Convert.ToString(model.ReferenceId) });
                    }
                    catch (Exception e)
                    {
                        DyntaxaLogger.WriteException(e);
                        errorMsg = Resources.DyntaxaResource.SharedNotPossibleToUpdateSpeciesFactError;
                        if (model.FactorList == null)
                        {
                            model.FactorList = new List<SpeciesFactDropDownModelHelper>();
                        }

                        ModelState.AddModelError(string.Empty, errorMsg);
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                    model.FactorList = new List<SpeciesFactDropDownModelHelper>();
                }
                
                //ViewData.Model = model;
              return RedirectToAction("EditFactors", new { @taxonId = Convert.ToString(model.TaxonId), @factorId = Convert.ToString((int)model.MainParentFactorId), @dataType = Convert.ToString((int)model.DataType), @factorDataType = Convert.ToString((int)model.FactorDataType), @referenceId = Convert.ToString(model.ReferenceId) });
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.SpeciesFactAddFactorHeaderText,
                Resources.DyntaxaResource.SpeciesFactAddFactorHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Post action for adding a factor, default values should be saved for all field values.
        /// </summary>
        /// <param name="model"> Model returning required values from view.</param>
        /// <returns></returns>
        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.SpeciesFactEVAEditor)]
        public ActionResult AddFromAllAvaliableFactor(SpeciesFactViewModel model)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;

            try
            {
                // Get selected taxon
                ITaxon taxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), model.TaxonId);
                IUserContext user = GetCurrentUser();
                if (ModelState.IsValid)
                {
                    try
                    {
                        SpeciesFactModelManager modelManager = new SpeciesFactModelManager(user);
                        modelManager.Taxon = taxon;
                        modelManager.AddFactorToTaxon(model.TaxonId, model.DropDownAllFactorId, model.ReferenceId, (int)model.MainParentFactorId);
                        return RedirectToAction("EditFactors", new { @taxonId = Convert.ToString(model.TaxonId), @factorId = Convert.ToString((int)model.MainParentFactorId), @dataType = Convert.ToString((int)model.DataType), @factorDataType = Convert.ToString((int)model.FactorDataType), @referenceId = Convert.ToString(model.ReferenceId) });
                    }
                    catch (Exception e)
                    {
                        DyntaxaLogger.WriteException(e);
                        errorMsg = Resources.DyntaxaResource.SharedNotPossibleToUpdateSpeciesFactError;
                        if (model.FactorList == null)
                        {
                            model.FactorList = new List<SpeciesFactDropDownModelHelper>();
                        }

                        ModelState.AddModelError(string.Empty, errorMsg);
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                    model.FactorList = new List<SpeciesFactDropDownModelHelper>();
                }

                //ViewData.Model = model;
                return RedirectToAction("EditFactors", new { @taxonId = Convert.ToString(model.TaxonId), @factorId = Convert.ToString((int)model.MainParentFactorId), @dataType = Convert.ToString((int)model.DataType), @factorDataType = Convert.ToString((int)model.FactorDataType), @referenceId = Convert.ToString(model.ReferenceId) });
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.SpeciesFactAddFactorHeaderText,
                Resources.DyntaxaResource.SpeciesFactAddFactorHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Post action for adding a taxon, default values should be saved for all field values.
        /// </summary>
        /// <param name="model"> Model returning required values from view.</param>
        /// <returns></returns>
        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.SpeciesFactEVAEditor)]
        public ActionResult AddHostTaxonAndFactor(SpeciesFactHostViewModel model)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;

            try
            {
                // Get selected taxon
                ITaxon taxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), model.TaxonId);
                IUserContext user = GetCurrentUser();
                if (ModelState.IsValid)
                {
                    try
                    {
                        SpeciesFactModelManager modelManager = new SpeciesFactModelManager(user);
                        modelManager.Taxon = taxon;
                        
                        // First check how what to add 
                        // No factors or taxa is selcetd in list, we have to create a new factor with host.
                        if (model.DropDownHostFactorId != 0 && model.DropDownTaxonId != SpeciesFactModelManager.SpeciesFactNoValueSet
                            && SpeciesFactHostTaxonIdList.IsNull() && SpeciesFactFactorIdList.IsNull())
                        {
                            modelManager.AddFactorToTaxon(model.TaxonId, model.DropDownHostFactorId, model.ReferenceId, (int)model.MainParentFactorId, true, model.DropDownTaxonId, model.IndividualCategoryId);
                        }

                        // Factor and taxa are selcetd in list, we have to create a new factor with host and
                        // and add selected factor to selected hosts in list and add selcted taxon to factors selected
                        // in list.
                        else if (model.DropDownHostFactorId != 0 && model.DropDownTaxonId != SpeciesFactModelManager.SpeciesFactNoValueSet
                            && SpeciesFactHostTaxonIdList.IsNotNull() && SpeciesFactFactorIdList.IsNotNull())
                        {
                            // First add newly selected taxon and factor
                            modelManager.AddFactorToTaxon(model.TaxonId, model.DropDownHostFactorId, model.ReferenceId, (int)model.MainParentFactorId, true, model.DropDownTaxonId, model.IndividualCategoryId);
                            
                            // Add factor with selected host
                            foreach (var host in SpeciesFactHostTaxonIdList)
                            {
                                modelManager.AddFactorToTaxon(model.TaxonId, model.DropDownHostFactorId, model.ReferenceId, (int)model.MainParentFactorId, true, host.Id, host.CategoryId);
                            }

                            // Add host for selected factors
                            foreach (var factor in SpeciesFactFactorIdList)
                            {
                                modelManager.AddFactorToTaxon(model.TaxonId, factor.Id, model.ReferenceId, (int)model.MainParentFactorId, true, model.DropDownTaxonId, factor.CategoryId);
                            }
                        }

                        // We have to create a new factor and add selected factor to selected hosts in list.
                        else if (model.DropDownHostFactorId != 0 && model.DropDownTaxonId == SpeciesFactModelManager.SpeciesFactNoValueSet
                            && SpeciesFactHostTaxonIdList.IsNotNull())
                        {
                            foreach (var host in SpeciesFactHostTaxonIdList)
                            {
                                modelManager.AddFactorToTaxon(model.TaxonId, model.DropDownHostFactorId, model.ReferenceId, (int)model.MainParentFactorId, true, host.Id, host.CategoryId);
                            }
                        }

                        // We have to create a new taxon and add selected taxon to selected factors in list.
                        else if (model.DropDownHostFactorId == 0 && model.DropDownTaxonId != SpeciesFactModelManager.SpeciesFactNoValueSet
                                    && SpeciesFactFactorIdList.IsNotNull())
                        {
                            foreach (var factor in SpeciesFactFactorIdList)
                            {
                                modelManager.AddFactorToTaxon(model.TaxonId, factor.Id, model.ReferenceId, (int)model.MainParentFactorId, true, model.DropDownTaxonId, factor.CategoryId);
                            }
                        }

                        // Reset selection
                        SpeciesFactFactorIdList = null;
                        SpeciesFactHostTaxonIdList = null;
                        return RedirectToAction("EditHostFactorsForSubstrate", new { @taxonId = Convert.ToString(model.TaxonId), @referenceId = Convert.ToString(model.ReferenceId),  @individualCategory= model.IndividualCategoryId });
                    }
                    catch (Exception e)
                    {
                        DyntaxaLogger.WriteException(e);
                        errorMsg = Resources.DyntaxaResource.SharedNotPossibleToUpdateSpeciesFactError;
                        if (model.SpeciesFactViewModel.FactorList == null)
                        {
                            model.SpeciesFactViewModel.FactorList = new List<SpeciesFactDropDownModelHelper>();
                        }

                        ModelState.AddModelError(string.Empty, errorMsg);
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                    model.SpeciesFactViewModel.FactorList = new List<SpeciesFactDropDownModelHelper>();
                }

                //ViewData.Model = model;
                return RedirectToAction("EditHostFactorsForSubstrate", new { @taxonId = Convert.ToString(model.TaxonId), @referenceId = Convert.ToString(model.ReferenceId), @individualCategory= model.IndividualCategoryId });
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.SpeciesFactAddFactorHeaderText,
                Resources.DyntaxaResource.SpeciesFactAddFactorHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Post action for removing a host taxon.
        /// </summary>
        /// <param name="model"> Model returning required values from view.</param>
        /// <returns></returns>
        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.SpeciesFactEVAEditor)]
        public ActionResult DeleteHostTaxonItem(int hostTaxonId, int factorId, int taxonId, int referenceId, int categoryId, int hostFactorId)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;

            try
            {
                // Get selected taxon
                ITaxon taxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), taxonId);
                IUserContext user = GetCurrentUser();
                if (ModelState.IsValid)
                {
                    try
                    {
                        SpeciesFactModelManager modelManager = new SpeciesFactModelManager(taxon, user);
                        modelManager.Taxon = taxon;

                        IList<DyntaxaSpeciesFact> speciesFacts = modelManager.GetSpeciesFactFromHost(hostTaxonId, categoryId, hostFactorId);
                        // Have to select which species fact to remove based on host.
                        if (speciesFacts.Count > 1)
                        {
                            var speciesFactsTemp = speciesFacts.Where(x => x.HostId != hostTaxonId).ToList();
                            foreach (var temp in speciesFactsTemp)
                            {
                                speciesFacts.Remove(temp);
                            }
                           
                        }
                        if (speciesFacts.Count == 1)
                        { 
                            // IList<SpeciesFactFieldValueModelHelper> newValuesInList = new List<SpeciesFactFieldValueModelHelper>();

                            DyntaxaSpeciesFact speciesFact = speciesFacts.FirstOrDefault();
                            SpeciesFactFieldValueModelHelper fieldValue = new SpeciesFactFieldValueModelHelper();

                            fieldValue.FactorField1Value = 0;
                            fieldValue.FactorField2Value = speciesFact.FactorEnumValue2;
                            fieldValue.FactorId = factorId;
                            fieldValue.HostId = hostTaxonId;
                            fieldValue.IndividualCategoryId = speciesFact.IndividualCatgory.Id;
                            fieldValue.ReferenceId = referenceId;
                            fieldValue.QualityId = speciesFact.Quality.QualityId;
                            fieldValue.FactorField1Type = SpeciesFactFieldType.ENUM;
                            fieldValue.FactorFieldType2 = SpeciesFactFieldType.ENUM;
                            fieldValue.StringValue5 = speciesFact.Comments;
                            fieldValue.MainParentFactorId = (int)DyntaxaFactorId.SUBSTRATE;
                            
                            // newValuesInList.Add(fieldValue);
                            modelManager.Taxon = taxon;
                            
                            //modelManager.UpdatedSpeciecFacts(newValuesInList, false);

                            modelManager.UpdatedSpeciecFacts(fieldValue, speciesFacts);
                            
                            // Remove from list if selected
                            if (SpeciesFactHostTaxonIdList.IsNotEmpty())
                            {
                                foreach (SpeciesFactHostsIdListHelper selectedHosts in SpeciesFactHostTaxonIdList)
                                {
                                    if (selectedHosts.Id == hostTaxonId)
                                    {
                                        SpeciesFactFactorIdList.Remove(selectedHosts);
                                    }
                                }
                            }

                            return RedirectToAction("EditHostFactorsForSubstrate", new { @taxonId = taxon.Id, @referenceId = referenceId,  @individualCategory= categoryId });
                        }
                        else
                        {
                            // Ops! Not possible to login to Service
                            errorMsg = Resources.DyntaxaResource.SharedNotPossibleToUpdateSpeciesFactError;
                            ModelState.AddModelError(string.Empty, errorMsg); 
                        }
                    }
                    catch (Exception)
                    {
                        errorMsg = Resources.DyntaxaResource.SharedNotPossibleToUpdateSpeciesFactError;
                           
                        ModelState.AddModelError(string.Empty, errorMsg);
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                }

                //ViewData.Model = model;
                return RedirectToAction("EditHostFactorsForSubstrate", new { @taxonId = taxon.Id, @referenceId = referenceId, @individualCategory = categoryId });
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.SpeciesFactAddFactorHeaderText,
                Resources.DyntaxaResource.SpeciesFactAddFactorHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }
        
        /// <summary>
        /// Post action for adding a taxon, default values should be saved for all field values.
        /// </summary>
        /// <param name="model"> Model returning required values from view.</param>
        /// <returns></returns>
        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.SpeciesFactEVAEditor)]
        public ActionResult DeleteHostFactorItem(int hostFactorId, int taxonId, int factorId, int referenceId, int categoryId)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;

            try
            {
                // Get selected taxon
                ITaxon taxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), taxonId);
                IUserContext user = GetCurrentUser();
                if (ModelState.IsValid)
                {
                    try
                    {
                        SpeciesFactModelManager modelManager = new SpeciesFactModelManager(taxon, user);
                        IList<int> categoryIds = new List<int>();
                        categoryIds.Add(categoryId);
                        IList<DyntaxaSpeciesFact> speciesFacts = modelManager.GetSpeciesFactFromFactor(taxonId, hostFactorId, categoryIds, null);
                        if (speciesFacts.IsNotNull() && speciesFacts.Count > 0)
                        {
                            foreach (DyntaxaSpeciesFact speciesFact in speciesFacts)
                            {
                                SpeciesFactFieldValueModelHelper fieldValue = new SpeciesFactFieldValueModelHelper();

                                fieldValue.FactorField1Value = 0;
                                fieldValue.FactorField2Value = speciesFact.FactorEnumValue2;
                                fieldValue.FactorId = hostFactorId;
                                // Always have a host since it is a host...
                                fieldValue.HostId = speciesFact.HostId;
                                fieldValue.IndividualCategoryId = speciesFact.IndividualCatgory.Id;
                                fieldValue.ReferenceId = referenceId;
                                fieldValue.QualityId = speciesFact.Quality.QualityId;
                                fieldValue.FactorField1Type = SpeciesFactFieldType.ENUM;
                                fieldValue.FactorFieldType2 = SpeciesFactFieldType.ENUM;
                                fieldValue.StringValue5 = speciesFact.Comments;
                                fieldValue.MainParentFactorId = (int)DyntaxaFactorId.SUBSTRATE;
                                IList<DyntaxaSpeciesFact> dyntaxaSpeciesFacts = new List<DyntaxaSpeciesFact>();
                                dyntaxaSpeciesFacts.Add(speciesFact);
                                modelManager.UpdatedSpeciecFacts(fieldValue, dyntaxaSpeciesFacts);
                            }

                            modelManager.Taxon = taxon;
                            // modelManager.UpdatedSpeciecFacts(newValuesInList, false);
                            // Remove from list if selected
                            SpeciesFactFactorIdList = null;
                            SpeciesFactHostTaxonIdList = null;
                        }
                        return RedirectToAction("EditHostFactorsForSubstrate", new { @taxonId = taxonId, @referenceId = referenceId, @individualCategory = categoryId });
                    }
                    catch (Exception e)
                    {
                        DyntaxaLogger.WriteException(e);
                        errorMsg = Resources.DyntaxaResource.SharedNotPossibleToUpdateSpeciesFactError;
                        ModelState.AddModelError(string.Empty, errorMsg);
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                }

                //ViewData.Model = model;
                return RedirectToAction("EditHostFactorsForSubstrate", new { @taxonId = taxonId, @referenceId = referenceId, @individualCategory = categoryId });
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.SpeciesFactAddFactorHeaderText,
                Resources.DyntaxaResource.SpeciesFactAddFactorHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }
        
        /// <summary>
        /// Getter for creation of the speciesfact list
        /// </summary>
        /// <param name="taxonId"></param>
        /// <returns></returns>
        [HttpGet]
        [DyntaxaAuthorize(Order = RequiredAuthorization.Authenticated)]
        public ActionResult SpeciesFactList(string taxonId)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;

            try
            {
                TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
                if (searchResult.NumberOfMatches != 1)
                {
                    return RedirectToSearch(taxonId);
                }
                // Set search out taxon to be the used one
                ITaxon taxon = searchResult.Taxon;
                this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
                ViewBag.Taxon = taxon;

                // get user and check if user has authority to show non public data...
                bool showNonPublicData = false;
                IUserContext user = GetCurrentUser();

                // Check if non public factor is to be shown
                showNonPublicData = AuthorizationManager.HasSpeciesFactAuthority(user);

                // Create View Model
                SpeciesFactViewModel model = new SpeciesFactViewModel();
                model.TaxonId = taxon.Id;
                model.PostAction = "SpeciesFactList";
                IUserContext loggedInUser = GetCurrentUser();
                if (loggedInUser.IsNotNull())
                {
                    // Gets the manager that handels the export of SpeciesFact
                    SpeciesFactDyntaxaListManager manager = new SpeciesFactDyntaxaListManager();

                    SpeciesFactModelManager modelManager = new SpeciesFactModelManager(user, taxon, true);
                  
                    // Here we get all factors that we needs
                    DyntaxaAllFactorData dyntaxaFactors = modelManager.GetAllSpeciesFact();
                       
                    // Create the view with all factors shown for the taxon
                    model = manager.CreateSpeciesFactViewData(user, taxon, dyntaxaFactors, showNonPublicData, model, true);
                 }
                else
                {
                    errorMsg = Resources.DyntaxaResource.SharedInvalidUserContext;
                }

                if (!errorMsg.Equals(string.Empty))
                {
                    ModelState.AddModelError(string.Empty, errorMsg);
                }

                // Remove taxonid from model state so that a string can be set in the URL and not only and id...
                // (bug in Rounte handling for this application) 
                ModelState.Remove("TaxonId");
                return View("SpeciesFactList", model);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.SpeciesFactExportActionHeaderText,
                Resources.DyntaxaResource.SpeciesFactExportActionHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Post action for species fact. ie creates an excel file 
        /// holding all factor for a taxon.
        /// </summary>
        /// <param name="model"> Model returing required values</param>
        /// <param name="downloadTokenValue"></param>
        /// <returns></returns>
        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.SpeciesFactEVAEditor)]
        public ActionResult SpeciesFactList(SpeciesFactViewModel model, string downloadTokenValue)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;

            try
            {
                // Get taxon to get all factors for
                ITaxon taxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), model.TaxonId);
                
                // get user and check if user has authority to show non public data...
                bool showNonPublicData = false;
                IUserContext user = GetCurrentUser();

                // Check if non public factor is to be shown
                showNonPublicData = AuthorizationManager.HasSpeciesFactAuthority(user);
               
                if (ModelState.IsValid)
                {
                    // Gets the manager that handels the export of SpeciesFact
                    SpeciesFactDyntaxaListManager manager = new SpeciesFactDyntaxaListManager();
                    
                    SpeciesFactModelManager modelManager = new SpeciesFactModelManager(user, taxon, true);
                    
                    // Here we get all factors that we needs
                    DyntaxaAllFactorData dyntaxaFactors = modelManager.GetAllSpeciesFact();
                    
                    // Use Excel openXml default format
                    ExcelFileFormat fileFormat = ExcelFileFormat.OpenXml;
                      
                    // Noew we create the excelfile
                    MemoryStream excelFileStream = manager.CreateSpeciesFactExcelFile(user, taxon, dyntaxaFactors, fileFormat, showNonPublicData);
                    var fileStreamResult = new FileStreamResult(excelFileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    var fileDownloadName = GetValidFileName(taxon.ScientificName) + ExcelFileFormatHelper.GetExtension(fileFormat);
                       
                    fileStreamResult.FileDownloadName = fileDownloadName;
                    Response.AppendCookie(new HttpCookie("fileDownloadToken", downloadTokenValue));

                    return fileStreamResult;
                }
                else
                {
                    ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                }
                // Reload speciec fact data...
                
                return View(model);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.SpeciesFactExportActionHeaderText,
                Resources.DyntaxaResource.SpeciesFactExportActionHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Get action for edit a factor, new values should be saved.
        /// </summary>
        /// <param name="taxonId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditFactorsFromEva(string taxonId, string factorId, string dataType, string factorDataType, string referenceId, string credentials)
        {
            String userName = "";
            String password = "";
            String evaCredentials = "";

            IUserContext loggedInUser = GetCurrentUser();
            if (loggedInUser.IsNull() || loggedInUser.User.Type == UserType.Application)
            {
                if (credentials.IsNotEmpty())
                {
                    /*
                    try
                    {
                        evaCredentials = ArtDatabanken.WebApplication.Dyntaxa.Data.Account.EvaCredentialsManager.Decrypt(credentials, "RKodS7FLbmumEy2t78ULT8e6M%Aj");
                        evaCredentials = credentials;
                    }
                    catch { }
                    */
                    System.Text.UnicodeEncoding encoding = new System.Text.UnicodeEncoding();
                    Byte[] bytes = System.Web.HttpServerUtility.UrlTokenDecode(credentials);
                    evaCredentials = encoding.GetString(bytes);

                    String[] creds = evaCredentials.Split(' ');
                    if (creds.Count() == 2)
                    {
                        userName = creds[0];
                        password = creds[1];
                        loggedInUser = CoreData.UserManager.Login(userName, password, ApplicationIdentifier.EVA.ToString());

                        if (loggedInUser.IsNotNull())
                        {
                            Session["userContext"] = loggedInUser;

                            /* globalization with cookie */
                            var cookie = Request.Cookies["CultureInfo"];

                            if (cookie == null)
                            {
                                var locale = CoreData.LocaleManager.GetUsedLocales(GetCurrentUser()).Get(loggedInUser.Locale.ISOCode);
                                SetLanguage(locale.ISOCode);
                            }
                            else
                            {
                                if (cookie.Value != null)
                                {
                                    var locale = CoreData.LocaleManager.GetUsedLocales(GetCurrentUser()).Get(cookie.Value);
                                    loggedInUser.Locale = locale;
                                }
                            }
                        }
                    }
                }

                return RedirectToAction("EditFactors", new { @taxonId = taxonId, @factorId = factorId, @dataType = dataType, @factorDataType = factorDataType, @referenceId = referenceId });
            }
            else
            {
                return RedirectToAction("EditFactors", new { @taxonId = taxonId, @factorId = factorId, @dataType = dataType, @factorDataType = factorDataType, @referenceId = referenceId });
            }
        }

        /// <summary>
        /// Get action for edit a factor, new values should be saved.
        /// </summary>
        /// <param name="taxonId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditHostSubstratesFromEva(string taxonId, string referenceId, string credentials)
        {
            String userName = "";
            String password = "";
            String evaCredentials = "";

            IUserContext loggedInUser = GetCurrentUser();
            if (loggedInUser.IsNull() || loggedInUser.User.Type == UserType.Application)
            {
                if (credentials.IsNotEmpty())
                {
                    /*
                    try
                    {
                        evaCredentials = ArtDatabanken.WebApplication.Dyntaxa.Data.Account.EvaCredentialsManager.Decrypt(credentials, "RKodS7FLbmumEy2t78ULT8e6M%Aj");
                        evaCredentials = credentials;
                    }
                    catch { }
                    */
                    System.Text.UnicodeEncoding encoding = new System.Text.UnicodeEncoding();
                    Byte[] bytes = System.Web.HttpServerUtility.UrlTokenDecode(credentials);
                    evaCredentials = encoding.GetString(bytes);

                    String[] creds = evaCredentials.Split(' ');
                    if (creds.Count() == 2)
                    {
                        userName = creds[0];
                        password = creds[1];
                        loggedInUser = CoreData.UserManager.Login(userName, password, ApplicationIdentifier.EVA.ToString());

                        if (loggedInUser.IsNotNull())
                        {
                            Session["userContext"] = loggedInUser;

                            /* globalization with cookie */
                            var cookie = Request.Cookies["CultureInfo"];

                            if (cookie == null)
                            {
                                var locale = CoreData.LocaleManager.GetUsedLocales(GetCurrentUser()).Get(loggedInUser.Locale.ISOCode);
                                SetLanguage(locale.ISOCode);
                            }
                            else
                            {
                                if (cookie.Value != null)
                                {
                                    var locale = CoreData.LocaleManager.GetUsedLocales(GetCurrentUser()).Get(cookie.Value);
                                    loggedInUser.Locale = locale;
                                }
                            }
                        }
                    }
                }

                return RedirectToAction("EditHostFactorsForSubstrate", new { @taxonId = taxonId, @referenceId = referenceId });
            }
            else
            {
                return RedirectToAction("EditHostFactorsForSubstrate", new { @taxonId = taxonId, @referenceId = referenceId });
            }
        }

        /// <summary>
        /// Get action for edit factors, ie setting new factor values for exsisting factors and
        /// adding new factors to selectetd datatype.
        /// </summary>
        /// <param name="taxonId"></param>
        /// <returns></returns>
        [HttpGet]
        [DyntaxaAuthorize(Order = RequiredAuthorization.Authenticated)]
        public ActionResult EditFactors(string taxonId, string factorId, string dataType, string factorDataType, string referenceId)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            Dictionary<int, DyntaxaFactor> dicDyntaxaFactors = null;

            try
            {
                TaxonSearchResult searchResult = TaxonSearchManager.GetTaxon(taxonId);
                if (searchResult.NumberOfMatches != 1)
                {
                    return RedirectToSearch(taxonId);
                }

                // Set search out taxon to be the used one
                ITaxon taxon = searchResult.Taxon;
                this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
                ViewBag.Taxon = taxon;

                // Get user and check if user has authority to show non public data...
                bool showNonPublicData = false;
                IUserContext loggedInUser = GetCurrentUser();
                showNonPublicData = AuthorizationManager.HasSpeciesFactAuthority(loggedInUser);
 
                // Create View Model
                SpeciesFactViewModel model = new SpeciesFactViewModel();
                model.TaxonId = taxon.Id;
                model.ReferenceId = referenceId;
                model.PostAction = "EditFactors";
                
                if (loggedInUser.IsNotNull())
                {
                    // Gets the manager that handels the export of SpeciesFact
                    SpeciesFactDyntaxaListManager manager = new SpeciesFactDyntaxaListManager();

                    SpeciesFactModelManager modelManager = new SpeciesFactModelManager(loggedInUser, taxon, Convert.ToInt32(factorId), Convert.ToInt32(dataType), Convert.ToInt32(factorDataType));
                    
                    // Set some model values
                    model.MainParentFactorId = modelManager.FactorEnumId;
                    model.DataType = modelManager.DataType;
                    model.FactorDataType = modelManager.FactorDataType;
                    bool okToViewFactor = false;
                    // Here we check if input data is ok for viewing data...
                    if (model.MainParentFactorId != DyntaxaFactorId.NOT_SUPPORTED && model.DataType != DyntaxaDataType.NOT_SUPPORTED && model.FactorDataType != DyntaxaFactorDataType.NOT_SUPPORTED)
                    {
                        // Here we check if kombination is allowed for viewing data...Since we both use EnumValues and Boolean....(For the moment DyntaxaFactorDataType.AF_LIFEFORM
                        // is handeled in Eva.
                        if (model.DataType == DyntaxaDataType.ENUM && model.FactorDataType != DyntaxaFactorDataType.AF_LIFEFORM)
                        {
                            okToViewFactor = true;
                        }
                        
                        // Here we check if kombination is allowed for viewing data...Since we bothe urs EnumValues and Boolean....
                        else if (model.DataType == DyntaxaDataType.BOOLEAN &&
                                    model.FactorDataType == DyntaxaFactorDataType.AF_LIFEFORM)
                        {
                            okToViewFactor = true;
                        }
                    }
                       
                    // Here we get all factors, speciesFacts, hosts, individual categories etc that belongs to this taxon
                    DyntaxaAllFactorData dyntaxaFactors = modelManager.GetFactorsFromTaxonAndParentFactor();
                    dicDyntaxaFactors = new Dictionary<int, DyntaxaFactor>();
                    foreach (DyntaxaFactor item in dyntaxaFactors.DyntaxaAllFactors)
                    {                     
                        if (!dicDyntaxaFactors.ContainsKey(item.FactorId))
                        {
                            dicDyntaxaFactors.Add(item.FactorId, item);
                        }
                    }

                    // IList<SpeciesFactViewModelItem> headerItems= new List<SpeciesFactViewModelItem>();
                    if (dyntaxaFactors.IsNotNull() && okToViewFactor)
                    {
                        IList<DyntaxaFactor> allShortListFactors = modelManager.GetFactorsFromFactorIdAndFactorDataType(factorDataType);
                        IList<DyntaxaFactor> allShortListFactorsAndHostfactors = new List<DyntaxaFactor>();
                        if (Convert.ToInt32(factorDataType) == (int)DyntaxaFactorDataType.AF_SUBSTRATE)
                        {
                            // We have to ger host factors too since they are shown in the menu
                            IList<DyntaxaFactor> hostSubstarteFactors = modelManager.GetFactorsFromFactorIdAndFactorDataTypeSubstrate(factorDataType);
                            allShortListFactorsAndHostfactors = allShortListFactors.Concat(hostSubstarteFactors).ToList();
                        }
                        else
                        {
                            allShortListFactorsAndHostfactors = allShortListFactors;
                        }

                        // Create the view with all factors shown for selcetd taxon
                        model = manager.CreateSpeciesFactViewData(loggedInUser, taxon, dyntaxaFactors, showNonPublicData, model, false);
                        foreach (SpeciesFactViewModelHeaderItem headerItem in model.SpeciesFactViewModelHeaderItemList)
                        {
                            foreach (SpeciesFactViewModelSubHeaderItem superiorHeaderItem in headerItem.SpeciecFactViewModelSubHeaderItemList)
                            {
                                IList<SpeciesFactViewModelItem> itemsToBeRemoved = new List<SpeciesFactViewModelItem>();
                            
                                //foreach (SpeciesFactViewModelItem speciesFactViewModelItem in superiorHeaderItem.SpeciesFactViewModelItemList)
                                for (int i = 0; i < superiorHeaderItem.SpeciesFactViewModelItemList.Count; i++)
                                {
                                    if (superiorHeaderItem.SpeciesFactViewModelItemList[i].FactorName.Equals(string.Empty))
                                    {
                                        superiorHeaderItem.SpeciesFactViewModelItemList[i].FactorName = "_";
                                    }

                                    superiorHeaderItem.SpeciesFactViewModelItemList[i].IsShortList = false;
                                    foreach (DyntaxaFactor dyntaxaFactor in allShortListFactorsAndHostfactors)
                                    {
                                        if (superiorHeaderItem.SpeciesFactViewModelItemList[i].FactorId == dyntaxaFactor.Id)
                                        {
                                            // Yes it is in ShortList
                                            superiorHeaderItem.SpeciesFactViewModelItemList[i].IsShortList = true;
                                            break;
                                        }
                                    }
                                    
                                    // Must check item befor if it is a header
                                    if (i > 0)
                                    {
                                        if (superiorHeaderItem.SpeciesFactViewModelItemList[i - 1].IsSuperiorHeader &&
                                            superiorHeaderItem.SpeciesFactViewModelItemList[i - 1].SuperiorHeader.Equals(superiorHeaderItem.SpeciesFactViewModelItemList[i].FactorName))
                                        {
                                            itemsToBeRemoved.Add(superiorHeaderItem.SpeciesFactViewModelItemList[i - 1]);
                                            superiorHeaderItem.SpeciesFactViewModelItemList[i].IsHeader = true;
                                            superiorHeaderItem.SpeciesFactViewModelItemList[i].SuperiorHeader = superiorHeaderItem.SpeciesFactViewModelItemList[i].FactorName;
                                        }
                                        else if (superiorHeaderItem.SpeciesFactViewModelItemList[i - 1].IsHeader && superiorHeaderItem.SpeciesFactViewModelItemList[i - 1].FactorId == superiorHeaderItem.SpeciesFactViewModelItemList[i].FactorId
                                            && !superiorHeaderItem.SpeciesFactViewModelItemList[i].IsHost)
                                        {
                                            superiorHeaderItem.SpeciesFactViewModelItemList[i].IsHeader = true;
                                            superiorHeaderItem.SpeciesFactViewModelItemList[i].SuperiorHeader = superiorHeaderItem.SpeciesFactViewModelItemList[i].FactorName;
                                        }
                                    }
                                }

                                foreach (var speciesFactViewModelItem in itemsToBeRemoved)
                                {
                                    superiorHeaderItem.SpeciesFactViewModelItemList.Remove(speciesFactViewModelItem);
                                }
                            }
                        }

                        IList<DyntaxaFactor> allAvaiableFactors = modelManager.GetFactorsFromFactorId(Convert.ToInt32(factorId), Convert.ToInt32(factorDataType));
                        IList<SpeciesFactDropDownModelHelper> allFactors = new List<SpeciesFactDropDownModelHelper>();
                        allFactors.Add(new SpeciesFactDropDownModelHelper(0, Resources.DyntaxaResource.SpeciesFactEditFactorsDropDownDefaultLabel));

                        List<DyntaxaFactor> existingAllFactors = dyntaxaFactors.DyntaxaAllFactors as List<DyntaxaFactor>;
                        bool isHeaderEditable = false;
                        foreach (var factorAll in allAvaiableFactors)
                        {
                            if (factorAll.FactorUpdateMode.OkToUpdate)
                            {
                                isHeaderEditable = true;
                            }
                            else
                            {
                                isHeaderEditable = false;
                            }

                            bool alreadyExists = false; //existingFactors != null && existingFactors.Exists(item => item.Id == factor.Id && factor.IsLeaf);
                            if (existingAllFactors.IsNotNull())
                            {
                                if (existingAllFactors.Any(existingFactor => existingFactor.Id == factorAll.Id))
                                {
                                    alreadyExists = true;
                                }
                            }

                            if (!alreadyExists) // kolla subheadrarna och sätt även dessa som headers)
                            {
                                bool isSelectable = isHeaderEditable;
                                bool isSubHeader = isHeaderEditable;
                                allFactors.Add(new SpeciesFactDropDownModelHelper(Convert.ToInt32(factorAll.Id), factorAll.Label, isSelectable, isSubHeader));
                            }
                        }

                        model.AllAvaliableFactors = allFactors;

                        // Add all factors that exist for displaying the add factors dropbox.
                        model.FactorList = new List<SpeciesFactDropDownModelHelper>();
                        model.FactorList.Add(new SpeciesFactDropDownModelHelper(0, Resources.DyntaxaResource.SpeciesFactEditFactorsDropDownDefaultLabel));

                        List<DyntaxaFactor> existingFactors = dyntaxaFactors.DyntaxaAllFactors as List<DyntaxaFactor>;
                           
                        foreach (DyntaxaFactor factor in allShortListFactors)
                        {                          
                            if (factor.FactorUpdateMode.OkToUpdate)
                            {
                                isHeaderEditable = true;
                            }
                            else
                            {
                                isHeaderEditable = false;
                            }

                            bool alreadyExists = false; //existingFactors != null && existingFactors.Exists(item => item.Id == factor.Id && factor.IsLeaf);
                            if (existingFactors.IsNotNull())
                            {
                                if (existingFactors.Any(existingFactor => existingFactor.Id == factor.Id))
                                {
                                    alreadyExists = true;
                                }
                            }
                               
                            if (!alreadyExists) // kolla subheadrarna och sätt även dessa som headers)
                            {
                                bool isSelectable = isHeaderEditable;
                                bool isSubHeader = isHeaderEditable;
                                //bool isLeaf = factor.IsLeaf;
                                //if (!isLeaf && isSelectable)
                                //{
                                //    isSelectable = false;
                                //}
                                    
                                model.FactorList.Add(new SpeciesFactDropDownModelHelper(Convert.ToInt32(factor.Id), factor.Label, isSelectable, isSubHeader));
                            }
                        }
                    }
                    else
                    {
                        string tempMessage = Resources.DyntaxaResource.SpeciesFactEditFactorInvalidCombinationErrorMessage.Replace("[taxonId]", taxonId);
                        tempMessage = tempMessage.Replace("[factorId]", factorId);
                        tempMessage = tempMessage.Replace("[dataType]", dataType);
                        tempMessage = tempMessage.Replace("[factorDataType]", factorDataType);
                        errorMsg = tempMessage;        
                    }
                }
                else
                {
                    errorMsg = Resources.DyntaxaResource.SharedInvalidUserContext;
                }

                if (!errorMsg.Equals(string.Empty))
                {
                    ModelState.AddModelError(string.Empty, errorMsg);
                }

                // Remove taxonid from model state so that a string can be set in the URL and not only and id...
                // (bug in Rounte handling for this application) 
                ModelState.Remove("TaxonId");

                // Update IsOkToUpdate
                if (dicDyntaxaFactors != null)
                {
                    foreach (SpeciesFactViewModelHeaderItem headerItem in model.SpeciesFactViewModelHeaderItemList)
                    {
                        DyntaxaFactor dyntaxaFactor2;
                        int factorId2;                 
                    
                        foreach (SpeciesFactViewModelSubHeaderItem subHeaderItem in headerItem.SpeciecFactViewModelSubHeaderItemList)
                        {                        
                            foreach (SpeciesFactViewModelItem item in subHeaderItem.SpeciesFactViewModelItemList)
                            {                            
                                if (int.TryParse(item.FactorId, out factorId2))
                                {
                                    if (dicDyntaxaFactors.TryGetValue(factorId2, out dyntaxaFactor2))
                                    {
                                        item.IsOkToUpdate = dyntaxaFactor2.FactorUpdateMode.OkToUpdate;
                                    }
                                }
                            }
                        }
                    }
                }

                return View("EditFactors", model);
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.SpeciesFactEditFactorHeaderText,
                Resources.DyntaxaResource.SpeciesFactEditFactorHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Post action for edit  factors, new factor values are saved.
        /// </summary>
        /// <param name="model"> Model returing required values from view</param>
        /// <returns></returns>
        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.SpeciesFactEVAEditor)]
        public ActionResult EditFactors(SpeciesFactViewModel model)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;

            try
            {
                // Extract quality from all viewed factors
                List<string> allFactorEnumFieldValueKeys3 = Request.Form.AllKeys.Where(key => key.StartsWith("factorEnumFieldValue3_")).ToList();
                IList<SpeciesFactFieldValueModelHelper> newValuesInList3 = new List<SpeciesFactFieldValueModelHelper>();
                int factorId3 = 0;
                int selectedValue3 = 0;
                int hostId3 = 0;
                int individualCategoryId3 = 0;
                int referenceId3 = 0;
                int qualityId3 = 3;
                foreach (string key in allFactorEnumFieldValueKeys3)
                {
                    SpeciesFactFieldValueModelHelper fieldValue = new SpeciesFactFieldValueModelHelper();
                    selectedValue3 = int.Parse(Request.Form[key]);

                    String[] keyParts = key.Split("_".ToCharArray());
                    factorId3 = int.Parse(keyParts[1]);
                    hostId3 = int.Parse(keyParts[2]);
                    individualCategoryId3 = int.Parse(keyParts[3]);
                    referenceId3 = int.Parse(keyParts[4]);
                    qualityId3 = int.Parse(keyParts[5]);

                    fieldValue.FactorId = factorId3;
                    fieldValue.HostId = hostId3;
                    fieldValue.IndividualCategoryId = individualCategoryId3;
                    fieldValue.ReferenceId = referenceId3;
                    fieldValue.QualityId = selectedValue3;
                    
                    fieldValue.MainParentFactorId = (int)model.MainParentFactorId;
                    newValuesInList3.Add(fieldValue);
                }
                
                // Extract enumvalues from all viewed factors
                List<string> allFactorEnumFieldValueKeys2 = Request.Form.AllKeys.Where(key => key.StartsWith("factorEnumFieldValue2_")).ToList();
                IList<SpeciesFactFieldValueModelHelper> newValuesInList2 = new List<SpeciesFactFieldValueModelHelper>();
                int factorId2 = 0;
                int selectedValue2 = 0;
                int hostId2 = 0;
                int individualCategoryId2 = 0;
                int referenceId2 = 0;
                int qualityId2 = 3;
                foreach (string key in allFactorEnumFieldValueKeys2)
                {
                    SpeciesFactFieldValueModelHelper fieldValue = new SpeciesFactFieldValueModelHelper();
                    selectedValue2 = int.Parse(Request.Form[key]);

                    String[] keyParts = key.Split("_".ToCharArray());
                    factorId2 = int.Parse(keyParts[1]);
                    hostId2 = int.Parse(keyParts[2]);
                    individualCategoryId2 = int.Parse(keyParts[3]);
                    referenceId2 = int.Parse(keyParts[4]);
                    qualityId2 = int.Parse(keyParts[5]);

                    fieldValue.FactorField2Value = selectedValue2;
                    fieldValue.FactorId = factorId2;
                    fieldValue.HostId = hostId2;
                    fieldValue.IndividualCategoryId = individualCategoryId2;
                    fieldValue.ReferenceId = referenceId2;
                    fieldValue.QualityId = qualityId2;
                    fieldValue.FactorFieldType2 = SpeciesFactFieldType.ENUM;
                    fieldValue.MainParentFactorId = (int)model.MainParentFactorId;
                    fieldValue.FactorField2HasValue = true;
                    newValuesInList2.Add(fieldValue);
                }

                // Extract enumvalues from all viewed factors
                List<string> allFactorEnumFieldValueKeys = Request.Form.AllKeys.Where(key => key.StartsWith("factorEnumFieldValue_")).ToList();
                IList<SpeciesFactFieldValueModelHelper> newValuesInList = new List<SpeciesFactFieldValueModelHelper>();
                int factorId = 0;
                int selectedValue = 0;
                int hostId = 0;
                int individualCategoryId = 0;
                int referenceId = 0;
                int qualityId = 3;
                foreach (string key in allFactorEnumFieldValueKeys)
                {
                    SpeciesFactFieldValueModelHelper fieldValue = new SpeciesFactFieldValueModelHelper();
                    selectedValue = int.Parse(Request.Form[key]);
                    
                    String[] keyParts = key.Split("_".ToCharArray());
                    factorId = int.Parse(keyParts[1]);
                    hostId = int.Parse(keyParts[2]);
                    individualCategoryId = int.Parse(keyParts[3]);
                    referenceId = int.Parse(keyParts[4]);
                    qualityId = int.Parse(keyParts[5]);
       
                    fieldValue.FactorField1Value = selectedValue;
                    fieldValue.FactorId = factorId;
                    fieldValue.HostId = hostId;
                    fieldValue.IndividualCategoryId = individualCategoryId;
                    fieldValue.ReferenceId = Convert.ToInt32(model.ReferenceId);
                    fieldValue.QualityId = qualityId;
                    fieldValue.FactorField1Type = SpeciesFactFieldType.ENUM;
                    fieldValue.MainParentFactorId = (int)model.MainParentFactorId;
                    fieldValue.StringValue5 = string.Empty;
                    newValuesInList.Add(fieldValue);
                }

                // Match dropdown value 2 together with new value...
                foreach (SpeciesFactFieldValueModelHelper speciesFactFieldValueModelHelper in newValuesInList)
                {
                    foreach (SpeciesFactFieldValueModelHelper factFieldValueModelHelper in newValuesInList2)
                    {
                        if (factFieldValueModelHelper.FactorId == speciesFactFieldValueModelHelper.FactorId && factFieldValueModelHelper.HostId == speciesFactFieldValueModelHelper.HostId
                            && factFieldValueModelHelper.IndividualCategoryId == speciesFactFieldValueModelHelper.IndividualCategoryId)
                        {
                            speciesFactFieldValueModelHelper.FactorField2Value = factFieldValueModelHelper.FactorField2Value;
                            speciesFactFieldValueModelHelper.FactorFieldType2 = factFieldValueModelHelper.FactorFieldType2;
                            speciesFactFieldValueModelHelper.FactorField2HasValue = factFieldValueModelHelper.FactorField2HasValue;
                            break;
                        }
                    }

                    foreach (SpeciesFactFieldValueModelHelper factFieldValueModelHelper in newValuesInList3)
                    {
                        if (factFieldValueModelHelper.FactorId == speciesFactFieldValueModelHelper.FactorId && factFieldValueModelHelper.HostId == speciesFactFieldValueModelHelper.HostId
                            && factFieldValueModelHelper.IndividualCategoryId == speciesFactFieldValueModelHelper.IndividualCategoryId)
                        {
                            speciesFactFieldValueModelHelper.QualityId = factFieldValueModelHelper.QualityId;
                            break;
                        }
                    }
                }

                // Get selected taxon
                ITaxon taxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), model.TaxonId);
                IUserContext user = GetCurrentUser();
                if (ModelState.IsValid)
                {
                    try
                    {                            
                        SpeciesFactModelManager modelManager = null;
                        if (SpeciesFactTestModelManager.IsNotNull())
                        {
                            SpeciesFactTestModelManager.Taxon = taxon;
                            SpeciesFactTestModelManager.UpdatedSpeciecFacts(newValuesInList, false, false);
                        }
                        else
                        {
                            modelManager = new SpeciesFactModelManager(user);
                            modelManager.Taxon = taxon;
                            modelManager.UpdatedSpeciecFacts(newValuesInList, false, false);
                        }

                        return RedirectToAction("EditFactors", new { @taxonId = Convert.ToString(model.TaxonId), @factorId = Convert.ToString((int)model.MainParentFactorId), @dataType = Convert.ToString((int)model.DataType), @factorDataType = Convert.ToString((int)model.FactorDataType), @referenceId = Convert.ToString(model.ReferenceId) });
                    }
                    catch (Exception e)
                    {
                        DyntaxaLogger.WriteException(e);
                        errorMsg = Resources.DyntaxaResource.SharedNotPossibleToUpdateSpeciesFactError;
                        if (model.FactorList == null)
                        {
                            model.FactorList = new List<SpeciesFactDropDownModelHelper>();
                        }

                        ModelState.AddModelError(string.Empty, errorMsg);
                    }                   
                }
                else
                {
                    LogError(ModelState);
                    ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                    model.FactorList = new List<SpeciesFactDropDownModelHelper>();
                }
               
                ViewData.Model = model;
               
                return View("EditFactors", model);
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.SpeciesFactEditFactorHeaderText,
                Resources.DyntaxaResource.SpeciesFactEditFactorHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="modelStateDictionary">The model state dictionary.</param>
        private void LogError(ModelStateDictionary modelStateDictionary)
        {
            if (modelStateDictionary == null)
            {
                return;
            }

            if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.Request == null)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();            
            sb.AppendLine("Error in: " + System.Web.HttpContext.Current.Request.Url);
            foreach (ModelState modelState in modelStateDictionary.Values)
            {
                for (int i = 0; i < modelState.Errors.Count; i++)
                {
                    ModelError error = modelState.Errors[i];
                    sb.AppendLine("Error " + i);
                    if (!string.IsNullOrEmpty(error.ErrorMessage))
                    {
                        sb.AppendLine(error.ErrorMessage);
                    }

                    if (error.Exception != null)
                    {
                        if (error.Exception.StackTrace != null)
                        {
                            sb.AppendLine(error.Exception.StackTrace);
                        }
                    }
                }
            }

            DyntaxaLogger.WriteMessage(sb.ToString());
        }

        /// <summary>
        /// Post action for edit  factors, new factor values are saved.
        /// </summary>
        /// <param name="model"> Model returing required values from view</param>
        /// <returns></returns>
        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.SpeciesFactEVAEditor)]
        public ActionResult EditHostFactors(SpeciesFactViewModel model)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;

            try
            {
                // Extract quality from all viewed factors
                List<string> allFactorEnumFieldValueKeys3 = Request.Form.AllKeys.Where(key => key.StartsWith("factorEnumFieldValue3_")).ToList();
                IList<SpeciesFactFieldValueModelHelper> newValuesInList3 = new List<SpeciesFactFieldValueModelHelper>();
                int factorId3 = 0;
                int selectedValue3 = 0;
                int hostId3 = 0;
                int individualCategoryId3 = 0;
                int referenceId3 = 0;
                int qualityId3 = 3;
                foreach (string key in allFactorEnumFieldValueKeys3)
                {
                    SpeciesFactFieldValueModelHelper fieldValue = new SpeciesFactFieldValueModelHelper();
                    selectedValue3 = int.Parse(Request.Form[key]);

                    String[] keyParts = key.Split("_".ToCharArray());
                    factorId3 = int.Parse(keyParts[1]);
                    hostId3 = int.Parse(keyParts[2]);
                    individualCategoryId3 = int.Parse(keyParts[3]);
                    referenceId3 = int.Parse(keyParts[4]);
                    qualityId3 = int.Parse(keyParts[5]);

                    fieldValue.FactorId = factorId3;
                    fieldValue.HostId = hostId3;
                    fieldValue.IndividualCategoryId = individualCategoryId3;
                    fieldValue.ReferenceId = referenceId3;
                    fieldValue.QualityId = selectedValue3;

                    fieldValue.MainParentFactorId = (int)model.MainParentFactorId;
                    newValuesInList3.Add(fieldValue);
                }
                
                // Extract enumvalues from all viewed factors
                List<string> allFactorEnumFieldValueKeys2 = Request.Form.AllKeys.Where(key => key.StartsWith("factorEnumFieldValue2_")).ToList();
                IList<SpeciesFactFieldValueModelHelper> newValuesInList2 = new List<SpeciesFactFieldValueModelHelper>();
                int factorId2 = 0;
                int selectedValue2 = 0;
                int hostId2 = 0;
                int individualCategoryId2 = 0;
                int referenceId2 = 0;
                int qualityId2 = 3;
                foreach (string key in allFactorEnumFieldValueKeys2)
                {
                    SpeciesFactFieldValueModelHelper fieldValue = new SpeciesFactFieldValueModelHelper();
                    selectedValue2 = int.Parse(Request.Form[key]);

                    String[] keyParts = key.Split("_".ToCharArray());
                    factorId2 = int.Parse(keyParts[1]);
                    hostId2 = int.Parse(keyParts[2]);
                    individualCategoryId2 = int.Parse(keyParts[3]);
                    referenceId2 = int.Parse(keyParts[4]);
                    qualityId2 = int.Parse(keyParts[5]);

                    fieldValue.FactorField2Value = selectedValue2;
                    fieldValue.FactorId = factorId2;
                    fieldValue.HostId = hostId2;
                    fieldValue.IndividualCategoryId = individualCategoryId2;
                    fieldValue.ReferenceId = referenceId2;
                    fieldValue.QualityId = qualityId2;
                    fieldValue.FactorFieldType2 = SpeciesFactFieldType.ENUM;
                    fieldValue.MainParentFactorId = (int)model.MainParentFactorId;
                    fieldValue.FactorField2HasValue = true;
                    newValuesInList2.Add(fieldValue);
                }

                // Extract enumvalues from all viewed factors
                List<string> allFactorEnumFieldValueKeys = Request.Form.AllKeys.Where(key => key.StartsWith("factorEnumFieldValue_")).ToList();
                IList<SpeciesFactFieldValueModelHelper> newValuesInList = new List<SpeciesFactFieldValueModelHelper>();
                int factorId = 0;
                int selectedValue = 0;
                int hostId = 0;
                int individualCategoryId = 0;
                int referenceId = 0;
                int qualityId = 3;
                foreach (string key in allFactorEnumFieldValueKeys)
                {
                    SpeciesFactFieldValueModelHelper fieldValue = new SpeciesFactFieldValueModelHelper();
                    selectedValue = int.Parse(Request.Form[key]);

                    String[] keyParts = key.Split("_".ToCharArray());
                    factorId = int.Parse(keyParts[1]);
                    hostId = int.Parse(keyParts[2]);
                    individualCategoryId = int.Parse(keyParts[3]);
                    referenceId = int.Parse(keyParts[4]);
                    qualityId = int.Parse(keyParts[5]);

                    fieldValue.FactorField1Value = selectedValue;
                    fieldValue.FactorId = factorId;
                    fieldValue.HostId = hostId;
                    fieldValue.IndividualCategoryId = individualCategoryId;
                    fieldValue.ReferenceId = Convert.ToInt32(model.ReferenceId);
                    fieldValue.QualityId = qualityId;
                    fieldValue.FactorField1Type = SpeciesFactFieldType.ENUM;
                    fieldValue.MainParentFactorId = (int)model.MainParentFactorId;
                    fieldValue.StringValue5 = string.Empty;
                    newValuesInList.Add(fieldValue);
                }

                // Match dropdown value 2 together with new value...
                foreach (SpeciesFactFieldValueModelHelper speciesFactFieldValueModelHelper in newValuesInList)
                {
                    foreach (SpeciesFactFieldValueModelHelper factFieldValueModelHelper in newValuesInList2)
                    {
                        if (factFieldValueModelHelper.FactorId == speciesFactFieldValueModelHelper.FactorId && factFieldValueModelHelper.HostId == speciesFactFieldValueModelHelper.HostId
                            && factFieldValueModelHelper.IndividualCategoryId == speciesFactFieldValueModelHelper.IndividualCategoryId)
                        {
                            speciesFactFieldValueModelHelper.FactorField2Value = factFieldValueModelHelper.FactorField2Value;
                            speciesFactFieldValueModelHelper.FactorFieldType2 = factFieldValueModelHelper.FactorFieldType2;
                            speciesFactFieldValueModelHelper.FactorField2HasValue = factFieldValueModelHelper.FactorField2HasValue;
                            break;
                        }
                    }

                    foreach (SpeciesFactFieldValueModelHelper factFieldValueModelHelper in newValuesInList3)
                    {
                        if (factFieldValueModelHelper.FactorId == speciesFactFieldValueModelHelper.FactorId && factFieldValueModelHelper.HostId == speciesFactFieldValueModelHelper.HostId
                            && factFieldValueModelHelper.IndividualCategoryId == speciesFactFieldValueModelHelper.IndividualCategoryId)
                        {
                            speciesFactFieldValueModelHelper.QualityId = factFieldValueModelHelper.QualityId;
                            break;
                        }
                    }
                }

                // Get selected taxon
                ITaxon taxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), model.TaxonId);
                IUserContext user = GetCurrentUser();
                if (ModelState.IsValid)
                {                   
                    try
                    {
                        SpeciesFactModelManager modelManager = new SpeciesFactModelManager(user);
                        modelManager.Taxon = taxon;
                        modelManager.UpdatedSpeciecFacts(newValuesInList, false, false);
                        return RedirectToAction("EditHostFactorsForSubstrate", new { @taxonId = Convert.ToString(model.TaxonId), @referenceId = Convert.ToString(model.ReferenceId) });
                    }
                    catch (Exception e)
                    {
                        DyntaxaLogger.WriteException(e);
                        errorMsg = Resources.DyntaxaResource.SharedNotPossibleToUpdateSpeciesFactError;
                        if (model.FactorList == null)
                        {
                            model.FactorList = new List<SpeciesFactDropDownModelHelper>();
                        }

                        ModelState.AddModelError(string.Empty, errorMsg);
                    }                   
                }
                else
                {
                    ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                    model.FactorList = new List<SpeciesFactDropDownModelHelper>();
                }

                ViewData.Model = model;
                return RedirectToAction("EditHostFactorsForSubstrate", new { @taxonId = Convert.ToString(model.TaxonId), @referenceId = Convert.ToString(model.ReferenceId) });
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.SpeciesFactEditFactorHeaderText,
                Resources.DyntaxaResource.SpeciesFactEditFactorHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        //[HttpPost]
        //[DyntaxaAuthorize(Order = RequiredAuthorization.Authenticated)]
        //public JsonResult EditHostFactorsForSubstrateAddTaxon(int taxonId)
        //{

        //    return Json(true, JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        /// Get action for edit factors, ie setting new factor values for exsisting factors and
        /// adding new factors to selectetd datatype.
        /// </summary>
        /// <param name="taxonId"></param>
        /// <returns></returns>
        [HttpGet]
        [DyntaxaAuthorize(Order = RequiredAuthorization.Authenticated)]
        public ActionResult EditHostFactorsForSubstrate(string taxonId, string referenceId, bool? reset = false)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            int factorId = (int)DyntaxaFactorId.SUBSTRATE;
            int factorDataType = (int)DyntaxaFactorDataType.AF_SUBSTRATE;
            int dataType = (int)DyntaxaDataType.ENUM;

            try
            {
                TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
                if (searchResult.NumberOfMatches != 1)
                {
                    return RedirectToSearch(taxonId);
                }

                // Set search out taxon to be the used one
                ITaxon taxon = searchResult.Taxon;
                this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
                ViewBag.Taxon = taxon;

                // Get user and check if user has authority to show non public data...
                bool showNonPublicData = false;
                IUserContext loggedInUser = GetCurrentUser();
                showNonPublicData = AuthorizationManager.HasSpeciesFactAuthority(loggedInUser);

                // Create View Model
                SpeciesFactHostViewModel model = new SpeciesFactHostViewModel();
                model.MainParentFactorId = (int)DyntaxaFactorId.SUBSTRATE;
                SpeciesFactViewModel subModel = new SpeciesFactViewModel();
                model.TaxonId = taxon.Id;
                model.ReferenceId = referenceId;
                model.FactorId = (int)DyntaxaFactorId.SUBSTRATE;

                model.PostAction = "EditHostFactorsForSubstrate";

                if (loggedInUser.IsNotNull())
                {
                    // Gets the manager that handels the export of SpeciesFact
                    SpeciesFactDyntaxaListManager manager = new SpeciesFactDyntaxaListManager();

                    SpeciesFactModelManager modelManager = new SpeciesFactModelManager(loggedInUser, taxon, factorId, dataType, factorDataType);
                    DyntaxaIndividualCategory dyntaxaDefaultIndividualCategory = modelManager.GetIndividualCategory(0);
                    // Must set what individual category we use
                    //string categoryId = null;
                    //if (categoryId.IsNull())
                    //{
                    //    model.IndividualCategoryId = modelManager.GetIndividualCategory(0).Id;
                    //}
                    //else
                    //{
                    //    model.IndividualCategoryId = modelManager.GetIndividualCategory(Convert.ToInt32(categoryId)).Id;
                    //}
                    //int selectedIndividaualCategory = model.IndividualCategoryId;

                    // Here we get all factors, speciesFacts, hosts, individual categories etc that belongs to this taxon
                    DyntaxaAllFactorData dyntaxaFactors = modelManager.GetFactorsFromTaxonAndParentFactor(true);

                    model.IndividualCategoryList = new List<SpeciesFactDropDownModelHelper>();

                    foreach (DyntaxaIndividualCategory individualCategory in modelManager.GetAllIndividualCategories())
                    {
                        model.IndividualCategoryList.Add(new SpeciesFactDropDownModelHelper(individualCategory.Id, individualCategory.Label));
                    }
                    
                    // Get hosts
                    IList<SpeciesFactHostViewModelItem> hosts = new List<SpeciesFactHostViewModelItem>();
                    foreach (DyntaxaHost dyntaxaHost in dyntaxaFactors.CompleteHostList)
                    {
                        SpeciesFactHostViewModelItem item = new SpeciesFactHostViewModelItem();
                        item.TaxonId = Convert.ToInt32(dyntaxaHost.Id);
                        item.Name = dyntaxaHost.Label;
                        item.IsChecked = false;
                        item.Type = "taxon";
                        item.FactorId = dyntaxaHost.FactorId;
                        //item.CategoryId = dyntaxaHost.IndividualCategory;
                        //item.CategoryName = dyntaxaHost.IndividualCategoryName;
                        hosts.Add(item);
                    }
                    
                    // Get factors
                    IList<SpeciesFactHostViewModelItem> factors = new List<SpeciesFactHostViewModelItem>();
                    foreach (DyntaxaSpeciesFact speciesFact in dyntaxaFactors.DyntaxaAllSpeciesFactsForATaxon)
                    {
                        SpeciesFactHostViewModelItem item = new SpeciesFactHostViewModelItem();
                        item.FactorId = Convert.ToInt32(speciesFact.Factor.FactorId);
                        item.CategoryName = speciesFact.IndividualCatgory.Label;
                        item.CategoryId = speciesFact.IndividualCatgory.Id;
                        item.Name = speciesFact.Factor.Label;
                        item.IsChecked = false;
                        item.Type = "factor";
                        foreach (SpeciesFactHostViewModelItem hostItem in hosts)
                        {
                            // We found out host
                            if (hostItem.TaxonId == speciesFact.HostId && hostItem.FactorId == speciesFact.FactorIdForHosts && !hostItem.IsCategorySet)
                            {
                                hostItem.IsHostToFactor = speciesFact.Factor.Label;
                                hostItem.IsHostToFactorId = speciesFact.FactorIdForHosts;
                                hostItem.CategoryName = speciesFact.IndividualCatgory.Label;
                                hostItem.CategoryId = speciesFact.IndividualCatgory.Id;
                                hostItem.FactorId = speciesFact.FactorIdForHosts;
                                hostItem.IsCategorySet = true;
                                string name = hostItem.Name;
                                    
                                // Ta bort dubbletter
                                var exist = false;
                                foreach (SpeciesFactHostViewModelItem it in factors)
                                {
                                    if (it.FactorId == item.FactorId)
                                    {
                                        exist = true;
                                        break;
                                    }
                                }

                                if (!exist)
                                {
                                    factors.Add(item);
                                }

                                break;
                            }
                        }
                    }

                    // Remove data with incorrect category
                    IList<SpeciesFactHostViewModelItem> finalFactorList = new List<SpeciesFactHostViewModelItem>();
                    IList<SpeciesFactHostViewModelItem> finalHostTaxaList = new List<SpeciesFactHostViewModelItem>();
                    foreach (SpeciesFactHostViewModelItem speciesFactHostViewModelItem in factors)
                    {
                        //if(speciesFactHostViewModelItem.CategoryId == selectedIndividaualCategory)
                        //{
                            finalFactorList.Add(speciesFactHostViewModelItem);
                        //}
                    }

                    foreach (SpeciesFactHostViewModelItem speciesFactHostViewModelItem in hosts)
                    {
                        // if (speciesFactHostViewModelItem.CategoryId == selectedIndividaualCategory)
                        // {
                            finalHostTaxaList.Add(speciesFactHostViewModelItem);
                        //}
                    }

                    finalHostTaxaList = finalHostTaxaList.OrderBy(x => x.FactorId).ThenBy(x => x.CategoryId).ThenBy(x => x.TaxonId).ToList();
                    model.HostTaxonList = finalHostTaxaList;

                    finalFactorList = finalFactorList.OrderBy(x => x.FactorId).ToList();
                    model.HostFactorList = finalFactorList;

                    // "AddTaxonDropDown  För att söka ut taxon, här sätts listan upp
                    // ***************************************************
                    IList<SpeciesFactDropDownModelHelper> addTaxaToHostList = new List<SpeciesFactDropDownModelHelper>();
                    addTaxaToHostList.Add(new SpeciesFactDropDownModelHelper(-1000, "Välj taxon"));
                    IList<DyntaxaHost> taxonHosts = modelManager.GetHostByFactorId(factorId);
                    foreach (DyntaxaHost dyntaxaHost in taxonHosts)
                    {
                        addTaxaToHostList.Add(new SpeciesFactDropDownModelHelper(Convert.ToInt32(dyntaxaHost.Id), dyntaxaHost.Label));
                    }
                    model.AddTaxonToHostList = addTaxaToHostList;

                    //**********************************************************************************
                    IList<DyntaxaFactor> taxonomicFactors = new List<DyntaxaFactor>();
                    for (int i = 0; i < dyntaxaFactors.DyntaxaAllFactors.Count; i++)
                    {
                        if (dyntaxaFactors.DyntaxaAllFactors[i].IsTaxonomic && !dyntaxaFactors.DyntaxaAllFactors[i].IsLeaf)
                        {
                            taxonomicFactors.Add(dyntaxaFactors.DyntaxaAllFactors[i]);
                        }
                        else if (i > 0 && dyntaxaFactors.DyntaxaAllFactors[i].IsTaxonomic && dyntaxaFactors.DyntaxaAllFactors[i].IsLeaf)
                        {
                            // Must add header since we have a leaf without any header
                            if (taxonomicFactors.Count == 0 || (taxonomicFactors.Last().FactorId != dyntaxaFactors.DyntaxaAllFactors[i - 1].FactorId))
                            {
                                taxonomicFactors.Add(dyntaxaFactors.DyntaxaAllFactors[i - 1]);
                            }

                            taxonomicFactors.Add(dyntaxaFactors.DyntaxaAllFactors[i]);
                        }
                        else if (i == 0 && dyntaxaFactors.DyntaxaAllFactors[i].IsTaxonomic && dyntaxaFactors.DyntaxaAllFactors[i].IsLeaf)
                        {
                            taxonomicFactors.Add(dyntaxaFactors.DyntaxaAllFactors[i]);
                        }
                    }

                    dyntaxaFactors.DyntaxaAllFactors = taxonomicFactors;
                    IList<DyntaxaFactor> allFactors = modelManager.GetFactorsFromFactorIdAndFactorDataTypeSubstrate(factorDataType.ToString());
                           
                    // IList<SpeciesFactViewModelItem> headerItems = new List<SpeciesFactViewModelItem>();
                    // IDictionary<int, string> factorsWitdthHosts = new Dictionary<int, string>();
                    if (dyntaxaFactors.IsNotNull())
                    {
                        // DyntaxaAllFactorData dyntaxaFactors2 = modelManager.GetFactorsFromTaxonAndParentFactor();
                        
                        // Create the view with all factors shown for selecetd taxon, add ony taxonomic factors....
                        subModel = manager.CreateSpeciesFactViewData(loggedInUser, taxon, dyntaxaFactors, showNonPublicData, subModel, false);

                        foreach (SpeciesFactViewModelHeaderItem headerItem in subModel.SpeciesFactViewModelHeaderItemList)
                        {
                            foreach (SpeciesFactViewModelSubHeaderItem superiorHeaderItem in headerItem.SpeciecFactViewModelSubHeaderItemList)
                            {
                                IList<SpeciesFactViewModelItem> newSpeciecFactList = new List<SpeciesFactViewModelItem>();
                                IList<SpeciesFactViewModelItem> itemsToBeRemoved = new List<SpeciesFactViewModelItem>();
                                //foreach (SpeciesFactViewModelItem speciesFactViewModelItem in superiorHeaderItem.SpeciesFactViewModelItemList)
                                for (int i = 0; i < superiorHeaderItem.SpeciesFactViewModelItemList.Count; i++)
                                {
                                    //if (speciesFactViewModelItem.IsMainHeader)
                                    //{
                                    //    headerItems.Add((speciesFactViewModelItem));
                                    //}
                                    newSpeciecFactList.Add(superiorHeaderItem.SpeciesFactViewModelItemList[i]);

                                    superiorHeaderItem.SpeciesFactViewModelItemList[i].IsShortList = false;
                                    foreach (DyntaxaFactor dyntaxaFactor in allFactors)
                                    {
                                        if (superiorHeaderItem.SpeciesFactViewModelItemList[i].FactorId == dyntaxaFactor.Id)
                                        {
                                            // Yes it is in ShortList
                                            superiorHeaderItem.SpeciesFactViewModelItemList[i].IsShortList = true;
                                            break;
                                        }
                                    }
                                    // Must check item befor if it is a header
                                    if (i > 0)
                                    {
                                        if (superiorHeaderItem.SpeciesFactViewModelItemList[i - 1].IsSuperiorHeader &&
                                            superiorHeaderItem.SpeciesFactViewModelItemList[i - 1].SuperiorHeader.Equals(superiorHeaderItem.SpeciesFactViewModelItemList[i].FactorName))
                                        {
                                            itemsToBeRemoved.Add(superiorHeaderItem.SpeciesFactViewModelItemList[i - 1]);
                                            superiorHeaderItem.SpeciesFactViewModelItemList[i].IsHeader = true;
                                            superiorHeaderItem.SpeciesFactViewModelItemList[i].SuperiorHeader = superiorHeaderItem.SpeciesFactViewModelItemList[i].FactorName;
                                        }
                                        else if (superiorHeaderItem.SpeciesFactViewModelItemList[i - 1].IsHeader && superiorHeaderItem.SpeciesFactViewModelItemList[i - 1].FactorId == superiorHeaderItem.SpeciesFactViewModelItemList[i].FactorId
                                            && !superiorHeaderItem.SpeciesFactViewModelItemList[i].IsHost)
                                        {
                                            superiorHeaderItem.SpeciesFactViewModelItemList[i].IsHeader = true;
                                            superiorHeaderItem.SpeciesFactViewModelItemList[i].SuperiorHeader = superiorHeaderItem.SpeciesFactViewModelItemList[i].FactorName;
                                        }
                                    }
                                }
                                    
                                foreach (var speciesFactViewModelItem in itemsToBeRemoved)
                                {
                                    newSpeciecFactList.Remove(speciesFactViewModelItem);
                                }

                                superiorHeaderItem.SpeciesFactViewModelItemList = newSpeciecFactList;
                            }
                        }
                         
                        // Add all factors that exist for displaying the add factors dropbox.
                        model.AddFactorToHostList = new List<SpeciesFactDropDownModelHelper>();
                        model.AddFactorToHostList.Add(new SpeciesFactDropDownModelHelper(0, Resources.DyntaxaResource.SpeciesFactEditFactorsDropDownDefaultLabel));

                        // Get factors that belongs to Species as substrate
                        List<DyntaxaFactor> existingFactors = dyntaxaFactors.DyntaxaAllFactors as List<DyntaxaFactor>;
                        bool isHeaderEditable = false;

                        foreach (DyntaxaFactor factor in allFactors)
                        {
                            if (factor.FactorUpdateMode.OkToUpdate)
                            {
                                isHeaderEditable = true;
                            }
                            else
                            {
                                isHeaderEditable = false;
                            }

                            // Bug fix 2014-03-11 agoh now we show all factors. existingFactors != null && existingFactors.Exists(item => item.Id == factor.Id);
                            bool alreadyExists = false;
                            if (!alreadyExists) // kolla subheadrarna och sätt även dessa som headers)
                            {
                                bool isSelectable = isHeaderEditable;
                                bool isSubHeader = isHeaderEditable;
                                //bool isLeaf = factor.IsLeaf;
                                //if (!isLeaf && isSelectable)
                                //{
                                //    isSelectable = false;
                                //}
                                model.AddFactorToHostList.Add(new SpeciesFactDropDownModelHelper(Convert.ToInt32(factor.Id), factor.Label, isSelectable, isSubHeader));
                            }
                        }

                        model.DataType = DyntaxaDataType.ENUM;
                        subModel.DataType = DyntaxaDataType.ENUM;
                        model.FactorDataType = DyntaxaFactorDataType.AF_SUBSTRATE;
                        subModel.FactorDataType = DyntaxaFactorDataType.AF_SUBSTRATE;
                        model.SpeciesFactViewModel = subModel; 
                    }
                    else
                    {
                        string tempMessage = Resources.DyntaxaResource.SpeciesFactEditFactorInvalidCombinationErrorMessage.Replace("[taxonId]", taxonId);
                        tempMessage = tempMessage.Replace("[factorId]", factorId.ToString());
                        tempMessage = tempMessage.Replace("[factorDataType]", factorDataType.ToString());
                        errorMsg = tempMessage;
                    }
                }
                else
                {
                    errorMsg = Resources.DyntaxaResource.SharedInvalidUserContext;
                }

                if (!errorMsg.Equals(string.Empty))
                {
                    ModelState.AddModelError(string.Empty, errorMsg);
                }

                // Remove taxonid from model state so that a string can be set in the URL and not only and id...
                // (bug in Rounte handling for this application) 
                ModelState.Remove("TaxonId");
                if (reset.IsNotNull() && (bool)reset)
                {
                    SpeciesFactFactorIdList = null;
                    SpeciesFactHostTaxonIdList = null;
                }

                if (SpeciesFactFactorIdList != null)
                {
                    foreach (var factor in SpeciesFactFactorIdList)
                    {
                        foreach (SpeciesFactHostViewModelItem item in model.HostFactorList)
                        {
                            // Find out of item is checked
                            if (item.FactorId == factor.Id)
                            {
                                item.IsChecked = true;
                                break;
                            }
                        }
                    }
                 }

                if (SpeciesFactHostTaxonIdList != null)
                {
                    foreach (SpeciesFactHostsIdListHelper host in SpeciesFactHostTaxonIdList)
                    {
                        foreach (SpeciesFactHostViewModelItem item in model.HostTaxonList)
                        {
                            // Find out of item is checked
                            if (item.TaxonId == host.Id && item.IsHostToFactorId == host.FactorId && item.CategoryId == host.CategoryId)
                            {
                                item.IsChecked = true;
                                break;
                            }
                        }
                    }
                }
              
                return View("EditHostFactorsForSubstrate", model);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.SpeciesFactEditFactorHeaderText,
                Resources.DyntaxaResource.SpeciesFactEditFactorHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Post action for edit  factors, new factor values are saved.
        /// </summary>
        /// <param name="model"> Model returing required values from view</param>
        /// <returns></returns>
        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.SpeciesFactEVAEditor)]
        public ActionResult EditHostFactorsForSubstrate(SpeciesFactHostViewModel model)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;

            try
            {
                // Extract enumvalues from all viewed factors
                List<string> allFactorEnumFieldValueKeys = Request.Form.AllKeys.Where(key => key.StartsWith("taxonCheckboxValue_")).ToList();
                SpeciesFactFactorIdList = new List<SpeciesFactHostsIdListHelper>();
                SpeciesFactHostTaxonIdList = new List<SpeciesFactHostsIdListHelper>();
                foreach (string key in allFactorEnumFieldValueKeys)
                {
                   String[] keyParts = key.Split("_".ToCharArray());
                    
                   int taxonId = int.Parse(keyParts[1]);
                   int factorId = int.Parse(keyParts[2]);
                   int category = int.Parse(keyParts[6]);
                   bool isFactor = key.Contains("factor");
                  
                   if (isFactor)
                   {
                       //if (SpeciesFactFactorIdList.IsNull())
                          
                       // Add checked speciesfact  factor items
                       //if (!SpeciesFactFactorIdList.Contains(id))
                       SpeciesFactHostsIdListHelper factorData = new SpeciesFactHostsIdListHelper();
                       factorData.CategoryId = category;
                       factorData.Id = factorId;
                       SpeciesFactFactorIdList.Add(factorData);
                   }
                   else
                   {
                       //if (SpeciesFactHostTaxonIdList.IsNull())
                        
                       // Add checked speciesfact host taxon items
                       //if (!SpeciesFactHostTaxonIdList.Contains(id))
                       SpeciesFactHostsIdListHelper hostData = new SpeciesFactHostsIdListHelper();
                       hostData.CategoryId = category;
                       hostData.Id = taxonId;
                       hostData.FactorId = factorId;
                       SpeciesFactHostTaxonIdList.Add(hostData);
                   }
                }
               
                ViewData.Model = model;

                return RedirectToAction("EditHostFactorsForSubstrate", new { @taxonId = Convert.ToString(model.TaxonId), @referenceId = Convert.ToString(model.ReferenceId) });
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.SpeciesFactEditFactorHeaderText,
                Resources.DyntaxaResource.SpeciesFactEditFactorHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Get action for editing one factor. The following values can be changed:
        /// Factorvalue "Betydelse"
        /// Comment
        /// Quality
        /// Reference
        /// Also possible to add values, se abowe, för a new individual category.
        /// </summary>
        /// <param name="taxonId"></param>
        /// <param name="factorId"></param>
        /// <param name="dataType"></param>
        /// <param name="factorDataType"></param>
        /// <param name="childFactorId"></param>
        /// <param name="referenceId"></param>
        /// <param name="individualCategoryId"></param>
        /// <param name="hostId"></param>
        /// <param name="mainParentFactorId"></param>
        /// <returns></returns>
        [HttpGet]
        [DyntaxaAuthorize(Order = RequiredAuthorization.Authenticated)]
        public ActionResult EditFactorItem(string taxonId, string dataType, string factorDataType, string childFactorId, string referenceId, string individualCategoryId, string hostId, string mainParentFactorId)
        {
            string errorMsg = string.Empty;

            // Get taxon and user.
            IUserContext user = GetCurrentUser();
            TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
          
            try
            {
                SpeciesFactModelManager modelManager = new SpeciesFactModelManager(user, searchResult.Taxon, Convert.ToInt32(childFactorId), Convert.ToInt32(dataType), Convert.ToInt32(factorDataType));
                
                // Create View Model
                FactorViewModel model = new FactorViewModel();
                model.FactorDataTypeId = Convert.ToInt32(factorDataType);
                SpeciesFactDyntaxaListManager manager = new SpeciesFactDyntaxaListManager();
                
                // Get all individual categories
                IList<DyntaxaIndividualCategory> categories = modelManager.GetAllIndividualCategories();
                
                // Get factor, speciesFact, hosts, individual categories etc that belongs to this taxon and factor
                DyntaxaSpeciesFact factorData = modelManager.GetSpeciesFactFromTaxonAndFactor(childFactorId, individualCategoryId, hostId);
                if (factorData != null)
                {
                    // Check if factor data exists for this individual category otherwise create new/defult values for this category and factor.
                    List<DyntaxaIndividualCategory> existingCategories = factorData.IndividualCategoryList as List<DyntaxaIndividualCategory>;
                    bool isHost = hostId.IsNotNull() && Convert.ToInt32(hostId) != 0;
                    bool alreadyExists = existingCategories != null && existingCategories.Any(x => x.Id == Convert.ToInt32(individualCategoryId));
                    if (alreadyExists)
                    {
                        model = manager.CreateFactorViewData(user, searchResult.Taxon, factorData, Convert.ToInt32(factorDataType), Convert.ToInt32(dataType), model, categories, referenceId, isHost);
                        model.IsNewCategory = false;
                    }
                    else
                    {
                        // Create data for adding new factor data

                        model = manager.CreateNewFactorViewData(user, searchResult.Taxon, factorData, Convert.ToInt32(factorDataType), Convert.ToInt32(dataType), model, categories, referenceId, isHost);
                        DyntaxaIndividualCategory dyntaxaIndividualCategory = modelManager.GetIndividualCategory(Convert.ToInt32(individualCategoryId));
                        model.IndividualCategoryName = dyntaxaIndividualCategory.Label;
                        model.IndividualCategoryId = dyntaxaIndividualCategory.Id;
                        model.IsNewCategory = true;
                    }
                }

                model.ChildFactorId = Convert.ToInt32(childFactorId);
                model.HostId = Convert.ToInt32(hostId);
                model.ReferenceId = Convert.ToInt32(referenceId);
                model.MainParentFactorId = Convert.ToInt32(mainParentFactorId);
                model.DataTypeId = Convert.ToInt32(dataType);
                return View(model);
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = Resources.DyntaxaResource.SharedNotPossibleToUpdateSpeciesFactError;
                ModelState.AddModelError(string.Empty, errorMsg);
            }
           
            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.SpeciesFactEditFactorHeaderText,
                Resources.DyntaxaResource.SpeciesFactEditFactorHeaderText,
                errorMsg, 
                "");
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Post action for editing one factor. The following values can be updated:
        /// Factorvalue "Betydelse"
        /// Comment
        /// Quality
        /// Reference
        /// Also possible to add values, se abowe, för a new individual category.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.SpeciesFactEVAEditor)]
        public ActionResult EditFactorItem(FactorViewModel model)
        {
            string errorMsg = string.Empty;

            // Get taxon and user
            IUserContext user = GetCurrentUser();
            TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(Convert.ToString(model.TaxonId));
            
            // TODO Model factorid är fel!!!!
            SpeciesFactModelManager modelManager = new SpeciesFactModelManager(user, searchResult.Taxon, Convert.ToInt32(model.ChildFactorId), Convert.ToInt32(model.DataTypeId), Convert.ToInt32(model.FactorDataTypeId));

            try
            {
                // Here we update values for selected factor and individual category.
                IList<SpeciesFactFieldValueModelHelper> newValues = new List<SpeciesFactFieldValueModelHelper>();
                SpeciesFactFieldValueModelHelper newValue = new SpeciesFactFieldValueModelHelper()
                    {
                        FactorId = Convert.ToInt32(model.ChildFactorId), 
                        FactorField1Value = model.FactorFieldEnumValue,
                        HostId = model.HostId, 
                        IndividualCategoryId = model.IndividualCategoryId,
                        StringValue5 = model.FactorFieldComment, 
                        QualityId = model.QualityId,
                        ReferenceId = model.FactorReferenceId,
                        FactorField2Value = model.FactorFieldEnumValue2,
                        MainParentFactorId = model.MainParentFactorId
                    };
                newValues.Add(newValue);
                modelManager.UpdatedSpeciecFacts(newValues, model.IsNewCategory, true);
                if (model.IsNewCategory)
                {
                    return RedirectToAction("EditFactors", new { @taxonId = Convert.ToString(model.TaxonId), @factorId = Convert.ToString((int)model.MainParentFactorId), @dataType = Convert.ToString((int)model.DataTypeId), @factorDataType = Convert.ToString((int)model.FactorDataTypeId), @referenceId = Convert.ToString(model.ReferenceId) });
                }
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = Resources.DyntaxaResource.SharedNotPossibleToUpdateSpeciesFactError;
                ModelState.AddModelError(string.Empty, errorMsg);
            }
            
            //if (!ModelState.IsValid)
            //{
            //    // Reload data to get list data... Plenty of code for doing very little.....
            //    DyntaxaSpeciesFact factorData = modelManager.GetSpeciesFactFromTaxonAndFactor(Convert.ToString(model.ChildFactorId), Convert.ToString(model.IndividualCategoryId), Convert.ToString(model.HostId));
            //    FactorViewModel tempModel = new FactorViewModel();
            //    SpeciesFactDyntaxaListManager manager = new SpeciesFactDyntaxaListManager();
            //    IList<DyntaxaIndividualCategory> categories = modelManager.GetAllIndividualCategories();
            //    tempModel = manager.CreateFactorViewData(user, searchResult.Taxon, factorData, (int)model.FactorDataType, tempModel, categories, Convert.ToString(model.ReferenceId));
          
            //    if (model.FactorFieldEnumValueList.IsNull())
            //    {
            //        model.FactorFieldEnumValueList = tempModel.FactorFieldEnumValueList;
            //    }
            //    if (model.FaktorReferenceList.IsNull())
            //    {
            //        model.FaktorReferenceList = tempModel.FaktorReferenceList;
            //    }
            //    if (model.IndividualCategoryList.IsNull())
            //    {
            //        model.IndividualCategoryList = tempModel.IndividualCategoryList;
            //    }
            //    return PartialView("EditFactorItem", model);
            //}
            return Json(new { success = true });
        }

        /// <summary>
        /// Get action for editing one factor. The following values can be changed:
        /// Factorvalue "Betydelse"
        /// Comment
        /// Quality
        /// Reference
        /// Also possible to add values, se abowe, för a new individual category.
        /// </summary>
        /// <param name="taxonId"></param>
        /// <param name="factorId"></param>
        /// <param name="dataType"></param>
        /// <param name="factorDataType"></param>
        /// <param name="childFactorId"></param>
        /// <param name="referenceId"></param>
        /// <param name="individualCategoryId"></param>
        /// <param name="hostId"></param>
        /// <param name="mainParentFactorId"></param>
        /// <returns></returns>
        [HttpGet]
        [DyntaxaAuthorize(Order = RequiredAuthorization.Authenticated)]
        public ActionResult EditHostFactorItem(string taxonId, string dataType, string factorDataType, string childFactorId, string referenceId, string individualCategoryId, string hostId, string mainParentFactorId, string oldIndividualCategoryId)
        {
            string errorMsg = string.Empty;

            // Get taxon and user.
            IUserContext user = GetCurrentUser();
            TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
            
            try
            {
                SpeciesFactModelManager modelManager = new SpeciesFactModelManager(user, searchResult.Taxon, Convert.ToInt32(childFactorId), Convert.ToInt32(dataType), Convert.ToInt32(factorDataType));
                
                // Create View Model
                FactorViewModel model = new FactorViewModel();
                   
                SpeciesFactDyntaxaListManager manager = new SpeciesFactDyntaxaListManager();
                
                // Get all individual categories
                IList<DyntaxaIndividualCategory> categories = modelManager.GetAllIndividualCategories();
                
                // Get factor, speciesFact, hosts, individual categories etc that belongs to this taxon and factor
                DyntaxaSpeciesFact factorData = modelManager.GetSpeciesFactFromTaxonAndFactor(childFactorId, individualCategoryId, hostId);
                if (factorData != null)
                {
                    // Check if factor data exists for this individual category otherwise create new/defult values for this category and factor.
                    List<DyntaxaIndividualCategory> existingCategories = factorData.IndividualCategoryList as List<DyntaxaIndividualCategory>;
                    bool alreadyExists = existingCategories != null && existingCategories.Any(x => x.Id == Convert.ToInt32(individualCategoryId));
                    if (alreadyExists)
                    {
                        model = manager.CreateFactorViewData(user, searchResult.Taxon, factorData, Convert.ToInt32(factorDataType), Convert.ToInt32(dataType), model, categories, referenceId, true);
                        model.IsNewCategory = false;
                       
                        model.IndividualCategoryId = Convert.ToInt32(individualCategoryId);
                        if (oldIndividualCategoryId.IsNotEmpty())
                        { 
                            model.OldIndividualCategoryId = Convert.ToInt32(oldIndividualCategoryId);
                        }
                        else
                        {
                            model.OldIndividualCategoryId = Convert.ToInt32(individualCategoryId);
                        }
                    }
                    else
                    {
                        // Create data for adding new factor data
                        model = manager.CreateNewFactorViewData(user, searchResult.Taxon, factorData, Convert.ToInt32(factorDataType), Convert.ToInt32(dataType), model, categories, referenceId, true);
                        DyntaxaIndividualCategory dyntaxaIndividualCategory = modelManager.GetIndividualCategory(Convert.ToInt32(individualCategoryId));
                        model.IndividualCategoryName = dyntaxaIndividualCategory.Label;
                        model.IndividualCategoryId = dyntaxaIndividualCategory.Id;
                        if (oldIndividualCategoryId.IsNotEmpty())
                        {
                            model.OldIndividualCategoryId = Convert.ToInt32(oldIndividualCategoryId);
                        }
                        else
                        {
                            model.OldIndividualCategoryId = dyntaxaIndividualCategory.Id;
                        }
                        model.IsNewCategory = true;
                    }
                }

                model.ChildFactorId = Convert.ToInt32(childFactorId);
                model.HostId = Convert.ToInt32(hostId);
                model.ReferenceId = Convert.ToInt32(referenceId);
                model.MainParentFactorId = Convert.ToInt32(mainParentFactorId);
                model.DataTypeId = Convert.ToInt32(dataType);
                model.FactorDataTypeId = Convert.ToInt32(factorDataType);
                return View(model);
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = Resources.DyntaxaResource.SharedNotPossibleToUpdateSpeciesFactError;
                ModelState.AddModelError(string.Empty, errorMsg);
            }
           
            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.SpeciesFactEditFactorHeaderText,
                Resources.DyntaxaResource.SpeciesFactEditFactorHeaderText,
                errorMsg, 
                "");
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Post action for editing one factor. The following values can be updated:
        /// Factorvalue "Betydelse"
        /// Comment
        /// Quality
        /// Reference
        /// Also possible to add values, se abowe, för a new individual category.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.SpeciesFactEVAEditor)]
        public ActionResult EditHostFactorItem(FactorViewModel model)
        {
            string errorMsg = string.Empty;

            // Get taxon and user
            IUserContext user = GetCurrentUser();
            TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(Convert.ToString(model.TaxonId));
            
            SpeciesFactModelManager modelManager = new SpeciesFactModelManager(user, searchResult.Taxon, Convert.ToInt32(model.ChildFactorId), Convert.ToInt32(model.DataTypeId), Convert.ToInt32(model.FactorDataTypeId));

            try
            {
                // Check if speciecfact exist for taxon, factor and category
                bool speciesfactExist = false;
               
               IList<DyntaxaSpeciesFact> speciesFacts = modelManager.GetSpeciesFactFromHost(model.HostId, model.IndividualCategoryId, Convert.ToInt32(model.ChildFactorId));
               if (speciesFacts.IsNotNull() && speciesFacts.Count == 1)
               {
                   speciesfactExist = true;
               }
                // Here we update values for selected factor and individual category.
                IList<SpeciesFactFieldValueModelHelper> newValues = new List<SpeciesFactFieldValueModelHelper>();
                SpeciesFactFieldValueModelHelper newValue = new SpeciesFactFieldValueModelHelper()
                {
                    FactorId = Convert.ToInt32(model.ChildFactorId),
                    FactorField1Value = model.FactorFieldEnumValue,
                    HostId = model.HostId,
                    IndividualCategoryId = model.IndividualCategoryId,
                    StringValue5 = model.FactorFieldComment,
                    QualityId = model.QualityId,
                    ReferenceId = model.FactorReferenceId,
                    FactorField2Value = model.FactorFieldEnumValue2,
                    MainParentFactorId = model.MainParentFactorId
                };

                newValues.Add(newValue);

                modelManager.UpdatedSpeciecFacts(newValues, model.IsNewCategory, true);
                bool isCategoryChanged = model.IndividualCategoryId != model.OldIndividualCategoryId;

                if (model.IsNewCategory || isCategoryChanged)
                {
                    return RedirectToAction("EditHostFactorsForSubstrate", new { @taxonId = Convert.ToString(model.TaxonId), @referenceId = Convert.ToString(model.ReferenceId) });
                }
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = Resources.DyntaxaResource.SharedNotPossibleToUpdateSpeciesFactError;
                ModelState.AddModelError(string.Empty, errorMsg);
            }

            return Json(new { success = true });
        }
        
         /// <summary>
        /// Get action for editing one factor. The following values can be changed:
        /// Factorvalue "Betydelse"
        /// Comment
        /// Quality
        /// Reference
        /// Also possible to add values, se abowe, för a new individual category.
        /// </summary>
        /// <param name="taxonId"></param>
        /// <param name="factorId"></param>
        /// <param name="dataType"></param>
        /// <param name="factorDataType"></param>
        /// <param name="childFactorId"></param>
        /// <param name="referenceId"></param>
        /// <param name="individualCategoryId"></param>
        /// <param name="hostId"></param>
        /// <param name="mainParentFactorId"></param>
        /// <returns></returns>
        [HttpGet]
        [DyntaxaAuthorize(Order = RequiredAuthorization.Authenticated)]
        public ActionResult EditHostFactorItems(string taxonId, string referenceId)
        {
            string errorMsg = string.Empty;
            int factorId = (int)DyntaxaFactorId.SUBSTRATE;
            int factorDataType = (int)DyntaxaFactorDataType.AF_SUBSTRATE;
            int dataType = (int)DyntaxaDataType.ENUM;

            // Get taxon and user.
            IUserContext user = GetCurrentUser();
            TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
            if (searchResult.NumberOfMatches != 1)
            {
                return RedirectToSearch(taxonId);
            }

            // Set search out taxon to be the used one
            ITaxon taxon = searchResult.Taxon;
            try
            {
                SpeciesFactModelManager modelManager = new SpeciesFactModelManager(user, null, 0, dataType, factorDataType);
                modelManager.Taxon = taxon;
                
                 // Create View Model
                FactorViewModel model = new FactorViewModel();
                model.FactorDataTypeId = Convert.ToInt32(factorDataType);
                SpeciesFactDyntaxaListManager manager = new SpeciesFactDyntaxaListManager();
                
                // Get factor, speciesFact, hosts, individual categories etc that belongs to this taxon and factor
                // Get selected factors and host taxa
                if (SpeciesFactFactorIdList.IsNotEmpty() || SpeciesFactHostTaxonIdList.IsNotEmpty())
                {
                    List<SpeciesFactHostsIdListHelper> taxonIds2 = null;
                    if (SpeciesFactHostTaxonIdList.IsNotEmpty())
                    {
                        taxonIds2 = SpeciesFactHostTaxonIdList;
                    }

                    List<SpeciesFactHostsIdListHelper> factorIds2 = null;
                    if (SpeciesFactFactorIdList.IsNotEmpty())
                    {
                        factorIds2 = SpeciesFactFactorIdList;
                    }

                    IList<DyntaxaSpeciesFact> factorData = modelManager.GetSpeciesFactFromSelectedTaxaAndFactors(Convert.ToInt32(taxonId), factorId, factorIds2, taxonIds2);
                    if (factorData != null)
                    {
                        model = manager.CreateFactorAndHostViewData(user, taxon, factorData, Convert.ToInt32(factorDataType), Convert.ToInt32(dataType), model, referenceId, true);
                    }
                }
                else
                {
                    // No data exist...
                }

                model.HostId = 0;
                model.ReferenceId = Convert.ToInt32(referenceId);
                model.MainParentFactorId = factorId;
                model.DataTypeId = dataType;

                return View(model);
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = Resources.DyntaxaResource.SharedNotPossibleToUpdateSpeciesFactError;
                ModelState.AddModelError(string.Empty, errorMsg);
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.SpeciesFactEditFactorHeaderText,
                Resources.DyntaxaResource.SpeciesFactEditFactorHeaderText,
                errorMsg, 
                "");
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Post action for editing one factor. The following values can be updated:
        /// Factorvalue "Betydelse"
        /// Comment
        /// Quality
        /// Reference
        /// Also possible to add values, se abowe, för a new individual category.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.SpeciesFactEVAEditor)]
        public ActionResult EditHostFactorItems(FactorViewModel model)
        {
            string errorMsg = string.Empty;

            // Get taxon and user
            IUserContext user = GetCurrentUser();
            TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(Convert.ToString(model.TaxonId));
            // Extract enumvalues from all viewed factors
          
            try
            {
                SpeciesFactModelManager modelManager = new SpeciesFactModelManager(user, null, 0, (int)model.DataTypeId, (int)model.FactorDataTypeId);
                
                modelManager.Taxon = searchResult.Taxon;
                
                // Create View Model
                SpeciesFactDyntaxaListManager manager = new SpeciesFactDyntaxaListManager();
                
                // Get factor, speciesFact, hosts, individual categories etc that belongs to this taxon and factor
                // Get selected factors and host taxa
                if (SpeciesFactFactorIdList.IsNotEmpty() || SpeciesFactHostTaxonIdList.IsNotEmpty())
                {
                    List<SpeciesFactHostsIdListHelper> taxonIds2 = null;
                    if (SpeciesFactHostTaxonIdList.IsNotEmpty())
                    {
                        taxonIds2 = SpeciesFactHostTaxonIdList;
                    }

                    List<SpeciesFactHostsIdListHelper> factorIds2 = null;
                    if (SpeciesFactFactorIdList.IsNotEmpty())
                    {
                        factorIds2 = SpeciesFactFactorIdList;
                    }

                    IList<DyntaxaSpeciesFact> factorData = modelManager.GetSpeciesFactFromSelectedTaxaAndFactors(searchResult.Taxon.Id, model.MainParentFactorId, factorIds2, taxonIds2);
                    if (factorData != null)
                    {
                        foreach (DyntaxaSpeciesFact speciesFact in factorData)
                        {
                            int individualCategory = 0;
                            foreach (SpeciesFactHostsIdListHelper speciesFactHostsIdListHelper in SpeciesFactHostTaxonIdList)
                            {
                                if (speciesFact.FactorIdForHosts == speciesFactHostsIdListHelper.FactorId && 
                                    speciesFact.IndividualCatgory.Id == speciesFactHostsIdListHelper.CategoryId &&
                                    speciesFact.HostId == speciesFactHostsIdListHelper.Id)
                                {
                                    individualCategory = speciesFactHostsIdListHelper.CategoryId;
                                    break;
                                }
                            }
                               
                            // Here we update values for selected factor and individual category.
                            SpeciesFactFieldValueModelHelper newValue = new SpeciesFactFieldValueModelHelper()
                            {
                                FactorField1Value = model.FactorFieldEnumValue,
                                IndividualCategoryId = individualCategory,
                                StringValue5 = model.FactorFieldComment,
                                QualityId = model.QualityId,
                                ReferenceId = model.FactorReferenceId,
                                FactorField2Value = model.FactorFieldEnumValue2,
                                MainParentFactorId = model.MainParentFactorId
                            };
                            IList<DyntaxaSpeciesFact> dyntaxaSpeciesFacts = new List<DyntaxaSpeciesFact>();
                            dyntaxaSpeciesFacts.Add(speciesFact);
                            modelManager.UpdatedSpeciecFacts(newValue, dyntaxaSpeciesFacts);
                        }
                    }
                }

                // Reset selection
                SpeciesFactFactorIdList = null;
                SpeciesFactHostTaxonIdList = null;
                return Json(new { success = true });
                
                //var result = RedirectToAction("EditHostFactorsForSubstrate", new { @taxonId = Convert.ToString(model.TaxonId), @referenceId = Convert.ToString(model.ReferenceId), @individualCategory = Convert.ToString(model.IndividualCategoryId) });
                //return result;
            }
            catch (Exception e)
            {
                DyntaxaLogger.WriteException(e);
                errorMsg = Resources.DyntaxaResource.SharedNotPossibleToUpdateSpeciesFactError;
                ModelState.AddModelError(string.Empty, errorMsg);
            }
            
            //if (!ModelState.IsValid)
            //{
            //    // Reload data to get list data... Plenty of code for doing very little.....
            //    DyntaxaSpeciesFact factorData = modelManager.GetSpeciesFactFromTaxonAndFactor(Convert.ToString(model.ChildFactorId), Convert.ToString(model.IndividualCategoryId), Convert.ToString(model.HostId));
            //    FactorViewModel tempModel = new FactorViewModel();
            //    SpeciesFactDyntaxaListManager manager = new SpeciesFactDyntaxaListManager();
            //    IList<DyntaxaIndividualCategory> categories = modelManager.GetAllIndividualCategories();
            //    tempModel = manager.CreateFactorViewData(user, searchResult.Taxon, factorData, (int)model.FactorDataType, tempModel, categories, Convert.ToString(model.ReferenceId));
          
            //    if (model.FactorFieldEnumValueList.IsNull())
            //    {
            //        model.FactorFieldEnumValueList = tempModel.FactorFieldEnumValueList;
            //    }
            //    if (model.FaktorReferenceList.IsNull())
            //    {
            //        model.FaktorReferenceList = tempModel.FaktorReferenceList;
            //    }
            //    if (model.IndividualCategoryList.IsNull())
            //    {
            //        model.IndividualCategoryList = tempModel.IndividualCategoryList;
            //    }
            //    return PartialView("EditFactorItem", model);
            //}
            return Json(new { success = true });
        }

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
    }
}
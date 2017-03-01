using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Navigation;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;
using ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonName;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using Dyntaxa.Helpers;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers.Extensions;

namespace Dyntaxa.Controllers
{
    public class TaxonNameController : DyntaxaBaseController
    {
        private readonly TaxonNameViewManager _taxonNameViewManager;

        // Called "LIVE"
        public TaxonNameController()
        {   
            _taxonNameViewManager = new TaxonNameViewManager(CoreData.UserManager.GetCurrentUser());
        }

        // Called by test
        public TaxonNameController(IUserDataSource userDataSourceRepository, ITaxonDataSource taxonDataSourceRepository, ISessionHelper session)
            : base(userDataSourceRepository, taxonDataSourceRepository, session)
        {
            _taxonNameViewManager = new TaxonNameViewManager(CoreData.UserManager.GetCurrentUser());
        }

        // GET: /TaxonName/List/5
        [HttpGet]                
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult List(string taxonId)
        {
            int revisionId = this.RevisionId.Value;
            if (taxonId.IsNull())
            {
                taxonId = this.TaxonIdentifier.Id.ToString();
            }
            TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
            if (searchResult.NumberOfMatches != 1)
            {
                return RedirectToSearch(taxonId, new { revisionId = revisionId });
            }

            this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
            ITaxon taxon = searchResult.Taxon;
            ViewBag.Taxon = taxon;
            //this.NavigateData = new NavigateData("TaxonName", "List");
            
            var model = new ListTaxonNameViewModel(taxon, this.TaxonRevision); 
            return View("List", model);            
        }

        [HttpPost]        
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult List(string[] isNotOkForObs, string[] isRecommended)
        {
            ITaxon taxon = TaxonSearchManager.GetTaxonById(TaxonIdentifier.Id.Value);
            var model = new ListTaxonNameViewModel(taxon, this.TaxonRevision); //todo change revisionId
            ValidateTaxon(GetCurrentUser(), taxon.Id);

            TaxonNameList changedTaxonNames = model.GetChangedTaxonNames(isNotOkForObs, isRecommended);
            
            // Only usage: Accepted may have Recommended: Yes.
            foreach (ITaxonName taxonName in changedTaxonNames)
            {
                if (taxonName.IsRecommended && taxonName.NameUsage.Id != (int)TaxonNameUsageId.Accepted)
                {
                    ModelState.AddModelError("", Resources.DyntaxaResource.TaxonNameListWrongNameUsage);
                    break;
                }    
            }
            
            if (ModelState.IsValid)
            {                
                if (changedTaxonNames.Count > 0)
                {
                    IUserContext loggedInUser = GetLoggedInUser();

                    using (ITransaction transaction = loggedInUser.StartTransaction())
                    {
                        CoreData.TaxonManager.UpdateTaxonNames(loggedInUser, this.TaxonRevision, changedTaxonNames);

                        transaction.Commit();
                    }
                }

                RedrawTree();
                return RedirectToAction("List");
            }
            
            ViewBag.Taxon = taxon;
            return View(model);
        }
     
        [HttpGet]
        public PartialViewResult Info(string guid)
        {
            if (string.IsNullOrEmpty(guid))
            {
                throw new ArgumentException("GUID argument is null. TaxonName->Info");
            }

            var model = _taxonNameViewManager.GetInfoViewModel(guid);
            return PartialView(model);
        }

        [HttpGet]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult Add(string taxonId, int? nameCategoryId)
        {            
            TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
            if (searchResult.NumberOfMatches != 1)
            {
                return RedirectToSearch(taxonId); //todo change revisionId
            }

            this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
            ITaxon taxon = searchResult.Taxon;
            ViewBag.Taxon = taxon;
            //this.NavigateData = new NavigateData("TaxonName", "List");

            TaxonNameDetailsViewModel model = _taxonNameViewManager.GetTaxonNameDetailsViewModel(taxon);
            if (nameCategoryId.HasValue)
            {
                model.SelectedCategoryId = nameCategoryId.Value;
            }
            // Since taxonAdd and edit uses the same view we have to set no of references to 1, otherwise 
            // client validation will not pass for TaxonName Add. TODO refactor Taxon name add and edit into two viewmodels
            model.NoOfTaxonNameReferences++;
            return View("Add", model);
        }

        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult Add(TaxonNameDetailsViewModel model)
        {
            ITaxon taxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), model.TaxonId);
            ITaxonName taxonName = new TaxonName();
            ITaxonRevision taxonRevision = this.TaxonRevision;
            _taxonNameViewManager.InitTaxonNameDetails(model, taxon);

            // Only Nomenclature: Correct, Provisional, PreliminarySuggestion or Informal names may have usage: Accepted.            
            if (model.SelectedTaxonNameUsageId == (int)TaxonNameUsageId.Accepted &&
                !(model.SelectedTaxonNameStatusId == (int)TaxonNameStatusId.ApprovedNaming
                || model.SelectedTaxonNameStatusId == (int)TaxonNameStatusId.Provisional
                || model.SelectedTaxonNameStatusId == (int)TaxonNameStatusId.PreliminarySuggestion
                || model.SelectedTaxonNameStatusId == (int)TaxonNameStatusId.Informal))
            {
                ModelState.AddModelError("", Resources.DyntaxaResource.TaxonNameAddEditIncorrectNomencalture);
            }

            if (!ModelState.IsValid)
            {
                return View("Add", model);                
            }
            IUserContext loggedInUser = GetLoggedInUser();
            // Creation of taxon name
            using (ITransaction transaction = loggedInUser.StartTransaction()) 
            {
                taxonName = _taxonNameViewManager.AddTaxonName(model, TaxonIdentifier.Id.Value, this.TaxonRevision);
                // Must set default reference, set from Revision..
                ReferenceRelationList referencesToAdd = ReferenceHelper.GetDefaultReferences(GetCurrentUser(), taxonName, taxonRevision, null);
                var referencesToRemove = new ReferenceRelationList();
                CoreData.ReferenceManager.CreateDeleteReferenceRelations(GetCurrentUser(), referencesToAdd, referencesToRemove);
                transaction.Commit();
            }
            return RedirectToAction("Edit", new { @taxonId = model.TaxonId, @nameId = taxonName.Version });
        }

        [HttpGet]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult Edit(string taxonId, string nameId)
        {
            int revisionId = this.RevisionId.Value;
            int taxonNameId = 0;
            if (nameId.IsNotNull())
            {
                taxonNameId = Int32.Parse(nameId);
            }

            TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
            if (searchResult.NumberOfMatches != 1)
            {
                return RedirectToSearch(taxonId, new { revisionId = revisionId, nameId = taxonNameId }); //todo change revisionId
            }

            this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
            ITaxon taxon = searchResult.Taxon;
            ViewBag.Taxon = taxon;
            //this.NavigateData = new NavigateData("TaxonName", "List");

            var model = _taxonNameViewManager.GetTaxonNameDetailsViewModel(taxon, nameId);                        
           
            return View("Edit", model);
        }
        
        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult Edit(TaxonNameDetailsViewModel model)
        {
            int taxonId = this.TaxonIdentifier.Id.Value;
            ITaxon taxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), model.TaxonId);
            _taxonNameViewManager.InitTaxonNameDetails(model, taxon);
            ValidateTaxon(GetCurrentUser(), taxonId);

            // Only Nomenclature: Correct, Provisional, PreliminarySuggestion or Informal names may have usage: Accepted.            
            if (model.SelectedTaxonNameUsageId == (int)TaxonNameUsageId.Accepted &&
                !(model.SelectedTaxonNameStatusId == (int)TaxonNameStatusId.ApprovedNaming 
                || model.SelectedTaxonNameStatusId == (int)TaxonNameStatusId.Provisional
                || model.SelectedTaxonNameStatusId == (int)TaxonNameStatusId.PreliminarySuggestion
                || model.SelectedTaxonNameStatusId == (int)TaxonNameStatusId.Informal))
            {
                ModelState.AddModelError("", Resources.DyntaxaResource.TaxonNameAddEditIncorrectNomencalture);
            }

            //// Only usage: Accepted may have Recommended: Yes.
            //if (!model.IsRecommended && model.SelectedTaxonNameUsageId == (int)TaxonNameUsageId.Accepted)
            //{
            //    ModelState.AddModelError("", "Only usage: Accepted may have Recommended: Yes.");
            //}

            if (ModelState.IsValid)
            {
                ITaxonName taxonName = _taxonNameViewManager.SaveTaxonNameDetailsChanges(model, taxonId, this.TaxonRevision);
                this.RedrawTree();
                // Must set new value since taxon name is updated for every save...
                return RedirectToAction("Edit", new { @taxonId = model.TaxonId, @nameId = taxonName.Version.ToString() });
            }
            else
            {
                return View("Edit", model);
            }
        }

        [HttpGet]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult Delete(string taxonId, int nameId)
        {            
            var searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
            if (searchResult.NumberOfMatches != 1)
            {
                return RedirectToSearch(taxonId);
            }

            this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
            ITaxon taxon = searchResult.Taxon;
            ViewBag.Taxon = taxon;
            //this.NavigateData = new NavigateData("TaxonName", "List");

            var model = _taxonNameViewManager.GetViewModel(taxon, nameId);            
            return View(model);
        }

        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult Delete(int nameId)
        {            
            ValidateTaxon(GetCurrentUser(), this.TaxonIdentifier.Id.Value);

            // Can't delete if the name is scientific and recommended
            if (!_taxonNameViewManager.CanDeleteName(this.TaxonIdentifier.Id.Value, nameId))
            {
                string errorMsg = Resources.DyntaxaResource.TaxonNameDeleteNotPossibleToDeleteErrorText;
                ModelState.AddModelError("", errorMsg);
            }

            if (ModelState.IsValid)
            {
                _taxonNameViewManager.DeleteName(this.TaxonIdentifier.Id.Value, this.TaxonRevision, nameId);
                return RedirectToAction("List", new { @taxonId = this.TaxonIdentifier.Id.Value });
            }
            else
            {                
                ITaxon taxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), this.TaxonIdentifier.Id.Value);
                var model = _taxonNameViewManager.GetViewModel(taxon, nameId);
                return View(model);
            }
        }
    }
}

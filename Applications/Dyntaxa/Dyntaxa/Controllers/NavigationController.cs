using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Menu;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Navigation;
using Dyntaxa.Helpers;
using Newtonsoft.Json;
using ArtDatabanken.Data;

namespace Dyntaxa.Controllers
{
    public class NavigationController : DyntaxaBaseController
    {
        public PartialViewResult MainMenu(int? id, int? revisionId)
        {
            var menuManager = new MenuManager(id, revisionId, GetCurrentUser());
            MenuModel menuModel = menuManager.CreateMenuModel();
            menuModel.SetCurrentClassOnMenuItem(CurrentAction, CurrentController);            
            return PartialView(menuModel);     
        }

        [ChildActionOnly]
        public ActionResult TaxonTree(int? taxonId, int? revisionId, bool? hideChangeRootTaxon)
        {
            if (taxonId.HasValue)
            {
                var sp = new Stopwatch();
                sp.Start();
                if ((TempData["RecreateTree"] as bool?).GetValueOrDefault())
                {
                    this.RedrawTree();
                }

                taxonId = this.RootTaxonId.Value;
                var model = Session["TaxonTree"] as TaxonTreeViewModel;
                if (model == null)
                {
                    ITaxon taxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), taxonId.Value);
                    model = new TaxonTreeViewModel(taxon, revisionId.HasValue ? revisionId.Value : this.RevisionId, this.RevisionTaxonCategorySortOrder);
                }                

                if (TempData.ContainsKey("expandedTaxaInTree"))
                {
                    var expandedTaxa = TempData["expandedTaxaInTree"] as List<int>;
                    model.RecreateExpandedTaxaAfterRedraw(expandedTaxa);
                    //model.ExpandedTaxa = expandedTaxa;
                }

                if (TempData.ContainsKey("selectedIdInTree"))
                {
                    var selectedId = (int)TempData["selectedIdInTree"];
                    model.ActiveId = selectedId;
                }
                // this is the case when we have created a new taxon
                else if (TempData.ContainsKey("selectedTaxonIdInTree")) 
                {
                    var selectedTaxonId = (int)TempData["selectedTaxonIdInTree"];
                    var treeViewItem = model.GetTreeViewItemByTaxonId(selectedTaxonId);
                    if (treeViewItem != null)
                    {
                        model.ActiveId = treeViewItem.Id;
                    }
                }

                Session["TaxonTree"] = model;

                sp.Stop();
                //DyntaxaLogger.WriteMessage("Taxon tree creation: {0:N0} milliseconds", sp.ElapsedMilliseconds);
                if (hideChangeRootTaxon.HasValue && hideChangeRootTaxon.Value == true)
                {
                    ViewBag.HideChangeRootTaxon = true;
                }
                return PartialView(model);
            }            

            return Content("No taxon selected");
        }

        /// <summary>
        /// Called when the user selects a taxon in the tree
        /// The tree model in Session is updated with which nodes is expanded
        /// and which node is selected.
        /// Then the user is redirected to the requested page
        /// </summary>
        /// <param name="navigateId"></param>
        /// <param name="expandedNodes"></param>
        /// <returns></returns>
        public ActionResult TaxonTreeNavigate(int navigateId, string expandedNodes)
        {                        
            // navigateId is a unique sequential number and is not the TaxonId.
            // you get the TaxonId by model.GetTreeViewItem(navigateTaxonId).TaxonId.
            var model = Session["TaxonTree"] as TaxonTreeViewModel;
            int taxonId = 0;
            if (model != null && !string.IsNullOrEmpty(expandedNodes))
            {
                TaxonTreeViewItem treeItem = model.GetTreeViewItemById(navigateId);                
                if (treeItem == null) // happens when the user navigates backwards and the tree is different from the one in session
                {
                    RedrawTree(taxonId);
                }
                else
                {
                    taxonId = treeItem.TaxonId;
                    var javascriptSerializer = new JavaScriptSerializer();
                    int[] expandedNodesList = javascriptSerializer.Deserialize<int[]>(expandedNodes);
                    model.ActiveId = navigateId;
                    model.ExpandedTaxa = expandedNodesList.ToList();
                    model.BuildParentTaxaList(treeItem);                    
                }
            }
                                    
            return RedirectToAction(this.NavigateData.Action, this.NavigateData.Controller, new { @taxonId = taxonId });
        }

        public ActionResult SetSecondaryRelationsVisibility(bool? show, string returnUrl)
        {
            if (show.HasValue)
            {
                Session["SecondaryRelationsVisibility"] = show.Value;
            }            
            this.RedrawTree();
            return Redirect(returnUrl);
        }

        /// <summary>
        /// Changes the root taxon
        /// Called when the user selects a new root in a select box.        
        /// </summary>
        /// <param name="selectRootTaxon"></param>
        /// <returns></returns>
        public ActionResult ChangeRootTaxon(int selectRootTaxon)
        {            
            this.RedrawTree(selectRootTaxon);
            return RedirectToAction(this.CurrentAction, this.CurrentController, new { @taxonId = selectRootTaxon });            
        }

        /// <summary>
        /// Called from Dynatree as an ajax call.
        /// Returns the closest children to the taxonId as Json
        /// </summary>
        /// <param name="taxonId"></param>
        /// <returns></returns>
        public JsonNetResult GetTreeData(int taxonId)
        {            
            var model = Session["TaxonTree"] as TaxonTreeViewModel;
            TaxonTreeViewItem item = model.GetTreeViewItemById(taxonId);
            if (item == null) // happens when the user navigates backwards and the tree is different from the one in session
            {
                ITaxon taxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), taxonId);
                item = new TaxonTreeViewItem(taxon, null, 1, model.GetUniqueNumber(), true);

                //var str2 = Url.Content(@"~/Images/Icons/minus-small.png");
                //var url = new UrlHelper(this.Request.RequestContext);
                //var str = url.Content(@"~/Images/Icons/minus-small.png");
            }
            item.Children = model.LoadChildren(item, false);            
            return new JsonNetResult(item.Children);
        }
    }
}

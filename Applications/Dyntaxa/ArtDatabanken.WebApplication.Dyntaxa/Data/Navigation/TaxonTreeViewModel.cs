using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using Newtonsoft.Json;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers.Extensions;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using System.Diagnostics;
using System.Web;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Navigation
{
    /// <summary>
    /// A tree view model that can be used with the javascript tree DynaTree
    /// </summary>
    public class TaxonTreeViewModel
    {        
        private readonly TaxonTreeViewItem _rootNode;
        private int? _revisionId;
        private int? _revisionTaxonCategorySortOrder;
        private int _counter = 0;

        /// <summary>
        /// Returns a unique number. A counter started by 0.
        /// Used to set a unique Id to each tree item.
        /// </summary>
        /// <returns></returns>
        public int GetUniqueNumber()
        {
            _counter++;
            return _counter;
        }

        public TaxonTreeViewItem RootNode
        {
            get { return _rootNode; }
        }

        public List<TaxonTreeViewTaxon> ParentTaxa { get; set; }        

        /// <summary>
        /// The current selected node
        /// </summary>
        public int ActiveId
        {
            get
            {
                return _activeId;
            }

            set
            {
                _activeId = value;
                bool hasFoundTaxonId = false;
                TaxonTreeViewItem oldSelectedTaxonItem = null;
                foreach (TaxonTreeViewItem item in GetTreeViewItemEnumerator())
                {
                    if (item.IsActive)
                    {
                        oldSelectedTaxonItem = item;
                    }
                    item.IsActive = false;
                    if (item.Id == _activeId)
                    {
                        item.IsActive = true;
                        hasFoundTaxonId = true;
                    }
                }
                if (!hasFoundTaxonId)
                {
                    if (oldSelectedTaxonItem != null)
                    {
                        //ExpandTreeToTaxon(_activeTaxonId, oldSelectedTaxonItem);
                        oldSelectedTaxonItem.IsActive = true;
                    }
                }
            }
        }
        private int _activeId;

        /// <summary>
        /// Indicates if secondary relations should be visible or not.
        /// </summary>
        public bool? ShowSecondaryRelations
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    return HttpContext.Current.Session["SecondaryRelationsVisibility"] as bool?;
                }

                return null;                       
            }
        }
        ///// <summary>
        ///// Expand the tree to this taxon and select it.
        ///// </summary>
        ///// <param name="taxonId">item to select</param>
        ///// <param name="oldSelectedItem">Backup tree item. If root node is not found then select this item</param>
        //protected void ExpandTreeToTaxon(int taxonId, TaxonTreeViewItem oldSelectedItem)
        //{
        //    try
        //    {
        //        ITaxon taxon = CoreData.TaxonManager.GetTaxonById(CoreData.UserManager.GetCurrentUser(), taxonId);
        //        IList<ITaxonRelation> parents = taxon.GetAllParentTaxonRelations(null, DyntaxaHelper.IsInRevision(), false);
        //        bool foundRootNode = false;                
        //        var expandedTaxa = new List<int>();
        //        foreach (ITaxonRelation taxonRelation in parents)
        //        {
        //            var parentTaxon = taxonRelation.RelatedTaxon;
        //            if (this.RootNode.TaxonId == parentTaxon.Id)
        //            {
        //                foundRootNode = true;                        
        //                expandedTaxa.Add(parentTaxon.Id);
        //            }
        //            else if (foundRootNode)
        //            {
        //                expandedTaxa.Add(parentTaxon.Id);
        //            }
        //        }
        //        if (!foundRootNode && oldSelectedItem != null)
        //        {
        //            oldSelectedItem.IsActive = true;
        //        }
        //        else
        //        {
        //            expandedTaxa.Add(taxonId);
        //            RecreateExpandedTaxaAfterRedraw(expandedTaxa);
        //            ActiveId = taxonId;
        //        }
        //    }
        //    catch (Exception ex)
        //    {                
        //        DyntaxaLogger.WriteException(ex);
        //    }            
        //}

        /// <summary>
        /// A list with all nodes that is expanded
        /// </summary>
        public List<int> ExpandedTaxa
        {
            get
            {
                return _expandedTaxa;
            }

            set
            {
                _expandedTaxa = value;
                if (_expandedTaxa == null)
                {
                    return;
                }
                foreach (TaxonTreeViewItem item in GetTreeViewItemEnumerator())
                {
                    item.IsExpanded = false;                    
                    foreach (int id in _expandedTaxa)
                    {
                        if (id == item.Id)
                        {
                            item.IsExpanded = true;
                            break;
                        }
                    }                    
                }
            }
        }
        private List<int> _expandedTaxa;

        /// <summary>
        /// When the tree is recreated and the children isn't loaded then
        /// this function loads the children.
        /// </summary>
        /// <param name="expandedTaxa"></param>
        public void RecreateExpandedTaxaAfterRedraw(List<int> expandedTaxa)
        {
            if (expandedTaxa == null)
            {
                return;
            }

            foreach (int itemId in expandedTaxa)
            {
                TaxonTreeViewItem item = this.GetTreeViewItemById(itemId);
                if (item == null || item.HasChildren)
                {
                    continue;
                }

                item.Children = LoadChildren(item, false);
            }
            this.ExpandedTaxa = expandedTaxa;
        }

        ///// <summary>
        ///// Constructor
        ///// </summary>
        ///// <param name="taxonId"></param>
        ///// <param name="revisionId"></param>
        ///// <param name="revisionTaxonCategorySortOrder"></param>
        //public TaxonTreeViewModel(int taxonId, int? revisionId, int? revisionTaxonCategorySortOrder) : 
        //    this(CoreData.TaxonManager.GetTaxonById(CoreData.UserManager.GetCurrentUser(), taxonId, revisionId, revsionc))
        //{
        //    this._revisionId = revisionId;
        //    this._revisionTaxonCategorySortOrder = revisionTaxonCategorySortOrder;
        //    IUserContext userContext = CoreData.UserManager.GetCurrentUser();
        //    ITaxon taxon = CoreData.TaxonManager.GetTaxonById(userContext, taxonId);            
        //    _rootNode = LoadTaxonTree(taxon, false);
        //    TaxonTreeViewItem treeViewItem = GetTreeViewItemByTaxonId(taxonId);
        //    if (treeViewItem != null)
        //    {
        //        this.ActiveId = treeViewItem.Id;
        //    }

        //    // load parents
        //    BuildParentTaxaList(this.RootNode);
        //}

        public TaxonTreeViewModel(ITaxon taxon, int? revisionId, int? revisionTaxonCategorySortOrder)
        {
            this._revisionId = revisionId;
            this._revisionTaxonCategorySortOrder = revisionTaxonCategorySortOrder;            
            _rootNode = LoadTaxonTree(taxon, false);
            TaxonTreeViewItem treeViewItem = GetTreeViewItemByTaxonId(taxon.Id);
            if (treeViewItem != null)
            {
                this.ActiveId = treeViewItem.Id;
            }

            // load parents
            BuildParentTaxaList(this.RootNode);
        }

        /// <summary>
        /// Builds a list with all parents.
        /// The current taxon is also added last.
        /// </summary>        
        /// <param name="rootTaxon"></param>
        public void BuildParentTaxaList(TaxonTreeViewItem rootTaxon)
        {            
            IUserContext userContext = CoreData.UserManager.GetCurrentUser();
            bool isInRevision = DyntaxaHelper.IsInRevision(userContext, this._revisionId);
            var allParents = (List<ITaxonRelation>)rootTaxon.Taxon.GetAllParentTaxonRelations(userContext, null, isInRevision, false);
            var parents = allParents.GroupBy(x => x.ParentTaxon.Id).Select(x => x.First().ParentTaxon).ToList();
            this.ParentTaxa = new List<TaxonTreeViewTaxon>();
            string categoryName = null;

            foreach (ITaxon parentTaxon in parents)
            {
                if (_revisionTaxonCategorySortOrder.HasValue)
                {
                    if (parentTaxon.Category.SortOrder < _revisionTaxonCategorySortOrder.Value)
                    {
                        continue;
                    }
                }
                //var parentTaxon = taxonRelation.RelatedTaxon;
                categoryName = parentTaxon.Category.Name;                
                this.ParentTaxa.Add(new TaxonTreeViewTaxon(parentTaxon.Id, parentTaxon.ScientificName.IsNotEmpty() ? parentTaxon.ScientificName : "-", categoryName));                
            }
            categoryName = rootTaxon.Taxon.Category.Name;
            this.ParentTaxa.Add(new TaxonTreeViewTaxon(rootTaxon.Taxon.Id, rootTaxon.Taxon.ScientificName.IsNotEmpty() ? rootTaxon.Taxon.ScientificName : "-", categoryName));
            
            // decide which is current root
            foreach (var item in this.ParentTaxa)
            {
                if (item.TaxonId == _rootNode.TaxonId)
                {
                    item.IsCurrentRoot = true;
                    break;
                }
            }            
        }

        /// <summary>
        /// Loads the taxon tree
        /// </summary>
        /// <param name="rootTaxon">the root</param>
        /// <param name="allDescendants">true if we should load all descendants. If false only the closest children is loaded</param>
        /// <returns></returns>
        public TaxonTreeViewItem LoadTaxonTree(ITaxon rootTaxon, bool allDescendants)
        {
            var root = new TaxonTreeViewItem(rootTaxon, null, 0, GetUniqueNumber(), true);
            root.Children = LoadChildren(root, allDescendants);
            root.IsExpanded = true;
            return root;
        }

        /// <summary>
        /// Loads children
        /// </summary>
        /// <param name="parent">the parent</param>
        /// <param name="allDescendants">true if we should load all descendants. If false only the closest children is loaded</param>
        /// <returns></returns>
        public List<TaxonTreeViewItem> LoadChildren(TaxonTreeViewItem parent, bool allDescendants)
        {
            IUserContext userContext = CoreData.UserManager.GetCurrentUser();
            bool isInRevision = DyntaxaHelper.IsInRevision(userContext, this._revisionId);
            var children = new List<TaxonTreeViewItem>();
            var parentChilds = parent.Taxon.GetChildTaxonRelations(userContext, isInRevision, false);
            foreach (ITaxonRelation taxonRelation in parentChilds)
            {
                if (!ShowSecondaryRelations.GetValueOrDefault(true))
                {
                    if (!taxonRelation.IsMainRelation)
                    {
                        continue;
                    }
                }                
                
                TaxonTreeViewItem item = new TaxonTreeViewItem(taxonRelation.ChildTaxon, parent, parent.Level + 1, GetUniqueNumber(), taxonRelation.IsMainRelation);
                children.Add(item);      
                //TODO might be a problem if we are in a revision (should then use CheckedOutChangerChildTaxa)
                if (isInRevision)
                {
                    if (allDescendants && item.Taxon.GetNearestChildTaxonRelations(userContext).IsNotEmpty())
                    {
                        item.Children = LoadChildren(item, true);
                    }                        
                }
                else
                {
                    if (allDescendants && item.Taxon.GetNearestChildTaxonRelations(userContext).IsNotEmpty())
                    {
                        item.Children = LoadChildren(item, true);
                    }
                }                               
            }
            return children;
        }

        /// <summary>
        /// Builds a tree representation for use with the Javascript tree Dynatree
        /// </summary>
        public string GetDynatreeJson()
        {           
            string json = JsonConvert.SerializeObject(this.RootNode);
            return json;            
        }

        /// <summary>
        /// Search the tree for a specific item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TaxonTreeViewItem GetTreeViewItemById(int id)
        {            
            return GetTreeViewItemEnumerator().FirstOrDefault(item => item.Id == id);            
        }

        public TaxonTreeViewItem GetTreeViewItemByTaxonId(int taxonId)
        {            
            return GetTreeViewItemEnumerator().FirstOrDefault(item => item.TaxonId == taxonId);            
        }

        /// <summary>
        /// Returns an enumerator that iterates the tree items.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TaxonTreeViewItem> GetTreeViewItemEnumerator()
        {            
            var stack = new Stack<TaxonTreeViewItem>();
            stack.Push(this.RootNode);
            while (stack.Count > 0)
            {
                TaxonTreeViewItem current = stack.Pop();
                yield return current;
                if (!current.HasChildren)
                {
                    continue;
                }

                foreach (TaxonTreeViewItem item in current.Children)
                {
                    stack.Push(item);
                }
            }            
        }
    }
}

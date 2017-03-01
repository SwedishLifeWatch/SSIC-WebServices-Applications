using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers.Extensions;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Menu
{
    /// <summary>
    /// Class that creates menu-objects.    
    /// </summary>
    public class MenuManager
    {
        private int? _taxonId;
        private int? _revisionId;
        private RouteValueDictionary _defaultRouteValueDictionary;
        private RouteValueDictionary _defaultRevisionIdRouteValueDictionary;
        private IUserContext _user;

        public MenuManager(int? taxonId, int? revisionId, IUserContext user)
        {
            this._taxonId = taxonId;
            this._revisionId = revisionId;
            _defaultRouteValueDictionary = CreateDictionary(taxonId, null);
            _defaultRevisionIdRouteValueDictionary = CreateDictionary(null, revisionId);
            _user = user;
        }

        private RouteValueDictionary CreateDictionary(int? taxonId, int? revisionId)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            if (taxonId.HasValue)
            {
                dic.Add("taxonId", taxonId.Value);
            }

            if (revisionId.HasValue)
            {
                dic.Add("revisionId", revisionId.Value);
            }

            return dic;
        }

        public MenuModel CreateMenuModel()
        {
            if (_user.IsTaxonRevisionAdministrator())
            {
                //Taxon revision administration authority
                return CreateTaxonRevisionAdministratorUserMenuModel();
            }
            if (_user.HasSpeciesFactAuthority())
            {
                //Taxon editor or species fact authorties
                return CreateTaxonEditorUserMenuModel();
            }
            if (_user.IsAuthenticated())
            {
                //A user with public authorties (Private person).
                return CreateAuthorizedUserMenuModel();
            }
            //An unknown user which has not loged in.
            return CreatePublicUserMenuModel();
        }

        /// <summary>
        /// Creates a menu with menu items visible when user is not authorized.
        /// </summary>
        /// <returns></returns>
        private MenuModel CreatePublicUserMenuModel()
        {
            MenuModel model = new MenuModel();

            model.MenuItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemSearch, "SearchResult", "Taxon", _defaultRouteValueDictionary));
            model.MenuItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemTaxonInformation, "Info", "Taxon", _defaultRouteValueDictionary));
            model.MenuItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemRevision, "List", "Revision", _defaultRouteValueDictionary));
            model.MenuItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemMatch, "Settings", "Match", _defaultRouteValueDictionary));
            model.MenuItems.Add(CreateExportMenuItems(false, false));

            //model.MenuItems[0].Current = true;
            return model;
        }

        /// <summary>
        /// Creates a menu with menu items visible when user is authorized.
        /// </summary>
        /// <returns></returns>
        private MenuModel CreateAuthorizedUserMenuModel()
        {
            MenuModel model = new MenuModel();

            model.MenuItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemSearch, "SearchResult", "Taxon", _defaultRouteValueDictionary));
            model.MenuItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemTaxonInformation, "Info", "Taxon", _defaultRouteValueDictionary));            
            model.MenuItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemRevision, "List", "Revision", _defaultRouteValueDictionary));
            model.MenuItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemMatch, "Settings", "Match", _defaultRouteValueDictionary));
            model.MenuItems.Add(CreateExportMenuItems(true, true));
            ////model.MenuItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemExportSubscription, "Subscriptions", "Subscription", _defaultRouteValueDictionary));

            return model;
        }

        /// <summary>
        /// Creates a menu with menu items visible when user is taxon editor.
        /// </summary>
        /// <returns></returns>
        private MenuModel CreateTaxonEditorUserMenuModel()
        {
            var model = new MenuModel();

            model.MenuItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemSearch, "SearchResult", "Taxon", _defaultRouteValueDictionary));
            model.MenuItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemTaxonInformation, "Info", "Taxon", _defaultRouteValueDictionary));
            MenuItem editMenuItem = CreateEditMenuItems(_taxonId, _revisionId, true);
            if (editMenuItem.IsNotNull() && editMenuItem.ChildItems.IsNotNull() && editMenuItem.ChildItems.Count > 0)
            {
                model.MenuItems.Add(editMenuItem);
            }

            if (_revisionId.HasValue)
            {
                model.MenuItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemTaxonName, "List", "TaxonName", _defaultRouteValueDictionary));
            }
            model.MenuItems.Add(CreateRevisionMenuItems(_taxonId, _revisionId));
            if (!_revisionId.HasValue)
            {
                model.MenuItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemMatch, "Settings", "Match", _defaultRouteValueDictionary));
                model.MenuItems.Add(CreateExportMenuItems(true, true));
                ////model.MenuItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemExportSubscription, "Subscriptions", "Subscription", _defaultRouteValueDictionary));
            }   
                     
            return model;
        }
        
        /// <summary>
        /// Creates a menu with menu items visible when user is taxon revision administrator.
        /// </summary>
        /// <returns></returns>
        private MenuModel CreateTaxonRevisionAdministratorUserMenuModel()
        {
            var model = new MenuModel();

            model.MenuItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemSearch, "SearchResult", "Taxon", _defaultRouteValueDictionary));
            model.MenuItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemTaxonInformation, "Info", "Taxon", _defaultRouteValueDictionary));
            MenuItem editMenuItems = CreateEditMenuItems(_taxonId, _revisionId, true);
            if (editMenuItems.IsNotNull() && editMenuItems.ChildItems.IsNotNull() && editMenuItems.ChildItems.Count > 0)
            {
                model.MenuItems.Add(editMenuItems);
            }

            model.MenuItems.Add(CreateRevisionMenuItems(_taxonId, _revisionId, true));
            if (!_revisionId.HasValue)
            {
                model.MenuItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemMatch, "Settings", "Match", _defaultRouteValueDictionary));
                model.MenuItems.Add(CreateExportMenuItems(true, true));
                ////model.MenuItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemExportSubscription, "Subscriptions", "Subscription", _defaultRouteValueDictionary));
            }
            
            return model;
        }

        /// <summary>
        /// Create the taxon menu items. 
        /// </summary>
        /// <param name="taxonId"></param>
        /// <param name="revisionId"></param>
        /// <param name="isTaxonEditor"></param>
        /// <returns></returns>
        private MenuItem CreateEditMenuItems(int? taxonId, int? revisionId, bool isTaxonEditor = false)
        {
            var editMenuItem = new MenuItem(Resources.DyntaxaResource.MenuItemEdit);
            editMenuItem.ChildItems = new List<MenuItem>();

            if (revisionId.HasValue)
            {
                editMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemTaxonName, "List", "TaxonName", _defaultRouteValueDictionary));
                editMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemTaxonEdit, "Edit", "Taxon", _defaultRouteValueDictionary));
                editMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemTaxonAdd, "Add", "Taxon", _defaultRouteValueDictionary));                                
                editMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemTaxonSortChildTaxa, "SortChildTaxa", "Taxon", _defaultRouteValueDictionary));
                editMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemTaxonMove, "Move", "Taxon", _defaultRouteValueDictionary));
                editMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemTaxonAddParent, "AddParent", "Taxon", _defaultRouteValueDictionary));
                editMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemTaxonRemoveParent, "DropParent", "Taxon", _defaultRouteValueDictionary));
                editMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemTaxonLump, "Lump", "Taxon", _defaultRouteValueDictionary));
                editMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemTaxonSplit, "Split", "Taxon", _defaultRouteValueDictionary));
                editMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemTaxonDelete, "Delete", "Taxon", _defaultRouteValueDictionary));
            }
            else
            {
                editMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemRevisionStartEdititng, "StartEditing", "Revision", _defaultRevisionIdRouteValueDictionary));
            }

            if (isTaxonEditor)
            {
                editMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemTaxonOccurrence, "EditSwedishOccurance", "Taxon", _defaultRouteValueDictionary));
                editMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemTaxonQuality, "EditQuality", "Taxon", _defaultRouteValueDictionary));
                editMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemTaxonReferences, "EditReferences", "Taxon", _defaultRouteValueDictionary));
            }

            if (revisionId.HasValue)
            {
                editMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemRevisionStopEdititng, "StopEditingRevision", "Revision", _defaultRevisionIdRouteValueDictionary));
            }
            return editMenuItem;
        }

        private MenuItem CreateRevisionMenuItems(int? taxonId, int? revisionId, bool isTaxonRevisionAdministrator = false)
        {
            MenuItem revisionMenuItem = new MenuItem(Resources.DyntaxaResource.MenuItemRevision);
            
            revisionMenuItem.ChildItems = new List<MenuItem>();

            if (taxonId.HasValue && isTaxonRevisionAdministrator && !revisionId.HasValue)
            {
                revisionMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemRevisionAdd, "Add", "Revision", _defaultRouteValueDictionary));
            }

            revisionMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemRevisionList, "List", "Revision", _defaultRouteValueDictionary));
            
            if (revisionId.HasValue)
            {
                revisionMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemRevisionListRevisionEvents, "ListEvent", "Revision", _defaultRevisionIdRouteValueDictionary));                
            }
                  
            return revisionMenuItem;
        }

        private MenuItem CreateExportMenuItems(bool showDatabase, bool enableSpeciesFact)
        {
            var exportMenuItem = new MenuItem(Resources.DyntaxaResource.MenuItemExport);
            exportMenuItem.ChildItems = new List<MenuItem>();
            exportMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemExportTaxonList, "TaxonList", "Export", _defaultRouteValueDictionary));
            exportMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemExportHierarchicalTaxonList, "HierarchicalTaxonList", "Export", _defaultRouteValueDictionary));
            if (showDatabase)
            {
                exportMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemExportDatabase, "Database", "Export", _defaultRouteValueDictionary));                
            }
            if (_user != null && (_user.IsTaxonRevisionAdministrator() || _user.IsTaxonEditor()))
            {
                exportMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemExportGraphviz, "Graphviz", "Export", _defaultRouteValueDictionary));
            }
            if (enableSpeciesFact)
            {
                exportMenuItem.ChildItems.Add(new MenuItem(Resources.DyntaxaResource.MenuItemSpeciesFactList, "SpeciesFactList", "SpeciesFact", _defaultRouteValueDictionary));
            }
            return exportMenuItem;
        }
    }
}

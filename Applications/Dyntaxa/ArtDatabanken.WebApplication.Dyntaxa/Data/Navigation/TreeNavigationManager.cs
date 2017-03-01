using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Navigation
{
    public static class TreeNavigationManager
    {
        private static readonly Dictionary<NavigateData, NavigateData> _navigationDictionary;
        //private static NavigateData _defaultNavigation = new NavigateData("Taxon", "SearchResult");

        static TreeNavigationManager()
        {
            _navigationDictionary = new Dictionary<NavigateData, NavigateData>();            
            _navigationDictionary.Add(new NavigateData("Taxon", "Info"), new NavigateData("Taxon", "Info"));
            _navigationDictionary.Add(new NavigateData("Taxon", "Delete"), new NavigateData("Taxon", "Delete"));
            _navigationDictionary.Add(new NavigateData("Taxon", "Add"), new NavigateData("Taxon", "Add"));
            _navigationDictionary.Add(new NavigateData("Taxon", "AddParent"), new NavigateData("Taxon", "AddParent"));
            _navigationDictionary.Add(new NavigateData("Taxon", "DropParent"), new NavigateData("Taxon", "DropParent"));
            _navigationDictionary.Add(new NavigateData("Taxon", "Edit"), new NavigateData("Taxon", "Edit"));
            _navigationDictionary.Add(new NavigateData("Taxon", "Move"), new NavigateData("Taxon", "Move"));
            _navigationDictionary.Add(new NavigateData("Taxon", "MoveChangeName"), new NavigateData("Taxon", "Move"));  
            _navigationDictionary.Add(new NavigateData("Taxon", "SortChildTaxa"), new NavigateData("Taxon", "SortChildTaxa"));   
            _navigationDictionary.Add(new NavigateData("Taxon", "Search"), new NavigateData("Taxon", "Info"));
            _navigationDictionary.Add(new NavigateData("Taxon", "SearchResult"), new NavigateData("Taxon", "Info"));
            _navigationDictionary.Add(new NavigateData("Taxon", "Split"), new NavigateData("Taxon", "Split"));
            _navigationDictionary.Add(new NavigateData("Taxon", "Lump"), new NavigateData("Taxon", "Lump"));
            _navigationDictionary.Add(new NavigateData("Taxon", "EditSwedishOccurance"), new NavigateData("Taxon", "EditSwedishOccurance"));
            _navigationDictionary.Add(new NavigateData("Taxon", "EditQuality"), new NavigateData("Taxon", "EditQuality"));
            _navigationDictionary.Add(new NavigateData("Taxon", "EditReferences"), new NavigateData("Taxon", "EditReferences"));
           
            _navigationDictionary.Add(new NavigateData("TaxonName", "List"), new NavigateData("TaxonName", "List"));
            _navigationDictionary.Add(new NavigateData("TaxonName", "Delete"), new NavigateData("TaxonName", "List"));
            _navigationDictionary.Add(new NavigateData("TaxonName", "Edit"), new NavigateData("TaxonName", "List"));
            _navigationDictionary.Add(new NavigateData("TaxonName", "Add"), new NavigateData("TaxonName", "List"));
            _navigationDictionary.Add(new NavigateData("TaxonName", "ChangeChildrenNames"), new NavigateData("TaxonName", "List"));

            _navigationDictionary.Add(new NavigateData("Revision", "List"), new NavigateData("Revision", "List"));
            _navigationDictionary.Add(new NavigateData("Revision", "Add"), new NavigateData("Revision", "Add"));
            _navigationDictionary.Add(new NavigateData("Revision", "Edit"), new NavigateData("Revision", "Edit"));
            _navigationDictionary.Add(new NavigateData("Revision", "Delete"), new NavigateData("Revision", "List"));
            _navigationDictionary.Add(new NavigateData("Revision", "StartEditing"), new NavigateData("Revision", "StartEditing"));
            _navigationDictionary.Add(new NavigateData("Revision", "StopEditing"), new NavigateData("Revision", "StopEditing"));
            _navigationDictionary.Add(new NavigateData("Revision", "ListEvent"), new NavigateData("Revision", "ListEvent"));

            _navigationDictionary.Add(new NavigateData("Match", "Settings"), new NavigateData("Match", "Settings"));

            _navigationDictionary.Add(new NavigateData("Export", "TaxonList"), new NavigateData("Export", "TaxonList"));
            _navigationDictionary.Add(new NavigateData("Export", "HierarchicalTaxonList"), new NavigateData("Export", "HierarchicalTaxonList"));
            _navigationDictionary.Add(new NavigateData("Export", "Database"), new NavigateData("Export", "Database"));
            _navigationDictionary.Add(new NavigateData("Export", "Graphviz"), new NavigateData("Export", "Graphviz"));

            _navigationDictionary.Add(new NavigateData("SpeciesFact", "SpeciesFactList"), new NavigateData("SpeciesFact", "SpeciesFactList"));

            _navigationDictionary.Add(new NavigateData("Subscription", "Subscriptions"), new NavigateData("Subscription", "Subscriptions"));

            _navigationDictionary.Add(new NavigateData("Account", "LogIn"), new NavigateData("Taxon", "Info"));
            _navigationDictionary.Add(new NavigateData("Account", "AccessIsNotAllowed"), new NavigateData("Taxon", "Info"));

            _navigationDictionary.Add(new NavigateData("Errors", "General"), new NavigateData("Taxon", "Info"));

            _navigationDictionary.Add(new NavigateData("Home", "Cookies"), new NavigateData("Taxon", "Info"));
            _navigationDictionary.Add(new NavigateData("Home", "About"), new NavigateData("Taxon", "Info"));
            _navigationDictionary.Add(new NavigateData("Home", "TermsOfUse"), new NavigateData("Taxon", "Info"));
            _navigationDictionary.Add(new NavigateData("Home", "Index"), new NavigateData("Taxon", "Info"));
        }

        public static NavigateData GetNavigation(string controller, string action)
        {
            return GetNavigation(new NavigateData(controller, action));
        }

        public static NavigateData GetNavigation(NavigateData navigateData)
        {            
            NavigateData navTo;
            return _navigationDictionary.TryGetValue(navigateData, out navTo) ? navTo : null;
        }
    }
}

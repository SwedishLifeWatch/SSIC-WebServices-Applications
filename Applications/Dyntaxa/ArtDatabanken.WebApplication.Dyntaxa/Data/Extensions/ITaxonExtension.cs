using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Taxon;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// Extension methods to ITaxon interface.
    /// </summary>
    public static class ITaxonExtension
    {
        private static ITaxonCategory _genusTaxonCategory;

        /// <summary>
        /// Searches among the direct children in childtaxonrelations.
        /// </summary>
        public static IList<ITaxonRelation> GetChildTaxonRelations(
            this ITaxon taxon,
            IUserContext userContext,
            bool isTaxonRevisionEditor,
            bool includeHistorical,
            bool isMainRelation = false)
        {
            if (includeHistorical == false && taxon.IsValid == false)
            {
                return new List<ITaxonRelation>();
            }
            else
            {
                // TaxonRevisionEditor == true (in revision - return published data + data that is changed in the revision)
                if (isTaxonRevisionEditor)
                {
                    if (includeHistorical)
                    {
                        return (from taxonRelation in taxon.GetNearestChildTaxonRelations(userContext)
                                where taxonRelation.IsMainRelation == true || isMainRelation == false
                                select taxonRelation).ToList();
                    }
                    // includeHistorical == false
                    else
                    {
                        return (from taxonRelation in taxon.GetNearestChildTaxonRelations(userContext)
                                where (taxonRelation.ValidToDate > DateTime.Now && (!taxonRelation.ReplacedInTaxonRevisionEventId.HasValue)) &&
                                       (taxonRelation.IsMainRelation == true || isMainRelation == false)
                                select taxonRelation).ToList();
                    }
                }
                // TaxonRevisionEditor == false (NOT in revision - return public data)
                else
                {
                    if (includeHistorical)
                    {
                        return (from taxonRelation in taxon.GetNearestChildTaxonRelations(userContext)
                                where taxonRelation.IsPublished == true &&
                                      (taxonRelation.IsMainRelation == true || isMainRelation == false)
                                select taxonRelation).ToList();
                    }
                    // includeHistorical = false
                    else
                    {
                        return (from taxonRelation in taxon.GetNearestChildTaxonRelations(userContext)
                                where (taxonRelation.ValidToDate > DateTime.Now && taxonRelation.IsPublished == true) &&
                                      (taxonRelation.IsMainRelation == true || isMainRelation == false)
                                select taxonRelation).ToList();
                    }
                }
            }
        }

        /// <summary>
        /// Gets a string with the scientific name, author and common name.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        /// <returns>A string with the scientific name, author and common name.</returns>
        public static string GetLabel(this ITaxon taxon)
        {
            if (taxon == null || taxon.ScientificName.IsEmpty())
            {
                return "";
            }

            var str = new StringBuilder();
            
            str.Append(taxon.ScientificName);
            if (taxon.Author.IsNotEmpty())
            {
                str.Append(" " + taxon.Author);
            }

            if (taxon.CommonName.IsNotEmpty())
            {
                str.Append(", " + taxon.CommonName);
            }
            return str.ToString();
        }

        /// <summary>
        /// Gets a taxons scientific and common name concatenated.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        /// <returns>A taxons scientific and common name concatenated</returns>
        public static string GetScientificAndCommonName(this ITaxon taxon)
        {
            if (taxon.CommonName.IsEmpty() && taxon.ScientificName.IsEmpty())
            {
                return "";
            }

            if (taxon.CommonName.IsEmpty())
            {
                return taxon.ScientificName.IsEmpty() ? taxon.ScientificName : "";
            }
            else
            {
                return string.Format("{0} ({1})", taxon.ScientificName, taxon.CommonName);
            }
        }

        /// <summary>
        /// Gets a taxons common name or the default value.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        /// <param name="strDefault">The default value.</param>
        /// <returns>A taxons common name or the default value.</returns>
        public static string GetCommonNameOrDefault(this ITaxon taxon, string strDefault)
        {
            return taxon.CommonName.IsNotEmpty() ? taxon.CommonName : strDefault;
        }

        /// <summary>
        /// Gets a string with taxons all synonyms separated by ;.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        /// <returns>a string with taxons all synonyms separated by ;.</returns>
        public static string GetScientificSynonymsString(this ITaxon taxon)
        {
            if (taxon == null || taxon.GetSynonyms(CoreData.UserManager.GetCurrentUser(), true) == null)
            {
                return "";
            }

            var names = new StringBuilder();

            foreach (var taxonName in taxon.GetSynonyms(CoreData.UserManager.GetCurrentUser(), true))
            {
                if (names.Length > 0)
                {
                    names.Append("; ");
                }

                names.Append(taxonName.Name);

                if (!string.IsNullOrEmpty(taxonName.Author))
                {
                    names.Append(" ");
                    names.Append(taxonName.Author);
                }
            }            
            return names.ToString();
        }

        /// <summary>
        /// Get all taxa that are possible parents for this taxon.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonRevision">Revision that this taxon belongs to.</param>
        /// <returns>List of taxa that are possible parent taxa.</returns>
        public static TaxonList GetTaxaPossibleParents(
            this ITaxon taxon,
            IUserContext userContext,
            ITaxonRevision taxonRevision)
        {
            ITaxonCategory genusTaxonCategory;
            ITaxonTreeNode revisionTaxonTree;
            TaxonList parentTaxa, possibleParents, revisionTaxa;

            possibleParents = new TaxonList(true);
            if (taxonRevision.IsNotNull())
            {
                genusTaxonCategory = CoreData.TaxonManager.GetTaxonCategory(userContext, TaxonCategoryId.Genus);
                revisionTaxonTree = taxonRevision.RootTaxon.GetChildTaxonTree(userContext, true);
                revisionTaxa = revisionTaxonTree.GetChildTaxa();
                // Add revision root taxon if it doesn't exist in revisionTaxa list.
                if (!revisionTaxa.Exists(taxonRevision.RootTaxon))
                {
                    revisionTaxa.Add(taxonRevision.RootTaxon);
                }                
                parentTaxa = new TaxonList();
                GetParentTaxa(revisionTaxonTree, taxon, parentTaxa);

                revisionTaxa.RemoveAll(x => parentTaxa.Contains(x));
                revisionTaxa.SortTaxonCategory();
                return revisionTaxa;

                //foreach (ITaxon revisionTaxon in revisionTaxa)
                //{
                //    possibleParents.Merge(revisionTaxon);
                //    continue;
                //    // add all taxa that has IsTaxonomic = false to the list of possibleParents -- GuNy 2013-01-09
                //    if (!revisionTaxon.Category.IsTaxonomic && !parentTaxa.Exists(revisionTaxon))
                //    {
                //        possibleParents.Merge(revisionTaxon);
                //    }
                //    // all taxa that is below taxon.Category is removed
                //    else if ((revisionTaxon.Category.SortOrder < taxon.Category.SortOrder) &&
                //        !parentTaxa.Exists(revisionTaxon))
                //    {
                //        if (genusTaxonCategory.SortOrder < taxon.Category.SortOrder)
                //        {
                //            // Limit possible parents to max genus taxon category.
                //            // If the taxon you want to move is below genus then you can only
                //            // move it to taxa that is of genus category or below.
                //            if (genusTaxonCategory.SortOrder <= revisionTaxon.Category.SortOrder)
                //            {
                //                possibleParents.Merge(revisionTaxon);
                //            }
                //        }
                //        else
                //        {
                //            // Include all possible parents from higher taxon categories.
                //            possibleParents.Merge(revisionTaxon);
                //        }
                //    }
                //}
                //possibleParents.SortTaxonCategory();
            }

            return possibleParents;
        }

        /// <summary>
        /// Get parent taxa for specified taxon.
        /// The information is extracted from a taxon tree.
        /// </summary>
        /// <param name="taxonTree">A taxon tree.</param>
        /// <param name="taxon">The taxon.</param>
        /// <param name="parentTaxa">Parent taxa output.</param>
        private static void GetParentTaxa(
            ITaxonTreeNode taxonTree,
            ITaxon taxon,
            TaxonList parentTaxa)
        {
            if (taxonTree.Taxon.Id == taxon.Id)
            {
                // This is the taxon that we are looking for.
                if (taxonTree.Parents.IsNotEmpty())
                {
                    foreach (ITaxonTreeNode parentTaxonTree in taxonTree.Parents)
                    {
                        // Add parent.
                        parentTaxa.Add(parentTaxonTree.Taxon);
                    }
                }
            }
            else
            {
                if (taxonTree.Children.IsNotEmpty())
                {
                    foreach (ITaxonTreeNode childTaxonTree in taxonTree.Children)
                    {
                        GetParentTaxa(childTaxonTree, taxon, parentTaxa);
                        if (parentTaxa.IsNotEmpty())
                        {
                            // We have already found the parents.
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Indicates whether or not the taxon is ranked after genus.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        /// <returns>True if taxon is ranked after genus.</returns>        
        public static Boolean IsBelowGenus(this ITaxon taxon)
        {
            if (_genusTaxonCategory.IsNull())
            {
                _genusTaxonCategory = CoreData.TaxonManager.GetTaxonCategory(
                    CoreData.UserManager.GetCurrentUser(),
                    TaxonCategoryId.Genus);
            }

            return taxon.Category.SortOrder > _genusTaxonCategory.SortOrder;
        }

        /// <summary>
        /// Indicates whether or not the taxon is ranked after genus.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        /// <param name="userContext">The user context.</param>
        /// <returns>True if taxon is ranked after genus.</returns>        
        public static Boolean IsBelowGenus(this ITaxon taxon, IUserContext userContext)
        {
            if (_genusTaxonCategory.IsNull())
            {
                _genusTaxonCategory = CoreData.TaxonManager.GetTaxonCategory(userContext, TaxonCategoryId.Genus);
            }

            return taxon.Category.SortOrder > _genusTaxonCategory.SortOrder;
        }

        /// <summary>
        /// Determines whether the taxon is Biota (life), or not.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        /// <returns>True if the taxon is Biota, and otherwise false.</returns>        
        public static bool IsLifeTaxon(this ITaxon taxon)
        {
            return taxon.Id == 0;
        }

        /// <summary>
        /// Gets a string with all parents to a taxon separated by ;.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        /// <param name="userContext">The user context.</param>
        /// <returns>A string with all parents to a taxon separated by ;.</returns>
        public static string GetParentTaxaString(this ITaxon taxon, IUserContext userContext)
        {
            if (taxon == null)
            {
                return "";
            }

            var parents = new StringBuilder();
            var parentRelations = taxon.GetAllParentTaxonRelations(userContext, null, false, false);
            foreach (var taxonRelation in parentRelations)
            {
                var relatedTaxon = taxonRelation.ParentTaxon;
                if (parents.Length > 0)
                {
                    parents.Append("; ");
                }

                parents.Append(relatedTaxon.Category.Name);
                parents.Append(": ");
                parents.Append(relatedTaxon.GetLabel());
            }

            return parents.ToString();
        }

        /// <summary>
        /// Get taxon names where taxon name category type 
        /// equals IDENTIFIER.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        /// <param name="userContext">The user context.</param>
        /// <returns>
        /// Taxon names where taxon name category type 
        /// equals IDENTIFIER.
        /// </returns>
        public static List<ITaxonName> GetAllIdentifiers(this ITaxon taxon, IUserContext userContext)
        {
            var identifiers = new List<ITaxonName>();
            foreach (ITaxonName taxonName in taxon.GetTaxonNames(userContext))
            {
                if (taxonName.Category.Type.Id == (Int32)TaxonNameCategoryTypeId.Identifier)
                {
                    identifiers.Add(taxonName);
                }
            }

            return identifiers;
        }

        /// <summary>
        /// All taxon synonyms
        /// </summary>
        public static List<TaxonNameViewModel> GetSynonymsViewModel(this ITaxon taxon, bool isInRevision, bool isEditorUser, bool includeProParteSynonyms)
        {
            List<ITaxonName> synonyms = taxon.GetSynonyms(CoreData.UserManager.GetCurrentUser(), includeProParteSynonyms);
            synonyms = synonyms.OrderBy(t => t.Category.Id).ThenBy(t => t.Status.SortOrder()).ToList();
            List<TaxonNameViewModel> resultList = new List<TaxonNameViewModel>();
            foreach (ITaxonName taxonName in synonyms)
            {
                if (IsNameOkToShow(taxonName, isInRevision, isEditorUser))
                {
                    resultList.Add(new TaxonNameViewModel(taxonName, taxon));
                }
            }

            return resultList;
        }

        /// <summary>
        /// Gets the proParte synonyms that is ok to show to the user.
        /// </summary>
        /// <param name="isInRevision">if set to <c>true</c> the session is in revision mode.</param>
        /// <param name="isEditorUser">if set to <c>true</c> the user is taxon editor.</param>
        /// <returns>List of proParte synonyms.</returns>
        public static List<TaxonNameViewModel> GetProParteSynonymsViewModel(this ITaxon taxon, bool isInRevision, bool isEditorUser)
        {
            List<ITaxonName> proParteSynonyms = taxon.GetProParteSynonyms(CoreData.UserManager.GetCurrentUser());
            proParteSynonyms = proParteSynonyms.OrderBy(t => t.Category.Id).ThenBy(t => t.Status.SortOrder()).ToList();
            List<TaxonNameViewModel> resultList = new List<TaxonNameViewModel>();
            foreach (ITaxonName taxonName in proParteSynonyms)
            {
                if (IsNameOkToShow(taxonName, isInRevision, isEditorUser))
                {
                    resultList.Add(new TaxonNameViewModel(taxonName, taxon));
                }
            }

            return resultList;
        }

        /// <summary>
        /// Gets the misapplied names that is ok to show to the user.
        /// </summary>
        /// <param name="isInRevision">if set to <c>true</c> the session is in revision mode.</param>
        /// <param name="isEditorUser">if set to <c>true</c> the user is taxon editor.</param>
        /// <returns>List of misapplied names.</returns>
        public static List<TaxonNameViewModel> GetMisappliedNamesViewModel(this ITaxon taxon, bool isInRevision, bool isEditorUser)
        {
            List<ITaxonName> misappliedNames = taxon.GetMisappliedNames(CoreData.UserManager.GetCurrentUser());
            misappliedNames = misappliedNames.OrderBy(t => t.Category.Id).ThenBy(t => t.Status.SortOrder()).ToList();
            List<TaxonNameViewModel> resultList = new List<TaxonNameViewModel>();
            foreach (ITaxonName taxonName in misappliedNames)
            {
                if (IsNameOkToShow(taxonName, isInRevision, isEditorUser))
                {
                    resultList.Add(new TaxonNameViewModel(taxonName, taxon));
                }
            }

            return resultList;
        }

        /// <summary>
        /// Gets identifiers that is ok to show to the user.
        /// </summary>
        /// <param name="isInRevision">if set to <c>true</c> the session is in revision mode.</param>
        /// <param name="isEditorUser">if set to <c>true</c> the user is taxon editor.</param>
        /// <returns>List of identifiers.</returns>
        public static List<TaxonNameViewModel> GetIdentfiersViewModel(this ITaxon taxon, bool isInRevision, bool isEditorUser)
        {
            List<ITaxonName> identifiers = taxon.GetIdentifiers(CoreData.UserManager.GetCurrentUser());
            identifiers = identifiers.OrderBy(t => t.Category.Id).ThenBy(t => t.Status.SortOrder()).ToList();
            List<TaxonNameViewModel> resultList = new List<TaxonNameViewModel>();
            foreach (ITaxonName taxonName in identifiers)
            {
                if (IsNameOkToShow(taxonName, isInRevision, isEditorUser))
                {
                    resultList.Add(new TaxonNameViewModel(taxonName, taxon));
                }
            }

            return resultList;
        }

        public static List<TaxonNameViewModel> GetNotRecommendedSwedishNamesViewModel(this ITaxon taxon, bool isInRevision, bool isEditorUser)
        {            
            List<ITaxonName> swedishNames = taxon.GetTaxonNamesByCategoryId(CoreData.UserManager.GetCurrentUser(), (int)TaxonNameCategoryId.SwedishName);
            swedishNames = swedishNames.OrderBy(t => t.Category.Id).ThenBy(t => t.Status.SortOrder()).ToList();
            List<TaxonNameViewModel> resultList = new List<TaxonNameViewModel>();
            foreach (ITaxonName taxonName in swedishNames)
            {
                if (taxonName.IsRecommended)
                {
                    continue;
                }
                
                if (taxonName.NameUsage.Id == (int)TaxonNameUsageId.MisappliedAuctName ||
                    taxonName.NameUsage.Id == (int)TaxonNameUsageId.ProParteSynonym)
                {
                    continue;
                }                

                if (IsNameOkToShow(taxonName, isInRevision, isEditorUser))
                {
                    resultList.Add(new TaxonNameViewModel(taxonName, taxon));
                }
            }

            ////if (!taxon.CommonName.IsEmpty())
            ////{
            ////    model.OtherValidCommonNames.AddRange(
            ////        from taxonName in taxon.GetTaxonNames(userContext)
            ////        where
            ////            taxonName.Category.Id == (int)TaxonNameCategoryId.SwedishName &&
            ////            taxonName.Version != taxon.GetCommonName(userContext).Version
            ////        select taxonName.Name);

            ////    // todo - även ha med att namnet är gilitigt. Hur ser man det???         
            ////}

            return resultList;
        }

        /// <summary>
        /// Determines whether the specified taxon name is ok to show to the user on the Info page.
        /// </summary>
        /// <param name="taxonName">Taxon name.</param>
        /// <param name="isInRevision">if set to <c>true</c> the session is in revision mode.</param>
        /// <param name="isEditorUser">if set to <c>true</c> the user is taxon editor.</param>
        /// <returns><c>true</c> if the name is ok to show; otherwise <c>false</c>.</returns>
        private static bool IsNameOkToShow(ITaxonName taxonName, bool isInRevision, bool isEditorUser)
        {
            // Name with status [Removed] is only visible inside revisions.
            // if (!isInRevision && taxonName.Status.Id == (int)TaxonNameStatusId.Removed)
            if (taxonName.Status.Id == (int)TaxonNameStatusId.Removed)
            {
                return false;
            }

            // Name with status [Preliminary] is only visible inside revisions or for logged in taxon editor.
            if (!isInRevision && !isEditorUser &&
                taxonName.Status.Id == (int)TaxonNameStatusId.PreliminarySuggestion)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get common names that are recommended and approved, but not swedish.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        /// <param name="userContext">The user context.</param>
        /// <returns>Common names that are recommended and approved, but not swedish.</returns>
        public static List<ITaxonName> GetOtherLanguagesNames(this ITaxon taxon, IUserContext userContext)
        {
            // todo - fungerar den här inne i en revision?
            var otherLanguagesNames = new List<ITaxonName>();
            DateTime today = DateTime.Now;
            foreach (ITaxonName taxonName in taxon.GetTaxonNames(userContext))
            {
                if (taxonName.Category.Type.Id == (Int32)TaxonNameCategoryTypeId.CommonName && 
                    taxonName.IsRecommended && 
                    taxonName.Status.Id == (int)TaxonNameStatusId.ApprovedNaming &&
                    taxonName.ValidFromDate <= today &&
                    today <= taxonName.ValidToDate &&
                    taxonName.Category.Id != (Int32)TaxonNameCategoryId.SwedishName)
                {                    
                    otherLanguagesNames.Add(taxonName);
                }
            }

            return otherLanguagesNames;
        }

        /// <summary>
        /// Get common names that are not recommended and not removed. And also
        /// scientific names that are not approved and not removed.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        /// <param name="userContext">The user context.</param>
        /// <returns>Common names that are not recommended and not removed. And also
        /// scientific names that are not approved and not removed.</returns>
        public static List<ITaxonName> GetNotRecommendedNames(this ITaxon taxon, IUserContext userContext)
        {
            // todo - fungerar den här inne i en revision?            
            var notRecommendedNames = new List<ITaxonName>();
            DateTime today = DateTime.Now;
            foreach (ITaxonName taxonName in taxon.GetTaxonNames(userContext))
            {
                if (taxonName.Category.Type.Id == (Int32)TaxonNameCategoryTypeId.CommonName &&
                    taxonName.IsRecommended == false &&                    
                    taxonName.ValidFromDate <= today &&
                    today <= taxonName.ValidToDate && 
                    taxonName.Status.Id != (int)TaxonNameStatusId.Removed)
                {
                    notRecommendedNames.Add(taxonName);
                }
                else if (taxonName.Category.Type.Id == (Int32)TaxonNameCategoryTypeId.ScientificName &&
                    taxonName.IsRecommended == false &&
                    taxonName.ValidFromDate <= today &&
                    today <= taxonName.ValidToDate &&
                    taxonName.Status.Id != (int)TaxonNameStatusId.Removed && 
                    taxonName.Status.Id != (int)TaxonNameStatusId.ApprovedNaming)
                {
                    notRecommendedNames.Add(taxonName);
                }
            }

            return notRecommendedNames;
        }

        /// <summary>
        /// Gets the taxon anamorph name or null if it doesn't exist.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        /// <returns>The taxon anamorph name or null if it doesn't exist.</returns>
        public static ITaxonName GetAnamorphName(this ITaxon taxon)
        {
            ITaxonName name = null;
            foreach (ITaxonName taxonName in taxon.GetTaxonNames(CoreData.UserManager.GetCurrentUser()))
            {                
                if (taxonName.Category.Id == (Int32)TaxonNameCategoryId.AnamorphName)
                {
                    name = taxonName;
                    if (name.IsRecommended)
                    {
                        break;
                    }
                }
            }

            return name;
        }

        /// <summary>
        /// Converts a list of ITaxon to a list with TaxonViewModel which is used
        /// to present the Taxa on screen.
        /// </summary>
        /// <param name="taxa">The taxa.</param>
        /// <returns></returns>
        public static List<TaxonViewModel> ToTaxonViewModelList(this IEnumerable<ITaxon> taxa)
        {
            var list = new List<TaxonViewModel>();

            if (taxa != null)
            {
                foreach (ITaxon taxon in taxa)
                {
                    list.Add(TaxonViewModel.CreateFromTaxon(taxon));
                }
            }

            return list;
        }
    }
}
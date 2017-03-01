using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions
{
    /// <summary>
    /// Extension methods to ITaxon interface.
    /// </summary>
    public static class ITaxonExtension
    {      
        /// <summary>
        /// Get not approved taxon names and common names that are not recommended.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        /// <param name="userContext">The user context.</param>
        /// <returns>Not approved taxon names and common names that are not recommended.</returns>
        public static List<ITaxonName> GetOtherNames(
            this ITaxon taxon,
            IUserContext userContext)
        {
            DateTime today;
            List<ITaxonName> otherNames;

            otherNames = new List<ITaxonName>();
            today = DateTime.Now;
            foreach (ITaxonName taxonName in taxon.GetTaxonNames(userContext))
            {
                if ((!taxonName.IsRecommended ||
                     (taxonName.ValidFromDate > today) ||
                     (today > taxonName.ValidToDate)) &&
                    (taxonName.Category.Id == (Int32)TaxonNameCategoryId.SwedishName))
                {
                    otherNames.Add(taxonName);
                }
                else
                {
                    if (taxonName.Status.Id != (Int32)TaxonNameStatusId.ApprovedNaming)
                    {
                        otherNames.Add(taxonName);
                    }
                }
            }
            return otherNames;
        }

        ///// <summary>
        ///// Gets a taxons scientific and common name concatenated.
        ///// </summary>
        ///// <param name="taxon">The taxon.</param>
        ///// <returns>A taxons scientific and common name concatenated</returns>
        //public static string GetScientificAndCommonName(this ITaxon taxon)
        //{
        //    if (taxon.CommonName.IsEmpty() && taxon.ScientificName.IsEmpty())
        //        return "";
        //    if (taxon.CommonName.IsEmpty())
        //        return taxon.ScientificName.IsEmpty() ? taxon.ScientificName : "";
        //    else
        //        return string.Format("{0} ({1})", taxon.ScientificName, taxon.CommonName);
        //}

        ///// <summary>
        ///// Gets a taxons common name or the default value.
        ///// </summary>
        ///// <param name="taxon">The taxon.</param>
        ///// <param name="strDefault">The default value.</param>
        ///// <returns>A taxons common name or the default value.</returns>
        //public static string GetCommonNameOrDefault(this ITaxon taxon, string strDefault)
        //{
        //    return taxon.CommonName.IsNotEmpty() ? taxon.CommonName : strDefault;
        //}

        ///// <summary>
        ///// Gets a string with taxons all synonyms separated by ;.
        ///// </summary>
        ///// <param name="taxon">The taxon.</param>
        ///// <returns>a string with taxons all synonyms separated by ;.</returns>
        //public static string GetScientificSynonymsString(this ITaxon taxon)
        //{
        //    if (taxon == null || taxon.GetSynonyms(CoreData.UserManager.GetCurrentUser()) == null)
        //        return "";
        //    var names = new StringBuilder();

        //    foreach (var taxonName in taxon.GetSynonyms(CoreData.UserManager.GetCurrentUser()))
        //    {
        //        if (names.Length > 0)
        //        {
        //            names.Append("; ");
        //        }

        //        names.Append(taxonName.Name);

        //        if (!string.IsNullOrEmpty(taxonName.Author))
        //        {
        //            names.Append(" ");
        //            names.Append(taxonName.Author);
        //        }
        //    }
        //    return names.ToString();
        //}

        ///// <summary>
        ///// Determines whether the taxon is Biota (life), or not.
        ///// </summary>
        ///// <param name="taxon">The taxon.</param>
        ///// <returns>True if the taxon is Biota, and otherwise false.</returns>        
        //public static bool IsLifeTaxon(this ITaxon taxon)
        //{
        //    return taxon.Id == 0;
        //}

        ///// <summary>
        ///// Gets a string with all parents to a taxon separated by ;.
        ///// </summary>
        ///// <param name="taxon">The taxon.</param>
        ///// <param name="userContext">The user context.</param>
        ///// <returns>A string with all parents to a taxon separated by ;.</returns>
        //public static string GetParentTaxaString(this ITaxon taxon, IUserContext userContext)
        //{
        //    if (taxon == null)
        //        return "";
        //    var parents = new StringBuilder();
        //    var parentRelations = taxon.GetAllParentTaxonRelations(userContext, null, false, false);
        //    foreach (var taxonRelation in parentRelations)
        //    {
        //        var relatedTaxon = taxonRelation.ParentTaxon;
        //        if (parents.Length > 0)
        //        {
        //            parents.Append("; ");
        //        }

        //        parents.Append(relatedTaxon.Category.Name);
        //        parents.Append(": ");
        //        parents.Append(relatedTaxon.GetLabel());
        //    }

        //    return parents.ToString();
        //}

        ///// <summary>
        ///// Get taxon names where taxon name category type 
        ///// equals IDENTIFIER.
        ///// </summary>
        ///// <param name="taxon">The taxon.</param>
        ///// <param name="userContext">The user context.</param>
        ///// <returns>
        ///// Taxon names where taxon name category type 
        ///// equals IDENTIFIER.
        ///// </returns>
        //public static List<ITaxonName> GetAllIdentifiers(this ITaxon taxon, IUserContext userContext)
        //{
        //    var identifiers = new List<ITaxonName>();
        //    foreach (ITaxonName taxonName in taxon.GetTaxonNames(userContext))
        //    {
        //        if (taxonName.NameCategory.Type == TaxonNameCategoryType.Identifier)
        //        {
        //            identifiers.Add(taxonName);
        //        }
        //    }
        //    return identifiers;
        //}

        ///// <summary>
        ///// Get common names that are recommended and approved, but not swedish.
        ///// </summary>
        ///// <param name="taxon">The taxon.</param>
        ///// <param name="userContext">The user context.</param>
        ///// <returns>Common names that are recommended and approved, but not swedish.</returns>
        //public static List<ITaxonName> GetOtherLanguagesNames(this ITaxon taxon, IUserContext userContext)
        //{
        //    // todo - fungerar den här inne i en revision?
        //    var otherLanguagesNames = new List<ITaxonName>();
        //    DateTime today = DateTime.Now;
        //    foreach (ITaxonName taxonName in taxon.GetTaxonNames(userContext))
        //    {

        //        if (taxonName.NameCategory.Type == TaxonNameCategoryType.CommonName &&
        //            taxonName.IsRecommended &&
        //            taxonName.Status.Id == (int)TaxonNameStatusId.ApprovedNaming &&
        //            taxonName.ValidFromDate <= today &&
        //            today <= taxonName.ValidToDate &&
        //            taxonName.NameCategory.Id != (Int32)TaxonNameCategoryId.SwedishName
        //            )
        //        {
        //            otherLanguagesNames.Add(taxonName);
        //        }
        //    }

        //    return otherLanguagesNames;
        //}

        ///// <summary>
        ///// Get common names that are not recommended and not removed. And also
        ///// scientific names that are not approved and not removed.
        ///// </summary>
        ///// <param name="taxon">The taxon.</param>
        ///// <param name="userContext">The user context.</param>
        ///// <returns>Common names that are not recommended and not removed. And also
        ///// scientific names that are not approved and not removed.</returns>
        //public static List<ITaxonName> GetNotRecommendedNames(this ITaxon taxon, IUserContext userContext)
        //{
        //    // todo - fungerar den här inne i en revision?            
        //    var notRecommendedNames = new List<ITaxonName>();
        //    DateTime today = DateTime.Now;
        //    foreach (ITaxonName taxonName in taxon.GetTaxonNames(userContext))
        //    {
        //        if (taxonName.NameCategory.Type == TaxonNameCategoryType.CommonName &&
        //            taxonName.IsRecommended == false &&
        //            taxonName.ValidFromDate <= today &&
        //            today <= taxonName.ValidToDate &&
        //            taxonName.Status.Id != (int)TaxonNameStatusId.Removed
        //            )
        //        {
        //            notRecommendedNames.Add(taxonName);
        //        }
        //        else if (taxonName.NameCategory.Type == TaxonNameCategoryType.ScientificName &&
        //            taxonName.IsRecommended == false &&
        //            taxonName.ValidFromDate <= today &&
        //            today <= taxonName.ValidToDate &&
        //            taxonName.Status.Id != (int)TaxonNameStatusId.Removed &&
        //            taxonName.Status.Id != (int)TaxonNameStatusId.ApprovedNaming
        //            )
        //        {
        //            notRecommendedNames.Add(taxonName);
        //        }

        //    }

        //    return notRecommendedNames;
        //}

        ///// <summary>
        ///// Gets the taxon anamorph name or null if it doesn't exist.
        ///// </summary>
        ///// <param name="taxon">The taxon.</param>
        ///// <returns>The taxon anamorph name or null if it doesn't exist.</returns>
        //public static ITaxonName GetAnamorphName(this ITaxon taxon)
        //{
        //    ITaxonName name = null;
        //    foreach (ITaxonName taxonName in taxon.GetTaxonNames(CoreData.UserManager.GetCurrentUser()))
        //    {

        //        if (taxonName.NameCategory.Id == (Int32)TaxonNameCategoryId.AnamorphName)
        //        {
        //            name = taxonName;
        //            if (name.IsRecommended)
        //                break;
        //        }
        //    }
        //    return name;
        //}

        /// <summary>
        /// Converts an ITaxon to a TaxonViewModel which is used
        /// to present the Taxon on screen.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        /// <returns></returns>
        public static TaxonViewModel ToTaxonViewModel(this ITaxon taxon)
        {
            return TaxonViewModel.CreateFromTaxon(taxon);
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

        public static List<TaxonSpeciesObservationCountViewModel> ToTaxonSpeciesObservationCountViewModelList(
            this IEnumerable<ITaxonSpeciesObservationCount> taxa)
        {
            var list = new List<TaxonSpeciesObservationCountViewModel>();

            if (taxa != null)
            {
                foreach (ITaxonSpeciesObservationCount taxonSpeciesObservationCount in taxa)
                {
                    list.Add(TaxonSpeciesObservationCountViewModel.CreateFromTaxon(taxonSpeciesObservationCount));
                }
            }
            return list;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions
{
    using Taxon = ArtDatabanken.Data.Taxon;

    /// <summary>
    /// Extension methods to ITaxonManager interface.
    /// </summary>
    public static class ITaxonManagerExtension
    {
        // Category Id
        private const int RANKLESS = 52;

        /// <summary>
        /// Get all taxon categories that could be used for a taxon.
        /// </summary>
        /// <param name="taxonManager">The taxon manager.</param>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">Taxon.</param>
        /// <returns>All  taxon categories that could be used for a taxon.</returns>
        public static IList<ITaxonCategory> GetPossibleTaxonCategories(
            this ITaxonManager taxonManager,
            IUserContext userContext,
            ITaxon taxon)
        {
            IList<ITaxonCategory> possibleCategories = taxonManager.GetTaxonCategories(userContext);
            IList<ITaxonCategory> originalList = new List<ITaxonCategory>(possibleCategories);

            // The taxon that we will search categories for
            ITaxon currentTaxon = taxon;

            // If taxon.Category = RANKLESS
            // Find nearest parent that is not RANKLESS and set the currentTaxon object to the parent taxon we found.
            while (currentTaxon.Category.Id == 52)
            {
                foreach (ITaxonRelation parent in currentTaxon.GetNearestParentTaxonRelations(userContext))
                {
                    if (parent.IsMainRelation == true)
                    {
                        currentTaxon = parent.ParentTaxon;
                    }
                }
            }

            foreach (ITaxonRelation parent in currentTaxon.GetNearestParentTaxonRelations(userContext))
            {
                possibleCategories = (from category in possibleCategories where category.SortOrder > parent.ChildTaxon.GetCheckedOutChangesTaxonProperties(userContext).TaxonCategory.SortOrder select category).ToList();

                var genusCategory = taxonManager.GetTaxonCategory(userContext, 14);
                if (parent.ChildTaxon.GetCheckedOutChangesTaxonProperties(userContext).TaxonCategory.SortOrder < genusCategory.SortOrder)
                {
                    // remove all with SortOrder higher than Genus.SortOrder
                    possibleCategories = (from category in possibleCategories where category.SortOrder <= genusCategory.SortOrder select category).ToList();
                }
            }
            foreach (var possibleCategory in originalList)
            {
                if (!possibleCategory.IsTaxonomic && !possibleCategories.Contains(possibleCategory))
                {
                    possibleCategories.Add(possibleCategory);
                }
                // Add category RANKLESS 
                if (possibleCategory.Id == RANKLESS && !possibleCategories.Contains(possibleCategory))
                {
                    possibleCategories.Add(possibleCategory);
                }
            }

            return possibleCategories;
        }
    }
}

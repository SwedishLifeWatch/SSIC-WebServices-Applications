using System.Collections.Generic;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    public class SearchViewModel
    {
        /// <summary>
        /// Is RedList Categories Enabled.
        /// </summary>
        public bool IsRedListCategoriesEnabled { get; set; }

        /// <summary>
        /// Contains list of red list categories.
        /// </summary>
        public List<RedListCategoryItemViewModel> RedListCategories { get; set; }

        /// <summary>
        /// Title for red list categories.
        /// </summary>
        public string RedListCategoriesTitle { get; set; }

        /// <summary>
        /// Is Swedish occurrence enabled
        /// </summary>
        public bool IsSwedishOccurrenceEnabled { get; set; }

        /// <summary>
        /// Contains list of Swedish occurence types (for future use)
        /// </summary>
        public IEnumerable<RedListSwedishOccurrenceItemViewModel> SwedishOccurrences { get; set; }

        /// <summary>
        /// Title for the swedish occurence
        /// </summary>
        public string SwedishOccurrenceTitle { get; set; }

        /// <summary>
        /// Is RedList Taxon Type Enabled.
        /// </summary>
        public bool IsRedListTaxonTypeEnabled { get; set; }

        /// <summary>
        /// Contains list of red list Taxon Types.
        /// </summary>
        public IEnumerable<RedListTaxonTypeItemViewModel> RedListTaxonTypes { get; set; }

        /// <summary>
        /// Title for red list taxon types.
        /// </summary>
        public string RedListTaxonTypesTitle { get; set; }

        /// <summary>
        /// Is RedList Taxon Type Enabled.
        /// </summary>
        public bool IsTaxonScopeEnabled { get; set; }

        /// <summary>
        /// Contains list of the scope forTaxon.
        /// </summary>
        public IEnumerable<TaxonScopeItemViewModel> TaxonScope { get; set; }

        /// <summary>
        /// Is Organism Groups Enabled.
        /// </summary>
        public bool IsOrganismGroupsEnabled { get; set; }

        /// <summary>
        /// Contains list of Organism Groups.
        /// </summary>
        public IEnumerable<RedListOrganismGroupItemViewModel> OrganismGroups { get; set; }

        /// <summary>
        /// Is Landscape Types Enabled.
        /// </summary>
        public bool IsLandscapeTypesEnabled { get; set; }

        /// <summary>
        /// Landscape occurrence.
        /// </summary>
        public LogicalOperator LandscapeOccurence { get; set; }

        /// <summary>
        /// Contains list of Landscape Types.
        /// </summary>
        public IEnumerable<RedListLandscapeTypeItemViewModel> LandscapeTypes { get; set; }

        /// <summary>
        /// Title for landscape types.
        /// </summary>
        public string LandscapeTypesTitle { get; set; }

        /// <summary>
        /// Is Landscape Types Important.
        /// </summary>
        public bool IsLandscapeTypesImportant { get; set; }

        /// <summary>
        /// Is County Occurrence Enabled.
        /// </summary>
        public bool IsCountyOccurrenceEnabled { get; set; }

        /// <summary>
        /// Contains list of County Occurrence factors.
        /// </summary>
        public IEnumerable<RedListCountyOccurrenceItemViewModel> CountyOccurrences { get; set; }

        /// <summary>
        /// Title for county occurrences.
        /// </summary>
        public string CountyOccurrencesTitle { get; set; }

        /// <summary>
        /// Is biotope Enabled.
        /// </summary>
        public bool IsBiotopeEnabled { get; set; }

        /// <summary>
        /// Contains list of biotope factors.
        /// </summary>
        public IEnumerable<RedListBiotopeItemViewModel> Biotopes { get; set; }

        /// <summary>
        /// Title for biotopes.
        /// </summary>
        public string BiotopesTitle { get; set; }

        /// <summary>
        /// Is biotope important.
        /// </summary>
        public bool IsBiotopeImportant { get; set; }

        /// <summary>
        /// Biotope operator.
        /// </summary>
        public LogicalOperator BiotopeOperator { get; set; }

        /// <summary>
        /// Is substrate enabled.
        /// </summary>
        public bool IsSubstrateEnabled { get; set; }

        /// <summary>
        /// Contains list of substrate factors.
        /// </summary>
        public IEnumerable<RedListSubstrateItemViewModel> Substrates { get; set; }

        /// <summary>
        /// Title for substrates.
        /// </summary>
        public string SubstratesTitle { get; set; }

        /// <summary>
        /// Is substrate important.
        /// </summary>
        public bool IsSubstrateImportant { get; set; }

        /// <summary>
        /// Substrate operator.
        /// </summary>
        public LogicalOperator SubstrateOperator { get; set; }

        /// <summary>
        /// Is impact enabled.
        /// </summary>
        public bool IsImpactEnabled { get; set; }

        /// <summary>
        /// Contains list of impact factors.
        /// </summary>
        public IEnumerable<RedListImpactItemViewModel> Impacts { get; set; }

        /// <summary>
        /// Title for impacts.
        /// </summary>
        public string ImpactsTitle { get; set; }

        /// <summary>
        /// Restrict the search to the species for which
        /// the change has large impact.
        /// </summary>    
        public bool LargeImpactOnly { get; set; }

        /// <summary>
        /// Restrict the search to the species for which
        /// the change has positive impact.
        /// If this property is false only species for which
        /// the change has negative impact are included.
        /// </summary>    
        public bool PositiveImpact { get; set; }

        /// <summary>
        /// Impact operator.
        /// </summary>
        public LogicalOperator ImpactOperator { get; set; }

        /// <summary>
        /// Is life form enabled.
        /// </summary>
        public bool IsLifeFormEnabled { get; set; }

        /// <summary>
        /// Contains list of life form factors.
        /// </summary>
        public IEnumerable<RedListLifeFormItemViewModel> LifeForms { get; set; }

        /// <summary>
        /// Title of life forms.
        /// </summary>
        public string LifeFormsTitle { get; set; }

        /// <summary>
        /// Is host enabled.
        /// </summary>
        public bool IsHostEnabled { get; set; }

        /// <summary>
        /// Is host important.
        /// </summary>
        public bool IsHostImportant { get; set; }

        /// <summary>
        /// Host operator.
        /// </summary>
        public LogicalOperator HostOperator { get; set; }

        /// <summary>
        /// Contains list of host factors.
        /// </summary>
        public IEnumerable<RedListHostItemViewModel> Hosts { get; set; }

        /// <summary>
        /// Is thematic list enabled.
        /// </summary>
        public bool IsThematicListEnabled { get; set; }

        /// <summary>
        /// Thematic list operator.
        /// </summary>
        public LogicalOperator ThematicListOperator { get; set; }

        /// <summary>
        /// Contains thematic list factors.
        /// </summary>
        public IEnumerable<RedListThematicListItemViewModel> ThematicLists { get; set; }

        /// <summary>
        /// Is taxon children search.
        /// </summary>
        public bool IsTaxonChildrenSearch { get; set; }

        /// <summary>
        /// Taxon Id search if IsTaxonChildrenSearch is true.
        /// </summary>
        public int? TaxonId { get; set; }

        /// <summary>
        /// If stringsearch then this is the string.
        /// </summary>
        public string SearchString { get; set; }

        /// <summary>
        /// string to store common name
        /// </summary>
        public string CommonName { get; set; }

        /// <summary>
        /// Indicates if a category search is made
        /// </summary>
        public bool IsRedListCategorySearch { get; set; }

        /// <summary>
        /// RedListCategoryToUseInSearch
        /// </summary>
        public int? RedListCategorySearchId { get; set; }
    }
}

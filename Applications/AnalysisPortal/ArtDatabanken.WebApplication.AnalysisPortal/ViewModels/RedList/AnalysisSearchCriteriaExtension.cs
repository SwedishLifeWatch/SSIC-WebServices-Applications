using System.Collections.Generic;
using System.Linq;
using ArtDatabanken;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    /// <summary>
    /// Contains extension methods to the AnalysisSearchCriteria class.
    /// </summary>
    public static class AnalysisSearchCriteriaExtension
    {
        ///// <summary>
        ///// Init Biotopes information.
        ///// </summary>
        ///// <param name="searchCriteria">The search criteria.</param>
        ///// <param name="model">The view model stored in session.</param>
        //public static void InitBiotopeInformation(this AnalysisSearchCriteria searchCriteria, SearchViewModel model)
        //{
        //    if (model.IsBiotopeEnabled && model.Biotopes.Any(r => r.Selected))
        //    {
        //        searchCriteria.Biotopes = new BiotopeSearchCriteria
        //        {
        //            ImportantOnly = model.IsBiotopeImportant,
        //            Operator = model.BiotopeOperator,
        //            BiotopeIds = new List<int>()
        //        };
        //        foreach (var biotope in model.Biotopes)
        //        {
        //            if (biotope.Selected)
        //            {
        //                searchCriteria.Biotopes.BiotopeIds.Add(biotope.Id);
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// Init SwedishOccurrence information
        ///// </summary>
        ///// <param name="searchCriteria"></param>
        ///// <param name="model"></param>
        //public static void InitSwedishOccurrenceInformation(this AnalysisSearchCriteria searchCriteria, SearchViewModel model)
        //{
        //    if (model.IsSwedishOccurrenceEnabled && model.SwedishOccurrences.Any(r => r.Selected))
        //    {
        //        searchCriteria.SwedishOccurrence = new List<int>();
        //        foreach (var swedishOccurrence in model.SwedishOccurrences)
        //        {
        //            if (swedishOccurrence.Selected)
        //            {
        //                searchCriteria.SwedishOccurrence.Add(swedishOccurrence.Id);
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// Init CountyOccurrence information.
        ///// </summary>
        ///// <param name="searchCriteria">The search criteria.</param>
        ///// <param name="model">The view model stored in session.</param>
        //public static void InitCountyOccurrencesInformation(this AnalysisSearchCriteria searchCriteria, SearchViewModel model)
        //{
        //    if (model.IsCountyOccurrenceEnabled && model.CountyOccurrences.Any(r => r.Selected))
        //    {
        //        searchCriteria.CountyOccurrence = new List<int>();
        //        foreach (var countyOccurrence in model.CountyOccurrences)
        //        {
        //            if (countyOccurrence.Selected)
        //            {
        //                searchCriteria.CountyOccurrence.Add(countyOccurrence.Id);
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// Init thematic list information.
        ///// </summary>
        ///// <param name="searchCriteria">The search criteria.</param>
        ///// <param name="model">The view model stored in session.</param>
        //public static void InitThematicListInformation(this AnalysisSearchCriteria searchCriteria, SearchViewModel model)
        //{
        //    if (model.IsThematicListEnabled && model.ThematicLists.Any(r => r.Selected))
        //    {
        //        searchCriteria.ThematicLists = new ThematicListSearchCriteria
        //        {
        //            Operator = model.ThematicListOperator,
        //            ThematicListIds = new List<int>()
        //        };
        //        foreach (var thematicList in model.ThematicLists)
        //        {
        //            if (thematicList.Selected)
        //            {
        //                searchCriteria.ThematicLists.ThematicListIds.Add(thematicList.Id);
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Init RedListCategories information.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <param name="model">The view model stored in session.</param>
        public static void InitRedListCategories(this AnalysisSearchCriteria searchCriteria, SearchViewModel model)
        {
            if (model.IsRedListCategoriesEnabled && model.RedListCategories.Any(r => r.Selected))
            {
                searchCriteria.RedListCategories = new List<int>();
                foreach (var redListCategory in model.RedListCategories)
                {
                    if (redListCategory.Selected)
                    {
                        searchCriteria.RedListCategories.Add(redListCategory.OrderNumber);
                    }
                }
            }
        }

    //    /// <summary>
    //    /// Init RedListTaxonCategories information.
    //    /// </summary>
    //    /// <param name="searchCriteria">The search criteria.</param>
    //    /// <param name="model">The view model stored in session.</param>
    //    public static void InitRedListTaxonCategories(this AnalysisSearchCriteria searchCriteria, SearchViewModel model)
    //    {
    //        if (model.IsRedListTaxonTypeEnabled && model.RedListTaxonTypes.Any(r => r.Selected))
    //        {
    //            searchCriteria.TaxonCategories = new List<string>();
    //            foreach (var redListTaxonType in model.RedListTaxonTypes)
    //            {
    //                if (redListTaxonType.Selected)
    //                {
    //                    searchCriteria.TaxonCategories.Add(redListTaxonType.ClassDefinitionTextValue);
    //                }
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Init RedListTaxonCategories information.
    //    /// </summary>
    //    /// <param name="searchCriteria">The search criteria.</param>
    //    /// <param name="model">The view model stored in session.</param>
    //    public static void InitTaxonScope(this AnalysisSearchCriteria searchCriteria, SearchViewModel model)
    //    {
    //        // Check if "Underarter" mfl is selected if so show the rest of the species belonging to taxon categories. TODO this check cah
    //        if (model.RedListTaxonTypes.IsNull() || !model.RedListTaxonTypes.Any(r => r.Selected))
    //        {
    //            // TODO in the future taxon scope might be added here as an own setting then this check is required instead
    //            // model.IsTaxonScopeEnabled && model.TaxonScope.Any(r => r.Selected)
    //            // Must get taxa from scope if no data is recived.
    //            if (model.TaxonScope.IsNull())
    //            {
    //                searchCriteria.TaxonScope = new TaxonScopeSearchCriteria
    //                {
    //                    UseAllAdditionalCategories = true
    //                };
    //            }
    //            else
    //            {
    //                var usedCategories = model.TaxonScope.Select(taxonItem => taxonItem.KeyId).ToList();

    //                searchCriteria.TaxonScope = new TaxonScopeSearchCriteria
    //                {
    //                    CategoryIds = usedCategories,
    //                    UseAllAdditionalCategories = false
    //                };
    //            }

    //        }
    //    }

    //    /// <summary>
    //    /// Init OrganismGroups information.
    //    /// </summary>
    //    /// <param name="searchCriteria">The search criteria.</param>
    //    /// <param name="model">The view model stored in session.</param>
    //    public static void InitOrganismGroups(this AnalysisSearchCriteria searchCriteria, SearchViewModel model)
    //    {
    //        if (model.IsOrganismGroupsEnabled && model.OrganismGroups.Any(r => r.Selected))
    //        {
    //            searchCriteria.OrganismGroups = new List<int>();
    //            foreach (var organismGroup in model.OrganismGroups)
    //            {
    //                if (organismGroup.Selected)
    //                {
    //                    searchCriteria.OrganismGroups.Add(organismGroup.Id);
    //                }
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Init LandscapeTypes information.
    //    /// </summary>
    //    /// <param name="searchCriteria">The search criteria.</param>
    //    /// <param name="model">The view model stored in session.</param>
    //    public static void InitLandscapeTypes(this AnalysisSearchCriteria searchCriteria, SearchViewModel model)
    //    {
    //        if (model.IsLandscapeTypesEnabled && model.LandscapeTypes.Any(r => r.Selected))
    //        {
    //            searchCriteria.LandscapeTypes = new LandscapeTypeSearchCriteria
    //            {
    //                LandscapeTypeIds = new List<int>(),
    //                Operator = model.LandscapeOccurence
    //            };
    //            if (model.IsLandscapeTypesImportant)
    //            {
    //                searchCriteria.LandscapeTypes.IncludeImportant = true;
    //                searchCriteria.LandscapeTypes.IncludePresent = false;
    //            }
    //            else
    //            {
    //                searchCriteria.LandscapeTypes.IncludeImportant = true;
    //                searchCriteria.LandscapeTypes.IncludePresent = true;
    //            }

    //            foreach (var landscapeType in model.LandscapeTypes)
    //            {
    //                if (landscapeType.Selected)
    //                {
    //                    searchCriteria.LandscapeTypes.LandscapeTypeIds.Add(landscapeType.Id);
    //                }
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Init Substrate information.
    //    /// </summary>
    //    /// <param name="searchCriteria">The search criteria.</param>
    //    /// <param name="model">The view model stored in session.</param>
    //    public static void InitSubstrate(this AnalysisSearchCriteria searchCriteria, SearchViewModel model)
    //    {
    //        if (model.IsSubstrateEnabled && model.Substrates.Any(r => r.Selected))
    //        {
    //            searchCriteria.Substrate = new SubstrateSearchCriteria
    //            {
    //                ImportantOnly = model.IsSubstrateImportant,
    //                Operator = model.SubstrateOperator,
    //                SubstrateIds = new List<int>()
    //            };
    //            foreach (var substrate in model.Substrates)
    //            {
    //                if (substrate.Selected)
    //                {
    //                    searchCriteria.Substrate.SubstrateIds.Add(substrate.Id);
    //                }
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Init Impact information.
    //    /// </summary>
    //    /// <param name="searchCriteria">The search criteria.</param>
    //    /// <param name="model">The view model stored in session.</param>
    //    public static void InitImpact(this AnalysisSearchCriteria searchCriteria, SearchViewModel model)
    //    {
    //        if (model.IsImpactEnabled && model.Impacts.Any(r => r.Selected))
    //        {
    //            searchCriteria.Impact = new ImpactSearchCriteria
    //            {
    //                LargeImpactOnly = model.LargeImpactOnly,
    //                PositiveImpact = model.PositiveImpact,
    //                Operator = model.ImpactOperator,
    //                ImpactIds = new List<int>()
    //            };
    //            foreach (var impact in model.Impacts)
    //            {
    //                if (impact.Selected)
    //                {
    //                    searchCriteria.Impact.ImpactIds.Add(impact.Id);
    //                }
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Init LifeForms information.
    //    /// </summary>
    //    /// <param name="searchCriteria">The search criteria.</param>
    //    /// <param name="model">The view model stored in session.</param>
    //    public static void InitLifeForms(this AnalysisSearchCriteria searchCriteria, SearchViewModel model)
    //    {
    //        if (model.IsLifeFormEnabled && model.LifeForms.Any(r => r.Selected))
    //        {
    //            searchCriteria.LifeForms = new List<int>();
    //            foreach (var lifeForm in model.LifeForms)
    //            {
    //                if (lifeForm.Selected)
    //                {
    //                    searchCriteria.LifeForms.Add(lifeForm.Id);
    //                }
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Init Host information.
    //    /// </summary>
    //    /// <param name="searchCriteria">The search criteria.</param>
    //    /// <param name="model">The view model stored in session.</param>
    //    public static void InitHost(this AnalysisSearchCriteria searchCriteria, SearchViewModel model)
    //    {

    //        if (model.IsHostEnabled)
    //        {
    //            searchCriteria.Host = new HostSearchCriteria
    //            {
    //                HostIds = new List<int>(),
    //                Operator = model.HostOperator,
    //                ImportantOnly = model.IsHostImportant
    //            };

    //            // Check superior host taxa and check children if selected. 
    //            foreach (RedListHostItemViewModel host in model.Hosts)
    //            {
    //                if (host.Selected)
    //                {
    //                    searchCriteria.Host.HostIds.Add(host.Id);
    //                }
    //                else if (host.Children.IsNotEmpty())
    //                {
    //                    foreach (RedListHostItemViewModel child in host.Children.Where(child => child.Selected))
    //                    {
    //                        searchCriteria.Host.HostIds.Add(child.Id);
    //                    }
    //                }
    //            }
    //        }

    //        //if (model.IsHostEnabled && model.Hosts.Any(r => r.Selected))
    //        //{
    //        //    searchCriteria.Host = new HostSearchCriteria();
    //        //    searchCriteria.Host.HostIds = new List<Int32>();
    //        //    searchCriteria.Host.Operator = model.HostOperator;
    //        //    searchCriteria.Host.ImportantOnly = model.IsHostImportant;
    //        //    foreach (var host in model.Hosts)
    //        //    {
    //        //        if (host.Selected)
    //        //        {
    //        //            searchCriteria.Host.HostIds.Add(host.Id);
    //        //        }
    //        //    }
    //        //}
    //    }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    /// <summary>
    /// Contains extension methods to the TaxonSpeciesFactViewModel class.
    /// </summary>
    public static class SearchViewModelExtension
    {
        /// <summary>
        /// The user.
        /// </summary>
        private static IUserContext mUser;

        /// <summary>
        /// The view model stored in session.
        /// </summary>
        private static SearchViewModel mSearchViewModelFromSession;

        /// <summary>
        /// Constructor for this extension class.
        /// </summary>
        /// <param name="searchViewModel">The view model.</param>
        /// <param name="user">The user.</param>
        /// <param name="searchViewModelFromSession">The view model stored in session.</param>
        public static void InitSearchViewModel(this SearchViewModel searchViewModel, IUserContext user, SearchViewModel searchViewModelFromSession)
        {
            mUser = user;
            mSearchViewModelFromSession = searchViewModelFromSession;
        }

        //public static void InitSwedishOccurrenceInformation(this SearchViewModel searchViewModel)
        //{
        //    if (mSearchViewModelFromSession.IsNotNull() && mSearchViewModelFromSession.SwedishOccurrences.IsNotNull())
        //    {
        //        searchViewModel.IsSwedishOccurrenceEnabled = mSearchViewModelFromSession.IsSwedishOccurrenceEnabled;
        //        searchViewModel.SwedishOccurrenceTitle = mSearchViewModelFromSession.SwedishOccurrenceTitle;
        //        searchViewModel.SwedishOccurrences = mSearchViewModelFromSession.SwedishOccurrences;
        //    }
        //    else
        //    {
        //        searchViewModel.SwedishOccurrences = new List<RedListSwedishOccurrenceItemViewModel>();

        //        // Get swedish occurrence factor
        //        FactorList swedishOccurrenceFactors = SwedishOccurrenceCache.GetFactors(mUser);

        //        // Populate swedish occurrence
        //        foreach (var swedishOccurrenceFactor in swedishOccurrenceFactors)
        //        {
        //            foreach (var swedishOccurrenceItem in swedishOccurrenceFactor.DataType.Field1.Enum.Values)
        //            {
        //                // The level of swedish occurrence is defined in the appsettings
        //                if (swedishOccurrenceItem.KeyInt.HasValue && swedishOccurrenceItem.KeyInt.Value > AppSettings.Default.SwedishOccurrenceExist)
        //                {
        //                    var redListSwedishOccurrenceItemViewModel = new RedListSwedishOccurrenceItemViewModel
        //                    {
        //                        Id = swedishOccurrenceItem.KeyInt.Value,
        //                        Name = swedishOccurrenceItem.OriginalLabel,
        //                        Selected = true,
        //                    };
        //                    searchViewModel.SwedishOccurrences.Add(redListSwedishOccurrenceItemViewModel);
        //                }
        //            }
        //        }

        //        // Set swedish occurrences to true as default
        //        searchViewModel.IsSwedishOccurrenceEnabled = true;
        //    }
        //}

        ///// <summary>
        ///// Init Biotopes information.
        ///// </summary>
        ///// <param name="searchViewModel">Search criteria view model.</param>
        //public static void InitBiotopeInformation(this SearchViewModel searchViewModel)
        //{
        //    if (mSearchViewModelFromSession.IsNotNull() && mSearchViewModelFromSession.Biotopes.IsNotNull())
        //    {
        //        searchViewModel.IsBiotopeEnabled = mSearchViewModelFromSession.IsBiotopeEnabled;
        //        searchViewModel.IsBiotopeImportant = mSearchViewModelFromSession.IsBiotopeImportant;
        //        searchViewModel.BiotopeOperator = mSearchViewModelFromSession.BiotopeOperator;
        //        searchViewModel.Biotopes = mSearchViewModelFromSession.Biotopes;
        //        searchViewModel.BiotopesTitle = mSearchViewModelFromSession.BiotopesTitle;
        //    }
        //    else
        //    {
        //        searchViewModel.Biotopes = new List<RedListBiotopeItemViewModel>();

        //        // Get superior factor for biotopes.
        //        IFactor superiorFactor = CoreData.FactorManager.GetFactor(mUser, FactorId.Biotopes);

        //        // Get biotope factors
        //        FactorList biotopeFactors = BiotopeCache.GetFactors(mUser);

        //        // Set title for biotopes, from factor.
        //        searchViewModel.BiotopesTitle = superiorFactor.Label;

        //        // Populate biotopes
        //        foreach (var biotopeFactor in biotopeFactors)
        //        {
        //            var redListBiotopeItemViewModel = new RedListBiotopeItemViewModel
        //            {
        //                Id = biotopeFactor.Id,
        //                Name = biotopeFactor.Name
        //            };

        //            searchViewModel.Biotopes.Add(redListBiotopeItemViewModel);
        //        }

        //        searchViewModel.BiotopeOperator = LogicalOperator.Or;
        //    }
        //}

        ///// <summary>
        ///// Init CountyOccurrences information.
        ///// </summary>
        ///// <param name="searchViewModel">Search criteria view model.</param>
        //public static void InitCountyOccurrencesInformation(this SearchViewModel searchViewModel)
        //{
        //    if (mSearchViewModelFromSession.IsNotNull() && mSearchViewModelFromSession.CountyOccurrences.IsNotNull())
        //    {
        //        searchViewModel.IsCountyOccurrenceEnabled = mSearchViewModelFromSession.IsCountyOccurrenceEnabled;
        //        searchViewModel.CountyOccurrences = mSearchViewModelFromSession.CountyOccurrences;
        //        searchViewModel.CountyOccurrencesTitle = mSearchViewModelFromSession.CountyOccurrencesTitle;
        //    }
        //    else
        //    {
        //        searchViewModel.CountyOccurrences = new List<RedListCountyOccurrenceItemViewModel>();

        //        // Get superior factor, for county occurrence.
        //        IFactor superiorFactor = CoreData.FactorManager.GetFactor(mUser, FactorId.CountyOccurrence);

        //        // Get county occurrence factors
        //        FactorList countyOccurenceFactors = CountyOccurrenceCache.GetFactors(mUser);

        //        // Set title for county occurrence, from factor.
        //        searchViewModel.CountyOccurrencesTitle = superiorFactor.Label;

        //        // Populate county occurrence
        //        foreach (var countyOccurenceFactor in countyOccurenceFactors)
        //        {
        //            var redListCountyOccurrenceItemViewModel = new RedListCountyOccurrenceItemViewModel
        //            {
        //                Id = countyOccurenceFactor.Id,
        //                Name = countyOccurenceFactor.Name
        //            };

        //            searchViewModel.CountyOccurrences.Add(redListCountyOccurrenceItemViewModel);
        //        }
        //    }
        //}

        ///// <summary>
        ///// Init ThematicLists information.
        ///// </summary>
        ///// <param name="searchViewModel">Search criteria view model.</param>
        //public static void InitThematicListInformation(this SearchViewModel searchViewModel)
        //{
        //    searchViewModel.ThematicLists = new List<RedListThematicListItemViewModel>();
        //    if (mSearchViewModelFromSession.IsNotNull() && mSearchViewModelFromSession.ThematicLists.IsNotNull())
        //    {
        //        searchViewModel.IsThematicListEnabled = mSearchViewModelFromSession.IsThematicListEnabled;
        //        searchViewModel.ThematicListOperator = mSearchViewModelFromSession.ThematicListOperator;
        //        searchViewModel.ThematicLists = mSearchViewModelFromSession.ThematicLists;
        //    }
        //    else
        //    {
        //        foreach (ThematicListEnum @enum in Enum.GetValues(typeof(ThematicListEnum)).Cast<ThematicListEnum>())
        //        {
        //            var redListThematicListItemViewModel = new RedListThematicListItemViewModel();
        //            switch (@enum)
        //            {
        //                case ThematicListEnum.ContinuousDecline:
        //                    redListThematicListItemViewModel.Id = (int)@enum;
        //                    redListThematicListItemViewModel.Name = RedListResource.TaxonSearchThematicListContinuousDecline;
        //                    break;
        //                case ThematicListEnum.ExtremeFluctuations:
        //                    redListThematicListItemViewModel.Id = (int)@enum;
        //                    redListThematicListItemViewModel.Name = RedListResource.TaxonSearchThematicListExtremeFluctuations;
        //                    break;
        //                case ThematicListEnum.SevereleyFragmented:
        //                    redListThematicListItemViewModel.Id = (int)@enum;
        //                    redListThematicListItemViewModel.Name = RedListResource.TaxonSearchThematicListSeverelyFragmented;
        //                    break;
        //                case ThematicListEnum.SmallPopulationOrDistribution:
        //                    redListThematicListItemViewModel.Id = (int)@enum;
        //                    redListThematicListItemViewModel.Name = RedListResource.TaxonSearchThematicListSmallPopulationOrDistribution;
        //                    break;
        //            }

        //            searchViewModel.ThematicLists.Add(redListThematicListItemViewModel);
        //        }

        //        searchViewModel.ThematicListOperator = LogicalOperator.Or;
        //    }
        //}

        /// <summary>
        /// Init RedListCategories information.
        /// </summary>
        /// <param name="searchViewModel">Search criteria view model.</param>
        public static void InitRedListCategories(this SearchViewModel searchViewModel)
        {
            if (mSearchViewModelFromSession.IsNotNull() && mSearchViewModelFromSession.RedListCategories.IsNotNull())
            {
                searchViewModel.IsRedListCategoriesEnabled = mSearchViewModelFromSession.IsRedListCategoriesEnabled;
                searchViewModel.RedListCategories = mSearchViewModelFromSession.RedListCategories;
                searchViewModel.RedListCategoriesTitle = mSearchViewModelFromSession.RedListCategoriesTitle;
            }
            else
            {
                var otherListedGroupCategoryIds = new List<int>();

                // Get red list category factor
                FactorList redListCategoryFactors = RedListCategoryCache.GetFactors(mUser);

                // Set red list categories title, from factor.
                searchViewModel.RedListCategoriesTitle = redListCategoryFactors[0].Label;

                // Group a limited list of red list categories; "red listed group"
                var categories = RedListedHelper.GetRedListCategoriesDdToNt();
                List<int> redListedGroupCategoryIds = categories.Select(cat => (int)cat).ToList();

                // Group a limited list of red list categories; "other group"
                RedListCategory redListCategory;
                for (redListCategory = RedListCategory.LC; redListCategory <= RedListCategory.NE; redListCategory++)
                {
                    otherListedGroupCategoryIds.Add((int)redListCategory);
                }

                searchViewModel.RedListCategories = new List<RedListCategoryItemViewModel>();

                // Populate red list categories
                RedListCategoryItemViewModel redListCategoryItemViewModel;
                foreach (var redListCategoryFactor in redListCategoryFactors)
                {
                    foreach (var redListCategoryItem in redListCategoryFactor.DataType.Field1.Enum.Values)
                    {
                        if (redListCategoryItem.KeyInt.HasValue && redListedGroupCategoryIds.Contains(redListCategoryItem.KeyInt.Value))
                        {
                            redListCategoryItemViewModel = new RedListCategoryItemViewModel
                            {
                                Id = redListCategoryItem.Id,
                                Name = redListCategoryItem.OriginalLabel,
                                OrderNumber = redListCategoryItem.KeyInt.Value,
                                Selected = true,
                                InRedListedGroup = true,
                                InOtherGroup = false
                            };
                            searchViewModel.RedListCategories.Add(redListCategoryItemViewModel);
                        }
                        if (redListCategoryItem.KeyInt.HasValue && otherListedGroupCategoryIds.Contains(redListCategoryItem.KeyInt.Value))
                        {
                            redListCategoryItemViewModel = new RedListCategoryItemViewModel
                            {
                                Id = redListCategoryItem.Id,
                                Name = redListCategoryItem.OriginalLabel,
                                OrderNumber = redListCategoryItem.KeyInt.Value,
                                Selected = false,
                                InRedListedGroup = false,
                                InOtherGroup = true
                            };
                            searchViewModel.RedListCategories.Add(redListCategoryItemViewModel);
                        }
                    }
                }

                const bool ShowNonCategorizedTaxa = true;
                const int NonCategorizedTaxaId = 1000;

                // Add value for non categorized taxa. Ie taxa that dont have category 743
                if (ShowNonCategorizedTaxa)
                {
                    redListCategoryItemViewModel = new RedListCategoryItemViewModel
                    {
                        Id = NonCategorizedTaxaId,
                        Name = "Non categorized taxa -TEST", //RedListResource.TaxonSearchNonCategorizedTaxaLabel,
                        OrderNumber = NonCategorizedTaxaId,
                        Selected = false,
                        InRedListedGroup = false,
                        InOtherGroup = true
                    };
                    searchViewModel.RedListCategories.Add(redListCategoryItemViewModel);
                }

                // Preselect red list category group as default
                searchViewModel.IsRedListCategoriesEnabled = true;
            }
        }

        ///// <summary>
        ///// Init RedListTaxonTypes information.
        ///// </summary>
        ///// <param name="searchViewModel">Search criteria view model.</param>
        //public static void InitRedListTaxonCategories(this SearchViewModel searchViewModel)
        //{
        //    if (mSearchViewModelFromSession.IsNotNull() && mSearchViewModelFromSession.RedListTaxonTypes.IsNotNull())
        //    {
        //        searchViewModel.IsRedListTaxonTypeEnabled = mSearchViewModelFromSession.IsRedListTaxonTypeEnabled;
        //        searchViewModel.RedListTaxonTypes = mSearchViewModelFromSession.RedListTaxonTypes;
        //        searchViewModel.RedListTaxonTypesTitle = mSearchViewModelFromSession.RedListTaxonTypesTitle;
        //    }
        //    else
        //    {
        //        searchViewModel.RedListTaxonTypes = new List<RedListTaxonTypeItemViewModel>();

        //        // Get red list taxon type factor
        //        FactorList redListTaxonCategoryFactors = TaxonScopeCache.GetFactors(mUser);

        //        // Set red list taxon types title, from factor.
        //        searchViewModel.RedListTaxonTypesTitle = redListTaxonCategoryFactors[0].Label;

        //        // Populate red list taxon types
        //        foreach (var redListTaxonCategoryFactor in redListTaxonCategoryFactors)
        //        {
        //            foreach (var redListTaxonTypeItem in redListTaxonCategoryFactor.DataType.Field4.Enum.Values)
        //            {
        //                var redListTaxonTypeItemViewModel = new RedListTaxonTypeItemViewModel
        //                {
        //                    Id = redListTaxonTypeItem.Id,
        //                    Name = redListTaxonTypeItem.OriginalLabel,
        //                    ClassDefinitionTextValue = ((int)GetCategoryIdFromString(redListTaxonTypeItem.KeyText)).ToString(),
        //                    Selected = ((int)GetCategoryIdFromString(redListTaxonTypeItem.KeyText)).ToString() == ((int)TaxonCategoryId.Species).ToString()
        //                };
        //                searchViewModel.RedListTaxonTypes.Add(redListTaxonTypeItemViewModel);
        //            }
        //        }

        //        // Preselect red list taxon types group as default
        //        searchViewModel.IsRedListTaxonTypeEnabled = true;
        //    }
        //}

        ///// <summary>
        ///// Get categoryid from textvalue
        ///// </summary>
        ///// <param name="categoryId"></param>
        ///// <returns></returns>
        //private static TaxonCategoryId GetCategoryIdFromString(string categoryId)
        //{
        //    if (categoryId == "A")
        //    {
        //        return TaxonCategoryId.Species;
        //    }
        //    else if (categoryId == "S")
        //    {
        //        return TaxonCategoryId.SmallSpecies;
        //    }
        //    else if (categoryId == "U")
        //    {
        //        return TaxonCategoryId.Subspecies;
        //    }

        //    throw new Exception();
        //}

        ///// <summary>
        ///// Init taxonScope information.
        ///// </summary>
        ///// <param name="searchViewModel">Search criteria view model.</param>
        //public static void InitTaxonScope(this SearchViewModel searchViewModel)
        //{
        //    if (mSearchViewModelFromSession.IsNotNull() && mSearchViewModelFromSession.TaxonScope.IsNotNull())
        //    {
        //        searchViewModel.IsTaxonScopeEnabled = mSearchViewModelFromSession.IsTaxonScopeEnabled;
        //        searchViewModel.TaxonScope = mSearchViewModelFromSession.TaxonScope;
        //    }
        //    else
        //    {
        //        searchViewModel.TaxonScope = new List<TaxonScopeItemViewModel>();

        //        // Get red list taxon type factor
        //        FactorList redListTaxonCategoryFactors = TaxonScopeCache.GetFactors(mUser);

        //        // Populate with Species,Smallspecies and Subspecies categories categories
        //        TaxonScopeItemViewModel taxonCategoriesItemViewModel;
        //        foreach (var redListTaxonCategoryFactor in redListTaxonCategoryFactors)
        //        {
        //            foreach (var redListTaxonTypeItem in redListTaxonCategoryFactor.DataType.Field4.Enum.Values)
        //            {
        //                taxonCategoriesItemViewModel =
        //                   new TaxonScopeItemViewModel
        //                   {
        //                       Id = redListTaxonTypeItem.Id,
        //                       KeyId = ((int)GetCategoryIdFromString(redListTaxonTypeItem.KeyText)).ToString(),
        //                       Name = redListTaxonTypeItem.OriginalLabel
        //                   };

        //                searchViewModel.TaxonScope.Add(taxonCategoriesItemViewModel);
        //            }
        //        }

        //        // Get red list taxon type factor
        //        IList<int> additionalTaxonCategories = TaxonScopeCache.GetAdditionalTaxonCategories(mUser);

        //        // Populate with additional categories
        //        foreach (var taxonCategoryId in additionalTaxonCategories)
        //        {

        //            taxonCategoriesItemViewModel =
        //               new TaxonScopeItemViewModel
        //               {
        //                   Id = taxonCategoryId,
        //                   KeyId = Convert.ToString(taxonCategoryId),
        //                   Name = Convert.ToString(taxonCategoryId)
        //               };

        //            searchViewModel.TaxonScope.Add(taxonCategoriesItemViewModel);
        //        }
        //    }
        //}

        ///// <summary>
        ///// Init OrganismGroups information.
        ///// </summary>
        ///// <param name="searchViewModel">Search criteria view model.</param>
        //public static void InitOrganismGroups(this SearchViewModel searchViewModel)
        //{
        //    if (mSearchViewModelFromSession.IsNotNull() && mSearchViewModelFromSession.OrganismGroups.IsNotNull())
        //    {
        //        searchViewModel.IsOrganismGroupsEnabled = mSearchViewModelFromSession.IsOrganismGroupsEnabled;
        //        searchViewModel.OrganismGroups = mSearchViewModelFromSession.OrganismGroups;
        //    }
        //    else
        //    {
        //        searchViewModel.OrganismGroups = new List<RedListOrganismGroupItemViewModel>();

        //        // Get organism group factor
        //        FactorList organismGroupFactors = OrganismGroupCache.GetFactors(mUser);

        //        // Populate organism groups
        //        foreach (var organismGroupFactor in organismGroupFactors)
        //        {
        //            foreach (var organismGroupItem in organismGroupFactor.DataType.Field1.Enum.Values)
        //            {
        //                if (organismGroupItem.KeyInt.HasValue)
        //                {
        //                    var redListOrganismGroupItemViewModel = new RedListOrganismGroupItemViewModel
        //                    {
        //                        Id = organismGroupItem.KeyInt.Value,
        //                        Name = organismGroupItem.OriginalLabel
        //                    };
        //                    searchViewModel.OrganismGroups.Add(redListOrganismGroupItemViewModel);
        //                }
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// Init LandscapeTypes information.
        ///// </summary>
        ///// <param name="searchViewModel">Search criteria view model.</param>
        //public static void InitLandscapeTypes(this SearchViewModel searchViewModel)
        //{
        //    if (mSearchViewModelFromSession.IsNotNull() && mSearchViewModelFromSession.LandscapeTypes.IsNotNull())
        //    {
        //        searchViewModel.IsLandscapeTypesEnabled = mSearchViewModelFromSession.IsLandscapeTypesEnabled;
        //        searchViewModel.IsLandscapeTypesImportant = mSearchViewModelFromSession.IsLandscapeTypesImportant;
        //        searchViewModel.LandscapeOccurence = mSearchViewModelFromSession.LandscapeOccurence;
        //        searchViewModel.LandscapeTypes = mSearchViewModelFromSession.LandscapeTypes;
        //        searchViewModel.LandscapeTypesTitle = mSearchViewModelFromSession.LandscapeTypesTitle;
        //    }
        //    else
        //    {
        //        searchViewModel.LandscapeTypes = new List<RedListLandscapeTypeItemViewModel>();

        //        // Get superior factor, for landscape types.
        //        IFactor superiorFactor = CoreData.FactorManager.GetFactor(mUser, FactorId.LandscapeFactors);

        //        // Get landscape type factors
        //        FactorList landscapeFactors = LandscapeTypeCache.GetFactors(mUser);

        //        // Set title, for landscape types.
        //        searchViewModel.LandscapeTypesTitle = superiorFactor.Label;

        //        // Populate landscape types
        //        foreach (var landscapeFactor in landscapeFactors)
        //        {
        //            var redListLandscapeTypeItemViewModel = new RedListLandscapeTypeItemViewModel
        //            {
        //                Id = landscapeFactor.Id,
        //                Name = landscapeFactor.Name
        //            };
        //            searchViewModel.LandscapeTypes.Add(redListLandscapeTypeItemViewModel);
        //        }

        //        // Preselect landscape occurence as default
        //        searchViewModel.LandscapeOccurence = LogicalOperator.Or;

        //        // Preselect landscape type important for species as default
        //        searchViewModel.IsLandscapeTypesImportant = true;
        //    }
        //}

        ///// <summary>
        ///// Init Substrates information.
        ///// </summary>
        ///// <param name="searchViewModel">Search criteria view model.</param>
        //public static void InitSubstrate(this SearchViewModel searchViewModel)
        //{
        //    if (mSearchViewModelFromSession.IsNotNull() && mSearchViewModelFromSession.Substrates.IsNotNull())
        //    {
        //        searchViewModel.IsSubstrateEnabled = mSearchViewModelFromSession.IsSubstrateEnabled;
        //        searchViewModel.IsSubstrateImportant = mSearchViewModelFromSession.IsSubstrateImportant;
        //        searchViewModel.SubstrateOperator = mSearchViewModelFromSession.SubstrateOperator;
        //        searchViewModel.Substrates = mSearchViewModelFromSession.Substrates;
        //        searchViewModel.SubstratesTitle = mSearchViewModelFromSession.SubstratesTitle;
        //    }
        //    else
        //    {
        //        searchViewModel.Substrates = new List<RedListSubstrateItemViewModel>();

        //        // Get superior factor for substrates.
        //        IFactor superiorFactor = CoreData.FactorManager.GetFactor(mUser, FactorId.Substrate);

        //        // Get substrate factors
        //        FactorList substrateFactors = SubstrateCache.GetFactors(mUser);

        //        // Set title for substrates, from factor.
        //        searchViewModel.SubstratesTitle = superiorFactor.Label;

        //        // Populate substrates
        //        foreach (var substrateFactor in substrateFactors)
        //        {
        //            var redListSubstrateItemViewModel = new RedListSubstrateItemViewModel
        //            {
        //                Id = substrateFactor.Id,
        //                Name = substrateFactor.Name
        //            };

        //            searchViewModel.Substrates.Add(redListSubstrateItemViewModel);
        //        }

        //        searchViewModel.SubstrateOperator = LogicalOperator.Or;
        //    }
        //}

        ///// <summary>
        ///// Init Impacts information.
        ///// </summary>
        ///// <param name="searchViewModel">Search criteria view model.</param>
        //public static void InitImpact(this SearchViewModel searchViewModel)
        //{
        //    if (mSearchViewModelFromSession.IsNotNull() && mSearchViewModelFromSession.Impacts.IsNotNull())
        //    {
        //        searchViewModel.IsImpactEnabled = mSearchViewModelFromSession.IsImpactEnabled;
        //        searchViewModel.LargeImpactOnly = mSearchViewModelFromSession.LargeImpactOnly;
        //        searchViewModel.PositiveImpact = mSearchViewModelFromSession.PositiveImpact;
        //        searchViewModel.ImpactOperator = mSearchViewModelFromSession.ImpactOperator;
        //        searchViewModel.Impacts = mSearchViewModelFromSession.Impacts;
        //        searchViewModel.ImpactsTitle = mSearchViewModelFromSession.ImpactsTitle;
        //    }
        //    else
        //    {
        //        searchViewModel.Impacts = new List<RedListImpactItemViewModel>();

        //        // Get superior factor for impacts.
        //        IFactor superiorFactor = CoreData.FactorManager.GetFactor(mUser, FactorId.Impact);

        //        // Get impact factors
        //        FactorList impactFactors = ImpactCache.GetFactors(mUser);

        //        // Set title for impacts, from factor.
        //        searchViewModel.ImpactsTitle = superiorFactor.Label;

        //        // Populate impacts
        //        foreach (var impactFactor in impactFactors)
        //        {
        //            var redListImpactItemViewModel = new RedListImpactItemViewModel
        //            {
        //                Id = impactFactor.Id,
        //                Name = impactFactor.Name
        //            };

        //            searchViewModel.Impacts.Add(redListImpactItemViewModel);
        //        }

        //        searchViewModel.ImpactOperator = LogicalOperator.Or;

        //        searchViewModel.LargeImpactOnly = true;
        //    }
        //}

        ///// <summary>
        ///// Init LifeForms information.
        ///// </summary>
        ///// <param name="searchViewModel">Search criteria view model.</param>
        //public static void InitLifeForms(this SearchViewModel searchViewModel)
        //{
        //    if (mSearchViewModelFromSession.IsNotNull() && mSearchViewModelFromSession.LifeForms.IsNotNull())
        //    {
        //        searchViewModel.IsLifeFormEnabled = mSearchViewModelFromSession.IsLifeFormEnabled;
        //        searchViewModel.LifeForms = mSearchViewModelFromSession.LifeForms;
        //        searchViewModel.LifeFormsTitle = mSearchViewModelFromSession.LifeFormsTitle;
        //    }
        //    else
        //    {
        //        searchViewModel.LifeForms = new List<RedListLifeFormItemViewModel>();

        //        // Get superior factor for life forms.
        //        IFactor superiorFactor = CoreData.FactorManager.GetFactor(mUser, FactorId.LifeFormFactors);

        //        // Get life form factors
        //        FactorList lifeFormFactors = LifeFormCache.GetFactors(mUser);

        //        // Set title for life forms, from factor.
        //        searchViewModel.LifeFormsTitle = superiorFactor.Label;

        //        // Populate life forms
        //        foreach (var lifeFormFactor in lifeFormFactors)
        //        {
        //            var redListLifeFormItemViewModel = new RedListLifeFormItemViewModel
        //            {
        //                Id = lifeFormFactor.Id,
        //                Name = lifeFormFactor.Name
        //            };

        //            searchViewModel.LifeForms.Add(redListLifeFormItemViewModel);
        //        }
        //    }
        //}

        ///// <summary>
        ///// Init Hosts information.
        ///// </summary>
        ///// <param name="searchViewModel">Search criteria view model.</param>
        //public static void InitHost(this SearchViewModel searchViewModel)
        //{
        //    if (mSearchViewModelFromSession.IsNotNull() && mSearchViewModelFromSession.Hosts.IsNotNull())
        //    {
        //        searchViewModel.IsHostEnabled = mSearchViewModelFromSession.IsHostEnabled;
        //        searchViewModel.IsHostImportant = mSearchViewModelFromSession.IsHostImportant;
        //        searchViewModel.HostOperator = mSearchViewModelFromSession.HostOperator;
        //        searchViewModel.Hosts = mSearchViewModelFromSession.Hosts;
        //    }
        //    else
        //    {
        //        searchViewModel.Hosts = new List<RedListHostItemViewModel>();

        //        // Get host taxa
        //        TaxonTreeNodeList hostTaxonTreeNodeList = HostCache.GetGroupedRootHosts(mUser);

        //        // Populate hosts
        //        foreach (var hostTaxon in hostTaxonTreeNodeList)
        //        {
        //            var redListHostItemViewModel = new RedListHostItemViewModel
        //            {
        //                Id = hostTaxon.Taxon.Id,
        //                Name = hostTaxon.Taxon.GetCommonNameOrDefault(hostTaxon.Taxon.ScientificName)
        //            };

        //            if (hostTaxon.Children.IsNotEmpty())
        //            {
        //                redListHostItemViewModel.Children = new List<RedListHostItemViewModel>();
        //                foreach (var child in hostTaxon.Children)
        //                {
        //                    var redListHostSubItemViewModel = new RedListHostItemViewModel
        //                    {
        //                        Id = child.Taxon.Id,
        //                        Name = child.Taxon.GetCommonNameOrDefault(child.Taxon.ScientificName)
        //                    };
        //                    redListHostItemViewModel.Children.Add(redListHostSubItemViewModel);
        //                }
        //            }

        //            searchViewModel.Hosts.Add(redListHostItemViewModel);
        //        }

        //        // Preselect host operator as default
        //        searchViewModel.HostOperator = LogicalOperator.Or;

        //        // Preselect that host is important for species as default
        //        searchViewModel.IsHostImportant = true;
        //    }
        //}
    }
}

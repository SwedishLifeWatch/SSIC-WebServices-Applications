using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using TaxonList = ArtDatabanken.Data.TaxonList;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Export
{
    /// <summary>
    /// Handling of user selection when exporting taxon
    /// information to Excel file.
    /// </summary>
    public class ExportViewModel
    {
        private Dictionary<Int32, Dictionary<ArtDatabanken.Data.FactorId, ArtDatabanken.Data.SpeciesFact>> _allSpeciesFacts;
        private Hashtable _taxonTrees;
        private TaxonCategoryList _filteredTaxonCategories;
        private TaxonCategoryList _outputTaxonCategories;
        private TaxonNameCategoryList _outputTaxonNameCategories;
        
        /// <summary>
        /// Create a new instance of ExportViewModel.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxon">Output is related to this taxon.</param>
        /// <param name="isHierarchical">
        /// Indicates if output should be a hierarchical or straight list.
        /// </param>
        public static ExportViewModel Create(
            IUserContext userContext,
            ITaxon taxon,
            Boolean isHierarchical)
        {
            ExportViewModel model;

            model = new ExportViewModel();
            model.FilterIsValidTaxon = true;
            model.IsHierarchical = isHierarchical;
            model.OutputAuthor = true;
            model.OutputCommonName = true;
            model.OutputScientificName = !isHierarchical;
            model.OutputTaxonId = true;
            model.OutputTaxonUrl = true;
            model.TaxonId = taxon.Id;
            model.InitSpeciesFacts(userContext);
            model.InitTaxonTree(userContext, taxon);
            model.InitTaxonCategories(userContext, taxon);
            model.InitTaxonNameCategories(userContext);            

            if (isHierarchical)
            {
                model.Title = model.Labels.HierarchicalTitleLabel;
                model.PostAction = "HierarchicalTaxonList";
            }
            else
            {
                model.Title = model.Labels.TitleLabel;
                model.PostAction = "TaxonList";
            }

            return model;
        }

        #region Properties

        /// <summary>
        /// When all taxon categories are selected, we wan't to delete
        /// this virtual taxon category and must use this constant.
        /// </summary>
        public Int32 AllTaxonCategoryId
        {
            get { return 99999; }
        }

        public List<ExportTaxonCategory> FilterAllTaxonCategories { get; set; }

        /// <summary>
        /// List of child taxon categories that can be listed given the current taxon.
        /// The checked items affect the selection of taxa that should be listed.
        /// </summary>
        public List<ExportTaxonCategory> FilterChildrenTaxonCategories { get; set; }

        public ExportTaxonCategory FilterCurrentTaxonCategory { get; set; }

        public Boolean? FilterIsValidTaxon { get; set; }

        /// <summary>
        /// List of parent taxon categories that can be listed given the current taxon.
        /// The checked items affect the selection of taxa that should be listed.
        /// </summary>
        public List<ExportTaxonCategory> FilterParentTaxonCategories { get; set; }

        public IList<ExportSpeciesFactFactorValue> FilterSwedishHistoryValues { get; set; }

        public IList<ExportSpeciesFactFactorValue> FilterSwedishOccurrenceValues { get; set; }

        public Boolean IsHierarchical { get; set; }

        public List<ExportTaxonCategory> OutputAllTaxonCategories { get; set; }

        /// <summary>
        /// Boolean to indicate if a column with author of recommended scientific name column should be included in output.
        /// </summary>
        [LocalizedDisplayName("ExportStraightColumnAuthor", NameResourceType = typeof(Resources.DyntaxaResource))]
        public Boolean OutputAuthor { get; set; }

        /// <summary>
        /// Indicates whether or not author should be included in the same cells as the scientific name.
        /// </summary>
        [LocalizedDisplayName("ExportStraightIncludeAuthorInAllNameCells", NameResourceType = typeof(Resources.DyntaxaResource))]
        public Boolean OutputAuthorInAllNameCells { get; set; }

        public List<ExportTaxonCategory> OutputChildTaxonCategories { get; set; }

        /// <summary>
        /// Boolean to indicate if a column with recommended common name should be included in output.
        /// </summary>
        [LocalizedDisplayName("ExportStraightColumnCommonName", NameResourceType = typeof(Resources.DyntaxaResource))]
        public Boolean OutputCommonName { get; set; }

        /// <summary>
        /// Indicates whether or not recommended commen name should be included in the same cells as the scientific name.
        /// </summary>
        [LocalizedDisplayName("ExportStraightIncludeCommonNameInAllNameCells", NameResourceType = typeof(Resources.DyntaxaResource))]
        public Boolean OutputCommonNameInAllNameCells { get; set; }

        public ExportTaxonCategory OutputCurrentTaxonCategory { get; set; }

        /// <summary>
        /// Boolean to indicate if a column with GUID column should be included in output.
        /// </summary>
        [LocalizedDisplayName("ExportStraightColumnGUID", NameResourceType = typeof(Resources.DyntaxaResource))]
        public Boolean OutputGUID { get; set; }

        public List<ExportTaxonCategory> OutputParentTaxonCategories { get; set; }

        [LocalizedDisplayName("ExportStraightColumnRecommendedGUID", NameResourceType = typeof(Resources.DyntaxaResource))]
        public Boolean OutputRecommendedGUID { get; set; }

        /// <summary>
        /// Boolean to indicate if a column with recommended scientific name column should be included in output.
        /// </summary>
        [LocalizedDisplayName("ExportStraightColumnScientificName", NameResourceType = typeof(Resources.DyntaxaResource))]
        public Boolean OutputScientificName { get; set; }

        [LocalizedDisplayName("ExportStraightSynonyms", NameResourceType = typeof(Resources.DyntaxaResource))]
        public Boolean OutputSynonyms { get; set; }

        [LocalizedDisplayName("ExportStraightProParteSynonyms", NameResourceType = typeof(Resources.DyntaxaResource))]
        public Boolean OutputProParteSynonyms { get; set; }

        [LocalizedDisplayName("ExportStraightMisappliedNames", NameResourceType = typeof(Resources.DyntaxaResource))]
        public Boolean OutputMisappliedNames { get; set; }

        /// <summary>
        /// If set to true than author should be excluded in the same cells as the scientific name.
        /// </summary>
        [LocalizedDisplayName("ExportStraightExcludeAuctor", NameResourceType = typeof(Resources.DyntaxaResource))]
        public Boolean OutputAuthorForSynonyms { get; set; }

        [LocalizedDisplayName("ExportStraightSwedishHistory", NameResourceType = typeof(Resources.DyntaxaResource))]
        public Boolean OutputSwedishHistory { get; set; }

        [LocalizedDisplayName("ExportStraightSwedishOccurrence", NameResourceType = typeof(Resources.DyntaxaResource))]
        public Boolean OutputSwedishOccurrence { get; set; }

        /// <summary>
        /// Boolean to indicate if a column with taxon category should be included in output.
        /// </summary>
        [LocalizedDisplayName("ExportStraightColumnTaxonCategory", NameResourceType = typeof(Resources.DyntaxaResource))]
        public Boolean OutputTaxonCategory { get; set; }

        /// <summary>
        /// Boolean to indicate if a column with OutputTaxonId column should be included in output.
        /// </summary>
        [LocalizedDisplayName("ExportStraightColumnTaxonId", NameResourceType = typeof(Resources.DyntaxaResource))]
        public Boolean OutputTaxonId { get; set; }

        /// <summary>
        /// Taxon name categories that should be included in output.
        /// </summary>
        public List<ExportNameType> OutputTaxonNameCategories { get; set; }

        /// <summary>
        /// Boolean to indicate if a column with taxon category should be included in output.
        /// </summary>
        [LocalizedDisplayName("ExportStraightColumnTaxonInfoUrl", NameResourceType = typeof(Resources.DyntaxaResource))]
        public Boolean OutputTaxonUrl { get; set; }

        public String PostAction { get; set; }

        public Int32 TaxonId { get; set; }

        /// <summary>
        /// All taxon categories.
        /// </summary>
        public TaxonCategoryList TaxonCategories { get; set; }

        /// <summary>
        /// All taxon name categories.
        /// </summary>
        public TaxonNameCategoryList TaxonNameCategories { get; set; }

        /// <summary>
        /// Taxon tree, which is the combination of both child taxon tree
        /// and parent taxon tree.
        /// The value of property TaxonTree is the taxon tree node which is 
        /// related to current taxon, which is somewhere inside the taxon tree.
        /// </summary>
        public ITaxonTreeNode TaxonTree { get; set; }

        public String Title { get; set; }

        #endregion

        /// <summary>
        /// Get checked swedish history factor enum values.
        /// </summary>
        /// <returns>Checked swedish history factor enum values.</returns>
        public List<Int32> GetFilteredSwedishHistoryValues(IUserContext userContext)
        {
            List<Int32> filteredSwedishHistoryValues;

            filteredSwedishHistoryValues = new List<Int32>();
            if (!IsAllSwedishHistoryChecked())
            {
                foreach (ExportSpeciesFactFactorValue factorEnumValue in FilterSwedishHistoryValues)
                {
                    if (factorEnumValue.IsChecked)
                    {
                        filteredSwedishHistoryValues.Add(factorEnumValue.Id);
                    }
                }
            }
            return filteredSwedishHistoryValues;
        }

        /// <summary>
        /// Get checked swedish occurrence factor enum values.
        /// </summary>
        /// <returns>Checked swedish occurrence factor enum values.</returns>
        public List<Int32> GetFilteredSwedishOccurrenceValues()
        {
            List<Int32> filteredSwedishOccurrenceValues;

            filteredSwedishOccurrenceValues = new List<Int32>();
            if (!IsAllSwedishOccurrenceChecked())
            {
                foreach (ExportSpeciesFactFactorValue factorEnumValue in FilterSwedishOccurrenceValues)
                {
                    if (factorEnumValue.IsChecked)
                    {
                        filteredSwedishOccurrenceValues.Add(factorEnumValue.Id);
                    }
                }
            }
            return filteredSwedishOccurrenceValues;
        }

        /// <summary>
        /// Get all taxa that fulfills filter criteria.
        /// </summary>
        /// <returns>All taxa that fulfills filter criteria.</returns>
        public TaxonList GetFilteredTaxa(IUserContext userContext)
        {
            Boolean isConditionFulfilled;
            Int32 index;
            List<Int32> filteredSwedishHistoryValues, filteredSwedishOccurrenceValues;
            ArtDatabanken.Data.SpeciesFact speciesFact;
            TaxonCategoryList filteredTaxonCategories;
            TaxonList filteredTaxa;
            ITaxonTreeNode taxonTreeNode;

            // Get all taxa.
            filteredTaxa = TaxonTree.GetTaxa();

            // Filter on taxon categories.
            if (!IsAllFilterTaxonCategoriesChecked())
            {
                filteredTaxonCategories = GetFilteredTaxonCategories();
                if (FilterParentTaxonCategories.IsNotEmpty())
                {
                    for (index = filteredTaxa.Count - 1; index >= 0; index--)
                    {
                        if (!filteredTaxonCategories.Contains(filteredTaxa[index].Category))
                        {
                            // Remove taxon from filtered list.
                            filteredTaxa.RemoveAt(index);
                        }
                    }
                }
            }

            // Filter on is-valid-taxon is "yes" or "no"
            if (FilterIsValidTaxon.HasValue)
            {
                for (index = filteredTaxa.Count - 1; index >= 0; index--)
                {
                    // Filter is "Valid = no"
                    if (!FilterIsValidTaxon.Value) 
                    {
                        // taxon is valid
                        if (filteredTaxa[index].IsValid)
                        {
                            // Remove taxon from filtered list.
                            filteredTaxa.RemoveAt(index);
                        }
                    }
                    // Filter is "Valid = yes"
                    else if (FilterIsValidTaxon.Value)
                    {
                        // taxon is NOT valid
                        if (!filteredTaxa[index].IsValid)
                        {
                            // Remove taxon from filtered list.
                            filteredTaxa.RemoveAt(index);
                        }
                    }
                }
            }

            // Filter on species facts.
            filteredSwedishHistoryValues = GetFilteredSwedishHistoryValues(userContext);
            filteredSwedishOccurrenceValues = GetFilteredSwedishOccurrenceValues();
            /*
            // GuNy 2013-02-04
            ITaxon tempTaxon;

            if (filteredSwedishHistoryValues.IsNotEmpty() || filteredSwedishOccurrenceValues.IsNotEmpty())
            {
                for (index = filteredTaxa.Count - 1; index >= 0; index--)
                {
                    // GuNy 2013-02-04
                    tempTaxon = filteredTaxa[index];
                    isConditionFulfilled = false;

                    if (filteredSwedishHistoryValues.IsNotEmpty())
                    {
                        // Filter on swedish history.
                        speciesFact = GetSpeciesFact(FactorId.SwedishHistory, tempTaxon);
                        if (speciesFact.IsNotNull() && speciesFact.Field1.IsNotNull()
                            && speciesFact.Field1.EnumValue.IsNotNull()
                            && filteredSwedishHistoryValues.Contains(speciesFact.Field1.EnumValue.KeyInt))
                        {
                            isConditionFulfilled = true;
                        }
                        if (!isConditionFulfilled)
                        {
                            // Remove taxon from filtered list.
                            filteredTaxa.RemoveAt(index);
                            continue;
                        }
                    }

                    if (filteredSwedishOccurrenceValues.IsNotEmpty())
                    {
                        // Filter on swedish occurrence
                        isConditionFulfilled = false;
                        speciesFact = GetSpeciesFact(FactorId.SwedishOccurence, tempTaxon);
                        if (speciesFact.IsNotNull() && speciesFact.Field1.IsNotNull()
                            && speciesFact.Field1.EnumValue.IsNotNull()
                            && filteredSwedishOccurrenceValues.Contains(speciesFact.Field1.EnumValue.KeyInt))
                        {
                            isConditionFulfilled = true;
                        }

                        if (!isConditionFulfilled)
                        {
                            // Remove taxon from filtered list.
                            filteredTaxa.RemoveAt(index);
                        }
                    }
                }
            }
            return filteredTaxa;
        }

        */
        
        if (filteredSwedishHistoryValues.IsNotEmpty() ||
            filteredSwedishOccurrenceValues.IsNotEmpty())
        {
            for (index = filteredTaxa.Count - 1; index >= 0; index--)
            {
                taxonTreeNode = GetTaxonTreeNode(filteredTaxa[index]);

                if (filteredSwedishHistoryValues.IsNotEmpty())
                {
                    // Filter on swedish history
                    isConditionFulfilled = false;
                    bool checkOwnFails = false;

                    // First check current taxon
                    speciesFact = GetSpeciesFact(userContext, ArtDatabanken.Data.FactorId.SwedishHistory, taxonTreeNode.Taxon);
                    if (speciesFact.IsNotNull() &&
                                speciesFact.Field1.IsNotNull() &&
                                speciesFact.Field1.EnumValue.IsNotNull() &&
                                speciesFact.Field1.EnumValue.KeyInt.HasValue)
                    {
                        if (filteredSwedishHistoryValues.Contains(speciesFact.Field1.EnumValue.KeyInt.Value))
                        {
                            isConditionFulfilled = true;
                        }
                        else
                        {
                            checkOwnFails = true;
                        }
                    }

                    if (!isConditionFulfilled && !checkOwnFails)
                    {
                        foreach (ITaxon tempTaxon in taxonTreeNode.GetTaxa())
                        {
                            speciesFact = GetSpeciesFact(userContext, ArtDatabanken.Data.FactorId.SwedishHistory, tempTaxon);
                            if (tempTaxon.Id == taxonTreeNode.Taxon.Id)
                            {
                                continue;
                            }

                            if (speciesFact.IsNotNull() &&
                                speciesFact.Field1.IsNotNull() &&
                                speciesFact.Field1.EnumValue.IsNotNull() &&
                                speciesFact.Field1.EnumValue.KeyInt.HasValue &&
                                filteredSwedishHistoryValues.Contains(speciesFact.Field1.EnumValue.KeyInt.Value))
                            {
                                isConditionFulfilled = true;
                                break;
                            }
                        }
                    }

                    if (!isConditionFulfilled)
                    {
                        // Remove taxon from filtered list.
                        filteredTaxa.RemoveAt(index);
                        continue;
                    }
                }

                if (filteredSwedishOccurrenceValues.IsNotEmpty())
                {
                    // Filter on swedish occurrence
                    isConditionFulfilled = false;
                    bool checkOwnFails = false;

                    // First check current taxon
                    speciesFact = GetSpeciesFact(userContext, ArtDatabanken.Data.FactorId.SwedishOccurrence, taxonTreeNode.Taxon);
                    if (speciesFact.IsNotNull() &&
                                speciesFact.Field1.IsNotNull() &&
                                speciesFact.Field1.EnumValue.IsNotNull() &&
                                speciesFact.Field1.EnumValue.KeyInt.HasValue)
                    {
                        if (filteredSwedishOccurrenceValues.Contains(speciesFact.Field1.EnumValue.KeyInt.Value))
                        {
                            isConditionFulfilled = true;
                        }
                        else
                        {
                            checkOwnFails = true;
                        }
                    }

                    if (!isConditionFulfilled && !checkOwnFails)
                    {
                        foreach (ITaxon tempTaxon in taxonTreeNode.GetTaxa())
                        {
                            speciesFact = GetSpeciesFact(userContext, ArtDatabanken.Data.FactorId.SwedishOccurrence, tempTaxon);
                            if (tempTaxon.Id == taxonTreeNode.Taxon.Id)
                            {
                                continue;
                            }

                            if (speciesFact.IsNotNull() &&
                                speciesFact.Field1.IsNotNull() &&
                                speciesFact.Field1.EnumValue.IsNotNull() &&
                                speciesFact.Field1.EnumValue.KeyInt.HasValue &&
                                filteredSwedishOccurrenceValues.Contains(speciesFact.Field1.EnumValue.KeyInt.Value))
                            {
                                isConditionFulfilled = true;
                                break;
                            }
                        }
                    }

                    if (!isConditionFulfilled)
                    {
                        // Remove taxon from filtered list.
                        filteredTaxa.RemoveAt(index);
                    }
                }
            }
        }

        return filteredTaxa;
    }
      
        /// <summary>
        /// Get all filter taxon categories that are checked.
        /// </summary>
        /// <returns>All filter taxon categories that are checked.</returns>
        public TaxonCategoryList GetFilteredTaxonCategories()
        {
            if (_filteredTaxonCategories.IsNull())
            {
                _filteredTaxonCategories = new TaxonCategoryList();
                foreach (ExportTaxonCategory filterTaxonCategory in FilterAllTaxonCategories)
                {
                    if (filterTaxonCategory.IsChecked)
                    {
                        _filteredTaxonCategories.Add(TaxonCategories.Get(filterTaxonCategory.CategoryId));
                    }
                }
            }
            return _filteredTaxonCategories;
        }

        /// <summary>
        /// Get all output taxon categories that are checked.
        /// </summary>
        /// <returns>All output taxon categories that are checked.</returns>
        public TaxonCategoryList GetOutputTaxonCategories()
        {
            if (_outputTaxonCategories.IsNull())
            {
                _outputTaxonCategories = new TaxonCategoryList();
                foreach (ExportTaxonCategory outputTaxonCategory in OutputAllTaxonCategories)
                {
                    if (outputTaxonCategory.IsChecked)
                    {
                        _outputTaxonCategories.Add(TaxonCategories.Get(outputTaxonCategory.CategoryId));
                    }
                }
            }
            return _outputTaxonCategories;
        }

        /// <summary>
        /// Get all output taxon name categories that are checked.
        /// </summary>
        /// <returns>All output taxon name categories that are checked.</returns>
        public TaxonNameCategoryList GetOutputTaxonNameCategories()
        {
            if (_outputTaxonNameCategories.IsNull())
            {
                _outputTaxonNameCategories = new TaxonNameCategoryList();
                foreach (ExportNameType outputTaxonNameCategory in OutputTaxonNameCategories)
                {
                    if (outputTaxonNameCategory.IsChecked)
                    {
                        _outputTaxonNameCategories.Add(TaxonNameCategories.Get(outputTaxonNameCategory.Id));
                    }
                }
            }
            return _outputTaxonNameCategories;
        }

        /// <summary>
        /// Get species fact (Swedish history or Swedish occurrence)
        /// for specified taxon.
        /// </summary>
        /// <param name="factor">Get species fact for this factor.</param>
        /// <param name="taxon">Get species fact for this taxon.</param>
        /// <returns>Species fact (Swedish history and Swedish occurrence) for specified taxon.</returns>
        public ArtDatabanken.Data.SpeciesFact GetSpeciesFact(IUserContext userContext, ArtDatabanken.Data.FactorId factor, ITaxon taxon)
        {
            Dictionary<ArtDatabanken.Data.FactorId, ArtDatabanken.Data.SpeciesFact> speciesFacts;
            List<ArtDatabanken.Data.FactorId> factorIds;
            List<ITaxon> allTaxa;
            ArtDatabanken.Data.SpeciesFact speciesFact;

            speciesFact = null;
            if (_allSpeciesFacts.IsNull())
            {
                // Get species facts.
                factorIds = new List<ArtDatabanken.Data.FactorId>();
                factorIds.Add(ArtDatabanken.Data.FactorId.SwedishHistory);
                factorIds.Add(ArtDatabanken.Data.FactorId.SwedishOccurrence);
                allTaxa = new List<ITaxon>();
                allTaxa.AddRange(TaxonTree.GetTaxa().GetGenericList());
                _allSpeciesFacts = SpeciesFactHelper.GetSpeciesFacts(userContext, allTaxa, factorIds);
            }
            if (_allSpeciesFacts.ContainsKey(taxon.Id))
            {
                speciesFacts = _allSpeciesFacts[taxon.Id];
                if (speciesFacts.ContainsKey(factor))
                {
                    speciesFact = speciesFacts[factor];
                }
            }

            return speciesFact;
        }

        /// <summary>
        /// Get taxon tree node for specified taxon.
        /// </summary>
        /// <param name="taxon">Taxon.</param>
        /// <returns>Taxon tree node for specified taxon.</returns>
        public ITaxonTreeNode GetTaxonTreeNode(ITaxon taxon)
        {
            // Get taxon tree nodes.
            if (_taxonTrees.IsNull())
            {
                _taxonTrees = new Hashtable();
                foreach (ITaxonTreeNode taxonTreeNode in TaxonTree.GetTaxonTreeNodes())
                {
                    _taxonTrees[taxonTreeNode.Taxon.Id] = taxonTreeNode;
                }
            }
            return (ITaxonTreeNode)_taxonTrees[taxon.Id];
        }

        /// <summary>
        /// Init swedish occurence and swedish history factor enum value lists.
        /// </summary>
        private void InitSpeciesFacts(IUserContext userContext)
        {
            Dictionary<ArtDatabanken.Data.FactorId, IList<ArtDatabanken.Data.FactorFieldEnumValue>> factorEnumValues;

            factorEnumValues = SpeciesFactHelper.GetFactorsValueLists(userContext, new[] { ArtDatabanken.Data.FactorId.SwedishOccurrence, ArtDatabanken.Data.FactorId.SwedishHistory });
            FilterSwedishOccurrenceValues = new List<ExportSpeciesFactFactorValue>();
            //SwedishOccurrenceValues.Add(ExportSpeciesFactFactorValue.CreateValueMissingFactorValue(true));
            foreach (ArtDatabanken.Data.FactorFieldEnumValue enumValue in factorEnumValues[ArtDatabanken.Data.FactorId.SwedishOccurrence])
            {
                FilterSwedishOccurrenceValues.Add(ExportSpeciesFactFactorValue.Create(enumValue, true));
            }

            FilterSwedishHistoryValues = new List<ExportSpeciesFactFactorValue>();
            //SwedishHistoryValues.Add(ExportSpeciesFactFactorValue.CreateValueMissingFactorValue(true));
            foreach (ArtDatabanken.Data.FactorFieldEnumValue enumValue in factorEnumValues[ArtDatabanken.Data.FactorId.SwedishHistory])
            {
                FilterSwedishHistoryValues.Add(ExportSpeciesFactFactorValue.Create(enumValue, true));
            }
        }

        /// <summary>
        /// Init selection of taxon categories.
        /// Taxon categories are always based on valid taxa and taxon relations.
        /// The code must be changed if this assumption is not correct.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxon">Taxon.</param>
        private void InitTaxonCategories(IUserContext userContext, ITaxon taxon)
        {
            ExportTaxonCategory taxonCategory;

            TaxonCategories = CoreData.TaxonManager.GetTaxonCategories(userContext);
            FilterAllTaxonCategories = new List<ExportTaxonCategory>();

            // Add all parent taxon categories.
            FilterParentTaxonCategories = new List<ExportTaxonCategory>();
            foreach (ITaxonCategory parentTaxonCategory in TaxonTree.GetParentTaxonCategories())
            {
                taxonCategory = ExportTaxonCategory.Create(parentTaxonCategory, false);
                FilterParentTaxonCategories.Add(taxonCategory);
                if (!FilterAllTaxonCategories.Exists(x => x.CategoryId == taxonCategory.CategoryId))
                {
                    FilterAllTaxonCategories.Add(taxonCategory);
                }
            }

            // Add the current taxon category.
            FilterCurrentTaxonCategory = ExportTaxonCategory.Create(taxon.Category, true);
            if (!FilterAllTaxonCategories.Exists(x => x.CategoryId == FilterCurrentTaxonCategory.CategoryId))
            {
                FilterAllTaxonCategories.Add(FilterCurrentTaxonCategory);
            }

            // Add all child taxon categories.
            FilterChildrenTaxonCategories = new List<ExportTaxonCategory>();
            foreach (ITaxonCategory childTaxonCategory in TaxonTree.GetChildTaxonCategories())
            {
                taxonCategory = ExportTaxonCategory.Create(childTaxonCategory, childTaxonCategory.IsMainCategory);
                FilterChildrenTaxonCategories.Add(taxonCategory);
                if (!FilterAllTaxonCategories.Exists(x => x.CategoryId == taxonCategory.CategoryId))
                {
                    FilterAllTaxonCategories.Add(taxonCategory);
                }
            }

            // Create output categories by cloning from filter categories.
            OutputAllTaxonCategories = new List<ExportTaxonCategory>();
            OutputParentTaxonCategories = new List<ExportTaxonCategory>();
            foreach (ExportTaxonCategory filterParentTaxonCategory in FilterParentTaxonCategories)
            {
                taxonCategory = (ExportTaxonCategory)filterParentTaxonCategory.Clone();
                taxonCategory.IsChecked = false;
                OutputParentTaxonCategories.Add(taxonCategory);
                OutputAllTaxonCategories.Add(taxonCategory);
            }

            OutputCurrentTaxonCategory = (ExportTaxonCategory)FilterCurrentTaxonCategory.Clone();
            OutputCurrentTaxonCategory.IsChecked = false;
            OutputAllTaxonCategories.Add(OutputCurrentTaxonCategory);

            OutputChildTaxonCategories = new List<ExportTaxonCategory>();
            foreach (ExportTaxonCategory filterChildTaxonCategory in FilterChildrenTaxonCategories)
            {
                taxonCategory = (ExportTaxonCategory)filterChildTaxonCategory.Clone();
                taxonCategory.IsChecked = false;
                OutputChildTaxonCategories.Add(taxonCategory);
                OutputAllTaxonCategories.Add(taxonCategory);
            }
        }

        /// <summary>
        /// Init taxon name categories selection list.
        /// </summary>
        private void InitTaxonNameCategories(IUserContext userContext)
        {
            ExportNameType exportTaxonNameCategory;

            TaxonNameCategories = CoreData.TaxonManager.GetTaxonNameCategories(userContext);
            OutputTaxonNameCategories = new List<ExportNameType>();
            foreach (TaxonNameCategory taxonNameCategory in TaxonNameCategories)
            {
                exportTaxonNameCategory = ExportNameType.Create(taxonNameCategory);
                // TODO: - create enum for scientific and swedish name
                //if (nameType.Id == 0 || nameType.Id == 1) 
                //    exportNameType.IsChecked = true;                

                OutputTaxonNameCategories.Add(exportTaxonNameCategory);
            }
        }

        /// <summary>
        /// Init information about taxon trees.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxon">Taxon.</param>
        public void InitTaxonTree(IUserContext userContext, ITaxon taxon)
        {
            TaxonTree = taxon.GetTaxonTree(userContext, !IsInvalidTaxaUsed());
            _allSpeciesFacts = null;
            _taxonTrees = null;
        }

        public Boolean IsAllSwedishOccurrenceChecked()
        {
            if (FilterSwedishOccurrenceValues == null)
            {
                return true;
            }

            return FilterSwedishOccurrenceValues.All(value => value.IsChecked);
        }

        public Boolean IsAllSwedishHistoryChecked()
        {
            if (FilterSwedishHistoryValues == null)
            {
                return true;
            }

            return FilterSwedishHistoryValues.All(value => value.IsChecked);
        }

        public Boolean IsAllFilterTaxonCategoriesChecked()
        {
            if (FilterAllTaxonCategories == null)
            {
                return true;
            }

            return FilterAllTaxonCategories.All(value => value.IsChecked);
        }

        /// <summary>
        /// Test if invalid taxa is included in filtered taxa.
        /// </summary>
        /// <returns>True, if invalid taxa is included in filtered taxa.</returns>
        public Boolean IsInvalidTaxaUsed()
        {
            return !(FilterIsValidTaxon.HasValue && FilterIsValidTaxon.Value);
        }

        public void ReInitialize(
            IUserContext userContext,
            ITaxon taxon,
            Boolean isHierarchical,
            List<Int32> filterTaxonCategories,
            List<Int32> outputTaxonCategories,
            List<Int32> outputTaxonNameCategories,
            List<Int32> filterSwedishOccurrence,
            List<Int32> filterSwedishHistory)
        {
            IsHierarchical = isHierarchical;
            InitSpeciesFacts(userContext);
            InitTaxonTree(userContext, taxon);
            InitTaxonCategories(userContext, taxon);
            InitTaxonNameCategories(userContext);

            if (filterTaxonCategories.IsNotEmpty())
            {
                foreach (ExportTaxonCategory category in FilterAllTaxonCategories)
                {
                    category.IsChecked = filterTaxonCategories.Contains(category.CategoryId);
                }
            }

            if (outputTaxonCategories.IsNotEmpty())
            {
                foreach (ExportTaxonCategory category in OutputAllTaxonCategories)
                {
                    category.IsChecked = outputTaxonCategories.Contains(category.CategoryId);
                }
            }

            if (outputTaxonNameCategories.IsNotEmpty())
            {
                foreach (ExportNameType exportTaxonNameCategory in OutputTaxonNameCategories)
                {
                    exportTaxonNameCategory.IsChecked = outputTaxonNameCategories.Contains(exportTaxonNameCategory.Id);
                }
            }

            // Swedish history and occurrence            
            foreach (ExportSpeciesFactFactorValue value in FilterSwedishHistoryValues)
            {
                value.IsChecked = filterSwedishHistory.IsNotEmpty() &&
                                   filterSwedishHistory.Contains(value.Id);
            }
            foreach (ExportSpeciesFactFactorValue value in FilterSwedishOccurrenceValues)
            {
                value.IsChecked = filterSwedishOccurrence.IsNotEmpty() &&
                                   filterSwedishOccurrence.Contains(value.Id);
            }            
        }

        private readonly ModelLabels _labels = new ModelLabels();

        public ModelLabels Labels
        {
            get { return _labels; }
        }


        /// <summary>
        /// Localized labels
        /// </summary>
        public class ModelLabels
        {
            public string TitleLabel { get { return Resources.DyntaxaResource.ExportStraightTitle; } }
            public string HierarchicalTitleLabel { get { return Resources.DyntaxaResource.ExportHierarchicalTitle; } }
            public string IsValidTaxonLabel { get { return Resources.DyntaxaResource.ExportStraightIsValidTaxon; } }
            public string TaxonNamesLabel { get { return Resources.DyntaxaResource.ExportStraightTaxonNames; } }
            public string SwedishOccurrenceLabel { get { return Resources.DyntaxaResource.ExportStraightSwedishOccurrence; } }
            public string SwedishHistoryLabel { get { return Resources.DyntaxaResource.ExportStraightSwedishHistory; } }
            public string GeneratingExcelFile { get { return Resources.DyntaxaResource.ExportStraightGeneratingExcelFile; } }
            public string FilterLabel { get { return Resources.DyntaxaResource.ExportStraightFilter; } }
            public string CategoriesLabel { get { return Resources.DyntaxaResource.ExportStraightCategories; } }
            public string ParentsLabel { get { return Resources.DyntaxaResource.ExportStraightParents; } }
            public string CurrentLabel { get { return Resources.DyntaxaResource.ExportStraightCurrent; } }
            public string ChildrenLabel { get { return Resources.DyntaxaResource.ExportStraightChildren; } }
            public string OutputTitle { get { return Resources.DyntaxaResource.ExportStraightOutputTitle; } }
            public string CheckAllNoneLabel { get { return Resources.DyntaxaResource.ExportStraightCheckAllNone; } }
            public string SelectLabel { get { return Resources.DyntaxaResource.ExportStraightSelect; } }
            public string AllOptionLabel { get { return Resources.DyntaxaResource.ExportStraightAllOption; } }
            public string GetExcelFile { get { return Resources.DyntaxaResource.ExportStraightGetExcelFile; } }
            public string IncludeAuthorInAllNameCells { get { return Resources.DyntaxaResource.ExportStraightIncludeAuthorInAllNameCells; } }
            public string ExcludeAuthorInSynonymNameCells { get { return Resources.DyntaxaResource.ExportStraightIncludeAuthorInAllNameCells; } }
            public string IncludeCommonNameInAllNameCells { get { return Resources.DyntaxaResource.ExportStraightIncludeCommonNameInAllNameCells; } }
            public string ColumnTaxonCategory { get { return Resources.DyntaxaResource.ExportStraightColumnTaxonCategory; } }
            public string ColumnTaxonInfoUrl { get { return Resources.DyntaxaResource.ExportStraightColumnTaxonInfoUrl; } }
            public string ColumnTaxonId { get { return Resources.DyntaxaResource.ExportStraightColumnTaxonId; } }
            public string ColumnGUID { get { return Resources.DyntaxaResource.ExportStraightColumnGUID; } }
            public string ColumnRecommendedGUID { get { return Resources.DyntaxaResource.ExportStraightColumnRecommendedGUID; } }
            public string ColumnAuthor { get { return Resources.DyntaxaResource.ExportStraightColumnAuthor; } }
            public string ColumnCommonName { get { return Resources.DyntaxaResource.ExportStraightColumnCommonName; } }
            public string ColumnScientificName { get { return Resources.DyntaxaResource.ExportStraightColumnScientificName; } }
            public string WarningText { get { return Resources.DyntaxaResource.ExportWarningText; } }
        }
    }
}

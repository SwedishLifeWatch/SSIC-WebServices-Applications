using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Filters;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Taxa;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers
{
    /// <summary>
    /// This class is used to search for taxa in different ways
    /// </summary>
    public class TaxonSearchManager
    {
        private readonly IUserContext _user;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxonSearchManager"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public TaxonSearchManager(IUserContext user)
        {
            _user = user;
        }

        /// <summary>
        /// Get all child taxon 
        /// </summary>
        /// <param name="parentTaxaIds">Id of parent taxon</param>
        /// <returns></returns>
        public List<TaxonSearchResultItemViewModel> GetChildTaxa(int[] parentTaxaIds)
        {
            var searchCriteria = new TaxonSearchCriteria()
            {
                TaxonIds = new List<int>(parentTaxaIds),
                TaxonCategoryIds = new List<int> { (int)TaxonCategoryId.Species },
                Scope = TaxonSearchScope.AllChildTaxa
            };
            
            var taxa = CoreData.TaxonManager.GetTaxa(_user, searchCriteria);
            var resultList = (from t in taxa select TaxonSearchResultItemViewModel.CreateFromTaxon(t, false)).Distinct().ToList();
      
            var speciesFactManager = new SpeciesFactManager(_user);

            var protectedTaxonList = speciesFactManager.GetProtectedTaxons();

            // Set protection level for each taxon; public or not
            resultList.ForEach(t => t.SpeciesProtectionLevel = protectedTaxonList.Any(ptl => ptl.Id == t.TaxonId) ? SpeciesProtectionLevelEnum.Protected1 : SpeciesProtectionLevelEnum.Public);

            return resultList;
        }

        /// <summary>
        /// Searches for taxa using the give search options.
        /// </summary>
        /// <param name="searchOptions">The search options to use in the search.</param>
        /// <returns></returns>
        public List<TaxonSearchResultItemViewModel> SearchTaxa(TaxonSearchOptions searchOptions)
        {            
            TaxonNameSearchCriteria taxonNameSearchCriteria = searchOptions.CreateTaxonNameSearchCriteriaObject();            
            TaxonNameList taxonNames = CoreData.TaxonManager.GetTaxonNames(_user, taxonNameSearchCriteria);
            ITaxon taxonFoundById = SearchTaxonById(searchOptions.NameSearchString);
            var resultList = new List<TaxonSearchResultItemViewModel>();
            if (taxonFoundById != null) // the user entered a valid taxonid as search string
            {
                resultList.Add(TaxonSearchResultItemViewModel.CreateFromTaxon(taxonFoundById));
            }
            for (int i = 0; i < taxonNames.Count; i++)
            {
                resultList.Add(TaxonSearchResultItemViewModel.CreateFromTaxonName(taxonNames[i]));
            }
            resultList = resultList.Distinct().ToList();

            SpeciesFactManager speciesFactManager = new SpeciesFactManager(_user);

            IEnumerable<ITaxon> protectedTaxonList = speciesFactManager.GetProtectedTaxons();

            // Set protection level for each taxon; public or not
            resultList.ForEach(t => t.SpeciesProtectionLevel = protectedTaxonList.Any(ptl => ptl.Id == t.TaxonId) ? SpeciesProtectionLevelEnum.Protected1 : SpeciesProtectionLevelEnum.Public);

            return resultList;
        }

        /// <summary>
        /// Search taxa by factor fields.
        /// </summary>
        /// <param name="taxonSearchFactorFieldViewModels">List of factor fields with properties.</param>
        /// <param name="factorId"></param>
        /// <param name="restrictToCurrentTaxonFilter"></param>
        /// <param name="settings"></param>
        public List<TaxonSearchResultItemViewModel> SearchTaxa(List<TaxonSearchFactorFieldViewModel> taxonSearchFactorFieldViewModels, Int32 factorId, Boolean restrictToCurrentTaxonFilter, MySettings.MySettings settings)
        {
            var resultList = new List<TaxonSearchResultItemViewModel>();

            List<ITaxon> taxons = SearchTaxonByFactor(taxonSearchFactorFieldViewModels, factorId, restrictToCurrentTaxonFilter, settings);

            foreach (var taxon in taxons)
            {
                resultList.Add(TaxonSearchResultItemViewModel.CreateFromTaxon(taxon));
            }

            SpeciesFactManager speciesFactManager = new SpeciesFactManager(_user);

            IEnumerable<ITaxon> protectedTaxonList = speciesFactManager.GetProtectedTaxons();

            // Set protection level for each taxon; public or not
            resultList.ForEach(t => t.SpeciesProtectionLevel = protectedTaxonList.Any(ptl => ptl.Id == t.TaxonId) ? SpeciesProtectionLevelEnum.Protected1 : SpeciesProtectionLevelEnum.Public);

            return resultList;
        }

        /// <summary>
        /// Searches for a taxon by TaxonId.
        /// </summary>
        /// <param name="searchString">The search string that can contain a TaxonId.</param>
        /// <returns></returns>
        private ITaxon SearchTaxonById(string searchString)
        {
            int taxonId;
            ITaxon taxon = null;
            if (int.TryParse(searchString, out taxonId))
            {
                try
                {
                    taxon = CoreData.TaxonManager.GetTaxon(_user, taxonId);                    
                }
                catch (Exception)
                {
                }
            }
            return taxon;
        }

        /// <summary>
        /// Search for list of taxon by factor.
        /// </summary>
        /// <param name="taxonSearchFactorFieldViewModels"></param>
        /// <param name="factorId"></param>
        /// <param name="restrictToCurrentTaxonFilter"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        private List<ITaxon> SearchTaxonByFactor(List<TaxonSearchFactorFieldViewModel> taxonSearchFactorFieldViewModels, Int32 factorId, Boolean restrictToCurrentTaxonFilter, MySettings.MySettings settings)
        {
            if (taxonSearchFactorFieldViewModels == null)
            {
                taxonSearchFactorFieldViewModels = new List<TaxonSearchFactorFieldViewModel>();
            }

            TaxonList taxonList = new TaxonList();
            ISpeciesFactSearchCriteria searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.FieldSearchCriteria = new SpeciesFactFieldSearchCriteriaList();
            searchCriteria.Factors = new FactorList();
            ISpeciesFactFieldSearchCriteria fieldSearchCriteria;
            searchCriteria.FieldLogicalOperator = LogicalOperator.And;

            if (taxonSearchFactorFieldViewModels.Count > 0)
            {
                if (restrictToCurrentTaxonFilter)
                {
                    if (settings.Filter.Taxa.HasSettings)
                    {
                        searchCriteria.Taxa = CoreData.TaxonManager.GetTaxa(_user, settings.Filter.Taxa.TaxonIds.ToList());
                    }
                    else
                    {
                        return new TaxonList();
                    }
                }
            
                IFactor factor = CoreData.FactorManager.GetFactor(_user, factorId);

                searchCriteria.Factors.Add(factor);

                // Add factor field to search criteria by list of factor field id
                foreach (var field in factor.DataType.Fields)
                {
                    if (field.Type.Id == (int)FactorFieldDataTypeId.Enum)
                    {
                        if (field.Enum != null && field.Enum.Values != null)
                        {
                            fieldSearchCriteria = new SpeciesFactFieldSearchCriteria();
                            // fieldSearchCriteria.IsEnumAsString = true;
                            fieldSearchCriteria.FactorField = field;
                            fieldSearchCriteria.Operator = CompareOperator.Equal;
                            foreach (var val in field.Enum.Values)
                            {
                                if (val.KeyInt.HasValue && taxonSearchFactorFieldViewModels.Any(x => x.FactorFieldTypeId == val.Id))
                                {
                                    String factorValue = taxonSearchFactorFieldViewModels.First(x => x.FactorFieldTypeId == val.Id).FactorFieldTypeValue;
                                    fieldSearchCriteria.AddValue(factorValue);
                                }
                            }

                            if (fieldSearchCriteria.Values.IsNotEmpty())
                            {
                                searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);
                            }
                        }
                    }
                    else if (field.Type.Id == (int)FactorFieldDataTypeId.Boolean)
                    {
                        if (field.Enum != null && field.Enum.Values != null)
                        {
                            fieldSearchCriteria = new SpeciesFactFieldSearchCriteria();
                            // fieldSearchCriteria.IsEnumAsString = true;
                            fieldSearchCriteria.FactorField = field;
                            fieldSearchCriteria.Operator = CompareOperator.Equal;
                            foreach (var val in field.Enum.Values)
                            {
                                if (val.KeyInt.HasValue && taxonSearchFactorFieldViewModels.Any(x => x.FactorFieldTypeId == val.Id))
                                {
                                    String factorValue = taxonSearchFactorFieldViewModels.First(x => x.FactorFieldTypeId == val.Id).FactorFieldTypeValue;
                                    fieldSearchCriteria.AddValue(factorValue);
                                }
                            }

                            if (fieldSearchCriteria.Values.IsNotEmpty())
                            {
                                searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);
                            }
                        }
                    }
                    else if (field.Type.Id == (int)FactorFieldDataTypeId.Double)
                    {
                        if (taxonSearchFactorFieldViewModels.Any(x => x.FactorFieldTypeId == field.Id))
                        {
                            TaxonSearchFactorFieldViewModel factorField = taxonSearchFactorFieldViewModels.First(x => x.FactorFieldTypeId == field.Id);
                            CompareOperator compareOperator = factorField.CompareOperatorIsSpecified ? factorField.CompareOperator : CompareOperator.Equal;
                            String factorValue = factorField.FactorFieldTypeValue;
                            fieldSearchCriteria = new SpeciesFactFieldSearchCriteria();
                            fieldSearchCriteria.FactorField = field;
                            fieldSearchCriteria.Operator = compareOperator;
                            fieldSearchCriteria.AddValue(factorValue.WebParseInt32());
                            searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);
                        }
                    }
                    else if (field.Type.Id == (int)FactorFieldDataTypeId.Int32)
                    {
                        if (taxonSearchFactorFieldViewModels.Any(x => x.FactorFieldTypeId == field.Id))
                        {
                            TaxonSearchFactorFieldViewModel factorField = taxonSearchFactorFieldViewModels.First(x => x.FactorFieldTypeId == field.Id);
                            CompareOperator compareOperator = factorField.CompareOperatorIsSpecified ? factorField.CompareOperator : CompareOperator.Equal;
                            String factorValue = factorField.FactorFieldTypeValue;
                            fieldSearchCriteria = new SpeciesFactFieldSearchCriteria();
                            fieldSearchCriteria.FactorField = field;
                            fieldSearchCriteria.Operator = compareOperator;
                            fieldSearchCriteria.AddValue(factorValue.WebParseInt32());
                            searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);
                        }
                    }
                    else if (field.Type.Id == (int)FactorFieldDataTypeId.String)
                    {
                        if (taxonSearchFactorFieldViewModels.Any(x => x.FactorFieldTypeId == field.Id))
                        {
                            TaxonSearchFactorFieldViewModel factorField = taxonSearchFactorFieldViewModels.First(x => x.FactorFieldTypeId == field.Id);
                            CompareOperator compareOperator = factorField.CompareOperatorIsSpecified ? factorField.CompareOperator : CompareOperator.Equal;
                            String factorValue = factorField.FactorFieldTypeValue;
                            fieldSearchCriteria = new SpeciesFactFieldSearchCriteria();
                            fieldSearchCriteria.FactorField = field;
                            fieldSearchCriteria.Operator = compareOperator;
                            fieldSearchCriteria.AddValue(factorValue);
                            searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);
                        }
                    }
                }
            }
            else
            {
                return new TaxonList();
            }

            // Get taxa.
            taxonList = CoreData.AnalysisManager.GetTaxa(_user, searchCriteria);

            return taxonList;
        }
    }
}

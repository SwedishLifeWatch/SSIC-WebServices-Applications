using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Taxon;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// Used to search for taxon objects and taxon names.
    /// </summary>
    public class TaxonSearchManager
    {
        public readonly ISessionHelper _sessionHelper;
        
        public TaxonSearchManager()
        {
        }

        // Called by test
        public TaxonSearchManager(IUserDataSource userDataSourceRepository, ITaxonDataSource taxonDataSourceRepository,  ISessionHelper session)
        {
            CoreData.UserManager.DataSource = userDataSourceRepository;
            CoreData.TaxonManager.DataSource = taxonDataSourceRepository;
            _sessionHelper = session;
        }

        /// <summary>
        /// Searches for taxa using the give search options.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="searchOptions">The search options to use in the search.</param>
        /// <returns></returns>
        public List<TaxonSearchResultItemViewModel> SearchTaxa(IUserContext userContext, TaxonSearchOptions searchOptions)
        {
            TaxonNameSearchCriteria taxonNameSearchCriteria = searchOptions.CreateTaxonNameSearchCriteriaObject();
            TaxonNameList taxonNames = CoreData.TaxonManager.GetTaxonNames(userContext, taxonNameSearchCriteria);
            ITaxon taxonFoundById = SearchTaxonById(userContext, searchOptions.NameSearchString);
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

            return resultList;
        }

        /// <summary>
        /// Searches for a taxon by TaxonId.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="searchString">The search string that can contain a TaxonId.</param>
        /// <returns></returns>
        private ITaxon SearchTaxonById(IUserContext userContext, string searchString)
        {
            int taxonId;
            ITaxon taxon = null;
            if (int.TryParse(searchString, out taxonId))
            {
                try
                {
                    taxon = CoreData.TaxonManager.GetTaxon(userContext, taxonId);
                }
                catch (Exception)
                {
                }
            }
            return taxon;
        }

        /// <summary>
        /// Search for a unique taxon.
        /// </summary>
        /// <param name="taxonIdentifier"></param>
        /// <returns></returns>
        public TaxonSearchResult GetTaxon(string taxonIdentifier)
        {
            if (string.IsNullOrEmpty(taxonIdentifier))
            {
                return new TaxonSearchResult(null, 0);
            }

            // If value is not an integer => get taxon by name...
            int taxonId = 0;
            if (!Int32.TryParse(taxonIdentifier, out taxonId))
            {
                return GetTaxonByName(taxonIdentifier);
            }

            // value is an integer => get taxon by id...
            try
            {                
                ITaxon taxon = GetTaxonById(taxonId);
                return new TaxonSearchResult(taxon, 1);
            }
            catch (Exception)
            {
                 // No taxon found return null
                return new TaxonSearchResult(null, 0);
                //throw new Exception(string.Format("Taxon not found. Id = {0}", taxonId));
            }
        }

        /// <summary>
        /// Gets taxon by Id number
        /// </summary>        
        public ITaxon GetTaxonById(int taxonId)
        {            
            return CoreData.TaxonManager.GetTaxon(GetCurrentUser(), taxonId);
        }

        private IUserContext GetCurrentUser()
        {
            return CoreData.UserManager.GetCurrentUser();
        }

        /// <summary>
        /// Search for a taxon by name.
        /// Returns a taxon if it finds a unique result
        /// </summary>
        /// <param name="taxonIdentifier"></param>
        /// <returns></returns>
        protected TaxonSearchResult GetTaxonByName(string taxonIdentifier)
        {
            var taxonNameSearchCriteria = new TaxonNameSearchCriteria();
            var stringSearchCriteria = new StringSearchCriteria();
            stringSearchCriteria.CompareOperators = new List<StringCompareOperator> { StringCompareOperator.Equal, StringCompareOperator.BeginsWith, StringCompareOperator.Contains };
            stringSearchCriteria.SearchString = taxonIdentifier;
            taxonNameSearchCriteria.NameSearchString = stringSearchCriteria;
            TaxonNameList list = CoreData.TaxonManager.GetTaxonNames(
                CoreData.UserManager.GetCurrentUser(),
                taxonNameSearchCriteria);
            if (list.Count == 1)
            {
                return new TaxonSearchResult(list[0].Taxon, 1);
            }
            else
            {
                return new TaxonSearchResult(null, list.Count);
            }
        }

        /// <summary>
        /// Search taxons
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public IList<ITaxonName> SearchTaxons(TaxonNameSearchCriteria taxonNameSearchCriteria)
        {
            //TaxonNameSearchCriteria taxonNameSearchCriteria = new TaxonNameSearchCriteria();
            //StringSearchCriteria stringSearchCriteria = new StringSearchCriteria();
            //stringSearchCriteria.CompareOperators = new List<StringCompareOperator> { StringCompareOperator.Like };
            //stringSearchCriteria.SearchString = searchString;
            //taxonNameSearchCriteria.NameSearchString = stringSearchCriteria;
            //taxonNameSearchCriteria.IsValidTaxon = true;

            return CoreData.TaxonManager.GetTaxonNames(CoreData.UserManager.GetCurrentUser(), taxonNameSearchCriteria);
        }
    }

    /// <summary>
    /// Search result.
    /// Taxon should be null if NumberOfMatches != 1
    /// </summary>
    public class TaxonSearchResult
    {
        public ITaxon Taxon { get; set; }
        public int NumberOfMatches { get; set; }

        public TaxonSearchResult(ITaxon taxon, int numberOfMatches)
        {
            Taxon = taxon;
            NumberOfMatches = numberOfMatches;
        }
    }
}

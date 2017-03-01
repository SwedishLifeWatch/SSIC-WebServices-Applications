using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Enums;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers
{
    /// <summary>
    /// This class is used to parse a text containing taxonids and return the corresponding taxa
    /// </summary>
    public class TaxonMatchManager
    {
        private IUserContext _user;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxonMatchManager"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public TaxonMatchManager(IUserContext user)
        {
            _user = user;
        }

        /// <summary>
        /// Parses a text containing taxonids and return the corresponding taxa        
        /// </summary>
        /// <param name="text">The text containing taxonids.</param>
        /// <param name="rowDelimiter">The row delimiter type.</param>
        /// <returns></returns>
        public List<ITaxon> GetMatchingTaxaFromText(string text, RowDelimiter rowDelimiter)
        {
            string[] stringArray = null;
            switch (rowDelimiter)
            {
                case RowDelimiter.ReturnLinefeed:
                    stringArray = text.Split('\n');
                    break;

                case RowDelimiter.Semicolon:
                    stringArray = text.Split(';');
                    break;

                case RowDelimiter.Tab:
                    stringArray = text.Split('\t');
                    break;

                case RowDelimiter.VerticalBar:
                    stringArray = text.Split('|');
                    break;

                default:
                    stringArray = text.Split('\n');
                    break;
            }
            stringArray = stringArray.Distinct().ToArray();
            return GetMatches(stringArray);
        }

        /// <summary>
        /// A method that perform matches of taxon identifiers provided in a list.
        /// </summary>
        /// <param name="items">Array of strings representing names or identifiers that should be matched with taxon concepts in Dyntaxa.</param>        
        /// <returns>A list of matches</returns>
        private List<ITaxon> GetMatches(string[] items)
        {
            var ids = new List<Int32>();
            foreach (var item in items)
            {
                Int32 id;
                if (Int32.TryParse(item, out id))
                {
                    ids.Add(id);
                }
            }
            return this.GetMatches(ids);
        }

        /// <summary>
        /// Get a list with corresponding taxon from taxon ids
        /// </summary>
        /// <param name="taxonIds">The taxon ids.</param>
        /// <returns></returns>
        private List<ITaxon> GetMatches(List<Int32> taxonIds)
        {            
            var searchCriteria = new TaxonSearchCriteria();
            searchCriteria.TaxonIds = new List<int>();
            searchCriteria.IsValidTaxon = true;
            foreach (Int32 id in taxonIds)
            {
                searchCriteria.TaxonIds.Add(id);
            }
            TaxonList taxa = CoreData.TaxonManager.GetTaxa(_user, searchCriteria);
            return (List<ITaxon>)taxa.GetGenericList();
        }
    }
}

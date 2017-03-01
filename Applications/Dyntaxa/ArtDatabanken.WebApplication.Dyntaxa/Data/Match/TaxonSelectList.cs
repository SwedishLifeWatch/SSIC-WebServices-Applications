using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Match
{
    public class TaxonSelectList
    {
        private readonly Dictionary<string, string> _listItems = null;

        private Dictionary<string, string> GetListItems(IList<ITaxonName> taxonNames)
        {
            var d = new Dictionary<string, string>();
            var addedTaxons = new List<ITaxon>();
            foreach (ITaxonName taxonName in taxonNames)
            {
                if (!TaxonInList(addedTaxons, taxonName.Taxon.Id))
                {
                    addedTaxons.Add(taxonName.Taxon);
                }
            }

            d.Add(Resources.DyntaxaResource.MatchTaxonDropDownListDefaultText.Replace("[Count]", addedTaxons.Count.ToString()), "0");
            foreach (ITaxon taxon in addedTaxons)
            {
                d.Add(string.Format("{0}: {1} [{2}]", taxon.Category.Name, taxon.GetLabel(), taxon.Id), taxon.Id.ToString());
            }
            return d;
        }

        /// <summary>
        /// Constructor of the Taxon drop down list class.
        /// </summary>
        /// <param name="taxonNames">A taxon list.</param>
        public TaxonSelectList(IList<ITaxonName> taxonNames)
        {
            _listItems = GetListItems(taxonNames);
        }

        /// <summary>
        /// Method that generate the taxon drop down list with the default value is selected.
        /// </summary>
        /// <returns>A list of drop down items.</returns>
        public SelectList GetList()
        {
            var selectList = new SelectList(_listItems, "Value", "Key", "0");
            return selectList;
        }

        /// <summary>
        /// Method that generate the taxon drop down list with a certain value selected.
        /// </summary>
        /// <param name="id">Id of the selected taxon.</param>
        /// <returns>A list of drop down items.</returns>
        public SelectList GetList(Int32 id)
        {
            var selectList = new SelectList(_listItems, "Value", "Key", id.ToString());
            return selectList;
        }

        /// <summary>
        /// Check if taxonId already is in the list of taxa.
        /// </summary>
        /// <param name="taxonList">
        /// The taxon list.
        /// </param>
        /// <param name="taxonId">
        /// The taxon id to check.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool TaxonInList(List<ITaxon> taxonList, int taxonId)
        {
            bool bInList = false;
            foreach (var taxon in taxonList)
            {
                if (taxon.Id.Equals(taxonId))
                {
                    bInList = true;
                    break;
                }
            }
            return bInList;
        }
    }
}

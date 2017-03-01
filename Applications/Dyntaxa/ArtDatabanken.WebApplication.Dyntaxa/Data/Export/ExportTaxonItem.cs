using System;
using System.Collections;
using System.Collections.Generic;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Export
{
    public class ExportTaxonItem
    {
        private Hashtable _taxonNames;
        private Hashtable _parentTaxa;
        private String _scientificName;

        public ExportTaxonItem(
            ITaxon taxon,
            ITaxonTreeNode taxonTreeNode,
            TaxonCategoryList outputTaxonCategories)
        {
            ParentTaxon = null;
            Taxon = taxon;
            InitParentTaxa(taxonTreeNode, outputTaxonCategories);
        }

        public ITaxon ParentTaxon { get; set; }
        public ITaxon Taxon { get; set; }
        public String SwedishHistory { get; set; }
        public String SwedishOccurrence { get; set; }
        public List<TaxonNameViewModel> Synonyms { get; set; }
        public List<TaxonNameViewModel> ProParteSynonyms { get; set; }
        public List<TaxonNameViewModel> MisappliedNames { get; set; }        

        public TaxonNameList TaxonNames
        { 
            set
            {
                DateTime today;

                _taxonNames = new Hashtable();
                if (value.IsNotEmpty())
                {
                    today = DateTime.Now;
                    foreach (ITaxonName taxonName in value)
                    {
                        if (taxonName.IsRecommended)
                        {
                            _taxonNames[taxonName.Category.Id] = taxonName;
                            if ((taxonName.Category.Id == (Int32)TaxonNameCategoryId.Guid) &&
                                (taxonName.ValidFromDate <= today) &&
                                (today <= taxonName.ValidToDate))
                            {
                                _taxonNames["RecommendedGUID"] = taxonName.Name;
                                break;
                            }
                        }
                    }
                    if (!_taxonNames.ContainsKey("RecommendedGUID"))
                    {
                        _taxonNames["RecommendedGUID"] = Taxon.Guid;
                    }
                }
            }
        }

        /// <summary>
        /// Get parent taxon of specified taxon category.
        /// Returns null if there is no parent of specified taxon category.
        /// </summary>
        /// <param name="parentTaxonCategory">Parent taxon category.</param>
        /// <returns>Parent taxon of specified taxon category.</returns>
        public ITaxon GetParentTaxon(ITaxonCategory parentTaxonCategory)
        {
            return (ITaxon)_parentTaxa[parentTaxonCategory.Id];
        }

        /// <summary>
        /// Get recommended GUID, ie name of type GUID and recommended.
        /// </summary>
        /// <returns>Recommended GUID.</returns>
        public String GetRecommendedGuid()
        {
            return (String)_taxonNames["RecommendedGUID"];
        }

        /// <summary>
        /// Get scientific name.
        /// </summary>
        /// <param name="addAuthor">Add author to scientific name.</param>
        /// <param name="addCommonName">Add common name to scientific name.</param>
        /// <returns>Scientific name.</returns>
        public String GetScientificName(Boolean addAuthor, Boolean addCommonName)
        {
            // TODO : We should check that parameters addAuthor and addCommonName
            // TODO : has the same value in each call.
            if (_scientificName.IsNull())
            {
                if (addAuthor && Taxon.Author.IsNotEmpty())
                {
                    if (addCommonName && Taxon.CommonName.IsNotEmpty())
                    {
                        // Add both author and common name.
                        _scientificName = Taxon.ScientificName + " " +
                                          Taxon.Author + " " +
                                          Taxon.CommonName;
                    }
                    else
                    {
                        // Add author.
                        _scientificName = Taxon.ScientificName + " " + Taxon.Author;
                    }
                }
                else
                {
                    if (addCommonName && Taxon.CommonName.IsNotEmpty())
                    {
                        // Add common name.
                        _scientificName = Taxon.ScientificName + " " +
                                          Taxon.CommonName;
                    }
                    else
                    {
                        // Add nothing.
                        _scientificName = Taxon.ScientificName;
                    }
                }
            }
            return _scientificName;
        }

        /// <summary>
        /// Get taxon name of specified taxon name category.
        /// May return null if taxon does not have a name of
        /// the specified taxon name category.
        /// Only recommended taxon names may be returned.
        /// </summary>
        /// <param name="taxonNameCategory">Taxon name category.</param>
        /// <returns>Taxon name of specified taxon name category.</returns>
        public ITaxonName GetTaxonName(ITaxonNameCategory taxonNameCategory)
        {
            return (ITaxonName)_taxonNames[taxonNameCategory.Id];
        }

        private void InitParentTaxa(
            ITaxonTreeNode taxonTreeNode,
            TaxonCategoryList outputTaxonCategories)
        {
            _parentTaxa = new Hashtable();
            foreach (ITaxon parentTaxon in taxonTreeNode.GetParentTaxa())
            {
                if (outputTaxonCategories.Exists(parentTaxon.Category))
                {
                    _parentTaxa[parentTaxon.Category.Id] = parentTaxon;
                    ParentTaxon = parentTaxon;
                }
            }
            if (outputTaxonCategories.Exists(Taxon.Category))
            {
                _parentTaxa[Taxon.Category.Id] = Taxon;
            }
        }
    }
}

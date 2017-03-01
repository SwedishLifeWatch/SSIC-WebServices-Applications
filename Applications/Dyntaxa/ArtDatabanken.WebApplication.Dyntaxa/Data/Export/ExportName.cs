using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Export
{
    public class ExportName
    {        
        public static ExportName Create(ITaxonName taxonName, ITaxon taxon)
        {
            var model = new ExportName();
            model.IsChecked = false;

            model.Name = taxonName.Name;
            model.Author = taxonName.Author;
            model.NameTypeId = taxonName.Category.Id;
            model.NameType = taxonName.Category.Name;
            model.NameUseId = taxonName.Status.Id;
            model.NameUse = taxonName.Status.Name;
            model.IsRecommended = taxonName.IsRecommended;
            model.NameTaxonCategory = taxon.Category.Name;                  
            
            model.TaxonId = taxonName.Taxon.Id;
            model.RecommendedScientificName = taxon.ScientificName;
            model.RecommendedAuthor = taxon.Author;
            model.RecommendedCommonName = taxon.GetCommonNameOrDefault("");
            model.TaxonCategory = taxon.Category.Name;
            model.TaxonCategoryId = taxon.Category.Id;

            return model;
        }

        public bool IsChecked { get; set; }        

        //Name properties
        public string TaxonNameId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int NameTypeId { get; set; }
        public string NameType { get; set; }
        public int NameUseId { get; set; }
        public string NameUse { get; set; }

        /// <summary>
        /// The taxon category represented by the name.
        /// This values is only relevant when the name taxon category differ from current category of the taxon.
        /// </summary>
        public string NameTaxonCategory { get; set; }

        public bool IsRecommended { get; set; }

        //Taxon properties
        public int TaxonId { get; set; }
        public string RecommendedCommonName { get; set; }
        public string RecommendedScientificName { get; set; }
        public string RecommendedAuthor { get; set; }
        public string TaxonCategory { get; set; }
        public int TaxonCategoryId { get; set; }
    }
}
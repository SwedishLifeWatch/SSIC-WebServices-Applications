using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using Newtonsoft.Json;
using Resources;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// View model for /Taxon/MoveChangeName/    
    /// </summary>
    public class TaxonMoveChangeNameViewModel
    {
        private readonly ModelLabels _labels = new ModelLabels();

        [Required]
        public int NewParentTaxonId { get; set; }

        [Required]
        public int OldParentTaxonId { get; set; }

        [Required]
        public List<ChangeNameItem> MovedChildTaxons { get; set; }        

        [Required]
        public List<int> SelectedChildTaxaToMove { get; set; }

        public int TaxonId { get; set; }        

        public string OldParentDescription { get; set; }

        public string NewParentDescription { get; set; }

        public List<string> MovedTaxonsDescription { get; set; }

        /// <summary>
        /// All localized labels
        /// </summary>
        public ModelLabels Labels
        {
            get { return _labels; }
        }

        /// <summary>
        /// Localized labels
        /// </summary>
        public class ModelLabels
        {
            public string NoSynonymsExists { get { return Resources.DyntaxaResource.TaxonMoveNoSynonymsExists; } }            
            public string ChangeChildrenNamesLabel { get { return Resources.DyntaxaResource.TaxonMoveChangeChildrenNames; } }
            public string NewRecommendedNamesLabel { get { return Resources.DyntaxaResource.TaxonMoveNewRecommendedNames; } }
            public string MoveInfo { get { return Resources.DyntaxaResource.TaxonMoveMoveInfo; } }
            public string OldParentLabel { get { return Resources.DyntaxaResource.TaxonMoveOldParent; } }
            public string NewParentLabel { get { return Resources.DyntaxaResource.TaxonMoveNewParent; } }
            public string MovedTaxonsLabel { get { return Resources.DyntaxaResource.TaxonMoveMovedTaxons; } }
        }
    }

    /// <summary>
    /// View model for name items used by TaxonMoveChangeNameViewModel
    /// </summary>
    public class ChangeNameItem
    {
        [Required]
        public int TaxonId { get; set; }

        [Required]
        public string Name { get; set; }

        public int? NameId { get; set; }

        public string Author { get; set; }

        public string Category { get; set; }

        public string OldName { get; set; }

        public string OldAuthor { get; set; }

        public List<SynonymName> Synonyms { get; set; }

        public string NameAndAuthorAsJson
        {            
            get { return JsonConvert.SerializeObject(new { name = this.Name, author = this.Author, id = -1 }); }
        }

        /// <summary>
        /// Creates a ChangeNameItem
        /// </summary>
        /// <remarks>
        /// Example:
        /// Here we choose to move the taxon A2 and all its children to Parent B
        /// In this step we create the taxon A221 => the following references is used:
        /// 
        /// Parent A (oldParentTaxon)           Parent B (newParentTaxon)
        ///  |-A1
        ///  |-A2 (movedTaxonParent)
        ///    |-A21
        ///    |-A22
        ///       |A221 (taxon)   
        /// </remarks>        
        public static ChangeNameItem CreateChangeNameItem(ITaxon taxon, ITaxon movedTaxonParent, ITaxon oldParentTaxon, ITaxon newParentTaxon, IUserContext user)
        {
            ChangeNameItem model = new ChangeNameItem();

            model.TaxonId = taxon.Id;
            model.Category = taxon.Category.Name;
            model.OldName = taxon.ScientificName.IsNotEmpty() ? taxon.ScientificName : string.Format("({0})", DyntaxaResource.ErrorNameIsMissing);
            model.OldAuthor = taxon.Author.IsNotEmpty() ? taxon.Author : "";
            model.Name = GetNewRecommendedName(taxon, oldParentTaxon, newParentTaxon);
            model.Author = model.OldAuthor;
            model.NameId = -1;

            // Synonyms        
            model.Synonyms = new List<SynonymName>();
            List<ITaxonName> synonyms = taxon.GetSynonyms(CoreData.UserManager.GetCurrentUser(), true);
            if (synonyms != null)
            {
                foreach (var taxonName in synonyms)
                {
                    model.Synonyms.Add(new SynonymName(taxonName, taxon));
                }
            }
            //foreach (var taxonName in taxon.TaxonNames)
            //{
            //    if (taxonName.NameCategory.Id == TaxonNameCategoryIds.SCIENTIFIC_NAME && taxonName.NameUsage.Id == (int)TaxonNameUsageId.ApprovedNaming)
            //    {
            //        model.Synonyms.Add(new SynonymeName(taxonName, taxon));
            //    }
            //}

            return model;
        }

        private static string GetNewRecommendedName(ITaxon taxon, ITaxon oldParentTaxon, ITaxon newParentTaxon)
        {
            if (oldParentTaxon.ScientificName.IsEmpty() ||
                newParentTaxon.ScientificName.IsEmpty())
            {
                return taxon.ScientificName.IsEmpty() ? "" : taxon.ScientificName;
            }

            return taxon.ScientificName.Replace(oldParentTaxon.ScientificName, newParentTaxon.ScientificName);
        }
    }

    /// <summary>
    /// View model used to show a synonyme name
    /// </summary>
    public class SynonymName : TaxonNameViewModel
    {
        public SynonymName(ITaxonName taxonName, ITaxon taxon)
            : base(taxonName, taxon)
        {
        }

        public string NameAndAuthor
        {
            get { return string.Format("{0} - {1}", this.Name, this.Author); }
        }

        public string NameAndAuthorAsJson
        {
            get { return JsonConvert.SerializeObject(new { name = this.Name, author = this.Author, id = this.Version }); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using System.Web.Mvc;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Match
{
    public class DyntaxaMatchItem
    {
        /// <summary>
        /// A sort order representing the original row number provided by end user.
        /// </summary>
        public int RowNumber { get; set; }

        /// <summary>
        /// The trimmed part of the provided text that is interprated as the taxon name.
        /// </summary>
        public String NameString { get; set; }

        /// <summary>
        /// The trimmed part of the provided text that is interprated as the the author of the name.
        /// </summary>
        public string AuthorString { get; set; }

        /// <summary>
        /// The original text provided by the end user.
        /// </summary>
        public string ProvidedText { get; private set; }

        /// <summary>
        /// Match status. 
        /// </summary>
        public MatchStatus Status { get; set; }

        public string StatusDescription 
        { 
            get { return Status.GetLocalizedDescription(); }
        }

        /// <summary>
        /// Drop down list representing list of alternative taxa matching the provided text.
        /// </summary>
        public SelectList AlternativeTaxa { get; set; }

        /// <summary>
        /// An identifier for the list of Alternativ taxa.
        /// </summary>
        public string DropDownListIdentifier { get; set; }

        /// <summary>
        /// All parent taxa as a string.
        /// </summary>
        public string ParentTaxa { get; set; }

        /// <summary>
        /// All scientific Synonyms as a string.
        /// </summary>
        public string ScientificSynonyms { get; set; }
        public int TaxonId { get; set; }
        public string ScientificName { get; set; }
        public string Author { get; set; }
        public string CommonName { get; set; }
        public object TaxonCategory { get; set; }
        public string GUID { get; set; }
        public string RecommendedGUID { get; set; }
        public string SwedishOccurrence { get; set; }

        public DyntaxaMatchItem(string nameString)
        {
            this.Status = MatchStatus.Undone;

            if (string.IsNullOrEmpty(nameString))
            {
                this.Status = MatchStatus.NoMatch;
            }

            if (this.Status.Equals(MatchStatus.Undone))
            {
                this.NameString = nameString.Trim();
                this.NameString = this.NameString.RemoveDuplicateBlanks();
                this.ProvidedText = this.NameString;
            }
        }

        public DyntaxaMatchItem(string nameString, string authorString)
        {
            this.Status = MatchStatus.Undone;

            if (string.IsNullOrEmpty(nameString))
            {
                this.Status = MatchStatus.NoMatch;
            }

            if (this.Status.Equals(MatchStatus.Undone))
            {
                this.NameString = nameString.Trim();
                this.NameString = this.NameString.RemoveDuplicateBlanks();
                this.ProvidedText = this.NameString;

                if (!string.IsNullOrEmpty(authorString))
                {
                    this.AuthorString = authorString.Trim();
                    this.AuthorString = this.AuthorString.RemoveDuplicateBlanks();
                    this.ProvidedText = this.ProvidedText + " " + this.AuthorString;
                }
            }
        }

        public DyntaxaMatchItem(int taxonId)
        {
            this.Status = MatchStatus.Undone;
            this.ProvidedText = taxonId.ToString();
        }

        public void SetTaxon(ITaxon taxon, MatchSettingsViewModel options)
        {
            IUserContext userContext = CoreData.UserManager.GetCurrentUser();
            this.TaxonId = taxon.Id;
            this.ScientificName = taxon.ScientificName;
            this.GUID = taxon.Guid;
            this.Author = taxon.Author;
            this.CommonName = taxon.CommonName.IsNotEmpty() ? taxon.CommonName : "";
            this.TaxonCategory = taxon.Category.Name;            
            if (options.OutputRecommendedGUID)
            {
                this.RecommendedGUID = taxon.GetRecommendedGuid(userContext);
            }

            if (options.OutputSwedishOccurrence)
            {
                try
                {
                    //Dictionary<FactorId, SpeciesFact> dicSpeciesFacts = SpeciesFactHelper.GetSpeciesFacts(taxon, new[] { FactorId.SwedishOccurence, FactorId.SwedishHistory });
                    Dictionary<ArtDatabanken.Data.FactorId, ArtDatabanken.Data.SpeciesFact> dicSpeciesFacts = SpeciesFactHelper.GetCommonDyntaxaSpeciesFacts(userContext, taxon);
                    this.SwedishOccurrence = SpeciesFactHelper.GetFactorValue(dicSpeciesFacts, ArtDatabanken.Data.FactorId.SwedishOccurrence);
                    //this.SwedishHistory = SpeciesFactHelper.GetFactorValue(dicSpeciesFacts, FactorId.SwedishHistory);                    
                }
                catch (Exception)
                {
                    this.SwedishOccurrence = "";
                }
            }

            //base.SetTaxon(taxon);
            //base.Author = taxon.Author;
            //base.CommonName = taxon.CommonName;
            //base.Id = taxon.Id;
            //base.ScientificName = taxon.ScientificName;
            //base.TaxonCategory = taxon.TaxonType.Name;
            //base.TaxonCategoryId = taxon.TaxonType.Id;
            //base.TaxonCategorySortOrder = taxon.TaxonType.SortOrder;
            //base.TaxonId = taxon.Id.ToString();
            //base.GUID = Resources.DyntaxaSettings.Default.LSIDString.Replace("[TaxonId]", taxon.Id.ToString());
        }
    }
}

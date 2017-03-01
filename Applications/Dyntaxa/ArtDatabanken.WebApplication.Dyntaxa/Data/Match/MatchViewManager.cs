using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Match
{
    public class MatchViewManager
    {
        private IUserContext _user;

        public MatchViewManager(IUserContext user)
        {
            _user = user;
        }

        public MatchSettingsViewModel GetMatchSettingsViewModel(ITaxon taxon)
        {
            var model = new MatchSettingsViewModel();

            model.MatchInputType = MatchTaxonInputType.ExcelFile;
            model.OutputTaxonId = true;
            model.OutputScientificName = true;                        

            //this._taxon = taxon;
            model.TaxonId = taxon.Id.ToString();            
            model.TaxonCategory = taxon.Category.Name;
            model.ScientificName = taxon.ScientificName;
            model.Author = taxon.Author;
            model.CommonName = taxon.CommonName.IsNotEmpty() ? taxon.CommonName : "-";

            //if (taxonCategory.SortOrder < Resources.DyntaxaSettings.Default.GenusTaxonCategoryId)
            //{
            model.LimitToParentTaxonId = taxon.Id;
            model.LimitToTaxonLabel = Resources.DyntaxaResource.MatchOptionsLimitToTaxonLabel.Replace("[TaxonName]", taxon.GetLabel());
            model.SearchOptions.RestrictToTaxonId = taxon.Id;
            //model.SearchOptions.RestrictToTaxonId = model.LimitToParentTaxonId;
            model.SearchOptions.RestrictToTaxonDescription = model.LimitToTaxonLabel;
                //Limit match to [TaxonName]
            //}            

            return model;
        }
    }
}

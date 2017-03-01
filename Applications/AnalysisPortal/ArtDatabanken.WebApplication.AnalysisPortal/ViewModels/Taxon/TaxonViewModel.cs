using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon
{
    /// <summary>
    /// A view model representing a Taxon.
    /// </summary>
    public class TaxonViewModel
    {
        public string ScientificName { get; set; }
        public string Author { get; set; }
        public string CommonName { get; set; }
        public string Category { get; set; }
        public int TaxonId { get; set; }
        public TaxonAlertStatusId TaxonStatus { get; set; }
        public SpeciesProtectionLevelEnum SpeciesProtectionLevel { get; set; }

        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(CommonName))
                {
                    return ScientificName;
                }

                return string.Format("{0} - {1}", ScientificName, CommonName);
            }            
        }

        public static TaxonViewModel CreateFromTaxon(ITaxon taxon)
        {
            var model = new TaxonViewModel();
            model.ScientificName = taxon.ScientificName;
            model.CommonName = taxon.CommonName;
            model.Author = taxon.Author;
            model.TaxonId = taxon.Id;
            model.Category = taxon.Category.Name;
            model.TaxonStatus = (TaxonAlertStatusId)taxon.AlertStatus.Id;
            return model;
        }
    }
}

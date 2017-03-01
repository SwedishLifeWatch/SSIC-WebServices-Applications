using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon
{
    /// <summary>
    /// A ViewvModel represnting a Taxon with related number of observed species.
    /// </summary>
    public class TaxonSpeciesObservationCountViewModel
    {
        public string ScientificName { get; set; }
        public string Author { get; set; }
        public string CommonName { get; set; }
        public string Category { get; set; }
        public int TaxonId { get; set; }
        public TaxonAlertStatusId TaxonStatus { get; set; }
        public int SpeciesObservationCount { get; set; }

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

        public static TaxonSpeciesObservationCountViewModel CreateFromTaxon(ITaxonSpeciesObservationCount taxonSpeciesObservationCount)
        {
            var model = new TaxonSpeciesObservationCountViewModel();

            model.ScientificName = taxonSpeciesObservationCount.ScientificName;
            model.CommonName = taxonSpeciesObservationCount.CommonName;
            model.Author = taxonSpeciesObservationCount.Author;
            model.TaxonId = taxonSpeciesObservationCount.Id;
            model.Category = taxonSpeciesObservationCount.Category.Name;
            model.TaxonStatus = (TaxonAlertStatusId)taxonSpeciesObservationCount.AlertStatus.Id;
            model.SpeciesObservationCount = taxonSpeciesObservationCount.SpeciesObservationCount;
            return model;
        }
    }
}
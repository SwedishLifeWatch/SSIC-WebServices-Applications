using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Observations;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result
{
    public class ResultObservationsListItem
    {
        public string ObservationId { get; set; }
        public string ScientificName { get; set; }
        public string CommonName { get; set; }
        public string ObservationDate { get; set; }
        public string RecordedBy { get; set; }
        public string TaxonId { get; set; }
        public string Locality { get; set; }
        public string Description { get; set; }

        public static ResultObservationsListItem Create(SpeciesObservationViewModel viewModel)
        {
            ResultObservationsListItem resultModel = new ResultObservationsListItem();
            resultModel.ObservationId = viewModel.ObservationId;
            resultModel.ScientificName = viewModel.ScientificName;
            resultModel.ObservationDate = viewModel.Start;
            resultModel.CommonName = viewModel.VernacularName;
            resultModel.RecordedBy = viewModel.RecordedBy;
            resultModel.TaxonId = viewModel.TaxonID;
            resultModel.Locality = viewModel.Locality;
            resultModel.Description = string.Format("{0} - {1}", viewModel.Start, viewModel.ScientificName);

            return resultModel;
        }
    }
}

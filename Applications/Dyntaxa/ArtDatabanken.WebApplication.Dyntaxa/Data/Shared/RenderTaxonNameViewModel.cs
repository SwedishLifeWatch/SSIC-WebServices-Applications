using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public class RenderTaxonNameViewModel
    {
        public bool IsPublicMode { get; set; }
        public string Label { get; set; }
        public string ReferenceViewAction { get; set; }
        public TaxonNameViewModel TaxonName { get; set; }
        public string ReturnAction { get; set; }
        public string ReturnController { get; set; }
        public int? TaxonId { get; set; }

        public RenderTaxonNameViewModel(string label, string referenceViewAction, TaxonNameViewModel taxonName, bool isPublicMode, string returnAction, string returnController, int? taxonId)
        {
            Label = label;
            ReferenceViewAction = referenceViewAction;
            TaxonName = taxonName;
            IsPublicMode = isPublicMode;
            ReturnAction = returnAction;
            ReturnController = returnController;
            TaxonId = taxonId;
        }
    }
}

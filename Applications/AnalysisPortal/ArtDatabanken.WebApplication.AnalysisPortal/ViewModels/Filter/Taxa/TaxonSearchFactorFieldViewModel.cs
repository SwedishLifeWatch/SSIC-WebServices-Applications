using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Taxa
{
    public class TaxonSearchFactorFieldViewModel
    {
        public Int32 Id { get; set; }

        public Int32 FactorFieldTypeId { get; set; }

        public CompareOperator CompareOperator { get; set; }

        public Boolean CompareOperatorIsSpecified { get; set; }

        public String FactorFieldTypeValue { get; set; }
    }
}

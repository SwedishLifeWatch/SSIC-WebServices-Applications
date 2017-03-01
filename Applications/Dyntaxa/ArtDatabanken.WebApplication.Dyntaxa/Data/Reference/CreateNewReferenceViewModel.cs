using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Reference
{
    public class CreateNewReferenceViewModel
    {
        public ReferenceViewModel Reference { get; set; }

        public ModelLabels Labels
        {
            get { return _labels; }
        }
        private readonly ModelLabels _labels = new ModelLabels();

        public class ModelLabels
        {
            //public string TitleLabel = "Create new reference";
            //public string CreateNewReferenceLabel = "Create new";            

            public string HierarchicalTitleLabel
            {
                get { return Resources.DyntaxaResource.ExportHierarchicalTitle; }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resources;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Reference
{
    /// <summary>
    /// View model used to present information about an object
    /// that is looked up by Guid
    /// </summary>
    public class GuidObjectViewModel
    {
        public string GUID { get; set; }
        public string Description { get; set; }
        public string TypeDescription { get; set; }
        public string ID { get; set; }

        public ModelLabels Labels
        {
            get { return _labels; }
        }
        private readonly ModelLabels _labels = new ModelLabels();

        /// <summary>
        /// Localized labels
        /// </summary>
        public class ModelLabels
        {
            public string GuidLabel { get { return DyntaxaResource.ReferenceGuidObjectInfoGuidLabel; } }
            public string TypeDescriptionLabel { get { return DyntaxaResource.ReferenceGuidObjectInfoTypeDescriptionLabel; } }
            public string DescriptionLabel { get { return DyntaxaResource.ReferenceGuidObjectInfoDescriptionLabel; } }
            public string IdLabel { get { return DyntaxaResource.ReferenceGuidObjectInfoIdLabel; } }            
        }
    }
}

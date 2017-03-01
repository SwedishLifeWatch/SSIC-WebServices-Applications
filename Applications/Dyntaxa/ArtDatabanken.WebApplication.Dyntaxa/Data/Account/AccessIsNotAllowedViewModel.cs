using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    public class AccessIsNotAllowedViewModel
    {
        public string Url { get; set; }

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
            public string Title { get { return Resources.DyntaxaResource.AccountAccessIsNotAllowedTitle; } }
            public string AccessIsNotAllowed { get { return Resources.DyntaxaResource.AccountAccessIsNotAllowedDescription; } }
        }
    }
}

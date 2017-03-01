using System.Web.Mvc;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Reflection;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ErrorViewModel
    {
        public ErrorViewModel(Exception exception, string controllerName, string actionName) // : base(exception, controllerName, actionName)
        {
        }

        public ErrorViewModel()
           // : base(null, null, null)
        {
        }

        private readonly ErrorViewModelLabels labels = new ErrorViewModelLabels();

        /// <summary>
        /// Gets or sets the Error Title.
        /// </summary>
        public string ErrorTitleHeader { get; set; }

        /// <summary>
        /// Gets or sets the error main header.
        /// </summary>
        public string ErrorMainHeader { get; set; }

        /// <summary>
        /// Gets or sets error information.
        /// </summary>
        public string ErrorInformationText { get; set; }

        /// <summary>
        /// Gets or sets additional error information.
        /// </summary>
        public string AdditionalErrorInformationText { get; set; }

        /// <summary>
        /// Gets or sets id for data to be transferred.
        /// </summary>
        public string TaxonId { get; set; }

        /// <summary>
        /// Gets or sets test on error button.
        /// </summary>
        public string ErrorButtonText { get; set; }

        /// <summary>
        /// Which action to performe when pushing error button
        /// </summary>
        public string ErrorAction { get; set; }

        /// <summary>
        /// Which controller to performe when pushing error button
        /// </summary>
        public string ErrorController { get; set; }

        /// <summary>
        /// Action causing this error
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Controller causing this error
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Gets or sets revision id for data to be transferred.
        /// </summary>
        public string RevisionId { get; set; }

        /// <summary>
        /// All localized labels
        /// </summary>
        public ErrorViewModelLabels Labels
        {
            get { return labels; }
        }

        /// <summary>
        /// Localized labels used in add revision view
        /// </summary>
        public class ErrorViewModelLabels
        {
            public string AdditionErrorLabel
            {
                get { return Resources.DyntaxaResource.SharedError + ": " + Resources.DyntaxaResource.SharedAdditionalErrorInformation; }
            }

            public string ErrorLabel
            {
                get { return Resources.DyntaxaResource.SharedError + ": "; }
            }
        }
    }
}

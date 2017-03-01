// -----------------------------------------------------------------------
// <copyright file="BaseViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using ArtDatabanken.Data;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BaseViewModel
    {
        /// <summary>
        /// Gets or sets the application path.
        /// </summary>
        public string ApplicationPath { get; set; }

        /// <summary>
        /// Gets or sets the current User model object.
        /// </summary>
        public User CurrentUser { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user is authenticated or not.
        /// </summary>
        public bool IsCurrentUserAuthenticated { get; set; }

        /// <summary>
        /// Gets or sets CurrentRole.
        /// </summary>
        public string CurrentRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ApplyAuthorizationFilter.
        /// This property is to be used in combination with SecuredEntityTypeIdentifier in order to enable Authorization Filtering in GUI Inputs
        /// Primary usage is in TaxonPicker
        /// </summary>
        public bool ApplyAuthorizationFilter { get; set; }

        public string SessionTaxonId { get; set; }

        public string SessionRevisionId { get; set; }

        public string Language { get; set; }

        /// <summary>
        /// Gets Base view model labels.
        /// </summary>
        public BaseViewModelLabels BaseLabels
        {
            get { return _labels; }
        }

        private readonly BaseViewModelLabels _labels = new BaseViewModelLabels();
        
        public class BaseViewModelLabels
        {
            public string NoDataMessage
            {
                get
                {
                    return Resources.DyntaxaResource.ErrorNoData;
                    //TODO: Refactor to this
                    //{ return Resources.DyntaxaResource.BaseNoDataLabel;
                }
            }

            public string ReferenceLabel
            {
                get { return Resources.DyntaxaResource.SharedReferenceLabel; }
            }
        }
    }
}

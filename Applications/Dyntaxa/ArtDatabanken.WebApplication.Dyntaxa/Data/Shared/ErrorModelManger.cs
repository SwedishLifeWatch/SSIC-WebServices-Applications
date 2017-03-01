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
    public class ErrorModelManager
    {
        private Exception exception = null;
        private string controllerName = string.Empty;
        private string actionName = string.Empty;

        public ErrorModelManager(Exception exception, string controllerName, string actionName) // : base(exception, controllerName, actionName)
        {
            this.exception = exception;
            this.controllerName = controllerName;
            this.actionName = actionName;
        }

        public ErrorViewModel GetErrorViewModel(string errorTitle, string errorHeader, string errorMessage, string taxonId, string revisionId, string addtionalErrorInformation)
        {
            ErrorViewModel errorModel = new ErrorViewModel(exception, controllerName, actionName);
            errorModel.ErrorTitleHeader = errorTitle;
            errorModel.ErrorMainHeader = errorHeader;
            errorModel.ErrorInformationText = errorMessage;
            errorModel.ErrorButtonText = Resources.DyntaxaResource.SharedOkButtonText;
            errorModel.ErrorAction = actionName;
            errorModel.ErrorController = controllerName;
            errorModel.TaxonId = taxonId;
            errorModel.RevisionId = revisionId;
            errorModel.AdditionalErrorInformationText = addtionalErrorInformation;
            return errorModel;
        }

        public ErrorViewModel GetErrorViewModel(string errorTitle, string errorHeader, string errorMessage, string taxonId, string addtionalErrorInformation)
        {
            ErrorViewModel errorModel = new ErrorViewModel(exception, controllerName, actionName);
            errorModel.ErrorTitleHeader = errorTitle;
            errorModel.ErrorMainHeader = errorHeader;
            errorModel.ErrorInformationText = errorMessage;
            errorModel.ErrorButtonText = Resources.DyntaxaResource.SharedOkButtonText;
            errorModel.ErrorAction = actionName;
            errorModel.ErrorController = controllerName;
            errorModel.TaxonId = taxonId;
            errorModel.RevisionId = string.Empty;
            errorModel.AdditionalErrorInformationText = addtionalErrorInformation;
            return errorModel;
        }

        public ErrorViewModel GetErrorViewModel(string errorTitle, string errorHeader, string errorMessage, string addtionalErrorInformation)
        {
            ErrorViewModel errorModel = new ErrorViewModel(exception, controllerName, actionName);
            errorModel.ErrorTitleHeader = errorTitle;
            errorModel.ErrorMainHeader = errorHeader;
            errorModel.ErrorInformationText = errorMessage;
            errorModel.ErrorButtonText = Resources.DyntaxaResource.SharedOkButtonText;
            errorModel.ErrorAction = actionName;
            errorModel.ErrorController = controllerName;
            errorModel.TaxonId = string.Empty;
            errorModel.RevisionId = string.Empty;
            errorModel.AdditionalErrorInformationText = addtionalErrorInformation;
            return errorModel;
        }
    }
}

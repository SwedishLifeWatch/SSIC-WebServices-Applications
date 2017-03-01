using System;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Error;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.General
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ErrorViewManager : ViewManagerBase
    {
        private Exception exception = null;
        private string controllerName = string.Empty;
        private string actionName = string.Empty;

        public ErrorViewManager(Exception exception, string controllerName, string actionName)
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
            errorModel.ErrorButtonText = Resources.Resource.SharedOkButtonText;
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
            errorModel.ErrorButtonText = Resources.Resource.SharedOkButtonText;
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
            errorModel.ErrorButtonText = Resources.Resource.SharedOkButtonText;
            errorModel.ErrorAction = actionName;
            errorModel.ErrorController = controllerName;
            errorModel.TaxonId = string.Empty;
            errorModel.RevisionId = string.Empty;
            errorModel.AdditionalErrorInformationText = addtionalErrorInformation;
            return errorModel;
        }
    }
}

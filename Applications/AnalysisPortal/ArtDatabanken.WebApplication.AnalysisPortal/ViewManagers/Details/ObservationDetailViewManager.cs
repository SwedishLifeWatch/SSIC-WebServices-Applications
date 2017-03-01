using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Details;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Details
{
    public class ObservationDetailViewManager : ViewManagerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservationDetailViewManager"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="mySettings">The MySettings object.</param>
        public ObservationDetailViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        /// <summary>
        /// Creates the observation details view model.
        /// </summary>
        /// <param name="observationId">The observation identifier.</param>
        /// <param name="showAsDialog">if set to <c>true</c> the create models ShowAsDialog property is set to true.</param>
        /// <returns>A observation detail view model.</returns>
        public ObservationDetailViewModel CreateObservationDetailsViewModel(string observationId, bool showAsDialog)
        {
            ObservationDetailViewModel model = new ObservationDetailViewModel();
            model.ObservationId = observationId;
            model.ShowAsDialog = showAsDialog;
            try
            {
                int obsId = int.Parse(observationId);
                SpeciesObservationDataManager speciesObservationDataManager = new SpeciesObservationDataManager(UserContext, MySettings);
                var speciesObservationViewModel = speciesObservationDataManager.GetSpeciesObservationViewModel(obsId);
                model.Projects = speciesObservationViewModel.Projects;                
                model.Fields = speciesObservationDataManager.GetObservationDetailFields(speciesObservationViewModel);                
                RemoveNewLinesInValueString(speciesObservationDataManager, model);
            }
            catch { }
            return model;            
        }

        /// <summary>
        /// Removes new lines chars in value string.
        /// </summary>
        /// <param name="speciesObservationDataManager">The species observation data manager.</param>
        /// <param name="model">The model.</param>
        private void RemoveNewLinesInValueString(
            SpeciesObservationDataManager speciesObservationDataManager, 
            ObservationDetailViewModel model)
        {
            speciesObservationDataManager.RemoveNewLinesInValueString(model.Fields);
            foreach (var projectViewModel in model.Projects)
            {
                speciesObservationDataManager.RemoveNewLinesInValueString(projectViewModel.ProjectParameters.Values);
            }
        }
    }
}

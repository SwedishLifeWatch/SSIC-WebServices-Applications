using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Observations;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Table;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.Observations
{
    /// <summary>
    /// This class is used to cache species observation data.     
    /// </summary>        
    public class SpeciesObservationsData
    {        
        public SpeciesObservationList SpeciesObservationList { get; set; }
        
        public List<SpeciesObservationViewModel> SpeciesObservationViewModels { get; set; }

        public SpeciesObservationsData(
            SpeciesObservationList speciesObservationList, 
            SpeciesObservationFieldDescriptionsViewModel fieldDescriptionsViewModel)
        {
            if (speciesObservationList.IsNull())
            {
                speciesObservationList = new SpeciesObservationList();
            }

            SpeciesObservationList = speciesObservationList;
            SpeciesObservationViewModels = speciesObservationList.GetGenericList().ToObservationDarwinCoreViewModelList(fieldDescriptionsViewModel);
        }
    }
}

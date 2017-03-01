using System.Collections.Generic;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Observations;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Table;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions
{
    /// <summary>
    /// Extension methods to ISpeciesObservationExtension interface.
    /// </summary>
    public static class ISpeciesObservationExtension
    {
        /// <summary>
        /// Gets a string with the scientific name, author and common name.
        /// </summary>
        /// <param name="obs">The obsevation.</param>
        /// <returns>A string with the scientific name, author and common name.</returns>
        public static string GetLabel(this ISpeciesObservation obs)
        {
            if (obs == null || obs.Taxon.ScientificName.IsEmpty())
            {
                return "";
            }

            var str = new StringBuilder();
            str.Append(obs.Taxon.ScientificName);
            //if (string.IsNullOrEmpty(obs.Taxon.Author))
            //{
            //    str.Append(" " + obs.Author);
            //}

            //if (obs.Taxon.CommonName.IsNotEmpty())
            //{
            //    str.Append(", " + obs.CommonName);
            //}
            return str.ToString();
        }

        /// <summary>
        /// Converts a ISpeciesObservations to a ObservationViewModel which is used
        /// to present the observation on screen.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <returns></returns>
        public static ObservationViewModel ToObservationViewModel(this ISpeciesObservation observation)
        {
            return ObservationViewModel.Create(observation);
        }

        /// <summary>
        /// Converts a list of ISpeciesObservations to a list with ObservationViewModel which is used
        /// to present the observations on screen.
        /// </summary>
        /// <param name="observations">The observations.</param>
        /// <returns></returns>
        public static List<ObservationViewModel> ToObservationViewModelList(this IEnumerable<ISpeciesObservation> observations)
        {
            var list = new List<ObservationViewModel>();
            
            if (observations != null)
            {
                foreach (ISpeciesObservation obs in observations)
                {
                    list.Add(ObservationViewModel.Create(obs));
                }
            }
            return list;
        }

        /// <summary>
        /// Converts a list of ISpeciesObservations to a list with ObservationDarwinCoreViewModel which is used
        /// to present the observations on screen.
        /// </summary>
        /// <param name="observations">The observations.</param>
        /// <param name="fieldDescriptionsViewModel">Field description view model.</param>
        /// <returns></returns>
        public static List<SpeciesObservationViewModel> ToObservationDarwinCoreViewModelList(
            this IEnumerable<ISpeciesObservation> observations,
            SpeciesObservationFieldDescriptionsViewModel fieldDescriptionsViewModel)
        {            
            var list = new List<SpeciesObservationViewModel>();
            if (observations != null)
            {
                foreach (ISpeciesObservation obs in observations)
                {                    
                    list.Add(SpeciesObservationViewModel.CreateFromSpeciesObservation(obs, fieldDescriptionsViewModel));
                }
            }            
            return list;
        }
    }
}

using System;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Database;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to the WebSpeciesObservationFieldDescription class.
    /// </summary>
    public static class WebSpeciesObservationFieldDescriptionExtension
    {
        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='webSpeciesObservationFieldDescription'>The Species Observation Field Description.</param>
        public static void CheckData(this WebSpeciesObservationFieldDescription webSpeciesObservationFieldDescription)
        {
            if (!webSpeciesObservationFieldDescription.IsDataChecked)
            {
                webSpeciesObservationFieldDescription.CheckStrings();
                webSpeciesObservationFieldDescription.IsDataChecked = true;
            }
        }
    }
}

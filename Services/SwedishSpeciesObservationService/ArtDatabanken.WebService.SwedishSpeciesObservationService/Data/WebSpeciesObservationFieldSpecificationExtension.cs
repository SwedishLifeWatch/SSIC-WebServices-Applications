using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Data
{
    /// <summary>
    /// Contains extension methods to the WebSpeciesObservationFieldSpecification class.
    /// </summary>
    public static class WebSpeciesObservationFieldSpecificationExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="fieldSpecification">Species observation field specification.</param>
        public static void CheckData(this WebSpeciesObservationFieldSpecification fieldSpecification)
        {
            if (fieldSpecification.IsNotNull())
            {
                fieldSpecification.Class.CheckNotNull("fieldSpecification.Class");
                fieldSpecification.Class.CheckData();
                fieldSpecification.Property.CheckNotNull("fieldSpecification.Property");
                fieldSpecification.Property.CheckData();
            }
        }
    }
}

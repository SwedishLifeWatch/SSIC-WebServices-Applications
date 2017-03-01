using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to the WebSpeciesObservationClass class.
    /// </summary>
    public static class WebSpeciesObservationClassExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="speciesObservationClass">Species observation class.</param>
        public static void CheckData(this WebSpeciesObservationClass speciesObservationClass)
        {
            if (speciesObservationClass.IsNotNull())
            {
                speciesObservationClass.Identifier = speciesObservationClass.Identifier.CheckJsonInjection();
                if (speciesObservationClass.GetClass().IsEmpty())
                {
                    throw new ArgumentException("Species observation class has not been specified.");
                }
            }
        }

        /// <summary>
        /// Get class as string.
        /// </summary>
        /// <param name="speciesObservationClass">Species observation class.</param>
        /// <returns>Class as string.</returns>
        public static String GetClass(this WebSpeciesObservationClass speciesObservationClass)
        {
            String classString;

            classString = null;
            if (speciesObservationClass.IsNotNull())
            {
                if (speciesObservationClass.Id == SpeciesObservationClassId.None)
                {
                    classString = speciesObservationClass.Identifier;
                }
                else
                {
                    classString = speciesObservationClass.Id.ToString();
                }
            }

            return classString;
        }
    }
}

using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Not defined in Darwin Core.
    /// This interface contains onservation related information about the taxon that
    /// the species observation is attached to.
    /// </summary>
    public interface IDarwinCoreConservation
    {
        /// <summary>
        /// Not defined in Darwin Core.
        /// This property indicates whether the species is the subject
        /// of an action plan ('åtgärdsprogram' in swedish).
        /// </summary>
        Boolean ActionPlan
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// This property indicates whether a species has been
        /// classified as nature conservation relevant
        /// ('naturvårdsintressant' in swedish).
        /// The concept 'nature conservation relevant' must be defined
        /// before this property can be used.
        /// </summary>
        Boolean ConservationRelevant
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// This property indicates whether
        /// the species is included in Natura 2000.
        /// </summary>
        Boolean Natura2000
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// This property indicates whether the species 
        /// is protected by the law in Sweden.
        /// </summary>
        Boolean ProtectedByLaw
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Information about how protected information
        /// about a species is in Sweden.
        /// Currently this is a value between 1 to 5.
        /// 1 indicates public access and 5 is the highest used security level.
        /// </summary>
        Int32 ProtectionLevel
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Redlist category for redlisted species. The property also
        /// contains information about which redlist that is referenced.
        /// Example value: CR (Sweden, 2010). Possible redlist values
        /// are DD (Data Deficient), EX (Extinct),
        /// RE (Regionally Extinct), CR (Critically Endangered),
        /// EN (Endangered), VU (Vulnerable), NT (Near Threatened).
        /// Not redlisted species has no value in this property.
        /// </summary>
        String RedlistCategory
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// This property contains information about the species
        /// immigration history.
        /// </summary>
        String SwedishImmigrationHistory
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Information about the species occurrence in Sweden.
        /// For example information about if the species reproduce
        /// in sweden.
        /// </summary>
        String SwedishOccurrence
        { get; set; }


    }
}

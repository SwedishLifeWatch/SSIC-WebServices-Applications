using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Not defined in Darwin Core.
    /// Conservation related information about the taxon that
    /// the species observation is attached to.
    /// </summary>
    [DataContract]
    public class WebDarwinCoreConservation : WebData
    {
        /// <summary>
        /// Not defined in Darwin Core.
        /// This property indicates whether the species is the subject
        /// of an action plan ('åtgärdsprogram' in swedish).
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public Boolean ActionPlan { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// This property indicates whether a species has been
        /// classified as nature conservation relevant
        /// ('naturvårdsintressant' in swedish).
        /// The concept 'nature conservation relevant' must be defined
        /// before this property can be used.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public Boolean ConservationRelevant { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// This property indicates whether
        /// the species is included in Natura 2000.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public Boolean Natura2000 { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// This property indicates whether the species 
        /// is protected by the law in Sweden.
        /// </summary>
        [DataMember]
        public Boolean ProtectedByLaw { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Information about how protected information
        /// about a species is in Sweden.
        /// Currently this is a value between 1 to 5.
        /// 1 indicates public access and 5 is the highest used security level.
        /// </summary>
        [DataMember]
        public Int32 ProtectionLevel { get; set; }

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
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public String RedlistCategory { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// This property contains information about the species
        /// immigration history.
        /// </summary>
        [DataMember]
        public String SwedishImmigrationHistory { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Information about the species occurrence in Sweden.
        /// For example information about if the species reproduce
        /// in sweden.
        /// </summary>
        [DataMember]
        public String SwedishOccurrence { get; set; }
    }
}

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains geological context information about a 
    /// species observation in Darwin Core 1.5 compatible format.
    /// Further information about the properties can
    /// be found at http://rs.tdwg.org/dwc/terms/
    /// This class is currently not used.
    /// </summary>
    [DataContract]
    public class WebDarwinCoreGeologicalContext : WebData
    {
        /// <summary>
        /// Darwin Core term name: bed.
        /// The full name of the lithostratigraphic bed from which
        /// the cataloged item was collected.
        /// This property is currently not used.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public String Bed { get; set; }

        /// <summary>
        /// Darwin Core term name: earliestAgeOrLowestStage.
        /// The full name of the earliest possible geochronologic
        /// age or lowest chronostratigraphic stage attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        /// This property is currently not used.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public String EarliestAgeOrLowestStage { get; set; }

        /// <summary>
        /// Darwin Core term name: earliestEonOrLowestEonothem.
        /// The full name of the earliest possible geochronologic eon
        /// or lowest chrono-stratigraphic eonothem or the informal
        /// name ("Precambrian") attributable to the stratigraphic
        /// horizon from which the cataloged item was collected.
        /// This property is currently not used.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public String EarliestEonOrLowestEonothem { get; set; }

        /// <summary>
        /// Darwin Core term name: earliestEpochOrLowestSeries.
        /// The full name of the earliest possible geochronologic
        /// epoch or lowest chronostratigraphic series attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        /// This property is currently not used.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public String EarliestEpochOrLowestSeries { get; set; }

        /// <summary>
        /// Darwin Core term name: earliestEraOrLowestErathem.
        /// The full name of the earliest possible geochronologic
        /// era or lowest chronostratigraphic erathem attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        /// This property is currently not used.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public String EarliestEraOrLowestErathem { get; set; }

        /// <summary>
        /// Darwin Core term name: earliestPeriodOrLowestSystem.
        /// The full name of the earliest possible geochronologic
        /// period or lowest chronostratigraphic system attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        /// This property is currently not used.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public String EarliestPeriodOrLowestSystem { get; set; }

        /// <summary>
        /// Darwin Core term name: formation.
        /// The full name of the lithostratigraphic formation from
        /// which the cataloged item was collected.
        /// This property is currently not used.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public String Formation { get; set; }

        /// <summary>
        /// Darwin Core term name: geologicalContextID.
        /// An identifier for the set of information associated
        /// with a GeologicalContext (the location within a geological
        /// context, such as stratigraphy). May be a global unique
        /// identifier or an identifier specific to the data set.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        // ReSharper disable once InconsistentNaming
        public String GeologicalContextID { get; set; }

        /// <summary>
        /// Darwin Core term name: group.
        /// The full name of the lithostratigraphic group from
        /// which the cataloged item was collected.
        /// This property is currently not used.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public String Group { get; set; }

        /// <summary>
        /// Darwin Core term name: highestBiostratigraphicZone.
        /// The full name of the highest possible geological
        /// biostratigraphic zone of the stratigraphic horizon
        /// from which the cataloged item was collected.
        /// This property is currently not used.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public String HighestBiostratigraphicZone { get; set; }

        /// <summary>
        /// Darwin Core term name: latestAgeOrHighestStage.
        /// The full name of the latest possible geochronologic
        /// age or highest chronostratigraphic stage attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        /// This property is currently not used.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public String LatestAgeOrHighestStage { get; set; }

        /// <summary>
        /// Darwin Core term name: latestEonOrHighestEonothem.
        /// The full name of the latest possible geochronologic eon
        /// or highest chrono-stratigraphic eonothem or the informal
        /// name ("Precambrian") attributable to the stratigraphic
        /// horizon from which the cataloged item was collected.
        /// This property is currently not used.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public String LatestEonOrHighestEonothem { get; set; }

        /// <summary>
        /// Darwin Core term name: latestEpochOrHighestSeries.
        /// The full name of the latest possible geochronologic
        /// epoch or highest chronostratigraphic series attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        /// This property is currently not used.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public String LatestEpochOrHighestSeries { get; set; }

        /// <summary>
        /// Darwin Core term name: latestEraOrHighestErathem.
        /// The full name of the latest possible geochronologic
        /// era or highest chronostratigraphic erathem attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        /// This property is currently not used.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public String LatestEraOrHighestErathem { get; set; }

        /// <summary>
        /// Darwin Core term name: latestPeriodOrHighestSystem.
        /// The full name of the latest possible geochronologic
        /// period or highest chronostratigraphic system attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        /// This property is currently not used.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public String LatestPeriodOrHighestSystem { get; set; }

        /// <summary>
        /// Darwin Core term name: lithostratigraphicTerms.
        /// The combination of all litho-stratigraphic names for
        /// the rock from which the cataloged item was collected.
        /// This property is currently not used.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public String LithostratigraphicTerms { get; set; }

        /// <summary>
        /// Darwin Core term name: lowestBiostratigraphicZone.
        /// The full name of the lowest possible geological
        /// biostratigraphic zone of the stratigraphic horizon
        /// from which the cataloged item was collected.
        /// This property is currently not used.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public String LowestBiostratigraphicZone { get; set; }

        /// <summary>
        /// Darwin Core term name: member.
        /// The full name of the lithostratigraphic member from
        /// which the cataloged item was collected.
        /// This property is currently not used.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here."), DataMember]
        public String Member { get; set; }
    }
}

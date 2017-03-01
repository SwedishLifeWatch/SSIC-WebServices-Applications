using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains geological context information about a 
    /// species observation when a flexible species observation format is 
    /// required. This class also includes all properties available 
    /// in Darwin Core 1.5 se class DarwinCoreGeologicalContext.
    /// Further information about the Darwin Core 1.5 properties can
    /// be found at http://rs.tdwg.org/dwc/terms/
    /// </summary>
    public class SpeciesObservationGeologicalContext : ISpeciesObservationGeologicalContext
    {
        /// <summary>
        /// Darwin Core term name: bed.
        /// The full name of the lithostratigraphic bed from which
        /// the cataloged item was collected.
        /// This property is currently not used.
        /// </summary>
        public String Bed
        { get; set; }

        /// <summary>
        /// Darwin Core term name: earliestAgeOrLowestStage.
        /// The full name of the earliest possible geochronologic
        /// age or lowest chronostratigraphic stage attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        /// This property is currently not used.
        /// </summary>
        public String EarliestAgeOrLowestStage
        { get; set; }

        /// <summary>
        /// Darwin Core term name: earliestEonOrLowestEonothem.
        /// The full name of the earliest possible geochronologic eon
        /// or lowest chrono-stratigraphic eonothem or the informal
        /// name ("Precambrian") attributable to the stratigraphic
        /// horizon from which the cataloged item was collected.
        /// This property is currently not used.
        /// </summary>
        public String EarliestEonOrLowestEonothem
        { get; set; }

        /// <summary>
        /// Darwin Core term name: earliestEpochOrLowestSeries.
        /// The full name of the earliest possible geochronologic
        /// epoch or lowest chronostratigraphic series attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        /// This property is currently not used.
        /// </summary>
        public String EarliestEpochOrLowestSeries
        { get; set; }

        /// <summary>
        /// Darwin Core term name: earliestEraOrLowestErathem.
        /// The full name of the earliest possible geochronologic
        /// era or lowest chronostratigraphic erathem attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        /// This property is currently not used.
        /// </summary>
        public String EarliestEraOrLowestErathem
        { get; set; }

        /// <summary>
        /// Darwin Core term name: earliestPeriodOrLowestSystem.
        /// The full name of the earliest possible geochronologic
        /// period or lowest chronostratigraphic system attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        /// This property is currently not used.
        /// </summary>
        public String EarliestPeriodOrLowestSystem
        { get; set; }

        /// <summary>
        /// Darwin Core term name: formation.
        /// The full name of the lithostratigraphic formation from
        /// which the cataloged item was collected.
        /// This property is currently not used.
        /// </summary>
        public String Formation
        { get; set; }

        /// <summary>
        /// Darwin Core term name: geologicalContextID.
        /// An identifier for the set of information associated
        /// with a GeologicalContext (the location within a geological
        /// context, such as stratigraphy). May be a global unique
        /// identifier or an identifier specific to the data set.
        /// This property is currently not used.
        /// </summary>
        public String GeologicalContextID
        { get; set; }

        /// <summary>
        /// Darwin Core term name: group.
        /// The full name of the lithostratigraphic group from
        /// which the cataloged item was collected.
        /// This property is currently not used.
        /// </summary>
        public String Group
        { get; set; }

        /// <summary>
        /// Darwin Core term name: highestBiostratigraphicZone.
        /// The full name of the highest possible geological
        /// biostratigraphic zone of the stratigraphic horizon
        /// from which the cataloged item was collected.
        /// This property is currently not used.
        /// </summary>
        public String HighestBiostratigraphicZone
        { get; set; }

        /// <summary>
        /// Id for this DarwinCoreGeologicalContext.
        /// </summary>
        public Int32? Id
        { get; set; }

        /// <summary>
        /// Darwin Core term name: latestAgeOrHighestStage.
        /// The full name of the latest possible geochronologic
        /// age or highest chronostratigraphic stage attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        /// This property is currently not used.
        /// </summary>
        public String LatestAgeOrHighestStage
        { get; set; }

        /// <summary>
        /// Darwin Core term name: latestEonOrHighestEonothem.
        /// The full name of the latest possible geochronologic eon
        /// or highest chrono-stratigraphic eonothem or the informal
        /// name ("Precambrian") attributable to the stratigraphic
        /// horizon from which the cataloged item was collected.
        /// This property is currently not used.
        /// </summary>
        public String LatestEonOrHighestEonothem
        { get; set; }

        /// <summary>
        /// Darwin Core term name: latestEpochOrHighestSeries.
        /// The full name of the latest possible geochronologic
        /// epoch or highest chronostratigraphic series attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        /// This property is currently not used.
        /// </summary>
        public String LatestEpochOrHighestSeries
        { get; set; }

        /// <summary>
        /// Darwin Core term name: latestEraOrHighestErathem.
        /// The full name of the latest possible geochronologic
        /// era or highest chronostratigraphic erathem attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        /// This property is currently not used.
        /// </summary>
        public String LatestEraOrHighestErathem
        { get; set; }

        /// <summary>
        /// Darwin Core term name: latestPeriodOrHighestSystem.
        /// The full name of the latest possible geochronologic
        /// period or highest chronostratigraphic system attributable
        /// to the stratigraphic horizon from which the cataloged
        /// item was collected.
        /// This property is currently not used.
        /// </summary>
        public String LatestPeriodOrHighestSystem
        { get; set; }

        /// <summary>
        /// Darwin Core term name: lithostratigraphicTerms.
        /// The combination of all litho-stratigraphic names for
        /// the rock from which the cataloged item was collected.
        /// This property is currently not used.
        /// </summary>
        public String LithostratigraphicTerms
        { get; set; }

        /// <summary>
        /// Darwin Core term name: lowestBiostratigraphicZone.
        /// The full name of the lowest possible geological
        /// biostratigraphic zone of the stratigraphic horizon
        /// from which the cataloged item was collected.
        /// This property is currently not used.
        /// </summary>
        public String LowestBiostratigraphicZone
        { get; set; }

        /// <summary>
        /// Darwin Core term name: member.
        /// The full name of the lithostratigraphic member from
        /// which the cataloged item was collected.
        /// This property is currently not used.
        /// </summary>
        public String Member
        { get; set; }
    }
}

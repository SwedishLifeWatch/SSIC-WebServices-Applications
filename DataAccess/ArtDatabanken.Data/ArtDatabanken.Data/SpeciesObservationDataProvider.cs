using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about a
    /// species observation data source in swedish life watch.
    /// </summary>
    public class SpeciesObservationDataProvider : ISpeciesObservationDataProvider
    {
        /// <summary>
        /// Email address to contact person.
        /// </summary>
        public String ContactEmail { get; set; }

        /// <summary>
        /// Name of contact person.
        /// </summary>
        public String ContactPerson { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Information about the data source.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// GUID (Globally Unique Identifier) for this object.
        /// It is a LSID, which is unique for each version of
        /// the record holding the information included in this object. 
        /// </summary>
        public String Guid { get; set; }

        /// <summary>
        /// Id for the data source.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Name of the data source.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Number of non public species observations in data source.
        /// </summary>
        public Int64 NonPublicSpeciesObservationCount { get; set; }

        /// <summary>
        /// Name of the organization that is responsible
        /// for the data source.
        /// </summary>
        public String Organization { get; set; }

        /// <summary>
        /// Number of public species observations in data source.
        /// </summary>
        public Int64 PublicSpeciesObservationCount { get; set; }

        /// <summary>
        /// Total number of species observations in data source.
        /// </summary>
        public Int64 SpeciesObservationCount { get; set; }

        /// <summary>
        /// Web address (that can be used in a web browser) where more
        /// information about the data source can be found.
        /// </summary>
        public String Url { get; set; }
    }
}

using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about a
    /// species observation data source in swedish lifewatch.
    /// </summary>
    /// 
    public interface ISpeciesObservationDataProvider : IDataId32
    {
        /// <summary>
        /// Email address to contact person.
        /// </summary>
        String ContactEmail { get; set; }

        /// <summary>
        /// Name of contact person.
        /// </summary>
        String ContactPerson { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Information about the data source.
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// GUID (Globally Unique Identifier) for this object.
        /// It is a LSID, which is unique for each version of
        /// the record holding the information included in this object. 
        /// </summary>
        String Guid { get; set; }

        /// <summary>
        /// Name of the data source.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Number of non public species observations in data source.
        /// </summary>
        Int64 NonPublicSpeciesObservationCount { get; set; }

        /// <summary>
        /// Name of the organization that is responsible
        /// for the data source.
        /// </summary>
        String Organization { get; set; }

        /// <summary>
        /// Number of public species observations in data source.
        /// </summary>
        Int64 PublicSpeciesObservationCount { get; set; }

        /// <summary>
        /// Total number of species observations in data source.
        /// </summary>
        Int64 SpeciesObservationCount { get; set; }

        /// <summary>
        /// Web address (that can be used in a web browser) where more
        /// information about the data source can be found.
        /// </summary>
        String Url { get; set; } 
    }
}

using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handles search criteria used when 
    /// searching persons.
    /// </summary>
    public interface IPersonSearchCriteria
    {

        /// <summary>
        /// Find persons with a first name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        String FirstName
        { get; set; }

        /// <summary>
        /// Find persons with a full name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        String FullName
        { get; set; }

        /// <summary>
        /// Find persons that are
        /// owners of a spieces collection
        /// </summary>
        Boolean? HasSpiecesCollection
        { get; set; }

        /// <summary>
        /// Find persons with a last name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        String LastName
        { get; set; }
    }
}

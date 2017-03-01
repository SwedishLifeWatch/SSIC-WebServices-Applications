using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class handles search criteria used when 
    /// searching persons.
    /// </summary>
    public class PersonSearchCriteria : IPersonSearchCriteria
    {

        /// <summary>
        /// Find persons with a first name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        public String FirstName
        { get; set; }

        /// <summary>
        /// Find persons with a full name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        public String FullName
        { get; set; }

        /// <summary>
        /// Find persons that are
        /// owners of a spieces collection
        /// </summary>
        public Boolean? HasSpiecesCollection
        { get; set; }

        /// <summary>
        /// Find persons with a last name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        public String LastName
        { get; set; }
    }
}

using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class handles search criteria used when 
    /// searching roles.
    /// </summary>
    public class AuthoritySearchCriteria : IAuthoritySearchCriteria
    {

        /// <summary>
        /// Find authorities with an authority identifier set
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        public string AuthorityIdentifier
        { get; set; }

        /// <summary>
        /// Find authorities with an application identifier set
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        public string ApplicationIdentifier
        { get; set; }

        /// <summary>
        /// Find authorities with an authority data type identifier set
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        public string AuthorityDataTypeIdentifier
        { get; set; }

        /// <summary>
        /// Find authorities with an authority name set
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        public string AuthorityName
        { get; set; }
    }
}

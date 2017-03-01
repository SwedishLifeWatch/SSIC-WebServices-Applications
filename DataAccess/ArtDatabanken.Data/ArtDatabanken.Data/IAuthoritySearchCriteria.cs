using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handles search criteria used when 
    /// searching authorities.
    /// </summary>
    public interface IAuthoritySearchCriteria
    {
        /// <summary>
        /// Find authorities with an authority identifier set
        /// similar to the specified value.
        /// </summary>
        String AuthorityIdentifier
        { get; set; }

        /// <summary>
        /// Find authorities with an application identifier set
        /// similar to the specified value.
        /// </summary>
        String ApplicationIdentifier
        { get; set; }

        /// <summary>
        /// Find authorities with an authority data type identifier
        /// similar to the specified value.
        /// </summary>
        String AuthorityDataTypeIdentifier
        { get; set; }

        /// <summary>
        /// Find authorities with an authority name set
        /// similar to the specified value.
        /// </summary>
        String AuthorityName
        { get; set; }

    }
}

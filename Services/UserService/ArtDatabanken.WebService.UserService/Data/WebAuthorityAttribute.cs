using System;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Information about one authority attribute that
    /// is related to exactly one authority.
    /// </summary>
    public class WebAuthorityAttribute
    {
        /// <summary>
        /// Id for the authority that this attribute is related to.
        /// </summary>
        public Int32 AuthorityId { get; set; }

        /// <summary>
        /// Unique identification of the authority attribute.
        /// </summary>
        public String Guid { get; set; }

        /// <summary>
        /// Id for this authority attribute type.
        /// </summary>
        public Int32 TypeId { get; set; }
    }
}

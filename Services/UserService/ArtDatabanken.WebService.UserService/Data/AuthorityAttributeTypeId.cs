namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// AttributeType: Authentication attribute of different kinds.
    /// </summary>
    public enum AuthorityAttributeTypeId
    {
        /// <summary>
        /// Attribute is an action
        /// </summary>
        Actions = 5,

        /// <summary>
        /// Attribute is a factor
        /// </summary>
        Factors = 4,

        /// <summary>
        /// Attribute is a locality
        /// </summary>
        Localities = 6,

        /// <summary>
        /// Attribute is a project
        /// </summary>
        Projects = 1,

        /// <summary>
        /// Attribute is a region
        /// </summary>
        Regions = 3,

        /// <summary>
        /// Attribute is a taxa
        /// </summary>
        Taxa = 2
    }
}

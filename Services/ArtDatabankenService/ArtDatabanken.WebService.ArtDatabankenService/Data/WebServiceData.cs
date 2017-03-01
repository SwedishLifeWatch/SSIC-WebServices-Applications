namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// This class contains other classes that manages
    /// different types of data.
    /// This data is used by WebServiceContext.
    /// </summary>
    public class WebServiceData
    {
        /// <summary>
        /// Get/set application manager instance.
        /// </summary>
        public static IApplicationManager ApplicationManager
        { get; set; }

        /// <summary>
        /// Get/set authorization manager instance.
        /// </summary>
        public static IAuthorizationManager AuthorizationManager
        { get; set; }

        /// <summary>
        /// Get/set region manager instance.
        /// </summary>
        public static IRegionManager RegionManager
        { get; set; }

        /// <summary>
        /// Get/set user manager instance.
        /// </summary>
        public static ITaxonManager TaxonManager
        { get; set; }

        /// <summary>
        /// Get/set user manager instance.
        /// </summary>
        public static IUserManager UserManager
        { get; set; }

        /// <summary>
        /// Get/set web service manager instance.
        /// </summary>
        public static IWebServiceManager WebServiceManager
        { get; set; }
    }
}

namespace ArtDatabanken.Log
{
    /// <summary>
    /// Definition of properties that are stored in Application Insights.
    /// </summary>
    public enum TelemetryProperty
    {
        /// <summary>
        /// Identifier for the application that the user uses.
        /// </summary>
        ApplicationIdentifier,

        /// <summary>
        /// Client IP address.
        /// </summary>
        ClientIpAddress,

        /// <summary>
        /// Date and time when the user logged in.
        /// </summary>
        LoginDateTime,

        /// <summary>
        /// Id for locale (according to UserService) used in the request.
        /// </summary>
        LocaleId,

        /// <summary>
        /// Request parameter.
        /// </summary>
        Parameter,

        /// <summary>
        /// Protected species observation indication result.
        /// </summary>
        ProtectedSpeciesObservationIndication,

        /// <summary>
        /// Id for this web service request.
        /// </summary>
        RequestId, 

        /// <summary>
        /// Id for role (according to UserService) used in the request.
        /// </summary>
        RoleId,

        /// <summary>
        /// User id according to UserService.
        /// </summary>
        UserId
    }
}

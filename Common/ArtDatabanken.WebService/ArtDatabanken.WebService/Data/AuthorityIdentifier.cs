namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Enumeration of authority identifiers.
    /// </summary>
    public enum AuthorityIdentifier
    {
        /// <summary>
        /// Dyntaxa editor.
        /// </summary>
        DyntaxaTaxonEditation,

        /// <summary>
        /// Reference updater.
        /// </summary>
        EditReference,

        /// <summary>
        /// Species fact updater.
        /// </summary>
        EditSpeciesFacts,

        /// <summary>
        /// Access right to species observation.
        /// </summary>
        Sighting,

        /// <summary>
        /// Access right to indication of protected species observations.
        /// </summary>
        SightingIndication,

        /// <summary>
        /// Dyntaxa Revision Administrator
        /// </summary>
        TaxonRevisionAdministration,

        /// <summary>
        /// Web service administrator.
        /// </summary>
        WebServiceAdministrator,

        /// <summary>
        /// User admin administrator.
        /// </summary>
        UserAdministration,

        /// <summary>
        /// Support user.
        /// </summary>
        SupportEdit,

        /// <summary>
        /// Picture administration administrator
        /// </summary>
        PictureAdmin,

        /// <summary>
        /// Get pictures from picture service.
        /// </summary>
        ReadPicture,

        /// <summary>
        /// Access right to ArtFakta.
        /// </summary>
        SpeciesFact
    }
}

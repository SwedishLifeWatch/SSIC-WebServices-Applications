using System.Diagnostics.CodeAnalysis;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumeration of identifiers for applications.
    /// The enumeration is not complete.
    /// Using ToString() on an enumeration value should return a value
    /// that is the same as the application identifier.
    /// </summary>
    public enum ApplicationIdentifier
    {
        /// <summary>
        /// Identifier for application AnalysisService.
        /// </summary>
        AnalysisService,

        /// <summary>
        /// Identifier for application ArtDatabankenService.
        /// </summary>
        ArtDatabankenService,

        /// <summary>
        /// Identifier for application ArtDatabankenSOA.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "SOA")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Databanken")]
        // ReSharper disable once InconsistentNaming
        ArtDatabankenSOA,

        /// <summary>
        /// Identifier for application Dyntaxa.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dyntaxa")]
        Dyntaxa,

        /// <summary>
        /// Identifier for application EVA.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        EVA,

        /// <summary>
        /// Identifier for application GeoReferenceService.
        /// </summary>
        GeoReferenceService,

        /// <summary>
        /// Identifier for application PrintObs.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Obs")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        PrintObs,

        /// <summary>
        /// The latest version of red list web application.
        /// </summary>
        RedList,

        /// <summary>
        /// Identifier for web service ReferenceService.
        /// </summary>
        ReferenceService,

        /// <summary>
        /// Identifier for application SpeciesIdentification.
        /// </summary>
        SpeciesIdentification,

        /// <summary>
        /// Identifier for application SpeciesObservationHarvestService.
        /// </summary>
        SpeciesObservationHarvestService,

        /// <summary>
        /// Identifier for application SwedishSpeciesObservationService.
        /// </summary>
        SwedishSpeciesObservationService,

        /// <summary>
        /// Identifier for application SwedishSpeciesObservationSOAPService.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "SOAP")]
        // ReSharper disable once InconsistentNaming
        SwedishSpeciesObservationSOAPService,

        /// <summary>
        /// Identifier for application TaxonAttributeService.
        /// </summary>
        TaxonAttributeService,

        /// <summary>
        /// Identifier for application TaxonService.
        /// </summary>
        TaxonService,

        /// <summary>
        /// Identifier for application UserAdmin.
        /// </summary>
        UserAdmin,

        /// <summary>
        /// Identifier for application UserService.
        /// </summary>
        UserService,

        /// <summary>
        /// Identifier for application AnalysisPortal.
        /// </summary>
        AnalysisPortal,

        /// <summary>
        /// Identifier for application GeodataServiceUpdateTool.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Geodata")]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        GeodataServiceUpdateTool,

        /// <summary>
        /// Identifier for application WebAdministration.
        /// </summary>
        WebAdministration,

        /// <summary>
        /// Identifier for web service PictureService.
        /// </summary>
        PictureService,

        /// <summary>
        /// Identifier for application PictureAdmin
        /// </summary>
        PictureAdmin
    }
}

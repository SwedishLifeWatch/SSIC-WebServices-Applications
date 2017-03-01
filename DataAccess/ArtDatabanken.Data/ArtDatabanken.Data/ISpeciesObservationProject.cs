using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface gives access to information about the project 
    /// in which this species observation was made.
    /// Only some species observations belongs to a project.
    /// </summary>
    public interface ISpeciesObservationProject
    {
        /// <summary>
        /// Indicates if species observations that are reported in
        /// a project are publicly available or not.
        /// </summary>
        Boolean? IsPublic
        { get; set; }

        /// <summary>
        /// Information about the type of project,
        /// for example 'Environmental monitoring'.
        /// </summary>
        String ProjectCategory
        { get; set; }

        /// <summary>
        /// Description of a project.
        /// </summary>
        String ProjectDescription
        { get; set; }

        /// <summary>
        /// Date when the project ends.
        /// </summary>
        String ProjectEndDate
        { get; set; }

        /// <summary>
        /// An identifier for the project.
        /// In the absence of a persistent global unique identifier,
        /// construct one from a combination of identifiers in
        /// the project that will most closely make the ProjectID
        /// globally unique.
        /// The format LSID (Life Science Identifiers) is used as GUID
        /// (Globally unique identifier).
        /// </summary>
        String ProjectID
        { get; set; }

        /// <summary>
        /// Name of the project.
        /// </summary>
        String ProjectName
        { get; set; }

        /// <summary>
        /// Name of person or organization that owns the project.
        /// </summary>
        String ProjectOwner
        { get; set; }

        /// <summary>
        /// Information about species observation project parameters that are 
        /// related to this species observation.
        /// Each project parameter contains parameter information but also information
        /// about the project. The reason for this is that one species observation can
        /// belong to more than one project.
        /// </summary>
        SpeciesObservationProjectParameterList ProjectParameters { get; set; }

        /// <summary>
        /// Date when the project starts.
        /// </summary>
        String ProjectStartDate
        { get; set; }

        /// <summary>
        /// Web address that leads to more information about the
        /// project. The information should be accessible
        /// from the most commonly used web browsers.
        /// </summary>
        String ProjectURL
        { get; set; }

        /// <summary>
        /// Survey method used in a project to
        /// retrieve species observations.
        /// </summary>
        String SurveyMethod
        { get; set; }
    }
}

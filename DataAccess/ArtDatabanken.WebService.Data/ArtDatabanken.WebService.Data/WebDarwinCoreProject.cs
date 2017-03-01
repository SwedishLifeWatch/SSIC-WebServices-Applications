using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Not defined in Darwin Core.
    /// Information about the project in which this
    /// species observation was made.
    /// Only some species observations belongs to a project.
    /// </summary>
    [DataContract]
    public class WebDarwinCoreProject : WebData
    {
        /// <summary>
        /// Not defined in Darwin Core.
        /// Indicates if species observations that are reported in
        /// a project are publicly available or not.
        /// </summary>
        [DataMember]
        public Boolean IsPublic { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Information about the type of project,
        /// for example 'Environmental monitoring'.
        /// </summary>
        [DataMember]
        public String ProjectCategory { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Description of a project.
        /// </summary>
        [DataMember]
        public String ProjectDescription { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Date when the project ends.
        /// </summary>
        [DataMember]
        public String ProjectEndDate { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// An identifier for the project.
        /// In the absence of a persistent global unique identifier,
        /// construct one from a combination of identifiers in
        /// the project that will most closely make the ProjectID
        /// globally unique.
        /// The format LSID (Life Science Identifiers) is used as GUID
        /// (Globally unique identifier).
        /// </summary>
        [DataMember]
        // ReSharper disable once InconsistentNaming
        public String ProjectID { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Name of the project.
        /// </summary>
        [DataMember]
        public String ProjectName { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Name of person or organization that owns the project.
        /// </summary>
        [DataMember]
        public String ProjectOwner { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Date when the project starts.
        /// </summary>
        [DataMember]
        public String ProjectStartDate { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Web address that leads to more information about the
        /// project. The information should be accessible
        /// from the most commonly used web browsers.
        /// </summary>
        [DataMember]
        // ReSharper disable once InconsistentNaming
        public String ProjectURL { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Survey method used in a project to
        /// retrieve species observations.
        /// </summary>
        [DataMember]
        public String SurveyMethod { get; set; }
    }
}

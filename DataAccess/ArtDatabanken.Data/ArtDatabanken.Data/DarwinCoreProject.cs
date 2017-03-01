using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Not defined in Darwin Core.
    /// Information about the project in which this
    /// species observation was made.
    /// Only some species observations belongs to a project.
    /// </summary>
    public class DarwinCoreProject : IDarwinCoreProject
    {

        /// <summary>
        /// Not defined in Darwin Core.
        /// Indicates if species observations that are reported in
        /// a project are publicly available or not.
        /// </summary>
        public Boolean IsPublic
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Information about the type of project,
        /// for example 'Environmental monitoring'.
        /// </summary>
        public String ProjectCategory
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Description of a project.
        /// </summary>
        public String ProjectDescription
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Date when the project ends.
        /// </summary>
        public String ProjectEndDate
        { get; set; }

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
        public String ProjectID
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Name of the project.
        /// </summary>
        public String ProjectName
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Name of person or organization that owns the project.
        /// </summary>
        public String ProjectOwner
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Date when the project starts.
        /// </summary>
        public String ProjectStartDate
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Web address that leads to more information about the
        /// project. The information should be accessible
        /// from the most commonly used web browsers.
        /// </summary>
        public String ProjectURL
        { get; set; }

        /// <summary>
        /// Not defined in Darwin Core.
        /// Survey method used in a project to
        /// retrieve species observations.
        /// </summary>
        public String SurveyMethod
        { get; set; }
    }
}

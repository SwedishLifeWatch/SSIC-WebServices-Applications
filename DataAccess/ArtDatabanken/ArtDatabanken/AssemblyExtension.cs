using System;
using System.Reflection;

namespace ArtDatabanken
{
    /// <summary>
    /// Contains extension methods to the Assembly class.
    /// </summary>
    public static class AssemblyExtension
    {
        /// <summary>
        /// Get name of application.
        /// </summary>
        /// <param name='assembly'>Assembly related to application.</param>
        /// <returns>Application name.</returns>
        public static String GetApplicationName(this Assembly assembly)
        {
            return assembly.GetName().Name;
        }

        /// <summary>
        /// Get version of application.
        /// </summary>
        /// <param name='assembly'>Assembly related to application.</param>
        /// <returns>Application version.</returns>
        public static String GetApplicationVersion(this Assembly assembly)
        {
            return assembly.GetName().Version.Major + "." +
                   assembly.GetName().Version.Minor + "." +
                   assembly.GetName().Version.Build + "." +
                   assembly.GetName().Version.Revision;
        }
    }
}

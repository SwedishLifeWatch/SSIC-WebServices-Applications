using System;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Contains extension methods to the WebPerson class.
    /// </summary>
    public static class WebPersonExtension
    {
        /// <summary>
        /// Get full name of person.
        /// </summary>
        /// <param name='person'>Person.</param>
        /// <returns>A version of text that is safe to use.</returns>
        public static String GetFullName(this WebPerson person)
        {
            return person.FirstName + " " + person.LastName;
        }
    }
}

using System;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension methods to the WebPerson class.
    /// </summary>
    public static class WebPersonExtension
    {
        /// <summary>
        /// Get full name of a person.
        /// </summary>
        /// <param name='person'>The person instance.</param>
        /// <returns>Full name of a person.</returns>
        public static String GetFullName(this WebPerson person)
        {
            return person.FirstName + " " + person.LastName;
        }
    }
}

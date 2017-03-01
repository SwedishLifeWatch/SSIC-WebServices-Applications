using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumerates the different types of users that web services handles.
    /// </summary>
    [DataContract]
    public enum UserType
    {
        /// <summary>
        /// User is an application (web service, web application
        /// or localy installed application).
        /// </summary>
        [EnumMember]
        Application,
        /// <summary>
        /// User is a person.
        /// </summary>
        [EnumMember]
        Person
    }
}

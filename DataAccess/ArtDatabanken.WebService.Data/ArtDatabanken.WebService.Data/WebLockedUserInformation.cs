using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Information about a user who is currently locked out from ArtDatabankenSOA.
    /// Users are locked out if the fail to login a couple of times.
    /// </summary>
    [DataContract]
    public class WebLockedUserInformation : WebData
    {
        /// <summary>
        /// User was locked out from ArtDatabankenSOA at this date and time.
        /// </summary>
        [DataMember]
        public DateTime LockedFrom
        { get; set; }

        /// <summary>
        /// User will be locked out from ArtDatabankenSOA until this date and time.
        /// </summary>
        [DataMember]
        public DateTime LockedTo
        { get; set; }

        /// <summary>
        /// Number of recently failed login attempt.
        /// </summary>
        [DataMember]
        public Int64 LoginAttemptCount
        { get; set; }

        /// <summary>
        /// User name for user that is locked out.
        /// User name may be invalid.
        /// </summary>
        [DataMember]
        public String UserName
        { get; set; }
    }
}

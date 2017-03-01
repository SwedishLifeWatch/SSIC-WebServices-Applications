using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handles information about users that are
    /// currently locked out from ArtDatabankenSOA.
    /// Users are locked out if the fail to login a couple of times.
    /// </summary>
    public interface ILockedUserInformation
    {
        /// <summary>
        /// Get data context.
        /// </summary>
        IDataContext DataContext
        { get; }

        /// <summary>
        /// User was locked out from ArtDatabankenSOA at this date and time.
        /// </summary>
        DateTime LockedFrom
        { get; }

        /// <summary>
        /// User will be locked out from ArtDatabankenSOA until this date and time.
        /// </summary>
        DateTime LockedTo
        { get; }

        /// <summary>
        /// Number of recently failed login attempt.
        /// </summary>
        Int64 LoginAttemptCount
        { get; }

        /// <summary>
        /// User name for user that is locked out.
        /// User name may be invalid.
        /// </summary>
        String UserName
        { get; }
    }
}

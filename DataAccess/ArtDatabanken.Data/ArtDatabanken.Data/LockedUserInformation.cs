using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class handles information about users that are
    /// currently locked out from ArtDatabankenSOA.
    /// Users are locked out if the fail to login a couple of times.
    /// </summary>
    public class LockedUserInformation : ILockedUserInformation
    {
        /// <summary>
        /// Create an LockedUserInformation instance.
        /// </summary>
        /// <param name="lockedFrom">Locked out date and time.</param>
        /// <param name="lockedTo">User will be locked out until this date and time.</param>
        /// <param name="loginAttemptCount">Number of recently failed login attempt.</param>
        /// <param name="userName">User name.</param>
        /// <param name="dataContext">Data context.</param>
        public LockedUserInformation(DateTime lockedFrom,
                                     DateTime lockedTo,
                                     Int64 loginAttemptCount,
                                     String userName,
                                     IDataContext dataContext)
        {
            // Check data.
            dataContext.CheckNotNull("dataContext");

            // Set data.
            DataContext = dataContext;
            LockedFrom = lockedFrom;
            LockedTo = lockedTo;
            LoginAttemptCount = loginAttemptCount;
            UserName = userName;
        }

        /// <summary>
        /// Get data context.
        /// </summary>
        public IDataContext DataContext
        { get; private set; }

        /// <summary>
        /// User was locked out from ArtDatabankenSOA at this date and time.
        /// </summary>
        public DateTime LockedFrom
        { get; private set; }

        /// <summary>
        /// User will be locked out from ArtDatabankenSOA until this date and time.
        /// </summary>
        public DateTime LockedTo
        { get; private set; }

        /// <summary>
        /// Number of recently failed login attempt.
        /// </summary>
        public Int64 LoginAttemptCount
        { get; private set; }

        /// <summary>
        /// User name for user that is locked out.
        /// User name may be invalid.
        /// </summary>
        public String UserName
        { get; private set; }
    }
}

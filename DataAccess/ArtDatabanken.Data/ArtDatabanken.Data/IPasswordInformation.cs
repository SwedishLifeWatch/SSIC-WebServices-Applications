using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface defines information that is returned
    /// in response to a ResetPassword method call. 
    /// </summary>
    public interface IPasswordInformation
    {
        /// <summary>
        /// Get data context.
        /// </summary>
        IDataContext DataContext
        { get; }

        /// <summary>
        /// Get email address.
        /// Password was changed for the user
        /// who has this email address.
        /// </summary>
        String EmailAddress
        { get; }

        /// <summary>
        /// Get new password.
        /// </summary>
        String Password
        { get; }

        /// <summary>
        /// Get user name for the user who has the specified email address.
        /// </summary>
        String UserName
        { get; }
    }
}

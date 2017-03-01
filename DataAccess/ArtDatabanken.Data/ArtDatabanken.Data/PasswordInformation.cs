using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information that is returned
    /// in response to a ResetPassword method call. 
    /// </summary>
    public class PasswordInformation : IPasswordInformation
    {
        /// <summary>
        /// Create an PasswordInformation instance.
        /// </summary>
        /// <param name='userName'>
        /// User name for the user who has the
        /// specified email address.
        /// </param>
        /// <param name='emailAddress'>
        /// Password was changed for the user
        /// who has this email address.
        /// </param>
        /// <param name='password'>New password.</param>
        /// <param name='dataContext'>Data context.</param>
        public PasswordInformation(String userName,
                                   String emailAddress,
                                   String password,
                                   IDataContext dataContext)
        {
            // Check data.
            dataContext.CheckNotNull("dataContext");

            // Set data.
            DataContext = dataContext;
            EmailAddress = emailAddress;
            Password = password;
            UserName = userName;
        }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; private set; }

        /// <summary>
        /// Email address.
        /// Password was changed for the user
        /// who has this email address.
        /// </summary>
        public String EmailAddress
        { get; private set; }

        /// <summary>
        /// New password.
        /// </summary>
        public String Password
        { get; private set; }

        /// <summary>
        /// User name for the user who has the specified email address.
        /// </summary>
        public String UserName
        { get; private set; }
    }
}

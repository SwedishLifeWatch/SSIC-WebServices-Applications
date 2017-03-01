using System;
using System.Web.Security;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    public class AccountMembershipService : IMembershipService
    {
        private readonly MembershipProvider _provider;

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        public bool ValidateUser(string userName, string password)
        {            
            if (String.IsNullOrEmpty(userName))
            {
                throw new ArgumentException(Resources.DyntaxaResource.SharedExeptionNullOrEmpty, "userName");
            }

            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentException(Resources.DyntaxaResource.SharedExeptionNullOrEmpty, "password");
            }

            return _provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            if (String.IsNullOrEmpty(userName))
            {
                throw new ArgumentException(Resources.DyntaxaResource.SharedExeptionNullOrEmpty, "userName");
            }

            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentException(Resources.DyntaxaResource.SharedExeptionNullOrEmpty, "password");
            }

            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentException(Resources.DyntaxaResource.SharedExeptionNullOrEmpty, "email");
            }

            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (String.IsNullOrEmpty(userName))
            {
                throw new ArgumentException(Resources.DyntaxaResource.SharedExeptionNullOrEmpty, "userName");
            }

            if (String.IsNullOrEmpty(oldPassword))
            {
                throw new ArgumentException(Resources.DyntaxaResource.SharedExeptionNullOrEmpty, "oldPassword");
            }

            if (String.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentException(Resources.DyntaxaResource.SharedExeptionNullOrEmpty, "newPassword");
            }

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }
    }

    public interface IMembershipService
    {
        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }
}
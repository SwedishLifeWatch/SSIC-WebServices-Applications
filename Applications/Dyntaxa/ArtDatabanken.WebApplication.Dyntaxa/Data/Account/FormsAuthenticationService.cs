using System;
using System.Web.Security;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    public interface IFormsAuthenticationService
    {
        void SignIn(string userName);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName)
        {
            if (String.IsNullOrEmpty(userName))
            {
                throw new ArgumentException(Resources.DyntaxaResource.SharedExeptionNullOrEmpty, "userName");
            }

            FormsAuthentication.SetAuthCookie(userName, false);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}
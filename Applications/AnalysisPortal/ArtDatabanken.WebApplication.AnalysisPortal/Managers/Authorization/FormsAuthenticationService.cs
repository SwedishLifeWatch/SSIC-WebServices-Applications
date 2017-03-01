using System;
using System.Web.Security;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers.Authorization
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
                throw new ArgumentException(Resources.Resource.SharedExeptionNullOrEmpty, "userName");
            }

            FormsAuthentication.SetAuthCookie(userName, false);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}

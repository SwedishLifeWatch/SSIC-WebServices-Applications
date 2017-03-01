using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.Authorization;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Account;
using Resources;

namespace AnalysisPortal.Controllers
{
    /// <summary>
    /// This Controller contains Actions that is used to login and logout a user. 
    /// </summary>
    [HandleError]
    public class AccountController : BaseController
    {
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        private void LoadMySettings(IUserContext userContext)
        {
            try
            {
                if (MySettingsManager.DoesNameExistOnDisk(userContext, MySettingsManager.SettingsName))
                {
                    var mySettings = MySettingsManager.LoadFromDisk(userContext, MySettingsManager.SettingsName);
                    mySettings.EnsureDataProviders(userContext);
                    SessionHandler.MySettings = mySettings;
                    RemoveCookie("MapState");
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Initializes data that might not be available when the constructor is called.
        /// Initializes FormsService and MembershipService
        /// </summary>
        /// <param name="requestContext">The HTTP context and route data.</param>
        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        /// <summary>
        /// Renders the login page
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LogIn(string userName)
        {
            IUserContext userContext = SessionHandler.UserContext;
            if (userContext.IsNotNull())
            {
                return View("LogOut");                
            }

            var model = new LogInModel { UserName = userName };
            ViewData.Model = model;
            return View("LogIn");
        }

        /// <summary>
        /// Tries to login the user.
        /// If the login succeeds the user is redirected to the returnUrl.
        /// If the login fails the Login page will be rendered again.
        /// </summary>
        /// <param name="model">The view model contains the user name and password.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LogIn(LogInModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                IUserContext userContext = CoreData.UserManager.Login(model.UserName, model.Password, AppSettings.Default.ApplicationIdentifier);

                if (userContext.IsNotNull())
                {
                    SessionHandler.UserContext = userContext;
                    //CoreData.TaxonManager.ClearCacheForUserRoles(userContext); // Must clear the service cache so that roles can be reloded.
                    
                    /* globalization with cookie */
                    var cookie = Request.Cookies["CultureInfo"];

                    if (cookie == null)
                    {
                        var locale = CoreData.LocaleManager.GetUsedLocales(GetCurrentUser()).Get(userContext.Locale.ISOCode);
                        SetLanguage(locale.ISOCode);
                    }
                    else
                    {
                        if (cookie.Value != null)
                        {
                            var locale = CoreData.LocaleManager.GetUsedLocales(GetCurrentUser()).Get(cookie.Value);
                            userContext.Locale = locale;
                        }
                    }

                    FormsService.SignIn(model.UserName);
                    if (SessionHandler.Results != null)
                    {
                        SessionHandler.Results.Clear();
                    }

                    LoadMySettings(userContext);

                    //Chenge roles for user if user has more than on role.
                    if (userContext.CurrentRoles.IsNotNull() && userContext.CurrentRoles.Count > 1)
                    {
                        return RedirectToAction("ChangeUserRole", new { url = returnUrl });
                    }
                    //If user only has one role set this role as default
                    if (userContext.CurrentRoles.IsNotNull() && userContext.CurrentRoles.Count == 1)
                    {
                        userContext.CurrentRole = userContext.CurrentRoles[0];
                    }
                    // If no current role is set or if there is no roles the somthing has gone wrong loggin in.
                    else if (userContext.CurrentRole.IsNull() || userContext.CurrentRoles.IsNull())
                    {
                        ModelState.AddModelError("", Resource.AccountControllerIncorrectLoginError);
                    }
                    if (ModelState.IsValid)
                    {
                        if (!String.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect(returnUrl.ToLower());
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resource.AccountControllerIncorrectLoginError);
                }
            }
            // If we got this far, something failed, redisplay form
            ViewData.Model = model;
            return View("LogIn");
        }

        /// <summary>
        /// Logs out the user in session.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        public ActionResult LogOut(string returnUrl)
        {
            IUserContext userContext = SessionHandler.UserContext;
            if (userContext != null)
            {
                if (SessionHandler.MySettings != null)
                {
                    MySettingsManager.SaveLastSettings(userContext, SessionHandler.MySettings);
                    MySettingsManager.SaveToDisk(userContext, MySettingsManager.SettingsName, SessionHandler.MySettings);
                }         

                CoreData.UserManager.Logout(userContext);
                SessionHandler.UserContext = null;
                // When logged out mySettings must be cleared
                SessionHandler.MySettings = new MySettings();
                FormsService.SignOut();
            }
                       
            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Renders an Accesses is not allowed page when a user
        /// tries to render a page which he or she doesn't have permission to.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public ActionResult AccessIsNotAllowed(string url)
        {
            var model = new AccessIsNotAllowedViewModel();
            model.Url = url;
            return View(model);
        }

        /// <summary>
        /// Shows a view on which roles the logged in user have. 
        /// It is also possible to select a new role for this user.
        /// This view is shown when logging in if logged in user has more than one role.
        /// If user has no role seleted current user role will be set to "PrivatePerson" if 
        /// role exist.
        /// </summary>  
        /// <param name="url">Url to view that is shown when choosing to
        /// change role.</param>
        /// <returns>ChangeUserRole view</returns>
        [HttpGet]
        public ActionResult ChangeUserRole(string url)
        {
            IUserContext userContext = GetCurrentUser();
            UserRoleModel model = new UserRoleModel();
            model.UserRoles = new List<UserRoleDropDownModelHelper>();
            model.ReturnUrl = url;
            RoleList roles = userContext.CurrentRoles;
            int index = 0;
            //Create list of roles first assuming that the list is not empty
            foreach (IRole role in roles)
            {
                if (model.UserRoleName != role.Name)
                {
                    model.UserRoles.Add(new UserRoleDropDownModelHelper(role.Id, role.Name, role.Description, index));
                    index++;
                }
            }
            // User has alredy a current role set 
            if (userContext.CurrentRole != null)
            {
                for (int i = 0; i < roles.Count; i++)
                {
                    if (userContext.CurrentRole.Id == roles[i].Id)
                    {
                        model.UserRoleIndex = i;
                        break;
                    }
                }
                model.UserRoleName = userContext.CurrentRole.Name;
                model.UserRoleDescription = userContext.CurrentRole.Description;
                model.UserRoleId = userContext.CurrentRole.Id;
            }
            else 
            {
                // Set private person as current role
                bool privateRoleFound = false;
                foreach (IRole tempRole in roles)
                {
                    if (tempRole.Id == AppSettings.Default.PrivatePersonRoleID)
                    {
                        for (int i = 0; i < model.UserRoles.Count; i++)
                        {
                            UserRoleDropDownModelHelper modelRole = model.UserRoles[i];
                            if (modelRole.Id == tempRole.Id)
                            {
                                model.UserRoleIndex = i;
                            }
                        }
                        userContext.CurrentRole = tempRole;
                        model.UserRoleDescription = tempRole.Description;
                        model.UserRoleName = tempRole.Name;
                        model.UserRoleId = tempRole.Id;
                        privateRoleFound = true;
                        break;
                    }
                }
                //No private user found set firsts user as current role
                if (!privateRoleFound)
                {
                    model.UserRoleIndex = 0;
                    model.UserRoleDescription = userContext.CurrentRoles[0].Description;
                    model.UserRoleName = userContext.CurrentRoles[0].Name;
                    model.UserRoleId = userContext.CurrentRoles[0].Id;
                    userContext.CurrentRole = userContext.CurrentRoles[0];
                }
            }
           
            ViewData.Model = model;
            return View("ChangeUserRole");
        }

        /// <summary>
        /// Change current user role to a new role identeified by id.
        /// (Post)
        /// </summary>
        /// <param name="newRoleId">Selected new role</param>
        /// <param name="model">UserRoleModel</param>
        /// <returns>Calling view,home view or if sonthing went wron Change user role view</returns>
        [HttpPost]
        public ActionResult ChangeUserRole(string newRoleId, UserRoleModel model)
        {
            if (newRoleId.IsNotNull())
            {
                // Set user role and return to if exist model.url
                if (SetUserRole(GetCurrentUser(), Convert.ToInt32(newRoleId)))
                {
                    if (SessionHandler.Results != null)
                    {
                        SessionHandler.Results.Clear();
                    }

                    if (!String.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl.ToLower());
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            // If we get here we have to reload view again...
            model.UserRoles = new List<UserRoleDropDownModelHelper>();
            int index = 0;
            foreach (IRole role in GetCurrentUser().CurrentRoles)
            {    
                model.UserRoles.Add(new UserRoleDropDownModelHelper(role.Id, role.Name, role.Description, index));
                index++;
            }
            //Set model and return view
            ViewData.Model = model;
            return View("ChangeUserRole");
        }

#region Helper methods

        /// <summary>
        /// Seta a new role as current
        /// </summary>
        /// <param name="user">User context for lodded in user</param>
        /// <param name="id">Id of new role to be set as current</param>
        /// <returns></returns>
        private static bool SetUserRole(IUserContext user, int id)
        {
            if (user.CurrentRoles.Count < 1)
            {
                return false;
            }

            foreach (IRole role in user.CurrentRoles)
            {
                if (role.Id == id)
                {
                    user.CurrentRole = role;
                    return true;
                }   
            }
            return false;
        }

#endregion

#if DEBUG

        /// <summary>
        /// Logs in our testuser. Only used in Debug.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>        
        /// <returns></returns>
        public RedirectResult AutoLogIn(string returnUrl)
        {
            IUserContext userContext = CoreData.UserManager.Login("testuser", "Qwertyasdfg123", AppSettings.Default.ApplicationIdentifier);

            if (userContext.IsNotNull())
            {
                SessionHandler.UserContext = userContext;                

                // Must clear the service cash so that roles can be reloded.
                //CoreData.TaxonManager.ClearCacheForUserRoles(userContext);

                /* globalization with cookie */
                HttpCookie cookie = new HttpCookie("CultureInfo");
                cookie.Value = userContext.Locale.ISOCode;
                cookie.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(cookie);

                FormsService.SignIn("testuser");
                //Check roles for user 
                if (userContext.CurrentRoles.IsNotNull() && userContext.CurrentRoles.Count > 0)
                {
                    userContext.CurrentRole = userContext.CurrentRoles[0];
                }

                LoadMySettings(userContext);
            }
            else
            {
                ModelState.AddModelError("", Resource.AccountControllerIncorrectLoginError);
            }
            
            if (String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(Url.Action("Index", "Home"));
            }

            return Redirect(returnUrl);
        }

        public RedirectResult SetUserInSession(string returnUrl)
        {
            IUserContext userContext = new UserContext();           
            DataSourceInformation dataSource;
            DataContext dataContext;
            Locale locale;
             
            dataSource = new DataSourceInformation(
                "UserService",
                "https://moneses-dev.artdata.slu.se/UserService/UserService.svc/Fast",
                DataSourceType.WebService);

            locale = new Locale(
                175,
                "sv-SE",
                "Swedish (Sweden)",
                "svenska (Sverige)",
                new DataContext(dataSource, null));

            userContext.Locale = locale;

            dataContext = new DataContext(dataSource, locale);
            userContext.User = new User(userContext);            
            userContext.User.IsAccountActivated = true;
            userContext.User.Id = 9710;
            userContext.User.PersonId = 12165;
            userContext.User.GUID = "urn:lsid:artdata.slu.se:User:9710:2";
            userContext.User.UserName = "calluna";

            userContext.User.DataContext = dataContext;
            userContext.CurrentRoles = new RoleList();

            Role role = new Role(userContext);
            role.GUID = "urn:lsid:artdata.slu.se:User:9710:2";
            role.Id = 2; //?
            role.Authorities = new AuthorityList();
            Authority authority = new Authority(userContext);
            authority.RoleId = 2; //?
            authority.Id = 3408;
            authority.Identifier = "Sighting";
            authority.Description = "Ger rättigheter att se publika observationer arter i naturen.";
            authority.GUID = "urn:lsid:artdata.slu.se:Authority:3408:3";
            authority.DataContext = dataContext;
            role.Authorities.Add(authority);
            role.DataContext = dataContext;
            userContext.CurrentRoles.Add(role);
            userContext.CurrentRole = userContext.CurrentRoles[0];

            userContext.Properties.Add("WebServiceClientToken:AnalysisService", "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAANMFHNLJMABLGPLEFIIGJEHCCCGDCCMDHAEAAAAAAACAAAAAAAAAAADGGAAAAKIAAAAAABAAAAAAAABLAJDMLMLANODBONBDJDIFGKPOGOCONAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADPNOALDAGGBLFAEMLKMGODLCCIGDONBMEAAAAAAANKMHBAGGLJDCMJGHBBADAENEABHJKEOEMNNDCELLNPGMJKLADIJEFIMNPCOBKLJDGPNEBFDPKBOJHILGECLEPAGOEKIGHAMMNMBHENBBPCIHKGNNBICGDOOCOEJDGDBCBEAAAAAALCFLNKBOHHIBMIJIBAGKFKEBPHOFPDIDECAEJIDA");
            userContext.Properties.Add("WebServiceClientToken:TaxonService", "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAANMFHNLJMABLGPLEFIIGJEHCCCGDCCMDHAEAAAAAAACAAAAAAAAAAADGGAAAAKIAAAAAABAAAAAAABNFMLFLLINODFMCBHLEJNNEHJJFEAEIIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPKAJNCCNBAFFGCCPPDOFJEEGJALBJOHBDIAAAAAAKLMNCLOLEDLEOOAOPMFOAIKMGBKDKDBFIPHAGGIEIJDNIKIDEJPIKDDJBGDDMHBGHANOBLFPLHGMLBAOLDLBDJNADNDFLLPDDKJFDLPOHJKHMEIPBEAAAAAAILLEPNJDDCNFHBHBKBJBFHGPBDHFKNBGFHNCLIAN");
            userContext.Properties.Add("WebServiceClientToken:SwedishSpeciesObservationSOAPService", "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAANMFHNLJMABLGPLEFIIGJEHCCCGDCCMDHAEAAAAAAACAAAAAAAAAAADGGAAAAKIAAAAAABAAAAAAAJIJEKMLGAMNFOFENAMIKFENEBAMJBFBJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAMCLBCJMCIHOGECAINIIBAKHLNGFJBIDHFAAAAAAADHHJGOJECDGLFBAEFDDLFDKMNFEMJLPFHMGCEGNFMNJALGCLDPBKNKIHFBBKFOKPNFDAKMHPPEABNMMHHNLDBDLKCEPJGIBBAIJLJFCDKEKKHCINFMJHJDOIOOLPPJJNDPEFEHDFNGNGNIBHDJIGAGJLPLLACPEBBEAAAAAAEIJCHDEGJFIHCADCNBCDNCOOBLICMDOEOHLCJGMH");
            userContext.Properties.Add("WebServiceClientToken:UserService", "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAANMFHNLJMABLGPLEFIIGJEHCCCGDCCMDHAEAAAAAAACAAAAAAAAAAADGGAAAAKIAAAAAABAAAAAAAJNLBEPHKAEIABPELPMMKECDNMAHKOPPLAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKCFCHEKOMBAGJHGOPLFLMLAKHFJFHCALDIAAAAAAHPILMCMMADNNPGEIFOPMAPNOBNANAJLEBBHJBCEPEOGPFJHCOKHNBNLHAEJIHOJHBIGBPICONOGNPGCHIBFLOFKMNOODOONPMIINBFDJJOOKFPLKBEAAAAAAOBHLFNOJNIPGNFDDCFDLLGBANHHGOIOAIABKOOJC");
            userContext.Properties.Add("WebServiceClientToken:GeoReferenceService", "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAANMFHNLJMABLGPLEFIIGJEHCCCGDCCMDHAEAAAAAAACAAAAAAAAAAADGGAAAAKIAAAAAABAAAAAAAMDCFKKNLGMECCCGFIGDGAMOOJOONJOLMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABKFHLHNALJFKLJPLGOBLKMNEBNLEFEOAEAAAAAAAJNKNGGPDEDGMEJFGBGJPFPMGJLOEPKALNNILJEBKLDOGLKLDLJDFJFBBEHMLHEBNAKADEGEBHNMHMLHBONANMILANEJGDJDOAKCMLDEJANNMDBALBBOKPNGOBDAINIILBEAAAAAAMFONCMJDPOAPCKANOOJECFIFEOOLCFNHAIPFNOIL");

            SessionHandler.UserContext = userContext;

            if (String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(Url.Action("Index", "Home"));
            }

            return Redirect(returnUrl);
        }

#endif
    }    
}

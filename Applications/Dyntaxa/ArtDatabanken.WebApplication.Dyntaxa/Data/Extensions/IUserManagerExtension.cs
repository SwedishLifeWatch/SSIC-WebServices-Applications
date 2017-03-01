using System;
using System.Web;
using ArtDatabanken.Security;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// Contains extension methods to IUserManager.
    /// </summary>
    public static class IUserManagerExtension
    {
        private static readonly object ThisLock = new object();

        /// <summary>
        /// Get application user context.
        /// </summary>
        /// <param name="userManager">A user manager instance.</param>
        /// <returns>Application user context.</returns>       
        public static IUserContext GetApplicationContext(this IUserManager userManager)
        {
            string localeIsoCode;
            if (HttpContextSessionHelper.IsInTestMode)
            {
                return HttpContextSessionHelper.TestHelper.GetFromSession<IUserContext>("applicationUserContext");
            }

            if (HttpContext.Current != null)
            {
                localeIsoCode = (String)HttpContext.Current.Session["language"];
            }
            else
            {
                localeIsoCode = null;
            }

            return GetApplicationContext(userManager, localeIsoCode);
        }

        /// <summary>
        /// Get application user context.
        /// </summary>
        /// <param name="userManager">A user manager instance.</param>
        /// <param name="localeIsoCode">The locale ISO code.</param>
        /// <returns>Application user context.</returns>       
        public static IUserContext GetApplicationContext(this IUserManager userManager, string localeIsoCode)
        {
            IUserContext applicationUserContext;
            String cacheKey;

            if (HttpContextSessionHelper.IsInTestMode)
            {
                applicationUserContext = HttpContextSessionHelper.TestHelper.GetFromSession<IUserContext>("applicationUserContext");
                return applicationUserContext;
            }
            
            cacheKey = GetApplicationContextCacheKey(localeIsoCode);
            applicationUserContext = (IUserContext)HttpRuntime.Cache.Get(cacheKey);
            if (applicationUserContext == null)
            {
                lock (ThisLock)
                {
                    applicationUserContext = (IUserContext)HttpRuntime.Cache.Get(cacheKey);
                    if (applicationUserContext == null)
                    { 
                        CoreData.UserManager.LoginApplicationUser();
                        applicationUserContext = (IUserContext)HttpRuntime.Cache.Get(cacheKey);
                        DyntaxaLogger.WriteException(new Exception(string.Format("GetApplicationContext() didn't find application user. Tried to login. Successfull: {0}", (applicationUserContext != null).ToString())));
                    }
                }
            }
            return applicationUserContext;
        }

        /// <summary>
        /// Get application transaction user context.
        /// </summary>
        /// <param name="userManager">A user manager instance.</param>
        /// <returns>Application transaction user context.</returns>       
        public static IUserContext GetApplicationTransactionContext(this IUserManager userManager)
        {
            string localeIsoCode;
            if (HttpContextSessionHelper.IsInTestMode)
            {
                return HttpContextSessionHelper.TestHelper.GetFromSession<IUserContext>("applicationTransactionUserContext");
            }

            if (HttpContext.Current != null)
            {
                localeIsoCode = (String)HttpContext.Current.Session["language"];
            }
            else
            {
                localeIsoCode = null;
            }

            return GetApplicationTransactionContext(userManager, localeIsoCode);
        }

        /// <summary>
        /// Get application transaction user context.
        /// </summary>
        /// <param name="userManager">A user manager instance.</param>
        /// <param name="localeIsoCode">The locale ISO code.</param>
        /// <returns>Application transaction user context.</returns>       
        public static IUserContext GetApplicationTransactionContext(this IUserManager userManager, string localeIsoCode)
        {
            IUserContext applicationTransactionUserContext;
            String cacheKey;

            if (HttpContextSessionHelper.IsInTestMode)
            {
                applicationTransactionUserContext = HttpContextSessionHelper.TestHelper.GetFromSession<IUserContext>("applicationTransactionUserContext");
                return applicationTransactionUserContext;
            }

            cacheKey = GetApplicationTransactionContextCacheKey(localeIsoCode);
            applicationTransactionUserContext = (IUserContext)HttpRuntime.Cache.Get(cacheKey);
            if (applicationTransactionUserContext == null)
            {
                lock (ThisLock)
                {
                    applicationTransactionUserContext = (IUserContext)HttpRuntime.Cache.Get(cacheKey);
                    if (applicationTransactionUserContext == null)
                    {                                                
                        CoreData.UserManager.LoginApplicationTransactionUser();
                        applicationTransactionUserContext = (IUserContext)HttpRuntime.Cache.Get(cacheKey);
                        DyntaxaLogger.WriteException(new Exception(string.Format("GetApplicationTransactionContext() didn't find application user. Tried to login. Successfull: {0}", (applicationTransactionUserContext != null).ToString())));
                    }
                }
            }

            return applicationTransactionUserContext;
        }

        public static IUserContext GetLoggedInApplicationUserContext(this IUserManager userManager, string localeIsoCode)
        {
            var cacheKey = GetApplicationContextCacheKey(localeIsoCode);
            var applicationUserContext = (IUserContext)HttpRuntime.Cache.Get(cacheKey);            
            return applicationUserContext;
        }

        /// <summary>
        /// Get cache key for application user context.
        /// </summary>
        /// <param name="localeIsoCode">ISO code for locale used in application user context.</param>
        /// <returns>Cache key for application user context.</returns>       
        private static String GetApplicationContextCacheKey(String localeIsoCode)
        {
            return Resources.DyntaxaSettings.Default.ApplicationContextCacheKey + localeIsoCode;
        }

        /// <summary>
        /// Get cache key for application transaction user context.
        /// </summary>
        /// <param name="localeIsoCode">ISO code for locale used in application transaction user context.</param>
        /// <returns>Cache key for application transaction user context.</returns>       
        private static String GetApplicationTransactionContextCacheKey(String localeIsoCode)
        {
            return Resources.DyntaxaSettings.Default.ApplicationTransactionContextCacheKey + localeIsoCode;
        }

        /// <summary>
        /// Get password for application user.
        /// The password should be kept as short time as possible.
        /// </summary>
        /// <param name="userManager">A user manager instance.</param>
        /// <returns>Password for application user.</returns>       
        public static String GetApplicationPassword(this IUserManager userManager)
        {
            CipherString cipherString;
            String encryptedString;

            // Get password.
            switch (Environment.MachineName)
            {
                case "ARTAP-ST": // Team Artportalen test server.
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAFHDLHFNBGIGDDBEFLAHNJDBGKBEEBFIGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAGJMHICGOPDFLGMOGDBOINBEJJGIFHFBCAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPMOHIDEILECNDHAHNNJLGNAFBBBGIMAOBAAAAAAAEGEADMNLLOFODFDECADMCIPOELOGBIKEBEAAAAAAGOCAPMNHKEBBIDIDBOKGNLMGJFPBKFKLOHLKBKNJ";
                    break;

                case "ARTFAKTA-DEV": // Team Species Fact test server.
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHKDEHDFCMCOCHOJEJFNDPBEDNMCFHBLCAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAMIDJNGPONKFBEJILNFLJDAMGBHEGPMJBBAAAAAAADHEDFNNALIPKNBKEHFFNMDCECELKENAJBEAAAAAAFLDIMGEGJGKOINKMPGLPONNKKGPGMDPHDOKAHIME";
                    break;

                case "SLU005060": // Mattias new
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAKHAHAGODPFACCPEBJFANOOFFGFKAAHHNAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAEOPIIMPLGFHAAMJOLCNEDCHGCNBJPBJJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFEBHHEGALNNEEOOCIDPKPDNEBAOOJAFDBAAAAAAAIGHDIBFEANLALKKKFLAKPKKDDFOJBAKCBEAAAAAACJPLHCAPIGINKGCLACBBCLNDMNLDEKEKKDDLJPHA";
                    break;

                case "MONESES-DEV":
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEAFPECBBEFEHIKEGLFFDMHECEONJDGANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHICBOPKGAMFBLABJNCPNJOAMNABHBFMEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAIAIGPINKENKAFEJEAOGEFKNJHOICPDPFBAAAAAAAFKFOCOHFGCBBFLHBIAKBPPFKIIINJJLPBEAAAAAAKMPKKGPJMAKIDKGEFNIJPDDNEKNNDAAEALPALJGM";
                    break;

                case "MONESES-ST": // System test server.
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAOBJEGFODPEFCHOMNLJJLLBLLHAHMDBOEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAEIHDPMACBIPKGLKOIIMNPDMJDGJKCHMPBAAAAAAADLOHBBFNFBOAIANANOEEHJFKNGOGKOCBBEAAAAAAPJOFPGFOADJGAPEONNLEIAJEKOBHNNOMIJKGINFB";
                    break;

                case "LAMPETRA2-1":
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEFNCNMALBJAAPDEFJAELMCDBLMLKOIAPAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJPNKOIJLHCEOLKLKFPABFMJBBFNJOFDJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALBCCLIMNJIIPGCBIECOENGMMKFHBGILNBAAAAAAALEHLPAKLKMMPPCPECMOJICNHENJDDABCBEAAAAAAKMBBHALHLFEJKJBLJBLIPKEEEJIJPMJDFDCBBLAM";
                    break;

                case "LAMPETRA2-2":
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAACNLJDFFELFNJJEEDLDFBHKHAEHCKNCDOAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJDNIFHHPEFIOHAHBEGIHEDFCGJIFHKDPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFINLBKKMGBPCANMMAFHMOGKEDEBEBMEIBAAAAAAABHPMBBDAKGNMGDPCEPGEEJOEGFBJJMNPBEAAAAAAJAJNNDJMBALFMPDICLBIJIJDIGKABPHNIKMGAMGH";
                    break;

                case "ARTDATA-OK196":
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAFGACFGAPDIFBMEENIHHJOIOKEBOBCMDDAEAAAAAAACAAAAAAAAAAADGGAAAAKIAAAAAABAAAAAAAHLLPKHKPAGCEDBAEBCIBMGLFJNKKDGJCAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAOHBCJOMABOEGGNAEJNJMJDMLEEEMDEFIBAAAAAAAKHFHJKHCCBKOMBBGPADBLOFOOGPEMGOEBEAAAAAAAMAEMMBPFOPDAEAHFJBCDNMIFPCFADPPICAEDEIM";
                    break;

                case "SLU010940": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEBNLCONJBEGNMHEHILNDFELGEJDEOEDLAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADPJHJOBKDLIDHPELPHEEAOFHFGBBODJDAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALABPBHKBFEHKPKIKJKBAJMNGJCGNGGBKBAAAAAAANDLBNDOBOFAGEOAKDCALDODDLDLKDDPOBEAAAAAAKLJJMCHJGDMIAKEGJMMGAKACNHFMEDAHNHOOIEEL";
                    break;

                case "SLU003354": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGLGBDIAIDBHFHEEKIOMMGMEHJFINCANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFNPHHGLLNLCLFHAEALKADHKKOOEENKJPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJPCDPEPIBBPIPLILEONEODNOHGCBKGLPBAAAAAAAKEDOEOIKMPJPOFJLJPPIIBKPAKJAOIHEBEAAAAAAKKGKLHAMMBGKOAFHBOAGOCCJHBEDHFBOHBCKIKCA";
                    break;

                case "SLU011730": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGKOONNFFCICKGAENLBBKPOHHMCHDGMEDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHOPJFOABEOBJDKEKFFLLPBKCCMPLPIMOAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAAPNAPOPDJJBPPOKJPOIKHEHNDEFEBPLCBAAAAAAAPCCDMJDBAJJBFBLJPFLDEJGBGFMJMINABEAAAAAAKOCEMGMGKBGLFJFBKBJAHFBMNBMMKMBDJHFFMFEJ";
                    break;

                case "SLU002760": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAKBCABGKFKMIFHKEFJMBEJDNDNAEMFGAFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAABMCBINJDCGOFBMJJGHNDLCHBFGFNGGMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKHGOKBOMABAEIMGPHNCCLCEKAEECLENABAAAAAAAKLNPBIGMLAEJKBLDDCCEHIENCIAAJHKMBEAAAAAADADPMMFAEHILIPIEENNDEGFPHENJHFKDANKBJGBM";
                    break;

                case "SLU011837": //  new
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPFOBOEOIJOPHGCEFJGCCBMFMHLOBJJLJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALPHOOOIFFOAMGLAKMGHGBFALKDOALJLKAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJIHILBGBJEFOJJJGMNFMLNNDDPBKAFNJBAAAAAAAFKJJEGBJGNKMEJFGKGHKICAIPDJLHEMMBEAAAAAAJOBFKABIPDLJOHEOMJGJCCBPFPHKDNPJHKJBPMGI";
                    break;

                case "SLU002759": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAHJPABDBOIBDFPDEIJCANFPFNLFEHMPODAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFAJOGNKLECOBJHHGGJNDACOPHPNKDHENAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJKEJGCMLIDBNMEAHBEEKKNCGMNEOBGCHBAAAAAAAKHKHGABACPHCIIAHFOOCHMJCDKBKLJHGBEAAAAAADPBGDCMHNFNNHEOPMCKCJIMGGFNBKHMHMLPJOINK";
                    break;

                case "TFSBUILD": // TFS Build machine
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAOAOOJDBKGNLDGAEKKDJEJDACOGFHLBDIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALPMIDAMGMJHOAEFJODGCHBPBAHHBGLAGAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFBHMFPBDGDNFBFNOCGHCFPGOJAIGHHIABAAAAAAAJNPDGKCFMMHAJGEMCADJOICHFHOEDBHABEAAAAAAOOONNDHNIAFPHCJDJFLJMHBPEEKLJLJOKOODFOIO";
                    break;

                case "SLU011895": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGOMDGCCAOKJOJMECJEKHFDIAIPDOIEMOAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHCFCCLEJDGFDBDLCJEGELGNLJEDDGMBLAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADACJLFNMKNMMMHEMDFHMILIPIKPIOEEHBAAAAAAAEEHELBJDMPKAKOHKBCMHMBLAIPGLJNFBBEAAAAAADKMBANOCKPFFJEOFLCCKEEFFLPHLLFAHCJBJOONI";
                    break;

                case "SLU012268": // Sohrab Pakyari
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAHBLEAIKLMODKNNELLDLMAOFBANAIEILIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACENDDOHEGDIMEDFDKBNNCPFIEODICHABAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAACFNMLPBGMEPEBLDFNMKFKFMMLNCOKEHNBAAAAAAAFBHPKKGGDALPNLCODNGGJBLIMJHPAGCHBEAAAAAAJOJLDGFHKGJPABPHFHHONDPADBJMJKEMAPJBNHHM";
                    break;

                case "SLU011360": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGDOKPMCLDJMGKCELIKOLCEDPLANJLBFEAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFCBNENPCFCLNJNPBJMBKKMKJKIBOCIGAAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAADBFFBMFPPKFMAPDOCMIKGNDAMMCIOAPBAAAAAAAONMOGJNKFNABPPFMEINLELDNJHPLFCDBBEAAAAAAOOJCGLLFBFBEDDPPNJEMEKOBJJNPFEEJKJANLGAI";
                    break;

                case "SLW-DEV": // Team Two Blueberries test server.
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAEDDADIHAJLEKBLGGJHEPJPDIGCKHIFHNAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADIILPPALBLLONPAEJLLKKIBEHLHDMIBKBAAAAAAAKJLDGNACNDOKPHAHHLIFFKOFNAHFCODOBEAAAAAAFKODEHHJAAMPANPIGMNGKDBLOOBPGHKHJELAKFKJ";
                    break;

                case "SLU010576": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAHGELMJJAOFKNIHEFJIJBEMPKIAGIHEIDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAGMOLFCKLKNHLEHAKLABKBPJFCBPEIHMFAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALBLGFAHKFCJCNHLCKHJMPNKFHKCEIENKBAAAAAAAHEOEOENABAEEHHIPMFDNMPGJNDAGMLJJBEAAAAAAHPHAJMJGGEINJOAJCDMAAMMCMLLEJFPMEGLCBELM";
                    break;

                case "ARTTAXA2-1": // Server för dyntaxa
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAELCHCBHCEBIIBFENLNEFBJCFECBJOLMBAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAGMDEPPIMINBDEBDIFHNKMNJHEGNCDINEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAGLKAPDGDACNMICNJJKIFLECFLHIFCKNBBAAAAAAAFNBGOFMALDBPLIBFOCOAJJIKLGCPFDCEBEAAAAAADGPODNFIAKNPCMHFAKEHFGHEAKLNBJPFCFOJMAGH";
                    break;
                case "ARTTAXA2-2": // Server för dyntaxa
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAADEMKPDMHHMPHCLEPLMDNDNNNEAGMIKFAAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFPBMFOBOMOPKFEBGIPCKPPJFLIGJIACBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAANLBNCDEMPNFDOPNEANIFJFLCFGAOHJAJBAAAAAAAGLLDGPMOOCFFMFHJHEBDCLEKLHEDLKNFBEAAAAAAJNHIMDEBNMAIPKNPAMPAHAGIEBOFMDCKEKDMDFEL";
                    break;

                case "SLU012925": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIPPPDJLGOBFNIKEAIDCFOGJFLFHBHOLNAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAAKKDPCAGPILIEJIDGAPIOOOKFBKDNIHNAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHOKEONFLPNJOJEAACLDGKIPJPOPHJKPDBAAAAAAACHPFNBIKMKMANMOOOONKDHOKFKFKLOEDBEAAAAAABIEMDAALINKKGEJABKMHOLPNIDKLNFCOMIFMOPEA";
                    break;

                case "SLU011161": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALEGNKAPEPFIKOLECLGFNLNJMBMJICEFLAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAEDIHDLIAOELBGNOHEBDPHONKCMBBKHPBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALDKGAMEMJBOGCBBNDFICKAMGBONMBKBMBAAAAAAAEBIILBOKLNCNOBKFCDNPFIJPFNFCOMOPBEAAAAAANGDJHNIEBNDBMGDKOFEEOPPFNIDBKGLPCOPFDLIG";
                    break;

                default:
                    throw new ApplicationException(Environment.MachineName + " - you can't run Dyntaxa from this machine, contact admin");
            }

            // Login application user.
            cipherString = new CipherString();
            return cipherString.DecryptText(encryptedString);
        }

        /// <summary>
        /// Get user name for application user.
        /// </summary>
        /// <param name="userManager">A user manager instance.</param>
        /// <returns>User name for application user.</returns>       
        public static String GetApplicationUserName(this IUserManager userManager)
        {
            return Resources.DyntaxaSettings.Default.DyntaxaApplicationUserName;
        }

        /// <summary>
        /// Get user context for current user.
        /// It can be either the application's user or the logged in user.
        /// </summary>
        /// <param name="userManager">A user manager instance.</param>
        /// <returns>User context for current user.</returns>       
        public static IUserContext GetCurrentUser(this IUserManager userManager)
        {
            IUserContext userContext;

            if (HttpContextSessionHelper.IsInTestMode)
            {
                userContext = HttpContextSessionHelper.TestHelper.GetFromSession<IUserContext>("userContext");
                if (userContext.IsNotNull())
                {
                    return userContext;
                }
                else
                {
                    return GetApplicationContext(userManager);
                }
            }

            userContext = HttpContext.Current.Session["userContext"] as IUserContext;
            if (userContext.IsNotNull())
            {
                return userContext;
            }

            return GetApplicationContext(userManager);            
        }

        /// <summary>
        /// Get user context for the logged in user.
        /// </summary>
        /// <param name="userManager">A user manager instance.</param>
        /// <returns>User context for the logged in user.</returns>       
        public static IUserContext GetUserContext(this IUserManager userManager)
        {
            if (HttpContextSessionHelper.IsInTestMode)
            {
                return HttpContextSessionHelper.TestHelper.GetFromSession<IUserContext>("userContext");
            }
            return HttpContext.Current.Session["userContext"] as IUserContext;
        }

        /// <summary>
        /// Login application user.
        /// </summary>
        /// <param name="userManager">A user manager instance.</param>
        public static void LoginApplicationUser(this IUserManager userManager)
        {
            IUserContext applicationUserContext, tempApplicationUserContext;
            String cacheKey;

            // Login application user.
            applicationUserContext = CoreData.UserManager.Login(
                userManager.GetApplicationUserName(),
                userManager.GetApplicationPassword(),
                ApplicationIdentifier.Dyntaxa.ToString());
            if (applicationUserContext.IsNull())
            {
                throw new ApplicationException("Login to UserService failed.");
            }

            // Cache application user context for all used locales.
            cacheKey = GetApplicationContextCacheKey(null);
            HttpRuntime.Cache[cacheKey] = applicationUserContext;
            foreach (ILocale locale in CoreData.LocaleManager.GetUsedLocales(applicationUserContext))
            {
                cacheKey = GetApplicationContextCacheKey(locale.ISOCode);
                tempApplicationUserContext = applicationUserContext.Clone();
                tempApplicationUserContext.Locale = locale;
                HttpRuntime.Cache[cacheKey] = tempApplicationUserContext;
            }
        }

        /// <summary>
        /// Login application transaction user that should be used when making transactions.
        /// </summary>
        /// <param name="userManager">A user manager instance.</param>
        public static void LoginApplicationTransactionUser(this IUserManager userManager)
        {
            IUserContext applicationUserContext, tempApplicationUserContext;
            String cacheKey;

            // Login application transaction user.
            applicationUserContext = CoreData.UserManager.Login(
                userManager.GetApplicationUserName(),
                userManager.GetApplicationPassword(),
                ApplicationIdentifier.Dyntaxa.ToString());
            if (applicationUserContext.IsNull())
            {
                throw new ApplicationException("Login to UserService failed.");
            }

            // Cache application transaction user context for all used locales.
            cacheKey = GetApplicationTransactionContextCacheKey(null);
            HttpRuntime.Cache[cacheKey] = applicationUserContext;
            foreach (ILocale locale in CoreData.LocaleManager.GetUsedLocales(applicationUserContext))
            {
                cacheKey = GetApplicationTransactionContextCacheKey(locale.ISOCode);
                tempApplicationUserContext = applicationUserContext.Clone();
                tempApplicationUserContext.Locale = locale;
                HttpRuntime.Cache[cacheKey] = tempApplicationUserContext;
            }
        }

        public static void SetUserRole(this IUserManager userManager, int id)
        {
            IUserContext userContext = HttpContext.Current.Session["userContext"] as IUserContext;
           
            if (userContext.IsNull())
            {
                return; // todo throw exception
                //throw new Exception("User is not logged in");
            }

            userContext.CurrentRole = userContext.CurrentRoles[id];            
        }

        public static IUserContext UpdateUserRoles(this IUserManager userManager, int id)
        {
            IUserContext userContext;
            if (HttpContextSessionHelper.IsInTestMode)
            {
                userContext = HttpContextSessionHelper.TestHelper.GetFromSession<IUserContext>("userContext");
            }
            else
            {
                userContext = HttpContext.Current.Session["userContext"] as IUserContext;
            }

            if (userContext.IsNotNull())
            {
               userContext.CurrentRoles = CoreData.UserManager.GetRolesByUser(userContext, id, Resources.DyntaxaSettings.Default.DyntaxaApplicationIdentifier);
               if (HttpContextSessionHelper.IsInTestMode)
               {
                   HttpContextSessionHelper.TestHelper.SetInSession("userContext", userContext);
               }
               else
               {
                   HttpContext.Current.Session["userContext"] = userContext;
               }                
            }

            return userContext;
        }
    }
}

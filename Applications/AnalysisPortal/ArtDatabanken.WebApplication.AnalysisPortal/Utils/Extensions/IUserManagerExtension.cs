using System;
using ArtDatabanken.Data;
using ArtDatabanken.Security;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Logging;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions
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
            IUserContext applicationUserContext;
            String cacheKey, localeIsoCode;           
            localeIsoCode = SessionHandler.Language;
            cacheKey = GetApplicationContextCacheKey(localeIsoCode);
            applicationUserContext = CacheHandler.GetApplicationUserContext(cacheKey);
            if (applicationUserContext == null)
            {
                lock (ThisLock)
                {
                    applicationUserContext = CacheHandler.GetApplicationUserContext(cacheKey);
                    if (applicationUserContext == null)
                    {
                        CoreData.UserManager.LoginApplicationUser();
                        applicationUserContext = CacheHandler.GetApplicationUserContext(cacheKey);
                        Logger.WriteException(new Exception(string.Format("GetApplicationContext() didn't find application user. Tried to login. Successfull: {0}", (applicationUserContext != null).ToString())));
                    }
                }
            }
            return applicationUserContext;
        }

        /// <summary>
        /// Get cache key for application user context.
        /// </summary>
        /// <param name="localeIsoCode">ISO code for locale used in application user context.</param>
        /// <returns>Cache key for application user context.</returns>       
        private static String GetApplicationContextCacheKey(String localeIsoCode)
        {
            return Resources.AppSettings.Default.ApplicationContextCacheKey + localeIsoCode;
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
                case "SLU004911": //  - TODO: delete (crasched computer)? 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAFJDJHEBAOCEPGDEIJKDHGJNLAFFCMLPPAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAADAIAHGNCHKMGCHEKCKNHMHFHCDNOPLEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAOCALJBGLBINJFIBPOAFJPHDLJFHAAOAFBAAAAAAADBHANLGLJEJFJIPOMDOANKAOBLALOAFCBEAAAAAAKHBBECNABAKNKGLDEJFDACOIPKJMILHODJBIANCC";
                    break;
                    
                case "SLU005060": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAKHAHAGODPFACCPEBJFANOOFFGFKAAHHNAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAEAACLEFNGGFICJEBGHFGKJKDIHFFPJIKAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFIMKGJAFIIHDPIPANFHOOAODKHJEJGNBBAAAAAAAJOGMEOJGIELEGPPPGLMACAHEALOKILFPBEAAAAAAIOIKBFPHFPJMIHJOPPBKDBAGAJCBEJIDMEECLJGK";
                    break;                    

                case "MONESES-DEV":
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEAFPECBBEFEHIKEGLFFDMHECEONJDGANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJIBDHABCHKEJFPDCPKCMOKNGMKLEPKAIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAENHPPJHJFHCAMHLONBNHBPDDHCCDNMBKBAAAAAAAPHGPJDJDAJDKJFKFBCBECNIOCMEHBHHNBEAAAAAAPAFKEIKHABECCPKGPEKEALJLMBEKIEAGONBMHOHA";
                    break;

                case "SLW-DEV": // Team Two Blueberries test server.
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAFIKLKKOPLDFOGIEBKMPEHDIPDHJCPNNBAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAEFOJOFDIHFACCLIGFNGAJMBKNECJLGPJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABBFEOIGGPFLMHEPNBELGKJOLGOFJAHFEBAAAAAAAGJBPKMLNKLLOECGAJKDEKGILLPPFPDMFBEAAAAAAICKHKFEFNFHGDFFEDIDEGLHFIFKALFNNJKICPBAC";
                    break;

                case "SILURUS2-1":
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABKFCOBBPIBEMBHEOJGJDNNOHKDPHDPJKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAKKFLACGFMOADDIHJKALKCPBCHOGGLPAEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAACCEKALPMEFEPNLHNEDDIMIBCMOMOEJMDBAAAAAAACDNMPFDPKOGAPGOJECMINGNBMEEBBJMMBEAAAAAAJMHFMKCJGDNCPHLNPDKFINCAIBEHNHKBHNNHLBMP";
                    break;

                case "SILURUS2-2":
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPCOKEIAGFHPEOLECKMFPKICGHLGIIJEBAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJPIGEKOKOAFPHEOHONNJAECLBMCBNONAAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAACJEDOOGBGHAHJJHHHGMIHKHAAOJBBDGCBAAAAAAAMNGCDNEIEMPMKMKKLAFMCAPCNFADLGBIBEAAAAAAOIGBPIIOINFGKHMANJJJGPBHJJOOEANIHHJHBLAP";
                    break;

                case "ARTWEB2-1":
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAACGCCKKBLALNHGFEFJBABKAGFKICLCNBFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALOMCEHNELAPEEKMAHBACOPCJEIPHHKJPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABMIILDHALLFBEFEINJPCKJLKKFDOOGHFBAAAAAAAJHNOLJLIEAJNPOPIMKDHCGEBJGDJANIPBEAAAAAAILGHGAFCIEDNLCOHMEHAMDIODPKMJHAJNPHDHJAE";
                    break;

                case "ARTWEB2-2":
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIPLFDCLPBINEKLEILFPEHNBHCCIPGBPKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFKODKKOHGKAHFGCOIMBJEKCMBFPIFEANAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAEJBNNADELHBIGAJBNPAGHLPAGACPOJCKBAAAAAAAJKLIFJKMFFODCKKJNMMBEHALGHKCCDDABEAAAAAAIEEBFJCKBKDOEILPDALHJLJOMIDBJDJJPMEOKMPC";
                    break;

                case "SLU003354": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPCDKCBKOHAGAJJEMINBFNAHDMEJMIHLJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADJDIJBCAILLPIJNHHNGMAHDKIDAFEPMDAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPOJFPBCNDBEBOFANELLOACLIACKMOMNGBAAAAAAABDJMJOIKBFNNGAIBEAKDBOEHBGPGNFDBBEAAAAAAFCCCBFHEKDENCIOMGIJGCMOAACAOLJPHCAJMHCBL";
                    break;

                case "SLU011730": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGKOONNFFCICKGAENLBBKPOHHMCHDGMEDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFKBOJMKLBHIHFJABPDCJMDKBLFPLEOOOAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKBIIKEBKFOCMOBMHPKLCPGKOHCBCHAEJBAAAAAAACDBHCEMBNLKKMIICGFAEDLJBNJOFIGCPBEAAAAAAKFBMPAKOLLANOAPEGEPDOIJGLPPBKHDHHNBIMFJE";                    
                    break;

                case "SLU002760": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAKBCABGKFKMIFHKEFJMBEJDNDNAEMFGAFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAMFJDEEFFNGNNKJHNNNHNEOFJDNPOKDOHAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAACGIKGLIOJMODMMEHABMMDJPALGHBGDJNBAAAAAAAIGDNPJOJIKMLFKLIPJFEDLOPFKBOCMFBBEAAAAAACNNELGFONLPJNNIBDAMKIHPDOEBFFLEJJDMBNDPP";
                    break;

                case "SLU011837": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPFOBOEOIJOPHGCEFJGCCBMFMHLOBJJLJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALHOALHEIGGEIIOKCHMGBOJEGJFBFHFBJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAAIMNBEDPJNJACDAHOPKCFABJBCCGHPCFBAAAAAAAJGHIHHODLMCJGGHMJEHKLMPGLPBABFBFBEAAAAAALNBFCAIMICNLDKLEFGJDPHDIBDDEPKHMGBOABDEL";
                    break;

                case "SLU002759": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAOPFFNIFOOGEAHNEJJAEOIOCPHBKMALPHAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAABLOKICACINNGMGDLNEBIEPLMNMJDHMEHAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAEIOMKHNLIEKJEDLPHJEJEIDKBKEGBHDIBAAAAAAAPIDEECGFOCOOJEFEEFLCCDNLKGGLLKPOBEAAAAAAAHGKMNHIMFIHAJJFAJJHMJPGBIIKJODPEGIKHCLP";
                    break;

                case "SLU003657": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPPJIFEGONPINMCEELIHEHJJLFFCPGBPHAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACILDOOCONPFKOCOPJBCADKGNKCNEFHGPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAANBDMCPDNNBCOOBDMGOEHBPANPOEEFPADBAAAAAAAEOMMBNGBONOAMMKPJEFNGGINGCGMHDHGBEAAAAAAOADMCEOPHNLHFHOKBJIKDIMCGBDCFPPPEOJHDJOA";
                    break;

                case "SLU010576": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAJCDEFJGCKDKMENEIILKEOKILKCKPBJBPAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAIBHAPPKJEHBJLNNCBLGHIACEOIPJGECJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABIKMLMIIBMEFGBHHDKBOIMPENODJJGCFBAAAAAAAFACCDAHFEPIPBMFBFACNGOOGBAMMONBMBEAAAAAAEHJFPAEKLMFKKKBPEPHIFGCMLMKCNMEJPBCJHBLO";
                    break;

                case "ARTDATA-TFS": // TFS
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAKOCAKECNCHGBBEEGKCNKHJFGPDHNCIBMAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAABBKELGEACKOEAEOANNAMFMNHIPFBGPOAAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAOMJKGEDJILGMJFKCEIKKILFJHMEIDGMLBAAAAAAALPMEJBBIKEDBLBIDDDKNGJEIHOOIHLJDBEAAAAAAGINPOHMCACKDJPLHOAJJPABGEKILEANNHJFCFHLK";
                    break;

                case "SLU005126":
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAKODAGIENDBFDOEEPKAIMHGGHOICOBJNKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAIPJDKKMBDIJKJJDIMMHDAFPFBCBNPBBNAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKBMFCPHNMKNCKNEHIJJPNDBBFDBDENLBBAAAAAAAOGKMGHHEHDGBGNFHOOLCOAINOPJIEOEOBEAAAAAADIHOIEDOGEDNABBAIKACMHHBOEEKGBBOJPGFNGHJ";
                    break;

                case "TFSBUILD": // TFS
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAJDEHMIDHELNCKKEHLHOKFNFFKPMHAKNDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAEBBCHBCEBKBIGNBJKICOJMFEBKHDNKJMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAOBMBDGNEKEHDKAOFOLHJPGAIBNLBMDPHBAAAAAAANJKPCKNFDKDKDFFPGIBCKMKELOCINHIHBEAAAAAAFMKCFNEBEIBAEHCHIACIPBHOPCNMMOJIIHEGOJCO";
                    break;

                case "SEA0105WTRD2": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABCDHLPKLHGBGOOEJKJIFHDOAFNEEFMNKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPIFDMABOPCMLFDAHEIIHPIAKJMDAEEPKAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAANJLFDIKPDKECGOFGAFAIKJHFDEOGPMMKBAAAAAAAEENKODKMNELGPGIPBCPDCHFPKJDFKLDJBEAAAAAAINOHAANKPNIKPHHPEEBEDPFCDPECLFPMINFKKNFF";
                    break;

                case "ARTDATAMGM":
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAADHFCONBKFPHHENEEKPGIIHDFEBHHAMHAAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAABHOGHKCNGIODANJENEJBLLDCOEFBFMPEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPFHKDFKGEAKHCBNMOAGEEEOKPKBECNJBBAAAAAAAIMIGICNJGHHIEBBHILGMBHLCBCJCEFFFBEAAAAAAGLKPFPIMMCDNKLNCDBBHLMLFAEFNAGPCNLFHMPEC";
                    break;

                case "SLU011895": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGOMDGCCAOKJOJMECJEKHFDIAIPDOIEMOAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPIIBIABFBJKLLHMPLNFKBOCEHNFJLPBJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABKHODLDDJOJJIENJHLGAJJJOFGEGGOEMBAAAAAAAHJAEAGNNGFFJIIAJHHGJLNOEDGEKGMBPBEAAAAAAFDEFGKAHBJGNGHAOAJNCKKJMHICOJMIKMJFJLMFO";
                    break;

                case "SLU011896": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAFNEPGOOEOHKKGCECIICFNMJLKFBFPGCCAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAOFJPBFBKDOOBGOMGEGEGMKPIHBNFOBNKAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAAHPLALNGGLPPJBMLECKJLHPPBHIHMLHIBAAAAAAAIKMMGJKDEDGFMFHDMDAFINPGGAFPCDDJBEAAAAAAHABDGNMAMBAGDPDIJECNIECKBJOIKFMCBOCDEEEK";
                    break;

                case "SLU011161": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAANLBMDHALKGCLEPEDJJCOHPMCCNMJKLBPAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAKOOIINIDFKEPJJMHNNCHHOKKCOIMGEBJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJHJJBAKAODAMHHKBEKJFOOFNMKGBIOKPBAAAAAAAEDAIONCJALGLOKFBAGMFHHNNNMLOCFCGBEAAAAAABMKCHGKBECHGNJDIGOKGAABGADHAOBJNBOBNKJFL";
                    break;
                case "SLU004994": // 
                    encryptedString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAACMDPKOFLFMOPAIEOIBFKEJBLJCLNIBEIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACPJKHHHBIFJNOIGDEPFOPDAGHMDIGAGPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHMEMPFLFNDPOKIMCDPJKHBLKCLBPMLANBAAAAAAAPMIKKPGEHOELLEJPFMPPJNPFJLPPMFCFBEAAAAAAEEDIAOOOGPMFEIDMMNEMNGGCJFGLNPJLJOFMKFEF";
                    ;
                    break;

                    

                default:
                    throw new ApplicationException(Environment.MachineName + " - you can't run AnalysisPortal from this machine, contact admin");
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
            return Resources.AppSettings.Default.ApplicationUserName;
        }

        /// <summary>
        /// Get user context for current user.
        /// It can be either the application's user or the logged in user.
        /// </summary>
        /// <param name="userManager">A user manager instance.</param>
        /// <returns>User context for current user.</returns>       
        public static IUserContext GetCurrentUser(this IUserManager userManager)
        {
            IUserContext userContext = SessionHandler.UserContext;
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
            return SessionHandler.UserContext;
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
            try
            {
                applicationUserContext = CoreData.UserManager.Login(
                    userManager.GetApplicationUserName(),
                    userManager.GetApplicationPassword(),            
                    ApplicationIdentifier.AnalysisPortal.ToString());                
            }
            catch (Exception)
            {
                throw new ApplicationException("Login to UserService or other service failed.");                
            }

            if (applicationUserContext.IsNull())
            {
                throw new ApplicationException("Login to UserService or other service failed.");
            }

            // Cache application user context for all used locales.
            cacheKey = GetApplicationContextCacheKey(null);
            CacheHandler.SetApplicationUserContext(cacheKey, applicationUserContext);            
            foreach (ILocale locale in CoreData.LocaleManager.GetUsedLocales(applicationUserContext))
            {
                cacheKey = GetApplicationContextCacheKey(locale.ISOCode);
                tempApplicationUserContext = applicationUserContext.Clone();
                tempApplicationUserContext.Locale = locale;
                CacheHandler.SetApplicationUserContext(cacheKey, tempApplicationUserContext);                
            }
            // Must set current role to application user if not no observation data can retrived
            if (applicationUserContext.CurrentRoles.IsNotNull() && applicationUserContext.CurrentRoles.Count > 0)
            {
               applicationUserContext.CurrentRole = applicationUserContext.CurrentRoles[0];
            }
        }

        public static void SetUserRole(this IUserManager userManager, int id)
        {
            IUserContext userContext = SessionHandler.UserContext;

            if (userContext.IsNull())
            {
                return; // todo throw exception
                //throw new Exception("User is not logged in");
            }

            userContext.CurrentRole = userContext.CurrentRoles[id];
        }
    }
}

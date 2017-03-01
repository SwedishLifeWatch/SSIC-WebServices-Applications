using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using ArtDatabanken.Database;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.Database;
using SqlCommandBuilder = ArtDatabanken.Database.SqlCommandBuilder;

namespace ArtDatabanken.WebService.UserService.Database
{
    /// <summary>
    /// Database interface for the user database.
    /// </summary>
    public class UserServer : WebServiceDataServer
    {
        /// <summary>
        /// Activate role membership.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <param name="roleId">Role Id.</param>
        /// <returns>True, if user account was activated.</returns>
        public Boolean ActivateRoleMembership(Int32 userId, Int32 roleId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("ActivateRoleMembership");
            commandBuilder.AddParameter(ActivateRoleMembershipData.ROLE_ID, roleId);
            commandBuilder.AddParameter(ActivateRoleMembershipData.USER_ID, userId);
            lock (this)
            {
                CheckTransaction();
                return (0 < ExecuteScalar(commandBuilder));
            }
        }

        /// <summary>
        /// Activate user account.
        /// </summary>
        /// <param name="userName">UserName.</param>
        /// <param name="activationKey">Activation key.</param>
        /// <returns>True, if user account was activated.</returns>
        public Boolean ActivateUserAccount(String userName, String activationKey)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("ActivateUserAccount");
            commandBuilder.AddParameter(UserData.USER_NAME, userName);
            commandBuilder.AddParameter(UserData.ACTIVATION_KEY, activationKey);
            lock (this)
            {
                CheckTransaction();
                return (0 < ExecuteScalar(commandBuilder));
            }
        }

        /// <summary>
        /// Adds an authority data type to an application
        /// </summary>
        /// <param name="authorityDataTypeId">AuthorityDataType Id.</param>
        /// <param name="applicationId">Application Id.</param>
        /// <returns>
        /// void
        /// </returns>
        public void AddAuthorityDataTypeToApplication(Int32 authorityDataTypeId, Int32 applicationId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("InsertApplicationAuthorityDataType");
            commandBuilder.AddParameter(AuthorityDataType.AUTHORITYDATATYPE_ID, authorityDataTypeId);
            commandBuilder.AddParameter(ApplicationData.APPLICATION_ID, applicationId);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Inserts a rolemember into database. 
        /// </summary>
        /// <param name="roleId">Id for role.</param>
        /// <param name="userId">Id for user.</param>
        /// <returns>
        /// void
        /// </returns>
        public void AddUserToRole(Int32 roleId, Int32 userId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("InsertRoleMember");
            commandBuilder.AddParameter(RoleData.ROLE_ID, roleId);
            commandBuilder.AddParameter(UserData.USER_ID, userId);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Check if a translation string is unique for this object/property and locale.
        /// </summary>
        /// <param name="value">String value to check.</param>
        /// <param name="localeId">Language code</param>
        /// <param name="objectName">Name of object this string belongs to</param>
        /// <param name="propertyName">Name of property</param>
        /// <returns>Boolean - 'true' if string value is unique
        ///                  - 'false' if string value already in database
        /// </returns>
        public Boolean CheckStringIsUnique(String value, Int32 localeId, String objectName, String propertyName)
        {
            Int32 numOfRecs;
            numOfRecs = -1;
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("CheckStringIsUniqueWrapper");
            commandBuilder.AddParameter(TranslationData.VALUE, value);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            commandBuilder.AddParameter(TranslationData.TABLE_NAME, objectName);
            commandBuilder.AddParameter(TranslationData.COLUMN_NAME, propertyName);
            using (DataReader dataReader = GetRow(commandBuilder))
            {
                if (dataReader.Read())
                {
                    numOfRecs = dataReader.GetInt32(TranslationData.NUM_OF_RECS);
                }
            }
            return (numOfRecs.Equals(0));
        }

        /// <summary>
        /// Check if current user has the authority to run a particular method
        /// </summary>
        /// <param name="userId">Id of current user.</param>
        /// <param name="roleId">Id of current role.</param>
        /// <param name="authorityIdentfier">Code identifier for current authority.</param>
        /// <param name="applicationIdentfier">Code identifier for current application.</param>
        /// <param name="applicationActionIdentfier">Code identifier for current application action.</param>
        /// <returns></returns>
        public Boolean IsUserAuthorized(Int32 userId,
                                        Int32 ?roleId,
                                        String authorityIdentfier, 
                                        String applicationIdentfier,
                                        String applicationActionIdentfier)
        {
            Int32 numOfRecs;
            numOfRecs = 0;
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("CheckAuthority");
            commandBuilder.AddParameter(CheckAuthorityData.USER_ID, userId);
            if (roleId.IsNotNull())
            {
                commandBuilder.AddParameter(CheckAuthorityData.ROLE_ID, (Int32)roleId);
            }
            commandBuilder.AddParameter(CheckAuthorityData.AUTHORITY_IDENTIFIER, authorityIdentfier);
            commandBuilder.AddParameter(CheckAuthorityData.APPLICATION_IDENTIFIER, applicationIdentfier);
            commandBuilder.AddParameter(CheckAuthorityData.APPLICATION_ACTION_IDENTIFIER, applicationActionIdentfier);
            commandBuilder.AddParameter(CheckAuthorityData.COUNT, numOfRecs);
            using (DataReader dataReader = GetRow(commandBuilder))
            {
                if (dataReader.Read())
                {
                    numOfRecs = dataReader.GetInt32(CheckAuthorityData.COUNT);
                }
            }
            return (numOfRecs > 0);
        }

        /// <summary>
        /// Connect to the database.
        /// </summary>
        protected override void Connect()
        {
            Connection = new SqlConnection(GetConnectionString());
            Connection.Open();

            if (Connection.State != ConnectionState.Open)
            {
                throw new ApplicationException("Could not connect to database.");
            }
        }

        /// <summary>
        /// Get address to database.
        /// That is, address to data base server and namn of database.
        /// </summary>
        /// <returns>Address to database.</returns>
        public static String GetAddress()
        {
            return GetAddress(GetConnectionString());
        }

        /// <summary>
        /// Get connection string.
        /// </summary>
        /// <returns>Connection string.</returns>
        private static String GetConnectionString()
        {
            CipherString cipherString = new CipherString();
            String connectionString;

            // Opens the database connection.
            switch (Environment.MachineName)
            {
                case "ARTAP-ST": // Team Artportalen test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAFHDLHFNBGIGDDBEFLAHNJDBGKBEEBFIGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAIHOFLDPAPABGBANEPFOCFBNMNJLBFNJLAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALNNIKAFFNAPJGJCAMMGNNLAOHNAKLGDKHAAAAAAAEDKEMCHEAHEALGPKBHMHHDLGHHNOJBIOBOHFKNNKNKJENIMENMHGFPPCAPBOCKBIBMKKOEBHKHIMCIPCPIJJDKKJAAAKIPMLHKIEPAOPMPIMNBCNHOHIKKLFDOKEABHAGMBGLBDOMHPJNCHLFGNIAJCCFFLKLFPLKPCCBELDDPJMKMKMLNCDJKOPOMJHOCMJIOEPGFEBHLNELOGBJKFDFEPDOLDPNIDLBEAAAAAAFLFAGDGJNKICLGPHCEEAFILMLFLBNHODMFIEANAI";
                    break;

                case "ARTFAKTA-DEV": // Team Species Fact test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADHFAALIKMDKGFPPJIJODIJFIHILEFHJBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAINJFNPPDCHHPKLINNGHEEALBNPOPCDEMIAAAAAAAHDIKIPPGAHLCALAALCPOKKHBKOGNADIABDPFIFFJJCCJCDOJEFOFBGDCDOFKGKLKPDMAGPKDHHKFNOMHIMNNNLKOFIMPPLBGODPFGGGDMNBMHMCNJHCIACEJMIHOKLCJBEBNIIELLDAGMDHDGMKMNPBGDLOEHDFLOAIHEDBJEKHFKPJIELEEBDFFGOFEGFDACADKGCKDNEPJGPLKPPADDFFEPIHEKLEIDCLDGNHBGNEGGIPOGBHGGMDEDGMDGLGDBEAAAAAABABNJHGNAHMNFLBPGOFCHDDMBBGHAKEKLIFCMEPH";
                    break;

                case "ARTSERVICE2-1": // New production web service server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAONNJACJFLABJNMELGNKIMKINHHIJKMDEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAAEAINBHDBHHCAEPMJADDLHFLHMDODPMJIAAAAAAAOGCHPFKCEEKDPEMPIACIHPOOKMIIGDHOFFBCKDMCABKPLDIJCFGNNLMCBHCAMPGHCEDCDNJMAHAOGEGKIPELODJMACGDBHFEOFHNIFEBBCEGHFJLOAJMKMOGDCLCKFGKIHHOKKPFCGNMNMFKBGDLGMAMHADMGFIAMDMIIKNGEDBMKPIOEIEGJFJJBLIMDAFJCONCBGPFKINOMAPJLDBLMNKAIAFCFFBCCPPNFKMFPKHIEJMMPKFEINGMGBFGDOLFBEAAAAAANGCCGEGFAOEGBHFKFNMAFAMJMNLAIFDAJGDKEKBL";
                    break;

                case "ARTSERVICE2-2": // New production web service server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALNOCEMAFGOHOPKFKMFIDEGKFBCFOCLJNAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAADCHPKIEDJNCOJMCJBOJMJAAODFCNOIPIAAAAAAALFGDLHLOPIBALKHJJJPDILNHLOEKMKKNHBGAHOOBMJPLGPPCHLOKBAHCGCAHBGHNAMILKHGCHIHJKJKMHOLHJLNNGPAHMBPOAFONLCKAFFCIKPDJCAAMDKBGKIEFPHKFBHPOBMHMPJPHAMPGCAMBFGPBOEABLPLHNPBOCPJFHLLEJFBCILMGGIFBAPHKAJIMBOGNLGOOMIDBCFLBPOFIAMMPNGPHOPKNGKHLANHGLFAEDMDLILDLKPFIFJGMLDIFBEAAAAAAANKPAGNNJGPDCCALOHLHHOHALBDEEGDKHIACDDOG";
                    break;

                case "LAMPETRA2-1": // Production Web Server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAADJEJOJGKCKIKGFEGJMLDIGOKGPIKEIJAAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFCOJBJOPDFCHFFIIFKFGPJAGJIFOIJLFAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPCIFOOODNAMNDLOACMFEBJKLMNMGEPIDJAAAAAAAPJBKBBOLOKOCEENIKNDDKIKMHKGBLEHODJAKDCMAIFIAFJHBGKOCPIAJIEDBOCLKDLKLHDNEKIHNBELNAKHLKKHHPANNDOHLAICJDCCCNHHMNDGDNKGPBEPIHLCBBMBOJLCFAOFJALOBAMKJMKPCPPIBFFHMMMDNIFGJPENMLEMDLMPFGMEODKGBCDLNECGAHOJHPJBDNBOCEIDJNNOAEICIONLCJDHJEPNFOKEMILPNIHPBADOLMBCAHMKEOMMJLCGOIEHPAJIKPMFHLPOLAPHFOICBMCOEBEAAAAAALIHBJHIPHDFOLKKJPADCKPKDNGLOHHBDCOFBLFDO";
                    break;

                case "LAMPETRA2-2": // Production Web Server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALLEFIHOPGJLCLEEKKHONMLFNJGFDHMPDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAKPNPJOHMPEKBICHDPANICMGECMIFCFOGAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAANDJGLEHBFKGHJFJFKAIPOFBDBALBBIGGJAAAAAAAEAMIEDHIHCGEAGAGBIILMBDEPBEDDGHDALINOJAPCKDMOHNCJGMOOIAEEKNNKCOBKEPPHBFCDEEMKIAGCJPGJDIBHEKLFIPFEHCGLKDGDEBCEDPKEGLGCDEKJPHKHAGCFILEAJDFEDANAFICMFDCJDCABNODNICHAMEJMPIKCNMGCCDPCJOJCHKDMDEDIMNDLGPHBAJLOBLCAPGGOKEGIPPGJGMKBOBNIFCBBDBHBIGBPHFCDNCCDIJIAHMOLFHBJODFEIDMBEEKFKANNDPOHBBOMJCMHAHCBEAAAAAAFNPGJJDALNPGMHJCNOBIDIMAHLMABCGEMFFALOEI";
                    break;

                case "MONESES-DEV":
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEAFPECBBEFEHIKEGLFFDMHECEONJDGANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANGKAIAKHMGCBOLJLEGMPNEOJLBCALMFFAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABDPGOCMAEPILCBJLIMOBBFDBNINHMNCAIIAAAAAAKKKJKADDOHNEFICLOPBLJOKMGLAJBDNAECBMKIIPOBMGEKJGCHAJCNFDGPNCKFHKJGOHFNIGLPNGEMBMLCJCJHKDNLHEALHDAKBLLFCKLIKABHGADKJMOKEOMNGDNONOLMGNFMFNJBACBCADCDGCGIAOALANJPAAEDLLJINPGIPECHINMONACNNIICPMPJDDMNDANMKGMGLCHIJLKPBAEHONMGFHJNJHCPCAMAJFOOCJHCAMNMJKCGMFOGAFGJKJODDDEELOOMFJKCEBBEAAAAAAHCMELPGBIFDLJNLPJBKPNAFFHOIFINKIKBOHPIEN";
                    break;

                case "MONESES-ST": // System test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIKGEBDAPMKNOAFEKIHFGCOJNGHIAKFIAAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHOKKKJDAAIKNLICHBOPHJDOFOLKKDNHJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAIAMIEJBPAECILMIGBODNGBEJMBAOKHKAIAAAAAAAFFMNNIIKHOCLALOHJDCOABPAGOGFCEDBGPFPIOPDDDCMCHCGIBBFKGJNEELEFPGNKKIOINPAECNECEHPPCCADALIEBPLCNAOMCIIMGNIMKDPOILGHPOFPHECFNNPDGDAMCEBEMNGENDKJKGNOMLCFJMIHMIAHAFMLAEFOKDENGHOEIPHKMGKKJGPGOGHIDKNMKGCIIOBOAOLEKOLGCDOMEDDJCGFLLJBALFKELENBAALNHPCDMAGJCAOJNGCIGCBBEAAAAAAGDNEJOLBFGBEIOOPFBIOMNFAOPLJDEGLPGHMIJHC";
                    break;

                case "ANDRENA2-1":
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPIJCEBDOMBCEGPEFJGCOJHFHKJDDKCPOAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAMMDBCOJNNMJAJGAIJKKCPPNIOELDPBIBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHMPBMPDCCKNDCNGMPIFKKOGKAOEKFJFAIIAAAAAAKJDABEKAEKFCNLJMFLJMMDNHLMHBFMFMDMOGNGAFPDNGGCKCILKGNGKPBAEMBCPIEEJLKEDKCIAOOLMJGBFKPCIPBELFOKOKIHPOMNAFNMBFPFOOBKPANDGNLPPLAABLCDBKJDEMBLHNCDKDPEPPMHBGAPBGCKFCBDNIJPJDJJMFOMDLCIDAHHIEGHOINJEKALBEEFLMHOCPCJDDMNHABHKGMFBAEDHOLADNEGFIIGNNPIAGDGMCPHMKJFOIDOMBPDNFBDCDBEFCDLODBEAAAAAAMDKMIIEAANOPFNMFCMBFDHFNJOJOEHNHNKAEBCIG";
                    break;

                case "SLU003354": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGLGBDIAIDBHFHEEKIOMMGMEHJFINCANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAGBNPLPMNIPGGNKGDLIDCODJAKPHICCKGAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJPAAKNBDPJADCODKDKPPPLFLOMBJLIMPHIAAAAAAFBHAKOGHHPHJEAKFOCCAKIHBEFBBMEKGADOPHNAGAEACKKOFOALMGHHKMNDPIOIMHEGHMIABKGJGGBPLFBPIKCDAJAMPMBKBHAIKOINEACCOIJBDBNPLOCJMONECHEKKCPJPJCNHPLMCCOABFHLADLNOFECLPGJEEBDPICDIKAPGPMKOBMDINMNKMGDCGBDBGCCBNNHKAKGNNOGILBBDMMGMNFHFGGDOMALGMKFIOJIGLDPOBEAAAAAAHFNAGGEFCDCGGGHOGDABEIPMJBKJFCIHBLAPJIEO";
                    break;

                case "SLU011837": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALCPBDBGCIAFPEPEILEKMLLIKEOOJMJMKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJIGHEIDHPEDFANGFKCBDONIDLCGGNLAEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABECJBCLBJOOHIFLKGNJLGNDNPGCDDDDNIAAAAAAAEGAIGCKFNGOMMABEGIHKCKHJKLPACJIKCABAILODFHFMOKICDNDAMCNIMELOFLDBFDHCDBAMIEAFJLGEINEKIAIGPCOHLNJCNPCHHBPHBCJFFHEEAOHOIAAAEODKBOCMPDOKMPACHAGDIIKONICCNOALLCOIADGELAJLFKIAMNDMPKJPLHHHFEJDECIHANCGGLNALGBEPCLGFPEKOIBJMONNILEHJHLOOBKBCFENIDGJDEADKPFCNMDOHHMCNNDABEAAAAAAJFCCJGBDHCOKECNHEDEDAKKGBELDODDBMGADFDLG";
                    break;

                case "SLU002759": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAKOJGMLOJCEHLOLEOLHGMDGJMGHPELCGAAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALAOMOJGEKGJBAIMPABFKNHJHNDKKMOMCAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFLFHAHMPNFMCONAKCEMIJOLLELMLABIAHIAAAAAAAIOMMCIMPJOCMGEPECGACJCHHGLNBHEHONBOLFCFKBMOPIIKFKHKKGNGKPJJNHNBCGFOHHIONDJCEECBBOADIKGBCDBCABDLNNGLMMKMDDPDJLHGLBFPLMHIOAPEAAIKKFBBGNJNJABMKCKGBLPNJEHCDAIBKBKOMLIFBBHGOHFMDADHGAFABCIBAGJPBLPOLKLADICLFHKLCCIHCIPEEKDKBEFFCJGNMEMFGCLPCBKALDJKBEAAAAAAOHLFGKPIAIMPNDJHFGMABBELKIFMKMMODOHDKEJH";
                    break;

                case "SLU002760": // 
                    // produktion sql-belone
                    // connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEEOMOLNGPCJLAPEBLNNPEFHNPDCANGOLAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAONBALNOLDBHHAKNGNGMMODNDLAGMAIPMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHOCOEGDPEFOLJPEPFEDNFOOCEMIGAEPEJAAAAAAADNJLCMMFFEKBKKIHBNIOGGHCPIPPJJMDGCIFIJONDMFPAPKDAMIAEFCHMFABMFFPPOCKLHDLJGEKJEPIJPCIEMANNGPNHDHKEFCBEFEFLGGIDPBBLBCLNBFPBAIAIKLAGFGLHONKBJMPEMBPDEPIIHDLFEFFBNPKBMLOGFJFHADFHNEOGPEHCCDMNNBKNDNGMKLIOHIAGLOHHDIAHDFIMCOMDKPNICHKJIICHPNOBFHEMFGLNIKBDFBOCLLGBCGDOJBKOENMPBLFABMLHPELEIJNKBGFFMHNBEAAAAAAJGHGCDOBDMPGJHMDEMFDOAOGFKDKAPPKEHHFJGJO";
                    // dev MONESES-DEV64
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAJBJHNPKPCAPPGHEHKCIIOPEAPHMLGIHIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANAHDNLIDJOLLILACFOCIANGEAPCEIBPLAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAABLIOKDHPBCPDOPGLOCEIFJOILEPLKEJHIAAAAAALLJONBFKEAAGLMIHIFDPJNOEIMGFGCFJGLFPFJJLKFGGNHPMGAOODJELKMLFBIDIBLMHCIEDNKKKBNALCOINMOBLHKJEAAKLDKLMBCFAGGJIOLOCCMLLCHLEHJKNMCBAEGNIDLAFKAHOMLAMAFKKPFCKOHFAPOLKKINFOFABNLFMPPMNPBHKCNANBCOECDIBGMEIIELJHHDPBJDNGMHOFJKPKPEDKNIHPBGLAABDHEMNGBGHBEAAAAAAGBAIMOKHKHOJDKJMCOIADNOIHJJAMBBAOJICFCBO";
                    break;                

                case "SLU011730": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAADIHMOJGONBDBMKEPJFMKFPHPJJOAAEHLAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANIMAGAHHFMENMPGHIGMPLEIPPANMCCDIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJBHBDFHCHCJNJNOOBCIIGPLKLGNGJCDMIIAAAAAALIHPNHIEODKAAKIPEAILJBFLJHMDKCFIMHBDNEDKBEEIMMCDHIOEHNBLJLCCNMCJLFHFCEPDELKEALDHLLNBDPNOIBPGDINAMMIHOFGFOEIPAJJDNELFFEAHPJIEBLDKJEIBEJCHDGBFMKICAALIAEEBKBPEKOPDMHHCBLHJAONNLKFHCELCHMJJIGFECIGNFMKCIMMAGFKGBGGDLHDLNPACAKKLPLCGEGPMBJNOCCGBNJKJOGKJAIGJJNEMDAGKKHHKFODMNKDJAMNDBEAAAAAAILLKCCFBFCODHAMFMCNCPJDEFGFBOCLIHNGENDPN";
                    break;

                case "SLW-DEV": // Team Two Blueberries test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAEONHMLNKHGBMPPIFCLIDPNDKGEALKPONAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAIAGEKOMIHMNJAADNDLOJJFEHHDBNICBOIAAAAAAAPHNBOCOMKLNPNBEFKGCEKFADFALOMHKOGOANNOEODPLJCLKLHAOJGLEBMBCNFNABFGOFNPJGPGKJPPKMDFNDHFINGICOCENOHONIHADBLDFPCJLPDDHKIGFMAIKGALDHHNKLLBAKHEIICJGMKLJGFPNKGLHCBIIBLJDEGMPENIANJJPOLGJFHFAHLPBLEBLJDJIALKECHJKCDHCPBHEJDBNBEPJFICDOACJIPMOIPFEGAFNJCGNJPJLCKBFIFENFBEAAAAAABLECAPNHEENCODAEDFKLPKMLBODCCIDGLCBAILAF";
                    break;
                    
                default:
                    throw new ApplicationException("Unknown web server " + Environment.MachineName);

            }
            return cipherString.DecryptText(connectionString);
        }

        /// <summary>
        /// Inserts an application into database. 
        /// </summary>
        /// <param name="applicationIdentity">Application identity.</param>
        /// <param name="name">Application name.</param>
        /// <param name="shortName">Application short name.</param>
        /// <param name="URL">Application URL</param>
        /// <param name="description">Description.</param>
        /// <param name="localeId">LocaleId.</param>
        /// <param name="contactPersonId">Contact person id (optional).</param>
        /// <param name="administrationRoleId">AdministrationRoleId.</param>
        /// <param name="createdBy">UserId that created the record.</param>
        /// <param name="validFromDate">Date from this application is valid.</param>
        /// <param name="validToDate">Date that this application stops being valid.</param>
        /// <returns>
        /// Returns applicationId for the created record
        /// </returns>
        public Int32 CreateApplication(String applicationIdentity, String name, String shortName,
                                String URL, String description, Int32 localeId, Int32? contactPersonId,
                                Int32? administrationRoleId, Int32 createdBy, 
                                DateTime validFromDate, DateTime validToDate)
        {
            Int32 applicationId = Int32.MinValue;
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("InsertApplication");
            commandBuilder.AddParameter(ApplicationData.APPLICATION_IDENTITY, applicationIdentity);
            commandBuilder.AddParameter(ApplicationData.NAME, name);
            commandBuilder.AddParameter(ApplicationData.SHORT_NAME, shortName);
            commandBuilder.AddParameter(ApplicationData.URL, URL);
            commandBuilder.AddParameter(ApplicationData.DESCRIPTION, description);
            if (contactPersonId.HasValue)
            {
                commandBuilder.AddParameter(ApplicationData.CONTACT_PERSON_ID, contactPersonId.Value);
            }
            if (administrationRoleId.HasValue)
            {
                commandBuilder.AddParameter(ApplicationData.ADMINISTRATION_ROLE_ID, administrationRoleId.Value);
            }
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            commandBuilder.AddParameter(ApplicationData.CREATED_BY, createdBy);
            commandBuilder.AddParameter(ApplicationData.VALID_FROM_DATE, validFromDate);
            commandBuilder.AddParameter(ApplicationData.VALID_TO_DATE, validToDate);
            lock (this)
            {
                CheckTransaction();
                using (DataReader dataReader = GetRow(commandBuilder))
                {
                    if (dataReader.Read())
                    {
                        applicationId = dataReader.GetInt32(ApplicationData.ID);
                    }
                }
            }
            return applicationId;
        }

        /// <summary>
        /// Inserts applicationaction into database. 
        /// </summary>
        /// <param name="applicationId">Application id.</param>
        /// <param name="name">Name.</param>
        /// <param name="actionIdentity">Action Identity</param>
        /// <param name="description">Description.</param>
        /// <param name="localeId">LocaleId.</param>
        /// <param name="administrationRoleId">AdministrationRoleId</param>
        /// <param name="createdBy">UserId for user creating the record.</param>
        /// <param name="validFromDate">Date from this applicationversion is valid.</param>
        /// <param name="validToDate">Date that this applicationversion stops being valid.</param>
        /// <returns>
        /// Returns Id for the created record
        /// </returns>
        public Int32 CreateApplicationAction(Int32 applicationId, String name, String actionIdentity,
                                String description, Int32 localeId, Int32? administrationRoleId,
                                Int32 createdBy, DateTime validFromDate, DateTime validToDate)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("InsertApplicationAction");
            commandBuilder.AddParameter(ApplicationActionData.APPLICATION_ID, applicationId);
            commandBuilder.AddParameter(ApplicationActionData.NAME, name);
            commandBuilder.AddParameter(ApplicationActionData.ACTION_IDENTITY, actionIdentity);
            commandBuilder.AddParameter(ApplicationActionData.DESCRIPTION, description);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            if (administrationRoleId.HasValue)
            {
                commandBuilder.AddParameter(ApplicationActionData.ADMINISTRATION_ROLE_ID, administrationRoleId.Value);
            }
            commandBuilder.AddParameter(ApplicationActionData.CREATED_BY, createdBy);
            commandBuilder.AddParameter(ApplicationActionData.VALID_FROM_DATE, validFromDate);
            commandBuilder.AddParameter(ApplicationActionData.VALID_TO_DATE, validToDate);
            lock (this)
            {
                CheckTransaction();
                // return Id for the ApplicationAction record
                return ExecuteScalar(commandBuilder);
            }
        }

        /// <summary>
        /// Inserts applicationversion into database. 
        /// </summary>
        /// <param name="applicationId">Application id.</param>
        /// <param name="version">Version.</param>
        /// <param name="isRecommended">Is this the recommended version?</param>
        /// <param name="isValid">Is this a valid version </param>
        /// <param name="description">Description.</param>
        /// <param name="localeId">LocaleId.</param>
        /// <param name="createdBy">UserId for user creating the record.</param>
        /// <param name="validFromDate">Date from this applicationversion is valid.</param>
        /// <param name="validToDate">Date that this applicationversion stops being valid.</param>
        /// <returns>
        /// Returns Id for the created record
        /// </returns>
        public Int32 CreateApplicationVersion(Int32 applicationId, String version, Boolean isRecommended, 
                                Boolean isValid, String description, Int32 localeId, 
                                Int32 createdBy, DateTime validFromDate, DateTime validToDate)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("InsertApplicationVersion");
            commandBuilder.AddParameter(ApplicationVersionData.APPLICATION_ID, applicationId);
            commandBuilder.AddParameter(ApplicationVersionData.VERSION, version);
            commandBuilder.AddParameter(ApplicationVersionData.IS_RECOMMENDED, isRecommended);
            commandBuilder.AddParameter(ApplicationVersionData.IS_VALID, isValid);
            commandBuilder.AddParameter(ApplicationVersionData.DESCRIPTION, description);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            commandBuilder.AddParameter(ApplicationVersionData.CREATED_BY, createdBy);
            commandBuilder.AddParameter(ApplicationVersionData.VALID_FROM_DATE, validFromDate);
            commandBuilder.AddParameter(ApplicationVersionData.VALID_TO_DATE, validToDate);
            lock (this)
            {
                CheckTransaction();
                // return Id for the ApplicationVersion record
                return ExecuteScalar(commandBuilder);
            }
        }

        /// <summary>
        /// Inserts authority into database. 
        /// </summary>
        /// <param name="roleId">Role id.</param>
        /// <param name="applicationId">Application id.</param>
        /// <param name="authorityIdentity">Authority Identity.</param>
        /// <param name="authorityDataTypeId">AuthorityDataTypeId.</param>
        /// <param name="name">Authority name.</param>
        /// <param name="showNonPublicData">ShowNonPublicData</param>
        /// <param name="maxProtectionLevel">MaxProtectionLevel</param>
        /// <param name="readPermission">ReadPermission</param>
        /// <param name="createPermission">CreatePermission</param>
        /// <param name="updatePermission">UpdatePermission</param>
        /// <param name="deletePermission">DeletePermission</param>
        /// <param name="administrationRoleId">AdministrationRoleId</param>
        /// <param name="description">Description.</param>
        /// <param name="obligation">Obligation</param>
        /// <param name="localeId">LocaleId.</param>
        /// <param name="createdBy">UserId for user creating the record.</param>
        /// <param name="validFromDate">Date from this authority is valid.</param>
        /// <param name="validToDate">Date that this authority stops being valid.</param>
        /// <returns>
        /// Returns Id for the created record
        /// </returns>
        public Int32 CreateAuthority(Int32 roleId, Int32? applicationId, String authorityIdentity, Int32? authorityDataTypeId, String name, Boolean showNonPublicData, Int32 maxProtectionLevel,
                                Boolean readPermission, Boolean createPermission, Boolean updatePermission, Boolean deletePermission, 
                                Int32? administrationRoleId, String description, String obligation, Int32 localeId,
                                Int32 createdBy, DateTime validFromDate, DateTime validToDate)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("InsertAuthority");
            if (applicationId.HasValue)
            {
                commandBuilder.AddParameter(AuthorityData.APPLICATION_ID, applicationId.Value);
            }
            commandBuilder.AddParameter(AuthorityData.ROLE_ID, roleId);
            commandBuilder.AddParameter(AuthorityData.AUTHORITY_IDENTITY, authorityIdentity);
            if (authorityDataTypeId.HasValue)
            {
                commandBuilder.AddParameter(AuthorityData.AUTHORITY_DATA_TYPE_ID, authorityDataTypeId.Value);
            }
            commandBuilder.AddParameter(AuthorityData.NAME, name);
            commandBuilder.AddParameter(AuthorityData.SHOW_NON_PUBLIC_DATA, showNonPublicData);
            commandBuilder.AddParameter(AuthorityData.MAX_PROTECTION_LEVEL, maxProtectionLevel);
            commandBuilder.AddParameter(AuthorityData.READ_PERMISSION, readPermission);
            commandBuilder.AddParameter(AuthorityData.CREATE_PERMISSION, createPermission);
            commandBuilder.AddParameter(AuthorityData.UPDATE_PERMISSION, updatePermission);
            commandBuilder.AddParameter(AuthorityData.DELETE_PERMISSION, deletePermission);
            if (administrationRoleId.HasValue)
            {
                commandBuilder.AddParameter(AuthorityData.ADMINISTRATION_ROLE_ID, administrationRoleId.Value);
            }
            commandBuilder.AddParameter(AuthorityData.DESCRIPTION, description);
            commandBuilder.AddParameter(AuthorityData.OBLIGATION, obligation);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            commandBuilder.AddParameter(AuthorityData.CREATED_BY, createdBy);
            commandBuilder.AddParameter(AuthorityData.VALID_FROM_DATE, validFromDate);
            commandBuilder.AddParameter(AuthorityData.VALID_TO_DATE, validToDate);
            lock (this)
            {
                CheckTransaction();
                // return Id for the Authority record
                return ExecuteScalar(commandBuilder);
            }
        }


        /// <summary>
        /// Inserts a authority attribute into database. 
        /// </summary>
        /// <param name="authorityId">AuthorityId.</param>
        /// <param name="authorityAttributeTypeId">AuthorityAttributeTypeId</param>
        /// <param name="attributeValue">Value for this attribute.</param>
        /// <returns>
        /// void.
        /// </returns>
        public void CreateAuthorityAttribute(Int32 authorityId, Int32 authorityAttributeTypeId, String attributeValue)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("InsertAuthorityAttribute");
            commandBuilder.AddParameter(AuthorityData.AUTHORITY_ID, authorityId);
            commandBuilder.AddParameter(AuthorityData.AUTHORITY_ATTRIBUTE_TYPE_ID, authorityAttributeTypeId);
            commandBuilder.AddParameter(AuthorityData.ATTRIBUTE_VALUE, attributeValue);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Inserts a organization into database. 
        /// </summary>
        /// <param name="name">Organization name.</param>
        /// <param name="shortName">Shortname.</param>
        /// <param name="description">Description.</param>
        /// <param name="administrationRoleId">AdministrationRoleId.</param>
        /// <param name="hasCollection">HasCollection - true if organization owns a collection</param>
        /// <param name="organizationCategoryId">Id for OrganizationCategory</param>
        /// <param name="createdBy">UserId that created the record.</param>
        /// <param name="localeId">LocaleId.</param>        
        /// <param name="validFromDate">Date from this organization is valid.</param>
        /// <param name="validToDate">Date that this organization stops being valid.</param>
        /// <returns>
        /// Returns organizationId for the created record
        /// </returns>
        public Int32 CreateOrganization(String name, String shortName, String description,
                                Int32? administrationRoleId, Boolean hasCollection, Int32 organizationCategoryId,
                                Int32 createdBy, Int32 localeId, DateTime validFromDate, DateTime validToDate)
        {
            Int32 organizationId = Int32.MinValue;
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("InsertOrganization");
            commandBuilder.AddParameter(OrganizationData.NAME, name);
            commandBuilder.AddParameter(OrganizationData.SHORT_NAME, shortName);
            commandBuilder.AddParameter(OrganizationData.DESCRIPTION, description);
            commandBuilder.AddParameter(OrganizationData.ORGANIZATION_CATEGORY_ID, organizationCategoryId);
            if (administrationRoleId.HasValue)
            {
                commandBuilder.AddParameter(OrganizationData.ADMINISTRATION_ROLE_ID, administrationRoleId.Value);
            }
            commandBuilder.AddParameter(OrganizationData.HAS_COLLECTION, hasCollection);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            commandBuilder.AddParameter(OrganizationData.CREATED_BY, createdBy);
            commandBuilder.AddParameter(OrganizationData.VALID_FROM_DATE, validFromDate);
            commandBuilder.AddParameter(OrganizationData.VALID_TO_DATE, validToDate);
            lock (this)
            {
                CheckTransaction();
                using (DataReader dataReader = GetRow(commandBuilder))
                {
                    if (dataReader.Read())
                    {
                        organizationId = dataReader.GetInt32(OrganizationData.ID);
                    }
                }
            }
            return organizationId;
        }

        /// <summary>
        /// Inserts OrganizationCategory into database. 
        /// </summary>
        /// <param name="name">OrganizationCategory name.</param>
        /// <param name="description">Description.</param>
        /// <param name="administrationRoleId">AdministrationRoleId.</param>
        /// <param name="createdBy">UserId that created the record.</param>
        /// <param name="localeId">LocaleId.</param>        
        /// <returns>
        /// Returns OrganizationCategoryId for the created record
        /// </returns>
        public Int32 CreateOrganizationCategory(String name, String description,
                                Int32? administrationRoleId, Int32 createdBy, Int32 localeId)
        {
            Int32 organizationCategoryId = Int32.MinValue;
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("InsertOrganizationCategory");
            commandBuilder.AddParameter(OrganizationCategoryData.ORGANIZATION_CATEGORY_NAME, name);
            commandBuilder.AddParameter(OrganizationCategoryData.ORGANIZATION_CATEGORY_DESCRIPTION, description);
            if (administrationRoleId.HasValue)
            {
                commandBuilder.AddParameter(OrganizationCategoryData.ADMINISTRATION_ROLE_ID, administrationRoleId.Value);
            }
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            commandBuilder.AddParameter(OrganizationCategoryData.CREATED_BY, createdBy);
            lock (this)
            {
                CheckTransaction();
                using (DataReader dataReader = GetRow(commandBuilder))
                {
                    if (dataReader.Read())
                    {
                        organizationCategoryId = dataReader.GetInt32(OrganizationCategoryData.ID);
                    }
                }
            }

            return organizationCategoryId;
        }

        /// <summary>
        /// Inserts a person into database. 
        /// </summary>
        /// <param name="userId">Id value for User that this person is related to.</param>
        /// <param name="firstName">FirstName.</param>
        /// <param name="middleName">MiddleName.</param>
        /// <param name="lastName">LastName.</param>
        /// <param name="genderId">Id for gender.</param>
        /// <param name="emailAddress">Emailaddress.</param>
        /// <param name="showEmail">Show emailaddress or not.</param>
        /// <param name="showAddresses">Show addresses or not.</param>
        /// <param name="showPhoneNumbers">Show phonenumbers or not.</param>
        /// <param name="birthYear">BirthYear.</param>
        /// <param name="deathYear">DeathYear.</param>
        /// <param name="administrationRoleId">AdministrationRoleId.</param>
        /// <param name="hasCollection">HasCollection</param>
        /// <param name="localeId">Locale expressed as id.</param>        
        /// <param name="taxonNameTypeId">TaxonNameTypeId.</param>
        /// <param name="URL">Persons URL.</param>
        /// <param name="presentation">Presentation written in Locale-language.</param>
        /// <param name="showPresentation">Show presentation or not.</param>
        /// <param name="showPersonalInformation">Show personalinformation or not.</param>
        /// <param name="createdBy">UserId that created the record.</param>
        /// <returns>
        /// Returns personId for the created record
        /// </returns>
        public Int32 CreatePerson(Int32? userId, String firstName, String middleName, String lastName,
                                Int32 genderId, String emailAddress, Boolean showEmail, Boolean showAddresses,
                                Boolean showPhoneNumbers, DateTime? birthYear, DateTime? deathYear,
                                Int32? administrationRoleId, Boolean hasCollection, Int32 localeId, Int32 taxonNameTypeId, String URL, String presentation, 
                                Boolean showPresentation, Boolean showPersonalInformation, Int32 createdBy)
        {
            Int32 personId = Int32.MinValue;
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("InsertPerson");
            if (userId.HasValue)
            {
                commandBuilder.AddParameter(PersonData.USER_ID, userId.Value);
            }
            commandBuilder.AddParameter(PersonData.FIRST_NAME, firstName);
            commandBuilder.AddParameter(PersonData.MIDDLE_NAME, middleName);
            commandBuilder.AddParameter(PersonData.LAST_NAME, lastName);
            commandBuilder.AddParameter(PersonData.GENDER_ID, genderId);
            commandBuilder.AddParameter(EmailData.EMAIL_ADDRESS, emailAddress);
            commandBuilder.AddParameter(EmailData.SHOW_EMAIL, showEmail);
            commandBuilder.AddParameter(PersonData.SHOW_ADDRESSES, showAddresses);
            commandBuilder.AddParameter(PersonData.SHOW_PHONENUMBERS, showPhoneNumbers);
            if (birthYear.HasValue)
            {
                commandBuilder.AddParameter(PersonData.BIRTH_YEAR, birthYear.Value);
            }
            if (deathYear.HasValue)
            {
                commandBuilder.AddParameter(PersonData.DEATH_YEAR, deathYear.Value);
            }
            if (administrationRoleId.HasValue)
            {
                commandBuilder.AddParameter(PersonData.ADMINISTRATION_ROLE_ID, administrationRoleId.Value);
            }
            commandBuilder.AddParameter(PersonData.HAS_COLLECTION, hasCollection);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            commandBuilder.AddParameter(PersonData.TAXON_NAME_TYPE_ID, taxonNameTypeId);
            commandBuilder.AddParameter(PersonData.URL, URL);
            commandBuilder.AddParameter(PersonData.PRESENTATION, presentation);
            commandBuilder.AddParameter(PersonData.SHOW_PRESENTATION, showPresentation);
            commandBuilder.AddParameter(PersonData.SHOW_PERSONALINFORMATION, showPersonalInformation);
            commandBuilder.AddParameter(PersonData.CREATED_BY, createdBy);
            lock (this)
            {
                CheckTransaction();
                using (DataReader dataReader = GetRow(commandBuilder))
                {
                    if (dataReader.Read())
                    {
                        personId = dataReader.GetInt32("Id");
                    }
                }
            }

            return personId;
        }

        /// <summary>
        /// Inserts an address into database. 
        /// </summary>
        /// <param name="personId">Id value for person that this address is related to.</param>
        /// <param name="organizationId">Id value for organization that this address is related to.</param>
        /// <param name="postalAddress1">PostalAddress1.</param>
        /// <param name="postalAddress2">PostalAddress2.</param>
        /// <param name="zipCode">ZipCode.</param>
        /// <param name="city">City.</param>
        /// <param name="countryId">Id for Country.</param>
        /// <param name="addressTypeId">Id value for AddressType.</param>
        /// <returns>void</returns>
        public void CreateAddress(Int32 personId, Int32 organizationId, String postalAddress1, String postalAddress2,
                        String zipCode, String city, Int32 countryId, Int32 addressTypeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("InsertAddress");
            commandBuilder.AddParameter(AddressData.PERSON_ID, personId);
            commandBuilder.AddParameter(AddressData.ORGANIZATION_ID, organizationId);
            commandBuilder.AddParameter(AddressData.POSTALADDRESS1 , postalAddress1);
            commandBuilder.AddParameter(AddressData.POSTALADDRESS2, postalAddress2);
            commandBuilder.AddParameter(AddressData.ZIPCODE, zipCode);
            commandBuilder.AddParameter(AddressData.CITY, city);
            commandBuilder.AddParameter(AddressData.COUNTRY_ID, countryId);
            commandBuilder.AddParameter(AddressData.ADDRESS_TYPE_ID, addressTypeId);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Inserts a phonenumber into database. 
        /// </summary>
        /// <param name="personId">Id value for person that this address is related to.</param>
        /// <param name="organizationId">Id value for organization that this address is related to.</param>
        /// <param name="phoneNumber">PhoneNumber.</param>
        /// <param name="countryId">Id for country. Used for Country prefix</param>
        /// <param name="phoneNumberTypeId">Id value for PhoneNumber.</param>
        /// <returns>void</returns>
        public void CreatePhoneNumber(Int32 personId, Int32 organizationId, String phoneNumber, Int32 countryId, Int32 phoneNumberTypeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("InsertPhoneNumber");
            commandBuilder.AddParameter(PhoneNumberData.PERSON_ID, personId);
            commandBuilder.AddParameter(PhoneNumberData.ORGANIZATION_ID, organizationId);
            commandBuilder.AddParameter(PhoneNumberData.PHONENUMBER, phoneNumber);
            commandBuilder.AddParameter(PhoneNumberData.COUNTRY_ID, countryId);
            commandBuilder.AddParameter(PhoneNumberData.PHONENUMBER_TYPE_ID, phoneNumberTypeId);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Inserts a role into database. 
        /// </summary>
        /// <param name="roleName">Role name.</param>
        /// <param name="shortName">Shortname.</param>
        /// <param name="description">Description.</param>
        /// <param name="administrationRoleId">AdministrationRoleId.</param>
        /// <param name="userAdministrationRoleId">UserAdministrationRoleId.</param>
        /// <param name="organizationId">OrganizationId.</param>        
        /// <param name="createdBy">UserId that created the record.</param>
        /// <param name="localeId">LocaleId.</param>        
        /// <param name="validFromDate">Date from this role is valid.</param>
        /// <param name="validToDate">Date that this role stops being valid.</param>
        /// <param name="identifier">Role identifier.</param>
        /// <param name="isActivationRequired">Is activation required.</param>
        /// <param name="messageTypeId">Message type id.</param>
        /// <returns>
        /// Returns roleId for the created record
        /// </returns>
        public Int32 CreateRole(String roleName, String shortName, String description, Int32 localeId,
                                Int32? administrationRoleId, Int32? userAdministrationRoleId,
                                Int32? organizationId, Int32 createdBy, DateTime validFromDate, DateTime validToDate,
                                String identifier, Boolean isActivationRequired, Int32 messageTypeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("InsertRole");
            commandBuilder.AddParameter(RoleData.ROLE_NAME, roleName);
            commandBuilder.AddParameter(RoleData.SHORT_NAME, shortName);
            commandBuilder.AddParameter(RoleData.DESCRIPTION, description);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            if (administrationRoleId.HasValue)
            {
                commandBuilder.AddParameter(RoleData.ADMINISTRATION_ROLE_ID, administrationRoleId.Value);
            }
            if (userAdministrationRoleId.HasValue)
            {
                commandBuilder.AddParameter(RoleData.USER_ADMINISTRATION_ROLE_ID, userAdministrationRoleId.Value);
            }
            if (organizationId.HasValue)
            {
                commandBuilder.AddParameter(RoleData.ORGANIZATION_ID, organizationId.Value);
            }
            commandBuilder.AddParameter(RoleData.CREATED_BY, createdBy);
            commandBuilder.AddParameter(RoleData.VALID_FROM_DATE, validFromDate);
            commandBuilder.AddParameter(RoleData.VALID_TO_DATE, validToDate);
            commandBuilder.AddParameter(RoleData.IDENTIFIER, identifier);
            commandBuilder.AddParameter(RoleData.IS_ACTIVATION_REQUIRED, isActivationRequired);
            commandBuilder.AddParameter(RoleData.MESSAGE_TYPE_ID, messageTypeId);
            lock (this)
            {
                CheckTransaction();
                // return Id for the created role
                return ExecuteScalar(commandBuilder);
            }
        }

        /// <summary>
        /// Inserts a user into database. 
        /// </summary>
        /// <param name="userName">UserName.</param>
        /// <param name="password">User password.</param>
        /// <param name="personId">Key to record in Person that this User is related to.</param>
        /// <param name="applicationId">ApplicationId - if User is an application.</param>
        /// <param name="userType">Type of user.</param>
        /// <param name="GUID">GUID.</param>
        /// <param name="emailAddress">Users email address.</param>
        /// <param name="showEmail">Show emailaddress or not.</param>
        /// <param name="authenticationType">AuthenticationType.</param>
        /// <param name="accountActivated">Is the account activated.</param>
        /// <param name="activationKey">Key for account activation.</param>
        /// <param name="administrationRoleId">AdministrationRoleId.</param>
        /// <param name="modifiedBy">UserId that creates the record.</param>
        /// <param name="validFromDate">Date from this user is valid.</param>
        /// <param name="validToDate">Date that this users stops being valid.</param>
        /// <returns>
        /// Returns userId for the created record
        /// </returns>
        public Int32 CreateUser(String userName, String password, Int32? personId, Int32? applicationId, String userType, String GUID, String emailAddress, 
                                Boolean showEmail, Int32 authenticationType, Boolean accountActivated, String activationKey,
                                Int32? administrationRoleId, Int32 modifiedBy, DateTime validFromDate, DateTime validToDate )
        {
            String _defaultNull = "null";
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("InsertUser");
            commandBuilder.AddParameter(UserData.USER_NAME, userName);
            commandBuilder.AddParameter(UserData.PASSWORD, password);
            if (personId.HasValue)
            {
                commandBuilder.AddParameter(UserData.PERSON_ID, personId.Value);
            }
            if (applicationId.HasValue)
            {
                commandBuilder.AddParameter(UserData.APPLICATION_ID, applicationId.Value);
            }
            commandBuilder.AddParameter(UserData.USER_TYPE, userType);
            commandBuilder.AddParameter(UserData.GUID, GUID);
            commandBuilder.AddParameter(EmailData.EMAIL_ADDRESS, emailAddress);
            commandBuilder.AddParameter(EmailData.SHOW_EMAIL, showEmail);
            commandBuilder.AddParameter(UserData.AUTHENTICATION_TYPE, authenticationType, _defaultNull);
            commandBuilder.AddParameter(UserData.ACCOUNT_ACTIVATED, accountActivated);
            commandBuilder.AddParameter(UserData.ACTIVATION_KEY, activationKey);
            if (administrationRoleId.HasValue)
            {
                commandBuilder.AddParameter(UserData.ADMINISTRATION_ROLE_ID, administrationRoleId.Value);
            }
            commandBuilder.AddParameter(UserData.MODIFIED_BY, modifiedBy);
            commandBuilder.AddParameter(UserData.VALID_FROM_DATE, validFromDate, _defaultNull);
            commandBuilder.AddParameter(UserData.VALID_TO_DATE, validToDate, _defaultNull);
            lock (this)
            {
                CheckTransaction();
                // return UserId for the created user
                return ExecuteScalar(commandBuilder);
            }
        }

        /// <summary>
        /// Delete addresses from database table
        /// </summary>
        /// <param name="personId">Id value for Person that has the addresses that should be deleted.</param>
        /// <param name="organizationId">Id value for Organization that has the addresses that should be deleted.</param>
        /// <returns>
        /// void
        /// </returns>
        public void DeleteAddress(Int32 personId, Int32 organizationId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("DeleteAddress");
            commandBuilder.AddParameter(AddressData.PERSON_ID, personId);
            commandBuilder.AddParameter(AddressData.ORGANIZATION_ID, organizationId);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Delete authority from database table
        /// </summary>
        /// <param name="authorityId">Id value for authority that should be deleted.</param>
        /// <returns>
        /// void
        /// </returns>
        public void DeleteAuthority(Int32 authorityId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("DeleteAuthority");
            commandBuilder.AddParameter(AuthorityData.AUTHORITY_ID, authorityId);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Delete Application from database table
        /// </summary>
        /// <param name="applicationId">Id value for Application to be deleted.</param>
        /// <param name="modifiedBy">UserId that deletes the record.</param>
        /// /// <returns>
        /// void
        /// </returns>
        public void DeleteApplication(Int32 applicationId, Int32 modifiedBy)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("DeleteApplication");
            commandBuilder.AddParameter(ApplicationData.ID, applicationId);
            commandBuilder.AddParameter(ApplicationData.MODIFIED_BY, modifiedBy);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Delete attribute values from authority 
        /// </summary>
        /// <param name="authorityId">Id value for Authority that has the attribute values that should be deleted.</param>
        /// <returns>
        /// void
        /// </returns>
        public void DeleteAttributeValues(Int32 authorityId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("DeleteAttributeValues");
            commandBuilder.AddParameter(AuthorityData.AUTHORITY_ID, authorityId);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }


        /// <summary>
        /// Delete Organization from database table
        /// </summary>
        /// <param name="organizationId">Id value for Organization to be deleted.</param>
        /// <param name="modifiedBy">UserId that deletes the record.</param> 
        /// <returns>
        /// void
        /// </returns>
        public void DeleteOrganization(Int32 organizationId, Int32 modifiedBy)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("DeleteOrganization");
            commandBuilder.AddParameter(OrganizationData.ID, organizationId);
            commandBuilder.AddParameter(OrganizationData.MODIFIED_BY, modifiedBy);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Delete Person from database table
        /// </summary>
        /// <param name="personId">Id value for Person to be deleted.</param>
        /// <param name="modifiedBy">UserId that deletes the record.</param>
        /// <returns>
        /// void
        /// </returns>
        public void DeletePerson(Int32 personId, Int32 modifiedBy)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("DeletePerson");
            commandBuilder.AddParameter(PersonData.ID, personId);
            commandBuilder.AddParameter(PersonData.MODIFIED_BY, modifiedBy);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Delete phonenumbers from database table
        /// </summary>
        /// <param name="personId">Id value for Person that has the phonenumbers that should be deleted.</param>
        /// <param name="organizationId">Id value for Organization that has the phonenumbers that should be deleted.</param>
        /// <returns>
        /// void
        /// </returns>
        public void DeletePhoneNumber(Int32 personId, Int32 organizationId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("DeletePhoneNumber");
            commandBuilder.AddParameter(AddressData.PERSON_ID, personId);
            commandBuilder.AddParameter(AddressData.ORGANIZATION_ID, organizationId);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Delete Role from database table
        /// </summary>
        /// <param name="roleId">Id value for Role to be deleted.</param>
        /// <param name="modifiedBy">UserId that deletes the record.</param>
        /// <returns>
        /// void
        /// </returns>
        public void DeleteRole(Int32 roleId, Int32 modifiedBy)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("DeleteRole");
            commandBuilder.AddParameter(RoleData.ID, roleId);
            commandBuilder.AddParameter(RoleData.MODIFIED_BY, modifiedBy);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Deleted role members for roles that are not valid. 
        /// </summary>
        /// <returns>Number of deleted roll members.</returns>
        public Int32 DeleteRoleMembers()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("DeleteRoleMembers");
            lock (this)
            {
                CheckTransaction();
                return ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Delete User from database table
        /// </summary>
        /// <param name="userId">Id value for User to be deleted.</param>
        /// <param name="modifiedBy">UserId that deletes the record.</param>
        /// <returns>
        /// void
        /// </returns>
        public void DeleteUser(Int32 userId, Int32 modifiedBy)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("DeleteUser");
            commandBuilder.AddParameter(UserData.ID, userId);
            commandBuilder.AddParameter(UserData.MODIFIED_BY, modifiedBy);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Get DataReader with information about a addresses for a person 
        /// or an organization. Data is selected for specific language.
        /// </summary>
        /// <param name="personId">Id value for Person.</param>
        /// <param name="organizationId">Id value for Organization.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetAddresses(Int32 personId, Int32 organizationId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetAddressList");
            commandBuilder.AddParameter(AddressData.PERSON_ID, personId);
            commandBuilder.AddParameter(AddressData.ORGANIZATION_ID, organizationId);
            commandBuilder.AddParameter(AddressData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about all Address types.
        /// Data is selected for specific language.
        /// </summary>
        /// <param name="localeId">Id representing language, i.e. current Locale </param>
        /// <returns>A data reader</returns>
        public DataReader GetAddressTypes(Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetAddressTypes");
            commandBuilder.AddParameter(AddressData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about an application.
        /// </summary>
        /// <param name="applicationId">Id value for Application.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetApplicationById(Int32 applicationId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetApplication");
            commandBuilder.AddParameter(ApplicationData.ID, applicationId);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetRow(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about an application.
        /// </summary>
        /// <param name="applicationIdentifier">Identifier for Application.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetApplicationByIdentifier(String applicationIdentifier,
                                                     Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetApplicationByIdentifier");
            commandBuilder.AddParameter(ApplicationData.IDENTIFIER, applicationIdentifier);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetRow(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about all applications.
        /// </summary>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetApplications(Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetApplications");
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about action for an application.
        /// Data is selected for specific language.
        /// </summary>
        /// <param name="applicationActionId">Id value for ApplicationAction.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetApplicationAction(Int32 applicationActionId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetApplicationAction");
            commandBuilder.AddParameter(ApplicationActionData.ID, applicationActionId);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about actions for an application.
        /// Data is selected for specific language.
        /// </summary>
        /// <param name="applicationId">Id value for Application.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetApplicationActions(Int32? applicationId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetApplicationActions");
            if (applicationId.HasValue)
            {
                commandBuilder.AddParameter(ApplicationActionData.APPLICATION_ID, applicationId.Value);
            }
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about actions for an application.
        /// Data is selected for specific language.
        /// </summary>
        /// <param name="applicationIdList">List of Id value for application actions.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetApplicationActionsByIds(List<Int32> applicationIdList, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            // Use SqlCommandBuilder with SqlParameter
            commandBuilder = new SqlCommandBuilder("GetApplicationActionsByIds", true);
            commandBuilder.AddParameter("IdValueTable", applicationIdList);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);  
        }

        /// <summary>
        /// Get DataReader with information about versions for an application.
        /// Data is selected for specific language.
        /// </summary>
        /// <param name="applicationVersionId">Id value for ApplicationVersion.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetApplicationVersion(Int32 applicationVersionId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetApplicationVersion");
            commandBuilder.AddParameter(ApplicationVersionData.ID, applicationVersionId);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about versions for an application.
        /// Data is selected for specific language.
        /// </summary>
        /// <param name="applicationId">Id value for Application.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetApplicationVersionList(Int32? applicationId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetApplicationVersionList");
            if (applicationId.HasValue)
            {
                commandBuilder.AddParameter(ApplicationVersionData.APPLICATION_ID, applicationId.Value);
            }
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with all authorities that matches the search criteria.
        /// </summary>
        /// <param name="authorityIdentifier">Identifier for the authority.</param>
        /// <param name="applicationIdentifier">Identifier for the application.</param>
        /// <param name="authorityDataTypeIdentifier">Identifier for the authority data type.</param>
        /// <param name="authorityName">Authority name</param>
        /// <param name="localeId">Locale id.</param>
        /// <returns>DataReader after reading has been finished.</returns>
        public DataReader GetAuthoritiesBySearchCriteria(String authorityIdentifier,
                                                         String applicationIdentifier,
                                                         String authorityDataTypeIdentifier,
                                                         String authorityName,
                                                         Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetAuthoritiesBySearchCriteria");
            if (authorityIdentifier.IsNotEmpty())
            {
                commandBuilder.AddParameter(AuthorityData.AUTHORITY_IDENTITY, authorityIdentifier, 2);
            }
            if (applicationIdentifier.IsNotEmpty())
            {
                commandBuilder.AddParameter(ApplicationData.APPLICATION_IDENTITY, applicationIdentifier, 2);
            }
            if (authorityDataTypeIdentifier.IsNotEmpty())
            {
                commandBuilder.AddParameter(AuthorityDataType.AUTHORITY_DATA_TYPE_IDENTITY, authorityDataTypeIdentifier, 2);
            }
            if (authorityName.IsNotEmpty())
            {
                commandBuilder.AddParameter(AuthorityData.NAME, authorityName, 2);
            }
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }


        /// <summary>
        /// Get DataReader with information about authority.
        /// Data is selected for specific language.
        /// </summary>
        /// <param name="authorityId">Id value for Authority.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetAuthority(Int32 authorityId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetAuthority");
            commandBuilder.AddParameter(AuthorityData.ID, authorityId);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about authorities 
        /// regarding application actions.
        /// Data is selected for specific language.
        /// </summary>
        /// <param name="authorityId">Id value for Authority.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetAuthorityApplicationAction(Int32 authorityId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetAuthorityApplicationAction");
            commandBuilder.AddParameter(AuthorityData.AUTHORITY_ID, authorityId);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about authority attributes.
        /// Data is selected for specific language.
        /// </summary>
        /// <param name="authorityId">Id value for Authority.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetAuthorityAttributes(Int32? authorityId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetAuthorityAttributeList");
            if (authorityId.HasValue)
            {
                commandBuilder.AddParameter(AuthorityData.AUTHORITY_ID, authorityId.Value);
            }

            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about authority attribute type.
        /// </summary>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetAuthorityAttributeTypeList()
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetAuthorityAttributeTypeList");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about all Authority data types.
        /// </summary>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetAuthorityDataTypes()
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetAuthorityDataTypes");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about authority
        /// data types for specified application.
        /// </summary>
        /// <param name="applicationId">Application id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetAuthorityDataTypesByApplicationId(Int32? applicationId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetAuthorityDataTypesByApplicationId");
            if (applicationId.HasValue)
            {
                commandBuilder.AddParameter(AuthorityData.APPLICATION_ID, applicationId.Value);
            }
            return GetReader(commandBuilder);
        }
        
        /// <summary>
        /// Get DataReader with information on all countries.
        /// </summary>
        /// <returns>A data reader</returns>
        public DataReader GetCountries()
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetCountries");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get list of all languages
        /// </summary>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetLocales()
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetLocales");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about all Message types.
        /// Data is selected for specific language.
        /// </summary>
        /// <param name="localeId">Id representing language, i.e. current Locale </param>
        /// <returns>A data reader</returns>
        public DataReader GetMessageTypes(Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetMessageTypes");
            commandBuilder.AddParameter(AddressData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get locale by id
        /// </summary>
        /// <param name="localeId">Id value for requested locale.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetLocaleById(Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetLocaleById");
            commandBuilder.AddParameter("ID", localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about a organization.
        /// </summary>
        /// <param name="organizationId">Id value for Organization.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetOrganization(Int32 organizationId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetOrganization");
            commandBuilder.AddParameter(OrganizationData.ID, organizationId);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetRow(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about a organization.
        /// </summary>
        /// <param name="organizationCategoryId">Id value for OrganizationCategory.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetOrganizations(Int32? organizationCategoryId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetOrganizations");
            if (organizationCategoryId.HasValue)
            {
                commandBuilder.AddParameter(OrganizationData.ORGANIZATION_CATEGORY_ID, organizationCategoryId.Value);
            }
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with all organizations that matches the search criteria.
        /// </summary>
        /// <param name="name">Organization name search string.</param>
        /// <param name="shortName">Shortname search string.</param>
        /// <param name="categoryId">Organization category Id.</param>
        /// <param name="hasSpiecesCollection">Organization owns a spieces collection</param>
        /// <param name="localeId">Id representing language, i.e. current Locale </param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetOrganizationsBySearchCriteria(String name,
                                                   String shortName,
                                                   Int32? categoryId,
                                                   Boolean? hasSpiecesCollection,
                                                   Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetOrganizationsBySearchCriteria");
            if (name.IsNotEmpty())
            {
                commandBuilder.AddParameter(OrganizationData.NAME, name, 2);
            }
            if (shortName.IsNotEmpty())
            {
                commandBuilder.AddParameter(OrganizationData.SHORT_NAME, shortName, 2);
            }
            if (categoryId.HasValue)
            {
                commandBuilder.AddParameter(OrganizationData.ORGANIZATION_CATEGORY_ID, categoryId.Value);
            }
            if (hasSpiecesCollection.HasValue)
            {
                commandBuilder.AddParameter(OrganizationData.HAS_COLLECTION, hasSpiecesCollection.Value);
            }
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with all roles for an organization
        /// </summary>
        /// <param name="organizationId">Id value for Organization.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetOrganizationRoles(Int32 organizationId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetOrganizationRoles");
            commandBuilder.AddParameter(OrganizationData.ORGANIZATION_ID, organizationId);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about organization type.
        /// Data is selected for specific language.
        /// </summary>
        /// <param name="organizationCategoryId">Id value for OrganizationCategory.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetOrganizationCategory(Int32 organizationCategoryId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetOrganizationCategory");
            commandBuilder.AddParameter(OrganizationCategoryData.ID, organizationCategoryId);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }


        /// <summary>
        /// Get DataReader with information about organization type.
        /// Data is selected for specific language.
        /// </summary>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetOrganizationCategories(Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetOrganizationCategories");
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }


        /// <summary>
        /// Get DataReader with information about all Person Genders.
        /// Data is selected for specific language.
        /// </summary>
        /// <param name="localeId">Id representing language, i.e. current Locale </param>
        /// <returns></returns>
        public DataReader GetPersonGenders(Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetPersonGenders");
            commandBuilder.AddParameter(PersonGenderData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with all persons that have been modified after and before certain dates.
        /// </summary>
        /// <param name="modifiedFromDate">Date after which person object have been modified.</param>
        /// <param name="modifiedUntilDate">Date before which person object have been modified.</param>
        /// <param name="localeId">Id representing language, i.e. current Locale </param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetPersonsByModifiedDate(DateTime modifiedFromDate, 
                                                   DateTime modifiedUntilDate, 
                                                   Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetPersonsByModifiedDate");
            
            commandBuilder.AddParameter(PersonData.MODIFIED_FROM_DATE, modifiedFromDate);
            commandBuilder.AddParameter(PersonData.MODIFIED_UNTIL_DATE, modifiedUntilDate);
            
            commandBuilder.AddParameter(PersonData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with all persons that matches the search criteria.
        /// </summary>
        /// <param name="fullName">Full name search string.</param>
        /// <param name="firstName">First name search string.</param>
        /// <param name="lastName">Last name search string.</param>
        /// <param name="hasSpiecesCollection">Person owns a spieces collection</param>
        /// <param name="localeId">Id representing language, i.e. current Locale </param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetPersonsBySearchCriteria(String fullName,
                                                     String firstName,
                                                     String lastName,
                                                     Boolean? hasSpiecesCollection,
                                                     Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetPersonsBySearchCriteria");
            if (fullName.IsNotEmpty())
            {
                commandBuilder.AddParameter(PersonData.FULL_NAME, fullName, 2);
            }
            if (firstName.IsNotEmpty())
            {
                commandBuilder.AddParameter(PersonData.FIRST_NAME, firstName, 2);
            }
            if (lastName.IsNotEmpty())
            {
                commandBuilder.AddParameter(PersonData.LAST_NAME, lastName, 2);
            }
            if (hasSpiecesCollection.HasValue)
            {
                commandBuilder.AddParameter(PersonData.HAS_COLLECTION, hasSpiecesCollection.Value);
            }
            commandBuilder.AddParameter(PersonData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about all addresses for a person 
        /// or an organization. Data is selected for specific language.
        /// </summary>
        /// <param name="personId">Id value for Person.</param>
        /// <param name="organizationId">Id value for Organization.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetPhoneNumbers(Int32 personId, Int32 organizationId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetPhoneNumberList");
            commandBuilder.AddParameter(AddressData.PERSON_ID, personId);
            commandBuilder.AddParameter(AddressData.ORGANIZATION_ID, organizationId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about all Phone number types.
        /// Data is selected for specific language.
        /// </summary>
        /// <param name="localeId">Id representing language, i.e. current Locale </param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetPhoneNumberTypes(Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetPhoneNumberTypes");
            commandBuilder.AddParameter(PhoneNumberTypeData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about a person.
        /// </summary>
        /// <param name="personId">Id value for Person.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetPerson(Int32 personId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetPerson");
            commandBuilder.AddParameter(PersonData.ID, personId);
            commandBuilder.AddParameter(PersonData.LOCALE_ID, localeId);
            return GetRow(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about a role.
        /// </summary>
        /// <param name="roleId">Id value for Role.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetRole(Int32 roleId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetRole");
            commandBuilder.AddParameter(RoleData.ID, roleId);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetRow(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with all authorities for
        /// requested role and application.
        /// Data is selected for specific language.
        /// </summary>
        /// <param name="roleId">Id value for Role.</param>
        /// <param name="applicationIdentifier">Application identifier.</param>
        /// <param name="localeId">Id representing language, i.e. current Locale.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetAuthoritiesByRole(Int32? roleId, String applicationIdentifier, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetAuthorityList");
            if (roleId.HasValue)
            {
                commandBuilder.AddParameter(RoleData.ROLE_ID, roleId.Value);
            }

            commandBuilder.AddParameter(ApplicationData.APPLICATION_IDENTITY, applicationIdentifier);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with all rolemembers that matches the search criteria.
        /// </summary>
        /// <param name="roleIds">List of role ids.</param>
        /// <param name="userIds">List of user ids.</param>
        /// <param name="isActivated">Search for roles activated or not activated.</param>
        /// <param name="localeId">Id representing language, i.e. current Locale </param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetRoleMembersBySearchCriteria(List<Int32> roleIds,
                                                        List<Int32> userIds,
                                                        Boolean? isActivated,
                                                        Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetRoleMembersBySearchCriteria", true);
            commandBuilder.AddParameter("RoleIdTable", roleIds);
            commandBuilder.AddParameter("UserIdTable", userIds);
            if (isActivated.HasValue)
            {
                commandBuilder.AddParameter("IsActivated", isActivated.Value);
            }
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with all roles that matches the search criteria.
        /// </summary>
        /// <param name="roleName">Rolename search string.</param>
        /// <param name="shortName">Shortname search string.</param>
        /// <param name="identifier">Identifier search string</param>
        /// <param name="organizationId">Organization Id.</param>
        /// <param name="localeId">Id representing language, i.e. current Locale </param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetRolesBySearchCriteria(String roleName,
                                                   String shortName,
                                                   String identifier,
                                                   Int32? organizationId,
                                                   Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetRolesBySearchCriteria");
            if (roleName.IsNotEmpty())
            {
                commandBuilder.AddParameter(RoleData.ROLE_NAME, roleName, 2);
            }
            if (shortName.IsNotEmpty())
            {
                commandBuilder.AddParameter(RoleData.SHORT_NAME, shortName, 2);
            }
            if (identifier.IsNotEmpty())
            {
                commandBuilder.AddParameter(RoleData.IDENTIFIER, identifier, 2);
            }
            if (organizationId.HasValue)
            {
                commandBuilder.AddParameter(OrganizationData.ORGANIZATION_ID, organizationId.Value);
            }
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with all roles that are associated with a certain usergroup administration role.
        /// </summary>
        /// <param name="roleId">Id value for the user group admnistration role.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetRolesByUserGroupAdministrationRoleId(Int32 roleId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetRolesByUserGroupAdministrationRoleId");
            commandBuilder.AddParameter(RoleData.ID, roleId);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetRow(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with all roles that are associated with a certain usergroup administrator.
        /// </summary>
        /// <param name="userId">Id value for the user group administrator user.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetRolesByUserGroupAdministratorUserId(Int32 userId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetRolesByUserGroupAdministratorUserId");
            commandBuilder.AddParameter(RoleData.ID, userId);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetRow(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with all users with a role.
        /// </summary>
        /// <param name="roleId">Id value for Role.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetUsersByRole(Int32 roleId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetUsersByRole");
            commandBuilder.AddParameter(RoleData.ROLE_ID, roleId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with all users associated with a role that stil has to activate their role membership.
        /// </summary>
        /// <param name="roleId">Id value for Role.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetNonActivatedUsersByRole(Int32 roleId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetNonActivatedUsersByRole");
            commandBuilder.AddParameter(RoleData.ROLE_ID, roleId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about a user.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetUser(String userName)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetUser");
            commandBuilder.AddParameter(UserData.USER_NAME, userName);
            return GetRow(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about a user.
        /// </summary>
        /// <param name="userId">Id value for User.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetUser(Int32 userId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("GetUserById");
            commandBuilder.AddParameter(UserData.ID, userId);
            return GetRow(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with all roles for a user.
        /// </summary>
        /// <param name="userId">Id value for User.</param>
        /// <param name="applicationIdentity">Application identity.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetRolesByUser(Int32 userId, String applicationIdentity, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetUserRoles");
            commandBuilder.AddParameter(UserData.USER_ID, userId);
            if (applicationIdentity.IsNotEmpty())
            {
                commandBuilder.AddParameter(ApplicationData.APPLICATION_IDENTITY, applicationIdentity);
            }

            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);                    
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with all users that matches the search criteria.
        /// </summary>
        /// <param name="fullName">Full name search string.</param>
        /// <param name="firstName">First name search string.</param>
        /// <param name="lastName">Last name search string.</param>
        /// <param name="emailAddress">Email address search string.</param>
        /// <param name="city">City search string.</param>
        /// <param name="userType">Type of user</param>
        /// <param name="organizationId">Organization id</param>
        /// <param name="organizationCategoryId">Organization category id</param>
        /// <param name="applicationId">Application id.</param>
        /// <param name="applicationActionId">Application action id</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetUsersBySearchCriteria(String fullName,
                                                   String firstName,
                                                   String lastName,
                                                   String emailAddress,
                                                   String city,
                                                   String userType,
                                                   Int32? organizationId,
                                                   Int32? organizationCategoryId,
                                                   Int32? applicationId,
                                                   Int32? applicationActionId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetUsersBySearchCriteria");
            if (fullName.IsNotEmpty())
            {
                commandBuilder.AddParameter(UserData.FULL_NAME, fullName, 2);
            }
            if (firstName.IsNotEmpty())
            {
                commandBuilder.AddParameter(UserData.FIRST_NAME, firstName, 2);
            }
            if (lastName.IsNotEmpty())
            {
                commandBuilder.AddParameter(UserData.LAST_NAME, lastName, 2);
            }
            if (emailAddress.IsNotEmpty())
            {
                commandBuilder.AddParameter(EmailData.EMAIL_ADDRESS, emailAddress, 2);
            }
            if (city.IsNotEmpty())
            {
                commandBuilder.AddParameter(AddressData.CITY, city, 2);
            }
            if (userType.IsNotEmpty())
            {
                commandBuilder.AddParameter(UserData.USER_TYPE, userType);
            }
            if (organizationId.HasValue)
            {
                commandBuilder.AddParameter(OrganizationData.ORGANIZATION_ID, organizationId.Value);
            }
            if (organizationCategoryId.HasValue)
            {
                commandBuilder.AddParameter(OrganizationData.ORGANIZATION_CATEGORY_ID, organizationCategoryId.Value);
            }
            if (applicationId.HasValue)
            {
                commandBuilder.AddParameter(ApplicationData.APPLICATION_ID, applicationId.Value);
            }
            if (applicationActionId.HasValue)
            {
                commandBuilder.AddParameter(ApplicationActionData.APPLICATION_ACTION_ID, applicationActionId.Value);
            }
            return GetReader(commandBuilder);
        }


        /// <summary>
        /// Get DataReader with information about versions for an application.
        /// Data is selected for specific language.
        /// </summary>
        /// <param name="applicationIdentity">Application Identifier.</param>
        /// <param name="version">Version to check if vaild.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader IsApplicationVersionValid(String applicationIdentity, String version, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("IsApplicationVersionValid");
            commandBuilder.AddParameter(ApplicationData.APPLICATION_IDENTITY, applicationIdentity);
            commandBuilder.AddParameter(ApplicationVersionData.VERSION, version);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Test if an email address is unique.
        /// Do not include specified person or user if 
        /// parameter userId or personId has a value.
        /// It is assumed that max one of the parameters userId
        ///	and personId has a value in one request.
        /// </summary>
        /// <param name="emailAddress">Email address.</param>
        /// <param name="userId">Id of user to exclude when searching for duplicates.</param>
        /// <param name="personId">Id of person to exclude when searching for duplicates.</param>
        /// <returns>True if an email address is unique.</returns>
        public Boolean IsEmailAddressUnique(String emailAddress,
                                            Int32? userId,
                                            Int32? personId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("IsEmailAddressUnique");
            commandBuilder.AddParameter(EmailData.EMAIL_ADDRESS, emailAddress);
            if (userId.HasValue)
            {
                commandBuilder.AddParameter(EmailData.USER_ID, userId.Value);
            }
            if (personId.HasValue)
            {
                commandBuilder.AddParameter(EmailData.PERSON_ID, personId.Value);
            }
            return (0 == ExecuteScalar(commandBuilder));
        }

        /// <summary>
        /// Get information about if user exists in database or not.
        /// </summary>
        /// <param name="userName">UserName.</param>
        /// <returns>True, if user exists in database.</returns>
        public Boolean IsExistingUser(String userName)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("ExistsUser");
            commandBuilder.AddParameter(UserData.USER_NAME, userName);
            return (0 < ExecuteScalar(commandBuilder));
        }

        /// <summary>
        /// Remove an authority data type from application
        /// </summary>
        /// <param name="authorityDataTypeId">AuthorityDataType Id.</param>
        /// <param name="applicationId">Application Id.</param>
        /// <returns>
        /// void
        /// </returns>
        public void RemoveAuthorityDataTypeFromApplication(Int32 authorityDataTypeId, Int32 applicationId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("DeleteApplicationAuthorityDataType");
            commandBuilder.AddParameter(AuthorityDataType.AUTHORITYDATATYPE_ID, authorityDataTypeId);
            commandBuilder.AddParameter(ApplicationData.APPLICATION_ID, applicationId);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Deleted a rolemember from database. 
        /// </summary>
        /// <param name="roleId">Id for role.</param>
        /// <param name="userId">Id for user.</param>
        /// <returns>
        /// void
        /// </returns>
        public void RemoveUserFromRole(Int32 roleId, Int32 userId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("DeleteRoleMember");
            commandBuilder.AddParameter(RoleData.ROLE_ID, roleId);
            commandBuilder.AddParameter(UserData.USER_ID, userId);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Get DataReader with information about userName 
        /// if password is successfully reset
        /// </summary>
        /// <param name="emailAddress">EmailAddress.</param>
        /// <param name="newPassword">New password.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader ResetPassword(String emailAddress, String newPassword)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("ResetPassword");
            commandBuilder.AddParameter(EmailData.EMAIL_ADDRESS, emailAddress);
            commandBuilder.AddParameter(UserData.PASSWORD, newPassword);
            lock (this)
            {
                CheckTransaction();
                return GetRow(commandBuilder);
            }
        }

        /// <summary>
        /// Updates an application in the database. 
        /// </summary>
        /// <param name="Id">Application Id.</param>
        /// <param name="applicationIdentity">Application identity</param>
        /// <param name="name">Name.</param>
        /// <param name="shortName">Short name.</param>
        /// <param name="URL">Application URL.</param>
        /// <param name="description">Description written in Locale-language.</param>
        /// <param name="localeId">Locale expressed as LocaleId.</param>
        /// <param name="contactPersonId">Id for contact person (optional).</param>
        /// <param name="administrationRoleId">AdministrationRoleId</param>
        /// <param name="modifiedBy">UserId that modifies the record.</param>
        /// <param name="validFromDate">Date from this application is valid.</param>
        /// <param name="validToDate">Date that this application stops being valid.</param>
        /// <returns>void</returns>
        public void UpdateApplication(Int32 Id,
                                      String applicationIdentity,
                                      String name,
                                      String shortName,
                                      String URL,
                                      String description,
                                      Int32 localeId,
                                      Int32? contactPersonId,
                                      Int32? administrationRoleId, Int32 modifiedBy, 
                                      DateTime validFromDate, DateTime validToDate)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("UpdateApplication");
            commandBuilder.AddParameter(ApplicationData.ID, Id);
            commandBuilder.AddParameter(ApplicationData.APPLICATION_IDENTITY, applicationIdentity);
            commandBuilder.AddParameter(ApplicationData.NAME, name);
            commandBuilder.AddParameter(ApplicationData.SHORT_NAME, shortName);
            commandBuilder.AddParameter(ApplicationData.URL, URL);
            commandBuilder.AddParameter(ApplicationData.DESCRIPTION, description);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            if (contactPersonId.HasValue)
            {
                commandBuilder.AddParameter(ApplicationData.CONTACT_PERSON_ID, contactPersonId.Value);
            }
            if (administrationRoleId.HasValue)
            {
                commandBuilder.AddParameter(ApplicationData.ADMINISTRATION_ROLE_ID, administrationRoleId.Value);
            }
            commandBuilder.AddParameter(ApplicationData.MODIFIED_BY, modifiedBy);
            commandBuilder.AddParameter(ApplicationData.VALID_FROM_DATE, validFromDate);
            commandBuilder.AddParameter(ApplicationData.VALID_TO_DATE, validToDate);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Updates applicationaction in database. 
        /// </summary>
        /// <param name="id">ApplicationAction id.</param>
        /// <param name="actionIdentity">ActionIdentity.</param>
        /// <param name="name">Name</param>
        /// <param name="administrationRoleId">AdminstrationRoleId</param>
        /// <param name="description">Description.</param>
        /// <param name="localeId">LocaleId.</param>
        /// <param name="modifiedBy">UserId that modifies the record.</param>
        /// <param name="validFromDate">Date from this applicationaction is valid.</param>
        /// <param name="validToDate">Date that this applicationaction stops being valid.</param>
        /// <returns>void</returns>
        public void UpdateApplicationAction(Int32 id, String actionIdentity, String name,
                                Int32? administrationRoleId, String description, Int32 localeId,
                                Int32 modifiedBy, DateTime validFromDate, DateTime validToDate)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("UpdateApplicationAction");
            commandBuilder.AddParameter(ApplicationActionData.ID, id);
            commandBuilder.AddParameter(ApplicationActionData.ACTION_IDENTITY, actionIdentity);
            commandBuilder.AddParameter(ApplicationActionData.NAME, name);
            if (administrationRoleId.HasValue)
            {
                commandBuilder.AddParameter(ApplicationActionData.ADMINISTRATION_ROLE_ID, administrationRoleId.Value);
            }
            commandBuilder.AddParameter(ApplicationActionData.DESCRIPTION, description);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            commandBuilder.AddParameter(ApplicationActionData.MODIFIED_BY, modifiedBy);
            commandBuilder.AddParameter(ApplicationActionData.VALID_FROM_DATE, validFromDate);
            commandBuilder.AddParameter(ApplicationActionData.VALID_TO_DATE, validToDate);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Updates applicationversion in database. 
        /// </summary>
        /// <param name="id">ApplicationVersion id.</param>
        /// <param name="version">Version.</param>
        /// <param name="isRecommended">Is this the recommended version?</param>
        /// <param name="isValid">Is this a valid version </param>
        /// <param name="description">Description.</param>
        /// <param name="localeId">LocaleId.</param>
        /// <param name="modifiedBy">UserId that modifies the record.</param>
        /// <param name="validFromDate">Date from this applicationversion is valid.</param>
        /// <param name="validToDate">Date that this applicationversion stops being valid.</param>
        /// <returns>void</returns>
        public void UpdateApplicationVersion(Int32 id, String version, Boolean isRecommended,
                                Boolean isValid, String description, Int32 localeId,
                                Int32 modifiedBy, DateTime validFromDate, DateTime validToDate)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("UpdateApplicationVersion");
            commandBuilder.AddParameter(ApplicationVersionData.ID, id);
            commandBuilder.AddParameter(ApplicationVersionData.VERSION, version);
            commandBuilder.AddParameter(ApplicationVersionData.IS_RECOMMENDED, isRecommended);
            commandBuilder.AddParameter(ApplicationVersionData.IS_VALID, isValid);
            commandBuilder.AddParameter(ApplicationVersionData.DESCRIPTION, description);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            commandBuilder.AddParameter(ApplicationVersionData.MODIFIED_BY, modifiedBy);
            commandBuilder.AddParameter(ApplicationVersionData.VALID_FROM_DATE, validFromDate);
            commandBuilder.AddParameter(ApplicationVersionData.VALID_TO_DATE, validToDate);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Update an authority in database. 
        /// </summary>
        /// <param name="authorityId">Role id.</param>
        /// <param name="authorityIdentity">Authority Identity</param>
        /// <param name="name">Authority name.</param>
        /// <param name="showNonPublicData">ShowNonPublicData</param>
        /// <param name="maxProtectionLevel">MaxProtectionLevel</param>
        /// <param name="readPermission">ReadPermission</param>
        /// <param name="createPermission">CreatePermission</param>
        /// <param name="updatePermission">UpdatePermission</param>
        /// <param name="deletePermission">DeletePermission</param>
        /// <param name="administrationRoleId">AdministrationRoleId</param>
        /// <param name="description">Description.</param>
        /// <param name="obligation">Obligation</param>
        /// <param name="localeId">LocaleId.</param>
        /// <param name="modifiedBy">UserId for user creating the record.</param>
        /// <param name="validFromDate">Date from this authority is valid.</param>
        /// <param name="validToDate">Date that this authority stops being valid.</param>
        /// <returns>
        /// void
        /// </returns>
        public void UpdateAuthority(Int32 authorityId, String authorityIdentity, String name, Boolean showNonPublicData, Int32 maxProtectionLevel,
                                Boolean readPermission, Boolean createPermission, Boolean updatePermission, Boolean deletePermission,
                                Int32? administrationRoleId, String description, String obligation, Int32 localeId,
                                Int32 modifiedBy, DateTime validFromDate, DateTime validToDate)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("UpdateAuthority");
            commandBuilder.AddParameter(AuthorityData.ID, authorityId);
            commandBuilder.AddParameter(AuthorityData.AUTHORITY_IDENTITY, authorityIdentity);
            commandBuilder.AddParameter(AuthorityData.NAME, name);
            commandBuilder.AddParameter(AuthorityData.SHOW_NON_PUBLIC_DATA, showNonPublicData);
            commandBuilder.AddParameter(AuthorityData.MAX_PROTECTION_LEVEL, maxProtectionLevel);
            commandBuilder.AddParameter(AuthorityData.READ_PERMISSION, readPermission);
            commandBuilder.AddParameter(AuthorityData.CREATE_PERMISSION, createPermission);
            commandBuilder.AddParameter(AuthorityData.UPDATE_PERMISSION, updatePermission);
            commandBuilder.AddParameter(AuthorityData.DELETE_PERMISSION, deletePermission);
            if (administrationRoleId.HasValue)
            {
                commandBuilder.AddParameter(AuthorityData.ADMINISTRATION_ROLE_ID, administrationRoleId.Value);
            }
            commandBuilder.AddParameter(AuthorityData.DESCRIPTION, description);
            commandBuilder.AddParameter(AuthorityData.OBLIGATION, obligation);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            commandBuilder.AddParameter(AuthorityData.MODIFIED_BY, modifiedBy);
            commandBuilder.AddParameter(AuthorityData.VALID_FROM_DATE, validFromDate);
            commandBuilder.AddParameter(AuthorityData.VALID_TO_DATE, validToDate);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Updates an organization in the database. 
        /// </summary>
        /// <param name="Id">Organization Id.</param>
        /// <param name="name">Name.</param>
        /// <param name="shortName">ShortName.</param>
        /// <param name="organizationCategoryId">Id for OrganizationCategory</param>
        /// <param name="administrationRoleId">AdministrationRoleId</param>
        /// <param name="hasCollection">HasCollection - true if organization owns a collection</param>
        /// <param name="description">Description written in Locale-language.</param>
        /// <param name="localeId">Locale expressed as LocaleId.</param>
        /// <param name="modifiedBy">UserId that modifies the record.</param>
        /// <param name="validFromDate">Date from this organization is valid.</param>
        /// <param name="validToDate">Date that this organization stops being valid.</param>
        /// <returns>void</returns>
        public void UpdateOrganization(Int32 Id, String name, String shortName, Int32 organizationCategoryId, 
                                       Int32? administrationRoleId, Boolean hasCollection, String description, Int32 localeId, 
                                       Int32 modifiedBy, DateTime validFromDate, DateTime validToDate)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("UpdateOrganization");
            commandBuilder.AddParameter(OrganizationData.ID, Id);
            commandBuilder.AddParameter(OrganizationData.NAME, name);
            commandBuilder.AddParameter(OrganizationData.SHORT_NAME, shortName);
            commandBuilder.AddParameter(OrganizationData.ORGANIZATION_CATEGORY_ID, organizationCategoryId);
            if (administrationRoleId.HasValue)
            {
                commandBuilder.AddParameter(OrganizationData.ADMINISTRATION_ROLE_ID, administrationRoleId.Value);
            }
            commandBuilder.AddParameter(OrganizationData.HAS_COLLECTION, hasCollection);
            commandBuilder.AddParameter(OrganizationData.DESCRIPTION, description);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            commandBuilder.AddParameter(OrganizationData.MODIFIED_BY, modifiedBy);
            commandBuilder.AddParameter(OrganizationData.VALID_FROM_DATE, validFromDate);
            commandBuilder.AddParameter(OrganizationData.VALID_TO_DATE, validToDate);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Updates an organization category in the database. 
        /// </summary>
        /// <param name="id">Organization category Id.</param>
        /// <param name="name">Name.</param>
        /// <param name="administrationRoleId">AdministrationRoleId</param>
        /// <param name="description">Description written in Locale-language.</param>
        /// <param name="localeId">Locale expressed as LocaleId.</param>
        /// <param name="modifiedBy">UserId that modifies the record.</param>
        /// <returns>void</returns>
        public void UpdateOrganizationCategory(Int32 id, String name, String description,
                                               Int32? administrationRoleId, Int32 modifiedBy, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("UpdateOrganizationCategory");
            commandBuilder.AddParameter(OrganizationCategoryData.ID, id);
            commandBuilder.AddParameter(OrganizationCategoryData.ORGANIZATION_CATEGORY_NAME, name);
            if (administrationRoleId.HasValue)
            {
                commandBuilder.AddParameter(OrganizationCategoryData.ADMINISTRATION_ROLE_ID, administrationRoleId.Value);
            }
            commandBuilder.AddParameter(OrganizationCategoryData.ORGANIZATION_CATEGORY_DESCRIPTION, description);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            commandBuilder.AddParameter(OrganizationData.MODIFIED_BY, modifiedBy);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Updates an role in the database. 
        /// </summary>
        /// <param name="Id">Role Id.</param>
        /// <param name="roleName">RoleName.</param>
        /// <param name="shortName">ShortName.</param>
        /// <param name="description">Description written in Locale-language.</param>
        /// <param name="localeId">Locale expressed as LocaleId.</param>        
        /// <param name="administrationRoleId">AdministrationRoleId</param>
        /// <param name="userAdministrationRoleId">UserAdministrationRoleId</param>
        /// <param name="organizationId">OrganizationId</param>
        /// <param name="modifiedBy">UserId that modifies the record.</param>
        /// <param name="validFromDate">Date from this role is valid.</param>
        /// <param name="validToDate">Date that this role stops being valid.</param>
        /// <param name="identifier">Identifier.</param>
        /// <param name="isActivationRequired">Is activation required.</param>
        /// <param name="messageTypeId">Message type id.</param>
        /// <returns>void</returns>
        public void UpdateRole(Int32 Id, String roleName, String shortName, String description, Int32 localeId,
                               Int32? administrationRoleId, Int32? userAdministrationRoleId, Int32? organizationId, 
                               Int32 modifiedBy, DateTime validFromDate, DateTime validToDate,
                               String identifier, Boolean isActivationRequired, Int32 messageTypeId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("UpdateRole");
            commandBuilder.AddParameter(RoleData.ID, Id);
            commandBuilder.AddParameter(RoleData.ROLE_NAME, roleName);
            commandBuilder.AddParameter(RoleData.SHORT_NAME, shortName);
            commandBuilder.AddParameter(RoleData.DESCRIPTION, description);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            if (administrationRoleId.HasValue)
            {
                commandBuilder.AddParameter(RoleData.ADMINISTRATION_ROLE_ID, administrationRoleId.Value);
            }
            if (userAdministrationRoleId.HasValue)
            {
                commandBuilder.AddParameter(RoleData.USER_ADMINISTRATION_ROLE_ID, userAdministrationRoleId.Value);
            }
            if (organizationId.HasValue)
            {
                commandBuilder.AddParameter(RoleData.ORGANIZATION_ID, organizationId.Value);
            }
            commandBuilder.AddParameter(RoleData.MODIFIED_BY, modifiedBy);
            commandBuilder.AddParameter(RoleData.VALID_FROM_DATE, validFromDate);
            commandBuilder.AddParameter(RoleData.VALID_TO_DATE, validToDate);
            commandBuilder.AddParameter(RoleData.IDENTIFIER, identifier);
            commandBuilder.AddParameter(RoleData.IS_ACTIVATION_REQUIRED, isActivationRequired);
            commandBuilder.AddParameter(RoleData.MESSAGE_TYPE_ID, messageTypeId);

            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Updates a user in the database. 
        /// </summary>
        /// <param name="userName">UserName.</param>
        /// <param name="personId">Key to record in Person that this User is related to.</param>
        /// <param name="applicationId">ApplicationId - if User is an application.</param>
        /// <param name="GUID">GUID.</param>
        /// <param name="emailAddress">Users email address.</param>
        /// <param name="showEmail">Show emailaddress or not.</param>
        /// <param name="authenticationType">AuthenticationType.</param>
        /// <param name="accountActivated">Is the account activated.</param>
        /// <param name="administrationRoleId">AdministrationRoleId</param>
        /// <param name="modifiedBy">UserId that creates the record.</param>
        /// <param name="validFromDate">Date from this user is valid.</param>
        /// <param name="validToDate">Date that this users stops being valid.</param>
        /// <returns>
        /// Returns userId for the created record
        /// </returns>
        public Int32 UpdateUser(String userName, Int32? personId, Int32? applicationId, String GUID, String emailAddress,
                                Boolean showEmail, Int32 authenticationType, Boolean accountActivated, 
                                Int32? administrationRoleId, Int32 modifiedBy, DateTime validFromDate, DateTime validToDate)
        {
            Int32 userId = Int32.MinValue;
            String _defaultNull = "null";
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("UpdateUser");
            commandBuilder.AddParameter(UserData.USER_NAME, userName);
            if (personId.HasValue)
            {
                commandBuilder.AddParameter(UserData.PERSON_ID, personId.Value);
            }

            if (applicationId.HasValue)
            {
                commandBuilder.AddParameter(UserData.APPLICATION_ID, applicationId.Value);
            }

            commandBuilder.AddParameter(EmailData.EMAIL_ADDRESS, emailAddress);
            commandBuilder.AddParameter(EmailData.SHOW_EMAIL, showEmail);
            commandBuilder.AddParameter(UserData.AUTHENTICATION_TYPE, authenticationType, _defaultNull);
            commandBuilder.AddParameter(UserData.ACCOUNT_ACTIVATED, accountActivated);
            if (administrationRoleId.HasValue)
            {
                commandBuilder.AddParameter(UserData.ADMINISTRATION_ROLE_ID, administrationRoleId.Value);
            }

            commandBuilder.AddParameter(UserData.MODIFIED_BY, modifiedBy);
            commandBuilder.AddParameter(UserData.VALID_FROM_DATE, validFromDate, _defaultNull);
            commandBuilder.AddParameter(UserData.VALID_TO_DATE, validToDate, _defaultNull);
            lock (this)
            {
                CheckTransaction();
                using (DataReader dataReader = GetRow(commandBuilder))
                {
                    if (dataReader.Read())
                    {
                        userId = dataReader.GetInt32(UserData.ID);
                    }
                }
            }

            return userId;
        }

        /// <summary>
        /// Updates a user password
        /// </summary>
        /// <param name="userName">UserName.</param>
        /// <param name="newPassword">New password.</param>
        /// <returns>Boolean - 'true' if password is succesfully changed
        ///                  - 'false' if password update fails.
        /// </returns>
        public Boolean UpdateUserPassword(String userName, String newPassword)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("UpdateUserPassword");
            commandBuilder.AddParameter(UserData.USER_NAME, userName);
            commandBuilder.AddParameter(UserData.PASSWORD, newPassword);
            lock (this)
            {
                CheckTransaction();
                return (ExecuteCommand(commandBuilder) > 0);
            }
        }

        /// <summary>
        /// Updates a person in the database. 
        /// </summary>
        /// <param name="Id">Person Id.</param>
        /// <param name="firstName">FirstName.</param>
        /// <param name="middleName">MiddleName.</param>
        /// <param name="lastName">LastName.</param>
        /// <param name="genderId">GenderId.</param>
        /// <param name="emailAddress">Emailaddress.</param>
        /// <param name="showEmail">Show emailaddress or not.</param>
        /// <param name="showAddresses">Show addresses or not.</param>
        /// <param name="showPhoneNumbers">Show phonenumbers or not.</param>
        /// <param name="birthYear">BirthYear.</param>
        /// <param name="deathYear">DeathYear.</param>
        /// <param name="administrationRoleId">AdministrationRoleId</param>
        /// <param name="hasCollection">HasCollection</param>
        /// <param name="localeId">Locale expressed as id.</param>
        /// <param name="taxonNameTypeId">TaxonNameTypeId.</param>
        /// <param name="URL">Persons URL.</param>
        /// <param name="presentation">Presentation written in Locale-language.</param>
        /// <param name="showPresentation">Show presentation or not.</param>
        /// <param name="showPersonalInformation">Show personal information or not.</param>
        /// <param name="userId">Id for User record that this person is connected to.</param>
        /// <param name="modifiedBy">UserId that modifies the record.</param>
        /// <returns>void</returns>
        public void UpdatePerson(Int32 Id, String firstName, String middleName, String lastName, 
                                Int32 genderId, String emailAddress, Boolean showEmail, Boolean showAddresses,
                                Boolean showPhoneNumbers, DateTime? birthYear, DateTime? deathYear,
                                Int32? administrationRoleId, Boolean hasCollection, Int32 localeId, Int32 taxonNameTypeId, 
                                String URL, String presentation, Boolean showPresentation, Boolean showPersonalInformation, Int32? userId, Int32 modifiedBy)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("UpdatePerson");
            commandBuilder.AddParameter(PersonData.ID, Id);
            commandBuilder.AddParameter(PersonData.FIRST_NAME, firstName);
            commandBuilder.AddParameter(PersonData.MIDDLE_NAME, middleName);
            commandBuilder.AddParameter(PersonData.LAST_NAME, lastName);
            commandBuilder.AddParameter(PersonData.GENDER_ID, genderId);
            commandBuilder.AddParameter(EmailData.EMAIL_ADDRESS, emailAddress);
            commandBuilder.AddParameter(EmailData.SHOW_EMAIL, showEmail);
            commandBuilder.AddParameter(PersonData.SHOW_ADDRESSES, showAddresses);
            commandBuilder.AddParameter(PersonData.SHOW_PHONENUMBERS, showPhoneNumbers);
            if (birthYear.HasValue)
            {
                commandBuilder.AddParameter(PersonData.BIRTH_YEAR, birthYear.Value);
            }
            if (deathYear.HasValue)
            {
                commandBuilder.AddParameter(PersonData.DEATH_YEAR, deathYear.Value);
            }
            if (administrationRoleId.HasValue)
            {
                commandBuilder.AddParameter(PersonData.ADMINISTRATION_ROLE_ID, administrationRoleId.Value);
            }
            commandBuilder.AddParameter(PersonData.HAS_COLLECTION, hasCollection);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            commandBuilder.AddParameter(PersonData.TAXON_NAME_TYPE_ID, taxonNameTypeId);
            commandBuilder.AddParameter(PersonData.URL, URL);
            commandBuilder.AddParameter(PersonData.PRESENTATION, presentation);
            commandBuilder.AddParameter(PersonData.SHOW_PRESENTATION, showPresentation);
            commandBuilder.AddParameter(PersonData.SHOW_PERSONALINFORMATION, showPersonalInformation);
            if (userId.HasValue)
            {
                commandBuilder.AddParameter(PersonData.USER_ID, userId.Value);
            }
            commandBuilder.AddParameter(PersonData.MODIFIED_BY, modifiedBy);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Updates a user password.
        /// Used by users with administrator role.
        /// </summary>
        /// <param name="userId">UserId.</param>
        /// <param name="newPassword">New password.</param>
        /// <returns>
        /// Boolean - 'true' if password is succesfully changed
        ///         - 'false' if password update fails.
        /// </returns>
        public Boolean UserAdminSetPassword(Int32 userId, String newPassword)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("UserAdminSetPassword");
            commandBuilder.AddParameter(UserData.USER_ID, userId);
            commandBuilder.AddParameter(UserData.PASSWORD, newPassword);
            lock (this)
            {
                CheckTransaction();
                return (0 < ExecuteScalar(commandBuilder));
            }
        }

        /// <summary>
        /// User login service
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="isActivationRequired">
        /// Flag that indicates that the user account must
        /// be activated before login can succeed.
        /// </param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader Login(String userName,
                                String password,
                                Boolean isActivationRequired)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("Login");
            commandBuilder.AddParameter(UserData.USER_NAME, userName);
            commandBuilder.AddParameter(UserData.PASSWORD, password);
            commandBuilder.AddParameter(UserData.IS_ACTIVATION_REQUIRED, isActivationRequired);
            return GetRow(commandBuilder);
        }
    }
}

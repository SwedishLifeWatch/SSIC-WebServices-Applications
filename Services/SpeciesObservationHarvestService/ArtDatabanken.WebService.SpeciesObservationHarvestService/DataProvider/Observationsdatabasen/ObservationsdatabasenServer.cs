﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.Database;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Observationsdatabasen
{
    /// <summary>
    /// Handle species observations for observationsdatabasen server.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class ObservationsdatabasenServer : WebServiceDataServer
    {
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

            // Set timeout to 2 mins.
            CommandTimeout = 1200;
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
                case "ARTFAKTA-DEV": // Team Species Fact test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHJAJANJFJAAAOPFIECNCBGGHKGHPPBKNAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHBGIPLOJAPMOMHIKBJPCHDBIHMBOMJFIKIAAAAAADHKPJKEFFHHLCMLHMACNKEDPNIJNOEIPHFBJOICDCPICONIJAOLGEDLDBGAMMILMEHJMBFAINBDFJGFAIJBCKIDEKHBEDMGFMDPFCJNPEHFCMIINFIAADEKKEPNLILIECPKGMIJNBADAAGGNIKGMDIHLAOAHIPNHJOBGBNMPPBFBCAFKIOKNLHIGNKDKJNIPEFJFHADOOFIGHIFKBEGHCHFBIIJJOHCOEGBCIBOABNHFNLDECMJBKOAJHIOCFMCGDPJPBJEFHEMLAEGNHAOJAGLGECLPAGBHKJFIPCDOLAJDBBFDKEAOELGABGJCEGAABEKDACPOIJGLBJKOBEAAAAAAEGNKMCEGHMBACJPCENNAPBGHHFLLBLCEOLBBDJBG";
                    break;

                case "ARTSERVICE2-1": // New production web service server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANNALEBNABJNDLNPAEJNHGIAEGBGDGICIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPNKFCPKICCMCMJMPPIBKIGJFALLNGBCMLIAAAAAAKFBCKCINGCOMIJBLNDMPJCHFHMIFDDKNADBOHALEBEGLOGPHGBCDGMIAGGMNLNMJOLFGIMHJHHIMILEGABBCOELNGOHFIOIPPNIGNDENNGOCLJFDNIPMBNDOFOEDMJEJAFAHABHPGKMEEJIPGHBOJOCIHLJLBANKPMJGPEDEBLMOBKHFPCFIFMNNCJBALFIIBJKEJDEHMLOKPJODDCABAFLCBEAGHAGDHMPIOMPOEDKNKKJMCAJFFJNLJINLGPJCOMAAHDINEJPGHOPEPHDCOKAGNLMOINONCKDJDEGEDAHKLAMCDBPAJBNLILGOCKDMJMKMJPJBHCDHDGHLABIHGGOCGJIJLCGNJPKBAPBIBEIHKEPBBEAAAAAAEBBEBAALBBLNLLEDAABPNLBPLIKDBGCEGNNNBGHI";
                    break;

                case "ARTSERVICE2-2": // New production web service server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAONEEGAFCOFJGEELIEFKOONPKHCGJCFGMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADHOHJJCMINGOKPFCJFDIHDDIFAFPNLBPLIAAAAAAAJHOHLDMBEEFEPFEKAGJFMKCPFADJOHEPCDGNKCEPCHCBPJPHIHLPBEIBOJJOMHABPDAHEENBHCIIAHGPCCGJLHLHEFPBMMMEPDNMKEKLPAMNPAEHOCHBJHKHGKEEBMCDEPDCOIJMNHGDLBAPECNCEKPPMBHGGMGLNKLFMOPIEPHJALIGNPFCCOIMMEJBFNGMCPJMNGDALKLEJCLOIIOCACNPFKMDDEDALFDMAGMCOBNHEFHIHGMAALEGELHDIJEHOAPFIJJAKJGBIFFJILOMBAKKKDONGJOPGGEBFEOJKMDBPINCHLBIKDFJHMHJAMOKDLNLHKPLBMIDEAHFBHNJKODLBFFCKLLLKOPAGDPOKEHOHHKBEAAAAAABGAMFOGKOFKPLHOPKBPBKNPMMHDBLKGLMDGBFMAL";
                    break;

                case "MONESES-DEV": // Test Web Server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEAFPECBBEFEHIKEGLFFDMHECEONJDGANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALAHAENPIKAAGJMJEJGPEBMHJENLNBEHAAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFHEOAPJDDNGEKCINKHJDODPMOCHALJPLLIAAAAAADJHCFHJPFPCLDLHLNGFFDEBKIEJGMOIAEKAFMFGKDCNNIDMDPDEJCGADDMDGLLKFBAGCJMDAJAAAHFMPEOJEPDPKNBPNHNANKOOLEGIALNDDJMFCEPOOPMKFFCMFJMNMKNNIFFFKNECAIHCEBIEIOOLJJGCLKBEHPHMBELPOPINDKFJJIIJAHDHECLFHJEKBAHJIFEDOCKFGBKLPLBKAHJFICAGPLKJBLDIPFFDPJFPOHKJNCOGNMFFGHKCFHEAFLBBFFCBDBMCNBAICFMEBFPAPEDJDDBHFOAKPAPCIEKBIAIEIGMONENMNHHNBACHGGJGOPCJHCDGDLJDOMPFJIPDJJMKDJEECMBMGCLKPGOBCNHAEBEAAAAAABODIOFHCOPJNHIABEBNKPEDIONIENFBFCNCKMFLM";
                    break;

                case "MONESES-ST": // System test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAKINDHEBALJGMJPGENJKPLEKBPINFPIJHAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAAOHPAIIEFIJFNADDGIMKAEIABBIHIMALKIAAAAAAKGFHDAOLOPEJLHAPMFBHADJFMJGPIFGHACFNIKOABBAECIGGHPICPBNCMMNAHMDABLGEBODHIJPLEMNCKOPGGKMLMMIAGLONIKLPPAINENBOJDKBNFCLJHHDFFDJKFDNGLDLAAFKKIFEGCIJHJHKGICNFMDDJOOMNCHJIJFKAEHAJAKLINCFMNFGNOKFNAAJCFGFHAMAIKGHFANKIGNIIBONKODJFLPBENHCPNIPIOKLFMLDDKNFBEEDOGIIBFDPGBBMMCPAJFEOCHPMKNOKFOCFELBMLNGCDFBPDOGOLAEHBMIGNELLPGDCMAIAPLJNPOHMMMKPIKMCMFKFBEAAAAAACLBIALMNBPFGPBMNNOHGJEAIAGBMJFGPEEMKKGPO";
                    break;

                case "SILURUS2-1": // Production Web Server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIDGFKMFBDIMAPEELJPFDBGPLIAHNCJMKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACPNAMLGLIEDCOOMPKEJBOPIDEEHNDMOLAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAMKPOBONDKOHOLAEMNMNAGPECBANCBOBGLIAAAAAAKOBAABKFOKNCDDGEEGAKAODEBLCAIJKKAFHKOMMAKLCGMOFHNBAEFAEJDPNBICIBOPGKBOKPEIJPBABIDPOOECIPPGNNNGONBKNIHJHEJHOFIAMKFFJEDCIMOGGHGOIFOGFCPIBBNPBFCKEDKGKELHNJIHDHCILKEMJKJBDDJFGHKOGBMIHHINHAEGPLAPNNPIJKFGHDPBCDLFHLPHJIOPMLJPDOIPHANJHPOGLCJLPPNDGIHDCHFPDKMNHDIBNICFBDDDPPCPPBIACEEMKKBFDPEHFMJIDHOENKLLDMNFIHONNIPDEEFOGNPLBEAFNIJFBHPGDDNFLAHHADAKGJIBFIMGEMLPKMOCGKPDECJCEGJFLABEAAAAAAIHBFLKHHPBOMHNHEDNLNOJNDOEPIAIPCKGNNIFHN";
                    break;

                case "SILURUS2-2": // Production Web Server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAODPAFKAIHGMAPDEOIPNDNGAIEJHDINBGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADKHBHJPGALFMLGHEGBEIENNJEMNOJIFKAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPJMJBFPEMKAAEABGPCKKOFKBNNEDFCLILIAAAAAADDFBFILHAFHJIFAEANDMHAGIHFKDAACMIIMNLIDHMFPKKGGAOPMFMDGODCDIBLDCAAHGABIBBPABFHKKBCOIHLDLKHJJDAALFIJGJPNODEHJKLKMEEFCFLJDCFFGDCPKKKNMCJPMDLMOAGJCOFJJAKMCFNLCBEEKOPIMNPFHAPNNLMMMJJDIMJCJLAKDKCDMGKOBEJLNJPFJHMILFLPPCIEKNFCNACLIOAHNNODGEBKKPIONJOAEIGECBJFOMONAGPMDKBJGGKJFEEOBHAAMDGPMANBLGDCKCPBCOFIHCNNIACCLNFGLOGMKOBDCHNDJOLEHIMCILGBGPMNGECOIBFFJFJGKLOPMNBPINMBFDABPOLFMBEAAAAAACOOMFKMDIPPBACNLFIFENCIPKGJAPCLPIOCDPJJP";
                    break;

                case "SLU011837": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPFOBOEOIJOPHGCEFJGCCBMFMHLOBJJLJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACEHMMCMCDDIICAAJGDFEPCLBPDFMHEPKAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKFCGNJLODFAICNKOPMKPGGIAHCEOCCKNLIAAAAAAOCLCPDHLGONDALDBGBOFKMFMJHOLKIGMBEBPBGBANECJDMMMHLCOPIDOONPKMMHJBOLPHKPJMKIMFPPJADLBKDMEAJNNFOOIBNPGGDMGDFCMJGMFMBOOGHNNOELACBFDADAFKMGFOLFALDKBBPNCEINLOILNNLAMKFJGPBJJGBPDLJKHDJFIJKLAEHIILBDCKOMENDCNLDKDMDLDCBFDDMNANKMEGAPKFHOFPEFMECDPOALFKJLCHLJEIGBHAEGLBCMIEGEFKCDBODOEPPMCIHGIFKOGDKAFKNCIOFOIAJKCCLKACBAIAHMPHHKAMHHDPIFFHLKFJKMCJCGJDAKKBNLLEMJFPAHPBCEAKCFALJEHEBDLBEAAAAAACCHIMNKANLNFFLPMIHEOFJCOBDMKMOPPILCONOCM";
                    break;

                case "SLU002760": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAOIKAMFPLIBLJKHEBKLMNEPHLFFHCEIBDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFDEIIOKLAFPAGBJEHBJHNDJJMFDCAHCFAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABMGDJLHLJKABHGIJIBNLEOFDMCCEHPCBLIAAAAAAAHDGPMHJBGOEPDHJDLCIMMOIHNDFKCKAHBKCBOHOHOEICKALGGKEKHGAHCBHLGBDOHBBBGAGOJKLJGBKOGHLAJGMNGMPEMKGGLEDJBJMOCBFPLLBJBOBEKGNLLLPBIDLIDCGBPKDNDMCLCHOEEJIMFMJIENJKJKIDHJDPNDBAADHDGMHPJIHCHBHHBLLEKDOHJBBCDAKGIFNIKGHKOMKBNJKCJNBDDPDGAOBKECMLBJNPMHHJBGFNNJAGGFHNOPMBFCEOHFHBADAGKHFBGDFOFBKJBBGKKANHKBIDMNCBMGCFCEHHOMLOJPICNJNLLBLJNJEFMIOGMIJCDKAICLENMAMKGILBEEFOLMFLNCOKGBJOPFJBEAAAAAABAADDGKFOLBDJPINAMJCBMHIHJGNMMCPHFJADICJ";
                    break;

                case "MATLOU8470WW7": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALHCOFDGPCNONAMEDJDCIAMKCOKLCFBPFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAABJAJCJFCCBECGOELEJMHEJHIKCECLCIHAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKGDALCOFGONBPCAJNLEHHMABGFMBMIFFLIAAAAAACECPFONLEJEOKFDGCDMMIGMHICGOIPGKJNNHAMOPJKOMCIHHPADPHKECONBGHDENEHFLDILBCICAOGDIAEPELJGMJNACAABIGBKJELJAEEBIMCIOHPDBNFMNDEHOPMJFCFOMOKEPJMEHDPAMJMLLHOEBAKMOHGHKKBALPKDJJHCBJOMJCOBDCKFNOCEKDPMCGGDNHKOAMODLOJAKECAKGOFBNBAAIMCDCGPCJIFGEJGBCAEFFHDDHHPAFMEODOCFEMDKOHJPOGKJEIDMGCCKCFCGNANAKMDNHOANJFOLDCEBNHNHMLKGNOPCHNCIINGJCAELCLPCFMOPBFPIDJMHAEFPCGAMKFOMLKJFMAJLBAJPFENLBEAAAAAAOAPPNKKPGLKGNOFKKJFGOMDGOLLKCBFFFHMINFAG";
                    break;

                case "ARTDATA-TFS": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAENFHPOKFCMDLPPEIKKMGLNPOKIBMBJBEAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHGAHHKMHEIFNBEABNMGNAOELGGFDKEMCAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPLFKKBBOCLPPCNPJEMJJBKDHABLOCMAILIAAAAAAJKADIPCNDDCHNJNOLNINPJOHPPLAEMLGCNIKPKIPNKOJCIPOAJCHGLFLOLPDPBNEBMDENOHLNALPPGJGKALCJHBHEFBEAOBGDALBPPIPKDBAGILBAJNCFCEKKFPKDENBLEKOGLDGNEDAMCHBDHGAFOCNCKJCAPOLDPIINLHKPGOGNHAJHCFFCHKEDMLPFNIFPPBJFNDEEOBLPGKCBHOCMMDMKDGDJHGOCAGOPJNFEEMADNAFNLFCAAEMCHELIGKMODPLALPHEBPPLJBPNNJKGINBIIHFFLFHFKDJMMNLMHPGPLOPCEHDFBEIJEHAKALOOPKONEIPBGCIDALJILHFHIMPINFMEFOPKFECNKHGHGKFIIJDBEAAAAAAFCKJPMBFOOPOPPKAENCADEJDKPNACCFBFJCNCPML";
                    break;

                case "SEA0105WTRD2": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABCDHLPKLHGBGOOEJKJIFHDOAFNEEFMNKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAGPPIFCKJIHBLLELECGIGHHMGNHMPPOMNAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHBGIOICKDOALBMGNHFHCGKBCGFNMOMLKLIAAAAAAKHBFDAOLDEFJAIIELEHHCHIIDOMDOICFECOMNNNEINMCKDABHHMICEPPLIIDIJJLMNBFFMKPBKADLFFPDLJHIMPGILOJKFFHFAPDIJBOGKLPNMNAEPLHGPIKOFEANIGIFPMLNDFHPGFPFKLJLFHIKEPKNGJGDFLABLGJBDMJCMFCCAFCLMEBPNECHFCBGEJBCECPDAMOIIBKJILMOGKANDNNIFJAKODINBMACEBGIAHOIJIKPLHODBJJLKBKIICPBGNIHKHPLJNJEOPOPELOBLMCGCCLJLMOINEGFOFBECLBIFLNAKMJDJJBCJHPJNCIELALOMLLMFCNEDDGNPADEFHNJFHIBOPKHMGLDIBMFGEHBCKIBEAAAAAADOFABBOEDFFNBGHOAKMGBNLPGBBCDGADIEPLBJMH";
                    break;

                case "SLU011896": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAFNEPGOOEOHKKGCECIICFNMJLKFBFPGCCAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAMCGFNOMLCECIMAKIPDIKAPKMAEIFKEMEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAAEKDEIJOFKJIFBMMFMOPGGIBFDJJLPOFLIAAAAAAECHGANALLCDLKGCGAKJIGLAJEAAMGIPKEGPEHDGBNODIPKNIOJPMJPJCPGCKLNJHKAGENFCOGHODLPGAKOJNEAFFBDAKOKMAEDBMMMHBBPIKFEDJFBDCPLIJEBNIHKOHFNLHGCKLDDEOMBLKAPHCMHMMMEBOKDHOLBLFOGCOOEEFLANMFINPDLCGBBAIFNMOOJDNKBNEAMDEMGOJIEGBMEAHDNJMKEDOIGDMMFONALAGMDMDBNGFOKPOKFOIGIPHANLMIIGKFONPCEGFGAKPEFBDMGJPCPJHHAGMBGGAIEFKKPJKEEDGFBOCBNBAPBGJJENGMLMBKDJDKNDHJDMCANJMELGOKIBBJHBCJEHAPFMLHBEFBEAAAAAAGDDLJGCFMMLFMDOLNPLGIIAHNECCNLNFGKJFBJFP";
                    break;

                case "SLW-DEV": // Team Two Blueberries test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPIDAADADLFOMHGCDPBBJAKMLIMMGCBHFAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAIGABGDMFBMLFGIJJINBLNAOPNCIBOJGLKIAAAAAAOCBGMLKLHBPCLGIAEENBFIHHKLOOKNMKHLHMDPJBAOIIAGLJLCDLLFEKNGPHOLLHMCEJHMKDAMAJHDDPOLLMKPGGELGMFHLKPFAIMDHAAOADFJCDNFBDOLBOGJMBKDEINHBLPCHOPKNJIPJNIFDAMFPNPABFILGFAJPPJKJDPAMLKCDPMAKPGNKCPEDKPEEHEDJBLDAAAJBLPPKFOECHIIHCMKHGBKDGPPPINFEOEEKOBGGPNJDNPACNOHNBBAJHOLIJGAPFMDMAHCEKOOCJOLGNBILKEOKLFNCGPEMAIILDBJNFALMEEAHEIMGOOAPMPOJHDMCOGLKIMPCIBEAAAAAAJJHBNCAMHJLFGONGGBFJBMFNFJGLOLHALBBDLBHI";
                    break;

                default:
                    throw new ApplicationException("Unknown web server " + Environment.MachineName);
            }

            return cipherString.DecryptText(connectionString);
        }

        /// <summary>
        /// Get DataReader with all taxa that matches the search criteria.
        /// </summary>
        /// <param name="changedFrom">Get observations registered at or later than this date.</param>
        /// <param name="changedTo">Get observations registered before this date.</param>
        /// <returns>DataReader after reading has been finished.</returns>
        public DataReader GetSpeciesObservations(DateTime changedFrom, DateTime changedTo)
        {
            ArtDatabanken.Database.SqlCommandBuilder commandBuilder = new ArtDatabanken.Database.SqlCommandBuilder("GetSpeciesObservations");
            commandBuilder.AddParameter(Observationsdatabasen.CHANGED_FROM, changedFrom);
            commandBuilder.AddParameter(Observationsdatabasen.CHANGED_TO, changedTo);
            commandBuilder.AddParameter(Observationsdatabasen.IS_PRODUCTION, (Configuration.InstallationType == InstallationType.Production));
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with all observation ids that has been deleted during the period changedFrom - changedTo.
        /// </summary>
        /// <param name="changedFrom">Get observations ids that was deleted at or later than this date.</param>
        /// <param name="changedTo">Get observations ids that was deleted at or before date.</param>
        /// <returns>An open DataReader for reading.</returns>
        public DataReader GetDeletedObservations(DateTime changedFrom, DateTime changedTo)
        {
            ArtDatabanken.Database.SqlCommandBuilder commandBuilder = new ArtDatabanken.Database.SqlCommandBuilder("GetDeletedObservations");
            commandBuilder.AddParameter(Observationsdatabasen.CHANGED_FROM, changedFrom);
            commandBuilder.AddParameter(Observationsdatabasen.CHANGED_TO, changedTo);
            return GetReader(commandBuilder);
        }
    }
}

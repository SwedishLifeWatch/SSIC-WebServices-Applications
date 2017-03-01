using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.Database;
using ArtDatabanken.WebService.SpeciesObservation.Database;
using Microsoft.SqlServer.Types;
using SqlCommandBuilder = ArtDatabanken.Database.SqlCommandBuilder;

namespace ArtDatabanken.WebService.AnalysisService.Database
{
    /// <summary>
    /// Database interface for the analysis service i.e swedish species observation database.
    /// </summary>
    public class AnalysisServer : SpeciesObservationServerBase
    {
        /// <summary>
        /// Analysis Server constructor.
        /// </summary>
        public AnalysisServer()
        {
            // Increased timeout
            CommandTimeout = 600;
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
        /// That is, address to data base server and name of database.
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

            switch (Environment.MachineName)
            {
                case "ARTFAKTA-DEV": // Team Species Fact test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAMODCOMDMCJIDHAGBLALJLPGBMNCALMJBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAACNKKPDJMDGIINKLCFLAMNHEGNBKBEGDOKAAAAAAADINGDJEFMOPFNFJNHCADPMGAJCBBGHEDPBCJGMDPEAADIPNDFANHEALDGKOCJKBBPEHODINMHOCKFEFPCNFDKFNONFMNIECDDJEMEMLKNNICFDIHBJGEMJIDCPCALMGJMIJBGFAFLNMACDICNFKPLCNGBKCKPMLGBGINDAAJHLFEBCJPFPMHLDONBKGLCAHNKKIHBDCGAKNFELPFFGAPFGDNPDEJLNOECNIEGBHEFBFJAKJGFIONMKAFDJEBIOBFHKAICGIAJPLOIOOPOCGNDGMOKBFBAKECIKHKBBMLMKGMOLLIPHIAPABFBJFLGNJPBEAAAAAAMCIDJLHNBIOGLBPNGNKLHIOIFFNPBDAHAGNAAAPA";
                    break;

                case "ARTSERVICE2-1": // New production web service server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAELDJLKDEHEAFOGONLAJMNLLEKJDDLLLAAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAMONALEPIJKLKIPAIGFGNJKOIABPMPJEJKIAAAAAACNELDFHPAAMHGKKEMAJKDMPMDEHKJIHAKKPHNLDELIKPGKNBLABPJPCMEBCLEPIOFLAIOLLAOOPLHICMCGICMACPBOOKHKGIMMOJFAJDEBJJGBAHDBOOLGFJBANAEBCIMCGGIFLLMDOKPLMNLCKNFFEHBANBPDKNOAAEOFNCPMJLMHMELIGDDDAOLEMGNKKJDIIMOIKAHHGBNIFPICKBBAGHOEEDEDAGHEKHJCHOFGDJGNBCJKNJNPMCNCOIDEHKIECFOMAFCGBFADBPEONBEACGCBNHJDGANBKLBCGBIJMAJJIMEMFPBOEMJECJMOODEMEGNCNCGOKNEIDEBEAAAAAANDGJBCJPMAOBHELCEIBDHNGEBKFNPOGAPPJPPGOE";
                    break;

                case "ARTSERVICE2-2": // New production web service server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACAAIODEOAPMOCAPKKEMFJJNGMJEDBOJBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALIPJAFDIKHGBBPLHHPKGIPCFJHGGAMEOKIAAAAAADGIFGNBIDDCKPBLJMLAHAGJLBDGMKMKOJMLCFPEOLHNLLPPMNAIFPIPOAOIKGCAPEONNEBHCANKOOKPCCOLIJHHOFALMONDDOIAGAKDOGBGIHBKOCEBPEPDOFCCNAMLJKEJIGKEGGMDFKNIFAODIHNEJCJEBGNBENHLIIOBIACGCGOOGECEOAOACIGPPCDPFIBFBOKNKHMPDGCFKCAPHGIEJCMNPHMDKBLEBMAKBEKNOLNLHGILPIOFMBJEJEENLBIBJMIOKKNIGHKJNLHNCMLKBECNHLAJPGDNOJIPDCIOCKMCBOKOLMNIEPLNJOJGJGJFMGDGKFPOJBJIGBEAAAAAAOGEPMFKJCDBEEEFAFGKABKPICMGIBMDABNMADEPI";
                    break;

                case "MONESES-DEV": // Test Web Server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEAFPECBBEFEHIKEGLFFDMHECEONJDGANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFCJGLOIAAHKDBJLDKPJNCFFJMFMBHFKNAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAANBDEKHKFMPPMJJCHEKLNAFHIDOCGHBELKAAAAAAAJAEGFAGNHBEOIIMDHNAAAAFMPCECEOMNBEDPNLELCMDMFHGMJFKFDOIGMNBLJFMHNMFKPIGLKNCIJKANOPLBNMAHFFOLGBJHAIEHHCHBNLMLPOOEHOPDBOCLAPBNBNPCLFGCFDOFOPHPFMLEMKKFKIFKLIKPCIHCPOKOKAEDCJNJFGJFDHCLEGEEJAFIFHEGBOIEENAPFJDIBHHNKIFDPJNNOLEIEBODDANLMAHCODAKLLAHFBKHIACFPOIOKPBIDEPEHCNJHCBBOMCHCILEKABGMOCJLAEIHBJOPEPLOOLBJEJNBDHPMGNKBMBJMDJNBEAAAAAADBLOPOKHCJNCEHAIJOJNFNDJMHHIIOMKJPBNMFKP";
                    break;

                case "MONESES-ST": // System test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIKGEBDAPMKNOAFEKIHFGCOJNGHIAKFIAAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPBKBOEJAODCCEHODNPKMBMLEDELEMMLNAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPOGHJNIFAEFBGOMIJGBGLLFKPHMCMDIOKAAAAAAAJJIDOBFCHPMLHEKFEEKFIEJDICIDAFOOCFGOIIADFPFMKCGJHBLMCOKCICGFDFNEILHKDKLBFJALEKJGJNDBFIMDAGBPANPPNBFALIBGEIOEKLEFLPLGIHFDLBBKEDDOMAPLHKOMBKFKLADNLCAMAEKFCOLGPJHBODAACPCNBDNKFHCBNHKLIFNIMNEKOANJELFHGPFFNEKFIKOPPBBKHMFMCOOGJIEPHANEFAFBNHOEKLGJCKOOEIDGCGGADLGBIFMEHJMCGIEGHFBEELAKIGFCABJILJNAIKCCENEKOLMLAIALBCCFKIGKCAOGGIEIBEAAAAAAJMEPBKAILBHIJHKAPFNPCOKFONBIKJKBKHEBECGK";
                    break;

                case "SILURUS2-1": // Production Web Server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIDGFKMFBDIMAPEELJPFDBGPLIAHNCJMKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAKJPIGFPGGPAPAEECHPLAOOGPFHFOAFNIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAMGIPJCLNEPNEALBHCGPKJFMOCLHNOHHIKIAAAAAABJAMMALKILAGJJDKIPCBFAHBIDIJFDJGINCLDMJMPMDFONMKIMJKNFCIOBBPPHNNNBIIHHEHOJOJONFDGHDHNAMNOKOLJHNNOFPDMDPMPHBFDIJPOMDNBKCNBCNGLLOPJALJJCDLEJHBNIPAAFAKNDPAOGLICAFMJOCIIPPFAKAFKNCJMNCHACBALOLGDIHNALHGLMPLABDPPMGOLJGGBFHALIHCICAHOPHMBECNCNPAJOGLKELEHMEJKMMJHPKEMDOCKACCAAKFFLDECNGPIIFGFLLGAPMBBCGLADJHNCKPACNDIAGDKFLFBFOIECABKNIEKLECIDDFBBPBBEAAAAAAPPDCMLPDOHMJPIHAGFAMHMLGONHGFEIFKCCFNILO";
                    break;

                case "SILURUS2-2": // Production Web Server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAODPAFKAIHGMAPDEOIPNDNGAIEJHDINBGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAGFMJAJNFNFEPPIKKLGBLIGANNKHIMLNHAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAACNMDCEILEEBFOBJBMHPFFKADGNLKMAEHKIAAAAAAKAJCMHOHGKMMHMPBNBBLMKNAEONOFHHPPHJLNJHKCIJCNMNEACEBNCLFKKNHJNHPJDLDFJEFKAGOEPNBIGLIOELCCFOOFKAFCJKMFGLDELEKCLJGIKNEOAENNDIGCGCFGGJBIGLKAPLHHCHOIPJLJCGLCDMNAPDJPCOFDDGICKJNPJKFFJOOGKBHFFAEPMKKLJIIJCHCBIIEPIBHACODKEJHEGJONDMLPGBNHGODFJKCNOAPIDNOIJBNKMELEDNKABJGLIIDKFDAMPEODBFHHJOFKKLNFMBLCBLEHMJEEJPAHEKOFNMONEMHNLBDIBPHNOAPOAMDJGFAACACBEAAAAAAFMJLNBDLEGCKBPEDMJFGDKLILPENMLMEBBPEKCFA";
                    break;

                case "SLU011837": // 
                    if (Configuration.InstallationType == InstallationType.Production)
                    {
                        connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABGDPKNGGHAFOAMEGJJHCDBPNFPCJJBMBAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAAHGKNDDMGOMFGHOKGAHPAIOCPDNBFKMDAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALICLKAHGAFKFEJNLPGGMIOIAFBKMGLAOKIAAAAAAFICOHLDIEBIGFOHJMPFGOJHPKFLGJILMLPKLEJMJFAIBDLFHDLIJFOIFLDFKFLKIICMDGHHAOOOADCGPHMEJPMBGLEIHPCNDCEKDAEIHBAKFPNAHPELKPFCFOMEADMBCKHOHIPHPPEPGOELPBGJODHEELMPEEKEAGBFGPLGICEHMNCCNFGBOJCNDACFJPEPFIKBMJJKLDJKPDPOICGGPPGDOEBGLGMCNHFINJHPJGNIPLADABGJFOCONDDFBNADEKFPCABEOFBNNLIHNDCPDONDJKJKHNKJJLMOHLDNGBAEFNNMCFNBHIIKJIMAGBOJBKBJCJLNEGEPHOLPCBEAAAAAALOOKDGHKHMNAMOJBGPDNHDENABOMKGHEKBKCEICM";
                    }
                    else
                    {
                        connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALCPBDBGCIAFPEPEILEKMLLIKEOOJMJMKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAABDEGGFPJKBPDGFCNMBNFALGABIDNPLIHAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAACKCDFJMGICPJGBGCNKPDIBPMBAJGMMMEJIAAAAAAPHACCHNEMOHAKFCNGAJACEMKEKKPKMENHKKLBIJJCGCJNKIJILODDIIFKPIHHOACFCLGLHDCMKMJKNHJJEJHDDJPPPPGEELILHFBHGKFJNOBFLBDDEIFBBKDFJDFJLEFGHDHFLHICBBIFCNEGKKPGPDCIBPHFLNJFIAGJAKEBGCIGANFHBBKHCAHDJIDKOABEJKNBFFALNDBDICEOAGDHAAFODDNPDHKOCGKIDGKGJMLMHGDLDEGMPCNCGPIOJPJCLNDHDBDFOKEJGOFOIEHPLNNONGHMMFHDGOEBPGFABMMJHFGBEAAAAAAKDDLIBNLBNLHOHEJHEOKOJFNLNJGEOBHFAHNJLIO";
                    }

                    break;

                case "SLU002759": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAACIDCABECFOLLCPEPKJMHHGGHFCPOMFOJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJOHGEGHAECFHOCMNLJNBCKHLMNIMEGAHAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAIFGEDIBOFGKDIMOPMDDJGHNBLOOOEHDFKAAAAAAAAOHLJFGOJJHKKGJBDDMKGHMNNEACEKBHJGFDOJKCHHLACMFINCGAPHOOPEDCMEIMCAOIDJMCNKHFHAEBLOCNCMFHBCDICOKMBCNJIAJOFKLHHAPJCOAINHPHBDHEGELFPMBHPJJOHECHEKEEFKCHEDDGHFFDEOLAKMFILFJOFCBEONDBNEMLAAGPKONCOOHGGMBGLJALDIFFOAAOKAMAPDHHNCJMGCFKODMKDMFLGHBHLIAHIGNCBBKJDFFAABIINMFEFKPPOPLEAFEHOMCGKIEOPEBPAGAHFLBGMCNCBDKIIALLKCHBODHBGLGJOKLHBEAAAAAAGPBCHKBGKOGFJNMKNOBDBFFPGOCJIOIGJCONHHIN";
                    break;

                case "SLU002760": // 
                    connectionString = null;
                    break;

                case "SLU003354": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAODHAJOLEPIKFCCELKPNHOCHFBJAKLOCOAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANKLHMJPLKCPOIFFEOJHAHBGNDNNPPAIEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALJBODOAEODDMLPGMGAAHPBIPJBJJNKAEJAAAAAAAFOBNDLBFLMBDGNBEKDPMGNKEEJJPNDMFIICBNJHJLDBLDCMPNHECIGOMMFHJMOPJMHHMNIENGBBDJJCHBCPDKBPDHFDDNKFNIMMGONNBFLJFEDIBEMEMNKIAKBACBAAEAFABIAFLIAIOHIFBKMBEPGKKLJBOMCOGDAAPMCOIMFMBDHKMDDNMPBKLINBIMDHGKODJECKNIIDLBFIMJAGOELPLPCPCADLGKGHOJPKMIFFPKPLCICCFNFHCPBNPKBIIJLPGHCKKPCNPDOEEOMMMNOOLGBGIOJODBEAAAAAAMIAACNCKCKBFKAALBGILLGOBEHHJLKFHIEFOMLMB";
                    break;

                case "SLU003657": // Maria Barret-Ripa
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAANKDBKANMMAJMDJEBJCBKFDBPNKCMLNMCAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALFLGDIOGDEKIKMJALCFMJEIJFPIDJEAEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFLFANKMAPCLOPCIEIICBDNCCGGBJOACFKAAAAAAALFMNGIMJLPHNMEFONLGBJFFGDGLBOPEOIHNOFAEIFPEGNGJDPOEMMLLMDLAOKPIBLGALLCFLOFODMJAEFHANEJBLMMHOLGBMKKANAGOGHDCKHKGFNIFECEEBLGPBDKPHILKLLHGLBECMJDIMCDCOGHDPDPAFAHNIJPPHEEBLHBHJOJKIMGPBGLPBLCIKIDHDAEOGNJOJOJKEHNHAMNOEFAEKIOBFEEOHIDFFKONJBOLGIOJOFGAJHJLOHAPHJLMMKKPBDGEOMPFNHCBECBCEGJOMPBBJEOKLENBMDMPJEJOODIIGPBDIBBEIHIIADPNFBEAAAAAAGCLFJMGNGIHHEPBAGOJDGJJLMDDEHICICMBJHIED";
                    break;

                case "ARTDATA-TFS": // TFS 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABCNHOEMJPBDFAPEFLODEONCFHMLDLFFJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAAKGNFKGODOEMGIOADBLIOMNMNPKJBMPCAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJOJLMBMNDHCNHKDLGHAFGOECIEIHOGHKJAAAAAAAJPLGLMCJKABLBAMMCDILEOFCHAKIFCLAOKPGGIBHGLNGFFCBEPCLOJBLCCLEKHELFDLBIGGDHLJCFKIFJIKLMCAIMICFCDNCFHIAAOBBMLKAOPAAKIPGIHMAAIENLNGFJLLNFEDFDBACGGKIBAGEIIHAHNFNMEDLCINKICCBKJDDOPHNEPGCICCAKCBOFPMJCBENHPIMGHIEBOFAHPEAHJDMINIGGOKJGIAODOEMGBGBBOCFJAEJPOAEGLODCCFGBNAKLFBLKNCGMLOMDGKFEOIIIHJKFPJKBEAAAAAAJKNNNHIHMJIIBKINJMIIEIANDIFKJOLPFIOEHJCM";
                    break;

                case "SLU011730": //                     
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAADIHMOJGONBDBMKEPJFMKFPHPJJOAAEHLAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALGLKDHNIMBJEMCKJCDCFEDECIJFDGGANAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAANLDCLHPAPNAIPLJIAHOBHGIFOJIALNMNKAAAAAAAEDNPLMJACCBLONKIECKKCPFBFPPPFNCCCJEBBMGNCKPKILDEFFPFGNKILLHAKIIMAFBNPBOIBMOBFCKADKKNAMOAHFNBKCJJKEBPBCCPCLKGGAIMBLLCHIMFIKOCNNECBBDJHNGDCEPDBBDANHNKFNJPAJBCACFJEENNPCHGDEBCJDHLECIECLOONMHJELELCELCMDNKJHEPHCFODODAIEBBAMGEMOCCJBOIIIPHKGCFECPEKDIIEPCHOAKNPNMIMLMMEJIINADAJCPGEIAFBDFDOFKJPLPLABEGJGAAHJOLOLOAKBCKPDHBIBLJALBLBEAAAAAAJKNHKIINIGNJIJCBDOPFKNKCDKAPGLOLMJDGOJLM";
                    break;

                case "SLU005126": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAKODAGIENDBFDOEEPKAIMHGGHOICOBJNKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALKFODHNPIFMPBBNLBFPENJCMPAKKEMDMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAANNBJBABAGMMJHJJGBGHOCGKANJIAFMAAJAAAAAAANPPKCMGHBKEDJGEJDJBFJHBJFJGGOIHGGLOKOODMMBHLGCLOIDIAPFGHBBANACCABCKPKHCGAIGPNKMPDHJANCJMOAOBMNNDMJMBKOAGIHGIHJPCHDLCJADLFKILMGEOLLANGOBMALMPCMFGJJLLANEDKLPDJMNAKGHHMJKAHCEGECCHDNNMENIHPEJFLKAJJJHKMPGMELGFEIIEOCIPHPOAJAACDKJOKGIIOFKDBNNKEDIPJCNJCJFBFBHCOACACLICNCGCDIPEDNPIMKMGPMCAOMHFFJBKBEAAAAAAJDFOCBPMJKHNMCADFEHFGHCBCIGCNIOIINOLKHEK";
                    break;

                case "MATLOU8470WW7": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAJPCKNPEHLGFIJPEOKNHDKMHIMDFHLDCIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADKGEEEIBGIHLILDKIPFEBFGMFFBLLBBBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALEMCIIBLIKFGAEGEAILPDMLCHNLJDCJLJAAAAAAAPGFLEMNLCFEPNNDNMOPNDGCDONAMMLGFMPKCGIGHKJDJAPJGJNIMLAEEHICJENPDOBDPMHJMIILCDNFIHEJEHHNLIDKJIAPFCJPLCMLHKOLFCIPAPDPDIMFNHHNBJELGDENAFKGGDPEOEBAMGGMCNODNBGDIFPILIODDHHGOAGMEOFHPPLDPBPDGMOEJOJCMKPJJFHIKCNLCKICEBOIJCHAGBLKNJCKAFKLOJMPABDALBABMDNLOCNFOPAMOMKINJIDNJFAAOEFAKINFAPGDADHCMNIOHAHDBEAAAAAABPJPMHKMIANAGOANJMMJAIKCFNPNLALLEECKIAPF";
                    break;

                case "TFSBUILD": // TFS-Build server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAJDEHMIDHELNCKKEHLHOKFNFFKPMHAKNDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAOPKEKNPIBFJECGPFHHLNDGMODNNGPBCCAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKMKGMJEGKIOPOLNKIFFKHGKHALDOOGGPJAAAAAAAAKMEAMPCHHPNLFMNOHBLPNPDKNCKGJKPHLKKNABMHEPGNKGKDDEFEJGHFDCKHOPMCOANALBLJNBMOMHLJCEGOICNEPKBNMNOLKONKODBDIFBPPHLFIFEGKEEPIPMMJAHAGPCPABOJIMJMIMDONGGFOEFJPIHMBOKFONEDHEHBKANEFIBMKGLBFCLDBKLIKLLBNFDJFFOHGLPINDGFGOFFAONEMFGJDCKICPDEDJBCCKMLJEOIOFOFDJMHJIBIDHFIOFACCBKGEKDMIIFDEDDIFAIEDFJILKFBEAAAAAACIMDKDJGCNADPIKCABLOKAGNKEKCBDDAAIBOCAGD";
                    break;

                case "SEA0105WTRD2": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABCDHLPKLHGBGOOEJKJIFHDOAFNEEFMNKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAMNLCKJMFEAJMNEPLKOCCGNNGBDNMPPEHAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJHCNBMIPJHGJPOLHJBJPPJFIHPLOINAOKAAAAAAABOPGEKEJDJAIDJLNMLOOBAKPMPJFEAMNLMFDCNNMEKILKGFPJEOMKIGIPHCGEBJENPFOKFNEDDLJAFNLKHOLIILIAMILIMJGFGFLNGGCPEMFEHADIFFCPCNLCALHEFHKEGFJAEPJCKPJKOMMNBJJDEDNNKKNCHOMONIFKFHGPCDNPMOHCANLABIEHJHAFEIELFFHKGNKPBEFFJDGEECCDHEDEPGCMDOCJAPDPDNOELKMIOCCFEFMALJOKKGEFILOFKPGKKDBDJEHLEFJOAOAKBPAHNMLPFHFLNHCIEKKKFFBDACMOCMDDFPCJJFDDDFNBEAAAAAAHEFDLEMIABJALIMNPIHGKHCLLNCMNLKDCGJHDGLF";
                    break;

                case "SLU011896": //
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAFNEPGOOEOHKKGCECIICFNMJLKFBFPGCCAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAIOJDFDOGKJFPHFBJOJAKJEBIKIGFPPPEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKJCGKCAMDPENBFNBADPPHGNBHOHCJKFGKAAAAAAANPBBOMIGLOIHAJPJHGNNCEDFMOKMCNIGPKPHMBBNINNHKOIIDFIGGAKNHEEKBNIOPHMACPJBMNJIGMIJLLLBMCHJJJMEMCPPOIGHFBPEDMCLIOAMMBFHHPANCKKDFHDPMJKEAJHBEMKBOBAPIBMPFOOICBKHPGKAEKGCALIIDOLAJLIKBBONAHKEHKOCIBCBECHKBANNOMFAEAHLDILLPPOBIEJHLFKMPPCJKLNEJFIPKEENMMFEIJFDMKHIKJGKALIDJMBPDIBBDDHLJMOCJOECPLGOCKBOFKFFKFJJOLCAGJPCHFNCLAMJHJPAEDPHBEAAAAAADMEKKLBCHJLKKHJNBPIBKHCIEPLIFAMEDOBKMLLA";
                    break;

                case "SLU011895": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGJNNAFOJJCNCANEIKCFBFEDAJKKMADANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAMDBAPANMMEMFEFBHEEKEGOLIJNGDGNGNAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADKBGGFEDKNIPMKOMHBKOGMKIBCKOLCEHKAAAAAAACNGKELGGFPIGKDOPDKCHBDNLILMDALENJPIHAJKHMDLEPPJGLIJMHJNABBHHJILCHIBELDEIAEPFPIAPMBCMMMGDJPNOMFMFCDGLEFGKGFBEOAGACFPPKCPINDILFLOHHAHJBEIOJAKODIAOKCLPGCIFALIADLJEMCKJEFOMFBDCDAHKOFFGNKFJPIKCLMKOHBAACKNJOEDBCMFPLIJPHOKKOMPNFOJPPCHPBANNEABAKGGIAPECIPLPGIHDBNGONPJONDCMCJMHPBLHGGPJKHMBAPNCLBFIMGDDFKKFCECIIFJDLNLHHPMFHAGBBKHPBEAAAAAACNKLBEGJIPPFAOAGOBEGFHOKCOAKINAPDBJIDFNL";
                    break;

                case "SLW-DEV": // Team Two Blueberries test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAEFNJJMHEFNEPELHNOECCCKLLJHNFHNDIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAOGGCMDGHNBNEKLNDPJFHLGLAEDDCKIBNJIAAAAAAGIAOAEHMIEAKGNIBCGFGLIHPGBHHCPPPMCKMELKCPGBGPOAIAPDNPOBLIMBDIGOIBCCKJNKGIIINCGFEEOHINLLOGKLGENAGEAACNJBKDLFMLDOOKGKLFHJKMEJFJHIBHBHLGPJGPOBJMKKGLJNENICPHCBDEBNGAJNMEALMKLMONOJENFJMHJFODLABGBGFCOHMCJHCDEPNHBABNEGCEJFNDDCBBJOHBPEAJLHJDOEIEFJMEOJEGNMHLFLHPEHOFAKPEJBHFMLPPLHFMBPPCJBCAPIGIBPPEHEBCEIPPJGPJEHNBEAAAAAADPLNMLIHFFHJHOLKDFKNAMANKILJIOMFDHFBANNP";
                    break;

                default:
                    throw new ApplicationException("Unknown web server " + Environment.MachineName);
            }

            return cipherString.DecryptText(connectionString);
        }

        /// <summary>
        /// Get number of species that matches
        /// the grid search criteria. 
        /// </summary>
        /// <param name="polygons">Polygons selected.</param>
        /// <param name="regionIds">Region ids selected.</param>
        /// <param name="taxonIds">
        /// Taxon ids selected. Observations related to specified taxa and
        /// all underlying taxa are included.
        /// </param>
        /// <param name="joinCondition">SQL join condition.</param>
        /// <param name="whereCondition">SQL where condition.</param>
        /// <param name="gridCellCoordinateSystem">Coordinate system when performing grid search.</param>
        /// <param name="gridCellSize">Size of the grid cell.</param>
        /// <param name="speciesObservationIds">Ids for species observations to get.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetGridCellSpeciesCounts(List<SqlGeometry> polygons,
                                                   List<Int32> regionIds,
                                                   List<Int32> taxonIds,
                                                   String joinCondition,
                                                   String whereCondition,
                                                   String gridCellCoordinateSystem,
                                                   Int32? gridCellSize,
                                                   List<Int64> speciesObservationIds)
        {
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter, name the stored procedure
            commandBuilder = new SqlCommandBuilder("GetGridCellSpeciesCountsBySearchCriteria", true);
            commandBuilder.AddParameter(SpeciesObservationData.POLYGONS, polygons);
            commandBuilder.AddParameter(SpeciesObservationData.REGION_IDS, regionIds);
            commandBuilder.AddParameter(SpeciesObservationData.TAXON_IDS, taxonIds);
            if (joinCondition.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.JOIN_CONDITION, joinCondition, 1);
            }

            commandBuilder.AddParameter(SpeciesObservationData.WHERE_CONDITION, whereCondition, 1);
            if (gridCellCoordinateSystem.IsNotEmpty())
            {
                commandBuilder.AddParameter(ObservationGridCellSearchCriteriaData.GRID_COORDINATE_SYSTEM, gridCellCoordinateSystem);
            }

            if (gridCellSize.HasValue)
            {
                commandBuilder.AddParameter(ObservationGridCellSearchCriteriaData.GRID_CELL_SIZE, Convert.ToInt32(gridCellSize, CultureInfo.InvariantCulture));
            }

            commandBuilder.AddParameter(SpeciesObservationData.SPECIES_OBSERVATION_IDS, speciesObservationIds);

            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get number of species observations that matches
        /// the grid search criteria. 
        /// </summary>
        /// <param name="polygons">Selected polygons.</param>
        /// <param name="regionIds">Region ids.</param>
        /// <param name="taxonIds">
        /// Taxon ids. Observations related to specified taxa and
        /// all underlying taxa are included.
        /// </param>
        /// <param name="joinCondition">SQL join condition.</param>
        /// <param name="whereCondition">SQL where condition.</param>
        /// <param name="gridCellCoordinateSystem">Coordinate system when performing grid search.</param>
        /// <param name="gridCellSize">Size of the grid cell.</param>
        /// <param name="speciesObservationIds">Ids for species observations to get.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Wkt"),
         System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "multi"),
         System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public DataReader GetGridCellSpeciesObservationCounts(List<SqlGeometry> polygons,
                                                              List<Int32> regionIds,
                                                              List<Int32> taxonIds,
                                                              String joinCondition,
                                                              String whereCondition,
                                                              String gridCellCoordinateSystem,
                                                              Int32? gridCellSize,
                                                              List<Int64> speciesObservationIds)
        {
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter, name the stored procedure
            commandBuilder = new SqlCommandBuilder("GetGridCellSpeciesObservationCountsBySearchCriteria", true);
            commandBuilder.AddParameter(SpeciesObservationData.POLYGONS, polygons);
            commandBuilder.AddParameter(SpeciesObservationData.REGION_IDS, regionIds);
            commandBuilder.AddParameter(SpeciesObservationData.TAXON_IDS, taxonIds);
            if (joinCondition.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.JOIN_CONDITION, joinCondition, 1);
            }

            commandBuilder.AddParameter(SpeciesObservationData.WHERE_CONDITION, whereCondition, 1);
            if (gridCellCoordinateSystem.IsNotEmpty())
            {
                commandBuilder.AddParameter(ObservationGridCellSearchCriteriaData.GRID_COORDINATE_SYSTEM, gridCellCoordinateSystem);
            }

            if (gridCellSize.HasValue)
            {
                commandBuilder.AddParameter(ObservationGridCellSearchCriteriaData.GRID_CELL_SIZE, Convert.ToInt32(gridCellSize, CultureInfo.InvariantCulture));
            }

            commandBuilder.AddParameter(SpeciesObservationData.SPECIES_OBSERVATION_IDS, speciesObservationIds);

            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get number of species 
        /// that matches search criteria. 
        /// </summary>
        /// <param name="polygons">Selected polygons.</param>
        /// <param name="regionIds">Region ids.</param>
        /// <param name="taxonIds">
        /// Taxon ids. Observations related to specified taxa and
        /// all underlying taxa are included.
        /// </param>
        /// <param name="joinCondition">SQL join condition.</param>
        /// <param name="whereCondition">SQL where condition.</param>
        /// <param name="speciesObservationIds">Ids for species observations to get.</param>
        /// <returns>
        /// Number of species that matches search criteria.
        /// </returns>
        public Int64 GetSpeciesCountBySearchCriteria(List<SqlGeometry> polygons,
                                                     List<Int32> regionIds,
                                                     List<Int32> taxonIds,
                                                     String joinCondition,
                                                     String whereCondition,
                                                     List<Int64> speciesObservationIds)
        {
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter, name the stored procedure
            commandBuilder = new SqlCommandBuilder("GetSpeciesCountBySearchCriteria", true);
            commandBuilder.AddParameter(SpeciesObservationData.POLYGONS, polygons);
            commandBuilder.AddParameter(SpeciesObservationData.REGION_IDS, regionIds);
            commandBuilder.AddParameter(SpeciesObservationData.TAXON_IDS, taxonIds);
            if (joinCondition.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.JOIN_CONDITION, joinCondition, 1);
            }

            commandBuilder.AddParameter(SpeciesObservationData.WHERE_CONDITION, whereCondition, 1);
            commandBuilder.AddParameter(SpeciesObservationData.SPECIES_OBSERVATION_IDS, speciesObservationIds);

            return ExecuteScalar(commandBuilder);
        }

        /// <summary>
        /// Get provenance darwin core observation(s) that matches search criteria.
        /// </summary>
        /// <param name="polygons">Selected polygons.</param>
        /// <param name="regionIds">Region ids.</param>
        /// <param name="taxonIds">
        /// Taxon ids. Observations related to specified taxa and
        /// all underlying taxa are included.
        /// </param>
        /// <param name="joinCondition">SQL join condition.</param>
        /// <param name="whereCondition">SQL where condition.</param>
        /// <param name="speciesObservationIds">Ids for species observations to get.</param>
        public DataReader GetProvenanceDarwinCoreObservationsBySearchCriteria(List<SqlGeometry> polygons,
                                                     List<Int32> regionIds,
                                                     List<Int32> taxonIds,
                                                     String joinCondition,
                                                     String whereCondition,
                                                     List<Int64> speciesObservationIds)
        {
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter, name the stored procedure
            commandBuilder = new SqlCommandBuilder("GetProvenanceDarwinCoreObservationsBySearchCriteria", true);
            commandBuilder.AddParameter(SpeciesObservationData.POLYGONS, polygons);
            commandBuilder.AddParameter(SpeciesObservationData.REGION_IDS, regionIds);
            commandBuilder.AddParameter(SpeciesObservationData.TAXON_IDS, taxonIds);
            if (joinCondition.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.JOIN_CONDITION, joinCondition, 1);
            }

            commandBuilder.AddParameter(SpeciesObservationData.WHERE_CONDITION, whereCondition, 1);
            commandBuilder.AddParameter(SpeciesObservationData.SPECIES_OBSERVATION_IDS, speciesObservationIds);

            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get provenance data provider(s) that matches search criteria.
        /// </summary>
        /// <param name="polygons">Selected polygons.</param>
        /// <param name="regionIds">Region ids.</param>
        /// <param name="taxonIds">
        /// Taxon ids. Observations related to specified taxa and
        /// all underlying taxa are included.
        /// </param>
        /// <param name="joinCondition">SQL join condition.</param>
        /// <param name="whereCondition">SQL where condition.</param>
        /// <param name="localeId">Output should be returned in this language.</param>
        /// <param name="speciesObservationIds">Ids for species observations to get.</param>
        public DataReader GetProvenanceDataProvidersBySearchCriteria(List<SqlGeometry> polygons,
                                                                     List<Int32> regionIds,
                                                                     List<Int32> taxonIds,
                                                                     String whereCondition,
                                                                     String joinCondition,
                                                                     Int32 localeId,
                                                                     List<Int64> speciesObservationIds)
        {
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter, name the stored procedure
            commandBuilder = new SqlCommandBuilder("GetProvenanceDataProvidersBySearchCriteria", true);
            commandBuilder.AddParameter(SpeciesObservationData.POLYGONS, polygons);
            commandBuilder.AddParameter(SpeciesObservationData.REGION_IDS, regionIds);
            commandBuilder.AddParameter(SpeciesObservationData.TAXON_IDS, taxonIds);

            if (joinCondition.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.JOIN_CONDITION, joinCondition, 1);
            }

            commandBuilder.AddParameter(SpeciesObservationData.WHERE_CONDITION, whereCondition, 1);
            commandBuilder.AddParameter(SpeciesObservationData.LOCALE_ID, localeId);
            commandBuilder.AddParameter(SpeciesObservationData.SPECIES_OBSERVATION_IDS, speciesObservationIds);

            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get taxon ids that matches search criteria. 
        /// </summary>
        /// <param name="polygons">Selected polygons.</param>
        /// <param name="regionIds">Region ids.</param>
        /// <param name="taxonIds">
        /// Taxon ids. Observations related to specified taxa and
        /// all underlying taxa are included.
        /// </param>
        /// <param name="joinCondition">SQL join condition.</param>
        /// <param name="whereCondition">SQL where condition.</param>
        /// <param name="speciesObservationIds">Ids for species observations to get.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonIdsBySearchCriteria(List<SqlGeometry> polygons,
                                                     List<Int32> regionIds,
                                                     List<Int32> taxonIds,
                                                     String joinCondition,
                                                     String whereCondition,
                                                     List<Int64> speciesObservationIds)
        {
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter, name the stored procedure
            commandBuilder = new SqlCommandBuilder("GetTaxonIdsBySpeciesObservationSearchCriteria", true);
            commandBuilder.AddParameter(SpeciesObservationData.POLYGONS, polygons);
            commandBuilder.AddParameter(SpeciesObservationData.REGION_IDS, regionIds);
            commandBuilder.AddParameter(SpeciesObservationData.TAXON_IDS, taxonIds);
            if (joinCondition.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.JOIN_CONDITION, joinCondition, 1);
            }

            commandBuilder.AddParameter(SpeciesObservationData.WHERE_CONDITION, whereCondition, 1);
            commandBuilder.AddParameter(SpeciesObservationData.SPECIES_OBSERVATION_IDS, speciesObservationIds);

            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get taxon ids, with related number of observed species, that matches search criteria. 
        /// </summary>
        /// <param name="polygons">Selected polygons.</param>
        /// <param name="regionIds">Region ids.</param>
        /// <param name="taxonIds">
        /// Taxon ids. Observations related to specified taxa and
        /// all underlying taxa are included.
        /// </param>
        /// <param name="joinCondition">SQL join condition.</param>
        /// <param name="whereCondition">SQL where condition.</param>
        /// <param name="speciesObservationIds">Ids for species observations to get.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonIdsWithSpeciesObservationCountsBySearchCriteria(List<SqlGeometry> polygons,
                                                     List<Int32> regionIds,
                                                     List<Int32> taxonIds,
                                                     String joinCondition,
                                                     String whereCondition,
                                                     List<Int64> speciesObservationIds)
        {
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter, name the stored procedure
            commandBuilder = new SqlCommandBuilder("GetTaxonIdsWithSpeciesObservationCountsBySpeciesObservationSearchCriteria", true);
            commandBuilder.AddParameter(SpeciesObservationData.POLYGONS, polygons);
            commandBuilder.AddParameter(SpeciesObservationData.REGION_IDS, regionIds);
            commandBuilder.AddParameter(SpeciesObservationData.TAXON_IDS, taxonIds);
            if (joinCondition.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.JOIN_CONDITION, joinCondition, 1);
            }

            commandBuilder.AddParameter(SpeciesObservationData.WHERE_CONDITION, whereCondition, 1);
            commandBuilder.AddParameter(SpeciesObservationData.SPECIES_OBSERVATION_IDS, speciesObservationIds);

            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get number of species observations as a time series 
        /// with specified periodicity and species observation search criteria.
        /// </summary>
        /// <param name="polygons">Selected polygons.</param>
        /// <param name="regionIds">Region ids.</param>
        /// <param name="taxonIds">
        /// Taxon ids. Observations related to specified taxa and
        /// all underlying taxa are included.
        /// </param>
        /// <param name="joinCondition">SQL join condition.</param>
        /// <param name="whereCondition">SQL where condition.</param>
        /// <param name="periodicity">Type of time step.</param>
        /// <param name="speciesObservationIds">Ids for species observations to get.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Wkt"),
         System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "multi"),
         System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public DataReader GetTimeSpeciesObservationCountsBySearchCriteria(List<SqlGeometry> polygons,
                                                                          List<Int32> regionIds,
                                                                          List<Int32> taxonIds,
                                                                          String joinCondition,
                                                                          String whereCondition,
                                                                          String periodicity,
                                                                          List<Int64> speciesObservationIds)
        {
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter, name the stored procedure
            commandBuilder = new SqlCommandBuilder("GetTimeSpeciesObservationCountsBySearchCriteria", true);
            commandBuilder.AddParameter(SpeciesObservationData.POLYGONS, polygons);
            commandBuilder.AddParameter(SpeciesObservationData.REGION_IDS, regionIds);
            commandBuilder.AddParameter(SpeciesObservationData.TAXON_IDS, taxonIds);
            if (joinCondition.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.JOIN_CONDITION, joinCondition, 1);
            }

            commandBuilder.AddParameter(SpeciesObservationData.WHERE_CONDITION, whereCondition, 1);
            commandBuilder.AddParameter(TimeSpeciesObservationCountData.PERIODICITY, periodicity);
            commandBuilder.AddParameter(SpeciesObservationData.SPECIES_OBSERVATION_IDS, speciesObservationIds);

            return GetReader(commandBuilder);
        }
    }
}

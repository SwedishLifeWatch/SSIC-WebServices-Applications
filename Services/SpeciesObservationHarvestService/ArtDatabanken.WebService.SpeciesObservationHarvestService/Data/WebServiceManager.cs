using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.Database;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Database;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Artportalen;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Observationsdatabasen;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Data
{
    /// <summary>
    /// Class that handles information related to 
    /// the web service that this project is included into.
    /// </summary>
    public class WebServiceManager : IWebServiceManager
    {
        /// <summary>
        /// Encryption key that is used in production.
        /// </summary>
        public String Key
        {
            get
            {
                CipherString cipherString;
                String key;

                if (Configuration.InstallationType == InstallationType.Production)
                {
                    cipherString = new CipherString();
                    switch (Environment.MachineName)
                    {
                        case "ARTSERVICE2-1": // New production web service server.
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAMFAEIFIOMJHBJPHIGMGGLPLDBGFDBAOBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADLKKMBNLJFFABIJAIBCAAPPJFGJBKNNDJAAAAAAABNDDOKCJFKEDOODNFOOEAPCEAODKGMCDEDPHNDOOIBLDCHOFFJBNPHEHODCIKJNJCPDCLEJCGEDFLKDIIKHLFIHMKJICICIPKCAPBBGLMOCKAICNKPEIPKHOGNJMMAJAAGONKKNKGMHFBJFKMGEJJCFDJGKGONLKAPDPJNKEAIBCODHAEPHKPMAMDEJLLINAGHGFOOLOECMMJDPLDDIFGKFNBDEGFKMOGJNEBPKJGPBKGAPEODPPOPOGLGMDCBCBGKOBBMDDIGMGOAECMJAJFMJLCGECHLIJBEAAAAAADOHAJHBONNNKNHEHBLKMADMBDHLCILBAAMEFLFIH";
                        break;

                        case "ARTSERVICE2-2": // New production web service server.
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFCHBLGPKEKAPJOCHDBEDPMANCAOIOKPMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFDBINNOGGBLECFCFDFALOLALNCLJDFHLJAAAAAAAIALLCHGGMNEAMPCLECLOOPIEPEPGJNPBKNKJCGLEFLNJNKEFABJPJKEDFAMIEODJCIFAIACDEAAGDGLLKNHCPNFPOICFFCBFKNEKELBPPGIALLKNNFFOPABAKBGLEEIELKCMENPLLPNGBEJBFFJKKNPILFKAADNENAEKJMFIMBOPAGDEEGDCILAIJFOHKOAGBMKGCCDIJEOEMGEMFGJICLFAPOKCIPLNPNMGHGBNCLEFNLNCLPAILFEPLDGPFKPFDOGAAKMIMBEHDMMAJEANEOBIDNKNLGLKBEAAAAAAOAPHFKNLCNGEIODPCKHLGPPLFMFPHKEDMPLDDJHA";
                        break;

                        case "SILURUS2-1": // Production Web Server
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAKCDIPIMNCHNKMLEDLNNLHANAPIJOPGEOAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAICAOKDKOHMDOGAEHCKOPDBNMELBKODKPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALGLBEHBOEKAIKLDNAABFMLNOHAPIAENMJAAAAAAACFEMIPPHHBCKDLCGDHGKPPEPCJJJGKKKBKDAAOIIAEJKCPKCONDADGMGGDFNKPJCCNNIKFMACDFDKBOEHLCGKHCLCFIFKEGPPJBNGEEIKPEEOOHLCCEFKDMFGHKLDKAHHBJLIDFBIKMIFEMGFOBODGJAJPPPKAFPADCJOMNGOIDPMIJBFCLJCMFEONGJCCHEHBBDADJIFHCHBENCIAHPBPFJBMBKMKODNDKOBFDMDAEDJJIJJEKOPHDNJHNBAECHBLMILGJNFBIIIJHCPIGLJPLHKMOFBICJBEAAAAAANMCGHBDDIHDBOGGOLDCHDKLIKNONJMECGMGEJIDM";
                            break;

                        case "SILURUS2-2": // Production Web Server
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAADJCPFBPDFGGAJPECKJKGADMIIFAPGPOLAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJEJMHJGOJOCHOEBPLAAHOPMFBBGKDJIPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAANHOIFFOPPEFCIJJLFLMLNEJNOINAAFHPJAAAAAAADPPPFELCGGMNOJGOHIEDPFDGJKEACHPLHGKPPJDLABLAFGDIFKGFMCIBADBAKBCKLOIGOJBEGKCPAALEIAKFMCENMCIDGFBEKEAIDEOEGBMLAFJCPHNHNDOBKDLCAFMFIBFAGLJHOFKJNILLEOLEFGKNEEKOIDOBKFLHOGPDGPMGFDNKHICGDHPJDOAECDLBLMKDFKKBPOLGDNGBKGOPFDDLGIHNKLLMMPIDMACCEGMOGAPPGJMCOKKBDGJMMENHOEHKEFOKNONBPLMIPBBHNJDAKIOOMEBFBEAAAAAAKMDOFJNIBFCBNCADIBAFPEIGIKLMOCCCKPMEAGCB";
                            break;

                        case "SLU011837": // 
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPFOBOEOIJOPHGCEFJGCCBMFMHLOBJJLJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFFGMEKICGCFKGKCGLPOFGICOLLOHFKNFAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAMOJAINNHEGBAMBBIFEPJCAPKLLCFMLKLJAAAAAAAHCFDIKPMGEGDDPMIBOIOLJJPONHJNJIACBCEPNCHOLHAPMDDIPDGKFLFAJBBOGOHOMFCCFGEGACJIHNCBGELMAHCBGKMMPDGCPIDPKBGKOHJOKCIPAMBPFLCMEPJBMILNDLFPBIKOBPIACOKOELFGHKIKFGCFNHAPJPBNIIADKGHCGGFMFEIAGIDIDEIMIHIMBPADLIFCDALLJLMHKGPJPJKBJHENMLONKDLNLMEBJFKOIBMPKOPGLNDBEIBLBAGFJGEPBINAFOMBGIHHICKFKFCIOPGNEKJBEAAAAAAFHJAGEIIAEFGHJAFBIDDGKODHFDLFMFJBLHHFDHL";
                            break;

                        case "SEA0105WTRD2": // 
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABKIOIJPBLDPKPCEELADDGIHIPCLMBNMGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJMCCDLHDAKNPAHENJFKLDBHDDEMKPGEDAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAACFGACIOHOINJFIILMEAOOFEALEOMKHKAJAAAAAAAIMMJDGJGADIIJKADAEIHHPEJNJHBCOGAHJEJKFEHFPJNDDJKKPKMHIODKLBCILOEACELMKHNGCEEPNFHIKJLMLOKDJABCFPIHKOGPNHCFPAJOBIDOPBPKKPPIHLENFCEKNLMONKMAGMKKLLJAJEEAIFEHKBEFBHHEHAHLEJCEABGPNIAIEIHOIOFAJFEBOFPAMFNODIKLADJDAJBDHIGEIGNNNDNLFAAPKEOMABGMGBFHLGEENINPBMKJLFBCPOPHDKDAKNJJFLEIBBJIGFNHEAIJFKIMKOFBEAAAAAAHHNBDBNHNBNDNPLNPGHIDHGIEDEKOOIAFHDDLOFK";
                            break;

                        case "SLU011896": // 
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAFNEPGOOEOHKKGCECIICFNMJLKFBFPGCCAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPOCLLCNGIOKPFPLGGHGKEDGJCCMMJHEIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADGAEKEHBMPENNMNGCFAEHPFJBPJKPLJDJAAAAAAABIDEIALHLJMGGHKFNNNLCOPKCLFGFMBHHMBIMHPGANLAOJIDBAKPHBJDPLKMKLCKILFKMNPOKIIAKKGKBNEJIOMNECAEKLEDIHDNIGPNAGPKFALLCHFJMALNADKDPBOLAJKKCJKMHIMDIEBALLFCMBBPCDEBJFPBLHJIHOHMJICBCDJOFMOEMNPPOACNAFFFPDCEBMJJMMDGPIHCHNHOCCBNCCALJEOMGEEMHMOJGJHDCACLBHAPEKLKFDAMAIBGEFDDDFOKDIDBGEGGHMOPBOCELHDAGHHKBEAAAAAAKKDAECJLBCOPDBKFCOIKEOICLANDNDMKNNGBCHDB";
                            break;

                        default:
                            throw new ApplicationException("Unknown web server " + Environment.MachineName);
                    }

                    return cipherString.DecryptText(key);
                }

                return null;
            }
        }

        /// <summary>
        /// Web service user name in UserService.
        /// </summary>
        public String Name
        {
            get
            {
                return ApplicationIdentifier.SpeciesObservationHarvestService.ToString();
            }
        }

        /// <summary>
        /// Web service password in UserService.
        /// </summary>
        public String Password
        {
            get
            {
                String password;

                CipherString cipherString = new CipherString();
                switch (Environment.MachineName)
                {
                    case "ARTFAKTA-DEV": // Team Species Fact test server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADCCDCPAKBKGIHGHHBCNLLGGLMCDDNLEDAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAACDGICPBLFHCLHECNMIAPGBKJPLHBAHICAAAAAAAOODHGLLEHMOPJJFHCIIILHKGPKOJPFLFPHLDAFKCOAPHFGFLFINKGOJENAKLIIJJBEAAAAAALGAEEGHDNEIAPNJPGCKLFGPMGDFHHEILGEOKNHAI";
                        break;

                    case "ARTSERVICE2-1": // New production web service server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACPOMFILKPCMBHAPGIFLPCGJLIEFHCPLOAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHFKLNDDKMJFMNKIMLEAGFMKNBNAMDFABCAAAAAAAALPPBPNEABJJDPHKHJLEEGIDBMFPGMJHPDEGGPILANHICLMEMBBKLIFMCHAJAKKABEAAAAAADONOCHJGDBJOIENIOOFKCJJHKLBGHHFEFIMPBKBH";
                        break;

                    case "ARTSERVICE2-2": // New production web service server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAABCABHIHHEMGJFBGECGLNOOBEEFNOMBMDAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABHAIPIPPJGKCAKOEBIFLKOJOOFNOPAPKCAAAAAAAAOJNEGBJCKDDOCJPMDMNECMBBLPIKEIGEJHCIPIOGOEDHIGLLNBDICPFFHLBGCDLBEAAAAAALFCEBOMGKFEKBDPDGDAFOLMIAEEHLIBJCLEFADGP";
                        break;

                    case "MONESES-DEV": // Test server
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEAFPECBBEFEHIKEGLFFDMHECEONJDGANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADCDDAHNAKBPPHKJBJPLAKHEKPLBOHBFMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAOGIBDHAJJIJNGMMLDKDMJIMGNMGLPDGHCAAAAAAAOLHMALIDAJDNNJDCGMLNHFCOKOIPLNLPBFLMHOHGKLLFNPKJHPKEPNPGDMGNBCMEBEAAAAAAJIFGGDDEIFLDBOKABBBAFNJFBMCCEJKBOFPKNKCC";
                        break;

                    case "MONESES-ST": // System test server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPPKJPDANEPBIAOGIKLFNKDCOFKGOKHHOAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFEHABNEJCHDDLDBBLOKCIHOJDMMDFBNJCAAAAAAAHKFHLMFNODLMFKCLGDGOOLFKOHMKAAHMLBGFCIKOMLEEOLCLPINKNOFCCMFMFIMCBEAAAAAAKJLJFBJBDGIHGJPGMIPCBPCMPIOHMPMJOKAKONNC";
                        break;

                    case "SILURUS2-1": // Production Web Server
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABKFCOBBPIBEMBHEOJGJDNNOHKDPHDPJKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAMFCNLBABFEHDMAAECKPEFDBKEOHCOPJDAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABOGJLKAPKMKFHHBKPDKJJLGNOLJCMCONCAAAAAAAIAJDDGMPCJMPLPFLBLBLDIAENJKJJCEHDCDAJLDMOANCLKEPMHBDGCBAHBHFFMELBEAAAAAAGCDKKODKIFBKPPDFPFAHCFKMBKCEFHFIEEDBEIAC";
                        break;

                    case "SILURUS2-2": // Production Web Server
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAMLHPLFJBLJNKIBEPJGEAILJLHMPOLAAGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANAEFDGBIFBIIEJMMGBGNOGBCNGCBPFNDAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPEILBKHLNACMOOFBIJCMPHDLBPCJGFOFCAAAAAAAEIOMKAICDMICGALINBKHMOOLGJCAPFKDOKIKJNOBEIHBGOALCECLBGMCIDKNNOLABEAAAAAAFLOBOJJHJIGOKJPEADJGJGAPJCBOCLJNEMLLAJHC";
                        break;

                    case "TFSBUILD": // Build Server
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAJDEHMIDHELNCKKEHLHOKFNFFKPMHAKNDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALEMOKAJELDMFNDHLJLJKKPLEFAJKIFGCAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAAPOJDGMECDLCAICECKMPNIMALOLEGFIDCAAAAAAAHABDJAJFCKLAMCBFGBNDHGOPDFBDDGAHNDILLHNHJBMEMDLHHLGLGLLFODNBJCMFBEAAAAAAHKGABLHEBFJFFHFDPFOLCIJPIJLEFKAOELFFBANM";
                        break;

                    case "SLU011837": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPFOBOEOIJOPHGCEFJGCCBMFMHLOBJJLJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAMJMJPFKGGGHBJEKHIGGFAAMJLAIAAHIFAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALHCMAFAAJMFPDABBPGABBGEGLLGHNCKFCAAAAAAAGJAOGCBMMCKLBECODNJIHIFPNANDJMGCFJCKKCAJCMMDDPINOEMBFOLKEKKDIGGJBEAAAAAADPGLDAMFMPLIHGJEDBHAILEJPIBIBOHGKIJDKOOE";
                        break;

                    case "SLU002760": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAOIKAMFPLIBLJKHEBKLMNEPHLFFHCEIBDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHNLLKPOIOONLKKILNLOPJANKNJPKMEFFAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFHAHNMBCGDDCLCMBFLCJILPIBEHDKJOBCAAAAAAANJENAEKMIAJCBJKFILIOBMIHNHFEGJACLCGOPNIECBHIBJAIDEJNNKMCNHFEEJDIBEAAAAAAOICBKIEIDFDNPKANKGALEPAOMNLGHLPKADPBDFKM";
                        break;

                    case "MATLOU8470WW7": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALHCOFDGPCNONAMEDJDCIAMKCOKLCFBPFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALEMHDHMGIBDAABNKAIGKCNOGBNGPMGPMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPCBCHFGIDGBJLBKJIPIBIIJJDCHIGKLNCAAAAAAADFLPCANNKMCACJDOJMHMOALMGALBDEPFMGPJDNKMGHJCPFDINJMLACIMFBPJAJAOBEAAAAAABHMOGBLFINJLNABPLOCMPMHHKHPOKJBFKMOIKHHC";
                        break;

                    case "ARTDATA-TFS": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAENFHPOKFCMDLPPEIKKMGLNPOKIBMBJBEAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAEFKFALIDHCHBHODNHOIBPIHGAHMLNKLAAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABFGDCPELHDMDPBOJLOKLGCKNMJIJGIALCAAAAAAAMEEJPGIIIFOGFMNHFLNGKOIFBJANCOKGADAHIBIAIELFKODELHFNKEPJCMJIAKAEBEAAAAAAGOJIOJDGPMCLEPMCEDGNCIBIHDILJENDCALDBCPN";
                        break;

                    case "SEA0105WTRD2": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABCDHLPKLHGBGOOEJKJIFHDOAFNEEFMNKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACJFLKEJAJCCGAPFPENABPCDNMIFKAJFOAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAEENFMCHAIANNNOCKKDBANDADOJJAGLFCCAAAAAAALBNCEJLHFLHMNJMCLEJCOOHPNMCDNIBKJJPNLHJBLKGCJNIJJCCBBFHFHLPDILKABEAAAAAABJJFAKMCMAFLJDOKOAKNPKJFEHFPAHFLIBIMGFPO";
                        break;

                    case "SLU004994": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGEAILBLPBFFGIEEIJEBFLGKELEJGNAODAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFHNLLFCDEMEEOCCPMMKKFCFAMBDKFJFDAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPBIAMOCKBDDLFGGIMBOHGHJJIFNKPMGDCAAAAAAAGDHGONFJEGBGDNGAMENAIIGJMPNFNNHHAMEBKGPEGCMNJCMLGLIHEAFHGDIJBJAIBEAAAAAAFCNOADGNFAGADOPCPDGNEBKENEENNGJDINMHFDJD";
                        break;

                    case "SLU011896": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAFNEPGOOEOHKKGCECIICFNMJLKFBFPGCCAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAKGEBELODOFFLFFBMFLPPDNBDADONGFCBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAMHPFFPGNGBFEDHJHAIMFFEKGIEHMMPPACAAAAAAANKIDDILCCPHJMLJPCDMMHGMNAAGBJOKKKEPOAEBDBFKCHNGGKLBNHGJCFAJJOMLIBEAAAAAAONIHADPDKHLCIMMBIBGIJFHMJLBMNPDLIGKGCOKB";
                        break;

                    case "SLW-DEV": // Team Two Blueberries test server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHNPAHHIMEHMIBIMNJOBMGMFCEHLENFMGAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADMLCEHFNALNIJKCCKHNJNHDCIBPJBADDCAAAAAAACLCKHIFABDFBANBOBJCBIGHJJEFNGFOLEHNGDLHGODEMNLPALLAFLBJBBMKBMMOKBEAAAAAABLALIEBIAADPJMIJABFDANMGFAIOKFPHIMKLJLPL";
                        break;

                    default:
                        throw new ApplicationException("Unknown web server " + Environment.MachineName);
                }

                return cipherString.DecryptText(password);
            }
        }

        /// <summary>
        /// Get status for ArtDatabankenService.
        /// </summary>
        /// <param name='status'>Status for ArtDatabankenService is saved in this object.</param>
        private void GetArtDatabankenServiceStatus(Dictionary<Int32, List<WebResourceStatus>> status)
        {
            Boolean ping;
            WebResourceStatus resourceStatus;

            ping = WebServiceClient.Ping();
            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = WebService.Settings.Default.ResourceAccessTypeReadSwedish;
            resourceStatus.Address = WebServiceClient.WebServiceAddress;
            resourceStatus.Name = ApplicationIdentifier.ArtDatabankenService.ToString();
            resourceStatus.ResourceType = WebServiceBase.GetResourceType(ResourceTypeIdentifier.WebService,
                                                                         (Int32)(LocaleId.sv_SE));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.sv_SE)].Add(resourceStatus);

            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = WebService.Settings.Default.ResourceAccessTypeReadEnglish;
            resourceStatus.Address = WebServiceClient.WebServiceAddress;
            resourceStatus.Name = ApplicationIdentifier.ArtDatabankenService.ToString();
            resourceStatus.ResourceType = WebServiceBase.GetResourceType(ResourceTypeIdentifier.WebService,
                                                                         (Int32)(LocaleId.en_GB));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.en_GB)].Add(resourceStatus);
        }

        /// <summary>
        /// Get status for Artportalen database.
        /// </summary>
        /// <param name="status">
        /// The status.
        /// </param>
        private void GetArtportalenDatabaseStatus(Dictionary<Int32, List<WebResourceStatus>> status)
        {
            Boolean ping;
            String address, informationEnglish, informationSwedish;
            WebResourceStatus resourceStatus;

            address = null;

            try
            {
                address = ArtportalenServer.GetAddress();
                informationEnglish = null;
                informationSwedish = null;
                using (ArtportalenServer database = new ArtportalenServer())
                {
                    ping = database.Ping();
                }

                if (!ping)
                {
                    informationEnglish = WebService.Settings.Default.DatabaseStatusErrorEnglish;
                    informationSwedish = WebService.Settings.Default.DatabaseStatusErrorSwedish;
                }
            }
            catch (Exception exception)
            {
                ping = false;
                informationEnglish = WebService.Settings.Default.DatabaseCommunicationFailureEnglish + " " +
                                     WebService.Settings.Default.ErrorMessageEnglish + " = " + exception.Message;
                informationSwedish = WebService.Settings.Default.DatabaseCommunicationFailureSwedish + " " +
                                     WebService.Settings.Default.ErrorMessageSwedish + " = " + exception.Message;
            }

            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = WebService.Settings.Default.ResourceAccessTypeReadSwedish;
            resourceStatus.Address = address;
            resourceStatus.Information = informationSwedish;
            resourceStatus.Name = DatabaseName.Artportalen.ToString();
            resourceStatus.ResourceType = WebServiceBase.GetResourceType(ResourceTypeIdentifier.Database,
                                                                         (Int32)(LocaleId.sv_SE));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.sv_SE)].Add(resourceStatus);

            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = WebService.Settings.Default.ResourceAccessTypeReadEnglish;
            resourceStatus.Address = address;
            resourceStatus.Information = informationEnglish;
            resourceStatus.Name = DatabaseName.Artportalen.ToString();
            resourceStatus.ResourceType = WebServiceBase.GetResourceType(ResourceTypeIdentifier.Database,
                                                                         (Int32)(LocaleId.en_GB));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.en_GB)].Add(resourceStatus);
        }

        /// <summary>
        /// Get status for database.
        /// </summary>
        /// <param name="status">
        /// The status.
        /// </param>
        private void GetDatabaseStatus(Dictionary<Int32, List<WebResourceStatus>> status)
        {
            Boolean ping;
            String address, informationEnglish, informationSwedish;
            WebResourceStatus resourceStatus;

            address = null;

            try
            {
                address = HarvestBaseServer.GetAddress();
                informationEnglish = null;
                informationSwedish = null;
                using (SpeciesObservationHarvestServer database = new SpeciesObservationHarvestServer())
                {
                    ping = database.Ping();
                }

                if (!ping)
                {
                    informationEnglish = WebService.Settings.Default.DatabaseStatusErrorEnglish;
                    informationSwedish = WebService.Settings.Default.DatabaseStatusErrorSwedish;
                }
            }
            catch (Exception exception)
            {
                ping = false;
                informationEnglish = WebService.Settings.Default.DatabaseCommunicationFailureEnglish + " " +
                                     WebService.Settings.Default.ErrorMessageEnglish + " = " + exception.Message;
                informationSwedish = WebService.Settings.Default.DatabaseCommunicationFailureSwedish + " " +
                                     WebService.Settings.Default.ErrorMessageSwedish + " = " + exception.Message;
            }

            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = WebService.Settings.Default.ResourceAccessTypeReadAndWriteSwedish;
            resourceStatus.Address = address;
            resourceStatus.Information = informationSwedish;
            resourceStatus.Name = DatabaseName.SwedishSpeciesObservation.ToString();
            resourceStatus.ResourceType = WebServiceBase.GetResourceType(ResourceTypeIdentifier.Database,
                                                                         (Int32)(LocaleId.sv_SE));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.sv_SE)].Add(resourceStatus);

            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = WebService.Settings.Default.ResourceAccessTypeReadAndWriteEnglish;
            resourceStatus.Address = address;
            resourceStatus.Information = informationEnglish;
            resourceStatus.Name = DatabaseName.SwedishSpeciesObservation.ToString();
            resourceStatus.ResourceType = WebServiceBase.GetResourceType(ResourceTypeIdentifier.Database,
                                                                         (Int32)(LocaleId.en_GB));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.en_GB)].Add(resourceStatus);
        }

        /// <summary>
        /// Get status for observation database.
        /// </summary>
        /// <param name="status">
        /// The status.
        /// </param>
        private void GetObservationDatabaseStatus(Dictionary<Int32, List<WebResourceStatus>> status)
        {
            Boolean ping;
            String address, informationEnglish, informationSwedish;
            WebResourceStatus resourceStatus;

            address = null;

            try
            {
                address = ObservationsdatabasenServer.GetAddress();
                informationEnglish = null;
                informationSwedish = null;
                using (ObservationsdatabasenServer database = new ObservationsdatabasenServer())
                {
                    ping = database.Ping();
                }

                if (!ping)
                {
                    informationEnglish = WebService.Settings.Default.DatabaseStatusErrorEnglish;
                    informationSwedish = WebService.Settings.Default.DatabaseStatusErrorSwedish;
                }
            }
            catch (Exception exception)
            {
                ping = false;
                informationEnglish = WebService.Settings.Default.DatabaseCommunicationFailureEnglish + " " +
                                     WebService.Settings.Default.ErrorMessageEnglish + " = " + exception.Message;
                informationSwedish = WebService.Settings.Default.DatabaseCommunicationFailureSwedish + " " +
                                     WebService.Settings.Default.ErrorMessageSwedish + " = " + exception.Message;
            }

            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = WebService.Settings.Default.ResourceAccessTypeReadSwedish;
            resourceStatus.Address = address;
            resourceStatus.Information = informationSwedish;
            resourceStatus.Name = DatabaseName.Observation.ToString();
            resourceStatus.ResourceType = WebServiceBase.GetResourceType(ResourceTypeIdentifier.Database,
                                                                         (Int32)(LocaleId.sv_SE));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.sv_SE)].Add(resourceStatus);

            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = WebService.Settings.Default.ResourceAccessTypeReadEnglish;
            resourceStatus.Address = address;
            resourceStatus.Information = informationEnglish;
            resourceStatus.Name = DatabaseName.Observation.ToString();
            resourceStatus.ResourceType = WebServiceBase.GetResourceType(ResourceTypeIdentifier.Database,
                                                                         (Int32)(LocaleId.en_GB));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.en_GB)].Add(resourceStatus);
        }

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <returns>Status for this web service.</returns>       
        public Dictionary<Int32, List<WebResourceStatus>> GetStatus()
        {
            Dictionary<Int32, List<WebResourceStatus>> status;

            status = new Dictionary<Int32, List<WebResourceStatus>>();
            status[(Int32)(LocaleId.en_GB)] = new List<WebResourceStatus>();
            status[(Int32)(LocaleId.sv_SE)] = new List<WebResourceStatus>();
            GetArtportalenDatabaseStatus(status);
            GetObservationDatabaseStatus(status);
            GetDatabaseStatus(status);
            this.GetUserServiceStatus(status);
            if (this.IsUserServiceStatusOk(status))
            {
                this.GetTaxonAttributeServiceStatus(status);
                this.GetTaxonServiceStatus(status);
            }

            return status;
        }
    }
}

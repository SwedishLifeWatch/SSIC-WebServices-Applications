using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.Database;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Database;

namespace ArtDatabanken.WebService.TaxonService.Data
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
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANCBCGBGHOECKBMKGHKBABOGIIIEEIGDOAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJICJPIILIPKAHBNMKOIJNDGIAACIJEMOIIAAAAAAGDHEEMPIOCBHHJKCBELAJDFCFAPIJIEJOCDEBCBKHJGCFEEKJHNPNHNAJFPKPJCJHFBFMMMOHAGEAONLAJIPDIAMECJCMBKMPDPAEKGOEMIIAJJCHEGKNCMDEKABMDBLFMMJDMBBECFEHCCOIENLHLFEBAKFHPBNAPKANCJGJJFLFGKPPHDELCPFPELFFHGOBPDHEDBIBGLNEFACAMCHHHOCHOPJFCLMMMBDEIGDKPNKHMAJACBJJCBMFBMAIJODLAAHPMGGBCGGCJHKBEAAAAAAAOINMHKFBHNCLHIPKCKKOIKPKGDOCLBMLFDDJKIE";
                            break;

                        case "ARTSERVICE2-2": // New production web service server.
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACPGPNOLBNAMBAMAGHLNEEPPFHFHHBKBCAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAACLPKKDACFNBAGINAECHNHIOMEHOFACANIIAAAAAAHDHFAGHIIFOOJIOPDKELGCOGJBFMIPDNEFNFOGAAGIBJGEALLEFLHNCHANJLKCGLDCIBLBIBBAJCMDMEFKJPCNPAMKKCGBCBLCIDLBFPJIKGKGIBAGCJDPGMCIANPKPCGHGMMFLOGHLABDMCLBLMPGDKKKMOEILBCDDLDDBMHPBHMPCKHGIOCAHGCKJCENFJDBBMDJENJEMHGGFBFDMNIOFFOFJPGFLHKJICEAOBLOACDGGFEHMHGJCOIGFHOOCANEKMCPNOCLMAICDABEAAAAAAPHINKDGLLLNACLAMDBCICIJDEKOKPKHMFPJLIACL";
                            break;

                        case "LAMPETRA2-1": // Production Web Server
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGCAGEOCJPNEJHLENJFHBFDJHAJKOHFFGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALHGKKOJPCELEHBJCOEFAPHMKLPBDFMANAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALNIMKHEGCIFHDOMLNFFENMJFKEPIJKEGIIAAAAAAPMKKEOIKOAGIDAIAHPLIJLALCKGOIMINMPLAHAPHNLLCCNKJAHJFFEPAMMEBBPJCMHENIAEPNCILLBBLKJBHDGNNFLAIFLDDCOPGLPBLCBFBIMFEGNALCEGIKJDLHJPHJNBOBAHADANCIOOONALGIDMPLAIFJDKGGKIJGLFLIOOMKCLLNHCEJCCEKAINACBKJPMHACABHECEJGFDIAMDHLBNPBPKHMDKAJIDAEGDNIMPHJFNNCDMBNIDOHNNNLILKFGIOMBCCKAJDCLNBEAAAAAAHGAGHIEFLBAAEIHDBACNBELCJPIGNEHIACLGOJMB";
                            break;

                        case "LAMPETRA2-2": // Production Web Server
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAMCBDJGPBBGNHNIEGILIAJMMMNJJLJIPGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACIENLPCGLBMNKDHMPBPDOCALBIDNFLBIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAIIAHLLHNCIAHNFANKMKCDGONHALABJFEIIAAAAAAHBIPIFFKMOBMADIIGCIIOPOACJDAPFHCENEPOGNHNNHAIIFCONABIPKGDFECEINKJJMHJNKNIJBFFMPEMAKJPIAKEMHCLDKEAJGOMCJOMBFOOADOEPMKHHGMFBJDAKOFHLPJHMJFCANFMAGADIDBBIILOENIPLFJCGABJICHENMOPCKEBIKFHEBPOFOOEPOLPJBAJDMMFMFMKBAJBHKNLOAHHNOBPAHELHPIOFFKHPKGLABHLDLAADDCKNGCKDLJLNLANEHPKKPCBCJLBEAAAAAAAJPJEOJIJAJJLDGPBMLKGNHDPDGFAHFDAKKNCMPA";
                            break;

                        case "SLU011837": // 
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPFOBOEOIJOPHGCEFJGCCBMFMHLOBJJLJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANNAFADKNMGJGLOHGJAMLOIANJKBPCAAEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKMHNOMPHMGLELPIHIDKCJFAMIGCFAOPAIIAAAAAABAAMGPPGEMKKDENHKCJEGINHBEILJPIEFALBLHHHCFPGJHLKOJLCFFHBCNFCHJNKMMAIEFMMEGMBIDLMFDEHEOEDHMELDEENADHBAHOCJAHLCBACLLLPKDJLOMNELDBFNKGEDDIMEKLCBEDCCAOGBDLGDJPJFNGJADODBONFLPDLPLBJBDNFHKJJMGINMCKIJHDGBOAFJMCGCHLHPEBHDDFADFNECIBLKNPJMOPLEENEFDBEPJDGOAAIDLKNHEFFCCNHNPMNBMCPHGJHBEAAAAAACNCLBIMICKPHFNDOGKOOIODLGENCAFOPKCMKPMCK";
                            break;

                        case "SEA0105WTRD2": // 
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABKIOIJPBLDPKPCEELADDGIHIPCLMBNMGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAAOGKFHCAHPDNIIOJNHLHBNNCIBDENHPJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAACOJPNAJLGPJDOECBBIGAIFCGICEOJGNIIAAAAAAJIBHLLCBBLLEDGMDOLBHIPGGGPIOFNOAJIEDJCPJPOICBIDKDDHBDNDDLCPMHFBBHODBAHCNBFLHAFDMCEFEMAINLHOKABGPCKCFGFIALEBAPEEFHJALEILAKGKFGIKAOGPPFMGCEOAPELEPMFKFMONCJAHEDMEOGFPMOIMGNMMJBDPBFBGMKNFEHJFCJAHJEIPDDJEJIKAAOBMPICCNPLGOLJJPPANENIHINMOFMHPMFBLCMANIBKGCHGGJLHLIGHKGGNLIGPCDAPFDBEAAAAAANAKNMNNNOPLACEJFLAOPFBAAMJFGIBFOBCKAGGJP";
                            break;

                        case "SLU002760": // 
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAFODHGIOLGHKPAFECLFIMOJLFPEGMHFDDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAOLHIEOKKBGJDCAEAKLCIBAANFAPKNGNJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHAKKKGJGEFHFOMPCPHPPNEHNDNEMEHNHIIAAAAAAGCJKHCKGOKFLEKOFOACJACIMHPAADCMOENAGJEENIPJBBEBIMOILJAIJEGJCCJLPCLHMFFEIPECPBOLFCFEIMEGCLGJCMHEEEOOLFKDHJFPAOCGJAKJOLJBPDBKGHANMLDCHHICAJDLDAOIANOKNCBKCMOAEHMEHNBBOBJBGLGEMJNHDBFOAJODHIABPJJPKOKGBPPNAHLJLNBGLCLCGOJBLPAOFAOKOKAGNBGCAGDCNGEAIMFHGBODMCPNGIKIMLKOLLEDJLMOPLJKLBEAAAAAAIMKLGIGDDHOKPDMBILHIIOFLMLINLMCKHMPIKIMN";
                            break;

                        case "SLU011730": // 
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGKOONNFFCICKGAENLBBKPOHHMCHDGMEDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACBGPNEBADEHFENIJFHMKHOCBPNKBDMOEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAAHMIHKEJBBLALBAONLFJOANJKIODKMAFIIAAAAAABLLFFGOJDEMKIKHHLOGGMCMGCLHOMKIONEMFDNOOHHPPDAEJABBCNCCBHBMMFAFMKGPHMEDIAOFNGGDGAMOBGGAHEHJFHLCFPMCPJKGBELAAFBPODJLMABBJDAOGPCNHFCMKJGJHPINANNEBMIOMGJCIEFMPMFBLBOOEEOONEGKBIDGJKEDCEBHPDMKEMHOFAOEMCAKBDCEIIPPNOCLLEMDHADHFJFJNOCCCBCPMLLIBLMJFKOAMGIADKHMNGOEFNLNNHMECHFCELJGEBEAAAAAADDBNIFADJPFMBLFAHGLPEFKNEHIKNBKABKIBKOJB";
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
            get { return ApplicationIdentifier.TaxonService.ToString(); }
        }

        /// <summary>
        /// Web service password in UserService.
        /// </summary>
        public String Password
        {
            get
            {
                CipherString cipherString = new CipherString();
                String password;

                // Opens the database connection.
                switch (Environment.MachineName)
                {
                    case "ARTAP-ST": // Team Artportalen test server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGOCKLHDJNMOKAHENLNFMCAIBJFNLKGFFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAOILIBJPFMLPHKIBCCBINHDIFKCLNLGMPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAANNAGPJLHGBNEHIDFIBGEHEMLEODNOFGCAAAAAAAGJKIOGIKFLPCKFOOKKOBLOFOBAEKKCMBHMOEDABGDBMIAJBPDGLIPEGAFDCLNPPKBEAAAAAAKKJOGLLLFNCFBHHMHOIECANKEHLIJALPPAMNBMGB";
                        break;

                    case "ARTFAKTA-DEV": // Team Species Fact test server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAMCNBEPNIEJBJPGJFFFGADIGNCKMIGGMNAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAOOONDGGKGNFHLKDKPCHFDLKIPKHAGPOACAAAAAAACDNKLHCIPLOKNLMCPBDBOOAOJHMFKLNFCFLCLODCFDGFKAOEBFNLOBAAFMAOPOPPBEAAAAAANEHCGFIEMGNCNPPEECIGIJGHKBBLKJCABAJLJDIL";
                        break;

                    case "ARTSERVICE2-1": // New production web service server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALBLOJGDFAGCGFIEJOJAIGLFFHAGGNGAGAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAACEFJKIHAKAIDEJABOPGGAJGIOGNIHOAPCAAAAAAAHDCGMFJLEHKCMJFBLIHDFKABGGLIMJHNIKOBCNAPBJLCIDLDPLHHGCLCLEMECGOBBEAAAAAAEMPOEMBIJMOIMIAOGGLNGGGNNKMNNKDACJEAHGAJ";
                        break;

                    case "ARTSERVICE2-2": // New production web service server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAKPPDPJDJJCAHNGPJLPINGBMLEMBOEMOJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAOLBHBJAOOFHNNPFNHGIKJDCIMHOPEFJHCAAAAAAAHGLFBJCLOKBOMPBMPDKNPKCMOEOHBOMMFMPNAJLJMLAIAMODINOGDDBMBIJDKAIEBEAAAAAADCNCIJGFABBKOHBLAGBCFKPPINHACPNJOACODHIE";
                        break;

                    case "LAMPETRA2-1": // Production Web Server
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEFNCNMALBJAAPDEFJAELMCDBLMLKOIAPAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALLGJGBOAKGADCMDPMKKLJIGJDCPIIKEJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAOFBCBGOMGPLHHIHEIPCADCOGFHCAEFCACAAAAAAAHJOEPAFMPLKPDNBLEFAHFAOGANOGFLLBOHGMFLALBOILGLBCMHLHEBHAOALJLIOHBEAAAAAAPBHEJLKCPIDHOCNLPGAELCNJMJCKCNMLKBHKNKJO";
                        break;

                    case "LAMPETRA2-2": // Production Web Server
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAACNLJDFFELFNJJEEDLDFBHKHAEHCKNCDOAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAEDNJIEAHLDFINOEPOLPANLAIKHPCFEOKAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAACJOFMAHNENKIOJCLCFHICDMCPFEIPOIDCAAAAAAAOCLIGDKLDMGJAGGHIEPGLBAOLHLNAOPJAFIJHDKCIFFJKDJHNDGALDLDJGEIHAHNBEAAAAAAFOKJFNCAKGPOIGEBBNGFPOFLDFJKGOECEGNGLLHN";
                        break;

                    case "MONESES-DEV": // Test server
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEAFPECBBEFEHIKEGLFFDMHECEONJDGANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAEHBJEJPPKOAEHFJJFNFKHDMJELLLGAJIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHENAGJAPFJCHGAIINOHENHOECBCLPFNACAAAAAAAFBKCLEPFCMHCFHPEGNBAENNAMCCLFAJJICJGOHHALAIAKLKJIKEPDNHOBJGOMPLMBEAAAAAAFLGKEFDDPFJMLCFKNHGFHCMOIFKDMINBHBIAJKFJ";
                        break;

                    case "MONESES-ST": // System test server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPFBIFKMBPOAAHCEDPEKPCGGNPICDPDPKAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAACEDNNICHILLJGCACMMBEGAAJJNLGFLGECAAAAAAAAHJJDCCOCDFBPDNMCPOEDBHFAMAPANPKPBDNAOCJAJGLFOFOKDPBDEGKFLFOMDEKBEAAAAAAMHAPMPIFCECILOILDEGKLBGNJBMKBAOJEOHIDBAJ";
                        break;

                    case "SLU011837": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPFOBOEOIJOPHGCEFJGCCBMFMHLOBJJLJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAICCIKHFCADBALABDLEFFNCOOOMMEPMAKAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAOPINGNKHJJFKJGFDMFENHNNGKFGINOKACAAAAAAAJKIKICJFCECLBAOAHNBJBPAHCLNEEADPJNNMFOLNCGDKNKEBDGMNNAPFFNOEECOGBEAAAAAAKIDDDDPIGNDPGHPOAHBKOHMCDMPEJJBCOENHGBKE";
                        break;

                    case "SLU002760": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAADMPFAOEONEELJBEJLDHDAOACGBNJFCIFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPDKJHFKECLECLMFJMKDHCGEEBILIDKDMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKEGBBMDADCPCJCMBCDKCLPBKAPBNPELMCAAAAAAANJEGCLIBLODFGOOEKAIPFICNBNFOCDAFOOMOIJOAKEDGNPMBJIICJNCEBKCMLEGIBEAAAAAABHOLFDLMFLFDNEMAMCPJNCAOIGJMNHJAGONLANFF";
                        break;

                    case "SLU003354": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGLGBDIAIDBHFHEEKIOMMGMEHJFINCANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAABAOBGFBHGBALKEMOPLKOGGGPHNIGLHKMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAENNHBADICPPDCKJFPCOLOJBKFOEDJIFFCAAAAAAAHPJGFKNPCKBKPFEHKOIGMNKHGJNJOMILMGOAPNFHIDNKDMACMFNADBGFJNHMCKEFBEAAAAAAOAICNDIHPOAAJIJDOPNCMDHPPKENGMALBHCPELDB";
                        break;

                    case "SLU002759": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAHJPABDBOIBDFPDEIJCANFPFNLFEHMPODAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJBBKFOPINJPPAECMIEPIKIGMAJOJGAONAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFBPOHILJCALMEMGDHOBNGOODPPGIMNFGBAAAAAAAKFOCEHDIDMJPNHIOPKAHKHDFDILBNCFNBEAAAAAACEAEFNDBHFEJMDKNIHBGBKPGEMAFOFHPCLALHFJF";
                        break;

                    case "SLU011730": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGKOONNFFCICKGAENLBBKPOHHMCHDGMEDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAMNGLPFBENPADHNENGJOJGNJLKEDBPBLLAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAAHAJDMONEAPNONHCGLOJBJHCAMJHFONNCAAAAAAALBJPEMEKPDLNAKJKDCAIONDHBHIIKHIOCJCHEGPEGAEOMOLLMHKNPLNAPHBNGFHBBEAAAAAAMDLJOMJHCGEMKDBIMOKGALHDAHDJJGJEBOLCPHDB";
                        break;

                    case "SEA0105WTRD2": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABKIOIJPBLDPKPCEELADDGIHIPCLMBNMGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAGKCAICLCEDJJFMENPHKCIIDJDAOIKPBDAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAOJAGLMJKBOPBPCMPAIPEMDKMCKKEGHJBCAAAAAAABELKLDBOGJHHAJFECCECCLHDAEJDNPIJLJDDOEFCJPGEIOMDOKEIECAIGPNALLFMBEAAAAAAKJLFGABKAMFDBDOAPFAEDAJBDNBAJMCCEBLKFJBO";
                        break;

                    case "SLU011895": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGOMDGCCAOKJOJMECJEKHFDIAIPDOIEMOAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJAMDMODBGNNLPGKFANODBDLNKDOCHNKGAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHENKPMFDJHKMGHNJIPLJLBIFGODMMFBPCAAAAAAAJHMFFEEHGIMPCAFGCAFEEHPOHJGMDPCPDDKGFGAINJPOHBMNFIFIDDPHGHGPINHPBEAAAAAAMFEBOAPCDNCEFOACOAPEDCHOJFJGENCIPKJNOGIH";
                        break;

                    case "SLW-DEV": // Team Two Blueberries test server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHIPDKGONGOEOOFKKFNBFDOPMKJIACKNAAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABCPDMJDCAJBHILJCIIAPHIBOOHHHCAIHCAAAAAAAFEEDNLCDDPAMKPMNNBJAKEMGLLHHECOLLPKDOCGKGCIGPGIEHGKGHLEILBBHLBNEBEAAAAAAOLAFMIDCJMBECIJMNJBFICDEGFLIGHMCHKAIOEJP";
                        break;

                    case "SLU010576": //
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIBCGMONNPLJJELEOJBIEMOPHMHBPHABPAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADELJEDOONILIAHLOCGLACPFOLMLJEHMEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABACOIGGIDOLIJBBGJFBAOKDEGHENMMBDCAAAAAAABAFAEGJHFNHNFFKAGPFHLFCMDOEDKHLAFBOPIEPNAACCLDEIENPDIFALKEKJAGAKBEAAAAAAKNKMONIPBEKCOOIMOPANDNOOIDOHBBOLIALIJBLJ";
                        break;

                    case "SLU012925": //
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIPPPDJLGOBFNIKEAIDCFOGJFLFHBHOLNAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALKILMBHAKDNFHPHMMFLDKJACJDINELOMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAACJKFGOOFEOBBHCKNBDMPLNHOOLOIPIAMCAAAAAAAJMPEBMJMLPIAENBIGDANDLNNNBEFCPBEAHKCHAOOCHNFHNIDPGDJGEEACOBMGKMCBEAAAAAAJCDKDJPFBIIJFJKFNJNMEKKADFGGCHDNGNHDOCFP";
                        break;

                    default:
                        throw new ApplicationException("Unknown web server " + Environment.MachineName);
                }

                return cipherString.DecryptText(password);
            }
        }

        /// <summary>
        /// Check for circularly dependency.
        /// E.g. TaxonService only has problem with ArtDatabankenService
        /// and ArtDatabankenService only has problem with TaxonService.
        /// </summary>
        /// <param name='statuses'>Web system statuses.</param>
        private void CheckCircularlyDependency(List<WebResourceStatus> statuses)
        {
            Boolean artDatabankenServiceHasProblems,
                    taxonServiceHasProblems;
            Int32 problemCount;
            List<ArtDatabanken.Data.WebService.WebResourceStatus> artDatabankenServiceStatuses;
            WebResourceStatus artDatabankenServiceResourceStatus;

            try
            {
                // Check if only ArtDatabankenService has problems.
                problemCount = 0;
                artDatabankenServiceHasProblems = false;
                artDatabankenServiceResourceStatus = null;
                foreach (WebResourceStatus resourceStatus in statuses)
                {
                    if (!resourceStatus.Status)
                    {
                        problemCount++;
                        if (resourceStatus.Name == ApplicationIdentifier.ArtDatabankenService.ToString())
                        {
                            artDatabankenServiceHasProblems = true;
                            artDatabankenServiceResourceStatus = resourceStatus;
                        }
                    }
                }

                if (artDatabankenServiceHasProblems &&
                    (problemCount == 1))
                {
                    // Check if ArtDatabankenService
                    // only has problems with TaxonService.
                    taxonServiceHasProblems = false;
                    problemCount = 0;
                    artDatabankenServiceStatuses = WebServiceClient.GetStatus();
                    foreach (ArtDatabanken.Data.WebService.WebResourceStatus resourceStatus in artDatabankenServiceStatuses)
                    {
                        if (!resourceStatus.Status)
                        {
                            problemCount++;
                            if (resourceStatus.Name == ApplicationIdentifier.TaxonService.ToString())
                            {
                                taxonServiceHasProblems = true;
                            }
                        }
                    }

                    if ((problemCount == 0) ||
                        ((problemCount == 1) &&
                         taxonServiceHasProblems))
                    {
                        // Circularly dependency detected.
                        // TaxonService only has problem with
                        // ArtDatabankenService and
                        // ArtDatabankenService only has problem
                        // with TaxonService.
                        // Break dependeny.
                        artDatabankenServiceResourceStatus.Status = true;
                    }
                }
            }
                // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // If exception occurred we probably don't want to
                // change status value from false to true.
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
        /// Get status for database.
        /// </summary>
        /// <param name='status'>Status for database is saved in this object.</param>
        private void GetDatabaseStatus(Dictionary<Int32, List<WebResourceStatus>> status)
        {
            Boolean ping;
            String address, informationEnglish, informationSwedish;
            WebResourceStatus resourceStatus;

            address = null;

            try
            {
                address = TaxonServer.GetAddress();
                informationEnglish = null;
                informationSwedish = null;
                using (TaxonServer database = new TaxonServer())
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
            resourceStatus.Name = DatabaseName.Taxon.ToString();
            resourceStatus.ResourceType = WebServiceBase.GetResourceType(ResourceTypeIdentifier.Database,
                                                                         (Int32)(LocaleId.sv_SE));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.sv_SE)].Add(resourceStatus);

            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = WebService.Settings.Default.ResourceAccessTypeReadAndWriteEnglish;
            resourceStatus.Address = address;
            resourceStatus.Information = informationEnglish;
            resourceStatus.Name = DatabaseName.Taxon.ToString();
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
            GetDatabaseStatus(status);
            this.GetUserServiceStatus(status);
            if (this.IsUserServiceStatusOk(status))
            {
                GetArtDatabankenServiceStatus(status);
                CheckCircularlyDependency(status[(Int32)(LocaleId.en_GB)]);
                CheckCircularlyDependency(status[(Int32)(LocaleId.sv_SE)]);
            }

            return status;
        }
    }
}

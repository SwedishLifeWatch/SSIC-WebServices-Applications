﻿using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.Database;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.AnalysisService.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.AnalysisService.Data
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
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAALBNKBCAMFDOKKFNACCOJAGDAKCLEBDAAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAMPGHPHIBLEAIALGDONMKMDDHNOJMPIGNIIAAAAAADAAOJMMFHDPPCDJGGOFGFPGEHJLMHNKONIEMGBCMAOJGCFDPLHHFNJLNLMPEMBBNFMNABENJOIEPAKDNJJCMIEEFCHDEFJPPHDNNHGALJOGELHFDDNCMPMPJIFEMMOMMFDOOBIBPKLFMKGGCGMEAFPKKODAAELAMMNLBFNKDHLHILBNBCGMLCPPGJELCGIPKCCJPKGNKIGDEBPGOPEPAOMDAEJOGBBHLGLDFKBGJDJHJMANDBOPDGIENBKIDAGMJCKKEMAEBAOEBHJBPBEAAAAAABAIPPDFABHLNFIKIIMHICCAMLMKAKHAMEHFILKOM";
                            break;

                        case "ARTSERVICE2-2": // New production web service server.
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALDCCCFKJHEFNLJBNHKMKECKHKENIPFIDAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAACLMOJBIOOKKJKJCNLOLLNLJBKJNLECINIIAAAAAAOIACBDAADEDNBCLJBHFDKKIJMFHJPCMHFFMLBFFIILIIELHPGLNIAPAHGKGPJHPICLIBHJDDIAFPNLEJIHFJFBGMABEHIMMDFEHIFAPHNJKKFADCEBGKMMNEPDCLDAILMIBOENNBGEBGJEIFIEKAIJCHGOLBEEDBJHAKIKIBGMHPNPHGFPEPKMCEMGCCOHFOHIHJMJLKOIANEDMNHEJBIOLLLNPAHDILJDHGDBJCENIGDIDIFDGMLOKOPOFCOLKICHDLFOKLMCOBKKOLBEAAAAAACKPCJJGJCFGHKGGMPHKGMFJLFJIKGEHGNHEIMDCM";
                            break;

                        case "SILURUS2-1": // Production Web Server
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAKCDIPIMNCHNKMLEDLNNLHANAPIJOPGEOAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALGNCAACIOFGBBHKICBODGPPEMEHGIAJEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAANGABHPPNICOLIMDOMCJHOIMGAEAMEIBJIIAAAAAAIPFDLJDACJDHIOJMGDCPMDOCHLEHHPLNDANIODJGEIEJADCDIGJOCLFCGDEFINIAFGOFPCEEADFADGPJKJKEBPPOPGNMNFMPEENDGOFECCNIOPOLNLKDFBGAEBBHCNMKDGMDGKAENGKODEMFBOAMPMDFPCCPDMEHOMDPGNNEKKONJCHGGBBIDLLLJEMMNBLILFOINJFHGJPOOHJPNPKGAKMFMBDGOOMJBKLPNNFLLOADGOKMKFJMHHMDKEEAADFLKEEMNBGCPOIGCHMFBEAAAAAAMFLLDOBNLNIHHNPGAPCMLPIPOMPFNDNPBPMGACIH";
                            break;

                        case "SILURUS2-2": // Production Web Server
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAADJCPFBPDFGGAJPECKJKGADMIIFAPGPOLAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPGOGNENKPHLDMAOFGKNHLNMCJHFBEFKPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAANIDGCIBPAFPGGFPBJLDPNNHJOKOLLAOKIIAAAAAADIMIIJDAFMCIFOMCBOAJGHIIGDLBDJHCKMKGEDIABOKAIIPFJIIGGLNPIOMJHIMIOAGMLMMGJACNFLCCBEAGKHMGOKMHEANHJIJNBBOIDIIDJFKAHBMCMBFKGACEENNJHANNKJKALPAGLAMDHEJAPBBMOLNBBKJEIOKFOCOHBAIMHLIDFGLKKMDPGFDLEBNPMHBECILHGNJEFJDLFJLPFMPGGNILMKAMLEIFFFKLBMJMMKGLKLGDADMELOFIPGJLKGOIDPMDGAOLNOFMBEAAAAAAECLBNLLJFIHACGMBKNBBGMKBBJDIOBKIKGPIBHPG";
                            break;

                        case "SLU011837": // 
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABGDPKNGGHAFOAMEGJJHCDBPNFPCJJBMBAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAEAAEJIMFOHGKLKKDIIIEBBAFDDHEHJGKAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAIMIDMPGDHJABMOBNNPDEMILLJKNFNHBAIIAAAAAAFLEEKJADGCOINBOCNELDIHNEBLBHJAMCLBNGLLKGDLOMKEOPGEHBCKNDBDJMIPPMGOABEHHGKJDOFNGELJGDKLLEPMLIJDMFIBLNAMPKMNPPEFBKOHMLAGJEFJJIIOGKJHHOBCKDMOLMIMLPFPKEENHMKMGOBNHGAICFCCLNIJOPJAHGHJNEDHIMGNHCCEAOECEMLCFFIPCJKJABCEELOLMAJKBMEFBJACFDGGKKKEMKGGMMHNHDABBLJPNLLIEEPCFOFBHFHNPDCDGNBEAAAAAANPNDPPMFEAGGJIAFENDCPJAJKKGEMLKFIEBBOANL";
                            break;

                        case "SEA0105WTRD2": // 
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABKIOIJPBLDPKPCEELADDGIHIPCLMBNMGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAAHFAKKEECFDIOCBNMPIGFOFAKCADJDOBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAIKEFIENAJHBAJKMKIBGGCEHLNFHOABNKIIAAAAAAJLEJHAOAMDPAAHDBOBLFICBHPKLDHPMCKOGHBDKBKOEKBLGAMAPEMNGBKJJMFCKBOGDCJPCHMGHDENFMDNKJCGAABMJHNFMOABGICHECFGEDMDJOGICJLNIJMEBNDAFIJEHHMDFJLFEBKPPEEBMCCIGMMCMOOBOELBJDCHABICJIJJONDAHJDLANGBPKEODBHNCOLJCMJABNMHOGCEKAOMBBGLOBPCAMPJOOPPKLNMMBAGBDIDNGOLIKINELDLPDBLBEPEBDMJAMDLOPBEAAAAAAGBAHFMAGCNNAFPJBGILDKBJNOABELCPGCCPJECFA";
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
                return ApplicationIdentifier.AnalysisService.ToString();
            }
        }

        /// <summary>
        /// Web service password in UserService.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public String Password
        {
            get
            {
                CipherString cipherString;
                String password;

                cipherString = new CipherString();
                switch (Environment.MachineName)
                {
                    case "ARTFAKTA-DEV": // Team Species Fact test server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACMPKCBFNLAKFIGBNDGPIIAGCNJHHJAFJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAACOEKIJKJIPLJFLKMHGHOKGBBGHLFGLCJBIAAAAAAHPKOCJHBAMKGMPDDFJBPJOGBKJNJFKDKNPMLLEOHPNCLPHHEBEAAAAAAEONCFEDPGPDHCBPCBALMGILJOCKHGLAJIOGBDCHP";
                        break;

                    case "ARTSERVICE2-1": // New production web service server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJCIDKFOMPNJPMBAPJDKAPLPKGECAHDJHAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALNHHOANJPKIAMMHKEBNBBGKCKIPPAGMEBIAAAAAAECJOPLPLCCOKEONFMAFCBDJFJGMPAMBMFCBAMFJKJOHNLOKFBEAAAAAACGJGAIPIPOFOHGBIEKDDEBFPMEBBJLBFDAEBCOFH";
                        break;

                    case "ARTSERVICE2-2": // New production web service server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPECCCMMIJICEFNJECNLIGJFAJDHGEPACAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABPMONBOKALLMOLMJDPGHNHLILKFEMDGBBIAAAAAAMMINBCGOHDNBEJBEIKCBHJOLIDAGNIIOCFNMHPCDMCBMDHOKBEAAAAAADBIGJJEEFPCKEPIICLFMJFJMCCEHKNCPAMMMHHGF";
                        break;

                    case "MONESES-DEV": // Test server
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEAFPECBBEFEHIKEGLFFDMHECEONJDGANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALAHMHBDIOHJCMMDFOOPHFBDMFBKDBMLOAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAGHOKMPIAPBMGLPIIKPFLOFAAOLOFHFBLBIAAAAAAHINJMGGMIOBFJDDOMMAHOLPDFCNGCHOPHFJOOGEGLJBBFBOJBEAAAAAACIEBFAGBFIHKJLANKACBMPEEPBNLBAOOBDGCJMJG";
                        break;

                    case "MONESES-ST": // System test server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAABHDJMLEJPOMJOMGCDFMBKMNHLAPFHANBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAEDPFMMDIHCGHEFCLMIFPAACPBMADKGMKBIAAAAAACICHJPONNBEAHEONEKDNCKOPOLPNGNLEAIAOMFPGAMEAFGMCBEAAAAAAEBPFOEDOAMDNPODKHHHKHFENOCPPLGCBGJHMKEMM";
                        break;

                    case "SILURUS2-1": // Production Web Server
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAKHKBMMIPCIPPPBECKLNLPBBFFAKLJNEFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANAEGGPBNKBOOEENGKDOGGNEJABCLELJDAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHAMHPHDOCHJGILHOLCBPMLDKABFKCJLFBIAAAAAAOGNLPEECDKIHDEEMHHHCDCJLGDIAMOJFLPMGGLEDLKGOEMMMBEAAAAAAMGPLBJGHBLLKCDAEHAMAIPBHNDAMOKLKPKAPGIAE";
                        break;

                    case "SILURUS2-2": // Production Web Server
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAFDHNJKHAOJDLKEEILDGJHHAAICHBGNBLAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPOBIJLAAENKEHEONIHPEFJEBFOEKIICEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAIIMHCGPBMNGIPLMANJMEJKHIHNAEAMNNBIAAAAAAIEMLENJBOEAMFNJOMOFPBBHJPPBHIIPOEDAPGJMKHHJLKKKOBEAAAAAAJBJIIKNAANBACANFJGPJGGJKHHDANAGBKFJOAJCH";
                        break;

                    case "SLU011837": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPFOBOEOIJOPHGCEFJGCCBMFMHLOBJJLJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJOBPPGJOEEMDGNCODHJLCDJIKOFIHAIEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAACHIAAEAPKBMMKJNHPIKCOBEJMGMJLMNBIAAAAAAEAMKBLDLHKGKEACMLLNGBDCAIBAGBDHAABIAEBFJDKEOFOHHBEAAAAAANHKMOKFJBGJNGFALMKAFBCFADFCIALLHODBACOFJ";
                        break;

                    case "SLU003354": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAOOAEJFIJMOIAMMEBIDIAGCFCCMDBGGBBAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACDDCKPDGJIAGOOMBIPLKGNEFPDOMAGHKAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAIKKAGIKPOMGBFEIJONFOOBFPAMJNDFMPBIAAAAAAKPCIHLBHDKHEGJGNHHKEMLCMNAAJHPBLOBDOKMNBKHFGLFPLBEAAAAAABJANBIJPNFIBOCAKIOBFPEFKCLEPOIKDIKKGJGOE";
                        break;

                    case "ARTDATA-TFS": // TFS-Build 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGNOIOEMFGNGPFMEILDGONEHGJJAPFOCHAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAOLDFLBDNKMEIAGBBBIBPJGHLBHAEJEKIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHDLJCCFCKAHGIGNIINFIIBBOHCPBNDHJBIAAAAAAFCDOOHLMHFGBKPNODOFPJHOENLHAANMNBJOEIAFKNGADFNDJBEAAAAAAHFFMGGMMCENKPALPBGOGJBMCGOFMDLGBHOHBABNE";
                        break;

                    case "SLU003657": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAJDELEPCAMJLGEHEIKFCPNHAAOANEPFJIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACLDIPNNMGMLCMGINLCHFJMFDDPPNGNIGAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPAHJMKGGBBKMGNBBKNMJCAONDOMLJNDFBIAAAAAAADONHLNDLILKOAFMOMPPHANPDPCFEIEIDNKPJOCOOEOPBGFEBEAAAAAADCECPHBBJHGKGOMGPHICNAJCMOPJJHEJPNGEAJLO";
                        break;

                    case "SLU002759": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAOHHDOKMHJBADKPEGINKJAACAMKIDOIIFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANIFKGJJPDNOKALBOGOEECPPHMFNIGHPDAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADOMCBHJOHLAPEGMDALFKKJKNDECAHONEBIAAAAAACBEFOKBHLHGFOKDHJECIDKNJDGPEDPMOJPMJNGGPLBFOAHDPBEAAAAAAOJIKLCCGFABIKLICGEGNIHLGLCEFKILIIEAACBDE";
                        break;

                    case "SLU011730": //            
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGKOONNFFCICKGAENLBBKPOHHMCHDGMEDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJJNOBACEMBGLOJAHPCEAJEIDMIHACFLEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFHBHHJJINAEMJCPIJLBEAOMNFFEJCNBIBIAAAAAAKGJDGJLFDMKELOGMDPNKCIJONMAFOHNFKLKNLIAKFOAPJEILBEAAAAAAHGNDFNFCECLIBDENLGHBABHKLGCLEKEGFNFFBLKC";                        
                        break;

                    case "SLU005126": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAKODAGIENDBFDOEEPKAIMHGGHOICOBJNKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFNFICCLGCHKEJLBPNDBPDNCLPACFHHHGAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHGJMDGBFEFNINDCOOOHAPPOOJCMIAEPNBIAAAAAAOBPDHCPMPODNCOIGPLGKFFMEFMLNJGHEOFPGHNOHBGHHHGIBBEAAAAAADLIJBGJNPOPNBFEAFNJPNGNCCIEOKDGDONANLEBD";
                        break;

                    case "MATLOU8470WW7": //            
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAJPCKNPEHLGFIJPEOKNHDKMHIMDFHLDCIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPJKHDLAOEINMMPNKCDDBNFBFNLBDFCIBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABKBLGHMPIEKMNLDHEBFLCGPLKHCNBHHMBIAAAAAAJOCKAOCLOLIMAFNJGOHOODADMADIOBJLHIDJJMLIAIPAFDJMBEAAAAAALEIGDKGJBGADCHOMBEJAHCAOODKPHIHIKLHIKLLN";
                        break;

                    case "TFSBUILD": // TFS
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAJDEHMIDHELNCKKEHLHOKFNFFKPMHAKNDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAMILHPHJPKADLIFAFKMJALBMIDMOHFMAMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAMDPDOCIFIFAPFMOJMAEHALBHKINHAKDOBIAAAAAAGBPLCFNMLJKGGDEBDDCIJLPJGFAOMPLCJGAKGMKDFCCKFLLPBEAAAAAANMICEPOFLBPAHOEDPBBEAOIBCJLBMJKGLGFIJJEB";
                        break;

                    case "SEA0105WTRD2": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABCDHLPKLHGBGOOEJKJIFHDOAFNEEFMNKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPEJAKPGHBELLKEAGKOHCODEGHBANPGDHAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAAKKPLHDAAFPMIABMFKLLFKCODPEKPAKEBIAAAAAAGNMAFHPJGLNFHNADFHJMJKGEEJHEPGGHFLDMHMEOCGNFIJBABEAAAAAAFOJBLGLAMLKJIIIAKJDPNGCCFABHJNAGLOMFKGAG";
                        break;

                    case "SLU011896": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAFNEPGOOEOHKKGCECIICFNMJLKFBFPGCCAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADPPFGJCOPLFBFMOHPINOHOFIHALECOADAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAAODEEANJIALBAFEFFJLFCHPBFGBDALKABIAAAAAAHHDNIJFGBJDBGJLFFOHAJDNICMFKJMJICNOCGBHBIOANHBOABEAAAAAAHOLMEJLIJELKNJMLMEIMDLFPKPOGNNFKFGFJOLIB";
                        break;

                    case "SLU011895": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGJNNAFOJJCNCANEIKCFBFEDAJKKMADANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFMLNAPCAIJPAADEIPNIGHICEFMMMDEEIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAOKBCDPMHJENAMIFHDDBOPINEHGHCINDCBIAAAAAAEKAIHAOOBAHOEBDHBJPNAGMKOFLDPHHPPGKANMNIMEOIDNNOBEAAAAAAPPCEFOJMCNDDPKAJGKHNGKGNEKDNMHDLNBAANILK";
                        break;

                    case "SLW-DEV": // Team Two Blueberries test server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACCINDPHIHFLHCKADNBEJPANFCIBMEPGKAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADOGAAICKHBPKIKAOHKBDJKNIGJLPGHEOBIAAAAAAIBGFCAKALAJLECJJGDEBJHKMGOKBNGINIFBKAAJAHGECPOJJBEAAAAAAJJMPAPIHGGGHPIGPFLPPPMNCNKJFJILAMCNHHKEG";
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
        /// Get status for database.
        /// </summary>
        private void GetDatabaseStatus(Dictionary<Int32, List<WebResourceStatus>> status)
        {
            Boolean ping;
            String address, informationEnglish, informationSwedish;
            WebResourceStatus resourceStatus;

            address = null;

            try
            {
                address = AnalysisServer.GetAddress();
                informationEnglish = null;
                informationSwedish = null;
                using (AnalysisServer database = new AnalysisServer())
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
            resourceStatus.Name = DatabaseName.SwedishSpeciesObservation.ToString();
            resourceStatus.ResourceType = WebServiceBase.GetResourceType(ResourceTypeIdentifier.Database,
                                                                         (Int32)(LocaleId.sv_SE));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.sv_SE)].Add(resourceStatus);

            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = WebService.Settings.Default.ResourceAccessTypeReadEnglish;
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
                this.GetGeoReferenceServiceStatus(status);
                this.GetTaxonServiceStatus(status);
            }

            return status;
        }
    }
}

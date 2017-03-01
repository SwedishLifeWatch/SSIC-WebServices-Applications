﻿using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.ReferenceService.Database;

namespace ArtDatabanken.WebService.ReferenceService.Data
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
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHELBILHMKLMNHBJMBONBKDGIBPJPLGIOAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABOLAICAOELLIKDHBACDGGNCHLKMJDFAJIIAAAAAAIECDHCCIFLCBKGHKGCENGEAPEPLFKGEIMNMLAIEKHDDCKDLOPLJOIPDIMPIJPEMBCNPDAEOJCCNPPLKCJGJHNPHEOIAPJPMPCHIOPBJADAOFEMBPFIICIHJPLMAHJJDHBDBBFAKMDHCJENPLAIPDOOAKJJMDMDKABHMFNBLLKNMPHBJKFJCHCBOJEIFNCFBLNLCCPNPLNBAPAFCPCODOGGONFCIIMIKJACBNJKOKGMACPNINGCJNKOFIEAFMDJJHACFDDJHFODAFAPDDBEAAAAAAGPOPPEEMHKPDAHNKNONOBJEMJGLGMGFKOOBGEHMO";
                            break;

                        case "ARTSERVICE2-2": // New production web service server.
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAABJPPDOKHMBIMLJJIOIJKEGINEAGFBDDDAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAMPANDNLJLFIMCFLCBNLJHFEBCIBLPMJIIIAAAAAAHPNGCKBLFHJMEEODNAJKJAAHIEMNOELJLKGJGJCMGIMBBMJHLNOPDJCNFMNLIADABAIBNOIGCEOOHPIPCAAMIMCOOPDCHKOFCOAABIACFPKIFIGNIAPHEIDKMIFHGLMONIDCMMIHEFEFPDAMIEOOILKMEBPPLFDPJBDBPAENNNGGBJCDDFBDDBKNKLKBMPEEJGCPIAGFNAKEBPGEGCIHFKHLFHHJBDHLHNLHAONNHNGMGECHEEMLHFHCPLFMFFOKEMIJBDFCBAEBHBDHBEAAAAAAKDMKODKNLFBMDMBDEGIFAGEGIJBAGIJEHACJGPPN";
                            break;

                        case "LAMPETRA2-1": // Production web server.
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGCAGEOCJPNEJHLENJFHBFDJHAJKOHFFGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAABDGLGJDMDBDMKKGJOALLCKKLEKAJKFNFAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADEBOBHFBIEMAMHAEMJPGJEHDHFGLJILCIIAAAAAAILJNIGELBLDJPDHBMMGPIFJEKNIIFFEPFKLANEAHMNIGFPAAELIDPNJGEJPJALHBDKALDNKGKNOMPKIBAGABNICKPIOLHBIJCPKHMOOIHDHEKIDHKHDCCJHBMCAGONJBLABLEABKAEDDLGNBBJPIFCEFFIMCKDFCIFKOHOJFJDKFOHAMJCFHHFJONJNGMMNFHFAFOGIGHOJILGODPNOFANPBJADNEDOEPAFGGHHOJODAGCCADOOFOEKGPDJNKJMELLENLKEKNENNEAEPBEAAAAAAJKECCPLBJHKNDFBKAOJPKMKDLCECIPDDMDJIFHNL"; 
                            break;

                        case "LAMPETRA2-2": // Production web server.
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAMCBDJGPBBGNHNIEGILIAJMMMNJJLJIPGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHEHLJOINGELGCLFODFEOCAPODNACDLBCAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAGLOIJCLCGDCIAJAIJNOIMPJKDAEBDLHAIIAAAAAAAFJNOCEIOCDGINKHNOCAKIGAGBPNBEMFFLCAKHIKCPFBKNEBKDAKPCMDHIOIBFACLGBBMMLBLFDMIPAPPDLBPNHGNHELJGMPCHCAEAAHAMBDEEOPKFHHBKMLCMJPEHGGFFMANBMHGDEMMMHJEJFCCCOEMJBCLMLEMECJCBHJLKELGFCJAGDPHOLMBCODAOCPFFOAOLFGNIEIOGNAKDFNGNCIDIHBGIOJEEIHFFLHGDBMADHLNECNEDPGAANBKGDCCOAFACHNONBJIFAOBEAAAAAAJNGFALPPOFLGKDKJGCPPJIMJONMAKJEMBPKKENJO";
                            break;

                        case "SLU011837": // 
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALCPBDBGCIAFPEPEILEKMLLIKEOOJMJMKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJJHLCFHJMFEGHOKKFEEJHMMNMHFLMBIMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAOEMHDFEHJJHKDHLKJDHCJIDKIPOADHKCIIAAAAAACDDHDOFPOIPJGLAIOLJPCOJEKMDLLDOKKBMOADLIHBMLNMHBAJNMLJMMOHONMPFLOEOJMLPEPLJDGHPGJDBKEGKLJOLCHKMOOMGMGNCLPKCJAONAEDCIHFIPLBFPBKCIIOCAJCCOHIPKIEGPMIODKOMDOJFPJCCKADPLKLCACHFPPGCGMGBHNLINAHOFAEMPNOBCEGNFMEAAHJCALFLLLDGBLMGKNCPDLJFNAKMMAKGPGNCJEACMGJLFJBANOBNMOFOHJHFPPFHBKIALBEAAAAAAMDMODJFJFFFMNDCNHBCMIDCACOJKEDGPJMJNIPHI";
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
                return ApplicationIdentifier.ReferenceService.ToString();
            }
        }

        /// <summary>
        /// Web service password in UserService.
        /// </summary>
        public String Password
        {
            get
            {
                CipherString cipherString;
                String password;

                switch (Environment.MachineName)
                {
                    case "ARTFAKTA-DEV": // Team Species Fact test server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADGKKMIPIADKCAONMCJGEJOFIFHEIAGGNAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALPMOKNDKICLFIKKCKDKKELBDFPLHAAHKCAAAAAAAKDFMJKLHAPKMCMBCIHNAHHKCOBCKEDNMOGNJGLOGHDFDCFFDEAJKLNHEIPLMBMFPBEAAAAAAGAEDFEAGNODIEBKGMKGOEAKFAFEGIPMLODENBNIK";
                        break;

                    case "ARTSERVICE2-1": // New production web service server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJANLIHDAMPFCGCAMBNKMCJGNCBFOCEHCAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKBNEAELPGNNEJMPNKGOFKPDDLFENLHMOCAAAAAAAEDILPDNBCJOKMFFFNMPDFGBDKDILPEFKFKCKPNKFHKGFLLIBLFDGBEGKLDHGGJIKBEAAAAAAMPEJNMOLLICLGDMLOLJOCPABLGEMDKGLKAOJNNOB";
                        break;

                    case "ARTSERVICE2-2": // New production web service server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPOOOJECEMFGAJJMBMMGAGDOMKIFGMMIAAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAOBFLIBHCOPNILNPIPMPGHMEHFHMBNDKICAAAAAAAHGJOFLCLBBHNPLFEGHFJIAHAJJKFNGOPODAFKOFKHGHAOGIGAONAEGPHMFDOJKDBBEAAAAAAFHMPJJMJJDFAGMOLEIDAHCEBDKICNIPIKHCHFDJL";
                        break;

                    case "LAMPETRA2-1": // Production web server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAJCEPODNLFCEAOPEJKAENKKFFOHNGCDHEAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAKNPCOBMCNKJJIIOIKLEOMCDFMLNMAOGJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAELBKMAFEPOMBCFMOBBOJNOGMJPILHHDOCAAAAAAAAMHMAAJEOPNMCMPMLIFGHEBFDCKHHAJHAONHEENLOPOKJBBNDLIHINAMHOJGLKNBBEAAAAAAAJFMKJDDHNFDOJLIMFNEMEMCMNDNMBEMNHKBNECK";
                        break;

                    case "LAMPETRA2-2": // Production web server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAECNHBGDNBMLGGOEILCGKDIMBCEOCDKGCAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALNKFCFKFONGOKJHGMHNJBAKBMANEHMBPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKIBCMGEFHBMHEFKHAPEEIHJBBEAGNGGLCAAAAAAAGFNFNDGAKILINHACPHFFMDFBONJLBHMECIMKPPECNJCMPICCOFNMKCEEKKAACMGIBEAAAAAAJNOHPIIHFMIHMDEODIOKJLHGILHKFLFDLEGEOBOP";
                        break;

                    case "MONESES-DEV": // Test server
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEAFPECBBEFEHIKEGLFFDMHECEONJDGANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADJFMDHFGKBKJFKOOJAIOAMPFKAKPCIOCAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHALDAKJJJHPLPDNJONBEIDPOIFKAFFEJCAAAAAAABDNHKNEDONJKLJBEGBEJGOMMGOJLHAMGLHPHBIEPBPEFIDDCIJHMAFPNOFDKFBCPBEAAAAAAIFDMHGDOBFPIJEALFPMNIBJDJMPFEBPIDGHFKHAM";
                        break;

                    case "MONESES-ST": // System test server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANNMAEPOMBDGPJDDFECHIHIMBBIFHKJBBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAODFJKNLOJEIJIENNDJHDJFOLMAMCEGIICAAAAAAACKIIPCAKDBIGLCDMGCOEKDFADDEOPELFMEPBEMMKOAJGOAPEPIDHJDMHINACDBOBBEAAAAAAJFPKKBFIKLGECLGKBBHBLPJBNIFIOLCPJIPBFMEE";
                        break;

                    case "SLU011837": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPFOBOEOIJOPHGCEFJGCCBMFMHLOBJJLJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAMJEEOOKDEIGADMMIGGMBNOCPFACAOCPHAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABGHLPPALMEANJIEJKAICKBFDFIAFCHLLCAAAAAAACFDPPEMCPMLJPJIJJGFOKGLGEICIANCPNCECAKKBIEAJGILMDLGKPBLAIIMCADGCBEAAAAAABHMNDJGDKMFDBPMBOBOKHBMBPCABMIPGHHDOPDGG";
                        break;

                    case "SLU002760": // 
                        password = null;
                        break;

                    case "SLU005126":
                        password = null;
                        break;

                    case "SLW-DEV": // Team Two Blueberries test server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFEOHNGCPBICNLKFADCODIDDMAPPHGJEKAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHLDIBHKHIMFPEEMPGBNGKHEFOKLCECOOCAAAAAAAHBGDCNCBNOGCPENGIBPCOHFHIINGLGBGFAAHDDDNDELIMEHJFECLIHKJEEHLDHJIBEAAAAAAIDAJMNIIGGKIOCCHHNMLOECGPOEPNPKMPCGHNONG";
                        break;

                    default:
                        throw new ApplicationException("Unknown web server " + Environment.MachineName);
                }

                cipherString = new CipherString();
                return cipherString.DecryptText(password);
            }
        }

        /// <summary>
        /// Get status for database.
        /// </summary>
        /// <param name="status">
        /// Current database status is returned in the object
        /// that is referenced by this parameter.
        /// </param>
        private void GetDatabaseStatus(Dictionary<Int32, List<WebResourceStatus>> status)
        {
            Boolean ping;
            String address, informationEnglish, informationSwedish;
            WebResourceStatus resourceStatus;

            address = null;

            try
            {
                address = ReferenceServer.GetAddress();
                informationEnglish = null;
                informationSwedish = null;
                using (ReferenceServer database = new ReferenceServer())
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
            resourceStatus.Name = DatabaseName.Reference.ToString();
            resourceStatus.ResourceType = WebServiceBase.GetResourceType(ResourceTypeIdentifier.Database,
                                                                         (Int32)(LocaleId.sv_SE));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.sv_SE)].Add(resourceStatus);

            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = WebService.Settings.Default.ResourceAccessTypeReadAndWriteEnglish;
            resourceStatus.Address = address;
            resourceStatus.Information = informationEnglish;
            resourceStatus.Name = DatabaseName.Reference.ToString();
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

            return status;
        }
    }
}

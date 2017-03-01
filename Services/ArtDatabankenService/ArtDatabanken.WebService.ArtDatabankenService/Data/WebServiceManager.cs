using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.ArtDatabankenService.Database;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Class that handles information related to 
    /// the web service that this project is included into.
    /// </summary>
    public class WebServiceManager : IWebServiceManager
    {
        private Boolean _ping;
        private DateTime _lastStatusUpdate;
        private Dictionary<Int32, List<WebResourceStatus>> _status;
        private readonly Object _lockObject;

        /// <summary>
        /// Create a WebServiceManager instance.
        /// </summary>
        public WebServiceManager()
        {
            _lockObject = new Object();
        }

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
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALOKOGPMELOBLBDKNIALBCFMNDGGINKJGAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAACNMIBEKHACLIKODIHANOMNBEAEPJMMDOIAAAAAAALANLECKEHADAOEEMPPCLPBJHDKFKLAAHJOMFCGOEAMPBHFGHBMHBGNDCFKEPHANAFPBNHIBDDPMCPIHADCOBLJOHPECCIDLLAOKFAJPJOKICCKKEAOLOBLGANFANODMAHBILAMBHLDGOABLFBJHCICNCFLCPLMLFPPNMHCHFKDLGHHPKOJIEJOPOHIBPLLNBMBKMCMNLOPGECPHDDNOJAPKHFDMAAELFIOBLEIHFDJOBHBHHMDANPPLJLCHNFAFMBEAAAAAAHAKGPPBKHHKIEBCKKMOIAHIPKALDMGBOCPLFKDME";
                            break;

                        case "ARTSERVICE2-2": // New production web service server.
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAGOKHGLPPLCDKEANKPMCBAGECILHFHICMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAMHDOCMAFPNFABIFPKMCFNAKFLJDPPOPPIAAAAAAANHNMPGJNPAGEAIEHBDELNIGLGLDCFNNLEKBFEGIDIODFMPCPAJILKELHJAJLJNCDMAFGDCGCAKOHBBEHAADPNFGFHIHBGGAPIACNFJJCEFGJCCFBJICPCAAGBLANPFOBLAFBAGDLEPENONEEFKFOBGNFGDFAFGGCNACCJLGPDOCOPOEENBFFLCLHMOKJNPDMPHFLPPHIBLJFJDOGJKJOPBFMDAJFCACMONBEFGHMIGGNMFBGGEOCKEAAFLCPICPPBEAAAAAADIMDMOIHFNBNEIEMBKGBDNAJIMNAAAINCOPNMEGH";
                            break;

                        case "SILURUS2-1": // Production Web Server
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAKCDIPIMNCHNKMLEDLNNLHANAPIJOPGEOAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFMJKJJKPGONAALLKKONOJOCEKNOILCMPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAAEPPECEJPDIKLPMHDEMICPINJKJKMEBEIAAAAAAADGNPJMBCFEGGCPGAMHCBDDABGHAGGLGBNINNHKIGFLICHDHNLCPKFOMELNDDJFMLEPOGANBCFENILPIKPDBDKEJNFBDFGKHHIMMHMNBEJOPIJFEEEEDPMEOINNHIJDPDLMKHCOPDDDELHMKGGBDPAEBMLOMOOKMLPKLMBNONJDNENCDPEHIMOJFBKOHCGDIOJJAALKLCDFACLBCOAAAMDPPLCGBPPODFJNCGCKLNCDEEBEDKPKHLEENKCBIPOHONBEAAAAAADOHNHAEEBBCAGKLJNMONLCLOGONPNHANPGIHEMOO";
                            break;

                        case "SILURUS2-2": // Production Web Server
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAADJCPFBPDFGGAJPECKJKGADMIIFAPGPOLAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHPEFPNAHMIIDLHCNLFOMJJKDFOJCCPHAAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKLIAJGOLIECGMKJOEEPKGDBKMGDIOHHOIAAAAAAAPFOBLHBAPAMOCPFHPDMLLBCFMIKJPABNLGGAHOIBILOJODOPOGAKHGPKMLLMMHGNLCDKDJBIHNJIEPFMGAENCGGLNNDLCGNMEIMCECENKDEPJEHGNDBNCMFCINMJEIMFOAFIMNFEILDMJBFFAFEOGFDKDFDICLNLCGAEHFOFILCACGFKOJJMPMIOPCIAEPDGFLILLAGJBIGNJMLBMMOMIIPDDEABKDJLECGKEFCEJHIFLCDPOAPAFEEIOMGPDFBKBEAAAAAAKHFBECMDIKDBDPGPKFJHKAOMFCFMGJGHHGIPKLAN";
                            break;

                        case "SLU011837": // 
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALCPBDBGCIAFPEPEILEKMLLIKEOOJMJMKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANNLKKHKHFAKHIKMIBIGKDABFBHFAJKKJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAGMPEPAKGBNAEIOOKAIGJBCOFNBJOPLKAIAAAAAAAPHIDEBBMHBIMOBJAJKFHCEFMHCEKNKGIBECILBCGAPMDHJKGKAFIBKALBNELCCJFDLLIHHIALCHBHOOIHCGCOGIILNCGNPLBFHKBPOHNIPMHKNEMOLIPPKKLHLDGNEGHDFFDKJCACIJPDGCHMOFIAIJHFNGLGKBPGEEDLGJCNJKHIMIOPCMLHHJBJDMHJPACJIAFGFLKKDANDGLFHKGONCEABHHLIHJFFEEFCBFFINEJOGBNNPDHEAMBFNLDJPNPBEAAAAAAFGINLCGIGDOGDINKJNINNLNNFJODAPOIGKLCLEHN";
                            break;

                        case "SEA0105WTRD2": // 
                            key = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABKIOIJPBLDPKPCEELADDGIHIPCLMBNMGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAKDNKGOCCKAOEEOOKHBNONMPFOOKEKKDKAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAOCHCLFKEDOMPHAOIONKOEMFFFKLIPKJAIAAAAAAADMDFOIDFJAJLCGJBAADMMJLNAOCIJBPLAPJBCCHADAGLLFDFBDLAIMGKOOGCAAIDDOHOMJHDIDMOEAEJIJKFFEEHLJHIPNIIACJKLCNAOOEDMJFKHACOKFILNIAFCCBMJIOMALGLLACGPNCDNLBKJOKPAGAENEHBJIPFDLBINPJGPCFLGMJCIHOAEGHNFELMKOIOHDALABICGICDHOJMOOBNJGFMLDFPBBNIAJBBALGEJKHBOKOALHCBNOPMKKGEBEAAAAAAINCHLLPKHKFHFPPMDCKJLANBPKJPKOMJDDKAGLKD";
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
                return ArtDatabanken.Data.ApplicationIdentifier.ArtDatabankenService.ToString();
            }
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
                    case "ARTFAKTA-DEV": // Team Species Fact test server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADMMAHNHHLGOFGLKKPPBCNILEDJFNBOKMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJAJMCEDICMEDMFLHCEGCOKFJKNMLGBCMCAAAAAAAIJEKKPPODJHILEMMCBGHBHGFKEFDCOHFKDHFNCAJOADOFDBKCGCGJGMNKOOKMEFABEAAAAAAGOADGAMCAAHDKLNMEPPGFLMNHBICCGEMDFFDNKNC";
                        break;

                    case "ARTSERVICE2-1": // New production web service server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADALDCLDKGOMIJPBNLJJAHBAEEMBJJJIEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAOIONHGDIMCBIGMKKEOGPAJGLKKLNCDKHCAAAAAAALFINKBABLGFIFPGMDMIGJNIJGCNIGGFMIIANLAPOANJENGEHANCLNDEBDLDNMEDCBEAAAAAAHHAGAHICCOANGFCLOAOKCFHECHLIHICDFHAAPALH";
                        break;

                    case "ARTSERVICE2-2": // New production web service server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAKCDNCNPBCDGNFEDFCOCOFLLMDPOLEKLJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALIKGODAEGDPIEHKHPHCNCDOLKGIJKCDACAAAAAAAFOODGNKDIMKFKHMCKIOANBEJCMCHMDEPJJMJEMGBMMEJDLHNOGLGIHEEDCOKPPLPBEAAAAAABJHDDBENNLBHOODAHMPAFDMJLCBNBMILGDJOPMCM";
                        break;

                    case "MONESES-DEV": // Test server
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEAFPECBBEFEHIKEGLFFDMHECEONJDGANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJDDEDNOPNMLEEDBAALMCAPFLAENOLEEHAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAOBLNMCEOPDHIHFAIFNMGMBPGKNNHJBNJCAAAAAAANHJPAOKECIEAHMFBBLPDINGGHFMGIKCOOKMPHBNMELAONGOMOHDAPOJHBFNGPEAPBEAAAAAAMLBICEHHDEJKKMDANCDLKCFKKILJPKIEKHIKDDLF";
                        break;

                    case "MONESES-ST": // System test server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALGGOMNKGFBBFFLGGAIEDMAPDLNMMGDOAAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAMCDKAOPEIHDHJHDICEMLINEICHBLKDEPCAAAAAAACLBJPMPPPDAPHLAHJBMKAPPPOMLCAKNPPIJBFOFNGHILKAHCJLHLLIGHJDOLEGFBBEAAAAAACEOIGBFLDKGNABDNIBHFAFKFIKJMNGCEFMCHEFOO";
                        break;

                    case "SILURUS2-1": // Production Web Server
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPABFADOCAMLLMCENIDDDEELNEHLKMMNJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADOLKHHMFJLDKEBGHJLJDGPODKBNPPOKHAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFOPLBDNFCMONBFCGEGGEENDOAKHPHIHMCAAAAAAAKLLINLJKGCDGCEBFJCELPHKPPCMGNLDFFEEHFLMHEILPJFDHHEKKIKKHIJNFEMFJBEAAAAAABDOAIGDPMAJAEAMKOHNINLMCEFFGOBCDIOJMNOHG";
                        break;

                    case "SILURUS2-2": // Production Web Server
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALOAGFPBCGMGPGKEPKLNIHBBLIBHCICBAAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFDOABCCPDCFHMLNAIGPJBFKFDPHKJJPHAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAMGLOPDBHEBPCGLNONKBIIJJHJMDKBFIDCAAAAAAACFOHNMLEPMEMAPFJOHCLFDLLJJHNKOFJNBBEMNLOADPNIEGDHPCGKPCBKMCJANPKBEAAAAAAGODLLBNDIFPKPNLNBKOHMGJDEPLDBAEGIHCBFHHL";
                        break;

                    case "SLU011837": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPFOBOEOIJOPHGCEFJGCCBMFMHLOBJJLJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAOONCPBHGJEOCAKGOGNGLDABPPPPBMGBAAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJGKGIEHBMOONIOMFBPBCOJEBHKADJJNFCAAAAAAALHNACDEKKLPFGIKJBEGMIOINDHCOLACPKIAJNFEGDOGKELOHJJGLGEBIDAKLJGKNBEAAAAAAIABFFGIMHHOHLHLFAHILKAGCLKMFIDAHJPJGGMCP";
                        break;

                    case "SLU002759": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAKOJGMLOJCEHLOLEOLHGMDGJMGHPELCGAAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAABFJDOLMGCHJJEPGKKIEMMGJOHKEDDGCMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABHANGLBGOMALNDPIPJKHICNBFBNOANOACAAAAAAAMNPOHLGDGFONJPNKGAMOJEICCOKKFKBJBFGPHANBNLDBPKFLHAAHCEACKAOFGLBLBEAAAAAAKLIOCJIHJCKCCDHPGLNEAOJGDPFIIGICEDJMOJFL";
                        break;

                    case "SLU002760": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABAKAMJGGBJHDJCEBIMHBCBNNBPAODGGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADOFNBLOFABFIKDFKBNIPIADHEMLOPEMDAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPIPGBGJJNCALBBLHMGAPMFMDKCJCOIJACAAAAAAAAIIFILJPHCPFJKKBHCOODNBECKLNCOEOECINOBJPAPINNHIBGJHLJPLABJLOLNGMBEAAAAAAJDOLGJCDJIOPCPCAOIDFHHIEKLPFAMNHKFPEKMND";
                        break;

                    case "SLU003354": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALCANIEEPNPMJJNEEJPDPFJDCBDBHMDOGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJPADPLNIOGFPFBMJLHHAPBJGDHFNPNPCAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKFBOLHLGPIHGJLMOJDLAINNILCDBBCHFCAAAAAAANLCNOCNDFEOOLFLPKBDEMFCBEFDHGNLJAHBFCDNOGFBBKCKDBBPLNGBDMOJFLJFPBEAAAAAADMIKOLLLBAMCBDJAGNJPBICNFNNEFCDHBMBCIOAC";
                        break;

                    case "SEA0105WTRD2": // 
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABKIOIJPBLDPKPCEELADDGIHIPCLMBNMGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHPPANHDIMHLIEKOEEDFMBOKIICKDCAFJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAIIDONBJHLPPAJGPHNCMHLAPIBNKOBAIJCAAAAAAACOCLLMMJJHGHLAGGJFBOEMBAELPGNIHDIJLNDHEKKMPAOKLPOGCHLDJKCFJGNPABBEAAAAAAEKIKGDJBLHPCCHEALHGNEKHFNOJPADPOMPOECFCL";
                        break;

                    case "SLW-DEV": // Team Two Blueberries test server.
                        password = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAKNLCDLEPBKGJAAPDEECOHODKOOOKMBGJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAILDPDNCBOCBJIMFHBPKENBGEPJEEGANOCAAAAAAACGEJLNHEHPLMCPFMBNAEMILNHDJBAADAIANGANCKDDNPPDMNIJNJNLJOJCOLADFPBEAAAAAAIMPHAMKKNDLPLCGFCFABFJKDPCJFCOKNCNKBCPJM";
                        break;

                    default:
                        throw new ApplicationException("Unknown web server " + Environment.MachineName);

                }
                return cipherString.DecryptText(password);
            }
        }

        /// <summary>
        /// Get specified resource type.
        /// </summary>
        /// <param name="resourceTypeIdentifier">Resource type identifier.</param>
        /// <param name="localeId">Locale id.</param>
        /// <returns>Resource type.</returns>       
        public static WebResourceType GetResourceType(ResourceTypeIdentifier resourceTypeIdentifier,
                                                      Int32 localeId)
        {
            WebResourceType resourceType;

            resourceType = new WebResourceType();
            resourceType.Id = (Int32)(resourceTypeIdentifier);
            resourceType.Identifier = resourceTypeIdentifier.ToString();
            switch (localeId)
            {
                case ((Int32)LocaleId.sv_SE):
                    switch (resourceTypeIdentifier)
                    {
                        case ResourceTypeIdentifier.Database:
                            resourceType.Name = Settings.Default.ResourceTypeDatabaseSwedishName;
                            break;
                        case ResourceTypeIdentifier.WebService:
                            resourceType.Name = Settings.Default.ResourceTypeWebServiceSwedishName;
                            break;
                        default:
                            throw new ArgumentException("Not supported resource type =" + resourceTypeIdentifier);
                    }
                    break;
                default:
                    // English is default and also returned if not
                    // supported language is requested.
                    switch (resourceTypeIdentifier)
                    {
                        case ResourceTypeIdentifier.Database:
                            resourceType.Name = Settings.Default.ResourceTypeDatabaseEnglishName;
                            break;
                        case ResourceTypeIdentifier.WebService:
                            resourceType.Name = Settings.Default.ResourceTypeWebServiceEnglishName;
                            break;
                        default:
                            throw new ArgumentException("Not supported resource type =" + resourceTypeIdentifier);
                    }
                    break;
            }

            return resourceType;
        }

        /// <summary>
        /// Get status for species fact database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="status">Add database status to this parameter.</param>
        private void GetSpeciesFactDatabaseStatus(WebServiceContext context,
                                                  Dictionary<Int32, List<WebResourceStatus>> status)
        {
            Boolean ping;
            String address, informationEnglish, informationSwedish;
            WebResourceStatus resourceStatus;

            address = null;

            try
            {
                address = context.GetDatabase(DataServer.DatabaseId.SpeciesFact).GetAddress();
                informationEnglish = null;
                informationSwedish = null;
                ping = DataServer.Ping(context, DataServer.DatabaseId.SpeciesFact);
                if (!ping)
                {
                    informationEnglish = Settings.Default.DatabaseStatusErrorEnglish;
                    informationSwedish = Settings.Default.DatabaseStatusErrorSwedish;
                }
            }
            catch (Exception exception)
            {
                ping = false;
                informationEnglish = Settings.Default.DatabaseCommunicationFailureEnglish + " " +
                                     Settings.Default.ErrorMessageEnglish + " = " + exception.Message;
                informationSwedish = Settings.Default.DatabaseCommunicationFailureSwedish + " " +
                                     Settings.Default.ErrorMessageSwedish + " = " + exception.Message;
            }
            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = Settings.Default.ResourceAccessTypeReadAndWriteSwedish;
            resourceStatus.Address = address;
            resourceStatus.Information = informationSwedish;
            resourceStatus.Name = DatabaseName.SpeciesFact.ToString();
            resourceStatus.ResourceType = GetResourceType(ResourceTypeIdentifier.Database,
                                                          (Int32)(LocaleId.sv_SE));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.sv_SE)].Add(resourceStatus);

            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = Settings.Default.ResourceAccessTypeReadAndWriteEnglish;
            resourceStatus.Address = address;
            resourceStatus.Information = informationEnglish;
            resourceStatus.Name = DatabaseName.SpeciesFact.ToString();
            resourceStatus.ResourceType = GetResourceType(ResourceTypeIdentifier.Database,
                                                          (Int32)(LocaleId.en_GB));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.en_GB)].Add(resourceStatus);
        }

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Status for this web service.</returns>       
        private Dictionary<Int32, List<WebResourceStatus>> GetStatuses(WebServiceContext context)
        {
            Dictionary<Int32, List<WebResourceStatus>> status;

            status = new Dictionary<Int32, List<WebResourceStatus>>();
            status[(Int32)(LocaleId.en_GB)] = new List<WebResourceStatus>();
            status[(Int32)(LocaleId.sv_SE)] = new List<WebResourceStatus>();
            GetSpeciesFactDatabaseStatus(context, status);
            GetUserDatabaseStatus(context, status);
            GetUserServiceStatus(status);
            if (IsUserServiceStatusOk(status))
            {
                GetTaxonServiceStatus(status);
            }

            return status;
        }

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Status for this web service.</returns>       
        public virtual List<WebResourceStatus> GetStatus(WebServiceContext context)
        {
            UpdateStatus(context);
            return _status[(Int32)(LocaleId.en_GB)];
        }

        /// <summary>
        /// Get status for TaxonService.
        /// </summary>
        /// <param name='status'>Status for TaxonService is saved in this object.</param>
        private void GetTaxonServiceStatus(Dictionary<Int32, List<WebResourceStatus>> status)
        {
            Boolean ping;
            WebResourceStatus resourceStatus;

            ping = WebServiceProxy.TaxonService.Ping();
            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = Settings.Default.ResourceAccessTypeReadSwedish;
            resourceStatus.Address = WebServiceProxy.TaxonService.GetWebAddress();
            resourceStatus.Name = ArtDatabanken.Data.ApplicationIdentifier.TaxonService.ToString();
            resourceStatus.ResourceType = GetResourceType(ResourceTypeIdentifier.WebService,
                                                                         (Int32)(LocaleId.sv_SE));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.sv_SE)].Add(resourceStatus);

            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = Settings.Default.ResourceAccessTypeReadEnglish;
            resourceStatus.Address = WebServiceProxy.TaxonService.GetWebAddress();
            resourceStatus.Name = ArtDatabanken.Data.ApplicationIdentifier.TaxonService.ToString();
            resourceStatus.ResourceType = GetResourceType(ResourceTypeIdentifier.WebService,
                                                                         (Int32)(LocaleId.en_GB));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.en_GB)].Add(resourceStatus);
        }

        /// <summary>
        /// Get status for user database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="status">Add database status to this parameter.</param>
        private void GetUserDatabaseStatus(WebServiceContext context,
                                           Dictionary<Int32, List<WebResourceStatus>> status)
        {
            Boolean ping;
            String address, informationEnglish, informationSwedish;
            WebResourceStatus resourceStatus;

            address = null;

            try
            {
                address = context.GetDatabase(DataServer.DatabaseId.User).GetAddress();
                informationEnglish = null;
                informationSwedish = null;
                ping = DataServer.Ping(context, DataServer.DatabaseId.User);
                if (!ping)
                {
                    informationEnglish = Settings.Default.DatabaseStatusErrorEnglish;
                    informationSwedish = Settings.Default.DatabaseStatusErrorSwedish;
                }
            }
            catch (Exception exception)
            {
                ping = false;
                informationEnglish = Settings.Default.DatabaseCommunicationFailureEnglish + " " +
                                     Settings.Default.ErrorMessageEnglish + " = " + exception.Message;
                informationSwedish = Settings.Default.DatabaseCommunicationFailureSwedish + " " +
                                     Settings.Default.ErrorMessageSwedish + " = " + exception.Message;
            }
            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = Settings.Default.ResourceAccessTypeReadSwedish;
            resourceStatus.Address = address;
            resourceStatus.Information = informationSwedish;
            resourceStatus.Name = DatabaseName.User.ToString();
            resourceStatus.ResourceType = GetResourceType(ResourceTypeIdentifier.Database,
                                                          (Int32)(LocaleId.sv_SE));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.sv_SE)].Add(resourceStatus);

            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = Settings.Default.ResourceAccessTypeReadEnglish;
            resourceStatus.Address = address;
            resourceStatus.Information = informationEnglish;
            resourceStatus.Name = DatabaseName.User.ToString();
            resourceStatus.ResourceType = GetResourceType(ResourceTypeIdentifier.Database,
                                                          (Int32)(LocaleId.en_GB));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.en_GB)].Add(resourceStatus);
        }

        /// <summary>
        /// Get status for UserService.
        /// </summary>
        /// <param name='status'>Status for user service is saved in this object.</param>
        private void GetUserServiceStatus(Dictionary<Int32, List<WebResourceStatus>> status)
        {
            Boolean ping;
            WebResourceStatus resourceStatus;

            ping = WebServiceProxy.UserService.Ping();
            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = Settings.Default.ResourceAccessTypeReadSwedish;
            resourceStatus.Address = WebServiceProxy.UserService.GetWebAddress();
            resourceStatus.Name = ArtDatabanken.Data.ApplicationIdentifier.UserService.ToString();
            resourceStatus.ResourceType = GetResourceType(ResourceTypeIdentifier.WebService,
                                                                         (Int32)(LocaleId.sv_SE));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.sv_SE)].Add(resourceStatus);

            resourceStatus = new WebResourceStatus();
            resourceStatus.AccessType = Settings.Default.ResourceAccessTypeReadEnglish;
            resourceStatus.Address = WebServiceProxy.UserService.GetWebAddress();
            resourceStatus.Name = ArtDatabanken.Data.ApplicationIdentifier.UserService.ToString();
            resourceStatus.ResourceType = GetResourceType(ResourceTypeIdentifier.WebService,
                                                                         (Int32)(LocaleId.en_GB));
            resourceStatus.Status = ping;
            resourceStatus.Time = DateTime.Now;
            status[(Int32)(LocaleId.en_GB)].Add(resourceStatus);
        }

        /// <summary>
        /// Check if it is time to update status.
        /// </summary>
        /// <returns>True, if it is time to update status.</returns>       
        private Boolean IsTimeToUpdateStatus()
        {
            DateTime compareDateTime;

            if (_ping)
            {
                compareDateTime = DateTime.Now - new TimeSpan(0,
                                                              Settings.Default.OkStatusUpdateIntervalMinutes,
                                                              Settings.Default.OkStatusUpdateIntervalSeconds);
            }
            else
            {
                compareDateTime = DateTime.Now - new TimeSpan(0,
                                                              Settings.Default.ErrorStatusUpdateIntervalMinutes,
                                                              Settings.Default.ErrorStatusUpdateIntervalSeconds);
            }

            return (_lastStatusUpdate < compareDateTime);
        }

        /// <summary>
        /// Test if status for UserService is ok.
        /// </summary>
        /// <param name='status'>Status for UserService is contained in this object.</param>
        /// <returns>True if status for UserService is ok.</returns>       
        private Boolean IsUserServiceStatusOk(Dictionary<Int32, List<WebResourceStatus>> status)
        {
            List<WebResourceStatus> resourceStatuses;

            resourceStatuses = status[(Int32)(LocaleId.en_GB)];
            if (resourceStatuses.IsNotEmpty())
            {
                foreach (WebResourceStatus resourceStatus in resourceStatuses)
                {
                    if (resourceStatus.Name == ArtDatabanken.Data.ApplicationIdentifier.UserService.ToString())
                    {
                        return resourceStatus.Status;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check if the web service is up and running.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// True = OK.
        /// False = Some kind of problem.
        /// </returns>       
        public virtual Boolean Ping(WebServiceContext context)
        {
            UpdateStatus(context);
            return _ping;
        }

        /// <summary>
        /// Update status for this web service.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        private void UpdateStatus(WebServiceContext context)
        {
            WebResourceStatus resourceStatus;

            if (IsTimeToUpdateStatus())
            {
                lock (_lockObject)
                {
                    // This dubbel check of last status update time
                    // is necessary since things may have changed
                    // during possible lock of this thread.
                    if (IsTimeToUpdateStatus())
                    {
                        try
                        {
                            _status = GetStatuses(context);
                        }
                        catch (Exception exception)
                        {
                            _status = new Dictionary<Int32, List<WebResourceStatus>>();
                            _status[(Int32)(LocaleId.en_GB)] = new List<WebResourceStatus>();
                            _status[(Int32)(LocaleId.sv_SE)] = new List<WebResourceStatus>();

                            resourceStatus = new WebResourceStatus();
                            resourceStatus.AccessType = "Läsa";
                            resourceStatus.Address = null;
                            resourceStatus.Information = "Misslyckades med att hämta status. Fel = " + exception.Message;
                            resourceStatus.Name = "Den här webbtjänsten";
                            resourceStatus.ResourceType = GetResourceType(ResourceTypeIdentifier.WebService,
                                                                          (Int32)(LocaleId.sv_SE));
                            resourceStatus.Status = false;
                            resourceStatus.Time = DateTime.Now;
                            _status[(Int32)(LocaleId.sv_SE)].Add(resourceStatus);

                            resourceStatus = new WebResourceStatus();
                            resourceStatus.AccessType = "Read";
                            resourceStatus.Address = null;
                            resourceStatus.Information = "Failed to retrieve status. Error = " + exception.Message;
                            resourceStatus.Name = "This web service";
                            resourceStatus.ResourceType = GetResourceType(ResourceTypeIdentifier.Database,
                                                                          (Int32)(LocaleId.en_GB));
                            resourceStatus.Status = false;
                            resourceStatus.Time = DateTime.Now;
                            _status[(Int32)(LocaleId.en_GB)].Add(resourceStatus);
                        }
                        _ping = true;
                        if (_status.IsNotEmpty())
                        {
                            foreach (WebResourceStatus tempResourceStatus in _status[(Int32)(LocaleId.en_GB)])
                            {
                                if (!tempResourceStatus.Status)
                                {
                                    _ping = false;
                                    break;
                                }
                            }
                        }
                        _lastStatusUpdate = DateTime.Now;
                    }
                }
            }
        }
    }
}

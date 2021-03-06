﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.Database;
using NotUsedCommandBuilder = System.Data.SqlClient.SqlCommandBuilder;
using SqlCommandBuilder = ArtDatabanken.Database.SqlCommandBuilder;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Artportalen
{
    /// <summary>
    /// Database interface for the swedish species observation database.
    /// </summary>
    public class ArtportalenServer : WebServiceDataServer
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

            // Opens the database connection.
            switch (Environment.MachineName)
            {
                case "ARTFAKTA-DEV": // Team Species Fact test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPHFAEMILECCAGFMGLPIFCMALLGNNNCEPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALADLPCICAEJKLGAGODGLHCPDENNNEDGLMAAAAAAACEEAMEPKJCHKOCHDLEMNDNAOFHJEKOJJAFGIPHNKNKPKPCJMCBOEOACJAOPKFIFHDAEECNEMKCOFNHLNIFBIIIEMIALHDDLNLIMIBEPGIJIIAMBCBPOECEPMGEMCHDPCEFJEKFEMOPBADOAJAHMBNMNBNLKNINDJJDGKJPPNLCGCPJFMBKBCMADJBGBKNBLIBPONFLKDHCGDMIJJIPOEEDECPLDHHOFNLFLPJDPHIPBGNFFKAPBENBCPAHJEEEFKNFKPLJOEDDIIKFMLKELHJJPHOMBAILKJDJJBGKKJCAJCBEDCCNLJCGEPNBKGAPDNKGFAIGPEOOMDCEGMDKCHGPAALLPOBHDBMEDONBDLIFNPAEOGAGMGBJJBKGCKKJNGBEAAAAAAMLBBEALDMDDIJHBLGKONFONLDLIAGJKOHAPIACLJ";
                    break;

                case "ARTSERVICE2-1": // New production web service server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAMHMNMJFGEOEDEMHFCCCFDHPAIKDJGKPJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPBMJJLJPGLPLLOGDBHCABMAOGGBEAOEDMAAAAAAAEDMIDFPMAFLIJEKIGGNKKFALLCLEFGEAGEIOMMGOBNFKMFLCGFOAINJEEMAGBAIELLOIBFECECGLJAEHNKGMCNIFHPGCDEELKKONEDEBIOKHBGFOMEOMBEJGAFMJAOBDKMEBHGJJIBPPMIJKCCPIIGFIFEJDIKOMFGDBJLCMMPCCHEEOCBKFMDGHFAKALLFEBLDHBMGMAFHAGOAINJKPKLPCCCOKAEIKKKEOFAKALJKOEJEEJFENPHODKFFNIOGNMHBPFDHECHEAHKFHKGKPFEFBLAHELAGJIOKJJOPOLBOAJHBICDPPLHJKOMAKDNFNKHELEIPHAFNFKAKAHODKNDJNHMNMCOPLFJCPJINLIKDKKBJGDKOEOHNKNIHNBIKKBEAAAAAAIIGMHPKMIDHJIEGOGMBPFNIEHKKKCLCHLLDMAFAK";
                    break;

                case "ARTSERVICE2-2": // New production web service server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPPFFNOADKLEBJNEIOAGLLJBPMPFDCMDBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADOACJKEGBFDIICGPLMJPDCEJNHIBEOIAMAAAAAAAJGKBJIINBJGNBOPAMJHLCICBAMHBHIOMOLGANLEOIAPBEGOOMHLELOLOIPMPBAFPOLAMKKOKKNOHFCHFFAEEIJHGLDEFFLCEEBPCINLNIHEINCGBJKPCFJPEELJLELCPLBIAIMDELPPDELDFDBAHGPAMGHBPMJMGOCNNOBFDMKGHIANCEPFEIJGPBIOCJKJDAGOEMNLFLEBFPPOPGJGONEMEMOJKCPHLBGAPACHIPJCLEKLLGCCMBEJFNBENMLLIHMNKDCNODMMHLMJFPILBACHCAJEOLPPIJNPHANELHNEENNKAONIEIHLLFCGJBCODIFBODCJNPKKBOEDMOPIKIGNBHAPKALCFMKINNBJKNHBAAEDONOBFEEOOHEOKCLNMBEAAAAAAOGJKFCNEOLFBHMHGOELIBCNBALJGFKHAFLCPDEOC";
                    break;

                case "MONESES-DEV": // Test Web Server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEAFPECBBEFEHIKEGLFFDMHECEONJDGANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPOPCPKKGCMKKNDDMEHACNBIDDHBKELCPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFGDJEDODMJDOIJCIIGOOKGLMJBKLIOCIMAAAAAAAKHLPCIKOEAPFJGIMHPKHGKEOEODIMFEJIPDKDIKGMIHOLCOKNNCBMNGHLDAMMEMGEJBFAPKMDKKGEPHAABJJBKNGAHMJHBGLJPEIMELOMGEBFKFELIMBKNBKALLFNMAOAJLOHOKLBKNFPAMNGAKHNKBCFBJBANPIDKHABEFBHAIDEJNNGPDAMEBNHANHJHFAMPGGOPFAKECPAJBJMJHBHPHOCDFNCGNIFPAJLEMOKJNACGPEAHKALDHCDJBHMKOHHNDMKGOIKDHHDBGCPDGDBGDAFOAFPABNOJDOJJOALCHLNGOCDPNPNJHAADEOAELENPJLDNJNBJAJNGDMHFCIKFDLKMHEBIOPLJENMCOKCHEEIHOGADAAFMJLPNMIMGMGBEAAAAAAOAAJBCMADBBFCNEKDIOKKHEMPKPHDJGNPDGIJNIL";
                    break;

                case "MONESES-ST": // System test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAEEKGHLNKPOHKDFIPBOEEEIJBKPHDKGMDAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFHCNIHIHGKIEKPKPMOMMFAECHELJJAIKMAAAAAAAJKBHLGJKMIOHBENAGIEPMJDEODHLBADHIIGEPKNFOCGLKJDHPKAGINMILIDJGNPHKICCDCDMBFLNNHDLBEPKIKFIEKDPMKILIHIBHEDMFOINFACIKCHIOPPPGMNMALOODIPPJNNOKCJKJDGGOJPGCCBLMCKDADLIPEDHPKCGKDLINHOFBENPAELLOBJGCIIJKLJHGEIGDNHNHEPOCADGJAMJCMPOHMHPFDMAMFGGHFGKMBAKDDEPFICDPPDKNOBFILPJIFPJJLJCFKLEAOEHFELLEBGDDAKBONNMKKEKLKIAGDMFPBGFLIMIMHCLICKBOMNJLLJJHNNGBONLJCBCADLFOJEEDMEGJNAGPODGHJBBPLOBGABLGBLFGCLFNJAJBEAAAAAAHONFIEANGOMDIHAOOLLFPLGMHCIHHLJHFAOMHGGG";
                    break;

                case "SILURUS2-1": // Production Web Server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIDGFKMFBDIMAPEELJPFDBGPLIAHNCJMKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAOHFGBBHIMCLNECGMIBNDHEOHEIMEAEMPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAEJEKGEEPCNKMAAJDEBHFGBAOJNCNMGKOMAAAAAAAPJOFALNMDBCEHACJDIEBMPKHBHNEKAALJJKLKEEOEBPAGDMDMAGPGOGMDNLPFKJIOOGNFPLGCJHLLEHDMFOIOAEHDKGCMDLNGAOLKIPJFGHEMAMIBFAIKGOEANDCLFGOMGFOPNECCDNIKHDMEFBIODKCMIHOPLGNELDCLCOCFBICAIMDNEFDLOOHIHNDHPJEDDFKHBPMMOGAPFMJLLDKLMPCLGJMDJIOIEHJCEJJJGMPOAAOEGCBGFCJBCNJMDLJHKMEJEMKABFCKOJLOKLHHEOIJLCCCKCJBCILGOKDHGFMJPHDIFFHLDKHFFHACFDOOGJEGKLOKPFBCNFIPGOHIPILNHCLKOLHACEIGHPAOLMCBCBMBMKMOCJGANHIJLNMBEAAAAAAAMMCJPJHCADIDLHHKGBLPHPOPIDONNEKCKEPJKAM";
                    break;

                case "SILURUS2-2": // Production Web Server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAODPAFKAIHGMAPDEOIPNDNGAIEJHDINBGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAOLLHCANAEHLNENCDIJIHFPOACAJACLGIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABGEADGJGCBPCPHONGOHEDKMJGPDHBLKBMAAAAAAACNMOHMDCKPFNLJLEKHBIJKIBOFILFHLNMBCOPLFJOILEOJEAIKLIPAECFLDFFEHEOJCEFAJMHMNLGHJBAJOIKCJPHHCCILNLICOKJKFILNPGEMDDJCCJIEGBECGKHCOPPAAPCDOIDCMINNLIKELONLGHHIMANOLAJDBPHEHGMMLGDIIDLNFEJPIPPFPMKNJEBPHMIEHCLPPFKLFMGCBBJNJPKBFAPEIJEPEDJNEDHOOGHFLEOGIAJGMKEDFKKNIDIKDBPDONHHBNFCJJFLHIEDHKKNMIMJGGMAKJAFHGGDAOFGBKJAMBMFADBCMDNJAFKAGEGNPKFIAPDEBEEGPHGJLLBNCOIPAOHONINMKLFEMJAOIJJHNPLICKADONKPIDBEAAAAAAMGLJBFILOANLGAJGKKLFOFNENAPEDAEBKLPNILOL";
                    break;

                case "SLU011837": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPFOBOEOIJOPHGCEFJGCCBMFMHLOBJJLJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFDOHGEAIBIIEABLFLNLLEOHCABPBLJGBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHLCJJBJNENLCBGMIMIIJHCGMMEBFEKDKMAAAAAAAODNBEAFCLLDDHKFAMMAOIABGCHNAFEBIPKDLHFEBFPBGGFBMODDMBBDKOKPHGMFKJOFEIHGGIBFKEELDIAOMAGHFCBOCFPHFBKPCMNFKOKHCEJGJICLOEMOKHEMNAJNKPIBGBJIFEPOMFPGEJLFEMFDFLODLBMAENOCNIAOMNGAPFEAAIFFMLNDMLCELHLILJOKJMJMJPOAAOCOAHJKCNDFCEACBCJPOIHPLKFNKNHBNEHFFDFMBDOLIBOKPLKCHLGJHKJDKIPNDEEGFKNDKCMPDJJIPGDGINDNKDFIGAKMCGAAKFKGIGEENAAGMIFKAKDEGCMCENOCABMHKKFGMKFFOLJJCHGGNFNODPCOKPBCKCIDOHKGHFOHPMKCKMDLMBEAAAAAAELLALDLCLIPIIELLABBLNNDIMBLJHANEEAGHKBKP";
                    break;

                case "SLU002760": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAOIKAMFPLIBLJKHEBKLMNEPHLFFHCEIBDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPLLPHLHHHBABLANPAOCAGJGFIDLEHHFAAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFOAIBKBBIKOLCGIONJCFPAAEKEIGIAANLIAAAAAAPFLBLJAFPDLPLFLMKKGJFAGOIJEIAGALLFKGEHAECAJADAOAMGNPDNECBCBJJKEDHOLHLHLGBMIDIDEBCFJPHGKMEBFEJMPOOALMJEONPJOBIMAOLHILBGAMIDCOCIGOLAPDCIMBBCOACIKDODHDELNEDGHCKBBDOFHPOOHOBFOKCKKMIFLHGCHKHBPLOJCIAACELAKPADANIKLKHLPJNDMDPJIOFFBKBJMHLLNKCFABJMJBOFEDOIOJIKLAGBBEOAMOKIDMAIEJFKANBGMLCIPKICDLDCGALKOKBHFFMIICJCMMGCHGCIJPNMMLJANBOFLICBPKKCAEGHNIPCEPEBNADHPPHEFJFPGJJCJDLFMAPEFHBEAAAAAAAKMLMKIFPOGLOFIGMDFBCIBPGKDOPGJKMLDDGJOK";
                    break;

                case "MATLOU8470WW7": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALHCOFDGPCNONAMEDJDCIAMKCOKLCFBPFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADBEHAKPBFIAMGOPBOKKFGFJKIJOELDDEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJILCCNAPHHFDLLJDEMNKACOPJOENJIECMAAAAAAAPOCHDFJMNEKODDKFHIHODBNCNIACNBOOJPDIFMFBAAALBDNLPJKLDBINGAKFKDPKMPNMIBKNCHPBEOIBIPMJEDJDPNODJGNLDMEIIMBNJGAOBOGAGKMPFABMKHEADNDGHBHFHOPPGIPOBJENDLMBFKFNDBKGDJMEGHDNMLHDHNHMKNCDOJMJMOEEDFNFHPKNJMDDHPDBHKANGMACNBCPEHJPOPBHFIFNBJCKNOOKDPFDOECMCAMEKIKIFMLOPLIKLGCDLDJKEPBIFPNFDIJOLEOBGNEFFJEGDCPHCJHAAOCEFCOAGDHKKGNACJCIJEICDHFCPHFKCPGCLIOMEDOCGLNMAFMMLCCGMMHFGJPHLBBHJGNKEFLKGENHAAOEBECLBEAAAAAAMPKDCDBEGMDPBCCBNIDNCNOAIDLEONPOFIINBPKK";
                    break;

                case "ARTDATA-TFS": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAENFHPOKFCMDLPPEIKKMGLNPOKIBMBJBEAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHFHHOAFCFLLDDHMCPGHDMIFFCHEDAGMGAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFLDFIPCDHNECOHEHOBPDOMLJLNOJANOBMAAAAAAACBPAOJHCOFKCGPLDLBPGPAAAPOPCAPOAEHLLLFMNJIJPMNNPFPINNAEJJNLGOOHHNGMMLNBMOHNODLIHBFPJANPOBPLMDOPIGPFBFMMEECKEEGGFDJOJFBGBBIOJDCEAFIOLGPBOLKCEILJFFJCHLAKOCBHHIMAMIONMICINAAIEJEKDCDFAHDKGLMOJOHDCAADNILFNDFNPFDDEMKGPCAIKLCCEALALLLHAKDCIBJBPLMOOJICFENBJBAPENFFLNKOPAJKLCAFFEBOEIABJGJAFBEPPLHPMCBDNJHJDMDONOFLFBDBMKCPEMDEPPEGDMHDANCHBOPCKLCMBILDKELNHCHMNGPFDCHBEMALMGAMACNBCMMENOFEIECJFEFNKBEAAAAAANAJHNOPLIPJCCPEDLKOOIAKABKPDNBCNKCIDNBBP";
                    break;

                case "SEA0105WTRD2": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABCDHLPKLHGBGOOEJKJIFHDOAFNEEFMNKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALLMKEBLBHCBHLOHGBLBBJAHDEMFICFJLAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAANIIAJIFALCEPMHPNFKAIBMHFHJBANAGIMAAAAAAAOCMEEIHONEMANOMADBFAEDKPDBCOKFLKOCLLCGDDFHJBKBBLOPGANGHGBJADCAEFKPKFLKKNPGEGOHMBACEBCKCCGLIHICLGDJOPDKFANNOEJPCJKCJBMLBMBEFAJAOCHMEHOKPCEAONCJHEGIEAJADPFAHLEPDEHACADKALALMOCOKJCKOFMBBKJCKOBCJHDFHLFCBPGJFAGMOHDONMAIBEHDJFMCEKIMOPCLLCCJJOPCIAOCNOALJFGPKPADLKCJDMIODNAPBHMJPPHNLKKJKCJAGNFGDMHFOGEKPFPNPJJOGKFNOLGKJANOKBBIMMMALJMLFLGLDDHHGFIKHCIBBFCDBKIHCLIIMJCNLIIIKFEINIFDPNPBKGEKPLHEPOBEAAAAAAFEHGFGAMMEJIAEDLEAAAOCKBKHBFGKPOCFCLNFHK";
                    break;

                case "SLU011896": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAFNEPGOOEOHKKGCECIICFNMJLKFBFPGCCAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAMNNBJCDEOCAJNJEAJKECJHLJLEEIHGDIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABJNLCKHAKPHKFENPIDDKPDOEEMIKCHHEMAAAAAAAHLMLGODHMAOOPJADOPMJAPHLJPFMDMDBENFGFDODHCMEAJLDCFALELKBMDBDIFIKIPLLHKDBAAGKOLPHCHNGICAPJCHALEFAGDBCFECPLKNBAFDDBHCHDAHCGNMJNMNKMNKNKDMJIGCLPJKKKAHBEGDDEHEEDHPLGKMEHJNLJJCLPMKPMHKAKKMODHBBKCHCDFALKGGJDIJJLGKKGKIFOLFBNDLPLAMMKGKDGDLGEDCMLGEGGIDOPFLLJGGEHNGGFPDLPBMMONIOPGEHBABBOHHBEPMNPFPNFIHFJGOKNOCLFFKHHDJCEKLKCFLCFICKNKDFGKEKHIAADCFCEEBINKLIIONHCPHDLCOFNAJBNMDPDOIPDFPBIIAHHPNOBMJCBEAAAAAADJCMOJJEDGBCBEJEAIFCAEAPLIJIILHJHOBEELCL";
                    break;

                case "SLW-DEV": // Team Two Blueberries test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAIEAPDLAOFCIHDKAPFJIBGLOADAIECPKEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAIDPPLABOIHAMFOBMDEJDFIOLDHIDBHKOMAAAAAAAONNDLFCMCGHFPILKOMJGENCNPIKGDOFGIBMMKBIOMNAHBNGDEEIICJLLJLECOLOOBNMLKKAHDKIEDJNDGENAFFGLCLKDHDBOBPMLHJBOBJAMKGFGHEABFDICMLLDDEIGFGKIEHBJHHBHJIENCINPDGOEPKNJGCIKNKGCNFBJKCIFJLBPKPDDFCGCNIJAIPNOCDDFMOBCPANGHOEBFHDMHPBBCLGHANMOBDCHDKPAMEGIGCNGLLCEMICNIEJANIHOHLHNKCGCMHMHOODFOAOIPMGMMCEAFOEJOOJOHAGDGMLHJHOODFGJODLEPHLAPIABHGLFBLEHDAHDENAPPKHPDDGPGLCLNFHHLOAEEJMIHDHHAHBAPACIIKCFAKNMLDNHBEAAAAAAHGAOPGKOJLFIIPNAEDDANIFIDGKFLIHFABDECNGD";
                    break;

                default:
                    throw new ApplicationException("Unknown web server " + Environment.MachineName);
            }

            return cipherString.DecryptText(connectionString);
        }

        /// <summary>
        /// Returns metadata information about all existing project parameters
        /// </summary>
        /// <returns>An open datareader for reading</returns>
        public DataReader GetProjectParametersInformation()
        {
            return this.GetReader(new SqlCommandBuilder("GetProjectParametersInformation"));
        }

        /// <summary>
        /// Get DataReader with all observations that matches the search criteria, including any project parameters as a second dataset.
        /// </summary>
        /// <param name="changedFrom">Get observations registered at or later than this date.</param>
        /// <param name="changedTo">Get observations registered before this date.</param>
        /// <returns>DataReader after reading has been finished.</returns>
        public DataReader GetSpeciesObservations(DateTime changedFrom, DateTime changedTo)
        {
            SqlCommandBuilder commandBuilder;

            CommandTimeout = 1200;
            commandBuilder = new SqlCommandBuilder("GetSpeciesObservations");
            commandBuilder.AddParameter(Artportalen.CHANGED_FROM, changedFrom);
            commandBuilder.AddParameter(Artportalen.CHANGED_TO, changedTo);
            commandBuilder.AddParameter(Artportalen.IS_PRODUCTION, (Configuration.InstallationType == InstallationType.Production));
            return GetReader(commandBuilder, CommandBehavior.Default);
        }

        /// <summary>
        /// Get DataReader with all observations that matches the list of id's (used for sync purposes and not normal harvest), including any project parameters as a second dataset
        /// </summary>
        /// <param name="sightingIds">Get observations that has it's id in this list</param>
        /// <returns>An open DataReader for reading.</returns>
        public DataReader GetSpeciesObservationsByIds(List<string> sightingIds)
        {
            CommandTimeout = 600;
            var commandBuilder = new SqlCommandBuilder("GetSpeciesObservationsByIds",true);
            commandBuilder.AddParameter(Artportalen.SIGHTING_IDS, sightingIds);
            commandBuilder.AddParameter(Artportalen.IS_PRODUCTION, (Configuration.InstallationType == InstallationType.Production));
            return GetReader(commandBuilder, CommandBehavior.Default);
        }

        /// <summary>
        /// Get DataReader with all observations that matches the list of id's (used for sync purposes and not normal harvest), including any project parameters as a second dataset
        /// </summary>
        /// <param name="sightingIds">Get observations that has it's id in this list</param>
        /// <returns>An open DataReader for reading.</returns>
        public DataReader GetSpeciesObservationsByIds(List<Int64> sightingIds)
        {
            CommandTimeout = 600;
            var commandBuilder = new SqlCommandBuilder("GetSpeciesObservationsByIds", true);
            commandBuilder.AddParameter(Artportalen.SIGHTING_IDS, sightingIds);
            commandBuilder.AddParameter(Artportalen.IS_PRODUCTION, (Configuration.InstallationType == InstallationType.Production));
            return GetReader(commandBuilder, CommandBehavior.Default);
        }

        /// <summary>
        /// Get DataReader with all observation ids that has been deleted during the period changedFrom - changedTo.
        /// </summary>
        /// <param name="changedFrom">Get observations ids that was deleted at or later than this date.</param>
        /// <param name="changedTo">Get observations ids that was deleted at or before date.</param>
        /// <returns>An open DataReader for reading.</returns>
        public DataReader GetDeletedObservations(DateTime changedFrom, DateTime changedTo)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetDeletedObservations");
            commandBuilder.AddParameter(Artportalen.CHANGED_FROM, changedFrom);
            commandBuilder.AddParameter(Artportalen.CHANGED_TO, changedTo);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with all observations that has been deleted and also matches the list of id's (used for sync purposes and not normal harvest)
        /// </summary>
        /// <param name="sightingIds">Get observations that has it's id in this list</param>
        /// <returns>An open DataReader for reading.</returns>
        public DataReader GetDeletedObservationsByIds(List<string> sightingIds)
        {
            CommandTimeout = 600;
            var commandBuilder = new SqlCommandBuilder("GetDeletedObservationsByIds", true);
            commandBuilder.AddParameter(Artportalen.SIGHTING_IDS, sightingIds);
            return GetReader(commandBuilder);
        }
    }
}

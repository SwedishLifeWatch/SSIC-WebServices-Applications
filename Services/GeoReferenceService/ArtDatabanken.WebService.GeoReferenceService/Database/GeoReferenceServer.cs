using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using ArtDatabanken.Database;
using NotUsedCommandBuilder = System.Data.SqlClient.SqlCommandBuilder;
using ArtDatabanken.Data;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.Database;
using SqlCommandBuilder = ArtDatabanken.Database.SqlCommandBuilder;

namespace ArtDatabanken.WebService.GeoReferenceService.Database
{
    /// <summary>
    /// Database interface for the geo reference database.
    /// </summary>
    public class GeoReferenceServer : WebServiceDataServer
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

            switch (Environment.MachineName)
            {
                case "ARTAP-ST": // Team Artportalen test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGOCKLHDJNMOKAHENLNFMCAIBJFNLKGFFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALAJPFLMPHLLKPLDPOGLBLOCOOAFKJJODAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABLFHFGLOJLPDAGJLOIPNPJOABCFDLLFFIIAAAAAAICNCOMAODOHGCBOBNMJHLDMOPAMPLICJDMAJHOLIKHJNGIIAKHEAHGODJFKFNKBLGAHHHDLOPFKDBLDDFJLNNAPFEKDGFINMKJKPJALCAOKCKIECCHFMBHKGAEAGGODGGIONKLNLHPHNKBOOHKDLHFJPLFKONAMECMLNFEMLJIMHPKEEKKIKONOAMLIFGOAOBLLIIEMDFMHAFGEKPOBFEDNMGNHJDLKNEOOLDJEMDGOINBKHBLNOOHHCBDLBECBNJEDDLFMKFFEHBBFHBEAAAAAAPKJDMBJMAABKHCBOIEJNANGPGGAKJFMOJPFJGMKO";
                    break;

                case "ARTFAKTA-DEV": // Team Species Fact test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANJHCJOAGEBEDNLDLJOIDIOEMMDLACJFFAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJCAIAGLJODEHLPLFOFPPMILMEJGDNNCBJIAAAAAAEIKPKNKPMICHEHCEPGIFFJMEAIFJJJCPPOOACCLMHKDGNGDJHLLAGOGJFPCLJPENBBPPOJBOINGPIPOEMIAFEOGCGKKKNFDFJEMOPLIFKOMDKEIOCIFBEGEBGKPCINPBJOKNFLMIAMBFDBADAKKHLJEHLFBOCLBALEHHNFJAGHGCCFMGHMDDEKKNCECJEGLGLJBJHNCMDLGDOIMCBNNPBKHIFOKGFODICCOMLLDALIMDBIONOKCAACOBIMIFNDGKHMBPDPEDJDCHGGCKCCFCJDAALIGKJLMGIJPEFDIMNHJDEHFFBEAAAAAAAJIIABDJILFENOMKFEKFMEBNABMMHKFPNIGHGAAN";
                    break;

                case "ARTSERVICE2-1": // New production web service server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACHHOGOAOMGGBJCABMFLDDFHKBDFOEIHAAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADMPLOCGJHBIDIOGHNMFMEDNDFDLBPENAKIAAAAAAKAJGBMGLFBGFDCOLGKHDCPPHJGAKKCJAJAIHEDHIBKKOANGHJMDAJHOLNEOMKIECMMPEILGIMIFPAKHKNKGCGIGGDFJNGEKDBHFIEFBFKIMIABPHOKOCGOEADDLIMFOBKOHHDMJHGLOOOOABPEPFHHCGICDJJCBFDICGEADOGAHAGMGHFJONFOKAGDOKGHDEODGOPBMABPDFNMNIGIOIMDKLJJBJNINBKDFLFGBIBGJGBJFOECDNNONBPFGBBCGJMEHHCMHFHIDPMDHKMDIOIFNJHKFCOEMCCMFKGIBFKABGEJKECOIHPMKODFHCLPIKFPJKNNFFJJDADNJOBEAAAAAAHFGOFINECALBIMIKAPOCPFODFMGAJABOHEKAJNGJ";
                    break;

                case "ARTSERVICE2-2": // New production web service server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAKJPGJFOHGGONOCAIKKIKJIKOAAEJLEKEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPAPBCGBEGGCBOKCJEBGOLEHIIEKIIIOMKIAAAAAAAGGDMOKOGGLHHPFBGPAKPLIILIPBAHFFFEGCLFDCHCCDGMPMLMDHCDEKMMGPDPHJABEKNFGGGCJDLCKPDJOHNOIOKMGBCDDEIBDBOOIEMDFLGMHAKOIGCFDPDHOBGMKFOJFPCAODLHGGOHOINCFLNDHACKFBOFHFBMCOBKMKIPBOJAAMDCHJAEGOABDJKIGLFMCKDKNDLAMEHMCIGFKEDLEEAEBCDHFGPEIHHAKCIFPEAMMBLBHCEEFBEBDPAMCEJFFKBKGFFPPDKFBGLJNNDFGBNCDDPKDDLNBGCMPEGNKFPAGIEAILNAIDGMLGFDHHJPPGBKPMFLNGIEEEBEAAAAAANFIMHEHIMGGDFEOFILOCLPICHIAGPLKIAJBNNGKK";
                    break;

                case "MONESES-DEV": // Test Web Server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEAFPECBBEFEHIKEGLFFDMHECEONJDGANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAKJLFDDPJJHDHDCPEDFIOGIIHMFNJNFLAAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHALJGGEGKHCPGGAMNFOHEOHNGNEKJGEGJIAAAAAABEJPDPDPCENICMDKCNHJDHLILHNHNGIKEPBDKPHFOAOOLEJJMBCECJEDJEKBMDENKBNNHLGJPNJHBCNLPHBBGMGPOFOMNGAELAGIAMEACAOODFAAMOPJJPIPEJCBFIJPACIHMJGLIJDEOAELFECDCIFKJFMDBDPPJGPODFIGAHIFAMMOLPFPCBCBKMPLOFNKEFOGOEIKNAGLAMBBOHNBCNHHBJMPFBKLKMKOCMICBOKLEFHAEAPCODEDDKHMPDLNCIJEMMKGDPJHANGFKEBLDFAOJBIKOEGHLALJJIDJPCKMCDECBEAAAAAAPFJMBCDDOJGJPFOAKHJMKGGFDNILDHJLDEPKODBK";
                    break;

                case "MONESES-ST": // System test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIKGEBDAPMKNOAFEKIHFGCOJNGHIAKFIAAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAOILIKCCACKCCKOILEAOCFKLEMCLELPHBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABMNDGFBIBKLADEIKMFCECIFABKGKKAGFJIAAAAAANJLMDHBKKLJLAFJGKHIOJOAJAEGHIMONLHHEOPELJKOCIPIJHGGDCAPJNCDHAHLJELNOGCIICNIIEGPJNOEHMBEBHBACIGKEAGCOPBPOMNJEPCBPFCNGMIINPMCGBMFMHHCBIPDLNCGDLIPDHEIFKANKEJHACDLHFJBKENAIEELAJHOAPJDGCLPMHBPENBMEGDLENJFLOFKDHIHPLGAKHFHEEACPPIAHEHIAHDHLMOFKBEGDBGAIOOOEGNCJHAILBLIGEPEIAMECFBJKOOFIANNIOGHKGLOIJBIGGKBCALOFCHLLBEAAAAAAIEDIHLJAKPEIFFHHDGKCAHDEFDIBIEDIDLMOCDOD";
                    break;

                case "SILURUS2-1": // Production Web Server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIDGFKMFBDIMAPEELJPFDBGPLIAHNCJMKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAEJEPEPNMGABPBGJPNHLNNBDAJFKEIPFEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAEEFDEGGOKHBAPLHNAHMCENNJMNIKAHBBKIAAAAAAKOJLAFKFGPFJIDEFPGPONIDAKDPEKCAJKPDOMADECEAOALMABJLPPDGNNICLCBEDHEOHPFHCEPCKKFMOBELPMPDAAHKHGHHLMBCFFDPLNJDPEMDMMFGLCMLLPMIAJOHOICBPAJAOHPLGLOMIGMGHCLLDDCLDNCNGGEJFGJPPJPFMLLLJINDJEODJNDEKGLKCFMBBAAMNIIKAHAPOCAGELBIJPFKIJIAGPJNCIFFFCCDLCBBEBDELFIDEBHLACNCLEGMHJDBDBDKGGKMLJBBJELHAGOGBIENDDIGABLPACHGHLJPMHJFGLGOGHGFEFEGFCLPOHJDGPBFNMKGBBEAAAAAAKLFJHFPBFOAGGCOPGOKONIAJKBMFPLEDJKCABFFD";
                    break;

                case "SILURUS2-2": // Production Web Server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAODPAFKAIHGMAPDEOIPNDNGAIEJHDINBGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAEOMPONFDCHIKPLHKAJHADKFOFKBBAKANAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKPNGIHOJFCIMOFPNEBANFOBMILACDCKMKIAAAAAAJLMNMIHNACFKEMNHDEKIKBIHGPDODDPAMDNIGMDICDGOFOCNAJEGKEKLMEGNHDKNGGOAKGOMNPFBGEIKINMEMPIOJBFIFFLEEOEMIFKCMMAJPLOJMIFNJOAPHLNOPKCDOLONCFBOBIEOPFIBFAHCCLCEIGMJKIMCOJFJKAMCPHAPKPCEAAOLJGHPOBFELCEGOCBODCCLMNDNGOLLLEGNNPAIKEEIPKIKEGEODGACAIKLLKLHOCHGGEMAICEKGDCCDCBPMKHAMJHJJKCDLLPOFKMICAEJNKMNDEIFCLKIAMJHCFIALMMDFPDGDELALLNCKFMKFNHCEJJHAKBNBEAAAAAAKDOHPGMIPKIKGIMIMELHLBCBPDECLNNIMAENMKHF";
                    break;

                case "SLU011837": // 
                    switch (Configuration.InstallationType)
                    {
                        case InstallationType.ServerTest:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPFOBOEOIJOPHGCEFJGCCBMFMHLOBJJLJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAABBDHBANJAEOLKCBJCNCFGPOBHDHJAJADAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADENNDIIAKDDCBNPCCNMFIHIJFCIKBECOJIAAAAAAFAEKDHDKIJDMOPADAIOLEACIEFDICFIOMFGOPDFEAFFGCANKJFDEMGIKEAAFDIICMNLFKENJGBOAMAOJEDMIPLALEOPOPCGFDJHECCNHPILOHFOHKIENJFJHFACHHFCCILEPBFMBFHHHOKGJPADGCIKJAAELMCKGOJPMFDCHIMFJACLLJECEADKJAGDIABCJPBGGIEKBHFHIHOMBOOIMHOLIMJMHJECNPEBEGNMCEFJEMGFMDIGMOAJJODGHBJDCHKICACHHAEBJKDFLNAIINFNHNLLBJHEPDNPBPMECHDFLIHODBEAAAAAAGPLADADGBLHDGLIBPDFIFLLHOFGBCKJNDMDBGBEB";
                            break;
                        case InstallationType.TwoBlueberriesTest:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALCPBDBGCIAFPEPEILEKMLLIKEOOJMJMKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACGICEPCLMLCDNKLHFGHGIJHJMONOIELAAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAMACBDKKJAKODGOGGLEIOOANCMEJOGHHPJIAAAAAAKJMFMFPMKLGLLIKAILODOFPIFEOGFOOEMKKCOJHDOKFPLMCKKHCLEHNDAPGOGEBPOPPIJKINDGIDDNHNNMCICLBJDOJHGNJNODCPFGJFKOJLEKIICBFACKNLKONHNPKMEBOFJEAMFCLBGDJIOALKOBAAPAHOKNIPMHOIJKPDFNODJLLAPHGOFBHBEJKKAKBANFFIHMMMPPGGINEAPACEOIENGCPNINPMGIFJOMBDGNAKAGDPMNKENNCIADIPLPAIMNKMFPOCMBBLAPGJPKKDCHDAHHCGLOKNBACBCDAFJFFFDJBFBEAAAAAAFJJGHCEJHALIBAJEPPDHLJFHFAPKHAMNDMJDOPID";
                            break;
                        default:
                            throw new ApplicationException("Not handled configuration = " + Configuration.InstallationType);
                    }

                    break;

                case "SLU002760": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEEOMOLNGPCJLAPEBLNNPEFHNPDCANGOLAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHFGLHOKJNKCDIHMACEDLKJEMCGAMFFCLAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAICDNOJINHGEPONIAGLHOILMICEPDJBDGIIAAAAAAJHLOOKHNFDONNNJFEAHNLKOBOCCDFGPIOPIDLFCJGOIANJCFEIADJELFKAECFPEGLAJDEIAICMLJADGOMCJAJCMBFICBFFJPOMCMOFJJBAMKDIFADHAJHJNAMKHIIOAANHCHECLJMIBOPAJPBANFJLLHBFLCKPOFHEFCIEACKCPGFHMKPKJOAOAKHCFEKILLEBPNIEADJILGCPAONCAADFILBMHOBMHGOELIKJIGIGNHBAKHENOKEPKLNLNELNALOKACLDANNFANHLFHBEAAAAAAMFMDOGNCBKLPNOBACNNAIPPNCNCIDLICBDLFCFKE";
                    break;

                case "MATLOU8470WW7": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALHCOFDGPCNONAMEDJDCIAMKCOKLCFBPFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADOPEHJEAFKBJCCHNGFOPIGJHELCAOONKAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAEGCGDLMDBDMOKAMHBGNHICJJFOGDIFHFJIAAAAAANMCPLOGNHFOOKGLNIENBJIBFOOGNAJKAFHNLNFPGKKHMKLIDFEPPLKKDBCJIPPHEIDBLIPIHJMIFHCABCDHLBBHNAFJMMOKCCBHJNOBIMDAEBPJIMBLKAEOAIJNCIGMFIFCKGMCMOEHOPAOOBCHEOMHCKCMNBADNCHBHJECGDFDAKMFFEBALHLHAEACNELJNINBCDBAOOMEFMHBHADAOLPPOGIKKIEKOANACNJNFAMPBDMLKAPEHJJFOHLAGNFOLJMOEINGOLLKJJNDEBPIBEGPMOBFPICAHFHGAEELBKGJLDIGEBEAAAAAAODOAMEBJALOPKFONDOLPMJIIBBKAFKAFPDFLBLJD";
                    break;

                case "SLW-DEV": // Team Two Blueberries test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAABIFNFPJBEJIBBDPFEHIPPKLLFDJDGBFOAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADNAFIEJIFIDCCGHPGCHIPOKOMHKFJOFNJAAAAAAAMMEDEOFFHHJPKHLDNPCELPIINKKLMGIMJKOGLGJIILJPPNNDOCEKAKLFDLOCCDFAPACMNOGEOGPGFKLAHHCIDDADBCOHNKPDAKOLCBPOLIENHKFEKAKGEEJGFMBLCEDMEPPDNNEMDLOBGJDFBNGAEHHDHDDGMCGAPBDCMNCOOLBEDNJOJPHCAKNCGGCKKFGCJGLIJKAIKEKCCLPCMIAKAOEPBIPCEIJJNAEOFKEMOGIKPIOAMLJFMPGMEOIKFMDDMHFAIEIPGIAHPLHJBCKHJFLFMODODJPEBEAAAAAAFEKJAHFOLCEJJHJLOGNFBJHLAKDKLLIGADANOMEF";
                    break;

                case "SLU010576": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIBCGMONNPLJJELEOJBIEMOPHMHBPHABPAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAABJHGBMBPIBFPLNICEDJKJBIKKMHNMGBNAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJBLEFEJJJMIOCCOGBNFHGIADDEMDGEBCJIAAAAAAJINPIPOOMFHLOOFLBOEPFCGCOLCHIHBFPCICPEPKNDLILAAEIFLCNLBOCNCIOLEDLHBCJNFMPEDKBKEJOJCNAIICJCNNKOKBNFFPBLACFIFOBEBDPCMJENINIDMIMNGAMIPAIEOPHKCGMLFPNNBNILPGAOKNEKIOENKELAAOOKBIIJLMEBDIOLIONBNJLCPHPIEMCEGNFKELFCGMADCLGLHFHDCDFIOJNHIALPHNJNBMOPNKLOBHNFNGGCHJIDHFDAAHPNODGCAGDLKKOIANENHEFAFDKCCCHLHDMIPMEFIIGLLIBEAAAAAACPKAABEIPGGPDHAHKCIHDBDIFDFIOIKJPCEFLLOK";
                    break;
                case "SLU011161": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALKIHLMKNLJKFGMEGLCLEKJGFGIDEMDMNAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANCJHIJJLNOEJAHFFPIKLLJIHMIGCNNAHAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABHFPIPJFADBLEJMHPPJHICDCHMDIECMNJIAAAAAABKNLHOBJIPGNGHOCMFGFODKBALJHPIPLIEEODJPEAABOIEDCJPAIGBEKEDLBDICAJMOBHEJMFIKBPBCLAPCIKJBILJGPAGPMLBECALLEBMDNEDPIBIMHMAMKEAAEGPJGHCOOMBHAGJHDCMGKNPPOKCMCCKJGDAONCGMCDCBPIBPOAMEAIDDACKKJCBGOFPKODFJHMEJKHGNILFLICBEKHIHOCHBBPLPJLGHDPCNGCLOBBKIKJBEDGMPONGHFJLENAJKGOJMDPMECPMLJBEJLMCCJKHJOINLJNGBJALJFDBJNGHGKBEAAAAAALBBLAAFHJAHEJAEABHOGDLOHNKINKMHCNBGNGADM";
                    break;

                default:
                    throw new ApplicationException("Unknown web server " + Environment.MachineName);

            }
            return cipherString.DecryptText(connectionString);
        }

        /// <summary>
        /// Get all region categories.
        /// </summary>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetRegionCategories()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetRegionCategories");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get data table with region GUID information.
        /// </summary>
        /// <param name="GUIDs">Region GUIDs.</param>
        /// <returns>Data table with region GUID information.</returns>
        private DataTable GetRegionGUIDTable(List<String> GUIDs)
        {
            DataColumn column;
            DataRow row;
            DataTable table;
            RegionGUID regionGUID;

            table = new DataTable();
            column = new DataColumn(RegionData.CATEGORY_ID, typeof(Int32));
            table.Columns.Add(column);
            column = new DataColumn(RegionData.NATIVE_ID, typeof(String));
            table.Columns.Add(column);
            foreach (String GUID in GUIDs)
            {
                regionGUID = new RegionGUID(GUID);
                row = table.NewRow();
                row[0] = regionGUID.CategoryId;
                row[1] = regionGUID.NativeId;
                table.Rows.Add(row);
            }
            return table;
        }

        /// <summary>
        /// Get DataReader with information about regions
        /// that belong to specified region categories.
        /// </summary>
        /// <param name="regionCategoryIds">Region category ids.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetRegionsByCategories(List<Int32> regionCategoryIds)
        {
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter
            commandBuilder = new SqlCommandBuilder("GetRegionsByCategories", true);
            commandBuilder.AddParameter(RegionData.CATEGORY_IDS, regionCategoryIds);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about requested regions.
        /// </summary>
        /// <param name="GUIDs">Region GUIDs.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetRegionsByGUIDs(List<String> GUIDs)
        {
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter.
            commandBuilder = new SqlCommandBuilder("GetRegionsByGUIDs", true);
            commandBuilder.AddParameter(RegionData.GUIDS, GetRegionGUIDTable(GUIDs));
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about requested regions.
        /// </summary>
        /// <param name="regionIds">Region ids.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetRegionsByIds(List<Int32> regionIds)
        {
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter.
            commandBuilder = new SqlCommandBuilder("GetRegionsByIds", true);
            commandBuilder.AddParameter(RegionData.IDS, regionIds);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get regions that matches the search criteria.
        /// </summary>
        /// <param name="nameSearchString">Name search string.</param>
        /// <param name="typeId">Id of type to search for.</param>
        /// <param name="categoryIds">Category ids.</param>
        /// <param name="countryIsoCodes">The country iso codes.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetRegionsBySearchCriteria(String nameSearchString,
                                                     Int32? typeId,
                                                     List<Int32> categoryIds,
                                                     List<Int32> countryIsoCodes)
        {
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter
            commandBuilder = new SqlCommandBuilder("GetRegionsBySearchCriteria", true);
            if (nameSearchString.IsNotEmpty())
            {
                commandBuilder.AddParameter(RegionData.NAME, nameSearchString, 1);
            }
            if (typeId.HasValue)
            {
                commandBuilder.AddParameter(RegionData.TYPE_ID, typeId.Value);
            }
            if (categoryIds.IsNotEmpty())
            {
                commandBuilder.AddParameter(RegionData.CATEGORY_IDS, categoryIds);
            }
            if (countryIsoCodes.IsNotEmpty())
            {
                commandBuilder.AddParameter(RegionData.COUNTRY_ISO_CODES, countryIsoCodes);
            }
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with geography
        /// information about requested regions.
        /// </summary>
        /// <param name="GUIDs">Region GUIDs.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetRegionsGeographyByGUIDs(List<String> GUIDs)
        {
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter.
            commandBuilder = new SqlCommandBuilder("GetRegionsGeographyByGUIDs", true);
            commandBuilder.AddParameter(RegionData.GUIDS, GetRegionGUIDTable(GUIDs));
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with geography
        /// information about requested regions.
        /// </summary>
        /// <param name="regionIds">Region ids.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetRegionsGeographyByIds(List<Int32> regionIds)
        {
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter.
            commandBuilder = new SqlCommandBuilder("GetRegionsGeographyByIds", true);
            commandBuilder.AddParameter(RegionData.IDS, regionIds);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get all region types.
        /// </summary>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetRegionTypes()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetRegionTypes");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get cities with names matching the search string
        /// </summary>
        /// <param name="searchString">The search string where % is wildcard</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetCitiesByNameSearchString(string searchString)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetCitiesByNameSearchString");
            if (searchString.IsNotEmpty())
            {
                commandBuilder.AddParameter(CityData.SEARCH_STRING, searchString);
            }

            return GetReader(commandBuilder);
        }
    }
}

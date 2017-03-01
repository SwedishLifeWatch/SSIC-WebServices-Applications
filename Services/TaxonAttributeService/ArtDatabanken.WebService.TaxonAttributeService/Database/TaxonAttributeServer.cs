using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ArtDatabanken.Database;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.Database;
using SqlCommandBuilder = ArtDatabanken.Database.SqlCommandBuilder;

namespace ArtDatabanken.WebService.TaxonAttributeService.Database
{
    /// <summary>
    /// Database interface for the taxon attribute database.
    /// </summary>
    public class TaxonAttributeServer : WebServiceDataServer
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
        /// Create species facts in the database.
        /// </summary>
        /// <param name="speciesFactTable">Table with species facts.</param>
        public void CreateSpeciesFacts(DataTable speciesFactTable)
        {
            if (speciesFactTable.Rows.Count > 0)
            {
                lock (this)
                {
                    CheckTransaction();
                    AddTableData(speciesFactTable);
                }
            }
        }

        /// <summary>
        /// Delete species facts.
        /// </summary>
        /// <param name="speciesFactIds">Ids of the species facts to delete.</param>
        public void DeleteSpeciesFactsByIds(List<Int32> speciesFactIds)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("DeleteSpeciesFactsByIds", true);
            if (speciesFactIds.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesFactData.SPECIES_FACT_IDS, speciesFactIds);
            }

            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
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
            CipherString cipherString;
            String connectionString;

            // Opens the database connection.
            switch (Environment.MachineName)
            {
                case "ARTFAKTA-DEV": // Team Species Fact test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAGFNPHKDNBMDBJAAPBEJJABBDJLIJIEIIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAGIJDAHKCIFMLGAOJNKEJPIBHBAFFFBFBKAAAAAAAENHCOJPGIGHNBCALPKIACHANOMJFMEPNDHPBFFCLJFKEJPAHPHHHMPICGPDALEKEIPNFGHGLPPFBFMANJJIDKIKNAGFIIAOCGOHPHGJPFBMJOMBECLEOMNLGLGIMLDPPPJOECFNLCHDDONNBEJMADOCOEDICFKBNHBHBKCMIOJECIDDNPEHIACOJMGCOAGJENLHPJGLBCIBFMAAIJDPEDKHMIPAEINLEEOFMALJNAOANODHENGMIIMIKPNBAJAAKKJALMFHBLHAMDFIFPKMHNPGCNJFNDECHEECLGMIBFECINFHPDFPKFKCDDDNIGBMGBEAAAAAAPGIPEEDMBFBNIHMHOFAODKNFOAKKHPPGDHIGIFCL";
                    break;

                case "ARTSERVICE2-1": // New production web service server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAKADKPNMPPEEIGFGKMDJFOJOLDGOJKCCIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAANCBHKFHCJOHKJJFJBCLICCANJOICDHEHKIAAAAAALGAHHIDNGEPAPGKPILNLGBDGKPCDJJFHKLMECMPAKHPKOCONPENBNBDLMHALPMGMBKGHMMBNCDJBFFCCHCMLAAJIHOFHNOCJBLFNLPFHKHHDLLKIFIBDEBMBNMJAMOMOGLBGMMDCHIACLMGNIMNNAHOCFOFMEGDMAFEBBDOELJKDFBPKJIBOGNJAHGIIDIKPDLDNBHHEFPPIDMDPKEBFKEJLEHPLANDGODPNBKPOOAFJCCDNNAKEMKOLBDJCLDANJAFCGKCFPFMFAFIKCECEIGFPCAAFCKJNNFCOEGHOIGAIACBPBMGKFFKNAFONGFFAOLDINAJDDPDMAGFIBEAAAAAABIELCHOICBDKHBLOGCDLGNAKDIGAFNJCJCBDLHEE";
                    break;

                case "ARTSERVICE2-2": // New production web service server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAGKKHKLOBMBIKFILDFEIBHNNLCOMMAIKFAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABMFICBBBEBCHFELAJODFGOIIBLMDLIPCKIAAAAAAKLIJIBPBLGLOHKGBPGBGGKPMMPHMPOLAIBFBFOALAMDDFLOCBAHMDBKFJMCNKLAGAGFENFLANAJBCGGKDCPCKHKCAIEKJKOJPKMJNOFLNPMEGGAEFBAEOAFIHPMNNODFJLPACBOIFOJEJJDMFADKMJFJJCGHKCKJJJIDPGKCNGMKLJPOJKAGJJEDANJJIGILENEAAFCLLABCMJAOMFMHFFCBHKHFGHPBJNPAMNGNIIPFKKCIAOHCGDLNAJGABPLIIKNKGLNEFOCNDNICFDLKGBGGFDMMFOKIGKICFFKJCODLBJBFKAOAIIAGLCCINAIHLJHDPHLKBJLFDEEHBEAAAAAAKGJLPKAFPNGEEKHCELBGMMPFAIOKDDNLMKEKFDCN";
                    break;

                case "LAMPETRA2-1": // Production web server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPODPODKBMCFCJFEAINJBICOOKAPLAGJPAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHFBMCIAAFNIKFGDGMIAPPOHJCKMCKMACAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAMJPOMPKNNAEMNMPJHPLBIKEIFJKHPJDJKIAAAAAAJIKCMODLHONKFGGCMAGKLHNKFJLHOGLPFPPDCIMBBBLHLMPAAINIIAJMLPFDKEJFOGMNFJAGJMOPEMHHKLHBHINBFKFNMJJLDADLBOBGOOPLLDBAGGELFFMJCBPOJOEHKAALNAPMDHLIBDPGMODLLIFCCOEBGNOKJBIPJIHLNDOBJKDELIGGINNIOFOHDEPBCKDPOPIEEMGADNBBEEAJACEOMFGKLHOLFGFCHGBKFOMJOBIHCIPAMDPEPBAILJKNPKCPNEDEDKCMPDEBMNDILPLPPDHDJOPCCNAAGKNHKFEODDBMIBJKOHPHFPNLKCFBOHGKKFHBDNBCKKHEBEAAAAAADEOHFCKNKINDHDBHEHHKCGGHHCDLGIMJOMHHEGNL";
                    break;

                case "LAMPETRA2-2": // Production web server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEDMBBJLFMMHMIBEJLJCCNBFHKPCHNNNAAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAABDKCDEFMJHLCCKDIMIPFMLNOLOCKILMFAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAANMDDBMJOHPFEGKBGLAKNPJHPBNGGDDOCKIAAAAAAHDJPFFGFKNEECIGCGDGLFAJBHJAALIHMIDJJLIHIPMLMCOPOMCPNEGJGLLMOCNFDMEBDPOOMAMHPKJOPLOFEHDDIPGEJJCNCHNGPNNEHNGKOOHHIJJDNNIMHDOADOOLIBCNKIACOFCIMILIGOGBDDNJDGCLJBDLGMOKJPLCLHMNELPBKAPHMPAPJCLONOOPFFKOMMHFBJDCNGAIBPBCOGOCKOEMBDJAMMNHCAEJBKEGPGLBDMNPHALJGEDKJGHCGAGOENALPNIPILGMPANGONGKBDFBPMBLNPOIIBNKPFOIOJNBNFEMIBHNPKLBPOIAKEIFHCJHGKCHGNOOOBEAAAAAALOEGAHPHLDCDKLILDHCIBAKCFHGNNDEIBPLBPFAI";
                    break;

                case "MONESES-DEV":
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEAFPECBBEFEHIKEGLFFDMHECEONJDGANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHFJEKOLIKMIGJFEPHOIONFBIKIJNMNAHAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALLFIDMJHOBNIKAOPMFAIELKGKCILCNKLKAAAAAAAFBKBLNHBCDBCDILHLNEBAPBHOFPIADNLPKIDDNFPMDGLNDAHCNGBKLMGFLEHLFKOHCBLCJDLDEKBGGIPPBDJOPPGPMEGMKGLNABNCIGIALNEBCCPADOGHIMLPHEAENJOHCGFJELEFGNBJMMNIIMHJFMHDILGNCKAHHDDHMJAIAFPIPLAKCHKHCKGAAEDDJIPLHMGDNIDAEFGBNMDAPPKNMDHOPBLFMJGBJINKGDGMHBJMCJBHIBLKPAFLAIBGBBFOGOIIHDDFDPHLIIDOEAMFAFOJAAJBPHLBHGMMNCGLOCONCCLJMBEBBJICCDCEHHNBEAAAAAAHJILJKDDJGAGNNDHMLFMENIIPFGNDODCEKKEOHBG";
                    break;

                case "MONESES-ST": // System test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIKGEBDAPMKNOAFEKIHFGCOJNGHIAKFIAAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADNGIKPONGEOJMJMLEGMOFPEIEIODFJKLAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFEIAHKLLEHDNJAHGPEIAOIHDOPCGGDBCKAAAAAAAFEHEJBABIHPFONEMPCKPBNFNNGOBEOAGCHAMGIAAHEPMOOMFCAOODOIOPKEGIEHPCLLKNPDCIJGLOENPDMAGOHJBLHKFJLNBCPHLEOMDPOKDDEMJPLGBNDGDGNKICDIGCDOEIEIPHEBIHFMMOMNLKFDLCKDLBJEMJEPOCJCKAPCNALGOBDKHAKBNIJNAMOIFCFLIHBANANOINJOBCJIHHDJJHPFCKBLACMHHFAIBIGDIIIAJKGJOCHDCGKCNAPJMECGAOJPAMIMBFIPDJNGFBLBGGLDGKMFIILBECHABACFBINMMCDNPLICHFCHPNFIDBEAAAAAACKLPICEKEHMMEKENAOFMDPIJKBPNDGAGNBNOLOFI";
                    break;

                case "SLU003354": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALCANIEEPNPMJJNEEJPDPFJDCBDBHMDOGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAGBBGNHGODKPACOBNMPMAEEHONINONCICAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAACMLPOBFELLCNALNIIEBBLOHNCPAIPLCKAAAAAAAMJGNAOOFGLJOEKEKGHDLKOLCODNBONJOLGBMJGMMDBHHNADHHNIKHFFDKPDIMNGPKAOCLFPMCLNMCFNBKJCOJEJAHDFPGEGKIBACDDHMGIIAENFAKDIFFHCECGNHMMLDEJKLBBPNHABDNEHLJKEFJMGIHOHDMJMHCKMIAOLDDNNAACHFFFGGBIOBCLKOPGGBCIKEGACOJNDBGFEOLHBPMCMGAJLHMHKDCHEMDBCNOJCJNGPLKAPJHHGLAEHMNCACHGJNHJPPBJAPFDPDCEDBHOPJEAADGECDHMJAHHFHFICIIODBHFKGGIMBACOAFKMFBEAAAAAACAEINNJCKAIFPMGBBPACAMOAGJJKAIEOIOMFEDBK";
                    break;

                case "SLU011837": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALCPBDBGCIAFPEPEILEKMLLIKEOOJMJMKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALJIBFDJOFALIDMMIJNAKDNJOALMLLDCFAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJHPNGPFEDJJDOCBLEHNKPMJDEEJDDDBGJIAAAAAAMOIFECKNMGDLNPAAFBIDKHFEFMAIHJNHJNKDLJCBJNFNIAENPPFNHKGNEBFHHBLOGBMCNJAFMEPJLLKBMJBEBFEHJFGDALGFAGBDDDDHNOPKAFKAFBPNOKMGBFPJCCMKLEFJGEKICLJOFMGJGODOGCJMCBNBJKBDGJIBEBDANNMFGAEHGJDPHNGCONLFOICKNNAAAHKNONBCFDBPLFJDPFCLMLFCOBMLOCKPCCJELGPPGHFGLHIPKMPDPMBAEBJMEDHMHMAKCPNDANGPLIALBELBEMGIEJIHOKHGBKMANEJHPJIMBEAAAAAAFIDOGDCNONBMDDMCCFFBDDAOGCOOIMNCOIOINKHO";
                    break;

                case "SLU002759": // 
                    connectionString = null;
                    break;

                case "SLU002760": // 
                    connectionString = null;
                    break;

                case "SLU005126": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAABDFEALCFAMIAHEKIDGCJCCEIFKGBPKPAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPDPELLHEAHHCKDMFGNMADOOJKMBLKMJJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAAHDMMCAFPPEANOPFNHABDJCNIHDPKNNLKAAAAAAAEAIGAACOBJENPKFKCCBAJCALJOMIEMMICLKKJGMKJGAPFLEEHEOMCBNBALJDJFCJEJJCAJHODGDPHFAGAHPKAPCLECMMGJGGLLDLAJJLDPCBHJNHIFCFCBIMNJKECADGLJOLJFIDDBILMJNAAIGBPLPOALEPNEGKNENMFCHGNMJKGDEFHGFLLNOFIIFLLDOIIHGIJIHMCEGNDIMFAMBHGALFHCHMFBMMNAEKCBCAOBIOIMCGHHJEGICCNEKGBBDAGEHILNFFBCPPNJMHAGCKPAMIDIECBOLBOOGECCPHCELOMNMFGCEOHAIBLFDEBJPBBEAAAAAAMEKIGAHIHMHEGANOCINAEDIBDKOEDBBCALFLBIJD";
                    break;

                case "SEA0105WTRD2": //  @ MONESES-DEV
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAKAOAAJBONCIHKHEBLAJCAHNKCFCEDBEAAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAOHNNFEHJFCDOIOCEBIIENGEPDGDFPDIBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAGKABDLCHFPPOPOKINFJNOGHNBMEDEBJCKAAAAAAAMJAFLLIOKJGDNJMMEMANMMFLPDNBHLMLMBGKGLGGMAAHJMIFMFMDDECHEBOIHDANMGIOLLOPKKIAEFIFHDBICOCGJGPGAFEDJIKDHBFBKKKOCAOAIFKFBKNFOPAIDMKGCBMIBDHNHPOBFMAMFLDMLJEIJEENCMIGJMHLLJMGCBGPJCCCNMFFNHAPLAHGILCPFNNOEKEKNICIMPOHIDDENNDAJDOBDMNKNLMLOLHLDFKMCDEOCCBFLCMDLKCHMFPDIBJKCFIAEIONCOOJDKOFAFCJNHPBPPECOHOLMOPHHHHILAIMJKIPPCKJDJNOHABFBEAAAAAAPLPJAPJEPNIGBPBHIEBIDDLMELELHDIAMPCCMEEF";
                    break;

                case "SLW-DEV": // Team Two Blueberries test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFPFGBOIKFMNDLKGAMKBJAMBHAGKIEENGAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJAJAICJMHDBCDGKFMCFKHGBDMGMLNMGKJIAAAAAAEFDFABIHIOBKNCFNDGFNLHMJKMPDHBPBADEICDLMNAOADFCLGMDNCAMNHECDPGIMOJAGPFLMCNDALBPOLOLGCIHAGCFBPELNJNCILMACPMAIMMJBHGKDGPFIADBNAONGPCIBAMEJOJHKKOPDONDMBJHALNEMCDIGOKFAOPMMHHJOEIJMJDPPKIOAPCEONAIDCNCEBJODBKBIFLIKBOBIDKHPMEJNOJHDPHCOJFNHMMFDPAJONPDJBGCMEBDAPMJBEDAIEIKKPHNBHEOOMGNJKCKJDDIMAPFGBGEDGAJMPNJNLGJABEAAAAAAJIAIEOENEACCOAFBCLLCLNNOENEFHKFNNCEBHJGB";
                    break;

                case "SLU011161":
                    connectionString =
                        "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALEGNKAPEPFIKOLECLGFNLNJMBMJICEFLAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFPCEJNHOCENADDNIHDOHGKKAHPAHGNHFAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAAAHEAAGALELAHFMKAPMCIIOBPNPLFNDHJIAAAAAALDEAJJIAMHBIOGHGECHLIJGCODMNBPJCDNOHBJGAJELECOFNEPIGDHPGICJNBAINPLBDDHBKKLLNOAGGCEDNGJBGHONLBGHMJKLGFLFGIKLELOEHNAFNAHHJMFIKBNGBNHHHHFBKIBFINLMGHEEOOPLKAKABANHNJAOCDOFCFFEMOMJAFPEKMAKKFPKPIAKOLMEKHDJLGAOHGOOMBBMNPBJHFLACDFLOLMBGNAAAADKADHJPBGOLDHMGCAHCJOFCNJDIJFPCLNJINJAGLBPFFBPNLMLJMBELGEDDKBNDDCLNJKAPBEAAAAAAHOIEJCCKHGKHODOPEMNNPDGOBGMEOPJFPCLMIJAD";
                    break;

                default:
                    throw new ApplicationException("Unknown web server " + Environment.MachineName);
            }

            cipherString = new CipherString();
            return cipherString.DecryptText(connectionString);
        }

        /// <summary>
        /// Get information about all factor field enumeration values.
        /// </summary>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetFactorFieldEnums()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetFactorFieldEnums");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get information about all factor fields.
        /// </summary>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetFactorFields()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetFactorFields");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get information about all factor field types.
        /// </summary>
        /// <returns>Factor field types.</returns>
        public DataReader GetFactorFieldTypes()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetFactorFieldTypes");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about factor origins.
        /// </summary>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetFactorOrigins()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetFactorOrigins");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about factors.
        /// </summary>
        /// <param name="getOnlyPublicFactors">True returns public factors, false returns all factors. True is default.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetFactors(Boolean getOnlyPublicFactors = true)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetFactors");

            commandBuilder.AddParameter(FactorData.GET_ONLY_PUBLIC_FACTORS, getOnlyPublicFactors);

            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get all factors that match search criteria.
        /// </summary>
        /// <param name="factorIds">Factors to filter on.</param>
        /// <param name="factorNameSearchString">String to match factor names with.</param>
        /// <param name="nameSearchMethod">How search should be performed.</param>
        /// <param name="restrictSearchToScope">Indicates scope of factor search.</param>
        /// <param name="restrictReturnToScope">Indicates scope for returned factors.</param>
        /// <returns>Filtered factors.</returns>
        public DataReader GetFactorsBySearchCriteria(List<Int32> factorIds,
                                                     String factorNameSearchString,
                                                     String nameSearchMethod,
                                                     String restrictSearchToScope,
                                                     String restrictReturnToScope)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetFactorsByIdsAndSearchCriteria", true);
            if (factorIds.IsNotEmpty())
            {
                commandBuilder.AddParameter(FactorData.FACTOR_IDS, factorIds);
            }

            if (factorNameSearchString.IsNotEmpty())
            {
                commandBuilder.AddParameter(FactorData.FACTOR_NAME_SEARCH_STRING, factorNameSearchString);
            }

            commandBuilder.AddParameter(FactorData.FACTOR_NAME_SEARCH_METHOD, nameSearchMethod);
            commandBuilder.AddParameter(FactorData.IS_FACTOR_IDS_SPECIFIED, factorIds.IsNotEmpty());
            commandBuilder.AddParameter(FactorData.RESTRICT_SEARCH_TO_SCOPE, restrictSearchToScope);
            commandBuilder.AddParameter(FactorData.RESTRICT_RETURN_TO_SCOPE, restrictReturnToScope);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get all factor tree nodes.
        /// </summary>
        /// <param name="getOnlyPublicFactors">True returns public factors, false returns all factors. True is default.</param>
        /// <returns>Factor tree nodes.</returns>
        public DataReader GetFactorTrees(Boolean getOnlyPublicFactors = true)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetFactorTrees");

            commandBuilder.AddParameter(FactorData.GET_ONLY_PUBLIC_FACTORS, getOnlyPublicFactors);

            return GetReader(commandBuilder, CommandBehavior.Default);
        }

        /// <summary>
        /// Get all factor tree nodes that match search criteria.
        /// </summary>
        /// <param name="factorIds">Factors to filter on.</param>
        /// <returns>Factor tree nodes.</returns>
        public DataReader GetFactorTreesBySearchCriteria(List<Int32> factorIds)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetFactorTreesByIdsAndSearchCriteria", true);
            if (factorIds.IsNotEmpty())
            {
                commandBuilder.AddParameter(FactorTreeData.FACTOR_IDS, factorIds);
            }

            commandBuilder.AddParameter(FactorTreeData.IS_FACTOR_IDS_SPECIFIED, factorIds.IsNotEmpty());
            return GetReader(commandBuilder, CommandBehavior.Default);
        }

        /// <summary>
        /// Get DataReader with information about all factor update modes.
        /// </summary>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetFactorUpdateModes()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetFactorUpdateModes");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get information about all individual categories.
        /// </summary>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetIndividualCategories()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetIndividualCategories");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get max species fact id.
        /// </summary>
        /// <returns>Max species fact id.</returns>
        public Int32 GetMaxSpeciesFactId()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetMaxSpeciesFactId");
            return ExecuteScalar(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about all periods.
        /// </summary>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetPeriods()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetPeriods");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about all period types.
        /// </summary>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetPeriodTypes()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetPeriodTypes");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get information about all species fact qualities.
        /// </summary>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetSpeciesFactQualities()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetSpeciesFactQualities");
            return GetReader(commandBuilder);
        }

        /// <summary>
        ///  Get information about species facts.
        /// </summary>
        /// <param name="speciesFactIdentifiers">Identifier information for requested species facts.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetSpeciesFactsByIdentifiers(DataTable speciesFactIdentifiers)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetSpeciesFactsBySpeciesFactIdentifiers", true);
            if (speciesFactIdentifiers.Rows.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesFactData.SPECIES_FACT_IDENTIFIERS, speciesFactIdentifiers);
            }

            return GetReader(commandBuilder);
        }

        /// <summary>
        ///  Get information about species facts.
        /// </summary>
        /// <param name="speciesFactIds">Ids for requested species facts.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetSpeciesFactsByIds(List<Int32> speciesFactIds)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetSpeciesFactsByIds", true);
            if (speciesFactIds.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesFactData.SPECIES_FACT_IDS, speciesFactIds);
            }

            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get species facts that match search criteria.
        /// </summary>
        /// <param name="query">SQL query where species facts are selected.</param>
        /// <param name="factorDataTypeIds">
        /// Filter species facts on factors that has one
        /// of these data types.
        /// </param>
        /// <param name="factorIds">Filter species facts on these factors.</param>
        /// <param name="hostIds">Filter species facts on these hosts.</param>
        /// <param name="taxonIds">Filter species facts on these taxa.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetSpeciesFactsBySearchCriteria(String query,
                                                          List<Int32> factorDataTypeIds,
                                                          List<Int32> factorIds,
                                                          List<Int32> hostIds,
                                                          List<Int32> taxonIds)
        {
            SqlCommandBuilder commandBuilder;

            CommandTimeout = 300;
            commandBuilder = new SqlCommandBuilder("GetSpeciesFactsBySearchCriteria", true);
            commandBuilder.AddParameter(SpeciesFactData.QUERY, query, 1);
            commandBuilder.AddParameter(SpeciesFactData.IS_FACTOR_DATA_TYPE_IDS_SPECIFIED, factorDataTypeIds.IsNotEmpty());
            if (factorDataTypeIds.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesFactData.FACTOR_DATA_TYPE_IDS, factorDataTypeIds);
            }

            commandBuilder.AddParameter(SpeciesFactData.IS_FACTOR_IDS_SPECIFIED, factorIds.IsNotEmpty());
            if (factorIds.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesFactData.FACTOR_IDS, factorIds);
            }

            commandBuilder.AddParameter(SpeciesFactData.IS_HOST_IDS_SPECIFIED, hostIds.IsNotEmpty());
            if (hostIds.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesFactData.HOST_IDS, hostIds);
            }

            commandBuilder.AddParameter(SpeciesFactData.IS_TAXON_IDS_SPECIFIED, taxonIds.IsNotEmpty());
            if (taxonIds.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesFactData.TAXON_IDS, taxonIds);
            }

            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Update existing species facts.
        /// </summary>
        /// <param name="values">Values to be updated.</param>
        public void UpdateSpeciesFacts(DataTable values)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("UpdateSpeciesFacts", true);
            if (values.Rows.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesFactData.VALUES, values);
            }

            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }
    }
}
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.Caching;
using ArtDatabanken.Data;
using ArtDatabanken.Security;

namespace ArtDatabanken.WebService.ArtDatabankenService.Database
{
    /// <summary>
    /// Class used as database interface.
    /// The class has a <code>static</code> part that
    /// is used as a generic interface without knowledge
    /// about in which database specific data is stored.
    /// The class also has an instance part that is used
    /// handle the specific databases that are used.
    /// </summary>
    public class DataServer : IDisposable
    {
        /// <summary>
        /// Definition of id for the different databases
        /// that this web service uses.
        /// </summary>
        public enum DatabaseId
        {
            /// <summary>
            /// Id for the species fact database.
            /// </summary>
            SpeciesFact = 0,
            /// <summary>
            /// Id for the user database.
            /// </summary>
            User = 1
        }

        private SqlConnection _connection;
        private readonly DatabaseId _databaseId;
        private SqlTransaction _transaction;

        /// <summary>
        /// Create an instance of the specified database.
        /// </summary>
        /// <param name="databaseId">Id for database to create instance of.</param>
        public DataServer(DatabaseId databaseId)
        {
            _databaseId = databaseId;
            switch (databaseId)
            {
                case DatabaseId.SpeciesFact:
                    CommandTimeout = 600; // 10 minutes.
                    break;
                case DatabaseId.User:
                    CommandTimeout = 120; // 2 minutes.
                    break;
            }
            Connect();
        }

        /// <summary>
        /// Time out to use. Unit is seconds.
        /// </summary>
        public Int32 CommandTimeout
        { get; set; }

        /// <summary>
        /// Add information to a table in the database.
        /// </summary>
        /// <param name="table">Holds data and name of table.</param>
        private void AddTableData(DataTable table)
        {
            SqlBulkCopy bulkCopy;

            bulkCopy = new SqlBulkCopy(_connection,
                                       SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.CheckConstraints,
                                       _transaction);
            bulkCopy.DestinationTableName = "dbo." + table.TableName;
            bulkCopy.WriteToServer(table);
        }

        /// <summary>
        /// Add user selected factors to the database.
        /// These factors will be used for some purpose
        /// later in the web service request.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='factorTable'>Table with factors.</param>
        public static void AddUserSelectedFactors(WebServiceContext context,
                                                  DataTable factorTable)
        {
            context.GetDatabase(DatabaseId.SpeciesFact).AddTableData(factorTable);
        }

        /// <summary>
        /// Add user selected hosts to the database.
        /// These hosts will be used for some purpose
        /// later in the web service request.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='hostTable'>Table with hosts.</param>
        public static void AddUserSelectedHosts(WebServiceContext context,
                                                DataTable hostTable)
        {
            context.GetDatabase(DatabaseId.SpeciesFact).AddTableData(hostTable);
        }

        /// <summary>
        /// Add user selected individual categories to the database.
        /// These individual categories will be used for some purpose
        /// later in the web service request.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='individualCategoryTable'>Table with individual categories.</param>
        public static void AddUserSelectedIndividualCategories(WebServiceContext context,
                                                               DataTable individualCategoryTable)
        {
            context.GetDatabase(DatabaseId.SpeciesFact).AddTableData(individualCategoryTable);
        }

        /// <summary>
        /// Add user selected periods to the database.
        /// These periods will be used for some purpose
        /// later in the web service request.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='periodTable'>Table with periods.</param>
        public static void AddUserSelectedPeriods(WebServiceContext context,
                                                  DataTable periodTable)
        {
            context.GetDatabase(DatabaseId.SpeciesFact).AddTableData(periodTable);
        }

        /// <summary>
        /// Add user selected parameters to the database.
        /// These parameters will be used for some purpose
        /// later in the web service request.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='userSelectedParameterTable'>Table with user selected parameters.</param>
        public static void AddUserSelectedParameters(WebServiceContext context,
                                                     DataTable userSelectedParameterTable)
        {
            context.GetDatabase(DatabaseId.SpeciesFact).AddTableData(userSelectedParameterTable);
        }

        /// <summary>
        /// Add user selected references to the database.
        /// These references will be used for some purpose
        /// later in the web service request.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='referenceTable'>Table with references.</param>
        public static void AddUserSelectedReferences(WebServiceContext context,
                                                     DataTable referenceTable)
        {
            context.GetDatabase(DatabaseId.SpeciesFact).AddTableData(referenceTable);
        }

        /// <summary>
        /// Add user selected species facts to the database.
        /// These species facts will be used for some purpose
        /// later in the web service request.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='speciesFactTable'>Table with species facts.</param>
        public static void AddUserSelectedSpeciesFacts(WebServiceContext context,
                                                       DataTable speciesFactTable)
        {
            context.GetDatabase(DatabaseId.SpeciesFact).AddTableData(speciesFactTable);
        }

        /// <summary>
        /// Add user selected species observations to the database.
        /// These species observations will be used for some purpose
        /// later in the web service request.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='speciesObservationsTable'>Table with species observations.</param>
        public static void AddUserSelectedSpeciesObservations(WebServiceContext context,
                                                              DataTable speciesObservationsTable)
        {
            context.GetDatabase(DatabaseId.SpeciesFact).AddTableData(speciesObservationsTable);
        }

        /// <summary>
        /// Add user selected taxa to the database.
        /// These taxa will be used for some purpose
        /// later in the web service request.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='taxaTable'>Table with taxa.</param>
        public static void AddUserSelectedTaxa(WebServiceContext context,
                                               DataTable taxaTable)
        {
            context.GetDatabase(DatabaseId.SpeciesFact).AddTableData(taxaTable);
        }

        /// <summary>
        /// Add user selected taxon types to the database.
        /// These taxon types will be used for some purpose
        /// later in the web service request.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='taxonTypesTable'>Table with taxon types.</param>
        public static void AddUserSelectedTaxonTypes(WebServiceContext context,
                                                     DataTable taxonTypesTable)
        {
            context.GetDatabase(DatabaseId.SpeciesFact).AddTableData(taxonTypesTable);
        }

        /// <summary>
        /// Add updated taxon information.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='taxaTable'>Table with taxa.</param>
        public static void AddTaxa(WebServiceContext context,
                                   DataTable taxaTable)
        {
            context.GetDatabase(DatabaseId.SpeciesFact).AddTableData(taxaTable);
        }

        /// <summary>
        /// Add updated taxon name information.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='taxonNameTable'>Table with taxa.</param>
        public static void AddTaxonNames(WebServiceContext context,
                                         DataTable taxonNameTable)
        {
            context.GetDatabase(DatabaseId.SpeciesFact).AddTableData(taxonNameTable);
        }

        /// <summary>
        /// Add updated taxon tree information.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='taxonTreeTable'>Table with taxon tree information.</param>
        public static void AddTaxonTrees(WebServiceContext context,
                                         DataTable taxonTreeTable)
        {
            context.GetDatabase(DatabaseId.SpeciesFact).AddTableData(taxonTreeTable);
        }

        /// <summary>
        /// Start a transaction.
        /// </summary>
        /// <exception cref="Exception">Thrown if a transaction is already active.</exception>
        public void BeginTransaction()
        {
            if (_transaction.IsNull())
            {
                _transaction = _connection.BeginTransaction();
            }
            else
            {
                throw new ApplicationException("Transaction already active.");
            }
        }

        /// <summary>
        /// Commit a transaction.
        /// </summary>
        /// <exception cref="Exception">Thrown if no transaction is active.</exception>
        public void CommitTransaction()
        {
            if (_transaction.IsNull())
            {
                throw new ApplicationException("Unable to commit inactive transaction.");
            }
            else
            {
                _transaction.Commit();
                _transaction = null;
            }
        }

        /// <summary>
        /// Connect to the database.
        /// </summary>
        protected void Connect()
        {
            _connection = new SqlConnection(GetConnectionString());
            _connection.Open();

            if (_connection.State != ConnectionState.Open)
            {
                throw new ApplicationException("Could not connect to database.");
            }
        }

        /// <summary>
        /// Get address to database.
        /// That is, address to data base server and namn of database.
        /// </summary>
        /// <returns>Address to database.</returns>
        public String GetAddress()
        {
            return GetAddress(GetConnectionString());
        }

        /// <summary>
        /// Get address to database.
        /// That is, address to data base server and namn of database.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <returns>Address to database.</returns>
        public String GetAddress(String connectionString)
        {
            String database, dataServer;

            dataServer = connectionString.Substring(connectionString.IndexOf("data source=") + 12);
            dataServer = dataServer.Substring(0, dataServer.IndexOf(";"));
            database = connectionString.Substring(connectionString.IndexOf("initial catalog=") + 16);
            database = database.Substring(0, database.IndexOf(";"));
            return dataServer + "#" + database;
        }

        /// <summary>
        /// Get connection string.
        /// </summary>
        /// <returns>Connection string.</returns>
        private String GetConnectionString()
        {
            CipherString cipherString = new CipherString();
            String connectionString;

            // Opens the database connection.
            switch (Environment.MachineName)
            {
                case "ARTFAKTA-DEV": // Team Species Fact test server.
                    switch (_databaseId)
                    {
                        case DatabaseId.SpeciesFact:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJDGICBLDODLNDNJIIBIEANNBJLDGLDADAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJLEKAGDIHFMKKJEAJCHOEPIBEMOBGDCJIIAAAAAAAKABIDDOCLNBOCJFDDFPBEDHIEPNMHHNOPICMIBICIFOGEIFPOMOPHDKECEHAFHNPDFLLNBMMBFBOKPJBONLCCNHHFOIFCHKHBHBGOMNBOABFAMMKDLBJLHNBMKDGLEFMKJKOPHDNGIAHAHJILMAIBBENPCLDLMCAADKOIGJAAFPGPEHFBNCOEOKCFPDKCILIJKBFDEHHHBAILJHMEIPINKLMDBGGIIBCPEFHMEOKMHFOFOLDOIJBEDMOONAHIDGGPOPIJFBOGFMJMGJBEAAAAAABFHNIOPLKHAEOBAKJNFHAPPMNNLNFJLOIBPEAKDF";
                            break;
                        case DatabaseId.User:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAIPFGJFIJKNILIJADICDFLJLDLOEHDNLPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALMIBIMENIFELIMHJLEOBDKNLCBLKEPDJIIAAAAAAKEDEEJIJFEKMAFCFAJPEIDBIFPNGHIAIDNHFJMLBLLNIGLECPMGJHOEOBMEGBNGNJOFELCDIPBBGCMPHOLPEGPDBDOIJICHIFFHGACKCDMLKNFNONDFOMIOKJMBMNIILABHAPIINGJNPLLNJAAEDJJAMPFHJNAFMMKJJOMGJNFHKBNLJLFIGIACPHJHAKJGMALEBLGBLMCIFHOGAEHMDOBCCBFMMDPIEAOIOIIEOAMKNKAFCHCOGHKJNEPEDLENCGIJMBECDGONFJFONBEAAAAAAGKEMCKKJKHEONKFGLAJFGDOEDFAPIGOBEJNGHNIB";
                            break;
                        default:
                            throw new ApplicationException("Unknown database " + _databaseId);
                    }

                    break;

                case "ARTSERVICE2-1": // New production web service server.
                    switch (_databaseId)
                    {
                        case DatabaseId.SpeciesFact:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANIMAJCKGGEBIBJDLLKGNANFCBECLDFLGAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAMIPBAKNLKAHNKMDDIJJFPGKAFBEKCOIHJIAAAAAALBKJPIAGMBDMJKOOPGJEMAIDHOFAJPFFKCGMFFOAIGAHHINBMDCALKHKNKJJDILAHICMBLMOIMFDFDFKNGIGIGDBEAENJMNDIJCPIADFCCKMMDJDFFHILPJPBMBPBONOEEGFFHPNKKEHDCBPJJDHADNMKFCDEENBDOHEICPIGJGLBCLLGIEGMOOKLCKGMKKCFKPBGOPIININDAKHDOBHNKOLBHHGLCHJNFDEPLHPPJDMINHGOBOCHOMPCIFGMPCBONBOBAHDBFMIOMIPBINBALKNBKJGFEEIJAJCLLIKDNJBFGEHBEAAAAAAGEHAMHBADFELEBADINAEDIJLKPFKJJGOMDOOOIPC";
                            break;
                        case DatabaseId.User:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJDCDNBKNNCJCEBIKNOAACDKGGLJNJPPAAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAGCHLHEOGOEAACIHFJGAODBGEINIIILAMJIAAAAAAKJKAFHGDPCMPFBNNCIOGKKNAGAAIFACFBGKHLFLCPHMNONOFCKCDNGALEFGBEBGDDMMEAJALMALDNKBDONEGJBBOFBALJCKELPCNPIEGDOLIBMEOIMAHKIPBOIOBEIEDIIEBJLILEPIJLCNLBCFNMIEFAKAPLDIEHNPKBHOBFKPJBMKOHNCFAFNFODEJAAOAIPCCLCMEGOLAKOOPPIEHNNBEEEJIMBMLIAOAHNEGOKIINFGCHKPEKPMCJEFJMFPJHHMEAMAOJKPBCCKLAAGLPKGNICLKKODOPKOELDDKCACIPONPBEAAAAAAGJKOAKLEECABMGOIIKFLJNIAIKHJABOCBKICHKAB";
                            break;
                        default:
                            throw new ApplicationException("Unknown database " + _databaseId);
                    }

                    break;

                case "ARTSERVICE2-2": // New production web service server.
                    switch (_databaseId)
                    {
                        case DatabaseId.SpeciesFact:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAGFLFBECNLPDKACHNMGFKFLPBINGAMLGGAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKHKMIHMNEKBCHCBDNMIBBHCMLFNBNMJAJIAAAAAANCLBMBGFEDKIFHMEEAAFBBDJAIPHGMCGAABPNODEGGIMIAKJDJFDAOOJFMNCONICEACOJCJJKKKJINAFIICBBNIHIPBNCEKPPDPNLBCPAHBPPGJNILGIAHFPICAKNPLMIFBGDCMLCDDGGCMANGJHIJGJPFHJOPJEOAFBHPEAFGGOGHOAKOKHDGGINHOMOKGJGGKOIGMJBJHEILHOHOLJLICNHCHAHFHHILOADKEFIFLNOMMDEBILEIBMPMFCOLJKAPCKBMLGIEAGHPKFCPMGKILEJCKDOCAMNJGECBPCCOKAPAOEBEAAAAAAFCKLABGNHJMBGKAMKKNCJIBJDKPOFPADJPABJOHL";
                            break;
                        case DatabaseId.User:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFJKPECBKBEFLKMJPPCHGBEHEGLBBCDJAAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABPDMLMHHNMPIKKGHIKEGGOBGCANICABFJIAAAAAAEFIJDGJNFMEINJCHLMKLGNLIFBALGNOABJICGCLEBLGIMLFHAFMOEEKAAPONBAOKJNHJBOALBIMOBHKIHPAJJMEOKANPFBFBIFDPKHBPBBJPIALJGBLOKOJIHBMBLDOPIKNNFJNDGFEFBACCIBPMLBCFGGEBGAKNHDOHIIBPHLLFENHMFILEGIIGGLIGHCOKFBNLIPNKIECJJOJOCGBDPDJPOFLNECAKMAFMCHDDAAOPMOBNGDHKIMGJCJNKBDIMEOAEBGDFFGJLEDCLOANPIHGOIAFEMOAKLOCACBJMFDACBFGMBEAAAAAAPIKGCKHOPAHCJBBKIGPOFKEBGLJEMOGHKLNBAKHJ";
                            break;
                        default:
                            throw new ApplicationException("Unknown database " + _databaseId);
                    }

                    break;

                case "MONESES-DEV":
                    switch (_databaseId)
                    {
                        case DatabaseId.SpeciesFact:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEAFPECBBEFEHIKEGLFFDMHECEONJDGANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAOKIANIOOPJFBCIHNDHCHAIPDFACJHMDIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABCAJHBJILKEBIPNEJMFNMAMJCEDENHNKIIAAAAAADHLBCHNODJNJLANFCFAJKPOCPJCHFKFNEIJKCIJKKFBDODLAPCDJPGFKAOLIOCAKJHICEJMGCJANLDGPIELEIPBFHLFKKHFEFBLCEGDHDGHNHPNELBKOGAEOCIPHBELMJPKCPGFAPDFNMIEJOLEFBCILHKBCNGNJMGFALLLGAEBINMEKKDEGEFLPKOCMELMHPMGGNDFMNIOKIIMOJIMGEEFDBLPLNFGLNPJDJABPNPODLHNFGPDFGDNIPGEJILGBJGFMIANFBGCOEABOBEAAAAAAKFHKPMPEHGEEKNOMAAKCIHIHICDIBPHCHICDJICO";
                            break;
                        case DatabaseId.User:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEAFPECBBEFEHIKEGLFFDMHECEONJDGANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJDOBAJKKEJPAAFDOGJPGLABMEMCOJPJCAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFFLFGCFPMELKKALAJOMKCPOBHBHEMNMJIIAAAAAACNNIGDDFANJBOGFGNKCKLGOPJIHNLDAGLPIBLPGJIDJHHBOEFIDCBHEEEMNNFOCPDDJCHBBGPFFIOPAIIPODFKAIPDNHEKICKJAFIDLIOGNMMCGIGLPBAHHPJMJJGMJFFDIDGCNPEHLIFENLPKMMOMOEKPOGMBBKMOPMOAMMILIFHKKMCKEIOFKGGAJPHBMCEANKHJACOOBAIDNGCDPPGBAIFLLPFPCIHLLAPDHJCKACPCEMEGKAMMPHLBPBONEBOOBEKMBPALOJIMNKBEAAAAAAEKMLGICMFODBNDFEMIEALNJNLJBHFIBFLJBCJHHJ";
                            break;
                        default:
                            throw new ApplicationException("Unknown database " + _databaseId);
                    }

                    break;

                case "MONESES-ST": // System test server.
                    switch (_databaseId)
                    {
                        case DatabaseId.SpeciesFact:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIKGEBDAPMKNOAFEKIHFGCOJNGHIAKFIAAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAALACCHJFNAHLHBLIAIOFIJNICHDNFAMHAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFCHCJNJGFONOBLPMMGHKLOPEBBIHNFAPIIAAAAAAGJFHGIOLDLBBFIDEAGNNECLOENFDNBHIDNBFHHKJJOOBBGCAJDGMNHKKGDACJCPPDPPEMECGAGPGHFOHKOFJDNIOADIGJHANJBEGBNCEANMECGIBEPGJDMMPLOKOCIOMEODLIBFJLMPJHGDLCJFGFOPIBPDPMFECFHOOGFLGPBBBLFBDGGAEIJKEJJKOBPLOPBHJEJFKKAFOACMEPIPLHCPIJFNGIANLAPPANNLMIALLJFJNDOEDEMIJGPLJFDHDOKFEPAIKOHABBIKDBEAAAAAAFMLFCLMKPEEDHGOIOGFIHBMFLNNDHGGAMNNODBDO";
                            break;
                        case DatabaseId.User:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIKGEBDAPMKNOAFEKIHFGCOJNGHIAKFIAAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACNAAACMLJLDHKHDMLJBFLKFBAMGHHLJMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKBABELAKNELGLGGMBNEOOJHADLNKNDKGIIAAAAAAHDGEJOJJHJHCNCFEOKDAOCFALDPKJKKLGAGDICBBLAOEDNOLFCAECNLGPKDFOLEFHFEGKNGHLOHIOBOOBMAFICHPOBKPJCKOCEPLNGPOMPKPBPFDFMPBGLLMKIANEGOANKKCGBBKHMMKLAMOLNFMNKAFBAJAMFJADKGJFNPCJFMADNFIMABLLMJFHOOPOEGNJNFALNICMMOHDGOHHKNNIDLMLCIJKIPOOPJECHCICNOEADBFIMGMMHPKMGMAGHNFGMEOACBGPGNCKNEPBEAAAAAADLOBOCACDKCMILJEIAJCCPBMJMMGAFMLGCLKNFBC";
                            break;
                        default:
                            throw new ApplicationException("Unknown database " + _databaseId);
                    }

                    break;

                case "SILURUS2-1":
                    switch (_databaseId)
                    {
                        case DatabaseId.SpeciesFact:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIDGFKMFBDIMAPEELJPFDBGPLIAHNCJMKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFAAGFFEGJJEPNFBIEOBKJBLNPCNKMEHMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAIBJIJHHINHJDECBANJKJNJLDGMGCHDMGJIAAAAAAICCPHHKLPDPPLNJHLFLBKOLAKECMIAFOMGLDFIFODDMIKBECJOEAOOIGLJPLGIEJBPJFBGHCFDGHLMCJNKKBKGADMOEINHJHHLBCGFOGNIPMMLJILEEEILEMPFDMDFDMFDNJNOGCNHGJLHKCNNLLDBLHHMOFDPPAMDMMLCFHNKBAHLPNHFIJMEDJHIJALIJLKJNCELEIHBBLHNOKGIBFILJAJBCGDFHPCDBLHLMIIGGCDJFANEFOODLDCADEDBEHNNNEPMKCHKPACNLFKCIJLJILDMINHIIBHPNJAHBLHOPLBODJBEAAAAAABCLMKICBBJNLDNLGPBCLCINDGHHOHLFPCOOENIPO";
                            break;
                        case DatabaseId.User:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIDGFKMFBDIMAPEELJPFDBGPLIAHNCJMKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAMHJOHILNJJHOOFIDMGCGOGMMBIBCLONDAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAANHGMCMKFJCFHKFPJOALEJHEFCALNPOBAJIAAAAAAKIHFKLIEHDMKBFGOKGJHDPLGMGBMAIKNMACELMOKMDHNBLJJALGGOHHBGPOEKIJLEKOGILEDFLBPNIMEMBENLCLBPKHNJDMNFNNDDOFJDGIENEHFKHBLOHDBECHJFNGNPCJCEGDDGGGPOCLPLAKAHAIKAEAAOCFCBNDHDGECGGCCEBEFGGFPLEOOBFAOOPOALLBLCLGAJDLDHBGOCIDLEDMPDBGFMFPILOJDIFHLMHEJLMHOHHPJLIGNGOKFOEPMHCPGDBGJBIFPFJBPGNINHCECBNHANHKFEAIBMHPLDPCCPFPHBEAAAAAAMHILJKKMOMMPJDMMCJCFLPNOFLMHGIJKMANOPBNG";
                            break;
                        default:
                            throw new ApplicationException("Unknown database " + _databaseId);
                    }

                    break;

                case "SILURUS2-2":
                    switch (_databaseId)
                    {
                        case DatabaseId.SpeciesFact:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAODPAFKAIHGMAPDEOIPNDNGAIEJHDINBGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFAKFANPHEGLMMDPKMJMOJNKLAKFPIPNMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAABDEKJEFEPEMCPGOIGBFDLEPBAACJJCAJIAAAAAAIGGMHJOKPLLNKCNIBMPDJJGHAMALEFLPAEBPMBDENJMJHMJNIBANCFIFPDJOHNABJNLHDFCNGNNCBJGPGGDGKOCEECGEAHAPOBLHCNDGNBELOPFHOJKBCCABOFABMDGJOBGCEDFCLEEDGOIHDJPDPFPEANDJICCELNNEDFIDMCBAFPNMHMDPFACJFCIIMKBFBIFGJAHKOIKLMBMMGALEDNLDBIOEFJMHHKINHPGGIFLGGPOIHLDILBNFBPLGPIAGBJFBPJOIPDILNDCONOJICPDAJOEJFLBPLBJNGAOBEJELNLFCBEAAAAAAKOCHLAOEGICDBNACCCMPPBKKEJIAPLDOHMGMNEDK";
                            break;
                        case DatabaseId.User:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAODPAFKAIHGMAPDEOIPNDNGAIEJHDINBGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFMAJNHPCGGOPANIIPPOGOBNFODMDEKMJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAEFKDHIEPPEDOPMCCEHIOEHMLENOJEJJOJIAAAAAAHGODLBMIJJLGBEFMOEOLIMOEHBCCFKMJPBFDNEGGFLCJLGFFDKOAHNLKBCGBECNDKPBKDGNNMBEBJMAOFDJGMHPIDDFEMJLLOOGKDHEHLBFGLMJOKOAJCHEOOMIPFBCIJIMAIFDPACGGBNAPJMLAMIIDDLLCHDIHHDBIEKEADPGFBDLGADHOHOLBDPLLAKHJHLCDMPDLMIIHJCFAJDKOPIBAHMPKEGFCPALPKHLFLDGIMBCNKNKEIHMPGGHKJCKNMACKJKCOKIJPAADMKOLHCFOLKPPDNDMEGGBDHHGIAIDHLLBNBEAAAAAAKANBNDLABCJNKDKGHCJOIAIHIFEIEHCKCCFMAHPI";
                            break;
                        default:
                            throw new ApplicationException("Unknown database " + _databaseId);
                    }

                    break;

                case "SLU011837": // 
                    switch (_databaseId)
                    {
                        case DatabaseId.SpeciesFact:
                            if (Configuration.Debug)
                            {
                                connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALCPBDBGCIAFPEPEILEKMLLIKEOOJMJMKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAGOCACEMHJOMPIHECHHHOEGNBPDBJPKFBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHAFHAJINLBDOCAHPCJIJAHGJEIPNLMMLIIAAAAAAAPCFFLLLDBPFLBBCJLEHGFLOHFCAKCGICHKMHNCJDGNOPIIOLAOBCPMEPKLNDAKNCDJJKPICHOKKANCKBKEINKBIPKBCDCLEIDNEMDJHIIJHILMFEHGNFBADEKHKKBDMLLBCOMNKIFLILGJLDLLPGNOPDBABBKLFKKOILFJLFOPHFNCKPEHCGNGNABJDNKHJHFPFAFIGPDNOFJKALOHPOAPBBLCDLMOENJLOHKDFIOIGOPJIPCKOOJMCDEMLPJIGFDDHDIPFFFFDHIAGBEAAAAAANOPLEPDGMKNPAINLJHPGPCCANHJCOIMLPMMJDOBI";
                            }
                            else
                            {
                                connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPFOBOEOIJOPHGCEFJGCCBMFMHLOBJJLJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFIMHDAPJNFBIMAHLOGKHILIHJIHPEELEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADKPAGOFIOCKKPKKCMAPCFFOEPIKKIHHCJIAAAAAANNEBJGFCOHAANJNNJNNOAMNPPJPBOKDAMFNLJKBIFFOGCPHEDLOJOMIJCJNLMJJEOGDICFADKHEELCNCMFIFKOKGBNGKPJMOJNJJJCHIBCAEAPBOMBDCELLIMEBAPABDKMGNNPHCIOMGPLHFJKICFAPBJIHNLDAKPPMIPMJPPDIJAJBNHEIGOCEFOACAJGBDIHNOKMBIHECFOLCAKHCPIEMILMDMBNGKOOOFMEFBOAJEFNGNOJBHGKAPIJOOENGPKBPPKLGFBFIPIEEBAPCLKHEGAIIKHCHDCJHEALCOCHGDFPOCBEAAAAAAOOCCJGLMOMDHAGKIGHKCPAMAINAIMIPAABMPKJKA";
                            }

                            break;
                        case DatabaseId.User:
                            if (Configuration.Debug)
                            {
                                connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALCPBDBGCIAFPEPEILEKMLLIKEOOJMJMKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAKKBCDEJHJNFOKNLGCBNGOBODLJJKMGAKAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAACJNFMBHEIEHCJOFKMAACHFDHFOOLCNKNIIAAAAAAACIGHKHJCKONCKBMGIDLECHPFFHEGAIKHMBECMBIGNLGGIEEGPOOKNFEIKFGHHBJPIFIPNJPOGKMKPBODBPKCINAGIMOFDFFGCLNJBMDOBFAEPCCJLKAFPNDHMDKCPIPOMOODNFLGHEMBDHAAHBANBLBJEGNDLCAPOOEFDJEKJIINDGACEHNAIFJODKFBDNAHOEDCFCGAMLCKDJDCHADKNHOAPBDOPPGPBPOCEDIHOALLDPDFPDGHNGCBDJCFHKDLADDCIBEEOBFGBJABEAAAAAABDDOAEFECFIIHCKINEIDEIAHMCGNNHEHONEKHGGK";
                            }
                            else
                            {
                                connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPFOBOEOIJOPHGCEFJGCCBMFMHLOBJJLJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAOHEKEHPCDEFCMCLBJPALKBHBLOIFJLBOAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKMOKLMKFIECANEBPENBJEOFMJJFAOJCMJIAAAAAANJEBEJAMJNDNEJGIJKPLGKPLMFLHDHMDIPMPLEMEMAGDOCIDPLFCAKDGKLHHCAEALKJFKHNILPNBPCFNPLCOBOPPMNEIILKBFKCMNCABCKNGECNBNEAGFILDIFANJBBCLGFACLFIMBGFOFFJPEJDIGCJCBFOBBGNLKKBBJBJLMKHBKEKCELAPNJNCICGEENFFLNMNECPGICICIFEFCCKNGDNBOMGBHFOGCPBMBHGNGOBPDMNGINLOHGPLDLLDNMIPMHKEECHMNHICCODBCLJOBHCBCEBPICFMDBFAAKIGONCCGBLBEAAAAAABNHKAAAOPKNFCGELBOIMOLCHJCNJCKLPLLEMMBDG";
                            }

                            break;
                        default:
                            throw new ApplicationException("Unknown database " + _databaseId);
                    }

                    break;

                case "SLU002759": // 
                    switch (_databaseId)
                    {
                        case DatabaseId.SpeciesFact:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAAIAJOOCKIMJJLEEPJMDPLEBMJCGIAHPKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPKKBGEJDGGPDBJFOPCLLFIFGIBJKJIBAAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPDACCPEFBMHDGCBEIDJKIICODHFGEBFNHIAAAAAAEAKPKFCAADMGDEJGIPBIBOBLOFEBPGGNEOPDJFDPIPCKLAMGCHGELDOEHIIEDLJNPPPHDOKCGCLMDDOBCLLCHELGADNJPJLLDDHJKJCDIPACIEOCDGPOIPAOLBJELKPNPJLNMHOPKBJECJKIMPAJNKHIPFKFBIKCAIPFEIHMNBGNEEFLEOADDFKJOLCNNNLNHJAMAJJOLLOFMMMCONNGFLDKEMFEMOAOEENBCKIIKFPBHGKLBEAAAAAAONKCBLECBPPDGAPBJLMMAICCAPGDLIBHKHODODDI";
                            break;
                        case DatabaseId.User:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAAIAJOOCKIMJJLEEPJMDPLEBMJCGIAHPKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFPABOOPJCNBCABNONGEBDJAMENBDNEEBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABGMDINIFGBMKEAOIAEGJCKGAHFNCEDKLHIAAAAAAOLHLMPMPHGAONNKNIHLHAMCIPPFPEDNJBNLOPPMAOHADMHODEKLOFLLIPGHBHLGPMPEDBDDBHJDHEICDODBAAABMFCEKFGCDKKMBBABJEOLEMNCGEGAABFLFHOEMLJLLJFABIGEAMNCCIDCKFKGFCAAABLEDBFPGAJDEHLPLDCMFPOLBBLADMMNLNECHEPNIKMCOBHJBOCDDGELDMDAKJCHINOFALBLGPKJLDGBEGCINMGGCBEAAAAAANIEPMFGAJKBPIBHLCFBOFHAGHKDAEBBGNJJEBDBM";
                            break;
                        default:
                            throw new ApplicationException("Unknown database " + _databaseId);
                    }
                    break;

                case "SLU002760": // 
                    switch (_databaseId)
                    {
                        case DatabaseId.SpeciesFact:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABAKAMJGGBJHDJCEBIMHBCBNNBPAODGGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAABCDMBMBBHPCPOPCNFJGPEMFEHLHMEANGAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADEBICKJGAKMKFPPDMLGJJHCLLJIBEEBLHIAAAAAAGLNKOJPGLJAEDPJMEAEPBIKJILELCNEFEJBDMCBFBMCFLNLFENAEFIHMOBHMEHFPAEFOHMHFCFOEICHGEPKDDNBBMIAHEKDLPMCIOFLNELPLKFMBIKOOGHCBHCOKBBOHPEDKJFIOEMBDFHJLDIIHNNBFJMJFKDOOJBOELFHOEHKMDIKOAFDHCNIJAFNOCOFOPDCALDCFDGPIINGIPAFDPEIOMAFIOKENKAIPDAENPDJNDLEEBEAAAAAAJICFAIKJNHOLICICMBNOGDOHJPDOLKEBALHJCLAJ";
                            break;
                        case DatabaseId.User:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABAKAMJGGBJHDJCEBIMHBCBNNBPAODGGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADGJNDLNIBDAIDPACNJHDMJCGNGIELBBPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAEEFGAFIAIPIJBNNLBHGOPMLEIOOIMGAEHIAAAAAAKAGNIPBCCFGCELFECOENDHPPLFDDHNLPDHPBKNFIPDKNKMMPMODDMIKOIGMKHEEDODPOCBLIBEDGLPBDLAMCIGNCHLLJILGNPELHGGBNLLGJEJBJIEJGFBAFFOFIBNJFFJLNBKHFOLAGBHLGHADDOAAIBCAFKAKMIMMFMFMOPMIMCAHHKKGCLPLPCGOLNBBCIGHIMLJIPENHJAGEHEMKIGLAOEGMAHCMNBKCCJBNMMNFNNNHBEAAAAAANPFPKDHCHHHJCOFBIMFDMJHPJLDHNIHEGJHHDBKI";
                            break;
                        default:
                            throw new ApplicationException("Unknown database " + _databaseId);
                    }
                    break;

                case "SLU003354": // 
                    switch (_databaseId)
                    {
                        case DatabaseId.SpeciesFact:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALCANIEEPNPMJJNEEJPDPFJDCBDBHMDOGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAENHGJAAMBKDOBJHJNPIMNDPADNAOGGPNAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAEPGFMNEJNAGFGLPDGPGHEJJHHIAPGOBPIIAAAAAAKAGKBKNBNCNBOKOBKBMMGCLDCFBAHBOEFEPJJBAIDMIDADPMKDPGNEAGAPNAFJCFMBEIKLMCJKKJLKLIABGDKDJAHLJEJNAPBGOBNOLBKGOFKJOJBJFDBGMAHECECBIMKCFLHCCDENHCNGMGBJCBALCFEDBAJHHBGACAELBOOGMMDKLHCCHHAJKGLIIICMMPGHKGJJIPMFDAKPIDAOBKINNPBIJMCMEGOKAJDBLCIEECIOPEMJKGPFKMHOJIMGJHCHGAPIIKMPJDLGDLBEAAAAAAJCHECJKNEPIECCDNENENMFDBGKIAEHLBDIEHINOK";
                            break;
                        case DatabaseId.User:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALCANIEEPNPMJJNEEJPDPFJDCBDBHMDOGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANLJODLGGNHPJMHCKGIEPIFKHEKBINGKJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHPFLOADIKMLEKPADELMBMDCDCAPAOJIIIIAAAAAACBMLNFHNAMNHADPOANLGIIKNHJIIENOADHLEMPNGADAGAEKCPPOKAIKJDJJCMIFNADOGPGFCLOIKLAJPLNKFFJJHHAHBGEAIBHGKKCGOIBNLLHBJHKGIADBEDHGGJLMNIIKBHDEKEPGAFDONLPABPJBMFACGKDPPOMEGPDNOJMKBNIANODNPEFOHCPIHJMOEPMJGBFAEIJCIKONEPLNLILOACJOGCLLPENHNBOFNMEDKLCPDHOLMKBEJKJLEHJPPBACKCJNNHFFOCCDPBEAAAAAALNFPHCIGNJBELOOBMJFJBIBGPNBGKMKLHODDNADF";
                            break;
                        default:
                            throw new ApplicationException("Unknown database " + _databaseId);
                    }
                    break;

                case "SEA0105WTRD2": // 
                    switch (_databaseId)
                    {
                        case DatabaseId.SpeciesFact:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABKIOIJPBLDPKPCEELADDGIHIPCLMBNMGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANGAOELLKNMBIPOPAECDGLLDEMONABIAOAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAIILGBJAAGJOCMMOLEICMJJHKOBOBDKOAIIAAAAAAKAOBFICGOLFIEGGCLJAJBOIJDGLOOLBMGKGGGIHMOFFHAJKIKILOJEALEADLNPKGDMFAMALIEGJPBHNLEPNHMMJCCOLEIKPBADBJENMNMBINMOPNNDIDJOJNFCACLPHDCKPJBLAJBBGMCKMNLEJOOLCEFHPMFJDOCNHGHJIBNFNGIFJBLALDIJMDCENHIHOMBCCMKAKGPGIDJFGMJLCPLFJGLMAKOLFACELCJCHBFCCECLDGHIJFFLGBIFGDKCGHKCGMCGEGNCMPBMMGBEAAAAAAJIJGCGFGFNLOINCLCKPFIMPMJJAHNIANGCLMHFFJ";
                            break;
                        case DatabaseId.User:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABKIOIJPBLDPKPCEELADDGIHIPCLMBNMGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHNOMGJHEIJGADNDBKIDJKNIFFHHMPIMPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJJHMEFPGHIBAFJNBEGGIDNHNLCDLBIHOIIAAAAAAFPFLBMIOCOLGHJFOFHDKFJEGOLLNDNAFMFOICNGCEDPNJMAOCJCGLJNIKOEMDHKEONGPIELNPCCLFGBJOEMFCHICNILCDCFAGEKCLBLCIFNDFCGNIGMNEPKCKCCODDAGBPOANABEIBCIFOCBICFGPHLJKJPCJMPANFDAPILEHGOBJNAACIIAHBHPFAFHBCFIPJOLDDEIAFJKMJCFNFEBBBEILDBDNKNJAPBNAEJNPGNGNFMOOOBEFILICJHJPLANCHILPGOHDHFECANMBEAAAAAAHILFDCNMACKKBPFOAEIDDBLFONMDJACEODKIPNFP";
                            break;
                        default:
                            throw new ApplicationException("Unknown database " + _databaseId);
                    }
                    break;

                case "WEBSERVICE":
                    switch (_databaseId)
                    {
                        case DatabaseId.SpeciesFact:
                            // Connection string to Titan.artdata.slu.se.
                            // connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAOFLLNFLGGGAELHENLMKEKOAAJEHOGDICAEAAAAAAACAAAAAAAAAAADGGAAAAKIAAAAAABAAAAAAAHCLBMOGBKOEGDPILFJPOJGAMLEOEENNBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPHEILJKMEPMLBKHDFDPCDFLOBIDKJLPKGIAAAAAAHANLPNAFBKDNLHLCDPDPPNNDDPMJPHDOHKAPNAEGBPEMIBNFGHAGLLBCLJEFPNBOMDEOEFCCOIMLLLBHGLILLGFPONMLBHLPNAMDOOFLCKGJMMDJHOEOJMHMONALNNLLAGDPIIMFAHKMDENPFLJNGNKKBDBFKOEJKMICFOIAPDLNFBCBGANPFHLHLOCJALNIFCEIGIIKNPBDIHHMBEAAAAAADJAPHEPBPEMOBJAMFPNDCOLLAPDOBDMFEFGAILFI";

                            // Connection string to sql-esox2-1.artdata.slu.se\artinst3.
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAMEFEGICNKOBOMAEOLOAAMGADIECHJFHJAEAAAAAAACAAAAAAAAAAADGGAAAAKIAAAAAABAAAAAAAIOOGPBHPHEABONHHDCMIHFEHADDCGKEIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAANFBABJFKNLBGBNKGCMNNOBCHEAEHFJIIJAAAAAAAGDLACCEKFGPGADFDGPKGLBBLBHFOKPAMNGNJOPAOIHPJPGMIKCHDIOPBDGJOLKFMMAKNJIPICOAFJEFDHMAEELMHBOBGPEBLBCHFMPJADJODBGDFDLNNAOLDKMKDKLBMCHFALNOPIOJECGCPJKHEDAMOKEGOKIBINHOHMONJFOPDFHMJPJADNFFLFLLNNKBJDLCLDOINBIHCLFADGOBAHJNPIDNNCAKBAAEHOIIDFLENIPECFOLAKIOONABMIBNLHKACFIPDFHGOFGPPMEIDDAEFHMGKADMFBEAAAAAAHJIJDNDACPKLOOKPJEJNODLGKMDCPHKKPHBCPNHJ";
                            break;
                        case DatabaseId.User:
                            // Connection string to Titan.artdata.slu.se.
                            // connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAOFLLNFLGGGAELHENLMKEKOAAJEHOGDICAEAAAAAAACAAAAAAAAAAADGGAAAAKIAAAAAABAAAAAAAFCDHCFODDANHIOJJFGFNHHALLAEBIFDIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAACMKFJHALMINEFDNPOJDMJEDNBBNGJPBAGIAAAAAAMHKODDCCEAAALFFCHNCJJMOIEOLHHAJDEGNIGKDAFGHGAJDDJIKEONMPJELDKIFHMGIDMMEDPALICEMEEMAKCPHKMHJAMCLGKJFHOAMHIDIMDLBKECJBFDJOCNKJPFKBEENKKHOIFDKPIMPIMCIOAIMNDDGKJCOGDNNJDAOGCKIMLEAAONDLKAJMBGJHHEOELNALFICDAFMHIBAFBEAAAAAAFMJLPBLNGJDDLJHMMHOHHMMIIKEILBEEKJKMNAJC";

                            // Connection string to sql-esox2-1.artdata.slu.se\artinst3.
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAMEFEGICNKOBOMAEOLOAAMGADIECHJFHJAEAAAAAAACAAAAAAAAAAADGGAAAAKIAAAAAABAAAAAAAFFGDGEMFNDFLCPHEMBOMKIDMHEPNNJIMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALLFKKEANMGIJKLKBJBHMCOGHHCALBFGJJAAAAAAAGFKPLBIAJCCHHEPLJOEFGNDNBIMNLDBCOKDOGBMEPNHDBJAOFNHNKGFPDNPOJLNIGPJMCCGOKKGAKGIEGKNOIJOCAPALIFKGEDDBLHIBDANPILJHNNKEGKCJEAKLFDOBNGOMOJGKEDMNHNIPGPHEIGGIPHGPHFCICIDKANHDHBMGFHOPGDAPIHPFODJCCJAAMPLDAJKICDKJBEECODCPMKMMFDHBLLOCCFINNOJGDCNBEIPJGIKCMPBIOEGBJFAJDOIIJLEPOBGOPGDGIOANKHJDKGIDEMLEBEAAAAAAANEFMNLMAEAEJHKJOHJOMGDCMCEKAGMPAIKDJNPL";
                            break;
                        default:
                            throw new ApplicationException("Unknown database " + _databaseId);
                    }
                    break;

                case "SLW-DEV": // Team Two Blueberries test server.
                    switch (_databaseId)
                    {
                        case DatabaseId.SpeciesFact:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALGJGIAKACJFAGCIGBHNPKHNCPJMEHJMOAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJJCGMCHEJHBPPIKEBDCCFKHJJOOKJGEGIIAAAAAAGBMOKPDGEBDMPMBLEKFPCAOGGCLCDMKMOKMKMLFCGCLFCNDGCGLFHMGOJBNAPLDDPOEANIGJAMOLKEHONPEJDFOEANFDNJBFDCHFKPPJMFBMHGIIOPCAOBMNOFGFNAJCBCHKIMPBDPMMNMJEELLHODCHKDOGBDAOBKHHBEDABAMKNGALJFAMEBHBGFOIJGAONFIEJFLNLJJGHELJPFBMFOOJAPENMJNCNGLHCKEJAJOOPPIFBBLHAAJMOPCLMELGPCDJLLIKLGKONGCABEAAAAAAKMKJPGJOMEOGALJCBJEAGLHKCDMMOIJOCDIIPPNL";
                            break;
                        case DatabaseId.User:
                            connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJMDPCPHKIPMOBFEDFDHLLGGJHBCBCIIKAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFMANEJEIGMAGPAKBLBDJDILMJKNIDKGGIIAAAAAAIIJANCLECJFLEPICHLPGFHOEIAAODEPDJPHBJOCFCIFOAOEEGJNKLHBMPNGLCEGKJIEPNDIKIEPICOONNOOEJIOAFJDEJFEINMOLNHLEJMCPLIPOOMEPIFKDKAPMECMAMLINNOKMLIGHGFGKCDPFNOCMLGHKLJNIFCHBGEMEPOINCHMOCPAELEPEHHFHJAFNMDBNOGBHPOFBJBGOOGEHNLGBKEGKFMLHFMCFLIIDHAHMCJCNPIONJMEJHJPLHNKBFCDEIAIMKGDCENFMBEAAAAAAMIHJFFANJFPDKGACDCDGNPNKOHPCAJCFHKKFEKMH";
                            break;
                        default:
                            throw new ApplicationException("Unknown database " + _databaseId);
                    }

                    break;

                default:
                    throw new ApplicationException("Unknown web server " + Environment.MachineName);
               
            }
            return cipherString.DecryptText(connectionString);
        }

        /// <summary>
        /// Create a new reference in database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="name">name of reference.</param>
        /// <param name="year">Year the reference was published.</param>
        /// <param name="text">Text about the reference.</param>
        /// <param name="person">Person inserting the reference.</param>  
        public static void CreateReference(WebServiceContext context,
                                           String name,
                                           Int32 year,
                                           String text,
                                           String person)
        {
            DataServer transactionDatabase;
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("CreateReference");
            commandBuilder.AddParameter(ReferenceData.NAME, name);
            commandBuilder.AddParameter(ReferenceData.YEAR, year);
            commandBuilder.AddParameter(ReferenceData.TEXT, text);
            commandBuilder.AddParameter(ReferenceData.PERSON, person);
            transactionDatabase = context.GetTransactionDatabase();
            if (transactionDatabase.IsNotNull())
            {
                lock (transactionDatabase)
                {
                    if (transactionDatabase.HasPendingTransaction())
                    {
                        transactionDatabase.ExecuteCommand(commandBuilder);
                    }
                    // ELSE Transaction has been aborted. No need to update.
                }
            }
            // ELSE Transaction has been aborted. No need to update.
        }

        /// <summary>
        /// Create species facts in the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name='speciesFactTable'>Table with taxa.</param>
        public static void CreateSpeciesFacts(WebServiceContext context,
                                              DataTable speciesFactTable)
        {
            DataServer transactionDatabase;

            transactionDatabase = context.GetTransactionDatabase();
            if (transactionDatabase.IsNotNull())
            {
                lock (transactionDatabase)
                {
                    if (transactionDatabase.HasPendingTransaction())
                    {
                        transactionDatabase.AddTableData(speciesFactTable);
                    }
                    // ELSE Transaction has been aborted. No need to update.
                }
            }
            // ELSE Transaction has been aborted. No need to update.
        }

        /// <summary>
        /// Create taxon related tables.
        /// This method is used during taxon information update.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void CreateTaxonTables(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("CreateTaxonTables");
            context.GetDatabase(DatabaseId.SpeciesFact).ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Delete species facts.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteSpeciesFacts(WebServiceContext context)
        {
            DataServer transactionDatabase;
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("DeleteSpeciesFacts");
            commandBuilder.AddParameter(SpeciesFactData.REQUEST_ID, context.RequestId);
            transactionDatabase = context.GetTransactionDatabase();
            if (transactionDatabase.IsNotNull())
            {
                lock (transactionDatabase)
                {
                    if (transactionDatabase.HasPendingTransaction())
                    {
                        transactionDatabase.ExecuteCommand(commandBuilder);
                    }
                    // ELSE Transaction has been aborted. No need to update.
                }
            }
            // ELSE Transaction has been aborted. No need to update.
        }

        /// <summary>
        /// Delete trace information from the web service log.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteTrace(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("DeleteWebServiceTrace");
            context.GetDatabase(DatabaseId.SpeciesFact).ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Delete user selected factors that has been
        /// used in this web service request.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteUserSelectedFactors(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("DeleteUserSelectedFactors");
            commandBuilder.AddParameter(UserSelectedFactorData.REQUEST_ID, context.RequestId);
            context.GetDatabase(DatabaseId.SpeciesFact).ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Delete user selected hosts that has been
        /// used in this web service request.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteUserSelectedHosts(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("DeleteUserSelectedHosts");
            commandBuilder.AddParameter(UserSelectedHostData.REQUEST_ID, context.RequestId);
            context.GetDatabase(DatabaseId.SpeciesFact).ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Delete user selected individual categories that has been
        /// used in this web service request.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteUserSelectedIndividualCategories(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("DeleteUserSelectedIndividualCategories");
            commandBuilder.AddParameter(UserSelectedIndividualCategoryData.REQUEST_ID, context.RequestId);
            context.GetDatabase(DatabaseId.SpeciesFact).ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Delete user selected parameters that has been
        /// used in this web service request.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteUserSelectedParameters(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("DeleteUserSelectedParameters");
            commandBuilder.AddParameter(UserSelectedParameterData.REQUEST_ID, context.RequestId);
            context.GetDatabase(DatabaseId.SpeciesFact).ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Delete user selected periods that has been
        /// used in this web service request.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteUserSelectedPeriods(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("DeleteUserSelectedPeriods");
            commandBuilder.AddParameter(UserSelectedPeriodData.REQUEST_ID, context.RequestId);
            context.GetDatabase(DatabaseId.SpeciesFact).ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Delete user selected references that has been
        /// used in this web service request.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteUserSelectedReferences(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("DeleteUserSelectedReferences");
            commandBuilder.AddParameter(UserSelectedReferenceData.REQUEST_ID, context.RequestId);
            context.GetDatabase(DatabaseId.SpeciesFact).ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Delete user selected species facts that has been
        /// used in this web service request.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteUserSelectedSpeciesFacts(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("DeleteUserSelectedSpeciesFacts");
            commandBuilder.AddParameter(UserSelectedSpeciesFactData.REQUEST_ID, context.RequestId);
            context.GetDatabase(DatabaseId.SpeciesFact).ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Delete user selected species observations that has been
        /// used in this web service request.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteUserSelectedSpeciesObservations(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("DeleteUserSelectedSpeciesObservations");
            commandBuilder.AddParameter(UserSelectedSpeciesObservationsData.REQUEST_ID, context.RequestId);
            context.GetDatabase(DatabaseId.SpeciesFact).ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Delete user selected taxa that has been
        /// used in this web service request.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteUserSelectedTaxa(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("DeleteUserSelectedTaxa");
            commandBuilder.AddParameter(UserSelectedTaxaData.REQUEST_ID, context.RequestId);
            context.GetDatabase(DatabaseId.SpeciesFact).ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Delete user selected taxon types that has been
        /// used in this web service request.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteUserSelectedTaxonTypes(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("DeleteUserSelectedTaxonTypes");
            commandBuilder.AddParameter(UserSelectedTaxonTypesData.REQUEST_ID, context.RequestId);
            context.GetDatabase(DatabaseId.SpeciesFact).ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Disconnect from the database.
        /// </summary>
        public void Disconnect()
        {
            //Closes the current database connection.
            if (_connection.IsNotNull() &&
                ((_connection.State == ConnectionState.Open) ||
                 (_connection.State == ConnectionState.Fetching)))
            {
                if (HasPendingTransaction())
                {
                    RollbackTransaction();
                }
                _connection.Close();
                _connection = null;
            }
        }

        /// <summary>
        /// Implementation of the IDisposable interface.
        /// Releases all resources related to this request.
        /// </summary>
        public void Dispose()
        {
            Disconnect();
        }

        /// <summary>
        /// Execute a command in the database.
        /// </summary>
        /// <param name="commandBuilder">Command to execute.</param>
        /// <returns>Status information about the command execution.</returns>
        private Int32 ExecuteCommand(SqlCommandBuilder commandBuilder)
        {
            return GetCommand(commandBuilder).ExecuteNonQuery();
        }

        /// <summary>
        /// Get an Int32 value from the database.
        /// </summary>
        /// <param name="commandBuilder">Command to execute.</param>
        /// <returns>The Int32 value.</returns>
        private Int32 ExecuteScalar(SqlCommandBuilder commandBuilder)
        {
            return Convert.ToInt32(GetCommand(commandBuilder).ExecuteScalar());
        }

        /// <summary>
        /// Get information about bird nest activities.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetBirdNestActivities(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetBirdNestActivities");
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get information about cities that matches the search string.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchString">String that city name must match.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetCitiesBySearchString(WebServiceContext context,
                                                         String searchString)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetCitiesBySearchString");
            commandBuilder.AddParameter(CityData.SEARCH_STRING, searchString);

            return context.GetDatabase(DatabaseId.User).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get length of a column in the database.
        /// This method is mainly used to retrive information about
        /// text columns but is works on any data type.
        /// </summary>
        /// <param name="tableName">Name of table to get information from.</param>
        /// <param name="columnName">Name of column to get length of.</param>
        /// <returns> The column length.</returns>
        private Int32 GetColumnLength(String tableName, String columnName)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetColumnLength");
            commandBuilder.AddParameter(ColumnLenghtData.TABLE_NAME, tableName);
            commandBuilder.AddParameter(ColumnLenghtData.COLUMN_NAME, columnName);

            return ExecuteScalar(commandBuilder);
        }

        /// <summary>
        /// Get length of a column in a database.
        /// This method is mainly used to retrive information about
        /// text columns but is works on any data type.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="database">The database where the table and column is stored.</param>
        /// <param name="tableName">Name of table to get information from.</param>
        /// <param name="columnName">Name of column to get length of.</param>
        /// <returns> The column length.</returns>
        public static Int32 GetColumnLength(WebServiceContext context,
                                            DatabaseId database,
                                            String tableName,
                                            String columnName)
        {
            Hashtable columnLenghtTable;
            Int32 columnLength;
            String cacheKey, hashKey;

            // Get column length table.
            cacheKey = "ColumnLengths";
            columnLenghtTable = (Hashtable)context.GetCachedObject(cacheKey);
            if (columnLenghtTable.IsNull())
            {
                // Create column length table.
                columnLenghtTable = new Hashtable();

                // Add information to cache.
                context.AddCachedObject(cacheKey, columnLenghtTable, DateTime.Now + new TimeSpan(12, 0, 0), CacheItemPriority.AboveNormal);
            }

            // Get length of specified column:
            hashKey = "Database:" + database + "Table:" + tableName + "Column:" + columnName;
            if (columnLenghtTable.Contains(hashKey))
            {
                // Get cached value.
                columnLength = (Int32)(columnLenghtTable[hashKey]);
            }
            else
            {
                // Get value from database.
                columnLength = context.GetDatabase(database).GetColumnLength(tableName, columnName);

                // Add value to cache.
                columnLenghtTable.Add(hashKey, columnLength);
            }
            return columnLength;
        }

        /// <summary>
        /// Get a SqlCommand instance.
        /// Transaction is added if available.
        /// </summary>
        /// <param name="commandBuilder">Command to set in the SqlCommand instance.</param>
        /// <returns> The SqlCommand instance.</returns>
        private SqlCommand GetCommand(SqlCommandBuilder commandBuilder)
        {
            SqlCommand command;

            if (_transaction.IsNull())
            {
                command = new SqlCommand(commandBuilder.GetCommand(), _connection);
            }
            else
            {
                command = new SqlCommand(commandBuilder.GetCommand(), _connection, _transaction);
            }
            command.CommandTimeout = CommandTimeout;
            return command;
        }

        /// <summary>
        /// Get information about swedish counties.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetCounties(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetCounties");
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get database id.
        /// </summary>
        public DatabaseId GetDatabaseId()
        {
            return _databaseId;
        }

        /// <summary>
        /// Get information about databases.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetDatabases(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetDatabases");
            return context.GetDatabase(DatabaseId.User).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about database update.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetDatabaseUpdate(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetDatabaseUpdate");
            return context.GetDatabase(DatabaseId.SpeciesFact).GetRow(commandBuilder);
        }

        /// <summary>
        /// Get a data set.
        /// </summary>
        /// <param name="commandBuilder">Command to get data set from.</param>
        /// <returns>A DataSet with results from the Sql command.</returns>
        private DataSet GetDataSet(SqlCommandBuilder commandBuilder)
        {
            SqlDataAdapter adapter;
            DataSet dataSet;
            SqlCommand command;

            command = GetCommand(commandBuilder);
            adapter = new SqlDataAdapter();
            dataSet = new DataSet();
            adapter.SelectCommand = command;
            adapter.Fill(dataSet);
            return dataSet;
        }

        /// <summary>
        /// Get a data table.
        /// </summary>
        /// <param name="commandBuilder">Command to get data table from.</param>
        /// <returns>A DataTable with results from the Sql command.</returns>
        private DataTable GetDataTable(SqlCommandBuilder commandBuilder)
        {
            DataTable table;
            SqlCommand command;

            table = new DataTable("Result");
            command = GetCommand(commandBuilder);
            using (SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.SingleResult))
            {
                table.Load(dataReader);
            }
            return table;
        }

        /// <summary>
        /// Get information about different endangered lists.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetEndangeredLists(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetEndangeredLists");
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about all factor field enum values.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetFactorFieldEnums(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetFactorFieldEnums");
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about all factor fields
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetFactorFields(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetFactorFields");
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about all factor field types
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetFactorFieldTypes(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetFactorFieldTypes");
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about factor origins.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetFactorOrigins(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetFactorOrigins");
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about all factors
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetFactors(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetFactors");
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about all factors that matches the search criteria
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="factorNameSearchString">String to match factor names with.</param>
        /// <param name="nameSearchMethod">How search should be performed.</param>
        /// <param name="hasSearchFactors">Indicates if search is limited to user selected factors.</param>
        /// <param name="restrictSearchToScope">Indicates scope of factor search.</param>
        /// <param name="restrictReturnToScope">Indicates scope for returned factors.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetFactorsBySearchCriteria(WebServiceContext context,
                                                  String factorNameSearchString,
                                                  String nameSearchMethod,
                                                  Boolean hasSearchFactors,
                                                  String restrictSearchToScope,
                                                  String restrictReturnToScope
                                                  )
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetFactorsBySearchCriteria");
            commandBuilder.AddParameter(FactorData.REQUEST_ID, context.RequestId);
            if (factorNameSearchString.IsNotEmpty())
            {
                commandBuilder.AddParameter(FactorData.FACTOR_NAME_SEARCH_STRING, factorNameSearchString);
            }
            commandBuilder.AddParameter(FactorData.NAME_SEARCH_METHOD, nameSearchMethod);
            commandBuilder.AddParameter(FactorData.HAS_SEARCH_FACTORS, hasSearchFactors);
            commandBuilder.AddParameter(FactorData.RESTRICT_SEARCH_TO_SCOPE, restrictSearchToScope);
            commandBuilder.AddParameter(FactorData.RESTRICT_RETURN_TO_SCOPE, restrictReturnToScope);
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about all factor tree nodes.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetFactorTrees(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetFactorTrees");
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder, CommandBehavior.Default);
        }

        /// <summary>
        /// Get DataReader with information about all factor tree nodes that matches the search criteria
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="hasSearchFactors">Indication if user selected factors are used</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetFactorTreesBySearchCriteria(
            WebServiceContext context,
            Boolean hasSearchFactors)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetFactorTreesBySearchCriteria");
            commandBuilder.AddParameter(FactorTreeData.REQUEST_ID, context.RequestId);
            commandBuilder.AddParameter(FactorTreeData.HAS_SEARCH_FACTORS, hasSearchFactors);
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder, CommandBehavior.Default);
        }

        /// <summary>
        /// Get DataReader with information about all factor update modes.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetFactorUpdateModes(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetFactorUpdateModes");
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader Information about host taxa
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="factorId">Id of factor which host taxa are related to.</param>
        /// <param name="taxonInformationType">Type of host taxon information to get</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetHostTaxa(WebServiceContext context, Int32 factorId, String taxonInformationType)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetHostTaxa");
            commandBuilder.AddParameter(TaxonData.REQUEST_ID, context.RequestId);
            commandBuilder.AddParameter(TaxonData.FACTOR_ID, factorId);
            commandBuilder.AddParameter(TaxonData.TAXON_INFORMATION_TYPE, taxonInformationType);

            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get Data Reader Information about host taxa related to a sertain taxon.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">Id of the taxon.</param>
        /// <param name="taxonInformationType">Type of host taxon information to get.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetHostTaxaByTaxonId(WebServiceContext context, Int32 taxonId, String taxonInformationType)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetHostTaxaByTaxonId");
            commandBuilder.AddParameter(TaxonHostData.REQUEST_ID, context.RequestId);
            commandBuilder.AddParameter(TaxonHostData.TAXON_ID, taxonId);
            commandBuilder.AddParameter(TaxonHostData.TAXON_INFORMATION_TYPE, taxonInformationType);

            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about individual categories.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetIndividualCategories(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetIndividualCategories");
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get entries from the web service log
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="type">Get log entries of this type. May be empty.</param>
        /// <param name="userName">Get log entries for this user. May be empty.</param>
        /// <param name="rowCount">Maximum number of log entries to get.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetLog(WebServiceContext context,
                                        String type,
                                        String userName,
                                        Int32 rowCount)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetWebServiceLog");
            if (type.IsNotEmpty())
            {
                commandBuilder.AddParameter(WebServiceLogData.TYPE, type);
            }
            if (userName.IsNotEmpty())
            {
                commandBuilder.AddParameter(WebServiceLogData.WEB_SERVICE_USER, userName);
            }
            commandBuilder.AddParameter(WebServiceLogData.ROW_COUNT, rowCount);

            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get max species fact id.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Max species fact id.</returns>
        public static Int32 GetMaxSpeciesFactId(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetMaxSpeciesFactId");
            return context.GetDatabase(DatabaseId.SpeciesFact).ExecuteScalar(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about periods.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetPeriods(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetPeriods");
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about period types.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetPeriodTypes(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetPeriodTypes");
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get information about swedish provinces.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetProvinces(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetProvinces");
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get a data reader.
        /// </summary>
        /// <param name="commandBuilder">Command to get data reader from.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        private DataReader GetReader(SqlCommandBuilder commandBuilder)
        {
            return GetReader(commandBuilder, CommandBehavior.SingleResult);
        }

        /// <summary>
        /// Get a data reader.
        /// </summary>
        /// <param name="commandBuilder">Command to get data reader from.</param>
        /// <param name="commandBehavior">Command behaviour. Is used to optimize (in speed) the data fetch.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        private DataReader GetReader(SqlCommandBuilder commandBuilder,
                                     CommandBehavior commandBehavior)
        {
            SqlCommand command;

            command = GetCommand(commandBuilder);
            return new DataReader(command.ExecuteReader(commandBehavior));
        }

        /// <summary>
        /// Get DataReader with information about references.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetReferences(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetReferences");
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about references based on search string.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchString">Search string.</param>
        /// <returns></returns>
        public static DataReader GetReferencesBySearchString(WebServiceContext context, String searchString)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetReferencesBySearchString");
            commandBuilder.AddParameter(ReferenceData.SEARCH_STRING, searchString);
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get a data reader that only contains one row of data.
        /// </summary>
        /// <param name="commandBuilder">Command to get data reader from.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        private DataReader GetRow(SqlCommandBuilder commandBuilder)
        {
            return GetReader(commandBuilder, CommandBehavior.SingleRow |
                             CommandBehavior.SingleResult);
        }

        /// <summary>
        /// Get DataReader with information about all species fact qualities.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetSpeciesFactQualities(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetSpeciesFactQualities");
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        ///  Get DataReader with information about species facts
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetSpeciesFactsById(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetSpeciesFacts");
            commandBuilder.AddParameter(SpeciesFactData.REQUEST_ID, context.RequestId);
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        ///  Get DataReader with information about species facts
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetSpeciesFactsByIdentifier(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetSpeciesFactsByIdentifier");
            commandBuilder.AddParameter(SpeciesFactData.REQUEST_ID, context.RequestId);
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about all species facts that belongs to user parameter selection
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="hasSearchFactors">Indication if user selected factors are used.</param>
        /// <param name="hasSearchTaxa">Indication if user selected taxa are used.</param>
        /// <param name="hasSearchIndividualCategories">Indication if user selected Individual categories are used.</param>
        /// <param name="hasSearchPeriods">Indication if user selected periods are used.</param>
        /// <param name="hasSearchHosts">Indication if user selected hosts are used.</param>
        /// <param name="hasSearchReferences">Indication if user selected references are used.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetSpeciesFactsByUserParameterSelection(WebServiceContext context,
                                                                         Boolean hasSearchFactors,
                                                                         Boolean hasSearchTaxa,
                                                                         Boolean hasSearchIndividualCategories,
                                                                         Boolean hasSearchPeriods,
                                                                         Boolean hasSearchHosts,
                                                                         Boolean hasSearchReferences)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetSpeciesFactsByUserParameterSelection");
            commandBuilder.AddParameter(SpeciesFactData.REQUEST_ID, context.RequestId);
            commandBuilder.AddParameter(SpeciesFactData.HAS_SEARCH_FACTORS, hasSearchFactors);
            commandBuilder.AddParameter(SpeciesFactData.HAS_SEARCH_TAXA, hasSearchTaxa);
            commandBuilder.AddParameter(SpeciesFactData.HAS_SEARCH_INDIVIDUAL_CATEGORIES, hasSearchIndividualCategories);
            commandBuilder.AddParameter(SpeciesFactData.HAS_SEARCH_PERIODS, hasSearchPeriods);
            commandBuilder.AddParameter(SpeciesFactData.HAS_SEARCH_HOSTS, hasSearchHosts);
            commandBuilder.AddParameter(SpeciesFactData.HAS_SEARCH_REFERENCES, hasSearchReferences);
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get all species observations that has 
        ///	changed in the specified date interval.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="maxProtectionLevel">Max protection level.</param>
        /// <param name="changedFrom">Changed from.</param>
        /// <param name="changedTo">Changed to.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetSpeciesObservationChange(WebServiceContext context,
                                                             Int32 maxProtectionLevel,
                                                             DateTime changedFrom,
                                                             DateTime changedTo)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetSpeciesObservationChange");
            commandBuilder.AddParameter(SpeciesObservationData.MAX_PROTECTION_LEVEL, maxProtectionLevel);
            commandBuilder.AddParameter(SpeciesObservationData.CHANGED_FROM, changedFrom);
            commandBuilder.AddParameter(SpeciesObservationData.CHANGED_TO, changedTo);
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder, CommandBehavior.Default);
        }

        /// <summary>
        /// Get number of species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="securityMinPower">Security min power.</param>
        /// <param name="securityMaxPower">Security max power.</param>
        /// <param name="hasUserRoleTaxonId">Indicates if user role taxon id is used.</param>
        /// <param name="userRoleTaxonId">User role taxon id.</param>
        /// <param name="hasUserRoleRegionId">Indicates if user role region id is used.</param>
        /// <param name="userRoleRegionId">User role region id.</param>
        /// <param name="distorsion">Distorsion of observation coordinates.</param>
        /// <param name="hideAttributes">Hide observation attributes.</param>
        /// <param name="hasUserSelectedTaxa">Indicates if user selected taxa has been added to the database.</param>
        /// <param name="useOfObservationDate">Use of observation date.</param>
        /// <param name="hasObservationDate">Indicates if observation date is used.</param>
        /// <param name="observationStartDate">Observation start date.</param>
        /// <param name="observationEndDate">Observation end date.</param>
        /// <param name="useOfRegistrationDate">Use of registration date.</param>
        /// <param name="hasRegistrationDate">Indicates if registration date is used.</param>
        /// <param name="registrationStartDate">Registration start date.</param>
        /// <param name="registrationEndDate">Registration end date.</param>
        /// <param name="isRectangleSpecified">Indicates if a rectangle has been specified.</param>
        /// <param name="northCoordinate">North coordinate.</param>
        /// <param name="southCoordinate">South coordinate.</param>
        /// <param name="eastCoordinate">East coordinate.</param>
        /// <param name="westCoordinate">West coordinate.</param>
        /// <param name="isAccuracySpecified">Indicates if accuracy is specified.</param>
        /// <param name="accuracy">Accuracy.</param>
        /// <param name="locationSearchString">Location search string.</param>
        /// <param name="includePositiveObservations">Include positive observations.</param>
        /// <param name="includeNeverFoundObservations">Include never found observations.</param>
        /// <param name="includeNotRediscoveredObservations">Include not rediscovered observations.</param>
        /// <param name="hasDatabaseId">Indicates if parameter id for database is used.</param>
        /// <param name="databaseId">Id for database.</param>
        /// <param name="hasProvinceId">Indicates if parameter id for province is used.</param>
        /// <param name="provinceId">Id for province.</param>
        /// <param name="hasProvincePartId">Indicates if parameter id for province part is used.</param>
        /// <param name="provincePartId">Id for province part.</param>
        /// <param name="hasCountyId">Indicates if parameter id for county is used.</param>
        /// <param name="countyId">Id for county.</param>
        /// <param name="hasCountyPartId">Indicates if parameter id for county part is used.</param>
        /// <param name="countyPartId">Id for county part.</param>
        /// <param name="isBirdNestActivityLevelSpecified">Indicates if parameter bird nest activity level is used.</param>
        /// <param name="birdNestActivityLevel">Bird nest activity level.</param>
        /// <param name="observerSearchString">Observer search string.</param>
        /// <param name="includeUncertainTaxonDetermination">Include observations where observer is uncertain about taxon determination.</param>
        /// <returns>Number of species observations that matches the search criteria.</returns>
        public static Int32 GetSpeciesObservationCountBySearchCriteria(WebServiceContext context,
                                                                       Int32 securityMinPower,
                                                                       Int32 securityMaxPower,
                                                                       Boolean hasUserRoleTaxonId,
                                                                       Int32 userRoleTaxonId,
                                                                       Boolean hasUserRoleRegionId,
                                                                       Int32 userRoleRegionId,
                                                                       Int32 distorsion,
                                                                       Boolean hideAttributes,
                                                                       Boolean hasUserSelectedTaxa,
                                                                       Int32 useOfObservationDate,
                                                                       Boolean hasObservationDate,
                                                                       DateTime observationStartDate,
                                                                       DateTime observationEndDate,
                                                                       Int32 useOfRegistrationDate,
                                                                       Boolean hasRegistrationDate,
                                                                       DateTime registrationStartDate,
                                                                       DateTime registrationEndDate,
                                                                       Boolean isRectangleSpecified,
                                                                       Int32 northCoordinate,
                                                                       Int32 southCoordinate,
                                                                       Int32 eastCoordinate,
                                                                       Int32 westCoordinate,
                                                                       Boolean isAccuracySpecified,
                                                                       Int32 accuracy,
                                                                       String locationSearchString,
                                                                       Boolean includePositiveObservations,
                                                                       Boolean includeNeverFoundObservations,
                                                                       Boolean includeNotRediscoveredObservations,
                                                                       Boolean hasDatabaseId,
                                                                       Int32 databaseId,
                                                                       Boolean hasProvinceId,
                                                                       Int32 provinceId,
                                                                       Boolean hasProvincePartId,
                                                                       Int32 provincePartId,
                                                                       Boolean hasCountyId,
                                                                       Int32 countyId,
                                                                       Boolean hasCountyPartId,
                                                                       Int32 countyPartId,
                                                                       Boolean isBirdNestActivityLevelSpecified,
                                                                       Int32 birdNestActivityLevel,
                                                                       String observerSearchString,
                                                                       Boolean includeUncertainTaxonDetermination)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetSpeciesObservationCount");
            commandBuilder.AddParameter("roleMinPower", securityMinPower);
            commandBuilder.AddParameter("roleMaxPower", securityMaxPower);
            if (hasUserRoleTaxonId)
            {
                commandBuilder.AddParameter("roleTaxaId", userRoleTaxonId);
            }
            if (hasUserRoleRegionId)
            {
                commandBuilder.AddParameter("roleRegionId", userRoleRegionId);
            }
            commandBuilder.AddParameter("roleDistorsion", distorsion);
            commandBuilder.AddParameter("roleHideAttributes", hideAttributes);
            if (hasUserSelectedTaxa)
            {
                commandBuilder.AddParameter("SearchId", context.RequestId);
            }
            commandBuilder.AddParameter("UseOfObservationDate", useOfObservationDate);
            if (hasObservationDate)
            {
                commandBuilder.AddParameter("ObservationStartDate", observationStartDate.ToShortDateString());
                commandBuilder.AddParameter("ObservationEndDate", observationEndDate.ToShortDateString());
            }
            commandBuilder.AddParameter("UseOfRegistrationDate", useOfRegistrationDate);
            if (hasRegistrationDate)
            {
                commandBuilder.AddParameter("RegistrationStartDate", registrationStartDate.ToShortDateString());
                commandBuilder.AddParameter("RegistrationEndDate", registrationEndDate.ToShortDateString());
            }
            if (isRectangleSpecified)
            {
                commandBuilder.AddParameter("MaxNorthCoordinate", northCoordinate);
                commandBuilder.AddParameter("MinNorthCoordinate", southCoordinate);
                commandBuilder.AddParameter("MaxEastCoordinate", eastCoordinate);
                commandBuilder.AddParameter("MinEastCoordinate", westCoordinate);
            }
            if (isAccuracySpecified)
            {
                commandBuilder.AddParameter("Accuracy", accuracy);
            }
            if (locationSearchString.IsNotEmpty())
            {
                commandBuilder.AddParameter("LocationName", locationSearchString);
            }
            commandBuilder.AddParameter("IncludePositiveObservations", includePositiveObservations);
            commandBuilder.AddParameter("IncludeNeverFoundObservations", includeNeverFoundObservations);
            commandBuilder.AddParameter("IncludeNotRediscoveredObservations", includeNotRediscoveredObservations);
            if (hasDatabaseId)
            {
                commandBuilder.AddParameter("DatabaseId", databaseId);
            }
            if (hasProvinceId)
            {
                commandBuilder.AddParameter("ProvinceId", provinceId);
            }
            if (hasProvincePartId)
            {
                commandBuilder.AddParameter("ProvincePartId", provincePartId);
            }
            if (hasCountyId)
            {
                commandBuilder.AddParameter("CountyId", countyId);
            }
            if (hasCountyPartId)
            {
                commandBuilder.AddParameter("CountyPartId", countyPartId);
            }
            if (isBirdNestActivityLevelSpecified)
            {
                commandBuilder.AddParameter("BirdNestActivityLevel", birdNestActivityLevel);
            }
            if (observerSearchString.IsNotEmpty())
            {
                commandBuilder.AddParameter("ObserverSearchString", observerSearchString);
            }
            commandBuilder.AddParameter("IncludeUncertainTaxonDetermination", includeUncertainTaxonDetermination);

            return context.GetDatabase(DatabaseId.SpeciesFact).ExecuteScalar(commandBuilder);
        }

        /// <summary>
        /// Get species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="securityMinPower">Security min power.</param>
        /// <param name="securityMaxPower">Security max power.</param>
        /// <param name="hasUserRoleTaxonId">Indicates if user role taxon id is used.</param>
        /// <param name="userRoleTaxonId">User role taxon id.</param>
        /// <param name="hasUserRoleRegionId">Indicates if user role region id is used.</param>
        /// <param name="userRoleRegionId">User role region id.</param>
        /// <param name="distorsion">Distorsion of observation coordinates.</param>
        /// <param name="hideAttributes">Hide observation attributes.</param>
        /// <param name="hasUserSelectedTaxa">Indicates if user selected taxa has been added to the database.</param>
        /// <param name="useOfObservationDate">Use of observation date.</param>
        /// <param name="hasObservationDate">Indicates if observation date is used.</param>
        /// <param name="observationStartDate">Observation start date.</param>
        /// <param name="observationEndDate">Observation end date.</param>
        /// <param name="useOfRegistrationDate">Use of registration date.</param>
        /// <param name="hasRegistrationDate">Indicates if registration date is used.</param>
        /// <param name="registrationStartDate">Registration start date.</param>
        /// <param name="registrationEndDate">Registration end date.</param>
        /// <param name="isRectangleSpecified">Indicates if a rectangle has been specified.</param>
        /// <param name="northCoordinate">North coordinate.</param>
        /// <param name="southCoordinate">South coordinate.</param>
        /// <param name="eastCoordinate">East coordinate.</param>
        /// <param name="westCoordinate">West coordinate.</param>
        /// <param name="isAccuracySpecified">Indicates if accuracy is specified.</param>
        /// <param name="accuracy">Accuracy.</param>
        /// <param name="locationSearchString">Location search string.</param>
        /// <param name="includePositiveObservations">Include positive observations.</param>
        /// <param name="includeNeverFoundObservations">Include never found observations.</param>
        /// <param name="includeNotRediscoveredObservations">Include not rediscovered observations.</param>
        /// <param name="hasDatabaseId">Indicates if parameter id for database is used.</param>
        /// <param name="databaseId">Id for database.</param>
        /// <param name="hasProvinceId">Indicates if parameter id for province is used.</param>
        /// <param name="provinceId">Id for province.</param>
        /// <param name="hasProvincePartId">Indicates if parameter id for province part is used.</param>
        /// <param name="provincePartId">Id for province part.</param>
        /// <param name="hasCountyId">Indicates if parameter id for county is used.</param>
        /// <param name="countyId">Id for county.</param>
        /// <param name="hasCountyPartId">Indicates if parameter id for county part is used.</param>
        /// <param name="countyPartId">Id for county part.</param>
        /// <param name="isBirdNestActivityLevelSpecified">Indicates if parameter bird nest activity level is used.</param>
        /// <param name="birdNestActivityLevel">Bird nest activity level.</param>
        /// <param name="speciesObservationInformationType">What information about species observation to get.</param>
        /// <param name="observerSearchString">Observer search string.</param>
        /// <param name="includeUncertainTaxonDetermination">Include observations where observer is uncertain about taxon determination.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetSpeciesObservations(WebServiceContext context,
                                                        Int32 securityMinPower,
                                                        Int32 securityMaxPower,
                                                        Boolean hasUserRoleTaxonId,
                                                        Int32 userRoleTaxonId,
                                                        Boolean hasUserRoleRegionId,
                                                        Int32 userRoleRegionId,
                                                        Int32 distorsion,
                                                        Boolean hideAttributes,
                                                        Boolean hasUserSelectedTaxa,
                                                        Int32 useOfObservationDate,
                                                        Boolean hasObservationDate,
                                                        DateTime observationStartDate,
                                                        DateTime observationEndDate,
                                                        Int32 useOfRegistrationDate,
                                                        Boolean hasRegistrationDate,
                                                        DateTime registrationStartDate,
                                                        DateTime registrationEndDate,
                                                        Boolean isRectangleSpecified,
                                                        Int32 northCoordinate,
                                                        Int32 southCoordinate,
                                                        Int32 eastCoordinate,
                                                        Int32 westCoordinate,
                                                        Boolean isAccuracySpecified,
                                                        Int32 accuracy,
                                                        String locationSearchString,
                                                        Boolean includePositiveObservations,
                                                        Boolean includeNeverFoundObservations,
                                                        Boolean includeNotRediscoveredObservations,
                                                        Boolean hasDatabaseId,
                                                        Int32 databaseId,
                                                        Boolean hasProvinceId,
                                                        Int32 provinceId,
                                                        Boolean hasProvincePartId,
                                                        Int32 provincePartId,
                                                        Boolean hasCountyId,
                                                        Int32 countyId,
                                                        Boolean hasCountyPartId,
                                                        Int32 countyPartId,
                                                        Boolean isBirdNestActivityLevelSpecified,
                                                        Int32 birdNestActivityLevel,
                                                        String speciesObservationInformationType,
                                                        String observerSearchString,
                                                        Boolean includeUncertainTaxonDetermination)
        { 
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetSpeciesObservations");
            commandBuilder.AddParameter("roleMinPower", securityMinPower);
            commandBuilder.AddParameter("roleMaxPower", securityMaxPower);
            if (hasUserRoleTaxonId)
            {
                commandBuilder.AddParameter("roleTaxaId", userRoleTaxonId);
            }
            if (hasUserRoleRegionId)
            {
                commandBuilder.AddParameter("roleRegionId", userRoleRegionId);
            }
            commandBuilder.AddParameter("roleDistorsion", distorsion);
            commandBuilder.AddParameter("roleHideAttributes", hideAttributes);
            if (hasUserSelectedTaxa)
            {
                commandBuilder.AddParameter("SearchId", context.RequestId);
            }
            commandBuilder.AddParameter("UseOfObservationDate", useOfObservationDate);
            if (hasObservationDate)
            {
                commandBuilder.AddParameter("ObservationStartDate", observationStartDate.ToShortDateString());
                commandBuilder.AddParameter("ObservationEndDate", observationEndDate.ToShortDateString());
            }
            commandBuilder.AddParameter("UseOfRegistrationDate", useOfRegistrationDate);
            if (hasRegistrationDate)
            {
                commandBuilder.AddParameter("RegistrationStartDate", registrationStartDate.ToShortDateString());
                commandBuilder.AddParameter("RegistrationEndDate", registrationEndDate.ToShortDateString());
            }
            if (isRectangleSpecified)
            {
                commandBuilder.AddParameter("MaxNorthCoordinate", northCoordinate);
                commandBuilder.AddParameter("MinNorthCoordinate", southCoordinate);
                commandBuilder.AddParameter("MaxEastCoordinate", eastCoordinate);
                commandBuilder.AddParameter("MinEastCoordinate", westCoordinate);
            }
            if (isAccuracySpecified)
            {
                commandBuilder.AddParameter("Accuracy", accuracy);
            }
            if (locationSearchString.IsNotEmpty())
            {
                commandBuilder.AddParameter("LocationName", locationSearchString);
            }
            commandBuilder.AddParameter("IncludePositiveObservations", includePositiveObservations);
            commandBuilder.AddParameter("IncludeNeverFoundObservations", includeNeverFoundObservations);
            commandBuilder.AddParameter("IncludeNotRediscoveredObservations", includeNotRediscoveredObservations);
            if (hasDatabaseId)
            {
                commandBuilder.AddParameter("DatabaseId", databaseId);
            }
            if (hasProvinceId)
            {
                commandBuilder.AddParameter("ProvinceId", provinceId);
            }
            if (hasProvincePartId)
            {
                commandBuilder.AddParameter("ProvincePartId", provincePartId);
            }
            if (hasCountyId)
            {
                commandBuilder.AddParameter("CountyId", countyId);
            }
            if (hasCountyPartId)
            {
                commandBuilder.AddParameter("CountyPartId", countyPartId);
            }
            if (isBirdNestActivityLevelSpecified)
            {
                commandBuilder.AddParameter("BirdNestActivityLevel", birdNestActivityLevel);
            }
            commandBuilder.AddParameter("SpeciesObservationInformationType", speciesObservationInformationType);
            if (observerSearchString.IsNotEmpty())
            {
                commandBuilder.AddParameter("ObserverSearchString", observerSearchString);
            }
            commandBuilder.AddParameter("IncludeUncertainTaxonDetermination", includeUncertainTaxonDetermination);

            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get requested species observations.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="securityMinPower">Security min power.</param>
        /// <param name="securityMaxPower">Security max power.</param>
        /// <param name="hasUserRoleTaxonId">Indicates if user role taxon id is used.</param>
        /// <param name="userRoleTaxonId">User role taxon id.</param>
        /// <param name="hasUserRoleRegionId">Indicates if user role region id is used.</param>
        /// <param name="userRoleRegionId">User role region id.</param>
        /// <param name="distorsion">Distorsion of observation coordinates.</param>
        /// <param name="hideAttributes">Hide observation attributes.</param>
        /// <param name="speciesObservationInformationType">What information about species observation to get.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetSpeciesObservationsById(WebServiceContext context,
                                                            Int32 securityMinPower,
                                                            Int32 securityMaxPower,
                                                            Boolean hasUserRoleTaxonId,
                                                            Int32 userRoleTaxonId,
                                                            Boolean hasUserRoleRegionId,
                                                            Int32 userRoleRegionId,
                                                            Int32 distorsion,
                                                            Boolean hideAttributes,
                                                            String speciesObservationInformationType)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetSpeciesObservationsById");
            commandBuilder.AddParameter("roleMinPower", securityMinPower);
            commandBuilder.AddParameter("roleMaxPower", securityMaxPower);
            if (hasUserRoleTaxonId)
            {
                commandBuilder.AddParameter("roleTaxaId", userRoleTaxonId);
            }
            if (hasUserRoleRegionId)
            {
                commandBuilder.AddParameter("roleRegionId", userRoleRegionId);
            }
            commandBuilder.AddParameter("roleDistorsion", distorsion);
            commandBuilder.AddParameter("roleHideAttributes", hideAttributes);
            commandBuilder.AddParameter("RequestId", context.RequestId);
            commandBuilder.AddParameter("SpeciesObservationInformationType", speciesObservationInformationType);

            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about taxa.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonInformationType">Type of taxa information to get.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetTaxa(WebServiceContext context,
                                         String taxonInformationType)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetTaxa");
            commandBuilder.AddParameter(TaxonData.REQUEST_ID, context.RequestId);
            commandBuilder.AddParameter(TaxonData.TAXON_INFORMATION_TYPE, taxonInformationType);
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get Data Reader Information about taxa utelizing a sertain host taxon.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="hostTaxonId">Id of the host taxon.</param>
        /// <param name="taxonInformationType">Type of host taxon information to get.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetTaxaByHostTaxonId(WebServiceContext context, Int32 hostTaxonId, String taxonInformationType)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetTaxaByHostTaxonId");
            commandBuilder.AddParameter(TaxonHostData.REQUEST_ID, context.RequestId);
            commandBuilder.AddParameter(TaxonHostData.HOST_TAXON_ID, hostTaxonId);
            commandBuilder.AddParameter(TaxonHostData.TAXON_INFORMATION_TYPE, taxonInformationType);

            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get information about taxa that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="hasOrganismGroupId">Indicates if organism group id is set.</param>
        /// <param name="organismGroupId">Organism group id.</param>
        /// <param name="hasEndangeredListId">Indicates if endangered list id is set.</param>
        /// <param name="endangeredListId">Endangered list id.</param>
        /// <param name="hasRedlistCategoryId">Indicates if redlist category id is set.</param>
        /// <param name="redlistCategoryId">Redlist category id.</param>
        /// <param name="taxonInformationType">Type of taxa information to get.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetTaxaByOrganismOrRedlist(WebServiceContext context,
                                                            Boolean hasOrganismGroupId,
                                                            Int32 organismGroupId,
                                                            Boolean hasEndangeredListId,
                                                            Int32 endangeredListId,
                                                            Boolean hasRedlistCategoryId,
                                                            Int32 redlistCategoryId,
                                                            String taxonInformationType)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetTaxaByOrganismOrRedlistGroup");
            commandBuilder.AddParameter(TaxonData.REQUEST_ID, context.RequestId);
            commandBuilder.AddParameter(TaxonData.TAXON_INFORMATION_TYPE, taxonInformationType);
            if (hasOrganismGroupId)
            {
                commandBuilder.AddParameter("organismGroupId", organismGroupId);
            }
            if (hasEndangeredListId)
            {
                commandBuilder.AddParameter("endangeredListId", endangeredListId);
            }
            if (hasRedlistCategoryId)
            {
                commandBuilder.AddParameter("redlistCategory", redlistCategoryId);
            }
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about taxa that matches the query.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="query">A taxa search query.</param>
        /// <param name="taxonInformationType">Type of taxon information to get.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetTaxaByQuery(WebServiceContext context,
                                                String query,
                                                String taxonInformationType)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetTaxaByQuery");
            commandBuilder.AddParameter(TaxonData.REQUEST_ID, context.RequestId);
            commandBuilder.AddParameter(TaxonData.QUERY, query);
            commandBuilder.AddParameter(TaxonData.TAXON_INFORMATION_TYPE, taxonInformationType);
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about taxa that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonNameSearchString">The taxon name search string.</param>
        /// <param name="taxonInformationType">Type of taxon information to get.</param>
        /// <param name="hasSearchTaxa">Indication if user selected taxa for search are used</param>
        /// <param name="hasSearchTaxonTypes">Indication if user selected taxon types for search are used</param>
        /// <param name="restrictSearchToSwedishSpecies">Indication if search should be limited to swedish species</param>
        /// <param name="restrictReturnToScope">Scope for taxa to return</param>
        /// <param name="restrictReturnToSwedishSpecies">Indication if return should be limited to swedish species</param>
        /// <param name="hasReturnTaxonTypes">Indication if user selected taxon types for return are used</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetTaxaBySearchCriteria(WebServiceContext context,
                                                         String taxonNameSearchString,
                                                         String taxonInformationType,
                                                         Boolean hasSearchTaxa,
                                                         Boolean hasSearchTaxonTypes,
                                                         Boolean restrictSearchToSwedishSpecies,
                                                         String restrictReturnToScope,
                                                         Boolean restrictReturnToSwedishSpecies,
                                                         Boolean hasReturnTaxonTypes)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetTaxaBySearchCriteria");
            commandBuilder.AddParameter(TaxonData.REQUEST_ID, context.RequestId);
            if (taxonNameSearchString.IsNotEmpty())
            {
                commandBuilder.AddParameter(TaxonData.TAXON_NAME_SEARCH_STRING, taxonNameSearchString);
            }
            commandBuilder.AddParameter(TaxonData.TAXON_INFORMATION_TYPE, taxonInformationType);
            commandBuilder.AddParameter(TaxonData.HAS_SEARCH_TAXA, hasSearchTaxa);
            commandBuilder.AddParameter(TaxonData.HAS_SEARCH_TAXON_TYPES, hasSearchTaxonTypes);
            commandBuilder.AddParameter(TaxonData.RESTRICT_SEARCH_TO_SWEDISH_SPECIES, restrictSearchToSwedishSpecies);
            commandBuilder.AddParameter(TaxonData.RESTRICT_RETURN_TO_SCOPE, restrictReturnToScope);
            commandBuilder.AddParameter(TaxonData.RESTRICT_RETURN_TO_SWEDISH_SPECIES, restrictReturnToSwedishSpecies);
            commandBuilder.AddParameter(TaxonData.HAS_RETURN_TAXON_TYPES, hasReturnTaxonTypes);
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get all taxa for species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="securityMinPower">Security min power.</param>
        /// <param name="securityMaxPower">Security max power.</param>
        /// <param name="hasUserRoleTaxonId">Indicates if user role taxon id is used.</param>
        /// <param name="userRoleTaxonId">User role taxon id.</param>
        /// <param name="hasUserRoleRegionId">Indicates if user role region id is used.</param>
        /// <param name="userRoleRegionId">User role region id.</param>
        /// <param name="distorsion">Distorsion of observation coordinates.</param>
        /// <param name="hideAttributes">Hide observation attributes.</param>
        /// <param name="hasUserSelectedTaxa">Indicates if user selected taxa has been added to the database.</param>
        /// <param name="useOfObservationDate">Use of observation date.</param>
        /// <param name="hasObservationDate">Indicates if observation date is used.</param>
        /// <param name="observationStartDate">Observation start date.</param>
        /// <param name="observationEndDate">Observation end date.</param>
        /// <param name="useOfRegistrationDate">Use of registration date.</param>
        /// <param name="hasRegistrationDate">Indicates if registration date is used.</param>
        /// <param name="registrationStartDate">Registration start date.</param>
        /// <param name="registrationEndDate">Registration end date.</param>
        /// <param name="isRectangleSpecified">Indicates if a rectangle has been specified.</param>
        /// <param name="northCoordinate">North coordinate.</param>
        /// <param name="southCoordinate">South coordinate.</param>
        /// <param name="eastCoordinate">East coordinate.</param>
        /// <param name="westCoordinate">West coordinate.</param>
        /// <param name="isAccuracySpecified">Indicates if accuracy is specified.</param>
        /// <param name="accuracy">Accuracy.</param>
        /// <param name="locationSearchString">Location search string.</param>
        /// <param name="includePositiveObservations">Include positive observations.</param>
        /// <param name="includeNeverFoundObservations">Include never found observations.</param>
        /// <param name="includeNotRediscoveredObservations">Include not rediscovered observations.</param>
        /// <param name="hasDatabaseId">Indicates if parameter id for database is used.</param>
        /// <param name="databaseId">Id for database.</param>
        /// <param name="hasProvinceId">Indicates if parameter id for province is used.</param>
        /// <param name="provinceId">Id for province.</param>
        /// <param name="hasProvincePartId">Indicates if parameter id for province part is used.</param>
        /// <param name="provincePartId">Id for province part.</param>
        /// <param name="hasCountyId">Indicates if parameter id for county is used.</param>
        /// <param name="countyId">Id for county.</param>
        /// <param name="hasCountyPartId">Indicates if parameter id for county part is used.</param>
        /// <param name="countyPartId">Id for county part.</param>
        /// <param name="isBirdNestActivityLevelSpecified">Indicates if parameter bird nest activity level is used.</param>
        /// <param name="birdNestActivityLevel">Bird nest activity level.</param>
        /// <param name="observerSearchString">Observer search string.</param>
        /// <param name="includeUncertainTaxonDetermination">Include observations where observer is uncertain about taxon determination.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetTaxaBySpeciesObservations(WebServiceContext context,
                                                              Int32 securityMinPower,
                                                              Int32 securityMaxPower,
                                                              Boolean hasUserRoleTaxonId,
                                                              Int32 userRoleTaxonId,
                                                              Boolean hasUserRoleRegionId,
                                                              Int32 userRoleRegionId,
                                                              Int32 distorsion,
                                                              Boolean hideAttributes,
                                                              Boolean hasUserSelectedTaxa,
                                                              Int32 useOfObservationDate,
                                                              Boolean hasObservationDate,
                                                              DateTime observationStartDate,
                                                              DateTime observationEndDate,
                                                              Int32 useOfRegistrationDate,
                                                              Boolean hasRegistrationDate,
                                                              DateTime registrationStartDate,
                                                              DateTime registrationEndDate,
                                                              Boolean isRectangleSpecified,
                                                              Int32 northCoordinate,
                                                              Int32 southCoordinate,
                                                              Int32 eastCoordinate,
                                                              Int32 westCoordinate,
                                                              Boolean isAccuracySpecified,
                                                              Int32 accuracy,
                                                              String locationSearchString,
                                                              Boolean includePositiveObservations,
                                                              Boolean includeNeverFoundObservations,
                                                              Boolean includeNotRediscoveredObservations,
                                                              Boolean hasDatabaseId,
                                                              Int32 databaseId,
                                                              Boolean hasProvinceId,
                                                              Int32 provinceId,
                                                              Boolean hasProvincePartId,
                                                              Int32 provincePartId,
                                                              Boolean hasCountyId,
                                                              Int32 countyId,
                                                              Boolean hasCountyPartId,
                                                              Int32 countyPartId,
                                                              Boolean isBirdNestActivityLevelSpecified,
                                                              Int32 birdNestActivityLevel,
                                                              String observerSearchString,
                                                              Boolean includeUncertainTaxonDetermination)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetSpeciesObservationTaxa");
            commandBuilder.AddParameter("roleMinPower", securityMinPower);
            commandBuilder.AddParameter("roleMaxPower", securityMaxPower);
            if (hasUserRoleTaxonId)
            {
                commandBuilder.AddParameter("roleTaxaId", userRoleTaxonId);
            }
            if (hasUserRoleRegionId)
            {
                commandBuilder.AddParameter("roleRegionId", userRoleRegionId);
            }
            commandBuilder.AddParameter("roleDistorsion", distorsion);
            commandBuilder.AddParameter("roleHideAttributes", hideAttributes);
            commandBuilder.AddParameter("SearchId", context.RequestId);
            commandBuilder.AddParameter("UseOfObservationDate", useOfObservationDate);
            if (hasObservationDate)
            {
                commandBuilder.AddParameter("ObservationStartDate", observationStartDate.ToShortDateString());
                commandBuilder.AddParameter("ObservationEndDate", observationEndDate.ToShortDateString());
            }
            commandBuilder.AddParameter("UseOfRegistrationDate", useOfRegistrationDate);
            if (hasRegistrationDate)
            {
                commandBuilder.AddParameter("RegistrationStartDate", registrationStartDate.ToShortDateString());
                commandBuilder.AddParameter("RegistrationEndDate", registrationEndDate.ToShortDateString());
            }
            if (isRectangleSpecified)
            {
                commandBuilder.AddParameter("MaxNorthCoordinate", northCoordinate);
                commandBuilder.AddParameter("MinNorthCoordinate", southCoordinate);
                commandBuilder.AddParameter("MaxEastCoordinate", eastCoordinate);
                commandBuilder.AddParameter("MinEastCoordinate", westCoordinate);
            }
            if (isAccuracySpecified)
            {
                commandBuilder.AddParameter("Accuracy", accuracy);
            }
            if (locationSearchString.IsNotEmpty())
            {
                commandBuilder.AddParameter("LocationName", locationSearchString);
            }
            commandBuilder.AddParameter("IncludePositiveObservations", includePositiveObservations);
            commandBuilder.AddParameter("IncludeNeverFoundObservations", includeNeverFoundObservations);
            commandBuilder.AddParameter("IncludeNotRediscoveredObservations", includeNotRediscoveredObservations);
            if (hasDatabaseId)
            {
                commandBuilder.AddParameter("DatabaseId", databaseId);
            }
            if (hasProvinceId)
            {
                commandBuilder.AddParameter("ProvinceId", provinceId);
            }
            if (hasProvincePartId)
            {
                commandBuilder.AddParameter("ProvincePartId", provincePartId);
            }
            if (hasCountyId)
            {
                commandBuilder.AddParameter("CountyId", countyId);
            }
            if (hasCountyPartId)
            {
                commandBuilder.AddParameter("CountyPartId", countyPartId);
            }
            if (isBirdNestActivityLevelSpecified)
            {
                commandBuilder.AddParameter("BirdNestActivityLevel", birdNestActivityLevel);
            }
            commandBuilder.AddParameter("HasUserSelectedTaxa", hasUserSelectedTaxa);
            if (observerSearchString.IsNotEmpty())
            {
                commandBuilder.AddParameter("ObserverSearchString", observerSearchString);
            }
            commandBuilder.AddParameter("IncludeUncertainTaxonDetermination", includeUncertainTaxonDetermination);

            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get number of unique taxa for species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="securityMinPower">Security min power.</param>
        /// <param name="securityMaxPower">Security max power.</param>
        /// <param name="hasUserRoleTaxonId">Indicates if user role taxon id is used.</param>
        /// <param name="userRoleTaxonId">User role taxon id.</param>
        /// <param name="hasUserRoleRegionId">Indicates if user role region id is used.</param>
        /// <param name="userRoleRegionId">User role region id.</param>
        /// <param name="distorsion">Distorsion of observation coordinates.</param>
        /// <param name="hideAttributes">Hide observation attributes.</param>
        /// <param name="hasUserSelectedTaxa">Indicates if user selected taxa has been added to the database.</param>
        /// <param name="useOfObservationDate">Use of observation date.</param>
        /// <param name="hasObservationDate">Indicates if observation date is used.</param>
        /// <param name="observationStartDate">Observation start date.</param>
        /// <param name="observationEndDate">Observation end date.</param>
        /// <param name="useOfRegistrationDate">Use of registration date.</param>
        /// <param name="hasRegistrationDate">Indicates if registration date is used.</param>
        /// <param name="registrationStartDate">Registration start date.</param>
        /// <param name="registrationEndDate">Registration end date.</param>
        /// <param name="isRectangleSpecified">Indicates if a rectangle has been specified.</param>
        /// <param name="northCoordinate">North coordinate.</param>
        /// <param name="southCoordinate">South coordinate.</param>
        /// <param name="eastCoordinate">East coordinate.</param>
        /// <param name="westCoordinate">West coordinate.</param>
        /// <param name="isAccuracySpecified">Indicates if accuracy is specified.</param>
        /// <param name="accuracy">Accuracy.</param>
        /// <param name="locationSearchString">Location search string.</param>
        /// <param name="includePositiveObservations">Include positive observations.</param>
        /// <param name="includeNeverFoundObservations">Include never found observations.</param>
        /// <param name="includeNotRediscoveredObservations">Include not rediscovered observations.</param>
        /// <param name="hasDatabaseId">Indicates if parameter id for database is used.</param>
        /// <param name="databaseId">Id for database.</param>
        /// <param name="hasProvinceId">Indicates if parameter id for province is used.</param>
        /// <param name="provinceId">Id for province.</param>
        /// <param name="hasProvincePartId">Indicates if parameter id for province part is used.</param>
        /// <param name="provincePartId">Id for province part.</param>
        /// <param name="hasCountyId">Indicates if parameter id for county is used.</param>
        /// <param name="countyId">Id for county.</param>
        /// <param name="hasCountyPartId">Indicates if parameter id for county part is used.</param>
        /// <param name="countyPartId">Id for county part.</param>
        /// <param name="isBirdNestActivityLevelSpecified">Indicates if parameter bird nest activity level is used.</param>
        /// <param name="birdNestActivityLevel">Bird nest activity level.</param>
        /// <param name="observerSearchString">Observer search string.</param>
        /// <param name="includeUncertainTaxonDetermination">Include observations where observer is uncertain about taxon determination.</param>
        /// <returns>Number of unique taxa for species observations that matches the search criteria.</returns>
        public static Int32 GetTaxaCountBySpeciesObservations(WebServiceContext context,
                                                              Int32 securityMinPower,
                                                              Int32 securityMaxPower,
                                                              Boolean hasUserRoleTaxonId,
                                                              Int32 userRoleTaxonId,
                                                              Boolean hasUserRoleRegionId,
                                                              Int32 userRoleRegionId,
                                                              Int32 distorsion,
                                                              Boolean hideAttributes,
                                                              Boolean hasUserSelectedTaxa,
                                                              Int32 useOfObservationDate,
                                                              Boolean hasObservationDate,
                                                              DateTime observationStartDate,
                                                              DateTime observationEndDate,
                                                              Int32 useOfRegistrationDate,
                                                              Boolean hasRegistrationDate,
                                                              DateTime registrationStartDate,
                                                              DateTime registrationEndDate,
                                                              Boolean isRectangleSpecified,
                                                              Int32 northCoordinate,
                                                              Int32 southCoordinate,
                                                              Int32 eastCoordinate,
                                                              Int32 westCoordinate,
                                                              Boolean isAccuracySpecified,
                                                              Int32 accuracy,
                                                              String locationSearchString,
                                                              Boolean includePositiveObservations,
                                                              Boolean includeNeverFoundObservations,
                                                              Boolean includeNotRediscoveredObservations,
                                                              Boolean hasDatabaseId,
                                                              Int32 databaseId,
                                                              Boolean hasProvinceId,
                                                              Int32 provinceId,
                                                              Boolean hasProvincePartId,
                                                              Int32 provincePartId,
                                                              Boolean hasCountyId,
                                                              Int32 countyId,
                                                              Boolean hasCountyPartId,
                                                              Int32 countyPartId,
                                                              Boolean isBirdNestActivityLevelSpecified,
                                                              Int32 birdNestActivityLevel,
                                                              String observerSearchString,
                                                              Boolean includeUncertainTaxonDetermination)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetSpeciesObservationTaxaCount");
            commandBuilder.AddParameter("roleMinPower", securityMinPower);
            commandBuilder.AddParameter("roleMaxPower", securityMaxPower);
            if (hasUserRoleTaxonId)
            {
                commandBuilder.AddParameter("roleTaxaId", userRoleTaxonId);
            }
            if (hasUserRoleRegionId)
            {
                commandBuilder.AddParameter("roleRegionId", userRoleRegionId);
            }
            commandBuilder.AddParameter("roleDistorsion", distorsion);
            commandBuilder.AddParameter("roleHideAttributes", hideAttributes);
            commandBuilder.AddParameter("SearchId", context.RequestId);
            commandBuilder.AddParameter("UseOfObservationDate", useOfObservationDate);
            if (hasObservationDate)
            {
                commandBuilder.AddParameter("ObservationStartDate", observationStartDate.ToShortDateString());
                commandBuilder.AddParameter("ObservationEndDate", observationEndDate.ToShortDateString());
            }
            commandBuilder.AddParameter("UseOfRegistrationDate", useOfRegistrationDate);
            if (hasRegistrationDate)
            {
                commandBuilder.AddParameter("RegistrationStartDate", registrationStartDate.ToShortDateString());
                commandBuilder.AddParameter("RegistrationEndDate", registrationEndDate.ToShortDateString());
            }
            if (isRectangleSpecified)
            {
                commandBuilder.AddParameter("MaxNorthCoordinate", northCoordinate);
                commandBuilder.AddParameter("MinNorthCoordinate", southCoordinate);
                commandBuilder.AddParameter("MaxEastCoordinate", eastCoordinate);
                commandBuilder.AddParameter("MinEastCoordinate", westCoordinate);
            }
            if (isAccuracySpecified)
            {
                commandBuilder.AddParameter("Accuracy", accuracy);
            }
            if (locationSearchString.IsNotEmpty())
            {
                commandBuilder.AddParameter("LocationName", locationSearchString);
            }
            commandBuilder.AddParameter("IncludePositiveObservations", includePositiveObservations);
            commandBuilder.AddParameter("IncludeNeverFoundObservations", includeNeverFoundObservations);
            commandBuilder.AddParameter("IncludeNotRediscoveredObservations", includeNotRediscoveredObservations);
            if (hasDatabaseId)
            {
                commandBuilder.AddParameter("DatabaseId", databaseId);
            }
            if (hasProvinceId)
            {
                commandBuilder.AddParameter("ProvinceId", provinceId);
            }
            if (hasProvincePartId)
            {
                commandBuilder.AddParameter("ProvincePartId", provincePartId);
            }
            if (hasCountyId)
            {
                commandBuilder.AddParameter("CountyId", countyId);
            }
            if (hasCountyPartId)
            {
                commandBuilder.AddParameter("CountyPartId", countyPartId);
            }
            if (isBirdNestActivityLevelSpecified)
            {
                commandBuilder.AddParameter("BirdNestActivityLevel", birdNestActivityLevel);
            }
            if (observerSearchString.IsNotEmpty())
            {
                commandBuilder.AddParameter("ObserverSearchString", observerSearchString);
            }
            commandBuilder.AddParameter("HasUserSelectedTaxa", hasUserSelectedTaxa);
            commandBuilder.AddParameter("IncludeUncertainTaxonDetermination", includeUncertainTaxonDetermination);

            return context.GetDatabase(DatabaseId.SpeciesFact).ExecuteScalar(commandBuilder);
        }

        /// <summary>
        /// Get DataReader information about a taxon.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">Taxon to get information about.</param>
        /// <param name="taxonInformationType">Type of taxon information to get.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetTaxon(WebServiceContext context,
                                          Int32 taxonId,
                                          String taxonInformationType)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetTaxon");
            commandBuilder.AddParameter(TaxonData.ID, taxonId);
            commandBuilder.AddParameter(TaxonData.TAXON_INFORMATION_TYPE, taxonInformationType);
            return context.GetDatabase(DatabaseId.SpeciesFact).GetRow(commandBuilder);
        }

        /// <summary>
        /// Get DataReader information about a taxon county occurrence.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">Taxon to get information about.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetTaxonCountyOccurrence(WebServiceContext context,
                                                          Int32 taxonId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetTaxonCountyOccurrence");
            commandBuilder.AddParameter(TaxonCountyOccurrenceData.TAXON_ID, taxonId);
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about taxon names
        /// for specified taxon.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">Id of taxon.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetTaxonNames(WebServiceContext context,
                                               Int32 taxonId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetTaxonNames");
            commandBuilder.AddParameter(TaxonNameData.TAXON_ID, taxonId);
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about taxon names
        /// that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="nameSearchString">Taxon name to match.</param>
        /// <param name="nameSearchMethod">Search method to use.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetTaxonNamesBySearchCriteria(WebServiceContext context,
                                                               String nameSearchString,
                                                               String nameSearchMethod)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetTaxonNamesBySearchCriteria");
            commandBuilder.AddParameter(TaxonNameData.NAME_SEARCH_STRING, nameSearchString);
            commandBuilder.AddParameter(TaxonNameData.NAME_SEARCH_METHOD, nameSearchMethod);
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about taxon trees that matches the search criteria.
        /// This method returnes two results sets.
        /// 1:  Information about taxa in the tree.
        /// 2:  Information about relations between taxa.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonInformationType">Type of taxon information to get for each node in the tree.</param>
        /// <param name="hasSearchTaxa">Indication if user selected taxa are used</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetTaxonTreesBySearchCriteria(WebServiceContext context,
                                                               String taxonInformationType,
                                                               Boolean hasSearchTaxa)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetTaxonTreesBySearchCriteria");
            commandBuilder.AddParameter(TaxonTreeData.REQUEST_ID, context.RequestId);
            commandBuilder.AddParameter(TaxonTreeData.TAXON_INFORMATION_TYPE, taxonInformationType);
            commandBuilder.AddParameter(TaxonTreeData.HAS_SEARCH_TAXA, hasSearchTaxa);
            return context.GetDatabase(DatabaseId.SpeciesFact).GetReader(commandBuilder, CommandBehavior.Default);
        }

        /// <summary>
        /// Test if the database has a pending transaction.
        /// </summary>
        /// <returns>True if there is a pending transaction.</returns>
        public Boolean HasPendingTransaction()
        {
            return (_transaction.IsNotNull());
        }

        /// <summary>
        /// Check if the database is up and running.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="databaseId">Select which database to check.</param>
        /// <returns>
        /// True = OK.
        /// False = Some kind of problem.
        /// </returns>       
        public static Boolean Ping(WebServiceContext context,
                                   DatabaseId databaseId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("Ping");

            return (context.GetDatabase(databaseId).ExecuteScalar(commandBuilder) == 1);
        }

        /// <summary>
        /// Rollback transaction.
        /// </summary>
        /// <exception cref="Exception">Thrown if no transaction is active.</exception>
        public void RollbackTransaction()
        {
            if (_transaction.IsNull())
            {
                throw new ApplicationException("Unable to rollback inactive transaction.");
            }
            else
            {
                try
                {
                    _transaction.Rollback();
                }
                catch
                {
                }
                _transaction = null;
            }
        }

        /// <summary>
        /// Add an entry to the web service log
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="text">Log text.</param>
        /// <param name="type">Type of log entry.</param>
        /// <param name="information">Extended information about the log entry.</param>
        public static void UpdateLog(WebServiceContext context,
                                     String text,
                                     String type,
                                     String information)
        {
            DataServer database;
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("UpdateWebServiceLog");
            if (text.IsNotEmpty())
            {
                commandBuilder.AddParameter(WebServiceLogData.TEXT, text);
            }
            commandBuilder.AddParameter(WebServiceLogData.TYPE, type);
            if (context.IsNotNull())
            {
                commandBuilder.AddParameter(WebServiceLogData.WEB_SERVICE_USER, context.ClientToken.UserName);
                commandBuilder.AddParameter(WebServiceLogData.TCP_IP, context.ClientToken.ClientIPAddress);
            }
            if (information.IsNotEmpty())
            {
                commandBuilder.AddParameter(WebServiceLogData.INFORMATION, information);
            }
            if (context.IsNotNull())
            {
                commandBuilder.AddParameter(WebServiceLogData.APPLICATION_IDENTIFIER, context.ClientToken.ApplicationIdentifier);
            }

            database = context.GetDatabase(DatabaseId.SpeciesFact);
            if (database.HasPendingTransaction())
            {
                // Create new database connection that has no transaction.
                // The log entry may be removed (rollback of transaction) 
                // if the database connection has an active transaction.
                using (DataServer tempDatabase = new DataServer(DatabaseId.SpeciesFact))
                {
                    tempDatabase.ExecuteCommand(commandBuilder);
                }
            }
            else
            {
                database.ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Update existing reference.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="id">Id of reference to update.</param>
        /// <param name="name">name of reference.</param>
        /// <param name="year">Year the reference was published.</param>
        /// <param name="text">Text about the reference.</param>
        /// <param name="person">Person updating the reference.</param>  
        public static void UpdateReference(WebServiceContext context,
                                           Int32 id,
                                           String name,
                                           Int32 year,
                                           String text,
                                           String person)
        {
            DataServer transactionDatabase;
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("UpdateReference");
            commandBuilder.AddParameter(ReferenceData.ID,     id);
            commandBuilder.AddParameter(ReferenceData.NAME,   name);
            commandBuilder.AddParameter(ReferenceData.YEAR,   year);
            commandBuilder.AddParameter(ReferenceData.TEXT,   text);
            commandBuilder.AddParameter(ReferenceData.PERSON, person);
            transactionDatabase = context.GetTransactionDatabase();
            if (transactionDatabase.IsNotNull())
            {
                lock (transactionDatabase)
                {
                    if (transactionDatabase.HasPendingTransaction())
                    {
                        transactionDatabase.ExecuteCommand(commandBuilder);
                    }
                    // ELSE Transaction has been aborted. No need to update.
                }
            }
            // ELSE Transaction has been aborted. No need to update.
        }

        /// <summary>
        /// Update existing species fact.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesFactId">Id of species fact to update.</param>
        /// <param name="referenceId">Id of reference.</param>
        /// <param name="updateDate">Date of this update.</param>
        /// <param name="updateUserFullName">Date of this update.</param>
        /// <param name="hasFieldValue1">Indication if field 1 has a value.</param>
        /// <param name="fieldValue1">Value of field 1.</param>
        /// <param name="hasFieldValue2">Indication if field 2 has a value.</param>
        /// <param name="fieldValue2">Value of field 2.</param>
        /// <param name="hasFieldValue3">Indication if field 3 has a value.</param>
        /// <param name="fieldValue3">Value of field 3.</param>
        /// <param name="fieldValue4">Value of field 4.</param>
        /// <param name="fieldValue5">Value of field 5.</param>
        /// <param name="qualityId">Id for quality.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static void UpdateSpeciesFact(WebServiceContext context,
                                             Int32 speciesFactId,
                                             Int32 referenceId,
                                             DateTime updateDate,
                                             String updateUserFullName,
                                             Boolean hasFieldValue1,
                                             Double fieldValue1,
                                             Boolean hasFieldValue2,
                                             Double fieldValue2,
                                             Boolean hasFieldValue3,
                                             Double fieldValue3,
                                             String fieldValue4,
                                             String fieldValue5,
                                             Int32 qualityId)
        {
            DataServer transactionDatabase;
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("UpdateSpeciesFact");
            commandBuilder.AddParameter(SpeciesFactData.ID, speciesFactId);
            commandBuilder.AddParameter(SpeciesFactData.REFERENCE_ID, referenceId);
            commandBuilder.AddParameter(SpeciesFactData.UPDATE_DATE, updateDate);
            commandBuilder.AddParameter(SpeciesFactData.UPDATE_USER_FULL_NAME, updateUserFullName);
            if (hasFieldValue1)
            {
                commandBuilder.AddParameter(SpeciesFactData.FIELD_VALUE_1, fieldValue1);
            }
            if (hasFieldValue2)
            {
                commandBuilder.AddParameter(SpeciesFactData.FIELD_VALUE_2, fieldValue2);
            }
            if (hasFieldValue3)
            {
                commandBuilder.AddParameter(SpeciesFactData.FIELD_VALUE_3, fieldValue3);
            }
            if (fieldValue4.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesFactData.FIELD_VALUE_4, fieldValue4);
            }
            if (fieldValue5.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesFactData.FIELD_VALUE_5, fieldValue5);
            }
            commandBuilder.AddParameter(SpeciesFactData.QUALITY_ID, qualityId);
            transactionDatabase = context.GetTransactionDatabase();
            if (transactionDatabase.IsNotNull())
            {
                lock (transactionDatabase)
                {
                    if (transactionDatabase.HasPendingTransaction())
                    {
                        transactionDatabase.ExecuteCommand(commandBuilder);
                    }
                    // ELSE Transaction has been aborted. No need to update.
                }
            }
            // ELSE Transaction has been aborted. No need to update.
        }

        /// <summary>
        /// Update taxon related tables.
        /// This method is used during taxon information update.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void UpdateTaxonTables(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("UpdateTaxonTables");
            context.GetDatabase(DatabaseId.SpeciesFact).ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Add all underlying taxa to user selected taxa.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void UpdateUserSelecedTaxa(WebServiceContext context)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("UpdateUserSelecedTaxa");
            commandBuilder.AddParameter(UserSelectedTaxaData.REQUEST_ID, context.RequestId);
            context.GetDatabase(DatabaseId.SpeciesFact).ExecuteCommand(commandBuilder);
        }
    }
}

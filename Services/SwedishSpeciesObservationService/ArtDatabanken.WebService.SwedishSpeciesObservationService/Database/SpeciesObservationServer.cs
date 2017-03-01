using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Database;
using ArtDatabanken.WebService.SpeciesObservation.Database;
using Microsoft.SqlServer.Types;
using SqlCommandBuilder = ArtDatabanken.Database.SqlCommandBuilder;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Database
{
    /// <summary>
    /// Database interface for the swedish species observation database.
    /// </summary>
    public class SpeciesObservationServer : SpeciesObservationServerBase
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

            switch (Environment.MachineName)
            {
                case "ARTFAKTA-DEV": // Team Species Fact test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANHHMFBKBKHMFAMNGOBAFNANGPNDENDFFAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADCOGKCODFGJDJACEAGLJGBDKIIIKEGMMLIAAAAAALHOLGACENILGJJNOGIKAEGNIJDAEBBOHGICHLIEEGHKFBBDCBCCCLGHAPFIKOPMIBDDCLKECFEAEKDANBNBGDDCJDGJFADAKLGIENJMHCHINPNIGAHKHCHMMOJECEEMFCIHIDMACMODAKMLOMAACOBIGFMMLGJNPPLBNFEHMIKBLCKBCFMLPHIGELMPPIDONJAPONGHELFPOELOKACCDMLJMPIPGEEOCDNNHFLFDKOJJOPNFLKNGNKOKELMFBGMICKOLLNELLBAEJOIMPNBIGIBEEOIOCFOAFCAKLNAPCKANEEGGOCBLIMDJFEAIPBDMBDJPJLPAAJKOCOMNCOJBIDMIPDGHDBAMPLIGFDJJPBJECMIBBEAAAAAAGMLAJHHBPLJFKNJACJOMOJDADJHCJPCLIJKBJDLF";
                    break;

                case "ARTSERVICE2-1": // New production web service server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADLGBCBFEDLOBCGCHLMEODBJGHLPLIKKIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAAILDLLJKICCBCIJPDKPHKDPCMLKEGGKHMIAAAAAADKCPHIKCKBLPDBEAIJJPLCFFEEGHEFGHGCMEDLJPAJKDFOOGDLFADDHEAKLPKLMEIEBFBOFKLHGDKIJJLDBFLLGMBBEGDGFPBIHCLJMDIFDPGIMCDPDJGMEMNIDJMKOCLLLJBHMFINCGCJBGDHCFMFABHCJALCGFJFBBHPLPKIGDDGBPHKJFIDAAHGMHKDLFOOODPEHFPNKDJJHAONIMKCOHHFCDFAEJGACDCOOAFIGGLJNKADDKMJJLJBLJIJBHPCFDEACDELNAOOBHLBEHEJCFMAHMGDGAOHODKBMKDDJGALJPPHOJNCCAMGNKNBGJDEHLOCMFLEGFLDKPKMHBHJCOPOIBBHCNOJEKHAAGLNLELCGPHGGJKMJMGONCPGLAABFLLHNCAFKAANAHBEAAAAAAOMAFDFMIPNFKNEFFJIMBNNOGACPKKPGALGJFNIDG";
                    break;

                case "ARTSERVICE2-2": // New production web service server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANKGPCPHEAPGCJEOFAIDGAAMPODMMFLLPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHKNGEDFHEMEHGJHLPBBEGGNHHPNKGBFDMIAAAAAAAFDPLOINKJOILFLKNLFOCEDAAJDODMDIAELKJOEJLNKKFIGLACEIAHLAPAMMJAKBBDKONCKGNIGNJFBNOMMGBEODFGLDNODPFFCODNNBCLIKADHGEMLLDEHJBEMAALAGMAHPPDMPEAMPONDPGEHIHEHIEDGFCLIDDFPJCLCMKCKINOIGPGJJJJCPCJKIDFDIHHPLIOLCEFOBHOBEKJAHHCCONDMNPIJPGOKLBKBIOPGJOKLJMLHOILKAJKCCPILPHDFMNDENMMPEPAMLEEAGOPJKJJGHLCHAGNPECNNCHADDPBNJHHLBLMHMAACEIFJMCBEIFBAPGBEPPLFHKBABHOLCBPFHLOEICELNALBNLLCKAEOBDDDJNLOMPEEJBPEGCCDHFGKDBFAKAGHJBEAAAAAAHACONEBCJKAGEBJPOIJJHMCMLHGAGGKAMCMIJBCA";
                    break;

                case "MONESES-DEV": // Test Web Server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEAFPECBBEFEHIKEGLFFDMHECEONJDGANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADLNDOLNFLMBGGCOJIDPJAFCPKGJAFBLNAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABAGJLGNPJKPMBHLJLOOPIHIODMJDPELPLIAAAAAALCGCHGGEHGBOEEKHFABOCDKEENJPLPGKLNMMHHBDFAGEJMFKPPILPNGBCDAKCOCIJEIKDODLOBMDODFOIIOKAMFDKMANBGKCCADIMLGIFLBEAGKMNOJEDJDOMPLDNIMJJJGEMHPAADGBNGOGOHMEJHALACBECABBIELDBPNEEKHIKILGCJNPDICBAPBFBBOCNKALDKHNCPNCNAFAGJDNBENJGFIHLGGKPFMLGIBKIJBKMMGGLIMGMOAMPODGPKJFCJEDBPIPJAFEGJMCGKLOPGKEGJOMHPNBEPJFLGIKKKKEKJOCLNEIOJFKNCJFANFEKJIDCIGPGOLDJCKEDCBOFCKIEFDAFAFACEMNLNCOOLGPLOJBBEAAAAAANOGGMHFHAAJCCEBPHHPHIAFKJDNGOJADIGLLHAMN";
                    break;

                case "MONESES-ST": // System test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIKGEBDAPMKNOAFEKIHFGCOJNGHIAKFIAAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJNNDLEFGGFHAAIILCNBDBPOFBHHKFOLGAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADOIMMFDIFKEBFAENCPKMIGHOOOJJCLADLIAAAAAAJHMGNIAFNEKLIPOJAHMKMBJCFCJDBAJHEDGPJFEIFDLFLMKNHIOCCBLOHKDHOEHOJIFPAJCPHOBABEEGCMANMPAJLNFFGPEDDHCIJFJEHLEDOMOHNHEMAOMBKCCFKGFCFKOOGJGOEJLKJOKHMCAJANLPHBKABOJEHLNJIJACBOICFGEDIBMAGDMIIAKFGHIMGLHNCGONHIEPAKIPPEFPGIBEEMNCKNFAPFBFDFECGPGNOKFBNGKFKOKDNEEACMMOOOKAOMNBBIMBIAPNBJPAODKIDPODLDEAKJAIEECCEEIFFCGBGLKLJDCGHDLLMFGDMCMJGOPOBPBMIMEGHGOHLBANACICGAMJFPPJILNGOGNIGJHIBEAAAAAAMMAFAFHPNCNNDLPPDADEMDMIBHNNDBHLABJBIPEJ";
                    break;

                case "SILURUS2-1": // Production Web Server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIDGFKMFBDIMAPEELJPFDBGPLIAHNCJMKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAAPOJDIGFNBGIAPFGFBFGFKCFFABFGPJDAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAEGAJPDABCBJOLOBOFDFKDKANBHCKJJHNMIAAAAAAPODPKENHBCCAGCPIACGNICIHOAKOPIGGFLJFFOFIHHENHHOAFHPFFHFCGLMAEICIFCIKGGMNFKGLMGJLNNGBBBEHAHHJOJDMKCCBLAJBIFKLHPKJNEEDFJIHJOCCLFHFLLHFBIPFONMNJODPHGMIBHHDAFMEHMEFEJJHNNPCENJEPLFFGFLJIAEKMOGMODJMMNFFCGCHOCJDMBAMIHJIGIKOBGEJJADFDBCLGPFHKKLPOKDODAEOMNNILNHPAMNNGHBECFBPNDJPKNDIAGKEGDFNAICNBECINBEKHBMHLMIBKEAAFDJMDENONAPAJFHDKAKNPNGOELPBDECFHEOGCNMBPAADOJGIBMOADOICHHKFGPAIIDOCJGIOPIJPKDAGFHPLMFPECLGJINMBBEAAAAAALMFGFKHMPHGLOFCHJFCFDEPJKKMJCPCPHIJOLJOO";
                    break;

                case "SILURUS2-2": // Production Web Server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAODPAFKAIHGMAPDEOIPNDNGAIEJHDINBGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAILIMGPFMBCAKLHAGNLJNACKEKKMFNJIDAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAIDMNGKIGDOEFEPPNKHENDGLOADOCMIOHMIAAAAAAABLLAOKBBCKGLLNNNOBNIFMJIGPJOLDFNBAJIFJGHFNOINJPPHBDOPKNGIKOHJIINJNDCMIOMABNGKFJDDFIMFBCNOJPGDBELGGBMPDEGMIPAEIKEACDGHNIEMIOCBIPMOAMGLGMMGEFEBMBHIANKMBLPANDOFHIPKLPADHGOJGKAFEJHAGAMLABJJJMNIFDGFAIOGHDDNJEIHMOGBAOLJKOAKJFHDGNNDOENKLFHLJDLPOEEDIIPKMLOJPGKEDAIIJNPLCHFLKEOLFCDLNIFGNIOBAPCINKCMLGBMDIHGAFLHIGCAGLOFPDCCFAAFDOEDGHMHMLDCEKCKPPCCAOABHMKGNCGNFMPMBKMOEJOHFCIOHPKKOGDPLONNPFDONPJOFCJHMPAKHMFPADBEAAAAAAKGAKLKAOIPEBOHEFGNDADEGIAMNHCKKGIJMPENLG";
                    break;

                case "SLU011837": // 
                    if (Configuration.InstallationType == InstallationType.Production)
                    {
                        connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPFOBOEOIJOPHGCEFJGCCBMFMHLOBJJLJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADGGPIPHOIFPODLDHHPMLKPNEHPPENEHIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAGAOGPKLODKMKILLFHDFJPMGMBCBCFDIJMIAAAAAAFGPOIJDNEMCKOCPFHDJGHOBAENELFOOEHPDJEKCFMAOLGCLPAINJHGBENNIMOJHACAAICECDOFGOJGHFFLCADBIBIIEPCNMODOJPKFKHIABFJFJCFMKMJOFOIPGKFJEOBMANJPCFDFMFPFGOHJFLOOBNGEBFNECNJLEDIEPNJOMNOPGEFPGBNJLGLIKFBNIKGJFFLDKEAIHBPBONILIKPPKPCCGOKJBHPKHPAAGOACAIKBCHAPLECJMNBKLCPFHCBEAMMCJCNDNONJCPBMJPNBEJJMIJGFJBLOKHNNENLKPLHDIKKBNDADCGLNMLNOCMEJDEBMBEAMFCOEGJLOCONLGDOIAGJCHKDELKFHBOKMDEDMHJMFIMAAAJGOEHKKCNOHIENAGPMLEMCMJOBEAAAAAAECJENKJNKMEIBIKNBHLEODNKNNIGKDFJCNNIILEK";
                    }
                    else
                    {
                        connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALCPBDBGCIAFPEPEILEKMLLIKEOOJMJMKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAAADCLFCMGKDOEDHDKGCKDIHKNGPKGOHLAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAACFGODLEMAFAHAJJPPOBOJPCAIBCHMHANLAAAAAAAMHKHFBDKEBDCOFGHLMOAHKLPNHADPKEHMEJCDMDGBMJKLCGKLCLGFMINCIJCELCFCANPKDKDBEMOPOEMKPDDJAEHIADNBBEDFMBBPMEEHOKHFGBIHJOPKKHCHIEGLOCCBDKFIJLCFPLJPNEHHIKBJIFOBCJLNNNAEPJCMBJMBCMCLIJIOIEPGNGNGFEOPMJEJFNFCPENJEDELOCMLOHDBBKPMIGPCBFAECBLCBIKJCANAEHLPJEJCPIFKMOOFIKAJKBMAHKADMLDEBGDIFBHNMIJCNNKFGMBGPLOMFHNCOCFJCKBNDGNDCONKGJLHPBLDLHFAMGHJGJCPGDNDDGJHFFMOGOBHFGDBEAAAAAANMDEFDBAMJGLJLCCCBMLGBCLHIAPOBHGCJANEBFN";
                    }

                    break;

                case "SLU002760": // 
                    connectionString = null; ////"ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABAKAMJGGBJHDJCEBIMHBCBNNBPAODGGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAANALEJOKMFPBLCNDLGHAMMDLPONPBBEIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAACKDHDFLEKLKOMJAHEKEOHBEFFPDKOKFMJIAAAAAAGDMFDCPPOKJPOKFGIOIGLEOKBHMMDPKABOJILGMNCHPCJINOAMKJAGAFJNBEEFIGKAMMDFPLHALGLCKCKAHEBEJLEACHGBBFFHKLNKOJFBIDKMGAPOJGJCHDNEFEANNEHCFGMAIGOPCMBAEEACICPGALONGCPPIHKPHOFKHGNPHHJPAEANGICPEMCLODMMMAPCKCIENKHBIGBNJOIONKDFHBFBENFOFNGLEBBABEHGAGPGONIAGEHEIEOACLOGAJJAABIOLDDMHNOPIPOJOMDLFFMJAPODECMELLMPJPIPABOGCKBEAAAAAAAFLENJOBBDHIPGOPGLDNAMAKGNEEHCIONEFEFIEO";
                    break;

                case "SLU003657": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAADDFLLGAELOMBMKEHKAFIJHEEFMCFCNKCAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAGNLEKMGMHENIBOOBJECGBHILEGHGOEOMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAGJFEJALOCLGOMFAONIKKNKALNBDNAPDLKIAAAAAADMLJNKKBJOCLFLDHGAOENPFCBCHJALPKOCFMKCGDPHBBGBDIJJJNJKOCHMFBCPKAGIEEBNAEPAINJKEOOCMFNLOHNJLABNKMEJMINKNLPGCKPEPDBNNIJHILNDMPHFEBJHLGFBNILKIOHBCJMDALLEENBJAKPOOFMPGHFIFPIKEKGCEPODPEMMMNDNKGFEIHIKCIKCANINBDHANJHGLAPKANDFFKIIAPEIACGNEMEAPCCPNHNFBEBCONLNAHLCHONBNIBPLEFCNIPFIKDDHIDKBMEICBKPGPPIIEBPINELIJEPOBFIJCGLNKKFKPIPCHDDCCMJAKIPFBCEOHBEAAAAAAIFDBJCIKCOIENLHEIPFPGOMEOEBDJLOBOKEBNNHD";
                    break;

                 case "MATLOU8470WW7": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAHMBBCHINPNNNEIEKKPAHDFHLGDNHNOKDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAABHFKKMNOLMIBLFDKBIJPKJNBKFJILGIGAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKOFFGGONLKOMJLMNBJAMHBNOOHPLICMLLIAAAAAAJJOBHOLNAMMLJIBFMLFCNNPOGALCONNBFEIHODBOCKIEBHGOKFNAALHFMHDJPGKALPNAEGJFMNAMHCKKGGLJMPCIKLEGGCCKPHOJCOJJOAFIFKKLKPBFEDMHLKIHCKCHCMELNAGCJJOOLHIBBJHKFIABPFFEOEOMFDDPCDLBOPHHOGPOFFFLFBFEFKFJHKEMNAFEMCINAONJOMPAHPPHDLCENFFNJLHNNKJGNLNHJGPMAKDOCFFMLLBALLPNADGHAHOHIGDPOABNAHECPADHLLLKJAPHAEFFCILGHCLHLELDPNOHGGNLMOCMPOBIEJIIJBNILNJMKMMAFODDOKILIHDAJCDGHLMFLENIDBKJGJMPNOFEBEAAAAAAEIPEBLLCHCBNPNJCMKCCOEJBGOBKEPDMLNBHMLEM";
                    break;

                 case "SLU003354": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAOOAEJFIJMOIAMMEBIDIAGCFCCMDBGGBBAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJONCBJPJJIFKDMMIIOJEKCGCPKLDMILNAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALBIHODOLAEKCOGDGNEMLLOKPICJKLBGPKIAAAAAAGMAMNKKCEMFHOLEPAHLBNGNNICFLDKAKHPJOGDDBMAEKAMHFHPAPPDBEGOOODGPJLDGLMEDMEEPLFMNNNKGKGJDJBHFGPCBPCFHCONOIEGCNJNLGCDPMONDHJOFJNCJBEGDFFGBAJJFOKMJOCILNFNOJENCOMIGDFGDIIPGKCDALLGAIBGHLJEKIDFLICBKFMMAIHIPKFADFFICPGIKFCMPIGEMEMPEPIFBKDNLCGDOGMHAHODDEBPFALAJBOONALEKAKOJLNJPOKMMGAHJPDFLEHPKGPPNDBLODEBCAOCKOBAIOECJBAMFOAADMDCMEPCGJNKJGGJJLCBJLBEAAAAAAHNKIEMDJKJMOOGGABJHDOAACMEJJOGEAHFMKOCBP";
                    break;

                 case "SLU002759": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEJCCAMNLPCOMGDEGLLEEMKLCCBJHDGEKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAABCCGDGFOAAJBFHEDLPCKLAABODLGDGNIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFKICIPNMEDHEOKNEPOFPODJGBMIAONJOKIAAAAAAPCOGOFGELMBHIPOGDHMJIFBJGEGNGOGJLIHBKGKMMMDIGDOJBDAFCPIAJKPBMLNCONLIJBJMABBOIMJEOEPGMMMMAPMDKIAOPJMKIJCFOOECDPIFLBLIIKCABIGMHEBFPGKDIHNNLIEDCAJMLJOBOOOHCMADLLKDALDFHPNFLPNECPFNAKKGINBPCMBAOBALJMIOPBDFBENHMHHINGLBOALEMKLOKICAEKOIDNNPNPKOLENNPPGLLCKNAALNMKNEPFBKIFMPFOKAOKAJIKGNPKEJFKJGBKCCJPPKHBBPIFGBCGDCHKBFFHNCKJMKDBPODFOHHLJDNAKKJDDMBEAAAAAALMJKICFDMPBCMOEIJACBMJJHBHFJMBBABLFHLLHC";
                    break;

                 case "ARTDATA-TFS": // TFS-Build 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABCNHOEMJPBDFAPEFLODEONCFHMLDLFFJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAOJHIBOPINDOMLHOEIONNAPEKAOEKGCGPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAIHKKHKGINJLNLGDGADFOOKLOFEHLFFNDKIAAAAAAJIMPPNEMNIOHCFKBJIPPNBGPOCEDKBLNAOMGGIKDPCJIPJBEMPNOOHCBMNHDGLJPGBLGIPGFGLGAEDNPLMKCBCGOJHBJCNGNKLPFCBBAHEHFFOALMFEMMPIKFOMBGBDNJEEJCMGMCPAPKIPHLIBODONLOGMHKKDPEDBJHLKNINJGGOLPMAHKJBDKGJNIMFDNHHHKAIOPCFADHDLJIAMMJODFFJKLKDGBJHPELBANEOHAJNHHFKCMGCLNDIPPPPEJPOHEHFLJFJKJGJCKGGDMPBCODCOPKGLEBFOMMPLGDMLAHBLCLMOKCHFJLOEEOPOPIDDEGGKNAGLJAFJLBEAAAAAADDDHPNFHBPDCDIILMJBONLFLJDHDKJEHMENDPAHG";
                    break;

                 case "TFSBUILD": // TFS-Build server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAJDEHMIDHELNCKKEHLHOKFNFFKPMHAKNDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAOBBPLPDJDKOCFOOKJBLDECNFKGHBEAKGAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALBDCLEOALEOCNEPLKMONJKMJKMEPBCPPKIAAAAAAILHNGCNNNKBHHFDBKEGBIFICJBHCENDHLEOLHIDCKKLCJHFJACAILGOIGFDOHBMCCJEEGKKCOGHDMGICJLIMCPGFALAPHBNKINHEJMKHKCCINOGEDEELKOPGHOIHMBDJMHAKDBGHIAKMIGLECNPCGNNHHKEAJECJFFBICEIAGACBKPHPKLOOJILGGMMHANMLJHLKOIENCBGBNMBJCPIOONHMLINEOBDMGCOECHDEDOECKKHMJDAMIMNKLDKLCCGLBBJNNEIBEMIPBEFGIOLGAPLPKIMADAFBOKGJLLFJNDLOGHBKJBMLPMDJOMHNDGDDBOCFOKKPCJJFIKPGBEAAAAAAIPGLBCFDOJKBBJBCBHAOMNJFIMCPGPHNCAKKMGPG";
                    break;

                 case "SEA0105WTRD2": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABCDHLPKLHGBGOOEJKJIFHDOAFNEEFMNKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAECPMLHLNEBMKIBCMDOAMKCNDGODOHEABAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAMIEMBOHBNGFKEHJANFDKCOMLHNHIBOCALIAAAAAANGMJDIJAKOJBHJIPHCFHGNABBNEEMGJBLJBCMDJPCCGJGNINJDCNJKIKIEGFNBECLIPDEOAHEMBHPCFPOEFICPBHPCKHCPIMHMIGDDHMLNHDGOBEGJEJJOHAEMOKKBCJDDHEEFAPBCPPHNHMEJKODLFKJKPOBFKPHPJDJJHBIBEIIONEMNKCKJCBGPBLMBEIMHOEEMONNLLDGNOEKGKCJNCLGIPAAGDNLODFHOFJEAMNALNJLKECKLCEDFIODBAJEOGAMONNNNAMOCECEBPFCNHLLMNJHOIICDPPLDFOIJPJALHDOINCHGEMAAPHGHGCMDJNBEFGFIMOEMIPLLLIENALFCPENMFEGMHOLJMACLGNEGNPBEAAAAAAAAKDMIPEIEGMBDNKIGOCDMFLCNJBDNKBAIAPBFFJ";
                    break;

                 case "MARKAC8560WW7":
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAMJAKAEAFNCBCNCEJIINGJGKEBOKONPNLAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANPLLDMACOPCOBHDLLLIFKNJLCJNBNJPEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJGNFDAAKGFNOEPONAHAMJDMKFCADOAFGLIAAAAAANNNPJBHLGIHMFIAMPLCDMCOJNCDJNJDIIMCBBGAKMKABEAKKMHHLOLDNIEDIKGNMEMILCBJCIAJHCAJCJELGAIDALGNNOBMBLAINILGMMCCEMKIOFJFLFOCEONFFIFAMDLCMCHKKBINLGBAOHKIBHCDAHEJEHMCLPDKJHDNOLHBLIELCJGHMFKDOPDFOOEKIOEPHDLPGMHJDMHEBPJDNPCBEAPCEBEJIMANHIHJOCJNIDDNPLGLEMFJGGMGIMAMLBEHPCJHKPDKPGPHBFJDGFIIBLCGBPPBLHFMAEILGMBKCJDPHDECGEACNAKFAADOEIPMHKKHHDGFLJOIFGMMEBDCPFELLKOAGEHBNENJKKKFJMCMEBEAAAAAANHNNJIPFHBGJCNKDBFEJNAEEGNAHBFEMBEOOPPAH";
                    break;

                 case "SLU011896": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAJMDHKKKEGEILFAEHKLLJKBJIAAMPBPCDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFMNDPDMMHJEIBDJBJKNPEKDOMFJMMCAMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAIMFPLJMPHFKDDECFCFNBDADNNBIBDODBLIAAAAAABDFKFHIGELEBKNAMICOGIHOBIDMJOHGIEHHOBLFFBEJGHFGEJKDGIJNOOJNJHFPFIMKOPEEMFNJCCHFKKEILIAEBCDLALLHOCNAOBIFLKLOMJOGKJACLDEDNNOKMLNPHDGHHMNBEIOLFGBHOFKNBMFBFHLJPGIKFPPCHGMGBKMAFFALBFHDALAEGGFMIELCICDKLFBAJMMJPHJMLPGHKNHMOBBGNPEBDDIFNNCAODADCKOILDOLAFFIDHGADGFCCKGOCKMDFKNHMODJNKBHKMFEOFIBAGOPLFJEPKAEPILPGGJPCKFDCPHJKFHEPDEEDBGKHNCDALPFAJCFCDAFIGLJOCJBMMPLIFHLGCALNBBDNLFIJBEAAAAAAENFHBLLGCKNPJBNDACOCOEHJEHKLLJCKNPDPKAEK";
                    break;

                case "SLW-DEV": // Team Two Blueberries test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAMHKHKAGJMHDAEDBFBILOBHMJEBLNOJGAAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAGJICAOGLCLHCAABIDIIDIAOIJAOHIGKNLIAAAAAAALKGMOOGHHEAKFKKFCMLGHIBHKDJCPGHHPGKIBOEGIMNLFCJJHBIKAAGACNCCELGNCFDMPJJPCHBKLMFPCHHDGHNNLALPEFCIMPPHIIOBDGMHLIELNMMJKMEGNNPFGNICBFOODMKOIGNMHIIJCLAPLIIMHJBOECJILOMPIOICDHDMMODHNNMMGLOKOMCFPAFGIMKMABCOAPDFGJBOHOGPLELJEGBMFMAOBDBNGIHJNBPEOCENMNFFDHHNJKHPAMDLPLOGGBINEKFOPHDNAEOCFINHFKNBBFPKEAKJAOAFEFLLGMIKFPGCAAGFPLHCCCBGFIKICDBCIEJODGDDKLGBEKHLFEDPLLMDDMIOAPLLJLNPNJJBEAAAAAADGNHIEOCJJCOEBBADAOHMIOCBMGDELJJEPEJGGEO";
                    break;
                case "SLU011161": // 
                    //connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAANLBMDHALKGCLEPEDJJCOHPMCCNMJKLBPAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHMEOIFOPMJJLKFMAPDCEPCGEHMAPBIBEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKDKKELICEOFFHGAOHKDFLKNONLBDLFGJLIAAAAAAHONPEAFJEOOMMMJAIGNOPOPGMLJNJAHCPCJDIMHDJBCBMIONEDFEOCFABGODJJKJPNBINAKPJFEPJAHDOHADJCDOGEALDOKIKLJOCFHPLOMPNDKKGAPHAFPAAIAOCLKJCFIKIBFFEMPFKFIAFOLDNFOHJDBPPBFPDAKAHPMLLDHMFGJPAMHMGDPFKIKDEIKDOHONKNONNJHEBPILIPLBDFFGHABMIEAFJOAPNBFNKDNFHPOABEMIEFDNEPLBBNCCAIDCNFGMJHDKLJECHLIAMILNNBMCELANOGIFOJCPPMDAOIAEELOMLMFCFPLDENFLMJDFMOFBJMFPPKJLOADEFINJGJFEGNLGBJOBGKKCLLHBNHAMBEAAAAAANKLCBADOCAJOKDIPBEDENAEEPEKIIJBFFKBOEKAI";
                    connectionString =
                        "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAANLBMDHALKGCLEPEDJJCOHPMCCNMJKLBPAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAMDHKHHBPNFJGCDCNKCLBKPAGHKLMPEEBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAOFBCNJMGGOBDHNPBIJEMGAIDALKIFCLALIAAAAAAJODILPJOBLDDDDABFDGNCDEEBJFGNPFLMDPNPCLDNGIHMONPPHKCNCLNAJMEOANIJPMPNCNBEAEIHCMKPHEJAFKEAGABIGDAMHKEMIAAKJMPEICBMJPNDAIABCNCJPIHINAJOIPKMCMIKHKDFOIGEJNBAADDOJDCCNELCIBMHHANNKBBBMDGGJCEMNEHEAKFGFKONMPFIOIAGEMBDIBDCPKCBOFOPCOAEIEFMNGHMGLELHMKOGCBGMAHJIJOAKNLNHKMCFEGBDMACIJNCOBIABEMIHMICLMBDJKKKPGAHNLCPIJGGGLBOENMBFOMEHBBIIGABFJLHLEOCDAGLHLNIFECGANPNNMFBGAFDKFKMAPAJHAKBEAAAAAALBNEJFBNNJCGHIBMEBDKCOCPIGHGNEHDHLFAAJAO";
                    break;

                default:
                    throw new ApplicationException("Unknown web server " + Environment.MachineName);
            }

            return cipherString.DecryptText(connectionString);
        }


        /// <summary>
        ///  Get all county Regions
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetCountyRegions()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetCountyRegions");
            return GetReader(commandBuilder);
        }


        /// <summary>
        /// Get species observations with specified ids.
        /// </summary>
        /// <param name="speciesObservationIds">Ids for species observations to get.</param>
        /// <param name="maxProtectionLevel">Max protection level.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetDarwinCoreByIds(List<Int64> speciesObservationIds,
                                             Int32 maxProtectionLevel)
        {
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter, name the stored procedure
            CommandTimeout = 600;
            commandBuilder = new SqlCommandBuilder("GetDarwinCoreById", true);
            commandBuilder.AddParameter("SpeciesObservationIdTable", speciesObservationIds);
            commandBuilder.AddParameter(SpeciesObservationData.MAX_PROTECTION_LEVEL, maxProtectionLevel);

            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get species observations (in Darwin Core format)
        /// that matches search criteria. 
        /// </summary>
        /// <param name="polygons">
        /// The Polygons.
        /// </param>
        /// <param name="regionIds">
        /// Region ids.
        /// </param>
        /// <param name="taxonIds">
        /// Taxon ids. Observations related to specified taxa and
        /// all underlying taxa are returned.
        /// </param>
        /// <param name="joinCondition">
        /// SQL join condition.
        /// </param>
        /// <param name="whereCondition">
        /// SQL where condition.
        /// </param>
        /// <param name="geometryWhereCondition">SQL geometry where condition.</param>
        /// <param name="sortOrder">
        /// SQL sort order.
        /// </param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetDarwinCoreBySearchCriteria(List<SqlGeometry> polygons,
                                                        List<Int32> regionIds,
                                                        List<Int32> taxonIds,
                                                        String joinCondition,
                                                        String whereCondition,
                                                        String geometryWhereCondition,
                                                        String sortOrder = "")
        {
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter, name the stored procedure
            CommandTimeout = 600;
            commandBuilder = new SqlCommandBuilder("GetDarwinCoreBySearchCriteria", true);
            commandBuilder.AddParameter(SpeciesObservationData.POLYGONS, polygons);
            commandBuilder.AddParameter(SpeciesObservationData.REGION_IDS, regionIds);
            commandBuilder.AddParameter(SpeciesObservationData.TAXON_IDS, taxonIds);
            if (joinCondition.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.JOIN_CONDITION, joinCondition, 1);
            }

            commandBuilder.AddParameter(SpeciesObservationData.WHERE_CONDITION, whereCondition, 1);

            commandBuilder.AddParameter(SpeciesObservationData.SORT_ORDER, sortOrder, 1);

            if (geometryWhereCondition.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.GEOMETRY_WHERE_CONDITION, geometryWhereCondition, 1);
            }

            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get species observations (in Darwin Core format)
        /// that matches search criteria. 
        /// </summary>
        /// <param name="polygons">The Polygons.</param>
        /// <param name="regionIds">Region ids.</param>
        /// <param name="taxonIds">
        /// Taxon ids. Observations related to specified taxa and
        /// all underlying taxa are returned.
        /// </param>
        /// <param name="joinCondition">SQL join condition.</param>
        /// <param name="whereCondition">SQL where condition.</param>
        /// <param name="geometryWhereCondition">SQL geometry where condition.</param>
        /// <param name="sortOrder">Sort order for paging.</param>
        /// <param name="startRow">Start row of page.</param>
        /// <param name="endRow">End row of page.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetDarwinCoreBySearchCriteriaPage(List<SqlGeometry> polygons,
                                                            List<Int32> regionIds,
                                                            List<Int32> taxonIds,
                                                            String joinCondition,
                                                            String whereCondition,
                                                            String geometryWhereCondition,
                                                            String sortOrder,
                                                            Int64 startRow,
                                                            Int64 endRow)
        {
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter, name the stored procedure
            CommandTimeout = 600;
            commandBuilder = new SqlCommandBuilder("GetDarwinCoreBySearchCriteriaPage", true);
            commandBuilder.AddParameter(SpeciesObservationData.POLYGONS, polygons);
            commandBuilder.AddParameter(SpeciesObservationData.REGION_IDS, regionIds);
            commandBuilder.AddParameter(SpeciesObservationData.TAXON_IDS, taxonIds);
            if (joinCondition.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.JOIN_CONDITION, joinCondition, 1);
            }

            commandBuilder.AddParameter(SpeciesObservationData.WHERE_CONDITION, whereCondition, 1);

            if (geometryWhereCondition.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.GEOMETRY_WHERE_CONDITION, geometryWhereCondition, 1);
            }

            commandBuilder.AddParameter(SpeciesObservationData.SORT_ORDER, sortOrder, 1);

            commandBuilder.AddParameter(SpeciesObservationData.START_ROW, startRow);

            commandBuilder.AddParameter(SpeciesObservationData.END_ROW, endRow);

            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get information about species observations that has
        /// changed.
        /// 
        /// Scope is restricted to those observations that the
        /// user has access rights to. There is no access right
        /// check on deleted species observations. This means
        /// that a client can obtain information about deleted
        /// species observations that the client has not
        /// received any create or update information about.
        /// 
        /// Max 25000 species observation changes can be
        /// retrieved in one web service call.
        /// Exactly one of the parameters changedFrom and 
        /// changeId should be specified.
        /// </summary>
        /// <param name="changedFrom">Start date and time for changes that should be returned. </param>
        /// <param name="isChangedFromSpecified">Indicates if parameter changedFrom should be used. </param>
        /// <param name="changedTo">
        /// End date and time for changes that should be
        /// returned. Parameter changedTo is optional and works
        /// with either parameter changedFrom or changeId.
        /// </param>
        /// <param name="isChangedToSpecified">Indicates if parameter changedTo should be used. </param>
        /// <param name="changeId">
        /// Start id for changes that should be returned.
        /// The species observation that is changed in the
        /// specified change id may be included in returned
        /// information.
        /// </param>
        /// <param name="isChangedIdSpecified">Indicates if parameter changeId should be used. </param>
        /// <param name="maxReturnedChanges">
        /// Requested maximum number of changes that should
        /// be returned. This property is used by the client
        /// to avoid problems with resource limitations on
        /// the client side.
        /// Max 25000 changes are returned if property
        /// maxChanges has a higher value than 25000.
        /// </param>
        /// <param name="polygons">The Polygons.</param>
        /// <param name="regionIds">Region ids.</param>
        /// <param name="taxonIds">
        /// Taxon ids. Observations related to specified taxa and
        /// all underlying taxa are returned.
        /// </param>
        /// <param name="joinCondition">SQL join condition.</param>
        /// <param name="whereCondition">SQL where condition.</param>
        /// <returns>
        /// Information about changed species observations.
        /// </returns>
        public DataReader GetDarwinCoreChange(DateTime changedFrom,
                                              Boolean isChangedFromSpecified,
                                              DateTime changedTo,
                                              Boolean isChangedToSpecified,
                                              Int64 changeId,
                                              Boolean isChangedIdSpecified,
                                              Int64 maxReturnedChanges,
                                              List<SqlGeometry> polygons,
                                              List<Int32> regionIds,
                                              List<Int32> taxonIds,
                                              String joinCondition,
                                              String whereCondition)
        {
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter, name the stored procedure
            CommandTimeout = 600;
            commandBuilder = new SqlCommandBuilder("GetDarwinCoreChanges", true);

            if (polygons.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.POLYGONS, polygons);
            }

            if (regionIds.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.REGION_IDS, regionIds);
            }

            if (taxonIds.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.TAXON_IDS, taxonIds);
            }

            if (joinCondition.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.JOIN_CONDITION, joinCondition, 1);
            }

            if (whereCondition.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.WHERE_CONDITION, whereCondition, 1);
            }

            if (isChangedFromSpecified)
            {
                commandBuilder.AddParameter(SpeciesObservationData.FROM_DATE, changedFrom);
            }

            if (isChangedToSpecified)
            {
                commandBuilder.AddParameter(SpeciesObservationData.TO_DATE, changedTo);
            }

            if (isChangedIdSpecified)
            {
                commandBuilder.AddParameter(SpeciesObservationData.CHANGE_ID, changeId);
            }

            commandBuilder.AddParameter(SpeciesObservationData.MAX_RETURNED_CHANGES, maxReturnedChanges);

            return GetReader(commandBuilder, CommandBehavior.Default);
        }

        /// <summary>
        /// Get species observations (in Darwin Core format)
        /// that matches search criteria. 
        /// </summary>
        /// <param name="polygons">The Polygons.</param>
        /// <param name="regionIds">Region ids.</param>
        /// <param name="taxonIds">
        /// Taxon ids. Observations related to specified taxa and
        /// all underlying taxa are returned.
        /// </param>
        /// <param name="joinCondition">SQL join condition.</param>
        /// <param name="whereCondition">SQL where condition.</param>
        /// <param name="geometryWhereCondition">SQL geometry where condition.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetProtectedSpeciesObservationIndication(List<SqlGeometry> polygons,
                                                                   List<Int32> regionIds,
                                                                   List<Int32> taxonIds,
                                                                   String joinCondition,
                                                                   String whereCondition,
                                                                   String geometryWhereCondition)
        {
            SqlCommandBuilder commandBuilder;

            CommandTimeout = 600;
            commandBuilder = new SqlCommandBuilder("GetProtectedSpeciesObservationIndication", true);
            commandBuilder.AddParameter(SpeciesObservationData.POLYGONS, polygons);
            commandBuilder.AddParameter(SpeciesObservationData.REGION_IDS, regionIds);
            commandBuilder.AddParameter(SpeciesObservationData.TAXON_IDS, taxonIds);
            if (joinCondition.IsNotEmpty())
            {
                commandBuilder.AddParameter(SpeciesObservationData.JOIN_CONDITION, joinCondition, 1);
            }

            commandBuilder.AddParameter(SpeciesObservationData.WHERE_CONDITION, whereCondition, 1);
            commandBuilder.AddParameter(SpeciesObservationData.GEOMETRY_WHERE_CONDITION, geometryWhereCondition, 1);

            return GetReader(commandBuilder);
        }

        /// <summary>
        ///  Get all province Regions
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetProvinceRegions()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetProvinceRegions");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get species observations project parameters for specified species observation ids.
        /// </summary>
        /// <param name="speciesObservationIds">Ids for species observations.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetSpeciesObservationProjectParametersByIds(List<Int64> speciesObservationIds)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetSpeciesObservationProjectParametersById", true);
            commandBuilder.AddParameter(SpeciesObservationProjectParameterData.SPECIES_OBSERVATION_IDS, speciesObservationIds);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get all species activities.
        /// </summary>
        /// <param name="localeId">Id for locale used by user.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetSpeciesActivities(Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetSpeciesActivities");
            commandBuilder.AddParameter(SpeciesActivityData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// The get species activity categories.
        /// </summary>
        /// <param name="localeId">Id for locale used by user.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetSpeciesActivityCategories(Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetSpeciesActivityCategories");
            commandBuilder.AddParameter(SpeciesActivityCategoryData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }
    }
}

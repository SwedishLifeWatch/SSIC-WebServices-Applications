using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using NotUsedCommandBuilder = System.Data.SqlClient.SqlCommandBuilder;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.Database;
using SqlCommandBuilder = ArtDatabanken.Database.SqlCommandBuilder;

namespace ArtDatabanken.WebService.TaxonService.Database
{
    /// <summary>
    /// Database interface for the Taxon database.
    /// </summary>
    public class TaxonServer : WebServiceDataServer
    {
        /// <summary>
        /// Create an instance of a database.
        /// </summary>
        public TaxonServer()
        {
            CommandTimeout = Settings.Default.DatabaseDefaultCommandTimeout; // Unit is seconds.
        }

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

            // Opens the database connection.
            switch (Environment.MachineName)
            {
                case "ARTAP-ST": // Team Artportalen test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGOCKLHDJNMOKAHENLNFMCAIBJFNLKGFFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALGBOHIGCNBDLCIKDKPCACCNLPBGLNGNNAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPFFEMHJMMEIFCMMCKLBMJDJOICHOLKDDIAAAAAAADNMAMAEDEPEAICKJHBNIFLEFHFKKGNEHHNCOMCGGJFLEPDIDFIPPEBIPGBGIMFEGBLLMKDKKDLDEDAFOPNHFOICPLFIINKCEDGMPMEFDLDIFMOAMDNNHMGHEHCJDFFPMBBCDCGPNJBMMBHGDGGDHJPLCFDMKGBDOOBJFAMLNAGNOPIEBKHKIHBPDMDEFPPNKCHPKBHKICHMLCPDAOCMMPANPJJPIONHOCCECHCGBEELEAPICAIOGPBHAJCKPMHCPBEAAAAAAIBKOIGNCDCOEJKOPHMHOFCOGKLCLJNFFDBBAPDEG";
                    break;

                case "ARTFAKTA-DEV": // Team Species Fact test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAKPNLADNBHMLFKMCCDFFGHKFAJLNCINDNAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAEMFGCNDAJBACNOHILLGLMEHKODABLCBGJAAAAAAAKJFFDKBICKLEIIOKIEMANLCDHBEDCBJDECNJMNEJPBJPDKMBHHIBCKHCOACMPBENFFIJMMGALKGGHLNPGMDLPLCLMBPKOBLLNEKCCBGAIPIGBFEEMHLIABANDADDNNLNABIAFOIMFADDNPICPLGLBMGADPJAJBOONGNOMPIFAIKJIFPOIDENAKMHIACLPKFNDCPBHGANEJKJBGBIDKHCBDBBCNDHFEPLBABCIOCONJGDMDHBAHGOPACIOKKMOLPDBIAGCDKPNEHELPEPANJNIEKPDFPBCPDPBEAAAAAADIMLKAGPJIDAPDGMHHKMLMOICEGMHMKFOMFCFFDD";
                    break;

                case "ARTSERVICE2-1": // New production web service server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANHGFMCJNJDDFDOKOCMLICICLMHMINLKKAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABEFBIGEMEIICDKKLMOMOOGKELLEAEKEGKAAAAAAALPIABFNICOOFFCLHOPEBMENKGHENHCJPECICDPBCDPIFABDGMFOABGKMPDODNGBEJCMGGCFKMBGEKKCOOHOHNOHHOJCMJLJBAGECLAFAOMKLAJBAJEDJJHAMCGJPFAJGHILFLGLEHMHHGGLLLMENMJPNJPAPODMLPLEEFENJGJLCCOAJOIKBCGMEIEPIHDHKLFJCDCNBEIOECJMDJFCLIHFGLCKGEKECFBJGMOEBEFELOBBNCAGJAFGCAPHCDLHDNCFGBKMFPJHFNPFDMJHPKAIJCIJKGCDHMBLDCFELBOLLJOOBFBEDDJNGAHJNHBOMBEAAAAAAFFFPMELIDILKOCLCCOHCAKJNPNEFOOHDKOJDAHCC";
                    break;

                case "ARTSERVICE2-2": // New production web service server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAABGOPFHEEFOIIMNKGBNFHGINFJOPEIMJCAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAANFCIPAIAIOEMAGKLLPFFKPCOADHGOONGKAAAAAAAOGPBCCNLMHEAGPFNJIDGJBHLNGJKIBDJDJOELNNNEAPLLLIHBGBJLKCOJOGIKOGAHOKCMKCMPPCLJPIGBJHMELNFONMEHDFGOFMPLMEPKJOEPLFNHAEGENCPNKMOPIGBOHPCHCNCHDIIEBDPEAJLPHKELJLNJHKLFMHHPICEALMANCIKCECPODMOGOPKJDNMJGAMLDDDAMPECGKKHLCMJNLOCPHBABJJICPFLOCLFKCHGJMJHNKCAOIEEOFADMBFIHEKLLNEBMLMJNHDMLJIIHAEMCIKPANFHIGNJJAJEDPDPJMKBGNNMNMIJDBENLMFBEAAAAAAKBGDCMOBCPFJGEOCDLFFHLDGFNNCMMEAKPBGHEOJ";
                    break;

                case "LAMPETRA2-1": // Production Web Server
                    // connection string to sql-belone2-1.artdatadb.slu.se\artinst5
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAADJEJOJGKCKIKGFEGJMLDIGOKGPIKEIJAAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAADJKFJNBGJJGCNANJJEFOBPHNCDHEAKNAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKMCCAMGPJECKFJGFLCCDPHBMGGMMNHLKKAAAAAAAECLLPJCFCFANBIDFIPHBPNABFDKPOHBPCEFOCLKKDICGGNHGKHLFNGLHEJHONNPCINMFIMEMCHEDJGJEBOIKENDPNBDFKGNONDEMNDJFKOOOELCPCBKCFLMMAFJAMAHHFNBHALPADHIAKONHFCFKNKDDIABFBFBPOPLDDIKMEIANMCKFBFCILOIHIKMHNOPEAPIAEBCMLBGPFNBDLAOIIMGHBNBFAHMMNGHAKBOMKAOLLGPLABBHKLEACBCBIPCJFBAJEIDNPNFIJICFPLIKPNMKGCEPGCCEEABBMILNNIGCAOIHLODPHNPPLKIABDAABEAAAAAAAPBNOCFIFDOJABMIPNHOOCPICAGOONKMLDADBFFG";
                    break;

                case "LAMPETRA2-2": // Production Web Server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALLEFIHOPGJLCLEEKKHONMLFNJGFDHMPDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFADJEKCDMFFLLAEAKDCEKMKDCBMNAFEFAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAICMFLJBKEKJIGNPDBMBJJJBEIOKADOKLKAAAAAAACKLCGHENNDFGKPABIALNHDMDCOINIKJEKPKHIKJNOKJNAFBIJJMAEDFKKKHLKOLGLKLCMEFJFHBIFCJKCLLELPHAOIODKPGNNHPHMPHNPOLDLMELDCIIJGEPBHPGJLIOKFJNEFHCFOBBIKGIMGKPKEKFBHOMIDMJPNMFOCFBLHKGPJDJMGJGPOKJJOBDCFOEBLNFGGKLLNBBANJHLAIBDJJKABGOKLAAOBLEEAMIHJAGCADKAAKHHBACMEDAJCPHACKOIGBMPLLELLBFBBCGLAKGPJPPAJFGEAPEDMCHHPFGCFENEKHMIPIBAFAANPAFBEAAAAAAAACNHNMPEEMBNLDHEOFJPIGHDKCPBDKFPLIDJOAB";
                    break;

                case "MONESES-DEV": // Test Web Server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEAFPECBBEFEHIKEGLFFDMHECEONJDGANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJPFGEKOBLGBPJLJNFKDCABCJKOINCHCLAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKMDECACNPLECGOKAFICMFBIHEOBEJGCGJAAAAAAAAPFGBKBEDGJOHBFMLOGINLAELOHCILCNHKFILCKLKDPECOHOBGGCMLDHECJJHMMFGDGIFGNMBOMJHKPMNLFIKDFEPBPOAIHLJIAKCLIJHJBJINPOIFLGNAKCGPJIFKGJDCKOKFMELGAEHANICEHNBKDNGKGLFPMOAKBFFHGLGINOGJEJGENACNNNDMPLLEANCFGJOEOAIDFHDFFIGJHDOBGCHAEEMEMCNABKCNODKJLGDOBFOAFIPBPOPIJABPNEOHBOEJHCCGLBOPNHNDNJBNILEEANBOCLBEAAAAAAOBBDFNHBGFLAKDHIDMPPNKGIKNEEECAJKPJKJANF";
                    break;

                case "MONESES-ST": // System test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIKGEBDAPMKNOAFEKIHFGCOJNGHIAKFIAAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAKIENINNCOJNAMMGCGILCBOBMKPCHFEBIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPKMEJDFNDDKKJECHOCBJJFHBDOJCJGBPJAAAAAAAJGNCPNNBBOLPAMNFHIMEOPIJLIGNADIKJNKEIFPLDMKLENDHHJGNOKAJDNFOMDBCHEEFGHAIDKKOCKHELFABMDAJNAEAIPELJMEEHBOBCCEHLILCHDAHKJAGBJNCFMDPIAFMIGCLNHDHCKPCPKBCHFMOIGIEACLPILMAAPILAIEEBFDPIMJEJAGFNMIFGPAHCNNHHPINPCPPJOAECJKNACBEGKFLKODPGELECDHNMCAGGBIMICHFELJCMLOPHOHKMHKCCIMCHAEFDIGKPIDAINPBPKKKMGNHBEAAAAAAMLPBJPJKDBJNNILCDMEHDBODOKBHKKIIKNMMPBIG";
                    break;

                case "SLU011837": // , new computer
                    if (Configuration.InstallationType == InstallationType.Production)
                    {
                        connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPFOBOEOIJOPHGCEFJGCCBMFMHLOBJJLJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJJPNMJFBGLJONCJCFFGEFMENLMBOGPKHAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAIGNMPGHLOCIHMHCMIDLBNMKIKFBBIBKDKAAAAAAAPBPPCKOFBIPIGHKNENKOKENBOHNNFJLGIALODLABLAHENFLAHNFOJHPDHCBPJFKFHKFNAPAOOLNDDKLGIHEPIEHPNOMCKKGGOMFBNKLJCAHDEKINBIMGJIIEBLFOHCAAMDIPGLJCHIBKHKACFHLFMGFLJEDPPDNKEIAMFPGLJJMIKLCLLNCMIEIKDFMDGEEBKICFEHHPPPBMPGAJEDGHBCAGMBAFALHJOMLMOAIHKEHPCKHBEJKOBKOCOEHFIJCFPLPFGKIGBAKMMHJEDKOACDNIPCMLIGGHKKKDNCBHBIBHJDLJJABBLMKMCBHIGELCBEAAAAAAKIEMMPFKDHAGDDEBODJKIBABGGAHPGIMPFINBNLL";
                    }
                    else
                    {
                        connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALCPBDBGCIAFPEPEILEKMLLIKEOOJMJMKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAGKGIMMGHMCLFEAJECPMKCMOMKOBKMEOBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAEHOKACOFCFFAANJNCAFFCAPLHGDPMNLNJAAAAAAACHMPKLCOGMNIPJPPIPCKHLLOFIICBGEJCPPEJKEIFEPANDCNHDABCHCLKFPOGHPNNCMKGPPMILGKFFGKHHOBOIGMANOHEIAGOANHPOPICFAMGGOAAOLFHEKIEDIJJCPEGGHKOGGDOPIEFMIHOHJCIFGGNMMMPJCHDBOKGHECKCMDILHGDPEPHHHMDONGKLJGAGLAGKJFPGOJJHNNMDPGGHACCFIIOHPLHFFAMDBPGPFMJFIMNAOPBFJBEAADMKJDKKEENNHCILFALOJGGGGAHLEJNNLBKFOIBEAAAAAAJMPOPDLIPOHIIPFLONEKBFKIPFOMJHPADDLKOMMM";
                    }
                    break;

                case "SLU002760": // 
                    
                    connectionString =
                        "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABOMKAMDCPAJLHAEPJLCAJNJPFILAGJCLAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPLNNIONDBFLBHIFGOPAEPGFAPDEDJFFEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALNDLGGDDEKOGLEBBLMBHONMNCECKNHBOIAAAAAAACABNBGGBPDHAAKGMMKMMFBCAFBANMPDMPNOPIDJPCGHNFDKGFDILOFJIKPDCHLNOOFMPFJCCFJDECNPGDMNKJCPCDKPDOMADFDMINBEOGMKOCDEMDFCPJEHFMMAMCEJHJKPONNLOHAPBGICFCGAHFPKGJFOEBCMHAHCNNEJIDNGKJMLKPMGGGGALGABAKKCONPHHCGMHLGNIDIPDLLOACKIPCOOKFEHPANFGBMOCLEDOELOKFMNKANHAEDNKEJDHBEAAAAAAINHHFIGEALAJDCNNJNCKGMPGFNAOILPFHJGBOBHL";
                     
                    /* PRODUKTION 
                    connectionString =
                        "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABLNGOFPIAKAPJGEGLMHEKNBDAALJOEMLAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALJEMODNFEMAKNOKHPNODFDCJCFIHFNBEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAGKADFEMAAEAFPHBFPFFPNJEKPGGKMBKDKAAAAAAAEBOMBOCIGNJGECPFNAKKJDKALFGELIDAINLLOOENEFLLFLDFGMOAKIICBDEJCAOAKPDFKPBMLDKLAGPIIOOBBPKGKGKMKACGMIMGAILBECHGBCJOKEELBHKFEFCPHNMPFJDMIHIJLGDGMHPJLHCEMMGFPIHFOBMBALCDGIHGCIEFHFBFDGEGLKMBPCKJNIIAOLMAKBIHFKONPFIJIKLADNDMBGOOHJGJJAKPPKOEFNIJBIHFKHHLOKBIAPNPKGIBAHBJLABHEBIMBENDLGKEHCNAHMCANFNPAMCMDCOFGGBMCOPGOMALMGBMLKGPEJHJBEAAAAAAKLJLBCNKBKLFGFBKECGMPAMIGOENEMPIIKGKCEEI";
                    */
                    break;
                    
                case "SLU003354": // 
                    connectionString =
                        "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGLGBDIAIDBHFHEEKIOMMGMEHJFINCANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACIPKCOABLFKEAGAIHJJOHKOKEAGOONAEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALENOFLCLBGIHENNMPLCPKGAEKHGFFDFCIAAAAAAAJBHDMBPEBEEPPHEENJLENHKKALJAFLFIGBKAJHHGDPCEGFEHKOHPLGPCHJLKEAHDDMLFEMINKCJMAMEHBFJJHAGFALLODOPKAEJGMIAPLAFEJADEBIBOEICDPBOKNMIIMIACIFPHGKLMFAIIANBPGABKPLGHHLOPDGACKGFAGNAJOANALDFKCDOCBDAMFOAJKEHIBOMIJJNFCCCPGBEAKMCNAKBBLFEFFJGEIHAMBMFLJNHLKEMMMPIBKPFCNPCDBEAAAAAANCMHODILJONBDDPIGCIHENGMJMMMMPMCCIPNEPNO";
                    break;

                case "SLU002759": // 
                     connectionString =
                         "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAHJPABDBOIBDFPDEIJCANFPFNLFEHMPODAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACCBCEFHJKBNFFEHCOCJLCDEPEHKJKDDDAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKCJGFNFNGKHCNLCGGDAEKPELNPJGCBGFIAAAAAAALMINKFILJLNPDBOIIIDMBOOOCINNHMOFKBIKEEKGMEDOJAAILOJJLGCKMFJDCEIOEHDKKLGMDEALAAIHDHNGINMJEIEJDPGMHFCCLJDICMDAFKAOBLKNEJHLBAEFPEODDJJFGNOGAGOKEOHOCOJJDMPBNJDLGPBJKDALOLKDGJFKGMLFIMDNGKIBAKBBFBGGIALMEJGOPIKHFDMAPJIFLLMEEIFIFNGOBIELMPNCKPNEFEODNPGJOAIGCKIIJPDLBEAAAAAAGAIDOFMHCPMIODGGIAMKHNLOOFOOGGODHNIDKCJP";
                     break;

                case "ARTDATA-TFS": // TFS-Build 
                     connectionString = 
                         "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABCNHOEMJPBDFAPEFLODEONCFHMLDLFFJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFMAFKFAEGODKNNOJMFLDLMLNIMEKKPPGAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAMDCKBFJKPKEGOGGMEKGPLDJPMBGNKEMAIAAAAAAANJCMNEALPBJJPIIPJFKIOODMHKBLLLBLAGBOKIIOFIHMCOJABLKILIIBMJFPHOEGAEGDFFOAFNLNDKPIHOBKAKBNBENEIMAOIGOIFIHLDOCPEKGGOOAFFFICFMNEPKMMLDIAAIIKMHDIEDIKKCEPLFNDACCBBEONACFDLDLOLKJOGCNMJHLOMJGCGHHHIJDOFNLGHNPGFMPEKOEHDBADADJGAEBKHCPLMLFHDKNICOECJGPKHFFCBCLCHJKDMPLJBEAAAAAAGAMODFJGCGDOEMCJBJDDILHCGJCPKEIPKEDJILKG";
                     break;

                case "TFSBUILD": // TFS-Build server
                     connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAJDEHMIDHELNCKKEHLHOKFNFFKPMHAKNDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFIOBMPHBFAOJKJFCJDBEOLECJPEOIPCIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAAANJJOAJMCBDGIJLJIKOOMMGKLOGNLHPIAAAAAAAOANHGOMCOMMIOMGDIKIEIMJMLEIJNBJLBOIMLOCPHILOJDDJPFAINAMAFDCOKOPJDCFOMFHEEBDALBPKLJACGIIGAEPOOCNLDGJNLECMJJCCDODHPOHGJKLBEFENJLEFHGBNHHMDHOHDLEEOGMAADGPNDNILLCGFJEIJJLHFDDONOKMPBIFEDONHMAACHGHHINABNCHDHEBOKFBJOJDEPJCOLIBMEIPDINJCDFIGKCINLGFJONJBDODAINFECPMJBEAAAAAALNEOMFKLBDDNBCOLEGDIMNFCLNPKEPJMGPGOADCN";
                     break;

                case "SLU011730": // 
                                  //if (Configuration.Debug)
                                  //{
                                  //    //Debug
                                  //    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGPPIFKCAOBFHIJEFLJMMMJPBEPDLMCIBAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHPDKCHJAACNFLLDKHFGNEFIEIJIOFCAGAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADCKBNNEFCIOBPJIFBOGBDENAGEBIBALJIIAAAAAANJPFBIHBCDLHDIDBOIHIBCDECIJBLGFJGEKAHLJBFMOCNNMFLGKBCPLLKBAHKJMENAOEKJNHCNFLADECFDEBPHPAHKKDDNCBNDBKCKEKIMHKNANIDJLNLNCBOEOMDEGIDHKCDCOMBIOINEMCPOLAJALKAAOOINIMEINCJKOBOMMIAOLPNMPCOHKFFNKOFODBPHNEAKJINPHGAJMLIOAMOEJEFGAMAPFMOCNILOFPCLFEFDCHLLOBNCGAEOHAEDMDEENAGNFBADKOJILBBEAAAAAALCAHLIDBKCHDHKCAJNIDFALHNJGKDLEINOCGMDKH";
                                  //}
                                  //else
                                  //{
                                  //    //Produktion
                                  //    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGKOONNFFCICKGAENLBBKPOHHMCHDGMEDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADCLPFBKLAALMJJDHGLNDPKCLMPCBBBBIAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAECBLOEDDNJMLFLNODEOBANBCHECHFCJJKAAAAAAANNCGEMLHBNPFBKGEEOIDNEHIKJFNEEOOPLBGPFHOOKOEAKNJMCAKFFJHNHOJKKKGBDDJJBFNNAFKLPBJEAMNOCEGIPBBIPGHOADMFBFKNNBEFFLKCJLFCOICPFAEEPEAGCLGCEHICPBBKGFHBEOEAJBJGNIGFBHMKJCDGCMGKAOKGOAJDFLKJHPLPFLKAFINFNAPBNHOEEIKMMIGBEKMEKNDHNPJIHGGGOJKDCDFDLEKHPNIPCJBNIFDGALHHPCPDHAHGKHMNOPMEEKONCEPDMIMLKEHLNEGBAHEAKPCBEKCJILDLNKJIDLIEMNKBGLHBEAAAAAAFHBHMINPIENLJDDKPADEFGBMJCGLPFKINAIKBFAN";
                                  //}

                     connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGPPIFKCAOBFHIJEFLJMMMJPBEPDLMCIBAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAHPDKCHJAACNFLLDKHFGNEFIEIJIOFCAGAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADCKBNNEFCIOBPJIFBOGBDENAGEBIBALJIIAAAAAANJPFBIHBCDLHDIDBOIHIBCDECIJBLGFJGEKAHLJBFMOCNNMFLGKBCPLLKBAHKJMENAOEKJNHCNFLADECFDEBPHPAHKKDDNCBNDBKCKEKIMHKNANIDJLNLNCBOEOMDEGIDHKCDCOMBIOINEMCPOLAJALKAAOOINIMEINCJKOBOMMIAOLPNMPCOHKFFNKOFODBPHNEAKJINPHGAJMLIOAMOEJEFGAMAPFMOCNILOFPCLFEFDCHLLOBNCGAEOHAEDMDEENAGNFBADKOJILBBEAAAAAALCAHLIDBKCHDHKCAJNIDFALHNJGKDLEINOCGMDKH";
                     break;

                case "SEA0105WTRD2": // 
                     connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABKIOIJPBLDPKPCEELADDGIHIPCLMBNMGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAAKKMOANLIFEKJKJIODLPOKIFLFAOKGHEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAANLJGJDGKIFJPNGBOJNIDKHIPONJLPFCGJAAAAAAAEEECLECPIFKOFODCKAHPABJCJBGMFEEDCODOAMMCCPEICCGEIENACFMHGMNANBAOCCGBAIFLMCNHAGENANLKDPPGPGLLMNCPKMKICFMIGBDEFLPLDMCFKHMKEPFHBKPDPECEMADCNKFLNGBGHDPEJLHEDHMOFDMNHEKHJJDODFHMNEPDLDPJAEMMLKNDMDILDDGMOMADLLAPKDAHEGBLFOFEEIICCOHLCLALHAMEFEHJKMGJGJHKNEJJENEJOIGELHJIKPLODIPEDBOGOGDPOBCPDPLPEECABEAAAAAADANCCEHAEHBMOIHLKHCDNPFNNFHFHBKBIFOKMMBN";
                     break;

                case "SLU011895": // 
                     connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAGOMDGCCAOKJOJMECJEKHFDIAIPDOIEMOAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADIKGOCBALDAHEPODDEJJGLNCCKAJNCFPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAFLBDFNHCJOJOBBKFMOBLJKHNOKHPBOLAJAAAAAAAHBEDHPOCIAHCDJKDFLKEIKPIDHKECBLHMAGPLCHDIFIPMCLKDJCADCLEJNOFFNFDAKABCAMLMANAFPENHOFLNNDBILFBMKACHABJMLDJBAOHKFMNHNFEJMKKLCAHKMLJPEHOKLDNPOPPLEKEDPMFMIAAOCBDHKMNDACOGMBNFGNHLPLEGEGHOICGDKOKMHBJHEPMJEMAOKJOHGHDBINJOHCDOEBPJJHOEJIMNADEOFBLPFGLCLOOICOKFBBOOPKLJMPCDPLJHMBDNIGFDDGDELENHOLMGCIABEAAAAAANNECKLOBDDDJIIMBNLMGFIDJEPFGICLACNONGECI";
                     break;

                case "SLW-DEV": // Team Two Blueberries test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAENCFAAFBOMFAMKNONBANCPOEJDNNIMHBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAELNPHDCAAHLKILKDCFOAJDKLMOKJBEOGIIAAAAAAKEMFMHGDGIBFBDNCCMPBFOLKDPDBLHMOJAEIOPLMJALGJECBGOPNHMPBJMMLCFABHJEHNCHOJJDOEONPLJIJDDOALAONJNBCLNLPAIJCJLOGDLMOEGADECPIPHFIJMKCPFLNEBLJFHLLAJBHHMNLFCGIAEPFJHACJLCOCHMMOKHDKFCLBMMDNIGECKIAIEICPIJLLKJEDFLFKGDAGHHJCCLBCPCKNEFHFGNDGKLPGFDPEIPFKMCFNGLHCCAOKCAIAEBNFNPLGCJCODDNBEAAAAAAAHFCCOIDFEMBBNAAGMEMCMEJDLMCNMKBLFGFEPCE";
                    break;
                
                case "SLU010576": //
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIBCGMONNPLJJELEOJBIEMOPHMHBPHABPAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACKJCBCOKJELOJLLMDNMNCJKIMIONBCKEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHEDKBLMBGCGNFEDJHIHNMIDNNLJOOLIHJAAAAAAAJCLNJGJNPOCHMDNDGDMDNNNOHIHCECKJAECOGDOACJKFNJAAJNCDGDLPKAEFEHFOLIFNPJGCIECEIIDBGBKNHBDEEBMPDKDHBIICPNGAFMLAGDEJANFACEAFFKGPHFBJEEPEDPCPKLIBJFFIGNDMHKHENPDPNENIOLOMIOBNOOHACJOKKKFHNOGIFJJLJMIOGFLOHIELDHKCJLPADACCAKNOLKHHIBGBHNGLPOPEHLMAICGKBAPGBBCLCCFKGDHONGOBAMEEFOBHKFLNOKOKLLDGOKLCBOAJBEAAAAAAIBIEHMJMNMALJCAGENBAMNOOECGCPCAFJAKLFHKN";
                    break;

                case "SLU012925": //
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIPPPDJLGOBFNIKEAIDCFOGJFLFHBHOLNAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAABLNNOIABJHINDLFDPEGLLNBCLLOMKEOFAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABOLMDDOJJMPFILBGDKKIBJIKCENIDCEBIIAAAAAAHPLLACLCDMDGOBIEMHBNGMLKBOKGHMGIOMEOJCJOPMECGKPBPKEMHEEALNMFMIEBKBNCMMNALJCKALKBONCHBKODCLMNPKBIHCFKCKNJDEENOLEHNELEDELGGAHMHHDDKCABNDAHMBALFEGNNBNKMICCJFIHPLOCECMOGJJKOPPPNNAKMBAKFBKOGONAMDODHFCADDKJNBCFHMOPDFMCEKFBJGCPFIKFOPFNOLGIHMEAHJKBLCNDDBLJCJALPHCICFOPIHLJHDMJMJLIBEAAAAAAMLCLCJBFEHNHGGDHENHGOGPECEIBABDKOGMBMABK";
                    break;

                default:
                    throw new ApplicationException("Unknown web server " + Environment.MachineName);

            }
            return cipherString.DecryptText(connectionString);
        }

        /// <summary>
        /// Inserts a taxon into database. 
        /// </summary>
        /// <param name="createdBy">User that has created taxon</param>
        /// <param name="personName">Name of a person</param>
        /// <param name="validFromDate">Valid date to </param>
        /// <param name="validToDate">Valid date from</param>
        /// <param name="localeId">Language id.</param>
        /// <param name="revisionEventId">The revision event id.</param>
        /// <param name="isPublished">if set to <c>true</c> [is published].</param>
        /// <param name="referenceIds">The reference Ids.</param>
        /// <returns></returns>
        public Int32 CreateTaxon(Int32 createdBy, string personName, DateTime validFromDate, DateTime validToDate, 
                                 Int32 localeId, int revisionEventId, Boolean isPublished, List<int> referenceIds)
        {
            if (referenceIds == null)
            {
                referenceIds = new List<int>();
            }

            var commandBuilder = new SqlCommandBuilder("InsertTaxon", true);
            commandBuilder.AddParameter(TaxonCommon.CREATED_BY, createdBy);
            commandBuilder.AddParameter(TaxonData.PERSON_NAME, personName);
            commandBuilder.AddParameter(TaxonCommon.VALID_FROM_DATE, validFromDate);
            commandBuilder.AddParameter(TaxonCommon.VALID_TO_DATE, validToDate);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            commandBuilder.AddParameter(TaxonCommon.REVISON_EVENT_ID, revisionEventId);
            commandBuilder.AddParameter(TaxonCommon.IS_PUBLISHED, isPublished);
            commandBuilder.AddParameter(ReferenceRelationData.REFERENCE_IDS, referenceIds);

            lock (this)
            {
                CheckTransaction();
                // Returns TaxonId for the created taxom
                return ExecuteScalar(commandBuilder);
            }
        }


        /// <summary>
        /// Get translation from database. 
        /// </summary>
        /// <param name="id">Text id.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>Translation of specified text in requested language.</returns>
        public string GetTranslation(int id, int localeId)
        {
            var commandBuilder = new SqlCommandBuilder("GetTranslation", true);
            commandBuilder.AddParameter(TaxonCommon.ID, id);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);

            return GetRow(commandBuilder).GetString("translation");
        }

        /// <summary>
        /// Updates an existing taxon.
        /// </summary>
        /// <param name="taxonId">The taxon id.</param>
        /// <param name="validFromDate">Valid from date.</param>
        /// <param name="validToDate">Valid to date.</param>
        /// <param name="revisionEventId">Id of the revision event.</param>
        /// <returns>Id of updated taxon</returns>
        public Int32 UpdateTaxon(int taxonId, DateTime validFromDate, DateTime validToDate, int revisionEventId)
        {
            var commandBuilder = new SqlCommandBuilder("UpdateTaxon");
            commandBuilder.AddParameter(TaxonCommon.ID, taxonId);
            commandBuilder.AddParameter(TaxonCommon.VALID_FROM_DATE, validFromDate);
            commandBuilder.AddParameter(TaxonCommon.VALID_TO_DATE, validToDate);
            commandBuilder.AddParameter(TaxonCommon.REVISON_EVENT_ID, revisionEventId);

            lock (this)
            {
                CheckTransaction();
                // Returns TaxonId for the created taxom
                return ExecuteScalar(commandBuilder);
            }
        }

        /// <summary>
        /// Create a revision.
        /// </summary>
        /// <param name="taxonId">Taxon id.</param>
        /// <param name="descriptionString">Description string.</param>
        /// <param name="expectedStartTime">Expected start time.</param>
        /// <param name="expectedEndTime">Expected end time.</param>
        /// <param name="revisionStateId">Revision state id.</param>
        /// <param name="createdById">User that creates the revision event.</param>
        /// <param name="localeId">Locale id.</param>
        /// <param name="referenceIds">Reference ids.</param>
        /// <returns></returns>
        public int CreateRevision(int taxonId,
                                  string descriptionString,
                                  DateTime expectedStartTime,
                                  DateTime expectedEndTime, 
                                  int revisionStateId,
                                  int createdById,
                                  int localeId,
                                  List<int> referenceIds)
        {
            if (referenceIds == null)
            {
                referenceIds = new List<int>();
            }

            var commandBuilder = new SqlCommandBuilder("InsertRevision", true);

            commandBuilder.AddParameter("TaxonId", taxonId);
            commandBuilder.AddParameter("DescriptionString", descriptionString);
            commandBuilder.AddParameter("ExpectedStartTime", expectedStartTime);
            commandBuilder.AddParameter("ExpectedEndTime", expectedEndTime);
            commandBuilder.AddParameter("RevisionStateId", revisionStateId);
            commandBuilder.AddParameter("CreatedById", createdById);

            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            commandBuilder.AddParameter(ReferenceRelationData.REFERENCE_IDS, referenceIds);

            lock (this)
            {
                CheckTransaction();
                // Returns RevisionId for the created taxom
                return ExecuteScalar(commandBuilder);
            }
        }

        /// <summary>
        /// Inserts all taxonId that are in a revision into a database table
        /// </summary>
        /// <param name="revisionId">The revision id.</param>
        public void CreateTaxonInRevision(int revisionId)
        {
            var commandBuilder = new SqlCommandBuilder("InsertTaxonInRevision");
            commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Create a taxon property.
        /// </summary>
        /// <param name="taxonId">The taxon id.</param>
        /// <param name="taxonCategoryId">The taxon category id.</param>
        /// <param name="conceptDefinitionPart">The concept definition part.</param>
        /// <param name="conceptDefinitionFullGenerated">The concept definition full generated.</param>
        /// <param name="alertStatus">Alert status.</param>
        /// <param name="validFromDate">The valid from date.</param>
        /// <param name="validToDate">The valid to date.</param>
        /// <param name="isValid">if set to <c>true</c> [is valid].</param>
        /// <param name="personName">Person name</param>
        /// <param name="modifiedById">UserId for person who modified the record.</param>
        /// <param name="revisionEventId">The revision event id.</param>
        /// <param name="changedInRevisionEventId">The changed in revision event id.</param>
        /// <param name="isPublished">if set to <c>true</c> [is published].</param>
        /// <param name="isMicrospecies">Is Microspecie flag</param>
        /// <param name="localeId">The locale id.</param>
        /// <returns></returns>
        public int CreateTaxonProperties(int taxonId, int taxonCategoryId,
                                         String conceptDefinitionPart, String conceptDefinitionFullGenerated,
                                         int alertStatus, DateTime validFromDate, DateTime validToDate, 
                                         Boolean isValid, String personName, int modifiedById, int revisionEventId,
                                         int changedInRevisionEventId, Boolean isPublished, Boolean isMicrospecies, int localeId)
        {

            var commandBuilder = new SqlCommandBuilder("InsertTaxonProperties", true);

            commandBuilder.AddParameter("TaxonId", taxonId);
            commandBuilder.AddParameter("TaxonCategory", taxonCategoryId);
            commandBuilder.AddParameter(TaxonPropertiesData.CONCEPT_DEFINITION_PART, conceptDefinitionPart);
            commandBuilder.AddParameter(TaxonPropertiesData.CONCEPT_DEFINITION_FULL_GENERATED, conceptDefinitionFullGenerated);
            commandBuilder.AddParameter(TaxonPropertiesData.ALERT_STATUS, alertStatus);
            commandBuilder.AddParameter("ValidFromDate", validFromDate);
            commandBuilder.AddParameter("ValidToDate", validToDate);
            commandBuilder.AddParameter("IsValid", isValid);
            commandBuilder.AddParameter(TaxonData.PERSON_NAME, personName);
            commandBuilder.AddParameter("ModifiedBy", modifiedById);
            commandBuilder.AddParameter("RevisionEventId", revisionEventId);
            commandBuilder.AddParameter("ChangedInRevisionEventId", changedInRevisionEventId);
            commandBuilder.AddParameter("IsPublished", isPublished);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            commandBuilder.AddParameter(TaxonPropertiesData.IS_MICROSPECIES, isMicrospecies);
            lock (this)
            {
                CheckTransaction();

                // Returns TaxonId for the created taxom
                return ExecuteScalar(commandBuilder);
            }
        }

        /// <summary>
        /// Set revision species fact published flag to true
        /// </summary>
        /// <param name="revisionId"></param>
        public bool SetRevisionSpeciesFactPublished(int revisionId)
        {
            var commandBuilder = new SqlCommandBuilder("SetRevisionSpeciesFactPublished", true);
            commandBuilder.AddParameter("Id", revisionId);

            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }

            return true;
        }

        /// <summary>
        /// Set revision reference relation published flag to true
        /// </summary>
        /// <param name="revisionId"></param>
        public bool SetRevisionReferenceRelationPublished(int revisionId)
        {
            var commandBuilder = new SqlCommandBuilder("SetRevisionReferenceRelationPublished", true);
            commandBuilder.AddParameter("Id", revisionId);

            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="taxonId">
        /// The taxon id.
        /// </param>
        /// <param name="taxonCategoryId">
        /// The taxon category id.
        /// </param>
        /// <param name="conceptDefinitionPart">The concept definition part.</param>
        /// <param name="conceptDefinitionFullGenerated">The concept definition full generated.</param>
        /// <param name="alertStatus">Alert status.</param>
        /// <param name="validFromDate">
        /// The valid from date.
        /// </param>
        /// <param name="validToDate">
        /// The valid to date.
        /// </param>
        /// <param name="isValid">
        /// Taxon-is-valid flag.
        /// </param>
        /// <param name="modifiedById">
        /// The modified by id.
        /// </param>
        /// <param name="revisionEventId">
        /// The revision event id.
        /// </param>
        /// <param name="changedInRevisionEventId">
        /// The changed in revision event id.
        /// </param>
        /// <param name="isPublished">Flag to indicate if data is published.</param>
        /// <param name="localeId">The locale id.</param>
        /// <param name="IsMicrospecies">Flag indicating it's microspecie</param>
        /// <returns>
        /// </returns>
        public int UpdateTaxonProperties(int id, int taxonId, int taxonCategoryId, String conceptDefinitionPart, String conceptDefinitionFullGenerated,
                                         int alertStatus, DateTime validFromDate, DateTime validToDate, 
                                         Boolean isValid, int modifiedById, int revisionEventId,
                                         int changedInRevisionEventId, Boolean isPublished, Boolean isMicrospecies, int localeId)
        {

            var commandBuilder = new SqlCommandBuilder("UpdateTaxonProperties", true);
            commandBuilder.AddParameter("Id", id);
            commandBuilder.AddParameter("TaxonId", taxonId);
            commandBuilder.AddParameter("TaxonCategory", taxonCategoryId);
            commandBuilder.AddParameter(TaxonPropertiesData.CONCEPT_DEFINITION_PART, conceptDefinitionPart);
            commandBuilder.AddParameter(TaxonPropertiesData.CONCEPT_DEFINITION_FULL_GENERATED, conceptDefinitionFullGenerated);
            commandBuilder.AddParameter(TaxonPropertiesData.ALERT_STATUS, alertStatus);
            commandBuilder.AddParameter("ValidFromDate", validFromDate);
            commandBuilder.AddParameter("ValidToDate", validToDate);
            commandBuilder.AddParameter("IsValid", isValid);
            commandBuilder.AddParameter("ModifiedBy", modifiedById);
            commandBuilder.AddParameter("RevisionEventId", revisionEventId);
            commandBuilder.AddParameter("ChangedInRevisionEventId", changedInRevisionEventId);
            commandBuilder.AddParameter("IsPublished", isPublished);
            commandBuilder.AddParameter(TaxonPropertiesData.IS_MICROSPECIES, isMicrospecies);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);

            lock (this)
            {
                CheckTransaction();

                // Returns TaxonPropertyId for the updated TaxonProperty
                return ExecuteScalar(commandBuilder);
            }
        }

        /// <summary>
        /// Creates the revision event.
        /// </summary>
        /// <param name="revisionId">The revision id.</param>
        /// <param name="eventTypeId">The event type id.</param>
        /// <param name="createdById">User that creates the revision event.</param>
        /// <returns></returns>
        public int CreateRevisionEvent(int revisionId, int eventTypeId, int createdById)
        {
            var commandBuilder = new SqlCommandBuilder("InsertRevisionEvent", true);

            commandBuilder.AddParameter("RevisionId", revisionId);
            commandBuilder.AddParameter("EventTypeId", eventTypeId);
            commandBuilder.AddParameter("CreatedById", createdById);

            lock (this)
            {
                CheckTransaction();

                // Returns RevisionEventId for the created RevisionEvent
                return ExecuteScalar(commandBuilder);
            }
        }


        /// <summary>
        /// Creates a new complete Revision event, i.e. all revision event data is set.
        /// </summary>
        /// <param name="revisionId">The revision id.</param>
        /// <param name="eventTypeId">The event type id.</param>
        /// <param name="createdById">User that creates the revision event.</param>
        /// <param name="createdDate">The created date.</param>
        /// <param name="descriptionAffectedTaxa">The description affected taxa.</param>
        /// <param name="descriptionNewValue">The description new value.</param>
        /// <param name="descriptionOldValue">The description old value.</param>
        /// <returns>Id for the created row.</returns>
        public int CreateCompleteRevisionEvent(int revisionId, int eventTypeId, int createdById, DateTime createdDate, 
            string descriptionAffectedTaxa, string descriptionNewValue, string descriptionOldValue)
        {
            var commandBuilder = new SqlCommandBuilder("InsertCompleteRevisionEvent", true);

            commandBuilder.AddParameter("RevisionId", revisionId);
            commandBuilder.AddParameter("EventTypeId", eventTypeId);
            commandBuilder.AddParameter("CreatedById", createdById);
            commandBuilder.AddParameter("CreatedDate", createdDate);
            commandBuilder.AddParameter("Description_AffectedTaxa", descriptionAffectedTaxa);
            commandBuilder.AddParameter("Description_NewValue", descriptionNewValue);
            commandBuilder.AddParameter("Description_OldValue", descriptionOldValue);
            
            lock (this)
            {
                CheckTransaction();

                // Returns RevisionEventId for the created RevisionEvent
                return ExecuteScalar(commandBuilder);
            }
        }


        /// <summary>
        /// Delete a revision.
        /// </summary>
        /// <param name="id">The revision id.</param>
        /// <returns></returns>
        public void DeleteRevision(int id)
        {
            var commandBuilder = new SqlCommandBuilder("DeleteRevision", true);

            commandBuilder.AddParameter("Id", id);

            lock (this)
            {
                CheckTransaction();

                // Returns RevisionEventId for the created RevisionEvent
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Update a revision.
        /// </summary>
        /// <param name="Id">The revision id.</param>
        /// <param name="descriptionString">Description string.</param>
        /// <param name="expectedStartTime">Expected start time.</param>
        /// <param name="expectedEndTime">Expected end time.</param>
        /// <param name="revisionStateId">Revision state id.</param>
        /// <param name="createdById">Created by id.</param>
        /// <param name="localeId">The locale id.</param>
        /// <returns></returns>
        public int UpdateRevision(int Id,
                                  string descriptionString,
                                  DateTime expectedStartTime,
                                  DateTime expectedEndTime,
                                  int revisionStateId,
                                  int createdById,
                                  int localeId)
        {

            var commandBuilder = new SqlCommandBuilder("UpdateRevision", true);

            commandBuilder.AddParameter("Id", Id);
            commandBuilder.AddParameter("DescriptionString", descriptionString);
            commandBuilder.AddParameter("ExpectedStartTime", expectedStartTime);
            commandBuilder.AddParameter("ExpectedEndTime", expectedEndTime);
            commandBuilder.AddParameter("RevisionStateId", revisionStateId);
            commandBuilder.AddParameter("ModifiedById", createdById);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);

            lock (this)
            {
                CheckTransaction();

                // Returns TaxonId for the created taxom
                return ExecuteScalar(commandBuilder);
            }
        }

        /// <summary>
        /// Update description for a revision event
        /// </summary>
        /// <param name="Id">The revision event id.</param>
        /// <param name="localeId">The locale id.</param>
        /// <returns>void</returns>
        public void UpdateRevisionEvent(int Id, int localeId)
        {
            var commandBuilder = new SqlCommandBuilder("UpdateRevisionEvent");
            commandBuilder.AddParameter("Id", Id);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="revisionEventId">
        /// The revision event id.
        /// </param>
        /// <param name="localeId">
        /// The locale id.
        /// </param>
        /// <returns>
        /// </returns>
        public DataReader GetRevisionEventById(int revisionEventId, int localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetRevisionEventById");
            commandBuilder.AddParameter(RevisionEventData.ID, revisionEventId);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return this.GetRow(commandBuilder);
        }

        /// <summary>
        /// Get all parents for a taxon.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="revisionId">Read data within this revision only.</param>
        /// <returns>
        /// </returns>
        public DataReader GetParentTaxonRelationsByTaxon(int id, Int32? revisionId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetParentTaxonRelationsByTaxon");
            commandBuilder.AddParameter(TaxonCommon.ID, id);
            if (revisionId.HasValue)
            {
                commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId.Value);
            }
            return this.GetReader(commandBuilder);
        }

        /// <summary>
        /// Get all parent taxon relations for a taxon.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="revisionId">Read data within this revision only.</param>
        /// <returns>
        /// </returns>
        public DataReader GetAllParentTaxonRelationsByTaxon(int id, int? revisionId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetAllParentTaxonRelationsByTaxon");
            commandBuilder.AddParameter("taxonId", id);

            if (revisionId.HasValue)
            {
                commandBuilder.AddParameter("revisionId", revisionId.Value);
            }

            return this.GetReader(commandBuilder);
        }

        /// <summary>
        /// </summary>
        /// <param name="taxonId">The taxon id.</param>
        /// <param name="revisionId">Read data within this revision only.</param>
        /// <returns>
        /// </returns>
        public DataReader GetChildTaxonRelationsByTaxon(int taxonId, Int32? revisionId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetChildTaxonRelationsByTaxon");
            commandBuilder.AddParameter(TaxonCommon.TAXON_ID, taxonId);
            if (revisionId.HasValue)
            {
                commandBuilder.AddParameter("revisionId", revisionId.Value);
            }

            return this.GetReader(commandBuilder);
        }

        /// <summary>
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="revisionId">
        /// Revision id.
        /// </param>
        /// <returns>
        /// </returns>
        public DataReader GetAllChildTaxonRelationsByTaxon(int id, Nullable<int> revisionId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetAllChildTaxonRelationsByTaxon");
            commandBuilder.AddParameter("taxonId", id);

            if (revisionId.HasValue)
            {
                commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId.Value);
            }

            return this.GetReader(commandBuilder);
        }


        /// <summary>
        /// Gets the revision event by its revision id.
        /// </summary>
        /// <param name="revisionId">
        /// The revision id.
        /// </param>
        /// <param name="localeId">
        /// The locale id.
        /// </param>
        /// <returns>
        /// </returns>
        public DataReader GetRevisionEventsByRevisionId(int revisionId, int localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetRevisionEventsByRevisionId");
            commandBuilder.AddParameter(RevisionData.REVISIONID, revisionId);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return this.GetReader(commandBuilder);
        }

        /// <summary>
        /// Gets the revision by its revision id.
        /// </summary>
        /// <param name="revisionId">
        /// The revision id.
        /// </param>
        /// <param name="localeId">
        /// The locale id.
        /// </param>
        /// <returns>
        /// </returns>
        public DataReader GetRevisionById(int revisionId, int localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetRevisionById");
            commandBuilder.AddParameter(RevisionData.ID, revisionId);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return this.GetRow(commandBuilder);
        }

        /// <summary>
        /// Inserts a taxon category into database. 
        /// </summary>
        /// TODO remove <param name="categoryId">Category id c</param>
        /// <param name="categoryName">Name of the category</param>
        /// <param name="mainCategory">If category is a main category</param>
        /// <param name="parentCategory">Parent category if exist</param>
        /// <param name="sortOrder">Sort order for created category</param>
        /// <param name="taxonomic">If category is taxonomic</param>
        /// <param name="localeId">Language id.</param>
        /// <returns></returns>
        public Int32 CreateTaxonCategory(int categoryId, string categoryName, bool mainCategory, Int32 parentCategory, Int32 sortOrder, bool taxonomic, Int32 localeId)
        {
            var commandBuilder = new SqlCommandBuilder("InsertTaxonCategory");
            commandBuilder.AddParameter(TaxonCategoryData.CATEGORY_NAME, categoryName);
            commandBuilder.AddParameter(TaxonCategoryData.MAIN_CATEGORY, mainCategory);
            commandBuilder.AddParameter(TaxonCategoryData.PARENT_CATEGORY, parentCategory);
            commandBuilder.AddParameter(TaxonCommon.SORT_ORDER, sortOrder);
            commandBuilder.AddParameter(TaxonCategoryData.TAXONOMIC, taxonomic);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            commandBuilder.AddParameter(TaxonCommon.ID, categoryId);
            lock (this)
            {
                CheckTransaction();

                // Returns TaxonCategoryId for the created taxon category
                return ExecuteScalar(commandBuilder);
            }
        }

        /// <summary>
        /// Inserts a taxon name into database.
        /// </summary>
        /// <param name="taxonId">The taxon id.</param>
        /// <param name="taxonNameId">The TaxonNameId.</param>
        /// <param name="name">The name.</param>
        /// <param name="author">The author.</param>
        /// <param name="nameCategory">The name category.</param>
        /// <param name="nameUsage">The name status.</param>
        /// <param name="nameUsageNew">The name usage.</param>
        /// <param name="personName">Name of the person.</param>
        /// <param name="isRecommended">if set to <c>true</c> [is recommended].</param>
        /// <param name="createdDate">Date taxa was created.</param>
        /// <param name="createdBy">UserId for user that creates record.</param>
        /// <param name="validFromDate">The valid from date.</param>
        /// <param name="validToDate">The valid to date.</param>
        /// <param name="description">The description.</param>
        /// <param name="revisionEventId">The revision event id.</param>
        /// <param name="isPublished">if set to <c>true</c> [is published].</param>
        /// <param name="isOkForObsSystems">if set to <c>true</c> [is ok name for observation systems].</param>
        /// <param name="isOriginal">if set to <c>true</c> [is original name].</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>Id for the created taxon name </returns>
        public Int32 CreateTaxonName(Int32 taxonId, Int32? taxonNameId, String name, String author, Int32 nameCategory, Int32 nameUsage, Int32 nameUsageNew, String personName, Boolean isRecommended, 
                                     DateTime createdDate, Int32 createdBy, DateTime validFromDate, DateTime validToDate, String description, Int32? revisionEventId, Boolean isPublished,
                                     Boolean isOkForObsSystems, Boolean isOriginal, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("InsertTaxonName");
            commandBuilder.AddParameter(TaxonCommon.TAXON_ID, taxonId);
            if (taxonNameId.HasValue && taxonNameId > 0)
            {
                commandBuilder.AddParameter(TaxonNameData.TAXON_NAME_ID, taxonNameId.Value);
            }
            commandBuilder.AddParameter(TaxonNameData.NAME, name);
            commandBuilder.AddParameter(TaxonNameData.AUTHOR, author);
            commandBuilder.AddParameter(TaxonNameData.NAMECATEGORY, nameCategory);
            commandBuilder.AddParameter(TaxonNameData.NAMEUSAGE, nameUsage);
            commandBuilder.AddParameter(TaxonNameData.NAMEUSAGENEW, nameUsageNew);
            commandBuilder.AddParameter(TaxonNameData.PERSONNAME, personName);
            commandBuilder.AddParameter(TaxonNameData.IS_RECOMMENDED, isRecommended);
            // if CreatedDate is DateTime.MinValue - don't send it to the st.proc
            // will be set to GETDATE() 
            if (!createdDate.Equals(DateTime.MinValue))
            {
                commandBuilder.AddParameter(TaxonCommon.CREATED_DATE, createdDate);
            }
            commandBuilder.AddParameter(TaxonCommon.CREATED_BY, createdBy);
            commandBuilder.AddParameter(TaxonCommon.VALID_FROM_DATE, validFromDate);
            commandBuilder.AddParameter(TaxonCommon.VALID_TO_DATE, validToDate);
            commandBuilder.AddParameter(TaxonNameData.DESCRIPTION, description);
            if (revisionEventId.HasValue)
            {
                commandBuilder.AddParameter(TaxonCommon.REVISON_EVENT_ID, revisionEventId.Value);
            }
            commandBuilder.AddParameter(TaxonCommon.IS_PUBLISHED, isPublished);
            commandBuilder.AddParameter(TaxonNameData.IS_OK_FOR_OBS_SYSTEMS, isOkForObsSystems);
            commandBuilder.AddParameter(TaxonNameData.IS_ORIGINAL, isOriginal);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            lock (this)
            {
                CheckTransaction();

                // Returns Id for the created taxon name 
                return ExecuteScalar(commandBuilder);
            }
        }

        /// <summary>
        ///  Inserts a taxon name category into database. 
        /// </summary>
        /// <param name="categoryName">Name of the category</param>
        /// <param name="shortName">Short name</param>
        /// <param name="sortOrder">Sort order for created name category</param>
        /// <param name="localeId">Language id.</param>
        /// <param name="type">Category name type</param>
        /// <returns></returns>
        public Int32 CreateTaxonNameCategory(string categoryName, string shortName, Int32 sortOrder, Int32 localeId, Int32 type)
        {
            var commandBuilder = new SqlCommandBuilder("InsertTaxonNameCategory");
            commandBuilder.AddParameter(TaxonCategoryData.CATEGORY_NAME, categoryName);
            commandBuilder.AddParameter(TaxonCommon.SORT_ORDER, sortOrder);
            commandBuilder.AddParameter(TaxonCommon.SHORT_NAME, shortName);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            commandBuilder.AddParameter(TaxonCategoryData.TYPE, type);
            lock (this)
            {
                CheckTransaction();

                // Returns TaxonNameCategoryId for the created taxon name category
                return ExecuteScalar(commandBuilder);
            }
        }

        /// <summary>
        /// Get DataReader with information about taxa all published taxa.
        /// Data is selected for specific language.
        /// </summary>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxa(Int32 localeId)
        {
            // 2013-02-06 Gunnar
            this.CommandTimeout = 300;
            SqlCommandBuilder commandBuilder;
                
            commandBuilder = new SqlCommandBuilder("GetTaxa");
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about taxa with selected ids.
        /// Data is selected for specific language.
        /// </summary>
        /// <param name="taxaIdList">List of Id values for taxa.</param>
        /// <param name="revisionId">Read data within this revision only.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxaByIds(List<Int32> taxaIdList, Int32? revisionId, Int32 localeId)
        {
            // Use SqlCommandBuilder with SqlParameter
            // 2013-01-14 Gunnar
            this.CommandTimeout = 300;
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetTaxaByIds", true);
            commandBuilder.AddParameter(TaxonCommon.ID_TABLE, taxaIdList);
            if (revisionId.HasValue)
            {
                commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId.Value);
            }
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with all taxa that matches the search criteria.
        /// </summary>
        /// <param name="taxonCategoryIds">Taxon categories id</param>
        /// <param name="taxonIds">Identifiers for taxa</param>
        /// <param name="swedishImmigrationHistory">Values for swedish immigration history.</param>
        /// <param name="swedishOccurrence">Values for swedish occurrence.</param>
        /// <param name="taxonName">Taxon name</param>
        /// <param name="isValidTaxon">Limit the search only for valid or invalid taxa.</param>
        /// <param name="returnScope">Return scope - all childs or all parents.</param>
        /// <param name="revisionId">Read data within this revision only.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>DataReader after reading has been finished.</returns>
        public DataReader GetTaxaBySearchCriteria( List<Int32> taxonIds, List<Int32> taxonCategoryIds, List<Int32> swedishImmigrationHistory, List<Int32> swedishOccurrence, String taxonName,
                                                    Boolean? isValidTaxon, String returnScope, Int32? revisionId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetTaxaBySearchCriteria", true);
            commandBuilder.AddParameter(TaxonCommon.TAXONID_TABLE, taxonIds);
            commandBuilder.AddParameter(TaxonCategoryData.TAXONCATEGORY_ID_TABLE, taxonCategoryIds);
            commandBuilder.AddParameter(TaxonSpeciesFact.SWEDISH_HISTORY_TABLE, swedishImmigrationHistory);
            commandBuilder.AddParameter(TaxonSpeciesFact.SWEDISH_OCCURENCE_TABLE, swedishOccurrence);
            commandBuilder.AddParameter(TaxonNameData.TAXONNAME_PARAM, taxonName, 1);
            if (isValidTaxon.HasValue)
            {
                commandBuilder.AddParameter(TaxonNameData.IS_VALID_TAXON, isValidTaxon.Value);
            }
            if (revisionId.HasValue)
            {
                commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId.Value);
            }
            commandBuilder.AddParameter(TaxonCommon.RETURN_SCOPE, returnScope);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with all taxa that are possible parents for input taxonid
        /// </summary>
        /// <param name="taxonId">Id for taxon</param>
        /// <param name="revisionId">Read data within this revision only.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>DataReader after reading has been finished.</returns>
        public DataReader GetTaxaPossibleParents (Int32 taxonId, Int32? revisionId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetTaxaPossibleParents");
            commandBuilder.AddParameter(TaxonCommon.TAXON_ID, taxonId);
            if (revisionId.HasValue)
            {
                commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId.Value);
            }
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }


        /// <summary>
        /// Get DataReader with information about a taxon.
        /// </summary>
        /// <param name="taxonId">Id value for taxon.</param>
        /// <param name="revisionId">Read data within this revision only.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonById(Int32 taxonId, Int32? revisionId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetTaxonById");
            commandBuilder.AddParameter(TaxonCommon.ID, taxonId);
            if (revisionId.HasValue)
            {
                commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId.Value);
            }
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return this.GetRow(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about a taxon.
        /// </summary>
        /// <param name="GUID">GUID for taxon.</param>
        /// <param name="revisionId">Read data within this revision only.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonByGUID(String GUID, Int32? revisionId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetTaxonByGUID");
            commandBuilder.AddParameter(TaxonCommon.GUID, GUID);
            if (revisionId.HasValue)
            {
                commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId.Value);
            }
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetRow(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about taxon name.
        /// </summary>
        /// <param name="guid">GUID.</param>
        /// <param name="revisionId">Revision id.</param>
        /// <param name="localeId">Locale id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonNameByGuid(String guid,
                                             Int32? revisionId,
                                             Int32 localeId)
        {
            Int32 version;
            SqlCommandBuilder commandBuilder;
            String[] split;

            split = guid.Split(new [] { ':' });
            if (split.Length == 5)
            {
                // Normal GUID without version information.
                commandBuilder = new SqlCommandBuilder("GetTaxonNameByGUID");
                commandBuilder.AddParameter(TaxonCommon.GUID, guid);
                if (revisionId.HasValue)
                {
                    commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId.Value);
                }
                commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            }
            else
            {
                // GUID with version information.
                if (Int32.TryParse(split[5], out version))
                {
                    commandBuilder = new SqlCommandBuilder("GetTaxonNameByVersion");
                    commandBuilder.AddParameter(TaxonNameData.VERSION, version);
                    commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
                }
                else
                {
                    throw new ArgumentException("Wrong taxon name GUID format: " + guid);
                }
            }
            return GetRow(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about lump split event.
        /// </summary>
        /// <param name="GUID">GUID.</param>
        /// <param name="localeId">Locale id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetLumpSplitEventByGUID(String GUID, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetLumpSplitEventByGUID");
            commandBuilder.AddParameter(TaxonCommon.GUID, GUID);
   
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetRow(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about revision.
        /// </summary>
        /// <param name="GUID">GUID.</param>
        /// <param name="localeId">Locale id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetRevisionByGUID(String GUID, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetRevisionByGUID");
            commandBuilder.AddParameter(TaxonCommon.GUID, GUID);
           
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetRow(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about lump split event.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="localeId">Locale id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetLumpSplitEventById(int id, int localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetLumpSplitEventById");
            commandBuilder.AddParameter(LumpSplitEventData.ID, id);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetRow(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about taxon properties.
        /// </summary>
        /// <param name="taxonPropertiesId">Taxon properties id.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonPropertiesById(int taxonPropertiesId, int localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetTaxonPropertiesById");
            commandBuilder.AddParameter(TaxonCommon.ID, taxonPropertiesId);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetRow(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about taxon properties.
        /// </summary>
        /// <param name="taxonId">Id value for taxon.</param>
        /// <param name="revisionId">Read data within this revision only.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonPropertiesByTaxonId(int taxonId, Int32? revisionId, int localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetTaxonPropertiesByTaxonId");
            commandBuilder.AddParameter(TaxonCommon.ID, taxonId);
            if (revisionId.HasValue)
            {
                commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId.Value);
            }
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return this.GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about
        /// taxon revision event types.
        /// </summary>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonRevisionEventTypes(Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetRevisionEventTypes");
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about taxon revision states.
        /// </summary>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonRevisionStates(Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetRevisionStates");
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about all taxon categories.
        /// </summary>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonCategories(Int32 localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetTaxonCategories");
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about all taxon categories 
        /// related to a taxon.
        /// </summary>
        /// <param name="parentTaxonId"></param>
        /// <param name="taxonId"></param>
        /// <param name="localeId">Language id.</param>
        /// <param name="isPublished"></param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonCategoriesForTaxonInTree(Int32 parentTaxonId,Int32 taxonId, Int32 localeId, bool isPublished)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetTaxonCategoriesForTaxonTreeNode");
            commandBuilder.AddParameter(TaxonData.PARENT_TAXON_ID, parentTaxonId);
            commandBuilder.AddParameter(TaxonData.CHILD_TAXON_ID, taxonId);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            commandBuilder.AddParameter(TaxonCommon.IS_PUBLISHED, isPublished);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about lumpsplit events for a specific taxon.
        /// Data is selected for specific language.
        /// </summary>
        /// <param name="id">TaxonId.</param>
        /// <param name="revisionId">If not null - read all data, if null - read published data.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetLumpSplitEventsByTaxon(int id, Int32? revisionId, int localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetLumpSplitEventsByTaxon");
            commandBuilder.AddParameter(TaxonCommon.ID, id);
            if (revisionId.HasValue)
            {
                commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId.Value);
            }
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);            
        }

        /// <summary>
        /// Get DataReader with information about lumpsplit events for a specific taxon.
        /// Data is selected for specific language.
        /// </summary>
        /// <param name="id">TaxonId.</param>
        /// <param name="revisionId">If not null - read all data, if null - read published data.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetLumpSplitEventsByReplacedTaxon(int id, Int32? revisionId, int localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetLumpSplitEventsByReplacedTaxon");
            commandBuilder.AddParameter(TaxonCommon.ID, id);
            if (revisionId.HasValue)
            {
                commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId.Value);
            }
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about all taxon name categories.
        /// </summary>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonNameCategories(Int32 localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetTaxonNameCategories");
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about taxon category
        /// for specified taxon.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonCategoryByTaxonPropertiesId(Int32 id,
                                                              Int32 localeId)
        {
            var commandBuilder = new SqlCommandBuilder("GetTaxonCategoryByTaxonPropertiesId");
            commandBuilder.AddParameter(TaxonCommon.ID, id);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return this.GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about taxon changes.
        /// </summary>
        /// <param name="rootTaxonId">The root taxon id. NULL if all changes should be returned.</param>
        /// <param name="dateFrom">Return changes from and including this date.</param>
        /// <param name="dateTo">Return changes to and including this date.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonChange(Int32? rootTaxonId, DateTime dateFrom, DateTime dateTo)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetTaxonChange");
            if (rootTaxonId.HasValue)
            {
                commandBuilder.AddParameter(TaxonChange.TAXON_ID, rootTaxonId.Value);
            }
            commandBuilder.AddParameter(TaxonChange.DATE_FROM, dateFrom);
            commandBuilder.AddParameter(TaxonChange.DATE_TO, dateTo);
            return GetReader(commandBuilder);
        }


        /// <summary>
        /// Gets a datareader with name for a specific taxonName id
        /// </summary>
        /// <param name="taxonNameId">The taxon name id </param>
        /// <param name="revisionId">Revision id.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonNameById(Int32 taxonNameId,
                                           Int32? revisionId,
                                           Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetTaxonNameById");
            commandBuilder.AddParameter(TaxonCommon.ID, taxonNameId);
            if (revisionId.HasValue)
            {
                commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId.Value);
            }
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Gets a datareader with names for taxon that matches the search criterias.
        /// </summary>
        /// <param name="nameSearchString">Name search string.</param>
        /// <param name="stringCompareOperator">Search method to use in stored procedure</param>
        /// <param name="authorSearchString">Author search string.</param>
        /// <param name="taxonIds">Restrict search to taxon trees with these root taxa.</param>
        /// <param name="taxonNameStatusId">Search for taxon names with this name usage.</param>
        /// <param name="taxonNameCategoryId">Search for taxon names with this name category.</param>
        /// <param name="isRecommended">Search for recommended (true) or NOT recommended (false) taxonnames.</param>
        /// <param name="isOriginal">Search for original names (true) or NOT original (false) taxonnames.</param>
        /// <param name="isOkForObsSystems">Search for taxonnames that are ok or NOT ok for observations systems.</param>
        /// <param name="isUnique">Search for unique (true) or NOT unique (false) taxonnames.</param>
        /// <param name="isValidTaxon">Search only for taxonnames to valid taxa.</param>
        /// <param name="isValidTaxonName">Search for taxonnames w/ valid date.</param>
        /// <param name="isAuthorIncludedInNameSearchString">Author is included in param nameSearchString (true).</param>
        /// <param name="modifiedDateStart">Date interval start for changed taxa</param>
        /// <param name="modifiedDateEnd">Date interval end for changed taxa</param>
        /// <param name="revisionId">Search for taxonnames within a specific revision only.</param>
        /// <param name="localeId">Specifies the users language.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonNamesBySearchCriteria(String nameSearchString,
                                                        String stringCompareOperator, 
                                                        String authorSearchString,
                                                        List<Int32> taxonIds,
                                                        Int32? taxonNameStatusId,
                                                        Int32? taxonNameCategoryId,
                                                        Boolean? isRecommended,
                                                        Boolean? isOriginal,
                                                        Boolean? isOkForObsSystems,
                                                        Boolean? isUnique,
                                                        Boolean? isValidTaxon,
                                                        Boolean? isValidTaxonName,
                                                        Boolean isAuthorIncludedInNameSearchString,
                                                        DateTime? modifiedDateStart,
                                                        DateTime? modifiedDateEnd,
                                                        Int32? revisionId,
                                                        Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;

            CommandTimeout = 300;
            commandBuilder = new SqlCommandBuilder("GetTaxonNamesBySearchCriteria", true);
            commandBuilder.AddParameter(TaxonNameData.TAXONNAME_PARAM, nameSearchString, 1);
            commandBuilder.AddParameter(TaxonNameData.COMPARE_OPERATOR, stringCompareOperator);
            commandBuilder.AddParameter(TaxonNameData.AUTHOR, authorSearchString, 1);
            if (taxonIds.IsNotNull())
            {
                commandBuilder.AddParameter(TaxonNameData.TAXON_ID_TABLE, taxonIds);
            }
            if (taxonNameStatusId.HasValue)
            {
                commandBuilder.AddParameter(TaxonNameData.NAMEUSAGE, taxonNameStatusId.Value);
            }
            if (taxonNameCategoryId.HasValue)
            {
                commandBuilder.AddParameter(TaxonNameData.NAMECATEGORY, taxonNameCategoryId.Value);
            }
            if (isRecommended.HasValue)
            {
                commandBuilder.AddParameter(TaxonNameData.IS_RECOMMENDED, isRecommended.Value);
            }
            if (isOriginal.HasValue)
            {
                commandBuilder.AddParameter(TaxonNameData.IS_ORIGINAL, isOriginal.Value);
            }
            if (isOkForObsSystems.HasValue)
            {
                commandBuilder.AddParameter(TaxonNameData.IS_OK_FOR_OBS_SYSTEMS, isOkForObsSystems.Value);
            }
            if (isUnique.HasValue)
            {
                commandBuilder.AddParameter(TaxonNameData.IS_UNIQUE, isUnique.Value);
            }
            if (isValidTaxon.HasValue)
            {
                commandBuilder.AddParameter(TaxonNameData.IS_VALID_TAXON, isValidTaxon.Value);
            }
            if (isValidTaxonName.HasValue)
            {
                commandBuilder.AddParameter(TaxonNameData.IS_VALID_TAXONNAME, isValidTaxonName.Value);
            }
            commandBuilder.AddParameter(TaxonNameData.IS_AUTHOR_INCLUDED_IN_NAME, isAuthorIncludedInNameSearchString);
            if (modifiedDateStart.HasValue)
            {
                commandBuilder.AddParameter(TaxonNameData.MODIFIED_DATE_START, modifiedDateStart.Value);
            }
            if (modifiedDateEnd.HasValue)
            {
                commandBuilder.AddParameter(TaxonNameData.MODIFIED_DATE_END, modifiedDateEnd.Value);
            }
            if (revisionId.HasValue)
            {
                commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId.Value);
            }
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);

            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Gets a datareader with information about all published taxon trees.
        /// </summary>
        /// <returns>
        /// Returns an open DataReader.
        /// Remember to close the DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonTrees()
        {
            // 2013-01-14 Gunnar
            this.CommandTimeout = 300;
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetTaxonTrees");
            return GetReader(commandBuilder, CommandBehavior.Default);
        }

        /// <summary>
        /// Gets a datareader with names for a specific taxon id
        /// </summary>
        /// <param name="taxonId">The taxon id </param>
        /// <param name="revisionId">Read data within this revision only.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonNamesByTaxonId(Int32 taxonId, Int32? revisionId, Int32 localeId)
        {
            var commandBuilder = new SqlCommandBuilder("GetTaxonNamesByTaxonId");
            commandBuilder.AddParameter(TaxonCommon.TAXON_ID, taxonId);
            if (revisionId.HasValue)
            {
                commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId.Value);
            }
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return this.GetReader(commandBuilder);
        }

        /// <summary>
        /// Gets a datareader with names for a specific taxon ids.
        /// </summary>
        /// <param name="taxonIds">The taxon ids.</param>
        /// <param name="revisionId">Read data within this revision only.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonNamesByTaxonIds(List<Int32> taxonIds,
                                                  Int32? revisionId,
                                                  Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;
            
            commandBuilder = new SqlCommandBuilder("GetTaxonNamesByTaxonIds", true);
            commandBuilder.AddParameter(TaxonCommon.ID_TABLE, taxonIds);
            if (revisionId.HasValue)
            {
                commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId.Value);
            }
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about all taxon name usages.
        /// </summary>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonNameUsages(Int32 localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetTaxonNameUsages");
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about all taxon name usages.
        /// </summary>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonNameUsagesNew(Int32 localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetTaxonNameUsagesNew");
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about a taxon tree.
        /// </summary>
        /// <param name="taxonIdAnchor">taxonIdAnchor - the anchor node in the taxon tree</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonTreeFromNode(Int32 taxonIdAnchor, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetTaxonTreeFromNode");
            commandBuilder.AddParameter(TaxonCommon.TAXON_ID_ANCHOR, taxonIdAnchor);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        
        /// <summary>
        /// Determines whether the taxon is in a checked out revision.
        /// </summary>
        /// <param name="taxonId">The taxon id.</param>
        /// <returns>
        ///   <c>true</c> if the taxon is in a checked out revision otherwise, <c>false</c>.
        /// </returns>
        public Boolean IsTaxonInRevision (Int32 taxonId)
        {
            SqlCommandBuilder commandBuilder;
            commandBuilder = new SqlCommandBuilder("IsTaxonInRevision ");
            commandBuilder.AddParameter(TaxonCommon.TAXON_ID, taxonId);
            return (0 < ExecuteScalar(commandBuilder));
        }


        /// <summary>
        /// Get DataReader with all revisions that matches the search criteria.
        /// </summary>
        /// <param name="taxonIds">Identifiers for taxa</param>
        /// <param name="revisionStateIds">Identifiers for eventstate</param>
        /// <param name="localeId"></param>
        /// <returns></returns>
        public DataReader GetRevisionsBySearchCriteria(List<Int32> taxonIds,
                                                       List<Int32> revisionStateIds,
                                                       Int32 localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetRevisionsBySearchCriteria", true);
            commandBuilder.AddParameter(TaxonCommon.TAXONID_TABLE, taxonIds);
            commandBuilder.AddParameter(RevisionData.REVISIONSTATEID_TABLE, revisionStateIds);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }


        /// <summary>
        /// Get DataReader with all revisions that affected a taxon or its childtaxa.
        /// </summary>
        /// <param name="taxonId">Id for taxon</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>DataReader after reading has been finished.</returns>
        public DataReader GetRevisionsByTaxon(Int32 taxonId, Int32 localeId)
        {
            CommandTimeout = 300;
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetRevisionsByTaxon");
            commandBuilder.AddParameter(TaxonCommon.TAXON_ID, taxonId);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }


        /// <summary>
        /// Check in a revision.
        /// </summary>
        /// <param name="revisionId">The revision id.</param>
        public void RevisionCheckIn(Int32 revisionId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("RevisionCheckIn");
            commandBuilder.AddParameter(RevisionData.REVISIONID, revisionId);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Internal sorting of all taxa w/ same parent taxon.
        /// </summary>
        /// <param name="taxonIdParent">Id of the parent taxon.</param>
        /// <param name="taxaIdChildList">Sorted list of taxa ids.</param>
        /// <param name="personName">Name of user doing the sort.</param>
        /// <param name="createdBy">UserId of user doing the sort.</param>
        /// <param name="revisionEventId">The revision event id.</param>
        public void SetTaxonTreeSortOrder(Int32 taxonIdParent, List<Int32> taxaIdChildList, String personName, Int32 createdBy, Int32 revisionEventId)
        {
            // Use SqlCommandBuilder with SqlParameter
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("SetTaxonTreeSortOrder", true);
            commandBuilder.AddParameter(TaxonTreeData.PARENT_TAXON_ID, taxonIdParent);
            commandBuilder.AddParameter(TaxonCommon.ID_TABLE, taxaIdChildList);
            commandBuilder.AddParameter(TaxonData.PERSON_NAME, personName);
            commandBuilder.AddParameter(TaxonCommon.CREATED_BY, createdBy);
            commandBuilder.AddParameter(TaxonCommon.REVISON_EVENT_ID, revisionEventId);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Undo all events in a revision.
        /// </summary>
        /// <param name="revisionId">The revision id.</param>
        public void UndoRevision(int revisionId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("UndoRevision");
            commandBuilder.AddParameter("revisionId", revisionId);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Undo specified revision event.
        /// </summary>
        /// <param name="revisionEventId">The revision event id.</param>
        public void UndoRevisionEvent(int revisionEventId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("UndoRevisionEvent");
            commandBuilder.AddParameter(RevisionEventData.ID, revisionEventId);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Create taxon relation.
        /// </summary>
        /// <param name="taxonId">Taxon id.</param>
        /// <param name="parentTaxonId">Parent taxon id.</param>
        /// <param name="personName">Person name.</param>
        /// <param name="createdBy">Created by.</param>
        /// <param name="sortOrder">Sort order.</param>
        /// <param name="revisionEventId">Revision event id.</param>
        /// <param name="isMainRelation">Is main relation.</param>
        /// <returns>Returns id for the created taxon relation.</returns>
        public int CreateTaxonRelation(int taxonId,
                                       int parentTaxonId,
                                       string personName,
                                       int createdBy,
                                       int sortOrder,
                                       int revisionEventId,
                                       bool isMainRelation)
        {
            var commandBuilder = new SqlCommandBuilder("InsertTaxonRelation", true);

            commandBuilder.AddParameter("TaxonId", taxonId);
            commandBuilder.AddParameter("ParentTaxonId", parentTaxonId);
            commandBuilder.AddParameter("PersonName", personName);
            commandBuilder.AddParameter("SortOrder", sortOrder);
            commandBuilder.AddParameter("CreatedBy", createdBy);
            commandBuilder.AddParameter("RevisionEventId", revisionEventId);
            commandBuilder.AddParameter("IsMainRelation", isMainRelation);

            lock (this)
            {
                CheckTransaction();

                // Returns TaxonId for the created taxon
                return ExecuteScalar(commandBuilder);
            }
        }

        /// <summary>
        /// Update taxon relation.
        /// </summary>
        /// <param name="taxonRelationId">Taxon relation id.</param>
        /// <param name="taxonId">Taxon id.</param>
        /// <param name="parentTaxonId">Parent taxon id.</param>
        /// <param name="sortOrder">Sort order.</param>
        /// <param name="revisionEventId">Revision event id.</param>
        /// <param name="changedInRevisionEventId">Changed in revision event id.</param>
        /// <param name="validFromDate">Valid from date.</param>
        /// <param name="validToDate">Valid to date.</param>
        /// <param name="isPublished">Is published.</param>
        /// <param name="modifiedBy">Modified by.</param>
        /// <returns>Returns id for the updated taxon relation.</returns>
        public int UpdateTaxonRelation(int taxonRelationId,
                                       int taxonId,
                                       int parentTaxonId, 
                                       int sortOrder,
                                       int revisionEventId,
                                       int changedInRevisionEventId, 
                                       DateTime validFromDate,
                                       DateTime? validToDate,
                                       bool isPublished,
                                       int modifiedBy)
        {
            var commandBuilder = new SqlCommandBuilder("UpdateTaxonRelation", true);

            commandBuilder.AddParameter("TaxonRelationId", taxonRelationId);
            commandBuilder.AddParameter("TaxonId", taxonId);
            commandBuilder.AddParameter("ParentTaxonId", parentTaxonId);
            commandBuilder.AddParameter("SortOrder", sortOrder);
            commandBuilder.AddParameter("RevisionEventId", revisionEventId);
            commandBuilder.AddParameter("ChangedInRevisionEventId", changedInRevisionEventId);
            commandBuilder.AddParameter("ValidFromdate", validFromDate);
            commandBuilder.AddParameter("ValidToDate", validToDate);
            commandBuilder.AddParameter("IsPublished", isPublished);
            commandBuilder.AddParameter("ModifiedBy", modifiedBy);

            lock (this)
            {
                CheckTransaction();

                // Returns Id for the updated taxon relation
                return ExecuteScalar(commandBuilder);
            }
        }

        /// <summary>
        /// Get Taxon Relation By Id. 
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        public DataReader GetTaxonRelationById(int id)
        {
            var commandBuilder = new SqlCommandBuilder("GetTaxonRelationById");
            commandBuilder.AddParameter("Id", id);
            return this.GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about taxon relations.
        /// </summary>
        /// <param name="taxonRevisionId">Taxon revision id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonRelations(Int32? taxonRevisionId)
        {
            // 2013-02-06 Gunnar
            this.CommandTimeout = 300;
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter
            commandBuilder = new SqlCommandBuilder("GetTaxonRelations");
            if (taxonRevisionId.HasValue)
            {
                commandBuilder.AddParameter(TaxonCommon.REVISON_ID, taxonRevisionId.Value);
            }
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about taxon relations
        /// that are related to specified taxa.
        /// This procedure assumes that the user is in an revision.
        /// </summary>
        /// <param name="taxonIds">Taxon ids.</param>
        /// <param name="taxonRelationSearchScope">Taxon relation search scope.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonRelationsByTaxa(List<Int32> taxonIds,
                                                  TaxonRelationSearchScope taxonRelationSearchScope)
        {
            // 2013-01-14 Gunnar
            this.CommandTimeout = 300;
            SqlCommandBuilder commandBuilder;

            // Use SqlCommandBuilder with SqlParameter
            commandBuilder = new SqlCommandBuilder("GetTaxonRelationsByTaxa", true);
            commandBuilder.AddParameter(TaxonCommon.ID_TABLE, taxonIds);
            commandBuilder.AddParameter(TaxonRelationData.SEARCH_SCOPE, taxonRelationSearchScope.ToString());
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about taxon statistics.
        /// </summary>
        /// <param name="taxonId">The taxon id.</param>
        /// <param name="revisionId">Read data within this revision only.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonStatistics(Int32 taxonId, Int32? revisionId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetTaxonStatistics");
            commandBuilder.AddParameter(TaxonCommon.TAXON_ID, taxonId);
            if (revisionId.HasValue)
            {
                commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId.Value);
            }
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about taxon info quality in DynTaxa.
        /// </summary>
        /// <param name="taxonId">The taxon id.</param>
        /// <param name="revisionId">Read data within this revision only.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonQualitySummary(Int32 taxonId, Int32? revisionId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetTaxonQualitySummary");
            commandBuilder.AddParameter(TaxonCommon.TAXON_ID, taxonId);
            if (revisionId.HasValue)
            {
                commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId.Value);
            }
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Updates a taxon name in database.
        /// </summary>
        /// <param name="id">The taxon name id.</param>
        /// <param name="modifiedBy">UserId for user that modifies record.</param>
        /// <param name="changedInRevisionEventId">The revision event id that invalidates this record.</param>
        /// <returns>Id for the updated taxon name </returns>
        public Int32 UpdateTaxonName(Int32 id, Int32 modifiedBy, Int32? changedInRevisionEventId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("UpdateTaxonName");
            commandBuilder.AddParameter(TaxonCommon.ID, id);
            commandBuilder.AddParameter(TaxonCommon.MODIFIED_BY, modifiedBy);
            if (changedInRevisionEventId.HasValue)
            {
                commandBuilder.AddParameter(TaxonCommon.CHANGED_IN_REVISON_EVENT_ID, changedInRevisionEventId.Value);
            }

            lock (this)
            {
                CheckTransaction();

                // Returns Id for the updated taxon name 
                return ExecuteScalar(commandBuilder);
            }
        }

        /// <summary>
        /// Create lump split event.
        /// </summary>
        /// <param name="changedInRevisionEvent">Changed in revision event.</param>
        /// <param name="createdBy">Created by.</param>
        /// <param name="createdDate">Created date.</param>
        /// <param name="descriptionString">Description string.</param>
        /// <param name="eventTimeStamp">Event time stamp.</param>
        /// <param name="eventType">Event type.</param>
        /// <param name="personName">Person name.</param>
        /// <param name="revisionEvent">Revision event.</param>
        /// <param name="taxonIdAfter">Taxon id after.</param>
        /// <param name="taxonIdBefore">Taxon id before.</param>
        /// <param name="localeId">Locale id.</param>
        /// <returns>An integer. ???</returns>
        public int CreateLumpSplitEvent(Int32? changedInRevisionEvent,
                                       WebUser createdBy,
                                       DateTime createdDate,
                                       string descriptionString,
                                       DateTime eventTimeStamp,
                                       int eventType,
                                       string personName,
                                       Int32? revisionEvent,
                                       int taxonIdAfter,
                                       int taxonIdBefore,
                                       int localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("InsertLumpSplitEvent", true);
            if (changedInRevisionEvent.HasValue)
            {
                commandBuilder.AddParameter(LumpSplitEventData.CHANGEDINREVISIONEVENTID, changedInRevisionEvent.Value);
            }

            if (createdBy != null)
            {
                commandBuilder.AddParameter(LumpSplitEventData.CREATEDBY, createdBy.Id);
            }

            commandBuilder.AddParameter(LumpSplitEventData.CREATEDDATE, createdDate);
            commandBuilder.AddParameter(LumpSplitEventData.DESCRIPTIONSTRING, descriptionString);
            commandBuilder.AddParameter(LumpSplitEventData.EVENTTYPE, eventType);
            commandBuilder.AddParameter(LumpSplitEventData.ISPUBLISHED, false);
            //// commandBuilder.AddParameter(LumpSplitEventData.PERSONNAME, );

            if (revisionEvent.HasValue)
            {
                commandBuilder.AddParameter(LumpSplitEventData.REVISIONEVENTID, revisionEvent.Value);
            }

            commandBuilder.AddParameter(LumpSplitEventData.TAXONIDAFTER, taxonIdAfter);
            commandBuilder.AddParameter(LumpSplitEventData.TAXONIDBEFORE, taxonIdBefore);

            commandBuilder.AddParameter(LumpSplitEventData.VALIDFROMDATE, DateTime.Now);
            commandBuilder.AddParameter(LumpSplitEventData.VALIDTODATE, "2111-11-11");
            commandBuilder.AddParameter(LumpSplitEventData.EVENTTIMESTAMP, eventTimeStamp);

            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);

            lock (this)
            {
                CheckTransaction();
                return ExecuteScalar(commandBuilder);
            }
        }

        /// <summary>
        /// Get DataReader with information about a taxon.
        /// This method should only be used after CreateTaxon.
        /// </summary>
        /// <param name="taxonId">Id value for taxon.</param>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetTaxonByIdAfterCreate(Int32 taxonId, Int32 localeId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("GetTaxonByIdAfterCreate");
            commandBuilder.AddParameter(TaxonCommon.ID, taxonId);
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return this.GetRow(commandBuilder);
        }

        /// <summary>
        /// Update Taxon with SpeciesFact data
        /// </summary>
        /// <param name="taxonId">Taxon id.</param>
        /// <param name="speciesFactValue">Value of the fact.</param>
        /// <param name="whichFact">Which fact.</param>
        public void UpdateTaxonSpeciesFact(int taxonId,
                                           int speciesFactValue,
                                           String whichFact)
        {
            var commandBuilder = new SqlCommandBuilder("UpdateTaxonSpeciesFact", true);

            commandBuilder.AddParameter(TaxonCommon.ID, taxonId);
            commandBuilder.AddParameter("speciesFactValue", speciesFactValue);
            commandBuilder.AddParameter("whichFact", whichFact);
            ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Update Taxon w/ IsSwedish
        /// </summary>
        public void UpdateTaxonIsSwedish()
        {
            var commandBuilder = new SqlCommandBuilder("UpdateTaxonIsSwedish");
            ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Update Taxon w/ DyntaxaQuality
        /// </summary>
        public void UpdateTaxonDyntaxaQuality()
        {
            // This step might take some time -> set db-transaction timeout to 5 mins.
            this.CommandTimeout = 300;
            var commandBuilder = new SqlCommandBuilder("UpdateTaxonDyntaxaQuality");
            ExecuteCommand(commandBuilder);
        }

        /// <summary>
        /// Gets a datareader with dyntaxa revision species fact.
        /// </summary>
        /// <param name="factorId">Factor id.</param>
        /// <param name="taxonId">Taxon id.</param>
        /// <param name="revisionId">Revision id.</param>        
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetDyntaxaRevisionSpeciesFact(
            Int32 factorId,
            Int32 taxonId,
            Int32 revisionId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetDyntaxaRevisionSpeciesFact");
            commandBuilder.AddParameter(DyntaxaRevisionSpeciesFact.FACTOR_ID, factorId);
            commandBuilder.AddParameter(TaxonCommon.TAXON_ID, taxonId);
            commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId);            
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxonRevisionId"></param>
        /// <returns></returns>
        public DataReader GetAllDyntaxaRevisionSpeciesFacts(int taxonRevisionId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetAllDyntaxaRevisionSpeciesFacts");
            commandBuilder.AddParameter(TaxonCommon.REVISON_ID, taxonRevisionId);
            return GetReader(commandBuilder);
        }

        public DataReader GetDyntaxaRevisionSpeciesFactById(int id)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetDyntaxaRevisionSpeciesFactById");
            commandBuilder.AddParameter(TaxonCommon.ID, id);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Inserts a dyntaxa revision species fact into database.
        /// </summary>
        /// <param name="factorId">The factor identifier.</param>
        /// <param name="taxonId">The taxon id.</param>
        /// <param name="revisionId">The revision identifier.</param>
        /// <param name="statusId">The status identifier.</param>
        /// <param name="qualityId">The quality identifier.</param>
        /// <param name="description">The description.</param>
        /// <param name="referenceId">The reference identifier.</param>
        /// <param name="createdDate">Date item was created.</param>
        /// <param name="createdBy">UserId for user that creates record.</param>
        /// <param name="revisionEventId">The revision event id.</param>
        /// <param name="speciesFactExists">Specifies wether the species fact exists in Artfakta database or not.</param>
        /// <param name="originalStatusId">The original status identifier.</param>
        /// <param name="originalQualityId">The original quality identifier.</param>
        /// <param name="originalReferenceId">The original reference identifier.</param>
        /// <param name="originalDescription">The original description.</param>
        /// <returns>
        /// Id for the created dyntaxa revision species fact.
        /// </returns>
        public Int32 CreateDyntaxaRevisionSpeciesFact(Int32 factorId, Int32 taxonId, Int32 revisionId, Int32? statusId, Int32? qualityId, String description, Int32? referenceId,
                                     DateTime createdDate, Int32 createdBy, Int32? revisionEventId, Boolean speciesFactExists, Int32? originalStatusId, Int32? originalQualityId, Int32? originalReferenceId, String originalDescription)
        {            
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("InsertDyntaxaRevisionSpeciesFact");
            commandBuilder.AddParameter(DyntaxaRevisionSpeciesFact.FACTOR_ID, factorId);   
            commandBuilder.AddParameter(TaxonCommon.TAXON_ID, taxonId);
            commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId);
            commandBuilder.AddParameter(DyntaxaRevisionSpeciesFact.STATUS_ID, statusId);
            commandBuilder.AddParameter(DyntaxaRevisionSpeciesFact.QUALITY_ID, qualityId);            
            commandBuilder.AddParameter(DyntaxaRevisionSpeciesFact.DESCRIPTION, description);
            commandBuilder.AddParameter(DyntaxaRevisionSpeciesFact.REFERENCE_ID, referenceId);

            // if CreatedDate is DateTime.MinValue - don't send it to the st.proc
            // will be set to GETDATE() 
            if (!createdDate.Equals(DateTime.MinValue))
            {
                commandBuilder.AddParameter(TaxonCommon.CREATED_DATE, createdDate);
            }
            commandBuilder.AddParameter(TaxonCommon.CREATED_BY, createdBy);                        
            if (revisionEventId.HasValue)
            {
                commandBuilder.AddParameter(TaxonCommon.REVISON_EVENT_ID, revisionEventId.Value);
            }
            commandBuilder.AddParameter(DyntaxaRevisionSpeciesFact.SPECIESFACT_EXISTS, speciesFactExists);
            commandBuilder.AddParameter(DyntaxaRevisionSpeciesFact.ORIGINAL_STATUS_ID, originalStatusId);
            commandBuilder.AddParameter(DyntaxaRevisionSpeciesFact.ORIGINAL_QUALITY_ID, originalQualityId);
            commandBuilder.AddParameter(DyntaxaRevisionSpeciesFact.ORIGINAL_REFERENCE_ID, originalReferenceId);
            commandBuilder.AddParameter(DyntaxaRevisionSpeciesFact.ORIGINAL_DESCRIPTION, originalDescription);

            lock (this)
            {
                CheckTransaction();

                // Returns Id for the created dyntaxa revision species fact.
                return ExecuteScalar(commandBuilder);
            }
        }

        /// <summary>
        /// Inserta a the dyntaxa revision reference relation into database.
        /// </summary>
        /// <param name="revisionId">The revision identifier.</param>
        /// <param name="action">The action.</param>        
        /// <param name="relatedObjectGuid">The related object unique identifier.</param>
        /// <param name="referenceId">The Reference DB reference identifier.</param>
        /// <param name="referenceType">Type of the reference.</param>
        /// <param name="oldReferenceType">Old type of the reference.</param>
        /// <param name="referenceRelationId">The Reference DB reference relation identifier.</param>
        /// <param name="createdDate">The created date.</param>
        /// <param name="createdBy">The created by.</param>
        /// <param name="revisionEventId">The revision event identifier.</param>
        /// <returns>
        /// Id for the created dyntaxa revision reference relation.
        /// </returns>        
        public Int32 CreateDyntaxaRevisionReferenceRelation(
            int revisionId,
            string action,
            string relatedObjectGuid,
            int referenceId,
            int referenceType,
            int? oldReferenceType,
            int? referenceRelationId,
            DateTime createdDate, 
            int createdBy, 
            int? revisionEventId)
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder("InsertDyntaxaRevisionReferenceRelation");
            commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId);            
            commandBuilder.AddParameter(DyntaxaRevisionReferenceRelation.ACTION, action);
            commandBuilder.AddParameter(DyntaxaRevisionReferenceRelation.RELATED_OBJECT_GUID, relatedObjectGuid);
            commandBuilder.AddParameter(DyntaxaRevisionReferenceRelation.REFERENCE_ID, referenceId);
            commandBuilder.AddParameter(DyntaxaRevisionReferenceRelation.REFERENCE_TYPE, referenceType);
            if (oldReferenceType.HasValue)
            {
                commandBuilder.AddParameter(DyntaxaRevisionReferenceRelation.OLD_REFERENCE_TYPE, oldReferenceType);
            }

            if (referenceRelationId.HasValue)
            {
                commandBuilder.AddParameter(DyntaxaRevisionReferenceRelation.REFERENCE_RELATION_ID, referenceRelationId);
            }
            // if CreatedDate is DateTime.MinValue - don't send it to the st.proc
            // will be set to GETDATE() 
            if (!createdDate.Equals(DateTime.MinValue))
            {
                commandBuilder.AddParameter(TaxonCommon.CREATED_DATE, createdDate);
            }

            commandBuilder.AddParameter(TaxonCommon.CREATED_BY, createdBy);
            if (revisionEventId.HasValue)
            {
                commandBuilder.AddParameter(TaxonCommon.REVISON_EVENT_ID, revisionEventId.Value);
            }     

            lock (this)
            {
                CheckTransaction();

                // Returns Id for the created dyntaxa revision species fact.
                return ExecuteScalar(commandBuilder);
            }
        }

        public DataReader GetDyntaxaRevisionReferenceRelationById(int id)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetDyntaxaRevisionReferenceRelationById");
            commandBuilder.AddParameter(TaxonCommon.ID, id);
            return GetReader(commandBuilder);
        }

        public DataReader GetAllDyntaxaRevisionReferenceRelations(int revisionId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetAllDyntaxaRevisionReferenceRelations");
            commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Gets a datareader with dyntaxa revision reference relation.
        /// </summary>        
        /// <param name="revisionId">Revision id.</param>
        /// <param name="relatedObjectGUID">Related object GUID.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetDyntaxaRevisionReferenceRelation(int revisionId, string relatedObjectGUID)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetDyntaxaRevisionReferenceRelation");            
            commandBuilder.AddParameter(TaxonCommon.REVISON_ID, revisionId);
            commandBuilder.AddParameter(DyntaxaRevisionReferenceRelation.RELATED_OBJECT_GUID, relatedObjectGUID);            
            return GetReader(commandBuilder);
        }        
    }
}
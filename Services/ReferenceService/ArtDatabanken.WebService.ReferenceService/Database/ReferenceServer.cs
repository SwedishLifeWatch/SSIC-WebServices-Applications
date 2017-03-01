using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ArtDatabanken.Database;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.Database;
using SqlCommandBuilder = ArtDatabanken.Database.SqlCommandBuilder;

namespace ArtDatabanken.WebService.ReferenceService.Database
{
    /// <summary>
    /// Database interface for the reference database.
    /// </summary>
    public class ReferenceServer : WebServiceDataServer
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
        /// Create a new reference in database.
        /// </summary>
        /// <param name="name">Name of reference.</param>
        /// <param name="year">Year the reference was published.</param>
        /// <param name="title">Title of the reference.</param>
        /// <param name="modifiedBy">Person inserting the reference.</param>  
        public void CreateReference(String name,
                                    Int32 year,
                                    String title,
                                    String modifiedBy)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("CreateReference");
            commandBuilder.AddParameter(ReferenceData.NAME, name);
            commandBuilder.AddParameter(ReferenceData.PERSON, modifiedBy);
            commandBuilder.AddParameter(ReferenceData.TITLE, title);
            commandBuilder.AddParameter(ReferenceData.YEAR, year);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }

        /// <summary>
        /// Creates a reference relation.
        /// </summary>
        /// <param name="relatedObjectGuid">
        /// Object GUID representing an object
        /// (taxon , taxon name, revision etc.).
        /// </param>
        /// <param name="referenceId">Id of the reference.</param>
        /// <param name="typeId">Reference type.</param>
        /// <returns>Id for the created reference relation.</returns>
        public Int32 CreateReferenceRelation(String relatedObjectGuid,
                                             Int32 referenceId,
                                             Int32 typeId)
        {
            SqlCommandBuilder commandBuilder;
                
            commandBuilder = new SqlCommandBuilder("CreateReferenceRelation");
            commandBuilder.AddParameter(ReferenceRelationData.OBJECT_GUID, relatedObjectGuid);
            commandBuilder.AddParameter(ReferenceRelationData.REFERENCEID, referenceId);
            commandBuilder.AddParameter(ReferenceRelationData.TYPE, typeId);
            lock (this)
            {
                CheckTransaction();
                return ExecuteScalar(commandBuilder);
            }
        }

        /// <summary>
        /// Delete a reference relation.
        /// </summary>
        /// <param name="referenceRelationId">The reference relation id.</param>
        public void DeleteReferenceRelation(Int32 referenceRelationId)
        {
            SqlCommandBuilder commandBuilder;
                
            commandBuilder = new SqlCommandBuilder("DeleteReferenceRelation");
            commandBuilder.AddParameter(ReferenceRelationData.REFERENCE_RELATION_ID, referenceRelationId);
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
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAFCOCNCJGKLKLENEMMBPJKPBHDGIHIGLJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAANGBBIDLDGCIPKGPEODJCLCJANOGANOOGJIAAAAAAPOLJFGIOGAHKBPIPMCDOHBNMCMKBCFDILBIBNPBACOEBFJALBCDEGNJEHKGCKNFJNPLECCAMELCFGJHOALIFBGJMCNONMNIFDHGFBEPENLGADJNOJCEGAFJADPPNGPACPNPAAKJLFIMDDAOMOGODABHAIPMIGOHKFAEFGAHFIACMPCCGILHANKCIGNOMCPBNGBCGHIKLKCPEHONAAABMNBPLFAHJOCHNDIFAFHFLJLNILLNFNINNDKAPICMFMOEEPBFHCKPOINMCPIPLJMINNHEIMEOCLFKNOLJFMPFPCLLIJMINBEAAAAAAPEFCEADLIJMNKDODPCLJDGCKKIIAEMMCAHFDNIBK";
                    break;

                case "ARTSERVICE2-1": // New production web service server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAGJNNDDDJKMNHGKOOGGOFCBLMKMKAPKKLAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAEKOHAIACKLDKEGDMCPIDOFEFBODIANKJKIAAAAAADBFCAJIGOGEAINGIEDMAOHCOJPHFJAHKBGCAHDAFNDCJOADPKAALFACMMNLAGEILOHILOCFMGAICDCDGBKIKLMECLALFIICGGEEGLGBKAMFIMCJOPHFBDDBPPODHAKEFFBFBDCFGCCMAPGCGIELAOBMKEEKLDELLCAEHOALENPAHOLEBKBJPDHCLDHNBODAFLPDJBAMFABENAEONGPEFEFOOJOMCDIIHDNEAIGDAAOJNBOIDMLEEHPBPAEEFODGLIJHPNDLFFFKGLCNOCEEGGHAEOHKJLOKHNKDMJDCFGIKECNBHDNIJDPEAHMEKGMEPFKNODIDBENKPJIHFBEAAAAAADMLPPKGLMFOGGAPCNEACFCHDBAIJLONLDDMBPBPA";
                    break;

                case "ARTSERVICE2-2": // New production web service server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPGBJALGDDHDONKEGJOECJFBANDBBPEGFAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJDCEMPHLOHDAABHHNHCCMMELFFEOOKMMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPLPBFHALMCHPLJPONJHNAHPGABKADOEJKIAAAAAALJHLCCJGKCHKCAGKBPKFDMGLMAOGIDGDECBIDPKEHFOKFJMGMHCDAFKBJBLNONDIFFGAGNMHDFDBOAELGFHFFADNPAFNBMDIBKBFFGEBGCNFGMABIJPAADPENNEDCGCHBHOBNDIIJJAOKLMMPAMADLPCDHCCLBGGNKKNHNLKGMCMPGABNKMOAFLHMMEECJBOMAFLBBOELKGBIKLJNCIFOPDBKIPHGCJLHNPADCEILEAPFEIPOICMPEKNOINGFGEPPHCMGODGEGIEKHBCPKKGHBOAIMILFLDCELNPJLBKOCAAHDEGGGDFBFBPPDOEHFLOBDNDNKFDKBGFGKLKBEAAAAAAAHNLNLHMEENODHFGIAEBLGECELFGKDLIKAHNDJDD";
                    break;

                case "LAMPETRA2-1": // Production web server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAJCEPODNLFCEAOPEJKAENKKFFOHNGCDHEAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAPNMONKCBBDJBHGHLNMMJLJGKFLIMCPNOAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPKFLCIBCLCLBOHANALHBAJDHDCOFKPFKKIAAAAAACBLIIIJCHKKACCFKCMHGGJDDIBOHHLOOJMPHADGPKFPANPOPDCGILDBPOJLABMLOJDNKGFHPJFBCONJEDKNOBOHFNFFNBDMMENCNDBBPAGCJIAANMPLFFDAPFJBEIKBLHGMNICGFPJHNONPDDNBBADHICAMMKNALCADLOAAOMBHGIMOAHIHBDLEFCJDDPMCINLKMLMOMJAONJOLNELNPNAJFFDONKHMFLKOFLDAKBHCJCJHBHNIOOOPCJAFLPAAHLMEGNOECLJEJIKIEGGNDCFFCHJCBGABJAHKIONPPBHGDPEEBLBDGBNDOLFFNGFLBNOKKOODAEEHLANCHBEAAAAAAOAIOMMOAHJMKPHDAALKNFPHFCDGBBCOBDEIOPHKG";
                    break;

                case "LAMPETRA2-2": // Production web server
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAECNHBGDNBMLGGOEILCGKDIMBCEOCDKGCAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAEKPCGAKFLFKGJDANJAFEGNCEFPHAFMFEAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAOBLEODLOFPDLLJFPHBHAHIPDAJAOHMJOKIAAAAAAFHEPNJHDPAJNENJJDLNBGBOJELBPAADPNJANPJMFHGHHIFNJIMEDMKGGADMLGAIBDKFOKDJEKPLCOFFDHGAHFHPNNIDHLHCCDFGLNBMPNPDEACOLJJNCFIONDFEKJJIKAJNLKFOOEAMBIBONGDFPMDNOIEDPGAHDEJPICFADFGBFNNLKMNKDGBLILCBCDJLAOJIIHCKHNAOGPFMCMPAPPPDPECLPLLMEIEALNACINIOHFPDGMNPDIONBGLIAFLNDIBLPLIPJKMBLNHCFBCCIKAJJBAELGNJCHNLINAJHEIAIOHPFOODHIIDFOKPHJGJOBIMJPGDMOBKJBEIJBEAAAAAAONAPCODLMFJPEKEEGNGPPCCMINODKDNLFBAHHJJP";
                    break;

                case "MONESES-DEV":
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAEAFPECBBEFEHIKEGLFFDMHECEONJDGANAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADJPBNKJJEKLGCNHMCBJIPIKMACAJHOJLAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJEJEMMMJOGAJNLKMHGILAAENGLHKLGONJIAAAAAAGFHENDHJBJGMCDJIMBGAIILILNHJDILAEPFBEBNGEPPGCMMKOJIGHJMAKIHFLLGLJBMMLJFCFGODEGEDECACMMDACOHGPPONOKNDEPHCFGBGLGHAALDLNJIGDELLOJOCMEJFINBOMPHONPPEOFKIPIDMPBMLJDBHICFIHFCFBHHJEAEDELGLDIOOKMLPEKJBLNFBCCGNECLJCNCADLEKMJOKDIPLOEBPKGCLAJJENIPIOMNPNICGKIBOFCABCDIDDJOAGMEJLNDIJONJIFLKADOACLEFLMMGFEJBIAGEDIOLBLHJBEAAAAAAEBDIKKACECDHAKGKEKKMNEBBPKMEKCIEOLMMHFAG";
                    break;

                case "MONESES-ST": // System test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAIKGEBDAPMKNOAFEKIHFGCOJNGHIAKFIAAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAGGGOFNLFLHBEEEIGMBBCAJLFEHNPEOPCAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAGONNKOKMLGKFLKPMEPGOIDPDKLAMPAPHJIAAAAAAMENMNIFGPAKAIDCNJOGCDHONLIAHNNHDLIFDJBHAJKAELKEANFPEOFLFOLALHGEANMOBJMLMMBNFJELEGJJKKPGEBIKLBDNAEECGBJJMOAIDKABEJIJJKNDNJIMHNIHNGFAANNFEMDFNOFBOCDCGFAAEGMDEAILEFCOCCFBMDLLJIHFJICAKKEPKAKLLDJMFBBPONCLNKLCNLPAOJLGAGPMHPNECGNLOBKPNHDLFIEHODLAGDEHOLGNMHMLHDKGCMMJGMDIKHLLHHIIGOCCBFLMLEKJHKBPOCLGOIPFMFKCPIMBIBEAAAAAAJOHPOBJFNPIKPEHBDMJFBMOPNFFAOEHLLBBOMPOE";
                    break;

                case "SLU003354": // 
                    connectionString = null;
                    break;

                case "SLU011837": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAALCPBDBGCIAFPEPEILEKMLLIKEOOJMJMKAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADPBNKDNBEBINNEJPONHBDFGFNJMGDCFBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAANBKBFOIKMGKFOHMLGDJIIKPMHEGMBNJKJIAAAAAAHHDFAGEPFMNALEPAOGJCOMAPEBFDBINNIBDEAKCDDODNACHNFCNBLLAHPJGNCFGACJNLGFBNNEKEOPNCKAMJINHPDJEGNPNNHAKJENJEGDGDJMGEHIJJHDFHCJCLCOMNMJPJEPPBAMMOFLGMMPABLACPPFKECJHNINLEIKFGDKAEHOHMBKHKBHLJNIGKFDIPOFLHFMINHLHNBMOCFNKLOPBKALHEPAPBNKCFAFGNAAHAJJICICMIAAOCAMJJGDHHIBBPNCKJPFEBPEKFMLCEIMPKNEPCGOCLPCDNCBEDGPJODHLOBEAAAAAAJNLPKEPEHBMLPIJNDIMPKFHCJNFPDOABLCMEEJBO";
                    break;

                case "SLU002759": // 
                    connectionString = null;
                    break;

                case "SLU002760": // 
                    connectionString = null;
                    break;

                case "SLU005126": // 
                    connectionString = null;
                    break;

                case "MARKAC8560WW7": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAOHJOEMKMDIHEDPECJFPBNGDEDBEEFDHDAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAANBLCCOIKBJHPBONGHHFGGFKBDKGCLAIBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAADKCDCBOGKDLLKFOEOCCEALAMGOEBKAEEJIAAAAAAMJFOOMDEJHJNDBOCJEEFFDNLILEHEIEHHPBNDLPAKFJEIFGLAGILBOEKAPIKMPKPNJMIJFJBHBALCGDEKIGCIOHLDEMKNCOLEKAGLAPMPJHHPBJMJJCGIGBPFEPMAHBNJBKHKIFOKFJINJLFIAJMKBDBJLILAOPAKBCBPHKPFKMMJOJCINJDHNDHFPCBJHGHFDNFODPBCDIAIAMDEMDCCNFABGCOLPNCLHAJGPODFPBDMFPKJDCGHOHPOMJINFHNOOPGHMKBEIOOMOANLMOEODPHFBLECPIKOLHEKENBBMIKEBIFBEAAAAAADNPPFLOKLPJGLAKJKBBHCLDPPHCBDCDCOBCOLADN";
                    break;

                case "SEA0105WTRD2": // 
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAABKIOIJPBLDPKPCEELADDGIHIPCLMBNMGAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAIOAFAOPGLJNKCEGCKKJFEMMAJENGNKMMAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAABAJIFBCANMCPENKNLHLFDKNDLKHNPHOGJIAAAAAAJGFLMIPAMFPINGFECFMGIPMCAANKNMJBHJJOFBFDDCHOGGEOBJFGELDOOIIHJKAGMIJAIMLGOIAFPMDCIJDIMKGMLHEIFFKDOMPKPIFFOKFIJJAFFAMNFPAKBGBIHCCCCIKNDJPIAIJBJLCCKJGCBLPNMEBHDPAINMOOMLKIAEJFCBFHNOBPAELCNPHEDDHAEKLOPKHIICHCPLCEIOKCNLCFFONGHHFGBBNEPLHLNAOEIGJLNLBJMPBDKBPBPOFEEKAJKMBDGNHMGCPANDNABOKEGAGPBJALNBNAOBMAIIJNAKBIBEAAAAAAMLPGODOCGHIIOGGHBNHEFFHEFHBBCHCHDLMIHBEB";
                    break;

                case "SLW-DEV": // Team Two Blueberries test server.
                    connectionString = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAPEAJKFJCOJJILJEMJOGHHPGGFJAANOKIAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAIELCDFEEBOOHBMKANOMMIKJEANFJLNMJAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKLLHDEIHHBJHLGEICPKNJAMKMINGELMNJIAAAAAACHMAKPGHDKBAFECDMEFKDANIFBBEPOLIMPHNPJIELDPGFMEMMIGKJMMBHPCDFEOIBBOHBGHNJKBDFHOIJJHPEACJGKEECFOEKNLLFEMCADLJBKNBMPBBMHNHHGNBJKCKINJNMMAKNJHAJGOKEAFHGLFBBGKLAGHGCLKHFNCAKAGILKFIGKHBJEKGNJCFHLFHMACKJCIJLMFAIJHEKNGJPGKPLLDDEGJMGDBLFLLJDNDCDJEJEBCFECFDMGKGBONOIEDCHKKKLPEAKOIMCJMLKGDGJHFOHBFJBKOHCNNHPAKNLLEABEAAAAAAPKJFACAHIMCDNLAOELIFLDNLKFGFPJIFNPJIHDMH";
                    break;

                default:
                    throw new ApplicationException("Unknown web server " + Environment.MachineName);
            }

            cipherString = new CipherString();
            return cipherString.DecryptText(connectionString);
        }

        /// <summary>
        /// Get DataReader with information about a reference relation.
        /// </summary>
        /// <param name="referenceRelationId">Reference relation id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetReferenceRelationById(Int32 referenceRelationId)
        {
            SqlCommandBuilder commandBuilder;
                
            commandBuilder = new SqlCommandBuilder("GetReferenceRelationsById");
            commandBuilder.AddParameter(ReferenceRelationData.ID, referenceRelationId);
            return GetRow(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about a reference relation.
        /// </summary>
        /// <param name="guid">Reference relation GUID.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetReferenceRelationsByGuid(String guid)
        {
            SqlCommandBuilder commandBuilder;
                
            commandBuilder = new SqlCommandBuilder("GetReferenceRelationsByGuid");
            commandBuilder.AddParameter(ReferenceRelationData.GUID, guid);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about reference relation types.
        /// </summary>
        /// <param name="localeId">Language id.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetReferenceRelationTypes(Int32 localeId)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetReferenceRelationTypes");
            commandBuilder.AddParameter(LocaleData.LOCALE_ID, localeId);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about all references.
        /// </summary>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetReferences()
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetReferences");
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about specified references.
        /// </summary>
        /// <param name="referenceIds">Reference ids.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetReferencesByIds(List<Int32> referenceIds)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetReferencesByIds", true);
            commandBuilder.AddParameter(ReferenceData.IDS, referenceIds);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Get DataReader with information about references
        /// that matches search criteria.
        /// </summary>
        /// <param name="whereCondition">Where condition.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public DataReader GetReferencesBySearchCriteria(String whereCondition)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("GetReferencesBySearchCriteria");
            commandBuilder.AddParameter(ReferenceData.WHERE_CONDITION, whereCondition);
            return GetReader(commandBuilder);
        }

        /// <summary>
        /// Update existing reference.
        /// </summary>
        /// <param name="id">Id of reference to update.</param>
        /// <param name="name">Name of reference.</param>
        /// <param name="year">Year the reference was published.</param>
        /// <param name="title">Title of the reference.</param>
        /// <param name="modifiedBy">Person updating the reference.</param>  
        public void UpdateReference(Int32 id,
                                    String name,
                                    Int32 year,
                                    String title,
                                    String modifiedBy)
        {
            SqlCommandBuilder commandBuilder;

            commandBuilder = new SqlCommandBuilder("UpdateReference");
            commandBuilder.AddParameter(ReferenceData.ID, id);
            commandBuilder.AddParameter(ReferenceData.NAME, name);
            commandBuilder.AddParameter(ReferenceData.PERSON, modifiedBy);
            commandBuilder.AddParameter(ReferenceData.TITLE, title);
            commandBuilder.AddParameter(ReferenceData.YEAR, year);
            lock (this)
            {
                CheckTransaction();
                ExecuteCommand(commandBuilder);
            }
        }
    }
}
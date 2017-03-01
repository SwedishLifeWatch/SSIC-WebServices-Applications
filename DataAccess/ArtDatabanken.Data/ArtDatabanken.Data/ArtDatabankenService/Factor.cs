using System;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  Enum that contains factor ids.
    /// </summary>
    public enum FactorId
    {
        /// <summary>AllRedlistFactors = 542</summary>
        AllRedlistFactors = 542,
        /// <summary>OldRedlistCategory = 544</summary>
        OldRedlistCategory = 544,
        /// <summary>CurrentRedlistFactors = 652</summary>
        CurrentRedlistFactors = 652,
        /// <summary>RedlistEvaluationFactors = 653</summary>
        RedlistEvaluationFactors = 653,
        /// <summary>RedlistFactorGroup1_GenerealFactors = 719</summary>
        RedlistFactorGroup1_GenerealFactors = 719,
        /// <summary>RedlistEvaluationProgressionStatus = 654</summary>
        RedlistEvaluationProgressionStatus = 654,
        /// <summary>Redlist_TaxonType = 655</summary>
        Redlist_TaxonType = 655,
        /// <summary>Redlist_OrganismLabel1 = 656</summary>
        Redlist_OrganismLabel1 = 656,
        /// <summary>Redlist_OrganismLabel2 = 657</summary>
        Redlist_OrganismLabel2 = 657,
        /// <summary>GlobalRedlistCategory = 978</summary>
        GlobalRedlistCategory = 978,
        /// <summary>SwedishOccurrence = 1938</summary>
        SwedishOccurrence = 1938,
        /// <summary>SwedishImmigrationHistory = 1939</summary>
        SwedishHistory = 1939,
        /// <summary>Number of Swedish Species = 1991</summary>
        NumberOfSwedishSpecies = 1991,
        /// <summary>Information Quality in Dyntaxa = 2115</summary>
        QualityInDyntaxa = 2115,
        /// <summary>Exclude From Reporting System = 1974</summary>
        ExcludeFromReportingSystem = 1974,
        /// <summary>BannedForReporting = 2116</summary>
        BanndedForReporting = 2116,
        /// <summary>Redlist_EvaluatorsNotebook = 660</summary>
        Redlist_EvaluatorsNotebook = 660,
        /// <summary>LandscapeFactors = 661</summary>
        LandscapeFactors = 661,
        /// <summary>LandscapeFactor_Forest = 662</summary>
        LandscapeFactor_Forest = 662,
        /// <summary>LandscapeFactor_Agricultural = 663</summary>
        LandscapeFactor_Agricultural = 663,
        /// <summary>LandscapeFactor_Urban = 664</summary>
        LandscapeFactor_Urban = 664,
        /// <summary>LandscapeFactor_Alpin = 665</summary>
        LandscapeFactor_Alpin = 665,
        /// <summary>LandscapeFactor_Wetland = 666</summary>
        LandscapeFactor_Wetland = 666,
        /// <summary>LandscapeFactor_FreshWater = 667</summary>
        LandscapeFactor_FreshWater = 667,
        /// <summary>LandscapeFactor_Coast = 668</summary>
        LandscapeFactor_Coast = 668,
        /// <summary>LandscapeFactor_Sea = 669</summary>
        LandscapeFactor_Sea = 669,
        /// <summary>LandscapeFactor_BrackishWater = 1984</summary>
        LandscapeFactor_BrackishWater = 1984,
        /// <summary>Livsform factor group, factor = 1859</summary>
        LifeFormFactors = 1859,
        /// <summary>GenerationTime = 671</summary>
        GenerationTime = 671,
        /// <summary>RedlistFactorGroup2_PopulationReduction = 670</summary>
        RedlistFactorGroup2_PopulationReduction = 670,
        /// <summary>ContinuingDecline = 678</summary>
        ContinuingDecline = 678,
        /// <summary>ContinuingDeclineBasedOnHeader = 672</summary>
        ContinuingDeclineBasedOnHeader = 672,
        /// <summary>ContinuingDeclineBasedOn_Bbi = 673</summary>
        ContinuingDeclineBasedOn_Bbi = 673,
        /// <summary>ContinuingDeclineBasedOn_Bbii = 674</summary>
        ContinuingDeclineBasedOn_Bbii = 674,
        /// <summary>ContinuingDeclineBasedOn_Bbiii = 675</summary>
        ContinuingDeclineBasedOn_Bbiii = 675,
        /// <summary>ContinuingDeclineBasedOn_Bbiv = 676</summary>
        ContinuingDeclineBasedOn_Bbiv = 676,
        /// <summary>ContinuingDeclineBasedOn_Bbv = 677</summary>
        ContinuingDeclineBasedOn_Bbv = 677,
        /// <summary>Reduction_AHeader = 682</summary>
        Reduction_AHeader = 682,
        /// <summary>Reduction_A1Header = 683</summary>
        Reduction_A1Header = 683,
        /// <summary>Reduction_A1 = 684</summary>
        Reduction_A1 = 684,
        /// <summary>ReductionBasedOn_A1Header = 685</summary>
        ReductionBasedOn_A1Header = 685,
        /// <summary>ReductionBasedOn_A1a = 686</summary>
        ReductionBasedOn_A1a = 686,
        /// <summary>ReductionBasedOn_A1b = 687</summary>
        ReductionBasedOn_A1b = 687,
        /// <summary>ReductionBasedOn_A1c = 688</summary>
        ReductionBasedOn_A1c = 688,
        /// <summary>ReductionBasedOn_A1d = 689</summary>
        ReductionBasedOn_A1d = 689,
        /// <summary>ReductionBasedOn_A1e = 690</summary>
        ReductionBasedOn_A1e = 690,
        /// <summary>Reduction_A2Header = 691</summary>
        Reduction_A2Header = 691,
        /// <summary>Reduction_A2 = 692</summary>
        Reduction_A2 = 692,
        /// <summary>ReductionBasedOn_A2Header = 693</summary>
        ReductionBasedOn_A2Header = 693,
        /// <summary>ReductionBasedOn_A2a = 694</summary>
        ReductionBasedOn_A2a = 694,
        /// <summary>ReductionBasedOn_A2b = 695</summary>
        ReductionBasedOn_A2b = 695,
        /// <summary>ReductionBasedOn_A2c = 696</summary>
        ReductionBasedOn_A2c = 696,
        /// <summary>ReductionBasedOn_A2d = 697</summary>
        ReductionBasedOn_A2d = 697,
        /// <summary>ReductionBasedOn_A2e = 698</summary>
        ReductionBasedOn_A2e = 698,
        /// <summary>Reduction_A3Header = 699</summary>
        Reduction_A3Header = 699,
        /// <summary>Reduction_A3 = 700</summary>
        Reduction_A3 = 700,
        /// <summary>ReductionBasedOn_A3Header = 701</summary>
        ReductionBasedOn_A3Header = 701,
        /// <summary>ReductionBasedOn_A3b = 702</summary>
        ReductionBasedOn_A3b = 702,
        /// <summary>ReductionBasedOn_A3c = 703</summary>
        ReductionBasedOn_A3c = 703,
        /// <summary>ReductionBasedOn_A3d = 704</summary>
        ReductionBasedOn_A3d = 704,
        /// <summary>ReductionBasedOn_A3e = 705</summary>
        ReductionBasedOn_A3e = 705,
        /// <summary>Reduction_A4Header = 706</summary>
        Reduction_A4Header = 706,
        /// <summary>Reduction_A4 = 707</summary>
        Reduction_A4 = 707,
        /// <summary>ReductionBasedOn_A4Header = 708</summary>
        ReductionBasedOn_A4Header = 708,
        /// <summary>ReductionBasedOn_A4a = 709</summary>
        ReductionBasedOn_A4a = 709,
        /// <summary>ReductionBasedOn_A4b = 710</summary>
        ReductionBasedOn_A4b = 710,
        /// <summary>ReductionBasedOn_A4c = 711</summary>
        ReductionBasedOn_A4c = 711,
        /// <summary>ReductionBasedOn_A4d = 712</summary>
        ReductionBasedOn_A4d = 712,
        /// <summary>ReductionBasedOn_A4e = 713</summary>
        ReductionBasedOn_A4e = 713,
        /// <summary>RedlistFactorGroup3_PopulationSize = 714</summary>
        RedlistFactorGroup3_PopulationSize = 714,
        /// <summary>PopulationSize_Total = 715</summary>
        PopulationSize_Total = 715,
        /// <summary>MaxSizeLocalPopulation = 716</summary>
        MaxSizeLocalPopulation = 716,
        /// <summary>MaxProportionLocalPopulation = 717</summary>
        MaxProportionLocalPopulation = 717,
        /// <summary>ExtremeFluctuations = 718</summary>
        ExtremeFluctuations = 718,
        /// <summary>ExtremeFluctuationsInHeader = 720</summary>
        ExtremeFluctuationsInHeader = 720,
        /// <summary>ExtremeFluctuationsIn_Bci = 721</summary>
        ExtremeFluctuationsIn_Bci = 721,
        /// <summary>ExtremeFluctuationsIn_Bcii = 722</summary>
        ExtremeFluctuationsIn_Bcii = 722,
        /// <summary>ExtremeFluctuationsIn_Bciii = 723</summary>
        ExtremeFluctuationsIn_Bciii = 723,
        /// <summary>ExtremeFluctuationsIn_Bciv = 724</summary>
        ExtremeFluctuationsIn_Bciv = 724,
        /// <summary>RedlistFactorGroup4_PopulationDistribution = 725</summary>
        RedlistFactorGroup4_PopulationDistribution = 725,
        /// <summary>SeverelyFragmented = 726</summary>
        SeverelyFragmented = 726,
        /// <summary>NumberOfLocations = 727</summary>
        NumberOfLocations = 727,
        /// <summary>VeryRestrictedArea_D2VU = 728</summary>
        VeryRestrictedArea_D2VU = 728,
        /// <summary>ExtentOfOccurrence_B1Header = 729</summary>
        ExtentOfOccurrence_B1Header = 729,
        /// <summary>ExtentOfOccurrence_B1Observed = 730</summary>
        ExtentOfOccurrence_B1Observed = 730,
        /// <summary>ExtentOfOccurrence_B1Estimated = 731</summary>
        ExtentOfOccurrence_B1Estimated = 731,
        /// <summary>AreaOfOccupancy_B2Header = 732</summary>
        AreaOfOccupancy_B2Header = 732,
        /// <summary>AreaOfOccupancy_B2Observed = 733</summary>
        AreaOfOccupancy_B2Observed = 733,
        /// <summary>AreaOfOccupancy_B2Estimated = 734</summary>
        AreaOfOccupancy_B2Estimated = 734,
        /// <summary>RedlistFactorGroup5_QuantitativeAnalysis = 735</summary>
        RedlistFactorGroup5_QuantitativeAnalysis = 735,
        /// <summary>ProbabilityOfExtinction = 736</summary>
        ProbabilityOfExtinction = 736,
        /// <summary>RedlistFactorGroup6_RegionalImplications = 737</summary>
        RedlistFactorGroup6_RegionalImplications = 737,
        /// <summary>ImmigrationOccurs = 738</summary>
        ImmigrationOccurs = 738,
        /// <summary>ImmigrationDeclines = 739</summary>
        ImmigrationDeclines = 739,
        /// <summary>SUBSTRAT = 986</summary>
        Substrate = 986,
        /// <summary>SwedenIsASink = 740</summary>
        SwedenIsASink = 740,
        /// <summary>Grading = 741</summary>
        Grading = 741,
        /// <summary>RedlistFactorGroup7_Results = 742</summary>
        RedlistFactorGroup7_Results = 742,
        /// <summary>RedlistCategory = 743</summary>
        RedlistCategory = 743,
        /// <summary>RedlistCriteriaString = 744</summary>
        RedlistCriteriaString = 744,
        /// <summary>RedlistCriteriaDocumentation = 745</summary>
        RedlistCriteriaDocumentation = 745,
        /// <summary>RedlistCriteriaDocumentationComments = 746</summary>
        RedlistCriteriaDocumentationComments = 746,
        /// <summary>ExludeThisTaxonFromResult = 747</summary>
        ExludeThisTaxonFromResult = 747,
        /// <summary>RedlistedAsAnotherTaxon = 748</summary>
        RedlistedAsAnotherTaxon = 748,
        /// <summary>RedlistFileName = 749</summary>
        RedlistFileName = 749,
        /// <summary>RedlistLandscapes = 750</summary>
        RedlistLandscapes = 750,
        /// <summary>RedlistFactorGroup8_RedlistChanges = 751</summary>
        RedlistFactorGroup8_RedlistChanges = 751,
        /// <summary>RedlistChange_ActualSteps = 752</summary>
        RedlistChange_ActualSteps = 752,
        /// <summary>RedlistChangeMechanismHeader = 753</summary>
        RedlistChangeMechanismHeader = 753,
        /// <summary>RedlistChange_Actual = 754</summary>
        RedlistChange_Actual = 754,
        /// <summary>RedlistChange_NewKnowledge = 755</summary>
        RedlistChange_NewKnowledge = 755,
        /// <summary>RedlistChange_NewCriteria = 756</summary>
        RedlistChange_NewCriteria = 756,
        /// <summary>RedlistChange_NewInterpretation = 757</summary>
        RedlistChange_NewInterpretation = 757,
        /// <summary>RedlistChange_NewTaxon = 758</summary>
        RedlistChange_NewTaxon = 758,
        /// <summary>RedlistChange_NewTaxonStatus = 759</summary>
        RedlistChange_NewTaxonStatus = 759,
        /// <summary>CountyOccurrence = 775</summary>
        CountyOccurrence = 775,
        /// <summary>RedListCategoryAutomatic = 1940</summary>
        RedListCategoryAutomatic = 1940,
        /// <summary>RedListCriteriaAutomatic = 1941</summary>
        RedListCriteriaAutomatic = 1941,
        /// <summary>ConservationDependent = 1943</summary>
        ConservationDependent = 1943,
        /// <summary>RedlistIndexGroup = 1944</summary>
        RedlistIndexGroup = 1944,
        /// <summary>RedlistIndexCategory2000 = 1945</summary>
        RedlistIndexCategory2000 = 1945,
        /// <summary>RedlistIndexCategory2005 = 1946</summary>
        RedlistIndexCategory2005 = 1946,
        /// <summary>RedlistIndexCategory2010 = 1947</summary>
        RedlistIndexCategory2010 = 1947,
        /// <summary>
        /// RedlistIndexCategory2015 = 2688 (omvärderad)
        /// </summary>
        RedlistIndexCategory2015 = 2688,
        /// <summary>ResponsibilityGroup = 1952</summary>
        ResponsibilityGroup = 1952,
        /// <summary>Kriteriedokumentation Ingress = 1950</summary>
        RedListCriteriaDocumentationIntroduction = 1950,
        /// <summary>Kriteriedokumentation automatisk beräknad = 1951</summary>
        RedListCriteriaDocumentationAutomatic = 1951,
        /// <summary>Senast påträffad = 1948</summary>
        LastEncounter = 1948,
        /// <summary>Artfaktabladens faktor grupp = 1997</summary>
        SpeciesInformationDocumentGroup = 1997,
        /// <summary>Artfaktabladens faktor Taxonomisk information = 1998</summary>
        SpeciesInformationDocumentTaxonomicInformation = 1998,
        /// <summary>Artfaktabladens faktor Beskrivning = 1999</summary>
        SpeciesInformationDocumentDescription = 1999,
        /// <summary>Artfaktabladens faktor Utbredning och status = 2000</summary>
        SpeciesInformationDocumentDistribution = 2000,
        /// <summary>Artfaktabladens faktor Ekologi = 2001</summary>
        SpeciesInformationDocumentEcology = 2001,
        /// <summary>Artfaktabladens faktor Hot = 2002</summary>
        SpeciesInformationDocumentThreats = 2002,
        /// <summary>Artfaktabladens faktor Åtgärder = 2003</summary>
        SpeciesInformationDocumentMeasures = 2003,
        /// <summary>Artfaktabladens faktor Övrigt = 2004</summary>
        SpeciesInformationDocumentExtra = 2004,
        /// <summary>Artfaktabladens faktor Ingress = 2101</summary>
        SpeciesInformationDocumentPreamble = 2101,
        /// <summary>Artfaktabladens faktor Litteratur = 2005</summary>
        SpeciesInformationDocumentReferences = 2005,
        /// <summary>Artfaktabladens faktor Författare = 2006</summary>
        SpeciesInformationDocumentAuthorAndYear = 2006,
        /// <summary>Artfaktabladens faktor Textens kursiverade ord = 2007</summary>
        SpeciesInformationDocumentItalicsInText = 2007,
        /// <summary>Artfaktabladens faktor Littersaturförteckningens kursiverade ord = 2008</summary>
        SpeciesInformationDocumentItalicsInReferences = 2008,
        /// <summary>Artfaktabladens faktor Är publiceringsbar = 2012</summary>
        SpeciesInformationDocumentIsPublishable = 2012,
        /// <summary>Artfaktabladens faktor för interna kommentarer = 2013</summary>
        SpeciesInformationDocumentNotes = 2013,
        /// <summary>Artfaktabladens faktorgrupp för interna kommentarer mm = 2014</summary>
        SpeciesInformationDocumentMetadataFactorGroup = 2014,
        /// <summary>Artfaktabladens faktorgrupp för textstycken = 2015</summary>
        SpeciesInformationDocumentParagraphFactorGroup = 2015,
        /// <summary>Artfaktabladens faktorgrupp för format = 2016</summary>
        SpeciesInformationDocumentFormatFactorGroup = 2016,
        /// <summary>Nationalnyckelns automatiska faktor för Namngivning = 2100</summary>
        NNAutomaticTaxonNameSummary = 2100,
        /// <summary>Protection level for species observations of associated taxon.</summary>
        ProtectionLevel = 761,
        /// <summary>Natura 2000, Birds directive.</summary>
        Natura2000BirdsDirective = 961,
        /// <summary>Natura 2000, Habitats directive article 2.</summary>
        Natura2000HabitatsDirectiveArticle2 = 958,
        /// <summary>Natura 2000, Habitats directive article 4.</summary>
        Natura2000HabitatsDirectiveArticle4 = 960,
        /// <summary>Natura 2000, Habitats directive article 5.</summary>
        Natura2000HabitatsDirectiveArticle5 = 979,
        /// <summary>Action plan (in swedish "Åtgärdsprogram ÅGP").</summary>
        ActionPlan = 2017,
        /// <summary>
        /// Protected by law (in swedish
        /// "Fridlyst enligt Artskyddsförodningen (SFS 2007:845)").
        /// </summary>
        ProtectedByLaw = 2009,
        /// <summary>Action plan (in swedish "Åtgärdsprogram ÅGP").</summary>
        DisturbanceRadius = 2471
    }

    /// <summary>
    /// This class represents a factor.
    /// </summary>
    [Serializable]
    public class Factor : DataSortOrder, IListableItem
    {
        /// <summary>
        /// Name of the IsLeaf data field in WebFactor.
        /// </summary>
        public const String IS_LEAF = "IsLeaf";

        private Boolean _isLeaf;
        private String _name;
        private String _label;
        private String _information;
        private Boolean _isTaxonomic;
        private String _hostLabel;
        private Int32 _defaultHostParentTaxonId;
        private Int32 _factorOriginId;
        private FactorUpdateMode _factorUpdateMode;
        private FactorDataType _factorDataType = null;
        private FactorOrigin _factorOrigin = null;
        private Boolean _isPublic;
        private Boolean _isPeriodic;

        /// <summary>
        /// Create a Factor instance.
        /// </summary>
        /// <param name="id">Id of the factor</param>
        /// <param name="sortOrder">Sorting order of the factor</param>
        /// <param name="name">Name of factor (Is not allways available yet)</param>
        /// <param name="label">Label of the factor (Can be used as an header label for groups of edit kontrols representing the factor)</param>
        /// <param name="information">Information about the factor</param>
        /// <param name="isTaxonomic">Indicates whether or not the factor can be associated with a host taxon</param>
        /// <param name="hostLabel">Host taxon label of the factor</param>
        /// <param name="defaultHostParentTaxonId">Id of parent taxon to all potential host taxa</param>
        /// <param name="factorUpdateModeId">Update mode of the factor</param>
        /// <param name="factorDataTypeId">Datatype of the factor</param>
        /// <param name="factorOriginId">Origin of the factor</param>
        /// <param name="isPublic">Indicates whether or not the factor, and information associated to it, is avialable for public use</param>
        /// <param name="isPeriodic">Indicates whether or not the factor is periodic.</param>
        /// <param name="isLeaf">Indicates whether or not the factor is a leaf in the factor tree.</param>
        public Factor(
            Int32 id,
            Int32 sortOrder,
            String name,
            String label,
            String information,
            Boolean isTaxonomic,
            String hostLabel,
            Int32 defaultHostParentTaxonId,
            Int32 factorUpdateModeId,
            Int32 factorDataTypeId,
            Int32 factorOriginId,
            Boolean isPublic,
            Boolean isPeriodic,
            Boolean isLeaf)
            : base(id, sortOrder)
        {
            _name = name;
            _label = label;
            _information = information;
            _isTaxonomic = isTaxonomic;
            _hostLabel = hostLabel;
            _defaultHostParentTaxonId = defaultHostParentTaxonId;
            _factorUpdateMode = FactorManager.GetFactorUpdateMode(factorUpdateModeId);

            _factorDataType = null;
            if (factorDataTypeId > -1)
            {
                _factorDataType = FactorManager.GetFactorDataType(factorDataTypeId);
            }

            _factorOriginId = factorOriginId;
            _factorOrigin = null;

            _isPublic = isPublic;
            _isPeriodic = isPeriodic;
            _isLeaf = isLeaf;
        }

        /// <summary>
        /// Indicates if this factor is a leaf in the factor tree.
        /// </summary>
        public Boolean IsLeaf
        {
            get { return _isLeaf; }
            set { _isLeaf = value; }
        }

        /// <summary>
        /// Get name of this factor object.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Get label of this factor object.
        /// </summary>
        public String Label
        {
            get { return _label; }
        }

        /// <summary>
        /// Get information of this factor object.
        /// </summary>
        public String Information
        {
            get { return _information; }
        }

        /// <summary>
        /// Indication about whether or not this factor object can be associated with a host taxon.
        /// </summary>
        public Boolean IsTaxonomic
        {
            get { return _isTaxonomic; }
        }

        /// <summary>
        /// HostLabel for this factor object.
        /// </summary>
        public String HostLabel
        {
            get { return _hostLabel; }
        }

        /// <summary>
        /// Taxon id for parent taxon for all potential hosts associated with this factor object.
        /// </summary>
        public Int32 DefaultHostParentTaxonId
        {
            get { return _defaultHostParentTaxonId; }
        }

        /// <summary>
        /// Indication about whether or not this factor object is periodic.
        /// </summary>
        public Boolean IsPeriodic
        {
            get { return _isPeriodic; }
        }

        /// <summary>
        /// Indication about whether or not this factor object should be available for public use.
        /// </summary>
        public Boolean IsPublic
        {
            get { return _isPublic; }
        }

        /// <summary>
        /// Factor update mode of this factor object.
        /// </summary>
        public FactorUpdateMode FactorUpdateMode
        {
            get { return _factorUpdateMode; }
        }

        /// <summary>
        /// Factor data type of this factor object.
        /// </summary>
        public FactorDataType FactorDataType
        {
            get { return _factorDataType; }
        }

        /// <summary>
        /// Origin of this factor object.
        /// </summary>
        public FactorOrigin FactorOrigin
        {
            get
            {
                if (_factorOrigin.IsNull() && (_factorOriginId > -1))
                {
                    _factorOrigin = FactorManager.GetFactorOrigin(_factorOriginId);
                }
                return _factorOrigin;
            }
        }

        /// <summary>
        /// Id of the Factor origin of this factor object.
        /// </summary>
        public Int32 FactorOriginId
        {
            get
            {
                return _factorOriginId;
            }
        }

        /// <summary>
        /// Get tree where this factor is the top node.
        /// </summary>
        public FactorTreeNode Tree
        {
            get
            {
                return FactorManager.GetFactorTree(Id);
            }
        }

        /// <summary>
        /// Get all factors that have an impact on this factors value.
        /// Only factors that are automatically calculated
        /// has dependent factors.
        /// </summary>
        /// <returns>Dependent factors.</returns>
        public FactorList GetDependentFactors()
        {
            FactorList factors;

            factors = new FactorList();
            switch (Id)
            {
                case ((Int32)FactorId.RedListCategoryAutomatic):
                case ((Int32)FactorId.RedListCriteriaDocumentationAutomatic):
                case ((Int32)FactorId.RedListCriteriaAutomatic):
                    factors.Add(FactorManager.GetFactor(FactorId.AreaOfOccupancy_B2Estimated));
                    factors.Add(FactorManager.GetFactor(FactorId.ConservationDependent));
                    factors.Add(FactorManager.GetFactor(FactorId.ContinuingDecline));
                    factors.Add(FactorManager.GetFactor(FactorId.ContinuingDeclineBasedOn_Bbi));
                    factors.Add(FactorManager.GetFactor(FactorId.ContinuingDeclineBasedOn_Bbii));
                    factors.Add(FactorManager.GetFactor(FactorId.ContinuingDeclineBasedOn_Bbiii));
                    factors.Add(FactorManager.GetFactor(FactorId.ContinuingDeclineBasedOn_Bbiv));
                    factors.Add(FactorManager.GetFactor(FactorId.ContinuingDeclineBasedOn_Bbv));
                    factors.Add(FactorManager.GetFactor(FactorId.ExtentOfOccurrence_B1Estimated));
                    factors.Add(FactorManager.GetFactor(FactorId.ExtremeFluctuations));
                    factors.Add(FactorManager.GetFactor(FactorId.ExtremeFluctuationsIn_Bci));
                    factors.Add(FactorManager.GetFactor(FactorId.ExtremeFluctuationsIn_Bcii));
                    factors.Add(FactorManager.GetFactor(FactorId.ExtremeFluctuationsIn_Bciii));
                    factors.Add(FactorManager.GetFactor(FactorId.ExtremeFluctuationsIn_Bciv));
                    factors.Add(FactorManager.GetFactor(FactorId.Grading));
                    factors.Add(FactorManager.GetFactor(FactorId.MaxProportionLocalPopulation));
                    factors.Add(FactorManager.GetFactor(FactorId.MaxSizeLocalPopulation));
                    factors.Add(FactorManager.GetFactor(FactorId.NumberOfLocations));
                    factors.Add(FactorManager.GetFactor(FactorId.PopulationSize_Total));
                    factors.Add(FactorManager.GetFactor(FactorId.ProbabilityOfExtinction));
                    factors.Add(FactorManager.GetFactor(FactorId.RedlistEvaluationProgressionStatus));
                    factors.Add(FactorManager.GetFactor(FactorId.Reduction_A1));
                    factors.Add(FactorManager.GetFactor(FactorId.Reduction_A2));
                    factors.Add(FactorManager.GetFactor(FactorId.Reduction_A3));
                    factors.Add(FactorManager.GetFactor(FactorId.Reduction_A4));
                    factors.Add(FactorManager.GetFactor(FactorId.ReductionBasedOn_A1a));
                    factors.Add(FactorManager.GetFactor(FactorId.ReductionBasedOn_A1b));
                    factors.Add(FactorManager.GetFactor(FactorId.ReductionBasedOn_A1c));
                    factors.Add(FactorManager.GetFactor(FactorId.ReductionBasedOn_A1d));
                    factors.Add(FactorManager.GetFactor(FactorId.ReductionBasedOn_A1e));
                    factors.Add(FactorManager.GetFactor(FactorId.ReductionBasedOn_A2a));
                    factors.Add(FactorManager.GetFactor(FactorId.ReductionBasedOn_A2b));
                    factors.Add(FactorManager.GetFactor(FactorId.ReductionBasedOn_A2c));
                    factors.Add(FactorManager.GetFactor(FactorId.ReductionBasedOn_A2d));
                    factors.Add(FactorManager.GetFactor(FactorId.ReductionBasedOn_A2e));
                    factors.Add(FactorManager.GetFactor(FactorId.ReductionBasedOn_A3b));
                    factors.Add(FactorManager.GetFactor(FactorId.ReductionBasedOn_A3c));
                    factors.Add(FactorManager.GetFactor(FactorId.ReductionBasedOn_A3d));
                    factors.Add(FactorManager.GetFactor(FactorId.ReductionBasedOn_A3e));
                    factors.Add(FactorManager.GetFactor(FactorId.ReductionBasedOn_A4a));
                    factors.Add(FactorManager.GetFactor(FactorId.ReductionBasedOn_A4b));
                    factors.Add(FactorManager.GetFactor(FactorId.ReductionBasedOn_A4c));
                    factors.Add(FactorManager.GetFactor(FactorId.ReductionBasedOn_A4d));
                    factors.Add(FactorManager.GetFactor(FactorId.ReductionBasedOn_A4e));
                    factors.Add(FactorManager.GetFactor(FactorId.SeverelyFragmented));
                    factors.Add(FactorManager.GetFactor(FactorId.SwedishOccurrence));
                    factors.Add(FactorManager.GetFactor(FactorId.VeryRestrictedArea_D2VU));
                    if (Id == ((Int32)FactorId.RedListCriteriaDocumentationAutomatic))
                    {
                        factors.Add(FactorManager.GetFactor(FactorId.RedListCriteriaDocumentationIntroduction));
                        factors.Add(FactorManager.GetFactor(FactorId.GlobalRedlistCategory));
                        factors.Add(FactorManager.GetFactor(FactorId.GenerationTime));
                        factors.Add(FactorManager.GetFactor(FactorId.LastEncounter));
                        factors.Add(FactorManager.GetFactor(FactorId.ImmigrationOccurs));
                    }

                    break;
            }

            return factors;
        }

        /// <summary>
        /// Get a text string with the basic information about the faktor object that can be presented in applications as a hint text.
        /// </summary>
        /// <returns>The hint text.</returns>
        public String GetHint()
        {
            StringBuilder hintText = new StringBuilder();

            hintText.AppendLine("Namn: " + _label + ", FaktorId: " + this.Id.ToString());

            if (_factorOrigin.IsNotNull())
            {
                hintText.AppendLine("Ursprung: " + _factorOrigin.Name);
            }

            if (_factorDataType.IsNotNull())
            {
                hintText.AppendLine("Datatyp: " + _factorDataType.Label + ", DatatypId: " + _factorDataType.Id.ToString());
            }

            if (_factorUpdateMode.IsHeader)
            {
                hintText.AppendLine("Denna faktor fungerar enbart som rubrik för en grupp underliggande faktorer");
            }
            else
            {
                if (_isLeaf)
                {
                    hintText.AppendLine("Det finns inga underordnade faktorer under denna faktor (faktorn är ett löv)");
                }
                else
                {
                    hintText.AppendLine("Denna faktor har underordnade faktorer");
                }


                if (!_factorUpdateMode.AllowManualUpdate)
                {

                    if (_factorUpdateMode.AllowAutomaticUpdate)
                    {
                        hintText.AppendLine("Manuell uppdatering tillåts inte, värdet beräknas automatiskt baserat på andra faktorers värden");
                    }
                    else
                    {
                        hintText.AppendLine("Uppdatering av information knuten till denna faktor sker inte längre (arkiv)");
                    }
                }

                if (_isPeriodic)
                {
                    hintText.AppendLine("Denna faktor uppdateras periodiskt");
                }
                else
                {
                    hintText.AppendLine("Denna faktor kan uppdateras kontinuerligt vid behov");
                }



                if (_isTaxonomic)
                {
                    hintText.AppendLine("Bedömningar av ett taxons relation till denna faktor kan specificeras olika för valfritt många värdtaxa");
                }


                if (_isPublic)
                {
                    hintText.AppendLine("Publik: Ja");
                }
                else
                {
                    hintText.AppendLine("Publik: Nej");
                }

                if (_factorDataType.Fields.Count == 1)
                {
                    hintText.AppendLine("Faktorn har endast ett fält: " + _factorDataType.Fields[0].Label);
                }
                else
                {
                    hintText.Append("Förutom huvudfältet finns ");
                    hintText.Append((_factorDataType.SubstantialFields.Count - 1).ToString() + " substantiella värdefält");
                    hintText.AppendLine(" och " + (_factorDataType.Fields.Count - _factorDataType.SubstantialFields.Count).ToString() + " övriga värdefält");
                }
            }

            return hintText.ToString();
        }

      
    }
}

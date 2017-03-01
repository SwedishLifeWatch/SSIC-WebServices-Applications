using System;
using System.Collections.Generic;
using System.Text;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class represents a Species Information Document (Artfaktablad).
    /// It holds all information included in such a document.
    /// </summary>
    [Serializable]
    public class SpeciesInformationDocument
    {
        private List<String> _italicStringsInAutomaticTaxonomicParagraph = null;
        private Period _period;
        private String _automaticTaxonomicString = null;
        private String _redlistCriteria = null;
        private String _organismGroupName = null;
        private String _redlistCategoryName = null;
        private String _redlistCategoryShortString = null;
        private SpeciesFact _authorParagraphSpeciesFact = null;
        private SpeciesFact _preambleParagraphSpeciesFact = null;
        private SpeciesFact _descriptionParagraphSpeciesFact = null;
        private SpeciesFact _distributionParagraphSpeciesFact = null;
        private SpeciesFact _ecologyParagraphSpeciesFact = null;
        private SpeciesFact _extraParagraphSpeciesFact = null;
        private SpeciesFact _isPublishable = null;
        private SpeciesFact _italicsInReferencesParagraphSpeciesFact = null;
        private SpeciesFact _italicsInTextParagraphSpeciesFact = null;
        private SpeciesFact _measuresParagraphSpeciesFact = null;
        private SpeciesFact _referencesParagraphSpeciesFact = null;
        private SpeciesFact _taxonomicParagraphSpeciesFact = null;
        private SpeciesFact _threatsParagraphSpeciesFact = null;
        private SpeciesFactList _speciesFacts = null;
        private TaxonList _suitableParents = null;
        private TaxonNameList _synonyms = null;
        private Taxon _taxon;

        /// <summary>
        /// Creates a instance of a Species Information Document object based on TaxonId.
        /// </summary>
        /// <param name="taxonId">Id of the taxon</param>
        public SpeciesInformationDocument(Int32 taxonId)
            : this(TaxonManager.GetTaxon(taxonId, TaxonInformationType.Basic))
        {
        }

        /// <summary>
        /// Creates a instance of a Species Information Document
        /// object based on a list of species facts.
        /// </summary>
        /// <param name="speciesFacts">The species facts related to species information document factors and a single taxon</param>
        public SpeciesInformationDocument(SpeciesFactList speciesFacts)
            : this(speciesFacts, PeriodManager.GetCurrentPublicPeriod())
        {
        }

        /// <summary>
        /// Creates a instance of a Species Information Document
        /// object based on a list of species facts.
        /// </summary>
        /// <param name="speciesFacts">The species facts related to species information document factors and a single taxon</param>
        /// <param name="period">Period for red list information.</param>
        public SpeciesInformationDocument(SpeciesFactList speciesFacts,
                                          Period period)
        {
            _period = period;
            _speciesFacts = speciesFacts;
            if (_speciesFacts.IsEmpty())
            {
                _taxon = null;
            }
            else
            {
                _taxon = _speciesFacts[0].Taxon;
            }
            SetParagraphSpeciesFacts();
        }

        /// <summary>
        /// Creates a instance of a Species Information Document
        /// object based on taxon.
        /// </summary>
        /// <param name="taxon">Taxon.</param>
        public SpeciesInformationDocument(Taxon taxon)
            : this(taxon, PeriodManager.GetCurrentPublicPeriod())
        {
        }

        /// <summary>
        /// Creates a instance of a Species Information Document
        /// object based on taxon and period.
        /// </summary>
        /// <param name="taxon">Taxon.</param>
        /// <param name="period">Period for red list information.</param>
        public SpeciesInformationDocument(Taxon taxon, Period period)
        {
            _period = period;
            _taxon = taxon;
            GetSpeciesFacts();
            SetParagraphSpeciesFacts();
        }

        /// <summary>
        /// A string representing the original author and the year the document was written
        /// </summary>
        public String AuthorAndYear
        {
            get
            {
                if (_authorParagraphSpeciesFact.IsNotNull() &&
                    _authorParagraphSpeciesFact.Field5.HasValue)
                {
                    return _authorParagraphSpeciesFact.Field5.StringValue;
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// An automatic alternative to the taxonomic paragraph,
        /// which is dynamically generated based on Dyntaxa information.
        /// If the quality value of the Taxonomic Paragraph Species Fact
        /// information is set to id 1 (Very good),
        /// then the text from Taxonomic paragraph is used instead.
        /// </summary>
        public String AutomaticTaxonomicParagraph
        {
            get
            {
                if (_automaticTaxonomicString.IsNull())
                {
                    _automaticTaxonomicString = GetAutomaticTaxonomicString();
                }
                return _automaticTaxonomicString;
            }
        }

        /// <summary>
        /// The recommended common name.
        /// </summary>
        public String CommonName
        {
            get { return _taxon.CommonName; }
        }

        /// <summary>
        /// A paragraph that gives a description on the taxon
        /// </summary>
        public String DescriptionParagraph
        {
            get
            {
                if (_descriptionParagraphSpeciesFact.IsNotNull() &&
                    _descriptionParagraphSpeciesFact.Field5.HasValue)
                {
                    return _descriptionParagraphSpeciesFact.Field5.StringValue;
                }
                return String.Empty;
            }

            set
            {
                if (_descriptionParagraphSpeciesFact.IsNotNull())
                {
                    _descriptionParagraphSpeciesFact.Field5.StringValue = value;
                }
            }
        }

        /// <summary>
        /// A paragraph that describes the distribution
        /// </summary>
        public String DistributionParagraph
        {
            get
            {
                if (_distributionParagraphSpeciesFact.IsNotNull() &&
                    _distributionParagraphSpeciesFact.Field5.HasValue)
                {
                    return _distributionParagraphSpeciesFact.Field5.StringValue;
                }
                return String.Empty;
            }

            set
            {
                if (_distributionParagraphSpeciesFact.IsNotNull())
                {
                    _distributionParagraphSpeciesFact.Field5.StringValue = value;
                }
            }
        }

        /// <summary>
        /// A paragraph that describes shortley the ecology of the taxon
        /// </summary>
        public String EcologyParagraph
        {
            get
            {
                if (_ecologyParagraphSpeciesFact.IsNotNull() &&
                    _ecologyParagraphSpeciesFact.Field5.HasValue)
                {
                    return _ecologyParagraphSpeciesFact.Field5.StringValue;
                }
                return String.Empty;
            }

            set
            {
                if (_ecologyParagraphSpeciesFact.IsNotNull())
                {
                    _ecologyParagraphSpeciesFact.Field5.StringValue = value;
                }
            }
        }

        /// <summary>
        /// A paragraph extra information about the taxon
        /// </summary>
        public String ExtraParagraph
        {
            get
            {
                if (_extraParagraphSpeciesFact.IsNotNull() &&
                    _extraParagraphSpeciesFact.Field5.HasValue)
                {
                    return _extraParagraphSpeciesFact.Field5.StringValue;
                }
                return String.Empty;
            }

            set
            {
                if (_extraParagraphSpeciesFact.IsNotNull())
                {
                    _extraParagraphSpeciesFact.Field5.StringValue = value;
                }
            }
        }

        /// <summary>
        /// Tests if the species fact document should
        /// be published or not.
        /// </summary>
        public Boolean IsPublishable
        {
            get
            {
                if (_isPublishable.IsNotNull() &&
                    _isPublishable.MainField.HasValue)
                {
                    return _isPublishable.MainField.BooleanValue;
                }
                return false;
            }

            set
            {
                if (_isPublishable.IsNotNull())
                {
                    _isPublishable.MainField.BooleanValue = value;
                }
            }
        }

        /// <summary>
        /// List of strings within the Reference list that should be formated with italics.
        /// </summary>
        public List<String> ItalicStringsInReferences
        {
            get
            {
                List<String> italicStringsInReferences;

                italicStringsInReferences = new List<String>();
                if (_italicsInReferencesParagraphSpeciesFact.IsNotNull() &&
                    _italicsInReferencesParagraphSpeciesFact.Field5.HasValue)
                {
                    italicStringsInReferences.AddRange(_italicsInReferencesParagraphSpeciesFact.Field5.StringValue.Split('\n'));
                }
                return italicStringsInReferences;
            }
        }

        /// <summary>
        /// List of strings within the document main text paragraphps that should be formated with italics.
        /// </summary>
        public List<String> ItalicStringsInText
        {
            get
            {
                List<String> italicStringsInText;

                italicStringsInText = new List<String>();
                if (_italicsInTextParagraphSpeciesFact.IsNotNull() &&
                    _italicsInTextParagraphSpeciesFact.Field5.HasValue)
                {
                    italicStringsInText.AddRange(_italicsInTextParagraphSpeciesFact.Field5.StringValue.Split('\n'));
                }
                if (_italicStringsInAutomaticTaxonomicParagraph.IsNotEmpty())
                {
                    italicStringsInText.AddRange(_italicStringsInAutomaticTaxonomicParagraph);
                }
                return italicStringsInText;
            }
        }

        /// <summary>
        /// A paragraph describing the conservation measures suitable for the taxon
        /// </summary>
        public String MeasuresParagraph
        {
            get
            {
                if (_measuresParagraphSpeciesFact.IsNotNull() &&
                    _measuresParagraphSpeciesFact.Field5.HasValue)
                {
                    return _measuresParagraphSpeciesFact.Field5.StringValue;
                }
                return String.Empty;
            }

            set
            {
                if (_measuresParagraphSpeciesFact.IsNotNull())
                {
                    _measuresParagraphSpeciesFact.Field5.StringValue = value;
                }
            }
        }

        /// <summary>
        /// The organism group name according tgo the Swedish redlist.
        /// </summary>
        public String OrganismGroup
        {
            get
            {
                LoadRedListInformation();
                return _organismGroupName;
            }
        }

        /// <summary>
        /// A paragraph that is given a short introduction to the species ant the whole document.
        /// </summary>
        public String PreambleParagraph
        {
            get
            {
                if (_preambleParagraphSpeciesFact.IsNotNull() &&
                    _preambleParagraphSpeciesFact.Field5.HasValue)
                {
                    return _preambleParagraphSpeciesFact.Field5.StringValue;
                }
                return String.Empty;
            }

            set
            {
                if (_preambleParagraphSpeciesFact.IsNotNull())
                {
                    _preambleParagraphSpeciesFact.Field5.StringValue = value;
                }
            }
        }

        /// <summary>
        /// The redlist category name according to the latest swedish redlist.
        /// </summary>
        public String RedlistCategoryName
        {
            get
            {
                LoadRedListInformation();
                return _redlistCategoryName;
            }
        }

        /// <summary>
        /// The redlist category short string according to the latest swedish redlist.
        /// </summary>
        public String RedlistCategoryShortString
        {
            get
            {
                LoadRedListInformation();
                return _redlistCategoryShortString;
            }
        }

        /// <summary>
        /// The redlist criteria if the taxon is
        /// threatened according to the swedish redlist.
        /// </summary>
        public String RedlistCriteria
        {
            get
            {
                LoadRedListInformation();
                return _redlistCriteria;
            }
        }

        /// <summary>
        /// A paragraph holding a list of references. Each reference is a paragraph it self separated by \n
        /// </summary>
        public String ReferenceParagraph
        {
            get
            {
                if (_referencesParagraphSpeciesFact.IsNotNull() &&
                    _referencesParagraphSpeciesFact.Field5.HasValue)
                {
                    return _referencesParagraphSpeciesFact.Field5.StringValue;
                }
                return String.Empty;
            }

            set
            {
                if (_referencesParagraphSpeciesFact.IsNotNull())
                {
                    _referencesParagraphSpeciesFact.Field5.StringValue = value;
                }
            }
        }

        /// <summary>
        /// Get the recommended scientific name.
        /// </summary>
        public String ScientificName
        {
            get { return _taxon.ScientificName; }
        }

        /// <summary>
        /// A list of all the species fact objects that are
        /// related to this Species Information Document.
        /// </summary>
        public SpeciesFactList SpeciesFacts
        {
            get { return _speciesFacts; }
        }

        /// <summary>
        /// The taxon object of the Species Information Document.
        /// </summary>
        public Taxon Taxon
        {
            get { return _taxon; }
        }

        /// <summary>
        /// A paragraph with basic taxonomic information
        /// </summary>
        public String TaxonomicParagraph
        {
            get
            {
                if (_taxonomicParagraphSpeciesFact.IsNotNull() &&
                    _taxonomicParagraphSpeciesFact.Field5.HasValue)
                {
                    return _taxonomicParagraphSpeciesFact.Field5.StringValue;
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// A paragraph about the thrats affecting the taxon
        /// </summary>
        public String ThreatsParagraph
        {
            get
            {
                if (_threatsParagraphSpeciesFact.IsNotNull() &&
                    _threatsParagraphSpeciesFact.Field5.HasValue)
                {
                    return _threatsParagraphSpeciesFact.Field5.StringValue;
                }
                return String.Empty;
            }

            set
            {
                if (_threatsParagraphSpeciesFact.IsNotNull())
                {
                    _threatsParagraphSpeciesFact.Field5.StringValue = value;
                }
            }
        }

        /// <summary>
        /// The latest date for revision of parts the content of this species information document.
        /// </summary>
        public DateTime UpdateDateMaxValue
        {
            get
            {
                DateTime date = DateTime.MinValue;
                if (_speciesFacts.IsNotEmpty())
                {
                    foreach (SpeciesFact speciesFact in _speciesFacts)
                    {
                        if (speciesFact.HasUpdateDate)
                        {
                            if (speciesFact.UpdateDate > date)
                            {
                                date = speciesFact.UpdateDate;
                            }
                        }
                    }
                }
                return date;
            }
        }

        /// <summary>
        /// The oldest date for revision of parts of the content of this species information document.
        /// </summary>
        public DateTime UpdateDateMinValue
        {
            get
            {
                DateTime date = DateTime.MaxValue;
                if (_speciesFacts.IsNotEmpty())
                {
                    foreach (SpeciesFact speciesFact in _speciesFacts)
                    {
                        if (speciesFact.HasUpdateDate)
                        {
                            if (speciesFact.UpdateDate < date)
                            {
                                date = speciesFact.UpdateDate;
                            }
                        }
                    }
                }
                return date;
            }
        }

        /// <summary>
        /// A selected number of suitable parent taxa.
        /// </summary>
        public TaxonList ParentTaxa 
        {
            get
            {
                if (_suitableParents.IsEmpty())
                {
                    if (_taxon.IsNotNull())
                    {
                        TaxonSearchCriteria criteria = new TaxonSearchCriteria();
                        List<Int32> taxonIds = new List<Int32>();
                        taxonIds.Add(_taxon.Id);
                        criteria.RestrictSearchToTaxonIds = taxonIds;
                        criteria.RestrictReturnToScope = WebService.TaxonSearchScope.AllParentTaxa;
                        TaxonList parentTaxa = TaxonManager.GetTaxaBySearchCriteria(criteria);
                        _suitableParents = new TaxonList();
                        foreach (Taxon parent in parentTaxa)
                        {
                            if ((parent.TaxonType.Id == (Int32)(TaxonTypeId.Kingdom)) ||
                                (parent.TaxonType.Id == (Int32)(TaxonTypeId.Phylum)) ||
                                (parent.TaxonType.Id == (Int32)(TaxonTypeId.Class)) ||
                                (parent.TaxonType.Id == (Int32)(TaxonTypeId.Order)) ||
                                (parent.TaxonType.Id == (Int32)(TaxonTypeId.Family)))
                            {
                                if (!(parent.Id == (Int32)TaxonId.Dummy))
                                {
                                    _suitableParents.Add(parent);
                                }
                            }
                        }
                    }
                }
                return _suitableParents;
            }
        }

        /// <summary>
        /// A selected number of suitable parent taxa.
        /// </summary>
        public TaxonNameList Synonyms
        {
            get
            {
                if (_synonyms.IsEmpty())
                {
                    if (_taxon.IsNotNull())
                    {
                        _synonyms = new TaxonNameList();
                        foreach (TaxonName name in _taxon.TaxonNames)
                        {
                            if ((name.TaxonNameType.Id == 0) &&
                                (name.TaxonNameUseType.Id == 0) &&
                                (!name.IsRecommended) &&
                                (name.Name != _taxon.ScientificName))
                            {
                                _italicStringsInAutomaticTaxonomicParagraph.Add(name.Name);
                                _synonyms.Add(name);
                            }
                        }
                    }
                }
                return _synonyms;
            }
        }
        

        /// <summary>
        /// Get string with taxonomic information.
        /// </summary>
        /// <returns>String with taxonomic information.</returns>
        private String GetAutomaticTaxonomicString()
        {
            StringBuilder text;

            if (_taxonomicParagraphSpeciesFact.IsNotNull() &&
                (_taxonomicParagraphSpeciesFact.Quality.Id == (Int32)SpeciesFactQualityId.VerryGood) &&
                _taxonomicParagraphSpeciesFact.Field5.HasValue)
            {
                return _taxonomicParagraphSpeciesFact.Field5.StringValue;
            }

            text = new StringBuilder();
            if (_taxon.IsNotNull())
            {
                _italicStringsInAutomaticTaxonomicParagraph.Clear();


                Int32 startFromIndex = ParentTaxa.Count - 3;
                if (startFromIndex < 0) startFromIndex = 0;
                for (Int32 index = startFromIndex; index < ParentTaxa.Count; index++)
                {
                    if (text.ToString() != String.Empty)
                    {
                        text.Append(", ");
                    }
                    text.Append(ParentTaxa[index].TaxonType.Label + " ");
                    text.Append(ParentTaxa[index].ScientificName);

                    //Eventuellt ska denna kodrad tas bort (förslag från Tomas Hallingbäck): Överordnade taxa bör ej vara kursiverade 
                    //Enligt artexperternas diskussion 2010-03-04 ska överordnade vetenskapliga taxonnamn ej kursiveras!
                    //_italicStringsInAutomaticTaxonomicParagraph.Add(suitableParents[index].ScientificName);

                    if (ParentTaxa[index].CommonName.IsNotEmpty())
                    {
                        text.Append(" (" + ParentTaxa[index].CommonName + ")");
                    }
                }

                if (text.ToString() != String.Empty)
                {
                    text.Append(", ");
                }

                text.Append(_taxon.ScientificNameAndAuthor);
                _italicStringsInAutomaticTaxonomicParagraph.Add(_taxon.ScientificName);

                

                if (Synonyms.Count > 0)
                {
                    text.Append(". Syn. ");
                    for (Int32 itemIndex = 0; itemIndex < Synonyms.Count; itemIndex++)
                    {
                        if (itemIndex > 0)
                        {
                            if (itemIndex == Synonyms.Count - 1)
                            {
                                text.Append(" och ");
                            }
                            else
                            {
                                text.Append(", ");
                            }
                        }

                        text.Append(Synonyms[itemIndex].Name);
                        if (Synonyms[itemIndex].Author.IsNotEmpty())
                        {
                            text.Append(" " + Synonyms[itemIndex].Author);
                        }
                    }
                }
                if (text.ToString() != String.Empty)
                {
                    text.Append(". ");
                }
            }
            String cleanText = text.ToString().Replace("..", ".");
            return cleanText;
        }

        /// <summary>
        /// Get species facts with species document information.
        /// </summary>
        private void GetSpeciesFacts()
        {
            UserParameterSelection parameters = new UserParameterSelection();
            TaxonList taxa = new TaxonList();
            taxa.Add(_taxon);
            parameters.Taxa = taxa;
            FactorSearchCriteria criteria = new FactorSearchCriteria();
            List<Int32> factorIds = new List<Int32>();
            factorIds.Add((Int32)FactorId.SpeciesInformationDocumentGroup);
            criteria.RestrictSearchToFactorIds = factorIds;
            criteria.RestrictReturnToScope = ArtDatabanken.Data.WebService.FactorSearchScope.LeafFactors;
            parameters.Factors = FactorManager.GetFactorsBySearchCriteria(criteria);
            _speciesFacts = SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(parameters);
        }

        /// <summary>
        /// A method that obtains red list information.
        /// </summary>
        private void LoadRedListInformation()
        {
            SpeciesFactList speciesFacts;
            UserParameterSelection parameters;

            if (_organismGroupName.IsNull())
            {
                _organismGroupName = String.Empty;
                _redlistCategoryName = String.Empty;
                _redlistCategoryShortString = String.Empty;
                _redlistCriteria = String.Empty;

                parameters = new UserParameterSelection();
                parameters.Taxa.Add(_taxon);
                parameters.Factors.Add(FactorManager.GetFactor(FactorId.RedlistCategory));
                parameters.Factors.Add(FactorManager.GetFactor(FactorId.RedlistCriteriaString));
                parameters.Factors.Add(FactorManager.GetFactor(FactorId.Redlist_OrganismLabel1));
                parameters.Periods.Add(_period);
                speciesFacts = SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(parameters);
                if (speciesFacts.IsNotEmpty())
                {
                    foreach (SpeciesFact speciesFact in speciesFacts)
                    {
                        switch (speciesFact.Factor.Id)
                        {
                            case (Int32)FactorId.RedlistCategory:
                                if (speciesFact.HasField1)
                                {
                                    _redlistCategoryName = speciesFact.Field1.EnumValue.OriginalLabel;
                                    _redlistCategoryName = _redlistCategoryName.Remove(_redlistCategoryName.Length - 5);
                                }
                                if (speciesFact.HasField4)
                                {
                                    _redlistCategoryShortString = speciesFact.Field4.StringValue;
                                }
                                break;
                            case (Int32)FactorId.RedlistCriteriaString:
                                if (speciesFact.HasField4 &&
                                    speciesFact.Field4.StringValue.IsNotEmpty())
                                {
                                    _redlistCriteria = speciesFact.Field4.StringValue.Trim();
                                }
                                break;
                            case (Int32)FactorId.Redlist_OrganismLabel1:
                                if (speciesFact.MainField.HasValue)
                                {
                                    _organismGroupName = speciesFact.MainField.EnumValue.OriginalLabel;
                                }
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Bind species fact to related member.
        /// </summary>
        private void SetParagraphSpeciesFacts()
        {
            SpeciesFactList speciesFacts;

            _italicStringsInAutomaticTaxonomicParagraph = new List<String>();
            if (_speciesFacts.IsNotEmpty())
            {
                speciesFacts = new SpeciesFactList();
                foreach (SpeciesFact speciesFact in _speciesFacts)
                {
                    switch (speciesFact.Factor.Id)
                    {
                        case (Int32)FactorId.SpeciesInformationDocumentTaxonomicInformation:
                            speciesFacts.Add(speciesFact);
                            _taxonomicParagraphSpeciesFact = speciesFact;
                            break;
                        case (Int32)FactorId.SpeciesInformationDocumentDescription:
                            speciesFacts.Add(speciesFact);
                            _descriptionParagraphSpeciesFact = speciesFact;
                            break;
                        case (Int32)FactorId.SpeciesInformationDocumentDistribution:
                            speciesFacts.Add(speciesFact);
                            _distributionParagraphSpeciesFact = speciesFact;
                            break;
                        case (Int32)FactorId.SpeciesInformationDocumentEcology:
                            speciesFacts.Add(speciesFact);
                            _ecologyParagraphSpeciesFact = speciesFact;
                            break;
                        case (Int32)FactorId.SpeciesInformationDocumentThreats:
                            speciesFacts.Add(speciesFact);
                            _threatsParagraphSpeciesFact = speciesFact;
                            break;
                        case (Int32)FactorId.SpeciesInformationDocumentMeasures:
                            speciesFacts.Add(speciesFact);
                            _measuresParagraphSpeciesFact = speciesFact;
                            break;
                        case (Int32)FactorId.SpeciesInformationDocumentExtra:
                            speciesFacts.Add(speciesFact);
                            _extraParagraphSpeciesFact = speciesFact;
                            break;
                        case (Int32)FactorId.SpeciesInformationDocumentPreamble:
                            speciesFacts.Add(speciesFact);
                            _preambleParagraphSpeciesFact = speciesFact;
                            break;
                        case (Int32)FactorId.SpeciesInformationDocumentReferences:
                            speciesFacts.Add(speciesFact);
                            _referencesParagraphSpeciesFact = speciesFact;
                            break;
                        case (Int32)FactorId.SpeciesInformationDocumentAuthorAndYear:
                            speciesFacts.Add(speciesFact);
                            _authorParagraphSpeciesFact = speciesFact;
                            break;
                        case (Int32)FactorId.SpeciesInformationDocumentItalicsInReferences:
                            speciesFacts.Add(speciesFact);
                            _italicsInReferencesParagraphSpeciesFact = speciesFact;
                            break;
                        case (Int32)FactorId.SpeciesInformationDocumentItalicsInText:
                            speciesFacts.Add(speciesFact);
                            _italicsInTextParagraphSpeciesFact = speciesFact;
                            break;
                        case (Int32)FactorId.SpeciesInformationDocumentIsPublishable:
                            speciesFacts.Add(speciesFact);
                            _isPublishable = speciesFact;
                            break;
                    }
                }

                // Make sure that only species facts that belong 
                // to this SpeciesInformationDocument is handled.
                _speciesFacts = speciesFacts;
            }
        }
    }
}

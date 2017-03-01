using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions;
using Resources;
using TaxonList = ArtDatabanken.Data.TaxonList;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    using FactorFieldDataTypeId = FactorFieldDataTypeId;

    /// <summary>
    /// Class that handles dyntaxa taxon species fact.
    /// </summary>
    public class SpeciesFactModelManager : ISpeciesFactModelManager
    {
        #region PRIVATE FIELDS

        /// <summary>
        /// The species fact no value set.
        /// </summary>
        private const int SpeciesFactNoValueSetValue = -1000;

        /// <summary>
        /// The taxon.
        /// </summary>
        private ITaxon taxon = null;

        /// <summary>
        /// The factor enum id.
        /// </summary>
        private DyntaxaFactorId factorEnumId = DyntaxaFactorId.NOT_SUPPORTED;

        /// <summary>
        /// The factor data type.
        /// </summary>
        private DyntaxaFactorDataType factorDataType = DyntaxaFactorDataType.NOT_SUPPORTED;

        /// <summary>
        /// The data type.
        /// </summary>
        private DyntaxaDataType dataType = DyntaxaDataType.NOT_SUPPORTED;

        /// <summary>
        /// The factor type id.
        /// </summary>
        public int FactorTypeId = 0;

        /// <summary>
        /// The user context.
        /// </summary>
        private IUserContext userContext = null;

        /// <summary>
        /// The species facts.
        /// </summary>
        private SpeciesFactList speciesFacts = new SpeciesFactList();

        /// <summary>
        /// The species facts occurance quality list.
        /// </summary>
        private SpeciesFactQualityList speciesFactsOccuranceQualityList = new SpeciesFactQualityList();

        /// <summary>
        /// The species facts history quality list.
        /// </summary>
        private SpeciesFactQualityList speciesFactsHistoryQualityList = new SpeciesFactQualityList();

        /// <summary>
        /// The swedish occurence fact.
        /// </summary>
        private SpeciesFact swedishOccurenceFact;

        /// <summary>
        /// The swedish history fact.
        /// </summary>
        private SpeciesFact swedishHistoryFact;

        /// <summary>
        /// The quality in dyntaxa.
        /// </summary>
        private SpeciesFact qualityInDyntaxa;

        /// <summary>
        /// The number of swedish species.
        /// </summary>
        private SpeciesFact numberOfSwedishSpecies;

        /// <summary>
        /// The bannded for reporting.
        /// </summary>
        private SpeciesFact banndedForReporting;

        /// <summary>
        /// The exclude from reporting system.
        /// </summary>
        private SpeciesFact excludeFromReportingSystem;

        /// <summary>
        /// The swedish occurrence list.
        /// </summary>
        private IList<FactorFieldEnumValue> swedishOccurrenceList = new List<FactorFieldEnumValue>();

        /// <summary>
        /// The swedish occurrence quality list.
        /// </summary>
        private IList<SpeciesFactQuality> swedishOccurrenceQualityList = new List<SpeciesFactQuality>();

        /// <summary>
        /// The swedish history list.
        /// </summary>
        private IList<FactorFieldEnumValue> swedishHistoryList = new List<FactorFieldEnumValue>();

        /// <summary>
        /// The swedish history quality list.
        /// </summary>
        private IList<SpeciesFactQuality> swedishHistoryQualityList = new List<SpeciesFactQuality>();

        /// <summary>
        /// The quality list.
        /// </summary>
        private IList<FactorFieldEnumValue> qualityList = new List<FactorFieldEnumValue>();

        #endregion

#if DEBUG

        /// <summary>
        /// The dyntaxa reference.
        /// </summary>
        private readonly int dyntaxaReference = DyntaxaSettings.Default.DyntaxaDefaultReferenceIdMoneses;
                    
#else
        private readonly int dyntaxaReference = Resources.DyntaxaSettings.Default.DyntaxaDefaultReferenceId;
        
#endif
        #region PUBLIC PROPERTIES

        /// <summary>
        /// Gets the species fact no value set.
        /// </summary>
        public static int SpeciesFactNoValueSet
        {
            get { return SpeciesFactNoValueSetValue; }
        }

        /// <summary>
        /// Sets the taxon.
        /// </summary>
        public ITaxon Taxon
        {
            set { taxon = value; }
        }

        /// <summary>
        /// Gets the factor enum id.
        /// </summary>
        public DyntaxaFactorId FactorEnumId
        {
            get { return factorEnumId; }
        }

        /// <summary>
        /// Gets the data type.
        /// </summary>
        public DyntaxaDataType DataType
        {
            get { return dataType; }
        }

        /// <summary>
        /// Gets the factor data type.
        /// </summary>
        public DyntaxaFactorDataType FactorDataType
        {
            get { return factorDataType; }
        }

        /// <summary>
        /// Gets the species fact.
        /// </summary>
        public SpeciesFactList SpeciesFact
         {
             get
             {
                 return speciesFacts;
             }
         }

        /// <summary>
        /// Gets the swedish occurrence species fact.
        /// </summary>
        public SpeciesFact SwedishOccurrenceSpeciesFact
        {
            get
            {
                return swedishOccurenceFact;                    
            }
        }

        /// <summary>
        /// Gets the swedish history species fact.
        /// </summary>
        public SpeciesFact SwedishHistorySpeciesFact
        {
            get
            {
                return swedishHistoryFact;
            }
        }

        /// <summary>
        /// Gets the swedish occurrence.
        /// </summary>
        public FactorFieldEnumValue SwedishOccurrence
        {
            get
            {
                if (swedishOccurenceFact == null)
                {
                    return null;
                }

                FactorFieldEnumValue val = swedishOccurenceFact.MainField.Value as FactorFieldEnumValue;
                if (val != null)
                {
                    return val;
                }

                return null;
            }
        }

        /// <summary>
        /// Sets the swedish occurrence id.
        /// </summary>
        public int? SwedishOccurrenceId
        {
            set
            {
                if (value.IsNotNull())
                {
                    FactorId factorId = FactorId.SwedishOccurrence;
                    int index = 0;                                        
                    FactorFieldEnumValueList valList = swedishOccurenceFact.Factor.DataType.MainField.Enum.Values;
                    FactorFieldEnumValue enumValue = valList.Cast<FactorFieldEnumValue>().FirstOrDefault(val => val.Id.Equals(value));

                    SetNewValueForSpeciesFactForTaxon(factorId, index, enumValue);
                }
            }
        }

        /// <summary>
        /// Gets the swedish occurrence list.
        /// </summary>
        public IList<FactorFieldEnumValue> SwedishOccurrenceList
        {
            get
            {
               return swedishOccurrenceList;
            }
        }

        /// <summary>
        /// Gets the swedish occurrence quality.
        /// </summary>
        public ISpeciesFactQuality SwedishOccurrenceQuality
        {
            get
            {
                if (swedishOccurenceFact == null)
                {
                    return null;
                }

                ISpeciesFactQuality val = swedishOccurenceFact.Quality;
                if (val != null)
                {
                    return val;
                }

                return null;
            }
        }

        /// <summary>
        /// Sets the swedish occurrence quality id.
        /// </summary>
        public int? SwedishOccurrenceQualityId
        {
            set
            {
                if (value.IsNotNull())
                {
                    FactorId factorId = FactorId.SwedishOccurrence;
                    int index = 0;
                    SpeciesFactQuality qualityValue = swedishOccurrenceQualityList.Cast<SpeciesFactQuality>().FirstOrDefault(val => val.Id.Equals(value));

                    SetNewValueForSpeciesFactForTaxon(factorId, index, qualityValue);
                }
            }
        }

        /// <summary>
        /// Gets the swedish occurrence quality list.
        /// </summary>
        public IList<SpeciesFactQuality> SwedishOccurrenceQualityList
        {
            get
            {
                return swedishOccurrenceQualityList;
            }
        }

        /// <summary>
        /// Gets or sets the swedish occurrence description.
        /// </summary>
        public string SwedishOccurrenceDescription
        {
            get
            {
                if (swedishOccurenceFact == null)
                {
                    return "-";
                }

                string val = swedishOccurenceFact.Field5.StringValue;
                if (val != null)
                {
                    return val;
                }

                return string.Empty;
            }

            set
            {
                FactorId factorId = FactorId.SwedishOccurrence;
                int index = 0;
                SetNewValueForSpeciesFactForTaxon(factorId, index, value);
            }
        }

        /// <summary>
        /// Gets the swedish occurrence reference.
        /// </summary>
        public IReference SwedishOccurrenceReference
        {
            get
            {
                if (swedishOccurenceFact == null)
                {
                    return null;
                }

                IReference val = swedishOccurenceFact.Reference;
                if (val != null)
                {
                    return val;
                }

                return null;
            }
        }

        /// <summary>
        /// Sets the swedish occurrence reference id.
        /// </summary>
        public int? SwedishOccurrenceReferenceId
        {
            set
            {
                if (value.IsNotNull())
                {
                    FactorId factorId = FactorId.SwedishOccurrence;
                    int index = 0;
                    IReference referenceValue = CoreData.ReferenceManager.GetReference(userContext, value.Value);
                    SetNewValueForSpeciesFactForTaxon(factorId, index, referenceValue);
                }
            }
        }

        /// <summary>
        /// Gets the swedish history.
        /// </summary>
        public FactorFieldEnumValue SwedishHistory
        {
            get
            {
                if (swedishHistoryFact == null)
                {
                    return null;
                }

                FactorFieldEnumValue val = swedishHistoryFact.MainField.Value as FactorFieldEnumValue;
                if (val != null)
                {
                    return val;
                }

                return null;
            }
        }

        /// <summary>
        /// Sets the swedish history id.
        /// </summary>
        public int? SwedishHistoryId
        {
            set
            {
                int index = 0;
                FactorId factorId = FactorId.SwedishHistory;
                FactorFieldEnumValueList valList = swedishHistoryFact.Factor.DataType.MainField.Enum.Values;
                FactorFieldEnumValue enumValue = null;
                if (value.IsNotNull() && value != 0)
                {
                    enumValue = valList.Cast<FactorFieldEnumValue>().FirstOrDefault(val => val.Id.Equals(value));
                    SetNewValueForSpeciesFactForTaxon(factorId, index, enumValue);
                }
                else
                {
                    SetNewNullValueForSpeciesFactForTaxon(factorId, index, "FactorFieldEnumValue");
                }
            }
        }

        /// <summary>
        /// Gets the swedish history list.
        /// </summary>
        public IList<FactorFieldEnumValue> SwedishHistoryList
        {
            get
            {
                return swedishHistoryList;
            }
        }

        /// <summary>
        /// Gets the swedish history quality.
        /// </summary>
        public ISpeciesFactQuality SwedishHistoryQuality
        {
            get
            {
                if (swedishOccurenceFact == null)
                {
                    return null;
                }

                ISpeciesFactQuality val = swedishHistoryFact.Quality;
                if (val != null)
                {
                    return val;
                }

                return null;
            }
        }

        /// <summary>
        /// Sets the swedish history quality id.
        /// </summary>
        public int? SwedishHistoryQualityId
        {
            set
            {
                FactorId factorId = FactorId.SwedishHistory;
                int index = 0;
                SpeciesFactQuality qualityValue = null;
                if (value.IsNotNull() && value != 0)
                {
                    qualityValue = swedishHistoryQualityList.Cast<SpeciesFactQuality>().FirstOrDefault(val => val.Id.Equals(value));
                    SetNewValueForSpeciesFactForTaxon(factorId, index, qualityValue);
                }
                else
                {
                    SetNewNullValueForSpeciesFactForTaxon(factorId, index, "SpeciesFactQuality");
                }
            }
        }

        /// <summary>
        /// Gets the swedish history quality list.
        /// </summary>
        public IList<SpeciesFactQuality> SwedishHistoryQualityList
        {
            get
            {
              return swedishHistoryQualityList;
            }
        }

        /// <summary>
        /// Gets the swedish history reference.
        /// </summary>
        public IReference SwedishHistoryReference
        {
            get
            {
                if (swedishHistoryFact == null)
                {
                    return null;
                }

                IReference val = swedishHistoryFact.Reference;
                if (val != null)
                {
                    return val;
                }

                return null;
            }
        }

        /// <summary>
        /// Sets the swedish history reference id.
        /// </summary>
        public int? SwedishHistoryReferenceId
        {
            set
            {
                FactorId factorId = FactorId.SwedishHistory;
                int index = 0;
                IReference referenceValue = null;
                if (value.IsNotNull() && value != 0)
                {
                    referenceValue = CoreData.ReferenceManager.GetReference(userContext, value.Value);
                    SetNewValueForSpeciesFactForTaxon(factorId, index, referenceValue);
                }
                else
                {
                    SetNewNullValueForSpeciesFactForTaxon(factorId, index, "Reference");
                }
            }
        }

        /// <summary>
        /// Gets or sets the swedish history description.
        /// </summary>
        public string SwedishHistoryDescription
        {
            get
            {
                if (swedishHistoryFact == null)
                {
                    return "-";
                }

                string val = swedishHistoryFact.Field5.StringValue;
                if (val != null)
                {
                    return val;
                }

                return string.Empty;
            }

            set
            {
                FactorId factorId = FactorId.SwedishHistory;
                int index = 0;
                if (value != null)
                {
                    SetNewValueForSpeciesFactForTaxon(factorId, index, value);
                }
                else
                {
                    SetNewNullValueForSpeciesFactForTaxon(factorId, index, "string");
                }
            }
        }

        /// <summary>
        /// Gets the quality status.
        /// </summary>
        public IFactorFieldEnumValue QualityStatus
        {
            get
            {
                if (qualityInDyntaxa == null)
                {
                    return null;
                }

                IFactorFieldEnumValue val = qualityInDyntaxa.MainField.EnumValue;
                if (val != null)
                {
                    return val;
                }

                return null;
            }
        }

        /// <summary>
        /// Sets the quality status id.
        /// </summary>
        public int? QualityStatusId
        {           
            set
            {
                if (value.IsNotNull())
                {
                    FactorId factorId = FactorId.QualityInDyntaxa;
                    int index = 0;
                    FactorFieldEnumValueList valList = qualityInDyntaxa.Factor.DataType.MainField.Enum.Values;
                    FactorFieldEnumValue enumValue = valList.Cast<FactorFieldEnumValue>().FirstOrDefault(val => val.Id.Equals(value));
                    SetNewValueForSpeciesFactForTaxon(factorId, index, enumValue);
                }
            }
        }

        /// <summary>
        /// Gets the quality status list.
        /// </summary>
        public IList<FactorFieldEnumValue> QualityStatusList
        {
            get
            {
                return qualityList;
            }
        }

        /// <summary>
        /// Gets or sets the quality description.
        /// </summary>
        public string QualityDescription
        {
            get
            {
                if (qualityInDyntaxa == null)
                {
                    return "-";
                }

                string val = qualityInDyntaxa.Field5.StringValue;
                if (val != null)
                {
                    return val;
                }

                return string.Empty;
            }

            set
            {
                FactorId factorId = FactorId.QualityInDyntaxa;
                int index = 0;
                SetNewValueForSpeciesFactForTaxon(factorId, index, value);
            }
        }

        ///// <summary>
        ///// Gets or sets the number of swedish species.
        ///// </summary>
        //public int? NumberOfSwedishSpecies
        //{
        //    get
        //    {
        //        return numberOfSwedishSpecies == null ? 0 : numberOfSwedishSpecies.MainField.Int32Value;
        //    }

        //    set
        //    {
        //        if (value.IsNotNull())
        //        {
        //            FactorId factorId = FactorId.NumberOfSwedishSpecies;
        //            int index = 0;
        //            SetNewValueForSpeciesFactForTaxon(factorId, index, value);
        //        }
        //    }
        //}

        /// <summary>
        /// Gets or sets a value indicating whether bannded for reporting.
        /// </summary>
        public bool BanndedForReporting
        {
            get
            {
                if (banndedForReporting == null)
                {
                    return false;
                }

                return banndedForReporting.MainField.BooleanValue;
            }

            set
            {
                if (value.IsNotNull())
                {
                    FactorId factorId = FactorId.BanndedForReporting;
                    int index = 0;
                    SetNewValueForSpeciesFactForTaxon(factorId, index, value);

                    // Must set that Dyntaxa user hase done this update..
                    IReference referenceValue = CoreData.ReferenceManager.GetReference(userContext, dyntaxaReference);
                    SetNewValueForSpeciesFactForTaxon(factorId, index, referenceValue);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether exclude from reporting system.
        /// </summary>
        public bool ExcludeFromReportingSystem
        {
            get
            {
                if (excludeFromReportingSystem == null)
                {
                    return false;
                }

                return excludeFromReportingSystem.MainField.BooleanValue;
            }

            set
            {
                if (value.IsNotNull())
                {
                    FactorId factorId = FactorId.ExcludeFromReportingSystem;
                    int index = 0;
                    SetNewValueForSpeciesFactForTaxon(factorId, index, value);

                    // Must set that Dyntaxa user hase done this update..
                    IReference referenceValue = CoreData.ReferenceManager.GetReference(userContext, dyntaxaReference);
                    SetNewValueForSpeciesFactForTaxon(factorId, index, referenceValue);
                }
            }
        }
        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for species fact for loggedInUser.
        /// </summary>
        /// <param name="loggedInUser">
        /// </param>
        public SpeciesFactModelManager(IUserContext loggedInUser)
        {
            userContext = loggedInUser;
        }

        /// <summary>
        /// Constructor for species fact for a specific taxon.
        /// </summary>
        /// <param name="taxon">
        /// </param>
        /// <param name="loggedInUser">
        /// </param>
        public SpeciesFactModelManager(ITaxon taxon, IUserContext loggedInUser)
        {
            this.taxon = taxon;
            userContext = loggedInUser;
            speciesFacts = GetSpeciesFactList();
            speciesFactsOccuranceQualityList = CoreData.SpeciesFactManager.GetSpeciesFactQualities(userContext);
            speciesFactsHistoryQualityList = CoreData.SpeciesFactManager.GetSpeciesFactQualities(userContext);

            // Create all used lists
            foreach (SpeciesFactQuality val in speciesFactsOccuranceQualityList)
            {
                swedishOccurrenceQualityList.Add(val);
            }

            foreach (SpeciesFactQuality val in speciesFactsHistoryQualityList)
            {
                swedishHistoryQualityList.Add(val);
            }

            if (swedishOccurenceFact.IsNotNull())
            {
                FactorFieldEnumValueList valList = swedishOccurenceFact.Factor.DataType.MainField.Enum.Values;
                swedishOccurrenceList = new List<FactorFieldEnumValue>();
                foreach (FactorFieldEnumValue val in valList)
                {
                    swedishOccurrenceList.Add(val);
                }
            }

            if (swedishHistoryFact.IsNotNull())
            {
                FactorFieldEnumValueList valList = swedishHistoryFact.Factor.DataType.MainField.Enum.Values;
                swedishHistoryList = new List<FactorFieldEnumValue>();
                foreach (FactorFieldEnumValue val in valList)
                {
                    swedishHistoryList.Add(val);
                }
            }

            if (qualityInDyntaxa.IsNotNull())
            {
                FactorFieldEnumValueList valList = qualityInDyntaxa.Factor.DataType.MainField.Enum.Values;
                qualityList = new List<FactorFieldEnumValue>();
                foreach (FactorFieldEnumValue val in valList)
                {
                    qualityList.Add(val);
                }
            }
        }

        /// <summary>
        /// Constructor for species fact for a logged in user.
        /// </summary>
        /// <param name="loggedInUser">
        /// </param>
        /// <param name="taxon">
        /// The taxon.
        /// </param>
        /// <param name="allFacts">
        /// The all Facts.
        /// </param>
        public SpeciesFactModelManager(IUserContext loggedInUser, ITaxon taxon, bool allFacts)
        {
            userContext = loggedInUser;
            this.taxon = taxon;
        }

        /// <summary>
        /// Constructor for species fact for a logged in user.
        /// </summary>
        /// <param name="loggedInUser">
        /// </param>
        /// <param name="taxon">
        /// The taxon.
        /// </param>
        /// <param name="factorEnumId">
        /// The factor Enum Id.
        /// </param>
        /// <param name="dataType">
        /// The data Type.
        /// </param>
        /// <param name="factorDataType">
        /// The factor Data Type.
        /// </param>
        public SpeciesFactModelManager(IUserContext loggedInUser, ITaxon taxon, int factorEnumId, int dataType, int factorDataType)
        {
            userContext = loggedInUser;
            this.taxon = taxon;

            // Here we assign all incomming data and factor parameters
            if (Enum.IsDefined(typeof(DyntaxaFactorId), factorEnumId))
            {
                this.factorEnumId = (DyntaxaFactorId)factorEnumId;
            }

            if (Enum.IsDefined(typeof(DyntaxaFactorDataType), factorDataType))
            {
                this.factorDataType = (DyntaxaFactorDataType)factorDataType;
            }

            if (Enum.IsDefined(typeof(DyntaxaDataType), dataType))
            {
                this.dataType = (DyntaxaDataType)dataType;
            }
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Adds a new factor to selected taxon.
        /// </summary>
        /// <param name="taxonId">
        /// Id of selected taxon.
        /// </param>
        /// <param name="factorToBeAddedId">
        /// Id of selected factor.
        /// </param>
        /// <param name="referenceId">
        /// The reference Id.
        /// </param>
        /// <param name="mainParentFactorId">
        /// The main Parent Factor Id.
        /// </param>
        /// <param name="setHost">
        /// The set Host.
        /// </param>
        /// <param name="hostTaxonId">
        /// The host Taxon Id.
        /// </param>
        /// <param name="individualCategory">
        /// The individual Category.
        /// </param>
        public void AddFactorToTaxon(int taxonId, int factorToBeAddedId, string referenceId, int mainParentFactorId, bool setHost = false, int hostTaxonId = 0, int individualCategory = 0)
        {
            SpeciesFactList speciesFactToUpdateList = null;
            ISpeciesFact speciesFactToUpdate = null;
            ISpeciesFactSearchCriteria searchCriteriaNoReference = new SpeciesFactSearchCriteria();
            searchCriteriaNoReference.EnsureNoListsAreNull();
            searchCriteriaNoReference.IncludeNotValidHosts = true;
            searchCriteriaNoReference.IncludeNotValidTaxa = true;

            // Add factor.
            IFactor factor = GetFactor(userContext, factorToBeAddedId);
            searchCriteriaNoReference.Factors = new FactorList();
            searchCriteriaNoReference.Factors.Add(factor);

            // Add taxon.
            ITaxon speciesFactsTaxon = CoreData.TaxonManager.GetTaxon(userContext, taxonId);
            searchCriteriaNoReference.Taxa = new TaxonList();
            searchCriteriaNoReference.Taxa.Add(speciesFactsTaxon);

            // SetIndividualCategory.
            searchCriteriaNoReference.IndividualCategories = new IndividualCategoryList();
            IIndividualCategory category = CoreData.FactorManager.GetIndividualCategory(userContext, individualCategory);
            searchCriteriaNoReference.IndividualCategories.Add(category);

            // SetReference
            int tempReferenceId = dyntaxaReference;
            if (referenceId.IsNotNull() && referenceId != string.Empty && Convert.ToInt32(referenceId) != 0)
            {
                tempReferenceId = Convert.ToInt32(referenceId);
            }

            IReference reference = CoreData.ReferenceManager.GetReference(userContext, tempReferenceId);
            searchCriteriaNoReference.Hosts = new TaxonList();
            if (setHost)
            {
                ITaxon tempHostTaxon = CoreData.TaxonManager.GetTaxon(userContext, hostTaxonId);
                searchCriteriaNoReference.Hosts.Add(tempHostTaxon);
            }

            // Don't set any periods
            searchCriteriaNoReference.Periods = new PeriodList();

            // First we must check if there exist data in Db for this factor. If so we reload that factor with all of its values.
            try
            {
                // Getting user parameter selections without any references. 
                SpeciesFactList existingSpeciesFactList = CoreData.SpeciesFactManager.GetDyntaxaSpeciesFacts(userContext, searchCriteriaNoReference);

                if (existingSpeciesFactList.IsNotNull() && existingSpeciesFactList.Count == 1 && existingSpeciesFactList[0].HasId)
                {
                    // Yes, we have an object in db. use it and update reference
                    ISpeciesFact existingSpeciesFacts = existingSpeciesFactList[0];
                    FactorFieldEnumValueList valList = existingSpeciesFacts.Factor.DataType.MainField.Enum.Values;
                    FactorFieldEnumValue enumValue = valList.Cast<FactorFieldEnumValue>().FirstOrDefault(val => val.KeyInt == 1);
                    existingSpeciesFacts.MainField.Value = enumValue;
                    if (referenceId.IsNotNull() && referenceId != string.Empty && Convert.ToInt32(referenceId) != 0)
                    {
                        existingSpeciesFacts.Reference = CoreData.ReferenceManager.GetReference(userContext, Convert.ToInt32(referenceId));
                    }
                    else
                    {
                        existingSpeciesFacts.Reference = CoreData.ReferenceManager.GetReference(userContext, dyntaxaReference);
                    }

                    speciesFactToUpdate = existingSpeciesFacts;
                }
                else if (existingSpeciesFactList.IsNotNull() && existingSpeciesFactList.Count == 1 && !existingSpeciesFactList[0].HasId)
                {
                    // Now we have a new speciesfact in the onion and not saved
                    ISpeciesFact fact = existingSpeciesFactList[0];

                    FactorFieldEnumValueList valList = fact.Factor.DataType.MainField.Enum.Values;
                    FactorFieldEnumValue enumValue = valList.Cast<FactorFieldEnumValue>().FirstOrDefault(val => val.KeyInt == 1);
                    fact.MainField.Value = enumValue;

                    foreach (ISpeciesFactField field in fact.Fields)
                    {
                        // Here we identify factor "nyttjande" used by Substrate
                        if (field.Type.DataType == FactorFieldDataTypeId.Enum && !field.IsMain
                            && (mainParentFactorId == (int)DyntaxaFactorId.SUBSTRATE))
                        {
                            // Here we get the list of all avalible enum values
                            // FactorFieldEnumValueList valList2 = field.FactorFieldEnum.Values;
                            // field.Value = valList2.Cast<FactorFieldEnumValue>().FirstOrDefault(val => val.KeyHasIntegerValue && val.KeyInt == 1);
                            field.Value = null;
                        }

                        // Here we identify factor "Faktorn relevans " used by Infuence
                        if (field.Type.DataType == FactorFieldDataTypeId.Enum && !field.IsMain
                            && (mainParentFactorId == (int)DyntaxaFactorId.INFLUENCE))
                        {
                            // Here we get the list of all avalible enum values
                            FactorFieldEnumValueList valList2 = field.FactorFieldEnum.Values;
                            field.Value = valList2.Cast<FactorFieldEnumValue>().FirstOrDefault(val => val.KeyInt.HasValue && val.KeyInt.Value == 1);

                            // Updated 2014-02-04
                        }
                    }

                    if (referenceId.IsNotNull() && referenceId != string.Empty && Convert.ToInt32(referenceId) != 0)
                    {
                        fact.Reference = CoreData.ReferenceManager.GetReference(userContext, Convert.ToInt32(referenceId));
                    }
                    else
                    {
                        fact.Reference = CoreData.ReferenceManager.GetReference(userContext, dyntaxaReference);
                    }

                    if (fact.Quality == null)
                    {
                        fact.Quality = new SpeciesFactQuality();
                    }

                    fact.Quality = CoreData.SpeciesFactManager.GetSpeciesFactQuality(userContext, 3);
                    speciesFactToUpdate = fact;
                }
                else
                {
                    throw new ApplicationException(DyntaxaResource.SpeciesFactEditHostFactorItemsErrorText);
                }

                // Updated factor
                speciesFactToUpdateList = new SpeciesFactList();
                speciesFactToUpdateList.Add(speciesFactToUpdate);
                UpdateSpeciesFacts(speciesFactToUpdateList);
            }
            catch (Exception ex)
            {
                if (speciesFactToUpdateList.IsNotNull())
                {
                    speciesFactToUpdateList = null;
                }

                DyntaxaLogger.WriteMessage("AddFactorToTaxon: " + ex.Message);
                throw new ApplicationException(DyntaxaResource.SpeciesFactEditHostFactorItemsErrorText);
            }
        }

        /// <summary>
        /// Adds a new factor to selected taxon.
        /// </summary>
        /// <param name="taxonId">
        /// Id of selected taxon.
        /// </param>
        /// <param name="factorId">
        /// Id of selected factor.
        /// </param>
        /// <param name="individualCategory">
        /// The individual Category.
        /// </param>
        /// <param name="qualityId">
        /// The quality Id.
        /// </param>
        /// <param name="referenceId">
        /// The reference Id.
        /// </param>
        /// <param name="mainParentFactorId">
        /// The main Parent Factor Id.
        /// </param>
        /// <param name="hostId">
        /// The host Id.
        /// </param>
        public void AddNewCategoryToSpeciecFact(int taxonId, int factorId, int individualCategory, int qualityId, string referenceId, int mainParentFactorId, int hostId)
        {
            SpeciesFactList speciesFacts = null;
            ITaxon hostTaxon = null;

            ISpeciesFactSearchCriteria searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.EnsureNoListsAreNull();
            searchCriteria.IncludeNotValidHosts = true;
            searchCriteria.IncludeNotValidTaxa = true;

            // Add factor.
            IFactor factor = GetFactor(userContext, factorId);
            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.Add(factor);
            
            // Add taxon.
            ITaxon speciesFactsTaxon = CoreData.TaxonManager.GetTaxon(userContext, taxonId);
            searchCriteria.Taxa = new TaxonList();
            searchCriteria.Taxa.Add(speciesFactsTaxon);
            
            // SetIndividualCategory.
            searchCriteria.IndividualCategories = new IndividualCategoryList();
            IIndividualCategory category = CoreData.FactorManager.GetIndividualCategory(userContext, individualCategory);
            searchCriteria.IndividualCategories.Add(category);
                        
            // Don't set any hosts or periods
            searchCriteria.Hosts = new TaxonList();
            hostTaxon = CoreData.TaxonManager.GetTaxon(userContext, hostId);
            searchCriteria.Hosts.Add(hostTaxon);

            searchCriteria.Periods = new PeriodList();
            try
            {
                speciesFacts = CoreData.SpeciesFactManager.GetDyntaxaSpeciesFacts(userContext, searchCriteria);                        
                ISpeciesFact fact = speciesFacts[0];                
                FactorFieldEnumValueList valList = fact.Factor.DataType.MainField.Enum.Values;
                FactorFieldEnumValue enumValue = valList.Cast<FactorFieldEnumValue>().FirstOrDefault(val => val.KeyInt == 1);
                fact.MainField.Value = enumValue;
                foreach (ISpeciesFactField field in fact.Fields)
                {
                    // Here we identify factor "nyttjande" used by Substrate.
                    if (field.Type.DataType == FactorFieldDataTypeId.Enum 
                     && !field.IsMain
                     && (mainParentFactorId == (int)DyntaxaFactorId.SUBSTRATE
                     || mainParentFactorId == (int)DyntaxaFactorId.INFLUENCE))
                    {
                        field.Value = null;
                    }
                }

                if (referenceId.IsNotNull() && referenceId != string.Empty && Convert.ToInt32(referenceId) != 0)
                {
                    fact.Reference = CoreData.ReferenceManager.GetReference(userContext, Convert.ToInt32(referenceId));
                }
                else
                {
                    fact.Reference = CoreData.ReferenceManager.GetReference(userContext, dyntaxaReference);
                }

                fact.Quality = CoreData.SpeciesFactManager.GetSpeciesFactQuality(userContext, qualityId);

                // Create new speciesFact.
                UpdateSpeciesFacts(speciesFacts);               
            }
            catch (Exception ex)
            {
                if (speciesFacts.IsNotNull())
                {
                    speciesFacts = null;
                }

                DyntaxaLogger.WriteMessage("AddNewCategoryToSpeciecFact: " + ex.Message);
                throw new ApplicationException(DyntaxaResource.SharedError);
            }
        }

        /// <summary>
        /// Gets species fact  and factors for a specific taxon from artdatabanken service.
        /// </summary>
        /// <returns>
        /// The <see cref="DyntaxaAllFactorData"/>.
        /// </returns>
        public DyntaxaAllFactorData GetAllSpeciesFact()
        {
            ITaxon speciesFactsTaxon = taxon;
            speciesFacts = new SpeciesFactList();
            
            // Get species fact for selected taxon 
            speciesFacts = GetSpeciesFacts(speciesFactsTaxon);

            // Must sort the list
            speciesFacts.Sort();

            IList<DyntaxaSpeciesFact> dyntaxaSpeciesFactForATaxon = new List<DyntaxaSpeciesFact>();
            string swedishOccuranceInfo = string.Empty;
            string swedishHistoryInfo = string.Empty;

            // Convert species fact list to a DyntaxaSpeciesFact list.
            foreach (ISpeciesFact spFactor in speciesFacts)
            {
                if (spFactor.Factor.Id == (int)FactorId.SwedishOccurrence)
                {
                    if (spFactor.MainField.IsNotNull() && spFactor.MainField.HasValue)
                    {
                        swedishOccuranceInfo = spFactor.MainField.EnumValue.OriginalLabel;
                    }
                }

                if (spFactor.Factor.Id == (int)FactorId.SwedishHistory)
                {
                    if (spFactor.MainField.IsNotNull() && spFactor.MainField.HasValue)
                    {
                        swedishHistoryInfo = spFactor.MainField.EnumValue.OriginalLabel;
                    }
                }

                DyntaxaFactorUpdateMode dyntaxaFactorUpdateMode = new DyntaxaFactorUpdateMode(spFactor.Factor.UpdateMode.IsHeader, true);

                string factorOriginName = string.Empty;
                int factorOriginId = -1;
                DyntaxaFactorOrigin factorOrigin = new DyntaxaFactorOrigin(factorOriginId, factorOriginName);

                if (spFactor.Factor.Origin.IsNotNull() && (spFactor.Factor.Origin.Id > -1))
                {
                    factorOrigin.OriginName = spFactor.Factor.Origin.Name;                        
                    factorOrigin.OriginId = spFactor.Factor.Origin.Id;
                }

                // Convert SpeciesFactField to DyntaxaFactorField
                IList<DyntaxaFactorField> fields = new List<DyntaxaFactorField>();
                foreach (ISpeciesFactField field in spFactor.Fields)
                {
                    DyntaxaFactorFieldDataTypeId dyntaxaFactorFieldDataTypeId = DyntaxaFactorFieldDataTypeId.String;
                    if (field.Type.DataType == FactorFieldDataTypeId.Boolean)
                    {
                        dyntaxaFactorFieldDataTypeId = DyntaxaFactorFieldDataTypeId.Boolean;
                    }

                    if (field.Type.DataType == FactorFieldDataTypeId.Double)
                    {
                        dyntaxaFactorFieldDataTypeId = DyntaxaFactorFieldDataTypeId.Double;
                    }

                    if (field.Type.DataType == FactorFieldDataTypeId.Enum)
                    {
                        dyntaxaFactorFieldDataTypeId = DyntaxaFactorFieldDataTypeId.Enum;
                    }

                    if (field.Type.DataType == FactorFieldDataTypeId.Int32)
                    {
                        dyntaxaFactorFieldDataTypeId = DyntaxaFactorFieldDataTypeId.Int32;
                    }

                    DyntaxaFactorField item = new DyntaxaFactorField(
                        field.Id.ToString(), 
                        field.Label, 
                        field.Unit, 
                        dyntaxaFactorFieldDataTypeId, 
                        field.Value, 
                        field.HasValue, 
                        null, 
                        field.IsMain);
                    fields.Add(item);
                }

                DyntaxaFactorQuality quality = new DyntaxaFactorQuality(spFactor.Quality.Id, spFactor.Quality.Name, null);

                DyntaxaIndividualCategory individualCategory = null;
                if (spFactor.IndividualCategory.IsNotNull())
                {
                    individualCategory = new DyntaxaIndividualCategory(spFactor.IndividualCategory.Id, spFactor.IndividualCategory.Name, string.Empty);
                }
                else
                {
                    IIndividualCategory dafultCat = CoreData.FactorManager.GetDefaultIndividualCategory(userContext);
                    individualCategory = new DyntaxaIndividualCategory(dafultCat.Id, dafultCat.Name, string.Empty);
                }

                int hostId = -1;
                if (spFactor.HasHost)
                {
                    hostId = spFactor.Host.Id;
                }

                DyntaxaSpeciesFact dyntaxaSpeciesFact = new DyntaxaSpeciesFact(
                    spFactor.Id.ToString(), 
                    spFactor.Factor.Information, 
                    spFactor.Factor.Label, 
                    spFactor.Factor.IsLeaf, 
                    spFactor.Factor.IsPeriodic, 
                    spFactor.Factor.SortOrder, 
                    spFactor.Factor.IsPublic, 
                    spFactor.Factor.IsTaxonomic, 
                    spFactor.Factor.HostLabel, 
                    hostId, 
                    quality, 
                    spFactor.Reference.Id, 
                    fields, 
                    factorOrigin, 
                    dyntaxaFactorUpdateMode, 
                    spFactor.Identifier, 
                    spFactor.ModifiedDate, 
                    individualCategory, 
                    spFactor.Factor.Id, 
                    spFactor.Reference.Name);

                dyntaxaSpeciesFactForATaxon.Add(dyntaxaSpeciesFact);
            }

            // Get all factors.
            ISpeciesFactSearchCriteria resultParameters = GetResultParameters(speciesFacts);
            FactorList allFactors = GetAllParentFactors(resultParameters.Factors);
            IList<DyntaxaFactor> dyntaxaAllFactors = new List<DyntaxaFactor>();

            // set up all data that are required.
            foreach (IFactor factor in allFactors)
            {
                string factorOriginName = string.Empty;
                int factorOriginId = -1;
                DyntaxaFactorOrigin factorOrigin = new DyntaxaFactorOrigin(factorOriginId, factorOriginName);
                if (factor.Origin.IsNotNull() && (factor.Origin.Id > -1))
                {
                    factorOrigin.OriginName = factor.Origin.Name;
                    factorOrigin.OriginId = factor.Origin.Id;
                }

                DyntaxaFactorUpdateMode dyntaxaFactorUpdateMode = new DyntaxaFactorUpdateMode(factor.UpdateMode.IsHeader, true);
                DyntaxaFactor dyntaxaFactor = new DyntaxaFactor(
                    factor.Id.ToString(), 
                    factor.Label, 
                    factor.IsLeaf, 
                    factor.IsPeriodic, 
                    factor.SortOrder, 
                    factor.IsPublic, 
                    factor.IsTaxonomic, 
                    factorOrigin, 
                    dyntaxaFactorUpdateMode, 
                    factor.Id);
                dyntaxaAllFactors.Add(dyntaxaFactor);
            }

            // Sort and convert Hosts,Periods and Individual categories for all factors.
            resultParameters.Hosts.Sort();
            resultParameters.Periods.Sort();
            resultParameters.IndividualCategories.Sort();

            IList<DyntaxaHost> hostList = new List<DyntaxaHost>();
            foreach (ITaxon host in resultParameters.Hosts)
            {
                hostList.Add(new DyntaxaHost(host.Id.ToString(), host.GetLabel(), host.ScientificName, host.CommonName));
            }

            IList<DyntaxaPeriod> periodList = new List<DyntaxaPeriod>();
            foreach (IPeriod period in resultParameters.Periods)
            {
                periodList.Add(new DyntaxaPeriod(period.Id.ToString(), period.Name));
            }

            IList<DyntaxaIndividualCategory> individualCategoriesList = new List<DyntaxaIndividualCategory>();
            foreach (IIndividualCategory category in resultParameters.IndividualCategories)
            {
                individualCategoriesList.Add(new DyntaxaIndividualCategory(category.Id, category.Name, string.Empty));
            }

            DyntaxaAllFactorData dyntaxaAllFactorData = new DyntaxaAllFactorData(
                dyntaxaAllFactors, 
                hostList, 
                individualCategoriesList, 
                periodList, 
                dyntaxaSpeciesFactForATaxon, 
                speciesFactsTaxon.Id, 
                swedishOccuranceInfo, 
                swedishHistoryInfo, 
                null);

            return dyntaxaAllFactorData;
        }

        /// <summary>
        /// Get DyntaxaSpeciecFact data from factorId, hostId and individual category.
        /// </summary>
        /// <param name="childFactorId">
        /// </param>
        /// <param name="individualCategoryId">
        /// </param>
        /// <param name="hostId">
        /// </param>
        /// <returns>
        /// The <see cref="DyntaxaSpeciesFact"/>.
        /// </returns>
        public DyntaxaSpeciesFact GetSpeciesFactFromTaxonAndFactor(string childFactorId, string individualCategoryId, string hostId)
        {
            // Set default values
            string fieldName = string.Empty;
            int fieldValue = 0;
            string fieldName2 = string.Empty;
            int fieldValue2 = 0;
            string factorComments = string.Empty;
            string existingEvaluations = string.Empty;
            bool newCategory = false;
            DyntaxaSpeciesFact dyntaxaSpeciesFact = null;
            SpeciesFactList speciesFactList = new SpeciesFactList();

            // Get selected taxon and factor
            ITaxon speciesFactsTaxon = taxon;
            IFactor factor = GetFactor(userContext, Convert.ToInt32(childFactorId));

            // get individual categories and se default value on category.            
            IndividualCategoryList catList = CoreData.FactorManager.GetIndividualCategories(userContext);            
            IIndividualCategory selectedCat = CoreData.FactorManager.GetDefaultIndividualCategory(userContext);
            if (individualCategoryId.IsNotEmpty())
            {
                selectedCat = CoreData.FactorManager.GetIndividualCategory(userContext, Convert.ToInt32(individualCategoryId));

                // First we must check if selected category is empty, if so null is returned and this is a new individual category for this factor.
                FactorList factorList = new FactorList();
                factorList.Add(factor);
                IndividualCategoryList selctedCatList = new IndividualCategoryList();
                selctedCatList.Add(selectedCat);
                SpeciesFactList checkSpeciesFacts = GetSpeciesFacts(speciesFactsTaxon, factorList, selctedCatList);
                if (checkSpeciesFacts == null || checkSpeciesFacts.Count == 0)
                {
                    // Set an existing category for collecting all other data required
                    newCategory = true;
                    foreach (IIndividualCategory tempIndividualCategory in catList)
                    {
                        IndividualCategoryList tempCatList = new IndividualCategoryList();
                        tempCatList.Add(tempIndividualCategory);

                        // Get speciesfacts per individual category amd check if there are any data
                        SpeciesFactList tempSpeciesFacts = GetSpeciesFacts(speciesFactsTaxon, factorList, tempCatList);
                        if (tempSpeciesFacts.IsNotEmpty())
                        {
                            selectedCat = tempIndividualCategory;
                            break;
                        }
                    }
                }
            }

            // Get data for existing individual categories ie existing categories.
            ISpeciesFact speciesFact = GetFactorFieldValues(hostId, catList, speciesFactsTaxon, factor, selectedCat, ref factorComments, ref fieldName, ref fieldValue, ref fieldName2, ref fieldValue2, ref existingEvaluations);

            FactorList factorlist = new FactorList();
            factorlist.Add(factor);
               
            ISpeciesFactSearchCriteria resultParameters = null;
            if (speciesFact != null)
            {
                // Convert ArtdatabankebnService SpeciecFact to DyntaxaSpeciesFact
                dyntaxaSpeciesFact = GetDyntaxaSpeciesFactFromSpeciesFact(userContext, speciesFact);
                resultParameters = GetResultParameters(speciesFactList);
                IList<DyntaxaFactor> dyntaxaAllFactors = new List<DyntaxaFactor>();
                
                // Set all required parameters to DyntaxaSpeciesFact.
                if (factorlist.IsNotEmpty() && resultParameters.IsNotNull())
                {
                    IList<DyntaxaPeriod> periodList;
                    IList<DyntaxaIndividualCategory> individualCategoriesList;
                    var hostList = GetAdditionalFactorData(userContext, factorlist, dyntaxaAllFactors, resultParameters, out periodList, out individualCategoriesList);
                    dyntaxaSpeciesFact.HostList = hostList;
                    
                    // Get individual category from factor.
                    if (individualCategoriesList.Count == 0)
                    {
                        individualCategoriesList.Add(new DyntaxaIndividualCategory(speciesFact.IndividualCategory.Id, speciesFact.IndividualCategory.Name, string.Empty));
                    }

                    dyntaxaSpeciesFact.IndividualCategoryList = individualCategoriesList;
                    dyntaxaSpeciesFact.PeriodList = periodList;
                    dyntaxaSpeciesFact.ExistingEvaluations = existingEvaluations;
                    if (newCategory)
                    {
                        dyntaxaSpeciesFact.IndividualCatgory = new DyntaxaIndividualCategory(selectedCat.Id, selectedCat.Name, string.Empty);
                        dyntaxaSpeciesFact.Comments = string.Empty;
                    }
                    else
                    {
                        dyntaxaSpeciesFact.IndividualCatgory = new DyntaxaIndividualCategory(speciesFact.IndividualCategory.Id, speciesFact.IndividualCategory.Name, string.Empty);
                        dyntaxaSpeciesFact.Comments = factorComments;
                    }
                      
                    dyntaxaSpeciesFact.FactorEnumValue = fieldValue;
                    dyntaxaSpeciesFact.FactorEnumLabel = fieldName;
                    dyntaxaSpeciesFact.FactorEnumValue2 = fieldValue2;
                    dyntaxaSpeciesFact.FactorEnumLabel2 = fieldName2;

                    if (speciesFact.Host.IsNotNull() && hostId.IsNotNull())
                    {
                        ITaxon hostTaxon = CoreData.TaxonManager.GetTaxon(userContext, Convert.ToInt32(hostId));
                        if (Convert.ToInt32(hostId) == 0)
                        {
                            dyntaxaSpeciesFact.HostName = speciesFact.Factor.HostLabel + ": ospecificerat";
                        }
                        else
                        {
                            dyntaxaSpeciesFact.HostName = hostTaxon.GetLabel() + " (" + speciesFact.Factor.Label + ")"; 
                        }                        
                    }
                    else
                    {
                        dyntaxaSpeciesFact.HostName = string.Empty;
                    }
                }
            }
            else
            {
                throw new ApplicationException(DyntaxaResource.SpeciesFactEditFactorItemErrorText + factor.Id + ".");
            }

            return dyntaxaSpeciesFact;
        }

        /// <summary>
        /// Gets the requred data from from a specific factor specified by its taxon and indiviual category.
        /// </summary>
        /// <param name="hostId">
        /// </param>
        /// <param name="catList">
        /// </param>
        /// <param name="speciesFactsTaxon">
        /// </param>
        /// <param name="factor">
        /// </param>
        /// <param name="selectedCat">
        /// </param>
        /// <param name="factorComments">
        /// </param>
        /// <param name="fieldName">
        /// </param>
        /// <param name="fieldValue">
        /// </param>
        /// <param name="fieldName2">
        /// The field Name 2.
        /// </param>
        /// <param name="fieldValue2">
        /// The field Value 2.
        /// </param>
        /// <param name="existingEvaluations">
        /// </param>
        /// <returns>
        /// The <see cref="ISpeciesFact"/>.
        /// </returns>
        private ISpeciesFact GetFactorFieldValues(
            string hostId, 
            IndividualCategoryList catList, 
            ITaxon speciesFactsTaxon, 
            IFactor factor, 
            IIndividualCategory selectedCat, 
            ref string factorComments, 
            ref string fieldName, 
            ref int fieldValue, 
            ref string fieldName2, 
            ref int fieldValue2, 
            ref string existingEvaluations)
        {
            ISpeciesFact speciesFact = null;
            foreach (IIndividualCategory individualCategory in catList)
            {
                SpeciesFactList tempSpeciesFacts;

                // Get speciesfacts per individual category
                FactorList factorList = new FactorList();
                factorList.Add(factor);
                IndividualCategoryList categoryList = new IndividualCategoryList();
                categoryList.Add(individualCategory);
                tempSpeciesFacts = GetSpeciesFacts(speciesFactsTaxon, factorList, categoryList);

                // Yes we have a species fact here and it is the selceted category
                if (tempSpeciesFacts.IsNotEmpty() && tempSpeciesFacts.Count == 1)
                {
                    if (selectedCat.Id == individualCategory.Id)
                    {
                        // Here we have only one speciesfact
                        speciesFacts = tempSpeciesFacts;
                        speciesFact = tempSpeciesFacts[0];
                        factorComments = GetFactorFieldValues(speciesFact, out fieldName, out fieldValue, out fieldName2, out fieldValue2);
                    }

                    existingEvaluations = GetExistingEvaluationValue(speciesFact, existingEvaluations, individualCategory);
                }
                else if (tempSpeciesFacts.Count > 1)
                {
                    // If we got several speciescfact hat belongs to a factor we have to check hostId to find the right one
                    foreach (ISpeciesFact tempSpeciesFact in tempSpeciesFacts)
                    {
                        // Get correct host id...
                        if (tempSpeciesFact.Host.IsNotNull() && tempSpeciesFact.Host.Id == Convert.ToInt32(hostId))
                        {
                            if (selectedCat.Id == individualCategory.Id)
                            {
                                // Here we have only one speciesfact
                                speciesFacts = tempSpeciesFacts;
                                speciesFact = tempSpeciesFact;
                                factorComments = GetFactorFieldValues(speciesFact, out fieldName, out fieldValue, out fieldName2, out fieldValue2);
                            }

                            existingEvaluations = GetExistingEvaluationValue(tempSpeciesFact, existingEvaluations, individualCategory);
                            break;
                        }
                    }
                }
            }

            return speciesFact;
        }

        /// <summary>
        /// Gets all dyntaxa factors, speciesfacts, hostes etc for a specific taxon and parent factor from artdatabanken service.
        /// </summary>
        /// <param name="useDataType">
        /// The use Data Type.
        /// </param>
        /// <returns>
        /// The <see cref="DyntaxaAllFactorData"/>.
        /// </returns>
        public DyntaxaAllFactorData GetFactorsFromTaxonAndParentFactor(bool useDataType = false)
        {
            DyntaxaAllFactorData dyntaxaAllFactorData = null;
            ITaxon speciesFactsTaxon;
            IFactor afFactor;
            speciesFacts = new SpeciesFactList();
            SpeciesFactList tempSpeciesFacts = new SpeciesFactList();
            speciesFactsTaxon = taxon;
            speciesFacts = GetSpeciesFacts(speciesFactsTaxon);

            // Get species fact for selected taxon.
            if (useDataType)
            {
                foreach (ISpeciesFact speciesFact in speciesFacts)
                {
                    if (speciesFact.Factor.DataType.Id == (int)DyntaxaFactorDataType.AF_SUBSTRATE &&
                        speciesFact.Factor.IsTaxonomic)
                    {
                        tempSpeciesFacts.Add(speciesFact);
                    }
                }

                speciesFacts = tempSpeciesFacts;
            }

            // Must sort the list
            speciesFacts.Sort();
            afFactor = GetFactor(userContext, (int)factorEnumId);

            FactorSearchCriteria criteria = new FactorSearchCriteria();
            criteria.RestrictReturnToScope = FactorSearchScope.AllChildFactors;
            criteria.RestrictSearchToScope = FactorSearchScope.NoScope;
            criteria.RestrictSearchToFactorIds = new List<int> { (int)factorEnumId };

            FactorList allChildFactorsToSelectedFactor = CoreData.FactorManager.GetFactors(userContext, criteria);
            FactorList allChildFactorsToSelectedFactorWithParents = new FactorList();
            allChildFactorsToSelectedFactorWithParents.Add(afFactor);

            IList<DyntaxaSpeciesFact> dyntaxaSpeciesFactForATaxon = new List<DyntaxaSpeciesFact>();
            string swedishOccuranceInfo = string.Empty;
            string swedishHistoryInfo = string.Empty;
            FactorList factorlist = new FactorList();
            factorlist.Add(afFactor);
            IFactor parentFactor = afFactor;
           
            // First we check if factor belongs to any of our factors/childfactors
            foreach (IFactor factor in allChildFactorsToSelectedFactor)
            {
                // Convert species fact list to a DyntaxaSpeciesFact list.
                foreach (ISpeciesFact spFactor in speciesFacts)
                {
                    // Check factors to be used, they muat match dataType and DataTypeId
                    ISpeciesFactField mainField = spFactor.MainField;
                   
                    // Special condition for life form...Improve error handling here...
                    if ((mainField.Type.Id == (int)FactorFieldDataTypeId.Enum &&
                        ((int)factorDataType == mainField.FactorField.FactorDataType.Id) &&
                        dataType == DyntaxaDataType.ENUM) ||
                        (factorEnumId == DyntaxaFactorId.LIFEFORM &&
                        mainField.Type.Id == (int)FactorFieldDataTypeId.Boolean &&
                        ((int)factorDataType == mainField.FactorField.FactorDataType.Id) &&
                        dataType == DyntaxaDataType.BOOLEAN))
                    {
                        if (factor.Id == spFactor.Factor.Id)
                        {
                            if (spFactor.Factor.Id == (int)FactorId.SwedishOccurrence)
                            {
                                if (spFactor.MainField.IsNotNull() && spFactor.MainField.HasValue)
                                {
                                    swedishOccuranceInfo = spFactor.MainField.EnumValue.OriginalLabel;
                                }
                            }

                            if (spFactor.Factor.Id == (int)FactorId.SwedishHistory)
                            {
                                if (spFactor.MainField.IsNotNull() && spFactor.MainField.HasValue)
                                {
                                    swedishHistoryInfo = spFactor.MainField.EnumValue.OriginalLabel;
                                }
                            }

                            bool okToUpdate = spFactor.Factor.IsNotNull() && spFactor.Factor.DataType.IsNotNull() && spFactor.Factor.UpdateMode.AllowUpdate;
                            DyntaxaFactorUpdateMode dyntaxaFactorUpdateMode = new DyntaxaFactorUpdateMode(spFactor.Factor.UpdateMode.IsHeader, okToUpdate);

                            string factorOriginName = string.Empty;
                            int factorOriginId = -1;
                            DyntaxaFactorOrigin factorOrigin = new DyntaxaFactorOrigin(factorOriginId, factorOriginName);
                            if (spFactor.Factor.Origin.IsNotNull() && (spFactor.Factor.Origin.Id > -1))
                            {
                                factorOrigin.OriginName = spFactor.Factor.Origin.Name;
                                factorOrigin.OriginId = spFactor.Factor.Origin.Id;
                            }

                            // Convert SpeciesFactField to DyntaxaFactorField
                            IList<DyntaxaFactorField> fields = new List<DyntaxaFactorField>();
                            foreach (ISpeciesFactField field in spFactor.Fields)
                            {
                                DyntaxaFactorFieldValues fieldValues = null;
                                DyntaxaFactorFieldDataTypeId dyntaxaFactorFieldDataTypeId = DyntaxaFactorFieldDataTypeId.String;
                                
                                if (field.Type.DataType == FactorFieldDataTypeId.Boolean)
                                {
                                    dyntaxaFactorFieldDataTypeId = DyntaxaFactorFieldDataTypeId.Boolean;

                                    fieldValues = new DyntaxaFactorFieldValues();
                                    fieldValues.FieldName = field.Label;
                                    fieldValues.FieldValue = Convert.ToInt32(field.BooleanValue);
                                    fieldValues.FactorFields.Add(new KeyValuePair<string, int>("sant", 1));
                                    fieldValues.FactorFields.Add(new KeyValuePair<string, int>("falskt", 0));
                                }

                                if (field.Type.DataType == FactorFieldDataTypeId.Double)
                                {
                                    dyntaxaFactorFieldDataTypeId = DyntaxaFactorFieldDataTypeId.Double;
                                }

                                if (field.Type.DataType == FactorFieldDataTypeId.Enum)
                                {
                                    fieldValues = new DyntaxaFactorFieldValues();
                                    fieldValues.FieldName = field.Label;
                                    if (field.EnumValue.IsNotNull())
                                    {
                                        if (field.EnumValue.KeyInt.HasValue)
                                        {
                                            fieldValues.FieldValue = field.EnumValue.KeyInt.Value;
                                        }
                                    }

                                    if (field.FactorFieldEnum.IsNotNull() && field.FactorFieldEnum.Values.IsNotNull())
                                    {
                                        foreach (FactorFieldEnumValue factorFieldEnum in field.FactorFieldEnum.Values)
                                        {
                                            if (factorFieldEnum.KeyInt.HasValue)
                                            {
                                                fieldValues.FactorFields.Add(new KeyValuePair<string, int>(factorFieldEnum.OriginalLabel, factorFieldEnum.KeyInt.Value));
                                            }
                                        }
                                    }

                                    dyntaxaFactorFieldDataTypeId = DyntaxaFactorFieldDataTypeId.Enum;
                                }

                                if (field.Type.DataType == FactorFieldDataTypeId.Int32)
                                {
                                    dyntaxaFactorFieldDataTypeId = DyntaxaFactorFieldDataTypeId.Int32;
                                }

                                DyntaxaFactorField item = new DyntaxaFactorField(
                                    field.Id.ToString(), 
                                    field.Label,
                                    field.Unit, 
                                    dyntaxaFactorFieldDataTypeId, 
                                    field.Value, 
                                    field.HasValue, 
                                    fieldValues,
                                    field.IsMain);
                                fields.Add(item);
                            }

                            // Get all avaiable qualities.
                            SpeciesFactQualityList qualityList = CoreData.SpeciesFactManager.GetSpeciesFactQualities(userContext);
                            IList<KeyValuePair<int, string>> qualities = new List<KeyValuePair<int, string>>();

                            foreach (SpeciesFactQuality spQuality in qualityList)
                            {
                                qualities.Add(new KeyValuePair<int, string>(spQuality.Id, spQuality.Name));
                            }

                            DyntaxaFactorQuality quality = new DyntaxaFactorQuality(spFactor.Quality.Id, spFactor.Quality.Name, qualities);
                            DyntaxaIndividualCategory individualCategory = null;
                            if (spFactor.IndividualCategory.IsNotNull())
                            {
                                individualCategory = new DyntaxaIndividualCategory(spFactor.IndividualCategory.Id, spFactor.IndividualCategory.Name, string.Empty);
                            }
                            else
                            {
                                IIndividualCategory dafultCat = CoreData.FactorManager.GetDefaultIndividualCategory(userContext);
                                individualCategory = new DyntaxaIndividualCategory(dafultCat.Id, dafultCat.Name, string.Empty);
                            }

                            int hostId = -1;
                            if (spFactor.HasHost)
                            {
                                hostId = spFactor.Host.Id;
                            }

                            DyntaxaSpeciesFact dyntaxaSpeciesFact = new DyntaxaSpeciesFact(
                                spFactor.Id.ToString(),
                                spFactor.Factor.Information, 
                                spFactor.Factor.Label, 
                                spFactor.Factor.IsLeaf,
                                spFactor.Factor.IsPeriodic, 
                                spFactor.Factor.SortOrder, 
                                spFactor.Factor.IsPublic,
                                spFactor.Factor.IsTaxonomic,
                                spFactor.Factor.HostLabel, 
                                hostId, 
                                quality, 
                                spFactor.Reference.Id, 
                                fields, 
                                factorOrigin,
                                dyntaxaFactorUpdateMode, 
                                spFactor.Identifier,
                                spFactor.ModifiedDate, 
                                individualCategory, 
                                spFactor.Factor.Id, 
                                spFactor.Reference.Name);

                            dyntaxaSpeciesFactForATaxon.Add(dyntaxaSpeciesFact);

                            // Check if parent header is set, what to do if a factor has several parents throw exception?
                            // We have to loop back to orginal factor....and add upp all parents....
                            bool validTree = false;
                            int level = 0;
                            FactorTreeNodeList nodeList = spFactor.Factor.GetFactorTree(userContext).Parents;
                            if (nodeList.IsNotEmpty())
                            {
                                // Get the factor node
                                IFactorTreeNode node = CoreData.FactorManager.GetFactorTree(userContext, spFactor.Factor.Id);
                                if (node.IsNotNull())
                                {
                                    // Get parents for factor, recursive.
                                    this.GetParentFactor(node, afFactor, factorlist, validTree, level);
                                }
                            } 
                            AddFactorToList(factorlist, spFactor.Factor);
                        }     
                    }
                }
            }

            ISpeciesFactSearchCriteria resultParameters = GetResultParameters(speciesFacts);
            IList<DyntaxaFactor> dyntaxaAllFactors = new List<DyntaxaFactor>();

            // Create all dyntaxa data for presenting SpeciesFact för a taxon and faktor. 
            if (factorlist.IsNotEmpty())
            {
                IList<DyntaxaPeriod> periodList;
                IList<DyntaxaIndividualCategory> individualCategoriesList;
                IList<DyntaxaHost> hostList = GetAdditionalFactorData(userContext, factorlist, dyntaxaAllFactors, resultParameters, out periodList, out individualCategoriesList);
                IList<DyntaxaHost> completeHostList = new List<DyntaxaHost>();

                // Missing out on some host factors; get them and add them to list sort list on factorNames
                if (useDataType)
                {
                    if (hostList.IsNotNull() && hostList.Count > 0)
                    {
                        IndividualCategoryList catList = new IndividualCategoryList();
                        foreach (DyntaxaIndividualCategory dyntaxaIndividualCategory in individualCategoriesList)
                        {
                            IIndividualCategory cat = CoreData.FactorManager.GetIndividualCategory(userContext, dyntaxaIndividualCategory.Id);
                            catList.Add(cat);
                        }
                        SpeciesFactList hostSpeciesFacts = new SpeciesFactList();
                        
                        foreach (DyntaxaHost dyntaxaHost in hostList)
                        {
                            ITaxon hostTaxon = CoreData.TaxonManager.GetTaxon(userContext, Convert.ToInt32(dyntaxaHost.Id));
                            TaxonList taxonList = new TaxonList();
                            taxonList.Add(hostTaxon);
                            SpeciesFactList hostSpeciesFactsTemp = GetSpeciesFacts(speciesFactsTaxon, null, catList, taxonList, true);
                            if (hostSpeciesFactsTemp.IsNotEmpty())
                            {
                                foreach (ISpeciesFact speciesFactTemp in hostSpeciesFactsTemp)
                                {
                                    bool containsItem = hostSpeciesFacts.Any(item => item.Identifier == speciesFactTemp.Identifier);
                                    if (!containsItem)
                                    {
                                        hostSpeciesFacts.Add(speciesFactTemp);
                                    }
                                }
                            }
                        }

                        foreach (ISpeciesFact hostSpeciesFact in hostSpeciesFacts)
                        {
                            if (hostSpeciesFact.Factor.IsTaxonomic)
                            {
                                ITaxon artTaxon = taxon;
                                Int32 hostId = 0, periodId = 0;
                                bool hasHostId = false;
                                bool hasPeriodId = false;

                                if (hostSpeciesFact.Host.IsNotNull() && hostSpeciesFact.Factor.IsTaxonomic)
                                {
                                    hostId = Convert.ToInt32(hostSpeciesFact.Host.Id);
                                    hasHostId = true;
                                }

                                if (hostSpeciesFact.Period.IsNotNull() && hostSpeciesFact.Factor.IsPeriodic)
                                {
                                    periodId = Convert.ToInt32(hostSpeciesFact.Period.Id);
                                    hasPeriodId = true;
                                }

                                string identifier = CoreData.SpeciesFactManager.GetSpeciesFactIdentifier(
                                    artTaxon.Id, 
                                    Convert.ToInt32(hostSpeciesFact.IndividualCategory.Id), 
                                    Convert.ToInt32(hostSpeciesFact.Factor.Id), 
                                    hasHostId, 
                                    hostId, 
                                    hasPeriodId, 
                                    periodId);

                                foreach (DyntaxaSpeciesFact speciesFact in dyntaxaSpeciesFactForATaxon)
                                {
                                    if (speciesFact.Identifier == identifier)
                                    {
                                        // Data found. Return it.
                                        ITaxon hostTaxon = CoreData.TaxonManager.GetTaxon(userContext, hostSpeciesFact.Host.Id);
                                        completeHostList.Add(new DyntaxaHost(
                                            hostTaxon.Id.ToString(), 
                                            hostTaxon.GetLabel(), 
                                            hostTaxon.ScientificName, 
                                            hostTaxon.CommonName, 
                                            Convert.ToInt32(speciesFact.FactorIdForHosts)));
                                    }
                                }
                            }
                        }
                    }
                }

           dyntaxaAllFactorData = new DyntaxaAllFactorData(
                dyntaxaAllFactors, 
                hostList, 
                individualCategoriesList, 
                periodList, 
                dyntaxaSpeciesFactForATaxon, 
                speciesFactsTaxon.Id, 
                swedishOccuranceInfo, 
                swedishHistoryInfo, 
                completeHostList);
            }

            return dyntaxaAllFactorData;
        }

        /// <summary>
        /// The get parent factor. using recusive call.
        /// </summary>
        /// <param name="parentFactorNode">
        /// The parent factor node.
        /// </param>
        /// <param name="afFactor">
        /// The af factor.
        /// </param>
        /// <param name="factorlist">
        /// The factorlist.
        /// </param>
        /// <param name="validTree">
        /// The valid tree.
        /// </param>
        /// <param name="level">
        /// The level.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// <exception cref="ApplicationException">
        /// </exception>
        private bool GetParentFactor(IFactorTreeNode parentFactorNode, IFactor afFactor, FactorList factorlist, bool validTree, int level)
        {
            // Keep checking parents up to factor (Påverkan, Substrate etc) or tree is valid, then set add all factors to list. Break if tree node is deeper than 10 levels.
            if (parentFactorNode.Factor.Id != afFactor.Id)
            {
                FactorTreeNodeList nodeList = parentFactorNode.Factor.GetFactorTree(this.userContext).Parents;
                if (!nodeList.IsNull())
                {
                    foreach (IFactorTreeNode grandParentFactorNode in nodeList)
                    {
                        level++;

                        // AddFactorToList(factorlist, grandParentFactorNode6.Factor)
                        validTree = GetParentFactor(grandParentFactorNode, afFactor, factorlist, validTree, level);
                        if (validTree)
                        {
                            AddFactorToList(factorlist, parentFactorNode.Factor);
                            break;
                        }
                    }
                }
            }
            else if (level == 10)
            {
                throw new ApplicationException("Incorrupt factor tree for factor:" + parentFactorNode.Factor.Label + " (" + parentFactorNode.Factor.Id + ").");
            }
            else
            {
                validTree = true;
            }

            return validTree;
        }

        /// <summary>
        /// Get identifier for a SpeciesFact.
        /// </summary>
        /// <param name="taxon">
        /// </param>
        /// <param name="individualCategory">
        /// </param>
        /// <param name="factor">
        /// </param>
        /// <param name="host">
        /// </param>
        /// <param name="period">
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetSpeciesFactIdentifier(ITaxon taxon, DyntaxaIndividualCategory individualCategory, DyntaxaFactor factor, DyntaxaHost host, DyntaxaPeriod period)
        {
            ITaxon artTaxon = taxon;
            Int32 hostId = 0, periodId = 0;
            bool hasHostId = false;
            bool hasPeriodId = false;

            if (host.IsNotNull() && factor.IsTaxonomic)
            {
                hostId = Convert.ToInt32(host.Id);
                hasHostId = true;
            }

            if (period.IsNotNull() && factor.IsPeriodic)
            {
                periodId = Convert.ToInt32(period.Id);
                hasPeriodId = true;
            }

            return CoreData.SpeciesFactManager.GetSpeciesFactIdentifier(
                artTaxon.Id, 
                Convert.ToInt32(individualCategory.Id), 
                Convert.ToInt32(factor.Id), 
                hasHostId, 
                hostId, 
                hasPeriodId, 
                periodId);     
        }

        /// <summary>
        /// Update species fact ie assign updated values. To implement them all the way to db 
        /// UpdateDyntaxaSpeciesFacts() must be called. 
        /// </summary>
        /// <param name="factorId">
        /// </param>
        /// <param name="index">
        /// </param>
        /// <param name="newValue">
        /// </param>
        public void SetNewValueForSpeciesFactForTaxon(FactorId factorId, int index, object newValue)
        {
            // UpdateSpeciesFacts.                        
            ISpeciesFact speciesObject = speciesFacts.GetSpeciesFacts(CoreData.FactorManager.GetFactor(userContext, factorId))[index];
            if (speciesObject.IsNotNull())
            {
                Int32 value = 0;
                bool booleanValue = false;
                IFactorFieldEnumValue enumValue = null;
                string stringValue = null;

                if (((int)factorId == (int)FactorId.QualityInDyntaxa) || ((int)factorId == (int)FactorId.SwedishOccurrence) || ((int)factorId == (int)FactorId.SwedishHistory))
                {
                    if (speciesObject.MainField != null)
                    {
                        if (newValue is FactorFieldEnumValue)
                        {                            
                            enumValue = speciesObject.MainField.EnumValue;
                            speciesObject.MainField.Value = newValue as FactorFieldEnumValue;
                        }
                        else if (newValue is string)
                        {
                            stringValue = speciesObject.Field5.StringValue;
                            speciesObject.Field5.Value = (string)newValue;
                        }
                        else if (newValue is SpeciesFactQuality)
                        {
                            speciesObject.Quality = (SpeciesFactQuality)newValue;
                        }
                        else if (newValue is ArtDatabanken.Data.Reference)
                        {
                            speciesObject.Reference = (ArtDatabanken.Data.Reference)newValue;
                        }
                    }
                }
                else if ((int)factorId == (int)FactorId.NumberOfSwedishSpecies)
                {
                    if (speciesObject.MainField != null)
                    {
                        value = speciesObject.MainField.Int32Value;
                        speciesObject.MainField.Int32Value = (int)newValue;                       
                    }
                }
                else if (((int)factorId == (int)FactorId.BanndedForReporting) || ((int)factorId == (int)FactorId.ExcludeFromReportingSystem))
                {
                    if (speciesObject.MainField != null)
                    {
                        if (newValue is bool)
                        {
                            booleanValue = speciesObject.MainField.BooleanValue;
                            speciesObject.MainField.BooleanValue = (bool)newValue;
                        }
                        else
                        {
                            speciesObject.Reference = (ArtDatabanken.Data.Reference)newValue;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Update species fact ie assign null values. To implemet them all the way to db 
        /// UpdateDyntaxaSpeciesFacts() must be called after words. 
        /// </summary>
        /// <param name="factorId">
        /// </param>
        /// <param name="index">
        /// </param>
        /// <param name="newValue">
        /// </param>
        public void SetNewNullValueForSpeciesFactForTaxon(FactorId factorId, int index, string newValue)
        {
            // UpdateSpeciesFacts.
            ISpeciesFact speciesObject = speciesFacts.GetSpeciesFacts(CoreData.FactorManager.GetFactor(userContext, factorId))[index];            
            if (speciesObject.IsNotNull())
            {
                if ((int)factorId == (int)FactorId.SwedishHistory)
                {
                    if (speciesObject.MainField != null)
                    {
                        if (newValue.Equals("FactorFieldEnumValue"))
                        {
                           speciesObject.MainField.Value = null;
                        }
                        else if (newValue.Equals("string"))
                        {
                            speciesObject.Field5.Value = null;
                        }                        
                        else if (newValue.Equals("Reference"))
                        {
                            speciesObject.Reference = null;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Commit all changes in the DB.
        /// </summary>
        public void UpdateDyntaxaSpeciesFacts()
        {
            UpdateDyntaxaSpeciesFacts(speciesFacts);
        }

        /// <summary>
        /// Commit changes for specified SpeciesFacts to the DB.
        /// </summary>
        public void UpdateDyntaxaSpeciesFacts(SpeciesFactList speciesFactList)
        {
            IReference referenceValue = CoreData.ReferenceManager.GetReference(userContext, dyntaxaReference);
            using (ITransaction transaction = userContext.StartTransaction(30))
            {
                CoreData.SpeciesFactManager.UpdateSpeciesFacts(userContext, speciesFactList, referenceValue);

                transaction.Commit();
            }
        }

        /// <summary>
        /// Updates all changes in the DB, without a transaction.
        /// The caller is responsible for using a transaction! 
        /// </summary>
        public void UpdateDyntaxaSpeciesFactsWithoutTransaction()
        {
            UpdateDyntaxaSpeciesFactsWithoutTransaction(speciesFacts);
        }

        /// <summary>
        /// Updates changes for specified SpeciesFacts in the DB, without a transaction.
        /// Instead the caller is responsible for using a transaction! 
        /// </summary>
        public void UpdateDyntaxaSpeciesFactsWithoutTransaction(SpeciesFactList speciesFactList)
        {
            var referenceValue = CoreData.ReferenceManager.GetReference(userContext, dyntaxaReference);
            CoreData.SpeciesFactManager.UpdateSpeciesFacts(userContext, speciesFactList, referenceValue);
        }

        /// <summary>
        /// </summary>
        /// <param name="factorDataTypeId">
        /// The factor Data Type Id.
        /// </param>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        public IList<DyntaxaFactor> GetFactorsFromFactorIdAndFactorDataType(string factorDataTypeId)
        {
            FactorList factors = new FactorList();

            // Updated with oskarna new lists:  FactorManager.GetFactors()
            if (Convert.ToInt32(factorDataTypeId) == (int)DyntaxaFactorDataType.AF_SUBSTRATE)
            {
                factors = CoreData.FactorManager.GetFactors(userContext, FactorShortListManager.GetSubstrateFactorShortlist());
            }
            else if (Convert.ToInt32(factorDataTypeId) == (int)DyntaxaFactorDataType.AF_BIOTOPE)
            {
                factors = CoreData.FactorManager.GetFactors(userContext, FactorShortListManager.GetHabitatFactorShortlist());
            }
            else if (Convert.ToInt32(factorDataTypeId) == (int)DyntaxaFactorDataType.AF_INFLUENCE)
            {
                factors = CoreData.FactorManager.GetFactors(userContext, FactorShortListManager.GetInfluenceFactorShortlist());
            }

            IList<DyntaxaFactor> dyntaxaFactorList = new List<DyntaxaFactor>();
            foreach (IFactor factor in factors)
            {
                if ((factor.DataType.IsNotNull() && (factor.DataType.Id == Convert.ToInt32(factorDataTypeId))) || factor.UpdateMode.IsHeader)
                {
                    bool okToUpdate = !factor.UpdateMode.IsHeader && factor.DataType.IsNotNull();
                    DyntaxaFactor dyntaxaFactor = new DyntaxaFactor(
                        Convert.ToString(factor.Id), 
                        factor.Label, 
                        factor.IsLeaf, 
                        factor.IsPeriodic, 
                        factor.SortOrder, 
                        factor.IsPublic, 
                        factor.IsTaxonomic, 
                        new DyntaxaFactorOrigin(factor.Origin.Id, factor.Origin.Name), 
                        new DyntaxaFactorUpdateMode(factor.UpdateMode.IsHeader, okToUpdate), 
                        factor.Id);
                    dyntaxaFactorList.Add(dyntaxaFactor);
                }
            }

            return dyntaxaFactorList;
        }

        /// <summary>
        /// </summary>
        /// <param name="factorId">
        /// The factor Id.
        /// </param>
        /// <param name="factorDataTypeId">
        /// The factor Data Type Id.
        /// </param>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        public IList<DyntaxaFactor> GetFactorsFromFactorId(int factorId, int factorDataTypeId)
        {
            FactorSearchCriteria searchCriteria = new FactorSearchCriteria();
            searchCriteria.RestrictSearchToFactorIds = new List<int>();
            searchCriteria.RestrictSearchToFactorIds.Add(Convert.ToInt32(factorId));
            searchCriteria.RestrictSearchToScope = FactorSearchScope.NoScope; // set to parent.
            searchCriteria.RestrictReturnToScope = FactorSearchScope.AllChildFactors;

            FactorList factors = CoreData.FactorManager.GetFactors(userContext, searchCriteria);
            IList<DyntaxaFactor> dyntaxaFactorList = new List<DyntaxaFactor>();
            foreach (IFactor factor in factors)
            {
                if ((factor.DataType.IsNotNull() && (factor.DataType.Id == Convert.ToInt32(factorDataTypeId))) || factor.UpdateMode.IsHeader)
                {
                    bool okToUpdate = !factor.UpdateMode.IsHeader && factor.DataType.IsNotNull();

                    DyntaxaFactor dyntaxaFactor = new DyntaxaFactor(
                        Convert.ToString(factor.Id), 
                        factor.Label, 
                        factor.IsLeaf, 
                        factor.IsPeriodic, 
                        factor.SortOrder, 
                        factor.IsPublic, 
                        factor.IsTaxonomic, 
                        new DyntaxaFactorOrigin(factor.Origin.Id, factor.Origin.Name), 
                        new DyntaxaFactorUpdateMode(factor.UpdateMode.IsHeader, okToUpdate), 
                        factor.Id);
                    dyntaxaFactorList.Add(dyntaxaFactor);
                }
            }

            return dyntaxaFactorList;
        }

        /// <summary>
        /// </summary>
        /// <param name="factorDataTypeId">
        /// The factor Data Type Id.
        /// </param>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        public IList<DyntaxaFactor> GetFactorsFromFactorIdAndFactorDataTypeSubstrate(string factorDataTypeId)
        {
            FactorList factors = new FactorList();            
            if (Convert.ToInt32(factorDataTypeId) == (int)DyntaxaFactorDataType.AF_SUBSTRATE)
            {
                factors = CoreData.FactorManager.GetFactors(userContext, FactorShortListManager.GetHostSubstrateFactorShortlist());
            }

            IList<DyntaxaFactor> dyntaxaFactorList = new List<DyntaxaFactor>();
            foreach (IFactor factor in factors)
            {
                if ((factor.DataType.IsNotNull() && factor.DataType.Id == Convert.ToInt32(factorDataTypeId)) || factor.UpdateMode.IsHeader)
                {
                    bool okToUpdate = !factor.UpdateMode.IsHeader && factor.DataType.IsNotNull();

                    DyntaxaFactor dyntaxaFactor = new DyntaxaFactor(
                        Convert.ToString(factor.Id), 
                        factor.Label, 
                        factor.IsLeaf, 
                        factor.IsPeriodic, 
                        factor.SortOrder, 
                        factor.IsPublic, 
                        factor.IsTaxonomic, 
                        new DyntaxaFactorOrigin(factor.Origin.Id, factor.Origin.Name), 
                        new DyntaxaFactorUpdateMode(factor.UpdateMode.IsHeader, okToUpdate), 
                        factor.Id);
                    dyntaxaFactorList.Add(dyntaxaFactor);
                }
            }

            return dyntaxaFactorList;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        public IList<DyntaxaIndividualCategory> GetAllIndividualCategories()
        {
            IndividualCategoryList list = CoreData.FactorManager.GetIndividualCategories(userContext);
            IList<DyntaxaIndividualCategory> individualCategoriesList = new List<DyntaxaIndividualCategory>();
            foreach (IIndividualCategory category in list)
            {
                individualCategoriesList.Add(new DyntaxaIndividualCategory(category.Id, category.Name, string.Empty));
            }

            return individualCategoriesList;
        }

        /// <summary>
        /// </summary>
        /// <param name="individualCategory">
        /// The individual Category.
        /// </param>
        /// <returns>
        /// The <see cref="DyntaxaIndividualCategory"/>.
        /// </returns>
        public DyntaxaIndividualCategory GetIndividualCategory(int individualCategory)
        {
            IIndividualCategory cat = CoreData.FactorManager.GetIndividualCategory(userContext, individualCategory);
            DyntaxaIndividualCategory dyntaxaIndividualCategory = new DyntaxaIndividualCategory(cat.Id, cat.Name, string.Empty);
            return dyntaxaIndividualCategory;
        }

        /// <summary>
        /// Updated the listed speciesFacts. If speciecfact is null ie nothing to update then it is removed from list.
        /// </summary>
        /// <param name="newValuesInList">
        /// </param>
        /// <param name="newCategory">
        /// </param>
        /// <param name="updateFieldValue2">
        /// </param>
        public void UpdatedSpeciecFacts(IList<SpeciesFactFieldValueModelHelper> newValuesInList, bool newCategory, bool updateFieldValue2)
        {
            SpeciesFactList updateList = new SpeciesFactList();
            int i = 0;
            foreach (var newValues in newValuesInList)
            {
                i++;
                ISpeciesFact speciesFact = SetNewValueForFactor(newValues, newCategory, updateFieldValue2);
               
                if (speciesFact.IsNotNull())
                {
                    updateList.Add(speciesFact);
                }
            }

            try
            {
                UpdateSpeciesFacts(updateList);
            }
            catch (Exception ex)
            {
                DyntaxaLogger.WriteMessage("UpdatedSpeciecFacts: " + ex.Message);
                // If something gone wrong we have to reset values in the onoin...
                if (updateList.IsNotNull())
                {
                    updateList = null;
                }

                throw;
            }
        }

        /// <summary>
        /// Updated the listed speciesFacts. If speciecfact is null ie nothing to update then it is removed from list.
        /// </summary>
        /// <param name="newCommonValues">
        /// The new Common Values.
        /// </param>
        /// <param name="dyntaxaSpeciesFacts">
        /// The dyntaxa Species Facts.
        /// </param>
        public void UpdatedSpeciecFacts(SpeciesFactFieldValueModelHelper newCommonValues, IList<DyntaxaSpeciesFact> dyntaxaSpeciesFacts)
        {
            SpeciesFactList updateList = new SpeciesFactList();
            List<int> speciesFactsIds = new List<int>();
            foreach (DyntaxaSpeciesFact dyntaxaSpeciesFact in dyntaxaSpeciesFacts)
            {
                speciesFactsIds.Add(Convert.ToInt32(dyntaxaSpeciesFact.Id));
            }

            SpeciesFactList speciesFactsList = CoreData.SpeciesFactManager.GetSpeciesFacts(userContext, speciesFactsIds);

            IList<SpeciesFactFieldValueModelHelper> newValuesInList = new List<SpeciesFactFieldValueModelHelper>();
            foreach (SpeciesFact speciesFact in speciesFactsList)
            {
                SpeciesFactFieldValueModelHelper newValue = new SpeciesFactFieldValueModelHelper();
                newValue.FactorId = speciesFact.Factor.Id;
                newValue.FactorField1Value = newCommonValues.FactorField1Value;
                newValue.HostId = speciesFact.Host.Id; 
                newValue.IndividualCategoryId = newCommonValues.IndividualCategoryId;
                newValue.StringValue5 = newCommonValues.StringValue5;
                newValue.QualityId = newCommonValues.QualityId;
                newValue.ReferenceId = newCommonValues.ReferenceId;
                newValue.FactorField2Value = newCommonValues.FactorField2Value;
                newValue.MainParentFactorId = newCommonValues.MainParentFactorId;
                newValuesInList.Add(newValue);
            }

            // Test that correct values is set...
            UpdatedSpeciecFacts(newValuesInList, false, true);           
        }

        #endregion

        #region PRIVATE METHODS

        /// <summary>
        /// Gets species fact for a specific taxon from artdatabanken service.
        /// </summary>
        /// <returns>
        /// The <see cref="SpeciesFactList"/>.
        /// </returns>
        private SpeciesFactList GetSpeciesFactList()
        {
            ISpeciesFactSearchCriteria speciesFactSearchCriteria;
            
            speciesFacts = new SpeciesFactList();
            try
            {
                ITaxon speciesFactsTaxon = taxon;

                speciesFactSearchCriteria = new SpeciesFactSearchCriteria();
                speciesFactSearchCriteria.EnsureNoListsAreNull();
                speciesFactSearchCriteria.IncludeNotValidHosts = true;
                speciesFactSearchCriteria.IncludeNotValidTaxa = true;
                
                speciesFactSearchCriteria.Factors = new FactorList();
                speciesFactSearchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.SwedishOccurrence));
                speciesFactSearchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.SwedishHistory));
                speciesFactSearchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.QualityInDyntaxa));
                
                //speciesFactSearchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.NumberOfSwedishSpecies));
                speciesFactSearchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.BanndedForReporting));
                speciesFactSearchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(userContext, FactorId.ExcludeFromReportingSystem));

                speciesFactSearchCriteria.Taxa = new TaxonList();
                speciesFactSearchCriteria.Taxa.Add(speciesFactsTaxon);
                
                speciesFacts = CoreData.SpeciesFactManager.GetDyntaxaSpeciesFacts(userContext, speciesFactSearchCriteria);
                        
                // set up all data that are required
                foreach (SpeciesFact speciesFact in speciesFacts)
                {
                    if (speciesFact.Factor.Id == (int)FactorId.SwedishOccurrence)
                    {
                        swedishOccurenceFact = speciesFact;
                    }

                    if (speciesFact.Factor.Id == (int)FactorId.SwedishHistory)
                    {
                        swedishHistoryFact = speciesFact;
                    }

                    if (speciesFact.Factor.Id == (int)FactorId.QualityInDyntaxa)
                    {
                        qualityInDyntaxa = speciesFact;
                    }

                    if (speciesFact.Factor.Id
                        == (int)FactorId.NumberOfSwedishSpecies)
                    {
                        numberOfSwedishSpecies = speciesFact;
                    }

                    if (speciesFact.Factor.Id
                        == (int)FactorId.BanndedForReporting)
                    {
                        banndedForReporting = speciesFact;
                    }

                    if (speciesFact.Factor.Id
                        == (int)FactorId.ExcludeFromReportingSystem)
                    {
                        excludeFromReportingSystem = speciesFact;
                    }
                }
            }
            catch (Exception ex)
            {
                DyntaxaLogger.WriteMessage("GetSpeciesFactList: " + ex.Message);
                throw new ApplicationException(DyntaxaResource.SharedError);
            }

            return speciesFacts;
        }

        /// <summary>
        /// Gets species fact for the specified taxa and factors.
        /// </summary>
        /// <returns>
        /// The <see cref="SpeciesFactList"/>.
        /// </returns>
        public static SpeciesFactList GetSpeciesFactListByTaxaAndFactors(IUserContext userContext, IEnumerable<FactorId> factorIds, IEnumerable<int> taxonIds, bool includeMissingSpeciesFactsAsEmptySpeciesFacts)
        {
            ISpeciesFactSearchCriteria speciesFactSearchCriteria;

            SpeciesFactList speciesFacts = new SpeciesFactList();
            try
            {               
                speciesFactSearchCriteria = new SpeciesFactSearchCriteria();
                speciesFactSearchCriteria.EnsureNoListsAreNull();
                speciesFactSearchCriteria.IncludeNotValidHosts = true;
                speciesFactSearchCriteria.IncludeNotValidTaxa = true;                
                speciesFactSearchCriteria.Factors = CoreData.FactorManager.GetFactors(userContext, factorIds.Select(factorId => (int)factorId).ToList());                
                speciesFactSearchCriteria.Taxa = CoreData.TaxonManager.GetTaxa(userContext, taxonIds.ToList());
                if (includeMissingSpeciesFactsAsEmptySpeciesFacts)
                {
                    speciesFacts = CoreData.SpeciesFactManager.GetDyntaxaSpeciesFacts(userContext, speciesFactSearchCriteria);
                }
                else
                {
                    speciesFacts = CoreData.SpeciesFactManager.GetSpeciesFacts(userContext, speciesFactSearchCriteria);    
                }
            }
            catch (Exception ex)
            {
                DyntaxaLogger.WriteMessage("GetSpeciesFactList: " + ex.Message);
                throw new ApplicationException(DyntaxaResource.SharedError);
            }

            return speciesFacts;
        }

        // same as GetSpeciesFactList() but with use of SpeciesFactHelper

        /// <summary>
        /// The get species fact list with helper.
        /// </summary>
        /// <returns>
        /// The <see cref="SpeciesFactList"/>.
        /// </returns>
        private SpeciesFactList GetSpeciesFactListWithHelper()
        {
            SpeciesFactList speciesFactList;
            Dictionary<FactorId, SpeciesFact> dicSpeciesFacts = SpeciesFactHelper.GetSpeciesFacts(
                userContext, 
                taxon, 
                new[]
                    {
                        FactorId.SwedishOccurrence, FactorId.SwedishHistory, 
                        FactorId.QualityInDyntaxa, FactorId.NumberOfSwedishSpecies, 
                        FactorId.BanndedForReporting, FactorId.ExcludeFromReportingSystem
                    }, 
                out speciesFactList);
            if (speciesFactList.IsNull())
            {
                speciesFactList = new SpeciesFactList();
            }

            dicSpeciesFacts.TryGetValue(FactorId.SwedishOccurrence, out swedishOccurenceFact);
            dicSpeciesFacts.TryGetValue(FactorId.SwedishHistory, out swedishHistoryFact);
            dicSpeciesFacts.TryGetValue(FactorId.QualityInDyntaxa, out qualityInDyntaxa);
            dicSpeciesFacts.TryGetValue(FactorId.NumberOfSwedishSpecies, out numberOfSwedishSpecies);
            dicSpeciesFacts.TryGetValue(FactorId.BanndedForReporting, out banndedForReporting);
            dicSpeciesFacts.TryGetValue(FactorId.ExcludeFromReportingSystem, out excludeFromReportingSystem);

            return speciesFactList;
        }

        /// <summary>
        /// Get all SpeciesFacts for a taxon.
        /// </summary>
        /// <param name="taxon">
        /// </param>
        /// <param name="factorList">
        /// The factor List.
        /// </param>
        /// <param name="individualCategories">
        /// The individual Categories.
        /// </param>
        /// <param name="taxonList">
        /// The taxon List.
        /// </param>
        /// <param name="useHost">
        /// The use Host.
        /// </param>
        /// <returns>
        /// The <see cref="SpeciesFactList"/>.
        /// </returns>
        private SpeciesFactList GetSpeciesFacts(ITaxon taxon, FactorList factorList = null, IndividualCategoryList individualCategories = null, TaxonList taxonList = null, bool useHost = false)
        {
            ISpeciesFactSearchCriteria searchParameters = new SpeciesFactSearchCriteria();
            searchParameters.EnsureNoListsAreNull();
            searchParameters.IncludeNotValidHosts = true;
            searchParameters.IncludeNotValidTaxa = true;
            searchParameters.Taxa = new TaxonList();
            searchParameters.Factors = new FactorList();
            searchParameters.IndividualCategories = new IndividualCategoryList();
            searchParameters.Hosts = new TaxonList();

            if (taxonList.IsNotEmpty())
            {
                if (useHost)
                {
                    foreach (ITaxon tempTaxon in taxonList)
                    {                        
                        searchParameters.Hosts.Add(tempTaxon);
                    }

                    searchParameters.Taxa.Add(taxon);
                }
                else
                {
                    foreach (ITaxon tempTaxon in taxonList)
                    {
                        searchParameters.Taxa.Add(tempTaxon);
                    }
                }
            }
            else if (taxon.IsNotNull())
            {
                searchParameters.Taxa.Add(taxon);
            }
            
            if (factorList.IsNotNull())
            {
                foreach (IFactor factor in factorList)
                {
                    searchParameters.Factors.Add(factor);
                }
            }
            
            if (individualCategories.IsNotEmpty())
            {
                searchParameters.IndividualCategories = individualCategories;
            }

            SpeciesFactList facts = new SpeciesFactList();
            try
            {
                SpeciesFactList allFacts = CoreData.SpeciesFactManager.GetDyntaxaSpeciesFacts(userContext, searchParameters);                    
                Boolean deleteFact;
                if (allFacts.IsNotEmpty())
                {
                    foreach (ISpeciesFact fact in allFacts)
                    {
                        deleteFact = false;
                        if (!fact.MainField.HasValue)
                        {
                            deleteFact = true;
                        }
                        else
                        {
                            if (fact.MainField.FactorField.Type.DataType == FactorFieldDataTypeId.Boolean)
                            {
                                if (!fact.MainField.GetBoolean())
                                {
                                    deleteFact = true;
                                }
                            }
                            else
                            {
                                if (fact.MainField.FactorField.Type.DataType == FactorFieldDataTypeId.Enum)
                                {
                                    if (fact.MainField.EnumValue.KeyInt == 0)
                                    {                                                                 
                                        if (fact.MainField.EnumValue.Id != 5
                                            && fact.MainField.EnumValue.Id != 18
                                            && fact.MainField.EnumValue.Id != 19
                                            && fact.MainField.EnumValue.Id != 21
                                            && fact.MainField.EnumValue.Id != 29
                                            && fact.MainField.EnumValue.Id != 49)
                                        {
                                            deleteFact = true;
                                        }
                                    }
                                }
                            }
                        }

                        if (!deleteFact)
                        {
                            facts.Add(fact);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DyntaxaLogger.WriteException(ex);
                throw new ApplicationException(DyntaxaResource.SharedError);
            }

            return facts;
        }

        /// <summary>
        /// Get and sort SpeciesFact as factors, hosts, individual categories and periods.
        /// </summary>
        /// <param name="facts">
        /// </param>
        /// <returns>
        /// The <see cref="ISpeciesFactSearchCriteria"/>.
        /// </returns>
        private ISpeciesFactSearchCriteria GetResultParameters(SpeciesFactList facts)
        {
            ISpeciesFactSearchCriteria parameters = new SpeciesFactSearchCriteria();
            parameters.EnsureNoListsAreNull();
            parameters.IncludeNotValidHosts = true;
            parameters.IncludeNotValidTaxa = true;
            parameters.Taxa = new TaxonList();
            parameters.Factors = new FactorList();
            parameters.IndividualCategories = new IndividualCategoryList();
            parameters.Hosts = new TaxonList();
            parameters.Periods = new PeriodList();            

            foreach (ISpeciesFact fact in facts)
            {
                if (!parameters.Factors.Exists(fact.Factor.Id))
                {
                    parameters.Factors.Add(fact.Factor);
                }

                if (!parameters.IndividualCategories.Exists(fact.IndividualCategory.Id))
                {
                    parameters.IndividualCategories.Add(fact.IndividualCategory);
                }

                if (fact.HasPeriod)
                {
                    if (!parameters.Periods.Exists(fact.Period.Id))
                    {
                        parameters.Periods.Add(fact.Period);
                    }
                }

                if (fact.HasHost)
                {
                    if (!parameters.Hosts.Exists(fact.Host.Id))
                    {
                        parameters.Hosts.Add(fact.Host);
                    }
                }
            }

            if (parameters.Factors.IsNotEmpty())
            {
                parameters.Factors.Sort();
            }

            if (parameters.Hosts.IsNotEmpty())
            {
                parameters.Hosts.Sort();
            }

            if (parameters.IndividualCategories.IsNotEmpty())
            {
                parameters.IndividualCategories.Sort();
            }

            if (parameters.Periods.IsNotEmpty())
            {
                parameters.Periods.Sort();
            }

            return parameters;
        }

        /// <summary>
        /// Gets the parent factors for a list of factors.
        /// </summary>
        /// <param name="factors">
        /// </param>
        /// <returns>
        /// The <see cref="FactorList"/>.
        /// </returns>
        private FactorList GetAllParentFactors(FactorList factors)
        {
            FactorSearchCriteria criteria = new FactorSearchCriteria();
            criteria.RestrictSearchToFactorIds = factors.GetIds();
            criteria.RestrictReturnToScope = FactorSearchScope.AllParentFactors;
            FactorList parents = CoreData.FactorManager.GetFactors(userContext, criteria);
            if (parents.IsNull())
            {
                parents = new FactorList();    
            }

            return parents;
        }

        /// <summary>
        /// Gets the childs factors for a list of factors.
        /// </summary>
        /// <returns>
        /// The <see cref="IFactor"/>.
        /// </returns>
        /// <summary>
        /// Get factor from factorId.
        /// </summary>
        /// <param name="userContext">
        /// The user Context.
        /// </param>
        /// <param name="factorId">
        /// Id for the factor to be collected.
        /// </param>
        /// <returns>
        /// A factor instance.
        /// </returns>
        public static IFactor GetFactor(IUserContext userContext, int factorId)
        {
            IFactor afFactor = CoreData.FactorManager.GetFactor(userContext, factorId);
            return afFactor;
        }

        /// <summary>
        /// </summary>
        /// <param name="userContext">
        /// The user Context.
        /// </param>
        /// <param name="factorlist">
        /// </param>
        /// <param name="dyntaxaAllFactors">
        /// </param>
        /// <param name="resultParameters">
        /// </param>
        /// <param name="periodList">
        /// </param>
        /// <param name="individualCategoriesList">
        /// </param>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        private static IList<DyntaxaHost> GetAdditionalFactorData(
            IUserContext userContext, 
            FactorList factorlist, 
            IList<DyntaxaFactor> dyntaxaAllFactors, 
            ISpeciesFactSearchCriteria resultParameters, 
            out IList<DyntaxaPeriod> periodList, 
            out IList<DyntaxaIndividualCategory> individualCategoriesList)
        {
            factorlist.Sort();
            
            // Set up all data that are required.
            foreach (IFactor factor in factorlist)
            {
                string factorOriginName = string.Empty;
                int factorOriginId = -1;
                DyntaxaFactorOrigin factorOrigin = new DyntaxaFactorOrigin(factorOriginId, factorOriginName);
                if (factor.Origin.IsNotNull() && (factor.Origin.Id > -1))
                {
                    factorOrigin.OriginName = factor.Origin.Name;
                    factorOrigin.OriginId = factor.Origin.Id;
                }

                DyntaxaFactorUpdateMode dyntaxaFactorUpdateMode = new DyntaxaFactorUpdateMode(factor.UpdateMode.IsHeader, factor.UpdateMode.AllowUpdate);
                DyntaxaFactor dyntaxaFactor = new DyntaxaFactor(
                    factor.Id.ToString(), 
                    factor.Label, 
                    factor.IsLeaf, 
                    factor.IsPeriodic, 
                    factor.SortOrder, 
                    factor.IsPublic, 
                    factor.IsTaxonomic, 
                    factorOrigin, 
                    dyntaxaFactorUpdateMode, 
                    factor.Id);
                dyntaxaAllFactors.Add(dyntaxaFactor);
            }

            // Sort and convert Hosts,Periods and Individual categories for all factors.
            resultParameters.Hosts.Sort();
            resultParameters.Periods.Sort();
            resultParameters.IndividualCategories.Sort();
            
            IList<DyntaxaHost> hostList = new List<DyntaxaHost>();
            foreach (ITaxon host in resultParameters.Hosts)
            {
                hostList.Add(new DyntaxaHost(host.Id.ToString(), host.GetLabel(), host.ScientificName, host.CommonName));
            }

            periodList = new List<DyntaxaPeriod>();
            foreach (IPeriod period in resultParameters.Periods)
            {
                periodList.Add(new DyntaxaPeriod(period.Id.ToString(), period.Name));
            }

            individualCategoriesList = new List<DyntaxaIndividualCategory>();
            foreach (IIndividualCategory category in resultParameters.IndividualCategories)
            {
                individualCategoriesList.Add(new DyntaxaIndividualCategory(category.Id, category.Name, string.Empty));
            }
           
            return hostList;
        }

        /// <summary>
        /// </summary>
        /// <param name="factorlist">
        /// </param>
        /// <param name="factor">
        /// </param>
        private static void AddFactorToList(FactorList factorlist, IFactor factor)
        {
            bool exist = factorlist.Cast<Factor>().Any(tempFactor => tempFactor.Id == factor.Id);
            if (!exist)
            {
                factorlist.Add(factor);
            }
        }

        /// <summary>
        /// The get existing evaluation value.
        /// </summary>
        /// <param name="tempSpeciesFact">
        /// The temp species fact.
        /// </param>
        /// <param name="existingEvaluations">
        /// The existing evaluations.
        /// </param>
        /// <param name="individualCategory">
        /// The individual category.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetExistingEvaluationValue(
            ISpeciesFact tempSpeciesFact, 
            string existingEvaluations, 
            IIndividualCategory individualCategory)
        {
            // Must get the enum value here and extract only the  text .....
            if (tempSpeciesFact != null)
            {
                foreach (SpeciesFactField tempField in tempSpeciesFact.Fields)
                {
                    if (tempField.Type.DataType == FactorFieldDataTypeId.Enum && tempField.IsMain)
                    {
                        if (tempField.EnumValue.Label.IsNotNull())
                        {
                            string fieldValueText = tempField.EnumValue.OriginalLabel;
                            if (!existingEvaluations.Equals(string.Empty))
                            {
                                existingEvaluations += ", ";
                            }

                            existingEvaluations += individualCategory.Name + " (" + fieldValueText + ") ";
                        }
                    }
                }
            }

            return existingEvaluations;
        }

        /// <summary>
        /// The get factor field values.
        /// </summary>
        /// <param name="speciesFact">
        /// The species fact.
        /// </param>
        /// <param name="fieldName">
        /// The field name.
        /// </param>
        /// <param name="fieldValue">
        /// The field value.
        /// </param>
        /// <param name="fieldName2">
        /// The field name 2.
        /// </param>
        /// <param name="fieldValue2">
        /// The field value 2.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetFactorFieldValues(ISpeciesFact speciesFact, out string fieldName, out int fieldValue, out string fieldName2, out int fieldValue2)
        {
            string tempFieldName = string.Empty;
            int tempFieldValue = SpeciesFactNoValueSetValue;
            string tempFieldName2 = string.Empty;
            int tempFieldValue2 = SpeciesFactNoValueSetValue;
            string tempFactorComments = string.Empty;

            // Must get the enum values and comments...
            foreach (ISpeciesFactField tempField in speciesFact.Fields)
            {
                // Must use main field otherwise if ther is several fields using enums it is not possible to know which one
                // to use.
                if (tempField.Type.DataType == FactorFieldDataTypeId.Enum && tempField.IsMain)
                {
                    tempFieldName = tempField.Label;
                    if (tempField.EnumValue.IsNotNull() && tempField.EnumValue.KeyInt.HasValue)
                    {
                        tempFieldValue = tempField.EnumValue.KeyInt.Value;
                    }
                }

                if (tempField.Type.DataType == FactorFieldDataTypeId.Enum && !tempField.IsMain)
                {
                    tempFieldName2 = tempField.Label;
                    if (tempField.EnumValue.IsNotNull() && tempField.EnumValue.KeyInt.HasValue)
                    {
                        tempFieldValue2 = tempField.EnumValue.KeyInt.Value;
                    }
                }

                if (tempField.Type.DataType == FactorFieldDataTypeId.String)
                {
                    if (tempField.Label.Equals("Kommentar"))
                    {
                        tempFactorComments = tempField.StringValue;
                    }
                }
            }

            fieldName = tempFieldName;
            fieldValue = tempFieldValue;
            fieldName2 = tempFieldName2;
            fieldValue2 = tempFieldValue2;
            return tempFactorComments;
        }

        /// <summary>
        /// The get dyntaxa species fact from species fact.
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="spFactor">
        /// The sp factor.
        /// </param>
        /// <returns>
        /// The <see cref="DyntaxaSpeciesFact"/>.
        /// </returns>
        private static DyntaxaSpeciesFact GetDyntaxaSpeciesFactFromSpeciesFact(IUserContext userContext, ISpeciesFact spFactor)
        {
            DyntaxaFactorUpdateMode dyntaxaFactorUpdateMode = new DyntaxaFactorUpdateMode(spFactor.Factor.UpdateMode.IsHeader, true);
            IList<KeyValuePair<int, String>> factorEnumValueList = new List<KeyValuePair<int, string>>();
            IList<KeyValuePair<int, String>> factorEnumValueList2 = new List<KeyValuePair<int, string>>();
            string factorOriginName = string.Empty;
            int factorOriginId = -1;
            DyntaxaFactorOrigin factorOrigin = new DyntaxaFactorOrigin(factorOriginId, factorOriginName);
            if (spFactor.Factor.Origin.IsNotNull() && (spFactor.Factor.Origin.Id > -1))
            {
                factorOrigin.OriginName = spFactor.Factor.Origin.Name;
                factorOrigin.OriginId = spFactor.Factor.Origin.Id;
            }

            // Convert SpeciesFactField to DyntaxaFactorField
            IList<DyntaxaFactorField> fields = new List<DyntaxaFactorField>();
            foreach (ISpeciesFactField field in spFactor.Fields)
            {
                DyntaxaFactorFieldValues fieldValues = null;
                DyntaxaFactorFieldDataTypeId dyntaxaFactorFieldDataTypeId = DyntaxaFactorFieldDataTypeId.String;
                if (field.Type.DataType == FactorFieldDataTypeId.Boolean)
                {
                    dyntaxaFactorFieldDataTypeId = DyntaxaFactorFieldDataTypeId.Boolean;
                    fieldValues = new DyntaxaFactorFieldValues();
                    fieldValues.FieldName = field.Label;
                    fieldValues.FieldValue = Convert.ToInt32(field.BooleanValue);
                    fieldValues.FactorFields.Add(new KeyValuePair<string, int>("sant", 1));
                    fieldValues.FactorFields.Add(new KeyValuePair<string, int>("falskt", 0));
                }

                if (field.Type.DataType == FactorFieldDataTypeId.Double)
                {
                    dyntaxaFactorFieldDataTypeId = DyntaxaFactorFieldDataTypeId.Double;
                }

                if (field.Type.DataType == FactorFieldDataTypeId.Enum && field.IsMain)
                {
                    fieldValues = new DyntaxaFactorFieldValues();
                    fieldValues.FieldName = field.Label;
                    if (field.EnumValue.IsNotNull())
                    {
                        if (field.EnumValue.KeyInt.HasValue)
                        {
                            fieldValues.FieldValue = field.EnumValue.KeyInt.Value;
                        }
                    }

                    foreach (FactorFieldEnumValue factorFieldEnum in field.FactorFieldEnum.Values)
                    {
                        if (factorFieldEnum.KeyInt.HasValue)
                        {
                           factorEnumValueList.Add(new KeyValuePair<int, string>(factorFieldEnum.KeyInt.Value, factorFieldEnum.Label));
                        }
                    }
                }

                if (field.Type.DataType == FactorFieldDataTypeId.Enum && !field.IsMain)
                {
                    fieldValues = new DyntaxaFactorFieldValues();
                    fieldValues.FieldName = field.Label;
                    if (field.EnumValue.IsNotNull())
                    {
                        if (field.EnumValue.KeyInt.HasValue)
                        {
                            fieldValues.FieldValue = field.EnumValue.KeyInt.Value;
                        }
                    }

                    foreach (FactorFieldEnumValue factorFieldEnum in field.FactorFieldEnum.Values)
                    {
                        if (factorFieldEnum.KeyInt.HasValue)
                        {
                            factorEnumValueList2.Add(new KeyValuePair<int, string>(factorFieldEnum.KeyInt.Value, factorFieldEnum.Label));
                        }
                    }
                }

                dyntaxaFactorFieldDataTypeId = DyntaxaFactorFieldDataTypeId.Enum;
                if (field.Type.DataType == FactorFieldDataTypeId.Int32)
                {
                    dyntaxaFactorFieldDataTypeId = DyntaxaFactorFieldDataTypeId.Int32;
                }

                DyntaxaFactorField item = new DyntaxaFactorField(
                    field.Id.ToString(), 
                    field.Label, 
                    field.Unit, 
                    dyntaxaFactorFieldDataTypeId, 
                    field.Value, 
                    field.HasValue, 
                    fieldValues, 
                    field.IsMain);
                fields.Add(item);
            }

            SpeciesFactQualityList qualityList = CoreData.SpeciesFactManager.GetSpeciesFactQualities(userContext);
            IList<KeyValuePair<int, string>> qualities = new List<KeyValuePair<int, string>>();

            foreach (ISpeciesFactQuality spQuality in qualityList)
            {
                qualities.Add(new KeyValuePair<int, string>(spQuality.Id, spQuality.Name));
            }

            IReference reference = CoreData.ReferenceManager.GetReference(userContext, spFactor.Reference.Id);
            IReferenceRelation dyntaxaFactorReference = null;
            if (reference.IsNotNull())
            {
                dyntaxaFactorReference = new ReferenceRelation();
                dyntaxaFactorReference.Reference = new ArtDatabanken.Data.Reference();
                dyntaxaFactorReference.Reference.Name = reference.Name;
                dyntaxaFactorReference.Reference.Year = reference.Year;
                dyntaxaFactorReference.Reference.Id = reference.Id;
            }

            DyntaxaFactorQuality quality = new DyntaxaFactorQuality(spFactor.Quality.Id, spFactor.Quality.Name, qualities);
            DyntaxaIndividualCategory individualCategory = null;
            if (spFactor.IndividualCategory.IsNotNull())
            {
                individualCategory = new DyntaxaIndividualCategory(spFactor.IndividualCategory.Id, spFactor.IndividualCategory.Name, string.Empty);
            }
            else
            {
                IIndividualCategory dafultCat = CoreData.FactorManager.GetDefaultIndividualCategory(userContext);
                individualCategory = new DyntaxaIndividualCategory(dafultCat.Id, dafultCat.Name, string.Empty);
            }

            int hostId = -1;
            if (spFactor.HasHost)
            {
                hostId = spFactor.Host.Id;
            }

            DyntaxaSpeciesFact speciesFact = new DyntaxaSpeciesFact(
                spFactor.Id.ToString(), 
                spFactor.Factor.Information, 
                spFactor.Factor.Label, 
                spFactor.Factor.IsLeaf, 
                spFactor.Factor.IsPeriodic, 
                spFactor.Factor.SortOrder, 
                spFactor.Factor.IsPublic, 
                spFactor.Factor.IsTaxonomic, 
                spFactor.Factor.HostLabel, 
                hostId, 
                quality, 
                spFactor.Reference.Id, 
                fields, 
                factorOrigin, 
                dyntaxaFactorUpdateMode, 
                spFactor.Identifier, 
                spFactor.ModifiedDate, 
                individualCategory, 
                spFactor.Factor.Id, 
                spFactor.Reference.Name, 
                dyntaxaFactorReference, 
                factorEnumValueList, 
                factorEnumValueList2, 
                spFactor.ModifiedBy);
            return speciesFact;
        }

        /// <summary>
        /// Update factor value. To implement them all the way to db 
        /// UpdateDyntaxaSpeciesFacts() must be called. 
        /// </summary>
        /// <param name="newFactorValues">
        /// </param>
        /// <param name="newCategory">
        /// </param>
        /// <param name="updateFieldValue2">
        /// The update Field Value 2.
        /// </param>
        /// <returns>
        /// The <see cref="ISpeciesFact"/>.
        /// </returns>
        private ISpeciesFact SetNewValueForFactor(SpeciesFactFieldValueModelHelper newFactorValues, bool newCategory, bool updateFieldValue2)
        {
            IFactor factor = GetFactor(userContext, newFactorValues.FactorId);
            ITaxon speciesFactsTaxon = taxon;
            IIndividualCategory individualCategory = CoreData.FactorManager.GetIndividualCategory(
                userContext, 
                newFactorValues.IndividualCategoryId);

            // TODO How to get factordata for factor with individual category that don't exist

            // We have a new category
            if (newCategory)
            {
                try
                {
                    // TODO Add get userparameterselectioon and creat a new speciec fact
                    AddNewCategoryToSpeciecFact(
                        speciesFactsTaxon.Id, 
                        newFactorValues.FactorId, 
                        newFactorValues.IndividualCategoryId, 
                        newFactorValues.QualityId, 
                        Convert.ToString(newFactorValues.ReferenceId), 
                        newFactorValues.MainParentFactorId, 
                        newFactorValues.HostId);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            FactorList factorList = new FactorList();
            factorList.Add(factor);
            IndividualCategoryList categoryList = new IndividualCategoryList();
            categoryList.Add(individualCategory);
            SpeciesFactList speciesFactsList = GetSpeciesFacts(speciesFactsTaxon, factorList, categoryList);
            ISpeciesFact speciesFact = null;

            // If we have more than one speciesfact then we have to check hosts todo add period handling?...
            if (speciesFactsList.Count > 1)
            {
                foreach (SpeciesFact fact in speciesFactsList.Cast<SpeciesFact>().Where(fact => fact.HasHost && fact.Host.Id == newFactorValues.HostId))
                {
                    speciesFact = fact;
                    break;
                }
            }
            else if (speciesFactsList.Count == 1)
            {
                // Yes we have a species fact here and it is the selceted category.
                speciesFact = speciesFactsList[0];
            }

            bool setReference = false;

            // Correct speciesFact is found...
            if (speciesFact != null)
            {
                // Check if value has changed, update field 1, field2, and textField5 (implemented now).
                foreach (SpeciesFactField field in speciesFact.Fields)
                {
                    FactorFieldEnumValue enumValue = null;
                    FactorFieldEnumValue enumValue2 = null;

                    // for the moment we know that this is (MainField that has enumValue) the "betydelse" for implemented factors..
                    if (field.Type.DataType == FactorFieldDataTypeId.Enum && field.IsMain)
                    {
                        // Here we get the list of all avalible enum values
                        FactorFieldEnumValueList valList = field.FactorFieldEnum.Values;

                        // We check the key to se if it matches the value set in radiobuttons
                        if (field.EnumValue.IsNull() ||
                            (field.EnumValue.IsNotNull() &&
                             field.EnumValue.KeyInt != Convert.ToInt32(newFactorValues.FactorField1Value)))
                        {
                            if (Convert.ToInt32(newFactorValues.FactorField1Value) == SpeciesFactNoValueSetValue)
                            {
                                // ie handle the not set value then set it to null
                                enumValue = null;
                            }
                            else
                            {
                                // Now we set the newValue and convert it to FactorFieldEnumValue;
                                enumValue = valList.Cast<FactorFieldEnumValue>().FirstOrDefault(val => val.KeyInt.HasValue && Math.Abs(val.KeyInt.Value - newFactorValues.FactorField1Value) < 0.001);
                                setReference = true;
                            }

                            field.Value = enumValue;
                        }
                    }

                    // Here we identify factor "nyttjande" used by Substrate.
                    if (field.Type.DataType == FactorFieldDataTypeId.Enum && !field.IsMain
                        &&
                        (newFactorValues.MainParentFactorId == (int)DyntaxaFactorId.SUBSTRATE ||
                         newFactorValues.MainParentFactorId == (int)DyntaxaFactorId.INFLUENCE))
                    {
                        // Here we get the list of all avalible enum values
                        FactorFieldEnumValueList valList2 = field.FactorFieldEnum.Values;

                        // Check if there is a value in view
                        if (newFactorValues.FactorField2HasValue || updateFieldValue2)
                        {
                            // We check the key to se if it matches the value set in DropdownList
                            if (field.EnumValue.IsNull() ||
                                (field.EnumValue.IsNotNull() &&
                                 field.EnumValue.KeyInt != Convert.ToInt32(newFactorValues.FactorField2Value)))
                            {
                                // Now we set the newValue and convert it to FactorFieldEnumValue;
                                if (Convert.ToInt32(newFactorValues.FactorField2Value) == SpeciesFactNoValueSetValue)
                                {
                                    // ie handle the not set value then set it to null
                                    enumValue2 = null;
                                }
                                else
                                {
                                    enumValue2 = valList2.Cast<FactorFieldEnumValue>().FirstOrDefault(val => val.KeyInt.HasValue && Math.Abs(val.KeyInt.Value - newFactorValues.FactorField2Value) < 0.001);
                                    setReference = true;
                                }

                                // Here we should handle the 
                                // if(enumValue2 == null)
                                // {
                                // enumValue2 = valList2.Cast<FactorFieldEnumValue>().FirstOrDefault(val => val.KeyHasIntegerValue && val.KeyInt == 1);
                                // }
                                field.Value = enumValue2;
                            }
                        }
                    }
                }

                if (newFactorValues.StringValue5.IsNotEmpty())
                {
                    speciesFact.Field5.Value = newFactorValues.StringValue5;
                    setReference = true;
                }

                if (speciesFact.Quality.Id != newFactorValues.QualityId)
                {
                    speciesFact.Quality = CoreData.SpeciesFactManager.GetSpeciesFactQuality(
                        userContext, 
                        newFactorValues.QualityId);
                    setReference = true;
                }

                if (setReference)
                {
                    speciesFact.Reference = CoreData.ReferenceManager.GetReference(
                        userContext, 
                        newFactorValues.ReferenceId);
                }
            }

            return speciesFact;
        }

        /// <summary>
        /// Update database with new species facts.
        /// </summary>
        /// <param name="speciesFactList">
        /// The species Fact List.
        /// </param>
        private void UpdateSpeciesFacts(SpeciesFactList speciesFactList)
        {
            IReference referenceValue = CoreData.ReferenceManager.GetReference(userContext, dyntaxaReference);
            using (ITransaction transaction = userContext.StartTransaction(300))
            {
                CoreData.SpeciesFactManager.UpdateSpeciesFacts(userContext, speciesFactList, referenceValue);                
                transaction.Commit();
            }
        }

        #endregion

        /// <summary>
        /// Gets all hosts from factor with id and all child factors.
        /// Note! Result is undependent of individula category.
        /// </summary>
        /// <param name="factorId">
        /// </param>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        public IList<DyntaxaHost> GetHostByFactorId(int factorId)
        {
            IList<DyntaxaHost> hosts = new List<DyntaxaHost>();            
            ISpeciesFactSearchCriteria searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.EnsureNoListsAreNull();
            searchCriteria.IncludeNotValidHosts = true;
            searchCriteria.IncludeNotValidTaxa = true;
            searchCriteria.Factors = new FactorList { CoreData.FactorManager.GetFactor(userContext, factorId) };
            TaxonList taxonList = CoreData.AnalysisManager.GetHosts(userContext, searchCriteria);            
            foreach (ITaxon taxon in taxonList)
            {
                hosts.Add(new DyntaxaHost(Convert.ToString(taxon.Id), taxon.GetLabel(), taxon.ScientificName, taxon.CommonName));
            }

            return hosts;
        }

        /// <summary>
        /// The get species fact from selected taxa and factors.
        /// </summary>
        /// <param name="taxonId">
        /// The taxon id.
        /// </param>
        /// <param name="factorId">
        /// The factor id.
        /// </param>
        /// <param name="factorHostData">
        /// The factor host data.
        /// </param>
        /// <param name="taxonIds">
        /// The taxon ids.
        /// </param>
        /// <param name="useAllCategories">
        /// The use all categories.
        /// </param>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        /// <exception cref="ApplicationException">
        /// </exception>
        public IList<DyntaxaSpeciesFact> GetSpeciesFactFromSelectedTaxaAndFactors(int taxonId, int factorId, List<SpeciesFactHostsIdListHelper> factorHostData, List<SpeciesFactHostsIdListHelper> taxonIds, bool useAllCategories = false)
        {
            ITaxon speciesFactsTaxon = taxon;
            
            // Get host speciesFacts.
            IList<DyntaxaSpeciesFact> allSpeciesFactsHosts = new List<DyntaxaSpeciesFact>();
            
            // Which categories to search from.
            if (taxonIds.IsNotEmpty())
            {
                foreach (SpeciesFactHostsIdListHelper hostTaxonData in taxonIds)
                {
                    IIndividualCategory categoryHost = CoreData.FactorManager.GetIndividualCategory(userContext, hostTaxonData.CategoryId);
                    IList<DyntaxaSpeciesFact> tempSpeciesFacts = GetSpeciesFactFromHost(hostTaxonData.Id, categoryHost.Id, hostTaxonData.FactorId);
                    if (tempSpeciesFacts.IsNotNull() && tempSpeciesFacts.Count > 0)
                    {
                        foreach (DyntaxaSpeciesFact dyntaxaSpeciesFact in tempSpeciesFacts)
                        {
                            dyntaxaSpeciesFact.HostName = GetHostName(dyntaxaSpeciesFact.HostId);
                        }
                       
                        allSpeciesFactsHosts = allSpeciesFactsHosts.Concat(tempSpeciesFacts).ToList();
                    }
                }
            }

            if (factorHostData.IsNotEmpty())
            {
                foreach (var factor in factorHostData)
                {
                    IList<int> categoryIdList = new List<int>();
                    categoryIdList.Add(factor.CategoryId);
                    IList<DyntaxaSpeciesFact> tempSpeciesFactorFacts = GetSpeciesFactFromFactor(taxonId, factor.Id, categoryIdList);
                    if (tempSpeciesFactorFacts.IsNotNull() && tempSpeciesFactorFacts.Count > 0)
                    {
                        foreach (DyntaxaSpeciesFact tempSpeciesFactorFact in tempSpeciesFactorFacts)
                        {
                            bool exist = false;
                            foreach (DyntaxaSpeciesFact allSpeciesFactsHost in allSpeciesFactsHosts)
                            {
                                if (allSpeciesFactsHost.Id == tempSpeciesFactorFact.Id)
                                {
                                    exist = true;
                                    break;
                                }
                            }

                            if (!exist)
                            {
                                tempSpeciesFactorFact.HostName = GetHostName(tempSpeciesFactorFact.HostId);
                                allSpeciesFactsHosts.Add(tempSpeciesFactorFact);
                            }
                        }
                    }
                }
            }
           
            if (allSpeciesFactsHosts.IsNull() || allSpeciesFactsHosts.Count < 1)
            {
                throw new ApplicationException(DyntaxaResource.SpeciesFactEditHostFactorItemsErrorText);
            }

            return allSpeciesFactsHosts;
        }

        /// <summary>
        /// The get species fact from host.
        /// </summary>
        /// <param name="hostTaxonId">
        /// The host taxon id.
        /// </param>
        /// <param name="categoryId">
        /// The category id.
        /// </param>
        /// <param name="hostFactorId">
        /// The host factor id.
        /// </param>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        /// <exception cref="ApplicationException">
        /// </exception>
        public IList<DyntaxaSpeciesFact> GetSpeciesFactFromHost(int hostTaxonId, int categoryId, int hostFactorId)
        {
            IList<DyntaxaSpeciesFact> dyntaxaSpeciesFacts = new List<DyntaxaSpeciesFact>();
            ITaxon speciesFactsTaxon = taxon;

            List<int> taxonIds = new List<int>();
            taxonIds.Add(hostTaxonId);
            TaxonList taxonList = null;
            taxonList = CoreData.TaxonManager.GetTaxa(userContext, taxonIds);

            IIndividualCategory individualCategory = CoreData.FactorManager.GetIndividualCategory(userContext, categoryId);
            IndividualCategoryList categoryList = new IndividualCategoryList(); 
            categoryList.Add(individualCategory);

            FactorList factorList = new FactorList();
            IFactor factor = CoreData.FactorManager.GetFactor(userContext, hostFactorId);
            factorList.Add(factor);

            // First we must check if selected category is empty, if so null is returned and this is a new individual category for this factor.
            SpeciesFactList speciesFacts = GetSpeciesFacts(speciesFactsTaxon, factorList, categoryList, taxonList, true);

            if (speciesFacts.IsNotEmpty() || speciesFacts.Count == 1)
            {
                foreach (ISpeciesFact speciesFact in speciesFacts)
                {
                    string fieldName = string.Empty;
                    int fieldValue = 0;
                    string fieldName2 = string.Empty;
                    int fieldValue2 = 0;
                    string factorComments = string.Empty;
                    string existingEvaluations = string.Empty;

                    DyntaxaSpeciesFact dyntaxaSpeciesFact = GetDyntaxaSpeciesFactFromSpeciesFact(userContext, speciesFact);
                    factorComments = GetFactorFieldValues(speciesFact, out fieldName, out fieldValue, out fieldName2, out fieldValue2);
                    dyntaxaSpeciesFact.Comments = factorComments;
                    dyntaxaSpeciesFact.FactorEnumValue = fieldValue;
                    dyntaxaSpeciesFact.FactorEnumLabel = fieldName;
                    dyntaxaSpeciesFact.FactorEnumValue2 = fieldValue2;
                    dyntaxaSpeciesFact.FactorEnumLabel2 = fieldName2;

                    existingEvaluations = GetExistingEvaluationValue(speciesFact, existingEvaluations, speciesFact.IndividualCategory);

                    dyntaxaSpeciesFact.IndividualCatgory = new DyntaxaIndividualCategory(speciesFact.IndividualCategory.Id, speciesFact.IndividualCategory.Name, string.Empty);
                    dyntaxaSpeciesFact.ExistingEvaluations = existingEvaluations;
                    dyntaxaSpeciesFacts.Add(dyntaxaSpeciesFact);
                }
                return dyntaxaSpeciesFacts;
            }
            else if (speciesFacts.IsNotEmpty() || speciesFacts.Count == 0)
            {
                return dyntaxaSpeciesFacts;
            }
            else
            {
                throw new ApplicationException(DyntaxaResource.SpeciesFactEditHostFactorItemsErrorText);
            }
        }

        /// <summary>
        /// The get species fact from factor.
        /// </summary>
        /// <param name="taxonId">
        /// The taxon id.
        /// </param>
        /// <param name="factorId">
        /// The factor id.
        /// </param>
        /// <param name="categoryIds">
        /// The category ids.
        /// </param>
        /// <param name="selectedHosts">
        /// The selected hosts.
        /// </param>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        public IList<DyntaxaSpeciesFact> GetSpeciesFactFromFactor(int taxonId, int factorId, IList<int> categoryIds, List<SpeciesFactHostsIdListHelper> selectedHosts = null)
        {
            IList<DyntaxaSpeciesFact> dyntaxaSpeciesFacts = new List<DyntaxaSpeciesFact>();
            ITaxon speciesFactsTaxon = taxon;

            FactorList factorList = new FactorList();
            factorList.Add(CoreData.FactorManager.GetFactor(userContext, factorId));
            IndividualCategoryList categoryList = new IndividualCategoryList();
            foreach (int categoryId in categoryIds)
            {
                IIndividualCategory individualCategory = CoreData.FactorManager.GetIndividualCategory(userContext, categoryId);
                categoryList.Add(individualCategory);
            }

            TaxonList hostTaxonList = new TaxonList();
            if (selectedHosts.IsNotNull())
            {
                foreach (var tempHostTaxon in selectedHosts)
                {
                    ITaxon hostTaxon = CoreData.TaxonManager.GetTaxon(userContext, tempHostTaxon.Id);
                    hostTaxonList.Add(hostTaxon);
                }
            }

            // First we must check if selected category is empty, if so null is returned and this is a new individual category for this factor.
            SpeciesFactList speciesFacts = GetSpeciesFacts(speciesFactsTaxon, factorList, categoryList, hostTaxonList, true);
            if (speciesFacts.IsNotEmpty())
            {
                foreach (SpeciesFact speciesFact in speciesFacts)
                {
                    string fieldName = string.Empty;
                    int fieldValue = 0;
                    string fieldName2 = string.Empty;
                    int fieldValue2 = 0;
                    string factorComments = string.Empty;
                    string existingEvaluations = string.Empty;

                    DyntaxaSpeciesFact dyntaxaSpeciesFact = GetDyntaxaSpeciesFactFromSpeciesFact(userContext, speciesFact);

                    factorComments = GetFactorFieldValues(speciesFact, out fieldName, out fieldValue, out fieldName2, out fieldValue2);
                    dyntaxaSpeciesFact.Comments = factorComments;
                    dyntaxaSpeciesFact.FactorEnumValue = fieldValue;
                    dyntaxaSpeciesFact.FactorEnumLabel = fieldName;
                    dyntaxaSpeciesFact.FactorEnumValue2 = fieldValue2;
                    dyntaxaSpeciesFact.FactorEnumLabel2 = fieldName2;

                    existingEvaluations = GetExistingEvaluationValue(speciesFact, existingEvaluations, speciesFact.IndividualCategory);

                    dyntaxaSpeciesFact.IndividualCatgory = new DyntaxaIndividualCategory(speciesFact.IndividualCategory.Id, speciesFact.IndividualCategory.Name, string.Empty);
                    dyntaxaSpeciesFact.ExistingEvaluations = existingEvaluations;
                    if (speciesFact.Host.Id != 0)
                    {
                        dyntaxaSpeciesFacts.Add(dyntaxaSpeciesFact);
                    }
                }
            }
            else
            {
               // throw new ApplicationException(Resources.DyntaxaResource.SpeciesFactEditHostFactorItemsErrorText);
            }

            return dyntaxaSpeciesFacts;
        }

        // public string GetFactorName(int factorId)
        // {
        // ArtDatabanken.Data.Factor factor = ArtDatabanken.Data.FactorManager.GetFactor(factorId);
        // return factor.Label;
        // }

        /// <summary>
        /// The get host name.
        /// </summary>
        /// <param name="hostId">
        /// The host id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetHostName(int hostId)
        {
            return CoreData.TaxonManager.GetTaxon(userContext, hostId).GetLabel(); // vad är Label?
        }
    }
    #region ENUMVALUES USED FOR EDITING FACTORS

    /// <summary>
    /// The dyntaxa factor id.
    /// </summary>
    public enum DyntaxaFactorId
    {
        /// <summary>
        /// The no t_ supported.
        /// </summary>
        NOT_SUPPORTED = 0, 

        /// <summary>
        /// The substrate.
        /// </summary>
        SUBSTRATE = 986, 

        /// <summary>
        /// The substrat e_ a s_ shortlist.
        /// </summary>
        SUBSTRATE_AS_SHORTLIST = 2520, 

        /// <summary>
        /// The influence.
        /// </summary>
        INFLUENCE = 1618, 

        /// <summary>
        /// The biotope.
        /// </summary>
        BIOTOPE = 1321, 

        /// <summary>
        /// The lifeform.
        /// </summary>
        LIFEFORM = 1859, 

        /// <summary>
        /// The specie s_ a s_ substrate.
        /// </summary>
        SPECIES_AS_SUBSTRATE = 1136
    }

    /// <summary>
    /// The dyntaxa data type.
    /// </summary>
    public enum DyntaxaDataType
    {
        /// <summary>
        /// The no t_ supported.
        /// </summary>
        NOT_SUPPORTED = -100, 

        /// <summary>
        /// The enum.
        /// </summary>
        ENUM = 1, 

        /// <summary>
        /// The boolean.
        /// </summary>
        BOOLEAN = 0, 
    }

    /// <summary>
    /// The dyntaxa factor data type.
    /// </summary>
    public enum DyntaxaFactorDataType
    {
        /// <summary>
        /// The no t_ supported.
        /// </summary>
        NOT_SUPPORTED = 0, 

        /// <summary>
        /// The a f_ substrate.
        /// </summary>
        AF_SUBSTRATE = 42, 

        /// <summary>
        /// The a f_ influence.
        /// </summary>
        AF_INFLUENCE = 71, 

        /// <summary>
        /// The a f_ biotope.
        /// </summary>
        AF_BIOTOPE = 39, 

        /// <summary>
        /// The a f_ lifeform.
        /// </summary>
        AF_LIFEFORM = 1
    }

    /// <summary>
    /// The factor short list manager.
    /// </summary>
    public static class FactorShortListManager
    {
        /// <summary>
        /// The _substrate ids.
        /// </summary>
        private static List<Int32> _substrateIds = null;

        /// <summary>
        /// The _host substrate ids.
        /// </summary>
        private static List<Int32> _hostSubstrateIds = null;

        /// <summary>
        /// The _habitat ids.
        /// </summary>
        private static List<Int32> _habitatIds = null;

        /// <summary>
        /// The _influence ids.
        /// </summary>
        private static List<Int32> _influenceIds = null;

        /// <summary>
        /// 2520.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public static List<Int32> GetSubstrateFactorShortlist()
        {
            if (_substrateIds.IsNull())
            {
                _substrateIds = new List<Int32>();

            // _substrateIds.Add(2520);
                _substrateIds.Add(989);
                _substrateIds.Add(1036);
                _substrateIds.Add(1080);
                _substrateIds.Add(1083);
                _substrateIds.Add(1091);
            }

            return _substrateIds;
        }

        /// <summary>
        /// 2520.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public static List<Int32> GetHostSubstrateFactorShortlist()
        {
            if (_hostSubstrateIds.IsNull())
            {
                _hostSubstrateIds = new List<Int32>();
                _hostSubstrateIds.Add(1136);
                _hostSubstrateIds.Add(1137);
                _hostSubstrateIds.Add(1142);
                _hostSubstrateIds.Add(1165);
                _hostSubstrateIds.Add(2458);
                _hostSubstrateIds.Add(2459);
                _hostSubstrateIds.Add(1222);
                _hostSubstrateIds.Add(2495);
            }

            return _hostSubstrateIds;
        }

        /// <summary>
        /// 2519.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public static List<Int32> GetHabitatFactorShortlist()
        {
            if (_habitatIds.IsNull())
            {
                _habitatIds = new List<Int32>();

             // _habitatIds.Add(2519);
                _habitatIds.Add(2487);
                _habitatIds.Add(2488);
                _habitatIds.Add(1339);
                _habitatIds.Add(2489);
                _habitatIds.Add(1386);
                _habitatIds.Add(1405);
                _habitatIds.Add(801);
                _habitatIds.Add(878);
                _habitatIds.Add(865);
                _habitatIds.Add(880);
                _habitatIds.Add(879);
                _habitatIds.Add(883);
                _habitatIds.Add(888);
                _habitatIds.Add(884);
                _habitatIds.Add(1484);
                _habitatIds.Add(2492);
                _habitatIds.Add(2491);
                _habitatIds.Add(1507);
                _habitatIds.Add(2493);
                _habitatIds.Add(2494);
                _habitatIds.Add(1585);
                _habitatIds.Add(1550);
                _habitatIds.Add(1535);
                _habitatIds.Add(1592);
                _habitatIds.Add(2524);
                _habitatIds.Add(2522);
                _habitatIds.Add(2523);
            }

            return _habitatIds;
        }

        /// <summary>
        /// 2521.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public static List<Int32> GetInfluenceFactorShortlist()
        {
            if (_influenceIds.IsNull())
            {
                _influenceIds = new List<Int32>();
                _influenceIds.Add(2517);
                _influenceIds.Add(2181);

             // _influenceIds.Add(2521);
                _influenceIds.Add(1805);
                _influenceIds.Add(2117);
                _influenceIds.Add(1821);
                _influenceIds.Add(2120);
                _influenceIds.Add(2124);

// Replaced by factor 2129; 2013-12-11  as input from Jonas Sandström _influenceIds.Add(2130);
                _influenceIds.Add(2129);
                _influenceIds.Add(2116);
                _influenceIds.Add(2126); // Added pectcider/herbicider 2013-12-13
                _influenceIds.Add(2119);
                _influenceIds.Add(2020);
                _influenceIds.Add(2511);
                _influenceIds.Add(2515);
                _influenceIds.Add(2157);
                _influenceIds.Add(2163);
                _influenceIds.Add(2518);
                _influenceIds.Add(2169);
                _influenceIds.Add(2172);
                _influenceIds.Add(1712);
                _influenceIds.Add(1764);
                _influenceIds.Add(2500);
                _influenceIds.Add(2516);
                _influenceIds.Add(1826);
                _influenceIds.Add(2504);
                _influenceIds.Add(1829);
                _influenceIds.Add(2503);

             // _influenceIds.Add(2501);
             // _influenceIds.Add(2502);
                _influenceIds.Add(1827);
                _influenceIds.Add(2513);
                _influenceIds.Add(2514);
            }

            return _influenceIds;
        }
    }
#endregion
}

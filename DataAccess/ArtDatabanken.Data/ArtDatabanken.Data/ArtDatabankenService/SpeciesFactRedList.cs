using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Base class for other species facts classes that
    /// calculates red list  values.
    /// </summary>
    [Serializable]
    public class SpeciesFactRedList : SpeciesFact
    {
        /// <summary>
        /// Creates a species fact instance with no data from web service.
        /// </summary>
        /// <param name="taxon">Taxon object of the species fact</param>
        /// <param name="individualCategory">Individual category object of the species fact</param>
        /// <param name="factor">Factor object of the species fact</param>
        /// <param name="host">Host taxon object of the species fact</param>
        /// <param name="period">Period object of the species fact</param>
        public SpeciesFactRedList(
            Taxon taxon,
            IndividualCategory individualCategory,
            Factor factor,
            Taxon host,
            Period period)
            : base(taxon, individualCategory, factor, host, period)
        {
            IsInInit = false;
            RedListCalculator = null;
        }

        /// <summary>
        /// Creates a species fact instance with data from web service.
        /// </summary>
        /// <param name="id">Id of the species fact</param>
        /// <param name="sortOrder">Sort order of the species fact</param>
        /// <param name="taxonId">Taxon Id of the species fact</param>
        /// <param name="individualCategoryId">Individual Category Id of the species fact</param>
        /// <param name="factorId">Foctor Id of the species fact</param>
        /// <param name="hostId">Taxon Id of the host taxon associated with the species fact</param>
        /// <param name="hasHost">Indicates if this species fact has a host.</param>
        /// <param name="periodId">Period Id of the species fact</param>
        /// <param name="hasPeriod">Indicates if this species fact has a period.</param>
        /// <param name="fieldValue1">Field value of field 1 for the species fact</param>
        /// <param name="hasFieldValue1">Indicates if field 1 has a value.</param>
        /// <param name="fieldValue2">Field value of field 2 for the species fact</param>
        /// <param name="hasFieldValue2">Indicates if field 2 has a value.</param>
        /// <param name="fieldValue3">Field value of field 3 for the species fact</param>
        /// <param name="hasFieldValue3">Indicates if field 3 has a value.</param>
        /// <param name="fieldValue4">Field value of field 4 for the species fact</param>
        /// <param name="hasFieldValue4">Indicates if field 4 has a value.</param>
        /// <param name="fieldValue5">Field value of field 5 for the species fact</param>
        /// <param name="hasFieldValue5">Indicates if field 5 has a value.</param>
        /// <param name="qualityId">Quality Id of the species fact</param>
        /// <param name="referenceId">Reference id of the species fact</param>
        /// <param name="updateUserFullName">Full Name of the pdate user of the species fact</param>
        /// <param name="updateDate">Update date of the species fact</param>
        public SpeciesFactRedList(
            Int32 id,
            Int32 sortOrder,
            Int32 taxonId,
            Int32 individualCategoryId,
            Int32 factorId,
            Int32 hostId,
            Boolean hasHost,
            Int32 periodId,
            Boolean hasPeriod,
            Double fieldValue1,
            Boolean hasFieldValue1,
            Double fieldValue2,
            Boolean hasFieldValue2,
            Double fieldValue3,
            Boolean hasFieldValue3,
            String fieldValue4,
            Boolean hasFieldValue4,
            String fieldValue5,
            Boolean hasFieldValue5,
            Int32 qualityId,
            Int32 referenceId,
            String updateUserFullName,
            DateTime updateDate)
            : base(id,
                   sortOrder,
                   taxonId,
                   individualCategoryId,
                   factorId,
                   hostId,
                   hasHost,
                   periodId,
                   hasPeriod,
                   fieldValue1,
                   hasFieldValue1,
                   fieldValue2,
                   hasFieldValue2,
                   fieldValue3,
                   hasFieldValue3,
                   fieldValue4,
                   hasFieldValue4,
                   fieldValue5,
                   hasFieldValue5,
                   qualityId,
                   referenceId,
                   updateUserFullName,
                   updateDate)
        {
            IsInInit = false;
            RedListCalculator = null;
        }

        /// <summary>
        /// Creates a species fact instance with data from web service.
        /// </summary>
        /// <param name="id">Id of the species fact</param>
        /// <param name="sortOrder">Sort order of the species fact</param>
        /// <param name="taxon">Taxon of the species fact</param>
        /// <param name="individualCategoryId">Individual Category Id of the species fact</param>
        /// <param name="factorId">Foctor Id of the species fact</param>
        /// <param name="host">Host taxon associated with the species fact</param>
        /// <param name="hasHost">Indicates if this species fact has a host.</param>
        /// <param name="periodId">Period Id of the species fact</param>
        /// <param name="hasPeriod">Indicates if this species fact has a period.</param>
        /// <param name="fieldValue1">Field value of field 1 for the species fact</param>
        /// <param name="hasFieldValue1">Indicates if field 1 has a value.</param>
        /// <param name="fieldValue2">Field value of field 2 for the species fact</param>
        /// <param name="hasFieldValue2">Indicates if field 2 has a value.</param>
        /// <param name="fieldValue3">Field value of field 3 for the species fact</param>
        /// <param name="hasFieldValue3">Indicates if field 3 has a value.</param>
        /// <param name="fieldValue4">Field value of field 4 for the species fact</param>
        /// <param name="hasFieldValue4">Indicates if field 4 has a value.</param>
        /// <param name="fieldValue5">Field value of field 5 for the species fact</param>
        /// <param name="hasFieldValue5">Indicates if field 5 has a value.</param>
        /// <param name="qualityId">Quality Id of the species fact</param>
        /// <param name="referenceId">Reference id of the species fact</param>
        /// <param name="updateUserFullName">Full Name of the pdate user of the species fact</param>
        /// <param name="updateDate">Update date of the species fact</param>
        public SpeciesFactRedList(
            Int32 id,
            Int32 sortOrder,
            Taxon taxon,
            Int32 individualCategoryId,
            Int32 factorId,
            Taxon host,
            Boolean hasHost,
            Int32 periodId,
            Boolean hasPeriod,
            Double fieldValue1,
            Boolean hasFieldValue1,
            Double fieldValue2,
            Boolean hasFieldValue2,
            Double fieldValue3,
            Boolean hasFieldValue3,
            String fieldValue4,
            Boolean hasFieldValue4,
            String fieldValue5,
            Boolean hasFieldValue5,
            Int32 qualityId,
            Int32 referenceId,
            String updateUserFullName,
            DateTime updateDate)
            : base(id,
                   sortOrder,
                   taxon,
                   individualCategoryId,
                   factorId,
                   host,
                   hasHost,
                   periodId,
                   hasPeriod,
                   fieldValue1,
                   hasFieldValue1,
                   fieldValue2,
                   hasFieldValue2,
                   fieldValue3,
                   hasFieldValue3,
                   fieldValue4,
                   hasFieldValue4,
                   fieldValue5,
                   hasFieldValue5,
                   qualityId,
                   referenceId,
                   updateUserFullName,
                   updateDate)
        {
            IsInInit = false;
            RedListCalculator = new RedListCalculator(taxon.TaxonType.Name, taxon.TaxonType.NameDefinite);
        }

        /// <summary>
        /// Flag that indicates if information is initialized.
        /// This flag is used to avoid unnecessary calculations.
        /// </summary>
        protected Boolean IsInInit
        { get; set; }

        /// <summary>
        /// Get RedListCalculator instance.
        /// </summary>
        protected RedListCalculator RedListCalculator
        { get; set; }

        /// <summary>
        /// Get species fact with the same, Taxon, IndividualCategory,
        /// Host and Period as this species fact but with
        /// another factor.
        /// </summary>
        /// <param name="speciesFacts">
        /// List of species facts from where the requested
        /// species are fetched.
        /// </param>
        /// <param name="factor">Factor to use in species fact identifier.</param>
        /// <returns>Requested species fact.</returns>
        protected SpeciesFact GetSpeciesFact(SpeciesFactList speciesFacts,
                                             Factor factor)
        {
            Int32 hostId = 0, periodId = 0;
            String speciesFactIdentifier;

            if (HasHost)
            {
                hostId = Host.Id;
            }
            if (HasPeriod)
            {
                periodId = Period.Id;
            }
            speciesFactIdentifier = SpeciesFactManager.GetSpeciesFactIdentifier(Taxon.Id,
                                                                                IndividualCategory.Id,
                                                                                factor.Id,
                                                                                HasHost,
                                                                                hostId,
                                                                                HasPeriod && factor.IsPeriodic,
                                                                                periodId);
            return speciesFacts.Get(speciesFactIdentifier);
        }

        /// <summary>
        /// Extract min, probable and max value from a species fact.
        /// </summary>
        /// <param name="speciesFact">Species facts with red list information.</param>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="valueMin">Min value.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="valueProbable">Probable value.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="valueMax">Max value.</param>
        protected void GetValues(SpeciesFact speciesFact,
                                 ref Boolean hasMin,
                                 ref Double valueMin,
                                 ref Boolean hasProbable,
                                 ref Double valueProbable,
                                 ref Boolean hasMax,
                                 ref Double valueMax)
        {
            valueMin = 0;
            valueProbable = 0;
            valueMax = 0;
            hasMin = speciesFact.Field2.HasValue;
            if (hasMin)
            {
                valueMin = speciesFact.Field2.GetDouble();
            }
            hasProbable = speciesFact.Field1.HasValue;
            if (hasProbable)
            {
                valueProbable = speciesFact.Field1.GetDouble();
            }
            hasMax = speciesFact.Field3.HasValue;
            if (hasMax)
            {
                valueMax = speciesFact.Field3.GetDouble();
            }
        }

        /// <summary>
        /// Extract min, probable and max value from a species fact.
        /// </summary>
        /// <param name="speciesFact">Species facts with red list information.</param>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="valueMin">Min value.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="valueProbable">Probable value.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="valueMax">Max value.</param>
        protected void GetValues(SpeciesFact speciesFact,
                                 ref Boolean hasMin,
                                 ref Int32 valueMin,
                                 ref Boolean hasProbable,
                                 ref Int32 valueProbable,
                                 ref Boolean hasMax,
                                 ref Int32 valueMax)
        {
            valueMin = 0;
            valueProbable = 0;
            valueMax = 0;
            hasMin = speciesFact.Field2.HasValue;
            if (hasMin)
            {
                valueMin = speciesFact.Field2.GetInt32();
            }
            hasProbable = speciesFact.Field1.HasValue;
            if (hasProbable)
            {
                valueProbable = speciesFact.Field1.GetInt32();
            }
            hasMax = speciesFact.Field3.HasValue;
            if (hasMax)
            {
                valueMax = speciesFact.Field3.GetInt32();
            }
        }

        /// <summary>
        /// Extract min, probable and max value from a species fact.
        /// </summary>
        /// <param name="speciesFact">Species facts with red list information.</param>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="valueMin">Min value.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="valueProbable">Probable value.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="valueMax">Max value.</param>
        protected void GetValues(SpeciesFact speciesFact,
                                 ref Boolean hasMin,
                                 ref Int64 valueMin,
                                 ref Boolean hasProbable,
                                 ref Int64 valueProbable,
                                 ref Boolean hasMax,
                                 ref Int64 valueMax)
        {
            valueMin = 0;
            valueProbable = 0;
            valueMax = 0;
            hasMin = speciesFact.Field2.HasValue;
            if (hasMin)
            {
                valueMin = speciesFact.Field2.GetInt64();
            }
            hasProbable = speciesFact.Field1.HasValue;
            if (hasProbable)
            {
                valueProbable = speciesFact.Field1.GetInt64();
            }
            hasMax = speciesFact.Field3.HasValue;
            if (hasMax)
            {
                valueMax = speciesFact.Field3.GetInt64();
            }
        }

        /// <summary>
        /// Init calculation of red list criteria.
        /// </summary>
        /// <param name="speciesFacts">Species facts to get data from.</param>
        public virtual void Init(SpeciesFactList speciesFacts)
        {
            SpeciesFact speciesFact;

            if (AllowAutomaticUpdate)
            {
                IsInInit = true;
                RedListCalculator.InitBegin();
                foreach (Factor factor in this.Factor.GetDependentFactors())
                {
                    speciesFact = GetSpeciesFact(speciesFacts, factor);
                    SetSpeciesFact(speciesFact);
                    speciesFact.UpdateEvent += new SpeciesFactUpdateEventHandler(SetSpeciesFact);
                }
                RedListCalculator.InitEnd();
                IsInInit = false;
            }
        }

        /// <summary>
        /// Update species fact fields with latest red list values.
        /// </summary>
        protected virtual void SetReadListValues()
        {
        }

        /// <summary>
        /// Update red list calculation with information from species fact.
        /// </summary>
        /// <param name="speciesFact">Species facts with red list information.</param>
        protected virtual void SetSpeciesFact(SpeciesFact speciesFact)
        {
            Boolean hasMax = false, hasMin = false, hasProbable = false;
            Boolean oldIsGraded = false;
            Double doubleMax = 0, doubleMin = 0, doubleProbable = 0;
            Int32 oldGrading = 0;
            Int32 int32Max = 0, int32Min = 0, int32Probable = 0;
            Int64 int64Max = 0, int64Min = 0, int64Probable = 0;
            RedListCategory oldCategory = RedListCategory.LC,
                              oldCategoryBestCase = RedListCategory.LC,
                              oldCategoryProbable = RedListCategory.LC,
                              oldCategoryWorstCase = RedListCategory.LC;
            String oldCriteria = null, oldCriteriaBestCase = null,
                   oldCriteriaProbable = null, oldCriteriaWorstCase = null, oldCriteriaDocumentation = null;

            if (!IsInInit)
            {
                oldCategory = RedListCalculator.Category;
                oldCategoryBestCase = RedListCalculator.CategoryBestCaseNoGrading;
                oldCategoryProbable = RedListCalculator.CategoryProbableNoGrading;
                oldCategoryWorstCase = RedListCalculator.CategoryWorstCaseNoGrading;
                oldCriteria = RedListCalculator.Criteria;
                oldCriteriaBestCase = RedListCalculator.CriteriaBestCase;
                oldCriteriaProbable = RedListCalculator.CriteriaProbable;
                oldCriteriaWorstCase = RedListCalculator.CriteriaWorstCase;
                oldGrading = RedListCalculator.Grading;
                oldIsGraded = RedListCalculator.IsGraded;
                oldCriteriaDocumentation = RedListCalculator.CriteriaDocumentation;
            }

            switch (speciesFact.Factor.Id)
            {
                case ((Int32)FactorId.AreaOfOccupancy_B2Estimated):
                    GetValues(speciesFact,
                              ref hasMin,
                              ref doubleMin,
                              ref hasProbable,
                              ref doubleProbable,
                              ref hasMax,
                              ref doubleMax);
                    RedListCalculator.SetAreaOfOccupancy(hasMin,
                                                          doubleMin,
                                                          hasProbable,
                                                          doubleProbable,
                                                          hasMax,
                                                          doubleMax,
                                                          speciesFact.Field4.GetStringValue(),
                                                          speciesFact.Field1.UnitLabel);
                    break;
                case ((Int32)FactorId.ConservationDependent):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsConservationDependent(speciesFact.Field1.GetBoolean(),
                                                                     speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsConservationDependent(false, null);
                    }
                    break;
                case ((Int32)FactorId.ContinuingDecline):
                    hasProbable = speciesFact.Field1.HasValue;
                    if (hasProbable)
                    {
                        int32Probable = speciesFact.Field1.GetInt32();
                    }
                    RedListCalculator.SetContinuingDecline(hasMin,
                                                           int32Min,
                                                           hasProbable,
                                                           int32Probable,
                                                           hasMax,
                                                           int32Max,
                                                           speciesFact.Field4.GetStringValue());
                    break;
                case ((Int32)FactorId.ContinuingDeclineBasedOn_Bbi):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaBB1Fulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaBB1Fulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ContinuingDeclineBasedOn_Bbii):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaBB2Fulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaBB2Fulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ContinuingDeclineBasedOn_Bbiii):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaBB3Fulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaBB3Fulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ContinuingDeclineBasedOn_Bbiv):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaBB4Fulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaBB4Fulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ContinuingDeclineBasedOn_Bbv):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaBB5Fulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaBB5Fulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ExtentOfOccurrence_B1Estimated):
                    GetValues(speciesFact,
                              ref hasMin,
                              ref doubleMin,
                              ref hasProbable,
                              ref doubleProbable,
                              ref hasMax,
                              ref doubleMax);
                    RedListCalculator.SetExtentOfOccurrence(hasMin,
                                                            doubleMin,
                                                            hasProbable,
                                                            doubleProbable,
                                                            hasMax,
                                                            doubleMax,
                                                            speciesFact.Field4.GetStringValue(),
                                                            speciesFact.Field1.UnitLabel);
                    break;
                case ((Int32)FactorId.ExtremeFluctuations):
                    hasProbable = speciesFact.Field1.HasValue;
                    if (hasProbable)
                    {
                        int32Probable = speciesFact.Field1.GetInt32();
                    }
                    RedListCalculator.SetExtremeFluctuations(hasMin,
                                                              int32Min,
                                                              hasProbable,
                                                              int32Probable,
                                                              hasMax,
                                                              int32Max);
                    break;
                case ((Int32)FactorId.ExtremeFluctuationsIn_Bci):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaBC1Fulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaBC1Fulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ExtremeFluctuationsIn_Bcii):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaBC2Fulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaBC2Fulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ExtremeFluctuationsIn_Bciii):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaBC3Fulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaBC3Fulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ExtremeFluctuationsIn_Bciv):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaBC4Fulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaBC4Fulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.Grading):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetGrading(true,
                                                     speciesFact.Field1.GetInt32(),
                                                     speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetGrading(false, 0, null);
                    }
                    break;
                case ((Int32)FactorId.MaxProportionLocalPopulation):
                    GetValues(speciesFact,
                              ref hasMin,
                              ref doubleMin,
                              ref hasProbable,
                              ref doubleProbable,
                              ref hasMax,
                              ref doubleMax);
                    RedListCalculator.SetMaxProportionLocalPopulation(hasMin,
                                                                       doubleMin,
                                                                       hasProbable,
                                                                       doubleProbable,
                                                                       hasMax,
                                                                       doubleMax);
                    break;
                case ((Int32)FactorId.MaxSizeLocalPopulation):
                    GetValues(speciesFact,
                              ref hasMin,
                              ref int64Min,
                              ref hasProbable,
                              ref int64Probable,
                              ref hasMax,
                              ref int64Max);
                    RedListCalculator.SetMaxSizeLocalPopulation(hasMin,
                                                                 int64Min,
                                                                 hasProbable,
                                                                 int64Probable,
                                                                 hasMax,
                                                                 int64Max);
                    break;
                case ((Int32)FactorId.NumberOfLocations):
                    GetValues(speciesFact,
                              ref hasMin,
                              ref int64Min,
                              ref hasProbable,
                              ref int64Probable,
                              ref hasMax,
                              ref int64Max);
                    RedListCalculator.SetNumberOfLocations(hasMin,
                                                           int64Min,
                                                           hasProbable,
                                                           int64Probable,
                                                           hasMax,
                                                           int64Max,
                                                           speciesFact.Field4.GetStringValue());
                    break;
                case ((Int32)FactorId.PopulationSize_Total):
                    GetValues(speciesFact,
                              ref hasMin,
                              ref int64Min,
                              ref hasProbable,
                              ref int64Probable,
                              ref hasMax,
                              ref int64Max);
                    RedListCalculator.SetPopulationSize(hasMin,
                                                        int64Min,
                                                        hasProbable,
                                                        int64Probable,
                                                        hasMax,
                                                        int64Max,
                                                        speciesFact.Field4.GetStringValue());
                    break;
                case ((Int32)FactorId.ProbabilityOfExtinction):
                    GetValues(speciesFact,
                              ref hasMin,
                              ref int32Min,
                              ref hasProbable,
                              ref int32Probable,
                              ref hasMax,
                              ref int32Max);
                    RedListCalculator.SetProbabilityOfExtinction(hasMin,
                                                                  int32Min,
                                                                  hasProbable,
                                                                  int32Probable,
                                                                  hasMax,
                                                                  int32Max,
                                                                  speciesFact.Field4.GetStringValue());
                    break;
                case ((Int32)FactorId.RedlistEvaluationProgressionStatus):
                    RedListCalculator.IsEvaluationStatusSet = speciesFact.Field1.HasValue;
                    if (speciesFact.Field1.HasValue)
                    {
                        switch (speciesFact.Field1.GetInt32())
                        {
                            case 0:
                                RedListCalculator.SetRedlistEvaluationStatus(RedListCategory.NE);
                                break;
                            case 1:
                                RedListCalculator.SetRedlistEvaluationStatus(RedListCategory.NA);
                                break;
                            case 4:
                                RedListCalculator.SetRedlistEvaluationStatus(RedListCategory.DD);
                                break;
                            default:
                                RedListCalculator.SetRedlistEvaluationStatus(RedListCategory.LC);
                                break;
                        }
                    }
                    else
                    {
                        RedListCalculator.SetRedlistEvaluationStatus(RedListCategory.LC);
                    }
                    break;
                case ((Int32)FactorId.Reduction_A1):
                    GetValues(speciesFact,
                              ref hasMin,
                              ref doubleMin,
                              ref hasProbable,
                              ref doubleProbable,
                              ref hasMax,
                              ref doubleMax);
                    RedListCalculator.SetPopulationReductionA1(hasMin,
                                                               doubleMin,
                                                               hasProbable,
                                                               doubleProbable,
                                                               hasMax,
                                                               doubleMax,
                                                               speciesFact.Field4.GetStringValue());
                    break;
                case ((Int32)FactorId.Reduction_A2):
                    GetValues(speciesFact,
                              ref hasMin,
                              ref doubleMin,
                              ref hasProbable,
                              ref doubleProbable,
                              ref hasMax,
                              ref doubleMax);
                    RedListCalculator.SetPopulationReductionA2(hasMin,
                                                               doubleMin,
                                                               hasProbable,
                                                               doubleProbable,
                                                               hasMax,
                                                               doubleMax,
                                                               speciesFact.Field4.GetStringValue());
                    break;
                case ((Int32)FactorId.Reduction_A3):
                    GetValues(speciesFact,
                              ref hasMin,
                              ref doubleMin,
                              ref hasProbable,
                              ref doubleProbable,
                              ref hasMax,
                              ref doubleMax);
                    RedListCalculator.SetPopulationReductionA3(hasMin,
                                                               doubleMin,
                                                               hasProbable,
                                                               doubleProbable,
                                                               hasMax,
                                                               doubleMax,
                                                               speciesFact.Field4.GetStringValue());
                    break;
                case ((Int32)FactorId.Reduction_A4):
                    GetValues(speciesFact,
                              ref hasMin,
                              ref doubleMin,
                              ref hasProbable,
                              ref doubleProbable,
                              ref hasMax,
                              ref doubleMax);
                    RedListCalculator.SetPopulationReductionA4(hasMin,
                                                               doubleMin,
                                                               hasProbable,
                                                               doubleProbable,
                                                               hasMax,
                                                               doubleMax,
                                                               speciesFact.Field4.GetStringValue());
                    break;
                case ((Int32)FactorId.ReductionBasedOn_A1a):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaA1AFulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaA1AFulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ReductionBasedOn_A1b):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaA1BFulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaA1BFulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ReductionBasedOn_A1c):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaA1CFulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaA1CFulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ReductionBasedOn_A1d):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaA1DFulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaA1DFulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ReductionBasedOn_A1e):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaA1EFulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaA1EFulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ReductionBasedOn_A2a):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaA2AFulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaA2AFulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ReductionBasedOn_A2b):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaA2BFulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaA2BFulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ReductionBasedOn_A2c):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaA2CFulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaA2CFulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ReductionBasedOn_A2d):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaA2DFulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaA2DFulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ReductionBasedOn_A2e):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaA2EFulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaA2EFulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ReductionBasedOn_A3b):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaA3BFulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaA3BFulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ReductionBasedOn_A3c):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaA3CFulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaA3CFulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ReductionBasedOn_A3d):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaA3DFulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaA3DFulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ReductionBasedOn_A3e):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaA3EFulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaA3EFulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ReductionBasedOn_A4a):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaA4AFulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaA4AFulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ReductionBasedOn_A4b):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaA4BFulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaA4BFulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ReductionBasedOn_A4c):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaA4CFulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaA4CFulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ReductionBasedOn_A4d):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaA4DFulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaA4DFulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.ReductionBasedOn_A4e):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetIsCriteriaA4EFulfilled(speciesFact.Field1.GetBoolean(),
                                                                    speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        RedListCalculator.SetIsCriteriaA4EFulfilled(false, null);
                    }
                    break;
                case ((Int32)FactorId.SeverelyFragmented):
                    hasProbable = speciesFact.Field1.HasValue;
                    if (hasProbable)
                    {
                        int32Probable = speciesFact.Field1.GetInt32();
                    }
                    RedListCalculator.SetSeverlyFragmented(hasMin,
                                                            int32Min,
                                                            hasProbable,
                                                            int32Probable,
                                                            hasMax,
                                                            int32Max);
                    break;
                case ((Int32)FactorId.SwedishOccurrence):
                    if (speciesFact.Field1.HasValue)
                    {
                        RedListCalculator.SetSwedishOccurrence(speciesFact.Field1.GetInt32());
                    }
                    else
                    {
                        RedListCalculator.SetSwedishOccurrence(0);
                    }
                    break;
                case ((Int32)FactorId.VeryRestrictedArea_D2VU):
                    hasProbable = (speciesFact.Field1.HasValue &&
                                   speciesFact.Field1.BooleanValue &&
                                   speciesFact.Field2.HasValue);
                    if (hasProbable)
                    {
                        int32Probable = speciesFact.Field2.EnumValue.KeyInt;
                    }
                    RedListCalculator.SetVeryRestrictedArea(hasMin,
                                                             int32Min,
                                                             hasProbable,
                                                             int32Probable,
                                                             hasMax,
                                                             int32Max,
                                                             speciesFact.Field4.GetStringValue());
                    break;
                default:
                       throw new ApplicationException("Not handled species fact. Factor id " + speciesFact.Id);
            }

            if (!IsInInit)
            {
                if ((oldCategory != RedListCalculator.Category) ||
                    (oldCriteria != RedListCalculator.Criteria) ||
                    (oldGrading != RedListCalculator.Grading) ||
                    (oldIsGraded != RedListCalculator.IsGraded) ||
                    (oldCriteriaDocumentation != RedListCalculator.CriteriaDocumentation) ||
                    (oldCategoryBestCase != RedListCalculator.CategoryBestCaseNoGrading) ||
                    (oldCategoryProbable != RedListCalculator.CategoryProbableNoGrading) ||
                    (oldCategoryWorstCase != RedListCalculator.CategoryWorstCaseNoGrading) ||
                    (oldCriteriaBestCase != RedListCalculator.CriteriaBestCase) ||
                    (oldCriteriaProbable != RedListCalculator.CriteriaProbable) ||
                    (oldCriteriaWorstCase != RedListCalculator.CriteriaWorstCase))
                {
                    SetReadListValues();
                }
            }
        }
    }
}

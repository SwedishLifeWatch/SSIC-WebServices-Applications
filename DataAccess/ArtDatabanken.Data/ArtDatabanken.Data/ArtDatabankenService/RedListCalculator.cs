using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class calculates red list category and red 
    /// list criteria based on values for red list data.
    /// </summary>
    [Serializable]
    public class RedListCalculator
    {
        private const Double MAX_PROPORTION_LOCAL_POPULATION_MIN = 0;
        private const Double MAX_PROPORTION_LOCAL_POPULATION_MAX = 100;
        private const Double POPULATION_REDUCTION_MIN = Double.MinValue;
        private const Double POPULATION_REDUCTION_MAX = 100;
        private const Int32 EXTREME_FLUCTUATIONS_MIN = 0;
        private const Int32 EXTREME_FLUCTUATIONS_MAX = 2;
        private const Int32 PROBABILITY_OF_EXTINCTION_MIN = 0;
        private const Int32 PROBABILITY_OF_EXTINCTION_MAX = 4;
        private const Int32 SEVERLY_FRAGMENTED_MIN = 0;
        private const Int32 SEVERLY_FRAGMENTED_MAX = 2;
        private const Int32 SMALL_POPULATION_CONTINUING_DECLINE_MIN = -1;
        private const Int32 SMALL_POPULATION_CONTINUING_DECLINE_MAX = 5;
        private const Int32 SWEDISH_OCCURRENCE_PROBABLY_REGIONAL_EXTINCT = 7;
        private const Int32 SWEDISH_OCCURRENCE_REGIONAL_EXTINCT = 8;
        private const Int32 VERY_RESTRICTED_AREA_MIN = 0;
        private const Int32 VERY_RESTRICTED_AREA_MAX = 2;
        private const Int64 MAX_SIZE_LOCAL_POPULATION_MIN = 0;
        private const Int64 NUMBER_OF_LOCATIONS_MIN = 0;
        private const Int64 POPULATION_SIZE_MIN = 0;
        /// <summary>Smallest possible value for generation length.</summary>
        protected const Double GENERATION_LENGTH_MIN = 0;
        /// <summary>Largest possible value for generation length.</summary>
        protected const Double GENERATION_LENGTH_MAX = Double.MaxValue;

        private Boolean _isConservationDependent;
        private Boolean _isEvaluationStatusSet;
        private Int32 _grading;
        private Int32 _swedishOccurrence;
        private String _areaOfOccupancyClarification;
        private String _areaOfOccupancyUnitLabel;
        private String _continuingDeclineClarification;
        private String _extentOfOccurrenceClarification;
        private String _extentOfOccurrenceUnitLabel;
        private String _gradingClarification;
        private String _isConservationDependentClarification;
        private String _isCriteriaA1AFulfilledClarification;
        private String _isCriteriaA1BFulfilledClarification;
        private String _isCriteriaA1CFulfilledClarification;
        private String _isCriteriaA1DFulfilledClarification;
        private String _isCriteriaA1EFulfilledClarification;
        private String _isCriteriaA2AFulfilledClarification;
        private String _isCriteriaA2BFulfilledClarification;
        private String _isCriteriaA2CFulfilledClarification;
        private String _isCriteriaA2DFulfilledClarification;
        private String _isCriteriaA2EFulfilledClarification;
        private String _isCriteriaA3BFulfilledClarification;
        private String _isCriteriaA3CFulfilledClarification;
        private String _isCriteriaA3DFulfilledClarification;
        private String _isCriteriaA3EFulfilledClarification;
        private String _isCriteriaA4AFulfilledClarification;
        private String _isCriteriaA4BFulfilledClarification;
        private String _isCriteriaA4CFulfilledClarification;
        private String _isCriteriaA4DFulfilledClarification;
        private String _isCriteriaA4EFulfilledClarification;
        private String _isCriteriaBB1FulfilledClarification;
        private String _isCriteriaBB2FulfilledClarification;
        private String _isCriteriaBB3FulfilledClarification;
        private String _isCriteriaBB4FulfilledClarification;
        private String _isCriteriaBB5FulfilledClarification;
        private String _isCriteriaBC1FulfilledClarification;
        private String _isCriteriaBC2FulfilledClarification;
        private String _isCriteriaBC3FulfilledClarification;
        private String _isCriteriaBC4FulfilledClarification;
        private String _numberOfLocationsClarification;
        private String _populationReductionA1Clarification;
        private String _populationReductionA2Clarification;
        private String _populationReductionA3Clarification;
        private String _populationReductionA4Clarification;
        private String _populationSizeClarification;
        private String _probabilityOfExtinctionClarification;
        private String _taxonTypeNameDefinite;
        private String _taxonTypeNameIndefinite;
        private String _veryRestrictedAreaClarification;
        private RedListCalculation _calculationBestCase;
        private RedListCalculation _calculationProbable;
        private RedListCalculation _calculationWorstCase;
        private RedListCategory _redlistEvaluationStatus;

        /// <summary>
        /// Creates a RedListCalculator instance.
        /// </summary>
        /// <param name="taxonTypeNameIndefinite">Taxon type name on indefinite form.</param>
        /// <param name="taxonTypeNameDefinite">Taxon type name on definite form.</param>
        public RedListCalculator(String taxonTypeNameIndefinite,
                                 String taxonTypeNameDefinite)
        {
            _areaOfOccupancyClarification = null;
            _areaOfOccupancyUnitLabel = null;
            _calculationBestCase = new RedListCalculation();
            _calculationProbable = new RedListCalculation();
            _calculationWorstCase = new RedListCalculation();
            _continuingDeclineClarification = null;
            _extentOfOccurrenceClarification = null;
            _extentOfOccurrenceUnitLabel = null;
            _grading = Int32.MinValue;
            _gradingClarification = null;
            _isConservationDependent = false;
            _isConservationDependentClarification = null;
            _isCriteriaA1AFulfilledClarification = null;
            _isCriteriaA1BFulfilledClarification = null;
            _isCriteriaA1CFulfilledClarification = null;
            _isCriteriaA1DFulfilledClarification = null;
            _isCriteriaA1EFulfilledClarification = null;
            _isCriteriaA2AFulfilledClarification = null;
            _isCriteriaA2BFulfilledClarification = null;
            _isCriteriaA2CFulfilledClarification = null;
            _isCriteriaA2DFulfilledClarification = null;
            _isCriteriaA2EFulfilledClarification = null;
            _isCriteriaA3BFulfilledClarification = null;
            _isCriteriaA3CFulfilledClarification = null;
            _isCriteriaA3DFulfilledClarification = null;
            _isCriteriaA3EFulfilledClarification = null;
            _isCriteriaA4AFulfilledClarification = null;
            _isCriteriaA4BFulfilledClarification = null;
            _isCriteriaA4CFulfilledClarification = null;
            _isCriteriaA4DFulfilledClarification = null;
            _isCriteriaA4EFulfilledClarification = null;
            _isCriteriaBB1FulfilledClarification = null;
            _isCriteriaBB2FulfilledClarification = null;
            _isCriteriaBB3FulfilledClarification = null;
            _isCriteriaBB4FulfilledClarification = null;
            _isCriteriaBB5FulfilledClarification = null;
            _isCriteriaBC1FulfilledClarification = null;
            _isCriteriaBC2FulfilledClarification = null;
            _isCriteriaBC3FulfilledClarification = null;
            _isCriteriaBC4FulfilledClarification = null;
            _isEvaluationStatusSet = false;
            _numberOfLocationsClarification = null;
            _populationReductionA1Clarification = null;
            _populationReductionA2Clarification = null;
            _populationReductionA3Clarification = null;
            _populationReductionA4Clarification = null;
            _populationSizeClarification = null;
            _probabilityOfExtinctionClarification = null;
            _redlistEvaluationStatus = RedListCategory.LC;
            _swedishOccurrence = Int32.MinValue;
            if (taxonTypeNameDefinite.IsEmpty())
            {
                _taxonTypeNameDefinite = "arten";
            }
            else
            {
                _taxonTypeNameDefinite = taxonTypeNameDefinite.ToLower();
            }
            if (taxonTypeNameIndefinite.IsEmpty())
            {
                _taxonTypeNameIndefinite = "art";
            }
            else
            {
                _taxonTypeNameIndefinite = taxonTypeNameIndefinite.ToLower();
            }
            _veryRestrictedAreaClarification = null;
        }

        /// <summary>
        /// Get area of occupancy clarification.
        /// </summary>
        public String AreaOfOccupancyClarification
        {
            get
            {
                return _areaOfOccupancyClarification;
            }
        }

        /// <summary>
        /// Get area of occupancy unit label.
        /// </summary>
        public String AreaOfOccupancyUnitLabel
        {
            get
            {
                return _areaOfOccupancyUnitLabel;
            }
        }

        /// <summary>
        /// Get red list calculation best case.
        /// </summary>
        protected RedListCalculation CalculationBestCase
        {
            get { return _calculationBestCase; }
        }

        /// <summary>
        /// Get red list calculation probable.
        /// </summary>
        protected RedListCalculation CalculationProbable
        {
            get { return _calculationProbable; }
        }

        /// <summary>
        /// Get red list calculation worst case.
        /// </summary>
        protected RedListCalculation CalculationWorstCase
        {
            get { return _calculationWorstCase; }
        }

        /// <summary>
        /// Handle red list category.
        /// </summary>
        public RedListCategory Category
        {
            get
            {
                RedListCategory category;

                if (IsEvaluationStatusSet)
                {
                    if (_redlistEvaluationStatus != RedListCategory.LC)
                    {
                        return _redlistEvaluationStatus;
                    }
                    else if (IsRegionalExtinct)
                    {
                        return RedListCategory.RE;
                    }
                    else if ((_calculationBestCase.Category == RedListCategory.LC) &&
                             ((_calculationWorstCase.Category == RedListCategory.CR) ||
                              (_calculationWorstCase.Category == RedListCategory.RE)))
                    {
                        return RedListCategory.DD;
                    }
                    else
                    {
                        category = GetGradingAdjustedCategory(_calculationProbable.Category);
                        if ((category == RedListCategory.LC) &&
                            IsConservationDependent)
                        {
                            return category = RedListCategory.NT;
                        }
                        return category;
                    }
                }
                else
                {
                    return RedListCategory.NA;
                }
            }
        }

        /// <summary>
        /// Get best case red list category.
        /// </summary>
        public RedListCategory CategoryBestCaseGraded
        {
            get { return GetGradingAdjustedCategory(_calculationBestCase.Category); }
        }

        /// <summary>
        /// Get best case red list category without grading added.
        /// </summary>
        public RedListCategory CategoryBestCaseNoGrading
        {
            get { return _calculationBestCase.Category; }
        }

        /// <summary>
        /// Get probable red list category.
        /// </summary>
        public RedListCategory CategoryProbableGraded
        {
            get { return GetGradingAdjustedCategory(_calculationProbable.Category); }
        }

        /// <summary>
        /// Get probable red list category.
        /// </summary>
        public RedListCategory CategoryProbableNoGrading
        {
            get { return _calculationProbable.Category; }
        }

        /// <summary>
        /// Get worst case red list category.
        /// </summary>
        public RedListCategory CategoryWorstCaseGraded
        {
            get { return GetGradingAdjustedCategory(_calculationWorstCase.Category); }
        }

        /// <summary>
        /// Get worst case red list category without grading added.
        /// </summary>
        public RedListCategory CategoryWorstCaseNoGrading
        {
            get { return _calculationWorstCase.Category; }
        }

        /// <summary>
        /// Get continuing decline clarification.
        /// </summary>
        public String ContinuingDeclineClarification
        {
            get
            {
                return _continuingDeclineClarification;
            }
        }

        /// <summary>
        /// Get red list criteria.
        /// </summary>
        public String Criteria
        {
            get
            {
                if (IsEvaluationStatusSet)
                {
                    switch (Category)
                    {
                        case RedListCategory.CR:
                        case RedListCategory.EN:
                        case RedListCategory.VU:
                        case RedListCategory.NT:
                            return _calculationProbable.Criteria;
                        default:
                            return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Get best case red list criteria.
        /// </summary>
        public String CriteriaBestCase
        {
            get
            {
                switch (CategoryBestCaseNoGrading)
                {
                    case RedListCategory.CR:
                    case RedListCategory.EN:
                    case RedListCategory.VU:
                    case RedListCategory.NT:
                        return _calculationBestCase.Criteria;
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// Get probable red list criteria.
        /// </summary>
        public String CriteriaProbable
        {
            get
            {
                switch (CategoryProbableNoGrading)
                {
                    case RedListCategory.CR:
                    case RedListCategory.EN:
                    case RedListCategory.VU:
                    case RedListCategory.NT:
                        return _calculationProbable.Criteria;
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// Get worst case red list criteria.
        /// </summary>
        public String CriteriaWorstCase
        {
            get
            {
                switch (CategoryWorstCaseNoGrading)
                {
                    case RedListCategory.CR:
                    case RedListCategory.EN:
                    case RedListCategory.VU:
                    case RedListCategory.NT:
                        return _calculationWorstCase.Criteria;
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// Get red list criteria documentation.
        /// </summary>
        public String CriteriaDocumentation
        {
            get
            {
                return GetCriteriaDocumentation();
            }
        }

        /// <summary>
        /// Get extent of occurrence clarification.
        /// </summary>
        public String ExtentOfOccurrenceClarification
        {
            get
            {
                return _extentOfOccurrenceClarification;
            }
        }

        /// <summary>
        /// Get extent of occurrence unit label.
        /// </summary>
        public String ExtentOfOccurrenceUnitLabel
        {
            get
            {
                return _extentOfOccurrenceUnitLabel;
            }
        }

        /// <summary>
        /// Amount of grading for this taxon.
        /// Down grading (negativ value) means that the taxons
        /// red list category changes to less threatened.
        /// Up grading means more threathened.
        /// Factor: RedlistCategory, Id = 741, value "Antal steg".
        /// </summary>
        public Int32 Grading
        {
            get { return _grading; }
        }

        /// <summary>
        /// Get grading clarification.
        /// </summary>
        public String GradingClarification
        {
            get
            {
                return _gradingClarification;
            }
        }

        /// <summary>
        /// Test if grading has been set.
        /// </summary>
        public Boolean HasGrading
        {
            get { return _grading != Int32.MinValue; }
            set
            {
                if (!value)
                {
                    _grading = Int32.MinValue;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException("Use method SetGrading if you want to set a value.");
                }
            }
        }

        /// <summary>
        /// Test if category has been set from outside this
        /// instance of RedListCalculator.
        /// </summary>
        protected Boolean IsCategorySet
        {
            get
            {
                return ((_redlistEvaluationStatus != RedListCategory.LC) ||
                        IsRegionalExtinct);
            }
        }

        /// <summary>
        /// Test if taxon is conservation dependent.
        /// Factor: ConservationDependent, Id = 1943,
        /// </summary>
        public Boolean IsConservationDependent
        {
            get { return _isConservationDependent; }
            set
            {
                _isConservationDependent = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Get is conservation dependent clarification.
        /// </summary>
        public String IsConservationDependentClarification
        {
            get
            {
                return _isConservationDependentClarification;
            }
        }

        /// <summary>
        /// Get is criteria A1A fulfilled clarification.
        /// </summary>
        public String IsCriteriaA1AFulfilledClarification
        {
            get
            {
                return _isCriteriaA1AFulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria A1B fulfilled clarification.
        /// </summary>
        public String IsCriteriaA1BFulfilledClarification
        {
            get
            {
                return _isCriteriaA1BFulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria A1C fulfilled clarification.
        /// </summary>
        public String IsCriteriaA1CFulfilledClarification
        {
            get
            {
                return _isCriteriaA1CFulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria A1D fulfilled clarification.
        /// </summary>
        public String IsCriteriaA1DFulfilledClarification
        {
            get
            {
                return _isCriteriaA1DFulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria A1E fulfilled clarification.
        /// </summary>
        public String IsCriteriaA1EFulfilledClarification
        {
            get
            {
                return _isCriteriaA1EFulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria A2A fulfilled clarification.
        /// </summary>
        public String IsCriteriaA2AFulfilledClarification
        {
            get
            {
                return _isCriteriaA2AFulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria A2B fulfilled clarification.
        /// </summary>
        public String IsCriteriaA2BFulfilledClarification
        {
            get
            {
                return _isCriteriaA2BFulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria A2C fulfilled clarification.
        /// </summary>
        public String IsCriteriaA2CFulfilledClarification
        {
            get
            {
                return _isCriteriaA2CFulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria A2D fulfilled clarification.
        /// </summary>
        public String IsCriteriaA2DFulfilledClarification
        {
            get
            {
                return _isCriteriaA2DFulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria A2E fulfilled clarification.
        /// </summary>
        public String IsCriteriaA2EFulfilledClarification
        {
            get
            {
                return _isCriteriaA2EFulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria A3B fulfilled clarification.
        /// </summary>
        public String IsCriteriaA3BFulfilledClarification
        {
            get
            {
                return _isCriteriaA3BFulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria A3C fulfilled clarification.
        /// </summary>
        public String IsCriteriaA3CFulfilledClarification
        {
            get
            {
                return _isCriteriaA3CFulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria A3D fulfilled clarification.
        /// </summary>
        public String IsCriteriaA3DFulfilledClarification
        {
            get
            {
                return _isCriteriaA3DFulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria A3E fulfilled clarification.
        /// </summary>
        public String IsCriteriaA3EFulfilledClarification
        {
            get
            {
                return _isCriteriaA3EFulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria A4A fulfilled clarification.
        /// </summary>
        public String IsCriteriaA4AFulfilledClarification
        {
            get
            {
                return _isCriteriaA4AFulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria A4B fulfilled clarification.
        /// </summary>
        public String IsCriteriaA4BFulfilledClarification
        {
            get
            {
                return _isCriteriaA4BFulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria A4C fulfilled clarification.
        /// </summary>
        public String IsCriteriaA4CFulfilledClarification
        {
            get
            {
                return _isCriteriaA4CFulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria A4D fulfilled clarification.
        /// </summary>
        public String IsCriteriaA4DFulfilledClarification
        {
            get
            {
                return _isCriteriaA4DFulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria A4E fulfilled clarification.
        /// </summary>
        public String IsCriteriaA4EFulfilledClarification
        {
            get
            {
                return _isCriteriaA4EFulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria BB1 fulfilled clarification.
        /// </summary>
        public String IsCriteriaBB1FulfilledClarification
        {
            get
            {
                return _isCriteriaBB1FulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria BB2 fulfilled clarification.
        /// </summary>
        public String IsCriteriaBB2FulfilledClarification
        {
            get
            {
                return _isCriteriaBB2FulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria BB3 fulfilled clarification.
        /// </summary>
        public String IsCriteriaBB3FulfilledClarification
        {
            get
            {
                return _isCriteriaBB3FulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria BB4 fulfilled clarification.
        /// </summary>
        public String IsCriteriaBB4FulfilledClarification
        {
            get
            {
                return _isCriteriaBB4FulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria BB5 fulfilled clarification.
        /// </summary>
        public String IsCriteriaBB5FulfilledClarification
        {
            get
            {
                return _isCriteriaBB5FulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria BC1 fulfilled clarification.
        /// </summary>
        public String IsCriteriaBC1FulfilledClarification
        {
            get
            {
                return _isCriteriaBC1FulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria BC2 fulfilled clarification.
        /// </summary>
        public String IsCriteriaBC2FulfilledClarification
        {
            get
            {
                return _isCriteriaBC2FulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria BC3 fulfilled clarification.
        /// </summary>
        public String IsCriteriaBC3FulfilledClarification
        {
            get
            {
                return _isCriteriaBC3FulfilledClarification;
            }
        }

        /// <summary>
        /// Get is criteria BC4 fulfilled clarification.
        /// </summary>
        public String IsCriteriaBC4FulfilledClarification
        {
            get
            {
                return _isCriteriaBC4FulfilledClarification;
            }
        }

        /// <summary>
        /// Test if evaluation status has been set.
        /// No calculation should be done if evaluation 
        /// status has not been set.
        /// </summary>
        public Boolean IsEvaluationStatusSet
        {
            get
            {
                return _isEvaluationStatusSet;
            }
            set
            {
                if (_isEvaluationStatusSet != value)
                {
                    _isEvaluationStatusSet = value;
                    _calculationBestCase.IsEvaluationStatusSet = value;
                    _calculationProbable.IsEvaluationStatusSet = value;
                    _calculationWorstCase.IsEvaluationStatusSet = value;
                    SetRedListValues();
                }
            }
        }

        /// <summary>
        /// Get population size clarification.
        /// </summary>
        public String PopulationSizeClarification
        {
            get
            {
                return _populationSizeClarification;
            }
        }

        /// <summary>
        /// Get number of locations clarification.
        /// </summary>
        public String NumberOfLocationsClarification
        {
            get
            {
                return _numberOfLocationsClarification;
            }
        }

        /// <summary>
        /// Get population reduction A1 clarification.
        /// </summary>
        public String PopulationReductionA1Clarification
        {
            get
            {
                return _populationReductionA1Clarification;
            }
        }

        /// <summary>
        /// Get population reduction A2 clarification.
        /// </summary>
        public String PopulationReductionA2Clarification
        {
            get
            {
                return _populationReductionA2Clarification;
            }
        }

        /// <summary>
        /// Get population reduction A3 clarification.
        /// </summary>
        public String PopulationReductionA3Clarification
        {
            get
            {
                return _populationReductionA3Clarification;
            }
        }

        /// <summary>
        /// Get population reduction A4 clarification.
        /// </summary>
        public String PopulationReductionA4Clarification
        {
            get
            {
                return _populationReductionA4Clarification;
            }
        }

        /// <summary>
        /// Get probability of extinction clarification.
        /// </summary>
        public String ProbabilityOfExtinctionClarification
        {
            get
            {
                return _probabilityOfExtinctionClarification;
            }
        }

        /// <summary>
        ///  Get taxon type name on definite form.
        /// </summary>
        public String TaxonTypeNameDefinite
        {
            get
            {
                return _taxonTypeNameDefinite;
            }
        }

        /// <summary>
        /// Get taxon type name on indefinite form
        /// </summary>
        public String TaxonTypeNameIndefinite
        {
            get
            {
                return _taxonTypeNameIndefinite;
            }
        }

        /// <summary>
        /// Get very restricted area clarification.
        /// </summary>
        public String VeryRestrictedAreaClarification
        {
            get
            {
                return _veryRestrictedAreaClarification;
            }
        }

        /// <summary>
        /// Set grading value in SpeciesFact.
        /// </summary>
        /// <param name="hasGrading">Indicates if grading has a value.</param>
        /// <param name="grading">Value for grading.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetGrading(Boolean hasGrading,
                               Int32 grading,
                               String clarification)
        {
            _gradingClarification = clarification;
            if (hasGrading)
            {
                _grading = grading;
            }
            else
            {
                HasGrading = false;
            }
            SetRedListValues();
        }

        /// <summary>
        /// Set if taxon is conservation dependent.
        /// Factor: ConservationDependent, Id = 1943,
        /// </summary>
        /// <param name="isConservationDependent">Indicates if is conservation dependent is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsConservationDependent(Boolean isConservationDependent,
                                               String clarification)
        {
            _isConservationDependent = isConservationDependent;
            _isConservationDependentClarification = clarification;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria A1A is Fulfilled.
        /// Reduction in population. Direct observation.
        /// Factor: ReductionBasedOn_A1a, Id = 686,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaA1AFulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaA1AFulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaA1AFulfilled = fulfilled;
            _calculationProbable.IsCriteriaA1AFulfilled = fulfilled;
            _calculationBestCase.IsCriteriaA1AFulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria A1B is Fulfilled.
        /// Reduction in population. Index of abundance.
        /// Factor: ReductionBasedOn_A1b, Id = 687,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaA1BFulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaA1BFulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaA1BFulfilled = fulfilled;
            _calculationProbable.IsCriteriaA1BFulfilled = fulfilled;
            _calculationBestCase.IsCriteriaA1BFulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria A1C is Fulfilled.
        /// Reduction in population. Decline in geography.
        /// Factor: ReductionBasedOn_A1c, Id = 688,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaA1CFulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaA1CFulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaA1CFulfilled = fulfilled;
            _calculationProbable.IsCriteriaA1CFulfilled = fulfilled;
            _calculationBestCase.IsCriteriaA1CFulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria A1D is Fulfilled.
        /// Reduction in population. Levels of exploitation.
        /// Factor: ReductionBasedOn_A1d, Id = 689,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaA1DFulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaA1DFulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaA1DFulfilled = fulfilled;
            _calculationProbable.IsCriteriaA1DFulfilled = fulfilled;
            _calculationBestCase.IsCriteriaA1DFulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria A1E is Fulfilled.
        /// Reduction in population. Effects of other taxa.
        /// Factor: ReductionBasedOn_A1e, Id = 690,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaA1EFulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaA1EFulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaA1EFulfilled = fulfilled;
            _calculationProbable.IsCriteriaA1EFulfilled = fulfilled;
            _calculationBestCase.IsCriteriaA1EFulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria A2A is Fulfilled.
        /// Reduction in population. Direct observation.
        /// Factor: ReductionBasedOn_A2a, Id = 694,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaA2AFulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaA2AFulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaA2AFulfilled = fulfilled;
            _calculationProbable.IsCriteriaA2AFulfilled = fulfilled;
            _calculationBestCase.IsCriteriaA2AFulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria A2B is Fulfilled.
        /// Reduction in population. Index of abundance.
        /// Factor: ReductionBasedOn_A2b, Id = 695,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaA2BFulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaA2BFulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaA2BFulfilled = fulfilled;
            _calculationProbable.IsCriteriaA2BFulfilled = fulfilled;
            _calculationBestCase.IsCriteriaA2BFulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria A2C is Fulfilled.
        /// Reduction in population. Decline in geography.
        /// Factor: ReductionBasedOn_A2c, Id = 696,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaA2CFulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaA2CFulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaA2CFulfilled = fulfilled;
            _calculationProbable.IsCriteriaA2CFulfilled = fulfilled;
            _calculationBestCase.IsCriteriaA2CFulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria A2D is Fulfilled.
        /// Reduction in population. Levels of exploitation.
        /// Factor: ReductionBasedOn_A2d, Id = 697,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaA2DFulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaA2DFulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaA2DFulfilled = fulfilled;
            _calculationProbable.IsCriteriaA2DFulfilled = fulfilled;
            _calculationBestCase.IsCriteriaA2DFulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria A2E is Fulfilled.
        /// Reduction in population. Effects of other taxa.
        /// Factor: ReductionBasedOn_A2e, Id = 698,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaA2EFulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaA2EFulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaA2EFulfilled = fulfilled;
            _calculationProbable.IsCriteriaA2EFulfilled = fulfilled;
            _calculationBestCase.IsCriteriaA2EFulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria A2B is Fulfilled.
        /// Reduction in population. Index of abundance.
        /// Factor: ReductionBasedOn_A3b, Id = 702,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaA3BFulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaA3BFulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaA3BFulfilled = fulfilled;
            _calculationProbable.IsCriteriaA3BFulfilled = fulfilled;
            _calculationBestCase.IsCriteriaA3BFulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria A2C is Fulfilled.
        /// Reduction in population. Decline in geography.
        /// Factor: ReductionBasedOn_A3c, Id = 703,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaA3CFulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaA3CFulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaA3CFulfilled = fulfilled;
            _calculationProbable.IsCriteriaA3CFulfilled = fulfilled;
            _calculationBestCase.IsCriteriaA3CFulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria A3D is Fulfilled.
        /// Reduction in population. Levels of exploitation.
        /// Factor: ReductionBasedOn_A3d, Id = 704,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaA3DFulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaA3DFulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaA3DFulfilled = fulfilled;
            _calculationProbable.IsCriteriaA3DFulfilled = fulfilled;
            _calculationBestCase.IsCriteriaA3DFulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria A3E is Fulfilled.
        /// Reduction in population. Effects of other taxa.
        /// Factor: ReductionBasedOn_A3e, Id = 705,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaA3EFulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaA3EFulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaA3EFulfilled = fulfilled;
            _calculationProbable.IsCriteriaA3EFulfilled = fulfilled;
            _calculationBestCase.IsCriteriaA3EFulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria A4A is Fulfilled.
        /// Reduction in population. Direct observation.
        /// Factor: ReductionBasedOn_A4a, Id = 709,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaA4AFulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaA4AFulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaA4AFulfilled = fulfilled;
            _calculationProbable.IsCriteriaA4AFulfilled = fulfilled;
            _calculationBestCase.IsCriteriaA4AFulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria A4B is Fulfilled.
        /// Reduction in population. Index of abundance.
        /// Factor: ReductionBasedOn_A4b, Id = 710,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaA4BFulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaA4BFulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaA4BFulfilled = fulfilled;
            _calculationProbable.IsCriteriaA4BFulfilled = fulfilled;
            _calculationBestCase.IsCriteriaA4BFulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria A4C is Fulfilled.
        /// Reduction in population. Decline in geography.
        /// Factor: ReductionBasedOn_A4c, Id = 711,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaA4CFulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaA4CFulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaA4CFulfilled = fulfilled;
            _calculationProbable.IsCriteriaA4CFulfilled = fulfilled;
            _calculationBestCase.IsCriteriaA4CFulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria A4D is Fulfilled.
        /// Reduction in population. Levels of exploitation.
        /// Factor: ReductionBasedOn_A4d, Id = 712,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaA4DFulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaA4DFulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaA4DFulfilled = fulfilled;
            _calculationProbable.IsCriteriaA4DFulfilled = fulfilled;
            _calculationBestCase.IsCriteriaA4DFulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria A4E is Fulfilled.
        /// Reduction in population. Effects of other taxa.
        /// Factor: ReductionBasedOn_A4e, Id = 713,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaA4EFulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaA4EFulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaA4EFulfilled = fulfilled;
            _calculationProbable.IsCriteriaA4EFulfilled = fulfilled;
            _calculationBestCase.IsCriteriaA4EFulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria BB1 is Fulfilled.
        /// Continuing decline in extent of occurence.
        /// Factor: ContinuingDeclineBasedOn_Bbi, Id = 673,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaBB1Fulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaBB1FulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaBB1Fulfilled = fulfilled;
            _calculationProbable.IsCriteriaBB1Fulfilled = fulfilled;
            _calculationBestCase.IsCriteriaBB1Fulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria BB2 is Fulfilled.
        /// Continuing decline in area of occupancy.
        /// Factor: ContinuingDeclineBasedOn_Bbii, Id = 674,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaBB2Fulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaBB2FulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaBB2Fulfilled = fulfilled;
            _calculationProbable.IsCriteriaBB2Fulfilled = fulfilled;
            _calculationBestCase.IsCriteriaBB2Fulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria BB3 is Fulfilled.
        /// Continuing decline in area, extent and/or quiality of habitat.
        /// Factor: ContinuingDeclineBasedOn_Bbiii, Id = 675,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaBB3Fulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaBB3FulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaBB3Fulfilled = fulfilled;
            _calculationProbable.IsCriteriaBB3Fulfilled = fulfilled;
            _calculationBestCase.IsCriteriaBB3Fulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria BB4 is Fulfilled.
        /// Continuing decline in number of locations or subpopulations.
        /// Factor: ContinuingDeclineBasedOn_Bbiv, Id = 676,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaBB4Fulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaBB4FulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaBB4Fulfilled = fulfilled;
            _calculationProbable.IsCriteriaBB4Fulfilled = fulfilled;
            _calculationBestCase.IsCriteriaBB4Fulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria BB5 is Fulfilled.
        /// Continuing decline in number of mature individuals.
        /// Factor: ContinuingDeclineBasedOn_Bbv, Id = 677,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaBB5Fulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaBB5FulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaBB5Fulfilled = fulfilled;
            _calculationProbable.IsCriteriaBB5Fulfilled = fulfilled;
            _calculationBestCase.IsCriteriaBB5Fulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria BC1 is Fulfilled.
        /// Extreme fluctuations in extent of occurrence.
        /// Factor: ExtremeFluctuationsIn_Bci, Id = 721,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaBC1Fulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaBC1FulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaBC1Fulfilled = fulfilled;
            _calculationProbable.IsCriteriaBC1Fulfilled = fulfilled;
            _calculationBestCase.IsCriteriaBC1Fulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria BC2 is Fulfilled.
        /// Extreme fluctuations in area of occupancy.
        /// Factor: ExtremeFluctuationsIn_Bcii, Id = 722,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaBC2Fulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaBC2FulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaBC2Fulfilled = fulfilled;
            _calculationProbable.IsCriteriaBC2Fulfilled = fulfilled;
            _calculationBestCase.IsCriteriaBC2Fulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria BC3 is Fulfilled.
        /// Extreme fluctuations in number of locations or subpopulations.
        /// Factor: ExtremeFluctuationsIn_Bciii, Id = 723,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaBC3Fulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaBC3FulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaBC3Fulfilled = fulfilled;
            _calculationProbable.IsCriteriaBC3Fulfilled = fulfilled;
            _calculationBestCase.IsCriteriaBC3Fulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Set if criteria BC4 is Fulfilled.
        /// Extreme fluctuations in number of locations or subpopulations.
        /// Factor: ExtremeFluctuationsIn_Bciv, Id = 724,
        /// </summary>
        /// <param name="fulfilled">Indicates if criteria is fulfilled.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetIsCriteriaBC4Fulfilled(Boolean fulfilled,
                                              String clarification)
        {
            _isCriteriaBC4FulfilledClarification = clarification;
            _calculationWorstCase.IsCriteriaBC4Fulfilled = fulfilled;
            _calculationProbable.IsCriteriaBC4Fulfilled = fulfilled;
            _calculationBestCase.IsCriteriaBC4Fulfilled = fulfilled;
            SetRedListValues();
        }

        /// <summary>
        /// Flag that indicates if Criteria is calculated.
        /// This flag is used to avoid unnecessary calculations.
        /// </summary>
        public Boolean IsCriteriaCalculated
        {
            set
            {
                _calculationWorstCase.IsCriteriaCalculated = value;
                _calculationProbable.IsCriteriaCalculated = value;
                _calculationBestCase.IsCriteriaCalculated = value;
            }
        }

        /// <summary>
        /// Check if taxon has been graded.
        /// Factor: RedlistCategory, Id = 743, value "Graderad".
        /// </summary>
        public Boolean IsGraded
        {
            get { return HasGrading && (Grading != 0); }
        }

        /// <summary>
        /// Test if initialization is being performed.
        /// </summary>
        protected Boolean IsInInit
        {
            get { return _calculationProbable.IsInInit; }
        }

        /// <summary>
        /// Get if taxon is probably regional extinct.
        /// This property has nothing to do with the calculated
        /// value RE which tests if population size is 0.
        /// Factor: SwedishOccurrence, Id = 1938, value "Försvunnen".
        /// </summary>
        public Boolean IsProbablyRegionalExtinct
        {
            get { return _swedishOccurrence == SWEDISH_OCCURRENCE_PROBABLY_REGIONAL_EXTINCT; }
        }

        /// <summary>
        /// Get if taxon is regional extinct.
        /// This property has nothing to do with the calculated
        /// value RE which tests if population size is 0.
        /// Factor: SwedishOccurrence, Id = 1938, value "Försvunnen".
        /// </summary>
        public Boolean IsRegionalExtinct
        {
            get { return _swedishOccurrence == SWEDISH_OCCURRENCE_REGIONAL_EXTINCT; }
        }

        /// <summary>
        /// Test if all values are missing.
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <returns> True if all values are missing. </returns>
        protected Boolean HasNoValues(Boolean hasMin,
                                      Boolean hasProbable,
                                      Boolean hasMax)
        {
            return (!hasMin && !hasProbable && !hasMax);
        }

        /// <summary>
        /// Get criteria documentation.
        /// </summary>
        protected virtual String GetCriteriaDocumentation()
        {
            // No criteria documentation is generated in the base clas.
            return null;
        }

        /// <summary>
        /// Adjust red list category value according
        /// to grading information.
        /// </summary>
        /// <param name="category">Category to adjust.</param>
        /// <returns>Category adjusted to grading.</returns>
        private RedListCategory GetGradingAdjustedCategory(RedListCategory category)
        {
            Int32 categoryId;

            categoryId = ((Int32)category);
            if (IsGraded &&
                (categoryId <= ((Int32)RedListCategory.LC)) &&
                (categoryId >= ((Int32)RedListCategory.CR)))
            {
                categoryId = ((Int32)category) - Grading;
                if (categoryId > ((Int32)RedListCategory.LC))
                {
                    categoryId = ((Int32)RedListCategory.LC);
                }
                if (categoryId < ((Int32)RedListCategory.CR))
                {
                    categoryId = ((Int32)RedListCategory.CR);
                }
                category = (RedListCategory)(Enum.Parse(typeof(RedListCategory), categoryId.ToString()));
            }
            return category;
        }

        /// <summary>
        /// Test if two values are missing.
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <returns> True if two values are missing. </returns>
        private Boolean HasOneValue(Boolean hasMin,
                                    Boolean hasProbable,
                                    Boolean hasMax)
        {
            return ((!hasMin && !hasProbable && hasMax) ||
                    (!hasMin && hasProbable && !hasMax) ||
                    (hasMin && !hasProbable && !hasMax));
        }

        /// <summary>
        /// Test if one value is missing.
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <returns> True if one value is missing. </returns>
        private Boolean HasTwoValues(Boolean hasMin,
                                     Boolean hasProbable,
                                     Boolean hasMax)
        {
            return ((!hasMin && hasProbable && hasMax) ||
                    (hasMin && !hasProbable && hasMax) ||
                    (hasMin && hasProbable && !hasMax));
        }


        /// <summary>
        /// Call this method before first initialization
        /// of red list values.
        /// RedListValues are not updated until initialization
        /// has finished with a call to method InitEnd.
        /// </summary>
        public void InitBegin()
        {
            _calculationBestCase.InitBegin();
            _calculationProbable.InitBegin();
            _calculationWorstCase.InitBegin();
        }

        /// <summary>
        /// Call this method after initialization
        /// of red list values.
        /// Calculation is turned on and performed in this call.
        /// </summary>
        public void InitEnd()
        {
            _calculationBestCase.InitEnd();
            _calculationProbable.InitEnd();
            _calculationWorstCase.InitEnd();
            SetRedListValues();
        }

        /// <summary>
        /// Set area of occupancy.
        /// Unit is square km.
        /// Factor: AreaOfOccupancy_B2Estimated, Id = 734.
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="areaOfOccupancyMin">Min value for area of occupancy.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="areaOfOccupancyProbable">Probable value for area of occupancy.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="areaOfOccupancyMax">Max value for area of occupancy.</param>
        /// <param name="clarification">Clarification about values.</param>
        /// <param name="unitLabel">Label for unit of values.</param>
        public void SetAreaOfOccupancy(Boolean hasMin,
                                       Double areaOfOccupancyMin, 
                                       Boolean hasProbable,
                                       Double areaOfOccupancyProbable,
                                       Boolean hasMax,
                                       Double areaOfOccupancyMax,
                                       String clarification,
                                       String unitLabel)
        {
            _areaOfOccupancyClarification = clarification;
            _areaOfOccupancyUnitLabel = unitLabel;

            if (HasNoValues(hasMin, hasProbable, hasMax))
            {
                // No values to set.
                _calculationWorstCase.HasAreaOfOccupancy = false;
                _calculationProbable.HasAreaOfOccupancy = false;
                _calculationBestCase.HasAreaOfOccupancy = false;
            }
            else
            {
                // Fill in missing values.
                SetValues(hasMin,
                          ref areaOfOccupancyMin,
                          hasProbable,
                          ref areaOfOccupancyProbable,
                          hasMax,
                          ref areaOfOccupancyMax,
                          RedListCalculation.AREA_OF_OCCUPANCY_MIN,
                          RedListCalculation.AREA_OF_OCCUPANCY_MAX);

                // Set values in RedListCalculation.
                _calculationWorstCase.AreaOfOccupancy = areaOfOccupancyMin;
                _calculationProbable.AreaOfOccupancy = areaOfOccupancyProbable;
                _calculationBestCase.AreaOfOccupancy = areaOfOccupancyMax;
            }
            SetRedListValues();
        }

        /// <summary>
        /// Handle extent of occurrence.
        /// Unit is square km.
        /// Factor: ExtentOfOccurrence_B1Estimated, Id = 731.
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="extentOfOccurrenceMin">Min value for extent of occurrence.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="extentOfOccurrenceProbable">Probable value for extent of occurrence.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="extentOfOccurrenceMax">Max value for extent of occurrence.</param>
        /// <param name="clarification">Clarification about values.</param>
        /// <param name="unitLabel">Label for unit of values.</param>
        public void SetExtentOfOccurrence(Boolean hasMin,
                                          Double extentOfOccurrenceMin, 
                                          Boolean hasProbable,
                                          Double extentOfOccurrenceProbable,
                                          Boolean hasMax,
                                          Double extentOfOccurrenceMax,
                                          String clarification,
                                          String unitLabel)
        {
            _extentOfOccurrenceClarification = clarification;
            _extentOfOccurrenceUnitLabel = unitLabel;

            if (HasNoValues(hasMin, hasProbable, hasMax))
            {
                // No values to set.
                _calculationWorstCase.HasExtentOfOccurrence = false;
                _calculationProbable.HasExtentOfOccurrence = false;
                _calculationBestCase.HasExtentOfOccurrence = false;
            }
            else
            {
                // Fill in missing values.
                SetValues(hasMin,
                          ref extentOfOccurrenceMin,
                          hasProbable,
                          ref extentOfOccurrenceProbable,
                          hasMax,
                          ref extentOfOccurrenceMax,
                          RedListCalculation.EXTENT_OF_OCCURRENCE_MIN,
                          RedListCalculation.EXTENT_OF_OCCURRENCE_MAX);

                // Set values in RedListCalculation.
                _calculationWorstCase.ExtentOfOccurrence = extentOfOccurrenceMin;
                _calculationProbable.ExtentOfOccurrence = extentOfOccurrenceProbable;
                _calculationBestCase.ExtentOfOccurrence = extentOfOccurrenceMax;
            }
            SetRedListValues();
        }

        /// <summary>
        /// Handle extreme fluctuations.
        /// Unit:
        /// 0 = No extreme fluctuations.
        /// 1 = Probably extreme fluctuations.
        /// 2 = Extreme fluctuations.
        /// Factor: ExtremeFluctuations, Id = 718
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="extremeFluctuationsMin">Min value for extreme fluctuations.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="extremeFluctuationsProbable">Probable value for extreme fluctuations.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="extremeFluctuationsMax">Max value for extreme fluctuations.</param>
        public void SetExtremeFluctuations(Boolean hasMin,
                                           Int32 extremeFluctuationsMin,
                                           Boolean hasProbable,
                                           Int32 extremeFluctuationsProbable,
                                           Boolean hasMax,
                                           Int32 extremeFluctuationsMax)
        {

            if (HasNoValues(hasMin, hasProbable, hasMax))
            {
                _calculationWorstCase.HasExtremeFluctuations = false;
                _calculationProbable.HasExtremeFluctuations = false;
                _calculationBestCase.HasExtremeFluctuations = false;
                // No values to set.
            }
            else
            {
                // Fill in missing values.
                SetValues(hasMin,
                          ref extremeFluctuationsMin,
                          hasProbable,
                          ref extremeFluctuationsProbable,
                          hasMax,
                          ref extremeFluctuationsMax,
                          EXTREME_FLUCTUATIONS_MIN,
                          EXTREME_FLUCTUATIONS_MAX);

                // Set values in RedListCalculation.
                _calculationWorstCase.ExtremeFluctuations = extremeFluctuationsMax;
                _calculationProbable.ExtremeFluctuations = extremeFluctuationsProbable;
                _calculationBestCase.ExtremeFluctuations = extremeFluctuationsMin;
            }
            SetRedListValues();
        }

        /// <summary>
        /// Handle status of taxon in sweden
        /// This property has nothing to do with the calculated
        /// value RE which tests if population size is 0.
        /// Factor: SwedishOccurrence, Id = 1938,
        /// Unit:
        /// 7 = Probably regional extinct.
        /// 8 = Regional extinct.
        /// </summary>
        /// <param name="swedishOccurrence">Status of taxon in sweden.</param>
        public void SetSwedishOccurrence(Int32 swedishOccurrence)
        {
            _swedishOccurrence = swedishOccurrence;
            SetRedListValues();
        }

        /// <summary>
        /// Handle max proportion local population.
        /// Unit is max number of per cent in one subpopulation
        /// of the total population, e.g. 90% individuals
        /// in one subpopulation has the value 90.0
        /// Factor: MaxProportionLocalPopulation, Id = 717
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="maxProportionLocalPopulationMin">Min value for max proportion local population.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="maxProportionLocalPopulationProbable">Probable value for max proportion local population.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="maxProportionLocalPopulationMax">Max value for max proportion local population.</param>
        public void SetMaxProportionLocalPopulation(Boolean hasMin,
                                                    Double maxProportionLocalPopulationMin,
                                                    Boolean hasProbable,
                                                    Double maxProportionLocalPopulationProbable,
                                                    Boolean hasMax,
                                                    Double maxProportionLocalPopulationMax)
        {

            if (HasNoValues(hasMin, hasProbable, hasMax))
            {
                // No values to set.
                _calculationWorstCase.HasMaxProportionLocalPopulation = false;
                _calculationProbable.HasMaxProportionLocalPopulation = false;
                _calculationBestCase.HasMaxProportionLocalPopulation = false;
            }
            else
            {
                // Fill in missing values.
                SetValues(hasMin,
                          ref maxProportionLocalPopulationMin,
                          hasProbable,
                          ref maxProportionLocalPopulationProbable,
                          hasMax,
                          ref maxProportionLocalPopulationMax,
                          MAX_PROPORTION_LOCAL_POPULATION_MIN,
                          MAX_PROPORTION_LOCAL_POPULATION_MAX);

                // Set values in RedListCalculation.
                _calculationWorstCase.MaxProportionLocalPopulation = maxProportionLocalPopulationMax;
                _calculationProbable.MaxProportionLocalPopulation = maxProportionLocalPopulationProbable;
                _calculationBestCase.MaxProportionLocalPopulation = maxProportionLocalPopulationMin;
            }
            SetRedListValues();
        }

        /// <summary>
        /// Handle max size of local population.
        /// Unit: Max number of individuals in one population.
        /// Factor: MaxSizeLocalPopulation, Id = 716
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="maxSizeLocalPopulationMin">Min value for max size of local population.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="maxSizeLocalPopulationProbable">Probable value for max size of local population.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="maxSizeLocalPopulationMax">Max value for max size of local population.</param>
        public void SetMaxSizeLocalPopulation(Boolean hasMin,
                                              Int64 maxSizeLocalPopulationMin,
                                              Boolean hasProbable,
                                              Int64 maxSizeLocalPopulationProbable,
                                              Boolean hasMax,
                                              Int64 maxSizeLocalPopulationMax)
        {
            if (HasNoValues(hasMin, hasProbable, hasMax))
            {
                // No values to set.
                _calculationWorstCase.HasMaxSizeLocalPopulation = false;
                _calculationProbable.HasMaxSizeLocalPopulation = false;
                _calculationBestCase.HasMaxSizeLocalPopulation = false;
            }
            else
            {
                // Fill in missing values.
                SetValues(hasMin,
                          ref maxSizeLocalPopulationMin,
                          hasProbable,
                          ref maxSizeLocalPopulationProbable,
                          hasMax,
                          ref maxSizeLocalPopulationMax,
                          MAX_SIZE_LOCAL_POPULATION_MIN);

                // Set values in RedListCalculation.
                _calculationWorstCase.MaxSizeLocalPopulation = maxSizeLocalPopulationMin;
                _calculationProbable.MaxSizeLocalPopulation = maxSizeLocalPopulationProbable;
                _calculationBestCase.MaxSizeLocalPopulation = maxSizeLocalPopulationMax;
            }
            SetRedListValues();
        }

        /// <summary>
        /// Handle number of locations.
        /// Unit is location count.
        /// Factor: NumberOfLocations, Id = 727,
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="numberOfLocationsMin">Min value for number of locations.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="numberOfLocationsProbable">Probable value for number of locations.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="numberOfLocationsMax">Max value for number of locations.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetNumberOfLocations(Boolean hasMin,
                                         Int64 numberOfLocationsMin,
                                         Boolean hasProbable,
                                         Int64 numberOfLocationsProbable,
                                         Boolean hasMax,
                                         Int64 numberOfLocationsMax,
                                         String clarification)
        {
            _numberOfLocationsClarification = clarification;

            if (HasNoValues(hasMin, hasProbable, hasMax))
            {
                // No values to set.
                _calculationWorstCase.HasNumberOfLocations = false;
                _calculationProbable.HasNumberOfLocations = false;
                _calculationBestCase.HasNumberOfLocations = false;
            }
            else
            {
                // Fill in missing values.
                SetValues(hasMin,
                          ref numberOfLocationsMin,
                          hasProbable,
                          ref numberOfLocationsProbable,
                          hasMax,
                          ref numberOfLocationsMax,
                          NUMBER_OF_LOCATIONS_MIN);

                // Set values in RedListCalculation.
                _calculationWorstCase.NumberOfLocations = numberOfLocationsMin;
                _calculationProbable.NumberOfLocations = numberOfLocationsProbable;
                _calculationBestCase.NumberOfLocations = numberOfLocationsMax;
            }
            SetRedListValues();
        }

        /// <summary>
        /// Handle population size reduction A1.
        /// Unit is number of per cent in reduction to
        /// previouse population size,
        /// e.g. 90% reduction has the value 90.0
        /// Factor: Reduction_A1, Id = 684,
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="populationReductionMin">Min value for population size reduction.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="populationReductionProbable">Probable value for population size reduction.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="populationReductionMax">Max value for max population size reduction.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetPopulationReductionA1(Boolean hasMin,
                                             Double populationReductionMin,
                                             Boolean hasProbable,
                                             Double populationReductionProbable,
                                             Boolean hasMax,
                                             Double populationReductionMax,
                                             String clarification)
        {
            _populationReductionA1Clarification = clarification;

            if (HasNoValues(hasMin, hasProbable, hasMax))
            {
                // No values to set.
                _calculationWorstCase.HasPopulationReductionA1 = false;
                _calculationProbable.HasPopulationReductionA1 = false;
                _calculationBestCase.HasPopulationReductionA1 = false;
            }
            else
            {
                // Fill in missing values.
                SetValues(hasMin,
                          ref populationReductionMin,
                          hasProbable,
                          ref populationReductionProbable,
                          hasMax,
                          ref populationReductionMax,
                          POPULATION_REDUCTION_MIN,
                          POPULATION_REDUCTION_MAX);

                // Set values in RedListCalculation.
                _calculationWorstCase.PopulationReductionA1 = populationReductionMax;
                _calculationProbable.PopulationReductionA1 = populationReductionProbable;
                _calculationBestCase.PopulationReductionA1 = populationReductionMin;
            }
            SetRedListValues();
        }

        /// <summary>
        /// Handle population size reduction A2.
        /// Unit is number of per cent in reduction to
        /// previouse population size,
        /// e.g. 90% reduction has the value 90.0
        /// Factor: Reduction_A2, Id = 692,
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="populationReductionMin">Min value for population size reduction.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="populationReductionProbable">Probable value for population size reduction.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="populationReductionMax">Max value for max population size reduction.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetPopulationReductionA2(Boolean hasMin,
                                             Double populationReductionMin,
                                             Boolean hasProbable,
                                             Double populationReductionProbable,
                                             Boolean hasMax,
                                             Double populationReductionMax,
                                             String clarification)
        {
            _populationReductionA2Clarification = clarification;

            if (HasNoValues(hasMin, hasProbable, hasMax))
            {
                // No values to set.
                _calculationWorstCase.HasPopulationReductionA2 = false;
                _calculationProbable.HasPopulationReductionA2 = false;
                _calculationBestCase.HasPopulationReductionA2 = false;
            }
            else
            {
                // Fill in missing values.
                SetValues(hasMin,
                          ref populationReductionMin,
                          hasProbable,
                          ref populationReductionProbable,
                          hasMax,
                          ref populationReductionMax,
                          POPULATION_REDUCTION_MIN,
                          POPULATION_REDUCTION_MAX);

                // Set values in RedListCalculation.
                _calculationWorstCase.PopulationReductionA2 = populationReductionMax;
                _calculationProbable.PopulationReductionA2 = populationReductionProbable;
                _calculationBestCase.PopulationReductionA2 = populationReductionMin;
            }
            SetRedListValues();
        }

        /// <summary>
        /// Handle population size reduction A3.
        /// Unit is number of per cent in reduction to
        /// previouse population size,
        /// e.g. 90% reduction has the value 90.0
        /// Factor: Reduction_A3, Id = 700,
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="populationReductionMin">Min value for population size reduction.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="populationReductionProbable">Probable value for population size reduction.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="populationReductionMax">Max value for max population size reduction.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetPopulationReductionA3(Boolean hasMin,
                                             Double populationReductionMin,
                                             Boolean hasProbable,
                                             Double populationReductionProbable,
                                             Boolean hasMax,
                                             Double populationReductionMax,
                                             String clarification)
        {
            _populationReductionA3Clarification = clarification;

            if (HasNoValues(hasMin, hasProbable, hasMax))
            {
                // No values to set.
                _calculationWorstCase.HasPopulationReductionA3 = false;
                _calculationProbable.HasPopulationReductionA3 = false;
                _calculationBestCase.HasPopulationReductionA3 = false;
            }
            else
            {
                // Fill in missing values.
                SetValues(hasMin,
                          ref populationReductionMin,
                          hasProbable,
                          ref populationReductionProbable,
                          hasMax,
                          ref populationReductionMax,
                          POPULATION_REDUCTION_MIN,
                          POPULATION_REDUCTION_MAX);

                // Set values in RedListCalculation.
                _calculationWorstCase.PopulationReductionA3 = populationReductionMax;
                _calculationProbable.PopulationReductionA3 = populationReductionProbable;
                _calculationBestCase.PopulationReductionA3 = populationReductionMin;
            }
            SetRedListValues();
        }

        /// <summary>
        /// Handle population size reduction A4.
        /// Unit is number of per cent in reduction to
        /// previouse population size,
        /// e.g. 90% reduction has the value 90.0
        /// Factor: Reduction_A4, Id = 707,
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="populationReductionMin">Min value for population size reduction.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="populationReductionProbable">Probable value for population size reduction.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="populationReductionMax">Max value for max population size reduction.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetPopulationReductionA4(Boolean hasMin,
                                             Double populationReductionMin,
                                             Boolean hasProbable,
                                             Double populationReductionProbable,
                                             Boolean hasMax,
                                             Double populationReductionMax,
                                             String clarification)
        {
            _populationReductionA4Clarification = clarification;

            if (HasNoValues(hasMin, hasProbable, hasMax))
            {
                // No values to set.
                _calculationWorstCase.HasPopulationReductionA4 = false;
                _calculationProbable.HasPopulationReductionA4 = false;
                _calculationBestCase.HasPopulationReductionA4 = false;
            }
            else
            {
                // Fill in missing values.
                SetValues(hasMin,
                          ref populationReductionMin,
                          hasProbable,
                          ref populationReductionProbable,
                          hasMax,
                          ref populationReductionMax,
                          POPULATION_REDUCTION_MIN,
                          POPULATION_REDUCTION_MAX);

                // Set values in RedListCalculation.
                _calculationWorstCase.PopulationReductionA4 = populationReductionMax;
                _calculationProbable.PopulationReductionA4 = populationReductionProbable;
                _calculationBestCase.PopulationReductionA4 = populationReductionMin;
            }
            SetRedListValues();
        }

        /// <summary>
        /// Handle population size.
        /// Unit. Number of mature individuals.
        /// Factor: PopulationSize_Total, Id = 715
        /// Has Probable, min and max values.
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="populationSizeMin">Min value for population size.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="populationSizeProbable">Probable value for population size.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="populationSizeMax">Max value for population size.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetPopulationSize(Boolean hasMin,
                                      Int64 populationSizeMin,
                                      Boolean hasProbable,
                                      Int64 populationSizeProbable,
                                      Boolean hasMax,
                                      Int64 populationSizeMax,
                                      String clarification)
        {
            _populationSizeClarification = clarification;

            if (HasNoValues(hasMin, hasProbable, hasMax))
            {
                // No values to set.
                _calculationWorstCase.HasPopulationSize = false;
                _calculationProbable.HasPopulationSize = false;
                _calculationBestCase.HasPopulationSize = false;
            }
            else
            {
                // Fill in missing values.
                SetValues(hasMin,
                          ref populationSizeMin,
                          hasProbable,
                          ref populationSizeProbable,
                          hasMax,
                          ref populationSizeMax,
                          POPULATION_SIZE_MIN);

                // Set values in RedListCalculation.
                _calculationWorstCase.PopulationSize = populationSizeMin;
                _calculationProbable.PopulationSize = populationSizeProbable;
                _calculationBestCase.PopulationSize = populationSizeMax;
            }
            SetRedListValues();
        }

        /// <summary>
        /// Handle probability of extinction.
        /// Unit.
        /// Unit.
        ///  0 = Probability of extinction is at least 50% within 10 years or 3 generations.
        ///  1 = Probability of extinction is at least 20% within 20 years or 5 generations.
        ///  2 = Probability of extinction is at least 10% within 100 years.
        ///  3 = Probability of extinction is at least 5% within 100 years.
        ///  4 = Probability of extinction is at lower than 5% within 100 years.
        /// Factor: ProbabilityOfExtinction, Id = 736.
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="probabilityOfExtinctionMin">Min value for probability of extinction.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="probabilityOfExtinctionProbable">Probable value for probability of extinction.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="probabilityOfExtinctionMax">Max value for probability of extinction.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetProbabilityOfExtinction(Boolean hasMin,
                                               Int32 probabilityOfExtinctionMin,
                                               Boolean hasProbable,
                                               Int32 probabilityOfExtinctionProbable,
                                               Boolean hasMax,
                                               Int32 probabilityOfExtinctionMax,
                                               String clarification)
        {
            _probabilityOfExtinctionClarification = clarification;

            if (HasNoValues(hasMin, hasProbable, hasMax))
            {
                // No values to set.
                _calculationWorstCase.HasProbabilityOfExtinction = false;
                _calculationProbable.HasProbabilityOfExtinction = false;
                _calculationBestCase.HasProbabilityOfExtinction = false;
            }
            else
            {
                // Fill in missing values.
                // Min and max are switched on purpose since
                // a high probability of extinction has a lower
                // integer value than low probability of extinction.
                SetValues(hasMin,
                          ref probabilityOfExtinctionMax,
                          hasProbable,
                          ref probabilityOfExtinctionProbable,
                          hasMax,
                          ref probabilityOfExtinctionMin,
                          PROBABILITY_OF_EXTINCTION_MIN,
                          PROBABILITY_OF_EXTINCTION_MAX);

                // Set values in RedListCalculation.
                _calculationWorstCase.ProbabilityOfExtinction = probabilityOfExtinctionMax;
                _calculationProbable.ProbabilityOfExtinction = probabilityOfExtinctionProbable;
                _calculationBestCase.ProbabilityOfExtinction = probabilityOfExtinctionMin;
            }
            SetRedListValues();
        }

        /// <summary>
        /// Handle how current redlist evaluation status
        /// affects Category.
        /// Factor: RedlistEvaluationProgressionStatus, Id = 654.
        /// </summary>
        /// <param name="redlistEvaluationStatus">
        /// Indicates how current redlist evaluation status affects Category.
        /// </param>
        public void SetRedlistEvaluationStatus(RedListCategory redlistEvaluationStatus)
        {
            _redlistEvaluationStatus = redlistEvaluationStatus;
            SetRedListValues();
        }

        /// <summary>
        /// Recalculated red list values.
        /// </summary>
        protected virtual void SetRedListValues()
        {
            // Nothing to do in base class.
            // All calculation is done in RedListCalculation.
        }

        /// <summary>
        /// Handle severly fragmented.
        /// Unit:
        /// 0 = Not severly fragmented.
        /// 1 = Probably severly fragmented.
        /// 2 = Populationen is severly fragmented.
        /// Factor: SeverelyFragmented, Id = 726,
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="severlyFragmentedMin">Min value for severly fragmented.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="severlyFragmentedProbable">Probable value for severly fragmented.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="severlyFragmentedMax">Max value for severly fragmented.</param>
        public void SetSeverlyFragmented(Boolean hasMin,
                                         Int32 severlyFragmentedMin,
                                         Boolean hasProbable,
                                         Int32 severlyFragmentedProbable,
                                         Boolean hasMax,
                                         Int32 severlyFragmentedMax)
        {
            if (HasNoValues(hasMin, hasProbable, hasMax))
            {
                _calculationWorstCase.HasSeverlyFragmented = false;
                _calculationProbable.HasSeverlyFragmented = false;
                _calculationBestCase.HasSeverlyFragmented = false;
                // No values to set.
            }
            else
            {
                // Fill in missing values.
                SetValues(hasMin,
                          ref severlyFragmentedMin,
                          hasProbable,
                          ref severlyFragmentedProbable,
                          hasMax,
                          ref severlyFragmentedMax,
                          SEVERLY_FRAGMENTED_MIN,
                          SEVERLY_FRAGMENTED_MAX);

                // Set values in RedListCalculation.
                _calculationWorstCase.SeverlyFragmented = severlyFragmentedMax;
                _calculationProbable.SeverlyFragmented = severlyFragmentedProbable;
                _calculationBestCase.SeverlyFragmented = severlyFragmentedMin;
            }
            SetRedListValues();
        }

        /// <summary>
        /// Handle continuing decline.
        /// Unit.
        /// -1 = Populationen growth.
        ///  0 = Populationen is stable.
        ///  1 = Presumable decrease.
        ///  2 = Decrease or expected decrease.
        ///  3 = Decrease of >5% within 10 years or 3 generations
        ///  4 = Decrease of >10% within 10 years or 3 generations
        ///  5 = Decrease of >20% within 5 years or 2 generations
        ///  6 = Decrease of >25% within 3 years or 1 generations
        /// Factor: ContinuingDecline, Id = 678
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="continuingDeclineMin">Min value for continuing decline.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="continuingDeclineProbable">Probable value for continuing decline.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="continuingDeclineMax">Max value for continuing decline.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetContinuingDecline(Boolean hasMin,
                                         Int32 continuingDeclineMin,
                                         Boolean hasProbable,
                                         Int32 continuingDeclineProbable,
                                         Boolean hasMax,
                                         Int32 continuingDeclineMax,
                                         String clarification)
        {
            _continuingDeclineClarification = clarification;

            if (HasNoValues(hasMin, hasProbable, hasMax))
            {
                // No values to set.
                _calculationWorstCase.HasContinuingDecline = false;
                _calculationProbable.HasContinuingDecline = false;
                _calculationBestCase.HasContinuingDecline = false;
            }
            else
            {
                // Fill in missing values.
                SetValues(hasMin,
                          ref continuingDeclineMin,
                          hasProbable,
                          ref continuingDeclineProbable,
                          hasMax,
                          ref continuingDeclineMax,
                          SMALL_POPULATION_CONTINUING_DECLINE_MIN,
                          SMALL_POPULATION_CONTINUING_DECLINE_MAX);

                // Set values in RedListCalculation.
                _calculationWorstCase.ContinuingDecline = continuingDeclineMax;
                _calculationProbable.ContinuingDecline = continuingDeclineProbable;
                _calculationBestCase.ContinuingDecline = continuingDeclineMin;
            }
            SetRedListValues();
        }

        /// <summary>
        /// Set values that has not been set with the
        /// help of the other values.
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="valueMin">Min value.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="valueProbable">Probable value.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="valueMax">Max value.</param>
        /// <param name="minValue">The lowest possible value for the data type.</param>
        /// <param name="maxValue">The highest possible value for the data type.</param>
        protected void SetValues(Boolean hasMin,
                                 ref Double valueMin,
                                 Boolean hasProbable,
                                 ref Double valueProbable,
                                 Boolean hasMax,
                                 ref Double valueMax,
                                 Double minValue,
                                 Double maxValue)
        {
            if (HasOneValue(hasMin, hasProbable, hasMax))
            {
                // Only one value. Use that value in all three cases.
                if (hasMin)
                {
                    valueProbable = valueMin;
                    valueMax = valueMin;
                }
                if (hasProbable)
                {
                    valueMin = valueProbable;
                    valueMax = valueProbable;
                }
                if (hasMax)
                {
                    valueMin = valueMax;
                    valueProbable = valueMax;
                }
            }

            if (HasTwoValues(hasMin, hasProbable, hasMax))
            {
                // One value is missing.
                // Calculate missing value with the help of the other values.
                if (!hasMin)
                {
                    if (valueProbable > valueMax)
                    {
                        valueMin = valueMax;
                    }
                    else
                    {
                        valueMin = valueProbable;
                    }
                }
                if (!hasProbable)
                {
                    valueProbable = (valueMax + valueMin) / 2;
                }
                if (!hasMax)
                {
                    if (valueProbable < valueMin)
                    {
                        valueMax = valueMin;
                    }
                    else
                    {
                        valueMax = valueProbable;
                    }
                }
            }
        }

        /// <summary>
        /// Set values that has not been set with the
        /// help of the other values.
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="valueMin">Min value.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="valueProbable">Probable value.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="valueMax">Max value.</param>
        /// <param name="minValue">The lowest possible value for the data type.</param>
        /// <param name="maxValue">The highest possible value for the data type.</param>
        private void SetValues(Boolean hasMin,
                               ref Int32 valueMin,
                               Boolean hasProbable,
                               ref Int32 valueProbable,
                               Boolean hasMax,
                               ref Int32 valueMax,
                               Int32 minValue,
                               Int32 maxValue)
        {
            if (HasOneValue(hasMin, hasProbable, hasMax))
            {
                // Only one value. Use that value in all three cases.
                if (hasMin)
                {
                    valueProbable = valueMin;
                    valueMax = valueMin;
                }
                if (hasProbable)
                {
                    valueMin = valueProbable;
                    valueMax = valueProbable;
                }
                if (hasMax)
                {
                    valueMin = valueMax;
                    valueProbable = valueMax;
                }
            }

            if (HasTwoValues(hasMin, hasProbable, hasMax))
            {
                // One value is missing.
                // Calculate missing value with the help of the other values.
                if (!hasMin)
                {
                    if (valueProbable > valueMax)
                    {
                        valueMin = valueMax;
                    }
                    else
                    {
                        valueMin = valueProbable;
                    }
                }
                if (!hasProbable)
                {
                    valueProbable = (valueMax + valueMin) / 2;
                }
                if (!hasMax)
                {
                    if (valueProbable < valueMin)
                    {
                        valueMax = valueMin;
                    }
                    else
                    {
                        valueMax = valueProbable;
                    }
                }
            }
        }

        /// <summary>
        /// Set values that has not been set with the
        /// help of the other values.
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="valueMin">Min value.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="valueProbable">Probable value.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="valueMax">Max value.</param>
        /// <param name="minValue">The lowest possible value for the data type.</param>
        private void SetValues(Boolean hasMin,
                               ref Int64 valueMin,
                               Boolean hasProbable,
                               ref Int64 valueProbable,
                               Boolean hasMax,
                               ref Int64 valueMax,
                               Int64 minValue)
        {
            if (HasOneValue(hasMin, hasProbable, hasMax))
            {
                // Only one value. Use that value in all three cases.
                if (hasMin)
                {
                    valueProbable = valueMin;
                    valueMax = valueMin;
                }
                if (hasProbable)
                {
                    valueMin = valueProbable;
                    valueMax = valueProbable;
                }
                if (hasMax)
                {
                    valueMin = valueMax;
                    valueProbable = valueMax;
                }
            }

            if (HasTwoValues(hasMin, hasProbable, hasMax))
            {
                // One value is missing.
                // Calculate missing value with the help of the other values.
                if (!hasMin)
                {
                    if (valueProbable > valueMax)
                    {
                        valueMin = valueMax;
                    }
                    else
                    {
                        valueMin = valueProbable;
                    }
                }
                if (!hasProbable)
                {
                    valueProbable = (valueMax + valueMin) / 2;
                }
                if (!hasMax)
                {
                    if (valueProbable < valueMin)
                    {
                        valueMax = valueMin;
                    }
                    else
                    {
                        valueMax = valueProbable;
                    }
                }
            }
        }

        /// <summary>
        /// Handle very restricted area.
        /// Unit.
        ///  0 = VU.
        ///  1 = NT.
        ///  2 = LC.
        /// Factor: VeryRestrictedArea_D2VU, Id = 728.
        /// </summary>
        /// <param name="hasMin">Indicates if min value is available.</param>
        /// <param name="veryRestrictedAreaMin">Min value for probability of extinction.</param>
        /// <param name="hasProbable">Indicates if probable value is available.</param>
        /// <param name="veryRestrictedAreaProbable">Probable value for probability of extinction.</param>
        /// <param name="hasMax">Indicates if max value is available.</param>
        /// <param name="veryRestrictedAreaMax">Max value for probability of extinction.</param>
        /// <param name="clarification">Clarification about values.</param>
        public void SetVeryRestrictedArea(Boolean hasMin,
                                          Int32 veryRestrictedAreaMin,
                                          Boolean hasProbable,
                                          Int32 veryRestrictedAreaProbable,
                                          Boolean hasMax,
                                          Int32 veryRestrictedAreaMax,
                                          String clarification)
        {
            _veryRestrictedAreaClarification = clarification;
            if (HasNoValues(hasMin, hasProbable, hasMax))
            {
                // No values to set.
                _calculationWorstCase.HasVeryRestrictedArea = false;
                _calculationProbable.HasVeryRestrictedArea = false;
                _calculationBestCase.HasVeryRestrictedArea = false;
            }
            else
            {
                // Fill in missing values.
                // Min and max are switched on purpose since
                // a high risk of extinction has a lower
                // integer value than low risk of extinction.
                SetValues(hasMin,
                          ref veryRestrictedAreaMax,
                          hasProbable,
                          ref veryRestrictedAreaProbable,
                          hasMax,
                          ref veryRestrictedAreaMin,
                          VERY_RESTRICTED_AREA_MIN,
                          VERY_RESTRICTED_AREA_MAX);

                // Set values in RedListCalculation.
                _calculationWorstCase.VeryRestrictedArea = veryRestrictedAreaMax;
                _calculationProbable.VeryRestrictedArea = veryRestrictedAreaProbable;
                _calculationBestCase.VeryRestrictedArea = veryRestrictedAreaMin;
            }
            SetRedListValues();
        }
    }
}

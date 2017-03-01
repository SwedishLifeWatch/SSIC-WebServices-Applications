using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class calculates red list category and red list criteria
    /// for one of the three (max, min, likely) values for
    /// red list data.
    /// </summary>
    [Serializable]
    public class RedListCalculation
    {
        /// <summary> CRITERIA_AREA_OF_OCCUPANCY_RE_LIMIT. </summary>
        public const Int32 CRITERIA_AREA_OF_OCCUPANCY_RE_LIMIT = 0;
        /// <summary> CRITERIA_EXTENT_OF_OCCURRENCE_RE_LIMIT. </summary>
        public const Int32 CRITERIA_EXTENT_OF_OCCURRENCE_RE_LIMIT = 0;
        /// <summary> CRITERIA_NUMBER_OF_LOCATIONS_RE_LIMIT. </summary>
        public const Int32 CRITERIA_NUMBER_OF_LOCATIONS_RE_LIMIT = 0;
        /// <summary> CRITERIA_POPULATION_SIZE_RE_LIMIT. </summary>
        public const Int32 CRITERIA_POPULATION_SIZE_RE_LIMIT = 0;

        /// <summary> CRITERIA_A1_CR_LIMIT. </summary>
        public const Int32 CRITERIA_A1_CR_LIMIT = 90;
        /// <summary> CRITERIA_A1_EN_LIMIT. </summary>
        public const Int32 CRITERIA_A1_EN_LIMIT = 70;
        /// <summary> CRITERIA_A1_VU_LIMIT. </summary>
        public const Int32 CRITERIA_A1_VU_LIMIT = 50;
        /// <summary> CRITERIA_A1_NT_LIMIT. </summary>
        public const Int32 CRITERIA_A1_NT_LIMIT = 25;

        /// <summary> CRITERIA_A2_CR_LIMIT. </summary>
        public const Int32 CRITERIA_A2_CR_LIMIT = 80;
        /// <summary> CRITERIA_A2_EN_LIMIT. </summary>
        public const Int32 CRITERIA_A2_EN_LIMIT = 50;
        /// <summary> CRITERIA_A2_VU_LIMIT. </summary>
        public const Int32 CRITERIA_A2_VU_LIMIT = 30;
        /// <summary> CRITERIA_A2_NT_LIMIT. </summary>
        public const Int32 CRITERIA_A2_NT_LIMIT = 15;

        /// <summary> CRITERIA_A3_CR_LIMIT. </summary>
        public const Int32 CRITERIA_A3_CR_LIMIT = 80;
        /// <summary> CRITERIA_A3_EN_LIMIT. </summary>
        public const Int32 CRITERIA_A3_EN_LIMIT = 50;
        /// <summary> CRITERIA_A3_VU_LIMIT. </summary>
        public const Int32 CRITERIA_A3_VU_LIMIT = 30;
        /// <summary> CRITERIA_A3_NT_LIMIT. </summary>
        public const Int32 CRITERIA_A3_NT_LIMIT = 15;

        /// <summary> CRITERIA_A4_CR_LIMIT. </summary>
        public const Int32 CRITERIA_A4_CR_LIMIT = 80;
        /// <summary> CRITERIA_A4_EN_LIMIT. </summary>
        public const Int32 CRITERIA_A4_EN_LIMIT = 50;
        /// <summary> CRITERIA_A4_VU_LIMIT. </summary>
        public const Int32 CRITERIA_A4_VU_LIMIT = 30;
        /// <summary> CRITERIA_A4_NT_LIMIT. </summary>
        public const Int32 CRITERIA_A4_NT_LIMIT = 15;

        /// <summary> CRITERIA_B1_CR_LIMIT. </summary>
        public const Int64 CRITERIA_B1_CR_LIMIT = 100;
        /// <summary> CRITERIA_B1_EN_LIMIT. </summary>
        public const Int64 CRITERIA_B1_EN_LIMIT = 5000;
        /// <summary> CRITERIA_B1_VU_LIMIT. </summary>
        public const Int64 CRITERIA_B1_VU_LIMIT = 20000;
        /// <summary> CRITERIA_B1_NT_LIMIT. </summary>
        public const Int64 CRITERIA_B1_NT_LIMIT = 40000;

        /// <summary> CRITERIA_B2_CR_LIMIT. </summary>
        public const Int64 CRITERIA_B2_CR_LIMIT = 10;
        /// <summary> CRITERIA_B2_EN_LIMIT. </summary>
        public const Int64 CRITERIA_B2_EN_LIMIT = 500;
        /// <summary> CRITERIA_B2_VU_LIMIT. </summary>
        public const Int64 CRITERIA_B2_VU_LIMIT = 2000;
        /// <summary> CRITERIA_B2_NT_LIMIT. </summary>
        public const Int64 CRITERIA_B2_NT_LIMIT = 4000;

        /// <summary> CRITERIA_BA_CR_LIMIT. </summary>
        public const Int32 CRITERIA_BA_CR_LIMIT = 1;
        /// <summary> CRITERIA_BA_EN_LIMIT. </summary>
        public const Int32 CRITERIA_BA_EN_LIMIT = 5;
        /// <summary> CRITERIA_BA_VU_LIMIT. </summary>
        public const Int32 CRITERIA_BA_VU_LIMIT = 10;
        /// <summary> CRITERIA_BA_NT_LIMIT. </summary>
        public const Int32 CRITERIA_BA_NT_LIMIT = 20;

        /// <summary> CRITERIA_BA_2_LIMIT. </summary>
        public const Int32 CRITERIA_BA_2_LIMIT = 2;
        /// <summary> CRITERIA_BA_1_LIMIT. </summary>
        public const Int32 CRITERIA_BA_1_LIMIT = 1;

        /// <summary> CRITERIA_BB_2_LIMIT. </summary>
        public const Int32 CRITERIA_BB_2_LIMIT = 2;
        /// <summary> CRITERIA_BB_1_LIMIT. </summary>
        public const Int32 CRITERIA_BB_1_LIMIT = 1;

        /// <summary> CRITERIA_BC_2_LIMIT. </summary>
        public const Int32 CRITERIA_BC_2_LIMIT = 2;
        /// <summary> CRITERIA_BC_1_LIMIT. </summary>
        public const Int32 CRITERIA_BC_1_LIMIT = 1;

        /// <summary> CRITERIA_C_CR_LIMIT. </summary>
        public const Int64 CRITERIA_C_CR_LIMIT = 250;
        /// <summary> CRITERIA_C_EN_LIMIT. </summary>
        public const Int64 CRITERIA_C_EN_LIMIT = 2500;
        /// <summary> CRITERIA_C_VU_LIMIT. </summary>
        public const Int64 CRITERIA_C_VU_LIMIT = 10000;
        /// <summary> CRITERIA_C_NT_LIMIT. </summary>
        public const Int64 CRITERIA_C_NT_LIMIT = 20000;

        /// <summary> CRITERIA_C1_CR_LIMIT. </summary>
        public const Int32 CRITERIA_C1_CR_LIMIT = 6;
        /// <summary> CRITERIA_C1_EN_LIMIT. </summary>
        public const Int32 CRITERIA_C1_EN_LIMIT = 5;
        /// <summary> CRITERIA_C1_VU_LIMIT. </summary>
        public const Int32 CRITERIA_C1_VU_LIMIT = 4;
        /// <summary> CRITERIA_C1_NT_LIMIT. </summary>
        public const Int32 CRITERIA_C1_NT_LIMIT = 3;

        /// <summary> CRITERIA_C2_LIMIT. </summary>
        public const Int32 CRITERIA_C2_LIMIT = 2;

        /// <summary> CRITERIA_C2A1_CR_LIMIT. </summary>
        public const Int64 CRITERIA_C2A1_CR_LIMIT = 50;
        /// <summary> CRITERIA_C2A1_EN_LIMIT. </summary>
        public const Int64 CRITERIA_C2A1_EN_LIMIT = 250;
        /// <summary> CRITERIA_C2A1_VU_LIMIT. </summary>
        public const Int64 CRITERIA_C2A1_VU_LIMIT = 1000;
        /// <summary> CRITERIA_C2A1_NT_LIMIT. </summary>
        public const Int64 CRITERIA_C2A1_NT_LIMIT = 2000;

        /// <summary> CRITERIA_C2A2_CR_LIMIT. </summary>
        public const Int32 CRITERIA_C2A2_CR_LIMIT = 90;
        /// <summary> CRITERIA_C2A2_EN_LIMIT. </summary>
        public const Int32 CRITERIA_C2A2_EN_LIMIT = 95;
        /// <summary> CRITERIA_C2A2_VU_LIMIT. </summary>
        public const Int32 CRITERIA_C2A2_VU_LIMIT = 100;
        /// <summary> CRITERIA_C2A2_NT_LIMIT. </summary>
        public const Int32 CRITERIA_C2A2_NT_LIMIT = 100;

        /// <summary> CRITERIA_C2B_LIMIT. </summary>
        public const Int32 CRITERIA_C2B_LIMIT = 2;

        /// <summary> CRITERIA_D_CR_LIMIT. </summary>
        public const Int64 CRITERIA_D_CR_LIMIT = 50;
        /// <summary> CRITERIA_D_EN_LIMIT. </summary>
        public const Int64 CRITERIA_D_EN_LIMIT = 250;

        /// <summary> CRITERIA_D1_VU_LIMIT. </summary>
        public const Int64 CRITERIA_D1_VU_LIMIT = 1000;
        /// <summary> CRITERIA_D1_NT_LIMIT. </summary>
        public const Int64 CRITERIA_D1_NT_LIMIT = 2000;

        /// <summary> CRITERIA_D2_VU_LIMIT. </summary>
        public const Int32 CRITERIA_D2_VU_LIMIT = 0;
        /// <summary> CRITERIA_D2_NT_LIMIT. </summary>
        public const Int32 CRITERIA_D2_NT_LIMIT = 1;

        /// <summary> CRITERIA_E_CR_LIMIT. </summary>
        public const Int32 CRITERIA_E_CR_LIMIT = 0;
        /// <summary> CRITERIA_E_EN_LIMIT. </summary>
        public const Int32 CRITERIA_E_EN_LIMIT = 1;
        /// <summary> CRITERIA_E_VU_LIMIT. </summary>
        public const Int32 CRITERIA_E_VU_LIMIT = 2;
        /// <summary> CRITERIA_E_NT_LIMIT. </summary>
        public const Int32 CRITERIA_E_NT_LIMIT = 3;

        /// <summary> AREA_OF_OCCUPANCY_MIN. </summary>
        public const Double AREA_OF_OCCUPANCY_MIN = 0;
        /// <summary> AREA_OF_OCCUPANCY_MAX. </summary>
        public const Double AREA_OF_OCCUPANCY_MAX = Double.MaxValue;

        /// <summary> EXTENT_OF_OCCURRENCE_MIN. </summary>
        public const Double EXTENT_OF_OCCURRENCE_MIN = 0;
        /// <summary> EXTENT_OF_OCCURRENCE_MAX. </summary>
        public const Double EXTENT_OF_OCCURRENCE_MAX = Double.MaxValue;

        /// <summary> MAX_PROPORTION_LOCAL_POPULATION_MIN. </summary>
        public const Double MAX_PROPORTION_LOCAL_POPULATION_MIN = 0;
        /// <summary> MAX_PROPORTION_LOCAL_POPULATION_MAX. </summary>
        public const Double MAX_PROPORTION_LOCAL_POPULATION_MAX = 100;

        /// <summary> MAX_SIZE_LOCAL_POPULATION_MIN. </summary>
        public const Int64 MAX_SIZE_LOCAL_POPULATION_MIN = 0;
        /// <summary> MAX_SIZE_LOCAL_POPULATION_MAX. </summary>
        public const Int64 MAX_SIZE_LOCAL_POPULATION_MAX = Int64.MaxValue;

        /// <summary> NUMBER_OF_LOCATIONS_MIN. </summary>
        public const Int64 NUMBER_OF_LOCATIONS_MIN = 0;
        /// <summary> NUMBER_OF_LOCATIONS_MAX. </summary>
        public const Int64 NUMBER_OF_LOCATIONS_MAX = Int32.MaxValue;

        /// <summary> POPULATION_REDUCTION_A1_MIN. </summary>
        public const Double POPULATION_REDUCTION_A1_MIN = Double.MinValue;
        /// <summary> POPULATION_REDUCTION_A1_MAX. </summary>
        public const Double POPULATION_REDUCTION_A1_MAX = 100;
        /// <summary> POPULATION_REDUCTION_A2_MIN. </summary>
        public const Double POPULATION_REDUCTION_A2_MIN = Double.MinValue;
        /// <summary> POPULATION_REDUCTION_A2_MAX. </summary>
        public const Double POPULATION_REDUCTION_A2_MAX = 100;
        /// <summary> POPULATION_REDUCTION_A3_MIN. </summary>
        public const Double POPULATION_REDUCTION_A3_MIN = Double.MinValue;
        /// <summary> POPULATION_REDUCTION_A3_MAX. </summary>
        public const Double POPULATION_REDUCTION_A3_MAX = 100;
        /// <summary> POPULATION_REDUCTION_A4_MIN. </summary>
        public const Double POPULATION_REDUCTION_A4_MIN = Double.MinValue;
        /// <summary> POPULATION_REDUCTION_A4_MAX. </summary>
        public const Double POPULATION_REDUCTION_A4_MAX = 100;

        /// <summary> POPULATION_SIZE_MIN. </summary>
        public const Int64 POPULATION_SIZE_MIN = 0;
        /// <summary> POPULATION_SIZE_MAX. </summary>
        public const Int64 POPULATION_SIZE_MAX = Int32.MaxValue;

        private Boolean _isCriteriaA1AFulfilled;
        private Boolean _isCriteriaA1BFulfilled;
        private Boolean _isCriteriaA1CFulfilled;
        private Boolean _isCriteriaA1DFulfilled;
        private Boolean _isCriteriaA1EFulfilled;
        private Boolean _isCriteriaA2AFulfilled;
        private Boolean _isCriteriaA2BFulfilled;
        private Boolean _isCriteriaA2CFulfilled;
        private Boolean _isCriteriaA2DFulfilled;
        private Boolean _isCriteriaA2EFulfilled;
        private Boolean _isCriteriaA3BFulfilled;
        private Boolean _isCriteriaA3CFulfilled;
        private Boolean _isCriteriaA3DFulfilled;
        private Boolean _isCriteriaA3EFulfilled;
        private Boolean _isCriteriaA4AFulfilled;
        private Boolean _isCriteriaA4BFulfilled;
        private Boolean _isCriteriaA4CFulfilled;
        private Boolean _isCriteriaA4DFulfilled;
        private Boolean _isCriteriaA4EFulfilled;
        private Boolean _isCriteriaBB1Fulfilled;
        private Boolean _isCriteriaBB2Fulfilled;
        private Boolean _isCriteriaBB3Fulfilled;
        private Boolean _isCriteriaBB4Fulfilled;
        private Boolean _isCriteriaBB5Fulfilled;
        private Boolean _isCriteriaBC1Fulfilled;
        private Boolean _isCriteriaBC2Fulfilled;
        private Boolean _isCriteriaBC3Fulfilled;
        private Boolean _isCriteriaBC4Fulfilled;
        private Boolean _isCriteriaCalculated;
        private Boolean _isEvaluationStatusSet;
        private Boolean _isInInit;
        private Double _areaOfOccupancy;
        private Double _extentOfOccurrence;
        private Double _maxProportionLocalPopulation;
        private Double _populationReductionA1;
        private Double _populationReductionA2;
        private Double _populationReductionA3;
        private Double _populationReductionA4;
        private Int32 _continuingDecline;
        private Int32 _extremeFluctuations;
        private Int32 _probabilityOfExtinction;
        private Int32 _severlyFragmented;
        private Int32 _veryRestrictedArea;
        private Int64 _maxSizeLocalPopulation;
        private Int64 _numberOfLocations;
        private Int64 _populationSize;
        private RedListCategory _category;
        private String _criteria;
        private Double _generationLength;

        /// <summary>
        /// Creates a RedListCalculation instance.
        /// </summary>
        public RedListCalculation()
        {
            _category = RedListCategory.LC;
            _criteria = null;
            _areaOfOccupancy = Double.NaN;
            _continuingDecline = Int32.MinValue;
            _extentOfOccurrence = Double.NaN;
            _extremeFluctuations = Int32.MinValue;
            _isCriteriaA1AFulfilled = false;
            _isCriteriaA1BFulfilled = false;
            _isCriteriaA1CFulfilled = false;
            _isCriteriaA1DFulfilled = false;
            _isCriteriaA1EFulfilled = false;
            _isCriteriaA2AFulfilled = false;
            _isCriteriaA2BFulfilled = false;
            _isCriteriaA2CFulfilled = false;
            _isCriteriaA2DFulfilled = false;
            _isCriteriaA2EFulfilled = false;
            _isCriteriaA3BFulfilled = false;
            _isCriteriaA3CFulfilled = false;
            _isCriteriaA3DFulfilled = false;
            _isCriteriaA3EFulfilled = false;
            _isCriteriaA4AFulfilled = false;
            _isCriteriaA4BFulfilled = false;
            _isCriteriaA4CFulfilled = false;
            _isCriteriaA4DFulfilled = false;
            _isCriteriaA4EFulfilled = false;
            _isCriteriaBB1Fulfilled = false;
            _isCriteriaBB2Fulfilled = false;
            _isCriteriaBB3Fulfilled = false;
            _isCriteriaBB4Fulfilled = false;
            _isCriteriaBB5Fulfilled = false;
            _isCriteriaBC1Fulfilled = false;
            _isCriteriaBC2Fulfilled = false;
            _isCriteriaBC3Fulfilled = false;
            _isCriteriaBC4Fulfilled = false;
            _isCriteriaCalculated = true;
            _isEvaluationStatusSet = false;
            _isInInit = false;
            _maxProportionLocalPopulation = Double.NaN;
            _maxSizeLocalPopulation = Int64.MinValue;
            _numberOfLocations = Int64.MinValue;
            _populationReductionA1 = Double.NaN;
            _populationReductionA2 = Double.NaN;
            _populationReductionA3 = Double.NaN;
            _populationReductionA4 = Double.NaN;
            _populationSize = Int64.MinValue;
            _probabilityOfExtinction = Int32.MinValue;
            _severlyFragmented = Int32.MinValue;
            _veryRestrictedArea = Int32.MinValue;
        }


        /// <summary>
        /// Handle generation length.
        /// </summary>
        public Double GenerationLength
        {
            get { return _generationLength; }
            set
            {
                _generationLength = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Handle area of occupancy.
        /// Unit is square km.
        /// Factor: AreaOfOccupancy_B2Estimated, Id = 734.
        /// Has Probable, min and max values.
        /// Usage: Is used by criteria B2.
        /// </summary>
        public Double AreaOfOccupancy
        {
            get { return _areaOfOccupancy; }
            set
            {
                if ((AREA_OF_OCCUPANCY_MIN <= value) &&
                    (value <= AREA_OF_OCCUPANCY_MAX))
                {
                    _areaOfOccupancy = value;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException(value.ToString() + " is not a valid value for area of occupancy");
                }
            }
        }

        //public String Introduction
        //{
        //    get { return _introduction; }
        //    set { 
        //            _introduction = value;
        //            SetRedListValues();
        //        }
        //}

        /// <summary>
        /// Get red list category.
        /// </summary>
        public RedListCategory Category
        {
            get
            {
                if (IsEvaluationStatusSet)
                {
                    return _category;
                }
                else
                {
                    return RedListCategory.NA;
                }
            }
        }

        /// <summary>
        /// Handle continuing decline.
        /// Unit.
        /// -1 = Populationen growth.
        ///  0 = Populationen is stable.
        ///  1 = Probable decrease.
        ///  2 = Decrease or expected decrease.
        ///  3 = Decrease of >5% within 10 years or 3 generations
        ///  4 = Decrease of >10% within 10 years or 3 generations
        ///  5 = Decrease of >20% within 5 years or 2 generations
        ///  6 = Decrease of >25% within 3 years or 1 generations
        /// Factor: ContinuingDecline, Id = 678.
        /// Usage: Is used by criteria B1B, B2B and C2.
        /// </summary>
        public Int32 ContinuingDecline
        {
            get { return _continuingDecline; }
            set
            {
                if ((ContinuingDeclineMin <= value) &&
                    (value <= ContinuingDeclineMax))
                {
                    _continuingDecline = value;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException(value.ToString() + " is not a valid value for continuing decline");
                }
            }
        }

        /// <summary>
        /// Max value for continuing decline.
        /// Factor: ContinuingDecline, Id = 678.
        /// </summary>
        public Int32 ContinuingDeclineMax
        {
            get
            {
                Factor factor;
                FactorField factorField;

                factor = FactorManager.GetFactor(FactorId.ContinuingDecline);
                factorField = factor.FactorDataType.Field1;
                return factorField.FactorFieldEnum.GetMaxKeyInt();
            }
        }

        /// <summary>
        /// Min value for continuing decline.
        /// Factor: ContinuingDecline, Id = 678.
        /// </summary>
        public Int32 ContinuingDeclineMin
        {
            get
            {
                Factor factor;
                FactorField factorField;

                factor = FactorManager.GetFactor(FactorId.ContinuingDecline);
                factorField = factor.FactorDataType.Field1;
                return factorField.FactorFieldEnum.GetMinKeyInt();
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
                    return _criteria;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Handle extent of occurrence.
        /// Unit is square km.
        /// Factor: ExtentOfOccurrence_B1Estimated, Id = 731.
        /// Has Probable, min and max values.
        /// Usage: Is used by criteria B1.
        /// </summary>
        public Double ExtentOfOccurrence
        {
            get { return _extentOfOccurrence; }
            set
            {
                if ((EXTENT_OF_OCCURRENCE_MIN <= value) &&
                    (value <= EXTENT_OF_OCCURRENCE_MAX))
                {
                    _extentOfOccurrence = value;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException(value.ToString() + " is not a valid value for extent of occurrence");
                }
            }
        }

        /// <summary>
        /// Handle extreme fluctuations.
        /// Unit:
        /// 0 = No extreme fluctuations.
        /// 1 = Probably extreme fluctuations.
        /// 2 = Extreme fluctuations.
        /// Factor: ExtremeFluctuations, Id = 718
        /// Usage: Is used by criteria B1C, B2C and C2B.
        /// </summary>
        public Int32 ExtremeFluctuations
        {
            get { return _extremeFluctuations; }
            set
            {
                if ((ExtremeFluctuationsMin <= value) &&
                    (value <= ExtremeFluctuationsMax))
                {
                    _extremeFluctuations = value;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException(value.ToString() + " is not a valid value for extreme fluctuations");
                }
            }
        }

        /// <summary>
        /// Max value for extreme fluctuations.
        /// Factor: ExtremeFluctuations, Id = 718
        /// </summary>
        public Int32 ExtremeFluctuationsMax
        {
            get
            {
                Factor factor;
                FactorField factorField;

                factor = FactorManager.GetFactor(FactorId.ExtremeFluctuations);
                factorField = factor.FactorDataType.Field1;
                return factorField.FactorFieldEnum.GetMaxKeyInt();
            }
        }

        /// <summary>
        /// Min value for extreme fluctuations.
        /// Factor: ExtremeFluctuations, Id = 718
        /// </summary>
        public Int32 ExtremeFluctuationsMin
        {
            get
            {
                Factor factor;
                FactorField factorField;

                factor = FactorManager.GetFactor(FactorId.ExtremeFluctuations);
                factorField = factor.FactorDataType.Field1;
                return factorField.FactorFieldEnum.GetMinKeyInt();
            }
        }


        /// <summary>
        /// Handle if generation length has been set.
        /// </summary>
        public Boolean HasGenerationLength
        {
            get { return !Double.IsNaN(_generationLength); }
            set
            {
                if (!value)
                {
                    // Reset value.
                    _generationLength = Double.NaN;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException("Use property GenerationLength if you want to set a value.");
                }
            }
        }

        /// <summary>
        /// Handle if area of occupancy has been set.
        /// </summary>
        public Boolean HasAreaOfOccupancy
        {
            get { return !Double.IsNaN(_areaOfOccupancy); }
            set
            {
                if (!value)
                {
                    // Reset value.
                    _areaOfOccupancy = Double.NaN;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException("Use property AreaOfOccupancy if you want to set a value.");
                }
            }
        }

        /// <summary>
        /// Handle if continuing decline has been set.
        /// </summary>
        public Boolean HasContinuingDecline
        {
            get { return _continuingDecline != Int32.MinValue; }
            set
            {
                if (!value)
                {
                    // Reset value.
                    _continuingDecline = Int32.MinValue;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException("Use property ContinuingDecline if you want to set a value.");
                }
            }
        }

        /// <summary>
        /// Handle if extent of occurrence has been set.
        /// </summary>
        public Boolean HasExtentOfOccurrence
        {
            get { return !Double.IsNaN(_extentOfOccurrence); }
            set
            {
                if (!value)
                {
                    // Reset value.
                    _extentOfOccurrence = Double.NaN;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException("Use property ExtentOfOccurrence if you want to set a value.");
                }
            }
        }

        /// <summary>
        /// Test if extreme fluctuations has been set.
        /// </summary>
        public Boolean HasExtremeFluctuations
        {
            get { return _extremeFluctuations != Int32.MinValue; }
            set
            {
                if (!value)
                {
                    // Reset value.
                    _extremeFluctuations = Int32.MinValue;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException("Use property ExtremeFluctuations if you want to set a value.");
                }
            }
        }

        /// <summary>
        /// Test if max proportion of local population has been set.
        /// </summary>
        public Boolean HasMaxProportionLocalPopulation
        {
            get { return !Double.IsNaN(_maxProportionLocalPopulation); }
            set
            {
                if (!value)
                {
                    // Reset value.
                    _maxProportionLocalPopulation = Double.NaN;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException("Use property MaxProportionLocalPopulation if you want to set a value.");
                }
            }
        }

        /// <summary>
        /// Test if max size of local population has been set.
        /// </summary>
        public Boolean HasMaxSizeLocalPopulation
        {
            get { return _maxSizeLocalPopulation != Int64.MinValue; }
            set
            {
                if (!value)
                {
                    _maxSizeLocalPopulation = Int64.MinValue;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException("Use property MaxSizeLocalPopulation if you want to set a value.");
                }
            }
        }

        /// <summary>
        /// Test if number of locations has been set.
        /// </summary>
        public Boolean HasNumberOfLocations
        {
            get { return _numberOfLocations != Int64.MinValue; }
            set
            {
                if (!value)
                {
                    _numberOfLocations = Int64.MinValue;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException("Use property NumberOfLocations if you want to set a value.");
                }
            }
        }

        /// <summary>
        /// Test if population size reduction A1 has been set.
        /// </summary>
        public Boolean HasPopulationReductionA1
        {
            get { return !Double.IsNaN(_populationReductionA1); }
            set
            {
                if (!value)
                {
                    _populationReductionA1 = Double.NaN;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException("Use property PopulationReductionA1 if you want to set a value.");
                }
            }
        }

        /// <summary>
        /// Test if population size reduction A2 has been set.
        /// </summary>
        public Boolean HasPopulationReductionA2
        {
            get { return !Double.IsNaN(_populationReductionA2); }
            set
            {
                if (!value)
                {
                    _populationReductionA2 = Double.NaN;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException("Use property PopulationReductionA2 if you want to set a value.");
                }
            }
        }

        /// <summary>
        /// Test if population size reduction A3 has been set.
        /// </summary>
        public Boolean HasPopulationReductionA3
        {
            get { return !Double.IsNaN(_populationReductionA3); }
            set
            {
                if (!value)
                {
                    _populationReductionA3 = Double.NaN;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException("Use property PopulationReductionA3 if you want to set a value.");
                }
            }
        }

        /// <summary>
        /// Test if population size reduction A4 has been set.
        /// </summary>
        public Boolean HasPopulationReductionA4
        {
            get { return !Double.IsNaN(_populationReductionA4); }
            set
            {
                if (!value)
                {
                    _populationReductionA4 = Double.NaN;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException("Use property PopulationReductionA4 if you want to set a value.");
                }
            }
        }

        /// <summary>
        /// Test if population size has been set.
        /// </summary>
        public Boolean HasPopulationSize
        {
            get { return _populationSize != Int64.MinValue; }
            set
            {
                if (!value)
                {
                    _populationSize = Int64.MinValue;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException("Use property PopulationSize if you want to set a value.");
                }
            }
        }

        /// <summary>
        /// Test if probability of extinction has been set.
        /// </summary>
        public Boolean HasProbabilityOfExtinction
        {
            get { return _probabilityOfExtinction != Int32.MinValue; }
            set
            {
                if (!value)
                {
                    _probabilityOfExtinction = Int32.MinValue;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException("Use property ProbabilityOfExtinction if you want to set a value.");
                }
            }
        }

        /// <summary>
        /// Test if severly fragmented has been set.
        /// </summary>
        public Boolean HasSeverlyFragmented
        {
            get { return _severlyFragmented != Int32.MinValue; }
            set
            {
                if (!value)
                {
                    _severlyFragmented = Int32.MinValue;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException("Use property SeverlyFragmented if you want to set a value.");
                }
            }
        }

        /// <summary>
        /// Test if very restricted area has been set.
        /// </summary>
        public Boolean HasVeryRestrictedArea
        {
            get { return _veryRestrictedArea != Int32.MinValue; }
            set
            {
                if (!value)
                {
                    _veryRestrictedArea = Int32.MinValue;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException("Use property VeryRestricedArea if you want to set a value.");
                }
            }
        }

        /// <summary>
        /// Test if criteria A1A is Fulfilled.
        /// Reduction in population. Direct observation.
        /// Factor: ReductionBasedOn_A1a, Id = 686,
        /// </summary>
        public Boolean IsCriteriaA1AFulfilled
        {
            get { return _isCriteriaA1AFulfilled; }
            set
            {
                _isCriteriaA1AFulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria A1B is Fulfilled.
        /// Reduction in population. Index of abundance.
        /// Factor: ReductionBasedOn_A1b, Id = 687,
        /// </summary>
        public Boolean IsCriteriaA1BFulfilled
        {
            get { return _isCriteriaA1BFulfilled; }
            set
            {
                _isCriteriaA1BFulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria A1C is Fulfilled.
        /// Reduction in population. Decline in geography.
        /// Factor: ReductionBasedOn_A1c, Id = 688,
        /// </summary>
        public Boolean IsCriteriaA1CFulfilled
        {
            get { return _isCriteriaA1CFulfilled; }
            set
            {
                _isCriteriaA1CFulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria A1D is Fulfilled.
        /// Reduction in population. Levels of exploitation.
        /// Factor: ReductionBasedOn_A1d, Id = 689,
        /// </summary>
        public Boolean IsCriteriaA1DFulfilled
        {
            get { return _isCriteriaA1DFulfilled; }
            set
            {
                _isCriteriaA1DFulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria A1E is Fulfilled.
        /// Reduction in population. Effects of other taxa.
        /// Factor: ReductionBasedOn_A1e, Id = 690,
        /// </summary>
        public Boolean IsCriteriaA1EFulfilled
        {
            get { return _isCriteriaA1EFulfilled; }
            set
            {
                _isCriteriaA1EFulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria A2A is Fulfilled.
        /// Reduction in population. Direct observation.
        /// Factor: ReductionBasedOn_A2a, Id = 694,
        /// </summary>
        public Boolean IsCriteriaA2AFulfilled
        {
            get { return _isCriteriaA2AFulfilled; }
            set
            {
                _isCriteriaA2AFulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria A2B is Fulfilled.
        /// Reduction in population. Index of abundance.
        /// Factor: ReductionBasedOn_A2b, Id = 695,
        /// </summary>
        public Boolean IsCriteriaA2BFulfilled
        {
            get { return _isCriteriaA2BFulfilled; }
            set
            {
                _isCriteriaA2BFulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria A2C is Fulfilled.
        /// Reduction in population. Decline in geography.
        /// Factor: ReductionBasedOn_A2c, Id = 696,
        /// </summary>
        public Boolean IsCriteriaA2CFulfilled
        {
            get { return _isCriteriaA2CFulfilled; }
            set
            {
                _isCriteriaA2CFulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria A2D is Fulfilled.
        /// Reduction in population. Levels of exploitation.
        /// Factor: ReductionBasedOn_A2d, Id = 697,
        /// </summary>
        public Boolean IsCriteriaA2DFulfilled
        {
            get { return _isCriteriaA2DFulfilled; }
            set
            {
                _isCriteriaA2DFulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria A2E is Fulfilled.
        /// Reduction in population. Effects of other taxa.
        /// Factor: ReductionBasedOn_A2e, Id = 698,
        /// </summary>
        public Boolean IsCriteriaA2EFulfilled
        {
            get { return _isCriteriaA2EFulfilled; }
            set
            {
                _isCriteriaA2EFulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria A2B is Fulfilled.
        /// Reduction in population. Index of abundance.
        /// Factor: ReductionBasedOn_A3b, Id = 702,
        /// </summary>
        public Boolean IsCriteriaA3BFulfilled
        {
            get { return _isCriteriaA3BFulfilled; }
            set
            {
                _isCriteriaA3BFulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria A2C is Fulfilled.
        /// Reduction in population. Decline in geography.
        /// Factor: ReductionBasedOn_A3c, Id = 703,
        /// </summary>
        public Boolean IsCriteriaA3CFulfilled
        {
            get { return _isCriteriaA3CFulfilled; }
            set
            {
                _isCriteriaA3CFulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria A3D is Fulfilled.
        /// Reduction in population. Levels of exploitation.
        /// Factor: ReductionBasedOn_A3d, Id = 704,
        /// </summary>
        public Boolean IsCriteriaA3DFulfilled
        {
            get { return _isCriteriaA3DFulfilled; }
            set
            {
                _isCriteriaA3DFulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria A3E is Fulfilled.
        /// Reduction in population. Effects of other taxa.
        /// Factor: ReductionBasedOn_A3e, Id = 705,
        /// </summary>
        public Boolean IsCriteriaA3EFulfilled
        {
            get { return _isCriteriaA3EFulfilled; }
            set
            {
                _isCriteriaA3EFulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria A4A is Fulfilled.
        /// Reduction in population. Direct observation.
        /// Factor: ReductionBasedOn_A4a, Id = 709,
        /// </summary>
        public Boolean IsCriteriaA4AFulfilled
        {
            get { return _isCriteriaA4AFulfilled; }
            set
            {
                _isCriteriaA4AFulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria A4B is Fulfilled.
        /// Reduction in population. Index of abundance.
        /// Factor: ReductionBasedOn_A4b, Id = 710,
        /// </summary>
        public Boolean IsCriteriaA4BFulfilled
        {
            get { return _isCriteriaA4BFulfilled; }
            set
            {
                _isCriteriaA4BFulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria A4C is Fulfilled.
        /// Reduction in population. Decline in geography.
        /// Factor: ReductionBasedOn_A4c, Id = 711,
        /// </summary>
        public Boolean IsCriteriaA4CFulfilled
        {
            get { return _isCriteriaA4CFulfilled; }
            set
            {
                _isCriteriaA4CFulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria A4D is Fulfilled.
        /// Reduction in population. Levels of exploitation.
        /// Factor: ReductionBasedOn_A4d, Id = 712,
        /// </summary>
        public Boolean IsCriteriaA4DFulfilled
        {
            get { return _isCriteriaA4DFulfilled; }
            set
            {
                _isCriteriaA4DFulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria A4E is Fulfilled.
        /// Reduction in population. Effects of other taxa.
        /// Factor: ReductionBasedOn_A4e, Id = 713,
        /// </summary>
        public Boolean IsCriteriaA4EFulfilled
        {
            get { return _isCriteriaA4EFulfilled; }
            set
            {
                _isCriteriaA4EFulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria BB1 is Fulfilled.
        /// Continuing decline in extent of occurence.
        /// Factor: ContinuingDeclineBasedOn_Bbi, Id = 673,
        /// </summary>
        public Boolean IsCriteriaBB1Fulfilled
        {
            get { return _isCriteriaBB1Fulfilled; }
            set
            {
                _isCriteriaBB1Fulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria BB2 is Fulfilled.
        /// Continuing decline in area of occupancy.
        /// Factor: ContinuingDeclineBasedOn_Bbii, Id = 674,
        /// </summary>
        public Boolean IsCriteriaBB2Fulfilled
        {
            get { return _isCriteriaBB2Fulfilled; }
            set
            {
                _isCriteriaBB2Fulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria BB3 is Fulfilled.
        /// Continuing decline in area, extent and/or quiality of habitat.
        /// Factor: ContinuingDeclineBasedOn_Bbiii, Id = 675,
        /// </summary>
        public Boolean IsCriteriaBB3Fulfilled
        {
            get { return _isCriteriaBB3Fulfilled; }
            set
            {
                _isCriteriaBB3Fulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria BB4 is Fulfilled.
        /// Continuing decline in number of locations or subpopulations.
        /// Factor: ContinuingDeclineBasedOn_Bbiv, Id = 676,
        /// </summary>
        public Boolean IsCriteriaBB4Fulfilled
        {
            get { return _isCriteriaBB4Fulfilled; }
            set
            {
                _isCriteriaBB4Fulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria BB5 is Fulfilled.
        /// Continuing decline in number of mature individuals.
        /// Factor: ContinuingDeclineBasedOn_Bbv, Id = 677,
        /// </summary>
        public Boolean IsCriteriaBB5Fulfilled
        {
            get { return _isCriteriaBB5Fulfilled; }
            set
            {
                _isCriteriaBB5Fulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria BB is Fulfilled.
        /// Continuing decline.
        /// </summary>
        public Boolean IsCriteriaBBFulfilled
        {
            get
            {
                return (HasContinuingDecline &&
                        (ContinuingDecline >= CRITERIA_BB_1_LIMIT) &&
                        (IsCriteriaBB1Fulfilled ||
                         IsCriteriaBB2Fulfilled ||
                         IsCriteriaBB3Fulfilled ||
                         IsCriteriaBB4Fulfilled ||
                         IsCriteriaBB5Fulfilled));
            }
        }

        /// <summary>
        /// Test if criteria BC1 is Fulfilled.
        /// Extreme fluctuations in extent of occurrence.
        /// Factor: ExtremeFluctuationsIn_Bci, Id = 721,
        /// </summary>
        public Boolean IsCriteriaBC1Fulfilled
        {
            get { return _isCriteriaBC1Fulfilled; }
            set
            {
                _isCriteriaBC1Fulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria BC2 is Fulfilled.
        /// Extreme fluctuations in area of occupancy.
        /// Factor: ExtremeFluctuationsIn_Bcii, Id = 722,
        /// </summary>
        public Boolean IsCriteriaBC2Fulfilled
        {
            get { return _isCriteriaBC2Fulfilled; }
            set
            {
                _isCriteriaBC2Fulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria BC3 is Fulfilled.
        /// Extreme fluctuations in number of locations or subpopulations.
        /// Factor: ExtremeFluctuationsIn_Bciii, Id = 723,
        /// </summary>
        public Boolean IsCriteriaBC3Fulfilled
        {
            get { return _isCriteriaBC3Fulfilled; }
            set
            {
                _isCriteriaBC3Fulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria BC4 is Fulfilled.
        /// Extreme fluctuations in number of mature individuals.
        /// Factor: ExtremeFluctuationsIn_Bciv, Id = 724,
        /// </summary>
        public Boolean IsCriteriaBC4Fulfilled
        {
            get { return _isCriteriaBC4Fulfilled; }
            set
            {
                _isCriteriaBC4Fulfilled = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Test if criteria BC is Fulfilled.
        /// Extreme fluctuations.
        /// </summary>
        public Boolean IsCriteriaBCFulfilled
        {
            get
            {
                return (HasExtremeFluctuations &&
                        (ExtremeFluctuations >= CRITERIA_BC_1_LIMIT) &&
                        (IsCriteriaBC1Fulfilled ||
                         IsCriteriaBC2Fulfilled ||
                         IsCriteriaBC3Fulfilled ||
                         IsCriteriaBC4Fulfilled));
            }
        }

        /// <summary>
        /// Flag that indicates if Criteria is calculated.
        /// This flag is used to avoid unnecessary calculations.
        /// </summary>
        public Boolean IsCriteriaCalculated 
        {
            get { return _isCriteriaCalculated; }
            set
            {
                if (value != _isCriteriaCalculated)
                {
                    _criteria = null;
                    _isCriteriaCalculated = value;
                    if (_isCriteriaCalculated)
                    {
                        SetRedListValues();
                    }
                }
            }
        }

        /// <summary>
        /// Test if initialization is being performed.
        /// </summary>
        public Boolean IsInInit
        {
            get { return _isInInit; }
        }

        /// <summary>
        /// Handle max proportion local population.
        /// Unit is max number of per cent in one subpopulation
        /// of the total population, e.g. 90% individuals
        /// in one subpopulation has the value 90.0
        /// Factor: MaxProportionLocalPopulation, Id = 717
        /// Has Probable, min and max values.
        /// </summary>
        public Double MaxProportionLocalPopulation
        {
            get { return _maxProportionLocalPopulation; }
            set
            {
                if ((MAX_PROPORTION_LOCAL_POPULATION_MIN <= value) &&
                    (value <= MAX_PROPORTION_LOCAL_POPULATION_MAX))
                {
                    _maxProportionLocalPopulation = value;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException(value.ToString() + " is not a valid value for max proportion local population");
                }
            }
        }

        /// <summary>
        /// Handle max size of local population.
        /// Unit: Max number of individuals in one population.
        /// Factor: MaxSizeLocalPopulation, Id = 716
        /// Has Probable, min and max values.
        /// </summary>
        public Int64 MaxSizeLocalPopulation
        {
            get { return _maxSizeLocalPopulation; }
            set
            {
                if ((MAX_SIZE_LOCAL_POPULATION_MIN <= value) &&
                    (value <= MAX_SIZE_LOCAL_POPULATION_MAX))
                {
                    _maxSizeLocalPopulation = value;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException(value.ToString() + " is not a valid value for max size of local population");
                }
            }
        }

        /// <summary>
        /// Handle number of locations.
        /// Unit is location count.
        /// Factor: NumberOfLocations, Id = 727,
        /// Has Probable, min and max values.
        /// </summary>
        public Int64 NumberOfLocations
        {
            get { return _numberOfLocations; }
            set
            {
                if ((NUMBER_OF_LOCATIONS_MIN <= value) &&
                    (value <= NUMBER_OF_LOCATIONS_MAX))
                {
                    _numberOfLocations = value;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException(value.ToString() + " is not a valid value for number of locations");
                }
            }
        }

        /// <summary>
        /// Handle population size reduction A1.
        /// Unit is number of per cent in reduction to
        /// previouse population size,
        /// e.g. 90% reduction has the value 90.0
        /// Factor: Reduction_A1, Id = 684,
        /// Has Probable, min and max values.
        /// </summary>
        public Double PopulationReductionA1
        {
            get { return _populationReductionA1; }
            set
            {
                if ((POPULATION_REDUCTION_A1_MIN <= value) &&
                    (value <= POPULATION_REDUCTION_A1_MAX))
                {
                    _populationReductionA1 = value;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException(value.ToString() + " is not a valid value for population size reduction A1");
                }
            }
        }

        /// <summary>
        /// Handle population size reduction A2.
        /// Unit is number of per cent in reduction to
        /// previouse population size,
        /// e.g. 90% reduction has the value 90.0
        /// Factor: Reduction_A2, Id = 692,
        /// Has Probable, min and max values.
        /// </summary>
        public Double PopulationReductionA2
        {
            get { return _populationReductionA2; }
            set
            {
                if ((POPULATION_REDUCTION_A2_MIN <= value) &&
                    (value <= POPULATION_REDUCTION_A2_MAX))
                {
                    _populationReductionA2 = value;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException(value.ToString() + " is not a valid value for population size reduction A2");
                }
            }
        }

        /// <summary>
        /// Handle population size reduction A3.
        /// Unit is number of per cent in reduction to
        /// previouse population size,
        /// e.g. 90% reduction has the value 90.0
        /// Factor: Reduction_A3, Id = 700,
        /// Has Probable, min and max values.
        /// </summary>
        public Double PopulationReductionA3
        {
            get { return _populationReductionA3; }
            set
            {
                if ((POPULATION_REDUCTION_A3_MIN <= value) &&
                    (value <= POPULATION_REDUCTION_A3_MAX))
                {
                    _populationReductionA3 = value;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException(value.ToString() + " is not a valid value for population size reduction A3");
                }
            }
        }

        /// <summary>
        /// Handle population size reduction A4.
        /// Unit is number of per cent in reduction to
        /// previouse population size,
        /// e.g. 90% reduction has the value 90.0
        /// Factor: Reduction_A4, Id = 707,
        /// Has Probable, min and max values.
        /// </summary>
        public Double PopulationReductionA4
        {
            get { return _populationReductionA4; }
            set
            {
                if ((POPULATION_REDUCTION_A4_MIN <= value) &&
                    (value <= POPULATION_REDUCTION_A4_MAX))
                {
                    _populationReductionA4 = value;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException(value.ToString() + " is not a valid value for population size reduction A4");
                }
            }
        }

        /// <summary>
        /// Handle population size.
        /// Unit. Number of mature individuals.
        /// Factor: PopulationSize_Total, Id = 715
        /// Has Probable, min and max values.
        /// </summary>
        public Int64 PopulationSize
        {
            get { return _populationSize; }
            set
            {
                if ((POPULATION_SIZE_MIN <= value) &&
                    (value <= POPULATION_SIZE_MAX))
                {
                    _populationSize = value;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException(value.ToString() + " is not a valid value population size");
                }
            }
        }

        
        /// <summary>
        /// Handle probability of extinction.
        /// Unit.
        ///  0 = Probability of extinction is at least 50% within 10 years or 3 generations.
        ///  1 = Probability of extinction is at least 20% within 20 years or 5 generations.
        ///  2 = Probability of extinction is at least 10% within 100 years.
        ///  3 = Probability of extinction is at least 5% within 100 years.
        ///  4 = Probability of extinction is at lower than 5% within 100 years.
        /// Factor: ProbabilityOfExtinction, Id = 736.
        /// Has Probable, min and max values.
        /// </summary>
        public Int32 ProbabilityOfExtinction
        {
            get { return _probabilityOfExtinction; }
            set
            {
                _probabilityOfExtinction = value;
                SetRedListValues();
            }
        }

        /// <summary>
        /// Max value for probability of extinction.
        /// Factor: ProbabilityOfExtinction, Id = 736.
        /// </summary>
        public Int32 ProbabilityOfExtinctionMax
        {
            get
            {
                Factor factor;
                FactorField factorField;

                factor = FactorManager.GetFactor(FactorId.ProbabilityOfExtinction);
                factorField = factor.FactorDataType.Field1;
                return factorField.FactorFieldEnum.GetMaxKeyInt();
            }
        }

        /// <summary>
        /// Min value for probability of extinction.
        /// Factor: ProbabilityOfExtinction, Id = 736.
        /// </summary>
        public Int32 ProbabilityOfExtinctionMin
        {
            get
            {
                Factor factor;
                FactorField factorField;

                factor = FactorManager.GetFactor(FactorId.ProbabilityOfExtinction);
                factorField = factor.FactorDataType.Field1;
                return factorField.FactorFieldEnum.GetMinKeyInt();
            }
        }

        /// <summary>
        /// Handle severly fragmented.
        /// Unit:
        /// 0 = Not severly fragmented.
        /// 1 = Probably severly fragmented.
        /// 2 = Populationen is severly fragmented.
        /// Factor: SeverelyFragmented, Id = 726,
        /// </summary>
        public Int32 SeverlyFragmented
        {
            get { return _severlyFragmented; }
            set
            {
                if ((SeverlyFragmentedMin <= value) &&
                    (value <= SeverlyFragmentedMax))
                {
                    _severlyFragmented = value;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException(value.ToString() + " is not a valid value for severly fragmented");
                }
            }
        }

        /// <summary>
        /// Max value for severly fragmented.
        /// Factor: SeverelyFragmented, Id = 726,
        /// </summary>
        public Int32 SeverlyFragmentedMax
        {
            get
            {
                Factor factor;
                FactorField factorField;

                factor = FactorManager.GetFactor(FactorId.SeverelyFragmented);
                factorField = factor.FactorDataType.Field1;
                return factorField.FactorFieldEnum.GetMaxKeyInt();
            }
        }

        /// <summary>
        /// Min value for severly fragmented.
        /// Factor: SeverelyFragmented, Id = 726,
        /// </summary>
        public Int32 SeverlyFragmentedMin
        {
            get
            {
                Factor factor;
                FactorField factorField;

                factor = FactorManager.GetFactor(FactorId.SeverelyFragmented);
                factorField = factor.FactorDataType.Field1;
                return factorField.FactorFieldEnum.GetMinKeyInt();
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
        public Int32 VeryRestrictedArea
        {
            get { return _veryRestrictedArea; }
            set
            {
                if ((VeryRestrictedAreaMin <= value) &&
                    (value <= VeryRestrictedAreaMax))
                {
                    _veryRestrictedArea = value;
                    SetRedListValues();
                }
                else
                {
                    throw new ArgumentException(value.ToString() + " is not a valid value for very restricted area");
                }
            }
        }

        /// <summary>
        /// Max value for very restricted area.
        /// Factor: VeryRestrictedArea_D2VU, Id = 728.
        /// </summary>
        public Int32 VeryRestrictedAreaMax
        {
            get
            {
                Factor factor;
                FactorField factorField;

                factor = FactorManager.GetFactor(FactorId.VeryRestrictedArea_D2VU);
                factorField = factor.FactorDataType.Field2;
                return factorField.FactorFieldEnum.GetMaxKeyInt();
            }
        }

        /// <summary>
        /// Min value for very restricted area.
        /// Factor: VeryRestrictedArea_D2VU, Id = 728.
        /// </summary>
        public Int32 VeryRestrictedAreaMin
        {
            get
            {
                Factor factor;
                FactorField factorField;

                factor = FactorManager.GetFactor(FactorId.VeryRestrictedArea_D2VU);
                factorField = factor.FactorDataType.Field2;
                return factorField.FactorFieldEnum.GetMinKeyInt();
            }
        }

        /// <summary>
        /// Get count for criteria B (used in both B1 and B2).
        /// </summary>
        /// <param name="category">Category to use when getting criteria count.</param>
        /// <returns>Criteria count. </returns>
        public Int32 GetCriteriaBCount(RedListCategory category)
        {
            Int32 count = 0;

            count = GetCriteriaBACount(category) +
                    GetCriteriaBBCount() +
                    GetCriteriaBCCount();

            return count;
        }

        /// <summary>
        /// Get count for criteria BA (used in both B1A and B2A).
        /// </summary>
        /// <param name="category">Category to use when getting criteria count.</param>
        /// <returns>Criteria count. </returns>
        private Int32 GetCriteriaBACount(RedListCategory category)
        {
            Int32 count = 0;

            if (IsCriteriaBAFulfilled(category))
            {
                if (HasSeverlyFragmented)
                {
                    if (SeverlyFragmented >= CRITERIA_BA_2_LIMIT)
                    {
                        count = 2;
                    }
                    else if (SeverlyFragmented >= CRITERIA_BA_1_LIMIT)
                    {
                        count = 1;
                    }
                }
                switch (category)
                {
                    case RedListCategory.CR:
                        if (HasNumberOfLocations &&
                            (NumberOfLocations <= CRITERIA_BA_CR_LIMIT))
                        {
                            count = 2;
                        }
                        break;
                    case RedListCategory.EN:
                        if (HasNumberOfLocations &&
                            (NumberOfLocations < CRITERIA_BA_EN_LIMIT))
                        {
                            count = 2;
                        }
                        break;
                    case RedListCategory.VU:
                        if (HasNumberOfLocations &&
                            (NumberOfLocations < CRITERIA_BA_VU_LIMIT))
                        {
                            count = 2;
                        }
                        break;
                    case RedListCategory.NT:
                        if (HasNumberOfLocations &&
                            (NumberOfLocations < CRITERIA_BA_NT_LIMIT))
                        {
                            count = 2;
                        }
                        break;
                    default:
                        throw new Exception("GetCriteriaBACount does not handle category " + category);
                }
            }

            return count;
        }

        /// <summary>
        /// Get count for criteria BB (used in both B1B and B2B).
        /// </summary>
        /// <returns>Criteria count. </returns>
        private Int32 GetCriteriaBBCount()
        {
            Int32 count = 0;

            if (IsCriteriaBBFulfilled)
            {
                if (HasContinuingDecline)
                {
                    if (ContinuingDecline >= CRITERIA_BB_2_LIMIT)
                    {
                        count = 2;
                    }
                    else if (ContinuingDecline >= CRITERIA_BB_1_LIMIT)
                    {
                        count = 1;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Get count for criteria BC (used in both B1C and B2C).
        /// </summary>
        /// <returns>Criteria count. </returns>
        private Int32 GetCriteriaBCCount()
        {
            Int32 count = 0;

            if (IsCriteriaBCFulfilled)
            {
                if (HasExtremeFluctuations)
                {
                    if (ExtremeFluctuations >= CRITERIA_BC_2_LIMIT)
                    {
                        count = 2;
                    }
                    else if (ExtremeFluctuations >= CRITERIA_BC_1_LIMIT)
                    {
                        count = 1;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Test if criteria is Fulfilled.
        /// Geographic range.
        /// </summary>
        /// <param name="category">Category to use when testing criteria.</param>
        /// <returns>True if criteria is Fulfilled. </returns>
        public Boolean IsCriteriaFulfilled(RedListCategory category)
        {
            if (category == RedListCategory.RE)
            {
                return IsRegionallyExtinct();
            }
            else
            {
                return (IsCriteriaAFulfilled(category) ||
                        IsCriteriaBFulfilled(category) ||
                        IsCriteriaCFulfilled(category) ||
                        IsCriteriaDFulfilled(category) ||
                        IsCriteriaEFulfilled(category));
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
                    SetRedListValues();
                }
            }
        }

        /// <summary>
        /// Test if any of the criterias for regionally extinct
        /// is fulfilled.
        /// </summary>
        /// <returns>True if any of the criterias for regionally extinct is fulfilled.</returns>
        private Boolean IsRegionallyExtinct()
        {
            return ((HasAreaOfOccupancy &&
                     (AreaOfOccupancy <= CRITERIA_AREA_OF_OCCUPANCY_RE_LIMIT)) ||
                    (HasExtentOfOccurrence &&
                     (ExtentOfOccurrence <= CRITERIA_EXTENT_OF_OCCURRENCE_RE_LIMIT)) ||
                    (HasNumberOfLocations &&
                     (NumberOfLocations <= CRITERIA_NUMBER_OF_LOCATIONS_RE_LIMIT)) ||
                    (HasPopulationSize &&
                     (PopulationSize <= CRITERIA_POPULATION_SIZE_RE_LIMIT)));
        }

        /// <summary>
        /// Call this method before first initialization
        /// of red list values.
        /// RedListValues are not updated until initialization
        /// has finished with a call to method InitEnd.
        /// </summary>
        public void InitBegin()
        {
            _isInInit = true;
        }

        /// <summary>
        /// Call this method after initialization
        /// of red list values.
        /// Calculation is turned on and performed in this call.
        /// </summary>
        public void InitEnd()
        {
            _isInInit = false;
            SetRedListValues();
        }

        /// <summary>
        /// Test if criteria A1 is Fulfilled.
        /// Reduction in population A1.
        /// </summary>
        /// <param name="category">Category to use when testing criteria.</param>
        /// <returns>True if criteria A1 is Fulfilled. </returns>
        protected Boolean IsCriteriaA1Fulfilled(RedListCategory category)
        {
            switch (category)
            {
                case RedListCategory.CR:
                    return (HasPopulationReductionA1 &&
                            (PopulationReductionA1 >= CRITERIA_A1_CR_LIMIT) &&
                            (IsCriteriaA1AFulfilled ||
                             IsCriteriaA1BFulfilled ||
                             IsCriteriaA1CFulfilled ||
                             IsCriteriaA1DFulfilled ||
                             IsCriteriaA1EFulfilled));
                case RedListCategory.EN:
                    return (HasPopulationReductionA1 &&
                            (PopulationReductionA1 >= CRITERIA_A1_EN_LIMIT) &&
                            (IsCriteriaA1AFulfilled ||
                             IsCriteriaA1BFulfilled ||
                             IsCriteriaA1CFulfilled ||
                             IsCriteriaA1DFulfilled ||
                             IsCriteriaA1EFulfilled));
                case RedListCategory.VU:
                    return (HasPopulationReductionA1 &&
                            (PopulationReductionA1 >= CRITERIA_A1_VU_LIMIT) &&
                            (IsCriteriaA1AFulfilled ||
                             IsCriteriaA1BFulfilled ||
                             IsCriteriaA1CFulfilled ||
                             IsCriteriaA1DFulfilled ||
                             IsCriteriaA1EFulfilled));
                case RedListCategory.NT:
                    return (HasPopulationReductionA1 &&
                            (PopulationReductionA1 >= CRITERIA_A1_NT_LIMIT) &&
                            (IsCriteriaA1AFulfilled ||
                             IsCriteriaA1BFulfilled ||
                             IsCriteriaA1CFulfilled ||
                             IsCriteriaA1DFulfilled ||
                             IsCriteriaA1EFulfilled));
                default:
                    return false;
                    //throw new Exception("Criteria A1 does not handle category " + category);
            }
        }

        /// <summary>
        /// Test if criteria A2 is Fulfilled.
        /// Reduction in population A2.
        /// </summary>
        /// <param name="category">Category to use when testing criteria.</param>
        /// <returns>True if criteria A2 is Fulfilled. </returns>
        protected Boolean IsCriteriaA2Fulfilled(RedListCategory category)
        {
            switch (category)
            {
                case RedListCategory.CR:
                    return (HasPopulationReductionA2 &&
                            (PopulationReductionA2 >= CRITERIA_A2_CR_LIMIT) &&
                            (IsCriteriaA2AFulfilled ||
                             IsCriteriaA2BFulfilled ||
                             IsCriteriaA2CFulfilled ||
                             IsCriteriaA2DFulfilled ||
                             IsCriteriaA2EFulfilled));
                case RedListCategory.EN:
                    return (HasPopulationReductionA2 &&
                            (PopulationReductionA2 >= CRITERIA_A2_EN_LIMIT) &&
                            (IsCriteriaA2AFulfilled ||
                             IsCriteriaA2BFulfilled ||
                             IsCriteriaA2CFulfilled ||
                             IsCriteriaA2DFulfilled ||
                             IsCriteriaA2EFulfilled));
                case RedListCategory.VU:
                    return (HasPopulationReductionA2 &&
                            (PopulationReductionA2 >= CRITERIA_A2_VU_LIMIT) &&
                            (IsCriteriaA2AFulfilled ||
                             IsCriteriaA2BFulfilled ||
                             IsCriteriaA2CFulfilled ||
                             IsCriteriaA2DFulfilled ||
                             IsCriteriaA2EFulfilled));
                case RedListCategory.NT:
                    return (HasPopulationReductionA2 &&
                            (PopulationReductionA2 >= CRITERIA_A2_NT_LIMIT) &&
                            (IsCriteriaA2AFulfilled ||
                             IsCriteriaA2BFulfilled ||
                             IsCriteriaA2CFulfilled ||
                             IsCriteriaA2DFulfilled ||
                             IsCriteriaA2EFulfilled));
                default:
                    return false;
                    //throw new Exception("Criteria A2 does not handle category " + category);
            }
        }

        /// <summary>
        /// Test if criteria A3 is Fulfilled.
        /// Reduction in population A3.
        /// </summary>
        /// <param name="category">Category to use when testing criteria.</param>
        /// <returns>True if criteria A3 is Fulfilled. </returns>
        protected Boolean IsCriteriaA3Fulfilled(RedListCategory category)
        {
            switch (category)
            {
                case RedListCategory.CR:
                    return (HasPopulationReductionA3 &&
                            (PopulationReductionA3 >= CRITERIA_A3_CR_LIMIT) &&
                            (IsCriteriaA3BFulfilled ||
                             IsCriteriaA3CFulfilled ||
                             IsCriteriaA3DFulfilled ||
                             IsCriteriaA3EFulfilled));
                case RedListCategory.EN:
                    return (HasPopulationReductionA3 &&
                            (PopulationReductionA3 >= CRITERIA_A3_EN_LIMIT) &&
                            (IsCriteriaA3BFulfilled ||
                             IsCriteriaA3CFulfilled ||
                             IsCriteriaA3DFulfilled ||
                             IsCriteriaA3EFulfilled));
                case RedListCategory.VU:
                    return (HasPopulationReductionA3 &&
                            (PopulationReductionA3 >= CRITERIA_A3_VU_LIMIT) &&
                            (IsCriteriaA3BFulfilled ||
                             IsCriteriaA3CFulfilled ||
                             IsCriteriaA3DFulfilled ||
                             IsCriteriaA3EFulfilled));
                case RedListCategory.NT:
                    return (HasPopulationReductionA3 &&
                            (PopulationReductionA3 >= CRITERIA_A3_NT_LIMIT) &&
                            (IsCriteriaA3BFulfilled ||
                             IsCriteriaA3CFulfilled ||
                             IsCriteriaA3DFulfilled ||
                             IsCriteriaA3EFulfilled));
                default:
                    return false;
                    //throw new Exception("Criteria A3 does not handle category " + category);
            }
        }

        /// <summary>
        /// Test if criteria A4 is Fulfilled.
        /// Reduction in population A4.
        /// </summary>
        /// <param name="category">Category to use when testing criteria.</param>
        /// <returns>True if criteria A4 is Fulfilled. </returns>
        protected Boolean IsCriteriaA4Fulfilled(RedListCategory category)
        {
            switch (category)
            {
                case RedListCategory.CR:
                    return (HasPopulationReductionA4 &&
                            (PopulationReductionA4 >= CRITERIA_A4_CR_LIMIT) &&
                            (IsCriteriaA4AFulfilled ||
                             IsCriteriaA4BFulfilled ||
                             IsCriteriaA4CFulfilled ||
                             IsCriteriaA4DFulfilled ||
                             IsCriteriaA4EFulfilled));
                case RedListCategory.EN:
                    return (HasPopulationReductionA4 &&
                            (PopulationReductionA4 >= CRITERIA_A4_EN_LIMIT) &&
                            (IsCriteriaA4AFulfilled ||
                             IsCriteriaA4BFulfilled ||
                             IsCriteriaA4CFulfilled ||
                             IsCriteriaA4DFulfilled ||
                             IsCriteriaA4EFulfilled));
                case RedListCategory.VU:
                    return (HasPopulationReductionA4 &&
                            (PopulationReductionA4 >= CRITERIA_A4_VU_LIMIT) &&
                            (IsCriteriaA4AFulfilled ||
                             IsCriteriaA4BFulfilled ||
                             IsCriteriaA4CFulfilled ||
                             IsCriteriaA4DFulfilled ||
                             IsCriteriaA4EFulfilled));
                case RedListCategory.NT:
                    return (HasPopulationReductionA4 &&
                            (PopulationReductionA4 >= CRITERIA_A4_NT_LIMIT) &&
                            (IsCriteriaA4AFulfilled ||
                             IsCriteriaA4BFulfilled ||
                             IsCriteriaA4CFulfilled ||
                             IsCriteriaA4DFulfilled ||
                             IsCriteriaA4EFulfilled));
                default:
                    return false;
                    //throw new Exception("Criteria A4 does not handle category " + category);
            }
        }

        /// <summary>
        /// Test if criteria A is Fulfilled.
        /// Reduction in population A.
        /// </summary>
        /// <param name="category">Category to use when testing criteria.</param>
        /// <returns>True if criteria A is Fulfilled. </returns>
        public Boolean IsCriteriaAFulfilled(RedListCategory category)
        {
            return (IsCriteriaA1Fulfilled(category) ||
                    IsCriteriaA2Fulfilled(category) ||
                    IsCriteriaA3Fulfilled(category) ||
                    IsCriteriaA4Fulfilled(category));
        }

        /// <summary>
        /// Test if criteria B is Fulfilled.
        /// Geographic range.
        /// </summary>
        /// <param name="category">Category to use when testing criteria.</param>
        /// <returns>True if criteria B is Fulfilled. </returns>
        public Boolean IsCriteriaBFulfilled(RedListCategory category)
        {
            return (IsCriteriaB1Fulfilled(category) ||
                    IsCriteriaB2Fulfilled(category));
        }

        /// <summary>
        /// Test if criteria B1 is Fulfilled.
        /// Extent of occurrence.
        /// </summary>
        /// <param name="category">Category to use when testing criteria.</param>
        /// <returns>True if criteria B1 is Fulfilled. </returns>
        public Boolean IsCriteriaB1Fulfilled(RedListCategory category)
        {
            Int32 criteriaCount, criteriaENCount;

            criteriaCount = GetCriteriaBCount(category);
            switch (category)
            {
                case RedListCategory.CR:
                    return (HasExtentOfOccurrence &&
                            (ExtentOfOccurrence < CRITERIA_B1_CR_LIMIT) &&
                            (criteriaCount >= 4));
                case RedListCategory.EN:
                    return (HasExtentOfOccurrence &&
                            (((ExtentOfOccurrence < CRITERIA_B1_EN_LIMIT) &&
                              (criteriaCount >= 4)) ||
                             ((ExtentOfOccurrence < CRITERIA_B1_CR_LIMIT) &&
                              (criteriaCount >= 3))));
                case RedListCategory.VU:
                    return (HasExtentOfOccurrence &&
                            (((ExtentOfOccurrence < CRITERIA_B1_VU_LIMIT) &&
                              (criteriaCount >= 4)) ||
                             ((ExtentOfOccurrence < CRITERIA_B1_EN_LIMIT) &&
                              (criteriaCount >= 3))));
                case RedListCategory.NT:
                    criteriaENCount = GetCriteriaBCount(RedListCategory.EN);
                    return (HasExtentOfOccurrence &&
                            (((ExtentOfOccurrence < CRITERIA_B1_NT_LIMIT) &&
                              (criteriaCount >= 4)) ||
                             ((ExtentOfOccurrence < CRITERIA_B1_VU_LIMIT) &&
                              (criteriaCount >= 3)) ||
                             ((ExtentOfOccurrence < CRITERIA_B1_EN_LIMIT) &&
                              (criteriaENCount >= 2))));
                default:
                    return false;
                    //throw new Exception("Criteria B1 does not handle category " + category);
            }
        }

        /// <summary>
        /// Test if criteria B2 is Fulfilled.
        /// Area of occupancy.
        /// </summary>
        /// <param name="category">Category to use when testing criteria.</param>
        /// <returns>True if criteria B2 is Fulfilled. </returns>
        public Boolean IsCriteriaB2Fulfilled(RedListCategory category)
        {
            Int32 criteriaCount, criteriaENCount;

            criteriaCount = GetCriteriaBCount(category);
            switch (category)
            {
                case RedListCategory.CR:
                    return (HasAreaOfOccupancy &&
                            (AreaOfOccupancy < CRITERIA_B2_CR_LIMIT) &&
                            (criteriaCount >= 4));
                case RedListCategory.EN:
                    return (HasAreaOfOccupancy &&
                            (((AreaOfOccupancy < CRITERIA_B2_EN_LIMIT) &&
                              (criteriaCount >= 4)) ||
                             ((AreaOfOccupancy < CRITERIA_B2_CR_LIMIT) &&
                              (criteriaCount >= 3))));
                case RedListCategory.VU:
                    return (HasAreaOfOccupancy &&
                            (((AreaOfOccupancy < CRITERIA_B2_VU_LIMIT) &&
                              (criteriaCount >= 4)) ||
                             ((AreaOfOccupancy < CRITERIA_B2_EN_LIMIT) &&
                              (criteriaCount >= 3))));
                case RedListCategory.NT:
                    criteriaENCount = GetCriteriaBCount(RedListCategory.EN);
                    return (HasAreaOfOccupancy &&
                            (((AreaOfOccupancy < CRITERIA_B2_NT_LIMIT) &&
                              (criteriaCount >= 4)) ||
                             ((AreaOfOccupancy < CRITERIA_B2_VU_LIMIT) &&
                              (criteriaCount >= 3)) ||
                             ((AreaOfOccupancy < CRITERIA_B2_EN_LIMIT) &&
                              (criteriaENCount >= 2))));
                default:
                    return false;
                    //throw new Exception("Criteria B2 does not handle category " + category);
            }
        }

        /// <summary>
        /// Test if criteria BA is Fulfilled.
        /// Severly fragmented or known to exist at only a single location.
        /// </summary>
        /// <param name="category">Category to use when testing sub criteria.</param>
        /// <returns> True if criteria BA is Fulfilled. </returns>
        protected Boolean IsCriteriaBAFulfilled(RedListCategory category)
        {
            switch (category)
            {
                case RedListCategory.CR:
                    return ((HasSeverlyFragmented &&
                             (SeverlyFragmented >= CRITERIA_BA_1_LIMIT)) ||
                            (HasNumberOfLocations &&
                             (NumberOfLocations <= CRITERIA_BA_CR_LIMIT)));
                case RedListCategory.EN:
                    return ((HasSeverlyFragmented &&
                             (SeverlyFragmented >= CRITERIA_BA_1_LIMIT)) ||
                            (HasNumberOfLocations &&
                             (NumberOfLocations < CRITERIA_BA_EN_LIMIT)));
                case RedListCategory.VU:
                    return ((HasSeverlyFragmented &&
                             (SeverlyFragmented >= CRITERIA_BA_1_LIMIT)) ||
                            (HasNumberOfLocations &&
                             (NumberOfLocations < CRITERIA_BA_VU_LIMIT)));
                case RedListCategory.NT:
                    return ((HasSeverlyFragmented &&
                             (SeverlyFragmented >= CRITERIA_BA_1_LIMIT)) ||
                            (HasNumberOfLocations &&
                             (NumberOfLocations < CRITERIA_BA_NT_LIMIT)));
                default:
                    return false;
                    //throw new Exception("Criteria BA does not handle category " + category);
            }
        }

        /// <summary>
        /// Test if criteria BA1 is Fulfilled.
        /// Severly fragmented.
        /// </summary>
        /// <param name="category">Category to use when testing sub criteria.</param>
        /// <returns> True if criteria BA is Fulfilled. </returns>
        public Boolean IsCriteriaBA1Fulfilled(RedListCategory category)
        {
            switch (category)
            {
                case RedListCategory.CR:
                    return ((HasSeverlyFragmented && (SeverlyFragmented >= CRITERIA_BA_1_LIMIT)));
                case RedListCategory.EN:
                    return ((HasSeverlyFragmented && (SeverlyFragmented >= CRITERIA_BA_1_LIMIT)));
                case RedListCategory.VU:
                    return ((HasSeverlyFragmented && (SeverlyFragmented >= CRITERIA_BA_1_LIMIT)));
                case RedListCategory.NT:
                    return ((HasSeverlyFragmented && (SeverlyFragmented >= CRITERIA_BA_1_LIMIT)));
                default:
                    return false;
            }
        }

        /// <summary>
        /// Test if criteria BA1 is Fulfilled.
        /// Known to exist at only a single location.
        /// </summary>
        /// <param name="category">Category to use when testing sub criteria.</param>
        /// <returns> True if criteria BA is Fulfilled. </returns>
        public Boolean IsCriteriaBA2Fulfilled(RedListCategory category)
        {
            switch (category)
            {
                case RedListCategory.CR:
                    return (HasNumberOfLocations && (NumberOfLocations <= CRITERIA_BA_CR_LIMIT));
                case RedListCategory.EN:
                    return (HasNumberOfLocations && (NumberOfLocations < CRITERIA_BA_EN_LIMIT));
                case RedListCategory.VU:
                    return (HasNumberOfLocations && (NumberOfLocations < CRITERIA_BA_VU_LIMIT));
                case RedListCategory.NT:
                    return (HasNumberOfLocations && (NumberOfLocations < CRITERIA_BA_NT_LIMIT));
                default:
                    return false;

            }
        }

        /// <summary>
        /// Test if criteria C is Fulfilled.
        /// Decline in small population.
        /// </summary>
        /// <param name="category">Category to use when testing sub criteria.</param>
        /// <returns> True if criteria C is Fulfilled. </returns>
        public Boolean IsCriteriaCFulfilled(RedListCategory category)
        {
            switch (category)
            {
                case RedListCategory.CR:
                    return (HasPopulationSize &&
                            (PopulationSize < CRITERIA_C_CR_LIMIT) &&
                            (IsCriteriaC1Fulfilled(category) ||
                             IsCriteriaC2Fulfilled(category)));
                case RedListCategory.EN:
                    return (HasPopulationSize &&
                            (PopulationSize < CRITERIA_C_EN_LIMIT) &&
                            (IsCriteriaC1Fulfilled(category) ||
                             IsCriteriaC2Fulfilled(category)));
                case RedListCategory.VU:
                    return (HasPopulationSize &&
                            (PopulationSize < CRITERIA_C_VU_LIMIT) &&
                            (IsCriteriaC1Fulfilled(category) ||
                             IsCriteriaC2Fulfilled(category)));
                case RedListCategory.NT:
                    return ((HasPopulationSize &&
                             (PopulationSize < CRITERIA_C_VU_LIMIT) &&
                             (IsCriteriaC1Fulfilled(category) ||
                              IsCriteriaC2Fulfilled(category))) ||
                            (HasPopulationSize &&
                             (PopulationSize < CRITERIA_C_NT_LIMIT) &&
                             (IsCriteriaC1Fulfilled(RedListCategory.VU) ||
                              IsCriteriaC2Fulfilled(RedListCategory.VU))));
                default:
                    return false;
                    //throw new Exception("Criteria C does not handle category " + category);
            }
        }

        /// <summary>
        /// Test if criteria C1 is Fulfilled.
        /// Decline in population.
        /// </summary>
        /// <param name="category">Category to use when testing sub criteria.</param>
        /// <returns> True if criteria C1 is Fulfilled. </returns>
        protected Boolean IsCriteriaC1Fulfilled(RedListCategory category)
        {
            if (HasContinuingDecline)
            {
                switch (category)
                {
                    case RedListCategory.CR:
                        return (ContinuingDecline >= CRITERIA_C1_CR_LIMIT);
                    case RedListCategory.EN:
                        return (ContinuingDecline >= CRITERIA_C1_EN_LIMIT);
                    case RedListCategory.VU:
                        return (ContinuingDecline >= CRITERIA_C1_VU_LIMIT);
                    case RedListCategory.NT:
                        return (ContinuingDecline >= CRITERIA_C1_NT_LIMIT);
                }
            }
            return false;
        }

        /// <summary>
        /// Test if criteria C2 is Fulfilled.
        /// Decline in number of mature individuals.
        /// </summary>
        /// <param name="category">Category to use when testing sub criteria.</param>
        /// <returns> True if criteria C2 is Fulfilled. </returns>
        protected Boolean IsCriteriaC2Fulfilled(RedListCategory category)
        {
            return (HasContinuingDecline &&
                    (ContinuingDecline >= CRITERIA_C2_LIMIT) &&
                    (IsCriteriaC2AFulfilled(category) ||
                     IsCriteriaC2BFulfilled(category)));
        }

        /// <summary>
        /// Test if criteria C2A is Fulfilled.
        /// Bad population structure.
        /// </summary>
        /// <param name="category">Category to use when testing sub criteria.</param>
        /// <returns>True if criteria C2A is Fulfilled. </returns>
        protected Boolean IsCriteriaC2AFulfilled(RedListCategory category)
        {
            return (IsCriteriaC2A1Fulfilled(category) ||
                    IsCriteriaC2A2Fulfilled(category));
        }

        /// <summary>
        /// Test if criteria C2A1 is Fulfilled.
        /// Small maximum size of subpopulation.
        /// </summary>
        /// <param name="category">Category to use when testing sub criteria.</param>
        /// <returns> True if criteria C2A1 is Fulfilled. </returns>
        protected Boolean IsCriteriaC2A1Fulfilled(RedListCategory category)
        {
            if (HasMaxSizeLocalPopulation)
            {
                switch (category)
                {
                    case RedListCategory.CR:
                        return (MaxSizeLocalPopulation <= CRITERIA_C2A1_CR_LIMIT);
                    case RedListCategory.EN:
                        return (MaxSizeLocalPopulation <= CRITERIA_C2A1_EN_LIMIT);
                    case RedListCategory.VU:
                        return (MaxSizeLocalPopulation <= CRITERIA_C2A1_VU_LIMIT);
                    case RedListCategory.NT:
                        return (MaxSizeLocalPopulation <= CRITERIA_C2A1_NT_LIMIT);
                }
            }
            return false;
        }

        /// <summary>
        /// Test if criteria C2A2 is Fulfilled.
        /// Large % of mature individuals in one subpopulation.
        /// </summary>
        /// <param name="category">Category to use when testing sub criteria.</param>
        /// <returns>
        /// <returns>True if criteria C2A2 is Fulfilled. </returns>
        /// </returns>
        protected Boolean IsCriteriaC2A2Fulfilled(RedListCategory category)
        {
            if (HasMaxProportionLocalPopulation)
            {
                switch (category)
                {
                    case RedListCategory.CR:
                        return (MaxProportionLocalPopulation >= CRITERIA_C2A2_CR_LIMIT);
                    case RedListCategory.EN:
                        return (MaxProportionLocalPopulation >= CRITERIA_C2A2_EN_LIMIT);
                    case RedListCategory.VU:
                        return (MaxProportionLocalPopulation >= CRITERIA_C2A2_VU_LIMIT);
                    case RedListCategory.NT:
                        return (MaxProportionLocalPopulation >= CRITERIA_C2A2_NT_LIMIT);
                }
            }
            return false;
        }

        /// <summary>
        /// Test if criteria C2B is Fulfilled.
        /// Extreme fluctuations in number of mature individuals.
        /// </summary>
        /// <param name="category">Category to use when testing sub criteria.</param>
        /// <returns> True if criteria C2B is Fulfilled. </returns>
        protected Boolean IsCriteriaC2BFulfilled(RedListCategory category)
        {
            return (HasExtremeFluctuations &&
                    (ExtremeFluctuations >= CRITERIA_C2B_LIMIT));
        }

        /// <summary>
        /// Test if criteria D is Fulfilled.
        /// Population very small or restricted.
        /// </summary>
        /// <param name="category">Category to use when testing criteria.</param>
        /// <returns>
        /// True if criteria D is Fulfilled.
        /// </returns>
        public Boolean IsCriteriaDFulfilled(RedListCategory category)
        {
            switch (category)
            {
                case RedListCategory.CR:
                    return (HasPopulationSize &&
                            (PopulationSize < CRITERIA_D_CR_LIMIT));
                case RedListCategory.EN:
                    return (HasPopulationSize &&
                            (PopulationSize < CRITERIA_D_EN_LIMIT));
                case RedListCategory.VU:
                case RedListCategory.NT:
                    return (IsCriteriaD1Fulfilled(category) ||
                            IsCriteriaD2Fulfilled(category));
                default:
                    return false;
                    //throw new Exception("Criteria D does not handle category " + category);
            }
        }

        /// <summary>
        /// Test if criteria D1 is Fulfilled.
        /// Population with a low number of mature individuals.
        /// </summary>
        /// <param name="category">Category to use when testing criteria.</param>
        /// <returns>
        /// True if criteria D1 is Fulfilled.
        /// </returns>
        public Boolean IsCriteriaD1Fulfilled(RedListCategory category)
        {
            switch (category)
            {
                case RedListCategory.VU:
                    return (HasPopulationSize &&
                            (PopulationSize < CRITERIA_D1_VU_LIMIT));
                case RedListCategory.NT:
                    return (HasPopulationSize &&
                            (PopulationSize < CRITERIA_D1_NT_LIMIT));
                default:
                    return false;
            }
        }

        /// <summary>
        /// Test if criteria D2 is Fulfilled.
        /// Population with a very restricted area.
        /// </summary>
        /// <param name="category">Category to use when testing criteria.</param>
        /// <returns>
        /// True if criteria D2 is Fulfilled.
        /// </returns>
        public Boolean IsCriteriaD2Fulfilled(RedListCategory category)
        {
            switch (category)
            {
                case RedListCategory.VU:
                    return (HasVeryRestrictedArea &&
                            (VeryRestrictedArea <= CRITERIA_D2_VU_LIMIT));
                case RedListCategory.NT:
                    return (HasVeryRestrictedArea &&
                            (VeryRestrictedArea <= CRITERIA_D2_NT_LIMIT));
                default:
                    return false;
            }
        }

        /// <summary>
        /// Test if criteria E is Fulfilled.
        /// Probability of extinction.
        /// </summary>
        /// <param name="category">Category to use when testing criteria.</param>
        /// <returns> True if criteria E is Fulfilled. </returns>
        public Boolean IsCriteriaEFulfilled(RedListCategory category)
        {
            switch (category)
            {
                case RedListCategory.CR:
                    return (HasProbabilityOfExtinction &&
                            (ProbabilityOfExtinction <= CRITERIA_E_CR_LIMIT));
                case RedListCategory.EN:
                    return (HasProbabilityOfExtinction &&
                            (ProbabilityOfExtinction <= CRITERIA_E_EN_LIMIT));
                case RedListCategory.VU:
                    return (HasProbabilityOfExtinction &&
                            (ProbabilityOfExtinction <= CRITERIA_E_VU_LIMIT));
                case RedListCategory.NT:
                    return (HasProbabilityOfExtinction &&
                            (ProbabilityOfExtinction <= CRITERIA_E_NT_LIMIT));
                default:
                    return false;
                    //throw new Exception("Criteria E does not handle category " + category);
            }
        }

        /// <summary>
        /// Recalculated red list category and criteria
        /// based on current red list values.
        /// </summary>
        protected virtual void SetRedListValues()
        {
            RedListCriteriaBuilder criteriaBuilder;

            if (!IsInInit && IsEvaluationStatusSet)
            {
                // Get category.
                if (IsCriteriaFulfilled(RedListCategory.RE))
                {
                    _category = RedListCategory.RE;
                }
                else if (IsCriteriaFulfilled(RedListCategory.CR))
                {
                    _category = RedListCategory.CR;
                }
                else if (IsCriteriaFulfilled(RedListCategory.EN))
                {
                    _category = RedListCategory.EN;
                }
                else if (IsCriteriaFulfilled(RedListCategory.VU))
                {
                    _category = RedListCategory.VU;
                }
                else if (IsCriteriaFulfilled(RedListCategory.NT))
                {
                    _category = RedListCategory.NT;
                }
                else
                {
                    _category = RedListCategory.LC;
                }

                // Get criteria.
                if (IsCriteriaCalculated)
                {
                    _criteria = null;
                    if ((_category != RedListCategory.LC) &&
                        (_category != RedListCategory.RE))
                    {
                        criteriaBuilder = new RedListCriteriaBuilder();

                        // Criteria A.
                        if (IsCriteriaAFulfilled(_category))
                        {
                            criteriaBuilder.AddCriteriaLevel1("A");
                            if (IsCriteriaA1Fulfilled(_category))
                            {
                                criteriaBuilder.AddCriteriaLevel2(1);
                                if (IsCriteriaA1AFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("a");
                                }
                                if (IsCriteriaA1BFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("b");
                                }
                                if (IsCriteriaA1CFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("c");
                                }
                                if (IsCriteriaA1DFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("d");
                                }
                                if (IsCriteriaA1EFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("e");
                                }
                            }
                            if (IsCriteriaA2Fulfilled(_category))
                            {
                                criteriaBuilder.AddCriteriaLevel2(2);
                                if (IsCriteriaA2AFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("a");
                                }
                                if (IsCriteriaA2BFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("b");
                                }
                                if (IsCriteriaA2CFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("c");
                                }
                                if (IsCriteriaA2DFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("d");
                                }
                                if (IsCriteriaA2EFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("e");
                                }
                            }
                            if (IsCriteriaA3Fulfilled(_category))
                            {
                                criteriaBuilder.AddCriteriaLevel2(3);
                                if (IsCriteriaA3BFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("b");
                                }
                                if (IsCriteriaA3CFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("c");
                                }
                                if (IsCriteriaA3DFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("d");
                                }
                                if (IsCriteriaA3EFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("e");
                                }
                            }
                            if (IsCriteriaA4Fulfilled(_category))
                            {
                                criteriaBuilder.AddCriteriaLevel2(4);
                                if (IsCriteriaA4AFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("a");
                                }
                                if (IsCriteriaA4BFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("b");
                                }
                                if (IsCriteriaA4CFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("c");
                                }
                                if (IsCriteriaA4DFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("d");
                                }
                                if (IsCriteriaA4EFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("e");
                                }
                            }
                        }

                        // Criteria B.
                        if (IsCriteriaBFulfilled(_category))
                        {
                            criteriaBuilder.AddCriteriaLevel1("B");
                            if (IsCriteriaB1Fulfilled(_category))
                            {
                                criteriaBuilder.AddCriteriaLevel2(1);
                                if (IsCriteriaBAFulfilled(_category))
                                {
                                    criteriaBuilder.AddCriteriaLevel3("a");
                                }
                                if (IsCriteriaBBFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("b");
                                    if (IsCriteriaBB1Fulfilled)
                                    {
                                        criteriaBuilder.AddCriteriaLevel4(1);
                                    }
                                    if (IsCriteriaBB2Fulfilled)
                                    {
                                        criteriaBuilder.AddCriteriaLevel4(2);
                                    }
                                    if (IsCriteriaBB3Fulfilled)
                                    {
                                        criteriaBuilder.AddCriteriaLevel4(3);
                                    }
                                    if (IsCriteriaBB4Fulfilled)
                                    {
                                        criteriaBuilder.AddCriteriaLevel4(4);
                                    }
                                    if (IsCriteriaBB5Fulfilled)
                                    {
                                        criteriaBuilder.AddCriteriaLevel4(5);
                                    }
                                }
                                if (IsCriteriaBCFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("c");
                                    if (IsCriteriaBC1Fulfilled)
                                    {
                                        criteriaBuilder.AddCriteriaLevel4(1);
                                    }
                                    if (IsCriteriaBC2Fulfilled)
                                    {
                                        criteriaBuilder.AddCriteriaLevel4(2);
                                    }
                                    if (IsCriteriaBC3Fulfilled)
                                    {
                                        criteriaBuilder.AddCriteriaLevel4(3);
                                    }
                                    if (IsCriteriaBC4Fulfilled)
                                    {
                                        criteriaBuilder.AddCriteriaLevel4(4);
                                    }
                                }
                            }
                            if (IsCriteriaB2Fulfilled(_category))
                            {
                                criteriaBuilder.AddCriteriaLevel2(2);
                                if (IsCriteriaBAFulfilled(_category))
                                {
                                    criteriaBuilder.AddCriteriaLevel3("a");
                                }
                                if (IsCriteriaBBFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("b");
                                    if (IsCriteriaBB1Fulfilled)
                                    {
                                        criteriaBuilder.AddCriteriaLevel4(1);
                                    }
                                    if (IsCriteriaBB2Fulfilled)
                                    {
                                        criteriaBuilder.AddCriteriaLevel4(2);
                                    }
                                    if (IsCriteriaBB3Fulfilled)
                                    {
                                        criteriaBuilder.AddCriteriaLevel4(3);
                                    }
                                    if (IsCriteriaBB4Fulfilled)
                                    {
                                        criteriaBuilder.AddCriteriaLevel4(4);
                                    }
                                    if (IsCriteriaBB5Fulfilled)
                                    {
                                        criteriaBuilder.AddCriteriaLevel4(5);
                                    }
                                }
                                if (IsCriteriaBCFulfilled)
                                {
                                    criteriaBuilder.AddCriteriaLevel3("c");
                                    if (IsCriteriaBC1Fulfilled)
                                    {
                                        criteriaBuilder.AddCriteriaLevel4(1);
                                    }
                                    if (IsCriteriaBC2Fulfilled)
                                    {
                                        criteriaBuilder.AddCriteriaLevel4(2);
                                    }
                                    if (IsCriteriaBC3Fulfilled)
                                    {
                                        criteriaBuilder.AddCriteriaLevel4(3);
                                    }
                                    if (IsCriteriaBC4Fulfilled)
                                    {
                                        criteriaBuilder.AddCriteriaLevel4(4);
                                    }
                                }
                            }
                        }

                        // Criteria C.
                        if (IsCriteriaCFulfilled(_category))
                        {
                            criteriaBuilder.AddCriteriaLevel1("C");
                            if (IsCriteriaC1Fulfilled(_category))
                            {
                                criteriaBuilder.AddCriteriaLevel2(1);
                            }
                            if (IsCriteriaC2Fulfilled(_category))
                            {
                                criteriaBuilder.AddCriteriaLevel2(2);
                                if (IsCriteriaC2AFulfilled(_category))
                                {
                                    criteriaBuilder.AddCriteriaLevel3("a");
                                    if (IsCriteriaC2A1Fulfilled(_category))
                                    {
                                        criteriaBuilder.AddCriteriaLevel4(1);
                                    }
                                    if (IsCriteriaC2A2Fulfilled(_category))
                                    {
                                        criteriaBuilder.AddCriteriaLevel4(2);
                                    }
                                }
                                if (IsCriteriaC2BFulfilled(_category))
                                {
                                    criteriaBuilder.AddCriteriaLevel3("b");
                                }
                            }
                        }

                        // Criteria D.
                        if (IsCriteriaDFulfilled(_category))
                        {
                            criteriaBuilder.AddCriteriaLevel1("D");
                            if (IsCriteriaD1Fulfilled(_category))
                            {
                                criteriaBuilder.AddCriteriaLevel2(1);
                            }
                            if (IsCriteriaD2Fulfilled(_category))
                            {
                                criteriaBuilder.AddCriteriaLevel2(2);
                            }
                        }

                        // Criteria E.
                        if (IsCriteriaEFulfilled(_category))
                        {
                            criteriaBuilder.AddCriteriaLevel1("E");
                        }

                        _criteria = criteriaBuilder.ToString();
                    }
                }
            }
        }
    }
}

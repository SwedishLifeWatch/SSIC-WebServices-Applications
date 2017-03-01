using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents a factor field.
    /// </summary>
    public class FactorField : IFactorField
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Name for this factor field in database.
        /// </summary>
        public String DatabaseFieldName { get; set; }

        /// <summary>
        /// Enumeration for this factor field.
        /// This property has the value null if this factor field
        /// is not of type enumeration.
        /// </summary>
        public IFactorFieldEnum Enum { get; set; }

        /// <summary>
        /// Factor data type that this factor field belongs to.
        /// </summary>
        public IFactorDataType FactorDataType { get; set; }

        /// <summary>
        /// Id for this factor field.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Index of this factor field.
        /// </summary>
        public Int32 Index { get; set; }

        /// <summary>
        /// Information for this factor field.
        /// </summary>
        public String Information { get; set; }

        /// <summary>
        /// Indicator of weather or not this factor field is in the main field.
        /// </summary>
        public Boolean IsMain { get; set; }

        /// <summary>
        /// Indicator of weather or not this factor field is a substantial field.
        /// </summary>
        public Boolean IsSubstantial { get; set; }

        /// <summary>
        /// Label for this factor field.
        /// </summary>
        public String Label { get; set; }

        /// <summary>
        /// Maximum length of this factor field if it is of type String.
        /// </summary>
        public Int32 Size { get; set; }

        /// <summary>
        /// Type for this factor field.
        /// </summary>
        public IFactorFieldType Type { get; set; }

        /// <summary>
        /// Unit label for this factor field.
        /// </summary>
        public String Unit { get; set; }

        /// <summary>
        /// Get min and max values for this factor field
        /// in specified factor context.
        /// Values are returned in parameter minValue and maxValue.
        /// </summary>
        /// <param name="factor">Factor context.</param>
        /// <param name='minValue'>Is set to min value.</param>
        /// <param name='maxValue'>Is set to max value.</param>
        public virtual void GetMinMax(IFactor factor,
                                      ref Double minValue,
                                      ref Double maxValue)
        {
            if (!IsMinMaxDefined(factor))
            {
                throw new Exception("This factor field does not have min and max values in specified factor context.");
            }

            switch (factor.Id)
            {
                case ((Int32)FactorId.AreaOfOccupancy_B2Estimated):
                    minValue = RedListCalculation.AREA_OF_OCCUPANCY_MIN;
                    maxValue = RedListCalculation.AREA_OF_OCCUPANCY_MAX;
                    break;

                case ((Int32)FactorId.ExtentOfOccurrence_B1Estimated):
                    minValue = RedListCalculation.EXTENT_OF_OCCURRENCE_MIN;
                    maxValue = RedListCalculation.EXTENT_OF_OCCURRENCE_MAX;
                    break;

                case ((Int32)FactorId.MaxProportionLocalPopulation):
                    minValue = RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MIN;
                    maxValue = RedListCalculation.MAX_PROPORTION_LOCAL_POPULATION_MAX;
                    break;

                case ((Int32)FactorId.MaxSizeLocalPopulation):
                    minValue = RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MIN;
                    maxValue = RedListCalculation.MAX_SIZE_LOCAL_POPULATION_MAX;
                    break;

                case ((Int32)FactorId.NumberOfLocations):
                    minValue = RedListCalculation.NUMBER_OF_LOCATIONS_MIN;
                    maxValue = RedListCalculation.NUMBER_OF_LOCATIONS_MAX;
                    break;

                case ((Int32)FactorId.PopulationSize_Total):
                    minValue = RedListCalculation.POPULATION_SIZE_MIN;
                    maxValue = RedListCalculation.POPULATION_SIZE_MAX;
                    break;

                case ((Int32)FactorId.Reduction_A1):
                    minValue = RedListCalculation.POPULATION_REDUCTION_A1_MIN;
                    maxValue = RedListCalculation.POPULATION_REDUCTION_A1_MAX;
                    break;

                case ((Int32)FactorId.Reduction_A2):
                    minValue = RedListCalculation.POPULATION_REDUCTION_A2_MIN;
                    maxValue = RedListCalculation.POPULATION_REDUCTION_A2_MAX;
                    break;

                case ((Int32)FactorId.Reduction_A3):
                    minValue = RedListCalculation.POPULATION_REDUCTION_A3_MIN;
                    maxValue = RedListCalculation.POPULATION_REDUCTION_A3_MAX;
                    break;

                case ((Int32)FactorId.Reduction_A4):
                    minValue = RedListCalculation.POPULATION_REDUCTION_A4_MIN;
                    maxValue = RedListCalculation.POPULATION_REDUCTION_A4_MAX;
                    break;

                default:
                    throw new Exception("Error in class FactorField method GetMinMax() or IsMinMaxDefined().");
            }
        }

        /// <summary>
        /// Get min and max values for this factor field
        /// in specified factor context.
        /// Values are returned in parameter minValue and maxValue.
        /// </summary>
        /// <param name="factor">Factor context.</param>
        /// <param name='minValue'>Is set to min value.</param>
        /// <param name='maxValue'>Is set to max value.</param>
        public virtual void GetMinMax(IFactor factor,
                                      ref Int32 minValue,
                                      ref Int32 maxValue)
        {
            if (!IsMinMaxDefined(factor))
            {
                throw new Exception("This factor field does not have min and max values in specified factor context.");
            }

            switch (factor.Id)
            {
                default:
                    throw new Exception("Error in class FactorField method GetMinMax() or IsMinMaxDefined().");
            }
        }

        /// <summary>
        /// Indicates whether or not this factor field
        /// has min and max values in specified factor context.
        /// </summary>
        /// <param name="factor">Factor context.</param>
        public virtual Boolean IsMinMaxDefined(IFactor factor)
        {
            switch (factor.Id)
            {
                case ((Int32)FactorId.AreaOfOccupancy_B2Estimated):
                    return (Type.DataType == FactorFieldDataTypeId.Double);

                case ((Int32)FactorId.ExtentOfOccurrence_B1Estimated):
                    return (Type.DataType == FactorFieldDataTypeId.Double);

                case ((Int32)FactorId.MaxProportionLocalPopulation):
                    return (Type.DataType == FactorFieldDataTypeId.Double);

                case ((Int32)FactorId.MaxSizeLocalPopulation):
                    return (Type.DataType == FactorFieldDataTypeId.Double);

                case ((Int32)FactorId.NumberOfLocations):
                    return (Type.DataType == FactorFieldDataTypeId.Double);

                case ((Int32)FactorId.PopulationSize_Total):
                    return (Type.DataType == FactorFieldDataTypeId.Double);

                case ((Int32)FactorId.Reduction_A1):
                    return (Type.DataType == FactorFieldDataTypeId.Double);

                case ((Int32)FactorId.Reduction_A2):
                    return (Type.DataType == FactorFieldDataTypeId.Double);

                case ((Int32)FactorId.Reduction_A3):
                    return (Type.DataType == FactorFieldDataTypeId.Double);

                case ((Int32)FactorId.Reduction_A4):
                    return (Type.DataType == FactorFieldDataTypeId.Double);

                default:
                    return false;
            }
        }
    }
}
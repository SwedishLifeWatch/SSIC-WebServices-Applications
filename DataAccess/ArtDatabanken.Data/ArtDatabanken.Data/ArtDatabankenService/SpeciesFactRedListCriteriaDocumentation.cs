using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class represents a species fact that
    /// automatically calculates red list criteria.
    /// </summary>
    [Serializable]
    public class SpeciesFactRedListCriteriaDocumentation : SpeciesFactRedList
    {
        /// <summary>
        /// Creates a species fact instance with no data from web service.
        /// </summary>
        /// <param name="taxon">Taxon object of the species fact</param>
        /// <param name="individualCategory">Individual category object of the species fact</param>
        /// <param name="factor">Factor object of the species fact</param>
        /// <param name="host">Host taxon object of the species fact</param>
        /// <param name="period">Period object of the species fact</param>
        public SpeciesFactRedListCriteriaDocumentation(
            Taxon taxon,
            IndividualCategory individualCategory,
            Factor factor,
            Taxon host,
            Period period)
            : base(taxon, individualCategory, factor, host, period)
        {
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
        public SpeciesFactRedListCriteriaDocumentation(
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
        public SpeciesFactRedListCriteriaDocumentation(
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
        }

        /// <summary>
        /// Get red list criteria.
        /// </summary>
        public String CriteriaDocumentation
        {
            get { return RedListCalculator.CriteriaDocumentation; }
        }

        private RedListCriteriaDocumentationCalculator GetRedListCalculator()
        {
            return ((RedListCriteriaDocumentationCalculator)RedListCalculator);
        }

        /// <summary>
        /// Init calculation of red list criteria.
        /// </summary>
        /// <param name="speciesFacts">Species facts to get data from.</param>
        public override void Init(SpeciesFactList speciesFacts)
        { 
            if (AllowAutomaticUpdate)
            {
                RedListCalculator = new RedListCriteriaDocumentationCalculator(Taxon.TaxonType.Name, Taxon.TaxonType.NameDefinite);
                RedListCalculator.IsCriteriaCalculated = true;
                base.Init(speciesFacts);
                SetReadListValues();
            }
        }

        /// <summary>
        /// Update species fact fields with latest red list values.
        /// </summary>
        protected override void SetReadListValues()
        {
            if (AllowAutomaticUpdate)
            {
                if (RedListCalculator.IsEvaluationStatusSet)
                {
                    Field5.SetValueAutomatic(CriteriaDocumentation);
                }
                else
                {
                    // Reset values to nothing.
                    Field5.SetValueAutomatic(null);
                }
            }
        }

        /// <summary>
        /// Update red list calculation with information from species fact.
        /// </summary>
        /// <param name="speciesFact">Species facts with red list information.</param>
        protected override void SetSpeciesFact(SpeciesFact speciesFact)
        {
            Boolean hasMax = false, hasMin = false, hasProbable = false;
            Double doubleMax = 0, doubleMin = 0, doubleProbable = 0;
            String oldCriteriaDocumentation = null;

            // Test if the species fact is criteria documentation specific.
            switch (speciesFact.Factor.Id)
            {
                case ((Int32)FactorId.RedListCriteriaDocumentationIntroduction):
                case ((Int32)FactorId.GlobalRedlistCategory):
                case ((Int32)FactorId.GenerationTime):
                case ((Int32)FactorId.LastEncounter):
                case ((Int32)FactorId.ImmigrationOccurs):
                    // Criteria documentation specific.
                    break;

                default:
                    // Not criteria documentation specific.
                    // Handle in base class.
                    base.SetSpeciesFact(speciesFact);
                    return;
            }

            if (!IsInInit)
            {
                oldCriteriaDocumentation = GetRedListCalculator().CriteriaDocumentation;
            }

            switch (speciesFact.Factor.Id)
            {
                case ((Int32)FactorId.GenerationTime):
                    GetValues(speciesFact,
                             ref hasMin,
                             ref doubleMin,
                             ref hasProbable,
                             ref doubleProbable,
                             ref hasMax,
                             ref doubleMax);
                    GetRedListCalculator().SetGenerationLength(hasMin,
                                                               doubleMin,
                                                               hasProbable,
                                                               doubleProbable,
                                                               hasMax,
                                                               doubleMax,
                                                               speciesFact.Field4.GetStringValue(),
                                                               speciesFact.Field1.UnitLabel);
                    break;
                case ((Int32)FactorId.GlobalRedlistCategory):
                    if (speciesFact.Field4.HasValue)
                    {
                        GetRedListCalculator().SetGlobalRedlistCategory(speciesFact.Field4.GetStringValue());
                    }
                    else
                    {
                        GetRedListCalculator().SetGlobalRedlistCategory(null);
                    }
                    break;
                case ((Int32)FactorId.ImmigrationOccurs):
                    if (speciesFact.Field1.HasValue)
                    {
                        GetRedListCalculator().SetImmigrationOccurs(speciesFact.Field1.GetBoolean());
                    }
                    else
                    {
                        GetRedListCalculator().SetImmigrationOccurs(false);
                    }
                    break;
                case ((Int32)FactorId.LastEncounter):
                    if (speciesFact.Field1.HasValue)
                    {
                        GetRedListCalculator().SetLastEncounter(true,
                                                                speciesFact.Field1.GetInt32());
                    }
                    else
                    {
                        GetRedListCalculator().SetLastEncounter(false, 0);
                    }
                    break;
                case ((Int32)FactorId.RedListCriteriaDocumentationIntroduction):
                    if (speciesFact.Field5.HasValue)
                    {
                        GetRedListCalculator().SetIntroduction(speciesFact.Field5.GetStringValue());
                    }
                    else
                    {
                        GetRedListCalculator().SetIntroduction(null);
                    }
                    break;
                default:
                    throw new ApplicationException("Not handled species fact. Factor id " + speciesFact.Id);
            }

            if (!IsInInit)
            {
                if (oldCriteriaDocumentation != GetRedListCalculator().CriteriaDocumentation)
                {
                    SetReadListValues();
                }
            }
        }
    }
}

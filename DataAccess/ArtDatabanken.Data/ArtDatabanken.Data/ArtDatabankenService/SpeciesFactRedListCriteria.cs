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
    public class SpeciesFactRedListCriteria : SpeciesFactRedList
    {
        /// <summary>
        /// Creates a species fact instance with no data from web service.
        /// </summary>
        /// <param name="taxon">Taxon object of the species fact</param>
        /// <param name="individualCategory">Individual category object of the species fact</param>
        /// <param name="factor">Factor object of the species fact</param>
        /// <param name="host">Host taxon object of the species fact</param>
        /// <param name="period">Period object of the species fact</param>
        public SpeciesFactRedListCriteria(
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
        public SpeciesFactRedListCriteria(
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
        public SpeciesFactRedListCriteria(
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
        public String Criteria
        {
            get { return RedListCalculator.Criteria; }
        }

        /// <summary>
        /// Get best case red list criteria.
        /// </summary>
        public String CriteriaBestCase
        {
            get { return RedListCalculator.CriteriaBestCase; }
        }

        /// <summary>
        /// Get probable red list criteria.
        /// </summary>
        public String CriteriaProbable
        {
            get { return RedListCalculator.CriteriaProbable; }
        }

        /// <summary>
        /// Get worst case red list criteria.
        /// </summary>
        public String CriteriaWorstCase
        {
            get { return RedListCalculator.CriteriaWorstCase; }
        }

        /// <summary>
        /// Init calculation of red list criteria.
        /// </summary>
        /// <param name="speciesFacts">Species facts to get data from.</param>
        public override void Init(SpeciesFactList speciesFacts)
        {
            if (AllowAutomaticUpdate)
            {
                RedListCalculator = new RedListCalculator(Taxon.TaxonType.Name, Taxon.TaxonType.NameDefinite);
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
                    Field4.SetValueAutomatic(Criteria);
                }
                else
                {
                    // Reset values to nothing.
                    Field4.SetValueAutomatic(null);
                }
            }
        }
    }
}

using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents a species fact that
    /// automatically calculates red list category.
    /// </summary>
    [Serializable]
    public class SpeciesFactRedListCategory : SpeciesFactRedList
    {
        private IFactor _redListCategoryAutomaticFactor;

        /// <summary>
        /// Creates a species fact instance with no data from web service.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="taxon">Taxon object of the species fact</param>
        /// <param name="individualCategory">Individual category object of the species fact</param>
        /// <param name="factor">Factor object of the species fact</param>
        /// <param name="host">Host taxon object of the species fact</param>
        /// <param name="period">Period object of the species fact</param>
        public SpeciesFactRedListCategory(IUserContext userContext,
                                          ITaxon taxon,
                                          IIndividualCategory individualCategory,
                                          IFactor factor,
                                          ITaxon host,
                                          IPeriod period)
            : base(userContext, taxon, individualCategory, factor, host, period)
        {
            _redListCategoryAutomaticFactor = CoreData.FactorManager.GetFactor(userContext, FactorId.RedListCategoryAutomatic);
        }

        /// <summary>
        /// Creates a species fact instance with data from web service.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="id">Id of the species fact</param>
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
        public SpeciesFactRedListCategory(IUserContext userContext,
                                          Int32 id,
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
            : base(userContext,
                   id,
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
            _redListCategoryAutomaticFactor = CoreData.FactorManager.GetFactor(userContext, FactorId.RedListCategoryAutomatic);
        }

        /// <summary>
        /// Creates a species fact instance with data from web service.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="id">Id of the species fact</param>
        /// <param name="taxon">Taxon Id of the species fact</param>
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
        public SpeciesFactRedListCategory(IUserContext userContext,
                                          Int32 id,
                                          ITaxon taxon,
                                          Int32 individualCategoryId,
                                          Int32 factorId,
                                          ITaxon host,
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
            : base(userContext,
                   id,
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
            _redListCategoryAutomaticFactor = CoreData.FactorManager.GetFactor(userContext, FactorId.RedListCategoryAutomatic);
        }

        /// <summary>
        /// Creates a species fact instance with data from data source.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="id">Id of the species fact.</param>
        /// <param name="taxon">Taxon of the species fact.</param>
        /// <param name="individualCategory">Individual Category of the species fact.</param>
        /// <param name="factor">Factor of the species fact.</param>
        /// <param name="host">Host taxon associated with the species fact.</param>
        /// <param name="period">Period of the species fact.</param>
        /// <param name="fieldValue1">Field value of field 1 for the species fact.</param>
        /// <param name="hasFieldValue1">Indicates if field 1 has a value.</param>
        /// <param name="fieldValue2">Field value of field 2 for the species fact.</param>
        /// <param name="hasFieldValue2">Indicates if field 2 has a value.</param>
        /// <param name="fieldValue3">Field value of field 3 for the species fact.</param>
        /// <param name="hasFieldValue3">Indicates if field 3 has a value.</param>
        /// <param name="fieldValue4">Field value of field 4 for the species fact.</param>
        /// <param name="hasFieldValue4">Indicates if field 4 has a value.</param>
        /// <param name="fieldValue5">Field value of field 5 for the species fact.</param>
        /// <param name="hasFieldValue5">Indicates if field 5 has a value.</param>
        /// <param name="quality">Quality of the species fact.</param>
        /// <param name="reference">Reference of the species fact.</param>
        /// <param name="modifiedBy">Full name of the update user of the species fact.</param>
        /// <param name="modifiedDate">Update date of the species fact.</param>
        public SpeciesFactRedListCategory(IUserContext userContext,
                                          Int32 id,
                                          ITaxon taxon,
                                          IIndividualCategory individualCategory,
                                          IFactor factor,
                                          ITaxon host,
                                          IPeriod period,
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
                                          ISpeciesFactQuality quality,
                                          IReference reference,
                                          String modifiedBy,
                                          DateTime modifiedDate)
            : base(userContext,
                   id,
                   taxon,
                   individualCategory,
                   factor,
                   host,
                   period,
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
                   quality,
                   reference,
                   modifiedBy,
                   modifiedDate)
        {
            _redListCategoryAutomaticFactor = CoreData.FactorManager.GetFactor(userContext, FactorId.RedListCategoryAutomatic);
        }

        /// <summary>
        /// Get red list category.
        /// </summary>
        public RedListCategory Category
        {
            get { return RedListCalculator.Category; }
        }

        /// <summary>
        /// Get best case red list category.
        /// </summary>
        public RedListCategory CategoryBestCase
        {
            get { return RedListCalculator.CategoryBestCaseGraded; }
        }

        /// <summary>
        /// Get probable red list category.
        /// </summary>
        public RedListCategory CategoryProbable
        {
            get { return RedListCalculator.CategoryProbableGraded; }
        }

        /// <summary>
        /// Get worst case red list category.
        /// </summary>
        public RedListCategory CategoryWorstCase
        {
            get { return RedListCalculator.CategoryWorstCaseGraded; }
        }

        /// <summary>
        /// Get best case red list category before adjustments for grading.
        /// </summary>
        public RedListCategory CategoryBestCaseNoGrading
        {
            get { return RedListCalculator.CategoryBestCaseNoGrading; }
        }

        /// <summary>
        /// Get probable red list category before adjustments for grading.
        /// </summary>
        public RedListCategory CategoryProbableNoGrading
        {
            get { return RedListCalculator.CategoryProbableNoGrading; }
        }

        /// <summary>
        /// Get worst case red list category before adjustments for grading.
        /// </summary>
        public RedListCategory CategoryWorstCaseNoGrading
        {
            get { return RedListCalculator.CategoryWorstCaseNoGrading; }
        }

        /// <summary>
        /// Init calculation of red list criteria.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="speciesFacts">Species facts to get data from.</param>
        public override void Init(IUserContext userContext,
                                  SpeciesFactList speciesFacts)
        {
            if (AllowAutomaticUpdate && !IsInitialized)
            {
                RedListCalculator = new RedListCalculator(userContext,
                                                          Taxon.Category.Name,
                                                          Taxon.Category.NameDefinite);
                RedListCalculator.IsCriteriaCalculated = false;
                base.Init(userContext, speciesFacts);
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
                    foreach (IFactorFieldEnumValue enumValue in _redListCategoryAutomaticFactor.DataType.Fields[0].Enum.Values)
                    {
                        if (enumValue.KeyInt == ((Int32)Category))
                        {
                            Field1.SetValueAutomatic(enumValue);
                            break;
                        }
                    }
                    Field2.SetValueAutomatic(RedListCalculator.IsGraded);
                    if (RedListCalculator.IsGraded &&
                        ((Category == RedListCategory.CR) ||
                         (Category == RedListCategory.EN) ||
                         (Category == RedListCategory.VU) ||
                         (Category == RedListCategory.NT) ||
                         (Category == RedListCategory.LC)) &&
                        !(((RedListCalculator.CategoryProbableNoGrading == RedListCategory.NT) ||
                           (RedListCalculator.CategoryProbableNoGrading == RedListCategory.LC)) &&
                          (RedListCalculator.CategoryProbableGraded == RedListCategory.LC) &&
                          (Category == RedListCategory.NT) &&
                          RedListCalculator.IsConservationDependent))
                    {
                        Field4.SetValueAutomatic(Category.ToString() + "°");
                    }
                    else
                    {
                        Field4.SetValueAutomatic(Category.ToString());
                    }
                }
                else
                {
                    // Reset values to nothing.
                    Field1.SetValueAutomatic(null);
                    Field2.SetValueAutomatic(null);
                    Field4.SetValueAutomatic(null);
                }
            }
        }
    }
}

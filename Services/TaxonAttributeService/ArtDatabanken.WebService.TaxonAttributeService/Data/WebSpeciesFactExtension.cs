using System;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonAttributeService.Database;

namespace ArtDatabanken.WebService.TaxonAttributeService.Data
{
    /// <summary>
    /// Contains extension to the WebSpeciesFact class.
    /// </summary>
    public static class WebSpeciesFactExtension
    {
        /// <summary>
        /// Load data into the WebSpeciesFact instance.
        /// </summary>
        /// <param name="speciesFact">The species fact instance.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebSpeciesFact speciesFact,
                                    DataReader dataReader)
        {
            speciesFact.FactorId = dataReader.GetInt32(SpeciesFactData.FACTOR_ID);
            speciesFact.FieldValue1 = dataReader.GetDouble(SpeciesFactData.FIELD_VALUE_1, -99);
            speciesFact.FieldValue2 = dataReader.GetDouble(SpeciesFactData.FIELD_VALUE_2, -99);
            speciesFact.FieldValue3 = dataReader.GetDouble(SpeciesFactData.FIELD_VALUE_3, -99);
            speciesFact.FieldValue4 = dataReader.GetString(SpeciesFactData.FIELD_VALUE_4);
            speciesFact.FieldValue5 = dataReader.GetString(SpeciesFactData.FIELD_VALUE_5);
            speciesFact.HostId = dataReader.GetInt32(SpeciesFactData.HOST_ID, -1);
            speciesFact.Id = dataReader.GetInt32(SpeciesFactData.ID);
            speciesFact.IndividualCategoryId = dataReader.GetInt32(SpeciesFactData.INDIVIDUAL_CATEGORY_ID);
            speciesFact.IsFieldValue1Specified = dataReader.IsNotDbNull(SpeciesFactData.FIELD_VALUE_1);
            speciesFact.IsFieldValue2Specified = dataReader.IsNotDbNull(SpeciesFactData.FIELD_VALUE_2);
            speciesFact.IsFieldValue3Specified = dataReader.IsNotDbNull(SpeciesFactData.FIELD_VALUE_3);
            speciesFact.IsFieldValue4Specified = dataReader.IsNotDbNull(SpeciesFactData.FIELD_VALUE_4);
            speciesFact.IsFieldValue5Specified = dataReader.IsNotDbNull(SpeciesFactData.FIELD_VALUE_5);
            speciesFact.IsHostSpecified = speciesFact.HostId > 0;
            speciesFact.IsPeriodSpecified = dataReader.IsNotDbNull(SpeciesFactData.PERIOD_ID);
            speciesFact.ModifiedBy = dataReader.GetString(SpeciesFactData.UPDATE_PERSON);
            speciesFact.ModifiedDate = dataReader.GetDateTime(SpeciesFactData.UPDATE_DATE, new DateTime(2000, 1, 1));
            speciesFact.PeriodId = dataReader.GetInt32(SpeciesFactData.PERIOD_ID, -1);
            speciesFact.QualityId = dataReader.GetInt32(SpeciesFactData.QUALITY_ID, -1);
            speciesFact.ReferenceId = dataReader.GetInt32(SpeciesFactData.REFERENCE_ID, -1);
            speciesFact.TaxonId = dataReader.GetInt32(SpeciesFactData.TAXON_ID);
        }
    }
}
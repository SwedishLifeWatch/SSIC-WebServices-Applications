using System;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Client.TaxonAttributeService
{
    /// <summary>
    /// Contains extension methods to the WebSpeciesFact class.
    /// </summary>
    public static class WebSpeciesFactExtension
    {
        /// <summary>
        /// Get information about species fact as string.
        /// </summary>
        /// <param name='speciesFact'>Species fact.</param>
        /// <returns>Information about species fact as string.</returns>
        public static String GetString(this WebSpeciesFact speciesFact)
        {
            return "FactorId = " + speciesFact.FactorId.WebToString() +
                   ": FieldValue1 = " + speciesFact.FieldValue1.WebToString() +
                   ": FieldValue2 = " + speciesFact.FieldValue2.WebToString() +
                   ": FieldValue3 = " + speciesFact.FieldValue3.WebToString() +
                   ": FieldValue4 = " + speciesFact.FieldValue4 +
                   ": FieldValue5 = " + speciesFact.FieldValue5 +
                   ": HostId = " + speciesFact.HostId.WebToString() +
                   ": Id = " + speciesFact.Id.WebToString() +
                   ": IndividualCategoryId = " + speciesFact.IndividualCategoryId.WebToString() +
                   ": IsFieldValue1Specified = " + speciesFact.IsFieldValue1Specified.WebToString() +
                   ": IsFieldValue2Specified = " + speciesFact.IsFieldValue2Specified.WebToString() +
                   ": IsFieldValue3Specified = " + speciesFact.IsFieldValue3Specified.WebToString() +
                   ": IsFieldValue4Specified = " + speciesFact.IsFieldValue4Specified.WebToString() +
                   ": IsFieldValue5Specified = " + speciesFact.IsFieldValue5Specified.WebToString() +
                   ": IsHostSpecified = " + speciesFact.IsHostSpecified.WebToString() +
                   ": IsPeriodSpecified = " + speciesFact.IsPeriodSpecified.WebToString() +
                   ": ModifiedBy = " + speciesFact.ModifiedBy +
                   ": ModifiedDate = " + speciesFact.ModifiedDate.WebToString() +
                   ": PeriodId = " + speciesFact.PeriodId.WebToString() +
                   ": QualityId = " + speciesFact.QualityId.WebToString() +
                   ": ReferenceId = " + speciesFact.ReferenceId.WebToString() +
                   ": TaxonId = " + speciesFact.TaxonId.WebToString();
        }
    }
}

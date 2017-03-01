using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class represents a species fact that
    /// automatically calculates the taxaon name paragraph
    /// aimed for the Swedish Encyclopedia of Life.
    /// </summary>
    [Serializable]
    public class SpeciesFactTaxonNameSummary : SpeciesFact
    {
        /// <summary>
        /// Creates a species fact instance with no data from web service. 
        /// </summary>
        /// <param name="taxon"></param>
        /// <param name="individualCategory"></param>
        /// <param name="factor"></param>
        /// <param name="host"></param>
        /// <param name="period"></param>
        public SpeciesFactTaxonNameSummary(
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
        public SpeciesFactTaxonNameSummary(
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
        public SpeciesFactTaxonNameSummary(
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
        /// Init calculation automatic species fact field values.
        /// </summary>
        /// <param name="speciesFacts">Species facts to get data from.</param>
        public void Init(SpeciesFactList speciesFacts)
        {
            if (AllowAutomaticUpdate)
            {
                SetTaxonNameValues();
            }
        }

        /// <summary>
        /// Update species fact fields from Dyntaxa.
        /// </summary>
        public void SetTaxonNameValues()
        {
            if (AllowAutomaticUpdate)
            {

                StringBuilder summary = new StringBuilder();
                summary.Append(this.Taxon.ScientificNameAndAuthor);
                summary.Append(".");
                TaxonNameList scientificNames = new TaxonNameList();
                TaxonNameList swedishNames = new TaxonNameList();
                //TaxonName originalName;
                foreach (TaxonName name in this.Taxon.TaxonNames)
                {
                    if (name.TaxonNameType.Id == 0
                        && name.TaxonNameUseType.Id == 0
                        && !name.IsRecommended)
                    {
                        scientificNames.Add(name);
                    }

                    if (name.TaxonNameType.Id == 1
                        && name.TaxonNameUseType.Id == 0
                        && !name.IsRecommended)
                    {
                        swedishNames.Add(name);
                    }
                }

                if (scientificNames.IsNotEmpty())
                {
                    if (scientificNames.Count == 1)
                    {
                        summary.Append(" Synonym: ");
                        summary.Append(scientificNames[0].Name);
                        if (scientificNames[0].Author.IsNotEmpty())
                        {
                            summary.Append(" ");
                            summary.Append(scientificNames[0].Author);
                            summary.Append(".");
                        }
                    }
                    else
                    {
                        summary.Append(" Synonymer: ");
                        for (int i = 0; i < scientificNames.Count - 1; i++)
                        {
                            summary.Append(scientificNames[i].Name);
                            if (scientificNames[i].Author.IsNotEmpty())
                            {
                                summary.Append(" ");
                                summary.Append(scientificNames[i].Author);

                            }
                            if (i < (scientificNames.Count - 2))
                            {
                                summary.Append(", ");
                            }
                        }
                        summary.Append(" och ");
                        summary.Append(scientificNames[scientificNames.Count - 1].Name);
                        if (scientificNames[scientificNames.Count - 1].Author.IsNotEmpty())
                        {
                            summary.Append(" ");
                            summary.Append(scientificNames[scientificNames.Count - 1].Author);
                        }
                        summary.Append(".");
                    }
                }

                if (swedishNames.IsNotEmpty())
                {
                    if (swedishNames.Count == 1)
                    {
                        summary.Append(" Svensk synonym: ");
                        summary.Append(swedishNames[0].Name);
                        summary.Append(".");
                    }
                    else
                    {
                        summary.Append(" Svenska synonymer: ");
                        for (int i = 0; i < swedishNames.Count - 1; i++)
                        {

                            summary.Append(scientificNames[i].Name);
                            if (i < (swedishNames.Count - 2))
                            {
                                summary.Append(", ");
                            }
                        }
                        summary.Append(" och ");
                        summary.Append(swedishNames[swedishNames.Count - 1].Name);
                        summary.Append(".");
                    }
                }

                Field5.SetValueAutomatic(summary.ToString());
            }
        }
    }
}

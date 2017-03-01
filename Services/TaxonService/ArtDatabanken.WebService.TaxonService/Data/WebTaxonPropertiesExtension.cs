using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Database;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    /// <summary>
    /// Extension method to class WebTaxonProperties
    /// </summary>
    public static class WebTaxonPropertiesExtension
    {
        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='taxonProperties'>The taxonproperty object.</param>
        public static void CheckData(this WebTaxonProperties taxonProperties)
        {
            if (!taxonProperties.IsDataChecked)
            {
                taxonProperties.CheckStrings();
                taxonProperties.IsDataChecked = true;
                if (taxonProperties.ValidFromDate.Equals(DateTime.MinValue))
                {
                    taxonProperties.ValidFromDate = DateTime.Now;
                }
                if (taxonProperties.ValidToDate.Equals(DateTime.MinValue))
                {
                    taxonProperties.ValidToDate = Settings.Default.DefaultValidToDate;
                }
            }
        }

        /// <summary>
        /// Get name of the person that made the last
        /// modification of this taxon properties.
        /// </summary>
        /// <param name='taxonProperties'>Taxon properties.</param>
        /// <returns>Name of the person that made the last modification of this taxon properties.</returns>
        public static String GetModifiedByPerson(this WebTaxonProperties taxonProperties)
        {
            return taxonProperties.DataFields.GetString(Settings.Default.WebDataModifiedByPerson);
        }

        /// <summary>
        /// </summary>
        /// <param name="taxonProperties">The taxon properties.</param>
        /// <param name="dataReader">The data reader.</param>
        public static void LoadData(this WebTaxonProperties taxonProperties, DataReader dataReader)
        {
            taxonProperties.Id = dataReader.GetInt32(TaxonCommon.ID);
            taxonProperties.Taxon = new WebTaxon {Id = dataReader.GetInt32(TaxonCommon.TAXON_ID)};
            taxonProperties.TaxonCategory = new WebTaxonCategory { Id = dataReader.GetInt32(TaxonPropertiesData.TAXONCATEGORYID), Name = dataReader.GetString(TaxonPropertiesData.TAXONCATEGORYNAME) };
            taxonProperties.ConceptDefinition = dataReader.GetString(TaxonPropertiesData.CONCEPT_DEFINITION_FULL_GENERATED);
            taxonProperties.PartOfConceptDefinition = dataReader.GetString(TaxonPropertiesData.CONCEPT_DEFINITION_PART);
            if (dataReader.IsNotDbNull(TaxonPropertiesData.ALERT_STATUS))
            {
                taxonProperties.AlertStatusId = dataReader.GetInt32(TaxonPropertiesData.ALERT_STATUS);
            }
            else
            {
                taxonProperties.AlertStatusId = (Int32)(TaxonAlertStatusId.Yellow);
            }
            taxonProperties.ValidFromDate = dataReader.GetDateTime(TaxonCommon.VALID_FROM_DATE);
            taxonProperties.ValidToDate = dataReader.GetDateTime(TaxonCommon.VALID_TO_DATE);
            taxonProperties.IsValid = dataReader.GetBoolean(TaxonPropertiesData.IS_VALID);
            if (dataReader.IsNotDbNull(TaxonPropertiesData.REVISIONEVENTID))
            {
                taxonProperties.ChangedInTaxonRevisionEvent = new WebTaxonRevisionEvent { Id = dataReader.GetInt32(TaxonPropertiesData.REVISIONEVENTID) };
            }

            if (dataReader.IsNotDbNull(TaxonPropertiesData.CHANGEDINREVISIONEVENTID))
            {
                taxonProperties.ReplacedInTaxonRevisionEvent = new WebTaxonRevisionEvent { Id = dataReader.GetInt32(TaxonPropertiesData.CHANGEDINREVISIONEVENTID) };    
            }
            
            taxonProperties.IsPublished = dataReader.GetBoolean(TaxonPropertiesData.ISPUBLISHED);
            taxonProperties.SetModifiedByPerson(dataReader.GetString(TaxonData.PERSON_NAME));
            taxonProperties.ModifiedBy = new WebUser { Id = dataReader.GetInt32(TaxonCommon.MODIFIED_BY) };
            taxonProperties.ModifiedDate = dataReader.GetDateTime(TaxonCommon.MODIFIED_DATE);

            if (taxonProperties.DataFields == null)
            {
                taxonProperties.DataFields = new List<WebDataField>();
            }

            if (dataReader.HasColumn(TaxonPropertiesData.IS_MICROSPECIES))
            {
                taxonProperties.DataFields.Add(new WebDataField
                {
                    Name = TaxonPropertiesData.IS_MICROSPECIES,
                    Type = WebDataType.Boolean,
                    Value = dataReader.GetBoolean(TaxonPropertiesData.IS_MICROSPECIES).ToString()
                });
            }
        }

        /// <summary>
        /// Set modified by person in WebTaxonProperties.
        /// </summary>
        /// <param name='taxonProperties'>Taxon properties.</param>
        /// <param name='modifiedByPerson'>Modified by person.</param>
        public static void SetModifiedByPerson(this WebTaxonProperties taxonProperties,
                                               String modifiedByPerson)
        {
            WebDataField dataField;

            // Add modified by person as dynamic property.
            dataField = new WebDataField();
            dataField.Name = Settings.Default.WebDataModifiedByPerson;
            dataField.Type = WebDataType.String;
            dataField.Value = modifiedByPerson;
            if (taxonProperties.DataFields.IsNull())
            {
                taxonProperties.DataFields = new List<WebDataField>();
            }
            taxonProperties.DataFields.Add(dataField);
        }
    }
}

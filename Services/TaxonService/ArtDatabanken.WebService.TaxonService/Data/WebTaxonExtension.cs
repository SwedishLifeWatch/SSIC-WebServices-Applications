using System;
using System.Collections.Generic;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Database;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    /// <summary>
    /// Contains extension to the WebTaxon class.
    /// </summary>
    public static class WebTaxonExtension
    {
        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='taxon'>The taxon.</param>
        public static void CheckData(this WebTaxon taxon)
        {
            if (!taxon.IsDataChecked)
            {
                taxon.CheckStrings();
                taxon.IsDataChecked = true;

                if (taxon.ValidFromDate.Equals(DateTime.MinValue))
                {
                    taxon.ValidFromDate = DateTime.Now;
                }

                if (taxon.ValidToDate.Equals(DateTime.MinValue))
                {
                    taxon.ValidToDate = Settings.Default.DefaultValidToDate;
                }
            }
        }

        /// <summary>
        /// Get name of the person that made the last
        /// modification of this taxon.
        /// </summary>
        /// <param name='taxon'>Taxon.</param>
        /// <returns>Name of the person that made the last modification of this taxon.</returns>
        public static String GetModifiedByPerson(this WebTaxon taxon)
        {
            return taxon.DataFields.GetString(Settings.Default.WebDataModifiedByPerson);
        }

        /// <summary>
        /// Load data into the WebTaxon instance.
        /// </summary>
        /// <param name='taxon'>Taxon.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebTaxon taxon,
                                    DataReader dataReader)
        {
            taxon.AlertStatusId = dataReader.GetInt32(TaxonData.ALERT_STATUS);
            taxon.Author = dataReader.GetString(TaxonData.AUTHOR);
            if (dataReader.IsNotDbNull(TaxonData.CATEGORY))
            {
                taxon.CategoryId = dataReader.GetInt32(TaxonData.CATEGORY);
            }
            
            taxon.ChangeStatusId = dataReader.GetInt32(TaxonData.CHANGE_STATUS);
            taxon.CommonName = dataReader.GetString(TaxonData.COMMON_NAME);
            taxon.CreatedBy = dataReader.GetInt32(TaxonCommon.CREATED_BY);
            taxon.CreatedDate = dataReader.GetDateTime(TaxonCommon.CREATED_DATE);
            taxon.Guid = dataReader.GetString(TaxonCommon.GUID);
            taxon.Id = dataReader.GetInt32(TaxonCommon.ID);
            taxon.IsInRevision = dataReader.IsNotDbNull(TaxonData.ONGOING_REVISION_ID);
            if (taxon.IsInRevision)
            {
                SetTaxonRevisionId(taxon, dataReader.GetInt32(TaxonData.ONGOING_REVISION_ID));
            }
            taxon.IsPublished = dataReader.GetBoolean(TaxonCommon.IS_PUBLISHED);
            taxon.IsValid = dataReader.GetBoolean(TaxonData.IS_VALID);
            taxon.ModifiedBy = dataReader.GetInt32(TaxonCommon.MODIFIED_BY);
            taxon.SetModifiedByPerson(dataReader.GetString(TaxonData.PERSON_NAME));
            taxon.ModifiedDate = dataReader.GetDateTime(TaxonCommon.MODIFIED_DATE);
            taxon.PartOfConceptDefinition = dataReader.GetString(TaxonData.CONCEPT_DEFINITION_PART);
            taxon.ScientificName = dataReader.GetString(TaxonData.SCIENTIFIC_NAME);
            if (dataReader.IsNotDbNull(TaxonCommon.SORT_ORDER))
            {
                taxon.SortOrder = dataReader.GetInt32(TaxonCommon.SORT_ORDER);
            }
            taxon.ValidFromDate = dataReader.GetDateTime(TaxonCommon.VALID_FROM_DATE);
            taxon.ValidToDate = dataReader.GetDateTime(TaxonCommon.VALID_TO_DATE);

            if (taxon.DataFields == null)
            {
                taxon.DataFields = new List<WebDataField>();
            }

            if (dataReader.HasColumn(TaxonPropertiesData.IS_MICROSPECIES))
            {
                taxon.DataFields.Add(new WebDataField
                {
                    Name = TaxonPropertiesData.IS_MICROSPECIES,
                    Type = WebDataType.Boolean,
                    Value = dataReader.GetBoolean(TaxonPropertiesData.IS_MICROSPECIES).ToString()
                });
            }
        }

        /// <summary>
        /// Set Taxon Revision Id in WebTaxon.
        /// </summary>        
        /// <param name="taxon">The taxon.</param>
        /// <param name="taxonRevisionId">The taxon revision identifier.</param>
        public static void SetTaxonRevisionId(this WebTaxon taxon, Int32 taxonRevisionId)
        {
            WebDataField dataField;
            
            // Add taxon revision id property.
            dataField = new WebDataField();
            dataField.Name = Settings.Default.WebDataTaxonRevisionId;
            dataField.Type = WebDataType.Int32;
            dataField.Value = taxonRevisionId.WebToString();
            if (taxon.DataFields.IsNull())
            {
                taxon.DataFields = new List<WebDataField>();
            }

            taxon.DataFields.Add(dataField);
        }

        /// <summary>
        /// Set modified by person in WebTaxon.
        /// </summary>
        /// <param name='taxon'>Taxon.</param>
        /// <param name='modifiedByPerson'>Modified by person.</param>
        public static void SetModifiedByPerson(this WebTaxon taxon,
                                               String modifiedByPerson)
        {
            WebDataField dataField;

            // Add modified by person as dynamic property.
            dataField = new WebDataField();
            dataField.Name = Settings.Default.WebDataModifiedByPerson;
            dataField.Type = WebDataType.String;
            dataField.Value = modifiedByPerson;
            if (taxon.DataFields.IsNull())
            {
                taxon.DataFields = new List<WebDataField>();
            }
            taxon.DataFields.Add(dataField);
        }
    }
}

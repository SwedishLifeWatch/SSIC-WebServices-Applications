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
    public static class WebTaxonNameExtension
    {
        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='taxonName'>TaxonName object.</param>
        public static void CheckData(this WebTaxonName taxonName)
        {
            if (!taxonName.IsDataChecked)
            {
                taxonName.CheckStrings();
                taxonName.IsDataChecked = true;
                if (taxonName.ValidFromDate.Equals(DateTime.MinValue))
                {
                    taxonName.ValidFromDate = DateTime.Now;
                }

                if (taxonName.ValidToDate.Equals(DateTime.MinValue))
                {
                    taxonName.ValidToDate = Settings.Default.DefaultValidToDate;
                }
            }
        }

        /// <summary>
        /// Get name of the person that made the last
        /// modification of this taxon name.
        /// </summary>
        /// <param name='taxonName'>Taxon name.</param>
        /// <returns>Name of the person that made the last modification of this taxon name.</returns>
        public static String GetModifiedByPerson(this WebTaxonName taxonName)
        {
            return taxonName.DataFields.GetString(Settings.Default.WebDataModifiedByPerson);
        }

        /// <summary>
        /// Get version of the WebTaxonName.
        /// </summary>
        /// <param name='taxonName'>TaxonName.</param>
        /// <returns>Version of the WebTaxonName.</returns>
        public static Int32 GetVersion(this WebTaxonName taxonName)
        {
            return taxonName.DataFields.GetInt32("Version");
        }

        /// <summary>
        /// Get name usage id of the WebTaxonName.
        /// </summary>
        /// <param name='taxonName'>TaxonName.</param>
        /// <returns>Name usage id of the WebTaxonName.</returns>
        public static Int32 GetNameUsageId(this WebTaxonName taxonName)
        {
            return taxonName.DataFields.GetInt32("NameUsageId");
        }

        /// <summary>
        /// Load data into the WebTaxonName instance.
        /// </summary>
        /// <param name='taxonName'>TaxonName.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebTaxonName taxonName, DataReader dataReader)
        {
            taxonName.Author = dataReader.GetString(TaxonNameData.AUTHOR);
            taxonName.CategoryId = dataReader.GetInt32(TaxonNameData.NAMECATEGORY);
            taxonName.IsChangedInTaxonRevisionEventIdSpecified = dataReader.IsNotDbNull(TaxonCommon.REVISON_EVENT_ID);
            if (taxonName.IsChangedInTaxonRevisionEventIdSpecified)
            {
                taxonName.ChangedInTaxonRevisionEventId = dataReader.GetInt32(TaxonCommon.REVISON_EVENT_ID);
            }
            taxonName.CreatedBy = dataReader.GetInt32(TaxonCommon.CREATED_BY);
            taxonName.CreatedDate = dataReader.GetDateTime(TaxonCommon.CREATED_DATE);
            taxonName.Description = dataReader.GetString(TaxonNameData.DESCRIPTION);
            taxonName.Id = dataReader.GetInt32(TaxonNameData.TAXON_NAME_ID);
            taxonName.Guid = dataReader.GetString(TaxonCommon.GUID);
            taxonName.IsOkForSpeciesObservation = dataReader.GetBoolean(TaxonNameData.IS_OK_FOR_OBS_SYSTEMS);
            taxonName.IsOriginalName = dataReader.GetBoolean(TaxonNameData.IS_ORIGINAL);
            taxonName.IsPublished = dataReader.GetBoolean(TaxonCommon.IS_PUBLISHED);
            taxonName.IsRecommended = dataReader.GetBoolean(TaxonNameData.IS_RECOMMENDED);
            taxonName.IsUnique = dataReader.GetBoolean(TaxonNameData.IS_UNIQUE);
            taxonName.ModifiedBy = dataReader.GetInt32(TaxonCommon.MODIFIED_BY);
            taxonName.SetModifiedByPerson(dataReader.GetString(TaxonNameData.PERSONNAME));
            taxonName.SetNameUsageId(dataReader.GetInt32(TaxonNameData.NAMEUSAGENEW));
            taxonName.ModifiedDate = dataReader.GetDateTime(TaxonCommon.MODIFIED_DATE);
            taxonName.Name = dataReader.GetString(TaxonNameData.NAME);
            taxonName.IsReplacedInTaxonRevisionEventIdSpecified = dataReader.IsNotDbNull(TaxonCommon.CHANGED_IN_REVISON_EVENT_ID);
            if (taxonName.IsReplacedInTaxonRevisionEventIdSpecified)
            {
                taxonName.ReplacedInTaxonRevisionEventId = dataReader.GetInt32(TaxonCommon.CHANGED_IN_REVISON_EVENT_ID);
            }
            taxonName.StatusId = dataReader.GetInt32(TaxonNameData.NAMEUSAGE);
            taxonName.Taxon = new WebTaxon { Id = dataReader.GetInt32(TaxonCommon.TAXON_ID) };
            taxonName.ValidFromDate = dataReader.GetDateTime(TaxonCommon.VALID_FROM_DATE);
            taxonName.ValidToDate = dataReader.GetDateTime(TaxonCommon.VALID_TO_DATE);
            taxonName.SetVersion(dataReader.GetInt32(TaxonNameData.ID));
        }

        /// <summary>
        /// Set modified by person in WebTaxonName.
        /// </summary>
        /// <param name='taxonName'>Taxon name.</param>
        /// <param name='modifiedByPerson'>Modified by person.</param>
        public static void SetModifiedByPerson(this WebTaxonName taxonName,
                                               String modifiedByPerson)
        {
            WebDataField dataField;

            // Add modified by person as dynamic property.
            dataField = new WebDataField();
            dataField.Name = Settings.Default.WebDataModifiedByPerson;
            dataField.Type = WebDataType.String;
            dataField.Value = modifiedByPerson;
            if (taxonName.DataFields.IsNull())
            {
                taxonName.DataFields = new List<WebDataField>();
            }
            taxonName.DataFields.Add(dataField);
        }

        /// <summary>
        /// Set version in WebTaxonName.
        /// </summary>
        /// <param name='taxonName'>TaxonName.</param>
        /// <param name='version'>Version.</param>
        public static void SetVersion(this WebTaxonName taxonName,
                                      Int32 version)
        {
            WebDataField dataField;

            // Add version as dynamic property.
            dataField = new WebDataField();
            dataField.Name = "Version";
            dataField.Type = WebDataType.Int32;
            dataField.Value = version.WebToString();
            if (taxonName.DataFields.IsNull())
            {
                taxonName.DataFields = new List<WebDataField>();
            }
            taxonName.DataFields.Add(dataField);
        }

        /// <summary>
        /// Set name usage id in WebTaxonName.
        /// </summary>
        /// <param name='taxonName'>TaxonName.</param>
        /// <param name='nameUsageId'>Name usage id.</param>
        public static void SetNameUsageId(this WebTaxonName taxonName,
                                      Int32 nameUsageId)
        {
            WebDataField dataField;

            // Add name usage as dynamic property.
            dataField = new WebDataField();
            dataField.Name = "NameUsageId";
            dataField.Type = WebDataType.Int32;
            dataField.Value = nameUsageId.WebToString();
            if (taxonName.DataFields.IsNull())
            {
                taxonName.DataFields = new List<WebDataField>();
            }

            taxonName.DataFields.Add(dataField);
        }

    }
}

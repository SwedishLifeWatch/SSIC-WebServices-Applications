using System;
using System.Collections.Generic;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Database;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    /// <summary>
    /// Extensions for lump/split
    /// </summary>
    public static class WebLumpSplitEventExtension
    {
        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='webLumpSplitEvent'>This lump split event.</param>
        public static void CheckData(this WebLumpSplitEvent webLumpSplitEvent)
        {
            if (!webLumpSplitEvent.IsDataChecked)
            {
                webLumpSplitEvent.CheckStrings();
                webLumpSplitEvent.IsDataChecked = true;
            }
        }

        /// <summary>
        /// Load data into the WebTaxon instance.
        /// </summary>
        /// <param name='webLumpSplitEvent'>LumpSplit event.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebLumpSplitEvent webLumpSplitEvent, DataReader dataReader)
        {
            webLumpSplitEvent.Id = dataReader.GetInt32(LumpSplitEventData.ID);
            webLumpSplitEvent.IsChangedInTaxonRevisionEventIdSpecified = dataReader.IsNotDbNull(TaxonPropertiesData.REVISIONEVENTID);
            if (webLumpSplitEvent.IsChangedInTaxonRevisionEventIdSpecified)
            {
                webLumpSplitEvent.ChangedInTaxonRevisionEventId = dataReader.GetInt32(LumpSplitEventData.REVISIONEVENTID);
            }
            webLumpSplitEvent.IsReplacedInTaxonRevisionEventIdSpecified = dataReader.IsNotDbNull(TaxonPropertiesData.CHANGEDINREVISIONEVENTID);
            if (webLumpSplitEvent.IsReplacedInTaxonRevisionEventIdSpecified)
            {
                webLumpSplitEvent.ReplacedInTaxonRevisionEventId = dataReader.GetInt32(LumpSplitEventData.CHANGEDINREVISIONEVENTID);
            }

            webLumpSplitEvent.CreatedBy = dataReader.GetInt32(LumpSplitEventData.CREATEDBY);
            webLumpSplitEvent.CreatedDate = dataReader.GetDateTime(LumpSplitEventData.CREATEDDATE);
            webLumpSplitEvent.Description = dataReader.GetString(LumpSplitEventData.DESCRIPTIONSTRING);
            webLumpSplitEvent.Guid = dataReader.GetString(TaxonCommon.GUID);
            webLumpSplitEvent.TypeId = dataReader.GetInt32(LumpSplitEventData.EVENTTYPE);
            webLumpSplitEvent.IsPublished = dataReader.GetBoolean(LumpSplitEventData.ISPUBLISHED);
            webLumpSplitEvent.SetCreatedByPerson(dataReader.GetString(LumpSplitEventData.PERSONNAME));
            webLumpSplitEvent.TaxonIdAfter = dataReader.GetInt32(LumpSplitEventData.TAXONIDAFTER);
            webLumpSplitEvent.TaxonIdBefore = dataReader.GetInt32(LumpSplitEventData.TAXONIDBEFORE);
        }

        /// <summary>
        /// Set created by person in WebLumpSplitEvent.
        /// </summary>
        /// <param name='lumpSplitEvent'>Lump split event.</param>
        /// <param name='createdByPerson'>Created by person.</param>
        public static void SetCreatedByPerson(this WebLumpSplitEvent lumpSplitEvent,
                                              String createdByPerson)
        {
            WebDataField dataField;

            // Add created by person as dynamic property.
            dataField = new WebDataField();
            dataField.Name = Settings.Default.WebDataCreatedByPerson;
            dataField.Type = WebDataType.String;
            dataField.Value = createdByPerson;
            if (lumpSplitEvent.DataFields.IsNull())
            {
                lumpSplitEvent.DataFields = new List<WebDataField>();
            }
            lumpSplitEvent.DataFields.Add(dataField);
        }
    }
}

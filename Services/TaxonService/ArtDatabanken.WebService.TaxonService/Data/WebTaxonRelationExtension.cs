using System;
using System.Collections.Generic;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Database;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    /// <summary>
    /// Extensions to WebTaxonRelation
    /// </summary>
    public static class WebTaxonRelationExtension
    {
        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='taxonRelation'>The taxonproperty object.</param>
        public static void CheckData(this WebTaxonRelation taxonRelation)
        {
            if (!taxonRelation.IsDataChecked)
            {
                taxonRelation.CheckStrings();
                taxonRelation.IsDataChecked = true;
                if (taxonRelation.ValidFromDate.Equals(DateTime.MinValue))
                {
                    taxonRelation.ValidFromDate = DateTime.Now;
                }
                if (taxonRelation.ValidToDate.Equals(DateTime.MinValue))
                {
                    taxonRelation.ValidToDate = Settings.Default.DefaultValidToDate;
                }
            }
        }

        /// <summary>
        /// Get name of the person that made the last
        /// modification of this taxon relation.
        /// </summary>
        /// <param name='taxonRelation'>Taxon relation.</param>
        /// <returns>Name of the person that made the last modification of this taxon relation.</returns>
        public static String GetModifiedByPerson(this WebTaxonRelation taxonRelation)
        {
            return taxonRelation.DataFields.GetString(Settings.Default.WebDataModifiedByPerson);
        }

        /// <summary>
        /// Load data into the WebTaxon instance.
        /// </summary>
        /// <param name='taxonRelation'>TaxonRelation.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebTaxonRelation taxonRelation,
                                    DataReader dataReader)
        {
            taxonRelation.ChildTaxonId = dataReader.GetInt32(TaxonRelationData.TAXONIDCHILD);
            taxonRelation.CreatedBy = dataReader.GetInt32(TaxonRelationData.CREATEDBY);
            taxonRelation.CreatedDate = dataReader.GetDateTime(TaxonRelationData.CREATEDDATE);
            taxonRelation.Guid = dataReader.GetString(TaxonRelationData.GUID);
            taxonRelation.Id = dataReader.GetInt32(TaxonRelationData.ID);
            taxonRelation.IsMainRelation = dataReader.GetBoolean(TaxonRelationData.IS_MAIN_RELATION);
            taxonRelation.IsPublished = dataReader.GetBoolean(TaxonRelationData.ISPUBLISHED);
            taxonRelation.ModifiedBy = dataReader.GetInt32(TaxonRelationData.MODIFIEDBY);
            taxonRelation.SetModifiedByPerson(dataReader.GetString(TaxonRelationData.PERSONNAME));
            taxonRelation.ModifiedDate = dataReader.GetDateTime(TaxonRelationData.MODIFIEDDATE);
            taxonRelation.ParentTaxonId = dataReader.GetInt32(TaxonRelationData.TAXONIDPARENT);
            taxonRelation.SortOrder = dataReader.GetInt32(TaxonRelationData.SORTORDER);
            taxonRelation.ValidFromDate = dataReader.GetDateTime(TaxonRelationData.VALIDFROMDATE);
            taxonRelation.ValidToDate = dataReader.GetDateTime(TaxonRelationData.VALIDTODATE);

            taxonRelation.IsChangedInTaxonRevisionEventIdSpecified = dataReader.IsNotDbNull(TaxonRelationData.REVISIONEVENTID);
            if (taxonRelation.IsChangedInTaxonRevisionEventIdSpecified)
            {
                taxonRelation.ChangedInTaxonRevisionEventId = dataReader.GetInt32(TaxonRelationData.REVISIONEVENTID);
            }

            taxonRelation.IsReplacedInTaxonRevisionEventIdSpecified = dataReader.IsNotDbNull(TaxonRelationData.CHANGEDINREVISIONEVENTID);
            if (taxonRelation.IsReplacedInTaxonRevisionEventIdSpecified)
            {
                taxonRelation.ReplacedInTaxonRevisionEventId = dataReader.GetInt32(TaxonRelationData.CHANGEDINREVISIONEVENTID);
            }
        }

        /// <summary>
        /// Set modified by person in WebTaxonRelation.
        /// </summary>
        /// <param name='taxonRelation'>Taxon relation.</param>
        /// <param name='modifiedByPerson'>Modified by person.</param>
        public static void SetModifiedByPerson(this WebTaxonRelation taxonRelation,
                                               String modifiedByPerson)
        {
            WebDataField dataField;

            // Add modified by person as dynamic property.
            dataField = new WebDataField();
            dataField.Name = Settings.Default.WebDataModifiedByPerson;
            dataField.Type = WebDataType.String;
            dataField.Value = modifiedByPerson;
            if (taxonRelation.DataFields.IsNull())
            {
                taxonRelation.DataFields = new List<WebDataField>();
            }
            taxonRelation.DataFields.Add(dataField);
        }
    }
}

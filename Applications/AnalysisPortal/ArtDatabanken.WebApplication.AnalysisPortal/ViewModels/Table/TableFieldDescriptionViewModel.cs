﻿using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Table
{
    /// <summary>
    /// This class is a view model for ISpeciesObservationFieldDescription
    /// </summary>
    public class TableFieldDescriptionViewModel
    {
        /// <summary>
        /// The id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of this data field.        
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// A recommended field label to be used in presentations of data. The label is by defauld in english, but swedish labels exists for most fields. The language of the label is determined by the Locale.        
        /// </summary>
        public String Label { get; set; }

        /// <summary>
        /// Sort order of the data field.        
        /// </summary>
        public Int32 SortOrder { get; set; }        

        /// <summary>
        /// Name of the class that contains this data field.        
        /// </summary>
        public String Class { get; set; }
        
        /// <summary>
        /// Definition of the data field according to TDWG or Swedish LifeWatch. 
        /// This property is mandatory. 
        /// </summary>
        public String Definition { get; set; }

        /// <summary>
        /// Url to the original definition of the data field.
        /// </summary>
        public String DefinitionUrl { get; set; }

        /// <summary>
        /// Description of the implementation of this field in the Swedish Species Observation Services.
        /// </summary>
        public String Documentation { get; set; }

        /// <summary>
        /// Url to the original description of the implementation of this field in the Swedish Species Observation Services.
        /// </summary>
        public String DocumentationUrl { get; set; }

        /// <summary>
        /// The LSID of this data field.        
        /// </summary>
        public String Guid { get; set; }

        /// <summary>
        /// Classification of the importance of the field. The lower the value the more gerneral and important is this field.        
        /// </summary>
        public Int32 Importance { get; set; }

        /// <summary>
        /// Indication of whether or not this data field is an accepted Darwin Core term.        
        /// </summary>
        public Boolean IsAcceptedByTdwg { get; set; }

        /// <summary>
        /// Indication of whether this object represents a full class representing a group of fields or an acutal field.        
        /// </summary>
        public Boolean IsClassName { get; set; }

        /// <summary>
        /// Indication of whether or not this field is implemented in the Swedish Species Observation Service.        
        /// </summary>
        public Boolean IsImplemented { get; set; }

        /// <summary>
        /// Indication of whether or not this field is mandatory at Swedsih Specis Observation Service. As a mandatory field values are excpected for all records.        
        /// </summary>
        public Boolean IsMandatory { get; set; }

        /// <summary>
        /// Indication of whether or not this field is mandatory when harvesting from a Data provider.        
        /// </summary>
        public Boolean IsMandatoryFromProvider { get; set; }

        /// <summary>
        /// Indication of whether this field is obtained from the Data provider or generated by other means.        
        /// </summary>
        public Boolean IsObtainedFromProvider { get; set; }

        /// <summary>
        /// Indication of whether or not this field is planned for the Swedish Species Observation Service.        
        /// </summary>
        public Boolean IsPlanned { get; set; }
       
        /// <summary>
        /// Comments on the current or planned implementation.
        /// </summary>
        public String Remarks { get; set; }

        /// <summary>
        /// Data type of the field values.        
        /// </summary>
        public string Type { get; set; }

        public TableFieldDescriptionViewModel()
        {
        }

        public TableFieldDescriptionViewModel(int id, string name, string label)
        {
            Id = id;
            Name = name;
            Label = label;
        }

        /// <summary>
        /// Creates a TableFieldDescriptionViewModel object 
        /// from a ISpeciesObservationFieldDescription object.
        /// </summary>
        /// <param name="fieldDescription">The field description.</param>        
        public static TableFieldDescriptionViewModel CreateFromSpeciesObservationFieldDescription(
            ISpeciesObservationFieldDescription fieldDescription)
        {
            TableFieldDescriptionViewModel model = new TableFieldDescriptionViewModel();
            model.Class = fieldDescription.Class.GetName();
            model.Definition = fieldDescription.Definition;
            model.DefinitionUrl = fieldDescription.DefinitionUrl;
            model.Documentation = fieldDescription.Documentation;
            model.DocumentationUrl = fieldDescription.DocumentationUrl;
            model.Guid = fieldDescription.Guid;
            model.Id = fieldDescription.Id;
            model.Importance = fieldDescription.Importance;
            model.IsAcceptedByTdwg = fieldDescription.IsAcceptedByTdwg;
            model.IsClassName = fieldDescription.IsClass;
            model.IsImplemented = fieldDescription.IsImplemented;
            model.IsMandatory = fieldDescription.IsMandatory;
            model.IsMandatoryFromProvider = fieldDescription.IsMandatoryFromProvider;
            model.IsObtainedFromProvider = fieldDescription.IsObtainedFromProvider;
            model.IsPlanned = fieldDescription.IsPlanned;
            model.Label = fieldDescription.Label;
            model.Name = fieldDescription.Name;
            model.Remarks = fieldDescription.Remarks;
            model.SortOrder = fieldDescription.SortOrder;
            model.Type = fieldDescription.Type.ToString();
            return model;
        }
    }
}
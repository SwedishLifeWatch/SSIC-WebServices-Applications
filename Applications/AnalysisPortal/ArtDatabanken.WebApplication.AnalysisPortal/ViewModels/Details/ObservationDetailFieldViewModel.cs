using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Details
{
    /// <summary>
    /// Observation field view model.
    /// </summary>
    public class ObservationDetailFieldViewModel
    {
        public string Class { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public int Importance { get; set; }
        public int FieldId { get; set; }

        /// <summary>
        /// Creates a observation field view model from species observation field description.
        /// </summary>
        /// <param name="speciesObservationFieldDescription">The species observation field description.</param>
        /// <returns>A observation field view model.</returns>
        public static ObservationDetailFieldViewModel CreateFromSpeciesObservationFieldDescription(
            ISpeciesObservationFieldDescription speciesObservationFieldDescription)
        {
            ObservationDetailFieldViewModel model = new ObservationDetailFieldViewModel();
            model.Class = speciesObservationFieldDescription.Class.GetName();
            model.Name = speciesObservationFieldDescription.Name;
            if (!string.IsNullOrEmpty(speciesObservationFieldDescription.Label))
            {
                model.Label = speciesObservationFieldDescription.Label;
            }
            else
            {
                model.Label = model.Name;
            }
            model.Importance = speciesObservationFieldDescription.Importance;
            model.FieldId = speciesObservationFieldDescription.Id;
            return model;
        }
    }

    /// <summary>
    /// Project parameters field view model
    /// </summary>
    /// <seealso cref="ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Details.ObservationDetailFieldViewModel" />
    public class ProjectParameterObservationDetailFieldViewModel : ObservationDetailFieldViewModel
    {
        /// <summary>
        /// Unique identifier among all species observation project parameters.
        /// </summary>
        public string PropertyIdentifier { get; set; }
        
        /// <summary>
        /// Unit for this species observation project parameter.
        /// Not defined in all species observation project parameters.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Data type for this species observation project parameter.
        /// </summary>
        public DataType Type { get; set; }

        /// <summary>
        /// Id of the Data provider.
        /// </summary>
        public int DataProviderId { get; set; }

        /// <summary>
        /// Field mapping id.
        /// </summary>        
        public int PropertyId { get; set; }

        /// <summary>
        /// Instances a new project parameter.
        /// </summary>
        /// <param name="fieldDescription">The field description to copy information from.</param>
        /// <param name="fieldMapping">The field mapping to copy information from.</param>
        /// <returns>A new <see cref="ProjectParameterObservationDetailFieldViewModel"/> object.</returns>
        public static ProjectParameterObservationDetailFieldViewModel Create(
            ISpeciesObservationFieldDescription fieldDescription,
            ISpeciesObservationFieldMapping fieldMapping)
        {
            ProjectParameterObservationDetailFieldViewModel field = new ProjectParameterObservationDetailFieldViewModel();            
            field.Label = fieldDescription.Label;
            field.Name = fieldDescription.Name;
            field.Class = "Project"; //fieldDescription.Class.Identifier
            field.Importance = fieldDescription.Importance;
            field.Type = fieldDescription.Type;
            field.FieldId = fieldDescription.Id;
            field.PropertyId = fieldMapping.Id;
            field.PropertyIdentifier = fieldMapping.PropertyIdentifier;
            field.DataProviderId = fieldMapping.DataProviderId;
            //field.Unit = ? // just exists in SpeciesObservation class.
            return field;
        }

        /// <summary>
        /// Instances a new project parameter object.
        /// </summary>
        /// <param name="projectParameter">The project parameter to copy information from.</param>
        /// <returns>A new <see cref="ProjectParameterObservationDetailFieldViewModel"/> object.</returns>
        internal static ProjectParameterObservationDetailFieldViewModel Create(ISpeciesObservationProjectParameter projectParameter)
        {
            var field = new ProjectParameterObservationDetailFieldViewModel
            {
                Value = projectParameter.Value,
                Label = projectParameter.Property,
                Name = projectParameter.Property,
                Class = "Project",
                PropertyIdentifier = projectParameter.PropertyIdentifier,
                Type = projectParameter.Type,
                Unit = projectParameter.Unit
            };
            //field.Importance = ?
            return field;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Value))
            {
                return string.Format("Label: {0}, Value: {1}", Label, Value);
            }
            else
            {
                return string.Format("Label: {0}", Label);
            }            
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A copy of this instance.</returns>
        public ProjectParameterObservationDetailFieldViewModel Clone()
        {
            ProjectParameterObservationDetailFieldViewModel other =
                (ProjectParameterObservationDetailFieldViewModel)this.MemberwiseClone();
            return other;
        }
    }
}
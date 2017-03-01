using System;
using System.Collections.Generic;
using ArtDatabanken;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Database;

namespace ArtDatabanken.WebService.SpeciesObservation.Data
{
    /// <summary>
    /// An instance of this class contains the
    /// value for one species observation project parameter.
    /// </summary>
    public class WebSpeciesObservationFieldProjectParameter : WebSpeciesObservationField
    {
        /// <summary>
        /// Id for the species observation that this project parameter belongs to.
        /// </summary>
        public Int64 SpeciesObservationId { get; set; }

        /// <summary>
        /// Convert a WebSpeciesObservationFieldProjectParameter instance into
        /// a WebSpeciesObservationField instance.
        /// </summary>
        /// <returns>A WebSpeciesObservationField instance.</returns>
        public WebSpeciesObservationField GetSpeciesObservationField()
        {
            WebSpeciesObservationField field;

            field = new WebSpeciesObservationField();
            field.ClassIdentifier = ClassIdentifier;
            field.PropertyIdentifier = PropertyIdentifier;
            field.DataFields = DataFields;
            field.Type = Type;
            field.Value = Value;
            return field;
        }

        /// <summary>
        /// Populate species observation project parameter with content from data reader.
        /// </summary>
        /// <param name="dataReader">Data source that will populate the species observation project parameter.</param>
        public void LoadData(DataReader dataReader)
        {
            WebDataField dataField;

            ClassIdentifier = dataReader.GetString(SpeciesObservationProjectParameterData.CLASS_IDENTIFIER);
            PropertyIdentifier = dataReader.GetString(SpeciesObservationProjectParameterData.PROPERTY_IDENTIFIER);
            SpeciesObservationId = dataReader.GetInt64(SpeciesObservationProjectParameterData.SPECIES_OBSERVATION_ID);
            Type = (WebDataType)(dataReader.GetInt32(SpeciesObservationProjectParameterData.DATA_TYPE));
            if (dataReader.IsNotDbNull(SpeciesObservationProjectParameterData.UNIT))
            {
                if (DataFields.IsNull())
                {
                    DataFields = new List<WebDataField>();
                }

                dataField = new WebDataField();
                dataField.Name = Settings.Default.WebDataUnit;
                dataField.Type = WebDataType.String;
                dataField.Value = dataReader.GetString(SpeciesObservationProjectParameterData.UNIT);
                if (dataField.Value.IsNotEmpty())
                {
                    DataFields.Add(dataField);
                }
            }

            Value = dataReader.GetString(SpeciesObservationProjectParameterData.VALUE);
        }
    }
}

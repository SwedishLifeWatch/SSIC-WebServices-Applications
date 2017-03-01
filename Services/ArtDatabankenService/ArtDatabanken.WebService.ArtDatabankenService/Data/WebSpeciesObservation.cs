using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Is used to decide which information that should be
    /// returned about species observation.
    /// </summary>
    [DataContract]
    public enum SpeciesObservationInformationType
    {
        /// <summary>
        /// Only id.
        /// </summary>
        [EnumMember]
        Id,

        /// <summary>
        /// All available information.
        /// </summary>
        [EnumMember]
        All
    }

    /// <summary>
    /// Contains information about an observation of a species.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservation : WebData
    {
        /// <summary>
        /// Create a WebSpeciesObservation instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebSpeciesObservation(DataReader dataReader)
        {
            WebDataField guid;

            Id = dataReader.GetInt64(SpeciesObservationData.ID);
            LoadData(dataReader);

            // Create GUID.
            guid = new WebDataField();
            guid.Name = SpeciesObservationData.GUID;
            guid.Type = WebDataType.String;
            guid.Value += GetGuid(dataReader);
            DataFields.Add(guid);
            CoordinateX = dataReader.GetInt32(SpeciesObservationData.COORDINATE_X);
            CoordinateY = dataReader.GetInt32(SpeciesObservationData.COORDINATE_Y);
            DyntaxaTaxonId = dataReader.GetInt32(SpeciesObservationData.TAXON_ID);
            ProtectionLevel = dataReader.GetInt32(SpeciesObservationData.PROTECTION_LEVEL);
        }

        /// <summary>
        /// Coordinate X.
        /// </summary>
        public Int32 CoordinateX { get; set; }

        /// <summary>
        /// Coordinate Y.
        /// </summary>
        public Int32 CoordinateY { get; set; }

        /// <summary>
        /// Taxon id according to Dyntaxa.
        /// </summary>
        public Int32 DyntaxaTaxonId { get; set; }

        /// <summary>
        /// Id for this species observation.
        /// </summary>
        [DataMember]
        public Int64 Id { get; set; }

        /// <summary>
        /// Protection level.
        /// </summary>
        public Int32 ProtectionLevel { get; set; }

        /// <summary>
        /// Get GUID for species observation.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        /// <returns>GUID for species observation.</returns>
        public static String GetGuid(DataReader dataReader)
        {
            Int64 originalSpeciesObservationId;
            String database;
            String guid;

            database = dataReader.GetString(SpeciesObservationData.DATABASE);
            originalSpeciesObservationId = dataReader.GetInt32(SpeciesObservationData.ORIGINAL_SPECIES_OBSERVATION_ID);
            switch (database)
            {
                case "Artportalen":
                    // Artportalen 2.
                    guid = "urn:lsid:artportalen.se:Sighting:";
                    break;
                case "Fisk":
                    // Artportalen 1.
                    guid = "urn:lsid:artportalen.se:Sighting:Fish.";
                    break;
                case "Fåglar":
                    // Artportalen 1.
                    guid = "urn:lsid:artportalen.se:Sighting:Bird.";
                    break;
                case "Marina evertebrater":
                    // Artportalen 1.
                    guid = "urn:lsid:artportalen.se:Sighting:MarineInvertebrates.";
                    break;
                case "Obsdatabasen":
                    guid = "urn:lsid:artdata.slu.se:SpeciesObservation:";
                    break;
                case "Småkryp":
                    // Artportalen 1.
                    guid = "urn:lsid:artportalen.se:Sighting:Bugs.";
                    break;
                case "Vertebrat":
                    // Artportalen 1.
                    guid = "urn:lsid:artportalen.se:Sighting:Vertebrate.";
                    break;
                case "Växter, svampar":
                    // Artportalen 1.
                    guid = "urn:lsid:artportalen.se:Sighting:PlantAndMushroom.";
                    break;
                default:
                    throw new ApplicationException("Unhandled species observation source " + database);
            }

            guid += originalSpeciesObservationId;
            return guid;
        }
    }
}

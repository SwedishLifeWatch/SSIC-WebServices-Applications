using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.AnalysisService.Database;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.AnalysisService.Data
{
    /// <summary>
    /// Contains extension methods for class WebSpeciesObservationProvenance.
    /// </summary>
    public static class WebSpeciesObservationProvenanceExtension
    {
        public static void LoadData(this WebSpeciesObservationProvenance speciesObservationProvenance, DataReader dataReader)
        {
            WebSpeciesObservationProvenanceValue speciesObservationProvenanceValue;

            if (speciesObservationProvenance != null && dataReader != null)
            {
                speciesObservationProvenance.Name = dataReader.GetString(SpeciesObservationProvenanceSearchCriteriaData.NAME);

                speciesObservationProvenanceValue = new WebSpeciesObservationProvenanceValue();
                if (dataReader.IsDbNull(SpeciesObservationProvenanceSearchCriteriaData.ID))
                {
                    speciesObservationProvenanceValue.Id = null;
                }
                else
                {
                    speciesObservationProvenanceValue.Id = dataReader.GetInt32(SpeciesObservationProvenanceSearchCriteriaData.ID, 0).WebToString();
                }

                speciesObservationProvenanceValue.SpeciesObservationCount = dataReader.GetInt64(SpeciesObservationProvenanceSearchCriteriaData.SPECIES_OBSERVATION_COUNT);
                speciesObservationProvenanceValue.Value = dataReader.GetString(SpeciesObservationProvenanceSearchCriteriaData.VALUE);
                speciesObservationProvenance.Values.Add(speciesObservationProvenanceValue);
            }
        }
    }
}

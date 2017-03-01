using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Contains information about changes in species observations.
    /// This class has three different types of changes.
    /// Deleted species observations.
    /// New species observations.
    /// Updated species observations.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationChange
    {
        /// <summary>
        /// Create a WebSpeciesObservationChange instance.
        /// </summary>
        public WebSpeciesObservationChange()
        {
        }

        /// <summary>
        /// Create a WebSpeciesObservationChange instance.
        /// </summary>
        /// <param name='dataReader'>An open data reader.</param>
        public WebSpeciesObservationChange(DataReader dataReader)
        {
            WebSpeciesObservation speciesObservation;

            DeletedSpeciesObservationGuids = new List<String>();
            DeletedSpeciesObservationCount = 0;
            MaxSpeciesObservationCount = Settings.Default.MaxSpeciesObservationWithInformation;
            NewSpeciesObservationCount = 0;
            NewSpeciesObservationIds = null;
            NewSpeciesObservations = new List<WebSpeciesObservation>();
            NewSpeciesObservationCount = 0;
            UpdatedSpeciesObservationCount = 0;
            UpdatedSpeciesObservationIds = null;
            UpdatedSpeciesObservations = new List<WebSpeciesObservation>();

            // Get new species observations.
            while (dataReader.Read())
            {
                NewSpeciesObservationCount++;
                if (NewSpeciesObservationCount <= MaxSpeciesObservationCount)
                {
                    // Add species observation with information.
                    speciesObservation = new WebSpeciesObservation(dataReader);
                    NewSpeciesObservations.Add(speciesObservation);
                }
                else
                {
                    if (NewSpeciesObservationCount == (MaxSpeciesObservationCount + 1))
                    {
                        // To many species observations.
                        // Return only species observation ids.
                        // Move species observation ids from 
                        // SpeciesObservations to SpeciesObservationIds.
                        NewSpeciesObservationIds = new List<Int64>();
                        foreach (WebSpeciesObservation speciesObservationTemp in NewSpeciesObservations)
                        {
                            NewSpeciesObservationIds.Add(speciesObservationTemp.Id);
                        }
                        NewSpeciesObservations = null;
                    }

                    // Add only species observation id.
                    NewSpeciesObservationIds.Add(dataReader.GetInt64(SpeciesObservationData.ID));
                }

                if (NewSpeciesObservationCount > Settings.Default.MaxSpeciesObservation)
                {
                    throw new ApplicationException("To many new species observations was retrieved!, Limit is set to " + Settings.Default.MaxSpeciesObservation + " observations.");
                }
            }

            if (!dataReader.NextResultSet())
            {
                throw new ApplicationException("Failed to read updated species observation.");
            }

            // Get updated species observations.
            while (dataReader.Read())
            {
                UpdatedSpeciesObservationCount++;
                if (UpdatedSpeciesObservationCount <= MaxSpeciesObservationCount)
                {
                    // Add species observation with information.
                    speciesObservation = new WebSpeciesObservation(dataReader);
                    UpdatedSpeciesObservations.Add(speciesObservation);
                }
                else
                {
                    if (UpdatedSpeciesObservationCount == (MaxSpeciesObservationCount + 1))
                    {
                        // To many species observations.
                        // Return only species observation ids.
                        // Move species observation ids from 
                        // SpeciesObservations to SpeciesObservationIds.
                        UpdatedSpeciesObservationIds = new List<Int64>();
                        foreach (WebSpeciesObservation speciesObservationTemp in NewSpeciesObservations)
                        {
                            UpdatedSpeciesObservationIds.Add(speciesObservationTemp.Id);
                        }
                        UpdatedSpeciesObservations = null;
                    }

                    // Add only species observation id.
                    UpdatedSpeciesObservationIds.Add(dataReader.GetInt64(SpeciesObservationData.ID));
                }

                if (UpdatedSpeciesObservationCount > Settings.Default.MaxSpeciesObservation)
                {
                    throw new ApplicationException("To many updated species observations was retrieved!, Limit is set to " + Settings.Default.MaxSpeciesObservation + " observations.");
                }
            }

            if (!dataReader.NextResultSet())
            {
                throw new ApplicationException("Failed to read deleted species observation.");
            }

            // Get deleted species observations.
            while (dataReader.Read())
            {
                DeletedSpeciesObservationCount++;

                // Add species observation GUID.
                DeletedSpeciesObservationGuids.Add(WebSpeciesObservation.GetGuid(dataReader));

                if (DeletedSpeciesObservationCount > Settings.Default.MaxSpeciesObservation)
                {
                    throw new ApplicationException("To many deleted species observations was retrieved!, Limit is set to " + Settings.Default.MaxSpeciesObservation + " observations.");
                }
            }
        }

        /// <summary>
        /// Number of deleted species observations that are
        /// returned in this response.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Int64 DeletedSpeciesObservationCount
        { get; set; }

        /// <summary>
        /// GUIDs for deleted species observations.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public List<String> DeletedSpeciesObservationGuids
        { get; set; }

        /// <summary>
        /// Max number of species observations (with information)
        /// that are returned as new or updated in a single web service
        /// call. It may be up to max number of species observations
        /// of each change type (new or updated).
        /// </summary>
        [DataMember]
        public Int64 MaxSpeciesObservationCount
        { get; set; }

        /// <summary>
        /// Number of new species observations that are
        /// returned in this response.
        /// </summary>
        [DataMember]
        public Int64 NewSpeciesObservationCount
        { get; set; }

        /// <summary>
        /// If NewSpeciesObservationCount is greater than
        /// MaxSpeciesObservationCount then only ids for
        /// species observations are returned in this property.
        /// </summary>
        [DataMember]
        public List<Int64> NewSpeciesObservationIds
        { get; set; }

        /// <summary>
        /// If NewSpeciesObservationCount is less or equal to
        /// MaxSpeciesObservationCount then species observations
        /// are returned in this property.
        /// </summary>
        [DataMember]
        public List<WebSpeciesObservation> NewSpeciesObservations
        { get; set; }

        /// <summary>
        /// Number of updated species observations that are
        /// returned in this response.
        /// </summary>
        [DataMember]
        public Int64 UpdatedSpeciesObservationCount
        { get; set; }

        /// <summary>
        /// If UpdatedSpeciesObservationCount is greater than
        /// MaxSpeciesObservationCount then only ids for
        /// species observations are returned in this property.
        /// </summary>
        [DataMember]
        public List<Int64> UpdatedSpeciesObservationIds
        { get; set; }

        /// <summary>
        /// If UpdatedSpeciesObservationCount is less or equal to
        /// MaxSpeciesObservationCount then species observations
        /// are returned in this property.
        /// </summary>
        [DataMember]
        public List<WebSpeciesObservation> UpdatedSpeciesObservations
        { get; set; }
    }
}

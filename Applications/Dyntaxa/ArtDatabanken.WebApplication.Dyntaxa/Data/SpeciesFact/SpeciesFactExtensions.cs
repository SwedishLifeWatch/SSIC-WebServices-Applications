using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// Species fact extensions.
    /// </summary>
    public static class SpeciesFactExtensions
    {
        /// <summary>
        /// Gets the status identifier.
        /// </summary>
        /// <param name="speciesFact">The species fact.</param>
        /// <returns>Species fact status id; otherwise null.</returns>
        public static Int32? GetStatusId(this ISpeciesFact speciesFact)
        {
            if (speciesFact == null)
            {
                return null;
            }

            FactorFieldEnumValue val = speciesFact.MainField.Value as FactorFieldEnumValue;            
            if (val != null)
            {                
                return val.Id;
            }

            return null;
        }

        /// <summary>
        /// Sets the status for a species fact.
        /// </summary>
        /// <param name="speciesFact">The species fact to change.</param>
        /// <param name="statusId">The new status Id.</param>
        public static void SetStatus(this ISpeciesFact speciesFact, int? statusId)
        {                        
            int? currentStatusId = GetStatusId(speciesFact);
            if (currentStatusId.HasValue && currentStatusId.Value == statusId)
            {
                return;
            }

            IFactorFieldEnumValue factorFieldEnumValue = speciesFact.MainField.FactorFieldEnum.Values.FirstOrDefault(enumValue => statusId == enumValue.Id);
            if (factorFieldEnumValue != null)
            {
                speciesFact.MainField.Value = factorFieldEnumValue;
            }            
        }

        /// <summary>
        /// Gets the status label.
        /// </summary>
        /// <param name="speciesFact">The species fact.</param>
        /// <returns>Species fact status label; otherwise null.</returns>
        public static String GetStatusLabel(this ISpeciesFact speciesFact)
        {
            if (speciesFact == null)
            {
                return null;
            }

            FactorFieldEnumValue val = speciesFact.MainField.Value as FactorFieldEnumValue;
            if (val != null)
            {
                return val.Label;
            }

            return null;
        }

        /// <summary>
        /// Gets the status original label.
        /// </summary>
        /// <param name="speciesFact">The species fact.</param>
        /// <returns>Species fact status original label; otherwise null.</returns>
        public static String GetStatusOriginalLabel(this ISpeciesFact speciesFact)
        {
            if (speciesFact == null)
            {
                return null;
            }

            FactorFieldEnumValue val = speciesFact.MainField.Value as FactorFieldEnumValue;
            if (val != null)
            {
                return val.OriginalLabel;
            }

            return null;
        }

        /// <summary>
        /// Gets the name of the quality.
        /// </summary>
        /// <param name="speciesFact">The species fact.</param>
        /// <returns>Species fact quality name; otherwise null.</returns>
        public static String GetQualityName(this ISpeciesFact speciesFact)
        {
            if (speciesFact == null)
            {
                return null;
            }

            ISpeciesFactQuality val = speciesFact.Quality;
            if (val != null)
            {
                return val.Name;
            }

            return null;
        }

        /// <summary>
        /// Gets the quality identifier.
        /// </summary>
        /// <param name="speciesFact">The species fact.</param>
        /// <returns>Species fact quality identifier; otherwise null.</returns>
        public static Int32? GetQualityId(this ISpeciesFact speciesFact)
        {
            if (speciesFact == null)
            {
                return null;
            }

            ISpeciesFactQuality val = speciesFact.Quality;
            if (val != null)
            {
                return val.Id;
            }

            return null;
        }

        /// <summary>
        /// Sets quality for a species fact.
        /// </summary>
        /// <param name="speciesFact">The species fact.</param>
        /// <param name="userContext">The user context.</param>
        /// <param name="qualityId">The quality id.</param>
        public static void SetQuality(this ISpeciesFact speciesFact, IUserContext userContext, int? qualityId)
        {            
            int? currentQualityId = GetQualityId(speciesFact);
            if (currentQualityId.HasValue && currentQualityId.Value == qualityId)
            {
                return;
            }
            if (!qualityId.HasValue)
            {
                speciesFact.Quality = null;
                return;
            }

            ISpeciesFactQuality speciesFactQuality = CoreData.SpeciesFactManager.GetSpeciesFactQuality(userContext, qualityId.Value);                                
            if (speciesFactQuality != null)
            {
                speciesFact.Quality = speciesFactQuality;
            }
        }

        /// <summary>
        /// Gets the name of the reference.
        /// </summary>
        /// <param name="speciesFact">The species fact.</param>
        /// <returns>Species fact reference name; otherwise null</returns>
        public static String GetReferenceName(this ISpeciesFact speciesFact)
        {
            if (speciesFact == null)
            {
                return null;
            }

            IReference val = speciesFact.Reference;
            if (val != null)
            {
                return val.Name;
            }

            return null;
        }

        /// <summary>
        /// Gets the reference identifier.
        /// </summary>
        /// <param name="speciesFact">The species fact.</param>
        /// <returns>Species fact reference id; otherwise null</returns>
        public static Int32? GetReferenceId(this ISpeciesFact speciesFact)
        {
            if (speciesFact == null)
            {
                return null;
            }

            IReference val = speciesFact.Reference;
            if (val != null)
            {
                return val.Id;                
            }

            return null;            
        }

        /// <summary>
        /// Sets reference for a species fact.
        /// </summary>
        /// <param name="speciesFact">The species fact.</param>
        /// <param name="userContext">The user context.</param>
        /// <param name="referenceId">The reference id.</param>
        public static void SetReference(this ISpeciesFact speciesFact, IUserContext userContext, int? referenceId)
        {
            int? currentReferenceId = GetReferenceId(speciesFact);
            if (currentReferenceId.HasValue && currentReferenceId.Value == referenceId)
            {
                return;
            }
            if (!referenceId.HasValue)
            {
                speciesFact.Reference = null;
                return;
            }

            speciesFact.Reference = CoreData.ReferenceManager.GetReference(userContext, referenceId.Value);            
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <param name="speciesFact">The species fact.</param>
        /// <returns>Species fact description; othwerwise null.</returns>
        public static string GetDescription(this ISpeciesFact speciesFact)
        {
            string val = speciesFact == null || speciesFact.Field5 == null ? null : speciesFact.Field5.StringValue;
            if (val != null)
            {
                return val;
            }

            return string.Empty;
        }

        /// <summary>
        /// Sets comment value for a species fact.
        /// </summary>
        /// <param name="speciesFact">The species fact.</param>
        /// <param name="comment">The comment.</param>
        public static void SetDescription(this ISpeciesFact speciesFact, string comment)
        {
            string currentComment = GetDescription(speciesFact);
            if (currentComment == comment)
            {
                return;
            }

            speciesFact.Field5.StringValue = comment;            
        }

        /// <summary>
        /// Determines whether status identifier has changed.
        /// </summary>
        /// <param name="speciesFact">The species fact.</param>
        /// <returns>true if status id has changed; othwerwise null.</returns>
        public static bool HasStatusIdChanged(this ISpeciesFact speciesFact)
        {
            if (speciesFact == null)
            {
                return false;
            }

            return speciesFact.MainField.HasChanged;            
        }
    }

    /// <summary>
    /// Species fact change description.
    /// </summary>
    public struct DyntaxaSpeciesFactChangeDescription
    {
        public String OldValues { get; set; }
        public String NewValues { get; set; }
    }

    /// <summary>
    /// Dyntaxa revision species fact change unit.
    /// Consists of RevisionEvent data and Revision species fact data.
    /// </summary>
    public struct DyntaxaRevisionSpeciesFactChangeUnit
    {
        public DyntaxaRevisionSpeciesFact DyntaxaRevisionSpeciesFact { get; set; }
        public TaxonRevisionEvent TaxonRevisionEvent { get; set; }
    }

    /// <summary>
    /// Dyntaxa revision species fact change status.
    /// </summary>
    public enum DyntaxaRevisionSpeciesFactChangeStatus
    {
        /// <summary>
        /// No changes is made.
        /// </summary>
        NoChanges,

        /// <summary>
        /// No previous changes in this revision for this species fact is made.
        /// This is the first change.
        /// </summary>
        ChangedFromOriginalSpeciesFact,

        /// <summary>
        /// A previous change in this revision for this species fact exists.        
        /// </summary>
        ChangedFromDyntaxaRevisionSpeciesFact,        
    }
}

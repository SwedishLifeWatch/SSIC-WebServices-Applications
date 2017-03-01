using System;
using System.Collections.Generic;
using ArtDatabanken;
using ArtDatabanken.Data;
//using RedList.Data.Extensions;
//using RedList.Data.Helpers;
//using RedList.Data.Managers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    /// <summary>
    /// This class handles caching of information for a taxon
    /// that is presented in taxon list view.
    /// </summary>
    [Serializable]
    public class TaxonListInformation
    {
        /// <summary>
        /// Get common name for taxon.
        /// </summary>
        public string CommonName { get; set; }

        /// <summary>
        /// Id for the taxon.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Information about red list status. Sets if taxon has factor 743/redlisted and is not of type 
        // LC/NE/NA.
        /// </summary>
        public bool IsRedListed { get; set; }

        /// <summary>
        /// Information about red list status. Sets if taxon has factor 743/redlisted.
        /// </summary>
        public bool IsRedListedEnsured { get; set; }

        /// <summary>
        /// Information about landscape type occurrence for taxon.
        /// </summary>
        public string LandscapeTypeOccurrence { get; set; }

        /// <summary>
        /// Information about organism group for taxon.
        /// </summary>
        public string OrganismGroup { get; set; }

        /// <summary>
        /// Information about red list category for taxon.
        /// </summary>
        public string RedListCategory { get; set; }

        /// <summary>
        /// Id for red list category applied to taxon.
        /// </summary>
        public int RedListCategoryId { get; set; }

        /// <summary>
        /// Information about red list criteria for taxon.
        /// </summary>
        public string RedListCriteria { get; set; }

        /// <summary>
        /// Get scientific name for taxon.
        /// </summary>
        public string ScientificName { get; set; }

        /// <summary>
        /// Information about swedish occurrence for taxon.
        /// </summary>
        public string SwedishOccurrence { get; set; }

        /// <summary>
        /// Id for swedish occurrence applied to taxon.
        /// </summary>
        public int SwedishOccurrenceId { get; set; }

        /// <summary>
        /// Information about older values for red list criteria for taxon.
        /// </summary>
        public Dictionary<string, string> PreviousRedListCriteria { get; set; }

        /// <summary>
        /// Category id
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Parent cageory id
        /// </summary>
        public int ParentCategoryId { get; set; }

        /// <summary>
        /// Set species fact information about this taxon.
        /// </summary>
        /// <param name="speciesFact">Species fact with red list information.</param>
        public void SetSpeciesFact(ISpeciesFact speciesFact)
        {
            if (LandscapeTypeManager.LandscapeTypeFactors.IsNotNull() &&
                LandscapeTypeManager.LandscapeTypeFactors.Exists(speciesFact.Factor))
            {
                if (speciesFact.IsLandscapeTypeOccurrenceSpecified())
                {
                    string landscapeTypeOccurrence = speciesFact.Factor.Label;
                    landscapeTypeOccurrence = landscapeTypeOccurrence.Substring(landscapeTypeOccurrence.Length - 2, 1);
                    switch (speciesFact.MainField.EnumValue.KeyInt)
                    {
                        case (int)LandscapeTypeImportanceEnum.HasImportance:
                            LandscapeTypeOccurrence += landscapeTypeOccurrence.ToLower();
                            break;
                        case (int)LandscapeTypeImportanceEnum.VeryImportant:
                            LandscapeTypeOccurrence += landscapeTypeOccurrence.ToUpper();
                            break;
                    }
                }
            }
            else
            {
                switch (speciesFact.Factor.Id)
                {
                    case (int)FactorId.RedlistCategory:
                        if (speciesFact.IsRedlistCategorySpecified())
                        {
                            RedListCategory = speciesFact.Field1.EnumValue.OriginalLabel.Substring(0, speciesFact.Field1.EnumValue.OriginalLabel.Length - 4) +
                                              "(" + speciesFact.MainField.StringValue + ")";
                            var category = (RedListCategory)Enum.Parse(typeof(RedListCategory), RedListCategory.Substring(RedListCategory.IndexOf("(", StringComparison.Ordinal) + 1, 2));
                            IsRedListed = RedListedHelper.IsRedListedDdToNt((int)category);
                            IsRedListedEnsured = RedListedHelper.IsRedListedDdToNe((int)category);
                            RedListCategoryId = Convert.ToInt32(speciesFact.Field1.EnumValue.KeyInt);
                        }

                        break;

                    case (int)FactorId.RedlistCriteriaString:
                        if (speciesFact.IsRedlistCriteriaSpecified())
                        {
                            RedListCriteria = speciesFact.MainField.StringValue;
                        }

                        break;

                    case (int)FactorId.Redlist_OrganismLabel1:
                        if (speciesFact.IsOrganismGroupSpecified())
                        {
                            if (OrganismGroup.IsEmpty())
                            {
                                OrganismGroup = speciesFact.MainField.EnumValue.OriginalLabel;
                            }
                            else
                            {
                                if (OrganismGroup != speciesFact.MainField.EnumValue.OriginalLabel)
                                {
                                    OrganismGroup = speciesFact.MainField.EnumValue.OriginalLabel + ", " + OrganismGroup;
                                }
                            }
                        }

                        break;
                    case (int)FactorId.SwedishOccurrence:
                        if (speciesFact.IsSwedishOccurrenceSpecified())
                        {
                            SwedishOccurrence = speciesFact.MainField.EnumValue.OriginalLabel;
                            SwedishOccurrenceId = Convert.ToInt32(speciesFact.MainField.EnumValue.KeyInt);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Set species fact information about this taxon regarding previously red listed facts.
        /// </summary>
        /// <param name="speciesFact">Species fact with red list information.</param>
        /// <param name="currentPeriod">Actual red list period.</param>
        public void SetSpeciesFact(ISpeciesFact speciesFact, IPeriod currentPeriod)
        {
            if (speciesFact.Factor.Id == (int)FactorId.RedlistCategory && speciesFact.Period.Id != currentPeriod.Id)
            {
                if (speciesFact.IsOldRedlistCategorySpecified())
                {
                    if (PreviousRedListCriteria.IsNull())
                    {
                        PreviousRedListCriteria = new Dictionary<string, string>();
                    }
                    var redListCategory = speciesFact.Field1.EnumValue.OriginalLabel.Substring(0, speciesFact.Field1.EnumValue.OriginalLabel.Length - 4) +
                                              "(" + speciesFact.MainField.StringValue + ")";
                    PreviousRedListCriteria.Add(speciesFact.Period.Name, redListCategory);
                }
            }
        }
    }
}

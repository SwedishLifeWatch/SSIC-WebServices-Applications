using System.Diagnostics.CodeAnalysis;
using ArtDatabanken;
using ArtDatabanken.Data;
//using RedList.Data.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    /// <summary>
    ///     Contains extension methods to the SpeciesFactExtension class.
    /// </summary>
    public static class SpeciesFactExtension
    {
        /// <summary>
        ///     Test if action plan is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with action plan information.</param>
        /// <returns>True if action plan is specified.</returns>
        public static bool IsActionPlanSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if biotope is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with biotope information.</param>
        /// <returns>True if biotope is specified.</returns>
        public static bool IsBiotopeSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if continuous decline is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with continuous decline information.</param>
        /// <param name="period">Redlisting period.</param>
        /// <returns>True if continuous decline is specified.</returns>
        public static bool IsContinuousDeclineSpecified(this ISpeciesFact speciesFact, IPeriod period)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue
                    && speciesFact.MainField.EnumValue.IsNotNull()
                    && (speciesFact.MainField.EnumValue.KeyInt >= (int)ContinuousDeclineEnum.GoesOn)
                    && (speciesFact.MainField.EnumValue.KeyInt <= (int)ContinuousDeclineEnum.DeclinesMoreThan25)
                    && speciesFact.HasPeriod && speciesFact.Period.Year == period.Year;
        }

        /// <summary>
        ///     Test if convention is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with convention information.</param>
        /// <returns>True if convention is specified.</returns>
        public static bool IsConventionSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if county occurrence is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with county occurrence information.</param>
        /// <returns>True if county occurrence is specified.</returns>
        public static bool IsCountyOccurrenceSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.Field1.IsNotNull() && speciesFact.Field1.HasValue;
        }

        /// <summary>
        ///     Test if extreme fluctuations is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with extreme fluctuations information.</param>
        /// <param name="period">Redlisting period.</param>
        /// <returns>True if extreme fluctuations is specified.</returns>
        public static bool IsExtremeFluctuationsSpecified(this ISpeciesFact speciesFact, IPeriod period)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue
                    && speciesFact.MainField.EnumValue.KeyInt >= (int)ExtremeFluctuationsEnum.CanOccur
                    && speciesFact.HasPeriod && speciesFact.Period.Year == period.Year;
        }

        /// <summary>
        ///     Test if global redlist category is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with global redlist category information.</param>
        /// <returns>True if global redlist category is specified.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static bool IsGlobalRedlistCategorySpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if impact is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with impact information.</param>
        /// <returns>True if impact is specified.</returns>
        public static bool IsImpactSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if landscape type occurrence is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with landscape type occurrence information.</param>
        /// <returns>True if landscape type occurrence is specified.</returns>
        public static bool IsLandscapeTypeOccurrenceSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue
                    && (speciesFact.MainField.EnumValue.KeyInt >= (int)LandscapeTypeImportanceEnum.HasImportance)
                    && (speciesFact.MainField.EnumValue.KeyInt <= (int)LandscapeTypeImportanceEnum.VeryImportant);
        }

        /// <summary>
        ///     Test if life form is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with landscape type occurrence information.</param>
        /// <returns>True if life form is specified.</returns>
        public static bool IsLifeFormSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if organism as substrate is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with substrate information.</param>
        /// <returns>True if organism as substrate is specified.</returns>
        public static bool IsOrganismAsSubstrateSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.HasHost && (speciesFact.Host.Id != (int)TaxonId.Life)
                    && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue
                    && (speciesFact.MainField.EnumValue.KeyInt >= (int)OrganismAsSubstrateImportanceEnum.HasImportance)
                    && (speciesFact.MainField.EnumValue.KeyInt <= (int)OrganismAsSubstrateImportanceEnum.VeryImportant);
        }

        /// <summary>
        ///     Test if organism group is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with organism group information.</param>
        /// <returns>True if organism group is specified.</returns>
        public static bool IsOrganismGroupSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if protected by law is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with protected by law information.</param>
        /// <returns>True if protected by law is specified.</returns>
        public static bool IsProtectedByLawSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if redlist category is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with redlist category information.</param>
        /// <returns>True if redlist category is specified.</returns>
        public static bool IsRedlistCategorySpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.Field1.IsNotNull() && speciesFact.Field1.HasValue
                    && RedListedHelper.IsRedListedDdToNe(speciesFact.Field1.EnumValue.KeyInt)
                    && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if redlist criteria is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with redlist criteria information.</param>
        /// <returns>True if redlist criteria is specified.</returns>
        public static bool IsRedlistCriteriaSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if redlist documentation is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with redlist documentation information.</param>
        /// <returns>True if redlist documentation is specified.</returns>
        public static bool IsRedlistDocumentationSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if redlist taxon type is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with redlist taxon type information.</param>
        /// <returns>True if redlist taxon type is specified.</returns>
        public static bool IsRedListTaxonTypeSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if reduction A1 is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with reduction A1 information.</param>
        /// <param name="period">Redlisting period.</param>
        /// <returns>True if reduction A1 is specified.</returns>
        public static bool IsReductionA1Specified(this ISpeciesFact speciesFact, IPeriod period)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue
                    && speciesFact.MainField.DoubleValue > 0 && speciesFact.HasPeriod
                    && speciesFact.Period.Year == period.Year;
        }

        /// <summary>
        ///     Test if reduction A2 is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with reduction A2 information.</param>
        /// <param name="period">Redlisting period.</param>
        /// <returns>True if reduction A2 is specified.</returns>
        public static bool IsReductionA2Specified(this ISpeciesFact speciesFact, IPeriod period)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue
                    && speciesFact.MainField.DoubleValue > 0 && speciesFact.HasPeriod
                    && speciesFact.Period.Year == period.Year;
        }

        /// <summary>
        ///     Test if reduction A3 is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with reduction A3 information.</param>
        /// <param name="period">Redlisting period.</param>
        /// <returns>True if reduction A3 is specified.</returns>
        public static bool IsReductionA3Specified(this ISpeciesFact speciesFact, IPeriod period)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue
                    && speciesFact.MainField.DoubleValue > 0 && speciesFact.HasPeriod
                    && speciesFact.Period.Year == period.Year;
        }

        /// <summary>
        ///     Test if reduction A4 is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with reduction A4 information.</param>
        /// <param name="period">Redlisting period.</param>
        /// <returns>True if reduction A4 is specified.</returns>
        public static bool IsReductionA4Specified(this ISpeciesFact speciesFact, IPeriod period)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue
                    && speciesFact.MainField.DoubleValue > 0 && speciesFact.HasPeriod
                    && speciesFact.Period.Year == period.Year;
        }

        /// <summary>
        ///     Test if severely fragmented is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with severely fragmented information.</param>
        /// <param name="period">Redlisting period.</param>
        /// <returns>True if severely fragmented is specified.</returns>
        public static bool IsSeverelyFragmentedSpecified(this ISpeciesFact speciesFact, IPeriod period)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue
                    && speciesFact.MainField.EnumValue.KeyInt >= (int)SeverelyFragementedEnum.CanBeFragmented
                    && speciesFact.HasPeriod && speciesFact.Period.Year == period.Year;
        }

        /// <summary>
        ///     Test if redlist criteria is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with redlist criteria information.</param>
        /// <param name="period">Redlisting period.</param>
        /// <returns>True if redlist criteria is specified.</returns>
        public static bool IsSmallPopulationOrDistributionSpecified(this ISpeciesFact speciesFact, IPeriod period)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue
                    && (speciesFact.MainField.StringValue.Contains("B")
                        || speciesFact.MainField.StringValue.Contains("C")
                        || speciesFact.MainField.StringValue.Contains("D")) && speciesFact.HasPeriod
                    && speciesFact.Period.Year == period.Year;
        }

        /// <summary>
        ///     Test if species information document author and year is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with species information document author and year information.</param>
        /// <returns>True if species information document author and year is specified.</returns>
        public static bool IsSpeciesInformationDocumentAuthorAndYearSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if species information document description is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with species information document description information.</param>
        /// <returns>True if species information document description is specified.</returns>
        public static bool IsSpeciesInformationDocumentDescriptionSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if species information document distribution is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with species information document distribution information.</param>
        /// <returns>True if species information document distribution is specified.</returns>
        public static bool IsSpeciesInformationDocumentDistributionSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if species information document ecology is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with species information document ecology information.</param>
        /// <returns>True if species information document ecology is specified.</returns>
        public static bool IsSpeciesInformationDocumentEcologySpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if species information document extra is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with species information document extra information.</param>
        /// <returns>True if species information document extra is specified.</returns>
        public static bool IsSpeciesInformationDocumentExtraSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if species information document is publishable is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with species information document is publishable information.</param>
        /// <returns>True if species information document is publishable is specified.</returns>
        public static bool IsSpeciesInformationDocumentIsPublishableSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if species information document author and year is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with species information document author and year information.</param>
        /// <returns>True if species information document author and year is specified.</returns>
        public static bool IsSpeciesInformationDocumentItalicsInReferencesSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if species information document author and year is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with species information document author and year information.</param>
        /// <returns>True if species information document author and year is specified.</returns>
        public static bool IsSpeciesInformationDocumentItalicsInTextSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if species information document measures is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with species information document measures information.</param>
        /// <returns>True if species information document measures is specified.</returns>
        public static bool IsSpeciesInformationDocumentMeasuresSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if species information document preamble is specified.
        /// </summary>
        /// <param name="speciesFact">Species fact with species information document preamble information.</param>
        /// <returns>True if species information document preamble is specified.</returns>
        public static bool IsSpeciesInformationDocumentPreambleSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if species information document references is specified.
        /// </summary>
        /// <param name="speciesFact">Species fact with species information document references information.</param>
        /// <returns>True if species information document references is specified.</returns>
        public static bool IsSpeciesInformationDocumentReferencesSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if species information document threats is specified.
        /// </summary>
        /// <param name="speciesFact">Species fact with species information document threats information.</param>
        /// <returns>True if species information document threats is specified.</returns>
        public static bool IsSpeciesInformationDocumentThreatsSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if substrate is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with substrate information.</param>
        /// <returns>True if substrate is specified.</returns>
        public static bool IsSubstrateSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue
                    && (speciesFact.MainField.EnumValue.KeyInt >= (int)SubstrateImportanceEnum.HasImportance)
                    && (speciesFact.MainField.EnumValue.KeyInt <= (int)SubstrateImportanceEnum.VeryImportant);
        }

        /// <summary>
        ///     Test if swedish occurence is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with swedish occurence information.</param>
        /// <returns>True if swedish occurrence is specified.</returns>
        public static bool IsSwedishOccurrenceSpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }

        /// <summary>
        ///     Test if swedish occurence is specified in species fact.
        /// </summary>
        /// <param name="speciesFact">Species fact with swedish occurence information.</param>
        /// <returns>True if swedish occurence is specified.</returns>
        public static bool IsOldRedlistCategorySpecified(this ISpeciesFact speciesFact)
        {
            return speciesFact.IsNotNull() && speciesFact.Field1.IsNotNull() && speciesFact.Field1.HasValue
                    && RedListedHelper.IsRedListedDdToNe(speciesFact.Field1.EnumValue.KeyInt)
                    && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue;
        }
    }
}

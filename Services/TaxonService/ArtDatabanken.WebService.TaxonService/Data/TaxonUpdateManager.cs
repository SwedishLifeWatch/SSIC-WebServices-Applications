using System;
using System.Collections.Generic;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.Data.WebService;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Database;
using WebSpeciesFact = ArtDatabanken.Data.WebService.WebSpeciesFact;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    /// <summary>
    /// Updates taxon database with
    /// species facts from ArtDatabankenService.
    /// </summary>
    public class TaxonUpdateManager
    {
        private static LogManager _logManager;

        private const int EXCLUDE_FROM_REPORTING_SYSTEMS = 1974;
        private const int DYNTAXTA_QUALITY = 2115;
        private const int NUMBER_OF_SPECIES_IN_SWEDEN = 1991;

        /// <summary>
        /// Create a TaxonUpdateManager instance.
        /// </summary>
        public TaxonUpdateManager()
        {
            _logManager = new LogManager();
        }

        /// <summary>
        /// Updates taxon with data from SpeciesFacts.
        /// </summary>
        /// <param name="context">The context.</param>
        public static void UpdateTaxonSpeciesFacts (WebServiceContext context)
        {
            // TODO Check authorization
            LogResult(context, "Start UpdateTaxonSpeciesFacts.");
            UpdateTaxonSwedishOccurence(context);
            UpdateTaxonSwedishHistory(context);
            UpdateTaxonDyntaxaQuality(context);
            UpdateTaxonNumberOfSpeciesInSweden(context);
            UpdateTaxonExcludeFromReportingSystem(context);
            LogResult(context, "Finished update Taxon table with ArtFakta.");
        }

        /// <summary>
        /// Updates taxon with swedish occurence fom SpeciesFatcs
        /// </summary>
        /// <param name="context">The context.</param>
        public static void UpdateTaxonSwedishOccurence(WebServiceContext context)
        {
            // Reset all values to SwedishOccurrence = -1
            context.GetTaxonDatabase().UpdateTaxonSpeciesFact(0, 0, TaxonSpeciesFact.RESET_SWEDISH_OCCURRENCE);

            List<ArtDatabanken.Data.WebService.WebSpeciesFact> webSpeciesFacts = GetSpeciesFacts(context, (Int32)FactorId.SwedishOccurrence);
            foreach (ArtDatabanken.Data.WebService.WebSpeciesFact speciesFact in webSpeciesFacts)
            {
                // Update Taxon table 
                if (speciesFact.TaxonId.IsNotNull() && speciesFact.FieldValue1.IsNotNull())
                {
                    context.GetTaxonDatabase().UpdateTaxonSpeciesFact(speciesFact.TaxonId,
                                                                      (int) speciesFact.FieldValue1,
                                                                      TaxonSpeciesFact.SWEDISH_OCCURENCE);
                }
            }
            LogResult(context, "Update taxon - SwedishOccurrence: " + webSpeciesFacts.Count + " records.");
            // IsSwedish - update all taxa w/ IsSwedish based on SwedishOccurrence value
            context.GetTaxonDatabase().UpdateTaxonIsSwedish();
        }

        /// <summary>
        /// Updates taxon with "Swedish history" from SpeciesFatcs
        /// </summary>
        /// <param name="context">The context.</param>
        public static void UpdateTaxonSwedishHistory(WebServiceContext context)
        {
            List<ArtDatabanken.Data.WebService.WebSpeciesFact> webSpeciesFacts = GetSpeciesFacts(context, (Int32)FactorId.SwedishHistory);

            foreach (ArtDatabanken.Data.WebService.WebSpeciesFact speciesFact in webSpeciesFacts)
            {
                // Update Taxon table 
                if (speciesFact.TaxonId.IsNotNull() && speciesFact.FieldValue1.IsNotNull())
                {
                    context.GetTaxonDatabase().UpdateTaxonSpeciesFact(speciesFact.TaxonId,  (int)speciesFact.FieldValue1, TaxonSpeciesFact.SWEDISH_HISTORY);
                }
            }
            LogResult(context, "Update taxon - SwedishHistory: " + webSpeciesFacts.Count + " records.");
        }

        /// <summary>
        /// Updates taxon with "Exclude from reporting system" from SpeciesFatcs
        /// </summary>
        /// <param name="context">The context.</param>
        public static void UpdateTaxonExcludeFromReportingSystem(WebServiceContext context)
        {
            List<ArtDatabanken.Data.WebService.WebSpeciesFact> webSpeciesFacts = GetSpeciesFacts(context, EXCLUDE_FROM_REPORTING_SYSTEMS);

            foreach (ArtDatabanken.Data.WebService.WebSpeciesFact speciesFact in webSpeciesFacts)
            {
                // Update Taxon table 
                if (speciesFact.TaxonId.IsNotNull() && speciesFact.FieldValue1.IsNotNull())
                {
                    context.GetTaxonDatabase().UpdateTaxonSpeciesFact(speciesFact.TaxonId, (int)speciesFact.FieldValue1, TaxonSpeciesFact.EXCLUDE_FROM_REPORTING_SYSTEMS);
                }
            }
            LogResult(context, "Update taxon - ExcludeFromReportingSystem: " + webSpeciesFacts.Count + " records.");
        }

        /// <summary>
        /// Updates taxon with Dyntaxa Quality from SpeciesFatcs
        /// </summary>
        /// <param name="context">The context.</param>
        public static void UpdateTaxonDyntaxaQuality(WebServiceContext context)
        {
            // Reset all values to DyntaxaQuality = NULL
            context.GetTaxonDatabase().UpdateTaxonSpeciesFact(0, 0, TaxonSpeciesFact.RESET_DYNTAXA_QUALITY);

            List<ArtDatabanken.Data.WebService.WebSpeciesFact> webSpeciesFacts = GetSpeciesFacts(context, DYNTAXTA_QUALITY);

            foreach (ArtDatabanken.Data.WebService.WebSpeciesFact speciesFact in webSpeciesFacts)
            {
                // Update Taxon table 
                if (speciesFact.TaxonId.IsNotNull() && speciesFact.FieldValue1.IsNotNull())
                {
                    context.GetTaxonDatabase().UpdateTaxonSpeciesFact(speciesFact.TaxonId, (int)speciesFact.FieldValue1, TaxonSpeciesFact.DYNTAXA_QUALITY);
                }
            }
            LogResult(context, "Update taxon - DyntaxaQuality: " + webSpeciesFacts.Count + " records.");
            // DyntaxaQuality - update all taxa w/ dyntaxa quality
            context.GetTaxonDatabase().UpdateTaxonDyntaxaQuality();
        }

        /// <summary>
        /// Updates taxon with "Number of species in Sweden" from SpeciesFatcs
        /// </summary>
        /// <param name="context">The context.</param>
        public static void UpdateTaxonNumberOfSpeciesInSweden(WebServiceContext context)
        {
            List<ArtDatabanken.Data.WebService.WebSpeciesFact> webSpeciesFacts = GetSpeciesFacts(context, NUMBER_OF_SPECIES_IN_SWEDEN);

            foreach (ArtDatabanken.Data.WebService.WebSpeciesFact speciesFact in webSpeciesFacts)
            {
                // Update Taxon table 
                if (speciesFact.TaxonId.IsNotNull() && speciesFact.FieldValue1.IsNotNull())
                {
                    context.GetTaxonDatabase().UpdateTaxonSpeciesFact(speciesFact.TaxonId, (int)speciesFact.FieldValue1, TaxonSpeciesFact.NUMBER_OF_SPECIES_IN_SWEDEN);
                }
            }
            LogResult(context, "Update taxon - NumberOfSpeciesInSweden: " + webSpeciesFacts.Count + " records.");
        }

        private static List<ArtDatabanken.Data.WebService.WebSpeciesFact> GetSpeciesFacts (WebServiceContext context, int FactorId)
        {
            List<ArtDatabanken.Data.WebService.WebSpeciesFact> webSpeciesFacts;
            WebUserParameterSelection webUserParameterSelection = new WebUserParameterSelection();
            webUserParameterSelection.FactorIds = new List<Int32>();
            webUserParameterSelection.FactorIds.Add(FactorId);

            webUserParameterSelection.IndividualCategoryIds = new List<Int32>();
            try {
                IndividualCategory individualcategory = IndividualCategoryManager.GetDefaultIndividualCategory();
                webUserParameterSelection.IndividualCategoryIds.Add(individualcategory.Id);
            }
            catch (Exception ex)
            {
                LogResult(context,
                          "Failed calling ArtDatabankenService. WebServiceAddress: " + WebServiceClient.WebServiceAddress);
                LogResult(context, "Exception: " + ex.Message);
            }
         
            // Get data from web service.
            webSpeciesFacts = WebServiceClient.GetSpeciesFactsByUserParameterSelection(webUserParameterSelection);
            return webSpeciesFacts;
        }

        private static void LogResult (WebServiceContext context, String sLogStr)
        {
            if (_logManager.IsNull())
            {
                _logManager = new LogManager();
            }
            _logManager.Log(context, sLogStr, ArtDatabanken.WebService.Data.LogType.Information, null);
        }

    }
}

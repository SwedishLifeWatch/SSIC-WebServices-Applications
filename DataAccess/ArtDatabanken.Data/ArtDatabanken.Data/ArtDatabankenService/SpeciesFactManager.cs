using System;
using System.Collections;
using System.Collections.Generic;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class contains handling of species facts and related objects.
    /// </summary>
    public class SpeciesFactManager : ManagerBase
    {
        private static Hashtable _organismGroups = null;
        private static SpeciesFactQualityList _speciesFactQualities = null;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static SpeciesFactManager()
        {
            RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// Makes access to the private member _organismGroups thread safe.
        /// </summary>
        private static Hashtable OrganismGroups
        {
            get
            {
                Hashtable organismGroups;

                lock (_lockObject)
                {
                    organismGroups = _organismGroups;
                }
                return organismGroups;
            }
            set
            {
                lock (_lockObject)
                {
                    _organismGroups = value;
                }
            }
        }

        /// <summary>
        /// Makes access to the private member _speciesFactQualities thread safe.
        /// </summary>
        private static SpeciesFactQualityList SpeciesFactQualities
        {
            get
            {
                SpeciesFactQualityList speciesFactQualities;

                lock (_lockObject)
                {
                    speciesFactQualities = _speciesFactQualities;
                }
                return speciesFactQualities;
            }
            set
            {
                lock (_lockObject)
                {
                    _speciesFactQualities = value;
                }
            }
        }

        /// <summary>
        /// Expands a Species Fact List with empty species facts so that every combination from the user parameter selection is represented.
        /// Factor Headers are excluded.
        /// Periodic factors are not expanded to individual categories other than the default. 
        /// </summary>
        /// <param name="taxon">Taxon object of the species fact</param>
        /// <param name="individualCategory">Individual category object of the species fact</param>
        /// <param name="factor">Factor object of the species fact</param>
        /// <param name="host">Host taxon object of the species fact</param>
        /// <param name="period">Period object of the species fact</param>
        /// <param name="speciesFacts">Species fact list to be expanded.</param>
        private static void ExpandSpeciesFactListWithEmptySpeciesFact(Taxon taxon,
                                                                      IndividualCategory individualCategory,
                                                                      Factor factor,
                                                                      Taxon host,
                                                                      Period period,
                                                                      SpeciesFactList speciesFacts)
        {
            if (!speciesFacts.Exists(GetSpeciesFactIdentifier(taxon,
                                                              individualCategory,
                                                              factor,
                                                              host,
                                                              period)))
            {
                speciesFacts.Add(GetSpeciesFact(taxon,
                                                individualCategory,
                                                factor,
                                                host,
                                                period));
            }
        }

        /// <summary>
        /// Expands a Species Fact List with empty species facts so that every combination from the user parameter selection is represented.
        /// Factor Headers are excluded.
        /// Periodic factors are not expanded to individual categories other than the default. 
        /// </summary>
        /// <param name="userParameterSelection">User Parameter Selection to be used as base for the species fact list. Needs to have factors and taxa. If it has no individual category, it will be given the default category</param>
        /// <param name="speciesFacts">Species fact list to be expanded.</param>
        public static void ExpandSpeciesFactListWithEmptySpeciesFacts(UserParameterSelection userParameterSelection, SpeciesFactList speciesFacts)
        {
            // Check parameters.
            userParameterSelection.CheckNotNull("userParameterSelection");
            speciesFacts.CheckNotNull("speciesFacts");
            userParameterSelection.IndividualCategories.CheckNotNull("userParameterSelection.IndividualCategories");
            userParameterSelection.Periods.CheckNotNull("userParameterSelection.Periods");
            userParameterSelection.Hosts.CheckNotNull("userParameterSelection.Hosts");
            userParameterSelection.Taxa.CheckNotEmpty("userParameterSelection.Taxa");
            userParameterSelection.Factors.CheckNotEmpty("userParameterSelection.Factors");

            // Add default host if necessary.
            if (!userParameterSelection.HasHosts)
            {
                userParameterSelection.Hosts.Add(TaxonManager.GetTaxon(0, TaxonInformationType.Basic));
            }

            // Add default individual category if necessary.
            if (!userParameterSelection.HasIndividualCategories)
            {
                userParameterSelection.IndividualCategories.Add(IndividualCategoryManager.GetIndividualCategory(IndividualCategoryId.Default));
            }

            foreach (Factor factor in userParameterSelection.Factors)
            {
                if (factor.FactorUpdateMode.IsHeader)
                {
                    // Don't create SpeicesFacts for 'Headers'.
                    continue;
                }

                foreach (IndividualCategory individualCategory in userParameterSelection.IndividualCategories)
                {
                    if (factor.IsPeriodic &&
                        (individualCategory.Id != ((Int32)IndividualCategoryId.Default)))
                    {
                        // Periodic factors should only be combined
                        // with default IndividualCategory.
                        continue;
                    }

                    foreach (Taxon taxon in userParameterSelection.Taxa)
                    {
                        if (factor.IsPeriodic)
                        {
                            // Factor is periodic
                            foreach (Period period in userParameterSelection.Periods)
                            {
                                ExpandSpeciesFactListWithEmptySpeciesFact(taxon, individualCategory, factor, null, period, speciesFacts);

                                if (factor.IsTaxonomic)
                                {
                                    foreach (Taxon host in userParameterSelection.Hosts)
                                    {
                                        ExpandSpeciesFactListWithEmptySpeciesFact(taxon, individualCategory, factor, host, period, speciesFacts);
                                    }
                                }
                            }
                            // End factor is periodic
                        }
                        else
                        {
                            // Factor is not periodic
                            if (factor.IsTaxonomic)
                            {
                                foreach (Taxon host in userParameterSelection.Hosts)
                                {
                                    ExpandSpeciesFactListWithEmptySpeciesFact(taxon, individualCategory, factor, host, null, speciesFacts);
                                }
                            }
                            else
                            {
                                ExpandSpeciesFactListWithEmptySpeciesFact(taxon, individualCategory, factor, null, null, speciesFacts);
                            }
                            // End factor is not periodic
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Makes sure that a user parameter selection contains all "dimensions" from a species fact list. 
        /// This means for example that if the species fact list contains individual categories that are not included in the user parameter selection, 
        /// those individual categories will be added to the user parameter selection. And so on for hosts and periods.
        /// </summary>
        /// <param name="userParameterSelection"></param>
        /// <param name="speciesFacts"></param>
        private static void ExpandUserParameterSelectionBySpeciesFacts(UserParameterSelection userParameterSelection,
                                                                       SpeciesFactList speciesFacts)
        {
            // Expand user parameter selection.
            foreach (SpeciesFact speciesFact in speciesFacts)
            {
                userParameterSelection.Factors.Merge(speciesFact.Factor);
                userParameterSelection.Hosts.Merge(speciesFact.Host);
                userParameterSelection.IndividualCategories.Merge(speciesFact.IndividualCategory);
                userParameterSelection.Periods.Merge(speciesFact.Period);
                userParameterSelection.Taxa.Merge(speciesFact.Taxon);
            }

            // Set default values if no values are entered.
            // TODO: Is this realy neccessary?
            if (userParameterSelection.Hosts.IsEmpty())
            {
                userParameterSelection.Hosts.Add(TaxonManager.GetTaxon(0, TaxonInformationType.Basic));
            }
            if (userParameterSelection.IndividualCategories.IsEmpty())
            {
                userParameterSelection.IndividualCategories.Add(IndividualCategoryManager.GetIndividualCategory(IndividualCategoryId.Default));
            }
            if (userParameterSelection.Periods.IsEmpty())
            {
                userParameterSelection.Periods.Merge(PeriodManager.GetPeriods());
            }
        }

        /// <summary>
        /// Get the default species fact quality object.
        /// </summary>
        /// <returns>Requested species fact quality.</returns>
        public static SpeciesFactQuality GetDefaultSpeciesFactQuality()
        {
            return GetSpeciesFactQuality(SpeciesFactQualityId.Acceptable);
        }

        /// <summary>
        /// Get information about default organism groups.
        /// </summary>
        /// <returns>Information about default organism groups.</returns>
        public static OrganismGroupList GetOrganismGroups()
        {
            return GetOrganismGroups(OrganismGroupType.Standard);
        }

        /// <summary>
        /// Get information about specified organism groups.
        /// </summary>
        /// <param name='type'>Type of organism groups.</param>
        /// <returns>Information about specified organism groups.</returns>
        public static OrganismGroupList GetOrganismGroups(OrganismGroupType type)
        {
            OrganismGroupList organismGroups = null;
            Hashtable allOrganismGroups;

            for (Int32 getAttempts = 0; (organismGroups.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadOrganismGroups();
                allOrganismGroups = OrganismGroups;
                if (allOrganismGroups.IsNotNull())
                {
                    organismGroups = (OrganismGroupList)(allOrganismGroups[type]);
                }
            }
            return organismGroups;
        }

        /// <summary>
        /// Convert a SpeciesFact into a WebSpeciesFact.
        /// </summary>
        /// <param name='speciesFact'>Species fact to convert.</param>
        /// <param name="defaultReference">Reference used if no reference is specified.</param>
        /// <returns>A WebSpeciesFact instance.</returns>
        private static WebSpeciesFact GetSpeciesFact(SpeciesFact speciesFact,
                                                     Reference defaultReference)
        {
            WebSpeciesFact webSpeciesFact;

            webSpeciesFact = new WebSpeciesFact();
            webSpeciesFact.FactorId = speciesFact.Factor.Id;
#if DATA_SPECIFIED_EXISTS
            webSpeciesFact.FactorIdSpecified = true;
#endif

            webSpeciesFact.IsHostSpecified = speciesFact.HasHost;
#if DATA_SPECIFIED_EXISTS
            webSpeciesFact.IsHostSpecifiedSpecified = true;
#endif
            if (speciesFact.HasHost)
            {
                webSpeciesFact.HostId = speciesFact.Host.Id;
#if DATA_SPECIFIED_EXISTS
                webSpeciesFact.HostIdSpecified = true;
#endif
            }

            if (speciesFact.HasId)
            {
                webSpeciesFact.Id = speciesFact.Id;
#if DATA_SPECIFIED_EXISTS
                webSpeciesFact.IdSpecified = true;
#endif
            }

            webSpeciesFact.IndividualCategoryId = speciesFact.IndividualCategory.Id;
#if DATA_SPECIFIED_EXISTS
            webSpeciesFact.IndividualCategoryIdSpecified = true;
#endif

            webSpeciesFact.IsPeriodSpecified = speciesFact.HasPeriod;
#if DATA_SPECIFIED_EXISTS
            webSpeciesFact.IsPeriodSpecifiedSpecified = true;
#endif
            if (speciesFact.HasPeriod)
            {
                webSpeciesFact.PeriodId = speciesFact.Period.Id;
#if DATA_SPECIFIED_EXISTS
                webSpeciesFact.PeriodIdSpecified = true;
#endif
            }

            webSpeciesFact.QualityId = speciesFact.Quality.Id;
#if DATA_SPECIFIED_EXISTS
            webSpeciesFact.QualityIdSpecified = true;
#endif

            if (speciesFact.Reference.IsNull())
            {
                if (defaultReference.IsNotNull())
                {
                    webSpeciesFact.ReferenceId = defaultReference.Id;
#if DATA_SPECIFIED_EXISTS
                    webSpeciesFact.ReferenceIdSpecified = true;
#endif
                }
            }
            else
            {
                webSpeciesFact.ReferenceId = speciesFact.Reference.Id;
#if DATA_SPECIFIED_EXISTS
                webSpeciesFact.ReferenceIdSpecified = true;
#endif
            }

            webSpeciesFact.TaxonId = speciesFact.Taxon.Id;
#if DATA_SPECIFIED_EXISTS
            webSpeciesFact.TaxonIdSpecified = true;
#endif

            // Set fields.
            webSpeciesFact.IsFieldValue1Specified = false;
#if DATA_SPECIFIED_EXISTS
            webSpeciesFact.IsFieldValue1SpecifiedSpecified = true;
#endif
            webSpeciesFact.IsFieldValue2Specified = false;
#if DATA_SPECIFIED_EXISTS
            webSpeciesFact.IsFieldValue2SpecifiedSpecified = true;
#endif
            webSpeciesFact.IsFieldValue3Specified = false;
#if DATA_SPECIFIED_EXISTS
            webSpeciesFact.IsFieldValue3SpecifiedSpecified = true;
#endif
            webSpeciesFact.IsFieldValue4Specified = false;
#if DATA_SPECIFIED_EXISTS
            webSpeciesFact.IsFieldValue4SpecifiedSpecified = true;
#endif
            webSpeciesFact.IsFieldValue5Specified = false;
#if DATA_SPECIFIED_EXISTS
            webSpeciesFact.IsFieldValue5SpecifiedSpecified = true;
#endif
            foreach (SpeciesFactField field in speciesFact.Fields)
            {
                switch (field.Index)
                {
                    case 0:
                        webSpeciesFact.IsFieldValue1Specified = field.HasValue;
#if DATA_SPECIFIED_EXISTS
                        webSpeciesFact.IsFieldValue1SpecifiedSpecified = true;
#endif
                        if (field.HasValue)
                        {
                            webSpeciesFact.FieldValue1 = field.GetDoubleValue();
#if DATA_SPECIFIED_EXISTS
                            webSpeciesFact.FieldValue1Specified = true;
#endif
                        }
                        break;
                    case 1:
                        webSpeciesFact.IsFieldValue2Specified = field.HasValue;
#if DATA_SPECIFIED_EXISTS
                        webSpeciesFact.IsFieldValue2SpecifiedSpecified = true;
#endif
                        if (field.HasValue)
                        {
                            webSpeciesFact.FieldValue2 = field.GetDoubleValue();
#if DATA_SPECIFIED_EXISTS
                            webSpeciesFact.FieldValue2Specified = true;
#endif
                        }
                        break;
                    case 2:
                        webSpeciesFact.IsFieldValue3Specified = field.HasValue;
#if DATA_SPECIFIED_EXISTS
                        webSpeciesFact.IsFieldValue3SpecifiedSpecified = true;
#endif
                        if (field.HasValue)
                        {
                            webSpeciesFact.FieldValue3 = field.GetDoubleValue();
#if DATA_SPECIFIED_EXISTS
                            webSpeciesFact.FieldValue3Specified = true;
#endif
                        }
                        break;
                    case 3:
                        webSpeciesFact.IsFieldValue4Specified = field.HasValue;
#if DATA_SPECIFIED_EXISTS
                        webSpeciesFact.IsFieldValue4SpecifiedSpecified = true;
#endif
                        if (field.HasValue)
                        {
                            webSpeciesFact.FieldValue4 = field.GetStringValue().Trim();
                        }
                        break;
                    case 4:
                        webSpeciesFact.IsFieldValue5Specified = field.HasValue;
#if DATA_SPECIFIED_EXISTS
                        webSpeciesFact.IsFieldValue5SpecifiedSpecified = true;
#endif
                        if (field.HasValue)
                        {
                            webSpeciesFact.FieldValue5 = field.GetStringValue().Trim();
                        }
                        break;
                    default:
                        throw new Exception("Unknown data field!");
                }
            }

            return webSpeciesFact;
        }

        /// <summary>
        /// Get an empty (only default data) SpeciesFact instance.
        /// </summary>
        /// <param name="taxon">Taxon object of the species fact</param>
        /// <param name="individualCategory">Individual category object of the species fact</param>
        /// <param name="factor">Factor object of the species fact</param>
        /// <param name="host">Host taxon object of the species fact</param>
        /// <param name="period">Period object of the species fact</param>
        /// <returns>A SpeciesFact instance.</returns>
        private static SpeciesFact GetSpeciesFact(Taxon taxon,
                                                  IndividualCategory individualCategory,
                                                  Factor factor,
                                                  Taxon host,
                                                  Period period)
        {
            SpeciesFact speciesFact;

            switch (factor.Id)
            {
                case (Int32)(FactorId.RedListCategoryAutomatic):
                    speciesFact = new SpeciesFactRedListCategory(taxon,
                                                                 individualCategory,
                                                                 factor,
                                                                 host,
                                                                 period);
                    break;
                case (Int32)(FactorId.RedListCriteriaAutomatic):
                    speciesFact = new SpeciesFactRedListCriteria(taxon,
                                                                 individualCategory,
                                                                 factor,
                                                                 host,
                                                                 period);
                    break;
                case (Int32)(FactorId.RedListCriteriaDocumentationAutomatic):
                    speciesFact = new SpeciesFactRedListCriteriaDocumentation(taxon,
                                                                 individualCategory,
                                                                 factor,
                                                                 host,
                                                                 period);
                    break;
                case (Int32)(FactorId.NNAutomaticTaxonNameSummary):
                    speciesFact = new SpeciesFactTaxonNameSummary(taxon,
                                                                 individualCategory,
                                                                 factor,
                                                                 host,
                                                                 period);
                    break;
                default:
                    speciesFact = new SpeciesFact(taxon,
                                                  individualCategory,
                                                  factor,
                                                  host,
                                                  period);
                    break;
            }
            return speciesFact;
        }

        /// <summary>
        /// Get the requsted species fact.
        /// </summary>
        /// <param name="webSpeciesFact">A webSpeciesFact object</param>
        /// <returns>A species fact</returns>
        public static SpeciesFact GetSpeciesFact(WebSpeciesFact webSpeciesFact)
        {
            SpeciesFact speciesFact;

            switch (webSpeciesFact.FactorId)
            {
                case (Int32)(FactorId.RedListCategoryAutomatic):
                    speciesFact = new SpeciesFactRedListCategory(webSpeciesFact.Id,
                                                                 webSpeciesFact.Id,
                                                                 webSpeciesFact.TaxonId,
                                                                 webSpeciesFact.IndividualCategoryId,
                                                                 webSpeciesFact.FactorId,
                                                                 webSpeciesFact.HostId,
                                                                 webSpeciesFact.IsHostSpecified,
                                                                 webSpeciesFact.PeriodId,
                                                                 webSpeciesFact.IsPeriodSpecified,
                                                                 webSpeciesFact.FieldValue1,
                                                                 webSpeciesFact.IsFieldValue1Specified,
                                                                 webSpeciesFact.FieldValue2,
                                                                 webSpeciesFact.IsFieldValue2Specified,
                                                                 webSpeciesFact.FieldValue3,
                                                                 webSpeciesFact.IsFieldValue3Specified,
                                                                 webSpeciesFact.FieldValue4,
                                                                 webSpeciesFact.IsFieldValue4Specified,
                                                                 webSpeciesFact.FieldValue5,
                                                                 webSpeciesFact.IsFieldValue5Specified,
                                                                 webSpeciesFact.QualityId,
                                                                 webSpeciesFact.ReferenceId,
                                                                 webSpeciesFact.UpdateUserFullName,
                                                                 webSpeciesFact.UpdateDate);
                    break;
                case (Int32)(FactorId.RedListCriteriaAutomatic):
                    speciesFact = new SpeciesFactRedListCriteria(webSpeciesFact.Id,
                                                                 webSpeciesFact.Id,
                                                                 webSpeciesFact.TaxonId,
                                                                 webSpeciesFact.IndividualCategoryId,
                                                                 webSpeciesFact.FactorId,
                                                                 webSpeciesFact.HostId,
                                                                 webSpeciesFact.IsHostSpecified,
                                                                 webSpeciesFact.PeriodId,
                                                                 webSpeciesFact.IsPeriodSpecified,
                                                                 webSpeciesFact.FieldValue1,
                                                                 webSpeciesFact.IsFieldValue1Specified,
                                                                 webSpeciesFact.FieldValue2,
                                                                 webSpeciesFact.IsFieldValue2Specified,
                                                                 webSpeciesFact.FieldValue3,
                                                                 webSpeciesFact.IsFieldValue3Specified,
                                                                 webSpeciesFact.FieldValue4,
                                                                 webSpeciesFact.IsFieldValue4Specified,
                                                                 webSpeciesFact.FieldValue5,
                                                                 webSpeciesFact.IsFieldValue5Specified,
                                                                 webSpeciesFact.QualityId,
                                                                 webSpeciesFact.ReferenceId,
                                                                 webSpeciesFact.UpdateUserFullName,
                                                                 webSpeciesFact.UpdateDate);
                    break;
                case (Int32)(FactorId.RedListCriteriaDocumentationAutomatic):
                    speciesFact = new SpeciesFactRedListCriteriaDocumentation(webSpeciesFact.Id,
                                                                 webSpeciesFact.Id,
                                                                 webSpeciesFact.TaxonId,
                                                                 webSpeciesFact.IndividualCategoryId,
                                                                 webSpeciesFact.FactorId,
                                                                 webSpeciesFact.HostId,
                                                                 webSpeciesFact.IsHostSpecified,
                                                                 webSpeciesFact.PeriodId,
                                                                 webSpeciesFact.IsPeriodSpecified,
                                                                 webSpeciesFact.FieldValue1,
                                                                 webSpeciesFact.IsFieldValue1Specified,
                                                                 webSpeciesFact.FieldValue2,
                                                                 webSpeciesFact.IsFieldValue2Specified,
                                                                 webSpeciesFact.FieldValue3,
                                                                 webSpeciesFact.IsFieldValue3Specified,
                                                                 webSpeciesFact.FieldValue4,
                                                                 webSpeciesFact.IsFieldValue4Specified,
                                                                 webSpeciesFact.FieldValue5,
                                                                 webSpeciesFact.IsFieldValue5Specified,
                                                                 webSpeciesFact.QualityId,
                                                                 webSpeciesFact.ReferenceId,
                                                                 webSpeciesFact.UpdateUserFullName,
                                                                 webSpeciesFact.UpdateDate);
                    break;
                case (Int32)(FactorId.NNAutomaticTaxonNameSummary):
                    speciesFact = new SpeciesFactTaxonNameSummary(webSpeciesFact.Id,
                                                                 webSpeciesFact.Id,
                                                                 webSpeciesFact.TaxonId,
                                                                 webSpeciesFact.IndividualCategoryId,
                                                                 webSpeciesFact.FactorId,
                                                                 webSpeciesFact.HostId,
                                                                 webSpeciesFact.IsHostSpecified,
                                                                 webSpeciesFact.PeriodId,
                                                                 webSpeciesFact.IsPeriodSpecified,
                                                                 webSpeciesFact.FieldValue1,
                                                                 webSpeciesFact.IsFieldValue1Specified,
                                                                 webSpeciesFact.FieldValue2,
                                                                 webSpeciesFact.IsFieldValue2Specified,
                                                                 webSpeciesFact.FieldValue3,
                                                                 webSpeciesFact.IsFieldValue3Specified,
                                                                 webSpeciesFact.FieldValue4,
                                                                 webSpeciesFact.IsFieldValue4Specified,
                                                                 webSpeciesFact.FieldValue5,
                                                                 webSpeciesFact.IsFieldValue5Specified,
                                                                 webSpeciesFact.QualityId,
                                                                 webSpeciesFact.ReferenceId,
                                                                 webSpeciesFact.UpdateUserFullName,
                                                                 webSpeciesFact.UpdateDate);
                    break;
                default:
                    speciesFact = new SpeciesFact(webSpeciesFact.Id,
                                                  webSpeciesFact.Id,
                                                  webSpeciesFact.TaxonId,
                                                  webSpeciesFact.IndividualCategoryId,
                                                  webSpeciesFact.FactorId,
                                                  webSpeciesFact.HostId,
                                                  webSpeciesFact.IsHostSpecified,
                                                  webSpeciesFact.PeriodId,
                                                  webSpeciesFact.IsPeriodSpecified,
                                                  webSpeciesFact.FieldValue1,
                                                  webSpeciesFact.IsFieldValue1Specified,
                                                  webSpeciesFact.FieldValue2,
                                                  webSpeciesFact.IsFieldValue2Specified,
                                                  webSpeciesFact.FieldValue3,
                                                  webSpeciesFact.IsFieldValue3Specified,
                                                  webSpeciesFact.FieldValue4,
                                                  webSpeciesFact.IsFieldValue4Specified,
                                                  webSpeciesFact.FieldValue5,
                                                  webSpeciesFact.IsFieldValue5Specified,
                                                  webSpeciesFact.QualityId,
                                                  webSpeciesFact.ReferenceId,
                                                  webSpeciesFact.UpdateUserFullName,
                                                  webSpeciesFact.UpdateDate);
                    break;
            }

            return speciesFact;
        }

        /// <summary>
        /// Method that generates an unique identifier for a species fact
        /// </summary>
        /// <param name="taxonId">Id of the taxon that is related to the species fact</param>
        /// <param name="individualCategoryId">Id of the individual category that is related to the species fact</param>
        /// <param name="factorId">Id of the factor that is related to the species fact</param>
        /// <param name="hasHostId">Indicates if hostId has a value</param>
        /// <param name="hostId">Id of the host taxon that is related to the species fact</param>
        /// <param name="hasPeriodId">Indicates if periodId has a value</param>
        /// <param name="periodId">Id of the period that is related to the species fact</param>
        /// <returns></returns>
        public static String GetSpeciesFactIdentifier(Int32 taxonId,
                                                      Int32 individualCategoryId,
                                                      Int32 factorId,
                                                      Boolean hasHostId,
                                                      Int32 hostId,
                                                      Boolean hasPeriodId,
                                                      Int32 periodId)
        {
            if (!hasHostId)
            {
                hostId = 0;
            }
            if (!hasPeriodId)
            {
                periodId = 0;
            }
            return GetSpeciesFactIdentifier(taxonId,
                                            individualCategoryId,
                                            factorId,
                                            hostId,
                                            periodId);
        }

        /// <summary>
        /// Method that generates an unique identifier for a species fact
        /// </summary>
        /// <param name="taxonId">Id of the taxon that is related to the species fact</param>
        /// <param name="individualCategoryId">Id of the individual category that is related to the species fact</param>
        /// <param name="factorId">Id of the factor that is related to the species fact</param>
        /// <param name="hostId">Id of the host taxon that is related to the species fact</param>
        /// <param name="periodId">Id of the period that is related to the species fact</param>
        /// <returns></returns>
        private static String GetSpeciesFactIdentifier(Int32 taxonId,
                                                       Int32 individualCategoryId,
                                                       Int32 factorId,
                                                       Int32 hostId,
                                                       Int32 periodId)
        {
            return "Taxon=" + taxonId + "," +
                   "Factor=" + factorId + "," +
                   "IndividualCategory=" + individualCategoryId + "," +
                   "Host=" + hostId + "," +
                   "Period=" + periodId;
        }

        /// <summary>
        /// Method that generates an unique identifier for a species fact
        /// </summary>
        /// <param name="taxon">Taxon that is related to the species fact</param>
        /// <param name="individualCategory">Individual category that is related to the species fact</param>
        /// <param name="factor">Factor that is related to the species fact</param>
        /// <param name="host">Host taxon that is related to the species fact</param>
        /// <param name="period">Period that is related to the species fact</param>
        /// <returns></returns>
        public static String GetSpeciesFactIdentifier(Taxon taxon,
                                                      IndividualCategory individualCategory,
                                                      Factor factor,
                                                      Taxon host,
                                                      Period period)
        {
            Int32 hostId = 0, periodId = 0;

            if ((host.IsNotNull()) && (factor.IsTaxonomic))
            {
                hostId = host.Id;
            }
            if ((period.IsNotNull()) && (factor.IsPeriodic))
            {
                periodId = period.Id;
            }

            return GetSpeciesFactIdentifier(taxon.Id,
                                            individualCategory.Id,
                                            factor.Id,
                                            hostId,
                                            periodId);
        }

        /// <summary>
        /// Method that generates an unique identifier for a species fact
        /// </summary>
        /// <param name="speciesFact">Species fact.</param>
        /// <returns>Species fact identifier.</returns>
        private static String GetSpeciesFactIdentifier(WebSpeciesFact speciesFact)
        {
            return GetSpeciesFactIdentifier(speciesFact.TaxonId,
                                            speciesFact.IndividualCategoryId,
                                            speciesFact.FactorId,
                                            speciesFact.IsHostSpecified,
                                            speciesFact.HostId,
                                            speciesFact.IsPeriodSpecified,
                                            speciesFact.PeriodId);
        }

        /// <summary>
        /// Get all species fact quality objects.
        /// </summary>
        /// <returns>All species fact qualities.</returns>
        public static SpeciesFactQualityList GetSpeciesFactQualities()
        {
            SpeciesFactQualityList speciesFactQualities = null;

            for (Int32 getAttempts = 0; (speciesFactQualities.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadSpeciesFactQualities();
                speciesFactQualities = SpeciesFactQualities;
            }
            return speciesFactQualities;
        }

        /// <summary>
        /// Get the requested species fact quality object.
        /// </summary>
        /// <param name='speciesFactQualityId'>Id of requested species fact quality.</param>
        /// <returns>Requested species fact quality.</returns>
        /// <exception cref="ArgumentException">Thrown if no factor update mode has the requested id.</exception>
        public static SpeciesFactQuality GetSpeciesFactQuality(SpeciesFactQualityId speciesFactQualityId)
        {
            return GetSpeciesFactQuality((Int32)speciesFactQualityId);
        }

        /// <summary>
        /// Get the requested species fact quality object.
        /// </summary>
        /// <param name='speciesFactQualityId'>Id of requested species fact quality.</param>
        /// <returns>Requested species fact quality.</returns>
        /// <exception cref="ArgumentException">Thrown if no factor update mode has the requested id.</exception>
        public static SpeciesFactQuality GetSpeciesFactQuality(Int32 speciesFactQualityId)
        {
            return GetSpeciesFactQualities().Get(speciesFactQualityId);
        }
        
        /// <summary>
        /// Get information about speciesFacts.
        /// </summary>
        /// <param name="speciesFactIds">Ids for speciesFacts to get information about.</param>
        /// <returns>SpeciesFacts information.</returns>
        public static SpeciesFactList GetSpeciesFacts(List<Int32> speciesFactIds)
        {
            List<WebSpeciesFact> webSpeciesFacts;

            // Check arguments.
            speciesFactIds.CheckNotEmpty("speciesFactIds");

            // Get data from web service.
            webSpeciesFacts = WebServiceClient.GetSpeciesFactsById(speciesFactIds);
            return GetSpeciesFacts(webSpeciesFacts);
        }

        /// <summary>
        /// Convert a SpeciesFactList into a WebSpeciesFact array.
        /// </summary>
        /// <param name='speciesFacts'>Species facts to convert.</param>
        /// <param name="defaultReference">Reference used if no reference is specified.</param>
        /// <returns>The WebSpeciesFact array.</returns>
        private static List<WebSpeciesFact> GetSpeciesFacts(SpeciesFactList speciesFacts,
                                                        Reference defaultReference)
        {
            Int32 speciesFactIndex;
            SpeciesFact speciesFact;
            List<WebSpeciesFact> webSpeciesFacts = null;

            if (speciesFacts.IsNotEmpty())
            {
                webSpeciesFacts = new List<WebSpeciesFact>();
                for (speciesFactIndex = 0; speciesFactIndex < speciesFacts.Count; speciesFactIndex++)
                {
                    speciesFact = speciesFacts[speciesFactIndex];
                    webSpeciesFacts.Add(GetSpeciesFact(speciesFact, defaultReference));
                }
            }

            return webSpeciesFacts;
        }

        /// <summary>
        /// Convert WebSpeciesFacts to SpeciesFacts. 
        /// </summary>
        /// <param name="webSpeciesFacts">WebSpeciesFact objects</param>
        /// <returns>A list of species facts.</returns>
        private static SpeciesFactList GetSpeciesFacts(List<WebSpeciesFact> webSpeciesFacts)
        {
            TaxonList taxa;

            taxa = new TaxonList(true);
            return GetSpeciesFacts(webSpeciesFacts, taxa);
        }

        /// <summary>
        /// Convert WebSpeciesFacts to SpeciesFacts. 
        /// </summary>
        /// <param name="webSpeciesFacts">WebSpeciesFact objects</param>
        /// <param name="taxa">
        /// List of taxa that includes the taxon that
        /// the species facts are related to.
        /// </param>
        /// <returns>A list of species facts.</returns>
        private static SpeciesFactList GetSpeciesFacts(List<WebSpeciesFact> webSpeciesFacts,
                                                       TaxonList taxa)
        {
            List<Int32> taxonIds;
            SpeciesFact speciesFact;
            SpeciesFactList speciesFacts;
            Taxon host, taxon;
            TaxonList tempTaxonList;

            // Get all taxa that is not already loaded.
            // This is an optimization. The class SpeciesFact
            // accepts TaxonId and HostId as input.
            taxonIds = new List<Int32>();
            foreach (WebSpeciesFact webSpeciesFact in webSpeciesFacts)
            {
                if (!taxa.Exists(webSpeciesFact.TaxonId) &&
                    !taxonIds.Contains(webSpeciesFact.TaxonId))
                {
                    taxonIds.Add(webSpeciesFact.TaxonId);
                }
                if (webSpeciesFact.IsHostSpecified &&
                    !taxa.Exists(webSpeciesFact.HostId) &&
                    !taxonIds.Contains(webSpeciesFact.HostId))
                {
                    taxonIds.Add(webSpeciesFact.HostId);
                }
            }
            if (taxonIds.IsNotEmpty())
            {
                tempTaxonList = new TaxonList();
                tempTaxonList.AddRange(taxa);
                tempTaxonList.AddRange(TaxonManager.GetTaxa(taxonIds, TaxonInformationType.Basic));
                taxa = tempTaxonList;
            }

            // Create SpeciesFact instances.
            speciesFacts = new SpeciesFactList();
            foreach (WebSpeciesFact webSpeciesFact in webSpeciesFacts)
            {
                // Get host and taxon.
                if (webSpeciesFact.IsHostSpecified)
                {
                    host = taxa.Get(webSpeciesFact.HostId);
                }
                else
                {
                    host = null;
                }
                taxon = taxa.Get(webSpeciesFact.TaxonId);

                // Create SpeciesFact.
                switch (webSpeciesFact.FactorId)
                {
                    case (Int32)(FactorId.RedListCategoryAutomatic):
                        speciesFact = new SpeciesFactRedListCategory(webSpeciesFact.Id,
                                                                     webSpeciesFact.Id,
                                                                     taxon,
                                                                     webSpeciesFact.IndividualCategoryId,
                                                                     webSpeciesFact.FactorId,
                                                                     host,
                                                                     webSpeciesFact.IsHostSpecified,
                                                                     webSpeciesFact.PeriodId,
                                                                     webSpeciesFact.IsPeriodSpecified,
                                                                     webSpeciesFact.FieldValue1,
                                                                     webSpeciesFact.IsFieldValue1Specified,
                                                                     webSpeciesFact.FieldValue2,
                                                                     webSpeciesFact.IsFieldValue2Specified,
                                                                     webSpeciesFact.FieldValue3,
                                                                     webSpeciesFact.IsFieldValue3Specified,
                                                                     webSpeciesFact.FieldValue4,
                                                                     webSpeciesFact.IsFieldValue4Specified,
                                                                     webSpeciesFact.FieldValue5,
                                                                     webSpeciesFact.IsFieldValue5Specified,
                                                                     webSpeciesFact.QualityId,
                                                                     webSpeciesFact.ReferenceId,
                                                                     webSpeciesFact.UpdateUserFullName,
                                                                     webSpeciesFact.UpdateDate);
                        break;
                    case (Int32)(FactorId.RedListCriteriaAutomatic):
                        speciesFact = new SpeciesFactRedListCriteria(webSpeciesFact.Id,
                                                                     webSpeciesFact.Id,
                                                                     taxon,
                                                                     webSpeciesFact.IndividualCategoryId,
                                                                     webSpeciesFact.FactorId,
                                                                     host,
                                                                     webSpeciesFact.IsHostSpecified,
                                                                     webSpeciesFact.PeriodId,
                                                                     webSpeciesFact.IsPeriodSpecified,
                                                                     webSpeciesFact.FieldValue1,
                                                                     webSpeciesFact.IsFieldValue1Specified,
                                                                     webSpeciesFact.FieldValue2,
                                                                     webSpeciesFact.IsFieldValue2Specified,
                                                                     webSpeciesFact.FieldValue3,
                                                                     webSpeciesFact.IsFieldValue3Specified,
                                                                     webSpeciesFact.FieldValue4,
                                                                     webSpeciesFact.IsFieldValue4Specified,
                                                                     webSpeciesFact.FieldValue5,
                                                                     webSpeciesFact.IsFieldValue5Specified,
                                                                     webSpeciesFact.QualityId,
                                                                     webSpeciesFact.ReferenceId,
                                                                     webSpeciesFact.UpdateUserFullName,
                                                                     webSpeciesFact.UpdateDate);
                        break;
                    case (Int32)(FactorId.RedListCriteriaDocumentationAutomatic):
                        speciesFact = new SpeciesFactRedListCriteriaDocumentation(webSpeciesFact.Id,
                                                                     webSpeciesFact.Id,
                                                                     taxon,
                                                                     webSpeciesFact.IndividualCategoryId,
                                                                     webSpeciesFact.FactorId,
                                                                     host,
                                                                     webSpeciesFact.IsHostSpecified,
                                                                     webSpeciesFact.PeriodId,
                                                                     webSpeciesFact.IsPeriodSpecified,
                                                                     webSpeciesFact.FieldValue1,
                                                                     webSpeciesFact.IsFieldValue1Specified,
                                                                     webSpeciesFact.FieldValue2,
                                                                     webSpeciesFact.IsFieldValue2Specified,
                                                                     webSpeciesFact.FieldValue3,
                                                                     webSpeciesFact.IsFieldValue3Specified,
                                                                     webSpeciesFact.FieldValue4,
                                                                     webSpeciesFact.IsFieldValue4Specified,
                                                                     webSpeciesFact.FieldValue5,
                                                                     webSpeciesFact.IsFieldValue5Specified,
                                                                     webSpeciesFact.QualityId,
                                                                     webSpeciesFact.ReferenceId,
                                                                     webSpeciesFact.UpdateUserFullName,
                                                                     webSpeciesFact.UpdateDate);
                        break;
                    case (Int32)(FactorId.NNAutomaticTaxonNameSummary):
                        speciesFact = new SpeciesFactTaxonNameSummary(webSpeciesFact.Id,
                                                                     webSpeciesFact.Id,
                                                                     taxon,
                                                                     webSpeciesFact.IndividualCategoryId,
                                                                     webSpeciesFact.FactorId,
                                                                     host,
                                                                     webSpeciesFact.IsHostSpecified,
                                                                     webSpeciesFact.PeriodId,
                                                                     webSpeciesFact.IsPeriodSpecified,
                                                                     webSpeciesFact.FieldValue1,
                                                                     webSpeciesFact.IsFieldValue1Specified,
                                                                     webSpeciesFact.FieldValue2,
                                                                     webSpeciesFact.IsFieldValue2Specified,
                                                                     webSpeciesFact.FieldValue3,
                                                                     webSpeciesFact.IsFieldValue3Specified,
                                                                     webSpeciesFact.FieldValue4,
                                                                     webSpeciesFact.IsFieldValue4Specified,
                                                                     webSpeciesFact.FieldValue5,
                                                                     webSpeciesFact.IsFieldValue5Specified,
                                                                     webSpeciesFact.QualityId,
                                                                     webSpeciesFact.ReferenceId,
                                                                     webSpeciesFact.UpdateUserFullName,
                                                                     webSpeciesFact.UpdateDate);
                        break;
                    default:
                        speciesFact = new SpeciesFact(webSpeciesFact.Id,
                                                      webSpeciesFact.Id,
                                                      taxon,
                                                      webSpeciesFact.IndividualCategoryId,
                                                      webSpeciesFact.FactorId,
                                                      host,
                                                      webSpeciesFact.IsHostSpecified,
                                                      webSpeciesFact.PeriodId,
                                                      webSpeciesFact.IsPeriodSpecified,
                                                      webSpeciesFact.FieldValue1,
                                                      webSpeciesFact.IsFieldValue1Specified,
                                                      webSpeciesFact.FieldValue2,
                                                      webSpeciesFact.IsFieldValue2Specified,
                                                      webSpeciesFact.FieldValue3,
                                                      webSpeciesFact.IsFieldValue3Specified,
                                                      webSpeciesFact.FieldValue4,
                                                      webSpeciesFact.IsFieldValue4Specified,
                                                      webSpeciesFact.FieldValue5,
                                                      webSpeciesFact.IsFieldValue5Specified,
                                                      webSpeciesFact.QualityId,
                                                      webSpeciesFact.ReferenceId,
                                                      webSpeciesFact.UpdateUserFullName,
                                                      webSpeciesFact.UpdateDate);
                        break;
                }

                // Save SpeciesFact.
                speciesFacts.Add(speciesFact);
            }
            return speciesFacts;
        }

        /// <summary>
        /// Convert WebSpeciesFacts to SpeciesFacts. 
        /// </summary>
        /// <param name="webSpeciesFacts">WebSpeciesFact objects</param>
        /// <param name="userParameterSelection">The user parmeter selection.</param>
        /// <returns>A list of species facts.</returns>
        private static SpeciesFactList GetSpeciesFacts(List<WebSpeciesFact> webSpeciesFacts,
                                                       UserParameterSelection userParameterSelection)
        {
            TaxonList taxa;

            if (userParameterSelection.Hosts.IsEmpty())
            {
                if (userParameterSelection.Taxa.IsNull())
                {
                    taxa = new TaxonList(true);
                }
                else
                {
                    taxa = userParameterSelection.Taxa;
                }
            }
            else
            {
                taxa = new TaxonList(true);
                taxa.Merge(userParameterSelection.Taxa);
                taxa.Merge(userParameterSelection.Hosts);
            }
            return GetSpeciesFacts(webSpeciesFacts, taxa);
        }

        /// <summary>
        /// Get information about species facts that correspond to all the combinations of parmeters in the user parmeter selection.
        /// In case data values exsists in database values are provided otherwise values is set to defaults
        /// When user parameter selection is incomplete, i.e. som parameter lists is empty, the user parameter selection is complemented.
        /// </summary>
        /// <param name="userParameterSelection">The user parmeter selection.</param>
        /// <returns>Information about species facts.</returns>
        /// <exception cref="ArgumentException">Thrown if user parameter selection is null or incomplete.</exception>
        public static SpeciesFactList GetSpeciesFactsByUserParameterSelection(UserParameterSelection userParameterSelection)
        {
            SpeciesFactList speciesFacts;
            List<WebSpeciesFact> webSpeciesFacts;
            WebUserParameterSelection webUserParameterSelection;

            // Check arguments.
            userParameterSelection.CheckNotNull("userParameterSelection");

            // Get data from web service.
            webUserParameterSelection = GetUserParameterSelection(userParameterSelection);
            webSpeciesFacts = WebServiceClient.GetSpeciesFactsByUserParameterSelection(webUserParameterSelection);
            speciesFacts = GetSpeciesFacts(webSpeciesFacts, userParameterSelection);

            return speciesFacts;
        }

        /// <summary>
        /// Get information about species facts that correspond to all the combinations of parmeters in the user parmeter selection.
        /// In case data values exsists in database values are provided otherwise values is set to defaults
        /// When user parameter selection is incomplete, i.e. som parameter lists is empty, the user parameter selection is complemented.
        /// This method is equivalent to GetDyntaxaSpeciesFactsByUserParameterSelection but adjusted for use in Dyntaxa Web Application.
        /// </summary>
        /// <param name="userParameterSelection">The user parmeter selection.</param>
        /// <returns>Information about species facts.</returns>
        /// <exception cref="ArgumentException">Thrown if user parameter selection is null or incomplete.</exception>
        public static SpeciesFactList GetDyntaxaSpeciesFactsByUserParameterSelection(UserParameterSelection userParameterSelection)
        {
            SpeciesFactList speciesFacts;
            List<WebSpeciesFact> webSpeciesFacts;
            WebUserParameterSelection webUserParameterSelection;
            

            // Check arguments.
            userParameterSelection.CheckNotNull("userParameterSelection");

            // Get data from web service.
            webUserParameterSelection = GetUserParameterSelection(userParameterSelection);
            webSpeciesFacts = WebServiceClient.GetSpeciesFactsByUserParameterSelection(webUserParameterSelection);
            speciesFacts = GetSpeciesFacts(webSpeciesFacts, userParameterSelection);

            if (userParameterSelection.Factors.IsNotEmpty())
            {
                ExpandSpeciesFactListWithEmptySpeciesFacts(userParameterSelection, speciesFacts);
            }
            return speciesFacts;
        }

        /// <summary>
        /// Get information about occurence in swedish
        /// counties for specified taxon.
        /// </summary>
        /// <param name="taxon">Taxon.</param>
        /// <returns>Information about occurence in swedish counties for specified taxon.</returns>
        public static TaxonCountyOccurrenceList GetTaxonCountyOccurence(Taxon taxon)
        {
            TaxonCountyOccurrenceList countyOccurrencies;
            List<WebTaxonCountyOccurrence> webCountyOccurrencies;

            webCountyOccurrencies = WebServiceClient.GetTaxonCountyOccurence(taxon.Id);
            countyOccurrencies = new TaxonCountyOccurrenceList();
            foreach (WebTaxonCountyOccurrence webCountyOccurrence in webCountyOccurrencies)
            {
                countyOccurrencies.Add(new TaxonCountyOccurrence(taxon,
                                                                 webCountyOccurrence.CountyId,
                                                                 webCountyOccurrence.CountyOccurrence,
                                                                 webCountyOccurrence.IsSourceIdSpecified,
                                                                 webCountyOccurrence.SourceId,
                                                                 webCountyOccurrence.Source,
                                                                 webCountyOccurrence.IsArtDataIdSpecified,
                                                                 webCountyOccurrence.ArtDataId,
                                                                 webCountyOccurrence.OriginalCountyOccurrence));
            }
            return countyOccurrencies;
        }

        /// <summary>
        /// Get a user data set based on information provided
        /// by a user parameter selection object.
        /// </summary>
        /// <param name="userParameterSelection"></param>
        /// <param name="userDataSet"></param>
        /// <exception cref="ArgumentException">Thrown if user parameter selection is null.</exception>
        public static void GetUserDataSetByParameterSelection(UserDataSet userDataSet, UserParameterSelection userParameterSelection)
        {
            Factor factor;
            Int32 factorIndex;
            SpeciesFactList speciesFacts;

            // Check arguments.
            userDataSet.CheckNotNull("userDataSet");
            userParameterSelection.CheckNotNull("userParameterSelection");
            userParameterSelection.Taxa.CheckNotEmpty("userParameterSelection.Taxa");
            userParameterSelection.Factors.CheckNotEmpty("userParameterSelection.Factors");

            // Add dependent factors to UserParameterSelection if necessary.
            for (factorIndex = 0; factorIndex < userParameterSelection.Factors.Count; factorIndex++)
            {
                factor = userParameterSelection.Factors[factorIndex];
                userParameterSelection.Factors.Merge(factor.GetDependentFactors());
            }

            speciesFacts = GetSpeciesFactsByUserParameterSelection(userParameterSelection);

            // Expand userParameterSelection where necessary.
            ExpandUserParameterSelectionBySpeciesFacts(userParameterSelection, speciesFacts);

            // Get missing species facts according to expanded combinations of user parameters
            ExpandSpeciesFactListWithEmptySpeciesFacts(userParameterSelection, speciesFacts);

            // Init automatic calculation.
            InitAutomatedCalculations(speciesFacts);

            if (userDataSet.SpeciesFacts.IsEmpty())
            {
                userDataSet.SpeciesFacts = speciesFacts;
            }
            else if (speciesFacts.IsNotEmpty())
            {
                foreach (SpeciesFact fact in speciesFacts)
                {
                    if (!userDataSet.SpeciesFacts.Exists(fact.Identifier))
                    {
                        userDataSet.SpeciesFacts.Add(fact);
                    }
                }
            }

            userDataSet.Factors.Merge(userParameterSelection.Factors);
            userDataSet.Hosts.Merge(userParameterSelection.Hosts);
            userDataSet.IndividualCategories.Merge(userParameterSelection.IndividualCategories);
            userDataSet.Periods.Merge(userParameterSelection.Periods);
            userDataSet.References.Merge(userParameterSelection.References);
            userDataSet.Taxa.Merge(userParameterSelection.Taxa);
        }

        /// <summary>
        /// Get a user data set based on information provided a user parameter selection object
        /// </summary>
        /// <param name="userParameterSelection"></param>
        /// <returns>A user data set</returns>
        /// <exception cref="ArgumentException">Thrown if user parameter selection is null.</exception>
        public static UserDataSet GetUserDataSetByParameterSelection(UserParameterSelection userParameterSelection)
        {
            UserDataSet userDataSet = new UserDataSet();
            GetUserDataSetByParameterSelection(userDataSet, userParameterSelection);

            return userDataSet;
        }

        /// <summary>
        /// Convert UserParameterSelection to WebUserParameterSelection.
        /// </summary>
        /// <param name="userParameterSelection">A UserParameterSelection instance.</param>
        /// <returns>A WebUserParameterSelection instance.</returns>
        public static WebUserParameterSelection GetUserParameterSelection(UserParameterSelection userParameterSelection)
        {
            WebUserParameterSelection webUserParameterSelection;

            webUserParameterSelection = new WebUserParameterSelection();
            webUserParameterSelection.FactorIds = null;
            webUserParameterSelection.HostIds = null;
            webUserParameterSelection.IndividualCategoryIds = null;
            webUserParameterSelection.PeriodIds = null;
            webUserParameterSelection.ReferenceIds = null;
            webUserParameterSelection.TaxonIds = null;

            if (userParameterSelection.Factors.IsNotEmpty())
            {
                webUserParameterSelection.FactorIds = new List<Int32>();
                foreach (Factor factor in userParameterSelection.Factors)
                {
                    webUserParameterSelection.FactorIds.Add(factor.Id);
                }
            }

            if (userParameterSelection.Hosts.IsNotEmpty())
            {
                webUserParameterSelection.HostIds = new List<Int32>();
                foreach (Taxon host in userParameterSelection.Hosts)
                {
                    webUserParameterSelection.HostIds.Add(host.Id);
                }
            }

            if (userParameterSelection.IndividualCategories.IsNotEmpty())
            {
                webUserParameterSelection.IndividualCategoryIds = new List<Int32>();
                foreach (IndividualCategory individualCategory in userParameterSelection.IndividualCategories)
                {
                    webUserParameterSelection.IndividualCategoryIds.Add(individualCategory.Id);
                }
            }

            if (userParameterSelection.Periods.IsNotEmpty())
            {
                webUserParameterSelection.PeriodIds = new List<Int32>();
                foreach (Period period in userParameterSelection.Periods)
                {
                    webUserParameterSelection.PeriodIds.Add(period.Id);
                }
            }

            if (userParameterSelection.References.IsNotEmpty())
            {
                webUserParameterSelection.ReferenceIds = new List<Int32>();
                foreach (Reference reference in userParameterSelection.References)
                {
                    webUserParameterSelection.ReferenceIds.Add(reference.Id);
                }
            }

            if (userParameterSelection.Taxa.IsNotEmpty())
            {
                webUserParameterSelection.TaxonIds = new List<Int32>();
                foreach (Taxon taxon in userParameterSelection.Taxa)
                {
                    webUserParameterSelection.TaxonIds.Add(taxon.Id);
                }
            }

            return webUserParameterSelection;
        }

        /// <summary>
        /// Make automated calculations of species facts that are "automatic" in a species fact list. 
        /// </summary>
        /// <param name="speciesFacts"></param>
        ///<remarks>No unit test</remarks>
        public static void InitAutomatedCalculations(SpeciesFactList speciesFacts)
        {
            foreach (SpeciesFact speciesFact in speciesFacts)
            {
                switch (speciesFact.Factor.Id)
                {
                    case (Int32)(FactorId.RedListCategoryAutomatic):
                        ((SpeciesFactRedListCategory)(speciesFact)).Init(speciesFacts);
                        break;
                    case (Int32)(FactorId.RedListCriteriaAutomatic):
                        ((SpeciesFactRedListCriteria)(speciesFact)).Init(speciesFacts);
                        break;
                    case (Int32)(FactorId.RedListCriteriaDocumentationAutomatic):
                        ((SpeciesFactRedListCriteriaDocumentation)(speciesFact)).Init(speciesFacts);
                        break;
                    /*case (Int32)(FactorId.NNAutomaticTaxonNameSummary):
                        ((SpeciesFactTaxonNameSummary)(speciesFact)).Init(speciesFacts);
                        break;*/
                }
            }
        }

        /// <summary>
        /// Set possible species fact qualitiy values.
        /// </summary>
        /// <param name="speciesFactQualityList">Possible species fact qualitiy values.</param>
        public static void InitialiseSpeciesFactQualities(SpeciesFactQualityList speciesFactQualityList)
        {
            SpeciesFactQualities = speciesFactQualityList;
        }

        /// <summary>
        /// Get organism groups from web service.
        /// </summary>
        private static void LoadOrganismGroups()
        {
            Hashtable allOrganismGroups;
            OrganismGroupList organismGroups;

            if (OrganismGroups.IsNull())
            {
                // Get data from web service.
                allOrganismGroups = new Hashtable();
                foreach (OrganismGroupType type in Enum.GetValues(typeof(OrganismGroupType)))
                {
                    switch (type)
                    {
                        case OrganismGroupType.Standard:
                            organismGroups = new OrganismGroupList();
                            foreach (FactorFieldEnumValue enumValue in FactorManager.GetFactorFieldEnum(FactorFieldEnumId.OrganismGroup).Values)
                            {
                                organismGroups.Add(new OrganismGroup(enumValue.KeyInt,
                                                                     enumValue.SortOrder,
                                                                     enumValue.OriginalLabel,
                                                                     type,
                                                                     enumValue.Information));
                            }
                            allOrganismGroups.Add(type, organismGroups);
                            break;
                        default:
                            throw new ApplicationException("Unhandled organism group type " + type);
                    }
                }
                OrganismGroups = allOrganismGroups;
            }
        }

        /// <summary>
        /// Get species fact qualities from web service.
        /// </summary>
        private static void LoadSpeciesFactQualities()
        {
            SpeciesFactQualityList speciesFactQualities;

            if (SpeciesFactQualities.IsNull())
            {
                // Get data from web service.
                speciesFactQualities = new SpeciesFactQualityList();
                foreach (WebSpeciesFactQuality webSpeciesFactQuality in WebServiceClient.GetSpeciesFactQualities())
                {
                    speciesFactQualities.Add(new SpeciesFactQuality(webSpeciesFactQuality.Id,
                                                                    webSpeciesFactQuality.Name,
                                                                    webSpeciesFactQuality.Definition,
                                                                    webSpeciesFactQuality.Id));
                }
                SpeciesFactQualities = speciesFactQualities;
            }
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        private static void RefreshCache()
        {
            OrganismGroups = null;
            SpeciesFactQualities = null;
        }

        /// <summary>
        /// Update species facts with latest information from database.
        /// This metod works on species facts both with and without id.
        /// </summary>
        /// <param name="speciesFacts">Species facts to update.</param>
        public static void UpdateSpeciesFacts(SpeciesFactList speciesFacts)
        {
            SpeciesFactList updatedSpeciesFacts;
            String speciesFactIdentifier;
            List<WebSpeciesFact> newWebSpeciesFacts, oldWebSpeciesFacts;

            // Get updated data from web service.
            oldWebSpeciesFacts = GetSpeciesFacts(speciesFacts, null);
            newWebSpeciesFacts = WebServiceClient.GetSpeciesFactsByIdentifier(oldWebSpeciesFacts);

            // Update species facts
            updatedSpeciesFacts = new SpeciesFactList();
            foreach (WebSpeciesFact webSpeciesFact in newWebSpeciesFacts)
            {
                // Get species fact.
                speciesFactIdentifier = GetSpeciesFactIdentifier(webSpeciesFact);
                SpeciesFact speciesFact = speciesFacts.Get(speciesFactIdentifier);

                // Update species fact.
                speciesFact.Update(webSpeciesFact.Id,
                                   webSpeciesFact.ReferenceId,
                                   webSpeciesFact.UpdateDate,
                                   webSpeciesFact.UpdateUserFullName,
                                   webSpeciesFact.IsFieldValue1Specified,
                                   webSpeciesFact.FieldValue1,
                                   webSpeciesFact.IsFieldValue2Specified,
                                   webSpeciesFact.FieldValue2,
                                   webSpeciesFact.IsFieldValue3Specified,
                                   webSpeciesFact.FieldValue3,
                                   webSpeciesFact.IsFieldValue4Specified,
                                   webSpeciesFact.FieldValue4,
                                   webSpeciesFact.IsFieldValue5Specified,
                                   webSpeciesFact.FieldValue5,
                                   webSpeciesFact.QualityId);
                updatedSpeciesFacts.Add(speciesFact);
            }

            // Update species facts that has been deleted.
            foreach (SpeciesFact speciesFact in speciesFacts)
            {
                if (speciesFact.HasId &&
                    !updatedSpeciesFacts.Exists(speciesFact.Identifier))
                {
                    speciesFact.Reset();
                }
            }
        }

        /// <summary>
        /// Set new values for species facts.
        /// This method updates information in web service/database
        /// but it does not update the species fact objects in the
        /// client. 
        /// </summary>
        /// <param name="speciesFacts">Species facts to set.</param>
        /// <param name="defaultReference">Reference used if no reference is specified.</param>
        public static void UpdateSpeciesFacts(SpeciesFactList speciesFacts,
                                              Reference defaultReference)
        {
            SpeciesFactList changedSpeciesFacts;
            SpeciesFactList createSpeciesFacts;
            SpeciesFactList deleteSpeciesFacts;
            SpeciesFactList updateSpeciesFacts;
            List<WebSpeciesFact> webCreateSpeciesFacts;
            List<WebSpeciesFact> webDeleteSpeciesFacts;
            List<WebSpeciesFact> webUpdateSpeciesFacts;

            // Check arguments.
            speciesFacts.CheckNotNull("speciesFacts");
            defaultReference.CheckNotNull("defaultReference");

            // Get all species facts that should be updated.
            changedSpeciesFacts = new SpeciesFactList();
            foreach (SpeciesFact speciesFact in speciesFacts)
            {
                if (speciesFact.AllowUpdate && speciesFact.HasChanged)
                {
                    changedSpeciesFacts.Add(speciesFact);
                }
            }
            if (changedSpeciesFacts.IsEmpty())
            {
                // Nothing to update.
                return;
            }

            // Split species facts that should be created, updated
            // or deleted into three different lists.
            createSpeciesFacts = new SpeciesFactList();
            deleteSpeciesFacts = new SpeciesFactList();
            updateSpeciesFacts = new SpeciesFactList();
            foreach (SpeciesFact speciesFact in changedSpeciesFacts)
            {
                if (!speciesFact.HasId &&
                    speciesFact.ShouldBeSaved &&
                    !speciesFact.ShouldBeDeleted)
                {
                    // Create new species fact.
                    createSpeciesFacts.Add(speciesFact);
                    continue;
                }

                if (speciesFact.HasId &&
                    speciesFact.ShouldBeDeleted)
                {
                    // Delete species fact.
                    deleteSpeciesFacts.Add(speciesFact);
                    continue;
                }

                if (speciesFact.HasId &&
                    speciesFact.ShouldBeSaved &&
                    !speciesFact.ShouldBeDeleted)
                {
                    // Update species fact.
                    updateSpeciesFacts.Add(speciesFact);
                    continue;
                }
            }
            webCreateSpeciesFacts = GetSpeciesFacts(createSpeciesFacts, defaultReference);
            webDeleteSpeciesFacts = GetSpeciesFacts(deleteSpeciesFacts, defaultReference);
            webUpdateSpeciesFacts = GetSpeciesFacts(updateSpeciesFacts, defaultReference);

            // Update species facts.
            WebServiceClient.UpdateSpeciesFacts(webCreateSpeciesFacts, webDeleteSpeciesFacts, webUpdateSpeciesFacts);
        }

        /// <summary>
        /// Update species facts with latest information from database.
        /// This metod works on species facts both with and without id.
        /// This method should be used by Dyntaxa application user only.
        /// </summary>
        /// <param name="speciesFacts">Species facts to update.</param>
        public static void UpdateDyntaxaSpeciesFacts(SpeciesFactList speciesFacts)
        {
            SpeciesFactList updatedSpeciesFacts;
            String speciesFactIdentifier;
            List<WebSpeciesFact> newWebSpeciesFacts, oldWebSpeciesFacts;

            // Get updated data from web service.
            oldWebSpeciesFacts = GetSpeciesFacts(speciesFacts, null);
            newWebSpeciesFacts = WebServiceClient.GetSpeciesFactsByIdentifier(oldWebSpeciesFacts);

            // Update species facts
            updatedSpeciesFacts = new SpeciesFactList();
            foreach (WebSpeciesFact webSpeciesFact in newWebSpeciesFacts)
            {
                // Get species fact.
                speciesFactIdentifier = GetSpeciesFactIdentifier(webSpeciesFact);
                SpeciesFact speciesFact = speciesFacts.Get(speciesFactIdentifier);

                // Update species fact.
                speciesFact.Update(webSpeciesFact.Id,
                                   webSpeciesFact.ReferenceId,
                                   webSpeciesFact.UpdateDate,
                                   webSpeciesFact.UpdateUserFullName,
                                   webSpeciesFact.IsFieldValue1Specified,
                                   webSpeciesFact.FieldValue1,
                                   webSpeciesFact.IsFieldValue2Specified,
                                   webSpeciesFact.FieldValue2,
                                   webSpeciesFact.IsFieldValue3Specified,
                                   webSpeciesFact.FieldValue3,
                                   webSpeciesFact.IsFieldValue4Specified,
                                   webSpeciesFact.FieldValue4,
                                   webSpeciesFact.IsFieldValue5Specified,
                                   webSpeciesFact.FieldValue5,
                                   webSpeciesFact.QualityId);
                updatedSpeciesFacts.Add(speciesFact);
            }

            // Update species facts that has been deleted.
            foreach (SpeciesFact speciesFact in speciesFacts)
            {
                if (speciesFact.HasId &&
                    !updatedSpeciesFacts.Exists(speciesFact.Identifier))
                {
                    speciesFact.Reset();
                }
            }
        }

        /// <summary>
        /// Set new values for species facts.
        /// This method updates information in web service/database
        /// but it does not update the species fact objects in the
        /// client. 
        /// This method should be used by Dyntaxa application user only.
        /// </summary>
        /// <param name="speciesFacts">Species facts to set.</param>
        /// <param name="defaultReference">Reference used if no reference is specified.</param>
        /// <param name="fullName">Fulle name of editor.</param>
        public static void UpdateDyntaxaSpeciesFacts(SpeciesFactList speciesFacts,
                                                     Reference defaultReference,
                                                     String fullName)
        {
            SpeciesFactList changedSpeciesFacts;
            SpeciesFactList createSpeciesFacts;
            SpeciesFactList deleteSpeciesFacts;
            SpeciesFactList updateSpeciesFacts;
            List<WebSpeciesFact> webCreateSpeciesFacts;
            List<WebSpeciesFact> webDeleteSpeciesFacts;
            List<WebSpeciesFact> webUpdateSpeciesFacts;

            // Check arguments.
            speciesFacts.CheckNotNull("speciesFacts");
            defaultReference.CheckNotNull("defaultReference");

            // Get all species facts that should be updated.
            changedSpeciesFacts = new SpeciesFactList();
            foreach (SpeciesFact speciesFact in speciesFacts)
            {
                if (speciesFact.AllowUpdate && speciesFact.HasChanged)
                {
                    changedSpeciesFacts.Add(speciesFact);
                }
            }
            if (changedSpeciesFacts.IsEmpty())
            {
                // Nothing to update.
                return;
            }

            // Split species facts that should be created, updated
            // or deleted into three different lists.
            createSpeciesFacts = new SpeciesFactList();
            deleteSpeciesFacts = new SpeciesFactList();
            updateSpeciesFacts = new SpeciesFactList();
            foreach (SpeciesFact speciesFact in changedSpeciesFacts)
            {
                if (!speciesFact.HasId &&
                    speciesFact.ShouldBeSaved &&
                    !speciesFact.ShouldBeDeleted)
                {
                    // Create new species fact.
                    createSpeciesFacts.Add(speciesFact);
                    continue;
                }

                if (speciesFact.HasId &&
                    speciesFact.ShouldBeDeleted)
                {
                    // Delete species fact.
                    deleteSpeciesFacts.Add(speciesFact);
                    continue;
                }

                if (speciesFact.HasId &&
                    speciesFact.ShouldBeSaved &&
                    !speciesFact.ShouldBeDeleted)
                {
                    // Update species fact.
                    updateSpeciesFacts.Add(speciesFact);
                    continue;
                }
            }
            webCreateSpeciesFacts = GetSpeciesFacts(createSpeciesFacts, defaultReference);
            webDeleteSpeciesFacts = GetSpeciesFacts(deleteSpeciesFacts, defaultReference);
            webUpdateSpeciesFacts = GetSpeciesFacts(updateSpeciesFacts, defaultReference);

            // Update species facts.
            WebServiceClient.UpdateDyntaxaSpeciesFacts(webCreateSpeciesFacts, webDeleteSpeciesFacts, webUpdateSpeciesFacts, fullName);
        }
    }
}

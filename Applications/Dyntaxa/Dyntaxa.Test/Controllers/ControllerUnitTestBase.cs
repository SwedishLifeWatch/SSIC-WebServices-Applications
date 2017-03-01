using System;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebService.Client.AnalysisService;
using ArtDatabanken.WebService.Client.GeoReferenceService;
using ArtDatabanken.WebService.Client.ReferenceService.Fakes;
using ArtDatabanken.WebService.Client.SpeciesObservationService;
using ArtDatabanken.WebService.Client.TaxonService;
using ArtDatabanken.WebService.Client.UserService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Resources;

namespace Dyntaxa.Test
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Generic.Fakes;
    using System.Runtime.Remoting.Messaging;
    using System.Security.AccessControl;
    using System.Web;
    using System.Web.Fakes;    
    using ArtDatabanken.Data.Fakes;
    using ArtDatabanken.Data.WebService.Fakes;
    using ArtDatabanken.WebApplication.Dyntaxa;
    using ArtDatabanken.WebService.Client.PESINameService;
    using ArtDatabanken.WebService.Client.PictureService;
    using ArtDatabanken.WebService.Client.TaxonAttributeService;
    using ArtDatabanken.WebService.Client.TaxonAttributeService.Fakes;

    using Dyntaxa.Controllers;
    using Dyntaxa.Test;
    using Dyntaxa.Test.TestModels;

    using Factor = ArtDatabanken.Data.Factor;
    using FactorDataType = ArtDatabanken.Data.FactorDataType;
    using FactorField = ArtDatabanken.Data.FactorField;
    using FactorFieldDataTypeId = ArtDatabanken.Data.FactorFieldDataTypeId;
    using FactorFieldEnum = ArtDatabanken.Data.FactorFieldEnum;
    using FactorFieldEnumId = ArtDatabanken.Data.FactorFieldEnumId;
    using FactorFieldList = ArtDatabanken.Data.FactorFieldList;
    using FactorUpdateMode = ArtDatabanken.Data.FactorUpdateMode;
    using IndividualCategory = ArtDatabanken.Data.IndividualCategory;
    using IndividualCategoryList = ArtDatabanken.Data.IndividualCategoryList;
    using Reference = ArtDatabanken.Data.Reference;
    using ReferenceManager = ArtDatabanken.Data.ReferenceManager;
    using SpeciesFact = ArtDatabanken.Data.SpeciesFact;
    using SpeciesFactList = ArtDatabanken.Data.SpeciesFactList;
    using SpeciesFactQuality = ArtDatabanken.Data.SpeciesFactQuality;
    using SpeciesFactQualityList = ArtDatabanken.Data.SpeciesFactQualityList;
    using SpeciesObservationManager = ArtDatabanken.Data.SpeciesObservationManager;
    using Taxon = ArtDatabanken.Data.Taxon;
    using TaxonList = ArtDatabanken.Data.TaxonList;
    using TaxonName = ArtDatabanken.Data.TaxonName;
    using TaxonNameList = ArtDatabanken.Data.TaxonNameList;

    [TestClass]
    public class ControllerUnitTestBase:ControllerTestBase
    {

       

        /// <summary>
        /// Use TestInitialize to run code before running each test.
        /// </summary>
        [TestInitialize]
        public void MyTestInitialize()
        {
            // Creates the stubbed application user context.
            ApplicationUserContext = GetStubApplicationUserContex(false);
            ApplicationUserContextSV = GetStubApplicationUserContex(true);

            // Connect all stubbed managers (data core) including stubbed dataSorces here. 
            StubDataSorcesAndManagers();

           // this.LoginApplicationUserAndSetSessinVariables(swedishApplicationUserContext);
        }

        /// <summary>
        /// The login application user and set session variables.
        /// </summary>
        public void LoginApplicationUserAndSetSessionVariables()
        {
            // Log in application user.
            this.LoginApplicationUser(ApplicationUserContextSV, ApplicationUserContext);
            SessionRevision = TaxonDataSourceTestRepositoryData.GetTaxonRevision(ApplicationUserContext, DyntaxaTestSettings.Default.TestRevisionId);
        }

        /// <summary>
        /// Use TestCleanup to run code after each test has run.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                this.LogoutApplicationUser();
            }
            catch
            {
                // Test is done.
                // We are not interested in problems that
                // occures due to test of error handling.
            }
        }

        /// <summary>
        /// Method for login stubbed test user.
        /// </summary>
        /// <param name="accountController">
        /// The account Controller.
        /// </param>
        public void LoginStubbedTestUser()
        {
            IUserContext loginUserContext = this.GetStubUserContext();
            LoginTestUser(loginUserContext);
        }


        #region Stub Managers and Data sources

        /// <summary>
        /// All dataSources and managers (core data) are stubbed here. 
        /// </summary>
        private void StubDataSorcesAndManagers()
        {
            // Stub Managers
            StubUserManager();
            StubCountryManager();
            StubReferenceManager();
            StubTaxonManager();
            StubLocaleManager();
            StubFactorManager();
            StubSpeciesFactManager();  
        }

        /// <summary>
        /// Shim od speciesfact implemetation here. TODO replace with new when done...
        /// </summary>
        public static void ShimSpeciesFactModelManager()
        {
            LockespindlarSpeciesFactTest speciesFactTest = new LockespindlarSpeciesFactTest(ApplicationUserContext);

            // Add testdata
            SpeciesFactList speciesFactTestList = speciesFactTest.SpeciesFactList;
            IList<DyntaxaSpeciesFact> dyntaxaSpeciesFacts = new List<DyntaxaSpeciesFact>();
            IList<DyntaxaFactor> dyntaxaFactor = new List<DyntaxaFactor>();

            // Shim specifact model manager, set methods.
            ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimSpeciesFactModelManager.AllInstances.AddFactorToTaxonInt32Int32StringInt32BooleanInt32Int32 = (taxonId, factorToBeAddedId, referenceId, mainParentFactorId, setHost, hostTaxonId, individualCategory, last)
            => { return; };
            ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimSpeciesFactModelManager.AllInstances.AddNewCategoryToSpeciecFactInt32Int32Int32Int32StringInt32Int32 = (taxonId, factorToBeAddedId, referenceId, mainParentFactorId, setHost, hostTaxonId, individualCategory, last)
            => { return; };

            ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimSpeciesFactModelManager.AllInstances.UpdateDyntaxaSpeciesFacts = (speciesFacts) =>
            {
                return;
            };
            ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimSpeciesFactModelManager.AllInstances.UpdatedSpeciecFactsIListOfSpeciesFactFieldValueModelHelperBooleanBoolean = (newValuesInList, newCategory, updateFieldValue2, last) =>
            {
                return;
            };
            ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimSpeciesFactModelManager.AllInstances.UpdatedSpeciecFactsSpeciesFactFieldValueModelHelperIListOfDyntaxaSpeciesFact = (newCommonValues, speciesFacts, last) =>
            {
                return;
            };

            ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimSpeciesFactModelManager.AllInstances.SetNewNullValueForSpeciesFactForTaxonFactorIdInt32String = (factor, id, newValue, last)
           => { return; };

            ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimSpeciesFactModelManager.AllInstances.SetNewValueForSpeciesFactForTaxonFactorIdInt32Object = (factor, id, newValue, last)
         => { return; };

            ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimSpeciesFactModelManager.AllInstances.GetAllSpeciesFact = (speciesFact) =>
            {
                return new DyntaxaAllFactorData(dyntaxaFactor, new List<DyntaxaHost>(), new List<DyntaxaIndividualCategory>(), new List<DyntaxaPeriod>(), dyntaxaSpeciesFacts,
                                                DyntaxaTestSettings.Default.ParnassiusApolloId, "occurance string", "history string", new List<DyntaxaHost>());
            };
            ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimSpeciesFactModelManager.AllInstances.GetSpeciesFactFromTaxonAndFactorStringStringString = (childFactorId, individualCategoryId, hostId, last) =>
            {
                return dyntaxaSpeciesFacts[0];
            };
            ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimSpeciesFactModelManager.AllInstances.GetFactorsFromTaxonAndParentFactorBoolean = (childFactorId, last) =>
            {
                return new DyntaxaAllFactorData(dyntaxaFactor, new List<DyntaxaHost>(), new List<DyntaxaIndividualCategory>(), new List<DyntaxaPeriod>(), dyntaxaSpeciesFacts,
                                                DyntaxaTestSettings.Default.ParnassiusApolloId, "occurance string", "history string", new List<DyntaxaHost>());
            };

            ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimSpeciesFactModelManager.GetSpeciesFactIdentifierITaxonDyntaxaIndividualCategoryDyntaxaFactorDyntaxaHostDyntaxaPeriod = (taxon, individualCategoryId, factor, host, period) =>
            {
                return "";
            };

            ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimSpeciesFactModelManager.AllInstances.GetFactorsFromFactorIdAndFactorDataTypeString = (factorDataTypeId, last) =>
            {
                return dyntaxaFactor;
            };
            ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimSpeciesFactModelManager.AllInstances.GetFactorsFromFactorIdInt32Int32 = (factorid, factorDataTypeId, last) =>
            {
                return dyntaxaFactor;
            };
            ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimSpeciesFactModelManager.AllInstances.GetFactorsFromFactorIdAndFactorDataTypeSubstrateString = (factorDataTypeId, last) =>
            {
                return dyntaxaFactor;
            };
            ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimSpeciesFactModelManager.AllInstances.GetAllIndividualCategories = (last) =>
            {
                return new List<DyntaxaIndividualCategory>();
            };

            ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimSpeciesFactModelManager.AllInstances.GetIndividualCategoryInt32 = (id, last) =>
            {
                return new DyntaxaIndividualCategory(12, "categorylabel", "factorValue");
            };

            ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimSpeciesFactModelManager.AllInstances.GetHostByFactorIdInt32 = (factoreId, last) =>
            {
                return new List<DyntaxaHost>();
            };
            ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimSpeciesFactModelManager.AllInstances.GetSpeciesFactFromSelectedTaxaAndFactorsInt32Int32ListOfSpeciesFactHostsIdListHelperListOfSpeciesFactHostsIdListHelperBoolean = (taxonId, factorId, factorHostData, taxonIds, useAllCategories, last) =>
            {
                return dyntaxaSpeciesFacts;
            };
            ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimSpeciesFactModelManager.AllInstances.GetSpeciesFactFromHostInt32Int32Int32 = (hostTaxonId, categoryId, hostFactorId, last) =>
            {
                return dyntaxaSpeciesFacts;
            };
            ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimSpeciesFactModelManager.AllInstances.GetSpeciesFactFromFactorInt32Int32IListOfInt32ListOfSpeciesFactHostsIdListHelper = (taxonId, factorId, categoryIds, selectedHosts, last) =>
            {
                return dyntaxaSpeciesFacts;
            };
            ArtDatabanken.WebApplication.Dyntaxa.Data.Fakes.ShimSpeciesFactModelManager.AllInstances.GetHostNameInt32 = (hostId, last) =>
            {
                return "hostName";
            };                  

            speciesFactTest.AddSpeciesFactTestData(new LockespindlarTaxaTest());
            IList<KeyValuePair<int,string>> keyValuePairs = new List<KeyValuePair<int, string>>();
            foreach (SpeciesFact spFactor in speciesFactTestList)
            {
                dyntaxaSpeciesFacts.Add(new DyntaxaSpeciesFact(spFactor.Id.ToString(), "Information", "Label", spFactor.Factor.IsLeaf,
                                                                               spFactor.Factor.IsPeriodic, 14515, spFactor.Factor.IsPublic, spFactor.Factor.IsTaxonomic,
                                                                               "HostLabel", 0, new DyntaxaFactorQuality(3,"QualityName", keyValuePairs), 
                                                                               10, new List<DyntaxaFactorField>(), new DyntaxaFactorOrigin(10, "origin name"), new DyntaxaFactorUpdateMode(false, true), spFactor.Identifier,
                                                                               spFactor.ModifiedDate, 
                                                                               new DyntaxaIndividualCategory(spFactor.IndividualCategory.Id, "IndividualCategoryName", "IndividualCategoryDefinition")
                                                                               , spFactor.Factor.Id,"ReferenceName"));
                dyntaxaFactor.Add(new DyntaxaFactor(Convert.ToString(spFactor.Factor.Id), "Label", spFactor.Factor.IsLeaf, spFactor.Factor.IsPeriodic, 14589, spFactor.Factor.IsPublic,spFactor.Factor.IsTaxonomic,
                                                    new DyntaxaFactorOrigin(10, "origin name"), new DyntaxaFactorUpdateMode(false, true), spFactor.Factor.Id));
            }
        }

        /// <summary>
        /// The stub of speciesFact manager. Missing implementation in code is added here. 
        /// </summary>
        private static void StubSpeciesFactManager()
        {
            LockespindlarSpeciesFactTest speciesFactTest = new LockespindlarSpeciesFactTest(ApplicationUserContext);

            // Add testdata
            SpeciesFactList speciesFactTestList = speciesFactTest.SpeciesFactList;
           
            SpeciesFactDataSource speciesFactDataSource = new StubSpeciesFactDataSource()
                                                              {                                                                               
                                                                  UpdateSpeciesFactsIUserContextSpeciesFactList =
                                                                      (userContex, speciesFactList)
                                                                          =>
                                                                      {
                                                                          return;
                                                                      }, 
                                                                  GetSpeciesFactsIUserContextISpeciesFactSearchCriteria = (userContext, searchCriteria) =>
                                                                  {return speciesFactTestList;}
                                                              };
           
            // Stub manager.
            ISpeciesFactManager speciesFactManager = new StubISpeciesFactManager
                                                        {
                                                            UpdateSpeciesFactsIUserContextSpeciesFactListIReference =
                                                                (userContext, speciesFactList, referenceValue) =>
                                                                {
                                                                    return;
                                                                },
                                                            GetSpeciesFactIdentifierITaxonIIndividualCategoryIFactorITaxonIPeriod = (taxon, category,factor,host,period) =>
                                                                {
                                                                    return "identifier";
                                                                },
                                                            GetSpeciesFactIdentifierInt32Int32Int32BooleanInt32BooleanInt32 = (taxonId,
                                                                               individualCategoryId,
                                                                               factorId,
                                                                               hasHost,
                                                                               hostId,
                                                                               hasPeriod,
                                                                               periodId) =>
                                                                {
                                                                    return taxonId.ToString() + factorId.ToString();
                                                                }
                                                        };

            CoreData.SpeciesFactManager.DataSource = speciesFactDataSource;
            CoreData.SpeciesFactManager = speciesFactManager;
            speciesFactTest.AddSpeciesFactTestData(new LockespindlarTaxaTest());

           
        }

        /// <summary>
        /// The stub of factor manager. Missing implementation in code is added here. 
        /// </summary>
        private static void StubFactorManager()
        {
            FactorDataSource factorDataSource = new StubFactorDataSource();
            Factor factor = new Factor();
            factor.UpdateMode = new FactorUpdateMode();
            factor.DataType = new FactorDataType();
            factor.DataType.Fields = new FactorFieldList();
            factor.DataType.Fields.Add(new FactorField()
                                           {
                                               Id = 109,
                                               Label = "field",
                                               DatabaseFieldName = "DBNAme",
                                               FactorDataType = new FactorDataType()
                                                                    {
                                                                        Fields = new FactorFieldList(),
                                                                        Id = (int)FactorFieldDataTypeId.String
                                                                    },
                                               Enum = new FactorFieldEnum()
                                                          {
                                                              Id = (int)FactorFieldEnumId.OrganismGroup
                                                          },
                                              Index = 3,

                                           });
            
            // Stub manager.
            IFactorManager factorManager = new StubIFactorManager
                                               {
                                                   GetFactorIUserContextFactorId = (userContext, fatcorId) =>
                                                       {
                                                           return factor;
                                                       },
                                                   GetFactorIUserContextInt32 = (userContext, fatcorId) =>
                                                   {
                                                       return factor;
                                                   },
                                                       GetIndividualCategoriesIUserContext = (userContext) =>
                                                       {
                                                           return new IndividualCategoryList();
                                                       },
                                                       GetIndividualCategoryIUserContextInt32 = (usercontext, Id) =>
                                                           {
                                                               return new IndividualCategory();
                                                           },   
                                                    GetFactorFieldMaxCount = () =>
                                                        {
                                                            return 4;
                                                        }
                                               };

            CoreData.FactorManager.DataSource = factorDataSource;
            CoreData.FactorManager = factorManager;
        }

        /// <summary>
        /// The stub of picture manager. Missing implementation in code is added here. 
        /// </summary>
        private static void StubPictureManager()
        {

            PictureDataSource pictureDataSource = new ArtDatabanken.WebService.Client.PictureService.Fakes.StubPictureDataSource();
            IPictureManager pictureManager = new ArtDatabanken.Data.Fakes.StubIPictureManager
            {
                // Add more implementation here
            };

            CoreData.PictureManager.DataSource = pictureDataSource;
            CoreData.PictureManager = pictureManager;
        }

        /// <summary>
        /// The stub of analaysis manager. Missing implementation in code is added here. 
        /// </summary>
        private static void StubAnalaysisManager()
        {
            AnalysisDataSource analysisDataSource = new ArtDatabanken.WebService.Client.AnalysisService.Fakes.StubAnalysisDataSource();
            IAnalysisManager analysisManager = new StubIAnalysisManager()
            {
                // Add implementation here
            };

            CoreData.AnalysisManager.DataSource = analysisDataSource;
            CoreData.AnalysisManager = analysisManager;
        }

        /// <summary>
        /// The stub of region manager. Missing implementation in code is added here. 
        /// </summary>
        private static void StubRegionManager()
        {
            RegionDataSource regionDataSource = new ArtDatabanken.WebService.Client.GeoReferenceService.Fakes.StubRegionDataSource();
            IRegionManager regionManager = new StubIRegionManager()
            {
                // Add implementation here
            };

            CoreData.RegionManager.DataSource = regionDataSource;
            CoreData.RegionManager = regionManager;

        }

        /// <summary>
        /// The stub of meta data manager. Missing implementation in code is added here. 
        /// </summary>
        private static void StubMetaDataManager()
        {
            SpeciesObservationDataSource speciesObservationDataSource = new ArtDatabanken.WebService.Client.SpeciesObservationService.Fakes.StubSpeciesObservationDataSource()
            {
            };

            CoreData.MetadataManager.SpeciesObservationDataSource = speciesObservationDataSource;
        }

        /// <summary>
        /// The stub of species observation manager. Missing implementation in code is added here. 
        /// </summary>
        private static void StubSpeciesObservationManager()
        {
            SpeciesObservationDataProviderList dataProviders = new SpeciesObservationDataProviderList
                                                                   {
                                                                       new SpeciesObservationDataProvider()
                                                                   };

            SpeciesObservationDataSource speciesObservationDataSource = new ArtDatabanken.WebService.Client.SpeciesObservationService.Fakes.StubSpeciesObservationDataSource();

            SpeciesObservationManager speciesObservationManager = new StubSpeciesObservationManager()
            {
                GetSpeciesObservationDataProvidersIUserContext = (context) => dataProviders };

            CoreData.SpeciesObservationManager = speciesObservationManager;
            CoreData.SpeciesObservationManager.DataSource = speciesObservationDataSource;
        }

        /// <summary>
        /// The stub of taxon manager. Missing implementation in code is added here. 
        /// </summary>
        private static void StubTaxonManager()
        {
            TaxonNameList nameList = new TaxonNameList();
            nameList.Add(new TaxonName()
                             {
                                 Id = 345,
                                 Name = "TaxonNameTest",
                                 
                             });
            List<TaxonNameList> taxonNameList = new List<TaxonNameList>();
            taxonNameList.Add(nameList);
            TaxonDataSource taxonDataSource = new ArtDatabanken.WebService.Client.TaxonService.Fakes.StubTaxonDataSource()
                                                  {
                                                     GetDataSourceInformation01 = () => TaxonDataSourceTestRepositoryData.GetDataSourceInformation(),
                                                     CreateTaxonNameIUserContextITaxonName = (userContext, taxonName) => TaxonDataSourceTestRepositoryData.CreateTaxonName(userContext, taxonName),
                                                     GetTaxonRevisionsIUserContextITaxonRevisionSearchCriteria = (userContext, searchCriteria) => TaxonDataSourceTestRepositoryData.GetTaxonRevisions(userContext, searchCriteria),
                                                     GetTaxonRevisionsIUserContextITaxon = (userContext, taxon) => TaxonDataSourceTestRepositoryData.GetTaxonRevisions(userContext, taxon),


                                                     GetTaxonIUserContextInt32 = (userContext, taxonId) => TaxonDataSourceTestRepositoryData.GetTaxon(userContext, taxonId),
                                                     GetTaxonRelationsIUserContextITaxonRelationSearchCriteria = (userContext, searchCriteria) =>
                                                     {
                                                         return TaxonDataSourceTestRepositoryData.GetTaxonRelations(null, null);
                                                     },
                                                     GetTaxonCategoriesIUserContext = (userContext) =>
                                                         {
                                                             return TaxonDataSourceTestRepositoryData.GetTaxonCategories(userContext);
                                                         },
                                                     GetTaxonCategoriesIUserContextITaxon = (userContext, taxon) =>
                                                     {
                                                         return TaxonDataSourceTestRepositoryData.GetTaxonCategories(userContext);
                                                     },
                                                   

                                                  };
            PesiNameDataSource PESIDataSource = new ArtDatabanken.WebService.Client.PESINameService.Fakes.StubPesiNameDataSource();
           

            ITaxonManager taxonManager = new StubITaxonManager()
            {
                GetTaxonRelationsIUserContextITaxonRelationSearchCriteria = (userContext, searchCriteria) =>
                {
                    return TaxonDataSourceTestRepositoryData.GetTaxonRelations(ApplicationUserContext, searchCriteria);
                },
                GetTaxonIUserContextInt32 = (userContext, taxonId) => TaxonDataSourceTestRepositoryData.GetTaxon(userContext, taxonId),
                GetTaxonCategoriesIUserContext = (userContext) =>
                {
                    return TaxonDataSourceTestRepositoryData.GetTaxonCategories(ApplicationUserContext);
                },
                GetTaxonCategoriesIUserContextITaxon = (userContext, taxon) =>
                {
                    return TaxonDataSourceTestRepositoryData.GetTaxonCategories(ApplicationUserContext);
                },
                GetTaxonPropertiesIUserContextITaxon = (userContext, taxon) => TaxonDataSourceTestRepositoryData.GetTaxonProperties(userContext, taxon),
                GetTaxonCategoryIUserContextInt32 = (userContext, category) => TaxonDataSourceTestRepositoryData.GetReferenceTaxonCategory(userContext, category),
                GetTaxonRevisionIUserContextInt32 = (userContext, revisionId) => TaxonDataSourceTestRepositoryData.GetTaxonRevision(userContext, revisionId),
                GetTaxonNamesIUserContextITaxon = (userContext, taxon) =>
                {
                    return nameList;
                },
                GetTaxonNamesIUserContextTaxonList = (userContext, taxonList) =>
                {
                    return taxonNameList;
                },
            };

            CoreData.TaxonManager = taxonManager;
            CoreData.TaxonManager.DataSource = taxonDataSource;
            CoreData.TaxonManager.PesiNameDataSource = PESIDataSource;
        }

        /// <summary>
        /// The stub of reference manager. Missing implementation in code is added here. 
        /// </summary>
        private static void StubReferenceManager()
        {
            StubReferenceDataSource referenceDataSource = new StubReferenceDataSource();
            ReferenceRelationList referenceRelationList = new ReferenceRelationList();
           
            IReferenceManager referenceManager = new StubIReferenceManager()
            {
                GetReferenceRelationsIUserContextString = (userContext, guid) =>
                    {
                        return referenceRelationList;
                    },
                    
                    GetReferenceRelationTypeIUserContextReferenceRelationTypeId = (userContext, type) =>
                        {
                            return new ReferenceRelationType()
                                       {
                                           Id = (int)ReferenceRelationTypeId.Source,
                                           Identifier = "referenceRelation"
                                       };
                        },
                GetReferenceIUserContextInt32 = (userContext, id) =>
                {
                    return new Reference();
                },                
            };
       
            CoreData.ReferenceManager = referenceManager;
            CoreData.ReferenceManager.DataSource = referenceDataSource;
        }


        /// <summary>
        /// The stub of application manager. Missing implementation in code is added here. 
        /// </summary>
        private static void StubApplicationManager()
        {
            ApplicationDataSource applicationDataSource = new ArtDatabanken.WebService.Client.UserService.Fakes.StubApplicationDataSource();

            IApplicationManager applicationManager = new StubIApplicationManager()
            {
                // Add implementation here
            };

            CoreData.ApplicationManager = applicationManager;
            CoreData.ApplicationManager.DataSource = applicationDataSource;
        }

        /// <summary>
        /// The stub of country manager. Missing implementation in code is added here. 
        /// </summary>
        private static void StubCountryManager()
        {
            CountryDataSource countryDataSource = new ArtDatabanken.WebService.Client.UserService.Fakes.StubCountryDataSource();


            ICountryManager countryManager = new StubICountryManager()
            {
                // Add implementation here
            };

            CoreData.CountryManager = countryManager;
            CoreData.CountryManager.DataSource = countryDataSource;

        }

        /// <summary>
        /// The stub of locale manager. Missing implementation in code is added here. 
        /// </summary>
        private void StubLocaleManager()
        {
            LocaleList usedLocales = new LocaleList();

            ILocale testEnLocale = new Locale(
                DyntaxaTestSettings.Default.EnglishLocaleId,
                DyntaxaTestSettings.Default.EnglishLocale,
                DyntaxaTestSettings.Default.EnglishNameString,
                DyntaxaTestSettings.Default.EnglishNameString,
                new DataContext(ApplicationUserContext));

            usedLocales.Add(testEnLocale);

            LocaleManager testLocaleManager = new StubLocaleManager()
            {
                GetUsedLocalesIUserContext = (context) => usedLocales,
                GetDefaultLocaleIUserContext = (context) => testEnLocale
            };

            LocaleDataSource localeDataSource = new ArtDatabanken.WebService.Client.UserService.Fakes.StubLocaleDataSource();
            {
                // Add implementation here
            }

            CoreData.LocaleManager = testLocaleManager;
            CoreData.LocaleManager.DataSource = localeDataSource;
        }

        /// <summary>
        /// The stub of organization manager. Missing implementation in code is added here. 
        /// </summary>
        private static void StubOrganizationManager()
        {
            UserDataSource userDataSource = new ArtDatabanken.WebService.Client.UserService.Fakes.StubUserDataSource();

            IOrganizationManager organizationManager = new StubIOrganizationManager()
            {
                // Add implementation here
            };

            CoreData.OrganizationManager = organizationManager;
            CoreData.OrganizationManager.DataSource = userDataSource;
        }

        /// <summary>
        /// The stub of user manager. Missing implementation in code is added here. 
        /// </summary>
        private void StubUserManager()
        {
            UserDataSource userDataSource = new ArtDatabanken.WebService.Client.UserService.Fakes.StubUserDataSource();

            IUserManager userManager = new StubIUserManager()
            {
                LoginStringStringStringBoolean =
                    (userName,
                     password,
                     applicationIdentifier,
                     isActivationRequired) =>
                    {
                        return UserContextData;
                    },
                LoginStringStringString =
                    (userName,
                     password,
                     applicationIdentifier) =>
                    {
                        return
                            ApplicationUserContext;
                    }
            };

            CoreData.UserManager = userManager;
            CoreData.UserManager.DataSource = userDataSource;
        }

        #endregion

       

        #region Stub application user context and login used context

        /// <summary>
        /// Gets a stub application user context.
        /// </summary>
        /// <param name="useSwedish">
        /// Indicates if application user context uses swedish locale otherwise english locale is used.
        /// </param>
        /// <returns>
        /// Stubbed application user context. <see cref="IUserContext"/>.
        /// </returns>
        private IUserContext GetStubApplicationUserContex(bool useSwedish)
        {

            IDataContext applicationDataContext = new StubIDataContext();

            ILocale applicationLocale;
            if (useSwedish)
            {
                applicationLocale = new Locale(
                     DyntaxaTestSettings.Default.SwedishLocaleId,
                     DyntaxaTestSettings.Default.SwedishLocale,
                     DyntaxaTestSettings.Default.SwedishNameString,
                     DyntaxaTestSettings.Default.SvenskNameString,
                     applicationDataContext);
            }
            else
            {
                applicationLocale = new Locale(DyntaxaTestSettings.Default.EnglishLocaleId,
               DyntaxaTestSettings.Default.EnglishLocale,
               DyntaxaTestSettings.Default.EnglishNameString,
               DyntaxaTestSettings.Default.EnglishNameString,

                    applicationDataContext);
            }

            IUser applicationUser = this.GetStubApplicationUser(applicationDataContext);
            IRole publicApplicationRole = GetStubPublicApplicationRole(DyntaxaSettings.Default.DyntaxaApplicationUserName, DyntaxaTestSettings.Default.TestDyntaxaApplicationId);
            TransactionList transactionList = new TransactionList();
            transactionList.Add(Transaction);
            IUserContext applicationUserContextClone = new StubIUserContext
            {
                LocaleSetILocale = (value) => applicationLocale = value,
                LocaleGet = () => applicationLocale,
                UserGet = () => applicationUser,
                CurrentRoleGet = () => publicApplicationRole
            };
            System.Collections.Hashtable table = new Hashtable();
            table.Add("WebServiceClientToken: UserService", "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAADBMHGOJPMCGHNMEDJJINANBHBHBAHDBJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAKODLBCBIEBJIOMKAPHEIFKDOALIBPAHPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAANIOIBKJOJONBGAFKINMLCPJKBBNEJFMKDAAAAAAANEJBAJHABLGJHJMPFILMENKCEBNLFKLOMNKHIEGKFIDLHGLNKFHKDBMGPJMFIGGAHMPDFJHDADMBPOKHCBPEHMAIKJKHPDPDBEAAAAAAEGMDAGONLGDGNCJLDGGIADCMGCLFDMEBPMIGNFCI");
            IUserContext applicationUserContext = new StubIUserContext
            {
                LocaleSetILocale = (value) => applicationLocale = value,
                LocaleGet = () => applicationLocale,
                UserGet = () => applicationUser,
                CurrentRoleGet = () => publicApplicationRole,
                Clone = () => applicationUserContextClone,
                PropertiesGet = () => { return table; },
                TransactionGet = () => { return Transaction; },
                StartTransaction = () => { return Transaction; },
                StartTransactionInt32 = (time) => { return Transaction; },
                TransactionsGet = () => { return transactionList; },
                
              
            };

            return applicationUserContext;
        }

        private ITransaction GetStubTransaction()
        {
            ITransaction transaction = new StubITransaction()
                                           {
                                               Commit = () => { return; },
                                           };
            return transaction;
        }

        /// <summary>
        /// Gets a stubbed user context for logged in user.
        /// </summary>
        /// <returns>
        /// Returns stubbed login user context <see cref="IUserContext"/>.
        /// </returns>
        protected IUserContext GetStubUserContext()
        {
            bool useSwedish = SessionHelper.GetFromSession<string>(DyntaxaTestSettings.Default.LanguageContextString).IsNotNull() && SessionHelper.GetFromSession<string>(DyntaxaTestSettings.Default.LanguageContextString).Equals(DyntaxaTestSettings.Default.SwedishLocale);

            IDataContext dataContext = new StubIDataContext();
            ILocale locale;

            if (useSwedish)
            {
                locale = new Locale(
                     DyntaxaTestSettings.Default.SwedishLocaleId,
                     DyntaxaTestSettings.Default.SwedishLocale,
                     DyntaxaTestSettings.Default.SwedishNameString,
                     DyntaxaTestSettings.Default.SvenskNameString,
                     dataContext);
            }
            else
            {
                locale = new Locale(DyntaxaTestSettings.Default.EnglishLocaleId,
               DyntaxaTestSettings.Default.EnglishLocale,
               DyntaxaTestSettings.Default.EnglishNameString,
               DyntaxaTestSettings.Default.EnglishNameString,

                    dataContext);
            }

            IUser user = this.GetStubLogInUser(dataContext);
            IRole testUserRole = GetStubTestUserRole(DyntaxaTestSettings.Default.TestUserName, DyntaxaTestSettings.Default.TestRoleId);
            TransactionList transactionList = new TransactionList();
            transactionList.Add(Transaction);
            IUserContext loginUserContext = new StubIUserContext
            {
                LocaleSetILocale = (value) => locale = value,
                LocaleGet = () => locale,
                UserGet = () => user,
                CurrentRoleGet = () => testUserRole,
                TransactionGet = () => { return Transaction; },
                StartTransaction = () => { return Transaction; },
                StartTransactionInt32 = (time) => { return Transaction; },
                TransactionsGet = () => { return transactionList; }
            };

            return loginUserContext;
        }

        /// <summary>
        /// The get stub for log in user.
        /// </summary>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <param name="multipleUsers">
        /// The multiple users.
        /// </param>
        /// <returns>
        /// Stubbed user <see cref="IUser"/>.
        /// </returns>
        public IUser GetStubLogInUser(IDataContext dataContext, bool multipleUsers = false)
        {

            int id = DyntaxaTestSettings.Default.TestUserId;

            if (multipleUsers)
            {
                id = id + 1;
            }

            UpdateInformation updateInformation = new UpdateInformation();
            updateInformation.CreatedBy = DyntaxaTestSettings.Default.TestUserId;
            updateInformation.CreatedDate = DateTime.Now;
            updateInformation.ModifiedBy = DyntaxaTestSettings.Default.TestUserId;
            updateInformation.ModifiedDate = DateTime.Now;

            IUser testUser = new StubIUser
            {
                UserNameGet = () => DyntaxaTestSettings.Default.TestUserName,
                ApplicationIdGet = () => DyntaxaTestSettings.Default.TestDyntaxaApplicationId,
                IsAccountActivatedGet = () => true,
                EmailAddressGet = () => DyntaxaTestSettings.Default.TestUserEmail,
                DataContextGet = () => dataContext,
                GUIDGet = () => DyntaxaTestSettings.Default.TestUserGuid,
                IdGet = () => id,
                ShowEmailAddressGet = () => true,
                TypeGet = () => UserType.Person,
                UpdateInformationGet = () => updateInformation,
                ValidFromDateGet = () => DateTime.Now,
                ValidToDateGet = () => new DateTime(2144, 12, 31) };

            return testUser;
        }

        /// <summary>
        /// The get stub for application user.
        /// </summary>
        /// <param name="dataContext">
        /// The data context.
        /// </param>
        /// <returns>
        /// Stubbed application user. <see cref="IUser"/>.
        /// </returns>
        public IUser GetStubApplicationUser(IDataContext dataContext)
        {
            UpdateInformation updateInformation = new UpdateInformation();
            updateInformation.CreatedBy = DyntaxaTestSettings.Default.TestUserId + 1;
            updateInformation.CreatedDate = DateTime.Now;
            updateInformation.ModifiedBy = DyntaxaTestSettings.Default.TestUserId + 1;
            updateInformation.ModifiedDate = DateTime.Now;

            IUser appUser = new StubIUser
            {
                UserNameGet = () => "Appuser_" + DyntaxaTestSettings.Default.TestUserName,
                ApplicationIdGet = () => DyntaxaTestSettings.Default.TestDyntaxaApplicationId,
                IsAccountActivatedGet = () => true,
                EmailAddressGet = () => "Appuser_" + DyntaxaTestSettings.Default.TestUserEmail,
                DataContextGet = () => dataContext,
                GUIDGet = () => "Appuser_" + DyntaxaTestSettings.Default.TestUserGuid,
                IdGet = () => DyntaxaTestSettings.Default.TestUserId + 1,
                ShowEmailAddressGet = () => true,
                TypeGet = () => UserType.Person,
                UpdateInformationGet = () => updateInformation,
                ValidFromDateGet = () => DateTime.Now,
                ValidToDateGet = () => new DateTime(2144, 12, 31) };

            return appUser;
        }

        /// <summary>
        /// Get pulic application role with species fact, obsrevation and taxon reader authority used in SpeciesIdentification.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="roleId">
        /// The role id.
        /// </param>
        /// <returns>
        /// Public application Role role <see cref="IRole"/>.
        /// </returns>
        public IRole GetStubPublicApplicationRole(string userName, int roleId)
        {
            AuthorityList autorities = new AuthorityList();
            IAuthority authority = new StubIAuthority
            {
                IdentifierGet = () => DyntaxaSettings.Default.SpeciesFactAuthorityIdentifier,
                ReadPermissionGet = () => true
            };

            IAuthority obsAuthority = new StubIAuthority
            {
                IdentifierGet = () => "Sighting",
                ReadPermissionGet = () => true
            };

            IAuthority taxonAuthority = new StubIAuthority
            {
                IdentifierGet = () => "Taxon",
                ReadPermissionGet = () => true
            };

            autorities.Add(authority);
            autorities.Add(obsAuthority);
            autorities.Add(taxonAuthority);

            IRole newRole = new StubIRole
            {
                NameGet = () => userName,
                ShortNameGet = () => userName,
                DescriptionGet = () => @"testSpeciesDescription",
                IdGet = () => roleId,
                UserAdministrationRoleIdGet = () => 1,
                IdentifierGet = () => DyntaxaSettings.Default.DyntaxaApplicationIdentifier,
                AuthoritiesGet = () => autorities
            };

            return newRole;
        }

        /// <summary>
        /// Get test user role with species fact reader authority used in SpeciesIdentification.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="roleId">
        /// The role id.
        /// </param>
        /// <returns>
        /// Public application Role role <see cref="IRole"/>.
        /// </returns>
        public IRole GetStubTestUserRole(string userName, int roleId)
        {
            AuthorityList autorities = new AuthorityList();
            IAuthority authority = new StubIAuthority
            {
                IdentifierGet = () => DyntaxaSettings.Default.SpeciesFactAuthorityIdentifier,
                ReadPermissionGet = () => true
            };

            autorities.Add(authority);

            IRole newRole = new StubIRole
            {
                NameGet = () => userName,
                ShortNameGet = () => userName,
                DescriptionGet = () => @"testSpeciesDescription",
                IdGet = () => roleId + 2,
                AuthoritiesGet = () => autorities
            };

            return newRole;
        }

        #endregion

        /// <summary>
        /// The locke spiders speciesFact test class.
        /// </summary>
        internal class LockespindlarSpeciesFactTest
        {
            /// <summary>
            /// The user context.
            /// </summary>
            private readonly IUserContext userContext;

            /// <summary>
            /// The species fact list.
            /// </summary>
            private readonly SpeciesFactList speciesFactList = new SpeciesFactList(true);

            /// <summary>
            /// The sepcies fact quality list.
            /// </summary>
            private readonly SpeciesFactQualityList speciesFactQualityList = new SpeciesFactQualityList();

            /// <summary>
            /// Gets the species fact list.
            /// </summary>
            internal SpeciesFactList SpeciesFactList
            {
                get
                {
                    return speciesFactList;
                }
            }

            /// <summary>
            /// Gets the species fact quality list.
            /// </summary>
            internal SpeciesFactQualityList SpeciesFactQualityList
            {
                get
                {
                    return speciesFactQualityList;
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="LockespindlarSpeciesFactTest"/> class.
            /// </summary>
            public LockespindlarSpeciesFactTest(IUserContext userContext)
            {
                this.userContext = userContext;
            }

            
            /// <summary>
            /// Creates test data for species facts.
            /// </summary>
            /// <param name="taxa">List of taxa.</param>
            public void AddSpeciesFactTestData(LockespindlarTaxaTest taxa)
            {
                this.speciesFactList.Add(
                    new SpeciesFact(
                        userContext,
                        1000000,
                        taxa.TaxonList[0],
                    // TaxonId 226664
                        0,
                        2540,
                        null,
                        false,
                        -1,
                        false,
                        -99,
                        false,
                        -99,
                        false,
                        -99,
                        false,
                        string.Empty,
                        false,
                        string.Empty,
                        false,
                        -1,
                        -1,
                        string.Empty,
                        DateTime.MinValue));
                this.speciesFactList.Add(
                    new SpeciesFact(
                        userContext,
                        1000001,
                        taxa.TaxonList[1],
                    // TaxonId 226665
                        0,
                        2540,
                        null,
                        false,
                        -1,
                        false,
                        -99,
                        false,
                        -99,
                        false,
                        -99,
                        false,
                        string.Empty,
                        false,
                        string.Empty,
                        false,
                        -1,
                        -1,
                        string.Empty,
                        DateTime.MinValue));
                this.speciesFactList.Add(
                    new SpeciesFact(
                        userContext,
                        1000002,
                        taxa.TaxonList[2],
                    // TaxonId 101932
                        0,
                        2540,
                        null,
                        false,
                        -1,
                        false,
                        -99,
                        false,
                        -99,
                        false,
                        -99,
                        false,
                        string.Empty,
                        false,
                        string.Empty,
                        false,
                        -1,
                        -1,
                        string.Empty,
                        DateTime.MinValue));
                this.speciesFactList.Add(
                    new SpeciesFact(
                        userContext,
                        1000003,
                        taxa.TaxonList[3],
                    // TaxonId 226666
                        0,
                        2540,
                        null,
                        false,
                        -1,
                        false,
                        -99,
                        false,
                        -99,
                        false,
                        -99,
                        false,
                        string.Empty,
                        false,
                        string.Empty,
                        false,
                        -1,
                        -1,
                        string.Empty,
                        DateTime.MinValue));
                this.speciesFactList.Add(
                    new SpeciesFact(
                        userContext,
                        1000004,
                        taxa.TaxonList[4],
                    // TaxonId 226667
                        0,
                        2540,
                        null,
                        false,
                        -1,
                        false,
                        -99,
                        false,
                        -99,
                        false,
                        -99,
                        false,
                        string.Empty,
                        false,
                        string.Empty,
                        false,
                        -1,
                        -1,
                        string.Empty,
                        DateTime.MinValue));
                this.speciesFactList.Add(
                    new SpeciesFact(
                        userContext,
                        1000005,
                        taxa.TaxonList[5],
                    // TaxonId 226668
                        0,
                        2540,
                        null,
                        false,
                        -1,
                        false,
                        -99,
                        false,
                        -99,
                        false,
                        -99,
                        false,
                        string.Empty,
                        false,
                        string.Empty,
                        false,
                        -1,
                        -1,
                        string.Empty,
                        DateTime.MinValue));
              
                this.speciesFactQualityList.Add(new SpeciesFactQuality { Id = 1, Name = "Hög" });
            }
        }
        /// <summary>
        /// The locke spiders taxa test class.
        /// </summary>
        internal class LockespindlarTaxaTest
        {
            /// <summary>
            /// The taxon list.
            /// </summary>
            private readonly TaxonList taxonlist = new TaxonList();

            /// <summary>
            /// Gets the taxon list.
            /// </summary>
            internal TaxonList TaxonList
            {
                get
                {
                    return this.taxonlist;
                }
            }

            // Nemastoma lugubre fläcklocke 226664
            // Mitostoma chrysomelas guldlocke 226665
            // Trogulus tricarinatus(Linnaeus, 1767)sköldlocke 101932
            // Oligolophus tridens skogslocke 226666
            // Oligolophus hanseni haglocke 226667
            // Paroligolophus agrestis vinterlocke 226668
            // Lacinius ephippiatus lundlocke 226669
            // Lacinius horridus igelkottslocke 226670
            // Mitopus morio hedlocke 226671
            // Phalangium opilio hornlocke 226672
            // Opilio parietinus stadslocke 226673
            // Opilio saxatilis strandlocke 226674
            // Opilio canestrini Italienlocke 226675
            // Rilaena triangularis tidiglocke 226676
            // Lophopilio palpinalis ljunglocke 226677
            // Leiobunum rotundum (Latreille, 1798) vanlig långbenslocke 226678
            // Leiobunum blackwalli  Meade, 1861 kärrlångbenslocke 226679
            // Leiobunum rupestre  (Herbst, 1799) skogslångbenslocke 226680
            // Nelima gothica kustlångbensslocke 226682
            // Dicranopalpus ramosus (Simon, 1909) grenlocke 6002571

            /// <summary>
            /// Initializes a new instance of the <see cref="LockespindlarTaxaTest"/> class.
            /// </summary>
            public LockespindlarTaxaTest()
            {
                this.taxonlist.Add(new Taxon
                {
                    CommonName = "fläcklocke",
                    ScientificName = "Nemastoma lugubre",
                    Id = 226664
                });
                this.taxonlist.Add(new Taxon
                {
                    CommonName = "guldlocke",
                    ScientificName = "Mitostoma chrysomelas",
                    Id = 226665
                });
                this.taxonlist.Add(new Taxon
                {
                    CommonName = "sköldlocke",
                    ScientificName = "Trogulus tricarinatus",
                    Id = 101932
                });
                this.taxonlist.Add(new Taxon
                {
                    CommonName = "skogslocke",
                    ScientificName = "Oligolophus tridens",
                    Id = 226666
                });
                this.taxonlist.Add(new Taxon
                {
                    CommonName = "haglocke",
                    ScientificName = "Oligolophus hanseni",
                    Id = 226667
                });
                this.taxonlist.Add(new Taxon
                {
                    CommonName = "vinterlocke",
                    ScientificName = "Paroligolophus agrestis",
                    Id = 226668
                });
            }
        }
    }

}
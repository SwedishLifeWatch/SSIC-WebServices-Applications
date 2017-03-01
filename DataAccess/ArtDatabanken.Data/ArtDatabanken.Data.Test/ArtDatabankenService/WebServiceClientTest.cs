using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class WebServiceClientTest : TestBase
    {
        [TestMethod]
        public void ClearCache()
        {
            TestInitialize("WebAdministration");

            WebServiceClient.ClearCache();
        }

        [TestMethod]
        public void CommitTransaction()
        {
            WebServiceClient.StartTransaction(1);
            WebServiceClient.CommitTransaction();
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void CommitTransactionNoTransactionError()
        {
            WebServiceClient.CommitTransaction();
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void CommitTransactionTransactionTimeoutError()
        {
            WebServiceClient.StartTransaction(1);
            Thread.Sleep(2000);
            WebServiceClient.CommitTransaction();
        }

        [TestMethod]
        public void GetBirdNestActivities()
        {
            List<WebBirdNestActivity> birdNestActivities;

            birdNestActivities = WebServiceClient.GetBirdNestActivities();
            Assert.IsTrue(birdNestActivities.IsNotEmpty());
        }

        [TestMethod]
        public void GetCounties()
        {
            List<WebCounty> counties;

            counties = WebServiceClient.GetCounties();
            Assert.IsTrue(counties.IsNotEmpty());
        }

        [TestMethod]
        public void GetDatabases()
        {
            List<WebDatabase> databases;

            databases = WebServiceClient.GetDatabases();
            Assert.IsTrue(databases.IsNotEmpty());
        }

        [TestMethod]
        public void GetIndividualCategory()
        {
            List<WebIndividualCategory> individualcategory;

            individualcategory = WebServiceClient.GetIndividualCategories();
            Assert.IsNotNull(individualcategory);
            Assert.IsTrue(individualcategory.IsNotEmpty());
        }

        [TestMethod]
        public void GetPeriods()
        {
            List<WebPeriod> periods;

            periods = WebServiceClient.GetPeriods();
            Assert.IsNotNull(periods);
            Assert.IsTrue(periods.IsNotEmpty());
        }

        [TestMethod]
        public void GetProvinces()
        {
            List<WebProvince> provinces;

            provinces = WebServiceClient.GetProvinces();
            Assert.IsTrue(provinces.IsNotEmpty());
        }

        [TestMethod]
        public void GetSpeciesObservationChange()
        {
            DateTime changedFrom, changedTo;
            WebSpeciesObservationChange change;

            TestInitialize("PrintObs");

            // Get some changes.
            changedFrom = new DateTime(2011, 2, 1, 0, 0, 0);
            changedTo = new DateTime(2011, 2, 1, 0, 0, 0);
            change = WebServiceClient.GetSpeciesObservationChange(changedFrom, changedTo);
            Assert.IsNotNull(change);
            Assert.IsTrue(change.NewSpeciesObservations.IsNotEmpty());
            Assert.IsTrue(change.NewSpeciesObservationIds.IsEmpty());
            Assert.IsTrue(change.UpdatedSpeciesObservations.IsNotEmpty());
            Assert.IsTrue(change.DeletedSpeciesObservationGuids.IsNotEmpty());

            // Get many changes.
            changedFrom = new DateTime(2011, 2, 1, 0, 0, 0);
            changedTo = new DateTime(2011, 2, 2, 0, 0, 0);
            change = WebServiceClient.GetSpeciesObservationChange(changedFrom, changedTo);
            Assert.IsNotNull(change);
            Assert.IsTrue(change.NewSpeciesObservations.IsEmpty());
            Assert.IsTrue(change.NewSpeciesObservationIds.IsNotEmpty());
            Assert.IsTrue(change.UpdatedSpeciesObservations.IsNotEmpty());
            Assert.IsTrue(change.DeletedSpeciesObservationGuids.IsNotEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetSpeciesObservationChangeDateSwitchError()
        {
            DateTime changedFrom, changedTo;
            WebSpeciesObservationChange change;

            TestInitialize("PrintObs");

            changedFrom = new DateTime(2011, 2, 2, 0, 0, 0);
            changedTo = new DateTime(2011, 2, 1, 0, 0, 0);
            change = WebServiceClient.GetSpeciesObservationChange(changedFrom, changedTo);
            Assert.IsNotNull(change);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetSpeciesObservationChangeFutureChangedToError()
        {
            DateTime changedFrom, changedTo;
            WebSpeciesObservationChange change;

            TestInitialize("PrintObs");

            changedFrom = new DateTime(2011, 2, 2, 0, 0, 0);
            changedTo = DateTime.Now;
            change = WebServiceClient.GetSpeciesObservationChange(changedFrom, changedTo);
            Assert.IsNotNull(change);
        }

        [TestMethod]
        public void GetSpeciesObservationCount()
        {
            Int32 speciesObservationCount;

            TestInitialize("PrintObs");

            speciesObservationCount = WebServiceClient.GetSpeciesObservationCount(SpeciesObservationManagerTest.GetOneWebSearchCriteria());
            Assert.IsTrue(speciesObservationCount > 0);
        }

        [TestMethod]
        public void GetSpeciesObservations()
        {
            WebSpeciesObservationInformation speciesObservationInformation;

            TestInitialize("PrintObs");

            speciesObservationInformation = WebServiceClient.GetSpeciesObservations(SpeciesObservationManagerTest.GetOneWebSearchCriteria());
            Assert.IsNotNull(speciesObservationInformation);
            Assert.IsTrue(speciesObservationInformation.SpeciesObservations.IsNotEmpty());
        }

        [TestMethod]
        public void GetStatus()
        {
            List<WebResourceStatus> status;

            status = WebServiceClient.GetStatus();
            Assert.IsTrue(status.IsNotEmpty());
            status = WebServiceClient.GetStatus();
            Assert.IsTrue(status.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxaById()
        {
            Boolean taxonFound;
            List<WebTaxon> taxa;

            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                taxa = WebServiceClient.GetTaxaById(TaxonManagerTest.GetTaxaIds(), taxonInformationType);
                Assert.IsNotNull(taxa);
                Assert.AreEqual(taxa.Count, TaxonManagerTest.GetTaxaIds().Count);

                foreach (WebTaxon taxon in taxa)
                {
                    Assert.AreEqual(taxon.TaxonInformationType, taxonInformationType);

                    taxonFound = false;
                    foreach (Int32 taxonId in TaxonManagerTest.GetTaxaIds())
                    {
                        if (taxonId == taxon.Id)
                        {
                            taxonFound = true;
                            break;
                        }
                    }
                    Assert.IsTrue(taxonFound);
                }
            }
        }

        [TestMethod]
        public void GetTaxaByOrganismOrRedlist()
        {
            List<WebTaxon> taxa;
            Int32 organismGroupId, endangeredListId, redlistCategoryId;

            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                organismGroupId = 5;
                endangeredListId = 1;
                redlistCategoryId = 0;
                taxa = WebServiceClient.GetTaxaByOrganismOrRedlist(true,
                                                                   organismGroupId,
                                                                   true,
                                                                   endangeredListId,
                                                                   true,
                                                                   redlistCategoryId,
                                                                   taxonInformationType);
                Assert.IsNotNull(taxa);
                Assert.IsTrue(taxa.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetTaxaByQuery()
        {
            Data.ArtDatabankenService.Factor factor;
            WebDataCondition dataCondition;
            WebDataQuery dataQuery;
            WebFactor webFactor;
            WebSpeciesFactCondition speciesFactCondition;
            List<WebTaxon> taxa;

            speciesFactCondition = new WebSpeciesFactCondition();
            speciesFactCondition.Factors = new List<WebFactor>();
            factor = FactorManagerTest.GetOneFactor();
            webFactor = Data.ArtDatabankenService.FactorManager.GetFactor(factor);
            speciesFactCondition.Factors.Add(webFactor);
            dataCondition = new WebDataCondition();
            dataCondition.SpeciesFactCondition = speciesFactCondition;
            dataQuery = new WebDataQuery();
            dataQuery.DataCondition = dataCondition;
            taxa = WebServiceClient.GetTaxaByQuery(dataQuery, TaxonInformationType.Basic);
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxaBySearchCriteria()
        {
            List<WebTaxon> taxa;
            WebTaxonSearchCriteria searchCriteria;

            searchCriteria = new WebTaxonSearchCriteria();
            searchCriteria.TaxonNameSearchString = "björn%";
            taxa = WebServiceClient.GetTaxaBySearchCriteria(searchCriteria);
            Assert.IsNotNull(taxa);
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxaBySpeciesObservations()
        {
            List<WebTaxon> taxa;

            TestInitialize("PrintObs");

            taxa = WebServiceClient.GetTaxaBySpeciesObservations(SpeciesObservationManagerTest.GetOneWebSearchCriteria());
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxaCountBySpeciesObservations()
        {
            Int32 taxaCount;

            TestInitialize("PrintObs");

            taxaCount = WebServiceClient.GetTaxaCountBySpeciesObservations(SpeciesObservationManagerTest.GetOneWebSearchCriteria());
            Assert.IsTrue(0 < taxaCount);
        }

        [TestMethod]
        public void GetHostTaxa()
        {
            
            Int32 factorId = 1142;
            List<WebTaxon> taxa;

            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                taxa = WebServiceClient.GetHostTaxa(factorId, taxonInformationType);
                Assert.IsNotNull(taxa);
                Assert.IsTrue(taxa.Count > 10);
            }
        }

        [TestMethod]
        public void GetTaxon()
        {
            WebTaxon taxon;

            foreach (Int32 taxonId in TaxonManagerTest.GetTaxaIds())
            {
                foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
                {
                    taxon = WebServiceClient.GetTaxon(taxonId, taxonInformationType);
                    Assert.IsNotNull(taxon);
                    Assert.AreEqual(taxon.Id, taxonId);
                    Assert.AreEqual(taxon.TaxonInformationType, taxonInformationType);
                }
            }
        }

        [TestMethod]
        public void GetTaxonCountyOccurence()
        {
            Int32 taxonId;
            List<WebTaxonCountyOccurrence> countyOccurrencies;

            taxonId = BEAR_TAXON_ID;
            countyOccurrencies = WebServiceClient.GetTaxonCountyOccurence(taxonId);
            Assert.IsTrue(countyOccurrencies.IsNotEmpty());
            foreach (WebTaxonCountyOccurrence countyOccurrence in countyOccurrencies)
            {
                Assert.IsNotNull(countyOccurrence);
                Assert.AreEqual(taxonId, countyOccurrence.TaxonId);
            }
        }

        [TestMethod]
        public void GetTaxonNames()
        {
            List<WebTaxonName> taxonNames;
            Int32 taxonId;

            taxonId = TaxonManagerTest.GetSomeTaxonIds(1)[0];
            taxonNames = WebServiceClient.GetTaxonNames(taxonId);
            Assert.IsTrue(taxonNames.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonNamesBySearchCriteria()
        {
            List<WebTaxonName> taxonNames;
            WebTaxonNameSearchCriteria searchCriteria;

            searchCriteria = new WebTaxonNameSearchCriteria();
            searchCriteria.NameSearchString = "björn";
            taxonNames = WebServiceClient.GetTaxonNamesBySearchCriteria(searchCriteria);
            Assert.IsNotNull(taxonNames);
            Assert.IsTrue(taxonNames.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonNameTypes()
        {
            List<WebTaxonNameType> taxonNameTypes;

            taxonNameTypes = WebServiceClient.GetTaxonNameTypes();
            Assert.IsNotNull(taxonNameTypes);
            Assert.IsTrue(taxonNameTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonNameUseTypes()
        {
            List<WebTaxonNameUseType> taxonNameUseTypes;

            taxonNameUseTypes = WebServiceClient.GetTaxonNameUseTypes();
            Assert.IsNotNull(taxonNameUseTypes);
            Assert.IsTrue(taxonNameUseTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonTreesBySearchCriteria()
        {
            List<WebTaxonTreeNode> trees;
            WebTaxonTreeSearchCriteria searchCriteria;

            searchCriteria = new WebTaxonTreeSearchCriteria();
            searchCriteria.RestrictSearchToTaxonIds = new List<Int32>();
            searchCriteria.RestrictSearchToTaxonIds.Add(HAWK_BIRDS_TAXON_ID);
            trees = WebServiceClient.GetTaxonTreesBySearchCriteria(searchCriteria);
            Assert.IsNotNull(trees);
            Assert.IsTrue(trees.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonTypes()
        {
            List<WebTaxonType> taxonTypes;

            taxonTypes = WebServiceClient.GetTaxonTypes();
            Assert.IsNotNull(taxonTypes);
            Assert.IsTrue(taxonTypes.IsNotEmpty());
        }

        [TestMethod]
        public void Login()
        {
            // Loggin existing user.
            Assert.IsTrue(WebServiceClient.Login(TEST_USER_NAME, TEST_PASSWORD, "EVA", false));

            // Login none existing user.
            Assert.IsFalse(WebServiceClient.Login("No user name", "No password", "EVA", false));
        }

        [TestMethod]
        public void Logout()
        {
            // Logout existing user.
            WebServiceClient.Logout();

            // Should be ok to logout an already logged out user.
            WebServiceClient.Logout();
        }

        [TestMethod]
        public void Ping()
        {
            Boolean ping;

            ping = WebServiceClient.Ping();
            Assert.IsTrue(ping);
        }

        [TestMethod]
        public void RollbackTransaction()
        {
            // Should be ok to rollback an unexisting transaction.
            WebServiceClient.RollbackTransaction();

            // Normal rollback.
            WebServiceClient.StartTransaction(1);
            WebServiceClient.RollbackTransaction();
            Thread.Sleep(2000);

            // Should be ok to rollback twice.
            WebServiceClient.StartTransaction(1);
            WebServiceClient.RollbackTransaction();
            WebServiceClient.RollbackTransaction();
        }

        [TestMethod]
        public void StartTransaction()
        {
            WebServiceClient.StartTransaction(1);
            WebServiceClient.RollbackTransaction();
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void StartTransactionAlreadyStartedError()
        {
            WebServiceClient.StartTransaction(1);
            WebServiceClient.StartTransaction(1);
        }
        
        [TestMethod]
        public void GetFactorUpdateModes()
        {
            List<WebFactorUpdateMode> factorUpdateModes;

            factorUpdateModes = WebServiceClient.GetFactorUpdateModes();
            Assert.IsNotNull(factorUpdateModes);
            Assert.IsTrue(factorUpdateModes.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorFieldEnums()
        {
            List<WebFactorFieldEnum> factorFieldEnums;

            factorFieldEnums = WebServiceClient.GetFactorFieldEnums();
            Assert.IsNotNull(factorFieldEnums);
            Assert.IsTrue(factorFieldEnums.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorFieldTypes()
        {
            List<WebFactorFieldType> factorFieldTypes;

            factorFieldTypes = WebServiceClient.GetFactorFieldTypes();
            Assert.IsNotNull(factorFieldTypes);
            Assert.IsTrue(factorFieldTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorDataTypes()
        {
            List<WebFactorDataType> factorDataTypes;

            factorDataTypes = WebServiceClient.GetFactorDataTypes();
            Assert.IsNotNull(factorDataTypes);
            Assert.IsTrue(factorDataTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetReferences()
        {
            List<WebReference> references;

            references = WebServiceClient.GetReferences();
            Assert.IsNotNull(references);
            Assert.IsTrue(references.IsNotEmpty());
        }

        [TestMethod]
        public void GetReferencesBySearchString()
        {
            List<WebReference> references;
            String searchString = "1995";
            references = WebServiceClient.GetReferencesBySearchString(searchString);
            Assert.IsNotNull(references);
            Assert.IsTrue(references.Count > 3);
        }

        [TestMethod]
        public void GetFactorOrigins()
        {
            List<WebFactorOrigin> factorOrigins;

            factorOrigins = WebServiceClient.GetFactorOrigins();
            Assert.IsNotNull(factorOrigins);
            Assert.IsTrue(factorOrigins.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactors()
        {
            List<WebFactor> factors;

            factors = WebServiceClient.GetFactors();
            Assert.IsTrue(factors.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorsBySearchCriteria()
        {
            List<WebFactor> factors;
            WebFactorSearchCriteria searchCriteria;

            searchCriteria = new WebFactorSearchCriteria();
            searchCriteria.NameSearchString = "Rödli%";

            searchCriteria.NameSearchMethod = WebService.SearchStringComparisonMethod.Like;
#if DATA_SPECIFIED_EXISTS
            searchCriteria.NameSearchMethodSpecified = true;
#endif
            factors = WebServiceClient.GetFactorsBySearchCriteria(searchCriteria);
            Assert.IsNotNull(factors);
            Assert.IsTrue(factors.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorTrees()
        {
            List<WebFactorTreeNode> trees;

            trees = WebServiceClient.GetFactorTrees();
            Assert.IsTrue(trees.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorTreesBySearchCriteria()
        {
            List<WebFactorTreeNode> trees;
            WebFactorTreeSearchCriteria searchCriteria;

            searchCriteria = new WebFactorTreeSearchCriteria();
            searchCriteria.RestrictSearchToFactorIds = new List<Int32>();
            searchCriteria.RestrictSearchToFactorIds.Add(LANDSCAPE_FACTOR_ID);

            // Get current top tree nodes.
            //searchCriteria.RestrictSearchToFactorIds.Add(985);
            //searchCriteria.RestrictSearchToFactorIds.Add(2525);
            //searchCriteria.RestrictSearchToFactorIds.Add(1973);
            //searchCriteria.RestrictSearchToFactorIds.Add(990);
            //searchCriteria.RestrictSearchToFactorIds.Add(2536);
            //searchCriteria.RestrictSearchToFactorIds.Add(2479);
            //searchCriteria.RestrictSearchToFactorIds.Add(2480);
            //searchCriteria.RestrictSearchToFactorIds.Add(1626);
            //searchCriteria.RestrictSearchToFactorIds.Add(1783);
            //searchCriteria.RestrictSearchToFactorIds.Add(1721);
            //searchCriteria.RestrictSearchToFactorIds.Add(1701);
            //searchCriteria.RestrictSearchToFactorIds.Add(1671);
            //searchCriteria.RestrictSearchToFactorIds.Add(2175);
            //searchCriteria.RestrictSearchToFactorIds.Add(1700);
            //searchCriteria.RestrictSearchToFactorIds.Add(542);
            //searchCriteria.RestrictSearchToFactorIds.Add(775);
            //searchCriteria.RestrictSearchToFactorIds.Add(681);
            //searchCriteria.RestrictSearchToFactorIds.Add(538);
            //searchCriteria.RestrictSearchToFactorIds.Add(531);
            //searchCriteria.RestrictSearchToFactorIds.Add(679);
            //searchCriteria.RestrictSearchToFactorIds.Add(680);
            //searchCriteria.RestrictSearchToFactorIds.Add(545);
            //searchCriteria.RestrictSearchToFactorIds.Add(910);
            //searchCriteria.RestrictSearchToFactorIds.Add(1890);
            //searchCriteria.RestrictSearchToFactorIds.Add(1954);
            //searchCriteria.RestrictSearchToFactorIds.Add(1997);
            //searchCriteria.RestrictSearchToFactorIds.Add(2182);
            //searchCriteria.RestrictSearchToFactorIds.Add(2263);
            //searchCriteria.RestrictSearchToFactorIds.Add(2484);
            //searchCriteria.RestrictSearchToFactorIds.Add(2538);
            trees = WebServiceClient.GetFactorTreesBySearchCriteria(searchCriteria);
            Assert.IsNotNull(trees);
            Assert.IsTrue(trees.IsNotEmpty());
        }

        [TestMethod]
        public void GetSpeciesFactQualities()
        {
            List<WebSpeciesFactQuality> speciesFactQualties;

            speciesFactQualties = WebServiceClient.GetSpeciesFactQualities();
            Assert.IsNotNull(speciesFactQualties);
            Assert.IsTrue(speciesFactQualties.IsNotEmpty());
        }

        [TestMethod]
        public void GetSpeciesFactsById()
        {
            Boolean speciesFactFound;
            List<WebSpeciesFact> speciesFacts;
            List<Int32> speciesFactIds;
            speciesFactIds = new List<Int32>();
            speciesFactIds.Add(1);
            speciesFactIds.Add(2);

            speciesFacts = WebServiceClient.GetSpeciesFactsById(speciesFactIds);
            Assert.IsNotNull(speciesFacts);
            Assert.AreEqual(speciesFacts.Count, speciesFactIds.Count);

            foreach (WebSpeciesFact speciesFact in speciesFacts)
            {
                Assert.IsNotNull(speciesFact.TaxonId);

                speciesFactFound = false;
                foreach (Int32 speciesFactId in speciesFactIds)
                {
                    if (speciesFactId == speciesFact.Id)
                    {
                        speciesFactFound = true;
                        break;
                    }
                }
                Assert.IsTrue(speciesFactFound);
            }
        }

        [TestMethod]
        public void GetSpeciesFactsByUserParameterSelection()
        {
            List<WebSpeciesFact> speciesFacts;
            WebUserParameterSelection userParameterSelection;
            
            List<Int32> taxonIds;
            taxonIds = new List<Int32>();
            taxonIds.Add(1);

            userParameterSelection = new WebUserParameterSelection();
            userParameterSelection.TaxonIds = taxonIds;
            
            speciesFacts = WebServiceClient.GetSpeciesFactsByUserParameterSelection(userParameterSelection);
            Assert.IsNotNull(speciesFacts);
            Assert.IsTrue(speciesFacts.IsNotEmpty());
        }

        [TestMethod]
        public void UpdateReference()
        {
            using (WebTransaction transaction = new WebTransaction(1))
            {
                WebReference reference = new WebReference();
                reference.Id = 1;
                reference.Text = "Changed Text";
                reference.Year = 1919;
#if DATA_SPECIFIED_EXISTS
                reference.YearSpecified = true;
#endif
                reference.Name = "Changed Name";
                
                WebServiceClient.UpdateReference(reference);
            }
        }

        [TestMethod]
        public void UpdateSpeciesFacts()
        {
            using (WebTransaction transaction = new WebTransaction(1))
            {
                WebServiceClient.UpdateSpeciesFacts(null, null, null);
            }
        }

        [TestMethod]
        public void UpdateDyntaxaSpeciesFacts()
        {
            Data.ArtDatabankenService.UserManager.Login(TEST_USER_NAME, TEST_PASSWORD, "Dyntaxa", false);
            using (WebTransaction transaction = new WebTransaction(1))
            {
                WebServiceClient.UpdateDyntaxaSpeciesFacts(null, null, null, "");
            }
        }

        [TestMethod]
        public void CreateReference()
        {
            using (WebTransaction transaction = new WebTransaction(1))
            {
                WebReference reference = new WebReference();
                reference.Id = 0;
                reference.Text = "New text";
                reference.Year = DateTime.Today.Year;
#if DATA_SPECIFIED_EXISTS
                reference.YearSpecified = true;
#endif
                reference.Name = "New reference name";

                WebServiceClient.CreateReference(reference);
                RollbackTransaction();
            }
        }
    }
}

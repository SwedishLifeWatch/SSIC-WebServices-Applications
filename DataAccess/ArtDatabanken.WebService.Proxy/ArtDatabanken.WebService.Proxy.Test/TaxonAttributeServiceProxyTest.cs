using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Proxy.Test
{
    [TestClass]
    public class TaxonAttributeServiceProxyTest
    {
        private WebClientInformation _clientInformation;

        private WebClientInformation GetClientInformation()
        {
            return _clientInformation;
        }

        [TestMethod]
        public void CreateSpeciesFacts()
        {
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.TaxonAttributeService))
            {
                List<WebSpeciesFact> inSpeciesFacts;

                // Create species facts.
                inSpeciesFacts = new List<WebSpeciesFact>
                                     {
                                         new WebSpeciesFact
                                             {
                                                 FactorId = 34,
                                                 HostId = 23,
                                                 IndividualCategoryId = 3,
                                                 PeriodId = 2,
                                                 TaxonId = 345
                                             },
                                         new WebSpeciesFact
                                             {
                                                 FactorId = 57,
                                                 HostId = 34,
                                                 IndividualCategoryId = 1,
                                                 PeriodId = 3,
                                                 TaxonId = 678
                                             }
                                     };
                WebServiceProxy.TaxonAttributeService.CreateSpeciesFacts(GetClientInformation(), inSpeciesFacts);
            }
        }

        [TestMethod]
        public void DeleteSpeciesFacts()
        {
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.TaxonAttributeService))
            {
                List<WebSpeciesFact> inSpeciesFacts, outSpeciesFacts;

                // Create species facts.
                inSpeciesFacts = new List<WebSpeciesFact>
                                     {
                                         new WebSpeciesFact
                                             {
                                                 FactorId = 34,
                                                 HostId = 23,
                                                 IndividualCategoryId = 3,
                                                 PeriodId = 2,
                                                 TaxonId = 345
                                             },
                                         new WebSpeciesFact
                                             {
                                                 FactorId = 57,
                                                 HostId = 34,
                                                 IndividualCategoryId = 1,
                                                 PeriodId = 3,
                                                 TaxonId = 678
                                             }
                                     };
                WebServiceProxy.TaxonAttributeService.CreateSpeciesFacts(GetClientInformation(), inSpeciesFacts);

                // Update species facts.
                List<int> speciesFactIds = new List<int> { 1, 2 };

                inSpeciesFacts = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsByIds(GetClientInformation(), speciesFactIds);
                WebServiceProxy.TaxonAttributeService.UpdateSpeciesFacts(GetClientInformation(), inSpeciesFacts);
                outSpeciesFacts = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsByIds(GetClientInformation(), speciesFactIds);
                foreach (WebSpeciesFact speciesFact in outSpeciesFacts)
                {
                    Assert.AreEqual(speciesFact.ModifiedBy, "TestFirstName TestLastName");
                }

                // Delete species facts.
                WebServiceProxy.TaxonAttributeService.DeleteSpeciesFacts(GetClientInformation(), outSpeciesFacts);
            }
        }

        [TestMethod]
        public void GetFactorOrigins()
        {
            List<WebFactorOrigin> factorOrigins;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.TaxonAttributeService))
            {
                factorOrigins = WebServiceProxy.TaxonAttributeService.GetFactorOrigins(GetClientInformation());
                Assert.IsTrue(factorOrigins.IsNotEmpty());
            }

            factorOrigins = WebServiceProxy.TaxonAttributeService.GetFactorOrigins(GetClientInformation());
            Assert.IsTrue(factorOrigins.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorUpdateModes()
        {
            List<WebFactorUpdateMode> factorUpdateModes;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.TaxonAttributeService))
            {
                factorUpdateModes = WebServiceProxy.TaxonAttributeService.GetFactorUpdateModes(GetClientInformation());
                Assert.IsTrue(factorUpdateModes.IsNotEmpty());
            }

            factorUpdateModes = WebServiceProxy.TaxonAttributeService.GetFactorUpdateModes(GetClientInformation());
            Assert.IsTrue(factorUpdateModes.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorFieldTypes()
        {
            List<WebFactorFieldType> factorFieldTypes;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.TaxonAttributeService))
            {
                factorFieldTypes = WebServiceProxy.TaxonAttributeService.GetFactorFieldTypes(GetClientInformation());
                Assert.IsTrue(factorFieldTypes.IsNotEmpty());
            }

            factorFieldTypes = WebServiceProxy.TaxonAttributeService.GetFactorFieldTypes(GetClientInformation());
            Assert.IsTrue(factorFieldTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetPeriodTypes()
        {
            List<WebPeriodType> periodTypes;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.TaxonAttributeService))
            {
                periodTypes = WebServiceProxy.TaxonAttributeService.GetPeriodTypes(GetClientInformation());
                Assert.IsTrue(periodTypes.IsNotEmpty());
            }

            periodTypes = WebServiceProxy.TaxonAttributeService.GetPeriodTypes(GetClientInformation());
            Assert.IsTrue(periodTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetPeriods()
        {
            List<WebPeriod> periods;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.TaxonAttributeService))
            {
                periods = WebServiceProxy.TaxonAttributeService.GetPeriods(GetClientInformation());
                Assert.IsTrue(periods.IsNotEmpty());
            }

            periods = WebServiceProxy.TaxonAttributeService.GetPeriods(GetClientInformation());
            Assert.IsTrue(periods.IsNotEmpty());
        }

        [TestMethod]
        public void GetIndividualCategories()
        {
            List<WebIndividualCategory> individualCategories;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.TaxonAttributeService))
            {
                individualCategories = WebServiceProxy.TaxonAttributeService.GetIndividualCategories(GetClientInformation());
                Assert.IsTrue(individualCategories.IsNotEmpty());
            }

            individualCategories = WebServiceProxy.TaxonAttributeService.GetIndividualCategories(GetClientInformation());
            Assert.IsTrue(individualCategories.IsNotEmpty());
        }

        [TestMethod]
        public void GetSpeciesFactQualities()
        {
            List<WebSpeciesFactQuality> speciesFactQualities;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.TaxonAttributeService))
            {
                speciesFactQualities = WebServiceProxy.TaxonAttributeService.GetSpeciesFactQualities(GetClientInformation());
                Assert.IsTrue(speciesFactQualities.IsNotEmpty());
            }

            speciesFactQualities = WebServiceProxy.TaxonAttributeService.GetSpeciesFactQualities(GetClientInformation());
            Assert.IsTrue(speciesFactQualities.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorFieldEnums()
        {
            List<WebFactorFieldEnum> factorFieldEnums;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.TaxonAttributeService))
            {
                factorFieldEnums = WebServiceProxy.TaxonAttributeService.GetFactorFieldEnums(GetClientInformation());
                Assert.IsTrue(factorFieldEnums.IsNotEmpty());
                foreach (WebFactorFieldEnum factorFieldEnum in factorFieldEnums)
                {
                    Assert.IsTrue(factorFieldEnum.Values.IsNotEmpty());
                }
            }

            factorFieldEnums = WebServiceProxy.TaxonAttributeService.GetFactorFieldEnums(GetClientInformation());
            Assert.IsTrue(factorFieldEnums.IsNotEmpty());
            foreach (WebFactorFieldEnum factorFieldEnum in factorFieldEnums)
            {
                Assert.IsTrue(factorFieldEnum.Values.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetFactorDataTypes()
        {
            List<WebFactorDataType> factorDataTypes;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.TaxonAttributeService))
            {
                factorDataTypes = WebServiceProxy.TaxonAttributeService.GetFactorDataTypes(GetClientInformation());
                Assert.IsTrue(factorDataTypes.IsNotEmpty());
                foreach (WebFactorDataType factorDataType in factorDataTypes)
                {
                    Assert.IsTrue(factorDataType.Fields.IsNotEmpty());
                }
            }

            factorDataTypes = WebServiceProxy.TaxonAttributeService.GetFactorDataTypes(GetClientInformation());
            Assert.IsTrue(factorDataTypes.IsNotEmpty());
            foreach (WebFactorDataType factorDataType in factorDataTypes)
            {
                Assert.IsTrue(factorDataType.Fields.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetFactors()
        {
            List<WebFactor> factors;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.TaxonAttributeService))
            {
                factors = WebServiceProxy.TaxonAttributeService.GetFactors(GetClientInformation());
                Assert.IsTrue(factors.IsNotEmpty());
            }

            factors = WebServiceProxy.TaxonAttributeService.GetFactors(GetClientInformation());
            Assert.IsTrue(factors.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorsBySearchCriteria()
        {
            List<WebFactor> factors;
            WebFactorSearchCriteria searchCriteria;

            searchCriteria = new WebFactorSearchCriteria();
            searchCriteria.NameSearchString = new WebStringSearchCriteria();
            searchCriteria.NameSearchString.SearchString = "Rödli%";
            searchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
            searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.Like);

            factors = WebServiceProxy.TaxonAttributeService.GetFactorsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsNotNull(factors);
            Assert.IsTrue(factors.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorTrees()
        {
            List<WebFactorTreeNode> factorTrees;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.TaxonAttributeService))
            {
                factorTrees = WebServiceProxy.TaxonAttributeService.GetFactorTrees(GetClientInformation());
                Assert.IsTrue(factorTrees.IsNotEmpty());
            }

            factorTrees = WebServiceProxy.TaxonAttributeService.GetFactorTrees(GetClientInformation());
            Assert.IsTrue(factorTrees.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorTreesBySearchCriteriaExpectsFactorTree()
        {
            SetUserAndApplicationIdentifier("testUserPublic", "8RevNymI&A3rW", ApplicationIdentifier.AnalysisPortal.ToString());

            List<WebFactorTreeNode> factorTrees;
            WebFactorTreeSearchCriteria searchCriteria = new WebFactorTreeSearchCriteria { FactorIds = new List<int> { 661 } };

            factorTrees = WebServiceProxy.TaxonAttributeService.GetFactorTreesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsNotNull(factorTrees);
            Assert.IsTrue(factorTrees.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorTreesBySearchCriteriaExpectsNoFactorTree()
        {
            SetUserAndApplicationIdentifier("testUserPublic", "8RevNymI&A3rW", ApplicationIdentifier.AnalysisPortal.ToString());

            List<WebFactorTreeNode> factorTrees;
            WebFactorTreeSearchCriteria searchCriteria = new WebFactorTreeSearchCriteria { FactorIds = new List<int> { 1872 } };
            
            factorTrees = WebServiceProxy.TaxonAttributeService.GetFactorTreesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsNotNull(factorTrees);
            Assert.IsTrue(factorTrees.IsEmpty());
        }

        [TestMethod]
        public void GetFactorTreesBySearchCriteriaExpectsPublicFactorTree()
        {
            List<WebFactorTreeNode> factorTrees;
            WebFactorTreeSearchCriteria searchCriteria = new WebFactorTreeSearchCriteria { FactorIds = new List<int> { 661 } };

            factorTrees = WebServiceProxy.TaxonAttributeService.GetFactorTreesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsNotNull(factorTrees);
            Assert.IsTrue(factorTrees.IsNotEmpty());
        }

        [TestMethod]
        public void GetFactorTreesBySearchCriteriaExpectsNonPublicFactorTree()
        {
            List<WebFactorTreeNode> factorTrees;
            WebFactorTreeSearchCriteria searchCriteria = new WebFactorTreeSearchCriteria { FactorIds = new List<int> { 1872 } };

            factorTrees = WebServiceProxy.TaxonAttributeService.GetFactorTreesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsNotNull(factorTrees);
            Assert.IsTrue(factorTrees.IsNotEmpty());
        }
        
        [TestMethod]
        public void GetSpeciesFactsByIds()
        {
            List<WebSpeciesFact> speciesFacts;
            List<int> speciesFactIds = new List<int> { 1, 2 };
            bool speciesFactFound;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.TaxonAttributeService))
            {
                speciesFacts = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsByIds(GetClientInformation(), speciesFactIds);
                Assert.IsTrue(speciesFacts.IsNotEmpty());
                Assert.AreEqual(speciesFacts.Count, speciesFactIds.Count);

                foreach (WebSpeciesFact speciesFact in speciesFacts)
                {
                    Assert.IsNotNull(speciesFact.TaxonId);
                    speciesFactFound = false;
                    foreach (int speciesFactId in speciesFactIds)
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

            speciesFacts = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsByIds(GetClientInformation(), speciesFactIds);
            Assert.IsTrue(speciesFacts.IsNotEmpty());
            Assert.AreEqual(speciesFacts.Count, speciesFactIds.Count);

            foreach (WebSpeciesFact speciesFact in speciesFacts)
            {
                Assert.IsNotNull(speciesFact.TaxonId);
                speciesFactFound = false;
                foreach (int speciesFactId in speciesFactIds)
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
        public void GetSpeciesFactsBySearchCriteria()
        {
            List<WebSpeciesFact> speciesFacts1, speciesFacts2;
            WebSpeciesFactSearchCriteria searchCriteria;

            // Test factor data types.
            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorDataTypeIds = new List<Int32>();
            searchCriteria.FactorDataTypeIds.Add(106); // SA_KvalitativaKaraktärer.
            speciesFacts1 = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add(2547); // 2:e gångbenparet lång relativt kroppslängden, ben/kroppslängd kvot = 2-8
            speciesFacts2 = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(speciesFacts2.IsNotEmpty());
            Assert.IsTrue(speciesFacts1.Count > speciesFacts2.Count);

            // Test factors.
            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add((Int32)(FactorId.RedlistCategory));
            searchCriteria.PeriodIds = new List<Int32>();
            searchCriteria.PeriodIds.Add(2); // 2010
            speciesFacts1 = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.HostIds = new List<Int32>();
            searchCriteria.HostIds.Add(102656); // Hedsidenbi.
            speciesFacts1 = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.IndividualCategoryIds = new List<Int32>();
            searchCriteria.IndividualCategoryIds.Add(9); // Ungar (juveniler)
            speciesFacts1 = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());
            searchCriteria.IndividualCategoryIds.Add(10); // Vuxna (imago).
            speciesFacts1 = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add((Int32)(FactorId.RedlistCategory));
            searchCriteria.PeriodIds = new List<Int32>();
            searchCriteria.PeriodIds.Add(1); // 2000
            speciesFacts1 = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());
            searchCriteria.PeriodIds.Add(2); // 2005
            speciesFacts1 = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            // Test include not valid hosts.
            // Hard to test since there are no host values
            // in database that belongs to not valid taxa.
            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add(5);
            searchCriteria.IncludeNotValidHosts = false;
            speciesFacts1 = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add(5);
            searchCriteria.IncludeNotValidHosts = true;
            speciesFacts2 = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(speciesFacts2.IsNotEmpty());
            Assert.IsTrue(speciesFacts2.Count >= speciesFacts1.Count);

            // Test include not valid taxa.
            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add(5);
            searchCriteria.IncludeNotValidTaxa = false;
            speciesFacts1 = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add(5);
            searchCriteria.IncludeNotValidTaxa = true;
            speciesFacts2 = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(speciesFacts2.IsNotEmpty());
            Assert.IsTrue(speciesFacts2.Count > speciesFacts1.Count);

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.PeriodIds = new List<Int32>();
            searchCriteria.PeriodIds.Add(3); // 2010
            searchCriteria.IndividualCategoryIds = new List<Int32>();
            searchCriteria.IndividualCategoryIds.Add(10); // Vuxna (imago).
            speciesFacts1 = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            speciesFacts1 = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());
        }
 
        [TestMethod]
        public void GetSpeciesFactsByIdentifiers()
        {
            List<WebSpeciesFact> inSpeciesFacts, outSpeciesFacts;
            List<int> speciesFactIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.TaxonAttributeService))
            {
                inSpeciesFacts = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsByIds(GetClientInformation(), speciesFactIds);
                outSpeciesFacts = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsByIdentifiers(GetClientInformation(), inSpeciesFacts);
                Assert.AreEqual(inSpeciesFacts.Count, outSpeciesFacts.Count);
            }

            inSpeciesFacts = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsByIds(GetClientInformation(), speciesFactIds);
            outSpeciesFacts = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsByIdentifiers(GetClientInformation(), inSpeciesFacts);
            Assert.AreEqual(inSpeciesFacts.Count, outSpeciesFacts.Count);
        }

        [TestMethod]
        public void UpdateSpeciesFacts()
        {
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.TaxonAttributeService))
            {
                List<WebSpeciesFact> inSpeciesFacts, outSpeciesFacts;

                // Create species facts.
                inSpeciesFacts = new List<WebSpeciesFact>
                                     {
                                         new WebSpeciesFact
                                             {
                                                 FactorId = 34,
                                                 HostId = 23,
                                                 IndividualCategoryId = 3,
                                                 PeriodId = 2,
                                                 TaxonId = 345
                                             },
                                         new WebSpeciesFact
                                             {
                                                 FactorId = 57,
                                                 HostId = 34,
                                                 IndividualCategoryId = 1,
                                                 PeriodId = 3,
                                                 TaxonId = 678
                                             }
                                     };
                WebServiceProxy.TaxonAttributeService.CreateSpeciesFacts(GetClientInformation(), inSpeciesFacts);

                // Update species facts.
                List<int> speciesFactIds = new List<int> { 1, 2 };

                inSpeciesFacts = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsByIds(GetClientInformation(), speciesFactIds);
                WebServiceProxy.TaxonAttributeService.UpdateSpeciesFacts(GetClientInformation(), inSpeciesFacts);
                outSpeciesFacts = WebServiceProxy.TaxonAttributeService.GetSpeciesFactsByIds(GetClientInformation(), speciesFactIds);
                foreach (WebSpeciesFact speciesFact in outSpeciesFacts)
                {
                    Assert.AreEqual(speciesFact.ModifiedBy, "TestFirstName TestLastName");
                }

                // Delete species facts.
                WebServiceProxy.TaxonAttributeService.DeleteSpeciesFacts(GetClientInformation(), outSpeciesFacts);
            }
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                WebServiceProxy.TaxonAttributeService.Logout(_clientInformation);
                _clientInformation = null;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // Test is done.
                // We are not interested in problems that
                // occures due to test of error handling.
            }
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void TestInitialize()
        {
            WebLoginResponse loginResponse;

            Configuration.InstallationType = InstallationType.ServerTest;
            loginResponse = WebServiceProxy.TaxonAttributeService.Login(Settings.Default.TestUserName,
                                                                        Settings.Default.TestPassword,
                                                                        ApplicationIdentifier.EVA.ToString(),
                                                                        false);
            if (loginResponse.IsNotNull())
            {
                _clientInformation = new WebClientInformation();
                _clientInformation.Locale = loginResponse.Locale;
                _clientInformation.Token = loginResponse.Token;
            }
        }

        /// <summary>
        /// Log out current user and log in requested user.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        /// <param name="applicationIdentifier"></param>
        protected void SetUserAndApplicationIdentifier(String userName, String userPassword, String applicationIdentifier)
        {
            WebLoginResponse loginResponse;

            Configuration.InstallationType = InstallationType.ServerTest;
            loginResponse = WebServiceProxy.TaxonAttributeService.Login(userName, userPassword, applicationIdentifier, false);

            _clientInformation = new WebClientInformation();
            _clientInformation.Locale = loginResponse.Locale;
            _clientInformation.Token = loginResponse.Token;
        }
    }
}

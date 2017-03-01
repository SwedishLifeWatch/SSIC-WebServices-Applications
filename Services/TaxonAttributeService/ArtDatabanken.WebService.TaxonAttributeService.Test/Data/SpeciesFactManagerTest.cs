using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using FactorManager = ArtDatabanken.WebService.TaxonAttributeService.Data.FactorManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeciesFactManager = ArtDatabanken.WebService.TaxonAttributeService.Data.SpeciesFactManager;

namespace ArtDatabanken.WebService.TaxonAttributeService.Test.Data
{
    [TestClass]
    public class SpeciesFactManagerTest : TestBase
    {
        [TestMethod]
        public void CreateSpeciesFacts()
        {
            Int32 currentMaxSpeciesFactId = GetContext().GetTaxonAttributeDatabase().GetMaxSpeciesFactId();
            List<WebSpeciesFact> inSpeciesFacts, outSpeciesFacts;

            UseTransaction = true;

            // Create species facts.
            inSpeciesFacts = new List<WebSpeciesFact>
                                     {
                                         new WebSpeciesFact
                                             {
                                                 FactorId = 34,
                                                 FieldValue5 = "Hej ' hopp",
                                                 HostId = 23,
                                                 IndividualCategoryId = 3,
                                                 IsFieldValue5Specified = true,
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
            SpeciesFactManager.CreateSpeciesFacts(GetContext(), inSpeciesFacts);
            outSpeciesFacts = SpeciesFactManager.GetSpeciesFactsByIds(GetContext(),
                                                                      new List<int>
                                                                          {
                                                                              GetContext().GetTaxonAttributeDatabase().GetMaxSpeciesFactId(),
                                                                              GetContext().GetTaxonAttributeDatabase().GetMaxSpeciesFactId() - 1
                                                                          });
            Assert.IsNotNull(outSpeciesFacts);
            Assert.IsTrue(outSpeciesFacts.IsNotEmpty());
            Assert.AreEqual(outSpeciesFacts.Count, inSpeciesFacts.Count);
            foreach (WebSpeciesFact createdSpeciesFact in outSpeciesFacts)
            {
                Assert.IsTrue(createdSpeciesFact.Id > currentMaxSpeciesFactId);
            }
        }

        [TestMethod]
        public void DeleteSpeciesFacts()
        {
            int currentMaxSpeciesFactId = GetContext().GetTaxonAttributeDatabase().GetMaxSpeciesFactId();
            List<WebSpeciesFact> inSpeciesFacts, outSpeciesFacts;

            UseTransaction = true;

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
            SpeciesFactManager.CreateSpeciesFacts(GetContext(), inSpeciesFacts);
            outSpeciesFacts = SpeciesFactManager.GetSpeciesFactsByIds(GetContext(),
                                                                      new List<int>
                                                                          {
                                                                              GetContext().GetTaxonAttributeDatabase().GetMaxSpeciesFactId(),
                                                                              GetContext().GetTaxonAttributeDatabase().GetMaxSpeciesFactId() - 1
                                                                          });
            Assert.IsNotNull(outSpeciesFacts);
            Assert.IsTrue(outSpeciesFacts.IsNotEmpty());
            Assert.AreEqual(outSpeciesFacts.Count, inSpeciesFacts.Count);
            foreach (WebSpeciesFact createdSpeciesFact in outSpeciesFacts)
            {
                Assert.IsTrue(createdSpeciesFact.Id > currentMaxSpeciesFactId);
            }

            // Delete species facts.
            SpeciesFactManager.DeleteSpeciesFacts(GetContext(), outSpeciesFacts);
        }

        private WebFactorField GetFactorField(Int32 factorDataTypeId,
                                              String databaseFieldName)
        {
            List<WebFactorDataType> factorDataTypes;

            factorDataTypes = FactorManager.GetFactorDataTypes(GetContext());
            foreach (WebFactorDataType factorDataType in factorDataTypes)
            {
                if (factorDataType.Id == factorDataTypeId)
                {
                    foreach (WebFactorField factorField in factorDataType.Fields)
                    {
                        if (factorField.DatabaseFieldName == databaseFieldName)
                        {
                            return factorField;
                        }
                    }
                }
            }

            return null;
        }

        [TestMethod]
        public void GetSpeciesFactQualities()
        {
            List<WebSpeciesFactQuality> speciesFactQualities;

            UseTransaction = true;
            speciesFactQualities = SpeciesFactManager.GetSpeciesFactQualities(GetContext());
            Assert.IsTrue(speciesFactQualities.IsNotEmpty());

            UseTransaction = false;
            speciesFactQualities = SpeciesFactManager.GetSpeciesFactQualities(GetContext());
            Assert.IsTrue(speciesFactQualities.IsNotEmpty());
        }

        [TestMethod]
        public void GetSpeciesFactsByIds()
        {
            List<WebSpeciesFact> speciesFacts;
            List<int> speciesFactIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            speciesFacts = SpeciesFactManager.GetSpeciesFactsByIds(GetContext(), speciesFactIds);
            Assert.IsTrue(speciesFacts.IsNotEmpty());
            Assert.AreEqual(speciesFacts.Count, speciesFactIds.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetSpeciesFactsByIdsTtooManyError()
        {
            List<WebSpeciesFact> speciesFacts;
            List<int> speciesFactIds = new List<int>();

            for (int speciesFactIndex = 0; speciesFactIndex < TaxonAttributeService.Settings.Default.MaxSpeciesFacts + 2; ++speciesFactIndex)
            {
                speciesFactIds.Add(speciesFactIndex);
            }

            speciesFacts = SpeciesFactManager.GetSpeciesFactsByIds(GetContext(), speciesFactIds);
            Assert.IsNotNull(speciesFacts);
            Assert.AreEqual(speciesFacts.Count, speciesFactIds.Count);
        }

        [TestMethod]
        public void GetSpeciesFactsByIdentifiers()
        {
            List<WebSpeciesFact> inSpeciesFacts, outSpeciesFacts;
            List<Int32> speciesFactIds = new List<Int32> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            WebSpeciesFact speciesFact;

            UseTransaction = true;
            inSpeciesFacts = SpeciesFactManager.GetSpeciesFactsByIds(GetContext(), speciesFactIds);
            outSpeciesFacts = SpeciesFactManager.GetSpeciesFactsByIdentifiers(GetContext(), inSpeciesFacts);
            Assert.AreEqual(inSpeciesFacts.Count, outSpeciesFacts.Count);

            UseTransaction = false;
            inSpeciesFacts = SpeciesFactManager.GetSpeciesFactsByIds(GetContext(), speciesFactIds);
            outSpeciesFacts = SpeciesFactManager.GetSpeciesFactsByIdentifiers(GetContext(), inSpeciesFacts);
            Assert.AreEqual(inSpeciesFacts.Count, outSpeciesFacts.Count);

            speciesFact = new WebSpeciesFact();
            speciesFact.FactorId = 655;
            speciesFact.FieldValue1 = 0;
            speciesFact.FieldValue2 = 0;
            speciesFact.FieldValue3 = 0;
            speciesFact.FieldValue4 = "A";
            speciesFact.FieldValue5 = null;
            speciesFact.HostId = -1;
            speciesFact.Id = 2167795;
            speciesFact.IndividualCategoryId = 0;
            speciesFact.IsFieldValue1Specified = false;
            speciesFact.IsFieldValue2Specified = false;
            speciesFact.IsFieldValue3Specified = false;
            speciesFact.IsFieldValue4Specified = true;
            speciesFact.IsFieldValue5Specified = false;
            speciesFact.IsHostSpecified = false;
            speciesFact.IsPeriodSpecified = true;
            speciesFact.ModifiedBy = "";
            speciesFact.ModifiedDate = DateTime.Now;
            speciesFact.PeriodId = 4;
            speciesFact.QualityId = 8;
            speciesFact.ReferenceId = 524;
            speciesFact.TaxonId = 700;
            inSpeciesFacts = new List<WebSpeciesFact>();
            inSpeciesFacts.Add(speciesFact);
            outSpeciesFacts = SpeciesFactManager.GetSpeciesFactsByIdentifiers(GetContext(), inSpeciesFacts);
            Assert.AreEqual(inSpeciesFacts.Count, outSpeciesFacts.Count);
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
            speciesFacts1 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add(2547); // 2:e gångbenparet lång relativt kroppslängden, ben/kroppslängd kvot = 2-8
            speciesFacts2 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts2.IsNotEmpty());
            Assert.IsTrue(speciesFacts1.Count > speciesFacts2.Count);

            // Test factors.
            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add((Int32)(FactorId.RedlistCategory));
            searchCriteria.PeriodIds = new List<Int32>();
            searchCriteria.PeriodIds.Add(2); // 2010
            speciesFacts1 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            // Test hosts.
            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.HostIds = new List<Int32>();
            searchCriteria.HostIds.Add(102656); // Hedsidenbi.
            speciesFacts1 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            // Test include not valid hosts.
            // Hard to test since there are no host values
            // in database that belongs to not valid taxa.
            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add(5);
            searchCriteria.IncludeNotValidHosts = false;
            speciesFacts1 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add(5);
            searchCriteria.IncludeNotValidHosts = true;
            speciesFacts2 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts2.IsNotEmpty());
            Assert.IsTrue(speciesFacts2.Count >= speciesFacts1.Count);

            // Test include not valid taxa.
            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add(5);
            searchCriteria.IncludeNotValidTaxa = false;
            speciesFacts1 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add(5);
            searchCriteria.IncludeNotValidTaxa = true;
            speciesFacts2 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts2.IsNotEmpty());
            Assert.IsTrue(speciesFacts2.Count > speciesFacts1.Count);

            // Test individual categories.
            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.IndividualCategoryIds = new List<Int32>();
            searchCriteria.IndividualCategoryIds.Add(9); // Ungar (juveniler)
            speciesFacts1 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());
            searchCriteria.IndividualCategoryIds.Add(10); // Vuxna (imago).
            speciesFacts1 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            // Test periods.
            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add((Int32)(FactorId.RedlistCategory));
            searchCriteria.PeriodIds = new List<Int32>();
            searchCriteria.PeriodIds.Add(1); // 2000
            speciesFacts1 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());
            searchCriteria.PeriodIds.Add(2); // 2005
            speciesFacts1 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            // Test periods and individual categories.
            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.PeriodIds = new List<Int32>();
            searchCriteria.PeriodIds.Add(3); // 2010
            searchCriteria.IndividualCategoryIds = new List<Int32>();
            searchCriteria.IndividualCategoryIds.Add(10); // Vuxna (imago).
            speciesFacts1 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            // Test taxa.
            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            speciesFacts1 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            // Test Boolean species fact field condition.
            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add((Int32)(FactorId.RedlistCategory));
            searchCriteria.FieldSearchCriteria = new List<WebSpeciesFactFieldSearchCriteria>();
            searchCriteria.FieldSearchCriteria.Add(new WebSpeciesFactFieldSearchCriteria());
            searchCriteria.FieldSearchCriteria[0].FactorField = GetFactorField(30, "tal2");
            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.Equal;
            searchCriteria.FieldSearchCriteria[0].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[0].Values.Add(Boolean.TrueString);
            speciesFacts1 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            // Test Float64 species fact field condition.
            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add(2547); // 2:e gångbenparet lång relativt kroppslängden, ben/kroppslängd kvot = 2-8
            searchCriteria.FieldSearchCriteria = new List<WebSpeciesFactFieldSearchCriteria>();
            searchCriteria.FieldSearchCriteria.Add(new WebSpeciesFactFieldSearchCriteria());
            searchCriteria.FieldSearchCriteria[0].FactorField = GetFactorField(106, "tal1");
            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.Greater;
            searchCriteria.FieldSearchCriteria[0].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[0].Values.Add("0");
            speciesFacts1 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.Greater;
            searchCriteria.FieldSearchCriteria[0].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[0].Values.Add("2");
            speciesFacts1 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsEmpty());

            // Test Int32 species fact field condition.
            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add((Int32)(FactorId.RedlistCategory));
            searchCriteria.FieldSearchCriteria = new List<WebSpeciesFactFieldSearchCriteria>();
            searchCriteria.FieldSearchCriteria.Add(new WebSpeciesFactFieldSearchCriteria());
            searchCriteria.FieldSearchCriteria[0].FactorField = GetFactorField(30, "tal1");
            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.Equal;
            searchCriteria.FieldSearchCriteria[0].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[0].Values.Add(2.WebToString()); // CR
            searchCriteria.FieldSearchCriteria.Add(new WebSpeciesFactFieldSearchCriteria());
            searchCriteria.FieldSearchCriteria[1].FactorField = GetFactorField(30, "tal1");
            searchCriteria.FieldSearchCriteria[1].Operator = CompareOperator.Equal;
            searchCriteria.FieldSearchCriteria[1].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[1].Values.Add(3.WebToString()); // EN
            searchCriteria.FieldSearchCriteria.Add(new WebSpeciesFactFieldSearchCriteria());
            searchCriteria.FieldSearchCriteria[2].FactorField = GetFactorField(30, "tal1");
            searchCriteria.FieldSearchCriteria[2].Operator = CompareOperator.Equal;
            searchCriteria.FieldSearchCriteria[2].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[2].Values.Add(4.WebToString()); // VU
            searchCriteria.FieldLogicalOperator = LogicalOperator.Or;
            speciesFacts1 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            // Test String species fact field condition.
            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add((Int32)(FactorId.RedlistCategory));
            searchCriteria.FieldSearchCriteria = new List<WebSpeciesFactFieldSearchCriteria>();
            searchCriteria.FieldSearchCriteria.Add(new WebSpeciesFactFieldSearchCriteria());
            searchCriteria.FieldSearchCriteria[0].FactorField = GetFactorField(30, "text1");
            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.Equal;
            searchCriteria.FieldSearchCriteria[0].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[0].Values.Add("VU");
            speciesFacts1 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.Like;
            searchCriteria.FieldSearchCriteria[0].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[0].Values.Add("%VU%");
            speciesFacts2 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts2.IsNotEmpty());
            Assert.IsTrue(speciesFacts1.Count < speciesFacts2.Count);

            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.Equal;
            searchCriteria.FieldSearchCriteria[0].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[0].Values.Add("V'U");
            speciesFacts2 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts2.IsEmpty());
        }

        [TestMethod]
        public void GetSpeciesFactsBySearchCriteriaFactFieldSearchCriteria()
        {
            // Test String species fact field condition in combination with Enum String.

            List<WebSpeciesFact> speciesFacts1;
            WebSpeciesFactSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add(1001);
            searchCriteria.FieldLogicalOperator = LogicalOperator.Or;

            // Test List of Enums as String species fact field condition.
            searchCriteria.FieldSearchCriteria = new List<WebSpeciesFactFieldSearchCriteria>();
            searchCriteria.FieldSearchCriteria.Add(new WebSpeciesFactFieldSearchCriteria());
            searchCriteria.FieldSearchCriteria[0].FactorField = GetFactorField(52, "tal1");
            Assert.IsTrue(searchCriteria.FieldSearchCriteria[0].FactorField.IsEnumField,"Choose other factor field that has enum fields. I.e. tal1.");
            searchCriteria.FieldSearchCriteria[0].IsEnumAsString = false;
            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.Equal;
            searchCriteria.FieldSearchCriteria[0].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[0].Values.Add("1");
            searchCriteria.FieldSearchCriteria[0].Values.Add("2");

            // Test String species fact field condition.
            searchCriteria.FieldSearchCriteria.Add(new WebSpeciesFactFieldSearchCriteria());
            searchCriteria.FieldSearchCriteria[1].FactorField = GetFactorField(52, "text2");
            searchCriteria.FieldSearchCriteria[1].Operator = CompareOperator.Like;
            searchCriteria.FieldSearchCriteria[1].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[1].Values.Add("bedömd för adulter");

            // Test Double species fact field condition.
            searchCriteria.FieldSearchCriteria.Add(new WebSpeciesFactFieldSearchCriteria());
            searchCriteria.FieldSearchCriteria[2].FactorField = GetFactorField(52, "tal2");
            searchCriteria.FieldSearchCriteria[2].Operator = CompareOperator.GreaterOrEqual;
            searchCriteria.FieldSearchCriteria[2].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[2].Values.Add("11");

            // Test Double species fact field condition.
            searchCriteria.FieldSearchCriteria.Add(new WebSpeciesFactFieldSearchCriteria());
            searchCriteria.FieldSearchCriteria[3].FactorField = GetFactorField(52, "tal3");
            searchCriteria.FieldSearchCriteria[3].Operator = CompareOperator.LessOrEqual;
            searchCriteria.FieldSearchCriteria[3].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[3].Values.Add("22");

            speciesFacts1 = SpeciesFactManager.GetSpeciesFactsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxaCountBySearchCriteriaFactFieldSearchCriteria()
        {
            Int32 taxaCount;
            WebSpeciesFactSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add(1001);
            searchCriteria.FieldLogicalOperator = LogicalOperator.Or;
            searchCriteria.FieldSearchCriteria = new List<WebSpeciesFactFieldSearchCriteria>();

            // Test Double species fact field condition.
            searchCriteria.FieldSearchCriteria.Add(new WebSpeciesFactFieldSearchCriteria());
            searchCriteria.FieldSearchCriteria[0].FactorField = GetFactorField(52, "tal2");
            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.GreaterOrEqual;
            searchCriteria.FieldSearchCriteria[0].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[0].Values.Add("11");

            taxaCount = SpeciesFactManager.GetTaxaCountBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxaCount > 0);

            // Test String species fact field condition.
            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add((Int32)(FactorId.RedlistCategory));
            searchCriteria.FieldSearchCriteria = new List<WebSpeciesFactFieldSearchCriteria>();
            searchCriteria.FieldSearchCriteria.Add(new WebSpeciesFactFieldSearchCriteria());
            searchCriteria.FieldSearchCriteria[0].FactorField = GetFactorField(30, "text1");
            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.Equal;
            searchCriteria.FieldSearchCriteria[0].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[0].Values.Add("VU");
            taxaCount = SpeciesFactManager.GetTaxaCountBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxaCount > 0);

            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.Like;
            searchCriteria.FieldSearchCriteria[0].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[0].Values.Add("%VU%");
            taxaCount = SpeciesFactManager.GetTaxaCountBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxaCount > 0);

            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.Equal;
            searchCriteria.FieldSearchCriteria[0].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[0].Values.Add("V'U");
            taxaCount = SpeciesFactManager.GetTaxaCountBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxaCount == 0);
        }

        [TestMethod]
        public void GetTaxaBySearchCriteriaFactFieldSearchCriteria()
        {
            List<WebTaxon> taxa;
            WebSpeciesFactSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add(1001);
            searchCriteria.FieldLogicalOperator = LogicalOperator.Or;
            searchCriteria.FieldSearchCriteria = new List<WebSpeciesFactFieldSearchCriteria>();

            // Test Double species fact field condition.
            searchCriteria.FieldSearchCriteria.Add(new WebSpeciesFactFieldSearchCriteria());
            searchCriteria.FieldSearchCriteria[0].FactorField = GetFactorField(52, "tal2");
            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.GreaterOrEqual;
            searchCriteria.FieldSearchCriteria[0].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[0].Values.Add("11");

            taxa = SpeciesFactManager.GetTaxaBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxa.Count > 0);

            // Test String species fact field condition.
            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add((Int32)(FactorId.RedlistCategory));
            searchCriteria.FieldSearchCriteria = new List<WebSpeciesFactFieldSearchCriteria>();
            searchCriteria.FieldSearchCriteria.Add(new WebSpeciesFactFieldSearchCriteria());
            searchCriteria.FieldSearchCriteria[0].FactorField = GetFactorField(30, "text1");
            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.Equal;
            searchCriteria.FieldSearchCriteria[0].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[0].Values.Add("VU");
            taxa = SpeciesFactManager.GetTaxaBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.Like;
            searchCriteria.FieldSearchCriteria[0].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[0].Values.Add("%VU%");
            taxa = SpeciesFactManager.GetTaxaBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.Equal;
            searchCriteria.FieldSearchCriteria[0].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[0].Values.Add("V'U");
            taxa = SpeciesFactManager.GetTaxaBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxa.IsEmpty());
        }

        [TestMethod]
        public void UpdateSpeciesFacts()
        {
            int currentMaxSpeciesFactId = GetContext().GetTaxonAttributeDatabase().GetMaxSpeciesFactId();
            List<WebSpeciesFact> inSpeciesFacts, outSpeciesFacts;

            UseTransaction = true;

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
            SpeciesFactManager.CreateSpeciesFacts(GetContext(), inSpeciesFacts);
            outSpeciesFacts = SpeciesFactManager.GetSpeciesFactsByIds(GetContext(),
                                                                      new List<int>
                                                                          {
                                                                              GetContext().GetTaxonAttributeDatabase().GetMaxSpeciesFactId(),
                                                                              GetContext().GetTaxonAttributeDatabase().GetMaxSpeciesFactId() - 1
                                                                          });
            Assert.IsNotNull(outSpeciesFacts);
            Assert.IsTrue(outSpeciesFacts.IsNotEmpty());
            Assert.AreEqual(outSpeciesFacts.Count, inSpeciesFacts.Count);
            foreach (WebSpeciesFact createdSpeciesFact in outSpeciesFacts)
            {
                Assert.IsTrue(createdSpeciesFact.Id > currentMaxSpeciesFactId);
            }

            // Update species facts.
            List<int> speciesFactIds = outSpeciesFacts.Select(speciesFact => speciesFact.Id).ToList();

            inSpeciesFacts = SpeciesFactManager.GetSpeciesFactsByIds(GetContext(), speciesFactIds);
            SpeciesFactManager.UpdateSpeciesFacts(GetContext(), inSpeciesFacts);
            outSpeciesFacts = SpeciesFactManager.GetSpeciesFactsByIds(GetContext(), speciesFactIds);
            foreach (WebSpeciesFact speciesFact in outSpeciesFacts)
            {
                Assert.AreEqual(speciesFact.ModifiedBy, "TestFirstName TestLastName");
            }

            // Delete species facts.
            SpeciesFactManager.DeleteSpeciesFacts(GetContext(), outSpeciesFacts);
        }
    }
}
using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.TaxonAttributeService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.TaxonAttributeService
{
    [TestClass]
    public class SpeciesFactDataSourceTest : TestBase
    {
        private SpeciesFactDataSource _speciesFactDataSource;

        public SpeciesFactDataSourceTest()
        {
            _speciesFactDataSource = null;
        }

        private SpeciesFactDataSource GetSpeciesFactDataSource(bool refresh = false)
        {
            if (_speciesFactDataSource.IsNull() || refresh)
            {
                _speciesFactDataSource = new SpeciesFactDataSource();
            }

            return _speciesFactDataSource;
        }

        protected override string GetTestApplicationName()
        {
            return ApplicationIdentifier.EVA.ToString();
        }

        [TestMethod]
        public void GetSpeciesFactQualities()
        {
            SpeciesFactQualityList speciesFactQualities;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                speciesFactQualities = GetSpeciesFactDataSource(true).GetSpeciesFactQualities(GetUserContext());
                Assert.IsTrue(speciesFactQualities.IsNotEmpty());
            }

            speciesFactQualities = GetSpeciesFactDataSource().GetSpeciesFactQualities(GetUserContext());
            Assert.IsTrue(speciesFactQualities.IsNotEmpty());
        }

        [TestMethod]
        public void GetSpeciesFactsByIds()
        {
            List<int> speciesFactIds = new List<int> { 1, 2 };
            SpeciesFactList speciesFacts;
            bool speciesFactFound;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                speciesFacts = GetSpeciesFactDataSource(true).GetSpeciesFacts(GetUserContext(), speciesFactIds);
                Assert.IsTrue(speciesFacts.IsNotEmpty());
                Assert.AreEqual(speciesFacts.Count, speciesFactIds.Count);

                foreach (ISpeciesFact speciesFact in speciesFacts)
                {
                    Assert.IsNotNull(speciesFact.Taxon);
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

            speciesFacts = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), speciesFactIds);
            Assert.IsTrue(speciesFacts.IsNotEmpty());
            Assert.AreEqual(speciesFacts.Count, speciesFactIds.Count);

            foreach (ISpeciesFact speciesFact in speciesFacts)
            {
                Assert.IsNotNull(speciesFact.Taxon);
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
        public void GetSpeciesFactsBySpeciesFactIdentifiers()
        {
            SpeciesFactList inSpeciesFacts, outSpeciesFacts;
            List<int> speciesFactIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            SpeciesFactDataSource dataSource;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                dataSource = GetSpeciesFactDataSource(true);
                inSpeciesFacts = dataSource.GetSpeciesFacts(GetUserContext(), speciesFactIds);
                outSpeciesFacts = dataSource.GetSpeciesFacts(GetUserContext(), inSpeciesFacts);
                Assert.AreEqual(inSpeciesFacts.Count, outSpeciesFacts.Count);
            }

            dataSource = GetSpeciesFactDataSource();
            inSpeciesFacts = dataSource.GetSpeciesFacts(GetUserContext(), speciesFactIds);
            outSpeciesFacts = dataSource.GetSpeciesFacts(GetUserContext(), inSpeciesFacts);
            Assert.AreEqual(inSpeciesFacts.Count, outSpeciesFacts.Count);
        }

        [TestMethod]
        public void GetSpeciesFactsBySearchCriteria()
        {
            SpeciesFactList speciesFacts1, speciesFacts2;
            ISpeciesFactSearchCriteria searchCriteria;

            // Test factor data types.
            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.FactorDataTypes = new FactorDataTypeList();
            searchCriteria.FactorDataTypes.Add(CoreData.FactorManager.GetFactorDataType(GetUserContext(), 5)); // EK-utbredning.
            speciesFacts1 = GetSpeciesFactDataSource(true).GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), 513)); // Fjällen
            speciesFacts2 = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts2.IsNotEmpty());
            Assert.IsTrue(speciesFacts1.Count > speciesFacts2.Count);

            // Test factors.
            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), (Int32)(FactorId.RedlistCategory)));
            searchCriteria.Periods = new PeriodList();
            searchCriteria.Periods.Add(CoreData.FactorManager.GetPeriod(GetUserContext(), 3)); // 2010
            speciesFacts1 = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Hosts = new TaxonList();
            searchCriteria.Hosts.Add(CoreData.TaxonManager.GetTaxon(GetUserContext(), 102656)); // Hedsidenbi.
            speciesFacts1 = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            // Test include not valid hosts.
            // Hard to test since there are no host values
            // in database that belongs to not valid taxa.
            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), 5));
            searchCriteria.IncludeNotValidHosts = false;
            speciesFacts1 = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), 5));
            searchCriteria.IncludeNotValidHosts = true;
            speciesFacts2 = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts2.IsNotEmpty());
            Assert.IsTrue(speciesFacts2.Count >= speciesFacts1.Count);

            // Test include not valid taxa.
            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), 5));
            searchCriteria.IncludeNotValidTaxa = false;
            speciesFacts1 = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), 5));
            searchCriteria.IncludeNotValidTaxa = true;
            speciesFacts2 = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts2.IsNotEmpty());
            Assert.IsTrue(speciesFacts2.Count > speciesFacts1.Count);

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.IndividualCategories = new IndividualCategoryList();
            searchCriteria.IndividualCategories.Add(CoreData.FactorManager.GetIndividualCategory(GetUserContext(), 9)); // Ungar (juveniler)
            speciesFacts1 = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());
            searchCriteria.IndividualCategories.Add(CoreData.FactorManager.GetIndividualCategory(GetUserContext(), 10)); // Vuxna (imago).
            speciesFacts1 = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), (Int32)(FactorId.RedlistCategory)));
            searchCriteria.Periods = new PeriodList();
            searchCriteria.Periods.Add(CoreData.FactorManager.GetPeriod(GetUserContext(), 3)); // 2010
            speciesFacts1 = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());
            searchCriteria.Periods.Add(CoreData.FactorManager.GetPeriod(GetUserContext(), 5)); // 2013 HELCOM
            speciesFacts1 = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), (Int32)(FactorId.RedlistCategory)));
            searchCriteria.Periods = new PeriodList();
            searchCriteria.Periods.Add(CoreData.FactorManager.GetPeriod(GetUserContext(), 4)); // 2015
            searchCriteria.IndividualCategories = new IndividualCategoryList();
            searchCriteria.IndividualCategories.Add(CoreData.FactorManager.GetIndividualCategory(GetUserContext(), 10)); // Vuxna (imago).
            speciesFacts1 = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsEmpty());

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Taxa = new TaxonList();
            searchCriteria.Taxa.Add(CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper));
            speciesFacts1 = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            // Test Boolean species fact field condition.
            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), (Int32)(FactorId.RedlistCategory)));
            searchCriteria.FieldSearchCriteria = new SpeciesFactFieldSearchCriteriaList();
            searchCriteria.FieldSearchCriteria.Add(new SpeciesFactFieldSearchCriteria());
            searchCriteria.FieldSearchCriteria[0].FactorField = searchCriteria.Factors[0].DataType.Fields[1];
            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.Equal;
            searchCriteria.FieldSearchCriteria[0].AddValue(Boolean.TrueString);
            speciesFacts1 = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            // Test Float64 species fact field condition.
            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), 2547)); // 2:e gångbenparet lång relativt kroppslängden, ben/kroppslängd kvot = 2-8
            searchCriteria.FieldSearchCriteria = new SpeciesFactFieldSearchCriteriaList();
            searchCriteria.FieldSearchCriteria.Add(new SpeciesFactFieldSearchCriteria());
            searchCriteria.FieldSearchCriteria[0].FactorField = searchCriteria.Factors[0].DataType.Fields[0];
            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.Greater;
            searchCriteria.FieldSearchCriteria[0].AddValue("0");
            speciesFacts1 = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.Greater;
            searchCriteria.FieldSearchCriteria[0].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[0].AddValue("2");
            speciesFacts1 = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsEmpty());

            // Test Int32 species fact field condition.
            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), (Int32)(FactorId.RedlistCategory)));
            searchCriteria.FieldSearchCriteria = new SpeciesFactFieldSearchCriteriaList();
            searchCriteria.FieldSearchCriteria.Add(new SpeciesFactFieldSearchCriteria());
            searchCriteria.FieldSearchCriteria[0].FactorField = searchCriteria.Factors[0].DataType.Fields[0];
            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.Equal;
            searchCriteria.FieldSearchCriteria[0].AddValue(2.WebToString()); // CR
            searchCriteria.FieldSearchCriteria.Add(new SpeciesFactFieldSearchCriteria());
            searchCriteria.FieldSearchCriteria[1].FactorField = searchCriteria.Factors[0].DataType.Fields[0];
            searchCriteria.FieldSearchCriteria[1].Operator = CompareOperator.Equal;
            searchCriteria.FieldSearchCriteria[1].AddValue(3.WebToString()); // EN
            searchCriteria.FieldSearchCriteria.Add(new SpeciesFactFieldSearchCriteria());
            searchCriteria.FieldSearchCriteria[2].FactorField = searchCriteria.Factors[0].DataType.Fields[0];
            searchCriteria.FieldSearchCriteria[2].Operator = CompareOperator.Equal;
            searchCriteria.FieldSearchCriteria[2].AddValue(4.WebToString()); // VU
            searchCriteria.FieldLogicalOperator = LogicalOperator.Or;
            speciesFacts1 = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            // Test String species fact field condition.
            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), (Int32)(FactorId.RedlistCategory)));
            searchCriteria.FieldSearchCriteria = new SpeciesFactFieldSearchCriteriaList();
            searchCriteria.FieldSearchCriteria.Add(new SpeciesFactFieldSearchCriteria());
            searchCriteria.FieldSearchCriteria[0].FactorField = searchCriteria.Factors[0].DataType.Fields[2];
            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.Equal;
            searchCriteria.FieldSearchCriteria[0].AddValue("VU");
            speciesFacts1 = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts1.IsNotEmpty());

            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.Like;
            searchCriteria.FieldSearchCriteria[0].Values = new List<String>();
            searchCriteria.FieldSearchCriteria[0].AddValue("%VU%");
            speciesFacts2 = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), searchCriteria);
            Assert.IsTrue(speciesFacts2.IsNotEmpty());
            Assert.IsTrue(speciesFacts1.Count < speciesFacts2.Count);
        }

        [TestMethod]
        public void UpdateSpeciesFact()
        {
            List<int> speciesFactIds = new List<int> { 388861, 388873, 388874, 388875, 388876, 388877 };
            SpeciesFactList speciesFacts;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                speciesFacts = GetSpeciesFactDataSource(true).GetSpeciesFacts(GetUserContext(), speciesFactIds);
                foreach (ISpeciesFact speciesFact in speciesFacts)
                {
                    if (speciesFact.AllowManualUpdate)
                    {
                        for (int fieldIndex = 0; fieldIndex < speciesFact.Fields.Count; fieldIndex++)
                        {
                            speciesFact.Fields[fieldIndex].Value = null;
                        }
                    }
                }
                GetSpeciesFactDataSource().DeleteSpeciesFacts(GetUserContext(), speciesFacts);
            }

            // TODO: complete with tests for create
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                speciesFacts = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), speciesFactIds);
                foreach (ISpeciesFact speciesFact in speciesFacts)
                {
                    if (speciesFact.AllowManualUpdate)
                    {
                        for (int i = 0; i < speciesFact.Fields.Count; i++)
                        {
                            switch (speciesFact.Fields[i].Type.DataType)
                            {
                                case FactorFieldDataTypeId.Boolean:
                                    speciesFact.Fields[i].Value = !(bool)speciesFact.Fields[i].Value;
                                    break;
                                case FactorFieldDataTypeId.Double:
                                    speciesFact.Fields[i].Value = 8.76;
                                    break;
                                case FactorFieldDataTypeId.Enum:
                                    speciesFact.Fields[i].Value = speciesFact.Fields[i].FactorFieldEnum.Values[0];
                                    break;
                                case FactorFieldDataTypeId.Int32:
                                    speciesFact.Fields[i].Value = 57;
                                    break;
                                case FactorFieldDataTypeId.String:
                                    speciesFact.Fields[i].Value = string.Empty;
                                    break;
                            }
                        }
                    }
                }
                GetSpeciesFactDataSource().UpdateSpeciesFacts(GetUserContext(), speciesFacts);
                speciesFacts = GetSpeciesFactDataSource().GetSpeciesFacts(GetUserContext(), speciesFactIds);
                foreach (ISpeciesFact speciesFact in speciesFacts)
                {
                    for (int i = 0; i < speciesFact.Fields.Count; i++)
                    {
                        switch (speciesFact.Fields[i].Type.DataType)
                        {
                            case FactorFieldDataTypeId.Boolean:
                                Assert.AreEqual(speciesFact.Fields[i].Value, !(bool)speciesFact.Fields[i].Value);
                                break;
                            case FactorFieldDataTypeId.Double:
                                Assert.AreEqual(speciesFact.Fields[i].Value, 8.76);
                                break;
                            case FactorFieldDataTypeId.Enum:
                                Assert.AreEqual(speciesFact.Fields[i].Value, speciesFact.Fields[i].FactorFieldEnum.Values[0]);
                                break;
                            case FactorFieldDataTypeId.Int32:
                                Assert.AreEqual(speciesFact.Fields[i].Value, 57);
                                break;
                            case FactorFieldDataTypeId.String:
                                Assert.IsNull(speciesFact.Fields[i].Value);
                                break;
                        }
                    }
                }
            }
        }
    }
}
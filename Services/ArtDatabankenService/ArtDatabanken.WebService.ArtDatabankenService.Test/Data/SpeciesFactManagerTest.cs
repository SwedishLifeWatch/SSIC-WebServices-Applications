using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class SpeciesFactManagerTest : TestBase
    {
        public SpeciesFactManagerTest()
            : base(true, 120)
        {
        }

        [TestMethod]
        public void AddUserSelectedParameters()
        {
            SpeciesFactManager.AddUserSelectedParameters(GetContext(), GetUserParameterSelection());
            SpeciesFactManager.DeleteUserSelectedParameters(GetContext());
        }

        [TestMethod]
        public void AddUserSelectedHosts()
        {
            foreach (UserSelectedTaxonUsage hostUsage in Enum.GetValues(typeof(UserSelectedTaxonUsage)))
            {
                SpeciesFactManager.AddUserSelectedHosts(GetContext(), TaxonManagerTest.GetSomeTaxonIds(), hostUsage);
                SpeciesFactManager.DeleteUserSelectedHosts(GetContext());
            }
        }

        [TestMethod]
        public void DeleteUserSelectedHosts()
        {
            foreach (UserSelectedTaxonUsage hostUsage in Enum.GetValues(typeof(UserSelectedTaxonUsage)))
            {
                SpeciesFactManager.AddUserSelectedHosts(GetContext(), TaxonManagerTest.GetSomeTaxonIds(), hostUsage);
                SpeciesFactManager.DeleteUserSelectedHosts(GetContext());
            }
        }

        [TestMethod]
        public void DeleteUserSelectedParameters()
        {
            SpeciesFactManager.AddUserSelectedParameters(GetContext(), GetUserParameterSelection());
            SpeciesFactManager.DeleteUserSelectedParameters(GetContext());
        }

        [TestMethod]
        public void GetEndangeredLists()
        {
            List<WebEndangeredList> endangeredLists;

            endangeredLists = SpeciesFactManager.GetEndangeredLists(GetContext());
            Assert.IsTrue(endangeredLists.IsNotEmpty());
        }

        public static DataTable GetUserSelectedHosts(WebServiceContext context)
        {
            return GetUserSelectedHosts(context, TaxonManagerTest.GetSomeTaxonIds(2));
        }

        public static DataTable GetUserSelectedHosts(WebServiceContext context,
                                                     List<Int32> taxaIds)
        {
            DataColumn column;
            DataRow row;
            DataTable hostTable;

            hostTable = new DataTable(UserSelectedHostData.TABLE_NAME);
            column = new DataColumn(UserSelectedHostData.REQUEST_ID, typeof(Int32));
            hostTable.Columns.Add(column);
            column = new DataColumn(UserSelectedHostData.HOST_ID, typeof(Int32));
            hostTable.Columns.Add(column);
            column = new DataColumn(UserSelectedHostData.HOST_USAGE, typeof(String));
            hostTable.Columns.Add(column);
            foreach (Int32 taxonId in taxaIds)
            {
                row = hostTable.NewRow();
                row[0] = context.RequestId;
                row[1] = taxonId;
                row[2] = UserSelectedTaxonUsage.Output.ToString();
                hostTable.Rows.Add(row);
            }
            return hostTable;
        }

        public static DataTable GetNewSpeciesFactTable(WebServiceContext context)
        {
            WebSpeciesFact speciesFact;
            List<WebSpeciesFact> speciesFacts;

            // Create new species fact with random values.
            speciesFact = GetOneSpeciesFact(context);
            speciesFact.FactorId = 10;
            speciesFact.FieldValue1 = 10;
            speciesFact.IsFieldValue1Specified = true;
            speciesFact.IndividualCategoryId = 2;
            speciesFact.TaxonId = 209210;

            speciesFacts = new List<WebSpeciesFact>();
            speciesFacts.Add(speciesFact);
            return SpeciesFactManager.GetSpeciesFactTable(context, speciesFacts, DateTime.Now, "");
        }

        public static WebSpeciesFact GetOneSpeciesFact(WebServiceContext context)
        {
            return GetSomeSpeciesFacts(context)[0];
        }

        public static WebSpeciesFactQuality GetOneSpeciesFactQuality(WebServiceContext context)
        {
            return SpeciesFactManager.GetSpeciesFactQualities(context)[0];
        }

        public static WebTaxonCountyOccurrence GetOneTaxonCountyOccurrence(WebServiceContext context)
        {
            Int32 taxonId;
            List<WebTaxonCountyOccurrence> countyOccurrencies;

            taxonId = BEAR_TAXON_ID;
            countyOccurrencies = SpeciesFactManager.GetTaxonCountyOccurence(context, taxonId);
            return countyOccurrencies[0];
        }

        public static List<Int32> GetSomeSpeciesFactIds(WebServiceContext context)
        {
            List<Int32> speciesFactIds;

            speciesFactIds = new List<Int32>();
            foreach (WebSpeciesFact speciesFact in GetSomeSpeciesFacts(context))
            {
                speciesFactIds.Add(speciesFact.Id);
            }
            return speciesFactIds;
        }

        public static List<WebSpeciesFact> GetSomeSpeciesFacts(WebServiceContext context)
        {
            WebUserParameterSelection userParameterSelection;
            userParameterSelection = new WebUserParameterSelection();
            userParameterSelection.TaxonIds = TaxonManagerTest.GetSomeTaxonIds();
            return SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(context, userParameterSelection);
        }

        [TestMethod]
        public void GetSpeciesFactQualities()
        {
            List<WebSpeciesFactQuality> speciesFactQualities;

            speciesFactQualities = SpeciesFactManager.GetSpeciesFactQualities(GetContext());
            Assert.IsNotNull(speciesFactQualities);
            Assert.IsTrue(speciesFactQualities.IsNotEmpty());
        }

        [TestMethod]
        public void GetSpeciesFactsById()
        {
            List<WebSpeciesFact> speciesFacts;
            List<Int32> speciesFactIds;

            speciesFactIds = new List<Int32>();
            speciesFactIds.Add(1);
            speciesFacts = SpeciesFactManager.GetSpeciesFactsById(GetContext(), speciesFactIds);
            Assert.IsNotNull(speciesFacts);
            Assert.AreEqual(speciesFacts.Count, speciesFactIds.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetSpeciesFactsByIdToManyError()
        {
            Int32 speciesFactIdIndex;
            List<WebSpeciesFact> speciesFacts;
            List<Int32> speciesFactIds;

            speciesFactIds = new List<Int32>();
            for (speciesFactIdIndex = 0; speciesFactIdIndex < (SpeciesFactManager.MAX_SPECIES_FACTS + 2); speciesFactIdIndex++)
            {
                speciesFactIds.Add(speciesFactIdIndex);
            }
            speciesFacts = SpeciesFactManager.GetSpeciesFactsById(GetContext(), speciesFactIds);
            Assert.IsNotNull(speciesFacts);
            Assert.AreEqual(speciesFacts.Count, speciesFactIds.Count);
        }

        [TestMethod]
        public void GetSpeciesFactsByIdentifier()
        {
            List<WebSpeciesFact> changeSpeciesFacts;
            List<WebSpeciesFact> oldSpeciesFacts;
            List<WebSpeciesFact> speciesFacts;

            // Get same species facts again.
            oldSpeciesFacts = GetSomeSpeciesFacts(GetContext());
            speciesFacts = SpeciesFactManager.GetSpeciesFactsByIdentifier(GetContext(), oldSpeciesFacts);
            Assert.AreEqual(oldSpeciesFacts.Count, speciesFacts.Count);

            // Delete some species facts.
            changeSpeciesFacts = new List<WebSpeciesFact>();
            changeSpeciesFacts.Add(speciesFacts[0]);
            changeSpeciesFacts.Add(speciesFacts[1]);
            changeSpeciesFacts.Add(speciesFacts[2]);
            SpeciesFactManager.UpdateSpeciesFacts(GetContext(), null, changeSpeciesFacts, null);
            speciesFacts = SpeciesFactManager.GetSpeciesFactsByIdentifier(GetContext(), oldSpeciesFacts);
            Assert.AreEqual(oldSpeciesFacts.Count - 3, speciesFacts.Count);

            // Create some species facts.
            SpeciesFactManager.UpdateSpeciesFacts(GetContext(), changeSpeciesFacts, null, null);
            speciesFacts = SpeciesFactManager.GetSpeciesFactsByIdentifier(GetContext(), oldSpeciesFacts);
            Assert.AreEqual(oldSpeciesFacts.Count, speciesFacts.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetSpeciesFactsByIdentifierToManyError()
        {
            Int32 speciesFactIndex;
            List<WebSpeciesFact> speciesFacts;

            speciesFacts = GetSomeSpeciesFacts(GetContext());
            for (speciesFactIndex = 0; speciesFactIndex < (SpeciesFactManager.MAX_SPECIES_FACTS + 2); speciesFactIndex++)
            {
                speciesFacts.Add(speciesFacts[0]);
            }
            speciesFacts = SpeciesFactManager.GetSpeciesFactsByIdentifier(GetContext(), speciesFacts);
            Assert.IsNotNull(speciesFacts);
        }

        [TestMethod]
        public void GetSpeciesFactsByUserParameterSelection()
        {
            List<Int32> factorIds, hostIds, individualCategoryIds,
                        periodIds, referenceIds, taxonIds;
            List<WebSpeciesFact> speciesFacts;
            WebUserParameterSelection userParameterSelection;
            Int32 speciesFactCount;

            userParameterSelection = new WebUserParameterSelection();
            factorIds = FactorManagerTest.GetSomeFactorIds();
            hostIds = TaxonManagerTest.GetSomeTaxonIds();
            individualCategoryIds = IndividualCategoryManagerTest.GetSomeIndividualCategoryIds(GetContext());
            individualCategoryIds.RemoveAt(0);
            periodIds = PeriodManagerTest.GetSomePeriodIds(GetContext());
            periodIds.RemoveAt(periodIds.Count - 2);
            periodIds.RemoveAt(periodIds.Count - 2);
            referenceIds = ReferenceManagerTest.GetSomeReferenceIds(GetContext());
            taxonIds = TaxonManagerTest.GetSomeTaxonIds();

            userParameterSelection.TaxonIds = taxonIds;
            speciesFacts = SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(GetContext(), userParameterSelection);
            Assert.IsTrue(speciesFacts.IsNotEmpty());
            speciesFactCount = speciesFacts.Count;

            userParameterSelection.FactorIds = factorIds;
            speciesFacts = SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(GetContext(), userParameterSelection);
            Assert.IsTrue(speciesFacts.IsNotEmpty());
            Assert.IsTrue(speciesFactCount > speciesFacts.Count);
            userParameterSelection.FactorIds = null;

            userParameterSelection.IndividualCategoryIds = individualCategoryIds;
            speciesFacts = SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(GetContext(), userParameterSelection);
            if (speciesFacts.IsNotEmpty())
            {
                Assert.IsTrue(speciesFactCount > speciesFacts.Count);
            }
            userParameterSelection.IndividualCategoryIds = null;

            userParameterSelection.PeriodIds = periodIds;
            speciesFacts = SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(GetContext(), userParameterSelection);
            if (speciesFacts.IsNotEmpty())
            {
                Assert.IsTrue(speciesFactCount > speciesFacts.Count);
            }
            userParameterSelection.PeriodIds = null;

            userParameterSelection.HostIds = hostIds;
            speciesFacts = SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(GetContext(), userParameterSelection);
            if (speciesFacts.IsNotEmpty())
            {
                Assert.IsTrue(speciesFactCount > speciesFacts.Count);
            }
            userParameterSelection.HostIds = null;

            userParameterSelection.ReferenceIds = referenceIds;
            speciesFacts = SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(GetContext(), userParameterSelection);
            if (speciesFacts.IsNotEmpty())
            {
                Assert.IsTrue(speciesFactCount > speciesFacts.Count);
            }
            userParameterSelection.ReferenceIds = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void GetSpeciesFactsByUserParameterSelectionToManyError()
        {
            List<WebSpeciesFact> speciesFacts;
            WebUserParameterSelection userParameterSelection;

            userParameterSelection = new WebUserParameterSelection();
            speciesFacts = SpeciesFactManager.GetSpeciesFactsByUserParameterSelection(GetContext(), userParameterSelection);
            Assert.IsTrue(speciesFacts.IsNotEmpty());
        }

        private WebSpeciesFact GetSpeciesFact(WebSpeciesFact speciesFact)
        {
            List<Int32> speciesFactIds;

            speciesFactIds = new List<Int32>();
            speciesFactIds.Add(speciesFact.Id);
            return SpeciesFactManager.GetSpeciesFactsById(GetContext(), speciesFactIds)[0];
        }

        [TestMethod]
        public void GetTaxonCountyOccurrence()
        {
            Int32 taxonId;
            List<WebTaxonCountyOccurrence> countyOccurrencies;

            taxonId = BEAR_TAXON_ID;
            countyOccurrencies = SpeciesFactManager.GetTaxonCountyOccurence(GetContext(), taxonId);
            Assert.IsTrue(countyOccurrencies.IsNotEmpty());
            foreach (WebTaxonCountyOccurrence countyOccurrence in countyOccurrencies)
            {
                Assert.IsNotNull(countyOccurrence);
                Assert.AreEqual(taxonId, countyOccurrence.TaxonId);
            }
        }

        private WebUserParameterSelection GetUserParameterSelection()
        {
            WebUserParameterSelection userParameterSelection;

            userParameterSelection = new WebUserParameterSelection();
            userParameterSelection.FactorIds = new List<Int32>();
            userParameterSelection.FactorIds.Add(FOREST_LANSCAPE_FACTOR_ID);
            userParameterSelection.FactorIds.Add(LANDSCAPE_FOREST_FACTOR_ID);
            userParameterSelection.TaxonIds = new List<Int32>();
            userParameterSelection.TaxonIds.Add(BEAVER_TAXON_ID);
            userParameterSelection.TaxonIds.Add(BEAR_TAXON_ID);

            return userParameterSelection;
        }

        public static DataTable GetUserSelectedParameterTable(WebServiceContext context)
        {
            DataColumn column;
            DataRow row;
            DataTable userSelectedParameter;

            userSelectedParameter = new DataTable(UserSelectedParameterData.TABLE_NAME);
            column = new DataColumn(UserSelectedParameterData.REQUEST_ID, typeof(Int32));
            userSelectedParameter.Columns.Add(column);
            column = new DataColumn(UserSelectedParameterData.FACTOR_ID, typeof(Int32));
            userSelectedParameter.Columns.Add(column);
            column = new DataColumn(UserSelectedParameterData.HOST_ID, typeof(Int32));
            userSelectedParameter.Columns.Add(column);
            column = new DataColumn(UserSelectedParameterData.INDIVIDUAL_CATEGORY_ID, typeof(Int32));
            userSelectedParameter.Columns.Add(column);
            column = new DataColumn(UserSelectedParameterData.PERIOD_ID, typeof(Int32));
            userSelectedParameter.Columns.Add(column);
            column = new DataColumn(UserSelectedParameterData.REFERENCE_ID, typeof(Int32));
            userSelectedParameter.Columns.Add(column);
            column = new DataColumn(UserSelectedParameterData.TAXON_ID, typeof(Int32));
            userSelectedParameter.Columns.Add(column);
            row = userSelectedParameter.NewRow();
            row[0] = context.RequestId;
            row[1] = FOREST_LANSCAPE_FACTOR_ID;
            row[6] = BEAVER_TAXON_ID;
            userSelectedParameter.Rows.Add(row);
            row = userSelectedParameter.NewRow();
            row[0] = context.RequestId;
            row[1] = LANDSCAPE_FOREST_FACTOR_ID;
            row[6] = BEAR_TAXON_ID;
            userSelectedParameter.Rows.Add(row);
            return userSelectedParameter;
        }

        public static DataTable GetUserSelectedSpeciesFacts(WebServiceContext context)
        {
            DataColumn column;
            DataRow row;
            DataTable speciesFactTable;

            speciesFactTable = new DataTable(UserSelectedSpeciesFactData.TABLE_NAME);
            column = new DataColumn(UserSelectedSpeciesFactData.REQUEST_ID, typeof(Int32));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn(UserSelectedSpeciesFactData.SPECIES_FACT_ID, typeof(Int32));
            speciesFactTable.Columns.Add(column);
            column = new DataColumn(UserSelectedSpeciesFactData.SPECIES_FACT_USAGE, typeof(String));
            speciesFactTable.Columns.Add(column);
            row = speciesFactTable.NewRow();
            row[0] = context.RequestId;
            row[1] = 1;
            row[2] = UserSelectedSpeciesFactUsage.Input.ToString();
            speciesFactTable.Rows.Add(row);
            row = speciesFactTable.NewRow();
            row[0] = context.RequestId;
            row[1] = 2;
            row[2] = UserSelectedSpeciesFactUsage.Input.ToString();
            speciesFactTable.Rows.Add(row);
            return speciesFactTable;
        }

        [TestMethod]
        public void UpdateSpeciesFacts()
        {
            DateTime oldUpdateDate;
            Double doubleValue;
            Int32 oldReferenceId, oldQualityId;
            List<WebSpeciesFact> oldSpeciesFacts;
            List<WebSpeciesFact> speciesFacts;
            String oldUpdateUser;
            String stringValue;
            WebReference reference;
            WebSpeciesFact speciesFact;

            // Delete species facts.
            oldSpeciesFacts = GetSomeSpeciesFacts(GetContext());
            oldUpdateUser = oldSpeciesFacts[0].UpdateUserFullName;
            Assert.IsTrue(oldSpeciesFacts.IsNotEmpty());
            SpeciesFactManager.UpdateSpeciesFacts(GetContext(), null, oldSpeciesFacts, null);
            speciesFacts = GetSomeSpeciesFacts(GetContext());
            Assert.IsTrue(speciesFacts.IsEmpty());

            // Create species facts.
            SpeciesFactManager.UpdateSpeciesFacts(GetContext(), oldSpeciesFacts, null, null);
            speciesFacts = GetSomeSpeciesFacts(GetContext());
            Assert.IsTrue(speciesFacts.IsNotEmpty());
            Assert.AreEqual(oldSpeciesFacts.Count, speciesFacts.Count);

            // Test change of reference id.
            speciesFact = GetOneSpeciesFact(GetContext());
            oldReferenceId = speciesFact.ReferenceId;
            reference = ReferenceManager.GetReferences(GetContext())[3];
            speciesFact.ReferenceId = reference.Id;
            UpdateSpeciesFacts(speciesFact);
            speciesFact = GetSpeciesFact(speciesFact);
            Assert.AreNotEqual(oldReferenceId, speciesFact.ReferenceId);
            Assert.AreEqual(reference.Id, speciesFact.ReferenceId);

            // Test change of update date.
            oldUpdateDate = speciesFact.UpdateDate;
            Thread.Sleep(200);
            UpdateSpeciesFacts(speciesFact);
            speciesFact = GetSpeciesFact(speciesFact);
            Assert.IsTrue(oldUpdateDate <= speciesFact.UpdateDate);

            // Test change of update user.
            Assert.AreNotEqual(oldUpdateUser, speciesFact.UpdateUserFullName);

            // Test change of field values.
            doubleValue = 4324;
            speciesFact.FieldValue1 = doubleValue;
            speciesFact.IsFieldValue1Specified = true;
            UpdateSpeciesFacts(speciesFact);
            speciesFact = GetSpeciesFact(speciesFact);
            Assert.IsTrue(speciesFact.IsFieldValue1Specified);
            Assert.AreEqual(doubleValue, speciesFact.FieldValue1);

            doubleValue *= Math.PI;
            speciesFact.FieldValue2 = doubleValue;
            speciesFact.IsFieldValue2Specified = true;
            UpdateSpeciesFacts(speciesFact);
            speciesFact = GetSpeciesFact(speciesFact);
            Assert.IsTrue(speciesFact.IsFieldValue2Specified);
            Assert.IsTrue((doubleValue - speciesFact.FieldValue2) < 0.0001);

            doubleValue *= Math.PI;
            speciesFact.FieldValue3 = doubleValue;
            speciesFact.IsFieldValue3Specified = true;
            UpdateSpeciesFacts(speciesFact);
            speciesFact = GetSpeciesFact(speciesFact);
            Assert.IsTrue(speciesFact.IsFieldValue3Specified);
            Assert.IsTrue((doubleValue - speciesFact.FieldValue3) < 0.0001);

            stringValue = null;
            speciesFact.FieldValue4 = stringValue;
            speciesFact.IsFieldValue4Specified = false;
            UpdateSpeciesFacts(speciesFact);
            speciesFact = GetSpeciesFact(speciesFact);
            Assert.IsFalse(speciesFact.IsFieldValue4Specified);
            Assert.IsNull(speciesFact.FieldValue4);
            stringValue = "testing time";
            speciesFact.FieldValue4 = stringValue;
            speciesFact.IsFieldValue4Specified = true;
            UpdateSpeciesFacts(speciesFact);
            speciesFact = GetSpeciesFact(speciesFact);
            Assert.IsTrue(speciesFact.IsFieldValue4Specified);
            Assert.AreEqual(stringValue, speciesFact.FieldValue4);

            stringValue = null;
            speciesFact.FieldValue5 = stringValue;
            speciesFact.IsFieldValue5Specified = false;
            UpdateSpeciesFacts(speciesFact);
            speciesFact = GetSpeciesFact(speciesFact);
            Assert.IsFalse(speciesFact.IsFieldValue5Specified);
            Assert.IsNull(speciesFact.FieldValue5);
            stringValue = "testing time";
            speciesFact.FieldValue5 = stringValue;
            speciesFact.IsFieldValue5Specified = true;
            UpdateSpeciesFacts(speciesFact);
            speciesFact = GetSpeciesFact(speciesFact);
            Assert.IsTrue(speciesFact.IsFieldValue5Specified);
            Assert.AreEqual(stringValue, speciesFact.FieldValue5);

            // Test change of quality.
            oldQualityId = speciesFact.QualityId;
            speciesFact.QualityId += 1;
            UpdateSpeciesFacts(speciesFact);
            speciesFact = GetSpeciesFact(speciesFact);
            Assert.AreNotEqual(oldQualityId, speciesFact.QualityId);
            Assert.AreEqual(oldQualityId + 1, speciesFact.QualityId);
        }

        private void UpdateSpeciesFacts(WebSpeciesFact speciesFact)
        {
            List<WebSpeciesFact> speciesFacts;

            speciesFacts = new List<WebSpeciesFact>();
            speciesFacts.Add(speciesFact);
            SpeciesFactManager.UpdateSpeciesFacts(GetContext(), null, null, speciesFacts);
        }

        [TestMethod]
        public void UpdateDyntaxaSpeciesFacts()
        {
            DateTime oldUpdateDate;
            Double doubleValue;
            Int32 oldReferenceId, oldQualityId;
            List<WebSpeciesFact> oldSpeciesFacts;
            List<WebSpeciesFact> speciesFacts;
            String oldUpdateUser;
            String stringValue;
            WebReference reference;
            WebSpeciesFact speciesFact;

            // Delete species facts.
            oldSpeciesFacts = GetSomeSpeciesFacts(GetContext());
            oldUpdateUser = oldSpeciesFacts[0].UpdateUserFullName;
            Assert.IsTrue(oldSpeciesFacts.IsNotEmpty());
            SpeciesFactManager.UpdateDyntaxaSpeciesFacts(GetContext(), null, oldSpeciesFacts, null, "Dyntaxa Test");
            speciesFacts = GetSomeSpeciesFacts(GetContext());
            Assert.IsTrue(speciesFacts.IsEmpty());

            // Create species facts.
            SpeciesFactManager.UpdateDyntaxaSpeciesFacts(GetContext(), oldSpeciesFacts, null, null, "Dyntaxa Test");
            speciesFacts = GetSomeSpeciesFacts(GetContext());
            Assert.IsTrue(speciesFacts.IsNotEmpty());
            Assert.AreEqual(oldSpeciesFacts.Count, speciesFacts.Count);

            // Test change of reference id.
            speciesFact = GetOneSpeciesFact(GetContext());
            oldReferenceId = speciesFact.ReferenceId;
            reference = ReferenceManager.GetReferences(GetContext())[3];
            speciesFact.ReferenceId = reference.Id;
            UpdateDyntaxaSpeciesFacts(speciesFact);
            speciesFact = GetSpeciesFact(speciesFact);
            Assert.AreNotEqual(oldReferenceId, speciesFact.ReferenceId);
            Assert.AreEqual(reference.Id, speciesFact.ReferenceId);

            // Test change of update date.
            oldUpdateDate = speciesFact.UpdateDate;
            Thread.Sleep(200);
            UpdateDyntaxaSpeciesFacts(speciesFact);
            speciesFact = GetSpeciesFact(speciesFact);
            Assert.IsTrue(oldUpdateDate <= speciesFact.UpdateDate);

            // Test change of update user.
            Assert.AreNotEqual(oldUpdateUser, speciesFact.UpdateUserFullName);

            // Test change of field values.
            doubleValue = 4324;
            speciesFact.FieldValue1 = doubleValue;
            speciesFact.IsFieldValue1Specified = true;
            UpdateDyntaxaSpeciesFacts(speciesFact);
            speciesFact = GetSpeciesFact(speciesFact);
            Assert.IsTrue(speciesFact.IsFieldValue1Specified);
            Assert.AreEqual(doubleValue, speciesFact.FieldValue1);

            doubleValue *= Math.PI;
            speciesFact.FieldValue2 = doubleValue;
            speciesFact.IsFieldValue2Specified = true;
            UpdateDyntaxaSpeciesFacts(speciesFact);
            speciesFact = GetSpeciesFact(speciesFact);
            Assert.IsTrue(speciesFact.IsFieldValue2Specified);
            Assert.IsTrue((doubleValue - speciesFact.FieldValue2) < 0.0001);

            doubleValue *= Math.PI;
            speciesFact.FieldValue3 = doubleValue;
            speciesFact.IsFieldValue3Specified = true;
            UpdateDyntaxaSpeciesFacts(speciesFact);
            speciesFact = GetSpeciesFact(speciesFact);
            Assert.IsTrue(speciesFact.IsFieldValue3Specified);
            Assert.IsTrue((doubleValue - speciesFact.FieldValue3) < 0.0001);

            stringValue = null;
            speciesFact.FieldValue4 = stringValue;
            speciesFact.IsFieldValue4Specified = false;
            UpdateDyntaxaSpeciesFacts(speciesFact);
            speciesFact = GetSpeciesFact(speciesFact);
            Assert.IsFalse(speciesFact.IsFieldValue4Specified);
            Assert.IsNull(speciesFact.FieldValue4);
            stringValue = "testing time";
            speciesFact.FieldValue4 = stringValue;
            speciesFact.IsFieldValue4Specified = true;
            UpdateDyntaxaSpeciesFacts(speciesFact);
            speciesFact = GetSpeciesFact(speciesFact);
            Assert.IsTrue(speciesFact.IsFieldValue4Specified);
            Assert.AreEqual(stringValue, speciesFact.FieldValue4);

            stringValue = null;
            speciesFact.FieldValue5 = stringValue;
            speciesFact.IsFieldValue5Specified = false;
            UpdateDyntaxaSpeciesFacts(speciesFact);
            speciesFact = GetSpeciesFact(speciesFact);
            Assert.IsFalse(speciesFact.IsFieldValue5Specified);
            Assert.IsNull(speciesFact.FieldValue5);
            stringValue = "testing time";
            speciesFact.FieldValue5 = stringValue;
            speciesFact.IsFieldValue5Specified = true;
            UpdateDyntaxaSpeciesFacts(speciesFact);
            speciesFact = GetSpeciesFact(speciesFact);
            Assert.IsTrue(speciesFact.IsFieldValue5Specified);
            Assert.AreEqual(stringValue, speciesFact.FieldValue5);

            // Test change of quality.
            oldQualityId = speciesFact.QualityId;
            speciesFact.QualityId += 1;
            UpdateDyntaxaSpeciesFacts(speciesFact);
            speciesFact = GetSpeciesFact(speciesFact);
            Assert.AreNotEqual(oldQualityId, speciesFact.QualityId);
            Assert.AreEqual(oldQualityId + 1, speciesFact.QualityId);
        }

        private void UpdateDyntaxaSpeciesFacts(WebSpeciesFact speciesFact)
        {
            List<WebSpeciesFact> speciesFacts;

            speciesFacts = new List<WebSpeciesFact>();
            speciesFacts.Add(speciesFact);
            SpeciesFactManager.UpdateDyntaxaSpeciesFacts(GetContext(), null, null, speciesFacts, "Dyntaxa Kindvall");
        }
    }
}

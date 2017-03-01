using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;
using ArtDatabanken.WebService.ArtDatabankenService.Database;
using ArtDatabanken.WebService.ArtDatabankenService.Test.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Database
{
    /// <summary>
    /// Summary description for SqlSpeciesFactQueryBuilderTest
    /// </summary>
    [TestClass]
    public class SqlSpeciesFactQueryBuilderTest : TestBase
    {
        private SqlSpeciesFactQueryBuilder _queryBuilder;

        public SqlSpeciesFactQueryBuilderTest()
        {
            _queryBuilder = null;
        }

        private SqlSpeciesFactQueryBuilder GetQueryBuilder()
        {
            return GetQueryBuilder(false);
        }

        private SqlSpeciesFactQueryBuilder GetQueryBuilder(Boolean refresh)
        {
            if (_queryBuilder.IsNull() || refresh)
            {
                _queryBuilder = new SqlSpeciesFactQueryBuilder();
            }
            return _queryBuilder;
        }

        [TestMethod]
        public void GetQuery()
        {
            String query, expectedQuery;

            // Test one condition and one factor.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "WHERE " +
                             "(Data1.faktor = 1)";
            Assert.AreEqual(expectedQuery, query);

            // Test one condition and several factors.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            GetQueryBuilder().AddFactor(5);
            GetQueryBuilder().AddFactor(10);
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "WHERE " +
                             "(Data1.faktor IN (1, 5, 10))";
            Assert.AreEqual(expectedQuery, query);

            // Test several conditions and one factor.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.LeftBracket);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.And);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(5);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.And);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(10);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.RightBracket);
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "INNER JOIN af_data AS Data2 WITH (NOLOCK) ON Data1.taxon = Data2.taxon " +
                             "INNER JOIN af_data AS Data3 WITH (NOLOCK) ON Data1.taxon = Data3.taxon " +
                             "WHERE " +
                             "((Data1.faktor = 1) AND " +
                             "(Data2.faktor = 5) AND " +
                             "(Data3.faktor = 10))";
            Assert.AreEqual(expectedQuery, query);

            // Test several conditions and several factors.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.LeftBracket);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            GetQueryBuilder().AddFactor(65);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.And);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(5);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.And);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(10);
            GetQueryBuilder().AddFactor(2);
            GetQueryBuilder().AddFactor(777);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.RightBracket);
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "INNER JOIN af_data AS Data2 WITH (NOLOCK) ON Data1.taxon = Data2.taxon " +
                             "INNER JOIN af_data AS Data3 WITH (NOLOCK) ON Data1.taxon = Data3.taxon " +
                             "WHERE " +
                             "((Data1.faktor IN (1, 65)) AND " +
                             "(Data2.faktor = 5) AND " +
                             "(Data3.faktor IN (10, 2, 777)))";
            Assert.AreEqual(expectedQuery, query);

            // Test no species fact field condition.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "WHERE " +
                             "(Data1.faktor = 1)";
            Assert.AreEqual(expectedQuery, query);

            // Test one species fact field Boolean condition.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            GetQueryBuilder().AddSpeciesFactFieldCondition(GetSpeciesFactFieldCondition(true));
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "WHERE " +
                             "(Data1.faktor = 1 AND Data1.tal2 = 1)";
            Assert.AreEqual(expectedQuery, query);

            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            GetQueryBuilder().AddSpeciesFactFieldCondition(GetSpeciesFactFieldCondition(false));
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "WHERE " +
                             "(Data1.faktor = 1 AND Data1.tal2 = 0)";
            Assert.AreEqual(expectedQuery, query);

            // Test one species fact field Boolean condition and
            // one species fact field Int32 condition.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            GetQueryBuilder().AddSpeciesFactFieldCondition(GetSpeciesFactFieldCondition(true));
            GetQueryBuilder().AddSpeciesFactFieldCondition(GetSpeciesFactFieldCondition(42));
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "WHERE " +
                             "(Data1.faktor = 1 AND Data1.tal2 = 1 AND Data1.tal1 = 42)";
            Assert.AreEqual(expectedQuery, query);

            // Test some species fact field Boolean condition and
            // some species fact field Int32 condition.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            GetQueryBuilder().AddSpeciesFactFieldCondition(GetSpeciesFactFieldCondition(false));
            GetQueryBuilder().AddSpeciesFactFieldCondition(GetSpeciesFactFieldCondition(true));
            GetQueryBuilder().AddSpeciesFactFieldCondition(GetSpeciesFactFieldCondition(2));
            GetQueryBuilder().AddSpeciesFactFieldCondition(GetSpeciesFactFieldCondition(4));
            GetQueryBuilder().AddSpeciesFactFieldCondition(GetSpeciesFactFieldCondition(42));
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "WHERE " +
                             "(Data1.faktor = 1 AND Data1.tal2 IN (0, 1) AND Data1.tal1 IN (2, 4, 42))";
            Assert.AreEqual(expectedQuery, query);

            // Test one species fact field Int32 condition.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            GetQueryBuilder().AddSpeciesFactFieldCondition(GetSpeciesFactFieldCondition(-1));
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "WHERE " +
                             "(Data1.faktor = 1 AND Data1.tal1 = -1)";
            Assert.AreEqual(expectedQuery, query);

            // Test one species fact field String condition.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            GetQueryBuilder().AddSpeciesFactFieldCondition(GetSpeciesFactFieldCondition("A"));
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "WHERE " +
                             "(Data1.faktor = 1 AND Data1.text1 = ''A'')";
            Assert.AreEqual(expectedQuery, query);

            // Test several species fact field Int32 condition.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            GetQueryBuilder().AddSpeciesFactFieldCondition(GetSpeciesFactFieldCondition(-1));
            GetQueryBuilder().AddSpeciesFactFieldCondition(GetSpeciesFactFieldCondition(2));
            GetQueryBuilder().AddSpeciesFactFieldCondition(GetSpeciesFactFieldCondition(4));
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "WHERE " +
                             "(Data1.faktor = 1 AND Data1.tal1 IN (-1, 2, 4))";
            Assert.AreEqual(expectedQuery, query);

            // Test several species fact field String condition.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            GetQueryBuilder().AddSpeciesFactFieldCondition(GetSpeciesFactFieldCondition("A"));
            GetQueryBuilder().AddSpeciesFactFieldCondition(GetSpeciesFactFieldCondition("S"));
            GetQueryBuilder().AddSpeciesFactFieldCondition(GetSpeciesFactFieldCondition("U"));
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "WHERE " +
                             "(Data1.faktor = 1 AND Data1.text1 IN (''A'', ''S'', ''U''))";
            Assert.AreEqual(expectedQuery, query);

            // Test "AND" condition.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.LeftBracket);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.And);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(5);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.And);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(10);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.RightBracket);
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "INNER JOIN af_data AS Data2 WITH (NOLOCK) ON Data1.taxon = Data2.taxon " +
                             "INNER JOIN af_data AS Data3 WITH (NOLOCK) ON Data1.taxon = Data3.taxon " + 
                             "WHERE " +
                             "((Data1.faktor = 1) AND " +
                             "(Data2.faktor = 5) AND " +
                             "(Data3.faktor = 10))";
            Assert.AreEqual(expectedQuery, query);

            // Test "OR" condition.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.LeftBracket);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.Or);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(5);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.Or);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(10);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.RightBracket);
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "INNER JOIN af_data AS Data2 WITH (NOLOCK) ON Data1.taxon = Data2.taxon " +
                             "INNER JOIN af_data AS Data3 WITH (NOLOCK) ON Data1.taxon = Data3.taxon " +
                             "WHERE " +
                             "((Data1.faktor = 1) OR " +
                             "(Data2.faktor = 5) OR " +
                             "(Data3.faktor = 10))";
            Assert.AreEqual(expectedQuery, query);

            // Test "OR" inside "AND" condition.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.LeftBracket);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.And);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.LeftBracket);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(5);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.Or);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(7);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.RightBracket);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.And);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(10);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.RightBracket);
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "INNER JOIN af_data AS Data2 WITH (NOLOCK) ON Data1.taxon = Data2.taxon " +
                             "INNER JOIN af_data AS Data3 WITH (NOLOCK) ON Data1.taxon = Data3.taxon " +
                             "INNER JOIN af_data AS Data4 WITH (NOLOCK) ON Data1.taxon = Data4.taxon " +
                             "WHERE " +
                             "((Data1.faktor = 1) AND " +
                             "((Data2.faktor = 5) OR " +
                             "(Data3.faktor = 7)) AND " +
                             "(Data4.faktor = 10))";
            Assert.AreEqual(expectedQuery, query);

            // Test "AND" inside "OR" condition.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.LeftBracket);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.Or);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.LeftBracket);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(5);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.And);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(7);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.RightBracket);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.Or);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(10);
            GetQueryBuilder().AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.RightBracket);
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "INNER JOIN af_data AS Data2 WITH (NOLOCK) ON Data1.taxon = Data2.taxon " +
                             "INNER JOIN af_data AS Data3 WITH (NOLOCK) ON Data1.taxon = Data3.taxon " +
                             "INNER JOIN af_data AS Data4 WITH (NOLOCK) ON Data1.taxon = Data4.taxon " +
                             "WHERE " +
                             "((Data1.faktor = 1) OR " +
                             "((Data2.faktor = 5) AND " +
                             "(Data3.faktor = 7)) OR " +
                             "(Data4.faktor = 10))";
            Assert.AreEqual(expectedQuery, query);

            // Test no host condition.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "WHERE " +
                             "(Data1.faktor = 1)";
            Assert.AreEqual(expectedQuery, query);

            // Test one host condition.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            GetQueryBuilder().AddHost(2);
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "WHERE " +
                             "(Data1.faktor = 1 AND Data1.host = 2)";
            Assert.AreEqual(expectedQuery, query);

            // Test several host condition.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            GetQueryBuilder().AddHost(2);
            GetQueryBuilder().AddHost(3);
            GetQueryBuilder().AddHost(4);
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "WHERE " +
                             "(Data1.faktor = 1 AND Data1.host IN (2, 3, 4))";
            Assert.AreEqual(expectedQuery, query);

            // Test no individual category condition.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "WHERE " +
                             "(Data1.faktor = 1)";
            Assert.AreEqual(expectedQuery, query);

            // Test one individual category condition.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            GetQueryBuilder().AddIndividualCategory(2);
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "WHERE " +
                             "(Data1.faktor = 1 AND Data1.IndividualCategoryId = 2)";
            Assert.AreEqual(expectedQuery, query);

            // Test several individual category condition.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            GetQueryBuilder().AddIndividualCategory(2);
            GetQueryBuilder().AddIndividualCategory(3);
            GetQueryBuilder().AddIndividualCategory(4);
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "WHERE " +
                             "(Data1.faktor = 1 AND Data1.IndividualCategoryId IN (2, 3, 4))";
            Assert.AreEqual(expectedQuery, query);

            // Test no period condition.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "WHERE " +
                             "(Data1.faktor = 1)";
            Assert.AreEqual(expectedQuery, query);

            // Test one period condition.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            GetQueryBuilder().AddPeriod(0);
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "WHERE " +
                             "(Data1.faktor = 1 AND Data1.period = 0)";
            Assert.AreEqual(expectedQuery, query);

            // Test several period conditions.
            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            GetQueryBuilder().AddFactor(1);
            GetQueryBuilder().AddPeriod(0);
            GetQueryBuilder().AddPeriod(1);
            GetQueryBuilder().AddPeriod(2);
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            expectedQuery += "WHERE " +
                             "(Data1.faktor = 1 AND Data1.period IN (0, 1, 2))";
            Assert.AreEqual(expectedQuery, query);
 
            // This code is used to discover where two strings differ.
            for (Int32 index = 0; index < query.Length; index++)
            {
                if (expectedQuery[index] != query[index])
                {
                    break;
                }
            }
        }

        private WebSpeciesFactFieldCondition GetSpeciesFactFieldCondition(Boolean value)
        {
            WebSpeciesFactFieldCondition speciesFactFieldCondition;

            speciesFactFieldCondition = new WebSpeciesFactFieldCondition();
            speciesFactFieldCondition.FactorField = FactorManagerTest.GetOneFactorFieldBoolean(GetContext());
            speciesFactFieldCondition.Operator = DataConditionOperatorId.Equal;
            speciesFactFieldCondition.Value = value.WebToString();
            return speciesFactFieldCondition;
        }

        private WebSpeciesFactFieldCondition GetSpeciesFactFieldCondition(Int32 value)
        {
            WebSpeciesFactFieldCondition speciesFactFieldCondition;

            speciesFactFieldCondition = new WebSpeciesFactFieldCondition();
            speciesFactFieldCondition.FactorField = FactorManagerTest.GetOneFactorFieldInt32(GetContext());
            speciesFactFieldCondition.Operator = DataConditionOperatorId.Equal;
            speciesFactFieldCondition.Value = value.WebToString();
            return speciesFactFieldCondition;
        }

        private WebSpeciesFactFieldCondition GetSpeciesFactFieldCondition(String value)
        {
            WebSpeciesFactFieldCondition speciesFactFieldCondition;

            speciesFactFieldCondition = new WebSpeciesFactFieldCondition();
            speciesFactFieldCondition.FactorField = FactorManagerTest.GetOneFactorFieldString(GetContext());
            speciesFactFieldCondition.IsEnumAsString = true;
            speciesFactFieldCondition.Operator = DataConditionOperatorId.Equal;
            speciesFactFieldCondition.Value = value;
            return speciesFactFieldCondition;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetQueryNoConditionError()
        {
            String query, expectedQuery;

            query = GetQueryBuilder(true).GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            Assert.AreEqual(expectedQuery, query);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetQueryNoFactorError()
        {
            String query, expectedQuery;

            GetQueryBuilder(true).AddQueryCondition(SqlSpeciesFactQueryBuilder.ConditionType.SpeciesFactCondition);
            query = GetQueryBuilder().GetQuery(GetContext());
            expectedQuery = GetQueryStart();
            Assert.AreEqual(expectedQuery, query);
        }

        private String GetQueryStart()
        {
            return " SELECT DISTINCT " +
                   GetContext().RequestId + " AS RequestId, " +
                   "Data1.taxon AS TaxonId, " +
                   "''Output'' AS TaxonUsage " +
                   "FROM af_data AS Data1 WITH (NOLOCK) ";
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;
using ArtDatabanken.WebService.ArtDatabankenService.Database;
using ArtDatabanken.WebService.ArtDatabankenService.Test.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Database
{
    [TestClass]
    public class DataReaderTest : TestBase
    {
        [TestMethod]
        public void Close()
        {
            using (DataReader dataReader = DataServer.GetDatabaseUpdate(GetContext()))
            {
                dataReader.Close();

                // Should be ok to close an already closed DataReader.
                dataReader.Close();
            }
        }

        [TestMethod]
        public void ColumnNamePrefix()
        {
            String columnNamePrefix;

            using (DataReader dataReader = DataServer.GetFactors(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());

                columnNamePrefix = null;
                dataReader.ColumnNamePrefix = columnNamePrefix;
                Assert.IsNull(dataReader.ColumnNamePrefix);

                columnNamePrefix = "";
                dataReader.ColumnNamePrefix = columnNamePrefix;
                Assert.AreEqual(columnNamePrefix, dataReader.ColumnNamePrefix);

                columnNamePrefix = "ColumnNamePrefix";
                dataReader.ColumnNamePrefix = columnNamePrefix;
                Assert.AreEqual(columnNamePrefix, dataReader.ColumnNamePrefix);
            }
        }

        [TestMethod]
        public void Constructor()
        {
            using (DataReader dataReader = DataServer.GetDatabaseUpdate(GetContext()))
            {
                Assert.IsNotNull(dataReader);
            }
        }

        [TestMethod]
        public void Dispose()
        {
            using (DataReader dataReader = DataServer.GetDatabaseUpdate(GetContext()))
            {
                dataReader.Dispose();

                // Should be ok to dispose an already disposed DataReader.
                dataReader.Dispose();
            }
        }

        [TestMethod]
        public void GetBoolean()
        {
            Boolean value, defaultValue;

            defaultValue = true;
            using (DataReader dataReader = DataServer.GetFactors(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
                value = dataReader.GetBoolean(FactorData.IS_PUBLIC, defaultValue);
                Assert.IsTrue(value);

                Assert.IsTrue(dataReader.Read());
                if (dataReader.IsDBNull(FactorData.IS_PERIODIC))
                {
                    value = dataReader.GetBoolean(FactorData.IS_PERIODIC, defaultValue);
                    Assert.AreEqual(value, defaultValue);
                }
                else
                {
                    value = dataReader.GetBoolean(FactorData.IS_PERIODIC);
                    Assert.AreNotEqual(value, defaultValue);
                    value = dataReader.GetBoolean(FactorData.IS_PERIODIC, defaultValue);
                    Assert.AreNotEqual(value, defaultValue);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetBooleanColumnNameError()
        {
            using (DataReader dataReader = DataServer.GetFactors(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
                dataReader.GetBoolean("No column name");
            }
        }

        [TestMethod]
        public void GetDateTime()
        {
            DateTime value, defaultValue;

            defaultValue = DateTime.Now;
            using (DataReader dataReader = DataServer.GetDatabaseUpdate(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
                value = dataReader.GetDateTime(DatabaseData.UPDATE_START);
                Assert.AreNotEqual(defaultValue, value);

                if (dataReader.IsDBNull(DatabaseData.UPDATE_END))
                {
                    value = dataReader.GetDateTime(DatabaseData.UPDATE_END, defaultValue);
                    Assert.AreEqual(value, defaultValue);
                }
                else
                {
                    value = dataReader.GetDateTime(DatabaseData.UPDATE_END);
                    Assert.AreNotEqual(value, defaultValue);
                    value = dataReader.GetDateTime(DatabaseData.UPDATE_END, defaultValue);
                    Assert.AreNotEqual(value, defaultValue);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetDateTimeColumnNameError()
        {
            using (DataReader dataReader = DataServer.GetDatabaseUpdate(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
                dataReader.GetDateTime("No column name");
            }
        }

        [TestMethod]
        public void GetDouble()
        {
            Double value, defaultValue;

            defaultValue = 53545.543;
            try
            {
                SpeciesFactManager.AddUserSelectedSpeciesFacts(GetContext(), SpeciesFactManagerTest.GetSomeSpeciesFactIds(GetContext()), UserSelectedSpeciesFactUsage.Output);
                using (DataReader dataReader = DataServer.GetSpeciesFactsById(GetContext()))
                {
                    while(dataReader.Read())
                    {
                        if (dataReader.IsDBNull(SpeciesFactData.FIELD_VALUE_1))
                        {
                            value = dataReader.GetDouble(SpeciesFactData.FIELD_VALUE_1, defaultValue);
                            Assert.AreEqual(value, defaultValue);
                        }
                        else
                        {
                            value = dataReader.GetDouble(SpeciesFactData.FIELD_VALUE_1);
                            Assert.AreNotEqual(value, defaultValue);
                            value = dataReader.GetDouble(SpeciesFactData.FIELD_VALUE_1, defaultValue);
                            Assert.AreNotEqual(value, defaultValue);
                        }
                    }
                }
            }
            finally
            {
                SpeciesFactManager.DeleteUserSelectedSpeciesFacts(GetContext());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetDoubleColumnNameError()
        {
            try
            {
                SpeciesFactManager.AddUserSelectedSpeciesFacts(GetContext(), SpeciesFactManagerTest.GetSomeSpeciesFactIds(GetContext()), UserSelectedSpeciesFactUsage.Output);
                using (DataReader dataReader = DataServer.GetSpeciesFactsById(GetContext()))
                {
                    Assert.IsTrue(dataReader.Read());
                    dataReader.GetDouble("No column name");
                }
            }
            finally
            {
                SpeciesFactManager.DeleteUserSelectedSpeciesFacts(GetContext());
            }
        }

        [TestMethod]
        public void GetInt32()
        {
            Int32 value, defaultValue;

            defaultValue = Int32.MinValue;
            using (DataReader dataReader = DataServer.GetDatabases(GetContext()))
            {
                while (dataReader.Read())
                {
                    if (dataReader.IsDBNull(DatabaseData.ID))
                    {
                        value = dataReader.GetInt32(DatabaseData.ID, defaultValue);
                        Assert.AreEqual(value, defaultValue);
                    }
                    else
                    {
                        value = dataReader.GetInt32(DatabaseData.ID);
                        Assert.AreNotEqual(value, defaultValue);
                        value = dataReader.GetInt32(DatabaseData.ID, defaultValue);
                        Assert.AreNotEqual(value, defaultValue);
                    }
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetInt32ColumnNameError()
        {
            using (DataReader dataReader = DataServer.GetDatabases(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
                dataReader.GetInt32("No column name");
            }
        }

        [TestMethod]
        public void GetInt64()
        {
            Int64 value, defaultValue;

            defaultValue = Int64.MinValue;
            DataServer.AddUserSelectedTaxa(GetContext(), TaxonManagerTest.GetUserSelectedTaxa(GetContext()));
            DataServer.UpdateUserSelecedTaxa(GetContext());
            using (DataReader dataReader = DataServer.GetSpeciesObservations(GetContext(),
                                                                             0,
                                                                             5,
                                                                             false,
                                                                             0,
                                                                             false,
                                                                             0,
                                                                             0,
                                                                             false,
                                                                             true,
                                                                             0,
                                                                             false,
                                                                             DateTime.Now,
                                                                             DateTime.Now,
                                                                             0,
                                                                             false,
                                                                             DateTime.Now,
                                                                             DateTime.Now,
                                                                             false,
                                                                             Int32.MinValue,
                                                                             Int32.MinValue,
                                                                             Int32.MinValue,
                                                                             Int32.MinValue,
                                                                             false,
                                                                             Int32.MinValue,
                                                                             null,
                                                                             true,
                                                                             false,
                                                                             false,
                                                                             false,
                                                                             -1,
                                                                             false,
                                                                             -1,
                                                                             false,
                                                                             -1,
                                                                             false,
                                                                             -1,
                                                                             false,
                                                                             -1,
                                                                             false,
                                                                             -1,
                                                                             "All",
                                                                             null,
                                                                             true))
            {
                while (dataReader.Read())
                {
                    if (dataReader.IsDBNull(SpeciesObservationData.ID))
                    {
                        value = dataReader.GetInt64(SpeciesObservationData.ID, defaultValue);
                        Assert.AreEqual(value, defaultValue);
                    }
                    else
                    {
                        value = dataReader.GetInt64(SpeciesObservationData.ID);
                        Assert.AreNotEqual(value, defaultValue);
                        value = dataReader.GetInt64(SpeciesObservationData.ID, defaultValue);
                        Assert.AreNotEqual(value, defaultValue);
                    }
                }
            }
            DataServer.DeleteUserSelectedTaxa(GetContext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetInt64ColumnNameError()
        {
            using (DataReader dataReader = DataServer.GetDatabases(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
                dataReader.GetInt64("No column name");
            }
        }

        [TestMethod]
        public void GetString()
        {
            String value;

            using (DataReader dataReader = DataServer.GetDatabases(GetContext()))
            {
                while (dataReader.Read())
                {
                    if (dataReader.IsDBNull(DatabaseData.LONG_NAME))
                    {
                        value = dataReader.GetString(DatabaseData.LONG_NAME);
                        Assert.IsNull(value);
                    }
                    else
                    {
                        value = dataReader.GetString(DatabaseData.LONG_NAME);
                        Assert.IsTrue(value.IsNotEmpty());
                    }
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetStringColumnNameError()
        {
            using (DataReader dataReader = DataServer.GetDatabases(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
                dataReader.GetString("No column name");
            }
        }

        [TestMethod]
        public void GetUnreadColumnName()
        {
            using (DataReader dataReader = DataServer.GetDatabases(GetContext()))
            {
                while (dataReader.Read())
                {
                    while (dataReader.NextUnreadColumn())
                    {
                        Assert.IsTrue(dataReader.GetUnreadColumnName().IsNotEmpty());
                    }
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void GetUnreadColumnNameWorkflowError()
        {
            using (DataReader dataReader = DataServer.GetDatabases(GetContext()))
            {
                while (dataReader.Read())
                {
                    Assert.IsTrue(dataReader.GetUnreadColumnName().IsNotEmpty());
                }
            }
        }

        [TestMethod]
        public void GetUnreadColumnType()
        {
            using (DataReader dataReader = DataServer.GetDatabases(GetContext()))
            {
                while (dataReader.Read())
                {
                    while (dataReader.NextUnreadColumn())
                    {
                        Assert.IsTrue(dataReader.GetUnreadColumnType().IsNotNull());
                    }
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void GetUnreadColumnTypeWorkflowError()
        {
            using (DataReader dataReader = DataServer.GetDatabases(GetContext()))
            {
                while (dataReader.Read())
                {
                    Assert.IsTrue(dataReader.GetUnreadColumnType().IsNotNull());
                }
            }
        }

        [TestMethod]
        public void HasColumn()
        {
            using (DataReader dataReader = DataServer.GetDatabases(GetContext()))
            {
                while (dataReader.Read())
                {
                    while (dataReader.NextUnreadColumn())
                    {
                        Assert.IsTrue(dataReader.HasColumn(dataReader.GetUnreadColumnName()));
                    }
                    Assert.IsFalse(dataReader.HasColumn("No column name"));
                }
            }
        }

        [TestMethod]
        public void IsDBNull()
        {
            using (DataReader dataReader = DataServer.GetDatabases(GetContext()))
            {
                while (dataReader.Read())
                {
                    while (dataReader.NextUnreadColumn())
                    {
                        Assert.IsFalse(dataReader.IsDBNull(dataReader.GetUnreadColumnName()));
                    }
                }
            }
        }

        [TestMethod]
        public void IsNotDBNull()
        {
            using (DataReader dataReader = DataServer.GetDatabases(GetContext()))
            {
                while (dataReader.Read())
                {
                    while (dataReader.NextUnreadColumn())
                    {
                        Assert.IsTrue(dataReader.IsNotDBNull(dataReader.GetUnreadColumnName()));
                    }
                }
            }
        }

        [TestMethod]
        public void NextResultSet()
        {
            using (DataReader dataReader = DataServer.GetDatabases(GetContext()))
            {
                Assert.IsFalse(dataReader.NextResultSet());
            }
        }

        [TestMethod]
        public void NextUnreadColumn()
        {
            using (DataReader dataReader = DataServer.GetDatabases(GetContext()))
            {
                while (dataReader.Read())
                {
                    while (dataReader.NextUnreadColumn())
                    {
                        Assert.IsTrue(dataReader.GetUnreadColumnName().IsNotEmpty());
                        Assert.IsTrue(dataReader.GetUnreadColumnType().IsNotNull());
                    }
                }

                // Should be ok to try to get next column
                // even if there are no more data.
                Assert.IsFalse(dataReader.NextUnreadColumn());

                // Should be ok to try to get next column
                // even if dataReader is closed.
                dataReader.Close();
                Assert.IsFalse(dataReader.NextUnreadColumn());
            }
        }

        [TestMethod]
        public void Read()
        {
            using (DataReader dataReader = DataServer.GetDatabases(GetContext()))
            {
                while (dataReader.Read())
                {
                    while (dataReader.NextUnreadColumn())
                    {
                        Assert.IsTrue(dataReader.GetUnreadColumnName().IsNotEmpty());
                        Assert.IsTrue(dataReader.GetUnreadColumnType().IsNotNull());
                    }
                }

                // Should be ok to try to read even if there are no more data.
                Assert.IsFalse(dataReader.Read());

                // Should be ok to try to read even if dataReader is closed.
                dataReader.Close();
                Assert.IsFalse(dataReader.Read());
            }
        }
    }
}

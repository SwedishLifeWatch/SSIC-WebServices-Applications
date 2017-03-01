using System;
using System.Diagnostics;
using System.Globalization;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Database;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.Database
{
    [TestClass]
    public class SpeciesObservationHarvestServerTest
    {
        [TestMethod]
        public void Constructor()
        {
            using (SpeciesObservationHarvestServer database = new SpeciesObservationHarvestServer())
            {
                Assert.IsNotNull(database);
            }
        }

        [TestMethod]
        public void DeleteUnnecessaryChanges()
        {
            using (SpeciesObservationHarvestServer database = new SpeciesObservationHarvestServer())
            {
                Assert.IsNotNull(database);
                database.DeleteUnnecessaryChanges();
            }
        }

        [TestMethod]
        public void EmptyTempElasticsearchTables()
        {
            using (SpeciesObservationHarvestServer database = new SpeciesObservationHarvestServer())
            {
                Assert.IsNotNull(database);
                database.EmptyTempElasticsearchTables();
            }
        }

        [TestMethod]
        public void GetAddress()
        {
            String address = HarvestBaseServer.GetAddress();
            Assert.IsTrue(address.IsNotEmpty());
        }

        [TestMethod]
        public void GetCountyFromCoordinates()
        {
            double coordinateX, coordinateY;
            String county;

            coordinateX = 1986116;
            coordinateY = 8146315;
            using (SpeciesObservationHarvestServer database = new SpeciesObservationHarvestServer())
            {
                county = database.GetCountyFromCoordinates(coordinateX, coordinateY);
                Assert.IsTrue(county.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetLatestLog()
        {
            using (SpeciesObservationHarvestServer database = new SpeciesObservationHarvestServer())
            {
                using (DataReader reader = database.GetLatestLog())
                {
                    while (reader.Read())
                    {
                        Debug.WriteLine(reader.GetDateTime(0) + " - " + reader.GetString(1));
                    }
                }
            }
        }

        [TestMethod]
        public void GetLogStatisticsLog_ExpectsMaxTenRows()
        {
            using (SpeciesObservationHarvestServer database = new SpeciesObservationHarvestServer())
            {
                using (DataReader reader = database.GetLogStatistics(10))
                {
                    while (reader.Read())
                    {
                        Debug.WriteLine(reader.GetDateTime(0) + " - " + reader.GetString(1) + " - " + reader.GetString(3));
                    }
                }
            }
        }

        [TestMethod]
        public void GetLogUpdateErrorLog_ExpectsMaxTenRows()
        {
            using (SpeciesObservationHarvestServer database = new SpeciesObservationHarvestServer())
            {
                using (DataReader reader = database.GetLogUpdateError(10))
                {
                    while (reader.Read())
                    {
                        Debug.WriteLine(reader.GetDateTime(14) + " - " + reader.GetString(0) + " - " + reader.GetInt32(10) + " - " + reader.GetString(12));
                    }
                }
            }
        }

        [TestMethod]
        public void GetMunicipalityFromCoordinates()
        {
            double coordinateX, coordinateY;
            String municipality;

            coordinateX = 1986116;
            coordinateY = 8146315;
            using (SpeciesObservationHarvestServer database = new SpeciesObservationHarvestServer())
            {
                municipality = database.GetMunicipalityFromCoordinates(coordinateX, coordinateY);
                Assert.IsTrue(municipality.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetNextChangeId()
        {
            Int64 currentChangeId, nextChangeId;

            using (SpeciesObservationHarvestServer database = new SpeciesObservationHarvestServer())
            {
                currentChangeId = 42;
                nextChangeId = database.GetNextChangeId(currentChangeId, null);
                Assert.IsTrue(currentChangeId < nextChangeId);
            }
        }

        [TestMethod]
        public void GetParishFromCoordinates()
        {
            double coordinateX, coordinateY;
            String parish;

            coordinateX = 1986116;
            coordinateY = 8146315;
            using (SpeciesObservationHarvestServer database = new SpeciesObservationHarvestServer())
            {
                parish = database.GetParishFromCoordinates(coordinateX, coordinateY);
                Assert.IsTrue(parish.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetSpeciesObservationChangesElasticsearch()
        {
            Int64 changeId = 77653355;

            using (SpeciesObservationHarvestServer database = new SpeciesObservationHarvestServer())
            {
                using (DataReader reader = database.GetSpeciesObservationChangesElasticsearch(changeId, 1000, null))
                {
                    // Get DarwinCore information.
                    Assert.IsTrue(reader.Read());
                    while (reader.Read())
                    {
                    }

                    // Get project parameters.
                    Assert.IsTrue(reader.NextResultSet());
                    Assert.IsTrue(reader.Read());
                    while (reader.Read())
                    {
                    }

                    // Get deleted species observations.
                    Assert.IsTrue(reader.NextResultSet());
                    Assert.IsFalse(reader.Read());
                    while (reader.Read())
                    {
                    }

                    // Get max change id.
                    Assert.IsTrue(reader.NextResultSet());
                    Assert.IsTrue(reader.Read());
                }
            }
        }

        [TestMethod]
        public void GetStateProvinceFromCoordinates()
        {
            double coordinateX, coordinateY;
            String stateProvince;

            coordinateX = 1986116;
            coordinateY = 8146315;
            using (SpeciesObservationHarvestServer database = new SpeciesObservationHarvestServer())
            {
                stateProvince = database.GetStateProvinceFromCoordinates(coordinateX, coordinateY);
                Assert.IsTrue(stateProvince.IsNotEmpty());
            }
        }

        /// <summary>
        /// asserts that the weeknumbergeneration on the server side is the same as in the code base
        /// </summary>
        [TestMethod]
        public void GetWeekOfYear()
        {
            for (int i = 0; i < 10000; i++)
            {
                var date = new DateTime(2000, 1, 1).AddDays(i);
                using (SpeciesObservationHarvestServer database = new SpeciesObservationHarvestServer())
                {
                    using (var reader = database.GetWeekOfYear(date))
                    {
                        if (reader.Read())
                        {
                            var weeknumber = reader.GetInt32(0);
                            var otherWeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
                            Debug.WriteLine(string.Format("{0} - {1} SQL {2} code {3}", date, date.DayOfWeek, weeknumber, otherWeekNumber));
                            Assert.IsTrue(weeknumber == otherWeekNumber);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void Ping()
        {
            using (WebServiceDataServer database = new SpeciesObservationHarvestServer())
            {
                Assert.IsTrue(database.Ping());
            }
        }

        [TestMethod]
        public void AddSpeciesObservationChangeForElasticSearchServerSide()
        {
            using (var database = new SpeciesObservationHarvestServer())
            {
                Assert.IsNotNull(database);//?
                database.AddSpeciesObservationChangeForElasticSearch(100000, true);
            }
        }

        [TestMethod]
        public void AddSpeciesObservationChangeForElasticSearchClientSide()
        {
            using (var database = new SpeciesObservationHarvestServer())
            {
                Assert.IsNotNull(database);//?
                database.AddSpeciesObservationChangeForElasticSearch(100000, false);
            }
        }

        [TestMethod]
        public void GetSpeciesObservationElasticsearch()
        {
            using (SpeciesObservationHarvestServer database = new SpeciesObservationHarvestServer())
            {
                using (DataReader dataReader = database.GetSpeciesObservationElasticsearch())
                {
                    Assert.IsTrue(dataReader.Read());
                }
            }
        }
    }
}

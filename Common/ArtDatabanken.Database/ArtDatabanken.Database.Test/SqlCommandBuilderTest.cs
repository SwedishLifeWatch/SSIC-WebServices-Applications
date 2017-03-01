using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotUsedCommandBuilder = System.Data.SqlClient.SqlCommandBuilder;

namespace ArtDatabanken.Database.Test
{
    [TestClass]
    public class SqlCommandBuilderTest
    {
        [TestMethod]
        public void AddParameterBoolean()
        {
            SqlCommandBuilder command;

            // Test Boolean argument only.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("RowCount", false);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @RowCount = 0");

            // Test Boolean argument last in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("WebServiceUser", "%DEV%");
            command.AddParameter("SqlServerUser", true);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @WebServiceUser = N'%DEV%', @SqlServerUser = 1");

            // Test Boolean argument first in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("SqlServerUser", false);
            command.AddParameter("WebServiceUser", "%Hej%");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @SqlServerUser = 0, @WebServiceUser = N'%Hej%'");

            // Test Boolean argument inside argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("TcpIp", "gfdgsdgsg%");
            command.AddParameter("WebServiceUser", true);
            command.AddParameter("SqlServerUser", 123654);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @TcpIp = N'gfdgsdgsg%', @WebServiceUser = 1, @SqlServerUser = 123654");
        }

        [TestMethod]
        public void AddParameterDateTime()
        {
            SqlCommandBuilder command;

            // Test DateTime argument only.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("Time", new DateTime(2008, 11, 11, 15, 12, 14));
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @Time = '2008-11-11 15:12:14.000'");

            // Test DateTime argument last in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("RowCount", 52365);
            command.AddParameter("Time", new DateTime(2008, 11, 14, 20, 10, 14));
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @RowCount = 52365, @Time = '2008-11-14 20:10:14.000'");

            // Test DateTime argument first in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("Time", new DateTime(2008, 11, 14, 20, 10, 33));
            command.AddParameter("RowCount", 52300);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @Time = '2008-11-14 20:10:33.000', @RowCount = 52300");

            // Test DateTime argument inside argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("RowCount", 52);
            command.AddParameter("Time", new DateTime(1888, 11, 14, 20, 10, 33));
            command.AddParameter("SqlServerUser", "En vanlig kommentar");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @RowCount = 52, @Time = '1888-11-14 20:10:33.000', @SqlServerUser = N'En vanlig kommentar'");
        }

        [TestMethod]
        public void AddParameterDouble()
        {
            SqlCommandBuilder command;

            // Test Double argument only.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("RowCount", 32.43);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @RowCount = 32.43");

            // Test Double argument last in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("RowCount", 52365);
            command.AddParameter("TcpIp", 100.0);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @RowCount = 52365, @TcpIp = 100");

            // Test Double argument first in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("TcpIp", 100.3);
            command.AddParameter("RowCount", 52300);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @TcpIp = 100.3, @RowCount = 52300");

            // Test Double argument inside argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("TcpIp", 52);
            command.AddParameter("RowCount", 222.222);
            command.AddParameter("SqlServerUser", "En vanlig kommentar");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @TcpIp = 52, @RowCount = 222.222, @SqlServerUser = N'En vanlig kommentar'");
        }

        [TestMethod]
        public void AddParameterInt32()
        {
            SqlCommandBuilder command;

            // Test Int32 argument only.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("RowCount", 32);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @RowCount = 32");

            // Test Int32 argument last in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("SqlServerUser", "lklgkj gsldfkg slk");
            command.AddParameter("RowCount", 42);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @SqlServerUser = N'lklgkj gsldfkg slk', @RowCount = 42");

            // Test Int32 argument first in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("RowCount", 101);
            command.AddParameter("SqlServerUser", "lklgkj gsldfkg slk");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @RowCount = 101, @SqlServerUser = N'lklgkj gsldfkg slk'");

            // Test Int32 argument inside argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("TcpIp", true);
            command.AddParameter("RowCount", 10134);
            command.AddParameter("SqlServerUser", "lklgkj gsldfkg slk");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @TcpIp = 1, @RowCount = 10134, @SqlServerUser = N'lklgkj gsldfkg slk'");
        }

        [TestMethod]
        public void AddParameterInt64()
        {
            SqlCommandBuilder command;

            // Test Int64 argument only.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("RowCount", 32123456789123);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @RowCount = 32123456789123");

            // Test Int64 argument last in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("SqlServerUser", "lklgkj gsldfkg slk");
            command.AddParameter("RowCount", 32123456789123);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @SqlServerUser = N'lklgkj gsldfkg slk', @RowCount = 32123456789123");

            // Test Int64 argument first in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("RowCount", 32123456789123);
            command.AddParameter("SqlServerUser", "lklgkj gsldfkg slk");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @RowCount = 32123456789123, @SqlServerUser = N'lklgkj gsldfkg slk'");

            // Test Int64 argument inside argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("TcpIp", true);
            command.AddParameter("RowCount", 32123456789125);
            command.AddParameter("SqlServerUser", "lklgkj gsldfkg slk");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @TcpIp = 1, @RowCount = 32123456789125, @SqlServerUser = N'lklgkj gsldfkg slk'");
        }

        [TestMethod]
        public void AddParameterNullableInt64()
        {
            SqlCommandBuilder command;

            // Test null Int64 argument only.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("RowCount", (Int64?)null);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @RowCount = NULL");

            // Test Int64 argument only.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("RowCount", (Int64?)32123456789123);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @RowCount = 32123456789123");

            // Test Int64 argument last in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("SqlServerUser", "lklgkj gsldfkg slk");
            command.AddParameter("RowCount", (Int64?)32123456789123);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @SqlServerUser = N'lklgkj gsldfkg slk', @RowCount = 32123456789123");

            // Test Int64 argument first in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("RowCount", (Int64?)32123456789123);
            command.AddParameter("SqlServerUser", "lklgkj gsldfkg slk");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @RowCount = 32123456789123, @SqlServerUser = N'lklgkj gsldfkg slk'");

            // Test Int64 argument inside argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("TcpIp", true);
            command.AddParameter("RowCount", (Int64?)32123456789125);
            command.AddParameter("SqlServerUser", "lklgkj gsldfkg slk");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @TcpIp = 1, @RowCount = 32123456789125, @SqlServerUser = N'lklgkj gsldfkg slk'");
        }

        [TestMethod]
        public void AddParameterString()
        {
            SqlCommandBuilder command;

            // Test String argument only.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("SqlServerUser", "Konstigt taxon det här");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @SqlServerUser = N'Konstigt taxon det här'");

            // Test String argument last in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("RowCount", 234);
            command.AddParameter("SqlServerUser", "Konstigt taxon det här");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @RowCount = 234, @SqlServerUser = N'Konstigt taxon det här'");

            // Test String argument first in argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("SqlServerUser", "Konstigt taxon det här");
            command.AddParameter("RowCount", 555234);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @SqlServerUser = N'Konstigt taxon det här', @RowCount = 555234");

            // Test String argument inside argument list.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("RowCount", 234);
            command.AddParameter("SqlServerUser", "Konstigt taxon det här");
            command.AddParameter("TcpIp", false);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @RowCount = 234, @SqlServerUser = N'Konstigt taxon det här', @TcpIp = 0");

            // Test String argument with ' and parameter as text.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("TaxonName", "Foo''Bar");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @TaxonName = N'Foo''Bar'");

            // Test String argument with ' and parameter as text and dynamic SQL is used in stored procedure.
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("TaxonName", "Foo''Bar", 1);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @TaxonName = N'Foo''Bar'");
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("TaxonName", "Foo''Bar", 2);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @TaxonName = N'Foo''''Bar'");
            command = new SqlCommandBuilder("GetTaxon");
            command.AddParameter("TaxonName", "Foo''Bar", 3);
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon @TaxonName = N'Foo''''''''Bar'");
        }

        [TestMethod]
        public void Constructor()
        {
            SqlCommandBuilder command;

            command = new SqlCommandBuilder("GetTaxon");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxon");
        }

        [TestMethod]
        public void GetCommand()
        {
            SqlCommandBuilder command;

            command = new SqlCommandBuilder("GetTaxonTypes");
            Assert.AreEqual(command.GetCommand(), "EXECUTE GetTaxonTypes");
        }
    }
}

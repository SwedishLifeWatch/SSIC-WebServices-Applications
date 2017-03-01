using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.Data.Test
{
    [TestClass]
    public class DataSourceInformationTest
    {
        DataSourceInformation _dataSourceInformation;

        public DataSourceInformationTest()
        {
            _dataSourceInformation = null;
        }

        [TestMethod]
        public void Address()
        {
            Assert.IsTrue(GetDataSourceInformation(true).Address.IsNotEmpty());
        }

        [TestMethod]
        public void Constructor()
        {
            DataSourceInformation dataSource;

            dataSource = new DataSourceInformation(Settings.Default.UserServiceName,
                                                   Settings.Default.UserServiceMonesesFastAddress,
                                                   DataSourceType.WebService);
            Assert.IsNotNull(dataSource);
        }

        private DataSourceInformation GetDataSourceInformation()
        {
            return GetDataSourceInformation(false);
        }

        private DataSourceInformation GetDataSourceInformation(Boolean refresh)
        {
            if (_dataSourceInformation.IsNull() || refresh)
            {
                _dataSourceInformation = new DataSourceInformation(Settings.Default.UserServiceName,
                                                                   Settings.Default.UserServiceMonesesFastAddress,
                                                                   DataSourceType.WebService);
            }
            return _dataSourceInformation;
        }

        public static DataSourceInformation GetOneDataSourceInformation()
        {
            return new DataSourceInformation(Settings.Default.UserServiceName,
                                             Settings.Default.UserServiceMonesesFastAddress,
                                             DataSourceType.WebService);
        }

        [TestMethod]
        public void Name()
        {
            Assert.IsTrue(GetDataSourceInformation(true).Name.IsNotEmpty());
        }

        [TestMethod]
        public void Type()
        {
            Boolean isTypeFound;

            isTypeFound = false;
            GetDataSourceInformation(true);
            foreach (String type in Enum.GetNames(typeof(DataSourceType)))
            {
                if (type == GetDataSourceInformation().Type.ToString())
                {
                    isTypeFound = true;
                    break;
                }
            }
            Assert.IsTrue(GetDataSourceInformation(true).Name.IsNotEmpty());
            Assert.IsTrue(isTypeFound);
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.Data.Test
{
    [TestClass]
    public class DataContextTest
    {
        DataContext _dataContext;

        public DataContextTest()
        {
            _dataContext = null;
        }

        [TestMethod]
        public void Constructor()
        {
            DataContext dataContext = null;

            dataContext = new DataContext(DataSourceInformationTest.GetOneDataSourceInformation(),
                                          LocaleTest.GetOneLocale());
            Assert.IsNotNull(dataContext);
        }

        [TestMethod]
        public void DataSource()
        {
            IDataSourceInformation dataSource;

            dataSource = DataSourceInformationTest.GetOneDataSourceInformation();
            GetDataContext(true).DataSource = dataSource;
            Assert.AreEqual(dataSource, GetDataContext().DataSource);
        }

        private DataContext GetDataContext()
        {
            return GetDataContext(false);
        }

        private DataContext GetDataContext(Boolean refresh)
        {
            if (_dataContext.IsNull() || refresh)
            {
                _dataContext = new DataContext(DataSourceInformationTest.GetOneDataSourceInformation(),
                                               LocaleTest.GetOneLocale());
            }
            return _dataContext;
        }

        public static DataContext GetOneDataContext()
        {
            return new DataContext(DataSourceInformationTest.GetOneDataSourceInformation(),
                                   LocaleTest.GetOneLocale());
        }

        [TestMethod]
        public void IsChanged()
        {
            Boolean isChanged;

            isChanged = false;
            GetDataContext(true).IsChanged = isChanged;
            Assert.AreEqual(isChanged, GetDataContext().IsChanged);
            isChanged = true;
            GetDataContext().IsChanged = isChanged;
            Assert.AreEqual(isChanged, GetDataContext().IsChanged);
        }

        [TestMethod]
        public void Locale()
        {
            Assert.IsTrue(GetDataContext(true).Locale.IsNotNull());
        }
    }
}

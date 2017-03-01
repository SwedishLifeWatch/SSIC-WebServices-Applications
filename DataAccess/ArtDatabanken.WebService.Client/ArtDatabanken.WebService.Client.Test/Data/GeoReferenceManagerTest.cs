using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Client.GeoReferenceService;
using ArtDatabanken.WebService.Client.UserService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class GeoReferenceManagerTest: TestBase
    {
        private GeoReferenceManager _geoReferenceManager;

        public GeoReferenceManagerTest()
        {
            _geoReferenceManager = null;
        }

        [TestMethod]
        public void Constructor()
        {
            GeoReferenceManager geoReferenceManager;

            geoReferenceManager = new GeoReferenceManager();
            Assert.IsNotNull(geoReferenceManager);
        }

        [TestMethod]
        public void DataSource()
        {
            IGeoReferenceDataSource dataSource;

            dataSource = null;
            GetGeoReferenceManager(true).DataSource = dataSource;
            Assert.AreEqual(dataSource, GetGeoReferenceManager().DataSource);

            dataSource = new GeoReferenceDataSource();
            GetGeoReferenceManager(true).DataSource = dataSource;
            Assert.AreEqual(dataSource, GetGeoReferenceManager().DataSource);
        }

        private GeoReferenceManager GetGeoReferenceManager()
        {
            return GetGeoReferenceManager(false);
        }

        private GeoReferenceManager GetGeoReferenceManager(Boolean refresh)
        {
            if (_geoReferenceManager.IsNull() || refresh)
            {
                _geoReferenceManager = new GeoReferenceManager();
                _geoReferenceManager.DataSource = new GeoReferenceDataSource();
            }
            return _geoReferenceManager;
        }

        [TestMethod]
        public void GetCitiesByNameSearchString()
        {
            ICoordinateSystem coordinateSystemRt90 = new CoordinateSystem(CoordinateSystemId.Rt90_25_gon_v);
            ICoordinateSystem coordinateSystemGoogleMercator = new CoordinateSystem(CoordinateSystemId.GoogleMercator);
            IStringSearchCriteria criteria = new StringSearchCriteria() { SearchString = "Uppsala%" };

            // Get cities in RT90
            CityInformationList citiesRt90 =
                GetGeoReferenceManager(true).GetCitiesByNameSearchString(GetUserContext(),
                    criteria,
                    coordinateSystemRt90);
            Assert.IsTrue(citiesRt90.IsNotEmpty());


            // Get cities in Google Mercator
            CityInformationList citiesGoogleMercator =
                GetGeoReferenceManager(true).GetCitiesByNameSearchString(GetUserContext(),
                    criteria,
                    coordinateSystemGoogleMercator);
            Assert.IsTrue(citiesGoogleMercator.IsNotEmpty());
        }
    }
}

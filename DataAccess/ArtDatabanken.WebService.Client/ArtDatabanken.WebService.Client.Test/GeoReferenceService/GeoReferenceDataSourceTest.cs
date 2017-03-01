using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.GeoReferenceService;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.GeoReferenceService
{
    [TestClass]
    public class GeoReferenceDataSourceTest: TestBase
    {
        private GeoReferenceDataSource _geoReferenceDataSource;

        public GeoReferenceDataSourceTest()
        {
            _geoReferenceDataSource = null;
        }

        [TestMethod]
        public void Constructor()
        {
            GeoReferenceDataSource geoReferenceDataSource = new GeoReferenceDataSource();
            Assert.IsNotNull(geoReferenceDataSource);
        }

        private GeoReferenceDataSource GetRegionDataSource()
        {
            return GetGeoReferenceDataSource(false);
        }

        private GeoReferenceDataSource GetGeoReferenceDataSource(Boolean refresh)
        {
            if (_geoReferenceDataSource.IsNull() || refresh)
            {
                _geoReferenceDataSource = new GeoReferenceDataSource();
            }
            return _geoReferenceDataSource;
        }

        [TestMethod]
        public void GetCitiesByNameSearchString()
        {
            ICoordinateSystem coordinateSystemRt90 = new CoordinateSystem(CoordinateSystemId.Rt90_25_gon_v);
            ICoordinateSystem coordinateSystemGoogleMercator = new CoordinateSystem(CoordinateSystemId.GoogleMercator);
            ICoordinateSystem coordinateSystemSweref99TM = new CoordinateSystem(CoordinateSystemId.SWEREF99_TM);
            IStringSearchCriteria criteria = new StringSearchCriteria() {SearchString = "Uppsala%"};
            
            // Get cities in RT90
            CityInformationList citiesRt90 =
                GetGeoReferenceDataSource(true).GetCitiesByNameSearchString(GetUserContext(),
                    criteria,
                    coordinateSystemRt90);
            Assert.IsTrue(citiesRt90.IsNotEmpty());


            // Get cities in Google Mercator
            CityInformationList citiesGoogleMercator =
                GetGeoReferenceDataSource(true).GetCitiesByNameSearchString(GetUserContext(),
                    criteria,
                    coordinateSystemGoogleMercator);
            Assert.IsTrue(citiesGoogleMercator.IsNotEmpty());

            // Get cities in Sweref99TM
            CityInformationList citiesSweref99Tm =
                GetGeoReferenceDataSource(true).GetCitiesByNameSearchString(GetUserContext(),
                    criteria,
                    coordinateSystemSweref99TM);
            Assert.IsTrue(citiesSweref99Tm.IsNotEmpty());
        }
    }
}

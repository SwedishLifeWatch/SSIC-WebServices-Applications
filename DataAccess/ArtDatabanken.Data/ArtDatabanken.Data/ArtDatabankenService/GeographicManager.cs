using System;
using System.Collections.Generic;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class contains handling of geographic related information.
    /// </summary>
    public class GeographicManager : ManagerBase
    {
        private static CountyList _counties = null;
        private static ProvinceList _provinces = null;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static GeographicManager()
        {
            ManagerBase.RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// Makes access to the private member _counties thread safe.
        /// </summary>
        private static CountyList Counties
        {
            get
            {
                CountyList counties;

                lock (_lockObject)
                {
                    counties = _counties;
                }
                return counties;
            }
            set
            {
                lock (_lockObject)
                {
                    _counties = value;
                }
            }
        }

        /// <summary>
        /// Makes access to the private member _provinces thread safe.
        /// </summary>
        private static ProvinceList Provinces
        {
            get
            {
                ProvinceList provinces;

                lock (_lockObject)
                {
                    provinces = _provinces;
                }
                return provinces;
            }
            set
            {
                lock (_lockObject)
                {
                    _provinces = value;
                }
            }
        }

        /// <summary>
        /// Get information about cities that matches the search string.
        /// </summary>
        /// <param name="searchString">String that city name must match.</param>
        /// <returns>Information about cities.</returns>
        public static CityList GetCitiesBySearchString(String searchString)
        {
            CityList cities;

            cities = new CityList();
            foreach (WebCity webCity in WebServiceClient.GetCitiesBySearchString(searchString))
            {
                cities.Add(new City(webCity.Name,
                                    webCity.County,
                                    webCity.Municipality,
                                    webCity.Parish,
                                    webCity.XCoordinate,
                                    webCity.YCoordinate));
            }
            return cities;
        }

        /// <summary>
        /// Get all county information objects.
        /// </summary>
        /// <returns>All county information objects.</returns>
        public static CountyList GetCounties()
        {
            CountyList counties = null;

            for (Int32 getAttempts = 0; (counties.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadCounties();
                counties = Counties;
            }
            return counties;
        }

        /// <summary>
        /// Convert a CountyList to a WebCounty array.
        /// </summary>
        /// <param name="counties">The CountyList.</param>
        /// <returns>The WebCounty array.</returns>
        internal static List<WebCounty> GetCounties(CountyList counties)
        {
            County county;
            Int32 countyIndex;
            WebCounty webCounty;
            List<WebCounty> webCounties;

            webCounties = null;
            if (counties.IsNotEmpty())
            {
                webCounties = new List<WebCounty>();
                for (countyIndex = 0; countyIndex < counties.Count; countyIndex++)
                {
                    county = counties[countyIndex];
                    webCounty = new WebCounty();
                    webCounty.Id = county.Id;
#if DATA_SPECIFIED_EXISTS
                    webCounty.IdSpecified = true;
#endif
                    webCounty.Name = county.Name;
                    webCounty.Identifier = county.Identifier;
                    webCounty.IsNumberSpecified = county.HasNumber;
#if DATA_SPECIFIED_EXISTS
                    webCounty.IsNumberSpecifiedSpecified = true;
#endif
                    webCounty.Number = county.Number;
                    webCounty.IsCountyPart = county.IsCountyPart;
#if DATA_SPECIFIED_EXISTS
                    webCounty.IsCountyPartSpecified = true;
#endif
                    webCounty.PartOfCountyId = county.PartOfCountyId;
#if DATA_SPECIFIED_EXISTS
                    webCounty.PartOfCountyIdSpecified = true;
#endif
                    webCounties.Add(webCounty);
                }
            }
            return webCounties;
        }

        /// <summary>
        /// Get the requested county object.
        /// </summary>
        /// <param name='countyId'>Id of requested county.</param>
        /// <returns>Requested county.</returns>
        /// <exception cref="ArgumentException">Thrown if no county has the requested id.</exception>
        public static County GetCounty(Int32 countyId)
        {
            return GetCounties().Get(countyId);
        }

        /// <summary>
        /// Get the requested county object.
        /// </summary>
        /// <param name='countyIdentifier'>Identifier for requested county.</param>
        /// <returns>Requested county.</returns>
        /// <exception cref="ArgumentException">Thrown if no county has the requested id.</exception>
        public static County GetCounty(String countyIdentifier)
        {
            foreach (County county in GetCounties())
            {
                if (countyIdentifier == county.Identifier)
                {
                    return county;
                }
            }
            throw new ApplicationException("No count found with identifier " + countyIdentifier);
        }

        /// <summary>
        /// Get the requested province object.
        /// </summary>
        /// <param name='provinceId'>Id of requested province.</param>
        /// <returns>Requested province.</returns>
        /// <exception cref="ArgumentException">Thrown if no province has the requested id.</exception>
        public static Province GetProvince(Int32 provinceId)
        {
            return GetProvinces().Get(provinceId);
        }

        /// <summary>
        /// Get all province information objects.
        /// </summary>
        /// <returns>All province information objects.</returns>
        public static ProvinceList GetProvinces()
        {
            ProvinceList provinces = null;

            for (Int32 getAttempts = 0; (provinces.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadProvinces();
                provinces = Provinces;
            }
            return provinces;
        }

        /// <summary>
        /// Convert a ProvinceList to a WebProvince array.
        /// </summary>
        /// <param name="provinces">The ProvinceList.</param>
        /// <returns>The WebProvince array.</returns>
        internal static List<WebProvince> GetProvinces(ProvinceList provinces)
        {
            Province province;
            Int32 provinceIndex;
            WebProvince webProvince;
            List<WebProvince> webProvinces;

            webProvinces = null;
            if (provinces.IsNotEmpty())
            {
                webProvinces = new List<WebProvince>();
                for (provinceIndex = 0; provinceIndex < provinces.Count; provinceIndex++)
                {
                    province = provinces[provinceIndex];
                    webProvince = new WebProvince();
                    webProvince.Id = province.Id;
#if DATA_SPECIFIED_EXISTS
                    webProvince.IdSpecified = true;
#endif
                    webProvince.Name = province.Name;
                    webProvince.Identifier = province.Identifier;
                    webProvince.IsProvincePart = province.IsProvincePart;
#if DATA_SPECIFIED_EXISTS
                    webProvince.IsProvincePartSpecified = true;
#endif
                    webProvince.PartOfProvinceId = province.PartOfProvinceId;
#if DATA_SPECIFIED_EXISTS
                    webProvince.PartOfProvinceIdSpecified = true;
#endif
                    webProvinces.Add(webProvince);
                }
            }
            return webProvinces;
        }

        /// <summary>
        /// Get counties from web service.
        /// </summary>
        private static void LoadCounties()
        {
            CountyList counties;

            if (Counties.IsNull())
            {
                // Get data from web service.
                counties = new CountyList();
                foreach (WebCounty webCounty in WebServiceClient.GetCounties())
                {
                    counties.Add(new County(webCounty.Id,
                                            webCounty.Name,
                                            webCounty.Identifier,
                                            webCounty.IsNumberSpecified,
                                            webCounty.Number,
                                            webCounty.IsCountyPart,
                                            webCounty.PartOfCountyId));
                }
                Counties = counties;
            }
        }

        /// <summary>
        /// Get provinces from web service.
        /// </summary>
        private static void LoadProvinces()
        {
            ProvinceList provinces;

            if (Provinces.IsNull())
            {
                // Get data from web service.
                provinces = new ProvinceList();
                foreach (WebProvince webProvince in WebServiceClient.GetProvinces())
                {
                    provinces.Add(new Province(webProvince.Id,
                                               webProvince.Name,
                                               webProvince.Identifier,
                                               webProvince.IsProvincePart,
                                               webProvince.PartOfProvinceId));
                }
                Provinces = provinces;
            }
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        private static void RefreshCache()
        {
            Counties = null;
            Provinces = null;
        }
    }
}

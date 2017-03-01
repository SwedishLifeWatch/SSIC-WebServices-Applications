using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Client.UserService;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Client.GeoReferenceService
{
    /// <summary>
    /// This class is used to retrieve geo reference related information.
    /// </summary>
    public class GeoReferenceDataSource : GeoReferenceDataSourceBase, IGeoReferenceDataSource
    {
        /// <summary>
        /// Get cities by name string search
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="criteria">City search criteria</param>
        /// <param name="coordinateSystem">The coordinate system used in the returned WebCityInformations</param>
        /// <returns></returns>
        public CityInformationList GetCitiesByNameSearchString(IUserContext userContext,
                                                               IStringSearchCriteria criteria,
                                                               ICoordinateSystem coordinateSystem)
        {
            WebStringSearchCriteria webStringSearchCriteria = GetStringSearchCriteria(userContext, criteria);
            WebCoordinateSystem webCoordinateSystem = GetCoordinateSystem(coordinateSystem);

            List<WebCityInformation> cityInformations = WebServiceProxy.GeoReferenceService.GetCitiesByNameSearchString(GetClientInformation(userContext),
                webStringSearchCriteria,
                webCoordinateSystem);
            return GetCityInformationList(userContext, cityInformations);
        }

        /// <summary>
        /// Get a CityInformationList from a List of WebCityInformation
        /// </summary>
        /// <param name="userContext">The user context</param>
        /// <param name="cityInformations">The list of WebCityInformations to "convert"</param>
        /// <returns></returns>
        private CityInformationList GetCityInformationList(IUserContext userContext, List<WebCityInformation> cityInformations)
        {
            CityInformationList cityInformationList = new CityInformationList();

            foreach (WebCityInformation cityInformation in cityInformations)
            {
                cityInformationList.Add(GetCityInformation(userContext, cityInformation));
            }
            return cityInformationList;
        }

        /// <summary>
        /// Get an ICityInformation from a WebCityInformation
        /// </summary>
        /// <param name="userContext">The user context</param>
        /// <param name="cityInformation">The WebCityInformation</param>
        /// <returns>The ICityInformation representation of the WebCityInformation</returns>
        private ICityInformation GetCityInformation(IUserContext userContext, WebCityInformation cityInformation)
        {
            return new CityInformation()
            {
                Name = cityInformation.Name,
                County = cityInformation.County,
                Parish = cityInformation.Parish,
                Municipality = cityInformation.Municipality,
                Coordinate = new Point(cityInformation.CoordinateX, cityInformation.CoordinateY),
                DataContext = GetDataContext(userContext)
            };
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="applicationIdentifier">
        /// Application identifier.
        /// User authorities for this application is included in
        /// the user context.
        /// </param>
        /// <param name="isActivationRequired">
        /// Flag that indicates if user must be activated
        /// for login to succed.
        /// </param>
        /// <returns>User context or null if login failed.</returns>
        public void Login(IUserContext userContext,
                          String userName,
                          String password,
                          String applicationIdentifier,
                          Boolean isActivationRequired)
        {
            WebLoginResponse loginResponse;

            loginResponse = WebServiceProxy.GeoReferenceService.Login(userName,
                                                                      password,
                                                                      applicationIdentifier,
                                                                      isActivationRequired);
            if (loginResponse.IsNotNull())
            {
                SetToken(userContext, loginResponse.Token);
            }
        }

        /// <summary>
        /// Logout user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public void Logout(IUserContext userContext)
        {
            WebServiceProxy.GeoReferenceService.Logout(GetClientInformation(userContext));
            SetToken(userContext, null);
        }

        /// <summary>
        /// Set GeoReferenceService as data source in the onion data model.
        /// </summary>
        public static void SetDataSource()
        {
            GeoReferenceDataSource geoReferenceDataSource;

            geoReferenceDataSource = new GeoReferenceDataSource();
            CoreData.GeoReferenceManager.DataSource = geoReferenceDataSource;
            CoreData.RegionManager.DataSource = new RegionDataSource();
            ((UserDataSource)(CoreData.UserManager.DataSource)).UserLoggedInEvent += new UserSOALoggedInEventHandler(geoReferenceDataSource.Login);
            ((UserDataSource)(CoreData.UserManager.DataSource)).UserLoggedOutEvent += new UserSOALoggedOutEventHandler(geoReferenceDataSource.Logout);
        }

        /// <summary>
        /// Get WebStringSearchCriteria from specified IStringSearchCriteria
        /// </summary>
        /// <param name="userContext">The user context</param>
        /// <param name="stringSearchCriteria">An IStringSearchCriteria</param>
        /// <returns></returns>
        private WebStringSearchCriteria GetStringSearchCriteria(IUserContext userContext,
                                                         IStringSearchCriteria stringSearchCriteria)
        {
            WebStringSearchCriteria webStringSearchCriteria = new WebStringSearchCriteria();
            webStringSearchCriteria.CompareOperators = stringSearchCriteria.CompareOperators;
            webStringSearchCriteria.SearchString = stringSearchCriteria.SearchString;
            return webStringSearchCriteria;
        }
    }
}
